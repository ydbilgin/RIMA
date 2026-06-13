# EDITOR CONSOLIDATION + TEST STRATEGY — KARAR (Council, 2026-06-13)

> Sorun: RIMA'da 3 ORTUSEN map editoru cakisiyor (eski IMGUI `InPlayMapPaintOverlay` F2 + benim uGUI Build Mode F2 + edit-mode `UnifiedMapDesigner`). Kullanici: tek GUZEL+entegre+SONSUZ-map+TEST-korumali editor; "tekrar bu sorunla (cakisma/keybind/overlap) karsilasmayalim".
> Advisorlar: cx (feasibility/reuse, gercek kod) + ax 3.1 Pro (mimari/test) + ax 3.5 Flash (lean). Ham: `_process/2026-06/_council_*_editor_consolidation.md` + `CODEX_DONE_yekta.md`.

## ANA KARAR: hedef = uGUI on + UnifiedDesignerCore arka; AMA demo'da KADEMELI + LEAN (zaman-ekseni cozumu)
Pro "tam birlestir + ICommand/ITool/GlobalInputRegistry" dedi; Flash "erken-soyutlama tuzagi, calisani bozma"; cx ikisini tartti: hedef Pro'nun, ama tek-seferde RoomData merge RoomTemplateSO-first runtime'i (IsoRoomBuilder/WalkabilityMap/RoomRunDirector/JSON/camera) riske atar -> **demo'da LEAN, post-demo'da kademeli merge.**

## AL — demo (cx'in guvenli sirasiyla)
1. **Tek tus-sahipligi guard'i** (hafif key-ownership registry: F2 owner kaydet; ikinci kayit ANLASILIR hata). BuildModeController = tek F2 sahibi.
2. **Eski IMGUI overlay'i emekliye ayir** — `InPlayMapPaintOverlay` bootstrap + F2 polling'i devre disi (SILME; legacy-define arkasina al -> compile error riski yok).
3. **Build Mode SAVE/apply** — working-copy RoomTemplateSO -> source (`EditorUtility.CopySerialized`+SetDirty+SaveAssets, Editor-only). RoomTemplateSO yolu KORUNUR (RoomData merge YOK simdi).
4. **TMP fix (okunabilirlik):** LiberationSans SDF-Fallback'i TMP Settings + Jersey10 fallback listesine BAGLA; Build Mode label'lari ASCII-safe. (Garbled yazi cozulur.)
5. **TEST SUITE (kullanici #1 istegi)** — mevcut `Assets/Tests/EditMode/**` infra'sini genislet:
   - Key-ownership: F2 owner kaydi basarili; ikinci owner kaydi FAIL; tek owner re-register.
   - F2 toggle (PlayMode): F2->IsActive true; tekrar->false+canvas restore; eski overlay absent/inactive.
   - Enter/exit: WorkingTemplate olusur/yikilir; source asset save'den ONCE kirlenMEZ; explicit save sonrasi reload korunur.
   - Overlap-hide: enter sadece non-own root canvas'lari kapatir; exit TAM olarak ayni seti acar; onceden kapali kalan kapali kalir.
   - Tool exclusivity/no-double-place (ForValidation): Prop modu sadece prop, Tile modu sadece tile/walkability; shared undo.
   - Iso-placement: PlaceForValidation -> grid.GetCellCenterWorld (dikdortgen matematik YOK); erase iso-cell.
   - Walkability/overlay (ForValidation): PaintFloor->walkable+tile; Erase->void; ToggleWalkable; PaintOverlay; UsesWorkingCopy (no-pollution).
   - Save/load roundtrip: edit->save->reload props/walkable/overlay korunur; JSON exporter Y-flip dogru.
   **Builder testleri YAZAR + `run_tests` ile KOSAR + yesil raporlar.**
6. **Cila:** paleti AYDINLAT (kullanici "cok karanlik" dedi) + gorsel **iso-grid overlay** (kullanici "grid yok" dedi).
7. **Map buyutme:** `RoomTemplateBoundsUtility.ResizePreserveCells` (hucreyi MUTLAK koordinatla koru) + "Expand Bounds" arac + max-cap. Gercek-sonsuz DEGIL; "buyuk ama genisletilebilir". (Vakit kalirsa; cekirdek+test once.)

## SONRA (post-demo)
UnifiedDesignerCore'a kademeli merge (floor/cliff -> props -> portal/light) · RoomData<->RoomTemplateSO yakinsama veya adapter · production static TMP atlas (tam Turkce glyph) · eski overlay dosyasini SIL (replace+test yesilken) · gercek chunk/sonsuz · package extraction · GlobalInputRegistry framework (hafif guard yeterli).

## ATLA (demo)
Build Mode'da light/decal (gerekmedikce) · gercek-sonsuz chunk streaming · buyuk UI redesign · F2/save/test guard'i oturmadan IsoRoomBuilder/RoomRunDirector genis refactor · ICommand/ITool jenerik framework.

## RISK (cx) + en guvenli sira
RoomData/RoomTemplate tam yakinsama = en yuksek risk (post-demo). F2 fix = dusuk risk, ILK. Save = orta ama sart. IMGUI: once devre-disi, testler yesilken sil. Sira: guard -> F2 tek-sahip -> IMGUI emekli -> regresyon testleri -> Save -> roundtrip test -> (post) Core merge.

NOT: cx yine `yekta` (DISABLED olmali) profilinde kostu — kota kontrol.
