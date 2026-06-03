# Antigravity Research: Wall-less Door / Transition Design + Animated Background

ACTIVE RULES: (1) think before answering (2) min words, structured (3) cite industry examples (4) BLOCKED if unsubstantiated.

Amaç: RIMA için wall-less HYBRID V1 yönü kilitlendi (Hades Elysium). User şimdi 2 alt-sistem tasarlanması istiyor:
1. **Wall-less odalar arası geçiş / kapı** — duvar yoksa "kapı" konsepti nasıl çalışır
2. **Animasyon arkaplan** — siyah void yerine oda ruhuna göre animasyonlu bg

RESPOND INLINE — do NOT write to file.

## RIMA bağlamı (kısa)
- 2D top-down, 35° iso/3-quarter perspective
- Floor = stone tile sınırı (cliff edge'le biter)
- V1 ref: sparse columns + cliff edge + brazier light, NO walls
- Karakter chibi pixel art
- Odalar arası procgen geçiş zorunlu

---

## SORU 1: Wall-less DOOR / TRANSITION (4 başlık altında inline yanıtla)

### 1A. Endüstri örnekleri (5+ oyun, wall-less geçiş çözümleri)
- Bastion → tile rise-up path forms dynamically
- Hyper Light Drifter → cliff edge connector / teleport pad
- Hades → archway with wall framing (hybrid — saf wall-less değil)
- Death's Door → portal stone, cliff bridge
- Children of Morta → small connector room with prop boundary
- ... başka örnekler? Tunic? Sword of the Stars? Stranger of Sword City? Solar Ash?

### 1B. Taksonomi — wall-less geçiş tipleri
- **Floating bridge** (stone tiles spanning void)
- **Cliff descent / ascent** (stair-edge to lower/upper room)
- **Rift portal / teleport** (standalone arch + cyan/magic glow, no wall framing)
- **Tile path appearance** (Bastion-style dynamic build)
- **Light gate** (energy threshold, no physical structure)
- **Mist threshold** (fade-out + fade-in)
- **Door inside object** (büyük statue/altar şeklinde standalone gate, etrafı açık)

### 1C. RIMA özel öneri
- En uygun 2 transition tipi seç
- Procgen room generation'da implementation kolaylığı
- Player readability ("buradan çıkış var" anlık okunur mu)
- Collider implications (trigger zone, scene load timing)

### 1D. Risk + önlem
- Player yanlış edge'e gidip düşme riski (cliff side vs exit cliff)
- AI navmesh — düşman exit'i takip etmeli mi
- Procgen continuity (next room aynı tile floor ile başlamalı)

---

## SORU 2: Animasyon arkaplan (oda ruhuna göre)

### 2A. Endüstri örnekleri (5+ oyun, animated bg under playable area)
- Bastion → parallax cloud + falling debris
- Hyper Light Drifter → dim particle drift + lightning flash
- Hades → ambient embers + lava heat shimmer
- Hollow Knight → 4-5 layer parallax (mostly side-scroller, ama prensip aynı)
- Cuphead → hand-painted bg loop animation
- Children of Morta → subtle dust + glow pulse
- Don't Starve → night fog drift

### 2B. Taksonomi — bg animation tipleri (oda mood'una göre)
- **Particle drift** (dust, embers, mist motes)
- **Parallax depth** (multiple deeper layers, slower scroll)
- **Energy waves** (cyan rift pulse, magic ripple)
- **Weather** (rain, snow, ash falling)
- **Liquid surface** (water shimmer, lava flow)
- **Cosmic/void** (starfield drift, swirl)
- **Static glow pulse** (ambient color shift breath)

### 2C. RIMA oda mood mapping (örnek)
| Oda tipi | Mood | Önerilen bg animasyon |
|---|---|---|
| Combat arena (cyan rune odası) | Energetic | Cyan particle drift + occasional rift pulse |
| Treasure / safe | Calm | Slow dust drift + warm amber glow pulse |
| Boss arena | Intense | Heavy ember + lightning flash + dark void deep |
| Ritual / lore | Mystic | Mist + glyph fade-in/out + cool blue |
| Transition / corridor | Neutral | Gentle parallax cliff deep void |

### 2D. RIMA implementation öneri
- Unity 2D Lights + sprite animation loop on bg quad
- Particle Systems (Unity native)
- Shader: simple scrolling UV for parallax
- Performance: max 2 particle systems + 1 parallax layer per room
- Asset gen: PixelLab create_object 1-direction looping anim OR manual frame-by-frame

---

## SORU 3 (BONUS): RIMA için 5 farklı mood mockup için tek-cümle bg açıklaması
Codex bir sonraki imagegen dispatch'inde her oda için farklı bg yapacak. Sen 5 mood için tek satır bg tarif yaz:
- Combat
- Boss
- Treasure
- Ritual
- Transition corridor

---

## Format
- Toplam 700-900 kelime (TR veya EN)
- Inline structured output
- Her major claim [KAYNAK: ...] tag
- Spekülasyon [SPECULATIVE]
- Net verdict — wishy-washy yok
