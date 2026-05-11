# RIMA 2.5D Gecis — LOCKED Kararlar (2026-05-11)

Opus onaylı, nihai karar belgesi.

## Mimari

| # | Karar | Secim | Tarih |
|---|---|---|---|
| 1 | Unity Proje Stratejisi | Yeni URP 3D + salvage manifest | 2026-05-11 |
| 2 | Silah Yaklasimi | Saf 2D sprite child + per-frame hand anchor atlas | 2026-05-11 |
| 3 | Animasyon Yon Sayisi | 4 yon + yatay flip (Animation Bible revize) | 2026-05-11 |
| 4 | Anchor Guncelleme | Body-only 16 anchor (4 class x 4 yon), eski → _archive | 2026-05-11 |
| 5 | Silah Fizik Sistemi | AnchorFollow (idle/walk) + AttackDriven (attack) + BoxCollider3D | 2026-05-11 |
| 6 | Room Designer | ITileWriter abstraction → 3D prefab; brush/palette/save korunur | 2026-05-11 |

## Silah Anchor Sistemi

| Alan | Karar |
|---|---|
| Body animasyon | SILAHSIZ uretim, "empty hands gripping" pose |
| Silah sprite PPU | 64 (body ile esit, ZORUNLU) |
| Silah canvas | 128x128 (one-handed) / 192x192 (two-handed/ranged) |
| Hand Anchor | Magenta-dot → WeaponAnchorMap ScriptableObject |
| Senkron | Animator.normalizedTime, code-driven curve |
| Sorting | S/E/W: body+1; N: body-1; mid-swing: keyframe |

## Degismeyen Sistemler

- PixelLab pipeline (boyutlar, PPU, chromakey) — AYNI
- Karakter view acisi (~30-40 deg) — AYNI, 2.5D uyumlu
- Skill sistemi, Shadow Echo, Hub, DungeonGraph, UI — AYNI
- animate_character MCP yasagi — DEVAM
- Start Frame kurali — DEVAM

## Revize Edilen LOCKED Kararlar

- Animation Bible 8 yon → 4 yon + flip (REVIZE)
- IsometricZAsY grid → REVOKE (3D kare grid)
- RuleTile / Domain Warp → REVISION PENDING (3D shader esdegeri)
- Weapon pass pipeline → SUPERSEDED (ayrik silah sistemi)

## POC Onkosulu

Yeni Room Designer kodu yazilmadan once:
- 1 oda (3D Plane + Cube) + 1 billboard karakter + 1 point light
- Pixel-perfect + lighting dogrulanmali
- POC PASS olmadan: Room Designer port baslamaz, anim uretimi baslamaz

## Salvage Manifest (Taslak)

Yeni projede KULLANILACAKLAR:
- Assets/Scripts/Runtime/ — tum mantik (2D→3D API conversion)
- Assets/Scripts/Editor/RoomDesigner/ — brush/palette/save mantigi
- PIXELLAB_OUTPUTS/ — tum sprite ciktilar
- TASARIM/, MEMORY/, GUIDES/ — tum tasarim dokumanlari
- Characters/anchors/ — stil referans (yeni body-only set eklenecek)

Yeni projede KULLANILMAYACAKLAR:
- Tilemap/Grid setup
- Physics2D layer config
- Isometric sorting layer setup
- LegacyRuntimeRoomManager
