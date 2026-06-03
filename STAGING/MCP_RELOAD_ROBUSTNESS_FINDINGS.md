# MCP Reload Robustness Findings

Status: FINDINGS_ONLY_PACKAGE_EDIT_GATED

## Current State

- Active transport in this Unity session: `Stdio`.
- Bridge status from Unity: running, port `6402`.
- HTTP auto-start EditorPref: `false`.
- No package files were modified. `Packages/com.coplaydev.unity-mcp` is installed package code; per task gate, package edits should go to Opus review before patching.

## Evidence

- `Packages/com.coplaydev.unity-mcp/Editor/Services/StdioBridgeReloadHandler.cs:16` has `[InitializeOnLoad]`.
- `StdioBridgeReloadHandler.cs:33-34` registers `AssemblyReloadEvents.beforeAssemblyReload` and `afterAssemblyReload`.
- `StdioBridgeReloadHandler.cs:43-89` sets `MCPForUnity.ResumeStdioAfterReload`, stops stdio, and writes a `reloading` heartbeat before domain reload.
- `StdioBridgeReloadHandler.cs:139-194` retries stdio resume on a schedule up to 30 seconds.
- `Packages/com.coplaydev.unity-mcp/Editor/Services/Transport/Transports/StdioBridgeHost.cs:126-132` auto-schedules stdio startup when HTTP transport is disabled.
- `StdioBridgeHost.cs:194-229` polls editor idle and skips startup while compiling.
- `StdioBridgeHost.cs:403` intentionally keeps `ProcessCommands` hooked across stop/start to reduce the reload gap.
- `Packages/com.coplaydev.unity-mcp/Editor/Services/HttpBridgeReloadHandler.cs:14` has `[InitializeOnLoad]`.
- `HttpBridgeReloadHandler.cs:29-30` registers before/after reload hooks.
- `HttpBridgeReloadHandler.cs:33-59` stores HTTP resume intent and force-stops HTTP before reload.
- `HttpBridgeReloadHandler.cs:109-154` retries HTTP resume on a schedule.
- `Packages/com.coplaydev.unity-mcp/Editor/Services/Transport/TransportManager.cs:35-43` caches one client instance per transport.
- `TransportManager.cs:45-64` restarts by reusing the cached transport client.
- `TransportManager.cs:68-91` stops clients but does not clear the cached client fields.
- `WebSocketTransportClient.cs:226-246` has a real `Dispose()` path that disposes `_sendLock`, `_socket`, and `_lifecycleCts`.
- `WebSocketTransportClient.cs:80-124` `StartAsync()` recreates socket/CTS, but not `_sendLock`; a cached client after `Dispose()` cannot be safely restarted.

## Root Cause Assessment

The bridge does have reload reconnect logic. The failure mode is the reload window itself:

1. During domain reload, Unity tears down the old AppDomain. The stdio listener and active TCP clients are stopped/closed, and HTTP sockets are force-stopped.
2. If an external MCP client sends a command during that window, the client can observe `Cannot access a disposed object` or equivalent broken-pipe behavior before the after-reload retry loop has completed.
3. On Unity side, reconnect is best-effort and asynchronous. Stdio has both a reload handler and host-level auto-start; HTTP has a reload handler but no local server launch in the reload handler.
4. `TransportManager` keeps cached transport client instances after stop/force-stop. This is acceptable for the normal stdio path, but brittle for HTTP because `WebSocketTransportClient.Dispose()` permanently disposes `_sendLock` while the manager has no way to detect or replace a disposed cached client.

So the user directive is correct: after recompile, agents should treat disposed-object errors as transient, wait until compilation is fully false, then reconnect/retry instead of reporting failure.

## Editor Windows

No evidence found that MCP explicitly closes RIMA editor windows such as Room Painter, Visual Map Designer, Brush Tool, or Blueprint Painter.

- `RimaRoomPainterWindow` stores window state in `[SerializeField]` fields and `EditorPrefs`; it has `OnEnable` and `OnDisable`, but no explicit `Close()` path in the searched code.
- `RimaVisualMapEditorWindow`, `MapDesignerBrushWindow`, and `BlueprintPainterWindow` are normal `EditorWindow` classes opened with `GetWindow`.
- The MCP window itself has duplicate-window cleanup (`MCPForUnityEditorWindow.cs:65`, `120`), but that targets MCP windows, not Map Designer windows.

If Map Designer windows disappear after reload, likely causes are compile errors, missing assembly/type after reload, or Unity window layout not restoring the type. A separate project-owned window restore shim could be added later if Opus wants it, but that is distinct from MCP transport reconnect.

## Smallest Viable Patch Proposal

Patch location: installed package, Opus review recommended before applying.

1. In `TransportManager`, clear cached clients after `StopAsync(mode)` and `ForceStop(mode)`.
   - After stopping HTTP, set `_httpClient = null`.
   - After stopping stdio, set `_stdioClient = null`.
   - This forces `GetOrCreateClient()` to create a fresh instance after reload instead of reusing a possibly disposed client.

2. In `HttpBridgeReloadHandler.OnAfterAssemblyReload()` and `StdioBridgeReloadHandler.OnAfterAssemblyReload()`, replace the single `delayCall` fallback with an `EditorApplication.update` poll that waits until both `EditorApplication.isCompiling` and `CompilationPipeline.isCompiling` are false before starting retries.
   - This matches the standing directive precisely.
   - It avoids starting a reconnect attempt while Unity is still in a post-reload compile/import state.

3. For HTTP local mode only, make reload resume mirror `HttpAutoStartHandler`:
   - If the local HTTP server is not reachable, attempt `MCPServiceLocator.Server.StartLocalHttpServer(quiet: true)` before `TransportManager.StartAsync(TransportMode.Http)`.
   - Current `HttpBridgeReloadHandler` reconnects only the bridge; it does not relaunch the local HTTP server.

## Risk

- Low code size, but medium operational risk because the patch touches installed MCP package code under `Packages/com.coplaydev.unity-mcp`.
- Clearing cached stdio clients may interact with `StdioBridgeHost` static startup logic; test with a deliberate script recompile.
- HTTP local relaunch can spawn duplicate server processes if process detection is wrong; guard with `IsLocalHttpServerReachable()` and existing PID checks.

## Recommended Verification After Patch

1. Open `_IsoGame`.
2. Confirm MCP active mode and port.
3. Trigger a script recompile.
4. Poll until `EditorApplication.isCompiling == false` and `CompilationPipeline.isCompiling == false`.
5. Retry a simple MCP call such as `manage_scene get_active`.
6. Confirm `read_console` reports 0 errors and no persistent `Cannot access a disposed object`.

