---
name: painter-suite-plan-v2-locked
description: LaurethStudio 2D Painter Suite V2 plan (RIMA reuse edition) locked 2026-05-26. V1 sifirdan tasarim, V2 RIMA tooling extract + decouple + publish.
metadata:
  type: project
---

# LaurethStudio 2D Painter Suite -- V2 Lock

**Locked:** 2026-05-26 (S109 close window)
**Files:**
- V1 plan: `STAGING/LAURETH_2D_PAINTER_SUITE_PLAN.md` (sifirdan tasarim, 3 hafta MVP)
- V2 plan: `STAGING/LAURETH_2D_PAINTER_SUITE_PLAN_V2_RIMA_REUSE.md` (RIMA reuse, 5-7 gun MVP)

**Why:** RIMA'da zaten guclu tooling var (VisualEditorScenePainter, BrushExecutorRouter, ParallaxLayer, MapDesignerBrushPresetSO, BrushPalettePanel). V2 bunlari extract + decouple + publish ediyor, %60 kod yeniden kullaniliyor.

**How to apply:** S110 yapilacaklari (Parallax Tab P0, Occlusion P0, Animated Layers P1) UPM package icine yazilir (`Packages/com.laureth.painter-suite/`), RIMA bu package'i tuketir. Boylece S110 RIMA isi = ayni zamanda Asset Store urunu icin yatirim. Cift kullanim.

**Decoupling discipline:** Package'da 0 RIMA.* reference. Generic interfaces (ITargetResolver, IPaintStroke). RIMA wrapper sadece RIMA Assets'te interface implementation yazar.

**Reuse map (V2 Bolum 1):**
- VisualEditorScenePainter -> ColliderPainter + LayerPainter base
- RimaVisualMapEditorWindow -> PainterSuiteWindow
- AutoLayeringService -> TargetResolverService (generic)
- BrushPalettePanel -> TemplateLibraryPanel
- BrushExecutorRouter -> PainterExecutorRouter
- ParallaxLayer.cs -> Runtime/ParallaxLayer.cs (namespace rename only)
- BrushHotkeyHandler -> PainterHotkeyHandler

**Crash safety protocol:** Her dispatch sonunda memory + CURRENT_STATUS update. Pickup checklist: CURRENT_STATUS read -> MEMORY/painter_suite_progress_*.md last entry -> V2 plan Bolum 6 dispatch list.

**Pending user decisions:**
1. V2 vs V1 -- kullanici onay verecek
2. S110 P0 PainterSuite'e tasiniyor mu -- onay
3. S110 cliff manuel override + double trigger fix bunlarla cakisma -- siralama netlestirilmeli

**Related:**
- [[parallax-layer-runtime]] (Assets/Scripts/Background/ParallaxLayer.cs)
- [[visual-editor-scene-painter]] (RIMA mevcut paint pipeline)
- [[bg-layer-architecture-verdict]] (STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md)
- [[parallax-review-codex]] (STAGING/PARALLAX_REVIEW_CODEX.md Codex Q5 Oneri 1-3)
