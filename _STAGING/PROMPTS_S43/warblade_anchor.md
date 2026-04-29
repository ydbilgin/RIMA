# Warblade — Anchor Forge Prompt (S43 v7)
# FAIL history: v1 chibi | v2 full plate | v3 demon knight | v4 Enhance demon knight | v5 chest glow+plate | v6 no sword+pauldrons (prompt too long → PixelLab lost sword)
# v7 FIX: shorter prompt, sword FIRST, pose explicit, critical negations only

## UI

| Alan | Değer |
|---|---|
| Direction | South |
| View | Low top-down |
| Detail | Low detailed |
| Outline | Single color outline |
| Width × Height | 128 × 128 |
| Transparent BG | ✓ |

## Description

```
Dark fantasy ARPG pixel art warrior. Male, late 20s, athletic muscular build, realistic proportions, not chibi. 30-35° low top-down view, facing directly south, both shoulders level.

HIS RIGHT HAND HOLDS A LARGE BROADSWORD. The sword is the most important element of this sprite. His right arm hangs relaxed at his side, the broadsword blade angled downward at 45°, tip near his right boot. The sword is large and clearly visible. Cold blue fractured energy cracks run across the blade irregularly. Left arm hangs loose at his side, empty hand.

He wears matte black leather: fitted chest layer, leather arms, leather trousers, leather boots. One flat grey metal breastplate on the sternum only. Two small close-fitting grey shoulder caps — small, not spiked. Dark iron bracers on forearms. Black leather belt.

Face: short dark brown hair, strong jaw, stubble, no helmet, face visible.

No chest glow. No glowing gem on torso. No cape. No big pauldrons. No full plate armor. Transparent background, no shadow.

Heavy dark single-color outline. Muted dark palette. Painterly weathered pixel shading.
```

## QC

1. Sağ elde broadsword görünür ✓
2. Blade 45° aşağı, tip sağ boot yanında ✓
3. Leather dominant, single breastplate ✓
4. Chest glow yok ✓
5. Small shoulder caps (büyük pauldron yok) ✓
6. Face görünür ✓

## Sonraki adım (PASS)
1. `_STAGING/anchors/warblade/warblade_128_v7.png` kaydet
2. Create Character → Reference Standard
