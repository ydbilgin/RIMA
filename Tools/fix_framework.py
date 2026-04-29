import sys, re

with open('GUIDES/CHARACTER_IDENTITY_FRAMEWORK.md', encoding='utf-8') as f:
    content = f.read()

# 1. Fix Sira arrows
content = content.replace(
    'Warblade ? Elementalist ? Gunslinger ? Ravager ? Brawler ? Ranger ? Ronin ? Shadowblade ? Summoner ? Hexer',
    'Warblade \u2192 Elementalist \u2192 Gunslinger \u2192 Ravager \u2192 Brawler \u2192 Ranger \u2192 Ronin \u2192 Shadowblade \u2192 Summoner \u2192 Hexer'
)

# 2. Camera rule: allow proportion hints
content = content.replace(
    "- **Create from Template Pro kullan\u0131l\u0131yorsa:** Camera View \u2192 `high top-down` UI'dan se\u00e7. Prompta yazma.",
    "- **Create from Template Pro kullan\u0131l\u0131yorsa:** Camera View \u2192 `high top-down` UI'dan se\u00e7. Proportion/visibility hints description'a eklenebilir."
)
content = content.replace(
    "- **Kamera metni Description'a yaz\u0131lmaz** \u2014 UI se\u00e7imi yeterli",
    "- **Kamera a\u00e7\u0131 metni Description'a yaz\u0131lmaz** \u2014 UI se\u00e7imi yeterli. Proportion hints izinli."
)

# 3. Description Prompt Kuralı
old_rule = "### Description Prompt Kural\u0131\n- Kamera metni prompta yaz\u0131lmaz \u2014 UI'dan se\u00e7ilir\n- `volumetric body forms` her promptun sonunda kal\u0131r\n- `mature proportions, not short not squat, readable leg length` \u2014 t\u00fcm class'larda zorunlu\n- Tek sat\u0131r yaz \u2014 \u00e7ok sat\u0131rl\u0131 format kopyalamay\u0131 zorla\u015ft\u0131r\u0131r"
new_rule = "### Description Prompt Kural\u0131\n- Kamera a\u00e7\u0131 metni prompta yaz\u0131lmaz \u2014 UI'dan se\u00e7ilir\n- Promptlar k\u0131sa ve odakl\u0131 \u2014 Concept Image geri kalan\u0131 ta\u015f\u0131r\n- Proportion sorunu varsa: `full body visible head to boots`, `longer legs` ekle\n- Tek sat\u0131r yaz \u2014 \u00e7ok sat\u0131rl\u0131 format kopyalamay\u0131 zorla\u015ft\u0131r\u0131r"
if old_rule in content:
    content = content.replace(old_rule, new_rule)
    print('Description rule: updated')
else:
    print('Description rule: not found')

# 4. Ravager lock: dual axes
content = content.replace(
    '- **B\u00fcy\u00fck \u00e7entikli balta** (notched, dried blood) zorunlu',
    '- **Dual b\u00fcy\u00fck \u00e7entikli balta** (notched, dried blood, one in each hand) zorunlu'
)

# 5. Warblade Armor text — find by anchor
# The garbled section contains "Kask" in some form
# Replace full Armor/clothing line
idx = content.find('**Armor / clothing:**')
if idx != -1:
    # Find the next ** section
    next_section = content.find('\n\n**', idx+1)
    armor_block = content[idx:next_section]
    if 'Kask' in armor_block or 'kask' in armor_block.lower():
        # Replace the whole armor block
        new_armor = '**Armor / clothing:** Yamanan zırh parçaları — göğüs plakası + omuzluklar gerçek metal, altında kalın gambeson + zincir. Bare head — kask yok, yüz açık. Pelerin: koyu kızıl, yıpranmış, kirli, pratik. Renk: soluk siyah, paslanmış demir, mat altın.'
        content = content[:idx] + new_armor + content[next_section:]
        print('Warblade armor: updated')
    else:
        print('Warblade armor: kask not found in block')

# 6. Warblade QC — find by anchor
qc_idx = content.find('[ ] Kask')
if qc_idx != -1:
    # Find start of QC block
    block_start = content.rfind('```', 0, qc_idx)
    block_end = content.find('```', qc_idx)
    old_qc_block = content[block_start:block_end+3]
    new_qc_block = '''```
[ ] Bare head — kask/visor/miğfer yok mu?
[ ] Pelerin yıpranmış/pratik, dramatik değil mi?
[ ] Kılıç çatlakları ince ve içsel — glowing değil?
[ ] Bacaklar görünüyor mu — squat/cüce değil mi?
[ ] Karakter hacimli — flat/sticker değil?
```'''
    content = content.replace(old_qc_block, new_qc_block)
    print('Warblade QC: updated')
else:
    print('Warblade QC: kask reference not found')

with open('GUIDES/CHARACTER_IDENTITY_FRAMEWORK.md', 'w', encoding='utf-8-sig') as f:
    f.write(content)
print('Done.')
