"""
RIMA Palette-Lock Daemon (Aseprite CLI batch remap)

Source: F:/LaurethStudio/02_GAMES/StudioHibritFarmingSim/Tools/PainterSuiteV2/palette_lock_daemon.py
        Studio universal S15 workflow (studio_custom_wang_build_workflow.md)
Adapted: 2026-05-26 for RIMA action-roguelite paths.

Watch folder: STAGING/asset_inbox/
Output:       Assets/Sprites/Environment/_remapped/
Archive:      STAGING/asset_processed/

Usage:
    python "F:/Antigravity Projeler/2d roguelite/RIMA/Tools/palette_lock_daemon.py"

Optional CLI args:
    --inbox <path>     Override inbox folder
    --outbox <path>    Override outbox folder
    --processed <path> Override archive folder
    --dither {none,ordered,error-diffusion}  Default: none
    --interval <sec>   Poll interval. Default: 2

See `MEMORY/wang_tile_build_workflow_rima.md` Bölüm VI for full integration spec.
"""

import argparse
import pathlib
import subprocess
import sys
import time

# RIMA defaults — Studio universal pattern with RIMA paths
RIMA_ROOT = pathlib.Path(__file__).resolve().parent.parent
RIMA_PALETTE = str(RIMA_ROOT / "Art" / "Palettes" / "rima_palette.gpl")
ASEPRITE = r"C:\Program Files\Aseprite\Aseprite.exe"

DEFAULT_INBOX = str(RIMA_ROOT / "STAGING" / "asset_inbox")
DEFAULT_OUTBOX = str(RIMA_ROOT / "Assets" / "Sprites" / "Environment" / "_remapped")
DEFAULT_PROCESSED = str(RIMA_ROOT / "STAGING" / "asset_processed")


def remap_palette(input_path: str, output_path: str, palette_path: str, dither: str = "none") -> bool:
    """Run Aseprite CLI to convert input PNG to 16-color indexed using palette."""
    cmd = [
        ASEPRITE,
        "-b",
        input_path,
        "--color-mode",
        "indexed",
        "--palette",
        palette_path,
        "--dithering-algorithm",
        dither,
        "--save-as",
        output_path,
    ]
    try:
        result = subprocess.run(cmd, capture_output=True, text=True, timeout=60)
    except FileNotFoundError:
        print(f"[FAIL] Aseprite not found at: {ASEPRITE}", file=sys.stderr)
        return False
    except subprocess.TimeoutExpired:
        print(f"[FAIL] Aseprite timeout on: {input_path}", file=sys.stderr)
        return False

    if result.returncode != 0:
        print(f"[FAIL] {input_path}: {result.stderr.strip()}", file=sys.stderr)
        return False
    return True


def watch_inbox(inbox: str, outbox: str, processed: str, palette: str, dither: str, interval: float) -> None:
    inbox_path = pathlib.Path(inbox)
    outbox_path = pathlib.Path(outbox)
    processed_path = pathlib.Path(processed)
    inbox_path.mkdir(parents=True, exist_ok=True)
    outbox_path.mkdir(parents=True, exist_ok=True)
    processed_path.mkdir(parents=True, exist_ok=True)

    if not pathlib.Path(palette).exists():
        print(f"[WARN] Palette not found: {palette}", file=sys.stderr)
        print(f"[WARN] Create palette first or pass --palette path/to/.gpl", file=sys.stderr)

    print(f"[Palette-Lock Daemon] RIMA instance running")
    print(f"  inbox    : {inbox_path}")
    print(f"  outbox   : {outbox_path}")
    print(f"  processed: {processed_path}")
    print(f"  palette  : {palette}")
    print(f"  dither   : {dither}")
    print(f"  interval : {interval}s")
    print(f"[Drop .png files into inbox; daemon will remap and move them.]")

    while True:
        for png in sorted(inbox_path.glob("*.png")):
            out = outbox_path / png.name
            if remap_palette(str(png), str(out), palette, dither):
                archive = processed_path / png.name
                try:
                    png.rename(archive)
                    print(f"[OK] {png.name} -> {out}")
                except OSError as e:
                    print(f"[WARN] Could not archive {png.name}: {e}", file=sys.stderr)
        time.sleep(interval)


def main() -> int:
    parser = argparse.ArgumentParser(description="RIMA Palette-Lock Daemon (Aseprite CLI batch remap)")
    parser.add_argument("--inbox", default=DEFAULT_INBOX, help="Watch folder for incoming PNGs")
    parser.add_argument("--outbox", default=DEFAULT_OUTBOX, help="Output folder for remapped PNGs")
    parser.add_argument("--processed", default=DEFAULT_PROCESSED, help="Archive folder for processed inputs")
    parser.add_argument("--palette", default=RIMA_PALETTE, help="Palette .gpl path")
    parser.add_argument(
        "--dither",
        choices=["none", "ordered", "error-diffusion"],
        default="none",
        help="Dithering algorithm",
    )
    parser.add_argument("--interval", type=float, default=2.0, help="Poll interval in seconds")
    args = parser.parse_args()

    try:
        watch_inbox(
            inbox=args.inbox,
            outbox=args.outbox,
            processed=args.processed,
            palette=args.palette,
            dither=args.dither,
            interval=args.interval,
        )
    except KeyboardInterrupt:
        print("\n[Palette-Lock Daemon] Stopped.")
        return 0
    return 0


if __name__ == "__main__":
    sys.exit(main())
