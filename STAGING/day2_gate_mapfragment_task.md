# Day 2 — Gate.cs + MapFragment.cs component implementation

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: bu task dosyası + Canonical spec'ler (MEMORY/map_fragment_canonical_spec.md + MEMORY/gate_socket_canonical_spec.md) + listelenen kod dosyaları.

## Amaç
Demo Faz 1 Milestone (5 oda Warblade) için Gate + MapFragment component'lerini ekle. Mevcut Portal flow'u (Day 1 LIVE) bozma — Gate/Fragment additive component (parallel architecture, opt-in via MapFragmentBridge bool toggle). Track A devamı.

## Allowed files (read/write)

### Read-only (context)
- `MEMORY/map_fragment_canonical_spec.md`
- `MEMORY/gate_socket_canonical_spec.md`
- `Assets/Scripts/Environment/MapFragmentBridge.cs`
- `Assets/Scripts/Environment/FragmentDropAnchor.cs`
- `Assets/Scripts/Environment/Portal.cs`
- `Assets/Scripts/Environment/PortalSpawnController.cs`
- `Assets/Scripts/Skills/DraftManager.cs` (sadece `TriggerDraftFromFragment` + `OnSkillPicked` signature)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` (sadece `LoadNext()` ve `OnRoomCleared`)

### Write
1. **NEW** `Assets/Scripts/Environment/MapFragment.cs`
2. **NEW** `Assets/Scripts/Environment/Gate.cs`
3. **MODIFY** `Assets/Scripts/Environment/MapFragmentBridge.cs` (add Fragment+Gate flow alongside existing Portal flow)

**YASAK:** scene file (`.unity`), prefab edits, başka kod dosyası, DraftManager.cs imza değişimi, RoomLoader.cs içerik değişimi.

## Spec — MapFragment.cs

Component summary (`RIMA.Environment.MapFragment`):

- `MonoBehaviour`, `[RequireComponent(typeof(SpriteRenderer))]`, `[RequireComponent(typeof(CircleCollider2D))]`
- Placeholder visual: 4x4 cyan procedural sprite (Portal.cs pattern reuse — buildPlaceholderSprite OK kopyalama), `Color(0f, 1f, 0.8f, 1f)` (#00FFCC)
- Trigger circle collider radius = 2.5f (pickup proximity), `isTrigger = true`
- Public events:
  - `public static event Action<MapFragment> OnAnyFragmentPickedUp;`
- Public state:
  - `public bool isPickedUp { get; private set; }`
- Idle anim (Update loop):
  - Bobbing: position.y = baseY + 0.10f * Mathf.Sin(2f * Mathf.PI * 2.2f * Time.time)
  - Alpha pulse: sr.color.a = Mathf.Lerp(0.6f, 1.0f, 0.5f + 0.5f * Mathf.Sin(2f * Mathf.PI * 3f * Time.time))
  - **NO rotate**
- Drop-in anim (0.4s, optional — Faz 1 placeholder yeterli):
  - Start scale 0 → 1, linear lerp 0.4s on Awake/OnEnable (`_dropElapsed` float, animate first 0.4s)
- Pickup logic:
  - `OnTriggerStay2D(Collider2D other)` — if `other.CompareTag(playerTag)` and `Input.GetKeyDown(KeyCode.G)` and !isPickedUp → `Pickup()`
  - `Pickup()`:
    - `isPickedUp = true`
    - `Debug.Log($"[MapFragment] Picked up at {transform.position}")`
    - `OnAnyFragmentPickedUp?.Invoke(this)`
    - GameObject.Destroy(gameObject, 0.05f) (or disable + queue destroy)
- Inspector tooltips (kanonik kaynak link comment): "Map Fragment canonical spec: MEMORY/map_fragment_canonical_spec.md — Broken Stone Tablet, #00FFCC cyan, bobbing ±0.10u @ 2.2Hz, alpha pulse 0.6-1.0 @ 3Hz, G + 2.5u pickup."

LOC budget: ~120-150 satır.

## Spec — Gate.cs

Component summary (`RIMA.Environment.Gate`):

- `MonoBehaviour`, `[RequireComponent(typeof(SpriteRenderer))]`, `[RequireComponent(typeof(BoxCollider2D))]`
- Placeholder visual: 8x8 grey procedural sprite + room type tint overlay (sr.color)
- Trigger box collider, `isTrigger = true`
- State machine `enum State { Locked, AwaitingFragment, Unlocked, Unrevealed }`
  - Default = `AwaitingFragment` (Demo Faz 1: room cleared, kapı orada ama hala fragment bekliyor)
  - Public state: `public State CurrentState { get; private set; }`
- Public room type → tint mapping (inspector serialize):
  - Combat → Color.white
  - Elite → new Color(1f, 0.3f, 0.3f) (red)
  - Boss → new Color(1f, 0.85f, 0.2f) (gold)
  - Shop → new Color(1f, 0.85f, 0.2f) (gold)
  - Spirit → new Color(0.7f, 0.3f, 1f) (purple)
  - Event → Color.green
  - Unknown → Color.gray
- `[SerializeField] private RoomTypeData roomType` (optional, ApplyTint() in OnEnable)
- Public methods:
  - `public void SetState(State newState)` — transitions + visual update (alpha 0.4 if Locked/AwaitingFragment, 1.0 if Unlocked, 0.2 if Unrevealed) + collider enabled (Unlocked only)
  - `public void Unlock()` — alias for `SetState(State.Unlocked)`, fires Open anim coroutine
- Open anim coroutine (6-8 frame placeholder):
  - 0.4s lerp: scale Y from 1.0 → 0.1 → 1.0 (squash) + alpha 0.4 → 1.0
  - 8 keyframe interpolation via discrete steps (0.05s per frame)
- Public event:
  - `public event Action<Gate> OnPlayerEntered;`
- `OnTriggerEnter2D` — if Unlocked + player tag → `OnPlayerEntered?.Invoke(this)` + Debug.Log
- Public tag: `public string playerTag = "Player";`
- Inspector tooltips: "Gate canonical spec: MEMORY/gate_socket_canonical_spec.md — 8 stil variant, 1.5-2x karakter, lock state machine, 6-8 frame open anim."

LOC budget: ~180-220 satır.

## Spec — MapFragmentBridge.cs modify

Mevcut Portal flow LIVE kalır (Day 1 playtest pending). Yeni opt-in flow eklenir:

- Yeni inspector bool: `[Tooltip("Day 2+ Fragment+Gate flow. If false, falls back to Day 1 Portal flow.")] public bool useFragmentGateFlow = false;` (DEFAULT FALSE — Day 1 scene bozulmasın)
- Day 2 flow (useFragmentGateFlow == true):
  - Subscribe `MapFragment.OnAnyFragmentPickedUp` in `OnEnable` (her zaman, branch in handler)
  - On fragment pickup → `DraftManager.Instance.TriggerDraftFromFragment(null)` (Portal arg null OK — DraftManager already handles null per its log line 134)
  - On `OnSkillPicked` → find all Gate components in scene with `CurrentState == AwaitingFragment` → call `gate.Unlock()` for each
  - Subscribe to each Gate's `OnPlayerEntered` → call `RoomLoader.LoadNext()`
- Day 1 flow LIVE (useFragmentGateFlow == false) — değiştirme, mevcut Portal subscription + armed HashSet aynen kalsın
- Class içine new private fields:
  - `private readonly List<Gate> _gateSubscriptions = new List<Gate>();`
- New private methods:
  - `private void HandleAnyFragmentPickedUp(MapFragment fragment)` — bridge-side log + draft trigger
  - `private void UnlockAllAwaitingGates()` — find + unlock + subscribe OnPlayerEntered
  - `private void HandleGateEntered(Gate gate)` — log + RoomLoader.LoadNext()

LOC artış: +60-80 satır.

## Verification (sub-agent self-check before reporting DONE)

1. **Grep:** `using RIMA.Environment;` ve namespace `RIMA.Environment` doğru tüm 3 dosyada
2. **Compile readiness:** mantıksal hata yok — DraftManager.TriggerDraftFromFragment imzası `(Portal source)` → null kabul ediyor mu? (Read DraftManager.cs line 131-150 to verify — null handling var)
3. **Scene safety:** PlayableArena_Test01.unity dokunulmamış (write list'te yok)
4. **Canonical spec uyum:** bobbing/pulse/pickup radius değerleri spec ile eşleşiyor (±0.10u, 2.2Hz, 0.6-1.0, 3Hz, 2.5u, G key)
5. **MapFragmentBridge backward-compat:** useFragmentGateFlow=false default → Day 1 portal flow aynen çalışıyor

## Output format (CODEX_DONE benzeri)

`STAGING/day2_gate_mapfragment_DONE.md` yaz, içerik:

```markdown
# Day 2 Gate + MapFragment — DONE

## Files
- MapFragment.cs — NEW, X LOC
- Gate.cs — NEW, X LOC
- MapFragmentBridge.cs — MODIFIED, +X LOC

## Verification
[checklist]

## Pending (user)
- Scene wire: drop MapFragment + Gate prefabs/GO'ları sahneye
- MapFragmentBridge.useFragmentGateFlow=true toggle
- Playtest: room cleared → fragment spawn → G pickup → draft → gate unlock → enter

## Compile check
[Not done in this dispatch — orchestrator UnityMCP read_console ile doğrulayacak]
```

## BLOCKED protocol

Eğer:
- DraftManager.TriggerDraftFromFragment null reddediyorsa → BLOCKED: "needs DraftManager.TriggerDraftFromFragment overload accepting null"
- RoomTypeData.cs bulamıyorsan → BLOCKED: "RoomTypeData missing, where?"
- Mevcut MapFragmentBridge.cs ile çakışma → BLOCKED: detail

Spec'ten sapma YOK. Bu spec konuşulmuş canonical, drift yapma.
