---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Görsel kalite standartları"
---
# RIMA — Görsel Kalite Standartları
> [STALE — see MASTER #53 and #42]

*Hedef referans: Hero Siege Season 9 seviyesi akıcı pixel art*
*Son güncelleme: 2026-04-07*

---

## Hedef

> "Hero Siege Season 9 gibi görünsün."
> Akıcı, canlı, combat feedback'i güçlü bir pixel art.
> Ham/amatör değil — juice var, okuma kolay, her hit hissediliyor.

---

## 1. Animasyon Frame Sayısı — Akıcılığın Temeli

Hero Siege S9 bu kadar akıcı görünüyor çünkü frame sayısı yüksek.

| Animasyon | Min | Hedef | Not |
|---|---|---|---|
| Idle | 6 | 8 | Nefes hareketi, hafif sallanma |
| Walk | 8 | 10 | Ağırlık hissi olsun |
| Run | 8 | 10 | Walk'tan hızlı, lean var |
| Attack | 8 | 10–12 | Windup + strike + follow-through |
| Dash | 6 | 8 | Blur frame + pozisyon |
| Death | 8 | 10–12 | Ağırlık + kolaps |
| Skill cast | 8 | 10 | Hazırlık + release |

**Kural:** 6 frame altı = choppy görünür. Mevcut 6-8 minimum kabul, 10-12 hedef.

---

## 2. Animasyon Hızı ve Timing — "Juice" Buradan Gelir

Frame sayısı kadar **ne zaman hızlı ne zaman yavaş olduğu** önemli.

### Melee Attack Pattern:
```
Windup (2-3 frame, YAVAŞ) → Strike (1-2 frame, ÇOK HIZLI) → Follow-through (2-3 frame, yavaş)
```
- Windup yavaş olmalı → oyuncu anticipation hisseder
- Strike anı neredeyse tek frame → güç hissi
- Follow-through → animasyon "fırlar" sonra yavaşlar

### Hit Stop (Combat Juice'un Çekirdeği):
- Vuruş anında oyun **2-5 frame durur** (time scale ~0.05)
- Hem vuran hem vurulan karakter dondurulur
- Hero Siege'de her saldırı bu şekilde çalışır
- **Uygulama:** `HitStop.cs` → `Time.timeScale` kısa süre düşür

### Animasyon FPS:
- Karakter animasyonları: **12 FPS** (8 FPS choppy, 24 FPS gereksiz)
- VFX partiküller: **real-time** (ayrı sistem)
- Idle: **8 FPS** yeterli (yavaş, sakin)

---

## 3. VFX ve Işık — Görsel Feedback Sistemi

Hero Siege'i "güzel" yapan şeyin %50'si VFX sistemidir.

### Her Vurulma Anında Olması Gerekenler (Priority 1):
- **Hit Flash:** Sprite beyaz ya da aksent renginde tek frame parlar → `HitFlash.cs`
- **Hit Particles:** Küçük spark/kan partikülleri (3-6 parçacık yeterli)
- **Screen Shake:** Hafif kamera titremesi (büyük hitlere göre scale)

### Skill Kullanımı (Priority 2):
- Skill cast'ta **glow/ışık patlaması** → Unity 2D Point Light anlık aktif
- Projectile'ların Trail'i olmalı
- AoE skillerin zemininde hafif renk dairesi (range indicator + VFX)

### Düşman Ölümü (Priority 2):
- Ölüm anında **renk flash** (kırmızı veya siyah)
- Particle burst (2-3 frame içinde patlama)
- Body dissolve ya da fade-out (hemen kaybolmasın)

### Ambient Işık (Priority 3):
- Dungeon: soğuk mavi ambient, sıcak turuncu/sarı torch ışıkları
- Karanlık + nokta ışıklar = atmosfer. Düz beyaz ışık olmayacak.
- Player'ın etrafında çok hafif aura (class renginde, intensity 0.1-0.2)

---

## 4. Renk Paleti — Okunabilirlik ve Stil

### Temel Kural: Hiyerarşi Var
```
Arka plan (en soluk) < Zemin/tile (orta) < Düşmanlar (kontrast) < Player (en parlak/net)
```

### RIMA Renk Kararları:
- **Arka plan/duvarlar:** Koyu, soğuk, muted (gri-mavi, siyah-mor)
- **Zemin:** Biraz daha açık ama hâlâ soğuk ton
- **Düşmanlar:** Düşük satürasyon, donuk renkler (zayıf, ölü hissi)
- **Player:** Güçlü silüet, class renginde ışık aksanı
  - Warblade: soğuk mavi/çelik
  - Elementalist: amber/turuncu
  - Shadowblade: mor/siyah
  - Ranger: yeşil/gri

- **Rift energy (RIMA'ya özel):** Mavi-mor crack light — her karakterde bir yerde görünmeli
- **Skill VFX:** Parlak, saturated, class renginde → arka plandan öne çıkar
- **UI:** Yüksek kontrast, küçük ikon, büyük rakam

### Palet Disiplini:
- Her karakter için 3-4 ana renk + highlight + shadow
- Çok fazla renk = amateur görünür
- Satürasyon: düşmanlar düşük (~30-50%), player ve skills yüksek (~60-80%)

---

## 5. Uygulama Öncelik Sırası

Bunların hangisi önce yapılır? (Combat feedback olmadan oyun "boş" hissettiriyor)

```
Priority 1 (Acil — play test için şart):
  → HitFlash.cs (sprite beyaz parlar)
  → HitStop.cs (2-5 frame freeze)
  → Temel hit particles (DamageZone'a ekle)

Priority 2 (Sonraki sprint):
  → Screen shake (küçük = her hit, büyük = boss hit)
  → Death VFX burst
  → Skill cast light flash (Point Light 2D)

Priority 3 (Faz 2):
  → Trail renderer (dash + projectile)
  → Ambient lighting sistemi
  → Player class aura
  → Enemy dissolve/death shader
```

---

## 6. Animasyon Kalite Kontrol Checklist (rima-qc icin)

Üretilen her animasyon bu standartları karşılıyor mu?

- [ ] Frame sayısı minimum değeri karşılıyor mu? (saldırı: 8+)
- [ ] Aksiyon okunuyor mu? (idle bakınca ne yaptığı anlaşılıyor)
- [ ] Windup → strike timing var mı? (sadece loop değil, güç hissi var)
- [ ] Siyah/boş frame yok mu?
- [ ] Yön tutarlı mı?
- [ ] Style referansla renk tutarlı mı?

