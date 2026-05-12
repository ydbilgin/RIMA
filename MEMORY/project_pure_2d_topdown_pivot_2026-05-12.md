---
name: Pure 2D Top-Down Pivot — 2.5D Detour Reverted
description: 2026-05-12 — RIMA mimari pivotu. 2.5D denemesi (3D env + 2D billboard) terkedildi, pure 2D top-down chibi 64px secildi. Eski RIMA projesine geri donus.
type: project
---

# Pure 2D Top-Down Pivot — 2026-05-12 (S59)

## Karar

**RIMA = Pure 2D Top-Down chibi pixel art ARPG roguelite.**

- Karakter: 64x64 chibi sprite
- Tile: 32x32 top-down grid
- VFX: 64-128px mix
- Mimari: URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- View: High top-down ~30-35° (Hades match, eski S57'den KEEP)
- Yon: 4 yon (N/S/E, W=flipX) MVP, 8 yon sonra

## Why

**2.5D detour (S57-S58) basarisiz:**
1. 128px detayli PixelLab karakter generation tutarsiz (AI variance yuksek)
2. ChatGPT 30° anchor → PixelLab Init Image workflow karakter outfit'ini koruyamiyordu
3. KayKit chibi 3D + Blender render = AD aesthetic'e uymadi (cok cute, dark fantasy degil)
4. Reallusion CC Bases alternatifi (yetiskin proporsiyon) icin Blender modelleme + texture workflow gerek — solo dev'e fazla
5. 3D environment + 2D sprite billboard = 3 sistem aynia anda (production cost x3)
6. Procedural oda generation 3D'de cok zor (mesh + Z-fighting + lighting bake)

**Pure 2D Top-Down dogru cevap cunku:**
1. PixelLab'in en gucu 32-64px chibi pixel art uretiminde (Pixen modeli native)
2. PixelLab Create Character — 4 yon + animasyon native tek workflow
3. PixelLab Create Tiles — tile generation enterprise-grade
4. Tek pipeline = tek tutarli stil
5. Procedural oda 2D tilemap ile cok daha kolay (RoomBlueprint zaten 2D mantiginda)
6. Hammerwatch, Hyper Light Drifter, CrossCode kanitlanmis stil
7. Eski RIMA projesinde tum infra mevcut (Scripts/Player + Combat + Skills + Enemies + Map + Systems, Room Designer F2 COMPLETED)

## How to apply

- **PixelLab kullanimi:** Create Character (4 yon + idle + walk + attack + death = ~30-40 gen per karakter), Create Tiles (200+ tile ~300 gen), Create Image (props 150+ ~200 gen), Animate Skeleton (mob anim 16x ~400 gen). Toplam ~3500 gen, 5000 budget'tan icerde.
- **3D pipeline (Blender / KayKit) artik gereksiz:** Mob poz referansi icin (animate skeleton) opsiyonel.
- **Eski RIMA infra korunur:** PlayerController, CameraFollow, PlayerAttack, Skills, Enemies, Map, Room Designer F2 hepsi calismaya devam eder. Sadece tile boyutu (64x64 → 32x32) ve sprite yon (4 → 4 KEEP) standardize edilir.
- **Yeni asset spec (sprite boyutlari):** 64x64 char, 32x32 tile, 64-128 VFX, 32-64 icon. PPU=64. Filter=Point, no compression, no mipmap.
- **Renderer setup:** Project Settings → Graphics → URP 2D Renderer Asset. Main Camera → Orthographic + Pixel Perfect Camera component. Scene → URP 2D Renderer + Light2D (Global Ambient + Point lights for atmosphere).

## Referans Oyunlar

- **Hammerwatch** — chibi top-down ARPG dungeon, RIMA gameplay'ine en yakin
- **Hyper Light Drifter** — atmosferik dark pixel top-down (RIMA aesthetic target)
- **CrossCode** — top-down pixel ARPG, derinlik sprite katmanlama ile
- **Eitr** — kayboldu ama estetik birebir RIMA
- **Hammerwatch 2** — pixel art top-down ARPG, ornek
- **Brotato / Vampire Survivors** — kucuk chibi + dev VFX = juice formulu

## REVOKE'lar (S57-S58 kararlari)

| Eski Karar | REVOKE Tarihi |
|---|---|
| Render mimarisi: 2.5D (3D env + 2D Billboard) | 2026-05-12 |
| Unity proje: Yeni URP 3D, mevcut arsiv | 2026-05-12 |
| Tilemap KALDIRILACAK | 2026-05-12 (RESTORE) |
| Dungeon mimari: 2.5D kare grid arena | 2026-05-12 |
| Body-only anchor 128px (V1 ChatGPT 30° ref pipeline) | 2026-05-12 (chibi 64px native uretim) |
| KayKit/Blender pre-render pipeline | 2026-05-12 (gereksiz) |
| Duvar boyutu 64x128 (isometric) | 2026-05-12 (32x32 top-down) |
| Zemin boyutu 64x64 (isometric) | 2026-05-12 (32x32 top-down) |

## RIMA_2.5D Nested Folder

`F:\Antigravity Projeler\2d roguelite\RIMA\RIMA_2.5D\` — taşınacak:
- Hedef: `F:\Antigravity Projeler\2d roguelite\RIMA\_ARCHIVE\RIMA_2.5D_attempt_2026-05-11\`
- Rationale: 1 günlük 2.5D denemesi, ders cikarildi (asset variance + tooling complexity), 2D'ye geri donuldu

Ayrica sibling `F:\Antigravity Projeler\2d roguelite\RIMA_2.5D\` da arsivlenecek (ayni proje, iki konum).

## Sonraki Adim

1. `/lint` ile dokuman tutarsizliklarini tara
2. NLM sorgusu ile knowledge base'deki S58 onceki kararlari guncelle
3. UnityMCP'yi eski RIMA projesine yonlendir (`_IsoGame.unity` veya `_Sandbox.unity`)
4. PixelLab Create Character ile ilk 64x64 chibi Warblade
5. `_Sandbox.unity`'ye yerlestir + mevcut PlayerController.cs test
