# ROOM_TRANSITIONS — Opus Design Pass

**Author:** Opus (rima-design / orchestrator-subagent)
**Date:** 2026-05-27 gece
**Scope:** Sub-room state machine + 5-oda Faz 1 demo loop runtime impl design
**Cross-track:** Cliff floating feel (D5.6) ve T3 standalone editor tool çakışmaz
**Lock refs:** Karar #149 (runtime sub-room), `STAGING/room_layout_phase1_demo.md` (5-oda spec)

---

## 1. Sorun + kapsam

Kullanıcının ilk istediği gameplay loop:
> "Odaları birbirine bağla sanal bi kapı yap içinden geçe geçe odaları gezmek istiyorum karakterimle"

`RoomLoader.LoadNext()` Day 2 itibarıyla STUB (`Assets/Scripts/Systems/Map/RoomLoader.cs:47-50`,
sadece Debug.Log). Day 2-2.5 boyunca Fragment + Gate + DraftManager + Bridge zinciri
LIVE; eksik tek halka **LoadNext gövdesi + 5-oda sequence data**.

Bu doc Opus'un design karar setini verir; Codex bu spec'le yazar, Sonnet review eder.

---

## 2. Karar matrisi (Opus judgment)

| # | Karar | Seçim | Gerekçe |
|---|---|---|---|
| 2.1 | Layout strategy | **Y offset teleport (tek-build)** | Pre-built 5-oda runtime instantiate'i hidden child + SetActive flip değil, mevcut PlayableArena_Test01 prototip Room 2'yi koruyup *content swap* model. Karar #149 single-scene LOCK; Y-offset hızlı prototype, Track A "functional minimum" tutar. |
| 2.2 | Room sequence storage | **ScriptableObject 5 asset (RoomSequenceData)** | `Assets/ScriptableObjects/Rooms/Phase1_Room{1..5}.asset`. Designer-editable, JSON serializer'a gerek yok (T3 scope). EncounterTemplateSO yapısını yansıt ama Faz 1 demo'ya özel sade alan seti. |
| 2.3 | Sub-room state machine | **Combat → FragmentSpawned → DraftPending → DraftPicked → GateUnlocked → Transitioning → NextLoaded** | Mevcut Day 2.5 flow (`MapFragmentSpawner` + `MapFragmentBridge`) zaten bu state'leri implicit kullanıyor. RoomLoader.LoadNext sadece "Transitioning" tetikler, geri kalan event-driven. Yeni explicit `RoomLoader.CurrentRoomIndex` (int 0-4) eklenir. |
| 2.4 | Fade timing | **FadeOut 0.3s + 0.15s hold + FadeIn 0.3s = 0.75s total** | RoomTransitionFX.cs Inspector default'ları zaten 0.25/0.15/0.35 — `DoTransition(onBlack)` callback'i kullan, yeniden timing inventionu YAPMA. Indie sweet spot 0.6-0.9s aralığı. Faz 1 0.3/0.15/0.3 = "snappy ama dramatik" — Sang Hendrix Hades reference. |
| 2.5 | Player teleport | **Rigidbody2D.position direct set + Camera follow auto-track** | Rb2D Dynamic ise position set physics-safe (FixedUpdate'te clamp olmaz çünkü fade sırasında player kontrolü zaten kapalı — see 2.6). transform.position değil rb.position kullan — Unity 2023 capsule collider sync'i bunu gerektirir. CameraFollow LIVE, target yeniden assign'a gerek yok, player aynı GameObject. |
| 2.6 | Player input freeze | **PlayerController.enabled = false → fade → teleport → fade → enabled = true** | Aynı pattern RuntimeRoomManager.TransitionToRoomRoutine'de (line 252-287) zaten kullanılıyor. PlayerMovementController [Obsolete] (S112 cleanup) — sadece PlayerController dokun. |
| 2.7 | Mob despawn previous room | **List<GameObject> activeEnemies.ClearAll() → Destroy** | RuntimeRoomManager.ClearActiveEnemies LIVE (pattern var). Bu RoomLoader scope'unda değil — RuntimeRoomManager.OnEncounterFinalCleared zaten temizliyor; ama bizim Faz 1 demo flow `RuntimeRoomManager` encounter pipeline'ı bypass ederse direct teardown gerekir. **Karar:** RoomLoader.LoadNext fade-out callback'inde `RuntimeRoomManager.Instance?.ClearAllActiveEnemiesForPhase1Transition()` public helper'a ihtiyaç var — bu RuntimeRoomManager extend kapsamı (~20 LOC) altında. |
| 2.8 | Mob spawn new room | **Instant batch (Faz 1)** | Staggered spawn (0.3s delay) RuntimeRoomManager.SpawnEnemies LIVE — ama Faz 1 demo'da Y offset teleport sonrası "snap into combat" hissi için instant. Wave 2 trigger Faz 2 scope. RoomSequenceData.mobSpawnConfig = `EnemySpawnEntry[] { prefab, position, isElite }`. |
| 2.9 | Gate destroy + spawn | **Previous Gate.Destroy → next Gate spawn (RoomSequenceData.gatePosition)** | Day 2 Gate pattern (`Assets/Scripts/Environment/Gate.cs`) reuse. Gate prefab gerekirse yoksa runtime `new GameObject + AddComponent<Gate>` (Gate.cs hot-creatable, Awake'te SR/Collider auto). |
| 2.10 | Camera follow target update | **Player aynı GO → noop** | Y offset teleport modelinde Camera target hâlâ Player. CameraFollow.SetTarget gerekmez. PixelPerfectCamera snap fade sırasında zaten gizli — black screen. |
| 2.11 | Mob spawn order | **Instant batch (tüm spawn'lar Tek Update)** | Faz 1 simplicity. Wave/stagger Faz 2 EncounterTemplateSO ile gelecek. |
| 2.12 | Room 5 boss demo end | **NotifyBossDefeated → DemoCompleteOverlay GO instantiate (full-screen black + "DEMO COMPLETE" text + Restart button)** | RuntimeRoomManager.NotifyBossDefeated LIVE (line 543-548) ama placeholder Debug.Log. Faz 1 spec: %50 HP death cinematic = demo end. Restart = SceneManager.LoadScene(current scene) → Room 0'dan başlar. |
| 2.13 | HUD oda counter | **HUDController.SetRoomStatus("Room N/5") FadeIn sonrası tetiklenir** | HUDController.SetRoomStatus LIVE (RuntimeRoomManager kullanıyor). Fade in callback'inde çağır. |

---

## 3. State machine — sub-room lifecycle

```
[Idle]
  ↓ RoomLoader.LoadFirstRoom() (Start hook)
[RoomActive: index=N]
  ↓ all enemies dead
  ↓ RoomLoader.OnRoomCleared event fire
[FragmentSpawned]
  ← MapFragmentSpawner listens, instantiates MapFragment at FragmentDropAnchor
  ↓ Player G-key in 2.5u radius
[DraftPending]
  ← MapFragmentBridge.HandleAnyFragmentPickedUp → DraftManager.TriggerDraftFromFragment
  ↓ DraftManager.OnSkillPicked event
[DraftPicked]
  ← MapFragmentBridge.HandleSkillPicked → UnlockAllAwaitingGates() → gate.Unlock()
[GateUnlocked]
  ↓ Player walks into gate trigger
  ← Gate.OnPlayerEntered → MapFragmentBridge.HandleGateEntered → RoomLoader.LoadNext()
[Transitioning] ← NEW (LoadNext impl)
  → currentRoomIndex++
  → if currentRoomIndex >= 5: DemoComplete (Room 5 boss kill ayrı path)
  → RoomTransitionFX.DoTransition(onBlack):
       onBlack callback:
         1. PlayerController.enabled = false
         2. Teardown current room content (mob despawn, gate destroy, fragment cleanup)
         3. Player teleport: rb.position = nextRoom.playerStartPos
         4. Build next room content (mob spawn batch, gate spawn, focal element)
         5. (Fade-in starts auto after callback)
       fade-in completion → re-enable PlayerController, HUD "Room N/5"
[RoomActive: index=N+1]
```

**Room 5 özel path:** Boss kill → `RuntimeRoomManager.NotifyBossDefeated()` →
`RoomLoader.OnDemoComplete()` → fade out → DemoCompleteOverlay + restart button.
Restart button → `SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)`.

---

## 4. RoomSequenceData ScriptableObject schema

```csharp
[CreateAssetMenu(fileName = "Phase1_Room", menuName = "RIMA/Phase1/Room Sequence Data")]
public class RoomSequenceData : ScriptableObject
{
    [Header("Identity")]
    public int roomIndex;                // 0-4
    public string displayName;           // "Room 1 — Tutorial Combat"

    [Header("Player")]
    public Vector3 playerStartPos;       // World position, relative to scene origin

    [Header("Mob Spawn")]
    public EnemySpawnEntry[] mobSpawns;  // Instant batch spawn at LoadNext

    [Header("Gate")]
    public Vector3 gatePosition;         // World position, exit gate (locked-until-cleared)
    public Vector2 gateSize;             // BoxCollider2D size (default 1.5, 2.0)

    [Header("Focal Element")]
    public GameObject focalElementPrefab; // Optional — cyan rune / brazier / column / plinth / chain anchor
    public Vector3 focalElementPos;

    [Header("Cliff Pattern (optional)")]
    public string cliffPatternKey;       // Empty for Faz 1; cliff swap Faz 2 scope

    [Header("Misc")]
    public bool isBossRoom;              // True for Room 5
    public bool isRewardRoom;            // True for Room 4 Vestibule (skip mob spawn)
    public float expectedDuration;       // For HUD pacing analytics, optional

    [Header("Fragment Drop")]
    public Vector3 fragmentDropOverride; // If non-zero, override default (mob death point)
}

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject prefab;
    public Vector3 position;
    public bool isElite;
}
```

5 asset:
- `Phase1_Room1.asset` — Tutorial Combat (3 FractureImp, cyan rune focal, Y=0)
- `Phase1_Room2.asset` — Combat Medium (3 Imp + 2 Walker, brazier focal, Y=40)
- `Phase1_Room3.asset` — Combat Hard L-shape (4 Imp + 2 Walker + 1 Hulk elite, broken column focal, Y=80)
- `Phase1_Room4.asset` — Vestibule Reward (mob YOK — isRewardRoom=true, plinth focal, Y=120). Day 2.5 MapFragmentSpawner pattern reuse — bu odada fragment plinth interactable olabilir veya direct draft trigger (Track A: direct draft on player enter trigger, plinth opsiyonel visual).
- `Phase1_Room5.asset` — Boss Arena (PenitentSovereign + 4 chain anchor, isBossRoom=true, Y=160)

---

## 5. RoomLoader.LoadNext() impl outline (Codex'e brief)

```csharp
public static int CurrentRoomIndex { get; private set; } = 0;
public static event Action<int> OnRoomChanged;
public static event Action OnDemoComplete;

public static void LoadNext()
{
    var loader = FindFirstObjectByType<RoomLoader>();
    if (loader == null) { Debug.LogError("[RoomLoader] No instance in scene."); return; }
    loader.LoadNextInstance();
}

private RoomSequenceData[] _sequence;       // Inspector array, 5 SO refs
private GameObject _currentRoomContent;     // Spawned mobs/gate/focal parent

private void LoadNextInstance()
{
    if (CurrentRoomIndex >= _sequence.Length - 1)
    {
        // Already on last room — Room 5 boss kill should trigger DemoComplete, not LoadNext
        Debug.LogWarning("[RoomLoader] LoadNext called on final room — ignoring.");
        return;
    }

    int nextIndex = CurrentRoomIndex + 1;
    RoomSequenceData nextData = _sequence[nextIndex];

    // Freeze player
    PlayerController pc = FindFirstObjectByType<PlayerController>();
    if (pc != null) pc.enabled = false;

    if (RoomTransitionFX.Instance != null)
    {
        RoomTransitionFX.Instance.DoTransition(() => SwapRoomWhileBlack(nextIndex, nextData));
    }
    else
    {
        SwapRoomWhileBlack(nextIndex, nextData);
    }

    StartCoroutine(ReenableAfterFade(pc, nextData));
}

private void SwapRoomWhileBlack(int nextIndex, RoomSequenceData nextData)
{
    // Teardown
    TeardownCurrentRoom();

    // Teleport player
    var player = GameObject.FindGameObjectWithTag("Player");
    if (player != null)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.position = nextData.playerStartPos;
        else            player.transform.position = nextData.playerStartPos;
    }

    // Build new room content
    BuildRoomContent(nextData);

    CurrentRoomIndex = nextIndex;
    OnRoomChanged?.Invoke(nextIndex);
}

private void TeardownCurrentRoom()
{
    if (_currentRoomContent != null)
    {
        Destroy(_currentRoomContent);
        _currentRoomContent = null;
    }

    // Also nuke any stray Gate / MapFragment children (defensive — Bridge subs may dangle)
    foreach (var gate in FindObjectsByType<Gate>(FindObjectsSortMode.None))
        Destroy(gate.gameObject);
    foreach (var frag in FindObjectsByType<MapFragment>(FindObjectsSortMode.None))
        Destroy(frag.gameObject);
}

private void BuildRoomContent(RoomSequenceData data)
{
    _currentRoomContent = new GameObject($"RoomContent_{data.roomIndex}_{data.displayName}");

    // Spawn mobs (skip if reward room)
    if (!data.isRewardRoom && data.mobSpawns != null)
    {
        foreach (var entry in data.mobSpawns)
        {
            if (entry.prefab == null) continue;
            var mob = Instantiate(entry.prefab, entry.position, Quaternion.identity, _currentRoomContent.transform);
            if (entry.isElite) EliteAffix.Apply(mob, EliteAffix.RandomAffix());
        }
    }

    // Spawn focal element
    if (data.focalElementPrefab != null)
    {
        Instantiate(data.focalElementPrefab, data.focalElementPos, Quaternion.identity, _currentRoomContent.transform);
    }

    // Spawn gate (state = AwaitingFragment, fragment drop triggers unlock chain)
    var gateGO = new GameObject($"Gate_Room{data.roomIndex}_Exit");
    gateGO.transform.position = data.gatePosition;
    gateGO.transform.SetParent(_currentRoomContent.transform);
    var sr = gateGO.AddComponent<SpriteRenderer>();
    var col = gateGO.AddComponent<BoxCollider2D>();
    col.size = data.gateSize == Vector2.zero ? new Vector2(1.5f, 2f) : data.gateSize;
    gateGO.AddComponent<Gate>(); // Awake sets sprite/collider; default state = AwaitingFragment

    // FragmentDropAnchor at room center (mob death triggers spawn via Spawner)
    if (data.fragmentDropOverride != Vector3.zero)
    {
        var anchorGO = new GameObject("FragmentDropAnchor");
        anchorGO.transform.position = data.fragmentDropOverride;
        anchorGO.transform.SetParent(_currentRoomContent.transform);
        anchorGO.AddComponent<FragmentDropAnchor>();
    }

    // Reward room: no mob = no auto-clear → manually trigger draft chain (Faz 1 shortcut)
    if (data.isRewardRoom)
    {
        StartCoroutine(RewardRoomAutoTrigger(data));
    }

    // Boss room: spawn boss, hook NotifyBossDefeated → OnDemoComplete
    if (data.isBossRoom)
    {
        // Boss prefab is one of mobSpawns entries (Faz 1 simplicity)
        // Hook Health.OnDeath to fire OnDemoComplete
        StartCoroutine(WireBossDeathListener(_currentRoomContent));
    }
}

private IEnumerator RewardRoomAutoTrigger(RoomSequenceData data)
{
    // Wait until player has stepped into room (3 sec budget to walk in)
    yield return new WaitForSeconds(2f);
    // Directly spawn fragment so player can pick up + draft + advance
    var anchor = FindFirstObjectByType<FragmentDropAnchor>();
    if (anchor != null)
    {
        var spawner = FindFirstObjectByType<MapFragmentSpawner>();
        spawner?.SendMessage("HandleRoomCleared", SendMessageOptions.DontRequireReceiver);
    }
}

private IEnumerator WireBossDeathListener(GameObject roomContent)
{
    yield return null; // 1 frame so Health components present
    var bossHealth = roomContent.GetComponentInChildren<Health>();
    if (bossHealth != null)
    {
        bossHealth.OnDeath.AddListener(() => OnDemoComplete?.Invoke());
    }
}

private IEnumerator ReenableAfterFade(PlayerController pc, RoomSequenceData data)
{
    // Wait for RoomTransitionFX.IsFading == false
    yield return new WaitUntil(() =>
        RoomTransitionFX.Instance == null || !RoomTransitionFX.Instance.IsFading);
    if (pc != null) pc.enabled = true;

    var hud = FindFirstObjectByType<HUDController>();
    hud?.SetRoomStatus($"Room {data.roomIndex + 1}/5 — {data.displayName}");
}
```

~150 LOC. RuntimeRoomManager genişlemesi minimal — sadece ClearActiveEnemies'in
public expose'una ihtiyaç olabilir (Faz 1 demo bypass path), ama yukarıdaki impl
TeardownCurrentRoom kendi root GO'sunu destroy ederek bunu bypass eder.

---

## 6. Demo Complete overlay (Room 5 sonu)

Yeni `Assets/Scripts/Core/DemoCompleteOverlay.cs` (~50 LOC):

```csharp
public class DemoCompleteOverlay : MonoBehaviour
{
    private Canvas _canvas;
    private Text _text;
    private Button _restartButton;

    public static void Show()
    {
        var go = new GameObject("DemoCompleteOverlay");
        go.AddComponent<DemoCompleteOverlay>().Build();
    }

    private void Build()
    {
        _canvas = gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 200; // above RoomTransitionFX (100)

        var bg = new GameObject("BG");
        bg.transform.SetParent(transform, false);
        var bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0, 0, 0, 0.85f);
        var rt = bg.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(transform, false);
        _text = textGO.AddComponent<Text>();
        _text.text = "DEMO COMPLETE\n\n5 oda, ~10 dakika";
        _text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        _text.fontSize = 48;
        _text.alignment = TextAnchor.MiddleCenter;
        _text.color = new Color(0f, 1f, 0.8f, 1f); // cyan #00FFCC
        var textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0.2f, 0.4f); textRT.anchorMax = new Vector2(0.8f, 0.7f);
        textRT.offsetMin = Vector2.zero; textRT.offsetMax = Vector2.zero;

        var btnGO = new GameObject("RestartButton");
        btnGO.transform.SetParent(transform, false);
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        _restartButton = btnGO.AddComponent<Button>();
        _restartButton.onClick.AddListener(Restart);
        var btnRT = btnGO.GetComponent<RectTransform>();
        btnRT.anchorMin = new Vector2(0.4f, 0.25f); btnRT.anchorMax = new Vector2(0.6f, 0.32f);
        btnRT.offsetMin = Vector2.zero; btnRT.offsetMax = Vector2.zero;

        var btnText = new GameObject("BtnText");
        btnText.transform.SetParent(btnGO.transform, false);
        var btnTxt = btnText.AddComponent<Text>();
        btnTxt.text = "RESTART (Room 1)";
        btnTxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnTxt.fontSize = 18;
        btnTxt.alignment = TextAnchor.MiddleCenter;
        btnTxt.color = Color.white;
        var btTRT = btnText.GetComponent<RectTransform>();
        btTRT.anchorMin = Vector2.zero; btTRT.anchorMax = Vector2.one;
        btTRT.offsetMin = Vector2.zero; btTRT.offsetMax = Vector2.zero;
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
```

RoomLoader.OnDemoComplete event → DemoCompleteOverlay.Show().

---

## 7. RuntimeRoomManager touch (minimal)

RuntimeRoomManager.cs 1434 LOC — Faz 1 demo path **bypass eder** (StartRoom legacy
random spawn modu). Yeni Faz 1 path RoomLoader-driven, RuntimeRoomManager hiç
çağrılmaz. Tek değişiklik: NotifyBossDefeated → RoomLoader.OnDemoComplete sinyali
fire etmek için single-line ekle:

```csharp
// RuntimeRoomManager.cs:543-548 NotifyBossDefeated
public void NotifyBossDefeated()
{
    Debug.Log("[RuntimeRoomManager] Boss defeated! Class selection will trigger.");
    RoomLoader.RaiseDemoComplete(); // ← NEW Faz 1 hook
}
```

RoomLoader.cs'e ekle:
```csharp
public static void RaiseDemoComplete() => OnDemoComplete?.Invoke();
```

Bu, Boss death listener wire'ı yedek path olarak korur — RuntimeRoomManager
boss spawn pipeline ile RoomLoader RoomSequenceData boss spawn pipeline iki
yoldan biri çalışacak, dual entry safe.

---

## 8. Scene wire (Sonnet'in görevi)

`Assets/Scenes/Test/PlayableArena_Test01.unity`:

1. **RoomLoader GameObject** — Systems root altında. RoomLoader component +
   RoomSequenceData[5] inspector array atanır.
2. **Phase1_Room1..5.asset** create — Sonnet 5 SO doldurur, mob prefab refs
   (FractureImp_Playtest LIVE), playerStartPos Y=0/40/80/120/160, gatePosition
   her oda N kenarda (örn Room1: Y=3.5; Room2: Y=43.5 vs).
3. **MapFragmentBridge.useFragmentGateFlow = true** zaten Day 2.5'te wire'lı
   olmalı — confirm.
4. **FractureImp_Playtest prefab refs** Room1/2/3 SO mobSpawns'a atanır.
   ShardWalker/HollowHulk YOK ise FractureImp ile placeholder doldurulur
   (count 1:1 substitute, total threat point spec'ten uzaklaşır ama Track A
   demo loop fonksiyonelliği yeterli).
5. **PenitentSovereign prefab** Room5'e atanır (LIVE), Health.OnDeath → demo end.
6. **DemoCompleteOverlay** scene'de pre-built **DEĞIL** — runtime spawn
   (Show() static method).

---

## 9. Yasaklar (HARD respect)

- Multi-scene LOCK ihlal etme (Karar #149)
- Yeni RoomTransitionFX yazma — LIVE class, sadece DoTransition/FadeOut/FadeIn
  consume et
- Cliff sistem dokunma (D5.6 paralel)
- T3 standalone editor tool tarafına geçme
- PlayerController değişiklik (D2 fix kapsamı kapalı)
- PlayerMovementController dokunma ([Obsolete], S112 cleanup)

---

## 10. BLOCKED criteria

- RuntimeRoomManager bypass path RoomLoader-driven runtime test'inde
  RoomLoader.OnRoomLoaded event chain hâlâ doğru tetiklenir mi? Eğer
  RuntimeRoomManager.Awake/Start hâlâ Room 0 spawn pipeline'ı çalıştırır
  ve RoomLoader Faz 1 path ile çakışırsa → BLOCKED, RuntimeRoomManager.cs'de
  Faz 1 mode flag eklemek gerek.
- MapFragmentSpawner Faz 1 flow'da hâlâ RoomLoader.OnRoomCleared listener.
  RoomLoader Faz 1 path **OnRoomCleared event fire etmeli** mob ölünce
  (manual mob death listener), yoksa fragment spawn olmaz.

---

## Özet (1 paragraf)

Tek scene, Y-offset teleport, 5 RoomSequenceData SO, RoomTransitionFX fade
0.3/0.15/0.3, PlayerController freeze + rb.position teleport + camera auto-track,
mob/gate/focal instant batch spawn under per-room parent GO, Gate state machine
LIVE reuse (AwaitingFragment → Unlocked → OnPlayerEntered → LoadNext), Room 5
boss death → RoomLoader.OnDemoComplete → DemoCompleteOverlay + restart button.
RuntimeRoomManager bypass minimal, sadece NotifyBossDefeated hook eklenir.
Codex bu spec'le LoadNext + 5 SO + DemoCompleteOverlay yazar; Sonnet scene
wire + smoke test yapar.
