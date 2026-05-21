# UIUX Bug Fix + User-Friendly — Codex Report

## Bug 1 Fix: Multi-Folder Wall Scanner
- ScanWallPrefabsMultiFolder helper added
- Folders: Assets/Prefabs/Props/ShatteredKeep_PixelLab, Assets/Prefabs/Walls/pilot_a, Assets/Prefabs/Walls
- Name patterns: case-insensitive contains "wall"
- Painter palette pilot_a_wall_* + legacy wall_*: PASS
- Smoke result: wallCount=7; pilot_a_wall_face_EW=True; pilot_a_wall_corner_outer=True; pilot_a_wall_arch_opening=True; legacy wall_*=True
- Files touched: RimaUnifiedPainterWindow.cs

## Bug 2 Fix: Generic Wall Naming Detection
- IsWallObject helper: case-insensitive contains "wall" on GameObject name or Sprite name
- FindWallAtCell + RebuildAllWallConnections refactored to use IsWallObject
- Auto-connect family selection added for generic wall families such as pilot_a_wall_* and act1_wall_*
- Auto-connect pilot_a_wall_*: PASS
- Smoke result: 3-cell corner test produced pilot_a_wall_corner_outer

## Bug 3 Fix: Parent-Compensated Scale
- Panel 5 Boyut field now edits uniform world scale
- SetUniformWorldScale uses target local scale = worldScale / parent.lossyScale.y
- Grid parent test: scale preserved YES
- Smoke result: parent scale (1, 0.5, 1), Boyut 1.0 => localY=2, worldY=1
- Props_Root parent test: identity preserved YES

## Bug 4 Fix: IsoSorter Wall Skip
- ApplySorting detects PaletteCategory.Wall / generic wall naming and does not add IsoSorter
- Wall renderers still receive CollisionResolver sorting layer/order
- Existing wall IsoSorter cleanup path added through ApplySorting and AttachIsoSorterToAllPlacedObjects
- Mevcut sahne wall IsoSorter cleanup: helper removes existing wall IsoSorter when touched by sorting attach/apply flow
- CollisionResolver sortingOrder=20 effective
- Smoke result: wallSorterRemoved=True; wallOrder=20; mobSorterAdded=True; mobOrder=40

## User-Friendly Label Revize
- 15+ field label Türkçeleştirildi: Tür, Seçili Fırça, Aktif Biyom, Hedef Zemin, Hedef Klasör, Fırça, Silgi, Damlalık, Hücreye Hizala, Fırça Boyutu, Çeşitlilik, Dönme, Boyut Çarpanı, Tabanı Hizala, Konum İnce Ayar, Duvarları Otomatik Bağla, Duvar Bozulması Rastgele, Çarpışma, Klasör değiştir, Seçili Obje, Boyut, Sil
- 3 technical field hidden from user-facing collision UI: sortingLayer, sortingOrder, spritePivot
- Panel 5 sadeleştirildi around Seçili Obje, Tür, Boyut, Dönme, Scene'de Düzenle, Sil, Klasör değiştir
- BoxCollider2D size/offset Vector2 editing removed from Panel 5; Scene handle message/button retained
- Tooltip added for edited user-facing fields

## Wall Randomize Variants Hazırlık
- WallVariantGroup data structure added
- wallVariantGroups serialized list added
- Empty pool fallback: single variant paint
- useRandomVariants now routes wall prefab picks through GetRandomWallVariantFromGroup
- Trigger gate: state üretildiğinde serialized groups can activate without changing paint flow

## Verify
- dotnet build: 0 error
- Command: dotnet build .\RIMA.slnx
- Painter smoke: PASS
- Palette scan: PASS
- Pilot A wall paint + corner auto-connect: PASS
- Panel 5 Boyut parent compensation: PASS
- IsoSorter wall cleanup / mob retention: PASS
- Console: clean, 0 error entries after smoke checks
- Türkçe labels applied: YES

## Git Diff Summary
- RimaUnifiedPainterWindow.cs: +1316 -355 lines in current working-tree diff
- Note: file already had uncommitted local edits before this task; diff count is working tree vs HEAD

## Açık Sorular
- None
