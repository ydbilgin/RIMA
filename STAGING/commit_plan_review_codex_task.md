ACTIVE RULES: (1) think before acting (2) min, no speculation (3) surgical — sadece review + git komut önerisi, COMMIT ATMA (4) BLOCKED if unclear.

# GÖREV: Commit planını review et + risk doğrula (Codex, xhigh)

RIMA repo'da 3150 uncommitted değişiklik birikmiş (önceki session'lar). Orchestrator (Claude) mantıksal-grup commit planı yaptı. Sen DOĞRULA + riskleri flag'le. **HİÇBİR COMMIT/PUSH ATMA** — sadece analiz + önerilen git komut dizisi döndür.

Repo: `F:/Antigravity Projeler/2d roguelite/RIMA` (branch master, remote origin github ydbilgin/RIMA).

## Git status özeti (orchestrator'ın bulduğu)
- CODE 257 [M20 D137 yeni100] — Assets/Scripts + Assets/Editor (dead script silme + parallax kodu)
- ART 899 [M75 D677 yeni147] — Assets/Prefabs/Scenes/Art (eski placeholder silme + kamera)
- DOCS 33 — CURRENT_STATUS, MEMORY/*, TASARIM/*, .claude/PROJECT_RULES.md
- STAGING 1292 [M1 D1005 yeni268 R29] — lock doc'lar + eski task/done silme
- OTHER 668: **Packages/com.coplaydev.unity-mcp 603 [M589 yeni14]** + root dispatch dosyaları + backup klasörleri

## Orchestrator'ın 3-tier planı (DOĞRULA)
**Grup 1 — COMMIT (mantıksal gruplar):** docs / staging / code / art.
**Grup 2 — EXCLUDE:** `Packages/com.coplaydev.unity-mcp/` (589 modified). Hipotez: satır-sonu (CRLF/LF) churn, içerik değil (Editor.meta diff "8+/8−" = tüm dosya re-write). Vendored MCP for Unity paketi, kullanıcı işi değil.
**Grup 3 — UNTRACK + gitignore:** root dispatch artıkları (`CODEX_DONE_*.md`, `CODEX_TASK_*.md`, `AGY_DONE_*.md`, `.agy_dispatch_state.json`, `.agy_dispatch_relaunch.log`, `*.tmp`) + backup klasörleri (`_backup_*/`, `_archive_root_junk*/`, `.antigravitycli/`). `git rm --cached` + .gitignore.

## SENİN İŞİN (git komutları çalıştırarak DOĞRULA, ama commit/push ATMA)
1. **Grup 2 doğrula:** `git diff --stat Packages/com.coplaydev.unity-mcp/ | tail -5` + 2-3 dosyada `git diff --numstat` ve içerik örneği → gerçekten satır-sonu churn mı yoksa anlamlı kod değişikliği mi? Eğer line-ending ise: dışlamak doğru mu, yoksa `.gitattributes` + renormalize daha mı iyi? Öner.
2. **Grup 3 güvenlik:** `cx_dispatch.py`, `agy_dispatch.py`, herhangi bir script veya workflow TRACKED `CODEX_DONE*.md` / `CODEX_TASK_*.md` dosyalarını OKUYOR mu (grep)? Untrack edersem bir şey kırılır mı? CODEX_DONE/CODEX_TASK ailesi gerçekten transient mi?
3. **Grup 1 hijyen:** code/art/docs/staging gruplarında commit'lenMEMESİ gereken bir şey var mı? Secret (.env, key, token), büyük binary, yanlışlıkla eklenen dosya? (`git status` + dosya adlarını tara.)
4. **Önerilen git komut dizisi:** Her grup için tam `git add -- ...` + `git commit -m "type(scope): ..."` dizisi (silinen dosyalar otomatik `git add` ile staged olur, `git rm` gerekmez normalde — doğrula). Grup 3 için `git rm --cached` + .gitignore satırları.

## DELIVERABLE
`CODEX_DONE.md`'ye yaz + kısa: (a) Grup 2 line-ending mi? exclude mi .gitattributes mi? (b) Grup 3 untrack güvenli mi, hangi dosya riskli? (c) Grup 1'de tehlike var mı? (d) önerilen tam git komut dizisi. COMMIT/PUSH ATMADIN, teyit et.