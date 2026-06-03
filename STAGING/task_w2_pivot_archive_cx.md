ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>". Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

Amaç: (A) 10 sınıfın 8-yön idle sprite'larının pivot'unu AYAKLARA oturt (karakter "yüzüyor" P0 bug'ı + depth-sort düzeltmesi). (B) Güvenli orphan'ları arşivle. Unity instance açık, UnityMCP profilinden eriş.

GROUND TRUTH (Opus doğruladı): canvas boyutları 120/124/128 px (sınıfa göre); PPU=64, filterMode=Point, spriteImportMode=Single hepsinde DOĞRU. SORUN sadece PIVOT: Warblade tüm yönler align=Custom(9) spritePivot=(0.5,0)=canvas DİBİ ama ayaklar ~30px yukarıda→yüzüyor; diğer 9 sınıf align=Center(0) → (0.5,0.5)=göğüsten asılı. Her canvas'ta ayakların altında şeffaf padding var, bu yüzden sabit (0.5,0) YANLIŞ — ÖLÇMEK gerekiyor.

## PART A — MEASURED feet-pivot fix (80 sprite)

`Assets/Editor/Tools/SpritePivotBatchFix.cs` ŞU AN BOZUK: yanlış klasör (Assets/Art/Characters) arıyor (gerçek yol Assets/Resources/Characters) + pivot (0.5,0) hardcode (yüzme bug'ını üretir). DÜZELT:
1. SearchFolders → `Assets/Resources/Characters` (recursive, tüm `*_idle_*.png`).
2. Pivot mantığını ÖLÇÜM-tabanlı yap: her texture için
   - importer.isReadable = true (geçici), spriteImportMode = Single garanti et.
   - Pixel'leri oku, alpha > ~10/255 olan EN ALT satırı bul (lowestOpaqueRow).
   - feetY_norm = lowestOpaqueRow / textureHeight.
   - TextureImporterSettings: spriteAlignment = 9 (Custom), spritePivot = (0.5, feetY_norm). PPU=64, filterMode=Point(0), alphaIsTransparency=true, mipmapEnabled=false KORU.
   - SaveAndReimport. (Bittiğinde isReadable'ı eski haline döndürmek opsiyonel; readable bırakmak da kabul.)
3. Çalıştır: 10 sınıf × 8 yön = 80 sprite (Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer; her biri _idle_{south,SE,east,NE,north,NW,west,SW}.png).
4. Raporla: sınıf başına uygulanan feet pivot.y aralığı + 80 sprite işlendi mi.

NOT: Warblade'in en-alt opak satırı ~30px olmalı (120 canvas → pivot.y ~0.25). Ölçüm bunu otomatik bulacak; sınıf/yön farklılıklarına dayanıklı.

## PART B — Archive SAFE orphans (move to Assets/_archive~/, .meta dahil)

SADECE şunlar (System.IO.Directory.Move/File.Move + .meta; sonra AssetDatabase.Refresh):
- `Assets/Animations/Characters/Warblade/Warblade.controller` (+ .meta) — SADECE bu DUPLICATE .controller dosyası (guid 3ada902577124af1b44b51161b0ba3a9). ⚠️ Aynı klasördeki `*.anim` dosyalarına DOKUNMA (onlar Resources controller'ın LIVE klipleri).
- `Assets/Sprites/Mobs/_Placeholders` (klasör)
- `Assets/Art/ConceptRefs` (klasör)
- `Assets/Art/Reference` (klasör)
- `Assets/_Recovery` (klasör, crash-recovery scene dump'ları)

⛔ DOKUNMA (belirsiz/live): Assets/Art/Characters, Assets/Prefabs/Characters/Warblade.prefab, Assets/Sprites/Characters, Assets/Art/_TempReferencePacks, Assets/Resources/Characters/* (live), env keep-triad (CliffKit_RefB/PixelLabFloor451/BgKit_RefC).

## PART C — Verify
read_console error/warning = 0 (missing-ref OLMAMALI). dotnet/Unity compile temiz. CODEX_DONE.md'ye: PART A (per-class feet pivot + 80 işlendi), PART B (taşınanlar), PART C (console/compile durumu) PASS/FAIL.
