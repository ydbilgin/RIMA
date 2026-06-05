# Task T6 Result Report — Skill Registration & Draft Guard

## What Registered
1. **Ronin (`ClassType.Ronin`)**:
   - `Quickdraw` (Real: `typeof(RoninQuickdraw)`, CD `2.2s`, Common)
   - `Iaido Stance` (Real: `typeof(RoninIaidoStance)`, CD `5.0s`, Common)
   - `Sakura Veil` (Real: `typeof(RoninSakuraVeil)`, CD `6.0s`, Rare)
   - `Final Draw` (Real: `typeof(RoninFinalDraw)`, CD `12.0s`, Mythic)
   - `Moon Cut` (Placeholder, `isImplemented = false`)
   - `Still Water` (Placeholder, `isImplemented = false`)
   - `Petal Riposte` (Placeholder, `isImplemented = false`)
   - `Last Sheath` (Placeholder, `isImplemented = false`)

2. **Ravager (`ClassType.Ravager`)**:
   - All 8 skills registered as placeholders (`isImplemented = false`): `Rend Hook`, `Bone Maw`, `Armor Split`, `Red Wake`, `Hook Slam`, `Gore Path`, `Fury Sink`, `Last Roar`.

3. **Gunslinger (`ClassType.Gunslinger`)**:
   - All 8 skills registered as placeholders (`isImplemented = false`): `Deadeye`, `Quick Reload`, `Ricochet`, `Smoke Round`, `Fan Hammer`, `Silver Mark`, `Pierce Shot`, `Last Bullet`.

4. **Brawler (`ClassType.Brawler`)**:
   - All 8 skills registered as placeholders (`isImplemented = false`): `Jawbreaker`, `Shoulder In`, `Ground Lock`, `Counter Jab`, `Iron Guard`, `Ring Step`, `Uppercut`, `No Mercy`.

5. **Summoner (`ClassType.Summoner`)**:
   - All 8 skills registered as placeholders (`isImplemented = false`): `Wisp Call`, `Bone Pact`, `Rift Totem`, `Twin Shade`, `Offering`, `Grave Door`, `Rift Swarm`, `Last Familiar`.

6. **Hexer (`ClassType.Hexer`)**:
   - All 8 skills registered as placeholders (`isImplemented = false`): `Wither`, `Hex Brand`, `Black Thread`, `Mire Curse`, `Glass Bone`, `Dread Bloom`, `Witch Knot`, `Last Omen`.

---

## How the Guard Works
1. **Flag Addition (`SkillData.cs`)**:
   - Added `public bool isImplemented = true;` to `SkillData` ScriptableObject, allowing runtime instances to carry their implementation readiness.
2. **Registration Integration (`SkillDatabase.cs`)**:
   - Updated helper method `Add(...)` to accept `bool isImplemented = true` (defaulting to true) and assign it to the instantiated `SkillData` object.
   - For all placeholder skills, `isImplemented: false` is explicitly passed.
3. **Draft Offer Filtering (`SkillDatabase.cs` & `SkillOfferGenerator.cs`)**:
   - In `SkillDatabase.GetPool`, a check `if (!s.isImplemented) continue;` is added to exclude placeholders from active class offerings.
   - In `SkillOfferGenerator.GetSource`, the fallback manual pool collection loop is updated with `if (!s.isImplemented) continue;` as a secondary guard.
   - Because `SkillDatabase.GetAll()` and `SkillDatabase.FindByName(...)` are untouched, Codex and Character Selection screens can still access, list, and read these placeholder skills, but players will never draw them during active run drafts.

---

## Files Touched
- [SkillData.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/SkillData.cs)
- [SkillDatabase.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/SkillDatabase.cs)
- [SkillOfferGenerator.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/SkillOfferGenerator.cs)
