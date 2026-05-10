# PixelLab Production Playbook
*Tek dosya, sırayla. Aç → Adım 1 → bitir → Adım 2.*
*Son güncelleme: 2026-05-10 (NLM çapraz-kontrol — PixelLabDocs/63 dosya ile doğrulandı)*

> **2026-05-10 NLM-FIX changelog:**
> - **HIGH-1 — Frame parity (4 yer):** Hurt anim'lerinde "3 frame" → **4 frame**. (Adım 19/28/37/46) Sebep: `animate-with-text-new.md` çift sayı zorunlu (4/6/8/10/12/14/16).
> - **HIGH-3 — Obstacle tool (8 yer + bölüm başı):** Adım 9-16 "Create Image S-XL (new)" → **Create S-L Image (Pro)**. Sebep: S-XL/Pixflux tek görsel verir; grid varyasyon Pro'da. Var sayıları gen×grid mantığına çevrildi.
> - **MEDIUM-4 — Transition tile (Adım 7-8):** "Create Tiles Pro + style ref" → **Create Tileset Standard** (Top-Down inner+outer). Sebep: `create-tileset.md`'ye göre Wang set / seamless transition standart tool'da.
> - **HIGH-2 — Interpolation 252px ✅ DOĞRULANDI (2026-05-10):** Kullanıcı UI testiyle Interpolate NEW v2'nin 252×252 (max 256×256) desteklediğini onayladı. PixelLabDocs/`interpolation.md` ESKİ — v1 limiti yazıyor. Playbook'un "v2 252px destekli" ifadesi geçerli.
> - **Doğrulananlar (değişiklik gerekmedi):** animate_character MCP yasağı, pixel budget formülü (524,288), Edit Image Pro weapon-pass (3-4 frame'lik anim tool'larının limiti yetmediği için manuel propagation doğru).
> - **F. Padding/Scale kuralı LOCKED (2026-05-10, Gemini önerisi + NLM doğrulaması):** Yeni Bölüm F eklendi. 4 base prompt'a (Adım 17/26/35/44) `~50% of canvas height + DO NOT fill canvas` constraint'i işlendi. PixelLab v3 motoru zaten %60 padding bırakıyor (resmi davranış); prompt-level kısıt çakışmayı önler. "Devasa boss" stratejisi PPU=32 ile (asla 256+ üretim YOK).
> - **Çelişki düzeltildi:** `TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` Final Boss "512px Canvas, PPU=64" satırı `256×256, PPU=32` olarak revize edildi (animate-with-text-new max 256px destekliyor — 512 imkansız).

> **Kullanım:** Bu dosyanın sırasını takip et. Her adım için **tool**, **ayarlar**, **prompt** (kopyala-yapıştır), **kaydet path**'i ve **process komutu** verilmiş. Bittikçe `[ ]` → `[x]` işaretle.

> **REPO_ROOT:** `F:/Antigravity Projeler/2d roguelite/RIMA`
> Tüm path'ler bu kökten relative.

---

## 📌 REVIZE NOTU — Kanonik Pipeline Kuralları (2026-05-10)

NLM çapraz kontrolü + Opus kararı sonucu aşağıdaki kurallar tüm dosyaya geçerlidir. Adım metinleri eski tool isimleri/format kullanıyorsa, bu bölüm KAZANIR.

### A. Tool Versiyonları (Codex/Antigravity directive LOCKED 2026-05-10)

> **HARD RULE — sadece NEW / PRO / FLUX:** RIMA asset üretiminde **legacy tool YASAK**. Tool adında "NEW", "PRO", veya "FLUX" yoksa → kullanma. Memory: `feedback_pixellab_new_pro_flux_mandate.md`.

- **"Animate with Text NEW"** = v3 Pixflux engine. Eski v2 araçlar %49 çöküyor — kullanma.
- **"Interpolate NEW"** = Interpolate NEW v2 — **UI'da 252×252 destekleniyor (max 256×256), kullanıcı 2026-05-10 testiyle doğruladı**. ⚠️ **PixelLabDocs/`interpolation.md` ESKİ:** "canvas must be exactly 64×64" diyor ama bu eski v1 limiti; v2 (NEW) 16-256px aralığını destekliyor. Doc'a güvenme, NEW'i kullan.
- **"Animation-to-Animation Bridging Mode"** — alternatif. KF1 + KF3 → 2 ara frame. Geniş silahlı sınıflarda (Warblade greatsword) Interpolate NEW kırpma yaparsa bunu kullan.

### B. Canvas Standardı
- PixelLab v3 araçları **252×252 ZORUNLU** (silah/uzuv'un kanvas dışına taşmasını engeller, RIMA'nın seçimi değil)
- Unity'ye **128×128 frame** olarak kırpılarak import edilir (PPU=64)
- Sprite slice: 252×252 üretim → Aseprite'ta merkez 128×128 crop → Unity multiple sprite

### C. Pixel Budget Formülü — ZORUNLU KONTROL
`width × height × frame_count ≤ 524,288`

| Canvas | Max Frame | RIMA Kullanımı |
|---|---|---|
| 252×252 | **8** | Standart (silahlı karakterler) |
| 160×160 | 16 | Silahsız karakterler için minimum safe |
| 128×128 | 16 | Silah taşıyan karakterde kırpma riski (kullanma) |

**Bütçe aşımı = generation hatası VEYA kalite düşüşü.** Frame sayısını planla.

### D. animate_character MCP — KALICI YASAK
2026-05-02 kararı. Karakter animasyonları sadece Web App'te üretilir.
**MCP OK:** tile, static prop, batch base 4-yön rotation.
**MCP YASAK:** idle, walk, run, attack, hurt, death, dash.

### E. Attack Animation Budget Uyarısı
Eski plan: START + PEAK + END = 1 + 4 + 4 = **9 frame** → 252² × 9 = 571,536 → **BÜTÇE AŞIMI**.

**Yeni format (zorunlu) — Seçenek 1:** PEAK frame'i windup'ın son frame'i ve follow'un ilk frame'i olarak paylaş:
- Windup: 4 frame (son frame = PEAK)
- Follow: 4 frame (ilk frame = PEAK, paylaşılır)
- Toplam Aseprite'ta: **8 unique frame**

**Seçenek 2:** İki ayrı Unity clip:
- `Attack_Windup` (4 frame) + `Attack_Follow` (4 frame) — Animator'da chain

Bu kural Adım 22, 23, 31, 32, 40, 41, 49, 50 için geçerli.

### F. Karakter/Boss Scale & Padding Kuralı (LOCKED 2026-05-10)

**Kaynak:** Antigravity Gemini önerisi + NLM doğrulaması (`mcp_docs.md`, `pixellab_api_reliability.md`, `animation.md`).

**1) Canvas tavanı:** Animate with Text NEW max **256×256**. RIMA standardı **252×252** (8 frame için pixel budget içinde). 256+ üzerine çıkmak imkansız — tool çöker.

**2) %60 padding zorunlu:**
- PixelLab v3 motoru zaten otomatik **karakteri canvas yüksekliğinin ~%60'ında** konumlandırıyor (`mcp_docs.md` resmi davranış: "*Canvas size is total area; character will be ~60% of canvas height*").
- 256×256 çıktıda gerçek karakter ≈ **168×168 px** (`pixellab_api_reliability.md`). Geri kalan ~%40 transparent padding = silah savurma / VFX / lunge için hayati alan.
- RIMA pratiği: **128px karakter on 252×252 canvas** = ~%50 oran (Gemini minimumundan daha cömert padding — daha güvenli).
- **Negatif kısıt (prompt'larda zorunlu):** "*don't fill canvas, leave wide transparent headroom*". Asla "fill canvas" / "occupies entire frame" yazma — modelle kavga edersin, kötü çıktı.

**3) "Devasa boss" stratejisi — PixelLab'i zorlama, Unity'de scale et:**

| Boss tipi | PixelLab üretim | Unity PPU | Unity ekran | Player'a oran |
|---|---|---|---|---|
| Player (referans) | 252×252, karakter ≈128px | 64 | ~2.0 unit | 1× |
| Miniboss | 252×252, karakter ≈128-160px | 128 | ~1.0 unit görsel ama büyük gözükür | 2× hissi |
| Act Boss | 252×252, karakter ≈160px | 64 | ~2.5 unit | ~3-4× hissi |
| **Final Boss** | **252×252** (ASLA 512+!) | **32** | ~5 unit | **~6× hissi (devasa)** |
| Architect canavar form | 256×256 | 32 | ~8 unit | LOCKED (memory) |

**Önemli:** Pixel-perfect physics + collider için PPU=16'nın altına inme; Final Boss PPU=32 sweet spot. Unity Filter Mode = **Point (no filter)**, scale-up bulanıklaşmaz.

**4) Clipping önleme — `animation.md` kuralı:**
> *"if you want to create large special effects, move your character to the side of the canvas so the model has more space"*

Geniş silah / büyük VFX gerekiyorsa karakteri canvas'ın kenarına yerleştir (asimetrik konum), tam ortaya değil.

---

### G. Style Reference Pro Scale Trap — KRİTİK (LOCKED 2026-05-10)

**Kaynak:** Antigravity (Codex) video analizi — Bulgu 1.

**Sorun:** Style Reference Pro referans görselin scale ve padding'ini kopyalar. Kırpılmış / padding'siz sprite yüklenirse AI yeni karakteri canvas'a doldurur (%60 padding kuralı ihlal edilir) → tüm anim pipeline çöker.

**Referans yükleme kuralı:**
| Görsel tipi | Yüklenebilir mi? |
|---|---|
| `rima_style_anchor.png` (252×252, %60 padding) | ✅ EVET |
| Approved base sprite (252×252, ~128px karakter) | ✅ EVET |
| Aseprite'ta kırpılmış sprite (örn. 128×128 frame) | ❌ HAYIR |
| Padding silinmiş temizlenmiş sprite | ❌ HAYIR |
| Ekran görüntüsü / 56×56 / 64×64 sprite | ❌ HAYIR |

**Pre-check (Style Reference Pro her çağrısında ZORUNLU):**
1. Referans dosya boyutu **252×252** mı?
2. Karakter canvas'ın **~%50'sini** kaplıyor mu? (etrafta %40 transparent padding görünmeli)
3. Background **transparent** veya **#00FF00** mı?

Bu 3 kontrol fail ediyorsa: referans olarak kullanma — `rima_style_anchor.png`'e geri dön.

---

## 📋 Üst Düzey Checklist

- [ ] **D. Warblade Anim** (10 alt adım, simetrik) — `04_NEXT_Warblade_anim/`
- [ ] **E. Ranger Anim** (10 alt adım, asimetrik) — `05_NEXT_Ranger_anim/`
- [ ] **F. Shadowblade Anim** (10 alt adım, asimetrik) — `06_NEXT_Shadowblade_anim/`
- [ ] **G. Elementalist Anim** (9 alt adım, asimetrik, weapon pass YOK) — `07_NEXT_Elementalist_anim/`

---

## 🔑 Genel Kurallar (her üretimde geçerli)

### RIMA Prompt Standartları (ZORUNLU)
1. **Description: kısa + spesifik.** Destansı/edebi cümle YASAK. Örnek doğru: `"Human mage, blue robe, glowing staff"`. Örnek yanlış: `"A wise wizard with flowing crystalline robes that shimmer in the moonlight..."`
2. **Negative Description: ZORUNLU.** Standart blok: `blur, 3d render, smooth gradient, ambient occlusion render, anti-aliasing, ugly, deformed, low contrast, soft shading, photo-realistic`. Her üretimde mutlaka.
3. **Floor (zemin):** Create Tiles (Pro), Tile size **64×64**, View angle **High top-down**.
4. **Wall (duvar):** Create Tiles (Pro), Tile size **64×128**, View angle **Low top-down** (gerçek açı 45-60°, side face görünür). 90° YASAK.
5. **MCP/Web App ayrımı:** Karakter animasyonu → Web App ZORUNLU. Floor/obje/static prop/batch base 4-yön → MCP OK.

### PixelLab Web App ortak ayarlar

| Ayar | Değer |
|---|---|
| Pixel Art Mode | **ON** |
| Upscale | **OFF** |
| Anti-aliasing | **OFF** |

### Karakter anim için (Sınıflar)

- **Web App ZORUNLU** — MCP `animate_character` KALICI YASAK (Revize Notu D)
- **Canvas: 252×252** (v3 zorunlu) → Unity import'unda 128×128 frame olarak kırpılır
- Pixel budget: max **8 frame** @ 252×252 (Revize Notu C)
- Tool versiyonları: "NEW" tag'li v3 araçlar zorunlu, eski v2 %49 çöküyor (Revize Notu A)
- Hero anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png` her gen'de yükle

### Tile/wall için chromakey

- Background: **#00FF00** (saf yeşil)
- Process script filter: `G>200 AND R<60 AND B<60`

### Obstacle için

- Background: **Transparent ON** (chromakey gerekmez, process_tiles.py atlanır)

---

# D — WARBLADE ANIM (Simetrik, 3 yön + W flip)

> **Sınıf bilgisi:**
> - Type: **Simetrik** → S, E, N üret. **W = Unity flipX**, üretme.
> - Accent: **Cold blue #7BA7BC**
> - Weapon: **Greatsword** (two-handed)
> - Yasak: el glow, mor renk
>
> **Tüm anim için ortak ayarlar:**
> - Web App ZORUNLU (MCP DEĞİL)
> - Hero anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
> - Canvas v3 otomatik 252x252
> - **Body-only** prompt (silah hep YOK, Adım 25 weapon pass'a kadar)
>
> **Pre-Animation Eraser Pass (ZORUNLU — 2026-05-10):**
> Base sprite (Adım 17/26/35/44) çıktısı direkt animate edilemez. Önce Pixelorama'da:
> 1. Zoom 800%+ → sprite'ı tara
> 2. Siluet dışındaki 1-2px stray pixel'ları sil
> 3. Çift outline varsa içtekini sil (1px standart)
> 4. Mixel'ları düzelt (3-4 farklı renk geçiş pikselleri)
> 5. `*_clean.png` olarak kaydet → anim adımlarına bu versiyonu ver
> **Neden:** Stray pixel 6-8 anim frame'ine yayılır → 8× temizlik iş yükü. Önce temizle.
> **Kaynak:** Antigravity (Codex) video analizi — Bulgu 4.
>
> **Head Swap QC (idle/walk/dash/attack anim için — hurt/death'te YAPMA):**
> Animate with Text NEW her çalıştırmada yüz/kask piksellerini hafifçe kaydırır.
> Her anim sheet'ten sonra Aseprite'ta:
> 1. Base sprite'tan head region'u (~32px üst alan) kopyala
> 2. Anim sheet'in her frame'ine Paste in Place
> 3. Onion Skin ON ile frame-by-frame kayma kontrolü
> **Kaynak:** Antigravity (Codex) video analizi — Bulgu 3.

## ✅ Adım 17: Warblade Base 4-yön (Body-only)

**🛠️ Tool:** **Create Character Pro New**

**⚙️ Ayarlar:**
- Canvas: 252x252
- 4 yön: South / East / North → 3 ayrı gen call (W üretme)
- Hero Anchor yükle

**📝 Prompt (her yön için aynı, "facing X" değiştir):**
```
Pixel art warrior character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35° elevation. Heavy plate armor, broad shoulders, cold blue cloth accent #7BA7BC at sash and shoulder straps. Palette: armor steel #4A4E5A / #5C6070 / #6E7280, accent blue #7BA7BC, leather #3A2818 / #5A4028, skin #C9A084 / #A07858, hair dark brown. Stoic stance, feet shoulder-width, arms relaxed. Hard pixel edges, no anti-aliasing, pixel cluster min 4px. NO embedded glow, NO VFX, NO weapon. [FACING SOUTH | FACING EAST | FACING NORTH] (face camera for south).
```

**💾 Kaydet (3 dosya):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/01_base_4dir/warblade_base_N.png
```

> W (West) üretme — Unity'de `flipX` ile E'den otomatik üretilir.

---

## ✅ Adım 18: Warblade Idle Anim

> **Önce Eraser Pass:** Base sprite'ı direkt input olarak verme — Bölüm D Pre-Animation Eraser Pass workflow'unu tamamla, `*_clean.png` versiyonunu kullan.

**🛠️ Tool:** **Animate with Text NEW**

**⚙️ Ayarlar:**
- Input: Adım 17'nin S/E/N sprite'ları
- Frame: 6-8
- Yön sayısı: 3 (S, E, N) → 3 gen call

**📝 Prompt (her yön için):**
```
Subtle breathing motion, 6-8 frames. Character chest rises and falls slowly, weight shifts subtly between feet. Same pose as base sprite, no weapon. [FACING SOUTH | EAST | NORTH].
```

**💾 Kaydet (3 dosya):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_idle_N.png
```

---

## ✅ Adım 19: Warblade Hurt Anim

**🛠️ Tool:** Animate with Text NEW

**⚙️ Ayarlar:** **4 frame** (tool çift sayı zorunlu — 4/6/8/10/12/14/16 only), 3 yön (S/E/N)

> **NLM-NOTE 2026-05-10:** Önceki "3 frame" değeri PixelLab API'sinde geçersizdi. Aseprite import'ta gerekirse fazla frame manuel silinir.

**📝 Prompt:**
```
Flinch backwards, 4 frames. Character's torso jerks back from impact, head tilts away, no weapon. Cold blue accent (#7BA7BC) flickers slightly. Frame 1: idle pose. Frame 2: peak flinch (max backward lean). Frame 3: hold reaction. Frame 4: recovery toward idle.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_hurt_N.png
```

---

## ✅ Adım 20: Warblade Death Anim

**🛠️ Tool:** Animate with Text NEW

**⚙️ Ayarlar:** 6 frame, 3 yön

**📝 Prompt:**
```
Collapse to ground, 6 frames. Heavy character falls forward to knees then face-down. No weapon. Frame 1: stagger. Frame 2: knees buckle. Frame 3: kneeling. Frame 4: torso falls forward. Frame 5: arm catches ground. Frame 6: prone, motionless.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/02_idle_hit_death/warblade_death_S.png (+E, +N)
```

---

## ✅ Adım 21: Warblade Walk Cycle (Brian's Extreme Pose)

**🛠️ Tool:** Animate with Text NEW + **Interpolate NEW**

**Adım 21a — Extreme Pose A üret:**
```
Walking forward, right leg fully extended in stride, weight shifted to front foot, arms swing in counter-rhythm, body lean slight forward. Heavy warrior gait, no weapon. [FACING S | E | N].
```
12 frame al → en uç pozu (diz en yukarıda) **seç** → Pose A.

**Adım 21b — Pose B = A flip:**
Aseprite'ta pose A'yı yatay flip → Pose B kaydet.

**Adım 21c — Interpolate A → B:**
Tool: **Interpolate NEW (v2, 252×252)**, Input: Pose A + Pose B, Output: 4-6 frame.
> **Alternatif (geniş silah kırpma yaşanırsa):** **Animation-to-Animation Bridging Mode** kullan — KF1 (Pose A) + KF3 (Pose B) ver, 2 ara frame üretir. Warblade greatsword için bu seçenek daha güvenli.

**💾 Kaydet (her yön için 6 frame walk cycle):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_S.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_E.png
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/03_run_cycle/warblade_walk_N.png
```

**Adım 21d — Walk Robotik Hissediyorsa: Mid-Stride Recovery (FALLBACK):**

Eğer 21a-21c sonucu walk loop robotik / sallanan hissediyorsa idle'dan yeniden promtlama. Bunun yerine:
1. Mevcut kötü walk output'unu Aseprite'ta aç
2. Bacaklar en geniş açıda olduğu frame'i bul (genelde frame 3 veya 5/12) → `warblade_walk_seed_S.png` olarak kaydet
3. Animate with Text NEW'i bu seed frame'le başlat (idle değil):
   - Frame: 12
   - Prompt: `"warrior in stride continuing forward walk loop, leg swing carries through, no reset to idle, [FACING X]"` — "loop" ve "continue" kritik
4. Yeni 12 frame'den ilk 6'sı genelde kullanılabilir → loop test et

**Neden çalışır:** Idle pose modeli neutralize eder → her frame idle'a dönmeye çalışır → robotik. Mid-stride seed momentum'u korur.

**Kaynak:** Antigravity (Codex) video analizi — Bulgu 2 (2026-05-10).

---

## ✅ Adım 22: Warblade Attack_LMB (3-Segment Greatsword Slash)

**🛠️ Tool:** Animate with Text NEW (PEAK) + **Interpolate NEW** (segments)

**Adım 22a — PEAK frame:**
```
Greatsword horizontal slash at full extension, arms parallel to ground, sword tip past character silhouette right edge. Body twisted 30° to follow slash, weight on back foot, full commitment. Cold blue accent flickers at sword wake (#7BA7BC).
```
> Body-only kuralı: PEAK frame'i silahla üretiyorsun (3-segment'in tamamı silahlı çekilir, weapon pass yok). **DİKKAT:** Karar #71 LOCKED — Warblade silah hep elde, sheath/draw YOK. Yani LMB/RMB anim'lerinde silah görünür ama base/idle/walk/hurt/death silah olmayabilir.

**Adım 22b — START → PEAK:**
- Tool: Interpolate NEW
- Input: idle sprite (Adım 18) + PEAK frame
- Output: 4 frame windup

**Adım 22c — PEAK → END:**
- Input: PEAK + recovery pose (idle benzeri)
- Output: 4 frame follow-through

**💾 Kaydet (her yön için 8 frame total — PEAK paylaşılır, Revize Notu E):**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/04_attack_LMB/warblade_lmb_S.png (+E, +N)
```
> 252² × 8 = 508,032 ≤ 524,288 ✓ (9 frame olsaydı bütçe aşardı). PEAK = windup son + follow ilk frame.

---

## ✅ Adım 23: Warblade Attack_RMB (Heavy Slam)

**🛠️ Tool:** Animate with Text NEW + Interpolate NEW

**PEAK frame:**
```
Greatsword slammed into ground, both hands gripping hilt at chest level, blade vertical with tip at character's feet. Body fully forward, knees bent, weight committed downward. Impact frame — peak commitment.
```

**START → PEAK:** 4 frame (sword raised overhead, max windup) — son frame = PEAK
**PEAK → END:** 4 frame (recovery, pull sword from ground) — ilk frame = PEAK paylaşılır
**Toplam: 8 unique frame** (Revize Notu E)

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/05_attack_RMB/warblade_rmb_S.png (+E, +N)
```

---

## ✅ Adım 24: Warblade Dash

**🛠️ Tool:** Animate with Text NEW (4 frame)

**📝 Prompt:**
```
Quick forward lunge, 4 frames. Frame 1: anticipation crouch (knees bent). Frame 2: explosive forward push, leading leg extended, body horizontal. Frame 3: airborne mid-dash, arms back. Frame 4: landing crouch, recovery. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/06_dash/warblade_dash_S.png (+E, +N)
```

---

## ✅ Adım 25: Warblade Weapon Pass (Edit Image Pro)

> **Weapon Pass Stratejisi DOĞRULANDI (2026-05-10):** Antigravity (Codex) video analizi bağımsız olarak aynı sonuca ulaştı — profesyonel pixel artist'ler bile sword attack frame'lerinde silahı manuel yeniden çiziyor. Mevcut body-only → Edit Image Pro → Aseprite propagate pipeline'ı değişmiyor.

**🛠️ Tool:** **Edit Image Pro**

> Bu adım body-only sprite setine **silah ekler**. Adım 22-23'teki LMB/RMB için silahlı PEAK zaten çekildi — bu adım idle/walk/dash/hurt/death için silah ekler.

**⚙️ Ayarlar:**
- Input: Adım 17 base S sprite (body-only)
- Yön: 3 ayrı pass (S, E, N)

**📝 Prompt:**
```
Add greatsword on right shoulder, two-handed grip when raised. Sword: 3.5 head-tall blade, steel #6E7280 / #8A8E98 / #A6AAB4, hilt wrapped leather #3A2818, crossguard iron #282830. Cold blue cloth wrap on hilt (#7BA7BC). NO glow, NO embedded VFX. Apply per direction: S, E, N (W = flip E with sword in correct hand — no separate weapon paint).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/04_NEXT_Warblade_anim/outputs/07_weapon_pass/warblade_weapon_S.png (+E, +N)
```

> Sonra Aseprite'ta tüm idle/walk/dash/hurt/death frame'lerine bu silah katmanını propagate et (kopyala-yapıştır).

---

# E — RANGER ANIM (Asimetrik, 4 yön ayrı)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → S, E, N, **W** dördü ayrı (yay tek elde)
> - Accent: **Cold blue #7BA7BC** (Warblade ile aynı), Yasak: mor
> - Weapon: **Compound bow** (sol elde)
> - Body-only adımlar 26-33, weapon pass adım 34

## ✅ Adım 26: Ranger Base 4-yön (Body-only)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı (S, E, N, W)

**📝 Prompt (her yön için):**
```
Pixel art ranger character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Lean agile build, hooded cloak, cold blue undertunic (#7BA7BC), forest green cloak (#3A4A38 / #4E5E48). Quiver visible on back (leather strap). Palette: cloak green #3A4A38 / #4E5E48, leather #3A2818 / #5A4028, accent blue #7BA7BC, skin #C9A084. Light leather armor, flexible stance, feet hip-width. Hood up, partial face shadow. NO weapon held. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/01_base_4dir/ranger_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 27: Ranger Idle (4 yön)

> **Önce Eraser Pass:** Base sprite'ı direkt input olarak verme — Bölüm D Pre-Animation Eraser Pass workflow'unu tamamla, `*_clean.png` versiyonunu kullan.

**🛠️ Tool:** Animate with Text NEW | 6-8 frame, 4 yön

**📝 Prompt:**
```
Alert breathing, 6-8 frames. Hood slightly sways, head subtly scans, weight subtly shifts. Tense posture but relaxed shoulders. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 28: Ranger Hurt (4 yön)

**⚙️ Ayarlar:** **4 frame** (tool çift zorunlu)

**📝 Prompt:**
```
Flinch sideways, 4 frames. Light agile recoil — body twists rather than falls back. Cape flares from motion. Frame 1: idle. Frame 2: peak twist (45° body turn). Frame 3: hold turn. Frame 4: recovery.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 29: Ranger Death (4 yön)

**📝 Prompt:**
```
Collapse sideways, 6 frames. Light body falls to one side, hood slips off head. No weapon. Frame 1: stagger. Frame 2-3: knees buckle and lean. Frame 4: hand catches ground. Frame 5: lying on side. Frame 6: motionless, hood on ground.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/02_idle_hit_death/ranger_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 30: Ranger Walk Cycle (Brian's Extreme Pose, 4 yön)

**Tool:** Animate with Text NEW + Interpolate NEW

**Extreme Pose A prompt:**
```
Walking forward light-footed, right leg extended in stride, body lean very slight forward, cape sways behind. Quick agile gait. No weapon. [FACING S | E | N | W].
```

**Walk pipeline:** Pose A → Aseprite flip → Pose B → Interpolate (4-6 frame).

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/03_run_cycle/ranger_walk_S.png (+E, +N, +W)
```

> **Walk robotik hissederse:** Adım 21d Mid-Stride Recovery prosedürünü uygula. Idle'dan yeniden üretme — mevcut kötü output'un mid-stride frame'ini seed olarak kullan.

---

## ✅ Adım 31: Ranger Attack_LMB (Bow Shot, 4 yön)

**🛠️ Tool:** Animate with Text NEW + Interpolate NEW (3-segment, **8 frame** — PEAK paylaşılır, Revize Notu E)

**PEAK prompt:**
```
Bow drawn fully, left arm extended forward holding bow, right hand at cheek anchor, arrow knocked. Body twisted 45° (asymmetric stance), bow vertical. Cold blue accent (#7BA7BC) glints on bowstring. Full draw commitment.
```

**START → PEAK:** 4 frame (bow raised, drawing motion)
**PEAK → END:** 4 frame (release — string snaps forward)

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/04_attack_LMB/ranger_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 32: Ranger Attack_RMB (Aim Shot 2-Stage, 4 yön)

**PEAK prompt:**
```
Slow aim — bow at full draw with extra time, breath held, body very still and centered. Arrow tip glows faintly cold blue (#7BA7BC charge). Pose more deliberate than LMB.
```

**Pipeline:** 3-segment, **8 frame** (PEAK paylaşılır — Revize Notu E).

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/05_attack_RMB/ranger_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 33: Ranger Dash (4 yön)

**📝 Prompt:**
```
Quick agile roll, 4 frames. Frame 1: crouch. Frame 2: forward dive low to ground, body horizontal. Frame 3: tucked roll mid-air. Frame 4: emerge upright with bow ready. No weapon.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/06_dash/ranger_dash_S.png (+E, +N, +W)
```

---

## ✅ Adım 34: Ranger Weapon Pass (Edit Image Pro, 4 yön)

**📝 Prompt:**
```
Add compound bow held in LEFT hand. Bow: vertical orientation when at rest, ~1.2x character height. Wood riser #5A4028 / #7A5838, limbs darker #3A2818, string thin off-white #C8C0A8. Cold blue grip wrap (#7BA7BC). Quiver on back already in base sprite. Apply per direction: S, E, N, W (each painted separately — flip changes weapon hand).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/05_NEXT_Ranger_anim/outputs/07_weapon_pass/ranger_weapon_S.png (+E, +N, +W)
```

---

# F — SHADOWBLADE ANIM (Asimetrik, 4 yön)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → 4 yön ayrı
> - Accent: **Void purple #5A2A8A** (Shadowblade kendi violet)
> - Weapon: **Twin short blades** (her elde 1)
> - Yasak: embedded glow karakter sprite'ında — VFX engine-side

## ✅ Adım 35: Shadowblade Base 4-yön (Body-only)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı

**📝 Prompt:**
```
Pixel art shadowblade assassin character, body-only, no weapon, character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Slim agile build, full dark hooded cloak, body almost entirely silhouetted in dark with violet undertones. Palette: cloak black-purple #1A0E2A / #2A1A3A, mid #3A2A4E, accent violet #5A2A8A, skin partial visible #C9A084 (only chin and jawline below hood), leather straps #3A2818. Crouched ready stance, body lean forward, weight on balls of feet. Hood deep — no eyes visible (silhouette only). NO weapon, NO embedded glow. [FACING S | E | N | W]. Hard pixel edges.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/01_base_4dir/shadowblade_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 36: Shadowblade Idle (4 yön)

> **Önce Eraser Pass:** Base sprite'ı direkt input olarak verme — Bölüm D Pre-Animation Eraser Pass workflow'unu tamamla, `*_clean.png` versiyonunu kullan.

**📝 Prompt:**
```
Predator stillness, 6-8 frames. Very subtle motion — only cloak hem flutters slightly, head turns by 5-10° to scan. Body stays low and tense. No weapon. Less obvious breathing than other classes — assassin stillness.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 37: Shadowblade Hurt (4 yön)

**⚙️ Ayarlar:** **4 frame** (tool çift zorunlu)

**📝 Prompt:**
```
Sharp recoil, 4 frames. Body twists hard from impact, cloak flares dramatically. Violet accent (#5A2A8A) flashes on cloak edge. Frame 1: idle. Frame 2: peak recoil (cloak full flare, body 60° twist). Frame 3: hold recoil. Frame 4: recovery.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 38: Shadowblade Death (4 yön)

**📝 Prompt:**
```
Dissolve / sink, 6 frames. Character collapses but with shadowy fade — last 2 frames show partial dissolution into ground (cloak fades to silhouette). Frame 1: stagger. Frame 2: knees buckle. Frame 3: lying on side. Frame 4: cloak fades (alpha 70%). Frame 5: only silhouette remains (alpha 40%). Frame 6: faint shadow stain on ground (alpha 20%).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/02_idle_hit_death/shadowblade_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 39: Shadowblade Walk (4 yön)

**📝 Prompt (Extreme Pose A):**
```
Walking forward predator-style, low and silent, body crouched, knees bent slightly, weight always on balls of feet. Cloak sways behind. No weapon. Subtle compared to Warblade — less stride distance, more compressed posture. [FACING S | E | N | W].
```

**Pipeline:** Pose A → flip → Pose B → Interpolate.

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/03_run_cycle/shadowblade_walk_S.png (+E, +N, +W)
```

> **Walk robotik hissederse:** Adım 21d Mid-Stride Recovery prosedürünü uygula. Idle'dan yeniden üretme — mevcut kötü output'un mid-stride frame'ini seed olarak kullan.

---

## ✅ Adım 40: Shadowblade Attack_LMB (Twin Blade Chain, 4 yön)

**PEAK prompt (1st hit):**
```
Right blade slash horizontal, arm extended at full slash, body 30° twisted. Left arm pulled back ready for next strike. Violet accent at blade trail (#5A2A8A streak). Body fully committed forward, weight on front foot.
```

**3-segment, 8 frame (PEAK paylaşılır — Revize Notu E)**.
> Eski plan 9 frame'di — 252² × 9 = 571,536 bütçe aşımı. PEAK frame'i windup'ın son frame'i + follow'un ilk frame'i olarak paylaş. Toplam unique: 8.

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/04_attack_LMB/shadowblade_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 41: Shadowblade Attack_RMB (Shadow Step / VeilStrike, 4 yön)

**PEAK prompt:**
```
Phase-strike — character mid-teleport, body half-dissolved into shadow, blade emerging at target side with violet streak (#5A2A8A intense). Anticipation pose: original location shows fading silhouette, peak position shows blade impact frame. Two visual elements in single frame — shadow trail behind, character at strike point.
```

**START → PEAK:** 4 frame (crouch, fade into shadow at original spot)
**PEAK → END:** 4 frame (re-materialize, blade lower, recovery)

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/05_attack_RMB/shadowblade_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 42: Shadowblade Dash (4 yön)

**📝 Prompt:**
```
Shadow dash, 4 frames. Frame 1: crouch with violet wisp at feet. Frame 2: body partially dissolves, blur streak forward. Frame 3: body re-forms ahead, mid-arrival. Frame 4: landing crouch, blades ready. Cloak trails violet smoke (#5A2A8A faint, max 4px wisp).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/06_dash/shadowblade_dash_S.png (+E, +N, +W)
```

---

## ✅ Adım 43: Shadowblade Weapon Pass (Edit Image Pro, 4 yön)

**📝 Prompt:**
```
Add twin short blades — one per hand. Each blade: ~0.6x character height, narrow silhouette, dark steel #4A4E5A / #5C6070, hilt wrapped black-violet (#1A0E2A / #5A2A8A). Curved or straight short-sword profile. Apply per direction: S, E, N, W (each painted separately).
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/06_NEXT_Shadowblade_anim/outputs/07_weapon_pass/shadowblade_weapon_S.png (+E, +N, +W)
```

---

# G — ELEMENTALIST ANIM (Asimetrik, 4 yön, SİLAHSIZ)

> **Sınıf bilgisi:**
> - Type: **Asimetrik** → 4 yön ayrı (el jestleri tek tarafta belirgin)
> - Accent: **Element bazlı** — sprite element-agnostic kalır (cool neutral robe), VFX engine-side
> - Weapon: **YOK** → **Adım 44'ten Adım 51'e kadar 8 adım, weapon pass YOK**
> - Yasak: void purple (Shadowblade'in mor'u), kitap/odiyak

## ✅ Adım 44: Elementalist Base 4-yön (Silahsız, eller serbest)

**🛠️ Tool:** Create Character Pro New | 4 yön ayrı

**📝 Prompt:**
```
Pixel art elementalist mage character, body-only, no weapon, NO book, NO staff (hands free for spell gestures), character occupies ~50% of canvas height (~128px tall) centered on a 252x252 transparent canvas. Wide transparent padding on all sides for animation headroom — DO NOT fill the canvas. High top-down view 30-35°. Long flowing robe, hood NOT up (face visible — confident mage), short hair. Robe palette: deep blue-grey #2A3848 / #3E4C5E / #525E74 (cool neutral default — element accent only on spell anims). Trim accent: faint cool #B8C8D0. Sash at waist #3A2818 leather. Skin #C9A084. Body pose: slightly forward, hands held at chest level, palms angled outward (ready to cast). Robe hem sways. NO weapon, NO held object. [FACING S | E | N | W]. Hard pixel edges, no anti-aliasing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/01_base_4dir/elementalist_base_S.png (+E, +N, +W)
```

---

## ✅ Adım 45: Elementalist Idle (4 yön)

> **Önce Eraser Pass:** Base sprite'ı direkt input olarak verme — Bölüm D Pre-Animation Eraser Pass workflow'unu tamamla, `*_clean.png` versiyonunu kullan.

**📝 Prompt:**
```
Subtle channeling stance, 6-8 frames. Hands hover at chest, fingers slightly curl and uncurl as if shaping invisible energy. Robe hem sways. Hair barely moves. Confident mage breathing.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_idle_S.png (+E, +N, +W)
```

---

## ✅ Adım 46: Elementalist Hurt (4 yön)

**⚙️ Ayarlar:** **4 frame** (tool çift zorunlu)

**📝 Prompt:**
```
Stagger backwards, 4 frames. Mage recoils, hands pulled to chest defensively, robe flares from sudden motion. Frame 1: idle. Frame 2: peak recoil (body bent backward 30°, hands up). Frame 3: hold backward. Frame 4: recovery stance.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_hurt_S.png (+E, +N, +W)
```

---

## ✅ Adım 47: Elementalist Death (4 yön)

**📝 Prompt:**
```
Knees buckle slowly, 6 frames. Mage falls to knees first (caster's last stand), then forward. Robe billows. Frame 1: stagger. Frame 2: knees give. Frame 3: kneeling. Frame 4: hands fall to floor. Frame 5: forward fall. Frame 6: prone.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/02_idle_hit_death/elementalist_death_S.png (+E, +N, +W)
```

---

## ✅ Adım 48: Elementalist Walk (4 yön)

**📝 Prompt:**
```
Walking forward mage gait, robe trailing behind, smooth deliberate step (not as quick as Ranger, not as heavy as Warblade). Hands held loosely at sides or chest. No weapon. [FACING S | E | N | W].
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/03_run_cycle/elementalist_walk_S.png (+E, +N, +W)
```

> **Walk robotik hissederse:** Adım 21d Mid-Stride Recovery prosedürünü uygula. Idle'dan yeniden üretme — mevcut kötü output'un mid-stride frame'ini seed olarak kullan.

---

## ✅ Adım 49: Elementalist Attack_LMB (CastRhythm Spell, 4 yön)

**PEAK prompt:**
```
Both hands extended forward, palms outward, fingers spread — peak cast moment. Element energy gathers at palms (small VFX placeholder area, ~12x12px each, intentionally blank for engine VFX overlay). Body lean slightly forward, weight balanced. NO embedded glow on character — only blank palm zones marked for engine particle. Confident mage release pose.
```

> **DİKKAT:** Karakter sprite'ında element rengi YOK. Sprite element-agnostic, VFX overlay runtime'da Fire/Frost/Lightning/Light farkını ekler.

**3-segment, 8 frame (PEAK paylaşılır — Revize Notu E)**.
> Eski plan 9 frame'di — 252² × 9 = 571,536 bütçe aşımı. PEAK frame'i windup'ın son frame'i + follow'un ilk frame'i olarak paylaş. Toplam unique: 8.

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/04_attack_LMB/elementalist_lmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 50: Elementalist Attack_RMB (Element Ranged Spell, 4 yön)

**PEAK prompt:**
```
One hand thrust forward (right hand), palm out, the other hand pulled back at hip channeling. Body twisted 30° to support thrust. Robe billows from cast force. Blank palm zone for VFX (16x16px on extended hand). NO embedded glow.
```

**3-segment, 8 frame (PEAK paylaşılır — Revize Notu E)**.
> Eski plan 9 frame'di — 252² × 9 = 571,536 bütçe aşımı. PEAK frame'i windup'ın son frame'i + follow'un ilk frame'i olarak paylaş. Toplam unique: 8.

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/05_attack_RMB/elementalist_rmb_S.png (+E, +N, +W)
```

---

## ✅ Adım 51: Elementalist Dash (4 yön)

**📝 Prompt:**
```
Mage step / blink, 4 frames. Frame 1: crouch with hand gesture (palm down at side, channeling). Frame 2: body briefly distorts (motion blur with robe streak forward). Frame 3: body re-forms ahead, robe still trailing. Frame 4: landing pose, hands return to chest. NO embedded glow — engine VFX adds element trail per active element.
```

**💾 Kaydet:**
```
STAGING/PIXELLAB/07_NEXT_Elementalist_anim/outputs/06_dash/elementalist_dash_S.png (+E, +N, +W)
```

> **❌ Weapon Pass YOK — Elementalist silahsız.** Adım 25/34/43 muadili Adım'ı YOK. Bu sınıf 51 adımda biter.

---

# 🏁 BİTİŞ

Tüm 51 adım tamamlandığında:
1. Klasör adlarına `__DONE_` prefix ekle (örn: `__DONE_01_NEXT_walls`) **VEYA** `_DONE/` altına taşı
2. Bana haber ver — `CURRENT_STATUS.md` güncellenecek
3. Yeni Act / sınıf üretimleri için yeni numaralı klasör aç (`08_NEXT_Brawler_anim` vs.)

## Hızlı Tahmin

| Bölüm | Süre (solo) | Kredit |
|---|---|---|
| A. Walls (3 üretim) | ~1-2 saat | ~150-200 |
| B. Floors (5 üretim) | ~1-2 saat | ~120-180 |
| C. Obstacles (8 üretim) | ~2-3 saat | ~100-150 |
| D. Warblade (Sym) | ~1-1.5 hafta | ~640-940 |
| E. Ranger (Asym) | ~1-1.5 hafta | ~750-1100 |
| F. Shadowblade (Asym) | ~1-1.5 hafta | ~750-1100 |
| G. Elementalist (Asym, no weapon) | ~1 hafta | ~660-960 |

> Tier 2 abonelik (Yasin'in mevcut planı) ~2000-3000 kredit/ay → 4 sınıf P0 ≈ 2 ay.

## Referanslar (sub-agent'lara gerekirse)

- Master Pipeline: `GUIDES/RIMA_MASTER_ART_PIPELINE.md`
- Animation Bible: `TASARIM/ANIMATION_BIBLE.md`
- Style anchor: `TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
- Klasör HOWTO/PROMPT detayları: her klasörün kendi içinde
