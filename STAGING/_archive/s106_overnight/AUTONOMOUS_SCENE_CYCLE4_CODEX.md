# Scene Composition Cycle 4 (Final Micro-Polish) — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: FINAL micro-polish. Antigravity verdict (POLISH-CYCLE-4) identified 3 numeric tweaks. No new art. Close the autonomous loop. This is the LAST scene dispatch — after this, ship or stop.

## PRIMARY INPUT
- **`STAGING/s106_overnight/SCENE_V4_REVIEW_VERDICT_AGY.md`** — final verdict + 3 micro tweaks

## TARGETED CHANGES (ONLY THESE 3 — no scope creep)

### Tweak 1 — Global Light boost
- Find `Global Light 2D` GameObject in PlayableArena
- Set `m_Intensity` from current 0.22 → **0.38**

### Tweak 2 — Central portal HDR boost
- Find `CentralPortal_CyanGlow` Light2D (child of CentralPortal)
- Set intensity from 2.5 → **5.0**
- If HDR shader available on the cyan glow sprite: set its material's emission color to HDR cyan (R=0, G=2, B=2) — but ONLY if there's a clean way (don't fight it)

### Tweak 3 — Brazier warm light boost + Bloom enable
- For all 4 brazier warm lights (`Brazier_NW_WarmLight`, `Brazier_NE_WarmLight`, `Brazier_SW_WarmLight`, `Brazier_SE_WarmLight`):
  - Set intensity from 2.2 → **4.5**
- Enable URP Post-Process Bloom:
  - Find or create a Volume GameObject in scene (Global Volume)
  - Add Bloom override: Intensity = 0.7, Threshold = 0.9, Scatter = 0.7, Tint = white
  - Ensure URP Renderer Asset has Post-processing enabled on the URP Asset
  - If Volume already exists, just modify its profile

## PROCESS

1. Phase 0: Open scene, find Light2D + Volume references
2. Phase 1: Apply 3 tweaks
3. Phase 2: Save scene
4. Phase 3: Screenshot (game view 1280×720 + side-by-side vs M3)
5. Phase 4: Done report

## DELIVERABLES

- Modified `Assets/Scenes/Test/PlayableArena.unity`
- `STAGING/s106_overnight/scene_v5_match_attempt.png`
- `STAGING/s106_overnight/scene_v5_vs_M3.png`
- `STAGING/s106_overnight/SCENE_V5_REPORT.md`:
  - Confirmed all 3 tweaks applied with before/after values
  - Note if HDR or Bloom path had issues
  - Honest comparison vs M3 (1-10 score)
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`

## CONSTRAINTS
- ONLY the 3 tweaks above — no other scene changes
- No new GameObjects beyond a Global Volume (if it doesn't exist)
- No new sprite assets
- Preserve all 13 Light2D target sorting layers from Cycle 3
- 0 error 0 warning
- Single scene save

## TIME ESTIMATE
~20-30 min at xhigh.

This is the FINAL cycle. After DONE:
- Opus reviews v5_vs_M3
- If acceptable → SHIP, write final memory + status close
- If still gap → accept the asset-density ceiling and ship anyway
