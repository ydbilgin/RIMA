# RIMA — Sanat Üretim Rehberi
*Son güncelleme: 2026-04-02 — Boyut hiyerarşisi kesinleşti, tüm eski karakterler silindi, MCP ile Pro mode üretim. —claude*

> **Bu belge TEK rehberdir.**

---

## ⚠️ BOYUT MANTIĞI — HADES TARZI

> Boyut = görsel tehdit/rol. Bazı moblar oyuncuyla aynı, hatta daha büyük olabilir.
> Hades'te Theseus, Minotaur oyuncudan dev. Loutlar oyuncudan küçük. Bu kasıtlı.

| Tip | size= | Mod | Yön | Animasyon yönü | Notlar |
|-----|-------|-----|-----|----------------|--------|
| Player class | **96px** | Pro | 8 | 8 | Baseline |
| Küçük/çevik mob | **48px** | Pro | 8 | 4 | Sürü tipleri, hızlı |
| Standart mob | **64px** | Pro | 8 | 4 | Çoğu grunt |
| Ağır mob | **96px** | Pro | 8 | 4 | Oyuncuyla boy ölçüşür — tehdit hissi |
| Elit mob | **80-112px** | Pro | 8 | 4 | Tipine göre base+16-24px |
| MiniBoss | **112px** | Pro | 8 | 8 | Oyuncudan açık büyük |
| Boss | **128px** | Pro | 8 | 8 | PixelLab max — ekranı doldurur |

**Her mob için boyut tasarım sorusu:** "Bu düşman oyuncuya bakınca küçük mü, eş mi, büyük mü görünmeli?"
- Küçük → 48-64px &nbsp; Eş → 80-96px &nbsp; Büyük → 96-128px+

---

## ÜRETİM KONTROL LİSTESİ

### PLAYER CLASSES (96px Pro, 8 yön)
Animasyonlar: fight-stance-idle-8-frames · walking-6-frames · running-6-frames · running-slide · lead-jab · falling-back-death

| Karakter | Karakter ID | Karakter | Animasyonlar |
|----------|-------------|----------|--------------|
| Warblade | `05c781e2` | ⏳ | ⏳ |
| Elementalist | — | bekliyor | bekliyor |
| Shadowblade | — | bekliyor | bekliyor |
| Ranger | — | bekliyor | bekliyor |
| Ravager | — | bekliyor | bekliyor |
| Hexer | — | bekliyor | bekliyor |
| Summoner | — | bekliyor | bekliyor |
| Templar | — | bekliyor | bekliyor |
| Hemomancer | — | bekliyor | bekliyor |

### MOBLAR (boyut moba göre, 8 yön karakter / 4 yön animasyon)
Animasyonlar: breathing-idle · walking-6-frames · lead-jab · falling-back-death

| Mob | Boyut | Tip | Karakter ID | Karakter | Animasyonlar |
|-----|-------|-----|-------------|----------|--------------|
| Shard Walker | **64px** | Standart | — | bekliyor | bekliyor |
| Void Thrall | **96px** | Ağır (oyuncu boyutu) | — | bekliyor | bekliyor |
| Seam Crawler | **48px** | Küçük/çevik | — | bekliyor | bekliyor |
| Echo Hound | **80px** | Orta | — | bekliyor | bekliyor |

### BOSSLAR (boyut bossa göre, 8 yön)
Animasyonlar: fight-stance-idle-8-frames · walking-6-frames · running-6-frames · lead-jab · cross-punch · falling-back-death

| Boss | Boyut | Karakter ID | Karakter | Animasyonlar |
|------|-------|-------------|----------|--------------|
| Iron Warden | **128px** | — | bekliyor | bekliyor |
| Fractured King | **128px** | — | bekliyor | bekliyor |
| Hollow Sovereign | **128px** | — | bekliyor | bekliyor |
| Nexus Core | **128px** | — | bekliyor | bekliyor |

---

## ÜRETİM SIRASI (her seferinde 1 Pro karakter)

```
1. create_character(size=96/64/128, mode="pro") → ID al
2. get_character(id) → south.png'yi ART/_REVIEW/'ye kaydet
3. Onayla → animate_character × 6 (8 job limit, sırayla)
4. Tüm animasyonlar bitince ZIP indir:
   curl --fail "https://api.pixellab.ai/mcp/characters/{ID}/download" -o karakter.zip
5. ZIP'i doğru yola çıkar:
   - Player: RIMA/Assets/Sprites/Characters/{İsim}/
   - Mob: RIMA/Assets/Sprites/Enemies/{İsim}/
   - Boss: RIMA/Assets/Sprites/Enemies/Bosses/{İsim}/
6. Unity'de: RIMA/2. Build {İsim} Animations menüsünü çalıştır
```

---

### 📋 İlerleme Takibi
Bir asset'i bitirdiğinde tablodaki hücreyi güncelle:
- `bekliyor` → `⏳ ID-kısa` → `✅`

---

## REFERANS ZİNCİRİ — NASIL ÇALIŞIYOR?

```
GEMİNİ görseli  ──(Init Image)──▶  BASE SPRITE  ──(Reference)──▶  TÜM ANİMASYONLAR
(konsept)                            (tek frame)                       (idle, walk, attack...)
```

**KURALLAR:**
1. **Gemini görseli** → sadece BASE üretirken Init Image olarak kullanılır
2. **BASE sprite** → TÜM animasyonlar için Reference Image olarak kullanılır
3. **Animasyonlar birbirinden bağımsız** — idle'dan attack türetME, hep BASE'den
4. Her animasyonu export ettikten sonra **Ctrl+Z ile BASE'e geri dön**, sonra sıradakini üret

### ⚠️ ARAÇ SEÇİM KURALI (PixelLab Resmi Docs)

| Canvas Boyutu | Doğru Araç | Menü Yolu |
|---------------|------------|----------|
| **≤ 48×48** (32×32, 16×16 vb.) | **Create S-M image** | `Create image > Create S-M image` |
| **≥ 64×64** (64×64, 128×128 vb.) | **Create M-XL image** | `Create image > Create M-XL image` |
| Tileset (herhangi boyut) | **Create tileset** | `Map > Create tileset` |
| Animasyon (herhangi boyut) | **Animate with text (New)** | `Animate > Animate with text (New)` |

> ⚠️ **Create S-M image** max canvas: 80×80 (Tier 1) / 140×140 (Tier 2+) — küçük sprite'lar için (grunt 32×32, tile 16×16)
> ⚠️ **Create M-XL image** min 32×32, max: 200×200 (free) / 320×320 (Tier 1) / 400×400 (Tier 2+) — karakter sprite'lar için (warblade 64×64, boss 128×128)
> ⚠️ **Animate with text (New)** max referans: 256×256, frame 4-16 (çift sayı), pixel bütçe: W×H×frame ≤ 524,288
> ⚠️ **Animate with text (New) — ÖNEMLI:** 2'li workflow kullan (aşağıda detaylı açıklama var). Frame Count çift sayı olmak zorunda (2, 4, 6, 8...).

### ⚠️ ANİMASYON ARASI BASE'E DÖNME KURALI

Her animasyonu export ettikten sonra, canvas'ta çoklu frame kalıyor.
**Sıradaki animasyona geçmeden önce BASE'e dönmen LAZIM:**

- **Önerilen (güvenli):** `File → Close` (kaydetme!) → `File → Open` → `xxx_S_BASE.aseprite` tekrar aç
- **Alternatif:** Ctrl+Z bas — Timeline'da tek frame kalana kadar (Ctrl+Z geçmişi derin değilse çalışmayabilir)

**Neden?** İdle frame'leri canvas'tayken attack üretirsen, PixelLab iyi sonuç vermez.

### ⚠️ ANİMATE WİTH TEXT (NEW) — DOĞRU ALANLAR

Yeni versiyon (ücretsiz, herkese açık) şu alanları destekler:
- **Reference Image** (zorunlu) — BASE sprite'ını seç
- **Animation Action** (zorunlu) — ne yapacağını yaz ("walking", "attacking" vb.)
- **Frame Count** — 4-16 arası, çift sayı zorunlu
- **Remove Background** — ON/OFF
- **Seed** — opsiyonel, aynı sonucu tekrar üretmek için

> ❌ Description, Camera View, Direction gibi alanlar **bu araçta YOK!**
> Onlar sadece "Create image" araçlarında var.

### ⚡ 2'Lİ FRAME WORKFLOW (ÖNERİLEN)

Tüm animasyonları bu şekilde üret — tek seferde yüksek frame count vermek yerine 2'li 2'li inşa et:

```
ADIM 1 — Frame Count: 2 → Generate
         [F1] [F2] ← 2 frame geldi, kontrol et

ADIM 2 — İyi bir frame'i Freeze yap, 2 daha Generate
         [F1] [F2🔒] [F3] [F4] ← şimdi 4 frame

ADIM 3 — Gerekirse tekrarla
         [F1] [F2🔒] [F3] [F4🔒] [F5] [F6] ← 6 frame
```

**Avantajları:**
- Her adımda kaliteyi görerek ilerlersin, beğenmediğini tekrar üretirsin
- Frozen frame sıradaki üretimi "yönlendirir" → daha tutarlı animasyon
- Pixel bütçesi: 2 frame = 4 frame'in yarısı maliyeti

**Ne zaman dur?** Her animasyonun altında hedef frame sayısı yazıyor. Ona ulaşınca export et.

---

## ⚠️ AÇI KONTROLÜ — HER GEMİNİ ÇEKİMİNDE YAP

Oyun top-down (tavandan bakış). PixelLab açıyı düzeltemez. Gemini'den gelen görsel yanlış açıdaysa PixelLab da yanlış üretir.

**✅ Doğru:** Omuzlar geniş, kafa tepeden küçük, yüz görünmüyor, ayaklar yok veya çok küçük (Hades / Enter the Gungeon gibi)
**❌ Yanlış:** Yüz görünüyor, yan profil, ayaklar omuzla aynı boyutta

**Gemini yanlış açı verdiyse** aynı chat'te şunu yaz:
`Now show the exact same character but viewed strictly from directly above, bird's eye aerial top-down perspective, head at top of frame, feet barely visible or hidden behind body`

---

## RIMA RENK PALETİ (referans için)

```
#080808  Void Black
#1E1E32  Deep Steel
#FFD700  Crack Gold
#D4D4F0  Ghost White
#7BA7BC  Act 1 — Cold Blue
#9E4FE0  Act 2 — Void Purple
#D4956A  Hub — Candle Warm
```

---

# ═══════════════════════════════════════
# FAZ 0 — Logo + Placeholder
# ═══════════════════════════════════════

---

## LOGO

Logo PixelLab boyut sınırını aşıyor. **Sadece Gemini ile üretilecek, Python script ile boyutlandırılacak.**

### ADIM 1 — Gemini'de logo üret
1. Tarayıcı aç → gemini.google.com → yeni sohbet başlat
2. Şu promptu kopyala yapıştır → Enter:
```
A dark fantasy pixel art game logo. The large uppercase letters "RIMA" are written
boldly in dark steel color, spanning the upper portion of the image.
Below and to the right of the letter "I", the small lowercase letters "ft" hang
downward at an angle — as if they cracked off from the word RIFT and are drooping,
falling debris from a broken seal. They are smaller, slightly rotated, cracked in texture.
Below and to the right of the letter "A", the small lowercase letters "rch" hang
downward at the same angle — as if they cracked off from the word MARCH, same drooping effect.
At the exact break points where "ft" detached from "RI" and "rch" detached from "MA",
brilliant gold light bleeds through the crack, color #FFD700.
Letter color: dark steel #1E1E32. Background: void black #080808.
The hidden full words RIFT and MARCH become readable on closer inspection.
Pixel art logo style, high contrast, kintsugi dark aesthetic.
```
3. Kontrol: "ft" harfi I'nın altından sarkıyor mu? "rch" A'nın altından sarkıyor mu? Kırık noktalarda altın parlaması var mı?
   Yanlışsa: `Regenerate — ft must hang below-right of I, rch must hang below-right of A, stronger gold glow at break points`
4. Beğendiğinde → görsele sağ tık → Farklı kaydet → `F:\Antigravity Projeler\2d roguelite\ART\logo\rima_logo_kaynak.png`

### ADIM 2 — Python ile boyutlandır
1. Terminal aç (Windows'ta: Win+R → cmd)
2. Şu komutu çalıştır:
```
cd "F:\Antigravity Projeler\2d roguelite"
python tools/logo_resize.py "ART/logo/rima_logo_kaynak.png"
```
3. Otomatik üretilen dosyalar:
   - `ART/logo/rima_logo_320x80.png` — Ana logo (menü, Steam)
   - `ART/logo/rima_logo_160x40.png` — Kompakt (loading, UI köşe)
   - `ART/logo/rima_icon_64x64.png` — Kare ikon (Steam küçük resim)

---

## PLACEHOLDER'LAR

Kalite önemsiz. Sadece Unity test için. Gemini ile üret, kaydet, bitti.

| Dosya | Boyut | Gemini Prompt |
|-------|-------|---------------|
| `ART/placeholder/warblade_placeholder.png` | 64×64 | `Simple pixel art placeholder sprite, dark blue #1E1E32 filled rectangle, large gold letter W centered #FFD700, transparent background, 64x64 game sprite` |
| `ART/placeholder/grunt_placeholder.png` | 32×32 | `Simple pixel art placeholder sprite, dark red #3D0000 filled square, white letter G centered, transparent background, 32x32 game sprite` |
| `ART/placeholder/floor_placeholder.png` | 16×16 | `Simple pixel art tile placeholder, dark grey stone #1E1E32, thin diagonal crack line #2A2A4A, 16x16 game tile` |
| `ART/placeholder/wall_placeholder.png` | 16×16 | `Simple pixel art tile placeholder, near-black #080808 stone tile with single horizontal dark line #1E1E32, 16x16 game wall tile` |

Her biri için: Gemini'de üret → sağ tık kaydet → yukarıdaki yola kaydet. Bitti.

---

# ═══════════════════════════════════════
# FAZ 1 — Core Loop
# ═══════════════════════════════════════

---

# WARBLADE — Ana Karakter

---

## WARBLADE BASE Sprite

### ADIM 1 — Gemini'de top-down referans üret
1. Tarayıcı aç → gemini.google.com → yeni sohbet başlat
2. Şu promptu kopyala yapıştır → Enter:
```
A heavily armored warrior viewed strictly from directly above, bird's eye aerial
top-down perspective — as if a camera is mounted on the ceiling looking straight down.
The warrior holds a large greatsword in their right hand, blade pointing downward
at their side. Their dark iron plate armor has a crack across the chest with cold
blue light bleeding through. Short torn cape visible behind the shoulders.
Heavy angular pauldrons visible from above. Head small at top, wide shoulders,
no face visible — only the top of the helmet. Feet barely visible or hidden.
The art style is retro pixel art, limited color palette: dark iron grey, cold blue,
worn gold. Transparent background, no background elements.
```
3. Açı kontrolü: omuzlar geniş ✓ | yüz görünmüyor ✓ | ayaklar küçük/yok ✓
   Yanlışsa aynı chat'te: `Now show the exact same character but viewed strictly from directly above, bird's eye aerial top-down perspective, head at top of frame, feet barely visible or hidden behind body`
4. Beğendiğinde → görsele sağ tık → Farklı kaydet →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_gemini_base.png`

### ADIM 2 — Aseprite'ta yeni dosya oluştur
1. Aseprite aç
2. **File → New**
3. Width: **64** | Height: **64** | Color Mode: **RGB** | Background: **Transparent** → **OK**
4. **File → Save As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_BASE.aseprite`

### ADIM 3 — Gemini görselini canvas'a al
1. Windows Explorer aç → şu dosyayı bul:
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_gemini_base.png`
2. Bu dosyayı Aseprite canvas'ına **sürükle bırak**
3. Layer panelinde gelen layer'a **çift tık** → ismini **`REFERANS`** yap

### ADIM 4 — PixelLab'de BASE sprite üret
1. Aseprite üst menüden: **Edit → PixelLab → Open plugin**
2. Sağ panelde araç listesi açılır
3. ⚠️ **"Create M-XL image"** yazısına tıkla (64×64 = M-XL kullan!)
4. Şu alanları doldur:
   - **Description** alanına tıkla, şunu gir:
     `Heavily armored dark iron plate warrior, large greatsword in right hand pointing downward, cracked breastplate glowing cold blue, torn cape, heavy pauldrons, facing downward (south), dark fantasy, top-down view`
   - **Negative Description** alanına:
     `side view, face visible, front facing, 3/4 view`
   - **Camera View** dropdown'ına tıkla → **"Low top-down"** seç
   - **Direction** dropdown'ına tıkla → **"South"** seç
   - **Outline** dropdown → **"Selective outline"** seç
   - **Shading** dropdown → **"Basic shading"** seç
   - **Details** dropdown → **"Medium detail"** seç
   - **Init Image** bölümüne bak → **"Set"** butonuna tıkla →
     `warblade_gemini_base.png` dosyasını bul, seç → **Open**
   - **Init Image Strength** slider'ını **500** değerine getir
     *(0 = init image'ı tamamen yoksay, sıfırdan üret | 1000 = init image'ı neredeyse kopyala | 500 = dengeli: Gemini görseli şekli/pozu yönlendirir, PixelLab pixel art kalitesi ekler. Gemini açısı iyi geldiyse 600-700'e çek. Yeterince pixel art çıkmıyorsa 300-400'e indir.)*
   - **Background Removal** toggle'ına tıkla → mavi/aktif hale getir (ON)
   - **Output Method** → **"Add new layer"** seç
5. **"Generate"** butonuna tıkla
6. Sonuç yeni layer olarak gelir
   - Beğenmediysen → tekrar **Generate** bas
   - Beğendiysen → devam et
7. **REFERANS** layer'ını sil: Layer panelinde sağ tık → **Delete Layer**

### ADIM 5 — Kaydet ve export et
1. **CTRL+S** → `warblade_S_BASE.aseprite` kaydedilir
2. **File → Export As**
3. Yol: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_BASE.png`
4. **Export**

---

## WARBLADE IDLE Animasyonu
**Referans olarak ürettiğin `warblade_S_BASE.png` kullanılacak.** (Gemini görseli DEĞİL!)

> ⚡ Her animasyondan sonra **Ctrl+Z ile tek frame'e dön** (baştaki Ctrl+Z kuralına bak).
> İlk animasyonsa bunu okuyorsun demek — BASE zaten tek frame, devam et.

### ADIM 1 — BASE dosyasını aç
`warblade_S_BASE.aseprite` zaten açıksa devam et.
Değilse: **File → Open** → `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_BASE.aseprite`

### ADIM 2 — PixelLab Animate with text (New) — 2'li workflow örneği
1. **Edit → PixelLab → Open plugin**
2. Araç listesinde **Animate >** → **"Animate with text (New)"** yazısına tıkla
3. Alanları doldur:
   - **Reference Image** → **"Set"** → `warblade_S_BASE.png` seç → **Open**
   - **Animation Action**: `standing idle, slow breathing, slight weapon sway, cold blue light pulsing from chest crack`
   - **Remove Background** → ON
4. **Tur 1:** Frame Count → **2** → **Generate**
   View → Timeline (Ctrl+T) → Space ile oynat. İyi görünüyorsa devam.
5. **Tur 2:** Son frame'i **Freeze** et → Frame Count → **2** → **Generate**
   Şimdi 4 frame var. Space ile oynat. Beğendiysen export'a geç.
   Beğenmediysen: son 2 frame'i tekrar Generate ya da sadece 1 frame freeze edip tekrar üret.
6. **Hedef: 4 frame** (idle için yeterli)

### ADIM 3 — Export Sprite Sheet
1. **File → Close** (KAYDETME!) → Yeni animasyon üretmeden önce BASE'e dön.
   *(Export'tan ÖNCE kaydetmek istersen: CTRL+S → kaydet, sonra export, sonra close.)*
2. **File → Export Sprite Sheet**
3. Şu ayarları yap:
   - **Sheet Type**: By Rows
   - **Columns**: **4** | **Rows**: 1
   - **Trim**: ✓ işaretle | **Extrude**: 1
   - **Output File**: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_idle.png`
   - **JSON Output**: ✓ işaretle (aynı klasöre aynı isimle `.json` üretilir)
4. **Export**

---

## WARBLADE WALK Animasyonu
**Referans: `warblade_S_BASE.png`**
> ⚡ **2'li workflow kullan** (IDLE örneğine bak). Aşağıdaki Frame Count = hedef, 2'li 2'li inşa et.

### ADIM 1 — BASE dosyasını aç
`warblade_S_BASE.aseprite` açık değilse: **File → Open** → aç.

### ADIM 2 — PixelLab Animate with text (New)
1. **Edit → PixelLab → Open plugin**
2. **Animate >** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` seç → Open
   - **Animation Action**: `walking south, heavy armored footsteps, armor plates shifting with weight, cape moving`
   - **Frame Count** → **6**
   - **Remove Background** → ON
4. **Generate**. Beğenmediysen tekrar Generate.
5. View → Timeline → 6 frame kontrol et.

### ADIM 3 — Export
1. **File → Export Sprite Sheet**
   - Sheet Type: By Rows | Columns: **6** | Rows: 1
   - Trim: ✓ | Extrude: 1
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_walk.png`
   - JSON: ✓
2. **Export**

---

## WARBLADE ATTACK1 Animasyonu
**Referans: `warblade_S_BASE.png`**

1. `warblade_S_BASE.aseprite` aç (açıksa devam et)
2. **Edit → PixelLab → Open plugin** → **Animate >** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` → Open
   - **Animation Action**: `powerful horizontal greatsword slash, windup pulling sword back then strong swing`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Kontrol et. Beğendiysen:
5. **File → Export Sprite Sheet**
   - Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_attack1.png`
6. **Export**

---

## WARBLADE ATTACK2 Animasyonu
**Referans: `warblade_S_BASE.png`**

1. `warblade_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **Animate >** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` → Open
   - **Animation Action**: `raising greatsword overhead with both hands then slamming into the ground, shockwave on impact`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet**
   - Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_attack2.png`
6. **Export**

---

## WARBLADE DASH Animasyonu
**Referans: `warblade_S_BASE.png`**

1. `warblade_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` → Open
   - **Animation Action**: `explosive forward dash, body leaning aggressively forward, blur of motion`
   - **Frame Count** → **4** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet**
   - Columns: **4** | Trim: ✓ | Extrude: 1 | JSON: ✓
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_dash.png`
6. **Export**

*Not: Dash afterimage efekti Unity shader ile yapılacak. Art gerekmez.*

---

## WARBLADE HURT Animasyonu
**Referans: `warblade_S_BASE.png`**

1. `warblade_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` → Open
   - **Animation Action**: `staggering sharply backward from impact, brief recoil flinch`
   - **Frame Count** → **4** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet**
   - Columns: **4** | Trim: ✓ | Extrude: 1 | JSON: ✓
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_hurt.png`
6. **Export**

---

## WARBLADE DEATH Animasyonu
**Referans: `warblade_S_BASE.png`**

1. `warblade_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `warblade_S_BASE.png` → Open
   - **Animation Action**: `slowly collapsing forward, armor cracking apart, cold blue energy dissipating`
   - **Frame Count** → **8** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet**
   - Columns: **8** | Trim: ✓ | Extrude: 1 | JSON: ✓
   - Output: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\warblade\warblade_S_death.png`
6. **Export**

*Not: Death dissolve efekti Unity shader ile yapılacak. Art gerekmez.*

---

# SHARD WALKER — Act 1 Grunt

---

## SHARD WALKER BASE Sprite

### ADIM 1 — Gemini'de referans üret
1. gemini.google.com → yeni sohbet
2. Prompt:
```
A humanoid enemy assembled entirely from floating broken stone shards and bone fragments.
There is no solid body — only hovering pieces arranged in a vague warrior shape,
with cold blue light bleeding through the gaps between the shards.
Viewed from directly above. The art style is retro pixel art with a dark stone
and cold blue color palette. The background must be transparent.
```
3. Açı kontrolü (aynı kural). Beğenince → sağ tık kaydet →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_gemini_base.png`

### ADIM 2 — Aseprite'ta yeni dosya
1. Aseprite → **File → New** → Width: **32** | Height: **32** | Background: Transparent → OK
2. **File → Save As** →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_BASE.aseprite`

### ADIM 3 — Gemini görselini al
1. `grunt_shard_gemini_base.png` dosyasını Aseprite canvas'ına sürükle bırak
2. Layer panelinde → çift tık → isim: **`REFERANS`**

### ADIM 4 — PixelLab BASE üret
1. **Edit → PixelLab → Open plugin**
2. **"Create S-M image"** tıkla
3. Alanları doldur:
   - **Description**: `Humanoid creature assembled from floating broken stone shards, cold blue light bleeding through the gaps, no solid body, hovering shard warrior shape, facing downward, dark fantasy enemy`
   - **Negative Description**: `side view, face visible, front facing, 3/4 view`
   - **Camera View** → **"Low top-down"**
   - **Direction** → **"South"**
   - **Outline** → **"Selective outline"**
   - **Shading** → **"Basic shading"**
   - **Details** → **"Medium detail"**
   - **Init Image → Set** → `grunt_shard_gemini_base.png` seç → Open
   - **Init Image Strength** → **500**
   - **Background Removal** → ON
   - **Output Method** → **"Add new layer"**
4. **Generate**. Beğendiysen → REFERANS layer sil.

### ADIM 5 — Kaydet ve export
1. **CTRL+S**
2. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_BASE.png`
3. **Export**

---

## SHARD WALKER IDLE Animasyonu
**Referans: `grunt_shard_S_BASE.png`**

1. `grunt_shard_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `grunt_shard_S_BASE.png` → Open
   - **Animation Action**: `hovering in place, stone shards slowly rotating, cold blue light pulsing`
   - **Frame Count** → **4** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **4** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_idle.png`
6. **Export**

---

## SHARD WALKER WALK Animasyonu
**Referans: `grunt_shard_S_BASE.png`**

1. `grunt_shard_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `grunt_shard_S_BASE.png` → Open
   - **Animation Action**: `moving forward, shards separating and rejoining with each step`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_walk.png`
6. **Export**

---

## SHARD WALKER ATTACK Animasyonu
**Referans: `grunt_shard_S_BASE.png`**

1. `grunt_shard_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `grunt_shard_S_BASE.png` → Open
   - **Animation Action**: `one arm shard launches forward as projectile then snaps back`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_attack.png`
6. **Export**

---

## SHARD WALKER DEATH Animasyonu
**Referans: `grunt_shard_S_BASE.png`**

1. `grunt_shard_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `grunt_shard_S_BASE.png` → Open
   - **Animation Action**: `all shards exploding outward simultaneously, cold blue burst then gone`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_shard\grunt_shard_S_death.png`
6. **Export**

---

# VOID THRALL — Act 1 Grunt

*Split efekti Unity'de iki sprite ile yapılır. 3 ayrı BASE sprite üretilecek, animasyon yok.*

---

## VOID THRALL — TAM FORM BASE

### ADIM 1 — Gemini
1. gemini.google.com → yeni sohbet
2. Prompt:
```
A dark armored humanoid soldier stands menacingly. Running vertically through
the center of their chest is a deep glowing crack, with void purple energy
seeping out from the fissure. The overall silhouette is sinister and medieval.
Viewed from directly above. The art style is retro pixel art with a dark iron
and void purple color palette. The background must be transparent.
```
3. Açı kontrol. Beğenince kaydet →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_thrall\grunt_thrall_gemini_base.png`

### ADIM 2 — Aseprite
1. **File → New** → Width: **32** | Height: **32** | Background: Transparent → OK
2. **File → Save As** →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_thrall\grunt_thrall_S_BASE.aseprite`

### ADIM 3 — Gemini görselini al
1. `grunt_thrall_gemini_base.png` → canvas'a sürükle → layer ismi **`REFERANS`**

### ADIM 4 — PixelLab
1. **Edit → PixelLab → Open plugin** → **"Create S-M image"** tıkla
2. Alanları doldur:
   - **Description**: `Dark armored soldier with deep glowing vertical crack through chest, void purple energy seeping from the fissure, sinister medieval enemy, facing downward, dark fantasy`
   - **Negative Description**: `side view, face visible, front facing, 3/4 view`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
   - **Init Image → Set** → `grunt_thrall_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
3. **Generate**. Beğendiysen → REFERANS sil.

### ADIM 5 — Export
**CTRL+S** → **File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_thrall\grunt_thrall_S_BASE.png`

---

## VOID THRALL — SOL YARI BASE
*Unity'de split efekti için. Tam form BASE'den sonra yap.*

### ADIM 1 — Gemini
1. gemini.google.com → yeni sohbet
2. Prompt:
```
Pixel art sprite, top-down 2D dark fantasy game, transparent background,
LEFT HALF ONLY of a dark armored soldier, cleanly split vertically down the center,
void purple energy glowing at the split edge #9E4FE0, still aggressive, facing south
```
3. Kaydet → `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_thrall\grunt_thrall_gemini_half_left.png`

### ADIM 2-4 — Aseprite + PixelLab
1. **File → New** → 32×32 → Transparent → **File → Save As** → `grunt_thrall_half_left_S_BASE.aseprite`
2. Gemini görselini canvas'a sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
4. Alanları doldur:
   - **Description**: `Left half only of a dark armored soldier split vertically, void purple energy glowing at split edge, right half absent, facing downward`
   - **Negative Description**: `side view, face visible, full body`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
   - **Init Image → Set** → `grunt_thrall_gemini_half_left.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
5. **Generate**. REFERANS sil.

### ADIM 5 — Export
**CTRL+S** → **File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\grunt_thrall\grunt_thrall_half_left_S_BASE.png`

---

## VOID THRALL — SAĞ YARI BASE
*Sol yarı ile aynı adımlar. Sadece "LEFT" → "RIGHT" değişir.*

### ADIM 1 — Gemini
Aynı prompt, `LEFT HALF ONLY` → `RIGHT HALF ONLY` yap. Kaydet →
`grunt_thrall_gemini_half_right.png`

### ADIM 2-5 — Aseprite + PixelLab + Export
Sol yarı ile aynı adımlar:
- Description: `Right half only of a dark armored soldier...`
- Init Image: `grunt_thrall_gemini_half_right.png`
- Save: `grunt_thrall_half_right_S_BASE.aseprite`
- Export: `grunt_thrall_half_right_S_BASE.png`

---

# IRON WARDEN — Act 1 Boss

⚠️ **128×128 büyük sprite — Create S-M image DEĞİL, "Create M-XL image" kullan!**
⚠️ 128×128 animasyonlarda max 8 frame güvenli.

---

## IRON WARDEN BASE Sprite

### ADIM 1 — Gemini
1. gemini.google.com → yeni sohbet
2. Prompt:
```
A massive iron golem guardian towers over the battlefield. Its full dark plate
armor is heavily damaged, covered in deep cracks from which cold blue energy
seeps through. Several broken sword shards are embedded in its back and shoulders
like trophies. The figure radiates an overwhelming, slow, unstoppable presence.
Viewed from directly above. The art style is retro pixel art with a dark iron
and cold blue color palette. The background must be transparent.
```
3. Açı kontrol. Boyutu büyük, başı küçük, omuzlar çok geniş olmalı. Kaydet →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_gemini_base.png`

### ADIM 2 — Aseprite
1. **File → New** → Width: **128** | Height: **128** | Background: Transparent → OK
2. **File → Save As** →
   `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_BASE.aseprite`

### ADIM 3 — Gemini görselini al
`boss_iron_warden_gemini_base.png` → canvas'a sürükle → layer ismi **`REFERANS`**

### ADIM 4 — PixelLab BASE üret
1. **Edit → PixelLab → Open plugin**
2. ⚠️ **"Create M-XL image"** tıkla (S-M image DEĞİL!)
3. Alanları doldur:
   - **Description**: `Massive heavily armored iron golem guardian, full dark plate armor with deep cracks, cold blue energy seeping through cracked breastplate, broken swords embedded in shoulders, imposing top-down view, overwhelming dark fantasy boss`
   - **Negative Description**: `side view, face visible, front facing`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"High detail"**
   - **Init Image → Set** → `boss_iron_warden_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
4. **Generate**. Beğendiysen → REFERANS sil.

### ADIM 5 — Export
**CTRL+S** → **File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_BASE.png`

---

## IRON WARDEN IDLE Animasyonu
**Referans: `boss_iron_warden_S_BASE.png`**

1. `boss_iron_warden_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `boss_iron_warden_S_BASE.png` → Open
   - **Animation Action**: `massive iron armored golem, standing menacingly still, very slow heavy breathing, armor plates barely shifting, cold blue glow pulsing`
   - **Frame Count** → **6** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_idle.png`
6. **Export**

---

## IRON WARDEN ATTACK1 Animasyonu
**Referans: `boss_iron_warden_S_BASE.png`**

1. `boss_iron_warden_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `boss_iron_warden_S_BASE.png` → Open
   - **Animation Action**: `raising massive sword then devastating overhead slam, ground shockwave on impact`
   - **Frame Count** → **8** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **8** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_attack1.png`
6. **Export**

---

## IRON WARDEN CHARGE Animasyonu
**Referans: `boss_iron_warden_S_BASE.png`**

1. `boss_iron_warden_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `boss_iron_warden_S_BASE.png` → Open
   - **Animation Action**: `slow windup pulling shield back, explosive forward shield charge, heavy collision`
   - **Frame Count** → **8** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **8** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_charge.png`
6. **Export**

---

## IRON WARDEN HURT Animasyonu
**Referans: `boss_iron_warden_S_BASE.png`**

1. `boss_iron_warden_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `boss_iron_warden_S_BASE.png` → Open
   - **Animation Action**: `massive body barely flinching from hit, minor stagger, shrugging it off`
   - **Frame Count** → **4** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **4** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_hurt.png`
6. **Export**

---

## IRON WARDEN DEATH Animasyonu
**Referans: `boss_iron_warden_S_BASE.png`**

1. `boss_iron_warden_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"** tıkla
3. Alanları doldur:
   - **Reference Image → Set** → `boss_iron_warden_S_BASE.png` → Open
   - **Animation Action**: `slowly collapsing forward, massive armor cracking open, cold blue energy erupting then dispersing and fading`
   - **Frame Count** → **8** | **Remove Background** → ON
4. **Generate**. Beğendiysen:
5. **File → Export Sprite Sheet** → Columns: **8** | Trim: ✓ | Extrude: 1 | JSON: ✓
   Output: `F:\Antigravity Projeler\2d roguelite\ART\dusmanlar\boss\iron_warden\boss_iron_warden_S_death.png`
6. **Export**

---

# ACT 1 TİLE SETİ

*Tile'lar için "Create tileset" aracı kullanılır. Gemini referans opsiyonel.*

---

## FLOOR TILE — 16×16 (3 varyasyon)

### ADIM 1 — Gemini (opsiyonel referans)
1. gemini.google.com → Prompt:
```
Pixel art tile, 16x16 pixels, seamless tileable, top-down 2D dark fantasy
dungeon floor, dark grey cracked cobblestone #1E1E32 with thin cold blue
glowing fissures #7BA7BC, dungeon atmosphere, transparent background,
3 slight variations of the same tile design
```
2. Kaydet → `ART\tiles\act1\act1_floor_gemini_ref.png`

### ADIM 2 — Aseprite
1. **File → New** → Width: **16** | Height: **16** | Background: Transparent → OK
2. **File → Save As** →
   `F:\Antigravity Projeler\2d roguelite\ART\tiles\act1\act1_floor_tileset.aseprite`

### ADIM 3 — PixelLab Create tileset
1. **Edit → PixelLab → Open plugin**
2. **"Create tileset"** tıkla
3. Alanları doldur:
   - **Description (Inner tile)**: `Dark grey cracked cobblestone dungeon floor, thin cold blue glowing fissures, seamless tileable, top-down dark fantasy`
   - **Description (Outer tile / Transition)**: `Dark grey cracked cobblestone, edge transition, cold blue fissures, top-down dark fantasy`
   - **Type** → **"Top-Down"** seç
   - **Tile Size** → **16** gir
   - **Remove Background** → ON
4. **Generate** tıkla
5. Önizleme için: **View → Tiled Mode** işaretle

### ADIM 4 — 3 Varyasyon export
- 1. Generate → **File → Export As** →
  `F:\Antigravity Projeler\2d roguelite\ART\tiles\act1\act1_floor_01.png`
- **Generate** → **File → Export As** →
  `...\act1_floor_02.png`
- **Generate** → **File → Export As** →
  `...\act1_floor_03.png`

---

## WALL TILE — 16×32 (2 varyasyon)

### ADIM 1 — Gemini (opsiyonel)
Prompt:
```
Pixel art tile, 16x32 pixels, seamless tileable, top-down dark fantasy game
wall, crumbling fortress stone, cold blue barely visible in cracks,
dark stone texture #080808 #1E1E32, game wall tile, transparent background
```
Kaydet → `act1_wall_gemini_ref.png`

### ADIM 2 — Aseprite
**File → New** → Width: **16** | Height: **32** | Background: Transparent
**File → Save As** → `act1_wall_tileset.aseprite`

### ADIM 3 — PixelLab
1. **Edit → PixelLab → Open plugin** → **"Create tileset"** tıkla
2. Alanları doldur:
   - **Description (Inner)**: `Crumbling dark fortress stone wall tile, cold blue light barely in cracks, seamless tileable, top-down dark fantasy`
   - **Type** → **"Top-Down"** | **Tile Size** → **16** | **Remove Background** → ON
3. **Generate** → Varyasyon 1: Export As → `act1_wall_01.png`
4. **Generate** → Varyasyon 2: Export As → `act1_wall_02.png`

---

## FLOOR CRACK OVERLAY — 16×16 (4 varyasyon)

### ADIM 1 — Gemini (opsiyonel)
Prompt:
```
Pixel art overlay sprite, 16x16 pixels, transparent background,
thin crack line patterns only — no fill, 1-2 pixel wide,
cold blue glow color #7BA7BC, dark fantasy floor decoration overlay,
4 different crack angle variations, game decor sprite
```

### ADIM 2 — Aseprite
**File → New** → **16×16** | Background: Transparent
**File → Save As** → `act1_crack_overlay.aseprite`

### ADIM 3 — PixelLab
1. **Edit → PixelLab → Open plugin** → **"Create S-M image"** tıkla
2. Alanları doldur:
   - **Description**: `Thin crack line pattern only, no fill, 1-2 pixel wide, cold blue glow, dark fantasy floor decoration overlay, transparent background`
   - **Negative Description**: `solid fill, background, stone texture`
   - **Camera View** → **"None"** | **Direction** → **"None"**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
3. **Generate** → Export As → `act1_crack_01.png`
4. Tekrar **Generate** → `act1_crack_02.png`
5. Tekrar **Generate** → `act1_crack_03.png`
6. Tekrar **Generate** → `act1_crack_04.png`

Tüm export yolları: `F:\Antigravity Projeler\2d roguelite\ART\tiles\act1\`

---

# FAZ 1 VFX

---

## HIT SPARK VFX

### ADIM 1 — Aseprite
**File → New** → **32×32** | Background: Transparent
**File → Save As** →
`F:\Antigravity Projeler\2d roguelite\ART\vfx\vfx_hit_spark.aseprite`

### ADIM 2 — PixelLab BASE üret
1. **Edit → PixelLab → Open plugin** → **"Create S-M image"** tıkla
2. Alanları doldur:
   - **Description**: `White impact spark burst, pixel art VFX, transparent background, single frame`
   - **Camera View** → **"None"** | **Direction** → **"None"**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
3. **Generate**

### ADIM 3 — Animate with text (New)
1. **"Animate with text (New)"** tıkla
2. Alanları doldur:
   - **Reference Image → Set** → `vfx_hit_spark.aseprite`'taki BASE layer'ı export edip referans göster
     VEYA Init Image → az önce üretilen frame'i kullan
   - **Animation Action**: `hit impact spark, white burst expanding then fading to nothing`
   - **Frame Count** → **6** | **Remove Background** → ON
3. **Generate**

### ADIM 4 — Export
**File → Export Sprite Sheet** → Columns: **6** | Trim: ✓ | Extrude: 1 | JSON: ✓
Output: `F:\Antigravity Projeler\2d roguelite\ART\vfx\vfx_hit_spark.png`

---

## SWORD TRAIL VFX

### ADIM 1 — Aseprite
**File → New** → **64×32** | Background: Transparent
**File → Save As** →
`F:\Antigravity Projeler\2d roguelite\ART\vfx\vfx_sword_trail.aseprite`

### ADIM 2 — PixelLab BASE
1. **Edit → PixelLab → Open plugin** → **"Create M-XL image"** tıkla
2. Alanları doldur:
   - **Description**: `Sword slash arc trail, white to gold fading arc, pixel art VFX, transparent background`
   - **Camera View** → **"None"** | **Direction** → **"None"**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
3. **Generate**

### ADIM 3 — Animate with text (New)
1. **"Animate with text (New)"** tıkla
2. **Animation Action**: `sword slash arc trail fading from bright white to gold to invisible`
3. **Frame Count** → **4** | **Remove Background** → ON
4. **Generate**

### ADIM 4 — Export
**File → Export Sprite Sheet** → Columns: **4** | Trim: ✓ | Extrude: 1 | JSON: ✓
Output: `F:\Antigravity Projeler\2d roguelite\ART\vfx\vfx_sword_trail.png`

---

# SKILL İKONLARI

*Skill ikonları için "Create S-M image" kullanılır (32×32 ≤ 80×80). Animasyon yok. Tek frame.*

**Her ikon için standart PixelLab ayarları:**
- Camera View → **"None"** | Direction → **"None"**
- Outline → **"Selective outline"** | Shading → **"Basic shading"** | Details → **"Medium detail"**
- Background Removal → ON | Output Method → **"Add new layer"**

---

## SKILL_IRON_CHARGE İkonu

### ADIM 1 — Aseprite
**File → New** → **32×32** | Background: Transparent → OK
**File → Save As** →
`F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_iron_charge.aseprite`

### ADIM 2 — PixelLab
1. **Edit → PixelLab → Open plugin** → **"Create S-M image"** tıkla
2. Alanları doldur:
   - **Description**: `Pixel art skill icon, armored gauntlet with forward momentum arrow, cold blue impact glow #7BA7BC, dark background #1E1E32, centered, game UI icon`
   - **Negative Description**: `character, landscape, side view`
   - Camera/Direction/Outline/Shading/Details: yukarıdaki standart ayarlar
   - Background Removal → ON | Output Method → **"Add new layer"**
3. **Generate**. Beğendiysen:

### ADIM 3 — Export
**File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_iron_charge.png`

---

## SKILL_CLEAVE İkonu

### ADIM 1 — Aseprite + PixelLab
1. **File → New** → **32×32** | **File → Save As** → `skill_cleave.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"** tıkla
3. **Description**: `Pixel art skill icon, horizontal sword slash arc with shockwave lines, steel grey and cold blue #7BA7BC, dark background, centered, game UI icon`
4. Standart ayarlar. **Generate**. Beğendiysen:
5. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_cleave.png`

---

## SKILL_DEATH_BLOW İkonu

1. **File → New** → **32×32** | **File → Save As** → `skill_death_blow.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
3. **Description**: `Pixel art skill icon, greatsword vertical downstroke, blood red edge glow, dark dramatic background, centered, game UI icon`
4. Standart ayarlar. **Generate**.
5. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_death_blow.png`

---

## SKILL_BLADESTORM İkonu

1. **File → New** → **32×32** | **File → Save As** → `skill_bladestorm.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
3. **Description**: `Pixel art skill icon, sword silhouette in circular spinning motion, gold energy trail #FFD700, dark background, centered, game UI icon`
4. Standart ayarlar. **Generate**.
5. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_bladestorm.png`

---

## SKILL_GRAVITY_CLEAVE İkonu

1. **File → New** → **32×32** | **File → Save As** → `skill_gravity_cleave.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
3. **Description**: `Pixel art skill icon, sword slamming ground with circular pull vortex, cold blue gravitational lines, dark background, centered, game UI icon`
4. Standart ayarlar. **Generate**.
5. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_gravity_cleave.png`

---

## SKILL_WAR_STOMP İkonu

1. **File → New** → **32×32** | **File → Save As** → `skill_war_stomp.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
3. **Description**: `Pixel art skill icon, armored boot slamming down, shockwave rings expanding, gold dust #FFD700, dark background, centered, game UI icon`
4. Standart ayarlar. **Generate**.
5. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\icons\skills\skill_war_stomp.png`

---

# ═══════════════════════════════════════
# FAZ 2 — Demo Hazırlık
# ═══════════════════════════════════════

*FAZ 2 asset'leri FAZ 1 ile aynı adım yapısını kullanır.*
*BASE için: Create M-XL image (64×64 karakterler). Animasyonlar için: BASE'i referans göster.*

---

# ELEMENTALIST

---

## ELEMENTALIST BASE Sprite

### ADIM 1 — Gemini
1. gemini.google.com → Prompt:
```
A dark-robed mage stands with two arcane tomes floating and orbiting around them.
Their left hand emanates cold blue frost crystals, while their right hand crackles
with orange-red fire. They wear an elaborate hood and carry a staff topped with
a split glowing crystal. Viewed from directly above.
The art style is retro pixel art with a limited palette of dark robes, cold blue,
and fire orange. The background must be transparent.
```
2. Açı kontrol. Kaydet → `ART\karakterler\elementalist\elementalist_gemini_base.png`

### ADIM 2 — Aseprite
**File → New** → **64×64** | Background: Transparent
**File → Save As** → `ART\karakterler\elementalist\elementalist_S_BASE.aseprite`

### ADIM 3 — Gemini görselini al
`elementalist_gemini_base.png` → canvas sürükle → layer: **REFERANS**

### ADIM 4 — PixelLab
1. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
2. Alanları doldur:
   - **Description**: `Dark-robed mage with two floating arcane tomes, left hand frost crystals, right hand fire sparks, elaborate hood, split crystal staff, facing downward`
   - **Negative Description**: `side view, face visible, front facing, 3/4 view`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
   - **Init Image → Set** → `elementalist_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
3. **Generate**. REFERANS sil.

### ADIM 5 — Export
**CTRL+S** → **File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\karakterler\elementalist\elementalist_S_BASE.png`

---

## ELEMENTALIST Animasyonları

Her animasyon için:
1. `elementalist_S_BASE.aseprite` aç
2. **Edit → PixelLab → Open plugin** → **"Animate with text (New)"**
3. **Reference Image → Set** → `elementalist_S_BASE.png` → Open
4. Aşağıdaki tablodan **Animation Action** değerini gir
5. **Frame Count** ve **Remove Background: ON** ayarla
6. **Generate** → **File → Export Sprite Sheet** → aşağıdaki tabloya göre Columns ve Output yolu

| Animasyon | Animation Action | Frames | Columns | Export Yolu |
|-----------|-----------------|--------|---------|-------------|
| idle | `standing still, tomes slowly orbiting, elemental energy flickering between hands` | 4 | 4 | `elementalist_S_idle.png` |
| walk | `gliding forward, robes flowing, tomes trailing behind` | 6 | 6 | `elementalist_S_walk.png` |
| attack1 | `casting ice shards forward, frost cone launching from left hand` | 6 | 6 | `elementalist_S_attack1.png` |
| attack2 | `slamming both hands down, fire explosion erupting forward` | 6 | 6 | `elementalist_S_attack2.png` |
| dash | `blinking forward in burst of frost energy` | 4 | 4 | `elementalist_S_dash.png` |
| hurt | `staggering, magical aura flickering` | 4 | 4 | `elementalist_S_hurt.png` |
| death | `collapsing, tomes crashing, energy dispersing` | 8 | 8 | `elementalist_S_death.png` |

Tüm export yolları: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\elementalist\`
Tüm animasyonlarda: **Trim** ✓ | **Extrude** 1 | **JSON** ✓

---

# SHADOWBLADE

---

## SHADOWBLADE BASE Sprite

### ADIM 1 — Gemini
Prompt:
```
A sleek assassin crouches in a tense, ready stance. They wear dark leather armor
and a half-face mask, with dual daggers resting at their hips. Their cloak seems
to absorb light rather than reflect it. Viewed from directly above.
The art style is retro pixel art with a near-black and deep purple color palette,
minimal color, maximum silhouette readability. The background must be transparent.
```
Kaydet → `ART\karakterler\shadowblade\shadowblade_gemini_base.png`

### ADIM 2-5 — Aseprite + PixelLab + Export
1. **File → New** → **64×64** | **File → Save As** → `shadowblade_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. Alanları doldur:
   - **Description**: `Sleek assassin in dark leather armor, dual daggers at hips, half-face mask, cloak absorbing light, crouching aggressive ready stance, facing downward`
   - **Negative Description**: `side view, face visible, front facing, 3/4 view`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
   - **Init Image → Set** → `shadowblade_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
5. **Generate**. REFERANS sil. **CTRL+S**
6. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\shadowblade\shadowblade_S_BASE.png`

---

## SHADOWBLADE Animasyonları

Her animasyon: `shadowblade_S_BASE.aseprite` aç → **Animate > Animate with text (New)** → Reference Image: `shadowblade_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export Yolu |
|-----------|-----------------|--------|-------------|
| idle | `crouching still, cloak barely shifting, daggers glinting` | 4 | `shadowblade_S_idle.png` |
| walk | `gliding forward silently, nearly weightless steps` | 6 | `shadowblade_S_walk.png` |
| attack1 | `quick double dagger slash, blur of motion` | 6 | `shadowblade_S_attack1.png` |
| attack2 | `lunge stab forward, retract` | 6 | `shadowblade_S_attack2.png` |
| dash | `blink dash, disappear frame 1, reappear frame 3` | 4 | `shadowblade_S_dash.png` |
| hurt | `sharp recoil, cloak flaring` | 4 | `shadowblade_S_hurt.png` |
| death | `crumpling silently, form dissolving into shadow particles` | 8 | `shadowblade_S_death.png` |

Tüm export: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\shadowblade\`

---

# RANGER

---

## RANGER BASE Sprite

### ADIM 1 — Gemini
Prompt:
```
A lean archer stands in an alert, ready stance with a longbow half-drawn.
They wear worn leather armor decorated with bone and earth accents, a quiver
on their back, and their hood is pulled back. Viewed from directly above.
The art style is retro pixel art with earthy brown, bone white, and leather
tan tones. The background must be transparent.
```
Kaydet → `ART\karakterler\ranger\ranger_gemini_base.png`

### ADIM 2-5 — Aseprite + PixelLab + Export
1. **File → New** → **64×64** | **File → Save As** → `ranger_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. Alanları doldur:
   - **Description**: `Lean archer in worn leather armor, longbow drawn halfway, quiver on back, hood pulled back, bone and earth accents, alert stance, facing downward`
   - **Negative Description**: `side view, face visible, front facing, 3/4 view`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
   - **Init Image → Set** → `ranger_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
5. **Generate**. REFERANS sil. **CTRL+S**
6. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\ranger\ranger_S_BASE.png`

---

## RANGER Animasyonları

Her animasyon: `ranger_S_BASE.aseprite` aç → **Animate > Animate with text (New)** → Reference Image: `ranger_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export Yolu |
|-----------|-----------------|--------|-------------|
| idle | `standing alert, bow at half-draw, scanning the area` | 4 | `ranger_S_idle.png` |
| walk | `moving carefully, bow ready, light careful footsteps` | 6 | `ranger_S_walk.png` |
| attack1 | `drawing and releasing arrow, snap bow release` | 6 | `ranger_S_attack1.png` |
| attack2 | `rapid triple shot, three quick releases in succession` | 8 | `ranger_S_attack2.png` |
| dash | `rolling sideways, quick dodge roll` | 4 | `ranger_S_dash.png` |
| hurt | `stumbling back, quiver rattling` | 4 | `ranger_S_hurt.png` |
| death | `falling backward, bow dropping from grip` | 8 | `ranger_S_death.png` |

Tüm export: `F:\Antigravity Projeler\2d roguelite\ART\karakterler\ranger\`

---

# SEAM CRAWLER — Faz 2 Düşman

---

## SEAM CRAWLER BASE Sprite

### ADIM 1 — Gemini
Prompt:
```
Pixel art sprite, 32x32 pixels, top-down 2D dark fantasy game,
transparent background, horror creature inside a floor crack —
only two long dark claws and spine ridge visible above the crack,
pressed flat, lurking, palette: #080808 void purple glow #9E4FE0, bone claws
```
Kaydet → `ART\duşmanlar\grunt_seam\grunt_seam_gemini_base.png`

### ADIM 2-5
1. **File → New** → **32×32** | **File → Save As** → `grunt_seam_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
4. **Description**: `Horror creature inside floor crack, only long dark claws and spine ridge above the surface, lurking enemy, void purple glow from crack, facing downward`
   **Negative**: `side view, full body, standing`
   Camera: **"Low top-down"** | Direction: **"South"** | Outline/Shading/Details: standart
   Init Image → `grunt_seam_gemini_base.png` | Strength: **500** | BG Removal: ON | Output: **"Add new layer"**
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_seam\grunt_seam_S_BASE.png`

---

## SEAM CRAWLER Animasyonları

`grunt_seam_S_BASE.aseprite` → **Animate > Animate with text (New)** → Reference: `grunt_seam_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export |
|-----------|-----------------|--------|--------|
| idle | `claws slowly scraping the stone surface` | 4 | `grunt_seam_S_idle.png` |
| attack | `claws shooting upward from crack, striking, retracting` | 6 | `grunt_seam_S_attack.png` |
| death | `claws retracting, crack sealing shut` | 6 | `grunt_seam_S_death.png` |

Export: `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_seam\`

---

# ECHO HOUND — Faz 2 Düşman

---

## ECHO HOUND BASE Sprite

### ADIM 1 — Gemini
Prompt:
```
Pixel art sprite, 32x32 pixels, top-down 2D dark fantasy game,
transparent background, ghostly wolf predator, semi-transparent indigo form
#3D1F6B #9E4FE0, white glowing eyes #D4D4F0, no solid flesh — just energy
lines and silhouette, predatory low crouching stance, facing south
```
Kaydet → `ART\duşmanlar\grunt_echo\grunt_echo_gemini_base.png`

### ADIM 2-5
1. **File → New** → **32×32** | **File → Save As** → `grunt_echo_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
4. **Description**: `Ghostly wolf creature, semi-transparent indigo energy form, white glowing eyes, no solid body just energy lines, predatory crouching stance, facing downward`
   **Negative**: `side view, solid body, front facing`
   Camera: **"Low top-down"** | Direction: **"South"** | standart ayarlar
   Init Image → `grunt_echo_gemini_base.png` | Strength: **500** | BG: ON | Output: Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_echo\grunt_echo_S_BASE.png`

---

## ECHO HOUND Animasyonları

`grunt_echo_S_BASE.aseprite` → **Animate > Animate with text (New)** → Reference: `grunt_echo_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export |
|-----------|-----------------|--------|--------|
| idle | `form flickering, hovering in place, eyes glowing` | 4 | `grunt_echo_S_idle.png` |
| walk | `bounding forward with strong motion trail` | 6 | `grunt_echo_S_walk.png` |
| attack | `blinking forward snapping, blinking back` | 6 | `grunt_echo_S_attack.png` |
| death | `form dispersing into indigo particles` | 6 | `grunt_echo_S_death.png` |

Export: `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_echo\`

---

# HOLLOW MITE — Faz 2 Düşman
⚠️ Sprite canvas'ın %30'unu doldursun — bilinçli küçük.

---

## HOLLOW MITE BASE

### ADIM 1 — Gemini
Prompt:
```
Pixel art sprite, 32x32 pixels, top-down 2D dark fantasy game,
transparent background, SMALL insect creature occupying only 30% of canvas,
hollow transparent exoskeleton, six legs, tiny glowing core inside,
facing south, palette: #1E1E32 exoskeleton, dim #9E4FE0 inner glow
```
Kaydet → `grunt_mite_gemini_base.png`

### ADIM 2-5
1. **File → New** → **32×32** | **File → Save As** → `grunt_mite_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS** layer
3. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
4. **Description**: `Tiny hollow insect creature, transparent exoskeleton, six legs, glowing core inside, small swarm enemy, facing downward, occupying only 30% of canvas area`
   **Negative**: `large, full body, side view`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `grunt_mite_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_mite\grunt_mite_S_BASE.png`

---

## HOLLOW MITE Animasyonları

`grunt_mite_S_BASE.aseprite` → **Animate > Animate with text (New)** → Reference: `grunt_mite_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export |
|-----------|-----------------|--------|--------|
| idle | `tiny insect hovering, legs twitching, core faintly pulsing` | 4 | `grunt_mite_S_idle.png` |
| walk | `scuttling forward quickly, six legs moving` | 6 | `grunt_mite_S_walk.png` |
| death | `exoskeleton cracking open, glowing core fading` | 6 | `grunt_mite_S_death.png` |

Export: `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\grunt_mite\`

---

# THE TWICE-BORN — Elite Act 1
*2 ayrı BASE sprite. Animasyon yok.*

---

## TWICE-BORN SPRITE A (Saldırgan)

### ADIM 1 — Gemini
Prompt:
```
Pixel art sprite, 64x64 pixels, top-down 2D dark fantasy game, transparent background,
elite warrior with two sword arms — one armored physical arm, one spectral blue #7BA7BC arm,
cracked iron armor with cold blue seeping through, one eye glowing #7BA7BC,
aggressive battle-ready stance, facing south
```
Kaydet → `elite_twice_born_gemini_A.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `elite_twice_born_A_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Elite warrior with two sword arms — one armored, one spectral blue energy arm, cracked armor with cold blue glow, one glowing eye, aggressive battle stance, facing downward`
   **Negative**: `side view, face visible, 3/4 view`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `elite_twice_born_gemini_A.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\elite_twice_born\elite_twice_born_A_S_BASE.png`

---

## TWICE-BORN SPRITE B (Savunmacı)

### ADIM 1 — Gemini
Prompt:
```
Pixel art sprite, 64x64 pixels, top-down 2D dark fantasy game, transparent background,
elite warrior companion with large shield raised defensively, matching cracked iron armor
with cold blue glow #7BA7BC, protective guardian stance, pair character to Sprite A, facing south
```
Kaydet → `elite_twice_born_gemini_B.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `elite_twice_born_B_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Elite guardian warrior with large shield raised defensively, cracked iron armor with cold blue cracks, protective stance, pair to attacker companion, facing downward`
   Init Image → `elite_twice_born_gemini_B.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\elite_twice_born\elite_twice_born_B_S_BASE.png`

---

# FRACTURE-BORN — Elite (4 Faz)
*4 ayrı canvas, her faz ayrı bir BASE. Animasyon yok.*

Her faz için aynı adımlar: Gemini → 64×64 Aseprite → PixelLab Create M-XL image → Export

| Faz | Aseprite Kayıt | Export Yolu | Description (kısaltılmış) |
|-----|---------------|-------------|--------------------------|
| Faz 1 (sağlam) | `elite_fracture_faz1.aseprite` | `elite_fracture_faz1_S_BASE.png` | `Heavily armored stone golem, no cracks, imposing full armor, facing downward` |
| Faz 2 (hafif hasar) | `elite_fracture_faz2.aseprite` | `elite_fracture_faz2_S_BASE.png` | `Stone golem armor beginning to crack, small cold blue light seeping through first cracks` |
| Faz 3 (ağır hasar) | `elite_fracture_faz3.aseprite` | `elite_fracture_faz3_S_BASE.png` | `Stone golem armor heavily cracked open, bright cold blue energy erupting through major fractures` |
| Faz 4 (kırılmak üzere) | `elite_fracture_faz4.aseprite` | `elite_fracture_faz4_S_BASE.png` | `Stone golem armor barely holding, intense cold blue energy overflowing from massive cracks` |

Tüm export: `F:\Antigravity Projeler\2d roguelite\ART\duşmanlar\elite_fracture\`

Her faz için PixelLab:
- **Camera View** → **"Low top-down"** | **Direction** → **"South"**
- **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"Medium detail"**
- **Background Removal** → ON | **Output Method** → **"Add new layer"**

---

# FRACTURED KING — Act 2 Boss
⚠️ **128×128 — "Create M-XL image" kullan!**

---

## FRACTURED KING BASE

### ADIM 1 — Gemini
Prompt:
```
A powerful dark king figure viewed from directly above, top-down aerial perspective.
The king wears fragmented plate armor that has split into four distinct sections,
each piece floating slightly away from the others as if held together by void energy.
Each armor section glows with a different energy: cold blue, void purple, fire orange, bone white.
A cracked crown floats above where the head should be. No face visible — only the
fractured armor pieces and the void between them. Retro pixel art style, transparent background.
```
Açı kontrol. Kaydet → `ART\boss\fractured_king\boss_fractured_king_gemini_base.png`

### ADIM 2 — Aseprite
**File → New** → **128×128** | Background: Transparent
**File → Save As** → `ART\boss\fractured_king\boss_fractured_king_S_BASE.aseprite`

### ADIM 3 — Gemini görselini al
`boss_fractured_king_gemini_base.png` → canvas sürükle → **REFERANS** layer

### ADIM 4 — PixelLab
1. **Edit → PixelLab → Open plugin**
2. ⚠️ **"Create M-XL image"** tıkla
3. Alanları doldur:
   - **Description**: `Powerful dark king figure, fragmented plate armor split into four floating sections each with different energy color (cold blue, void purple, fire orange, bone white), cracked floating crown, no face, void energy between pieces, imposing top-down boss view`
   - **Negative Description**: `side view, face visible, front facing, unified armor`
   - **Camera View** → **"Low top-down"** | **Direction** → **"South"**
   - **Outline** → **"Selective outline"** | **Shading** → **"Basic shading"** | **Details** → **"High detail"**
   - **Init Image → Set** → `boss_fractured_king_gemini_base.png` → Open | **Strength** → **500**
   - **Background Removal** → ON | **Output Method** → **"Add new layer"**
4. **Generate**. REFERANS sil.

### ADIM 5 — Export
**CTRL+S** → **File → Export As** →
`F:\Antigravity Projeler\2d roguelite\ART\boss\fractured_king\boss_fractured_king_S_BASE.png`

---

## FRACTURED KING Animasyonları

Her animasyon: `boss_fractured_king_S_BASE.aseprite` aç → **Animate > Animate with text (New)** → Reference: `boss_fractured_king_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export Yolu |
|-----------|-----------------|--------|-------------|
| idle | `armor pieces floating and slowly rotating independently, crown drifting, void energy pulsing between fragments` | 6 | `boss_fractured_king_S_idle.png` |
| attack_melee | `armor fragments slamming together into a fist form and striking forward, then separating again` | 8 | `boss_fractured_king_S_attack_melee.png` |
| attack_ranged | `one armor fragment launching as a projectile then snapping back into formation` | 8 | `boss_fractured_king_S_attack_ranged.png` |
| phase2_trigger | `all armor pieces suddenly exploding outward then slamming back together in new configuration, energy color shifts` | 8 | `boss_fractured_king_S_phase2.png` |
| hurt | `armor fragments briefly scattering then reforming, minor disruption` | 4 | `boss_fractured_king_S_hurt.png` |
| death | `all armor pieces slowly drifting apart, void energy fading, crown cracking and dissolving` | 8 | `boss_fractured_king_S_death.png` |

Tüm export: `F:\Antigravity Projeler\2d roguelite\ART\boss\fractured_king\`

---

# ═══════════════════════════════════════
# FAZ 3 — Full Acts
# ═══════════════════════════════════════

*FAZ 3 asset'leri aynı adım yapısını kullanır.*

---

# HOLLOW SOVEREIGN — Act 3 Boss
⚠️ **128×128 — "Create M-XL image" kullan!**

---

## HOLLOW SOVEREIGN BASE

### ADIM 1 — Gemini
Prompt:
```
An ancient sovereign entity viewed from directly above. The figure is impossibly
tall and thin, with elongated limbs that bend at wrong angles. Their robes seem
to be made of compressed void space — swirling dark matter with faint echoes of
faces and silhouettes trapped within. Where hands should be are elongated tendrils
of compressed energy. A pale mask-like face is barely visible at the top.
Retro pixel art style, transparent background, cold blue and void purple palette.
```
Kaydet → `boss_hollow_sovereign_gemini_base.png`

### ADIM 2-5
1. **File → New** → **128×128** | **File → Save As** → `boss_hollow_sovereign_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → ⚠️ **"Create M-XL image"**
4. **Description**: `Ancient elongated sovereign entity, impossibly thin figure with wrong-angle limbs, void-matter robes with trapped faces within, energy tendril hands, pale mask face, overwhelming presence, top-down boss`
   **Negative**: `side view, front facing, human proportions`
   Camera: **"Low top-down"** | Direction: **"South"** | Details: **"High detail"**
   Init Image → `boss_hollow_sovereign_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\boss\hollow_sovereign\boss_hollow_sovereign_S_BASE.png`

---

## HOLLOW SOVEREIGN Animasyonları

`boss_hollow_sovereign_S_BASE.aseprite` → **Animate > Animate with text (New)** → Reference: `boss_hollow_sovereign_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export |
|-----------|-----------------|--------|--------|
| idle | `impossibly still, robes slowly churning, trapped faces shifting within void matter, barely breathing` | 6 | `boss_hollow_sovereign_S_idle.png` |
| attack_tendril | `one elongated tendril arm lashing forward, energy whip strike, retracting` | 8 | `boss_hollow_sovereign_S_attack_tendril.png` |
| attack_void | `both arms raising, compressed void ball forming between them, launching forward` | 8 | `boss_hollow_sovereign_S_attack_void.png` |
| counter_phase | `robes suddenly expanding outward, trapped faces screaming, full void eruption then contracting` | 8 | `boss_hollow_sovereign_S_counter.png` |
| hurt | `mask briefly cracking, form flickering, brief moment of fragility` | 4 | `boss_hollow_sovereign_S_hurt.png` |
| death | `form slowly unraveling, all trapped faces escaping upward, void collapsing inward` | 8 | `boss_hollow_sovereign_S_death.png` |

Export: `F:\Antigravity Projeler\2d roguelite\ART\boss\hollow_sovereign\`

---

# NEXUS CORE — Final Boss
⚠️ **128×128 — "Create M-XL image" kullan!**

---

## NEXUS CORE BASE

### ADIM 1 — Gemini
Prompt:
```
An abstract geometric entity that is the source of all fractures. Viewed from directly above.
At its center is a perfect dark void sphere. Around it, four crystalline arms extend outward
at 45-degree angles, each arm a different length, each pulsing with a different class energy
color (cold blue, void purple, fire orange, forest green). The entire structure rotates slowly.
Dark geometry, mathematical precision corrupted by cracks. Retro pixel art, transparent background.
```
Kaydet → `boss_nexus_core_gemini_base.png`

### ADIM 2-5
1. **File → New** → **128×128** | **File → Save As** → `boss_nexus_core_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → ⚠️ **"Create M-XL image"**
4. **Description**: `Abstract geometric dark entity, central void sphere with four crystalline arms extending at 45-degree angles, each arm different energy color (cold blue, void purple, fire orange, forest green), slowly rotating structure, mathematical precision, top-down boss`
   **Negative**: `humanoid, side view, organic`
   Camera: **"Low top-down"** | Direction: **"None"** | Details: **"High detail"**
   Init Image → `boss_nexus_core_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\boss\nexus_core\boss_nexus_core_S_BASE.png`

---

## NEXUS CORE Animasyonları

`boss_nexus_core_S_BASE.aseprite` → **Animate > Animate with text (New)** → Reference: `boss_nexus_core_S_BASE.png`
**Frame Count** ve **Remove Background: ON** ayarla. Trim ✓ | Extrude 1 | JSON ✓

| Animasyon | Animation Action | Frames | Export |
|-----------|-----------------|--------|--------|
| idle | `geometric structure slowly rotating, crystalline arms pulsing with alternating energy colors, void center swirling` | 8 | `boss_nexus_core_S_idle.png` |
| attack_beam | `one crystalline arm aligning toward target, intense energy beam launching, arm recoiling` | 8 | `boss_nexus_core_S_attack_beam.png` |
| attack_burst | `all four arms simultaneously releasing energy bursts in four diagonal directions` | 8 | `boss_nexus_core_S_attack_burst.png` |
| phase2 | `central void sphere cracking, inner light exploding outward, structure rebuilding with darker more corrupted form` | 8 | `boss_nexus_core_S_phase2.png` |
| hurt | `structure briefly destabilizing, arms misaligning, geometry corrupting momentarily` | 4 | `boss_nexus_core_S_hurt.png` |
| death | `all crystalline arms shattering simultaneously, void sphere imploding, absolute stillness` | 8 | `boss_nexus_core_S_death.png` |

Export: `F:\Antigravity Projeler\2d roguelite\ART\boss\nexus_core\`

---

# ACT 2 TİLE SETİ

*Aynı adımlar. Renk paleti değişiyor: void purple (#9E4FE0), corrupted stone.*

## FLOOR TILE — 16×16

1. Aseprite → **File → New** → **16×16** | **File → Save As** → `act2_floor_tileset.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create tileset"**
3. Alanları doldur:
   - **Description (Inner)**: `Corrupted stone dungeon floor, void purple energy cracks #9E4FE0, dark organic texture, seamless tileable, top-down dark fantasy`
   - **Type** → **"Top-Down"** | **Tile Size** → **16** | **Remove Background** → ON
4. **Generate** → Export As → `act2_floor_01.png`
5. **Generate** → Export As → `act2_floor_02.png`
6. **Generate** → Export As → `act2_floor_03.png`

Export: `F:\Antigravity Projeler\2d roguelite\ART\tiles\act2\`

## WALL TILE — 16×32

1. **File → New** → **16×32** | **File → Save As** → `act2_wall_tileset.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create tileset"**
3. **Description (Inner)**: `Corrupted fortress wall, void purple corruption seeping through stone cracks, dark organic texture, seamless tileable`
   **Type** → **"Top-Down"** | **Tile Size** → **16** | **Remove Background** → ON
4. **Generate** → `act2_wall_01.png` | **Generate** → `act2_wall_02.png`

---

# ACT 3 TİLE SETİ

*Renk: void/nebula — karanlık, neredeyse soyut.*

## FLOOR TILE — 16×16

1. **File → New** → **16×16** | **File → Save As** → `act3_floor_tileset.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create tileset"**
3. **Description (Inner)**: `Abstract void floor tiles, deep space dark matter texture, barely visible stone beneath, cold blue and void purple nebula wisps, seamless tileable, top-down`
   **Type** → **"Top-Down"** | **Tile Size** → **16** | **Remove Background** → ON
4. **Generate** → `act3_floor_01.png` | **Generate** → `act3_floor_02.png` | **Generate** → `act3_floor_03.png`

Export: `F:\Antigravity Projeler\2d roguelite\ART\tiles\act3\`

---

# ═══════════════════════════════════════
# FAZ 4 — Polish + UI
# ═══════════════════════════════════════

---

# HUB KARAKTERLERİ

*64×64, Create M-XL image, animasyon yok (statik NPC). Her biri için aynı adımlar.*

---

## THE FERRYMAN (Hub NPC)

### ADIM 1 — Gemini
Prompt:
```
An ancient ferryman figure viewed from directly above. Impossibly thin, wrapped in
tattered dark robes that seem too long. Holds a long pole. Has no visible face —
only darkness beneath a wide-brimmed hat. Candle warm ambient light around them.
Retro pixel art, dark tones with warm candle orange accents #D4956A, transparent background.
```
Kaydet → `hub_ferryman_gemini_base.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `hub_ferryman_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Ancient ferryman figure, thin tattered robes, long pole, no visible face under wide-brimmed hat, candle warm ambient light, hub NPC, facing downward`
   **Negative**: `side view, face visible`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `hub_ferryman_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\hub\hub_ferryman_S_BASE.png`

---

## VREL — Hayalet Demirci

### ADIM 1 — Gemini
Prompt:
```
A ghostly blacksmith figure viewed from directly above. Semi-transparent form,
wearing a heavy blacksmith apron, holding a spectral hammer. Their form flickers
in and out of visibility. Faint warm forge light emanates from where their chest would be.
Retro pixel art, ghost white #D4D4F0 and warm forge #D4956A palette, transparent background.
```
Kaydet → `hub_vrel_gemini_base.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `hub_vrel_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Ghostly blacksmith, semi-transparent flickering form, heavy apron, spectral hammer, warm forge light from chest, ghost NPC, facing downward`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `hub_vrel_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\hub\hub_vrel_S_BASE.png`

---

## SISTER MOURNE — Şifacı Rahibe

### ADIM 1 — Gemini
Prompt:
```
A mournful healer nun figure viewed from directly above. Wearing heavy dark robes
with a white hood. Hands clasped together, holding a dim candle. An aura of gentle
healing light surrounds her, but her posture is bent with grief.
Retro pixel art, dark robes with ghost white hood #D4D4F0, candle warm #D4956A, transparent background.
```
Kaydet → `hub_sister_mourne_gemini_base.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `hub_sister_mourne_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Mournful healer nun, heavy dark robes, white hood, clasped hands holding candle, healing aura, grief-bent posture, hub NPC, facing downward`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `hub_sister_mourne_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\hub\hub_sister_mourne_S_BASE.png`

---

## THE CARTOGRAPHER — Harita Satıcısı

### ADIM 1 — Gemini
Prompt:
```
A hunched cartographer figure viewed from directly above. Surrounded by floating
map fragments and quill pens. Wearing ink-stained scholar robes. One hand perpetually
reaching out as if pointing to something on a map.
Retro pixel art, dark scholar robes #1E1E32, parchment warm #D4956A, transparent background.
```
Kaydet → `hub_cartographer_gemini_base.png`

### ADIM 2-5
1. **File → New** → **64×64** | **File → Save As** → `hub_cartographer_S_BASE.aseprite`
2. Gemini görselini sürükle → **REFERANS**
3. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
4. **Description**: `Hunched cartographer scholar, floating map fragments around them, quill pens, ink-stained robes, one hand pointing at map, hub NPC, facing downward`
   Camera: **"Low top-down"** | Direction: **"South"** | standart
   Init Image → `hub_cartographer_gemini_base.png` | Strength: **500** | BG: ON | Add new layer
5. **Generate**. REFERANS sil. **Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\karakterler\hub\hub_cartographer_S_BASE.png`

---

# UI ELEMENTLERI

*Tüm UI için: ≤80×80 ise Create S-M image, >80×80 ise Create M-XL image. Camera View → None, Direction → None*

---

## SKILL SLOT FRAME

1. **File → New** → **48×48** | **File → Save As** → `ui_skill_slot.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create S-M image"**
3. **Description**: `Dark fantasy game UI skill slot frame, 48x48, dark iron border #1E1E32, inner void black area, thin gold corner accent #FFD700, game HUD element`
   **Camera View** → **"None"** | **Direction** → **"None"** | BG Removal: ON | Add new layer
4. **Generate**. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\ui_skill_slot.png`

---

## HP BAR FRAME

1. **File → New** → **200×20** | **File → Save As** → `ui_hp_bar.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
3. **Description**: `Dark fantasy game UI health bar frame, 200x20 horizontal, dark iron border, inner area for fill, left side skull or heart icon, dark RPG aesthetic`
   **Camera View** → **"None"** | **Direction** → **"None"** | BG: ON | Add new layer
4. **Generate**. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\ui_hp_bar_frame.png`

---

## RAGE BAR FRAME

1. **File → New** → **200×20** | **File → Save As** → `ui_rage_bar.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
3. **Description**: `Dark fantasy game UI rage/energy bar frame, 200x20 horizontal, dark iron border with orange-red accent, flame icon on left side, dark RPG aesthetic`
   **Camera View** → **"None"** | **Direction** → **"None"** | BG: ON | Add new layer
4. **Generate**. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\ui_rage_bar_frame.png`

---

## SKILL DRAFT CARD

1. **File → New** → **80×120** | **File → Save As** → `ui_skill_card.aseprite`
2. **Edit → PixelLab → Open plugin** → **"Create M-XL image"**
3. **Description**: `Dark fantasy game UI skill card, 80x120 portrait, dark iron border with slight gold glow, inner area for icon and text, top section for icon, bottom section for name/description, dark RPG card aesthetic`
   **Camera View** → **"None"** | **Direction** → **"None"** | BG: ON | Add new layer
4. **Generate**. **File → Export As** →
   `F:\Antigravity Projeler\2d roguelite\ART\ui\ui_skill_card.png`

---

*Son güncelleme: 2026-03-31 — Tüm FAZ 0-4 asset'leri için Gemini → Aseprite adımı → PixelLab hangi buton/alan → Export hangi yol tam inline adım adım yeniden yazıldı. —claude*
