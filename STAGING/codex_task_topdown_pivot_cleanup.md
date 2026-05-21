# Codex Task — Top-Down Pivot Cleanup

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## CONTEXT — Major Pivot Decision

User locked **top-down + fake-iso pivot** (Children of Morta / Death Trash model) 2026-05-21 S97 LATE NIGHT 2. Iso experiment 3 sprint sonrası terk edildi (AI gen + iso tile architecturally mismatched).

**LIVE pivot memory:** `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_topdown_fakeiso_pivot_lock.md`

User direktifi: "codexle konuşup temizliği yap stagingde de gereksizleri temizleyin. artık temiz bi oyun yapısıyla oyunumu yapmak istiyorum"

→ Clean game structure ZORUNLU. Iso era archive, top-down baseline ready.

## DELIVERABLES (in order)

### 1. Memory Archive (16 SUPERSEDED iso entries)

**Hedef klasör:** `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/_archive/iso_experiment_pre_topdown_pivot/`

**Taşı (move, not delete):**
- project_wall_pack_v3_live.md
- project_iso_scene_rebuild_s97.md
- project_isometric_floor_pivot_s95.md
- project_karar_150_fake_isometric_lock.md
- project_transform_squash_floor_fix_s93.md
- project_tile_angle_verdict_branch_d_e_lock.md
- project_alabaster_dawn_pipeline_lock.md
- project_karar_143_layered_pipeline.md
- project_wall_seam_birlesim_sorunu_s95.md
- project_faz1_wall_structural_live_s95.md
- project_wall_object_production_plan_s95.md
- project_rima_visual_vision_reference_s95.md
- project_path_c_hybrid_lock.md
- feedback_painter_user_paint_iso_view_hard_lock.md
- project_wall_randomize_variants_when_states_live.md
- project_wall_variation_system_plan.md

**Optional re-evaluate (DEFER, sadece review notu ekle):**
- project_rima_hades_style_cb_wang_split_lock.md (RIMA için Wang16 reactivation candidate)
- project_wang16_compositor_pipeline_lock.md (RIMA için reactivation candidate)
- project_karar_143_layered_pipeline.md (top-down 2-layer adapted variant)

### 2. MEMORY.md Index Regen

**Mevcut sorun:** MEMORY.md 32.7KB (limit 24.4KB), index entries çok uzun.

**Yeni format:**
- Aktif "Active" section: TOP-DOWN pivot live + canonical lore + character roster + combat + audio
- Yeni "Iso Archive" section (en altta): kısa bir-line entries, _archive/ klasörüne pointer
- Her satır ≤200 char
- "project_topdown_fakeiso_pivot_lock.md" en üstte (most-recent active)

### 3. Scene Archive

**Hedef:** `Assets/_ARCHIVE/Scenes/iso_experiment_pre_topdown_pivot/`

**Taşı:**
- Assets/Scenes/Demo/PlayableRoom_v2.unity (+ .meta)
- Assets/Scenes/Demo/WallTest_Map1_Rectangle.unity (+ .meta)
- Assets/Scenes/Demo/WallTest_Map2_LShape.unity (+ .meta)
- Iso-related screenshots: WallTest_Map*_v*.png, IsoShowcaseRoom_*.png → Assets/_ARCHIVE/Screenshots/iso_experiment/

### 4. Iso-Specific Editor Scripts Disable

**Disable (comment-out + add deprecated header note, DO NOT delete):**
- Assets/Editor/DevTools/IsometricSortSetup.cs → wrap class with `#if false` + add header `// DEPRECATED 2026-05-21 — top-down pivot. See memory/project_topdown_fakeiso_pivot_lock.md`
- Assets/Components/IsoSortingOrder.cs (if exists in scripts) → same treatment

**Keep but reconfigure:**
- Assets/Editor/RimaWorldPainterWindow.cs → change `currentPaintMode` default from `PaintMode.Isometric` to `PaintMode.TopDown`, change `projectionMode` default to `GridProjectionMode.TopDown`
- Assets/Editor/RimaSortingLayerValidator.cs → keep, still valid for top-down

### 5. STAGING Cleanup

**Audit STAGING/ folder, archive obviously stale.**

**Hedef arşiv:** `STAGING/_archive/iso_experiment_pre_topdown_pivot/`

**Taşı (iso-specific, completed, or old):**
- codex_task_wall_iso_placement_fix.md (just-completed, iso)
- wall_iso_placement_fix_REPORT.md (just-completed, iso)
- wall_pack_v3_tile_audit.md (iso)
- ROADMAP_dungeon_buildup.md (iso 6-faz)
- ACT_FLOOR_TAXONOMY.md (iso 6-layer)
- HADES_FLOOR_RESEARCH.md (iso)
- Iso-specific codex_task_*.md files: tile_angle_verdict, fake_isometric, sub_room iso variants, wall_iso_*, granite_low_*, painted_vs_hybrid_iso
- Old screenshots STAGING/_pixellab_outputs/walls/ subset that's iso
- Older STAGING/_codex_done/ files (anything older than 7 days that's iso-anchored)

**KEEP (top-down relevant or active):**
- character_production_prompts.md (10 anchor production)
- PIXELLAB_WORKFLOW_INSIGHTS.md (workflow projection-agnostic)
- door_choice_brainstorm + visualizations (still pending lock)
- Active research docs
- _pixellab_outputs/characters/ (anchors)

**Audit rule:** Eğer dosya başlığı/içeriği "iso", "isometric", "wall pack v3 tilemap", "fake-iso sub-room", "transform squash" geçiyorsa → muhtemelen archive candidate. Eğer "character", "skill", "combat", "lore", "music", "UI" geçiyorsa → keep.

**Belirsizler:** Liste yap raporda, archive ETME — user kararı bekle.

### 6. New TopDownTest_Map1.unity Baseline Scene

**Path:** `Assets/Scenes/Demo/TopDownTest_Map1.unity`

**İçerik:**
- Grid (cellLayout=Rectangular, cellSize=(1,1,0))
  - L1_Floor Tilemap (sortingLayer="Floor", order=0)
  - L2_Walls Tilemap (sortingLayer="Walls", order=10)
  - Props_Root empty GameObject (sortingLayer="Entities", order=20)
  - Lighting_Root empty GameObject
- Main Camera: orthographic, size=5, pos=(6, 4, -10), follow CameraFollow component
- Global Light 2D (URP), intensity 0.8
- Warblade prefab placed at (6, 4, 0) — load from Assets/Prefabs/Characters/Warblade.prefab if exists, otherwise leave a placeholder GameObject with note
- Empty FloorTilemap + WallTilemap (no tiles yet — will be painted manually via Painter or programmatically later)

**Save scene + verify no console errors.**

### 7. CURRENT_STATUS.md Rewrite

Archive current `CURRENT_STATUS.md` → `STAGING/_archive/current_status_pre_topdown_pivot.md`

**Write new CURRENT_STATUS.md with sections:**
- 🟢 LIVE: Top-Down + Fake-Iso PIVOT LOCKED (2026-05-21 S97 LATE NIGHT 2)
- ❌ ARCHIVED: Iso experiment (3 sprint, archived to _archive/, lessons learned section)
- 🎯 NEXT SESSION:
  1. Open TopDownTest_Map1.unity
  2. First wall + floor PixelLab gen dispatch (top-down create_tiles_pro)
  3. Painter usage test (top-down mode)
  4. Combat test (Warblade + 1 mob top-down)
- 🛠️ Tools active: PixelLab (top-down focus), Codex imagegen, Painter, UnityMCP
- ⚠️ Known: wall_pack_v3 hero pieces decoration overlay olarak kalıyor (archway, large column); tilemap wall NEW gen gerek
- 📊 Budget: ~2,400/5,000 PixelLab; top-down baseline ~80-120 gen tahmini
- 🚦 Reference: Children of Morta + Death Trash + Tunic + user screenshot

### 8. Final Git Commit

After all above done:
```
[S97 LATE NIGHT 2] PIVOT — Top-down + fake-iso LOCK, iso experiment archived

- Top-down + fake-iso (Children of Morta model) HARD LOCK
- 16 SUPERSEDED iso memories archived → memory/_archive/iso_experiment_pre_topdown_pivot/
- 3 iso scenes archived → Assets/_ARCHIVE/Scenes/iso_experiment_pre_topdown_pivot/
- Iso editor scripts disabled (IsometricSortSetup, IsoSortingOrder)
- Painter default: TopDown mode
- STAGING cleanup: N stale files → _archive/iso_experiment_pre_topdown_pivot/
- MEMORY.md regen (compact one-line entries, active vs archived sections)
- CURRENT_STATUS rewritten for pivot
- New TopDownTest_Map1.unity baseline scene (Rectangular grid, Warblade, ortho cam, 2D light)
```

(Co-Authored-By: Codex GPT-5.5 ekle)

## SUCCESS CRITERIA

1. `git status` clean (her şey staged + committed)
2. `MEMORY.md` < 24KB, one-liner entries, archived section ayrı
3. Unity console 0 errors after script disable + scene create
4. TopDownTest_Map1.unity açılıyor, Warblade görünür, camera ortho top-down
5. Painter window açıldığında default TopDown mode
6. STAGING/ folder dosya sayısı belirgin azalmış (~50%+ archive)
7. Iso scenes git'te _ARCHIVE/Scenes/ altında, eski yerde yok

## NOTES

- Console error olursa fix + recheck zorunlu (HARD RULE feedback_codex_fix_unity_errors_always)
- Memory dosyaları taşıma `git mv` ile yap (history korunsun)
- Belirsiz STAGING dosyaları için archive ETME, raporda liste yap user kararı bekle
- Unity açık olmalı (UnityMCP scene create için)
- Effort: high (multi-step, multi-system)
- Tahmini süre: 30-60 dk
