# Warblade Pro Forge — Anchor #1 (S42 v4, Aşama 0)

**Tarih:** 2026-04-26 · **Pipeline:** v4 Pro Identity Forge → Aşama 0
**Amaç:** Hero Siege ref olmadan, RIMA Warblade identity'sini Create Image Pro ile doğrudan forge et. Çıktı = 3×3 sprite sheet (8 yön + boş orta). Her hücre = 1 direction anchor. South hücresi = Anchor #1 (Aşama A girişi).

---

## 1. UI Settings

| Alan | Değer |
|---|---|
| Mode | **Create Image Pro** |
| Style slot | `_STAGING/chatgpt_pixel_grid_s42/clean_outputs/warblade/` içindeki en iyi Hero Siege ref (kamera açısı iyi olanı) — sadece style referansı olarak, identity için DEĞİL |
| Concept slot | Aşağıdaki Description bloğunu yapıştır |
| Output size | **256×256** (512 varsa: 512 — nearest-neighbor ile 64'e indirilecek) |
| Background | **Transparent** |
| Karakter adı / proje adı | **YAZMA** (text leak riski) |

> Style slot yoksa (Pro Rotate modundaysa): sadece Description yeterli, ref yükleme.
> Output 128-256px arası beklenir. Kullanıcı Aseprite/Photoshop nearest-neighbor ile 3×3 grid'i 192×192'ye (64×64 per cell) indirir.

---

## 2. Sheet Layout

3×3 grid. Orta hücre (5. hücre) boş. 8 çevre hücre = 8 yön. CCW sıra (PixelLab NEW sequence ile örtüşen):

```
┌──────┬──────┬──────┐
│  NW  │  N   │  NE  │
├──────┼──────┼──────┤
│  W   │      │  E   │
│      │ BOŞ  │      │
├──────┼──────┼──────┤
│  SW  │  S   │  SE  │
└──────┴──────┴──────┘
```

| Grid Pozisyon | Yön | Açıklama |
|---|---|---|
| Üst-sol | NW | Kuzeybatı — arkadan sol köşe |
| Üst-orta | N | Kuzey — tam arka |
| Üst-sağ | NE | Kuzeydoğu — arkadan sağ köşe |
| Orta-sağ | E | Doğu — tam sağ profil |
| Alt-sağ | SE | Güneydoğu — ön-sağ köşe |
| Alt-orta | S | **Güney — tam ön (ANA ANCHOR)** |
| Alt-sol | SW | Güneybatı — ön-sol köşe |
| Orta-sol | W | Batı — tam sol profil |
| Orta | — | BOŞ (transparent) |

Her hücre kare, eşit boyut, transparent arka plan. Hücreler arası padding minimal veya sıfır (kesme işlemini kolaylaştırır).

> **N/NW/NE hücreleri back-of-head gösterir — bu normaldir.** Warblade kameraya bakmaz, ileri bakar.

---

## 3. Description (kopyala-yapıştır)

```
SPRITE SHEET FORMAT: 3x3 grid of 8 directional views. Center cell empty (transparent). Cells arranged as: top-left=NW, top-center=N, top-right=NE, middle-right=E, bottom-right=SE, bottom-center=S, bottom-left=SW, middle-left=W. SAME IDENTICAL CHARACTER in all 8 cells — only the facing direction changes. Identical armor, weapon, hair, palette, proportions, and silhouette across all 8 cells. No variation in design between cells.

Male battle-worn human warrior. Mature serious face. Short dark hair. Light beard. Medium-stocky build, adult male proportions — not a child, not a caricature. Face clearly adult and grounded.

Heavy dark steel plate armor over warm brown leather straps and padding. Layered shoulder pauldrons with battle-worn scratches. Brown leather belt, bracers, and boot wraps. Metal shows age, not shine.

Holds a massive two-handed greatsword resting on his right shoulder, blade angled up and back. Both hands on the hilt. In side/back-facing cells the greatsword orientation adjusts to match the direction — weapon remains identifiable in all cells.

Color palette: dark steel gray, warm brown leather, ember orange and silver accents only. No bright blue magic, no glowing blue. No purple. No green. Muted desaturated field-worn tones — no neon, no bloom.

Face fully visible in S, SW, SE, E, W cells. In N, NW, NE cells the back of the head and hair are visible — this is correct and expected, do not add a face there.

Camera angle: ~60° from horizontal (~30° from vertical/zenith). Hero Siege / Diablo 2 high top-down ARPG gameplay camera. Camera above. Top of head and hair clearly visible in all cells. Forehead and hair-top more prominent than lower face. Eyes small and high on face — NOT centered like a portrait. Nose and mouth tiny, simplified. Chin compressed. Upper shoulder planes visible from above. Torso slightly compressed by camera angle. Feet lower in frame, slightly smaller.

Avoid: southeast 3/4 diagonal. Eye-level character select view. Flat front paper-doll. Pure side view. Pure bird's-eye 90 degree. Portrait orientation.

Grounded dark fantasy warrior. Heavy readable silhouette. Sharp pixel clusters. Limited palette 2-3 shade steps per material. No dithering. No painterly soft rendering. No cute chibi. No childish proportions. No glowing magic hands.

Pixel art. Transparent background per cell. All 8 cells same character, same camera angle, only direction changes.

ABSOLUTELY NO TEXT. NO LETTERS. NO LABELS. NO NUMBERS. NO UI ELEMENTS. NO WATERMARKS. PIXEL ART ONLY.
```

---

## 4. Workflow (adım adım)

1. PixelLab UI → **Create Image Pro** sekmesine gir.
2. Style slot'a (varsa) Hero Siege ref yükle — sadece style/kamera açısı için, karakter identity'si Description'dan gelecek.
3. Description bloğunu concept slot'a yapıştır.
4. Output size: **256** (veya maks desteklenen).
5. Background: **Transparent**.
6. **Run** — Pro mode `character_id` vermez, sadece PNG çıktı döner.
7. Output'u incele: 3×3 grid formatında mı? 8 hücrede aynı karakter mi? Kamera ~60° mü?
8. PASS → PNG'yi `_STAGING/pro_forge_output/warblade_grid_raw.png` olarak kaydet.
9. **Aseprite veya Photoshop'ta:**
   - Grid'i aç, her hücreyi ayrı layer/crop olarak kes.
   - Her hücreyi **64×64 nearest-neighbor** downsample yap (bicubic/smooth YASAK).
   - Her yönü ayrı PNG olarak kaydet:
     - `_STAGING/anchors/anchor_male_warrior_S.png` ← **Anchor #1 (ANA REF)**
     - `_STAGING/anchors/anchor_male_warrior_SW.png`
     - `_STAGING/anchors/anchor_male_warrior_SE.png`
     - `_STAGING/anchors/anchor_male_warrior_W.png`
     - `_STAGING/anchors/anchor_male_warrior_E.png`
     - `_STAGING/anchors/anchor_male_warrior_NW.png`
     - `_STAGING/anchors/anchor_male_warrior_N.png`
     - `_STAGING/anchors/anchor_male_warrior_NE.png`
10. **South (S) hücresi = Anchor #1** → Aşama A (Create Character → Create from Reference Standard) girişi.
11. Diğer 7 yön = Aşama F atlıyor (8 yön anchor elimizde).

---

## 5. QC — 6 Kapı

| # | Kriter | PASS Kriteri |
|---|---|---|
| 1 | **Kamera** | Her hücrede head-top görünür, eye-level/portrait kayma yok |
| 2 | **Identity per-cell** | Her hücrede: greatsword + dark steel + ember palette tanınıyor |
| 3 | **Palette** | Blue glow / purple / green sızıntısı yok. Muted field-worn tonlar |
| 4 | **Face** | S/SW/SE/W/E hücrelerinde net yüz, melted değil. N/NW/NE arka kafa = OK |
| 5 | **Outline** | Tek renk koyu strong outline, tüm hücrelerde tutarlı |
| 6 | **Cross-cell consistency** | 8 hücrede aynı zırh/silah/saç/oran — cell-to-cell drift yok |

**PASS:** 6/6 · **Tweak:** 5/6 · **Re-roll:** ≤4/6 · 3 re-roll sonrası FAIL → Fallback'e geç.

---

## 6. FAIL Fallback

**Senaryo A — Pro mode 3×3 grid formatını reddederse:**
Sheet format direktifi görmezden gelinmiş, tek bir south-facing karakter çıkmış.

> Tek S facing çıktı kabul edilir = **Anchor #1 (S) only**. Kalan 7 yön Aşama F'te (PixelLab UI Rotate veya MCP `animate_character`) üretilir.
> South PNG → `_STAGING/anchors/anchor_male_warrior_S.png` → Aşama A'ya devam.

**Senaryo B — Text leak (hücrelerde isim/label render edilmişse):**
Description'daki "NO TEXT" bloğunu daha öne al, run'u tekrarla. 2. denemede de leak varsa: style ref slot'u boşalt (yalnız prose çalıştır).

**Senaryo C — Identity yok, Hero Siege mavi şövalye benzeri flat çıktı:**
Style slot'tan Hero Siege ref'i çıkar. Yalnız Description ile re-roll. Pro mode'da ref'in identity'yi geçersiz kılması olası — refsiz çalıştır.

---

## 7. Sonraki Adım

**PASS + 8 yön anchor kaydedildikten sonra:**

- `_STAGING/anchors/anchor_male_warrior_S.png` = Aşama A girişi (Create Character → Create from Reference Standard)
- Aşama A → `character_id` döner → `_STAGING/character_ids.md`'ye yaz
- Aşama F atlanır (8 yön zaten var)
- Aşama G: MCP `animate_character` via `character_id` — idle, run, attack, skill animasyonları
- Pro Forge `character_id` vermez — sadece Aşama A'nın Reference Standard adımı verir.
- Elementalist için aynı süreç: Aşama C-D (Anchor #2).
