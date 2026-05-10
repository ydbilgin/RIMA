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

## Anchor Referans (Adim 1 -- Baslangic Noktasi)

Weaponless base body URETME. Bu sinifin standing pose anchor'i zaten hazir:

  Characters/anchors/shadowblade/rotations/south.png
  Characters/anchors/shadowblade/rotations/south-east.png
  Characters/anchors/shadowblade/rotations/east.png
  Characters/anchors/shadowblade/rotations/north-east.png
  Characters/anchors/shadowblade/rotations/north.png
  Characters/anchors/shadowblade/rotations/north-west.png
  Characters/anchors/shadowblade/rotations/west.png
  Characters/anchors/shadowblade/rotations/south-west.png

Bu dosyalar silahini tutan, duran poz karakteri icerir -- temiz arka plan, uretim hazir.
Tum animasyonlarin start frame kaynagi bu anchor dosyalaridir.

Edit Image Pro kullanim durumlari:
- Anchor'dan uretilemayan asiri pozlar icin (walk extreme pose A/B, attack windup) use anchor as source image in Edit Image Pro to produce variant
- Animasyon start frame ve end frame'i yeni uretimden gelecekse: Edit Image Pro + anchor source
- Weapon zaten anchor'da var; ayri weapon pass GEREKMIYOR

## Idle (Adım 2)

```
Predator stillness, 6-8 frames. Very subtle motion — only cloak hem flutters slightly, head turns by 5-10° to scan. Body stays low and tense. twin short blades in both hands, low guard ready. Less obvious breathing than other classes — assassin stillness.
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
Walking forward predator-style, low and silent, body crouched, knees bent slightly, weight always on balls of feet. Cloak sways behind. twin short blades in both hands, low guard ready. South-facing. Subtle compared to Warblade — less stride distance, more compressed posture.
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

## Edit Image Pro -- Yeni Frame Uretimi (Adim 5, ihtiyac varsa)

Anchor standing pose yeterli degilse (attack PEAK, walk extreme) yeni frame uret:
1. Source image: ilgili yon anchor dosyasi (Characters/anchors/shadowblade/rotations/south.png)
2. Prompt: istenen poz degisikligini describe et, weapon zaten source'da gorunur
3. Output: direction_variant.png olarak kaydet, eraser pass uygula

Weapon ayrica ekleme -- anchor'da zaten var.

## Notes

- **Echo system primary class** — Shadow Echo havuzu cyan #00FFCC iken Shadowblade kendi violet #5A2A8A kullanır (renk dili ayrımı LOCKED)
- BasicAttackProfile_Shadowblade.asset = VeilStrike strategy
- Embedded glow YASAK — phase/teleport efektleri engine-side particle ile
- Scar Memory mekaniği için Hurt anim 3 frame net hissedilmeli
