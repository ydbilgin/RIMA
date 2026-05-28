# S75-D — CharacterClass + MobDefinition SO Scaffold

**Effort:** medium
**Prereq:** S75-C merged

---

## GOAL

10 class + 6 new mob için ScriptableObject data containers. Asset placeholders user PixelLab gen ettiğinde wire-up. Reference: `STAGING/character_idle_LOCK_S74.md`, `STAGING/new_mobs_64px_LOCK_S74.md`, `STAGING/weapons_pixel_sizes_LOCK_S74.md`.

---

## CharacterClassDefinition

**Yeni dosya:** `Assets/Scripts/Data/CharacterClassDefinition.cs`

```csharp
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(menuName = "RIMA/Character Class Definition", fileName = "Class_New")]
    public class CharacterClassDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string className = "Warblade";
        public string anchorName = "warblade";
        [TextArea] public string roleDescription = "Two-handed greatsword fighter, mid-range melee";
        
        [Header("Visual")]
        public Sprite idleSprite;       // 64x64 PixelLab gen
        public Sprite weaponSprite;     // ref weapons_pixel_sizes_LOCK_S74.md
        public Vector2Int weaponCanvas = new Vector2Int(56, 20); // Warblade greatsword
        
        [Header("Weapon Decouple (Karar #123)")]
        public bool weaponDecoupled = true; // false for Brawler (bare fists), Elementalist (VFX only)
        
        [Header("Stats Placeholder")]
        public float maxHp = 100f;
        public float moveSpeed = 5f;
        public float baseAttackDamage = 10f;
        public float baseAttackCooldown = 0.6f;
        
        [Header("Echo Skill Tier 1 (Karar #5/#7)")]
        public string echoTier1Name;
        public string echoTier1Description;
        
        [Header("Identity Body Accessories (passive, Karar #18)")]
        public List<string> passiveAccessories = new List<string>(); // Ronin scabbard, Hexer grimoire
    }
}
```

## MobDefinition

**Yeni dosya:** `Assets/Scripts/Data/MobDefinition.cs`

```csharp
using UnityEngine;

namespace RIMA.Data
{
    public enum MobRole { Swarm, Melee, Ranged, Caster, Elite, MiniBoss, Support, Pack }
    
    [CreateAssetMenu(menuName = "RIMA/Mob Definition", fileName = "Mob_New")]
    public class MobDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string mobName = "Seam Crawler";
        public MobRole role = MobRole.Swarm;
        [TextArea] public string silhouette;
        public string biomeKey = "F1";
        
        [Header("Visual")]
        public Sprite idleSprite;
        public Vector2Int canvasSize = new Vector2Int(64, 64); // 64/80/96
        public string riftPaletteAccent = "cyan #00FFCC + violet #5A2A8A";
        
        [Header("Stats Placeholder")]
        public float maxHp = 30f;
        public float moveSpeed = 3.5f;
        public float damage = 8f;
        public float attackRange = 1.2f;
        public float detectionRange = 6f;
        
        [Header("Behavior")]
        public bool isFlying = false;
        public bool isElite = false;
        public bool hasIntegratedWeapon = false; // Hollow Arbiter fused sword
    }
}
```

---

## ASSET CREATION (Editor tool)

**Yeni dosya:** `Assets/Editor/InitializeClassMobAssets.cs`

Menu: `RIMA > Tools > Initialize Class + Mob Definition Assets`

Davranış:
1. `Assets/Data/Classes/` klasörü oluştur (yoksa)
2. 10 class asset oluştur: Warblade, Ranger, Shadowblade, Elementalist, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer
   - Her birinin canvasSize weapons_pixel_sizes_LOCK_S74.md'den
   - weaponDecoupled flags: Brawler=false, Elementalist=false, others=true
   - passiveAccessories: Ronin → ["scabbard"], Hexer → ["grimoire"]
3. `Assets/Data/Mobs/F1/` klasörü oluştur
4. 6 yeni mob asset oluştur: SeamCrawler, PlateWidow, RelicCaster, RiftHound, HollowArbiter, SpireChoirling
   - canvasSize: 64×64 (SeamCrawler, RelicCaster, SpireChoirling), 80×80 (PlateWidow), 96×96 (RiftHound, HollowArbiter)
   - role: per new_mobs_64px_LOCK_S74.md
   - hasIntegratedWeapon: Hollow Arbiter = true
5. AssetDatabase.SaveAssets + Refresh
6. Debug.Log özet

---

## VALIDATION

1. `dotnet build` PASS
2. Run "Initialize Class + Mob Definition Assets" menu → 16 asset oluşur
3. Project window: Assets/Data/Classes/*.asset (10) + Assets/Data/Mobs/F1/*.asset (6)
4. Inspector açıldığında her field doğru görünür
5. CharacterClass asset Warblade için: weaponCanvas=(56,20), weaponDecoupled=true, anchorName="warblade"
6. Mob asset SeamCrawler için: role=Swarm, canvasSize=(64,64), isFlying=false

---

## COMMIT MESAJI

```
[S75-D] CharacterClass + MobDefinition SO scaffold

- CharacterClassDefinition.cs (10 class data containers)
- MobDefinition.cs (mob data containers, MobRole enum)
- InitializeClassMobAssets editor menu: bulk asset creation
- Assets/Data/Classes/{10 classes}.asset + Assets/Data/Mobs/F1/{6 mobs}.asset
- Pre-filled per LOCK docs (canvas sizes, decouple flags, roles)
- Sprite slots empty (user fills after PixelLab gen)
```
