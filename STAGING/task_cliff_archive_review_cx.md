ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

Amaç: (A) Kullanılmayan/orphan/duplicate environment asset SET'lerini arşivle (proje temizliği), (B) Opus'un yaptığı cliff shader+offset değişikliklerini correctness açısından review et. Unity instance açık; UnityMCP profilinden erişebilirsin.

## PART A — ARCHIVE unused environment sprite sets

Audit (GUID cross-ref ile doğrulandı) şu setlerin _IsoGame.unity'de KULLANILMADIĞINI buldu. LIVE keep-triad = CliffKit_RefB + PixelLabFloor451 + BgKit_RefC (BUNLARA DOKUNMA).

Arşiv hedef klasörü: `Assets/Sprites/Environment/_archive~/` (sondaki `~` = Unity import etmez = projeden temiz çıkar). Mevcut konvansiyon: `Assets/_archive~/`, `Assets/Sprites/Environment/_archive_imagegen~/` zaten var.

Adımlar (SIRAYLA):
1. Önce baker'ı düzelt: `Assets/Editor/RoomPainter/LiveTool/RuntimeAssetRegistryBaker.cs` içindeki `ScanRoots` listesinden ŞU ÜÇ folder satırını SİL (içerikle bul, satır no güvenme): `KitB_Cliff`, `PixelLabFloorFlat`, `PixelLabFloorIso`. (Bunlar registry'de stale GUID bırakmasın diye önce baker'dan çıkmalı.) Diğer ScanRoots girişlerine dokunma.
2. Şu 9 klasörü `_archive~/` altına TAŞI (System.IO.Directory.Move kullan — `~` klasör AssetDatabase dışı; .meta dosyalarını da taşı):
   - KitC_BG, PixelLabFloor, PixelLabFloorIsoThin, IsoMockKit, Phase0_ScaleTest, Placeholders  (Tier-1, sıfır ref)
   - KitB_Cliff, PixelLabFloorFlat, PixelLabFloorIso  (Tier-2, baker'dan çıkarıldı)
   Hepsi `Assets/Sprites/Environment/<folder>` altında. Klasörün `.meta`'sını da taşı.
3. `AssetDatabase.Refresh()`.
4. Registry'yi yeniden bake et (RuntimeAssetRegistryBaker'ın menü item'ı / bake metodu — `RIMA/...` menüsünden veya doğrudan static bake metodunu çağır). Bake sonrası registry artık taşınan setlerin GUID'lerini içermemeli.
5. DOĞRULA: `read_console` (error/warning) — missing-script/missing-reference HATA OLMAMALI (folder-skip warning'leri normal). Sahne `_IsoGame.unity` hâlâ CliffKit_RefB/PixelLabFloor451/BgKit_RefC referanslarını korumalı (bunları taşımadın, dolayısıyla intact). dotnet/Unity compile temiz.

DİKKAT: CliffKit_RefB, PixelLabFloor451, BgKit_RefC, IsoKit, PixelLabKit, PixelLab_Selected_Assets, RIMA_AssetParts_v2, ShatteredKeep_PixelLab, RuinedKeepKit, Parallax — BUNLARI ARŞİVLEME (live ya da ayrı audit kapsamında).

## PART B — REVIEW Opus's cliff changes (correctness only, do NOT modify)

Opus şu 3 değişikliği yaptı; sadece correctness/regression açısından incele, BULGULARI RAPORLA (düzeltme yapma):
1. `Assets/Shaders/CliffVoidFade.shader` UnlitFragment son satırı: `max(color.a, fade)` -> `color.a` (artık fade sadece RGB karartıyor, alpha'yı void'e zorlamıyor; şeffaf damla-ucu void'i gösteriyor). Shader hâlâ derleniyor mu, blend/queue Transparent doğru mu, regresyon var mı?
2. `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset` transformOffset.y: -0.25 -> -0.15 (pivot top-center + ~7px şeffaf padding hesabıyla cliff rim'i floor ön-köşesine flush). Mantıklı mı?
3. CliffTilemap material `CliffVoidFade`: _FadeStart=0.45, _FadeEnd=0.10, _VoidColor=#020918 (bg_L0_void ile eşleşen indigo). Değerler tutarlı mı?

## ÇIKTI FORMAT
CODEX_DONE.md'ye yaz: PART A = yapılan taşımalar + baker edit + rebake sonucu + console durumu (PASS/FAIL). PART B = 3 madde için correctness verdict (OK / concern + neden).
