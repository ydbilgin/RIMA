# ANIM2_axpro - Elementalist anim profili ve VFX karari

## E1 - Caster profil karari

Elementalist, Warblade'in melee "strike readability" problemini paylasmaz; onun problemi **cast niyeti + VFX shape ayrimi**dir. Bu yuzden P1'de tek bir iyi "basic cast" animasyonu, dort ayri skill animasyonundan daha degerlidir.

**P1:**
- Idle: class identity stance. Eller ve orb/rune detayi okunmali.
- Run: robe hareketi temiz, ust beden fazla savrulmasin.
- Basic cast/bolt: LMB/Rift Bolt karsiligi. Kisa charge, release, recovery.

**P2:**
- Flinch: cast interruption okunur.
- Channel-hold: Meteor/Blizzard/Prism gibi uzun castler icin tek reuse poz.
- Element switch micro-pose: Fire/Frost gecisi body language ile desteklenir ama engine flash asil dili tasir.

**P3:**
- Fire-specific throw, Frost shaping, Light beam channel.
- Lightbreak/V-state hero animasyonu.
- Death.

State listesi P1+P2 icin yeterli: `breathing-idle`, `mid-run`, `cast-charge/windup`, `flinch`. Channel-hold daha sonra gerekiyorsa `cast-hold` olarak besinci state olabilir; simdi degil.

## E2 - VFX ownership

**Secim: C hybrid.** Elementalist'in mevcut kitinde Fire/Frost/Light ayrimi zaten kodda var:
- Fire: Fireball, Living Bomb, Meteor, Combustion.
- Frost: Glacial Spike, Frozen Orb, Frost Wall, Blizzard.
- Light: Chain Lightning, Prism Beam, Solar Flare, Lightbreak state.

Bu sinifin gorsel kimligi "renk" degil, **geometri** olmali:
- Fire = projectile / explosion / meteor fall.
- Frost = line / shard / orb / wall control.
- Light = beam / chain / radiant cone.

PixelLab sadece cekirdek shape sprite'larini uretmeli. Engine sunlari sahiplenmeli:
- cast flash
- additive tint
- projectile trail
- impact scale-fade
- beam/line path
- state stacks, Lightbreak, resonance, target feedback
- telegraph timing

Mevcut `SkillVfx` zaten `CastFlash`, `ProjectileTrail`, `ImpactBurst`, `PlayBurst`, additive palette ve scale-fade veriyor. Bu yuzden PixelLab'i "her skill animasyonu" icin kullanmak yanlis sahiplik olur; PixelLab sadece shape kalitesini artirsin.

## Minimal PixelLab VFX paketi

1. **Fireball projectile, 64px, transparent, 8-dir**
   - En cok tekrar kullanilacak asset. Mevcut kod `Resources/VFX/Fireball/{direction}` ariyor.
2. **Fire impact burst, 80px, transparent**
   - `SkillVfx.PlayBurst` ile scale-fade.
3. **Glacial spike cluster, 96x64/96px, transparent**
   - Frost'un "line/control" farkini satar.
4. **Frozen orb core, 64px, transparent**
   - Slow moving orb icin tek okunur sprite.
5. **Light beam/flare core, 96x32 veya 80px, transparent**
   - Prism/Solar/Lightbreak renk ailesi icin engine tarafinda uzatilabilir.

Simdilik uretme:
- Meteor detay seti: buyuk ama tek skill; mevcut `meteor_main` var.
- Blizzard field: engine particle/telegraph daha dogru.
- Frost Wall: collision/placement engine-owned.
- Living Bomb mark: target/state cue engine-owned.
- Full Lightbreak hero VFX: P3.

## Uretim sirasi

**Once Warblade P1.** Demo tek-sinif Warblade oldugu icin Elementalist uretimi, Warblade idle/run/LMB oyunda gorulmeden baslamamali.

Warblade:
- 4 state call: idle, run, strike, flinch.
- P1 anim call: 15.
- P2 flinch: +5 sadece gerekirse.

Elementalist:
- 4 state call.
- P1 anim call: idle/run/basic cast = 15.
- P2 flinch: +5.

VFX:
- Fireball 8-dir: 8 gen.
- 4 single objects: 4 gen.
- Reroll buffer: 10-20.

Kredi 874 oldugu icin problem gen call degil. Problem import, alpha kontrol, PPU64, direction cleanup ve in-game readability. Bu paket "guzel" hissi verir ama post-demo scope'u patlatmaz.
