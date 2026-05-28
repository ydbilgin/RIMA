# RIMA Room Types + Yarık System + PixelLab Tool Selection

ACTIVE RULES: (1) net karar, savun (2) tool seçiminde MCP > Web UI tercih (sebep yoksa) (3) cost-aware (4) BLOCKED if asset spec unclear.

NLM ACCESS (Codex için): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / STAGING

# AMAÇ
RIMA için Opus 3-Kit modular system + 6 room type + 3-scale yarık (rift) design önerisi hazırladı. Cross-validation gerekli. Ayrıca her asset için en doğru PixelLab tool'unu seç. MCP varsa MCP, yoksa Web UI.

# LOCKED ARCHITECTURE (Opus önerisi, doğrula veya iter et)

## 3-Kit System
| Kit | Rol | Sorting |
|---|---|---|
| A. Top Surface | Floor + edge | 0 |
| B. Depth / Cliff Face | Floor'dan aşağı sarkan stone | -10 |
| C. Background | Void + ruins + fog + parallax | -300 to -500 |

## 6 Room Types × Yarık (rift) seviyesi
| Oda tipi | Story beat | Yarık | Kit A | Kit B | Kit C |
|---|---|---|---|---|---|
| Combat (regular) | Rift seepage | LOW (zemin çatlak) | stone + 1-2 cyan crack | normal | L0+L2+L4 |
| Ritual (failed seal) | Geçmiş mühür | MEDIUM (rune circle) | concentric runes | normal | L0+L4 purple |
| Boss (Penitent) | Active containment | HIGH (Rift Tear hazard) | full runes | edge cyan glow | L0+L1 BIG+L2+embers |
| Treasure / Safe | Pre-rift sanctuary | ZERO | clean stone | + faint moss | L0 + warm amber |
| Bridge transition | Crossing tearing void | MED-HIGH (under-bridge rift) | bridge tiles + edge runes | special bridge cliff variant | L0 + visible Rift Tear |
| Pre-boss approach | Building dread | RISING | rune-lines inward | normal | L0+L1 building+L4 heavy |

## 3-Scale Yarık (Rift) language
| Ölçek | Görsel | Kit | Asset |
|---|---|---|---|
| Mikro | Floor tile cyan crack | A | Mevcut tile_4-6 |
| Orta | Cliff face cyan glow underside | B | YENI (cliff face glow variant) |
| Büyük ambient | Void içinde uzakta rift tear | C-L1 | YENI (Cyan Nebula 512×512) |
| Büyük HAZARD | Boss arena Rift Tear (3m) | Unique | YENI (Rift Tear 128×128) |
| Personal | Penitent chest glow | Boss sprite içinde | Boss sprite üretildiğinde |
| Particle | Cyan motes drift up | C particle sheet | YENI (256×256 4×4) |

## YENI üretim listesi (toplam ~16 asset)
| # | Asset | Boyut | Aspect | Notu |
|---|---|---|---|---|
| 1-8 | Kit B cliff face N/S/E/W edge + 4 corner | 64×96 | Vertical | Hanging stone hero |
| 9 | Kit B cliff face cyan-glow variant | 64×96 | Vertical | Bridge underside dressing |
| 10-12 | Kit A bridge tiles (3 variant) | 64×64 | Square | Bridge floor extension |
| 13 | Kit C-L0 Void Base | 512×512 | Square | Seamless tileable opaque |
| 14 | Kit C-L1 Cyan Nebula/Rift Hero | 512×512 | Square | Unique transparent |
| 15 | Kit C-L2 Far Ruins Strip A | 688×384 | 16:9 | Horizontal tile |
| 16 | Kit C-L2 Far Ruins Strip B (variant) | 688×384 | 16:9 | Variation |
| 17 | Kit C-L3 Floating Island small (4-piece set) | 256×256 | Square | Modular far islands |
| 18 | Kit C-L3 Floating Island large (boss landmark) | 512×512 | Square | Hero piece |
| 19 | Kit C-L4 Fog Veil | 688×384 | 16:9 | Wide tileable |
| 20 | Kit C particle sheet | 256×256 | 4×4 grid | Cyan motes |
| 21 | Light beam decal | 512×512 | Square | Additive |
| 22 | Rift Tear hazard (boss room) | 128×128 | Square | Unique cyan circle |
| 23 | Warm amber overlay (treasure mood) | 512×512 | Square | Treasure dressing |

# MEVCUT PixelLab Tools

## MCP tools (otonom çağrılabilir)
- `create_1_direction_object(description, size 32-256, view: top-down/sidescroller)` — size ≤42→64 cand, 43-85→16, 86-170→4, 171-256→1; 20-40 gen
- `create_8_direction_object(description, size 32-256)` — 8 rotation, 20-40 gen
- `create_isometric_tile(description, size)` — iso tile generation
- `create_topdown_tileset(lower_description, upper_description)` — wang tileset with auto-edge
- `create_sidescroller_tileset(...)` — platformer tileset
- `create_tiles_pro(description, tile_type, tile_size, tile_view)` — pro tile set with type choice
- `create_map_object(description, ...)` — map prop
- `create_character(...)` — character (kullanmıyoruz)
- `animate_object(object_id, animation_description)` — existing object'e animasyon

## Web UI tools (manuel, user gen yapar)
- Create S-XL Image (Pro): max 1:1=512×512, 16:9=688×384, slider aspect ratio (262K px² area)
- Create M-XL Image (PixFlux): Tier 2 max area 400×400 (160K px²)
- Create images from style references (Pro): style image with custom size

# SORULAR (her ikiniz cevap verin)

## 1. Room+Yarık system validation
- 6 room type yeterli mi, eksik mi?
- 3-scale yarık taxonomy doğru mu, refine gerekli mi?
- Story-room mapping makul mü?

## 2. TOOL SELECTION (asset bazlı tablo)
Her asset için (1-23 listesi yukarıda):
- **BEST tool** (MCP function veya Web UI tool)
- **BEST method** (single, n_frames batch, style ref chain, multi-call)
- **Tahmini cost** (gen sayısı)
- **MCP yapılabiliyor mu / Web UI şart mı**

## 3. Production order
- 23 asset'i hangi sırayla üretelim (MVP → polish)
- Hangileri paralel, hangileri serial bağımlı
- Toplam cost estimate

## 4. MCP otonom başlatma uygunluğu
- User awake, onay verdi ("üret mcp ile üretebiliyosan")
- Hangi assetleri ŞIMDI MCP ile başlatabiliriz (otomatik dispatch)
- Hangileri user manuel Web UI ile yapsın

# OUTPUT FORMAT
```
## Section 1: Room+Yarık validation
<your refinements or "approved as-is">

## Section 2: Tool Selection Table
| # | Asset | Best Tool | Method | Cost | Source |
|---|---|---|---|---|---|

## Section 3: Production Order
<sequence + dependencies>

## Section 4: MCP autonomous candidates
<list assets that can start NOW>

## Final verdict: <tool/method strategy summary>
```

# Constraints
- 800-1200 kelime
- Tüm sorular cevaplı
- Tool seçiminde MCP > Web UI default (sebep yoksa)
- Net verdict
