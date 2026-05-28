import base64
import pathlib

import openai


client = openai.OpenAI()

images = [
    (
        "opentd_concept_01_arena_overview.png",
        """Pure top-down 2D pixel art, 90-degree overhead view, boss fight arena. Large open stone floor with subtle tile grid pattern, warm granite beige tones with faint cyan glowing cracks. Walls are minimal - just thin dark edge border/shadow strip at room perimeter, no tall wall sprites needed. Small chibi hero characters (sword warrior, mage with orb) face a large stone golem boss in open center. Floor props: rune circles, small scattered stones, glowing floor crystals. Style: clean readable top-down, Hades-inspired open arena. No isometric angle, pure overhead.""",
    ),
    (
        "opentd_concept_02_floor_detail.png",
        """Pure top-down overhead pixel art, 2D roguelite boss room close-up. Detailed stone floor tiles visible from directly above - cracked granite texture, cyan magical rune inscriptions baked into floor. Thin wall shadow border at edges only. Boss (large stone golem, seen from directly above - round chunky top silhouette) surrounded by 2 chibi heroes. AoE ground markers as concentric circles on floor. Torch shadow pools on floor. No perspective distortion. Clean shapes, readable at small scale. Diablo 1 / Hades visual reference.""",
    ),
    (
        "opentd_concept_03_wallless_arena.png",
        """Top-down 2D pixel art, completely open boss arena with zero wall sprites - boundary defined only by floor tile cutoff and darkness beyond. Stone floor tiles in center, fade to black void at edges (no wall needed, just darkness). Chibi Warblade hero (small, sword raised) vs large stone golem boss (top-down silhouette, glowing cyan eyes visible from above). Scattered floor props: broken column tops seen from above (circles), rune stones, candle pools. Style: minimal, elegant, like Hades arena floor. Pixel art 32px grid.""",
    ),
]

for filename, prompt in images:
    response = client.images.generate(
        model="gpt-image-1",
        prompt=prompt,
        size="1024x1024",
        quality="medium",
    )
    img_data = base64.b64decode(response.data[0].b64_json)
    out_path = pathlib.Path("STAGING") / filename
    out_path.write_bytes(img_data)
    print(f"Saved: {out_path}")
