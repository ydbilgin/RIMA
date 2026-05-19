# Codex Review: RoomTemplate backgroundSprite field + runtime integration

## Context

S91 Map Plan v1 LOCK = hibrit "monolithic painted background + gameplay overlay" (Hades model). RoomTemplateSO'ya `backgroundSprite` field eklenip RoomBankRuntimeTester runtime spawn'ında SpriteRenderer olarak instantiate edilecek.

Orchestrator (Claude) iki dosyada cerrahi değişiklik yaptı. Review hedefi: PASS/FAIL + regression risk değerlendirmesi.

## Değişen dosyalar (sadece bu 2)

1. `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`
2. `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs`

## Beklenen değişiklikler

### File 1: RoomTemplateSO.cs
- `prefabRef`'in HEMEN ALTINA eklenen 3 satır:
```csharp
[Header("Painted Background (Map Plan v1 LOCK — S91)")]
[Tooltip("Monolithic painted background sprite (Hades hybrid model). Null = no painted bg, fallback to procedural tile fill.")]
public Sprite backgroundSprite;
```
- `schemaVersion` "1.0" olarak KALDI (bump revert edildi, test compatibility için).
- Başka değişiklik yok.

### File 2: RoomBankRuntimeTester.cs
- `RunTest()` metodu içinde, prefabRef instantiate (if/else) bloğunun HEMEN SONRASINDA, `if (picked.playerSpawn == null)` check'inden ÖNCE şu blok eklendi:

```csharp
if (picked.backgroundSprite != null)
{
    GameObject bgGO = new GameObject("PaintedBackground");
    bgGO.transform.SetParent(result.roomInstance != null ? result.roomInstance.transform : transform, false);

    Vector3 bgPos = new Vector3(
        picked.bounds.xMin + picked.bounds.width * 0.5f,
        picked.bounds.yMin + picked.bounds.height * 0.5f,
        1f
    );
    bgGO.transform.position = bgPos;

    var sr = bgGO.AddComponent<SpriteRenderer>();
    sr.sprite = picked.backgroundSprite;
    sr.sortingOrder = -100;
    sr.drawMode = SpriteDrawMode.Simple;

    result.diagnostics.Add($"Painted background spawned at {bgPos}, sprite={picked.backgroundSprite.name}.");
}
else
{
    result.diagnostics.Add("backgroundSprite null; skipped painted bg spawn.");
}
```

## Review checklist (PASS/FAIL her satır)

1. **Diff cerrahi mi?** — Sadece bu 2 dosya değişti mi? `git diff --stat` ile teyit et.
2. **schemaVersion "1.0" değişmedi mi?** — Test'lerde `Assert.AreEqual("1.0", reloaded.schemaVersion)` var (RoomTemplateSaveLoadTests.cs:50). 
3. **Eski 10 RoomTemplate asset'i (Assets/Data/Rooms/Library/*.asset) hala deserialize edilebiliyor mu?** — `backgroundSprite` field null default olduğundan eski asset'ler etkilenmemeli. Unity Editor'da `AssetDatabase.LoadAssetAtPath<RoomTemplateSO>` 10 oda için error yok mu? (Bu Codex yapamayabilir, sadece kod inspection'a güven.)
4. **EditMode test suite hala PASS mi?** — 333/333 EditMode test'i çalıştır:
   ```bash
   # Unity batch test runner via MCP, veya manuel:
   # Unity > Window > General > Test Runner > EditMode > Run All
   ```
   Eğer Codex'in elinde UnityMCP yoksa, en azından test dosyalarındaki schemaVersion ve RoomTemplateSO field reference'larını grep ile kontrol et — yeni field hiçbir test'i bozmuyor mu.
5. **walkable grid logic'i bozulmadı mı?** — `IsWalkable(Vector2Int)` metoduna dokunulmadı, sadece yeni field eklendi. Confirm.
6. **Runtime null safety:**
   - `picked.backgroundSprite != null` check var → null sprite skip, log var.
   - `result.roomInstance != null ? ... : transform` ternary → prefabRef null durumunda bg standalone spawn ediyor (tester transform'una parent).
   - `picked.bounds` zaten RectInt struct (non-null guarantee).
7. **SpriteRenderer parametreleri mantıklı mı?**
   - sortingOrder = -100 → çok arka plan (gameplay overlay üstte kalır).
   - z = 1f → camera'dan uzak (perspective değil orthographic ama yine de güvenli z-order).
   - drawMode = Simple → sprite kendi pixel boyutunda render edilecek. (NOT: User PixelLab Pro'dan büyük sprite üretirse bounds'a fit etmesi gerekebilir — V2 işi, şu an scope dışı.)
8. **Test eklenmesi gerekir mi?** — Şu an EKLEME. Task #3 UnityMCP PlayMode test ile manuel verify edilecek. Codex sadece **mevcut testlerin geçtiğini** doğrulasın.

## Output beklediğim

`CODEX_DONE_bg_field_review.md` dosyasına şu format:

```markdown
# Codex Review: RoomTemplate backgroundSprite Field — VERDICT

## Verdict: PASS | FAIL

## Checklist
1. Diff cerrahi: PASS/FAIL (evidence: git diff --stat output)
2. schemaVersion: PASS/FAIL
3. Asset deserialization: PASS/FAIL/SKIPPED
4. EditMode test suite: PASS/FAIL/SKIPPED (test count)
5. Walkable grid: PASS/FAIL
6. Runtime null safety: PASS/FAIL
7. SpriteRenderer params: PASS/FAIL
8. Test addition needed: YES/NO

## Risk findings
[List specific issues, file:line citations]

## Recommendation
[GO TO NEXT TASK / FIX FIRST]
```

## Hard limits

- Sadece review yap, KOD DEĞİŞTİRME.
- 333/333 test runner çağıramazsan SKIPPED işaretle, evidence olarak grep'le bul.
- UnityMCP elinde varsa run_tests kullan, yoksa kod inspection yeterli.
- Memory/CURRENT_STATUS güncelleme YOK — Task #4'te yapılacak.
