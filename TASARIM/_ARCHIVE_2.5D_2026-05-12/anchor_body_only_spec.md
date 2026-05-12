# RIMA — Body-Only Anchor Üretim Spesifikasyonu (V1)

**Status:** LOCKED 2026-05-11
**Bağlam:** 2.5D mimari + Silah Anchor sistemi + 4 yön flip kuralı

---

## 1. TEKNİK SPESİFİKASYON

| Parametre | Değer |
|---|---|
| Canvas boyutu | **128×128 px** |
| Karakter yüksekliği (canvas içi) | **80–96 px** (canvas'ın %62–75'i) |
| PPU (Unity import) | **64** |
| Arka plan | **Transparent** (Web App: Transparent BG ON — chromakey gerekmez) |
| Format | **PNG, RGBA** |
| Pivot | **Bottom-center (0.5, 0.0)** |
| Outline | Soft 1px, anchor stiliyle uyumlu |

### Canvas Oranı

```
+------------------------+ 128px
|       head room        | ~19px (%15)
|         ___            |
|        /   \           | head ~20px
|        |___|           |
|       __|||__          | torso + arms ~40px
|      |  |||  |         |
|      |__|_|__|         |
|        / \             | legs ~30px
|       /   \            |
|      ground space      | ~13px (%10)
+------------------------+
```

Karakter yatay merkezde, alt %10 boş (billboard pivot bottom-center).

---

## 2. YÖNLER ve FLIP KURALI

Üretilecek 3 yön (her sınıf için):

| Yön | Açıklama | Yüz |
|---|---|---|
| **S (south)** | Yüz kameraya, ön görüş | Tam görünür |
| **E (east)** | Yan profil, sağa bakan | Yarım profil |
| **N (north)** | Sırt kameraya, omuz/kafa silueti | Görünmez |

**W = E.flipX** — runtime'da yapılır, ayrıca üretilmez.
NE/NW/SE/SW yönleri YOK (8 yön REVOKE LOCKED).

**Toplam:** 4 sınıf × 3 yön = **12 anchor**

---

## 3. POZ — "Empty Hands Gripping Air"

Karakter olmayan silahı tutuyormuş gibi konumlanmış — silah runtime'da WeaponAnchorMap'ten eklenir.

### Warblade (Greatsword — iki el dikey)
- Sağ el: bel hizasında yumruk, parmak boğumları ileri
- Sol el: sağ elin 8–10 px altında yumruk (kabza alt kısım)
- Eller dikey çizgide, vücudun **sağ tarafında**
- S yönü: eller göğüs ortasında dikey
- N yönü: sağ el sırtın sağında bel hizasında

### Ranger (Bow — sol el ileri, sağ el tele)
- Sol el: öne uzanmış, dirsek hafif kırık, yumruk dikey (bow grip)
- Sağ el: çene/yanak hizasında, parmaklar pinch (tel çekme), dirsek geride
- S yönü: sol el kameraya uzanmış, sağ el yüzün yanında

### Shadowblade (Dual daggers — bel hizası, ters grip)
- Her iki el: bel yanında, dirsekler vücuda yapışık
- **Ters grip**: başparmak aşağı bakar
- Hafif öne eğik silüet (stalker pose)
- S yönü simetrik; E yönde her iki el görünür

### Elementalist (Open palm cast pose)
- Sağ el: göğüs/omuz hizasında öne uzanmış, **açık avuç**, parmaklar hafif kıvrık
- Sol el: bel hizasında geride, açık avuç yukarı bakar
- Dik duruş (caster)
- S yönü: sağ el kameraya doğru

---

## 4. PIXELLAB WEB APP ADIM ADIM ÜRETIM

### Tool ve Ayarlar

**Tool:** PixelLab Web App → **Create Character (NEW)**

| Setting | Değer |
|---|---|
| Style | Bold |
| Size | 128×128 |
| View | High top-down |
| Direction | Tek yön: N / E / S |
| Detail | High |
| Outline | Single color outline |
| Shading | Detailed shading |
| **Background** | **Transparent** |

### Prompt Şablonu (Genel İskelet)

```
<class desc>, empty hands in <weapon> grip pose without weapon,
holding nothing, hands positioned as if holding <weapon>, standing combat ready,
high top-down view, <DIRECTION> facing, pixel art, bold style, single color outline,
no weapon visible, no props, body-only anchor pose, RIMA character anchor reference
```

**Not:** "no weapon visible" + "holding nothing" + "as if holding" üçü birlikte — tek başına "no weapon" yetersiz.

### Sınıf Prompt'ları

#### Warblade
```
Heavy armored warrior, plate chest piece, short cape, empty hands in
greatsword grip pose without weapon, both hands stacked vertically at hip
height holding nothing, fists clenched as if gripping a two-handed sword,
shoulders slightly rotated right, combat ready stance, high top-down view,
<DIRECTION> facing, pixel art, bold style, single color outline, detailed
shading, no sword visible, body-only anchor, RIMA Warblade reference
```

#### Ranger
```
Light leather armor archer, hooded cloak, empty hands in bow shooting pose
without weapon, left arm extended forward with fist closed as if gripping a
bow, right hand near the cheek with fingers pinched as if drawing a string,
no bow visible, no arrow visible, high top-down view, <DIRECTION> facing,
pixel art, bold style, single color outline, body-only anchor, RIMA Ranger reference
```

#### Shadowblade
```
Dark hooded assassin, lean silhouette, low stance, empty hands in reverse
dagger grip pose without daggers, both hands at hip sides with fists in
reverse grip (thumbs pointing down), elbows tucked to body, slight forward
lean, high top-down view, <DIRECTION> facing, pixel art, bold style, single
color outline, no daggers visible, body-only anchor, RIMA Shadowblade reference
```

#### Elementalist
```
Robed mage, long robe, hood down, empty hands in spell casting pose without
staff, right arm extended forward with open palm fingers slightly curled as
if channeling, left hand at hip behind with open palm facing up, upright
posture, high top-down view, <DIRECTION> facing, pixel art, bold style,
single color outline, no staff visible, no orb visible, body-only anchor,
RIMA Elementalist reference
```

### DIRECTION Değerleri

| Yön | Prompt değeri | Ek talimat |
|---|---|---|
| S | `south (facing camera)` | Full front view |
| E | `east (right side profile)` | Both arms visible |
| N | `north (back to camera)` | "head and shoulders visible, hood/hair detail on back" |

### Eraser Pass

Sadece 3 durumda kullan:
1. Model ısrarla silah ekledi → silahı sil
2. Gölge canvas dışına taştı → ground shadow sil
3. Arka planda artefakt kaldı

Karakter siluetine dokunma.

### Output Dosya İsimlendirme

```
Characters/anchors/<class>/body_only/s.png
Characters/anchors/<class>/body_only/e.png
Characters/anchors/<class>/body_only/n.png
```

Örnekler: `Characters/anchors/shadowblade/body_only/s.png`

W **üretilmez** — runtime flipX.

---

## 5. ÜRETİM SIRASI ve BATCH PLANI

### Sınıf Önceliği

1. **Shadowblade** (POC pilot — V1 Production Pipeline LOCKED) → 3 anchor
2. **Warblade** → 3 anchor
3. **Ranger** → 3 anchor
4. **Elementalist** → 3 anchor

### POC GATE Entegrasyonu

**POC için MİNİMUM:** Shadowblade S + E (2 anchor)

Bu 2 anchor ile POC'ta test edilmeli:
- WeaponAnchorMap ile dagger sprite el üzerine pixel-perfect oturuyor mu?
- Billboard 35° kamerada silüet okunabilir mi?
- W = E.flipX doğru çalışıyor mu?
- 64 PPU + 128px canvas tile boyutuyla uyumlu mu?

**POC FAIL ise:** Spec re-eval (boyut, karakter oranı). Diğer sınıflara geçilmez.

### Oturum Planı

| Oturum | Üretim |
|---|---|
| 1 | Shadowblade × 3 → POC GATE |
| 2 | Warblade × 3 |
| 3 | Ranger × 3 |
| 4 | Elementalist × 3 + batch Unity import |

Günde max 6 anchor önerilir.

---

## 6. KALİTE KRİTERLERİ

### ACCEPT

- Silüet okunur — 3m mesafede sınıf tanınır
- El pozisyonu silah tipiyle uyumlu (grip mantıklı)
- Bilek açısı silah ekseniyle tutarlı
- Karakter canvas'ta %62–75 dikey doluluk
- Pivot bottom-center hizalı
- Arka plan tamamen şeffaf (0 artefakt)

### REJECT

1. Silah görünür (eraser ile silinemiyorsa)
2. Yanlış grip (Warblade tek el, Ranger açık avuç vs.)
3. Karakter çok küçük (%55 altı doluluk)
4. Karakter canvas kenarına değiyor
5. N yönünde yüz görünüyor
6. Ara yön üretilmiş (NE/SW vb.)
7. Outline tutarsız
8. Sınıf siluetinden uzak drift

### WeaponAnchorMap ile Uyum

Silah sprite runtime'da **bilek anchor noktasına** parent'lanır. El pozisyonu yanlışsa:
- Silah havada görünür
- Kılıç yamuk asılı kalır
- AnimationCurve düzeltemez

Her anchor kabul edildiğinde el bileği koordinatını not al → `WeaponAnchorMap` JSON'a geçecek.

---

## LOCKED Uyum Referansı

| Karar | Kaynak |
|---|---|
| 4 yön + flipX | CURRENT_STATUS LOCKED 2026-05-11 |
| 2.5D billboard 35° ortho | project_2.5d_architecture.md |
| PPU 64 | project_weapon_anchor_system.md |
| PixelLab NEW > PRO | feedback_pixellab_new_over_pro.md |
| animate_character MCP YASAK | feedback_animate_character_mcp.md |
| Transparent BG (karakter araçları) | feedback_pixellab_create_character_transparent_bg.md |
| V1 Production Pipeline POC GATE | project_v1_production_pipeline.md |
