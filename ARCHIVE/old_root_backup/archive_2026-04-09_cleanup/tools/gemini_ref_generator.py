#!/usr/bin/env python3
"""
RIMA — Gemini Referans Görsel Üretici
======================================
Faz 0 + Faz 1 için tüm referans görselleri Gemini API ile üretip doğru klasörlere kaydeder.
Mevcut dosyaları atlar — istediğin zaman yeniden çalıştırabilirsin.

Kurulum:
  pip install google-genai pillow

API Key:
  https://aistudio.google.com/apikey adresinden ücretsiz alabilirsin.
  Oraya gir → "Create API key" → kopyala → aşağıdaki API_KEY değişkenine yaz.

Çalıştırma:
  cd "F:/Antigravity Projeler/2d roguelite"
  python tools/gemini_ref_generator.py

Model seçimi:
  - "imagen-3.0-generate-002" : En iyi kalite, Google AI Studio'da ücretsiz (günlük limit var)
  - "imagen-4.0-generate-preview-05-20" : Yeni model, deneysel
  Sorun çıkarsa MODEL satırını değiştir.
"""

import os
import sys
import time
from pathlib import Path

# ─── AYARLAR ──────────────────────────────────────────────────────────────────

API_KEY = "BURAYA_API_KEY_YAZ"   # aistudio.google.com/apikey → Create API key
MODEL   = "imagen-3.0-generate-002"

BASE_DIR = Path(r"F:\Antigravity Projeler\2d roguelite")
ART_DIR  = BASE_DIR / "ART"

# ─── ÜRETILECEK GÖRSELLER ─────────────────────────────────────────────────────
# (prompt, çıktı yolu)  — yol ART_DIR'e göreli

ASSETS = [

    # ══════════════════════════════════════════════
    # FAZ 0 — Logo + Placeholder
    # ══════════════════════════════════════════════

    (
        "logo/rima_logo_kaynak.png",
        """A dark fantasy pixel art game logo. The large uppercase letters "RIMA" are written
boldly in dark steel color, spanning the upper portion of the image.
Below and to the right of the letter "I", the small lowercase letters "ft" hang
downward at an angle — as if they cracked off from the word RIFT and are drooping,
falling debris from a broken seal. They are smaller, slightly rotated, cracked in texture.
Below and to the right of the letter "A", the small lowercase letters "rch" hang
downward at the same angle — as if they cracked off from the word MARCH, same drooping effect.
At the exact break points where "ft" detached from "RI" and "rch" detached from "MA",
brilliant gold light bleeds through the crack, color #FFD700.
Letter color: dark steel #1E1E32. Background: void black #080808.
The hidden full words RIFT and MARCH become readable on closer inspection.
Pixel art logo style, high contrast, kintsugi dark aesthetic."""
    ),

    (
        "placeholder/warblade_placeholder.png",
        """Simple pixel art placeholder sprite, dark blue #1E1E32 filled rectangle,
large gold letter W centered #FFD700, transparent background, 64x64 game sprite"""
    ),

    (
        "placeholder/grunt_placeholder.png",
        """Simple pixel art placeholder sprite, dark red #3D0000 filled square,
white letter G centered, transparent background, 32x32 game sprite"""
    ),

    (
        "placeholder/floor_placeholder.png",
        """Simple pixel art tile placeholder, dark grey stone #1E1E32,
thin diagonal crack line #2A2A4A, 16x16 game tile"""
    ),

    (
        "placeholder/wall_placeholder.png",
        """Simple pixel art tile placeholder, near-black #080808 stone tile
with single horizontal dark line #1E1E32, 16x16 game wall tile"""
    ),

    # ══════════════════════════════════════════════
    # FAZ 1 — Warblade (Ana Karakter)
    # ══════════════════════════════════════════════

    (
        "karakterler/warblade/warblade_gemini_base.png",
        """A heavily armored warrior viewed strictly from directly above, bird's eye aerial
top-down perspective — as if a camera is mounted on the ceiling looking straight down.
The warrior holds a large greatsword in their right hand, blade pointing downward
at their side. Their dark iron plate armor has a crack across the chest with cold
blue light bleeding through. Short torn cape visible behind the shoulders.
Heavy angular pauldrons visible from above. Head small at top, wide shoulders,
no face visible — only the top of the helmet. Feet barely visible or hidden.
The art style is retro pixel art, limited color palette: dark iron grey, cold blue,
worn gold. Transparent background, no background elements."""
    ),

    # ══════════════════════════════════════════════
    # FAZ 1 — Shard Walker (Act 1 Grunt)
    # ══════════════════════════════════════════════

    (
        "dusmanlar/grunt_shard/grunt_shard_gemini_base.png",
        """A humanoid enemy assembled entirely from floating broken stone shards and bone fragments.
There is no solid body — only hovering pieces arranged in a vague warrior shape,
with cold blue light bleeding through the gaps between the shards.
Viewed from directly above. The art style is retro pixel art with a dark stone
and cold blue color palette. The background must be transparent."""
    ),

    # ══════════════════════════════════════════════
    # FAZ 1 — Void Thrall (Act 1 Grunt)
    # ══════════════════════════════════════════════

    (
        "dusmanlar/grunt_thrall/grunt_thrall_gemini_base.png",
        """A dark armored humanoid soldier stands menacingly. Running vertically through
the center of their chest is a deep glowing crack, with void purple energy
seeping out from the fissure. The overall silhouette is sinister and medieval.
Viewed from directly above. The art style is retro pixel art with a dark iron
and void purple color palette. The background must be transparent."""
    ),

    (
        "dusmanlar/grunt_thrall/grunt_thrall_gemini_half_left.png",
        """Pixel art sprite, top-down 2D dark fantasy game, transparent background,
LEFT HALF ONLY of a dark armored soldier, cleanly split vertically down the center,
void purple energy glowing at the split edge #9E4FE0, still aggressive, facing south"""
    ),

    (
        "dusmanlar/grunt_thrall/grunt_thrall_gemini_half_right.png",
        """Pixel art sprite, top-down 2D dark fantasy game, transparent background,
RIGHT HALF ONLY of a dark armored soldier, cleanly split vertically down the center,
void purple energy glowing at the split edge #9E4FE0, still aggressive, facing south"""
    ),

    # ══════════════════════════════════════════════
    # FAZ 1 — Iron Warden (Act 1 Boss)
    # ══════════════════════════════════════════════

    (
        "dusmanlar/boss/iron_warden/boss_iron_warden_gemini_base.png",
        """A massive iron golem guardian towers over the battlefield. Its full dark plate
armor is heavily damaged, covered in deep cracks from which cold blue energy
seeps through. Several broken sword shards are embedded in its back and shoulders
like trophies. The figure radiates an overwhelming, slow, unstoppable presence.
Viewed from directly above. The art style is retro pixel art with a dark iron
and cold blue color palette. The background must be transparent."""
    ),

    # ══════════════════════════════════════════════
    # FAZ 1 — Tile Referansları (opsiyonel)
    # ══════════════════════════════════════════════

    (
        "tiles/act1/act1_floor_gemini_ref.png",
        """Pixel art tile, 16x16 pixels, seamless tileable, top-down 2D dark fantasy
dungeon floor, dark grey cracked cobblestone #1E1E32 with thin cold blue
glowing fissures #7BA7BC, dungeon atmosphere, transparent background,
3 slight variations of the same tile design"""
    ),

    (
        "tiles/act1/act1_wall_gemini_ref.png",
        """Pixel art tile, 16x32 pixels, seamless tileable, top-down dark fantasy game
wall, crumbling fortress stone, cold blue barely visible in cracks,
dark stone texture #080808 #1E1E32, game wall tile, transparent background"""
    ),

    (
        "tiles/act1/act1_crack_gemini_ref.png",
        """Pixel art overlay sprite, 16x16 pixels, transparent background,
thin crack line patterns only — no fill, 1-2 pixel wide,
cold blue glow color #7BA7BC, dark fantasy floor decoration overlay,
4 different crack angle variations, game decor sprite"""
    ),

]

# ─── ÜRETIM ───────────────────────────────────────────────────────────────────

def main():
    if API_KEY == "BURAYA_API_KEY_YAZ":
        print("HATA: API_KEY ayarlanmamış!")
        print("  1. https://aistudio.google.com/apikey adresine git")
        print("  2. 'Create API key' tıkla")
        print("  3. Bu dosyada API_KEY = '...' satırına yapıştır")
        sys.exit(1)

    try:
        from google import genai
        from google.genai import types
    except ImportError:
        print("HATA: google-genai paketi yüklü değil.")
        print("  pip install google-genai")
        sys.exit(1)

    client = genai.Client(api_key=API_KEY)

    toplam   = len(ASSETS)
    atlanan  = 0
    uretilen = 0
    hatali   = 0

    print(f"\nRIMA Referans Görsel Üretici")
    print(f"Model: {MODEL}")
    print(f"Toplam asset: {toplam}")
    print("=" * 50)

    for i, (rel_path, prompt) in enumerate(ASSETS, 1):
        out_path = ART_DIR / rel_path
        out_path.parent.mkdir(parents=True, exist_ok=True)

        if out_path.exists():
            print(f"[{i:02d}/{toplam}] ATLA (mevcut): {rel_path}")
            atlanan += 1
            continue

        print(f"[{i:02d}/{toplam}] Üretiliyor: {rel_path}")
        print(f"         Prompt: {prompt[:80].strip()}...")

        try:
            response = client.models.generate_images(
                model=MODEL,
                prompt=prompt,
                config=types.GenerateImagesConfig(
                    number_of_images=1,
                    aspect_ratio="1:1",
                    output_mime_type="image/png",
                    include_rai_reason=True,
                ),
            )

            if not response.generated_images:
                print(f"         UYARI: Görsel üretilemedi (içerik filtresi?)")
                hatali += 1
                continue

            img_bytes = response.generated_images[0].image.image_bytes
            out_path.write_bytes(img_bytes)
            print(f"         ✓ Kaydedildi: {out_path}")
            uretilen += 1

            # Rate limit — Imagen 3 ücretsiz tier: 3 istek/dakika
            if i < toplam:
                time.sleep(22)

        except Exception as e:
            print(f"         HATA: {e}")
            hatali += 1
            # API hatası: 5 saniye bekle, devam et
            time.sleep(5)

    print("\n" + "=" * 50)
    print(f"Tamamlandı!")
    print(f"  Üretilen : {uretilen}")
    print(f"  Atlanan  : {atlanan} (zaten mevcuttu)")
    print(f"  Hatalı   : {hatali}")
    print(f"\nŞimdi ne yapacaksın:")
    print(f"  1. ART klasörünü aç, görselleri incele")
    print(f"  2. Açı yanlışsa: Gemini web'de tek tek düzelt")
    print(f"  3. Sonra Aseprite + PixelLab workflow'una geç (URETIM_PLANI.md)")


if __name__ == "__main__":
    main()
