# ROOM_TRANSITIONS — Codex Write Task

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
RoomLoader.LoadNext() impl + RoomSequenceData SO + DemoCompleteOverlay. Opus design
karar matrisi `STAGING/ROOM_TRANSITIONS_DESIGN.md`'de — kati. Triple-AI rotation:
Sen kodu yazıyorsun, Sonnet review eder.

## Pre-read (zorunlu, sırayla)
1. `STAGING/ROOM_TRANSITIONS_DESIGN.md` — Opus karar matrisi + state machine + impl outline
2. `Assets/Scripts/Systems/Map/RoomLoader.cs` — current stub
3. `Assets/Scripts/Core/RoomTransitionFX.cs` — LIVE fade API (DoTransition/FadeOut/FadeIn)
4. `Assets/Scripts/Environment/Gate.cs` — gate state machine LIVE
5. `Assets/Scripts/Environment/MapFragmentBridge.cs` — bridge state LIVE (D2.5)
6. `Assets/Scripts/Environment/MapFragmentSpawner.cs` — spawner LIVE (D2.5)
7. `Assets/Scripts/Environment/FragmentDropAnchor.cs` — anchor pattern
8. `Assets/Scripts/Core/RuntimeRoomManager.cs:543-548` — NotifyBossDefeated hook
9. `Assets/Scripts/Core/Health.cs` — OnDeath UnityEvent

## Çıktı dosyaları

### 1. `Assets/Scripts/Systems/Map/RoomLoader.cs` (EXTEND ~150 LOC)

LoadNext() stub'ını gerçeklerle değiştir. Design doc §5 outline'ı **harfiyen
uygula**. Ek statik state:
- `public static int CurrentRoomIndex { get; private set; } = 0;`
- `public static event Action<int> OnRoomChanged;`
- `public static event Action OnDemoComplete;`
- `public static void RaiseDemoComplete() => OnDemoComplete?.Invoke();`

Instance state:
- `[SerializeField] private RoomSequenceData[] _sequence;` (5 SO ref Inspector)
- `private GameObject _currentRoomContent;`

LoadNext static → FindFirstObjectByType<RoomLoader> → LoadNextInstance().
LoadNextInstance → freeze player → RoomTransitionFX.DoTransition(SwapRoomWhileBlack)
+ StartCoroutine(ReenableAfterFade).

SwapRoomWhileBlack → TeardownCurrentRoom → teleport player rb.position → BuildRoomContent.

BuildRoomContent → mob spawn (skip if isRewardRoom) + focal element + gate
(runtime new GameObject + AddComponent<Gate>, default state AwaitingFragment) +
FragmentDropAnchor + reward room auto-trigger coroutine + boss death listener.

ReenableAfterFade → WaitUntil RoomTransitionFX.IsFading false → PlayerController.enabled = true
→ HUDController.SetRoomStatus("Room N/5 — DisplayName").

`LoadFirstRoom()` public method — Start veya manuel trigger için, RoomSequenceData[0]
LoadNext mantığıyla aynı ama CurrentRoomIndex=0 koruyup BuildRoomContent çağırır
(fade YOK çünkü first load).

**ÖNEMLİ:** OnRoomCleared event'i mevcut — RoomLoader.cs içinde Faz 1 mob death
listener mob count == 0 olduğunda OnRoomCleared fire etmeli (MapFragmentSpawner
bu eventi dinliyor). BuildRoomContent içinde mob spawn loop'unda her mob'un
Health.OnDeath'ine kalan-mob-counter callback ekle, count == 0 → OnRoomCleared
fire.

### 2. `Assets/Scripts/Systems/Map/RoomSequenceData.cs` (NEW ~50 LOC)

ScriptableObject, design doc §4 schema **birebir**. CreateAssetMenu path
"RIMA/Phase1/Room Sequence Data". Namespace `RIMA.Systems.Map`.

`EnemySpawnEntry` nested serializable class — `GameObject prefab; Vector3 position; bool isElite;`.

### 3. `Assets/Scripts/Core/DemoCompleteOverlay.cs` (NEW ~80 LOC)

Design doc §6 schema. Namespace `RIMA`. Static `Show()` runtime spawn pattern.
Restart button → SceneManager.LoadScene(current build index).

Subscribe to RoomLoader.OnDemoComplete via [RuntimeInitializeOnLoadMethod]:
```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
private static void HookDemoComplete()
{
    RoomLoader.OnDemoComplete -= Show; // idempotent
    RoomLoader.OnDemoComplete += Show;
}
```

### 4. `Assets/Scripts/Core/RuntimeRoomManager.cs` (1-LINE EDIT)

Line 543-548 NotifyBossDefeated — son satır olarak ekle:
```csharp
RIMA.Systems.Map.RoomLoader.RaiseDemoComplete();
```
**SADECE** bu satır. RuntimeRoomManager refactor yapma. Eğer Faz 1 path
RuntimeRoomManager.Start ile çakışırsa BLOCKED flag at, ek değişiklik yapma.

### 5. 5 ScriptableObject asset

`Assets/ScriptableObjects/Rooms/Phase1_Room1.asset` .. `Phase1_Room5.asset`.

Sonnet bu .asset'leri scene wire pass'inde Editor üzerinden doldurur — sen
**SADECE ScriptableObject class kodunu yaz**, .asset dosyalarını Sonnet
Unity'de oluşturur. Klasörü oluşturmak gerekirse `Assets/ScriptableObjects/Rooms/`
yoksa Codex create.

## Compile + verify

`refresh_unity scope=all mode=force` ZORUNLU yeni .cs sonrası (`feedback_unity_safety_protocol`).
`read_console` filter=Error — clean PASS şart. Compile fail → fix, ASLA partial commit.

## Output

`STAGING/ROOM_TRANSITIONS_codex_output.md` — değişen dosyalar listesi + compile durum
+ kısa impl notları (10-15 satır).

## YASAK

- Multi-scene (Karar #149)
- Yeni RoomTransitionFX yazma
- PlayerController değişiklik
- Cliff sistem dokunma (D5.6 paralel)
- T3 editor tool dokunma
- RuntimeRoomManager refactor (sadece 1-line NotifyBossDefeated edit)
- PlayerMovementController dokunma ([Obsolete])

## Süre

45-90 dk. Effort xhigh (`feedback_codex_effort_xhigh_2026_05_24`). Background
zorunlu. Stale lock cleanup TaskStop sonrası (`feedback_codex_stale_lock_after_taskstop`).

BLOCKED durum: (a) RuntimeRoomManager.Start Faz 1 path ile çakışırsa flag,
RuntimeRoomManager.cs refactor scope dışı; (b) MapFragmentSpawner subscription
OnRoomCleared event'i fire etmek için ek mob death wiring gerekirse flag;
(c) Health component OnDeath public UnityEvent değilse veya tip uyuşmazsa flag.
