# RIMA -- FAZ MASTER v2 (Detayli Scope Dokumani)

Son guncelleme: 2026-05-09
Kaynak: MASTER_KARAR_BELGESI.md, FAZ_MASTER.md, GUIDES/RIMA_MASTER_ART_PIPELINE.md,
        TASARIM/dungeon_act1_map.md, TASARIM/map_fragment_system.md,
        TASARIM/CINEMATIC_LAYER_v1.md, TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md
        Kararlar #60-#71

> Bu dosya: Faz basi calistirilebilir blueprint. Faz 1 en detayli -- sonrakiler icin sablonu kurar.
> Ozet harita icin: FAZ_MASTER.md
> Bireysel faz detayi icin: FAZ1_CORE_LOOP.md ... FAZ5_FULL_GAME.md

---

# FAZ 1 -- WARBLADE (Demo Sinifi)

## 1. Faz Hedefi

Warblade ile Act 1'in ilk 8-9 odasini oynanabilir hale getirmek; combat hissi, temel dungeon loop
ve map fragment sistemi calisir durumda olmak.

## 2. Class Scope

**Sadece Warblade bu fazda uretilir.**

| Tier | Hafta | Kapsam |
|------|-------|--------|
| P0 | Hf 1-2 | Karakter silue, idle/run/attack 4-yon, normal death |
| P1 | Hf 3-4 | 12 Skill VFX (Common havuz), Hit/hurt flash, HUD ikonlari |
| P2 | Hf 5-6 | Polish: Rage VFX, hit-stop frame, screen shake katmani |

Warblade skill havuzu: SINIF_VE_SKILL_KARAR_BELGESI.md + SKILL_POOLS_10CLASS_2026-05-07.md
Animasyon spesifikasyonu: Karar #46-48 -- Run 6f, 4 yon, Death/Hit 4 yon

## 3. System Scope

Bu fazda calisir hale gelmesi gereken sistemler:

| Sistem | Aciklama | Kaynak |
|--------|----------|--------|
| Rage (V Meter) | Warblade-spesifik dolum kosulu | Karar #17 |
| Sundered State | Warblade uretir, Brawler tuketir -- Faz 3'e hazirlik | Karar #55 |
| BasicAttackBehavior | LMB/RMB 4-yon, combo zinciri | BASIC_ATTACK_LMB_RMB_8CLASS_CONTRACT |
| ActionCommitProfile | Startup/active/recovery frame data, dash-cancel kurali | Karar #64 + #67 |
| 3-Layer Feedback | Hit impact (screen shake + freeze + flash) | Karar #65 |
| Posture System v1 | Posture bar, guard break, staggers | Karar #66 |
| Status Effect System | Bleed, Burn, Chill, Stagger temel set | FAZ_MASTER.md |
| HUD 6-slot | 4 aktif + 2 kilitli; Warblade rage gostergesi | HUD_DESIGN_SPEC.md |
| Skill Draft (Common) | 3 kart sunusu, combat odasi sonu | SKILL_OFFER_SYSTEM_DECISION |
| Elite Affix (4 tip) | Armored/Shielded/Berserker/Mage prefix | Karar #32 |
| Mob AI (7 mob) | ShardWalker, VoidThrall, Penitent, ChainWarden, FractureImp, SeamCrawler, HollowMite | FAZ_MASTER.md |
| Dungeon Graph (8-9 oda) | RoomLoader + node baglantisi + Act 1 haritasi (15 node) | dungeon_act1_map.md |
| Map Fragment System | Kirik Tas Tablet -- kismi goruntu | map_fragment_system.md |
| Olum + Restart | GameOver ekrani, run istatistigi | FAZ_MASTER.md |
| Boss Faz 1: Penitent Sovereign | Sadece F1 (HP %66 kesimi yok -- Faz 2) | FAZ_MASTER.md |

**Bu fazda OLMAYAN sistemler:** Shop, Secondary class, Cross-class, Echoes, meta-progression.

## 4. Asset Production

### 4a. Karakter (PixelLab)

Warblade tam production:
- P0: ~30 gen call (~225-300 kredit) -- silue + idle + run + attack iskelet
- P1: ~50 gen call (~375-500 kredit) -- 12 skill VFX + ikonlar
- P2: ~20 gen call (~150-200 kredit) -- polish iterasyonlari
Toplam Warblade: ~100 call / ~750-1000 kredit

Pipeline detayi: GUIDES/RIMA_MASTER_ART_PIPELINE.md

### 4b. Mob (7 mob, Act 1 seti)

| Mob | Boyut | Animasyon | Durum |
|-----|-------|-----------|-------|
| ShardWalker | 128px | idle/run/attack/death (4-yon) | Prefab hazir -- S43 |
| VoidThrall | 128px | idle/run/attack/death (4-yon) | Prefab hazir -- S43 |
| Penitent | 128px | idle/run/attack/death (4-yon) | Prefab hazir -- S43 |
| ChainWarden | 128px | idle/run/attack/death (4-yon) | Prefab hazir -- S43 |
| FractureImp | 128px | idle/run/attack/death (4-yon) | Prefab hazir -- S43 |
| SeamCrawler | 256x128px | idle/run/attack/death | Prefab hazir -- S43 |
| HollowMite | 48px | idle/run/swarm | Uretilecek -- P0 |

Yeni uretim: sadece HollowMite (~15 gen call, ~110 kredit)

### 4c. Cevre (Tile Setleri)

Act 1 -- Shattered Keep:
- F1 tile seti: 16 tile (f1_00..f1_15) -- GIT STATUS'ta guncellendi, hazir
- F2 tile seti: Meta guncellendi -- gorsel hazir
- Yeni ihtiyac: Boss odasi floor variant (F1B) -- ~10 gen call

### 4d. Cinematic Frames

Faz 1'de cinematic yok. CINEMATIC_LAYER_v1.md katmanlari Faz 2'den baslayacak.

### 4e. UI Assets

| Asset | Durum |
|-------|-------|
| HUD 6-slot cerceve | Uretilecek -- RIMA_UI_HUD_PRODUCTION_COMPOSITE |
| Rage/V-meter bar | Uretilecek |
| Map Fragment overlay | Uretilecek -- map_fragment_system.md icindeki sprite tanimi |
| Skill kart UI (Common) | Uretilecek |
| GameOver ekrani | Uretilecek |

## 5. Code Scope

| Sistem | Script | Oncelik |
|--------|--------|---------|
| Player controller | PlayerController.cs, InputHandler.cs | P0 |
| Basic attack | BasicAttackBehavior.cs (LMB/RMB/4-yon) | P0 |
| Rage/V-Meter | RageMeter.cs, WarbladeResource.cs | P0 |
| ActionCommit | ActionCommitProfile.cs, DashCancelLogic.cs | P0 |
| Status effect | StatusEffectManager.cs (4 temel efekt) | P0 |
| Mob AI | MobBehaviorBase.cs + 7 konkret class | P0 |
| Dungeon gen | DungeonGraph.cs, RoomLoader.cs | P0 |
| Map fragment | MapFragmentManager.cs, FragmentReveal.cs | P0 |
| Skill system | SkillDataSO.cs, SkillDraftUI.cs, SkillSlotManager.cs | P1 |
| HUD | HUDController.cs, SlotDisplay.cs | P1 |
| Boss AI | PenitentSovereign.cs (F1 fazi) | P1 |
| Elite affix | EliteAffixSystem.cs (4 tip) | P1 |
| 3-layer feedback | HitFeedback.cs (freeze + shake + flash) | P1 |
| Posture | PostureBar.cs, GuardBreakEvent.cs | P1 |
| Death/restart | GameOverScreen.cs, RunStats.cs | P2 |

**Hedef test sayisi:** 40+ edit-mode unit test (ActionCommit frame data + status effect + map fragment)

## 6. Test / Verification

- [ ] Warblade: LMB/RMB combo zinciri 4 yonde calisir
- [ ] Rage dolumu: belgelenmiş kosulda (3 LMB hit) tetiklenir
- [ ] Sundered State: uretilir, expire eder
- [ ] 7 mob: spawn, patrol, attack, death animasyonu tamam
- [ ] Dungeon: 8-9 oda generate edilir, boss odasina erisilebilir
- [ ] Map Fragment: 3 farkli fragment varianti gorsel olarak acilir
- [ ] Penitent Sovereign: F1 fazinda oldurulebilir
- [ ] Skill draft: combat sonrasi 3 kart sunulur, secilebilir
- [ ] HUD: 4 skill slotu dogru gosterilir, Rage bar dolar/boslalir
- [ ] GameOver: olum ekrani gosterilir, yeniden baslama calisir

**Milestone testi:** 10 dakikalik loop -- Act 1 bas-bos kesintisiz.

## 7. Sure Tahmini

| Hafta | Odak |
|-------|------|
| Hf 1 | PlayerController + BasicAttack + Mob AI (3 grunt) |
| Hf 2 | Dungeon gen + RoomLoader + 4 kalan mob |
| Hf 3 | Rage/V-Meter + StatusEffect + Map Fragment |
| Hf 4 | Skill system + HUD + Draft UI |
| Hf 5 | Boss (Penitent Sovereign F1) + Elite Affix |
| Hf 6 | 3-layer feedback + Posture + GameOver |
| Hf 7 | Warblade P1 asset integration (VFX + animasyon) |
| Hf 8 | Test + polish + milestone loop |

**Toplam: 6-8 hafta** (hedef 6 -- buffer 2)

## 8. Bagimlilıklar

Faz 1 ilk faz -- dissal bagimlilık yok.

Teknik gereksinim: Unity LTS (2022.3+), PixelLab Tier 2 abonelik aktif.

## 9. Risk

| Risk | Olasilik | Azaltma |
|------|---------|---------|
| Dungeon gen karmasikligi (DungeonGraph) | Yuksek | Act 1 haritasi LOCKED 15 node -- Manuel fallback: sabit oda siralama |
| Warblade feel: combat hissi tatmin etmez | Orta | 3-layer feedback + Posture v1 erken entegre et |
| Map Fragment sprite scope creep | Dusuk | Fragment sistemi duz overlay -- 3 variant kilitle |
| PixelLab kredit asimi | Orta | Warblade uretimi ~1000 kredit max; Tier 2 = 2000-3000/ay |

## 10. Cikti (Deliverable)

Warblade ile Act 1 bas-bos oynanabilir loop: combat, skill draft, map fragment reveal,
Penitent Sovereign F1 kill. 10 dakika kesintisiz demo.

---

# FAZ 2 -- 4 SINIF (+Elementalist, Shadowblade, Ranger)

## 1. Faz Hedefi

4 class ile Act 1 tam oynanabili; Penitent Sovereign tam (F1+F2 faz gecisi); Shop, Unknown oda,
Echoes para birimi ve Rare skill tier aktif.

## 2. Class Scope

4 sinif bu fazda uretilir. Warblade polish devam eder.

| Sinif | P0 (Hafta) | P1 (Hafta) | P2 (Hafta) | Kredit Tahmini |
|-------|-----------|-----------|-----------|----------------|
| Elementalist | Hf 1-2 | Hf 3 | Hf 5 | ~800-1000 |
| Shadowblade | Hf 2-3 | Hf 4 | Hf 5 | ~800-1000 |
| Ranger | Hf 3-4 | Hf 5 | Hf 6 | ~800-1000 |
| Warblade polish | -- | Hf 1 | -- | ~150 |

Pipeline: Her sinif icin RIMA_MASTER_ART_PIPELINE.md akisi.
Toplam Faz 2 karakter: ~2550-3150 kredit (~3500 kredit buffer)

Echo Imprint (Faz 2 baslangici, 4 sinif, temel set): Skill taxonomy SKILL_SYSTEM_TAXONOMY_2026-05-06.md

## 3. System Scope

| Sistem | Aciklama |
|--------|----------|
| ClassData + ClassManager + ClassSelectUI | 4 class secim, resource yonetimi |
| Mana / Energy / Focus / CP kaynaklar | Her class icin ayri kaynak | 
| ShopManager + GoldManager | Shop odasi ekonomisi |
| Shards (in-run currency) | Para birimi altyapisi |
| Rare skill tier | Draft havuzuna Rare karti ekle |
| Sandik sistemi (3 tip) | Normal/Elite/Boss sandik |
| Reroll sistemi (1 ucretsiz) | Draft UI'da reroll butonu |
| Echo Imprint (4 class, temel) | Her 3 combat odasinda 1 Imprint sunusu |
| SeamCrawler + Twice-Born mob | Faz 2 mob seti +2 |
| Boss: Penitent Sovereign tam | F1+F2 faz gecisi (%66/%33) |
| Unknown oda | 3 varyant (event-lite) |
| Cinematic Layer A (Karar #64) | Cutscene alt yapisi, 4 katman -- sadece Faz 2'de kurulur |

## 4. Asset Production

### 4a. Karakter
3 yeni sinif x ~100 call = ~300 gen call (~2400-3000 kredit)

### 4b. Mob
+2 mob (SeamCrawler animasyon tamamlama + Twice-Born yeni uretim)
Twice-Born: 160px, Elite tier, ~25 gen call (~190 kredit)

### 4c. Cevre
Act 1 tam tile seti: F1+F2 (mevcut) + F3 aralik baslangiç (~20 gen call)
Shop odasi ortam varliklari (~15 gen call)

### 4d. Cinematic
Layer A altyapisi: 2-3 kare baslangic (run giris + boss kapi acilis)
~10 gen call cinematic frame

### 4e. UI
Shop UI, Class Select screen, Echo Imprint overlay, Rare skill kart cercevesi

## 5. Code Scope

| Sistem | Script |
|--------|--------|
| ClassManager | ClassManager.cs, ClassDataSO.cs |
| Kaynak sistemleri | ManaResource.cs, EnergyResource.cs, FocusResource.cs, CPResource.cs |
| ShopManager | ShopManager.cs, ShopItemSO.cs |
| Echoes currency | EchoesManager.cs |
| Rare skill draft | SkillTierFilter.cs (Rare ekleme) |
| Sandik | ChestInteractable.cs, ChestRewardTable.cs |
| Reroll | DraftRerollButton.cs |
| Echo Imprint | EchoImprintManager.cs, ImprintOfferUI.cs |
| Unknown oda | UnknownRoomController.cs |
| Penitent Sovereign F2 | BossPhaseTransition.cs (F2 ekleme) |
| Cinematic altyapi | CinematicDirector.cs, CutsceneLayer.cs |

## 6. Test / Verification

- [ ] 4 class secilip oynanabilir (kaynak bari dogru gosterilir)
- [ ] Shop: alim, gold kesme, refresh calisir
- [ ] Rare kart: draft havuzunda %20 oraninda cikar
- [ ] Echo Imprint: her 3 combat sonrasi teklif gelir, uygulanir
- [ ] Penitent Sovereign: F2'de gecis animasyonu + yeni saldiri seti
- [ ] Sandik: 3 tip farkli reward tablosu

**Milestone:** 4 class ile Act 1 tam loop, her sinif distinct feel.

## 7. Sure Tahmini

**6-8 hafta** (3 sinif paralel asset + sistem genislemesi)

Not: Asset uretimi en uzun -- Elementalist, Shadowblade, Ranger ardisik degil kismi paralel.

## 8. Bagimlilıklar

- Faz 1 tamamen bitti (DungeonGraph, RoomLoader, Skill system stabil)
- PixelLab Tier 2 aktif (~3500 kredit bütçe)

## 9. Risk

| Risk | Azaltma |
|------|---------|
| 3 sinif asset paralel bütçe asimi | Elementalist önce; diger ikisi siraya al |
| Cinematic Layer altyapisi buyur | Sadece Layer A kur; B/C/D Faz 3-4'e birak |
| Echo Imprint dengesizligi | Sadece 4 class imprint; cross-class Faz 3'e |

## 10. Cikti (Deliverable)

4 class ile tam Act 1 run, Shop entegre, Rare draft, Penitent Sovereign tam (2 faz).

---

# FAZ 3 -- 8 SINIF (Cross-Class + Secondary Unlock)

## 1. Faz Hedefi

8 class ile Act 1 + Act 2 baslangici oynanabilir; secondary class secilebilir;
28 cross-class pasif kombinasyonu aktif; Epic skill tier; 4 yeni mob.

## 2. Class Scope

4 yeni sinif: Ravager, Ronin, Gunslinger, Brawler.

| Sinif | P0 | P1 | P2 | Kredit |
|-------|----|----|----|----|
| Ravager | Hf 1-2 | Hf 3 | Hf 7 | ~850-1000 |
| Ronin | Hf 2-3 | Hf 4 | Hf 7 | ~850-1000 |
| Gunslinger | Hf 3-4 | Hf 5 | Hf 8 | ~850-1000 |
| Brawler | Hf 4-5 | Hf 6 | Hf 8 | ~850-1000 |

Kaynaklar: DrawTension (Ronin), Heat (Gunslinger), Charge 0-5 (Brawler) -- Karar #57
Cross-class proc text: CROSS_CLASS_PROC_SYSTEM.md + Karar #69

Toplam Faz 3 karakter: ~3400-4000 kredit

## 3. System Scope

| Sistem | Aciklama |
|--------|----------|
| Secondary class secimi | Act 1 boss kill sonrasi unlock |
| +2 aktif slot acilisi | Secondary secimle 6-slot tam aktif |
| Cross-class pasif (28 kombo) | 8x7/2 kombinasyon tablosu -- CROSS_CLASS_PROC_SYSTEM |
| Mixed draft oranlari | Primary/Secondary havuzu karistir |
| Spirit Encounter (3 tip) | Yeni oda tipi |
| Epic skill tier | Draft havuzuna Epic ekleme |
| Echo Imprint (8 class) | Tam 8-class imprint seti |
| Tag Sinerji Bonusu | 2 ayni tag -> otomatik pasif bonus |
| Echo Hound + FractureBorn mob | +2 mob Act 2 icin |
| Boss: Echo Twin | Faz 3 yeni boss, %40 gecis |
| Act 2 baslangici (5-6 oda) | Ikinci akt ilk odalari |
| Hub tasarimi (iskelet) | Faz 4'te detaylanacak |
| OnDash proc sistemi | Karar #68 |

## 4. Asset Production

### 4a. Karakter
4 yeni sinif x ~100 call = ~400 gen call (~3200-4000 kredit)

### 4b. Mob
+4 mob:
- EchoHound: 96px, Grunt (~20 gen call)
- FractureBorn: 160px, Elite (~25 gen call)
- Twice-Born varyant (Act 2 renk) (~10 gen call)
- ClassMimic iskelet (~20 gen call)

### 4c. Cevre
Act 2 tile seti ilk seti (W1/W2 veya yeni palet -- STYLE_BIBLE.md)
Spirit Encounter odasi ortam (~20 gen call)
Act 1-2 gecis koridor varliklari

### 4d. Cinematic
Layer B + C iskelet: Secondary class secim cutscene (2-3 kare)
Echo Twin reveal (1-2 kare)
~15 gen call

### 4e. UI
Secondary class secim ekrani, 6-slot tam aktif HUD, Spirit Encounter UI, Epic kart cercevesi

## 5. Code Scope

| Sistem | Script |
|--------|--------|
| Secondary class | SecondaryClassManager.cs, ClassUnlockTrigger.cs |
| SlotExpander | HUDSlotExpander.cs (4->6) |
| CrossClassPassive | CrossClassPassiveTable.cs, ComboResolver.cs |
| MixedDraft | MixedDraftPool.cs |
| Spirit Encounter | SpiritEncounterController.cs (3 tip) |
| Epic tier | SkillTierFilter.cs (Epic ekleme) |
| Tag Sinerji | TagSynergyEvaluator.cs |
| OnDash proc | DashProcHandler.cs |
| Echo Twin AI | EchoTwin.cs |
| Act 2 loader | Act2RoomLoader.cs |

## 6. Test / Verification

- [ ] Secondary class secilebilir, cross-class pasif aktif
- [ ] 28 kombinasyondan 10 rastgele test edildi
- [ ] 6 slot: ikisi secondary skilllerle dolabilir
- [ ] Epic kart: draft havuzunda cikabilir, efekti calisir
- [ ] Spirit Encounter: 3 tip teklif edilebilir
- [ ] Echo Twin: faz gecisi + distinct saldiri
- [ ] Tag Sinerji: 2 ayni tag bonusu gorsel feedback gosterir

**Milestone:** 8 class x secondary = 56 kombinasyon iskelet; Spirit Encounter oynanabilir.

## 7. Sure Tahmini

**8-10 hafta** (4 sinif asset + en karmasik sistem genislemesi)

## 8. Bagimlilıklar

- Faz 2 tamam: ClassManager, Echoes, Draft havuzu stabil
- Act 1 boss kill endpoint temiz

## 9. Risk

| Risk | Azaltma |
|------|---------|
| 28 cross-class pasif denge | Sayisal cap koy (3 pasif max actif), QC sprint ayir |
| 4 sinif ayni faz, kredit asimi | Ravager+Ronin once; Gun+Brawler ikinci dalga |
| Act 2 tile set scope | W1 tile seti ile baslat, expand Faz 4'te |

## 10. Cikti (Deliverable)

8 class ile secondary class secimli run; Act 1 tam + Act 2 basi; 28 cross-class pasif.

---

# FAZ 4 -- 10 SINIF TAMAMLANDI (Demo-Ready)

## 1. Faz Hedefi

Summoner + Hexer ile 10 class tam set; Act 1-2 tam; Cross-class Ultimate; meta-progression
iskelet; Fracture Echoes ilk 2 boss; demo-ready standalone run.

## 2. Class Scope

| Sinif | Uretim | Not |
|-------|--------|-----|
| Summoner | P0->P2 bu fazda | Summoner ekonomisi: SUMMONER_ECONOMY_RULES.md |
| Hexer | P0->P2 bu fazda | Summoner+Hexer tasarimi: SUMMONER_HEXER_CLASS_DESIGN.md |

Toplam: ~200 gen call (~1600-2000 kredit)

## 3. System Scope

| Sistem | Aciklama |
|--------|----------|
| Cross-class Ultimate | Secondary class ile unlock, Shift+key toggle | Karar #54 |
| Curse sistemi (5 efekt) | Curse Gate oda tipi |
| Event odasi (10 event) | Faz 4 oda genisleme |
| Temel meta-progression | Echoes harcan, kalici unlock |
| Fracture Echoes (2 boss) | Boss varyasyon -- 5 echo per boss | MASTER_KARAR_BELGESI |
| Hub NPC iskelet | Merchant, Oracle, Blacksmith stump |
| Act 2 tam | 9-11 oda tam |
| Fracture Sovereign boss | F1-F3 faz gecisi (%60/%30) |

## 4. Asset Production

- 2 sinif: ~200 gen call (~1600-2000 kredit)
- Fracture Echo varyant sprite: ~20 gen call (boss 2x renk palette)
- Act 2 tam tile seti tamamlama
- Curse Gate oda UI + VFX
- Event oda UI
- Hub ortam varliklari (mimari iskelet)

## 5. Code Scope

Summoner.cs, HexerResource.cs, CrossClassUltimate.cs, CurseSystem.cs, EventRoomController.cs,
MetaProgressionManager.cs, EchoSpendUI.cs, FractureEchoVariant.cs, HubSceneController.cs,
FractureSovereign.cs

## 6. Test / Verification

- [ ] 10 class tam oynanabilir
- [ ] Cross-class Ultimate: 45 kombinasyon iskelet (10x9/2)
- [ ] Fracture Echoes: 2 boss farkli echo varyantlari
- [ ] Meta-progression: run sonu Echoes harcama UI calisir
- [ ] Act 2 tam loop: bas-bos kesintisiz

**Milestone:** Standalone demo -- basin ve playtest icin dagitilabilir.

## 7. Sure Tahmini

**6-8 hafta**

## 8. Bagimlilıklar

- Faz 3 tamam: 8 class, cross-class pasif, Act 2 basi

## 9. Risk

Summoner/Hexer tasarim karmasikligi (ekonomi farkli) -- onceden SUMMONER_ECONOMY_RULES.md gozden gecir.
45 cross-class Ultimate denge -- sayisal test gerekli.

## 10. Cikti (Deliverable)

10 class + meta-progression + Act 1-2 tam. Press-ready demo build.

---

# FAZ 5 -- TAM OYUN (Early Access)

## 1. Faz Hedefi

Act 1-2-3 tam; Final Boss (The Architect); 45 cross-class kombo (10x9/2);
Legendary skill tier; Grudge/Nemesis; zorluk modlari; Early Access launch.

## 2. Class Scope

Tum 10 sinif polish + balans. Yeni class yok -- derinlik ve denge odakli.

## 3. System Scope

| Sistem |
|--------|
| Legendary skill tier |
| Grudge / Nemesis sistemi |
| Class unlock sistemi (meta-progression kilidi) |
| Tam meta-progression + Hub NPC'ler (konusma + efekt) |
| 45 cross-class Ultimate tam set |
| Fracture Echoes tum bosslar |
| Zorluk modu: Echo/Rift/Fracture/Void |
| Act 3 (9-11 oda) |
| The Architect boss (4 faz: %75/%45/%20) |
| Makeup Backlog (MAKEUP_BACKLOG.md, HIGH oncelikler) |

## 4. Asset Production

- Act 3 tile seti (yeni palet -- 3. akt mimari)
- The Architect boss: ~50 gen call (~400 kredit)
- Fracture Echo varyantlar tum bosslar: ~40 gen call
- Zorluk modu UI
- Hub NPC tam sprite set
- Legendary skill kart cercevesi
- Cinematic Layer D (Final Boss cutscene -- 4-5 kare)

## 5. Code Scope

Act3RoomLoader.cs, LegendarySkillTier.cs, GrudgeSystem.cs, ClassUnlockProgression.cs,
DifficultyModeManager.cs, TheArchitect.cs, FractureEchoAll.cs, MakeupBacklogFixes.cs

## 6. Test / Verification

- [ ] Act 1-2-3 tam loop kesintisiz
- [ ] The Architect 4 fazda oldurulebildi
- [ ] Legendary kart oyun dengesini bozmadi
- [ ] Zorluk mod 2 (Rift) oynanabilir
- [ ] Grudge: ayni boss 3 kez oldururken tracker degisiyor
- [ ] Hub NPC konusmalar calisir

**Milestone:** Early Access launch build.

## 7. Sure Tahmini

**8-12 hafta** (polish + QC agir; Act 3 yeni content)

## 8. Bagimlilıklar

- Faz 4 demo build stabil
- Makeup Backlog HIGH oncelikleri Faz 4 sonunda teslim edilmis olmali

## 9. Risk

Act 3 scope creep -- oda sayisini sabit tut (9-11). Polish > yeni sistem.

## 10. Cikti (Deliverable)

Early Access build: Act 1-2-3, 10 class, Final Boss, zorluk modlari.

---

# POST-LAUNCH -- TEMPEST + HEMOMANCER (DLC)

## 1. Kapsam Ozeti

2 ek sinif; her biri ayri DLC olarak yayinlanabilir veya bundle.
Karar: Post-launch -- Faz 5 sonrasi. Faz 5'te hicbir Tempest/Hemomancer altyapisi eklenmez.

## 2. Class Scope

| Sinif | Tasarim Durumu | Uretim |
|-------|---------------|--------|
| Tempest | Taslak yok -- Faz 5 sonrasi tanimlanacak | -- |
| Hemomancer | Taslak yok -- Faz 5 sonrasi tanimlanacak | -- |

## 3. Asset Production

Her DLC sinifi: ~100-120 gen call (~800-1100 kredit).

## 4. System Scope

- Cross-class pasif: +9+8 yeni kombo (Tempest ve Hemomancer icin)
- Ultimate: +2 yeni cross-class Ultimate
- Olasi yeni kaynak mekaniki her sinif icin

## 5. Sure Tahmini

4-6 hafta / DLC sinif (asset + kod + denge)

---

# TOPLAM ROADMAP TABLOSU

| Faz | Sure | Kumulatif | Sinif Sayisi | Ana Cikti |
|-----|------|-----------|-------------|-----------|
| 1 (Warblade) | 6-8 hf | 0-2 ay | 1 | Act 1 demo, Penitent Sovereign F1 |
| 2 (+Elem/Shadow/Ranger) | 6-8 hf | 2-4 ay | 4 | Act 1 tam, Shop, Rare tier |
| 3 (+4 sinif + Cross) | 8-10 hf | 4-6.5 ay | 8 | Secondary class, 28 kombo, Act 2 |
| 4 (+Summoner/Hexer) | 6-8 hf | 6.5-9 ay | 10 | Demo-ready, meta-prog, 45 kombo |
| 5 (Early Access) | 8-12 hf | 9-12 ay | 10 | Act 1-2-3, Final Boss, launch |
| Post-Launch DLC | 4-6 hf/sinif | 12 ay+ | +2 | Tempest + Hemomancer |

**Toplam Early Access hedefi: ~12 ay solo dev (Yasin, Antigravity)**

---

# KRITIK PATIKA (Kaynak Dagilimi)

| Kategori | Yuzde | Notlar |
|----------|-------|--------|
| Code (C# sistemler) | ~%40 | Faz 1-3 agir; Faz 4-5'te azalir |
| Art (PixelLab gen) | ~%35 | 10 sinif x ~1000 kredit; her faza dagilir |
| Design + Doc | ~%15 | TASARIM/ dokumanlari + karar kayitlari |
| QC + Polish | ~%10 | Faz 4-5'te agir; her fazin sonunda sprint |

**Toplam asset butce tahmini:** ~10.000-12.000 kredit (10 sinif tam production)
Tier 2: 2000-3000 kredit/ay -> ~4-5 ay kesintisiz asset uretimi (diger icerikler haric)

---

# OPSIYONEL ASYNC ISLER

Asagidaki isler main kritik patikada degil -- paralel yuruyebilir:

| Is | Fazlar | Sorumlu |
|----|--------|---------|
| Cinematic Layer A/B | Faz 2-3 | PixelLab + kod (CinematicDirector) |
| Cinematic Layer C/D | Faz 4-5 | PixelLab + kod |
| Hub NPC diyalog tasarimi | Faz 3-4 | Design (rima-design agent) |
| Lore drip pipeline | Faz 2-5 boyunca | TASARIM dokumanlari |
| Makeup Backlog (MEDIUM/LOW) | Faz 5+ | MAKEUP_BACKLOG.md |
| Mob armor variant gorselleri | Faz 2-3 | PixelLab (Karar #32) |
| Fracture Echo varyant spriteler | Faz 3-4 | PixelLab |
| Grudge/Nemesis sistem tasarimi | Faz 4 paralel | Design |

---

# SISTEM-FAZ MATRISI (Hizli Referans)

| Sistem | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 |
|--------|-------|-------|-------|-------|-------|
| Rage/V-Meter | IMPL | -- | -- | -- | -- |
| ActionCommitProfile | IMPL | -- | -- | -- | -- |
| 3-Layer Feedback | IMPL | -- | -- | -- | -- |
| Posture v1 | IMPL | -- | -- | -- | -- |
| Dungeon Graph (Act 1) | IMPL | -- | -- | -- | -- |
| Map Fragment System | IMPL | -- | -- | -- | -- |
| Skill Draft (Common) | IMPL | RARE | EPIC | -- | LEGEND |
| HUD 6-slot | 4-slot | -- | 6-slot | -- | -- |
| Echo Imprint | -- | 4cls | 8cls | 10cls | -- |
| ClassManager | -- | IMPL | -- | -- | -- |
| Shop + Gold | -- | IMPL | -- | -- | -- |
| Secondary Class | -- | -- | IMPL | -- | -- |
| Cross-class Pasif | -- | -- | IMPL (28) | -- | FULL (45) |
| Cross-class Ultimate | -- | -- | -- | IMPL | FULL |
| Curse Sistemi | -- | -- | -- | IMPL | -- |
| Meta-Progression | -- | -- | -- | IMPL | FULL |
| Fracture Echoes | -- | -- | -- | 2 boss | TUM |
| Zorluk Modlari | -- | -- | -- | -- | IMPL |

---

*Son guncelleme: 2026-05-09. Sonraki revizyon: Faz 1 Hf 4 (milestone loop sonrasi).*
*Kaynak dosyalar: MASTER_KARAR_BELGESI.md, FAZ_MASTER.md, GUIDES/RIMA_MASTER_ART_PIPELINE.md*
