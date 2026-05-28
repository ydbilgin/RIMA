---
name: nine-class-animation-states-demo-phase1-plan
description: 9 class (Warblade hariç) demo Faz 1 animation state planı master dosya. Cross-class shared base (Idle/Walk/Hit/Death) + class-spesifik skill state. Tier 1/2/3 prioritization. Production YAPMA gece — sadece plan. Pending user approval Faz 4'te 10 class production.
metadata:
  type: project
  source: NLM 30ddffa5 + canonical_character_roster_v2 + warblade_animation_states_demo_phase1_plan
---

## Workflow disiplini
- Claude state spec yazar → kullanıcı onaylar → MCP `create_character_state` gen ([[feedback-state-gen-mcp-user-approval-exception]])
- **Animation in-between** kullanıcı yapar (Unity Animator / Aseprite tween)
- **Gece PixelLab gen YASAK** ([[feedback-no-pixellab-night-autonomous]])
- Faz 1 demo Warblade TEK, 9 class **Faz 4 Steam Demo** scope ([[project-demo-phase1-milestone-lock]])
- Bu plan **referans + Faz 4 production sırası**, Track A demo'yu bloklamaz

## Cross-class shared base (Tier 1 — her class identical state count, pose adapts)

| State | View count | First → End frame (universal) | Class adaptation notu |
|---|---|---|---|
| **Idle** | 5 native + 3 mirror | rest pose → weight shift | her class kendi silah stance'i (lantern hover for Summoner, hood-on for Ranger) |
| **Walk** | 5 + 3 | left foot fwd → right foot fwd | universal gait, silah HandAnchor'da idle |
| **Basic Attack** | 5 + 3 | windup → follow-through | silaha göre (swing/shoot/draw/cast) |
| **Hit/Stagger** | 4 (S/E/N/W) | impact recoil → recovery | universal |
| **Death** | 4 | knees buckling → flat | universal |

**Toplam Tier 1 her class:** 5 state × 5-8 view ≈ 25-40 keyframe pair. **9 class toplamı:** ~225-360 keyframe pair.

## Class-spesifik Tier 2 (skill state, demo havuzdan 4)

| Class | Tier 2 skill state #1 | #2 | #3 | #4 |
|---|---|---|---|---|
| **Ronin** | Iaido Stance (hareketsiz Tension build, glow aura) | Sakura Veil (deflect penceresi, yaprak particle) | Counter Draw (parry windup→reflect) | Quickdraw Slash (kın çek+slash) |
| **Gunslinger** | Rift Dash (slide + dual fire) | Fan the Hammer (6-shot rapid pose) | Reload Dance (slide+reload) | Deadshot (cursor aim+pierce) |
| **Ranger** | Aimed Shot (1.5s şarj→release) | Disengage (geri takla) | Rapid Fire (kanal pose hold) | Explosive Trap (yere koy) |
| **Elementalist** | Fireball (orb hover cast) | Glacial Spike (buz çıkar) | Meteor (windup overhead) | Blink (ışınlanma start→end) |
| **Shadowblade** | Scarbinding (phase-through pose) | Phase Step (dash + invis fade) | Backstab Mark (arkadan vuruş) | Severance (collapse cleave) |
| **Ravager** | Bloodlust Strike (koni swing) | Carnage Spin (2s dönüş) | Frenzied Leap (jump→land) | Wild Hack (devasa swing, vulnerable) |
| **Hexer** | Corruption (anlık cast + 3 stack hand glow) | Pandemic (spread gesture) | Hexblast (10 stack patlama windup) | Blight Sigil (zemine kur) |
| **Brawler** | Bully (4-jab pose) | Crackjaw (combo finish hook) | Counter Blow (parry pose) | Glass Strike (cross-class execute) |
| **Summoner** | Raise Skeleton (lantern raise cast) | Command Beacon (cursor point) | Blood for Power (minyon feda gesture) | Mass Sacrifice (Phase 2 ulti pose) |

**Toplam Tier 2 her class:** 4 state × 5-8 view ≈ 20-32 keyframe pair. **9 class:** ~180-288 keyframe pair.

## Tier 3 (polish, Track B Faz 4+)
- Combo finishers (3-hit chain)
- Skill variants (Frenzy Strike, Pandemic Wave, etc)
- Victory pose
- Run faster cycle (walk variant)
- Idle alternates (boredom)

## Char ID mapping (canonical_character_roster_v2)
| Class | PixelLab ID | Canvas |
|---|---|---|
| Ronin | a7957352-cc57-44a1-a9fc-96f1fbd1119a | 128×128 |
| Gunslinger | a78545eb-ef10-4e1e-827e-784000e45886 | 124×124 |
| Ranger | d5b1cf71-0158-4347-97b9-a34a5ac0d98a | 128×128 |
| Elementalist | 4c83c0be-e856-48f1-b8b5-9626e041a082 | 120×120 |
| Shadowblade | deee34b5-7796-4c8f-9262-b8a83f907240 | 124×124 |
| Ravager | 091e9552-7f57-44d0-8ae3-49f689304c7e | 124×124 |
| Hexer | e260a1af-930d-4e5b-9d5e-bc11abd7c92f | 124×124 |
| Brawler | d4fa3d13-35f1-4d65-849c-dfafff688593 | 120×120 |
| Summoner | 83039c80-d2fe-448a-8c15-ecf55c0f2f7c | 124×124 |

## Production sıra (Faz 4 hazırlık, demo demo değil)
**Phase A (Faz 1 ZATEN AÇIK):** Warblade ([[warblade-animation-states-demo-phase1-plan]])

**Phase B (Faz 4 öncelik, 4-char playtest):**
1. Elementalist (mage archetype)
2. Ranger (ranged physical)
3. Shadowblade (stealth melee)

**Phase C (Faz 4 production tail, 6-char batch):**
4. Ronin
5. Gunslinger
6. Ravager
7. Hexer
8. Brawler
9. Summoner

## PixelLab prompt template (9 class universal)
Sadece char_id ve view değişir, state name/description her class state'inde:

```
Tool: PixelLab Create Character State (MCP create_character_state)
Character: <char_id from canonical_character_roster_v2>
State Name: <state_name>
View: south (önce, sonra E/SE/NE)
Canvas: <class canvas from roster>
Pivot: bottom-center foot

First Frame: <pose tanım>
End Frame: <pose tanım>

Outfit: identical to anchor reference (weaponless body, weapon HandAnchor child)
Camera POV: high top-down 3/4 ~70-80°
Negative: do NOT add weapon (varsa silaha class spesifik exception)
Background: transparent
Style: pixel art, 16-color, crisp edges
```

## Skill state vs basic attack reuse
Bazı skill state'leri basic attack ile VFX overlay yetebilir (cost saving):
- Sunder Mark (Warblade) = Basic Attack + cyan VFX overlay
- Spitback (Hexer) = Basic Attack + purple curse VFX
- Quickdraw (Gunslinger) = Basic Attack + muzzle flash variant

**Reuse listesi Faz 4 production'ında her class için belirlenir** (Track B otonom analiz).

## Cross-link
[[canonical-character-roster-v2]] [[warblade-animation-states-demo-phase1-plan]] [[project-demo-phase1-milestone-lock]] [[feedback-state-gen-mcp-user-approval-exception]] [[ronin-12-common-skills-spec]] [[gunslinger-12-common-skills-spec]] [[ranger-12-common-skills-spec]] [[elementalist-12-common-skills-spec]] [[shadowblade-12-common-skills-spec]] [[ravager-12-common-skills-spec]] [[hexer-12-common-skills-spec]] [[brawler-12-common-skills-spec]] [[summoner-12-common-skills-spec]]
