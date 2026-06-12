# PixelLab VFX Batch Limits + Üretim Planı (2026-06-12)

Skill VFX asset üretimi için PixelLab limitleri + ne kadar/ne çizilecek. Kaynak (doğrulanmış): [[reference-pixellab-real-sizes-and-tools]] (S106) + [[pixellab-create-image-pro-format]] (S87 LOCK). VFX spec: `SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md`.

## ⚠️ EN KRİTİK KURAL — "variant" ≠ "farklı item"
PixelLab'ın "boyut küçülünce daha çok çıktı" grid'i = **AYNI PROMPT'un N farklı YORUMU** (en iyisini seç), **farklı item DEĞİL**.
- 64px batch = "slash arc"ın **16 versiyonu** (birini seç) — slash + explosion + shatter DEĞİL.
- 6 farklı VFX istiyorsan → **6 ayrı generation**. Her generation içinde size'a göre N variant gelir (seçersin).

## Boyut → variant/candidate sayısı (doğrulanmış)

### MCP `create_1_direction_object` (square only, otonom)
| Canvas ≤ | Candidate/call | Cost |
|---|---|---|
| 42px | **64** | 20-40 gen |
| 85px | **16** | 20-40 gen |
| 170px | **4** | 20-40 gen |
| 256px | 1 | 20-40 gen |

### Web "Create Image Pro" V3 (manuel, MCP'de yok)
| Output | Variant | Total grid |
|---|---|---|
| 32×32 | 32 | 256² |
| 64×64 | 16 | 256² (4×4) |
| 64×96 / 64×128 / 96×96 custom | 4 | non-square |
| 128×128 | 4 | 256² (2×2) |
| 256×256+ | 1 | tek shot |

→ Hepsi tek prompt'un varyantları. Farklı asset = ayrı prompt + her birinde 4-16 variant.

## Tool seçimi (VFX için)
| İhtiyaç | Tool | Limit |
|---|---|---|
| Şeffaf VFX objesi (arc/patlama/shatter/crack) | **`create_map_object`** | 32–400px, **non-square OK**, basic max 400², ~15-30s, 8h autodelete |
| Variant seçmek istersen (square) | `create_1_direction_object` | 32–256 square, candidate grid yukarıda |
| Animasyon (her VFX'e) | **`animate_object` v3** | max **256×256**, 4–16 frame (default 8), 1-dir obj "unknown" yönü animate eder |
| Farklı numaralı item tek çağrı | `create_tiles_pro` | SADECE tile (terrain/seamless), VFX'e UYGUN DEĞİL |
| ≥512px hero | Web Create Image Pro | MCP 400 cap'li |

**Not:** `animate_object` pro modu pahalı (20-40 gen/yön) → **v3 kullan** (default, ucuz, daha iyi). Pro frame: ≤64→16f, ≤128→4/9/16f, ≤170→9f, >170 desteklenmez.

## VFX asset listesi — PixelLab'da üretilecekler (şekil-kritik)
Her biri: `create_map_object` (şeffaf, element-renkli, alfa-doğrula) → `animate_object` v3 → hemen indir → Unity flipbook. **Yön: 1 base (east) çiz, Unity'de döndür/flip — 8-yön ÇİZME.**

| # | Asset | Canvas (px) | Frame | Renk | View |
|---|---|---|---|---|---|
| 1 | Warblade slash arc (light) | 80×64 | 4 | ember `#E89020` | high top-down |
| 2 | Cleave wide arc (Gravity Cleave) | 128×96 | 4 | ember + void `#7B3FA8` | high top-down |
| 3 | Fireball patlama | 128×128 | 6 | fire `#FF6A1F` | high top-down |
| 4 | Frost shatter (Glacial Spike impact) | 96×96 | 5 | frost `#7FE0FF` | high top-down |
| 5 | Ground crack/fissure (Earthsplitter) | 128×64 (uzun) | 5 | grey/dust + ember kor | high top-down |
| 6 | Ice spike (Glacial Spike) | 64×64 | 3 | frost `#7FE0FF` | high top-down |

→ 6 obje, hepsi ≤128px → `animate_object` v3 sınırı (256) içinde rahat. Paralel background fire = "batch".

## PixelLab'da ÜRETİLMEYECEK (kod/particle — ucuz, daha temiz)
| Asset | Çözüm |
|---|---|
| Spark / ember / toz / frost chip / lightning spark | Unity ParticleSystem + 4–16px chunky texture (1px-art, trivial) |
| Chain Lightning bolt | LineRenderer (kod) |
| Dash trail / projectile trail | TrailRenderer / particle |
| Cast el-parıltısı | mevcut `HandGlowVFX` prefab tint |

## Renk kuralı
Şekil-kritik VFX'ler çoğu **tek-element** (frost shatter hep frost) → **direkt o renkte çiz**, runtime-tint gerekmez. Sadece generic particle'lar (kod) Palette'ten tint'lenir. Lightning = `#FFE600` (Crit `#FFD24A` DEĞİL).

## Prompt formatı (S87 LOCK)
Tek block, sonunda inline `Negative Prompt :` satırı. Ayrı negative field YOK. İngilizce. Arka plan prompt'a YAZMA (şeffaf zaten).
