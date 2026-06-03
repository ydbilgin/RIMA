# cx TASK — Modular PixelLab production pipeline (DESIGN/PLAN ONLY, no codegen, no asset gen)

**Amaç:** RIMA'nın yeni LOCKED sanat yönünü (yüzen iso granit ada + mor void + cyan #00FFCC seal-enerji çatlaklar, painterly Hades/Children-of-Morta hissi) **MODÜLER, parça-parça üretilebilir** bir PixelLab→Unity pipeline'ına çevirecek bir ÜRETİM-MÜHENDİSLİĞİ planı yaz. Hedef: prosedürel roguelite odalarını bu stilde IsoGrid üstünde birleştirebilmek. **Bu görev SADECE plan/doküman — kod yazma, asset üretme, dosya değiştirme YOK.**

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece aşağıdaki dosyaları OKU, değiştirme (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## GİRDİ — hedef stil (agy/Imagen konsept görselleri, art-direction north star)
Şu görselleri açıp incele (target look — bunları ÜRETMEK değil, EŞLEŞTİRMEK istiyoruz):
- `STAGING/imagegen/concept01_hero_room_ISO.png` (baz oda: yüzen granit ada, cyan çatlaklı zemin, kalın taş kenar, arkada kırık duvar)
- `STAGING/imagegen/concept03_sundered_beat_ISO.png` (combat: cyan şok-halkası zemini çatlatıyor)
- `STAGING/imagegen/concept05_portal_chest_ISO.png` (reward: cyan portal + sandık)
- `STAGING/imagegen/concept07_boss_arena_ISO.png` (boss: zincirli yuvarlak rün-platform, cyan-alev boss)
- `STAGING/imagegen/concept09_void_map_AGY.png` (StS-stili node harita: cyan-ipliklerle bağlı platformlar)

## OKUMAN GEREKEN MEVCUT VARLIKLAR
- `STAGING/PIXELLAB_KNOWLEDGE_BASE.md` — TOOL MATRIX (create_tiles_pro, create_topdown_tileset=gerçek Wang16, create_1_direction_object=64-aday batch prop motoru, create_8_direction_object, create_map_object, style_images/init-strength skalası, prompt grammar, RIMA Style Lock string). PIPELINE'I BUNA DAYANDIR.
- Mevcut Unity import tooling (yeniden kullan, sıfırdan yazma): `Assets/Editor/TileImport/PixelLabPngSheetImporter.cs`, `Assets/Editor/TileImport/PixelLabWangImporter.cs`, `Assets/Editor/MapDesigner/PatchAtlasSpriteAtlasBuilder.cs`, `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs`.
- Doğrulanmış iso reçetesi: IsoGrid Isometric, cellSize (0.96, 0.585, 1), floor451 (`Assets/Sprites/Environment/PixelLabFloor451/`), squash YOK. Floor zaten `create_tiles_pro` ile üretildi ve çalışıyor.

## CEVAPLA — ÜRETİM PIPELINE TASARIMI
1. **Katman ayrıştırması:** stili oynanabilir oda için kaç ayrı asset-katmanına böleriz (floor / cliff-edge+ada-altı / duvar-harabe / proplar / cyan-VFX-decal / karakter-boss)? Her katman ayrı Unity tilemap/obje/particle olarak mı durur? Cyan'ı tile'a baked ETMEDEN ayrı decal+2D-Light katmanı tutmanın somut yolu (%5-8 bütçe).
2. **Asset-tipi → PixelLab aracı eşlemesi:** her katman için TAM hangi PixelLab tool + neden (tileability, batch verimi, maliyet/gen). KB Tool Matrix'inden seç. Floor=create_tiles_pro mu yoksa gerçek Wang16 için create_topdown_tileset mi? Cliff kenar/köşe seti nasıl? Proplar create_1_direction_object batch mi?
3. **Stil tutarlılığı:** agy painterly konsepti PixelLab çıktısına nasıl bağlanır — concept image'i `style_images[]`/init image olarak kullanma stratejisi + init-strength değerleri (KB'deki 0-1000 skalası). pixelify-tarzı HD→PixelLab köprüsü uygulanır mı?
4. **Unity montaj otomasyonu:** PixelLab çıktısı → import → SpriteAtlas → TileBase → IsoGrid'e montaj akışı. Mevcut importer'ların (yukarıda) hangisi neyi karşılıyor, eksik ne? Sıfırdan yazılması gereken MİNİMUM tooling (varsa) nedir — sadece liste, kod değil.
5. **Arketip kit yapısı:** paylaşılan "Shattered Keep base kit" (floor Wang + cliff kenar + duvar + moloz) + arketip-başı prop paketi (boss: rün-daire+zincir; reward: portal+sandık; hub: shrine). Bu kit'ler asset klasör/SO yapısında nasıl organize edilir ki prosedürel oda-kurucu (RoomLoader/LargeDungeonMapPainter) bunları çekebilsin?
6. **Üretim sırası + maliyet:** ilk üretilecek minimum kit (MVP oda) için PixelLab çağrı listesi + tahmini gen maliyeti (KB cost sütunu). Hangi sırayla.

## ÇIKTI
- Planı `STAGING/MODULAR_PIPELINE_CX.md` dosyasına YAZ (yeni dosya). Başlıkları yukarıdaki 6 madde olsun.
- `CODEX_DONE.md`'ye 5-10 satır özet + dosya yolu + varsa BLOCKED/belirsizlik.
- Kod değiştirme, asset üretme, MCP çağrısı YOK. Sadece OKU + PLAN YAZ.
