# Day 2.5 — MapFragmentSpawner DONE

## File

`Assets/Scripts/Environment/MapFragmentSpawner.cs`

LOC: 83

## Verification Checklist

- [x] namespace `RIMA.Environment` correct
- [x] `RoomLoader.OnRoomCleared` signature confirmed: `public static event Action OnRoomCleared` (line 12, no parameters) — handler `private void HandleRoomCleared()` matches
- [x] `MapFragment` runtime build: `new GameObject + AddComponent<MapFragment>()` — `[RequireComponent(SpriteRenderer, CircleCollider2D)]` auto-adds components, Awake procedural sprite build fires
- [x] `FragmentDropAnchor.roomType` field type `RoomTypeData` confirmed (line 20)
- [x] `RoomTypeData.RoomCategory` enum: Combat, Treasure, Ritual, BossApproach, Bridge (line 12) — exact match
- [x] Backward-compat: `gateOnBridgeFlag=true` default → gates on `MapFragmentBridge.useFragmentGateFlow=true`; Day 1 portal flow unaffected

## No modifications to existing files

Pure additive. 0 existing files changed.

## Pending (user action required)

Scene'e MapFragmentSpawner ekle:
1. Boş GameObject oluştur (e.g. "MapFragmentSpawner")
2. `MapFragmentSpawner` component ekle
3. `fragmentPrefab` alanını doldur (opsiyonel — null bırakılırsa runtime GO oluşturulur)
4. `gateOnBridgeFlag` = true (default) — Day 2 `MapFragmentBridge.useFragmentGateFlow` true olmalı
