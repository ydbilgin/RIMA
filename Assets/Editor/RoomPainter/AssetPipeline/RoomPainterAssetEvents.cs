using System;

namespace RIMA.RoomPainter.Editor
{
    public static class RoomPainterAssetEvents
    {
        public static event Action<RoomPainterAsset> AssetCreatedOrUpdated;
        public static event Action<string> AssetDeleted;

        public static void PublishCreatedOrUpdated(RoomPainterAsset asset)
        {
            AssetCreatedOrUpdated?.Invoke(asset);
        }

        public static void PublishDeleted(string guid)
        {
            AssetDeleted?.Invoke(guid);
        }
    }
}
