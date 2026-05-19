# Codex Task — Professional Level Design Map Redesign (Mutual QC with Orchestrator)

**Profile:** auto-selected (quota-aware)
**Effort:** xhigh
**Timeout:** 7200s (2 hours)
**Type:** Critique → redesign plan → implement → self-verify → iterate

## User feedback (verbatim 2026-05-18)

> "tam bi map gibi değil"
> "floor çok düz ve bazı şeyler çok random yerleştirilmiş"
> "gerçekten bir map dizaynı gibi olmalı"
> "siz tatmin olana kadar bunu yapın"
> "şu resim üretmeyle devam edelim" (= Codex imagegen ile asset üret eğer gerek)

## Authority granted

Orchestrator (Sonnet/Opus) has delegated full Map Phase A authority to you (Codex). User explicitly said "bana hiçbir şey sormayın, kararı sen ver". You are the senior level designer for this iteration. Iterate internally until BOTH you and the orchestrator can defend the result as "real game map" quality.

## Reference visual targets

- **Hades** (Supergiant Games) — chamber design, focal hierarchy, atmospheric lighting
- **Alabaster Dawn** — painterly continuous floor with organic detail
- **Diablo 2** — natural terrain, varied surfaces, environmental storytelling
- **Hyper Light Drifter** — strong silhouette, intentional negative space
- Architecture LOCK: pure 2D orthographic, PPU=32 IMMUTABLE, 30-35° angled-view sprites, NO camera tilt

## CRITICAL — Unity pre-check

Unity OPEN, instance `RIMA@ed023e0b`, scene `RoomPipelineTest`. Verify before any tool call.

Existing assets:
- 84 sliced sprites at `Assets/Sprites/Environment/RIMA_AssetParts_v2/` + 7 PatchAtlasSO `Assets/Data/Brush/AssetParts_v2/`
- 40 sliced sprites at `Assets/Sprites/Environment/RIMA_AssetParts_v3/` + 7 PatchAtlasSO `Assets/Data/Brush/AssetParts_v3/`
- Warblade character `Assets/Sprites/Characters/Anchors/01_warblade.png`
- Test scripts `Assets/Scripts/Test/` (TestPlayerMovement, TestCameraFollow, TestDoorExit)

Existing scene `PlayableRoom`:
- `Floor_BigBiomes` (1 procedural big sprite 1152×704, multi-biome tinted)
- `Decoration` parent with 9 zone children (8 thematic + center columns)
- Player Warblade at (18,11) center
- Camera at ortho 11 (overview), follow Lerp 0.15 attached

Latest screenshot: `Assets/Screenshots/PlayableRoom_8zone_biome_v11.png`

## STAGE 1 — Critique (write to RECOMMENDATION.md first)

Output `STAGING/CODEX_PRO_LEVEL_DESIGN_RECOMMENDATION.md`:

1. **What's broken** in v11 from a professional level designer's eye:
   - Floor (too flat? too uniform? missing organic detail?)
   - Composition (focal hierarchy wrong? balance off? negative space misused?)
   - Placement (which zones feel random vs intentional?)
   - Visual flow (where does eye travel? are there dead zones?)
   - Camera (ortho 11 too far? specific suggestion?)
2. **Diagnose root causes** (procedural texture too simple? scatter algorithm bad? composition principles ignored?)
3. **Concrete redesign plan**:
   - New floor approach (specific: cracks layer? path tracks? worn stone overlay? Codex imagegen new asset?)
   - Zone restructure (merge/remove/move/add)
   - Specific placement rules (rule of thirds, golden ratio, focal triangle, etc.)
   - Asset gaps — what NEW Codex imagegen sheets to produce
4. **Risks** + **what could fail**

## STAGE 2 — Implement (after writing RECOMMENDATION)

Execute your own plan via Unity-MCP `execute_code` + Codex imagegen if you specified new assets:

- Generate new asset sheets if needed (via `imagegen` skill, output to `STAGING/RIMA_AssetParts_v4/`)
- Slice + import to Assets/ + create PatchAtlasSO
- Reconfigure PlayableRoom hierarchy per plan
- Camera config (gameplay ortho, follow target)
- Lighting tweaks
- Anything else your plan specified

## STAGE 3 — Self-verify

Capture Game view screenshot `Assets/Screenshots/PlayableRoom_pro_redesign_v12.png`.

Review your own screenshot:
- Is it "real game map" quality now?
- Floor reads as designed (not flat/uniform)?
- Composition flows (eye travels, balance, focal hierarchy)?
- No remaining random feel?

## STAGE 4 — Iterate (if not satisfied)

If your self-verify FAILS, iterate (max 3 internal iterations):
- Adjust composition
- Add/remove assets
- Re-screenshot v13, v14, etc.

Stop when YOU are satisfied AND can defend each design choice in writing.

## STAGE 5 — DONE marker

`STAGING/CODEX_TASK_PRO_LEVEL_DESIGN_REDESIGN_DONE.md`:

- Iterations attempted (v12, v13, v14...)
- Final screenshot path
- Each redesign decision + rationale (level design principle invoked)
- New assets produced (if any) + paths
- Visual gate verdict: PASS / PARTIAL — with brutal honesty
- What you would still improve given more time
- EditMode test count (must remain 333/333)
- Camera ortho setting (gameplay vs screenshot)

## Constraints

- DO NOT modify SO contract scripts (`PatchAtlasSO.cs`, etc.)
- DO NOT modify Phase 1.5 data-first executors
- DO NOT change PPU=32 (immutable)
- DO NOT tilt camera (architecture LOCK — pure orthographic top-down)
- DO use existing v2 + v3 assets first; new imagegen ONLY if existing assets cannot achieve the design
- DO save scene at end (Edit mode only, not during Play)
- DO use new Input System for any new player scripts (legacy `Input.GetAxisRaw` BANNED per Player Settings)

## Authority + handoff

You have **full autonomy** for this task. Make design decisions, generate new assets if needed, restructure the scene. Document every decision in the DONE marker so the orchestrator can review your reasoning post-hoc.

## NEXT_SIGNAL

After DONE: orchestrator inspects screenshot + DONE marker. If PASS → Phase A LOCK, Phase B (Map Designer UI/UX) begins. If still PARTIAL/FAIL after 3 iterations → escalate to orchestrator with specific blockers.
