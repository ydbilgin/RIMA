# TASK: PortalSpawnAnchor + FanLayoutSolver + Walkability Constraint

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Combat clear sonrası 1/2/3 portal yan yana spawn olsun. Her oda için 1 PortalSpawnAnchor (designer placement). FanLayoutSolver portal positions hesaplar. WalkabilityMap constraint — portal MAP DIŞINA, walkable olmayan cell'e KESİNLİKLE spawn olmaz. Placeholder visual (yarık sprite ileride swap), şimdi pure mantık + test edilebilir spawn.

## Sistem Mantığı

### Walkability-Constrained Fan Layout Kuralları
Anchor pozisyonu verilince N portal (1/2/3) için fan positions hesabı:

```
Default fan layout (anchor merkez, yatay):
  1 portal: [anchor]
  2 portal: [anchor - 0.6f * right, anchor + 0.6f * right]      → 1.2 unit total
  3 portal: [anchor - 1.2f * right, anchor, anchor + 1.2f * right]  → 2.4 unit total
```

### Constraint Algoritması (sırayla dene)
1. **Validate default fan:** her position için `WalkabilityMap.IsWalkable(cell)` check
2. **Eğer tüm positions walkable → ACCEPT**
3. **Eğer 1+ position walkable değil:**
   - **Adım A (Anchor adjust):** Walkable olmayan portal'ın counter-direction'una doğru anchor'ı 0.5 unit kaydır. Tekrar test. Max 4 iterasyon (toplam 2 unit shift).
   - **Adım B (Spacing compress):** Spacing 1.2 → 1.0 → 0.8 unit (min 0.8). Her aşamada test.
   - **Adım C (Rotation 30°):** Fan'ı 30° aşağı eğ (down-bias), tekrar test.
   - **Adım D (Reduce count):** Hala başarısızsa portal count azalt (3 → 2 → 1). Min 1 garantili (anchor'ın kendisi walkable olmalı).
4. **Eğer anchor bile walkable değil → BLOCKED warning + spawn iptal** (designer error)

### 1/2/3 Random Logic (RoomTypeData)
Her oda için weight set:
- **Combat:** %20 (1), %50 (2), %30 (3)
- **Treasure:** %0 (1), %30 (2), %70 (3)
- **Boss approach:** %100 (1)
- **Bridge:** %0 (1), %0 (2), %100 (3)
- **Default:** %20 / %40 / %40

## Hedef Dosyalar

### 1. `Assets/Scripts/Environment/PortalSpawnAnchor.cs` (yeni)
~30 satır MonoBehaviour. Designer scene'e yerleştirir, sadece marker.
```csharp
[ExecuteAlways]
public sealed class PortalSpawnAnchor : MonoBehaviour
{
    public RoomTypeData roomType;  // null → default weights
    public Vector2 fanDirection = Vector2.right;  // sağ-sol fan ekseni
    
    void OnDrawGizmos()
    {
        // Visual marker — anchor noktası + fan ekseni
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.DrawLine(transform.position - (Vector3)(fanDirection * 1.5f),
                        transform.position + (Vector3)(fanDirection * 1.5f));
    }
}
```

### 2. `Assets/Scripts/Environment/RoomTypeData.cs` (yeni)
~40 satır ScriptableObject. Per-room-type portal count weights.
```csharp
[CreateAssetMenu(menuName = "RIMA/Environment/Room Type Data")]
public sealed class RoomTypeData : ScriptableObject
{
    public enum RoomCategory { Combat, Treasure, Ritual, BossApproach, Bridge }
    public RoomCategory category;
    [Range(0,100)] public int weight1Portal = 20;
    [Range(0,100)] public int weight2Portal = 50;
    [Range(0,100)] public int weight3Portal = 30;

    public int PickPortalCount(System.Random rng)
    {
        int total = weight1Portal + weight2Portal + weight3Portal;
        if (total <= 0) return 1;
        int roll = rng.Next(total);
        if (roll < weight1Portal) return 1;
        if (roll < weight1Portal + weight2Portal) return 2;
        return 3;
    }
}
```

### 3. `Assets/Scripts/Environment/FanLayoutSolver.cs` (yeni)
~120 satır static utility class. Tüm constraint algoritması burada.
```csharp
public static class FanLayoutSolver
{
    public struct Result
    {
        public Vector3[] positions;
        public int finalCount;
        public bool blocked;  // anchor bile walkable değilse true
        public string adjustmentNote;
    }

    public static Result Solve(
        Vector3 anchor,
        Vector2 fanDir,
        int requestedCount,
        WalkabilityMap walkMap,
        Grid grid)
    {
        // 1) Validate anchor
        // 2) Default fan (1.2 spacing) → walkability check
        // 3) Adım A: anchor shift (max 4 iter)
        // 4) Adım B: spacing compress (1.2 → 1.0 → 0.8)
        // 5) Adım C: rotation 30° down
        // 6) Adım D: reduce count
        // 7) Return final positions
    }
}
```

### 4. `Assets/Scripts/Environment/Portal.cs` (yeni)
~50 satır MonoBehaviour. Placeholder visual (mavi/cyan basit cube veya quad sprite + trigger). Interactive (collider trigger), Player interact → log + invoke OnPortalEntered event (sonraki dispatch'te scene transition bağlanır).
```csharp
public sealed class Portal : MonoBehaviour
{
    public enum DestinationType { Combat, Treasure, Ritual, BossApproach, Bridge }
    public DestinationType destination;
    public System.Action<Portal> OnEntered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log($"[Portal] Entered: {destination}");
        OnEntered?.Invoke(this);
    }
}
```

### 5. `Assets/Scripts/Environment/PortalSpawnController.cs` (yeni)
~80 satır MonoBehaviour. Spawn orchestration.
- Public method `SpawnPortals(PortalSpawnAnchor anchor)`
- Random portal count from anchor.roomType
- FanLayoutSolver.Solve(...)
- Foreach position → Instantiate Portal prefab (yoksa runtime GameObject + Portal component + placeholder sprite)
- Optional: spawn animation hook (TODO comment, sonradan)

### 6. Test scene setup
- `PlayableArena_Test01.unity` içine manuel:
  - `PortalSpawnAnchor` GameObject ekle (Player'a 3-4 unit önde)
  - RoomTypeData asset oluştur (`Assets/ScriptableObjects/Environment/RoomType_TestCombat.asset`)
  - Anchor.roomType → asset bağla
  - `PortalSpawnController` GameObject ekle (singleton-ish)
- Test API: execute_code ile `PortalSpawnController.SpawnPortals(anchor)` çağırılabilir

## Test Senaryosu (kullanıcı manuel)
1. Sahnede anchor Player'ın önünde
2. Floor çiz (geniş arena)
3. execute_code veya context menu ile SpawnPortals trigger
4. Portallar yan yana belirir (placeholder cube)
5. **Edge case test:** Anchor'ı walkable kenara yakın yerleştir → fan dışarı taşacak → constraint algoritması anchor'ı içeri kaydırır VEYA spacing'i daraltır VEYA portal sayısını azaltır
6. Player portala yürür → trigger log

## Hard Constraints
- Placeholder visual yeterli (Portal.cs içinde basit SpriteRenderer + Color.cyan, asset gerek YOK). Yarık sprite ileride swap için public Sprite field bırak.
- Backward-compat YASAK
- Surgical — sadece listelenen 5 dosya + test scene manuel setup
- Walkability constraint **tam çalışmalı** — portal asla walkable olmayan cell'e spawn olmamalı
- BLOCKED: WalkabilityMap.Instance null ise, Grid component eksikse
- Commit YAPMA

## Önemli notlar
- **Codex YOK** — Sonnet (sen) UnityMCP + Edit ile yap
- WalkabilityMap.cs ZATEN VAR (`Assets/Scripts/Environment/WalkabilityMap.cs`) — kullan, yeniden yazma
- Portal sprite YOK şu an, placeholder ile başla — yarık sprite PixelLab'dan geliyor (ayrı job)
- RoomTypeData enum'unu basit tut (5 kategori yeterli, sonradan eklenir)
- Compile sonrası `read_console` ile 0 error

## Inline rapor (<500 kelime)
- 5 yeni dosya path + satır
- Test scene manuel setup adımları (kullanıcının ne yapması lazım)
- FanLayoutSolver algoritma özet (hangi adımlar implement edildi)
- Compile error count
- Manuel test simülasyonu (execute_code ile SpawnPortals çağrı + walkability constraint test sonucu)
- BLOCKED varsa neden
