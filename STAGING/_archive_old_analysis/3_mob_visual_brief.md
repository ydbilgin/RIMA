---
status: DRAFT
faz: 1
tarih: 2026-05-14
ozet: "3 F1 mob (Plate Widow / Rift Hound / Hollow Arbiter) visual brief — Create Image Pro hazırlık"
source: rima-design Opus brief, Karar #98 LOCKED mob canon
---

# 3 F1 MOB VISUAL BRIEF
**LOCKED REFERENCES:** Karar #98 (rift cyan+violet palette), Karar #74 (mob size 2^n), Karar #100 (chibi 64+35°), Karar #112 (Shard/Trace/Awakening lore glossary), Karar #91 (3-channel telegraph for hazards).

**Faz 1 ekosistem fit:** F1 Shattered Keep biome — Penitent Sovereign'in boyun eğmiş muhafız mirası. Mevcut 8 mob roster: Fracture Imp (swarm 48px), Shard Walker (ranged 112px), Seam Crawler (horizontal 96×30px), Penitent Bruiser (bruiser 128px), Chain Warden Echo (charger 128px), Relic Caster (small 80px), Riftbound Augur (debuffer 96px), Hollow Hulk (mini-boss 160px).

**Silüet ayrışma matrisi (3-saniye glance test):** Mevcut F1 mob silüet alfabesi → fragmented thin (Imp), tall thin spiked (Walker), horizontal ground-hugging (Crawler), hunched bulky (Bruiser), chain-tendril armored (Warden), small upright relic (Caster), cloaked stooped (Augur), headless mass (Hulk). Bu 3 yeni mob bu silüet alfabesinde EŞSİZ slot doldurmalı.

---

### MOB: Plate Widow
**Role:** Elite (T2-tier defensive predator, F1 elite room cap eki)
**Size:** 128×128 px, PPU=64 (Karar #74 elite class)
**Act:** F1 (Shattered Keep) — Penitent doctrine'inin kadın muhafız varyantı

**Silhouette (combat readability):**
Tall bipedal female form, geniş plate omuzlar V-shape oluşturur ama dış hatları **kırılmış levha katmanları** — düzgün knight değil, fragmented samurai oyo kabuğu hissi. Pose: wide-stanced düşük merkez ağırlık, sol kol önde defensive guard (kalkan değil — kol plakası kalkan görevi görüyor), sağ kol yan-arkada gizli pençe/jambia. Baş kapalı kask altında — yalnızca dar cyan iki göz yarığı. 128px thumbnail'de **trapezoidal armored top + narrow waist + planted leg base** üçgen siluet okunur, Penitent Bruiser'ın **yuvarlak hunched mass** silüetinden net ayrışır.

**Identity (kim/ne):**
Plate Widow = Penitent Sovereign'in ilk muhafız konseyinde yer alan, "Yas Tutan" rütbesi taşıyan kadın çelik-disiplinist. Sovereign zincirlerini kıramadan önce kendi ölümünü bekliyordu (cellât rolü); Fracturing onu o anda dondurdu — şimdi her ziyaretçide eşinin (Sovereign) intikamını arayan widow rolünde. Lore beat: F1'deki bazı ruined chapel odalarında 2-3 Widow heykeli görünür → environmental storytelling; oyuncu canlı Widow ile karşılaşınca "bu heykellerden biri" tanıması.

**Color palette (Karar #98 align):**
- Primary: #3A2D3A (faded mourning-violet plate, biome-fit muted desaturated)
- Secondary: #1F1822 (deep shadow recess, plate undercut)
- Accent (rift cyan): #00E5C8 (plate fracture seam glow + helm visor slit pair)
- Damage variant: #6B1A2A (split armor crack reveal — Faz 2 elite tint için reserve)

**Attack archetype:**
Melee parry-into-counter + tek seferlik forward lunge.
- T1: Shoulder-Plate Slam — 1.0s tell (sol omuz öne döner), 1.2m radius arc 25 dmg, parry penceresi geniş (öğretici).
- T2 (elite hazırlığı): Mourning Veil — 1.4s tell, kendisi etrafına 0.8s reflect shield (knockback projectile mob threat-disc reflect), Penitent ekosistem teması korunur ("kendini cezalandır → karşıdakini cezalandır"). T2 cooldown 14s.

**Distinguishing visual cue:**
- vs **Penitent Bruiser:** Widow tall + planted ve plate-laminated; Bruiser hunched + cloth/leather; Bruiser göğsünden mor pulse yayıyor, Widow plate joint cyan seam ışığı.
- vs **Chain Warden Echo:** Warden chain-tendril şovu yayar (silhouette wild), Widow chain YOK — sadece statik plate authority.
- vs **Hollow Arbiter:** Widow kapalı/armored kütle, Arbiter cavity-open exposing-interior (decisively farklı: closed-shell vs open-shell).
- vs **Hollow Hulk:** Hulk headless titan kütlesi 160px; Widow 128px insan-formu okunabilir kask başı var.

**PixelLab idle Create Image Pro prompt hint:**
"128x128 pixel art chibi top-down armored rift widow, tall female figure in fragmented dark mourning-violet plate armor #3A2D3A with cracked seam lines glowing faint cyan #00E5C8, helm fully closed with narrow horizontal visor slit pair, wide-stanced defensive pose with left forearm raised in plate-guard across torso and right arm coiled back at hip, layered shoulder plates angled outward in trapezoidal silhouette, narrow armored waist, planted leg base, hairline cyan crack veins along plate joints only — no full body glow, muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64."

**rima-asset prompt revize notu:** Mevcut prompt (idle_batch SECTION B Mob 05) ana çerçevede doğru fakat "insect segments" benzetmesi YANLIŞ — Widow disciplined human-knight aesthetic, böcek değil. Ayrıca "wide and flat-footed" → "wide-stanced + narrow armored waist" değişimi silhouette trapezoid'unu netleştirir.

---

### MOB: Rift Hound
**Role:** Grunt / Skirmisher (T1 fast attacker)
**Size:** 96×96 px, PPU=64 (Karar #74 small/orta mob; Seam Crawler 96 ile aynı tier ama silhouette form farklı)
**Act:** F1 (Shattered Keep) + ileride F2 echo varyant aday

**Silhouette (combat readability):**
Quadruped beast — köpek-benzeri ama anatomisi rift-distorted: ön bacaklar arka bacaklardan **belirgin uzun**, omurga sırtta yüksek tepe çıkıntısı, kafa düşük ve ileri-uzun. Pose: low coiled hunting stance, ağırlık tamamen ön ayaklarda, kuyruk değil — **tendril void-tail trail** (ışık değil, partikül kuyruğu siluet sınırlı). Kafada gözler yerine 2 dar cyan slit. 96px thumbnail'de **horizontal-arched quadruped + raised back ridge + low head forward**: Crawler'ın yere-yapışık-altı-pence horizontal'inden net ayrışır (Crawler tile-flat, Hound coiled-springy).

**Identity (kim/ne):**
F1'deki tutsak hayvanlar ve muhafız köpekleri Fracturing'de eridi. Plural — onlar bir kişi değil, **Shackle Pack** kollektifi. Lore beat: F1 prison/keep koridorlarında zincirlenmiş duvar halkalarından kopmuş, hâlâ koşma refleksleri var. Penitent Bruiser ve Widow gibi insanca yas tutamıyorlar — sadece av olan her şeyi takip ediyorlar. F1 ecology'sinde "wild rift" temasını dengeliyor (insan-yapısı muhafız mob'larının yanında animal-instinct mob).

**Color palette (Karar #98 align):**
- Primary: #2A2530 (matted dark fur-stone hybrid, biome muted)
- Secondary: #3D2D26 (warmer hide patches, rust-leather)
- Accent (rift cyan): #00E5C8 (spine ridge crack + eye slit pair + tendril tail trace)
- Damage variant: #4A1818 (exposed muscle/rift wound for Faz 2 hurt state)

**Attack archetype:**
Charge-dash melee — Mobility Punisher aday.
- T1: Lunge Bite — 0.5s telgrf (back coil), 2.5m forward leap + 18 dmg bite + 0.3s knockback. Cooldown 3s. Player için **dash-side counter teaching** mob'u.
- T2 (Faz 1.5+ aday): Pack Howl — 1.2s tell (sırt ridge cyan flash), 4m radius 3s "Marked" debuff aura → o radius içindeki diğer Hound'ların lunge hızı +%25. **Pack synergy mekaniği** (2+ Hound aynı odada elite scaling).

**Distinguishing visual cue:**
- vs **Seam Crawler:** Crawler ZEMINE YAPIŞIK 6 pence; Hound dik dört-ayak + spine ridge tepe. Crawler horizontal-flat-rectangle silüet, Hound diagonal-arched silüet — 96px'te yanyana fark net.
- vs **Fracture Imp:** Imp 48px tiny triangular humanoid; Hound 96px quadruped — boyut + sınıf ayrı.
- vs **Penitent Bruiser:** Bruiser bipedal hunched human; Hound quadruped beast — anatomi sınıfı farklı.

**PixelLab idle Create Image Pro prompt hint:**
"96x96 pixel art chibi top-down rift beast, quadruped canine-like creature with elongated front legs and raised spinal ridge along back, low coiled hunting stance with weight forward on front paws, head held low and forward elongated, dark matted fur-stone hybrid hide #2A2530 with warmer rust-leather patches #3D2D26, hairline cyan #00E5C8 rift cracks tracing along spine ridge and seeping through hide seams, eye slits glowing faint cyan, tendril void-tail trailing wisps behind (silhouette only, not full glow), muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64."

**rima-asset prompt revize notu:** Mevcut prompt (Mob 04) genel olarak doğru fakat **128px size canvas yanlış** — Hound elite değil, grunt — 96×96 olmalı (Karar #74 small/orta mob class, Seam Crawler eşdeğeri). "Hollow eye sockets filled with cyan light" → "narrow cyan eye slits" daha minimalist okunabilir 96px'te.

---

### MOB: Hollow Arbiter
**Role:** Elite (T2-tier zone authority / debuffer)
**Size:** 128×128 px, PPU=64 (Karar #74 elite class)
**Act:** F1 (Shattered Keep) — bölgenin eski yargı ritüellerini sürdüren içi-boş otorite

**Silhouette (combat readability):**
Tall slender bipedal — Widow'un opposite'i: **wrapped cloth + minimal armor**, gövde uzun ve dar, omuz sembolik (kemikli yüksek omuz çıkıntıları ama silhouette ana kütlesi kumaş). **Kritik kimlik öğesi**: göğüs kavitesi **AÇIK** — kumaşın altında dar dikey yarık, içeriden cyan derinlik ışığı (yüzey glow değil, **depth glow through gap**). Pose: rigid upright + her iki kol **omuz seviyesinde açık avuçla yana açılmış** (judge/arbiter "weigh the verdict" duruşu). Baş kapalı kapüşon altında 2 asılı cyan orb. 128px'te **narrow waist + wide shoulders + raised arms with palms outward + tall silhouette + chest cavity slit** kombinasyonu Widow'un kapalı plate trapezoid'undan da, Sovereign'in chained-bipedal'inden de okunabilir şekilde ayrışır.

**Identity (kim/ne):**
Hollow Arbiter = Penitent Keep'in ritüel yargıçlarından birinin Hollow formu — bedeni Fracturing'de boşaltıldı, fakat **rütbe duruşu içgüdüsel olarak kaldı**. Hâlâ her ziyaretçiye yargı yapıyor: göğüs cavity'sinden Trace (Karar #112) okuma jesti benzeri bir hareket geliyor — ama Trace değil, oyuncuya **silence/debuff** uyguluyor. F1 ecology bağı: Sovereign'in yargılayanları (Arbiter'lar), Sovereign'i yargılayan rolündeydi; şimdi hepsi hollowed → "yargı boşaldı" temasını combat'a taşır.

**Color palette (Karar #98 align):**
- Primary: #2D2A35 (dim stone-grey wrapped cloth, biome muted)
- Secondary: #5B4A6E (decayed wisp-violet drapery accent #5A2A8A'ya yakın ama daha soluk)
- Accent (rift cyan): #00E5C8 (chest cavity DEPTH glow + eye orb pair + faint hand-edge wisp)
- Damage variant: #FFCC00 sığ flash (Faz 2 "judgment broken" flash — rezerv)

**Attack archetype:**
Ranged zone debuff + ritual cast — Riftbound Augur ailesine yakın ama yargı/silence teması.
- T1: Verdict Beam — 1.2s tell (chest cavity yarığı parlar), 6m düz dar konide 20 dmg + 2s "Silenced" (skill CD %30 uzar, debuff variant). Telegraph spec Karar #91 3-channel ground shake + outline pulse.
- T2: Final Judgment — 1.6s tell, 5m radius "Marked" zone bırakır 4s; içindeki oyuncu dash yaparsa +12 punitive damage (anti-mobility, dash-baiting teaching). Penitent Sovereign Faz 2 "Rift Tear" ile semantik kuzen ama mekanik farklı.

**Distinguishing visual cue (KRİTİK — Penitent Sovereign overlap riski):**
- vs **Penitent Sovereign (boss):** Sovereign 256px boss + **chains** üzerinde silüet kimliği taşır + arena merkezde sahnesi var. Arbiter 128px elite + **NO chains** + ritual hand-raised-palms-outward pose (Sovereign hand-raised-fist-clench Faz 1, broken-chains-trailing Faz 2). Arbiter chest cavity AÇIK depth glow; Sovereign Faz 2'de göğüs MOR PULSE yayan (full surface emission, depth-glow değil). Bu fark glance test'te okunmalı.
- vs **Penitent Bruiser:** Bruiser 128px ama hunched + cloak; Arbiter 128px tall-rigid + drapery — duruş tipi (hunched-down vs ritual-upright) net ayrışır.
- vs **Riftbound Augur:** Augur 96px stooped + 3 head shard; Arbiter 128px tall + 2 floating cyan eye orbs + chest cavity. Boyut ve tac/baş profili ayrı.
- vs **Plate Widow:** Widow kapalı plate + closed helm; Arbiter açık göğüs cavity + cloth drapery. Closed-shell vs Open-shell antitetik kompozisyon — bu **bilinçli design statement**.

**PixelLab idle Create Image Pro prompt hint:**
"128x128 pixel art chibi top-down hollow arbiter judge, tall slender bipedal authority figure draped in dim stone-grey wrapped cloth #2D2A35 with decayed wisp-violet drapery accents #5B4A6E, hooded head with two suspended cyan #00E5C8 orb eyes, chest cavity visibly open as a narrow vertical slit through the wrapped cloth with cyan depth glow seeping from within (cavity opening only — not surface body glow), rigid upright posture with both arms raised at shoulder height palms turned outward in arbiter judgment stance, narrow waist with raised bony shoulder spikes beneath cloth, fabric trailing at base, no chains anywhere, muted desaturated Salt and Sanctuary tone, view 35 degree high top-down, south facing, PPU 64."

**rima-asset prompt revize notu:** Mevcut prompt (Mob 06) genel istikamet doğru fakat **KRİTİK eksiklik: "no chains anywhere" explicit negative yok** → Sovereign'le drift riski yüksek. Ayrıca "arms slightly raised with open palms" → "arms raised at shoulder height palms turned outward" daha ritualistic okunur (slight raise yetersiz authority). "Towering" kelimesi tehlikeli — 128px Arbiter'ı Hollow Hulk 160px ile karıştırmamak için "tall slender" tercih edilmeli. "Featureless except for two suspended cyan orbs" → "hooded with two suspended cyan orb eyes beneath the hood" daha defined.

---

## Ekosistem-genel notlar

- **Yol A weapon decouple (Karar #123) MOB için uygulanmaMAlı:** Karar #123 player primary için; "Phantom Echo: weapon-baked OK (0.4s brief, drift mümkün değil)" emsali mob'a uygulanır (mob silahları kısa expose). Hound pençe + Widow plate-arm + Arbiter cavity ışığı = body-integrated, ayrı sprite yapısal değer katmaz, sadece pipeline maliyeti artırır.
- **8 yön (Karar #114):** Plate Widow + Hollow Arbiter T2 elite-tier mob → 8 yön zorunlu (5 gen + 3 mirror, ama Widow/Arbiter asymmetric pose nedeniyle full 8 gen önerilir). Rift Hound T1 → 8 yön (Karar #114 "tüm playable + T2 mob + boss" → T1 mob 4-yön opsiyonel; ancak quadruped Crawler gibi 4 yön mob'lar mevcut, Hound 4 yön cost-friendly).
- **Silüet pack test:** Bu 3 mob + 8 mevcut F1 mob = 11 mob roster yan yana 128px thumbnail strip'te combat clarity testi yapılmalı (Faz 1.0 exit criteria önerisi).

---

## Cross-References
- Karar #74 (Boss/Mob size hierarchy)
- Karar #82 (Mob 3-Tier Skill System staged)
- Karar #84 (T2/T3 staged budget — Plate Widow + Hollow Arbiter T2 aday)
- Karar #91 (3-channel telegraph standard)
- Karar #98 (Rift cyan+violet palette LOCKED, bu 3 mob isim canon)
- Karar #100 (chibi 64+35°)
- Karar #112 (Shard/Trace/Awakening glossary)
- Karar #114 (8 yön animation)
- Karar #123 (Yol A weapon decouple — mob uygulanmıyor)
- COMBAT_ROSTER.md (8 F1 mob mevcut roster)
- MOB_COMPOSITION_RULES.md (kompozisyon kuralları)
- BOSS_DESIGN.md (Penitent Sovereign — Hollow Arbiter overlap riski)
