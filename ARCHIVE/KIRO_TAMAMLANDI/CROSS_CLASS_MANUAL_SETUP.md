# Cross-Class Infrastructure Setup - Manual Steps Required

## ⚠️ IMPORTANT: Unity Editor Setup Required

The following tasks have been completed programmatically:
- ✅ All 3 C# scripts created
- ✅ CrossClassGhost.prefab created (YAML)
- ✅ All 10 ScriptableObject assets created

**However, Unity MCP tools timed out. You must complete these steps manually in Unity Editor:**

---

## TASK 4 - Verify/Fix CrossClassGhost Prefab

1. Open Unity Editor
2. Navigate to `Assets/Prefabs/VFX/CrossClassGhost.prefab`
3. If the prefab doesn't load correctly (YAML may need Unity refresh):
   - Create new empty GameObject in scene
   - Name it: `CrossClassGhost`
   - Add components:
     - `SpriteRenderer`
     - `CrossClassGhostEffect` script
   - Configure SpriteRenderer:
     - Sorting Layer: `VFX` (create if doesn't exist)
     - Order in Layer: `50`
   - Drag to `Assets/Prefabs/VFX/` to create prefab
   - Delete from scene

---

## TASK 5 - Add CrossClassManager to _IsoGame Scene

1. Open scene: `Assets/Scenes/_IsoGame.unity`
2. Create new empty GameObject (right-click Hierarchy → Create Empty)
3. Name it: `CrossClassManager`
4. Add component: `CrossClassSkillManager`
5. In Inspector, configure:
   - **Ghost Effect Prefab**: Drag `Assets/Prefabs/VFX/CrossClassGhost.prefab`
   - **All Skills**: Expand list, set Size = 10
   - Drag all 10 ScriptableObject assets from `Assets/Data/CrossClass/`:
     - CCS_Warblade_IronFragment
     - CCS_Elem_EmberTouch
     - CCS_Shadow_VoidTrace
     - CCS_Ranger_HuntersMark
     - CCS_Ravager_Bloodfuel
     - CCS_Ronin_FirstBlood
     - CCS_Gunslinger_QuickReload
     - CCS_Brawler_MomentumBurst
     - CCS_Summoner_GravePact
     - CCS_Hexer_HexLeech
6. Save scene (Ctrl+S)

---

## TASK 7 - Console Check

1. Check Unity Console for compilation errors
2. If errors exist:
   - Script GUIDs may need regeneration (Unity will auto-fix on import)
   - Check that `RIMA` namespace exists in other project scripts
3. Enter Play Mode
4. Open Console and run test (if you have a test script):
   ```csharp
   CrossClassSkillManager.Instance.GetDiscoveryOffer()
   ```
   Should return 3 random skills from different classes

---

## Known Issues & Fixes

### If scripts show "missing reference" errors:
- Unity needs to regenerate .meta files
- Solution: Right-click `Assets/Scripts/CrossClass` → Reimport

### If VFX sorting layer doesn't exist:
- Edit → Project Settings → Tags and Layers
- Add "VFX" to Sorting Layers

### If ScriptableObject assets don't show in Inspector:
- The YAML files use placeholder GUIDs
- Unity will regenerate correct GUIDs on first import
- If they still don't work, recreate manually:
  - Right-click in `Assets/Data/CrossClass/`
  - Create → RIMA → CrossClassSkill
  - Fill in values from the table in KIRO_CROSS_CLASS_INFRA.md

---

## After Manual Setup Complete

Run this command to verify all files exist:
```bash
ls -R RIMA/Assets/Scripts/CrossClass RIMA/Assets/Data/CrossClass RIMA/Assets/Prefabs/VFX/CrossClassGhost.prefab
```
