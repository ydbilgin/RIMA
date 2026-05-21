# CODEX TASK — Overnight: Wall Types Per Room Comparison

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen tool. Output → `STAGING/concepts/overnight/`.

---

## Hedef

RIMA için **hangi wall variant'lar lazım** sorusuna görsel cevap. 8 room type × wall mood. Universal Shader pattern bunlar shader ile mi yoksa ayrı PNG mi gerek belirler.

## Bağlam — Pilot A 7-piece (mevcut, S95)

Wall pieces:
- face_EW (horizontal wall)
- face_NS (vertical wall)
- corner_outer, corner_inner
- T_junction, end_cap, arch_opening

**MEVCUT SORUN:** face_EW + face_NS canvas %52 fill, dar dikey çubuk drift. Re-gen lazım (Karar pending).

## Görsel İstek

**1 PNG sheet, 1280×960 portrait, 4×2 grid:**

| Slot | Room Mood | Wall State | Material |
|---|---|---|---|
| 1 | **Entry** intact | Clean granite block + faint cyan | Pristine |
| 2 | **Combat** standard | Granite + visible cyan rift cracks | Battle-worn |
| 3 | **Elite** heavy | Reinforced + sigil carving + intense rift | Marked |
| 4 | **Rest** sealed | Warm granite + dormant rift, transit symbol | Safe |
| 5 | **Shop** warm | Granite + golden trade mark + lantern hooks | NPC-friendly |
| 6 | **Curse** corrupted | Black-stained + red rift bleed + thorny accent | Cursed |
| 7 | **Mystery** veiled | Pale stone + mist haze + question sigil | Enigmatic |
| 8 | **Boss** sealed massive | Towering + sealed runes + 8-fragment slot pattern | Final |

Her slot içinde:
- Wall corner_outer at left
- face_EW horizontal stretch at center (3 stretched blocks)
- corner_outer at right
- Wall variant mood + palette

## Sorular Codex Cevaplayacak

1. **Universal Shader yeterli mi?** Hangi variants shader-driven OK, hangileri ayrı PNG gerek?
2. **Re-gen list**: Hangi piece'ler için yeni PixelLab/imagegen lazım? (Component icon set gibi)
3. **Tile-mate edge mate okunabilir mi?** Her variant'ın brick pattern aynı temel ile devam etmeli.

## Output

`STAGING/concepts/overnight/03_wall_types_per_room.png`

## Final Report

`STAGING/CODEX_DONE_overnight_wall_types.md`:
- PNG path
- Shader vs new-gen ayrımı tablosu
- Production cost tahmini (kaç ek gen)
- Tile-mate edge verdict (continues / breaks)

Background.
