ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read: kod / STAGING / CURRENT_STATUS.md / .claude/PROJECT_RULES.md.

# Amaç
Live editor "cliff tile live-reload" no-op'unu KAPAT. Şu an floor+prop reload çalışıyor, cliff intentional no-op çünkü `cliff_cells` JSON şemasında `tile_guid` yok. ADDITIVE şema değişikliği ile cliff'i de reload-edilebilir yap. Writer=sen (Codex), reviewer=Opus (sonra).

# ÖNCE OKU (gap analizi + kök neden)
- STAGING/LIVE_EDITOR_GAP_S114.md  (cliff no-op kök neden + önerilen fix burada)
- Assets/Scripts/Live/RoomLayoutData.cs       (CliffCell struct — tile_guid ekle)
- Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs  (cliff yazımı — guid yaz)
- Assets/Scripts/Live/LiveRoomReloader.cs     (ApplyCliffTiles no-op — kaldır, floor pattern uygula)
- Assets/Scripts/Live/RuntimeAssetRegistry.cs (GetTile(guid) API — floor reload bunu kullanıyor, aynısı)

# GÖREV (additive, surgical — floor pipeline pattern'ini birebir taklit et)
1. `CliffCell`'e cliff tile GUID alanı ekle (floor cell'deki tileGuid/tile_guid ile AYNI naming convention — snake_case impl tutarlı). Direction/manual alanları KORU.
2. RoomLayoutSerializer: cliff cell yazarken tile GUID'i de yaz (floor tile guid'i nasıl alıyorsa cliff için aynı yol — Tilemap.GetTile + AssetDatabase guid).
3. LiveRoomReloader.ApplyCliffTiles: no-op'u kaldır, floor reload pattern'ini uygula — guid→TileBase (RuntimeAssetRegistry.GetTile), cliff Tilemap'e SetTile. Cliff Tilemap referansını floor gibi al.
4. Schema doc varsa (STAGING/T3 veya schema md) cliff_cells'e tile_guid eklendiğini not düş — schemaVersion additive bump (1.0→1.1).

# KISITLAR
- ADDITIVE — eski JSON (tile_guid'siz cliff) okunabilir kalmalı (null/empty guid = skip, eski davranış). Geriye-dönük kır MA.
- Floor reload pattern'ini AYNEN taklit et, yeni mimari icat etme. Min kod.
- SADECE listelenen 4-5 dosya. Compile-clean bırak.
- Unity'de test ETME (orchestrator yapacak). Sadece kod + compile-temiz.

# ÇIKTI
Değişen her dosya + ne yaptığın özet. Compile-clean onayı. Belirsizlik (örn. cliff Tilemap referansı nereden) → BLOCKED yaz, tahmin etme.
