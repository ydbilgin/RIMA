---
name: canonical-character-roster-v2
description: 2026-05-18 LIVE — 10 canonical class anchor PixelLab IDs (user uploaded + cleaned). Replaces older roster-LOCK with TBD IDs. 4-char playtest focus = Warblade FIRST + Elementalist + Ranger + Shadowblade.
metadata: 
  node_type: memory
  type: project
  originSessionId: f8cac4ae-346e-4aa6-8c4b-f83c84e7c29d
---

# Canonical Character Roster v2 — LIVE IDs (2026-05-18)

User produced + cleaned the 10-class roster via PixelLab Web UI V3. Identity state edits applied (cursemark, recolor, hair removal, young variant). All anchors are **weaponless body** (weapons = separate sprite child per `weaponless_animation_v1`).

## 10 canonical anchors

| # | Class | PixelLab ID | Size | Notes |
|---|---|---|---|---|
| 1 | **Warblade** | `2656075d-d113-4f18-a6c1-94b5a6b8bf65` | 8dir 120×120 | Young variant (face+hair edit applied). Adult version `dbfbb77d-...` also exists |
| 2 | **Ronin** | `a7957352-cc57-44a1-a9fc-96f1fbd1119a` | 8dir 128×128 | Weaponless body, no katana baked. Drawn katana = separate `create_object` sprite |
| 3 | **Gunslinger** | `a78545eb-ef10-4e1e-827e-784000e45886` | 8dir 124×124 | Trenchcoat variant with all hair accessories removed (white tie + teal cap fix). Anchor `4a7c999d` "hard armored" alt variant |
| 4 | **Ranger** | `d5b1cf71-0158-4347-97b9-a34a5ac0d98a` | 8dir 128×128 | Hood-on default. Optional hood-back state pending if combat readability needed |
| 5 | **Elementalist** | `4c83c0be-e856-48f1-b8b5-9626e041a082` | 8dir 120×120 | **Orb ONLY** (memory `class_identity_pivots_s43` LOCK — staff dropped). Orb = separate sprite child, hover R-palm |
| 6 | **Shadowblade** | `deee34b5-7796-4c8f-9262-b8a83f907240` | 8dir 124×124 | Teal recolor (was purple, clash with Hexer fix). Red accents kept |
| 7 | **Ravager** | `091e9552-7f57-44d0-8ae3-49f689304c7e` | 8dir 124×124 | Long hair + beard + leather chest barbarian read |
| 8 | **Hexer** | `e260a1af-930d-4e5b-9d5e-bc11abd7c92f` | 8dir 124×124 | Cursemark + face revealed state (was full-hood, no identity glyph) |
| 9 | **Brawler** | `d4fa3d13-35f1-4d65-849c-dfafff688593` | 8dir 120×120 | Shirtless + wrap gloves. Knuckles = body part, no separate weapon |
| 10 | **Summoner** | `83039c80-d2fe-448a-8c15-ecf55c0f2f7c` | 8dir 124×124 | Focus orb in hand = identity. Familiar/pet may be separate sprite if needed |

## 4-char playtest focus

**Phase A (Warblade FIRST):** Full pipeline on Warblade — anchor + sword 96×96 + animations + Unity wire + WASD playtest. Lessons learned applied to other 3.

**Phase B (after Warblade validation):** Elementalist + Ranger + Shadowblade same pipeline.

**Phase C (after 4 validated):** Other 6 (Ronin, Gunslinger, Ravager, Hexer, Brawler, Summoner) batch production.

| Phase | Classes | Reasoning |
|---|---|---|
| A | Warblade | Most developed, lessons-learned target |
| B | Elementalist, Ranger, Shadowblade | Archetype dengeli (mage / ranged physical / stealth melee) — Warblade ile combat variety test |
| C | Ronin, Gunslinger, Ravager, Hexer, Brawler, Summoner | Production tail after pipeline validated |

## Weapon production (LIVE)

| # | Weapon | PixelLab ID | Size | Status |
|---|---|---|---|---|
| 1 | Warblade longsword | `441bccf0-9d9c-4bb7-a981-555b132eae00` | 8dir 96×96 | ✓ LIVE (recommended size after sword=64 felt small) |
| 2 | Ronin katana | `692f43ce-2c6d-45ea-910d-2b5ec4f6ec99` | 8dir 64×64 | ✓ LIVE (curved blade + brass tsuba + brown tsuka) |

Older Warblade sword 64×64 (`e84d8c62`) exists, can deprecate (96 preferred).

Other weapons (priority order, NOT YET produced — production minimum mode):
- 3. Gunslinger pistol 32×32 (1-dir runtime rotation OK)
- 4. Ranger bow 64×64 (5+3 mirror)
- 5. Elementalist orb 48×48 (1-dir hover effect, separate sprite)
- 6. Shadowblade dagger 32×32 (1-dir runtime rotation OK)
- 7. Ravager axe 64×64
- 8. Hexer wand 48×48
- 9. Brawler gloves 48×48 pair (or built-in body)
- 10. Summoner focus 48×48

## State identity edits applied (these are the canonical anchors now)

| Class | Original anchor (deleted/replaced) | Canonical state ID |
|---|---|---|
| Warblade | adult `dbfbb77d` | young v2 `2656075d` (kept both for now) |
| Hexer | base `4d35c634` (deleted) | cursemark `e260a1af` (now canonical) |
| Shadowblade | base `e42f2057` (deleted) | teal recolor `deee34b5` (now canonical) |
| Gunslinger | base `9d287494` trenchcoat | no-hair-acc `a78545eb` (now canonical) |

Other 6 classes: no state edits, anchors are direct user productions.

## Related

- [[weaponless-animation-v1]] — silahsız body + WeaponSR child SR LOCK
- [[5000-pixellab-allocation-lock]] — budget LOCK
- [[v15d-composition-budget-lock]] — map composition LOCK
- [[pixellab-character-states-workflow]] — state production workflow
- [[class-identity-pivots-s43]] — Elem orb / Cursemark / pose decisions
