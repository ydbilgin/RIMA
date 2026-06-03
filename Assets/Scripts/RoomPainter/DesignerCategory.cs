namespace RIMA.RoomPainter
{
    /// <summary>
    /// User-facing placement categories for the Unified Designer tool. These are the tabs
    /// the author picks ("I'm placing floor / cliff / objects / portals / lights"). Each
    /// category resolves to an underlying <see cref="RoomLayer"/> + RoomData collection via
    /// <see cref="DesignerCategoryMap"/>, so both surfaces (Editor window + in-game F2 overlay)
    /// share one routing table and cannot drift.
    /// </summary>
    public enum DesignerCategory
    {
        Floor = 0,
        Cliff = 1,
        Object = 2,
        Portal = 3,
        Light = 4
    }
}
