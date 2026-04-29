# RIMA — PixelLab Faz Master Üretim Rehberi
*Güncelleme: 2026-04-17 | Tüm fazlar için tek referans*
*Ghost attack spec: `GHOST_ATTACK_SPEC.md` | Warblade base anims: `18_NISAN_PIXELLAB_TALIMAT.md`*

---

## SABIT PARAMETRELER (Her üretimde aynı)

```
View:            high top-down
Isometric:       ON (Pixflux/Standard checkbox)
Outline:         single color black outline
Shading:         detailed shading
Detail:          high detail
ai_freedom:      400 (tutarlılık için)
Proportions:     heroic (SADECE player + champion boyutlu)

Pro mode (site):
  → Checkbox yok → description'a şunu ekle:
  "facing southeast, isometric pixel art, 45 degree angle, orthographic projection"

Canvas:          karakter animasyonu için 160×160 (Aseprite'ta canvas expand)
Remove bg:       ON (her üretimde)
```

**Style anchor:** `F:/Antigravity Projeler/Pixellab/RIMA_REFS/rima_style_anchor.png`

---

## MOB ARMOR VARİANT SİSTEMİ — YENİ TASARIM

Hades'teki gibi aynı mob, daha zor versiyonları olarak odaya çıkabilir.

### Tier Tanımı

| Tier | HP | HP Bar | İkon | Görsel |
|------|-----|--------|------|--------|
| Normal | 1x | Kırmızı-turuncu | — | Standart sprite |
| Armored | 2x | **Altın** → kırmızı | 🛡 | +metalik omuz/göğüs zırh eklentisi |
| Heavily Armored | 3x | **Gümüş** → altın → kırmızı | 🛡🛡 | +tam plaka zırh varlığı |

### HP Bar Renk Sistemi (Unity)
```
Normal:         [====RED====]
Armored:        [====GOLD===][====RED====]
Heavily Armored:[==SILVER===][====GOLD===][====RED====]
```
HP bar katmanlar soldan sağa değil üst üste — alt katman altın biter üst katman görünür.

### PixelLab Üretim Workflow

Base sprite hazır → armored variant için `edit-images-v2`:
```
Edit description: "add heavy metallic shoulder pauldrons and chest plate to this creature,
                  grey-blue metallic armor reinforcement, same pose same creature,
                  visible battle damage on armor surface"
```
**Cost:** 1g per variant sprite (base sprite zaten var)

### Act 1 Armored Varyantları (Act 1 geç odalar + Act 2)

| Varyant İsim | Base Mob | Tier | Ekstra Mekanik |
|-------------|----------|------|----------------|
| Shard Bulwark | ShardWalker | Armored | Shard sayısı 5'e çıkar |
| Void Juggernaut | VoidThrall | Armored | Split: 2 yerine 3 HalfThrall |
| Iron Penitent | Penitent | Armored | Anti-heal aura radius +50% |
| Chain Executioner | ChainWarden | Heavily Armored | Zincir range +50% |
| Relic Archon | RelicCaster | Armored | Kalkan 4s → 2 düşmana aynı anda |

---

## ═══════════════════════════════
## FAZ 1 — CORE LOOP ÜRETİMİ
## ═══════════════════════════════

### F1 GEN BÜTÇE TAHMİNİ

| Kategori | Gen |
|----------|-----|
| Warblade eklentiler (Ghost/V/RMB) | ~60g |
| Act 1 mobs (8 normal, 4 elite) | ~180g |
| Penitent Sovereign boss | ~80g |
| Armor varyantlar (5 mob × 4 yön) | ~20g |
| **FAZ 1 TOPLAM** | **~340g** |

---

### F1-CHAR: WARBLADE ANIMASYON EKLENTİLERİ

> Warblade base (idle/run/attack/dash/death) → **`18_NISAN_PIXELLAB_TALIMAT.md`**
> Aşağıdakiler O REHBERE EK olarak üretilecekler.

---

#### F1-CHAR-A: GHOST ATTACK (Warblade versiyonu)

Spec: `GHOST_ATTACK_SPEC.md` → **Warblade bölümü** (geniş yatay sword sweep, 12f, 4 yön)

Üretim: `18_NISAN_PIXELLAB_TALIMAT.md` attack workflow'u ile aynı araçlar.
Ghost sprite nötr üretilir → Unity runtime'da `#66AAFF` tint MaterialPropertyBlock ile.

Kaydet:
```
Assets/Sprites/Characters/Warblade/ghost/ghost_attack_S.png
Assets/Sprites/Characters/Warblade/ghost/ghost_attack_N.png
Assets/Sprites/Characters/Warblade/ghost/ghost_attack_W.png
Assets/Sprites/Characters/Warblade/ghost/ghost_attack_E.png
```

---

#### F1-CHAR-B: V ANİMASYON — BLADESTORM

**Mekanik:** Rage 100 → 5s dönerek spin AoE, her 0.5s hasar. CC immune.
**Animasyon:** Looping spin (16f), 4 yön (S/N/W/E ile spin yönü değişmez, karakter dönüyor zaten)

**Windup keyframe (Edit PRO):**
```
Image: warblade_S.png
Edit description: "warrior begins spinning bladestorm, body rotating into first spin,
                  greatsword extended horizontally outward in centrifugal arc,
                  cold blue rift energy trails behind spinning blade,
                  one foot pivoting, centrifugal force visible,
                  isometric pixel art, 45 degree angle, hades game art style"
Output: 128×128, remove bg ON
```
→ Kaydet: `warblade_bladestorm_spin_S.png`

**Loop oluştur (Interpolate Pro site):**
```
Start:  warblade_bladestorm_spin_S.png
End:    warblade_bladestorm_spin_S.png (aynı — seamless loop)
Action: "warrior spinning bladestorm, continuous rotation, greatsword extended in wide arc,
        cold blue rift energy spiraling, powerful centrifugal spin, seamless loop"
Frames: 16
```

4 yön: Spin animasyonu yönden bağımsız — 1 adet üret, Animator Controller'da tüm yönlerde kullan.

Kaydet:
```
Assets/Sprites/Characters/Warblade/animations/warblade_bladestorm.png (spritesheet)
```

---

#### F1-CHAR-C: RMB ANİMASYON — RAGE OUTLET

**Mekanik:** Rage 30+ → kısa AoE patlaması, Rage -30, çevredeki düşmanlar sendeler.
**Animasyon:** 8f, 2-segment (charge→burst), 4 yön

**Burst keyframe (Edit PRO):**
```
Image: warblade_S.png
Edit description: "warrior releases rage in short explosive burst, both arms thrown outward,
                  body expanding outward with force, armor vibrating,
                  cold blue rift energy erupting from armor cracks in all directions,
                  powerful AoE release, explosive stance,
                  isometric pixel art, 45 degree angle, hades game art style"
Output: 128×128, remove bg ON
```
→ Kaydet: `warblade_rmb_burst_S.png`

**Animasyon (Interpolate Pro site):**
```
Pasaj 1: base → burst (4f)
  Start: warblade_S.png | End: warblade_rmb_burst_S.png
  Action: "warrior coils then releases rage explosion outward, arms throwing wide"

Pasaj 2: burst → base (4f)
  Start: warblade_rmb_burst_S.png | End: warblade_S.png
  Action: "warrior settles after rage release, arms returning to guard position"
```

---

### F1-MOB: ACT 1 NORMAL DÜŞMANLAR

> Üretici: **Kiro (MCP bulk)** — Claude'un talimatları, Kiro üretir.
> Her mob için: base sprite (4 yön) + 4 anim state (idle/walk/attack/death)
> Workflow: `18_NISAN_PIXELLAB_TALIMAT.md` mob anim template = aynı toolchain

**Sabit anim spec (tüm moblar için):**
```
Idle:   8f, 1-segment (Interpolate Pro: base→base loop)
Walk:   8f, 2-segment (keyframe + Interpolate)
Attack: 8-10f, 2-segment (Edit PRO keyframe + Interpolate)
Death:  10f, 2-segment (Edit PRO: dead pose → Interpolate)
Yön:    4 (S/N/W/E) — E = W flipX (Aseprite'ta)
```

---

#### MOB 1: FRACTURİMP — *İlk Üret* (combat test için şart)

**Boyut:** 48px | **Canvas:** 64×64 (animasyon için 80×80)
**Tasarım:** Küçük, sivri, rift enerjisiyle parçalanmış insansı form. Net ve farklı siluet.

**Base sprite prompt (Create Image, 64×64):**
```
tiny rift-born imp creature, small sharp-featured humanoid, body cracked with cold blue
rift energy seeping through fractures, jagged small claws, lean and quick-looking,
distinctly different silhouette from any other enemy, small but dangerous-looking,
facing southeast, isometric pixel art, 45 degree angle,
Fractured Epic dungeon creature, transparent background, 64x64
```

**Attack keyframe:** düşmana fırlatma/pençe anı — 4 kez spawn, çevreliyor
```
"fracture imp mid-lunge, body airborne, claws leading, small frame fully extended
in attack leap, frenzied expression, cold blue cracks glowing in exertion"
```

**Death:** şiddetli patlama → yerde küçük parçacık
```
"fracture imp collapsing, body crumbling apart, rift cracks exploding outward,
small body disintegrating on ground, violent death"
```

---

#### MOB 2: SHARDWALKERs — *İkinci Üret*

**Boyut:** 112px | **Canvas:** 128×128 (animasyon için 160×160)
**Tasarım:** Parçalı insansı, vücutta gap'ler var ışık sızıyor, dağınık ama tehdit edici.

**Base sprite prompt (Create Image PRO, 128×128):**
```
fractured humanoid creature, body fragmented with visible gaps between shattered body parts,
cold blue rift energy light seeping from between the fractures, hunched aggressive stance,
shards floating nearby ready to throw, imposing near-player-size threat,
proportions: heroic, facing southeast, isometric pixel art, 45 degree angle,
Fractured Epic dungeon, transparent background, 128x128
```

**Attack keyframe:** shard fırlatma — 3'lü yayılım poz
```
"shard walker throws three crystal shards outward in a spreading fan pattern,
throwing arm extended forward, body weight forward, three shards visible in throw arc"
```

**Death:** ölünce AoE patlama poz (hitbox kapsar)
```
"shard walker shatters on death, body exploding in shards, central mass collapsing,
fragments bursting outward in death explosion, ground impact"
```

**Armored varyant (Shard Bulwark):** base sprite hazır olunca `edit-images-v2`:
```
"add heavy metallic shoulder pauldrons and partial chest plate, grey-blue metallic armor,
creature still visible through armor gaps, 5 shards instead of 3"
```

---

#### MOB 3: VOİDTHRALL + HALFTHRALLs

**VoidThrall boyutu:** 128px | **HalfThrall boyutu:** 64px

**VoidThrall base prompt (128×128):**
```
void-infused tall humanoid, swollen with void energy, body barely containing the power,
long thin arms with void tendrils extending, pale purple aura emanating, imposing presence,
same height as player, looks like it might explode, facing southeast, isometric pixel art,
45 degree, Fractured Epic, transparent background, 128x128, heroic proportions
```

**VoidThrall Attack:** göğüs void pulse
```
"void thrall chest expanding massively with void energy buildup, arms splaying wide,
chest pulsing with purple void explosion about to release, AoE attack charge pose"
```

**VoidThrall Death (split-death):** özel — ikiye bölünüyor
```
"void thrall splitting down the middle, body dividing into two halves, void energy erupting
from the split, each half collapsing separately, dramatic split death"
```

**HalfThrall base prompt (64×64):**
```
half of a split void creature, one half of former void thrall, diminutive and frenzied,
asymmetric body missing half its mass, one arm, half a face, void tendrils dangling,
fast and desperate movement implied, facing southeast, isometric pixel art,
45 degree, Fractured Epic, transparent background, 64x64
```

---

#### MOB 4: SEAM CRAWLER

**Boyut:** 96px | Özellik: Zemine yapışık, yassı, GENİŞ yatay siluet

**Base sprite prompt (128×128 canvas, 96px sprite alanı):**
```
ground-hugging rift creature, flat body pressed close to ground, wide horizontal form,
spine and claws visible from above, crawling beneath the surface plane,
only upper body and claws visible, wide coverage hitbox implied,
facing southeast, isometric pixel art, 45 degree angle, zemine yapışık,
Fractured Epic dungeon, transparent background
```

**Özel anim — Emergence:** zemin çatlaktan çıkış (oda enter)
```
Start: zemin crack frame (Edit PRO ile base sprite gizli)
End: full visible stance
Action: "seam crawler emerging from ground crack, body rising from below surface,
        claws and spine breaking through, ambush entry animation"
```

---

#### MOB 5: CHAİN WARDEN

**Boyut:** 128px | Özellik: Zincirleri silueti karmaşıklaştırıyor

**Base sprite prompt:**
```
heavily armored fractured guardian, massive shoulders with hanging chains,
chains extending outward from both wrists, imposing warrior silhouette,
armor cracked and worn, chains clearly visible and distinct feature,
facing southeast, isometric pixel art, 45 degree, heroic proportions,
Fractured Epic dungeon, transparent background, 128x128
```

**Attack:** zincir fırlatma (3 yöne 45° açıyla)
```
"chain warden simultaneously launching three chains outward in spreading pattern,
body braced with legs wide, chains extending at different angles,
telegraphed throw stance, 2s open window after this"
```

**Armored varyant (Chain Executioner):** `edit-images-v2` ile ek plaka zırh + daha uzun zincirler

---

#### MOB 6: PENİTENT

**Boyut:** 128px | Özellik: Ağır, yuvarlak, içine çökmüş, aura görünür

**Base sprite prompt:**
```
self-flagellating religious fanatic creature, hunched heavy figure, rounded drooping shoulders,
wrapped in penitent robes with self-inflicted wounds, visible anti-heal aura field
surrounding the body as a shimmer effect, slow deliberate stance,
facing southeast, isometric pixel art, 45 degree, Fractured Epic, transparent bg, 128x128
```

**Attack:** kutsal ceza — eller gökyüzüne açılıp yere iner (ANIMATION_REDESIGN'dan)
```
"penitent raising both hands to sky, holy energy gathering overhead, then slamming
both hands to ground, energy waves radiating outward, ground crack at impact"
```

---

#### MOB 7: RELİC CASTER

**Boyut:** 80px | Özellik: İnce, yüksek, kırık relikvar tutuyor — EN KÜÇÜK figür

**Base sprite prompt (128×128 canvas, 80px sprite):**
```
thin tall fractured sorcerer, frail body draped in tattered robes, holding a cracked
broken relic artifact in outstretched hand, visually the most fragile enemy in the room,
clearly distinguishable as support target, thin limbs, minimal threat posture,
facing southeast, isometric pixel art, 45 degree, Fractured Epic, transparent bg
```

**Attack:** relik'ten kalkan verme jesti (kalkan alana)
```
"relic caster extending broken relic toward nearby enemy, shield energy projecting
from the cracked artifact, giving target a protective barrier, focused support gesture"
```

---

#### MOB 8: RUİN HULK — *Son Üret (görsel tasarım karmaşık)*

**Boyut:** 160px | Özellik: BÜYÜK GÖRÜNÜR, düşük tehdit (false threat)
**Tasarım Notu:** Yenilmez görünmeli ama öldürülünce "o kadar mı?" dedirtmeli. Açık kırıklar ve kopuk parçalar zorunlu.

**Base sprite prompt (256×256 canvas, 160px sprite):**
```
massive broken golem construct, towering figure with visible shatter cracks throughout body,
chunks of stone missing from limbs and torso revealing hollow interior,
body held together by remaining rift energy only, visually imposing but clearly damaged,
slow lumbering defensive stance, proportions: heroic but massive scale,
facing southeast, isometric pixel art, 45 degree, Fractured Epic, transparent bg, 160px character
```

**Attack:** yavaş telegraph yumruğu — 2.5s telgraf süresi
```
"ruin hulk arm raising extremely slowly in telegraphed ground slam, movement laborious
and heavy, arm at full raise position — player has clear 2.5s window to move"
```

---

### F1-ELİTE: ACT 1 ELİTE DÜŞMANLAR

> **Üretici:** Kullanıcı (Aseprite extension) — kalite kritik, elitler = oda boss hissi

---

#### ELİTE 1: FRACTURE KNİGHT

**Boyut:** 160px | Özellik: Büyük ama HIZLI — "büyük ama hızlı" paradoksu

**Base sprite prompt (256×256 canvas):**
```
elite rift knight, powerful armored warrior larger than player, fractured plate armor with
rift energy crackling through every crack, speed-optimized martial stance,
dynamic pose implying swift movement despite large frame, visible rift energy in joints,
facing southeast, isometric pixel art, 45 degree, heroic proportions,
Fractured Epic, transparent background
```

**Özel anim — Mirror Dash:** oyuncunun dash yönünü taklit ediyor (0.5s gecikme)
Animasyon: normal dash anımı + özel "mirror echo" VFX (kod tarafında)

---

#### ELİTE 2: THE TWİCE-BORN

**Boyut:** 128px × 2 | İKİZ düşman — renk ton farkı (birincil/ikincil)
Aynı sprite, farklı renk variation. Primary: normal. Secondary: tinted 30% darker.

**Base sprite prompt:**
```
elite fractured warrior pair, powerful armored fighter, mirror image aesthetic,
battle-hardened stance, heavily built, fractured energy bonding two entities,
distinctly dangerous looking, not a normal grunt — elite presence,
facing southeast, isometric pixel art, 45 degree, heroic proportions,
Fractured Epic, transparent bg, 128x128
```

**Secondary tint:** Edit PRO veya `edit-images-v2`:
```
"same character but with 30% darker tones across all colors, slightly cooler hue,
visual twin but clearly secondary entity"
```

**Berserk animasyon:** ikincil ölünce birincil berserk - hız artışı ifade eder
```
"warrior in frenzied berserk state, doubled speed pose, erratic aggressive stance,
surviving entity after partner death, rage-fueled energy visible"
```

---

#### ELİTE 3: THE RELİQUARY

**Boyut:** 160px | SABIT, hareket etmez | 4 dönen shard çevresinde

**Base sprite prompt:**
```
stationary sacred relic guardian construct, large circular crystalline construct,
four glowing shard satellites orbiting around it, ancient and powerful,
immovable but radiating threat, shards clearly destructible,
facing southeast (stationary, no directional animation needed),
isometric pixel art, 45 degree, Fractured Epic, transparent bg, 160px
```

**Shard satellite sprite (ayrı, 24×24):**
```
"single floating crystal shard, sharp faceted gem, glowing with warm golden light,
orbiting object, isolated, transparent background, 24x24, pixel art"
```

**Animasyon notu:** Walk animasyonu YOK (sabit). Idle = shard dönerken hafif pulse.

---

### F1-BOSS: PENİTENT SOVEREIGN

**Boyut:** 256px (Phase 1) | **Üretici:** Kullanıcı (Aseprite extension — quality critical)

**Lore:** Mekan muhafızının cezalandırıcı tecessümü. Büyük, ağır, kutsal enerji taşıyor.

**Base sprite prompt (Create Image PRO, 256px → site Pro mode):**
```
massive penitent sovereign boss, towering religious authority figure,
gigantic form wrapped in tattered execution robes, massive holy mace weapon,
crown of thorns integrated into armored hood, hands bound with heavy chains
that serve as weapons, golden holy energy contrasting with dark rotten fabric,
imposing dominant presence that fills the arena, boss-level threat reading,
facing southeast, isometric pixel art, 45 degree angle, orthographic projection,
heroic proportions maximized, transparent background, 256x256 canvas
```

**Faz 1 Animasyon Listesi:**

| Animasyon | Frame | Araç |
|-----------|-------|------|
| Idle (heavy breathing) | 8f | Interpolate Pro |
| Walk (slow, deliberate) | 10f | Animate w/text + Interpolate |
| **Melee — Mace Slam** | 14f (3 seg) | Edit PRO × 2 + Interpolate × 3 |
| **Ranged — Holy Bolt** | 10f (2 seg) | Edit PRO + Interpolate × 2 |
| Phase transition (50% HP) | 16f | Edit PRO × 3 + Interpolate × 2 |
| Death | 16f | Edit PRO × 2 + Interpolate × 2 |

**Melee slam keyframes:**
```
Windup:
"sovereign raises massive mace above head slowly, both hands gripping handle,
chains swinging wide, robes billowing with the weight of the raise"

Peak:
"sovereign slams mace into ground with catastrophic force, mace head buried in stone,
crater implied, chains wrapped around impact point, dust cloud"
```

**Holy bolt keyframe:**
```
"sovereign extends one chain-bound hand forward, golden holy energy condensed into
projectile orb in palm, ready to release, other hand drawn back"
```

---

## ═══════════════════════════════
## FAZ 2 — FIRST PLAYABLE ÜRETİMİ
## ═══════════════════════════════

*Faz 1 exit kriterleri tamamlandıktan sonra başla.*
*FAZ 1'deki tüm mob sprite'ları ve workflow referans olarak kullanılır.*

### F2 GEN BÜTÇE TAHMİNİ

| Kategori | Gen |
|----------|-----|
| 3 karakter (Elem/Shadow/Ranger) base+anim+ghost | ~250g |
| Act 2 mobs (7 normal + 3 elite) | ~180g |
| Boss: Penitent Sovereign Phase 2 eklentiler | ~40g |
| **FAZ 2 TOPLAM** | **~470g** |

---

### F2-CHAR: ELEMENTALIST

**Kimlik:** Pratik büyücü, deri+keten, asimetrik kol, elemental yanık.
**Ghost attack:** Çift el thrust — `GHOST_ATTACK_SPEC.md` Elementalist bölümü
**[V] Burst:** ARCANE OVERLOAD — Mana 100: 4s: tüm speller otomatik yayılır, area 2x

**Özel V animasyon prompt:**
```
elementalist channeling massive arcane overload, both hands raised overhead,
tremendous arcane energy spiraling around the body, hair and clothes whipping in arcane wind,
overwhelming power barely contained, full-body channeling stance,
isometric pixel art, 45 degree, transparent bg
```

**Anim listesi:** idle/run/stop/attack(LMB combo)/RMB/dash/death + ghost attack + V = `18_NISAN_PIXELLAB_TALIMAT.md` workflow ile aynı, farklı class prompt.

**LMB Attack kimliği:** Elemental spiral — hızlı birleşik element jesti (yumruk değil)
```
Attack windup: "elementalist gathering elemental energy in both hands, drawing from around her"
Attack peak: "elementalist releasing combined elemental blast forward, both hands extended,
             multi-colored energy eruption, spiral release"
```

---

### F2-CHAR: SHADOWBLADE

**Kimlik:** Charcoal-navy giyim, deri paneller, çok soluk dar göz beyazı, suikastçi formu.
**Ghost attack:** Çapraz X-kesiş — `GHOST_ATTACK_SPEC.md` Shadowblade bölümü
**[V] Burst:** SHADOW MELD — Energy 100: 3s stealth, sonraki 2 saldırı +200%

**LMB Attack kimliği:** Hızlı çapraz çift bıçak serisi
```
Attack peak: "shadowblade mid-rapid dual-blade cross slash, both blades crossing at
             speed, body low and mobile, combo continuation implied"
```

---

### F2-CHAR: RANGER

**Kimlik:** Deri asimetrik kıyafet, avcı pelerini, yayda void kristal ok.
**Ghost attack:** Yay çekip bırakma — `GHOST_ATTACK_SPEC.md` Ranger bölümü
**[V] Burst:** VOLLEY STORM — Focus 100: 5s: her 0.25s 3 ok yayılımı, CD sıfır

**LMB Attack kimliği:** Çabuk yay atışı (sürekli, mobil)
```
Attack peak: "ranger firing arrow at full draw while stepping forward, bow arm steady,
             movement maintained during shot, hunter's efficiency"
```

---

### F2-MOB: ACT 2 NORMAL DÜŞMANLAR

> Combat Roster Act 2: **MireStalker, RotPriest, ThornBrute, CarrionWeaver, BloodLancer, HuskThrower, DecayAnchor**
> Full tasarım: `TASARIM/COMBAT_ROSTER.md` Act 2 bölümü

**Boyut referansı:**
| Mob | Boyut | Tip |
|-----|-------|-----|
| MireStalker | 64px | Skirmisher |
| RotPriest | 64px | Support |
| ThornBrute | 96px | Bruiser |
| CarrionWeaver | 80px | Controller |
| CarrionWeaver Rotling | 24px | Minion |
| BloodLancer | 72px | Skirmisher |
| HuskThrower | 64px | Ranged |
| DecayAnchor | 128px | Construct (sabit) |

**Üretici:** Kiro (MCP bulk) — Faz 1 mob workflow şablon olarak kullanılır.
Her mob için `COMBAT_ROSTER.md` tasarımını Kiro'ya ver, sprite + 4 anim üretsin.

**Özel notlar:**
- **MireStalker:** Ayak altında bataklık efekti → ayrı 32px ground stain sprite üret
- **CarrionWeaver:** Rotling 24px → en küçük sprite, silüet net tutulmalı
- **DecayAnchor:** Sabit, walk anim yok

---

### F2-BOSS: PENİTENT SOVEREİGN PHASE 2

Faz 1 sprite'ları korunur. Ek animasyonlar:

**Phase 2 transformation (50% HP):**
```
"penitent sovereign erupts with golden holy light, chains breaking free, robes tearing,
transitioning to second phase, more exposed divine wrath form, dramatic transformation burst"
(16f, Edit PRO × 2 + Interpolate)
```

**Phase 2 ek saldırı — Holy Cross:**
```
"sovereign forms giant holy cross symbol with both arms, divine energy channeling,
preparing cross-shaped beam attack, imposing crucifix stance"
```

---

## ═══════════════════════════════
## FAZ 3 — SECONDARY CLASS ÜRETİMİ (OUTLINE)
## ═══════════════════════════════

*Faz 2 tamamlanınca detaylandırılır.*

### Karakterler (4 yeni class)

| Class | Kimlik özeti | Ghost renk |
|-------|-------------|-----------|
| **Ravager** | Berserker, dev balta, kan sistemi | #FF3322 |
| **Ronin** | Kendo/iaido, kılıç hızı, hassasiyet | #FFFFFF |
| **Gunslinger** | Silah çekiş ustası, ısı/Heat mekanizması | #FFB800 |
| **Brawler** | Dövüşçü, punch finisher, Charge banking | #FF8800 |

Her class için workflow: F2 ile aynı. `GHOST_ATTACK_SPEC.md` ghost prompts hazır.

### Act 3 Mobs

Tasarım: `TASARIM/COMBAT_ROSTER.md` Act 3 bölümü (+ EchoHound, FractureBorn eklenecek)
Üretici: Kiro bulk + kullanıcı elitler

### Boss: Echo Twin
256px × 2, FAZ 2 boss kalitesinde — kullanıcı Aseprite

---

## ═══════════════════════════════
## FAZ 4-5 ÜRETİM (OUTLINE)
## ═══════════════════════════════

### Faz 4 Karakterler
| Class | Ghost renk |
|-------|-----------|
| **Summoner** | #22FF88 |
| **Hexer** | #CCFF00 |

### Faz 5 Boss: The Architect
320px — ekranı doldurur. En büyük üretim. Faz 5 başlangıcında planlanır.

---

## GENEL ÜRETİM KURALLARI

1. **Kiro yapar:** Tüm mob sprite + anim (bulk, deterministik)
2. **Kullanıcı yapar:** Player attack/V anim + boss sprite/anim (kalite kritik)
3. **Her armored variant:** base hazır → edit-images-v2, 1g/sprite
4. **Ghost attack sprite:** her class base anim hazır olduktan sonra
5. **18 Nisan başlangıç sırası:**
   ```
   → Warblade base (18_NISAN_PIXELLAB_TALIMAT.md referans)
   → Act 1 floor/wall tileset (referans: 18_NISAN Bölüm 3)
   → FractureImp sprite + anim (combat test için şart)
   → ShardWalker sprite + anim
   → VoidThrall + HalfThrall sprite + anim
   → Warblade ghost attack + V + RMB
   → Kalan Act 1 mobs (Kiro'ya)
   → Penitent Sovereign boss (kullanıcı)
   ```
