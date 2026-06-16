# Önerilen uGUI Hiyerarşileri

## HUD
```text
HUDCanvas
  SafeArea
    TopTransient
      RoomTitle
      Objective
    BottomLeftVitality
      ClassEmblem
      HPFrame / HPFill / DamageLagFill
      ResourceFrame / ResourceFill / PerfectPulse
    BottomSkillBar
      PrimarySlots (LMB, RMB)
      SecondarySlots (Q,E,R,F,V)
    TopRightMinimap
    BottomRightCurrency
    WorldPromptLayer
```

## Reward selection
```text
RewardCanvas
  Dimmer
  Header
  CardRow (HorizontalLayoutGroup)
    RewardCard x3
      HeaderSocket
      IconFrame
      Title
      Description
      ComboBox (LayoutElement min width)
      SelectButton
  Footer / Reroll / InputHints
```

## Settings
```text
SettingsCanvas
  Dimmer
  MainPanel
    Header
    Body
      CategoryRail
      ContentViewport / ScrollRect
    FixedFooter
```

## Codex
```text
CodexCanvas
  FullPanel
    Header
    ClassRail
    SkillList
    DetailPanel
      IconAndTitle
      StatsGrid
      Description
      StateBlock
      ComboBlock
      BossRuleBlock
```

## Layout güvenliği

- Aynı RectTransform'da hem ContentSizeFitter hem parent-driven LayoutGroup kullanımı dikkatle sınırlandırılmalı.
- Reward card child'larına `LayoutElement` preferred/min width ver.
- TMP title/body/metadata için ayrı overflow policy.
- Modal açılırken input maps merkezi UI state machine üzerinden değişmeli; her panel bağımsız disable etmemeli.
