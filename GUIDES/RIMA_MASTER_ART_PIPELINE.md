# RIMA — Master Art Pipeline (v1)
*Tüm görsel üretim için tek canonical kaynak.*
*Son güncelleme: 2026-05-09 | Status: LOCKED v1*
*Replaces: PIXELLAB_FAZ_MASTER, CHARACTER_IDENTITY_FRAMEWORK, CHARACTER_BASE_PRODUCTION_GUIDE, PIXELLAB_CREATE_CHARACTER_UI (hepsi ARCHIVE)*

---

## 0. Hızlı Karar Tablosu — "Hangi Asset Hangi Araçla?"

| Asset Tipi | Araç | Neden |
|---|---|---|
| Karakter base sprite (idle 4-yön) | PixelLab **Create Character Pro New** (Web App) | Body-only, 4 yön rotate, silah YOK |
| Karakter Idle / Hit / Death | PixelLab **Animate with Text NEW (v3)** (Web App) | Doğrudan 6-8 frame, kısa anim için optimal |
| Karakter Run cycle | **Animate with Text + Interpolate NEW** | Brian's Extreme Pose method (extreme + flip + interpolate) |
| Karakter Action / Skill / Attack | **3-Segment KF+Interpolate** (Web App) | PEAK önce, START→PEAK + PEAK→END interpolate |
| Karakter silah katmanı | **Edit Image Pro** (Web App) | İkinci pass, body bittiğinde silah eklenir |
| Floor / Wall tiles (F1-F4, W1-W5) | **ChatGPT (GPT-4o)** (Web) | Pixel art quality > PixelLab tile için |
| Dungeon objects, props, chests | PixelLab **MCP `create_object`** | Static asset, batch OK |
| Isometric tiles (custom) | PixelLab **MCP `create_isometric_tile`** | MCP supported, batch OK |
| Cinematic frames (boss intro, ending) | **Aseprite** + Edit Image Pro | Hand-painted single still |

**Altın Kural:** Karakter animasyonları **kesinlikle** MCP `animate_character` ile üretilmez (4-frame limit + VFX bug). Web app zorunlu.

---

## 1. Görsel Kimlik (Style Bible Özet)

**Tone:** "Fractured Epic" — dramatik, yüksek kontrast, dark fantasy ama **grimdark değil**. Hades vibe + Diablo 2 atmosfer.

**Kamera:** 35° low top-down ARPG (Diablo 2/PoE açısı). 8-direction terkedildi → **4 cardinal yön LOCKED** (S/E/N/W).

**Renk Disiplini:**
- **Çevre/zemin:** Soğuk, soluk, muted gri-mavi / siyah-mor
- **Karakter aksanı (Class Energy):** Saturated, Rift accent (cyan #00FFCC ana imza)
- **Combat readability:** Skill VFX/proc/Rift saturated → environment muted (kasıtlı hiyerarşi)

**Class Energy Rengi (sprite + VFX):**

| Class | Accent | Yasak |
|---|---|---|
| Warblade | Cold blue (#7BA7BC) | El glow, mor |
| Shadowblade | Void purple | — |
| Ranger | Cold blue (#7BA7BC) | Mor |
| Elementalist | Element rengine göre (Fire/Frost/Lightning/Light) | Void energy |
| Ravager | Blood red (#8B1A1A) | Mavi tonlar |
| Brawler | Charge yellow + earth | Magic glow |
| Gunslinger | Cold blue / muzzle flash | Western/cowboy aksesuar |
| Ronin | Silver-blue blade shimmer | — |
| Summoner | Crystal cyan + minion vapor | — |
| Hexer | Soul lantern green-gold + rune black | Kitap/odiyak (silahsız) |

**Style Anchor Image (her gen call'da `init_image`):**
`F:/Antigravity Projeler/2d roguelite/TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`

---

## 2. Unity Standartları (Sprite Import)

**LOCKED Settings — Sapma yasak:**

| Setting | Value |
|---|---|
| Texture Type | Sprite (2D and UI) |
| PPU (Pixels Per Unit) | **64** |
| Sprite Mode | Multiple |
| Frame size (Unity-side) | **128×128 per cell** |
| Pivot | Center (0.5, 0.5) |
| Filter Mode | **Point (no filter)** |
| Compression | Uncompressed |

**Canvas Boyutu — KRİTİK Netleştirme:**

| Aşama | Boyut | Anlamı |
|---|---|---|
| **Karakter native size** | **128×128** | RIMA'nın gerçek sprite boyutu (Unity import boyutu). Kahramanın *kendi* alanı. |
| **PixelLab Create Character v3 render canvas** | **252×252** | v3 araç **otomatik olarak** animasyon üretimi için canvas'ı büyütür — silah/uzuv hareketi headroom için. Karakter hâlâ 128, render alanı 252. |
| **Animation üretimi** | 252×252 (PixelLab) → **kırp** → 128×128 (Unity) | Aseprite'ta merkez-kırp 128 box. |

**Yani 252×252 PixelLab v3'ün dayatması, RIMA'nın seçimi değil.** Karakter her zaman 128px. v3 Create Character ile üretirken canvas otomatik 252 olur, animasyon sırasında bu boşluk silah/uzuv'un kanvas dışına taşmasını engeller. Unity'ye 128'e kırparak import edilir.

Eski v2 araçları 128 native kullanırdı — silah savurma frame'i kırpılırdı (Warblade greatsword örneği). v3'ün 252 canvas'ı bu sorunu çözmek için var.

**Sorting Layers (LOCKED):**
- Default → Ground → Wall → Entities → VFX → UI

**Tile boyutları (Karar #20'den):**
- Floor 64×32 (zemin), 16 variation pool
- Wall 64×96 (dikey duvar), 16 variation pool
- Tall wall 64×128 (büyük duvar), 12 variation pool

---

## 3. PixelLab Tools — Karşılaştırma

| Tool | Versiyon | Ne için | Frame/Gen | Canvas | Notes |
|---|---|---|---|---|---|
| **Create Character Pro New** | NEW v3 | Karakter base 4-yön rotate | 1 frame × 4 yön | 252×252 | Body-only, silahsız, idle pose |
| **Animate with Text NEW** | NEW v3 | Idle, Hit, Death (basit anim) | 4 frame / call | 252×252 | Pixflux engine, eski v2 %49'da çöküyor |
| **Animate with Text Pro** | Pro | Daha uzun anim dizileri | 4 frame / call (128px) | 252×252 | Tek aksiyon odaklı |
| **Animation-to-Animation (Bridging Mode)** | Mevcut | KF1 + KF3 ver → 2 ara frame üret | 2 frame / call | 128×128+ | Eski Interpolation'ın yerine |
| **Interpolate NEW (v2)** | NEW | Run cycle + 3-segment dolgu | 4 frame / call | 252×252 | Extreme pose arası dolgu |
| **Edit Image Pro** | Pro | Silah ekleme (2. pass), inpaint | 1 frame / call | 252×252 | Existing sprite'a detay |

**Kritik bulgu:** "NEW" tag'li v3 araçları (Pixflux engine) eski v2'lere göre **mecburi** — eski v2 generation'ların %90'ı %49'da çöküyor.

**Eski Interpolation (64×64 constraint'li) — ÖLÜ.** Yerine: Animation-to-Animation "bridging mode" (KF1+KF3 → 2 frame üretir, 128px+ destekli).

---

## 4. Karakter Üretim Pipeline (Web App, sırayla)

### Adım 1 — Base Sprite (Create Character Pro New)
- Web App'i aç → Create Character Pro New
- 4 yön: South (front) → East (sağ profil) → North (arka) → West (sol profil)
- **Body-only** prompt (silahsız!)
- Canvas: 252×252
- Hero Anchor: warrior_idle_128.png yükle (kamera açısı + boyut tutarlılığı)
- Çıktı: 4 idle sprite, sınıf referans

### Adım 2 — Idle / Hit React / Death (Animate with Text NEW)
- Adım 1'in idle sprite'ı = referans
- Idle: 6-8 frame, "subtle breathing motion" prompt
- Hit React: 3 frame, "flinch backwards"
- Death: 6 frame, "collapse to ground"
- Her yön ayrı (S/E/N/W) — simetrik class'larda W = E flip Unity-side
- Canvas: 252×252

### Adım 3 — Run Cycle (Brian's Extreme Pose Method)
- Animate with Text NEW: "running forward, leg extended" prompt → 12 frame al
- En uç poz seç (diz en yukarıda) = Extreme Pose A
- Extreme Pose A'yı **yatay flip** → Extreme Pose B
- Interpolate NEW: A → B arası 4-6 frame doldur
- Toplam: 6 frame run cycle / yön
- Asimetrik class'larda W ayrı üret (silah pozisyonu kayar)

### Adım 4 — Action Animations (3-Segment KF+Interpolate)
**Saldırı, dash, skill için:**

```
ADIM 4.1 — PEAK frame üret (en dramatik an: impact, burst peak)
   Tool: Animate with Text NEW (tek frame mod) veya Edit Image Pro
   Prompt: "axe slammed into ground, weight fully forward, peak impact"
   Output: 1 keyframe görseli

ADIM 4.2 — START → PEAK segment
   Tool: Interpolate NEW
   Input: idle sprite (start) + PEAK frame (target)
   Output: 4 frame windup/anticipation

ADIM 4.3 — PEAK → END segment
   Tool: Interpolate NEW
   Input: PEAK frame + recovery pose
   Output: 4 frame follow-through/recovery
```

**Toplam:** 1 + 4 + 4 = 9 frame her aksiyon. Aseprite'ta birleştir.

### Adım 5 — Silah Pass (Edit Image Pro)
**Body bittikten sonra silah ekle.** Bu kural Karar #71 LOCKED.

- Body-only animation seti hazır → idle S frame'i seç
- Edit Image Pro: "add greatsword on right shoulder" prompt
- Tek frame'de silah eklenir → diğer frame'lere propagate (her yön 1 base inpaint)
- Bu yöntem silah teleport / shimmer bug'larını engeller

**İstisna — Ronin:** Silah hep **kında** (sheath/draw kimliği). Idle ve Run'da kın görünür, sadece skill animasyonlarında çekilip kına döner.

**İstisna — Elementalist & Hexer:** Silahsız (büyü el jestleriyle). Adım 5 atlanır.

---

## 5. 4 Cardinal Yön Sistemi (Karar #53)

**Kural:**
- 8-direction iptal (Hades izometrik için, RIMA 35° ARPG değil)
- 4 yön üret: South / East / North / West
- Diagonal hareket → en yakın cardinal'e snap (45° threshold + hysteresis)

**Üretim Optimizasyonu:**

| Class Tipi | Üretim |
|---|---|
| **Simetrik** (Warblade, Ravager, Summoner, Brawler — silah ortalanmış veya çift el) | S, E, N üret. **W = E'nin Unity flipX'i** (üretme!) |
| **Asimetrik** (Shadowblade, Ronin, Gunslinger, Ranger, Elementalist, Hexer — silah/poz tek tarafta) | S, E, N, W **dördünü ayrı üret** (W flip'i silahı yanlış ele atar) |

**Maliyet farkı:** Simetrik 3-yön × N anim = X gen call; Asimetrik 4-yön = 1.33×X.

---

## 6. Çevre & Obje Üretimi

### 6.1 Floor / Wall Tiles — ChatGPT (GPT-4o) Tercih Edildi
PixelLab izometrik floor için smooth 3D-render üretiyor. ChatGPT pixel art quality daha yüksek.

**Workflow (Karar #18):**
1. ChatGPT canvas'ında "1024×1024 grid 4×4 tile" iste
2. Style anchor PNG (W1_PRIMARY) yükle
3. Çıktıyı `process_tiles.py` ile 16 ayrı tile'a böl
4. Unity import: PPU=64, Pivot=center

**Aktif prompt files:**
- `STAGING/CHATGPT_PROMPT_FLOOR_TILES.md`
- `STAGING/CHATGPT_BATCH_PROMPTS.md`

### 6.2 Dungeon Objects, Props, Chests — PixelLab MCP
- `mcp__UnityMCP__create_object` — sandık, meşale, sütun
- `mcp__UnityMCP__create_isometric_tile` — özel tile'lar
- Static asset, batch OK, ~$0.008/gen

### 6.3 Cinematic Frames (Boss Intro, Ending) — Aseprite + Edit Image Pro
- Hand-painted single still
- Boss reveal anı, 3-ending cutscene frame'leri
- Detay: `TASARIM/CINEMATIC_LAYER_v1.md` Katman D

---

## 7. Site (Web App) vs MCP — Karar Matrisi

| Görev | Tool | Neden |
|---|---|---|
| Karakter prototip | Web App | Görsel feedback, iterasyon |
| Karakter animasyon (her clip) | **Web App ZORUNLU** | MCP 4-frame limit + VFX bug |
| Style reference | Web App | Yan yana karşılaştırma |
| Batch yön (4 yön × 10 sınıf base) | **MCP** | Programmatic loop |
| Tile / obje / static | **MCP** | Batch OK, hızlı |
| Animasyon polish | Aseprite (PixelLab plugin) | Pixel-level kontrol |
| Cinematic frame | Aseprite + Edit Image Pro | Hand-paint quality |

**Solo dev kombinasyonu:** Prototip Web → Batch yön MCP → Animasyon Web (zorunlu) → Polish Aseprite → Unity manuel import.

---

## 8. QC Checklist (Üretim Sonrası)

Her sprite/anim üretildikten sonra:

- [ ] **Silüet:** Her frame'de okunaklı mı? (squint test — 1 sn'de class kim?)
- [ ] **4 yön tutarlılığı:** Boyut + orantı + palette aynı mı?
- [ ] **Embedded VFX:** Karakter sprite'ında glow/particle var mı? **YASAK** — VFX engine-side
- [ ] **Body-only:** Adım 5 öncesi silah var mı? **YOK olmalı**
- [ ] **Palette:** Sınıf accent rengi doğru mu? Class Energy tablosuna uyuyor mu?
- [ ] **127px bug:** Canvas boyutu 252 tam mı yoksa 251/253 mü? (Aseprite extend ile düzelt)
- [ ] **Frame count:** Run=6, Idle=8, Hit=3, Death=6, Skill segment'ler tam mı?
- [ ] **Cardinal flip kontrol:** Simetrik class W = E flipX mi? Asimetrik W ayrı üretildi mi?
- [ ] **Hero anchor uyum:** Camera açısı warrior_idle_128.png ile aynı mı? (eyes facing forward, no extreme overhead)

---

## 9. Production Sırası (P0/P1/P2)

**Sınıf üretim önceliği — her sınıf için sırayla:**

| Pri | İçerik | Neden |
|---|---|---|
| **P0** | Idle (8f) + Run (6f) + LMB attack (3-segment, ~9f) + RMB skill (~9f) + Dash (4f) + Hit React (3f) + Death (6f) | Sınıf oynanabilir hale gelir |
| **P1** | Sinerji/kombo skill'ler (3-segment her biri) + V Burst windup | Cross-class proc + V mechanic test edilebilir |
| **P2** | Ultimate (R-skill V Burst), late-game skill, alt-state animations (Charged/Marked vb.) | Polish phase |

**10 sınıf üretim sırası (Faz Master ile uyum):**
1. **Faz 1:** Warblade (referans sınıf — diğerleri için style anchor)
2. **Faz 2:** Elementalist, Shadowblade, Ranger
3. **Faz 3:** Ravager, Ronin, Gunslinger, Brawler
4. **Faz 4:** Summoner, Hexer

---

## 10. Maliyet & Süre Tahmini (Solo Dev — Yasin, **PixelLab Tier 2 abonelik aktif**)

**Tier 2 Abonelik (Yasin'in mevcut planı):**
- Aylık ~2000-3000 kredit havuzu (~$22/ay)
- **Pro tools erişimi:** Animate with Text Pro, Create 8-directional Pro, Create Instant Character — tier 2 gerekli
- Higher rate limits + öncelikli sıra
- API + MCP erişimi tam

**Sınıf başına gen call tahmini:**

| İçerik | Call sayısı | Kredit |
|---|---|---|
| Base 4-yön (Create Character Pro New) | 4 call | ~30-50 kredit |
| Idle (8f) + Hit (3f) + Death (6f), 4 yön | ~24 call (Animate w/ Text NEW) | ~150-200 kredit |
| Run cycle (4 yön × extract + interpolate) | ~12 call | ~80-120 kredit |
| 6 Skill × 3-segment × 4 yön (asimetrik), 3 yön (simetrik) | ~50-72 call | ~400-600 kredit |
| Weapon pass (Edit Image Pro × 4 yön + propagate) | ~6-8 call | ~80-120 kredit |
| **Toplam / sınıf** | **~100-120 call** | **~750-1100 kredit** |

**10 sınıf × 1000 kredit ≈ 10000 kredit ≈ 4-5 ay Tier 2 abonelik bütçesi.**

**Optimizasyon ipuçları (Tier 2'yi en verimli kullan):**
1. **Simetrik class W flip:** 3 yön üret + Unity flipX → %25 call tasarrufu
2. **Style anchor reuse:** Warblade idle S sprite'ı tüm sınıflar için `init_image` → ilk-pass kalite yüksek, iterasyon az
3. **Bridging mode > Interpolate v2 (eski):** 64×64 constraint'siz, daha az başarısız call
4. **Batch yön: MCP loop:** Web app manuel yerine programmatic — insan hatası yok
5. **Aseprite polish:** Polish web app'te yapma, kredit yakar — Aseprite'ta yap

**Süre / sınıf (solo dev):**
- P0 (oynanabilir): 1.5-2 hafta
- P1 (sinerji): 1 hafta  
- P2 (polish + ulti): 1 hafta
- **Toplam:** ~3.5-4 hafta / sınıf
- 10 sınıf P0 → P0 → P0 sırayla: ~3-4 ay full character production
- Cinematic + environment + UI ek work paralel

**Süre / sınıf (solo dev):**
- P0 (oynanabilir): 1.5-2 hafta
- P1 (sinerji): 1 hafta
- P2 (polish): 1 hafta
- **Toplam:** ~3.5-4 hafta / sınıf
- 10 sınıf × 4 hafta paralel olmaz; P0 hepsini önce → P1 hepsi → P2 hepsi pattern'ı = 8-12 ay total animation production

---

## 11. Migration Notu — Stale Dosyalardan Bu Guide'a Geçiş

**Bu guide'a merge edilen / arşivlenen eski dosyalar:**

| Eski Dosya | Status | Bilgi nereye gitti |
|---|---|---|
| `GUIDES/PIXELLAB_FAZ_MASTER.md` | ARCHIVE | Bu guide §9 (Production Sırası) |
| `GUIDES/CHARACTER_IDENTITY_FRAMEWORK.md` | ARCHIVE | Bu guide §1 (Class Energy) |
| `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md` | ARCHIVE | Bu guide §4 (Karakter Pipeline) |
| `GUIDES/PIXELLAB_CREATE_CHARACTER_UI.md` | ARCHIVE | Bu guide §3 (Tools) + §4 |
| `TASARIM/ANIMATION_REDESIGN.md` | STALE — mob için aktif | Class anim'leri buraya merge, mob bilgisi orada kalır |
| `STAGING/PIXELLAB_PRODUCTION_GUIDE_WARBLADE_ANIMATIONS.md` | KEEP — örnek olarak | §4 referans olarak kullanılır |
| `STAGING/PIXELLAB_ANIMATION_PROMPTS_10CLASS_2026-05-06.md` | KEEP — prompt library | §4 prompt template kaynağı |
| `STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md` | KEEP — character base prompts | §1 + §4 prompt detail |
| `TASARIM/CHARACTER_PROMPT_PIPELINE.md` | KEEP — class identity | §1 sınıf detayları |

---

## 12. Resmi Kaynaklar (Reference)

- **PixelLab Docs:** https://www.pixellab.ai/docs
- **Animate with Text Pro:** https://www.pixellab.ai/docs/tools/animate-with-text-pro
- **Animation to Animation:** https://www.pixellab.ai/docs/tools/animation-to-animation
- **Interpolation Docs:** https://www.pixellab.ai/docs/tools/interpolation
- **Create Character Options:** https://www.pixellab.ai/docs/options/character
- **PixelLab API:** https://www.pixellab.ai/pixellab-api
- **PixelLab MCP GitHub:** https://github.com/pixellab-code/pixellab-mcp

---

## 13. Hızlı Erişim Linkleri (Internal)

- Style Bible: `STYLE_BIBLE.md` (palette + identity detail)
- Class Identity: `TASARIM/CHARACTER_PROMPT_PIPELINE.md`
- 10-class anim prompts: `STAGING/PIXELLAB_ANIMATION_PROMPTS_10CLASS_2026-05-06.md`
- Warblade örnek: `STAGING/PIXELLAB_PRODUCTION_GUIDE_WARBLADE_ANIMATIONS.md`
- Floor tile prompts: `STAGING/CHATGPT_PROMPT_FLOOR_TILES.md`
- Dungeon assets: `STAGING/PIXELLAB_PRODUCTION_GUIDE_DUNGEON_ASSETS.md`
- Cinematic Layer: `TASARIM/CINEMATIC_LAYER_v1.md`
- Makeup Backlog: `TASARIM/MAKEUP_BACKLOG.md`
- Master Karar: `TASARIM/MASTER_KARAR_BELGESI.md`

---

**Versioning:** Bu dosya tüm görsel üretim değişikliklerinde güncellenir. Master Karar Belgesi #71'den sonraki ek karar'lar otomatik buraya yansıtılır.
