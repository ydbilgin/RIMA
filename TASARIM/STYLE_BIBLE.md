# STYLE_BIBLE.md
> **Ne zaman yÃ¼kle:** Yeni asset Ã¼retmeden Ã¶nce â€” tek doÄŸruluk kaynaÄŸÄ±.
> **Ne zaman yÃ¼kleme:** Kod yazma, Unity iÅŸleri sÄ±rasÄ±nda.

Bu dosya `SANAT_PROMPTLARI.md` ve `GORSEL_YONERGE.md`'nin Ã¶zeti. Detay iÃ§in o dosyalarÄ± aÃ§.

---

## KÄ°MLÄ°K

- **TÃ¼r:** Dark fantasy action roguelite
- **Perspektif:** 30-35Â° 3/4 ARPG view (Cursemark / Last Epoch / Hero Siege tarzÄ± â€” yÃ¼z okunaklÄ±, vertical faceler dominant)
- **Referans:** Cursemark (kamera + netlik) + Hades (vibe) + Diablo 2 (atmosfer)
- **PixelLab view parametresi:** `"low top-down"` (25-40Â° native zone) â€” tÃ¼m Ã¼retimlerde. **High top-down YASAK.**
- **YÃ¶n Kilidi:** 4-yÃ¶n (South, East, North, West) + diagonal movement cheat.
- **Style Anchor:** `F:/Antigravity Projeler/2d roguelite/TASARIM/CLASS_CONCEPTS/rima_style_anchor.png`
- **Style anchor (S-XL description):** "Diablo 2 / Darkest Dungeon vibe, heavy dark outline, painterly weathered shading, muted cool palette with warm amber/cyan signature accents, gritty texture."

---

## BOYUT TABLOSU (S43 128px Pivot)

Unity scale her zaman 1x. Boyut PixelLab'da verilir, Unity'de deÄŸiÅŸtirilmez.
**PPU = 128.** (128px sprite = 1 Unity birimi).

| Tip | PixelLab `size` | Oran | Ne zaman |
|---|---|---|---|
| **Player Class** | **128px** | 1.0x | baseline â€” PPU=128, scale=1.0 |
| Knee-high swarm | **80-96px** | 0.6-0.7x | True swarm (Fracture Imp) |
| KÄ±rÄ±lgan support | **128px** | 1.0x (thin) | Thin tall targets (Relic Caster) |
| **Normal grunt** | **128-144px** | 1.0-1.1x | Standart dÃ¼ÅŸmanlar, player'dan hafif bulky |
| AÄŸÄ±r / Elite | **160-176px** | 1.25-1.4x | Odaya girince fark edilen tehdit (Shard Walker) |
| **Mini-Boss** | **180-200px** | 1.4-1.5x | Penitent Sovereign Phase 0 |
| **Boss** | **256-384px** | 2.0-3.0x | Ekrana hakim (Phase 1-2) |
| Floor tile | **128x64** | â€” | Isometric diamond |
| Wall tile | **128x192** | â€” | Two visible faces |

---

## PIXELLAB PARAMETRELERÄ° (S-XL / S43)

```
mode: pro (final) | standard (prototype)
view: low top-down (IMPORTANT: 30-35Â° target)
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

## RENK PALETÄ° â€” ACT 1 (AKTÄ°F)

```
Zemin:     #2C2A2A Â· #3D3535  (koyu enkaz taÅŸÄ±)
Duvar:     #4A3F3F Â· #5C4E4E
Ã‡atlak:    #7BA7BC             (soÄŸuk mavi rift Ä±ÅŸÄ±ÄŸÄ±)
Tehlike:   #8B1A1A             (kan kÄ±rmÄ±zÄ±sÄ±)
UI vurgu:  #A8C8D8             (buzul mavi)
```

---

## CLASS ENERGY RENK TABLOSU â€” KESÄ°N KURAL

| Class | Accent Rengi | Nerede gÃ¶rÃ¼nÃ¼r | YASAK |
|-------|-------------|----------------|-------|
| Warblade | Cold blue (#7BA7BC) | ZÄ±rh Ã§atlaklarÄ±, kÄ±lÄ±Ã§ kenarlÄ±ÄŸÄ± | Ellerde glow, mor |
| Elementalist | Fire/Frost/Lightning | Aktif elemente gÃ¶re | Void energy |
| **Shadowblade** | **Void purple** | Silahtan smoke, gÃ¶zler, ayak tendrilleri | â€” |
| Ranger | Cold blue (minimal) | Sadece ok uÃ§larÄ± | Mor |
| Ravager | Blood red (#8B1A1A) | Rage aura, dÃ¶vme izleri | Mor, mavi |
| **Brawler** | **Void purple** | Yumruklar, dÃ¶vmeler | â€” |
| Ronin | Cold silver-blue | KÄ±lÄ±Ã§ aÄŸzÄ± shimmer | Alev, mor |
| Gunslinger | Cold silver (minimal) | Namlu iÃ§i rift kazÄ±masÄ± | El glow, mor |        
| **Hexer** | **Cursed green + void purple** | Fener, zemin tendrilleri | â€” |
| Summoner | Cold blue | Kristal, Ã§aÄŸÄ±rma daireleri | Mor, yeÅŸil |

---

## YASAK LÄ°STE

- **High top-down YASAK** â€” extreme tepeden aÃ§Ä± identity Ã¶ldÃ¼rÃ¼r.
- **8-yÃ¶n Ã¼retim YASAK** â€” SADECE 4-yÃ¶n (S/E/N/W).
- **64px native YASAK** â€” identity sÄ±kÄ±ÅŸmasÄ±na yol aÃ§ar, tÃ¼m Ã¼retim 128px native.
- **PPU=64 YASAK** â€” yeni standart PPU=128.
- **Upscale YASAK** â€” PixelLab'da ne Ã§Ä±ktÄ±ysa o (128px).
- **Side view YASAK** â€” isometric/3/4 ARPG kuralÄ±.
