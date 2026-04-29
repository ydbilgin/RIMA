# UNITY BAŞLANGIÇ PLANI — Tam Yol Haritası
*2026-03-27 | Solo dev | FAZ 1'den Steam'e kadar adım adım*

---

## NASIL OKUNMALI

Bu belge iki paralel süreci birlikte yönetiyor:
- **ART SIRASINDA** — Unity açmadan önce Aseprite + PixelLab'da yapabileceğin her şey
- **UNITY SIRASI** — Kodu ne zaman yazacaksın, hangi art neye bağlı

Her adımda **"Bu adıma girebilmek için ne hazır olmalı?"** sorusunun cevabı var.
İkonlar önceden yapılabilir. Bağlı oldukları sistemi yazmadan önce Unity'e import et, klasörüne koy — kullanılmaya hazır beklesin.

---

## ÖNCELİKLİ OKUMA — BAĞIMLILIK HARİTASI

```
ART üretimi → Unity import → ScriptableObject referans → Sistem çalışır

Örnek:
  warblade_charge.png (Aseprite'ta üret)
    → Assets/Sprites/Icons/ klasörüne koy
      → SkillData (ScriptableObject) icon alanına ata
        → RewardPanel'de kartlar ikonuyla gösteriyor ✓
```

**Kural:** Bir sistem kodu yazılmadan önce o sisteme ait ikonlar/sprite'lar hazırda beklemeli.
Yoksa placeholder (beyaz kare) kullanırsın ama görsel sonuçsuz çalışmak motivasyonu düşürür.

---

## AŞAMA 0 — UNITY AÇMADAN ÖNCEKİ ART (paralel yapılabilir)

Bu listedeki her şeyi Unity'den bağımsız yapabilirsin.
Aseprite + PixelLab açık, Unity kapalı.

### 0-A: FAZ 1 İçin Zorunlu Art

**Warblade Karakter Sprite'ları** — 64×64px, Aseprite + PixelLab Animate

| Dosya Adı | Animasyon | Frame |
|-----------|-----------|-------|
| `warblade_front_idle.aseprite` | Idle (nefes alıyor) | 6f @ 6fps |
| `warblade_front_walk.aseprite` | Yürüyüş döngüsü | 8f @ 10fps |
| `warblade_front_attack1.aseprite` | Temel saldırı | 6f @ 12fps |
| `warblade_front_dash.aseprite` | Dash (afterimage) | 4f @ 16fps |
| `warblade_front_hit.aseprite` | Hasar aldı | 3f @ 12fps |
| `warblade_front_death.aseprite` | Ölüm | 8f @ 8fps |
| `warblade_back_idle.aseprite` | Arkaya idle | 6f @ 6fps |
| `warblade_back_walk.aseprite` | Arkaya yürüyüş | 8f @ 10fps |
| `warblade_side_idle.aseprite` | Yana idle (flip ile sağ) | 6f @ 6fps |
| `warblade_side_walk.aseprite` | Yana yürüyüş | 8f @ 10fps |
| `warblade_side_attack1.aseprite` | Yana saldırı | 6f @ 12fps |

> PixelLab workflow: Aseprite'ta front pose çiz → PixelLab Animate → Humanoid skeleton → animasyon üret → Export Sprite Sheet

**Grunt Düşman Sprite'ları** — 32×32px

| Dosya Adı | Animasyon | Frame |
|-----------|-----------|-------|
| `grunt_front_idle.aseprite` | Idle | 4f @ 6fps |
| `grunt_front_walk.aseprite` | Yürüyüş | 6f @ 8fps |
| `grunt_front_attack.aseprite` | Saldırı | 5f @ 12fps |
| `grunt_front_hit.aseprite` | Hasar | 3f @ 12fps |
| `grunt_front_death.aseprite` | Ölüm | 6f @ 8fps |
| `grunt_side_idle.aseprite` | Yana idle | 4f @ 6fps |
| `grunt_side_walk.aseprite` | Yana yürüyüş | 6f @ 8fps |

**Zemin ve Duvar Tile'ları** — 16×16px

| Dosya Adı | İçerik |
|-----------|--------|
| `tileset_dungeon_floor.png` | 4-6 varyant zemin tile (Random Tile için) |
| `tileset_dungeon_wall_h.png` | Yatay duvar |
| `tileset_dungeon_wall_v.png` | Dikey duvar |
| `tileset_dungeon_corner_tl.png` | Köşeler (4 adet) |
| `tileset_dungeon_wall_cap_top.png` | Duvar üst kapağı (derinlik için) |

**Proplar** — 16×16 veya 32×32px

| Dosya Adı | Boyut |
|-----------|-------|
| `prop_torch_wall.aseprite` | 16×32px, 4f tutuşma animasyonu |
| `prop_door_closed.png` | 16×32px |
| `prop_door_open.png` | 16×32px |

**VFX Spritesheet'ler** — 32×32px veya 64×64px

| Dosya Adı | İçerik |
|-----------|--------|
| `vfx_hit_sparks.png` | Beyaz vuruş kıvılcımı, 6f döngüsüz |
| `vfx_slash_white.png` | Saldırı yay efekti, 4f |
| `vfx_blood_splatter.png` | Küçük kan sıçraması, 5f |
| `vfx_dust_dash.png` | Dash tozu, 4f |

---

### 0-B: FAZ 1 İçin UI Art

| Dosya Adı | Boyut | İçerik |
|-----------|-------|--------|
| `ui_hp_bar_frame.png` | 200×24px | HP çerçevesi (9-slice için tasarla) |
| `ui_hp_bar_fill.png` | 196×20px | HP dolgu rengi |
| `ui_rage_bar_frame.png` | 200×20px | Rage çerçevesi |
| `ui_rage_bar_fill.png` | 196×16px | Rage dolgu (#ffe566) |
| `ui_skill_slot_empty.png` | 48×48px | Boş skill slot çerçevesi |
| `ui_skill_slot_filled.png` | 48×48px | Dolu slot çerçevesi (altın kenarlık) |
| `ui_skill_slot_cooldown.png` | 48×48px | CD overlay (koyu alpha) |

**FAZ 1 İçin 1 İkon (oynanabilir minimum):**

| Dosya Adı | Boyut | Skill |
|-----------|-------|-------|
| `icon_warblade_charge.png` | 32×32px | Warblade — Charge |
| `icon_warblade_mortal_strike.png` | 32×32px | Warblade — Mortal Strike |
| `icon_warblade_whirlwind.png` | 32×32px | Warblade — Whirlwind |
| `icon_warblade_colossus_smash.png` | 32×32px | Warblade — Colossus Smash |

> Promptlar: `ART/IKON_VE_UI_PROMPTLARI.md` → Warblade bölümü

---

### 0-C: Önceden Yapılabilir Ama Acil Olmayan Art (FAZ 2+ için bekletebilir)

Bu listeyi **0-A ve 0-B bitince** al:

```
FAZ 2 art:
  □ Tüm Warblade aktif ikonları (8 adet)
  □ Tüm Warblade pasif ikonları (4 adet)
  □ Her 2 ultimate ikonu
  □ Sınıf seçim portrait'leri (8 × 64×64px)
  □ Skill kart UI çerçevesi (Normal / Rare / Ultimate kalite)
  □ Sistem ikonları: Soul Dust, Boss Soul

FAZ 3 art:
  □ Elite düşman sprite'ı (64×64px)
  □ 4 Grudge Badge ikonu (🔥❄⚡☠)
  □ Boss sprite'ı (128×128px)
  □ Harita node ikonları (6 adet)

FAZ 4 art:
  □ 7 sınıf daha karakter sprite'ları
  □ Tüm skill ikonları (8 sınıf × 14 = 112 ikon)
  □ 14 Neutral pasif ikonu
  □ 28 Cross-class pasif ikonu
```

---

## AŞAMA 1 — UNITY PROJE KURULUMU

### 1-1: Proje Oluştur

```
Unity Hub → New Project
  Template: 2D (URP)   ← "Universal 2D" şablonu seç
  Project Name: 2DRoguelite
  Location: F:\Antigravity Projeler\2d roguelite\Unity\
  Editor Version: Unity 6.3 LTS
  → Create Project
```

> Bu template otomatik olarak URP pipeline'ı, 2D Renderer, URP settings'i kurar.
> 2D URP + normal map + bloom zaten hazır gelir.

---

### 1-2: Paket Kurulumları

```
Window → Package Manager

Şunları kur (sırayla):
  1. Input System                → arama: "Input System"
     ⚠ Unity sorar: "Enable new Input System?" → YES (editör yeniden başlar)

  2. Cinemachine                 → arama: "Cinemachine"

  3. 2D Tilemap Extras           → arama: "2D Tilemap Extras"
     (Random Tile, Rule Tile için gerekli)

  4. TextMeshPro                 → arama: "TextMeshPro"
     → "Import TMP Essential Resources" yap (prompt gelir)
```

> Ek kütüphane (paket dışı — Assets/Plugins'e elle koy):
> - **A* Pathfinding Project** (AstarPathfindingProject.unitypackage)
>   İndir: arongranberg.com/astar/download → Free version yeterli
>   FAZ 3'te lazım ama şimdi kurabilirsin

---

### 1-3: Klasör Yapısı (Assets içinde)

Unity'de Project penceresi açık → Assets klasörü içinde bu yapıyı oluştur:

```
Assets/
├── _Scenes/
│   ├── MainMenu.unity
│   ├── Hub.unity
│   └── Dungeon.unity          ← FAZ 1'de burada çalışacaksın
│
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerCombat.cs
│   │   └── PlayerSkillManager.cs
│   ├── Enemy/
│   │   ├── EnemyAI.cs
│   │   └── EnemyHealth.cs
│   ├── Combat/
│   │   ├── CombatSystem.cs
│   │   ├── Hitbox.cs
│   │   └── DamageNumber.cs
│   ├── Skills/
│   │   ├── SkillBase.cs
│   │   ├── SkillData.cs       ← ScriptableObject
│   │   └── Warblade/
│   │       ├── Skill_Charge.cs
│   │       ├── Skill_MortalStrike.cs
│   │       └── Skill_Whirlwind.cs
│   ├── Systems/
│   │   ├── RageSystem.cs
│   │   ├── RoomManager.cs
│   │   ├── RunData.cs
│   │   └── GameManager.cs
│   └── UI/
│       ├── HUDController.cs
│       ├── SkillSlotUI.cs
│       └── RewardPanel.cs
│
├── Sprites/
│   ├── Characters/
│   │   ├── Warblade/
│   │   │   ├── warblade_front_idle.png     ← buraya koy
│   │   │   ├── warblade_front_walk.png
│   │   │   └── ...
│   │   └── Enemies/
│   │       └── Grunt/
│   ├── Tilesets/
│   │   └── Dungeon/
│   ├── Icons/
│   │   ├── Skills/
│   │   │   └── Warblade/
│   │   │       ├── icon_warblade_charge.png  ← buraya koy
│   │   │       └── ...
│   │   ├── System/
│   │   │   ├── soul_dust.png
│   │   │   ├── grudge_fire.png
│   │   │   └── ...
│   │   └── Neutral/
│   │       └── ...
│   ├── VFX/
│   │   ├── vfx_hit_sparks.png
│   │   └── ...
│   └── UI/
│       ├── ui_hp_bar_frame.png
│       └── ...
│
├── Animations/
│   ├── Player/
│   │   └── Warblade/
│   └── Enemy/
│       └── Grunt/
│
├── Prefabs/
│   ├── Player/
│   ├── Enemies/
│   ├── Projectiles/
│   ├── VFX/
│   └── UI/
│
├── ScriptableObjects/
│   ├── Skills/
│   │   └── Warblade/
│   │       ├── SO_Charge.asset
│   │       └── ...
│   ├── Enemies/
│   └── Rooms/
│
├── Tilemaps/
│   └── Dungeon/
│
├── Audio/
│   ├── SFX/
│   └── Music/
│
└── Settings/
    ├── UniversalRenderPipeline-2DRenderer.asset
    └── InputActions.inputactions
```

---

### 1-4: Layer Ayarları (Edit → Project Settings → Tags and Layers)

```
Layer 8:  Player
Layer 9:  PlayerHitbox
Layer 10: Enemy
Layer 11: EnemyHitbox
Layer 12: Projectile
Layer 13: Environment
Layer 14: Interactable
Layer 15: TriggerOnly
```

**Physics 2D Matrix (Edit → Project Settings → Physics 2D → Layer Collision Matrix):**
```
Player      ↔ Enemy:          ✓ (çarpışır)
Player      ↔ EnemyHitbox:   ✓ (hasar alır)
PlayerHitbox ↔ Enemy:        ✓ (hasar verir)
PlayerHitbox ↔ EnemyHitbox:  ✗ (hayır — ikisi birbiriyle çarpışmaz)
Projectile  ↔ Enemy:         ✓
Projectile  ↔ Player:        ✓
```

---

### 1-5: URP 2D Lighting Ayarları

```
Project Settings → Graphics → URP Asset:
  Anti Aliasing: FXAA
  Bloom: aktif (intensity: 0.3, threshold: 1.1)
  Post Processing: Volume Override ile yönetilecek

2D Renderer Asset:
  HDR Emulation Scale: 1.0
  Transparent Layer Mask: tüm layer'lar hariç Environment zemin
```

**Scene'e Global Light ekle:**
```
Hierarchy → + → Light → Global Light 2D
  Intensity: 0.3   ← karanlık ortam
  Color: #1a1a2e   ← soğuk mavi-siyah

Sprite'lara normal map için:
  Sprite Renderer → Material → Sprite-Lit-Default
  (bu material normal map okur — ışık dinamik çalışır)
```

---

### 1-6: Input Actions Kurulumu

```
Assets/Settings/ → sağ tık → Create → Input Actions
  İsim: InputActions.inputactions

Action Maps → + → "Player" ekle

Actions:
  Move        → Value → Vector2 → WASD + Left Stick
  Dash        → Button → Left Shift + gamepad South
  Skill_Q     → Button → Q
  Skill_W     → Button → W
  Skill_E     → Button → E
  Skill_R     → Button → R
  Skill_F     → Button → F
  Skill_Space → Button → Space
  Interact    → Button → F / gamepad West

Asset Inspector'da:
  ✓ Generate C# Class → "InputActions.cs" adıyla
  → Save Asset
```

---

## AŞAMA 2 — FAZ 1: CORE LOOP

> **Ön koşul — bu art hazır olmalı:**
> - `warblade_front_idle.png` (en azından)
> - `grunt_front_idle.png`
> - `tileset_dungeon_floor.png`
> - `ui_hp_bar_frame.png` + `ui_rage_bar_frame.png`
> - 4 Warblade skill ikonu

---

### 2-1: Sprite Import Ayarları

Her sprite import edilince şu ayarları yap:

```
Texture Type: Sprite (2D and UI)
Pixels Per Unit: 16   ← tile ile uyumlu (16px tile = 1 Unity unit)
Filter Mode: Point (no filter)   ← pixel art için şart
Compression: None   ← kalite kaybetme
Max Size: 512 veya 1024
→ Apply
```

> Sprite Atlas kullan (Window → 2D → Sprite Atlas):
> Tüm UI sprite'larını bir atlasa, tüm karakter sprite'larını ayrı atlasa koy.
> Performans: draw call azalır.

**Tileset için:**
```
Texture Type: Sprite (2D and UI)
Sprite Mode: Multiple → Sprite Editor → Slice → By Cell Size → 16×16
Filter Mode: Point
→ Apply
```

---

### 2-2: Dungeon Scene Kurulumu

```
_Scenes/Dungeon.unity → aç

Hierarchy yapısı:
  ─ Main Camera
  │    └── Cinemachine Brain (component)
  ─ Virtual Camera (Cinemachine)
  │    └── CinemachineVirtualCamera → Follow: [Player]
  │    └── Confiner2D → [RoomBounds polygon]
  ─ Lighting
  │    └── Global Light 2D (intensity 0.3)
  │    └── Point Light 2D (meşale başlarına — prop_torch_wall etrafına)
  ─ Tilemaps
  │    └── Floor Tilemap (sorting: 0)
  │    └── Walls Tilemap (sorting: 1) + Composite Collider 2D
  │    └── Props Tilemap (sorting: 2)
  ─ Player (prefab buraya)
  ─ Enemies (boş parent, runtime spawn edilecek)
  ─ Managers
  │    └── GameManager
  │    └── RoomManager
  │    └── AudioManager
  └── UI (Canvas)
       └── HUD
```

**Tilemaps + Rule Tile kurulumu:**
```
1. Assets/Tilemaps/Dungeon/ → sağ tık → Create → 2D → Rule Tile
   İsim: RuleTile_DungeonWall

2. Rule Tile Inspector → + → kural ekle
   Komşuya göre duvar yönü otomatik seçiyor
   (North, South, East, West komşu kontrolü)

3. Tile Palette'i aç: Window → 2D → Tile Palette
   + Create New Palette → "DungeonPalette"
   Rule Tile'ı palette'e sürükle

4. Scene'de Walls Tilemap seç → Palette'ten çiz
```

---

### 2-3: PlayerController.cs

Dungeon scene'de:
```
Hierarchy → + → Sprite → Player

Inspector:
  - Sprite Renderer → Sprite: warblade_front_idle[0]
  - Rigidbody 2D → Body Type: Dynamic, Freeze Rotation Z: ✓
  - Capsule Collider 2D → boyutu ayarla (ayak seviyesi)
  - PlayerController.cs component ekle
  - PlayerCombat.cs component ekle
  - Animator component ekle → AnimatorController ata
```

**AnimatorController kurulumu:**
```
Assets/Animations/Player/Warblade/ → sağ tık → Create → Animator Controller
İsim: AC_Warblade

Animator penceresi:
  Parameters:
    Bool: IsMoving
    Float: MoveX, MoveY
    Trigger: Attack, Dash, Hit, Death

Blend Tree (hareket için):
  New State → Motion: Blend Tree
  Blend Type: 2D Freeform Cartesian
  Parameters: MoveX, MoveY
  Motions:
    (0, 1)  → warblade_front_walk
    (0, -1) → warblade_back_walk
    (1, 0)  → warblade_side_walk (flipX = false)
    (-1, 0) → warblade_side_walk (flipX = true)
    (0, 0)  → idle blend tree (kendi içinde yön)

Transitions:
  Idle ↔ Walk: IsMoving = true/false (HasExitTime: ✗, Transition Duration: 0)
  Any → Hit: Hit trigger (HasExitTime: ✗)
  Any → Death: Death trigger
```

---

### 2-4: Combat, Hitbox, Hurtbox

```
Player Hierarchy altına:
  ─ Player
    ├── Hitbox (boş GameObject)
    │     ├── Collider2D (Capsule veya Box, IsTrigger: ✓)
    │     ├── Layer: PlayerHitbox
    │     └── Hitbox.cs component
    └── Hurtbox (boş GameObject)
          ├── Collider2D
          ├── Layer: Player
          └── Hurtbox.cs component
```

**DamageNumber prefab:**
```
Küçük TextMeshPro → beyaz veya sarı renk → yukarı kayarak soluklaşıyor
Assets/Prefabs/VFX/ → DamageNumber.prefab
Animasyonu: DOTween veya basit coroutine (transform.position += up * speed)
```

---

### 2-5: RageSystem.cs

```
Player'a component olarak eklenir.
Değerler:
  [SerializeField] float maxRage = 100f
  float currentRage = 0f

  OnHitDealt    → currentRage += 5f
  OnHitReceived → currentRage += 12f
  OnKill        → currentRage += 10f
  PassiveDecay  → currentRage -= 2f/sn (Update, sadece boşta)

  currentRage >= 100f → event: OnRageFull (UI parlamayı tetikler)
  V tuşu + currentRage == 100 → Rage Ability aktive

RageBarUI.cs:
  Image (type: Filled) → fillAmount = currentRage / maxRage
  Lerp: anlık değil, smooth fillAmount geçiş (0.3s)
  currentRage >= 100 → color = #ffe566 + Bloom glow (2D Light point light)
```

---

### 2-6: EnemyAI.cs — Grunt

```
Grunt GameObject:
  - Sprite Renderer → grunt_front_idle[0]
  - Rigidbody 2D (Dynamic, Freeze Z)
  - Capsule Collider 2D
  - EnemyAI.cs
  - EnemyHealth.cs
  - Hurtbox (EnemyHitbox layer)
  - HP bar (küçük Canvas → World Space)

State machine:
  enum EnemyState { Idle, Chase, AttackWindup, Attack, Cooldown, Dead }

  Idle → Chase: oyuncu detection range içinde (Physics2D.OverlapCircle radius 8f)
  Chase: Vector2.MoveTowards → oyuncuya
  AttackWindup (0.3s): telegraph sprite göster (zemin kırmızı daire, SpriteRenderer)
  Attack (0.1s): hitbox aktif → DealDamage çağır
  Cooldown (1.5s): bekle → Chase'e dön
  Dead: animator Death → 1.5s → Destroy

Knockback:
  void TakeKnockback(Vector2 dir, float force) {
    rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
  }
```

---

### 2-7: Warblade Skill'leri — FAZ 1 için 4 Skill

**Skill sistemi genel yapı:**

```csharp
// SkillData.cs — ScriptableObject
[CreateAssetMenu(menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject {
    public string skillName;
    public Sprite icon;
    public float cooldown;
    public string description;
    public ClassType classOwner;
    public KeyCode defaultKey;
}
```

**Assets/ScriptableObjects/Skills/Warblade/ klasörü:**
```
sağ tık → Create → Skills → SkillData
  → SO_Charge.asset
    skillName: "Charge"
    icon: icon_warblade_charge (import ettiğin PNG)
    cooldown: 8
    classOwner: Warblade

  → SO_MortalStrike.asset
  → SO_Whirlwind.asset
  → SO_ColossusSmash.asset
```

**Her skill için ayrı class:**

```csharp
// Skill_Charge.cs
public class Skill_Charge : SkillBase {
    public float dashForce = 30f;
    public float stunDuration = 1.5f;

    public override void Execute(PlayerController player, Vector2 direction) {
        // 1. Hedefe yön bul
        // 2. Rigidbody2D.AddForce(dir * dashForce, Impulse)
        // 3. OnTriggerEnter2D → hedef varsa: EnemyAI.ApplyStun(stunDuration)
        // 4. Rage += 20
        // 5. Animasyon: player.animator.SetTrigger("Dash")
    }
}
```

---

### 2-8: RoomManager.cs — Wave Sistemi

```
WaveData (ScriptableObject):
  EnemySpawnEntry[] enemies:
    - EnemyPrefab: Grunt.prefab
    - Count: 4
    - SpawnDelay: 0.5s aralar

RoomManager.cs:
  List<Transform> spawnPoints    ← Hierarchy'de boş GameObject'ler
  WaveData[] waves
  int currentWave = 0

  StartRoom() → SpawnWave(0)
  SpawnWave(i) → enemyleri spawn et, enemyAlive sayacını artır
  OnEnemyDead() → enemyAlive-- → if (0) → OnWaveClear()
  OnWaveClear() → currentWave++ → if (son wave) → OpenExit()
  OpenExit() → kapı animasyonu + RewardManager.ShowReward()
```

**Exit kapısı:**
```
prop_door_closed.png ve prop_door_open.png → Animator ile geçiş
DoorTrigger: OnTriggerEnter2D → SceneManager.LoadScene("Dungeon") (aynı sahne yeniden yükleme, farklı seed)
```

---

### 2-9: HUD — Minimal FAZ 1 UI

```
Canvas (Screen Space — Overlay)
  ├── HP Bar
  │     ├── Frame Image (ui_hp_bar_frame)
  │     └── Fill Image (ui_hp_bar_fill) → Image type: Filled, Horizontal
  ├── Rage Bar
  │     ├── Frame Image (ui_rage_bar_frame)
  │     └── Fill Image (ui_rage_bar_fill)
  └── Skill Bar (yatay, sol alt)
        ├── Slot_Q (ui_skill_slot_empty → ikon gelince filled slot)
        ├── Slot_W
        ├── Slot_E
        └── Slot_R
              └── Her slot:
                    ├── Background Image (skill slot çerçeve)
                    ├── Icon Image (skill ikonu)
                    ├── Cooldown Overlay (Image type: Filled, Radial 360, counter-clockwise)
                    └── KeyLabel (TextMeshPro — "Q", "W" vs)
```

**SkillSlotUI.cs:**
```csharp
public void SetSkill(SkillData data) {
    iconImage.sprite = data.icon;
    iconImage.enabled = true;
    emptyLabel.enabled = false;
}

void Update() {
    float cd = player.GetSkillCooldown(slotIndex);
    float maxCd = player.GetSkillMaxCooldown(slotIndex);
    cdOverlay.fillAmount = cd / maxCd;  // 0 = hazır, 1 = tam CD
}
```

---

### 2-10: FAZ 1 Test Kriterleri — Geçmeden Sonraki FAZ'a Gitme

```
✓ Warblade 8 yönde hareket ediyor
✓ Dash var, dash sırasında kısa hasar azaltma var
✓ Q/W/E/R ile 4 skill çalışıyor
✓ Rage bar dolup boşalıyor
✓ V tuşu ile Rage ability (Blood Tide) çalışıyor
✓ Grunt görünce yaklaşıyor, saldırıyor, öldürülebiliyor
✓ Saldırı öncesi kırmızı telegraph dairesi 0.3s görünüyor
✓ Hit-stop: her vuruda 2-3 frame time scale 0.05'e iniyor
✓ Screen shake: çok küçük, Cinemachine noise ile
✓ Düşman ölünce kapı açılıyor
✓ Oda yeniden başlayınca düşmanlar yeniden spawn oldu
✓ HP 0 → death animasyonu → sahne yeniden başlıyor
✓ Normal map'ler aktif, meşale Point Light 2D çalışıyor
```

---

## AŞAMA 3 — FAZ 2: SİSTEMLER

> **Ön koşul — bu art hazır olmalı:**
> - Tüm Warblade ikonları (8 aktif + 4 pasif + 2 ultimate)
> - Skill kart UI çerçevesi (3 kalite tipi)
> - Sınıf seçim ekranı için 8 sınıf portrait (64×64px)
> - Soul Dust ikonu

---

### 3-1: SkillDatabase ve ScriptableObject Sistemi

```
Tüm 8 Warblade skill için SO_*.asset oluştur (Assets/ScriptableObjects/Skills/Warblade/)
Her birini doldurun: isim, ikon, CD, açıklama, upgrades listesi

SkillDatabase.cs (singleton, DontDestroyOnLoad):
  List<SkillData> allSkills
  GetSkillsByClass(ClassType c) → filtrelenmiş liste
  GetRandomOffer(ClassType primary, ClassType secondary, List<SkillData> owned) → 3 kart
```

---

### 3-2: Sınıf Seçim Ekranı

```
MainMenu → Play → ClassSelectScene.unity

ClassSelectScreen.cs:
  8 class icon button grid
  1. tıklama → primary seçildi (outline yeşil)
  2. tıklama → secondary seçildi (outline turuncu)
  Aynı sınıf seçilemez (kontrol)
  Preview panel (sağ taraf): seçilen 2 sınıfın imza skill'leri listesi
  Kombo ismi: CLAUDE_SENTEZ.md'den alınır, TextMeshPro ile gösterilir
  "Başla" → RunData'yı doldur → Dungeon sahnesine geç
```

---

### 3-3: Oda Sonu Reward Paneli

```
RewardPanel.cs (UI Canvas → Panel, varsayılan kapalı)
  3 SkillCard prefab'ı yan yana gösterir

  SkillCard prefab:
    - Arkaplan (kalite rengine göre: altın/mor/turuncu çerçeve)
    - Skill ikonu (64×64)
    - Skill adı (TextMeshPro)
    - Açıklama (TextMeshPro, küçük)
    - "Al" butonu
    - "Geç" butonu (sadece 1 kez geç hakkı)

  Kart seçilince:
    PlayerSkillManager.AddSkill(selectedSkill)
    RewardPanel.Close()
    Time.timeScale = 1 (panel açıkken 0 yapılmıştı)
```

---

### 3-4: Dual Class Sistemi

```
RunData.cs (ScriptableObject veya singleton class):
  ClassType primaryClass
  ClassType secondaryClass
  List<SkillData> availablePool   ← primary + secondary skills birleştirilmiş

PlayerSkillManager.cs:
  List<SkillData> activeSkills     (max 6)
  List<SkillData> passiveSkills    (max 2)
  SkillData ultimateSkill

  void AddSkill(SkillData skill):
    if (activeSkills.Count < 6) → ekle
    else → "Bir skill'i bırak" paneli aç (SlotSwapPanel)

  void ExecuteSkill(int slotIndex):
    if (cooldowns[slotIndex] <= 0) → activeSkills[slotIndex].Execute()
```

---

### 3-5: FAZ 2 Test Kriterleri

```
✓ Sınıf seçim ekranı çalışıyor (2 sınıf seçilebiliyor)
✓ Kombo ismi ekranda görünüyor
✓ Oda sonrası 3 kart geliyor (birleşik class pool)
✓ Skill slot'lara ekleniyor ve çalışıyor
✓ 6 slot dolu iken yeni skill gelince swap paneli açılıyor
✓ Pasif oda: 3 pasif seçenek, 1 aktif oluyor
✓ Skill kart kalite renkleri doğru (normal/rare/ultimate)
✓ Cross-class pasif: iki sınıf seçilince otomatik aktif (slot kullanmıyor)
```

---

## AŞAMA 4 — FAZ 3: DÜŞMAN + ODA YAPISI

> **Ön koşul — bu art hazır olmalı:**
> - Elite düşman sprite'ı (64×64px)
> - 4 Grudge Badge ikonu
> - Boss sprite'ı taslak (128×128px, animasyonsuz olabilir)
> - Harita node ikonları (6 adet)
> - Boss Soul ikonu

---

### 4-1: Grudge Sistemi

**Tasarım özeti:** Elite düşman öldürülünce ne ile öldürüldüğü kaydedilir. Sonraki karşılaşmada o hasara +%35 direnç geliyor. Grudge Badge haritada görünür.

```csharp
// GrudgeData.cs — ScriptableObject
public class GrudgeData : ScriptableObject {
    public string enemyID;          // "PlaguenKnight_01"
    public DamageType killedBy;     // Fire, Frost, Lightning, Physical, Poison
    public int encounterCount;
    public List<DamageType> resistances;  // her kill sonrası resistance eklenir
}

// GrudgeManager.cs — singleton
Dictionary<string, GrudgeData> grudgeRegistry;

void OnEnemyKilled(string enemyID, DamageType damageType):
    if (grudgeRegistry.ContainsKey(enemyID)):
        grudgeRegistry[enemyID].resistances.Add(damageType)
        grudgeRegistry[enemyID].encounterCount++
    else:
        yeni GrudgeData oluştur → kaydet

float GetResistance(string enemyID, DamageType type):
    return grudgeRegistry[enemyID].resistances.Contains(type) ? 0.35f : 0f
```

**Elite entry ekranı (Geri Dön YOK):**
```
EliteEntryPanel (UI Canvas):
  - Düşman ismi (TextMeshPro, büyük)
  - Grudge Badge ikonu (varsa)
  - Hafıza notu: "Act 1'de buz ile öldürüldü" (string)
  - Ödül önizleme: "Soul Dust +10 + garantili upgrade"
  - [GİRİŞ] butonu (tek seçenek)

Kapıya yaklaşınca panel açılır → GİRİŞ'e basınca kapanır + oda başlar
```

---

### 4-2: Harita Sistemi

**Harita veri yapısı:**

```csharp
// MapNode.cs
public class MapNode {
    public RoomType type;     // Normal, Elite, Shop, Boss, Flux, Secret
    public string enemyID;    // Elite için: hangi düşman
    public bool isVisited;
    public bool isAccessible;
    public List<MapNode> connectedNodes;  // bağlantılı sonraki nodlar
}

// MapGenerator.cs
// Act başında garantili havuz + random sıra + pity sistemi
// Harita_SISTEMI.md → Bölüm 4 kurallarına göre
```

**Harita UI:**
```
MapScreen.cs (Act başında açılıyor, oda arası kapalı)
  Düğüm grid (node'lar arası çizgi)
  Her node: harita ikonunu gösteriyor (Normal/Elite/Shop vs)
  Elite node'da: Grudge Badge ikonu
  Mevcut pozisyon: parlıyor
  Erişilebilir nodlar: tıklanabilir (beyaz outline)
  Ziyaret edilmiş: koyu, soluk
  Erişilemeyen: gri, tıklanamaz
```

---

### 4-3: Boss AI — Taslak

```csharp
// BossAI.cs — FAZ 3 minimal versiyon
public enum BossPhase { Phase1, Phase2 }
BossPhase currentPhase;

// Phase 1: HP %100-50
//   Pattern 1: arena boyunca hücum
//   Pattern 2: 3 grunt spawn
//   Pattern 3: AoE saldırı (0.5s telegraph)

// Phase 2: HP %50-0 (savaş boyunca öğrendiklerini kullanıyor)
//   Oyuncunun en çok kullandığı skill türüne karşı direnç: +%20
//   Hız artışı: %30
//   Yeni pattern: oyuncuya doğru yönelen AoE dalga

// Boss death → BossSoulPanel aç
```

**Boss Soul Paneli:**
```
BossSoulPanel.cs:
  Aktif skill'leri listele (ikon + isim)
  "Hangi skill mutasyona uğrasın?" → tıklanınca
  SkillMutation.Mutate(selectedSkill) → skill upgrade edilmiş versiyon olur
  (Mutation listesi her skill için SINIF_SKILL_HAVUZU'nda tanımlı)
```

---

### 4-4: Shop Sistemi

```csharp
// ShopManager.cs
void GenerateShop():
    // Blind offer: kategori göster (Skill / Passive / Upgrade)
    // Revealed offer: 3 item göster, 3x fiyat

ShopItem:
    ItemType type
    SkillData skill (null ise passive)
    int blindPrice    // 1x
    int revealPrice   // 3x

UI:
  Sol panel: Kör al seçenekleri (3 adet, sadece kategori görünür)
  Sağ panel: Seç al (3 item tam açık, 3x fiyat)
  Soul Dust sayacı: üst köşede
  "Satın Al" → Soul Dust düş → item ver
```

---

### 4-5: FAZ 3 Test Kriterleri

```
✓ Harita açılıyor, node'lar görünüyor, yol seçilebiliyor
✓ Elite odaya girince entry paneli çıkıyor (geri dön yok)
✓ Grudge Badge haritada ve entry panelde görünüyor
✓ Öldürme tipi kaydediliyor, sonraki karşılaşmada direnç var
✓ Shop açılıyor: kör al ve seç al çalışıyor
✓ Soul Dust kazanılıp harcanıyor
✓ Boss spawn oluyor, 2 phase var
✓ Boss öldürünce Boss Soul paneli açılıyor, skill mutasyona uğruyor
✓ 3 Act tamamlanınca "run sonu" ekranı açılıyor
```

---

## AŞAMA 5 — FAZ 4: TÜM 8 SINIF

> **Ön koşul — bu art hazır olmalı:**
> - 7 sınıf daha karakter sprite'ları (7 × 64×64px set)
> - Tüm skill ikonları (112 ikon)
> - 14 neutral pasif ikonu
> - 28 cross-class pasif ikonu

---

### 5-1: Her Sınıf için Yapılacaklar (×7)

Her yeni sınıf için bu adımları tekrarla:

```
1. Art import:
   Assets/Sprites/Characters/{SinifAdi}/ → sprite'ları koy
   Assets/Sprites/Icons/Skills/{SinifAdi}/ → ikonları koy

2. AnimatorController:
   Assets/Animations/Player/{SinifAdi}/AC_{SinifAdi}.controller
   Warblade AC'yi kopyala → sprite referanslarını güncelle

3. ScriptableObjects:
   Assets/ScriptableObjects/Skills/{SinifAdi}/
   Her skill için SO_*.asset oluştur (8 aktif + 4 pasif + 2 ultimate = 14 SO)

4. Skill script'leri:
   Assets/Scripts/Skills/{SinifAdi}/
   Skill_*.cs dosyaları

5. Resource bar (sınıfa özel):
   Warblade → Rage
   Mage → Mana
   Rogue → Energy + Combo Points
   Ranger → sadece CD (bar yok)
   Berserker → Fury
   Paladin → Holy Power
   Summoner → Charges
   Hexer → Hex Stacks (düşman başına)

6. Test: Sınıf seçilince çalışıyor mu? Kombo pasifi aktif mi?
```

---

### 5-2: Resource Bar Mimarisi

```csharp
// ResourceBar.cs — abstract base
public abstract class ResourceBar : MonoBehaviour {
    public float current, max;
    public abstract void OnHitDealt(float damage);
    public abstract void OnHitReceived(float damage);
    public abstract void OnKill();
    public abstract void Tick();  // her frame
}

// Concrete implementations:
RageBar : ResourceBar        (Warblade)
ManaBar : ResourceBar        (Elementalist)
EnergyBar : ResourceBar      (Rogue) + ComboPoints
FuryBar : ResourceBar        (Berserker)
HolyPowerBar : ResourceBar   (Paladin)
ChargeBar : ResourceBar      (Summoner)
HexStackManager : MonoBehaviour  (Hexer — düşman başına Dictionary<EnemyID, int>)
```

---

### 5-3: Cross-Class Pasif Sistemi

```csharp
// CrossClassPassiveDatabase.cs
Dictionary<(ClassType, ClassType), PassiveData> crossPassives;

// Run başı:
ClassType a = runData.primaryClass;
ClassType b = runData.secondaryClass;
PassiveData xp = crossPassives[(a, b)] ?? crossPassives[(b, a)];
playerPassiveManager.ActivateCrossPassive(xp);

// CrossClassPassive.cs — her komboya özel trigger mantığı
// Örn: FallenSaint → onHolyEnduranceProc + onBerserkerLowHP → Martyr's Loop
```

---

### 5-4: FAZ 4 Test Kriterleri

```
✓ 8 sınıfın hepsi seçilebiliyor ve oynanabiliyor
✓ Her sınıfın resource bar'ı kendine özgü çalışıyor
✓ 28 cross-class pasifin hepsi aktif oluyor
✓ Build çeşitliliği: aynı iki sınıfla iki farklı run = farklı hissettiriyor
✓ Dual class "insane" anları yaşanabiliyor (en az 3 kombo)
✓ Grudge sistemi tüm 8 sınıf için çalışıyor
```

---

## AŞAMA 6 — FAZ 5: META PROGRESSION + POLISHING

> **Ön koşul:** FAZ 4 test kriterleri geçildi.

---

### 6-1: Hub Sahnesi

```
Hub.unity — yeni sahne

Elementler:
  - Hub mimarisi (dungeon girişi etrafında küçük alan)
  - NPC'ler: Soul Dust satıcısı, lore figürü
  - Dungeon kapısı → ClassSelectScene'e geçiş
  - Meta upgrade tahtası (duvara asılı panel)

MetaProgression.cs:
  PlayerPrefs veya JSON → run'lar arası kalıcı
  Toplam Soul Dust
  Açık sınıflar (başlangıçta 2, kalanı unlock)
  Açık meta upgrade'ler

MetaUpgradeBoard.cs:
  Upgrade listesi (GUCLENME_SISTEMI.md'den alınır)
  Her upgrade: Soul Dust maliyeti + efekt
  Satın alınca: RunData'ya etki
```

---

### 6-2: URP 2D Lighting Tam Kurulum

```
Her sahne için:
  Global Light 2D: intensity 0.2-0.3
  Meşaleler: Point Light 2D (color #e07030, radius 3, intensity 1.5)
  Kapı açıkken: Point Light 2D interpolate açılır (Coroutine)
  Elite oda girişi: kırmızı tint (Global Light 2D color lerp, 0.5s)
  Boss oda: tüm ışıklar sönükleşir, boss etrafı Point Light 2D

Normal map'ler:
  Karakter sprite'lar: Laigter ile normal map üret
  Tile sprite'lar: Laigter veya Aseprite-plugin
  Material: Sprite-Lit-Default (zaten 2D URP'de standart)
```

---

### 6-3: VFX Polishing

```
Skill efektleri — her skill için:
  Hit VFX: ParticleSystem burst (sınıf rengine göre) + sprite flash
  Cast VFX: küçük aura/glow (başlamadan 0.1s önce)
  Death VFX: karakter dissolve shader veya particle patlama

Shader'lar:
  Hit flash: Material.SetColor("_Color", white) → 2 frame → normal
  Dissolve death: basit Sprite-Unlit-Default shader + alpha cutoff
  Outline (selected/hovered enemy): SpriteOutline shader (free packages var)
```

---

### 6-4: Ses Sistemi

```
AudioManager.cs (singleton, DontDestroyOnLoad):
  PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
  PlayMusic(AudioClip track, float fadeTime = 1f)
  Pitch randomization: 0.9 - 1.1 arası random → mekanik tekrar hissi azalır

SFX listesi (FAZ 5 minimum):
  Warblade: charge_whoosh, sword_swing, hit_heavy, rage_activate
  Grunt: footstep, attack, death
  UI: card_flip, skill_select, reward_fanfare
  Ambient: dungeon_hum, torch_crackle (loop)

Müzik:
  Normal oda: ambient dungeon track
  Elite oda: gerilim track (drums giriyor)
  Boss oda: tam boss müzik
  Cross-fade: AudioMixer → snapshot geçişleri
```

---

### 6-5: FAZ 5 Test Kriterleri

```
✓ Hub sahnesi oynanabilir, NPC ile konuşulabiliyor
✓ Meta unlocklar kalıcı (oyunu kapatıp açınca hâlâ var)
✓ URP 2D lighting mood doğru: soğuk ortam, sıcak meşale
✓ Her skill hit'te beyaz flash var
✓ Boss music → normal music geçişi smooth
✓ Ses açma/kapama ayarı var
✓ Fps 60+ sabit (profiler ile kontrol et)
```

---

## AŞAMA 7 — FAZ 6: DENGE + STEAM

---

### 7-1: Denge Testleri

```
Playtest protokolü:
  □ Her sınıf ayrı ayrı 5'er run → "insane" build anı geldi mi?
  □ Grudge: düşmanlar ölme tipi değiştirince farklı davranıyor mu?
  □ Run uzunluğu: 35-55 dakika aralığında mı?
  □ Zorluk eğrisi: Act 1 ölüm oranı %20-30, Act 3 %60-70 olmalı
  □ Shop dengesi: Soul Dust çok mu kolay kazanılıyor?
  □ Overpowered kombo var mı? (Fallen Saint, Plague Doctor)
  □ Cross-class sinerji: 5 farklı kombo denendi mi?
```

---

### 7-2: Steam Hazırlığı

```
Steamworks SDK:
  Steamworks.NET paketi (Unity için)
  AppID: steamworks.steampowered.com'dan başvur

Store Page gereksinimler:
  □ Capsule (616×353px)
  □ Header Capsule (460×215px)
  □ Library Capsule (600×900px)
  □ Screenshot (5 adet minimum, 1280×720px+)
  □ Trailer (60-90s, gameplay odaklı)
  □ Store açıklaması (kısa + uzun)
  □ Etiketler: Roguelite, Action, RPG, Dark Fantasy, Pixel Art

Build ayarları:
  File → Build Settings → PC, Mac & Linux Standalone
  Compression: LZ4HC
  IL2CPP (mono yerine — daha hızlı runtime)
  Steam Overlay entegrasyon test et
```

---

## ÖZET — PARALEL İŞ AKIŞI

```
ŞUAN YAPILABİLİR (Unity kapalı):
  [x] IKON_VE_UI_PROMPTLARI.md → tüm prompt'lar hazır
  [ ] Warblade karakter sprite'ları (FAZ 1 için kritik)
  [ ] Grunt sprite'ları
  [ ] Tileset
  [ ] UI frame'ler (HP/Rage bar, slot)
  [ ] FAZ 1 ikonları (4 Warblade aktif)

UNITY AÇINCA İLK YAPILACAK:
  [ ] Proje oluştur (2D URP)
  [ ] 4 paket kur (Input System, Cinemachine, Tilemap Extras, TMP)
  [ ] Klasör yapısı
  [ ] Layer'lar
  [ ] Art import (0-A listesi hazırsa)

PARALEL DEVAM:
  Unity'de: PlayerController.cs yazarken
  Aseprite'ta: Elite sprite + Grudge badge ikonları yap (FAZ 3 için stok)

KURAL: Bir sistemin art'ı, sistemi yazmadan önce hazır olsun.
       Ama beklemek zorunda değilsin — placeholder ile başla, art gelince değiştir.
```

---

## HIZLI REFERANS — "Nereye koyacağım bu ikonu?"

| Art Dosyası | Unity Klasörü | Nasıl Kullanılır |
|-------------|--------------|-----------------|
| `icon_warblade_charge.png` | `Assets/Sprites/Icons/Skills/Warblade/` | SkillData ScriptableObject → icon alanı |
| `soul_dust.png` | `Assets/Sprites/Icons/System/` | ShopUI + HUD Soul Dust sayacı |
| `grudge_fire.png` | `Assets/Sprites/Icons/System/` | EliteEntryPanel + MapNode |
| `ui_skill_slot_empty.png` | `Assets/Sprites/UI/` | SkillSlotUI prefab |
| `warblade_front_idle.png` | `Assets/Sprites/Characters/Warblade/` | Animator → Idle state |
| `grunt_front_walk.png` | `Assets/Sprites/Characters/Enemies/Grunt/` | Grunt AnimatorController |
| `tileset_dungeon_floor.png` | `Assets/Sprites/Tilesets/Dungeon/` | Tile Palette → Floor layer |
| `vfx_hit_sparks.png` | `Assets/Sprites/VFX/` | HitEffect.cs → ParticleSystem |
| `neutral_evasion.png` | `Assets/Sprites/Icons/Neutral/` | PassiveData SO |
| `xp_fallen_saint.png` | `Assets/Sprites/Icons/CrossClass/` | CrossPassiveData SO |

---

*Dosya: UNITY_BASLANGIC_PLANI.md*
*İlgili: GOREVLER.md, TASARIM/PROJE_GELISTIRME_PLANI.md, ART/IKON_VE_UI_PROMPTLARI.md*
*Son güncelleme: 2026-03-27*
