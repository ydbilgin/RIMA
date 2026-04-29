#!/usr/bin/env python3
"""
Pixel Art Prompt Optimizasyonu — 2D Roguelite
==============================================
Araştırılan: AI araçları için doğru prompt formatı,
PixelLab benzeri araçlar, top-down 2D karakter üretimi
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

CIKTI_DOSYASI    = r"F:\Antigravity Projeler\2d roguelite\ART\PROMPT_ARASTIRMA_CIKTISI.md"
HAM_VERI_DOSYASI = r"F:\Antigravity Projeler\2d roguelite\ART\PROMPT_ARASTIRMA_HAM.md"

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
Concrete facts, examples, keyword lists in TABLE or BULLET format.

### PART 2 — ANALYSIS
Practical recommendations for our specific use case.

Now answer:
---
"""

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — AI Pixel Art Üretimi: Doğru Prompt Anatomisi",
        "prompt": """
You are an expert in AI image generation for pixel art game assets.

Research: What is the correct anatomy of a prompt for generating pixel art sprites using
AI image generation tools (Stable Diffusion, DALL-E, Midjourney, or similar tools)?

Give a detailed breakdown of:

1. PROMPT STRUCTURE — in what ORDER should elements appear?
   e.g.: [art style] [subject] [details] [perspective] [background] [quality tags]
   Give the optimal order with explanation of WHY this order.

2. ESSENTIAL KEYWORDS for pixel art:
   - Which keywords tell the AI "this must be pixel art"?
   - Which keywords help with top-down 2D perspective?
   - Which keywords improve character readability/silhouette?
   - Which keywords help with transparent backgrounds?
   - Which keywords improve consistency across multiple generations?

3. NEGATIVE PROMPT — complete list for pixel art:
   What must ALWAYS be in negative prompt when generating pixel art?
   Anti-aliasing, gradients, etc. — give full list with explanation.

4. WHAT NOT TO PUT IN PROMPT:
   - Pixel dimensions (e.g., "48x48") — does this help or hurt?
   - File format references ("PNG", "sprite sheet") — useful or not?
   - Technical Unity terms — should you mention Unity?

5. EXAMPLES — show 3 complete prompts (positive + negative) for:
   a) A warrior character, top-down 2D, dark fantasy
   b) A dungeon floor tile, top-down view, seamless
   c) A magic explosion VFX effect, top-down
"""
    },
    {
        "baslik": "BOLUM 02 — Top-Down 2D Karakter: Prompt Özel Teknikleri",
        "prompt": """
You are an expert pixel art game artist who uses AI tools extensively.

Research: What specific prompt techniques work best for generating top-down 2D game characters?

Cover:

1. PERSPECTIVE KEYWORDS:
   What exact words/phrases tell AI to generate a top-down perspective?
   "top-down", "bird's eye", "overhead view", "from above" — which work best?
   What about "front-facing top-down" (like Enter the Gungeon style)?

2. CHARACTER SILHOUETTE:
   Which prompt elements improve the silhouette clarity of small pixel art sprites?
   How do you prevent the character from blending into the background?

3. STYLE CONSISTENCY across multiple generations:
   If you need 8 different characters in the same art style,
   what prompt elements must stay IDENTICAL across all generations?
   What elements must CHANGE for each character?

4. DARK FANTASY / PLAGUE MEDIEVAL aesthetic keywords:
   Give a keyword list that produces a dark, plague-era, medieval fantasy look.
   Reference games: Children of Morta, Enter the Gungeon, Darkest Dungeon.

5. SPECIFIC CHARACTER TYPES — give complete prompts for:
   a) Heavy armored warrior (top-down, dark fantasy, pixel art)
   b) Plague doctor with mask and vials (pixel art, dark fantasy)
   c) Hooded assassin with daggers (pixel art, dark fantasy)
   d) Summoner necromancer with floating spirits (pixel art, dark fantasy)
   e) Holy paladin with divine light (pixel art, dark fantasy)

6. VFX / EFFECTS prompts:
   How do you prompt for skill effects (explosions, auras, trails)?
   Different from character prompting?

Include both the positive and negative prompts for each example.
"""
    },
    {
        "baslik": "BOLUM 03 — Gemini / Imagen için Prompt Optimizasyonu",
        "prompt": """
You are an expert in Google's Imagen image generation system and Gemini AI.

Research: How should prompts be written specifically for Google Imagen 3 (used in Gemini)?

Compare with Stable Diffusion / Midjourney prompting:

1. IMAGEN vs STABLE DIFFUSION prompt differences:
   - Imagen responds better to: [natural language? keyword lists? both?]
   - What SD techniques DON'T work well in Imagen?
   - What Imagen-specific techniques exist?

2. IMAGEN STRENGTHS for game art:
   - What types of images does Imagen excel at?
   - What should you use Imagen for vs other tools?

3. OPTIMAL PROMPT FORMAT for Imagen/Gemini:
   - Length: short vs long prompts?
   - Style: natural sentences vs comma-separated keywords?
   - Should you describe lighting, mood, style separately?

4. CONCEPT ART generation for game characters:
   Give 3 optimized Imagen prompts for:
   a) Game character concept sheet (front view, dark fantasy warrior)
   b) Environment mood board (dungeon, dark medieval)
   c) VFX reference (magic explosion effect, dark purple)

5. USING IMAGEN OUTPUT as PIXEL ART REFERENCE:
   What style of Imagen output works best as reference for pixel art tracing?
   "Semi-realistic", "concept art", "illustration" — which is easiest to pixel-art-ize?

6. CONSISTENCY between multiple Imagen generations:
   If you generate 8 characters, how do you keep the same art style?
   Any Imagen-specific tricks for style consistency?
"""
    },
    {
        "baslik": "BOLUM 04 — Referans Görselden Pixel Art: Workflow Analizi",
        "prompt": """
You are an expert in pixel art creation workflow for solo indie game developers.

Research: What is the most efficient workflow for converting AI-generated reference images to pixel art?

Cover:

1. REFERENCE IMAGE → PIXEL ART CONVERSION METHODS:
   a) Manual tracing in Aseprite (trace by hand)
   b) Automatic pixelation (Aseprite's built-in, or plugins)
   c) AI-to-pixel-art tools (what exists? how good are they?)
   d) Hybrid: auto-convert then manually fix

   For each method: speed, quality, skill required, recommended for solo dev?

2. ASEPRITE MANUAL TRACING TECHNIQUE:
   Step by step: how do you efficiently trace a reference image in Aseprite?
   - Import reference image as layer
   - Reduce opacity
   - Trace on new layer
   - Any tips to make this faster?

3. ASEPRITE BUILT-IN PIXELATION:
   Does Aseprite have tools to auto-pixelate an imported image?
   Image → Resize with "Nearest Neighbor" — does this work for game sprites?
   What are the limitations?

4. MAINTAINING STYLE CONSISTENCY:
   When you have multiple characters all traced from AI references,
   how do you ensure they look like they belong to the same game?
   Color palette locking? Grid system? Template layer?

5. COLOR PALETTE EXTRACTION:
   How do you extract a consistent color palette from multiple reference images?
   Can Aseprite help with this?

6. RECOMMENDED WORKFLOW for our game (8 classes, 1 solo dev):
   Given the constraints (no art experience, must make 8 characters),
   what is the most efficient specific workflow? Step by step.
"""
    },
]

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
        f.write(f"# Prompt Optimizasyon Araştırması\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Ham Veri — Prompt\n*{datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

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
    print("="*65)

if __name__ == "__main__":
    main()
