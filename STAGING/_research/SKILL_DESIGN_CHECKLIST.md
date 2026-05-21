# RIMA Skill Design Checklist

**Kullanım:** Yeni skill tasarlarken (özellikle az-skill class'lara expansion için — Ronin 4→12, Gunslinger/Ravager/Hexer/Brawler/Summoner 8→12-15) bu checklist'ten geç.

**Filozofi:** Basit tut, scope dar, mekanik şişirme YOK. Bir skill ancak A+B+C+D bölümünden GEÇERSE roster'a girer.

---

## A. PRODUCTION FEASIBILITY

A skill ÖNCE production-mümkün olmalı. Beğenilen ama yapılmayan skill = boşa zihin.

### A1. Pattern Reuse Check
Mevcut 5 Unity utility class'tan biri yeterli mi?
- [ ] `ChainBinder.cs` (caster↔target tether/chain/beam)
- [ ] `SequentialStrike.cs` (multi-jump chain attack)
- [ ] `ProjectileFanSpawner.cs` (arc/spread shot)
- [ ] `PlacedEffectSpawner.cs` (trap/sigil/totem placement)
- [ ] `AfterimageTrail.cs` (dash/teleport)
- [ ] Yeni pattern lazım ⚠️ → re-think

**Yeni pattern gerekiyorsa:** Skill scope için aşırı maliyet. Mevcut pattern'lara uyacak şekilde **redesign** önerilir.

### A2. PixelLab Gen Maliyeti
- [ ] EASY: Mevcut anim spritesheet re-use (0 ek gen)
- [ ] EASY+: 1 yeni anim spritesheet (10-15 gen unit)
- [ ] MEDIUM: 1 yeni anim + 1 yeni VFX texture (20-30 gen)
- [ ] HARD: Birden fazla yeni asset (50+ gen)
- [ ] HARD+: Yeni mob/karakter gen gerekli ⚠️ → re-think

**HARD/HARD+** = skill scope'a sığmaz. Pattern reuse veya simplification gerekir.

### A3. Unity Süre Tahmini
- [ ] LOW: <1 saat (mevcut utility hook-up)
- [ ] MEDIUM: 1-3 saat (yeni state machine + tuning)
- [ ] HIGH: 4-8 saat (yeni component + multi-system entegrasyon)
- [ ] VERY HIGH: 1+ gün ⚠️ → re-think

**Phase 1 Demo için:** Sadece LOW/MEDIUM skill'ler kabul.

### A4. Asset Reuse Maximization
- [ ] VFX bir başka skill'den re-use edilebilir mi (color tint)
- [ ] Mob hit reaction generic frame mı, özel mi
- [ ] Audio cue mevcut SFX bank'ten mi?

---

## B. VISUAL CLARITY

Player skill'i 1-saniyede tanımalı.

### B1. Tek-Frame Tanınırlık
- [ ] Skill'in en aktif frame'i tek başına ne yaptığı belli mi?
- [ ] Color palette skill kategorisini iletiyor mu? (kırmızı=damage, mavi=ice, mor=curse, vs.)
- [ ] Karakter pose'u skill direction'ı söylüyor mu?

### B2. Class İçinde Distinct Mı?
- [ ] Bu skill class'ın diğer skill'lerinden görsel olarak ayrışıyor mu?
- [ ] Aynı pose + aynı VFX rengi başka bir skill'de var mı? ⚠️
- [ ] Skill name + visual eşleşmesi okunabilir mi? (örn. "Iron Crush" = ağır vertical slam, "Cleave" = wide horizontal arc)

### B3. Class Color Theme Uyumu
| Class | Primary Color |
|---|---|
| Warblade | Steel + kırmızı pulse |
| Ronin | Beyaz blade flash + sakura pembe |
| Gunslinger | Narıncı muzzle flash + duman gri |
| Ranger | Altın ok trail + yeşil natural |
| Elementalist | Element-specific (cyan/red/blue/purple) |
| Shadowblade | Mor + siyah duman + kırmızı kan |
| Ravager | Yoğun kırmızı + kahverengi rage |
| Hexer | Mor + yeşil curse |
| Brawler | Narıncı energy + gri stone |
| Summoner | Altın + cyan spirit |

- [ ] Skill VFX class color theme'i ile uyumlu mu?

---

## C. MECHANICAL DESIGN

Skill gameplay olarak değer yaratmalı.

### C1. Counterplay Window
- [ ] Telegraph süresi var mı (0.2-0.8s windup)?
- [ ] Mob dodge edebilir mi (cast sırasında hareket sansı)?
- [ ] Skill instant-cast değilse → fine; instant-cast ise → damage cap kontrolü

### C2. Damage / Effect Band
- [ ] Damage value class'ın damage band'inde mi?
- [ ] Cooldown skill power'a göre dengeli mi?
- [ ] Resource cost (varsa) class economy'ye uygun mu?

### C3. Tag System Uyumu
- [ ] Skill tag'ı (Strike/Outlet/Surge/Burst/Sustain/Channel) belli mi?
- [ ] Class'taki diğer skill tag'larıyla synergy potansiyeli var mı? (Tag Synergy Phase 2+)
- [ ] Echo Imprint variant olabilir mi? (Strike Form / Outlet Form / Surge Form upgrade)

### C4. Build Variety Katkısı
- [ ] Bu skill yeni bir build path açıyor mu, yoksa mevcut skill'in tekrarı mı?
- [ ] Class roster'ında benzer fonksiyonlu başka skill var mı? (örn. "Cleave" + "Whirlwind" ikisi de AOE — fark netleştirilmiş mi?)

---

## D. SCOPE GATE

Phase 1 / Phase 2+ / cut karar.

### D1. Phase 1 Demo Inclusion
- [ ] **Class identity için MUST-HAVE** mi (örn. Warblade'in Cleave'i, Ranger'ın Aimed Shot'ı)?
- [ ] Demo combat loop için kritik mi?
- [ ] Production maliyeti LOW/MEDIUM mı (A3)?

**YES → Phase 1 demo'ya dahil**

### D2. Phase 2+ Expansion
- [ ] Class roster'da skill sayısını dengelemek için mi (Ronin 4→12 yolunda)?
- [ ] Build variety + replay depth için "nice to have" mı?
- [ ] Production maliyeti uygun mu (HIGH değil)?

**YES → Phase 2+ patch content**

### D3. Cut
- [ ] Pattern reuse imkansız + yeni pattern aşırı maliyet?
- [ ] Class içinde benzer skill zaten var?
- [ ] Visual clarity sağlanamıyor?

**YES → DROP**

---

## Pratik Örnek — Ronin 4→12 Expansion Adayları

Mevcut Ronin: Final Draw / Iaido Stance / Quickdraw / Sakura Veil (4)

Eklenmesi düşünülenler (örnek 8 skill):

### 5. "Wind Step" — DASH
- A1: ✅ AfterimageTrail.cs reuse
- A2: ✅ EASY (sword swing reuse)
- A3: ✅ LOW (<1 saat)
- B1: ✅ Tanınır (mor afterimage)
- B2: ✅ Quickdraw'dan farklı (dash != slash)
- C1: ✅ Cooldown 3s
- C3: Surge Form (Dash kategori)
- D1: Phase 1 candidate ✅

**VERDICT: APPROVED Phase 1**

### 6. "Crimson Storm" — N-jump chain attack
- A1: ✅ SequentialStrike.cs reuse
- A2: ✅ EASY (dagger cut + blood reuse)
- A3: ✅ MEDIUM (2-3 saat hookup)
- B1: ✅ Sakura petal + kırmızı blood trail
- B2: ✅ Sakura Veil'den farklı (n-target sequential)
- C1: ✅ Charge time 0.5s
- D1: Phase 1 candidate ✅

**VERDICT: APPROVED Phase 1**

### 7. "Ki Channel" — Persistent damage aura
- A1: ⚠️ Yeni pattern gerek (persistent aura ticking)
- A2: MEDIUM (1 yeni VFX gen)
- A3: HIGH (3-5 saat aura system)
- B1: ⚠️ Iaido Stance ile karışabilir (ikisi de defensive aura)
- C2: ⚠️ Resource cost gerekli
- D1: ❌ Phase 2+ (production maliyeti yüksek + B2 fail)

**VERDICT: DEFER Phase 2+**

### 8. "Shogun's Wrath" — Ultimate, persistent buff + multi-strike + tether
- A1: ⚠️ Birden fazla pattern lazım (Persistent buff + SequentialStrike + Tether)
- A2: HARD (2-3 yeni gen)
- A3: VERY HIGH (1+ gün)
- D3: Cut → Yeni pattern aşırı maliyet

**VERDICT: CUT** (mevcut Ronin "Final Draw" ultimate role'ünü dolduruyor)

---

## Toplu Checklist Şablonu (yeni skill için)

```markdown
## Skill: [Name]
**Class:** [Class]
**Tag:** [Strike/Outlet/Surge/Burst/Sustain/Channel]

### A. Production
- Pattern: [ChainBinder/SequentialStrike/ProjectileFan/PlacedEffect/AfterimageTrail/NEW]
- PixelLab Gen: [0 reuse / 1 new / 2+ new]
- Unity Hours: [LOW/MED/HIGH]

### B. Visual
- 1-second readable: [Yes/No]
- Class-distinct: [Yes/No]
- Color theme: [matches/conflict]

### C. Mechanical
- Telegraph: [s]
- Damage band: [in/out class average]
- Tag synergy: [skills it pairs with]

### D. Scope
- Verdict: [Phase 1 / Phase 2+ / CUT]
- Rationale: [1 sentence]
```

---

## Bu Checklist'in Kullanım Senaryoları

1. **Az-skill class expansion** (Ronin 4→12, Ravager/Hexer/Brawler/Summoner/Gunslinger 8→12-15) — yeni skill önerirken bu checklist'i her aday için doldur.
2. **Mevcut skill audit** — 115 skill'in her biri için bu checklist'i geri-doldur (post-demo balance pass).
3. **Class identity gap** — bir class roster'ında eksik tag/role varsa, hangi pattern'le doldurulabileceği belli olur.
4. **Demo cut decision** — production scope aşan skill'ler hızlı tespit edilir.

---

## Tek Cümle Özet

**Skill = (Pattern reuse) × (Asset reuse) × (Visual distinct) × (Counterplay) × (Phase 1 fit).** Bunlardan biri fail → CUT veya Phase 2+ defer.
