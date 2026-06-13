#!/usr/bin/env python3
"""rima-conventions check_violations.py

Scans a RIMA project file and reports convention violations against
project rules sourced from .claude/PROJECT_RULES.md, memory/, and
CURRENT_STATUS.md.

Usage:
  python check_violations.py <file-path>
  python check_violations.py --all  (scan common RIMA directories)

Exit codes:
  0 = no HIGH priority violations
  1 = at least one HIGH priority violation
"""

import argparse
import re
import sys
from pathlib import Path

PROJECT_ROOT = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA")
MEMORY_ROOT = Path.home() / ".claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory"


# --- Violation dataclass -------------------------------------------------

class Violation:
    def __init__(self, priority, category, file, line, rule, found, fix):
        self.priority = priority  # HIGH / MEDIUM / LOW
        self.category = category  # DISPATCH / MEMORY / DESIGN / CODE / WORKFLOW
        self.file = file
        self.line = line
        self.rule = rule
        self.found = found
        self.fix = fix

    def to_md(self):
        return (
            f"### [{self.priority}] [{self.category}] {self.rule[:80]}\n"
            f"**File:** {self.file}:{self.line}\n"
            f"**Rule:** {self.rule}\n"
            f"**Found:** {self.found[:200]}\n"
            f"**Fix:** {self.fix}\n"
        )


# --- Checks --------------------------------------------------------------

def check_dispatch_task(path: Path, content: str, lines: list[str]) -> list[Violation]:
    """Codex dispatch task .md files in STAGING/."""
    v = []
    name = path.name
    is_dispatch = (
        "STAGING" in str(path)
        and name.endswith(".md")
        and ("task" in name.lower() or "dispatch" in name.lower() or "codex_" in name.lower())
    )
    if not is_dispatch:
        return v

    # Check 1: First non-empty line must be ACTIVE RULES
    first_nonempty = None
    first_idx = 0
    for i, line in enumerate(lines, 1):
        if line.strip():
            first_nonempty = line.strip()
            first_idx = i
            break
    if first_nonempty and not first_nonempty.startswith("ACTIVE RULES:"):
        v.append(Violation(
            "HIGH", "DISPATCH",
            str(path), first_idx,
            ".claude/PROJECT_RULES.md — every dispatch task must start with ACTIVE RULES",
            first_nonempty,
            "Prepend: 'ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.'",
        ))

    # Check 2: NLM ACCESS block within first 15 lines
    head = "\n".join(lines[:15])
    if "NLM ACCESS:" not in head:
        v.append(Violation(
            "HIGH", "DISPATCH",
            str(path), 1,
            ".claude/PROJECT_RULES.md S91 LOCK — NLM ACCESS block required in dispatch",
            "NLM ACCESS block not found in first 15 lines",
            "Add 'NLM ACCESS: ...' block after ACTIVE RULES line. See PROJECT_RULES.md 'Sub-Agent NLM Access — MANDATORY'.",
        ))

    # Check 3: "Amaç:" purpose line required
    if not re.search(r"\*\*Amaç:\*\*|Amaç:", content):
        v.append(Violation(
            "HIGH", "DISPATCH",
            str(path), 1,
            "memory: feedback_state_task_purpose_explicitly — Amaç: required",
            "No 'Amaç:' purpose line found in file",
            "Add '**Amaç:** <bir cümle, dispatch'in amacı>' near top of task body",
        ))

    # Check 4: If image generation mentioned, must use built-in tool mode
    if re.search(r"\$imagegen|image.gen|image_gen|gpt-image", content, re.I):
        if "OPENAI_API_KEY" in content and "do NOT" not in content and "DO NOT" not in content:
            v.append(Violation(
                "HIGH", "DISPATCH",
                str(path), 1,
                "memory: feedback_codex_imagegen_skill_not_env_var — built-in tool mode lock",
                "Mentions OPENAI_API_KEY without explicit 'do NOT use CLI fallback' guard",
                "Add explicit 'Use $imagegen in BUILT-IN TOOL MODE. DO NOT use CLI fallback. OPENAI_API_KEY is not required.'",
            ))

    return v


def check_memory_file(path: Path, content: str, lines: list[str]) -> list[Violation]:
    """Memory files under ~/.claude/projects/.../memory/."""
    v = []
    if "memory" not in str(path).replace("\\", "/").lower():
        return v
    if not path.name.endswith(".md") or path.name == "MEMORY.md":
        return v

    # Check 1: Frontmatter
    if not content.startswith("---"):
        v.append(Violation(
            "MEDIUM", "MEMORY",
            str(path), 1,
            "memory format — frontmatter required",
            "File does not start with '---' frontmatter delimiter",
            "Add YAML frontmatter at top: ---\\nname: ...\\ndescription: ...\\nmetadata:\\n  type: ...\\n---",
        ))
    else:
        # Extract frontmatter
        end_idx = content.find("---", 4)
        if end_idx == -1:
            v.append(Violation(
                "MEDIUM", "MEMORY",
                str(path), 1,
                "memory format — frontmatter not closed",
                "Opening '---' found but no closing '---'",
                "Close frontmatter with '---' line",
            ))
        else:
            fm = content[4:end_idx]
            for field in ("name:", "description:", "type:"):
                if field not in fm:
                    v.append(Violation(
                        "MEDIUM", "MEMORY",
                        str(path), 1,
                        f"memory format — frontmatter missing {field}",
                        f"Field '{field}' not found in frontmatter",
                        f"Add '{field} <value>' to frontmatter",
                    ))

    # Check 2: ASCII-only body
    body_start = content.find("---", 4) + 3 if content.startswith("---") else 0
    body = content[body_start:]
    for i, line in enumerate(lines, 1):
        for ch in line:
            if ord(ch) > 127:
                # Allow non-ASCII inside frontmatter
                if i <= 10 and content.startswith("---"):
                    continue
                v.append(Violation(
                    "LOW", "MEMORY",
                    str(path), i,
                    "memory format — ASCII-only body (transliterate Turkish chars)",
                    f"Non-ASCII char '{ch}' (U+{ord(ch):04X}) on line {i}: {line[:80]}",
                    "Transliterate Turkish chars (ı→i, ş→s, ç→c, ğ→g, ü→u, ö→o, İ→I, Ş→S, Ç→C, Ğ→G, Ü→U, Ö→O)",
                ))
                break  # one per line max

    return v


def check_design_locks(path: Path, content: str, lines: list[str]) -> list[Violation]:
    """Cross-cutting design lock violations in any file."""
    v = []
    # S101 PILLAR-LESS asserted as live (revoked 2026-05-24)
    for i, line in enumerate(lines, 1):
        stripped = line.strip()
        if not stripped or stripped.startswith("#"):
            continue
        # Asserted, not historical reference
        if re.search(r"PILLAR-LESS", line, re.I):
            # Skip if line clearly references revocation
            if "REVOKED" in line.upper() or "supersede" in line.lower() or "archive" in line.lower():
                continue
            # Skip if line says "no pillar" in a layout context that's not the S101 lock
            if "S101" in line or "pillar-less wall" in line.lower():
                v.append(Violation(
                    "HIGH", "DESIGN",
                    str(path), i,
                    "memory: project_pillar_seam_cover_lock_2026_05_24 — S101 PILLAR-LESS REVOKED",
                    line.strip()[:200],
                    "Update reference: pillar-as-seam-cover is the live strategy. Mark old PILLAR-LESS lock as REVOKED.",
                ))

    # PPU value other than 64
    for i, line in enumerate(lines, 1):
        m = re.search(r"PPU[^\d]{0,5}(\d+)", line)
        if m:
            ppu = int(m.group(1))
            if ppu not in (64, 100):  # 100 only for Gemini reference quotes
                v.append(Violation(
                    "MEDIUM", "DESIGN",
                    str(path), i,
                    "Karar #114 PPU 64 LOCK — character baseline",
                    f"PPU {ppu} mentioned: {line.strip()[:120]}",
                    "Use PPU 64 (RIMA character baseline). Other values only valid in research/reference quotes.",
                ))

    # Y-sort axis other than (0, 1, 0)
    for i, line in enumerate(lines, 1):
        if re.search(r"y.?sort|custom\s*axis|sort\s*axis", line, re.I):
            if re.search(r"\(\s*0\s*,\s*[02-9]\s*,\s*0\s*\)", line):
                v.append(Violation(
                    "MEDIUM", "DESIGN",
                    str(path), i,
                    "research lock — Custom Axis (0, 1, 0) for iso ARPG Y-sort",
                    line.strip()[:200],
                    "Use Custom Axis (0, 1, 0) — research-backed pattern from industry analysis.",
                ))

    # Banner in new wall pieces (user lock 2026-05-24)
    if "wall" in path.name.lower() and "banner" in content.lower():
        # Only flag if banner is in active spec, not in revocation note
        for i, line in enumerate(lines, 1):
            if "banner" in line.lower() and "NO banner" not in line and "no banner" not in line and "removed" not in line.lower():
                v.append(Violation(
                    "MEDIUM", "DESIGN",
                    str(path), i,
                    "user lock 2026-05-24 — NO banner in new wall pieces",
                    line.strip()[:200],
                    "Remove banner from wall asset spec. Banner is a separate decor overlay, not baked into wall.",
                ))
                break

    # Doorway with wood door
    if "doorway" in content.lower() and re.search(r"wood\s*door|wooden\s*door", content, re.I):
        for i, line in enumerate(lines, 1):
            if re.search(r"wood\s*door|wooden\s*door", line, re.I) and "NO wood" not in line and "no wood" not in line:
                v.append(Violation(
                    "MEDIUM", "DESIGN",
                    str(path), i,
                    "user lock — doorway must be empty void interior (no wood door)",
                    line.strip()[:200],
                    "Remove wood door reference. Doorway is stone arch + black void interior only.",
                ))
                break

    return v


def check_code_conventions(path: Path, content: str, lines: list[str]) -> list[Violation]:
    """Unity C# script + asset path conventions."""
    v = []
    p = str(path).replace("\\", "/")

    # Fractured chamber assets must be in Assets/Art/FracturedChamber/
    if "FracturedChamber" in content or "fractured_chamber" in content.lower():
        if "/Assets/" in p and "FracturedChamber" not in p:
            # File talks about fractured chamber but lives elsewhere in Assets
            v.append(Violation(
                "LOW", "CODE",
                str(path), 1,
                "asset path convention — fractured chamber assets go under Assets/Art/FracturedChamber/",
                f"File at {p} discusses FracturedChamber but is not under Assets/Art/FracturedChamber/",
                "Move or rename so fractured chamber assets live in Assets/Art/FracturedChamber/. Test scenes under Assets/Scenes/Demo/.",
            ))

    # PPU import settings in dispatch tasks
    if path.name.endswith(".md") and "Sprite Mode" in content:
        if "PPU" not in content and "Pixels Per Unit" not in content:
            v.append(Violation(
                "LOW", "CODE",
                str(path), 1,
                "sprite import convention — PPU 64 must be specified explicitly",
                "File discusses Sprite Mode but doesn't specify PPU",
                "Add 'PPU: 64' to sprite import settings block.",
            ))

    return v


def check_workflow(path: Path, content: str, lines: list[str]) -> list[Violation]:
    """Workflow rule violations (delegation, autonomous PixelLab, etc.)."""
    v = []
    # PixelLab autonomous night warning — check if dispatch implies night autonomous
    if "PixelLab" in content and re.search(r"overnight|autonomous|background.*?(7|8|9|10)\s*hours", content, re.I):
        v.append(Violation(
            "MEDIUM", "WORKFLOW",
            str(path), 1,
            "memory: feedback_no_pixellab_night_autonomous — no PixelLab gen autonomous overnight",
            "Dispatch mentions PixelLab + overnight/autonomous long-running",
            "PixelLab gen must be user-supervised (web UI when user awake). Replace with mockup workflow or wait for user.",
        ))
    return v


# --- Driver --------------------------------------------------------------

def scan_file(path: Path) -> list[Violation]:
    if not path.is_file():
        return []
    try:
        content = path.read_text(encoding="utf-8", errors="replace")
    except Exception as e:
        return [Violation("LOW", "META", str(path), 0, "read error", str(e), "Verify file readable as UTF-8")]
    lines = content.splitlines()
    violations = []
    violations.extend(check_dispatch_task(path, content, lines))
    violations.extend(check_memory_file(path, content, lines))
    violations.extend(check_design_locks(path, content, lines))
    violations.extend(check_code_conventions(path, content, lines))
    violations.extend(check_workflow(path, content, lines))
    return violations


def render_report(target: str, all_violations: dict[str, list[Violation]]) -> str:
    total = sum(len(v) for v in all_violations.values())
    high = sum(1 for v in all_violations.values() for x in v if x.priority == "HIGH")
    medium = sum(1 for v in all_violations.values() for x in v if x.priority == "MEDIUM")
    low = sum(1 for v in all_violations.values() for x in v if x.priority == "LOW")

    out = []
    out.append(f"# RIMA Conventions Review - {target}\n")
    out.append("## Summary")
    out.append(f"- Total violations: {total}")
    out.append(f"- HIGH priority: {high}")
    out.append(f"- MEDIUM priority: {medium}")
    out.append(f"- LOW priority: {low}\n")

    if total == 0:
        out.append("**No violations found.** File passes RIMA conventions check.\n")
        return "\n".join(out)

    out.append("## Violations\n")
    # Sort by priority HIGH > MEDIUM > LOW
    priority_order = {"HIGH": 0, "MEDIUM": 1, "LOW": 2}
    flat = [v for vs in all_violations.values() for v in vs]
    flat.sort(key=lambda x: (priority_order.get(x.priority, 3), x.file, x.line))
    for v in flat:
        out.append(v.to_md())
    return "\n".join(out)


def main():
    parser = argparse.ArgumentParser(description="RIMA convention violation scanner")
    parser.add_argument("path", nargs="?", help="File or directory to scan")
    parser.add_argument("--all", action="store_true", help="Scan common RIMA directories")
    parser.add_argument("--report", help="Write report to this path instead of stdout")
    args = parser.parse_args()

    targets = []
    if args.all:
        # Common RIMA dirs
        for d in [PROJECT_ROOT / "STAGING", MEMORY_ROOT, PROJECT_ROOT / ".claude"]:
            if d.is_dir():
                for p in d.rglob("*.md"):
                    if "_archive" in str(p):
                        continue
                    targets.append(p)
    elif args.path:
        p = Path(args.path)
        if p.is_file():
            targets = [p]
        elif p.is_dir():
            targets = [x for x in p.rglob("*.md") if "_archive" not in str(x)]
        else:
            print(f"Path not found: {args.path}", file=sys.stderr)
            sys.exit(2)
    else:
        parser.print_help()
        sys.exit(2)

    all_v = {}
    for t in targets:
        vs = scan_file(t)
        if vs:
            all_v[str(t)] = vs

    target_desc = args.path if args.path else "RIMA project (all)"
    report = render_report(target_desc, all_v)
    if args.report:
        Path(args.report).write_text(report, encoding="utf-8")
        print(f"Report written to {args.report}")
    else:
        print(report)

    high_count = sum(1 for vs in all_v.values() for v in vs if v.priority == "HIGH")
    sys.exit(1 if high_count > 0 else 0)


if __name__ == "__main__":
    main()
