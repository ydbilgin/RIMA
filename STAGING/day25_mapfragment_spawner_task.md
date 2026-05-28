# Day 2.5 — MapFragmentSpawner component

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: bu task dosyası + listelenen kod dosyaları.

## Amaç
Day 2 MapFragment.cs + Gate.cs + MapFragmentBridge.cs çalıştı (0 compile error). Eksik: room cleared olduğunda MapFragment'i kim instantiate edecek? Bu task pure additive bir spawner ekler. Hiçbir mevcut dosyayı dokundurmaz.

## Allowed files

### Read-only
- `Assets/Scripts/Environment/MapFragment.cs` (just verify type signature)
- `Assets/Scripts/Environment/FragmentDropAnchor.cs` (anchor lookup)
- `Assets/Scripts/Environment/RoomTypeData.cs` (RoomCategory enum)
- `Assets/Scripts/Systems/Map/RoomLoader.cs` (OnRoomCleared static event)

### Write
1. **NEW** `Assets/Scripts/Environment/MapFragmentSpawner.cs` (~70-90 LOC)

**YASAK:** Mevcut hiçbir dosya MODIFY edilmez. Sadece NEW file. Scene file ASLA.

## Spec — MapFragmentSpawner.cs

`RIMA.Environment.MapFragmentSpawner`:

```csharp
using UnityEngine;
using RIMA.Systems.Map;

namespace RIMA.Environment
{
    /// <summary>
    /// Day 2.5: Subscribes to RoomLoader.OnRoomCleared.
    /// Finds first FragmentDropAnchor in scene + instantiates a MapFragment GameObject
    /// at the anchor position. Drop count from anchor.roomType.category:
    /// Combat=1, BossApproach=1 (Elite), others=0.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MapFragmentSpawner : MonoBehaviour
    {
        [Tooltip("Optional fragment prefab. If null a runtime GO with MapFragment + SpriteRenderer + CircleCollider2D is built.")]
        public MapFragment fragmentPrefab;

        [Tooltip("If true, spawner activates only when MapFragmentBridge.useFragmentGateFlow is also true at scene level. Default true.")]
        public bool gateOnBridgeFlag = true;

        private void OnEnable() { RoomLoader.OnRoomCleared += HandleRoomCleared; }
        private void OnDisable() { RoomLoader.OnRoomCleared -= HandleRoomCleared; }

        private void HandleRoomCleared()
        {
            if (gateOnBridgeFlag)
            {
                // Find bridge in scene; bail if useFragmentGateFlow=false (Day 1 portal flow active)
                var bridge =
#if UNITY_2023_1_OR_NEWER
                    Object.FindFirstObjectByType<MapFragmentBridge>();
#else
                    Object.FindObjectOfType<MapFragmentBridge>();
#endif
                if (bridge == null || !bridge.useFragmentGateFlow)
                {
                    Debug.Log("[MapFragmentSpawner] Skipped — useFragmentGateFlow=false (Day 1 portal flow).");
                    return;
                }
            }

            // Find anchor
            FragmentDropAnchor anchor =
#if UNITY_2023_1_OR_NEWER
                Object.FindFirstObjectByType<FragmentDropAnchor>();
#else
                Object.FindObjectOfType<FragmentDropAnchor>();
#endif
            if (anchor == null) { Debug.LogWarning("[MapFragmentSpawner] No FragmentDropAnchor in scene."); return; }

            int count = DropCountForRoom(anchor.roomType);
            if (count <= 0) { Debug.Log("[MapFragmentSpawner] Drop count=0 for room type — skipping."); return; }

            for (int i = 0; i < count; i++) SpawnFragment(anchor.transform.position);
        }

        private static int DropCountForRoom(RoomTypeData rt)
        {
            if (rt == null) return 1; // default fallback
            switch (rt.category)
            {
                case RoomTypeData.RoomCategory.Combat:       return 1;
                case RoomTypeData.RoomCategory.BossApproach: return 1; // Elite
                default: return 0;
            }
        }

        private void SpawnFragment(Vector3 position)
        {
            MapFragment fragment;
            if (fragmentPrefab != null)
            {
                fragment = Instantiate(fragmentPrefab, position, Quaternion.identity);
            }
            else
            {
                var go = new GameObject("MapFragment_AutoSpawn");
                go.transform.position = position;
                // Required components auto-added via [RequireComponent] on MapFragment
                fragment = go.AddComponent<MapFragment>();
            }
            Debug.Log($"[MapFragmentSpawner] Spawned MapFragment at {position}");
        }
    }
}
```

## Verification (sub-agent self-check)

1. namespace `RIMA.Environment` ✓
2. RoomLoader.OnRoomCleared event signature: `public static event Action OnRoomCleared` (no parameters) → handler signature OK
3. MapFragment runtime build: `new GameObject + AddComponent<MapFragment>()` — `[RequireComponent(SpriteRenderer, CircleCollider2D)]` auto-adds, Awake will procedural-build sprite
4. FragmentDropAnchor existing field `roomType` (RoomTypeData) usage correct
5. RoomTypeData.RoomCategory enum: Combat, Treasure, Ritual, BossApproach, Bridge
6. Backward-compat: `gateOnBridgeFlag=true` default → spawner gates on `MapFragmentBridge.useFragmentGateFlow=true`. Day 1 portal flow ile çakışma yok.

## Output

`STAGING/day25_mapfragment_spawner_DONE.md`:
- File path
- LOC
- Verification checklist
- Pending (kullanıcı): scene'e MapFragmentSpawner GO ekle (boş GO + bu component)

## BLOCKED triggers
- RoomLoader.OnRoomCleared imzası farklı → BLOCKED detail
- MapFragment public API erişilemez (sealed iç görünür değil vb) → BLOCKED

## ÖZ

PUREadditive. 1 yeni dosya. Mevcut hiçbir şey değişmez. Sub-agent burada drift yapamaz.
