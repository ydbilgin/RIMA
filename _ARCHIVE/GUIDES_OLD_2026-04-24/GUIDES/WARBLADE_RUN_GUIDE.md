# Warblade Run Animasyon Rehberi
Güncelleme: 2026-04-23 | Kilitlenen kararlara göre (ANIM_LOCKED_V3)

---

## Yaklaşım

- Primary tool: **Custom Animation V3** (UI'da "Add Animation" → "Custom Animation V3")
- Fallback: **Create from Reference** (yalnızca kalite düşerse)
- Referanslar yön-bazlıdır. Her yönde ChatGPT running referansı + o yönün base karakter sprite'ı birlikte kullanılır.
- AI Freedom: **0** (karakter kimliği/siluet korunur)

**Yön referans eşleşmesi:**
- SE -> `southeast.png`
- E -> `east.png`
- S -> `south.png`
- NE -> `northeast.png`
- N -> `north.png`
- SW -> `southwest.png`
- W -> `west.png`
- NW -> `northwest.png`

---

## Sabit Ayarlar (Her Üretimde Aynı)

| Alan | Değer |
|------|-------|
| Tool | **Custom Animation V3** |
| Custom Frames - Start | o yönün run key pose A (bir ayak önde) |
| Custom Frames - End | run key pose B (karşı adım) — **boş bırakma** |
| Keep first frame (idle pose) | **OFF** — açık kalırsa model idle'a kayar |
| Referans 1 (stil) | `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/[yön].png` |
| Referans 2 (karakter) | `Characters/Warblade/base/warblade_base_[YÖN].png` |
| Frame sayısı | 8 (9 çıkarsa sonu sil) |
| FPS | 10-12 |
| Maliyet | **13 gen/yön** |
| Preset | male human |
| Camera View | UI'dan yöne göre ver — prompta yazma |

---

## Üretim Sırası

**SE ✅ → E ✅ → S ✅ → NE → SW → N → W → NW**

(Drift azaltma: karşı diyagonal SW, N'den önce — anchor noktası S+SE kalıyor)

SE/E/S tamamlandı, baseline kabul edildi. QC'den geçmiş anchor olarak korunuyor.

---

## Per-Yön Promptlar

Her prompt **tek satır** - kopyala yapıştır.

---

### SE yönü
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/southeast.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_SE.png`

`warblade heavy knight running southeast, body leaning diagonal toward lower-right, both arms pulled together behind the body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together not swinging freely, greatsword blade trailing low behind right side below hip level blade readable, legs pumping forward torso leaning into run, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_SE.png`

---

### E yönü
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/east.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_E.png`

`warblade heavy knight running east, body strongly leaning sideways angled east, both arms pulled together behind body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together not swinging freely, greatsword blade trailing low behind right hip extending past body silhouette blade readable, legs pumping torso angled into run direction, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_E.png`

---

### S yönü
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/south.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_S.png`

`warblade heavy knight running south toward viewer, slight forward lean, both arms pulled together downward behind center mass right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together not swinging freely, greatsword blade trailing low behind body below waist level readable between or beside legs, legs pumping forward, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_S.png`

---

### NE yönü ⚠️ Zor yön
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/northeast.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_NE.png`

`warblade heavy knight running northeast, torso angling away upper-right, both arms pulled together behind body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together spacing between hands clearly visible, greatsword blade trailing low behind right side blade readable beside torso, legs pumping body leaning into northeast direction, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_NE.png`

> İki el hilt spacing görünmezse retry yap.

---

### N yönü ⚠️ Zor yön
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/north.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_N.png`

`warblade heavy knight running north fully back-facing only the back visible, both arms pulled downward behind the back right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together at lower back, greatsword blade trailing low behind the body below waist dragging as character runs forward blade readable beside spine, legs pumping shoulders slightly hunched forward, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_N.png`

> Kılıç gövdeye gömülürse "blade clearly visible to one side of spine" ekle, retry.
> Tek el tutuyorsa yukarıdaki promptu kullan — "TWO HANDS BOTH GRIPPING" vurgusu bunu engeller.

---

### SW yönü
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/southwest.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_SW.png`

`warblade heavy knight running southwest, body leaning diagonal toward lower-left, both arms pulled together behind the body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together not swinging freely, greatsword blade trailing low behind left side below hip level blade readable, legs pumping forward torso leaning into run, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_SW.png`

---

### W yönü
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/west.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_W.png`

`warblade heavy knight running west, body strongly leaning sideways angled west, both arms pulled together behind body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together not swinging freely, greatsword blade trailing low behind left hip extending past body silhouette blade readable, legs pumping torso angled into run direction, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_W.png`

---

### NW yönü ⚠️ Zor yön
**Referans (ChatGPT):** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/warblade_running_8_direction/northwest.png`
**Referans (Karakter):** `Characters/Warblade/base/warblade_base_NW.png`

`warblade heavy knight running northwest, torso angling away upper-left, both arms pulled together behind body right arm gripping crossguard left arm gripping pommel on the same two-handed greatsword hilt arms locked together spacing between hands clearly visible, greatsword blade trailing low behind left side blade readable beside torso, legs pumping body leaning into northwest direction, pixel art sprite, MATCH THE RUNNING POSE AND DIRECTION of the reference image exactly`

Kaydet: `warblade_run_NW.png`

> İki el hilt spacing görünmezse retry yap.

---

## Run->Idle Geçiş Animasyonu (run_stop)

Direkt run->idle bağlantısı snap/pop yapar - koşma pozu dinamik (öne eğik, kılıç momentumlu).
Her yön için 2-3 frame deceleration klibi üret.

### Yaklaşım
- Tool: Animate with text NEW
- Referans 1: warblade_running_8_direction/[yön].png (başlangıç pozu)
- Referans 2: warblade_base_[YÖN].png (bitiş pozu - idle)
- Frame sayısı: 2-3
- FPS: 8-10 (run ile idle arasında)
- Prompt yapısı: "warblade heavy knight deceleration stopping, transitioning from full run to idle stance, [YÖN] facing, sword settling from momentum, pixel art sprite 128px"

### Üretim Sırası
SE -> E -> S -> NE -> N -> SW -> W -> NW (run loop'la aynı sıra)

### Klasör
Assets/Sprites/Characters/Warblade/run_stop/
  warblade_run_stop_SE.png
  warblade_run_stop_E.png
  ... (8 yön)

### Unity'de Kullanım
Animator: Run -> run_stop (has exit time) -> Idle
run_stop klibi loop değil - tek seferlik oynar, ardından idle'a geçer.

---

## Aseprite

- Run: 8 frame, frame duration **80-100ms** (10-12 FPS)
- run_stop: 2-3 frame, frame duration **100-125ms**
- Idle: 4-6 frame, frame duration **160-250ms** (4-6 FPS)
- Eğer run çıktısı 9 frame gelirse: son frame'i sil (genelde loop kopyası)
- Export -> Sprite Sheet (Horizontal)

---

## QC Checklist (Her Yön)

- [ ] Kılıç silhouette her framede okunabilir, gövdeye gömülmemiş
- [ ] İki el hilt'te her framede - sağ crossguard'a yakın, sol pommel'a yakın
- [ ] Kılıç boyutu frame'ler arası tutarlı (scale pumping yok)
- [ ] Loop seamless: son frame -> ilk frame pop yapmıyor
- [ ] NE/N/NW: iki el spacing görünüyor

---

## Klasör Yapısı

```text
Assets/Sprites/Characters/Warblade/run/
  warblade_run_SE.png
  warblade_run_E.png
  warblade_run_S.png
  warblade_run_NE.png
  warblade_run_N.png
  warblade_run_SW.png
  warblade_run_W.png
  warblade_run_NW.png

Assets/Sprites/Characters/Warblade/run_stop/
  warblade_run_stop_SE.png
  warblade_run_stop_E.png
  warblade_run_stop_S.png
  warblade_run_stop_NE.png
  warblade_run_stop_N.png
  warblade_run_stop_SW.png
  warblade_run_stop_W.png
  warblade_run_stop_NW.png
```

---

## Maliyet

| Yöntem | Gen/yön | Toplam |
|--------|---------|--------|
| Run (Custom Animation V3) | 13 | **104 gen** (8 yön) |
| run_stop (Custom Animation V3) | 13 | **104 gen** (8 yön) |
| **Toplam** | - | **~208 gen** (tüm 8 yön + run_stop) |

> Kalan 5 yön (NE/SW/N/W/NW) için: 5 × 13 = **65 gen** run + **65 gen** run_stop = **130 gen**.
> Üretim öncesi kredi kontrolü yap.
