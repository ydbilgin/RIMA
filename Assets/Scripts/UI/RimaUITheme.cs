using UnityEngine;

namespace RIMA
{
    public static class RimaUITheme
    {
        public const string ResourceFramePath = "UI/RIMA/RIMA_UI_ResourceFrame";
        public const string SkillSlotFramePath = "UI/RIMA/RIMA_UI_SkillSlotFrame";
        public const string MiniMapFramePath = "UI/RIMA/RIMA_UI_MiniMapFrame";
        public const string PromptFramePath = "UI/RIMA/RIMA_UI_PromptFrame";
        public const string RoomBannerFramePath = "UI/RIMA/RIMA_UI_RoomBannerFrame";
        public const string SmallPanelFramePath = "UI/RIMA/RIMA_UI_SmallPanelFrame";
        public const string MenuDungeonBackgroundPath = "UI/RIMA/RIMA_MenuDungeonBackground";

        public static readonly Color PanelTint = new Color(1f, 1f, 1f, 0.93f);
        public static readonly Color TextPrimary = new Color(0.86f, 0.92f, 0.94f, 1f);
        public static readonly Color TextMuted = new Color(0.54f, 0.66f, 0.70f, 1f);
        public static readonly Color Cyan = new Color(0.28f, 0.88f, 1f, 1f);
        public static readonly Color Gold = new Color(0.95f, 0.74f, 0.24f, 1f);

        public static Sprite ResourceFrame => RimaGeneratedSpriteCache.Load(ResourceFramePath);
        public static Sprite SkillSlotFrame => RimaGeneratedSpriteCache.Load(SkillSlotFramePath);
        public static Sprite MiniMapFrame => RimaGeneratedSpriteCache.Load(MiniMapFramePath);
        public static Sprite PromptFrame => RimaGeneratedSpriteCache.Load(PromptFramePath);
        public static Sprite RoomBannerFrame => RimaGeneratedSpriteCache.Load(RoomBannerFramePath);
        public static Sprite SmallPanelFrame => RimaGeneratedSpriteCache.Load(SmallPanelFramePath);
        public static Sprite MenuDungeonBackground => RimaGeneratedSpriteCache.Load(MenuDungeonBackgroundPath);

        public static Sprite NodeIcon(RoomType type)
        {
            string path = type switch
            {
                RoomType.Elite => "UI/RIMA/RIMA_UI_Node_Elite",
                RoomType.Boss => "UI/RIMA/RIMA_UI_Node_Boss",
                RoomType.Chest => "UI/RIMA/RIMA_UI_Node_Chest",
                RoomType.Merchant => "UI/RIMA/RIMA_UI_Node_Event",
                RoomType.Event => "UI/RIMA/RIMA_UI_Node_Event",
                RoomType.Forge => "UI/RIMA/RIMA_UI_Node_Forge",
                RoomType.Curse => "UI/RIMA/RIMA_UI_Node_Curse",
                _ => "UI/RIMA/RIMA_UI_Node_Combat",
            };
            return RimaGeneratedSpriteCache.Load(path);
        }
    }
}
