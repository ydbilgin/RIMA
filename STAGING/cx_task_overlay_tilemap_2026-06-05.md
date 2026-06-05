ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Modüler-props kararı adım 3 [M]: overlay/path tilemap KATMAN ALTYAPISI (council `STAGING/MODULAR_PROPS_DECISION_2026-06-05.md` K2: ikinci Tilemap AYNI Grid'de, Ground'un hemen üstü sort order; serbest decal sprite YASAK — depth-sort'a girmez, drift riski sıfır).

# İş
1. **RoomTemplateSO** (`Assets/Scripts/MapDesigner/Room/`): `overlayMask` grid alanı ekle — walkableGrid'in serialization pattern'ini AYNEN taklit et (aynı boyut, aynı encode tarzı). Değer: 0=yok, 1..N=overlay tile index. Default boş (mevcut asset'ler etkilenmez, migration gerekmez — doğrula).
2. **IsoRoomBuilder**: `[SerializeField] TileBase[] overlayTiles` + Build sırasında `OverlayTilemap` çocuğu (AYNI Grid, Ground tilemap'in sorting order +1, aynı sorting layer). overlayMask[x,y]>0 ise overlayTiles[v-1] bas. overlayTiles boşsa/null'sa katmanı hiç yaratma (zero-cost default).
3. **Placeholder tile:** gerçek patika tile'ı henüz YOK (asset gen GATED). Mevcut floor451 varyantlarından koyu bir varyantı placeholder overlay TileBase olarak kullanılabilir hale getir (varsa mevcut Tile asset'ini referansla — YENİ asset yaratma gerekiyorsa tek bir Tile asset'i `Assets/Data/Tiles/` konvansiyonuna uygun oluştur, gerekçele).
4. **EditMode test (1 dosya, 2-3 test):** overlayMask round-trip + boş-mask'ta OverlayTilemap yaratılmadığı + mask'lı build'de doğru hücrelere tile basıldığı (kod-kurulu Grid ile, IsoRoomBuilderTests pattern'i).
5. **Play-verify:** donut odasını runtime'da yükle, overlayMask'ı RUNTIME'da programatik doldur (asset'i DEĞİŞTİRME — memory'de) → OverlayTilemap hücre sayısı + sort order kanıtla.

# YASAK
.unity düzenleme · mevcut RoomTemplateSO asset'lerine overlayMask verisi YAZMA (painting ayrı iş) · decal sprite yaklaşımı · IsoRoomBuilder'da ilgisiz refactor.

# Doğrulama
dotnet build PASS + read_console 0 error + testler yeşil (Test Runner MCP ile koş) + play-verify kanıtları. CODEX_DONE.md'ye yaz.
