# CODEX TASK — Overnight: Death Imprint Visual Concept

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen. Output → `STAGING/concepts/overnight/`.

---

## Hedef

Death Imprint mekanik proposal görsel concept — **map node distortion on death** (Codex v1 önerisi, fragment kaybı YASAK).

## Bağlam

- Death Imprint = eski "Echo Imprint Cascade", Karar #122 collision rename
- Top epic mechanic candidate, NOT yet LOCKED (spec gate pending)
- Records: encounterId + subRoomIndex + subRoomTag + mob comp + env context
- Cadence: per macro encounter
- **Orchestrator karar (overnight LOCK):** Death visual effect = revealed map nodes distortion (cyan rift intensify), narrative reinforcement only, no mechanical penalty

## Görsel Spec

**1 PNG mockup, 1280×800, 4-frame sequence:**

| Frame | State | Visual |
|---|---|---|
| 1 | **Pre-death** | Kırık Taş Tablet map, 3 revealed nodes clean (granite + cyan rift normal) |
| 2 | **Death moment** | Tablet flashes cyan, "DEATH IMPRINT" text overlay, map shakes |
| 3 | **Post-death distortion** | Revealed nodes have INTENSIFIED cyan rift cracks (cracks deeper, brighter glow, "uncertain" haze around node icons) |
| 4 | **Next run start** | Player respawns, map memory persists with distortion visible — narrative cue "the room remembers" |

Her frame 320×800 zone, vertical mode.

Alternative concept (2nd render): single hero composite showing death imprint signature — silhouette of fallen player + cyan ghost echo + map distortion overlay.

## Stil

- Pixel art, painterly
- Cyan rift signature INTENSIFIED (Death Imprint visual marker)
- Lore-aligned: "echo cascade" — death cascades into room memory
- Anti-Hades clone (RIMA Style Manifesto)

## Output

`STAGING/concepts/overnight/12_death_imprint_concept.png` (4-frame sequence)

## Final Report

`STAGING/CODEX_DONE_overnight_death_imprint.md`:
- PNG path + alpha
- Mekanik proposal'a görsel uyum verdict
- Implementation suggestion (Unity VFX + ScriptableObject for record fields)
- LOCK önerisi: visual cue ile mekanik penalty yokken narrative reinforcement yeterli mi?

Background.
