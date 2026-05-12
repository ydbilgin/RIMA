---
status: REFERENCE
faz: 1
tarih: 2026-05-14
ozet: "RIMA referans dokumani"
---
# RIMA — FAZ MASTER (Tüm Fazların Özet Haritası)

*Son güncelleme: 2026-05-13 | S60 | Kaynak: GDD, SINIF_VE_SKILL, MOB_TASARIMI, BOSS_DESIGN, MEKANIK_KARARLARI, MASTER_KARAR_BELGESI.md (Karar #1-80), dungeon_act1_map.md, map_fragment_system.md, MAKEUP_BACKLOG.md, CINEMATIC_LAYER_v1.md, RIMA_MASTER_ART_PIPELINE.md, chibi_lore_integration_decision_2026-05-13.md*

> Bu dosya **genel haritadır** (high-level: sistem dağılımı, mob, narrative, asset pipeline overview).
> Hafta-hafta detaylı plan + bütçe + kritik patika için: [`FAZ_DETAILED_SCOPE.md`](FAZ_DETAILED_SCOPE.md)
> Claude'a sadece çalışılacak fazın dosyasını ver (`FAZ1_CORE_LOOP.md` vb.).

---

## KARARLAR (Kesinleşmiş — 2026-04-09)

- **İsim teması:** Seçenek C (Hybrid) — Warblade, Elementalist, Shadowblade, Ranger, Ravager, **Ronin**, **Gunslinger**, **Brawler**, Summoner, Hexer
- **Ronin + Gunslinger + Brawler:** Faz 3 ANA class'ları
- **~~Crusader~~ → KALDIRILDI** (Brawler ile değiştirildi)
- **~~Lancer~~ → KALDIRILDI** (Fantasy oturmadı — MASTER_KARAR_BELGESI Karar #2)
- **Boss kaynağı:** BOSS_DESIGN.md (daha güncel, detaylı)
- **Mob kaynağı:** MOB_TASARIMI + MEKANIK_KARARLARI (iki set birleşik — toplam 16 benzersiz mob)
- **~~Rift Parry~~ → KALDIRILDI** (Karar #6 — class'a özel parry skill'leriyle değiştirildi)
- **Fracture Echoes:** Boss varyasyon sistemi — her boss 5 echo (MASTER_KARAR_BELGESI.md)
- **Tempest + Hemomancer:** Post-launch expansion (Faz 5 sonrası)
- **Faz numaralandırma:** GDD bazlı (daha granüler)
- **Detaylı kararlar:** `../MASTER_KARAR_BELGESI.md` dosyasında
- **Act 1 oda revize (Karar #62 LOCKED):** ~~8-9 oda~~ -> **15 node** (1 entry + 6 combat + 2 elite + 2 rest + 1 shop + 1 curse gate + 1 mystery + 1 boss). Detay: `../dungeon_act1_map.md`
- **Map Fragment + Kirrik Tas Tablet (Karar #63 LOCKED):** Fragment-based reveal sistemi, Boss kapisi 8 fragment. UI: TAB MapPanel + sol-ust MiniMap.
- **Silah Gorünürlük Single-State (Karar #71 LOCKED):** Silah hep elde, Ronin istisna (sheath/draw kimligi). Pixel art constraint + combat-readability. Detay: `../MAKEUP_BACKLOG.md` + `../CINEMATIC_LAYER_v1.md`
- **AD v1 Sprint (Karar #64-70):** ActionCommitProfile 5 alan, 3-layer feedback, posture v1 (boolean armor flag), dash-cancel per-class (Ravager/Shadow %15-25, Ranger/GS %30-55, Warblade/Brawler %60-75), OnDash proc, Cross-Class Proc text feedback.
- **S59 Pivot LOCKED (Karar #72-76, 2026-05-12):** Pure 2D top-down + 64x64 chibi karakter, silahlı 1-piece (sınıf-silah sabit, body-only + WeaponAnchorMap REVOKED), boyut hiyerarşi 2^n + PPU=64 standardize, Final Boss 256x256+PPU=64 (sahnede ~2.5x), Map tools KULLANILMAYACAK (NLM lock), asset prompt format TYPE/HEAD/BODY/LIMBS. Eski 2.5D mimari + 128px native + chibi YASAK + KayKit/Blender pipeline REVOKED. Mevcut RIMA projesi RESTORE (RIMA_2.5D nested arsivlenecek).

---

## FAZ HARITASI

| | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 |
|---|---|---|---|---|---|
| **Hedef** | Combat hissi | İlk oynanabilir loop | Dual-class kırılma + 4 yeni class | Demo-ready run | Early Access |
| **Class** | 1 (Warblade) | 4 (+Elem, Shadow, Ranger) | 8 (+Ravager, **Ronin**, **Gunslinger**, **Brawler**) | 10 (+Summoner, Hexer) | 10 (tam set) |
| **Act** | Act 1 kismen (~5-7 node — Entry+Combat+Elite slice) | Act 1 tam (15 node, dungeon_act1_map.md) | Act 1 + Act 2 | Act 1-2 tam | Act 1-2-3 + Final |
| **Boss** | Penitent Sovereign (F1) | Penitent Sovereign (F1+F2) | Echo Twin (F1+F2) | Fracture Sovereign (F1-F3) | The Architect (F1-F4) |
| **Mob** | 7 (3 grunt + 4 prefab) | 9 (+SeamCrawler, Twice-Born) | 11 (+EchoHound, FractureBorn) | 13 (+SporeHollow, TheWound) | 16 (tam set) |
| **Oda** | Combat, Elite | +Shop, Unknown | +Spirit Encounter | +Curse Gate, Event | Tam |
| **Tier** | Common | Common, Rare | +Epic | +Legendary | Tam |
| **Economy** | Shards | Shards + Echoes basit | Echoes aktif | + meta shop | Tam meta-prog |
| **Cross-class** | — | — | Pasif (28 kombo: 8x7/2) | +Ultimate | 45 kombo (10x9/2) |
| **Echo Imprint** | — | Temel (4 class) | +Tag Sinerji | +Cross-class Imprint | Tam |
| **Map Fragment** | — | spec only | implement | 8-fragment boss kapisi | tam reveal sistemi |
| **Combat Feel (AD v1)** | ActionCommit + commit windows | 3-layer feedback | posture v1 + dash-cancel | Cross-Class Proc text + OnDash | Resonance 2-tag (v2) |
| **Cinematic Layer** | — | Katman A (camera) | Katman B (env) + C (UI) | Katman D (boss intro frames) | 3-ending cinematic |
| **Silah/Asset Pipeline** | Warblade base + 3-segment skill | 3 yeni class (10x4x6 frame) | 4 yeni class + weapon pass | 2 yeni class + makeup #1-3 | tum siniflar makeup #4-8 |
| **Fracture Echoes** | — | — | — | ✅ (ilk 2 boss) | ✅ (tüm bosslar) |
| **Dosya** | `FAZ1_CORE_LOOP.md` | `FAZ2_FIRST_PLAYABLE.md` | `FAZ3_SECONDARY_CLASS.md` | `FAZ4_FULL_DEMO.md` | `FAZ5_FULL_GAME.md` |

---

## ASSET PIPELINE -- Faz Basina Üretim Yol Haritasi

Detayli pipeline: `../../GUIDES/RIMA_MASTER_ART_PIPELINE.md`

| Faz | PixelLab Üretim | ChatGPT Tile | Cinematic Frame | Toplam Kredi |
|---|---|---|---|---|
| Faz 1 | Warblade tam (~1000 cred) | F1 floor + W1 wall (DONE) | — | ~1000 |
| Faz 2 | 3 sinif P0 (~3000 cred) | F2 floor + W2 wall + Trans | — | ~3000-3500 |
| Faz 3 | 4 sinif P0 + 4 sinif P1 (~5000 cred) | F3 floor + OBW (DONE) | Boss intro v1 (~10 frames) | ~5500 |
| Faz 4 | 2 sinif P0 + 8 sinif P1 (~6000 cred) | F4 Rift tile set | Boss intro v2 + 3-ending preview | ~6500 |
| Faz 5 | 10 sinif P2 (Ulti) + makeup polish (~4000 cred) | All Acts complete | 3-ending full + Architect 4-faz | ~5000 |

**Tier 2 abonelik (~2000-3000 cred/ay):** Faz 1-2 tek ay sigar, Faz 3-5 her biri ~2-3 ay.
**Toplam tahmini kredi:** ~21000-22000 (~7-9 ay solo dev asset production paralel olarak code+design ile).

---

## RIMA / RIFT MARCH NARRATIVE LAYER (Candidate - 2026-05-04)

> Lore audit verified KEEP (lore audit 2026-05-09). This is a phase-level story split only; no mechanical scope is added here.
> Detailed pointer: `../../MEMORY/MEMORY.md` (index)

| | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 |
|---|---|---|---|---|---|
| **RIMA read** | Game/place/symbol | Full Act 1 symbol | Seal/mechanism hint | Lock/warden system | Full truth |
| **Rift March read** | Visual hint only | Lore fragments | Named/foreshadowed | Threat reframed | Final reveal |
| **Boss meaning** | Enemy ruler | First seal stress | Echo/duality clue | Bosses as locks/echoes | Final boss as last lock or Architect |
| **Player knowledge** | "RIMA is where I fight" | "RIMA has old rift history" | "RIMA repeats/records runs" | "Victory may weaken containment" | "Killing final boss changes the march" |
| **Ending hook** | None | None | Seed only | Strong foreshadow | Choice/post-game/sequel hook |

Narrative direction:
- Keep the logo clean as `RIMA`; do not force `RIft MArch` hidden letters into production logo.
- Use `Rift March` as lore: rifts open and outside forces begin marching into reality.
- Use `RIMA` as the seal, place, machine, ritual, or command system that delays/contains the
  march.
- Candidate final reveal: the final boss may be the last lock/warden, so killing it can trigger
  a final choice or post-game state instead of a simple "evil defeated" ending.

---

## MOB DAĞILIM HARİTASI (Tüm Aktlar)

### Set A — Mevcut Prefablar (MEKANIK_KARARLARI)

| Mob | Act 1 | Act 2 | Act 3 | Boyut | Tier |
|-----|-------|-------|-------|-------|------|
| ShardWalker | ✅ | ✅ | ✅ | 128px (S43) | Grunt |
| VoidThrall (+HalfThrall) | ✅ | ✅ | — | 128px (S43) | Grunt |
| Penitent | ✅ | ✅ | — | 128px (S43) | Grunt |
| ChainWarden | ✅ | ✅ | ✅ | 128px (S43) | Grunt |
| RelicCaster | — | ✅ | ✅ | 128px (S43) | Grunt |
| FractureImp | ✅ | ✅ | — | 128px (S43) | Grunt |
| SeamCrawler | ✅ | ✅ | — | 256×128px (S43) | Grunt |

### Set B — Planlanan (MOB_TASARIMI)

| Mob | Act 1 | Act 2 | Act 3 | Boyut | Tier |
|-----|-------|-------|-------|-------|------|
| Hollow Mite | ✅ | ✅ | — | 48px | Swarm |
| Echo Hound | — | ✅ | ✅ | 96px | Grunt |
| Twice-Born | ✅ (nadir) | ✅ | — | 160px | Elite |
| Fracture-Born | — | ✅ | ✅ | 160px | Elite |
| Spore Hollow | — | ✅ | — | 160px | Elite |
| Rift Maw | — | — | ✅ | 160px | Elite |
| Class Mimic | — | ✅ | ✅ | 128px | Special |
| Remnant Host | — | — | ✅ | 160px | Special |
| The Wound | ✅ (nadir) | ✅ | ✅ | 128px | Special |

---

## CLASS DAĞILIM HARİTASI

| Class | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 | Post |
|-------|-------|-------|-------|-------|-------|------|
| Warblade | ✅ Primary | ✅ | ✅ | ✅ | ✅ | — |
| Elementalist | — | ✅ | ✅ | ✅ | ✅ | — |
| Shadowblade | — | ✅ | ✅ | ✅ | ✅ | — |
| Ranger | — | ✅ | ✅ | ✅ | ✅ | — |
| Ravager | — | — | ✅ | ✅ | ✅ | — |
| **Ronin** | — | — | ✅ | ✅ | ✅ | — |
| **Gunslinger** | — | — | ✅ | ✅ | ✅ | — |
| **Brawler** | — | — | ✅ | ✅ | ✅ | — |
| Summoner | — | — | — | ✅ | ✅ | — |
| Hexer | — | — | — | ✅ | ✅ | — |
| Tempest | — | — | — | — | — | Post-launch DLC |
| Hemomancer | — | — | — | — | — | Post-launch DLC |
| ~~Crusader~~ | — | — | KALDIRILDI | — | — | — |
| ~~Lancer~~ | — | — | KALDIRILDI | — | — | — |

---

## MASTER_KARAR_BELGESI SENKRONU

> FAZ_MASTER son guncelleme: 2026-05-09 (S46). Detayli kararlar `MASTER_KARAR_BELGESI.md`'de.
> #1-#71 isaretlendi; #62-71 S46 ile eklendi.

| Karar # | Konu | Faz Etkisi |
|---------|------|-----------|
| #17 | V Meter ayrımı — her class farklı dolum koşulu | Faz 1+ |
| #18 | Item System D — Relic + Skill Modifier, ekipman slot yok | Faz 2+ |
| #22-23 | Rift Break sistemi — boss/elite slow-mo interactive phase | Faz 2+ |
| #24 | Cross-class Tier Unlock — Act 2 boss sonrası Tier 3 açılır | Faz 2+ |
| #27 | Echo Imprint — her 3 combat odada 1 Imprint sunusu | Faz 2+ |
| #28 | Tag Sinerji — 2 aynı tag → otomatik pasif bonus | Faz 2+ |
| #29 | Oda sayısı revizyonu — 8-9 / 9-11 / 9-11 / 5-6 (eski tablo geçersiz) | Tüm fazlar |
| #30 | Ton: Fractured Epic (grimdark değil) | — |
| #31 | Ghost Attack Opsiyon C — cross-class + Z/X secondary | Faz 2+ |
| #32 | Mob Armor Variant — Normal/Armored/Heavily Armored tier | Faz 2+ |
| #34 | Class cinsiyetleri 5E/5K kilitlendi | — |
| #40/#45 | Kamera: PixelLab Low Top-Down = ~35° ARPG (#36 override) | — |
| #37-38 | Ranger/Gunslinger identity kilitleri | — |
| #46-48 | Animasyon: Run 6f, 4 yön, Death/Hit 4 yön | — |
| #50-51 | Feel Toggles default ON / Localization Day 1 TR+EN | Faz 1+ |
| #52 | Skill VFX + Projectile mimarisi kilitlendi | Faz 2+ |
| #53 | 4 Cardinal yön S/E/N/W kilitlendi (#49 override) | — |
| #54 | Ulti Toggle per-skill Shift+key, Lock ON default, Perfect Condition | Faz 1+ |
| #55 | Brawler state = Shattered (Sundered IPTAL, sadece Warblade uretir) | Faz 1+ |
| #56 | Execute gates: HP<30% YASAK, class-specific state gate | Faz 1+ |
| #57 | Counter arketip ayrimi: Warblade=absorb, Ronin=pre-draw, Brawler=whiff | Faz 1+ |
| #58 | Movement Option C: Space = kisa dash, no state/damage/resource | Faz 1+ |
| #59 | Pixel Art Constraint: skill VFX mob-side efektler sinirli | Faz 1+ |
| #62 | Act 1 node sayisi: 15 node (entry+combat+elite+rest+shop+curse+mystery+boss) | Faz 2+ |
| #63 | Map Fragment + Kirrik Tas Tablet reveal sistemi, 8 fragment boss kapisi | Faz 2+ |
| #64-70 | AD v1 Sprint: ActionCommitProfile, 3-layer feedback, posture v1, dash-cancel | Faz 1+ |
| #71 | Silah Single-State: hep elde, Ronin sheath/draw istisna | Faz 1+ |

**Durum: SYNC GUNCELLENDI 2026-05-09 S46 (#60-#71 eklendi).**

---

## ODA TİPİ DAĞILIMI

| Oda | İkon | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 |
|-----|------|-------|-------|-------|-------|-------|
| Combat | ⚔️ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Elite | 💀 | ✅ | ✅ | ✅ | ✅ | ✅ |
| Shop | 🛒 | — | ✅ | ✅ | ✅ | ✅ |
| Unknown | ❓ | — | ✅ | ✅ | ✅ | ✅ |
| Spirit Encounter | 👁️ | — | — | ✅ | ✅ | ✅ |
| Curse Gate | 🌀 | — | — | — | ✅ | ✅ |
| Event | 🎲 | — | — | — | ✅ | ✅ |

---

## BOSS DAĞILIMI

| Boss | Faz Sayısı | Hangi Fazda | Geçiş HP |
|------|-----------|-------------|----------|
| Penitent Sovereign | 3 | Faz 1 (F1 only) → Faz 2 (tam) | %66 / %33 (S42) |
| Echo Twin | 2 | Faz 3 | %40 |
| Fracture Sovereign | 3 | Faz 4 | %60 / %30 |
| The Architect | 4 | Faz 5 | %75 / %45 / %20 |

---

## SİSTEM DAĞILIMI

| Sistem | Faz |
|--------|-----|
| Rage sistemi | 1 |
| Component-based mob mimarisi | 1 |
| Elite affix (4 tip) | 1 |
| Skill draft (Common) | 1 |
| Status effect sistemi | 1 |
| HUD 6 slot (4 aktif, 2 kilitli) | 1 |
| Ölüm + restart ekranı | 1 |
| Map fragment / kısmi görünür harita | 1 |
| ClassData + ClassManager + ClassSelectUI | 2 |
| Mana/Energy/Focus/CP kaynak sistemleri | 2 |
| ShopManager + GoldManager | 2 |
| Shards (in-run currency) | 2 |
| Rare skill tier | 2 |
| Sandık sistemi (3 tip) | 2 |
| Reroll sistemi (1 ücretsiz) | 2 |
| Secondary class seçimi | 3 |
| +2 aktif slot açılışı | 3 |
| Cross-class pasif sistemi (28 kombo) | 3 |
| Mixed draft oranları | 3 |
| Spirit Encounter (3 tip) | 3 |
| Epic skill tier | 3 |
| Echo Imprint (full set 10 class) | 3 |
| Tag Sinerji Bonusu | 3 |
| Ronin kaynak: Draw Tension | 3 |
| Gunslinger kaynak: Heat | 3 |
| Brawler kaynak: Charge (0-5) | 3 |
| Cross-class Ultimate | 4 |
| Curse sistemi (5 efekt) | 4 |
| Event odası (10 event) | 4 |
| Temel meta-progression (Echo harcanır) | 4 |
| Fracture Echoes (ilk 2 boss) | 4 |
| Legendary skill tier | 5 |
| Grudge / Nemesis sistemi | 5 |
| Class unlock sistemi | 5 |
| Tam meta-progression + Hub NPC'ler | 5 |
| 90 cross-class kombo (10×9) | 5 |
| Fracture Echoes (tüm bosslar) | 5 |
| Zorluk modu (Echo/Rift/Fracture/Void) | 5 |

