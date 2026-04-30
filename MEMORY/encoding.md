---
name: Encoding and language rules
type: feedback
trigger: ASCII, encoding, Turkish, prompt, internal-doc
description: ASCII-only rule and language split for project files
---

## Rules
- Internal .md files: ASCII-only. No Turkish diacritics.
  Forbidden chars: s-cedilla, i-dotless, g-breve, u-umlaut, o-umlaut, c-cedilla (and capitals).
- User-facing chat replies: Turkish OK.
- AI prompts (PixelLab, Gemini, any AI input): English only.
- Internal docs read by AI (CURRENT_STATUS, SYSTEM_MAP, memory files, CODEX_TASKS): English only, minimal-token.

## Why
Claude (Write tool) and Codex (API) use different encoding paths.
Turkish diacritics get double-encoded -- each agent writes but the other reads mojibake.
Confirmed corruption in STYLE_BIBLE.md, AGENTS.md, GUIDES/ files.

## Workarounds
- em-dash -> "--"
- degree symbol -> "deg" or number + "deg"
- Turkish special chars -> transliterate or omit ("tur" not "tur-umlaut", "ozellik" not "ozel+cedilla+lik")
