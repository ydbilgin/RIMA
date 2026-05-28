from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

import numpy as np
from PIL import Image


TOOL = Path(__file__).resolve().parents[1] / "pixel_cleanup.py"
STONE = [26, 22, 18, 255]
RIFT = [0, 221, 255, 255]


def write_png(path: Path, arr: np.ndarray) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    Image.fromarray(arr, mode="RGBA").save(path)


def write_palette(path: Path) -> None:
    path.write_text(
        json.dumps(
            {
                "name": "test",
                "colors": [
                    STONE[:3],
                    RIFT[:3],
                    [0, 0, 0],
                    [255, 255, 255],
                ],
            }
        ),
        encoding="utf-8",
    )


def image_with_block(size: int = 10) -> np.ndarray:
    arr = np.zeros((size, size, 4), dtype=np.uint8)
    arr[1:5, 1:5] = STONE
    return arr


def run_cli(*args: str) -> subprocess.CompletedProcess[str]:
    result = subprocess.run(
        [sys.executable, str(TOOL), *args],
        text=True,
        capture_output=True,
        check=True,
    )
    assert result.stderr == ""
    return result


def read_report(path: Path) -> dict:
    return json.loads(path.read_text(encoding="utf-8"))


def test_clean_image_reports_no_stray_or_outlier(tmp_path: Path) -> None:
    image = tmp_path / "clean.png"
    palette = tmp_path / "palette.json"
    report = tmp_path / "report.json"
    write_png(image, image_with_block())
    write_palette(palette)

    run_cli("--input", str(image), "--palette", str(palette), "--report", str(report))

    data = read_report(report)
    assert data["stray_component_count"] == 0
    assert data["palette_outlier_count"] == 0
    assert data["semi_transparent_pixel_count"] == 0


def test_stray_dots_are_detected(tmp_path: Path) -> None:
    arr = image_with_block(12)
    arr[10, 1] = STONE
    arr[1, 10] = STONE
    arr[10, 10] = STONE
    image = tmp_path / "stray.png"
    palette = tmp_path / "palette.json"
    report = tmp_path / "report.json"
    write_png(image, arr)
    write_palette(palette)

    run_cli("--input", str(image), "--palette", str(palette), "--report", str(report))

    assert read_report(report)["stray_component_count"] == 3


def test_semi_transparent_edges_detect_and_threshold_fix(tmp_path: Path) -> None:
    arr = image_with_block()
    arr[5, 1] = [26, 22, 18, 64]
    arr[5, 2] = [26, 22, 18, 200]
    image = tmp_path / "alpha.png"
    palette = tmp_path / "palette.json"
    report = tmp_path / "report.json"
    output = tmp_path / "clean.png"
    write_png(image, arr)
    write_palette(palette)

    run_cli("--input", str(image), "--palette", str(palette), "--report", str(report))
    assert read_report(report)["semi_transparent_pixel_count"] == 2

    run_cli(
        "--input",
        str(image),
        "--output",
        str(output),
        "--palette",
        str(palette),
        "--apply_cleanup",
    )
    cleaned = np.array(Image.open(output).convert("RGBA"))
    assert cleaned[5, 1, 3] == 0
    assert cleaned[5, 2, 3] == 255


def test_off_palette_pixels_are_detected(tmp_path: Path) -> None:
    arr = image_with_block()
    arr[2, 2] = [255, 0, 255, 255]
    image = tmp_path / "outlier.png"
    palette = tmp_path / "palette.json"
    report = tmp_path / "report.json"
    write_png(image, arr)
    write_palette(palette)

    run_cli("--input", str(image), "--palette", str(palette), "--report", str(report))

    assert read_report(report)["palette_outlier_count"] == 1


def test_batch_mode_writes_outputs_and_reports(tmp_path: Path) -> None:
    input_dir = tmp_path / "raw"
    output_dir = tmp_path / "clean"
    palette = tmp_path / "palette.json"
    write_palette(palette)
    for index in range(5):
        write_png(input_dir / f"tile_{index}.png", image_with_block())

    run_cli(
        "--input_dir",
        str(input_dir),
        "--output_dir",
        str(output_dir),
        "--palette",
        str(palette),
        "--apply_cleanup",
    )

    assert all((output_dir / f"tile_{index}.png").exists() for index in range(5))
    assert len(list(output_dir.glob("*_report.json"))) == 5


def test_apply_cleanup_writes_output_file(tmp_path: Path) -> None:
    arr = image_with_block(12)
    arr[10, 10] = STONE
    image = tmp_path / "input.png"
    output = tmp_path / "output.png"
    palette = tmp_path / "palette.json"
    write_png(image, arr)
    write_palette(palette)

    run_cli(
        "--input",
        str(image),
        "--output",
        str(output),
        "--palette",
        str(palette),
        "--remove_stray",
        "--apply_cleanup",
    )

    cleaned = np.array(Image.open(output).convert("RGBA"))
    assert output.exists()
    assert cleaned[10, 10, 3] == 0


def test_report_only_does_not_write_output_file(tmp_path: Path) -> None:
    image = tmp_path / "input.png"
    output = tmp_path / "output.png"
    report = tmp_path / "report.json"
    palette = tmp_path / "palette.json"
    write_png(image, image_with_block())
    write_palette(palette)

    run_cli(
        "--input",
        str(image),
        "--output",
        str(output),
        "--palette",
        str(palette),
        "--report",
        str(report),
    )

    assert report.exists()
    assert not output.exists()
