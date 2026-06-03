namespace RIMA.RoomPainter
{
    /// <summary>
    /// Single source of truth mapping a user-facing <see cref="DesignerCategory"/> to the
    /// underlying <see cref="RoomLayer"/> it writes into, plus display metadata for the tab UI.
    /// Both the Editor window and the F2 overlay route through this so a category always lands
    /// in the same RoomData collection / sorting layer on either surface.
    /// </summary>
    public static class DesignerCategoryMap
    {
        /// <summary>The RoomLayer a category's placements belong to.</summary>
        public static RoomLayer LayerFor(DesignerCategory category)
        {
            switch (category)
            {
                case DesignerCategory.Floor:  return RoomLayer.Floor;
                case DesignerCategory.Cliff:  return RoomLayer.Cliff;
                case DesignerCategory.Object: return RoomLayer.Props;
                case DesignerCategory.Portal: return RoomLayer.Props;
                case DesignerCategory.Light:  return RoomLayer.Lighting;
                default:                      return RoomLayer.Props;
            }
        }

        /// <summary>
        /// True when the category paints into a Tilemap-backed cell list (floorCells/cliffCells);
        /// false when it spawns prop/portal/light GameObjects (propPlacements / portalPlacements).
        /// </summary>
        public static bool IsTileCategory(DesignerCategory category)
        {
            return category == DesignerCategory.Floor || category == DesignerCategory.Cliff;
        }

        /// <summary>True when the category stores into the dedicated portalPlacements list.</summary>
        public static bool IsPortalCategory(DesignerCategory category)
        {
            return category == DesignerCategory.Portal;
        }

        /// <summary>Short tab label.</summary>
        public static string Label(DesignerCategory category)
        {
            switch (category)
            {
                case DesignerCategory.Floor:  return "Floor";
                case DesignerCategory.Cliff:  return "Cliff";
                case DesignerCategory.Object: return "Object";
                case DesignerCategory.Portal: return "Portal";
                case DesignerCategory.Light:  return "Light";
                default:                      return category.ToString();
            }
        }

        /// <summary>Registry tag used to filter the palette to this category's assets.</summary>
        public static string RegistryTag(DesignerCategory category)
        {
            switch (category)
            {
                case DesignerCategory.Floor:  return "floor";
                case DesignerCategory.Cliff:  return "cliff";
                case DesignerCategory.Object: return "prop";
                case DesignerCategory.Portal: return "portal";
                case DesignerCategory.Light:  return "light";
                default:                      return string.Empty;
            }
        }

        public static readonly DesignerCategory[] All =
        {
            DesignerCategory.Floor,
            DesignerCategory.Cliff,
            DesignerCategory.Object,
            DesignerCategory.Portal,
            DesignerCategory.Light
        };
    }
}
