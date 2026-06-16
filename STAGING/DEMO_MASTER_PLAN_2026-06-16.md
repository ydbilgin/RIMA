# DEMO MASTER PLAN — 2026-06-16 (LIVE)

> Demo = 19 June 2026 (3 gun). Synthesis of 3 adversarial council reviews (C1 DEMO-RISK, C2 CANON, C3 COMPLETENESS) + real-code verification. Planning agent: read-only, no Unity calls.
>
> **CUT-LINE felsefesi:** golden-path PLAYABLE + DURUST (run-map yalan soylemez) once gelir. Yeni-davranis (beam/zoom coroutine) ve kompozisyon isi (buyuk oda) STRETCH/SONRA. Her T-item: compile + read_console=0 + screenshot zorunlu (G3).

---

## GROUND-TRUTH VERIFICATION (bu plan gercek koda dayanir)

| Iddia | Durum | Kanit |
|---|---|---|
| `RoomBankSO.GetList(Chest)` -> `default: return null`, `chestRooms` field YOK | DOGRULANDI | RoomBankSO.cs l.61-71 (combat/elite/boss/merchant/event only) |
| `IsAnyOverlayOpen` = 5-flag OR (tab/settings/skillOffer/skillCodex/pause), Director flag YOK | DOGRULANDI | UIManager.cs l.41 |
| `TryEnterDoor` -> `MarkAdvancing()` -> `AdvanceTo` -> `BuildCurrentRoom()`, INSTANT, scene-load YOK, coroutine/FX YOK | DOGRULANDI | RoomRunDirector.cs l.1801-1828 |
| `RunMapOverlay.cs` ZATEN VAR (IMGUI ekran-alti, M toggle, `director.Graph` okur) | DOGRULANDI | RunMapOverlay.cs l.1-129 (7.5KB) |
| Camera `useFixedDemoCamera=true` (l.110), l.353 fixed-sizing gate | DOGRULANDI | RoomRunDirector.cs l.110/353/429 |
| Iki `DungeonGraph.cs` (Core + MapDesigner.Room.Runtime); run-map live = MapDesigner | DOGRULANDI | Glob; RunMapOverlay namespace=MapDesigner |

---

## ORDERED MVP (CUT-LINE — playable+durust demo icin must-have)

Sira = golden-path temizligi > dusuk risk > mevcut-kod-uzantisi. Hepsi 3 gunde guvenli.

### T1 — Director-bleed gate (re-spec)
- **why:** Director overlay reward-draft uzerine tasiyor ("cok sacma"); EN yuksek gorsel deger.
- **effort:** M · **risk:** MED · **Unity:** Y · **deps:** yok
- **golden-path:** reward ekrani okunamaz hale geliyor -> demo gorsel kalitesini kirar.
- **FIX (council C2-7 ile DUZELTILDI):** Load-bearing fix = **sortingOrder 950 -> ~120** (DirectorMode.cs l.700), ClassSelectionUI=190 zaten l.2066-2073'te kismi cozum var; bu desen reward-draft yoluna genisletilir. `IsAnyOverlayOpen` poll **MIS-TARGETED** (Director flag icermez -> no-op) -> KULLANMA, ya da once Director-flag ekle. T1=mevcut yarim bleed-fix'i bitir, kucuk.
- **verify:** Director acik+reward acik screenshot (Game-view `manage_camera screenshot` overlay yakalar, G2) -> reward draft TAM okunur, Director arkada. + read_console=0.

### T4 — Chest bank wire (CODE gap, not config)
- **why:** Run-map Chest15% mix ediyor (DONE-2) ama bank Chest'i servis edemiyor -> **run-map YALAN soyluyor** (Chest dugumu combat olarak cikar veya null).
- **effort:** S->M · **risk:** LOW · **Unity:** Y · **deps:** DONE-2
- **golden-path:** sandik vaadi tutmaz = demo durustlugu kirilir.
- **FIX (council C3-G1 ile YUKSELTILDI — config DEGIL kod):** RoomBankSO.cs'e `chestRooms` field + `GetList` switch `case Chest` + `AllRooms()` + Validate ekle; SONRA `DemoRoomBank.asset` Chest girisini doldur. Yoksa `GetList(Chest)=null` -> Pick null -> sessiz combat-fallback.
- **verify:** EditMode `GetList(Chest)!=null`; PlayMode spawn-a-chest-room assert. Screenshot: oyun-ici SANDIK odasi yuklendi. + read_console=0.
- **dikkat (G9):** edit ONCESI graphify ile hangi `DungeonGraph` live dogrula (Core dead olabilir).

### T7 — Reward chip trigger+outcome (DATA-01)
- **why:** reward kartinda combo/outcome bilgisi (Loc).
- **effort:** S · **risk:** LOW · **Unity:** kismi (SkillOfferUI.cs)
- **golden-path:** kirmaz, bilgi netligi.
- **FIX (council C2-6 ile sinirlandi):** trigger+outcome **gercek ScriptableObject'lerden** oku; paketin "Earthsplitter->Gravity Cleave canonical degil" iddiasi DOGRULANMADI -> paket combo tablosunu HARD-CODE ETME.
- **verify:** EditMode Loc-key-resolves; oyun-ici chip screenshot (G8). + read_console=0.

### T6 — Run-map portal bar (M) baglama + dal-nav (SCOPE KUCULDU)
- **why:** N dugum = N portal; oyuncu rotayi gorur+secer.
- **effort:** M->LOW · **risk:** LOW · **Unity:** Y · **deps:** DONE-2 (T5 DEGIL)
- **golden-path:** kirmaz; instant-gecisle de calisir.
- **FIX (council C1 ile SCOPE KUCULDU):** `RunMapOverlay.cs` ZATEN VAR (ekran-alti IMGUI graph). T6 = bu overlay'i M'e bagla + node-tikla=`TryEnterDoor(i)`, **tablet-panel/yeni-panel SIFIRDAN DEGIL**. T5 onkosulu KALDIRILDI (yanlis bagimlilik — overlay instant-gecisle calisir; T5'i onkosul yapmak ikisini de bloklardi).
- **canon (C2-2):** dugum cubugu EKRAN-ALTI kalir; HUD polish minimap'i sag-uste tasimasin.
- **verify:** M -> 3 portal -> 3 dugum screenshot (ekran-alti). Tikla -> oda gecisi. + read_console=0.

### T2 — Golden-path full LIVE verify (en son, gate)
- **why:** input+screenshot artik calisiyor (MCP 9.7.3); T1/T4/T7/T6 sonrasi tam akis.
- **effort:** M · **risk:** LOW · **Unity:** Y · **deps:** T1,T4,T7,T6
- **golden-path:** dogrulama adimi.
- **verify:** _Arena dev-direct -> sinif sec -> combat -> reward(G-collect=DONE-1) -> M -> portal -> sonraki oda. Her beat screenshot. + read_console=0. Ambient 0.22'de okunabilirlik (G4).

**MVP CUT-LINE BURADA. Alttakiler STRETCH — golden-path verify (T2) GREEN olmadan baslama.**

---

## STRETCH (T2 GREEN sonrasi; risk sirasiyla)

### S-A (=scope-a) — Buyuk/iyi-kompoze cliff oda (KOMPOZISYON, camera DEGIL)
- **why:** "oda kucuk" hissinin GERCEK koku (C1: camera degil, oda boyutu/kompozisyon).
- **effort:** M · **risk:** MED · **Unity:** Y
- **FIX:** RIMA'da buyuk oda sablonlari ZATEN VAR -> redo etme, sec/build. T3 (camera) buna BAGLI, tek basina yapilmaz.
- **verify (G10):** buyuk `_Arena` template screenshot + nav/spawn-bounds sanity (oyuncu OOB degil, portal erisilebilir). Ambient 0.22 okunabilirlik (G4).

### T3 — Camera zoom-out (S-A'ya BAGLI, tek basina YOK)
- **risk:** MED. Camera ZATEN sabit 5.0; degeri degistirmek tum oda kompozisyonu+spawn'i bozar -> her oda re-verify.
- **FIX (C2-9):** `useFixedDemoCamera` (l.110) + l.353/445 camera owner; zoom degeri S-A buyuk-oda ile birlikte ayarlanir.

### T5 — Portal teleport coroutine (beam+zoom) — EN TEHLIKELI, sona
- **risk:** YUKSEK. Yeni-davranis + kirilgan FSM. `TryEnterDoor`->`MarkAdvancing`->`AdvanceTo`->`BuildCurrentRoom` INSTANT, in-place rebuild (scene-load YOK).
- **FIX (C2-8):** beam+zoom coroutine `AdvanceTo`'yu SARAR; async scene-load'a GUVENME (rooms in-place rebuilt). Paket "RoomFlowController/OnRoomExit/scene transition" modeli RIMA'ya UYMAZ. Portal.cs:58 `// TODO` FARKLI yol (PortalSpawnController) — `TryEnterDoor` ile KARISTIRMA.
- **FIX (C2-9):** zoom coroutine camera tasir -> `useFixedDemoCamera` lock ile CATISIR; T3 ile camera-owner koordine et.
- **verify (G7):** null-FX fallback -> direct-advance test; "beam sirasinda G-spam = double-advance YOK" (RoomTransitionFX.IsFading guard); beam-mid screenshot.

### Diger STRETCH
- **T8 LIFE-01 batch** (M/LOW): graphify ile gercek lazy-singleton tara (AttackTokenManager vb. desen VALID, C2-10), paket OnApplicationQuit template'ini KORU-KORUNE bulk-uygulama. Gameplay-kirmaz console-noise -> post-demo OK.
- **T9 SkillBar slot+font** (S/LOW): kozmetik.
- **T11 reward kart overflow** (S/LOW): SkillOfferUI sabit `sizeDelta` (l.222/476), LayoutGroup/ContentSizeFitter YOK -> paket UI-01 root-cause YANLIS (C2-4); surgical overflow, olmayan LayoutGroup ile savasma.
- **Egg re-skin** (P3, art-only): RewardPickup re-skin SADECE; `WorldRewardChoiceSet`/`RewardDefinition` SO + 7-state FSM = REJECT (C2-3/G5). 7 gorsel state = art ref OK.
- Katman-2: HUD scaler/bar, Director recolor, U1-U4/J1/F1.

---

## CANON-REJECTIONS (herhangi bir task bunu benimserse = REJECT)
- **4-cardinal S/E/N/W sprite** (C2-1) — Karar #114 = 8-dir 5 sprite + 3 flipX. AIM isi MEVCUT 8-dir facing kullanir, paketin `CardinalFacing`'i DEGIL.
- **no-flip / asimetrik-flip-edilmemeli** (C2-1) — REJECT.
- **PPU=128**, **35 derece iso-grid cellSize** — REJECT.
- **minimap sag-ust** (C2-2, HUD-01) — run-map ekran-alti ile catisir; HUD polish minimap'i re-anchor etmesin.
- **Egg WorldRewardChoiceSet/RewardDefinition SO + 7-state hatch FSM** (C2-3) — REJECT; Egg=presentation re-skin.
- **REWARD 8-state RewardSession FSM + RoomFlowController single-owner refactor** (C2-5) — REJECT; REWARD-02 zaten OnTriggerStay2D+CheckInitialPlayerOverlap ile FIXED (DONE-1).
- **DATA-01 paket combo tablosu hard-code** (C2-6) — REJECT; gercek SO oku.
- **single AimSnapshot refactor** — DEFER.

---

## ASSET-GAPS
- blue-beam VFX (P1) — reuse EchoPuffBurst + telegraph_line_beam (T5 stretch icin).
- egg sprite (P3) — re-skin.
- run-map node ikonlari — renk-placeholder yeter (P3).
- Settings/Director 9-slice (P2) — RIMA'da 9-slice pack ZATEN VAR.
- **ZATEN VAR (redo ETME):** 9-slice pack, frame'ler, node-set, reward chrome, ~85 icon, buyuk oda sablonlari.

---

## SCREENSHOT PLAN (hoca raporu icin — method: Game-view `manage_camera screenshot`, overlay yakalar)
| Ne | Ne zaman | Amac |
|---|---|---|
| Director acik + reward draft | T1 sonrasi (before/after) | bleed cozuldu kaniti |
| Oyun-ici SANDIK odasi | T4 sonrasi | run-map durustlugu |
| Reward chip (trigger+outcome) | T7 sonrasi | DATA-01 |
| Ekran-alti portal bar (3 dugum=3 portal) | T6 sonrasi | run-map UX |
| Tam golden-path beat-by-beat | T2 | playable kaniti |
| Buyuk cliff oda (wide) + ambient 0.22 okunabilirlik | S-A/T3 sonrasi | kompozisyon (G4/G10) |
| Beam mid-transition | T5 sonrasi (stretch) | portal feel |
| SkillBar / reward kart / overflow | T9/T11 (stretch) | polish |

---

## COUNCIL CONCERNS — RESOLUTION OZETI
- **C1 (DEMO-RISK):** T6 scope kuculdu (RunMapOverlay var); T5/T3 DEFER->STRETCH (en tehlikeli); T5 dep T3 DEGIL FSM; T6 dep T5 KALDIRILDI. CUT-LINE: T1->T4->T7->T6->T2. ✓
- **C2 (CANON):** 4-cardinal/no-flip/minimap-top/Egg-SO/REWARD-FSM/combo-table = REJECT listesinde. T1 re-spec (sortingOrder load-bearing, IsAnyOverlayOpen mis-target). T5 in-place-rebuild + camera-owner koordinasyon. ✓
- **C3 (COMPLETENESS):** T4 CODE-gap (chestRooms field+switch); her item screenshot+read_console=0 gate; ambient-0.22 okunabilirlik test (G4); G9 DungeonGraph graphify-confirm; T5 fail-test (null-FX fallback + double-advance guard); S-A acceptance test eklendi. ✓
- **DEFER gerekce:** T5/T3/S-A = yeni-davranis/kompozisyon, golden-path PLAYABLE+DURUST (T1/T4/T7/T6/T2) once; 3 gunde once must-have, sonra polish.

---
*Dosya: STAGING/DEMO_MASTER_PLAN_2026-06-16.md (LIVE decision doc)*
