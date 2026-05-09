# Elementalist — Class-Specific Prompts
*Faz 2, asimetrik (4 yön ayrı), silahsız (büyü el jestleriyle)*

## Class Identity

| Field | Value |
|---|---|
| Type | **Asimetrik** (S, E, N, W ayrı üret — el jestleri tek tarafta belirgin) |
| Weapon | **YOK** (silahsız) — Adım 5 weapon pass ATLA |
| Accent Color | **Element bazlı** — varsayılan idle: cool neutral #B8C8D0; skill aktifken Fire/Frost/Lightning/Light spesifik |
| Yasak | Void energy (Shadowblade'in mor'u), kitap/odiyak (silahsızlığı vurgula — el jestleri ile büyü) |
| Basic Attack | CastRhythm (cast → channel → release pattern) |
| RMB | Element-based ranged spell (placement) |
| Silhouette | Robe + büyücü duruşu, eller ön planda (büyü için) |

## Base Body Prompt (Adım 1, body-only, silahsız)

```
Pixel art elementalist mage character, body-only, no weapon, NO book, NO staff (hands free for spell gestures), 128x128 sprite on 252x252 canvas. High top-down view 30-35°. Long flowing robe, hood NOT up (face visible — confident mage), short hair. Robe palette: deep blue-grey #2A3848 / #3E4C5E / #525E74 (cool neutral default — element accent only on spell anims). Trim accent: faint cool #B8C8D0. Sash at waist #3A2818 leather. Skin #C9A084. Body pose: slightly forward, hands held at chest level, palms angled outward (ready to cast). Robe hem sways. NO weapon, NO held object. South-facing default. Hard pixel edges, no anti-aliasing.
```

Yönler: **S, E, N, W** ayrı üret (asimetrik — el pozisyonları farklı).

## Idle (Adım 2)

```
Subtle channeling stance, 6-8 frames. Hands hover at chest, fingers slightly curl and uncurl as if shaping invisible energy. Robe hem sways. Hair barely moves. Confident mage breathing.
```

## Hurt (Adım 2)

```
Stagger backwards, 3 frames. Mage recoils, hands pulled to chest defensively, robe flares from sudden motion. Frame 1: idle. Frame 2: peak recoil (body bent backward 30°, hands up). Frame 3: recovery stance.
```

## Death (Adım 2)

```
Knees buckle slowly, 6 frames. Mage falls to knees first (caster's last stand), then forward. Robe billows. Frame 1: stagger. Frame 2: knees give. Frame 3: kneeling. Frame 4: hands fall to floor. Frame 5: forward fall. Frame 6: prone.
```

## Walk (Adım 3, Extreme Pose)

```
Walking forward mage gait, robe trailing behind, smooth deliberate step (not as quick as Ranger, not as heavy as Warblade). Hands held loosely at sides or chest. No weapon. South-facing.
```

## Attack_LMB — CastRhythm Spell (Adım 4, 3-segment)

**PEAK frame:**
```
Both hands extended forward, palms outward, fingers spread — peak cast moment. Element energy gathers at palms (small VFX placeholder area, ~12x12px each, intentionally blank for engine VFX overlay). Body lean slightly forward, weight balanced. NO embedded glow on character — only blank palm zones marked for engine particle. Confident mage release pose.
```

**START → PEAK:** 4 frame (hands rise from chest, palms shape gathering motion)
**PEAK → END:** 4 frame (hands lower, energy released, return to ready)

> **VFX-place tıklanmaz:** Karakter sprite'ında element rengi YOK. CastRhythm VFX engine-side (cast → channel → release pattern). Sprite sadece pose verir, rengi VFX overlay sağlar.

## Attack_RMB — Element Ranged Spell (Adım 4, 3-segment)

**PEAK frame:**
```
One hand thrust forward (right hand), palm out, the other hand pulled back at hip channeling. Body twisted 30° to support thrust. Robe billows from cast force. Blank palm zone for VFX (16x16px on extended hand). NO embedded glow.
```

**START → PEAK:** 4 frame (anticipation — hands gather)
**PEAK → END:** 4 frame (released, hand drops, recovery)

## Dash (Adım 4, 4 frame)

```
Mage step / blink, 4 frames. Frame 1: crouch with hand gesture (palm down at side, channeling). Frame 2: body briefly distorts (motion blur with robe streak forward). Frame 3: body re-forms ahead, robe still trailing. Frame 4: landing pose, hands return to chest. NO embedded glow — engine VFX adds element trail per active element.
```

## Weapon Pass (Adım 5)

**❌ ATLA — Elementalist silahsız.**

Master Pipeline §4.5 kuralı: "Elementalist & Hexer: Silahsız. Adım 5 atlanır."

`outputs/07_weapon_pass/` klasörü boş kalır.

## Notes

- BasicAttackProfile_Elementalist.asset = CastRhythm strategy
- **Critical:** Karakter sprite'ında element rengi YOK — sadece blank VFX zone
- Element seçimi runtime'da değişir (Fire/Frost/Lightning/Light), aynı sprite'a 4 farklı VFX overlay
- Element accent pose'da değil sadece VFX'te — bu sprite asset'i element-agnostic kalır
- Aynı sprite Faz 3 element switching mekaniğinde hala kullanılacak
