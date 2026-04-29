import os

filepath = os.path.join(os.path.dirname(os.path.dirname(os.path.abspath(__file__))), 'ART', 'URETIM_PLANI.md')
outpath = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'audit_result.txt')

with open(filepath, 'r', encoding='utf-8') as f:
    lines = f.readlines()

results = []

results.append("=== Create S-M image kullanan satirlar ===")
for i, line in enumerate(lines, 1):
    if '"Create S-M image"' in line:
        results.append(f"  L{i}: {line.strip()[:120]}")

results.append("")
results.append("=== Create M-XL image kullanan satirlar ===")
for i, line in enumerate(lines, 1):
    if '"Create M-XL image"' in line:
        results.append(f"  L{i}: {line.strip()[:120]}")

results.append("")
results.append("=== Animate with text - (New) olmadan kalan ===")
for i, line in enumerate(lines, 1):
    if 'Animate with text' in line and '(New)' not in line:
        # exclude the header doc section
        if 'ANIMATE' not in line:
            results.append(f"  L{i}: {line.strip()[:120]}")

results.append("")
results.append(f"Toplam satir: {len(lines)}")

with open(outpath, 'w', encoding='utf-8') as f:
    f.write('\n'.join(results))

print(f"Sonuc yazildi: {outpath}")
