# Görsel Yönerge ve Lighting Rehberi
*Son güncelleme: 2026-03-29 | Unity 6.3 LTS + URP 2D*

---

## HEDEF GÖRSEL KİMLİK

> "Pixel art tabanı ama post-processing ile zenginleştirilmiş —
> bütünüyle pixelated değil, karanlık ve atmosferik."

**Referanslar:**
- **Dead Cells** → pixel art + dinamik ışıklandırma + zengin partiküller
- **Hades** → güçlü glow/bloom, sıcak vurgu ışıkları, karanlık ambient
- **Hollow Knight** → karanlık ama derinlikli atmosfer
- **Enter the Gungeon** → top-down okunabilirlik + ışık kontrast

**Kaçınılacaklar:**
- Nuclear Throne tipi çok ham piksel → "amatör" hissi verir
- Aşırı bloom → her şey yıkanır, okunabilirlik düşer
- Düz beyaz ambient ışık → tüm atmosfer gider

---

## SPRITE BOYUTU VE KALİTESİ

### Karar: 48×48 Base Sprite

| Boyut | Görünüm | Neden |
|-------|---------|-------|
| 16×16 | Çok ham, detay yok | Erken NES dönemi, günümüz için kötü |
| 32×32 | Klasik pixel art | İyi ama sınırlı detay, "amatör" riski |
| **48×48** | **Önerilen** | Güçlü silüet + yeterli detay + animate edilebilir |
| 64×64 | Çok detaylı | Yavaş üretim, solo dev için ağır |

**Pixel/Unit ayarı:** 48 PPU (Pixels Per Unit)
**Filter Mode:** Point (no filter) — her pixel net
**Compression:** None veya lossless

### Tile Boyutu
- Floor/Wall tile: **16×16** (tiled efficiently)
- Environment props: **32×32** veya **48×48**

---

## UNITY URP 2D LIGHTING KURULUMU

### Paket Gereksinimleri
```
Universal RP (URP) — Package Manager'dan
2D Renderer etkin olmalı (URP Asset → Renderer Type: 2D)
```

### URP Asset Ayarları
```
Renderer: 2D Renderer
HDR: Enabled                    ← Bloom için şart
Post Processing: Enabled
MSAA: 4x (2D için yeterli)
```

### 2D Renderer Data
```
Shadow Type: 2D Shadow Caster
Enable HDR Emulation Scale: Yes
```

---

## LIGHTING KATMANLARI

Her karakter/çevre farklı lighting layer'a girer:

```
Layer 0 — Background     (zemin tile'ları, en düşük ışık)
Layer 1 — Foreground     (duvarlar, objeler)
Layer 2 — Characters     (oyuncu, düşmanlar)
Layer 3 — Effects        (skill efektleri, partiküller — en yüksek bloom)
Layer 4 — UI             (HUD — lighting'den etkilenmez)
```

### Global Light 2D (Her Act İçin)

**Act 1 — Shattered Ruins:**
```
Color:     #2A3A4A  (soğuk mavi-gri)
Intensity: 0.25     (karanlık ama okunabilir)
```

**Act 2 — Bleeding Wastes:**
```
Color:     #1E0F26  (derin mor)
Intensity: 0.18
```

**Act 3 — Core Approach:**
```
Color:     #060610  (neredeyse siyah)
Intensity: 0.08     (çok karanlık, kaynak ışıklar öne çıkar)
```

**The Threshold (Hub):**
```
Color:     #2A1A10  (sıcak mum kahvesi)
Intensity: 0.35     (hub rahat olmalı)
```

---

## KAYNAK IŞIKLAR (Point Light 2D)

### Oyuncu Işığı
```
Type:      Point Light 2D
Color:     #FFE8CC  (ılık beyaz)
Intensity: 0.6
Outer Radius: 4
Inner Radius: 1.5
Falloff:   Gaussian
```
Oyuncu her zaman çevresini hafif aydınlatır. "Karanlıkta bir umut" hissi.

### Meşale / Ateş Işığı
```
Type:      Point Light 2D
Color:     #FF6020  (turuncu ateş)
Intensity: 1.2 → flicker animasyonu (0.9 - 1.4 arası)
Outer Radius: 3
Flicker: AnimationCurve ile Intensity animasyonu (6-8 frame loop)
```

### Skill Efekt Işıkları (geçici, runtime spawn)
```
Fireball:       #FF4400, Intensity 2.5, Radius 2.5
Ice Spike:      #80D0FF, Intensity 2.0, Radius 2.0
Shadowblade:    #6600CC, Intensity 1.5, Radius 1.5
Boss warn aura: #FF0000, Intensity 3.0, Radius 5.0 (tehlike sinyali)
```

Runtime spawn:
```csharp
// Skill çalıştığında geçici ışık spawn et
IEnumerator SpawnSkillLight(Color color, float intensity, float duration)
{
    var light = Instantiate(skillLightPrefab);
    light.color = color;
    light.intensity = intensity;
    yield return new WaitForSeconds(duration);
    // DOTween ile fade out
    DOTween.To(() => light.intensity, x => light.intensity = x, 0f, 0.3f)
           .OnComplete(() => Destroy(light.gameObject));
}
```

---

## NORMAL MAPS (Derinlik için)

Normal map eklemek görüntüyü dramatik ölçüde iyileştirir, flat tile'lara 3D his verir.

### Sprite 2D ile Normal Map
```
1. Aseprite'de tile/sprite çiz
2. Sprite'ı Photoshop / GIMP'e al
3. Filter → 3D → Normal Map veya NormalMap Online (normalmap.online) kullan
4. Unity'de Sprite'ın Inspector'ında:
   Texture Type: Normal Map
5. Sprite Renderer → Material: Lit Sprite Material (URP 2D Lit)
```

### Hangi objelere normal map eklenmeli?
| Obje | Öncelik |
|------|---------|
| Duvar tile'ları | ⭐⭐⭐ Yüksek — en çok fark yaratır |
| Zemin tile'ları | ⭐⭐ Orta |
| Karakter sprite'ları | ⭐⭐⭐ Yüksek — canlı hissettiren |
| Prop'lar (kasa, sütun) | ⭐⭐ Orta |
| UI elementleri | ❌ Gerek yok |

---

## POST-PROCESSING STACK

Volume component ile sahneye eklenir:

### Global Volume (her sahnede)
```csharp
// Post-process profile per act, ScriptableObject'te saklı
public PostProcessingProfile act1Profile;
public PostProcessingProfile act2Profile;
```

### Bloom
```
Threshold:    0.95   (sadece çok parlak pixeller — overexpose olmaz)
Intensity:    0.4
Scatter:      0.7
Tint:         Nötr beyaz
Quality:      High

⚠️ Bloom threshold düşükse her şey yıkanır. 0.9+ tut.
```

### Vignette
```
Intensity:  0.35
Smoothness: 0.4
Color:      Siyah
Rounded:    Yes
```
Köşeleri karartan vignette oyuncunun gözünü merkeze çeker.

### Color Adjustments (per act)
```
Act 1:
  Contrast:       +15
  Saturation:     -20   (daha soluk)
  Color Filter:   #E0EEF0 (soğuk)

Act 2:
  Contrast:       +25
  Saturation:     -10
  Color Filter:   #F0D0FF (hafif mor)

Act 3:
  Contrast:       +40
  Saturation:     -30   (neredeyse monokrom)
  Color Filter:   #F8F0C0 (altın)
```

### Film Grain
```
Type:       Grain
Intensity:  0.15   (hafif, fark edilmez ama gritty his katar)
Response:   0.8
```

### Chromatic Aberration (sadece hit reaction ve boss moments)
```
Normal:     0
Hit react:  0.4 (0.2s lerp ile geri döner)
Boss faz:   0.6 (faz geçiş anında)
```

---

## PARTİKÜL SİSTEMİ

Partiküller pixel art olmak zorunda değil — aksine pixel-dışı partiküller kontrast yaratır ve görüntüyü zenginleştirir.

### Temel Partiküller

**Kan/Hasar İzi:**
```
- Shape: Sphere, 0.1 radius
- Start Speed: 2-5
- Lifetime: 0.3-0.6s
- Size: 0.05-0.15
- Color: #AA0000 → #330000 (fade)
- Gravity: 1.5 (düşer)
- Count: 8-15 burst
```

**Skill Hit Spark:**
```
- Shape: Circle, emit from edge
- Start Speed: 4-8
- Lifetime: 0.1-0.2s
- Size: 0.08-0.2
- Color: Skill rengi → fade
- Count: 6-10 burst
```

**Ateş Efekti (Elementalist):**
```
- Shape: Cone, 10° angle
- Start Speed: 1-3
- Lifetime: 0.4-0.8s
- Size over time: 0.3 → 0.0
- Color: #FF6600 → #FF2200 → #00000000
- Emission: 30/sn continuous
```

**Ambient Toz/Kül (ortam):**
```
- Shape: Box, arena boyutunda
- Start Speed: 0.1-0.3 (çok yavaş)
- Lifetime: 4-8s
- Size: 0.02-0.05
- Color: #808080 (gri, düşük alpha)
- Emission: 5/sn continuous
- Bu partiküller sahneye "eski, terk edilmiş" hissi verir
```

---

## SHADER EFEKTLERİ

### Sprite Outline (Düşman seçili/uyarı)
```
URP Sprite Outline → asset store veya custom shader
Renk: Düşmana göre (kırmızı tehlikeli, sarı normal, mor elite)
Thickness: 1 pixel
```

### Hit Flash (Hasar aldığında)
```csharp
// Material instance + _FlashAmount property
IEnumerator HitFlash()
{
    mat.SetFloat("_FlashAmount", 1f);
    yield return new WaitForSeconds(0.05f);
    DOTween.To(() => mat.GetFloat("_FlashAmount"),
               x => mat.SetFloat("_FlashAmount", x), 0f, 0.1f);
}
```

### Dissolve (Ölüm animasyonu)
```
Shader: URP Dissolve (noise texture tabanlı)
Karakter ölünce: dissolve _Cutoff 0 → 1 (0.6s)
Renk: Class'a özgü (Warblade: kırmızı, Elementalist: mavi ateş)
```

### Status Effect Renk Tint
```csharp
// Buff/debuff görselleştirilmesi
void ApplyStatusTint(StatusEffect effect)
{
    Color tint = effect switch
    {
        StatusEffect.Slowed    => new Color(0.6f, 0.8f, 1f),   // buz mavisi
        StatusEffect.Burning   => new Color(1f, 0.5f, 0.2f),   // ateş turuncusu
        StatusEffect.Poisoned  => new Color(0.4f, 1f, 0.3f),   // zehir yeşili
        StatusEffect.Cursed    => new Color(0.7f, 0.2f, 0.9f), // lanet moru
        _ => Color.white
    };
    spriteRenderer.color = tint;
}
```

---

## KAMERA AYARLARI

```
Camera Type:    Orthographic
Size:           5 (720p) / 6 (1080p)
Background:     Solid Color, #000000
Anti-aliasing:  Off  ← pixel art için. MSAA kapatılmalı
Post-process:   Volume Layer'dan

// Pixel-perfect için:
Pixel Perfect Camera component:
  Assets PPU: 48
  Reference Resolution: 1920x1080
  Crop Frame: None
  Grid Snapping: Upscale Render Texture: Off (blur yapar)
```

---

## SCREEN SHAKE

Boss saldırıları ve büyük vuruşlarda:
```csharp
// Cinemachine veya custom impulse
public void ScreenShake(float intensity, float duration)
{
    StartCoroutine(ShakeCoroutine(intensity, duration));
}

IEnumerator ShakeCoroutine(float intensity, float duration)
{
    Vector3 originalPos = cam.transform.localPosition;
    float elapsed = 0f;
    while (elapsed < duration)
    {
        float x = Random.Range(-1f, 1f) * intensity;
        float y = Random.Range(-1f, 1f) * intensity;
        cam.transform.localPosition = originalPos + new Vector3(x, y, 0);
        elapsed += Time.deltaTime;
        // Pixel-snap: intensity değerleri 1/PPU katları
        yield return null;
    }
    cam.transform.localPosition = originalPos;
}
```

---

## PERFORMANs NOTLARI

- 2D Lighting hesaplama maliyetlidir. **Max 15-20 aktif Point Light 2D** per sahne.
- Büyük Outer Radius'lu ışıklar maliyetli. Skill efekti ışıklarını **kısa süreli** tut.
- Partiküller: Max **500 partiküler** aynı anda. Burst pool sistemi kur.
- Post-processing: Bloom en maliyetli efekt. **Downsample: 1** yap (tam res gerek yok).
- Normal map'li sprite sayısını kontrol et — her tile için normal map gerekmiyor.

---

## ÖZET — AŞAMA AŞAMA GÖRSEL HEDEF

| Faz | Görsel Hedef |
|-----|-------------|
| Faz 0 | Placeholder sprite'larla ışık sistemi kur, renk paletleri ayarla |
| Faz 1 | Warblade + 1 düşman için final sprite, basic lighting |
| Faz 2 | Tüm 4 demo class sprite, Act 1 tile seti, partiküller aktif |
| Faz 3 | Normal map'ler, per-act color grading, boss görsel efektleri |
| Full Demo | Post-processing tam stack, ambient partiküller, screen shake tuned |
