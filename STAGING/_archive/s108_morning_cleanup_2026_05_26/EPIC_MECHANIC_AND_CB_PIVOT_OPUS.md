# EPIC MECHANIC + CB PIVOT — Opus Verdict (2026-05-19 night)

**Bağlam:** User uyuyor. Sabah review edecek. İki paralel deep analiz:
- **A:** CB pivot honest verdict (RIMA bırakılsın mı?)
- **B:** 5 epic RIMA signature mechanic candidate (mechanic bank'tan)

**Honest mod:** Flattery yok. Sunk cost yok. "Hangi proje shippable?" sorusu hakim.

---

# PART A — CB PIVOT VERDICT

## TL;DR — Tek satır

**RIMA continue + Pitch refactor + 1 epic mechanic eklesin. CB pivot YAPMA. Ama CB'nin pitch netliği ödüntü alınmalı.**

CB pivot ÇEKİCİ görünmesinin sebebi yeni oyun olması değil — **tek cümle pitch'ini çözmüş olması** ("Paint the floor. Trigger the chain. Erase the room."). RIMA aynı netliği bulamadığı için cazip duruyor. Ama CB'nin design state'i implementation öncesi vaadlerden ibaret; RIMA'da 6 ay kod + 78 test + 10 char anchor + Brush V1 + Multi-Layer Painter + Map Fragment LIVE var. **Pivot = 6 ay yatırımı sıfırlama + yeni risk** karşılığında **sadece pitch netliği kazanmak**.

Doğru hareket: RIMA'nın pitch'ini CB seviyesine taşı + 1 epic signature mekanik eklem ile "Hades-clone-with-more-classes" labelinden kurtar.

## A1. CB design state honest read

CB **tasarım olarak çok güçlü, kod olarak sıfır**. Realite:

| Boyut | CB durum |
|---|---|
| Vision doc | 770 satır, 34 section, Codex review PASS |
| Pivot decision | 5 gate kriterli koşullu pivot LOCKED |
| Locked kararlar | 30+ (sub-genre / 5 element / 3 class / 7 tile / 3 hibrit / 12-18 modifier / 6 floor / Form CD 45s / $14.99) |
| **Açık sorular** | **5 büyük** — POC bekliyor (map structure, wave vs pre-placed, portal vs corridor, cascade zorlama, exploration miktarı) |
| Unity proje | YOK |
| Kod | SIFIR |
| Asset pipeline | YOK |
| Konsept ismi | finalize bekliyor (Circuit Breaker mı yoksa alternatif mi) |
| Playable POC | YOK (Week -2 task) |

CB'nin **17 alanı locked, 5 alanı açık** — ve açık olanlar oyunun *temel feel* sorunları (map structure, düşman akış paradigması). Bunlar dokümandan çözülmez, POC zorunlu.

**Locked vs Open dengesizliği:** Vision/sub-genre/anti-clone hard rules tartışılmaz, ama **gerçek gameplay shape'i (map flow / wave dinamiği)** hâlâ açık. Yani CB'nin "tightness" hissi büyük ölçüde **sub-genre cümlesinden ve marketing pitch'inden** geliyor — gerçek tasarım katmanından değil.

## A2. CB ship realism vs RIMA ship realism

| Boyut | RIMA (bugün) | CB (pivot edilirse) |
|---|---|---|
| Sub-genre netliği | Amorf ("Hades-like + class depth") | NET ("Real-Time Generative Action Roguelike") |
| Tek cümle pitch | YOK | VAR ("Paint the floor...") |
| Mekanik signature kanıt | %15 (Warblade-only) | %0 (POC yapılmadı) |
| Yatırım kod | 6 ay, 78 test, Brush V1, Multi-Layer Painter, Map Fragment LIVE | 0 |
| Yatırım asset | 10 char anchor + Karar #143 6-layer pipeline + RIMA F1 tilesets | 0 (RIMA F1'i recycle etse bile palette/RuleTile/RoomTemplate üzerinden ~%30 reuse) |
| Solo dev 6-12 ay ship | 1 Act + 4-5 class realistik (cut önerilerini uygularsa) | 16 hafta MVP plan tutturulursa 3 class + 6 floor + 1 boss |
| Asıl risk | Scope drift (10 class × 12 skill × 80 evrim overdesigned) | 2-mod silah + TileStateMachine + hibrit cascade UX *henüz playtest edilmedi* — pitch güzel ama feel ispatlanmadı |
| Pitch kontrol | YOK | VAR |
| Kod riski | KISMI (signature implement %15) | TAM (her şey sıfırdan) |

**Brutal observation:** 16 hafta CB MVP planı *kâğıtta çalışır*. Ama:
1. CB'nin 2-mod silah UX'i playtest gerektirir — POC gate'i geçemezse pivot çürüğe oturur
2. TileStateMachine + 7 tile + 3 hibrit + 12-18 modifier + 6 floor + 1 boss = solo dev için yumuşak hedef değil
3. RIMA'nın "Hades + class depth" konfüzyonu CB'nin "Real-Time Generative" netliğinden daha çabuk fixlenir (1 hafta pitch sprint vs 16 hafta yeni MVP)

**Şu varsayım yanlış:** "CB temiz scope'tan başladığı için daha kolay ship." Yanlış. CB scope'u kâğıtta temiz, *implementation'da scope drift riski aynı*. 5 açık karar (map flow, wave, portal vs corridor) RIMA'nın yaşadığı tile pipeline drift'inin replikası olabilir.

## A3. Pivot cost (RIMA 6-month progress preserved or lost?)

**Asset reuse oranı (CB pivot edilirse):**

| RIMA asset | CB reuse durumu |
|---|---|
| 10 char PixelLab anchor (5000 PixelLab cred allocation) | %0 — CB 32px karakter (RIMA 64px chibi). Boyut farkı + class identity tamamen farklı (Aquamancer/Pyrotechnist/Stormcaller vs Warblade/Elementalist/...). Anchor pipeline öğreniminin %100'ü taşınır, sprite'ların %0'ı |
| Brush V1 (13 sprint LIVE, 328 test PASS, SO contracts) | %60-70 — Tile Palette + RuleTile + Asset Pack Browser + Brush Executor + Room Template SO portable |
| Multi-Layer Painter v1 (Karar #147) | **%0** — CB pivot kararında EXPLICIT: "Karar #143 6-layer painted composition SİLİNECEK". CB pure top-down + state-readability. Multi-Layer Painter yatırımı silinir |
| Map Fragment System (Karar #63 LIVE) | %30 — UI metaforu + drop logic CB run loop için yeniden tasarlanır (CB 6 floor + 1 fork model farklı) |
| Karar #143 6-layer pipeline (S88-S92, ~15+ session) | **SIFIR** — CB için silinecek |
| Wang16 tilesets (11 set, S88) | %50 — placeholder Week 1-8, sonra CB Act 1 yeni gen |
| CombatEventBus + StatusMatrix + Combat v4 | %80 — mimari aynı, content (class skill) farklı |
| Map Designer (Blueprint Profile / Zone System / Adjacency Rules) | %20 — CB için BlueprintZoneSystem defer/optional |

**Kabaca:** RIMA → CB pivot 6 ay yatırımın yaklaşık **%40-50'sini taşır** (kod mimari + bazı tool). **Geri kalan %50-60 (Multi-Layer Painter + 6-layer pipeline + 10 char identity + 64px chibi pipeline + Karar #80 silhouette bible) silinir.**

Bu *küçük bir kayıp değil*. Multi-Layer Painter S92 LATE'de Codex PASS_WITH_REVISIONS + Karar #147 LOCKED — orchestrator drift'i yendi, 4 paralel agent dispatch ile çözüldü. Bu spec sıfırlanır.

**Okul ödevi framing riski:** "Scope refinement" anlatımı plausible ama hocaya 6 ay sonra başka oyun teslim edilmesi *hâlâ pivot risk*. CB pivot decision doc'unda bu risk kabul edildi ama unutulmasın.

## A4. Final pivot recommendation

**TEK EYLEM: RIMA CONTINUE + 1 hafta pitch sprint + 5-7 gün Ronin stress test (önceki Opus verdict).**

Justification:

1. **Pitch refactor 1 hafta < CB MVP 16 hafta.** CB'nin asıl sundurduğu şey ("tek cümle pitch") RIMA'da 1 haftada bulunabilir. RIMA'nın gerçek imzası "10-class × 10 farklı resource ekonomisi". Bu cümleyi pitch'e dönüştür → CB'nin avantajı erir.

2. **CB POC gate'i mevcut RIMA içinde 2 haftada zaten yapılacaktı (Week -2/-1).** Eğer Ronin stress test PASS olursa RIMA imzası kanıtlanır → CB pivot mantığı çürür. FAIL olursa zaten alternatif yol gerekir; o zaman CB pivot ciddi düşünülür.

3. **CB'nin 5 açık sorusu (map flow / wave / portal-corridor / cascade zorlama / exploration miktarı) RIMA'nın yaşadığı drift'in replikası.** Yeni proje "temiz scope" değil — *henüz drift yapmamış scope*. 6 ay sonra CB de benzer drift yapacak (insan + solo dev + ambition + 12-class Faz 4 vision).

4. **Sunk cost trap'e karşı uyarı:** Bu öneri "RIMA'ya yatırım yapıldı, devam" demiyor — *gerçek karar parametresi: pivot CB'nin tek cümle pitch netliğini almak için 6 ay reset değer mi?* Cevap **hayır** — pitch RIMA'da 1 haftada bulunur.

5. **CB ileride yapılabilir.** RIMA Faz 1 ship'ten sonra (8-12 hafta) CB Sezon 2 olarak başlar. Bu CB decision doc'unda da yazılı. Pivot zorunlu DEĞİL.

**Hybrid path (eğer kesin CB istiyorsa):** Mevcut CB Hybrid path'i (Week -3 doc → Week -2/-1 POC → 5 gate) zaten optimal. Onun yerine "now or later" sorgusu: Şimdi pivot edilirse 6 ay reset; sonra pivot edilirse RIMA ship + Sezon 2 CB. **Sonra pivot her açıdan üstün.**

**Conflicts with locked rules:** NONE. Master Karar #143 (6-layer painted) ve Karar #147 (Multi-Layer Painter) RIMA içinde aktif kalır. CB pivot bunları silmek isteyecekti — RIMA continue kararı bu locked kararları korur.

---

# PART B — EPIC RIMA SIGNATURE MECHANIC BRAINSTORM

## Strategi

**Önceki Opus verdict (RIMA_MECHANIC_ANTI_GENERIC_OPUS.md):** RIMA'nın asıl signature *10-class × 10 unique resource economies* — ama kanıtlanmamış (Warblade-only). Sorun: 10 farklı resource bar bile **ekstra mekaniğe değil, sadece varyasyona** dönüşebilir. "Generic değil" iddiası için **resource ekonomilerinin üstünde bir epic verb** lazım.

5 candidate aşağıdaki kriterleri sağlamalı:
- Solo dev implementable (Codex pattern-copy + Opus design judgment ile)
- Hades/DC/Spire klon değil
- *Genuinely talked-about* potansiyeli (TikTok klip, streamer hook, Steam review "this is the thing that...")
- RIMA'nın 10-class × resource ekonomi imzasını **güçlendirir, baltalamaz**
- Implementation cost reasonable (≤ 3-4 hafta solo dev integration)

Mechanic bank scoring sistemi (Easy entry 5/5, Deep master 5/5, Content variety 5/5) RIMA'nın action-roguelite pillar'ında M59-M68 + sonradan eklenen M150 Cascading Trajectory family + bazı abstract primitif (D2 Passive Media, D4 Multi-Gen Tile, C2 Trail-as-Functional, M73 Echo Strikers, M68 Build Synergy Detection, M92 Dual Ledger) RIMA-adapt potential taşıyor.

## B1. 5 Epic Signature Candidates

### Candidate 1 — **Resource Cross-Channel** (R-Cross)

**Pitch (one-liner):** "Her sınıfın resource'u **kendi sınıfından çıkıp** diğer sınıf skill'lerini etkiler — Warblade Rage'i Ronin Tension'a tap edebilir, Elementalist Mana'sı Hexer Stack'i besleyebilir; combo = build-içi enerji ekosistemi."

**Detay:**
- 10 class × 10 resource = 100 (class, resource) çifti
- Cross-class slot ile ikinci class'tan 2 skill alındığında (Karar #24), **kaynak akışı da açılır**: primary class kaynağı → secondary class skill consume edebilir
- Örnek: Warblade primary + Ronin secondary build → Rage stack'i Sakura Veil deflect ile tüketilebilir; Tension stack'i Warblade Beat 3 buff'a kanal edilebilir
- Cross-class tier yükseldikçe (T1/T2/T3) channel verimi artar (%30 → %50 → %70)
- HUD: 2 resource bar yan yana + channel arrow (sol class → sağ class transfer indicator)
- Build identity: "Pure Warblade Rage" vs "Warblade-Ronin Hybrid Tension"

**Source mechanic bank entries:**
- M68 Build Synergy Detection UI (RIMA-anchored) — *named archetype tetiklenince UI bildirir*
- M75 Gossip Relay Buff — *bilgi/buff yan komşuya yayılır*
- M70 Baton Pass (Pokémon mantığı, RIMA action context'e adapt)
- D1 Reset Loop + Knowledge Persistence (resource ekosistemi run-içi knowledge gibi davranır)

**Integration cost:** ~5-7 hafta solo dev
- Tüm 10 class resource SO'ya `crossChannel: CrossClassResourceFlow` alan eklenir
- 10×10 = 100 (consume, gain) çifti tasarım (sadece anlamlı olanlar yazılır, ~30 çift MVP)
- HUD channel indicator (~5 gün)
- Codex pattern-copy ile cross-class skill consume logic
- Balance hell: 100 çiftin %20'si dominant strategy oluşturur — sürekli tuning gerek

**Risk:**
- 9/10 sınıf hâlâ implement edilmediği için çok erken
- Balance complexity: 100 çift cross-pair → 10 sınıf kanıtlanmadan 10×10 ekleyemezsiniz
- "Resource transfer game" başka bir alt-türü besler (Path of Exile mana-leech) — orijinalite riski

**Scores:**
- Signature strength: **8/10** (10-class resource ekonomi imzasını DERINLESTIRIR, başka oyun yapmıyor)
- Ship realism: **4/10** (10 class implement gerekli → 2027+)

---

### Candidate 2 — **Echo Imprint Cascade** (Death-as-Architect)

**Pitch (one-liner):** "Her run'da öldüğün odada **mini-arena imprint** bırakırsın — sonraki run'da o oda fragmentlerle dolu, ölümünün **fiziksel etkisi** bir sonraki run'a bir-kerelik arena modifier olarak işlenir. Dünya seni hatırlıyor."

**Detay:**
- Karar #27 (Echo Imprint sistemi) + Karar #67 (Death Echo / Ghost Trail M67) zaten LIVE değil ama LOCKED
- Bu candidate **mevcut Echo Imprint'i death-as-architect verb'üne dönüştürür**
- Player room X'te öldüğünde: ölüm pozisyonu + son kullanılan skill + son aldığı damage type **environmental seed** olur
- Sonraki run'da aynı room X'te: 1 fragment cluster spawn (eski ölümün geometrisi) + 1 micro-hazard (eski damage type'a göre — ateş kaldıysa burning patch, void kaldıysa rift tear)
- Player aynı odada 3 kez ölürse → "Cursed Room" tag açılır (3+ fragment cluster, daha sert ama daha iyi reward)
- Run-arası kalıcı: lifetime ölüm geometrileri map'e işlenir (10-15 run sonra 100+ death imprint = recognizable cartography of struggle)

**Source mechanic bank entries:**
- D4 Persistent Multi-Generation Tile State — *kuşak boyu tile metadata*
- C2 Trail-as-Functional-Pattern — *hareket izi fonksiyonel*
- M67 Death Echo / Ghost Trail — *Spelunky 2 ghost runs*
- D2 Passive Media → Active Intervention — *pasif anıyı aktif sahnele*
- M93 Memory Distillation — *run sonu hafif meta-modifier*

**Integration cost:** ~3-4 hafta
- DeathImprintSO + per-room persistent storage (player save)
- RoomBank.GetImprintsForRoom(roomId) → instantiate 1 fragment + 1 hazard
- Hazard library 10-15 entry (4 damage type × 3-4 visual variant)
- Run-end save flow (Karar #25 meta progression compatible)
- UI: opening run "Cursed: 12 echoes" indicator

**Risk:**
- Random ölüm spamı = boring "death tax" yerine "talked-about story" — careful balance needed
- Persistent data save scope (mobile/console port düşünülürse cloud sync)
- "Hades but with death persistence" diye olabilir — *implementation kalitesi everything*

**Scores:**
- Signature strength: **9/10** (Hades/DC/Spire YAPMADI — bu gerçek bir verb. Roguelite genre'de death-as-content rare)
- Ship realism: **8/10** (Karar #27 zaten LOCKED, mimari hazır; sadece DeathImprintSO + room storage + hazard library)

---

### Candidate 3 — **Cascading Skill Trajectory** (Aim-Once Resolve-Many)

**Pitch (one-liner):** "Sınıf skill'lerin **trajectory'leri sekiyor** — bir Warblade slash duvarda sekip 2. mob'a vuruyor, Elementalist Frost Orb 3 düşmana zıplıyor, Ranger trap kurulu trap'a temas edince zincir tetik. Tek aim, çoklu resolve."

**Detay:**
- Mechanic bank M150 Cascading Water Drop'tan adapt — FourLeaf Fields cozy farm mekaniği RIMA combat'a port
- Her sınıf skill'inin "bounce/chain" version'u (signature mod): trajectory'sini değiştirir, hit count'u artırır, damage scaling'i değiştirir
- Default skill: damage(X) for N targets (linear)
- Cascade variant (Treasure Room / Echo Imprint reward): damage(X × cascade^n) for N+chain targets
- VFX: her seksme cyan arc + slow-mo 0.15s freeze (Hades signature finish ama trajectory'e bağlı)
- Class identity bağlanır: Warblade slash duvardan seker (geometrik), Elementalist orb düşman→düşman zıplar (target-based), Ranger trap→trap chain (network), Ronin iaido cut tek hattan 5 düşman dik geçer (penetrasyon), Hexer hex stack 7+ konumdan dalga (zone)
- Skill modifier (Karar #18 Item System D ile uyumlu): "+1 cascade depth" / "+30% cascade damage" / "cascade ignite tile" relic'ler

**Source mechanic bank entries:**
- **M150 Cascading Water Drop** — *shoot + bounce + soak + chain + redirect* — TAM EŞLEŞME
- M81 Domino Effect — *fiziksel zincir tetik*
- M73 Echo Strikers — *önceki hamlenin daha zayıf yankısı*
- M82 Prism Refraction — *enerji yolunda lensler*

**Integration cost:** ~4-5 hafta
- SkillTrajectorySO + 5 trajectory profile (bounce / chain / penetration / zone-wave / domino)
- Per-class signature cascade tasarımı (10 class × 1 signature = 10 cascade)
- VFX trail system (zaten Karar #52 projectile mimarisi LIVE)
- Damage scaling balance (cascade^n diminishing return)
- Animation: cascade sırasında micro-freeze + camera shake (Karar #50 Game Feel uyumlu)

**Risk:**
- Magicraft/Noita cascade hissi çok yakın — *Hades skill identity'sini öldürebilir*
- 10 class × 1 signature cascade = 10 farklı resolve language; pixel art readability stress
- Boss arena cascade trivial burst riski (Karar #82 mob 3-tier zaten Codex review'da bu sorunu vurguladı)

**Scores:**
- Signature strength: **8/10** (CB'nin Cascade USP'sini RIMA'ya getirir AMA RIMA'nın *class identity verb* yapısına işler — CB klonu DEĞİL, ters yön)
- Ship realism: **6/10** (5 trajectory profile + per-class signature + balance complexity; ama Karar #18 + Karar #52 zaten LIVE altyapı)

---

### Candidate 4 — **Tag Sinerji as Visible Build Identity** (M68 + Karar #28 Force-Mature)

**Pitch (one-liner):** "Build 'gizli synergy' değil — **fiziksel olarak görünür**. Q-R-E slot'unda 2 'Bleed' tag varsa sigil pulse + UI 'Crimson Sovereign Triad Active' announce + ROOM içinde **cosmetic environmental change** (zeminde kan damlaları akıyor). Build = arena state."

**Detay:**
- Karar #28 (Tag Sinerji Bonusu) zaten LIVE: 2 aynı tag → pasif bonus. Bu candidate **görünürlük katmanı**:
- 9 family tag (Karar 65 Bleed/Fracture/Echo/Rift/Burn/Frost/Shadow/Beast/Curse) için archetype detection
- 2-tag → Resonance (UI announce + minor cosmetic VFX trail player'da)
- 3-tag → Rift Proc (UI announce + major cosmetic + room ambient color shift)
- 5-tag → "Sovereign" archetype unlock (room itself shifts: zemin damar, hava parçacık, music layer +1)
- Named archetype list ~20 (Crimson Sovereign / Frost Triad / Echo Phantom / Burning Beast / vb.) — Steam achievement bağlanır
- TikTok hook: room aesthetic değişimi = klip-worthy moment

**Source mechanic bank entries:**
- **M68 Build Synergy Detection UI** — TAM EŞLEŞME (RIMA-anchored zaten)
- C1 Symbol-as-Rule — *sembol yerleştirme = fizik kuralı*
- M83 Stamping — *üst üste mühür kompozisyonu*
- M82 Prism Refraction (light arena reshape)

**Integration cost:** ~2-3 hafta
- BuildArchetypeDetector (skill tag set → archetype name lookup)
- ~20 archetype SO + UI announcement (Karar #69 sigil glyph text feedback ile uyumlu)
- Room ambient layer system (Multi-Layer Painter v1 Karar #147 ile EXTRA layer ekleyebilir — `BackgroundLayerData` tint dinamik)
- Music layer: 5-tag triggerlı 1 ek track elementi (Stable Audio pipeline ile)
- Steam achievement bağlama (~3 gün)

**Risk:**
- 9 family tag × archetype kombinasyon balance edge case
- "Generic gibi" görünebilir (Slay the Spire deck archetype klonu) — implementation visual quality important
- Multi-Layer Painter ekstra layer load on existing pipeline

**Scores:**
- Signature strength: **7/10** (RIMA'nın mevcut Karar #28 + Karar #147 + Karar #69 LIVE altyapısını birleştiren; "build = visible arena state" *RIMA-spesifik* iddiaya çıkar)
- Ship realism: **9/10** (mevcut LIVE sistemler üstüne bina; pure C# + SO + Multi-Layer Painter extension)

---

### Candidate 5 — **Anti-Run Memory Distillation** (Wisdom Shield + Memory Currency)

**Pitch (one-liner):** "Ölüm bir distilled aphorism üretir: '7. odada Beat 3 timing'in yenildi → Beat 3 cooldown timing kartı.' Sonraki run aynı sahne **olmadan önce** kart self-trigger; cooldown azalır veya skill bir-kerelik buff alır. Run'lar arasında *bilgelik birikir*."

**Detay:**
- Mechanic bank M93 Memory Distillation + M79 Wisdom Shield + D1 Reset Loop + Knowledge Persistence sentezi
- Death analysis system: ölüm anı kaydı (room, last-skill, last-resource, last-damage-type, time-to-death)
- Run sonunda 1 "Aphorism Card" damıtılır (~12-15 template, doldurulur with run data)
- Aphorism card kalıcı koleksiyona girer (hub'da galeri)
- Sonraki run: aynı koşul (room X + same class + similar build) tespit edilince aphorism otomatik self-trigger → ek skill buff bir-kerelik
- 50 aphorism toplama = Steam codex achievement
- *Steam review hook:* "I died 12 times in the same room and the game wrote me a poem about it"
- Co-op spec: hub'da arkadaşların aphorism'leri görülebilir (asynchronous, optional)

**Source mechanic bank entries:**
- **M93 Memory Distillation** — *run sonu memory essence*
- **M79 Wisdom Shield** — *3 wisdom/aphorism shield*
- D2 Passive Media → Active Intervention — *pasif anıya gir, müdahale et*
- D1 Reset Loop + Knowledge Persistence — *Outer Wilds bilgi taşıma*
- M99 Memory-as-Production Currency — *memory'nin harcama noktası*

**Integration cost:** ~3-4 hafta
- DeathAnalyzer.cs (run kayıt + analiz)
- AphorismCardSO + 12-15 template (lore-tied — Karar #79 Tone Surfaces standard ile uyumlu)
- AphorismGallery hub UI (Karar #25 meta progression Faz 4-5'e ertelenmişti; bu candidate **partial meta** ekler — locked karara çelişmez çünkü "kalıcı hub upgrade" değil "kalıcı aphorism koleksiyon")
- Trigger detection (run state → condition match)
- Localization (Karar #51 — TR/EN aphorism template yazımı)

**Risk:**
- Meta progression LOCKED karara EDGE — collectibles ≠ persistent upgrade, ama interpretation gerek
- Aphorism template balance: çok güçlü = power creep, çok zayıf = ignored
- Tone risk (cozy aforizm RIMA Fractured Epic tone'una hizmet etmek zorunda; ironic değil dramatic)

**Scores:**
- Signature strength: **9/10** (Hades/DC/Spire YAPMADI; "death writes you poetry" *genuinely talked-about* potansiyeli yüksek; Fractured Epic tone ile mükemmel rezonans)
- Ship realism: **7/10** (mimari kompleks değil ama 12-15 aphorism template + lore yazım + tone calibration creative work)

## B2. Ranked Recommendation

### Ranking by signature × ship

| Rank | Candidate | Sig × Ship | Comment |
|---:|---|---:|---|
| **#1** | **C4 Tag Sinerji as Visible Build Identity** | 7×9 = **63** | LIVE altyapı, hızlı entegre, "build = arena state" RIMA-spesifik |
| **#2** | **C2 Echo Imprint Cascade (Death-as-Architect)** | 9×8 = **72** | TAM signature potential, Karar #27 LOCKED altyapısı, "world remembers you" verb |
| **#3** | **C5 Anti-Run Memory Distillation** | 9×7 = **63** | Most poetic; risk: meta progression locked karar edge |
| **#4** | **C3 Cascading Skill Trajectory** | 8×6 = **48** | Class identity güçlendirir ama CB cascade USP'siyle çakışma riski |
| **#5** | **C1 Resource Cross-Channel** | 8×4 = **32** | En derin signature ama 10 class implement öncesi prematüre |

**Top "go signature" — #2 Echo Imprint Cascade (Death-as-Architect).**

Justification:
- **Karar #27 zaten LOCKED** — sıfırdan tasarım değil, mevcut spec'i gerçek bir verb haline getirir
- **TikTok klip potansiyeli en yüksek:** "I died here 12 times and now the room is haunted by my failures"
- **Hades/DC/Spire YAPMADI:** death-as-content rare; Spelunky 2 ghost runs benzer ama tek-frame, RIMA bunu *persistent arena geometry*'ye taşır
- **Solo dev cost reasonable:** 3-4 hafta, mevcut RoomBank + Map Fragment + Echo Imprint altyapısı + persistent save
- **RIMA imzasını güçlendirir:** 10-class × 10 resource ekonomi imzasıyla çakışmaz, üzerine bina; her sınıf farklı death imprint signature (Warblade: kan dalga, Elementalist: frost shard, Hexer: hex stack residue)
- **Pitch'e dönüşür:** "Die. The room remembers. Die again. The room learns."

**Yedek go signature — #4 Tag Sinerji Build Identity** (eğer Death-as-Architect implementation riski yüksek bulunursa). 2-3 hafta, LIVE altyapı, daha düşük tasarım risk.

## B3. Mechanic bank entry traceability

| Candidate | Birincil mechanic bank entry | Destekleyici |
|---|---|---|
| **C1 Resource Cross-Channel** | M68 Build Synergy Detection (RIMA) + M75 Gossip Relay Buff (Composition) | M70 Baton Pass · D1 Reset Loop |
| **C2 Echo Imprint Cascade** | **D4 Persistent Multi-Gen Tile + C2 Trail-as-Functional + M67 Death Echo** | D2 Passive Media · M93 Memory Distillation |
| **C3 Cascading Skill Trajectory** | **M150 Cascading Water Drop (Cascading Trajectory family)** | M81 Domino · M73 Echo Strikers · M82 Prism |
| **C4 Tag Sinerji Visible Build** | **M68 Build Synergy Detection (RIMA)** | C1 Symbol-as-Rule · M83 Stamping · M82 Prism |
| **C5 Anti-Run Memory Distillation** | **M93 Memory Distillation + M79 Wisdom Shield** | D1 Reset Loop · D2 Passive Media · M99 Memory-as-Currency |

---

# PART C — COMBINED RECOMMENDATION

## Single concrete morning action item

**RIMA continue + Echo Imprint Cascade (Death-as-Architect) be designated as RIMA's epic signature mechanic + 1 hafta pitch sprint + 5-7 gün Ronin stress test devam et.**

### Sequence (morning dispatch için öneri sıra)

1. **Sabah ilk iş — Ronin stress test (5-7 gün, önceki Opus verdict).** Bu zaten queued. CB pivot kararını bilgilendirecek. PASS olursa imza kanıtlanır, CB pivot mantığı çürür. FAIL/MIXED ise epic mechanic eklendiğinde imza güçlenebilir.

2. **Stress test PARALEL — 1 hafta pitch sprint.** RIMA için tek cümle pitch yaz. Candidate: *"Die. The room remembers. Each death writes the arena. The next class learns from your last fall."* (Echo Imprint signature + 10-class hint). Tek cümle olmazsa 2-3 alternatif kıyas. Bu CB pitch netliği avantajını siler.

3. **Echo Imprint Cascade design dispatch (rima-design + Codex review).** Karar #27 LOCKED spec'ini "Death-as-Architect" verb'üne genişlet. 3-4 hafta implementation roadmap çıkar.

4. **CB klasörüne not düş:** Bu verdict CB klasörüne *bilgi* olarak gitsin (`F:\LaurethStudio\02_GAMES\CircuitBreaker\STAGING\rima_pivot_evaluation_2026-05-19.md`). CB future-self pickup için: "2026-05-19 RIMA pivot evaluation: continue RIMA + add Echo Imprint, CB Sezon 2'ye ertelenmiş."

### Pivot rule going forward

Eğer Ronin stress test PASS + 1 hafta pitch sprint pitch_clarity > 80% (self-report) ise **CB pivot DEFER (Sezon 2)**. Eğer Ronin FAIL VE pitch sprint pitch_clarity < 50% ise **CB pivot ciddi düşünülmeli** (5-gate full path).

### Conflicts with locked rules

NONE.
- Karar #143 (6-layer painted) + Karar #147 (Multi-Layer Painter): KEEP, Candidate 4 (Tag Sinerji) bunlardan ek layer kullanabilir
- Karar #27 (Echo Imprint): KEEP, Candidate 2 bu spec'i derinleştirir
- Karar #25 (Meta progression Faz 4-5): KEEP, Candidate 5 EDGE — "collectibles ≠ persistent upgrade" yorumu gerekirse re-evaluation
- Karar #80 (Class Silhouette Bible) + 10-char anchor: KEEP, hiçbir candidate bunları değiştirmiyor
- Master Karar Belgesi 17 sistem: hiçbiri çelişmiyor, Echo Imprint mevcut #27'yi genişletiyor

---

## NOTES TO MORNING REVIEW

- Bu verdict acımasız mod. CB pivot çekici görünme sebebi *pitch* — RIMA'ya pitch ekle, CB cazibesi erir.
- 5 candidate'ten **Death-as-Architect (Echo Imprint Cascade)** *gerçek* signature. Klon değil. Solo dev shippable. RIMA imzasını güçlendirir.
- Eğer "CB pivot zaten karar verilmiş" feel'i hâkimse: kararı _geri çevir_, en kötü Sezon 2'ye ertele. Pivot decision document `05_PIVOT_DECISION_2026-05-18.md` zaten **Hybrid (Option C) — 5 gate ile koşullu** — "RIMA-lite'a dön (4-6 hafta kayıp kabul)" yolu açık.
- Tüm verdict honest. Flattery yok. Sunk cost trap için ayrı disiplinle değerlendirildi.

**Conflicts with locked rules:** NONE.
**Orchestrator next step:** Morning user review → karar (Echo Imprint dispatch + pitch sprint + Ronin test devam) → eğer onay rima-doc'a Death-as-Architect spec expansion task + Codex'e prototype task delege edilir.
