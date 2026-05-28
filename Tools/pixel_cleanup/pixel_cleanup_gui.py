#!/usr/bin/env python3
"""Tkinter GUI for local pixel cleanup."""

from __future__ import annotations

import argparse
import json
from pathlib import Path
from typing import Any

import numpy as np
from PIL import Image, ImageDraw, ImageTk
import tkinter as tk
from tkinter import filedialog, messagebox, ttk

try:
    from . import pixel_cleanup
    from .region_fix import analyze_region, apply_suggestion, render_region_preview
    from .smart_merge import (
        build_cluster_report,
        classify_clusters,
        cluster_palette,
        compute_pixel_confidence,
        snap_outliers_to_dominant,
    )
except ImportError:  # pragma: no cover - script execution fallback
    import pixel_cleanup
    from region_fix import analyze_region, apply_suggestion, render_region_preview
    from smart_merge import (
        build_cluster_report,
        classify_clusters,
        cluster_palette,
        compute_pixel_confidence,
        snap_outliers_to_dominant,
    )


class PixelCleanupGUI:
    def __init__(self, root: tk.Tk, input_path: str | None = None) -> None:
        self.root = root
        self.root.title("pixel_cleanup GUI")
        self.input_path: Path | None = None
        self.original: np.ndarray | None = None
        self.cleaned: np.ndarray | None = None
        self.report: dict[str, Any] = {}
        self.zoom = 4.0
        self._photo_refs: dict[str, ImageTk.PhotoImage] = {}

        self.remove_stray = tk.BooleanVar(value=True)
        self.snap_palette = tk.BooleanVar(value=True)
        self.fix_noise = tk.BooleanVar(value=True)
        self.smart_merge = tk.BooleanVar(value=True)
        self.region_mode = tk.BooleanVar(value=False)
        self.diff_overlay = tk.BooleanVar(value=True)

        self.min_area = tk.IntVar(value=4)
        self.palette_tol = tk.IntVar(value=24)
        self.noise_thresh = tk.IntVar(value=40)
        self.confidence = tk.DoubleVar(value=0.7)
        self.cluster_size = tk.IntVar(value=32)

        self._build_layout()
        if input_path:
            self.load_path(Path(input_path))
        else:
            self.root.after(100, self.open_file)

    def _build_layout(self) -> None:
        frame = ttk.Frame(self.root, padding=8)
        frame.grid(row=0, column=0, sticky="nsew")
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(0, weight=1)
        frame.columnconfigure(0, weight=1)
        frame.columnconfigure(1, weight=1)

        self.original_canvas = self._make_canvas(frame, "ORIGINAL", 0)
        self.preview_canvas = self._make_canvas(frame, "CLEANED PREVIEW", 1)
        self.original_canvas.bind("<Button-1>", self._on_original_click)

        tools = ttk.LabelFrame(frame, text="Tools panel", padding=8)
        tools.grid(row=1, column=0, columnspan=2, sticky="ew", pady=(8, 0))
        for col in range(5):
            tools.columnconfigure(col, weight=1)
        ttk.Checkbutton(tools, text="Stray", variable=self.remove_stray).grid(row=0, column=0, sticky="w")
        ttk.Checkbutton(tools, text="Outlier", variable=self.snap_palette).grid(row=0, column=1, sticky="w")
        ttk.Checkbutton(tools, text="Color Noise", variable=self.fix_noise).grid(row=0, column=2, sticky="w")
        ttk.Checkbutton(tools, text="Smart Merge", variable=self.smart_merge).grid(row=0, column=3, sticky="w")
        ttk.Checkbutton(tools, text="Region Fix Mode", variable=self.region_mode).grid(row=0, column=4, sticky="w")
        ttk.Checkbutton(tools, text="Diff overlay", variable=self.diff_overlay, command=self._render_images).grid(row=1, column=0, sticky="w")

        sliders = ttk.LabelFrame(frame, text="Sliders", padding=8)
        sliders.grid(row=2, column=0, columnspan=2, sticky="ew", pady=(8, 0))
        for col in range(4):
            sliders.columnconfigure(col, weight=1)
        self._slider(sliders, "min_area", self.min_area, 1, 24, 0)
        self._slider(sliders, "palette_tol", self.palette_tol, 0, 96, 1)
        self._slider(sliders, "noise_thresh", self.noise_thresh, 0, 128, 2)
        self._slider(sliders, "confidence", self.confidence, 0.5, 1.0, 3)
        self._slider(sliders, "cluster_size", self.cluster_size, 2, 64, 4)

        actions = ttk.Frame(frame)
        actions.grid(row=3, column=0, columnspan=2, sticky="ew", pady=(8, 0))
        for index, (label, command) in enumerate(
            [
                ("Open", self.open_file),
                ("Analyze", self.analyze),
                ("Apply Auto", self.apply_auto),
                ("Save", self.save_output),
                ("Export Rep", self.export_report),
            ]
        ):
            ttk.Button(actions, text=label, command=command).grid(row=0, column=index, padx=3)

        self.status = tk.StringVar(value="Open a PNG to begin.")
        ttk.Label(frame, textvariable=self.status).grid(row=4, column=0, columnspan=2, sticky="ew", pady=(8, 0))

    def _make_canvas(self, parent: ttk.Frame, label: str, col: int) -> tk.Canvas:
        container = ttk.LabelFrame(parent, text=label, padding=4)
        container.grid(row=0, column=col, sticky="nsew", padx=4)
        canvas = tk.Canvas(container, width=380, height=380, background="#1f1f1f", highlightthickness=0)
        canvas.grid(row=0, column=0, sticky="nsew")
        canvas.bind("<MouseWheel>", self._on_zoom)
        canvas.bind("<ButtonPress-2>", lambda event, c=canvas: c.scan_mark(event.x, event.y))
        canvas.bind("<B2-Motion>", lambda event, c=canvas: c.scan_dragto(event.x, event.y, gain=1))
        container.columnconfigure(0, weight=1)
        container.rowconfigure(0, weight=1)
        return canvas

    def _slider(
        self,
        parent: ttk.Frame,
        label: str,
        variable: tk.IntVar | tk.DoubleVar,
        start: float,
        end: float,
        row: int,
    ) -> None:
        ttk.Label(parent, text=label).grid(row=row, column=0, sticky="w")
        ttk.Scale(parent, from_=start, to=end, variable=variable, orient="horizontal").grid(row=row, column=1, columnspan=2, sticky="ew", padx=6)
        ttk.Label(parent, textvariable=variable).grid(row=row, column=3, sticky="e")

    def open_file(self) -> None:
        path = filedialog.askopenfilename(filetypes=[("PNG images", "*.png"), ("All files", "*.*")])
        if path:
            self.load_path(Path(path))

    def load_path(self, path: Path) -> None:
        try:
            self.original = pixel_cleanup.load_image(path)
        except Exception as exc:
            messagebox.showerror("Open failed", str(exc))
            return
        self.input_path = path
        self.cleaned = self.original.copy()
        self.report = {"image_path": str(path)}
        self.root.title(f"pixel_cleanup GUI - {path.name}")
        self.status.set(f"Loaded {path.name}")
        self._render_images()

    def analyze(self) -> None:
        if self.original is None or self.input_path is None:
            return
        palette = pixel_cleanup.make_palette_from_image(self.original, int(self.cluster_size.get()))
        arr, masks, stray_components, report = pixel_cleanup.analyze_image(
            self.input_path,
            palette,
            128,
            int(self.min_area.get()),
            int(self.palette_tol.get()),
            int(self.noise_thresh.get()),
        )
        cleaned = pixel_cleanup.apply_cleanup(
            arr,
            masks,
            {
                "keep_alpha": False,
                "alpha_threshold": 128,
                "remove_stray": self.remove_stray.get(),
                "snap_to_palette": self.snap_palette.get(),
                "fix_color_noise": self.fix_noise.get(),
                "palette": palette,
            },
        )
        smart_report: list[dict[str, object]] = []
        if self.smart_merge.get():
            clusters = cluster_palette(cleaned, k=int(self.cluster_size.get()))
            classifications = classify_clusters(clusters, int(np.count_nonzero(cleaned[..., 3] > 0)))
            confidence = compute_pixel_confidence(cleaned, clusters)
            merged = snap_outliers_to_dominant(cleaned, clusters, classifications)
            auto_mask = confidence >= float(self.confidence.get())
            cleaned[auto_mask] = merged[auto_mask]
            smart_report = build_cluster_report(clusters, classifications)
        report["smart_merge_clusters"] = smart_report
        report["recommended_actions"] = pixel_cleanup._recommended_actions(
            report, stray_components, argparse.Namespace(
                remove_stray=self.remove_stray.get(),
                snap_to_palette=self.snap_palette.get(),
                keep_alpha=False,
                fix_color_noise=self.fix_noise.get(),
                alpha_threshold=128,
            ),
            palette,
        )
        self.cleaned = cleaned
        self.report = report
        self.status.set("Analysis complete.")
        self._render_images()

    def apply_auto(self) -> None:
        self.analyze()
        self.status.set(f"Applied auto threshold {float(self.confidence.get()):.2f}.")

    def save_output(self) -> None:
        if self.cleaned is None or self.input_path is None:
            return
        default = self.input_path.with_name(f"{self.input_path.stem}_clean.png")
        path = filedialog.asksaveasfilename(defaultextension=".png", initialfile=default.name)
        if not path:
            return
        Image.fromarray(self.cleaned, mode="RGBA").save(path)
        report_path = Path(path).with_suffix(".json")
        report_path.write_text(json.dumps(self.report, indent=2) + "\n", encoding="utf-8")
        self.status.set(f"Saved {Path(path).name} and {report_path.name}.")

    def export_report(self) -> None:
        if not self.report:
            return
        path = filedialog.asksaveasfilename(defaultextension=".json", initialfile="pixel_cleanup_report.json")
        if path:
            Path(path).write_text(json.dumps(self.report, indent=2) + "\n", encoding="utf-8")
            self.status.set(f"Exported {Path(path).name}.")

    def _on_original_click(self, event: tk.Event) -> None:
        if self.original is None:
            return
        x = int(self.original_canvas.canvasx(event.x) / self.zoom)
        y = int(self.original_canvas.canvasy(event.y) / self.zoom)
        if not (0 <= x < self.original.shape[1] and 0 <= y < self.original.shape[0]):
            return
        if self.region_mode.get():
            self._open_region_modal(x, y)
        else:
            self.status.set(f"Selected region at {x},{y}. Enable Region Fix Mode to edit.")

    def _open_region_modal(self, x: int, y: int) -> None:
        assert self.original is not None
        suggestions = analyze_region(self.original, x, y, 32)
        modal = tk.Toplevel(self.root)
        modal.title(f"Region fix {x},{y}")
        preview = render_region_preview(self.original, x, y, 32, suggestions)
        photo = ImageTk.PhotoImage(preview.resize((preview.width * 4, preview.height * 4), Image.Resampling.NEAREST))
        self._photo_refs["region"] = photo
        ttk.Label(modal, image=photo).grid(row=0, column=0, columnspan=3)
        if not suggestions:
            ttk.Label(modal, text="No reliable fixes detected, manual mode required").grid(row=1, column=0, sticky="w")
            return
        for row, suggestion in enumerate(suggestions, start=1):
            ttk.Label(modal, text=f"{suggestion.confidence:.2f}  {suggestion.message}").grid(row=row, column=0, sticky="w")
            ttk.Button(modal, text="Apply", command=lambda s=suggestion, m=modal: self._apply_region(s, m)).grid(row=row, column=1)
        ttk.Button(modal, text="Apply All", command=lambda: self._apply_all_regions(suggestions, modal)).grid(row=len(suggestions) + 1, column=0, sticky="w")

    def _apply_region(self, suggestion: Any, modal: tk.Toplevel) -> None:
        if self.original is None:
            return
        self.original = apply_suggestion(self.original, suggestion)
        self.cleaned = self.original.copy()
        modal.destroy()
        self._render_images()

    def _apply_all_regions(self, suggestions: list[Any], modal: tk.Toplevel) -> None:
        if self.original is None:
            return
        out = self.original
        for suggestion in suggestions:
            out = apply_suggestion(out, suggestion)
        self.original = out
        self.cleaned = out.copy()
        modal.destroy()
        self._render_images()

    def _on_zoom(self, event: tk.Event) -> None:
        if event.delta > 0:
            self.zoom = min(16.0, self.zoom * 1.25)
        else:
            self.zoom = max(1.0, self.zoom / 1.25)
        self._render_images()

    def _render_images(self) -> None:
        if self.original is None:
            return
        self._draw_canvas(self.original_canvas, self.original, "orig")
        preview = self.cleaned if self.cleaned is not None else self.original
        if self.diff_overlay.get() and self.cleaned is not None:
            preview = _diff_overlay(self.original, self.cleaned)
        self._draw_canvas(self.preview_canvas, preview, "preview")

    def _draw_canvas(self, canvas: tk.Canvas, arr: np.ndarray, key: str) -> None:
        image = Image.fromarray(arr, mode="RGBA")
        size = (max(1, int(image.width * self.zoom)), max(1, int(image.height * self.zoom)))
        photo = ImageTk.PhotoImage(image.resize(size, Image.Resampling.NEAREST))
        self._photo_refs[key] = photo
        canvas.delete("all")
        canvas.create_image(0, 0, image=photo, anchor="nw")
        canvas.configure(scrollregion=(0, 0, size[0], size[1]))


def _diff_overlay(before: np.ndarray, after: np.ndarray) -> np.ndarray:
    overlay = after.copy()
    removed = (before[..., 3] > 0) & (after[..., 3] == 0)
    changed = np.any(before != after, axis=2)
    overlay[removed] = [255, 0, 0, 255]
    overlay[changed & ~removed] = [0, 210, 60, 255]
    return overlay


def run_gui(input_path: str | None = None) -> int:
    root = tk.Tk()
    PixelCleanupGUI(root, input_path or None)
    root.mainloop()
    return 0


if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("input", nargs="?")
    ns = parser.parse_args()
    raise SystemExit(run_gui(ns.input))
