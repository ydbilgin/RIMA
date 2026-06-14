# S110 Phase 2 — Cliff manuel override + Erase blacklist + UI kategori

**Agent:** general-purpose Sonnet sub-agent (mekanik implementation)
**Effort:** ~1.5 saat (3-4 surgical edit + compile verify)

---

## Active rules

1. Think before coding. Varsayım listele.
2. Min code, no speculation, no refactor beyond listed scope.
3. Surgical — sadece aşağıdaki dosyalara dokun.
4. BLOCKED if unclear → orchestrator'a sor.

---

## Bağlam

S109 evening user talimatı (CURRENT_STATUS S110 Phase 2):

> Auto-regen kalır ama manuel cliff override mümkün olmalı — silinen cliff geri gelmemeli.

Şu an `CliffAutoPlacer.Regenerate()` her çağrıldığında `clearExistingOnRegenerate=true` ile tüm cliff'leri siliyor + algoritma'dan tekrar üretiyor. User manuel sildiği cliff cell'i bir sonraki regen'de geri gelmemeli — **blacklist** gerekiyor.

Aynı zamanda Antigravity Visual Editor `RimaVisualMapEditorWindow.cs` brush kategorilerinde **Cliff** yok — kullanıcı cliff'leri manuel brush ile boyayamıyor. AutoLayeringService de `BrushCategory.Cliff` için target tilemap mapping vermiyor.

Phase 3 cliff refactor LIVE (cliff artık floor cell üzerine konuyor). Bu task onun üzerine inşa eder.

---

## Görev 1: CliffAutoPlacer.cs — manualOverrideCells blacklist

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Environment\CliffAutoPlacer.cs`

**Aksiyon:**
1. Class'a serializable field ekle:
   ```csharp
   [SerializeField, HideInInspector] private List<Vector3Int> manualOverrideCellsSerialized = new List<Vector3Int>();
   private HashSet<Vector3Int> manualOverrideCells;
   ```
   Inspector görünmesin, sadece persistence için.

2. Property:
   ```csharp
   public HashSet<Vector3Int> ManualOverrideCells
   {
       get
       {
           if (manualOverrideCells == null)
               manualOverrideCells = new HashSet<Vector3Int>(manualOverrideCellsSerialized);
           return manualOverrideCells;
       }
   }
   ```

3. Public method:
   ```csharp
   public void AddManualOverride(Vector3Int cell)
   {
       ManualOverrideCells.Add(cell);
       SyncOverridesToSerialized();
   #if UNITY_EDITOR
       UnityEditor.EditorUtility.SetDirty(this);
   #endif
   }

   public void RemoveManualOverride(Vector3Int cell)
   {
       ManualOverrideCells.Remove(cell);
       SyncOverridesToSerialized();
   #if UNITY_EDITOR
       UnityEditor.EditorUtility.SetDirty(this);
   #endif
   }

   private void SyncOverridesToSerialized()
   {
       manualOverrideCellsSerialized.Clear();
       manualOverrideCellsSerialized.AddRange(ManualOverrideCells);
   }
   ```

4. `Regenerate()` içinde, `CollectCliffCells()` sonucundaki cell'leri filter et:
   ```csharp
   HashSet<Vector3Int> targets = CollectCliffCells();
   targets.ExceptWith(ManualOverrideCells);  // S110 Phase 2: skip blacklisted
   foreach (Vector3Int cell in targets) { ... }
   ```

5. Context menu eklemesi (manual reset için):
   ```csharp
   [ContextMenu("Clear Manual Overrides")]
   public void ClearManualOverrides()
   {
       ManualOverrideCells.Clear();
       SyncOverridesToSerialized();
   #if UNITY_EDITOR
       UnityEditor.EditorUtility.SetDirty(this);
   #endif
   }
   ```

**KORUNACAK:** mevcut `Regenerate`, `CollectCliffCells`, `IsReady`, iso vectors. `clearExistingOnRegenerate` aynı kalır.

---

## Görev 2: VisualEditorScenePainter.cs — Cliff erase blacklist hook

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\MapDesigner\VisualEditor\VisualEditorScenePainter.cs`

**Sorun:** Antigravity erase mode bir cliff cell'i sildiğinde, sonraki `CliffAutoPlacer.Regenerate()` (örn floor değişikliğinde tetiklenen) o cell'i geri koyar.

**Aksiyon:**
1. Erase mode handler'ı bul (muhtemelen `ApplyStroke` veya `EraseStroke` adlı method).
2. Eğer silinen cell **cliffTilemap**'tan ise (kategorisi BrushCategory.Cliff veya target tilemap = CliffTilemap), `CliffAutoPlacer.AddManualOverride(cell)` çağır.
3. Eğer silinen cell **floorTilemap**'tan ise, ilgili cliff cell'lerini (yani komşu floor'ları) blacklist'e EKLEME — sadece cliff direkt silindiyse.
4. CliffAutoPlacer reference nasıl elde edilecek: `UnityEngine.Object.FindObjectOfType<RIMA.Environment.CliffAutoPlacer>()` veya `RimaVisualMapEditorWindow` üzerinden inject. Sonnet karar versin — basit olan yöntemi seç.

**KORUNACAK:** mevcut paint logic, ghost preview, R rotation, undo group, `_dummyWalkableCache` (Phase 1).

---

## Görev 3: RimaVisualMapEditorWindow.cs — Cliff kategori ekle

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\MapDesigner\VisualEditor\RimaVisualMapEditorWindow.cs`

**Aksiyon:**
1. `categories` array veya `BrushCategory` enum kullanım yerini bul.
2. Mevcut kategoriler (örn `Floor`, `Wall`, `Prop`, `Decor` vb.) arasına `Cliff` ekle.
3. UI'da seçilebilir hale gelsin — toolbar/dropdown'da görünmeli.

**Eğer `BrushCategory` enum'da `Cliff` zaten varsa**, sadece UI listesine ekleyip diğer eksikleri gözden geçir.

---

## Görev 4: AutoLayeringService.cs — BrushCategory.Cliff → CliffTilemap mapping

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\MapDesigner\VisualEditor\AutoLayeringService.cs`

**Aksiyon:**
1. `FindTargetTilemap` method içindeki switch/if zincirini bul.
2. `BrushCategory.Cliff` case'i ekle. Hedef GameObject adı: `"CliffTilemap"` (sahnedeki var olan GO).
3. Eğer GO bulunamazsa null döndür — VisualEditor zaten null check yapıyor.

---

## Verification checklist

1. `grep -n "manualOverrideCells\|AddManualOverride\|RemoveManualOverride" Assets/Scripts/Environment/CliffAutoPlacer.cs` → en az 5 hit
2. `grep -n "ExceptWith" Assets/Scripts/Environment/CliffAutoPlacer.cs` → en az 1 hit
3. `grep -n "Cliff" Assets/Editor/MapDesigner/VisualEditor/RimaVisualMapEditorWindow.cs` → mevcut + en az 1 yeni hit
4. `grep -n "BrushCategory.Cliff\|\"Cliff\"\|CliffTilemap" Assets/Editor/MapDesigner/VisualEditor/AutoLayeringService.cs` → en az 2 hit
5. Unity compile error 0 (mcp__UnityMCP__read_console verify edecek)
6. Senin değişikliklerin LiveAutotiler veya CliffPlacementRules'a dokunmadı

---

## Çıktı

Inline raporla (Agent yanıtı):
- Her 4 görev için PASS / FAIL + dokunulan satır aralıkları
- Verification grep çıktıları
- Eğer enum/category yapısı varsayımım yanlışsa (örn `BrushCategory` enum yok, başka mekanizma var) BLOCKED + nasıl yapı var
- Eğer CliffAutoPlacer reference inject yöntemi belirsizse seçimini açıkla (singleton mu, find mı, serialized field mı)

**Refactor yapma. Yeni feature ekleme. Sadece bu 4 dosyada listelenen aksiyonları yap. Phase 3 cliff refactor (LIVE) bozma — `Regenerate()` ve `CollectCliffCells` interface aynı kalsın.**
