# Scene v4 vs M3 — Antigravity Visual Review (Cycle 3 Verdict)

## 1. Re-Score Card (v4 vs v3/Cycle 2)

* **Tonal Contrast & Readability**: UP (Global Light raised to 0.22, but still significantly darker than M3's bright cobblestones).
* **Brazier Corners & Local Lights**: UP (Orange flames and warm amber lights are now active and casting actual light on props and floors).
* **Central Portal Glow**: UP (Target sorting layers fixed, allowing the cyan glow to successfully illuminate the floor decals).
* **Lightning/Storm BG**: UP (Particle emission rate, trails, and sorting order adjusted, making the storm active and visible).
* **Nebula/Void Atmosphere**: UP (Brighter purple and void tints applied, providing a visible background silhouette).
* **Columns/Statues**: UP (Pillars scaled to 1.5x and illuminated by dedicated amber lights, enhancing their depth).
* **"Looks Alive" Factor**: UP (The combination of flickering flames, trail-emitting lightning, and local glows adds dynamic movement).

## 2. Final Decision

We pick **POLISH-CYCLE-4 (final)**. 

While v4 is structurally sound and technical sorting-layer bugs are 100% resolved, it remains too dark to match the combat-readability and high-contrast epicness of the M3 reference mockup. Because we are so close, a final 30-minute micro-polish of numeric values—without needing any new authored assets—will close the remaining tonal gap, make the floor assets pop, and elevate this to a premium, production-ready demo.

## 3. Top 3 Micro-Tweaks (No New Art)

1. **Global Light Intensity Boost (0.22 → 0.38)**
   - *Target*: Raise `Global Light 2D` intensity on the `Floor` and `Ground` layers to enhance playfield readability and mimic M3's bright, reflective cobblestone floor.
2. **Central Portal Cyan Glow Boost (2.5 → 5.0 with HDR)**
   - *Target*: Set the `CentralPortal_CyanGlow` intensity to `5.0` to create an intense, energetic focal point at the arena's center.
3. **Brazier Warm Light Boost (2.2 → 4.5) & Bloom Enable**
   - *Target*: Boost the local orange lights on the 4 braziers to `4.5` to cast dramatic warm highlights on the pillars. Enable Unity Post-Process Bloom to make the emissive lightning and cyan runes pop.

VERDICT: POLISH-CYCLE-4
