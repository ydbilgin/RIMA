---
name: camera-angle-direction-strategy-locked-s86-update
description: 30-35° 3/4 ARPG view with 8-direction sprites via 5-produce-3-mirror strategy. Weapons attached separately in Unity.
metadata: 
  node_type: memory
  type: project
  originSessionId: acfbcb3e-45ce-4896-b9be-0301b00dee90
---

# Camera Angle + 8-Direction Mirror Strategy — LOCKED (2026-05-16 S86)

## ANGLE LOCK (unchanged from S43)
- **Angle:** 30-35° 3/4 ARPG View (Diablo 2/Hades style)
- **Style anchor:** `rima_style_anchor.png` — Heavy dark outline, painterly weathered shading
- **Palette:** Muted slate/cold-blue base + warm amber/cyan accent
- **Banned:** High Top-Down (55-70° extreme)

## DIRECTION LOCK — UPDATED (S86 flip)

**Old (S43):** 4 Cardinal (S/E/N/W) + Diagonal Cheat
**New (S86):** **8-Direction LOCKED** with 5-produce-3-mirror production strategy

### 8 Directions
S, SE, E, NE, N, NW, W, SW

### Production Strategy (5 produce, 3 mirror)
| Direction | Production | Source |
|---|---|---|
| S | ✅ Produce | PixelLab Create Image Pro |
| N | ✅ Produce | PixelLab |
| E | ✅ Produce | PixelLab |
| SE | ✅ Produce | PixelLab |
| NE | ✅ Produce | PixelLab |
| **W** | 🔄 Mirror E (flipX) | Unity SpriteRenderer.flipX = true |
| **SW** | 🔄 Mirror SE (flipX) | Unity SpriteRenderer.flipX = true |
| **NW** | 🔄 Mirror NE (flipX) | Unity SpriteRenderer.flipX = true |

**Production cost: 5 sprite/character × 8 yön coverage = %38 daha az dispatch**

### Why Mirror Works
- Karakterler **silahsız** üretilir (body only)
- Silah ayrı sprite/prefab olarak Unity'de attach edilir (pattern: `Assets/Resources/Weapons/Warblade_Greatsword.png` + `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab`)
- Body mirror edilince child weapon SpriteRenderer de otomatik mirror (Unity transform inheritance)
- **Sonuç:** 5 body sprite × asymmetric weapon attachments = unlimited weapon variety

### Asymmetric Features = YASAK
Mirror düzgün çalışsın diye karakterler **simetrik** olmalı:
- ❌ Eyepatch (sadece tek gözde)
- ❌ Asymmetric armor (sadece sol omuzda pauldron)
- ❌ Tek tarafta yara izi
- ❌ Tek tarafta tattoo
- ✅ Symmetric clothing
- ✅ Symmetric face features
- ✅ Symmetric base body

**Asymmetric "imza" özellikler = SİLAH veya ACCESSORY üzerinden** (eyepatch prop, scar overlay, asymmetric belt buckle attached as separate sprite)

## PixelLab Create Image Pro Settings (S86)

### For Each Character Direction
- **Boyut:** 256×256 (single-direction body sprite)
- **View:** Low top-down (25-40°)
- **Detail:** Low/medium detail
- **Outline:** Single color (heavy dark)
- **Style image:** `rima_style_anchor.png` veya class-specific anchor

### Per-Direction Prompt Suffix
- S: "facing south, looking at camera"
- N: "facing north, back to camera"
- E: "facing east, profile right"
- SE: "facing south-east, ¾ profile right"
- NE: "facing north-east, ¾ profile right back"

## Existing Sprite Migration Plan

| Class | Status | Action |
|---|---|---|
| Warblade | 4-yön idle var (S/N/E/W) | Regen 8-yön (5 produce) — "içine sinmedi" listesi |
| Elementalist | 4-yön idle var | Regen 8-yön (5 produce) |
| Ranger | 4-yön idle var | Regen 8-yön (5 produce) |
| Shadowblade | 4-yön idle var | Regen 8-yön (5 produce) |
| **Gunslinger** | ✅ 8-yön zaten var | KEEP — yeni LOCK ile uyumlu |
| Ravager | ❌ sadece anchor | Üret 8-yön (5 produce) — Ravager balta dahil |
| Brawler | ❌ | Üret 8-yön (5 produce) |
| Ronin | ❌ | Üret 8-yön (5 produce) |
| Summoner | ❌ | Üret 8-yön (5 produce) |
| Hexer | ❌ | Üret 8-yön (5 produce) |

**Total üretim:** 9 karakter × 5 yön = 45 sprite (Gunslinger zaten var)
**Batch:** Önceki plan 5+5 → şimdi düzeltilmiş 5 batch × ~9 sprite (her batch 1-2 karakter)

## Cross-References
- [[8dir-mirror-production-strategy]] — production rule detayı
- [[128px-pivot-s43]] — 128px native, sprite size
- [[character-system]] — Body+Weapon separate, Aseprite composite (eski) → Unity composite (yeni)
- [[pixellab-character-via-web-ui-v3]] — character üretim web UI manuel
- [[class-genders]] — class gender list
- [[character-visual-identity]] — Hair/skin/clothing locks per class

## Migration Note
Mevcut 4-yön sprite'lar **legacy kalır** (script reference'lar bozulmasın), 8-yön regen tamamlandığında scriptler 8-yön referansa migrate edilir. Eski sprite'ları silmeden, parallel üret.
