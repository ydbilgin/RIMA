# B1: ANIMATION_PROMPT_CATALOG Weaponless Cleanup

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
ANIMATION_PROMPT_CATALOG.md'deki 11 anim CHARACTER + CONSTRAINTS bloklarını **weaponless** variant'a çevir. Warblade body zaten silahsız (verify done), sadece prompt cleanup.

## Bağlam
- Verify done: warblade_south.png silahsız LIVE
- Plan ref: `STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md` Bölüm 1
- Mevcut catalog: `STAGING/ANIMATION_PROMPT_CATALOG.md` (26.6 KB, 10 bölüm)
- Silah HandAnchor child SR ile mount (Karar #144/#123/#146 LIVE)

## İş kalemleri

### 1. CHARACTER block update (her promptta sabit)
**Eski:**
```
64x64 chibi top-down character, male heavy warrior with two-handed greatsword,
dark steel armor uniform with bulky shoulder pads, brown leather straps,
...
```

**Yeni:**
```
64x64 chibi top-down character, male heavy warrior,
EMPTY HANDS, fists loosely clenched in weapon-ready grip posture,
dark steel armor uniform with bulky shoulder pads, brown leather straps,
light skin, messy black hair, stern neutral face,
view 35 degree high top-down ARPG angle,
dark brown body #4F3A2C brass accent #C09455
```

### 2. CONSTRAINTS block update
**Eski:**
```
2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
don't fill canvas, leave wide transparent headroom.
WEAPON LOCK: weapon stays firmly in hands,
do not use words drop, release, slip, fall, throw.
```

**Yeni:**
```
2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing.
don't fill canvas, leave wide transparent headroom.
NO weapons, NO held items, hands empty throughout.
```

### 3. ACTION block per-anim update (11 anim)
Her anim için "greatsword raised", "two-hand grip", "blade extending" benzeri silah referanslarını silahsız hand motion'a çevir. Hand position weapon mount uyumlu olmalı (HandAnchor offset table ile uyumlu).

Örnek (Basic Attack):
- Eski: `Frame 1: greatsword raised high at right side. Frame 4 (apex): wide horizontal arc reaching maximum extension to left.`
- Yeni: `Frame 1: right arm raised high above right shoulder, hand in weapon-ready grip (knuckles forward). Frame 4 (apex): right arm fully extended in wide horizontal arc reaching maximum extension to left, hand still in grip pose.`

Tier 1 (5 anim) + Tier 2 (6 skill) = 11 anim total update.

### 4. Karar #71 (Weapon Lock) referansları kaldır
- Bölüm 1.4 Karar #71 "WEAPON LOCK" silaha referans veriyor
- Yeniden formüle et: "Karar #71 ARTIK weaponless production'da uygulanmaz. Weapon mount Unity child SR (Karar #144 + #123 + #146 LIVE)."

### 5. PixelLab V3 gen cost update (S114 LIVE)
- Bölüm 1.8 "Karar #108 — Gen count" SUPERSEDED note ekle
- Yeni cost mapping: 4f=1 / 6-8f=2 / 10-12f=3 / 14-16f=4 gen per dir
- Cost tablolarında (Tier 1, Tier 2) bu mapping'e göre revize

## Dosyalar (scope)
- `STAGING/ANIMATION_PROMPT_CATALOG.md` (EXTEND, ~26 KB → ~28 KB tahmin)
- Yeni `STAGING/ANIMATION_PROMPT_CATALOG_v2_weaponless.md` (alternatif, eski v1 archive)

## YASAK
- Asset gen (sadece doc edit)
- HandAnchor/OrientationSync/WeaponSorter codebase dokunma (LIVE)
- WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md modify (üst plan dokunulmaz)

## Verify
- ANIMATION_PROMPT_CATALOG.md tüm 11 anim CHARACTER + CONSTRAINTS + ACTION blokları weaponless
- Karar #71 weapon lock referansı düzeltilmiş
- Cost mapping S114 V3 ile uyumlu (Karar #108 SUPERSEDED note)
- Memory `project_animation_prompt_catalog_warblade` ile uyumlu

## Output
- `STAGING/B1_PROMPT_CLEANUP_DONE.md` — değişen satırlar diff + verify checklist

## Süre
~30 dk Sonnet bg.

BLOCKED durumu: Eğer Karar #120 (split-animation pipeline) ile weaponless conflict varsa → orchestrator'a flag (apex frame hand position weapon mount ile uyumlu kalmalı, kritik).
