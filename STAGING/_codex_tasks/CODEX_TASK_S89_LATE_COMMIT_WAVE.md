# S89 LATE Commit Wave

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: yasinderyabilgin (boş %1)
Effort: medium
Timeout: 600s

## Universal Coding Principles (Karpathy 4)

1. THINK BEFORE COMMITTING: hangi dosya hangi commit'e ait, ANCHORS/ + Assets.meta gibi belirsiz dosyalar için investigate veya BLOCKED.
2. SURGICAL CHANGES: sadece bu session'da yapılan iş için commit; alakasız working-tree drift'i ayrı commit veya skip.
3. MIN CODE: tek commit yeter — 3 logical commit YAPMA, bir bütün olarak landingt et.
4. GOAL-DRIVEN: working tree clean değilse veya conflict varsa BLOCKED yaz.

## Görev

`git status` 40+ değiştirilmiş + untracked dosya. Tek logical commit hazırla: **S89_LATE wave** (UnityMCP modal bypass + v15c 8-layer refactor + v15c imagegen integration + autosprite trial state sync).

## Adımlar

1. `git status` ve `git diff --stat` ile durumu kavra.
2. **Bilinmeyenleri investigate** (commit'e girer mi):
   - `ANCHORS/` — ne içeriyor? Bu session ile alakalı mı?
   - `Assets.meta` — Unity meta drift?
   - `Assets/Scripts/MapDesigner/Brush/Data/*` — Brush V1 mi (alakasız) yoksa v15c collateral mi?
   - `ProjectSettings/EditorSettings.asset` — Unity setting drift?
   - `STAGING/character_production_prompts.md` — alakasız iter mi?
   - `cx_dispatch.py` — Karpathy adım 2 mi (alakalı)?
   - `.claude/PROJECT_RULES.md` — Karpathy 4 prensip add (alakalı)?
3. **Eğer alakasız working-tree drift varsa**: o dosyaları stage etme, sadece şu listeyi commit et:

### Mutlaka commit'e dahil (session work)

**UnityMCP modal bypass:**
- `Packages/com.coplaydev.unity-mcp/` (new local override)
- `Packages/manifest.json`
- `Packages/packages-lock.json`
- `Assets/Tests/EditMode/MCPSceneLoadModalBypassTests.cs` (new)
- `STAGING/CODEX_TASK_UNITYMCP_SCENE_MODAL_BYPASS.md`
- `STAGING/CODEX_TASK_UNITYMCP_SCENE_MODAL_BYPASS_DONE.md` (new)

**v15c 8-layer refactor:**
- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintZoneTypeSO.cs`
- `Assets/Scripts/Rima/MapDesigner/SO/BlueprintProfileSO.cs`
- `Assets/Editor/MapDesigner/Blueprint/AutoPopulator.cs`
- `Assets/Editor/MapDesigner/PropPlacementService.cs`
- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs`
- `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindowTests.cs` (if changed)
- `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulator8LayerTests.cs` (new)
- `Assets/Tests/EditMode/MapDesigner/Blueprint/AutoPopulatorTests.cs`
- `Assets/Tests/EditMode/MapDesigner/Blueprint/BlueprintPainterWindowTests.cs`
- `Assets/Data/Blueprint/ZoneTypes/zone_*.asset` (6 files)
- `Assets/Scenes/Demo/RoomPipelineTest.unity`
- `Assets/Screenshots/PlayableRoom_combat_v15c_8layer.png` (new)
- `STAGING/CODEX_TASK_PHASE_A_v15c_8_LAYER_REFACTOR.md`
- `STAGING/CODEX_TASK_PHASE_A_v15c_8_LAYER_REFACTOR_DONE.md` (new)
- `STAGING/v15c_8layer_metrics.txt` (new)

**v15c Layer1+8 imagegen (separate Codex run earlier):**
- `Assets/Data/Blueprint/GeneratedProps/v15c_Layered/` (new, 22 prop wrappers)
- `Assets/Data/Blueprint/GeneratedProps/v15c_Layered.meta`
- `Assets/Data/Blueprint/GeneratedProps/Phase_B3_Gaps.meta`
- `Assets/Data/Brush/AssetParts_v5_Layer1_8/` (new, 22 PNG)
- `Assets/Data/Blueprint/PropPools/pool_atmospheric_*.asset` (6 new)
- `Assets/Data/Blueprint/PropPools/LayerPools/` (new)
- `Assets/Data/Blueprint/PropPools/LayerPools.meta`
- `Assets/Data/Blueprint/PropPools/LayerPools 1.meta`
- `STAGING/RIMA_AssetParts_v15c_Layered/` (new staging PNG batch)
- `STAGING/CODEX_TASK_PHASE_A_v15c_LAYER1_LAYER8_IMAGEGEN.md`
- `STAGING/CODEX_TASK_PHASE_A_v15c_LAYER1_LAYER8_IMAGEGEN_DONE.md`

**State sync (doc + memory):**
- `CURRENT_STATUS.md` (S89_LATE section added)
- `MEMORY/INDEX.md`
- `MEMORY/project_autosprite_trial_pending.md` (new)
- `MEMORY/feedback_autosprite_vs_pixellab_verdict.md` (new)
- `STAGING/autosprite_vs_pixellab_review.md`
- `STAGING/autosprite_tile_inventory_pilot.md`
- `STAGING/karpathy_integration_plan.md` (if present)

**Codex meta (always rotates, include to keep clean):**
- `CODEX_DONE.md`
- `CODEX_DONE_laurethayday.md`
- `CODEX_DONE_laurethgame.md`
- `CODEX_DONE_yasinderyabilgin.md`
- `CODEX_TASK_laurethayday.md`
- `CODEX_TASK_laurethgame.md`
- `CODEX_TASK_yasinderyabilgin.md`
- `cx_dispatch.py` (if changed for Karpathy step 2)
- `.claude/PROJECT_RULES.md` (Karpathy 4 prensipleri added)
- `.claude/scheduled_tasks.lock`

### SKIP veya investigate (kategori belirsiz)

- `ANCHORS/` — `ls ANCHORS/` ile içerik gör. Eğer session work değilse SKIP + commit message'da not düş.
- `Assets.meta` — eğer Unity drift ise include (yoksa commit'i dirty bırakır)
- `Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs`, `MapDesignerBrushPresetSO.cs`, `BrushExecutorRouter.cs` — `git diff` ile değişikliği gör. v15c collateral mi yoksa alakasız Brush V1 iter mi karar ver.
- `ProjectSettings/EditorSettings.asset` — Unity drift, include OK
- `STAGING/RIMA_BrushTool_Dependencies.md`, `STAGING/character_production_prompts.md` — alakasız ise SKIP

### Commit message

```
[S89 LATE] UnityMCP modal bypass + v15c 8-layer painted top-down + autosprite trial state sync

UnityMCP modal bypass:
- Local package override `Packages/com.coplaydev.unity-mcp/` from PackageCache
- `forceDiscard` parameter on ManageScene load/create
- 4 new EditMode tests (MCPSceneLoadModalBypassTests)
- "Save changes?" modal artık bypass edilebilir

v15c 8-layer refactor:
- BlueprintZoneTypeSO 8-pool schema (L1 macro fill, L2 base floor, L3-L4 mid/detail, L5 small scatter, L6 medium props, L7 tall focal cap, L8 atmospheric)
- AutoPopulator 8-pass + PropPlacementService.PlaceSprite
- 6 zone assets migrated (path/grass/stone/wall/water/feature)
- BlueprintPainter Layer Visibility L1-L8
- 10 new AutoPopulator 8-layer tests + 2 visibility tests
- Scene Pro_Redesign_v15c_8LayerPainted_CombatRoom LIVE (375 cells, 842 children)
- v15b deactivated
- Screenshot PlayableRoom_combat_v15c_8layer.png

v15c Layer1+8 imagegen:
- 22 sprite (12 macro fill + 10 atmospheric)
- 6 atmospheric pools (pool_atmospheric_path/grass/stone/wall/water/feature)
- v15c_Layered PropDefinitionSO wrappers
- Note: zone asset Layer1Sprites + Layer8Sprites arrays empty pending v15d wiring

autosprite trial state:
- MCP server registered (Bearer token, ✓ Connected)
- Free plan VFX pilot pending (5 candidate: dash trail / hitspark / portal / aura loop / coin)
- Verdict: PixelLab production primary, autosprite niş VFX trial only
- 2 new memory files (project_autosprite_trial_pending, feedback_autosprite_vs_pixellab_verdict)

Tests: 392/392 EditMode PASS (UnityMCP fix + v15c refactor both verified)
```

## Verification

- `git log -1 --stat` ile commit görür
- `git status` clean veya sadece SKIP edilen dosyalar kalır
- Hata varsa BLOCKED yaz, push YAPMA

## Output

DONE marker: `STAGING/CODEX_TASK_S89_LATE_COMMIT_WAVE_DONE.md` ile commit SHA + dahil edilen dosya sayısı + skip edilen liste.
