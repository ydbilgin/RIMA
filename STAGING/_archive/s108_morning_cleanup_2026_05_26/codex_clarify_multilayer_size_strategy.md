# Codex Task: Multi-Layer Painted BG — Asset Size Strategy Validation

## User'ın sorusu (S92, 2026-05-18)

> "Bu 632×424 ü 256×256 decaller mi üretecez gerçekten? Yoksa daha başka bi yol mu? Codex'le bunu netleştir."

User multi-layer painter plan'da kilitlediğimiz "632×424 floor + 256×256 decal'lar" stratejisini sorguluyor. Doğru mu, gerçek Hades / Alabaster Dawn aesthetic için en iyi yol mu, yoksa farklı bir REST API endpoint daha uygun mu?

## Context

- LOCKED plan: `STAGING/MULTILAYER_PAINTER_PLAN_v1.md` §5 — floor base 632×424, decal 256×256, accent 128×128.
- 632×424 size: `memory/reference_pixellab_create_image_pro.md` (PixelLab Web UI Create Image Pro size table — Web UI'da var).
- Şu an gerçek soru: **REST API `/generate-image-v2` 632×424 destekliyor mu?** Eğer DESTEKLEMİYORSA strategy revize gerekiyor.
- Spec özet: "Up to 512x512 for square, 688x384 for 16:9, above 170px = 1 image."
- Multi-layer LOCK locked, ama içerik üretim stratejisi henüz "asset roadmap" düzeyinde.

## Senin görevin

**STAGING/PIXELLAB_API_V2_OPENAPI.json** (full OpenAPI spec) ve **STAGING/PIXELLAB_API_V2_RAW.md** dosyalarına bak. Şu 5 maddeyi netleştir:

### Soru 1 — `/generate-image-v2` size enum
OpenAPI spec'inde `image_size` parametresinin tam enum / pattern listesi nedir? 632×424 destekleniyor mu, yoksa Web UI-only mı?

### Soru 2 — Alternatif endpoint'ler
Şu endpoint'lerin Hades-style multi-layer painted bg için kullanım uygunluğunu değerlendir:
- `POST /generate-image-v2` (Create Image Pro) — text prompt → painted scene
- `POST /generate-with-style-v2` — style image + prompt → style-consistent scene (layer'lar arası coherence için ÖNEMLI olabilir)
- `POST /create-tiles-pro` (async) — Wang16 tile set (single biome geçişleri için)
- `POST /create-tileset` (top-down tileset, async)
- `POST /create-isometric-tile` — değil iso (kaybedilebilir, RIMA top-down)

Her endpoint için: "Hades-style multi-layer bg için" UYUMLU mu / NE İÇİN UYUMLU?

### Soru 3 — Stratejik karar
**A) Tek 512×512 / 688×384 painted scene** (1 layer floor sadece, decal yok)  
**B) 512×512 floor base + 256×256 decal'lar** (multi-layer ama küçük decal'lar)  
**C) `generate-with-style-v2` ile style-consistent stack** (3-5 layer style-locked, AYNI sanatçının elinden çıkmış gibi)  
**D) `create-tileset` ile painted tileset üret + manuel stack** (procedural variety ama tile-y görünüm)

Hades / Alabaster Dawn (Studio Pixel Pop tweet referans) çıkış noktasına en uygun yol HANGİSİ? Neden?

### Soru 4 — Spawn_01 (8×6 wu room) için somut üretim önerisi
3-5 layer'lık konkret asset list ver. Her layer için:
- Endpoint (`/generate-image-v2` vb.)
- Boyut + aspect
- Prompt theme (1 cümle — "cracked stone floor", "rift glow accent", "broken statue silhouette" vb.)
- Estimated credit cost (1 generation = ? credit, plan toplam credit)

### Soru 5 — Multi-Layer Plan v1 revizyonu
Eğer 632×424 REST'te yoksa, plan §5 size tablosunu nasıl revize etmeliyiz? Concrete edit önerisi (which line, what replacement).

## Output

`CODEX_DONE_multilayer_size_strategy.md` yaz, şu format:

```markdown
# Multi-Layer BG Asset Strategy — Codex Verdict

## Q1: /generate-image-v2 size enum
[verbatim from OpenAPI spec — paste enum list]
632×424 supported: YES/NO

## Q2: Endpoint suitability (5 endpoints)
| Endpoint | Hades multi-layer fit | Use case |
|---|---|---|
| ... | ... | ... |

## Q3: Strategy verdict
Recommendation: [A/B/C/D]
Why: [3 bullet]
Risks: [...]

## Q4: Spawn_01 concrete production (3-5 layers)
| Layer | Endpoint | Size | Prompt theme | Credits |
|---|---|---|---|---|
| 0 floor | ... | ... | ... | ... |
| 1 decal | ... | ... | ... | ... |
| ... |

Total credits: X (of 4524 remaining)

## Q5: Plan §5 revision
[exact diff: old line → new line for STAGING/MULTILAYER_PAINTER_PLAN_v1.md]

## Final recommendation
[1 paragraph: do A/B/C/D, generate N assets, expected outcome]
```

## Hard limits

- KOD YAZMA. Spec-okuma + recommendation.
- OpenAPI JSON büyük — sadece `/generate-image-v2`, `/generate-with-style-v2`, `/create-tiles-pro`, `/create-tileset` schema'larını oku.
- 632×424 sorusu mihver — bunu kesin cevapla (YES/NO + evidence).
- Studio Pixel Pop / Hades reference: STAGING/painted_background_prompts_by_size.md varsa peek.
- 10 dk effort.
