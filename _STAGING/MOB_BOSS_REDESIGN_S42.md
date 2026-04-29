# RIMA Mob/Boss Redesign — S42 Faz 1

> Faz 1 (Warblade-only) için 8-mob roster + Penitent Sovereign 3-fazlı boss redesign.
> Mevcut COMBAT_ROSTER.md/BOSS_DESIGN.md baz, kullanıcı feedback'iyle revize:
> "sıradan melee auto-attack yapan mob olmamalı, her mob karakteristik skill kit'e sahip olmalı".
> Kaynak conflict: memory > TASARIM. Memory `project_mob_sprites.md` boyut hiyerarşisi (48→160px) korundu, COMBAT_ROSTER ile mob isim seti büyük ölçüde örtüşüyor; davranış spec'i baştan yazıldı.

---

## Tasarım Felsefesi

Faz 1 redesign'ın çıkış noktası: **her mob bir oyuncu alışkanlığını kırar VE bunu spesifik bir skill ile yapar.** Auto-attack zayıflatıldı (Bruiser/Hulk dışında temas hasarı yok denecek kadar düşük), tehdit tamamen telegraphed skill window'lardan geliyor — Hades dilinde "tell → window → punish".

Üç yapısal kural:
1. **Auto-attack ≤ %25 DPS:** Mob'un toplam tehdit çıkışının çoğu skill'lerden gelmeli. Pasif "yürü-vur-ölüm" mob YASAK.
2. **Telegraph + window <2s:** Class hız feedback'i (`feedback_class_speed_design.md`) gereği "bekle kazan" tasarımı yasak. Tell maksimum 1.5s, window minimum 0.4s. Pencere içinde dodge/parry/burst lazım.
3. **RIMA Fracture Trait pozitif spec:** Generic ARPG mob (orc/skeleton/zombie) yerine her mob "Fracturing'i nasıl yaşadı, vücudunda hangi spesifik iz kaldı" tek cümle ile tanımlanmalı. Warblade'in "fractured vanguard armor" mantığı silüet imzasına uygulanır.

Faz 1'de Warblade tek class — roster onu ÖZELLİKLE zorlamalı: greatsword melee (slow swing), dash (kısa CD), Rage resource (hit aldıkça dolar). Roster'da bu üç özelliğe karşılık üç anti-pattern var: hit-and-run (dash punisher), ranged kite (gap closer zorunlu), burst window'da pesek bruiser (Rage harcatır).

Faz 2-aware: roster'daki 2 mob (Warden Echo, Shard Walker) ranged class'ları da tehdit edecek — gap-closer/dash-immune mekanik ile.

---

## Mob Roster (8)

### M01 — Fracture Imp
- **Role:** Trash / Swarm
- **RIMA Fracture Trait:** Çatlaktan fışkırmış, gövdesinde ışık sızdıran açık deliklerden hâlâ rift enerjisi damlıyor.
- **Silüet İmzası:** 48×48, sivri uzun kollar, gövde küçük üçgen, başı silüetin %40'ı (orantısız büyük).
- **HP/Tier:** 25 HP (base 100 referans, x0.25). Damage tier S (low).
- **Auto-attack:** Yok. Temas hasarı 5 (negligible — deflect bile etmez).
- **Skill 1: Rift Lunge** — 0.4s cup-back tell (vücut germe), 0.5m ileri sıçrayış + 12 hasar bite. Recovery 0.6s (savunmasız window).
- **Skill 2: Death Splatter** — Ölünce 0.3s sonra 1m radius rift goo zemine düşer (3s slow %20). Sürünün dağıtılma yönünü zorlar.
- **Warb Counter:** LMB sweep tek vuruşta öldürür. Dash attack 3-4 imp grubunu temizler (Rage +12). Ana sorun: 4'ü birden geldiğinde flank, lunge'ları üst üste bindirip Rage burnout.
- **Roster Synergy:** Shard Walker'la → Walker odağı çekerken Imp'ler arkadan lunge. Splatter goo'lar Warb dash lane'ini kapatır.

### M02 — Shard Walker
- **Role:** Ranged Caster
- **RIMA Fracture Trait:** Vücudu yarısından kopmuş kristal levhalardan oluşuyor; her atışta bir levha kendinden kopup fırlatılıyor (gövdesi atış başına küçülüyor).
- **Silüet İmzası:** 112×112, dik dar gövde, omuzdan dışarı uzayan 3-4 keskin shard (silüette dikenli profil), bacak silüeti minimum.
- **HP/Tier:** 75 HP. Damage tier M.
- **Auto-attack:** Yok.
- **Skill 1: Triple Shard** — 0.8s tell (gövde kasılır, omuzdaki 3 shard parlar), 0.2s'lik gap'lerle 3 ardışık projectile (15° yelpaze). Her shard 18 hasar. Pierce yok.
- **Skill 2: Fracture Burst** — Death triggered. 0.5s sonra 2m radius patlama (25 hasar). Cesedin yanında dur YASAK; Warb burst-finish ile self-damage riski.
- **Warb Counter:** Dash forward (triple shard yelpazesini geç), LMB-LMB-Ram (3-hit'te öldür, ama Ram knockback'i death burst'ten kaçınmak için kullan). Faz 2 ranged class'lar için: dodge roll lazım, projectile reflect lategame.
- **Roster Synergy:** Penitent Bruiser ile → Bruiser'ı kite ederken Walker arkadan triple shard. Imp ile → Walker'ı vurmaya çalışırken Imp lunge.

### M03 — Seam Crawler
- **Role:** Skirmisher (hit-and-run)
- **RIMA Fracture Trait:** Zemindeki çatlaklarda yaşıyor; vücudu sadece zemine bakan yüzeyden görünür, üst silüeti bir parça ışık ve gölge.
- **Silüet İmzası:** 96×96 ama yatay (geniş, 30px yüksekliğinde), zemine yapışık siluet, sadece omurga ve 6 pençe çıkıntısı belirgin.
- **HP/Tier:** 60 HP. Damage tier M.
- **Auto-attack:** Yok.
- **Skill 1: Submerge** — Pasif. Crawler %50 zaman zemin altında (görünmez ama 1.5m radius'ta dim distortion shader). Underground iken hasar almaz.
- **Skill 2: Burst Strike** — 1.0s yer altı approach (gölge oyuncuya doğru kayar — telegraph!), zeminden fırlayıp 1m radius bite (28 hasar + 0.5s knockback). Burst sonrası 1.4s exposed (Warb burst penceresi).
- **Warb Counter:** Gölgeyi takip et, fırladığı an dash-back + LMB combo. Submerge'deyken hit alamaz — Crawler'ı çıkmaya zorlamak için harcamadan beklemek lazım, ama "bekle kazan" değil çünkü submerge süresi 2s cap.
- **Roster Synergy:** Chain Warden ile → Warden zincirleri Warb'ı kilitlerken Crawler burst strike. EN ZORLU FAZ 1 KOMBOSU.

### M04 — Penitent Bruiser
- **Role:** Bruiser (telegraphed heavy)
- **RIMA Fracture Trait:** Kendine ceza olarak Fracturing enerjisini içine kapattı; göğsünden mor ışık dalgaları yayılıyor (aura görsel ipucu).
- **Silüet İmzası:** 128×128, omuzları aşağı çökmüş, kambur gövde, kollar kısa-kalın, baş eğik (silüet "bowing" pose'u).
- **HP/Tier:** 180 HP. Damage tier L.
- **Auto-attack:** Slow melee swing — 1.0s telegraph, 30 hasar. KULLANIMI: ana tehdit DEĞİL, oyuncu yakına gelince zorla mesafe açtırır.
- **Skill 1: Anti-Heal Aura** — Pasif, 3m radius. İçindeyken Warb'ın lifesteal/heal %50 azalır. Aura görseli mor dalga (sürekli aktif).
- **Skill 2: Penitent Surge** — 1.2s tell (yumruk yere kalkar), 3m radius AOE itme + 35 hasar + 0.5s stagger. Window 1.5s recovery (Rage burst penceresi).
- **Warb Counter:** Aura'yı zorla — 3m içine girip Surge'ünden dodge, sonra LMB sweep. Boss Bruiser değil, MID-tier bruiser. Rage burst window'da yık.
- **Roster Synergy:** Imp ile → aura içinde Imp goo + Bruiser swing = sandwich.

### M05 — Chain Warden Echo
- **Role:** Charger / Mobility Punisher
- **RIMA Fracture Trait:** Eski hapishane muhafızının yankısı; zincirleri artık fiziksel değil, rift enerjisinden örülmüş — fırlatıldığında havada parlıyor.
- **Silüet İmzası:** 128×128, ağır zırh göğsü, omuzlardan 2 zincir uçuşan (silüette dinamik tendril hareketi), miğfer yüzü kapalı.
- **HP/Tier:** 140 HP. Damage tier L.
- **Auto-attack:** Yok.
- **Skill 1: Triple Chain** — 0.7s tell (zincirleri geriye sallar), 3 zincir 45° yelpazede fırlatılır (her biri 6m menzil). Hit edilen Warb 1.5s slow %50. Telegraph net (chain havada görünür).
- **Skill 2: Chain Pull** — 1.0s tell (zincir oyuncuya kilitlenir), Warb 4m boss yönüne çekilir + 20 hasar. Dash-immune (dash atılsa bile pull tamamlanır — bu RANGED CLASS PUNISHER mekanik). Pull sonrası 1.2s window.
- **Warb Counter:** Triple Chain → dash side. Chain Pull → erken parry timing veya pull sonrası burst. Pull dash-immune olduğu için Faz 2 Ranger/Gunslinger için gerçek tehdit.
- **Roster Synergy:** Seam Crawler + Warden = en yüksek gerilim; Pull sonrası exposed Warb'a Crawler burst.

### M06 — Relic Caster
- **Role:** Summoner / Spawner + buff support
- **RIMA Fracture Trait:** Eski mühür büyücüsü; elinde tuttuğu kırık relikvarın içinden minik rift fragmanları çağırıyor.
- **Silüet İmzası:** 80×80 (KÜÇÜK — execution priority cue), ince uzun gövde, elinde silüette belirgin dik kristal kırığı.
- **HP/Tier:** 50 HP (most fragile). Damage tier S kendisi, M sumon.
- **Auto-attack:** Yok.
- **Skill 1: Summon Shardling** — 1.5s channel (görsel: relikvar parlar, oyuncu görsün diye uzun tell), 1.5m radius'ta 1 Shardling spawn (Shardling = mini Imp, 15 HP, 8 hasar dash). Cooldown 6s. Kanaldayken Caster savunmasız.
- **Skill 2: Aegis Mark** — 0.5s tell, en yakın allied mob'a 3s damage shield (%50 reduction). Cooldown 5s. Mark'lı mob silüette altın aura ile belirginleşir (oyuncu önceliği değiştir cue'su).
- **Warb Counter:** EXECUTION TARGET. Önce Caster, sonra diğerleri. Caster channeling sırasında dash-in + LMB combo tek sırada öldürür. Mark ekonomisi: Mark süresi içinde diğer mob'a saldırma ZAYIF, Mark cooldown'unda diğerlerine yüklen.
- **Roster Synergy:** Chain Warden + Caster → Warden Mark'lı, Pull immune, Warb kapana sıkışır. Faz 1'in en taktiksel encounter'ı.

### M07 — Riftbound Augur
- **Role:** Debuffer / Disruptor
- **RIMA Fracture Trait:** Fracturing sırasında bir kahin'in geri dönüşü — gözleri rift enerjisiyle dolu, etrafındaki tüm zaman algısını bozuyor.
- **Silüet İmzası:** 96×96, eğik duruş, başında gözleri yerine 3 küçük rift fragmanı (silüette belirgin tepe çıkıntısı), uzun pelerin (alt silüet genişliyor).
- **HP/Tier:** 70 HP. Damage tier S kendisi (debuff focus).
- **Auto-attack:** Yok.
- **Skill 1: Mark of Folly** — 1.0s tell (Augur Warb'a bakar, gözünden mor ışın çizilir), Warb'a 5s "Folly" debuff: dash CD %50 uzar. Tell süresinde sight-break (Warb arkasını dön) ile iptal edilir — RIMA'da unique mekanik.
- **Skill 2: Time Shudder** — 0.8s tell (yere çöker, etrafına dim ripple), 4m radius alan 2s "stutter" (içindeki Warb skill animasyonları %30 yavaşlar — Rage gain de yavaşlar). Window içinde alandan çıkmak veya dash kullanmak gerek.
- **Warb Counter:** Sight-break Mark of Folly'yi iptal eder (kaçınılır debuff). Time Shudder içinde değil dışında savaş — pozisyonel öğreti. Augur kendisi fragile, mark/shudder cooldown'larında temizle.
- **Roster Synergy:** Penitent Bruiser ile → Bruiser aura içinde + Augur Time Shudder = Warb hem heal yok hem yavaş.

### M08 — Hollow Hulk
- **Role:** Elite variant / Mini-boss tier (rare spawn)
- **RIMA Fracture Trait:** Eski dev muhafız golemi; Fracturing içini boşalttı, hâlâ büyük ama gövdesi içinde rift enerjisi sallanan bir kovuk.
- **Silüet İmzası:** 160×160, masif kareli omuzlar, kollar gövdeden büyük, miğfer yok (boyun çukurundan rift ışığı çıkıyor — silüet "headless titan").
- **HP/Tier:** 350 HP. Damage tier XL ama low frequency.
- **Auto-attack:** Yok (yumruk swing skill'e dönüştü).
- **Skill 1: Quake Slam** — 1.4s tell (kol yukarı kalkar, zemin titrer), 3m radius zemin AOE (60 hasar). Window 1.8s recovery. Tell uzun ama hasar büyük — ilk öğretici "büyük hasar = uzun tell" mob'u.
- **Skill 2: Cavity Pulse** — Pasif. HP %50 altına düştüğünde ACTIVATES: her 4s'de göğsündeki rift kovuğundan 2m radius pulse (25 hasar). Hulk'a yakın savaşmayı tehlikeli yapar. Phase change cue.
- **Skill 3: Fracture Charge** — 1.0s tell (omuzunu indirir, ışık birikir), 6m düz çizgi charge (40 hasar + knockback). Dash perpendicular ile dodge. Window 2.0s.
- **Warb Counter:** False threat DEĞİL artık — gerçek mini-boss. Quake telegraph net ama overlapping skill'leri okuma testi. Cavity Pulse aktifken Hulk içinde değil dışında savaş, Charge sonrası burst window.
- **Roster Synergy:** Chain Warden ile → Warden Pull Warb'ı Hulk'un Quake AOE içine çeker. Solo spawn'da bile mini-boss tier challenge.

---

## Mini-Boss / Boss

### MB01 — Penitent Sovereign (Faz 1 Act Boss, 3 Phase)

**Lore concept:** Bölgenin eski koruyucusu, kendini cezalandırmak için rift enerjisini içinde hapsetti. Zincirleri artık fiziksel kilit değil, kendi öz disiplinin metaforu — kırıldıkları an yeni bir form ortaya çıkıyor.

**Arena:** 14×14 tile dairesel taş platform (Act 1 ritüel kalıntısı). 2 dash lane karşılıklı (kuzey-güney). Faz 2'de orta noktada Rift Tear hazard belirir, dash lane'ler tek-yönlü olur.

#### Phase 1 — "Zincirin Altında" (HP 100 → 66%)

Telegraphed kit, oyuncu öğrenir.

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Chain Whip** (baseline) | 0.8s — kol geriye | 6m düz çizgi 30 hasar | 1.0s |
| **Penitent Surge** (telegraphed AOE) | 1.2s — yumruk yere | 4m radius itme + 35 hasar | 1.5s |
| **Shackle Cast** (control) | 1.0s — zincir havalanır | 8m menzil tek hedef chain → 2s slow %50 + 15 hasar | 1.2s |

Phase 1 ritmi: Whip → Surge → Whip → Shackle. Pattern öğreti, Warb dash + LMB ile alıştırma.

#### Phase 2 — "Kırılan Zincir" (HP 66 → 33%)

**Geçiş sahnesi (1.5s):** Sovereign yere çöker, göğsünden mor ışık fışkırır, "...artık yetmez" → zincirleri kırılır, hız +%30. Arena merkezinde Rift Tear (3m radius hazard) belirir, sürekli aktif.

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Fracture Strike** (combo) | 0.5s — dash forward | 3 ardışık swing (sol-sağ-orta), her biri 22 hasar | 0.8s son swing'den sonra |
| **Chain Detonation** (zone control) | 1.0s — zincir parçaları zemine saplanır | 3 nokta marker, 2.5s sonra her biri 2m radius patlama (40 hasar) | 1.5s (parça yerleştirme sonrası) |
| **Shackle Cast** (carry-over) | 1.0s | Phase 1 ile aynı | 1.2s |

Phase 2 baskısı: Rift Tear orta hazard + chain mines + 3-hit combo = arena managment. Warb dash lane'lerinde patrol, mine yerleşim sonrası burst window.

#### Phase 3 — "Sovereign Awakened" (HP 33 → 0%)

**Geçiş sahnesi (2.0s):** Sovereign havaya kalkar, kalan tüm zincirler erir, gövdesi yarısı rift enerjisiyle değişir. Müzik tema değişir. Hız +%50 (Phase 1'e göre toplam).

| Skill | Tell | Etki | Window |
|---|---|---|---|
| **Fracture Charge** (gap closer) | 0.6s — başlangıç pozisyonu glow | Arena boyunca dash + 50 hasar düz çizgi | 1.5s charge sonrası |
| **Sovereign's Wrath** (arena AOE) | 1.5s — zemine kök salar, çevre kızarır | Tüm arena hasar (60) HARİÇ orta 2m güvenli daire | 2.0s recovery |
| **Chain Detonation** (faster) | 0.7s tell, 4 nokta, 1.5s patlama | Phase 2 versiyonu hızlandı | 1.0s |
| **Fracture Strike** (combo, faster) | 0.4s | 3-hit combo +%20 hasar | 0.6s |

Phase 3 desperation: Wrath'ın güvenli dairesi merkezde ama orada Phase 2'den kalan Rift Tear var (oyuncu yön seçer: dış kenar = Charge riski, iç merkez = Tear hazard). 30-45s kill hedefi.

**Ölüm sahnesi (2.5s):** Sovereign çöker, son söz "...sonunda boş", zemin çatlar → secondary class seçimi açılır (Faz 2 unlock cue, Faz 1'de placeholder credits).

---

## Encounter Design Kuralları

**Spawn rules (Faz 1 Combat odası bütçesi 8-12 threat point):**
- Imp: 1pt, max 4 anda
- Walker: 3pt, max 2
- Crawler: 3pt, max 2
- Bruiser: 4pt, max 1
- Warden: 4pt, max 1
- Caster: 4pt, max 1
- Augur: 3pt, max 1
- Hulk: 8pt, max 1 (Elite/Unknown only)

**Anti-pattern (asla aynı odada birlikte):**
- Bruiser + Warden + Hulk → 3 ağır mob, Warblade tempo'sunu öldürür
- 2× Caster → çift Aegis Mark loop, hiçbir mob ölmez
- Augur + Hulk → Time Shudder içinde Quake dodge imkansız
- 4× Imp + Crawler + Walker → görsel kalabalık, target priority confusion

**Recommended encounters:**
- "Triple Threat" (8pt): Walker + Imp×3 + Crawler → ranged + swarm + flank, AoE/single test
- "Lockdown" (10pt): Warden + Crawler + Imp×3 → en yüksek Faz 1 baskısı
- "Execution Test" (9pt): Caster + Warden + Imp×2 → priority + chain pull
- "Aura Trap" (10pt): Bruiser + Augur + Walker → heal yok + slow + ranged
- "Mini-Boss Solo" (8pt, Elite oda): Hollow Hulk solo (3-skill testi)

---

## Faz 2 Hazırlığı

**Ranged class (Elementalist/Ranger/Gunslinger) geldiğinde:**

- **Chain Warden Echo:** Chain Pull mekanik dash-immune; Faz 2'de ana ranged class punisher. Revize gerekmez.
- **Shard Walker:** Triple Shard projectile reflect Faz 2'de aktif (`EnemyProjectile.cs` deflect). Walker'ın silüeti deflect cue'su olarak parlama eklemeli (visual feedback).
- **Seam Crawler:** Submerge mekanik ranged class için tehlikeli (görünmez approach). Faz 2'de "underground tracking" UI marker eklenebilir (Ranger trap synergy).
- **Riftbound Augur:** Mark of Folly Faz 2'de class-specific debuff'a evrilir (Elementalist mana drain, Ranger trap CD, Gunslinger heat). Faz 2 task.

**Yeni mob ihtiyacı (Faz 2 Act 2):**
- Anti-projectile mob (sentry / reflect bariyeri) — Goldfract Sentry COMBAT_ROSTER'dan Act 3'ten Act 2'ye taşınabilir.
- True channeling caster (Warblade'i kanal kesimine zorlayan, ranged class için projectile race) — yeni tasarım.

**Penitent Sovereign Faz 2'ye taşınmıyor** — Faz 1 act boss olarak kalacak. Faz 2'de Echo Twin (mevcut BOSS_DESIGN.md) kullanılır, dual-class mirroring testi.

---

## Tasarım Tradeoff'ları (LABEL/WHY/NEXT/STATUS)

**LABEL:** 8 mob (7+Hulk) seçimi, 6 mob alternatifine karşı
**WHY:** 6 mob roster (Imp/Walker/Crawler/Bruiser/Warden/Caster) Faz 1 core öğretiyi karşılar AMA debuffer (Augur) ve mini-boss tier (Hulk) eksik kalır. Augur'sız "status priority" öğretilemez (Faz 2 Withered Apostle çok geç). Hulk'sız Elite/Unknown odası yeterince ödüllü hissetmez. 8 mob production cost ~25% artırır (PixelLab generation + Codex import) ama gameplay variance ve faz öğreti gradient'i için zorunlu.
**NEXT:** Codex'e MOB_SPRITE_PIPELINE.md update (Augur+Hulk ekle, Penitent → Bruiser rename), rima-asset'e silüet prompt'ları.
**STATUS:** locked

**LABEL:** Auto-attack ≤%25 DPS kuralı
**WHY:** Mevcut COMBAT_ROSTER'da çoğu mob "yürü-vur" davranışı var (Bruiser slow melee, Warden chain throw, Crawler bite). Kullanıcı feedback "sıradan melee yapan olmamalı" gereği auto-attack tehdit kaynağı OLAMAZ — sadece pozisyonel pressure aracı (yakına gelirse zorla mesafe açtır). Trade-off: bazı mob'lar (Bruiser) "tank-melee" arketipini KAYBEDİYOR, "telegraphed AOE bruiser" oluyorlar. Bu Hades/Last Epoch mob diline yakın ama klasik ARPG (Diablo, PoE) mob diline ters. RIMA Fractured Epic ton Hades+Last Epoch ekseninde olduğu için OK.
**NEXT:** Combat tuning Faz 1 playtest sonrası — bazı auto-attack hasarları yükseltilebilir eğer "bekle kazan" oluşursa.
**STATUS:** locked

**LABEL:** Penitent Sovereign 3-phase (mevcut 2-phase yerine)
**WHY:** Mevcut BOSS_DESIGN.md'de 2-phase. Kullanıcı "Hades-style telegraph→dodge→punish" istedi, Hades bossları typically 3-phase (intro/mid-pressure/desperation). 2-phase yapı mid-pressure katmanını atlıyor, oyuncu "kit öğren → boss zaten ölüyor" hissediyor. 3-phase ile mid-phase mekanik (Rift Tear hazard + chain mines) class-arena interaction öğretiyor; bu Faz 2 Echo Twin ve Faz 3 Fracture Sovereign için temel. Trade-off: boss kill süresi 2-3dk → 3-4dk (uzun ama Faz 1'de tek boss olduğu için OK). Fight 4dk üzeri çıkarsa Phase 1 daha kısa kesilir.
**NEXT:** BOSS_DESIGN.md update gerekecek (rima-doc), Codex'e BossPhaseTransition.cs için phase 3 hook.
**STATUS:** locked
