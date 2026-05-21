# Codex Task — Opus RIMA Mechanic Verdict Final Review

ACTIVE RULES: (1) think before deciding (2) honest verdict, no padding (3) cross-reference impl reality (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Opus (rima-design agent) just produced strategic verdict on RIMA's mechanical identity. Full text: `STAGING/RIMA_MECHANIC_ANTI_GENERIC_OPUS.md`.

User explicitly requested: "o codexe review yaptırsın son kararı versin ne gelirse sana söylesin"
= "have Codex review it, give final decision, tell me whatever comes"

Your job: pragmatic technical reality check on Opus's recommendations. Give the FINAL DECISION orchestrator should present to user.

## Opus's key claims to verify or challenge

1. **Signature claim:** RIMA's unique angle is "10 classes × 10 unique resource economies" (Rage/Focus/Tension/Heat/Fury/Charge/Hex/Mana+Element/Energy+Combo/Charges). Hades/DC/Brotato don't do this.

2. **Implementation reality:** 17 systems 100% designed, ~20% average implemented, only Warblade Rage actually playable. Verify by checking actual code.

3. **CB pivot honesty:** CB has tighter single-sentence pitch ("Paint the floor. Trigger the chain. Erase the room."), RIMA doesn't have equivalent. CB's apparent freshness is partly pitch clarity, not just newness.

4. **Cut recommendations (acımasız):**
   - 80 evolution points → 40
   - 50 Shadow Echo → 20
   - 12 skills/class → 8
   - 9 family tags → 4
   - Secondary class defer Phase 5
   - Steam EA: 6 classes ship, 4 post-launch
   - 2 Act + Final EA, Act 3 DLC
   - ~300-400h dev work saved

5. **Validation Gate:** 5-7 day "Two-Class Combat Stress Test"
   - Day 1: Ronin BasicAttackProfile + 4 skills (Warblade pattern copy via Codex, ~12-15h)
   - Day 2: Tension resource UI + Sakura Veil deflect (~6h)
   - Day 3: Hit feel tuning matrix (40/80/50ms hitstop, white flash) (~6-8h)
   - Day 4: Cross-class T1 echo Warblade→Ronin (~4-5h)
   - Day 5: A/B playtest 5min each class
   - Day 6: PASS/FAIL/MIXED decision
   - Total: ~30-40h, 1 week
   - **VISUAL ASSET PRODUCTION FROZEN** during this gate

6. **PASS criteria (4 questions):**
   - Warblade vs Ronin "farklı oyun" hissi?
   - Parry 50ms micro-freeze + white flash hissedildi mi?
   - Cross-class echo dopamin mi clutter mı?
   - "Tekrar oynamak isterim" hissi var mı?

## Your specific verification tasks

1. **Code reality check:** Briefly look at `Assets/Scripts/Combat/`, `Assets/Data/Classes/`, `Assets/Scripts/CombatFeel/`. Does Opus's 15% Warblade-only claim hold? Or is more actually implemented than Opus saw?

2. **Pattern-copy feasibility:** Is "Codex copies Warblade → Ronin in 12-15h" realistic? Verify by checking how Warblade is structured (single class implementation pattern).

3. **Cut recommendations sanity:** Are the cuts SAFE (preserve signature) or destructive (remove unique features)?

4. **Validation gate completeness:** Does the 4-question checklist truly cover "is RIMA generic?" or are there blind spots?

5. **CB pivot probability:** Honest read — given current state, is CB pivot the right call if validation fails?

## Required output — `STAGING/CODEX_DONE_opus_mechanic_verdict_review.md`

```
# FINAL VERDICT (single paragraph)
[What orchestrator should tell user. Concrete next action.]

# 1. Opus claims verification
[Each numbered claim: VERIFIED / CHALLENGED / NUANCED with evidence]

# 2. Code reality vs Opus's 20% impl claim
[Quick code spot-check evidence]

# 3. Validation gate sanity
[Is 5-7 day Two-Class test the right gate? Better alternative?]

# 4. Cut recommendations refinement
[Which cuts are safe-and-recommended, which are risky]

# 5. CB pivot probability honest take
[Given current state, if validation fails what's the actual move]

# 6. Final action item
[Exactly one thing orchestrator should dispatch/do next]
```

Effort: medium. ~15-20 min. Be brutal-honest. User has been gaslighting themselves with sunk cost for 6 months — they need straight signal.

If Opus is right, say so. If Opus overcautions, say that too.
