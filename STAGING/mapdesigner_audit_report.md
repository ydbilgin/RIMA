## MapDesigner Audit - 2026-05-22

### Summary
- Total files scanned: 94
- LIVE: 92
- NEEDS_ADAPT: 1
- ARCHIVE: 1

### Per-file classification

| Path | Class | Status | Reason | Lines to adapt (if NEEDS_ADAPT) |
|---|---|---|---|---|
| Assets/Scripts/MapDesigner/WallOverlayPainter.cs | WallOverlayPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/VoronoiWaterFeatureGenerator.cs | VoronoiWaterFeatureGenerator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/VoronoiElevationFeatureGenerator.cs | VoronoiElevationFeatureGenerator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/TransitionBrushPainter.cs | TransitionBrushPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/SimpleWASDMover.cs | SimpleWASDMover | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/SimpleCameraFollow.cs | SimpleCameraFollow | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/ScatterBrushSO.cs | ScatterEntry | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/ScatterBrushPainter.cs | ScatterBrushPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/Map/RoomBuilder.cs | RoomBuilder | NEEDS_ADAPT | Room generation logic is useful, but helper creates IsoGrid with GridLayout.CellLayout.Isometric. | 320-328 replace IsoGrid name/isometric GridLayout/cell size with top-down Room grid defaults; 345 and 356 remove IsoGrid child assumptions |
| Assets/Scripts/Map/ObstaclePrefabBuilder.cs | ObstaclePrefabBuilder | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/FeatureEdgeSmoothingPass.cs | FeatureEdgeSmoothingPass | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Encounter/EncounterTemplateValidator.cs | ValidationResult | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Validation/RoomValidationIssue.cs | ValidationSeverity | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs | RoomTemplateValidator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs | SubRoomLink | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Encounter/Data/SubRoomEntry.cs | SubRoomEntry | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs | EncounterTemplateSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/DetailDecalPainter.cs | DetailDecalPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs | RoomTestResult | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Composition/WangContextResolver.cs | WangContextResolver | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Composition/CompositionRoleMapGenerator.cs | CompositionRoleMapGenerator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Composition/CompositionRoleMap.cs | CompositionRoleMap | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Composition/CompositionRole.cs | CompositionRole | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/PatchOverlayPainter.cs | PatchOverlayPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/NaturalFeatureGraph.cs | FeatureType | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/MapLayerOrchestrator.cs | MapLayerOrchestrator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Editor/SaveLoadResults.cs | SaveResult | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs | RoomTemplateSaver | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateMenu.cs | RoomTemplateMenu | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateLoader.cs | RoomTemplateLoader | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Stroke/BrushStroke.cs | BrushStroke | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Editor/SliceTemplateFactory.cs | SliceTemplateFactory | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Editor/BrushVariantPreviewWindow.cs | BrushVariantPreviewWindow | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Editor/BrushAtlasImportMenu.cs | BrushAtlasImportMenu | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/ZoneToLayerMappingSO.cs | ZoneToLayerMappingSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs | RoomTemplateSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs | RoomBankSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/PlayerSpawnSocket.cs | PlayerSpawnSocket | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs | EnemySpawnSocket | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs | DoorSocket | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/CameraBounds.cs | CameraBounds | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Room/Data/BackgroundLayerData.cs | BackgroundLayerData | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/AccentPainter.cs | AccentPainter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/AssetPoolSO.cs | AssetPoolSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/AssetPackManifestSO.cs | AssetPackManifestSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BiomeSkinSO.cs | BiomeSkinSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs | BrushAssetVariant | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/ValidationIssue.cs | ValidationIssue | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/SliceLayoutTemplateSO.cs | SliceLayoutTemplateSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/RoomDecalDataSO.cs | RoomDecalDataSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/MapDesignerBrushPresetSO.cs | MapDesignerBrushPresetSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/ImportResult.cs | ImportResult | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/Enums.cs | BrushCategory | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushRadiusProfileSO.cs | BrushRadiusProfileSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushPipelineConfigSO.cs | BrushPipelineConfigSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushPackSO.cs | BrushPackSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushLayerOperation.cs | BrushLayerOperation | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Data/BrushJsonSerializer.cs | BrushJsonSerializer | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs | EncounterPlacement | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Import/Editor/WangSliceGenerator.cs | WangSliceGenerator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasValidator.cs | BrushAtlasValidator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Import/Editor/BrushAtlasImporter.cs | BrushAtlasImporter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Runtime/RoomDecalChunkRenderer.cs | RoomDecalChunkRenderer | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Automation/Editor/SmartFillSelection.cs | SmartFillSelection | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Automation/Editor/RegenerateDecorativeLayers.cs | RegenerateDecorativeLayers | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Automation/Editor/BrushAlongEdgesAutomation.cs | BrushAlongEdgesAutomation | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Automation/Editor/AutoDressRoom.cs | AutoDressRoom | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Karar143Enforcement.cs | Karar143Enforcement | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/IBrushExecutor.cs | IBrushExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Render/Editor/BiomeSkinApplier.cs | BiomeSkinApplier | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/WallStampExecutor.cs | WallStampExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/StampExecutor.cs | StampExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/ScatterAlongStrokeExecutor.cs | ScatterAlongStrokeExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/ScatterAlongStrokeDataExecutor.cs | ScatterAlongStrokeDataExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/GridTileExecutor.cs | GridTileExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs | FreeformDecalExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalDataExecutor.cs | FreeformDecalDataExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/EraseByLayerExecutor.cs | EraseByLayerExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/EraseAllDecorativeExecutor.cs | EraseAllDecorativeExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/CompositeStrokeExecutor.cs | CompositeStrokeExecutor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Brush/Executors/Editor/BrushExecutorRouter.cs | BrushExecutorRouter | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs | PropSorterRuntime | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs | PropRuntimeSpawner | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs | PropColliderAutoBuilder | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/PropRegistrySO.cs | PropRegistrySO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/PropPlacementData.cs | PropPlacementData | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/PropFootprintValidator.cs | PropFootprintValidator | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs | PropDefinitionSO | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Editor/PropsTab.cs | PropsTab | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Editor/PropPlacer.cs | PropPlacer | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Editor/PropDefinitionPostprocessor.cs | PropDefinitionPostprocessor | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/MapDesigner/Props/Auto/BridsonPoissonAutoPlacer.cs | BridsonPoissonAutoPlacer | LIVE | Projection-agnostic data/editor/runtime helper; no iso-specific markers in header/signature scan. | - |
| Assets/Scripts/Core/LegacyRuntimeRoomManager.cs | LegacyRuntimeRoomManager | LIVE -> rename | Runtime room lifecycle is tilemap/top-down compatible; Legacy label is outdated post-pivot. IsoGrid path fallbacks are non-blocking compatibility only. | - |
| Assets/Scripts/Utilities/IsoSortingOrder.cs | IsoSortingOrder | ARCHIVE | Iso-specific sprite sorting component; already disabled with #if false for top-down pivot. | - |

### Archive batch list
- Assets/Scripts/Utilities/IsoSortingOrder.cs

### Adapt batch list
- Assets/Scripts/Map/RoomBuilder.cs (320-328 replace IsoGrid name/isometric GridLayout/cell size with top-down Room grid defaults; 345 and 356 remove IsoGrid child assumptions)


