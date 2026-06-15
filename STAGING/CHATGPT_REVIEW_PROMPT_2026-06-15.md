# ChatGPT Independent Code Review — RIMA demo-critical surface

> Bu dosya = ChatGPT'ye yapıştırılacak TARAFSIZ inceleme promptu. Belirli bulgular VERİLMEDİ (taze/bağımsız göz). Repo erişimi: GitHub. NOT: `graphify-out/graph.json` gitignored → repoda YOK; sadece kaynak kodu incele.

---

## PROMPT (kopyala → ChatGPT'ye, repo erişimi açıkken)

You have read access to this GitHub repository (a Unity 2D top-down ARPG called RIMA, preparing a technical demo). I want an **independent, unbiased code review** of a specific set of runtime systems. Do NOT assume any prior analysis — review from scratch and form your own conclusions.

**What the demo does (context only, not a hint):** The player clears a combat room; a reward spawns; collecting it opens a 3-card skill draft; picking a card opens exit doors to the next room. Separately, there are two in-editor/runtime tools: a "Director Mode" overlay (toggled by the backquote ` key — enemy spawning, stat sliders, telemetry, free camera) and a "Build Mode" (toggled by F2 — places props into a working copy of the room template). These tools and the game share the same scene and some state.

**Files to review (this is just where things live — not what to find):**
- `Assets/Scripts/UI/DirectorMode.cs` — Director overlay; self-bootstraps via RuntimeInitializeOnLoadMethod; has a Test/Director state machine, time-scale control, free-camera, overlay visibility, spawn/stat/telemetry tabs.
- `Assets/Scripts/UI/BuildModeController.cs` — F2 build-mode toggle; owns a working-copy RoomTemplate; interacts with DirectorMode state.
- `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs` — prop placement (palette → ghost → commit) in build mode.
- `Assets/Scripts/Skills/DraftManager.cs` — 3-card draft after room clear; flags `IsDraftActive`/`IsDraftPending`; Forge milestones (rooms 4/8); Echo offer cadence; secondary-class unlock draft.
- `Assets/Scripts/Core/RewardPickup.cs` — room reward pickup (G key); opens the draft then exit doors.
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` — room-run orchestration: builds rooms, runs encounters, spawns reward, opens doors, advances nodes; manages `Time.timeScale`.
- `Assets/Scripts/Balance/DamageCalculator.cs` — damage formula (physical/ability power scaling, identity/situational multipliers, defense).

**Structural map (provided alongside this prompt — `graphify_demo_subgraph_2026-06-15.md`):** a condensed AST call-graph of exactly these files (per-file complexity hotspots by in/out-degree + the real cross-file coupling edges). Use it ONLY to prioritize where complexity and coupling cluster (e.g., which methods are god-nodes, which systems call into which). The AST call-edges contain generic-method-name noise — treat the map as a guide, and verify every concrete claim against the actual source. It is NOT a list of bugs.

**Focus areas (where to look — not what's wrong):**
1. How Director Mode and Build Mode get **activated/triggered**, and how their **state machines, time-scale, camera ownership, and overlay visibility** interact (including any coupling between them and with the normal game).
2. The **reward → draft → door** flow: state flags, early-returns, lifecycle (Awake/Start/OnEnable/OnDisable/OnDestroy), event subscribe/unsubscribe, coroutine guards, and any race conditions.
3. **DamageCalculator** correctness of stat scaling.

**Deliverable:** A numbered list of concrete issues you find. For each:
- Severity: **WOULD-BREAK-A-LIVE-DEMO** / **COSMETIC** / **NON-ISSUE-OR-FUTURE**.
- Exact `file:line`.
- One sentence: the issue + why it matters (or why it's minor).
Be rigorous and skeptical. If something looks fine, don't invent a problem. If you're unsure, mark it SUSPECTED. Prioritize anything that could freeze, soft-lock, double-trigger, or visibly misbehave during a single live play-through and across repeated play-throughs (the demo is rehearsed ~10×).

---
*(Sonuç gelince orchestrator'a ver → council 4-lens bulgularıyla birleştirip toplu fix yapılacak.)*
