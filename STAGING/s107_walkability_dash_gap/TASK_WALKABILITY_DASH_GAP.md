# TASK: Walkable Area Validation + Dash Gap-Cross MVP

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Karakter walkable alan dışına çıkamasın (map dışı), AMA dash ile uygun mesafeli gap'i atlayabilsin. Hades pattern — dash sırasında karakter kinematic mode, void ignore, hedef cell walkable ise dash başlar; değilse dash başlamaz (input registered, no movement). MVP scope — sadece walkable/void ayrımı + dash gap-cross. Passable obstacle (kemik vb.) ve destructible statue ileride ayrı dispatch.

## Spec (kullanıcı tarafından verildi)
1. Karakter walkable alan (floor tile'ları) dışına çıkamaz. Map dışı yasak.
2. Walkable tile'lar arasında gap olabilir (1-N tile void).
3. Dash range ≥ gap mesafesi → dash atlanır, karakter karşıya geçer.
4. Dash range < gap mesafesi → dash BAŞLAMAZ (input alındı ama karakter olduğu yerde kalır — "olduğum yerde kalacam"). Animasyon/ses trigger OK, hareket NO.
5. (Future) Üstünden geçilebilen obje (Passable) — walk + dash ikisi de allow
6. (Future) Statü/duvar (Blocking) — walk block, dash allow, bazı destructible (Hades pattern damage receive)
7. (Future) Skill'ler — bazılarının canBypassObstacle flag'i var

**Bu MVP sadece 1-4'ü kapsar.** 5-7 için extension hook (interface) bırak, implement etme.

## Hedef dosyalar

### 1. `Assets/Scripts/Environment/WalkabilityMap.cs` (yeni)
~60-80 satır. MonoBehaviour, ExecuteAlways.
```csharp
public sealed class WalkabilityMap : MonoBehaviour
{
    public Tilemap floorTilemap;
    public List<MonoBehaviour> obstacleSources = new();  // future IObstacle providers

    public bool IsWalkable(Vector3Int cell)
    {
        if (floorTilemap == null) return false;
        if (!floorTilemap.HasTile(cell)) return false;  // no floor = not walkable
        // future: obstacleSources iteration for IObstacle.IsWalkable check
        return true;
    }

    public bool IsWalkableWorld(Vector3 worldPos)
    {
        if (floorTilemap == null) return false;
        Vector3Int cell = floorTilemap.WorldToCell(worldPos);
        return IsWalkable(cell);
    }

    public bool IsDashable(Vector3Int cell)
    {
        // For MVP: dashable = walkable (no blocking obstacles yet)
        // Future: blocking obstacles allow dash but not walk
        return IsWalkable(cell);
    }
}
```

### 2. `Assets/Scripts/Environment/IObstacle.cs` (yeni)
~20 satır interface. Future extension için, MVP'de implementer yok.
```csharp
namespace RIMA.Environment
{
    public interface IObstacle
    {
        Vector3Int Cell { get; }
        bool IsWalkable { get; }       // can walk through?
        bool IsDashable { get; }       // can dash through?
        bool IsSkillable { get; }      // can skill bypass?
        bool TakesDamage { get; }      // destructible?
    }
}
```

### 3. `Assets/Scripts/Player/PlayerController.cs` (edit, surgical)
Mevcut dash logic'i bul — büyük ihtimal velocity-based veya coroutine. **DEĞIŞIKLIK:**
- Dash trigger noktasında (input registered, cooldown OK) **pre-check ekle**:
  ```csharp
  // Calculate dash end position (current + facingDir * dashRange)
  Vector3 dashEnd = transform.position + (Vector3)(dashDirection * dashRangeUnits);
  WalkabilityMap walkMap = FindObjectOfType<WalkabilityMap>();  // veya cache
  if (walkMap == null || !walkMap.IsWalkableWorld(dashEnd))
  {
      // Optional: trigger dash-fail animation/sound but no movement
      // For MVP: just return, dash does not start
      return;
  }
  // ELSE: proceed with existing dash logic
  ```
- Dash sırasında **collider ignore void** — eğer karakter Rigidbody2D üzerinde collider VoidBlocker ile etkileşiyorsa, dash boyunca `Physics2D.IgnoreCollision(playerCollider, voidCollider, true)`. Dash bitince `false`.
- Eğer dash mevcut implementasyonda gap'leri zaten geçebiliyorsa (kinematic mode + range), sadece pre-check yeterli.

### 4. WalkabilityMap GameObject sahneye ekle
Auto-create veya manuel:
- `PlayableArena_Test01.unity` içine yeni GameObject "WalkabilityMap"
- WalkabilityMap component ekle
- `floorTilemap` → sahnedeki Floor Tilemap (scene'in Grid altındaki)
- Save scene

VEYA `CliffGenerateAction.AutoCreatePlacer` pattern'i taklit eden bir `WalkabilityMap.AutoCreate()` editor utility yaz — eğer sahnede yoksa otomatik yaratır + Floor Tilemap auto-bind. Editor static class.

Tercih: **Manuel** önce — auto-create gelecek dispatch'te (Walk + Dash test edilince).

### 5. (Optional, sadece if needed) `DashRange` cell hesabı
- Mevcut dashSpeed=18, dashDuration=0.15 → range = 2.7 unit
- `dashRangeUnits` calculate veya yeni public field
- Spec "3 tile dash" örnek — şu an MVP için MEVCUT range korunsun, balance ayrı karar

## Test Senaryosu
1. PlayableArena_Test01 scene aç
2. Floor'da bilinçli 1-3 tile boşluk bırak (painter ile)
3. Player gap kenarına git
4. Dash dene:
   - 1 tile gap + dash range 2.7 → ATLAR ✓
   - 3 tile gap + dash range 2.7 → ATLAMAZ, dash başlamaz ✓
5. Walkable dışına yürümeye çalış → VoidBlocker engeller (zaten var)

## Hard Constraints
- Backward-compat YASAK — yeni IObstacle interface temiz
- Surgical — sadece listelenen dosyalar
- BLOCKED: PlayerController.cs dash logic karmaşık ise (state machine, animator-driven), refactor scope büyürse raporla, dash logic'i tamamen rewrite ETME
- Commit YAPMA

## Inline rapor (<500 kelime, NOT file)
- Yeni dosyalar (path + satır sayısı)
- PlayerController değişiklik (satır numarası + ne)
- Sahnede WalkabilityMap GameObject manuel mi auto-create mi
- Compile error count (0 hedef)
- Mevcut dash logic özet (1-2 cümle) — gelecek dispatch için
- BLOCKED varsa neden

## Önemli notlar (yeni RIMA pipeline kuralları)
- **Codex YOK** — sen (Sonnet) UnityMCP + Edit ile yapacaksın
- Image gen GEREK YOK — pure code
- Test simülasyonu opsiyonel — kullanıcı manuel test edecek
