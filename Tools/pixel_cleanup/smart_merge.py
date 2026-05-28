#!/usr/bin/env python3
"""Smart palette clustering and outlier snapping for pixel art cleanup."""

from __future__ import annotations

from dataclasses import dataclass
from typing import Any

import numpy as np


@dataclass(frozen=True)
class ClusterResult:
    centroids: np.ndarray
    counts: np.ndarray
    labels: np.ndarray
    distances: np.ndarray
    opaque_mask: np.ndarray | None
    original_shape: tuple[int, ...]
    space_centroids: np.ndarray
    color_space: str


def cluster_palette(
    rgb_array: np.ndarray, k: int = 32, perceptual: bool = True
) -> ClusterResult:
    pixels, opaque_mask, original_shape = _extract_pixels(rgb_array)
    color_space = "lab" if perceptual else "rgb"
    if len(pixels) == 0:
        return ClusterResult(
            centroids=np.empty((0, 3), dtype=np.uint8),
            counts=np.array([], dtype=int),
            labels=np.array([], dtype=int),
            distances=np.array([], dtype=float),
            opaque_mask=opaque_mask,
            original_shape=original_shape,
            space_centroids=np.empty((0, 3), dtype=float),
            color_space=color_space,
        )

    unique_rgb, inverse, unique_counts = np.unique(
        pixels, axis=0, return_inverse=True, return_counts=True
    )
    k = max(1, min(int(k), len(unique_rgb)))
    unique_space = _rgb_to_lab(unique_rgb) if perceptual else unique_rgb.astype(float)

    if len(unique_rgb) <= k:
        unique_labels = np.arange(len(unique_rgb), dtype=int)
        centroids_rgb = unique_rgb.astype(np.uint8)
        space_centroids = unique_space.astype(float)
        counts = unique_counts.astype(int)
        unique_distances = np.zeros(len(unique_rgb), dtype=float)
    else:
        unique_labels, space_centroids = _weighted_kmeans(
            unique_space, unique_counts, k
        )
        centroids_rgb, counts, space_centroids, unique_labels = _finalize_clusters(
            unique_rgb, unique_counts, unique_labels, space_centroids
        )
        unique_distances = np.linalg.norm(
            unique_space - space_centroids[unique_labels], axis=1
        )

    labels = unique_labels[inverse].astype(int)
    distances = unique_distances[inverse].astype(float)
    return ClusterResult(
        centroids=centroids_rgb.astype(np.uint8),
        counts=counts.astype(int),
        labels=labels,
        distances=distances,
        opaque_mask=opaque_mask,
        original_shape=original_shape,
        space_centroids=space_centroids.astype(float),
        color_space=color_space,
    )


def classify_clusters(
    clusters: ClusterResult, total_pixels: int
) -> dict[int, str]:
    total = int(total_pixels) if int(total_pixels) > 0 else int(np.sum(clusters.counts))
    if total <= 0:
        return {}

    classifications: dict[int, str] = {}
    for index, count in enumerate(clusters.counts):
        ratio = float(count) / float(total)
        if ratio > 0.05:
            classifications[index] = "dominant"
        elif ratio < 0.005:
            classifications[index] = "outlier"
        else:
            classifications[index] = "review"
    return classifications


def snap_outliers_to_dominant(
    rgb_array: np.ndarray,
    clusters: ClusterResult,
    classifications: dict[int, str],
) -> np.ndarray:
    out = np.array(rgb_array, copy=True)
    if len(clusters.centroids) == 0 or len(clusters.labels) == 0:
        return out

    dominant = [
        cluster_id
        for cluster_id, classification in classifications.items()
        if classification == "dominant"
    ]
    outliers = {
        cluster_id
        for cluster_id, classification in classifications.items()
        if classification == "outlier"
    }
    if not dominant or not outliers:
        return out

    dominant_space = clusters.space_centroids[dominant]
    snap_map: dict[int, np.ndarray] = {}
    for cluster_id in outliers:
        distances = np.linalg.norm(
            dominant_space - clusters.space_centroids[cluster_id], axis=1
        )
        nearest = dominant[int(np.argmin(distances))]
        snap_map[cluster_id] = clusters.centroids[nearest]

    flat = out.reshape((-1, out.shape[-1]))
    if clusters.opaque_mask is None:
        target_indices = np.arange(len(flat))
    else:
        target_indices = np.flatnonzero(clusters.opaque_mask.reshape(-1))

    for cluster_id, target_rgb in snap_map.items():
        selected = target_indices[clusters.labels == cluster_id]
        if len(selected):
            flat[selected, :3] = target_rgb
    return out


def compute_pixel_confidence(
    rgb_array: np.ndarray, clusters: ClusterResult
) -> np.ndarray:
    if clusters.opaque_mask is not None:
        confidence = np.zeros(clusters.opaque_mask.shape, dtype=float)
    else:
        confidence = np.zeros((len(clusters.labels),), dtype=float)

    if len(clusters.labels) == 0:
        return confidence

    scale = 100.0 if clusters.color_space == "lab" else 441.67295593
    pixel_confidence = 1.0 - np.clip(clusters.distances / max(scale * 0.35, 1.0), 0, 1)

    if clusters.opaque_mask is None:
        return pixel_confidence.astype(float)
    confidence[clusters.opaque_mask] = pixel_confidence
    return confidence.astype(float)


def build_cluster_report(
    clusters: ClusterResult,
    classifications: dict[int, str],
    confidence: np.ndarray | None = None,
) -> dict[str, Any]:
    rows: list[dict[str, Any]] = []
    total = int(np.sum(clusters.counts))
    for index, centroid in enumerate(clusters.centroids):
        count = int(clusters.counts[index])
        ratio = float(count) / float(total) if total else 0.0
        rows.append(
            {
                "id": int(index),
                "centroid": [int(channel) for channel in centroid],
                "size": count,
                "ratio": ratio,
                "classification": classifications.get(index, "unknown"),
            }
        )

    report: dict[str, Any] = {
        "color_space": clusters.color_space,
        "cluster_count": len(rows),
        "total_pixels": total,
        "clusters": rows,
        "dominant_count": sum(
            1 for item in classifications.values() if item == "dominant"
        ),
        "outlier_count": sum(
            1 for item in classifications.values() if item == "outlier"
        ),
        "review_count": sum(1 for item in classifications.values() if item == "review"),
    }
    if confidence is not None and confidence.size:
        opaque_confidence = confidence[confidence > 0]
        if opaque_confidence.size:
            report["confidence"] = {
                "min": float(np.min(opaque_confidence)),
                "mean": float(np.mean(opaque_confidence)),
                "max": float(np.max(opaque_confidence)),
            }
    return report


def _extract_pixels(
    arr: np.ndarray, alpha_threshold: int = 0
) -> tuple[np.ndarray, np.ndarray | None, tuple[int, ...]]:
    array = np.asarray(arr)
    if array.ndim != 3 or array.shape[-1] < 3:
        raise ValueError("Input must be an RGB or RGBA image array")
    if array.shape[-1] >= 4:
        opaque_mask = array[..., 3] > alpha_threshold
        pixels = array[..., :3][opaque_mask]
    else:
        opaque_mask = None
        pixels = array[..., :3].reshape((-1, 3))
    return pixels.astype(np.uint8), opaque_mask, tuple(array.shape)


def _weighted_kmeans(
    points: np.ndarray, weights: np.ndarray, k: int, iterations: int = 24
) -> tuple[np.ndarray, np.ndarray]:
    centers = _initial_centers(points, weights, k)
    labels = np.zeros(len(points), dtype=int)
    for _ in range(iterations):
        distances = _distance_matrix(points, centers)
        next_labels = np.argmin(distances, axis=1)
        if np.array_equal(labels, next_labels):
            break
        labels = next_labels
        next_centers = centers.copy()
        for cluster_id in range(len(centers)):
            selected = labels == cluster_id
            if np.any(selected):
                next_centers[cluster_id] = np.average(
                    points[selected], axis=0, weights=weights[selected]
                )
        centers = next_centers
    return labels, centers


def _initial_centers(points: np.ndarray, weights: np.ndarray, k: int) -> np.ndarray:
    selected: list[int] = [int(np.argmax(weights))]
    min_distances = np.linalg.norm(points - points[selected[0]], axis=1)
    while len(selected) < k:
        scores = min_distances * np.sqrt(weights.astype(float))
        for index in selected:
            scores[index] = -1.0
        next_index = int(np.argmax(scores))
        selected.append(next_index)
        min_distances = np.minimum(
            min_distances, np.linalg.norm(points - points[next_index], axis=1)
        )
    return points[selected].astype(float)


def _finalize_clusters(
    unique_rgb: np.ndarray,
    unique_counts: np.ndarray,
    labels: np.ndarray,
    centers: np.ndarray,
) -> tuple[np.ndarray, np.ndarray, np.ndarray, np.ndarray]:
    raw_counts = np.bincount(labels, weights=unique_counts, minlength=len(centers))
    active = raw_counts > 0
    remap = np.full(len(centers), -1, dtype=int)
    active_indices = np.flatnonzero(active)
    remap[active_indices] = np.arange(len(active_indices))
    labels = remap[labels]

    centroids_rgb: list[np.ndarray] = []
    counts: list[int] = []
    space_centroids: list[np.ndarray] = []
    for cluster_id, center_index in enumerate(active_indices):
        selected = labels == cluster_id
        weights = unique_counts[selected]
        centroids_rgb.append(
            np.average(unique_rgb[selected], axis=0, weights=weights).round()
        )
        counts.append(int(np.sum(weights)))
        space_centroids.append(centers[center_index])
    return (
        np.array(centroids_rgb, dtype=np.uint8),
        np.array(counts, dtype=int),
        np.array(space_centroids, dtype=float),
        labels.astype(int),
    )


def _rgb_to_lab(rgb: np.ndarray) -> np.ndarray:
    values = np.asarray(rgb, dtype=float) / 255.0
    linear = np.where(
        values > 0.04045,
        ((values + 0.055) / 1.055) ** 2.4,
        values / 12.92,
    )
    matrix = np.array(
        [
            [0.4124564, 0.3575761, 0.1804375],
            [0.2126729, 0.7151522, 0.0721750],
            [0.0193339, 0.1191920, 0.9503041],
        ],
        dtype=float,
    )
    xyz = linear @ matrix.T
    xyz = xyz / np.array([0.95047, 1.0, 1.08883], dtype=float)
    epsilon = 216.0 / 24389.0
    kappa = 24389.0 / 27.0
    f = np.where(xyz > epsilon, np.cbrt(xyz), (kappa * xyz + 16.0) / 116.0)
    l_value = 116.0 * f[:, 1] - 16.0
    a_value = 500.0 * (f[:, 0] - f[:, 1])
    b_value = 200.0 * (f[:, 1] - f[:, 2])
    return np.stack([l_value, a_value, b_value], axis=1)


def _distance_matrix(points: np.ndarray, centers: np.ndarray) -> np.ndarray:
    diff = points[:, None, :] - centers[None, :, :]
    return np.sqrt(np.sum(diff * diff, axis=2))
