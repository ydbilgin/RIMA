# Warblade Animasyon Guide — Custom Animation V3 (Enhance Action default)

**Session 40 kilidi (2026-04-24):** Tüm class animasyonları bu pipeline'ı takip eder. Warblade = anchor.

---

## Temel İlkeler

1. **Tek tool:** Custom Animation V3 (Add Animation → CUSTOM → Custom Animation V3).
2. **Prompt basit:** Tek cümleyle aksiyonu tarif et → **Enhance action with AI** butonuna bas → AI referans sprite'a bakarak promptu genişletir.
3. **Karakter kimliği referans sprite'tan gelir** — prompt içine "male warrior with beard, plate armor, greatsword" gibi kimlik tarifi yazma, Enhance AI bunları zaten resimden alır.
4. **Flip yasak.** 8 yön ayrı üretilir.
5. **AI Freedom: 0** (silüet/kimlik korunur).

---

## Ortak Sabitler

| Alan | Değer |
|------|-------|
| Size | 128 px |
| Outline | Single color black |
| Shading | Medium |
| Detail | Medium |
| AI Freedom | **0** |
| Preset | male human |
| Frame count | Animasyona göre (aşağıda) |
| Cost | 13 gen / yön |

**Base sprite klasörü:** `Characters/Warblade/base/warblade_base_[DIR].png`
(`DIR` ∈ `S, SE, E, NE, N, NW, W, SW`)

**Üretim sırası (her animasyon için aynı):**
`S → SE → E → NE → N → NW → W → SW`
S yönü PASS olmadan diğer yönlere geçme.

---

## Keyframe Üretimi — KARAR KİLİDİ (Session 40)

"Keyframe" = Start/End Frame için kullanılan statik poz sprite'ı. Üç yolu var, sırasıyla dene:

### Yol 1 — Base sprite yeter (önce bunu dene)
Start Frame = mevcut `warblade_base_[DIR].png`. End Frame ya boş ya da aynı base (loop). Çoğu idle/run/single-shot case için yeter.

### Yol 2 — V3 text-only ile keyframe üret
Farklı bir poz (ör. Attack PEAK) lazımsa → V3 çağrısı yap, **Custom Frames slot'larını BOŞ bırak**, Keep first frame **OFF**, Frame 4, action description = PEAK tarifinin basit hali + Enhance Action. Çıkan sprite serisinden ortadaki (en ekstrem) frame'i Aseprite'ta kes → yeni keyframe. Ek galeri kaydı olarak kullan.

### Yol 3 — Edit Image (site ekranı SS gelince netleşecek)
Base sprite'ı alıp poz değiştirmek için PixelLab site'de **Edit Image** / **Pose Variation** aracı varsa. Kullanıcı screenshot atınca bu bölüm güncellenir. Şimdilik Yol 2 default.

**Prensip:** Hiçbir keyframe Aseprite'ta manuel çizilmez. Her keyframe PixelLab çıktısı (direkt veya frame kesme).

---

## 1) Idle

**Yöntem:** V3, tek segment, loop.

| Alan | Değer |
|------|-------|
| Start Frame | `warblade_base_[DIR].png` |
| End Frame | (boş) |
| Keep first frame | **ON** |
| Frame count | **4** |
| Action description | `standing idle with subtle breathing` |
| Enhance Action with AI | ✅ bas |

**Çıktı:** `Characters/Warblade/idle/warblade_idle_[DIR].png` (horizontal strip, 4f)
**Aseprite:** Frame duration 160ms (tüm frameler).
**Maliyet:** 13 × 8 = **104 gen**.

---

## 2) Run

**Yöntem:** V3, tek segment, loop YOK (Unity side loop).

| Alan | Değer |
|------|-------|
| Start Frame | `warblade_base_[DIR].png` |
| End Frame | (boş — V3 locomotion türetir) |
| Keep first frame | **OFF** |
| Frame count | **6** |
| Action description | `running forward with two-handed greatsword` |
| Enhance Action with AI | ✅ bas |

**Çıktı:** `Characters/Warblade/run/warblade_run_[DIR].png` (strip, 6f)
**Aseprite:** Frame duration 80ms.
**Maliyet:** 13 × 8 = **104 gen**.

**Fallback (loop kötüyse):** Start ve End'e ayrı adım keyframe'leri (sol ayak / sağ ayak) koy — Yol 2 ile üret.

---

## 3) Attack LMB — 3-Segment

Horizontal slash, two-handed. Segment sırası: **B (PEAK) önce**, sonra A (Windup), sonra C (Recovery).

### Segment B — PEAK

| Alan | Değer |
|------|-------|
| Start Frame | (boş) |
| End Frame | (boş) |
| Keep first frame | **OFF** |
| Frame count | **4** |
| Action description | `swinging greatsword in wide horizontal slash at full reach` |
| Enhance Action with AI | ✅ bas |

Stripten en uç framei seç → galeri kaydı `warblade_attack_[DIR]_B`.

### Segment A — Windup (B üretildikten sonra)

| Alan | Değer |
|------|-------|
| Start Frame | `warblade_base_[DIR].png` |
| End Frame | galeri — `warblade_attack_[DIR]_B` |
| Keep first frame | **ON** |
| Frame count | **3** |
| Action description | `coiling back to prepare strike` |
| Enhance Action with AI | ✅ bas |

### Segment C — Recovery

| Alan | Değer |
|------|-------|
| Start Frame | galeri — `warblade_attack_[DIR]_B` |
| End Frame | `warblade_base_[DIR].png` |
| Keep first frame | **OFF** |
| Frame count | **3** |
| Action description | `recovering stance after strike` |
| Enhance Action with AI | ✅ bas |

### Aseprite Assembly

A → B → C sırasıyla birleştir, tek horizontal strip export et.

| Segment | Frame | Süre |
|---------|:----:|-----:|
| A (windup) | 3 | 80 ms |
| B (peak/impact) | 1 (ana) | **40 ms** (snap) |
| C (recovery) | 3 | 100 ms |

**Çıktı:** `Characters/Warblade/attack/warblade_attack_[DIR].png`
**Maliyet (yön):** 13 × 3 = 39 gen | **8 yön:** 312 gen.

---

## 4) Hit Reaction (4 yön)

4 yön yeterli: `S, E, N, W`. SE/NE/NW/SW için yan yönler reuse.

| Alan | Değer |
|------|-------|
| Start Frame | `warblade_base_[DIR].png` |
| End Frame | `warblade_base_[DIR].png` (geri dön) |
| Keep first frame | **OFF** |
| Frame count | **3** |
| Action description | `flinching back from hit` |
| Enhance Action with AI | ✅ bas |

**Çıktı:** `Characters/Warblade/hit/warblade_hit_[DIR].png`
**Maliyet:** 13 × 4 = **52 gen**.

---

## 5) Death (4 yön)

| Alan | Değer |
|------|-------|
| Start Frame | `warblade_base_[DIR].png` |
| End Frame | (boş — kollaps serbest) |
| Keep first frame | **OFF** |
| Frame count | **8** |
| Action description | `collapsing forward and falling limp to the ground` |
| Enhance Action with AI | ✅ bas |

**Çıktı:** `Characters/Warblade/death/warblade_death_[DIR].png`
**Aseprite:** Frame duration 100ms, son frame 500ms (ölü kal).
**Maliyet:** 13 × 4 = **52 gen**.

---

## Maliyet Özeti

| Animasyon | Yön | Gen/yön | Toplam |
|-----------|:---:|:-------:|:------:|
| Idle | 8 | 13 | 104 |
| Run | 8 | 13 | 104 |
| Attack | 8 | 39 (3 segment) | 312 |
| Hit | 4 | 13 | 52 |
| Death | 4 | 13 | 52 |
| **TOPLAM** | — | — | **624 gen** |

---

## QC Checklist

### Her animasyon (genel)
- [ ] 8 yön (veya 4 yön) set tam
- [ ] Kılıç silüeti her karede okunabilir, gövdeye gömülmemiş
- [ ] İki el hilt'te (NE/N/NW dahil)
- [ ] Base sprite ile scale/silüet tutarlı (karakter büyüyüp-küçülmüyor)
- [ ] Loop varsa seamless (idle, run)

### Attack özel
- [ ] B PEAK: tam reach, snap hissi (40ms)
- [ ] A: silah geride, ağırlık arkada
- [ ] C: blade aşağıda, guard'a dönüş

---

## Enhance Action — Ne Zaman Kullanma

- **Kullan:** %95 durumda default.
- **Kullanma:** Çok spesifik teknik constraint gerekiyorsa (ör. "blade must stay visible past right hip"). O zaman manuel detaylı prompt yaz, Enhance basma.

---

## Cross-Class Uygulama

Bu guide'ın şablonu tüm class'lar için geçerli:
1. Base sprite klasörünü değiştir (`Characters/[Class]/base/`)
2. Action description'daki silah ismini değiştir (greatsword → scythe / bow / daggers vb.)
3. Geri kalan tüm V3 ayarları aynı
