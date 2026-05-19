# Codex Task — Ronin Implementation (Warblade Pattern Copy)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Implement **Ronin class** as Warblade pattern-copy with **Tension** resource (the polar opposite ritm of Warblade's Rage). This is Day 1 of the Two-Class Combat Stress Test (per `STAGING/RIMA_MECHANIC_ANTI_GENERIC_OPUS.md` and `STAGING/CODEX_DONE_opus_mechanic_verdict_review.md`).

User decision: implement Ronin AND verify Warblade + Elementalist gaps. **Visual asset production frozen** during this gate.

## Ronin design (NLM canonical)

Query NLM for Ronin design canonical:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Ronin class design — resource Tension mechanics, 4 skill names, base attack profile, Sakura Veil deflect, Final Draw V Burst"
```

Expected canonical (verify against NLM):
- **Resource: Tension** — gains while stationary (`+1/sec idle`), drains while moving (`-2/sec`)
- **BasicAttack: Iaido stance** — slow draw, high commit-beat damage on beat 3 (vs Warblade fast 3-beat)
- **Skills (4 primary):**
  1. **Quickdraw (LMB):** instant dash-strike, costs 20 Tension, refunds 10 on hit
  2. **Iaido Stance (RMB):** root-in-place, gain 5 Tension/sec, exit with empowered slash
  3. **Final Draw (F — V Burst):** spend ALL Tension, slow-mo cone slash, damage scales with Tension spent
  4. **Sakura Veil (R — deflect/parry):** 0.4s frame-perfect counter, refunds 30 Tension on success, white flash + 50ms micro-freeze
- **Cross-class echo placeholder:** when Warblade Beat 3 fires nearby Ronin, trigger Ronin Quickdraw ghost (T1 echo)

## Implementation scope

### Required files (create new, pattern-copy from Warblade)

```
Assets/Scripts/Combat/Classes/Ronin/
├── RoninController.cs              (pattern: WarbladeController.cs)
├── TensionSystem.cs                (pattern: RageSystem.cs — INVERT gain logic)
└── Skills/
    ├── RoninQuickdraw.cs
    ├── RoninIaidoStance.cs
    ├── RoninFinalDraw.cs
    └── RoninSakuraVeil.cs

Assets/Data/Combat/Profiles/
└── Ronin_BasicAttackProfile.asset  (pattern: Warblade_BasicAttackProfile.asset, IaidoStance type)

Assets/Data/Skills/Ronin/
├── ronin_quickdraw.asset
├── ronin_iaido_stance.asset
├── ronin_final_draw.asset
└── ronin_sakura_veil.asset
```

### Required wiring

1. `PlayerClassManager.cs` — add Ronin case, register controller + profile
2. `ClassType` enum — verify Ronin entry exists, add if missing
3. Resource UI prefab — pattern-copy Rage bar → Tension bar (color: cyan-violet vs Rage red)
4. Cross-class echo registry — add Warblade Beat 3 → Ronin Quickdraw T1 echo entry (placeholder VFX OK, audio defer)

### Verify gaps (don't fix, REPORT)

Spot-check these 4 classes and report % completeness:
- Warblade (baseline 100%)
- Elementalist (Mana+Element)
- Ranger (Focus)
- Shadowblade (Energy+Combo)

For each: do BasicAttackProfile, controller, skills, resource system exist and wire to PlayerClassManager? Report gaps. Don't fix Elementalist/Ranger/Shadowblade in this task — just inventory.

## Required output

`STAGING/CODEX_DONE_ronin_implementation.md`:

```
# STATUS
[Ronin LIVE / Ronin LIVE with caveats / BLOCKED]

# Files added
[List]

# Files modified
[List with line ranges]

# Gap inventory (Warblade/Elem/Ranger/Shadowblade)
[Per class: % complete + gaps]

# Tension resource validation
[Idle gain + movement drain test plan, in-editor manual]

# Next steps for Day 2-5 of stress test
[What needs to happen for Tension UI + Sakura Veil deflect feel + cross-class echo]
```

## Acceptance gates

- All new .cs files compile (no errors in Unity Console)
- Ronin Controller can be added to a GameObject in scene without exception
- BasicAttackProfile asset references load correctly
- PlayerClassManager.SwitchClass(ClassType.Ronin) does not throw
- No existing tests break (run RIMA.Combat tests if applicable)

Effort: high. Budget ~12-15h equivalent. Surgical — only touch Combat/Classes folders.
