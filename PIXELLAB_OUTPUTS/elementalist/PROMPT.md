# Elementalist — Class-Specific Prompts
*Faz 2, asimetrik (4 yön ayrı), silahsız (büyü el jestleriyle)*

## Class Identity

| Field | Value |
|---|---|
| Type | **Asimetrik** (S, E, N, W ayrı üret — el jestleri tek tarafta belirgin) |
| Weapon | **YOK** (silahsız) — Weapon pass zaten yok -- anchor'da orb/gesture gorunur, Edit Image Pro sadece poz varyanti icin. |
| Accent Color | **Element bazlı** — varsayılan idle: cool neutral #B8C8D0; skill aktifken Fire/Frost/Lightning/Light spesifik |
| Yasak | Void energy (Shadowblade'in mor'u), kitap/odiyak (silahsızlığı vurgula — el jestleri ile büyü) |
| Basic Attack | CastRhythm (cast → channel → release pattern) |
| RMB | Element-based ranged spell (placement) |
| Silhouette | Robe + büyücü duruşu, eller ön planda (büyü için) |

## Anchor Referans (Adim 1 -- Baslangic Noktasi)

Weaponless base body URETME. Bu sinifin standing pose anchor'i zaten hazir:

  Characters/anchors/elementalist/rotations/south.png
  Characters/anchors/elementalist/rotations/south-east.png
  Characters/anchors/elementalist/rotations/east.png
  Characters/anchors/elementalist/rotations/north-east.png
  Characters/anchors/elementalist/rotations/north.png
  Characters/anchors/elementalist/rotations/north-west.png
  Characters/anchors/elementalist/rotations/west.png
  Characters/anchors/elementalist/rotations/south-west.png

Bu dosyalar silahini tutan, duran poz karakteri icerir -- temiz arka plan, uretim hazir.
Tum animasyonlarin start frame kaynagi bu anchor dosyalaridir.

Edit Image Pro kullanim durumlari:
- Anchor'dan uretilemayan asiri pozlar icin (walk extreme pose A/B, attack windup) use anchor as source image in Edit Image Pro to produce variant
- Animasyon start frame ve end frame'i yeni uretimden gelecekse: Edit Image Pro + anchor source
- Weapon zaten anchor'da var; ayri weapon pass GEREKMIYOR

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
Walking forward mage gait, robe trailing behind, smooth deliberate step (not as quick as Ranger, not as heavy as Warblade). Hands held loosely at sides or chest. orb hovering near dominant hand, palm slightly extended. South-facing.
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

## Edit Image Pro -- Yeni Frame Uretimi (Adim 5, ihtiyac varsa)

Anchor standing pose yeterli degilse (attack PEAK, walk extreme) yeni frame uret:
1. Source image: ilgili yon anchor dosyasi (Characters/anchors/elementalist/rotations/south.png)
2. Prompt: istenen poz degisikligini describe et, weapon zaten source'da gorunur
3. Output: direction_variant.png olarak kaydet, eraser pass uygula

Weapon ayrica ekleme -- anchor'da zaten var.

## Notes

- BasicAttackProfile_Elementalist.asset = CastRhythm strategy
- **Critical:** Karakter sprite'ında element rengi YOK — sadece blank VFX zone
- Element seçimi runtime'da değişir (Fire/Frost/Lightning/Light), aynı sprite'a 4 farklı VFX overlay
- Element accent pose'da değil sadece VFX'te — bu sprite asset'i element-agnostic kalır
- Aynı sprite Faz 3 element switching mekaniğinde hala kullanılacak
