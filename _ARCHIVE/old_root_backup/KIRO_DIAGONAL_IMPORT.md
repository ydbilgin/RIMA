# KIRO TASK: Diagonal Sprite Import

**Priority:** ⭐ CRITICAL — blocks 8-directional animation
**Estimated time:** 5 minutes
**Risk:** LOW — no code changes, only asset import settings

---

## Context

4 characters have diagonal base sprites (NE/NW/SE/SW) sitting on disk but NOT imported into Unity.
The cardinal sprites (S/N/E/W) are already imported with correct settings.

## Task

Import all diagonal sprites into Unity with the **exact same settings** as the existing cardinal sprites.

### Step 1: Verify files exist on disk

Check these paths exist:
```
Assets/Sprites/Characters/Warblade/base/warblade_NE.png
Assets/Sprites/Characters/Warblade/base/warblade_NW.png
Assets/Sprites/Characters/Warblade/base/warblade_SE.png
Assets/Sprites/Characters/Warblade/base/warblade_SW.png

Assets/Sprites/Characters/Elementalist/base/elementalist_NE.png
Assets/Sprites/Characters/Elementalist/base/elementalist_NW.png
Assets/Sprites/Characters/Elementalist/base/elementalist_SE.png
Assets/Sprites/Characters/Elementalist/base/elementalist_SW.png

Assets/Sprites/Characters/Ranger/base/ranger_NE.png
Assets/Sprites/Characters/Ranger/base/ranger_NW.png
Assets/Sprites/Characters/Ranger/base/ranger_SE.png
Assets/Sprites/Characters/Ranger/base/ranger_SW.png

Assets/Sprites/Characters/Shadowblade/base/shadowblade_NE.png
Assets/Sprites/Characters/Shadowblade/base/shadowblade_NW.png
Assets/Sprites/Characters/Shadowblade/base/shadowblade_SE.png
Assets/Sprites/Characters/Shadowblade/base/shadowblade_SW.png
```

### Step 2: Force refresh to generate .meta files

Run via Unity MCP execute_code:
```csharp
UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.ForceUpdate);
return "Refresh done";
```

### Step 3: Set import settings on each diagonal sprite

For EACH of the 16 diagonal PNG files, run:
```csharp
var path = "Assets/Sprites/Characters/{Char}/base/{char}_{DIR}.png";
var importer = (UnityEditor.TextureImporter)UnityEditor.AssetImporter.GetAtPath(path);
if (importer != null)
{
    importer.textureType = UnityEditor.TextureImporterType.Sprite;
    importer.spriteImportMode = UnityEditor.SpriteImportMode.Single;
    importer.spritePixelsPerUnit = 64;
    importer.filterMode = UnityEngine.FilterMode.Point;
    importer.textureCompression = UnityEditor.TextureImporterCompression.Uncompressed;
    importer.SaveAndReimport();
}
```

Use batch_execute to do all 16 at once if possible.

### Step 4: Verify

After import, verify all 16 sprites load correctly:
```csharp
var chars = new[] { "Warblade", "Elementalist", "Ranger", "Shadowblade" };
var dirs = new[] { "NE", "NW", "SE", "SW" };
int ok = 0, fail = 0;
foreach (var c in chars)
    foreach (var d in dirs)
    {
        var path = $"Assets/Sprites/Characters/{c}/base/{c.ToLower()}_{d}.png";
        var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(path);
        if (tex != null) ok++; else { fail++; UnityEngine.Debug.LogError($"MISSING: {path}"); }
    }
return $"OK={ok} FAIL={fail}";
```

Expected: `OK=16 FAIL=0`

### Import Settings (must match exactly)
| Setting | Value |
|---------|-------|
| Texture Type | Sprite (2D and UI) |
| Sprite Mode | Single |
| Pixels Per Unit | **64** |
| Filter Mode | **Point (no filter)** |
| Compression | **None** |

## DO NOT
- ❌ Change ANY existing cardinal (S/N/E/W) sprites
- ❌ Move files to different folders
- ❌ Create animation clips (that's a separate task)
- ❌ Modify any .cs scripts

---

## REPORT

```
STATUS: [DONE / FAILED / PARTIAL]
COMPLETED: [list files imported]
ERRORS: [errors] or NONE
NEXT_SIGNAL: "Diagonal import done — Claude devam edebilir"
```
