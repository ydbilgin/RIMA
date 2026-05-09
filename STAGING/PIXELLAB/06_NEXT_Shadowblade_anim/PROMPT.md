# Shadowblade — Class-Specific Prompts
*Faz 2, asimetrik (4 yön ayrı), VeilStrike teleport-strike*

## Class Identity

| Field | Value |
|---|---|
| Type | **Asimetrik** (S, E, N, W ayrı üret) |
| Weapon | Twin short blades (her elde 1) |
| Accent Color | **Void purple** (#5A2A8A / #8A4ABA — Shadowblade kendi violet) |
| Yasak | Embedded glow karakter sprite'ında — VFX engine-side |
| Basic Attack | VeilStrike (3-hit chain, son hit teleport-finisher) |
| RMB | Shadow Step / phase strike |
| Silhouette | İnce siluet, hooded, dark cloak, low profile |

## Base Body Prompt (Adım 1, body-only, silahsız)

```
Pixel art shadowblade assassin character, body-only, no weapon, 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Slim agile build, full dark hooded cloak, body almost entirely silhouetted in dark with violet undertones. Palette: cloak black-purple #1A0E2A / #2A1A3A, mid #3A2A4E, accent violet #5A2A8A, skin partial visible #C9A084 (only chin and jawline below hood), leather straps #3A2818. Crouched ready stance, body lean forward, weight on balls of feet. Hood deep — no eyes visible (silhouette only). NO weapon, NO embedded glow. South-facing default. Hard pixel edges.
```

Yönler: **S, E, N, W** ayrı üret (asimetrik).

## Idle (Adım 2)

```
Predator stillness, 6-8 frames. Very subtle motion — only cloak hem flutters slightly, head turns by 5-10° to scan. Body stays low and tense. No weapon. Less obvious breathing than other classes — assassin stillness.
```

## Hurt (Adım 2)

```
Sharp recoil, 3 frames. Body twists hard from impact, cloak flares dramatically. Violet accent (#5A2A8A) flashes on cloak edge. Frame 1: idle. Frame 2: peak recoil (cloak full flare, body 60° twist). Frame 3: recovery.
```

## Death (Adım 2)

```
Dissolve / sink, 6 frames. Character collapses but with shadowy fade — last 2 frames show partial dissolution into ground (cloak fades to silhouette). Frame 1: stagger. Frame 2: knees buckle. Frame 3: lying on side. Frame 4: cloak fades (alpha 70%). Frame 5: only silhouette remains (alpha 40%). Frame 6: faint shadow stain on ground (alpha 20%).
```

## Walk (Adım 3, Extreme Pose)

```
Walking forward predator-style, low and silent, body crouched, knees bent slightly, weight always on balls of feet. Cloak sways behind. No weapon. South-facing. Subtle compared to Warblade — less stride distance, more compressed posture.
```

## Attack_LMB — Twin Blade Chain (Adım 4, 3-segment)

**PEAK frame (1st hit):**
```
Right blade slash horizontal, arm extended at full slash, body 30° twisted. Left arm pulled back ready for next strike. Violet accent at blade trail (#5A2A8A streak). Body fully committed forward, weight on front foot.
```

**START → PEAK:** 4 frame (blade raised, arm cocked)
**PEAK → END:** 4 frame (slash through, recovery, left arm prepares for chain follow-up)

## Attack_RMB — Shadow Step / VeilStrike (Adım 4, 3-segment)

**PEAK frame:**
```
Phase-strike — character mid-teleport, body half-dissolved into shadow, blade emerging at target side with violet streak (#5A2A8A intense). Anticipation pose: original location shows fading silhouette, peak position shows blade impact frame. Two visual elements in single frame — shadow trail behind, character at strike point.
```

**START → PEAK:** 4 frame (crouch, body fades into shadow at original spot)
**PEAK → END:** 4 frame (full body re-materializes at new position, blade lowering, recovery stance)

## Dash (Adım 4, 4 frame)

```
Shadow dash, 4 frames. Frame 1: crouch with violet wisp at feet. Frame 2: body partially dissolves, blur streak forward. Frame 3: body re-forms ahead, mid-arrival. Frame 4: landing crouch, blades ready. Cloak trails violet smoke (#5A2A8A faint, max 4px wisp).
```

## Weapon Pass (Adım 5, Edit Image Pro)

```
Add twin short blades — one per hand. Each blade: ~0.6x character height, narrow silhouette, dark steel #4A4E5A / #5C6070, hilt wrapped black-violet (#1A0E2A / #5A2A8A). Curved or straight short-sword profile. Apply per direction: S, E, N, W (each painted separately).
```

## Notes

- **Echo system primary class** — Shadow Echo havuzu cyan #00FFCC iken Shadowblade kendi violet #5A2A8A kullanır (renk dili ayrımı LOCKED)
- BasicAttackProfile_Shadowblade.asset = VeilStrike strategy
- Embedded glow YASAK — phase/teleport efektleri engine-side particle ile
- Scar Memory mekaniği için Hurt anim 3 frame net hissedilmeli
