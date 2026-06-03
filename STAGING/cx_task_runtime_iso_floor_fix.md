# cx TASK — Runtime (F5/Play) iso floor fix

**Amaç:** F5/Play-mode'da prosedürel runtime oda-kurma path'i, edit-mode'da DOĞRULANMIŞ iso görünümle eşleşsin: floor Tilemap'in Grid'i **Isometric + cellSize (0.96, 0.585, 1)** olsun ki `floor451` granit tile'ları yassı/rectangular değil, doğru iso elmas olarak render edilsin. Şu an runtime Grid'i default Rectangular kalıyor → floor451 yassı görünüyor (= "ürettiğimiz tile'lar yok / düz gri floor" bug'ı).

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the files listed below (4) BLOCKED if unclear — partial impl etme, CODEX_DONE.md'ye BLOCKED yaz.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

---

## ROOT CAUSE (already traced via read-only map — VERIFY against current code, then fix; don't re-investigate from scratch)

Linear play-mode trace:
```
PlayableArena_Test01.unity (active play scene; _IsoGame.unity is RETIRED — do NOT use it)
  -> Systems/Map/RoomLoader.Start()            (autoStart -> LoadFirstRoom)   ~line 39-41, 98-110
  -> RoomLoader.BuildRoomContent()             (spawns mobs/gates/fragments; does NOT create floor) ~line 228-397
  -> Core/LargeDungeonMapPainter.PaintForRoom()                                ~line 283-300
  -> Core/LargeDungeonMapPainterBase.ResolveTilemaps()  finds floor Tilemap via:
        GameObject.Find("IsoGrid/Ground") ?? "Room/Floor" ?? "Grid/BaseTilemap" ?? "BaseTilemap"   ~line 396-415
  -> Core/LargeDungeonMapPainterBase.PaintFloor()  SetTile() into that Tilemap ~line 667-679
```

THE BUG: the painter paints tiles into the found Tilemap but **never configures the parent Grid's `cellLayout` / `cellSize`**, so the Grid stays Rectangular(default) and the iso tiles render flat.
- `Systems/Map/RoomLoader.ValidateContract()` (~line 580-587) only **LogWarning**s on cellSize / gridLayout mismatch — it never APPLIES the values.
- Editor-only path `Map/RoomBuilder.cs:327-328` DOES set `grid.cellLayout = Isometric; grid.cellSize = (0.96,0.585,1)` — but that path does not run at F5.
- `Systems/Map/RoomConfig.cs:30-32` defines the iso recipe (cellSize / gridLayout) but it is never applied at runtime.

Active vs dead loader (confirm before editing):
- LIVE: `Assets/Scripts/Systems/Map/RoomLoader.cs`
- DEAD: `Assets/Scripts/Map/Runtime/RoomLoader.cs` (`[System.Obsolete]`) — do NOT touch.

---

## THE FIX (Approach B — make the procedural runtime path produce the verified iso look)

**STEP 0 — verify the trace.** Open the files and confirm: (a) which RoomLoader/painter actually runs in PlayableArena_Test01's bootstrap, (b) that the Grid layout is indeed never set at runtime. If the real entry point differs from the trace above, follow the real one and note it in the report.

**STEP 1 — PRIMARY fix (high confidence): apply the iso Grid recipe at runtime.**
Make the runtime floor path set the floor Tilemap's parent `Grid` to:
- `cellLayout = GridLayout.CellLayout.Isometric`
- `cellSize  = new Vector3(0.96f, 0.585f, 1f)`

Pick the SINGLE correct site (whichever actually executes in the PlayableArena_Test01 path):
- Preferred if it runs: `Systems/Map/RoomLoader.ValidateContract()` — change warn-only into APPLY (set baseGrid.cellLayout = config.gridLayout; baseGrid.cellSize = config.cellSize) **but only if** `config.gridLayout`/`config.cellSize` actually hold the iso values (Isometric / 0.96,0.585,1). If they're rectangular/zero, do NOT propagate garbage — use the hardcoded iso recipe constants instead (matching RoomBuilder.cs:327-328) and note it.
- Alternative if ValidateContract isn't on the live path: `Core/LargeDungeonMapPainterBase.ResolveTilemaps()` — after the floor Tilemap is resolved, get `floorTilemap.transform.parent.GetComponent<Grid>()` and set the iso layout+cellSize before any paint.

Use ONE site, not both. Keep the iso values consistent with RoomBuilder.cs:327-328 (single source of truth — if there's a const/static for the recipe, reuse it rather than re-hardcoding; if not, a local const is fine).

**STEP 2 — SECONDARY (investigate + report; only fix if low-risk pure-code):**
Determine what floor TILE the runtime painter actually uses (`LargeDungeonMapPainterBase.floorTiles[]` or wherever PaintFloor pulls from). Is it `floor451_*` (the locked granite — `Assets/Sprites/Environment/PixelLabFloor451/`) or a generic grey/brown palette?
- If it's ALREADY floor451 → STEP 1 alone solves the visual; just confirm in report.
- If it's a grey palette and the floor tile source is a **serialized field on a prefab/.asset/SO** → do NOT blind-edit YAML. REPORT it as a follow-up needing Unity-open rewiring, with the exact field + asset path. Do not force it.
- If repointing is a clean pure-code default (e.g. a Resources.Load path or a code-level default tile) → fix it to floor451 and note it.

---

## VERIFY
- `dotnet build` the solution/csproj (compile-verify WITHOUT Unity — Unity may be closed). Must be **0 errors**. Report the exact command + result tail.
- Do NOT require UnityMCP/Unity. The F5 runtime VISUAL verification is GATED to the user (they will press F5 in Unity). Do not claim the visual is fixed — only that the code now applies the iso Grid recipe at runtime and compiles clean.

## BLOCKED IF
- You cannot determine which loader/painter actually runs at F5 (ambiguous entry point) → BLOCKED, report findings.
- The iso recipe values are not available anywhere (no config iso values AND no RoomBuilder constants) → BLOCKED.
- Fixing the floor would require Unity-only serialized/scene edits with no safe pure-code path → do STEP 1, mark STEP 2 as Unity-gated follow-up (NOT blocked overall).

## REPORT to CODEX_DONE.md
1. Confirmed entry point + active loader/painter (file:line).
2. Exact files + line ranges changed, with the iso-recipe site chosen and why.
3. STEP 2 outcome: what floor tile source the runtime uses; floor451 already, or follow-up needed (with field+asset path).
4. `dotnet build` result (command + 0 errors).
5. Any BLOCKED items.
6. Files touched: ONLY from {Systems/Map/RoomLoader.cs, Core/LargeDungeonMapPainterBase.cs, Core/LargeDungeonMapPainter.cs, Systems/Map/RoomConfig.cs}. If you must touch anything else, explain why first.
