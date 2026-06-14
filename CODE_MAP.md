# CODE_MAP — RIMA Kod Haritası (insan-okunur)

> Fresh session için: "neyin kodu nerede". Pointer haritası, içerik değil.
> Tam AST grafiği (6925 node): `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`.
> Son güncelleme: 2026-06-15 · Kök: `Assets/Scripts/` (625 .cs, 37 alt-dizin).

---

## ⭐ God-node'lar (graphify en-bağlı 10'un öne çıkanları; 6/10 = editor/environment)

### Editor / Environment (RIMA'nın "environment" tezi — sunum centerpiece'i)
| Node | Dosya | Ne yapar |
|---|---|---|
| **BuildModeController** | `UI/BuildModeController.cs` | F2 runtime Build Mode toggle (tek-sahip F2 registry, WorkingTemplate, edit↔play) |
| **DirectorMode** | `UI/DirectorMode.cs` | backquote overlay — Stat slider / Spawn / Telemetry (CSV) |
| **RoomRunDirector** | `MapDesigner/Room/Runtime/RoomRunDirector.cs` | Oda yaşam-döngüsü: build → cleared → reward → door |
| **IsoRoomBuilder** | `MapDesigner/Room/Runtime/IsoRoomBuilder.cs` | Data (RoomTemplateSO) → sahne oda (cliff-tile yüzen ada) |
| **BuildPlacementController** | `UI/BuildMode/BuildPlacementController.cs` | Prop placement (palette→ghost→click) |

### Oyun (golden-path)
| Node | Dosya | Ne yapar |
|---|---|---|
| **RewardPickup** | `Core/RewardPickup.cs` | Oda temizlenince ödül; G ile topla → draft |
| **DraftManager** | `Skills/DraftManager.cs` | Kart seçim akışı (ShowDraft) |
| **RoomTemplateSO** | `MapDesigner/Room/Data/RoomTemplateSO.cs` | Oda verisi (ScriptableObject) |
| **PlayerController** | `Player/PlayerController.cs` | Oyuncu hareket/combat girişi |

---

## Dizin haritası (büyükten küçüğe, .cs sayısı)

| Dizin | #cs | İçerik |
|---|---|---|
| `MapDesigner/` | 98 | Oda sistemi, IsoRoomBuilder, RoomRunDirector, template/data |
| `Skills/` | 92 | Yetenek/kart sistemi, DraftManager, cross-class |
| `Systems/` | 52 | Çekirdek oyun sistemleri |
| `UI/` | 47 | BuildMode, DirectorMode, HUD, tooltip |
| `Combat/` | 45 | Dövüş, hasar, stat |
| `Core/` | 34 | RewardPickup, çekirdek akış |
| `Environment/` | 32 | Çevre, ışık, prop |
| `Enemies/` | 32 | Düşman AI/spawn |
| `Runtime/` | 26 | Runtime orchestration |
| `Editor/` | 25 | Unity editor araçları (RoomJsonImporter vb.) |
| `RoomPainter/` | 19 | (legacy paint — RIMA_LEGACY_MAPPAINT, off) |
| `Data/` 14 · `Rima/` 12 · `Map/` 10 · `Player/` 10 | — | veri/oyuncu/harita |

> Diğer küçük dizinler: Balance, CrossClass, Encounter, Obstacles, VFX, Save, Shop, Camera, Audio, Debug/Dev/DevTools/Test (geliştirme).

---

## Nasıl ararım
- Class bul: `Grep "class <Ad>" Assets` (Scripts `Assets/Scripts` altında ama bazı editor araçları `Assets/Editor`).
- Tasarım kararı: NLM notebook (gizli ID `.claude/nlm.local`).
- Anlık iş: `CURRENT_STATUS.md`. Giriş haritası: `PROJECT_INDEX.md`.
