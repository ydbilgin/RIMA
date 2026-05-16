# RIMA Map Designer — FINAL Architecture Review (ChatGPT'ye son feedback)

## Bağlam (özet)

RIMA = 2D roguelite, Hades-style painter pixel art, Unity 2D, 128px native. Karar #143 6-layer map pipeline LIVE (L1+L2 zemin tilemap'i sahnede çizili, L3-L6 sprite üretimi bekleniyor). Brush V1 8-sprint LIVE (custom EditorWindow, composite stroke, 12 default brush, BiomeSkin shader, undo/redo). Camera 30-35° high top-down ARPG (NOT isometric, NOT 8-dir character).

PixelLab MCP bağlı ama 256px square + 400px non-square max. **Production üretim PixelLab web UI "Create Image Pro" üzerinden manuel** (32×32 → 688×384 boyut seçenekleri, 512×512 + 512×288 + 424×632 dahil). Karakter üretimi de web UI V3 üzerinden manuel.

İlk cross-review (Codex + ChatGPT) yapıldı, **Hybrid Auto-Slice strategy** LOCK edildi (FINAL_PLAN.md). Şimdi user vision + Claude'un çözümlerini son kez gözden geçirmek istiyoruz.

---

## USER VISION (kullanıcı tarafından bu session'da netleşti)

### V1: Editor-Only Room Library (NOT runtime procedural)

```
EDITOR (ben + algoritma + Brush V1):
  ├─ 20-30-100 oda tasarla (combat / shop / shrine / boss / elite / rest)
  ├─ Her oda = RoomTemplate asset olarak kaydedilir
  └─ RoomBank'a girer (tipe göre)

RUNTIME (oyun çalışırken):
  ├─ Player STS2 DungeonMap'ten bir node seçer ("combat room")
  ├─ Game RoomBank.combat'tan random pick yapar
  ├─ Seçilen RoomTemplate'i yükler (zaten tam dekoratif)
  └─ HİÇBİR map runtime'da çizilmez
```

Tasarım yükü: ~3 hafta = full game library (5 tip × 100 oda × ~3 gün her tip).

### V2: Three Creators Coexist

| Creator | Role | When |
|---|---|---|
| AI agents (Claude/ChatGPT/Codex) | System design, code, sprite prompt formula, plan harmonization | Pre-editor (planning, asset prep) |
| Procedural algorithm (Karar #143 + Auto-Dress) | Auto-decorates rooms in editor (productivity tool) | Editor only — NOT in build |
| User (designer) | Manual paint refinement + props management | Editor anytime |

**Hybrid workflow most common:** Auto-Dress fills 80%, user paints 20% signature.

### V3: User's Brush UX Promise

"Paint gibi boyar." User does:
- Pick brush, drag in scene → done
- `[/]` for size, `B/E` for brush/eraser, `1-9` for brush hotkeys
- Aseprite/Photoshop muscle memory

User does NOT:
- Manage slice rects, variant lists, layer routing, density math
- Open Sprite Editor, edit .meta files, manually tag variants
- Touch atlas rules, walkable masks, edge bias formulas

System auto-handles: bucket selection, weighted pick, layer coordination, walkable filter, edge bias, minDistance, flip/rotation jitter, composite multi-layer.

### V4: Natural Look Algorithm (NOT procedural-feeling)

User wants rooms to look "hand-painted by an artist", not "algorithmic". Auto-Dress output should be **Hades-quality**, not "procgen mush".

### V5: Full Room Designer (Props + Override)

Auto-Dress places base. User then:
- Add props (brazier, banner, broken pillar, chest, statue, breakable jar, doors, levers)
- Move/rotate/delete props
- Refine decorative layers (paint over, erase)
- Re-roll with different seed if not satisfied

User wants **complete authoring control** while keeping the simple paint UX.

### V6: AI-Assisted Label Matching

User asked: "labels'ı sen mi match etsen, yoksa raw output verip ben mi match etsem?"

Decision (Claude proposed): **AI-assisted auto-match with manual override**:
- Importer auto-tags from slice template (`corner_NE`, `patch_hero`, `crack_thin`)
- Auto-bucket assignment from rect.size
- Default weights uniform (1.0)
- User edits in Preview Window if needed (weight slider, tag editor, layer reassign, disable variant)
- 99% use auto, 1% override

User confirmed this approach.

### V7: Industry Pattern Composition (no reinvention)

User explicitly said: "insanlar buna çözüm bulmuştur buradan esinlenerek". Proven tools/games:
- Polybrush (radius/falloff/overlap-avoid scatter)
- Tiled Automapping (rule-based corner/edge detection)
- LDtk Auto Layers (IntGrid context detection)
- Unity Random Brush (weighted variant pick)
- Hades art pipeline (semantic tag-driven assembly, GDC 2019)
- Spelunky room template + decoration rules
- Houdini scatter SOP (attribute-driven density)

RIMA = composition of these patterns, extended for 2D pixel art + 6-layer composite.

---

## CLAUDE'S PROPOSED SOLUTIONS (validate or refine)

### Solution 1: Hybrid Auto-Slice Strategy (already LOCK)

- L3 wall = Strategy A (semantic master per wall type, semantic slice via TemplateRect with gutters + Validator gutter check + fallback to explicit PNG on FAIL)
- L4-L6 = Strategy B (organic master atlas, TemplateRect slice with bucket assignment)
- 5 size buckets: **Micro (32) / Small (64) / Medium (128) / Large (192) / Hero (256)**
- `BrushRadiusProfileSO` with **soft overlap mapping** (radius 4 = 50% Small + 50% Medium for smooth transition, NOT hard cutoff)
- `BrushAssetVariant` data model (sprite + bucket + weight + tags + pivot + footprintRadius + flip/rotation policy + Karar #143 atlas fields)
- 7 starter `SliceLayoutTemplateSO` (L3_Horiz, L3_Vert, L3_Corners with gutter, L3_Doorway, L4_Organic_512, L5_Detail_512, L6_Accent_512)

### Solution 2: BrushAtlasImporter + Validator + Preview Window (Sprint 9 — proposed)

- One-click PNG → "Import as Brush Atlas" → template select → done
- 13-check Validator (transparency, gutter, no-border, pivot, bucket, layer, weight, footprint, no-duplicate-IDs, Point filter, no compression, no mipmaps, gutter-cross-transparent for L3 corner)
- BrushVariantPreviewWindow: variant grid by bucket, weight slider, sample stroke distribution histogram, tag editor

### Solution 3: Natural Placement Engine — 6 Techniques (Sprint 10 — proposed)

To make Auto-Dress output look hand-painted:

1. **Perlin noise density modulation** (anti-uniform): density mask sampled per-cell, multiplied with base density
2. **Poisson disk sampling** (Bridson 2007, anti-grid): uniform-random placement with min distance enforcement
3. **Markov chain cluster bias** (anti-isolation): P(decal | nearby decal) > P(decal | none), capped cluster size
4. **Anti-repetition variant cycling**: last-N pick history, temporary weight reduction for recently picked variants
5. **Mask-driven spatial awareness** (Karar #143 LIVE): walkable + encounterAvoid + edgeBias + featureMask
6. **Multi-scale composition**: Hero pick rare, Medium common, Small frequent — bucket pick weights modulated per layer

These compose in BrushExecutorRouter as a single placement pipeline. Each can be independently toggled in BrushPreset for different brush types (e.g., L4 moss uses all 6, L6 hero rift skips Markov clustering since it's intentionally sparse).

### Solution 4: Props Mode (Sprint 11 — proposed)

Brush Window second tab:
- Paint Mode (existing) — layered brush stroke
- **Props Mode (new)** — single-click prop placement

Props Mode features:
- PropsPool (new AssetPool category): static props (brazier, banner, pillar, chest, statue), interactive (door, lever, jar), decorative (rugs, debris, banners)
- Per-prop metadata: sprite/prefab, footprint, pivot (default bottom-center), snap-to-grid toggle, layer hint, biome tags
- Click prop → ghost preview cursor → click in scene → place
- Click + drag → move prop
- Right-click → rotate / flip / delete / duplicate
- Selection box → multi-prop operations

References: Hades level editor (drag-drop prop on procedural floor), LDtk Entity Layer, Tiled Object Layer, Spelunky 2 room editor.

### Solution 5: Room Library Format

- `RoomTemplateSO` (ScriptableObject) per room
- Stores: room dimensions, floor tile data, wall placements, decoration data (placed sprite refs), prop placements, room type tag, biome tag, difficulty tier
- Format choice: **prefab-with-data hybrid** (scene-like child hierarchy + data SO for metadata)
- RoomBank: `Dictionary<RoomType, List<RoomTemplateSO>>` queried at runtime
- Save: "File > Save Room as Template" menu item
- Load: "File > Load Room Template" repopulates current scene with the saved layout

### Solution 6: AI-Assisted Auto-Tag with Manual Override

(Already described in V6 above — auto-tag from template + Preview Window override.)

### Solution 7: Sprint Roadmap Adjustment

| Sprint | Status | What |
|---|---|---|
| 1-8 | LIVE | Brush V1 core (data, executor, decorative, UI, brush pack, automation, BiomeSkin) |
| 1 (retrofit) | Adjust | Add SizeBucket / TargetLayer / BrushAssetVariant migration |
| 6 (retrofit) | Adjust | CompositeStrokeExecutor uses BrushRadiusProfile + bucket gating + Karar #143 sequencing |
| 7 (retrofit) | Adjust | Auto-Dress / Regenerate / Smart Fill use bucket presets |
| **9 (new)** | Proposed | BrushAtlasImporter + Validator + Preview Window + 7 starter SliceLayoutTemplateSO |
| **10 (new)** | Proposed | Natural Placement Engine (6 techniques composition) |
| **11 (new)** | Proposed | Props Mode + PropsPool + RoomTemplate save/load |
| 12 (new) | Proposed | Room Library + RoomBank + STS2 integration |

Total V1 effort: ~3 sprint adjustment + 4 new sprints = ~5-7 day Codex/Opus parallel.

---

## QUESTIONS FOR FINAL FEEDBACK

1. **Architecture validation:** Editor-only RoomBank (NO runtime procedural) — is this sound for a roguelite, or are we missing the "every run feels different" promise that Slay the Spire / Hades players expect? Does 20-30 rooms per type provide enough variety, or should we go higher (50-100)?

2. **Natural-look algorithm:** Are the 6 techniques sufficient, or are we missing key approaches (e.g., reaction-diffusion patterns, Worley noise, Wang tile boundary blending)? Should any be cut as overkill for V1?

3. **Props Mode design:** Separate brush mode tab (Paint / Props) — is this clearer than treating props as a 7th brush layer (L7)? Pros/cons of each?

4. **Auto-tag UX:** Auto-tag from template + Preview Window override — sufficient, or should we add an "AI tag suggestion" pass (Claude/Codex analyzes sprite content and proposes tags post-import)? V1 vs V2?

5. **Pixel art specifics — are we missing anything?**
   - Color quantization enforcement (palette LOCK validation)?
   - Outline consistency check across variants?
   - Bayer dither pattern verification?
   - Palette swap support (one master texture, multiple biome tints)?
   - Anti-aliasing detector (warn if sprite has AA pixels)?

6. **Room template format:** Prefab-with-data hybrid vs pure SO vs scene asset (.unity) — which is most maintainable and version-control friendly for 100-300 rooms?

7. **Production workflow realism:** 100 combat rooms × ~10 min each = ~17 hours = ~3 days per type. Is this realistic given Auto-Dress + manual refine + save? What's the likely bottleneck (asset gen QC? Auto-Dress quality? Save/load friction?)?

8. **Sprint sequence:** Is Sprint 9 (Importer) → 10 (Natural Engine) → 11 (Props) → 12 (Room Library) the right order? Should Room Library come earlier (before Natural Engine) so we can test the actual game loop earlier?

9. **What's missing from FINAL_PLAN that would bite us?**
   - Multi-room transitions (door alignment between adjacent rooms)?
   - Room scaling for different player counts (single vs co-op)?
   - Localization-ready prop names (TR/EN tags)?
   - Hot-reload for iterating on a room without scene reload?
   - Performance profiling (200+ sprites in scene render cost)?
   - Save format versioning (room template format migration in V2)?

10. **Last red flags:** Any production gotcha we haven't considered?
    - Asset path naming conventions for 100+ rooms (collision-free)?
    - Atlas import validation false positives that block production?
    - Pivot consistency across biomes (a "corner" prop must align in any biome)?
    - Memory budget for 200-300 RoomTemplate assets loaded at runtime (or lazy load)?
    - Test coverage strategy for Brush V1 changes (regression risk for existing 37 tests)?

---

## EXPECTED OUTPUT FORMAT

```
1. ARCHITECTURE VALIDATION: [editor-only RoomBank — sound or risky? specific concerns]
2. NATURAL-LOOK ALGORITHM: [6 techniques sufficient? cut/add? V1 vs V2 split]
3. PROPS MODE DESIGN: [tab vs L7 layer recommendation + reasoning]
4. AUTO-TAG UX: [Preview override sufficient? AI suggestion V1 or V2?]
5. PIXEL ART GAPS: [color/outline/dither/palette swap/AA — must-have V1?]
6. ROOM TEMPLATE FORMAT: [prefab vs SO vs scene asset — recommendation + version control angle]
7. PRODUCTION REALISM: [10 min/room realistic? bottleneck identification]
8. SPRINT SEQUENCE: [9-10-11-12 order OK or reshuffle?]
9. MISSING FROM FINAL_PLAN: [specific items, prioritized P0/P1/P2]
10. RED FLAGS: [production gotchas we haven't considered, specific to RIMA scale]
11. CONFIDENCE LEVEL: [Hybrid Auto-Slice + Natural Engine + Props Mode — your confidence this ships well in 5-7 days; if low, what's the mitigation?]
12. ALTERNATIVE WE HAVEN'T CONSIDERED: [is there a fundamentally different approach we're missing? e.g., commercial Unity asset, different paradigm]
```

---

## DELIVERABLE EXPECTED

After your feedback, we will:
1. Update `STAGING/sprite_strategy_FINAL_PLAN.md` with any architectural adjustments
2. Update RIMA memory files (MEMORY index + brush-tool-v1 + new natural-engine + props-mode entries)
3. Write Sprint 9, 10, 11, 12 task specs for Codex/Opus dispatch
4. Revise PixelLab batch files (`pixellab_l3_wall_batch.md` + `pixellab_l4_l5_l6_batch.md`) to master+template format
5. User starts producing first L3 horizontal wall master to test the actual import + paint loop

This is the LAST cross-review before code adjustment + production. **Be direct — what would you change, cut, or add? What are we wrong about?**
