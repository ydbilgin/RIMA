---
name: autosprite-vs-pixellab-verdict
description: Community sentiment + RIMA verdict -- PixelLab production kalsin, autosprite sadece non-directional VFX pilot
metadata:
  type: feedback
---

# Autosprite vs PixelLab Verdict

## Rule

PixelLab production karakter + tile + prop pipeline kalsin. Autosprite production'a girmesin.
Non-directional VFX (dash trail, hitspark, aura loop, portal) icin pilot deneme izni var; production'a girmeden once kalite + cost A/B zorunlu.

## Why

- RIMA 4-yon zorunlu (S, SW, W, N minimum); autosprite sidescroller-first, top-down 4-dir weak
- autosprite MCP Pro plan arkasinda -- free plan output kalitesi unverified
- 32x32 tile autotile/Wang support yok; PixelLab Create Topdown Tileset var
- Community kullanimi: hibrit (PixelLab base + autosprite spritesheet export), "kitlesel gecis" FAIL

## How to Apply

1. Yeni gen ihtiyaci → PixelLab default
2. Non-directional VFX (dash trail, hitspark, aura loop, portal) → autosprite pilot dene
3. Pilot sonucu degerlendirmeden production'a ekleme
4. Directional VFX (8-dir attack slash, hit reaction) → PixelLab animate_character veya custom

## Source

- `STAGING/autosprite_vs_pixellab_review.md` (May 2026) -- rima-research community sentiment Q1 2026
- S89 LATE rima-research raporu
