---
name: music-production-pipeline
description: Royalty-free music pipeline for RIMA — Stable Audio Open (local RTX 5080) + REAPER + 2-state dynamic music + paste-ready prompts.
metadata: 
  node_type: memory
  type: project
  originSessionId: 625b4fb5-a2e2-4711-bd76-14dc1b6b53d4
---

# Music Production Pipeline — Royalty-Free, RIMA-specific

**Status:** Strategy defined, implementation Phase 2-3.
**Why:** Bandit Knight reviewer-cited "best feature" was dynamic 2-state music (exploration → combat seamless). RIMA needs same dynamic system without licensed library cost.
**How to apply:** Stable Audio Open generates base tracks (already locked Karar #10), REAPER mixes, Unity AudioMixer Snapshot crossfade for 2-state.

## Recommended pipeline

```
1. Stable Audio Open (LOCAL, RTX 5080)
   → Generate 5-10 base tracks per biome/state
   → Output: CC-BY (attribution required, commercial OK)
2. REAPER (FREE) — mix + layer + EQ + master
3. ChipTone — SFX (hit/parry/dash/pickup) — already in Karar #10
4. Unity AudioMixer
   → 2-state dynamic music (Bandit Knight insight)
   → Snapshot Combat vs Exploration, 500ms crossfade
   → Hit-event accent layer (cymbal swell at boss intro)
```

## Tool options (rated for RIMA fit)

### AI music generation (primary)
| Tool | License | Quality | RIMA fit | Cost |
|---|---|---|---|---|
| **Stable Audio Open** | CC-BY local | ⭐⭐⭐⭐ | LOCKED Karar #10 | Free (own GPU) |
| **Suno Pro** | Pro plan royalty-free | ⭐⭐⭐⭐⭐ | Backup, vocal tracks | $10/ay |
| **Udio Standard** | Commercial OK | ⭐⭐⭐⭐⭐ | Suno alternative | $10/ay |
| **AIVA Pro** | Royalty-free | ⭐⭐⭐⭐ | Orkestra odaklı | $30/ay |
| **MusicGen (Meta)** | MIT açık | ⭐⭐⭐ | Local backup | Free |

### Royalty-free libraries (fallback/quickstart)
| Source | License | Game commercial OK? |
|---|---|---|
| **Pixabay Music** | CC0 | ✅ Sıfır kısıtlama, kredi gereksiz |
| **Free Music Archive** | CC0/CC-BY mix | ✅ CC0 serbest |
| **Incompetech** | CC-BY | ✅ Kredi gerekli |
| **OpenGameArt.org** | CC0/CC-BY | ✅ Game-specific |
| **PlayOnLoop** | Per-track $5-10 | ✅ Curated, game odaklı |

### Production
- **REAPER** ($60, unlimited 60-day trial) — DAW
- **Splice Sounds** ($10/ay) — royalty-free loop library
- **LMMS** (free open source) — DAW alternative

## Paste-ready prompt examples (Stable Audio Open)

### Combat track (boss/elite)
```
Hades-style percussive orchestral combat track,
brass swells and taiko drums, driving 130 BPM,
dark fantasy roguelite, dynamic crescendos,
8-bit pixel art video game soundtrack feel,
no vocals, instrumental, looped intro.
Duration: 90 seconds.
```

### Exploration track (dungeon ambient)
```
Dark fantasy dungeon exploration ambient,
slow sustained strings, distant ethereal choir,
70 BPM, melancholic but heroic undertone,
top-down roguelite atmosphere,
faint bell tolls every 15 seconds,
no vocals, instrumental, seamless loop.
Duration: 120 seconds.
```

### Pre-boss buildup
```
Tense pre-boss buildup track,
low cello pulse, slow timpani builds,
60 BPM accelerating to 100 BPM,
dark fantasy game,
final 8 bars: full orchestra release,
instrumental, one-shot (no loop).
Duration: 45 seconds.
```

## License notes

| Asset | License | Steam ship OK? |
|---|---|---|
| Stable Audio Open output | CC-BY | ✅ Credit list: "Music generated with Stable Audio Open" |
| Suno Pro / Udio Standard | Commercial royalty-free (abone iken) | ✅ İptal sonrası üretilenler senin |
| Pixabay Music | CC0 | ✅ Kredi bile gereksiz |
| Incompetech | CC-BY | ✅ Kredi gerekli |

## V1 scope (Phase 1-2)

Faz 1: 1 combat track + 1 exploration track (~2 dakika her biri) — yeterli
Faz 2: + 1 pre-boss + 1 boss-fight + 1 hub
Faz 3+: biome başına ayrı track (Act 1/2/3)

## Cross-links
[[sfx-pipeline]] [[sfx-v2]] [[combat-feel-research-combined]]
