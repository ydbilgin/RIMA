# RIMA — Claude Agent Rules (Full)
**Universal project rules: see `RULES.md`.**
Routing details: `AGENTS.md`.

## HARD RULES (Always Active) — S58/S59 lessons

### Orchestrator Context Koruma (S61 LOCKED)
**ORCHESTRATOR BULK İŞ YAPMAZ.** Bu hard rule — istisna yok.

| Durum | Kural |
|---|---|
| 3+ dosya okuma/yazma | → rima-codex dispatch |
| Herhangi bir kod yazma/düzenleme | → rima-codex dispatch (küçük bile olsa) |
| Batch doc update / frontmatter ekleme | → rima-codex dispatch |
| Git commit | → rima-codex dispatch |
| Analiz / cross-ref / plan | → rima-sonnet dispatch |
| Kısa QC / tek dosya okuma | → orchestrator direkt |

**Dispatch formatı:** Orchestrator görevi tanımlar + izin verilen dosya listesini verir → rima-codex çalıştırır → orchestrator sadece sonucu onaylar. Context şişmez.

### Codex Görev Routing
1. **UnityMCP gerekiyor mu?** EVET → CODEX_TASK.md'ye yaz → `rima-codex` agent spawn et.
2. **UnityMCP yok mu?** (kod yazma / araştırma / mekanik / dosya işlemi) → `/codex` skill ile **cx bash** üzerinden çağır. CODEX_TASK.md'ye ASLA yazma.
3. **cx wrapper** profil-bağlamlı Codex launcher'ı. 3 profil: `laurethgame` (ana), `laurethayday`, `yasinderyabilgin` (paralel/fallback). Non-interactive: `cx <profil> exec --prompt-file <dosya>`. **cx sadece PowerShell'de çalışır, Bash'te PATH'te yok.**
4. **rima-codex agent** → sadece Bash tool'u, ~2-3k token baseline. Büyük batch UnityMCP işi için. Küçük iş (<5 tool call) → Claude direkt UnityMCP çağır.

### Asset Üretim (S59 LOCKED — 2026-05-12)
- **Pure 2D Top-Down chibi pixel art** mimarisi
- Karakter sprite: **64x64 chibi**, PixelLab Create Character (Pixen)
- Tile: **32x32** top-down grid
- VFX: **64-128px mix** (küçük 64-80, ultimate 96-128)
- Yön (MVP): **4 yön** (N/S/E uretilir, W=flipX)
- Renderer: **URP 2D Renderer + Pixel Perfect Camera + 2D Lights**
- Anim view: **High top-down ~30-35° (Hades match)**
- Anim FPS: **10-12 fps**
- PPU: **64**
- **YASAK:** 2.5D mimarisi, 3D environment + billboard, 128px detaylı karakter, KayKit/Blender 3D pipeline (S57-S58 REVOKED)

### Diğer
5. **Konuşmayı bloklamadan çalış.** Background task'ları `run_in_background: true` ile çağır.
6. **Sub-agent açmadan önce düşün.** Küçük iş için doğrudan tool call yap.

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

## Agent Routing (S61 LOCKED — 2026-05-14)

**Orchestrator = Sonnet (default).** Opus sadece gerçekten gerektiğinde.

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

- MCP tool (tercih): `mcp__notebooklm__notebook_query`, notebook_id: `06a27df3-79e6-43da-a550-2937149af0a4`
- CLI fallback: `uvx --from notebooklm-mcp-cli nlm notebook query 06a27df3-79e6-43da-a550-2937149af0a4 "soru"`
- Dosya oku: **sadece** NotebookLM yetersiz kalırsa, sadece ilgili satır aralıklarını.
- **YASAK Notebook ID:** `ed3c8952-417c-4988-84a7-425d25ba3b08` (eski, deprecated)

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

## Token Saving
- Session start: PROJECT_RULES.md + CURRENT_STATUS.md only.
- Bulk mechanical work → Codex (GPT 5.5). Bulk analysis → Gemini (2.5 Pro).
- `/lint` at: phase transitions, 5+ decisions, before asset work.

## Output Economy
- **Mechanical work**: terse, fragments OK.
- **Design/decision work**: nuanced output.

## /clear
Call after: Review PASS, new phase, 20+ messages, heavy batch, topic switch.

## Multi-Account Routing
- **"AWS'deyim"** → Bedrock. Opus 4.6 decisions, Sonnet 4.6 mechanical.
- **"Asil hesabimdayim"** → Claude Pro.
