# Demo Mob Fonksiyonellik Audit (S114)
**Tarih:** 2026-05-29  
**Kapsam:** Faz-1 demo mob seti + 5 RoomSequenceData SO — sadece okuma, kod yazılmadı.

---

## Mob Prefab Tablosu

| Mob | Prefab Var? | Health? (OnDeath?) | AI Script | Damage? | Collider / RB? | VERDICT |
|---|---|---|---|---|---|---|
| **FractureImp** | EVET — `Assets/Prefabs/Enemies/FractureImp.prefab` | EVET — `RIMA.Health` (maxHP=100, OnDeath UnityEvent mevcut ama listener=0) | `BaseMobBehavior` (chase/attack state machine, detRange=8, atkRange=1.5, speed=3) | EVET — `MobAttack_Melee` (dmg=14, radius=0.8) + `FractureImp_ShardScatter` (dmg=8, **shardPrefab=null**) | CircleCollider2D + RB2D (Dynamic, gravity=1, freeze=none) | **KISMİ** |
| **ShardWalker** | HAYIR — prefab yok | — | Script var (`Assets/Scripts/Enemies/ShardWalker.cs`), animasyonlar var (16 klip, 8 yön) | Script var (projectile atar, `projectilePrefab` null bağlantı gerektirir) | — | **İSKELET** (script+anim mevcut, prefab montajı eksik) |
| **HollowHulk** | HAYIR — prefab yok | — | Yok (sadece tasarım belgelerinde geçiyor) | — | — | **MEVCUT DEĞİL** |
| **PenitentSovereign** (Boss) | İKİ prefab var — `Prefabs/Enemies/Boss/PenitentSovereign.prefab` (eski) + `Prefabs/Enemies/Boss/BossAI_PenitentSovereign.prefab` (yeni) | EVET — `RIMA.Health` (eski maxHP=100, yeni maxHP=500, OnDeath listener=0) | Eski: `PenitentSovereign.cs` (faz1+faz2 tam iki-faz AI, bossMaxHP=800). Yeni: `BossAI_PenitentSovereign.cs` (4 saldırı döngüsü, boss bar referansı=null). | EVET — her iki versiyonda da yerleşik hasar hesabı (chain whip, surge, shackle, blessed whip) | CapsuleCollider2D + RB2D (gravity=0, ContinuousDetection) | **KISMİ** |

### FractureImp Detay
- Rigidbody2D `m_BodyType: 0` (Dynamic), `m_GravityScale: 1` → **SORUN**: top-down/arena oyunda yer çekimi mob'u aşağı çeker. BaseMobBehavior.Awake() bodyType'ı Kinematic'e ayarlıyor → runtime'da override edilir, prefab değeri yanlış.
- `FractureImp_ShardScatter.shardPrefab = null` → ölüm patlaması hiçbir şey oluşturmaz. Shard mekaniği silent-fail.
- `OnDeath` UnityEvent: listener yok → death event sistemi çalışır (BaseMobBehavior.HandleDeath listener ekler kod ile), prefab'ta ek listener gerekmez.
- SpriteRenderer: `m_Sprite: {fileID: 0}` → sprite atanmamış. `PlaceholderSprite` componenti var (color=kırmızı, 32px) + BaseMobBehavior.EnsureVisibleSprite() fallback kırmızı quad oluşturuyor → oyunda görünür olur.

### PenitentSovereign Detay
- **İki ayrı prefab var, SO'da hangisi bağlı?** Room5_BossArena SO, fileID=2119183627493839208 kullanıyor → bu `PenitentSovereign.prefab` (eski, Sprite child var, tag=Untagged). Yeni `BossAI_PenitentSovereign.prefab` (tag=Enemy, bossHealthBar=null) SO'ya bağlı DEĞİL.
- Eski prefab: `PenitentSovereign` script — iki-faz tam AI (100%→50% Faz1, 50%→0% Faz2), bossMaxHP=800 ama Health.maxHP=100. HP çelişkisi: Health'e 100 HP atanmış, boss script bossMaxHP=800 kullanıyor. OnEnable'da bossMaxHP ile Health.maxHP sync edilip edilmediği belirsiz (script tam okunamadı, 60 satır limit).
- `chainProjectilePrefab = null` → Shackle Throw saldırısı silent-fail.

---

## RoomSequenceData Mob Config

| Oda (roomIndex) | displayName | Mob Prefab(lar) | Adet | Elite? | isRewardRoom / isBossRoom |
|---|---|---|---|---|---|
| 0 | Tutorial Combat | FractureImp (x3) | 3 | Hepsi Normal | — |
| 1 | Combat Medium | FractureImp (x5) | 5 | Hepsi Normal | — |
| 2 | Combat Hard | FractureImp (x6) + FractureImp Elite (x1) | 7 | 1 Elite | — |
| 3 | Vestibule | (Boş) | 0 | — | isRewardRoom=true |
| 4 | Boss Arena | PenitentSovereign (x1) | 1 | Normal flag | isBossRoom=true |

**Çeşitlilik Durumu:** 4 combat oda'dan 3'ü tamamen tek tip (FractureImp only). Hiçbir SO'da ShardWalker veya HollowHulk referansı yok. Tüm non-boss moblar aynı prefab GUID `34f9366f44422d54487fd7edd55288d4` (FractureImp).

---

## Mob AI Altyapısı

### Ortak Taban
- **`BaseMobBehavior`** — tüm Tier-1 düşmanların ortak base'i. State machine: `Idle → Chase → Attack → Dead`. FixedUpdate'te player'a Kinematic RB hareket, AttackTokenManager sistemiyle koordineli saldırı. `OnAttackReady` event'i ile MobAttack_* componentleri bağlanır.
- **`EnemyAI.cs`** — eski/legacy class, BaseMobBehavior'dan önce yazılmış. FractureImp kullanmıyor (BaseMobBehavior kullanıyor).

### Mob-Özel Scriptler
| Script | Durum |
|---|---|
| `BaseMobBehavior` | LIVE — FractureImp, diğer enemies |
| `MobAttack_Melee` | LIVE — FractureImp'te bağlı, BaseMobBehavior.OnAttackReady dinliyor |
| `FractureImp_ShardScatter` | KISMİ — shardPrefab null, silent-fail |
| `ShardWalker` | SCRIPT HAZIR, prefab yok |
| `BossAI_PenitentSovereign` | LIVE script, yeni prefab SO'ya bağlı DEĞİL |
| `PenitentSovereign` (Boss eski) | LIVE script, SO'ya bağlı, HP çelişkisi var |
| `EnemyAI` | LEGACY — aktif hiçbir prefabda gözlemlenmedi |

### Eksikler
1. AttackTokenManager singleton — kodda referans var, sahnede/prefab'ta olup olmadığı doğrulanamadı (prefab arama yapılmadı; runtime crash riski).
2. Player tag "Player" olmalı — BaseMobBehavior ve ShardWalker FindGameObjectWithTag("Player") kullanıyor.
3. FractureImp Animator: `m_Controller: {fileID: 0}` → animator controller atanmamış. Death trigger (`SetTrigger("IsDead")`) çalışmaz, fallback graying devreye girer.

---

## Demo-Readiness Özeti

| Mob | Graybox-Playable? | Kritik Eksikler |
|---|---|---|
| **FractureImp** | EVET (graybox) | shardPrefab null (shard scatter çalışmaz), Animator boş (death anim yok), Sprite boş (runtime placeholder ile görünür) |
| **ShardWalker** | HAYIR | Prefab yok — SO'ya bağlanamaz, sahneye eklenemez |
| **HollowHulk** | HAYIR | Script + prefab yok — tasarım aşamasında |
| **PenitentSovereign** | KISMİ (eski prefab) | HP çelişkisi (Health=100, Boss script=800), chainProjectilePrefab=null, Sprite boş, yeni BossAI prefab SO'ya bağlanmamış, bossHealthBar=null |

**Sonuç:** Demo için 1/4 mob combat-playable (FractureImp graybox). 3 oda FractureImp spam. ShardWalker ve HollowHulk Faz-1 demo dışında. Boss oynanabilir ama HP sistemi çelişkili ve projectile saldırısı silent-fail.
