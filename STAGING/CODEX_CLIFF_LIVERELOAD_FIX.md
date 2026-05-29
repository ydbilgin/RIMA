ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the 3 listed files (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>" (probably not needed).
Direct-read: STAGING/LIVE_EDITOR_GAP_S114.md (§"Cliff Live-Reload Neden No-Op" + "Fix için gerekli adımlar"), and the 3 files below.

# Amaç
Live editor'da cliff tile live-reload no-op'unu kapat. Floor pipeline ÇALIŞIYOR, cliff aynı zincire bağlanmamış (schema'da cliff_cells'te tile_guid yok). Additive, low-risk. Bu, "floor reload oluyor ama cliff olmuyor" asimetrisini bitirir.

# Tam spec (LIVE_EDITOR_GAP_S114.md "Fix için gerekli adımlar" — birebir uygula)
3 dosya, surgical:

1. **`Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs`**
   - İç `CliffCell` sınıfına `public string tile_guid;` ekle.
   - Cliff tilemap için `AddTiles()` (FloorTile üreten) yerine yeni `AddCliffTiles()` yaz: cliff tilemap'in dolu hücrelerini gez, her biri için `CliffCell { cell=[x,y], is_decor=..., tile_guid=<TileBase'in AssetDatabase GUID'i> }` üret, `cliff_cells[]` listesine yaz (floor_tiles'a DEĞİL).
   - Hangi tilemap'in cliff olduğunu naming convention ile ayır (örn. name contains "Cliff"), floor'u floor_tiles'a cliff'i cliff_cells'e route et. Floor pipeline'ı BOZMA.

2. **`Assets/Scripts/Live/RoomLayoutData.cs`**
   - `CliffCellData` sınıfına `public string tile_guid;` ekle (deserialize tarafı). Mevcut alanları koru (additive).

3. **`Assets/Scripts/Live/LiveRoomReloader.cs`**
   - `ApplyCliffTiles()` içindeki `// no-op` bloğunu kaldır, floor pipeline ile AYNI mantığı uygula: `_cliffTilemap.ClearAllTiles()` + her CliffCellData için `ResolveTile(cc.tile_guid)` ile `SetTile(cell, tile)`. ResolveTile/registry erişimi floor ile aynı yardımcıyı kullansın.
   - `_cliffTilemap` bulma mantığını güçlendir: `tm.name.ToLowerInvariant().Contains("cliff")` zayıf — ayrıca null ise log uyarısı ver. (Naming convention'ı serializer ile tutarlı tut.)

# Kısıtlar
- ADDITIVE: yeni field null/boş = mevcut davranış (eski JSON'lar çalışmaya devam). Floor pipeline'a dokunma.
- `#if DEVELOPMENT_BUILD || UNITY_EDITOR` guard'larını koru.
- Compile-clean bırak. Mümkünse LiveToolSmokeTests'e cliff round-trip için 1-2 test ekle (RoomLayoutData.FromJson cliff_cells tile_guid parse).
- Bitince CODEX_DONE'a: hangi dosyalar+satırlar değişti, additive olduğunu nasıl garantiledin, compile durumu.
