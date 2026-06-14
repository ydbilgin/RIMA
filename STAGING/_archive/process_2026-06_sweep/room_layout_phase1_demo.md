# Faz 1 Milestone Demo — 5 Oda Layout Vision

Track A — Sonnet design pass, 2026-05-27.
Scope: 5-oda layout vision (no code). Warblade TEK, 4 mob, Map Fragment + Gate
loop, ~10 dk total playtime. Hades Elysium V1 wall-less brand korunur.

---

## 1. Mevcut PlayableArena_Test01 Inceleme Raporu

Scene path: `Assets/Scenes/Test/PlayableArena_Test01.unity` (active, isDirty=true).
Root count: 17. Inspection via UnityMCP execute_code + get_hierarchy.

### 1.1 LIVE (sahnede mevcut, çalışıyor)

| Sistem | Detay | Durum |
|---|---|---|
| Floor Grid | cellLayout=Isometric, cellSize=(1.00, 0.61, 1.00), cellSwizzle=XYZ | LIVE (kanonik 0.609375 ≈ 0.61 match) |
| Tilemap (Floor sortingLayer, order 0) | 2368 tile total; -25..25 sample = 1308 tile (4 floor variants ağırlıklı + 106 cobblestone) | LIVE — geniş play area |
| VoidBlocker (Floor layer, order -999) | 5232 tile (NoPass outer ring) | LIVE |
| CliffTilemap (Floor layer, order -1) | 311 tile, bounds Size=78×77 | LIVE (S110 carry doğru) |
| RoomBackgroundRig | 7 child (parallax planes) | INACTIVE (kapalı) |
| RIMA_Cycle2_Dressing | 4 child kategori (column/brazier/rune dressing) | INACTIVE (kapalı) |
| ArenaAreaLight (Light2D) | spot/area | INACTIVE |
| Global Light 2D | ambient | LIVE |
| Cliff_cliff_E (rim object) + ~233 Parallax_cliff_cyan_glow_Near siblings | Floor rooted decor children | LIVE ama kalabalık (1 oda için aşırı) |
| Player | pos (0,0,0), Rigidbody2D + PlayerController + CombatHandler + HandAnchorAttach + RageSystem + Health | LIVE |
| Main Camera | PixelPerfectCamera + CameraFollow → Player | LIVE |
| WalkabilityMap | LIVE (reachable cell precompute, S108 portal bridge) | LIVE |
| PortalSpawnAnchor | pos (-4.45, 2.77, 0) | LIVE (Phase 1 portal MVP) |
| PortalSpawnController + PortalRewardBridge | child: Portal_0_Treasure (1 portal seeded) | LIVE — ama bu CANONICAL DEĞIL: portal değil, GATE olmalı |
| DraftManager | LIVE (skill draft UI) | LIVE |
| DeathScreenCanvas | LIVE | LIVE |
| Systems (HitStop + CameraShake) | LIVE | LIVE |
| FractureImp prefab instance | INACTIVE, scene'de hazır (manuel spawn placeholder) | INACTIVE |

### 1.2 EKSIK (Faz 1 demo için gereken)

- **Gate sistemi:** PortalSpawnController/PortalRewardBridge "portal" terminolojisi ile
  duruyor. Canonical: Gate (kapı), GateSocket (data-driven), 1-3 exit. Portal terimini
  Track B'de Gate'e rename gerekecek. (Track A bu doc'ta sadece spec verir.)
- **Map Fragment drop entity:** Mob kill sonrası zemine düşen, pickup ile gate unlock
  tetikleyen interactable. Sahnede yok.
- **Mob spawn point GameObject'leri:** Her oda için edge/flank pocket pozisyonlarında
  spawn transform marker yok. Sahnede tek bir inactive FractureImp prefab var.
- **Visual focal point markers:** Cyan rune odak yeri yok (CliffTilemap brand'i tutuyor
  ama merkez focal element yok).
- **Per-room scene split:** Şu an tek devasa arena. 5 oda için ya 5 ayrı sahne ya
  encounter-internal sub-room sekansı gerek (Karar #149 sub-room sekansı doğru yol).

### 1.3 Mevcut sahne 5 odadan hangisi?

Mevcut PlayableArena_Test01, **Room 2 — Combat Medium** spec'ine en yakın:
- 50×50 cell sample bölgesinde ~1300 playable tile → 16×10 sub-room iki adetlik
  alana denk (Karar #149 default).
- Tek odak (cliff perimeter + cyan parallax rim) Hades Elysium V1 wall-less marker
  zaten oturmuş.
- Tek FractureImp manuel placeholder, Room 2 spawn budgetine uygun start.

Diğer odalar yeni layout gerektirir. Mevcut arena'yı Room 2 prototip kabul edip
diğerlerini ondan türetmek en az iş.

---

## 2. NLM Canonical Findings

### 2.1 Combat Room Canon (NLM query 1)

| Karar | Detay |
|---|---|
| Karar #149 LOCK | Combat node = 1 EncounterTemplateSO içinde **3-5 sub-room** sekansı. Tek devasa arena DEĞİL. |
| Default sub-room size | **16×10 cell** (Faz 1 default). |
| Min sub-room size | **12×8 cell** — sadece connector / ambush pocket / low-threat transition için. |
| Şekil | Diamond REVOKED. İzin verilen: square, rectangular, oval, L-shape, cross, crescent. Wall-less open. |
| Dash lane | En az 2 yönde 10+ unit clean dash lane zorunlu. |
| Center | Boss/elite oda merkezinde blocker YASAK. Obstacle max 2 tile yüksek. |
| Mob spawn | **Kenar cell pocket / flank**, asla merkez yığılma. Pre-authored socket + enter-trigger hibrit. |
| Mob budget | Act 1 Combat: **8-12 threat point**. Elite: 14-18. |
| Wave | Faz 1: 1-2 wave. 2. wave 1. wave %50 öldüğünde trigger (opsiyonel Faz 1). |
| Gate sistemi | GateSocket data-driven (DoorNorth/East cardinal child'lar IMPLEMENTATION PLACEHOLDER, final değil). Inverse direction kuralı: N↔S, E↔W. |
| Sub-room transition | Fade-to-black (RoomTransitionFX). Door+pan REJECTED, seamless REJECTED. |
| Sub-room reward | YOK. Sadece encounter-final reward (Map Fragment + Skill Draft) sub-room N temizlendikten sonra. |

### 2.2 Reward Room Canon (NLM query 2)

| Karar | Faz 1 Demo |
|---|---|
| Skill Draft (Map Fragment + 3-seçenek UI) | **ZORUNLU**, tek reward döngüsü |
| Healing fountain | **YASAK** — RIMA'da rest room yok, HP lifesteal + boss/elite kill yenileme |
| Chest (Common/Rare/Rift) | **YOK** — Faz 2 scope |
| Forge / NPC vendor | **YOK** — Faz 2+ |
| Reward room boyut (A04 Map Fragment Vestibule) | **16×12 cell**, 1 kapı (geri dönüş yok, tek-yönlü exit) |
| İçerik | Tek map table/plinth ekseninde küçük chamber. Ink/map cracks plinth'ten dışarı yayılır. |
| Kabuk | Symmetrical, quiet, safe camera framing |
| Mob spawn | **CombatQuestion: none** — mob spawn kesinlikle YASAK |

Önemli not: Faz 1'de Skill Draft bir reward "oda"sı değil, her Combat/Elite oda
temizlenince **o oda içinde tetiklenen UI**. A04 Vestibule (ayrı reward oda) Faz 2+
scope'unda olur. **Faz 1 demo için "Room 4 — Reward" ayrı oda değil, Combat
odasında kill sonrası Map Fragment drop → pickup → in-place Skill Draft UI olmalı.**

### 2.3 Boss Room Canon (NLM query 3)

| Karar | Detay |
|---|---|
| Tilemap boyut | **40×30 cell** (özel) |
| Temiz combat alan | **34×22 cell** |
| Platform | **14×14 cell dairesel/sekizgen ritüel platformu** merkezde |
| Kapı sayısı | **1 (sadece giriş)** — exit yok, boss kill → demo end |
| Dash lane | En az 2 karşılıklı (N-S) temiz lane. Faz 1'de 2-yönlü serbest. |
| Faz 1 hazard | Litany of Restraint — pusula köşelerinde 4 chain anchor, line geçiş 1.5s slow + 15 dmg |
| Telegraph alan | Chain Whip (6m line) + Penitent Surge (4m radius) + Shackle Cast (8m single-target) telegraph window'ları için merkez açık zemin |
| Center | Boss merkezinde — Faz 1'de Rift Tear/Bloom HENÜZ aktif değil, Faz 2+ hazard |
| Foreground occluder | Kenardan içe max 3u, weak-point alanı temiz |

---

## 3. 5 Oda Spec Tablosu

Faz 1 demo akışı **5 oda lineer** — sub-room sekansı DEĞİL, 5 ayrı oda. Karar #149
sub-room sequence Faz 2 daha doğal (3-5 ana node, her node 3-5 sub-room). Faz 1
demo için 5 oda = **5 ana node, her node tek sub-room**. Bu, hem demo loop'unu
~10 dk'ya çeker hem Karar #149 yapısını ihlal etmez (sequence length = 1 izinli).

| # | Room ID | Tip | Floor Cell | Şekil | Cliff Perimeter | Gate | Map Fragment | Mob (threat pt) | Focal Element | ~Süre |
|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Room1_TutorialCombat | Combat | 12×8 = 96 cell | rectangular small | full surround, S açık | 1 entry (S, locked-after-enter) + 1 exit (N, locked-until-clear) | merkez (center) | 3× FractureImp = 3pt | Cyan rune merkez (focal) | 60-90s |
| 2 | Room2_CombatMedium | Combat | 16×10 = 160 cell | square medium | full surround, S açık + N açık | 1 entry (S) + 1 exit (N) | merkez | 3× FractureImp + 2× ShardWalker = 9pt | Brazier (NW corner) | 90-120s |
| 3 | Room3_CombatHard | Combat | 16×10 + ambush pocket = ~190 cell | L-shape (ana 16×10 + 6×4 ambush) | full surround, S açık + E açık | 1 entry (S) + 2 exit (N, E) | ambush pocket sonrası ana oda merkez | 4× FractureImp + 2× ShardWalker + 1× HollowHulk (elite stand-in) = 18pt | Kırık sütun (broken column, NE corner) | 120-180s |
| 4 | Room4_Vestibule | Reward (A04) | 16×12 = 192 cell | small chamber, symmetric | full surround, S açık | 1 entry (S) — geri-dönüş model, exit aynı kapı (entry+exit unified) | merkez plinth (yok, replaced by direct entry → Skill Draft) | YOK (NLM canon: mob YASAK) | Map plinth merkez, ink crack decal | 30-60s |
| 5 | Room5_BossArena | Boss | 40×30 outer / 34×22 combat / 14×14 platform | circular ritual platform | thick perimeter shell, S açık | 1 entry (S, locked-after-enter), exit YOK (boss kill = demo end) | YOK (boss death = demo end) | 1× PenitentSovereign (Faz 1 only — %50 HP placeholder kill) + 4 chain anchor hazard | 4 chain anchor pusula köşeleri | 180-240s |

Total demo süresi: 8.5–11.5 dk → spec hedef ~10 dk match.

**Mob roster (NLM Faz 1 onaylı 8 mob'dan demo subset):**
- FractureImp (HP 25, 1pt, max 4): hızlı zayıf, 3-hit dies
- ShardWalker (HP 75, 3pt, max 2): orta bruiser
- HollowHulk (HP 350, 8pt, max 1, Elite slot): elite stand-in Room 3'te
- PenitentSovereign (boss, %50 HP placeholder kill — Faz 1 spec)

Anti-pattern check (NLM): "4× Imp + Crawler + Walker → görsel kalabalık" → Room 3
encounter'da Imp 4'e değil 4'e cap, Walker 2'de, Hulk solo → Room 3 = 4 Imp + 2
Walker + 1 Hulk = 4+6+8 = 18pt → Elite budget üstü değil (14-18 elite range OK).
Ama anti-pattern listesinde "4× Imp + Crawler + Walker" var → Crawler kullanmıyoruz
demo'da, Walker var → check passed.

---

## 4. Her Oda Detay

### Room 1 — Tutorial Combat

- **Amaç:** Player WASD + attack + dash mekaniği test, ilk Map Fragment loop demo.
- **Cell count:** 12×8 = 96 cell (NLM "connector/ambush" boyutu — tutorial için
  amaca uygun, hızlı clear).
- **Şekil:** Rectangular (geniş kenar yatay).
- **Cliff perimeter pattern:** Full surround, sadece S kenarda 2-cell entry gap.
  N kenar başlangıçta opak cliff (kilitli exit), ilk wave kill sonrası N gap açılır.
- **Gate yerleşimi:**
  - Entry: S kenar center (player_start +0, -4 cell), locked-after-enter (geri
    dönüş yok, oyuncu içeri girince kapanır).
  - Exit: N kenar center, locked-until-cleared. Map Fragment pickup = unlock.
- **Map Fragment drop:** Tüm mob ölünce arena merkezinde (0,0,0) spawn. WalkabilityMap
  reachable check zaten LIVE.
- **Mob spawn points (3 nokta):**
  - SP_1: NW corner pocket (cell -4, +3 relative to room center)
  - SP_2: NE corner pocket (cell +4, +3)
  - SP_3: N center pocket (cell 0, +3)
  - Hepsi edge/flank disiplini, merkezde spawn YOK.
  - Tek wave (Faz 1 default), entry trigger ile spawn.
- **Visual focal:** Cyan rune dekoratif sprite merkez (0,0) — placeholder olarak
  basit cyan SpriteRenderer, Track B real art.
- **Player start:** (0, -3.5, 0) (S entry gate'in 0.5 cell içerisinde).
- **Süre:** 60-90s clear.

### Room 2 — Combat Medium

- **Amaç:** İlk skill draft loop test, 2 farklı mob tipi tanıtım.
- **Cell count:** 16×10 = 160 cell (NLM default sub-room size).
- **Şekil:** Square medium, simetrik.
- **Cliff perimeter:** Full surround, S + N gap (entry + exit). E, W tam kapalı.
- **Gate yerleşimi:**
  - Entry: S kenar center.
  - Exit: N kenar center (locked-until-cleared).
- **Map Fragment drop:** Merkez, mob kill sonrası.
- **Mob spawn points (5 nokta):**
  - SP_1, SP_2, SP_3: FractureImp — NW, NE, N-center pocket (edge cells)
  - SP_4, SP_5: ShardWalker — W-mid, E-mid edge pocket
  - Tek wave, entry-trigger spawn.
- **Visual focal:** Brazier NW corner (warm orange contrast cyan'a). 1 adet,
  dekor süsleme YOK.
- **Süre:** 90-120s.

### Room 3 — Combat Hard

- **Amaç:** Elite mob + L-shape pacing (ambush pocket), 2-exit choice.
- **Cell count:** 16×10 ana + 6×4 NE ambush pocket = ~190 cell. L-shape connector.
- **Şekil:** L-shape — ana oda 16×10 + NE köşesinde 6×4 ambush pocket (cliff'le
  ayrılmış, dar 3-cell connector).
- **Cliff perimeter:** Full surround ana oda + ambush pocket. Cliff ayırıcı L'in
  iç köşesini oluşturur. Player visibility: ambush pocket gözükür ana odadan
  (max 2-tile yükseklik kuralı, NLM).
- **Gate yerleşimi:**
  - Entry: S kenar center (ana oda).
  - Exit 1: N kenar center (ana oda).
  - Exit 2: E kenar (ambush pocket'in dışı). 2 exit = route choice.
- **Map Fragment drop:** Ambush pocket clear sonrası ana oda merkezi.
- **Mob spawn points (7 nokta):**
  - Wave 1 (ana oda entry trigger): 2 Imp NW edge + 2 Imp NE edge + 1 Walker
    W-mid edge.
  - Wave 2 (ambush pocket trigger when player enters connector): 1 Walker
    + 1 Hulk (Elite) ambush pocket içinde.
  - Imp 4'e cap, Walker 2'de, Hulk solo → anti-pattern check OK.
- **Visual focal:** Kırık sütun (broken column) NE corner ambush pocket içinde.
  Elite encounter'ı framing eder.
- **Süre:** 120-180s.

### Room 4 — Vestibule (Reward)

- **Amaç:** A04 Map Fragment Vestibule kanonik prototip — quiet reward
  threshold odası, mob yok, Skill Draft UI dramatik staging.
- **Cell count:** 16×12 = 192 cell.
- **Şekil:** Small chamber, symmetrical (NLM canon: quiet, symmetrical).
- **Cliff perimeter:** Full surround, sadece S kenar 2-cell entry gap.
- **Gate yerleşimi:**
  - Entry: S kenar center (locked-after-enter).
  - Exit: aynı S gate (geri dönüş yok kanonik — ama Faz 1 demo flow Room 5 boss
    arena'ya geçiş gerek; alternatif: N exit unified at-skill-pick).
  - **Karar:** Skill pick sonrası S gate'in karşısında (N kenar) yeni exit gate
    visible olur. Tek-yön flow Room 5'e.
- **Map Fragment drop:** YOK — bunun yerine merkezde **map plinth** sahne objesi
  (interactable). Player plinth'e dokununca Skill Draft UI açılır (Map Fragment
  pickup behavior'unun symbolic stand-in'i).
- **Mob:** YASAK (NLM canon). Spawn point yok.
- **Visual focal:** Merkez plinth (map table). Ink/map crack decal plinth'ten
  dışarı radial yayılır (placeholder Track A: cyan glow sprite, Track B real art).
- **Süre:** 30-60s (skill choice deliberation dahil).
- **Önemli:** Bu oda NLM canon'a göre Faz 2+ scope. Faz 1 demo'da bu odayı atlama
  alternatifi: Room 3 → Room 5 direct (Room 3 sonu Skill Draft Room 3 içinde
  in-place). Ama 5-oda spec için Room 4 vestibule **rewarding-pause** olarak
  değerli — open question (bkz §8).

### Room 5 — Boss Arena (PenitentSovereign Faz 1)

- **Amaç:** Boss fight prototip, Litany of Restraint hazard test, demo finale.
- **Cell count:** Outer 40×30 = 1200 cell. Combat alan 34×22 = 748. Ritüel
  platform merkez 14×14 = 196.
- **Şekil:** Circular ritual platform içinde, thick perimeter shell dışında.
  Sekizgen veya pure circle (NLM cited "circular/octagonal platform").
- **Cliff perimeter:** Thick perimeter shell (cliff 4-tile derinlik) outer
  boundary. Iç platform cliff perimeter ile sınırlı değil — yumuşak floor
  decal geçişi. Foreground occluder kenardan içe max 3u (NLM).
- **Gate yerleşimi:**
  - Entry: S kenar center (locked-after-enter).
  - Exit: YOK (boss kill = demo end screen).
- **Map Fragment:** YOK (boss room map fragment drop etmez, NLM canon).
- **Mob spawn:** 1 PenitentSovereign center platform spawn. 4 chain anchor
  hazard pusula köşelerinde (NE, NW, SE, SW) — interactable destructible (40 HP/
  anchor), chain line geçiş 1.5s slow + 15 dmg.
- **Visual focal:** 4 chain anchor (NLM canon Litany of Restraint). 14×14
  ritüel platform merkez = visual anchor. Boss spawn cinematic platform
  merkez. Faz 1'de Rift Tear YOK (Faz 2 hazard).
- **Boss spec (NLM Faz 1):**
  - HP: %50'de death cinematic (Faz 1 placeholder, full 2-phase Faz 2'de)
  - Posture: 700
  - Pattern: Chain Whip → Penitent Surge → Chain Whip → Shackle Cast
  - Telegraph: 0.8-1.2s, hit window 1.0-1.5s
- **Player start:** S entry gate +0.5 cell içeri, (0, -10, 0) (boss arena
  merkez +0,+0 olursa).
- **Süre:** 180-240s.

---

## 5. ASCII Sketch (5 oda, top-down)

Notasyon: `.` = floor, `C` = cliff, `G` = gate (entry S, exit N/E), `M` = mob
spawn, `F` = Map Fragment drop, `P` = player start, `R` = visual focal (rune /
brazier / column / plinth), `B` = boss spawn, `A` = chain anchor.

### Room 1 (12×8)

```
C C C C C C C C C C C C
C M . . M . . . M . . C
C . . . . . . . . . . C
C . . . . R . . . . . C
C . . . . . . . . . . C
C . . . . F . . . . . C
C . . . . P . . . . . C
C C C C C G G C C C C C
```

### Room 2 (16×10)

```
C C C C C C C G G C C C C C C C
C R . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . M . . M . . M . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . F . . . . . . . C
C . . . . . . . . . . . . . . C
C M . . . . . . . . . . . M . C
C . . . . . . . . . . . . . . C
C . . . . . . . P . . . . . . C
C C C C C C C G G C C C C C C C
```

### Room 3 (L-shape, 16×10 + 6×4 NE ambush)

```
C C C C C C C C G G C C C C C C
C . . . . . . . . . . . . . . C C C C C C
C . . . . . . . . . . . . . . C M M . R C
C M M . . M M . . . M M . . . . . . . . C  ← ambush pocket (NE)
C . . . . . . . . . . . . . . C . M B . C   B=Hollow Hulk (Elite)
C . . . . . . . . . . . . . . C C C . . C
C . . . . . F . . . . . . . . . . . . G G  ← Exit 2 (E)
C . . . . . . . . . . . . . . C C C C C C
C M . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . P . . . . . . C
C C C C C C C G G C C C C C C C
```

### Room 4 (Vestibule, 16×12)

```
C C C C C C C G G C C C C C C C  ← Exit (Skill pick sonrası)
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . R . . . . . . . C   R=plinth (interactable)
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . C
C . . . . . . . P . . . . . . C
C C C C C C C G G C C C C C C C
```

### Room 5 (Boss, 40×30 — sembolik 20×15 sketch)

```
C C C C C C C C C C C C C C C C C C C C
C . . . . . . . . . . . . . . . . . . C
C . . A . . . . . . . . . . . . A . . C   A=chain anchor (NW, NE)
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . B . . . . . . . . . C   B=PenitentSovereign center
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . A . . . . . . . . . . . . A . . C   A=chain anchor (SW, SE)
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . . . . . . . . . . C
C . . . . . . . . . P . . . . . . . . C
C C C C C C C C C G G C C C C C C C C C
```

(14×14 platform sembolik — gerçekte cliff perimeter 40×30 outer, iç platform
14×14 floor decal differentiation; sketch ölçek sığdırılmış.)

---

## 6. Mevcut Sahne Değerlendirme

### 6.1 Korunan (PlayableArena_Test01 brand & sistemler)

- **Hades Elysium V1 wall-less brand** — floating arena + cliff edges + cyan
  glow Near parallax CliffTilemap formula. Tüm 5 odada bu pattern korunur.
- **Cyan rune focal motif** — Room 1 odak elementi olarak doğrudan adapt.
- **Cobblestone outer floor variant** (floor_base_01, 106 cell sample) — cliff
  perimeter altındaki cobblestone Vestibule (Room 4) zemin texture'u olarak
  geri kullanılabilir.
- **iso grid (cellSize 1×0.609375×1, cellLayout=Isometric, cellSwizzle=XYZ)** —
  tüm 5 odada bu grid spec aynı.
- **HIGH TOP-DOWN 3/4 sprite angle** — sprite stack tüm odalarda korunur.
- **WalkabilityMap reachability check** — Map Fragment drop pozisyonu sadece
  reachable cell'lere düşmeli, sistem LIVE.
- **CameraFollow + PixelPerfectCamera** — kamera config tüm odalarda aynı.
- **DraftManager + PortalRewardBridge** — DraftManager LIVE, portal naming
  Gate'e rename gerek (Track B).
- **Global Light 2D + Light2D config** — ArenaAreaLight inactive ama prefab
  olarak hazır.
- **HitStop + CameraShake (Systems root)** — combat feel LIVE.

### 6.2 Değişen (5 oda variety için)

- **Tek arena → 5 oda yapısı.** PlayableArena_Test01 prototip Room 2 olarak
  baseline. Diğer 4 oda yeni scene veya runtime sub-room loader.
- **Decor overload temizliği.** Şu an Floor child olarak ~233 adet
  `Parallax_cliff_cyan_glow_Near` instance var — bu Room 2 için bile aşırı.
  Cliff parallax 6-8 instance yeterli (RoomBackgroundRig 7 child zaten doğru
  count). Decor minimal disiplin: oda başına 1 focal element (cyan rune /
  brazier / column / plinth / chain anchor).
- **Gate sistemi.** PortalSpawnController/PortalRewardBridge → GateSocket
  data-driven rename. Locked/unlocked state visualization (placeholder Track A:
  cube quad with opacity, real art Track B).
- **Mob spawn point GameObject'leri.** Her oda için edge/flank pocket
  pozisyonlarında transform marker'lar. Şu an tek inactive FractureImp prefab,
  spawn point data-driven değil.
- **CliffTilemap pattern adaptation.** 311 tile mevcut Room 2 boyutuna match
  ediyor; Room 1 (96 cell) için ~80 cliff, Room 3 L-shape için ~140, Room 4
  vestibule için ~100, Room 5 boss için ~250+ thick perimeter shell.

### 6.3 Mevcut sahne 5'inden hangisi?

**Room 2 (Combat Medium) baseline.** Bu sahne ~16×10 sub-room ölçeğine en yakın,
brand pattern oturmuş, sistemler LIVE. Diğer 4 oda Room 2'den türetilir (Room 1
küçültme, Room 3 L-shape ekleme, Room 4 mob temizlik + plinth, Room 5
genişletme + chain anchor).

---

## 7. Track A / Track B Sınır

### 7.1 Track A (bu doc + functional minimum implementation scope)

**DAHİL:**
- Floor tilemap (tüm odalar, mevcut grid spec ile)
- Cliff perimeter (her oda için adapted CliffTilemap)
- Gate hitbox + locked/unlocked state machine (placeholder visual: cube/quad)
- Map Fragment hitbox + pickup trigger (placeholder visual: cyan sphere)
- Mob spawn point GameObject (transform position + spawn type/timing data)
- Player start transform (her oda)
- Mob prefab spawn (FractureImp prefab LIVE, ShardWalker ve HollowHulk
  placeholder data — Track B real)
- Boss spawn anchor (Room 5)
- Chain anchor placeholder (Room 5, destructible 40HP hitbox)
- Focal element placeholder marker (cyan rune / brazier / column / plinth /
  chain anchor — sade SpriteRenderer veya cube)
- Encounter clear → Map Fragment drop → pickup → unlock exit gate flow

### 7.2 Track B (otonom asset/art pipeline)

**DAHİL:**
- Cliff sprite variant'ları (her oda için tematik nuance — Room 1 sade, Room 3
  L köşeli, Room 5 thick perimeter shell)
- Parallax BG art (RoomBackgroundRig 6-7 layer art)
- Dekoratif öğeler (çatlak decal, kemik cluster, magic mark, yosun, ink/map
  crack — A04 Vestibule)
- Gate sprite stilleri (stone arch / wall breach / chained doorway / rift
  threshold — NLM canonical 6 allowed form)
- Map Fragment final art (currently cyan sphere placeholder)
- Plinth/map table art (Room 4)
- Chain anchor art (Room 5)
- Cyan rune art (Room 1)
- Brazier art (Room 2)
- Broken column art (Room 3)
- Boss PenitentSovereign sprite + animation
- Mob art ShardWalker, HollowHulk (FractureImp LIVE)

**ÇIKARILACAKLAR (Track A scope dışı):**
- Hiçbir final art asset
- Hiçbir custom shader / VFX
- Mob AI tuning (mob davranışı LIVE, sadece spawn count + position)
- Boss multi-phase logic (Faz 1 sadece %50 HP placeholder kill)
- Audio (SFX/music)

---

## 8. Open Questions (user karar gerek)

1. **Room 4 Vestibule include / exclude?**
   NLM canonical reward room A04 Vestibule Faz 2+ scope. Faz 1 demo'da Skill
   Draft her Combat odasında in-place trigger olur (Room 1, 2, 3 sonu). Ayrı
   Vestibule oda Faz 1'de **opsiyonel pause** olarak değerli ama demo loop'unu
   uzatır.
   - **Option A (canonical):** 5 oda = 4 Combat + 1 Boss. Room 4 skip. (Room 4
     yerine Room 3'ün ambush pocket'ı veya başka combat variation.)
   - **Option B (current spec):** 5 oda = 3 Combat + 1 Vestibule + 1 Boss.
     Vestibule rewarding-pause sağlar ama Faz 2 scope creep.
   - **Option C (hybrid):** Room 4 vestibule var ama Skill Draft Room 3 sonunda
     in-place tetiklenir; Vestibule sadece visual breather, mekanik etkisi yok.
   - **Default:** Bu doc Option B ile spec yazılmış. User confirm gerek.

2. **5 oda ayrı scene mi runtime sub-room mı?**
   - **Ayrı scene:** Her oda Assets/Scenes/Test/Room1.unity, Room2.unity vs.
     SceneManager.LoadScene transition. Basit, debug kolay.
   - **Runtime sub-room (Karar #149 model):** Tek scene, RuntimeRoomManager
     EncounterTemplateSO sequence + RoomTransitionFX fade-to-black. Karar #149
     ile uyumlu, Faz 2 scaling kolay.
   - **Default:** Karar #149 LOCKED → runtime sub-room model doğru yol. Ama Faz
     1 demo için ayrı scene daha hızlı prototype. User karar.

3. **PortalSpawnController/PortalRewardBridge rename?**
   NLM canon "portal" değil **Gate**. PortalSpawnController → GateController,
   PortalRewardBridge → GateRewardBridge (veya MapFragmentRewardBridge daha
   açık). Bu Track A (functional rename) mi Track B (cosmetic rename) mi?
   - **Default:** Track A (functional, çünkü logic flow Gate kanonik kontratı
     follow etmek zorunda — locked/unlocked state, target room icon, vs).

4. **Room 5 boss exit?**
   NLM Faz 1: PenitentSovereign %50 HP'de death cinematic → demo end. Exit gate
   yok. Demo end screen ne gösterir? "Demo complete" + restart button?
   - **Default:** "Demo complete" + restart to Room 1. (Restart Room 5 değil,
     Room 1 — full demo loop replay.)

5. **Map Fragment drop visual + pickup feel.**
   Track A placeholder cyan sphere yeterli mi yoksa scale/glow/wobble anim
   needed mı? Pickup SFX/VFX Track A scope mu?
   - **Default:** Placeholder static cyan sphere + pickup trigger (no SFX/VFX,
     Track B). Demo "functional minimum" hedef.

6. **Threat budget Room 3 = 18pt — Elite range üstü mü?**
   NLM Combat: 8-12 threat. Elite: 14-18. Room 3 = 18pt Elite range içinde
   ama mob spec'te HollowHulk "max 1 Elite/Unknown only" — Faz 1 demo'da
   Elite oda mı normal Combat mı?
   - **Default:** Room 3 = Elite-tagged Combat (canonical Faz 1 Act 1: 5-6
     Combat + 1 Elite + 1 Boss). Room 3 elite slot. 18pt OK.

---

## Önemli Not (Track A scope reminder)

Bu doc **design vision**, kod değil. Tüm cell count'lar, mob spawn pozisyonları,
gate placement'ları **bağlayıcı spec**. Implementation Track A'da functional
minimum (placeholder visual + LIVE logic), Track B'de art polish.

NLM canonical cevaplar §2'de, scene inspection §1'de. Open questions §8'de
user-decision pending — diğer her şey canonical lock'lara dayalı.
