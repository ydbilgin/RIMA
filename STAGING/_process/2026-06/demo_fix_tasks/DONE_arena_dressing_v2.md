# DONE — _Arena combat odası scene-child prop döşeme (v2)

VERDICT: **PASS** — 6 scene-child prop yerleştirildi, deterministik floor'a oturdu, nav temiz, console 0 error, _Arena kaydedildi.

## Kritik bulgu (Y-flip)
STAGING/rooms_json/*.json row-0=top (y=0 üst); ama **canonical `combat_large_cross_01.asset` Y-FLIP'li** (template y=0 = oyuncu spawn ucu/ön, y=17 = arka/boss). playerSpawn=(12,0), enemySpawns=(13,11),(5,9),(20,9),(11,5),(16,5). İlk yerleştirme JSON-y kullandı = dikey ayna → SİLİNDİ, template-space'e göre yeniden yerleştirildi.

## Yerleştirilen prop'lar (parent: `ArenaDressing` scene root; layer=Entities, Y-sort; solid'ler foot-collider Default-layer = room native prop'larla aynı)
| Prop | template cell | world | onFloor | collider | konum |
|---|---|---|---|---|---|
| BrokenPillar | (1,9) | (-3.84,3.22) | ✓ | solid | sol duvar dibi |
| StatueFragment | (24,9) | (7.20,9.95) | ✓ | solid | sağ duvar dibi (landmark) |
| IronBrazier | (8,4) | (1.92,3.80) | ✓ | solid | spawn-koridoru sol omuz |
| BarrelWeathered | (17,4) | (6.24,6.44) | ✓ | solid | spawn-koridoru sağ omuz |
| RubblePile | (9,16) | (-3.36,7.61) | ✓ | yok (floor) | arka kol sol dekor |
| ClothBanner | (16,16) | (0.00,9.65) | ✓ | yok (floor) | arka kol sağ dekor |

## Nav-test (Play, _Arena deterministik floorCells=281)
- KEEP-OPEN blocked=False: player(12,0), 5 enemy spawn, boss(13,11), combat-center(13,8), koridor. Hepsi AÇIK.
- Player(layer10)↔Default collide=True → solid'ler bloklar; brazier linecast room-wall'a takıldı (duvar ardı = yol kapatmıyor).
- Player combat-center'da dressing-overlap=False (serbest hareket). Canlı dalga: FractureImp+HalfThrall spawn etti.
- spawnProps=False, room Props container=0 (template prop render yok, dup yok). ArenaDressing runtime build'i atlattı (6/6 visible).

## Screenshot / console / state
- AFTER (overlay): `arena_dressing_AFTER_v2.png` · AFTER (clean): `arena_dressing_AFTER_clean.png` (hep _process/2026-06/demo_fix_tasks). BEFORE = oda edit-mode'da build olmuyor (runtime-only), bu yüzden anlamlı before = boş-overlay shot.
- read_console: 0 error/warning (play sırasında). Stop sonrası tek warning="Some objects were not cleaned up when closing the scene" = jenerik Unity play-exit teardown gürültüsü, prop'larımla ALAKASIZ (prop'lar script'siz inert GO).
- playModeStartScene MainMenu'ye GERİ YÜKLENDİ (no debug-state-leak); hidden reward-canvas/scrim/flash GERİ AÇILDI; _Arena saved (dirty=False).

NOT: estetik ilk-pass; kullanıcı konum/scale zevkine ayarlayabilir (scale 0.9–1.15 verildi).
