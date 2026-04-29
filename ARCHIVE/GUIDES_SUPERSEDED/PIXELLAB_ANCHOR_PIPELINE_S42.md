# PixelLab Anchor Pipeline — S42

**Tarih:** 2026-04-26 · **Faz:** 1 · **Status:** AKTİF
**Önceki recipe ile değişim:** Refsiz roster Pro → **Create from Reference Standard + Anchor chaining**.

---

## 0. Amaç

10 class + 6 mob/boss = **16 base sprite** üretmek.
Her base sprite **64×64**, **High Top-Down** kamera, **single south-facing direction**, **transparent background**, **PixelLab `character_id` üretimi** (MCP animation için zorunlu).

8-yön ve animasyon **bu aşamada YOK** — anchor PASS'ten sonra ayrı.

---

## 1. Pipeline Genel Bakış

```
Aşama A — Warblade Pilot (Hero Siege ref)
   ↓ PASS
Aşama B — Warblade PNG = ANCHOR #1 (RIMA Male Combat)
   ↓
Aşama C — Elementalist (Hero Siege ref)
   ↓ PASS
Aşama D — Elementalist PNG = ANCHOR #2 (RIMA Female Mage)
   ↓
Aşama E — Kalan 14 karakter (Anchor #1 veya #2 ref'i ile)
   ↓ Tümü PASS
Aşama F — 8-yön rotate (PixelLab UI veya MCP)
   ↓
Aşama G — Animation (MCP `animate_character` via character_id)
```

**Her PASS character_id verir.** Bu id'ler bir liste'de tutulur (`STAGING/character_ids.md`).

---

## 2. UI Settings — Create from Reference Standard

### Sabit field'lar (her run için)

| Alan | Değer |
|---|---|
| Tab | **Create from Reference** |
| Generation Mode | **Standard** |
| Character Type | **Humanoid** (mob #11-14 farklı olabilir, aşağıda) |
| Character Size | **64px** |
| Camera View | **High Top-Down** ⭐ (kamera kilidi) |
| Quick Preset | **Heroic** (chunky readable, NOT chibi) |
| Reference image | Class'a göre değişir (aşağıda tablo) |

### Heroic preset yetmezse Custom slider'lar
- Head Size: **1.0–1.1×** (okunabilir yüz)
- Shoulder Width: **1.1–1.2×** (chunky Hero Siege hissi)
- Hip Width: **1.0–1.1×**
- Arms Length: **1.0×**
- Legs Length: **1.0×**

### Quick Preset notları
- **Default** — düz humanoid, RIMA için fazla zayıf
- **Chibi** — büyük kafa küçük gövde — **YASAK**
- **Cartoon** — abartılı oranlar — RIMA'ya uymaz
- **Stylized** — orta yol, denenebilir
- **Realistic Male/Female** — gerçekçi oran — 64px'te detay kaybolur — kullanma
- **Heroic** — ✅ **default seçim**

---

## 3. Reference image seçimi

### Hero Siege ref havuzu (`STAGING/chatgpt_pixel_grid_s42/clean_outputs/`)

| Dosya | Görsel |
|---|---|
| 01_blue_armor_knight | Mavi plate knight |
| 02_viking | Boynuzlu Viking, kahverengi |
| 03_green_amazon | Yeşil amazon, sportif |
| 04_butcher | Pembe-beyaz cellat figürü |
| 05_demon_slayer | Beyaz saçlı demon slayer |
| 06_jotunn_mage | Mor robe büyücü |
| 07_plague_doctor | Beyaz mask plague doctor |
| 08_shield_lancer | Beyaz mask shield lancer |

### Class → Ref tablosu (TAM)

**Aşama A-D (anchor pilotları):**

| Class | Ref | Sebep |
|---|---|---|
| **Warblade** (Anchor #1 pilot) | `01_blue_armor_knight` | Plate + ağır silah arketipi |
| **Elementalist** (Anchor #2 pilot) | `06_jotunn_mage` | Robe + staff kadın mage |

**Aşama E (Anchor chaining — Hero Siege ref'leri BIRAK):**

| Class | Ref olarak ne kullan |
|---|---|
| Ravager | `STAGING/anchors/anchor_male_warrior.png` (Warblade PASS) |
| Ronin | `STAGING/anchors/anchor_male_warrior.png` |
| Shadowblade | `STAGING/anchors/anchor_male_warrior.png` |
| Brawler | `STAGING/anchors/anchor_male_warrior.png` |
| Hexer | `STAGING/anchors/anchor_female_mage.png` (Elementalist PASS) |
| Summoner | `STAGING/anchors/anchor_female_mage.png` |
| Ranger | `STAGING/anchors/anchor_female_mage.png` (mage değil ama kadın combat oran kaynağı) |
| Gunslinger | `STAGING/anchors/anchor_female_mage.png` |
| Chain Warden (mob) | `STAGING/anchors/anchor_male_warrior.png` |
| Mini-Boss Penitent | `STAGING/anchors/anchor_male_warrior.png` |

**Non-humanoid mob (Quadruped veya humanoid ama farklı arketip):**

| Mob | Character Type | Ref |
|---|---|---|
| Fracture Imp | Humanoid | `STAGING/anchors/anchor_male_warrior.png` (oranı küçült manuel sonradan) |
| Relic Caster | Humanoid | `STAGING/anchors/anchor_female_mage.png` (skeleton/mask değişimi prompt'tan) |
| Seam Crawler | **Quadruped** (centipede) | Hero Siege ref YOK uygun → **kendi pilot** (refsiz veya 04_butcher heavy ref) |
| Shard Walker | Humanoid (taş golem) | `STAGING/anchors/anchor_male_warrior.png` (chunky şişir) |

---

## 4. Description prompt yapısı

Structured field'lar (camera, size, outline, proporsiyon) prompt'tan **ÇIKARILDI** — UI handle ediyor. Prompt sadece **kimlik** anlatır.

### Genel template

```
[CINSIYET] [ARKETIP TANIM].
[YÜZ TANIMI — saç rengi/stil, ten, ifade, sakal/maske].
[KIYAFET — armor/robe/leather, materyal, üst-alt parçaları, accessory].
[SILAH — eldeki silah(lar), nasıl tutuluyor, hangi el].
Color palette: [DOMINANT 2-3 RENK + ACCENT]. No [YASAK RENKLER].
Face must be fully visible — [helmet/mask/hood durumu].
Grounded serious dark fantasy. Heavy readable silhouette. Sharp pixel clusters. Limited palette 2-3 shade steps per material. No dithering. No painterly soft rendering. No cute chibi.
Transparent background. No text. No labels. No UI.
```

### Warblade örnek (referans)

```
Male disciplined battle-worn human warrior, mature serious face, short dark hair, light beard.
Heavy dark steel plate armor over warm brown leather straps. Layered shoulder pauldrons. Battle-worn metal with subtle scratches. Brown leather belt and bracers.
Holds a massive two-handed greatsword resting against his right shoulder, blade pointing up and slightly back. Both hands on the hilt.
Color palette: dark steel gray, warm brown leather, ember orange and silver accents only. No bright blue magic. No purple glow. No green.
Face must be fully visible — no helmet covering the face, no full-face mask. Clean readable face even at small size.
Grounded serious dark fantasy. Heavy readable silhouette. Sharp pixel clusters. Limited palette 2-3 shade steps per material. No dithering. No painterly soft rendering. No cute chibi. No childish proportions.
Transparent background. No text. No labels. No UI.
```

### Diğer 9 class + 6 mob prompt'ları

Mevcut roster prompt'undan al: `STAGING/ROSTER_PROMPT_S42.md` cell #1-#16. Her cell bloğu **structured field'larla çakışan satırları çıkar** (camera, size, outline, no chibi tekrarları). Sadece kimlik kalır.

**Çakışan satırlar (HER promptan ÇIKAR):**
- "Top of head visible" / "high top-down" / "camera above" → field
- "64x64" / "fills 75-85%" → field
- "Strong dark outline" → field
- "compact heroic human proportions" → preset
- "transparent background" → kalsın (negatif emniyet)

---

## 5. Workflow — Aşama A (Warblade Pilot)

1. PixelLab UI → **Create Character** sayfası
2. Tab: **Create from Reference**
3. Generation Mode: **Standard**
4. Character Type: **Humanoid**
5. Reference Image yükle: `STAGING/chatgpt_pixel_grid_s42/clean_outputs/01_blue_armor_knight_64x64_transparent.png`
6. Character Size: **64px**
7. Camera View: **High Top-Down**
8. Quick Preset: **Heroic**
9. Description: yukarıdaki Warblade prompt'unu yapıştır
10. **Run**
11. Output → 5 kapı QC (aşağıda)
12. PASS → `character_id`'yi kopyala + PNG'yi indir → `STAGING/anchors/anchor_male_warrior.png`
13. FAIL → tweak + re-roll (tweak rehberi aşağıda)

### Aşama B — Anchor #1 Lock

PASS Warblade PNG'yi `STAGING/anchors/anchor_male_warrior.png` olarak kaydet.
`character_id`'yi `STAGING/character_ids.md` dosyasına yaz: `Warblade: <id>`.

### Aşama C — Elementalist Pilot

Aşama A ile aynı; tek farklar:
- Reference Image: `06_jotunn_mage_64x64_transparent.png`
- Description: ROSTER_PROMPT_S42.md cell #2 (Elementalist) — kimlik bloğu (kamera/size satırları çıkarılmış)

### Aşama D — Anchor #2 Lock

PASS PNG → `STAGING/anchors/anchor_female_mage.png`. id'yi listeye ekle.

### Aşama E — Kalan 14 karakter

Class tablo'suna göre Anchor #1 veya #2'yi ref olarak yükle. Description = kimlik. Run.

PASS sırası önerisi (kolay → zor):
1. Ravager (Anchor #1) — strong silüet, kolay
2. Ronin (Anchor #1) — temiz disiplinli
3. Brawler (Anchor #1) — orta
4. Shadowblade (Anchor #1) — pose riski (predator crouch)
5. Hexer (Anchor #2) — **CRITICAL** human face (skull risk)
6. Summoner (Anchor #2) — **CRITICAL** human face
7. Ranger (Anchor #2) — bow detay
8. Gunslinger (Anchor #2) — **CRITICAL** dual-pistol
9. Chain Warden (Anchor #1) — chains identity
10. Mini-Boss Penitent (Anchor #1) — chained ritual
11. Fracture Imp (Anchor #1, küçült)
12. Relic Caster (Anchor #2, robe + reliquary)
13. Shard Walker (Anchor #1, golem)
14. Seam Crawler — **kendi pilot** (Quadruped, refsiz)

---

## 6. QC — 5 Kapı

Her PASS şu beş kapıdan geçmeli. Eksiklik = FAIL.

| Kapı | Kriter |
|---|---|
| **1. Kamera** | Kafanın üstü görünüyor mu? Omuz düzlemleri yukarıdan mı? Yüz hafif foreshortened mi? Eye-level/portrait/side-view ise FAIL. |
| **2. Identity** | Class'ın temel tanım maddeleri (silah, palette, kıyafet, yüz) prompt'taki gibi mi? Sapma >%20 ise FAIL. |
| **3. Palette** | Locked dominant renkler doğru mu? Yasak renk (örn. Warblade'de mavi/mor) sızdı mı? Sızdıysa FAIL. |
| **4. Face quality** | Yüz simetrik mi, melted/duplicate feature yok mu, gözler stabil mi? FAIL ise re-roll. |
| **5. Outline + Pixel hijyen** | Tek renk koyu outline mı? Pixel kümeleri net mi, anti-alias bulanıklığı yok mu? Dithering/noise YOK mu? |

PASS eşiği: 5/5. 4/5 = "tweak ile düzelir mi" yargısı (palette tek noktada sızdıysa düzeltirsin; identity bozuksa re-roll).

---

## 7. FAIL Tweak Rehberi

| Sorun | Çözüm |
|---|---|
| Kamera eye-level kaymış | Camera View **High Top-Down** mı kontrol et. Hâlâ kayıyorsa Description'a tek satır: `Top of head and shoulder caps clearly visible from above.` |
| Yüz melted / asymmetric | Description'a: `Face must be clean, symmetric, centered. No melted features.` Re-roll. |
| Palette sızıntı (yasak renk) | Negatif vurgu güçlendir: `Absolutely no [renk]. No [renk] glow.` 2-3 yerde tekrarla. |
| Çok zayıf gövde (chibi değil ama ince) | Custom slider Shoulder Width 1.2x, Heroic preset reset. |
| Çok büyük kafa (chibi gibi) | Heroic preset'e dön (Chibi yanlışlıkla seçilmiş olabilir). Custom Head Size 1.0x. |
| Silah yanlış (örn. Gunslinger tek pistol) | Description'da CRITICAL bloğunu güçlendir, "REJECTED" tekrarla. Re-roll. |
| Maske/skull gelmemesi gereken yerde geldi | Pozitif spec kullan: "fully visible normal HUMAN face. visible eyes nose mouth cheeks chin." |

3 re-roll'da PASS gelmezse: ref görseli değiştir veya Description'ı yeniden yapılandır.

---

## 8. 8-Yön (Anchor PASS sonrası, ANIMATION aşamasında)

**Bu aşamada (anchor üretim) 8-yön YOK.** Base = single south-facing.

Anchor 16 karakter PASS sonrası iki yol:

### Yol 1 — PixelLab UI Rotate
- Karakteri aç → "Rotate" özelliği → 4 veya 8 yön grid üret
- 8 yön sırası (PixelLab CCW konvansiyonu — `reference_pixellab_direction_sequence.md`):
  - **NEW1 = S** (south, facing camera)
  - **NEW2 = SW**
  - **NEW3 = W**
  - **NEW4 = NW**
  - **NEW5 = N** (kuzey, sırtı dönük)
  - **NEW6 = NE**
  - **NEW7 = E**
  - **NEW8 = SE**

### Yol 2 — MCP `animate_character` (önerilen)
- character_id ile direkt anim üret
- MCP her yön için iç rotation yapıyor — Run/Attack/Idle/Hit/Death için her birinde 4 veya 8-dir döner
- Avantaj: tek MCP call, tutarlı rotation + anim
- RIMA hedef: **4-yön (S/E/N/W)** yeterli (top-down ARPG için diagonal yaygın değil); budget kalırsa 8-yön

### Animation spec (memory'den)
- **Run:** V3 mode, **8 frame**, Enhance OFF, Keep First ON (`feedback_enhance_action_default.md`)
- **Attack/Skill:** 3-segment (PEAK önce), Custom Frames Start+End=KF + Interpolate (`feedback_combat_animation_workflow.md`)
- **Idle:** 4-6 frame loop, basit nefes
- **Hit:** 3-4 frame, knockback flinch, 4-yön (`#48 Hit/Death 4-yön` karar)
- **Death:** 6-8 frame, ground collapse, 4-yön
- **First+Last Interpolate:** her anim first+last frame KF, ortayı Interpolate doldurur (`feedback_first_last_interpolate.md`)

---

## 9. Klasör yapısı

```
STAGING/
  anchors/
    anchor_male_warrior.png       (Warblade PASS)
    anchor_female_mage.png        (Elementalist PASS)
  character_ids.md                (PASS id listesi)
  chatgpt_pixel_grid_s42/
    clean_outputs/                (Hero Siege ref havuzu — PILOT'LARDAN SONRA KULLANILMAZ)
  base_sprites_s42/               (16 karakter PASS PNG'leri — yeni klasör)
    01_warblade.png
    02_elementalist.png
    ...
    16_penitent.png
  ROSTER_PROMPT_S42.md            (cell prompt kaynağı, kimlik blokları için)
```

PASS sonrası 16 PNG → `Assets/Sprites/Characters/Base/` (Codex import).

---

## 10. character_ids.md format

```markdown
# RIMA Character IDs (S42)

## Class (10)
- Warblade: <id>
- Elementalist: <id>
- Shadowblade: <id>
...

## Mob/Boss (6)
- Fracture Imp: <id>
- Relic Caster: <id>
...
```

Bu liste MCP `animate_character` aşamasında girdi.

---

## 11. Animation aşaması (sonraki guide)

Anchor + 16 base PASS olunca ayrı guide yazılır: `GUIDES/PIXELLAB_ANIMATION_PIPELINE_S42.md`. Bu aşamada:
- MCP schema doğrula (ToolSearch → `mcp__pixellab__animate_character`)
- Per character_id: Run → Attack → Idle → Hit → Death
- Per direction: 4 yön (S/E/N/W) öncelik, budget varsa 8
- Output → Codex import + Aseprite composite + Unity .anim build

---

## 12. Sorun → Eskalasyon

- 3 re-roll FAIL → ref değiştir
- 5 re-roll FAIL → recipe revize (description yeniden, preset değişimi)
- Anchor #1 PASS gelmiyor → Hero Siege ref havuzundan farklı bir crop dene (#02 viking, #04 butcher)
- Anchor #2 PASS gelmiyor → ref olarak Anchor #1 dene + identity prompt çok güçlü kadın spec
- Tüm pipeline tıkanırsa → CURRENT_STATUS.md'ye log + kullanıcıya bildir

---

## Özet Checklist

- [ ] Hero Siege ref havuzu mevcut (`STAGING/chatgpt_pixel_grid_s42/clean_outputs/`)
- [ ] `STAGING/anchors/` klasörü oluştur
- [ ] Aşama A: Warblade pilot (Reference Standard + 01_blue_knight)
- [ ] Aşama B: Anchor #1 PASS lock
- [ ] Aşama C: Elementalist pilot (Reference Standard + 06_jotunn_mage)
- [ ] Aşama D: Anchor #2 PASS lock
- [ ] Aşama E: 14 karakter sırayla (kolay→zor)
- [ ] character_ids.md güncel
- [ ] 16/16 PASS → Animation guide aç (ayrı dosya)
