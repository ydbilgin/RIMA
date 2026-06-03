# Codex Task — Wall-less Gameplay Mockup Generation

ACTIVE RULES: (1) think before generating (2) min words in report, image quality high (3) use $imagegen built-in tool ONLY (NOT scripts/image_gen.py — no OPENAI_API_KEY), (4) BLOCKED if $imagegen unavailable.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# AMAÇ
User RIMA için duvarsız oyun yaklaşımları araştırdı (Antigravity + Codex + Opus 3-agent). 3 farklı approach mevcut. User şimdi her birinin **nasıl GÖRÜNECEĞİNİ** ve **uygulanabilirliğini** görmek istiyor. Sen Codex olarak `$imagegen` built-in tool ile 4 gameplay mockup üret. Pixel art zorunlu değil — concept art yeterli, design comprehension için.

# RIMA görsel referans (mockup'lara dahil et)
- **Karakter (Warblade):** Chibi pixel art, 120×120 px equivalent (mockup'ta tabii orantılı), kısa zırh, kılıç, dark hair. Top-down 3/4 view 70-80° açı.
- **Tile zemin:** 35° iso pixel art, dark stone cobblestone, ara ara cyan crack + cyan rune circles. Referans screenshot: `STAGING/s106_overnight/playable_arena_35deg_v2.png`
- **Mevcut PixelLab tile set:** floor_iso_pixellab_35deg/ 16 tile (STONE/CYAN_CRACK/DIRT/RUNE 4 grup)
- **Camera:** High top-down 3/4 (Hades, Children of Morta, Diablo III tarz)
- **Atmosphere:** Hollow, ritual-aftermath, dim ambient + cyan accent light + warm brazier accent

# 4 MOCKUP — her biri 1024×576 (16:9) en az

## Mockup 1: Hybrid Hades Elysium (Antigravity's recommendation)
**Senaryo:** Açık arena. Stone floor. 3-4 yıkık antik taş sütun (rounded base, ~2-3 tile tall). 1 köşede ışıklı brazier (warm orange flame, sıcak ışık halkası 3-4 tile radius). Arena kenarında cliff edge (zemin aniden biter, alt katmandı koyu void). NO walls. Warblade chibi karakter ortada combat stance, kılıç havada (downward swing wind-up). 3-4 küçük cyan rune circle yere serpilmiş. Hades Elysium aesthetic.

**Caption:** "Hybrid Hades-style: sparse columns + cliff + light. Premium look, mechanic-light."

## Mockup 2: Pure Object Barriers (Children of Morta style)
**Senaryo:** Aynı stone floor. Walls yok ama YOĞUN object density — 8-12 prop scattered: kırık taş bloklar, ağaç kütükleri, kırık mobilya, sandık, tonel, bone pile. Boundary tamamen objects'lerle çiziliyor. Warblade ortada, 2 düşman yaklaşıyor (skeleton/zombie chibi). Hareketli, kalabalık, ev gibi.

**Caption:** "Pure object barriers — high density, gameplay-rich, asset-heavy."

## Mockup 3: V2 walls + prop dressing (Codex's recommendation)
**Senaryo:** Dungeon room — DUVARLAR var (high top-down 3/4 stone wall, ~1.5 tile tall, perimeter complete). Duvarlara dayalı 4-5 prop: wall torch (warm yellow flame), kırık taş sütun (duvar köşesinde), bir sandık, bir antik altar. Floor stone + birkaç cyan rune. Warblade ortada, melee stance. Klasik dungeon hissi.

**Caption:** "V2 walls + prop dressing — proven, mechanical boundary + decorative depth."

## Mockup 4: Pure Negative Space (Bastion / Hyper Light Drifter style)
**Senaryo:** Floating island — Stone floor ortada, kenarları aniden VOID'e biter, altta yok. NO walls, NO heavy props. Sadece zeminin sınırı = oyun sınırı. Çok az object — 1-2 sütun fragment, 1 brazier. Warblade ortada, dash moment (motion trail). Background = pure black void + dim particles falling. Minimalist, atmospheric.

**Caption:** "Pure negative space — minimalist, atmospheric, dash-tactical."

# Çıktı
1. 4 PNG dosyası: `STAGING/s106_overnight/walless_mockup_v1.png`, `_v2.png`, `_v3.png`, `_v4.png`
2. Kısa karşılaştırma memo: `STAGING/s106_overnight/walless_mockup_REPORT.md`
   - Her variant için 2-3 cümle: visual strengths, gameplay implications, asset cost gut-check
   - Final öneri (sadece görsel olarak, sen seç): hangisi RIMA'ya en iyi UYUYOR (Antigravity/Codex verdict'lerini bilmeden, sadece imaj kalitesine bak)

# Constraints
- $imagegen kullan, scripts/image_gen.py KULLANMA (API key gerektirir)
- 4 imaj 1 dispatch'te peş peşe üret (her birine ayrı $imagegen çağrısı)
- Pixel art style mümkünse hedefle ama zorunlu değil — design comprehension öncelik
- Her imajın kompozisyon merkezi = oyuncu karakteri (Warblade)
- Lighting tutarlı: dim ambient + cyan rune glow + warm brazier accent (varsa)
- Estimated time: 15-25 min

# Report format (kısa)
```
# WALLLESS MOCKUP REPORT - <profile> - <time>

## V1 (Hybrid Hades): <2-3 satır>
## V2 (Object Barriers): <2-3 satır>
## V3 (Walls + dressing): <2-3 satır>
## V4 (Negative Space): <2-3 satır>

## Visual verdict (purely aesthetic):
V<N> — <neden>

## Time: <N> min
```
