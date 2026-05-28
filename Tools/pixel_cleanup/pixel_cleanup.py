#!/usr/bin/env python3
"""Local pixel art cleanup and artifact reporting tool."""

from __future__ import annotations

import argparse
import json
import sys
from dataclasses import dataclass
from pathlib import Path
from typing import Any

import numpy as np
from PIL import Image, ImageDraw


RGB = tuple[int, int, int]
MaskMap = dict[str, np.ndarray]

MASK_COLORS: dict[str, tuple[int, int, int, int]] = {
    "stray": (255, 0, 0, 255),
    "palette_outliers": (255, 0, 255, 255),
    "alpha": (255, 220, 0, 255),
    "color_noise": (0, 255, 255, 255),
}

MASK_FILENAMES: dict[str, str] = {
    "stray": "mask_stray.png",
    "palette_outliers": "mask_palette_outliers.png",
    "alpha": "mask_alpha.png",
    "color_noise": "mask_color_noise.png",
}


@dataclass(frozen=True)
class Component:
    pixels: tuple[tuple[int, int], ...]
    area: int
    bbox: tuple[int, int, int, int]


def load_image(path: str | Path) -> np.ndarray:
    image_path = Path(path)
    if not image_path.exists():
        raise FileNotFoundError(f"Input image not found: {image_path}")
    with Image.open(image_path) as img:
        return np.array(img.convert("RGBA"), dtype=np.uint8)


def detect_alpha_issues(arr: np.ndarray, threshold: int) -> np.ndarray:
    if threshold < 0 or threshold > 255:
        raise ValueError("--alpha_threshold must be between 0 and 255")
    alpha = arr[..., 3]
    return (alpha > 0) & (alpha < 255)


def detect_stray_components(alpha_mask: np.ndarray, min_area: int) -> list[Component]:
    if min_area < 1:
        raise ValueError("--min_component_area must be at least 1")
    components = _connected_components(alpha_mask.astype(bool))
    if not components:
        return []
    main_component = max(components, key=lambda item: item.area)
    return [
        component
        for component in components
        if component is not main_component and component.area < min_area
    ]


def detect_palette_outliers(
    rgb_arr: np.ndarray, palette: list[RGB], tolerance: int
) -> np.ndarray:
    if tolerance < 0:
        raise ValueError("--palette_tolerance must be non-negative")
    shape = rgb_arr.shape[:2]
    if not palette:
        return np.zeros(shape, dtype=bool)

    rgb = rgb_arr[..., :3]
    opaque_mask = _opaque_mask(rgb_arr)
    outlier_mask = np.zeros(shape, dtype=bool)
    if not np.any(opaque_mask):
        return outlier_mask

    pixels = rgb[opaque_mask]
    distances, _ = _nearest_palette(pixels, palette)
    outlier_mask[opaque_mask] = distances > tolerance
    return outlier_mask


def detect_color_noise(rgb_arr: np.ndarray, threshold: int) -> np.ndarray:
    if threshold < 0:
        raise ValueError("--noise_threshold must be non-negative")
    rgb = rgb_arr[..., :3].astype(np.int32)
    opaque = _opaque_mask(rgb_arr)
    suspicious = np.zeros(rgb.shape[:2], dtype=bool)
    height, width = opaque.shape

    for y, x in np.argwhere(opaque):
        neighbors: list[np.ndarray] = []
        for dy in (-1, 0, 1):
            for dx in (-1, 0, 1):
                if dy == 0 and dx == 0:
                    continue
                ny = int(y) + dy
                nx = int(x) + dx
                if 0 <= ny < height and 0 <= nx < width and opaque[ny, nx]:
                    neighbors.append(rgb[ny, nx])
        if len(neighbors) < 5:
            continue
        neighbor_arr = np.array(neighbors, dtype=np.int32)
        distances = np.sqrt(np.sum((neighbor_arr - rgb[y, x]) ** 2, axis=1))
        if int(np.count_nonzero(distances > threshold)) >= 5:
            suspicious[y, x] = True

    noise_mask = np.zeros_like(suspicious)
    for component in _connected_components(suspicious):
        if component.area <= 3:
            _paint_component(noise_mask, component, True)
    return noise_mask


def generate_preview(orig: np.ndarray, masks: MaskMap) -> Image.Image:
    base = _rgba_on_checker(Image.fromarray(orig, mode="RGBA"))
    overlay = Image.fromarray(_overlay_masks(orig, masks), mode="RGBA")
    overlay = _rgba_on_checker(overlay)

    panels = [
        _labeled_panel(base, "original"),
        _labeled_panel(overlay, "overlay"),
        _labeled_panel(_mask_panel(masks["stray"], MASK_COLORS["stray"]), "stray"),
        _labeled_panel(
            _mask_panel(masks["palette_outliers"], MASK_COLORS["palette_outliers"]),
            "palette",
        ),
        _labeled_panel(_mask_panel(masks["alpha"], MASK_COLORS["alpha"]), "alpha"),
        _labeled_panel(
            _mask_panel(masks["color_noise"], MASK_COLORS["color_noise"]),
            "noise",
        ),
    ]

    width = sum(panel.width for panel in panels)
    height = max(panel.height for panel in panels)
    sheet = Image.new("RGBA", (width, height), (255, 255, 255, 255))
    x = 0
    for panel in panels:
        sheet.alpha_composite(panel, (x, 0))
        x += panel.width
    return sheet


def apply_cleanup(arr: np.ndarray, masks: MaskMap, flags: dict[str, Any]) -> np.ndarray:
    out = arr.copy()
    alpha_before = out[..., 3].copy()

    if not flags.get("keep_alpha", False):
        threshold = int(flags.get("alpha_threshold", 128))
        alpha = out[..., 3]
        semi = (alpha > 0) & (alpha < 255)
        alpha[semi & (alpha < threshold)] = 0
        alpha[semi & (alpha >= threshold)] = 255
        out[..., 3] = alpha

    if flags.get("remove_stray", False):
        out[masks["stray"], :3] = 0
        out[masks["stray"], 3] = 0

    palette = flags.get("palette") or []
    if flags.get("snap_to_palette", False) and palette:
        snap_mask = masks["palette_outliers"] & (out[..., 3] == 255)
        if np.any(snap_mask):
            _, nearest = _nearest_palette(out[..., :3][snap_mask], palette)
            out[..., :3][snap_mask] = nearest.astype(np.uint8)

    if flags.get("fix_color_noise", False):
        out = _fix_noise_with_median(out, masks["color_noise"])

    # Transparent RGB is normalized so removed pixels do not keep hidden color.
    removed = (alpha_before > 0) & (out[..., 3] == 0)
    out[removed, :3] = 0
    return out


def load_palette(path: str | Path) -> list[RGB]:
    palette_path = Path(path)
    if not palette_path.exists():
        raise FileNotFoundError(f"Palette file not found: {palette_path}")
    try:
        data = json.loads(palette_path.read_text(encoding="utf-8"))
    except json.JSONDecodeError as exc:
        raise ValueError(f"Palette JSON is invalid: {palette_path}") from exc
    colors = data.get("colors")
    if not isinstance(colors, list) or not colors:
        raise ValueError("Palette JSON must contain a non-empty 'colors' list")

    parsed: list[RGB] = []
    for index, color in enumerate(colors):
        if not isinstance(color, list) or len(color) != 3:
            raise ValueError(f"Palette color at index {index} must be [r, g, b]")
        rgb: list[int] = []
        for value in color:
            if not isinstance(value, int) or value < 0 or value > 255:
                raise ValueError(
                    f"Palette color at index {index} contains invalid channel {value!r}"
                )
            rgb.append(value)
        parsed.append((rgb[0], rgb[1], rgb[2]))
    return parsed


def make_palette_from_image(arr: np.ndarray, count: int) -> list[RGB]:
    if count < 1:
        raise ValueError("--make_palette must be at least 1")
    rgb = arr[..., :3]
    alpha = arr[..., 3]
    sample_mask = alpha > 0
    if not np.any(sample_mask):
        return []
    colors, hits = np.unique(rgb[sample_mask], axis=0, return_counts=True)
    order = np.argsort(-hits)
    selected = colors[order[:count]]
    return [tuple(int(channel) for channel in color) for color in selected]


def analyze_image(
    image_path: Path,
    palette: list[RGB],
    alpha_threshold: int,
    min_component_area: int,
    palette_tolerance: int,
    noise_threshold: int,
) -> tuple[np.ndarray, MaskMap, list[Component], dict[str, Any]]:
    arr = load_image(image_path)
    alpha_mask = arr[..., 3] > 0
    stray_components = detect_stray_components(alpha_mask, min_component_area)
    masks: MaskMap = {
        "alpha": detect_alpha_issues(arr, alpha_threshold),
        "stray": _components_to_mask(arr.shape[:2], stray_components),
        "palette_outliers": detect_palette_outliers(arr, palette, palette_tolerance),
        "color_noise": detect_color_noise(arr, noise_threshold),
    }

    report = _build_report(image_path, arr, masks, stray_components)
    return arr, masks, stray_components, report


def process_single(args: argparse.Namespace) -> dict[str, Any]:
    input_path = Path(args.input)
    output_path = Path(args.output) if args.output else None
    report_path = Path(args.report) if args.report else None
    preview_path = Path(args.preview) if args.preview else None
    _validate_output_path(input_path, output_path, args.force)

    arr = load_image(input_path)
    if args.region:
        report = _process_region_request(arr, args)
        if args.apply_cleanup:
            if output_path is None:
                raise ValueError("--output is required when --apply_cleanup is used")
            cleaned = _apply_region_suggestions(
                arr, report["suggestions"], _auto_threshold(args.auto_mode)
            )
            report["changed_pixel_count"] = _changed_pixel_count(arr, cleaned)
            _save_rgba(cleaned, output_path, args.force)
        if report_path is not None:
            _write_json(report_path, report)
        else:
            print(json.dumps(report, indent=2))
        return report

    palette = _palette_for_image(arr, input_path, args, report_path, preview_path, output_path)
    arr, masks, stray_components, report = analyze_image(
        input_path,
        palette,
        args.alpha_threshold,
        args.min_component_area,
        args.palette_tolerance,
        args.noise_threshold,
    )

    artifact_dir = _single_artifact_dir(input_path, report_path, preview_path, output_path)
    if artifact_dir is not None:
        _save_mask_exports(masks, artifact_dir, None)

    if preview_path is not None:
        _ensure_parent(preview_path)
        generate_preview(arr, masks).save(preview_path)

    if args.apply_cleanup:
        if output_path is None:
            raise ValueError("--output is required when --apply_cleanup is used")
        cleaned = apply_cleanup(arr, masks, _cleanup_flags(args, palette))
        if args.smart_merge:
            cleaned, smart_report = _apply_smart_merge(
                cleaned, args.cluster_size, args.auto_mode
            )
            report["smart_merge"] = smart_report
        report["removed_pixel_count"] = _removed_pixel_count(arr, cleaned)
        report["changed_pixel_count"] = _changed_pixel_count(arr, cleaned)
        _save_rgba(cleaned, output_path, args.force)
    elif args.smart_merge:
        _, smart_report = _apply_smart_merge(arr, args.cluster_size, args.auto_mode)
        report["smart_merge"] = smart_report

    report["recommended_actions"] = _recommended_actions(
        report, stray_components, args, palette
    )
    if report_path is not None:
        _write_json(report_path, report)
    else:
        print(json.dumps(report, indent=2))
    return report


def process_batch(args: argparse.Namespace) -> list[dict[str, Any]]:
    input_dir = Path(args.input_dir)
    output_dir = Path(args.output_dir)
    if not input_dir.exists() or not input_dir.is_dir():
        raise FileNotFoundError(f"Input directory not found: {input_dir}")
    output_dir.mkdir(parents=True, exist_ok=True)

    reports: list[dict[str, Any]] = []
    images = sorted(input_dir.glob("*.png"))
    if not images:
        raise ValueError(f"No PNG files found in input directory: {input_dir}")

    shared_palette = load_palette(args.palette) if args.palette else None
    for image_path in images:
        output_path = output_dir / image_path.name
        report_path = output_dir / f"{image_path.stem}_report.json"
        _validate_output_path(image_path, output_path if args.apply_cleanup else None, args.force)

        source_arr = load_image(image_path)
        palette = shared_palette
        if palette is None:
            palette = _palette_for_image(
                source_arr, image_path, args, report_path, None, output_path
            )

        arr, masks, stray_components, report = analyze_image(
            image_path,
            palette,
            args.alpha_threshold,
            args.min_component_area,
            args.palette_tolerance,
            args.noise_threshold,
        )
        _save_mask_exports(masks, output_dir, image_path.stem)

        if args.apply_cleanup:
            cleaned = apply_cleanup(arr, masks, _cleanup_flags(args, palette))
            if args.smart_merge:
                cleaned, smart_report = _apply_smart_merge(
                    cleaned, args.cluster_size, args.auto_mode
                )
                report["smart_merge"] = smart_report
            report["removed_pixel_count"] = _removed_pixel_count(arr, cleaned)
            report["changed_pixel_count"] = _changed_pixel_count(arr, cleaned)
            _save_rgba(cleaned, output_path, args.force)
        elif args.smart_merge:
            _, smart_report = _apply_smart_merge(arr, args.cluster_size, args.auto_mode)
            report["smart_merge"] = smart_report

        report["recommended_actions"] = _recommended_actions(
            report, stray_components, args, palette
        )
        _write_json(report_path, report)
        reports.append(report)
    return reports


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        description=(
            "Detect and optionally clean AI-generated pixel art artifacts without AI calls."
        )
    )
    source = parser.add_mutually_exclusive_group(required=True)
    source.add_argument("--input", help="Single PNG input path")
    source.add_argument("--input_dir", help="Directory of PNG files for batch mode")
    source.add_argument(
        "--gui",
        nargs="?",
        const="",
        help="Open Tkinter GUI, optionally with an input PNG",
    )
    parser.add_argument("--output", help="Cleaned PNG path for single-image mode")
    parser.add_argument("--output_dir", help="Batch output directory")
    parser.add_argument("--palette", help="Palette JSON path")
    parser.add_argument("--report", help="Report JSON path for single-image mode")
    parser.add_argument("--preview", help="Preview PNG path for single-image mode")
    parser.add_argument("--alpha_threshold", type=int, default=128)
    parser.add_argument("--keep_alpha", action="store_true", help="Do not binarize alpha")
    parser.add_argument("--min_component_area", type=int, default=4)
    parser.add_argument("--remove_stray", action="store_true")
    parser.add_argument("--palette_tolerance", type=int, default=24)
    parser.add_argument("--snap_to_palette", action="store_true")
    parser.add_argument("--noise_threshold", type=int, default=40)
    parser.add_argument("--fix_color_noise", action="store_true")
    parser.add_argument("--smart_merge", action="store_true")
    parser.add_argument("--cluster_size", type=int, default=32)
    parser.add_argument(
        "--auto_mode",
        choices=("safe", "aggressive", "manual"),
        default="safe",
    )
    parser.add_argument(
        "--region",
        help="Analyze a clicked region as x,y or x,y,size in single-image mode",
    )
    parser.add_argument(
        "--apply_cleanup",
        action="store_true",
        help="Write cleaned PNG output. Default is report-only.",
    )
    parser.add_argument("--force", action="store_true", help="Allow output overwrite")
    parser.add_argument(
        "--make_palette",
        nargs="?",
        const=32,
        type=int,
        help="Extract the most common N colors when no palette is supplied",
    )
    return parser


def main(argv: list[str] | None = None) -> int:
    parser = build_parser()
    args = parser.parse_args(argv)
    try:
        if args.gui is not None:
            from pixel_cleanup_gui import run_gui

            return run_gui(args.gui or None)
        if args.input_dir:
            if args.region:
                raise ValueError("--region is only supported with --input")
            if not args.output_dir:
                raise ValueError("--output_dir is required with --input_dir")
            process_batch(args)
        else:
            process_single(args)
    except Exception as exc:
        print(f"error: {exc}", file=sys.stderr)
        return 1
    return 0


def _connected_components(mask: np.ndarray) -> list[Component]:
    height, width = mask.shape
    visited = np.zeros_like(mask, dtype=bool)
    components: list[Component] = []

    for start_y, start_x in np.argwhere(mask):
        sy = int(start_y)
        sx = int(start_x)
        if visited[sy, sx]:
            continue
        stack = [(sy, sx)]
        visited[sy, sx] = True
        pixels: list[tuple[int, int]] = []
        min_y = max_y = sy
        min_x = max_x = sx

        while stack:
            y, x = stack.pop()
            pixels.append((y, x))
            min_y = min(min_y, y)
            max_y = max(max_y, y)
            min_x = min(min_x, x)
            max_x = max(max_x, x)
            for dy in (-1, 0, 1):
                for dx in (-1, 0, 1):
                    if dy == 0 and dx == 0:
                        continue
                    ny = y + dy
                    nx = x + dx
                    if (
                        0 <= ny < height
                        and 0 <= nx < width
                        and mask[ny, nx]
                        and not visited[ny, nx]
                    ):
                        visited[ny, nx] = True
                        stack.append((ny, nx))

        components.append(
            Component(
                pixels=tuple(pixels),
                area=len(pixels),
                bbox=(min_x, min_y, max_x + 1, max_y + 1),
            )
        )
    return components


def _components_to_mask(shape: tuple[int, int], components: list[Component]) -> np.ndarray:
    mask = np.zeros(shape, dtype=bool)
    for component in components:
        _paint_component(mask, component, True)
    return mask


def _paint_component(mask: np.ndarray, component: Component, value: bool) -> None:
    for y, x in component.pixels:
        mask[y, x] = value


def _opaque_mask(arr: np.ndarray) -> np.ndarray:
    if arr.shape[-1] >= 4:
        return arr[..., 3] == 255
    return np.ones(arr.shape[:2], dtype=bool)


def _nearest_palette(pixels: np.ndarray, palette: list[RGB]) -> tuple[np.ndarray, np.ndarray]:
    if len(pixels) == 0:
        return np.array([], dtype=float), np.empty((0, 3), dtype=np.uint8)
    palette_arr = np.array(palette, dtype=np.int32)
    pixel_arr = pixels.astype(np.int32)
    diff = pixel_arr[:, None, :] - palette_arr[None, :, :]
    distances_sq = np.sum(diff * diff, axis=2)
    nearest_index = np.argmin(distances_sq, axis=1)
    distances = np.sqrt(distances_sq[np.arange(len(pixel_arr)), nearest_index])
    return distances, palette_arr[nearest_index].astype(np.uint8)


def _fix_noise_with_median(arr: np.ndarray, noise_mask: np.ndarray) -> np.ndarray:
    out = arr.copy()
    opaque = out[..., 3] == 255
    height, width = opaque.shape
    for y, x in np.argwhere(noise_mask & opaque):
        neighbors: list[np.ndarray] = []
        for dy in (-1, 0, 1):
            for dx in (-1, 0, 1):
                if dy == 0 and dx == 0:
                    continue
                ny = int(y) + dy
                nx = int(x) + dx
                if (
                    0 <= ny < height
                    and 0 <= nx < width
                    and opaque[ny, nx]
                    and not noise_mask[ny, nx]
                ):
                    neighbors.append(out[ny, nx, :3])
        if neighbors:
            out[y, x, :3] = np.median(np.array(neighbors), axis=0).astype(np.uint8)
    return out


def _overlay_masks(orig: np.ndarray, masks: MaskMap) -> np.ndarray:
    out = orig.copy()
    for name in ("stray", "palette_outliers", "alpha", "color_noise"):
        mask = masks[name]
        color = np.array(MASK_COLORS[name], dtype=np.uint8)
        if not np.any(mask):
            continue
        existing = out[mask, :3].astype(np.float32)
        blend = (existing * 0.35 + color[:3].astype(np.float32) * 0.65).astype(np.uint8)
        out[mask, :3] = blend
        out[mask, 3] = 255
    return out


def _mask_panel(mask: np.ndarray, color: tuple[int, int, int, int]) -> Image.Image:
    arr = np.zeros((mask.shape[0], mask.shape[1], 4), dtype=np.uint8)
    arr[mask] = np.array(color, dtype=np.uint8)
    return _rgba_on_checker(Image.fromarray(arr, mode="RGBA"))


def _rgba_on_checker(img: Image.Image) -> Image.Image:
    checker = Image.new("RGBA", img.size, (230, 230, 230, 255))
    draw = ImageDraw.Draw(checker)
    tile = 8
    for y in range(0, img.height, tile):
        for x in range(0, img.width, tile):
            if ((x // tile) + (y // tile)) % 2 == 0:
                draw.rectangle((x, y, x + tile - 1, y + tile - 1), fill=(250, 250, 250, 255))
    checker.alpha_composite(img)
    return checker


def _labeled_panel(img: Image.Image, label: str) -> Image.Image:
    header_height = 18
    panel = Image.new("RGBA", (img.width, img.height + header_height), (255, 255, 255, 255))
    draw = ImageDraw.Draw(panel)
    draw.rectangle((0, 0, img.width, header_height), fill=(24, 24, 24, 255))
    draw.text((3, 3), label, fill=(255, 255, 255, 255))
    panel.alpha_composite(img, (0, header_height))
    return panel


def _palette_for_image(
    arr: np.ndarray,
    input_path: Path,
    args: argparse.Namespace,
    report_path: Path | None,
    preview_path: Path | None,
    output_path: Path | None,
) -> list[RGB]:
    if args.palette:
        return load_palette(args.palette)
    if args.snap_to_palette:
        raise ValueError("--snap_to_palette requires --palette or --make_palette")
    if args.make_palette is None:
        return []

    palette = make_palette_from_image(arr, int(args.make_palette))
    artifact_dir = _single_artifact_dir(input_path, report_path, preview_path, output_path)
    if args.input_dir and args.output_dir:
        artifact_dir = Path(args.output_dir)
    if artifact_dir is None:
        artifact_dir = input_path.parent
    palette_name = (
        f"{input_path.stem}_suggested_palette.json"
        if args.input_dir
        else "suggested_palette.json"
    )
    _write_json(
        artifact_dir / palette_name,
        {"name": f"{input_path.stem}_suggested", "colors": [list(c) for c in palette]},
    )
    return palette


def _build_report(
    image_path: Path,
    arr: np.ndarray,
    masks: MaskMap,
    stray_components: list[Component],
) -> dict[str, Any]:
    alpha = arr[..., 3]
    return {
        "image_path": str(image_path),
        "image_size": [int(arr.shape[1]), int(arr.shape[0])],
        "total_opaque_pixels": int(np.count_nonzero(alpha == 255)),
        "semi_transparent_pixel_count": int(np.count_nonzero(masks["alpha"])),
        "stray_component_count": len(stray_components),
        "removed_pixel_count": 0,
        "palette_outlier_count": int(np.count_nonzero(masks["palette_outliers"])),
        "color_noise_count": int(np.count_nonzero(masks["color_noise"])),
        "bounding_box": _bounding_box(alpha > 0),
        "recommended_actions": [],
    }


def _recommended_actions(
    report: dict[str, Any],
    stray_components: list[Component],
    args: argparse.Namespace,
    palette: list[RGB],
) -> list[str]:
    actions: list[str] = []
    stray_pixels = sum(component.area for component in stray_components)
    if stray_components and not args.remove_stray:
        actions.append(
            "Run with --remove_stray to clean "
            f"{len(stray_components)} stray components ({stray_pixels} total pixels)"
        )
    if report["palette_outlier_count"] and palette and not args.snap_to_palette:
        actions.append(
            "Palette outliers detected "
            f"({report['palette_outlier_count']}). Consider --snap_to_palette."
        )
    if report["semi_transparent_pixel_count"] and not args.keep_alpha:
        actions.append(
            "Edge anti-aliasing detected "
            f"({report['semi_transparent_pixel_count']} semi-transparent). "
            f"Use --alpha_threshold {args.alpha_threshold}."
        )
    if report["color_noise_count"] and not args.fix_color_noise:
        actions.append(
            "Local color noise detected "
            f"({report['color_noise_count']}). Consider --fix_color_noise."
        )
    return actions


def _bounding_box(mask: np.ndarray) -> list[int] | None:
    if not np.any(mask):
        return None
    ys, xs = np.where(mask)
    return [int(xs.min()), int(ys.min()), int(xs.max() + 1), int(ys.max() + 1)]


def _removed_pixel_count(before: np.ndarray, after: np.ndarray) -> int:
    return int(np.count_nonzero((before[..., 3] > 0) & (after[..., 3] == 0)))


def _changed_pixel_count(before: np.ndarray, after: np.ndarray) -> int:
    return int(np.count_nonzero(np.any(before != after, axis=2)))


def _apply_smart_merge(
    arr: np.ndarray, cluster_size: int, auto_mode: str
) -> tuple[np.ndarray, dict[str, Any]]:
    if cluster_size < 1:
        raise ValueError("--cluster_size must be at least 1")
    from smart_merge import (
        build_cluster_report,
        classify_clusters,
        cluster_palette,
        compute_pixel_confidence,
        snap_outliers_to_dominant,
    )

    clusters = cluster_palette(arr, k=cluster_size, perceptual=True)
    total = int(np.count_nonzero(arr[..., 3] > 0))
    classifications = classify_clusters(clusters, total)
    snapped = snap_outliers_to_dominant(arr, clusters, classifications)
    confidence = compute_pixel_confidence(arr, clusters)
    changed = np.any(arr != snapped, axis=2)
    threshold = _auto_threshold(auto_mode)
    apply_mask = changed & (confidence > threshold)
    out = arr.copy()
    out[apply_mask] = snapped[apply_mask]
    return out, {
        "cluster_size": int(cluster_size),
        "auto_mode": auto_mode,
        "auto_threshold": threshold,
        "auto_fixed_pixel_count": int(np.count_nonzero(apply_mask)),
        "clusters": build_cluster_report(clusters, classifications),
    }


def _auto_threshold(auto_mode: str) -> float:
    if auto_mode == "aggressive":
        return 0.5
    if auto_mode == "manual":
        return 1.0
    return 0.7


def _process_region_request(arr: np.ndarray, args: argparse.Namespace) -> dict[str, Any]:
    from region_fix import analyze_region

    x, y, size = _parse_region(args.region)
    suggestions = analyze_region(arr, x, y, size)
    return {
        "region": {"x": x, "y": y, "size": size},
        "auto_mode": args.auto_mode,
        "auto_threshold": _auto_threshold(args.auto_mode),
        "suggestions": [_suggestion_to_dict(item) for item in suggestions],
    }


def _parse_region(value: str) -> tuple[int, int, int]:
    parts = [part.strip() for part in value.split(",")]
    if len(parts) not in (2, 3):
        raise ValueError("--region must be x,y or x,y,size")
    try:
        x = int(parts[0])
        y = int(parts[1])
        size = int(parts[2]) if len(parts) == 3 else 32
    except ValueError as exc:
        raise ValueError("--region values must be integers") from exc
    if size < 1:
        raise ValueError("--region size must be at least 1")
    return x, y, size


def _suggestion_to_dict(suggestion: Any) -> dict[str, Any]:
    return {
        "kind": suggestion.kind,
        "message": suggestion.message,
        "confidence": float(suggestion.confidence),
        "bbox": [int(value) for value in suggestion.bbox],
        "pixels": [[int(y), int(x)] for y, x in suggestion.pixels],
        "target_color": (
            [int(channel) for channel in suggestion.target_color]
            if suggestion.target_color is not None
            else None
        ),
        "metadata": suggestion.metadata or {},
    }


def _apply_region_suggestions(
    arr: np.ndarray, suggestions: list[dict[str, Any]], threshold: float
) -> np.ndarray:
    from region_fix import Suggestion, apply_suggestion

    out = arr.copy()
    if threshold >= 1.0:
        return out
    for item in suggestions:
        if float(item["confidence"]) <= threshold:
            continue
        target_color = item.get("target_color")
        suggestion = Suggestion(
            kind=str(item["kind"]),
            message=str(item["message"]),
            confidence=float(item["confidence"]),
            bbox=tuple(int(value) for value in item["bbox"]),
            pixels=tuple((int(y), int(x)) for y, x in item["pixels"]),
            target_color=(
                tuple(int(channel) for channel in target_color)
                if target_color is not None
                else None
            ),
            metadata=item.get("metadata") or {},
        )
        out = apply_suggestion(out, suggestion)
    return out


def _cleanup_flags(args: argparse.Namespace, palette: list[RGB]) -> dict[str, Any]:
    return {
        "keep_alpha": args.keep_alpha,
        "alpha_threshold": args.alpha_threshold,
        "remove_stray": args.remove_stray,
        "snap_to_palette": args.snap_to_palette,
        "fix_color_noise": args.fix_color_noise,
        "palette": palette,
    }


def _single_artifact_dir(
    input_path: Path,
    report_path: Path | None,
    preview_path: Path | None,
    output_path: Path | None,
) -> Path | None:
    for path in (report_path, preview_path, output_path):
        if path is not None:
            return path.parent
    return None


def _save_mask_exports(masks: MaskMap, directory: Path, stem: str | None) -> None:
    directory.mkdir(parents=True, exist_ok=True)
    for name, mask in masks.items():
        filename = MASK_FILENAMES[name]
        if stem is not None:
            filename = f"{stem}_{filename}"
        _mask_panel(mask, MASK_COLORS[name]).save(directory / filename)


def _save_rgba(arr: np.ndarray, path: Path, force: bool) -> None:
    _validate_output_path(None, path, force)
    _ensure_parent(path)
    Image.fromarray(arr, mode="RGBA").save(path)


def _validate_output_path(
    input_path: Path | None, output_path: Path | None, force: bool
) -> None:
    if output_path is None:
        return
    if input_path is not None:
        try:
            if input_path.resolve() == output_path.resolve():
                raise ValueError("Output path must not overwrite the original input")
        except FileNotFoundError:
            pass
    if output_path.exists() and not force:
        raise FileExistsError(f"Output already exists, pass --force to overwrite: {output_path}")


def _write_json(path: Path, data: dict[str, Any]) -> None:
    _ensure_parent(path)
    path.write_text(json.dumps(data, indent=2) + "\n", encoding="utf-8")


def _ensure_parent(path: Path) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)


if __name__ == "__main__":
    raise SystemExit(main())
