# N2 — Cliff + Envanter Tutarlılık Aksiyon Listesi (S114)

**Amaç:** "Cliffler/envanterde uyumsuzluk varsa kaldır" — duvarsız floating-ada canon'una göre tutarsız asset'leri tespit + kaldırma kararı. Kaynak: #41 PixelLab sentez (3-AI) + #42 safe-delete audit + N1 cliff canon. **SİLME YOK — kullanıcı onay listesi (git-recoverable).**

## A. SAFE-DELETE (0 referans, git-recoverable — kullanıcı tek "evet" yeter)
`STAGING/SAFE_DELETE_AUDIT_S114.md`'den:
- `Art/_TempReferencePacks/Jaqmarti_IsoWalls/` (klasör + library_walls_00.png) — 0 ref
- `Art/_TempReferencePacks/Nilsen303_IsoDungeon/` (klasör + DungeonIsometricTileset.png) — 0 ref
- `Art/Characters/Warblade/south.png` — 0 ref (Resources'taki idle_south farklı dosya)
→ 3 grup, ~4 dosya. Tamamen güvenli. **Aksiyon: kullanıcı onayında sil.**

## B. UNSAFE — SİLME (aktif referanslı, audit kanıtlı)
- `Sprites/AssetPackV3/floor_iso_pixellab_35deg/` (16 PNG) — PlayableArena_Test01 + PlayableArena aktif kullanıyor. ⚠️ Karar#150 iso-deprecated AMA aktif sahnede → ÖNCE sahne temizliği (oda rebuild'de KitA-floor'a geçince serbest kalır).
- `Prefabs/Obstacles/StoneColumn.prefab` — aktif sahne + RoomBuilder.cs + ObstaclePrefabBuilder.cs hardcoded. KAL (void-içi column wall-less'ta L5 dekor olabilir).
- `Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` (3) — _archive_old_iso/ bağlı; archive silinince serbest.

## C. PixelLab CLOUD envanter (`STAGING/PIXELLAB_SYNTHESIS_S114.md`, 3-AI)
- **DELETE adayı (51, cloud — kullanıcı PixelLab Web UI):** weapons_8dir ×3 (yanlış format), mob off-roster ×10, skill_icons generic violet ×20 (off-brand), misc_props off-palette ×3, awaiting-selection duplicate ×15.
- **REVIEW (46):** walls/floor violet sapma, untagged weapons, **tiles_rift_cliff** (cliff-ilgili!), vfx_blood, scatter awaiting.

## D. CLIFF-özel tutarlılık (wall-less floating canon)
- **tiles_rift_cliff** (PixelLab REVIEW): cliff-ile-ilgili, violet/format şüpheli. Karar: wall-less ambiyansa uyuyorsa KEEP, değilse regen (N3 cyan-rim metodu + N4 erozyon ile). → kullanıcı görsel-bakış.
- **Sahnedeki cliff'ler kullanıcı tarafından silinmişti** (N1 bulgu) → oda rebuild gerek (2-stage auto-placer, N1 canon). Bu bir EKLEME işi, kaldırma değil.
- **KitB_Cliff** sprite kiti (T3 doc ref) = wall-less cliff face için canonical. Eski wall-production duvar setleri (119 parça) L5-dekor'a downgrade (NLM canon), cliff-sınır DEĞİL.

## E. Envanter genel (KEEP teyit)
37 KEEP-T1 + 109 KEEP-T2 (PixelLab sentez) — wall_s95, painterly-hand VFX, 5 class weapon, statues, mob KEEP-6, mounting. Bunlar canon-uyumlu, dokunma.

## Özet aksiyon
1. **Gece-güvenli (kullanıcı tek-onay):** A grubu 3 sil (git-recoverable).
2. **Sahne-rebuild sonrası:** B floor_iso serbest kalınca sil.
3. **Kullanıcı Web-UI:** C 51 cloud DELETE + 46 REVIEW (özellikle tiles_rift_cliff cliff-bakış).
4. **Ekleme (kaldırma değil):** oda cliff rebuild (auto-placer) — ND5/oda-rebuild task.

**Not:** Bu liste 3-AI (#41 sentez Opus+Codex+agy) + audit (#42) üstüne Opus konsolidasyonu. Yeni silme JUDGMENT'ı = bu doc. Hiçbir şey silinmedi.
**Index:** `reference_inventory_consistency_n2`.
