# CODEX TASK — Archive Legacy Non-Iso Wall + Floor Assets (S95 LATE NIGHT 2)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory.

## Görev Özeti

User direktifi: "Wall + floor asset pack'ten isometric harici hepsi archive."
Source audit: `STAGING/ASSET_PACK_ISO_AUDIT_S95.md` (orchestrator yazdı, full envanter orada).

Taşınacak:
- **41 PNG** (5 wall + 36 floor)
- **73 .meta** (41 PNG meta + 32 tile asset meta)
- **32 `.asset`** (Data/Tiles/ companion tile asset)
- **1 scene** (`RoomPipelineTest.unity`) → ayrı hedefe

Hedef:
- Asset'ler → `Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/`
- Scene → `Assets/_ARCHIVE/Scenes/`

## CRITICAL: GUID Preservation
- **`git mv` kullan** (history + GUID korunur).
- Her PNG/`.asset` ile birlikte `.meta` taşı (aynı komutta).
- Asla `cp` + `rm` yapma.
- Hedef klasörler yoksa önce `mkdir -p` ile oluştur (hem fs hem git tracked).

## Adım 1 — Hedef Klasör Yapısı

```
Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/
  walls/
    painted_v01/
  floor_tiles/
    granite_base/
    granite_low_topdown_v02/
    painted_v03/

Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/
  Act1_granite_low_v02/
  Act1_ShatteredKeep_painted_v03/

Assets/_ARCHIVE/Scenes/
```

## Adım 2 — Taşıma Listesi (Exact Paths)

### Walls → `_archive/act1_legacy_topdown_s95/walls/`
```
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01/walls_set_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01/ (klasör meta varsa)
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_straight_horizontal_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_corner_L_NE_v01.png (+ .meta)
Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_cyan_rift_integrated_v01.png (+ .meta)
```

`painted_v01/` alt klasörü `walls_set_v01.png` taşındıktan sonra boşalırsa SİL.
Root-level 4 PNG taşındıktan sonra `walls/` klasöründe sadece `pilot_a_test/` kalır (KEEP).

### Floor (tüm klasör içeriği) → `_archive/act1_legacy_topdown_s95/floor_tiles/`

Tüm aşağıdaki klasörler full content (PNG + .meta) hedef klasöre taşınır:
```
Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base/  → _archive/.../floor_tiles/granite_base/
Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02/  → _archive/.../floor_tiles/granite_low_topdown_v02/
Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03/  → _archive/.../floor_tiles/painted_v03/
```

Sonuç: `floor_tiles/` altında sadece `iso/` kalır.

### Companion Tile Assets → `_archive/act1_legacy_topdown_s95/Data/Tiles/`
```
Assets/Data/Tiles/Act1_granite_low_v02/  → _archive/.../Data/Tiles/Act1_granite_low_v02/
Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03/  → _archive/.../Data/Tiles/Act1_ShatteredKeep_painted_v03/
```

(İkincide isim flatten — `Act1_ShatteredKeep/painted_v03/` → `Act1_ShatteredKeep_painted_v03/` çakışmayı önler.)

### Scene → `Assets/_ARCHIVE/Scenes/`
```
Assets/Scenes/Demo/RoomPipelineTest.unity (+ .meta)
```

Eğer `Assets/_ARCHIVE/Scenes/` yoksa oluştur.

## Adım 3 — Komut Şablonu

PowerShell veya Bash, tercih senin. Örnek (bash):

```bash
# Hedef klasörler
mkdir -p "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls"
mkdir -p "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles"
mkdir -p "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles"
mkdir -p "Assets/_ARCHIVE/Scenes"

# Walls
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painted_v01" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/painted_v01"

git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/"
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/act1_wall_partition_low_stub_v01.png.meta" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/walls/"
# ... diğer 3 wall PNG + meta için tekrarla

# Floor klasörleri (klasör taşıma — içerik + meta otomatik)
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_base" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_base"
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_low_topdown_v02" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/granite_low_topdown_v02"
git mv "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/painted_v03" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/floor_tiles/painted_v03"

# Companion Data/Tiles
git mv "Assets/Data/Tiles/Act1_granite_low_v02" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_granite_low_v02"
git mv "Assets/Data/Tiles/Act1_ShatteredKeep/painted_v03" \
       "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/Data/Tiles/Act1_ShatteredKeep_painted_v03"

# Scene
git mv "Assets/Scenes/Demo/RoomPipelineTest.unity" "Assets/_ARCHIVE/Scenes/RoomPipelineTest.unity"
git mv "Assets/Scenes/Demo/RoomPipelineTest.unity.meta" "Assets/_ARCHIVE/Scenes/RoomPipelineTest.unity.meta"
```

**Klasör meta dosyaları:** Eğer Unity klasör `.meta` üretmişse (`walls/painted_v01.meta`, vs.) bunlar da taşınır. `git mv` klasör hedefiyle çağırıldığında içerik+meta birlikte taşınır.

## Adım 4 — Doğrulama

Bittikten sonra şunları çalıştır:

```bash
# Hedef klasör listele
ls -R "Assets/Art/AssetPacks/_archive/act1_legacy_topdown_s95/" | head -100

# Kaynak klasörler temizlendi mi
ls "Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/"           # sadece pilot_a_test/ + 5 KEEP png olmamalı — bu 5 archive'a gitti, sadece pilot_a_test/ kalmalı
ls "Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/"     # sadece iso/ kalmalı
ls "Assets/Data/Tiles/" | grep -E "Act1_granite_low|painted_v03"  # boş dönmeli
ls "Assets/Scenes/Demo/" | grep RoomPipelineTest               # boş dönmeli

# Git status
git status --short | head -50
```

## Adım 5 — Çıktı Raporu

`STAGING/CODEX_DONE_archive_legacy_iso_s95.md` yaz:
- Taşınan dosya sayısı (PNG / meta / asset / scene)
- Boş kalan kaynak klasörler (silindi mi)
- Hedef klasör tree (`ls -R` çıktısı)
- Git status (taşımalar göründü mü, rename olarak mı yoksa add+delete olarak mı — git mv ile rename olmalı)
- Hata varsa flag at

## Kısıtlar

- **HİÇBİR DOSYA İÇERİĞİNE DOKUNMA.** Sadece taşıma. Sprite re-import yok, prefab yeniden bağlama yok.
- **GUID korunur** — `git mv` PNG ile .meta birlikte taşır.
- Eğer `RoomPipelineTest.unity` zaten missing ref'lerle dolu, sorun değil — archive eden sahne, açılmayacak.
- `PathC_BaseTest.unity` (LIVE) etkilenmemeli — kontrol et, ondaki Pilot A ve iso/ ref'leri korunmalı.
- Commit YAPMA — orchestrator commit edecek.

## Effort
high — 41 PNG + 32 asset + 1 scene, ~25-30 git mv komutu. Hata payı düşük, sistematik iş.
