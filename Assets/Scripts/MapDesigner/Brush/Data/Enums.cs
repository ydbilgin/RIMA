namespace RIMA.MapDesigner.Brush.Data
{
    public enum BrushCategory { Floor, Variation, Wall, Transition, Detail, RiftAccent, Composite, Cliff }

    public enum PaintMode
    {
        GridTile,
        GridTileRandom,
        FreeformDecal,
        ScatterAlongStroke,
        Stamp,
        CompositeStroke,
        EraseByLayer,
        EraseAllDecorative
    }

    public enum TargetLayer { L1, L2, L3, L4, L5, L6 }

    public enum SnapMode { None, FullGrid32, HalfGrid16, QuarterGrid8 }

    public enum AlphaMode { Hard, SoftAlpha8, SoftAlpha16, MultiplyBlend }

    public enum AssetCategory
    {
        Floor,
        FloorVariation,
        Wall,
        WallCorner,
        Doorway,
        MossPatch,
        DirtPatch,
        BiomeBlend,
        Crack,
        Rubble,
        Pebble,
        SmallMoss,
        DirtChip,
        RiftCrack,
        RiftCorruption,
        MagicalMark,
        // D2: 6-layer architecture additions (2026-05-27)
        CliffFaceDecor,   // L3 — mounting_*.prefab, cliff-hanging decorations
        WallBlocker,      // L5 — statue, pillar, broken column, small wall
        GameplayObject    // L6 — chest, fragment, gate, trigger volumes
    }

    public enum SizeBucket { Micro, Small, Medium, Large, Hero }

    public enum ValidationIssueSeverity { Error, Warning, Info }
}
