# CODEX DONE - Overnight Death Imprint Concept

PNG path + alpha:
- STAGING/concepts/overnight/12_death_imprint_concept.png
  - Format: PNG
  - Size: 1280x800
  - Mode: RGBA
  - Alpha: present, fully opaque (255..255)
- STAGING/concepts/overnight/12_death_imprint_hero_composite.png
  - Format: PNG
  - Size: 1280x800
  - Mode: RGBA
  - Alpha: present, fully opaque (255..255)

Mekanik proposal'a gorsel uyum verdict:
PASS. The 4-frame sequence communicates Death Imprint as revealed map node distortion after death: pre-death clean memory, cyan death flash, intensified cyan rift cracks on remembered nodes, and next-run persistence. It avoids fragment loss, resource loss, punishment UI, and mechanical penalty messaging.

Implementation suggestion:
- Unity VFX: drive cyan rift intensity, crack bloom, node haze, and short screen-shake flash from a DeathImprintVfxController.
- ScriptableObject: DeathImprintRecord with encounterId, subRoomIndex, subRoomTag, mob composition summary, environment context, macro encounter cadence, and visual intensity state.
- Runtime: when death occurs, write/update the record; on next run map reveal, query records and apply distorted node material overrides only to remembered/revealed nodes.
- Keep gameplay data read-only for this visual cue unless the mechanic later passes spec gate.

LOCK onerisi:
YES, with scope locked as visual/narrative reinforcement only. The cyan map distortion is enough to make the room feel like it remembers without adding a mechanical penalty. Do not add fragment loss or stat/resource punishment to this version.

Notes:
- ANTIGRAVITY.md was requested by local routing rules but was not present at project root.
