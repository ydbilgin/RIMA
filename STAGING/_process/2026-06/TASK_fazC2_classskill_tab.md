ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
DEMO TOOLS FAZ C2 — Director "Class&Skill" sekmesini doldur. 10 class anında swap + skill draft override + LMB/RMB ata. GATE: derleme 0 error + class swap canlı çalışıyor. Görsel doğruluk SABAHA.

# OKU (zorunlu)
1. `Assets/Scripts/UI/DirectorMode.cs` — Faz B/C3. Panel: ContentArea > ClassSkill (boş CanvasGroup). `DirectorMode.Instance`, panels dict, jersey10Font + chrome sprite. C3 Stats sekmesini ÖRNEK al (aynı kalıp: chrome, font, Loc).
2. `Assets/Scripts/Systems/PlayerClassManager.cs` — `SetPrimaryClass(ClassType)` (DOĞRULA imza), `ClassType` enum (10 class), `CurrentPrimaryStats`. Swap bunu çağırır → Faz A stat wiring otomatik tetiklenir.
3. `Assets/Scripts/Skills/DraftManager.cs` — skill draft/override API (DOĞRULA: skill atama metodu var mı). Skill override bunu kullanır.
4. `Assets/Scripts/UI/SkillBarUI.cs` + LMB/RMB binding (commit `78c18087` — BasicAttackProfile icon + SkillBarUI). LMB/RMB atama bunu kullanır.
5. `STAGING/SANDBOX_DIRECTOR_DECISION_2026-06-12.md` §4 Class&Skill satırı + §3 chrome (skill/preset kart=`reward_card`, palette hücre=`slot_normal`).
6. `Loc.cs` — `Loc.T()` key'le.

# İŞ — Class&Skill sekmesi
- **Class swap:** 10 class butonu/grid (palette `slot_normal`). Tıkla → `PlayerClassManager.SetPrimaryClass(type)`. Stat/visual/weapon Faz A wiring ile otomatik. Aktif class vurgulanır (`slot_active`).
- **Skill draft override:** aktif class'ın skill listesi → kart (`reward_card`). Skill seç/değiştir → `DraftManager` API (doğrula). Min-code: mevcut draft sistemini KULLAN, yeni skill sistemi yazma.
- **LMB/RMB ata:** seçili skill/basic-attack'ı LMB veya RMB slot'a bağla → `SkillBarUI` binding (`78c18087` yolları). Mevcut binding'i kullan.
- Chrome/font/Loc C3 kalıbıyla aynı.

# GATE + COMMIT
- `read_console` 0 error.
- UnityMCP doğrula: Director→Class&Skill sekmesi → class butonu tıkla → `PlayerClassManager.PrimaryClass` değişiyor + `CurrentPrimaryStats` güncelleniyor (execute_code assert). Skill override + LMB/RMB binding uygulanıyor.
- CombatContract `run_tests` 3/3 bozulmamalı. (Full suite'teki ilgisiz pre-existing failure'lara DOKUNMA.)
- Geçerse commit: `feat(director): Faz C2 Class&Skill tab — 10-class swap + skill draft + LMB/RMB assign [visual unverified]`. Geçmezse BLOCKED, COMMIT ETME.
- CODEX_DONE.md: ne eklendi, swap doğrulaması, hangi hook'lar kullanıldı (imza teyidi), derleme/test, commit hash. Hook imzası beklenenden farklıysa BLOCKED yaz (tahmin etme).

# YAPMA
- Diğer sekmeler YOK. Yeni skill/class/draft sistemi YOK — mevcut API'leri çağır. Stat wiring'i tekrar yazma (Faz A'da var). DraftManager/SkillBarUI imzası belirsizse BLOCKED.
