#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
Generate 10 class south masters as pixel-art-friendly outputs via ComfyUI FLUX Kontext.

Workflow:
1) Load locked south reference image per class
2) Run FLUX Kontext img2img with pixel-art-constrained prompt
3) Save Comfy output (512x512)
4) Resize to 128x128 with nearest-neighbor

Usage:
  python Tools/comfy_pixelart_south_batch.py
  python Tools/comfy_pixelart_south_batch.py --seed 42 --steps 28
"""

from __future__ import annotations

import argparse
import json
import shutil
import time
import urllib.request
from pathlib import Path

from PIL import Image

COMFY_URL = "http://127.0.0.1:8188"
COMFY_INPUT_DIR = Path("F:/AI/ComfyUI/input")
OUTPUT_ROOT = Path(
    "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_master"
)
REF_ROOT = Path(
    "F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new"
)
REF_ROOT_ALT = REF_ROOT / "new"

CLASS_SPECS = {
    "warblade": "male warblade, no helmet, practical partial armor, dark crimson battle wrap, heavy greatsword at low rest, cold blue hairline cracks only on blade",
    "brawler": "male brawler, muscular build, torn green vest, bare arms, reinforced gauntlets with subtle void-purple glow, fighter stance",
    "elementalist": "female elementalist, practical blue-purple robe with modest open neckline, flowing cloak, one raised electric orb, no staff",
    "gunslinger": "female gunslinger, rift-tech duelist, dual pistols, long copper-orange hair, fitted combat jacket, kinetic but readable pose",
    "hexer": "female hexer, ragged dark-purple robe, hunched silhouette, cursed green flame accent, staff with subtle void-purple skull motif",
    "ranger": "female ranger, charcoal-slate tactical hunter, hooded leather, tether spool and trap canisters, longbow with single nocked arrow",
    "ravager": "male ravager, massive barbarian plate silhouette, blood-red rage accent, single huge double-headed axe, no helmet",
    "ronin": "male ronin, layered blue-green worn robes, topknot, right katana forward and left at guard hip, clean overhead silhouette",
    "shadowblade": "male shadowblade, black-purple suit with grey armor plates, lower-face shadow wrap, dual blades with subtle void-purple smoke trails",
    "summoner": "female summoner, ornate purple-gold practical commander robe, one hand with scepter, other hand open control pose, no giant runic circle",
}

BASE_POSITIVE = (
    "pixel art game sprite, high top-down view, steep overhead camera, full body readable from above, "
    "top of head visible, torso clearly readable, mature adult proportions, long legs, normal head-to-body ratio, "
    "not short, not squat, clean readable silhouette, limited palette, crisp edges, single character centered, "
    "plain neutral backdrop, no floor texture, no checkerboard pattern"
)

NEGATIVE = (
    "photo, realistic, painterly, 3d render, smooth shading, blurry, noisy texture, "
    "isometric, side view portrait, extreme perspective, chibi, dwarf-like, short legs, squat body, "
    "full plate knight helmet, oversized head, multiple characters, watermark, logo, text"
)


def comfy_get(url: str) -> dict:
    with urllib.request.urlopen(url) as r:
        return json.loads(r.read().decode("utf-8"))


def comfy_post(url: str, payload: dict) -> dict:
    data = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(url, data=data, headers={"Content-Type": "application/json"})
    with urllib.request.urlopen(req) as r:
        return json.loads(r.read().decode("utf-8"))


def ensure_comfy_alive() -> None:
    stats = comfy_get(f"{COMFY_URL}/system_stats")
    if "system" not in stats:
        raise RuntimeError("ComfyUI API is not returning system stats.")


def pick_reference_path(cls: str) -> Path:
    candidates = [
        REF_ROOT_ALT / f"{cls}_south_lock1.png",
        REF_ROOT_ALT / f"{cls}_south_lock.png",
        REF_ROOT / f"{cls}_south_lock.png",
        REF_ROOT_ALT / f"{cls}_south.png",
        REF_ROOT_ALT / f"{cls}.png",
    ]
    for c in candidates:
        if c.exists():
            return c
    raise FileNotFoundError(f"No reference found for class={cls}")


def preprocess_reference(src: Path, dst: Path) -> None:
    """
    Flatten alpha/checker-like assets onto neutral gray and normalize canvas.
    """
    with Image.open(src).convert("RGBA") as im:
        base = Image.new("RGBA", im.size, (188, 188, 188, 255))
        base.alpha_composite(im)
        # Normalize to 512 canvas while preserving silhouette scale.
        target = Image.new("RGBA", (512, 512), (188, 188, 188, 255))
        scaled = base.resize((384, 384), resample=Image.NEAREST)
        target.paste(scaled, (64, 64), scaled)
        target.save(dst)


def build_workflow(ref_name: str, prompt: str, seed: int, steps: int, prefix: str) -> dict:
    return {
        "1": {
            "inputs": {
                "unet_name": "flux1-kontext-dev-fp8-e4m3fn.safetensors",
                "weight_dtype": "default",
            },
            "class_type": "UNETLoader",
        },
        "2": {
            "inputs": {
                "clip_name1": "clip_l.safetensors",
                "clip_name2": "t5xxl_fp8_e4m3fn.safetensors",
                "type": "flux",
            },
            "class_type": "DualCLIPLoader",
        },
        "3": {
            "inputs": {"vae_name": "flux-vae-bf16.safetensors"},
            "class_type": "VAELoader",
        },
        "4": {
            "inputs": {"image": ref_name, "upload": "image"},
            "class_type": "LoadImage",
        },
        "5": {
            "inputs": {"image": ["4", 0]},
            "class_type": "FluxKontextImageScale",
        },
        "6": {
            "inputs": {"pixels": ["5", 0], "vae": ["3", 0]},
            "class_type": "VAEEncode",
        },
        "7": {
            "inputs": {"text": prompt, "clip": ["2", 0]},
            "class_type": "CLIPTextEncode",
        },
        "8": {
            "inputs": {"guidance": 3.5, "conditioning": ["7", 0]},
            "class_type": "FluxGuidance",
        },
        "9": {
            "inputs": {"conditioning": ["8", 0], "latent": ["6", 0]},
            "class_type": "ReferenceLatent",
        },
        "10": {
            "inputs": {"text": NEGATIVE, "clip": ["2", 0]},
            "class_type": "CLIPTextEncode",
        },
        "11": {
            "inputs": {"width": 512, "height": 512, "batch_size": 1},
            "class_type": "EmptySD3LatentImage",
        },
        "12": {
            "inputs": {
                "seed": seed,
                "steps": steps,
                "cfg": 1.0,
                "sampler_name": "euler",
                "scheduler": "simple",
                "denoise": 0.75,
                "model": ["1", 0],
                "positive": ["9", 0],
                "negative": ["10", 0],
                "latent_image": ["11", 0],
            },
            "class_type": "KSampler",
        },
        "13": {
            "inputs": {"samples": ["12", 0], "vae": ["3", 0]},
            "class_type": "VAEDecode",
        },
        "14": {
            "inputs": {"filename_prefix": prefix, "images": ["13", 0]},
            "class_type": "SaveImage",
        },
    }


def queue_and_wait(workflow: dict) -> dict:
    payload = {"prompt": workflow}
    queued = comfy_post(f"{COMFY_URL}/prompt", payload)
    prompt_id = queued["prompt_id"]
    t0 = time.time()
    while True:
        time.sleep(2)
        history = comfy_get(f"{COMFY_URL}/history/{prompt_id}")
        if prompt_id not in history:
            continue
        elapsed = time.time() - t0
        outputs = history[prompt_id].get("outputs", {})
        for node_data in outputs.values():
            images = node_data.get("images")
            if images:
                img = images[0]
                img["elapsed"] = elapsed
                return img
        raise RuntimeError(f"ComfyUI finished but no image output for prompt_id={prompt_id}")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--seed", type=int, default=42)
    parser.add_argument("--steps", type=int, default=28)
    args = parser.parse_args()

    ensure_comfy_alive()
    OUTPUT_ROOT.mkdir(parents=True, exist_ok=True)

    print("[start] comfy pixel-art south batch")
    print(f"[cfg] seed={args.seed} steps={args.steps}")

    for cls, identity in CLASS_SPECS.items():
        try:
            ref_src = pick_reference_path(cls)
        except FileNotFoundError as e:
            print(f"[skip] {cls}: {e}")
            continue

        comfy_ref_name = f"rima_ref_{cls}_south_lock.png"
        comfy_ref_path = COMFY_INPUT_DIR / comfy_ref_name
        preprocess_reference(ref_src, comfy_ref_path)

        positive = f"{BASE_POSITIVE}, {identity}"
        prefix = f"RIMA/pixelart_south/{cls}/{cls}_s{args.seed}"
        wf = build_workflow(
            ref_name=comfy_ref_name,
            prompt=positive,
            seed=args.seed,
            steps=args.steps,
            prefix=prefix,
        )

        print(f"[run] {cls} ...", end="", flush=True)
        out_meta = queue_and_wait(wf)
        print(f" ok ({out_meta['elapsed']:.1f}s) -> {out_meta['filename']}")

        comfy_out_path = Path("F:/AI/ComfyUI/output") / out_meta["subfolder"] / out_meta["filename"]
        if not comfy_out_path.exists():
            raise FileNotFoundError(f"Comfy output missing: {comfy_out_path}")

        class_dir = OUTPUT_ROOT / cls
        class_dir.mkdir(parents=True, exist_ok=True)
        out_512 = class_dir / f"{cls}_south_seed{args.seed}_512.png"
        out_128 = class_dir / f"{cls}_south_seed{args.seed}_128.png"

        shutil.copy2(comfy_out_path, out_512)
        with Image.open(comfy_out_path).convert("RGBA") as im:
            im_128 = im.resize((128, 128), resample=Image.NEAREST)
            im_128.save(out_128)

    print(f"[done] outputs: {OUTPUT_ROOT}")


if __name__ == "__main__":
    main()
