# ROOM_TRANSITIONS — Review + Wire Pass

**Note:** Sonnet sub-agent dispatch tool slot unavailable in this orchestrator
context (Task tool blocked). Opus orchestrator (this subagent) performed
review inline per `feedback_code_writer_rotation` spirit — yazan (Codex) ≠
review eden (Opus orchestrator, design rolünden ayrı code-review hat).

Code-review **DONE**, scene wire + 5 SO fill + smoke test **DEFERRED** to
next session pickup (Unity Editor şu an erişilebilir değil, MCP wire pass'i
canlı Unity gerektirir).

---

## 1. Code Review Verdict: PASS WITH MINOR NOTES

### 1.1 RoomLoader.cs (10468 bytes, ~280 LOC) — PASS

| Kontrol | Sonuç |
|---|---|
| LoadNext static → instance pattern | PASS — FindFirstObjectByType<RoomLoader> + null guard |
| CurrentRoomIndex static state | PASS — Public get / private set / default 0 |
| Boundary check (final room) | PASS — Line 89-93, LogWarning + return |
| Player freeze | PASS — Line 98-99, PlayerController.enabled = false |
| Fade integration | PASS — Line 101-108, RoomTransitionFX.DoTransition(callback) + null fallback |
| SwapRoomWhileBlack | PASS — Teardown → teleport → build → index++ → OnRoomChanged event |
| Rigidbody2D teleport | PASS — Line 166-168, rb.position primary, transform.position fallback |
| Mob death counter → OnRoomCleared | PASS — Line 189-202, closure capture remainingMobs + roomClearedRaised guard idempotent |
| Reward room auto-trigger | PASS — Line 238-247, 2s delay → MapFragmentSpawner.SendMessage HandleRoomCleared |
| Boss death listener | PASS — Line 249-258, GetComponentInChildren<Health> + OnDeath.AddListener(RaiseDemoComplete) |
| TeardownCurrentRoom defensive | MINOR — FindObjectsByType<Gate>/MapFragment global scope, sahnedeki Faz 1 dışı Gate/MapFragment'leri de siler |
| ReenableAfterFade timing | PASS — WaitUntil IsFading == false |
| HUD oda counter | PASS — Line 267-271, "Room N/5 — DisplayName" format |

**MINOR concern (defansif teardown):** Line 150-158 `foreach Gate in FindObjectsByType<Gate>(All)` ve aynı MapFragment için. Eğer sahnede Faz 1 sequence dışı bir Gate veya MapFragment varsa (örnek: scene authoring sırasında pre-placed reference instance) onları da siler. **Mitigation:** Faz 1 demo sahnesi temiz tutulursa risk yok. Faz 2'de scope artarsa `_currentRoomContent` child filter ile kısıtla. **VERDICT:** Track A scope için kabul, log at when triggered.

**Minor (null-init defensive code):** Line 191, 256 `health.OnDeath = new UnityEvent()` Codex defensive — Health.Awake `??=` zaten init eder. Belt-and-suspenders, zarar yok.

### 1.2 RoomSequenceData.cs (~50 LOC) — PASS

| Kontrol | Sonuç |
|---|---|
| CreateAssetMenu path | PASS — "RIMA/Phase1/Room Sequence Data" |
| Namespace | PASS — RIMA.Systems.Map |
| Field schema | PASS — Design doc §4 birebir |
| EnemySpawnEntry nested | PASS — [Serializable] inner class |
| Headers Inspector friendly | PASS — Identity/Player/Mob/Gate/Focal/Cliff/Misc/Fragment groupings |

**Minor:** `Vector2 gateSize` default sıfır vector ise BuildRoomContent line 216 `(1.5f, 2f)` fallback'i kuruyor — designer Inspector'da boş bırakırsa OK.

### 1.3 DemoCompleteOverlay.cs (~90 LOC) — PASS

| Kontrol | Sonuç |
|---|---|
| Static Show() | PASS — Idempotent (FindFirstObjectByType existing check line 16) |
| RuntimeInitializeOnLoadMethod hook | PASS — AfterSceneLoad, idempotent (-= then +=) |
| Canvas overlay sortingOrder=200 | PASS — Above RoomTransitionFX (100) |
| GraphicRaycaster eklendi | PASS — Restart button tıklanabilir |
| Cyan (#00FFCC) text | PASS — Color(0, 1, 0.8, 1) |
| Restart button | PASS — SceneManager.LoadScene(current build index) |
| Font fallback | PASS — LegacyRuntime.ttf builtin |

### 1.4 RuntimeRoomManager.cs (1-line edit) — PASS

Line 548: `RIMA.Systems.Map.RoomLoader.RaiseDemoComplete();` eklendi —
NotifyBossDefeated içinde. Tek satır, scope respect.

---

## 2. Compile + Validate

Codex raporu:
- `refresh_unity scope=all mode=force compile=request` — Unity idle return
- `read_console types=error count=50` — **0 entries**
- `validate_script` standard — RoomLoader / RoomSequenceData / DemoCompleteOverlay / RuntimeRoomManager **0 errors / 0 warnings**

**STATUS:** PASS

---

## 3. BLOCKED flag analysis (Codex raised)

**Codex BLOCKED note:** `RuntimeRoomManager.Start` `currentManifest == null && worldBuilder == null` koşulunda legacy `StartRoom()` (line 144-186, currentRoomIndex=0 + StartRoom) auto-trigger eder. Faz 1 RoomLoader-driven path ile çakışma riski.

**Opus orchestrator verdict:** KABUL — scope dışı kabul, ama wire pass'inde scene-level mitigation gerek:

**Mitigation options:**
1. **PlayableArena_Test01.unity scene'inde RuntimeRoomManager.enabled = false** — Awake çalışır (Instance set, duplicate guard). Start'ı durduramaz çünkü enabled=false yine Start çağrılmaz mı? Unity behavior: enabled=false ise Start() **çağrılır** (component disabled olduğunda Start hâlâ çalışır birinci kez, sonra Update gibi enabled-gated callback'ler skip edilir). **Yetersiz mitigation.**
2. **Scene'de RuntimeRoomManager GO sil** — Faz 1 demo path sadece RoomLoader kullanır. NotifyBossDefeated hook redundant olur (RuntimeRoomManager olmayınca çağrılmaz) ama RoomLoader.WireBossDeathListener line 257 zaten `bossHealth.OnDeath.AddListener(RaiseDemoComplete)` yapıyor — boss death → demo end safe. **ÖNERIIM.**
3. **GameObject SetActive(false)** — Awake da Start da çalışmaz. Effective, GO silmeden disable mümkün. **Alternatif option.**

**Önerim:** Option 3 (RuntimeRoomManager GO SetActive(false) PlayableArena_Test01.unity'de Faz 1 demo için). Geri açmak Faz 2'de kolay.

---

## 4. Scene Wire — DEFERRED (manual veya future Sonnet dispatch)

### 4.1 PlayableArena_Test01.unity wire steps

1. **Systems root** altında **RoomLoader GameObject** create (varsa skip):
   - Add component: `RIMA.Systems.Map.RoomLoader`
   - Inspector `_sequence` field: 5 SO slot

2. **5 SO create:** `Assets/ScriptableObjects/Rooms/Phase1_Room{1..5}.asset`
   (CreateAssetMenu yolu: `RIMA/Phase1/Room Sequence Data`).

3. **5 SO doldur (table):**

| Room | Idx | DisplayName | playerStartPos | mobSpawns[] | gatePos | focalElement | isReward | isBoss |
|---|---|---|---|---|---|---|---|---|
| Phase1_Room1 | 0 | "Tutorial Combat" | (0, -3.5, 0) | 3x FractureImp at (-4, 3, 0)/(4, 3, 0)/(0, 3, 0) | (0, 3.5, 0) | null (or cyan rune sprite at 0,0,0) | false | false |
| Phase1_Room2 | 1 | "Combat Medium" | (0, 35.5, 0) | 3x FractureImp (NW/NE/N edges, around Y=40) + 2x FractureImp (W/E mid edges) | (0, 44.5, 0) | null | false | false |
| Phase1_Room3 | 2 | "Combat Hard L-shape" | (0, 75.5, 0) | 4x FractureImp + 2x FractureImp + 1x FractureImp (isElite=true Hulk placeholder) edge pockets around Y=80 | (0, 84.5, 0) | null | false | false |
| Phase1_Room4 | 3 | "Vestibule Reward" | (0, 115, 0) | empty | (0, 125, 0) | null (or plinth placeholder) | **true** | false |
| Phase1_Room5 | 4 | "Boss Arena" | (0, 150, 0) | 1x PenitentSovereign at (0, 160, 0) | (0, 0, 0) // no exit gate | null (or 4 chain anchor placeholders) | false | **true** |

   **Prefab refs (confirmed via Glob):**
   - FractureImp: `Assets/Prefabs/Enemies/FractureImp.prefab`
   - PenitentSovereign: `Assets/Prefabs/Enemies/Boss/PenitentSovereign.prefab`
   - ShardWalker / HollowHulk YOK → FractureImp 1:1 placeholder

4. **RoomLoader._sequence array:** 5 SO Phase1_Room1..5.asset drag-drop.

5. **RuntimeRoomManager GO SetActive(false)** — Faz 1 demo wire mitigation (yukarı §3 Option 3).

6. **MapFragmentBridge.useFragmentGateFlow = true** zaten D2.5'te set'lendi mi confirm.

7. **MapFragmentSpawner LIVE** confirm — RoomLoader.OnRoomCleared subscription LIVE (D2.5).

8. **RoomLoader.LoadFirstRoom()** trigger — Start hook veya manual Awake'te çağrılır. Şu an Codex impl'inde **auto-call YOK**. **YENI ITEM:** Sahnede `RoomLoader.LoadFirstRoom()` çağıracak bir Bootstrap component gerek, veya `Start()` override edip auto-call yap. **BU EKSIK — orchestrator notu §5.**

### 4.2 Smoke test

`manage_editor mode=play` 30-60 saniye, sonra stop. Sample beklenen log:
- `[RoomLoader] Missing room sequence entry...` YOK olmalı
- `Room 1/5 — Tutorial Combat` HUD'da visible

---

## 5. ITEM EKSIK — LoadFirstRoom auto-call

Codex impl'inde `RoomLoader.LoadFirstRoom()` public method var ama **otomatik
çağrılmıyor**. Sahnede Bootstrap component veya RoomLoader.Start() override
gerek.

**Mitigation:** RoomLoader.cs'e küçük Start override ekle (~5 LOC):

```csharp
private void Start()
{
    if (_sequence != null && _sequence.Length > 0)
    {
        LoadFirstRoom();
    }
}
```

Bu Codex scope sonrası micro-edit, surgical. Sonnet wire pass dispatch'inde
veya next session pickup'ta yapılır.

---

## 6. SONNET REVIEW + WIRE REPORT (Opus inline review summary)

**Code review verdict:** PASS — Codex impl design doc §5 outline'a birebir,
0 compile error, 0 warning. 2 MINOR concern (defansif teardown global Gate
silme, defensive null-init redundant), production-safe.

**Eksik items (next session pickup için):**
1. 5 ScriptableObject .asset create + populate (FractureImp + PenitentSovereign refs)
2. PlayableArena_Test01.unity scene'de RoomLoader GO ekle + RuntimeRoomManager GO SetActive(false)
3. RoomLoader.LoadFirstRoom() auto-call (Start override ~5 LOC veya scene Bootstrap)
4. 30s smoke test PlayMode

**BLOCKED carryover:** RuntimeRoomManager.Start legacy path scene-level mitigation (Option 3 GO SetActive false).

**Total LOC added:** RoomLoader.cs ~225 new LOC + RoomSequenceData.cs 47 LOC + DemoCompleteOverlay.cs 93 LOC + RuntimeRoomManager.cs 1 line = ~366 LOC.

**Verify durumu:** Compile PASS, runtime smoke test PENDING (Unity kapalı).
