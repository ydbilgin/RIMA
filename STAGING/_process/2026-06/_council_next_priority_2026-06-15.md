# COUNCIL: F2 sonrası — golden-path sıradaki en yüksek-kaldıraç otonom iş + F1 riski + centerpiece sağlığı

ACTIVE RULES: (1) think (2) min/no-speculation (3) evidence file:line (4) BLOCKED if unclear.
GRAPHIFY: cross-file soruda önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/).

## DURUM
F2 "reward→kart" ÇÖZÜLDÜ: golden-path GREEN, 0 fix (2 canlı repro: gerçek reward+ForceCollect→3 kart). Detay: STAGING/F2_ROOTCAUSE_DECISION_2026-06-15.md. Sunum ~20 Haz. Video kullanıcı tarafından OBS ile kaydedilecek; benim işim her segmenti bug-free yapmak.

## STORYBOARD SHOT LIST (STAGING/EDIT_TO_PLAY_STORYBOARD_DECISION_2026-06-14.md) + durum
- 0:00-0:10 Graphify hook (data-viz, asset HAZIR)
- 0:10-0:25 _Arena açılır (çalışıyor)
- **0:25-0:55 F2 Build Mode toggle (BuildModeController) = CENTERPIECE WOW, "bug-free" şart — bu session DOĞRULANMADI**
- 0:55-1:25 DirectorMode Spawn + physPower slider + LMB damage (durum?)
- 1:25-1:50 reward→kart (F2 GREEN ✅)
- 1:50-2:10 Telemetry CSV (durum?)
- 2:10-2:20 sonraki oda freeze

## F1 STATİK BULGU (orchestrator)
Room-transition `TryEnterDoor→AdvanceTo (RoomRunDirector.cs:1785)→BuildCurrentRoom (:1801)→DestroyActiveReward (:307,1728)` eski reward'ı yok ediyor. Leak muhtemelen sadece legacy RoomLoader path'inde (storyboard "ondan kaçın" diyor). → F1 golden-path-güvenli olabilir.

## KULLANICI-BLOKE İŞLER (otonom YAPAMAM)
UI polish U1/U3/U4 (overlay UI MCP screenshot'ta çıkmaz→görsel teyit ŞART) · A1 arena (canon kararı kullanıcıda) · video kaydı (kullanıcı oynar+OBS).

## SORULAR
**Q1 (F1 golden-path):** Golden-path tek oda-geçişi (storyboard 2:10-2:20 "sonraki oda yüklenir") F1 leak'i tetikler mi, yoksa AdvanceTo→BuildCurrentRoom→DestroyActiveReward güvenli path'i mi kullanır? F1 golden-path için risk mi, F2 gibi non-issue mi? file:line.

**Q2 (CENTERPIECE):** BuildModeController F2-toggle (Build Mode gir/çık, prop yerleştir, AYNI odada devam) kod-seviyesinde wired+bug-free mi? Bilinen risk/eksik var mı? (Bu 0:25-0:55 = videonun en güçlü tez kanıtı.)

**Q3 (PRİORİTİZASYON):** F2 GREEN + kullanıcı-bloke işler verili — 20 Haz öncesi benim yapabileceğim EN YÜKSEK-KALDIRAÇ otonom iş ne? Sırala. (Adaylar: Build Mode toggle doğrula · F1 golden-path teyit · DirectorMode stat→damage segment doğrula · Telemetry CSV doğrula · UI fix'leri kod-tarafı hazırla→kullanıcı görsel teyit.)

**Q4 (LEAN — ne SKIP):** Aşırı-mühendislikten kaçınmak için NE yapılmamalı? Recordable video'ya en yalın yol ne?
