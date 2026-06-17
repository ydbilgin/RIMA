# DONE — Act-1 odaları cliff-island capture (editor-mode revert, no-leak)

Tarih: 2026-06-18. Tek Unity ajanı (seri). LOCKED karar uygulandı: `COUNCIL_ROOM_CAPTURE_DECISION.md`.

## Sonuç: PASS (verify-gate geçti)

## Kullanılan API / yöntem
- Canonical **IsoRoomBuilder.Build(RoomTemplateSO)** (`Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs:96`) — procedural cliff/void/boundary üretir.
- Oda kaynağı: Act-1 JSON (`Assets/Data/Map/Act1_ShatteredKeep/json/act1_*.json`) → **transient RoomTemplateSO** (HideFlags.DontSave). 6 odanın hepsi `shape: rectangle` → full-bounds walkable (`walkableGrid = empty` = fallback all-walkable). Faithful, conversion sadece rectangle footprint.
- Not: RoomBank (DemoRoomBank) RoomTemplateSO asset'leri generic (combat_large_* vb.), isimli Act-1 odaları DEĞİL → LOCKED fallback (JSON→transient) kullanıldı.
- `IsoRoomBuilder.spawnProps = false` (temiz ada; transient template'in prop'u yok zaten).
- Capture: **_Arena game camera** (Main Camera, ortho, void bg #0D0A14, Global Light 2D rig) ile `manage_camera capture_source=game_view`. SceneView DEĞİL — void/Light2D rig'i sadece game camera faithful render eder. Kamera her oda için floor world-bounds'a (TryGetLastFloorWorldBounds) konumlandı + ortho-size aspect-fit.

## Verify-gate (ZORUNLU, ada doğrulandı mı)
- İlk oda (entry_hall) capture → AÇILDI + incelendi: **cliff kenarları VAR** (cyan-venli procedural cliff faces), **void arka plan VAR** (siyah + mor void glow), **ışık VAR** (teal rift glow + Global Light 2D). `_Arena` yüzen-ada görünümüyle birebir. → GATE PASS, kalan 5 batch'lendi.
- 6/6 oda incelendi, hepsi authentic floating cliff-island. Önceki yanlış denemenin düz/moloz-zeminli çıktısı TEKRARLANMADI.

## Üretilen dosyalar (STAGING/report/figures_2026-06-18/rooms_island/)
| Oda | Boyut | PNG |
|---|---|---|
| Entry Hall | 32x24 | act1_entry_hall.png |
| West Chamber | 24x18 | act1_west_chamber.png |
| East Corridor | 8x24 | act1_east_corridor.png |
| Treasure Vault | 16x12 | act1_treasure_vault.png |
| North Antechamber | 20x16 | act1_north_antechamber.png |
| Shattered Throne | 40x30 | act1_shattered_throne.png |

- Contact sheet (2x3, etiketli): `STAGING/report/figures_2026-06-18/fig_rooms_island_grid.png` (1632x782)

## No-leak teyidi (HARD)
- _Arena diskten yeniden açıldı (Build/kamera mutasyonları KAYDEDİLMEDEN atıldı). Final: aktif sahne `_Arena`, **isDirty=false**, rootCount=10 (başlangıçla aynı).
- Hiçbir sahne kaydedilmedi. playModeStartScene'e dokunulmadı. Play mode'a girilmedi.
- read_console: **0 error, 0 warning**.
