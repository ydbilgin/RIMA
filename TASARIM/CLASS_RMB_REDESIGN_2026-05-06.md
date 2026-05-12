---
status: LOCKED
faz: 1
tarih: 2026-05-06
ozet: "RMB skill redesign 8 sınıf"
---
# Class RMB Redesign — All 10 Classes
Status: LOCKED 2026-05-06
Companion to: BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT_2026-05-06.md

## Philosophy

LMB and RMB are filler — they fill space between skill casts, generate resources, and maintain
combat feel. Heavy tools (dashes, resource trades, parry windows, mobility) live in skill slots.
RMB must be naturally repeatable without conscious decision-making.

## Changes Per Class

### Warblade
- OLD RMB: Rage Outlet (AoE burst, costs Rage) — too heavy, moved to skill slot
- NEW RMB: Crossguard Bash — short forward shoulder-check with sword crossguard, light damage, 0.3s stagger on contact, no resource cost, different angle from LMB arcing slash

### Elementalist
- OLD RMB: tap=Element Switch / hold=Lightbreak — hold-channel beam is skill-level; split into two pieces
- NEW RMB: Spark Tap — single element-tinted burst at cursor area, no charge, no resource, rhythm break between LMB bolts
- Element Switch: moved to UI hotkey (L1/LB on controller)
- Lightbreak: moved to skill slot with Aim Shot integration

### Shadowblade
- OLD RMB: Veil Flicker (phase-step + afterimage) — positional decision tool, moved to skill slot
- NEW RMB: Shadow Jab — quick off-hand short-range stab with inky smear, 0.2s rate-limit, no teleport, no afterimage

### Ranger
- OLD RMB: Tactical Roll (roll + arrow release) — mobility decision, moved to skill slot
- NEW RMB: Quick Loose — fast snap shot, lower damage than tapped LMB, no Draw Weight banked, fires even mid-LMB-charge as panic interrupt

### Ravager
- OLD RMB: Blood Pact (HP trade for Fury) — resource decision, moved to skill slot
- NEW RMB: Hook Pull — short axe-hook drag, pulls light enemies 1 tile toward player (or player pulled to heavy enemies), stuns briefly, no HP cost

### Ronin
- OLD RMB: Drawn Edge tap=iaido slash / hold=parry window — split; hold-parry moved to skill slot
- NEW RMB: Drawn Edge (tap only) — clean iaido slash, 3 frames, no stance change, naturally repeatable
- Parry Stance: moved to skill slot (dedicated parry with iframes + counter-cut on success)

### Gunslinger
- OLD RMB: Hip Shot (side-step + precise shot) — movement decision, moved to skill slot
- NEW RMB: Fan Shot — quick fan of 3 close-range shots in forward cone, 3 chambers consumed, no movement, no aim decision

### Brawler
- OLD RMB: Weave (defensive micro-move, perfect timing = iframes) — split; iframe-perfect-dodge moved to skill slot
- NEW RMB: Weave (evade only) — small defensive shoulder-roll, short evade, no iframes, banks 1 Counter on contact-avoidance
- Slip: moved to skill slot (full iframe dodge + guaranteed Counter bank on success)

### Summoner
- RMB: Tether Pull — yanks nearest minion 1 tile toward cursor, chip damage + focus-tag on enemies, 0.25s recovery. True filler.

### Hexer
- RMB: Ill Wind — quick conical cursed mist puff (3 tiles), chip damage, 0.3s recovery. Tags adjacent enemies with 1 Hex stack between LMB cycles.

