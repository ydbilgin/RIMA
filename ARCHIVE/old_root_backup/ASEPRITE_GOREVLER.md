# Aseprite Görev Listesi
*Tüm hero karakterler 128px canvas. Plugin: Edit → PixelLab → Open Plugin (Ctrl+Space+P)*

---

## Canvas ve Scale Kararları

| Karakter | Canvas | PPU | Prefab scale | Ekranda |
|---|---|---|---|---|
| Warblade | 128px | 48 | 0.5 | 1.33 birim |
| Elementalist | 128px | 48 | 0.5 | 1.33 birim |
| Shadowblade | 128px | 48 | 0.5 | 1.33 birim |
| Ranger | 128px | 48 | 0.5 | 1.33 birim |
| Tüm mobs | 64px | 48 | tier'a göre | < 1 birim |

**Smoothness = frame sayısı:** Attack 8-10 frame, walk/idle 8 frame.

---

## GÖREV 1 — Warblade Base Sprite (128px) ⭐ ÖNCELİKLİ

**Neden önce bu:** Ürettiğin south.png tüm proje için style lock referansı.
**Nerede:** **pixellab.ai sitesinde** — base sprite üretimi için site daha iyi (pixflux model, hızlı iterasyon).

---

### 1A — Siteye gir

**pixellab.ai** → sol menüden **"Pixel Art Generator"** veya **"Create"** → **"Character"** seç.

Karşına karakter üretim formu gelir.

---

### 1B — Ayarlar (sitedeki form)

| Alan | Değer |
|---|---|
| **Size** | `128` (dropdown veya slider'dan) |
| **View** | `low top-down` |
| **Mode** | `Pro` (en yüksek kalite) |
| **Name** | `Warblade` |
| **Description** | Aşağıdaki metni kopyala-yapıştır |

**Description:**
```
tall imposing warrior with lean muscular build, long legs and broad shoulders,
NOT stocky NOT short NOT dwarf proportions, upright 6-head-height figure,
completely bare head with NO helmet NO visor NO headgear whatsoever,
short battle-worn hair clearly visible, scarred determined face with intense eyes
fully visible and prominent as the primary focal point of the character,
dark iron armor on body with open chest plate,
large two-handed greatsword held in right hand with blue-purple rift energy
crackling along blade edge, short tattered cape behind,
dominant upright combat stance, dark iron and cold steel blue palette,
rift energy blue-purple crack lines glowing at armor joints and sword edge,
dark fantasy roguelite, RIMA universe
```

**Generate** → 8 yönlü sprite sheet gelir → ZIP indir.

---

### 1C — Kontrol et

ZIP'i aç, `south.png`'ye bak:
- Yüz görünüyor, kask yok → PASS
- Hâlâ kasklıysa → Negative'e ekle: `helmet, visor, full face cover, face guard` → tekrar üret
- Çok kısa/bodur → Negative'e ekle: `stocky, short, dwarf, crouching, squatting`
- 3 denemede olmadıysa bana söyle

---

### 1D — Kaydet

ZIP'teki her yön PNG'sini doğrudan şuraya kopyala:
```
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Warblade\
```
İsimler: `Warblade_S.png`, `Warblade_N.png`, `Warblade_E.png`, `Warblade_W.png`, `Warblade_SE.png`, `Warblade_SW.png`, `Warblade_NE.png`, `Warblade_NW.png`
Eskinin üzerine yaz.

---

### 1E — Bitince söyle

**"warblade base bitti"** → Ben Unity'de prefab scale günceller, eski 96px animasyonları bağlantıdan kaldırırım.

---

## GÖREV 2 — Elementalist Base Sprite (128px)

**Önce GÖREV 1'i bitir.** Style lock için Warblade_S.png kullanılacak.
**Nerede:** pixellab.ai sitesi → Character.

---

### 2A — Siteye gir, ayarlar

| Alan | Değer |
|---|---|
| **Size** | `128` |
| **View** | `low top-down` |
| **Mode** | `Pro` |
| **Style image** | Warblade_S.png'yi yükle (sitede "style image" veya "reference" alanı) |
| **Name** | `Elementalist` |
| **Description** | Aşağıya bak |

**Description:**
```
female mage character, woman with clearly feminine face and build,
flowing open hair falling freely around face and shoulders as primary visual feature,
hood of robe hanging loosely down BEHIND back NOT on head,
sharp intelligent eyes clearly visible, composed expression,
dark flowing robes with deep purple and midnight blue tones,
holding a tall ornate staff with split amber-frost crystal at top as ONLY held object,
hands holding only the staff grip, no flames no orbs no fire in hands,
upright composed casting stance,
rift energy blue-purple glow on staff crystal and robe hem,
dark fantasy roguelite, RIMA universe
```

**Negative** (varsa): `male face, masculine jaw, hood on head, helmet, orbs in hands, flames in hands`

**Generate** → kontrol → ZIP indir → kaydet:
```
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Elementalist\
```
`Elementalist_S.png` ... `Elementalist_NW.png`

**Bitince:** "elementalist base bitti"

---

## GÖREV 3 — Shadowblade Base Sprite (128px)

**Nerede:** pixellab.ai sitesi → Character.
**Not:** Mevcut Shadowblade_S.png 96px ve blob görünümlü → tamamen yenisi.

---

### 3A — Sitede ayarlar

| Alan | Değer |
|---|---|
| **Size** | `128` |
| **View** | `low top-down` |
| **Mode** | `Pro` |
| **Style image** | Warblade_S.png yükle |
| **Name** | `Shadowblade` |

**Description:**
```
agile assassin character, lower face wrapped in dark cloth,
sharp glowing crimson-red eyes clearly visible as primary focal point,
layered dark indigo-charcoal leather armor with subtle deep violet trim at edges,
dual curved daggers with faint crimson rift energy glow on blade edges,
crouched predatory stance weight on balls of feet,
dark cloak with indigo-purple inner lining clearly visible,
deep indigo and charcoal palette NOT pure black, visible armor layer detail throughout,
glowing crimson eyes and dagger edges as light sources against dark body,
dark fantasy roguelite, RIMA universe
```

**Negative:** `pure black blob, faceless, shapeless, solid silhouette, mixels, amateur`

**Kontrol:** Kırmızı gözler görünüyor mu, vücut çok karanlık değil mi → PASS

**ZIP → kaydet:**
```
F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Shadowblade\
```
`Shadowblade_S.png` ... `Shadowblade_NW.png`

**Bitince:** "shadowblade base bitti"

---

## GÖREV 4 — Shadowblade lead-jab Animasyonu (8 yön)

**Önce GÖREV 3'ü bitir** — lead-jab yeni 128px Shadowblade_S.png üzerine yapılacak.

---

### Her yön için adımlar:

**1. O yönün sprite'ını aç:**
```
Shadowblade_S.png   → south için
Shadowblade_N.png   → north için
Shadowblade_E.png   → east için
... (hepsi Characters/Shadowblade/ klasöründe)
```

**2. Ctrl+Space+A** → **Animate with text (new)** penceresi açılır

Pencerede şu alanlar var:

- Üstte referans görseli için bir alan (küçük kare thumbnail)
- Altında **"Set reference image"** (veya sadece **"Set"**) butonu → **tıkla** (aktif layer/frame referans olarak seçilir)
- **Action description** metin kutusu → yöne göre aşağıdaki tablodan yaz
- **"Enhance prompt"** butonu (opsiyonel, basma)
- **Number of frames** slider → `8` yap
- **Output method** dropdown → `New frame`
- **Remove background** kutucuğu → ☑ açık
- **seed** → `0`

**Action description — yöne göre:**

| Yön | Action description |
|---|---|
| south | `shadowy assassin throws a fast lead jab punch directly toward camera (south), quick dagger strike forward, attack motion clearly aimed toward camera, no loop` |
| north | `shadowy assassin throws a fast lead jab punch directly away from camera (north), quick dagger strike backward away from camera, no loop` |
| east | `shadowy assassin throws a fast lead jab punch to the right (east), quick dagger strike toward right side, no loop` |
| west | `shadowy assassin throws a fast lead jab punch to the left (west), quick dagger strike toward left side, no loop` |
| south-east | `shadowy assassin throws a fast lead jab punch toward lower-right (south-east diagonal), dagger strike toward lower-right, no loop` |
| south-west | `shadowy assassin throws a fast lead jab punch toward lower-left (south-west diagonal), dagger strike toward lower-left, no loop` |
| north-east | `shadowy assassin throws a fast lead jab punch toward upper-right (north-east diagonal), dagger strike toward upper-right, no loop` |
| north-west | `shadowy assassin throws a fast lead jab punch toward upper-left (north-west diagonal), dagger strike toward upper-left, no loop` |

**Generate** → kontrol:
- Hızlı yumruk/bıçak hamlesi var mı? → PASS
- Yanlış yöne bakıyorsa / duruyorsa → seed değiştir, tekrar Generate
- 3 denemede olmadıysa bana söyle

**Export — her yön için:**
- **File → Export Sprite Sheet**
- Sheet type: **By Rows**
- Padding: **0**
- Kaydet:

```
...\Shadowblade\animations\lead-jab\south\     → frame_000.png ... frame_007.png
...\Shadowblade\animations\lead-jab\north\
...\Shadowblade\animations\lead-jab\east\
...\Shadowblade\animations\lead-jab\west\
...\Shadowblade\animations\lead-jab\south-east\
...\Shadowblade\animations\lead-jab\south-west\
...\Shadowblade\animations\lead-jab\north-east\
...\Shadowblade\animations\lead-jab\north-west\
```
Tam yol başı: `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Shadowblade\animations\lead-jab\`

Klasör yoksa oluştur.

**8 yön bitti → söyle:** "shadowblade lead-jab bitti" → Ben Unity clip'i kurarım.

---

## Genel

- Plugin bağlanmıyorsa: Aseprite kapat → VPN toggle → yeniden aç
- Her generate sonrası görseli kontrol et
- 3 denemede sonuç çıkmıyorsa bana söyle, description beraber düzeltelim
