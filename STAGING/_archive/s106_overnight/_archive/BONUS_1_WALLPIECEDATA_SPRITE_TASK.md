ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only, NO removal of existing fields (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: WallPieceData.cs'a `Sprite spriteRef` field ekle ki Stream B-followup'taki 4 wpd_*_real.asset gerçek sprite'lara bind olabilsin. WallPiece.cs'i de güncelle ki spriteRef set ise visual.sprite'a apply etsin. Bu olmadan Stream B2 (visual swap proof) yapılamıyor. **NO removal of existing fields** — sadece field eklenir, backwards compat şart.

---

# BONUS #1 — WallPieceData Sprite Schema Extension

## Context

Stream B-followup (DONE 03:58) created 4 `wpd_*_real.asset` files for Combat/Ritual/Boss room real-asset visual swap. But `WallPieceData.cs` has NO `Sprite` field — only `prefab` (GameObject). So Codex couldn't bind sprites → assets exist with metadata but visual placeholder remains.

This task adds:
1. `public Sprite spriteRef` field to `WallPieceData`
2. `WallPiece.ApplyMetadata` updates `visual.sprite = data.spriteRef` IF spriteRef set (else keeps placeholder)
3. Bind sprites to the 4 _real.asset files (use the GUIDs Codex resolved in Stream B-followup)

## Files in scope
- `Assets/Scripts/Runtime/Walls/V2/WallPieceData.cs` (add field)
- `Assets/Scripts/Runtime/Walls/V2/WallPiece.cs` (use field in ApplyMetadata)
- `Assets/ScriptableObjects/Walls/V2/wpd_rear_wall_2x_real.asset` (bind sprite GUID b990b636ce45a3849a34d0cd81a43f1a)
- `Assets/ScriptableObjects/Walls/V2/wpd_side_wall_stepped_2x_real.asset` (bind 16a3fc7bdef74e84cb075f8fd4bece56)
- `Assets/ScriptableObjects/Walls/V2/wpd_low_front_outer_corner_real.asset` (bind 8e0aee3cc62fab846be1f606e1f2ebd6)
- `Assets/ScriptableObjects/Walls/V2/wpd_door_arch_2x_real.asset` (bind 2b78722fcbb032e4097edb18b67ade3a)

NO other files. NO removal of existing fields. NO refactor.

## Procedure

1. **Backup:** `WallPieceData.cs` + `WallPiece.cs` → `Assets/_archive~/pre_s106_bonus1/`
2. **Edit WallPieceData.cs:** Add `public Sprite spriteRef;` field (at the end, or anywhere logical). Add UnityEngine namespace if not already imported. Use `[SerializeField]` if you want it private+serialized; public works too.
3. **Edit WallPiece.cs ApplyMetadata:** After existing `visual.color = data.placeholderColor` line, add conditional:
   ```csharp
   if (data.spriteRef != null && visual != null)
   {
       visual.sprite = data.spriteRef;
       visual.color = Color.white; // restore default tint when using real sprite
   }
   ```
4. **Edit the 4 .asset YAMLs:** Add `spriteRef: {fileID: 21300000, guid: <SPRITE_GUID>, type: 3}` line. The fileID `21300000` is the standard Unity sprite sub-asset ID for the first sprite in a single-sprite image. If the sprite was sliced/multiple, the fileID might differ — use UnityMCP `manage_asset` to verify exact sub-asset ID.
5. **Compile-check after each .cs edit** via UnityMCP `read_console`. 0 errors required.
6. **Sanity test:** Read back one of the .asset files via reflection (or just verify Unity Inspector shows the sprite — UnityMCP screenshot of inspector).

## Safety
- ❌ NO scene operations
- ❌ NO prefab generation (separate task)
- ❌ NO existing wpd_*.asset modification (touch ONLY the 4 _real ones)
- ✅ AssetDatabase.Refresh ONCE at end
- ✅ Backwards compat: existing wpd_*.asset have NO spriteRef field → defaults to null → ApplyMetadata skips → behavior unchanged

## Output (mandatory)

Write to `CODEX_DONE_<profile>.md`:

```
# BONUS #1 WALLPIECEDATA SPRITE - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## C# changes
- WallPieceData.cs:<line> added `public Sprite spriteRef;`
- WallPiece.cs:<lines> added conditional sprite assignment

## Asset bindings (4 files)
- wpd_rear_wall_2x_real.asset: spriteRef bound to b990b636... (verified Inspector y/n)
- wpd_side_wall_stepped_2x_real.asset: spriteRef bound to 16a3fc7b... (y/n)
- wpd_low_front_outer_corner_real.asset: spriteRef bound to 8e0aee3c... (y/n)
- wpd_door_arch_2x_real.asset: spriteRef bound to 2b787229... (y/n)

## Compile check
- 0 errors, 0 warnings

## Backwards compat verified
- Loaded one existing wpd_rear_wall_1x.asset (no spriteRef) → ApplyMetadata behavior unchanged y/n

## Time: N min
```

## Estimated: 15-20 min
