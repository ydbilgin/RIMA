---
name: multi-projection-architecture-lock
description: "RIMA Map Composer 3-verdict consensus LOCK — angled top-down for RIMA, multi-projection architecture (renderer plug-in), HD-2D/iso deferred to future games, data-first decal Phase 1.5 critical path"
metadata: 
  node_type: memory
  type: project
  originSessionId: a12da79a-6b77-423a-8b7c-59af8ccea2f8
---

# Map Composer Multi-Projection Architecture LOCK

3 bağımsız high-quality verdict converge etti — Codex narrow (`STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md`), Opus overnight (conversation log), Codex multi-projection (`STAGING/CODEX_OVERNIGHT_multi_projection.md` 415 satır).

## Headline decision

**RIMA = angled top-down (30-35° low top-down, S86 LOCK preserved). NO HD-2D pivot. NO iso. Multi-projection ARCHITECTURE prepared, multi-projection IMPLEMENTATION deferred.**

Camera angle visually reads as "isometric-like depth" (karakter sprite 30-35° angle + orthographic camera) but logically rectangular grid + cell-aligned 32×32 tile. Hades / Diablo 2 / Enter the Gungeon stilinde — dash + projectile + dense combat'a optimum.

## Why projection NOT the natural-look fix

3 verdict ortak insight (Opus breakthrough): "Doğal görünüm" projeksiyon problemi değil, **kompozisyon problemi**. Alabaster Dawn top-down, CrossCode top-down, Hades low top-down, Octopath HD-2D — hepsi farklı kamera ama hepsi doğal. Çözüm her zaman **L0-L11 layered composition** (clean base + macro patches + organic decals + scatter + accent). ChatGPT FINAL recipe + Codex data-first pipeline zaten yeterli.

HD-2D pivot RIMA için 3-4 hafta arkitektür yatırımı + sıfır gameplay iyileşmesi = over-engineering. Defer.

## V1 boyunca 6 hard rule (multi-projection future bloklamaz)

1. **Brush executor `VisualPlacement` data emit**, GameObject değil ([[room-composer-paint-intent-lock]] + Codex Phase 1.5 plan)
2. **SO kontratlarında Unity render-stack type YOK** (no SpriteRenderer/Tilemap/Mesh in public API)
3. **LightingProfile intent-language** (warm/cold/pulse), URP component-language değil
4. **PPU=32 + cell=1 unit IMMUTABLE**
5. **CornerField encoding IMMUTABLE** (Wang16 corner mask `NW<<3|NE<<2|SW<<1|SE<<0`)
6. **RoomVisualMode enum → SO reference** (gelecek oyun kendi mode'unu tanımlasın)

## Implementation order (locked)

| Phase | Scope | Status |
|---|---|---|
| Phase 1A | L2b macro slicing + Codex imagegen + asset library | DEVAM |
| **Phase 1.5** | **Data-first decal migration** (FreeformDecalDataExecutor + ScatterAlongStrokeDataExecutor + RoomDecalDataSO + ChunkedRenderer + feature flag) — **CRITICAL PATH** | START NOW |
| Phase 1B-1C | 20-30 oda MVP, low top-down only, combat playable | NEXT |
| Phase 1D | Renderer abstraction refactor (`IRoomRenderer` + extract LowTopDownRenderer) — post Phase 1.5 | AFTER 1.5 |
| Phase 2 | Top-Down sibling renderer (1 hafta validate abstraction) — abstraction proof | POST V1 |
| Phase 3+ | Iso renderer — sadece konkret gelecek oyun talep ederse | DEFER |
| Phase 4+ | HD-2D renderer — sadece konkret gelecek oyun talep ederse | DEFER |

## Asset rules

- Asset source PixelLab pixel art + Codex imagegen painterly texture — **all 4 projections**'a uyumlu (HD-2D'de billboard quad, iso'da farklı 8-dir, top-down ve low top-down aynı)
- Wang16 corner mask = pure data, **renderer interpret** (tile sprite vs mesh height transition)
- Macro patch from large painterly source — Codex imagegen `STAGING/Phase1A_L2b_Source/codex_floor_source_v1.png` (1024×1024) **verified working** + slicing pipeline ready

## Visual gate

Phase 1.5 + L2b slicing + 1 reference room compose sonrası **visual verdict**: "Doğal görünüyor mu?"
- **PASS** → V1 ship trajectory (Yol A locked, HD-2D defer)
- **FAIL** (3-verdict consensus'tan sonra düşük olasılık) → Codex'in kill-criteria HD-2D prototip escape hatch'i (`STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md:39-66`)

## Future games (RIMA dışı)

RIMA composer kodu **MapComposer.\*** namespace altında extract edilecek (Phase 1D). Gelecek oyunlar:
- Local copy / UPM package olarak import
- Aynı `RoomData` + `VisualPlacement` + `IRoomRenderer` API
- Sadece projeksiyon değişimi: TopDown/LowTopDown/Iso/HD2D renderer plug-in
- 1-3 hafta renderer extension per yeni projeksiyon

## Cross-references

- [[room-composer-paint-intent-lock]] — ChatGPT FINAL Phase 1A direction
- [[3d-portability-strategy]] — renderer-agnostic SO design
- [[brush-tool-v1]] — Brush V1 ship-ready + semantic 3-mode mapping
- [[karar-143-layered-pipeline]] — L0-L11 layer model
- [[camera-angle-direction-strategy-locked-s86-update]] — 30-35° low top-down LOCK
- [[hybrid-asset-pipeline-lock]] — PixelLab + Codex imagegen hybrid

## Authoritative documents

- `STAGING/CHATGPT_PHASE1_FINAL_DIRECTION.md` — ChatGPT Yol A locked
- `STAGING/CODEX_TWEET_REVIEW_xhigh.md` — Codex tweet review (Phase 1.5 blueprint)
- `STAGING/CODEX_STRATEGIC_2D_vs_HD2D.md` — Codex narrow strategic verdict
- `STAGING/CODEX_OVERNIGHT_multi_projection.md` — Codex multi-projection 415-line architecture
- Opus overnight verdict — conversation log (multi-projection breakthrough insight)

## Status

LOCK 3-verdict consensus. Phase 1.5 data-first decal migration **immediate next step**. No HD-2D / iso prototype work in current sprint. RIMA V1 trajectory clear: angled top-down + layered composition + data-first decals.
