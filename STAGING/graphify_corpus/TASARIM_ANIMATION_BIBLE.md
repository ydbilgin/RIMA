---
status: REFERENCE
faz: 1
tarih: 2026-04-23
ozet: "Animasyon üretim kuralları ve workflow"
---
# RIMA Animation Bible — LOCKED
# Tarih: 2026-05-09 | Opus kararı | v1

Tüm karakter, mob ve boss animasyon kararları bu dosyada kilitlidir.
Karakter art pipeline: `GUIDES/PIXELLAB_PRODUCTION_GUIDE_v1.md`

---

## KARAR 1 — Kamera Açısı / PixelLab View (LOCKED)

**View: High top-down (~30-35° elevation) — tüm assetler için tekdüze**

- Hades ~30° kamera açısı kullanır — PixelLab "High top-down" bunu karşılar
- RIMA IsometricZAsY (pseudo-iso) = gerçek 30/60° değil; High top-down bu grid için daha okunabilir
- Low top-down (~20°) karakter siluetlerini fazla sıkıştırır, mob'lar (48-52px) okunamaz hale gelir
- **Tüm assetler aynı açı:** oyuncu, mob, boss, prop, decal, tile gölge yönü

**PixelLab S-XL view seçimi:**
- Karakterler / moblar: **High top-down**
- 3D derinlikli objeler (pillar, altar): **Low top-down** (görsel anlam için istisna)
- Zemine yapışık objeler (rubble, decal): **High top-down**

---

## KARAR 2 — Karakter Yönleri (LOCKED)

**4 yönlü + yatay ayna = efektif 6 unique açı**

Unique üretilen: **N, NE, E, SE, S, SW**
Aynalanan: W = E mirror, NW = NE mirror

- Hades modeli — PixelLab Animate with Text v3 4 yön setini güvenilir üretir
- 8 yön üretim maliyeti 3x artırır + NE/NW tutarsızlık riski yüksek
- Simetrik art bile mirror'da doğal görünür (silah eli dahil)
- **İstisna:** Ronin'in sol kalçada kın asymmetrisi — v1'de kabul et, v2'de NE/NW hand-paint

**Yön kodlaması:**
```
N = kuzey (sırt kameraya)
S = güney (yüz kameraya) ← default
E = doğu (sağa bakan)
NE = kuzey-doğu
SE = güney-doğu
SW = güney-batı (SE mirror)
W = batı (E mirror)
NW = kuzey-batı (NE mirror)
```

---

## KARAR 3 — Oyuncu Animasyon Seti v1 (LOCKED)

**7 temel animasyon (tüm sınıflar):**

| # | Animasyon | Açıklama |
|---|---|---|
| 1 | Idle | Bekleme, south-facing default |
| 2 | Walk | 8 frame döngü |
| 3 | Attack_LMB | Temel saldırı, sınıfa göre |
| 4 | Attack_RMB | RMB saldırı/skill |
| 5 | Dash | Hareket dash (sınıf bağımsız) |
| 6 | Hurt | Hit-react frame (Scar Memory 1.2s için zorunlu) |
| 7 | Death | Ölüm sekansı |

**Sınıfa özel ekleme:**
- Summoner: **Summon_Cast** (8. anim) — pet spawn kimlik mekaniği
- Ronin sheath/draw: **Idle varyant** olarak sakla — ayrı state değil (v1 scope)
- Diğer 9 sınıf: 7 anim

**Toplam oyuncu animasyonu v1:** ~71 (7 × 10 + 1 Summoner)

---

## KARAR 4 — Mob Animasyon Seti v1 (LOCKED)

**4 temel (standart mob):**

| # | Animasyon |
|---|---|
| 1 | Walk |
| 2 | Attack |
| 3 | Hurt |
| 4 | Death |

- Idle yok — moblar combat state'te spawn eder
- Stagger ayrı anim yok — Hurt frame stagger feedback'i karşılar
- Walk frame 0 = standing pose (ambient durumda)

**Elite mob (tier 2+):** +**Telegraph** (5. anim) — dodge için adil uyarı

**Boss:** 8 anim tam set:
Idle, Walk, Attack_1, Attack_2, Telegraph, Hurt, Death, Phase_Transition

---

## KARAR 5 — Default Yüzleşme Yönü (LOCKED)

**South-facing (kameraya doğru) = default**

- Hades convention — karakter kimliği (yüz, silah) spawn ve idle'da okunur
- PixelLab Create Character Pro front-facing referans üretiyor — eşleşiyor
- Unity IsometricZAsY'nin tercih yönü yok — tamamen art convention

---

## BÜTÇE ÖZETİ v1

| Kategori | Anim Sayısı | Not |
|---|---|---|
| Oyuncu (10 sınıf × 7 + 1) | ~71 | Summoner +1 |
| Standart mob (~16 mob × 4) | ~64 | Act 1 roster |
| Elite mob (~4 × 5) | ~20 | Telegraph dahil |
| Boss (~3 × 8) | ~24 | Phase_Transition dahil |
| **TOPLAM** | **~179** | v1 minimum |

---

## PixelLab Üretim Notu

**Pipeline:** Create Character Pro New → Animate with Text NEW v3 → Interpolate NEW → Edit Image Pro
**Canvas:** 128px native, 252px v3 render (büyük canvas v3'te otomatik)
**Tier 2 kredi tahmini:** ~750-1100 cred/sınıf → 10 sınıf ~4-5 ay

Detay: `GUIDES/RIMA_MASTER_ART_PIPELINE.md`

---

## Extracted from STALE memory (S91 2026-05-18)
### Source: project_animation_notes.md

**Animator timing (PlayerAnimator.cs):**
- Impact snap: 40ms
- All other frames: 80-100ms
- Blend Tree Exit Time: OFF (transition: 0.05s)
- `run_stop` state: MANDATORY (must exist in animator graph)
- Idle: 4 frames, KeepFirst ON

