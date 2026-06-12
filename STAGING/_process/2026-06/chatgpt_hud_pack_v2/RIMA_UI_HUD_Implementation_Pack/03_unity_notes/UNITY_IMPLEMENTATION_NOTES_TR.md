# Unity Implementation Notes — RIMA HUD

## Klasör yapısı

```text
Assets/
  Art/
    UI/
      Core/
        UI_Atlas_ObsidianRift.png
        UI_Core_Sprites.asset
      Icons/
        SkillIcons/
        ClassIcons/
        ResourceIcons/
      Fonts/
  Prefabs/
    UI/
      HUD_Gameplay.prefab
      RewardSelectionScreen.prefab
      PauseMenu.prefab
      SkillCodexScreen.prefab
      MinimapPanel.prefab
  Scripts/
    UI/
      HUDController.cs
      RewardCardView.cs
      PauseMenuController.cs
      SkillCodexView.cs
      SkillRowView.cs
      HotbarSlotView.cs
```

## Canvas düzeni

Önerilen hiyerarşi:

```text
Canvas_Root
  SafeArea
    HUD_Persistent
      TopLeft_PlayerPanel
      TopRight_Minimap
      Bottom_Hotbar
      BottomLeft_Hints
      BottomRight_Resources
    ModalLayer
      ScreenDimmer
      RewardSelectionScreen
      PauseMenu
      SkillCodexScreen
    TooltipLayer
```

Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920×1080
- Match: 0.5

## 9-slice kullanımı

Panel ve butonların tamamı tek PNG olarak scale edilmemeli. Yoksa köşeler jelibon gibi uzar, ki bu da görsel tasarımın küçük düşürücü bir ölümü olur.

Unity:
- Sprite Editor > Border gir
- Image Type: Sliced
- `preserveAspect` kapalı olabilir
- Köşeleri 16/24/32 px sabit tut

## Hotbar veri modeli

Skill slotları UI'da hard-code olmasın:

```csharp
public enum InputSlot { LMB, RMB, Q, E, R, F }

[Serializable]
public class HotbarSlotData
{
    public InputSlot input;
    public SkillDefinition skill;
    public Sprite icon;
    public string displayName;
    public int stackCount;
    public float cooldownRemaining;
    public float cooldownTotal;
}
```

## Reward card veri modeli

```csharp
public class RewardOption
{
    public SkillDefinition skill;
    public Rarity rarity;
    public string title;
    public string description;
    public string classTag;
    public string synergyText;
    public Sprite icon;
}
```

Kart prefab sadece veriyi basmalı:
- rarity ribbon rengini set et
- icon bas
- title/desc/synergy bas
- hover/selected state animasyonunu oynat

## Skill Codex row modeli

```csharp
public class SkillCodexRowData
{
    public Sprite icon;
    public string skillName;
    public string description;
    public Rarity rarity;
    public float cooldown;
}
```

Liste için:
- ScrollRect + pooled row prefab kullan
- Row yüksekliği: 64 px veya 72 px
- Sağda rarity + cooldown chip sabit kolon
- Açıklama en fazla 1 satır, tooltipte uzun açıklama

## Modal davranışı

- RewardSelection açılınca gameplay input kilitlenir.
- Pause açılınca timeScale = 0.
- SkillCodex açılınca gameplay arka plan dimlenir ama HUD faint kalabilir.
- ModalLayer sırası:
  1. ScreenDimmer
  2. Aktif modal
  3. Tooltip

## Animasyon önerileri

Abartma. UI Las Vegas tabelasına dönmesin.

- Button hover: 0.08 sn cyan/amber glow
- Card hover: scale 1.02, border glow
- Reward seçilince: seçilen kart cyan pulse, diğerleri dissolve/dim
- Pause açılınca: panel scale 0.96 -> 1.0, alpha 0 -> 1
- Codex row hover: row background alpha + amber left line

## Net yapılacaklar

- Kırmızı debug spawn karelerini gameplay build'de kapat.
- Mevcut küçük yazıları büyüt: özellikle hotbar altındaki control hintleri.
- Skill Codex'teki çok uzun açıklamaları kısalt, tooltip sistemine taşı.
- Top-left HUD'u sadeleştir: önce can/resource, sonra class/room/objective.
- Minimap gerçek bilgiye göre sade kalmalı, UI süsü haritayı boğmamalı.
