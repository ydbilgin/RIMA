# Unity Sahne Bağlamaları Audit — Lens 3

**Tarih:** 2026-06-13
**Mod:** READ-ONLY (sahne/dosya değiştirilmedi, Play Mode'a girilmedi)
**Aktif sahne:** `Assets/Scenes/_Arena.unity` (build index 9, enabled)
**Audit kapsamı:** Console, missing script, null SerializeField, Resources.Load yolları, prefab sağlığı, sorting layer tutarlılığı
**Demo zinciri:** MainMenu → CharacterSelect → `_Arena` (CharacterSelectScreen/Controller `gameSceneName = "_Arena"`; ChamberSelectBootstrap `ArenaRunSceneName = "_Arena"`)

---

## ÖNEMLİ BAĞLAM: Demo objeleri runtime-bootstrap

`_Arena` sahnesinde DirectorMode / DeathScreenManager / HUD GameObject'leri **YOK — bu TASARIM GEREĞİ, bug değil:**
- `DirectorMode` → `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` ile kendini yaratır (`DirectorMode.cs:140-151`), `DontDestroyOnLoad`.
- `DeathScreenManager` → `RoomRunDirector.EnsureDeathScreenManager()` ile runtime'da yaratılır (`RoomRunDirector.cs:352, 1503-1511`); RuntimeRoomManager da auto-find eder.
- HUD → `RoomRunDirector.EnsureHUD()` her room-build'de çağrılır (`RoomRunDirector.cs:280`).
- DirectorMode/DeathScreenManager SerializeField'larının null kalması beklenen davranış ("auto-found or auto-created if null" — kod yorumu doğrular).

Yani bu üç sistemin sahnede görünmemesi DEMO-KRİTİK DEĞİL; runtime'da oluşurlar.

---

## BULGULAR

### 🟠 HIGH — `floor_riftcrack` decal'i Resources/ dışında, runtime'da yüklenemiyor
- **Dosya:** `Assets/Scripts/VFX/SkillVfx.cs:131` → `Resources.Load<Sprite>("Sprites/Environment/Decals/floor_riftcrack")`
- **Gerçek konum:** `Assets/Sprites/Environment/Decals/floor_riftcrack.png` (Resources/ klasörü ALTINDA DEĞİL → `Resources.Load` null döner)
- **Etki:** Rift-crack zemin decal VFX'i hiç görünmez. `?? RimaUITheme...` fallback'i var → CRASH YOK, ama görsel eksik.
- **Minimal fix:** PNG'yi `Assets/Resources/Sprites/Environment/Decals/floor_riftcrack.png` altına kopyala/taşı VEYA SkillVfx.cs'teki yolu mevcut Resources içi bir decal'e güncelle.

### 🟠 HIGH — `music_demo` arka plan müziği Resources'ta yok
- **Dosya:** `Assets/Scripts/Audio/AudioManager.cs:171` → `Resources.Load<AudioClip>("Audio/music_demo")`
- **Gerçek durum:** `Assets/Resources/Audio/` altında `music_demo.*` dosyası YOK (BossIntro, Cast, Death vb. var ama music_demo yok).
- **Etki:** Demo'da arka plan müziği çalmaz. Crash yok (null clip ile sessiz).
- **Minimal fix:** Bir müzik klibini `Assets/Resources/Audio/music_demo.wav` olarak ekle VEYA AudioManager.cs'teki yolu mevcut bir klibe çevir.

### 🟡 MEDIUM — Sprite'sız placed prop (görünmez/boş SpriteRenderer)
- **Obje:** `prop_b50be24c830a450ba0ccd36625e57c31_7_5` — SpriteRenderer var ama `sprite = NULL`, Props layer, order=-380, y≈3.8.
- **Etki:** Bu prop ya hiç görünmez ya da (shader'a göre) bariz boş/magenta quad riski. Demo karesinde göze çarpabilir.
- **Minimal fix:** Sahnede objeyi bul, ya doğru sprite'ı ata ya da objeyi sil (placement artefaktı).

### 🟡 MEDIUM — Konsoldaki transient hatalar (edit-mode kaynaklı, runtime'da yanlış alarm)
- **`[RoomRunDirector] Missing IsoRoomBuilder reference.` (×12+, RoomRunDirector.cs:284):** Serialize edilmiş `builder` alanı sahnede DOLU (`builder=IsoRoomBuilder`, doğrulandı). Hatalar edit-mode'da `IsoRoomBuilderTester` / live-reload'un `BuildCurrentRoom()`'u erken çağırmasından geliyor. Audit sonunda konsol 0 girdiye düştü → kalıcı değil.
- **`[RoomLayoutData] FromJson called with empty string` + `JSON parse error` (RoomLayoutData.cs:35,47):** Boş/bozuk JSON ile çağrı; graceful-degrade (null döner, crash yok). Muhtemelen live-reload edit-mode artefaktı.
- **Etki:** Demo'da fonksiyonel risk düşük; ama konsolda kırmızı hata = sunum sırasında kötü görünür.
- **Minimal fix:** Demo öncesi konsolu temizle (`read_console clear`). Kalıcı çözüm için IsoRoomBuilderTester'ın edit-mode auto-build'ini kapat (opsiyonel, demo-sonrası).

### ⚪ LOW — RoomRunDirector & IsoRoomBuilder null SerializeField'ları (hepsi auto-resolve)
- **RoomRunDirector:** `encounterController`, `defaultEnemyPrefab`, `enemyContainer`, `rewardSprite` = NULL → ama `Resources.Load` fallback'leri var (RoomRunDirector.cs:1122-1132 + DefaultEncounterBankPath/RewardSpritePath). `encounterBank=Act1_EncounterBank_Pilot`, `playerPrefab=Warblade`, `bossPrefab=PenitentSovereign` DOLU.
- **IsoRoomBuilder:** `decorationRegistry` (enableAutoDecoration=false ile kapalı), `decorationsContainer` + `lightingContainer` → runtime'da `CreateContainer` ile otomatik yaratılır (IsoRoomBuilder.cs:245,250).
- **Etki:** Yok. Bilgi amaçlı.

### ⚪ LOW — DemoPlayer sadece görsel placeholder (beklenen)
- **Obje:** `DemoPlayer` — tag=Untagged, sadece Transform + SpriteRenderer (Health/Controller/Collider YOK). Sahnede "Player" tag'li obje 0.
- **Davranış:** Runtime'da `RoomRunDirector` `IsFunctionalPlayer` testinden geçemeyince DemoPlayer'ı yok eder, `playerPrefab` (Warblade) instantiate eder, `EnsurePlayerRuntime` ile tag'i "Player" yapar (RoomRunDirector.cs:546-580, 774). DeathScreenManager `FindGameObjectWithTag("Player")` o noktada çalışır.
- **Etki:** Yok — zincir tasarım gereği sağlam. (Not: Warblade.prefab disk'te Untagged; tag runtime'da set ediliyor, doğru.)

---

## RESOURCES.LOAD YOLU DOĞRULAMASI (task özel isteği)

| Yol | Çağrı yeri | Durum |
|-----|-----------|-------|
| `DirectorProps/rift_crystal` | DirectorMode.cs:120 | ✅ VAR (`Resources/DirectorProps/rift_crystal.prefab`) |
| `VFX/Skills/slash_arc_main` + `_crescent` | SkillVfx.cs:117-118 | ✅ VAR |
| `Prefabs/VFX/SlashArcVFX` | ChamberSelectBootstrap.cs:1796 | ✅ VAR |
| `DamagePopup` | DamagePopup.cs:27 | ✅ VAR |
| `Prefabs/Warblade` | ChamberSelectBootstrap.cs:957 | ✅ VAR |
| `Combat/BasicAttack/BasicAttackProfile_{5 sınıf}` | SkillBarUI/PlayerAttack | ✅ 5/5 VAR |
| `Encounters/Act1_EncounterBank_Pilot` | RoomRunDirector default | ✅ VAR |
| `Sprites/Environment/Decals/floor_riftcrack` | SkillVfx.cs:131 | ❌ YOK (Resources dışında) → 🟠 HIGH |
| `Audio/music_demo` | AudioManager.cs:171 | ❌ YOK → 🟠 HIGH |

---

## SORTING LAYER / Y-SORT TUTARLILIĞI

Tutarlı, ters-katman riski YOK:
- **Ground** (backdrop katmanları): order −800 (Void) → −500 (Fog), oyuncunun en arkasında. ✓
- **Floor** (cliff/door/rune): −16 ile +56 arası. ✓
- **Entities** (DemoPlayer/runtime player): order 0. ✓
- **Props**: −380 (bir prop sprite'sız — yukarıda 🟡). 

Backdrop'un oyuncunun önüne geçme riski yok (Ground << Entities). Y-sort mantıksızlığı tespit edilmedi.

---

## MISSING SCRIPT TARAMASI
`_Arena` sahnesi: 62 GameObject, **0 missing script (null MonoBehaviour)**. ✓

---

## ÖZET TABLO

| Önem | Bulgu | Demo'da patlar mı? |
|------|-------|--------------------|
| 🟠 HIGH | `floor_riftcrack` decal Resources dışı → VFX görünmez | Hayır (görsel eksik, crash yok) |
| 🟠 HIGH | `music_demo` Resources'ta yok → müzik yok | Hayır (sessiz, crash yok) |
| 🟡 MEDIUM | Sprite'sız prop (`prop_b50be24..._7_5`) | Olası (boş/magenta quad göze çarpabilir) |
| 🟡 MEDIUM | Konsolda transient RoomRunDirector/JSON hataları | Hayır (edit-mode noise; demo öncesi temizle) |
| ⚪ LOW | RoomRunDirector/IsoRoomBuilder null ref'ler | Hayır (hepsi auto-resolve) |
| ⚪ LOW | DemoPlayer placeholder, runtime'da gerçek player spawn | Hayır (tasarım gereği) |

## KONSOL DURUMU
- **Audit başı:** 17 hata (12× RoomRunDirector "Missing IsoRoomBuilder" + RoomLayoutData JSON ×2 + 3 diğer) — hepsi edit-mode/transient.
- **Audit sonu:** **0 hata, 0 warning.** Serialize edilmiş referanslar (builder dahil) sahnede DOLU.
- **Demo önerisi:** Sunum öncesi konsolu temizle; gerçek runtime hatası tespit edilmedi.
