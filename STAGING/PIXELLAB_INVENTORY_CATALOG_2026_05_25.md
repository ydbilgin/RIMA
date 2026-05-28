# PixelLab Inventory Catalog — 220 Objects (2026-05-25)

**Source:** `mcp__pixellab__list_objects` paginated full scan (4 batches × 50)
**Account:** 5000 gen Tier 2, used 3631, remaining 1369
**Total completed objects:** 220

## Categorization (KEEP / PULL / DELETE proposal)

Legend:
- **KEEP** = stay in cloud, still useful as reference / future asset
- **PULL** = download to Unity project Assets/, then can delete from cloud
- **DELETE** = clearly unused, safe to remove
- **ASK** = needs user input before action

---

## 🧱 WALLS (KEEP ALL — user explicit "duvarlar hariç dursun")

| ID | Description | Size | Tag |
|---|---|---|---|
| 9670ddb0-3f5d-4636-adfa-9b5227499925 | RIMA Act 1 Shattered Keep wall | 96×96 | RIMA_Wall_Production_v1_Batch1 |
| f91b6a15-d915-4524-8c8f-24726674d698 | RIMA Act 1 Shattered Keep wall | 96×96 | RIMA_Wall_Production_v1_Batch1 |
| 221a7c39-0adb-46ea-8b5d-796959e4969e | RIMA Act 1 Shattered Keep wall | 96×96 | RIMA_Wall_Production_v1_Batch1 |
| 7ffea6dc-eea3-49bc-bc85-374d292a648b | RIMA Act 1 Shattered Keep wall | 96×96 | RIMA_Wall_Production_v1_Batch1 |
| 4c561563-f0ff-4c35-93d6-0f8490fdab6f | Tall stone dungeon wall | 96×160 | (no tag) |
| 6bde8fba | Iso 35° dungeon wall | 128×128 | act1_wall_structural_v1 |
| 6f22346a | Iso 35° dungeon wall | 128×128 | act1_wall_structural_v1 |
| 34d423e2 | Iso 35° dungeon wall | 128×128 | act1_wall_structural_v1 |
| 576575a3 | Iso 35° dungeon wall | 128×128 | act1_wall_structural_v1 |
| e325aa12 | Act 1 stone wall | 128×128 | aaa |
| aa49fd5c, 7daff11c, 0a36c905 | Act 1 stone wall pilot | 128×128 | act1_wall_pilot_a_s95 (3 objects) |
| 65c99904, a52f6711, abf9c178, 8530799c | Act 1 stone wall pieces | 128×128 | act1_wall_pieces_s95 (4 objects) |
| 825ddbdd, 06338801, 76693f8f, f053b5f0 | Dark warm grey wall | 32×32 | keep_wall_v2 (4 objects) |
| 2c1ebaac, 7f603f7d, eb3fcf85, 56cc237f | Dungeon wall 32px | 32×32 | alabaster_wall (4 objects) |
| **TOTAL WALLS** | | | **~25 wall objects KEEP** |

---

## 🏛️ COLUMNS / PILLARS (PULL → use as wall-less Stone Column)

| ID | Description | Size | Status |
|---|---|---|---|
| 6b52751d-67eb-4684-b7e4-f4a0a00c7831 | Broken stone pillar segment | 64×96 | ✅ PULL (matches P0 Stone Column intent) |

---

## 🗿 STATUES / RITUAL (PULL TOP 3-4 — Altar/Sunak candidates)

12 candidates @ 64×64, tag `act1_statue_ritual_s95`. User selects best 3-4:

| ID | Description |
|---|---|
| 3675a661 | Ancient ruined stone keep statue |
| a7d8dd6d | Ancient ruined stone keep statue |
| 52e58d14 | Ancient ruined stone keep statue |
| 8b0f8790 | Ancient ruined stone keep statue |
| f75b1fa4 | Ancient ruined stone keep statue |
| f1ed6cce | Ancient ruined stone keep statue |
| a2fb0a7f | Ancient ruined stone keep statue |
| e899a33d | Ancient ruined stone keep statue |
| a0da1143 | Ancient ruined stone keep statue |
| c5711681 | Ancient ruined stone keep statue |
| d5574785 | Ancient ruined stone keep statue |
| cdc9ceb0 | Ancient ruined stone keep statue |
| 1ecd6d55 | Ancient ruined stone keep statue |

Action: get_object preview each, user picks best 3-4 → PULL → DELETE rest

---

## 🔥 MOUNTING APPARATUSES (PULL TOP 2-3 — Brazier base candidates)

14 candidates @ 64×64, tag `act1_mounting_apparatus_s95`. User selects best 2-3:

| ID | Description |
|---|---|
| 41342e20, bf737208, 98bc117a, 7227fa35, de169bda, e4ff0cfa, 173aa624, ebd1307e, f7763267, 7ab5ec96, f88e821a, 1802229f, 4fce26a7, bb34bab3, 50257aa7 | Pure mounting apparatuses |

Action: preview, pick 2-3, PULL rest DELETE

---

## ⛰️ CLIFF / TILE BASE (PULL)

| ID | Description | Size |
|---|---|---|
| 886684b6-14f5-4bd0-b377-7d9816adedff | Top-down 35° cliff drop base | 32×32 |
| a5dbe36c-71db-4834-afb8-6d7244ac6c98 | Top-down 35° rift pool base | 32×32 |

Action: PULL both (cliff edge candidate + ritual pool)

---

## 🪨 SCATTER PROPS (PULL ALL — wall-less object barriers + floor accent)

| ID Group | Description | Size | Count |
|---|---|---|---|
| f4cad16c, 4d6ec9d5, 4b320e3d, ee13e5e2 | Dark earth dirt patch | 32×32 | 4 |
| 5cdd41f6, 4e947c06, 0b239a15, 79c3845c | Broken stone rubble piece | 32×32 | 4 |
| 5c474658, 20517ffc, 8cd4557a, 0df42e80 | Organic moss patch | 48×48 | 4 |
| 488fb7dd, 80c1ac28, 361267b0, a271376e | Irregular stone cluster | 48×48 | 4 |

Action: PULL all 16 (scatter variety perfect for boundary dressing)

---

## 📦 CRATES (PULL — object barriers)

| ID | Description | Size |
|---|---|---|
| ce1d1144-eecb-4dbd-b513-d200a135585a | Wooden crate iron bands | 64×64 |
| e27c411d-e3be-40d2-9646-765e59b87feb | Wooden crate iron bands | 64×64 |

Action: PULL both

---

## 🪦 RUBBLE / DEBRIS (PULL)

| ID | Description | Size |
|---|---|---|
| 075242f4-6184-4295-baa1-2fdbd16c8707 | Rubble heap | 80×80 |
| 60502d16-5cd6-4890-bbec-de9e61cc1294 | Violet rift dust patch overlay | 64×64 |
| eea16a35-7fb6-488f-a76e-6803ceac82b9 | Loose rubble debris pile | 64×64 |
| f2ba1bed-f225-4237-bff6-aab6530c516d | Hairline rift fracture | 64×64 |
| 5dbfb74a-4f5c-4d63-9477-9a41518d3d14 | Faint hexagonal honeycomb | 64×64 |
| 93130cc6-b427-45ae-a545-0378eacbf638 | Pale cream-beige sand drift | 64×64 |
| fd1ab1b9-c2bc-47a5-8941-aad11910b600 | Soft warm pink dust cloud | 64×64 |

Action: PULL all (cliff/arena dressing)

---

## 👹 MOBS (KEEP — already referenced for enemy spawn)

14 objects @ 64×64, tag `act1_mob_s95`. Wave 1 enemies for combat scenes.

| ID Group | Description |
|---|---|
| dd2c1909, ff768082, 3b22bdfa, 9938f947, 3b7b3d40, ee4439c9, de32fa37, 92904369, 709f2c76, a17d9fd4, b364028d, 67dd4af5, e8695fff, 457560c3, e42dd84f, 510d2864 | Act 1 Shattered Keep ancient mob |

Action: KEEP all (will need them for combat)

---

## ⚔️ WEAPONS (PULL needed classes, DELETE unused)

| ID | Description | Class |
|---|---|---|
| 4bde2642 | Curse staff | Hexer (S82, class deprecated?) — ASK |
| 894bba4a | Flintlock dueling pistol | Gunslinger — KEEP |
| 9312ea86 | Reverse-grip dagger | Shadowblade — KEEP |
| a032d9b5 | Katana | Ronin — KEEP |
| 31ee0f73 | T2 rift greatsword | Warblade T2 — KEEP |
| afcab14c | Soul lantern | (unknown) — ASK |
| 19693073 | Single-handed hand_axe? | (unknown) — ASK |
| ebc33ebf | Compound bow | Ranger — KEEP |
| c0509b93 | Two-handed greatsword | Warblade — KEEP |

Action: ASK user about Hexer (still active?) + 2 unknowns

---

## 🎨 SKILL ICONS (DELETE BULK — 25+ icons, "dark fantasy game skill icon" generic)

25 objects @ 64×64, mostly generic descriptions:

| ID Group | Description |
|---|---|
| 71a8ea2f, 6c577804, a46d8a81, 7cb94adb, 5fac9a06, d16e4d8c, 67c43298, aafef68b, d991be20, 676e7a8e, 08ebbceb, 5a5ba856, 92e26475, a203494f, 343d5ee0, 6de02faa, 9ed449c6, e3846e91, aa0b0f61 | Dark fantasy skill icon |
| ca29419d | Crushing blow icon |
| a49fbc6c | Rift portal strike icon |
| 213a59b5 | Spinning sword icon |

Action: DELETE bulk (likely unused — ASK user to confirm before delete) OR PULL if needed for UI

---

## 🎭 PAINTERLY PIXEL ART (ASK — mixed pile, ~25 objects)

23 objects with prefix "Painterly pixel art" (various sizes 64-128). Mixed content: characters, environments, etc.

| ID Range | Status |
|---|---|
| 23a2dce1...d536f49c (23 objects total) | Stylistic experiments — ASK user |

Action: PREVIEW first 5, USER decides what's worth keeping

---

## 🦸 PAINTERLY 8DIR CHARACTERS (PULL likely — 3 character sprites with 8 directions)

| ID | Description | Size |
|---|---|---|
| 441bccf0-9d9c-4bb7-a981-555b132eae00 | Painterly longsword 8dir | 96×96 |
| 692f43ce-2c6d-45ea-910d-2b5ec4f6ec99 | Painterly katana 8dir | 64×64 |
| e84d8c62-7f79-46be-9cc7-9db06cf6e59e | Painterly longsword 8dir | 64×64 |

Action: ASK — may be Warblade test variants

---

## 💥 VFX (PULL — limited stock)

| ID | Description | Size | Anim |
|---|---|---|---|
| 11127e69 | Hitspark VFX burst, white | 64×64 | 1anim ✅ |
| 58c183a0 | Cold blue energy dash trail | 64×64 | 1anim ✅ |

Action: PULL both (VFX never enough)

---

## 🖼️ DECALS (PULL — keep_decal_v2 + alabaster_decal)

| ID Group | Description | Size | Count |
|---|---|---|---|
| 9214aa06...21406da0 | Dark dungeon floor decal | 32×32 | 8 (keep_decal_v2) |
| 5ccc5721, 3b41f8eb, 32e911d1, 31827857 | Decorative decal | 32×32 | 4 (alabaster_decal) |

Action: PULL all 12 (floor accents)

---

## 🌐 16:9 GAMEPLAY MOCKUPS (KEEP — 10 reference images)

10 mockups @ 256×256, "Full 16:9 pixel art gameplay screenshot" — already produced, design reference value.

| ID Group |
|---|
| a271376e (knight+thief combat), 06edbfdb (ranger+golem HUD), 25c6ab46 (assassin vs knight), ebed5dd3 (mage+arch+crabs), 5e833d4f (ranger+skeleton horde), b55849ca, 1a2222ac, c7a5c672, 53e19d3e, 85e763c5 (5 more, not previewed) |

Action: KEEP all (visual reference value)

---

## 🚧 PRE-RIMA EXPERIMENTAL (DELETE candidates — 4 objects, vague tags)

| ID | Description | Reason |
|---|---|---|
| 4793b916 | Transform into RIMA | Vague S81 experiment |
| 02cee97d | Transform into RIMA | Same |
| b204a08b | Transform into RIMA | Same |
| 9b562391 | Transform into RIMA | Same |

Action: DELETE (S81 experimental, no follow-up reference)

---

## 📊 Summary stats

| Category | Count | Action |
|---|---|---|
| WALLS | ~25 | KEEP cloud |
| Columns/Pillars | 1 | PULL |
| Statues/Ritual (Altar candidates) | 12 | PULL 3-4, DELETE rest |
| Mounting/Brazier | 14 | PULL 2-3, DELETE rest |
| Cliff/Tile base | 2 | PULL both |
| Scatter props | 16 | PULL all |
| Crates | 2 | PULL both |
| Rubble/Debris | 7 | PULL all |
| Mobs | 14 | KEEP cloud |
| Weapons | 9 | PULL 5-6, ASK 3 |
| Skill icons | 22 | ASK user before delete |
| Painterly art | 23 | ASK user (preview first) |
| Painterly 8dir chars | 3 | ASK user |
| VFX | 2 | PULL both |
| Decals | 12 | PULL all |
| Mockups (16:9) | 10 | KEEP cloud |
| Pre-RIMA experimental | 4 | DELETE |
| Misc Act1 (mixed) | ~35 | Needs detailed review |
| **TOTAL** | **220** | |

## Cleanup credit savings
PixelLab subscription does NOT refund delete (generations not returned). Delete is for organization, not credit recovery.

## Next steps
1. User reviews this catalog
2. Approves bulk DELETE list (Pre-RIMA experimental + skill icons + unused statues/braziers/painterly)
3. PULL all marked PULL items → save to `Assets/Sprites/PixelLab_Pulled/<category>/`
4. After PULL verified, DELETE from cloud
