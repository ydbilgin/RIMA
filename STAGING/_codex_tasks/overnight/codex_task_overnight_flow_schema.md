# CODEX TASK — Overnight: Master Flow Schema PNG

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

Codex's **built-in imagegen tool** (NOT shell OpenAI API). Source PNG → `STAGING/concepts/overnight/`.

---

## Hedef

RIMA Act 1 progression flow için **görsel akış şeması** çiz. User uzun-akış doc'unu değil, **tek bakışta okunan diagram** istiyor.

## Görsel İçerik

**Layout (önerilen):** 1024×1280 portrait PNG, dark dungeon backdrop, cyan rift accent.

**Akış:**

```
┌─────────────────────────────────────────────────┐
│  N00 ENTRY (Tutorial)                            │
│  ↓                                                │
│  Combat 1 → [Map Fragment drop, G pickup]        │
│  ↓ → 3-Choice Skill Draft (new/upgrade/imprint)  │
│  Combat 2 → [Fragment + Skill Draft]             │
│  ↓                                                │
│  Elite 1 → [Fragment + Elite HP %12 + Draft]    │
│  ↓ ←──── B01 Curse Gate (branch, no fragment)  │
│  Rest 1 (F1→F2 transit, no fragment)            │
│  ↓                                                │
│  Combat 3 → [Fragment + Draft]                   │
│  ↓                                                │
│  Shop (no fragment, Gold spend, HP service)     │
│  ↓                                                │
│  Combat 4 → [Fragment + Draft]                   │
│  ↓ ←──── B02 Mystery (branch, chance fragment) │
│  Elite 2 → [Fragment + Elite HP + Draft]        │
│  ↓                                                │
│  Rest 2 (F2→F3 transit)                          │
│  ↓                                                │
│  Combat 5 → [Fragment + Draft]                   │
│  ↓                                                │
│  Combat 6 → [Fragment + Draft]                   │
│  ↓                                                │
│  ╔════════════════════════════════════╗         │
│  ║  8-FRAGMENT GATE                   ║         │
│  ║  6 Combat + 2 Elite = 8 mandatory  ║         │
│  ╚════════════════════════════════════╝         │
│  ↓                                                │
│  ⚔ BOSS — Relic + Boss HP %50                   │
└─────────────────────────────────────────────────┘
```

Her node için:
- **İkon** (rift seam variant — Combat/Elite/Rest/Shop/Curse/Mystery/Boss)
- **Drop badge** (fragment ✓ / fragment ✗ / chance)
- **Skill Draft badge** (3-choice indicator if applicable)
- **Konnektör** (cyan rift line)

Branch nodes (B01 Curse Gate, B02 Mystery) yan dal olarak sağda/solda.

Stil:
- Kırık Taş Tablet metaforu (Karar #63) — rusty gold frame, abstract grid, cyan rift cracks
- Painterly pixel art
- Hades + Diablo synthesis (RIMA Style Manifesto)

## Output

`STAGING/concepts/overnight/01_progression_flow_schema.png`

1024×1280 PNG, transparent corners OK, chroma key remove sonrası post-process.

## Acceptance Criteria

- ✅ 15 node hepsi görünür (N00-N12 + B01/B02)
- ✅ Fragment drop/skip rules her node'da açık
- ✅ 8-fragment gate prominent
- ✅ Branch nodes (B01, B02) net ayrılmış
- ✅ Cyan rift signature görsel kimlik
- ✅ Hades clone değil (RIMA Style Manifesto)

## Final Report

`STAGING/CODEX_DONE_overnight_flow_schema.md`:
- PNG path
- Alpha analysis (transparent / opaque px count)
- Lore-fit verdict
- Improvement suggestion (if any)

## Dispatch

Background, effort=high.
