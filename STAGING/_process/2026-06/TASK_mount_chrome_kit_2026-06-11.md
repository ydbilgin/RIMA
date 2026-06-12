# TASK: Mount ChatGPT UI Chrome Kit (KIT A + B2) — 2026-06-11

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS gerekmez (asset import işi).

## Görev
ChatGPT'nin ürettiği 9 UI chrome slice'ını Unity projesine import et, 9-slice border'larını ayarla, SpriteAtlas oluştur. Live UI'a BAĞLAMA — bu ayrı task. Sadece import + 9-slice + atlas + rapor.

## Kaynak dosyalar (NATIVE sürüm, 512scale DEĞİL)
```
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_A/minimap_frame.png   (576x416)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_A/node_frame.png      (224x224)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_A/tooltip_box.png     (336x240)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_A/reward_card.png     (352x464)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_B2/slot_normal.png    (160x160)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_B2/slot_active.png    (160x160)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_B2/slot_lmb_rmb.png   (192x192)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_B2/ribbon_base.png    (360x88)
STAGING/_process/2026-06/chatgpt_chrome_v1/slices/KIT_B2/menu_button.png    (480x104)
```

## Hedef klasör
`Assets/Sprites/UI/Chrome/` (yoksa oluştur)

## Import ayarları (HER dosya)
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 64
- Mesh Type: Full Rect
- Filter Mode: Point (no filter)
- Compression: None
- Generate Mip Maps: OFF
- Alpha Is Transparency: ON

## 9-Slice Border (Sprite Editor Border L/R/T/B — hepsi eşit, native 2x değerleri)
| Dosya | Border (px) |
|---|---|
| minimap_frame | 56 |
| node_frame | 44 |
| tooltip_box | 44 |
| reward_card | 56 |
| slot_normal | 48 |
| slot_active | 48 |
| slot_lmb_rmb | 56 |
| ribbon_base | 36 |
| menu_button | 40 |

(Kaynak: `STAGING/_process/2026-06/chatgpt_chrome_v1/cutlist_and_9slice.json` → `suggested_9slice_border_px_2x`)

## SpriteAtlas
`Assets/Sprites/UI/Chrome/UI_Chrome.spriteatlas` oluştur, 9 sprite'ı ekle. Include in Build: ON. Filter: Point, Compression None, Padding 4.

## Live UI'a bağlama — YAPMA (ayrı task)
Sadece şu eşlemeyi RAPORLA (kod yazma): hangi chrome hangi mevcut UI bileşenine bağlanmalı?
- slot_normal/active/lmb_rmb → `SkillBarUI` slot background
- ribbon_base → reward rarity ribbon
- minimap_frame → minimap panel
- node_frame → map node
- reward_card → reward card
- menu_button → pause/menu button
- tooltip_box → TooltipSystem

## Başarı Kriteri
1. 9 PNG `Assets/Sprites/UI/Chrome/` altında, doğru import ayarları + 9-slice border
2. `UI_Chrome.spriteatlas` oluşturuldu, 9 sprite içeriyor
3. Unity console: compile/import error yok
4. Mevcut UI'a kod bağlantısı YAPILMADI (rapor edildi)

## Commit
```
feat(ui): import ChatGPT UI chrome kit (9 slices) + 9-slice borders + SpriteAtlas
```
