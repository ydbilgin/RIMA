# F6 + F7: Cliff Runtime Culling Stub + Smoke Test

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Cliff F path son adım. F6: runtime culling stub (Unity built-in). F7: PlayMode smoke test F1-F5 visual verify.

## Bağlam
- F1 ✅ AdaptiveClusterFilter
- F2 ✅ Drop shadow tilemap
- F3 ✅ 6-katman parallax
- F4 ✅ Dust particle
- F5 ✅ Cliff face idle anim
- Tüm Sonnet write LIVE, Opus review F1+F4+F5 paralel devam (`af4345f16cb4fc12d`)
- F2+F3 Opus review sonra ayrı

## F6 — Runtime Culling Stub

### İş kalemleri
1. **NEW `Assets/Scripts/Environment/CliffRuntimeVisibility.cs`** (~30 LOC)
   - MonoBehaviour, CliffTilemap GameObject'e attach
   - Unity built-in `TilemapRenderer.detectChunkCullingBounds = true` ayarla `Awake()`'te
   - Optional: `cullingExtensions = new Vector3(2, 2, 0)` override
   - Default-on (Inspector toggle)
2. **Scene wire:** PlayableArena_Test01 CliffTilemap GO'ya attach component

### YASAK
- TilemapRenderer kendi yazma (Unity built-in API)
- Camera frustum custom impl (Unity zaten yapıyor)
- Performance test (smoke test scope)

## F7 — Smoke Test

### İş kalemleri
1. **UnityMCP `execute_code`** ile PlayMode automation:
   - `EditorApplication.EnterPlaymode()`
   - Wait 5 seconds
   - Read console log
   - `EditorApplication.ExitPlaymode()`
2. **Verify checklist:**
   - F1: CliffClusterRules slot atanmamışsa **YASAK** atama (kullanıcı manuel yapar) — sadece statu raporla
   - F2: PlayMode'da CliffDropShadowTilemap GO active + tile count > 0
   - F3: ParallaxRig 6 child active + ParallaxLayer.cs attached + factor değerleri doğru
   - F4: CliffEdgeDustEmitter GO mevcut (kullanıcı manuel wire yapacak)
   - F5: CliffFaceIdleAnimator component CliffTilemap GO'da active
3. **Console output check:**
   - 0 error / 0 warning
   - F2: "CliffDropShadowPlacer: mirrored N cells" log
   - F5: "CliffFaceIdleAnimator: animating N cells" log
4. **Screenshot (UnityMCP capable ise):** PlayableArena_Test01 PlayMode 5s sonra ekran

### Verify
- F6 compile 0 err / 0 warn
- F7 PlayMode entry/exit smooth
- 5 F-task LIVE doğrulandı

## Dosyalar (scope)
- `Assets/Scripts/Environment/CliffRuntimeVisibility.cs` NEW (~30 LOC)
- PlayableArena_Test01.unity (CliffRuntimeVisibility component attach)
- Toplam ~30 LOC + 1 scene mutation

## Output
- `STAGING/F6_F7_CULLING_SMOKE_DONE.md` — F6 değişen dosyalar + F7 smoke test sonucu + 5 F-task LIVE checklist + console log + screenshot path
