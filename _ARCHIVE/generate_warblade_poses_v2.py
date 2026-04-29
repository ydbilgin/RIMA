#!/usr/bin/env python3
"""
Warblade Pose Generator v2 — IP-Adapter Edition
IP-Adapter keeps character visually locked to warblade1.png across all poses.
Text prompt drives the pose; IP-Adapter drives character identity.

Usage:
  conda activate stable-audio
  python Tools/generate_warblade_poses_v2.py

First-run downloads:
  - dreamlike-art/dreamlike-diffusion-1.0  (already cached ~2GB)
  - h94/IP-Adapter weight  (~300MB)
  - openai/clip-vit-large-patch14  (~1.7GB, image encoder for IP-Adapter)

Output: _STAGING/warblade_poses_v2/
"""

import torch
from diffusers import StableDiffusionPipeline
from diffusers.utils import load_image
from PIL import Image
import os

ANCHOR_PATH = r"F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\warblade1.png"
OUTPUT_DIR  = r"F:\Antigravity Projeler\2d roguelite\RIMA\_STAGING\warblade_poses_v2"
MODEL_ID    = "dreamlike-art/dreamlike-diffusion-1.0"

BASE_STYLE = (
    "dark fantasy heavy armored warrior, black iron plate armor, long black cape, "
    "massive greatsword with cold blue glowing energy cracks, "
    "isometric 35-degree top-down view, game sprite art"
)
NEG = (
    "ugly, blurry, low quality, bad anatomy, deformed, disfigured, extra limbs, "
    "3D render, photorealistic, chibi, cartoon, cute, western comic, sketch, "
    "missing limbs, text, watermark, signature"
)

POSES = [
    {
        "name": "neutral_base_S",
        "prompt": f"{BASE_STYLE}, standing in neutral ready stance, both hands gripping sword hilt low in front, "
                  "weight balanced, facing viewer, calm and ready",
        "ip_scale": 0.65,
        "cfg": 7.5,
        "steps": 30,
        "seed": 42,
    },
    {
        "name": "breath_S",
        "prompt": f"{BASE_STYLE}, standing still, natural resting pose, "
                  "slight chest rise from calm breath, subtle relaxed posture",
        "ip_scale": 0.70,
        "cfg": 7.0,
        "steps": 28,
        "seed": 42,
    },
    {
        "name": "runA_S",
        "prompt": f"{BASE_STYLE}, running at full sprint, left leg fully forward stride, "
                  "right arm pumping backward, body leaning aggressively into run, "
                  "powerful forward momentum, cape flowing behind",
        "ip_scale": 0.60,
        "cfg": 8.0,
        "steps": 30,
        "seed": 123,
    },
    {
        "name": "runB_S",
        "prompt": f"{BASE_STYLE}, running at full sprint, right leg fully forward stride, "
                  "left arm pumping backward, body leaning into run, "
                  "opposite gait from previous, cape flowing",
        "ip_scale": 0.60,
        "cfg": 8.0,
        "steps": 30,
        "seed": 456,
    },
    {
        "name": "windup_S",
        "prompt": f"{BASE_STYLE}, winding up massive overhead slam, "
                  "both hands gripping sword raised HIGH above head, "
                  "body coiling backward, weight on back foot, "
                  "extreme overhead raise before downward strike, dramatic anticipation pose",
        "ip_scale": 0.58,
        "cfg": 8.5,
        "steps": 32,
        "seed": 42,
    },
    {
        "name": "impact_S",
        "prompt": f"{BASE_STYLE}, sword slammed fully downward at lowest impact point, "
                  "both arms fully extended downward, body bent forward at waist, "
                  "weight shifted completely to front foot, "
                  "cold blue rift energy exploding from blade on ground impact",
        "ip_scale": 0.58,
        "cfg": 8.5,
        "steps": 32,
        "seed": 42,
    },
    {
        "name": "dash_launch_S",
        "prompt": f"{BASE_STYLE}, explosive dash launch pose, "
                  "torso dropped sharply forward, rear leg driving powerfully off ground, "
                  "sword trailing behind, body low and surging forward, "
                  "explosive acceleration from standstill",
        "ip_scale": 0.60,
        "cfg": 8.0,
        "steps": 30,
        "seed": 42,
    },
    {
        "name": "dash_peak_S",
        "prompt": f"{BASE_STYLE}, maximum forward dash lean, body nearly horizontal, "
                  "legs fully extended and off ground, sword trailing far behind, "
                  "committed full dash, air-borne lunge",
        "ip_scale": 0.60,
        "cfg": 8.0,
        "steps": 30,
        "seed": 42,
    },
    {
        "name": "stagger_S",
        "prompt": f"{BASE_STYLE}, hit stagger reaction, lurching backward from severe blow, "
                  "knees buckling, head dropping, sword grip loosening, "
                  "losing balance from critical hit",
        "ip_scale": 0.63,
        "cfg": 7.5,
        "steps": 30,
        "seed": 42,
    },
    {
        "name": "dead_S",
        "prompt": f"{BASE_STYLE}, collapsed completely on ground face-down, "
                  "body sprawled forward, sword fallen beside body, "
                  "armor dented and cracked, utterly defeated death pose, "
                  "lying still on dungeon floor",
        "ip_scale": 0.65,
        "cfg": 7.5,
        "steps": 30,
        "seed": 42,
    },
]


def load_anchor_image(path: str) -> Image.Image:
    img = Image.open(path).convert("RGBA")
    bg = Image.new("RGB", img.size, (255, 255, 255))
    bg.paste(img, mask=img.split()[3])
    return bg.resize((512, 512), Image.LANCZOS)


def main():
    print("=" * 60)
    print("Warblade Pose Generator v2  (IP-Adapter)")
    print("=" * 60)
    print(f"Model  : {MODEL_ID}")
    print(f"Input  : {ANCHOR_PATH}")
    print(f"Output : {OUTPUT_DIR}")
    print("Downloads on first run: IP-Adapter (~300MB) + CLIP encoder (~1.7GB)")
    print("=" * 60)

    if not os.path.exists(ANCHOR_PATH):
        print(f"ERROR: Anchor not found: {ANCHOR_PATH}")
        return

    print("\nLoading base pipeline...")
    pipe = StableDiffusionPipeline.from_pretrained(
        MODEL_ID,
        torch_dtype=torch.float16,
        safety_checker=None,
        requires_safety_checker=False,
    ).to("cuda")
    print("Loading IP-Adapter weights (downloads ~2GB on first run)...")
    pipe.load_ip_adapter(
        "h94/IP-Adapter",
        subfolder="models",
        weight_name="ip-adapter_sd15.bin",
    )
    print("Pipeline ready.\n")

    anchor_image = load_anchor_image(ANCHOR_PATH)
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    for i, pose in enumerate(POSES, 1):
        name    = pose["name"]
        prompt  = pose["prompt"]
        scale   = pose["ip_scale"]
        cfg     = pose["cfg"]
        steps   = pose["steps"]
        seed    = pose["seed"]

        print(f"[{i:02d}/{len(POSES)}] {name}  (ip_scale={scale})")
        pipe.set_ip_adapter_scale(scale)

        result = pipe(
            prompt=prompt,
            ip_adapter_image=anchor_image,
            num_inference_steps=steps,
            guidance_scale=cfg,
            negative_prompt=NEG,
            generator=torch.Generator("cuda").manual_seed(seed),
            width=512,
            height=512,
        ).images[0]

        out_path = os.path.join(OUTPUT_DIR, f"{name}.png")
        result.save(out_path)
        print(f"  -> {out_path}")

    print("\n" + "=" * 60)
    print(f"DONE — {len(POSES)} poses saved to:")
    print(f"  {OUTPUT_DIR}")
    print("""
NEXT: PixelLab site
  Tool: Image to Image (Depth)
  Depth image:  each pose PNG from this folder
  Style image:  warblade1.png
  Output size:  128x128
  Depth strength: 0.70
  Guidance weight: 80
  Direction: South
  Prompt: same character, transparent background
""")


if __name__ == "__main__":
    main()
