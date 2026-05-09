# Act 1 -- Shattered Keep -- Map Cizim Paketi
**Tarih: 2026-05-09 | Durum: LOCKED v1 | Kaynak: Opus tasarim analizi + S46 user feedback**

## Mimari Karar -- KEEP Hades-style

Combat v1 + Alabaster Dawn v2 eklemeleri (DepthBand rest room, Resonance 2-tag,
ActionCommitProfile, posture meter) acik dunyaya degil **Hades-style ayrik oda akisina
zorunlu kiliyor**. Ayrik arena + kapi kilidi olmadan 2-tag procu yetismiyor, commit-combat
okunaklilik bozuluyor.

**Diablo paradigm**: REJECTED (devam) -- procedural scatter combat readability'yi yok eder.
**Open world (Hollow Knight)**: REJECTED (devam) -- 55-70dk run + draft pacing'i bozar,
backtracking allow eder.
**Hibrit StS macro graph + Hades micro arena**: KABUL -- RIMA zaten budur, sadece UI'i
StS-style gostermek yeni karar.

## Act 1 Node Duzeni (15 node -- 13 ana hat + 2 dal)

### Tur dagilimi

| Tur | Adet | Depth | Not |
|---|---|---|---|
| Entry koridoru | 1 | 0 | F1 baslangic |
| Combat | 6 | 1,2,4,5,6,7 | butce 8-12 threat |
| Elite | 2 | 3, 6 | butce 14-18, 1+ Elite mob |
| Rest (transition) | 2 | 3 (F1->F2), 5 (F2->F3) | combat-yok 16x12, ritual masa |
| Shop | 1 | 4 | 20x16 kapi kilidi yok |
| Curse Gate (dal) | 1 | 5 dal | 20x16 lanetli oda |
| Mystery (dal) | 1 | 4 dal | "?" runtime authored event |
| Boss | 1 | 7 | 40x30 ozel arena |
| **TOPLAM** | **15** | | |

### Lineer + hafif dal grafigi (her run ayni topoloji, icerik random)

```
[1 Entry] -> [2 Combat F1] -> [3 Combat F1]
                                     |
                            [4 Rest F1->F2] <- (zorunlu yol)
                                     |
                            [5 Elite F2] ----------- (re-merge)
                            |                    \
                    [6 Combat F2 BRUISER]    [6b Mystery? F2 dal]
                            |                   /
                            [7 Shop F2] <-------
                                     |
                            [8 Combat F2] --------
                            |                    \
                    [9 Rest F2->F3]          [9b Curse Gate F2 dal]
                            |                   /
                            [10 Combat F3] <----
                                     |
                            [11 Elite F3 miniboss-tier]
                                     |
                            [12 Combat F3 boss prep]
                                     |
                            [13 BOSS F3 40x30]
```

### Depth band -- tile pool mapping (LOCKED)

- Depth 0-2 (Threshold, F1, "clean built halls"): Node 1, 2, 3
- Depth 3-5 (Guard->Ossuary, F2, "cells + loops"): Node 4 (rest), 5, 6, 6b, 7, 8, 9 (rest), 9b
- Depth 6+ (Ritual->Containment, F3, "sacred geometry"): Node 10, 11, 12, 13 (boss)

### Procedural Per-Run Variation (S46 karar)

Topoloji sabit, icerik random. Her run'da node sayisi/turu/depth ayni; ama:

1. **Oda template secimi**: Her node tipi icin RoomRegistry pool'undan rastgele prefab.
   Combat odasi 6 node = 6 ayri combat prefab (overlap olabilir, max 2 ayni).
   Elite 2 node = 2 farkli Elite prefab.
2. **Mob composition**: Threat butcesi sabit (8-12 / 14-18); spawn template farkli.
   %30-40 odada Bruiser+2trash composition (2-tag ogrenme penceresi).
3. **Reward draft pool**: 3-choice draft her oda sonu -- pool farkli.
4. **Mystery (6b) icerigi**: Authored event seti (6 hazir event -> 1 random). StS event-style.
   (Q2 karari: authored, runtime random DEGIL.)
5. **Curse Gate (9b) Burden+Gift pair**: Pool'dan random. v3'te 5 portal turu acildiginda genisler.
6. **Map fragment reveal sirasi**: Reveal kurali sabit ama hangi node'un ne tur oldugu
   **icon olarak** gorunur -> oyuncu o tipe gore planlar (build planning UX).
7. **Rift Portal spawn**: %4 sans, pool {5,6,8,10,11}. Run-by-run hangi node'da cikacagi stochastic.

## Combat Oda Kati Kurallari (LOCKED)

- En az 2 yonde **10+ unit clean dash lane** (commit window dash-cancel icin; onceki 8u'dan artirildi)
- Boss/Elite oda merkezinde blocker YASAK
- Obstacle max 2 tile yuksek
- Mob spawn: kenar cell pocket / flank, asla merkez yigilma
- Elite/Miniboss arenalarinda en az **2 trap pocket** (3x3 kose alani, posture break itme yeri)
- Boss arena foreground occluder kenardan ice **3u'dan fazla tasmaz** (weak-point sprite alani temiz)

## Mob Butce (Act 1)

- Combat: 8-12 threat
- Elite: 14-18 threat (1+ Elite mob, HPx2.5, 1 Affix)
- 1. dalga = butce %40, 2. dalga = 1. %50 olunce (opsiyonel Act 1)
- Ayni turden max 4
- **Bruiser kompozisyon orani: %30-40** (2-tag biriktirme penceresi icin)
- Special mob YOK (Wound %5 nadir)

## Boss Kapisi Acilma Sarti

- 8 zorunlu fragment toplandigi zaman (combat 6 + elite 2 = 8 node).
- Rest node fragment **vermez** (Q5 karari: rest = combat-yok = fragment-yok; conceptual coupling preserved).
- Dal node'lar (6b, 9b) opsiyonel fragment verir; bonus, zorunlu degil.

## Rift Portal Act 1 Spawn (LOCKED %4)

- Spawn pool: Node {5, 6, 8, 10, 11}
- Min 8 oda spacing, max 1/act, 2/run
- Wall seam / room edge spawn, asla gate/loot/path bloke etmez
- En yuksek dramatic payoff slot: Node 11 (Elite, boss prep'e dramatik girer)

## Sonraki Adimlar

1. RoomRegistry SO populate -- her tip icin 2-4 prefab pool
2. DungeonGraph generation kodu -- 15 node template'i + dal node random select
3. Map fragment system implementasyon -- bkz. `TASARIM/map_fragment_system.md`
4. MapPanel UI (StS-style abstract graph) ve MiniMap (Hades-style sol-ust) implement
5. Pilot validation: 3 prefab Play mode'da Instantiate -> event fire -> console log

## Acik Sorular (CLOSED -- Opus karar verdi)

- [CLOSED] Q1: Rest node fragment kaynagi -> rest fragment yok (Q5 ile birlesti)
- [CLOSED] Q2: Mystery node icerigi -> AUTHORED EVENTS (StS event-style 6 hazir -> 1 random)
- [CLOSED] Q3: Spirit sistemi -> v1'DE YOK (sadece Shop; Spirit AD v2 onay sonrasi rotation aday)
- [CLOSED] Q4: Map UI -> HIBRIT (StS macro panel TAB + Hades-style sol-ust minimap surekli)
- [CLOSED] Q5: Boss kapisi fragment -> 8 fragment (combat+elite, rest haric)
