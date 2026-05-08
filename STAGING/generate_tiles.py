import os, base64
from pathlib import Path
from openai import OpenAI

client = OpenAI()

RAW_DIR = Path("STAGING/tiles_raw")
RAW_DIR.mkdir(parents=True, exist_ok=True)

batches = [
    {
        "name": "f2",
        "prompt": "Create a pixel art isometric dungeon floor tile sprite sheet.\n\nLAYOUT: 16 tiles in a 4 columns x 4 rows grid, zero gaps between tiles, perfectly equal sizing.\n\nBACKGROUND: Solid magenta #FF00FF everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat magenta only.\n\nTILE STYLE (apply identically to ALL tiles):\n- Isometric diamond/rhombus shape, 2:1 width-to-height ratio\n- Dark medieval dungeon stone floor, deep blue-grey tone — SAME stone as F1 base floor\n- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing\n- Consistent top-left light source: top face slightly lighter, bottom edges thin dark strip\n- No tile surface brighter than mid-grey, no tile surface darker than #151820\n- All tiles share identical stone color, lighting direction, pixel art style, and diamond proportions\n- Cracks are the DOMINANT feature — stone identity reads first, then damage detail\n\nTILES:\n1). deep diagonal crack corner to corner, crack #0a0a14 2px wide\n2). spiderweb fracture radiating from center, 4-5 crack branches #0a0a14\n3). two parallel longitudinal cracks, one block slightly displaced\n4). crack runs through mortar joint and continues across adjacent block\n5). heavy fracture, 2px gap, edge of block offset 1px suggesting collapse\n6). crack with faint white mineral deposit along edge #1e2030\n7). multiple micro-cracks forming a network across two block faces\n8). corner rubble: 4-6 loose debris pixels #14141e at diamond edge\n9). single deep crack, dark shadow inside #080810, runs half tile\n10). mortar joint fully cracked out, raw block edge exposed\n11). diagonal crack plus one small debris cluster at terminus\n12). heavy crack with block inset 1px, depression in surface\n13). crack pattern radiates from corner across full block face\n14). two crossing cracks, X pattern, both cracks #0a0a14\n15). crack with dark green mineral seepage trace #1a2810 along one side\n16). worst damage: heavy crack network, 4+ cracks, debris at two corners"
    },
    {
        "name": "f3",
        "prompt": "Create a pixel art isometric dungeon floor tile sprite sheet.\n\nLAYOUT: 12 tiles in a 4 columns x 3 rows grid, zero gaps between tiles, perfectly equal sizing.\n\nBACKGROUND: Solid magenta #FF00FF everywhere outside the diamond shapes. No transparency, no feathering, no anti-aliasing at edges — pure flat magenta only.\n\nTILE STYLE (apply identically to ALL tiles):\n- Isometric diamond/rhombus shape, 2:1 width-to-height ratio\n- Dark medieval dungeon stone floor, deep blue-grey tone — SAME stone as F1 base floor\n- Hard pixel edges, visible pixel grain, zero smooth gradients, zero anti-aliasing\n- Consistent top-left light source\n- Moss and organic growth are the DOMINANT feature, always desaturated dark green tones\n- Moss color range: #1a2810 (deep shadow) to #263a1a (lit edge) — never bright green\n\nTILES:\n1). dark moss colony covering 20% of one block face, irregular pixel blob 5-7px\n2). moss in mortar joints only, thin dark line #1a2810 tracing joint pattern\n3). moss on two adjacent block faces, spreading pattern\n4). heavy moss on one full block face, 40% coverage with #263a1a edge accent\n5). moss patch at one diamond corner edge, organic blob shape\n6). pale mineral stain ring, dried moisture #1e2030 faint circle\n7). moisture sheen on full surface, wet stone — subtle specular line #2a2c40\n8). fungal growth: pale dots cluster #1e1e2e, 3-4px each, scattered on one block\n9). moss plus hairline crack, organic growth in crack\n10). water pooling corner: darkened diamond edge, moisture gradient\n11). mold bloom: dark irregular patch #141414 with faint organic texture\n12). most organic: moss AND stain AND moisture, surface richly textured"
    },
    {
        "name": "w1",
        "prompt": "Create a pixel art isometric dungeon wall tile sprite sheet.\n\nLAYOUT: 12 tiles in a 4 columns x 3 rows grid, zero gaps, perfectly equal sizing. Each tile is TALLER than it is wide (portrait rectangle, not diamond).\n\nBACKGROUND: Solid magenta #FF00FF everywhere outside the wall shapes. No transparency, no feathering — pure flat magenta only.\n\nWALL SHAPE AND GEOMETRY:\n- Each tile shows the FRONT FACE of an isometric stone wall block\n- The wall block has: a narrow TOP surface (thin strip across the top, ~8px), and a tall FRONT FACE (main visible area below the top strip)\n- Top strip: lighter stone #3a3c50 — lit from top-left\n- Front face: dark medieval dungeon stone, same palette as floor #1e2030 and #262838, mortar joints #161620 1px\n- Left edge shadow strip: 4px wide, #12141a\n- Right edge: same stone as front, slightly darker than center\n- Bottom edge: #12141a 1px base shadow\n\nTILE STYLE:\n- Hard pixel edges, pixel art style, zero anti-aliasing, zero smooth gradients\n- Consistent top-left light source across ALL tiles\n- Same stone identity as floor (F1): blue-grey dungeon stone, cold muted palette\n- Mortar joints visible on front face, horizontal and vertical grid pattern\n- Stone blocks 2x2 visible on front face (same masonry as floor viewed from side)\n\nTILES:\n1). plain stone wall, clean masonry, no damage\n2). single horizontal crack on front face mid-height #0a0a14\n3). vertical crack runs from top strip down to base\n4). two parallel horizontal cracks, one upper one lower\n5). chipped upper-right corner, 3-4 debris pixels at top\n6). mortar joint deeper than usual on one horizontal line, recessed shadow\n7). scorch mark: dark carbon smear #0e0e0e on lower front face\n8). single iron bracket/ring mounted on front face, dark metal #181820\n9). moss growing in horizontal mortar joint, #1a2810 thin line\n10). moisture stain running vertically from top to base, mineral streak\n11). chipped lower-left corner, stone debris at base 3-5 pixels\n12). diagonal crack from upper-right toward lower-left across front face"
    },
    {
        "name": "w2",
        "prompt": "Create a pixel art isometric dungeon wall tile sprite sheet.\n\nLAYOUT: 8 tiles in a 4 columns x 2 rows grid, zero gaps, perfectly equal sizing. Each tile is portrait rectangle (taller than wide).\n\nBACKGROUND: Solid magenta #FF00FF everywhere outside the wall shapes. No transparency, no feathering — pure flat magenta only.\n\nWALL SHAPE: Same isometric wall block as W1 — top strip #3a3c50, front face #1e2030/#262838, left shadow strip #12141a, mortar joints #161620.\n\nTILE STYLE: Same as W1 but HEAVY DAMAGE is the dominant feature.\n\nTILES:\n1). deep crack network: 3+ cracks crossing on front face #0a0a14\n2). collapsed upper corner: top strip edge crumbled, 5-7 debris pixels\n3). spiderweb fracture radiating from mortar joint intersection\n4). partial block displacement: one stone block protrudes 2px from face\n5). heavy scorch mark covering 40% of front face, smoke stain #0e0e0e\n6). crack with gap: 2px separation, dark void inside #080810\n7). moss plus crack combination: organic growth IN the crack line\n8). worst damage: multiple deep cracks, debris at base, top strip partially broken"
    }
]

for batch in batches:
    name = batch["name"]
    out_path = RAW_DIR / f"{name}_raw.png"
    print(f"Generating {name}...")

    response = client.images.generate(
        model="gpt-image-1",
        prompt=batch["prompt"],
        size="1024x1024",
        quality="high"
    )

    img_data = response.data[0].b64_json
    img_bytes = base64.b64decode(img_data)
    with open(out_path, "wb") as f:
        f.write(img_bytes)
    print(f"  Saved: {out_path}")

print("All batches generated.")
