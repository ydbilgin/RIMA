# Council — ax Gemini 3.1 Pro (High) — DERIN FRAMEWORK MIMARISI + GENRE-GENELLIGI lensi

LENS (sen=ax 3.1 Pro): derin mimari + genre-genelligi. Bu editoru gercekten TASINABILIR ve cok-genre yapan tasarim. Tiled/LDtk/Unity/Trackmania/LBP/Dreams/Hammer'dan ilham. BAGIMSIZ ve genis dusun.

## BAGLAM
- RIMA = Unity 2D top-down 3/4 ARPG, ISOMETRIC grid (fake-iso), 64px tile, URP 2D + Pixel Perfect. Demo ~1 hafta sonra in-editor; demo sonrasi cekirdek STANDALONE pakete (LaurethStudio.LevelEditor) cikarilacak, her Unity oyununa adapter ile takilacak.
- ZATEN PLANLI (TEKRARLAMA, USTUNE EKLE): Asset Catalog (4 GENISLETILEBILIR kategori + kullanici yeni kategori ekler) + browser (ikon+scroll+arama) + odaya ekle/sil (instance) + select/move/rotate/duplicate + katalog CRUD (runtime asset ekle/sil + kategori ekle) + isik + runtime save/load + undo/redo.
- ZATEN ONERILDI (TEKRARLAMA, USTUNE EKLE): play-test toggle, multi-select+kopyala/yapistir, object inspector, stamp/prefab brush + scatter, validation/lint hook, JSON export/import, layers paneli, minimap+focus+zoom, editor theming, hotkey config, autosave+undo-history+yikici-op onayi.
- PORTABILITY (planli): cekirdek oyun-agnostik; arayuzler IGridSpace (world<->cell/bounds/snap), IAssetCatalog, IPlacementValidator, ILevelStore, IPlaceable; her oyun adapter; standalone asmdef.

## YANITLA (4)
1) Mevcut+onerilenin DISINDA hangi ozellikler eklenmeli? Her biri: aciklama + deger + efor S/M/L/XL + asiri-mi.
2) GENRE-GENELLIGI: ayni cekirdek 2D iso ARPG + top-down + platformer + grid-siz/free-form'a nasil hizmet eder? Mevcut 5 arayuz (IGridSpace/IAssetCatalog/IPlacementValidator/ILevelStore/IPlaceable) YETERLI mi? Hangi arayuz/soyutlama EKSIK (or. komut/undo abstraction, tool/mode abstraction, selection model, coordinate-system abstraction grid-vs-free, layer model, input abstraction, gizmo/handle sistemi)? Genre eklendikce cekirdek bozulmadan nasil genisler?
3) Mimaride/portability'de gozden kacan riskler (serialization stabilitesi/migration/versioning, asmdef/dependency, Unity Input System vs legacy, undo komut modeli, asset-referans-by-id, theming, test edilebilirlik, headless/CI).
4) "Asiri/atla" dedigin ama VAKIT KALIRSA degerli olabilecekler.

Cikti: BAGIMSIZ feature listesi + DERIN mimari notlari (ozellikle eksik arayuzler/soyutlamalar) + kendi AL/SONRA/ATLA cagrilari.
