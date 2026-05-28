# N3 — Floating-Island Lighting Design FINAL (Opus 4.8, triple-AI CONVERGE)

**Amaç (DÜŞÜN):** duvarsız havada-asılı ada + altı cliff + void ambiyansı için Light2D metodu + üretilebilirlik. ÜRETİM YOK. Triple-AI: Opus + Codex (yasinderyabilgin) + agy (ydbilgin). Kaynak task: `STAGING/N3_LIGHTING_DESIGN.task.md`.

## Işık reçetesi (LOCK — tune play'de)
| Işık | Type | Renk | Intensity | Falloff (in/out) | Target sorting layers |
|---|---|---|---|---|---|
| **Global_Ambient** | Global | `#1E1B2E`–`#262838` | **0.22** | — | Floor, **Decor_Cliff(12)**, **Decor_Floor(13)**, Props, Characters, Gameplay, Wall_Blocker |
| **RimLight_*_Cyan** (W/E/S) | Freeform | `#00FFCC` | **1.2** (Opus orta-karar) | sharp, strength 0.3 | Decor_Cliff(12), Floor edge, Gameplay (kenardaki karakter rim) |
| **Brazier_*_Warm** | Point | `#E89020`/`#C4682A` | 1.0 (flicker ±0.1) | 0.5 / 4.0 | Floor, Decor_Floor, Gameplay, Wall_Blocker |
| **Rune_Pulse_Cyan** | Point | `#00FFCC` | 0.8 (pulse ±0.2) | 0.2 / 2.5 | Floor, Decor_Floor, Gameplay |
| **Void_BG** | unlit parallax (Light2D YOK) | `#3A1A4A`→black | — | — | KitC_BG katmanları — local light target listelerinde OLMAZ |

**Cyan rim intensity uyuşmazlığı çözümü:** Codex 0.45-0.8 (subtle) vs agy 1.4-1.8 (vivid neon). Opus: **1.2 başla, sharp falloff (0.3), kısa erim** — rim sadece kenardaki 1-2px'i yakalasın, ada merkezini aydınlatmasın. Vivid-brand ile over-light arası orta. A5/playtest'te ±0.4 tune.

## Derinlik / "havada asılı" hissi
- Cliff yüzü global ambient alır ama floor kadar sıcak local fill ALMAZ → aşağı indikçe karanlığa düşer.
- **CliffDropShadowTilemap** (sorting < Floor): cliff tile altına %60-70 siyah, top-alpha güçlü→alt 0 vertical gradient. Unlit/multiply — Light2D parlatmaz.
- **Void parallax bg** local light target'ında DEĞİL → combat patlamaları bg'yi aydınlatıp "karton dekor" yapmaz. Arena factor 1.0, bg yavaş → boşlukta-asılı hissi.

## Cliff-rim cyan METOT (bake YASAK)
Hibrit = **Secondary Texture Emission Map (Sprite-Lit) + dynamic Freeform Light2D**. Emission mask'ta sadece dış/void-bakan 1-2px beyaz, gerisi siyah → materyalde `#00FFCC` ile çarpılır = pixel-perfect neon glow, sıfır perf yükü. Dynamic RimLight kenara yaklaşan karakteri ayrıca aydınlatır. (Codex alt: additive rim sprite layer — fallback.)

## 🔴 SAÇMALIK / RİSK TESPİTİ (triple-AI)
1. **Black-cliff KÖK NEDEN (agy + Codex):** Lit material + `Decor_Cliff` hiçbir Light2D `m_ApplyToSortingLayers`'ında yok → 0 ışık → siyah. ÇÖZÜM: layer değiştirme DEĞİL, tüm ışıkların target listesine Decor_Cliff(12)+Decor_Floor(13) ekle.
2. **Işık parent'ı (agy KRİTİK):** RimLight/Brazier ışıkları `RIMA_Cycle2_Dressing` (dekoratif) child'ı → parent inaktif olunca ışık söner = tasarım hatası. ÇÖZÜM: ışıklar bağımsız `Scene_Lighting` GO altında, dekor objesi child'ı DEĞİL. **← uygulanabilir SİSTEM fix (demo-blocker).**
3. **Shadowcaster2D perf (agy):** organik (kare-olmayan) tilemap'te binlerce shadow poly = perf çöküşü. ASLA dinamik URP 2D shadow; düşen gölge sprite-based CliffDropShadowTilemap ile statik.
4. **pixelSnapping OFF → tile-seam (agy):** kılcal şeffaf çizgi riski → tile sprite kenarında 1px pixel-bleed veya tilemap renderer edge mode.

## ÜRETİLECEKLER (spec, ÜRETİLMEDİ — hazır olsun)
| Asset | Boyut | Tip | Araç |
|---|---|---|---|
| Floor/Cliff Emission Mask | 32×32 (tile 1:1) | grayscale, dış kenar 1-2px beyaz | Python edge-extract script (scratch/), PixelLab GEREKMEZ |
| void_gradient_bg | 1×256 stretch / 512×512 | unlit, #3A1A4A→#1C162E→black | Python Pillow gradient |
| cyan_rim_edge_strip (fallback) | 128×32 / 256×32 | additive transparent, cyan edge glow | Python/manual |
| rune_glow_mask | 128×128 | additive transparent, cyan radial | Python |
| brazier_glow_mask | 128×128 | additive transparent, amber radial | Python |
Hepsi 64 PPU, point filter, no compression, no mipmaps.

**Uygulama notu:** Bu DESIGN. Black-cliff fix (#1+#2: Decor_Cliff target + Scene_Lighting GO) = SİSTEM işi → demo-visual task'ına alındı. Asset üretimi (emission mask vb.) Python-cheap, animasyon-bağımsız → kullanıcı onayıyla istendiğinde.

**Index:** `reference_floating_island_lighting_n3` → ışık reçetesi + 4 saçmalık + üretilebilirlik.
