# CODEX TASK — Overnight: All 4 Acts Schema + Detailed Game Flow

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen. Output → `STAGING/concepts/overnight/`.

---

## Hedef

Tüm 4 Act için flow schema + Act-specific material/theme görselleri. NLM canonical data inlined.

## NLM Canonical (Authoritative)

### Act 1: Shattered Keep
- 15 node fixed: 1 Entry + 6 Combat + 2 Elite + 2 Rest + 1 Shop + 1 Curse Gate + 1 Mystery + 1 Boss
- Material: Cool Granite #3A3D42 + Worn Stone #4A4842 + cyan rift
- Theme: Castle dungeon, ancient granite, fault lines
- Tier: Common + Rare
- Tablet visual: Kale oymaları (castle carvings, paslı altın frame)

### Act 2: Bleeding Wastes
- 9-11 oda dynamic: 5-6 Combat + 1-2 Elite + 1 Shop + 1 Spirit (NEW) + 1-2 Unknown + 1 Boss
- Material: Corrupted Bog #3A2840 + Rust ember #C8502A accent
- Walls: kemik-sarılı granit + dark roots
- Theme: Living corrupted wound, dark swamp + ossuary
- Boss: Echo Twin (2 phase) — dual-class mirror
- Tier: + Epic (Purple)
- Tablet visual: Damarlı et (veined flesh, organic + bone + rust)

### Act 3: Core Approach
- 9-11 oda: 5-6 Combat + 1-2 Elite + 1 Shop + 1-2 Spirit + 1-2 Unknown + 1 Boss
- Material: Void Substrate #0A0810 + Incandescent gold #FFD700 + Void bleed #4F2A6B
- Walls: black void-stone + half-erased gold seals
- Theme: Cosmic, reality thinning, void
- Boss: Fracture Sovereign (3 phase) — environment collapse
- Tier: + Legendary (Gold)
- Tablet visual: Yüzen parçalar (floating pieces, void + gold flecks)

### Act 4: Nexus Core
- 5-6 oda short+dense: 3-4 Combat + 1 Elite + 0-1 Spirit/Unknown + 1 Final Boss
- Material: Pure white + pure black + player class dynamic VFX
- Theme: Mirror chamber, ultimate confrontation
- Boss: The Architect (4 phase) — Fracturing creator, Phase 3 = 4s Silence
- 3 endings: Stay / Break / Carry
- Tablet visual: Ayna (mirror, reflective + multi-color rift)

## Görsel İstek — 5 PNG

### Render 1: `13_all_acts_master_flow.png` (1024×1536 portrait)

Tüm 4 Act flow tek diagram — Kırık Taş Tablet metaforu, paslı altın frame, cyan rift accents.

Layout (top-to-bottom):
```
┌─────────────────────────────────────────┐
│  RIMA — THE FULL JOURNEY                │
├─────────────────────────────────────────┤
│                                          │
│  ACT 1: SHATTERED KEEP (15 nodes)       │
│  [granite stone theme]                   │
│  Entry → 6 Combat + 2 Elite → ...        │
│  ↓ Boss (8-fragment gate)                │
│                                          │
├─────────────────────────────────────────┤
│  ACT 2: BLEEDING WASTES (9-11 nodes)    │
│  [bog flesh bone theme]                  │
│  Combat + Elite + Spirit (NEW) + ...     │
│  ↓ Echo Twin (2-phase boss)              │
│  Unlock: Epic tier + Cross-class Ult     │
│                                          │
├─────────────────────────────────────────┤
│  ACT 3: CORE APPROACH (9-11 nodes)      │
│  [void gold theme]                       │
│  Combat + Spirit + Unknown + ...         │
│  ↓ Fracture Sovereign (3-phase)          │
│  Unlock: Legendary tier                  │
│                                          │
├─────────────────────────────────────────┤
│  ACT 4: NEXUS CORE (5-6 nodes)          │
│  [mirror dynamic theme]                  │
│  Combat condensed + final                │
│  ↓ The Architect (4-phase, Silence)      │
│  3 Endings: Stay / Break / Carry         │
└─────────────────────────────────────────┘
```

### Render 2: `14_act2_bleeding_wastes_room.png` (1280×800)

Act 2 environment showcase. Chibi Warblade in scene.
- Floor: Corrupted Bog #3A2840
- Walls: bone-wrapped granite + dark roots
- Accent: Rust ember candles, dried blood patches
- Encounter: Spirit Encounter (NEW Act 2 mechanic) — ghostly NPC offering conditional reward
- 35° iso, painterly pixel art

### Render 3: `15_act3_core_approach_room.png` (1280×800)

Act 3 environment showcase.
- Floor: Void Substrate #0A0810 (stardust black)
- Walls: black void-stone + half-erased gold seals
- Accent: Incandescent gold + void bleed leak
- Encounter: enemy with "Adaptation" tag (visual cue — copying player's last skill)
- 35° iso, cosmic atmosphere

### Render 4: `16_act4_nexus_core_room.png` (1280×800)

Act 4 final showcase.
- Floor: Pure white + black checker (mirror pattern)
- Walls: reflective mirror surfaces + player class color refraction
- Encounter: The Architect glimpse silhouette
- 35° iso, ethereal Hades-trial finale

### Render 5: `17_boss_lineup_4acts.png` (1280×640, 4×1 row)

4 boss yan yana, hero silüet portrait:
- Act 1 Boss (TBD — Shattered Keep finale, granite warden)
- Act 2 Echo Twin (mirrored dual class entity)
- Act 3 Fracture Sovereign (void wound personified)
- Act 4 The Architect (mirror reflection of player + creator power)

## Stil

- RIMA Style Manifesto compliant (Hades + Alabaster Dawn + Diablo synthesis, clone değil)
- Kırık Taş Tablet aesthetic
- Per-Act material/palette tutarlı
- Painterly pixel art

## Output

5 PNG yukarıdaki paths.

## Final Report

`STAGING/CODEX_DONE_overnight_all_acts.md`:
- 5 PNG list + alpha analysis
- Act material continuity verdict
- Boss design lineup uyum verdict
- Production cost tahmini (per-Act material set needed)

## Dispatch

`--profile laurethayday` (alternatif: ydbilgin if quota), `--timeout 3600`, background.
