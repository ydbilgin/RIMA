import re
import os

filepath = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), 'ART', 'URETIM_PLANI.md')
print(f"Reading: {filepath}")

with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

original_lines = content.split('\n')

# 1. Fix 'Animate with text' -> 'Animate with text (New)' (only those without New already)
content = re.sub(r'"Animate with text"(?! \()', '"Animate with text (New)"', content)

# 2. Fix FAZ 2 comment
content = content.replace('Create S-M image (64\u00d764 karakterler)', 'Create M-XL image (64\u00d764 karakterler)')

# 3. Fix Hub NPC comment
content = content.replace('64\u00d764, Create S-M image, animasyon yok', '64\u00d764, Create M-XL image, animasyon yok')

# 4. Fix Fracture-Born table comment
content = content.replace('PixelLab Create S-M \u2192 Export', 'PixelLab Create M-XL image \u2192 Export')

# 5. Replace Action Description -> Animation Action
content = content.replace('**Action Description**', '**Animation Action**')

# 6. Replace No Background -> Remove Background
content = content.replace('**No Background**', '**Remove Background**')

# Process lines for context-aware fixes
lines = content.split('\n')
new_lines = []
context_size = 0
in_animate_section = False

for i, line in enumerate(lines):
    # Track canvas size from File New
    if '\u00d7' in line or 'x' in line.lower():
        for sz_str, sz in [('64\u00d764', 64), ('128\u00d7128', 128), ('32\u00d732', 32), ('16\u00d716', 16),
                           ('48\u00d748', 48), ('200\u00d720', 200), ('80\u00d7120', 120), ('64\u00d732', 64),
                           ('16\u00d732', 32)]:
            if sz_str in line and ('New' in line or 'File' in line or 'Width' in line):
                context_size = sz
                break

    # Track sections by header keywords
    section_map = [
        ('# WARBLADE', 64), ('# ELEMENTALIST', 64), ('# SHADOWBLADE', 64), ('# RANGER', 64),
        ('TWICE-BORN', 64), ('Fracture-Born', 64), ('FRACTURE-BORN', 64),
        ('FERRYMAN', 64), ('VREL', 64), ('SISTER MOURNE', 64), ('CARTOGRAPHER', 64),
        ('SHARD WALKER', 32), ('VOID THRALL', 32), ('SEAM CRAWLER', 32),
        ('ECHO HOUND', 32), ('HOLLOW MITE', 32),
        ('IRON WARDEN', 128), ('FRACTURED KING', 128), ('HOLLOW SOVEREIGN', 128), ('NEXUS CORE', 128),
        ('HIT SPARK', 32), ('SWORD TRAIL', 64),
        ('SKILL SLOT', 48), ('HP BAR', 200), ('RAGE BAR', 200), ('SKILL DRAFT', 120),
        ('FLOOR TILE', 16), ('WALL TILE', 16), ('CRACK OVERLAY', 16),
    ]
    stripped = line.strip()
    if stripped.startswith('#'):
        for kw, sz in section_map:
            if kw in line:
                context_size = sz
                break
    # Also check non-header lines for section context
    for kw, sz in section_map:
        if kw in line and ('##' in line or '---' in line or 'ADIM' in line):
            context_size = sz
            break

    # Fix Create S-M -> Create M-XL for 64+ pixel context
    if '"Create S-M image"' in line and context_size >= 64:
        line = line.replace('"Create S-M image"', '"Create M-XL image"')

    # Track animate sections
    if 'Animate with text' in line and ('ADIM' in line or 'tıkla' in line or 'Edit' in line):
        in_animate_section = True
    if stripped.startswith('### ADIM') and 'Animate' not in line:
        in_animate_section = False
    if stripped == '---':
        in_animate_section = False

    # Remove Description lines in animate sections (when followed by Animation Action)
    if in_animate_section and '**Description**:' in line and '**Animation Action**' not in line:
        j = i + 1
        while j < len(lines) and lines[j].strip() == '':
            j += 1
        if j < len(lines) and '**Animation Action**' in lines[j]:
            continue

    # Remove Camera View + Direction standalone lines in animate sections
    if in_animate_section and '**Camera View**' in line and '**Frame Count**' not in line:
        if '**Direction**' in line:
            continue

    # Fix combined Camera+Direction+Frame Count lines in animate
    if in_animate_section and '**Camera View**' in line and '**Frame Count**' in line:
        match = re.search(r'\*\*Frame Count\*\*\s*\u2192\s*\*\*(\d+)\*\*', line)
        if match:
            fc = match.group(1)
            indent = line[:len(line) - len(line.lstrip())]
            line = f'{indent}- **Frame Count** \u2192 **{fc}** | **Remove Background** \u2192 ON'

    # Remove standalone Output Method lines in animate
    if in_animate_section and stripped.startswith('- **Output Method**'):
        continue

    # Remove Output Method from end of lines
    line = re.sub(r'\s*\|\s*\*\*Output Method\*\*\s*\u2192\s*\*\*"New layer"\*\*', '', line)

    new_lines.append(line)

content = '\n'.join(new_lines)

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)

final_lines = content.split('\n')
changes = sum(1 for a, b in zip(original_lines, final_lines) if a != b)
removed = len(original_lines) - len(final_lines)
print(f'Modified lines: {changes}')
print(f'Removed lines: {removed}')
print(f'Total lines original: {len(original_lines)}')
print(f'Total lines now: {len(final_lines)}')
