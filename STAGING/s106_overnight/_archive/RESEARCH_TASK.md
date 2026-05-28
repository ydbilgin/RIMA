ACTIVE RULES: (1) think before answering (2) cite sources (3) compare 3+ industry examples (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: Antigravity'nin web research yeteneğini kullanarak Stream D (painter tool UX) ve Stream C (algorithm validation) için industry best-practice insights çıkar. Bu rapor MASTER_PLAN sentezine girecek.

---

# RESEARCH TASK — Industry Best Practices for ARPG Room Builder + Blueprint Editor

You (Antigravity) have web search. Use it. Cite specific games, articles, GDC talks, open-source repos. Be concrete.

## Background

RIMA is a 2D top-down ARPG (Hades/Diablo/Children of Morta visual style). We're building a Unity Editor blueprint painter tool where the level designer draws a room footprint on a grid, and the system auto-derives wall chains + colliders + door sockets from modular pixel-art wall pieces. Full spec: `STAGING/s106_overnight/MASTER_CONTEXT.md`.

## Research questions (answer each with concrete examples)

### Q1. How do AAA ARPG/roguelite games handle modular dungeon room composition?

Compare at minimum:
- **Hades / Hades II** (Supergiant) — known for tight room composition
- **Diablo 4** (Blizzard) — modular dungeon piece system
- **Path of Exile / PoE 2** (GGG) — random dungeon assembly
- **Children of Morta** (Dead Mage) — handcrafted modular feel
- **Darkest Dungeon** (Red Hook) — scrolling 2D modular
- **Curse of the Dead Gods** (Passtech) — 3D pseudo-2D
- **Enter the Gungeon / Nuclear Throne** — tile-grid roguelite

For each:
- Is it tile-grid, prefab-stamp, or hybrid?
- How do they handle wall corners / connectors / door arches?
- Public-facing dev posts / GDC talks / postmortems?
- Any Unity asset store or open-source equivalents?

### Q2. Unity-specific patterns for grid-based level editors

What's the modern (2024-2026) best practice for:
- **EditorWindow + IMGUI vs UI Toolkit (UXML)** — which is better for grid paint UIs?
- **Tilemap (built-in) vs prefab-stamp vs hybrid** — tradeoffs for ARPG with pixel-perfect cameras
- **Brush systems** — Unity's GridBrush extension API, or roll-own?
- **Undo/redo in EditorWindow** — Undo.RecordObject vs custom stack
- **Scene preview during paint** — gizmos vs handles vs prefab preview
- **Save format** — ScriptableObject vs JSON vs binary

Cite Unity docs version, blog posts, asset store examples (Astar Pathfinding, ProGrids, etc.)

### Q3. Logic-first vs visual-first room design

The user's hard requirement: collider/walkable logic INDEPENDENT of sprite visuals.
- Which engines/games separate logic from visual layer at the level editor?
- Common architectural patterns? (entity-component vs data-driven vs hybrid)
- Pitfalls when sprite is updated but collider isn't? (e.g. Brigador, Hyper Light Drifter)

### Q4. Wall chain auto-derivation from blueprint paint

User wants to PAINT walkable area, system AUTO-DERIVES wall chain + connectors + door sockets.
- Closest industry example you can find?
- Algorithm class (marching squares? edge detection on grid? rule-based extension of Wang tiling?)
- Open-source implementation (GitHub, asset store) we could study?
- Specific paper/blog post that covers this transformation?

### Q5. Pixel-art collider alignment best practices

User: "2dboxları ayarlı şekilde olacak". Pixel art prefabs often have visual extending past collider, or pivot at sprite center vs ground footprint. Industry standard?
- Children of Morta / Hyper Light Drifter / Death's Door — what's their footprint anchor pattern?
- Unity sprite pivot modes (Bottom-Center vs Custom) — what's the GDC consensus?
- BoxCollider2D vs CompositeCollider2D vs polygon — for modular wall pieces?

### Q6. Stepped diamond / fake-isometric techniques

The user wants "diamond / stepped diamond arena" using SQUARE GRID + stepped chain (no true diagonal sprites). What games pioneered this technique?
- Diablo 1/2 fake-iso roots
- Modern revivals (Hades, Children of Morta)
- 1-2 cell step rules — any formal writeup?

## Output format

Write to AGY_DONE_<account>.md as:

```
# Industry Research — Antigravity — 2026-05-25

## Q1. AAA ARPG room composition patterns
<3+ games per pattern category, cite sources>

## Q2. Unity grid editor best practices 2024-2026
<concrete API recommendations, Unity version notes>

## Q3. Logic-first vs visual-first separation
<architectural patterns, pitfalls>

## Q4. Wall chain auto-derivation
<closest examples, algorithms, source pointers>

## Q5. Pixel-art collider alignment
<industry standards, Unity patterns>

## Q6. Stepped diamond on square grid
<historical + modern techniques>

## TL;DR — Top 5 recommendations for RIMA tonight
<5 concrete actionable items, prioritized>
```

**Length:** 1500-3000 words. Cite at least 12 specific sources (game, article, repo, doc).

**Quality bar:** This drives Stream D (painter tool) and Stream C (algorithm validation) decisions. Surface fact, not speculation. If a question has no good industry answer → say "no consensus, our approach is novel" rather than inventing.
