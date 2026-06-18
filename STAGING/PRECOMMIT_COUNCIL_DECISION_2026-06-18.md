# PRE-COMMIT COUNCIL DECISION — 2026-06-18 (demo YARIN)

**Karar mercii:** Orchestrator (Opus 4.8) · 3-lens adversarial council + kod-doğrulamalı sentez
**Panel:** cx (kod/diff) APPROVE-WITH-FIXES · ax Pro (mimari/figür-vision) **REJECT** · ax Flash (demo-risk) APPROVE-WITH-FIXES

## VERDICT: APPROVE-WITH-FIXES — commit'e HAZIR DEĞİL (4 madde düzeltilince hazır)

Adversarial mandate çalıştı (kör-APPROVE yok). ax Pro önceki auditor'ların kaçırdığı gerçek bir footgun buldu. ax Flash'ın must-fix listesi büyük ölçüde BU SESSION'da çözülmüş maddeleri tekrar etti (eski evidence okudu, güncel diff'i re-check etmedi) — kanıtla elendi.

## MUST-FIX (commit ÖNCESİ)
1. **[P0 kod] Husk-player fallback** — `PlayerClassManager.SetPrimaryClass`: non-demo sınıf APPLY yolunda (persisted SelectedClass + applyPrimaryOnStart) `LogWarning(); return;` → `ApplyPrimaryClassToPlayer` hiç çağrılmaz → sınıfsız/bozuk oyuncu. FIX: sessiz return yerine **Warblade'e fallback** + kurulumu tamamla. Director bypass + normal gate korunacak. (ax Pro KRİTİK; cx gate'i select-yolunda PASS etti ama bu apply-yolunu test etmedi.)
2. **[P1 figür/metin] Şekil 9 caption** — `fig_weapon_mount.png`: silah elde DEĞİL (sırtta/arkada). Caption "el-yuvası hizalama" iddiası jüri önünde yalanlanır. FIX: rapor caption'ını "silah-mount sistemi (silah karaktere bağlanıyor)" diye dürüstleştir, "el-yuvası hizalandı" iddiasını kaldır → DOCX regen. (Derin el-hizalama BLOCKED = post-demo Task #1.)
3. **[P1 kod] Telemetri tamlığı** — Fireball + SolarFlare + PrismBeam + FrostWall + IronCharge attacker'sız damage çağırıyor → SkillRuntime'ın player-tag gate'i `[Damage]` loglamıyor. Kullanıcının "her şeyi logla" hedefi için (özellikle Fireball) bariz boşluk. FIX: bu çağrıları `attacker: player.gameObject` + element ile geçir → tekdüze `[Damage]`. (cx P1, güncel diff.)
4. **[commit hijyeni] Selective stage** — blind-commit YOK. HARİÇ: `Assets/_Recovery/0 (2).unity`+meta (CRASH artifact!), `Assets/Fonts/Jersey10/...SDF.asset` (kullanıcının), `capture_v3.zip`. Junk sil: 2 `_fig_*_OLD*.png.bak`. (cx P1.)

## DOĞRULANDI / PASS (cx kod-kanıtlı, fix gerekmez)
Director bypass (DirectorBypassClassUnlock + try/finally) · class-select gate tüm giriş noktaları IsDemoPlayable · ArcaneBlast runtime fallback · pasif draft + ShowToast (ToastRoutine null-safe) · DebugLogOverlay sub/unsub (leak yok) · 8-yön clip GUID'leri · git diff --check temiz.

## STALE-NOISE (ax Flash, kanıtla ELENDİ — fix gerekmez)
- Çift-loglama → telemetri dedup'ta silindi (crafter, 0 hata)
- Şekil 6 void-leak → ax Pro fresh-vision **TEMİZ** + figür builder fix'ledi
- 549 vs 411 test → Opus karar-ajanı savunulabilir (ham method 675+); cevap SUNUM_QA'da
- 244↔705 Elementalist çelişki → crafter düzeltti (705 "8-yön entegre")

## DÜŞÜK/ERTELE
- DebugLogOverlay background-thread Queue lock (ax Pro) — demo için tolere; post-demo not
- HUD toast kuyruk-yok overlap (ax Flash) — draft tek-kart+time-paused, düşük risk

## PLAN
fix 1+3 (builder, Unity, fresh-Play verify) ∥ fix 2 (crafter, .md caption + DOCX regen, Unity'siz) → auditor gate → selective commit (madde 4).
