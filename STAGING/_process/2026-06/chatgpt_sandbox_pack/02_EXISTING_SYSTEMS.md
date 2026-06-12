# 02 — Hook'lanacak Mevcut Runtime Sistemleri (doğrulandı)

Sekmeler sıfırdan değil, bu mevcut runtime-çağrılabilir sistemlere bağlanır:

| Sistem | Dosya | Ana API | Not |
|---|---|---|---|
| **Tile paint** | `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` | `PaintCell(cell,bool)` (şu an private + IMGUI) | F2 overlay zaten var; refactor: PaintCell public yap, IMGUI sök, uGUI bağla |
| **Cliff gen** | `Assets/Scripts/Environment/CliffAutoPlacer.cs` | `Regenerate()` | + `CliffPlacementRules`, `DirectionalCliffTile` (8-dir + height variation), manual override (AddManualOverride/Painted) |
| **Prop place** | `Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs` | `Generate(template, roleMap, pool, seed, density)` | + `RoomDecorationPass.Apply()`, `PropFootprintValidator.Validate()` |
| **Room build** | `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` | `Build(template, seed)` | floor+overlay+cliff+props cascade |
| **Spawn** | `EncounterController` | `SpawnEnemy(id, pos)` | ⚠️ public imza DOĞRULANMALI (cx ilk iş) |
| **Map graph** | `Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs` | `Generate(seed, depth)` | + `RoomRunDirector.JumpToNode()` |
| **Class swap** | `Assets/Scripts/Systems/PlayerClassManager.cs` | `SetPrimaryClass(classType)` | sprite/animator/profile swap |
| **Skill** | `DraftManager` | draft override | skill loadout |
| **Stat (yeni)** | `ClassStatProfile` / `ClassStatRuntime` | runtime copy, slider→canlı | dengeleme kararı kilitlendi (bkz 04) |

**Hepsi runtime-safe.** Tile/cliff/prop sistemleri `InPlayMapPaintOverlay` (F2) ile zaten oyun içinde çalışıyor — sandbox bunları absorbe edip uGUI ile güzelleştirecek.
