ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>". Direct-read: code under Assets/, STAGING, CURRENT_STATUS.md.

# Amac
RIMA'da 3 ORTUSEN map editoru var; bunlari TEK guzel+entegre+TEST-korumali oyun-ici editore indirmenin FIZIBILITE/REUSE planini + bu sorunlarin tekrar etmemesi icin TEST SUITE'ini cikar. ANALIZ ONLY, kod degisikligi YOK. CODEX_DONE.md'ye yaz.

LENS (sen=cx): feasibility/reuse — gercek kodu OKU ve somutla.

## Durum: 3 ortusen editor (kanit dosyalari)
1. `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` — ONCEDEN VAR, F2, IMGUI (cirkin/karanlik), self-bootstrap, Floor/Cliff/Prop tilemap boyar, UnifiedDesignerCore + JSON sidecar ile KAYDEDER, Editor ile ayni veri.
2. Benim Build Mode: `Assets/Scripts/UI/BuildMode/*` (BuildModeController/BuildPlacementController/BuildTileBrushController/BuildModeAssetCatalog/BuildModeUiStyle) — uGUI premium, prop+tile+walkability, overlap-hide (oyun UI'ini gizler), undo, working-copy/no-pollution; AMA kayit YOK, eski overlay'den habersiz kuruldu, az once F2'ye de baglandi -> CAKISMA.
3. `Assets/Scripts/RoomPainter/UnifiedDesignerCore.cs` + `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` — edit-mode editor, UnifiedDesignerCore'u paylasir.

## YANITLA (gercek kodu okuyarak)
1) KONSOLIDASYON: en temiz tek-editor yolu? Onerim: uGUI ON-YUZ (benim Build Mode) + `UnifiedDesignerCore` ARKA-UC (kanitli kayit/veri) + eski IMGUI F2'yi EMEKLIYE AYIR. InPlayMapPaintOverlay.cs + UnifiedDesignerCore.cs + benim BuildMode'u oku: her birinden NE reuse edilir, NE kirilir, merge nasil yapilir? Daha iyi bir yol var mi?
2) TEST SUITE (kullanicinin #1 istegi: "tekrar bu sorunla karsilasmayalim"): hangi otomatik testler (EditMode/PlayMode, RIMA'nin `*ForValidation` data-proof precedenti ile) bu hata sinifini yakalar? -> ayni tusa iki arac (F2 cakismasi), keybind-tetiklenmiyor (layout), Build Mode acilmiyor, overlap-hide, placement iso-dogru, tool exclusivity/no-double-place, walkability, save/load roundtrip. + "tek in-play-tool kayit/tus-sahipligi" guard'i (iki arac ayni tusu kapamaz) onerebilir misin? RIMA'da hazir test infra: Unity Test Framework, `*ForValidation` methodlari.
3) SONSUZ/GENISLEYEN MAP + ayarlar: RoomTemplateSO.bounds genisletme, dizi buyutme, kamera/grid sinirlari — fizibilite + yaklasim + risk.
4) TMP YAZI BOZULMASI (garbled UI, kronik): kok fix (static font atlas?) simdi mi yapilmali, mitigate mi? RIMA'da TMP font asset(ler)i nerede, ayar ne?
5) Demo ~1 hafta riski: bu konsolidasyon calisan parcalari bozar mi? En guvenli sira?

Cikti: konsolidasyon plani + somut test suite listesi + AL/SONRA/ATLA. CODEX_DONE.md'ye yaz.
