# UnityMCP Scene Modal Bypass

Status: SPEC_READY_FOR_DISPATCH
Date: 2026-05-18
Profile: any (laurethayday önerilen — boş)
Effort: medium
Timeout: 1800s
Package: com.coplaydev.unity-mcp v9.6.8

## Universal Coding Principles (Karpathy 4)

1. THINK BEFORE CODING: assumption'larını listele, belirsizlik varsa BLOCKED yaz.
2. SURGICAL CHANGES: sadece listelenen dosyalar — başka dosyaya dokunma.
3. MIN CODE: spekülatif feature yok, abstraction ekleme.
4. GOAL-DRIVEN: success criteria muğlaksa BLOCKED yaz.

## Context

User feedback 2026-05-18:
"Do you want to save the changes you made in the scenes: Untitled" dialog çıkıyor, Unity bloklanıyor, MCP timeout, Codex dispatch'leri takılıyor. Unity defalarca çökmüş.

Two distinct blocking scenarios in `ManageScene.cs`:

1. **action="load"** — `LoadScene(string)` and `LoadScene(int)` guard `isDirty` with `return new ErrorResponse(...)`. Codex receives an error and cannot proceed.
2. **action="create"** — `CreateScene()` calls `EditorSceneManager.NewScene(EmptyScene, NewSceneMode.Single)` with NO prior dirty check. Unity raises the native OS modal "Do you want to save?" — MCP server blocks waiting for response that never comes.

## Affected File

`Library/PackageCache/com.coplaydev.unity-mcp@13fb3ee12774/Editor/Tools/ManageScene.cs`

Note: This is a UPM PackageCache file — read-only. Codex must create a local package override.

## Package Override Strategy

1. Create `Packages/com.coplaydev.unity-mcp/` directory
2. Copy all files from `Library/PackageCache/com.coplaydev.unity-mcp@13fb3ee12774/` into it
3. Add to `Packages/manifest.json`: `"com.coplaydev.unity-mcp": "file:com.coplaydev.unity-mcp"` (overriding registry version)
4. Edit the local copy

Pre-condition: Verify `Packages/manifest.json` does not already have a `file:` override. If it does, abort and write BLOCKED.

## Implementation

### Step 1 — Add `forceDiscard` to `SceneCommand`

In `SceneCommand` sealed class, add after `autoRepair` field:

```csharp
public bool? autoRepair { get; set; }
public bool? forceDiscard { get; set; }   // NEW
```

### Step 2 — Parse in `ToSceneCommand()`

```csharp
autoRepair = ParamCoercion.CoerceBoolNullable(p["autoRepair"] ?? p["auto_repair"]),
forceDiscard = ParamCoercion.CoerceBoolNullable(p["forceDiscard"] ?? p["force_discard"]),
```

### Step 3 — Shared helper `HandleDirtyScene`

Add before `CreateScene()`:

```csharp
private static string HandleDirtyScene(bool forceDiscard)
{
    if (!forceDiscard) return null;

    var active = EditorSceneManager.GetActiveScene();
    if (!active.isDirty) return null;

    if (string.IsNullOrEmpty(active.path))
    {
        // Untitled + dirty: save to temp then delete asset
        string tempPath = "Assets/_mcp_discard_temp.unity";
        EditorSceneManager.SaveScene(active, tempPath, saveAsCopy: false);
        AssetDatabase.DeleteAsset(tempPath);
        return null;
    }
    else
    {
        // Named + dirty: auto-save in-place (preserve user work)
        bool saved = EditorSceneManager.SaveScene(active);
        if (!saved)
            return $"Failed to auto-save scene '{active.path}' before load.";
        return null;
    }
}
```

### Step 4 — Patch `LoadScene(string)` (lines 374-418)

Change signature: `private static object LoadScene(string relativePath, bool forceDiscard = true)`

Replace dirty block (lines 391-399) with:
```csharp
if (EditorSceneManager.GetActiveScene().isDirty)
{
    if (!forceDiscard)
        return new ErrorResponse(
            "Current scene has unsaved changes. Pass force_discard=true to auto-handle."
        );
    string err = HandleDirtyScene(forceDiscard: true);
    if (err != null) return new ErrorResponse(err);
}
```

### Step 5 — Patch `LoadScene(int)` (lines 420-457)

Same as Step 4. Signature: `private static object LoadScene(int buildIndex, bool forceDiscard = true)`

### Step 6 — Patch `CreateScene()` (lines 336-372)

Signature: `private static object CreateScene(string fullPath, string relativePath, bool forceDiscard = true)`

Insert at start of try block, BEFORE `EditorSceneManager.NewScene(...)`:
```csharp
string dirtyErr = HandleDirtyScene(forceDiscard);
if (dirtyErr != null) return new ErrorResponse(dirtyErr);
```

### Step 7 — Update call sites in `HandleCommand()`

```csharp
// action="load" (lines 248-258):
return LoadScene(loadPath, cmd.forceDiscard ?? true);
return LoadScene(buildIndex.Value, cmd.forceDiscard ?? true);

// action="create":
return CreateScene(fullPath, relativePath, cmd.forceDiscard ?? true);
```

Inspect `CreateSceneFromTemplate` too — if it calls `NewScene` internally, apply same guard.

## Default Behavior Decision

`force_discard` default = **`true`** (auto-handle enabled by default).

Rationale: existing `ErrorResponse` behavior already blocks ALL automated Codex workflows. Defaulting `true` fixes them. Named+dirty = auto-save preserves user work. Untitled+dirty = safe to discard (never saved with intent).

## Success Criteria

### Pre-conditions
- [ ] Unity Editor başlatılabilir + 0 console error baseline
- [ ] `Packages/manifest.json` mevcut, `file:` override yok (yoksa BLOCKED)
- [ ] PackageCache path exists: `Library/PackageCache/com.coplaydev.unity-mcp@13fb3ee12774/Editor/Tools/ManageScene.cs`

### Acceptance Tests
- [ ] Package local override at `Packages/com.coplaydev.unity-mcp/` compiles cleanly
- [ ] `LoadScene(string)` and `LoadScene(int)` no longer return ErrorResponse when dirty + force_discard=true
- [ ] `CreateScene` no longer triggers Unity's native "save changes?" dialog when dirty + force_discard=true
- [ ] 4 new EditMode tests PASS (untitled+dirty discard, named+dirty save, force_discard=false backward compat, create+dirty bypass)
- [ ] Existing test baseline preserved (run_tests EditMode, 0 regressions)
- [ ] Console 0 errors after reload

### Out of Scope
- Prefab stage modal bypass (manage_prefabs ayrı tool)
- Play mode auto-save (Unity Preferences)
- Multi-scene additive dirty handling (LoadSceneAdditive replace etmiyor — modal risk yok)
- `set_active_scene` — modal tetiklemiyor

### DONE Marker
`STAGING/CODEX_TASK_UNITYMCP_SCENE_MODAL_BYPASS_DONE.md` formatı:
```
PASS_FOR_ORCHESTRATOR_REVIEW
Package override path: Packages/com.coplaydev.unity-mcp/
Tests added: 4
Tests passed: N/N
Dirty-untitled discard: confirmed
Dirty-named auto-save: confirmed
Console errors: 0
```

### BLOCKED Durumu
Eğer `Packages/manifest.json`'da zaten override varsa, veya package fingerprint farklıysa (`@13fb3ee12774` yerine başka), DONE marker'a `STATUS: BLOCKED — [sebep]` yaz, dur.

## Tests Detail

Yeni dosya: `Assets/Tests/EditMode/MCPSceneLoadModalBypassTests.cs`

```csharp
[Test] LoadScene_WithDirtyUntitledScene_ForceDiscardTrue_LoadsTarget()
[Test] LoadScene_WithDirtyNamedScene_ForceDiscardTrue_SavesAndLoads()
[Test] LoadScene_WithDirtyScene_ForceDiscardFalse_ReturnsError()
[Test] CreateScene_WithDirtyUntitledScene_ForceDiscardTrue_CreatesWithoutModal()
```

ManageScene package'da olduğu için test assembly direkt erişemeyebilir. Yol:
- (A) Package override eklendikten sonra `Packages/com.coplaydev.unity-mcp/Editor/Tools/ManageScene.cs.asmdef` test asmdef'ine reference olarak ekle
- (B) `execute_code` ile integration test (ManageScene.HandleCommand'i reflection ile çağır)

Codex tercih etsin. Eğer (A) zor, (B) yeterli.
