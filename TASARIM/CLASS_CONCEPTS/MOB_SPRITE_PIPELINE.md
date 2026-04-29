# MOB SPRITE PIPELINE — FAZ 1 (Act 1: Shattered Ruins)
> PixelLab "Create Character" tool. Camera View: high top-down (UI'dan ver, prompta YAZMA).
> Format: `[appearance], [threat pose], full-body pixel art sprite, clear silhouette`
> YASAK: "dark fantasy", "isometric", "3/4", oyun ismi. Kamera kelimesi YAZMA.
> Stil: Fractured Epic — canlı renkler, dramatik kontrast, okunabilir siluet.

---

## PixelLab Sabit Ayarlar (Mob için)

| Alan | Değer |
|------|-------|
| Tool | Create Character |
| Camera View | high top-down (= 35°) — UI'dan ver |
| Preset | (boş bırak — insan preset KULLANMA) |
| Outline | single color black |
| Shading | medium |
| Detail | medium |
| AI Freedom | 0 |

---

## MOB 1 — Fracture Imp (48px)

**Combat:** Swarm skirmisher, melee rush, AoE bait.

**PixelLab Prompt:**
`tiny spindly imp creature, jagged crystal shards jutting from shoulders and knuckles, cracked dark chitinous carapace with cold blue void light seeping through fractures, crouching low sprint lunge pose with arms spread wide and claws extended forward, compact aggressive silhouette, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Clearly smaller than all other enemies
- [ ] Jagged silhouette reads at small size
- [ ] Cold blue void glow visible on cracks
- [ ] Crouching lunge pose (not standing upright)

---

## MOB 2 — Relic Caster (80px)

**Combat:** Fragile support, priority kill target, shields allies.

**PixelLab Prompt:**
`gaunt robed humanoid, tattered grey-blue mage robes with frayed edges, cracked amber reliquary held in one hand at chest height, other arm raised in casting gesture, frail thin build with sunken hollowed face, faint golden glow from reliquary illuminates robes from below, upright fragile stance, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Clearly smallest humanoid — reads as fragile/easy kill
- [ ] Reliquary visible and distinct from body
- [ ] Amber glow from reliquary provides clear faction read
- [ ] Robes tattered (worn, not pristine)

---

## MOB 3 — Seam Crawler (96px)

**Combat:** Fast flanker, ground-hugging, anti-kiting, emerges from cracks.

**PixelLab Prompt:**
`wide flat ground-hugging creature, prominent serrated spine ridge running along back, two large curved front claws extended forward, dark charcoal stone-textured carapace with glowing cold blue cracks along body seams, belly close to ground in spread crawl pose, horizontal silhouette wider than it is tall, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Width clearly greater than height — horizontal threat
- [ ] Spine ridge readable from overhead view
- [ ] Two front claws prominent in silhouette
- [ ] Cold blue seam cracks visible on carapace

---

## MOB 4 — Shard Walker (112px)

**Combat:** Ranged pressure, floating fragment body, death explosion.

**PixelLab Prompt:**
`fractured humanoid construct, body composed of floating dark stone fragments with cold blue void light filling the visible gaps between pieces, right arm raised and cocked back in shard-throw pose, jagged crystal projectile gripped in throwing hand, fragmented chest and head with light bleeding through, upright combat stance, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Visible gaps in body with cold blue glow — key visual read
- [ ] Throwing arm pose clear (not ambiguous idle)
- [ ] Crystal shard visible in throwing hand
- [ ] Fragmented silhouette distinct from solid humanoids

---

## MOB 5 — Chain Warden (128px)

**Combat:** Mobility check, dash punisher, slows on hit.

**PixelLab Prompt:**
`heavily armored humanoid guardian, thick worn iron pauldrons and barrel chest plate, three chain lengths extending from both gauntleted fists with cold blue glow on chain links, arms spread wide in chain-release swing pose, broad combat stance, rusted dark iron armor with hairline cracks showing faint void light, imposing wide silhouette, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Chains clearly visible and extending outward from fists
- [ ] Armor reads as heavy and worn (not pristine)
- [ ] Cold blue glow on chain links only (not all-over glow)
- [ ] Silhouette clearly wider than player due to spread arms + chains

---

## MOB 6 — Penitent (128px)

**Combat:** Anti-heal aura bruiser, slow melee, heal suppression.

**PixelLab Prompt:**
`broad hunched bruiser, deeply slumped rounded shoulders and collapsed posture, lash scarring across exposed arms and bare chest, ashen pale grey skin with dark bruised mottling, heavy arms hanging low and forward in slow shambling advance pose, faint desaturated sickly grey-green aura radiating from body, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Hunched slumped posture clearly distinct from upright enemies
- [ ] Lash scars readable on arms/chest
- [ ] Sickly aura visible (subtle, desaturated — not vivid green)
- [ ] Ashen skin tone distinct from other mob palette

---

## MOB 7 — Void Thrall (128px)

**Combat:** Splitter — dies into two HalfThralls, priority management target.

**PixelLab Prompt:**
`tall slender wraith-like humanoid, elongated thin limbs with void tendrils trailing from wrists and upper back, deep purple-black semi-translucent body with soft lavender glow pulsing from within, long neck and tapered oval head, arms spread wide in looming threat display pose, towering vertical silhouette, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Tallest/thinnest of the 128px enemies — vertical contrast with Penitent/ChainWarden
- [ ] Void tendrils clearly trailing from wrists and back
- [ ] Lavender inner glow distinct from cold blue of other mobs
- [ ] Arms-spread pose reads as looming threat (not idle)

---

## MOB 8 — Ruin Hulk (160px)

**Combat:** False threat / spatial blocker, intimidating but low damage, attention decoy.

**PixelLab Prompt:**
`massive ancient stone golem, broad blocky body with large chunks visibly missing from torso and shoulders exposing hollow interior with faint cold blue light within, one heavy arm raised slowly in overhead swing telegraph, other arm trailing low, deeply cracked and crumbling dark grey stone surface across entire body, monolithic intimidating silhouette showing clear structural damage and fractures, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**QC:**
- [ ] Clearly largest sprite — must dominate canvas height
- [ ] Missing chunks with glowing interior = key visual (false threat read)
- [ ] Overhead swing telegraph pose (one arm up, one down) — not symmetrical
- [ ] Cracks and crumble clearly visible — NOT pristine/clean stone

---

## Üretim Sırası (önerilen)

1. Fracture Imp (48px) — en basit, test için önce üret
2. Relic Caster (80px) — fragile read'i test et
3. Seam Crawler (96px) — horizontal silhouette zor olabilir, erken test
4. Shard Walker (112px)
5. Chain Warden (128px)
6. Penitent (128px)
7. Void Thrall (128px)
8. Ruin Hulk (160px) — en büyük, en son

## Çıktı Klasörü

`Assets/Sprites/Characters/Mobs/Act1/[MobName]/`

Her mob için: `[mobname]_idle_[size]px.png` (8 yön, ayrı dosyalar)
