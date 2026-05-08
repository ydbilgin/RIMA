# RIMA Skill Tree -- 10 Classes Canonical Draft
Version: v0.1 (2026-05-06) -- numbers are design constants, not implementation contracts.

Cross-class proc bus: Hexer is the central hub. 10 cross-procs total (1 per class).
See CROSS_CLASS_PROC_SYSTEM.md for the full proc registry.

---

### Warblade
Identity: Chain berserker. 3-hit LMB loop feeds every skill.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Wrathfall | Signature (RMB) | 6s | Overhead cleave; consumes current chain step, deals (80% + 40% per chain stack) AoE 3m arc; resets chain | Pays out chain damage banked by LMB |
| 2 | Bloodgale | Active (Q) | 5s | 4 hits/1.5s spinning sweep, each hit +1 chain stack (cap 5); ignore chain reset on dodge for 2s | Refills chain mid-fight |
| 3 | Gorestride | Active (E) | 4s | Dash 5m forward; next 3 LMB hits gain 25% lifesteal | Mobility + sustain feeder |
| 4 | Skullsplitter | Active (R) | 9s | 220% single strike; execute below 30% HP; Hexer 3-stack curse raises execute threshold to 50% | Cross-class: Hexer |
| 5 | Crimson Apotheosis | Ultimate (F) | 35s | 8s: chain cap removed, each LMB refunds 0.5s on actives, +40% attack speed | Capstone |
| 6 | Iron Hunger | Passive | -- | Per chain stack: +6% damage, +3% damage taken | Risk/reward |
| 7 | Sunder Vow | Passive | -- | Heavy hit (chain step 3) applies 4s armor shred (-30% enemy armor) | Team damage amp |
| 8 | Last Breath Roar | Passive | -- | Below 30% HP: chain stacks generate 2x, +20% lifesteal | Comeback |

---

### Shadowblade
Identity: Echo assassin. Stack echoes (max 2), detonate on RMB.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Echo Rupture | Signature (RMB) | 5s | Detonates all active Echoes 3m radius around target, 140% per echo; refunds 1s on Q if crit | Core payout |
| 2 | Ashveil Step | Active (Q) | 6s | Vanish 1.2s; exit = guaranteed crit next LMB, spawns 2 echoes instead of 1 | Echo multiplier |
| 3 | Nightseam Throw | Active (E) | 4s | Marks target; LMB on marked = 100% power echoes for 4s | Burst setup |
| 4 | Hollow Bloom | Active (R) | 8s | Shadow seed at location; if enemy dies within 4m in 5s, spawns 3 echoes | Cross-class: Hexer kill-proc |
| 5 | Thousand Whisper Veil | Ultimate (F) | 40s | 6s: every LMB spawns 2 echoes (no cap), echoes auto-detonate every 1s at 80% | Capstone |
| 6 | Borrowed Death | Passive | -- | Echo detonations apply 2s Marked: +15% damage from all sources | Party-wide amp |
| 7 | Doublestep Pact | Passive | -- | Dodge refunds an echo charge; next LMB within 1s spawns echo ignoring CD | Mobility rewards aggression |
| 8 | Curseblade Resonance | Passive | -- | Echoes deal +50% to cursed enemies; detonating on 3-stack curse also triggers Hexer explosion proc | Cross-class: Hexer |

---

### Ironclad
Identity: Parry tank. RMB = 0.4s parry window -> Fortify stacks (max 5).

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Bulwark Vow | Signature (RMB) | 5s | 0.4s parry window; successful parry = 1 Fortify stack (max 5, 8s), reflects 60% damage; perfect parry stuns 1s | Core resource |
| 2 | Aegis Slam | Active (Q) | 6s | Shield bash 3m, damage 80% + 40% per Fortify stack; consumes all stacks | Fortify payout |
| 3 | Anchorhold | Active (E) | 7s | 2s stance: -50% dmg taken, immune to displacement, parry window 0.6s | Defensive setup |
| 4 | Ground Knell | Active (R) | 9s | 4m AoE slam, 100%; Hexer-cursed targets pulled to center | Cross-class: Hexer |
| 5 | Mountain's Final Word | Ultimate (F) | 30s | 6s unstoppable: every hit taken = Fortify stack (no cap), damage dealt +8% per stack | Capstone |
| 6 | Reforged Oath | Passive | -- | At 5 Fortify stacks: one-time block absorbs next lethal hit (3min CD) | Survival floor |
| 7 | Sundered Bell | Passive | -- | Reflected damage from Fortify applies 2s Stagger: -30% enemy attack speed | Layered defense |
| 8 | Vow of the Wall | Passive | -- | Allies within 4m gain 50% of your Fortify DR | Cross-class: shields Summoner minions, Riftstalker traps |

---

### Arcanist
Identity: Mid-range elemental burst. Cycles Fire/Frost/Lightning each cast.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Prism Lance | Signature (RMB) | 6s | Pierce line, payload by element: Fire=burn DoT 80%/3s; Frost=slow 40% 3s+70%; Lightning=chain 3 targets 60% each | Element dictates use |
| 2 | Ember Veil | Active (Q) | 5s | 3m AoE: Fire=ignite zone 100%/s/3s; Frost=freeze 1.5s; Lightning=shock+25% stun chance | Zone control |
| 3 | Glimmer Step | Active (E) | 4s | Blink 4m; next spell forced to previous element (repeat element) | Combo enabler |
| 4 | Hexbind Sigil | Active (R) | 8s | Plants sigil; enemies inside +30% spell damage; Hexer curse proc inside extends sigil 3s + pulses 100% AoE | Cross-class: Hexer |
| 5 | Threefold Spiral | Ultimate (F) | 35s | 5s: cast all 3 elements simultaneously (Fire ring + Frost nova + Lightning storm), 120% each, cycle every 1s | Capstone |
| 6 | Cycle's Edge | Passive | -- | Casting same element twice in a row (via Glimmer Step) = guaranteed crit + 50% bonus | Rewards combo |
| 7 | Embergate | Passive | -- | Each element leaves 2s residue zone; allied projectiles through zone inherit element | Party utility |
| 8 | Stillness Between | Passive | -- | Standing still 1s: next spell CD -40% and element of choice (hold key) | Caster identity |

---

### Elementalist
Identity: Long-range elemental specialist. Commits to elements for cross-element reactions.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Stormveil Mark | Signature (RMB) | 7s | Marks target with current element 5s; LMB consumes mark for elemental detonation (130% + reaction bonus) | Sets up reactions |
| 2 | Cinder Pillar | Active (Q) | 8s | Long-range 1s windup, 180% 2m radius: Fire=ignite 4s; Frost=freeze 2s; Lightning=stun 1s | Heavy artillery |
| 3 | Tideturn | Active (E) | 6s | Instantly switch element; switching to element already on a marked enemy triggers Reaction immediately | Reaction trigger |
| 4 | Hailspine Volley | Active (R) | 10s | Channel 3s, fires 8 elemental shards inheriting current element | Sustained DPS |
| 5 | Worldfracture | Ultimate (F) | 45s | 8s elemental storm cycling all 3; every reaction triggered inside is doubled in radius | Capstone |
| 6 | Reactive Lattice | Passive | -- | Reactions: Fire+Frost=200% steam burst; Frost+Lightning=300% Shatter on frozen; Fire+Lightning=Overload -50% resist 4s | Core reaction matrix |
| 7 | Skyvein Conduit | Passive | -- | LMB at >8m: +35% damage, refunds 1s on Q per hit | Rewards positioning |
| 8 | Cursebound Element | Passive | -- | Hexer 3-stack curse on target = guaranteed Shatter on next mark consume | Cross-class: Hexer |

---

### Hexer
Identity: Curse-stack engine and cross-class proc bus. 3 stacks = explosion + 10 cross-procs fire.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Witherbrand | Signature (RMB) | 5s | Curse-bolt, 1 stack (3m AoE around impact); 3 stacks = proc explosion 140% + all cross-class procs | Core curse applicator |
| 2 | Bonecage Pulse | Active (Q) | 6s | 4m radial pulse, 1 stack to all hit; cursed enemies rooted 1.5s | AoE stack spread |
| 3 | Effigy Bind | Active (E) | 8s | Totem 5s, 4m range: +1 stack/1.5s to enemies; absorbs 30% damage dealt to allies | Stationary engine |
| 4 | Soulrend Hex | Active (R) | 7s | Instantly applies 3 stacks (triggers proc) to single target; next curse on this target +50% CD for 8s | Burst tool |
| 5 | The Drowning Name | Ultimate (F) | 40s | 8s: all enemies on screen +1 stack/2s; each 3-stack proc also stacks nearest 2 enemies | Capstone: cascade |
| 6 | Withering Bloom | Passive | -- | Cursed enemy: +12% damage per stack (max +36%) from all sources | Party-wide amp |
| 7 | Echoed Affliction | Passive | -- | 3-stack proc broadcasts: Shadowblade free echo, Warblade +1 chain, Ironclad +1 Fortify, Riftstalker rift charge | Cross-class bus |
| 8 | Long Memory | Passive | -- | Curse stacks persist 8s after death; transfer to nearest enemy within 5m on kill | Boss-room sustain |

---

### Summoner
Identity: Soul-economy commander. Kill=+1 Soul. Souls summon minions (cap 4). Minions persist across rooms.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Bind the Forsaken | Signature (RMB) | 6s | Spend 1 Soul, summon minion at cursor (Wraith=melee 80%, Hollow=ranged 60%, Husk=tank); cap 4 | Core soul-spend |
| 2 | Conduit Lash | Active (Q) | 5s | Whip-line 70%; all minions focus hit target 4s (+30% minion damage) | Command/focus |
| 3 | Soulreap Pulse | Active (E) | 4s | 5m AoE; enemies hit drop Soul Mote on death for 6s (+1 bonus Soul per kill) | Economy accelerant |
| 4 | Marrow Tether | Active (R) | 9s | Sacrifice 1 minion: heal self 30% HP + other minions 50%; refund 1 Soul | Resource conversion |
| 5 | Coronation of Bone | Ultimate (F) | 45s | Summons Bone Sovereign (12s, 250% dmg, AoE spike aura); all minions +50% damage during its life | Capstone |
| 6 | Soulkeep Pact | Passive | -- | Souls persist across rooms (cap 8 banked); minions persist at 50% HP if alive at room clear | Run-loop persistence |
| 7 | Necrotic Conductor | Passive | -- | Hexer curse stacks: your minions inherit and can apply +1 stack per 3 hits | Cross-class: Hexer |
| 8 | Last Rites | Passive | -- | Minion death refunds 1 Soul; leaves 3s 80% damage zone at death location | Death-as-utility |

---

### Riftstalker
Identity: Portal-anchor teleporter. RMB=place Rift Portal. Q=teleport to it. Traps at Rift.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Rift Anchor | Signature (RMB) | 5s | Place Rift Portal at cursor (20s duration); recasting moves it; 1 active max | Defines entire kit |
| 2 | Tearstep | Active (Q) | 4s | Instantly teleport to active Rift; arrival 2m shockwave 90%; on cursed target Rift: shockwave 200% | Cross-class: Hexer |
| 3 | Rift-Trap | Active (E) | 7s | Place delayed trap at Rift (or feet if no Rift); 3s arm, 130% + 2s slow zone | Zone control |
| 4 | Phase Slip | Active (R) | 6s | Invuln dash 5m; if dash ends within 3m of Rift: refund 50% Tearstep CD | Mobility + tempo |
| 5 | Schism Bloom | Ultimate (F) | 35s | Place 2nd Rift Portal 10s; free Tearstep between both (no CD); each Tearstep leaves a Rift-Trap | Capstone: NOTE requires N>1 portal support |
| 6 | Anchored Patience | Passive | -- | Within 4m of active Rift: +20% damage, +15% damage reduction | Rewards rift-camping |
| 7 | Doormaker's Gift | Passive | -- | Tearstep arrival: 2s Riftmark (+25% next ally hit); Hexer proc at Rift broadcasts Riftmark to all in 5m | Cross-class: Hexer proc broadcast |
| 8 | Echo Beneath | Passive | -- | Enemies dying within 3m of Rift: spawn Soul Mote (Summoner) + 1.5s 60% damage tear | Cross-class: Summoner economy |

---

### Vanguard
Identity: Frontline hybrid. Melee+shield bash+crossbow. Range-swap loop.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Ironpoint Volley | Signature (RMB) | 6s | 3-bolt crossbow burst 30-cone, 70% each; consumes Bashed stacks +25% per stack (max 4) | Pays out melee setup |
| 2 | Aegis Crash | Active (Q) | 5s | Shield bash 2m, 100%; applies 1 Bashed stack to target (max 4, 6s) | Builds RMB payout |
| 3 | Halberd Sweep | Active (E) | 6s | 180 frontal arc, 110%, knockback 2m; Hexer-cursed enemies knocked into Riftstalker Rift if in range | Cross-class: Hexer + Riftstalker |
| 4 | Standardbearer | Active (R) | 10s | Banner 6s in 5m radius: allies+minions +20% damage, +10% DR; immovable | Team anchor |
| 5 | Last Bastion | Ultimate (F) | 35s | 8s: melee hits fire free crossbow bolt 60%; shield bash is 360; +30% DR | Capstone |
| 6 | Pikewall Drill | Passive | -- | After 3 LMB melee hits: next crossbow shot pierces at 150% | Rewards range-swap |
| 7 | Oathbound Step | Passive | -- | Near Standardbearer banner OR Ironclad: Bashed stacks apply 2x rate | Cross-class: Ironclad |
| 8 | Bleedline Discipline | Passive | -- | Crossbow on 3+ Bashed targets: 4s bleed 40%/s; bleed ticks count as Hexer curse stacks | Cross-class: Hexer |

---

### Specter
Identity: Phase-shift life drainer. Drain HP from enemies -> spend on Ghost Form.

| # | Name | Type | CD | Effect | Synergy note |
|---|------|------|----|--------|--------------|
| 1 | Hollowveil | Signature (RMB) | 7s | Ghost Form 2s: invulnerable, pass through enemies, 4m aura 60%/s, drain 8% max HP per enemy | Defensive-offensive verb |
| 2 | Wraithgrasp | Active (Q) | 5s | Spectral pull 4m, 80%; drains 5% max HP from target back to self | Engage + sustain |
| 3 | Sepulcher Whisper | Active (E) | 4s | Cone 110%; applies 3s Drained: enemy heals you 20% of damage they take from any source | Sustain enabler |
| 4 | Marrowtide | Active (R) | 8s | AoE pulse 4m: damage = 50% of HP drained in last 6s; heals allies 20% of that | Burst window |
| 5 | The Long Vigil | Ultimate (F) | 40s | Ghost Form 6s no movement penalty; aura damage 2x; each kill in form extends +1s (max +4s) | Capstone |
| 6 | Tetherbound Greed | Passive | -- | Drained HP banked (cap 80% max HP); Ghost Form spends banked HP if 30%+ banked (no CD cost) | Resource transform |
| 7 | Corpsewake | Passive | -- | Enemies killed while Drained leave 3s soul-pool: allies in pool +15% lifesteal | Party utility |
| 8 | Final Veil | Passive | -- | Lethal damage prevented once/room -> force-triggers Hollowveil 1.5s; Hexer 3-stack proc while in Ghost Form heals 5% max HP | Cross-class: Hexer |

---

## FLAGS FOR ORCHESTRATOR
- Hexer is load-bearing for all 10 cross-procs. If cut: 9 passives need rewrite. Mark as Tier-S identity dependency.
- Riftstalker Schism Bloom (Ultimate) requires N>1 active portals -- confirm with RIFT_PORTAL_OPPORTUNITY.md scope.
- Summoner Soulkeep Pact (Passive) -- minions persist across rooms -- verify with RuntimeRoomManager room-transition code.
- BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md is named "8CLASS" but we have 10 -- rename to 10CLASS, add Summoner + Hexer RMB rows.
- Numbers (%, CD in seconds) are v0.1 design constants. Do NOT implement in code until playtest pass.
