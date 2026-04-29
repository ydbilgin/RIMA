# RIMA — FAZ MASTER (Tüm Fazların Özet Haritası)

*Son güncelleme: 2026-04-09 | Kaynak: GDD, SINIF_VE_SKILL, MOB_TASARIMI, BOSS_DESIGN, MEKANIK_KARARLARI*

> Bu dosya genel haritadır. Claude'a sadece çalışılacak fazın dosyasını ver.

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

---

## FAZ HARITASI

| | Faz 1 | Faz 2 | Faz 3 | Faz 4 | Faz 5 |
|---|---|---|---|---|---|
| **Hedef** | Combat hissi | İlk oynanabilir loop | Dual-class kırılma + 4 yeni class | Demo-ready run | Early Access |
| **Class** | 1 (Warblade) | 4 (+Elem, Shadow, Ranger) | 8 (+Ravager, **Ronin**, **Gunslinger**, **Brawler**) | 10 (+Summoner, Hexer) | 10 (tam set) |
| **Act** | Act 1 kısmen (8-9 oda) | Act 1 tam | Act 1 + Act 2 | Act 1-2 tam | Act 1-2-3 + Final |
| **Boss** | Penitent Sovereign (F1) | Penitent Sovereign (F1+F2) | Echo Twin (F1+F2) | Fracture Sovereign (F1-F3) | The Architect (F1-F4) |
| **Mob** | 7 (3 grunt + 4 prefab) | 9 (+SeamCrawler, Twice-Born) | 11 (+EchoHound, FractureBorn) | 13 (+SporeHollow, TheWound) | 16 (tam set) |
| **Oda** | Combat, Elite | +Shop, Unknown | +Spirit Encounter | +Curse Gate, Event | Tam |
| **Tier** | Common | Common, Rare | +Epic | +Legendary | Tam |
| **Economy** | Shards | Shards + Echoes basit | Echoes aktif | + meta shop | Tam meta-prog |
| **Cross-class** | — | — | Pasif (28 kombo: 8×7÷2) | +Ultimate | 45 kombo (10×9÷2) |
| **Echo Imprint** | — | Temel (4 class) | +Tag Sinerji | +Cross-class Imprint | Tam |
| **Fracture Echoes** | — | — | — | ✅ (ilk 2 boss) | ✅ (tüm bosslar) |
| **Dosya** | `FAZ1_CORE_LOOP.md` | `FAZ2_FIRST_PLAYABLE.md` | `FAZ3_SECONDARY_CLASS.md` | `FAZ4_FULL_DEMO.md` | `FAZ5_FULL_GAME.md` |

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

> FAZ_MASTER son güncelleme: 2026-04-28 (S43 partial). Detaylı kararlar `MASTER_KARAR_BELGESI.md`'de.
> #17-#52 tam işlenmedi — Faz 2 başında detaylı güncelleme yapılacak.

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
| #40/#45 | Kamera: PixelLab High Top-Down = ~35° ARPG (#36 override) | — |
| #37-38 | Ranger/Gunslinger identity kilitleri | — |
| #46-48 | Animasyon: Run 6f, 4 yön, Death/Hit 4 yön | — |
| #50-51 | Feel Toggles default ON / Localization Day 1 TR+EN | Faz 1+ |
| #52 | Skill VFX + Projectile mimarisi kilitlendi | Faz 2+ |
| #53 | 4 Cardinal yön S/E/N/W kilitlendi (#49 override) | — |

**Durum: SYNC PENDING — Faz 2 başında tam işlenecek.**

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
