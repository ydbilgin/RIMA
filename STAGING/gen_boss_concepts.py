import base64
import pathlib

import openai


BOSS_DETAIL = (
    "Canonical Act 1 boss: The Penitent Sovereign, a former guardian bound by "
    "chains, with a hunched stone body, glowing rift energy in the chest, chain "
    "anchor motifs, and readable boss-fight telegraphs."
)

PROMPT_1 = f"""
{BOSS_DETAIL}
Top-down 2D pixel art, cute chibi style, boss fight room in a fantasy dungeon.
Rounded stone floor tiles in warm beige/cream tones. Simple rounded pillar props in corners.
Cute glowing cyan cracks on floor as decoration. Two small chibi hero characters (one sword warrior, one mage with orb) face a large cute stone golem boss in center.
Boss is big, round-shaped, friendly-scary design with soft chain details and cyan rift glow. Soft color palette, readable silhouettes.
64x64 tile grid visible. No dark gritty textures. Stardew Valley / Undertale aesthetic influence.
""".strip()

PROMPT_2 = f"""
{BOSS_DETAIL}
2D pixel art, cute chibi top-down perspective (85-90 degree camera), boss fight encounter closeup.
Large cute stone golem boss (round body, glowing cyan eyes, simple chunky stone arms, soft broken chains, tiny rift glow in chest) looming over small chibi Warblade hero with tiny sword.
Floor: warm cream stone tiles with simple rounded edges. Props: cute small barrels, round glowing crystals, tiny skull decorations.
Soft lighting, warm orange torch glow + cool cyan boss glow.
Readable shapes at small pixel scale. Friendly-scary tone, not grimdark.
""".strip()

PROMPT_3 = f"""
{BOSS_DETAIL}
2D pixel art top-down view, cute chibi style, epic boss fight moment.
3 chibi heroes (sword warrior, mage with orb, hooded ranger) surround a large cute stone golem boss.
Boss has simple AoE attack pattern shown with cute rounded ground marker circles in soft red/orange, plus soft chain and rift motifs.
Room props: round pillars, glowing cyan rune tiles, cute treasure chests pushed to corners.
Color palette: warm stone beige + cyan accent + soft orange torch.
Clean readable silhouettes. Fantasy roguelite, Hades-meets-Stardew visual tone.
""".strip()

IMAGES = [
    ("boss_concept_01_room_overview.png", PROMPT_1),
    ("boss_concept_02_boss_closeup.png", PROMPT_2),
    ("boss_concept_03_party_action.png", PROMPT_3),
]


def main() -> None:
    client = openai.OpenAI()
    staging_dir = pathlib.Path("STAGING")
    staging_dir.mkdir(exist_ok=True)

    for filename, prompt in IMAGES:
        response = client.images.generate(
            model="gpt-image-1",
            prompt=prompt,
            size="1024x1024",
            quality="medium",
        )
        img_data = base64.b64decode(response.data[0].b64_json)
        out_path = staging_dir / filename
        out_path.write_bytes(img_data)
        print(f"Saved: {out_path.as_posix()}")


if __name__ == "__main__":
    main()
