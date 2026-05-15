namespace RIMA.MapDesigner.Brush.Data
{
    public enum BrushCategory { Floor, Variation, Wall, Transition, Detail, RiftAccent, Composite }

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
        MagicalMark
    }
}
