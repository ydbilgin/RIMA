# RIMA Skill Icon Pack — BATCH üretim (32px, tek-batch 64 ikon)

Hazırlık: 2026-06-09. Hepsi (mevcutlar dahil) baştan, tutarlı tek pack.

## SINIFLAR (skill'i olan = ikon gereken)
Sadece **4 sınıf** kodlu: **Warblade 14 · Elementalist 15 · Ranger 20 · Shadowblade 22 = 71**. Diğer 6 sınıf (Ronin/Gunslinger/Ravager/Hexer/Brawler/Summoner) henüz skill'siz → ikon yok.

## TEK 64-BATCH (W+E öncelik)
71 > 64 → bir batch 64 alır. Öncelik: **Warblade+Elementalist (29) garantili** → Ranger (20) → Shadowblade ilk 15 = **64**. Kalan **7 Shadowblade** = mini batch-2. Pratikte: aşağıdaki Batch-1 (49) + Batch-2'nin ilk 15'i = tek 64'lük çağrı; son 7 ayrı.

## YÖNTEM (NLM-doğrulandı)
- **Tool = `create_1_direction_object`** (Create Object / 1-direction). **Create Image Pro DEĞİL** (o tek görsel üretir).
- **Boyut → varyant:** 32px (≤42) → **64 obje (8×8 grid)** tek çağrıda. (64px→16, 128px→4.)
- **Farklı ikonlar:** ana `description` = ortak stil; **`item_descriptions[]`** = her ikon için ayrı tanım (≤64 adet). Web UI'da item'ları **` - ` ile ayırarak** gir.
- **Style reference:** `size` ve `style_images` BİRLİKTE verilemez. Style ref verince çıktı boyutu en büyük ref'e kilitlenir → **style ref'i ~32px'e küçültüp ver**, 64-grid otomatik tetiklenir. (Style ref vermezsen `size=32` gir.)

## STYLE REFERENCE — ne ver?
- **Öneri:** mevcut RIMA skill ikonlarından 1-2 temiz örneği (ör. şu anki **Fireball + Glacial Spike + Iron Charge** ikonları) **~32px'e küçült** → ver. Böylece yeni pack RIMA'nın mevcut diline oturur.
- Taze stil istersen: beğendiğin **tek tutarlı dark-fantasy ikon** referansı (32px). Tek dil = tutarlı pack.

## ANA DESCRIPTION (ortak stil — her batch'te aynı)
```
dark fantasy game skill icon, single centered glowing emblem, bold readable silhouette, dark transparent background, no text, no border, pixel art
```

---

## BATCH 1 — Sınıflar (öncelik) · 49 item
Aksent: Warblade=pirinç/çelik+cyan rift · Elementalist=elemente göre (alev turuncu/buz cyan/şimşek mor/altın #FFF000) · Ranger=soğuk mavi #7BA7BC.

item_descriptions ( ` - ` ile ayır ):

Iron Crush: iron gauntlet slamming down - Blade Rush: dashing sword streak forward - Deep Wound: dripping bleeding gash - Iron Counter: raised shield parry spark - Battle Surge: roaring red rage aura - Iron Charge: armored bull charge horns - Crippling Blow: cracked bone heavy hammer - Gravity Cleave: downward axe vortex pull - Sunder Mark: cracked armor target rune - Ironclad Momentum: spinning steel gauge - Earthsplitter: ground fissure shockwave crack - Cleave: wide horizontal sword arc - War Stomp: stomping foot shock ring - Death Blow: skull-marked executioner strike - Arcane Blast: violet arcane energy burst - Combustion: exploding fire bloom - Prism Beam: refracted rainbow light beam - Frost Wall: jagged ice wall barrier - Solar Flare: blazing sun flare ring - Frozen Orb: floating ice orb shards - Arcane Surge: surging gold arcane sigil - Blizzard: swirling snowstorm cloud - Chain Lightning: forked branching lightning bolt - Glacial Spike: sharp ice lance upward - Living Bomb: glowing unstable fire rune - Meteor: falling flaming meteor impact - Mirror Image: duplicate phantom clones - Blink: teleport flash dash sparkle - Fireball: classic flaming fireball - Aimed Shot: precise crosshair arrow - Concussive Arrow: blunt impact arrow burst - Barbed Net Shot: thrown barbed net - Multi Shot: three fanned arrows - Disengage: backflip leap away - Black Arrow: dark cursed arrow - Volley: arrow rain arc - Rapid Fire: rapid arrow streaks - Tethering Arrow: arrow with binding rope - Pinning Shot: arrow pinning target - Marked Detonate: exploding marked target - Hunters Step: dash footprint trail - Bone Trap: snapping bone jaw trap - Sweep Volley: wide sweeping arrow arc - Predators Mark: glowing hunter eye mark - Final Strike: charged finishing arrow - Wireline Trap: tripwire snare - Explosive Trap: spike bomb trap blast - Flare: bright signal flare burst - Point Blank: close-range arrow blast

---

## BATCH 2 — Shadowblade (post-demo) · 22 item
Aksent: void moru #5A2A8A.

item_descriptions ( ` - ` ile ayır ):

Kidney Shot: stunning dagger stab - Preparation: ready dagger flip stance - Evasion: blurred dodge afterimage - Vanish: smoke disappearing figure - Phase Step: phasing shadow teleport - Death Mark: red death target rune - Veil Burst: shadow shockwave burst - Severance: clean cutting slash line - Smoke Veil: smoke cloud screen - Chain Cull: chained dagger combo - Shadow Pin: shadow spike pinning - Night Aperture: dark void portal eye - Backstab Mark: dagger behind-back rune - Shadow Clone: shadow duplicate figure - Ambush: pouncing strike from dark - Backstab: dagger stab from behind - Fan Of Knives: fanned thrown knives - Hemorrhage: heavy bleeding slash - Mirage Blade: illusory phantom blade - Rupture: bursting bleed wound - Toxic Eruption: green poison eruption - Shadow Step: shadow dash step

---

## WIRING (Claude yapacak)
Her ikon `SkillIconRegistry.entries`'e `key = skill-adı-lowercase-boşluksuz` ile bağlanır (ör. "Glacial Spike"→`glacialspike`). Tam key eşlemesini Claude koddan çıkarır + Unity'ye import (64px effective; 32px source UI'da Point ile ölçeklenir).
