---
name: map-plan-v1-lock
description: "2026-05-18 S91 LOCK: RIMA map system canonical plan. Opus + Codex review synthesis. 16 LOCK decisions covering MVP scope, room taxonomy, biome, library, v15h fate, asset production. CRITICAL: Visual layer = Hades-style monolithic painted background per RoomTemplate (Option C). v15h tile composition DEPRECATED for production."
metadata:
  node_type: memory
  type: project
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# Map Plan v1 LOCK (2026-05-18 S91)

**Source:** `STAGING/MAP_PLAN_v1_FINAL.md` (full plan), Codex review `STAGING/CODEX_REVIEW_MAP_PLAN_DONE.md`, NLM canonical (30ddffa5-...)

**Why locked:** "Map plan netleşsin ki ne üreteceğimizi bilelim" — user direktifi 2026-05-18

## 16 LOCK Decisions

1. **MVP = vertical slice 8-9 rooms** (NOT full Act 1 15-node)
2. **Room taxonomy = LIVE code enum:** Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor (Plan v1 yanlıştı Shop/Unknown/Spirit dedi — code reality FARKLI)
3. **MVP active types:** Combat, Elite, Boss, Chest, Merchant, Corridor (Spawn/Rest tag); Forge/Curse/Event Phase 2; Spirit Phase 3 (UI layer)
4. **Biome canonical:** "Shattered Ruins / Shattered Keep" (tek isim, Sunken Keep flavor only)
5. **Room sizes = LIVE prototype scale LOCK** (8×6-20×14), NOT NLM 24×18-40×30 (re-author maliyeti yüksek, prototype phase için OK)
6. **Library MVP gate = 14 templates, polish target = 19** (mevcut 10 + 4 yeni: Combat_Small_02, Combat_Medium_02, Elite_02, Boss_Arena_01, Merchant_01)
7. **Boss_Arena_01 = NEW asset** (Boss_Intro_01 reuse ETME)
8. **Pipeline canonical:** RoomData/RoomTemplate primary, v15h authoring generator only
9. **v15h fix priority:** 2 Warblade HIGH (bug fix), Wang coverage MED, L5-L8 LOW, re-author DEFER
10. **ThreatBudget = NEW pre-MVP critical** (mevcut yok, count-based spawn var; combat scaling için zorunlu)
11. **DungeonMapUI = AUDIT not implement** (LIVE: `Assets/Scripts/UI/DungeonMapUI.cs`, `DungeonGraph.cs`, `MapFragment.cs`)
12. **Phase 1.5 RoomData 5 questions resolved:** BatchedSprites first via `IRoomChunkRenderer`; SO authoritative serialization; RIMA-owned wrapper selector; WangResolver dirty-bounds; Collider=EntityRecord, decorative=VisualPlacementData
13. **Mob roster MVP = 4** (not 8); 8 = polish target
14. **🔥 Visual layer architecture = Hibrit Monolithic Painted Background + Gameplay Overlay (Option C — Hades model)**
15. **v15h tile composition pipeline = DEPRECATED for production**; kept for procedural Unknown room + test scaffold only. Existing v15h GameObject roots DISABLED in scene.
16. **Per-room painted background = PixelLab create_topdown_tileset OR gpt-image-1** (~14-19 MVP, ~$0.6 gpt OR ~280 PixelLab gen from 5000 budget)

## Why Option C (Visual layer LOCK)

User feedback 2026-05-18: "anlamsız tilelar var, oval olarak görünmesini istiyordum, fake 3D gibi dursun"

Tweet ref: Spirit of the Goddess (https://x.com/SpiritOTGoddess/status/2055868938194723068) → confirmed PURE 3D engine, NOT 2D pixel art. Achieving identical look requires 3D pivot (rejected).

**User chose 2D + Hades painted (not 3D pivot).** Memory [[multi-projection-architecture-lock]] holds (NO HD-2D pivot for now).

Three options considered:
- A — Pure tile composition (v15h current): rejected, "scattered patches" visual problem
- B — Pure monolithic per-room: kills procedural variation
- **C — Hybrid: painted background + gameplay data overlay** ✓ LOCK

## Background art RULE (asset prompt)

**ZORUNLU:** "Angled perspective oval/rounded surface" (Hades / Sea of Stars / Octopath Traveler 30-35° angled top-down).
**YASAK:** "Flat top-down square tile grid".

### Asset prompt template (PixelLab Create Image Pro V3 — copy-paste)

```
A top-down 2D pixel art game scene viewed from a 30-35 degree angled
perspective (NOT flat top-down). The floor is a continuous painted
surface — ROUNDED, ORGANIC, OVAL stone shapes (not square grid tiles)
with subtle foreshortening: top edges compressed, bottom edges wider,
creating a fake-3D depth feel. Soft directional shadows hint at
elevation. Painterly, hand-painted texture, no harsh grid lines,
no visible tile boundaries, seamless natural composition.

Biome: Shattered Ruins / Shattered Keep — cracked granite flagstones
in cool gray with cyan rift cracks, broken stone walls at edges,
moss patches, scattered rubble.

Style reference: Hades / Sea of Stars angled top-down painted
background, NOT pixel art tile sets.

Resolution: 640x480 (or 512x320 for smaller rooms).

Negative Prompt: flat top-down, square tile grid, visible grid lines,
isometric diamond tiles, axonometric view, sharp square stones,
repeating tile pattern, gridded floor.
```

## v15h composer fate

- **GameObject roots:** DISABLED in `Assets/Scenes/Demo/RoomPipelineTest.unity` (this LOCK turn). NOT deleted (keep for procedural Unknown room future).
- **Tile assets:** Wang dirt/cobble PNG + 16 Tile.asset + RuleTile = KEPT (transition decal use case)
- **AutoPopulator code path:** KEPT (procedural future, Unknown room)
- **Composer scripts (RimaV15hPlayableComposer.cs, etc):** KEPT but marked deprecated in comments (do NOT use for production rooms)

## Production workflow (NEW)

```
1. Designer concept (e.g. "Combat_Medium_02 = arena with broken statues + cyan rift cracks")
2. PixelLab Create Image Pro OR gpt-image-1 painted scene 640×480 → import Assets/Art/Rooms/Backgrounds/
3. Create RoomTemplateSO.asset:
   - background_sprite ref → painted image
   - bounds = 12×8 tile grid
   - collision_grid = designer-painted (Tile/Pass)
   - spawn_sockets, gates, decoration_overlay (interactables only)
4. RoomBank.Pick(type) → RoomTemplate → runtime renders bg sprite + collision + entities
```

## Phase 1.5 RoomData spec impact

Q1 revised: BatchedSprites primary, **add `BackgroundLayer` sprite quad rendering BEFORE tile layers** in `IRoomChunkRenderer`. Background = single SpriteRenderer per room (not chunked).

## Asset roadmap Wave 1 (immediate)

| Asset | Producer | Estimate |
|---|---|---|
| 5 painted RoomTemplate backgrounds (Spawn_01, Combat_S/M/L_01, Elite_01) | PixelLab Create Image Pro OR gpt-image-1 | 5 images |
| Boss_Arena_01 background | Same | 1 |
| Merchant_01 background | Same | 1 |
| Chest_01 background (Treasure_01 rename) | Same | 1 |
| Warblade re-download (manual user) | PixelLab Web V3 | — |
| RoomTemplate editor: background_sprite field | Sonnet+UnityMCP | — |
| Background layer renderer | Codex impl + Sonnet wire | — |
| **Total Wave 1 MVP** | | **~9 painted images** |

## Related

- [[5000-pixellab-allocation-lock]] (budget fit)
- [[hybrid-asset-pipeline-lock]] (PixelLab + Codex split — still valid for character/mob)
- [[multi-projection-architecture-lock]] (3D pivot rejected, 2D LOCK holds)
- [[blueprint-first-map-design]] (procedural fallback for Unknown rooms)
- [[v15g-v15h-minimal-pixellab-pipeline]] (DEPRECATE — superseded by Option C)
- [[3d-portability-strategy]] (data+logic portable, only rendering swap; future option preserved)
