"""Clean raw Opus design output: strip preamble, decode HTML entities."""
import html
from pathlib import Path

SRC = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_opus_design_raw.md")
DST = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/RIMA_FLUID_TRANSITION_DESIGN.md")

text = SRC.read_text(encoding="utf-8")

marker = "# RIMA Fluid Transition Architecture"
idx = text.find(marker)
assert idx >= 0, "Marker not found"
body = text[idx:]

body = html.unescape(body)

header = "<!-- Source: rima-design (Opus) verdict 2026-05-17. Codex review pending. -->\n\n"
DST.write_text(header + body, encoding="utf-8")
print(f"Wrote {DST}")
print(f"Bytes: {DST.stat().st_size}")
