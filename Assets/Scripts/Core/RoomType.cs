namespace RIMA
{
    /// <summary>
    /// Oda tipleri — GateBehavior hangi sprite'ı göstereceğini buna göre seçer.
    /// Faz B gate spriteleri gelince tüm tipler aktif olacak.
    /// </summary>
    public enum RoomType
    {
        Combat,
        Elite,
        Boss,
        Chest,
        Merchant,
        Forge,
        Event,
        Curse
    }
}
