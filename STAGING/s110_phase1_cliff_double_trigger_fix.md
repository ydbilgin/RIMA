# S110 Phase 1 — Cliff double-trigger fix + 500×500 allocation cache

**Agent:** general-purpose Sonnet sub-agent (mekanik implementation)
**Effort:** ~1 saat (2 surgical edit + compile verify)

---

## Active rules

1. Think before coding. Varsayım listele.
2. Min code, no speculation, no refactor beyond listed files.
3. Surgical — sadece aşağıdaki 2 dosyaya dokun.
4. BLOCKED if unclear → orchestrator'a sor.

---

## Bağlam

S109 evening user feedback (`feedback_double_auto_regen_avoid.md`):

> DOUBLE auto-trigger anti-pattern. MonoBehaviour event subscription + Editor-side trigger = çift execute.
> CliffAutoPlacer'a eklediğim `tilemapTileChanged` + Antigravity LiveAutotiler MouseUp aynı anda → S110 Phase 1 fix.

Şu an floor paint/erase yapıldığında iki ayrı kaynaktan cliff regen tetikleniyor:
1. **CliffAutoPlacer.OnEnable** → `Tilemap.tilemapTileChanged` event subscription → her tile değişikliğinde `Regenerate()`
2. **LiveAutotiler.TriggerLiveAutotile** (Antigravity Visual Editor) → MouseUp'ta `CliffAutoPlacer.Regenerate()`

İkincisi daha mantıklı (stroke bittikten sonra tek seferde regen) — birincisi her cell değişiminde çağrılıyor + Visual Editor stroke içinde 100+ cell paint edilebilir → 100+ regen çağrısı. Performans + race risk.

**Karar:** `CliffAutoPlacer`'dan event subscription'ı KALDIR. LiveAutotiler tek trigger kaynağı kalsın.

---

## Görev 1: CliffAutoPlacer.cs — remove tilemapTileChanged subscription

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Environment\CliffAutoPlacer.cs`

**Aksiyon:**
1. `OnEnable()` metodunda `Tilemap.tilemapTileChanged += OnTilemapTileChanged;` satırını sil (varsa).
2. `OnDisable()` metodunda `Tilemap.tilemapTileChanged -= OnTilemapTileChanged;` satırını sil (varsa).
3. `OnTilemapTileChanged(Tilemap, Tilemap.SyncTile[])` method'unu **komple sil** (artık çağıran yok).
4. Eğer S109 evening commit'lerinde başka event handler eklenmişse (örn floor tilemap callback), onları da kaldır.

**KORUNACAK:**
- Public `Regenerate()` method (LiveAutotiler hala çağırıyor)
- Editor inspector button `Regenerate Cliffs`
- Tüm cliff placement logic (iso vectors, spike filter, 3-direction S/SE/SW)

**Verify:** `grep -n "tilemapTileChanged" Assets/Scripts/Environment/CliffAutoPlacer.cs` → 0 hit beklenir.

---

## Görev 2: VisualEditorScenePainter.cs — 500×500 allocation cache

**Dosya:** `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Editor\MapDesigner\VisualEditor\VisualEditorScenePainter.cs`

**Sorun:** `ApplyStroke` metodunda her çağrıda `new bool[500, 500]` (250K element) allocate ediliyor. Stroke içinde çok defa çağrılırsa GC pressure.

**Aksiyon:**
1. Class-level `private static bool[,] _dummyWalkableCache;` field ekle.
2. `ApplyStroke` içinde `new bool[500, 500]` ifadesini ara. Bunun yerine:
   ```csharp
   if (_dummyWalkableCache == null || _dummyWalkableCache.GetLength(0) != 500)
       _dummyWalkableCache = new bool[500, 500];
   // (varsa) clear: Array.Clear(_dummyWalkableCache, 0, _dummyWalkableCache.Length);
   ```
3. Kullanıcı `RoomData` constructor'una geçiriyor olabilir — referansı paylaştığında, sonradan modify edilirse cache kirlenmesin diye `Array.Clear` ile clear et (eğer her stroke'ta fresh false grid bekleniyorsa).

**KORUNACAK:**
- ApplyStroke'un dışındaki tüm logic (grid snap, ghost preview, R rotation, undo group, BrushExecutorRouter call)
- 500 magic number değişmez (RoomData expected size)

**Verify:** `grep -n "new bool\[500" Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` → 1 hit beklenir (cache init bloğu içinde).

---

## Verification checklist

1. `grep -n "tilemapTileChanged" Assets/Scripts/Environment/CliffAutoPlacer.cs` → 0
2. `grep -n "OnTilemapTileChanged" Assets/Scripts/Environment/` → 0 (method ve subscription kalktı)
3. `grep -n "new bool\[500" Assets/Editor/MapDesigner/VisualEditor/VisualEditorScenePainter.cs` → 1 (cache init only)
4. Unity console kontrol — compile error yok, sadece bilinen Phase 2 stub uyarıları kalabilir
5. Senin değişikliklerin LiveAutotiler veya AutoLayeringService dosyalarına dokunmadı

---

## Çıktı

Inline raporla (orchestrator'a Agent yanıtı olarak):
- Her görev için: PASS / FAIL + dokunulan satır aralıkları
- Verification grep komutlarının çıktıları
- Eğer LiveAutotiler tetikleme zinciri kırıldıysa veya başka kod CliffAutoPlacer.tilemapTileChanged'i bekliyorsa BLOCKED + sebep

**Refactor yapma. Yeni feature ekleme. Sadece bu iki dosyada listelenen aksiyonları yap.**
