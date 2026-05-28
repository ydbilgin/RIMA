from __future__ import annotations

import sys
from pathlib import Path

import numpy as np

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from region_fix import (  # noqa: E402
    analyze_region,
    apply_suggestion,
    render_region_preview,
)


STONE = np.array([26, 22, 18, 255], dtype=np.uint8)
ODD = np.array([230, 20, 180, 255], dtype=np.uint8)


def test_clean_region_returns_no_suggestions() -> None:
    arr = np.zeros((48, 48, 4), dtype=np.uint8)
    arr[8:40, 8:40] = STONE

    suggestions = analyze_region(arr, 24, 24, 32)

    assert suggestions == []


def test_region_with_stray_and_outlier_returns_multiple_suggestions() -> None:
    arr = np.zeros((64, 64, 4), dtype=np.uint8)
    arr[22:42, 22:42] = STONE
    arr[18, 18] = STONE
    arr[30, 30] = ODD

    suggestions = analyze_region(arr, 32, 32, 32)
    kinds = {suggestion.kind for suggestion in suggestions}

    assert "stray_pixels" in kinds
    assert "palette_snap" in kinds


def test_apply_suggestion_modifies_array() -> None:
    arr = np.zeros((64, 64, 4), dtype=np.uint8)
    arr[22:42, 22:42] = STONE
    arr[18, 18] = STONE

    suggestions = analyze_region(arr, 32, 32, 32)
    stray = next(item for item in suggestions if item.kind == "stray_pixels")
    edited = apply_suggestion(arr, stray)

    assert arr[18, 18, 3] == 255
    assert edited[18, 18, 3] == 0


def test_preview_rendering_does_not_modify_original() -> None:
    arr = np.zeros((64, 64, 4), dtype=np.uint8)
    arr[22:42, 22:42] = STONE
    arr[18, 18] = STONE
    original = arr.copy()

    suggestions = analyze_region(arr, 32, 32, 32)
    preview = render_region_preview(arr, 32, 32, 32, suggestions)

    assert preview.width > 0
    assert preview.height > 0
    assert np.array_equal(arr, original)
