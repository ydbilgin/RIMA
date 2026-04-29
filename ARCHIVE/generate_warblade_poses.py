#!/usr/bin/env python3
"""
Warblade Keyframe Pose Generator
Generates 10 keyframe poses from warblade1.png anchor using img2img.
Uses SD1.5 model - no pixel art conversion here; PixelLab does that step.

Usage:
  conda activate stable-audio
  python Tools/generate_warblade_poses.py

Model download: ~2GB (first run only)
Output: _STAGING/warblade_poses/
"""

import torch
from diffusers import StableDiffusionImg2ImgPipeline
from PIL import Image, ImageFilter
import os

ANCHOR_PATH = r"F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\warblade1.png"
OUTPUT_DIR  = r"F:\Antigravity Projeler\2d roguelite\RIMA\_STAGING\warblade_poses"
MODEL_ID    = "dreamlike-art/dreamlike-diffusion-1.0"

# Every pose includes the style anchor as a visual grounding phrase.
BASE_STYLE = (
    "dark fantasy warrior, heavy plate armor, black steel cape, greatsword, "
    "isometric 35-degree angle, game sprite art"
)
NEG = (
    "ugly, blurry, low quality, deformed, disfigured, extra limbs, "
    "3D render, photorealistic, photo, text, watermark"
)

POSES = [
    {
        "name": "neutral_base_S",
        "prompt": f"{BASE_STYLE}, both hands gripped low in front, neutral idle ready stance, "
                  "sword held vertically, weight balanced, facing south",
        "strength": 0.50,
    },
    {
        "name": "breath_S",
        "prompt": f"{BASE_STYLE}, standing still, very subtle chest rise from a slow breath, "
                  "armor micro-shift upward, almost imperceptible, facing south",
        "strength": 0.30,
    },
    {
        "name": "runA_S",
        "prompt": f"{BASE_STYLE}, mid-sprint left leg fully forward, right arm swinging, "
                  "body leaning forward, heavy armor, powerful stride, facing south",
        "strength": 0.65,
    },
    {
        "name": "runB_S",
        "prompt": f"{BASE_STYLE}, mid-sprint right leg fully forward, left arm swinging, "
                  "body leaning forward, heavy armor, opposite stride mirror, facing south",
        "strength": 0.65,
    },
    {
        "name": "windup_S",
        "prompt": f"{BASE_STYLE}, sword raised high above head with both hands gripping hilt, "
                  "body coiling, weight shifting to back foot, overhead slam windup, facing south",
        "strength": 0.72,
    },
    {
        "name": "impact_S",
        "prompt": f"{BASE_STYLE}, sword at lowest point full impact, arms fully extended downward, "
                  "body bent forward, weight on front foot, explosive overhead slam, facing south",
        "strength": 0.72,
    },
    {
        "name": "dash_launch_S",
        "prompt": f"{BASE_STYLE}, pushing off explosively into dash, torso dropping forward, "
                  "rear leg driving off ground, sword trailing behind, launch pose, facing south",
        "strength": 0.70,
    },
    {
        "name": "dash_peak_S",
        "prompt": f"{BASE_STYLE}, maximum forward dash lean, body almost horizontal, "
                  "sword trailing behind, legs fully extended, full sprint commit, facing south",
        "strength": 0.72,
    },
    {
        "name": "stagger_S",
        "prompt": f"{BASE_STYLE}, staggering backward from fatal blow, knees buckling, "
                  "head dropping, sword slipping from grip, hit reaction pose, facing south",
        "strength": 0.72,
    },
    {
        "name": "dead_S",
        "prompt": f"{BASE_STYLE}, collapsed face-down on ground, sword dropped to side, "
                  "armor dented, lifeless body, death pose, facing south",
        "strength": 0.78,
    },
]

def main():
    print("=" * 60)
    print("Warblade Pose Generator")
    print("=" * 60)
    print(f"Model  : {MODEL_ID}  (~2GB first download)")
    print(f"Input  : {ANCHOR_PATH}")
    print(f"Output : {OUTPUT_DIR}")
    print("=" * 60)

    if not os.path.exists(ANCHOR_PATH):
        print(f"ERROR: Anchor not found: {ANCHOR_PATH}")
        return

    print("\nLoading pipeline...")
    pipe = StableDiffusionImg2ImgPipeline.from_pretrained(
        MODEL_ID,
        torch_dtype=torch.float16,
        safety_checker=None,
        requires_safety_checker=False,
    )
    pipe = pipe.to("cuda")
    pipe.enable_attention_slicing()
    print("Pipeline ready.\n")

    # Load anchor - scale up to 512 for SD1.5, keep aspect
    anchor_raw = Image.open(ANCHOR_PATH).convert("RGBA")
    # Compose onto white bg (depth extraction works better)
    bg = Image.new("RGB", anchor_raw.size, (255, 255, 255))
    bg.paste(anchor_raw, mask=anchor_raw.split()[3])
    anchor_512 = bg.resize((512, 512), Image.LANCZOS)
    # Optional mild denoise to help model read shape better
    anchor_512 = anchor_512.filter(ImageFilter.SMOOTH_MORE)

    os.makedirs(OUTPUT_DIR, exist_ok=True)

    for i, pose in enumerate(POSES, 1):
        name    = pose["name"]
        prompt  = pose["prompt"]
        strength = pose["strength"]

        print(f"[{i:02d}/{len(POSES)}] {name}  (strength={strength})")

        result = pipe(
            prompt=prompt,
            image=anchor_512,
            strength=strength,
            num_inference_steps=28,
            guidance_scale=7.5,
            negative_prompt=NEG,
            generator=torch.Generator("cuda").manual_seed(42),
        ).images[0]

        out_path = os.path.join(OUTPUT_DIR, f"{name}.png")
        result.save(out_path)
        print(f"  -> {out_path}")

    print("\n" + "=" * 60)
    print(f"Done! {len(POSES)} poses saved to:")
    print(f"  {OUTPUT_DIR}")
    print("""
NEXT STEPS (PixelLab site):
  For each pose PNG:
  1. pixellab.ai → Image to Image (Depth)
  2. Upload pose PNG as init image
  3. Upload warblade1.png as style reference
  4. See WARBLADE_PIXELLAB_PROMPTS.md for prompts + settings
""")

if __name__ == "__main__":
    main()
