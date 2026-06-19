namespace RIMA.RoomPainter
{
    /// <summary>
    /// Single source of truth for the floating-island DEPTH STACK (user-locked ordering).
    /// Resolves each <see cref="RoomLayer"/> to a concrete sorting layer NAME + sorting ORDER.
    /// Previously these values were scattered/hardcoded in 3+ places (CliffPlacementRules,
    /// CliffGenerateAction, scene files). This table replaces those magic numbers so the
    /// stack is shiftable from one place and identical on both authoring surfaces + runtime.
    ///
    /// User's conceptual stack (top = nearest camera, drawn last):
    ///   L1 Floor      — walkable iso tiles, just above the cliff lip
    ///   L2 Cliff      — directly UNDER the floor, near-depth drop
    ///   (preview)     — next-room preview islands, BELOW the cliff, in the void  (see PORTAL spec)
    ///   L3 Backdrop   — Deep background art, FAR below (parallax)
    ///
    /// All sorting-layer names here MUST exist in ProjectSettings/TagManager.asset
    /// (Default, Ground, Floor, Decals, Walls, Entities, VFX, Player, BackwallLandmark,
    ///  Characters, Props, Decor_Cliff, Decor_Floor).
    /// </summary>
    public static class RoomDepthStack
    {
        public struct DepthSlot
        {
            public string sortingLayer;
            public int sortingOrder;
            public bool ySort;
        }

        // ── named tiers (the user's L1/L2/L3 + helpers) ────────────────────
        public const string LayerFloor      = "Floor";    // L1
        public const string LayerCliff      = "Ground";   // L2 (under floor)
        public const string LayerWalls      = "Walls";
        public const string LayerEntities   = "Entities"; // props/objects/lights/portals (y-sorted)
        public const string LayerDecals     = "Decals";
        public const string LayerBackdrop   = "BackwallLandmark"; // L3 far depth art / preview islands

        public const int OrderFloor   = 0;
        public const int OrderCliff   = -10;   // just under floor (normalizes the old hardcoded -50)
        public const int OrderPreview = -50;   // preview islands: below cliff, above far backdrop
        public const int OrderBackdrop = -100; // far depth art

        /// <summary>Resolve the default depth slot for a logical room layer.</summary>
        public static DepthSlot SlotFor(RoomLayer layer)
        {
            switch (layer)
            {
                case RoomLayer.Floor:
                    return new DepthSlot { sortingLayer = LayerFloor, sortingOrder = OrderFloor, ySort = false };
                case RoomLayer.Edge:
                    return new DepthSlot { sortingLayer = LayerFloor, sortingOrder = OrderFloor + 1, ySort = false };
                case RoomLayer.Cliff:
                    return new DepthSlot { sortingLayer = LayerCliff, sortingOrder = OrderCliff, ySort = false };
                case RoomLayer.Wall:
                    return new DepthSlot { sortingLayer = LayerWalls, sortingOrder = 0, ySort = true };
                case RoomLayer.Props:
                    return new DepthSlot { sortingLayer = LayerEntities, sortingOrder = 0, ySort = true };
                case RoomLayer.Decals:
                    return new DepthSlot { sortingLayer = LayerDecals, sortingOrder = 2, ySort = false };
                case RoomLayer.Lighting:
                    return new DepthSlot { sortingLayer = LayerEntities, sortingOrder = 20, ySort = false };
                case RoomLayer.Parallax:
                    return new DepthSlot { sortingLayer = LayerBackdrop, sortingOrder = OrderBackdrop, ySort = false };
                case RoomLayer.Collision:
                case RoomLayer.Occlusion:
                default:
                    return new DepthSlot { sortingLayer = "Default", sortingOrder = 0, ySort = false };
            }
        }
    }
}
