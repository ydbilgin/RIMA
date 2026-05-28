#!/usr/bin/env python3
"""Interactive local region analysis for pixel cleanup."""

from __future__ import annotations

import argparse
import json
from dataclasses import dataclass
from pathlib import Path
from typing import Any

import numpy as np
from PIL import Image, ImageDraw

try:
    from smart_merge import (
        classify_clusters,
        cluster_palette,
        snap_outliers_to_dominant,
    )
except ImportError:  # pragma: no cover - package style fallback
    from .smart_merge import (  # type: ignore
        classify_clusters,
        cluster_palette,
        snap_outliers_to_dominant,
    )


@dataclass(frozen=True)
class Suggestion:
    kind: str
    message: str
    confidence: float
    bbox: tuple[int, int, int, int]
    pixels: tuple[tuple[int, int], ...]
    target_color: tuple[int, int, int] | None = None
    metadata: dict[str, Any] | None = None


def analyze_region(
    image: np.ndarray, x: int, y: int, size: int = 32
) -> list[Suggestion]:
    arr = _as_rgba(image)
    left, top, right, bottom = _region_bounds(arr, x, y, size)
    region = arr[top:bottom, left:right]
    opaque = region[..., 3] > 0
    semi = (region[..., 3] > 0) & (region[..., 3] < 255)
    if not np.any(opaque):
        return []

    suggestions: list[Suggestion] = []
    stray_pixels = _stray_pixels(opaque, top, left)
    if stray_pixels:
        suggestions.append(
            _make_suggestion(
                "stray_pixels",
                f"{len(stray_pixels)} stray pixels detected (alpha < threshold)",
                0.95,
                stray_pixels,
                target_color=(0, 0, 0),
                metadata={"action": "clear_alpha"},
            )
        )

    if semi.shape[0]:
        bottom_hits = tuple((bottom - 1, left + int(px)) for px in np.flatnonzero(semi[-1]))
        if bottom_hits:
            suggestions.append(
                _make_suggestion(
                    "alpha_edge",
                    "Edge anti-aliasing on bottom row "
                    f"({len(bottom_hits)} semi-transparent)",
                    0.8,
                    bottom_hits,
                    metadata={"action": "binarize_alpha"},
                )
            )

    semi_other = semi.copy()
    if semi_other.shape[0]:
        semi_other[-1, :] = False
    semi_pixels = tuple(
        (top + int(py), left + int(px)) for py, px in np.argwhere(semi_other)
    )
    if semi_pixels:
        suggestions.append(
            _make_suggestion(
                "alpha_edge",
                f"{len(semi_pixels)} semi-transparent pixels detected",
                0.74,
                semi_pixels,
                metadata={"action": "binarize_alpha"},
            )
        )

    suggestions.extend(_cluster_suggestions(region, top, left))
    return suggestions


def apply_suggestion(image: np.ndarray, suggestion: Suggestion) -> np.ndarray:
    out = _as_rgba(image).copy()
    height, width = out.shape[:2]
    for py, px in suggestion.pixels:
        if not (0 <= py < height and 0 <= px < width):
            continue
        if suggestion.kind == "stray_pixels":
            out[py, px] = np.array([0, 0, 0, 0], dtype=np.uint8)
        elif suggestion.kind == "alpha_edge":
            out[py, px, 3] = 255 if out[py, px, 3] >= 128 else 0
            if out[py, px, 3] == 0:
                out[py, px, :3] = 0
        elif suggestion.target_color is not None:
            out[py, px, :3] = np.array(suggestion.target_color, dtype=np.uint8)
            out[py, px, 3] = 255
    return out


def render_region_preview(
    image: np.ndarray,
    x: int,
    y: int,
    size: int,
    suggestions: list[Suggestion],
) -> Image.Image:
    arr = _as_rgba(image)
    edited = arr.copy()
    for suggestion in suggestions:
        edited = apply_suggestion(edited, suggestion)

    left, top, right, bottom = _region_bounds(arr, x, y, size)
    before = Image.fromarray(arr[top:bottom, left:right], mode="RGBA")
    after = Image.fromarray(edited[top:bottom, left:right], mode="RGBA")
    scale = max(1, 192 // max(before.width, before.height, 1))
    before = before.resize((before.width * scale, before.height * scale), Image.NEAREST)
    after = after.resize((after.width * scale, after.height * scale), Image.NEAREST)
    header = 18
    gutter = 8
    sheet = Image.new(
        "RGBA",
        (before.width + after.width + gutter, max(before.height, after.height) + header),
        (245, 245, 245, 255),
    )
    draw = ImageDraw.Draw(sheet)
    draw.text((3, 3), "before", fill=(20, 20, 20, 255))
    draw.text((before.width + gutter + 3, 3), "after", fill=(20, 20, 20, 255))
    sheet.alpha_composite(before, (0, header))
    sheet.alpha_composite(after, (before.width + gutter, header))
    return sheet


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description="Analyze one pixel cleanup region.")
    parser.add_argument("--input", required=True)
    parser.add_argument("--region", required=True, help="x,y or x,y,size")
    parser.add_argument("--report")
    args = parser.parse_args(argv)

    arr = np.array(Image.open(args.input).convert("RGBA"), dtype=np.uint8)
    x, y, size = _parse_region(args.region)
    suggestions = analyze_region(arr, x, y, size)
    data = {"suggestions": [_suggestion_to_dict(item) for item in suggestions]}
    if args.report:
        Path(args.report).write_text(json.dumps(data, indent=2) + "\n", encoding="utf-8")
    else:
        print(json.dumps(data, indent=2))
    return 0


def _cluster_suggestions(region: np.ndarray, top: int, left: int) -> list[Suggestion]:
    opaque_count = int(np.count_nonzero(region[..., 3] > 0))
    if opaque_count < 8:
        return []
    unique_count = len(np.unique(region[..., :3][region[..., 3] > 0], axis=0))
    clusters = cluster_palette(region, k=max(1, min(8, unique_count)), perceptual=True)
    classifications = classify_clusters(clusters, opaque_count)
    outlier_ids = {
        cluster_id
        for cluster_id, classification in classifications.items()
        if classification == "outlier"
    }
    if not outlier_ids:
        return []

    snapped = snap_outliers_to_dominant(region, clusters, classifications)
    coords = np.argwhere(region[..., 3] > 0)
    suggestions: list[Suggestion] = []
    for cluster_id in sorted(outlier_ids):
        local_indices = np.flatnonzero(clusters.labels == cluster_id)
        if len(local_indices) == 0:
            continue
        pixels = tuple(
            (top + int(coords[index][0]), left + int(coords[index][1]))
            for index in local_indices
        )
        first_y, first_x = coords[int(local_indices[0])]
        target = tuple(int(channel) for channel in snapped[first_y, first_x, :3])
        centroid = clusters.centroids[cluster_id]
        distance = float(
            np.linalg.norm(centroid.astype(float) - np.array(target, dtype=float))
        )
        suggestions.append(
            _make_suggestion(
                "palette_snap",
                "Color noise cluster of "
                f"{len(pixels)} pixels (suggest: merge to #{target[0]:02X}{target[1]:02X}{target[2]:02X})",
                0.88 if len(pixels) <= 5 else 0.68,
                pixels,
                target_color=target,
                metadata={
                    "cluster_id": int(cluster_id),
                    "source_centroid": [int(channel) for channel in centroid],
                    "distance": distance,
                },
            )
        )
    return suggestions


def _stray_pixels(
    opaque: np.ndarray, top: int, left: int
) -> tuple[tuple[int, int], ...]:
    components = _connected_components(opaque)
    if len(components) <= 1:
        return tuple()
    main_index = int(np.argmax([len(component) for component in components]))
    pixels: list[tuple[int, int]] = []
    for index, component in enumerate(components):
        if index == main_index or len(component) > 3:
            continue
        pixels.extend((top + py, left + px) for py, px in component)
    return tuple(pixels)


def _connected_components(mask: np.ndarray) -> list[list[tuple[int, int]]]:
    height, width = mask.shape
    visited = np.zeros_like(mask, dtype=bool)
    components: list[list[tuple[int, int]]] = []
    for start_y, start_x in np.argwhere(mask):
        sy = int(start_y)
        sx = int(start_x)
        if visited[sy, sx]:
            continue
        stack = [(sy, sx)]
        visited[sy, sx] = True
        pixels: list[tuple[int, int]] = []
        while stack:
            py, px = stack.pop()
            pixels.append((py, px))
            for dy in (-1, 0, 1):
                for dx in (-1, 0, 1):
                    if dy == 0 and dx == 0:
                        continue
                    ny = py + dy
                    nx = px + dx
                    if (
                        0 <= ny < height
                        and 0 <= nx < width
                        and mask[ny, nx]
                        and not visited[ny, nx]
                    ):
                        visited[ny, nx] = True
                        stack.append((ny, nx))
        components.append(pixels)
    return components


def _make_suggestion(
    kind: str,
    message: str,
    confidence: float,
    pixels: tuple[tuple[int, int], ...],
    target_color: tuple[int, int, int] | None = None,
    metadata: dict[str, Any] | None = None,
) -> Suggestion:
    ys = [pixel[0] for pixel in pixels]
    xs = [pixel[1] for pixel in pixels]
    bbox = (min(xs), min(ys), max(xs) + 1, max(ys) + 1) if pixels else (0, 0, 0, 0)
    return Suggestion(
        kind=kind,
        message=message,
        confidence=float(confidence),
        bbox=bbox,
        pixels=pixels,
        target_color=target_color,
        metadata=metadata or {},
    )


def _suggestion_to_dict(suggestion: Suggestion) -> dict[str, Any]:
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


def _region_bounds(
    arr: np.ndarray, x: int, y: int, size: int
) -> tuple[int, int, int, int]:
    size = max(1, int(size))
    half = size // 2
    height, width = arr.shape[:2]
    left = max(0, min(width - 1, int(x) - half))
    top = max(0, min(height - 1, int(y) - half))
    right = min(width, left + size)
    bottom = min(height, top + size)
    return left, top, right, bottom


def _parse_region(value: str) -> tuple[int, int, int]:
    parts = [part.strip() for part in value.split(",")]
    if len(parts) not in (2, 3):
        raise ValueError("--region must be x,y or x,y,size")
    x = int(parts[0])
    y = int(parts[1])
    size = int(parts[2]) if len(parts) == 3 else 32
    return x, y, size


def _as_rgba(image: np.ndarray) -> np.ndarray:
    arr = np.asarray(image)
    if arr.ndim != 3 or arr.shape[-1] < 4:
        raise ValueError("Region analysis requires an RGBA image array")
    return arr.astype(np.uint8, copy=False)


if __name__ == "__main__":
    raise SystemExit(main())
