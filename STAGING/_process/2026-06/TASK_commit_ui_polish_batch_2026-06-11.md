# TASK: Commit UI Polish Batch (2026-06-11)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev
Aşağıdaki uncommitted dosyaları tek commit olarak stage et ve commit et.

## Staged Dosyalar
- `Assets/Scripts/UI/SkillBarUI.cs`
- `Assets/Scripts/UI/PauseMenuUI.cs`
- `Assets/Scripts/UI/RimaUITheme.cs`
- `Assets/Scripts/UI/SkillCodexUI.cs`
- `Assets/Scripts/Utils/PlaceholderSprite.cs`
- `Assets/Prefabs/RewardPickup.prefab`
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
- `Assets/RoomPainter/AssetMetadata/9624783efc4afd043a3d32e44d0278b8.asset`

## Commit Mesajı
```
feat(ui): skill bar tooltip + SkillCodex + PauseMenu + reward sorting fix

- SkillBarUI: hover tooltip (TooltipSystem reuse)
- SkillCodexUI + PauseMenuUI: Act1 void-purple/slate/ember palette, procedural 9-slice
- RimaUITheme: Act1 renk sabitleri
- PlaceholderSprite: Awake → OnEnable (domain-reload fix)
- RewardPickup: Entities layer order 5 (floor=0, reward sorting fix)
- RoomRunDirector: ilgili küçük düzeltme
```

## Komut
```powershell
git add Assets/Scripts/UI/SkillBarUI.cs Assets/Scripts/UI/PauseMenuUI.cs Assets/Scripts/UI/RimaUITheme.cs Assets/Scripts/UI/SkillCodexUI.cs Assets/Scripts/Utils/PlaceholderSprite.cs Assets/Prefabs/RewardPickup.prefab Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs "Assets/RoomPainter/AssetMetadata/9624783efc4afd043a3d32e44d0278b8.asset"
git commit -m "feat(ui): skill bar tooltip + SkillCodex + PauseMenu + reward sorting fix

- SkillBarUI: hover tooltip (TooltipSystem reuse)
- SkillCodexUI + PauseMenuUI: Act1 void-purple/slate/ember palette, procedural 9-slice
- RimaUITheme: Act1 renk sabitleri
- PlaceholderSprite: Awake → OnEnable (domain-reload fix)
- RewardPickup: Entities layer order 5 (floor=0, reward sorting fix)
- RoomRunDirector: ilgili küçük düzeltme"
```

## Başarı Kriteri
`git status` temiz (bu dosyalar için). STAGING dosyaları (`??`) ignore edilir.
