# RIMA — Art Direction Master Spec
**LOCKED 2026-05-08 | Gemini 3.1 Pro + Claude analysis confirmed**

---

## Hedef Stil: JVerbroucht 16-bit Isometric Pixel Art

**Referans:** @jverbroucht (Twitter/X) — temiz, sınırlı palette, grid'e tam oturan, chunky piksel, 16-bit retro izometrik.

### Temel Kurallar (Strict — ihlal edilemez)

| Kural | Detay |
|---|---|
| PPU | 16 (tüm sprite ve tile'lar) |
| Tile boyutu | 64×64 (floor), 64×96 (wall) |
| Pixel yoğunluğu | Karakter = çevre = prop: AYNI chunky piksel yoğunluğu |
| Gradient | YOK — sadece dithering (checker pattern) ile geçiş |
| Anti-aliasing | YOK — sert piksel kenarlar |
| Palette | Sınırlı (per-set max 24-32 renk), soğuk mavi-gri dominant |
| Isometric açı | 2:1 ratio, standart 26.57° — her asset aynı açı |
| Işık kaynağı | Sol üst — tüm asset'lerde tutarlı |

---

## Palette — Shattered Keep (Act 1)

| Rol | HEX | Kullanım |
|---|---|---|
| Floor shadow edge | `#0e0f14` | Tile kenar, derin gölge |
| Stone dark | `#161820` | Taş ana koyu ton |
| Stone mid | `#1e2030` | Taş orta ton |
| Stone light | `#262838` | Taş aydınlık yüz |
| Mortar joint | `#12141a` | Duvar/zemin arası çizgi |
| Moss dark | `#1a2810` | Yosun gölge |
| Moss mid | `#263a1a` | Yosun orta |
| Metal dark | `#181820` | Demir/halka |
| Metal mid | `#24262e` | Demir aydınlık |
| Rift cyan dark | `#0a2030` | Cyan rift gölge |
| Rift cyan mid | `#0a4060` | Cyan kristal gövde |
| Rift cyan bright | `#20c0e0` | Cyan rift parlaklık |
| Torch warm | `#8b3a00` | Meşale ateş koyu |
| Torch bright | `#ff8c00` | Meşale ateş parlak |

---

## Üretim Kılavuzu — ChatGPT Prompt Şablonu

### Sabit Prefix (her asset için ekle)
```
Strict 16-bit isometric pixel art, chunky pixels, hard pixel edges, NO anti-aliasing,
NO smooth gradients (dithering only), limited color palette (max 24 colors),
consistent top-left light source, dark medieval dungeon Shattered Keep aesthetic,
cold blue-grey stone palette (#1e2030 base), 2:1 isometric ratio.
Background: solid magenta #FF00FF.
```

### Floor Tile Şablonu (64×64 diamond)
```
[PREFIX] Isometric floor tile, diamond shape 2:1 ratio, 64px.
Stone: 2x2 cut masonry blocks, mortar joints #12141a 1px.
[VARYASYON TANIMI]
```

### Wall Tile Şablonu (64×96 front face)
```
[PREFIX] Isometric wall tile, front face of stone wall block, 64x96px portrait.
Top strip 8px lighter stone #262838. Front face 2x3 masonry grid, mortar #12141a.
Left shadow edge 4px #0e0f14. No isometric depth visible — pure front face.
[VARYASYON TANIMI]
```

### Wall Corner Şablonu (64×96 L-shape)
```
[PREFIX] Isometric wall corner tile, L-shaped stone wall where two walls meet at 90°,
64x96px. Left wall face and front wall face both visible. Corner edge sharp pixel line.
Same masonry as wall tiles. [VARYASYON TANIMI]
```

### Prop Şablonu (isometric sprite, Y-sorted)
```
[PREFIX] Isometric dungeon prop, [BOYUT]px, transparent background (no chromakey needed).
[PROP TANIMI]
```

---

## Asset Üretim Listesi

### Floor (ChatGPT — magenta BG, process_tiles.py ile import)

| Batch | Durum | Codex Komutu |
|---|---|---|
| F1 Base (16 tile) | ✅ DONE — Assets/Art/Tiles/Act1/F1/ | — |
| F2 Cracked (16 tile) | Prompt HAZIR | `--cols 4 --rows 4 --prefix f2_` |
| F3 Mossy (12 tile) | Prompt HAZIR | `--cols 4 --rows 3 --prefix f3_` |

### Wall (ChatGPT — 64×96, magenta BG)

| Batch | Durum | Codex Komutu |
|---|---|---|
| W1 Base (12 tile, 4×3) | Prompt HAZIR | `--cols 4 --rows 3 --width 64 --height 96 --prefix w1_` |
| W2 Damaged (8 tile, 4×2) | Prompt HAZIR | `--cols 4 --rows 2 --width 64 --height 96 --prefix w2_` |
| W1-Corner L (4 tile, 2×2) | **EKSİK — üretilecek** | `--cols 2 --rows 2 --width 64 --height 96 --prefix w1c_` |
| W1-EndCap (4 tile, 2×2) | **EKSİK — üretilecek** | `--cols 2 --rows 2 --width 64 --height 96 --prefix w1e_` |

### Props — Old Asset Pack Re-Master (ChatGPT, transparent BG)

Eski asset pack şekil/konsept referans. Aynı obje → JVerbroucht stilinde yeniden üretilecek.

| Prop | Eski Asset | Hedef Boyut | Öncelik |
|---|---|---|---|
| Torch (meşale) | rima_act1_environment_sheet_alpha | 32×64 | YÜK |
| Cyan Crystal Cluster | rima_act1_environment_sheet_alpha | 64×96 | YÜK |
| Stone Pillar | rima_act1_environment_sheet_alpha | 32×96 | ORTA |
| Rubble Heap (moloz) | rima_act1_environment_sheet_alpha | 64×48 | ORTA |
| Shrine with Candles | rima_act1_environment_sheet_alpha | 64×96 | DÜŞÜK |

**Prompt şablonu — meşale örneği:**
```
Strict 16-bit isometric pixel art, chunky pixels, NO anti-aliasing, NO gradients, limited palette.
Isometric dungeon wall torch/brazier, 32x64px sprite, transparent background.
Iron bracket #181820 mounted on left side. Flame: orange #ff8c00 top, dark red #8b3a00 base.
Flame is 6x8px chunky animated-look flicker shape. Same stone palette as wall behind.
Top-left light source. Dark medieval dungeon style.
```

**Prompt şablonu — cyan kristal örneği:**
```
Strict 16-bit isometric pixel art, chunky pixels, NO anti-aliasing, dithering only.
Isometric dungeon cyan rift crystal cluster, 64x96px sprite, transparent background.
3-4 hexagonal crystal spires, tallest ~60px. Base: dark stone rubble.
Crystal: #0a4060 dark body, #20c0e0 bright edge highlight, #0a2030 shadow.
2px bright pixel dots floating above (rift energy). Top-left light source.
```

---

## UI Sanat Yönü Kararı

**KARAR: Ashen Glyph procedural UI — pixel art DEĞİL.**

Gerekçe:
- Hades / Dead Cells yaklaşımı: oyun dünyası pixel art, UI temiz geometrik
- Mevcut UI zaten tamamen code-driven procedural (commit `8e7a5f0`) — yeniden yapmaya gerek yok
- Pixel art UI ile pixel art dünya iç içe geçince okunabilirlik düşer
- Ashen Glyph: koyu cam panel + cyan accent + sharp pixel border = RIMA brand tutarlı

**UI elementleri için imge üretimi (istenirse):**
- Icon'lar (skill icon'ları, resource icon'ları) → pixel art, 32×32, limited palette
- Skill card art → pixel art portrait, 64×96
- Bu elementler için yine JVerbroucht stili uygulanır

---

## QC Protokolü (Her Batch Sonrası)

Her ChatGPT batch üretiminden sonra:
1. **Claude** → PNG'leri oku, palette + pixel yoğunluğu + isometric açı kontrol
2. **Codex** → process_tiles.py çalıştır, meta dosyaları üret, commit
3. **Gemini** (text QC) → stil tutarlılığı değerlendirmesi
4. PASS kriterleri: chunky piksel ✓, dithering ✓, palette ≤32 renk ✓, açı tutarlı ✓

---

## NotebookLM Sync Gerekli

Bu dosya + güncellenen CURRENT_STATUS.md → NLM'e sync edilmeli.
Sync tag: `nlm-sync-20260508-artdir`
