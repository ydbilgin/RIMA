# Animation Planning Rules
> Claude için dahili kural. Pipeline değişmez. Yeni karakter/animasyon talebinde bu çerçeveyle planla.
> Yükleme zamanı: Yeni karakter animasyonu planlarken.

---

## Pipeline Kuralı

Bu dosya mevcut PixelLab pipeline'ı değiştirmez:
- `Edit Image PRO` → peak/impact keyframe
- `Interpolate (new)` → start+end frame arası doldur
- `Animate with text (new)` → fallback

Claude'un rolü: her animasyon için **planlama beyni** olmak.

---

## Her Animasyon Talebi İçin Adımlar

### 1. Motion Analizi
- Bu animasyon fiziksel olarak ne yapıyor?
- Kaç aşaması var: anticipation / release / impact / recovery / settle

### 2. Frame Range Kararı
- **Minimum** effective frame sayısı — maksimum değil
- Idle: 8 | Walk: 8 | Dash: 8 | Attack combo step: 10 | Death: 10
- Daha fazlasını yalnızca motion gerçekten gerektiriyorsa kullan

### 3. Key Poses
- Animasyonun sırasıyla major pose'larını listele
- Tüm aşamaları kullanma — sadece okunabilirliği artıranları seç

### 4. Segment Planı
- Her pose geçişinin ayrı mı üretileceğini mi yoksa interpolate mu edileceğini karar ver
- **Ayrı üret:** karmaşık silah arc, büyük gövde rotasyonu, el yoğun aksiyonlar
- **Interpolate:** kısa geçişler, basit arc, iki temiz pose arası bağlantı

### 5. Her Segment İçin Prompt

Her prompt şunları içermeli:
- Gövde yönelimi ve baktığı yön
- Gövde rotasyonu
- Silah başlangıç / bitiş pozisyonu
- El davranışı (önemliyse)
- Ağırlık transferi
- Hareketin tonu: yavaş / patlayıcı / keskin / uzayan / kontrollü

**Kaçın:** "fluid motion", "dynamic slash", "cool attack" — belirsiz wording yasak.

### 6. Interpolation Guidance
- Nerede güvenli: kısa geçişler, basit arc, temiz pose bağlantısı
- Nerede riskli: tam attack tek pasajda, komplike silah arc, büyük silüet değişimi

### 7. Consistency Warnings
- Silah tutarlılığı riski
- El pozisyonu riski
- Silüet değişimi riski
- Karakter kimliği riski

---

## Segment Karar Kuralı (Saldırılar)

```
MASTER FRAME KARARLARI (2026-04-14 kilitlendi):

  Idle:   1 segment | 8f  | same-frame loop
  Walk:   2 segment | 5+5=10f | base→mid-stride→base
  Death:  2 segment | 4+8=12f | base→stagger→flat dead
  Dash:   2 segment | 4+4=8f  | base→peak→landing
  Attack: 3 segment | 3+6+3=12f | base→windup→peak→base

Segment başına frame limiti: MAX 6f (üzerinde AI drift başlar)
Daha fazla frame = daha fazla segment, segment başına fazla frame değil

Zincirleme kural (attack ve dash):
  WINDUP/PEAK üret → WINDUP'ı input alarak PEAK üret
  PEAK/LANDING için → PEAK'i input alarak LANDING üret
  Her keyframe bir öncekinden beslenir → tutarlılık artar
```

---

## Output Formatı (Yeni Animasyon Talebi Gelince)

```
### [Animasyon Adı] — [Karakter]

**Motion Reading:** [ne yapıyor]
**Frame Range:** [min] — [opsiyonel üst]
**Key Poses:** [liste]
**Segment Plan:** [hangi geçişler ayrı / hangisi interpolate]

**Segment 1 Prompt:**
[mekanik, somut, wording]

**Segment 2 Prompt (varsa):**
[...]

**Interpolation Notes:** [güvenli / riskli]
**Consistency Warnings:** [silah / el / silüet]
```

---

## Öncelik Sırası

1. Kontrol
2. Okunabilirlik
3. Tutarlılık
4. Temiz segmentasyon

**DEĞIL:** fazla frame, tek pasajda üretim, belirsiz sinematik wording.
