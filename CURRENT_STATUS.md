# CURRENT_STATUS

## ⏯️ RESUME (2026-06-18 DEMO-PREP FINALIZE — TAMAMLANDI) — demo ~19 Haziran (YARIN)

**Durum:** Demo-prep maratonu bitti, 6 commit master'a düştü (`f4bcc9ad`..`708cd810`). Rapor demo-hazır; oyun-içi araçlar + bug-fix'ler council-onaylı (ağırlıklı APPROVE-WITH-FIXES → fix'ler uygulandı).

### ✅ COMMIT (master, 6 grup)
- `f4bcc9ad` Elementalist 8-yön idle `.anim` → gerçek sprite (stale GUID fix)
- `8a03c756` Elementalist 13 skill **SkillVfx** + **ArcaneBlast** ölü-skill fix + damage telemetri
- `9dc1116c` **F3 DebugLogOverlay** (logMessageReceived + `[Cast]/[Damage]/[Grant]`) + pasif HUD toast
- `2784089b` demo-safety: **pedestal-lock** (IsDemoPlayable, 2 oynanabilir/8 kilitli) + **husk-fallback** + PlayerPrefs leak kapatıldı (Director bypass korundu)
- `8118c90f` rapor council-fix (metin + Şekil9 caption dürüst + DOCX regen) + SUNUM_QA
- `708cd810` process artifacts

### 🎯 RAPOR DEMO-HAZIR
Council CONDITIONAL → tüm must-fix uygulandı. DOCX 12 figür + kapak logo, masaüstü-sızıntısı yok, iddialar kodla doğrulandı (111/67 yetenek, test 549 savunulabilir, 4 controller / 2 oynanabilir, boss bespoke). Savunma: `STAGING/report/SUNUM_QA_VE_CERCEVE_2026-06-18.md`. Karar: `STAGING/PRECOMMIT_COUNCIL_DECISION_2026-06-18.md`.

### 🧰 YENİ ARAÇ: F3 log overlay
Play'de **F3** = ekranda Debug.Log/Warning/Error (kırmızı/sarı) + `[Cast]/[Damage]/[Grant]` event'leri = demo geri-bildirim + canlı hata-avı. `Assets/Scripts/Debug/DebugLogOverlay.cs`.

### 🎮 SKILL/VFX FIX (workflow `wf_79468e61` — playtest-PASS + auditor PASS)
- **LMB basic-atak VFX** (`Assets/Scripts/Combat/BasicAttack/CastRhythmBehavior.cs`) — Elementalist sol-click artık VFX'li.
- **Skiller dummy'e atılıyor+vuruyor** = chamber/practice'te skill slotları BOŞTU (skiller run'da draft'la gelir) → `ChamberSelectBootstrap.GrantPracticeLoadout` seçili sınıfın Q/E/R/F'sini doldurur (= "karakter seçerken skill deneme" özelliği). **Run-start boş-loadout + pedestal-gate'e DOKUNULMADI** (chamber-only/additive; auditor 3-katman doğruladı).
- ArcaneBlast gerçek-oynanışta doğrulandı (HP −35). Tüm Q/E/R/F+LMB cast+hit, canlı `[Cast]/[Damage]/[Grant]`.
- ⚠️ Skiller **FARE-nişanlı** → dummy'i vurmak için fareyi dummy'e tut (bug değil).

### 🖥️ EDİTÖR STABİLİTE (demo EDİTÖRDE olacak — build değil)
- Hang = native **D3D11 `Assertion failed: SUCCEEDED(hr)`** (RTX 5080) + uncapped FPS + eşzamanlı MCP yükü. **KOD DEĞİL** (census temiz: 0 sızıntı, overlay/SkillVfx temiz).
- **`Assets/Scripts/Core/FrameRateGuard.cs`** = 60 FPS hard-cap (vSync=0+targetFPS=60) → GPU thrash kalkar.
- **DEMO CHECKLIST:** (1) Play'i bir kez durdur → recompile → taze Play (cap+F3-overlay+commit'li kod yüklenir) · (2) **canlı demoda MCP'yi KAPAT** (Claude'u aktif tutma; eşzamanlı bridge yükü = stall'ın muhtemel ana tetikleyicisi) · (3) hâlâ takılırsa: editör grafik API → **D3D12** (projede destekli, restart) + NVIDIA RTX 5080 sürücü güncelle.
- ✅ **`timeScale=0` donma KÖKÜ BULUNDU + FİX (`eb3a16cd`):** dual-owner race — obsolete `HitStop` (MarkPulseBehavior basic-atak) ham `timeScale=0` yapıyor, ardından `HitPauseDriver.PauseRoutine` `previousTimeScale`'i 0 iken yakalayıp 0'a "restore" ediyor → KALICI DONMA (combat'taki "takılı kalıyor"). Fix: MarkPulse → `HitPauseDriver.TriggerPause(0.03f)` (tek guard'lı sahip). Logic-kanıtlı + validate temiz; gerçek-oynanışta teyit önerilir. Post-demo: 4 Ronin HitStop çağrısı (demo-dışı sınıf) + `HitStop` emekliye ayır.

### ⏸️ ERTELENEN (post-demo) — Task #1/#3
**Silah mount × animasyon:** silah pivot bıçak-merkezi + per-yön hand-socket verisi 8 yönden 1'inde var → gerçek el-hizalama = weaponless-anim (kilitli) işi. Şekil 9 caption buna göre dürüstleştirildi; demo statik figürle idare.

### 🛑 DOKUNMA / NOT
- Working tree'de KALAN (BENİM DEĞİL, dokunma): `Jersey10 SDF` (M) · `Assets/_Recovery/0 (2).unity`+meta (Unity crash artifact) · `capture_v3.zip`.
- Locked sistemler: GATE / Boss-akış / reward-bleed / Build-çekirdek / weaponless-anim / branching-seed.
- ⚠️ **Uzun `_Arena` Play session Unity ana-thread'i STALL ediyor** (bu session 1× crash) → playtest'te `execute_code` data-proof tercih et, Play kısa tut.
- Council oy ağırlığı: cx2/opus2/ax_pro1/ax_flash0.5 (memory `feedback_council_vote_weighting`).
