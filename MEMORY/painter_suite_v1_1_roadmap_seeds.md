---
name: painter-suite-v1-1-roadmap-seeds
description: Post-v1.0 roadmap seeds for Painter Suite from X posts research (aminerehioui + orb_3d). GameObject-free + splat shader + auto-collider focus.
metadata:
  type: project
---

# PainterSuite v1.1+ Roadmap Seeds

**Source:** agy report `STAGING/x_posts_research_agy_2026_05_26.md` (2026-05-26)
**Origin URLs:**
- https://x.com/aminerehioui/status/2055785406315090062 -- iso map editor, GameObject-free terrain, drag-paint trees/ramps, theme swap
- https://x.com/orb_3d/status/2043745118054940794 -- world-space pixellated splat shader, organic grass/dirt brush

**USP synthesis:** "GameObject-Free, Shader-Driven Organic 2D Level Design with Instant Physics"

## Top 5 seeds (post-v1.0, from X posts research)

1. **GameObject-Free Iso Grid Renderer** (35h, v1.1.0) -- mesh / GPU instancing for terrain
2. **World-Space Pixellated Splat Shader + Brush** (28h, v1.1.0) -- PPU-aligned organic brush, no grid bounds
3. **Auto-Collider Generator from Splat Map** (32h, v1.1.0) -- **HIGHEST sinergy with current ColliderPainter**, automatic Edge/Polygon Collider2D paths from splat mask color channels
4. **Context-Aware Height & Ramp Editor** (22h, v1.1.1) -- iso vertical depth + ramp connector with sorting safety
5. **Real-Time Minimap + Coord Overlay** (15h, v1.1.2) -- nav + dev QoL

## Additional seeds (from LaurethStudio 2D Illusion KB, 2026-05-26 S109 close)

Source: `STAGING/LAURETH_2D_ILLUSION_LIBRARY.md` + [[lauretthstudio-2d-illusion-kb-locked]]

### Illusion automation tools (PainterSuite v1.1+)

6. **Auto-DoF Foreground Layer Generator** (20h, v1.1.x) -- foreground tag'li sprite'lara kamera fokus mesafesine göre dinamik Gaussian Blur shader uygulama. Designer her sahne için ayrı blur texture üretmek zorunda kalmaz.
7. **Parallax Profile Editor (visual curve)** (24h, v1.1.x) -- drag-drop graph UI ile katman Z-depth + ease-in/out hız eğrileri. Matematiksel hesaplama yapmadan derinlik tuning.
8. **Dithered Shadow Baker** (16h, v1.1.x) -- seçilen sprite'ın alt bound'undan otomatik dithered shadow decal üretim ve yerleşim. Karakter/nesne shadow setup → 0 saniye.

### Platform-level libraries (LaurethStudio scope, beyond PainterSuite)

9. **LaurethProc Depth & Parallax Controller** (24h, lib, separate package) -- procgen dünyada katman derinlikleri runtime auto-hesap. Designer sadece sis yoğunluğu + mesafe katsayısı verir.
10. **PainterSuite Fake-3D Billboarding + Normal Generator** (40h, v1.2+) -- 2D sprite'tan edge-detection + bevel → otomatik normal map → URP 3D light eşleşme. **HD-2D democratization** (Square Enix-tier görsel toolset democratized).
11. **LaurethTime Rhythm & ASMR Feedback Sync** (28h, lib, separate package) -- sprite keyframe ↔ audio sample peak freq dynamic align. Cozy/Incremental premium game feel.

## Decoupling discipline (agy critical insight)

PainterSuite's identity = "fizik ve collider boyama suite." Terrain rendering = adjacent concern, not core. Ship as:

- `LaurethStudio.PainterSuite.Runtime` -- existing (Collider templates, ParallaxLayer)
- `LaurethStudio.PainterSuite.Editor` -- existing (window, painters, services)
- `LaurethStudio.PainterSuite.TerrainExtensions` -- NEW, v1.1+, optional sub-module (splat brush + auto-collider from splat)
- Splat shader bundled with TerrainExtensions, ColliderPainter consumes mask as data provider only

This keeps v1.0 focused (ship Day 7), unlocks v1.1+ as "expansion pack" -- repeat customer hook.

## Marketing visuals (agy suggestion)

For Asset Store trailer once v1.1 ships:
- "GameObject vs GameObject-Free" split-screen (10000 tiles, scene hierarchy 1 GO, 60+ FPS)
- Fast-forward organic terrain paint + auto-collider auto-snap
- Iso ramp + sorting axis demo
- Theme swap (desert -> snow) with collider integrity preserved

## Why post-v1.0, not now

- v1.0 scope already locked (4 shapes + templates + layer + demo + submission Day 7)
- Inserting these features now = ship date slips 4-6 weeks, lose momentum
- Better: ship v1.0, get customer feedback, then expand with v1.1 as paid update or free 2.x bump

## Related

- [[painter-suite-plan-v2-locked]]
- [[painter-suite-progress-2026-05-26]]
- STAGING/x_posts_research_agy_2026_05_26.md (full agy report, 2490 words)
