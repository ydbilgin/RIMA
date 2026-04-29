# KIRO TASK — rima_sim.py v4 Balance Update
*Date: 2026-04-14 | Read ONLY this file. Apply all steps in order.*

> SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.

---

## RISK LEVEL: LOW
All changes are deterministic find-and-replace + mechanical code additions. No judgment required.

---

## FILES TOUCHED

- `F:\Antigravity Projeler\2d roguelite\RIMA\rima_sim.py`

Do not touch any other file.

---

## STOP AND ESCALATE if:
- Any expected old value is not found in the file
- Any step is ambiguous
- You are about to edit any file not listed above

---

## STEP 1 — Update version comment (line 3)

Find exact string:
```
RIMA Balance Simulator v3.0
```
Replace with:
```
RIMA Balance Simulator v4.0
```

---

## STEP 2 — Add MobType dataclass

Find exact string (after BossConfig class, before CLASS CONFIG section):
```
# ─────────────────────────────────────────────
# CLASS CORE TANIMI
```

Insert the following block IMMEDIATELY BEFORE that line (add a blank line before the new block):

```python
# ─────────────────────────────────────────────
# MOB TİPLERİ  (sadece veri — simülasyon mantığı değişmez)
# ─────────────────────────────────────────────
@dataclass
class MobType:
    name:           str
    hp:             float
    dps:            float
    count:          int
    approach_speed: float   # APPROACH_BASE çarpanı
    notes:          str = ""

MOB_TYPES = [
    MobType("Hollow Grunt",  hp=180, dps=18, count=5, approach_speed=1.0,  notes="Act 1 std"),
    MobType("Bone Revenant", hp=220, dps=22, count=5, approach_speed=0.9,  notes="Act 1-2 std"),
    MobType("Rift Shade",    hp=200, dps=20, count=6, approach_speed=1.2,  notes="Act 2 std — hızlı ama zayıf"),
    MobType("Iron Sentinel", hp=280, dps=25, count=4, approach_speed=0.8,  notes="Act 3 std — yavaş ama sağlam"),
    MobType("Void Stalker",  hp=240, dps=28, count=5, approach_speed=1.1,  notes="Act 4 std"),
    MobType("Crimson Elite", hp=500, dps=30, count=2, approach_speed=0.85, notes="Act 1-2 elite"),
    MobType("Iron Warden",   hp=650, dps=35, count=2, approach_speed=0.75, notes="Act 3-4 elite"),
]


```

---

## STEP 3 — Add two fields to RoomResult dataclass

Find exact string:
```
    resource_eff_pct:  float
```

Replace with:
```
    resource_eff_pct:  float
    bone_revenant_ttk: float = 0.0
    bone_revenant_surv: bool = True
    iron_sentinel_ttk: float = 0.0
    iron_sentinel_surv: bool = True
```

---

## STEP 4 — Class stat changes

### 4a — Ranger: max_hp 320→380, defense_rating 0.12→0.18

Find exact string:
```
_RANGER = ClassConfig(            # balance fix: HP 300→320, def 0.10→0.12
    name="Ranger",
    lmb_damage=18, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=4,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=10,
    burst_threshold=75, burst_damage=250, burst_aoe_targets=4,
    burst_cost=0, burst_cooldown=30,
    max_hp=320, defense_rating=0.12,
```

Replace with:
```
_RANGER = ClassConfig(            # balance fix v4: HP 320→380, def 0.12→0.18
    name="Ranger",
    lmb_damage=18, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=4,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=10,
    burst_threshold=75, burst_damage=250, burst_aoe_targets=4,
    burst_cost=0, burst_cooldown=30,
    max_hp=380, defense_rating=0.18,
```

### 4b — Elementalist: max_hp 275→320, defense_rating 0.08→0.13

Find exact string:
```
_ELEMENTALIST = ClassConfig(      # balance fix: HP 250→275, def 0.05→0.08
    name="Elementalist",
    lmb_damage=20, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=3,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=8,
    burst_threshold=100, burst_damage=400, burst_aoe_targets=5,
    burst_cost=100, burst_cooldown=20,
    max_hp=275, defense_rating=0.08,
```

Replace with:
```
_ELEMENTALIST = ClassConfig(      # balance fix v4: HP 275→320, def 0.08→0.13
    name="Elementalist",
    lmb_damage=20, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=3,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=8,
    burst_threshold=100, burst_damage=400, burst_aoe_targets=5,
    burst_cost=100, burst_cooldown=20,
    max_hp=320, defense_rating=0.13,
```

### 4c — Summoner: max_hp 300→340

Find exact string:
```
    max_hp=300, defense_rating=0.10,
    move_speed=0.85, contact_scale=1.0,
)

_HEXER
```

Replace with:
```
    max_hp=340, defense_rating=0.10,
    move_speed=0.85, contact_scale=1.0,
)

_HEXER
```

### 4d — Hexer: resource_drain_per_sec 2→1.5

Find exact string:
```
    resource_max=10, resource_drain_per_sec=2,
```

Replace with:
```
    resource_max=10, resource_drain_per_sec=1.5,
```

---

## STEP 5 — Build stat changes

### 5a — Ravager Berserk: skill_avg_dps 28→45

Find exact string:
```
    Build("Ravager — Berserk", _RAVAGER,
          skill_avg_dps=28, note="Rend + War Cry + Berserker's Seal + Carnage Rush"),
```

Replace with:
```
    Build("Ravager — Berserk", _RAVAGER,
          skill_avg_dps=45, note="Rend + War Cry + Berserker's Seal + Carnage Rush"),
```

### 5b — Hexer Debuff: skill_avg_dps 14→22

Find exact string:
```
    Build("Hexer — Debuff", _HEXER,
          skill_avg_dps=14, skill_def_bonus=0.07, skill_regen_bonus=1,
```

Replace with:
```
    Build("Hexer — Debuff", _HEXER,
          skill_avg_dps=22, skill_def_bonus=0.07, skill_regen_bonus=1,
```

### 5c — Summoner Army: skill_avg_dps 55→75

Find exact string:
```
    Build("Summoner — Army", _SUMMONER,
          skill_avg_dps=55,  # 3 minyon × ~18 DPS tahmin
```

Replace with:
```
    Build("Summoner — Army", _SUMMONER,
          skill_avg_dps=75,  # 3 minyon × ~25 DPS tahmin (v4 fix)
```

### 5d — Summoner Support: skill_avg_dps 38→55

Find exact string:
```
    Build("Summoner — Support", _SUMMONER,
          skill_avg_dps=38, skill_def_bonus=0.10,
```

Replace with:
```
    Build("Summoner — Support", _SUMMONER,
          skill_avg_dps=55, skill_def_bonus=0.10,
```

---

## STEP 6 — Boss stat changes

### 6a — Void Weaver: Void Collapse damage 130→95, cooldown 11→13

Find exact string:
```
            BossSkill("Void Collapse",cooldown=11.0, damage=130, skill_type="ARENA", dodge_factor=0.22),
```

Replace with:
```
            BossSkill("Void Collapse",cooldown=13.0, damage=95,  skill_type="ARENA", dodge_factor=0.22),
```

### 6b — Void Weaver: Void Drain cooldown 28→35

Find exact string:
```
            BossSkill("Void Drain",   cooldown=28.0, damage=0,   skill_type="DRAIN",
```

Replace with:
```
            BossSkill("Void Drain",   cooldown=35.0, damage=0,   skill_type="DRAIN",
```

### 6c — Iron Colossus: dps_p1 10→8, dps_p2 15→12

Find exact string:
```
        dps_p1 = 10, dps_p2 = 15,
        p1_skills = [
            # Hedef: Act 1 ~80%
```

**NOTE:** That comment is not on that line. Find this instead:

Find exact string:
```
        hp   = 2800,
        phase_threshold = 0.40,
        dps_p1 = 10, dps_p2 = 15,
```

Replace with:
```
        hp   = 2800,
        phase_threshold = 0.40,
        dps_p1 = 8, dps_p2 = 12,
```

### 6d — Iron Colossus P1: Cannon Blast damage 120→90, cooldown 5→6

Find exact string:
```
            BossSkill("Cannon Blast", cooldown=5.0,  damage=120, skill_type="BURST", dodge_factor=0.68),
```

Replace with:
```
            BossSkill("Cannon Blast", cooldown=6.0,  damage=90,  skill_type="BURST", dodge_factor=0.68),
```

### 6e — Iron Colossus P2: Cannon Blast+ damage 140→110, Molten Core damage 100→80

Find exact string:
```
            BossSkill("Cannon Blast+",cooldown=4.0,  damage=140, skill_type="BURST", dodge_factor=0.62),
            BossSkill("Molten Core",  cooldown=8.0,  damage=100, skill_type="ARENA", dodge_factor=0.18),
```

Replace with:
```
            BossSkill("Cannon Blast+",cooldown=4.0,  damage=110, skill_type="BURST", dodge_factor=0.62),
            BossSkill("Molten Core",  cooldown=8.0,  damage=80,  skill_type="ARENA", dodge_factor=0.18),
```

### 6f — Abyssal Judge: dps_p2 14→12

Find exact string:
```
        dps_p1 = 8, dps_p2 = 14,
        transition_dur  = 2.0,
```

Replace with:
```
        dps_p1 = 8, dps_p2 = 12,
        transition_dur  = 2.0,
```

### 6g — Abyssal Judge P1: Verdict damage 105→85

Find exact string:
```
            BossSkill("Verdict",       cooldown=7.0,  damage=105, skill_type="BURST", dodge_factor=0.55),
```

Replace with:
```
            BossSkill("Verdict",       cooldown=7.0,  damage=85,  skill_type="BURST", dodge_factor=0.55),
```

### 6h — Abyssal Judge P2: Final Verdict 155→120, Judgment Day 145→110

Find exact string:
```
            BossSkill("Final Verdict", cooldown=5.0,  damage=155, skill_type="BURST", dodge_factor=0.46),
            BossSkill("Judgment Day",  cooldown=12.0, damage=145, skill_type="ARENA", dodge_factor=0.16),
```

Replace with:
```
            BossSkill("Final Verdict", cooldown=5.0,  damage=120, skill_type="BURST", dodge_factor=0.46),
            BossSkill("Judgment Day",  cooldown=12.0, damage=110, skill_type="ARENA", dodge_factor=0.16),
```

---

## STEP 7 — run_all: add Bone Revenant + Iron Sentinel room simulations

Find exact string:
```
        ttk_s, bursts, dps, res_eff, hp_pct, taken, surv = simulate_room(
            b, e_std, ENEMY_DPS_MOB, APPROACH_BASE)
        ttk_e, *_ = simulate_room(b, e_elite, ENEMY_DPS_MOB, APPROACH_BASE)

        room_results.append(RoomResult(
            build_label      = b.label,
            std_ttk          = round(ttk_s, 1),
            elite_ttk        = round(ttk_e, 1),
            bursts_in_std    = bursts,
            avg_dps          = round(dps, 1),
            hp_remaining_pct = round(hp_pct, 1),
            survived         = surv,
            dmg_taken        = round(taken, 0),
            resource_eff_pct = round(res_eff * 100, 1),
        ))
```

Replace with:
```
        ttk_s, bursts, dps, res_eff, hp_pct, taken, surv = simulate_room(
            b, e_std, ENEMY_DPS_MOB, APPROACH_BASE)
        ttk_e, *_ = simulate_room(b, e_elite, ENEMY_DPS_MOB, APPROACH_BASE)

        # Ek oda simülasyonları: Bone Revenant (Act 1-2) + Iron Sentinel (Act 3)
        _br = MOB_TYPES[1]  # Bone Revenant
        _is = MOB_TYPES[3]  # Iron Sentinel
        ttk_br, _, _, _, _, _, surv_br = simulate_room(
            b, [_br.hp] * _br.count, _br.dps, APPROACH_BASE * _br.approach_speed)
        ttk_is, _, _, _, _, _, surv_is = simulate_room(
            b, [_is.hp] * _is.count, _is.dps, APPROACH_BASE * _is.approach_speed)

        room_results.append(RoomResult(
            build_label        = b.label,
            std_ttk            = round(ttk_s, 1),
            elite_ttk          = round(ttk_e, 1),
            bursts_in_std      = bursts,
            avg_dps            = round(dps, 1),
            hp_remaining_pct   = round(hp_pct, 1),
            survived           = surv,
            dmg_taken          = round(taken, 0),
            resource_eff_pct   = round(res_eff * 100, 1),
            bone_revenant_ttk  = round(ttk_br, 1),
            bone_revenant_surv = surv_br,
            iron_sentinel_ttk  = round(ttk_is, 1),
            iron_sentinel_surv = surv_is,
        ))
```

---

## STEP 8 — Update print_room_report to show new columns

Find exact string:
```
    print("\n" + "=" * W)
    print(f"  RIMA BALANCE SIM v3.0  —  ODA RAPORU  |  Mob DPS={ENEMY_DPS_MOB}  |  Oda: {ENEMIES_STD}×{ENEMY_HP_STD}HP std, {ENEMIES_ELITE}×{ENEMY_HP_ELITE}HP elite")
    print("=" * W)
    hdr = f"{'Build':<26} {'StdTTK':>8} {'EliteTTK':>9} {'Bursts':>7} {'DPS':>7} {'HP%':>7} {'Taken':>7} {'Res%':>6}  Flags"
    print(hdr); print("-" * W)
```

Replace with:
```
    print("\n" + "=" * W)
    print(f"  RIMA BALANCE SIM v4.0  —  ODA RAPORU  |  Mob DPS={ENEMY_DPS_MOB}  |  Oda: {ENEMIES_STD}×{ENEMY_HP_STD}HP std, {ENEMIES_ELITE}×{ENEMY_HP_ELITE}HP elite")
    print("=" * W)
    hdr = f"{'Build':<26} {'StdTTK':>8} {'EliteTTK':>9} {'Bursts':>7} {'DPS':>7} {'HP%':>7} {'Taken':>7} {'Res%':>6} {'A1-2TTK':>8} {'A3TTK':>7}  Flags"
    print(hdr); print("-" * W)
```

Also find exact string (inside the for loop, the print line):
```
            print(
                f"{r.build_label:<26}{r.std_ttk:>7.1f}s{r.elite_ttk:>8.1f}s"
                f"{r.bursts_in_std:>8}{r.avg_dps:>7.1f}{r.hp_remaining_pct:>6.1f}%"
                f"{r.dmg_taken:>7.0f}{r.resource_eff_pct:>5.1f}%  {'  '.join(flags)}"
            )
```

Replace with:
```
            br_flag = "" if r.bone_revenant_surv else "✗BR"
            is_flag = "" if r.iron_sentinel_surv else "✗IS"
            print(
                f"{r.build_label:<26}{r.std_ttk:>7.1f}s{r.elite_ttk:>8.1f}s"
                f"{r.bursts_in_std:>8}{r.avg_dps:>7.1f}{r.hp_remaining_pct:>6.1f}%"
                f"{r.dmg_taken:>7.0f}{r.resource_eff_pct:>5.1f}%"
                f"{r.bone_revenant_ttk:>7.1f}s{r.iron_sentinel_ttk:>6.1f}s"
                f"  {br_flag}{is_flag}{'  ' if br_flag or is_flag else ''}{'  '.join(flags)}"
            )
```

Also update the W (width) constant. Find:
```
    W = 130
    print("\n" + "=" * W)
    print(f"  RIMA BALANCE SIM v4.0
```

Replace W value:
```
    W = 155
    print("\n" + "=" * W)
    print(f"  RIMA BALANCE SIM v4.0
```

---

## STEP 9 — QC: Run the script

Run:
```
cd "F:\Antigravity Projeler\2d roguelite\RIMA"
python -X utf8 rima_sim.py
```

**PASS criteria:**
- Script runs without any Python errors or exceptions
- Output shows "RIMA BALANCE SIM v4.0" in header
- ODA RAPORU table has columns: StdTTK, EliteTTK, Bursts, DPS, HP%, Taken, Res%, A1-2TTK, A3TTK
- BOSS MATRİSİ table appears with 4 bosses

**FAIL criteria:**
- Any Python traceback or error
- Missing columns in report
- Script hangs

If FAIL: read the error, fix only the exact line causing it, re-run. Do not change unrelated code.

---

## REPORT FILE — Write before saying anything

**Write to:** `F:\Antigravity Projeler\2d roguelite\KIRO_LAST_REPORT.md`

```
# KIRO REPORT — rima_sim v4
Date: 2026-04-14

STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Step 1: version comment updated
  - Step 2: MobType dataclass added
  - Step 3: RoomResult new fields
  - Step 4: class stats (Ranger/Elem/Summoner/Hexer)
  - Step 5: build stats (Ravager/Hexer/Summoner)
  - Step 6: boss stats (VoidWeaver/IronColossus/AbyssalJudge)
  - Step 7: run_all extra simulations
  - Step 8: report columns updated
  - Step 9: QC run result

ERRORS:
  - [exact error] or NONE

QC_RESULT:
  - Script run: PASS / FAIL
  - v4.0 header: PASS / FAIL
  - A1-2TTK column visible: PASS / FAIL

NEXT_SIGNAL: "sim v4 hazır, raporu göster"
```
