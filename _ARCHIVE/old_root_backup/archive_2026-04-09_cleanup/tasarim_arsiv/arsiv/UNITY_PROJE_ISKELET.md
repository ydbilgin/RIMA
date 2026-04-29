# Unity Proje İskeleti
*Son güncelleme: 2026-03-29 | Faz 0*

---

## Kurulum Adımları

### 1. Unity Projesi Oluştur
```
Unity Hub → New Project
Template: Universal 2D (URP 2D hazır gelir)
Project Name: TheConfluence
Location: F:\Antigravity Projeler\2d roguelite\
```
> Not: Proje klasörü `F:\Antigravity Projeler\2d roguelite\TheConfluence\` olacak.

### 2. Klasör İskeleti Kur
Unity projesi oluşturulduktan sonra `_kurulum_klasorler.ps1` scriptini çalıştır:
```
! powershell -ExecutionPolicy Bypass -File "_scriptler/_kurulum_klasorler.ps1"
```

### 3. URP 2D Renderer Ayarla
```
Window → Package Manager → Universal RP → Install
Edit → Project Settings → Graphics → URP Asset seç
URP Asset → Renderer Type: 2D Renderer
HDR: Enabled | Post Processing: Enabled | MSAA: 4x
```

### 4. Lighting Layers Tanımla
```
Edit → Project Settings → 2D → Light Layers
Layer 0: Background
Layer 1: Foreground
Layer 2: Characters
Layer 3: Effects
Layer 4: UI
```

---

## Assets/ Klasör Yapısı

```
Assets/
├── _Project/                    ← Proje genelinde paylaşılan
│   ├── Settings/               ← URP Asset, Input System, Audio Mixer
│   ├── Rendering/              ← URP Renderer Data, Post-process Profiles
│   └── Fonts/
│
├── Scripts/
│   ├── Core/                   ← GameManager, SceneLoader, EventBus
│   ├── Player/                 ← PlayerController, PlayerStats, PlayerAnimator
│   ├── Enemies/                ← EnemyBase, EnemyAI, EnemySpawner
│   ├── Skills/
│   │   ├── Base/               ← SkillData (ScriptableObject), SkillBase, SkillTag enum
│   │   ├── Warblade/           ← IronCharge, CripplingBlow, GravityCleave, vb.
│   │   ├── Elementalist/
│   │   ├── Shadowblade/
│   │   └── Shared/             ← Nötr/cross-class skilleri
│   ├── Systems/
│   │   ├── SkillDraft/         ← DraftManager, OfferGenerator, TierWeights
│   │   ├── Resources/          ← ResourceSystem, RageSystem, FocusSystem, vb.
│   │   ├── Map/                ← MapGenerator, RoomData, RoomConnector
│   │   ├── Loot/               ← LootTable, RewardManager
│   │   ├── Meta/               ← MetaProgressionManager, EchoesManager
│   │   └── StatusEffects/      ← StatusEffectBase, Burn, Slow, Poison, Curse
│   ├── Bosses/
│   │   ├── BossBase/           ← BossBase, PhaseManager, NemesisTracker
│   │   ├── IronWarden/
│   │   ├── VoidWarden/
│   │   ├── ChainWarden/
│   │   ├── FracturedKing/
│   │   ├── HollowSovereign/    ← AnalyzePlayerBuild(), SetBehaviorModifiers()
│   │   └── NexusCore/
│   ├── UI/
│   │   ├── HUD/                ← HealthBar, ResourceBar, SkillSlots, MapFragment
│   │   ├── Menus/              ← MainMenu, PauseMenu, RunOver
│   │   ├── Draft/              ← SkillOfferUI, TierDisplay
│   │   └── Hub/                ← HubUI, MetaProgressUI
│   ├── Hub/                    ← ThresholdManager, Ferryman, Vrel, SisterMourne, Cartographer
│   ├── Localization/           ← LocalizationManager, StringKeyHelper
│   └── Utils/                  ← Extensions, ObjectPool, DOTweenHelper
│
├── ScriptableObjects/
│   ├── Skills/
│   │   ├── Warblade/           ← IronCharge.asset, CripplingBlow.asset, vb.
│   │   ├── Elementalist/
│   │   ├── Shadowblade/
│   │   └── Shared/
│   ├── Classes/                ← ClassData.asset (8 class)
│   ├── Enemies/                ← EnemyData.asset
│   ├── Bosses/                 ← BossData.asset
│   └── Rooms/                  ← RoomData.asset
│
├── Prefabs/
│   ├── Player/                 ← Player.prefab
│   ├── Enemies/
│   ├── Skills/                 ← Projectile, AoE, hit spark, vb.
│   ├── Bosses/
│   ├── UI/
│   ├── Lighting/               ← SkillLight.prefab, TorchLight.prefab
│   ├── Particles/              ← BloodSplash, HitSpark, AmbientDust, vb.
│   └── Rooms/                  ← Room templates
│
├── Sprites/
│   ├── Characters/
│   │   ├── Warblade/           ← idle, run, attack, death (Aseprite export)
│   │   ├── Elementalist/
│   │   ├── Shadowblade/
│   │   └── Enemies/
│   ├── Environment/
│   │   ├── Act1_Ruins/         ← floor, wall, props
│   │   ├── Act2_Wastes/
│   │   ├── Act3_Core/
│   │   └── Hub_Threshold/
│   ├── UI/
│   │   ├── HUD/
│   │   ├── Icons/              ← skill icon'ları
│   │   └── Map/                ← harita parçaları
│   └── VFX/                    ← sprite tabanlı efektler
│
├── Animations/
│   ├── Player/
│   ├── Enemies/
│   └── UI/
│
├── Audio/
│   ├── Music/                  ← per-act BGM
│   └── SFX/
│       ├── Player/
│       ├── Enemies/
│       └── UI/
│
├── Materials/
│   ├── Lit/                    ← URP 2D Lit Sprite materials
│   ├── Unlit/
│   └── PostProcess/            ← per-act Volume profiles
│
├── Shaders/
│   ├── HitFlash.shader
│   ├── Dissolve.shader
│   └── Outline.shader
│
├── Scenes/
│   ├── _Boot.unity             ← ilk yüklenen sahne, GameManager init
│   ├── Hub_Threshold.unity
│   ├── Act1_ShadowedRuins.unity
│   ├── Act2_BleedingWastes.unity
│   ├── Act3_CoreApproach.unity
│   └── _Sandbox.unity          ← test sahnesi
│
└── Localization/               ← Unity Localization Package tabloları
    ├── Tables/
    │   ├── SkillNames_TR.asset
    │   ├── SkillNames_EN.asset
    │   └── UIStrings_TR.asset
    └── Settings/
```

---

## Faz 0 Minimum Hedefi

Faz 0 bitti sayılır:
- [ ] Unity projesi açılıyor, URP 2D Renderer aktif
- [ ] Warblade placeholder sprite sahneye eklendi
- [ ] Klavye ile 8 yönlü hareket çalışıyor
- [ ] 1 skill (Iron Charge) — dash + hasar veriyor
- [ ] 1 düşman — sahada duruyor, hasar alıyor, ölüyor
- [ ] Sahne: `_Sandbox.unity`

---

## Script Konvansiyonları

```csharp
// Dosya adı: PascalCase → PlayerController.cs
// Namespace: TheConfluence.Player, TheConfluence.Skills, vb.
// ScriptableObject dosyaları: [CreateAssetMenu] ile
// Singleton: GameManager, AudioManager — diğerleri singleton değil
// Events: C# events veya UnityEvent — ScriptableObject event channel

// SkillData örneği:
[CreateAssetMenu(fileName = "NewSkill", menuName = "TheConfluence/Skill")]
public class SkillData : ScriptableObject
{
    public string skillNameKey;    // LOC key: "SKILL.WARBLADE.IRON_CHARGE"
    public SkillTag[] tags;
    public SkillTier tier;
    public int resourceCost;
    public bool costsHP;
    public bool isAreaEffect;
}
```

---

## Git .gitignore (Unity için)

```
# Unity
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/
*.pidb
*.unityproj
*.suo
*.user
*.booproj
.DS_Store
*.userprefs
```

Proje kökünde `.gitignore` dosyası oluştur, içine yapıştır.
