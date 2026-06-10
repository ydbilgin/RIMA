Gelişmiş mimari analizi (Gemini 3.1 Pro High) ve incelemeler tamamlandı. Sistem geneli bütünlüğe, spawn pipeline'ına ve `DraftManager` ile `RoomRunDirector` arasındaki akışlara odaklanıldı. 

İstediğiniz yapısal analiz ve tespitler aşağıdadır:

### (A) KESİN TEŞHİS TABLOSU

| Bug | Kök-Neden (Root Cause) | Dosya & Satır | Çözüm (Fix) |
| :--- | :--- | :--- | :--- |
| **Siyah Render (Moblar)** | Normal mobların prefabları URP ortamında desteklenmeyen built-in **`Sprites-Default`** (`guid: 0000000000000000f000000000000000`) materyalini kullanıyor. (Elitler `Sprite-Lit-Default` kullanıyor ve sorunsuz). | `Assets/Prefabs/Enemies/` altındaki normal mob prefabları (Örn: `FractureImp.prefab`) | Prefablardaki `SpriteRenderer.sharedMaterial`ı Elit/Boss'ların kullandığı lit materyal ile (`guid: a97c105638bdf8b4a8650670310a4cd3`) değiştirin. |
| **Görünmeme / Kaybolma** | `BossAI_PenitentSovereign` ve `Projectile` gibi prefablar `m_SortingLayerID: 0` (Default) katmanında. Zemin/background da Default katmanındaysa render sırasında zeminin altında/arkasında kalıyorlar. | `BossAI_PenitentSovereign.prefab`, `Projectile.prefab` vb. | `m_SortingLayerID` değerini `1293760285` (Entities katmanı) olarak güncelleyin. |
| **Skill-Icon Boş / Timeout Softlock** | `DraftManager.ClassKits` içinde sadece `Warblade` ve `Elementalist` var. Oyuncu `Shadowblade`, `Ranger` veya `Ronin` seçerse, açılış kiti (opening kit) normal draft'a düşüyor. Ancak `RoomRunDirector` 90 sn timeout bitince `draft.ForcePickFirstOpeningKitSkill()` çağırıyor; bu metot kit olmadığı için sessizce iptal oluyor (`return`). Oyuncu skillsiz ve boş barla kalıyor. | `DraftManager.cs:263` (`ForcePickFirstOpeningKitSkill`) ve `RoomRunDirector.cs:257` | `ForcePickFirstOpeningKitSkill` kit bulamazsa, UI'daki açık tekliflerin ilkini (veya default warblade fallback) zorla seçecek şekilde güncellenmeli. |
| **Mimari Boşluk (Mob Çeşitliliği)** | `RoomRunDirector` bir dalga bulamazsa veya test modundaysa `defaultEnemyPrefab`'e düşüyor, bu da hardcoded `FractureImp`. 12 ayrı sprite olmasına rağmen sistem bank'e veya stub'a mecbur; fallback durumunda diversity sıfıra iniyor. | `RoomRunDirector.cs:127` ve `EncounterBankStub.cs:30` | `RoomRunDirector`'a sadece tek prefab değil, fallback için de bir `EncounterWaveSO` verilmeli veya klasördeki prefablardan rastgele pool oluşturan bir sistem yazılmalı. |

---

### (B) 8-P0 CLEANUP EKSİKLERİ VE MİMARİ RİSKLER

1. **DraftManager Depth Riski:** `DraftManager.cs:167` içinde derinlik `run.CurrentNodeId + 1` olarak alınıyor. Eğer Node ID'ler gelecekte GUID formatına geçerse veya non-sequential (lineer olmayan) dallanmalar olursa (örn: 0 -> 4 -> 7), Forge Room milestone'ları (Oda 4 ve Oda 8) tetiklenmeyecektir. Depth hesabı doğrudan `CurrentNode.depth` parametresine bağlanmalıdır.
2. **Opening-Draft Timeout Polling'i:** `RoomRunDirector` içinde `while (draft.IsDraftActive)` dönerek `unscaledDeltaTime` sayılıyor. Bir UI öğesinin dışarıdan (bir yönetmen objesi) sürekli polling (yoklama) ile zorlanması (tight-coupling) yanlıştır. Zamanlayıcı ve timeout aksiyonları tamamen `DraftManager`'ın kendi sorumluluğunda olmalı; bittiğinde bir event `OnDraftResolved` fırlatmalı, yönetmen bu event'i dinlemelidir.
3. **Çift Sistem Çakışması (Legacy vs New):** Sistem geçiş sürecinde olduğu için `RoomRunDirector.cs:167` hem `RoomRunDirector` hem de `RuntimeRoomManager` (eski Legacy) sorgusu atıyor. Sahnelerde hem eski manager hem de yeni director varsa, ikisi de kendi loop'unu çalıştırmaya çalışarak softlock veya double-spawn yaratacaktır. Legacy kod yolları derhal `#if LEGACY_SUPPORT` içine veya obsolete modülüne itilmeli.
4. **Kapı Konsolidasyonu (Softlock Riski):** `builder.BuildExitDoors(doorTypes)` kapıları diziyor ancak kapı collider'ları açıldıktan sonra oyuncuyu "Advancing" state'ine geçirip geçirmediği `RoomRunDirector` lifecycle'ında yeterince sıkı güvence altında değil. Eğer iki kapı üst üste binerse OnTriggerEnter iki kere tetiklenip haritayı bozabilir.

---

### (C) YAZILABİLİR TEST LİSTESİ (Test Otomasyonu)

Mevcut Unity Test Framework altyapısı üzerine eklenebilecek en efektif otomasyon senaryoları:

| Test Adı | Ne Assert Eder? | Test Modu | Zorluk |
| :--- | :--- | :--- | :--- |
| **`EnemiesRenderConfigurationTest`** | `Assets/Prefabs/Enemies/` altındaki tüm objeleri tarar: `SpriteRenderer.sharedMaterial`'ın Default-Sprite **olmadığını** ve `SortingLayerID`'nin `0` (Default) **olmadığını** assert eder. "Mob siyah çıkıyor / yerin altına iniyor" regresyonunu kalıcı çözer. | **EditMode** | **Kolay** (Sadece AssetDatabase ile prefab iterate edilecek) |
| **`KitlessClassDraftTimeoutTest`** | `PlayerClassManager.SelectedClass`'ı `Ranger` yapar. `RoomRunDirector.BeginRun()` çalıştırılır. `Time.timeScale` 100 yapılıp 90 sn (timeout) simüle edilir. Assert olarak `skillController.GetSlot(0) != null` kontrol edilir. Oyuncunun yeteneksiz (boş bar) odaya girip girmediğini yakalar. | **PlayMode** | **Zor** (Zaman simülasyonu, UI atlaması, sahne bootstrap ister) |
| **`DualSystemsConflictTest`** | Sahnede veya bootstrap sırasında `RuntimeRoomManager` (legacy) ve `RoomRunDirector` (yeni) sınıflarının *ikisinin birden* aktif olmadığını assert eder. Çift sistem çalışmasını yasaklar. | **EditMode / PlayMode** | **Kolay** (`Object.FindObjectsByType` sayımı) |
| **`RoomDepthSequentialTest`** | `DungeonGraph.Generate()` bir run üretir; her düğüm için `Depth` property'sinin ebeveyninden tam 1 fazla olduğunu ve ID'lere bağlı olmayan yapısının düzgün olduğunu assert eder. (İlerideki milestone kırılmalarını önler). | **EditMode** | **Orta** (Graph mocklaması gerektirir) |
| **`DoorSoftlockStateTest`** | `RoomRunDirector`'e `MarkCleared()` çağrısı yapıldığında kapıların `BoxCollider2D.isTrigger` / `enabled` olduğunu ve üst üste (aynı Vector3'te) yaratılmadıklarını assert eder. | **PlayMode** | **Orta** |

Analizde Unity'nin native yapısındaki Asset serialize meta verilerine inilerek kök-nedenler tam tespit edilmiştir. Tüm düzeltmeleri ve cleanup önerilerini mevcut sisteme entegre edebilirsiniz.

