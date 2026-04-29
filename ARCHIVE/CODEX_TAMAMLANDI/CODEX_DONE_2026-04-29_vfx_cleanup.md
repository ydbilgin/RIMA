STATUS: DONE
TASK: CODEX_TASKS.md — Task 1 + Task 2 + Task 3

COMPLETED:
- Task 1: Reviewed current 10 anchor PNGs visually.
  - Cohesion score: 72/100.
  - LOCKED: Warblade, Ravager.
  - EDIT: Elementalist, Shadowblade, Ranger, Ronin, Gunslinger, Summoner, Hexer.
  - REGEN: Brawler.
  - Appended `CODEX SELF-EVALUATION — 2026-04-29` to `STAGING/PROMPTS_S43/CODEX_SPRITE_REVIEW_FIXPLAN.md`.
- Task 2: Converted verdicts into PixelLab prompts.
  - Appended `BÖLÜM D — CODEX SELF-EVALUATION FIXES` to `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`.
  - Flagged conflicts: Ranger accent color mismatch; Elementalist no fixed hex because orb is runtime-element-tinted.
- Task 3: Implemented Unity VFX scripts.
  - Added `Assets/Scripts/VFX/HandGlowVFX.cs`.
  - Added `Assets/Scripts/VFX/RiftGlowVFX.cs`.
  - Added `SkillFlowTracker.OnSkillUsed` event and fired it from `NotifySkillUsed()`.
  - Appended `BÖLÜM 3 — Prefab Wiring Plan` to `STAGING/PROMPTS_S43/UNITY_VFX_HAND_EFFECTS.md`.

VERIFICATION:
- Ran `dotnet restore RIMA.Runtime.csproj`.
- Ran `dotnet build RIMA.Runtime.csproj --no-restore`.
- Result: build succeeded, 0 errors.
- Warnings: 33 existing warnings, mostly obsolete Unity/TMP API usage and unused fields; no new compile errors from added VFX scripts.
- Unity MCP used: NO.

NOTES / FLAGS:
- Did not modify Unity scenes or prefab assets.
- Did not open or start Unity MCP.
- Existing working tree contains unrelated changes/untracked files not created by this task; left untouched.
- `git status` also shows `STAGING/anchors/elementalist/metadata.json` deleted, but this task did not intentionally delete that file.

FILES CHANGED BY THIS TASK:
- `STAGING/PROMPTS_S43/CODEX_SPRITE_REVIEW_FIXPLAN.md`
- `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md`
- `STAGING/PROMPTS_S43/UNITY_VFX_HAND_EFFECTS.md`
- `Assets/Scripts/VFX/HandGlowVFX.cs`
- `Assets/Scripts/VFX/RiftGlowVFX.cs`
- `Assets/Scripts/Systems/SkillFlowTracker.cs`
- `CODEX_DONE.md`
- `CODEX_TASKS.md` cleared

NEXT_SIGNAL: "Claude review Bölüm D prompt conflicts and Unity VFX scripts; Unity MCP can be opened only on explicit request for prefab wiring."

---

STATUS: DONE
TASK: CODEX_TASKS.md — Project-Wide File Cleanup

COMPLETED:
- Batch 1: Deleted S42-era and old one-time files from `STAGING/` root.
- Batch 2: Skipped `STAGING/PROMPTS_WARBLADE.md` because Batch 1 already deleted it.
- Batch 3: Archived superseded guide files to `ARCHIVE/GUIDES_SUPERSEDED/`.
- Batch 4: Archived one-time TASARIM prompt/plan files to `ARCHIVE/`.
- Batch 5: Deleted selected `STAGING/discord_pixellab/` intermediate files.

FILES DELETED: 43
- `STAGING/16varcharprompt.md`
- `STAGING/CODEX_CLASSES_S_PROMPTS.md`
- `STAGING/CODEX_CLASSES_S_PROMPTS_V2.md`
- `STAGING/CODEX_CLASSES_S_PROMPTS_V2_BOARD_SHORT.md`
- `STAGING/CODEX_REVIEW_RESULT.md`
- `STAGING/GEMINI_REVIEW_S43_ROSTER.md`
- `STAGING/GEMINI_SKILL_SYSTEM_REVIEW_PROMPT_S43.md`
- `STAGING/GEMINI_VISUAL_QC_ROSTER_CONSISTENCY_S43.md`
- `STAGING/IMAGE2_REMAINING_S_WITH_WARBLADE_REFERENCE.md`
- `STAGING/IMAGE2_RIMA_BOARD_MESSAGE.md`
- `STAGING/IMAGE2_SINGLE_CLASS_PRODUCTION_MESSAGE.md`
- `STAGING/KESIF_PROMPTS_S42.md`
- `STAGING/MOB_BOSS_REDESIGN_S42.md`
- `STAGING/PILOT4_PROMPTS_S42.md`
- `STAGING/PIPELINE_8DIR_CHATGPT.md`
- `STAGING/PIXELLAB_SCALE_RESEARCH.md`
- `STAGING/PROMPTS_BRAWLER.md`
- `STAGING/PROMPTS_ELEMENTALIST.md`
- `STAGING/PROMPTS_GUNSLINGER.md`
- `STAGING/PROMPTS_HEXER.md`
- `STAGING/PROMPTS_RANGER_SHADOWBLADE.md`
- `STAGING/PROMPTS_RAVAGER.md`
- `STAGING/PROMPTS_RONIN.md`
- `STAGING/PROMPTS_SOUTH_ALL_CLASSES.md`
- `STAGING/PROMPTS_SUMMONER.md`
- `STAGING/PROMPTS_WARBLADE.md`
- `STAGING/ROSTER_PROMPT_S42.md`
- `STAGING/SKILL_REVIZYON_PLANI.md`
- `STAGING/SKILL_SYSTEM_FEEDBACK_CODEX_S43.md`
- `STAGING/SKILL_SYSTEM_FEEDBACK_GEMINI_S43.md`
- `STAGING/WARBLADE_PILOT_PROMPT_S42.md`
- `STAGING/WARBLADE_PRO_FORGE_S42.md`
- `STAGING/WARBLADE_RUN_KEYFRAME_PROMPTS.md`
- `STAGING/WARBLADE_RUN_PROMPTS_V2.md`
- `STAGING/_AUDIT_REPORT.md`
- `STAGING/_CLEANUP_REPORT_2026-04-29.md`
- `STAGING/compact_pass_s42_report.md`
- `STAGING/RIFT_REMNANT_PLAN_S43.md`
- `STAGING/discord_pixellab/_gemini_FULL_map_prompt.md`
- `STAGING/discord_pixellab/_gemini_FULL_map_prompt_v2.md`
- `STAGING/discord_pixellab/_gemini_input.md`
- `STAGING/discord_pixellab/_gemini_map_prompt.md`
- `STAGING/discord_pixellab/PIXELLAB_KNOWLEDGE_MAP.md`

FILES ARCHIVED: 5
- `GUIDES/PIXELLAB_ANCHOR_PIPELINE_S42.md` -> `ARCHIVE/GUIDES_SUPERSEDED/PIXELLAB_ANCHOR_PIPELINE_S42.md`
- `GUIDES/S41_CLASS_PIPELINE_GUIDE.md` -> `ARCHIVE/GUIDES_SUPERSEDED/S41_CLASS_PIPELINE_GUIDE.md`
- `GUIDES/CHATGPT_CHARACTER_PIPELINE.md` -> `ARCHIVE/GUIDES_SUPERSEDED/CHATGPT_CHARACTER_PIPELINE.md`
- `TASARIM/CHATGPT_CLASS_REGEN_PROMPTS_2026-04-24.md` -> `ARCHIVE/CHATGPT_CLASS_REGEN_PROMPTS_2026-04-24.md`
- `TASARIM/GEMINI_CONCEPT_PROMPTS.md` -> `ARCHIVE/GEMINI_CONCEPT_PROMPTS.md`

FILES SKIPPED:
- `STAGING/PROMPTS_WARBLADE.md` — missing during Batch 2 because it was deleted in Batch 1 as instructed.

VERIFICATION:
- Confirmed `STAGING/discord_pixellab/PIXELLAB_KNOWLEDGE_MAP_FULL.md` still exists.
- Confirmed `STAGING/discord_pixellab/digest/` still exists.
- Confirmed `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md` still exists.
- Confirmed all 5 archive destinations exist.

ERRORS: NONE

NOTES:
- Did not touch `Assets/`, `Packages/`, `ProjectSettings/`, `.claude/`, or `ARCHIVE/` contents except adding the specified archived files.
- Existing unrelated working tree changes were left untouched.

NEXT_SIGNAL: "Claude QC cleanup result; run git status review before commit."
