---
status: REFERENCE
faz: 1
tarih: 2026-04-17
ozet: "Görsel stil rehberi (erken dönem)"
---
# STYLE_BIBLE.md
> **Ne zaman yukle:** Yeni asset uretmeden once — tek dogruluk kaynagi.
> **Ne zaman yukleme:** Kod yazma, Unity isleri sirasinda.

Bu dosya `SANAT_PROMPTLARI.md` ve `GORSEL_YONERGE.md`'nin ozeti. Detay icin o dosyalari ac.

---

## KIMLIK

- **Tur:** Dark fantasy action roguelite
- **Perspektif:** **35 derece 3/4 ARPG view — CANONICAL LOCKED.** (CoplayDev High Top-Down style reference). 80 derece overhead concept abandoned.
- **Referanslar:** Cursemark (kamera + netlik) · Last Epoch · Hero Siege — yuz okunakli, vertical faceler dominant
- **Atmosfer:** Hades (vibe) + Diablo 2 (atmosfer)
- **PixelLab view parametresi:** `"low top-down"` (25-40 derece native zone) — tum uretimlerde. **Extreme overhead (80 derece+) YASAK.**
- **Yon Kilidi:** 4-yon (South, East, North, West) + diagonal movement cheat.
- **Style Anchor:** `F:/Antigravity Projeler/2d roguelite/TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
- **Style anchor (S-XL description):** "Diablo 2 / Darkest Dungeon vibe, heavy dark outline, painterly weathered shading, muted cool palette with warm amber/cyan signature accents, gritty texture."

---

## BOYUT TABLOSU (S43 128px Pivot)

Unity scale her zaman 1x. Boyut PixelLab'da verilir, Unity'de degistirilmez.
**PPU = 128.** (128px sprite = 1 Unity birimi).

| Tip | PixelLab `size` | Oran | Ne zaman |
|---|---|---|---|
| **Player Class** | **128px** | 1.0x | baseline — PPU=128, scale=1.0 |
| Knee-high swarm | **80-96px** | 0.6-0.7x | True swarm (Fracture Imp) |
| Kirilgan support | **128px** | 1.0x (thin) | Thin tall targets (Relic Caster) |
| **Normal grunt** | **128-144px** | 1.0-1.1x | Standart dusmanlar, player'dan hafif bulky |
| Agir / Elite | **160-176px** | 1.25-1.4x | Odaya girince fark edilen tehdit (Shard Walker) |
| **Mini-Boss** | **180-200px** | 1.4-1.5x | Penitent Sovereign Phase 0 |
| **Boss (üretim)** | **252-256px** (max) | — | PixelLab `Animate with Text NEW` max 256, asla 384+ değil |
| **Boss (Unity ekran)** | (PPU=64 → 256→512px görsel) / (PPU=32 → 256→1024px görsel "devasa") | 2.0-6.0x | Devasa hissi Unity scale ile, PixelLab'i zorlama |
| Floor tile | **128x64** | — | Isometric diamond |
| Wall tile | **128x192** | — | Two visible faces |

---

## PIXELLAB PARAMETRELERI (S-XL / S43)

```
mode: pro (final) | standard (prototype)
view: low top-down (IMPORTANT: 35 degree canonical)
detail: low detailed (for clarity)
outline: single color outline (black)
ai_freedom: 400
proportions: {"type": "preset", "name": "heroic"} (for classes)
```

**Pro mode zorunlu suffix (128px native):**
```
facing southeast, 3/4 ARPG view, 35 degree angle, isometric pixel art, orthographic projection,
hades game art style, dark fantasy pixel art, transparent background
```

---

## RENK PALETI — ACT 1 (AKTIF)

```
Zemin:     #2C2A2A · #3D3535  (koyu enkaz tasi)
Duvar:     #4A3F3F · #5C4E4E
Catlak:    #7BA7BC             (soguk mavi rift isigi)
Tehlike:   #8B1A1A             (kan kirmizisi)
UI vurgu:  #A8C8D8             (buzul mavi)
```

---

## CLASS ENERGY RENK TABLOSU — KESIN KURAL

| Class | Accent Rengi | Nerede gorunur | YASAK |
|-------|-------------|----------------|-------|
| Warblade | Cold blue (#7BA7BC) | Zirh catlaklari, kilic kenarligi | Ellerde glow, mor |
| Elementalist | Fire/Frost/Lightning | Aktif elemente gore | Void energy |
| **Shadowblade** | **Void purple** | Silahtan smoke, gozler, ayak tendrilleri | — |
| Ranger | Cold blue (#7BA7BC) — RiftGlowVFX runtime | Sadece ok uclari | Mor |
| Ravager | Blood red (#8B1A1A) | Rage aura, dovme izleri | Mor, mavi |
| **Brawler** | **Amber (#FF8800)** | Sol bilek crack, knuckle contact glow | Mor, void purple |
| Ronin | Cold silver-blue | Kilic agzi shimmer | Alev, mor |
| Gunslinger | Cold silver (minimal) | Namlu ici rift kazimasi | El glow, mor |
| **Hexer** | **Cursed green + void purple** | Fener, zemin tendrilleri | — |
| Summoner | Cold blue | Kristal, cagirma daireleri | Mor, yesil |

---

## YASAK LISTE

- **Extreme overhead (80 derece+) YASAK** — kimlik oldurur. 35 derece ARPG canonical locked.
- **8-yon uretim YASAK** — SADECE 4-yon (S/E/N/W).
- **64px native YASAK** — identity sikismasina yol acar, tum uretim 128px native.
- **PPU=64 YASAK** — yeni standart PPU=128.
- **Upscale YASAK** — PixelLab'da ne ciktiysa o (128px).
- **Side view YASAK** — isometric/3/4 ARPG kurali.

