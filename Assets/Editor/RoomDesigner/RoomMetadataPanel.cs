using RIMA.Runtime.Rooms;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public sealed class RoomMetadataPanel : VisualElement
    {
        internal const string PanelName = "room-metadata-panel";
        private readonly IRoomDesignerContext ctx;
        private string roomId = "room_001";
        private BiomeType biomeType = BiomeType.Keep;
        private int gateCount = 2;
        private int noiseSeed = 0;
        private bool previewBakeEnabled = false;
        private TileBase[] floorVariantSet;
        private ObjectField[] variantSlots;
        private IntegerField _seedField;

        public RoomMetadataPanel(IRoomDesignerContext ctx)
        {
            this.ctx = ctx;
            name = PanelName;
            Build();
        }

        private void Build()
        {
            var title = new Label("Room Metadata");
            title.AddToClassList("rd-meta-title");
            Add(title);

            var row2 = new VisualElement();
            row2.AddToClassList("rd-meta-row");
            var idField = new TextField { label = "", value = roomId };
            idField.AddToClassList("rd-meta-field");
            idField.RegisterValueChangedCallback(e => roomId = e.newValue);
            var biomeChoices = new System.Collections.Generic.List<string> { "Keep", "Crypt", "Volcanic" };
            var biomeDropdown = new DropdownField(biomeChoices, biomeChoices.IndexOf(biomeType.ToString()));
            biomeDropdown.style.width = 80;
            biomeDropdown.RegisterValueChangedCallback(e =>
            {
                if (System.Enum.TryParse<BiomeType>(e.newValue, out var parsed))
                    biomeType = parsed;
            });
            row2.Add(idField);
            row2.Add(biomeDropdown);
            Add(row2);

            var row3 = new VisualElement();
            row3.AddToClassList("rd-meta-row");
            var gatesField = new IntegerField("Gates") { value = gateCount };
            gatesField.style.width = 80;
            gatesField.RegisterValueChangedCallback(e => gateCount = e.newValue);
            _seedField = new IntegerField("Seed") { value = noiseSeed };
            _seedField.style.flexGrow = 1;
            _seedField.RegisterValueChangedCallback(e => noiseSeed = e.newValue);
            var reseedBtn = new Button(Reseed) { text = "Reseed" };
            reseedBtn.style.width = 60;
            row3.Add(gatesField);
            row3.Add(_seedField);
            row3.Add(reseedBtn);
            Add(row3);

            var previewToggle = new Toggle("Preview Floor") { value = previewBakeEnabled };
            previewToggle.RegisterValueChangedCallback(e => OnPreviewBakeChanged(e.newValue));
            Add(previewToggle);

            var overrideToggle = new Toggle("Wall Override Mode");
            overrideToggle.RegisterValueChangedCallback(e =>
            {
                if (ctx is RIMA.Editor.RoomDesigner.IRoomDesignerContext c)
                    c.IsWallOverrideMode = e.newValue;
            });
            Add(overrideToggle);

            var sep = new VisualElement();
            sep.AddToClassList("rd-meta-sep");
            Add(sep);

            Add(new Label("Assign floor variant tiles below"));
        }

        private void Reseed()
        {
            noiseSeed = UnityEngine.Random.Range(0, 99999);
            if (_seedField != null) _seedField.SetValueWithoutNotify(noiseSeed);
            if (previewBakeEnabled && ctx?.FloorTilemap != null && floorVariantSet != null)
                RunPreview();
        }

        private void OnPreviewBakeChanged(bool enabled)
        {
            previewBakeEnabled = enabled;
            if (ctx?.FloorTilemap == null) return;
            if (enabled && floorVariantSet != null)
                RunPreview();
            else if (!enabled && floorVariantSet != null && floorVariantSet.Length > 0)
                FloorVariantPainter.RestoreDefault(ctx.FloorTilemap, floorVariantSet[0]);
        }

        private void RunPreview()
        {
            var bp = UnityEngine.ScriptableObject.CreateInstance<RIMA.Runtime.Rooms.RoomBlueprint>();
            bp.noiseSeed = noiseSeed;
            bp.roomWidth = 20;
            bp.roomHeight = 20;
            try { FloorVariantPainter.PreviewVariants(ctx.FloorTilemap, bp, floorVariantSet); }
            finally { UnityEngine.Object.DestroyImmediate(bp); }
        }

        public (string roomId, BiomeType biome, int noiseSeed, int gateCount) GetBlueprintData() =>
            (roomId, biomeType, noiseSeed, gateCount);

        internal static bool IsMounted(VisualElement rightPanel) =>
            rightPanel?.Q<RoomMetadataPanel>(PanelName) != null;
    }

    [InitializeOnLoad]
    internal static class RoomMetadataPanelBootstrap
    {
        static RoomMetadataPanelBootstrap()
        {
            EditorApplication.update += TryMount;
        }

        private static double _lastCheck = 0;

        private static void TryMount()
        {
            if (EditorApplication.timeSinceStartup - _lastCheck < 0.25) return;
            _lastCheck = EditorApplication.timeSinceStartup;

            var windows = UnityEngine.Resources.FindObjectsOfTypeAll<RimaRoomDesignerWindow>();
            for (int i = 0; i < windows.Length; i++)
            {
                var win = windows[i];
                var rightPanel = win.RightPanel;
                if (rightPanel == null || RoomMetadataPanel.IsMounted(rightPanel)) continue;
                rightPanel.Add(new RoomMetadataPanel(win));
                win.MarkDirty();
            }
        }
    }
}
