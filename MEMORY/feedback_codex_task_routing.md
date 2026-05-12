---
name: Codex Task Routing — Hard Rules
description: UnityMCP işi=CODEX_TASK.md+rima-codex; non-Unity iş=cx bash (PowerShell only). cx wrapper 3 profil (laurethgame ana). rima-codex agent lean (~2-3k token, Bash only).
type: feedback
---

# Codex Task Routing — Hard Rules (S58'de LOCKED)

## Karar Ağacı

**1. UnityMCP gerekiyor mu?**

- EVET → `CODEX_TASK.md`'ye task yaz → `rima-codex` agent spawn et
- HAYIR → 2'ye geç

**2. Non-UnityMCP iş (kod / araştırma / mekanik / dosya işlemi):**

- `/codex` skill kullan
- Skill prompt'u temp dosyaya yazar, `cx <profil> exec --prompt-file <path>` çalıştırır
- **CODEX_TASK.md'ye ASLA yazma** (UnityMCP-only)

## cx Wrapper

- 3 profil: `laurethgame` (ana tercih), `laurethayday` (paralel/fallback), `yasinderyabilgin` (paralel/fallback)
- Format: `cx <profil> exec --prompt-file <path>` (non-interactive)
- Veya kısa: `cx <profil> exec -pf <path>`
- **cx sadece PowerShell'de çalışır.** Bash'te PATH'te yok — Bash tool ile koşturmaz.
- Rate-limit olan profili at, diğerini kullan (3 paralel mümkün)

## rima-codex Agent

- Lean: sadece **Bash tool**, ~2-3k token baseline
- Sadece UnityMCP batch işleri için (>5 tool call iş)
- Küçük iş (1-3 tool call) → Claude direkt UnityMCP MCP'sini çağırsın, agent spawn etme

## Why

- UnityMCP rima-codex'in işi çünkü Unity API'lerini bilen Codex var
- Non-Unity işler (Python, doc, refactor) için CODEX_TASK overhead gereksiz — direkt cx bash
- 3 profil paralelizasyon için (rate-limit yedek)

## How to apply

Her Codex göreve başlamadan önce:
1. UnityMCP API call var mı? Varsa: CODEX_TASK + rima-codex
2. Yoksa: `/codex` skill (cx bash direkt)
3. Küçük UnityMCP işi (<5 call): Claude direkt yapar
