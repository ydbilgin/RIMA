# Weapon Variation Extension — Karar #124 + #125 Önerisi

**Author:** rima-design (Opus)
**Date:** 2026-05-14, S69 sonu
**Extends:** Karar #123 (Yol A weapon decouple)
**Status:** PROPOSED (locked değil — orchestrator confirm bekliyor)

User spec (2026-05-14):
> "bazı durumlarda silahın formları değişebilir artık bu yeni kararımızla bunlar da mümkün. ekstra silah da takabilir aynı değil farklı bi türünü ama classıyla uyumlu gibi"

Karar #123 weapon decouple iki yeni mümkünlük açtı: (1) runtime weapon **form variation** (aynı arketip, farklı sprite), (2) class'a **ek farklı tür silah** attach. Bu doküman ikisini ayrı karar olarak tasarlar.

---

## Bölüm 1 — Weapon Form Variation (Karar #124 öneri)

### Trigger Architecture

Form değişimi 3 trigger kategorisine bağlanır — her biri mevcut locked sisteme yapışır:

1. **T3 Empowered Skill (Karar #122 T3)** — Brief swap (1.5-2s skill cast süresi). Empowered Iron Combo cast → greatsword cyan-cracked form, cast bitince base form. Bu MVP-friendly çünkü transient — animation clip aynı, sadece sprite swap WeaponDatabase üzerinden.
2. **Run-Persistent Tier (Karar #18 Relic uyumlu)** — Treasure room / mini-boss drop / elite reward "Rift-Cracked Greatsword" relic'i Warblade'in greatsword sprite'ını run boyunca değiştirir. Sadece **görsel + flavor** — stat bloat YOK (Karar #18 koruması). Mekanik buff Relic kendi içinde, weapon sprite değişimi ayrı bir layer (visual feedback).
3. **Echo Imprint / Cross-Class Bond (Karar #5/#7 T4 Rift Proc)** — 3 Family Tag stack tetiklenince primary weapon brief glow shader + secondary form swap (örn. Warblade greatsword T4 trigger sırasında void-empowered sprite 4-5s, sonra normale döner). Karar #122 T4 white-cyan flash ile sync.

**Trigger seçimi:** Üçü de geçerli, **birbirini tamamlar**. Faz 1 sadece (1) kullanır (T3 brief swap), Faz 2 (2) + (3) eklenir.

### Visual Spec (per-class form set)

Production cost reality check: 10 class × N form × 8 dir × (idle+anim varyasyon) = patlama. Bu yüzden **form sprite weapon-only** — body etkilenmez. Karar #123 Level 1 (orbit attach) ile weapon idle pose tek sprite, Level 2 polish'te per-frame anchor zaten variant'lara serbest açılır.

Form variation matrix (önerilen MVP+Faz 2 scope):

| Class | Base | Tier 2 (Rift) | Tier 3 (Void/Empowered) | Notlar |
|---|---|---|---|---|
| Warblade | Çelik greatsword | Rift-cracked greatsword (cyan/violet crack) | Void-empowered (T4 white-cyan emissive) | Karar #80 silhouette sabit, sadece sprite detail |
| Ranger | Compound bow | Rift-strung bow (cyan tendon highlight) | Void bow (T4 violet aura layer) | Bow shape değişmez, accent/glow swap |
| Shadowblade | Twin reverse-grip blades | Rift-shard blades (cyan crack) | Void blades (T4 phase trail emissive) | Asimetrik twin — her ikisi senkron |
| Elementalist | Floating rune disc (Karar #59) | Cracked rune disc | Void rune disc | Disc Unity VFX'i + sprite swap, hibrit |
| Ravager | Dual hatchets | Bloodied hatchets (rust/crimson) | Frenzy hatchets (T4 red emissive) | Rift theme değil, "blood" theme (kimlik) |
| Ronin | Katana | Spirit katana (cyan steam glow) | Void katana | Sheath (body sprite) değişmez (kimlik) |
| Gunslinger | Dual rift-tech pistols | Overcharged pistols (orange emissive) | Void pistols | Karar #71 holster state korunur |
| Brawler | Bare fists + leather wrap | Brass-knuckle wrap | Void-empowered wrist glow | Wrap = body part, wrist glow = Unity light |
| Summoner | Soul lantern | Cracked lantern (cyan flame intensity up) | Void lantern (T4 violet flame) | Karar #96 cyan+violet palette uyumlu |
| Hexer | Curse staff (green flame tip) | Cracked staff (green→cyan flame mix) | Void staff (green→white flame) | Karar #97 yeşil kimlik korunur |

**Production economy (PixelLab Create Image Pro batch, Karar #90):**
- Form variation = weapon-only sprite (Karar #123 sayesinde body etkilenmez)
- 32px batch ekonomisi: tek generation = 64 cell → 10 weapon × 3 form × 2 var = 60 cell, **TEK BATCH'TE biter**
- Style Reference workflow (base weapon = anchor, T2/T3 = enhance prompt)
- Cleanup: 5-10 dk per weapon → 60 weapon × 7 dk = ~7 saat manual polish
- **Total Faz 1: ~1 saat generation + ~7 saat cleanup = 8 saat.** Faz 2 scope hedefi makul.
- **Faz 1 MVP scope: Warblade base + Tier 2 (Rift-cracked) only = 2 sprite, ~30 dk.** T3 cast empowered visual demo için yeterli.

### Animation Impact

**Sıfır.** Karar #123 decouple sayesinde aynı animation clip (HandAnchor.localPosition) tüm form'ları sürer — weapon sprite swap GameObject child reference değişimi, animation curve etkilenmez. Karar #120 Split-Animation Technique apex frame'i de form-agnostic (apex = anchor pozisyonu, weapon sprite o anchora atanır). Smear frame Karar #120 ile uyumlu — smear weapon sprite'ı ayrı bir varyant olabilir (extra cost) veya base sprite stretch shader ile yapılır (sıfır cost). Recommendation: shader stretch.

### Per-Class Form Granularity

- **Warblade:** Base / T2 Rift / T3 Void = 3 form (Iron Combo Empowered showcase için ideal)
- **Ranger:** Base / T2 Rift / T3 Void = 3 form (Aim Empowered showcase)
- **Shadowblade:** Base / T2 Rift / T3 Void = 3 form (phase-strike showcase)
- **Elementalist:** Base / Fire / Frost / Lightning / T3 Trinity = 5 form (Elementalist'in zaten 3 element switch state'i var — bunu form variation'a açmak doğal, **bonus identity**)
- **Ravager / Ronin / Gunslinger / Brawler / Summoner / Hexer:** Base + T2 + T3 = 3 form
- **Toplam:** 9 × 3 + 1 × 5 = **32 weapon form sprite** (Faz 2 hedefi)

### Cross-System Fit

- **Karar #18 (no equipment slot):** KORUNUR. Form variation visual-only veya Relic-driven (Relic zaten Karar #18 sistemi, slot ekleme yok).
- **Karar #71 (silah hep elde):** KORUNUR. Form değişimi swap-in-hand, hide/sheath YOK (Ronin/Gunslinger istisnaları korunur).
- **Karar #80 (silhouette bible):** KORUNUR. Form variation **silüet'i değiştirmez** — weapon shape sabit, sadece sprite detail/color/emission değişir. Bu zorunlu kural — form variation silüeti kırarsa o variant REJECT.
- **Karar #109 (ambient idle):** Etkilenmez. Idle anim weapon sprite-agnostic.
- **Karar #122 (Echo Resonance Tier system):** T3 empowered skill cast → form swap **NATIVE INTEGRATION** (rima-design recommendation: T3 visual signature olarak form swap kullan).

### rima-design Recommendation: Karar #124

**LOCK with reduced MVP scope.**

- **Faz 1 MVP:** Sadece T3 Empowered Skill brief swap (1 class = Warblade, 1 form pair = Base/T2 Rift). Cost: ~30 dk PixelLab + Unity 1-line sprite swap kod.
- **Faz 2:** 9 class × 3 form + Elementalist 5 form full matrix.
- **Trigger architecture:** 3-layer (T3 transient + Relic persistent + T4 brief flash) — hepsi Faz 2.
- **No code burden until decouple Level 1 working.** Form variation Level 1 üzerine sıfır maliyetle kurulur.

---

## Bölüm 2 — Extra Weapon Attach (Karar #125 öneri)

### Class Uyumluluk Matrix

Her class için "compatible secondary weapon" önerisi. Kriter: **(a) class identity'yi güçlendirmeli, (b) silüet'i kirletmemeli (Karar #80), (c) Karar #71 single-state ile uyumlu olmalı, (d) production cost makul.**

| Class | Primary | Önerilen Secondary | Slot | Mekanik Rol | Recommendation |
|---|---|---|---|---|---|
| Warblade | Greatsword (2-hand) | **Shield-strap on back** (body sprite'da, parry trigger) | Visual + skill prop | Parry skill visual cue | LOCK Faz 2 |
| Ranger | Compound bow | **Tactical dagger on belt** (close-range last-resort) | Visual only | Identity flavor | LOCK Faz 2 |
| Shadowblade | Twin blades | **Throwing daggers belt loop** | Visual + skill prop | Ranged proc skill (aux) | LOCK Faz 2 |
| Elementalist | Floating disc | **Spell-book hover** (yanı sıra ek rune disc) | Visual only | T3 multi-element showcase | REVISE — disc zaten Unity VFX, ek sprite drift riski |
| Ravager | Dual hatchets | **Throwing hatchet belt loop** (1 ranged proc) | Skill prop | Aux ranged | LOCK Faz 2 |
| Ronin | Katana + sheath | **Tanto on hip** (close-quarter execute) | Skill prop | Iaido execute mekanik | LOCK Faz 2 (signature fit) |
| Gunslinger | Dual pistols | **Bandolier/grenade belt** (Unity VFX prop, ammo display) | Visual + UI | Reload skill visual | LOCK Faz 2 |
| Brawler | Bare fists | **Boxing wrap stays (no extra)** veya **brass knuckle T3 upgrade** | Form variation | Karar #124 ile birleş | REVISE → Karar #124 form variation'a aktar (extra weapon değil form) |
| Summoner | Soul lantern | **Bone fetish on hip** (summon trigger prop) | Skill prop | Sacrifice skill visual | LOCK Faz 2 |
| Hexer | Curse staff (sağ) + grimoire (bel) | **Curse trinket on neck** (hex trigger) | Skill prop | T4 Rift Proc trigger | LOCK Faz 2 |

### Slot Architecture — Karar #18 Conflict Position

**rima-design KARAR: HYBRID (preserve + extend).**

Karar #18 LOCKED: "Hybrid: Relic (2+1 garantili/run) + Skill Modifier (2+1 garantili/run). Ekipman slot yok, stat bloat yok."

**Önerilen yorum:** Karar #18 **stat-bearing equipment slot**'u yasaklıyor (zırh/silah/aksesuar slot economy). Extra weapon attach bu sınıfa girmiyor çünkü:

1. **Extra weapon = SKILL PROP veya VISUAL ONLY.** Stat vermez, slot ekonomisi açmaz.
2. Bazı extra weapon (Warblade kalkan, Ronin tanto, Shadowblade throwing dagger) **belirli bir skill'in görsel partneri** — Karar #18'in "Skill Modifier" kategorisine doğal yapışır.
3. Diğerleri (Ranger belt-dagger, Gunslinger bandolier) **pure visual flavor** — hiçbir mekanik açmaz, hiçbir slot işgal etmez.

**Slot kuralı:**
- Class doğuştan sahip olduğu secondary weapon = **identity** (Ronin sheath gibi — Karar #71 istisnası). Run boyunca DEĞİŞMEZ.
- Run sırasında "yeni extra weapon" KAZANILMAZ. Yani Warblade run ortasında "kılıç + kalkan" buluşturmaz; Warblade KENDİ kalkanıyla başlar (class identity).
- **Bu Karar #18'i KIRMAZ.** Equipment slot ekonomisi açılmaz, sadece her class'ın "doğuştan yanında getirdiği aksesuar weapon" tanımı genişler.

**Alternatif (REJECT):** Run sırasında "extra weapon drop" → drop economy + slot UI + stat bloat → Karar #18 KESİN ihlali. REJECT.

### Gameplay Function

Extra weapon iki kategoriye ayrılır:

**Kategori A — Skill Prop (gameplay-relevant):**
- Warblade kalkan → Parry skill'in görsel cue'su. Skill cast olunca kalkan sırttan ele iner (Unity 0.3s tween), parry window aktif, sonra geri döner. Karar #71 "silah hep elde" KORUNUR çünkü kalkan secondary, primary greatsword hep elde.
- Ronin tanto → Iaido execute skill'in görsel partneri. Katana sheath'ten draw olurken tanto sol elden execute.
- Shadowblade throwing dagger → Aux ranged proc skill. Belt loop'tan throw, sonra "regenerate" (sprite respawn).
- Ravager throwing hatchet → Aynı pattern, aux ranged.
- Hexer curse trinket → T4 Rift Proc tetiklenince trinket emissive glow (Unity light).

**Kategori B — Visual Only:**
- Ranger belt dagger, Gunslinger bandolier, Summoner bone fetish → Class kimliği görsel desteği. Mekanik yok, sadece silhouette zenginleştirici.

### Production Cost

- **Kategori A** (5 class × 1 secondary): Decouple weapon sprite + skill animasyon override (kalkan parry tween, tanto draw, dagger throw). **PixelLab cost:** 5 weapon sprite × 1 form = 5 cell (32px batch'in %8'i, bedava). **Animation cost:** Skill clip başına 1 ek HandAnchor (offhand) per-frame data = Karar #123 Level 2 zorunlu. Per-skill ~10 dk anchor data entry × 5 skill = 50 dk.
- **Kategori B** (5 class × 1 secondary): Pure sprite, body sprite'a baked veya static child SpriteRenderer. **PixelLab cost:** 5 sprite = 5 cell. **Animation cost:** Yok (body sprite'ında veya static child).
- **Toplam Faz 2 cost:** ~1 saat PixelLab + ~1 saat Unity setup + Karar #123 Level 2 upgrade prerequisite.
- **Faz 1 MVP scope: HİÇBİR EXTRA WEAPON.** Faz 1 Warblade primary greatsword decouple Level 1 yeterli, kalkan Faz 2.

### Per-Frame Anchor (Karar #123 Level 2)

Extra weapon Kategori A gerektirir Level 2 — primary HandAnchor + OffHandAnchor iki transform animation curve'le sürülür. Karar #123 zaten Level 1 → Level 2 migration "painless, single line code change" diyor. Memory cost: per-class anim başına 2 anchor × 8 frame × 8 yön = 128 keyframe (Vector2). Unity AnimationCurve memory bu trivial — 1 KB per anim. **Codex review sorusu:** Bu sayı 9 anim × 10 class = 90 anim → 90 KB toplam, sıfır endişe.

### Cross-System Fit

- **Karar #18 (no equipment slot):** HYBRID position — equipment slot economy açılmaz, extra weapon class identity'sinin parçası, run-içi drop yok.
- **Karar #71 (silah hep elde):** GENİŞLEYEN YORUM. Primary weapon hep elde KEEP. Secondary weapon "yanında" (kalkan sırtta, tanto belde, dagger belt loop) — single-state kuralı SADECE primary için. Karar #71 metnindeki "Ronin sheath/draw" + "Gunslinger kılıftan çekme" istisnaları zaten secondary weapon state'ini onaylıyor — Karar #125 bu istisnayı genelleştiriyor.
- **Karar #80 (silhouette bible):** Her class için secondary weapon **silüet ekler ama signature'ı bozmaz**. Warblade greatsword + sırt-kalkan → silüet "greatsword + back-mass" doğal. Karar #80 silhouette spec güncelleme: her class için "primary + secondary silhouette signature" yeniden dokumentasyon (Faz 2 başında).
- **Karar #99 (silah kamera uyumluluk):** GENİŞLEYEN UYGULAMA. Secondary weapon da ~35° top-down'da net dikey siluet oluşturmalı — kalkan sırtta yatay duruyorsa görünmez, dikey strap-mounted olmalı.
- **Karar #109 (ambient idle):** Bazı class idle pose'larını zenginleştirir. Warblade kalkan-tap fidget, Shadowblade dagger-flip, Ronin tanto-grip check — class personality boost. Faz 2 polish layer.
- **Karar #122 (Echo Resonance Tier):** T4 Rift Proc Bond → secondary weapon emissive glow (Hexer trinket, Warblade kalkan rune crack). T4 visual showcase için doğal kanal.
- **Karar #5/#7 (Cross-class shadow echo):** Secondary weapon CROSS-CLASS PROC TRIGGER OLMAZ. Phantom Echo weapon-baked OK (Karar #122 + #123 mevcut spec) — secondary weapon Echo trigger'a karışmaz, sadece primary class'ın oyun-içi kimliği.
- **Karar #123 (weapon decouple):** Level 2 per-frame anchor PREREQUISITE Kategori A için. Level 1 MVP scope dışı.

### rima-design Recommendation: Karar #125

**LOCK (with HYBRID Karar #18 interpretation) but DEFER all production to Faz 2.**

- **Faz 1 MVP:** SIFIR extra weapon. Warblade greatsword primary decouple Level 1 yeterli.
- **Faz 2:** 10 class secondary weapon roster (8 LOCK + 1 REVISE Elementalist + 1 REVISE Brawler-to-form-variation).
- **Faz 3:** Karar #80 silhouette bible v2 update (primary + secondary signature).
- **Karar #18 stance:** HYBRID — equipment slot yok, drop economy yok, class identity-bound secondary weapon kavramı genişletildi.
- **Karar #71 stance:** Mevcut istisna pattern genelleştirildi (Ronin sheath/Gunslinger holster zaten secondary state örnekleri).

---

## Bölüm 3 — Cross-System Conflict Audit

| Karar | Conflict? | Resolution |
|---|---|---|
| #5 / #7 (Shadow Echo + Resonance Altar) | Karar #124 T3 swap doğal extension, Karar #125 trigger DEĞİL | NO CONFLICT |
| #18 (no equipment slot) | Karar #125 potansiyel conflict | RESOLVED via HYBRID — slot economy açılmaz, identity-bound only |
| #71 (silah hep elde single-state) | Karar #125 secondary weapon state | RESOLVED via interpretation — Karar #71 PRIMARY için, Ronin/Gunslinger istisnaları zaten template |
| #80 (silhouette bible) | Karar #124 form variation, Karar #125 secondary silhouette | RESOLVED — form variation silüet sabit (sadece detail), Karar #80 v2 update Faz 3 |
| #99 (silah kamera uyumluluk) | Karar #125 secondary weapon kamera | EXTENSION — secondary weapon da dikey siluet kuralı |
| #109 (ambient idle) | Karar #125 idle pose enrichment | OPPORTUNITY — Faz 2 polish katmanı |
| #122 (Echo Resonance Tier) | Karar #124 T3 swap native trigger, Karar #125 T4 emissive | NATIVE INTEGRATION |
| #123 (weapon decouple Yol A) | Direct foundation for #124, prerequisite Level 2 for #125 Kategori A | EXTENSION layer |
| #120 (split-animation) | Karar #124 form variation apex agnostic | NO CONFLICT |

**Hiçbir locked karar BREAK olmuyor.** Karar #18 sadece HYBRID yorum genişlemesi alıyor (rima-design otoritesiyle, "no stat slot" özü korunur).

---

## Bölüm 4 — Final Recommendation

### Karar #124 (Weapon Form Variation)

**rima-design recommendation: LOCK with reduced Faz 1 MVP.**

- **Konu:** Decoupled weapon'ın runtime sprite swap'la form değişimi (tier evolution, T3 empowered, T4 proc).
- **Faz 1 MVP scope:** 1 class (Warblade) × 1 form pair (Base/T2 Rift) = 2 sprite, ~30 dk PixelLab + 1 saat Unity kod. T3 Empowered Skill brief swap showcase.
- **Faz 2 scope:** 10 class × 3-5 form full matrix (~32 weapon form sprite total, ~8 saat).
- **Trigger:** T3 transient (Karar #122) + Relic persistent (Karar #18) + T4 brief flash (Karar #122).
- **No code burden** beyond Karar #123 Level 1 (sprite swap = 1-line WeaponDatabase lookup).
- **Cross-system:** SIFIR conflict, NATIVE Karar #122 entegrasyon.

### Karar #125 (Extra Weapon Attach)

**rima-design recommendation: LOCK (HYBRID Karar #18) but full DEFER to Faz 2+.**

- **Konu:** Class identity'sine bağlı, run boyunca sabit secondary weapon. Skill prop veya visual flavor. Equipment slot economy AÇMAZ.
- **Faz 1 MVP scope:** SIFIR. Warblade primary greatsword decouple yeterli.
- **Faz 2 scope:** 10 class secondary roster (8 LOCK + Elementalist REVISE + Brawler REVISE-to-Karar-#124).
- **Faz 3 scope:** Karar #80 v2 silhouette bible update (primary + secondary signature pairs).
- **Slot architecture:** HYBRID — Karar #18 "stat-bearing equipment slot" yorumu KEEP, "class identity-bound secondary visual/skill-prop" extension açılır.
- **Prerequisite:** Karar #123 Level 2 (per-frame anchor) Kategori A skill props için zorunlu. Faz 2 başlangıcında migration.

### Karar #18 Pozisyon

**HYBRID** (preserve "no stat slot" core, extend "class-identity secondary weapon" boundary). Stat ekonomisi kapalı kalır, Relic + Skill Modifier sistemi tek progression channel olarak korunur. Secondary weapon = sprite + animation prop, drop chain'inde değil.

### Faz 1 MVP Scope Inclusion

- **Karar #124:** EVET (1 class × 1 form pair, 30 dk PixelLab + 1 saat Unity). T3 Empowered showcase için kanıt yeterli.
- **Karar #125:** HAYIR. Faz 1 sıfır extra weapon, Warblade primary decouple yeterli. School deadline 25 gün — extra scope creep riski yüksek.

### School Deadline Reality Check (25 gün)

S70 Hafta 1: Yol A foundation. Karar #124 MVP (Warblade Base + T2 Rift) bu hafta sonu **30 dk PixelLab + 1 saat Unity** ile sıkıştırılabilir, T3 Empowered showcase için iyi marketing hook. **Recommendation: Hafta 2 sonu polish task olarak ekle, P1 değil P2.**

Karar #125 Hafta 1-3 KESİNLİKLE GİRMEMELİ. Faz 2+ scope.

---

## Bölüm 5 — Codex Review Soruları

Codex production feasibility review için 7 spesifik soru:

1. **PixelLab Create Image Pro batch cost — form variation:**
   Warblade 3 form × 8 dir = 24 weapon sprite (32px). Karar #90 batch ekonomisi 32px = 64 cell/generation. Bu 24 sprite tek batch'te biter mi, yoksa multi-batch gerekiyor mu? Cleanup cost per sprite tahmini (Aseprite manual polish)?

2. **Unity AnimationCurve memory — Level 2 per-frame anchor + secondary anchor:**
   Karar #123 Level 2 + Karar #125 OffHandAnchor = anim başına 2 anchor × ~8 frame × 8 yön = 128 keyframe. 9 anim × 10 class = 90 anim. Per-anim memory cost gerçek byte sayısı? Total project memory etkisi <100 KB makul mu?

3. **WeaponDatabase + form swap implementation pattern:**
   T3 Empowered Skill cast → 1.5-2s weapon sprite swap → cast bitince revert. Single SpriteRenderer.sprite assignment yeterli mi yoksa GameObject pool gerekli mi? Şu anki Karar #123 Level 1 implementation'la doğrudan uyumlu mu?

4. **Karar #80 silhouette bible v2 update scope:**
   10 class × (primary + secondary) silüet signature dokümantasyonu. Mevcut CLASS_SILHOUETTE_BIBLE.md format yeterli mi, yoksa secondary için ayrı bölüm gerekiyor mu? Faz 3 timing realistic mi?

5. **Karar #71 single-state interpretation extension:**
   Secondary weapon "yanında durur" (kalkan sırtta, tanto belde) — body sprite'a baked mi yoksa static child SpriteRenderer mi? Production cost ve animation maintenance farkı?

6. **Cross-class Echo proc + form variation interaction:**
   Karar #122 Phantom Echo = weapon-baked OK (0.4s brief). Eğer player'ın o anki weapon form'u T2 Rift-cracked ise, phantom da T2 mı yoksa Base mi olarak doğmalı? Combat readability açısından hangi karar daha iyi okunur?

7. **Faz 1 MVP scope marginal cost — Warblade form variation showcase:**
   Hafta 2 sonu polish slot'una 30 dk PixelLab + 1 saat Unity sıkıştırılabilir mi, yoksa school deadline 25 gün baskısıyla Faz 2'ye atılmalı mı? Codex implementation risk değerlendirmesi.

**En kritik 2 soru:** #2 (memory cost Level 2 + secondary anchor) ve #6 (cross-class Echo proc + form interaction).

---

## Summary

**Karar #124 (form variation):** LOCK + Faz 1 MVP 30 dk showcase yeterli, Faz 2 full matrix. Karar #122 native integration.

**Karar #125 (extra weapon):** LOCK + Faz 2 defer, HYBRID Karar #18 yorumu. Class identity-bound, slot economy açılmaz.

**Karar #18 pozisyon:** HYBRID (preserve core, extend identity-secondary boundary).

**Hiçbir locked karar BREAK olmuyor.**

**Combat readability:** Form variation silüet-stable (Karar #80 KEEP); secondary weapon silüet enrich ama signature-preserve (Karar #80 v2 update Faz 3).

**Faz 1 MVP impact:** Sadece Warblade form variation showcase (30 dk + 1 saat). School deadline 25 gün baskısı altında kabul edilebilir polish task.

**rima-design final authority confirm:** Bu iki Karar (#124 + #125) Karar #123 (Yol A) extension'ı olarak tutarlı, locked sistemleri kırmaz, production economy makul, combat clarity korur. **Önerilen: orchestrator LOCK ile MASTER_KARAR_BELGESI'ne #124 + #125 olarak ekle.**
