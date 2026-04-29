# KIRO — PROJECTILE + IMPACT BATCH 1
*Bu dosyayı oku, sırayla uygula, başka dosya okuma.*
*Tüm görevler create_map_object ile yapılır. animate_character veya create_character kullanma.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## SİSTEM MANTIĞI — BU BATCH NE ÜRETECEK

Oyunda 3 kategori görsel var:

1. **In-flight sprite** — Projectile uçarken görünen sprite. Unity bunu hareket ettirir.
2. **Impact hit sprite** — Düşmana çarptığında hit noktasında spawn olur, 3-5 frame oynar, yok olur.
3. **Impact miss sprite** — Duvara veya boşluğa çarptığında spawn olur, yok olur.

Lightning: sprite üretilmez — Unity LineRenderer ile yapılır.

---

## GÖREV 1 — Ok / Arrow (Ranger)

### 1a — In-flight
```
create_map_object(
  description="pixel art game projectile, wooden arrow in flight, horizontal orientation pointing right, brown wooden shaft with grey metal arrowhead at right tip, small feathered fletching at left end, simple clean design, dark fantasy roguelite style, single centered object on transparent background",
  width=24,
  height=8,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Ranger/arrow_flight.png`

### 1b — Impact hit (düşmana çarptı)
```
create_map_object(
  description="pixel art game hit impact effect, small blood and force burst from arrow impact, red-crimson splatter with radial energy lines, impact flash, top-down view, 3-frame feel captured in single image, single centered object on transparent background",
  width=24,
  height=24,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Ranger/arrow_impact_hit.png`

### 1c — Impact miss (duvara çarptı)
```
create_map_object(
  description="pixel art game impact effect, arrow hitting stone wall, small grey stone dust chips and puff burst, debris scatter lines, muted grey and brown tones, top-down view, single centered object on transparent background",
  width=20,
  height=20,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Ranger/arrow_impact_miss.png`

---

## GÖREV 2 — Fireball / Ateş Topu (Elementalist)

### 2a — In-flight
```
create_map_object(
  description="pixel art game magic projectile fireball, small glowing orange and yellow fire orb in flight, bright hot core with trailing flame wisps behind it, rift energy blue-purple glow at core center, dark fantasy roguelite style, horizontal orientation, single centered object on transparent background",
  width=20,
  height=16,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/fireball_flight.png`

### 2b — Impact hit
```
create_map_object(
  description="pixel art game fire explosion impact effect, orange and red flame burst with radiating heat lines, bright yellow-white core at center, small ember particles scattering outward, top-down view, single centered object on transparent background",
  width=32,
  height=32,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/fireball_impact_hit.png`

### 2c — Impact miss
```
create_map_object(
  description="pixel art game fire scorching impact on stone, small scorch mark burst with fading embers and smoke puff, dark grey-orange tones, top-down view, single centered object on transparent background",
  width=24,
  height=24,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/fireball_impact_miss.png`

---

## GÖREV 3 — Glacial Spike / Buz Dikeni (Elementalist)

### 3a — In-flight
```
create_map_object(
  description="pixel art game magic projectile ice spike, elongated sharp ice crystal shard in flight, horizontal orientation pointing right, cold blue and white tones, icy sharp tip at right end, frost sparkle at edges, dark fantasy roguelite style, single centered object on transparent background",
  width=28,
  height=10,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/ice_spike_flight.png`

### 3b — Impact hit
```
create_map_object(
  description="pixel art game ice shatter impact effect, blue-white ice crystal explosion with sharp shards flying outward radially, frost burst with snowflake-like scatter pattern, cold blue tones, top-down view, single centered object on transparent background",
  width=32,
  height=32,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/ice_spike_impact_hit.png`

### 3c — Impact miss (aynı shatter kullanılır)
```
create_map_object(
  description="pixel art game ice impact on stone wall, small ice crystal shatter burst with frost dust and tiny ice chips, pale blue and grey tones, top-down view, single centered object on transparent background",
  width=22,
  height=22,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/ice_spike_impact_miss.png`

---

## GÖREV 4 — Wind Blade / Rüzgar Bıçağı (Elementalist)

### 4a — In-flight
```
create_map_object(
  description="pixel art game magic projectile wind crescent blade, thin crescent or curved slash shape in flight, horizontal orientation pointing right, translucent pale green and white air distortion tones, sharp curved edge with motion blur lines trailing behind, dark fantasy roguelite style, single centered object on transparent background",
  width=22,
  height=14,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/wind_blade_flight.png`

### 4b — Impact hit
```
create_map_object(
  description="pixel art game wind slash impact effect, radial air burst with sharp curved slash lines emanating outward, pale green-white translucent tones, top-down view, wind pressure ring visible, single centered object on transparent background",
  width=30,
  height=30,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/wind_blade_impact_hit.png`

---

## GÖREV 5 — Lightning Flash (Elementalist — sadece impact, in-flight yok)

Lightning in-flight sprite yok — Unity LineRenderer çizer. Sadece hedefte flash gerekli.

### 5a — Impact hit flash
```
create_map_object(
  description="pixel art game lightning strike impact flash effect, bright yellow-white electric burst at center with jagged spark lines radiating outward, electric arc splinters, intense bright core fading to pale blue-white edges, top-down view, single centered object on transparent background",
  width=32,
  height=32,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Elementalist/lightning_impact_hit.png`

---

## GÖREV 6 — Generic Physical Hit (Warblade + Shadowblade melee)

Warblade ve Shadowblade'in yakın dövüş vuruşlarında kullanılır.

### 6a — Impact hit (slash/strike)
```
create_map_object(
  description="pixel art game melee hit impact effect, sharp white-yellow impact flash with speed slash lines, bold impact star burst shape at center, dark fantasy roguelite style, top-down view, single centered object on transparent background",
  width=28,
  height=28,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Melee/melee_impact_hit.png`

### 6b — Heavy impact (Warblade heavy attack, daha büyük)
```
create_map_object(
  description="pixel art game heavy melee impact effect, large powerful shockwave burst with blue-purple rift energy at edges, ground crack lines radiating outward from center, heavy impact feeling, top-down view, single centered object on transparent background",
  width=40,
  height=40,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/Projectiles/Melee/heavy_impact_hit.png`

---

## GÖREV 7 — Status Efekt Overlay: ICE FREEZE

Düşman veya oyuncu donunca üstüne overlay sprite bindirilir. Unity SpriteRenderer olarak spawn edilir.

```
create_map_object(
  description="pixel art game status effect overlay, humanoid figure completely encased in solid blue ice crystal shell, full body ice block shape, jagged ice facets visible on surface, cold blue and white tones with inner glow, transparent background so only ice shell visible, top-down low angle view matching character perspective, dark fantasy roguelite style",
  width=80,
  height=96,
  view="low top-down",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/StatusEffects/freeze_overlay.png`

---

## GÖREV 8 — Status Efekt Overlay: STUN İNDİKATÖRÜ

Baş üstünde dönen yıldız/şimşek halkası. Unity'de karakterin üstünde ayrı obje olarak döner.

```
create_map_object(
  description="pixel art game stun status indicator, small circular arrangement of yellow electric sparks and stars orbiting a center point, lightning bolt mini icons mixed with small stars, bright yellow and white tones, meant to float above character head, single centered object on transparent background",
  width=32,
  height=20,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/StatusEffects/stun_indicator.png`

---

## GÖREV 9 — Status Efekt: WIND LAUNCHED (Hava Fırlatma)

Karakterin ayak altında beliren rüzgar girdabı. LAUNCHED status'ta spawn olur.

```
create_map_object(
  description="pixel art game wind launch effect at feet, upward swirling wind vortex, pale green-white translucent spiral of air currents, lifting force visual effect, seen from side-low angle, dynamic upward motion lines, single centered object on transparent background",
  width=40,
  height=40,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/StatusEffects/wind_launched.png`

---

## GÖREV 10 — Blink VFX (Elementalist Dash)

Işınlanma: karakter kaybolunca ve belirince oynayan 2 sprite.

### 10a — Blink Out (kaybolma)
```
create_map_object(
  description="pixel art game teleport disappear effect, humanoid silhouette dissolving into blue-purple rift energy shards and light particles, fragments scattering outward from center, magical teleport vanish visual, dark fantasy style, single centered object on transparent background",
  width=56,
  height=72,
  view="low top-down",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/StatusEffects/blink_out.png`

### 10b — Blink In (belirme)
```
create_map_object(
  description="pixel art game teleport appear effect, blue-purple rift energy shards and light particles converging inward from edges, coalescing into humanoid form at center, magical teleport arrival visual, bright rift energy core, dark fantasy style, single centered object on transparent background",
  width=56,
  height=72,
  view="low top-down",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/StatusEffects/blink_in.png`

---

## GÖREV 11 — Boss AoE Tell: Daire (Ground Slam)

Zemine belirecek uyarı zonu. Hasar gelmeden 1.5s önce spawn olur, kırmızı-turuncu yanıp söner.

```
create_map_object(
  description="pixel art game danger zone warning indicator, circular red-orange glowing area marker on ground, pulsing danger ring shape, transparent center with bright red warning border and hazard lines, used as telegraphed AoE ground attack warning, top-down view, single centered object on transparent background",
  width=80,
  height=80,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/BossVFX/aoe_tell_circle.png`

---

## GÖREV 12 — Boss AoE Tell: Koni (Charge / Nefes Saldırısı)

Boss'un baktığı yönde açılan tehlike konisi.

```
create_map_object(
  description="pixel art game danger zone warning indicator, cone-shaped red-orange glowing area marker on ground, fan or wedge shape pointing from narrow tip outward, bright red warning border, pulsing hazard lines inside cone, telegraphed directional AoE warning, top-down view, single centered object on transparent background",
  width=96,
  height=80,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/BossVFX/aoe_tell_cone.png`

---

## GÖREV 13 — Boss AoE Tell: Çizgi (Dash Yolu)

Boss dash ederken geçeceği çizgi üzerinde uyarı.

```
create_map_object(
  description="pixel art game danger zone warning indicator, narrow rectangular line-shaped red-orange glowing area marker on ground, long thin danger strip showing dash path direction, bright red warning border with hazard arrows pointing along direction, telegraphed boss dash path warning, top-down view, single centered object on transparent background",
  width=120,
  height=28,
  view="top",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```
Kaydet: `STAGING/BossVFX/aoe_tell_line.png`

---

## KAYIT YAPISI

```
STAGING/Projectiles/
  Ranger/
    arrow_flight.png
    arrow_impact_hit.png
    arrow_impact_miss.png
  Elementalist/
    fireball_flight.png
    fireball_impact_hit.png
    fireball_impact_miss.png
    ice_spike_flight.png
    ice_spike_impact_hit.png
    ice_spike_impact_miss.png
    wind_blade_flight.png
    wind_blade_impact_hit.png
    lightning_impact_hit.png
  Melee/
    melee_impact_hit.png
    heavy_impact_hit.png
StatusEffects/
    freeze_overlay.png
    stun_indicator.png
    wind_launched.png
    blink_out.png
    blink_in.png
BossVFX/
    aoe_tell_circle.png
    aoe_tell_cone.png
    aoe_tell_line.png
```

---

## SIRA

1–13 sırayla yap. Her görev bağımsız, paralel gönderme.
Bitince `STAGING/DONE.txt`'e ekle:
```
[PROJ-DONE] ProjectileAdı | id | tarih
```
