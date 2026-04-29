#!/usr/bin/env python3
"""
RIMA — Sprite Üretim Scripti
==============================
Imagen 3 referanslarından → pixel art sprite + animasyon sprite sheet üretir.
Çıktıları doğru klasörlere kaydeder, URETIM_PLANI.md'yi ✅ ile günceller.

Önkoşullar:
  1. tools/comfyui_install.bat çalıştırılmış olmalı
  2. tools/rima_models.py ile modeller indirilmiş olmalı
  3. tools/gemini_ref_generator.py ile Imagen 3 referanslar üretilmiş olmalı

Çalıştırma:
  python tools/rima_generate.py

  Sadece belirli bir karakter:
  python tools/rima_generate.py --only warblade
  python tools/rima_generate.py --only shard_walker
  python tools/rima_generate.py --only iron_warden
"""

import argparse
import sys
import os
import re
from pathlib import Path

# ─── TOPLULUK AYARLARI (kaynak: CivitAI + Inner-Reflections AnimateDiff guide) ─
#
#  img2img BASE sprite:
#    Sampler : DPM++ 2M Karras | CFG: 7.5 | Steps: 25 | Strength: 0.60
#    LoRA    : "p1x3l" trigger @ 0.85  (Pixelized Art v3 — civitai.com/models/550585)
#    HF fallback: artificialguybr/PixelArtRedmond15V2
#
#  AnimateDiff animasyon:
#    Module  : v3_sd15_mm  | Sampler: Euler_a | CFG: 7.5 | Steps: 25
#    Frames  : max 16 (sliding window devreye girmeden)
#    IPAdapter Plus @ 0.70, end_at=0.6  (karakter tutarlılığı)
#    LoRA    : 0.75
#
#  Hybrid mod (ÖNERİLEN — en iyi sonuç):
#    BASE sprite → PixelLab (top-down kontrolü çok daha iyi)
#    Animasyon   → Bu script
#    Kullan: python tools/rima_generate.py --anim-only

# ─── YOLLAR ───────────────────────────────────────────────────────────────────

BASE_DIR     = Path(r"F:\Antigravity Projeler\2d roguelite")
ART_DIR      = BASE_DIR / "ART"
COMFYUI_DIR  = Path(r"F:\ComfyUI")
HF_CACHE_DIR = COMFYUI_DIR / "hf_cache"

os.environ["HF_HOME"] = str(HF_CACHE_DIR)

# ─── SPRITE TANIMLARI ─────────────────────────────────────────────────────────

SPRITES = {

    "warblade": {
        "ref_image"  : ART_DIR / "karakterler/warblade/warblade_gemini_base.png",
        "base_out"   : ART_DIR / "karakterler/warblade/warblade_S_BASE.png",
        "canvas_size": 64,
        "base_prompt": (
            "top-down pixel art game sprite, heavily armored dark iron warrior, "
            "greatsword in right hand pointing down, cracked breastplate glowing cold blue, "
            "torn cape, heavy pauldrons, facing south, 64x64, transparent background, "
            "pixel art, dark fantasy RPG, sprite, no background"
        ),
        "animations": [
            {
                "name"       : "idle",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_idle.png",
                "frame_count": 4,
                "prompt"     : "pixel art warrior idle, slow breathing, weapon sway, cold blue light pulsing",
            },
            {
                "name"       : "walk",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_walk.png",
                "frame_count": 6,
                "prompt"     : "pixel art warrior walking south, armored footsteps, cape moving",
            },
            {
                "name"       : "attack1",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_attack1.png",
                "frame_count": 6,
                "prompt"     : "pixel art warrior horizontal greatsword slash, windup then strong swing",
            },
            {
                "name"       : "attack2",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_attack2.png",
                "frame_count": 6,
                "prompt"     : "pixel art warrior raising greatsword overhead slam into ground, shockwave",
            },
            {
                "name"       : "dash",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_dash.png",
                "frame_count": 4,
                "prompt"     : "pixel art warrior explosive forward dash, body leaning aggressively",
            },
            {
                "name"       : "hurt",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_hurt.png",
                "frame_count": 4,
                "prompt"     : "pixel art warrior staggering backward from impact, flinch recoil",
            },
            {
                "name"       : "death",
                "out"        : ART_DIR / "karakterler/warblade/warblade_S_death.png",
                "frame_count": 8,
                "prompt"     : "pixel art warrior collapsing forward, armor cracking, energy dissipating",
            },
        ],
        "md_section": "WARBLADE",
    },

    "shard_walker": {
        "ref_image"  : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_gemini_base.png",
        "base_out"   : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_S_BASE.png",
        "canvas_size": 32,
        "base_prompt": (
            "top-down pixel art game sprite, humanoid creature floating broken stone shards, "
            "cold blue light bleeding through gaps, no solid body, hovering shard warrior shape, "
            "facing south, 32x32, transparent background, pixel art, dark fantasy, no background"
        ),
        "animations": [
            {
                "name"       : "idle",
                "out"        : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_S_idle.png",
                "frame_count": 4,
                "prompt"     : "pixel art shard creature hovering idle, shards slowly rotating, cold blue pulsing",
            },
            {
                "name"       : "walk",
                "out"        : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_S_walk.png",
                "frame_count": 6,
                "prompt"     : "pixel art shard creature moving forward, shards separating and rejoining",
            },
            {
                "name"       : "attack",
                "out"        : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_S_attack.png",
                "frame_count": 6,
                "prompt"     : "pixel art shard arm launches forward as projectile then snaps back",
            },
            {
                "name"       : "death",
                "out"        : ART_DIR / "dusmanlar/grunt_shard/grunt_shard_S_death.png",
                "frame_count": 6,
                "prompt"     : "pixel art shard creature all shards exploding outward simultaneously, cold blue burst",
            },
        ],
        "md_section": "SHARD WALKER",
    },

    "iron_warden": {
        "ref_image"  : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_gemini_base.png",
        "base_out"   : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_BASE.png",
        "canvas_size": 128,
        "base_prompt": (
            "top-down pixel art game sprite, massive iron golem guardian, heavily damaged dark plate armor, "
            "deep cracks with cold blue energy seeping through, broken sword shards embedded in shoulders, "
            "overwhelming dark fantasy boss, facing south, 128x128, transparent background, pixel art, no background"
        ),
        "animations": [
            {
                "name"       : "idle",
                "out"        : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_idle.png",
                "frame_count": 6,
                "prompt"     : "massive iron golem standing menacingly, very slow heavy breathing, cold blue glow pulsing",
            },
            {
                "name"       : "attack1",
                "out"        : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_attack1.png",
                "frame_count": 8,
                "prompt"     : "massive iron golem raising huge sword overhead slam, devastating ground shockwave on impact",
            },
            {
                "name"       : "charge",
                "out"        : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_charge.png",
                "frame_count": 8,
                "prompt"     : "massive iron golem slow windup then explosive shield charge, heavy collision",
            },
            {
                "name"       : "hurt",
                "out"        : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_hurt.png",
                "frame_count": 4,
                "prompt"     : "massive iron golem barely flinching from hit, minor stagger, shrugging off damage",
            },
            {
                "name"       : "death",
                "out"        : ART_DIR / "dusmanlar/boss/iron_warden/boss_iron_warden_S_death.png",
                "frame_count": 8,
                "prompt"     : "massive iron golem collapsing, armor cracking open, cold blue energy erupting then fading",
            },
        ],
        "md_section": "IRON WARDEN",
    },
}

NEGATİF = (
    "side view, front facing, 3/4 view, face visible, background, floor, "
    "shadow, blur, low quality, watermark, text, logo, realistic, 3D, "
    "deformed, extra limbs, bad anatomy"
)

# ─── PIPELINE YÜKLEYİCİ ──────────────────────────────────────────────────────

_pipes = {}

def load_base_pipe():
    if "base" in _pipes:
        return _pipes["base"]
    import torch
    from diffusers import StableDiffusionImg2ImgPipeline
    print("  Modeller yükleniyor (ilk seferde ~30 saniye)...")
    pipe = StableDiffusionImg2ImgPipeline.from_pretrained(
        "SG161222/Realistic_Vision_V5.1_noVAE",
        torch_dtype=torch.float16,
        cache_dir=str(HF_CACHE_DIR / "realistic_vision_v5"),
        safety_checker=None,
        requires_safety_checker=False,
    ).to("cuda")
    pipe.load_lora_weights(
        "artificialguybr/PixelArtRedmond15V2",
        weight_name="PixelArtRedmond15V2-PixelArt15V2-SDXLPAHD.safetensors",
        adapter_name="pixel_art",
    )
    pipe.set_adapters(["pixel_art"], adapter_weights=[0.85])
    pipe.enable_attention_slicing()
    _pipes["base"] = pipe
    return pipe

def load_anim_pipe():
    if "anim" in _pipes:
        return _pipes["anim"]
    import torch
    from diffusers import AnimateDiffPipeline, MotionAdapter, DDIMScheduler
    print("  AnimateDiff yükleniyor...")
    adapter = MotionAdapter.from_pretrained(
        "guoyww/animatediff-motion-adapter-v1-5-3",
        cache_dir=str(HF_CACHE_DIR),
        torch_dtype=torch.float16,
    )
    scheduler = DDIMScheduler.from_pretrained(
        "SG161222/Realistic_Vision_V5.1_noVAE",
        subfolder="scheduler",
        clip_sample=False,
        timestep_spacing="linspace",
        beta_schedule="linear",
        steps_offset=1,
        cache_dir=str(HF_CACHE_DIR / "realistic_vision_v5"),
    )
    pipe = AnimateDiffPipeline.from_pretrained(
        "SG161222/Realistic_Vision_V5.1_noVAE",
        motion_adapter=adapter,
        scheduler=scheduler,
        torch_dtype=torch.float16,
        cache_dir=str(HF_CACHE_DIR / "realistic_vision_v5"),
        safety_checker=None,
        requires_safety_checker=False,
    ).to("cuda")
    pipe.load_ip_adapter(
        "h94/IP-Adapter",
        subfolder="models",
        weight_name="ip-adapter-plus_sd15.bin",  # Plus = daha iyi karakter tutarlılığı
        cache_dir=str(HF_CACHE_DIR),
    )
    pipe.set_ip_adapter_scale(0.70)  # 0.6-0.8 arası; end_at=0.6 trick diffusers'da yok ama 0.70 dengeli
    pipe.load_lora_weights(
        "artificialguybr/PixelArtRedmond15V2",
        weight_name="PixelArtRedmond15V2-PixelArt15V2-SDXLPAHD.safetensors",
        adapter_name="pixel_art",
    )
    pipe.set_adapters(["pixel_art"], adapter_weights=[0.75])
    pipe.enable_attention_slicing()
    pipe.enable_vae_slicing()
    _pipes["anim"] = pipe
    return pipe

# ─── ÜRETİM FONKSİYONLARI ────────────────────────────────────────────────────

def generate_base(sprite_def):
    import torch
    from PIL import Image

    ref_path = sprite_def["ref_image"]
    out_path = sprite_def["base_out"]
    size = sprite_def["canvas_size"]

    if out_path.exists():
        print(f"  [ATLA] BASE sprite zaten var: {out_path.name}")
        return

    if not ref_path.exists():
        print(f"  [ATLA] Referans görsel bulunamadı: {ref_path}")
        print(f"         → tools/gemini_ref_generator.py çalıştır")
        return

    out_path.parent.mkdir(parents=True, exist_ok=True)
    print(f"  Referans: {ref_path.name}")

    ref_img = Image.open(ref_path).convert("RGB").resize((512, 512), Image.LANCZOS)

    pipe = load_base_pipe()
    result = pipe(
        prompt="p1x3l, " + sprite_def["base_prompt"],   # "p1x3l" = Pixelized Art v3 trigger
        negative_prompt=NEGATİF,
        image=ref_img,
        strength=0.60,          # 0.55-0.75 arası dene; düşük = referansa yakın
        guidance_scale=7.5,     # 7-9 arası; 9+ palette bozuyor
        num_inference_steps=25, # 25 topluluk sweet spot
        generator=torch.Generator("cuda").manual_seed(42),
    ).images[0]

    # Pixel art boyutuna küçült (NEAREST = piksel keskinliği korur)
    final = result.resize((size, size), Image.NEAREST)
    final = final.convert("RGBA")
    out_path.parent.mkdir(parents=True, exist_ok=True)
    final.save(out_path, "PNG")
    print(f"  [OK] BASE kaydedildi: {out_path}")

def generate_animation(sprite_def, anim_def):
    import torch
    from PIL import Image

    base_path = sprite_def["base_out"]
    out_path  = anim_def["out"]
    frames_n  = anim_def["frame_count"]
    size      = sprite_def["canvas_size"]

    if out_path.exists():
        print(f"    [ATLA] {anim_def['name']} sprite sheet zaten var")
        return

    if not base_path.exists():
        print(f"    [ATLA] BASE sprite yok, önce BASE üret")
        return

    out_path.parent.mkdir(parents=True, exist_ok=True)
    base_img = Image.open(base_path).convert("RGB").resize((512, 512), Image.LANCZOS)
    pipe = load_anim_pipe()

    prompt = (
        f"p1x3l, top-down pixel art game sprite animation, {anim_def['prompt']}, "
        f"pixel art, dark fantasy, transparent background, sprite sheet"
    )

    # Frames 16'yı geçmesin — sliding window öncesi topluluk sweet spot
    safe_frames = min(frames_n, 16)

    output = pipe(
        prompt=prompt,
        negative_prompt=NEGATİF,
        ip_adapter_image=base_img,
        num_frames=safe_frames,
        guidance_scale=7.5,     # 7-9 arası; Euler_a ile 7.5 iyi
        num_inference_steps=25, # min 25 (Inner-Reflections guide)
        generator=torch.Generator("cuda").manual_seed(42),
    )
    frames = output.frames[0]  # list of PIL images

    # Pixel art boyutuna küçült
    frames_small = [f.resize((size, size), Image.NEAREST).convert("RGBA") for f in frames]

    # Yatay sprite sheet birleştir
    sheet_w = size * len(frames_small)
    sheet   = Image.new("RGBA", (sheet_w, size), (0, 0, 0, 0))
    for i, frame in enumerate(frames_small):
        sheet.paste(frame, (i * size, 0))

    sheet.save(out_path, "PNG")
    print(f"    [OK] {anim_def['name']} ({frames_n} frame) → {out_path.name}")

def update_md_checkmark(section_name):
    """URETIM_PLANI.md'de ilgili bölüm başlığına ✅ ekler."""
    md_path = BASE_DIR / "ART" / "URETIM_PLANI.md"
    if not md_path.exists():
        return
    content = md_path.read_text(encoding="utf-8")
    # "## WARBLADE BASE Sprite" → "## ✅ WARBLADE BASE Sprite"
    updated = re.sub(
        rf"(## )(?!✅)({re.escape(section_name)})",
        r"\1✅ \2",
        content,
    )
    if updated != content:
        md_path.write_text(updated, encoding="utf-8")

# ─── ANA DÖNGÜ ────────────────────────────────────────────────────────────────

def run(only=None, anim_only=False):
    keys = [only] if only else list(SPRITES.keys())
    unknown = [k for k in keys if k not in SPRITES]
    if unknown:
        print(f"HATA: Bilinmeyen sprite: {unknown}")
        print(f"Geçerli seçenekler: {list(SPRITES.keys())}")
        sys.exit(1)

    if anim_only:
        print("MOD: --anim-only → BASE sprite'lar atlanıyor (PixelLab'dan gelmeli)")

    for key in keys:
        spr = SPRITES[key]
        print(f"\n{'='*50}")
        print(f"  {key.upper()}")
        print(f"{'='*50}")

        # BASE sprite (hybrid modda atla)
        if not anim_only:
            print(f"\n[BASE Sprite — {spr['canvas_size']}×{spr['canvas_size']}]")
            generate_base(spr)
        else:
            if not spr["base_out"].exists():
                print(f"\n⚠️  BASE sprite bulunamadı: {spr['base_out'].name}")
                print(f"   PixelLab'da üret ve şu yola kaydet:")
                print(f"   {spr['base_out']}")
                print(f"   Animasyonlar atlanıyor.\n")
                continue

        # Animasyonlar
        print(f"\n[Animasyonlar — {len(spr['animations'])} adet]")
        for anim in spr["animations"]:
            generate_animation(spr, anim)
            update_md_checkmark(f"{spr['md_section']} {anim['name'].upper()}")

        update_md_checkmark(f"{spr['md_section']} BASE Sprite")

    print("\n" + "="*50)
    print("Üretim tamamlandı!")
    print(f"Klasöre bak: {ART_DIR}")
    print("\nSonraki adım: Unity'de sprite'ları import et.")

# ─── GİRİŞ ────────────────────────────────────────────────────────────────────

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="RIMA Sprite Üretici")
    parser.add_argument("--only", choices=list(SPRITES.keys()), help="Sadece bu sprite'ı üret")
    parser.add_argument("--anim-only", action="store_true",
                        help="BASE sprite'ları atla, sadece animasyon üret (PixelLab hybrid modu)")
    args = parser.parse_args()

    try:
        import torch
        if not torch.cuda.is_available():
            print("UYARI: CUDA bulunamadı! CPU ile devam edilecek (çok yavaş).")
        else:
            print(f"GPU: {torch.cuda.get_device_name(0)}")
            print(f"VRAM: {torch.cuda.get_device_properties(0).total_memory / 1e9:.1f} GB")
    except ImportError:
        print("HATA: PyTorch yüklü değil. comfyui_install.bat çalıştır.")
        sys.exit(1)

    run(only=args.only, anim_only=args.anim_only)
