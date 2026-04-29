#!/usr/bin/env python3
"""
RIMA — Model İndirici
======================
ComfyUI + diffusers için gerekli tüm modelleri indirir.
Toplam: ~8-9 GB, bir kez indir, hep kullan.

Çalıştırma:
  python tools/rima_models.py
"""

import sys
import os
from pathlib import Path

COMFYUI_DIR  = Path(r"F:\ComfyUI")
HF_CACHE_DIR = COMFYUI_DIR / "hf_cache"
os.environ["HF_HOME"] = str(HF_CACHE_DIR)

def check_deps():
    try:
        from huggingface_hub import hf_hub_download, snapshot_download
    except ImportError:
        print("HATA: huggingface_hub yüklü değil.")
        print("  pip install huggingface_hub")
        sys.exit(1)

def download_model(repo_id, filename, dest_dir, desc=""):
    from huggingface_hub import hf_hub_download
    dest = Path(dest_dir) / filename
    if dest.exists():
        print(f"  [ATLA] {desc or filename} (zaten var)")
        return str(dest)
    print(f"  [İNDİR] {desc or filename}...")
    path = hf_hub_download(
        repo_id=repo_id,
        filename=filename,
        local_dir=dest_dir,
        local_dir_use_symlinks=False,
    )
    print(f"  [OK]    → {dest}")
    return path

def download_snapshot(repo_id, dest_dir, desc=""):
    from huggingface_hub import snapshot_download
    dest = Path(dest_dir)
    if dest.exists() and any(dest.iterdir()):
        print(f"  [ATLA] {desc} (zaten var)")
        return str(dest)
    print(f"  [İNDİR] {desc} (~birkaç dakika)...")
    snapshot_download(repo_id=repo_id, local_dir=dest_dir, local_dir_use_symlinks=False)
    print(f"  [OK]    → {dest}")
    return str(dest)

def main():
    check_deps()

    print("\nRIMA Model İndirici")
    print("=" * 50)
    print(f"Hedef: {COMFYUI_DIR}")
    print(f"HF Cache: {HF_CACHE_DIR}\n")

    # ── 1. SD 1.5 Base Model (AnimateDiff için diffusers formatı) ─────────────
    print("[1/5] SD 1.5 Base — Realistic Vision V5.1 (~2 GB)")
    download_snapshot(
        repo_id="SG161222/Realistic_Vision_V5.1_noVAE",
        dest_dir=COMFYUI_DIR / "hf_cache" / "realistic_vision_v5",
        desc="Realistic Vision V5.1 (diffusers format)"
    )

    # ── 2. AnimateDiff Motion Module ──────────────────────────────────────────
    print("\n[2/5] AnimateDiff Motion Module v3 (~1.7 GB)")
    download_model(
        repo_id="guoyww/animatediff-motion-adapter-v1-5-3",
        filename="diffusion_pytorch_model.safetensors",
        dest_dir=COMFYUI_DIR / "models" / "animatediff_models",
        desc="AnimateDiff v3 motion module"
    )
    # ComfyUI için ayrıca config
    download_model(
        repo_id="guoyww/animatediff-motion-adapter-v1-5-3",
        filename="config.json",
        dest_dir=COMFYUI_DIR / "models" / "animatediff_models",
        desc="AnimateDiff config"
    )

    # ── 3. Pixel Art LoRA ─────────────────────────────────────────────────────
    print("\n[3/5] Pixel Art LoRA (~144 MB)")
    download_model(
        repo_id="artificialguybr/PixelArtRedmond15V2",
        filename="PixelArtRedmond15V2-PixelArt15V2-SDXLPAHD.safetensors",
        dest_dir=COMFYUI_DIR / "models" / "loras",
        desc="Pixel Art Redmond LoRA"
    )

    # ── 4. IPAdapter (karakter tutarlılığı için) ──────────────────────────────
    print("\n[4/5] IPAdapter SD 1.5 (~290 MB)")
    download_model(
        repo_id="h94/IP-Adapter",
        filename="models/ip-adapter-plus_sd15.bin",  # Plus = daha iyi karakter tutarlılığı
        dest_dir=COMFYUI_DIR / "models" / "ipadapter",
        desc="IPAdapter Plus SD 1.5"
    )
    download_model(
        repo_id="h94/IP-Adapter",
        filename="models/image_encoder/pytorch_model.bin",
        dest_dir=COMFYUI_DIR / "models" / "clip_vision",
        desc="CLIP Vision (IPAdapter için)"
    )

    # ── 5. VAE ────────────────────────────────────────────────────────────────
    print("\n[5/5] VAE (~320 MB)")
    download_model(
        repo_id="stabilityai/sd-vae-ft-mse-original",
        filename="vae-ft-mse-840000-ema-pruned.safetensors",
        dest_dir=COMFYUI_DIR / "models" / "vae",
        desc="SD VAE ft-mse"
    )

    print("\n" + "=" * 50)
    print("Tüm modeller indirildi!")
    print("\nSıradaki adım:")
    print("  python tools/rima_generate.py")
    print("\nVEYA ComfyUI web UI için:")
    print("  cd F:\\ComfyUI && python main.py")

if __name__ == "__main__":
    main()
