# ANIM2_axflash - Lean scope check

## Net cevap

Elementalist icin P1:
1. idle
2. run
3. basic cast / bolt

"Attack" = sword/staff vurus degil. "Attack" = kisa cast charge + forward release. Bunu Q/E/R/F icin reuse et.

## VFX karari

**Hybrid yap.** Full PixelLab skill-VFX seti yapma.

Engine-only fazla kuru kalir; full PixelLab fazla pahali degil ama fazla cleanup/entegrasyon ister. En iyi lean paket:
- 1 fireball projectile 8-dir
- 1 fire impact
- 1 glacial spike cluster
- 1 frozen orb
- 1 light beam/flare core

Gerisi engine:
- `SkillVfx.CastFlash`
- `SkillVfx.ProjectileTrail`
- `SkillVfx.ImpactBurst`
- additive/tint/scale-fade
- telegraph, state, beam path, wall collision

## Over-produce yasak listesi

Simdi yapma:
- her Elementalist skill icin ayri cast animasyonu
- Meteor/Blizzard/Frost Wall full VFX paketi
- Lightbreak ultimate seti
- 8 yonlu cast varyantlari disinda element-specific body animasyonlari
- death animasyonu

## Prompt uygulama notu

State promptlari kisa kalsin: ayni karakter, high top-down, poz, kimlik koru, VFX yok. Mekanik paragraf yazma; PixelLab mekanigi degil pozu cizer.

Anim promptlari:
- idle 8 frame
- run 8 frame
- basic cast 6 frame
- flinch 4 frame
- 5 direction generate: S, SE, E, NE, N
- mirror: SW, W, NW

## Budget gercegi

Warblade demo P1:
- state: 4
- anim: 15
- P2 flinch opsiyonel: +5

Elementalist post-demo P1:
- state: 4
- anim: 15
- P2 flinch opsiyonel: +5

VFX lean core:
- 8-dir fireball: 8
- single VFX objects: 4
- reroll buffer: 10-20

874 gen bol. Darbogaz cleanup. Bu yuzden siralama degismez: **Warblade P1 once, Elementalist sonra.**

## Tek cumle

Elementalist'e Warblade gibi P1 hareket cekirdegi ver, basic cast'i tum skillerde reuse et, sadece 5 cekirdek PixelLab VFX shape'iyle "guzel" hissi ekle.
