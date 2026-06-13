# Director Mode Ekleri — Opus Uygulama Raporu (2026-06-13)

Sadece `Assets/Scripts/UI/DirectorMode.cs` değiştirildi. RoomRunDirector/PlayerClassManager/SkillRuntime'a DOKUNULMADI (yalnız mevcut public API çağrıldı; wrapper gerekmedi).

## ÖZELLİK 1 — DUAL-CLASS DRAFT butonu
- ClassSkill panel header'ına buton eklendi (`AddClassSkillPanel`, mor buton). Handler `TriggerDualClassDraft` → `PlayerClassManager.Instance.TriggerClassSelection()` (gate yolunun AYNISI; ClassSelectionUI bu event'e abone).
- Görünürlük: `RefreshDualClassDraftButton` (secondary==None iken aktif/görünür). Hook: `TriggerDualClassDraftForValidation`, `IsDualClassDraftAvailableForValidation`.
- **KABUL KRİTERİ — GEÇTİ (Play Mode):** availBefore=True, ctrl 1→2, secondary=Elementalist, ManaSystem=True, Elementalist controller enabled+4 slot, hostGo+selCanvasActive=True (seçim UI ekranda GERÇEKTEN açıldı), timeScale=0, seçim sonrası availAfter=False. Buton DEMODA KALDI.

## ÖZELLİK 2 — Stat preset butonları
- Stats panel'e 3 buton (TANK/GLASS CANNON/VARSAYILAN), `ApplyStatPreset` → mevcut `SetStatForValidation`→`OnStatSliderChanged` yolu (yeni stat yolu YOK, toast'lar doğal). Hook: `ApplyStatPresetForValidation`.
- Değerler slider min/max içinden. Play Mode: TANK maxHP=300/dmg=0.5, GLASS maxHP=30/phys=220/dmg=4.5, DEFAULT profil default'una döndü (maxHP=115). VARSAYILAN = `ResetStatsFromProfile` (Quick Reset ile aynı kaynak).

## DOĞRULAMA
- Recompile: 0 error (DirectorMode + genel).
- EditMode (RIMA.Tests.EditMode): 541 test, 12 fail — hepsi ilgisiz altsistemler (MapDesigner PNG, perf budget, MCP scene reflection, prefab, SubRoom DDOL). DirectorMode/DualClass/StatPreset fail'i YOK; yeni fail eklenmedi (delta=timing/DDOL flake).
- Play Mode'dan ÇIKILDI (stop onaylandı). Console: yalnız benign "Some objects were not cleaned up" teardown uyarısı (exception/NullRef/derleme hatası YOK).
- Git commit YAPILMADI.
