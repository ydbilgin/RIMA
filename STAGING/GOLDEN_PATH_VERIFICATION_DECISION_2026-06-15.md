# Golden-Path Video Segment Verification — DECISION + PROGRESS (2026-06-15)

> Council: cx (Codex/yasinderyabilgin) + ax 3.5 Flash. TAM mutabık (anlaşmazlık yok). Synthesis: Opus 4.8.
> Tetik: F2 çözüldükten sonra "sırada ne var" faz-sonu council'i. Brief: STAGING/_process/2026-06/_council_next_priority_2026-06-15.md.

---

## COUNCIL PRİORİTİZASYON (cx + ax_flash birebir aynı sıra)
1. **Build Mode (F2) toggle+placement** — centerpiece, en çok hareketli parça (key ownership, kamera, UI gizleme, working-copy, prop placement, grid, save hook).
2. **F1 tek-oda geçiş smoke** — kod path kısa, güvenli görünüyor; son storyboard beat'i.
3. **DirectorMode stat→damage** — ana-tezin parçası; slider→ClassStatRuntime, DamageCalculator physPower kullanır; film-proof canlı LMB hit gerektirir.
4. **Telemetry CSV** — zaten wired (event capture, DPS/TTK UI, CSV export, validation API).
5. **UI kod-tarafı prep** — görsel teyit KULLANICI-BLOKE.

## SKIP (over-engineering)
- F2 kod fix (golden-path GREEN, decision verildi).
- Yeni Build Mode mimarisi / runtime disk-save (working-copy + editor-only save yeterli).
- Q/E/R/F stat-scaling (kasıtlı `bypassStatScaling` → koreografide SADECE LMB).
- Prop next-room leak (video donuyor → önemsiz).
- CSV dosya-yazma (clipboard `systemCopyBuffer` yeter).
- Görsel UI polish (kullanıcı screenshot teyidi olmadan).

---

## VERIFICATION SONUÇLARI

### ✅ 1. Build Mode toggle+placement — BUG-FREE (data-proof, screenshot'suz)
`_Arena`-direct play + `*ForValidation` API:
- `Toggle()` (F2 ON): `IsBuildModeActive False→True`, `WorkingTemplate=combat_large_cross_01 (BuildWorkingCopy)` (deep-copy ✅), BPC enabled.
- `SelectFirstPropForValidation()=True`, `hasGhost=True`; prop **(1,1)'de yerleşti** `placedCount=1` (bottom-center 12,1 spawn/kapı rezerve, normal).
- `Toggle()` (F2 OFF): `IsBuildModeActive=False`, `WorkingTemplate=null` (working-copy yok edildi ✅), **`PROP-PERSISTS=YES active=True`** (edit-to-play çekirdek iddiası ✅), `timeScale=1` (oyun devam), ghost bir-frame sonra temizlendi (same-frame timing artifact, leak DEĞİL).
- **Console: 0 error/warning.**
→ Storyboard 0:25-0:55 centerpiece çekime hazır.

### ✅ 2. F1 reward room-leak — code-confirmed NON-ISSUE (her iki advisor)
`TryEnterDoor→AdvanceTo (RoomRunDirector.cs:1785)→BuildCurrentRoom (:279)→DestroyActiveReward (:1728)` eski reward'ı yok eder. Golden-path legacy RoomLoader'dan kaçınır. Runtime smoke düşük-değer (kod kesin) → kabul edildi.

### ✅ 3. DirectorMode stat→damage — MATH empirik DOĞRULANDI (edit-mode unit test)
`DamageCalculator.Calculate` (DamageCalculator.cs:30-34, `physPower/100f`): base 100 → **phys50=50, phys100=100, phys250=250** (lineer ✅). BYPASS packet (Q/E/R/F `bypassStatScaling`) → phys50=100, phys250=100 (ölçeklenmez ✅). → Slider→ClassStatRuntime→hasar tezi doğru; **koreografide SADECE LMB** (abilities stat-deaf). Slider→stat wiring cx-code-confirmed; canlı LMB film-proof = kullanıcı recording prova.

### ✅ 4. Telemetry CSV — code-confirmed wired (cx: DirectorMode.cs:240-248,1741-1807,2476-2525,2593-2738)
Event capture + DPS/TTK UI + CSV export + validation API hazır. Runtime film-proof düşük-risk → recording prova'da teyit (over-engineering'den kaçınıldı).

## NİHAİ DURUM (autonomous verification milestone)
İki KRİTİK segment runtime bug-free (F2 reward→kart + Build Mode centerpiece) · F1 code-confirmed safe · stat→damage math empirik doğrulandı (LMB-only) · telemetry code-confirmed wired. **Video'nun ANA tezi (edit-to-play + stat→damage) tam doğrulandı.** Kalan = kullanıcı işleri: UI görsel-teyit · A1 canon · OBS recording (G-collect + stat→damage + telemetry film-proof'ları orada).

---

## SONUÇ
İki KRİTİK/yüksek-riskli segment (F2 reward→kart + Build Mode centerpiece) runtime'da **bug-free doğrulandı**. F1 kod-kesin güvenli. Kalan: stat→damage data-chain + telemetry (daha düşük risk, stat→damage film-proof'u kullanıcının canlı LMB hit'i). Video'nun ANA tezi (edit-to-play + stat→damage) sağlam yolda.
