# RIMA — UI Chrome Kit Brief (ChatGPT için)

## Ne istiyorum
Ekteki **blueprint PNG'leri** RIMA oyununun UI chrome'unun **yerleşim haritası** (wireframe). Her kutu bir UI parçasının canvas'taki yeri + boyutu + köşe yuvarlaklığı (r). Senden bu kutuları **bitmiş dark-fantasy pixel-art UI chrome** olarak çizmeni istiyorum — aynı layout, aynı boyutlar.

## Oyun stili
- 2D top-down pixel-art roguelite (Hades / Dead Cells tonu)
- **Gerçek pixel-art** (anti-aliased/blur DEĞİL), keskin kenar
- Palet (SADECE bunlar + tonları):
  - void mor `#3A1A4A` (panel zemini)
  - ember turuncu `#E89020` (kenar/vurgu/köşe süsü)
  - slate `#3A3D42` (metal/nötr)
  - cyan `#00FFCC` **KULLANMA** (sadece kodda aktif-state, ≤%15)

## Chrome kuralı (önemli)
Bunlar **içi boş çerçeve/kap** — içerik (ikon, yazı, bar dolgusu) sonradan koddan gelecek. Yani:
- Çerçevelerin **içi boş/şeffaf** olmalı
- Barların **içinde renkli dolgu OLMAMALI** (sadece boş kap + uç süsleri)
- Slot'ların **içi boş** (ikon sonra konacak)
- Butonlarda **yazı yok** (TMP ile basılacak)
- **9-slice uyumlu:** köşe süsleri köşede kalsın, kenarlar düz olsun (panel uzayınca bozulmasın)
- **Şeffaf arka plan**, her parça ayrı kesilebilsin (aralarında boşluk var)

## Çıktı formatı
Tercihen: blueprint'teki layout'u koruyan **tek şeffaf sheet** (kutular aynı yerde, aynı boyutta) VEYA her parça ayrı transparent PNG. İsimlendirme blueprint'teki `role` ile aynı.

---

## KIT A — Frames (canvas 512×512)
**Stil tarifi:**
```
Dark fantasy pixel-art UI frames, forged dark iron with carved arcane runes, deep void purple #3A1A4A base, ember #E89020 rune-glow accents, slate grey #3A3D42 metal. Thick uniform borders, completely hollow empty interiors, corner ornaments confined to corners, edges kept flat and featureless for clean 9-slice tiling. No text, no icons inside. Transparent background.
```
| role | tip | boyut | r | konum (x,y) | → oyunda |
|---|---|---|---|---|---|
| minimap_frame | Window | 288×208 | 8 | 16,16 | minimap çerçevesi |
| node_frame | Panel | 112×112 | 8 | 336,16 | harita node çerçevesi |
| tooltip_box | Panel | 168×120 | 8 | 320,160 | skill tooltip kutusu |
| reward_card | Panel | 176×232 | 8 | 16,256 | ödül kartı çerçevesi |

## KIT B1 — Bars (canvas 688×384)
**Stil tarifi:**
```
Dark fantasy pixel-art UI bar frames, forged dark iron and slate, void purple #3A1A4A base, ember #E89020 bevel accents, slate grey #3A3D42. Thin hollow horizontal containers, empty interior with NO inner fill, ornate end caps only, flat stretchable middle for 9-slice. No text. Transparent background.
```
| role | tip | boyut | r | konum | → oyunda |
|---|---|---|---|---|---|
| boss_hp_bar | Panel | 420×30 | 6 | 16,24 | boss can barı |
| player_hp_bar | Panel | 300×24 | 6 | 16,90 | oyuncu can barı |
| resource_bar | Panel | 300×18 | 6 | 16,150 | mana/resource barı |
| xp_bar | Panel | 360×14 | 4 | 16,205 | tecrübe barı |

## KIT B2 — Slots & Buttons (canvas 512×512)
**Stil tarifi:**
```
Dark fantasy pixel-art UI elements, forged dark iron and slate, void purple #3A1A4A base, ember #E89020 bevel accents, slate grey #3A3D42. Square icon slots with thick inset hollow frames, flat horizontal button surfaces, neutral grey tintable base, corner and edge details kept flat for 9-slice. No text, no icon inside. Transparent background.
```
| role | tip | boyut | r | konum | → oyunda |
|---|---|---|---|---|---|
| slot_normal | Icon button | 80×80 | 8 | 16,16 | skill slot |
| slot_active | Icon button | 80×80 | 8 | 112,16 | aktif skill slot |
| slot_lmb_rmb | Icon button | 96×96 | 8 | 208,16 | LMB/RMB slot (büyük) |
| ribbon_base | Button | 180×44 | 6 | 16,140 | rarity şeridi (nötr, tint koddan) |
| menu_button | Button | 240×52 | 8 | 16,220 | menü/seç butonu |

## KIT C — Menu chrome (canvas 512×512) — düşük öncelik
**Stil tarifi:** (KIT A ile aynı palet + flat-edge dili)
| role | tip | boyut | r | konum | → oyunda |
|---|---|---|---|---|---|
| codex_tab_normal | Tab | 200×52 | 6 | 16,16 | codex class sekmesi |
| codex_tab_selected | Tab | 200×52 | 6 | 16,84 | seçili sekme |
| portrait_frame | Avatar | 120×120 | 0 | 16,160 | portre çerçevesi |
| pause_button | Button | 300×56 | 8 | 160,160 | pause buton |
| pause_main_panel | Panel | 220×260 | 8 | 160,240 | pause ana panel |

---

## Ekli dosyalar
- `KIT_A_Frames.png` · `KIT_B1_Bars.png` · `KIT_B2_Slots.png` · `KIT_C_Menu.png` — wireframe layout haritaları (hedef yerleşim + boyut)
