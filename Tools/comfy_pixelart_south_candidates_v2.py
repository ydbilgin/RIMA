#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
RIMA - PixelArt South Candidate Generator (v2)

Goal:
- Generate 10 candidates per class (not a single "best").
- Use existing south lock references as anchors.
- Output 128px PNGs + class contact sheets for manual picking.

Run:
  python Tools/comfy_pixelart_south_candidates_v2.py
  python Tools/comfy_pixelart_south_candidates_v2.py --classes warblade,ranger --count 10
"""

from __future__ import annotations

import argparse
import json
import shutil
import time
import urllib.request
from pathlib import Path

from PIL import Image, ImageDraw

COMFY_URL = "http://127.0.0.1:8188"
COMFY_INPUT = Path("F:/AI/ComfyUI/input")
COMFY_OUTPUT = Path("F:/AI/ComfyUI/output")
REF_ROOT_A = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new")
REF_ROOT_B = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new")
OUT_ROOT = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_candidates_v2")

SEEDS_10 = [42, 77, 123, 189, 256, 333, 456, 589, 777, 902]

GLOBAL_PREFIX = (
    "pixel art, 128px sprite target, single black outline, detailed shading, "
    "grounded dark fantasy, fractured epic color contrast, "
    "high top-down view around 75-80 degrees overhead, top of head visible, torso readable from above, "
    "mature realistic proportions, long legs, normal head size, volumetric body, "
    "not flat, not sticker-like, not chibi, not toy-like, "
    "single character full body centered, plain neutral backdrop, no environment props"
)

GLOBAL_NEG = (
    "photorealistic, 3d render, smooth painterly shading, splash art composition, dramatic camera closeup, "
    "isometric, side-view portrait, low top-down, tiny legs, short squat body, oversized head, "
    "huge aura, particle storm, heavy glow bloom, text, logo, watermark, multiple characters, UI panel, tileset, furniture"
)

CLASS_RULES = {
    "warblade": "male warblade, last warrior of a broken order, bare head no helmet, worn functional mixed armor not parade plate, two-handed greatsword held low, cold blue hairline cracks only on blade, mid-age face not old",
    "elementalist": "female elementalist, rift storm arcanist, one raised lightning orb only, no staff no wand, practical robe with modest slightly open neckline and flowing cloak, cyan-blue orb accent",
    "shadowblade": "male shadowblade, void assassin, lower-face shadow wrap not full mask, dual void blades with subtle purple wisps, black-purple suit with grey plates",
    "ranger": "female ranger, tactical rift hunter not forest archer, longbow with one nocked arrow, trap canisters and tether spool, cold blue arrow tips only",
    "gunslinger": "female gunslinger, rift-tech run-and-gun duelist, dual pistols, long copper-orange hair clearly visible, practical fitted combat coat, not cowboy western trope",
    "hexer": "female hexer, curse-and-decay caster, dark staff plus iron lantern, cursed green plus void purple accents, ragged practical robes",
    "summoner": "female summoner commander, scepter in one hand and other hand open, no giant magic circle effect, ornate purple-gold practical robes, cold blue crystal accent",
    "brawler": "male brawler, unarmed close-pressure fighter, fists ready, muscular build, torn green vest and bare arms, reinforced gauntlets and subtle void-purple tattoo accents",
    "ronin": "male ronin exile, clean readable silhouette, dual katana one drawn one sheathed, layered blue-green robes, minimal cold silver edge accent",
    "ravager": "male ravager blood berserker, broadest roster silhouette, dual large notched axes, blood-red scarification accents, heavy worn practical armor, no helmet",
}


def comfy_get(path: str) -> dict:
    with urllib.request.urlopen(f"{COMFY_URL}{path}") as r:
        return json.loads(r.read().decode("utf-8"))


def comfy_post(path: str, payload: dict) -> dict:
    data = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        f"{COMFY_URL}{path}",
        data=data,
        headers={"Content-Type": "application/json"},
    )
    with urllib.request.urlopen(req) as r:
        return json.loads(r.read().decode("utf-8"))


def ensure_api() -> None:
    stats = comfy_get("/system_stats")
    if "system" not in stats:
        raise RuntimeError("ComfyUI API unavailable")


def pick_ref(cls: str) -> Path:
    candidates = [
        REF_ROOT_A / f"{cls}_south_lock1.png",
        REF_ROOT_A / f"{cls}_south_lock.png",
        REF_ROOT_B / f"{cls}_south_lock.png",
        REF_ROOT_A / f"{cls}_south.png",
        REF_ROOT_A / f"{cls}.png",
    ]
    for p in candidates:
        if p.exists():
            return p
    raise FileNotFoundError(f"reference missing for {cls}")


def prep_ref(src: Path, dst: Path) -> None:
    with Image.open(src).convert("RGBA") as im:
        base = Image.new("RGBA", im.size, (186, 186, 186, 255))
        base.alpha_composite(im)
        out = base.resize((512, 512), Image.NEAREST)
        out.save(dst)


def workflow(ref_name: str, prompt: str, seed: int, prefix: str, steps: int = 26) -> dict:
    # FLUX + InstructPixToPixConditioning path (more stable for anchored edits)
    return {
        "1": {
            "inputs": {"unet_name": "flux1-kontext-dev-fp8-e4m3fn.safetensors", "weight_dtype": "default"},
            "class_type": "UNETLoader",
        },
        "2": {
            "inputs": {"clip_name1": "clip_l.safetensors", "clip_name2": "t5xxl_fp8_e4m3fn.safetensors", "type": "flux"},
            "class_type": "DualCLIPLoader",
        },
        "3": {"inputs": {"vae_name": "flux-vae-bf16.safetensors"}, "class_type": "VAELoader"},
        "11": {"inputs": {"image": ref_name, "upload": "image"}, "class_type": "LoadImage"},
        "12": {"inputs": {"pixels": ["11", 0], "vae": ["3", 0]}, "class_type": "VAEEncode"},
        "4": {"inputs": {"text": prompt, "clip": ["2", 0]}, "class_type": "CLIPTextEncode"},
        "5": {"inputs": {"text": GLOBAL_NEG, "clip": ["2", 0]}, "class_type": "CLIPTextEncode"},
        "6": {"inputs": {"guidance": 3.0, "conditioning": ["4", 0]}, "class_type": "FluxGuidance"},
        "13": {"inputs": {"conditioning": ["6", 0], "latent": ["12", 0]}, "class_type": "InstructPixToPixConditioning"},
        "8": {
            "inputs": {
                "seed": seed,
                "steps": steps,
                "cfg": 1.0,
                "sampler_name": "euler",
                "scheduler": "simple",
                "denoise": 0.72,
                "model": ["1", 0],
                "positive": ["13", 0],
                "negative": ["5", 0],
                "latent_image": ["12", 0],
            },
            "class_type": "KSampler",
        },
        "9": {"inputs": {"samples": ["8", 0], "vae": ["3", 0]}, "class_type": "VAEDecode"},
        "10": {"inputs": {"filename_prefix": prefix, "images": ["9", 0]}, "class_type": "SaveImage"},
    }


def queue_wait(wf: dict) -> dict:
    prompt_id = comfy_post("/prompt", {"prompt": wf})["prompt_id"]
    while True:
        time.sleep(2)
        hist = comfy_get(f"/history/{prompt_id}")
        if prompt_id not in hist:
            continue
        outputs = hist[prompt_id].get("outputs", {})
        for v in outputs.values():
            imgs = v.get("images")
            if imgs:
                return imgs[0]
        raise RuntimeError("No images in history output")


def make_sheet(cls: str, paths: list[Path]) -> None:
    tiles = []
    for i, p in enumerate(paths, 1):
        with Image.open(p).convert("RGBA") as im:
            up = im.resize((128, 128), Image.NEAREST)
            c = Image.new("RGBA", (180, 180), (10, 10, 10, 255))
            c.paste(up, (26, 16), up)
            d = ImageDraw.Draw(c)
            d.text((8, 150), f"{cls} #{i:02d}", fill=(230, 230, 230, 255))
            tiles.append(c)
    cols = 5
    rows = (len(tiles) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * 180, rows * 180), (6, 6, 6, 255))
    for i, t in enumerate(tiles):
        sheet.paste(t, ((i % cols) * 180, (i // cols) * 180), t)
    sheet.save(OUT_ROOT / cls / f"{cls}_contact_sheet.png")


def main() -> None:
    ap = argparse.ArgumentParser()
    ap.add_argument("--classes", default="all", help="comma list or 'all'")
    ap.add_argument("--count", type=int, default=10)
    args = ap.parse_args()

    ensure_api()
    OUT_ROOT.mkdir(parents=True, exist_ok=True)

    if args.classes == "all":
        classes = list(CLASS_RULES.keys())
    else:
        classes = [c.strip().lower() for c in args.classes.split(",") if c.strip()]

    seeds = SEEDS_10[: max(1, min(10, args.count))]

    for cls in classes:
        if cls not in CLASS_RULES:
            print(f"[skip] unknown class: {cls}")
            continue
        ref_src = pick_ref(cls)
        ref_name = f"rima_v2_{cls}_ref.png"
        ref_dst = COMFY_INPUT / ref_name
        prep_ref(ref_src, ref_dst)

        class_dir = OUT_ROOT / cls
        class_dir.mkdir(parents=True, exist_ok=True)
        produced: list[Path] = []

        prompt = f"{GLOBAL_PREFIX}, {CLASS_RULES[cls]}"
        print(f"\n=== {cls.upper()} ===")
        print(f"[ref] {ref_src.name}")

        for idx, seed in enumerate(seeds, 1):
            prefix = f"RIMA/pixelart_v2/{cls}/{cls}_s{seed}"
            wf = workflow(ref_name=ref_name, prompt=prompt, seed=seed, prefix=prefix, steps=26)
            print(f"[run] {cls} #{idx:02d} seed={seed} ...", end="", flush=True)
            img = queue_wait(wf)
            src_out = COMFY_OUTPUT / img["subfolder"] / img["filename"]
            out_512 = class_dir / f"{cls}_cand_{idx:02d}_seed{seed}_512.png"
            out_128 = class_dir / f"{cls}_cand_{idx:02d}_seed{seed}_128.png"
            shutil.copy2(src_out, out_512)
            with Image.open(src_out).convert("RGBA") as im:
                im.resize((128, 128), Image.NEAREST).save(out_128)
            produced.append(out_128)
            print(" ok")

        make_sheet(cls, produced)
        print(f"[done] {cls}: {len(produced)} candidates")

    print(f"\nAll done -> {OUT_ROOT}")


if __name__ == "__main__":
    main()
