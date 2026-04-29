#!/usr/bin/env python3
"""
Regenerate extreme poses with lower IP-Adapter scale (0.25-0.30).
Fixes: windup, impact, stagger, dead, dash_peak
Lower scale = text prompt has more control over body pose.

Usage: conda activate stable-audio && python Tools/regen_extreme_poses.py
"""
import torch
from diffusers import StableDiffusionPipeline
from PIL import Image
import os

ANCHOR_PATH = r"F:\Antigravity Projeler\2d roguelite\RIMA\TASARIM\CLASS_CONCEPTS\PixelLab_Refs_128\warblade1.png"
OUTPUT_DIR  = r"F:\Antigravity Projeler\2d roguelite\RIMA\_STAGING\warblade_poses_v2"
MODEL_ID    = "dreamlike-art/dreamlike-diffusion-1.0"

STYLE = (
    "dark fantasy heavy armored warrior, black iron plate armor, long black cape, "
    "massive greatsword with cold blue energy cracks, isometric view, game sprite art"
)
NEG = (
    "ugly, blurry, bad anatomy, deformed, standing, idle, resting, "
    "extra limbs, 3D render, photorealistic, chibi, cartoon, text, watermark"
)

EXTREME_POSES = [
    {
        "name": "windup_S",
        "prompt": (
            f"{STYLE}, SWORD RAISED HIGH OVERHEAD with both hands stretched up, "
            "arms reaching toward sky, body fully arched backward, extreme overhead raise, "
            "weight on back foot, dramatic coiling before downward slam"
        ),
        "ip_scale": 0.28,
        "cfg": 10.0,
        "steps": 35,
        "seed": 777,
        "neg_extra": "sword low, sword horizontal, sword at side",
    },
    {
        "name": "impact_S",
        "prompt": (
            f"{STYLE}, BOTH ARMS SLAMMED FULLY DOWNWARD, sword tip pointing at ground, "
            "body bent completely forward at waist, knees slightly bent absorbing impact, "
            "weight on front foot, explosive downward strike complete"
        ),
        "ip_scale": 0.28,
        "cfg": 10.0,
        "steps": 35,
        "seed": 888,
        "neg_extra": "sword up, arms up, standing straight",
    },
    {
        "name": "stagger_S",
        "prompt": (
            f"{STYLE}, STAGGERING BACKWARD violently from massive hit, "
            "torso lurching backward, both knees buckling and bending, "
            "sword arm swinging away, head thrown back, losing footing"
        ),
        "ip_scale": 0.30,
        "cfg": 9.5,
        "steps": 35,
        "seed": 999,
        "neg_extra": "standing, balanced, upright",
    },
    {
        "name": "dead_S",
        "prompt": (
            f"{STYLE}, LYING FLAT FACE-DOWN on ground, body fully horizontal, "
            "arms spread out beside body on floor, legs flat, "
            "greatsword dropped and lying on ground beside the body, "
            "death pose, completely collapsed, motionless"
        ),
        "ip_scale": 0.22,
        "cfg": 11.0,
        "steps": 40,
        "seed": 12345,
        "neg_extra": "standing, sitting, kneeling, upright",
    },
    {
        "name": "dash_peak_S",
        "prompt": (
            f"{STYLE}, BODY NEARLY HORIZONTAL mid-air leap forward, "
            "both feet off ground, legs fully extended behind, "
            "sword trailing behind the body, extreme forward lean, "
            "horizontal flying lunge at maximum speed"
        ),
        "ip_scale": 0.30,
        "cfg": 9.5,
        "steps": 35,
        "seed": 2024,
        "neg_extra": "standing, walking, upright",
    },
]

def load_anchor(path):
    img = Image.open(path).convert("RGBA")
    bg = Image.new("RGB", img.size, (255, 255, 255))
    bg.paste(img, mask=img.split()[3])
    return bg.resize((512, 512), Image.LANCZOS)

def main():
    print("Extreme Pose Regenerator")
    print(f"Model: {MODEL_ID}")
    print(f"Poses: {[p['name'] for p in EXTREME_POSES]}\n")

    pipe = StableDiffusionPipeline.from_pretrained(
        MODEL_ID,
        torch_dtype=torch.float16,
        safety_checker=None,
        requires_safety_checker=False,
    ).to("cuda")

    pipe.load_ip_adapter("h94/IP-Adapter", subfolder="models", weight_name="ip-adapter_sd15.bin")
    print("Pipeline + IP-Adapter ready.\n")

    anchor = load_anchor(ANCHOR_PATH)
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    for i, pose in enumerate(EXTREME_POSES, 1):
        name     = pose["name"]
        prompt   = pose["prompt"]
        scale    = pose["ip_scale"]
        cfg      = pose["cfg"]
        steps    = pose["steps"]
        seed     = pose["seed"]
        neg_full = NEG + ", " + pose["neg_extra"]

        print(f"[{i}/{len(EXTREME_POSES)}] {name}  ip_scale={scale}  cfg={cfg}")
        pipe.set_ip_adapter_scale(scale)

        result = pipe(
            prompt=prompt,
            ip_adapter_image=anchor,
            num_inference_steps=steps,
            guidance_scale=cfg,
            negative_prompt=neg_full,
            generator=torch.Generator("cuda").manual_seed(seed),
            width=512,
            height=512,
        ).images[0]

        out = os.path.join(OUTPUT_DIR, f"{name}.png")
        result.save(out)
        print(f"  -> {out}")

    print("\nDone! Extreme poses regenerated in warblade_poses_v2/")

if __name__ == "__main__":
    main()
