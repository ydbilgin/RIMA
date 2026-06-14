# RIMA — Claude Agent Rules (Full)
**Universal project rules: see `RULES.md`.**
Routing details: `AGENTS.md`.

## Universal Coding Principles (Karpathy 4 — TÜM agent'lar için)

1. **KOD YAZMADAN ÖNCE DÜŞÜN.** Varsayımlarını listele. Belirsizlik varsa flag at, körü körüne devam etme. Birden fazla yorum varsa hepsini sun, susarak seçim yapma.
2. **MİNİMUM KOD.** Problemi çözen en az kod. Spekülatif feature, gereksiz abstraction, "olur ihtimaline karşı" error handling YOK. "Senior engineer bunu overcomplicate görür mü?" testi.
3. **CERRAHİ DEĞİŞİKLİK.** Sadece görevin gerektirdiği dosyalara dokun. İlgisiz kodu refactor etme. Pre-existing dead code'u not düş, silme. Sadece kendi değişikliğinin oluşturduğu unused import'ları kaldır.
4. **HEDEF ODAKLI ÇALIŞMA.** Her görevi doğrulanabilir başarı kriterine çevir. Multi-step işte plan + verification step. Muğlak kriter → orchestrator'a sor, tahmin etme. Başarısızsa BLOCKED yaz, sessizce partial implement etme.

Sub-agent dispatch'inde her zaman ilk satır olarak inline ekle:
`ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.`

## HARD RULES (Always Active) — S58/S59 lessons

### Orchestrator Context Koruma (S63 UPDATED)
**ORCHESTRATOR BULK İŞ YAPMAZ.** Bu hard rule — istisna yok.

| Durum | Kural |
|---|---|
| Kod yazma/düzenleme / dosya batch | → `cx dispatch` (Codex, background) |
| Batch doc update / frontmatter ekleme | → `cx dispatch` (Codex, background) |
| Git commit | → `cx dispatch` içinde Codex yapar |
| Analiz / cross-ref / plan | → rima-sonnet dispatch |
| Kısa QC / tek dosya okuma | → orchestrator direkt |

**Dispatch formatı:** STAGING/'e task .md yaz → `cx dispatch --task-file ... --effort high` (run_in_background: true) → CODEX_DONE.md'den sonuç oku. (GLOBAL komut, 2026-06-05: CodexAuthManager `6dbe8d18`; RIMA-lokal cx_dispatch.py EMEKLİ.)

### Codex Görev Routing
1. **Tüm Codex görevleri (impl, review, UnityMCP dahil):** `cx dispatch` — sub-agent YOK.
   ```powershell
   # PowerShell tool, run_in_background: true — CWD=proje kökü (task/done dosyaları CWD'ye yazılır)
   cx dispatch --task-file "STAGING/task.md" --effort high
   # L-task'lerde: --timeout 3600 (default 1200s yetmez)
   ```
2. **Workflow:** CODEX_TASK.md'ye yaz (cx dispatch yapar) → cx exec → CODEX_DONE.md oku.
3. **Background zorunlu:** Her `cx dispatch` çağrısı `run_in_background: true` ile. Orchestrator bloklanmaz, notify gelince okur.
4. **UnityMCP:** Her profil config'de stdio MCP olarak tanımlı, otonom çalışır. **Unity açık olmalı.**
5. **Model:** gpt-5.5. Profil otomatik seçilir (`cx dispatch` — quota-aware, DISABLED atlar; yönetim: `cx enable/disable <profil>`).
6. **rima-codex agent:** KALDIRILDI. `cx dispatch` ile replace edildi.

### Asset Üretim (S59 LOCKED — 2026-05-12)
- **Pure 2D Top-Down chibi pixel art** mimarisi
- Karakter sprite: actual canvas **120x120** (Warblade verified 2026-05-24). "Chibi" = design intent terimi (proportion), canvas boyutu DEGIL. Karar #100 (64x64 chibi RESTORE) canvas tarafi REVISE; PixelLab Create Image Pro (master sheet) -> Create Character -> Custom Animation V3
- Tile: **32x32** top-down grid
- VFX: **64-128px mix** (küçük 64-80, ultimate 96-128)
- Yön: **8 yön LOCKED (Karar #114, 2026-05-13)** — 5 sprite üret (S, SE, E, NE, N), 3 mirror (W←E, SW←SE, NW←NE) Unity SpriteRenderer.flipX. Karar #53 + #88 (4-dir) REVOKED.
- Renderer: **URP 2D Renderer + Pixel Perfect Camera + 2D Lights**
- Anim view: **HIGH TOP-DOWN 3/4 (~70-80° from horizon, Hades / Children of Morta / Diablo III ref, Karar #114 reaffirm 2026-05-24). Camera close-up zoom for hero scale. NO iso projection math, NO true 45° diamond. Sprite 3/4 styling = sprite art choice, separate from camera angle.**
- Anim FPS: **10-12 fps**
- PPU: **64**
- **YASAK:** 2.5D mimarisi, 3D environment + billboard, 128px detaylı karakter, KayKit/Blender 3D pipeline (S57-S58 REVOKED)

### Diğer
5. **Konuşmayı bloklamadan çalış.** Background task'ları `run_in_background: true` ile çağır.
6. **Sub-agent açmadan önce düşün.** Küçük iş için doğrudan tool call yap.

### Iteration Cleanup — One LIVE Version Rule (S86_LATE LOCK 2026-05-17)

**Kural:** Bir dosya iterasyon yapılırsa (v1 → v2 → ... → vN), **sadece son LIVE versiyon ana dosyada kalır**, eski versiyonlar archive'a taşınır.

| Hedef | Kural |
|---|---|
| `STAGING/character_production_prompts.md` (örnek) | Tek LIVE versiyon kalır (v10). Eski v1-v9 → `STAGING/_archive/character_production_prompts_archive_v1-v9.md` |
| Sprint spec dosyaları | Sprint kapanınca DONE.md + spec aynı dosyada kalır; sonraki sprint için yeni dosya |
| Memory drift | NLM canonical > local memory > prompt iteration. Çakışmada NLM kazanır, local memory NLM'e uygun revize |

**Drift Hierarchy (kim doğru):**
1. **NLM canonical** (notebook ID = `.claude/nlm.local` gizli/gitignored — RIMA design docs source)
2. **Local memory** (MEMORY/ klasörü — point-in-time observations, 14+ days olunca stale risk)
3. **Prompt iteration / STAGING draft** (en oynak, drift kaynağı)

Çakışma → yukarıdan aşağıya geçerli kaynak kazanır. Local memory NLM'e drift gösterirse rima-doc dispatch ile sync edilir.

**Cleanup Checkpoint:**
- Her 5+ iterasyon sonunda `/lint` çalıştır
- Phase transition sonunda `/phase-close` protokolü
- Session sonunda büyük cleanup → `/nlm-sync push`
- STAGING/_archive/ ve memory/_archive/ standart klasörler — eski iterasyonlar oraya, asla silinmez (konservatif)

### Süreç Artifact Konvansiyonu (2026-06-07 LOCK — birikme önlemi)
**STAGING üst-seviyesi = SADECE LIVE dokümanlar:** `*_DECISION_*` · `*_PLAN/MASTER/SPEC/LOCK/AUDIT/BIBLE*` · aktif TASK'ler.

| Dosya türü | NEREYE yazılır |
|---|---|
| Council soruları/ham çıktıları (`_council_*`) | `STAGING/_process/<YYYY-MM>/` |
| Dispatch task dosyaları (cx/ax görevleri) | `STAGING/_process/<YYYY-MM>/` |
| `_done_* / _review_* / _research_* / _nlm_*` kayıtları | `STAGING/_process/<YYYY-MM>/` |
| TASK_* — iş TAMAMLANINCA | `STAGING/_process/<YYYY-MM>/` taşınır |
| Kararlar/spec'ler (kalıcı) | STAGING üst-seviye (LIVE) |

- `_process/` git-TRACKED'tir (izlenebilirlik korunur) ama `_` prefix'i sayesinde **nlm-sync taramaz** → NLM kirlenmez.
- Kaçak birikirse: `powershell STAGING/tools/archive_staging_process.ps1` (whitelist-desenli, idempotent, SİLMEZ sadece taşır).
- `/phase-close` checklist'ine dahildir. Tarihsel toplu arşiv = `STAGING/_archive/process_2026-06/` (2026-06-07 temizliği).

## Role: Orchestra Conductor (PRIMARY)
Claude dispatches work; does NOT do mechanical bulk itself.

- Hold project map via `MEMORY/INDEX.md`. Do NOT re-read files already read this session.
- Delegate to sub-agents with: (a) explicit file paths, (b) inline excerpts when small, (c) exact output format.
- Sub-agents do NOT auto-discover context. Orchestrator feeds them.
- **Mutual QC:** Claude reviews Codex/Gemini commits. Errors -> fix and re-commit.

## Session Start
Read `.claude/PROJECT_RULES.md` (bu dosya) + `CURRENT_STATUS.md`. Başka dosya okuma.

## Memory
Shared: `MEMORY/INDEX.md` -> open related `*.md` only when trigger matches current task.
All agents (Claude, Codex, Gemini) read from `MEMORY/`. All must commit after changes.

## Claude Skills (Slash Commands)
| Skill | When to use |
|---|---|
| `/plan` | Before any non-trivial implementation |
| `/lint` | Phase transitions, 5+ decisions, before asset work |
| `/phase-close` | Before moving to next phase |
| `/nlm` | NotebookLM knowledge query |
| `/playtest` | Generate Gemini-executed playtest scenarios |
| `/save-session` | Save session state for future resume |
| `/simplify` | Review changed code after Codex commits |
| `/update-config` | Configure Claude Code settings/hooks/permissions |
| `/accounts` | Check all Claude account rate limit status |

## Agent Routing (S61 — 2026-05-14) — ⚠️ KISMEN SUPERSEDED (2026-06-13)
> **GÜNCEL ROUTING (tek-gerçek):** Orchestrator=**Opus 4.8** · execute=**Opus sub-agent** · review=auditor-opus/cx (writer≠reviewer) · council=cx+ax 3.1 Pro+ax 3.5 Flash · `rima-codex` agent **KALDIRILDI → `cx dispatch`**. Kaynak: MEMORY `opus_executes_cx_reviews` + `review_routing_2026-06-13` + CURRENT_STATUS RESUME. Aşağıdaki S61 tablosu **tarihsel referans** (Sonnet-orkestra dönemi).

**(S61 tarihsel) Orchestrator = Sonnet (default).** Opus sadece gerçekten gerektiğinde.

### Model Seçim Kuralı
| Model | Ne zaman |
|---|---|
| **Sonnet (orchestrator)** | Her şey — varsayılan |
| **Codex (GPT-o3)** | Kod yazma, Unity değişiklik, mekanik batch iş — ANA YARDIMCI |
| **rima-sonnet (Sonnet sub-agent)** | Analiz, cross-ref, planlama, Codex prompt yazımı — Sonnet ama ayrı context |
| **rima-design (Opus)** | Sadece: 2+ sistem kesen karar VE orchestrator çözemiyor. Tek-sistem → Sonnet/rima-sonnet yeter |
| **Gemini** | Nadir: uzun araştırma, video synthesis, bulk doc analiz |

### Agent Kadrosu
| Agent | Model | Ne yapar |
|---|---|---|
| `rima-codex` | Sonnet | cx ile Codex'e dispatch, transcript döndürür |
| `rima-sonnet` | Sonnet | Analiz, planlama, cross-ref, Codex prompt yazımı |
| `rima-doc` | Sonnet | Docs/memory yazımı, CURRENT_STATUS sync |
| `rima-qc` | Sonnet | Codex çıktısı / görsel QC, PASS/FAIL |
| `rima-asset` | Sonnet | PixelLab prompt yazımı, asset üretim planı |
| `rima-research` | Sonnet | gemini -p router, ham çıktı döndürür |
| `rima-design` | **Opus** | 2+ sistem kesen derin design judgment — son çare |
| `Explore` | — | Fast read-only codebase search |

### Sub-Agent Spawn Eşiği (S61)
Sub-agent token overhead artık düşük → **serbestçe spawn et**. Eşik:
- **Doğrudan yap:** tek araç call, < 3 dosya okuma, < 5 satır düzenleme
- **Sub-agent spawn:** paralel iş, context koruma, uzmanlık gereken iş
- **Codex dispatch:** her türlü kod yazma/düzenleme (küçük bile olsa tercih et)

### Orchestrator Direkt Durumlar
- < 5 UnityMCP tool call → Claude direkt çağırır
- Kullanıcı "sen yap" derse → direkt yapar, agent açmaz

## NotebookLM — HARD RULE (Context Source)
**Proje dosyalarını direkt okuma. Tüm bağlam NotebookLM MCP üzerinden gelir.**

- **LIVE Notebook ID:** `.claude/nlm.local`'dan oku (gitignored, repo'ya konmaz; private memory'de de var)
- MCP tool (tercih varsa): `mcp__notebooklm__notebook_query`
- CLI fallback (her ortam çalışır): `uvx --from notebooklm-mcp-cli nlm notebook query $(cat .claude/nlm.local) "soru"`
- Dosya oku: **sadece** NotebookLM yetersiz kalırsa, sadece ilgili satır aralıklarını.
- **YASAK Notebook ID'ler:** `ed3c8952-417c-4988-84a7-425d25ba3b08`, `06a27df3-79e6-43da-a550-2937149af0a4` (eski, deprecated)

### Sub-Agent NLM Access — MANDATORY (S91 LOCK)
**Her sub-agent dispatch prompt'una ZORUNLU ekle (ilk satırlardan biri):**

```
NLM ACCESS: If you need RIMA design context, query NLM first via:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.
```

Bu satır yoksa sub-agent dosyaları kendi başına okur → context israfı + drift riski. Orchestrator dispatch öncesi prompt'u kontrol eder.

**İstisnalar (direkt oku, NLM'e gitme):**
- `CURRENT_STATUS.md` — session başında
- `.claude/PROJECT_RULES.md` — session başında
- `CODEX_DISPATCH.md` — Codex görevi yazarken
- `Assets/Scripts/**` — kod dosyaları NLM'de yok; Explore/Grep kullan

**NLM Sync Policy:**
| Dosya | Aksiyon |
|---|---|
| `TASARIM/*.md` | Hemen sync et |
| `MEMORY/*.md` | Hemen sync et |
| `CURRENT_STATUS.md` | Session sonunda sync |
| `CLAUDE.md`, `RULES.md` | Büyük değişimde sync |
| `Assets/Scripts/**` | Hiçbir zaman sync etme |

## NLM Auth Recovery
Auth expired → bash `nlm login` çalıştır → otomatik auth (Chrome açılır) → komutu tekrar dene.

## Token Saving — Context Economy E1-E8 (2026-06-13 LOCK, detay: memory `feedback_context_economy_rules.md`)
- Session start: PROJECT_RULES.md + CURRENT_STATUS.md only.
- Bulk mechanical work → Codex (GPT 5.5). Bulk analysis → Gemini.
- `/lint` at: phase transitions, 5+ decisions, before asset work.
- **E1:** Agent/dispatch çıktısı DOSYAYA yazılır; dönüş ≤10 satır özet + yol. Rapor içeriği dönüşe gömülmez. (Her dispatch brief'ine bu satır eklenir.)
- **E2:** 200+ satır dosya → Grep ile bölge bul + offset'li kısmi Read. Tam-Read sadece küçük dosyada; Grep'te head_limit.
- **E3:** Uzun iş = background + bildirim. Poll yok; output'un sadece tail'i okunur; sub-agent transcript'i asla.
- **E4:** Envanter/çok-dosyalı tarama = ax_flash → dosya; orchestrator sadece karar bölümünü okur.
- **E5:** Unity execute_code compact özet-string döndürür; screenshot dosyaya, gerekirse 1 Read.
- **E6:** Session içinde aynı dosya 2. kez Read edilmez; Edit-sonrası doğrulama-Read gereksiz.
- **E7:** Memory dosyası = tek gerçek ~30 satır + recall-kalite description; bayat dosya süpersede-notla derhal arşive.
- **E8:** Review PASS / faz geçişi / 20+ mesaj → CURRENT_STATUS lean güncelle + `/clear`; %70+ doluluk → `/save-session` öner.

## Output Economy
- **Mechanical work**: terse, fragments OK.
- **Design/decision work**: nuanced output.

## /clear
Call after: Review PASS, new phase, 20+ messages, heavy batch, topic switch.

## Multi-Account Routing
- **"AWS'deyim"** → Bedrock. Opus 4.6 decisions, Sonnet 4.6 mechanical.
- **"Asil hesabimdayim"** → Claude Pro.
