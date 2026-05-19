#!/usr/bin/env python3
"""Generate painted background layers through PixelLab v2 REST endpoints."""

from __future__ import annotations

import argparse
import base64
import json
import os
import re
import struct
import sys
import time
import urllib.error
import urllib.request
from dataclasses import dataclass, field
from io import BytesIO
from pathlib import Path
from typing import Any

try:
    import tomllib
except ModuleNotFoundError:  # pragma: no cover - Python < 3.11 fallback.
    tomllib = None


API_BASE = "https://api.pixellab.ai/v2"
POLL_TIMEOUT_SECONDS = 1500
MAX_POLL_SECONDS = POLL_TIMEOUT_SECONDS
POLL_INTERVAL_SECONDS = 20
ABNORMAL_GENERATION_COST = 10
REPORT_PATH = Path("CODEX_DONE_spawn01_layers_1_to_4.md")


class PixelLabError(RuntimeError):
    pass


def truncate_text(text: str, limit: int = 800) -> str:
    if len(text) <= limit:
        return text
    return f"{text[:limit]}... [truncated {len(text) - limit} chars]"


@dataclass
class LayerResult:
    index: int
    name: str
    endpoint: str
    size: str
    job_id: str = ""
    status: str = "not_started"
    cost_usd: str = ""
    png_path: str = ""
    error: str = ""
    file_size: str = ""
    dimensions: str = ""


@dataclass
class RunSummary:
    status: str = "FAILED"
    layers: list[LayerResult] = field(default_factory=list)
    total_cost_usd: str = "unknown"
    remaining_balance: str = "unknown"
    script_path: str = "STAGING/scripts/generate_painted_layers.py"
    wall_time_minutes: float = 0.0


def read_json(path: Path) -> dict[str, Any]:
    return json.loads(path.read_text(encoding="utf-8"))


def unwrap_response(payload: Any) -> Any:
    if isinstance(payload, dict) and "success" in payload and "data" in payload:
        return payload["data"]
    return payload


def read_auth_header() -> str:
    env_header = os.environ.get("PIXELLAB_AUTH_HEADER")
    if env_header:
        return env_header

    env_token = os.environ.get("PIXELLAB_API_TOKEN")
    if env_token:
        return env_token if env_token.lower().startswith("bearer ") else f"Bearer {env_token}"

    config_path = Path.home() / ".codex" / "config.toml"
    if config_path.exists() and tomllib is not None:
        with config_path.open("rb") as fh:
            data = tomllib.load(fh)
        header = (
            data.get("mcp_servers", {})
            .get("pixellab", {})
            .get("env", {})
            .get("AUTH_HEADER")
        )
        if header:
            return str(header)

    if config_path.exists():
        text = config_path.read_text(encoding="utf-8", errors="replace")
        match = re.search(r'AUTH_HEADER\s*=\s*["\']([^"\']+)["\']', text)
        if match:
            return match.group(1)

    raise PixelLabError("PixelLab auth header not found in env or ~/.codex/config.toml")


def request_json(
    method: str,
    path: str,
    auth_header: str,
    payload: dict[str, Any] | None = None,
    timeout: int = 60,
) -> dict[str, Any]:
    body = None
    headers = {
        "Authorization": auth_header,
        "Accept": "application/json",
    }
    if payload is not None:
        body = json.dumps(payload).encode("utf-8")
        headers["Content-Type"] = "application/json"

    req = urllib.request.Request(
        f"{API_BASE}{path}",
        data=body,
        headers=headers,
        method=method,
    )
    try:
        with urllib.request.urlopen(req, timeout=timeout) as resp:
            text = resp.read().decode("utf-8")
            return json.loads(text) if text else {}
    except urllib.error.HTTPError as exc:
        detail = exc.read().decode("utf-8", errors="replace")
        detail = truncate_text(detail)
        if exc.code == 402:
            raise PixelLabError(f"HTTP 402 Insufficient credits: {detail}")
        if exc.code == 429:
            raise PixelLabError(f"HTTP 429 Too many concurrent jobs/requests: {detail}")
        raise PixelLabError(f"HTTP {exc.code} {exc.reason}: {detail}") from exc
    except urllib.error.URLError as exc:
        raise PixelLabError(f"Network error: {exc}") from exc


def get_balance(auth_header: str) -> dict[str, Any] | None:
    try:
        return unwrap_response(request_json("GET", "/balance", auth_header))
    except PixelLabError as exc:
        print(f"[WARN] balance check failed: {exc}", flush=True)
        return None


def balance_generations(balance: dict[str, Any] | None) -> float | None:
    if not isinstance(balance, dict):
        return None
    subscription = balance.get("subscription")
    if isinstance(subscription, dict):
        value = subscription.get("generations")
        if isinstance(value, (int, float)):
            return float(value)
    for key in ("remaining_generations", "generations"):
        value = balance.get(key)
        if isinstance(value, (int, float)):
            return float(value)
    return None


def balance_label(balance: dict[str, Any] | None) -> str:
    if not isinstance(balance, dict):
        return "unknown"
    subscription = balance.get("subscription")
    credits = balance.get("credits")
    credits_label = None
    if isinstance(credits, dict):
        amount = credits.get("usd") or credits.get("amount") or credits.get("balance")
        if isinstance(amount, (int, float)):
            credits_label = f"credits ${amount:g}"
    elif isinstance(credits, (int, float)):
        credits_label = f"credits ${credits:g}"

    if isinstance(subscription, dict):
        gens = subscription.get("generations")
        total = subscription.get("total")
        if gens is not None and total is not None:
            label = f"subscription {gens:g}/{total:g}"
            return f"{label} + {credits_label}" if credits_label else label
        if gens is not None:
            label = f"subscription {gens:g}"
            return f"{label} + {credits_label}" if credits_label else label
    if credits_label:
        return credits_label
    return json.dumps(balance, ensure_ascii=True)


def usage_label(usage: Any, before_balance: dict[str, Any] | None, after_balance: dict[str, Any] | None) -> str:
    before = balance_generations(before_balance)
    after = balance_generations(after_balance)
    if before is not None and after is not None:
        used = before - after
        if used >= 0:
            return f"{used:g} generations"
    if isinstance(usage, dict):
        for key in ("generations_used", "remaining_generations", "credits_used", "credits"):
            value = usage.get(key)
            if isinstance(value, (int, float)):
                return f"{value:g} {key}"
        if isinstance(usage.get("usd"), (int, float)):
            return f"${usage['usd']:g}"
        return json.dumps(usage, ensure_ascii=True)
    return "unknown"


def usage_numeric_for_abort(label: str, usage: Any) -> float | None:
    if isinstance(usage, dict):
        for key in ("generations_used", "credits_used", "credits", "generations"):
            value = usage.get(key)
            if isinstance(value, (int, float)):
                return float(value)
    match = re.match(r"^([0-9]+(?:\.[0-9]+)?) generations$", label)
    if match:
        return float(match.group(1))
    return None


def post_layer(layer: dict[str, Any], auth_header: str, style_reference_path: Path | None) -> tuple[str, Any]:
    endpoint = layer["endpoint"]
    payload: dict[str, Any] = {
        "description": build_description(layer),
        "image_size": {"width": int(layer["width"]), "height": int(layer["height"])},
        "no_background": bool(layer.get("no_background", True)),
    }
    if layer.get("seed") is not None:
        payload["seed"] = int(layer["seed"])

    if endpoint == "/generate-with-style-v2":
        if not style_reference_path:
            raise PixelLabError("Config missing style_reference_path for /generate-with-style-v2")
        payload["style_images"] = [build_style_image(style_reference_path)]
        payload["style_description"] = layer.get(
            "style_description",
            "Match the provided floor image palette, brushwork, shading direction, and pixel art finish.",
        )
    elif endpoint != "/generate-image-v2":
        raise PixelLabError(f"Unsupported endpoint: {endpoint}")

    response = unwrap_response(request_json("POST", endpoint, auth_header, payload, timeout=90))
    if not isinstance(response, dict):
        raise PixelLabError(f"Unexpected POST response: {response!r}")
    job_id = response.get("background_job_id") or response.get("job_id") or response.get("id")
    if not job_id:
        raise PixelLabError(f"POST response did not include a job id: {response!r}")
    return str(job_id), response.get("usage")


def build_description(layer: dict[str, Any]) -> str:
    description = str(layer["description"]).strip()
    negative = str(layer.get("negative_description", "")).strip()
    if negative:
        description = f"{description}\n\nAvoid: {negative}"
    return description


def build_style_image(path: Path) -> dict[str, Any]:
    png_bytes, width, height = load_style_reference_png(path)
    encoded = base64.b64encode(png_bytes).decode("ascii")
    return {
        "image": {"base64": f"data:image/png;base64,{encoded}"},
        "width": width,
        "height": height,
    }


def load_style_reference_png(path: Path) -> tuple[bytes, int, int]:
    width, height = png_dimensions(path)
    data = path.read_bytes()
    if width <= 512 and height <= 512:
        return data, width, height

    try:
        from PIL import Image
    except Exception as exc:
        raise PixelLabError(
            f"Style reference {path} is {width}x{height}; Pillow is required to downscale to the API 512px cap."
        ) from exc

    with Image.open(path) as img:
        img = img.convert("RGBA")
        scale = min(512 / img.width, 512 / img.height)
        new_size = (max(1, round(img.width * scale)), max(1, round(img.height * scale)))
        img = img.resize(new_size, Image.Resampling.LANCZOS)
        buffer = BytesIO()
        img.save(buffer, format="PNG")
    return buffer.getvalue(), new_size[0], new_size[1]


def poll_job(job_id: str, auth_header: str) -> dict[str, Any]:
    start = time.monotonic()
    while True:
        response = unwrap_response(request_json("GET", f"/background-jobs/{job_id}", auth_header, timeout=60))
        if not isinstance(response, dict):
            raise PixelLabError(f"Unexpected poll response for {job_id}: {response!r}")
        status = str(response.get("status", "")).lower()
        print(f"[POLL] {job_id} status={status or 'unknown'}", flush=True)
        if status == "completed":
            return response
        if status in {"failed", "error", "cancelled", "canceled"}:
            raise PixelLabError(f"Job {job_id} failed: {response.get('last_response') or response}")
        if time.monotonic() - start > POLL_TIMEOUT_SECONDS:
            raise PixelLabError(f"Job {job_id} timed out after {POLL_TIMEOUT_SECONDS}s")
        time.sleep(POLL_INTERVAL_SECONDS)


def extract_first_image_bytes(data: Any) -> bytes:
    base64_candidates: list[str] = []
    url_candidates: list[str] = []

    def visit(node: Any, key_hint: str = "") -> None:
        if isinstance(node, dict):
            if isinstance(node.get("base64"), str):
                base64_candidates.append(node["base64"])
            for key, value in node.items():
                hint = str(key).lower()
                if isinstance(value, str):
                    if value.startswith("data:image/"):
                        base64_candidates.append(value)
                    elif hint in {"url", "image_url", "file_url", "storage_url", "download_url"} and value.startswith("http"):
                        url_candidates.append(value)
                else:
                    visit(value, hint)
        elif isinstance(node, list):
            for item in node:
                visit(item, key_hint)

    visit(data)
    if base64_candidates:
        text = base64_candidates[0]
        if "," in text and text.startswith("data:image/"):
            text = text.split(",", 1)[1]
        return base64.b64decode(text)
    if url_candidates:
        req = urllib.request.Request(url_candidates[0], headers={"User-Agent": "RIMA-Codex/1.0"})
        with urllib.request.urlopen(req, timeout=90) as resp:
            return resp.read()
    raise PixelLabError(f"No image base64 or URL found in completed response: {data!r}")


def png_dimensions(path: Path) -> tuple[int, int]:
    with path.open("rb") as fh:
        header = fh.read(24)
    if len(header) < 24 or header[:8] != b"\x89PNG\r\n\x1a\n":
        raise PixelLabError(f"Not a PNG file: {path}")
    return struct.unpack(">II", header[16:24])


def file_size_label(path: Path) -> str:
    size = path.stat().st_size
    if size >= 1024 * 1024:
        return f"{size / (1024 * 1024):.1f} MB"
    return f"{size / 1024:.1f} KB"


def write_report(summary: RunSummary) -> None:
    rows = []
    for result in summary.layers:
        rows.append(
            "| {index} | {name} | {job_id} | {status} | {cost_usd} | {png_path} | {size} |".format(
                index=result.index,
                name=result.name,
                job_id=result.job_id or "-",
                status=result.status,
                cost_usd=result.cost_usd or "-",
                png_path=result.png_path or "-",
                size=result.dimensions or result.size,
            )
        )

    previews = []
    for result in summary.layers:
        if result.png_path:
            previews.append(f"{result.name}: {result.file_size}, {result.dimensions} PNG")
        elif result.error:
            previews.append(f"{result.name}: FAILED - {truncate_text(result.error, 300)}")

    content = "\n".join(
        [
            "# Spawn_01 Layers 1-4 - REPORT",
            "",
            f"## Status: {summary.status}",
            "",
            "## Per-layer",
            "| # | Name | Job ID | Status | Cost USD | PNG path | Size |",
            "|---|---|---|---|---|---|---|",
            *rows,
            "",
            f"## Total cost USD: {summary.total_cost_usd}",
            f"## Remaining balance: {summary.remaining_balance}",
            f"## Script path: {summary.script_path} (reusable for future bg)",
            f"## Wall time: {summary.wall_time_minutes:.2f} min",
            "",
            "## Sample preview",
            *previews,
            "",
        ]
    )
    REPORT_PATH.write_text(content, encoding="utf-8")


def main() -> int:
    parser = argparse.ArgumentParser(description="Generate PixelLab painted layer PNGs from a JSON config.")
    parser.add_argument("config", type=Path)
    args = parser.parse_args()

    start_time = time.monotonic()
    summary = RunSummary()

    try:
        config = read_json(args.config)
        output_dir = Path(config["output_dir"])
        output_dir.mkdir(parents=True, exist_ok=True)
        style_reference_path = Path(config["style_reference_path"]) if config.get("style_reference_path") else None
        if style_reference_path:
            png_dimensions(style_reference_path)
        auth_header = read_auth_header()

        initial_balance = get_balance(auth_header)
        previous_balance = initial_balance
        total_cost = 0.0

        for layer in sorted(config["layers"], key=lambda item: int(item["index"])):
            result = LayerResult(
                index=int(layer["index"]),
                name=str(layer["name"]),
                endpoint=str(layer["endpoint"]),
                size=f"{int(layer['width'])}x{int(layer['height'])}",
            )
            summary.layers.append(result)
            out_path = output_dir / f"layer_{result.index:02d}_{result.name}.png"

            try:
                print(f"[POST] layer {result.index} {result.name} {result.endpoint}", flush=True)
                before_balance = previous_balance or get_balance(auth_header)
                job_id, post_usage = post_layer(layer, auth_header, style_reference_path)
                result.job_id = job_id
                result.status = "processing"
                print(f"[JOB] layer {result.index} job_id={job_id}", flush=True)

                job_response = poll_job(job_id, auth_header)
                png_bytes = extract_first_image_bytes(job_response.get("last_response") or job_response)
                out_path.write_bytes(png_bytes)
                width, height = png_dimensions(out_path)
                result.png_path = out_path.as_posix()
                result.file_size = file_size_label(out_path)
                result.dimensions = f"{width}x{height}"
                result.status = str(job_response.get("status", "completed"))

                after_balance = get_balance(auth_header)
                previous_balance = after_balance
                last_response = job_response.get("last_response")
                usage = last_response if isinstance(last_response, dict) else job_response.get("usage") or post_usage
                cost = usage.get("usage_cost_usd") if isinstance(usage, dict) else None
                if isinstance(cost, (int, float)):
                    result.cost_usd = f"{cost:g}"
                    total_cost += float(cost)
                else:
                    result.cost_usd = "unknown"

                numeric_cost = float(cost) if isinstance(cost, (int, float)) else usage_numeric_for_abort(usage_label(usage, before_balance, after_balance), usage)
                if numeric_cost is not None and numeric_cost > ABNORMAL_GENERATION_COST:
                    raise PixelLabError(f"Abnormal usage for layer {result.index}: ${numeric_cost:g}")

                print(f"[DONE] layer {result.index} -> {out_path} ({result.file_size}, {result.dimensions}) cost_usd={result.cost_usd}", flush=True)
            except Exception as exc:
                result.status = "failed"
                result.error = str(exc)
                print(f"[FAIL] layer {result.index}: {exc}", flush=True)
                raise

        final_balance = previous_balance or get_balance(auth_header)
        summary.total_cost_usd = f"{total_cost:g}" if total_cost > 0 else "unknown"
        summary.remaining_balance = balance_label(final_balance)
        summary.status = "SUCCESS"
        return 0
    except Exception as exc:
        if summary.layers and all(layer.status != "completed" for layer in summary.layers):
            summary.status = "FAILED"
        elif summary.layers:
            summary.status = "PARTIAL"
        print(f"[ERROR] {exc}", flush=True)
        return 1
    finally:
        summary.wall_time_minutes = (time.monotonic() - start_time) / 60
        if summary.remaining_balance == "unknown":
            try:
                summary.remaining_balance = balance_label(get_balance(read_auth_header()))
            except Exception:
                pass
        write_report(summary)
        print(f"[REPORT] {REPORT_PATH}", flush=True)


if __name__ == "__main__":
    sys.exit(main())
