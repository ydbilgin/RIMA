namespace RIMA.RoomDesigner.Core
{
    public enum RoomLayer
    {
        Base = 0,
        Decal = 1,
        Wall = 2,
        Prop = 3,

        Floor = Base,
        Decals = Decal,
        Walls = Wall
    }

    public enum BrushMode
    {
        Stamp,
        Eraser,
        Picker,
        Bucket,
        Circle,
        Soft
    }
}
