# C) Unity uGUI Prefab Hiyerarşisi

```txt
Canvas_DirectorOverlay
├── CanvasScaler
│   ├── UI Scale Mode = Scale With Screen Size
│   ├── Reference Resolution = 1920 x 1080
│   └── Match = 0.5
├── GraphicRaycaster
└── DirectorRoot (CanvasGroup)
    ├── ScreenDimmer
    │   └── Image: solid #080710 alpha 0.35
    ├── TopBadge
    │   ├── Image: ribbon_base, Sliced
    │   ├── TMP_Title: DIRECTOR MODE
    │   ├── TMP_Subtitle: FREE-CAM · TIME SCALE 0
    │   └── Button_StartTest
    │       ├── Image: ribbon_base, Sliced
    │       └── TMP: BAŞLAT
    ├── TabRail
    │   ├── RectTransform: left, width 96
    │   ├── VerticalLayoutGroup
    │   ├── Button_Tab_Spawn
    │   │   ├── Image: slot_active / slot_normal
    │   │   ├── Icon
    │   │   └── TMP_Label
    │   ├── Button_Tab_Map
    │   ├── Button_Tab_Build
    │   ├── Button_Tab_ClassSkill
    │   ├── Button_Tab_Stats
    │   └── Button_Tab_Telemetry
    ├── ContentArea
    │   ├── RectTransform: left panel, x 150, y center
    │   ├── Panel_Spawn (CanvasGroup)
    │   │   ├── Window
    │   │   │   └── Image: minimap_frame, Sliced
    │   │   ├── Header
    │   │   │   ├── TMP_Title: SPAWN PALETTE
    │   │   │   └── TMP_Hint: Sol tık koy · Sağ tık sil
    │   │   ├── SubTabs
    │   │   │   ├── Button_Mobs: menu_button
    │   │   │   ├── Button_Boss: menu_button
    │   │   │   └── Button_Wave: menu_button
    │   │   ├── EnemyPaletteGrid
    │   │   │   ├── GridLayoutGroup: cell 96x96, spacing 12
    │   │   │   └── EnemySlot_*: slot_normal + icon + cost
    │   │   ├── WavePresetRow
    │   │   │   └── Button_*: menu_button
    │   │   └── FooterModeText
    │   ├── Panel_Map (CanvasGroup)
    │   │   ├── Window: minimap_frame
    │   │   ├── DungeonNodeGraph
    │   │   │   ├── NodeButton_*: node_frame
    │   │   │   └── ConnectionLines: UILineRenderer veya custom Graphic
    │   │   ├── Button_RerollSeed: menu_button
    │   │   └── Button_JumpToNode: ribbon_base
    │   ├── Panel_Build (CanvasGroup)
    │   │   ├── Window: minimap_frame
    │   │   ├── SubTabs: Tile / Cliff / Prop
    │   │   ├── TilePaletteGrid: slot_normal cells
    │   │   ├── CliffSettingsList
    │   │   ├── PropPoolGrid
    │   │   └── Button_Generate: ribbon_base
    │   ├── Panel_ClassSkill (CanvasGroup)
    │   │   ├── Window: minimap_frame
    │   │   ├── ClassGrid: 10 buttons, slot_normal
    │   │   ├── SkillLoadout
    │   │   │   ├── Slot_LMB: slot_lmb_rmb
    │   │   │   ├── Slot_RMB: slot_lmb_rmb
    │   │   │   └── Slot_1_5: slot_normal
    │   │   └── DraftOverrideCards: reward_card
    │   ├── Panel_Stats (CanvasGroup)
    │   │   ├── Window: minimap_frame
    │   │   ├── ClassSelectorRow
    │   │   ├── Slider_MaxHP
    │   │   ├── Slider_PhysPower
    │   │   ├── Slider_AbilityPower
    │   │   ├── Slider_AttackSpeedMult
    │   │   ├── Slider_MoveSpeed
    │   │   ├── Slider_DebugDamageMult
    │   │   ├── Button_ResetClass
    │   │   ├── Button_SavePreset
    │   │   └── Button_ExportBuild
    │   └── Panel_Telemetry (CanvasGroup)
    │       ├── Window: minimap_frame
    │       ├── DPSGraph
    │       ├── TTKReadout
    │       ├── DamageSourceBreakdown
    │       ├── ResourceGraph
    │       └── Button_ExportCSV: ribbon_base
    ├── WorldCursorOverlay
    │   ├── GhostPreview
    │   │   └── Image/SpriteRenderer bridge: selected enemy/prop/tile ghost
    │   ├── BrushCircle
    │   └── GridCellHighlight
    ├── MinimapMini
    │   ├── Image: minimap_frame, Sliced
    │   └── NodeGraphMini
    ├── SelectionInspector
    │   ├── Image: tooltip_box, Sliced
    │   ├── TMP_Name
    │   ├── TMP_RuntimeStats
    │   └── Buttons: AI Mode / Delete / Duplicate
    └── BottomTelemetryStrip
        ├── Image: menu_button, Sliced stretched
        ├── TMP_DPS
        ├── TMP_TTK
        ├── TMP_Hits
        ├── TMP_Seed
        └── Button_QuickReset: menu_button
```

## Import ayarları
- Sprite Mode: Single
- Mesh Type: Full Rect
- Filter Mode: Point
- Compression: None
- Pixels Per Unit: UI için tutarlı, örn. 100
- `Image Type = Sliced` olanlar: `minimap_frame`, `tooltip_box`, `menu_button`, `ribbon_base`, `reward_card`
- Slot sprite’ları genelde Simple kullanılabilir; ölçek çok değişecekse Sliced denenebilir.

## Runtime panel yönetimi
```csharp
void ShowTab(DirectorTab tab)
{
    foreach (var panel in panels)
    {
        bool active = panel.Tab == tab;
        panel.CanvasGroup.alpha = active ? 1f : 0f;
        panel.CanvasGroup.interactable = active;
        panel.CanvasGroup.blocksRaycasts = active;
    }
}
```

Destroy/Instantiate yok. İnsanların UI performansını baltalamak için yeterince başka yolu var zaten.
