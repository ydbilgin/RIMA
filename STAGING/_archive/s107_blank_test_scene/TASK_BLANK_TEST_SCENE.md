# TASK: Blank Test Scene — PlayableArena_Test01

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Mevcut `PlayableArena.unity`'nin TEMİZ bir kopyasını oluştur. Floor tilemap BOŞ olmalı (kullanıcı painter v4 ile çizecek), CliffRing altındaki manuel cliff sprite'ları silinmiş olmalı (CliffAutoPlacer regenerate edecek). Tüm settings korunsun (lighting, camera, player, parallax, sorting layers, BG layers).

## Hedef sahne
`Assets/Scenes/Test/PlayableArena_Test01.unity`

## Adımlar

### Adım 1: Sahne duplicate
- UnityMCP `manage_scene` ile mevcut `Assets/Scenes/Test/PlayableArena.unity`'yi load et (zaten yüklü olabilir)
- Save as / duplicate → `Assets/Scenes/Test/PlayableArena_Test01.unity`
- Eğer doğrudan duplicate API yoksa: Bash ile dosya kopyala (`cp PlayableArena.unity PlayableArena_Test01.unity` + .meta dosyası YENI GUID ile) — Unity refresh sonrası yeni scene olarak görünür

### Adım 2: Floor tilemap temizle
- Yeni scene'i aç (`manage_scene` load)
- Hierarchy'de Floor tilemap GameObject'ini bul (büyük ihtimal "Floor" veya "FloorTilemap" adında, içinde Tilemap component var)
- `execute_code` ile tilemap.ClearAllTiles() çağır VEYA scene YAML'inde Tilemap component'inin `m_TileSpriteArray` / `m_Tiles` array'lerini boşalt
- Tilemap component kendisi KALSIN (painter çalışsın diye), sadece içerdeki tile data temizlensin

### Adım 3: CliffRing temizle
- Hierarchy'de "CliffRing" GameObject'ini bul (status notuna göre 24 cliff sprite child'ı var, manuel yerleştirilmiş)
- Tüm child'larını sil (`manage_gameobject` delete, veya parent.DestroyChildren)
- CliffRing GameObject KENDISI kalsın — CliffAutoPlacer onu parent olarak kullanıyor

### Adım 4: VoidBlocker temizle (varsa)
- "VoidBlocker" Tilemap'in tile'larını da temizle (collision floor şekline göre yeniden generate edilecek)
- VoidBlocker component'leri (Tilemap + CompositeCollider2D + TilemapCollider2D) kalsın

### Adım 5: Verify
- Yeni sahne hierarchy'sini kontrol et:
  - Player GameObject var (Health/RageSystem vb. 5 component prefab'tan miras)
  - Main Camera + CameraFollow target=Player bağlı
  - CliffAutoPlacer GameObject + CliffPlacementRules_Hades reference dolu
  - Floor tilemap EMPTY ama component dolu
  - CliffRing GameObject EMPTY (0 child)
  - RoomBackgroundRig 6 child (L0-L4 + L3 Small + L3 Large) — L3_Island_Large disabled (varsayılan)
  - Lighting (Global Light 2D + Freeform + braziers + portal + rim/pillar lights) korunmuş
  - URP Bloom profile referansı korunmuş
- `read_console` — 0 error olmalı
- Scene save

### Adım 6: Inline rapor
- Yeni sahne path
- Tilemap empty doğrulaması (cell count = 0)
- CliffRing child count = 0
- Player + CameraFollow + CliffAutoPlacer hala bağlı
- Console error count
- BLOCKED varsa neden

## Hard constraints
- AssetDatabase batch wrapper
- Scene save discipline (yeni sahneyi save etmeden bitirme)
- Surgical — sadece yeni dosya ekle (`PlayableArena_Test01.unity` + .meta), ORIJINAL `PlayableArena.unity` DOKUNULMASIN
- BLOCKED: duplicate edilemiyorsa, tile temizlenemiyorsa
- Commit YAPMA — orchestrator user onayı sonra commit eder

## Önemli not (S106 night-late HARD)
Codex/agy ÇAĞIRMA — bu mekanik iş Sonnet (sen) için. UnityMCP tool'larını kullan: `manage_scene`, `manage_gameobject`, `manage_components`, `execute_code`, `find_gameobjects`, `read_console`.
