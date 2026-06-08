# ChatGPT Kod İnceleme Prompt'u (2026-06-09) — paste-ready

> Kullanım: aşağıdaki bloğu ChatGPT'ye (repo erişimi olan web) yapıştır. Cevabı council'den geçir (cx olgusal + ax-3.1 + ax-3.5 → Opus); ChatGPT repo'yu okuyabilse de **stale/legacy kod uydurabilir** → her bulguyu file:line ile doğrula. [[feedback-chatgpt-web-github-review]]

---

Sen bir senior Unity/C# code reviewer'sın. Bu repo'ya (RIMA — 2D top-down ARPG, Unity URP 2D) erişimin var. `master` branch'inin EN SON commit'ini incele. Amacım: **(A) nerede bug/hata var, (B) nerede wiring/bağlantı kopuk** (atanmamış serialized reference, runtime'da eklenip init edilmeyen component, abone olunmayan event, null kalabilecek scene/asset referansı).

Bu repo'da legacy/ölü kod var. Lütfen **sadece CANLI akıştaki** (gerçekten sahnede/runtime'da kullanılan) sorunlara odaklan ve her bulgu için bunu belirt.

Özellikle şu dosyalara/akışlara bak:

1. **`Assets/Scripts/UI/ChamberSelectBootstrap.cs`** (sınıf-seçim odası, runtime'da kuruluyor — büyük dosya):
   - Interaction prompt canvas'ı (ScreenSpaceOverlay/ScreenSpaceCamera) GERÇEKTEN render oluyor mu, görünmeme riski var mı?
   - Proximity/interact tetikleme (radius vs collider standoff) — oyuncu yürüyünce figüre/portala/kuklaya yaklaşınca prompt çıkar mı?
   - `SpawnPlayer` → `PlayerAttack` / `BasicAttackProfile` atama zamanlaması (Awake yarışı) — null kalma riski?
   - Runtime'da eklenen component'ler init/enable ediliyor mu?

2. **Combat oda yaşam döngüsü** — `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` + `Assets/Scripts/Encounter/EncounterController.cs`:
   - Oda "temizlendi" nasıl işaretleniyor, gate'ten geçince sonraki odaya nasıl geçiliyor?
   - **Oda temizlenmesine rağmen gate'ten geçince takılma/softlock** nereden olabilir? (sonraki oda yüklenmiyor, exit door açılmıyor, clear event kaçıyor, coroutine bitmiyor, timeScale geri gelmiyor)

3. **Gate / exit-door** — `IsoRoomBuilder` (`CreateExitDoorObject`) ve gate davranışı: collider boyutu/tipi mantıklı mı (hitbox aşırı büyük mü), trigger vs solid karışıklığı var mı?

4. **Kamera** — `CameraFollow` + orthographic size: zoom oda-başına mı hesaplanıyor (fit-to-room) yoksa sabit mi? Odalar arası zoom tutarsızlığı nereden gelir?

5. **Genel wiring taraması:** atanmamış `[SerializeField]` referansları, prefab'da null kalan alanlar (ör. `slashArcVFX`, `basicAttackProfile`), abone olunup çözülmeyen event'ler (memory leak), `FindObjectOfType`/`Resources.Load` null-dönüş guard'ı eksik yerler.

Çıktı formatı: her bulgu için → **Dosya:satır · Sorun (1-2 cümle) · Güven (Yüksek/Orta/Düşük) · Canlı-akışta mı yoksa legacy mi · Önerilen düzeltme (kısa).** En kritik 10-15 bulguyu önceliklendir. Emin olmadığın yeri "Düşük güven" işaretle, uydurma.

---
