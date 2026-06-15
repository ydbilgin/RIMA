# Demo Batch-Fix Result — 2026-06-15

## Özet
6/6 fix uygulandı. Console: 0 derleme hatası (1 MCP-bridge disposed-object = tooling artifact, önceden-var, Unity hatası değil).

---

## FIX-1 — DraftManager lambda leak ✅
**Dosya:** `Assets/Scripts/Skills/DraftManager.cs`
- L113-124: Anonim lambda → `OnSecondaryClassSelectedDraft(ClassType _)` named private method
- L101-105 (OnDisable): `PlayerClassManager.Instance.OnSecondaryClassSelected -= OnSecondaryClassSelectedDraft` eklendi
- Yeni `OnDestroy()` metodu eklendi: aynı unsubscribe

## FIX-2 — Build search-field hotkey guard ✅
**Dosya:** `Assets/Scripts/UI/BuildMode/BuildPlacementController.cs`
- L295 (HandleKeyboard başı): `if (searchField != null && searchField.isFocused) return;` eklendi
- Arama kutusuna yazarken F/E/bracket/Ctrl+Z tetiklenmez

## FIX-3 — ShowDraft re-entry guard ✅
**Dosya:** `Assets/Scripts/Skills/DraftManager.cs`
- L195 (ShowDraft başı, IsDraftPending=false'dan önce): `if (IsDraftActive) return;` eklendi
- Çift-draft softlock önlendi

## FIX-4 — EnterBuildMode modal guard ✅
**Dosya:** `Assets/Scripts/UI/BuildModeController.cs`
- L211 (EnterBuildMode, IsBuildModeActive check'inden sonra): UIManager.IsAnyOverlayOpen || DraftManager.IsDraftActive || IsDraftPending guard eklendi
- Draft/overlay açıkken F2 ile Build Mode'a girme engellendi

## FIX-5 — DirectorMode camera target reset ✅
**Dosya:** `Assets/Scripts/UI/DirectorMode.cs`
- L320-325 (SetState else-branch — overlay-fix'in eklediği blok MEVCUT): `hasCameraTarget = false;` eklendi
- Director'dan çıkınca kamera hedefi sıfırlanır → 2. odada eski koordinata uçma önlenir

## FIX-6 — Opening-draft coroutine retention ✅
**Dosya:** `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs`
- L144: `private Coroutine openingDraftSequence;` field eklendi
- L209: `openingDraftSequence = StartCoroutine(OpeningKitDraftSequence());`
- L277 (coroutine sonu): `openingDraftSequence = null;`
- L1737 (StopClearSequences): openingDraftSequence stop+null bloğu eklendi

---

## Console durumu
- Derleme hatası: **0**
- Uyarı: 0 (yeni)
- Tek mesaj: MCP bridge "Client handler error: Cannot access a disposed object" — domain reload sırasında oluşan tooling artifact, önceden-var, script kaynaklı değil.

## Golden-path koruma notları
- FIX-1: Unsubscribe OnDisable+OnDestroy → multi-take'te birden fazla draft açılmaz, tek normal draft yolu korundu
- FIX-2: searchField.isFocused check → search kutusundan çıkınca hotkey'ler normal çalışır
- FIX-3: IsDraftActive guard → ShowDraftWithSkill'deki mevcut guard ile tutarlı
- FIX-4: Overlay guard early-return → IsBuildModeActive=true olmadan return, normal toggle akışı bozulmaz
- FIX-5: hasCameraTarget=false → CacheCameraTarget() Director'a girince tekrar true yapar, normal davranış korunur
- FIX-6: openingDraftSequence null → StopClearSequences güvenli çağırılabilir, BeginRun'da yeni coroutine referans alır
