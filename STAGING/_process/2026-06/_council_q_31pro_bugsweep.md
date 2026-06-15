# ax Gemini 3.1 Pro (High) — DEEP / architecture / systemic-risk lens

READ: STAGING/_process/2026-06/_council_bugsweep_2026-06-15.md (tam brief + scope + format ŞART) + scope'taki dosyalar (RewardPickup.cs, DraftManager.cs, RoomRunDirector.cs, BuildModeController.cs, BuildMode/BuildPlacementController.cs, DirectorMode.cs, Balance/DamageCalculator.cs).
GRAPHIFY: cross-file soruda graph.json (STAGING/_process/2026-06/graphify_fullmap/graphify-out/) sorgula, ~71× ucuz.

## DURUM
Sunum ~20 Haz. F2 GREEN, Build Mode bug-free, stat→damage math kanıtlı, Director overlay bleed FIX'lendi. Demo = data-proof/canlı (video YOK).

## SENİN LENS'İN: derin mimari + sistemik risk
Tara: state-machine tutarsızlıkları (Director Test↔Director, BuildMode toggle, draft active/pending flag'leri arası yarış/desync) · singleton/bootstrap pattern riskleri (DirectorMode RuntimeInitialize, DraftManager_Auto, çift-instance) · time-scale yönetimi (pause/resume, Director timeScale=0 ↔ draft ↔ BuildMode geçişleri) · kamera ownership devir/restore (BuildMode↔Director↔gameplay) · lifecycle/teardown leak'leri · golden-path'i bozabilecek gizli kuplaj.

Format: brief'teki KESİN format ([SEVERITY] file:line — sorun — demo etkisi). DEMO-BLOCKING öne. ≤15 madde. Spekülasyon değil, koda dayalı (SUSPECTED işaretle emin değilsen).
