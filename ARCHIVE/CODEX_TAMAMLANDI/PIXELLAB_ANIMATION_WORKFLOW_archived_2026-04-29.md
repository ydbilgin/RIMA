# PixelLab Animasyon Workflow — Tüm Classlar

Bu dosya RIMA projesindeki tüm class animasyonları için genel workflow rehberidir. Class-spesifik detaylar için ilgili class guide'ına bak.

---

## Bölüm 1 — Araç Seçimi

### Hangi Ekran, Ne İçin

Animasyon üretimi için **"Add Animation"** butonuna tıkla. Açılan ekranda:

- **Custom Animation V3 (BETA)** seç — bu tek geçerli seçenek
- **Action Description** — animasyonu tanımlayan tek satır İngilizce metin
- **Frame Count slider** — 4 ile 10 arasında (13 gen/yön tüketir)
- **"Keep first frame (idle pose)"** checkbox — attack/skill/dash başlangıcı için ON yap
- **Advanced Options > Custom Frames** — Start Frame ve End Frame galeriden seçilir

### Custom Frames Nedir

Custom Frames, V3 AI'ın Start ve End Frame arasındaki kareleri doldurmasını sağlar. Bu "KF+Interpolate" yaklaşımının PixelLab arayüzüdür — Aseprite interpolate'e gerek kalmaz, sadece segment birleştirme için Aseprite kullanılır.

Keyframe galeriye iki yoldan girer:
1. "Add Animation" ile min frame (4) + tek segment desc → üret → galeri
2. Create from Reference ile tek poz → galeri

Bir sonraki animasyonun End Frame = "Pick from gallery" ile önceki frame seçilir.

### "Keep First Frame" Checkbox

**ON yap:** Attack windup, skill başlangıcı, dash başlangıcı — animasyon karakterin idle/base pozu ile açılır.

**OFF bırak:** Run, idle döngüsünün sonraki segmenti, herhangi bir ortadan-başlayan segment.

### Segment Başına Max Frame

Her segment için frame count **6'yı geçme.** Daha fazlası drift ve deformasyon riski yaratır. Uzun animasyonlar birden fazla segmente bölünür.

---

## Bölüm 2 — Animasyon Tipi ve Yöntem Tablosu

| Animasyon | Yöntem | Segment | Frame/segment | Start Frame | End Frame | Checkbox |
|-----------|--------|---------|---------------|-------------|-----------|----------|
| Idle | Custom V3 + Custom Frames | 1 | 4-6f | idle pose | hafif ağırlık kayması (galeriden) | ON |
| Run | Custom V3 (text only) | 1 | 6f | — | — | OFF |
| Attack LMB | Custom V3 + Custom Frames | 3 | A:3-4f / B:4-6f / C:3-4f | A:idle / B:WINDUP / C:PEAK | A:WINDUP / B:PEAK / C:idle | A:ON / B:OFF / C:OFF |
| Skill | Custom V3 + Custom Frames | 3 | A:4-5f / B:5-6f / C:4-5f | A:idle / B:WINDUP / C:PEAK | A:WINDUP / B:PEAK / C:idle | A:ON / B:OFF / C:OFF |
| Dash | Custom V3 + Custom Frames | 2 | 4f her segment | A:idle / B:PEAK | A:PEAK / B:landing | A:ON / B:OFF |
| Death | Custom V3 + Custom Frames | 2 | A:4f / B:8f | A:idle / B:stagger | A:stagger / B:flat | A:ON / B:OFF |

---

## Bölüm 3 — Keyframe Üretim Sırası (Zincirleme Kural)

**Altın kural: PEAK frame her zaman önce üretilir.**

PEAK frame en kritik pose'tur — tüm diğer keyframeler bu referanstan beslenir.

Genel sıra:

1. PEAK frame üret → galeriye kaydet
2. WINDUP üret → End Frame = PEAK (galeriden), Start Frame = idle (checkbox ON)
3. RECOVERY üret → Start Frame = PEAK (galeriden), End Frame = idle (galeriden)
4. Segmentleri Aseprite'de birleştir

Her keyframe bir öncekinin output'undan beslenir. Sırayı bozma — önce recovery veya windup üretmek anlamsız, AI referans frame olmadan tutarsız pose üretir.

---

## Bölüm 4 — 8 Yön Stratejisi

- Her animasyon için ilk üretilen yön **S (south)** — kameraya en dik, en okunabilir yön
- S yönü QC geçince kalan 7 yön (SE, E, NE, N, NW, W, SW) üretilir
- **Flip yasak** — tüm 8 yön PixelLab'de ayrı ayrı üretilir
- S yönünü onaylamadan diğer yönlere geçme — prompt veya ayar problemi varsa 8×tüketmemiş olursun

---

## Bölüm 5 — Aseprite Görevi (Sadece Birleştirme)

Custom V3, segment içi interpolasyonu kendisi yapar. Aseprite'ın görevi sadece segmentleri birleştirmek ve frame sürelerini ayarlamaktır.

### Birleştirme Adımları

1. Yeni Aseprite dosyası aç (128px yükseklik)
2. Segment A spritesheet'ini import et
3. Segment B spritesheet'ini ekle (A'nın son frame'inden sonra)
4. Segment C spritesheet'ini ekle (B'nin son frame'inden sonra)
5. Frame sürelerini ayarla (aşağıdaki tabloya göre)
6. Export: Horizontal Strip, 128px yükseklik

### Standart Frame Süre Tablosu

| Segment | İçerik | Süre |
|---------|--------|------|
| Windup | Anticipation | 80ms/frame |
| Impact | Snap | 40ms/frame |
| Recovery | Settle | 100ms/frame |

---

## Bölüm 6 — Dosya Adlandırma Kuralı

```
[class]/[animasyon]/[class]_[animasyon]_[yön]_[segment].png

Örnekler:
warblade/attack/warblade_attack_S_B.png       ← PEAK frame (önce üretilen)
warblade/attack/warblade_attack_S_A.png       ← Windup frame
warblade/attack/warblade_attack_S_C.png       ← Recovery frame
warblade/attack/warblade_attack_S_anim.png    ← Aseprite final strip
warblade/run/warblade_run_S_anim.png          ← Run final strip
warblade/idle/warblade_idle_S_anim.png        ← Idle final strip
```

Galeri kayıtları için de aynı isimlendirmeyi kullan — galeriden "Pick" yaparken karıştırma.

---

## Bölüm 7 — Genel Action Description Template

Her class için description yaz, `[PLACEHOLDER]` kısımları class-specific kelimelerle doldur. Tüm promptlar tek satır, İngilizce.

**Run:**
`[CLASS_DESC], running forward [WEAPON_CARRY_DESC], pixel art sprite`

**Attack PEAK (Impact):**
`[CLASS_DESC], [WEAPON_ACTION_PEAK_DESC], pixel art sprite`

**Attack WINDUP (Anticipation):**
`[CLASS_DESC], [WEAPON_WINDUP_DESC], pixel art sprite`

**Attack RECOVERY:**
`[CLASS_DESC], [WEAPON_RECOVERY_DESC], settling back to guard position, pixel art sprite`

**Idle:**
`[CLASS_DESC], standing in guard stance with slight weight shift, pixel art sprite`

**Skill PEAK:**
`[CLASS_DESC], [SKILL_PEAK_DESC], dramatic moment of skill release, pixel art sprite`

### Yasaklı Prompt İfadeleri

- "dark fantasy" — genre etiketi yazma; yerine: `muted desaturated palette, weathered field-worn`
- "3/4 view" — açı ifadesi prompt'a girmez, UI'dan yön seçilir
- "no eyes" / "eyeless" — karakter gözsüz çizilir, yazma
- "80 degree" / "extreme top-down" / "bird's eye" — yanlış açı
- Oyun adı (RIMA)

Standart kamera açısı prompt'a YAZILMAZ — UI'daki yön seçimi yeterlidir.
