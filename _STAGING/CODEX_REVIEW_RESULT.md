# Review: Sprite Size + Direction Count — PARTIAL

Date: 2026-04-24
Reviewer: Codex (doc-based cross-check only; final QC/decision is Claude per AGENTS.md)

## Findings
1. 128px baseline is consistent across core RIMA docs. — `TASARIM/STYLE_BIBLE.md:37-43` + `GUIDES/CHATGPT_CHARACTER_PIPELINE.md:146-154` + `CLAUDE.md:251-253` — Kritiklik: MAJOR
- Evidence:
  - STYLE_BIBLE sets Player baseline to 128px.
  - CHATGPT_CHARACTER_PIPELINE ADIM 4 says PixelLab Canvas 128x128.
  - CLAUDE.md declares PixelLab PPU=64, Canvas 128x128, prefab scale 0.5, camera ortho size 5.

2. 8-direction is explicitly aligned with locked decisions #46/#47; 4-direction main locomotion would conflict. — `CURRENT_STATUS.md:53-60` — Kritiklik: BLOCKER
- Evidence:
  - #46: run 6f, 8 directions separate, no flip.
  - #47: Run=Animate 8gen, Attack/Skill=KF+Interp.

3. Camera angle claim (~60-65 deg overhead, eyes not visible) matches GDD + CURRENT_STATUS. — `TASARIM/GDD.md:14` + `CURRENT_STATUS.md:55-56` — Kritiklik: MAJOR

4. "128px @ PPU64 @ prefab scale 0.5 => ~1 world unit height" is documented; the "~108px at 1080p (~10% screen height)" math is consistent IF camera ortho size=5 and 1080 vertical resolution. — `CLAUDE.md:252-253` — Kritiklik: MINOR
- Math check (assumption: 1080px vertical):
  - Ortho size 5 => vertical world span = 10 units.
  - 1080px / 10u = 108 px/u.
  - 128px @ PPU64 => 2u sprite; prefab scale 0.5 => 1u.
  - 1u => 108px => 10% of 1080.
- Gap: this is resolution-dependent (1440p/4K change the %).

5. "PixelLab Create Character naturally outputs 8 directions so 4-dir saves no cost" is PARTIALLY supported by provided PixelLab docs, but the exact UI/tool naming is inconsistent. — `F:/Antigravity Projeler/Pixellab/PIXELLAB_API_V2.md:45-85` + `F:/Antigravity Projeler/Pixellab/PIXELLAB_AI_MASTER_MAP.json:1048-1095` + `CURRENT_STATUS.md:45-48` — Kritiklik: MAJOR
- Evidence:
  - API has a dedicated `generate-8-rotations-v2` endpoint returning 8 directions in a fixed order.
  - MASTER_MAP shows a "Create 8-directional sprite PRO" flow with a required method selector and a generate button.
  - CURRENT_STATUS says "Base sprite tool: PixelLab Create Character 128x128" (naming differs from MASTER_MAP).
- Gap:
  - The doc set does not prove the UI has (or lacks) a cheaper 4-direction generation mode.
  - Conclusion for now: if you use the 8-rotation flow, you pay for 8 outputs; cost-savings via "only use 4" is not supported.

6. Production-cost axis has a potential internal conflict: the analysis quotes large generation counts for Create Character, while MASTER_MAP snapshot shows "Generate · 20 generations" in the 8-direction flow, and STYLE_BIBLE template math implies 48 gens for full player template pack. — `PIXELLAB_AI_MASTER_MAP.json:1088-1095` + `TASARIM/STYLE_BIBLE.md:416-428` — Kritiklik: MAJOR
- Evidence:
  - MASTER_MAP includes a generate button label with a concrete number (20).
  - STYLE_BIBLE says Player (8-dir): 6 animations x 8 directions = 48 gens (template).
- Gap:
  - Those numbers may refer to different operations (base 8-rotation vs template animation vs custom animation).
  - The analysis should not hardcode a single cost model without specifying operation type.

7. Clip-count arithmetic is consistent (7 anim x 8 dir x 10 class = 560), but it does not match STYLE_BIBLE's "player template = 6 animations" list 1:1. — `TASARIM/STYLE_BIBLE.md:416-428` + `CURRENT_STATUS.md:59` — Kritiklik: MINOR
- Evidence:
  - STYLE_BIBLE template list includes walk; CURRENT_STATUS says walk is removed (run locomotion).
  - Analysis includes 2 skills (Z/X) which are not part of the template list.
- Gap:
  - Need an explicit canonical "per-class animation list" document for budgeting.

8. Cross-class "ghost shader + body reuse" claim is plausible but was NOT evidenced inside the Part B files; evidence exists elsewhere in repo. — `GUIDES/GHOST_ATTACK_SPEC.md:27` + `CURRENT_STATUS.md:67` — Kritiklik: MINOR
- Evidence:
  - GHOST_ATTACK_SPEC references `CrossClassGhostEffect.cs` and tinting via MaterialPropertyBlock (suggests reuse+tint strategy).
  - CURRENT_STATUS notes CrossClassSkillManager exists.
- Gap:
  - Part B list did not include GHOST_ATTACK_SPEC, so the analysis should cite it explicitly if it relies on it.

## Missing considerations
- Texture memory + atlas strategy:
  - 8-dir x multi-anim can balloon texture count if stored as many separate textures; needs an explicit packing plan (SpriteAtlas usage, max atlas size, compression policy).
- Animation event timing:
  - Hit-stop, camera shake, trails help perceived smoothness, but they are timing-sensitive; needs a test plan per animation category (run/attack/skill).
- Death/hit reactions in 4-dir:
  - This is a behavior/UX decision. If death direction is used for gameplay readability (e.g., directional knockback/death pose), 4-dir may be noticeable. Needs Claude decision.

## Recommendation
NEEDS CLAUDE DECISION ON:
1. Confirm the actual PixelLab cost model per operation (base 8-rotation vs template animation vs custom V3) and update the analysis to avoid a single ambiguous "gen cost" number.
2. Decide whether death/hit reactions are allowed to be 4-direction, given current 8-direction policy for player locomotion and aiming.

KEEP AS-IS (supported by current locked decisions and docs):
- 128px baseline for player-class characters.
- 8-direction for main locomotion and aim-facing.

