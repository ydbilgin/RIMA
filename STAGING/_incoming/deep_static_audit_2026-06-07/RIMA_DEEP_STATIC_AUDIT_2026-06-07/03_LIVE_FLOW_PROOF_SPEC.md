# 03 — LIVE_FLOW_PROOF Spec

Claude'un ilk yapacağı şey.

## Amaç

Gerçek canlı demo hangi room/gate sistemini kullanıyor, bunu kanıtlamak.

## Sorular

1. `_IsoGame` veya aktif ana scene'de hangi component aktif?
   - `RoomLoader`
   - `RuntimeRoomManager`
   - `RoomRunDirector`
   - `IsoRoomBuilder`
   - `RoomTemplateSO`
   - `DungeonGraph`

2. Oda yükleme kimden geçiyor?
   - `RoomLoader.LoadFirstRoom/LoadNextInstance`
   - `RuntimeRoomManager.StartRoom/TransitionToRoom`
   - `RoomRunDirector`
   - başka builder

3. Kapı/portal spawn eden gerçek kod neresi?
   - `RoomLoader.BuildRoomContent`
   - `BuildExitDoors`
   - `DoorPortal` prefab
   - `Gate.cs` root

4. Oda temizlenince next-room geçiş sözleşmesi ne?
   - Linear `LoadNext()`
   - graph child choice
   - portal trigger index

## Çıktı dosyası

`STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md`

## Format

```md
# LIVE FLOW PROOF

## Active Scene
- scene:
- active room manager component:
- disabled/legacy manager components:

## Room Load Path
file/method chain:
1.
2.
3.

## Portal/Gate Spawn Path
file/method chain:
1.
2.
3.

## Choice/Branch Path
- 1 exit:
- 2 exit:
- 3 exit:

## Verdict
[ROOMLOADER_LIVE | TEMPLATE_BUILDER_LIVE | RRM_MANIFEST_LIVE | MIXED]

## Risk
- blocker:
- patch:
```

## Kabul kriteri

"Sanırım" yok. Dosya/method/scene component kanıtı var.
