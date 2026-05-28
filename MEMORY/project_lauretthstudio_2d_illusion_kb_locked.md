---
name: lauretthstudio-2d-illusion-kb-locked
description: LaurethStudio studio-level 2D illusion knowledge library. 32 technique catalog + TOP 10 cookbook + PixelLab prompt formulas + 3 platform seeds. Source = agy 2026-05-26.
metadata:
  type: project
---

# LaurethStudio 2D Illusion KB — LOCKED 2026-05-26 (S109 close)

**Full doc (RIMA STAGING):** `STAGING/LAURETH_2D_ILLUSION_LIBRARY.md`
**Master copy (LaurethStudio):** `F:/LaurethStudio/05_RESEARCH/2026_05_26_2d_illusion_library.md`
**Source:** agy dispatch `b8xc1t05i` — studio-level catalog, RIMA-bağımsız reusable knowledge.

## Scope

Tüm LaurethStudio 2D ve hibrit 2.5D oyun projelerinde (Roguelite, Cozy Farm/Sim, 2D RPG, Platformer, Side-Scroller, Top-Down Adventure, Retro Arcade, Casual Incremental) kullanılacak illüzyon teknikleri kütüphanesi.

## 8 Kategori (32 teknik)

1. Depth & Perspective (4) — Parallax, Forced perspective, Atmospheric tint, Vanishing point scale
2. Lighting & Shading (4) — Palette ramps, Dithered shadow, Bevel fake normal, Poor man's bloom
3. Motion (5) — Mode 7, Sub-pixel scroll, Pseudo-zoom, Sprite flicker, Parallax mismatch
4. Volume & 3D (5) — Skewed billboard, HD-2D, Fake reflection, Water refraction, Dimetric squash
5. Particle & Atmosphere (4) — Dust motes, Scroll mist, Ember drag, Sparkle pulse
6. Time & Rhythm (4) — Palette cycling, Ghost trails, Squash easing, ASMR sync
7. Composition & Framing (4) — Letterbox, DoF blur, Vignette, Fog-of-war
8. Engine & Render (2) — Pixel-perfect snap, Shader UV-offset wave

## Studio Cookbook — TOP 10 Cross-Game Patterns

Her oyunda doğrudan uygulanabilir:
1. Dual-axis parallax with ground anchor
2. Squash & stretch jump/land juice
3. Low-opacity vignette overlay
4. Foreground DoF layering
5. Dithered drop shadows
6. Sub-pixel float + integer render
7. Dynamic screen shake + positional decay
8. Poor man's bloom (sprite stacking) — real bloom piksel sanatı bulandırır
9. Color temperature palette shimmer
10. Cinematic viewport reveal

## Common Mistakes (yeni başlayanlar)

- **Uniform parallax velocity** → flat. Logaritmik dağılım: %5 → %150 spread.
- **Baked + dynamic light yön çakışması** → confusion. Sprite'ları nötr çiz.
- **Camera jitter** (no pixel-perfect snap) → titreyen dikey çizgi.
- **Mixels** (16+32+64 PPU yan yana) → illüzyon kırılır. Tek PPU lock zorunlu.
- **Linear alpha fog** → "kirli cam" hissi. Exponential + dithered transition.

## 3 Platform Seeds

| Seed | Tip | Efor | Diferansiyatör |
|---|---|---|---|
| **LaurethProc Depth & Parallax Controller** | Lib | M (3 gün) | Procgen dünyada katman derinlikleri runtime auto-hesap. Tasarımcı sadece "sis yoğunluğu" + "mesafe katsayısı" verir. |
| **PainterSuite Fake-3D Billboard + Normal Gen** | Tool | L (1 hafta) | 2D sprite'tan edge-detection + bevel → otomatik normal map → URP 3D light eşleşme. HD-2D democratization. |
| **LaurethTime Rhythm & ASMR Feedback Sync** | Lib | M (4 gün) | Sprite keyframe ↔ audio sample tepe frekansı dinamik sync. Cozy/Incremental premium game feel. |

## PainterSuite v1.1+ Otomasyon Önerileri (3 ek)

- **Auto-DoF Foreground Layer Generator** — kamera fokus mesafesine göre dinamik Gaussian blur
- **Parallax Profile Editor (visual curve)** — drag-drop Z-depth + ease curve UI
- **Dithered Shadow Baker** — sprite bound'undan otomatik decal üretim

## Related

- [[painter-suite-v1-1-roadmap-seeds]] — güncellendi, LaurethStudio illusion seed'leri eklendi
- [[painter-suite-plan-v2-locked]]
- [[project-laureth-studio-master-plan]]
- `STAGING/LAURETH_2D_ILLUSION_LIBRARY.md` (full doc, 32 teknik)
- `STAGING/x_posts_research_agy_2026_05_26.md` (precursor — splat shader + iso editor seeds)
