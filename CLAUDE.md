# RIMA — Claude Agent Rules
**Universal project rules: see `RULES.md`.**
Routing details: `AGENTS.md`.

## Role: Orchestra Conductor (PRIMARY)
Claude dispatches work; does NOT do mechanical bulk itself.

- Hold project map via `MEMORY/INDEX.md`. Do NOT re-read files already read this session.
- Delegate to sub-agents with: (a) explicit file paths, (b) inline excerpts when small, (c) exact output format.
- Sub-agents do NOT auto-discover context. Orchestrator feeds them.
- **Mutual QC:** Claude reviews Codex/Gemini commits. Errors -> fix and re-commit. All revertible via git.

## Session Start
Read `CURRENT_STATUS.md` only. Continue from there. Pull other files on demand.

## Memory
Shared: `MEMORY/INDEX.md` -> open related `*.md` only when trigger matches current task.
All agents (Claude, Codex, Gemini) read from `MEMORY/`. All must commit after changes.

## Claude Skills (Slash Commands)
| Skill | When to use |
|---|---|
| `/plan` | Before any non-trivial implementation -- alignment first |
| `/lint` | Phase transitions, 5+ decisions, before asset work |
| `/phase-close` | Before moving to next phase |
| `/nlm` | NotebookLM knowledge query — tüm tasarım kararları |
| `/graphify` | Codebase knowledge graph (run on demand, not every session) |
| `/playtest` | Generate Gemini-executed playtest scenarios |
| `/save-session` | Save session state for future resume |
| `/simplify` | Review changed code for reuse/quality after Codex commits |
| `/update-config` | Configure Claude Code settings/hooks/permissions |
| `/accounts` | Check all Claude account rate limit status |

## Claude Sub-Agents
| Agent | Purpose |
|---|---|
| `rima-design` | Game design decisions, balance, combat systems |
| `rima-codex` | Dispatch tasks to Codex CLI (laurethgame / laurethayday / yasinderyabilgin) |
| `rima-doc` | Docs/memory updates, CURRENT_STATUS sync |
| `rima-qc` | QC/review of Codex output, images, cross-file consistency |
| `rima-asset` | Sprite/asset PixelLab prompts |
| `rima-research` | Web research, external docs (Unity, PixelLab API, etc.) |
| `Explore` | Fast read-only codebase search |
| `Plan` | Implementation planning before code |

## NotebookLM — HARD RULE (Context Source)
**Proje dosyalarını direkt okuma. Tüm bağlam NotebookLM MCP üzerinden gelir.**

- **Claude (orkestra şefi)** bağlamı sub-agent'lara besler — sub-agent'lar bağımsız sorgu yapmaz.
- MCP tool (tercih): `mcp__notebooklm__notebook_query`, notebook_id: `ed3c8952-417c-4988-84a7-425d25ba3b08`
- CLI fallback: `uvx --from notebooklm-mcp-cli nlm notebook query ed3c8952-417c-4988-84a7-425d25ba3b08 "soru"`
- Dosya oku: **sadece** NotebookLM yetersiz kalırsa ve sadece ilgili satır aralıklarını.
- Notebook: `RIMA Game Design Knowledge Base`. Detay: `MEMORY/notebooklm_workflow.md`

**İstisnalar (direkt oku, NLM'e gitme):**
- `CURRENT_STATUS.md` — session başında sadece bu okunur
- `CLAUDE.md` — harness otomatik yükler
- `CODEX_DISPATCH.md` — Codex görevi yazarken claude okur
- `Assets/Scripts/**` — kod dosyaları NLM'de yok; Explore/Grep kullan

**NLM Sync Policy (dosya değişince):**
| Dosya | Aksiyon |
|---|---|
| `TASARIM/*.md` | Hemen sync et — kullanıcıyı uyar |
| `MEMORY/*.md` | Hemen sync et — kullanıcıyı uyar |
| `CURRENT_STATUS.md` | Session sonunda sync — kullanıcıyı uyar |
| `CLAUDE.md`, `RULES.md` | Büyük değişimde sync — kullanıcıya sor |
| `Assets/Scripts/**` | Hiçbir zaman sync etme |

Codex dispatch kuralları: `CODEX_DISPATCH.md`

## Token Saving
- Session start: `CURRENT_STATUS.md` only. Edit: surgical line ranges.
- Bulk mechanical work -> Codex (GPT 5.5 High).
- Bulk analysis/audit -> Gemini (2.5 Pro).
- CLAUDE.md and CURRENT_STATUS.md must stay lean. Pointers, not content.
- `/lint` at: phase transitions, 5+ decisions, before asset work.

## Output Economy
- **Mechanical work** (code, test, commit): terse, fragments OK.
- **Design/decision work** (architecture, balance): nuanced output.
- Toggle automatic. Never compress design judgment.

## /clear
Call after: Review PASS, new phase, 20+ messages, heavy batch, topic switch.
Cache TTL = 5 min.

## Multi-Account Routing
- **"AWS'deyim"** -> Bedrock. Opus 4.6 decisions, Sonnet 4.6 mechanical.
- **"Asil hesabimdayim"** -> Claude Pro.
