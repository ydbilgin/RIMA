# RIMA — Queue (2026-06-02 post-/clear, user-given, agent-driven)

> User: "sıraya al, agentları kullanarak yap." Orchestrator = Opus (coordinates only). Execution → cx (laurethayday → if limited, yekta) + ax (Gemini 3.1 Pro / 3.5 Flash) + NLM. Do IN ORDER, one at a time, autonomous (no per-step approval).

## T1 — CLIFF OVERFLOW + FLOOR-GAP TEST  (NOW, agent: cx + ax, I review screenshots)
User: "taşan örneklerden çok fazla var, ben sadece örnek verdim. Floor ortasına 3 boşluk / 4 boşluk koyarak cliff generate et, nasıl görünüyor test et. Testleri ax + cx koordineli yürüt."
- Current placement: per Ground cell, first void-facing FRONT dir [S>SE>SW>E>W] → `cliff_<DIR>` at cellCenter+(0,+0.85), sortLayer Floor order=-30+round(20-y). Asset: `Assets/Sprites/Environment/CliffKit_RefB_pixelified/`.
- Problem: MANY cliffs overflow past floor silhouette (esp. W/E side edges + convex corners). Reference correct = south-edge offset (0,+0.85).
- PLAN:
  - cx: on a COPY scene `_IsoGame_cliffgaptest`, punch interior 3-cell + 4-cell holes → re-place cliffs in 3 variants (VAR0 current / VAR1 interior-push / VAR2 edge-anchored) → screenshots + overflow report. Live scenes untouched.
  - ax (3.1 Pro): geometry diagnosis — correct per-direction offset formula for iso cliff tuck.
  - I review screenshots → pick/tune → cx phase-2 finalizes on 3 scenes (_IsoGame/Map02/Map03), equalizes offsets.
- STATUS: dispatched.

## T2 — REWARD SYSTEM DESIGN  (agent: ax 3.1 Pro + 3.5 Flash + NLM → decision doc)
User: "oyunda ödül sistemi tam nasıl olmalı, buna kafa yorun. ax (3.1 Pro + 3.5 Flash) + NLM kullanabilirsin."
- NLM canon (done): Map Fragment + Skill Draft = reward primitives; Skill Draft = 3-choice (SkillOfferGenerator exists). Hub NPCs Ferryman/Vrel/Sister Mourne/Cartographer; meta currency = Echo + Boss Fragment.
- PLAN: ax 3.1 Pro full design (cadence, choice vs auto-pickup, run-loop + meta tie-in, MVP→full) + ax 3.5 Flash second lens → I synthesize → `STAGING/REWARD_SYSTEM_DECISION_2026-06-02.md`. DOCS ONLY (no code yet unless trivial wire).
- STATUS: ax dispatched (combined with portal Q).

## T3 — PORTAL: horizontal vs vertical + cx production  (agent: ax + cx)
User: "kapı yerine portal düşünmüştük, yatay mı dikey mi nasıl olacak, karar verip cx ile üretim yap."
- NLM canon (done): door→portal canonical; existing code Portal.cs / PortalSpawnController.cs / PortalSpawnAnchor.cs / FanLayoutSolver.cs / RoomTypeData.cs. P1 single→3-choice, P2 3-portal fan, P3 +preview. Orientation NOT specified → decide.
- PLAN: ax recommends orientation (high-top-down readability + cyan-rift fantasy + edge-fan layout) → I decide → cx wires portal into live scenes (replace DoorTrigger→portal) + produce/place sprite. Art = PixelLab gated (together-session) → cx uses placeholder/existing portal art for now, real gen later.
- STATUS: ax dispatched; cx production after orientation locked.

## Routing
cx = laurethayday (bg, cx_dispatch.py, recompile=MCP reload→poll). If laureth 5h-limited → yekta. ax = settings.json model-inline, ONE at a time (race). NLM = parallel OK.
Standing: update CURRENT_STATUS + memory after each job (resumability). Commit GATED.
