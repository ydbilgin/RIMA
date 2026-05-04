import os
import re
import sys
import argparse
from pathlib import Path
from datetime import datetime, timedelta

def get_frontmatter_value(content, key):
    match = re.search(rf'^{key}:\s*(.*)$', content, re.MULTILINE)
    return match.group(1).strip() if match else ""

def count_lines(file_path):
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            return sum(1 for _ in f)
    except:
        return 0

def run_audit(memory_dir, repo_dir, output_file):
    memory_path = Path(memory_dir)
    repo_path = Path(repo_dir)
    staging_path = repo_path / "STAGING"
    archive_path = repo_path / "_ARCHIVE"
    
    memory_index_file = memory_path / "MEMORY.md"
    current_status_file = repo_path / "CURRENT_STATUS.md"
    codex_tasks_file = repo_path / "CODEX_TASKS.md"
    
    memory_index_content = ""
    if memory_index_file.exists():
        with open(memory_index_file, 'r', encoding='utf-8') as f:
            memory_index_content = f.read()
            
    current_status_content = ""
    if current_status_file.exists():
        with open(current_status_file, 'r', encoding='utf-8') as f:
            current_status_content = f.read()
            
    codex_tasks_content = ""
    if codex_tasks_file.exists():
        with open(codex_tasks_file, 'r', encoding='utf-8') as f:
            codex_tasks_content = f.read()

    now = datetime.now()
    
    report = []
    summary = {}

    # Pass 1: Memory Orphans
    orphans = []
    memory_files = list(memory_path.glob("*.md"))
    for f in memory_files:
        if f.name == "MEMORY.md": continue
        if f.name not in memory_index_content:
            content = ""
            try:
                with open(f, 'r', encoding='utf-8') as file:
                    content = file.read()
            except: pass
            desc = get_frontmatter_value(content, "description")
            orphans.append(f"- {f.name} | {desc}")
    
    if orphans:
        report.append("## Memory Orphans")
        report.extend(orphans)
        report.append("")
    summary['mem_orphans'] = len(orphans)

    # Pass 2: Memory Staleness
    stale = []
    active_passes_match = re.search(r'## Active Passes(.*?)(##|$)', memory_index_content, re.DOTALL)
    active_passes = active_passes_match.group(1) if active_passes_match else ""
    
    for f in memory_files:
        if f.name == "MEMORY.md": continue
        mtime = datetime.fromtimestamp(f.stat().st_mtime)
        if now - mtime > timedelta(days=60) and f.name not in active_passes:
            content = ""
            try:
                with open(f, 'r', encoding='utf-8') as file:
                    content = file.read()
            except: pass
            desc = get_frontmatter_value(content, "description")
            stale.append(f"- {f.name} | last modified: {mtime.strftime('%Y-%m-%d')} | {desc}")
            
    if stale:
        report.append("## Memory Stale")
        report.extend(stale)
        report.append("")
    summary['mem_stale'] = len(stale)

    # Pass 3: Memory Size Violators
    oversized = []
    skip_list = [
        "reference_pixellab_v3_ui.md", "reference_pixellab_direction_sequence.md",
        "feedback_enhance_action_default.md", "feedback_compact_default.md",
        "project_compact_pass_s40.md", "project_rima_backlog.md", "MEMORY.md"
    ]
    for f in memory_files:
        if f.name in skip_list: continue
        lc = count_lines(f)
        if lc > 30:
            oversized.append(f"- {f.name} | {lc} lines")
            
    if oversized:
        report.append("## Memory Oversized")
        report.extend(oversized)
        report.append("")
    summary['mem_oversized'] = len(oversized)

    # Pass 4: Memory Hard Cap
    total_mem = len([f for f in memory_files if f.name != "MEMORY.md"])
    if total_mem > 60:
        report.append(f"## Memory Cap WARNING — {total_mem} files (cap: 60)")
        report.append("")
    summary['mem_total'] = total_mem

    # Pass 5: STAGING Orphans
    staging_orphans = []
    if staging_path.exists():
        for item in staging_path.iterdir():
            mtime = datetime.fromtimestamp(item.stat().st_mtime)
            if item.name not in current_status_content and item.name not in codex_tasks_content:
                if now - mtime > timedelta(days=14):
                    size_str = ""
                    if item.is_file():
                        size_str = f"{item.stat().st_size // 1024}kB"
                    else:
                        size_str = f"{len(list(item.glob('**/*')))} files"
                    staging_orphans.append(f"- {item.name} | last modified: {mtime.strftime('%Y-%m-%d')} | size: {size_str}")
                    
    if staging_orphans:
        report.append("## Staging Orphans")
        report.extend(staging_orphans)
        report.append("")
    summary['staging_orphans'] = len(staging_orphans)
    summary['staging_total'] = len(list(staging_path.iterdir())) if staging_path.exists() else 0

    # Pass 6: _ARCHIVE Intruders
    archive_intruders = []
    if archive_path.exists():
        for item in archive_path.glob("**/*"):
            mtime = datetime.fromtimestamp(item.stat().st_mtime)
            if now - mtime < timedelta(days=7):
                archive_intruders.append(f"- {item.relative_to(archive_path)} | last modified: {mtime.strftime('%Y-%m-%d')}")
                
    if archive_intruders:
        report.append("## Archive Intruders")
        report.extend(archive_intruders)
        report.append("")
    summary['archive_intruders'] = len(archive_intruders)

    # Pass 7: Root Clutter
    root_clutter = []
    allow_list = [
        "CLAUDE.md", "CURRENT_STATUS.md", "AGENTS.md", "README.md", "CODEX.md",
        "CODEX_TASKS.md", "CODEX_DONE.md", "SYSTEM_MAP.md", "MEMORY.md",
        ".gitignore", ".graphifyignore", ".graphify_incremental.json", "RIMA.slnx"
    ]
    for f in repo_path.iterdir():
        if not f.is_file(): continue
        name = f.name
        is_allowed = name in allow_list or \
                     name.startswith("Assembly-CSharp") or \
                     name.startswith("RIMA.") and name.endswith(".csproj") or \
                     name.endswith(".unitypackage") or \
                     name == ".graphify_python"
        if not is_allowed:
            root_clutter.append(f"- {name}")
            
    if root_clutter:
        report.append("## Root Clutter")
        report.extend(root_clutter)
        report.append("")
    summary['root_clutter'] = len(root_clutter)

    # Final Report Construction
    timestamp = now.strftime("%Y-%m-%d %H:%M")
    header = [
        f"# Audit Report — {timestamp}",
        "",
        f"**Memory:** {summary['mem_total']} files total (cap: 60). Orphans: {summary['mem_orphans']}. Stale: {summary['mem_stale']}. Oversized: {summary['mem_oversized']}.",
        f"**Staging:** {summary['staging_total']} items. Orphans: {summary['staging_orphans']}.",
        f"**Archive:** {summary['archive_intruders']} intruders.",
        f"**Root:** {summary['root_clutter']} clutter items.",
        "",
        "---",
        ""
    ]
    
    final_content = "\n".join(header + report)
    if not report:
        final_content += "All clean.\n"
        
    output_path = Path(output_file)
    output_path.parent.mkdir(parents=True, exist_ok=True)
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(final_content)
        
    print("Audit complete.")
    print(f"  Memory: {summary['mem_total']} files (orphans: {summary['mem_orphans']}, stale: {summary['mem_stale']}, oversized: {summary['mem_oversized']})")
    print(f"  Staging: {summary['staging_total']} items (orphans: {summary['staging_orphans']})")
    print(f"  Archive: {summary['archive_intruders']} intruders")
    print(f"  Root: {summary['root_clutter']} clutter")
    print(f"Report -> {output_file}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--memory-dir", default=r"C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory")
    parser.add_argument("--repo-dir", default=".")
    parser.add_argument("--output", default=r"STAGING\_AUDIT_REPORT.md")
    args = parser.parse_args()
    
    run_audit(args.memory_dir, args.repo_dir, args.output)
