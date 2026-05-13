# Codex Final Strategy Review: PixelLab Tool S66

## Verdict
C - Hybrid Asymmetric stratejisi kilitlenmeli. RIMA icin en dogru yol, PixelLab'i sadece tile/asset uretim motoru olarak kullanmak, oda tasarimi ve kompozisyonu Unity Room Designer icinde birakmaktir. Bu karar, mevcut vizyonla ve Karar #115, #117, #118 ile dogrudan uyumlu. C secenegi mevcut Floor-Wall ve Floor-Path Wang sheet'lerini cope atmadan kullanir, ama variant ve decal/prop ihtiyacini Wang sistemine zorla bindirmez. Bu kritik, cunku RIMA'nin uzun vadeli gucu Unity tarafindaki portable room core, layer ayrimi ve editor brush deneyiminden gelecek.

A secenegi teorik olarak ucuz ve esnek, fakat Floor-Path gecisini gereksiz yere Unity RuleTile isine iter. B secenegi ise PixelLab'i fazla merkeze koyar; moss, rift crack ve biome gibi aslinda overlay veya faz gecisi olan seyleri terrain-pair Wang sistemine cevirerek Karar #118'in multi-layer mantigini bulandirir. C, bu iki ucu dengeler: temel collider/okunabilirlik icin Wang, zemin ve duvar canliligi icin create_tiles_pro, atmosfer ve varyasyon icin create_object.

## Strateji Karsilastirma Tablosu
| Konu | A | B | C |
|---|---|---|---|
| Variant zenginligi | En yuksek; floor, wall, path icin 64 batch odakli | Yuksek gorunur ama Wang pair cogaldikca variant kullanimi daha katilasir | Yuksek ve kontrollu; ana terrain variantlari 64 batch, detaylar decal/prop |
| Cost | Dusuk, yaklasik 5-7 gen | Yuksek, yaklasik 10-12 gen | Dusuk-orta, ek 4-5 gen ile en iyi verim |
| Unity entegrasyon karmasikligi | Floor-Path edge kurali Unity tarafinda ekstra is ister | Cok sayida Wang pair import, mapping ve rule bakimi ister | Orta; mevcut iki Wang sheet + net layer ayrimi |
| Decal/Prop ayrimi | Iyi; decals ve props object olarak kalir | Zayif; moss gibi overlayler terrain transition'a karisir | En iyi; decal, prop, wall ve base ayrimi korunur |
| Karar #118 uyumu | Uyumlu ama path transition isini Unity'ye iter | Kismi uyumlu; multi-layer yerine terrain-pair sismesi yaratir | Tam uyumlu; Base/Decal/Wall/Prop ayrimini dogal destekler |

## Mevcut Wang Sheet'leri Kullanim Onerisi
- Floor-Wall: KEEP. Bu sheet ana gameplay okunabilirligi, collider siniri ve oda formu icin dogru yerde kullaniliyor. Karar #116'daki raggedness, edge-blend ve drop shadow ihtiyacini ilk basta bu sheet uzerinden test etmek yeterli. Replace ancak Unity import sonrasi edge kalitesi kamera acisinda bariz bozulursa dusunulmeli.
- Floor-Path: KEEP. Bu sheet zorunlu collider siniri degil, ama mevcut uretilmis kaliteli bir transition reference. Faz 1.0'da path layer icin opsiyonel RuleTile olarak import edilmeli. Replace gerekmez; path'i tamamen RandomTile'a indirgemek A secenegindeki gibi gereksiz rule isine yol acar.

## JSON Parser Onerisi
- INTEGRATE Faz 1.0. PixelLab export parser Faz 1.0'a alinmali, cunku bu bir luxury feature degil, pipeline disiplininin temel parcasi. Kullanici artik PixelLab Map editor kullanmayacaksa, PixelLab'den gelen her sheet'in Unity Room Designer'a hizli ve tekrar edilebilir girmesi gerekir. Bu parser, ileride her export icin manuel slice, terrain id okuma, wang mapping ve RuleTile kurulumu tekrarini ortadan kaldirir.
- Implementation tasarimi: TileImportWizard icinde "Import PixelLab Export Folder" akisi olmali. Hook noktasi, mevcut import wizard'in folder selection adimindan sonra calismali: `asset_000.json` parse edilir, `tilesets[]` icinden `tileSize`, `gridLayout`, `wangIndexMapping`, `lowerTerrainId`, `upperTerrainId` okunur, ayni klasordeki PNG sheet'ler eslestirilir, sprite slicing yapilir, Wang index mapping standard ise RuleTile asset'i olusturulur. Son adimda Base, Decal, Wall, Prop tilemap iskeleti ve Tile Palette referanslari hazirlanir. Parser editor-only kalmali; runtime portable core'a baglanmamali.

## Pixellab Pipeline Onerilen Siralama (Adim Adim)
1. Mevcut `asset_006.png` Floor-Wall ve `asset_007.png` Floor-Path sheet'lerini Unity'ye parser ile import et.
2. Floor-Wall icin ilk RuleTile olustur, collider/duvar okunabilirligini RoomBaselineGenerator sahnesinde test et.
3. Floor-Path icin opsiyonel Base overlay veya Path tilemap RuleTile olustur; path'i gameplay-critical collider gibi ele alma.
4. `create_tiles_pro` ile 64 cell floor variant batch uret. Hedef: ayni palet, farkli kir, catlak, tas ritmi, hafif ton farklari.
5. `create_tiles_pro` ile 64 cell wall variant batch uret. Hedef: duvar yuzeyi zenginligi, edge sheet ile palet uyumu.
6. `create_object` ile moss decal seti uret. Bunlar Decal layer'a girmeli, Wang terrain olmamali.
7. `create_object` ile rift crack decal seti uret. Bunlar gameplay siniri degil, atmosfer ve rota vurgusu icin kullanilmali.
8. `create_object` ile prop seti uret: kirik sutun, kemik, sandik, rituel obje, duvar dip detaylari.
9. Unity Room Designer brush sisteminde Base, Decal, Wall, Prop layer secimi, silme ve boya akisi kur.
10. Ilk oda mockup'lari tamamen Unity icinde yapilsin; PixelLab Map editor tekrar devreye alinmasin.

## RIMA Spesifik Risk + Mitigation
- Variant zenginligi yetersiz olursa: Ilk cozum yeni Wang pair uretmek degil, floor/wall 64 batch'leri genisletmek olmali. RandomTile agirliklari ve oda bolgesine gore variant palette secimi eklenmeli. Sadece gameplay edge okunurlugu zayiflarsa Wang replace dusunulmeli.
- Wang sheet'in Unity RuleTile rule'a mapping karmasikligi: Parser icinde `wangIndexMapping: standard` tek kaynak kabul edilmeli. Mapping tabloya gomulmeli ve import sonrasi preview grid gosterilmeli. Manual rule editing fallback olarak kalsin, ana akis otomatik olmali.
- create_tiles_pro 64-batch palet driftleyebilir: Her batch icin mevcut Wang sheet'ten palette reference veya promptta net renk/kontrast siniri verilmeli. Unity import sonrasi otomatik palette sanity check yapilamasa bile editor preview sahnesinde yan yana gosterim zorunlu olmali.
- Decal layer gameplay'i kirletebilir: Moss ve rift crack tilemap'lerinde collider kapali olmali. Gameplay etkisi olan her sey Wall/Prop veya explicit gameplay marker layer'inda kalmali.
- 32x32 tile, PPU=64 ve top-down aci tutarsizligi: Import wizard defaultlari Karar #100'e kilitlenmeli. PixelLab export baska tile size bildirirse wizard uyari vermeli, sessizce kabul etmemeli.

## Karar #118 Guncelleme Ihtiyaci
Ek refine gerekir: "PixelLab outputlari multi-layer tilemap sistemine layer semantigine gore import edilir. Wang sheet'ler yalnizca terrain transition ve okunabilir edge icin kullanilir; moss, crack, grime, small debris gibi yuzey detaylari Decal layer'da object/decal asset olarak tutulur. PixelLab Map editor oda tasarimi veya mockup icin kullanilmaz."

Bu Karar #118'i degistirmez, sadece tool pipeline disiplinini netlestirir.

## Faz 1.0 MVP Codex Task Scope Onerisi
Faz 1.0 icin somut is listesi:

1. RoomBaselineGenerator: 32x32 tile, PPU=64 ve yaklasik 35 derece top-down kamera varsayimlariyla test room baseline uretir. Base, Decal, Wall, Prop tilemap objelerini olusturur. Tahmin: 3-4 saat.
2. TileImportWizard PixelLab parser: export folder secimi, `asset_000.json` parse, sheet detection, auto-slice, standard Wang mapping, RuleTile asset creation. Tahmin: 5-7 saat.
3. Multi-layer setup: Tilemap layer order, collider policy, sorting layer, palette klasor yapisi ve default brush hedefleri. Tahmin: 2-3 saat.
4. Preview/test scene: Floor-Wall, Floor-Path, floor variants, wall variants ve decal layer'in ayni sahnede kontrolu. Tahmin: 2 saat.
5. Documentation note: PixelLab sadece asset/tile uretir; map design Unity Room Designer icinde yapilir. Tahmin: 30 dakika.

Toplam Faz 1.0 MVP: 12-16 saat. Bu scope kod yazma ve editor tooling agirlikli, tasarim kararini tekrar acmadan uygulanabilir.

## ORCHESTRATOR NEXT STEP
1. C - Hybrid Asymmetric stratejisini LOCK et ve Karar #118'e yukaridaki refine maddesini ekle.
2. PixelLab tarafinda siradaki uretim emrini ver: floor variant 64 batch, wall variant 64 batch, sonra moss/rift/prop object setleri.
3. Codex'e Faz 1.0 mechanical implementation task'i ac: RoomBaselineGenerator + TileImportWizard PixelLab parser + multi-layer tilemap setup.
4. QC icin rima-qc'ye parser import akisi, RuleTile mapping ve layer collider policy checklist'i hazirlat.
