ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read allowed: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# TASK: Review the demo "big work" priority against the ACTUAL repo state (CODE LENS)

You are the **code/repo-state reviewer**. Do NOT write code. Your job: validate the
candidate big-work list below against what is ACTUALLY in the repo, and rank it for a
**playable + Steam-wishlist-able demo**. Be concrete with file references.

## Candidate big-work items (8)
1. **Combat-feel polish + F5 playtest** (A5 gate — user-manual feel lock)
2. **Weapon system live-test** — mount/swing/VFX import + Resources/WeaponDatabase.asset wiring (mount code is LIVE but UNCOMMITTED)
3. **Boss fight** — PenitentSovereign (BossHealthBar + death→DemoComplete + class-select BYPASS; sprite MISSING)
4. **Mob variety + art** — FractureImp + ShardWalker + HollowHulk; archive-restore vs PixelLab vs RTX-local
5. **Audio skeleton** — procedural/placeholder music + SFX (AudioManager.cs exists, UNCOMMITTED, clips private)
6. **Live editor T3 integration** — scaffold ready in STAGING/livetool_t3/, NOT integrated into Assets/
7. **Map/minimap + gate preview** — MiniMap.cs + MapPanelUI.cs + MapProgressController.cs exist
8. **Demo loop E2E (5 rooms) + Victory Wishlist CTA** — RoomLoader + RoomSequenceData + Gate exist

## Current state (from CURRENT_STATUS.md S6-EXEC — verify against code, flag if stale)
- DONE+committed: rank-1 HUD (HP+Rage), rank-3 hit-confirm WIRED (SlashArc+white-flash HitFlashDriver+HitSpark), rank-4 SkillBarUI, rank-6 RoomTransitionFX fade. PlayerClassManager in scene.
- ALREADY WORKING (do not redo): hitstop 0.04s, screen shake, floating damage numbers, RageSystem, combo+knockback, dash i-frame.
- PENDING: rank-2 draft play-verify (DraftManager #1 fix — does it list real skills?), rank-5a death-screen zero-scale (UNVERIFIED), rank-5b Victory Wishlist CTA, rank-7 boss, rank-9 duplicate "Systems" GO cleanup, F5 visual playtest of hit-confirm.
- Known bug: boss-death → class-select races with Victory (PenitentSovereign.cs:571 + RoomLoader:346) → boss demo needs class-select BYPASS.

## Canonical NLM verdict (anchor — validate or challenge it)
Order: (1) Combat-feel + Weapon → A5 freeze gate, (2) Audio skeleton (game-feel ROI #1), (3) Demo E2E + Wishlist CTA, then (4) Mob/Boss art AFTER A5 (no juice-on-placeholders = asset churn). DEFER: T3 editor, Map/minimap. FractureImp archive-restore is 0-gen → autonomous-safe.

## DELIVER (write your result to the DONE file, concise, evidence-based)
For EACH of the 8 items:
- **% done** (verify in code, cite files)
- **real remaining effort**: S / M / L
- **blocker/risk** (concrete, with file refs)
- **autonomous-safe?** (can Opus build it without user art/audio gen or manual playtest?)
Then: **your ranked top-5** for next autonomous work, and **anything the NLM canonical order got WRONG about the actual code state** (e.g. item already done, or hidden dependency). 1-2 sentence rationale per ranking. No fluff.
