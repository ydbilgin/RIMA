# Codex FINAL Review: Tile Tool Seçimi v2 (S66) — YENİ KANITLAR

**Tarih:** 2026-05-13
**Tip:** REVISION REVIEW — yeni kanıtlar geldi, önceki karar (v1) muhtemelen yanlış varsayım üzerine
**Çıktı:** CODEX_DONE.md'ye yeni bölüm append et

## v1 Özet (önceki kararın sonucu)

Önceki karar zinciri (NLM → Opus → Codex):
- KARAR: `create_image_pixen NEW (S-XL)` (web UI), single 32x32, izole tile + Unity RuleTile birleştir
- REDDEDİLDİ: `create_tiles_pro` MCP
- GEREKÇE: Karar #75 (Map Workshop tools autotile vermiyor)

Bu v1 kararı, eksik bilgi üzerine kuruluydu. Kullanıcı PixelLab UI'larından ekran görüntüleri paylaştı + MCP schema'sı detaylı incelendi + web araştırma yapıldı. Şimdi durum farklı.

## YENİ KANIT #1 — 3 UI Ekranı

PixelLab'da **iki ayrı tile pipeline** mevcut:

### (A) `Create tiles PRO` Tool (Image #1)
- 25 generation cost
- Description: numbered tile list (max ~12)
- Style tiles upload: up to 12 reference images (128x128 max)
- Tile type: hex / hex_pointy / isometric / octagon / **square_topdown**
- Tile size: 16-128px
- View angle: 0° (side) → 90° (top-down) slider
- Thickness: 0-100%
- Outline mode: outline / segmentation / no outline
- **Çıktı: izole varyant tile'lar** (autotile DEĞİL)

### (B) `Maps > Tileset` (Image #2 Standard, Image #3 Pro)
- Lower Terrain + Upper Terrain (text desc veya upload)
- Transition: None / Small 25% / Large 50% / Full 100%
- Pro mode: Gemini-powered, 20 generations, shape controls, transition height
- **Çıktı: Wang tileset, connected autotile, 16 veya 23 tile**

## YENİ KANIT #2 — MCP Schema (Tam Belgeleme)

### `mcp__pixellab__create_topdown_tileset`
> "Generate a **Wang tileset** for top-down game maps with **corner-based autotiling**.
> Returns **16 tiles** (transition_size < 1.0) or **23 tiles** (transition_size = 1.0).
> Vertices define terrain, tiles render based on their 4 corner values.
> Use case: Building top-down game maps with **seamless terrain transitions**."

Params:
- `lower_description`, `upper_description` (zorunlu)
- `transition_size` (0 / 0.25 / 0.5 / 1.0)
- `transition_description`
- `tile_size` (16 veya 32)
- `view` (low/high top-down)
- `detail` (low / medium / highly detailed)
- `shading` (5 level)
- `outline` (single color / selective / lineless)
- `text_guidance_scale`, `tile_strength`, `tileset_adherence`, `tileset_adherence_freedom`
- `lower_base_tile_id` / `upper_base_tile_id` — **connected tileset chain support** (ocean→beach, beach→grass, grass→stone şeklinde lore-tutarlı multi-tileset)

### `mcp__pixellab__create_tiles_pro`
> "Generates multiple tile **variations** by drawing tile shape outlines and having AI fill them. Two modes: Shape OR Style (12 ref tiles)."

- Description: "1) tile 2) tile 3) ..." numbered
- `style_images` JSON array (12 max, 128x128) — varyant üretim için style anchor
- `outline_mode: segmentation` = NO outline (cleaner)
- **Çıktı: numbered tile list, izole** — Wang DEĞİL

## YENİ KANIT #3 — Web Araştırma

PixelLab resmi dokümantasyon:
- Create Tileset Tool → Wang tileset, dual-grid 15-tileset, 3x3 tileset export
- "Smooth transitions between terrain types"
- "Autotile-like adjacency rules"

PixelLab 2026 Şubat review (jonathanyu.xyz):
- "Amazing tool, high quality, consistent 2D assets"
- Wang tilesets seamless

Sprite Fusion native support:
- Tilemap editor PixelLab tileset'leri native import ediyor
- Unity package / Godot / Defold export

## Karar #75 (LOCKED, 2026-05-04) Durumu

Eski metin:
> "PixelLab Map Workshop tool'ları (`create_topdown_tileset` vb.) YASAK. Discord deneyimi: ayrık tile veriyor, connected vermiyor."

**Yeni delillerle çelişen kısımlar:**
1. `create_topdown_tileset` aslında Wang autotile veriyor (MCP schema + web docs net)
2. Discord deneyimi muhtemelen ESKİ versiyon veya `create_tiles_pro` ile karıştırılmış
3. Pro mode (Gemini-backed) Şubat 2026'da yüksek kalite onayı almış

**Olası senaryo:** Karar #75 doğru tool'u (`create_tiles_pro` = izole varyant) yanlış etiketlemiş (`create_topdown_tileset` = Wang autotile). İki tool farklı amaçlar için — biri yasaklanırken diğeri de kapsam dışı kalmış olabilir.

## v1 Kararının Sorunları

1. `create_image_pixen NEW` single-tile akışı, izole 32x32 üretir → **autotile MANUEL** yapılır (Unity tarafında 8 wall + 16 corner manuel kurulum, saatler sürer)
2. WallAutoConnect.cs 4-bit NSEW maskeli 8 variant zaten implement edildi → ama tile assetleri yok, manuel üretmek 1-2 gün
3. `create_topdown_tileset` 16-23 Wang tile'ı tek generation'da veriyor (~100 saniye) → tüm autoconnect set hazır

## Senin Görevin (Codex) — v2 FINAL REVIEW

**3 soruya net cevap ver:**

### Q1: Karar #75 revize edilmeli mi?
- EVET / KISMEN / HAYIR
- Eğer EVET/KISMEN: Yeni Karar metni öner. Hangi tool yasak (sadece `create_tiles_pro` mı?), hangisi onaylı (`create_topdown_tileset` Pro mode?)

### Q2: F1 Shattered Keep room designer test için doğru tool zinciri?
**Seçenekler:**

**(A) Pure Wang Pipeline:** `create_topdown_tileset` (Pro mode, Gemini)
- Lower Terrain: dark rubble stone floor description
- Upper Terrain: dark stone wall description
- Transition: 0.25 veya 0.5 (small-large)
- Çıktı: 16 Wang tile → Unity'de Rule Tile setup
- ⚠ WallAutoConnect.cs 4-bit NSEW; Wang corner-based — schema mismatch riski

**(B) Hybrid Pipeline:** Wang base + varyant ekle
- Step 1: `create_topdown_tileset` Pro → 16 base Wang tile (floor↔wall connected)
- Step 2: Base tile'lar style reference → `create_tiles_pro` style mode → 4-8 floor varyantı (lichen, broken flagstone, rune dust, vb.)
- WallAutoConnect değil, RuleTile + variant blending
- En esnek, en doğal görünüm

**(C) Variant-only Pipeline:** `create_tiles_pro` shape mode + 4 numbered floor variant
- Hızlı pilot, sadece floor varyant
- Wall ayrı pipeline (manuel veya başka tool)
- En düşük güven, en hızlı test

### Q3: Doğal görünüm için spesifik prompt (seçilen pipeline için)
- Lower Terrain text (eğer A/B): Vivid Vulnerability, F1 palette
- Upper Terrain text: dark stone wall
- Transition text: rubble pile, broken seam
- Style options (outline, shading, detail level)
- Negatif kısıtlar

## RIMA Mevcut Pipeline Tarafı (önemli kısıt)

- `WallAutoConnect.cs` — 4-bit NSEW mask → 8 wall variants (hâlihazırda kod var)
- `FloorVariantPainter.cs` — Perlin 3-katman variant blend (kod var)
- `TileImportWizard` — single sprite tile import (NSEW autoconnect mapping pending, S60'tan)
- Karar #100: 32x32, high top-down ~30-35°, PPU=64
- Karar #77: Vivid Vulnerability, F1 Shattered Keep — #2C2A2A floor / #4A3F3F wall / #7BA7BC cold blue / cyan-violet rift

**Schema uyumsuzluk uyarısı:** Wang corner-based (4 corner → 16 kombinasyon) vs NSEW edge-based (4 edge → 16 kombinasyon ama farklı topology). Unity RuleTile her ikisini de destekler ama WallAutoConnect.cs sadece NSEW yapıyor → Wang çıktısı için ya:
- (i) WallAutoConnect'i Wang'a portla
- (ii) Wang çıktısını NSEW'e map'le (kod yazılır)
- (iii) Unity RuleTile + RandomTile kullan, WallAutoConnect bypass

## Çıktı Format (CODEX_DONE.md'ye append)

```
# Codex Final Decision v2: Tile Tool Seçimi REVISION

## Verdict (v1 üzerine)
[CONFIRM v1 / REVISE v1 / OVERTURN v1]

## Q1: Karar #75 Revision
[EVET/KISMEN/HAYIR + yeni metin önerisi]

## Q2: Selected Pipeline
[(A) / (B) / (C) + gerekçe 3-5 madde]

## Q3: Concrete Prompt (PixelLab UI'a yazılacak metin)
### Lower Terrain
[metin]
### Upper Terrain
[metin]
### Transition (eğer >0)
[metin]
### Style options
[outline, shading, detail level seçimi]
### Negative constraints
[liste]

## RIMA Integration Plan
[WallAutoConnect uyumluluğu nasıl çözülecek — (i)/(ii)/(iii) seç]

## Pilot → Batch Steps
[1, 2, 3, ... concrete actions]

## Risks Identified
[liste + mitigation]

## Sources Cited
[hangi NLM bulgusu, hangi UI ekranı, hangi web kaynağı]
```

## Kısıtlar
- Kod yazma yok, dosya değiştirme yok, commit yok
- LOCKED kararları sorgulayabilirsin (zaten görev bu) ama override etme — revize önerisini Karar metni olarak yaz, kullanıcı LOCK eder
- Türkçe yaz
- CODEX_DONE.md'ye **append** (üzerine yazma — v1 raporu altına yeni bölüm)
- Effort: high
