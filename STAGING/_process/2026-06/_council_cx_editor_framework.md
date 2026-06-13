ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amac
Tasinabilir oyun-ici seviye editoru framework'une (LaurethStudio.LevelEditor) BENIM+planli listenin DISINDA hangi ozellikler/mimari eklenmeli — FEASIBILITY/REUSE lensiyle BAGIMSIZ ve genis dusun. ANALIZ ONLY, kod degisikligi YOK, Unity'ye DOKUNMA. CODEX_DONE.md'ye yaz.

LENS (sen=cx): feasibility / RIMA'da & Unity'de NE VAR (reuse) / build-vs-reuse. Onerdigin her ozellik icin RIMA'da/Unity'de hazir parca var mi belirt (grep ile dogrula: Assets/Scripts/MapDesigner/Brush/**, Poisson, RoomDecalChunkRenderer, PropRegistry, WalkabilityMap, telemetry, ChamberSelect, vb.).

## BAGLAM
- RIMA = Unity 2D top-down 3/4 ARPG, ISOMETRIC grid (fake-iso), 64px tile, URP 2D + Pixel Perfect. Demo ~1 hafta sonra in-editor; demo sonrasi cekirdek STANDALONE pakete cikarilacak, her oyuna adapter ile takilacak.
- ZATEN PLANLI (TEKRARLAMA, USTUNE EKLE): Asset Catalog (4 GENISLETILEBILIR kategori Props/Tiles/Lights/Decals + kullanici yeni kategori ekler) + browser (ikon+scroll+arama) + odaya ekle/sil (instance) + select/move/rotate/duplicate + katalog CRUD (runtime asset ekle/sil + kategori ekle) + isik + runtime save/load + undo/redo.
- ZATEN ONERILDI (TEKRARLAMA, USTUNE EKLE): play-test toggle, multi-select+kopyala/yapistir, object inspector, stamp/prefab brush + scatter (Poisson var), validation/lint hook, JSON export/import, layers paneli, minimap+focus+zoom presetleri, editor theming, hotkey config, autosave+undo-history+yikici-op onayi.
- PORTABILITY (planli): cekirdek oyun-agnostik; arayuzler IGridSpace, IAssetCatalog, IPlacementValidator, ILevelStore, IPlaceable; her oyun adapter yazar; standalone asmdef, oyun referansi YOK.

## YANITLA (4)
1) Mevcut+onerilenin DISINDA hangi ozellikler eklenmeli? Her biri: kisa aciklama + deger + efor S/M/L/XL + asiri-mi + RIMA/Unity'de hazir reuse parcasi var mi.
2) Bu framework'u GENRE'LER ARASI reusable yapan ne? (iso ARPG, top-down, platformer ayni cekirdekten nasil beslenir? arayuzlerde eksik gordugun?)
3) Mimaride/portability'de gozden kacan riskler/eksikler (Unity-ozel tuzaklar: asmdef, serialization, addressables, undo, input system, vb.).
4) "Asiri/atla" dedigin ama VAKIT KALIRSA degerli olabilecekler.

Cikti: BAGIMSIZ feature listesi + mimari notlari + kendi AL/SONRA/ATLA cagrilari. CODEX_DONE.md'ye yaz.
