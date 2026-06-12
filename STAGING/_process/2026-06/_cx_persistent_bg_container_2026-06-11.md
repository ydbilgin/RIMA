ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Görev: Persistent Background Container + ParallaxLayer wiring (CODE-ONLY, demo-safe)

## Amaç
Şu an arka plan PER-ROOM (`SubRoomSequenceController.PaintBackgroundLayers`, satır 175-202) — her oda
kurulurken `currentSubRoomRoot`'un çocuğu olarak boyanıyor ve oda geçişinde `TeardownCurrentSubRoom()`
(satır 329-336) ile YOK EDİLİP yeniden kuruluyor. İstediğimiz: run başında BİR KEZ kurulan, oda
teardown'undan ETKİLENMEYEN, parallax'lı KALICI bir arka plan (FAR/MID/FRONT). Odalar bg'den bağımsız
üretilmeye devam eder; BG oda değiştikçe parallax'la kayar ama asla destroy/rebuild edilmez.

## Kapsam = SADECE KOD (art YOK, sahne EDİTİ YOK)
Bu görevde gerçek arka plan görseli ÜRETME ve ATAMA. Sadece sistemi kur, default-OFF bırak. Görsel
sonra PixelLab'den gelince (256×144 native, FAR opaque + MID/FRONT 6-frame transparent) atanacak.

## Yapılacaklar

### 1) YENİ: `Assets/Scripts/Background/PersistentBackgroundController.cs`
- namespace `RIMA.Background`.
- MonoBehaviour. Run-level KALICI bir BG kökü kurar (oda root'unun DIŞINDA).
- Serialized config (art-agnostik slot'lar):
  - `[SerializeField] bool enablePersistentBackground = false;`  ← DEFAULT OFF (demo bozulmaz).
  - 3 katman tanımı (FAR/MID/FRONT). Her biri için: `Sprite[] frames` (1 frame = statik), `int sortingOrder`,
    `Vector2 parallaxFactor`, `float animFps`. Bir `[System.Serializable] class BgLayerDef` ile temsil et.
  - Önerilen default'lar (atama yok, sadece inspector default): FAR factor (0.02,0.015) sorting -200,
    MID (0.05,0.04) sorting -150, FRONT (0.10,0.06) sorting -100. (BackgroundLayerData tooltip'ine göre
    boyalı-bg rezerve aralığı -200..-50 — bu aralıkta kal.)
- `BuildIfEnabled()` (Start veya RoomRunDirector çağırır): `enablePersistentBackground` false ise hiçbir şey yapma.
  True ise: tek bir "PersistentBackground" GameObject yarat, parent'ı = bu controller'ın transform'u
  (RoomRunDirector gibi run-boyu yaşayan obje). Her layer için child SpriteRenderer oluştur:
  - sprite = frames[0] (varsa), sortingOrder = def.sortingOrder, drawMode Simple.
  - MEVCUT `RIMA.Background.ParallaxLayer` component'ini EKLE (YENİDEN YAZMA): `factor = def.parallaxFactor`,
    `target = Camera.main`, `snapToPixel = true`, `pixelsPerUnit = 64`.
  - `frames.Length > 1` ise basit bir frame-cycler (animFps, Point/seamless loop) — minik bir coroutine veya
    Update sayacı; sadece sprite değiştirir. Tek frame ise animasyon yok.
- İki kez build edilmeye karşı guard (idempotent): zaten kurulmuşsa tekrar kurma.

### 2) `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` — minimal hook
- RoomRunDirector run başında (mevcut run-init noktasında, ilk oda kurulmadan ÖNCE veya hemen sonra)
  sahnede/objesinde bir `PersistentBackgroundController` varsa `BuildIfEnabled()` çağır. YOKSA hiçbir şey yapma
  (FindObjectOfType ile bul; null ise sessizce geç — yeni bağımlılık zorlama).
- Başka davranış DEĞİŞTİRME. `SubRoomSequenceController.PaintBackgroundLayers`'ı SİLME/DOKUNMA (per-room
  boyalı bg geriye uyumlu kalsın; kalıcı katman -200 aralığında onun ARKASINDA durur).

### 3) Test: `Assets/Tests/EditMode/Background/PersistentBackgroundControllerTests.cs`
- enablePersistentBackground=false → BuildIfEnabled hiçbir child yaratmaz.
- enable=true + her layer'a 1 sprite verilince → 3 child SpriteRenderer + her birinde ParallaxLayer + doğru
  factor/sortingOrder. İki kez BuildIfEnabled → hâlâ 3 child (idempotent).
- (Mümkünse) sorting değerleri -200..-50 aralığında.

## KISITLAR (geri-dönüşü-zor — dikkat)
- `_Arena.unity` sahnesini DEĞİŞTİRME, asset ÜRETME, sprite ATAMA.
- `ParallaxLayer.cs`'i DEĞİŞTİRME — sadece kullan.
- `SubRoomSequenceController.cs`'i DEĞİŞTİRME.
- Pre-existing kodu refactor etme. Sadece bu görevin gerektirdiği 2 dosya + 1 test.
- Compile 0-error. Yeni test yeşil. Mevcut testlerde 0 yeni fail.
- Commit ETME (orchestrator QC sonrası karar verecek) — değişiklikleri uncommitted bırak.

## Başarı kriteri (doğrula + CODEX_DONE'a yaz)
1. `PersistentBackgroundController.cs` derlenir, default-OFF.
2. RoomRunDirector hook'u null-safe (controller yoksa demo aynen çalışır).
3. EditMode test yeşil; tam suite'te yeni fail yok (fail sayısını öncesi/sonrası raporla).
4. _Arena dokunulmadı (git status ile doğrula — `_Arena.unity` diff'te OLMAMALI).
