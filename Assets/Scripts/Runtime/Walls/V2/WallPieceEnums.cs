namespace RIMA.Walls.V2
{
    public enum WallPieceType
    {
        RearWall,
        SideWall,
        Connector,
        OuterCorner,
        InnerCorner,
        DoorArch,
        LowFront,
        OpenGap,
        Seam
    }

    public enum WallDirection
    {
        Rear,
        SideLeft,
        SideRight,
        Front,
        Any
    }

    public enum WallHeight
    {
        Low,
        Normal,
        Tall
    }

    public enum RoomShapeType
    {
        Rectangle,
        Diamond,
        Irregular
    }

    public enum FrontEdgeMode
    {
        Open,
        LowWall,
        Broken
    }
}
