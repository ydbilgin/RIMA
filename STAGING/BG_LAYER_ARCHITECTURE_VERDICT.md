# RIMA Wall-less BG Layer Architecture — 3-AI Synthesis (2026-05-25)

> ⚠️ **SUPERSEDED (parallax factors):** Bu doc 0.03–0.14 dar aralık önerir. **GÜNCEL canonical = F3 6-katman (0.05–1.10)** — `STAGING/F3_PARALLAX_6LAYER_DONE.md` (2026-05-27, kullanıcı onayı 2026-05-28). BG_Void 0.05/-500 · BG_Far 0.15/-420 · BG_Mid 0.30/-350 · BG_Near 0.50/-300 · Mid_Ground 0.85/+10 · Foreground_Front 1.10/+600. Bu doc'un Kit yapısı/sorting referansı hâlâ kullanılabilir, sadece factor değerleri eski.

**Cross-validation:** ChatGPT (user externally) + Antigravity (Gemini 3 Pro) + Codex (gpt-5.5) + Opus synthesis.

**CONVERGED VERDICT: `parallax-modular` (single-image NOT, panorama NOT).**

Both Antigravity + Codex independently converged. ChatGPT pending review (user has it).

---

## Section 1: UNIFIED LAYER ARCHITECTURE (6 layers)

| # | Layer | Tekrar tipi | Boyut (PixelLab S-XL Pro REAL limits) | Aspect | Opaklık | Rol | Tool |
|---|---|---|---|---|---|---|---|
| L0 | **Void Base** | Seamless 2D tileable | **512×512** | 1:1 max | Opaque | En dipteki sonsuz karanlık + hafif cyan rift damar | S-XL Pro 1:1 (40 gen) |
| L1 | **Cyan Nebula / Rift Hero** | Unique (oda merkez) | **512×512** | 1:1 max | Transparent | Boss/oda merkezi rift hub | S-XL Pro 1:1 (40 gen) |
| L2 | **Far Ruins Strip A/B** | Horizontal tileable | **688×384** (×2 variant) | 16:9 max | Transparent | Uzak harabe siluetleri | S-XL Pro 16:9 (40 gen) |
| L3 | **Floating Islands** | Modular unique | **256×256** small + **512×512** large | 1:1 | Transparent | Bağımsız kaya parçaları (derinlik) | small=create_object 4-batch, large=S-XL Pro |
| L4 | **Atmospheric Fog Veil** | Wide tileable | **688×384** | 16:9 max | Transparent (low alpha) | Derinlik sisi, katman ayrımı | S-XL Pro 16:9 (40 gen) |
| L5 | **Arena Floor (LIVE)** | Unity Tilemap | Dynamic | — | Opaque | Oynanabilir taş zemin | Mevcut Tilemap |
| (+) | **Light beam / decal** | Unique additive | **512×512** veya **384×216** | 1:1 / 16:9 | Transparent additive | Cyan rift + brazier glow | S-XL Pro |
| (+) | **Particle sheet** | Sprite sheet 4×4 | **256×256** | 1:1 | Transparent | Cyan mote / spark / dust | create_object |

**Önemli (2026-05-25 düzeltme):** PixelLab S-XL Pro max boyut aspect-ratio'ya göre değişir, total area ~262K px² sabit (1:1=512×512, 16:9=688×384, 2:3=384×688). 1024×1024 MÜMKÜN DEĞİL. Wide strip için 688×384 üret, Unity'de horizontal tile et (2 strip = 1376 px wide).

**Toplam asset üretimi:** 8 layer × varyasyon = ~15-20 unique PNG (PixelLab tek-tek üretilebilir, ~150-250 gen).

---

## Section 2: UNITY SETUP — Sorting + Parallax + Math

### Sorting hierarchy (Codex range, daha granular)

| Layer | Sorting Order | Parallax X,Y | Z (editor) | URP material |
|---|---:|---:|---:|---|
| BG_Void (L0) | **-500** | 0.03, 0.02 | +10 | Sprite Default (Unlit) |
| BG_Far (L2) | -420 | 0.08, 0.05 | +8 | Unlit Transparent |
| BG_Mid (L3) | -350 | 0.14, 0.08 | +6 | Sprite Diffuse |
| BG_FogBack (L4) | -300 | 0.10, 0.06 | +5 | Custom Alpha Scroll |
| BG_Rift (L1) | -250 | 0.06, 0.04 | +7 | Additive Blend |
| **ArenaFloor (L5)** | **0** | 1.00, 1.00 | 0 | Lit URP 2D |
| Props | 100 | 1.00, 1.00 | 0 | Lit URP 2D |
| Characters | 300 | 1.00, 1.00 | 0 | Lit URP 2D (SortingGroup) |
| FrontFX (front fog, hit flash) | 600 | 0.95, 0.95 | -2 | Additive |

### Parallax C# formula (Antigravity)

```csharp
public class ParallaxLayer : MonoBehaviour {
    [SerializeField] Vector2 parallaxFactor;
    Transform cam;
    Vector3 lastCam;
    
    void Start() { cam = Camera.main.transform; lastCam = cam.position; }
    
    void LateUpdate() {
        Vector3 delta = cam.position - lastCam;
        Vector3 newPos = transform.position + new Vector3(delta.x * parallaxFactor.x, delta.y * parallaxFactor.y, 0);
        // Pixel snap (Codex addition — prevents shimmer at 64 PPU)
        newPos.x = Mathf.Round(newPos.x * 64f) / 64f;
        newPos.y = Mathf.Round(newPos.y * 64f) / 64f;
        transform.position = newPos;
        lastCam = cam.position;
    }
}
```

### Performance budget (Codex)
- BG renderers: **8-14 SpriteRenderer**
- Total room renderers: **30-45**
- Boss room max: **60**
- Sprite Atlas single: `RIMA_BG_ElysiumWallless.spriteatlas` (2048-4096 max)
- Draw call hedef: BG **3-6 batch** (atlas birleşik)

---

## Section 3: ROOM TYPE VARIATIONS (aynı sistem, parametre değişir)

| Oda tipi | L0 Void | L1 Rift | L2 Ruins | L3 Islands | L4 Fog | L1 Cyan tonu |
|---|---|---|---|---|---|---|
| **Combat** | Standard tile | Subtle drift | 1 strip variant | 3-5 small | Light low alpha | Medium |
| **Boss** | Standard tile | LARGE central pulse | 1-2 strips | 1 large landmark | Heavy embers | High contrast pulse |
| **Treasure / Safe** | Standard tile | Disabled OR warm pulse | 1 strip | 2 small | Warm soft veil | Low (warm dominates) |
| **Ritual** | Standard tile | Glyphs particle | 1 strip | 2 medium | Heavy purple mist | Purple shift |
| **Transition corridor** | Standard tile | Off | 0 (just void) | 0 | Deep parallax | Off |

**Sistem değişmiyor** — sadece layer enable/disable + sprite swap + palette LUT shift.

---

## Section 4: PRODUCTION CHECKLIST (Codex MVP order)

### MVP path (1 hafta)
1. **L0 Void Base** üret (512×512 tileable, 1 PixelLab gen)
2. **L2 Far Ruins Strip A** üret (1024×256 transparent, 1 gen)
3. **Unity ParallaxLayer.cs** yaz (C# script above) + scene test
4. **L3 4 Floating Islands** üret (256×256 transparent, 1 sheet via create_object n_frames=16 batch)
5. **L4 1 Fog Veil** üret (1024×256 transparent, 1 gen)
6. **Sprite Atlas** kur, sorting orders set
7. **Test screenshot** 1920×1080 + 1366×768 — depth okunuyor mu

### Nice-to-have (sonraki sprint)
- L1 Cyan Nebula Hero (1024×1024 unique, 1 gen, boss arena için)
- Far Ruins Strip B + C (variation)
- L3 Large Floating Island (512×512, boss landmark)
- Light beam decals (512×512 additive, 2-3 variant)
- Particle sheet (256×256 4×4 frames, 1 gen)
- Biome LUT (Shader Graph cyan/warm/purple shift)
- Editor preset selector

---

## Section 5: PIXELLAB GENERATION PROMPTS (her layer)

### L0 Void Base (512×512 tileable opaque)
```
Top-down 2D pixel art seamless tile of deep magical void, dark indigo space (#0A0E1A) with subtle cyan rift veins (#00FFCC at 10% intensity), Hades Elysium inspired premium ARPG style. No walls, no characters, no horizon. Low contrast center, edges seamlessly tile. 64 PPU readable. Hard pixel edges, no anti-aliasing.
```

### L1 Cyan Nebula / Rift Hero (1024×1024 unique transparent)
```
Large transparent pixel art cyan rift nebula sprite, swirling magical energy with cyan (#00FFCC) glowing core fading to deep purple (#3A1A4A) edges. Central focal point with soft alpha radial gradient. Premium ARPG dark fantasy. No characters, no floor. For overlay above void backdrop.
```

### L2 Far Ruins Strip (1024×256 wide horizontal transparent)
```
Wide transparent pixel art background strip of distant broken marble ruins floating far below a sky void. Warm gold highlights (#E89020) on column edges, cyan cracks (#00FFCC) in stone. High top-down 3/4 camera angle. Soft silhouettes, low contrast, designed to tile horizontally seamlessly. No floor, no characters.
```

### L3 Floating Island (256×256 small transparent)
```
Transparent pixel art floating stone island chunk, weathered dark slate gray (#3A3D42) with broken irregular edges, faint cyan glow underside (#00FFCC 30% alpha). Top-down 3/4 ARPG perspective. Mossy patches on top. Readable at 256 px. No background.
```

### L3 Large Floating Island (512×512 boss landmark)
```
Transparent pixel art large floating stone monument fragment, ancient marble pillars and broken stairs, ritual altar visible, cyan rift cracks at base (#00FFCC), warm gold weathering (#E89020). Top-down 3/4 ARPG perspective. Designed as boss arena backdrop landmark. No characters.
```

### L4 Fog Veil (1024×256 horizontal transparent low alpha)
```
Transparent pixel art soft magical fog veil, pale cyan (#80D0FF 15% alpha) and warm gold (#E89020 10% alpha) tint blend, horizontal drifting strip. Low detail organic shapes. Designed to overlay void background as depth cue without obscuring gameplay above.
```

### Light Beam Decal (512×512 additive)
```
Transparent pixel art vertical magical light beam decal, cyan rift energy (#00FFCC) at top fading to warm orange (#E89020) at base, soft additive edges. Designed to layer over arena edges as glow accent. No background, no characters.
```

### Particle Sheet (256×256 4×4 frames)
```
Small transparent pixel art particle sheet for Unity particle system: cyan motes (#00FFCC), warm orange sparks (#E89020), tiny rift dust mote, 4x4 frame grid. Clean silhouettes, 16-32 px per cell. Designed for ambient drift in floating arena void.
```

---

## Section 6: NEXT-STEP DISPATCH RECOMMENDATIONS

| Adım | Aksiyon | Effort |
|---|---|---|
| 1 | User PixelLab web UI'da L0 Void Base üret (1 gen) | 2 dk |
| 2 | User L2 Far Ruins Strip üret (1 gen) | 2 dk |
| 3 | Codex dispatch: ParallaxLayer.cs + scene test setup | 15 dk |
| 4 | User L3 4 Floating Islands batch (1 PixelLab call = 4 sheet 256×256) | 3 dk |
| 5 | User L4 Fog Veil üret (1 gen) | 2 dk |
| 6 | Codex Unity scene: Sprite Atlas + parallax wire-up + screenshot | 15 dk |
| 7 | QC: 1920×1080 + 1366×768 screenshot, depth okunabilirlik kontrolü | User + Opus |
| **TOPLAM MVP** | ~3 PixelLab gen + 30 dk Codex + 5 dk user web UI | **1 saat shippable** |

**Sonraki batch:** Boss arena için L1 Cyan Nebula + L3 Large Island + Particle Sheet.

---

## Section 7: COMPARISON SUMMARY (3-AI verdict matrix)

| Boyut | ChatGPT (user externally) | Antigravity | Codex | Convergence |
|---|---|---|---|---|
| Architecture | (user verify) | Parallax-modular 6 layer | Parallax-modular 4 BG + 2 VFX | **✅ AGREE** |
| Void Base | (user verify) | 512×512 tileable | 512×512 tileable | **✅ AGREE** |
| Far Ruins | (user verify) | 1024×512 strip | 1024×256 strip A/B/C | **✅ AGREE on strip** |
| Floating Islands | (user verify) | 256-512 modular | 256-512 modular | **✅ AGREE** |
| Parallax range | (user verify) | 0.05-0.35 | 0.03-0.14 | Minor — Opus pick conservative (Codex range) |
| Sorting | (user verify) | -100 to 0 | -500 to +600 | Opus pick Codex (extended for FX) |
| Pixel snap | (user verify) | (none) | round*64/64 | Codex addition LOCKED |
| Performance budget | (user verify) | (none) | 8-14 BG SR, 30-45 room | Codex LOCKED |
| Production order | (user verify) | MVP→nice | MVP→nice (more linear) | Codex order LOCKED |

**Opus final verdict:** Codex'in pratik Unity-specific detayları + Antigravity'nin görsel hierarchy = unified architecture above. ChatGPT'nin yanıtını gör, eklemek/çıkarmak istediğin varsa söyle.
