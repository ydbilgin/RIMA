#!/usr/bin/env python3
"""
Pixel Art & Animasyon Araştırması — 2D Roguelite
=================================================
Araştırılan konular:
  1. Top-down 2D pixel art karakter animasyonu — frame sayıları, timing
  2. Sprite sheet organizasyonu ve boyut standartları
  3. Top-down 2D düşman ve boss tasarım prensipleri
  4. VFX / Efekt animasyonu pixel art'ta
  5. Renk paleti teorisi — dark fantasy için
  6. İkon tasarımı — skill ikonları için best practice
  7. Pixel art için referans görsel kullanımı — workflow
  8. Children of Morta / Enter the Gungeon görsel analizi
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

CIKTI_DOSYASI    = r"F:\Antigravity Projeler\2d roguelite\ART\PIXELART_ARASTIRMA_CIKTISI.md"
HAM_VERI_DOSYASI = r"F:\Antigravity Projeler\2d roguelite\ART\PIXELART_ARASTIRMA_HAM.md"

GPU_OPTIONS = {
    "temperature":    0.6,
    "top_p":          0.9,
    "num_predict":    6000,
    "num_ctx":        4096,
    "num_gpu":        99,
    "repeat_penalty": 1.05,
}

FORMAT_ONEKI = """Structure your response in EXACTLY TWO PARTS:

### PART 1 — RAW DATA
List ALL facts, names, numbers in TABLE or BULLET format. No analysis yet.

### PART 2 — ANALYSIS
Practical lessons, best practices, concrete recommendations.

Now answer:
---
"""

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — Top-Down 2D Karakter Animasyonu: Frame Sayıları ve Timing",
        "prompt": """
You are an expert pixel art animator specializing in top-down 2D action games.

Research and answer: What are the EXACT frame counts and timing (FPS/ms per frame) used in professional
top-down 2D pixel art games for these animations?

For EACH animation type below, give:
- Minimum frames (indie/solo dev acceptable quality)
- Standard frames (professional quality)
- FPS / ms per frame
- Key frames that MUST exist (windup, impact, recovery etc.)
- Games that use this timing as reference (Hades, Enter the Gungeon, Children of Morta etc.)

Animation types to cover:
1. Player idle
2. Player walk/run
3. Player basic attack (melee)
4. Player dash/dodge
5. Player hit (taking damage)
6. Player death
7. Player skill/magic cast
8. Enemy idle
9. Enemy walk
10. Enemy basic attack
11. Enemy death
12. Boss idle
13. Boss attack (slow/heavy)
14. Boss special/charge-up
15. Boss death

Also: What is the difference in feel between 6fps, 8fps, 12fps, 16fps animations?
When should you use each?
"""
    },
    {
        "baslik": "BOLUM 02 — Sprite Boyutları ve Sprite Sheet Standartları",
        "prompt": """
You are an expert in 2D game development and pixel art asset pipeline.

Research: What are the industry standard sprite sizes for top-down 2D pixel art action games?

Answer for each category:
- Recommended pixel dimensions (width × height)
- Why this size? (readability, detail, performance)
- Real game examples using this size
- Unity PPU (Pixels Per Unit) recommendation

Categories:
1. Player character (top-down action roguelite)
2. Standard enemy (grunt/minion)
3. Elite enemy (larger, more detailed)
4. Boss character
5. Wall/floor tiles
6. Props (barrels, chests, torches)
7. Projectiles (arrows, fireballs, etc.)
8. Skill icons (UI)
9. VFX/effects
10. Character portrait (selection screen)

Also: Sprite sheet layout — rows vs columns, what format does Unity prefer?
What is the ideal canvas padding/spacing between sprites in a sheet?
"""
    },
    {
        "baslik": "BOLUM 03 — Dark Fantasy Pixel Art: Renk Paleti ve Işık Teorisi",
        "prompt": """
You are an expert in pixel art color theory and dark fantasy game aesthetics.

Research: How do successful dark fantasy pixel art games (Children of Morta, Dead Cells, Darkest Dungeon,
Hollow Knight) handle their color palettes?

Cover:
1. How many colors total in a typical dark fantasy pixel art palette?
2. Exact hue/saturation/value principles for:
   - Dark environment backgrounds
   - Player character (must contrast against background)
   - Enemy characters
   - Skill/magic effects (must be brightest)
   - UI elements
3. The "value contrast" rule — how many value steps between character and background?
4. Rim lighting in pixel art — how to simulate it
5. Color coding for danger/safety in roguelites
6. How to make a small (48×48px) character look "detailed" and "premium"

Give SPECIFIC color hex values or HSV ranges for a plague-doctor / medieval dark fantasy aesthetic.
Reference: Children of Morta color palette, Enter the Gungeon palette.
"""
    },
    {
        "baslik": "BOLUM 04 — VFX ve Efekt Animasyonu Pixel Art'ta",
        "prompt": """
You are an expert in pixel art visual effects for 2D action games.

Research: How are VFX animations made in top-down 2D pixel art games?

Cover:
1. Standard frame counts for common VFX:
   - Hit spark / impact flash
   - Slash trail
   - Explosion / area blast
   - Magic projectile
   - Death burst/dissolve
   - Aura/power-up effect
   - Status effect (burning, frozen, poisoned)

2. The "smear frame" technique — what is it, when to use it in pixel art

3. How to make VFX readable at small sizes (32×32px or 48×48px)

4. The "anticipation flash" technique (brief white flash before impact)

5. Sprite-based vs particle-based VFX — when to use each in Unity 2D

6. How Children of Morta and Dead Cells handle VFX timing and style

7. Canvas sizes for different VFX types:
   - Small hit effect
   - Medium skill effect
   - Large area/ultimate effect
"""
    },
    {
        "baslik": "BOLUM 05 — AI Görsel Üretim Araçları: Pixel Art için Kullanım",
        "prompt": """
You are an expert in AI-assisted pixel art creation workflows for indie game development.

Research: How can AI image generation tools be used effectively for pixel art game assets?

Cover:

1. STABLE DIFFUSION / AUTOMATIC1111:
   - Best models for pixel art generation (which checkpoints?)
   - Key prompts / negative prompts for pixel art
   - Settings (steps, CFG, sampler)
   - Top-down 2D character generation tips

2. MIDJOURNEY:
   - Best prompts for pixel art characters
   - Style parameters (--style, --stylize)
   - How to get consistent character style across multiple generations

3. DALLE-3 / BING IMAGE CREATOR:
   - Effectiveness for pixel art
   - Prompt format

4. USING AI ART AS REFERENCE (not direct use):
   - Generate concept art in realistic style → use as reference to draw in Aseprite
   - Generate color palette references
   - Generate environment mood boards

5. AI → PIXEL ART CONVERSION:
   - Tools that convert regular images to pixel art
   - Aseprite's built-in pixelate options

6. Common mistakes when using AI for game asset generation

Include specific prompt examples that work well for:
- Top-down 2D dark fantasy character
- Dungeon environment tiles
- Skill effect icons
"""
    },
    {
        "baslik": "BOLUM 06 — Skeleton-Based Pixel Art Animasyonu: Araçlar ve Workflow",
        "prompt": """
You are an expert in pixel art animation tools for indie game developers.

Research: What are the best skeleton/bone-based animation tools for pixel art?

For each tool, cover:
- How it works (is it truly skeleton-based or frame-by-frame?)
- Aseprite integration (does it work inside Aseprite?)
- Learning curve for a non-artist
- Output format (spritesheet, individual frames?)
- Cost
- Best use case

Tools to cover:
1. Spine (Esoteric Software)
2. DragonBones
3. Spriter Pro
4. Creature2D
5. Unity's built-in 2D Animation with PSD Importer
6. PixelLab (pixellab.ai Aseprite extension) — if you know it
7. Any other relevant tools

Also:
- What is the realistic workflow for a solo dev with no art experience to create animated pixel art characters?
- Is it faster to do frame-by-frame in Aseprite or use skeleton tools?
- What animations SHOULD be frame-by-frame vs skeleton-based?
"""
    },
    {
        "baslik": "BOLUM 07 — Enter the Gungeon ve Children of Morta Görsel Analizi",
        "prompt": """
You are an expert pixel art game analyst. Analyze the visual/art style of these reference games:

1. ENTER THE GUNGEON:
   - Sprite size (approximate pixel dimensions for characters)
   - Animation frame counts (idle, walk, shoot, death)
   - Color palette approach
   - How dungeon tiles are structured (16x16? 32x32?)
   - VFX style
   - What makes it look "premium" despite small sprites

2. CHILDREN OF MORTA:
   - Sprite size (much larger than ETG)
   - Animation frame counts
   - The URP-style lighting effect on sprites
   - How normal maps are used
   - Color palette
   - What makes the characters feel "alive"

3. HADES:
   - Character scale vs screen
   - How they achieve the isometric-but-not-really look
   - Animation philosophy (few frames but impactful)

4. DEAD CELLS:
   - Skeleton/bone animation in pixel art context
   - How they mix procedural and frame-by-frame

For our game specifically (flat top-down 2D, 48×48px characters, dark plague medieval):
Which of these references is MOST applicable and why?
What specific techniques should we copy from each?
"""
    },
    {
        "baslik": "BOLUM 08 — Normal Map Pixel Art'ta: Uygulama ve Araçlar",
        "prompt": """
You are an expert in 2D game lighting with Unity URP.

Research: How are normal maps used with 2D pixel art sprites?

Cover:
1. What is a normal map and why does it matter for pixel art (briefly)

2. Tools for generating normal maps from pixel art sprites:
   - Laigter (free) — how it works, quality assessment
   - SpriteIlluminator
   - Aseprite Normal Map plugin (if any)
   - Sprite Lamp
   - Manual creation in Photoshop/GIMP

3. Unity URP 2D Lighting setup for pixel art:
   - Which shader? (Sprite-Lit-Default)
   - Global Light 2D settings for dark dungeon atmosphere
   - Point Light 2D for player torch effect
   - How to assign normal map to sprite in Unity (Secondary Textures)
   - PPU consistency requirement

4. Performance considerations:
   - How many lights can you have before performance drops?
   - Mobile vs PC difference
   - Which sprites NEED normal maps vs which can skip

5. What does Children of Morta's lighting setup look like technically?

6. Common mistakes when setting up 2D URP lighting:
   - Wrong PPU causing misaligned shadows
   - HDR vs LDR
   - Blend mode issues
"""
    },
]

# ─── ÇALIŞMA ──────────────────────────────────────────────────────────────────

def ollama_sor(prompt: str) -> str:
    payload = {
        "model": MODEL,
        "prompt": FORMAT_ONEKI + prompt,
        "stream": False,
        "options": GPU_OPTIONS,
    }
    try:
        r = requests.post(OLLAMA_URL, json=payload, timeout=1200)
        r.raise_for_status()
        return r.json().get("response", "").strip()
    except Exception as e:
        return f"HATA: {e}"


def ham_veri_cikar(metin: str) -> str:
    if "### PART 2" in metin:
        return metin.split("### PART 2")[0].strip()
    return metin


def main():
    toplam = len(BOLUMLER)
    print("\n" + "="*65)
    print(f"  Model: {MODEL} | Bölüm: {toplam}")
    print(f"  Tahmini süre: {toplam*5}–{toplam*10} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    os.makedirs(os.path.dirname(CIKTI_DOSYASI), exist_ok=True)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Pixel Art & Animasyon Araştırması\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Ham Veri — Pixel Art\n*{datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

    basari = 0
    for i, bolum in enumerate(BOLUMLER, 1):
        print(f"\n[{i:02d}/{toplam}] {datetime.now().strftime('%H:%M')} — {bolum['baslik'][:60]}")
        print(f"         Düşünüyor...", end="", flush=True)

        cikti = ollama_sor(bolum["prompt"])

        if cikti.startswith("HATA"):
            print(f" ✗\n         {cikti}")
        else:
            print(f" ✓  ({len(cikti):,} karakter)")
            basari += 1

        with open(CIKTI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{cikti}\n\n---\n\n")

        with open(HAM_VERI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{ham_veri_cikar(cikti)}\n\n---\n\n")

    print(f"\n{'='*65}")
    print(f"  TAMAMLANDI: {basari}/{toplam} | {datetime.now().strftime('%H:%M')}")
    print(f"  Çıktı: {CIKTI_DOSYASI}")
    print("="*65)


if __name__ == "__main__":
    main()
