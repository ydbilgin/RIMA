# Anchor Roster QC Report — S43
**Date:** 2026-04-28 17:01
**Tool:** Pillow + numpy
**Scope:** Read-only source PNG analysis; only this markdown report was written.
**Accent source:** CURRENT_STATUS MASTER_KARAR #163 for exact hex values; `styleref_cheatsheet_v1.md` for identity descriptions. `STYLE_BIBLE.md` not used.

## Summary
| Class | Canvas | Outline | Accent | Identity | Verdict |
|---|---|---|---|---|---|
| Warblade | ✅ PASS | ✅ PASS | ❌ FAIL | ✅ PASS | ❌ FAIL |
| Elementalist | ✅ PASS | ✅ PASS | ✅ PASS | ✅ PASS | ✅ PASS |
| Shadowblade | ✅ PASS | ✅ PASS | ⚠️ WARN | ⚠️ WARN | ⚠️ WARN |
| Ranger | ✅ PASS | ✅ PASS | ❌ FAIL | ⚠️ WARN | ❌ FAIL |
| Ravager | ✅ PASS | ✅ PASS | ❌ FAIL | ✅ PASS | ❌ FAIL |
| Ronin | ✅ PASS | ✅ PASS | ✅ PASS | ✅ PASS | ✅ PASS |
| Brawler | ✅ PASS | ✅ PASS | ❌ FAIL | ✅ PASS skin avg 125.6 | ❌ FAIL |
| Gunslinger | ✅ PASS | ✅ PASS | ❌ FAIL | ✅ PASS skin avg 111.3 | ❌ FAIL |
| Hexer | ✅ PASS | ✅ PASS | ⚠️ WARN | ⚠️ WARN | ⚠️ WARN |
| Summoner | ✅ PASS | ✅ PASS | ❌ FAIL | ✅ PASS | ❌ FAIL |

## Per-Character Detail
### Warblade
- Source: `_STAGING\anchors\warblade\warblade_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (3,4)-(91,125), non-transparent=4809px (29.35% canvas), bbox density=44.29%, head top y=4, feet anchor y=125, height=122px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=503, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #66AAFF at sword fuller / chest plate crack; pixels within tolerance=0 (tol=36 Euclidean RGB), nearest=RGB(107,189,218) Δ=41.9; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(212,152,113) from box (36, 14, 59, 37), n=61, brightness=159.0
  - chest: RGB(152,99,72) from box (34, 41, 61, 65), n=104, brightness=107.7
  - viewer_left_hand: RGB(74,79,79) from box (7, 47, 34, 85), n=72, brightness=77.3
  - viewer_right_hand: RGB(126,99,72) from box (61, 47, 88, 85), n=301, brightness=99.0
  - top hair region: RGB(66,52,53), n=270
- Identity assertion: identity-specific assertion measured through accent + region samples
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=PASS)

### Elementalist
- Source: `_STAGING\anchors\elementalist\elementalist_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (29,2)-(89,125), non-transparent=4160px (25.39% canvas), bbox density=55.00%, head top y=2, feet anchor y=125, height=124px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=480, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected gold/cream approx #FFDFA0 at left palm orb; pixels within tolerance=792 (tol=62 Euclidean RGB), nearest=RGB(254,227,174) Δ=14.6; centroid=(56.3, 37.6); components: area 237 bbox (31,28)-(48,44) centroid (39.5,36.0); area 148 bbox (55,12)-(68,30) centroid (60.9,21.5); area 120 bbox (56,51)-(70,60) centroid (62.9,55.9); area 104 bbox (72,34)-(81,57) centroid (76.4,45.4); area 64 bbox (58,32)-(68,41) centroid (63.1,37.1)
- Identity region RGB samples:
  - face: RGB(255,204,179) from box (52, 12, 67, 35), n=236, brightness=212.7
  - chest: RGB(255,204,179) from box (50, 39, 69, 64), n=198, brightness=212.7
  - viewer_left_hand: RGB(199,132,76) from box (32, 45, 50, 84), n=59, brightness=135.7
  - viewer_right_hand: RGB(235,166,140) from box (69, 45, 87, 84), n=151, brightness=180.3
  - top hair region: RGB(219,156,24), n=515
- Identity assertion: identity-specific assertion measured through accent + region samples
- Verdict: PASS (Canvas=PASS, Outline=PASS, Accent=PASS, Identity=PASS)

### Shadowblade
- Source: `_STAGING\anchors\shadowblade\shadowblade_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (26,3)-(99,125), non-transparent=3693px (22.54% canvas), bbox density=40.57%, head top y=3, feet anchor y=125, height=123px
- Outline: dominant dark boundary RGB(4,4,8), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=495, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #9933CC at dagger edges / eye; pixels within tolerance=8 (tol=42 Euclidean RGB), nearest=RGB(153,61,180) Δ=26.0; centroid=(39.5, 83.6); components: area 2 bbox (43,80)-(44,80) centroid (43.5,80.0)
- Identity region RGB samples:
  - face: RGB(176,126,82) from box (53, 13, 73, 36), n=92, brightness=128.0
  - chest: RGB(87,80,60) from box (52, 40, 74, 64), n=63, brightness=75.7
  - viewer_left_hand: RGB(154,126,82) from box (30, 46, 52, 84), n=86, brightness=120.7
  - viewer_right_hand: RGB(149,125,96) from box (74, 46, 96, 84), n=133, brightness=123.3
  - top hair region: RGB(29,33,29), n=501
- Identity assertion: purple accent connected components >=2px: 1
- Identity assertion: two dagger clusters not clearly separated by color scan
- Verdict: WARN (Canvas=PASS, Outline=PASS, Accent=WARN, Identity=WARN)

### Ranger
- Source: `_STAGING\anchors\ranger\ranger_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (36,4)-(107,125), non-transparent=3566px (21.77% canvas), bbox density=40.60%, head top y=4, feet anchor y=125, height=122px
- Outline: dominant dark boundary RGB(8,8,8), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=549, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #FFCC00 at bow tips / arrow fletching; pixels within tolerance=0 (tol=48 Euclidean RGB), nearest=RGB(225,165,53) Δ=72.3; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(199,134,95) from box (63, 14, 81, 37), n=138, brightness=142.7
  - chest: RGB(131,113,78) from box (61, 41, 83, 65), n=266, brightness=107.3
  - viewer_left_hand: RGB(154,116,78) from box (40, 47, 61, 85), n=336, brightness=116.0
  - viewer_right_hand: RGB(140,128,102) from box (83, 47, 104, 85), n=90, brightness=123.3
  - top hair region: RGB(119,44,21), n=411
- Identity assertion: bow gold accent centroid: None = not found
- Identity assertion: bow/arrow gold accent weak
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=WARN)

### Ravager
- Source: `_STAGING\anchors\ravager\ravager_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (24,5)-(109,125), non-transparent=5527px (33.73% canvas), bbox density=53.11%, head top y=5, feet anchor y=125, height=121px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=526, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #FF3322 at scar lines / axe edge; pixels within tolerance=0 (tol=46 Euclidean RGB), nearest=RGB(186,114,41) Δ=93.7; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(188,114,68) from box (56, 15, 78, 38), n=338, brightness=123.3
  - chest: RGB(160,85,63) from box (54, 41, 80, 66), n=426, brightness=102.7
  - viewer_left_hand: RGB(148,82,61) from box (28, 47, 54, 85), n=294, brightness=97.0
  - viewer_right_hand: RGB(131,73,51) from box (80, 47, 106, 85), n=307, brightness=85.0
  - top hair region: RGB(158,93,41), n=652
- Identity assertion: hair sample RGB(158,93,41) from top hair region
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=PASS)

### Ronin
- Source: `_STAGING\anchors\ronin\ronin_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (10,4)-(99,125), non-transparent=3944px (24.07% canvas), bbox density=35.92%, head top y=4, feet anchor y=125, height=122px
- Outline: dominant dark boundary RGB(4,8,8), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=530, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #FFFFFF at scabbard edge; pixels within tolerance=22 (tol=42 Euclidean RGB), nearest=RGB(242,248,245) Δ=17.8; centroid=(32.9, 85.6); components: area 3 bbox (12,109)-(14,109) centroid (13.0,109.0)
- Identity region RGB samples:
  - face: RGB(215,162,116) from box (43, 14, 67, 37), n=200, brightness=164.3
  - chest: RGB(221,166,118) from box (42, 41, 68, 65), n=11, brightness=168.3
  - viewer_left_hand: RGB(80,86,85) from box (14, 47, 42, 85), n=53, brightness=83.7
  - viewer_right_hand: RGB(197,139,95) from box (68, 47, 96, 85), n=58, brightness=143.7
  - top hair region: RGB(46,44,41), n=450
- Identity assertion: white scabbard/katana accent centroid: (32.9, 85.6) = viewer-left
- Verdict: PASS (Canvas=PASS, Outline=PASS, Accent=PASS, Identity=PASS)

### Brawler
- Source: `_STAGING\anchors\brawler\brawler_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (26,5)-(97,124), non-transparent=4500px (27.47% canvas), bbox density=52.08%, head top y=5, feet anchor y=124, height=120px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=434, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #FF8800 at knuckle gauntlet; pixels within tolerance=0 (tol=52 Euclidean RGB), nearest=RGB(199,129,92) Δ=107.9; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(216,153,110) from box (53, 15, 71, 37), n=338, brightness=159.7
  - chest: RGB(222,160,120) from box (51, 41, 73, 65), n=474, brightness=167.3
  - viewer_left_hand: RGB(137,71,53) from box (30, 47, 51, 84), n=204, brightness=87.0
  - viewer_right_hand: RGB(139,73,53) from box (73, 47, 94, 84), n=168, brightness=88.3
  - top hair region: RGB(121,58,43), n=547
- Identity assertion: skin brightness 125.6 in/near medium-dark target
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=PASS)

### Gunslinger
- Source: `_STAGING\anchors\gunslinger\gunslinger_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (32,3)-(89,125), non-transparent=3707px (22.63% canvas), bbox density=51.96%, head top y=3, feet anchor y=125, height=123px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=488, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #FFB800 at pistol cylinder/barrel; pixels within tolerance=0 (tol=48 Euclidean RGB), nearest=RGB(221,166,53) Δ=65.5; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(184,120,76) from box (53, 13, 69, 36), n=213, brightness=126.7
  - chest: RGB(171,112,71) from box (52, 40, 70, 64), n=84, brightness=118.0
  - viewer_left_hand: RGB(136,83,50) from box (35, 46, 52, 84), n=123, brightness=89.7
  - viewer_right_hand: RGB(171,112,50) from box (70, 46, 87, 84), n=175, brightness=111.0
  - top hair region: RGB(109,40,19), n=473
- Identity assertion: skin brightness 111.3 supports medium/dark brown
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=PASS)

### Hexer
- Source: `_STAGING\anchors\hexer\hexer_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (24,4)-(91,125), non-transparent=4068px (24.83% canvas), bbox density=49.04%, head top y=4, feet anchor y=125, height=122px
- Outline: dominant dark boundary RGB(0,0,0), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=578, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #CCFF00 at lantern interior; pixels within tolerance=7 (tol=58 Euclidean RGB), nearest=RGB(196,239,39) Δ=42.9; centroid=(88.0, 67.4); components: area 5 bbox (86,67)-(88,69) centroid (87.2,68.2); area 2 bbox (90,65)-(90,66) centroid (90.0,65.5)
- Identity region RGB samples:
  - face: RGB(188,186,166) from box (49, 14, 67, 37), n=159, brightness=180.0
  - chest: RGB(91,87,66) from box (48, 41, 68, 65), n=248, brightness=81.3
  - viewer_left_hand: RGB(99,96,74) from box (27, 47, 48, 85), n=171, brightness=89.7
  - viewer_right_hand: RGB(146,145,122) from box (68, 47, 89, 85), n=177, brightness=137.7
  - top hair region: RGB(91,87,66), n=478
- Identity assertion: lantern/yellow-green cluster centroid: (88.0, 67.4) = viewer-right
- Identity assertion: lantern accent cluster weak
- Verdict: WARN (Canvas=PASS, Outline=PASS, Accent=WARN, Identity=WARN)

### Summoner
- Source: `_STAGING\anchors\summoner\summoner_anchor.png`
- Canvas: 128×128, mode=RGBA, alpha=Y, edge non-transparent=0/1008, corner non-transparent=0/4
- BBox: (24,4)-(99,125), non-transparent=5300px (32.35% canvas), bbox density=57.16%, head top y=4, feet anchor y=125, height=122px
- Outline: dominant dark boundary RGB(4,8,8), dark-boundary coverage within ±18 RGB distance=100.0%, boundary pixels=589, estimate=1px likely, single-color 95% rule=PASS
- Accent: expected #22FF88 at staff orb + palm glow; pixels within tolerance=0 (tol=58 Euclidean RGB), nearest=RGB(99,238,153) Δ=69.3; centroid=None; components: none
- Identity region RGB samples:
  - face: RGB(142,164,151) from box (52, 14, 72, 37), n=484, brightness=152.3
  - chest: RGB(237,224,183) from box (51, 41, 73, 65), n=21, brightness=214.7
  - viewer_left_hand: RGB(223,212,173) from box (28, 47, 51, 85), n=11, brightness=202.7
  - viewer_right_hand: RGB(235,224,190) from box (73, 47, 96, 85), n=12, brightness=216.7
  - top hair region: RGB(190,200,197), n=481
- Identity assertion: identity-specific assertion measured through accent + region samples
- Verdict: FAIL (Canvas=PASS, Outline=PASS, Accent=FAIL, Identity=PASS)

## Cross-Roster
- Present anchors: 10/10. Missing/skipped: none.
- Height median: 122.0px, std dev: 1.04px; target <8px = PASS.
- Canvas fill range: 21.77%–33.73%, std dev: 3.98 percentage points.
- Outline avg dominant color: RGB(2,3,3), std dev RGB=(2.7,3.6,3.9), max delta=9.3 on Ranger.
- Height deviations from median:
  - Warblade: 122px (+0.0%)
  - Elementalist: 124px (+1.6%)
  - Shadowblade: 123px (+0.8%)
  - Ranger: 122px (+0.0%)
  - Ravager: 121px (-0.8%)
  - Ronin: 122px (+0.0%)
  - Brawler: 120px (-1.6%)
  - Gunslinger: 123px (+0.8%)
  - Hexer: 122px (+0.0%)
  - Summoner: 122px (+0.0%)

## Failures Requiring Action
- Warblade: accent has only 0 pixels near #66AAFF
- Shadowblade: accent has only 8 pixels near #9933CC; purple accent connected components >=2px: 1; two dagger clusters not clearly separated by color scan
- Ranger: accent has only 0 pixels near #FFCC00; bow gold accent centroid: None = not found; bow/arrow gold accent weak
- Ravager: accent has only 0 pixels near #FF3322
- Brawler: accent has only 0 pixels near #FF8800
- Gunslinger: accent has only 0 pixels near #FFB800
- Hexer: accent has only 7 pixels near #CCFF00; lantern/yellow-green cluster centroid: (88.0, 67.4) = viewer-right; lantern accent cluster weak
- Summoner: accent has only 0 pixels near #22FF88

## Self-Check
- summary_has_10_rows: PASS
- per_character_has_10_sections: PASS
- cross_roster_present: PASS
- failures_section_present: PASS
- source_pngs_not_modified: PASS
