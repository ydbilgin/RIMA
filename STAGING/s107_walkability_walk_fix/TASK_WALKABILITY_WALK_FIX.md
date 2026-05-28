# TASK: Walkability WALK Fix (Player map dışına yürüyebiliyor)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Player **yürürken** walkable area dışına çıkamasın. Önceki dispatch (Walkability + Dash MVP) sadece dash pre-check ekledi — walk validation eksikti. Mevcut VoidBlocker tilemap'i Test01 sahnesinde BOŞ (cleared during scene creation), o yüzden Player rahatlıkla map dışına yürüyebiliyor.

## Çözüm Stratejisi (2 katman defense)

### Katman 1: VoidBlocker Auto-Fill (asıl çözüm)
WalkabilityMap'e `tilemapTileChanged` event subscribe — Floor değişince VoidBlocker otomatik inverse fill:
- Floor'a tile YAZILDIYSA → bounding box +1 cell genişlet
- Bounding box içindeki TÜM boş cell'lere VoidBlocker'a `voidTile` yerleştir
- CompositeCollider2D otomatik regenerate (Unity Tilemap+Composite default behavior)

Result: Floor değişikliği → VoidBlocker collider perimeter genişler → Player physics-side bloklanır.

### Katman 2: PlayerController Defansif Walkable Check (savunma)
Eğer VoidBlocker bir sebepten yok/yetersiz ise, PlayerController FixedUpdate'te next-position walkable check:
- `nextPos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime`
- `WalkabilityMap.Instance.IsWalkableWorld(nextPos)` false ise → velocity = 0, hareket yok
- Walkable ise → normal velocity-based move

İki katman birlikte: VoidBlocker fail-safe + PlayerController defansif.

## Hedef Dosyalar

### 1. `Assets/Scripts/Environment/WalkabilityMap.cs` (edit, extend)
Mevcut sınıfa ekle:
```csharp
public Tilemap voidBlockerTilemap;   // optional, auto-fill enabled if assigned
public TileBase voidTile;            // tile to place in empty cells

private void OnEnable()
{
    // existing static instance setup
    if (floorTilemap != null)
        Tilemap.tilemapTileChanged += OnAnyTileChanged;
}

private void OnDisable()
{
    Tilemap.tilemapTileChanged -= OnAnyTileChanged;
}

private void OnAnyTileChanged(Tilemap changedMap, Tilemap.SyncTile[] tiles)
{
    if (changedMap != floorTilemap) return;
    if (voidBlockerTilemap == null || voidTile == null) return;
    AutoFillVoidBlocker();
}

[ContextMenu("Auto-Fill VoidBlocker Now")]
public void AutoFillVoidBlocker()
{
    if (floorTilemap == null || voidBlockerTilemap == null || voidTile == null) return;
    
    // Clear existing void tiles first
    voidBlockerTilemap.ClearAllTiles();
    
    // Get floor bounds + 1 cell padding
    BoundsInt floorBounds = floorTilemap.cellBounds;
    BoundsInt expanded = new BoundsInt(
        floorBounds.xMin - 1, floorBounds.yMin - 1, 0,
        floorBounds.size.x + 2, floorBounds.size.y + 2, 1
    );
    
    // For every cell in expanded bounds:
    // if Floor empty → place voidTile
    foreach (Vector3Int cell in expanded.allPositionsWithin)
    {
        if (!floorTilemap.HasTile(cell))
        {
            voidBlockerTilemap.SetTile(cell, voidTile);
        }
    }
}
```

### 2. `Assets/Scripts/Player/PlayerController.cs` (edit, surgical FixedUpdate)
Mevcut FixedUpdate'te (büyük ihtimal `rb.linearVelocity = moveDir * moveSpeed` satırı var), önce walkable check ekle:
```csharp
// FixedUpdate içinde, velocity set etmeden önce
if (moveDir.sqrMagnitude > 0.01f && WalkabilityMap.Instance != null)
{
    Vector3 nextPos = transform.position + (Vector3)(moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    if (!WalkabilityMap.Instance.IsWalkableWorld(nextPos))
    {
        rb.linearVelocity = Vector2.zero;
        return;
    }
}
rb.linearVelocity = moveDir * moveSpeed;
```

### 3. Sahne setup (manuel veya execute_code)
`PlayableArena_Test01.unity`:
- WalkabilityMap GameObject zaten var → Inspector'da `voidBlockerTilemap` slot'una sahnedeki Floor/VoidBlocker tilemap'i sürükle
- `voidTile` slot'una standart bir tile (örneğin Tile asset, herhangi bir görünmez/şeffaf tile yeterli — collision için var olması yeterli, görsel önemsiz)
- Voidtile YOK ise: Sonnet basit bir `Tile` asset oluşturur `Assets/ScriptableObjects/Environment/VoidBlocker_Tile.asset` (sprite=null, colliderType=Grid)

### 4. Test sonrası verify
- execute_code ile manuel test:
  - Floor tilemap'e birkaç tile yaz → AutoFillVoidBlocker tetiklenmeli
  - VoidBlocker tile count > 0 olmalı
  - CompositeCollider2D bound check
- Sonnet bunu otomatik test edebilir veya kullanıcıya manuel doğrulama bırakır

## Hard Constraints
- Surgical — sadece 2 dosya edit + 1 asset (VoidBlocker_Tile.asset)
- PlayerController değişiklik MİNİMAL — sadece walkable check, refactor yok
- WalkabilityMap.cs mevcut yapı korunsun, sadece extend
- BLOCKED: PlayerController karmaşıksa veya WalkabilityMap.Instance pattern uyumsuzsa
- Commit YAPMA

## Inline rapor (<400 kelime)
- WalkabilityMap.cs eklemeler (satır)
- PlayerController.cs değişiklik (satır + ne)
- VoidBlocker_Tile.asset oluşturuldu mu, path
- Sahne setup adımları (kullanıcı manuel veya auto)
- Compile error count
- AutoFillVoidBlocker test sonucu (eğer simüle edildi ise)
- BLOCKED varsa neden
