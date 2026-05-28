from __future__ import annotations

import sys
from pathlib import Path

import numpy as np

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

from smart_merge import (  # noqa: E402
    classify_clusters,
    cluster_palette,
    compute_pixel_confidence,
    snap_outliers_to_dominant,
)


def test_three_dominant_clusters_have_no_outliers() -> None:
    arr = np.zeros((30, 30, 4), dtype=np.uint8)
    arr[:10, :, :] = [30, 20, 10, 255]
    arr[10:20, :, :] = [80, 70, 60, 255]
    arr[20:, :, :] = [140, 130, 120, 255]

    clusters = cluster_palette(arr, k=3)
    classes = classify_clusters(clusters, 900)

    assert len(clusters.centroids) == 3
    assert set(classes.values()) == {"dominant"}


def test_stray_pixels_are_outliers_and_snap_to_dominant() -> None:
    arr = np.zeros((100, 100, 4), dtype=np.uint8)
    arr[:, :, :] = [26, 22, 18, 255]
    for y, x in ((5, 5), (6, 5), (7, 5), (8, 5), (9, 5)):
        arr[y, x] = [220, 20, 200, 255]

    clusters = cluster_palette(arr, k=2)
    classes = classify_clusters(clusters, 10000)
    snapped = snap_outliers_to_dominant(arr, clusters, classes)

    assert "outlier" in classes.values()
    assert np.all(snapped[5:10, 5, :3] == np.array([26, 22, 18], dtype=np.uint8))


def test_gradient_cluster_sizes_sum_to_opaque_total() -> None:
    arr = np.zeros((16, 16, 4), dtype=np.uint8)
    for y in range(16):
        for x in range(16):
            value = x * 16
            arr[y, x] = [value, value // 2, y * 16, 255]

    clusters = cluster_palette(arr, k=4)
    classes = classify_clusters(clusters, 256)

    assert int(np.sum(clusters.counts)) == 256
    assert len(classes) == len(clusters.centroids)


def test_confidence_scores_are_in_unit_range() -> None:
    arr = np.zeros((20, 20, 4), dtype=np.uint8)
    arr[:, :, :] = [50, 40, 30, 255]
    arr[0, 0] = [60, 42, 32, 255]

    clusters = cluster_palette(arr, k=2)
    confidence = compute_pixel_confidence(arr, clusters)

    assert confidence.shape == arr.shape[:2]
    assert float(confidence.min()) >= 0.0
    assert float(confidence.max()) <= 1.0
