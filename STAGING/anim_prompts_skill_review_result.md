# Anim Prompts Skill Alignment Review - 2026-05-13

## Ozet
- PASS: 21/30
- WARN: 8
- FAIL: 1

## Karakter Bazli Review

### 01 - Warblade
**run** [6f] PASS - Heavy two-handed greatsword charge fits Warblade identity, weapon weight, and braced movement style.
**attack_basic** [5f] PASS - Horizontal two-handed sweep matches Iron Combo first hit: sweep/horizontal opener.
**attack_heavy** [6f] WARN - Reads as a skill-level ground smash and can map to Gravity Cleave/Earthsplitter, but it does not show pull, crack, knockup, or close AoE detail.

### 02 - Elementalist
**run** [6f] PASS - Light caster run with hand-adjacent rune effect and no staff fits Elementalist identity.
**attack_basic** [5f] WARN - No staff, but the projectile is the rune disc itself rather than a clear hand-launched Rift Bolt; empowered third bolt is not represented.
**skill_cast** [7f] PASS - Two-arm elemental release can represent Fireball/Meteor-style skill casting and avoids staff use.

### 03 - Shadowblade
**run** [6f] PASS - Low compact sprint with reverse-grip twin daggers fits Shadowblade identity.
**attack_basic** [4f] PASS - Fast reverse-grip slash chain fits Veil Strike/Twin Carve basic attack language.
**attack_heavy** [5f] WARN - Twin lunge fits assassin burst, but it does not show phase-step, Rift Scar, mark detonation, or Chain Cull behavior.

### 04 - Ranger
**run** [6f] PASS - Bow in left hand, athletic forward run, and quiver bounce fit Ranger identity.
**attack_basic** [5f] WARN - The left-hand bow shot fits Rift Arrow, but the prompt does not specify compound bow and reads as a normal bow release only.
**skill_cast** [7f] PASS - Charged full draw with bright arrow streak fits hold Rift Arrow or Pinning Shot visual language.

### 05 - Ravager
**run** [6f] PASS - Wild stomping charge with dual compact axes fits Ravager identity.
**attack_basic** [5f] WARN - Alternating axe chops fit brutality, but they miss the locked 3-hit Brutal Swing sequence: wide arc, overhead slam, ground pound.
**attack_heavy** [7f] PASS - Forward leap into landing AoE maps well to Frenzied Leap.

### 06 - Ronin
**run** [6f] PASS - Saya hand position, draw-ready posture, and controlled strides fit Ronin identity.
**attack_basic** [5f] FAIL - This is an iaido draw-slash and return to saya, which matches Drawn Edge/RMB more than LMB Sheath Walk moving light slash.
**attack_heavy** [6f] WARN - Skill-level katana finisher is plausible, but it is a single overhead cut rather than Quickdraw Slash, Soken-giri fan slashes, Iaido Stance, or Void Cleave cone payoff.

### 07 - Gunslinger
**run** [6f] PASS - Dual pistols low and parallel with kinetic run fits Gunslinger identity.
**attack_basic** [4f] PASS - Both pistols fire on the same frame, correctly matching simultaneous Dual Fire.
**skill_cast** [6f] PASS - Rapid alternating muzzle flashes and fan-fire follow-through match Fan the Hammer skill language.

### 08 - Brawler
**run** [6f] PASS - Guard-up forward run with tight footwork fits Brawler identity.
**attack_basic** [4f] WARN - A straight cross is close to a quick punch, but LMB is specifically Jab with Charge+1 and optional 4-hit auto-combo language.
**attack_heavy** [5f] PASS - Wide hook/haymaker can represent Kidney Hook or a heavier brawler skill hit.

### 09 - Summoner
**run** [6f] PASS - Lantern-held composed run fits Summoner identity.
**attack_basic** [5f] PASS - Open-palm command gesture plus short staff tap matches Command Strike and avoids confusing it with Soul Dart.
**skill_cast** [7f] PASS - Lantern raised with cyan wisps spiraling outward reads as summon/raise magic.

### 10 - Hexer
**run** [6f] PASS - Hunched uneven run with skull staff prop and grimoire fits Hexer identity.
**attack_basic** [4f] PASS - Curse bolt launches from the left fingertips while staff remains a prop, matching hand-cast Hex Bolt.
**skill_cast** [7f] WARN - Green ring burst can imply Blight Sigil/Hexblast, but the locked kit emphasizes hand curse grasp, stacks, curse zone, or stack explosion more clearly than staff slam.

## Genel Bulgular
- All 30 prompts are within the hard 4-16 frame limit.
- Strongest locked-skill matches: Warblade attack_basic, Gunslinger attack_basic, Summoner attack_basic, Hexer attack_basic, Ravager attack_heavy.
- Highest-priority fix: Ronin attack_basic should become a moving Sheath Walk light slash instead of a stationary iaido draw-slash.
- Medium-priority fixes: Elementalist attack_basic should launch a clear hand Rift Bolt, Ravager attack_basic should show the 3-hit Brutal Swing chain, Ranger attack_basic should name/show compound bow, and Hexer skill_cast should show curse stack/zone language more explicitly.
