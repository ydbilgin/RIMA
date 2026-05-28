# PixelLab Envanter Tam Çek + Initial Kategorize

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
220 PixelLab cloud objesini tam çek, master doc skeleton oluştur. Sonraki 3-AI bağımsız analiz için temel data.

## Bağlam
- `mcp__pixellab__get_balance`: $0 credit, 1208 gen kalmış, 3792 kullanılmış, Tier 2 Pixel Artisan
- `mcp__pixellab__list_objects` 220 total
- Mevcut sample (offset=0..50): walls (act1_wall_pieces_s95, act1_wall_pilot_a_s95, act1_wall_structural_v1, RIMA_Wall_Production_v1_Batch1), mobs (act1_mob_s95, 64x64), statues (act1_statue_ritual_s95, 64x64), cliffs (128x96), "Transform into RIMA" (96x96), stone keep generic (96x96, 128x128, 128x160)
- Kullanıcı endişeli: "64lü ürettiklerimiz varmış onu da sen otomatik ürettin" — gece halt rule ihlali şüphesi (`feedback_pixellab_mcp_halt_strict`)

## İş kalemleri

### 1. Tam envanter çekimi (5 batch)
- `mcp__pixellab__list_objects(limit=50, offset=0)` ✅ DONE (sample mevcut)
- `mcp__pixellab__list_objects(limit=50, offset=50)`
- `mcp__pixellab__list_objects(limit=50, offset=100)`
- `mcp__pixellab__list_objects(limit=50, offset=150)`
- `mcp__pixellab__list_objects(limit=50, offset=200)` (kalan ~20 obje)

### 2. Kategorize (tag bazlı + content sniff)
Her obje için kayıt: `{id, title_truncated, dimensions, tag, kategori_tahmin, notes}`

Kategoriler:
- **walls** — `act1_wall_*` tag'leri + "stone dungeon wall", "stone keep"
- **mobs** — `act1_mob_s95` + "ancient" (mob roster)
- **statues** — `act1_statue_ritual_s95` + "ruined stone keep"
- **cliffs** — "cliff face" + 128x96 dim
- **transforms** — "Transform into RIMA" (4 sample)
- **weapons** — eğer varsa (sample'da görünmedi, batch 2-5'te olabilir)
- **characters** — varsa
- **decoration_misc** — kalan

### 3. Master doc skeleton
File: `STAGING/PIXELLAB_INVENTORY_MASTER.md`

İçerik:
```
# PixelLab Cloud Envanter Master Doc

## Hesap
- Tier 2 Pixel Artisan, 1208 gen kalan
- Total 220 obje

## Envanter Tablosu (220 obje)
| # | ID | Title | Boyut | Tag | Kategori | Notes |
|---|---|---|---|---|---|---|
| 1 | 2a383ea6 | pixel art cliff face | 128x96 | — | cliffs | — |
| ... |

## Kategori Özeti
| Kategori | Adet | Genel Notes |
|---|---|---|

## Pending 3-AI Analiz
- [ ] Opus visual/brand coherence
- [ ] Codex technical + codebase mapping
- [ ] agy external research + weapon spec
- [ ] Sentez loop
```

### 4. Local asset cross-check
- `Glob: Assets/Sprites/Environment/ShatteredKeep_PixelLab/**` — mevcut local download'lar
- `Glob: Assets/Prefabs/Props/ShatteredKeep_PixelLab/**` — mevcut prefab'lar (S113 D2 backfill 33 prefab)
- Cloud'da var ama local'de yok olan obje'leri işaretle

## Output
- `STAGING/PIXELLAB_INVENTORY_MASTER.md` (skeleton + tam envanter)
- `STAGING/PIXELLAB_INVENTORY_DUMP_DONE.md` (rapor — batch sayıları, kategori dağılımı)

## YASAK
- PixelLab gen YOK (sadece list, read-only)
- Asset modify yok
- Cloud delete YOK (kullanıcı onayı sonrası)

## Süre
~30-45 dk Sonnet bg.
