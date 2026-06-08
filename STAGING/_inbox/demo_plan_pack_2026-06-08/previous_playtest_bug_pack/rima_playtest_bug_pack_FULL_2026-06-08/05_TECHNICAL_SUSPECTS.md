# Technical Suspects — Claude/Codex Kontrol Listesi

## Canlı sistem yönlendirmesi

Eski RoomLoader / RuntimeRoomManager / GateBehavior / DoorTrigger hattına patch yazma. Güncel canlı yol:

```txt
MainMenu → CharacterSelect / ChamberSelectBootstrap → _Arena
_Arena → RoomRunDirector → IsoRoomBuilder
IsoRoomBuilder.BuildExitDoors → RoomRunExitDoorTrigger
```

Önce bu dosyalar incelenmeli:

```txt
Assets/Scripts/UI/MainMenuController.cs
Assets/Scripts/UI/CharacterSelectScreen.cs
Assets/Scripts/UI/ChamberSelectBootstrap.cs
Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs
Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs
Assets/Scenes/MainMenu.unity
Assets/Scenes/CharacterSelect.unity
Assets/Scenes/_Arena.unity
ProjectSettings/EditorBuildSettings.asset
```

## UI / Input suspect files

Aranacak kelimeler:

```txt
Escape
KeyCode.Escape
Cancel
Pause
SkillCodex
Codex
Yetenek
Tooltip
ShowTooltip
HideTooltip
Cursor.visible
Time.timeScale
SetActive(true)
```

Muhtemel dosyalar:

```txt
Assets/Scripts/UI/UIManager.cs
Assets/Scripts/UI/PauseMenu*.cs
Assets/Scripts/UI/SkillCodex*.cs
Assets/Scripts/UI/Tooltip*.cs
Assets/Scripts/UI/SettingsMenuUI.cs
Assets/Scripts/UI/HUDController.cs
Assets/Scripts/Skills/SkillDatabase.cs
Assets/Scripts/Skills/SkillController.cs
Assets/Scripts/UI/SkillBarUI.cs
```

## Movement / collider suspect files

Aranacak kelimeler:

```txt
walkable
Walkable
bounds
Bounds
Clamp
CompositeCollider2D
TilemapCollider2D
BoxCollider2D
EdgeCollider2D
void
cliff
Block
CanMove
isWalkable
```

Muhtemel dosyalar:

```txt
Assets/Scripts/Player/PlayerController.cs
Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs
Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs
Assets/Data/Rooms/*.asset
Assets/Scenes/_Arena.unity
```

## Weapon / sorting suspect files

Aranacak kelimeler:

```txt
Weapon
WeaponSlot
WeaponMount
WeaponRenderer
SortingGroup
sortingLayer
sortingOrder
SpriteRenderer
Sword
Warblade
```

Muhtemel dosyalar:

```txt
Assets/Scripts/Player/*Weapon*.cs
Assets/Scripts/Weapons/*.cs
Assets/Scripts/Player/PlayerAnimator*.cs
Assets/Scripts/Player/PlayerController.cs
Assets/Prefabs/Player*.prefab
Assets/Prefabs/Weapons*.prefab
```

## Özel debug patch önerileri

### Player block debug

```csharp
private void OnCollisionStay2D(Collision2D collision)
{
    Debug.Log($"[PlayerBlock] {collision.collider.name} layer={LayerMask.LayerToName(collision.collider.gameObject.layer)} pos={transform.position}");
}
```

### Missing icon log

```csharp
if (icon == null)
{
    Debug.LogWarning($"[SkillIcon] Missing icon for skillId={skillId} path={iconPath}");
    icon = fallbackIcon;
}
```

### Codex close hard reset

```csharp
public void CloseCodex()
{
    codexRoot.SetActive(false);
    TooltipManager.Instance?.Hide(true);
    Time.timeScale = 1f;
    InputRouter.SetGameplayEnabled(true);
    UIState.PopModal("SkillCodex");
}
```
