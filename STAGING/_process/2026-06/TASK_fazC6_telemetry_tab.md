ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ C6 — Director "Telemetry" sekmesini doldur. Hasar event'lerine abone ol → DPS/TTK + damage-source kırılımı canlı + CSV export. GATE: derleme 0 error + hasar verince telemetri güncelleniyor. Görsel doğruluk SABAHA.

# ⚠️ İLK İŞ — HASAR EVENT HOOK DOĞRULA (tahmin etme)
- Hasar nerede uygulanıyor? `Assets/Scripts/Skills/SkillRuntime.cs` DealDamage → `DamageCalculator.Calculate` → `Health` (TakeDamage). `Health.cs`'de hasar-alındı event'i VAR MI? Yoksa DamageCalculator/DealDamage sonucu yakalanabilecek tek nokta neresi?
- `DamagePacket` zaten `DamageSourceType { Unknown, LMB, RMB, Skill, Dot, Minion, Item }` taşıyor (telemetri için eklendi) + damageType + elementTag.
- Abone olunacak temiz event/hook YOKSA → minimal Director-only hook ekle (DealDamage sonucu → static event/callback). Belirsizse BLOCKED, yeni hasar sistemi YAZMA.

# OKU
1. `Assets/Scripts/UI/DirectorMode.cs` — Faz B/C1/C2/C3. Panel: Telemetry (boş CanvasGroup). C3 kalıbı (chrome/font/Loc).
2. `Assets/Scripts/Balance/DamagePacket.cs` + `DamageCalculator.cs` (DamageCalculationResult.finalDamage, DamageSourceType, DamageType, ElementTag).
3. `Assets/Scripts/Core/Health.cs` (hasar uygulama + ölüm — TTK için).
4. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` §4 Telemetry + §3 (alt telemetri şeridi `menu_button` stretched, Export `ribbon_base`).
5. `STAGING/DEMO_TOOLS_REPORT_AND_PLAN_2026-06-12.md` §8.4 (damage-number/source renkleri).
6. `Loc.cs`.

# İŞ — Telemetry sekmesi
- **Abone ol:** hasar uygulandığında (doğrulanan hook) → kaydet: zaman, finalDamage, DamageType, ElementTag, DamageSourceType, kaynak/hedef.
- **DPS:** son N saniye penceresi (toplam hasar / süre), canlı sayı.
- **TTK:** bir hedefin ilk-hasar→ölüm süresi (Health ölüm event'i).
- **Damage-source kırılımı:** LMB/RMB/Skill/Dot/... yüzde/bar (renkler §8.4).
- **CSV export:** biriken hasar event'leri → CSV string → pano/Debug.Log (+ opsiyonel dosya). `ribbon_base` buton.
- **Reset/Clear:** sayaçları sıfırla.
- Chrome/font/Loc C3 kalıbı. Abonelik mod çıkışında/yıkımında çöz (memory leak yok).

# GATE + COMMIT
- `read_console` 0 error.
- UnityMCP doğrula (Play, _Arena): mob spawn (C1) → hasar ver → telemetri sayacı artıyor (execute_code: DPS>0, source kırılımı dolu). CSV export string üretiyor.
- CombatContract `run_tests` 3/3 bozulmamalı.
- Geçerse commit: `feat(director): Faz C6 Telemetry tab — DPS/TTK + damage-source breakdown + CSV export [visual unverified]`. BLOCKED ise COMMIT ETME, sebep yaz.
- CODEX_DONE.md: hasar hook ne çıktı (mevcut event mi / minimal yeni hook mu), doğrulama, derleme/test, commit hash veya BLOCKED.

# YAPMA
- Diğer sekmeler YOK. Yeni hasar/event sistemi YOK — mevcut DealDamage/DamageCalculator akışına minimal abone ol. Abone olunacak nokta belirsizse BLOCKED. DamageSourceType'ı KULLAN (mevcut).
