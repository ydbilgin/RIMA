# Day 2 Gate + MapFragment — DONE

## Files
- MapFragment.cs — NEW, 130 LOC
- Gate.cs — NEW, 204 LOC
- MapFragmentBridge.cs — MODIFIED, +73 LOC (33 → 106 effective lines added in Day 2 region)

## Verification
- [x] namespace `RIMA.Environment` correct in all 3 files
- [x] `DraftManager.TriggerDraftFromFragment(Portal source)` — null accepted (line 134: `source != null ? ... : "<null>"`)
- [x] `RoomTypeData.cs` found at `Assets/Scripts/Environment/RoomTypeData.cs` — `RoomCategory` enum used for Gate tint
- [x] `RoomLoader.LoadNext()` — static method confirmed at line 47
- [x] Canonical values verified:
  - Bobbing: ±0.10u @ 2.2Hz ✓
  - Alpha pulse: 0.6–1.0 @ 3Hz ✓
  - Pickup radius: 2.5u ✓
  - Pickup key: G ✓
  - Fragment cyan: Color(0f, 1f, 0.8f, 1f) / #00FFCC ✓
  - Gate tint mapping: Combat=white, Elite/Boss=red/gold, Shop=gold, Spirit=purple, Event=green, Unknown=gray ✓
  - Gate open anim: 8 discrete frames × 0.05s = 0.4s, scaleY 1→0.1→1 squash + alpha 0.4→1.0 ✓
- [x] `useFragmentGateFlow = false` default — Day 1 portal subscription (`RefreshPortalSubscriptions`, `HandlePortalEntered`, `_armed` HashSet) untouched; `HandleSkillPicked` returns early to Day 2 branch only when flag is true
- [x] Scene files (.unity) not touched
- [x] DraftManager.cs / RoomLoader.cs signatures not modified

## Pending (user)
- Scene wire: drop MapFragment + Gate prefabs/GO's onto scene
- MapFragmentBridge inspector: set `useFragmentGateFlow = true`
- Tag: ensure player GameObject has tag `"Player"` (used by both MapFragment and Gate trigger checks)
- RoomType assignment: optionally wire a `RoomTypeData` SO to Gate's `roomType` field for tint
- Playtest flow: room cleared → fragment spawns → player walks to fragment → presses G → draft opens → skill selected → all AwaitingFragment gates unlock (squash anim) → player enters gate → RoomLoader.LoadNext()

## Compile check
Not done in this dispatch — orchestrator UnityMCP read_console ile doğrulayacak.
