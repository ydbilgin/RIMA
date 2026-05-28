# Antigravity Visual Review: Scene v2 vs M3 Reference

Visual audit of Codex Cycle 1 PlayableArena against Hades Elysium M3 target reference.

## 1. Score Card

1. **Arena shape** (★★★½☆)
   - *Gap:* Accurate hex silhouette, but slightly too narrow relative to M3’s broad play space.
   - *Cycle 2 should:* Expand floor size slightly to 14x9.
   - *Achievable via:* Tilemap repaint.

2. **Central rune circle focal** (★★☆☆☆)
   - *Gap:* Scattered runes look disjointed; lacks M3's prominent glowing circular central focal point.
   - *Cycle 2 should:* Place a central decal ring with high-intensity light.
   - *Achievable via:* Decal placement / additive Light2D.

3. **Brazier corners** (★★☆☆☆)
   - *Gap:* Mismatched colors (SW/SE cyan) and missing brazier props; M3 has four uniform warm flame braziers.
   - *Cycle 2 should:* Pull corner props and unify light colors to warm orange.
   - *Achievable via:* Pulled sprites / Light2D intensity.

4. **Cliff face perimeter** (★★½☆☆)
   - *Gap:* Cliffs are uniform and visually box-in the small arena due to vertical scale.
   - *Cycle 2 should:* Scale down/vary cliff sprites and offset positions outward.
   - *Achievable via:* Scaling / sprite selection.

5. **Lightning/storm BG** (★★☆☆☆)
   - *Gap:* Purple background electric streaks are completely missing.
   - *Cycle 2 should:* Add electric particle spikes in the nebula background.
   - *Achievable via:* Particle system / Sprite Mask.

6. **Nebula/void atmosphere** (★★½☆☆)
   - *Gap:* Cyan-dominated void; misses M3's signature purple gas and deep dark contrast.
   - *Cycle 2 should:* Shift background sprite tints to purple and add haze particles.
   - *Achievable via:* Shader / sprite tinting.

7. **Columns/statues** (★★☆☆☆)
   - *Gap:* Outer edge framing columns are missing.
   - *Cycle 2 should:* Place broken pillar and statue props at outer arena corners.
   - *Achievable via:* Pulled sprites / scale.

8. **Tonal contrast** (★★½☆☆)
   - *Gap:* Scene is flat and dim overall due to high global light (0.3) and weak local emissions.
   - *Cycle 2 should:* Drop global light to 0.15 and boost local light intensities.
   - *Achievable via:* Light2D intensity.

9. **Cyan glow intensity** (★★½☆☆)
   - *Gap:* Ground runes and crystal spots lack vibrant emissive bloom.
   - *Cycle 2 should:* Add local glowing lights under cliff edges and intensify runes.
   - *Achievable via:* Light2D intensity / LightPulse tuning.

10. **Overall "looks alive"** (★★☆☆☆)
    - *Gap:* Flat grid tiles look sterile and feel like a test environment rather than a combat arena.
    - *Cycle 2 should:* Dress borders with rubble, dirt patches, and dynamic torch flickers.
    - *Achievable via:* Decals / scatter props.

## 2. Top 5 Cycle 2 Changes

| Change | Impact / Effort | Specific Action |
|---|---|---|
| **1. Four Corner Braziers** | High / Low | Pull `41342e20` (mounting apparatus) from PixelLab catalog. Place at all 4 corners with warm Point Light2D (intensity 2.0, radius 1.5). |
| **2. Central Portal & Glow** | High / Medium | Pull decorative decal `5ccc5721`. Center it at origin with an additive Light2D (intensity 2.5, radius 2.0). |
| **3. Corner Framing Pillars** | High / Medium | Pull broken stone pillar `6b52751d-67eb-4684-b7e4-f4a0a00c7831`. Place at arena bounds to block/frame view. |
| **4. High-Contrast Contrast** | High / Low | Set Global Light 2D to 0.15. Boost corner torches to 2.2. Add cyan rim lights under Kit B cliffs to cast outward. |
| **5. Purple Storm BG** | Medium / Medium | Tint `bg_L1_nebula` to HSL violet. Add Particle System on Nebula layer emitting magenta stretched-billboards. |

## 3. Overall Assessment

Cycle 1 is a solid technical blockout that establishes correct layer sorting and floating layout geometry, but it is visually too dark, sterile, and lacks the structural framing props of Hades Elysium M3. It cannot ship as a playable demo. Cycle 2 is mandatory to implement corner props, the central portal focal, and high tonal contrast.

VERDICT: ITERATE-CYCLE-2
