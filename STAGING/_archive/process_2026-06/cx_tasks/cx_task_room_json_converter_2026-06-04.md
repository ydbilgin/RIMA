ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ChatGPT'nin ürettiği oda JSON'unu Unity'de `RoomTemplateSO` asset'lerine çeviren bir EDITOR import aracı yaz. (Oda havuzunu genişletmek için — kullanıcı 10-15 oda istiyor.)

## Önce OKU (şema)
- Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs (hedef tip: roomId, biomeId, roomType, bounds RectInt, walkableGrid bool[] index=y*width+x, doorSockets, playerSpawn, enemySpawnSockets, ...).
- Aynı klasör/namespace'teki soket tipleri: DoorSocket, PlayerSpawnSocket, EnemySpawnSocket (alan adlarını OKU — pozisyon/yön nasıl tutuluyor, ona göre map et).
- RIMA.RoomType enum (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor) — Assets/Scripts/Core/RoomType.cs. ("CombatLarge" gelirse → Combat + difficultyTags veya bounds büyük; "Spawn"/"Corridor" map et; tanınmayan → Combat fallback + uyarı.)

## Girdi JSON şeması (ChatGPT çıktısı — STAGING/CHATGPT_ROOM_DESIGN_PROMPT.md'deki format)
Top-level JSON ARRAY (ya da {"rooms":[...]} wrapper — İKİSİNİ DE kabul et). Her eleman:
```
{ "roomId":"combat_cross_01", "roomType":"Combat", "width":16, "height":12,
  "grid":["...row strings...", ...],  "doors":[{"dir":"N","x":8,"y":0}], "notes":"..." }
```
grid sembolleri: '.'=floor, ' '=void, 'P'=player spawn, 'e'=enemy spawn, 'C'=chest, 'B'=boss spawn.

## Yapılacaklar (YENİ dosya: Assets/Editor/Rooms/RoomJsonImporter.cs)
1. Menü: `RIMA/Rooms/Import ChatGPT JSON`. Girdi dosyası = proje-kökü `STAGING/chatgpt_rooms.json` (System.IO ile oku; yoksa EditorUtility.OpenFilePanel ile seçtir).
2. JSON parse (JsonUtility top-level array'i parse EDEMEZ → ya {"rooms":[...]} wrapper'a sar ya da küçük manuel/Newtonsoft parser; Newtonsoft varsa kullan). grid string[]'i da düzgün oku.
3. Her oda için RoomTemplateSO oluştur (`Assets/Data/Rooms/Generated/<roomId>.asset`, klasör yoksa yarat):
   - bounds = RectInt(0,0,width,height).
   - walkableGrid = bool[width*height], index=y*width+x; '.','P','e','C','B' → true, ' ' → false. (grid satır sayısı=height, her satır=width char; eksik/fazla char → pad/clamp + uyarı.)
   - roomType = enum map.
   - doors → doorSockets (dir N/E/W + x,y → soketin alan yapısına göre; güney YOK kuralı, S gelirse uyar+atla).
   - 'P' → playerSpawn (ilk bulunan; yoksa floor merkezine fallback).
   - 'e' → enemySpawnSockets (her 'e' bir soket).
   - 'B' → boss enemy socket (varsa işaretle), 'C' → chest (uygun soket/prop ya da şimdilik enemySpawn değil — chest marker; uygun alan yoksa notes'a bırak + uyar).
4. AssetDatabase.CreateAsset + SaveAssets + Refresh. Konsola özet: kaç oda, kaç hata/uyarı, hangi roomId'ler.
5. Idempotent: aynı roomId tekrar import edilirse üzerine yaz (CreateAsset yerine varsa load+overwrite).

## Doğrulama
- validate_script + Unity compile temiz (0 hata). Play-mode'a GİRME.
- Gerçek JSON henüz yok → SADECE derlenir-temiz araç yaz; test için STAGING/chatgpt_rooms.sample.json'a 1 küçük örnek oda koyup import-dry mantığını koda yorum olarak açıkla (gerçek import kullanıcı JSON verince çalışacak).
- CODEX_DONE.md'ye: dosya, menü yolu, mapping tablosu, compile durumu, bilinen sınırlar.
