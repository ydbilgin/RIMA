ACTIVE RULES: (1) önce düşün (2) DÜZENLEME YOK — sadece REVIEW (3) cerrahi: yalnız bu batch'in diff'i (4) belirsizse BLOCKED.
NLM ACCESS: gerekmiyor (kod/diff review). Gerekirse: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"`.

READ-ONLY HARD: HİÇBİR dosya/kod düzenleme, HİÇBİR git komutu (add/commit/checkout/restore) ÇALIŞTIRMA. (Geçmişte advisor rogue `git add .` yaptı — KESİNLİKLE YASAK.) `git diff`/`git status`/`git log` OKUMA komutları SERBEST. İzin verilen tek yazma: kendi bulgu dosyan.

## ⚠️ ADVERSARIAL MANDATE — KRİTİK
RUBBER-STAMP DEĞİLSİN. Varsayılan tavrın ŞÜPHE. Bu batch'i AI ajanları üretti — HATA/REGRESYON YAPTIKLARINI VARSAY, aktif olarak KIRMAYA/ÇÜRÜTMEYE çalış. "Hepsi iyi görünüyor → APPROVE" = BAŞARISIZLIK. APPROVE vermeden önce gerçekten kırmaya çalıştığını KANITLA (denenen edge-case/senaryo + neden tutmadığı). En az 1 somut şüphe/risk noktası getir; hiç bulamıyorsan sağlamlığı KOD-KANITIYLA göster (his değil). Şüphede → APPROVE-WITH-FIXES veya REJECT, asla "kör APPROVE".

# Amaç
RIMA demo YARIN. Commit ÖNCESİ, bu session'ın TÜM uncommitted batch'ini KOD-CORRECTNESS lensinden bağımsız denetle. Onay verecek misin?

## İncele
- `git diff` + `git status` (uncommitted working tree). 6 grup:
  (1) `Assets/Animations/Characters/Elementalist/elementalist_idle_*.anim` (8 yön re-point)
  (2) `Assets/Scripts/Skills/Elementalist/*.cs` (13 skill SkillVfx + ArcaneBlast runtime-projectile fallback)
  (3) demo-safety: `ClassUnlockPolicy.cs` (IsDemoPlayable) + `ChamberSelectBootstrap.cs` + `CharacterSelectScreen.cs` + `CharacterSelectController.cs` + `PlayerClassManager.cs` + `Loc.cs`
  (4) `Assets/Scripts/Debug/DebugLogOverlay.cs` (new, F3) + `SkillBase.cs`/`SkillRuntime.cs` ([Cast]/[Damage]) + `DraftManager.cs` (pasif HUD toast + [Grant]) + `HUDController.cs` (ShowToast) + Warblade `DeepWound.cs`/`Earthsplitter.cs` ([Damage])
- Önceki auditor verdict'leri (referans, kendin doğrula): `STAGING/_process/2026-06/report_council/auditor_*.md` + builder raporları `STAGING/_process/2026-06/_done_*.md`

## Sorular (KANIT + dosya:satır)
1. **Regression riski:** Çalışan akışlar bozulmuş mu — Warblade/Elementalist play, reward→draft→grant, GATE/Boss? Diff'te bunları kıracak bir şey var mı?
2. **Director:** `PlayerClassManager.SetPrimaryClass` IsDemoPlayable gate'i DirectorMode'u (her sınıfı spawn edebilmeli) bozuyor mu? Bypass gerçek mi?
3. **Gate bütünlüğü:** IsDemoPlayable tüm class-select giriş noktalarında mı? Bypass kalan yol var mı?
4. **Yeni kod doğruluğu:** ArcaneBlast fallback (Fireball pattern), DraftManager pasif-grant path, DebugLogOverlay (logMessageReceived sub/unsub leak?), instrumentation — null-deref / yanlış overload / sızıntı?
5. **Genel:** Bu batch commit'e hazır mı?

## Çıktı
Bulguları YAZ: `STAGING/_process/2026-06/_council_cx_precommit_findings.md`. Dönüşte (stdout) ≤10 satır: **VERDICT (APPROVE / APPROVE-WITH-FIXES / REJECT)** + varsa P0/P1 + commit-öncesi MUTLAKA-düzelt listesi.
