# CODEX TASK — Overnight: Floor Types Per Room Comparison

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen tool. Output → `STAGING/concepts/overnight/`.

---

## Hedef

Floor tile variant'ları için **per room type** karşılaştırma sheet. Karar #143 6-layer pipeline ile uyumlu, b340684f bundle baseline.

## Bağlam

**Mevcut:** b340684f cyan rift floor tile bundle (16 PNG, S95 LATE NIGHT 3 re-import LIVE).

**Karar #143:** Alabaster Dawn 6-layer pipeline — tile base + large organic patches + L3 wall sprites + scatter + rift accent.

## Görsel

**1 PNG sheet, 1280×960, 4×2 grid:**

| Slot | Room Type | Floor Mood | Key Feature |
|---|---|---|---|
| 1 | **Entry** | Clean granite tile + dormant rift | Tutorial-safe palette |
| 2 | **Combat** | Standard cyan rift cracks scattered | Battle arena ready |
| 3 | **Elite** | Heavy rift veins + sigil circles | Marked challenge |
| 4 | **Rest** | Warm muted granite + healing glow patch | Recovery |
| 5 | **Shop** | Polished tile + trade carpet + golden glow | NPC zone |
| 6 | **Curse Gate** | Blood-stained + dark rift + corrupted patch | Risk warning |
| 7 | **Mystery** | Misty tile + pale runes + event sigil | Variable |
| 8 | **Boss** | Sealed obsidian + massive rift + arena lines | Final showdown |

Her slot: 3×3 tile arrangement (mock floor), camera 35° iso, lighting matching mood.

## Sorular Codex Cevaplayacak

1. **Tile variant breakdown**: Aynı b340684f bundle shader-driven yeterli mi, hangi room için ayrı PNG gerek?
2. **6-layer pipeline alignment**: Her variant Layer 1 (base) + L2a (patch) + L4 (decal) + L5 (vfx) parçaları nasıl üst üste binmeli?
3. **Re-gen ne kadar lazım**: Tahmini PixelLab gen count yeni floor pack için.

## Output

`STAGING/concepts/overnight/04_floor_types_per_room.png`

## Final Report

`STAGING/CODEX_DONE_overnight_floor_types.md`:
- PNG path
- Shader vs new-gen ayrımı
- Production cost
- 6-layer pipeline mapping per variant

Background.
