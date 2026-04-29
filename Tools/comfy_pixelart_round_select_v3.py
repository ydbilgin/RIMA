#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
RIMA pixel-art candidate selection (4->1 x 10 rounds per class)

Per class:
- 10 rounds
- each round generates 4 variants
- auto-selects best 1 by heuristic score
- saves all raw variants + selected

Output root:
  TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_round_select_v3/<class>/
"""

from __future__ import annotations

import argparse
import json
import math
import shutil
import time
import urllib.request
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw

COMFY_URL = "http://127.0.0.1:8188"
COMFY_INPUT = Path("F:/AI/ComfyUI/input")
COMFY_OUTPUT = Path("F:/AI/ComfyUI/output")
REF_ROOT_A = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new")
REF_ROOT_B = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new")
OUT_ROOT = Path("F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/pixelart_round_select_v3")
CHECKPOINT_NAME = "sd_xl_base_1.0_0.9vae.safetensors"
LORA_NAME = "pixel-art-xl.safetensors"

GLOBAL_PREFIX = (
    "pixel art, 128px sprite target, single black outline, detailed shading, "
    "grounded dark fantasy, fractured epic contrast, high top-down 75-80 degree overhead, "
    "top of head visible, torso readable from above, mature realistic proportions, long legs, normal head size, "
    "volumetric body, full body single character centered, plain neutral background"
)
GLOBAL_NEG = (
    "photorealistic, 3d render, painterly, splash art, isometric, side-view portrait, UI panel, furniture, tileset, "
    "chibi, dwarf-like, short legs, squat body, oversized head, huge aura, heavy particles, watermark, text, logo"
)

CLASS_RULES = {
    "warblade": "male warblade, broken-order last warrior, bare head no helmet, worn practical armor, two-handed greatsword low, cold blue hairline cracks only on blade, mid-age not old",
    "elementalist": "female elementalist, one lightning orb only, no staff no wand, practical robe with modest slightly open neckline and flowing cloak, cyan-blue orb accent",
    "shadowblade": "male shadowblade, lower-face wrap, dual void blades, subtle purple wisps, black-purple suit with grey plates",
    "ranger": "female tactical rift hunter not forest archer, longbow plus single nocked arrow, trap canisters and tether spool, cold blue arrow tip accent only",
    "gunslinger": "female rift-tech duelist run-and-gun, dual pistols, long copper-orange hair, practical fitted coat, not cowboy trope",
    "hexer": "female hexer, dark staff and iron lantern, cursed green and void purple accents, ragged practical robes",
    "summoner": "female summoner commander, scepter in one hand and other hand open, no giant runic circle, ornate practical purple-gold robe with cold blue crystal accent",
    "brawler": "male brawler unarmed fists, muscular, torn green vest, reinforced gauntlets, subtle void-purple tattoo accents",
    "ronin": "male ronin exile, clean silhouette, dual katana one drawn one sheathed, layered blue-green robes, minimal cold silver edge accent",
    "ravager": "male ravager blood berserker, widest silhouette, dual large notched axes, blood-red scarification accent, heavy worn practical armor, no helmet",
}


def comfy_get(path: str) -> dict:
    with urllib.request.urlopen(f"{COMFY_URL}{path}") as r:
        return json.loads(r.read().decode("utf-8"))


def comfy_post(path: str, payload: dict) -> dict:
    body = json.dumps(payload).encode("utf-8")
    req = urllib.request.Request(
        f"{COMFY_URL}{path}",
        data=body,
        headers={"Content-Type": "application/json"},
    )
    with urllib.request.urlopen(req) as r:
        return json.loads(r.read().decode("utf-8"))


def ensure_api() -> None:
    s = comfy_get("/system_stats")
    if "system" not in s:
        raise RuntimeError("ComfyUI API unavailable.")


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
    raise FileNotFoundError(f"Missing reference for {cls}")


def prep_ref(src: Path, dst: Path) -> None:
    with Image.open(src).convert("RGBA") as im:
        bg = Image.new("RGBA", im.size, (186, 186, 186, 255))
        bg.alpha_composite(im)
        bg.resize((512, 512), Image.NEAREST).save(dst)


def build_workflow(ref_name: str, prompt: str, seed: int, prefix: str, steps: int = 30) -> dict:
    return {
        "1": {
            "inputs": {"ckpt_name": CHECKPOINT_NAME},
            "class_type": "CheckpointLoaderSimple",
        },
        "2": {
            "inputs": {
                "lora_name": LORA_NAME,
                "strength_model": 0.90,
                "strength_clip": 0.90,
                "model": ["1", 0],
                "clip": ["1", 1],
            },
            "class_type": "LoraLoader",
        },
        "3": {"inputs": {"image": ref_name, "upload": "image"}, "class_type": "LoadImage"},
        "4": {"inputs": {"pixels": ["3", 0], "vae": ["1", 2]}, "class_type": "VAEEncode"},
        "5": {"inputs": {"text": prompt, "clip": ["2", 1]}, "class_type": "CLIPTextEncode"},
        "6": {"inputs": {"text": GLOBAL_NEG, "clip": ["2", 1]}, "class_type": "CLIPTextEncode"},
        "7": {
            "inputs": {
                "seed": seed,
                "steps": steps,
                "cfg": 6.0,
                "sampler_name": "dpmpp_2m",
                "scheduler": "karras",
                "denoise": 0.52,
                "model": ["2", 0],
                "positive": ["5", 0],
                "negative": ["6", 0],
                "latent_image": ["4", 0],
            },
            "class_type": "KSampler",
        },
        "8": {"inputs": {"samples": ["7", 0], "vae": ["1", 2]}, "class_type": "VAEDecode"},
        "9": {"inputs": {"filename_prefix": prefix, "images": ["8", 0]}, "class_type": "SaveImage"},
    }


def queue_wait(wf: dict) -> dict:
    prompt_id = comfy_post("/prompt", {"prompt": wf})["prompt_id"]
    while True:
        time.sleep(2)
        hist = comfy_get(f"/history/{prompt_id}")
        if prompt_id not in hist:
            continue
        outputs = hist[prompt_id].get("outputs", {})
        for node in outputs.values():
            imgs = node.get("images")
            if imgs:
                return imgs[0]
        raise RuntimeError("No image output returned.")


def fg_mask(img_rgba: np.ndarray) -> np.ndarray:
    rgb = img_rgba[:, :, :3].astype(np.int16)
    h, w = rgb.shape[:2]
    corners = np.array([rgb[0, 0], rgb[0, w - 1], rgb[h - 1, 0], rgb[h - 1, w - 1]])
    bg = np.median(corners, axis=0)
    dist = np.max(np.abs(rgb - bg), axis=2)
    return dist > 22


def score_image(path: Path) -> float:
    with Image.open(path).convert("RGBA") as im:
        arr = np.array(im)
    m = fg_mask(arr)
    h, w = m.shape
    fg_ratio = float(m.mean())

    ys, xs = np.where(m)
    if len(xs) == 0:
        return -9999.0

    x0, x1 = xs.min(), xs.max()
    y0, y1 = ys.min(), ys.max()
    bw = max(1, x1 - x0 + 1)
    bh = max(1, y1 - y0 + 1)

    # center score (prefer centered character)
    cx, cy = xs.mean(), ys.mean()
    dc = math.hypot(cx - (w / 2), cy - (h / 2))
    center_score = 1.0 - min(1.0, dc / (w * 0.35))

    # border touch penalty
    edge = np.zeros_like(m)
    edge[0, :] = True
    edge[-1, :] = True
    edge[:, 0] = True
    edge[:, -1] = True
    edge_touch = (m & edge).sum()
    edge_pen = min(1.0, edge_touch / 40.0)

    # ideal body occupancy for 512 -> about 10%-35%
    occ_score = 1.0 - min(1.0, abs(fg_ratio - 0.20) / 0.20)

    # vertical dominance (character should be taller than wide in south)
    shape_score = min(1.0, (bh / bw) / 1.6) if bw > 0 else 0.0

    return (center_score * 2.0) + (occ_score * 1.8) + (shape_score * 1.4) - (edge_pen * 2.5)


def make_contact_sheet(cls_dir: Path, cls: str, selected_paths: list[Path]) -> None:
    tiles = []
    for i, p in enumerate(selected_paths, 1):
        with Image.open(p).convert("RGBA") as im:
            up = im.resize((128, 128), Image.NEAREST)
            c = Image.new("RGBA", (180, 180), (10, 10, 10, 255))
            c.paste(up, (26, 16), up)
            d = ImageDraw.Draw(c)
            d.text((8, 150), f"{cls} pick {i:02d}", fill=(230, 230, 230, 255))
            tiles.append(c)
    cols = 5
    rows = (len(tiles) + cols - 1) // cols
    sheet = Image.new("RGBA", (cols * 180, max(1, rows) * 180), (6, 6, 6, 255))
    for i, t in enumerate(tiles):
        sheet.paste(t, ((i % cols) * 180, (i // cols) * 180), t)
    sheet.save(cls_dir / f"{cls}_selected10_sheet.png")


def main() -> None:
    ap = argparse.ArgumentParser()
    ap.add_argument("--classes", default="all", help="all or comma-separated class list")
    ap.add_argument("--rounds", type=int, default=10)
    ap.add_argument("--variants", type=int, default=4)
    ap.add_argument("--steps", type=int, default=24)
    ap.add_argument("--smoke", action="store_true", help="run only first class, 1 round")
    args = ap.parse_args()

    ensure_api()
    OUT_ROOT.mkdir(parents=True, exist_ok=True)

    classes = list(CLASS_RULES.keys()) if args.classes == "all" else [c.strip().lower() for c in args.classes.split(",") if c.strip()]
    if args.smoke:
        classes = classes[:1]
        args.rounds = 1
        args.variants = 2

    for cls in classes:
        if cls not in CLASS_RULES:
            print(f"[skip] unknown class {cls}")
            continue

        cls_dir = OUT_ROOT / cls
        raw_dir = cls_dir / "raw"
        picks_dir = cls_dir / "selected10"
        raw_dir.mkdir(parents=True, exist_ok=True)
        picks_dir.mkdir(parents=True, exist_ok=True)

        ref_src = pick_ref(cls)
        ref_name = f"rima_v3_ref_{cls}.png"
        ref_dst = COMFY_INPUT / ref_name
        prep_ref(ref_src, ref_dst)

        prompt = f"{GLOBAL_PREFIX}, {CLASS_RULES[cls]}"
        print(f"\n=== {cls.upper()} ===")
        print(f"[ref] {ref_src.name}")

        selected: list[Path] = []
        for r in range(1, args.rounds + 1):
            round_paths: list[Path] = []
            round_scores: list[float] = []

            for v in range(1, args.variants + 1):
                seed = (r * 10000) + (v * 997) + (hash(cls) % 9973)
                prefix = f"RIMA/pixelart_v3/{cls}/{cls}_r{r:02d}_v{v:02d}_s{seed}"
                wf = build_workflow(ref_name, prompt, seed, prefix, steps=args.steps)
                print(f"[gen] {cls} r{r:02d} v{v:02d} ...", end="", flush=True)
                meta = queue_wait(wf)
                src = COMFY_OUTPUT / meta["subfolder"] / meta["filename"]
                raw_512 = raw_dir / f"{cls}_r{r:02d}_v{v:02d}_seed{seed}_512.png"
                raw_128 = raw_dir / f"{cls}_r{r:02d}_v{v:02d}_seed{seed}_128.png"
                shutil.copy2(src, raw_512)
                with Image.open(src).convert("RGBA") as im:
                    im.resize((128, 128), Image.NEAREST).save(raw_128)
                score = score_image(raw_512)
                round_paths.append(raw_128)
                round_scores.append(score)
                print(f" ok score={score:.3f}")

            best_idx = int(np.argmax(np.array(round_scores)))
            best_src = round_paths[best_idx]
            best_dst = picks_dir / f"{cls}_pick_{r:02d}.png"
            shutil.copy2(best_src, best_dst)
            selected.append(best_dst)
            print(f"[pick] {cls} round {r:02d} -> v{best_idx + 1:02d}")

        make_contact_sheet(cls_dir, cls, selected)
        print(f"[done] {cls}: {len(selected)} selected images")

    print(f"\nAll done -> {OUT_ROOT}")


if __name__ == "__main__":
    main()
