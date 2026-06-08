# Testler ve Acceptance Önerileri

## PlayMode smoke — boot flow

```txt
Test: MainMenu_Start_Loads_CharacterSelectOrChamber_NotArena
Given MainMenu scene loaded
When Start/New Run clicked
Then CharacterSelect/Chamber flow appears
And _Arena is not loaded until interaction confirms run start
```

## PlayMode smoke — ESC

```txt
Test: Escape_Opens_Pause_NotCodex
Given _Arena gameplay active
When ESC pressed
Then PauseMenu visible
And SkillCodex not visible
And Time.timeScale == 0
```

```txt
Test: Pause_Codex_Button_Opens_Codex
Given PauseMenu visible
When Skill Codex button clicked
Then SkillCodex visible
When ESC pressed
Then SkillCodex closes and returns to PauseMenu or gameplay according to chosen stack rule
```

## PlayMode smoke — room clear

```txt
Test: Killing_All_Enemies_Unlocks_Room_Progression
Given _Arena combat room with 2 enemies
When both enemies die
Then room clear state reached
And reward or exits spawn
And player input remains enabled
And Time.timeScale == 1
```

## EditMode / validation — skill icons

```txt
Test: ImplementedSkills_HaveIconOrRegisteredFallback
For each SkillData where isImplemented == true
Assert skill.icon != null OR RimaUITheme.PassiveIcon(skill.skillName) != null
If not, fail with class/skill id.
```

## PlayMode smoke — tooltip

```txt
Test: SkillCodex_Hover_DoesNotSpawnBrokenTooltip
Open SkillCodex
Hover first implemented skill row
Assert no tooltip object is outside screen bounds
Assert tooltip parent canvas sorting order >= SkillCodex canvas sorting order
Close codex
Assert tooltip hidden
```

## PlayMode / scene validation — walkability

```txt
Test: VisualFloorCells_AreWalkable
Build active RoomTemplateSO through IsoRoomBuilder
For each LastFloorCell not marked obstacle/collision
Assert WalkabilityMap allows player position
```

## PlayMode / render validation — weapon sorting

```txt
Test: Weapon_Renderer_Sorts_With_Player
Given player equipped weapon
Assert weapon renderer is under PlayerRoot or same SortingGroup
Assert weapon sortingLayer == Characters/Entities
Assert weapon sortingOrder >= body renderer order when facing south/east/west according to profile
```

## Manual acceptance checklist

```txt
[ ] MainMenu'den Play -> CharacterSelect/Chamber açılıyor.
[ ] Karakter seçmeden _Arena başlamıyor.
[ ] Eğer Unity Editor doğrudan _Arena'da Play'e basılırsa bu debug mode olarak loglanıyor.
[ ] İki mob öldürülünce oyun kilitlenmiyor.
[ ] Oda clear sonrası reward/exit/portal çıkıyor.
[ ] ESC PauseMenu açıyor, Codex değil.
[ ] PauseMenu'de Resume/New Run/Settings/Skill Codex/Exit seçenekleri var.
[ ] SkillCodex hover'da mavi bozuk yazı çıkmıyor.
[ ] Missing skill iconlar loglanıyor.
[ ] Kılıç cliff altından görünmüyor.
[ ] Görsel floor = walkable floor.
[ ] Oda boyutu combat için rahat.
```
