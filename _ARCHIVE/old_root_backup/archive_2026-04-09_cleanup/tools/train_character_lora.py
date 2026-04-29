"""
train_character_lora.py — RIMA karakterleri icin SDXL LoRA egitir.

RTX 5080 + gece modunda tum karakterleri egitir.
Her karakter icin ~45-90 dakika. Toplam 8 karakter = ~8-12 saat.

Kullanim:
    python tools/train_character_lora.py --char warblade
    python tools/train_character_lora.py --all          (gece modu)
    python tools/train_character_lora.py --prep-only    (sadece veri hazirla)

Gereksinimler:
    pip install diffusers transformers accelerate peft torch torchvision bitsandbytes
    pip install datasets pillow

Egitim sonrasi:
    models/loras/warblade_lora.safetensors
    Bu LoRA'yi ComfyUI veya mimicmotion_run.py icinde kullanabilirsin.
"""

import sys
import os
import json
import shutil
import argparse
from pathlib import Path

ROOT = Path(__file__).parent.parent
ART_DIR = ROOT / "ART"
MODELS_DIR = ROOT / "models"
LORA_DIR = MODELS_DIR / "loras"
DATASET_DIR = MODELS_DIR / "datasets"

# Egitim parametreleri — RTX 5080 icin optimize
TRAIN_CONFIG = {
    "base_model":        "stabilityai/stable-diffusion-xl-base-1.0",
    "resolution":        512,
    "train_batch_size":  1,
    "gradient_accumulation_steps": 4,
    "max_train_steps":   1000,    # karakter basina, gece modu icin yeterli
    "learning_rate":     1e-4,
    "lr_scheduler":      "cosine",
    "lora_rank":         16,      # 8-32 arasi; 16 = kalite/boyut dengesi
    "mixed_precision":   "bf16",  # RTX 5080 bf16 destekler
    "enable_xformers":   True,
    "save_every_n_steps": 250,
    "seed":              42,
}

CHARS = {
    "warblade":    {
        "img": "karakterler/warblade/warblade_gemini_base.png",
        "trigger": "warblade_rima",
        "caption": "warblade_rima character, battle-hardened warrior, battered armor, scarred face, greatsword, top-down view, pixel art game sprite",
    },
    "elementalist": {
        "img": "karakterler/elementalist/elementalist_gemini_base.png",
        "trigger": "elementalist_rima",
        "caption": "elementalist_rima character, tired mage, weathered robes, staff, fire magic, top-down view, pixel art game sprite",
    },
    "shadowblade": {
        "img": "karakterler/shadowblade/shadowblade_gemini_base.png",
        "trigger": "shadowblade_rima",
        "caption": "shadowblade_rima character, rogue assassin, worn leather, dual daggers, masked eyes, top-down view, pixel art game sprite",
    },
    "ranger": {
        "img": "karakterler/ranger/ranger_gemini_base.png",
        "trigger": "ranger_rima",
        "caption": "ranger_rima character, wilderness survivor, dirty face, bow, patched leather, top-down view, pixel art game sprite",
    },
    "ravager": {
        "img": "karakterler/ravager/ravager_gemini_base.png",
        "trigger": "ravager_rima",
        "caption": "ravager_rima character, berserker, bare torso, war paint, axe, wild hair, top-down view, pixel art game sprite",
    },
    "paladin": {
        "img": "karakterler/paladin/paladin_gemini_base.png",
        "trigger": "paladin_rima",
        "caption": "paladin_rima character, fallen paladin, tarnished armor, weary face, shield, fading holy light, top-down view, pixel art game sprite",
    },
    "summoner": {
        "img": "karakterler/summoner/summoner_gemini_base.png",
        "trigger": "summoner_rima",
        "caption": "summoner_rima character, old weathered mage, tattered robes, bone staff, skeletal minions, top-down view, pixel art game sprite",
    },
    "hexer": {
        "img": "karakterler/hexer/hexer_gemini_base.png",
        "trigger": "hexer_rima",
        "caption": "hexer_rima character, young corrupted warlock, void corruption on face, cursed tome, purple decay, top-down view, pixel art game sprite",
    },
    "grunt_shard": {
        "img": "dusmanlar/grunt_shard/grunt_shard_gemini_base.png",
        "trigger": "grunt_shard_rima",
        "caption": "grunt_shard_rima enemy, shard crystal creature, small grunt, top-down view, pixel art game sprite",
    },
}


def check_deps():
    try:
        import torch
        import diffusers
        import peft
        import accelerate
        print(f"PyTorch: {torch.__version__} | CUDA: {torch.cuda.is_available()} | GPU: {torch.cuda.get_device_name(0) if torch.cuda.is_available() else 'YOK'}")
    except ImportError as e:
        print(f"HATA eksik paket: {e}")
        print("Calistir: pip install diffusers transformers accelerate peft torch bitsandbytes")
        sys.exit(1)


def prepare_dataset(char_name: str, char_info: dict) -> Path:
    """Egitim icin veri seti hazirla: gorsel + caption dosyasi."""
    img_path = ART_DIR / char_info["img"]
    if not img_path.exists():
        print(f"  ATLA: Gorsel bulunamadi: {img_path}")
        return None

    ds_dir = DATASET_DIR / char_name
    ds_dir.mkdir(parents=True, exist_ok=True)

    from PIL import Image
    img = Image.open(img_path).convert("RGB")

    # Tek gorsel varsa augmentasyon ile cogalt (yatay flip + renk varyantlari)
    variants = []

    # Orijinal
    resized = img.resize((TRAIN_CONFIG["resolution"], TRAIN_CONFIG["resolution"]), Image.LANCZOS)
    variants.append(("orig", resized))

    # Yatay flip
    variants.append(("flip", resized.transpose(Image.FLIP_LEFT_RIGHT)))

    # Hafif parlaklik varyantlari
    from PIL import ImageEnhance
    for i, factor in enumerate([0.85, 1.15]):
        enhanced = ImageEnhance.Brightness(resized).enhance(factor)
        variants.append((f"bright_{i}", enhanced))

    # Kontrast varyanti
    for i, factor in enumerate([0.9, 1.1]):
        enhanced = ImageEnhance.Contrast(resized).enhance(factor)
        variants.append((f"contrast_{i}", enhanced))

    caption = char_info["caption"]
    saved = 0
    for name, v_img in variants:
        img_out = ds_dir / f"{char_name}_{name}.png"
        txt_out = ds_dir / f"{char_name}_{name}.txt"
        v_img.save(img_out)
        txt_out.write_text(caption)
        saved += 1

    print(f"  Veri seti hazir: {saved} gorsel → {ds_dir}")
    return ds_dir


def train_lora(char_name: str, ds_dir: Path):
    """SDXL LoRA egitimi — diffusers DreamBooth script kullanir."""
    import torch
    from accelerate.utils import write_basic_config

    LORA_DIR.mkdir(parents=True, exist_ok=True)
    out_path = LORA_DIR / f"{char_name}_lora"
    out_path.mkdir(exist_ok=True)

    # accelerate config (varsa atla)
    accel_cfg = Path.home() / ".cache" / "huggingface" / "accelerate" / "default_config.yaml"
    if not accel_cfg.exists():
        write_basic_config(save_location=str(accel_cfg))

    # diffusers DreamBooth LoRA egitim scripti
    script = Path(__file__).parent / "dreambooth_lora_sdxl.py"
    if not script.exists():
        print("  DreamBooth script indiriliyor...")
        import urllib.request
        url = "https://raw.githubusercontent.com/huggingface/diffusers/main/examples/dreambooth/train_dreambooth_lora_sdxl.py"
        urllib.request.urlretrieve(url, script)

    char_info = CHARS[char_name]
    trigger = char_info["trigger"]

    cmd = [
        sys.executable, str(script),
        "--pretrained_model_name_or_path",    TRAIN_CONFIG["base_model"],
        "--instance_data_dir",                str(ds_dir),
        "--output_dir",                       str(out_path),
        "--instance_prompt",                  f"a photo of {trigger}",
        "--resolution",                       str(TRAIN_CONFIG["resolution"]),
        "--train_batch_size",                 str(TRAIN_CONFIG["train_batch_size"]),
        "--gradient_accumulation_steps",      str(TRAIN_CONFIG["gradient_accumulation_steps"]),
        "--max_train_steps",                  str(TRAIN_CONFIG["max_train_steps"]),
        "--learning_rate",                    str(TRAIN_CONFIG["learning_rate"]),
        "--lr_scheduler",                     TRAIN_CONFIG["lr_scheduler"],
        "--rank",                             str(TRAIN_CONFIG["lora_rank"]),
        "--mixed_precision",                  TRAIN_CONFIG["mixed_precision"],
        "--seed",                             str(TRAIN_CONFIG["seed"]),
        "--checkpointing_steps",              str(TRAIN_CONFIG["save_every_n_steps"]),
        "--enable_xformers_memory_efficient_attention",
    ]

    print(f"\n  LoRA egitimi basliyor: {char_name}")
    print(f"  Trigger kelime: '{trigger}'")
    print(f"  Max steps: {TRAIN_CONFIG['max_train_steps']} (~45-90 dk RTX 5080)")
    print(f"  Cikti: {out_path}\n")

    import subprocess
    result = subprocess.run(cmd, cwd=str(ROOT))

    if result.returncode == 0:
        # safetensors bul ve ana dizine kopyala
        safetensors_files = list(out_path.rglob("*.safetensors"))
        if safetensors_files:
            final = LORA_DIR / f"{char_name}_lora.safetensors"
            shutil.copy(safetensors_files[-1], final)
            print(f"\n  TAMAMLANDI: {final}")
        else:
            print(f"\n  TAMAMLANDI: {out_path}")
    else:
        print(f"\n  HATA: Egitim basarisiz (return code {result.returncode})")


def main():
    parser = argparse.ArgumentParser(description="RIMA karakter LoRA egitici")
    parser.add_argument("--char", choices=list(CHARS.keys()), help="Tek karakter egit")
    parser.add_argument("--all", action="store_true", help="Tum karakterleri gece boyunca egit")
    parser.add_argument("--prep-only", action="store_true", help="Sadece veri seti hazirla, egitme")
    args = parser.parse_args()

    check_deps()
    LORA_DIR.mkdir(parents=True, exist_ok=True)
    DATASET_DIR.mkdir(parents=True, exist_ok=True)

    targets = list(CHARS.keys()) if args.all else ([args.char] if args.char else [])
    if not targets:
        parser.print_help()
        return

    print(f"\nGECE MODU: {len(targets)} karakter egitilecek")
    print(f"Tahmini sure: {len(targets) * 1.5:.0f}-{len(targets) * 2:.0f} saat\n")

    for i, char_name in enumerate(targets, 1):
        print(f"\n{'='*50}")
        print(f"[{i}/{len(targets)}] {char_name.upper()}")
        print(f"{'='*50}")

        char_info = CHARS[char_name]
        ds_dir = prepare_dataset(char_name, char_info)
        if ds_dir is None:
            print(f"  ATLA: {char_name} (gorsel eksik)")
            continue

        if not args.prep_only:
            train_lora(char_name, ds_dir)

    print(f"\n{'='*50}")
    print("TUM EGITIMLER TAMAMLANDI")
    print(f"LoRA dosyalari: {LORA_DIR}")
    print("ComfyUI'da kullanim: Load LoRA node -> models/loras/[karakter]_lora.safetensors")
    print(f"{'='*50}")


if __name__ == "__main__":
    main()
