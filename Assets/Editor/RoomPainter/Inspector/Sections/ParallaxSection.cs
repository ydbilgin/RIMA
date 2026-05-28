using RIMA.Background;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class ParallaxSection
    {
        private const int CustomPresetIndex = 0;
        private const float MinFactor = 0.01f;
        private const float MaxFactor = 1.50f;

        // Preview scrub range in world units — enough to feel the depth difference.
        private const float PreviewRange = 12f;

        private static float _previewX;
        private static float _previewY;

        private static readonly string[] TierPresetNames =
        {
            "Custom",
            "FG 1.20",
            "Playable 1.00",
            "Near 0.65",
            "Mid 0.40",
            "Far 0.22",
            "Skyline 0.10",
            "Ambient 0.08",   // L3 cliff-base ambiance: factor (0.08, 0.04), sortingOrder -400, pixelSnap PPU 64
            "Horizon 0.03"
        };

        private static readonly float[] TierPresetValues =
        {
            1.20f,
            1.00f,
            0.65f,
            0.40f,
            0.22f,
            0.10f,
            0.08f,   // L3 ambient — set Y=0.04, sortingOrder=-400 on the ParallaxLayer component
            0.03f
        };

        public static void Draw(RoomPainterAsset asset)
        {
            bool enabled = asset.defaultLayer == RoomLayer.Parallax;
            using (new EditorGUI.DisabledScope(!enabled))
            {
                int currentPresetIndex = ResolvePopupIndex(asset.parallaxTier);

                EditorGUI.BeginChangeCheck();
                int nextPresetIndex = EditorGUILayout.Popup("Tier", currentPresetIndex, TierPresetNames);
                if (EditorGUI.EndChangeCheck())
                {
                    if (nextPresetIndex == CustomPresetIndex)
                    {
                        asset.parallaxTier = -1;
                        if (asset.parallaxFactor <= 0f)
                        {
                            asset.parallaxFactor = 1f;
                        }

                        asset.parallaxFactor = Mathf.Clamp(asset.parallaxFactor, MinFactor, MaxFactor);
                    }
                    else
                    {
                        int tierIndex = nextPresetIndex - 1;
                        asset.parallaxTier = tierIndex;
                        asset.parallaxFactor = TierPresetValues[tierIndex];
                    }
                }

                DrawStickPresets(asset);
                DrawDepthSlider(asset);

                asset.cameraRelative = (RoomPainterBoolOverride)EditorGUILayout.EnumPopup("Camera Relative", asset.cameraRelative);
                asset.pixelSnap = (RoomPainterBoolOverride)EditorGUILayout.EnumPopup("Pixel Snap", asset.pixelSnap);
            }

            GUILayout.Space(6f);
            DrawPreviewScrub();
        }

        /// <summary>
        /// Draws the Preview Pan scrub UI and pushes the offset into
        /// <see cref="ParallaxLayer.EditorPreviewOffset"/> so every
        /// <c>[ExecuteAlways]</c> ParallaxLayer in the scene responds in Edit mode.
        /// Can be called standalone (e.g. from the Parallax mode section) without
        /// requiring a selected asset.
        /// </summary>
        public static void DrawPreviewScrub()
        {
            // Header
            Color prevBg = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.22f, 0.14f, 0.38f, 1f);
            EditorGUILayout.LabelField("Preview Pan (Edit Mode)", EditorStyles.boldLabel);
            GUI.backgroundColor = prevBg;

            EditorGUILayout.HelpBox("Drag sliders to scrub camera position. ParallaxLayer depth effect visible without Play mode.", MessageType.None);

            // X scrub
            EditorGUI.BeginChangeCheck();
            float nextX = EditorGUILayout.Slider("Pan X", _previewX, -PreviewRange, PreviewRange);
            float nextY = EditorGUILayout.Slider("Pan Y", _previewY, -PreviewRange, PreviewRange);
            bool changed = EditorGUI.EndChangeCheck();

            if (changed)
            {
                _previewX = nextX;
                _previewY = nextY;
                ParallaxLayer.EditorPreviewOffset = new Vector2(_previewX, _previewY);
                // Recapture origins from current transform positions so the preview
                // is relative to the correct base. Required because EditorPreviewOffset
                // is applied against _layerStart set at OnEnable.
                RecaptureAllLayerOrigins();
                SceneView.RepaintAll();
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                // Visual offset indicator bar (horizontal)
                float normalizedX = Mathf.InverseLerp(-PreviewRange, PreviewRange, _previewX);
                Rect barRect = GUILayoutUtility.GetRect(1f, 4f, GUILayout.ExpandWidth(true));
                EditorGUI.DrawRect(barRect, new Color(0.12f, 0.12f, 0.14f, 1f));
                // Draw the cursor dot
                float cursorX = barRect.x + barRect.width * normalizedX;
                EditorGUI.DrawRect(new Rect(cursorX - 2f, barRect.y, 5f, barRect.height), new Color(0.55f, 0.40f, 0.78f, 1f));
            }

            GUILayout.Space(2f);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Offset: " + _previewX.ToString("0.0") + ", " + _previewY.ToString("0.0"), EditorStyles.miniLabel);
                GUILayout.FlexibleSpace();

                bool isActive = _previewX != 0f || _previewY != 0f;
                Color prevContent = GUI.contentColor;
                if (isActive)
                {
                    GUI.contentColor = new Color(0.0f, 0.95f, 0.78f, 1f);
                }

                GUILayout.Label(isActive ? "PREVIEW ACTIVE" : "centered", EditorStyles.miniBoldLabel, GUILayout.Width(100f));
                GUI.contentColor = prevContent;

                if (GUILayout.Button("Reset", EditorStyles.miniButton, GUILayout.Width(48f)))
                {
                    _previewX = 0f;
                    _previewY = 0f;
                    ParallaxLayer.EditorPreviewOffset = Vector2.zero;
                    RecaptureAllLayerOrigins();
                    SceneView.RepaintAll();
                }
            }
        }

        /// <summary>
        /// Recaptures origins on all <see cref="ParallaxLayer"/> components in loaded scenes
        /// so the preview scrub is always relative to the current rest position.
        /// Only runs in Edit mode.
        /// </summary>
        private static void RecaptureAllLayerOrigins()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ParallaxLayer[] layers = Object.FindObjectsByType<ParallaxLayer>(FindObjectsSortMode.None);
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].RecaptureOrigin();
            }
        }

        /// <summary>
        /// Sang-Hendrix-L4 "stick-to-map vs stick-to-camera" convenience. Both buttons write the
        /// EXISTING <see cref="RoomPainterAsset.parallaxFactor"/> field (baked onto
        /// <see cref="ParallaxLayer.factor"/> by the scene placer) — no parallel data model.
        /// Runtime math is pos = layerStart + cameraDelta * factor, so factor 0 keeps the layer
        /// pinned in world (Stick to Map) and factor 1.0 glues it to the camera (Stick to Camera).
        /// </summary>
        private static void DrawStickPresets(RoomPainterAsset asset)
        {
            // Both predicates read the RAW asset.parallaxFactor (not the resolved value) so the
            // two states stay consistent. In the post-"Stick to Map" state (parallaxFactor == 0)
            // this keeps isStickToCamera false, so the user can always escape the map lock.
            bool isStickToMap = asset.parallaxFactor <= 0f;
            bool isStickToCamera = Mathf.Approximately(asset.parallaxFactor, ParallaxLayer.StickToCameraFactor);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Anchor", EditorStyles.miniLabel, GUILayout.Width(48f));

                using (new EditorGUI.DisabledScope(isStickToMap))
                {
                    if (GUILayout.Button(new GUIContent("Stick to Map", "Pin to world — no parallax drift (factor 0)."), EditorStyles.miniButtonLeft))
                    {
                        // Stick to Map is allowed to clamp fully to 0 (special world-lock value).
                        asset.parallaxFactor = ParallaxLayer.StickToMapFactor;
                        asset.parallaxTier = -1;
                    }
                }

                using (new EditorGUI.DisabledScope(isStickToCamera))
                {
                    if (GUILayout.Button(new GUIContent("Stick to Camera", "Glue to camera — HUD-like (factor 1.0)."), EditorStyles.miniButtonRight))
                    {
                        asset.parallaxFactor = Mathf.Clamp(
                            ParallaxLayer.StickToCameraFactor,
                            ParallaxLayer.CanonicalMinFactor,
                            ParallaxLayer.CanonicalMaxFactor);
                        asset.parallaxTier = -1;
                    }
                }
            }
        }

        private static void DrawDepthSlider(RoomPainterAsset asset)
        {
            float currentFactor = ResolveCurrentFactor(asset);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Far", EditorStyles.miniLabel, GUILayout.Width(26f));
                EditorGUI.BeginChangeCheck();
                float nextFactor = EditorGUILayout.Slider(currentFactor, MinFactor, MaxFactor);
                if (EditorGUI.EndChangeCheck())
                {
                    asset.parallaxFactor = nextFactor;
                    asset.parallaxTier = -1;
                }

                GUILayout.Label("Near", EditorStyles.miniLabel, GUILayout.Width(32f));
            }

            Rect barRect = GUILayoutUtility.GetRect(1f, 4f, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(barRect, new Color(0.12f, 0.12f, 0.14f, 1f));
            float normalized = Mathf.InverseLerp(MinFactor, MaxFactor, currentFactor);
            EditorGUI.DrawRect(new Rect(barRect.x, barRect.y, barRect.width * normalized, barRect.height), new Color(0.55f, 0.40f, 0.78f, 1f));

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Depth Factor", EditorStyles.miniBoldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.Label(currentFactor.ToString("0.00"), EditorStyles.miniBoldLabel);
            }
        }

        private static int ResolvePopupIndex(int tier)
        {
            if (tier >= 0 && tier < TierPresetValues.Length)
            {
                return tier + 1;
            }

            return CustomPresetIndex;
        }

        private static float ResolveCurrentFactor(RoomPainterAsset asset)
        {
            if (asset.parallaxFactor > 0f)
            {
                return Mathf.Clamp(asset.parallaxFactor, MinFactor, MaxFactor);
            }

            if (asset.parallaxTier >= 0 && asset.parallaxTier < TierPresetValues.Length)
            {
                return TierPresetValues[asset.parallaxTier];
            }

            return 1f;
        }
    }
}
