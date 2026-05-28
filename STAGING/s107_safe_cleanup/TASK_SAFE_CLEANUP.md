# TASK: Safe Cleanup — Kesin Junk Sadece (Conservative Move-to-Archive)

ACTIVE RULES: (1) think before deleting (2) min surgical action (3) MOVE not DELETE (geri alınabilir) (4) BLOCKED if unsure.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: RIMA proje root + STAGING klasöründeki **kesin junk** dosyaları `_archive/` altına taşı (silme — geri alınabilir olsun). Sadece KESIN listeyi taşı, şüpheli her şeyi BIRAK. Assets/Sprites + Anchors + MEMORY ellenmez (ayrı dispatch konuşulacak).

## Yapılacaklar

### Adım 1: Root-level junk → `_archive_root_junk_2026_05_26/`
Aşağıdaki **22 dosyayı** taşı (path: `F:/Antigravity Projeler/2d roguelite/RIMA/`):
- `AGY_DONE.md`
- `AGY_DONE_laurethayday.md`
- `AGY_DONE_laurethgame.md`
- `AGY_DONE_yasinderyabilgin.md`
- `AGY_DONE_ydbilgin.md`
- `AGY_DONE_ydbilginn.md`
- `ANTIGRAVITY_DONE.md`
- `CODEX_DONE.md`
- `CODEX_DONE_laurethayday.md`
- `CODEX_DONE_laurethgame.md`
- `CODEX_DONE_yasinderyabilgin.md`
- `CODEX.md`
- `CODEX_DISPATCH.md`
- `CODEX_TASK.md`
- `CODEX_TASK_laurethayday.md`
- `CODEX_TASK_laurethgame.md`
- `CODEX_TASK_splatpaint_executor.md`
- `CODEX_TASK_yasinderyabilgin.md`
- `CODEX_TASK_ydbilgin.md`
- `claude_code_flat_topdown_plan.md`
- `codex_imagegen_asset_packs_DONE.md`
- `shader_blend_spec.md`

Plus opsiyonel (kontrol ederek):
- `NEXT_SESSION_BRIEF.md` — eğer içeriği CURRENT_STATUS ile redundant ise taşı, değilse bırak

### Adım 2: STAGING junk → `STAGING/_archive/s107_pre_cleanup_2026_05_26/`
STAGING klasöründeki DONE/REVIEW/eski dosyaları taşı. **TAŞINACAK pattern** (case-insensitive):
- `*_DONE.md`, `*_DONE.txt`
- `CODEX_REVIEW_*` (eski codex review)
- `CODEX_TASK_*` (root-level değil, STAGING'deki)
- `CODEX_EXEC_*`
- `CODEX_TECH_REPORT_*`
- `CODEX_PRO_*`
- `CODEX_STRATEGIC_*`
- `CODEX_TWEET_*`
- `CODEX_OVERNIGHT_*`
- `CODEX_PHASE_*`
- `CODEX_IMAGEGEN_*` (her ikisi de — eski)
- `GEMINI_DONE_*`
- `EDITMODE_TESTS_DONE.txt`
- `EditModeTestRun_*.log`

**TUTULACAK pattern (taşıma!):**
- `STAGING/s106_overnight/` — bu session referansları
- `STAGING/s107_*/` — current session
- `STAGING/_archive/` — zaten arşiv
- `STAGING/CLEANUP_*` — meta cleanup analizleri
- `STAGING/BG_LAYER_ARCHITECTURE_VERDICT.md` — kanonik
- `STAGING/KARAR_*` — kanonik karar
- `STAGING/CHATGPT_RIMA_*` — referans
- `STAGING/CROSSCLASS_TIER_SPEC_*` — kanonik
- `STAGING/EPIC_MECHANIC_*` — opus design verdict
- `STAGING/IMAGEGEN_OUTPUTS/` — asset (eğer aktif kullanılıyorsa)
- `STAGING/CHATGPT_TOPDOWN/`, `STAGING/Chatgpt_walless/` — referans klasörler

**ŞÜPHELIYSE TAŞIMA — bırak.** "Belki kullanılır" yaklaşımı.

### Adım 3: Verify ve raporla
- Taşınan dosya sayısı (her adım için)
- `_archive_root_junk_2026_05_26/` ve `STAGING/_archive/s107_pre_cleanup_2026_05_26/` klasörlerinin son içeriği (ilk 10 dosya sample)
- Root'ta kalan .md dosyaları liste (KAL listesi doğru mu)
- BLOCKED varsa neden (dosya zaten yok, permission vb.)

## YASAK BÖLGELER (kesinlikle dokunma)
- `Assets/` — hiçbir şey
- `MEMORY/` (proje root)
- `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/` — auto-memory
- `Packages/`
- `ProjectSettings/`
- `Library/`
- `.git/`
- `.claude/`
- `*.unity`, `*.cs`, `*.asset`, `*.prefab`
- `CURRENT_STATUS.md`, `CLAUDE.md`, `AGENTS.md`, `RULES.md`, `README.md`, `SYSTEM_MAP.md`

## Hard Constraints
- **MOVE not DELETE** — `mv` kullan, `rm` ASLA
- **Backward-compat** zaten _archive korur (gerek varsa geri taşı)
- Plus dosya taşıma sırasında `.meta` dosyalarını da BIRLIKTE taşı (Unity asset değil ama .md de olabilir — kontrol et)
- Commit YAPMA
- Git operasyonu YAPMA

## Inline rapor (<300 kelime)
- Root junk: X dosya taşındı / Y bırakıldı
- STAGING: X dosya taşındı / Y bırakıldı
- Yeni `_archive` klasörleri path + total file count
- BLOCKED veya beklenmedik durumlar
