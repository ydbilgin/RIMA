# Act 1 Sub-Room Templates — Opus Tasarım (2026-05-20)

> 5 canonical tag için somut 32×22 grid layoutlar. Faz-1 vertical slice = `entry_chamber` + `pillar_arena`. Diğer 3 backlog.

## Koordinat Sistemi
- Bounds 32×22, origin (0,0) = bottom-left
- X: 0-31 (sağa artar), Y: 0-21 (yukarı artar)
- Playable bounds: X 1..30, Y 1..20 (perimeter rim 1 tile)
- Door socket yönleri: N (Y=21), S (Y=0), E (X=31), W (X=0)
- Player footprint = 1 tile, dash ≈ 4 tile

---

## A0 — entry_chamber (FAZ-1 LOCK)

```
Giriş: (4, 3) player spawn, facing NE
Çıkış: (27, 0) S-edge archway  [REVISED — mirror edge fix]

Perimeter wall: Y=0 (gap X=26..28), Y=21, X=0 SE quarter only
Internal:
  - Pillar A: (10, 8)
  - Pillar B: (10, 14)
  - Collapsed stub: (18, 4)–(20, 4)
  - Archway frame: (26, 0)–(28, 0)  [REVISED]

Enemy spawns (3 — wave 1, low threat):
  - S1 (15, 10) standard, avoidRadius=1.5
  - S2 (19, 13) standard, avoidRadius=1.5
  - S3 (22, 8)  standard, avoidRadius=1.5

Objeler:
  - 2× granite_pillar (zincirli)        @ A, B
  - 1× collapsed_pillar_stub            @ (20, 6)
  - 1× brazier_lit                      @ (6, 4)
  - 1× moss_patch                       @ (12, 6)
  - 1× dust_drift                       @ (8, 12)
  - 1× chain_anchor_ceiling             @ (26, 15)

Combat: Açık alan, kamera/dash alışma odası. Pillar landmark, tek dalga.
Asset envanteri: %100 mevcut, yeni üretim yok.
```

## A0 — pillar_arena (FAZ-1 LOCK)

```
Giriş: (28, 21) N-edge  [REVISED — entry'nin S-exit'i ile mirror]
Çıkış: (28, 0) S-edge (sonraki sub-room'a)

Perimeter wall: Y=21 (gap X=27..29), Y=0 (gap X=27..29)
                X=0 partial (Y=12..20), X=31 partial (Y=0..10)
Internal:
  - Pillar NW: (10, 15)
  - Pillar NE: (22, 15)
  - Pillar SW: (10, 7)
  - Pillar SE: (22, 7)
  - Half-wall stub vertical: (16, 11)–(16, 13)
  - Collapsed stub: (5, 5)–(7, 5)

Enemy spawns (6 — wave 1 + wave 2):
  Wave 1:
   - S1 (16, 16) standard, avoidRadius=1.5
   - S2 (16, 6)  standard, avoidRadius=1.5
   - S3 (26, 11) standard, avoidRadius=1.5
  Wave 2:
   - S4 (5, 11)  ranged,   avoidRadius=2.0
   - S5 (12, 11) elite,    avoidRadius=2.5
   - S6 (20, 11) standard, avoidRadius=1.5

Objeler:
  - 4× granite_pillar (zincirli)        @ 4 pillar coords
  - 1× half_wall_stub_vertical          @ (16, 11)–(16, 13)
  - 1× collapsed_pillar_stub            @ (6, 5)
  - 2× brazier_lit                      @ (10, 19), (22, 3)
  - 2× moss_patch                       @ (8, 10), (24, 12)
  - 1× rift_seep_decal                  @ (16, 11)
  - 2× chain_anchor_ceiling             @ (10, 21), (22, 0)

Combat: Ana çatışma odası. 4 sütun kite hattı, half-wall LOS böler,
dash ile geçilebilir. Pillar arası 6 tile = ranged + 2 melee sığar.

Open flag: half_wall_stub_vertical envanterde net yok — yatay rubble
pile'ı 90° döndür veya rima-asset üretsin (Faz-1 kararı: rotate kullan).
```

---

## BACKLOG — Faz-2/3 için Saklanan Layoutlar

### collapse_corridor (Faz-2)
```
Boyut: 32×22, koridor formu
Giriş: (6, 21) N, çıkış: (26, 0) S
Yapı: Slab A (upper choke) (10..14, 16) + (17..22, 16) gap 2-tile @ (15-16, 16)
      Slab B (lower choke) (10..14, 6) + (17..22, 6) gap @ (15-16, 6)
      Ambush pillar: (15, 11)
      Rubble cluster: (4-6, 12-13), (26-28, 9-10), (12, 3), (20, 19)

Spawns (5):
  - S1 (16, 13) standard, ilk karşılaşma
  - S2 (4, 18)  ranged, upper ambush
  - S3 (28, 4)  ranged, lower ambush
  - S4 (15, 9)  elite_ambush, gap geçince tetiklenir
  - S5 (16, 3)  standard, lower wave

Objeler: wall_slab_long (3× module birleşik), pillar, 4× collapsed_stub,
3× moss_patch, 2× dust_drift, rift_seep_decal, chain_anchor_ceiling broken.

Gerekçe ertelendi: Multi-phase spawn trigger + EncounterController logic
gerekiyor → Faz-1 scope dışı.
```

### ritual_hall (Faz-3 boss combat ile)
```
Boyut: 32×22, simetrik tören düzeni
Giriş: (15-16, 0) 2-tile S gate
Çıkış: YOK (encounter clear sonrası reward fade) veya (15-16, 21) ikinci kapı

Yapı: Altar block (15-16, 10-11) 2×2 yükseltilmiş
      Hero rift glow decal (14-17, 9-12) 4×4
      Pillar quartet: NW(8,16), NE(23,16), SW(8,6), SE(23,6)
      Outer pillar pair: (12, 3), (19, 3)
      Reliquary shelves: (3-4, 11-12), (27-28, 11-12) mirror
      Collapsed stub: (5, 18)

Spawns (8 — boss + 3 phase adds):
  Phase 1: S1 boss (16,16), S2-S3 adds (8,11)+(23,11)
  Phase 2: S4-S5 ranged (8,16)+(23,16), S6-S7 (8,6)+(23,6)
  Phase 3 finale: S8 elite_summon (16,6) altar altından

Objeler: 6× pillar, 1× altar_block, 1× rift_seep_decal_large,
2× reliquary_shelf, 4× brazier (köşeler), 6× chain_anchor, 2× moss.

Gerekçe ertelendi: Boss combat sistemi LIVE değil + altar_block art
dependency Faz-3 boss pass'ten gelecek.
```

### crypt_cell (Faz-2 reward pocket)
```
Boyut: 12×8 (Karar #150 küçük oda exception)
Giriş: (5-6, 7) 2-tile N gate, çıkış YOK (clear sonra back-archway açılır)

Yapı: Sarcophagus 2×2 (5-6, 3-4)
      Wall stub: (2, 5)
      Cell bars: (8, 1)–(8, 3)
      Pillar: (2, 2)

Spawns (3 — tight encounter):
  - S1 (3, 5) standard, avoidRadius=1.2
  - S2 (9, 5) standard, avoidRadius=1.2
  - S3 (6, 6) elite_lurker, avoidRadius=2.0 (giriş arkası ambush)

Reward socket: Chest @ (5, 4) sarcophagus üstü, clear sonrası interact.

Objeler: sarcophagus_block (2×2), cell_bars_vertical, granite_pillar,
collapsed_stub, brazier, moss, dust, rift_seep_decal_small.

Gerekçe ertelendi: 12×8 exception + chest interactable + back-archway
gate logic Karar #149 "isFinal" semantiği netleşmeden riskli.

Open flag: S3 elite_lurker küçük oda için fazla tight olabilir;
S3'ü standard'a düşür VEYA bounds 14×10'a çıkar (Faz-2'de karar).
```

---

## Mirror Edge Lock

Karar #149 mirror archway validator için **entry_chamber S-exit ↔ pillar_arena N-entry** zorunlu (compatible-edge rule top↔bottom).

Revised:
- entry_chamber çıkış: **(27, 0) S-edge**
- pillar_arena giriş: **(28, 21) N-edge** (mirror tolerance ±2 cell ✓)

---

## Implementation Next Steps

1. **rima-codex dispatch:** `EncounterTemplateValidator.cs` mirror archway rules implement + 2 RoomTemplateSO asset author + 1 EncounterTemplateSO bind
2. **rima-asset backlog (Faz-2/3):** altar_block, sarcophagus_block, wall_slab_long üretim listesi
3. **Open flags:** half_wall_stub_vertical → rotate vs üretim (Faz-1: rotate), crypt_cell elite tier (Faz-2)
