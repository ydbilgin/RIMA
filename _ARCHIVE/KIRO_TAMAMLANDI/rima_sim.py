"""
RIMA Balance Simulator v4.0
Çalıştır: python -X utf8 rima_sim.py

v3 yenilikleri (2026-04-14):
  - Boss skill sistemi: BURST / ARENA / SPAWN / DRAIN
  - 4 act boss (Flesh Warden, Void Weaver, Iron Colossus, Abyssal Judge)
  - Hız → boss skill dodge şansı (hızlı class telegraphed saldırıları daha iyi kaçar)
  - Build × Boss matris raporu (her build 4 bossa karşı)
  - Balance fix: Warblade burst_threshold 50 / Hexer HP+lmb_dmg / Ranger HP+def / Elem HP
  - Mob DPS 22 / Boss base DPS 15 (P1) → P2 artar

TODO v4: Oda şablonu bazlı simülasyon
  - Koridor oda → alive tek sıra (contact=1)
  - Açık oda → alive çevreleme (contact yüksek)
  - Boss oda mimarisi → contact_scale boss arketipine göre değişir
  - Bu değişiklikler RoomSelector + BossRoom tasarımı tamamlanınca eklenir.

Modellenemeyen: Summoner minyon econ, CC hasar bonusu, Hexer hedef-bazlı stack,
  Ravager kan siklozu V, Elementalist Lightbreak, Charge banking, stealth.
"""

from dataclasses import dataclass, field
from typing import List, Optional
import sys

# ─────────────────────────────────────────────
# ODA / DÜŞMAN SABİTLERİ
# ─────────────────────────────────────────────
ENEMY_HP_STD     = 200
ENEMY_HP_ELITE   = 500
ENEMY_HP_BOSS    = 2000     # standart boss HP (act boss'ları kendi HP'ini taşır)

ENEMIES_STD      = 5
ENEMIES_ELITE    = 2

ENEMY_DPS_MOB    = 22       # oda mobb'u (dodge varsayımı dahil ortalama)
ENEMY_DPS_BOSS   = 15       # boss P1 (telegraph → dodge penceresi geniş)
APPROACH_BASE    = 2.5      # temel hızda (1.0) mob temas süresi
BOSS_APPROACH    = 0.8      # boss oda merkezinde → daha hızlı temas

BOSS_P2_DPS_MULT = 1.35
BOSS_TRANSITION  = 1.5      # P1→P2 geçiş: hasar yok (her iki yönde)

# Skill dodge hesabı çarpanları
DODGE_BURST_BASE = 1.0      # BURST: dodge_factor = temel hız class'ın gerçek dodge şansı
DODGE_ARENA_BASE = 1.0      # ARENA: aynı formül; dodge_factor değerleri düşük ayarlanmış

SIM_TICK = 0.1
SIM_CAP  = 180.0


# ─────────────────────────────────────────────
# BOSS SKILL TANIMI
# ─────────────────────────────────────────────
@dataclass
class BossSkill:
    name:          str
    cooldown:      float      # saniye
    damage:        float      # oyuncuya ham hasar
    skill_type:    str        # "BURST" | "ARENA" | "SPAWN" | "DRAIN"
    dodge_factor:  float      # 0=kaçınılamaz, 1=tam kaçınılabilir
    spawn_count:   int   = 0  # SPAWN: kaç düşman çıkar
    spawn_hp:      float = 0.0
    drain_amount:  float = 0.0  # DRAIN: kaynak azaltımı
    _timer:        float = field(default=0.0, init=False, repr=False)

    def reset(self):
        self._timer = self.cooldown   # ilk ateş tam cooldown sonra (0 olursa 1. tickte tetiklenirdi)

    def tick(self, dt: float) -> bool:
        """True dönerse skill tetiklendi."""
        self._timer -= dt
        if self._timer <= 0:
            self._timer = self.cooldown
            return True
        return False


# ─────────────────────────────────────────────
# BOSS TANIMI
# ─────────────────────────────────────────────
@dataclass
class BossConfig:
    name:              str
    act:               str
    hp:                float
    phase_threshold:   float      # HP oranı (örn. 0.5 = %50'de P2)
    dps_p1:            float
    dps_p2:            float
    p1_skills:         List[BossSkill]
    p2_skills:         List[BossSkill]   # P2'de kullanılan skill seti (P1 yerine)
    transition_dur:    float = 1.5
    notes:             str   = ""


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


# ─────────────────────────────────────────────
# CLASS CORE TANIMI
# ─────────────────────────────────────────────
@dataclass
class ClassConfig:
    name: str

    lmb_damage:           float
    lmb_attacks_per_sec:  float
    lmb_resource_per_hit: float
    lmb_aoe_targets:      int   = 1

    resource_max:              float = 100.0
    resource_regen_per_sec:    float = 0.0
    resource_drain_per_sec:    float = 0.0
    resource_per_dmg_taken:    float = 0.0   # Ravager Fury

    burst_threshold:   float = 100.0
    burst_damage:      float = 0.0
    burst_cost:        float = 0.0
    burst_cooldown:    float = 0.0
    burst_aoe_targets: int   = 1

    max_hp:         float = 300.0
    defense_rating: float = 0.10

    move_speed:    float = 1.0
    contact_scale: float = 1.0


# ─────────────────────────────────────────────
# BUILD TANIMI
# ─────────────────────────────────────────────
@dataclass
class Build:
    label:              str
    cfg:                ClassConfig
    skill_avg_dps:      float = 0.0   # skill slot'larının ortalama DPS katkısı
    skill_burst_bonus:  float = 0.0   # burst anında ek hasar
    skill_def_bonus:    float = 0.0   # savunma/mobility bonusu (0–0.25)
    skill_regen_bonus:  float = 0.0   # ekstra kaynak/sn
    note:               str   = ""


# ─────────────────────────────────────────────
# SONUÇ YAPILARI
# ─────────────────────────────────────────────
@dataclass
class RoomResult:
    build_label:       str
    std_ttk:           float
    elite_ttk:         float
    bursts_in_std:     int
    avg_dps:           float
    hp_remaining_pct:  float
    survived:          bool
    dmg_taken:         float
    resource_eff_pct:  float
    bone_revenant_ttk: float = 0.0
    bone_revenant_surv: bool = True
    iron_sentinel_ttk: float = 0.0
    iron_sentinel_surv: bool = True

@dataclass
class BossResult:
    build_label:       str
    boss_name:         str
    ttk:               float
    hp_remaining_pct:  float
    survived:          bool
    phase2_entry:      float    # –1 = hiç girilmedi
    skill_hits:        int      # kaç boss skilli indi
    bursts_fired:      int


# ─────────────────────────────────────────────
# ODA SİMÜLASYONU
# ─────────────────────────────────────────────
def simulate_room(build: Build, enemy_hps: List[float],
                  enemy_dps: float = ENEMY_DPS_MOB,
                  approach_base: float = APPROACH_BASE):
    cfg = build.cfg
    eff_def  = min(0.85, cfg.defense_rating + build.skill_def_bonus)
    eff_regen = cfg.resource_regen_per_sec + build.skill_regen_bonus

    t = 0.0; enemies = list(enemy_hps); alive = sum(1 for h in enemies if h > 0)
    resource = 0.0; burst_cd = 0.0; burst_count = 0
    total_dmg = 0.0; total_taken = 0.0; time_above = 0.0
    player_hp = cfg.max_hp; atk_timer = 0.0; atk_iv = 1.0 / cfg.lmb_attacks_per_sec
    approach_delay = approach_base * cfg.move_speed

    while alive > 0 and t < SIM_CAP and player_hp > 0:
        atk_timer += SIM_TICK
        if atk_timer >= atk_iv:
            atk_timer -= atk_iv
            hits = 0
            for i in range(len(enemies)):
                if enemies[i] > 0 and hits < cfg.lmb_aoe_targets:
                    enemies[i] -= cfg.lmb_damage; total_dmg += cfg.lmb_damage; hits += 1
                    if enemies[i] <= 0: alive -= 1
            if hits > 0:
                resource = min(cfg.resource_max, resource + cfg.lmb_resource_per_hit)

        if build.skill_avg_dps > 0 and alive > 0:
            sd = build.skill_avg_dps * SIM_TICK
            for i in range(len(enemies)):
                if enemies[i] > 0:
                    enemies[i] -= sd; total_dmg += sd
                    if enemies[i] <= 0: alive -= 1
                    break

        resource = min(cfg.resource_max, resource + eff_regen * SIM_TICK)

        if burst_cd <= 0 and resource >= cfg.burst_threshold:
            hits = 0
            for i in range(len(enemies)):
                if enemies[i] > 0 and hits < cfg.burst_aoe_targets:
                    bd = cfg.burst_damage + build.skill_burst_bonus
                    enemies[i] -= bd; total_dmg += bd; hits += 1
                    if enemies[i] <= 0: alive -= 1
            resource = max(0.0, resource - cfg.burst_cost)
            burst_cd = cfg.burst_cooldown; burst_count += 1

        resource = max(0.0, resource - cfg.resource_drain_per_sec * SIM_TICK)
        if resource >= cfg.burst_threshold: time_above += SIM_TICK

        if t >= approach_delay and alive > 0:
            max_c = max(1, round(alive * (1.0 / cfg.move_speed) * 0.70 * cfg.contact_scale))
            contact = min(alive, max_c)
            raw = contact * enemy_dps * SIM_TICK
            eff = raw * (1.0 - eff_def)
            player_hp -= eff; total_taken += eff
            if cfg.resource_per_dmg_taken > 0:
                resource = min(cfg.resource_max, resource + raw * cfg.resource_per_dmg_taken)

        burst_cd = max(0.0, burst_cd - SIM_TICK); t += SIM_TICK

    return (
        t if alive == 0 else SIM_CAP,
        burst_count,
        total_dmg / max(t, SIM_TICK),
        time_above / max(t, SIM_TICK),
        max(0.0, player_hp) / cfg.max_hp * 100.0,
        total_taken,
        player_hp > 0,
    )


# ─────────────────────────────────────────────
# BOSS SİMÜLASYONU
# ─────────────────────────────────────────────
def simulate_boss(build: Build, boss: BossConfig):
    cfg = build.cfg
    eff_def  = min(0.85, cfg.defense_rating + build.skill_def_bonus)
    eff_regen = cfg.resource_regen_per_sec + build.skill_regen_bonus

    # Boss skill timer sıfırla
    for sk in boss.p1_skills + boss.p2_skills:
        sk.reset()

    t = 0.0
    boss_hp = boss.hp
    resource = 0.0; burst_cd = 0.0; burst_count = 0
    total_dmg = 0.0; total_taken = 0.0; time_above = 0.0
    player_hp = cfg.max_hp; atk_timer = 0.0; atk_iv = 1.0 / cfg.lmb_attacks_per_sec
    approach_delay = BOSS_APPROACH * cfg.move_speed

    in_p2 = False; in_transition = False; transition_timer = 0.0
    p2_entry = -1.0; current_boss_dps = boss.dps_p1
    active_skills = boss.p1_skills

    # Spawn sistemi için sahte düşman listesi (boss + possible spawns)
    spawns: List[float] = []   # boss dışı ek düşmanlar

    skill_hits = 0

    while boss_hp > 0 and t < SIM_CAP and player_hp > 0:

        # ── Phase geçiş kontrolü ────────────────────────
        if not in_p2 and boss_hp <= boss.hp * boss.phase_threshold:
            in_p2 = True; p2_entry = t
            in_transition = True; transition_timer = boss.transition_dur
            active_skills = boss.p2_skills

        if in_transition:
            transition_timer -= SIM_TICK
            if transition_timer <= 0:
                in_transition = False
                current_boss_dps = boss.dps_p2
            t += SIM_TICK
            continue

        # ── Oyuncu LMB → boss'a hasar ──────────────────
        atk_timer += SIM_TICK
        if atk_timer >= atk_iv:
            atk_timer -= atk_iv
            boss_hp = max(0.0, boss_hp - cfg.lmb_damage)
            total_dmg += cfg.lmb_damage
            resource = min(cfg.resource_max, resource + cfg.lmb_resource_per_hit)

        # ── Skill DPS → boss'a ──────────────────────────
        if build.skill_avg_dps > 0:
            sd = build.skill_avg_dps * SIM_TICK
            boss_hp = max(0.0, boss_hp - sd); total_dmg += sd

        resource = min(cfg.resource_max, resource + eff_regen * SIM_TICK)

        # ── Burst ───────────────────────────────────────
        if burst_cd <= 0 and resource >= cfg.burst_threshold:
            bd = cfg.burst_damage + build.skill_burst_bonus
            boss_hp = max(0.0, boss_hp - bd); total_dmg += bd
            resource = max(0.0, resource - cfg.burst_cost)
            burst_cd = cfg.burst_cooldown; burst_count += 1

        resource = max(0.0, resource - cfg.resource_drain_per_sec * SIM_TICK)
        if resource >= cfg.burst_threshold: time_above += SIM_TICK

        # ── Boss gelen hasar (temas DPS) ────────────────
        if t >= approach_delay:
            # Boss + mevcut spawn'lar toplam temas
            total_contact_dps = current_boss_dps
            for s_hp in spawns:
                if s_hp > 0:
                    total_contact_dps += ENEMY_DPS_MOB * 0.6  # spawn zayıf mob

            raw = total_contact_dps * SIM_TICK
            eff = raw * (1.0 - eff_def)
            player_hp -= eff; total_taken += eff

            if cfg.resource_per_dmg_taken > 0:
                resource = min(cfg.resource_max, resource + raw * cfg.resource_per_dmg_taken)

        # ── Boss skill'leri ─────────────────────────────
        for sk in active_skills:
            if sk.tick(SIM_TICK):
                if sk.skill_type == "BURST":
                    # Telegraphed saldırı: hız = daha iyi dodge
                    dodge = min(0.82, sk.dodge_factor * cfg.move_speed * DODGE_BURST_BASE)
                    dmg = sk.damage * (1.0 - dodge) * (1.0 - eff_def)
                    player_hp -= dmg; total_taken += dmg; skill_hits += 1

                elif sk.skill_type == "ARENA":
                    # Alan hasarı: çok az dodge
                    dodge = min(0.45, sk.dodge_factor * cfg.move_speed * DODGE_ARENA_BASE)
                    dmg = sk.damage * (1.0 - dodge) * (1.0 - eff_def)
                    player_hp -= dmg; total_taken += dmg; skill_hits += 1

                elif sk.skill_type == "SPAWN":
                    for _ in range(sk.spawn_count):
                        spawns.append(sk.spawn_hp)

                elif sk.skill_type == "DRAIN":
                    resource = max(0.0, resource - sk.drain_amount)

        # Spawn'ları boss AoE ile temizle (burst hit spawn'ları da vurur)
        if burst_count > 0:
            spawns = []  # simplifikasyon: burst spawn'ları temizler

        burst_cd = max(0.0, burst_cd - SIM_TICK); t += SIM_TICK

    return (
        t if boss_hp <= 0 else SIM_CAP,
        max(0.0, player_hp) / cfg.max_hp * 100.0,
        player_hp > 0,
        p2_entry,
        skill_hits,
        burst_count,
    )


# ─────────────────────────────────────────────
# TÜM BUILD'LERİ ÇALIŞTIR
# ─────────────────────────────────────────────
def run_all(builds: List[Build], bosses: List[BossConfig]):
    room_results: List[RoomResult] = []
    boss_results: List[BossResult] = []

    for b in builds:
        e_std   = [ENEMY_HP_STD]   * ENEMIES_STD
        e_elite = [ENEMY_HP_ELITE] * ENEMIES_ELITE

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

        for boss in bosses:
            ttk_b, hp_b, surv_b, p2, hits, b_cnt = simulate_boss(b, boss)
            boss_results.append(BossResult(
                build_label      = b.label,
                boss_name        = boss.name,
                ttk              = round(ttk_b, 1),
                hp_remaining_pct = round(hp_b, 1),
                survived         = surv_b,
                phase2_entry     = round(p2, 1) if p2 >= 0 else -1,
                skill_hits       = hits,
                bursts_fired     = b_cnt,
            ))

    return room_results, boss_results


# ─────────────────────────────────────────────
# RAPOR: ODA
# ─────────────────────────────────────────────
def print_room_report(results: List[RoomResult]):
    W = 155
    print("\n" + "=" * W)
    print(f"  RIMA BALANCE SIM v4.0  —  ODA RAPORU  |  Mob DPS={ENEMY_DPS_MOB}  |  Oda: {ENEMIES_STD}×{ENEMY_HP_STD}HP std, {ENEMIES_ELITE}×{ENEMY_HP_ELITE}HP elite")
    print("=" * W)
    hdr = f"{'Build':<26} {'StdTTK':>8} {'EliteTTK':>9} {'Bursts':>7} {'DPS':>7} {'HP%':>7} {'Taken':>7} {'Res%':>6} {'A1-2TTK':>8} {'A3TTK':>7}  Flags"
    print(hdr); print("-" * W)

    from itertools import groupby
    key = lambda r: r.build_label.split(" — ")[0]
    for cls, grp in groupby(sorted(results, key=lambda r: (key(r), r.std_ttk)), key=key):
        for r in grp:
            flags = []
            if not r.survived:                              flags.append("!! DIED")
            elif r.hp_remaining_pct < 20:                  flags.append("!! LOW HP")
            if r.std_ttk >= SIM_CAP * 0.7:                 flags.append("!! TOO SLOW")
            if r.bursts_in_std == 0:                        flags.append("!! NO BURST")
            elif r.bursts_in_std > 12:                      flags.append("BURST SPAM")
            if r.hp_remaining_pct > 70 and r.std_ttk < 9:  flags.append("★ DOMINANT")
            br_flag = "" if r.bone_revenant_surv else "✗BR"
            is_flag = "" if r.iron_sentinel_surv else "✗IS"
            print(
                f"{r.build_label:<26}{r.std_ttk:>7.1f}s{r.elite_ttk:>8.1f}s"
                f"{r.bursts_in_std:>8}{r.avg_dps:>7.1f}{r.hp_remaining_pct:>6.1f}%"
                f"{r.dmg_taken:>7.0f}{r.resource_eff_pct:>5.1f}%"
                f"{r.bone_revenant_ttk:>7.1f}s{r.iron_sentinel_ttk:>6.1f}s"
                f"  {br_flag}{is_flag}{'  ' if br_flag or is_flag else ''}{'  '.join(flags)}"
            )
        print()
    print("=" * W)


# ─────────────────────────────────────────────
# RAPOR: BOSS MATRİSİ
# ─────────────────────────────────────────────
def print_boss_report(boss_results: List[BossResult], bosses: List[BossConfig]):
    boss_names = [b.name for b in bosses]
    W = 30 + len(boss_names) * 22

    print("\n" + "=" * W)
    print(f"  BOSS MATRİSİ  —  Her build × {len(boss_names)} akt boss")
    print("=" * W)

    # Başlık
    hdr = f"{'Build':<28}"
    for bn in boss_names:
        hdr += f" {bn[:18]:>18} "
    print(hdr); print("-" * W)

    # Build'e göre grupla
    build_names = list(dict.fromkeys(r.build_label for r in boss_results))

    for bl in build_names:
        row = f"{bl:<28}"
        for bn in boss_names:
            match = next((r for r in boss_results if r.build_label == bl and r.boss_name == bn), None)
            if match is None:
                row += f" {'N/A':>18} "
            else:
                status = "✓" if match.survived else "✗"
                hp_str = f"{match.hp_remaining_pct:.0f}%HP" if match.survived else "DIED"
                cell = f"{status} {match.ttk:.1f}s {hp_str}"
                row += f" {cell:>18} "
        print(row)

        # Sınıf değişiminde boş satır
        if bl == build_names[-1] or build_names[build_names.index(bl)+1].split(" — ")[0] != bl.split(" — ")[0]:
            print()

    print("=" * W)
    print()
    print("BOSS DETAY")
    for boss in bosses:
        print(f"\n  [{boss.act}] {boss.name}  |  HP={boss.hp}  P1 DPS={boss.dps_p1} → P2 DPS={boss.dps_p2}")
        print(f"  P1 skills: " + " | ".join(f"{sk.name}({sk.skill_type} {sk.damage:.0f}dmg @{sk.cooldown:.0f}s)" for sk in boss.p1_skills))
        print(f"  P2 skills: " + " | ".join(f"{sk.name}({sk.skill_type} {sk.damage:.0f}dmg @{sk.cooldown:.0f}s)" for sk in boss.p2_skills))
        if boss.notes:
            print(f"  Not: {boss.notes}")

    print()
    print("MODEL  BURST: dodge = dodge_factor × hız × 0.65 (hızlı class → daha iyi kaçar)")
    print("       ARENA: dodge = dodge_factor × hız × 0.22 (alan hasarı, az kaçınılır)")
    print("       SPAWN: ek mob çıkar → temas DPS artar, burst temizler")
    print("       DRAIN: resource sıfırlar → burst gecikir")
    print()


# ─────────────────────────────────────────────
# CLASS CONFIG'LER  (v3 balance)
# ─────────────────────────────────────────────

_WARBLADE = ClassConfig(          # balance fix: threshold 50, burst_cd 2.5
    name="Warblade",
    lmb_damage=35, lmb_attacks_per_sec=1.5, lmb_resource_per_hit=10,
    lmb_aoe_targets=2,
    resource_max=100,
    burst_threshold=50, burst_damage=150, burst_aoe_targets=3,
    burst_cost=30, burst_cooldown=2.5,
    max_hp=400, defense_rating=0.20,
    move_speed=0.90, contact_scale=1.0,
)

_ELEMENTALIST = ClassConfig(      # balance fix v4: HP 275→320, def 0.08→0.13
    name="Elementalist",
    lmb_damage=20, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=3,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=8,
    burst_threshold=100, burst_damage=400, burst_aoe_targets=5,
    burst_cost=100, burst_cooldown=20,
    max_hp=320, defense_rating=0.13,
    move_speed=1.10, contact_scale=1.0,
)

_SHADOWBLADE = ClassConfig(
    name="Shadowblade",
    lmb_damage=25, lmb_attacks_per_sec=2.0, lmb_resource_per_hit=5,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=15,
    burst_threshold=100, burst_damage=300, burst_aoe_targets=1,
    burst_cost=100, burst_cooldown=15,
    max_hp=280, defense_rating=0.10,
    move_speed=1.45, contact_scale=0.55,
)

_RANGER = ClassConfig(            # balance fix v4: HP 320→380, def 0.12→0.18
    name="Ranger",
    lmb_damage=18, lmb_attacks_per_sec=2.5, lmb_resource_per_hit=4,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=10,
    burst_threshold=75, burst_damage=250, burst_aoe_targets=4,
    burst_cost=0, burst_cooldown=30,
    max_hp=380, defense_rating=0.18,
    move_speed=1.20, contact_scale=0.35,
)

_RAVAGER = ClassConfig(
    name="Ravager",
    lmb_damage=45, lmb_attacks_per_sec=1.2, lmb_resource_per_hit=0,
    lmb_aoe_targets=3,
    resource_max=100, resource_per_dmg_taken=0.18,
    burst_threshold=80, burst_damage=500, burst_aoe_targets=5,
    burst_cost=0, burst_cooldown=20,
    max_hp=350, defense_rating=0.05,
    move_speed=0.80, contact_scale=1.0,
)

_RONIN = ClassConfig(
    name="Ronin",
    lmb_damage=30, lmb_attacks_per_sec=1.8, lmb_resource_per_hit=5,
    lmb_aoe_targets=1,
    resource_max=100, resource_regen_per_sec=20,
    burst_threshold=80, burst_damage=250, burst_aoe_targets=2,
    burst_cost=50, burst_cooldown=3.0,
    max_hp=280, defense_rating=0.15,
    move_speed=1.30, contact_scale=0.75,
)

_GUNSLINGER = ClassConfig(
    name="Gunslinger",
    lmb_damage=22, lmb_attacks_per_sec=3.0, lmb_resource_per_hit=6,
    lmb_aoe_targets=2,
    resource_max=100,
    burst_threshold=100, burst_damage=200, burst_aoe_targets=4,
    burst_cost=0, burst_cooldown=5.0,
    max_hp=280, defense_rating=0.10,
    move_speed=1.20, contact_scale=0.65,
)

_BRAWLER = ClassConfig(
    name="Brawler",
    lmb_damage=28, lmb_attacks_per_sec=2.0, lmb_resource_per_hit=1,
    lmb_aoe_targets=1,
    resource_max=5,
    burst_threshold=5, burst_damage=180, burst_aoe_targets=2,
    burst_cost=5, burst_cooldown=0.5,
    max_hp=350, defense_rating=0.15,
    move_speed=1.10, contact_scale=0.85,
)

_SUMMONER = ClassConfig(
    name="Summoner",
    lmb_damage=15, lmb_attacks_per_sec=1.0, lmb_resource_per_hit=0,
    lmb_aoe_targets=1,
    resource_max=4, resource_regen_per_sec=0.125,
    burst_threshold=4, burst_damage=350, burst_aoe_targets=3,
    burst_cost=4, burst_cooldown=20,
    max_hp=340, defense_rating=0.10,
    move_speed=0.85, contact_scale=1.0,
)

_HEXER = ClassConfig(             # balance fix: HP 270→350, def 0.05→0.10, lmb_dmg 12→15
    name="Hexer",
    lmb_damage=15, lmb_attacks_per_sec=3.0, lmb_resource_per_hit=1,
    lmb_aoe_targets=1,
    resource_max=10, resource_drain_per_sec=1.5,
    burst_threshold=7, burst_damage=320, burst_aoe_targets=1,
    burst_cost=7, burst_cooldown=0,
    max_hp=350, defense_rating=0.10,
    move_speed=1.10, contact_scale=0.90,
)


# ─────────────────────────────────────────────
# 4 AKT BOSS CONFIG'LERİ
# Biome: Crimson Crypt / Rift Sanctum / Iron Vault / Abyssal Court
# ─────────────────────────────────────────────

BOSSES: List[BossConfig] = [

    # Hedef: Act 1 ~80% build hayatta, Act 4 ~25% hayatta
    # Kalibrasyon: contact DPS düşük (pattern-based boss), skill hasarı asıl tehdit

    BossConfig(
        name = "Flesh Warden",
        act  = "Act 1 — Crimson Crypt",
        hp   = 2000,
        phase_threshold = 0.50,
        dps_p1 = 7, dps_p2 = 10,          # düşük sustained — asıl tehdit skill'ler
        p1_skills = [
            # BURST dodge_factor: 1.0-speed class için gerçek kaçış şansı
            BossSkill("Bone Slam",    cooldown=6.0,  damage=85,  skill_type="BURST", dodge_factor=0.65),
            BossSkill("Blood Pool",   cooldown=14.0, damage=65,  skill_type="ARENA", dodge_factor=0.35),
        ],
        p2_skills = [
            BossSkill("Bone Slam+",   cooldown=4.5,  damage=100, skill_type="BURST", dodge_factor=0.60),
            BossSkill("Blood Frenzy", cooldown=10.0, damage=80,  skill_type="ARENA", dodge_factor=0.28),
            BossSkill("Risen",        cooldown=20.0, damage=0,   skill_type="SPAWN",
                      dodge_factor=0, spawn_count=2, spawn_hp=100),
        ],
        notes = "Ağır melee, bleed tank. P2: zombie spawn + hızlı slam.",
    ),

    BossConfig(
        name = "Void Weaver",
        act  = "Act 2 — Rift Sanctum",
        hp   = 2400,
        phase_threshold = 0.45,
        dps_p1 = 6, dps_p2 = 9,
        p1_skills = [
            BossSkill("Void Bolt",    cooldown=4.0,  damage=75,  skill_type="BURST", dodge_factor=0.78),
            BossSkill("Rift Tear",    cooldown=16.0, damage=95,  skill_type="ARENA", dodge_factor=0.38),
        ],
        p2_skills = [
            BossSkill("Void Bolt+",   cooldown=3.0,  damage=90,  skill_type="BURST", dodge_factor=0.74),
            BossSkill("Void Collapse",cooldown=13.0, damage=95,  skill_type="ARENA", dodge_factor=0.22),
            BossSkill("Rift Walker",  cooldown=22.0, damage=0,   skill_type="SPAWN",
                      dodge_factor=0, spawn_count=2, spawn_hp=140),
            BossSkill("Void Drain",   cooldown=35.0, damage=0,   skill_type="DRAIN",
                      dodge_factor=0, drain_amount=40),
        ],
        notes = "Ranged void caster. P2: resource drain + büyük ARENA.",
    ),

    BossConfig(
        name = "Iron Colossus",
        act  = "Act 3 — Iron Vault",
        hp   = 2800,
        phase_threshold = 0.40,
        dps_p1 = 8, dps_p2 = 12,
        p1_skills = [
            BossSkill("Cannon Blast", cooldown=6.0,  damage=90,  skill_type="BURST", dodge_factor=0.68),
            BossSkill("Tremor Stomp", cooldown=11.0, damage=80,  skill_type="ARENA", dodge_factor=0.28),
        ],
        p2_skills = [
            BossSkill("Cannon Blast+",cooldown=4.0,  damage=110, skill_type="BURST", dodge_factor=0.62),
            BossSkill("Molten Core",  cooldown=8.0,  damage=80,  skill_type="ARENA", dodge_factor=0.18),
            BossSkill("Drone Swarm",  cooldown=15.0, damage=0,   skill_type="SPAWN",
                      dodge_factor=0, spawn_count=3, spawn_hp=120),
        ],
        notes = "Tank mech. P2: yüksek DPS + drone baskısı.",
    ),

    BossConfig(
        name = "Abyssal Judge",
        act  = "Act 4 — Abyssal Court",
        hp   = 3200,
        phase_threshold = 0.50,
        dps_p1 = 8, dps_p2 = 12,
        transition_dur  = 2.0,
        p1_skills = [
            BossSkill("Verdict",       cooldown=7.0,  damage=85,  skill_type="BURST", dodge_factor=0.55),
            BossSkill("Decree",        cooldown=16.0, damage=100, skill_type="ARENA", dodge_factor=0.30),
        ],
        p2_skills = [
            BossSkill("Final Verdict", cooldown=5.0,  damage=120, skill_type="BURST", dodge_factor=0.46),
            BossSkill("Judgment Day",  cooldown=12.0, damage=110, skill_type="ARENA", dodge_factor=0.16),
            BossSkill("Shade Spawn",   cooldown=18.0, damage=0,   skill_type="SPAWN",
                      dodge_factor=0, spawn_count=3, spawn_hp=130),
            BossSkill("Soul Drain",    cooldown=24.0, damage=0,   skill_type="DRAIN",
                      dodge_factor=0, drain_amount=55),
        ],
        notes = "Final boss. 3200 HP, 2s geçiş, P2 çok agresif. Soul Drain burst ritmine zarar verir.",
    ),
]


# ─────────────────────────────────────────────
# BUILD'LER
# ─────────────────────────────────────────────

BUILDS: List[Build] = [

    # WARBLADE
    Build("Warblade — Execution", _WARBLADE,
          skill_avg_dps=40, skill_burst_bonus=110,
          note="Iron Charge + Crippling Blow + Iron Crush + Death Blow"),
    Build("Warblade — Control", _WARBLADE,
          skill_avg_dps=28, skill_def_bonus=0.05,
          note="Gravity Cleave + Sunder Mark + War Stomp + Blade Rush"),
    Build("Warblade — Last Stand", _WARBLADE,
          skill_avg_dps=18, skill_def_bonus=0.18,
          note="Iron Counter + Ironclad Momentum + Battle Surge + Deep Wound"),

    # ELEMENTALIST
    Build("Elementalist — Fire", _ELEMENTALIST,
          skill_avg_dps=50, skill_burst_bonus=80,
          note="Fireball + Living Bomb + Combustion + Meteor"),
    Build("Elementalist — Frost", _ELEMENTALIST,
          skill_avg_dps=38, skill_def_bonus=0.08,
          note="Glacial Spike + Blizzard + Frozen Orb + Meteor"),
    Build("Elementalist — Radiant", _ELEMENTALIST,
          skill_avg_dps=42, skill_burst_bonus=60, skill_regen_bonus=3,
          note="Prism Lance + Luminary Surge + Halo Fracture + Sunshard"),

    # SHADOWBLADE
    Build("Shadowblade — Scar", _SHADOWBLADE,
          skill_avg_dps=32, note="Rift Fang + Maw Collapse + Split Wake + Hollow Spiral"),
    Build("Shadowblade — Predator", _SHADOWBLADE,
          skill_avg_dps=40, skill_def_bonus=0.10,
          note="Veil Hunt + Rift Hook + Predator's Fold + Maw Collapse"),

    # RANGER
    Build("Ranger — Sniper", _RANGER,
          skill_avg_dps=38, skill_burst_bonus=50,
          note="Power Shot + Cobra Shot + Volley + Rain of Arrows"),
    Build("Ranger — Trap", _RANGER,
          skill_avg_dps=28, skill_def_bonus=0.08,
          note="Bear Trap + Explosive Trap + Concuss Shot + Volley"),

    # RAVAGER
    Build("Ravager — Berserk", _RAVAGER,
          skill_avg_dps=45, note="Rend + War Cry + Berserker's Seal + Carnage Rush"),
    Build("Ravager — Blood", _RAVAGER,
          skill_avg_dps=18, skill_def_bonus=0.08, skill_regen_bonus=5,
          note="Bloodlust + Crimson Tide + Carnage + Rend"),

    # RONIN
    Build("Ronin — Swift", _RONIN,
          skill_avg_dps=48, note="Quickdraw + Iaijutsu + Phantom Step + Afterimage"),
    Build("Ronin — Counter", _RONIN,
          skill_avg_dps=28, skill_def_bonus=0.12,
          note="Parry Strike + Counter Slash + Void Step + Iaijutsu"),

    # GUNSLINGER
    Build("Gunslinger — DPS", _GUNSLINGER,
          skill_avg_dps=48, note="Fan the Hammer + Barrage + Slug Round + Crossfire"),
    Build("Gunslinger — Kite", _GUNSLINGER,
          skill_avg_dps=32, skill_def_bonus=0.08,
          note="Smoke Grenade + Slide + Barrage + Fan the Hammer"),

    # BRAWLER
    Build("Brawler — Charged", _BRAWLER,
          skill_avg_dps=42, skill_burst_bonus=55,
          note="Power Jab + Haymaker + Overdrive + Charged State"),
    Build("Brawler — Defense", _BRAWLER,
          skill_avg_dps=22, skill_def_bonus=0.15,
          note="Guard + Parry + Clinch + Power Jab"),

    # SUMMONER  (!! minyon DPS proxy)
    Build("Summoner — Army", _SUMMONER,
          skill_avg_dps=75,  # 3 minyon × ~25 DPS tahmin (v4 fix)
          note="!! Minyon DPS proxy — gerçek econ modellenemez"),
    Build("Summoner — Support", _SUMMONER,
          skill_avg_dps=55, skill_def_bonus=0.10,
          note="Bone Shield + Soul Link + minyon proxy"),

    # HEXER
    Build("Hexer — Phase", _HEXER,
          skill_avg_dps=28,
          note="Doom Bolt + Wither + Hex Nova + Curse Grasp"),
    Build("Hexer — Debuff", _HEXER,
          skill_avg_dps=22, skill_def_bonus=0.07, skill_regen_bonus=1,
          note="Wither + Slow Hex + Entropy + Doom Bolt"),
]


# ─────────────────────────────────────────────
# ÇALIŞTIR
# ─────────────────────────────────────────────
if __name__ == "__main__":
    room_res, boss_res = run_all(BUILDS, BOSSES)
    print_room_report(room_res)
    print_boss_report(boss_res, BOSSES)
