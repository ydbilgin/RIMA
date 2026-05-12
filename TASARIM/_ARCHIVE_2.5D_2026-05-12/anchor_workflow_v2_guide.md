---
status: ARCHIVED
faz: 1
tarih: 2026-05-12
ozet: "ARCHIVED: 2.5D dönem anchor workflow (S59 REVOKED)"
---
# RIMA — V1 Karakter Anchor + Animasyon Workflow (V2)

**Status:** DRAFT — Codex Review Bekliyor
**Önceki:** anchor_body_only_spec.md (SUPERSEDED — bu dosya geçerli)
**Bağlam:** Mevcut silahlı south anchor'lardan başla → Edit Image (silah sil) → Create from Reference (temiz south) → Custom Animation V3 (8 yön + animasyon)

---

## GENEL AKIŞ

```
Mevcut anchor (silahlı south)
        ↓
[ADIM 1] PixelLab Edit Image — silahı sil
        ↓
Silahsız south (ham versiyon)
        ↓
[ADIM 2] PixelLab Create Character → Create from Reference — temiz south üret
        ↓
Body-only south anchor (final)
        ↓
[ADIM 3] PixelLab Custom Animation V3 — 8 yön + animasyon kareleri
        ↓
8 yönlü animasyon seti (N/NE/E/SE/S/SW/W/NW)
```

---

## ADIM 1 — Edit Image: Silah Kaldırma

**Tool:** PixelLab Web App → **Edit Image**
**Amaç:** Mevcut south anchor'dan silahı sil, vücut ve el pozisyonu kalsın.

### Genel Ayarlar

| Setting | Değer |
|---|---|
| Tool | Edit Image |
| Mode | Erase / Inpaint |
| Background | Transparent |
| Mask | Sadece silahın bulunduğu bölge |

### Karakter Başına Talimatlar

#### Warblade
- **Kaynak:** `Characters/anchors/warblade/warblade_anchor.png`
- **Silinecek:** Büyük mavi greatsword — sağ tarafta, belden omza uzanan blade
- **Mask bölgesi:** Karakterin sağından uzanan kılıç gövdesi (blade + crossguard)
- **Prompt:** `Remove the large sword completely. Keep the warrior's right arm and hand in natural downward grip position with closed fist. Fill the empty space with background transparency. Do not alter the armor, cape, or body.`
- **Dikkat:** Kabza (grip) eli bozma — el yumruk pozisyonunda kalmalı

#### Ranger
- **Kaynak:** `Characters/anchors/ranger/ranger_anchor.png`
- **Silinecek:** Yay (bow) — sol elde, dikey uzanan yay gövdesi
- **Mask bölgesi:** Sol elin tuttuğu yay + üst/alt uzantıları
- **Prompt:** `Remove the bow from the left hand. Keep the left arm extended forward with a closed fist, as if still holding something. Maintain the natural arm pose. Fill removed area with transparency.`
- **Dikkat:** Sol kolun pozu (öne uzanmış) bozulmamalı

#### Shadowblade
- **Kaynak:** `Characters/anchors/shadowblade/shadowblade_anchor.png`
- **Silinecek:** Her iki eldeki mor dagger (2 adet)
- **Mask bölgesi:** Sol el dagger + sağ el dagger — ayrı ayrı iki mask işlemi yap
- **Prompt (her dagger için ayrı):** `Remove the purple dagger from this hand. Keep the hand in a closed fist with reverse grip (thumb pointing down). Fill with transparency.`
- **Dikkat:** Reverse grip pozisyonu (başparmak aşağı) korunmalı — iki el için ayrı işlem önerilir

#### Elementalist
- **Kaynak:** `Characters/anchors/elementalist/elementalist_anchor.png`
- **Silinecek:** Yüzen orb — sağ elin önünde, havada asılı
- **Mask bölgesi:** Orb'un bulunduğu alan (elden ayrı, sadece küre)
- **Prompt:** `Remove the floating orb. Keep the right arm extended with open palm gesture. The palm should remain empty and slightly glowing area should be replaced with transparency.`
- **Dikkat:** Orb elden ayrık olduğu için en temiz işlem — el pozu hiç bozulmaz

### Çıktı Kaydetme

```
Characters/anchors/<class>/body_only/south_edit.png
```

Örnek: `Characters/anchors/warblade/body_only/south_edit.png`

---

## ADIM 2 — Create from Reference: Temiz South Anchor

**Tool:** PixelLab Web App → **Create Character (NEW)** → **Create from Reference**
**Amaç:** Edit Image çıktısından daha temiz, yüksek kaliteli bir south anchor üret.

### Ayarlar

| Setting | Değer |
|---|---|
| Tool | Create Character (NEW) |
| Mode | Create from Reference |
| Reference Image | ADIM 1 çıktısı (`south_edit.png`) |
| Size | 128×128 |
| View | High top-down |
| Direction | South (facing camera) |
| Background | Transparent |
| Style | Bold |
| Detail | High |

### Prompt Şablonu

```
<class description>, body only without any weapon, hands in <weapon> grip
position holding nothing, standing combat ready pose, south facing camera,
high top-down view, pixel art, bold style, single color outline, transparent
background, no weapon visible, body-only character anchor
```

### Sınıf Prompt'ları

#### Warblade
```
Armored warrior with plate chest, short cape, body only, no sword,
both hands in downward greatsword grip position holding nothing,
fists closed, combat ready stance, south facing camera, high top-down
view, pixel art, bold style, transparent background
```

#### Ranger
```
Leather armor archer with white hair and cloak, body only, no bow,
left arm extended forward with closed fist as if holding bow grip,
right hand relaxed at side, south facing camera, high top-down view,
pixel art, bold style, transparent background
```

#### Shadowblade
```
Dark hooded assassin, lean build, body only, no daggers,
both hands in reverse grip fists (thumbs down) at hip sides,
elbows tucked, slight forward lean, south facing camera, high top-down
view, pixel art, bold style, transparent background
```

#### Elementalist
```
Robed mage with short hair, body only, no orb, no staff,
right arm extended forward with open palm casting gesture,
left hand at hip with palm up, upright posture, south facing camera,
high top-down view, pixel art, bold style, transparent background
```

### Çıktı Kaydetme

```
Characters/anchors/<class>/body_only/south.png
```

Bu dosya final anchor — Custom Animation V3'e girecek kaynak.

---

## ADIM 3 — Custom Animation V3: 8 Yön + Animasyon

**Tool:** PixelLab Web App → **Custom Animation V3**
**Amaç:** South anchor'dan 8 yön üret + animasyon kareleri oluştur.

### Ayarlar

| Setting | Değer |
|---|---|
| Tool | Custom Animation V3 |
| Source Image | `Characters/anchors/<class>/body_only/south.png` |
| Directions | 8 (N/NE/E/SE/S/SW/W/NW) |
| View | High top-down |

### Animasyon Listesi (V1 Öncelik Sırası)

Her sınıf için bu sırayla üret:

1. **Idle** — 4 kare, looping
2. **Walk** — 6-8 kare, looping
3. **Hurt** — 3 kare
4. **Death** — 5-6 kare
5. **Attack LMB** — sınıfa göre (Warblade: slash, Ranger: shoot, vb.)
6. **Dash** — 4 kare

### Sınıf Başına Animasyon Notu

| Sınıf | Idle özelliği | Attack özelliği |
|---|---|---|
| Warblade | Omuz nefesi, sword-ready | Horizontal slash arc |
| Ranger | Hafif yay germe, nefes | Draw → release, arrow visual engine-side |
| Shadowblade | Low crouch sway | Dash-stab, cross-slash |
| Elementalist | Orb/palm glow pulse (engine VFX) | Cast push, orb throw |

**Not — Elementalist:** Animasyon karesinde palm boş olmalı (open palm, no orb). Orb/glow engine-side VFX olarak eklenir. V3 promptuna "no orb, empty palm" ekle.

### Çıktı Klasör Yapısı

```
PIXELLAB_OUTPUTS/<class>/animations/
  idle/      → idle_N.png, idle_NE.png, ... idle_NW.png (8 yön)
  walk/      → walk_N.png, ...
  hurt/      → hurt_N.png, ...
  death/     → death_N.png, ...
  attack_lmb/ → attack_lmb_N.png, ...
  dash/      → dash_N.png, ...
```

---

## ÜRETİM SIRASI

| Sıra | Sınıf | Neden |
|---|---|---|
| 1 | **Shadowblade** | POC GATE pilot — en önce |
| 2 | **Warblade** | V1 ana karakter |
| 3 | **Ranger** | V1 |
| 4 | **Elementalist** | V1 |

Her sınıf için akış:
1. Edit Image (silah sil) → `south_edit.png`
2. Create from Reference → `south.png` (final anchor)
3. Custom Animation V3 → 8 yön × tüm animasyonlar

---

## KALİTE KRİTERLERİ

### Edit Image ACCEPT
- Silah tamamen yok, artefakt kalmamış
- El/kol pozisyonu bozulmamış
- Arka plan şeffaf, kenar temiz

### Create from Reference ACCEPT
- South anchor ile aynı sınıf silueti
- El grip pozisyonu reference ile uyumlu
- 128×128, %62-75 doluluk, bottom-center hizalı

### Custom Animation V3 ACCEPT
- 8 yön tutarlı (S referansı tüm yönlerde okunabilir)
- Animasyon loop'u sorunsuz (idle/walk ilk kare = son kare)
- Silüet her yönde sınıfı tanıtıyor

---

## ÖNCEKİ KARAR DEĞİŞİKLİKLERİ

| Eski Karar | Yeni Karar | Gerekçe |
|---|---|---|
| N/E/S prompt'tan üret, W=flipX | Sadece S üret, V3 8 yön döndürür | Custom Animation V3 8 yön otomatik — fazla iş yok |
| Create Character (NEW) direkt prompt | Edit Image → Create from Reference | Mevcut anchor kullanılıyor, kalite daha tutarlı |
| 4 yön kaynak, W=flipX game engine | V3 çıktısı 8 yön kaynak mevcut | Oyunda hâlâ 4+flip kullanılabilir ama kaynak 8 yönlü |

