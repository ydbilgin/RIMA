#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.Systems.Map;
using RIMA.Editor;
using RIMA;

namespace RIMA.Editor.MapDesigner
{
    public class RimaWorldPainterWindow : EditorWindow
    {
        public enum PaletteCategory { Floor, Wall, Prop, Mob }
        public enum ToolMode { Paint, Erase, Eyedropper }
        public enum CollisionMode { Auto, Passable, SmallFootprint, FullFootprint, WallBlock, Custom }
        public enum PaintMode { TopDown, Isometric }
        public enum GridProjectionMode { Isometric, TopDown }

        [System.Serializable]
        public class ScanResult
        {
            public string assetPath;
            public string displayName;
            public GameObject prefab;
            public TileBase tile;
            public Sprite icon;
        }

        [SerializeField] private PaletteCategory currentCategory = PaletteCategory.Floor;
        [SerializeField] private PaintMode currentPaintMode = PaintMode.TopDown;
        [SerializeField] private GridProjectionMode projectionMode = GridProjectionMode.TopDown;
        [SerializeField] private ToolMode currentTool = ToolMode.Paint;
        [SerializeField] private string searchQuery = string.Empty;
        [SerializeField] private RimaBiomePreset activeBiome;
        [SerializeField] private Tilemap targetTilemap;
        [SerializeField] private Transform targetParent;
        [SerializeField] private int prefabRotation = 0; // 0=0, 1=90, 2=180, 3=270
        [SerializeField] private bool snapToGrid = true;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private float prefabScaleMultiplier = 1.0f;
        [SerializeField] private float floorScale = 1.0f;
        [SerializeField] private float wallScale = 0.5f;
        [SerializeField] private float propScale = 0.4f;
        [SerializeField] private float mobScale = 1.0f;
        [SerializeField] private bool useCategoryScale = true;
        [SerializeField] private CollisionMode customCollisionMode = CollisionMode.Auto;
        [SerializeField] private Vector2 customColliderSize = new Vector2(1f, 1f);
        [SerializeField] private Vector2 customColliderOffset = Vector2.zero;
        [SerializeField] private bool autoAlignBaseToGrid = true;
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

        // Selection state
        private TileBase selectedTile;
        private GameObject selectedPrefab;
        private string selectedAssetName = "None";
        private Sprite selectedAssetIcon;
        private string selectedPaletteAssetPath = string.Empty;

        // Scanned assets lists
        private List<ScanResult> floorTiles = new List<ScanResult>();
        private List<ScanResult> wallPrefabs = new List<ScanResult>();
        private List<ScanResult> propPrefabs = new List<ScanResult>();
        private List<ScanResult> mobPrefabs = new List<ScanResult>();
        [SerializeField] private Dictionary<PaletteCategory, List<string>> paletteCustomAdds = new Dictionary<PaletteCategory, List<string>>();
        [SerializeField] private Dictionary<PaletteCategory, List<string>> paletteExcludes = new Dictionary<PaletteCategory, List<string>>();
        [SerializeField] private List<string> wallScanFolders = new List<string>
        {
            "Assets/Prefabs/Props/ShatteredKeep_PixelLab",
            "Assets/Prefabs/Walls/pilot_a",
            "Assets/Prefabs/Walls"
        };
        [SerializeField] private List<string> wallScanNamePatterns = new List<string> { "wall" };

        // Preview rendering state
        private GameObject previewObject;
        private GameObject lastPreviewPrefab;
        private int lastPreviewRotation;
        private Vector3 lastMouseWorldPos;
        private Vector3Int lastCellPos;
        private bool isMouseInSceneView;
        private GameObject cachedHoveredObject;

        // Grouping & Randomization state
        [SerializeField] private bool useRandomVariants = false;
        [SerializeField] private bool autoConnectWalls = true;
        [SerializeField] private bool wallRuleTileMode = true;
        [SerializeField] private TileBase wallRuleTile;
        [SerializeField] private bool randomizeWallCracks = true;

        [System.Serializable]
        public class TerrainGroup
        {
            public string terrainName;
            public TileBase baseTile;
            public List<TileBase> allTiles = new List<TileBase>();
        }
        private List<TerrainGroup> terrainGroups = new List<TerrainGroup>();

        [System.Serializable]
        public class WallVariantGroup
        {
            public string groupName;
            public GameObject basePrefab;
            public List<GameObject> variants = new List<GameObject>();
        }
        [SerializeField] private List<WallVariantGroup> wallVariantGroups = new List<WallVariantGroup>();


        // UI state
        private Vector2 paletteScroll;
        private Vector2 optionsScroll;
        private Dictionary<int, Texture2D> previewCache = new Dictionary<int, Texture2D>();
        private readonly Dictionary<PreviewCacheKey, CollisionResolver.ResolvedCollider> collisionPreviewCache = new Dictionary<PreviewCacheKey, CollisionResolver.ResolvedCollider>();
        private const string DefaultCollisionRulesPath = "Assets/Editor/MapDesigner/Rules/DefaultCollisionRules.asset";
        private const string PaintModePrefsKey = "RimaPainter_PaintMode";
        private const string FloorScalePrefsKey = "Painter.FloorScale";
        private const string WallScalePrefsKey = "Painter.WallScale";
        private const string PropScalePrefsKey = "Painter.PropScale";
        private const string MobScalePrefsKey = "Painter.MobScale";
        private const string UseCategoryScalePrefsKey = "Painter.UseCategoryScale";
        private const string PaletteAddsPrefsPrefix = "RimaPainter_CustomAdds_";
        private const string PaletteExcludesPrefsPrefix = "RimaPainter_Excludes_";
        private const string WangRuleTileFolder = "Assets/Data/Tiles/Act1_ShatteredKeep/wang_rules";
        private const string WallRuleTileAssetPath = WangRuleTileFolder + "/dark_rubble_to_broken_wall.asset";
        [SerializeField] private CollisionRulesSO collisionRules;

        [SerializeField] private string activeMapName = string.Empty;
        [SerializeField] private string activeMapPath = string.Empty;
        [SerializeField] private bool showMapList = true;
        [SerializeField] private bool showTargetsSection = true;
        [SerializeField] private bool showToolSettingsSection = true;
        [SerializeField] private bool showPrefabSettingsSection = true;
        [SerializeField] private bool showMapManagementSection = true;
        [SerializeField] private bool showSceneOrgSection = true;
        [SerializeField] private bool showSelectedInstanceSection = true;
        [SerializeField] private bool showStatusBanner = true;
        [SerializeField] private bool showCollisionGizmo = true;
        private bool sceneOrgDirty = true;
        private bool selectionDirty = true;
        private UnityEditor.Editor activeColliderEditor;
        private List<string> savedMapPaths = new List<string>();

        [System.Serializable]
        public class UnifiedTileData
        {
            public Vector3Int position;
            public string tileAssetPath;
        }

        [System.Serializable]
        public class UnifiedObjectData
        {
            public string prefabAssetPath;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale = Vector3.one;
            public CollisionMode collisionMode = CollisionMode.Auto;
            public Vector2 colliderSize = Vector2.one;
            public Vector2 colliderOffset = Vector2.zero;
            public string name;
        }

        [System.Serializable]
        public class UnifiedMapSaveData
        {
            public string mapName;
            public List<UnifiedTileData> tiles = new List<UnifiedTileData>();
            public List<UnifiedObjectData> objects = new List<UnifiedObjectData>();
        }

        internal static class GroupClassifier
        {
            public static readonly string[] CanonicalGroups =
                { "Walls", "Statues", "WallMountings", "Patches", "Mobs", "FloorProps" };

            public static string Classify(string prefabName, PaletteCategory category)
            {
                string lowerName = string.IsNullOrEmpty(prefabName) ? string.Empty : prefabName.ToLowerInvariant();
                if (category == PaletteCategory.Wall || lowerName.Contains("wall")) return "Walls";
                if (lowerName.StartsWith("statue_") || lowerName.Contains("statue")) return "Statues";
                if (lowerName.StartsWith("mounting_") || lowerName.Contains("torch") || lowerName.Contains("banner")) return "WallMountings";
                if (lowerName.StartsWith("patch_") || lowerName.StartsWith("rug_") || lowerName.StartsWith("carpet_")) return "Patches";
                if (category == PaletteCategory.Mob || lowerName.StartsWith("mob_") || lowerName.StartsWith("enemy_")) return "Mobs";
                return "FloorProps";
            }
        }

        internal static class CollisionResolver
        {
            public struct ResolvedCollider
            {
                public CollisionMode effectiveMode;
                public Vector2 worldSize;
                public Vector2 worldOffset;
                public string layerName;
                public int sortingOrder;
                public string resolveReason;
            }

            public static ResolvedCollider Resolve(
                GameObject prefab,
                PaletteCategory category,
                CollisionMode chosenMode,
                Vector2 customSize,
                Vector2 customOffset,
                float scaleMult,
                int rotationSteps,
                CollisionRulesSO rules)
            {
                float scale = scaleMult != 0f ? Mathf.Abs(scaleMult) : 1f;
                CollisionMode effectiveMode = chosenMode;
                string reason = "user:override";
                Vector2 resolvedCustomSize = customSize;
                Vector2 resolvedCustomOffset = customOffset;
                string prefabName = prefab != null ? prefab.name : string.Empty;

                if (effectiveMode == CollisionMode.Auto)
                {
                    if (rules != null && rules.TryResolve(prefabName, out CollisionRulesSO.Rule rule))
                    {
                        effectiveMode = rule.mode;
                        resolvedCustomSize = rule.customSize;
                        resolvedCustomOffset = rule.customOffset;
                        reason = "rule:" + rule.pattern;
                    }
                    else
                    {
                        effectiveMode = ResolveDefaultMode(prefab, category);
                        reason = ResolveDefaultReason(prefab, category, effectiveMode);
                    }
                }

                Vector2 worldSize = Vector2.zero;
                Vector2 worldOffset = Vector2.zero;
                SpriteRenderer sr = prefab != null ? prefab.GetComponentInChildren<SpriteRenderer>(true) : null;
                float spriteWidth = sr != null && sr.sprite != null ? sr.sprite.bounds.size.x : 1f;
                float localVisibleBottom = sr != null && sr.sprite != null ? GetRotatedBoundsMin(sr.sprite.bounds, rotationSteps) : 0f;

                if (effectiveMode == CollisionMode.Custom)
                {
                    worldSize = resolvedCustomSize;
                    worldOffset = resolvedCustomOffset;
                }
                else if (effectiveMode == CollisionMode.Passable)
                {
                    worldSize = new Vector2(spriteWidth * scale * 0.8f, 0.1f);
                    worldOffset = new Vector2(0f, localVisibleBottom * scale + 0.05f);
                }
                else
                {
                    float desiredWorldWidth = 0.85f;
                    float desiredWorldHeight = 0.85f;

                    switch (effectiveMode)
                    {
                        case CollisionMode.WallBlock:
                            desiredWorldWidth = spriteWidth * scale;
                            desiredWorldHeight = 0.8f;
                            break;
                        case CollisionMode.FullFootprint:
                            desiredWorldWidth = spriteWidth * scale * 0.85f;
                            desiredWorldHeight = 0.6f;
                            break;
                        case CollisionMode.SmallFootprint:
                            desiredWorldWidth = 0.4f;
                            desiredWorldHeight = 0.3f;
                            break;
                    }

                    worldSize = new Vector2(desiredWorldWidth, desiredWorldHeight);
                    worldOffset = new Vector2(0f, localVisibleBottom * scale + desiredWorldHeight / 2f);
                }

                return new ResolvedCollider
                {
                    effectiveMode = effectiveMode,
                    worldSize = worldSize,
                    worldOffset = worldOffset,
                    layerName = ResolveLayerName(category, effectiveMode),
                    sortingOrder = ResolveSortingOrder(category, effectiveMode),
                    resolveReason = reason
                };
            }

            private static CollisionMode ResolveDefaultMode(GameObject prefab, PaletteCategory category)
            {
                if (prefab == null) return CollisionMode.Passable;
                string name = prefab.name.ToLowerInvariant();
                if (category == PaletteCategory.Wall || name.Contains("wall")) return CollisionMode.WallBlock;
                if (category == PaletteCategory.Mob || name.StartsWith("enemy_") || name.StartsWith("mob_")) return CollisionMode.Passable;

                if (category == PaletteCategory.Prop)
                {
                    if (name.Contains("bone") || name.Contains("skeleton") || name.Contains("skull") ||
                        name.Contains("rubble") || name.Contains("debris") || name.Contains("stone_small") ||
                        name.Contains("pebble") || name.Contains("carpet") || name.Contains("rug") ||
                        name.Contains("blood") || name.Contains("splat") || name.Contains("crack") ||
                        name.Contains("decal") || name.Contains("mounting_"))
                    {
                        return CollisionMode.Passable;
                    }

                    if (name.StartsWith("statue_") || name.Contains("statue") ||
                        name.Contains("column") || name.Contains("pillar") ||
                        name.Contains("chest") || name.Contains("box") ||
                        name.Contains("barrel") || name.Contains("crate") ||
                        name.Contains("tomb") || name.Contains("sarcophagus"))
                    {
                        return CollisionMode.FullFootprint;
                    }

                    if (name.Contains("lamp") || name.Contains("torch") || name.Contains("lantern") ||
                        name.Contains("candle") || name.Contains("light") || name.Contains("urn") ||
                        name.Contains("pot"))
                    {
                        return CollisionMode.SmallFootprint;
                    }

                    if (prefab.GetComponentInChildren<Collider2D>(true) != null)
                    {
                        return CollisionMode.FullFootprint;
                    }
                }

                return CollisionMode.Passable;
            }

            private static string ResolveDefaultReason(GameObject prefab, PaletteCategory category, CollisionMode mode)
            {
                string name = prefab != null ? prefab.name.ToLowerInvariant() : string.Empty;
                if (category == PaletteCategory.Wall || name.Contains("wall")) return "keyword:wall";
                if (name.Contains("mounting_")) return "keyword:mounting_";
                if (name.StartsWith("statue_") || name.Contains("statue")) return "keyword:statue";
                if (mode == CollisionMode.Passable) return "category:" + category;
                return "keyword:" + mode;
            }

            private static string ResolveLayerName(PaletteCategory category, CollisionMode mode)
            {
                if (category == PaletteCategory.Wall || mode == CollisionMode.WallBlock) return "Walls";
                if (category == PaletteCategory.Prop || category == PaletteCategory.Mob) return "Entities";
                return string.Empty;
            }

            private static int ResolveSortingOrder(PaletteCategory category, CollisionMode mode)
            {
                if (category == PaletteCategory.Wall || mode == CollisionMode.WallBlock) return 20;
                if (category == PaletteCategory.Mob) return 40;
                if (category == PaletteCategory.Prop) return mode == CollisionMode.Passable ? 8 : 30;
                return 0;
            }

            private static float GetRotatedBoundsMin(Bounds bounds, int rotationSteps)
            {
                int r = (rotationSteps % 4 + 4) % 4;
                if (r == 0) return bounds.min.y;
                if (r == 1) return bounds.min.x;
                if (r == 2) return -bounds.max.y;
                return -bounds.max.x;
            }
        }

        private readonly struct PreviewCacheKey : IEquatable<PreviewCacheKey>
        {
            private readonly int prefabId;
            private readonly PaletteCategory category;
            private readonly CollisionMode chosenMode;
            private readonly float scaleMult;
            private readonly int rotationSteps;
            private readonly Vector2 customSize;
            private readonly Vector2 customOffset;
            private readonly int rulesSOVersion;

            public PreviewCacheKey(GameObject prefab, PaletteCategory category, CollisionMode chosenMode, float scaleMult, int rotationSteps, Vector2 customSize, Vector2 customOffset, CollisionRulesSO rules)
            {
                this.prefabId = prefab != null ? prefab.GetInstanceID() : 0;
                this.category = category;
                this.chosenMode = chosenMode;
                this.scaleMult = scaleMult;
                this.rotationSteps = rotationSteps;
                this.customSize = customSize;
                this.customOffset = customOffset;
                this.rulesSOVersion = rules != null ? EditorUtility.GetDirtyCount(rules) : 0;
            }

            public bool Equals(PreviewCacheKey other)
            {
                return prefabId == other.prefabId &&
                       category == other.category &&
                       chosenMode == other.chosenMode &&
                       Mathf.Approximately(scaleMult, other.scaleMult) &&
                       rotationSteps == other.rotationSteps &&
                       customSize == other.customSize &&
                       customOffset == other.customOffset &&
                       rulesSOVersion == other.rulesSOVersion;
            }

            public override bool Equals(object obj)
            {
                return obj is PreviewCacheKey other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(prefabId, category, chosenMode, scaleMult, rotationSteps, customSize, customOffset, rulesSOVersion);
            }
        }


        [MenuItem("RIMA/Tools/World Painter", false, 10)]
        public static void Open()
        {
            var window = GetWindow<RimaWorldPainterWindow>("World Painter");
            window.minSize = new Vector2(850f, 500f);
            window.Show();
        }

        private void OnEnable()
        {
            // Register callbacks
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;

            // Auto scan assets
            LoadPainterPrefs();
            LoadCollisionRulesAsset();
            ScanAllAssets();
            LoadDefaultWallRuleTile();
            TryAutoAssignTargets();
            RefreshSavedMapsList();
        }


        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorApplication.update -= OnEditorUpdate;
            Selection.selectionChanged -= OnSelectionChanged;
            SavePainterPrefs();
            if (activeColliderEditor != null)
            {
                DestroyImmediate(activeColliderEditor);
                activeColliderEditor = null;
            }
            ClearPreviewObject();
        }

        private void OnDestroy()
        {
            ClearPreviewObject();
        }

        private double lastRepaintTime;

        private void OnEditorUpdate()
        {
            if (selectionDirty)
            {
                selectionDirty = false;
                Repaint();
            }

            if (showSelectedInstanceSection && Selection.activeGameObject != null)
            {
                var box = Selection.activeGameObject.GetComponentInChildren<BoxCollider2D>(true);
                if (box != null)
                {
                    Repaint();
                }
            }

            // Rate limit repaints during preview loading to avoid overloading GUI pipeline
            if (AssetPreview.IsLoadingAssetPreviews())
            {
                if (EditorApplication.timeSinceStartup - lastRepaintTime > 0.25)
                {
                    lastRepaintTime = EditorApplication.timeSinceStartup;
                    Repaint();
                }
            }
        }

        private void OnHierarchyChange()
        {
            sceneOrgDirty = true;
            Repaint();
        }

        private void OnSelectionChange()
        {
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            selectionDirty = true;
            Repaint();
        }

        private void LoadCollisionRulesAsset()
        {
            if (collisionRules == null)
            {
                collisionRules = AssetDatabase.LoadAssetAtPath<CollisionRulesSO>(DefaultCollisionRulesPath);
            }
        }

        private void LoadPainterPrefs()
        {
            int savedMode = PlayerPrefs.GetInt(PaintModePrefsKey, (int)PaintMode.Isometric);
            currentPaintMode = Enum.IsDefined(typeof(PaintMode), savedMode) ? (PaintMode)savedMode : PaintMode.Isometric;
            floorScale = PlayerPrefs.GetFloat(FloorScalePrefsKey, 1.0f);
            wallScale = PlayerPrefs.GetFloat(WallScalePrefsKey, 0.5f);
            propScale = PlayerPrefs.GetFloat(PropScalePrefsKey, 0.4f);
            mobScale = PlayerPrefs.GetFloat(MobScalePrefsKey, 1.0f);
            useCategoryScale = PlayerPrefs.GetInt(UseCategoryScalePrefsKey, 1) != 0;
            SyncLegacyProjectionMode();
            EnsurePalettePrefsLoaded();
        }

        private void SavePainterPrefs()
        {
            PlayerPrefs.SetInt(PaintModePrefsKey, (int)currentPaintMode);
            PlayerPrefs.SetFloat(FloorScalePrefsKey, floorScale);
            PlayerPrefs.SetFloat(WallScalePrefsKey, wallScale);
            PlayerPrefs.SetFloat(PropScalePrefsKey, propScale);
            PlayerPrefs.SetFloat(MobScalePrefsKey, mobScale);
            PlayerPrefs.SetInt(UseCategoryScalePrefsKey, useCategoryScale ? 1 : 0);
            SavePalettePrefs();
        }

        private void SetPaintMode(PaintMode mode)
        {
            if (currentPaintMode == mode) return;
            currentPaintMode = mode;
            SyncLegacyProjectionMode();
            PlayerPrefs.SetInt(PaintModePrefsKey, (int)currentPaintMode);
            PlayerPrefs.Save();
            ClearPreviewObject();
            SceneView.RepaintAll();
        }

        private void SyncLegacyProjectionMode()
        {
            projectionMode = currentPaintMode == PaintMode.Isometric ? GridProjectionMode.Isometric : GridProjectionMode.TopDown;
        }

        private void EnsurePalettePrefsLoaded()
        {
            if (paletteCustomAdds == null) paletteCustomAdds = new Dictionary<PaletteCategory, List<string>>();
            if (paletteExcludes == null) paletteExcludes = new Dictionary<PaletteCategory, List<string>>();
            EnsurePaletteDictionary(paletteCustomAdds);
            EnsurePaletteDictionary(paletteExcludes);

            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory)))
            {
                LoadPaletteList(paletteCustomAdds, PaletteAddsPrefsPrefix, category);
                LoadPaletteList(paletteExcludes, PaletteExcludesPrefsPrefix, category);
            }
        }

        private void SavePalettePrefs()
        {
            if (paletteCustomAdds == null) paletteCustomAdds = new Dictionary<PaletteCategory, List<string>>();
            if (paletteExcludes == null) paletteExcludes = new Dictionary<PaletteCategory, List<string>>();
            EnsurePaletteDictionary(paletteCustomAdds);
            EnsurePaletteDictionary(paletteExcludes);

            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory)))
            {
                SavePaletteList(paletteCustomAdds, PaletteAddsPrefsPrefix, category);
                SavePaletteList(paletteExcludes, PaletteExcludesPrefsPrefix, category);
            }
            PlayerPrefs.Save();
        }

        private void EnsurePaletteDictionary(Dictionary<PaletteCategory, List<string>> dictionary)
        {
            if (dictionary == null) return;
            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory)))
            {
                if (!dictionary.ContainsKey(category) || dictionary[category] == null)
                {
                    dictionary[category] = new List<string>();
                }
            }
        }

        private void LoadPaletteList(Dictionary<PaletteCategory, List<string>> dictionary, string prefix, PaletteCategory category)
        {
            if (dictionary == null) return;
            string raw = PlayerPrefs.GetString(prefix + category, string.Empty);
            dictionary[category] = string.IsNullOrEmpty(raw)
                ? new List<string>()
                : raw.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(NormalizeAssetPath)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
        }

        private void SavePaletteList(Dictionary<PaletteCategory, List<string>> dictionary, string prefix, PaletteCategory category)
        {
            if (dictionary == null || !dictionary.TryGetValue(category, out List<string> paths) || paths == null)
            {
                PlayerPrefs.DeleteKey(prefix + category);
                return;
            }

            string raw = string.Join("|", paths.Select(NormalizeAssetPath).Where(x => !string.IsNullOrEmpty(x)).Distinct(StringComparer.OrdinalIgnoreCase));
            PlayerPrefs.SetString(prefix + category, raw);
        }

        private void ScanAllAssets()
        {
            // 1. Scan Biome for Floors
            floorTiles.Clear();
            terrainGroups.Clear();
            if (activeBiome == null)
            {
                // Try load Shattered Keep biome preset as default
                activeBiome = AssetDatabase.LoadAssetAtPath<RimaBiomePreset>("Assets/Art/Tiles/F1/Shattered_Keep_F1_BiomePreset.asset");
            }

            if (activeBiome != null && activeBiome.terrains != null)
            {
                foreach (var terrain in activeBiome.terrains)
                {
                    if (terrain == null) continue;
                    
                    var tg = new TerrainGroup { terrainName = terrain.name, baseTile = terrain.baseTile };
                    
                    if (terrain.baseTile != null)
                    {
                        tg.allTiles.Add(terrain.baseTile);
                        AddTileToScanList(terrain.baseTile, terrain.name);
                    }
                    if (terrain.variantPool != null)
                    {
                        for (int i = 0; i < terrain.variantPool.Count; i++)
                        {
                            if (terrain.variantPool[i] != null)
                            {
                                tg.allTiles.Add(terrain.variantPool[i]);
                                AddTileToScanList(terrain.variantPool[i], $"{terrain.name} (Var {i+1})");
                            }
                        }
                    }
                    if (tg.allTiles.Count > 0)
                    {
                        terrainGroups.Add(tg);
                    }
                }
            }
            ScanFloorTilesInFolder(WangRuleTileFolder);

            // 2. Scan Wall Prefabs (generic wall naming)
            wallPrefabs.Clear();
            ScanWallPrefabsMultiFolder(wallScanFolders, wallScanNamePatterns);

            // 3. Scan Prop Prefabs (mounting_*, statue_*)
            propPrefabs.Clear();
            ScanPrefabsInFolder("Assets/Prefabs/Props/ShatteredKeep_PixelLab", "mounting_", propPrefabs);
            ScanPrefabsInFolder("Assets/Prefabs/Props/ShatteredKeep_PixelLab", "statue_", propPrefabs);

            // 4. Scan Mob Prefabs (enemy_*)
            mobPrefabs.Clear();
            ScanPrefabsInFolder("Assets/Prefabs/Mobs/ShatteredKeep_PixelLab", "enemy_", mobPrefabs);

            ApplyPaletteOverrides();
        }

        private void ScanFloorTilesInFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !AssetDatabase.IsValidFolder(folderPath)) return;
            string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { folderPath });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                if (tile != null)
                {
                    AddTileToScanList(tile, CleanName(tile.name));
                }
            }
        }

        private void AddTileToScanList(TileBase tile, string name)
        {
            if (tile == null) return;
            if (floorTiles.Any(x => x.tile == tile)) return;

            Sprite s = (tile as Tile)?.sprite;
            floorTiles.Add(new ScanResult
            {
                assetPath = AssetDatabase.GetAssetPath(tile),
                displayName = name,
                tile = tile,
                icon = s
            });
        }

        private void ScanPrefabsInFolder(string folderPath, string prefixFilter, List<ScanResult> targetList)
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string filename = Path.GetFileName(path).ToLower();
                
                if (filename.StartsWith(prefixFilter.ToLower()) && filename.EndsWith(".prefab"))
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (go != null)
                    {
                        var sr = go.GetComponentInChildren<SpriteRenderer>();
                        Sprite s = sr != null ? sr.sprite : null;

                        // Check if already exists
                        if (targetList.Any(x => x.prefab == go)) continue;

                        targetList.Add(new ScanResult
                        {
                            assetPath = path,
                            displayName = CleanName(go.name),
                            prefab = go,
                            icon = s
                        });
                    }
                }
            }
        }

        private void ScanWallPrefabsMultiFolder(List<string> folders, List<string> namePatterns)
        {
            EnsureWallScanDefaults();
            if (folders == null || namePatterns == null) return;

            foreach (string folderPath in folders)
            {
                if (string.IsNullOrWhiteSpace(folderPath) || !AssetDatabase.IsValidFolder(folderPath)) continue;

                string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    string filename = Path.GetFileNameWithoutExtension(path);
                    if (!Path.GetExtension(path).Equals(".prefab", StringComparison.OrdinalIgnoreCase)) continue;
                    if (!MatchesAnyPattern(filename, namePatterns)) continue;

                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (go == null || wallPrefabs.Any(x => x.prefab == go || x.assetPath == path)) continue;

                    var sr = go.GetComponentInChildren<SpriteRenderer>(true);
                    wallPrefabs.Add(new ScanResult
                    {
                        assetPath = path,
                        displayName = CleanName(go.name),
                        prefab = go,
                        icon = sr != null ? sr.sprite : null
                    });
                }
            }
        }

        private void EnsureWallScanDefaults()
        {
            if (wallScanFolders == null)
            {
                wallScanFolders = new List<string>();
            }
            AddDefaultIfMissing(wallScanFolders, "Assets/Prefabs/Props/ShatteredKeep_PixelLab");
            AddDefaultIfMissing(wallScanFolders, "Assets/Prefabs/Walls/pilot_a");
            AddDefaultIfMissing(wallScanFolders, "Assets/Prefabs/Walls");
            AddDefaultIfMissing(wallScanFolders, WangRuleTileFolder);

            if (wallScanNamePatterns == null)
            {
                wallScanNamePatterns = new List<string>();
            }
            AddDefaultIfMissing(wallScanNamePatterns, "wall");
        }

        private static void AddDefaultIfMissing(List<string> values, string value)
        {
            if (values == null || values.Any(x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase))) return;
            values.Add(value);
        }

        private static bool MatchesAnyPattern(string filename, List<string> patterns)
        {
            if (string.IsNullOrEmpty(filename) || patterns == null) return false;
            string lower = filename.ToLowerInvariant();
            return patterns.Any(pattern => !string.IsNullOrWhiteSpace(pattern) && lower.Contains(pattern.ToLowerInvariant()));
        }

        private void ApplyPaletteOverrides()
        {
            EnsurePalettePrefsLoaded();
            foreach (PaletteCategory category in Enum.GetValues(typeof(PaletteCategory)))
            {
                List<ScanResult> list = GetPaletteList(category);
                if (list == null) continue;

                foreach (string addPath in GetPalettePathList(paletteCustomAdds, category).ToList())
                {
                    ScanResult result = CreateScanResultForAsset(category, addPath);
                    if (result == null) continue;
                    if (!list.Any(x => SameAssetPath(x.assetPath, result.assetPath)))
                    {
                        list.Add(result);
                    }
                }

                HashSet<string> excluded = new HashSet<string>(GetPalettePathList(paletteExcludes, category).Select(NormalizeAssetPath), StringComparer.OrdinalIgnoreCase);
                if (excluded.Count > 0)
                {
                    list.RemoveAll(x => x != null && excluded.Contains(NormalizeAssetPath(x.assetPath)));
                }
            }
        }

        private List<ScanResult> GetPaletteList(PaletteCategory category)
        {
            switch (category)
            {
                case PaletteCategory.Floor: return floorTiles;
                case PaletteCategory.Wall: return wallPrefabs;
                case PaletteCategory.Prop: return propPrefabs;
                case PaletteCategory.Mob: return mobPrefabs;
                default: return null;
            }
        }

        private List<string> GetPalettePathList(Dictionary<PaletteCategory, List<string>> dictionary, PaletteCategory category)
        {
            if (dictionary == null) return new List<string>();
            if (!dictionary.TryGetValue(category, out List<string> paths) || paths == null)
            {
                paths = new List<string>();
                dictionary[category] = paths;
            }
            return paths;
        }

        private ScanResult CreateScanResultForAsset(PaletteCategory category, string assetPath)
        {
            assetPath = NormalizeAssetPath(assetPath);
            if (string.IsNullOrEmpty(assetPath)) return null;

            if (category == PaletteCategory.Floor)
            {
                TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(assetPath);
                if (tile == null)
                {
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    if (sprite != null)
                    {
                        tile = CreateTileAssetForSprite(sprite);
                        assetPath = AssetDatabase.GetAssetPath(tile);
                    }
                }

                if (tile == null) return null;
                Sprite icon = (tile as Tile)?.sprite;
                return new ScanResult
                {
                    assetPath = assetPath,
                    displayName = CleanName(tile.name),
                    tile = tile,
                    icon = icon
                };
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null) return null;
            var sr = prefab.GetComponentInChildren<SpriteRenderer>(true);
            return new ScanResult
            {
                assetPath = assetPath,
                displayName = CleanName(prefab.name),
                prefab = prefab,
                icon = sr != null ? sr.sprite : null
            };
        }

        private TileBase CreateTileAssetForSprite(Sprite sprite)
        {
            if (sprite == null) return null;

            const string root = "Assets/Data";
            const string tilesRoot = "Assets/Data/Tiles";
            const string customRoot = "Assets/Data/Tiles/RimaPainterCustomPalette";
            EnsureAssetFolder("Assets", "Data");
            EnsureAssetFolder(root, "Tiles");
            EnsureAssetFolder(tilesRoot, "RimaPainterCustomPalette");

            string basePath = customRoot + "/" + SanitizeAssetFileName(sprite.name) + "_tile.asset";
            TileBase existing = AssetDatabase.LoadAssetAtPath<TileBase>(basePath);
            if (existing != null) return existing;

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            string path = AssetDatabase.GenerateUniqueAssetPath(basePath);
            AssetDatabase.CreateAsset(tile, path);
            AssetDatabase.SaveAssets();
            return tile;
        }

        private static void EnsureAssetFolder(string parent, string folder)
        {
            string path = parent + "/" + folder;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, folder);
            }
        }

        private static string SanitizeAssetFileName(string value)
        {
            if (string.IsNullOrEmpty(value)) return "custom";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(c, '_');
            }
            return value;
        }

        private static bool SameAssetPath(string a, string b)
        {
            return string.Equals(NormalizeAssetPath(a), NormalizeAssetPath(b), StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeAssetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            return path.Replace('\\', '/').Trim();
        }

        private string CleanName(string raw)
        {
            // Remove guid suffixes like mounting_00_7227fa35-ade1-...
            int firstUnderscore = raw.IndexOf('_');
            if (firstUnderscore < 0) return raw;
            int secondUnderscore = raw.IndexOf('_', firstUnderscore + 1);
            if (secondUnderscore < 0) return raw;

            // Check if there is a third part
            int thirdUnderscore = raw.IndexOf('_', secondUnderscore + 1);
            if (thirdUnderscore > 0)
            {
                return raw.Substring(0, thirdUnderscore);
            }
            return raw.Substring(0, secondUnderscore);
        }

        private static GUIContent L(string text, string tooltip)
        {
            return new GUIContent(text, tooltip);
        }

        private static string GetCategoryLabel(PaletteCategory category)
        {
            switch (category)
            {
                case PaletteCategory.Floor: return "Zemin";
                case PaletteCategory.Wall: return "Duvar";
                case PaletteCategory.Prop: return "Obje";
                case PaletteCategory.Mob: return "Canavar";
                default: return category.ToString();
            }
        }

        private static string GetCollisionModeLabel(CollisionMode mode)
        {
            switch (mode)
            {
                case CollisionMode.Auto: return "Otomatik";
                case CollisionMode.Passable: return "Geçilebilir";
                case CollisionMode.SmallFootprint: return "Küçük Engel";
                case CollisionMode.FullFootprint: return "Büyük Engel";
                case CollisionMode.WallBlock: return "Duvar";
                case CollisionMode.Custom: return "Özel";
                default: return mode.ToString();
            }
        }

        private static CollisionMode DrawCollisionModePopup(string label, CollisionMode mode, string tooltip)
        {
            CollisionMode[] values =
            {
                CollisionMode.Auto,
                CollisionMode.Passable,
                CollisionMode.SmallFootprint,
                CollisionMode.FullFootprint,
                CollisionMode.WallBlock,
                CollisionMode.Custom
            };
            string[] labels = values.Select(GetCollisionModeLabel).ToArray();
            int current = Mathf.Max(0, Array.IndexOf(values, mode));
            int next = EditorGUILayout.Popup(L(label, tooltip), current, labels);
            return values[Mathf.Clamp(next, 0, values.Length - 1)];
        }

        private void TryAutoAssignTargets()
        {
            // Try to find open RimaMapDesignerWindow and read fields via reflection
            var windowType = typeof(RimaMapDesignerWindow);
            var windows = Resources.FindObjectsOfTypeAll(windowType);
            if (windows.Length > 0)
            {
                var mapDesigner = windows[0] as EditorWindow;
                if (mapDesigner != null)
                {
                    var biomeField = windowType.GetField("activeBiome", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (biomeField != null)
                    {
                        var biomeVal = biomeField.GetValue(mapDesigner) as RimaBiomePreset;
                        if (biomeVal != null) activeBiome = biomeVal;
                    }

                    var tilemapField = windowType.GetField("outputTilemap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (tilemapField != null)
                    {
                        var tilemapVal = tilemapField.GetValue(mapDesigner) as Tilemap;
                        if (tilemapVal != null) targetTilemap = tilemapVal;
                    }
                }
            }

            // Fallbacks in scene
            if (targetTilemap == null)
            {
                targetTilemap = GameObject.FindObjectsOfType<Tilemap>().FirstOrDefault(x => x.name.Contains("Floor") || x.name.Contains("Zemin"));
            }

            if (targetTilemap != null && targetParent == null)
            {
                Transform parent = targetTilemap.transform.parent;
                if (parent != null &&
                    parent.GetComponent<Grid>() == null &&
                    parent.GetComponent<Tilemap>() == null)
                {
                    targetParent = parent;
                }
            }
        }

        private void OnGUI()
        {
            // Intercept keyboard shortcuts inside the editor window
            Event e = Event.current;
            if (e != null && e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.R && !e.alt && !e.control)
                {
                    prefabRotation = (prefabRotation + 1) % 4;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.B && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Paint;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.E && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Erase;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.I && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Eyedropper;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.Escape)
                {
                    selectedTile = null;
                    selectedPrefab = null;
                    selectedAssetName = "None";
                    selectedAssetIcon = null;
                    selectedPaletteAssetPath = string.Empty;
                    e.Use();
                    Repaint();
                }
            }

            DrawHeader();
            DrawTargetStatusBanner();

            using (new EditorGUILayout.HorizontalScope())
            {
                // Left Column: Options, Snapping, Targets
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(300f)))
                {
                    DrawOptionsPanel();
                }

                // Right Column: Search + Palette Grid
                using (new EditorGUILayout.VerticalScope())
                {
                    DrawPalettePanel();
                }
            }
        }

        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                margin = new RectOffset(6, 6, 8, 8),
                alignment = TextAnchor.MiddleLeft
            };

            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("RIMA Unified Level Designer & Painter", headerStyle);
                GUILayout.FlexibleSpace();

                GUILayout.Label("Mod:", EditorStyles.miniBoldLabel, GUILayout.Width(32f));
                PaintMode[] modes = { PaintMode.TopDown, PaintMode.Isometric };
                string[] modeLabels = { "Top-down", "Isometric" };
                int currentModeIndex = currentPaintMode == PaintMode.TopDown ? 0 : 1;
                EditorGUI.BeginChangeCheck();
                int nextModeIndex = GUILayout.Toolbar(currentModeIndex, modeLabels, EditorStyles.toolbarButton, GUILayout.Width(170f), GUILayout.Height(24f));
                if (EditorGUI.EndChangeCheck())
                {
                    SetPaintMode(modes[Mathf.Clamp(nextModeIndex, 0, modes.Length - 1)]);
                }
                 
                // Active selection preview
                using (new EditorGUILayout.HorizontalScope(GUILayout.Height(40f)))
                {
                    GUILayout.Label("Active Brush:", EditorStyles.miniBoldLabel, GUILayout.Height(30f));
                    if (selectedAssetIcon != null)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(32f, 32f, GUILayout.Width(32f), GUILayout.Height(32f));
                        GUI.DrawTexture(iconRect, selectedAssetIcon.texture, ScaleMode.ScaleToFit, true);
                    }
                    GUILayout.Label(selectedAssetName, EditorStyles.boldLabel, GUILayout.Height(30f));
                }
            }
        }

        private void DrawTargetStatusBanner()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    showStatusBanner = GUILayout.Toggle(showStatusBanner, showStatusBanner ? "v" : ">", EditorStyles.toolbarButton, GUILayout.Width(24f));
                    GUILayout.Label("Target Status", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();
                    Transform peekParent = PeekTargetParent();
                    EditorGUI.BeginDisabledGroup(peekParent == null);
                    if (GUILayout.Button("Ping", EditorStyles.toolbarButton, GUILayout.Width(48f)) && peekParent != null)
                    {
                        EditorGUIUtility.PingObject(peekParent.gameObject);
                    }
                    EditorGUI.EndDisabledGroup();
                }

                if (!showStatusBanner)
                {
                    return;
                }

                DrawStatusRow(targetTilemap != null, targetTilemap != null ? "Tilemap: " + targetTilemap.name : "No Target Tilemap - assign in Foldout 1", targetTilemap != null ? MessageType.Info : MessageType.Error);

                if (targetTilemap != null)
                {
                    Transform peekParent = PeekTargetParent();
                    string parentText;
                    MessageType parentType;
                    if (targetParent != null)
                    {
                        parentText = "Parent: " + targetParent.name;
                        parentType = MessageType.Info;
                    }
                    else if (peekParent != null && peekParent.name == "Props_Root")
                    {
                        parentText = "Parent: Props_Root (auto, exists)";
                        parentType = MessageType.Info;
                    }
                    else if (peekParent != null)
                    {
                        parentText = "Parent: " + peekParent.name + " (inferred)";
                        parentType = MessageType.Info;
                    }
                    else
                    {
                        parentText = "Parent will auto-create Props_Root on first paint";
                        parentType = MessageType.Warning;
                    }

                    DrawStatusRow(parentType == MessageType.Info, parentText, parentType);
                }

                DrawStatusRow(activeBiome != null, activeBiome != null ? "Biome: " + activeBiome.name : "Biome: None - all assets shown", activeBiome != null ? MessageType.Info : MessageType.Warning);
            }
        }

        private void DrawStatusRow(bool ok, string text, MessageType type)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUIContent icon = type == MessageType.Error
                    ? EditorGUIUtility.IconContent("console.erroricon.sml")
                    : type == MessageType.Warning
                        ? EditorGUIUtility.IconContent("console.warnicon.sml")
                        : EditorGUIUtility.IconContent("TestPassed");
                GUILayout.Label(icon, GUILayout.Width(20f), GUILayout.Height(18f));
                GUILayout.Label((ok ? "[OK] " : "[!] ") + text, EditorStyles.miniLabel);
            }
        }

        private void DrawOptionsPanel()
        {
            optionsScroll = EditorGUILayout.BeginScrollView(optionsScroll, EditorStyles.helpBox);

            EditorGUILayout.LabelField("Tür", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawCategoryButton("Zemin", PaletteCategory.Floor);
                DrawCategoryButton("Duvar", PaletteCategory.Wall);
                DrawCategoryButton("Obje", PaletteCategory.Prop);
                DrawCategoryButton("Canavar", PaletteCategory.Mob);
            }
            EditorGUILayout.Space(10);

            // Active Brush Details & Quick Cancel
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Seçili Fırça", EditorStyles.boldLabel);
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (selectedAssetIcon != null)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(40f, 40f, GUILayout.Width(40f), GUILayout.Height(40f));
                        GUI.DrawTexture(iconRect, selectedAssetIcon.texture, ScaleMode.ScaleToFit, true);
                    }
                    else
                    {
                        GUILayout.Box("No Icon", GUILayout.Width(40f), GUILayout.Height(40f));
                    }
                    
                    using (new EditorGUILayout.VerticalScope())
                    {
                        GUILayout.Label(selectedAssetName, EditorStyles.boldLabel);
                        GUILayout.Label($"Tür: {GetCategoryLabel(currentCategory)}", EditorStyles.miniLabel);
                    }
                }
                
                if (selectedTile != null || selectedPrefab != null)
                {
                    EditorGUILayout.Space(5);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(L("Fırçayı Temizle (Esc)", "Seçili fırçayı kapatır."), GUILayout.Height(20f)))
                        {
                            selectedTile = null;
                            selectedPrefab = null;
                            selectedAssetName = "None";
                            selectedAssetIcon = null;
                            selectedPaletteAssetPath = string.Empty;
                        }
                    }
                }
            }
            EditorGUILayout.Space(10);

            // 1. Target Configuration Foldout
            showTargetsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showTargetsSection, "1. Hedef");
            if (showTargetsSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUI.BeginChangeCheck();
                    activeBiome = (RimaBiomePreset)EditorGUILayout.ObjectField(L("Aktif Biyom", "Zemin paletinin beslendiği biyom."), activeBiome, typeof(RimaBiomePreset), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        ScanAllAssets();
                    }

                    targetTilemap = (Tilemap)EditorGUILayout.ObjectField(L("Hedef Zemin", "Zemin boyamasının yapılacağı Tilemap."), targetTilemap, typeof(Tilemap), true);
                    targetParent = (Transform)EditorGUILayout.ObjectField(L("Hedef Klasör", "Objelerin sahnede yerleşeceği ana klasör."), targetParent, typeof(Transform), true);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);

            // 2. Painting Tools (Core - Always visible)
            EditorGUILayout.LabelField("2. Fırça", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawToolButton("Fırça (B)", ToolMode.Paint);
                DrawToolButton("Silgi (E)", ToolMode.Erase);
                DrawToolButton("Damlalık (I)", ToolMode.Eyedropper);
            }
            EditorGUILayout.Space(10);

            // 3. Tool Settings Foldout
            showToolSettingsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showToolSettingsSection, "3. Ayarlar");
            if (showToolSettingsSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField(L("Hizalama Modu", "Header'daki painter mod toggle'i ile degistirilir."), new GUIContent(currentPaintMode == PaintMode.Isometric ? "Isometric" : "Top-down"));
                    snapToGrid = EditorGUILayout.Toggle(L("Hücreye Hizala", "Yerleşimi grid hücresine kilitler."), snapToGrid);
                    
                    if (currentCategory == PaletteCategory.Floor)
                    {
                        brushSize = EditorGUILayout.IntSlider(L("Fırça Boyutu", "Tek seferde boyanan zemin hücresi alanı."), brushSize, 1, 5);
                        useRandomVariants = EditorGUILayout.Toggle(L("Çeşitlilik", "Varyantları rastgele seçer."), useRandomVariants);
                    }
                    else
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField(L("Dönme (R)", "Objeyi 90 derece döndürür."), GUILayout.Width(80f));
                            if (GUILayout.Button("Rot L ↺", GUILayout.Width(60f)))
                            {
                                prefabRotation = (prefabRotation + 3) % 4;
                            }
                            GUILayout.Label($"{prefabRotation * 90}°", EditorStyles.centeredGreyMiniLabel);
                            if (GUILayout.Button("Rot R ↻", GUILayout.Width(60f)))
                            {
                                prefabRotation = (prefabRotation + 1) % 4;
                            }
                        }
                    }

                    EditorGUILayout.Space(5);
                    EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
                    useCategoryScale = EditorGUILayout.Toggle("Per-Category Scale", useCategoryScale);
                    using (new EditorGUI.DisabledScope(!useCategoryScale))
                    {
                        floorScale = EditorGUILayout.Slider("Floor", floorScale, 0.1f, 2f);
                        wallScale = EditorGUILayout.Slider("Wall", wallScale, 0.1f, 2f);
                        propScale = EditorGUILayout.Slider("Prop", propScale, 0.1f, 2f);
                        mobScale = EditorGUILayout.Slider("Mob", mobScale, 0.1f, 2f);
                    }
                    using (new EditorGUI.DisabledScope(useCategoryScale))
                    {
                        prefabScaleMultiplier = EditorGUILayout.Slider("Universal Override", prefabScaleMultiplier, 0.1f, 2f);
                    }

                    if (currentCategory != PaletteCategory.Floor)
                    {
                        EditorGUILayout.Space(5);
                        autoAlignBaseToGrid = EditorGUILayout.Toggle(L("Tabanı Hizala", "Sprite tabanını hücre altına yaklaştırır."), autoAlignBaseToGrid);
                        positionOffset = EditorGUILayout.Vector3Field(L("Konum İnce Ayar", "Yerleşime küçük ek kaydırma uygular."), positionOffset);

                        if (currentCategory == PaletteCategory.Wall)
                        {
                            EditorGUILayout.Space(5);
                            autoConnectWalls = EditorGUILayout.Toggle(L("Duvarları Otomatik Bağla", "Komşu duvarlara göre doğru parçayı seçer."), autoConnectWalls);
                            if (autoConnectWalls)
                            {
                                wallRuleTileMode = EditorGUILayout.Toggle(L("Wall RuleTile Mode", "Wall Tilemap üzerinde RuleTile ile otomatik bağlantı boyar."), wallRuleTileMode);
                                using (new EditorGUI.DisabledScope(!wallRuleTileMode))
                                {
                                    wallRuleTile = (TileBase)EditorGUILayout.ObjectField(L("Wall RuleTile", "Otomatik bağlantı için kullanılacak RuleTile asset'i."), wallRuleTile, typeof(TileBase), false);
                                }
                                randomizeWallCracks = EditorGUILayout.Toggle(L("Duvar Bozulması Rastgele", "Bazı duvarları hasarlı varyantla değiştirir."), randomizeWallCracks);
                                if (GUILayout.Button(L("Bağlantıları Yenile", "Sahnedeki duvar bağlantılarını yeniden hesaplar."), GUILayout.Height(20f)))
                                {
                                    RebuildAllWallConnections();
                                }
                            }
                        }
                    }

                    EditorGUILayout.Space(5);
                    if (GUILayout.Button(L("Ayarları Sıfırla", "Fırça ayarlarını varsayılana döndürür."), GUILayout.Height(20f)))
                    {
                        ResetToolSettings();
                    }

                    EditorGUILayout.Space(5);
                    EditorGUILayout.HelpBox("Use standard keys in Scene View:\n- R: Rotate prefab\n- B: Brush Tool\n- E: Erase Tool\n- I: Eyedropper\n- Esc: Clear active brush", MessageType.Info);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);

            DrawCollisionInspector();

            // 5. Map Management Foldout
            showMapManagementSection = EditorGUILayout.BeginFoldoutHeaderGroup(showMapManagementSection, "5. Level File Management");
            if (showMapManagementSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    string activeMapDisplay = string.IsNullOrEmpty(activeMapName) ? "None (Unsaved)" : activeMapName;
                    GUILayout.Label($"Active: {activeMapDisplay}", EditorStyles.miniBoldLabel);
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (!string.IsNullOrEmpty(activeMapPath))
                        {
                            if (GUILayout.Button("Save", GUILayout.Height(22f)))
                            {
                                SaveMapData(activeMapPath);
                            }
                        }
                        if (GUILayout.Button("Save As...", GUILayout.Height(22f)))
                        {
                            SaveActiveMapAs();
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("New Map", GUILayout.Height(22f)))
                        {
                            CreateNewMap();
                        }
                        if (GUILayout.Button("Refresh List", GUILayout.Height(22f)))
                        {
                            RefreshSavedMapsList();
                        }
                    }

                    EditorGUILayout.Space(5);
                    if (GUILayout.Button("Attach IsoSorter to All Placed Objects", GUILayout.Height(22f)))
                    {
                        AttachIsoSorterToAllPlacedObjects();
                    }

                    showMapList = EditorGUILayout.Foldout(showMapList, "Saved Level Files", true);
                    if (showMapList)
                    {
                        if (savedMapPaths == null || savedMapPaths.Count == 0)
                        {
                            EditorGUILayout.LabelField("No maps found in UnifiedMaps/", EditorStyles.miniLabel);
                        }
                        else
                        {
                            EditorGUI.indentLevel++;
                            foreach (var path in savedMapPaths)
                            {
                                string mName = Path.GetFileNameWithoutExtension(path);
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    bool isActive = path == activeMapPath;
                                    GUIStyle nameStyle = isActive ? EditorStyles.boldLabel : EditorStyles.label;
                                    GUILayout.Label(mName, nameStyle);
                                    
                                    if (GUILayout.Button("Load", GUILayout.Width(45f)))
                                    {
                                        if (EditorUtility.DisplayDialog("Load Map", $"Load '{mName}' and overwrite the scene canvas?", "Yes", "No"))
                                        {
                                            LoadMapData(path);
                                        }
                                    }
                                    if (GUILayout.Button("Fix", GUILayout.Width(40f)))
                                    {
                                        if (EditorUtility.DisplayDialog("Overwrite Map", $"Overwrite '{mName}' with the current canvas?", "Yes", "No"))
                                        {
                                            SaveMapData(path);
                                        }
                                    }
                                }
                            }
                            EditorGUI.indentLevel--;
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(10);

            DrawSceneOrganizationPanel();
            DrawSelectedInstanceEditor();

            if (targetTilemap == null)
            {
                EditorGUILayout.HelpBox("Assign a Target Tilemap to start painting tiles.", MessageType.Warning);
            }

            if (GUILayout.Button("Rescan & Sync Assets", GUILayout.Height(30f)))
            {
                ScanAllAssets();
                TryAutoAssignTargets();
            }

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Setup Asset Pack Colliders", GUILayout.Height(26f)))
            {
                if (EditorUtility.DisplayDialog("Setup Asset Pack Colliders?", 
                    "This will automatically configure colliders for all prefabs in ShatteredKeep_PixelLab:\n\n- wall naming & statue_* will get BoxCollider2D (Blocking)\n- mounting_* will have colliders removed (Passable)\n\nAre you sure you want to proceed?", "Yes", "No"))
                {
                    ConfigureAssetPackColliders();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawCollisionInspector()
        {
            if (currentCategory == PaletteCategory.Floor)
            {
                return;
            }

            showPrefabSettingsSection = EditorGUILayout.BeginFoldoutHeaderGroup(showPrefabSettingsSection, "4. Çarpışma");
            if (showPrefabSettingsSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    float effectiveScale = GetEffectiveScale(currentCategory);
                    CollisionResolver.ResolvedCollider resolved = ResolveCollisionPreview(selectedPrefab, currentCategory, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, prefabRotation);
                    EditorGUILayout.LabelField("Aktif", GetCollisionModeLabel(resolved.effectiveMode), EditorStyles.boldLabel);

                    EditorGUI.BeginChangeCheck();
                    customCollisionMode = DrawCollisionModePopup("Çarpışma", customCollisionMode, "Objenin oyuncuyu engelleyip engellemediği.");
                    if (customCollisionMode == CollisionMode.Custom)
                    {
                        EditorGUILayout.HelpBox("Özel çarpışmayı sahnede seçili obje üzerinden düzenleyin.", MessageType.Info);
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        SceneView.RepaintAll();
                    }

                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Önizleme", EditorStyles.miniBoldLabel);
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.Vector2Field("Alan", resolved.worldSize);
                        EditorGUILayout.Vector2Field("Kaydırma", resolved.worldOffset);
                    }

                    showCollisionGizmo = EditorGUILayout.Toggle(L("Scene'de Çarpışmayı Göster", "Çarpışma alanını Scene penceresinde çizer."), showCollisionGizmo);

                    EditorGUILayout.Space(4);
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.ObjectField(L("Kural Seti", "Otomatik çarpışma ayarlarının kaynağı."), collisionRules, typeof(CollisionRulesSO), false);
                        if (GUILayout.Button(L("Aç", "Kural setini düzenleme penceresinde açar."), GUILayout.Width(44f)))
                        {
                            if (collisionRules != null)
                            {
                                EditorUtility.OpenPropertyEditor(collisionRules);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);
        }

        private void DrawSceneOrganizationPanel()
        {
            showSceneOrgSection = EditorGUILayout.BeginFoldoutHeaderGroup(showSceneOrgSection, "6. Scene Organization");
            if (showSceneOrgSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    if (sceneOrgDirty)
                    {
                        sceneOrgDirty = false;
                    }

                    Transform root = PeekTargetParent();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Root: " + (root != null ? root.name : "None"), EditorStyles.miniBoldLabel);
                        GUILayout.FlexibleSpace();
                        EditorGUI.BeginDisabledGroup(root == null);
                        if (GUILayout.Button("Ping", GUILayout.Width(44f)) && root != null)
                        {
                            EditorGUIUtility.PingObject(root.gameObject);
                        }
                        EditorGUI.EndDisabledGroup();
                    }

                    int total = 0;
                    foreach (string groupName in GroupClassifier.CanonicalGroups)
                    {
                        Transform group = root != null ? root.Find(groupName) : null;
                        int count = group != null ? group.childCount : 0;
                        total += count;
                        DrawSceneGroupRow(root, groupName, group, count);
                    }

                    DrawSeparator();
                    EditorGUILayout.LabelField("Total instances: " + total, EditorStyles.miniLabel);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);
        }

        private void DrawSceneGroupRow(Transform root, string groupName, Transform group, int count)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                bool expanded = SessionState.GetBool("RIMA.UnifiedPainter.groupExpand." + groupName, true);
                bool nextExpanded = EditorGUILayout.Foldout(expanded, groupName, true, EditorStyles.foldout);
                if (nextExpanded != expanded)
                {
                    SessionState.SetBool("RIMA.UnifiedPainter.groupExpand." + groupName, nextExpanded);
                }

                GUILayout.Label(count.ToString(), EditorStyles.miniLabel, GUILayout.Width(26f));

                if (group == null)
                {
                    GUI.enabled = root != null;
                    if (GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(24f)) && root != null)
                    {
                        CreateGroupTransform(root, groupName);
                    }
                    GUI.enabled = true;
                    GUILayout.Label("(empty)", EditorStyles.centeredGreyMiniLabel);
                    return;
                }

                GameObject groupGo = group.gameObject;
                bool hidden = SceneVisibilityManager.instance.IsHidden(groupGo, includeDescendants: true);
                GUIContent visibilityContent = EditorGUIUtility.IconContent(hidden ? "animationvisibilitytoggleoff" : "animationvisibilitytoggleon");
                visibilityContent.tooltip = "Toggle editor visibility (Scene View only, does not affect save or runtime).";
                if (GUILayout.Button(visibilityContent, EditorStyles.miniButton, GUILayout.Width(26f)))
                {
                    if (hidden) SceneVisibilityManager.instance.Show(groupGo, includeDescendants: true);
                    else SceneVisibilityManager.instance.Hide(groupGo, includeDescendants: true);
                    SceneView.RepaintAll();
                }

                bool pickingDisabled = SceneVisibilityManager.instance.IsPickingDisabled(groupGo, includeDescendants: true);
                GUIContent lockContent = EditorGUIUtility.IconContent(pickingDisabled ? "InspectorLock" : "InspectorLockOff");
                lockContent.tooltip = "Toggle editor scene picking. Editor state is stored under Library and may reset.";
                if (GUILayout.Button(lockContent, EditorStyles.miniButton, GUILayout.Width(26f)))
                {
                    if (pickingDisabled) SceneVisibilityManager.instance.EnablePicking(groupGo, includeDescendants: true);
                    else SceneVisibilityManager.instance.DisablePicking(groupGo, includeDescendants: true);
                    SceneView.RepaintAll();
                }

                if (GUILayout.Button("All", EditorStyles.miniButton, GUILayout.Width(30f)))
                {
                    Selection.objects = group.Cast<Transform>().Select(t => t.gameObject).ToArray();
                }

                if (GUILayout.Button("Frame", EditorStyles.miniButton, GUILayout.Width(46f)))
                {
                    FrameGroup(group);
                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), EditorStyles.miniButton, GUILayout.Width(26f)))
                {
                    ClearOrDeleteGroup(group, groupName, count);
                }
            }
        }

        private void DrawSelectedInstanceEditor()
        {
            showSelectedInstanceSection = EditorGUILayout.BeginFoldoutHeaderGroup(showSelectedInstanceSection, "7. Seçili Obje");
            if (showSelectedInstanceSection)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    GameObject instance = GetSelectedEditableInstance();
                    if (instance == null)
                    {
                        EditorGUILayout.LabelField("Düzenlenebilir obje seçili değil.", EditorStyles.miniLabel);
                    }
                    else
                    {
                        Transform parentGroup = instance.transform.parent;
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField("Obje", instance.name, EditorStyles.boldLabel);
                            if (GUILayout.Button(L("Bul", "Objeyi Project/Hierarchy içinde gösterir."), GUILayout.Width(44f))) EditorGUIUtility.PingObject(instance);
                            if (GUILayout.Button(L("Odak", "Scene kamerasını objeye taşır."), GUILayout.Width(50f))) FrameObject(instance);
                        }

                        EditorGUILayout.LabelField("Klasör", parentGroup != null ? parentGroup.name : "Yok", EditorStyles.miniLabel);
                        DrawSelectedMoveTo(instance);

                        BoxCollider2D box = instance.GetComponentInChildren<BoxCollider2D>(true);
                        CollisionMode instanceMode = box != null ? CollisionMode.Custom : CollisionResolver.Resolve(PrefabUtility.GetCorrespondingObjectFromSource(instance), GuessCategory(instance), CollisionMode.Auto, Vector2.one, Vector2.zero, GetInstanceScale(instance), Mathf.RoundToInt(instance.transform.eulerAngles.z / 90f), collisionRules).effectiveMode;

                        EditorGUILayout.Space(4);
                        EditorGUI.BeginChangeCheck();
                        CollisionMode newMode = DrawCollisionModePopup("Tür", instanceMode, "Seçili objenin geçilebilir ya da engel olduğunu belirler.");
                        if (newMode == CollisionMode.Custom)
                        {
                            if (box == null) box = Undo.AddComponent<BoxCollider2D>(instance);
                            EditorGUILayout.HelpBox("Çarpışma alanını Scene'deki handle ile düzenleyin.", MessageType.Info);
                            if (EditorGUI.EndChangeCheck())
                            {
                                EditorUtility.SetDirty(box);
                            }
                        }
                        else if (EditorGUI.EndChangeCheck())
                        {
                            ApplyInstanceCollision(instance, newMode);
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUI.BeginDisabledGroup(box == null);
                            if (GUILayout.Button(L("Scene'de Düzenle", "Çarpışma alanını Scene penceresinde düzenler.")) && box != null)
                            {
                                customCollisionMode = CollisionMode.Custom;
                                Undo.RecordObject(box, "Edit Collider");
                                if (activeColliderEditor != null) DestroyImmediate(activeColliderEditor);
                                activeColliderEditor = UnityEditor.Editor.CreateEditor(box);
                                UnityEditorInternal.EditMode.ChangeEditMode(UnityEditorInternal.EditMode.SceneViewEditMode.Collider, box.bounds, activeColliderEditor);
                            }
                            EditorGUI.EndDisabledGroup();

                            if (GUILayout.Button(L("Otomatik", "Çarpışmayı otomatik ayara döndürür.")))
                            {
                                ApplyInstanceCollision(instance, CollisionMode.Auto);
                            }
                        }

                        EditorGUILayout.Space(4);
                        EditorGUI.BeginChangeCheck();
                        float worldScale = EditorGUILayout.FloatField(L("Boyut", "Parent ölçeğini hesaba katan dünya ölçeği."), GetInstanceScale(instance));
                        EditorGUILayout.LabelField(L("Dönme", "Seçili objenin dönüş açısı."));
                        
                        bool isWall = IsWallObject(instance);
                        bool useIsometricWallLogic = isWall && (currentPaintMode == PaintMode.Isometric);
                        int currentRot = 0;
                        if (useIsometricWallLogic)
                        {
                            var sr = instance.GetComponentInChildren<SpriteRenderer>();
                            currentRot = (sr != null && sr.flipX) ? 90 : 0;
                        }
                        else
                        {
                            currentRot = Mathf.RoundToInt(instance.transform.localEulerAngles.z / 90f) * 90;
                        }

                        int rotation = EditorGUILayout.IntPopup(currentRot, new[] { "0", "90", "180", "270" }, new[] { 0, 90, 180, 270 });
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (useIsometricWallLogic)
                            {
                                Undo.RecordObject(instance.transform, "Edit Instance Transform");
                                instance.transform.localEulerAngles = new Vector3(instance.transform.localEulerAngles.x, instance.transform.localEulerAngles.y, 0f);
                                
                                bool shouldFlipX = (rotation == 90 || rotation == 270);
                                var srs = instance.GetComponentsInChildren<SpriteRenderer>(true);
                                foreach (var sr in srs)
                                {
                                    Undo.RecordObject(sr, "Flip SpriteRenderer");
                                    sr.flipX = shouldFlipX;
                                    EditorUtility.SetDirty(sr);
                                }
                                SetUniformWorldScale(instance.transform, worldScale);
                            }
                            else
                            {
                                Undo.RecordObject(instance.transform, "Edit Instance Transform");
                                instance.transform.localEulerAngles = new Vector3(instance.transform.localEulerAngles.x, instance.transform.localEulerAngles.y, rotation);
                                SetUniformWorldScale(instance.transform, worldScale);
                            }
                        }

                        if (GUILayout.Button(L("Sil", "Seçili objeyi sahneden kaldırır.")))
                        {
                            Undo.DestroyObjectImmediate(instance);
                        }
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space(5);
        }

        private CollisionResolver.ResolvedCollider ResolveCollisionPreview(GameObject prefab, PaletteCategory category, CollisionMode mode, Vector2 size, Vector2 offset, float scale, int rotation)
        {
            PreviewCacheKey key = new PreviewCacheKey(prefab, category, mode, scale, rotation, size, offset, collisionRules);
            if (!collisionPreviewCache.TryGetValue(key, out CollisionResolver.ResolvedCollider resolved))
            {
                resolved = CollisionResolver.Resolve(prefab, category, mode, size, offset, scale, rotation, collisionRules);
                collisionPreviewCache[key] = resolved;
            }
            return resolved;
        }

        private Transform CreateGroupTransform(Transform root, string groupName)
        {
            if (root == null) return null;
            GameObject groupGo = new GameObject(groupName);
            Undo.RegisterCreatedObjectUndo(groupGo, "Create Group " + groupName);
            groupGo.transform.SetParent(root, false);
            sceneOrgDirty = true;
            return groupGo.transform;
        }

        private void ClearOrDeleteGroup(Transform group, string groupName, int count)
        {
            if (group == null) return;
            string message = count > 0 ? $"Delete {count} objects in group {groupName}?" : $"Delete empty group {groupName}?";
            if (!EditorUtility.DisplayDialog("Clear Group", message, "Delete", "Cancel"))
            {
                return;
            }

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Clear " + groupName);
            if (count == 0)
            {
                Undo.DestroyObjectImmediate(group.gameObject);
            }
            else
            {
                List<GameObject> children = group.Cast<Transform>().Select(t => t.gameObject).ToList();
                foreach (GameObject child in children)
                {
                    Undo.DestroyObjectImmediate(child);
                }
            }
            sceneOrgDirty = true;
        }

        private void FrameGroup(Transform group)
        {
            if (group == null || SceneView.lastActiveSceneView == null) return;
            var renderers = group.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                Selection.activeGameObject = group.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
                return;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            SceneView.lastActiveSceneView.Frame(bounds, false);
        }

        private void FrameObject(GameObject go)
        {
            if (go == null || SceneView.lastActiveSceneView == null) return;
            Selection.activeGameObject = go;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private void DrawSelectedMoveTo(GameObject instance)
        {
            Transform root = PeekTargetParent();
            if (instance == null || root == null) return;
            string currentGroup = instance.transform.parent != null ? instance.transform.parent.name : GroupClassifier.CanonicalGroups[0];
            int currentIndex = Mathf.Max(0, Array.IndexOf(GroupClassifier.CanonicalGroups, currentGroup));
            EditorGUI.BeginChangeCheck();
            int nextIndex = EditorGUILayout.Popup(L("Klasör değiştir", "Seçili objeyi başka sahne klasörüne taşır."), currentIndex, GroupClassifier.CanonicalGroups);
            if (EditorGUI.EndChangeCheck() && nextIndex >= 0 && nextIndex < GroupClassifier.CanonicalGroups.Length)
            {
                string nextGroupName = GroupClassifier.CanonicalGroups[nextIndex];
                Transform nextGroup = root.Find(nextGroupName) ?? CreateGroupTransform(root, nextGroupName);
                if (nextGroup != null)
                {
                    Undo.SetTransformParent(instance.transform, nextGroup, "Move Instance Group");
                    instance.transform.SetParent(nextGroup, worldPositionStays: true);
                    sceneOrgDirty = true;
                }
            }
        }

        private GameObject GetSelectedEditableInstance()
        {
            GameObject selected = Selection.activeGameObject;
            Transform root = PeekTargetParent();
            if (selected == null || root == null)
            {
                return null;
            }

            if (selected.transform == root)
            {
                return null;
            }

            GameObject placed = FindPlacedParent(selected, root);
            if (placed != null && placed.GetComponentInChildren<SpriteRenderer>(true) != null)
            {
                return placed;
            }

            if (selected.transform.IsChildOf(root) && selected.GetComponentInChildren<SpriteRenderer>(true) != null)
            {
                return selected;
            }

            return null;
        }

        private PaletteCategory GuessCategory(GameObject instance)
        {
            string group = instance != null && instance.transform.parent != null ? instance.transform.parent.name : string.Empty;
            string name = instance != null ? instance.name.ToLowerInvariant() : string.Empty;
            if (group == "Walls" || name.Contains("wall") || IsWallObject(instance)) return PaletteCategory.Wall;
            if (group == "Mobs" || name.StartsWith("mob_") || name.StartsWith("enemy_")) return PaletteCategory.Mob;
            return PaletteCategory.Prop;
        }

        private bool IsWallObject(GameObject go)
        {
            if (go == null) return false;
            if (!string.IsNullOrEmpty(go.name) && go.name.ToLowerInvariant().Contains("wall")) return true;

            var renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var renderer in renderers)
            {
                if (renderer != null &&
                    renderer.sprite != null &&
                    !string.IsNullOrEmpty(renderer.sprite.name) &&
                    renderer.sprite.name.ToLowerInvariant().Contains("wall"))
                {
                    return true;
                }
            }
            return false;
        }

        private float GetInstanceScale(GameObject instance)
        {
            if (instance == null) return 1f;
            float scale = Mathf.Abs(instance.transform.lossyScale.y);
            return scale > 0f ? scale : 1f;
        }

        private void SetUniformWorldScale(Transform target, float worldScale)
        {
            if (target == null) return;
            float safeWorldScale = Mathf.Max(0.01f, worldScale);
            float parentScaleY = 1f;
            if (target.parent != null)
            {
                parentScaleY = Mathf.Abs(target.parent.lossyScale.y);
                if (parentScaleY <= 0.0001f) parentScaleY = 1f;
            }
            target.localScale = Vector3.one * (safeWorldScale / parentScaleY);
        }

        private void ApplyInstanceCollision(GameObject instance, CollisionMode mode)
        {
            if (instance == null) return;
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(instance);
            PaletteCategory category = GuessCategory(instance);
            CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(prefab != null ? prefab : instance, category, mode, Vector2.one, Vector2.zero, GetInstanceScale(instance), Mathf.RoundToInt(instance.transform.eulerAngles.z / 90f), collisionRules);
            ConfigureCollider(instance, resolved.effectiveMode, GetInstanceScale(instance));
            ApplySorting(instance, resolved);
            EditorUtility.SetDirty(instance);
        }

        private void ApplySorting(GameObject go, CollisionResolver.ResolvedCollider resolved)
        {
            if (go == null) return;
            var renderers = go.GetComponentsInChildren<SpriteRenderer>(true);
            bool isWall = GuessCategory(go) == PaletteCategory.Wall || IsWallObject(go);
            if (isWall)
            {
                RemoveIsoSorter(go);
            }
            foreach (var r in renderers)
            {
                if (!string.IsNullOrEmpty(resolved.layerName))
                {
                    r.sortingLayerName = resolved.layerName;
                }
                if (r.sortingOrder == 0 || resolved.sortingOrder == 20)
                {
                    r.sortingOrder = resolved.sortingOrder;
                }
                if (isWall)
                {
                    RemoveIsoSorter(r.gameObject);
                    continue;
                }
                var sorter = r.GetComponent<RIMA.IsoSorter>();
                if (sorter == null)
                {
                    sorter = r.gameObject.AddComponent<RIMA.IsoSorter>();
                }
            }
        }

        private bool RemoveIsoSorter(GameObject go)
        {
            if (go == null) return false;
            var sorter = go.GetComponent<RIMA.IsoSorter>();
            if (sorter == null) return false;
            Undo.DestroyObjectImmediate(sorter);
            EditorUtility.SetDirty(go);
            return true;
        }

        private float GetEffectiveScale(PaletteCategory category)
        {
            if (!useCategoryScale)
            {
                return prefabScaleMultiplier;
            }

            switch (category)
            {
                case PaletteCategory.Floor: return floorScale;
                case PaletteCategory.Wall: return wallScale;
                case PaletteCategory.Prop: return propScale;
                case PaletteCategory.Mob: return mobScale;
                default: return 1.0f;
            }
        }

        private void ResetToolSettings()
        {
            snapToGrid = true;
            brushSize = 1;
            useRandomVariants = false;
            prefabRotation = 0;
            prefabScaleMultiplier = 1.0f;
            floorScale = 1.0f;
            wallScale = 0.5f;
            propScale = 0.4f;
            mobScale = 1.0f;
            useCategoryScale = true;
            autoAlignBaseToGrid = true;
            positionOffset = Vector3.zero;
            customCollisionMode = CollisionMode.Auto;
            customColliderSize = Vector2.one;
            customColliderOffset = Vector2.zero;
        }

        private void DrawCategoryButton(string label, PaletteCategory category)
        {
            bool isSelected = currentCategory == category;
            Color prevBg = GUI.backgroundColor;
            if (isSelected) GUI.backgroundColor = new Color(0.2f, 0.6f, 1f, 1f);
            
            if (GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Height(24f)))
            {
                currentCategory = category;
                ClearSelection();
            }
            GUI.backgroundColor = prevBg;
        }

        private void DrawToolButton(string label, ToolMode tool)
        {
            bool isSelected = currentTool == tool;
            Color prevBg = GUI.backgroundColor;
            if (isSelected) GUI.backgroundColor = new Color(0.2f, 0.7f, 0.3f, 1f);
            
            if (GUILayout.Button(label, EditorStyles.miniButton, GUILayout.Height(26f)))
            {
                currentTool = tool;
            }
            GUI.backgroundColor = prevBg;
        }

        private void ClearSelection()
        {
            selectedTile = null;
            selectedPrefab = null;
            selectedAssetName = "None";
            selectedAssetIcon = null;
            selectedPaletteAssetPath = string.Empty;
            ClearPreviewObject();
        }

        private void DrawPaletteActionButtons(int itemCount)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Label($"Palette of {currentCategory} - {itemCount} items", EditorStyles.miniBoldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add From Project", GUILayout.Width(140f), GUILayout.Height(24f)))
                {
                    AddPaletteAssetFromProject();
                }

                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(selectedPaletteAssetPath));
                if (GUILayout.Button("Remove Selected", GUILayout.Width(130f), GUILayout.Height(24f)))
                {
                    RemoveSelectedPaletteAsset();
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        private void AddPaletteAssetFromProject()
        {
            string filter = currentCategory == PaletteCategory.Floor ? "asset,png,psd,tga,jpg,jpeg" : "prefab";
            string fullPath = EditorUtility.OpenFilePanel("Add From Project", Application.dataPath, filter);
            if (string.IsNullOrEmpty(fullPath)) return;

            string assetPath = AbsolutePathToAssetPath(fullPath);
            if (string.IsNullOrEmpty(assetPath))
            {
                EditorUtility.DisplayDialog("Invalid Asset", "Choose an asset under this Unity project's Assets folder.", "OK");
                return;
            }

            ScanResult result = CreateScanResultForAsset(currentCategory, assetPath);
            if (result == null)
            {
                EditorUtility.DisplayDialog("Invalid Asset", $"Selected asset cannot be added to {currentCategory} palette.", "OK");
                return;
            }

            assetPath = NormalizeAssetPath(result.assetPath);
            List<string> adds = GetPalettePathList(paletteCustomAdds, currentCategory);
            List<string> excludes = GetPalettePathList(paletteExcludes, currentCategory);
            excludes.RemoveAll(x => SameAssetPath(x, assetPath));
            if (!adds.Any(x => SameAssetPath(x, assetPath)))
            {
                adds.Add(assetPath);
            }

            SavePalettePrefs();
            ScanAllAssets();
            SelectPaletteResult(result);
            Repaint();
        }

        private void RemoveSelectedPaletteAsset()
        {
            string assetPath = NormalizeAssetPath(selectedPaletteAssetPath);
            if (string.IsNullOrEmpty(assetPath)) return;

            bool confirmed = EditorUtility.DisplayDialog(
                "Remove Selected",
                "Bu asset palette'ten kaldırılsın mı? (asset dosyası SİLİNMEZ, sadece palette'ten çıkar)",
                "Remove",
                "Cancel");
            if (!confirmed) return;

            List<string> adds = GetPalettePathList(paletteCustomAdds, currentCategory);
            List<string> excludes = GetPalettePathList(paletteExcludes, currentCategory);
            adds.RemoveAll(x => SameAssetPath(x, assetPath));
            if (!excludes.Any(x => SameAssetPath(x, assetPath)))
            {
                excludes.Add(assetPath);
            }

            SavePalettePrefs();
            ClearSelection();
            ScanAllAssets();
            Repaint();
        }

        private static string AbsolutePathToAssetPath(string fullPath)
        {
            fullPath = NormalizeAssetPath(fullPath);
            string dataPath = NormalizeAssetPath(Application.dataPath);
            if (!fullPath.StartsWith(dataPath, StringComparison.OrdinalIgnoreCase)) return string.Empty;
            return "Assets" + fullPath.Substring(dataPath.Length);
        }

        private void SelectPaletteResult(ScanResult item)
        {
            if (item == null) return;
            if (currentCategory == PaletteCategory.Floor)
            {
                selectedTile = item.tile;
                selectedPrefab = null;
            }
            else
            {
                selectedPrefab = item.prefab;
                selectedTile = null;
            }

            selectedAssetName = item.displayName;
            selectedAssetIcon = item.icon;
            selectedPaletteAssetPath = NormalizeAssetPath(item.assetPath);
            customCollisionMode = CollisionMode.Auto;
            ClearPreviewObject();
        }

        private void DrawPalettePanel()
        {
            // Search field
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Filter Search:", GUILayout.Width(80f));
                searchQuery = EditorGUILayout.TextField(searchQuery, EditorStyles.toolbarSearchField);
                if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(45f)))
                {
                    searchQuery = string.Empty;
                }
            }

            paletteScroll = EditorGUILayout.BeginScrollView(paletteScroll, EditorStyles.helpBox);

            List<ScanResult> activeList = null;
            switch (currentCategory)
            {
                case PaletteCategory.Floor: activeList = floorTiles; break;
                case PaletteCategory.Wall: activeList = wallPrefabs; break;
                case PaletteCategory.Prop: activeList = propPrefabs; break;
                case PaletteCategory.Mob: activeList = mobPrefabs; break;
            }

            if (activeList == null)
            {
                activeList = new List<ScanResult>();
            }

            if (activeList.Count == 0)
            {
                EditorGUILayout.HelpBox("No assets found in this category. Make sure paths and filters match.", MessageType.Info);
                EditorGUILayout.EndScrollView();
                DrawPaletteActionButtons(0);
                return;
            }

            // Filter the list based on query
            var filtered = activeList.Where(x => string.IsNullOrEmpty(searchQuery) || x.displayName.ToLower().Contains(searchQuery.ToLower())).ToList();

            // Draw items in responsive Grid
            float viewWidth = position.width - 320f; // subtract options panel width
            float itemWidth = 110f;
            float itemHeight = 130f;
            int columns = Mathf.Max(1, Mathf.FloorToInt(viewWidth / itemWidth));

            int column = 0;
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < filtered.Count; i++)
            {
                var result = filtered[i];
                DrawPaletteItemButton(result, itemWidth, itemHeight);

                column++;
                if (column >= columns)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    column = 0;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            DrawPaletteActionButtons(filtered.Count);
        }

        private void DrawPaletteItemButton(ScanResult item, float width, float height)
        {
            Rect btnRect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
            
            bool isSelected = false;
            if (currentCategory == PaletteCategory.Floor)
            {
                isSelected = selectedTile == item.tile;
            }
            else
            {
                isSelected = selectedPrefab == item.prefab;
            }

            bool hover = btnRect.Contains(Event.current.mousePosition);
            Color hoverColor = hover ? new Color(1f, 1f, 1f, 0.1f) : Color.clear;
            EditorGUI.DrawRect(btnRect, isSelected ? new Color(0.2f, 0.5f, 0.9f, 0.25f) : hoverColor);

            // Thumbnail Rect
            Rect thumbRect = new Rect(btnRect.x + 7f, btnRect.y + 6f, 96f, 96f);
            Texture2D thumbnail = null;

            if (currentCategory == PaletteCategory.Floor && item.tile != null)
            {
                thumbnail = GetCachedPreview(item.tile);
            }
            else if (item.prefab != null)
            {
                thumbnail = GetCachedPreview(item.prefab);
            }

            if (thumbnail != null)
            {
                GUI.DrawTexture(thumbRect, thumbnail, ScaleMode.ScaleToFit);
            }
            else if (item.icon != null && item.icon.texture != null)
            {
                Rect tc = new Rect(
                    item.icon.rect.x / item.icon.texture.width,
                    item.icon.rect.y / item.icon.texture.height,
                    item.icon.rect.width / item.icon.texture.width,
                    item.icon.rect.height / item.icon.texture.height);
                GUI.DrawTextureWithTexCoords(thumbRect, item.icon.texture, tc);
            }
            else
            {
                EditorGUI.DrawRect(thumbRect, new Color(0.2f, 0.2f, 0.2f));
                GUI.Label(thumbRect, "No Image", EditorStyles.centeredGreyMiniLabel);
            }

            // Outline
            Color outlineColor = isSelected ? new Color(0f, 0.7f, 1f, 0.8f) : (hover ? new Color(1f, 1f, 1f, 0.3f) : new Color(0.25f, 0.25f, 0.25f, 0.5f));
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y, btnRect.width, 1f), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y + btnRect.height - 1f, btnRect.width, 1f), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y, 1f, btnRect.height), outlineColor);
            EditorGUI.DrawRect(new Rect(btnRect.x + btnRect.width - 1f, btnRect.y, 1f, btnRect.height), outlineColor);

            if (isSelected)
            {
                Color innerColor = new Color(0f, 0.7f, 1f, 0.8f);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + 1f, btnRect.width - 2f, 1f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + btnRect.height - 2f, btnRect.width - 2f, 1f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + 1f, btnRect.y + 1f, 1f, btnRect.height - 2f), innerColor);
                EditorGUI.DrawRect(new Rect(btnRect.x + btnRect.width - 2f, btnRect.y + 1f, 1f, btnRect.height - 2f), innerColor);
            }

            // Label Rect
            Rect labelRect = new Rect(btnRect.x + 4f, btnRect.y + height - 34f, btnRect.width - 28f, 30f);
            GUIStyle labelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                clipping = TextClipping.Clip,
                wordWrap = true,
                fontSize = 9
            };
            GUI.Label(labelRect, item.displayName.Replace("_", "_\u200B"), labelStyle);

            if (currentCategory == PaletteCategory.Wall || currentCategory == PaletteCategory.Prop)
            {
                float effectiveScale = GetEffectiveScale(currentCategory);
                CollisionResolver.ResolvedCollider badge = ResolveCollisionPreview(item.prefab, currentCategory, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, prefabRotation);
                DrawCollisionBadge(new Rect(btnRect.x + btnRect.width - 22f, btnRect.y + btnRect.height - 24f, 18f, 18f), badge.effectiveMode);
            }

            // Handle Selection click
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && hover)
            {
                SelectPaletteResult(item);
                
                // Automatically switch to Paint mode if Eyedropper selected
                if (currentTool == ToolMode.Eyedropper)
                {
                    currentTool = ToolMode.Paint;
                }

                Event.current.Use();
                Repaint();
            }
        }

        private void DrawCollisionBadge(Rect rect, CollisionMode mode)
        {
            string letter = "P";
            string tooltip = "Walks through";
            Color color = new Color(0.23f, 0.82f, 0.48f, 0.95f);

            switch (mode)
            {
                case CollisionMode.WallBlock:
                    letter = "B";
                    tooltip = "Wall blocker";
                    color = new Color(0.84f, 0.29f, 0.29f, 0.95f);
                    break;
                case CollisionMode.SmallFootprint:
                    letter = "S";
                    tooltip = "Small collider";
                    color = new Color(0.9f, 0.77f, 0.29f, 0.95f);
                    break;
                case CollisionMode.FullFootprint:
                    letter = "F";
                    tooltip = "Full footprint blocker";
                    color = new Color(0.9f, 0.54f, 0.23f, 0.95f);
                    break;
                case CollisionMode.Custom:
                    letter = "C";
                    tooltip = "Custom override";
                    color = new Color(0.75f, 0.31f, 0.82f, 0.95f);
                    break;
            }

            EditorGUI.DrawRect(rect, color);
            GUIStyle badgeStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.black },
                fontSize = 10
            };
            GUI.Label(rect, new GUIContent(letter, tooltip), badgeStyle);
        }

        private Texture2D GetCachedPreview(UnityEngine.Object asset)
        {
            if (asset == null) return null;
            int id = asset.GetInstanceID();
            
            if (previewCache.TryGetValue(id, out Texture2D tex) && tex != null)
            {
                return tex;
            }

            tex = AssetPreview.GetAssetPreview(asset);
            if (tex != null)
            {
                previewCache[id] = tex;
            }
            return tex;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            // Intercept hotkeys
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode == KeyCode.R && !e.alt && !e.control)
                {
                    prefabRotation = (prefabRotation + 1) % 4;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.B && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Paint;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.E && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Erase;
                    e.Use();
                    Repaint();
                }
                else if (e.keyCode == KeyCode.I && !e.alt && !e.control)
                {
                    currentTool = ToolMode.Eyedropper;
                    e.Use();
                    Repaint();
                }
            }

            if (currentTool != ToolMode.Erase)
            {
                cachedHoveredObject = null;
            }

            // Check if there is an active brush, or if we are in Erase / Eyedropper mode
            bool hasActiveBrush = (currentCategory == PaletteCategory.Floor && selectedTile != null) ||
                                  (currentCategory != PaletteCategory.Floor && selectedPrefab != null);

            if (!hasActiveBrush && currentTool != ToolMode.Erase && currentTool != ToolMode.Eyedropper)
            {
                ClearPreviewObject();
                return;
            }

            // Disable standard Unity selection click
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            // Get snapped cell and center positions
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Grid grid = GameObject.FindObjectOfType<Grid>();
            
            Vector3 worldPos = Vector3.zero;
            Vector3 gridNormal = Vector3.forward;
            Vector3 gridPoint = Vector3.zero;
            if (targetTilemap != null)
            {
                gridNormal = targetTilemap.transform.forward;
                gridPoint = targetTilemap.transform.position;
            }
            else if (grid != null)
            {
                gridNormal = grid.transform.forward;
                gridPoint = grid.transform.position;
            }
            
            Plane gridPlane = new Plane(gridNormal, gridPoint);
            if (gridPlane.Raycast(ray, out float enter))
            {
                worldPos = ray.GetPoint(enter);
            }
            else
            {
                worldPos = new Vector3(ray.origin.x, ray.origin.y, 0f);
            }

            Vector3Int cellPos;
            Vector3 snapPos;
            GetCellAndSnapPos(worldPos, out cellPos, out snapPos);

            // Draw scene previews/outlines
            if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Floor && selectedTile != null)
            {
                ClearPreviewObject();
                DrawCellOutline(targetTilemap, grid, cellPos, new Color(0.2f, 0.8f, 1f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Wall && selectedPrefab != null)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Prop && selectedPrefab != null)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(0.2f, 1f, 0.35f, 0.8f));
            }
            else if (currentTool == ToolMode.Paint && currentCategory == PaletteCategory.Mob && selectedPrefab != null)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                UpdatePreviewObject(selectedPrefab, snapPos, finalOffset, prefabRotation);
                DrawPrefabOutline(selectedPrefab, snapPos + finalOffset, prefabRotation, new Color(1f, 0.4f, 0.1f, 0.8f));
            }
            else if (currentTool == ToolMode.Erase)
            {
                ClearPreviewObject();
                if (cachedHoveredObject != null)
                {
                    DrawTargetObjectOutline(cachedHoveredObject, new Color(1f, 0.2f, 0.2f, 0.8f));
                }
                else
                {
                    DrawCellOutline(targetTilemap, grid, cellPos, new Color(1f, 0.2f, 0.2f, 0.8f));
                }
            }
            else if (currentTool == ToolMode.Eyedropper)
            {
                ClearPreviewObject();
                DrawCellOutline(targetTilemap, grid, cellPos, new Color(1f, 0.85f, 0f, 0.8f));
            }

            if (showCollisionGizmo)
            {
                DrawActiveCollisionGizmo(snapPos);
            }

            // Paint/Erase actions
            if (e.button == 0 && !e.alt && !e.control)
            {
                if (e.type == EventType.MouseDown)
                {
                    GUIUtility.hotControl = controlId;
                    PerformAction(cellPos, snapPos);
                    e.Use();
                }
                else if (e.type == EventType.MouseDrag && GUIUtility.hotControl == controlId)
                {
                    PerformAction(cellPos, snapPos);
                    e.Use();
                }
                else if (e.type == EventType.MouseUp && GUIUtility.hotControl == controlId)
                {
                    GUIUtility.hotControl = 0;
                    e.Use();
                }
            }

            if (e.type == EventType.MouseMove || e.type == EventType.MouseDrag || e.type == EventType.MouseDown || e.type == EventType.MouseUp)
            {
                bool hoverChanged = false;
                if (currentTool == ToolMode.Erase)
                {
                    GameObject currentHover = GetEraseTargetObject(cellPos, snapPos);
                    if (currentHover != cachedHoveredObject)
                    {
                        cachedHoveredObject = currentHover;
                        hoverChanged = true;
                    }
                }

                if (cellPos != lastCellPos || hoverChanged || e.type == EventType.MouseDown || e.type == EventType.MouseUp)
                {
                    lastCellPos = cellPos;
                    SceneView.RepaintAll();
                }
            }
        }

        private void GetCellAndSnapPos(Vector3 worldPos, out Vector3Int cellPos, out Vector3 snapPos)
        {
            if (targetTilemap != null)
            {
                Vector3 localPos = targetTilemap.transform.InverseTransformPoint(worldPos);
                cellPos = targetTilemap.LocalToCell(localPos);
                Vector3 localCenter = targetTilemap.GetCellCenterLocal(cellPos);
                snapPos = targetTilemap.transform.TransformPoint(localCenter);
            }
            else
            {
                Grid grid = GameObject.FindObjectOfType<Grid>();
                if (grid != null)
                {
                    Vector3 localPos = grid.transform.InverseTransformPoint(worldPos);
                    cellPos = grid.LocalToCell(localPos);
                    Vector3 localCenter = grid.GetCellCenterLocal(cellPos);
                    snapPos = grid.transform.TransformPoint(localCenter);
                }
                else
                {
                    cellPos = new Vector3Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), 0);
                    snapPos = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0f);
                }
            }
        }

        private Vector3 GetCellWorldPosition(Vector3Int cellPos)
        {
            if (targetTilemap != null)
            {
                Vector3 localPos = targetTilemap.GetCellCenterLocal(cellPos);
                return targetTilemap.transform.TransformPoint(localPos);
            }
            Grid grid = GameObject.FindObjectOfType<Grid>();
            if (grid != null)
            {
                Vector3 localPos = grid.GetCellCenterLocal(cellPos);
                return grid.transform.TransformPoint(localPos);
            }
            return new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0f);
        }

        private void DrawCellOutline(Tilemap tilemap, Grid grid, Vector3Int cellPos, Color color)
        {
            Vector3 worldC0, worldC1, worldC2, worldC3;

            if (tilemap != null)
            {
                worldC0 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos));
                worldC1 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                worldC2 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                worldC3 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else if (grid != null)
            {
                worldC0 = grid.transform.TransformPoint(grid.CellToLocal(cellPos));
                worldC1 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                worldC2 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                worldC3 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else
            {
                worldC0 = new Vector3(cellPos.x, cellPos.y, 0f);
                worldC1 = new Vector3(cellPos.x + 1f, cellPos.y, 0f);
                worldC2 = new Vector3(cellPos.x + 1f, cellPos.y + 1f, 0f);
                worldC3 = new Vector3(cellPos.x, cellPos.y + 1f, 0f);
            }

            Handles.color = color;
            Handles.DrawAAPolyLine(3f, worldC0, worldC1, worldC2, worldC3, worldC0);

            Color fillCol = color;
            fillCol.a = 0.15f;
            Handles.DrawSolidRectangleWithOutline(new[] { worldC0, worldC1, worldC2, worldC3 }, fillCol, Color.clear);
        }

        private void DrawBoxOutline(Tilemap tilemap, Grid grid, Vector3Int cellPos, Color color, float height)
        {
            Vector3 b0, b1, b2, b3;

            if (tilemap != null)
            {
                b0 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos));
                b1 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                b2 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                b3 = tilemap.transform.TransformPoint(tilemap.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else if (grid != null)
            {
                b0 = grid.transform.TransformPoint(grid.CellToLocal(cellPos));
                b1 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 0, 0)));
                b2 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(1, 1, 0)));
                b3 = grid.transform.TransformPoint(grid.CellToLocal(cellPos + new Vector3Int(0, 1, 0)));
            }
            else
            {
                b0 = new Vector3(cellPos.x, cellPos.y, 0f);
                b1 = new Vector3(cellPos.x + 1f, cellPos.y, 0f);
                b2 = new Vector3(cellPos.x + 1f, cellPos.y + 1f, 0f);
                b3 = new Vector3(cellPos.x, cellPos.y + 1f, 0f);
            }

            Vector3 offset = new Vector3(0f, height, 0f);
            Vector3 t0 = b0 + offset;
            Vector3 t1 = b1 + offset;
            Vector3 t2 = b2 + offset;
            Vector3 t3 = b3 + offset;

            Handles.color = color;
            
            // Draw base diamond (dotted)
            Handles.DrawDottedLine(b0, b1, 2f);
            Handles.DrawDottedLine(b1, b2, 2f);
            Handles.DrawDottedLine(b2, b3, 2f);
            Handles.DrawDottedLine(b3, b0, 2f);

            // Draw top diamond (solid)
            Handles.DrawAAPolyLine(3f, t0, t1, t2, t3, t0);

            // Draw vertical pillars
            Handles.DrawAAPolyLine(2f, b0, t0);
            Handles.DrawAAPolyLine(2f, b1, t1);
            Handles.DrawAAPolyLine(2f, b2, t2);
            Handles.DrawAAPolyLine(2f, b3, t3);

            // Draw transparent fill on the top face
            Color topFill = color;
            topFill.a = 0.1f;
            Handles.DrawSolidRectangleWithOutline(new[] { t0, t1, t2, t3 }, topFill, Color.clear);
            
            // Draw transparent fill on the side faces facing the camera
            Color sideFill = color;
            sideFill.a = 0.05f;
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b1, t1, t0 }, sideFill, Color.clear);
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b3, t3, t0 }, sideFill, Color.clear);
        }

        private void UpdatePreviewObject(GameObject sourcePrefab, Vector3 position, Vector3 offset, int rotationSteps)
        {
            if (sourcePrefab == null)
            {
                ClearPreviewObject();
                return;
            }

            if (previewObject == null || lastPreviewPrefab != sourcePrefab)
            {
                ClearPreviewObject();
                previewObject = Instantiate(sourcePrefab);
                previewObject.name = "__RimaWorldPainter_Preview__";
                previewObject.hideFlags = HideFlags.HideAndDontSave;

                // Strip non-visual gameplay scripts/colliders
                var components = previewObject.GetComponentsInChildren<Component>(true);
                foreach (var comp in components)
                {
                    if (comp == null) continue;
                    if (comp is Transform || comp is Renderer || comp is SpriteRenderer || comp is MeshFilter || comp is MeshRenderer)
                    {
                        continue;
                    }
                    DestroyImmediate(comp);
                }

                // Opacity reduction for ghost effect and set sorting order
                int previewSortingOrder = 0;
                if (currentCategory == PaletteCategory.Wall) previewSortingOrder = 20;
                else if (currentCategory == PaletteCategory.Prop) previewSortingOrder = 30;
                else if (currentCategory == PaletteCategory.Mob) previewSortingOrder = 40;

                var renderers = previewObject.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var r in renderers)
                {
                    Color col = r.color;
                    col.a = 0.55f;
                    r.color = col;
                    
                    if (r.sortingOrder == 0)
                    {
                        r.sortingOrder = previewSortingOrder;
                    }
                }

                lastPreviewPrefab = sourcePrefab;
            }

            bool isWall = (currentCategory == PaletteCategory.Wall) || IsWallObject(sourcePrefab);
            previewObject.transform.position = position + offset;
            
            bool useIsometricWallLogic = isWall && (currentPaintMode == PaintMode.Isometric);
            float effectiveRotation = useIsometricWallLogic ? 0f : rotationSteps * 90f;
            previewObject.transform.rotation = Quaternion.Euler(0f, 0f, effectiveRotation);
            
            var previewRenderers = previewObject.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var r in previewRenderers)
            {
                if (useIsometricWallLogic)
                {
                    r.flipX = (rotationSteps == 1 || rotationSteps == 3);
                }
                else
                {
                    r.flipX = false;
                }
            }
            
            // In the world, we want the preview to be at original prefab world size (un-squashed)
            float effectiveScale = GetEffectiveScale(currentCategory);
            previewObject.transform.localScale = sourcePrefab.transform.localScale * effectiveScale;
            
            lastPreviewRotation = rotationSteps;
        }

        private void ClearPreviewObject()
        {
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
                previewObject = null;
            }
            lastPreviewPrefab = null;
        }

        private void PerformAction(Vector3Int cellPos, Vector3 snapPos)
        {
            if (currentTool == ToolMode.Paint)
            {
                if (currentCategory == PaletteCategory.Floor && selectedTile != null)
                {
                    PaintTile(cellPos, selectedTile);
                }
                else if (currentCategory == PaletteCategory.Wall && autoConnectWalls && wallRuleTileMode)
                {
                    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    {
                        if (wallRuleTile == null)
                        {
                            LoadDefaultWallRuleTile();
                        }

                        if (wallRuleTile != null)
                        {
                            PaintTile(cellPos, wallRuleTile);
                        }
                    }
                }
                else if (selectedPrefab != null)
                {
                    bool isPressOrDrag = Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag;
                    if (currentCategory == PaletteCategory.Wall && autoConnectWalls)
                    {
                        if (isPressOrDrag && FindWallAtCell(cellPos) == null)
                        {
                            PaintWallWithConnections(cellPos, snapPos, selectedPrefab);
                        }
                    }
                    else if (currentCategory == PaletteCategory.Prop)
                    {
                        if (isPressOrDrag)
                        {
                            PaintPrefab(snapPos, selectedPrefab);
                        }
                    }
                    else if (Event.current.type == EventType.MouseDown)
                    {
                        PaintPrefab(snapPos, selectedPrefab);
                    }
                }
            }
            else if (currentTool == ToolMode.Erase)
            {
                if (currentCategory == PaletteCategory.Floor)
                {
                    // Drag to erase floor tiles
                    EraseAt(cellPos, snapPos);
                }
                else
                {
                    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                    {
                        EraseAt(cellPos, snapPos);
                    }
                }
            }
            else if (currentTool == ToolMode.Eyedropper)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    PerformEyedropper(cellPos, snapPos);
                }
            }
        }

        private void PaintTile(Vector3Int cellPos, TileBase tile)
        {
            if (targetTilemap == null) return;

            // Brush size loop
            int half = brushSize / 2;
            int oddOffset = brushSize % 2 == 0 ? 0 : 0;
            
            Undo.RegisterCompleteObjectUndo(targetTilemap, "Paint Tiles");

            for (int dx = -half; dx <= half + oddOffset; dx++)
            {
                for (int dy = -half; dy <= half + oddOffset; dy++)
                {
                    Vector3Int pos = cellPos + new Vector3Int(dx, dy, 0);
                    TileBase finalTile = tile;
                    if (useRandomVariants)
                    {
                        finalTile = GetRandomTileFromGroup(tile);
                    }
                    targetTilemap.SetTile(pos, finalTile);
                }
            }
            EditorUtility.SetDirty(targetTilemap);
        }

        private TileBase GetRandomTileFromGroup(TileBase tile)
        {
            if (tile == null) return null;
            
            // Find if there is a group that contains this tile
            var group = terrainGroups.Find(tg => tg.allTiles.Contains(tile));
            if (group != null && group.allTiles.Count > 0)
            {
                int rIndex = UnityEngine.Random.Range(0, group.allTiles.Count);
                return group.allTiles[rIndex];
            }
            return tile;
        }

        private void LoadDefaultWallRuleTile()
        {
            if (wallRuleTile != null) return;
            wallRuleTile = AssetDatabase.LoadAssetAtPath<TileBase>(WallRuleTileAssetPath);
            if (wallRuleTile != null) return;
            if (!AssetDatabase.IsValidFolder(WangRuleTileFolder)) return;

            string[] guids = AssetDatabase.FindAssets("broken wall t:TileBase", new[] { WangRuleTileFolder });
            if (guids.Length == 0)
            {
                guids = AssetDatabase.FindAssets("wall t:TileBase", new[] { WangRuleTileFolder });
            }
            if (guids.Length == 0) return;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            wallRuleTile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
        }

        private GameObject GetRandomWallVariantFromGroup(GameObject prefab)
        {
            if (!useRandomVariants || prefab == null || wallVariantGroups == null || wallVariantGroups.Count == 0)
            {
                return prefab;
            }

            foreach (WallVariantGroup group in wallVariantGroups)
            {
                if (group == null) continue;
                bool matchesBase = group.basePrefab == prefab;
                bool matchesVariant = group.variants != null && group.variants.Contains(prefab);
                if (!matchesBase && !matchesVariant) continue;

                List<GameObject> candidates = new List<GameObject>();
                if (group.basePrefab != null) candidates.Add(group.basePrefab);
                if (group.variants != null)
                {
                    candidates.AddRange(group.variants.Where(x => x != null));
                }
                candidates = candidates.Distinct().ToList();
                if (candidates.Count == 0) return prefab;
                return candidates[UnityEngine.Random.Range(0, candidates.Count)];
            }

            return prefab;
        }

        private void PaintPrefab(Vector3 snapPos, GameObject prefab)
        {
            if (currentCategory == PaletteCategory.Wall)
            {
                if (!TryResolveTopDownWallPrefab(ref prefab, prefabRotation, true))
                {
                    return;
                }
                prefab = GetRandomWallVariantFromGroup(prefab);
            }
            Transform baseParent = GetTargetParent();
            Transform parent = GetOrCreateGroupParent(baseParent, prefab.name, currentCategory);
            Vector3 finalOffset = GetPlacementOffset(prefab);
            Vector3 targetPos = snapPos + finalOffset;
            
            // Check if there is already an object placed in the exact cell to avoid stacking
            if (baseParent != null)
            {
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(baseParent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child != null && child.GetComponent<SpriteRenderer>() != null && Vector3.Distance(child.position, targetPos) < 0.1f)
                    {
                        return; // Already occupied
                    }
                }
            }

            GameObject placed = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (parent != null)
            {
                placed.transform.SetParent(parent, false);
            }
            placed.transform.position = targetPos;
            
            bool isWall = (currentCategory == PaletteCategory.Wall) || IsWallObject(placed);
            bool useIsometricWallLogic = isWall && (currentPaintMode == PaintMode.Isometric);
            int placementRotationSteps = currentPaintMode == PaintMode.TopDown && isWall && IsTopDownNsDirection(prefabRotation) ? 0 : prefabRotation;
            float effectiveRotationDegrees = useIsometricWallLogic ? 0f : placementRotationSteps * 90f;
            float effectiveScale = GetEffectiveScale(currentCategory);
            placed.transform.rotation = Quaternion.Euler(0f, 0f, effectiveRotationDegrees);
            
            if (isWall)
            {
                var srs = placed.GetComponentsInChildren<SpriteRenderer>(true);
                bool shouldFlipX = useIsometricWallLogic && (prefabRotation == 1 || prefabRotation == 3);
                foreach (var sr in srs)
                {
                    sr.flipX = shouldFlipX;
                }
            }
            
            // Un-squash localScale relative to parent's lossyScale to maintain the prefab's intended world scale
            if (parent != null)
            {
                placed.transform.localScale = ComputeCompensatedLocalScale(
                    parent,
                    prefab.transform.localScale * effectiveScale,
                    effectiveRotationDegrees
                );
            }
            else
            {
                placed.transform.localScale = prefab.transform.localScale * effectiveScale;
            }

            int effectiveCollisionRotation = useIsometricWallLogic ? 0 : placementRotationSteps;
            CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(prefab, currentCategory, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, effectiveCollisionRotation, collisionRules);
            ConfigureCollider(placed, resolved.effectiveMode, effectiveScale);
            ApplySorting(placed, resolved);

            Undo.RegisterCreatedObjectUndo(placed, "Place Prefab");
            EditorUtility.SetDirty(placed);
            
            // Sync Scene GUI
            if (parent != null)
            {
                EditorUtility.SetDirty(parent.gameObject);
            }
        }

        private void EraseAt(Vector3Int cellPos, Vector3 snapPos)
        {
            if (currentCategory == PaletteCategory.Floor)
            {
                // Only erase Floor tiles
                if (targetTilemap != null && targetTilemap.HasTile(cellPos))
                {
                    Undo.RegisterCompleteObjectUndo(targetTilemap, "Erase Tiles");
                    targetTilemap.SetTile(cellPos, null);
                    EditorUtility.SetDirty(targetTilemap);
                }
            }
            else
            {
                // Only erase GameObjects
                Transform parent = GetTargetParent();
                if (parent != null)
                {
                    List<GameObject> toDelete = new List<GameObject>();

                    if (cachedHoveredObject != null && cachedHoveredObject.transform.IsChildOf(parent))
                    {
                        toDelete.Add(cachedHoveredObject);
                        cachedHoveredObject = null;
                    }
                    else
                    {
                        // Fallback: check objects snapped to the same cell position or within close distance
                        List<Transform> allChildren = new List<Transform>();
                        GetRecursiveChildren(parent, allChildren);
                        foreach (Transform child in allChildren)
                        {
                            if (child == null || child.gameObject == null) continue;
                            if (child.GetComponent<Tilemap>() != null) continue;
                            if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

                            Vector3Int childCell;
                            Vector3 childSnap;
                            GetCellAndSnapPos(child.position, out childCell, out childSnap);

                            if (childCell == cellPos || Vector3.Distance(child.position, snapPos) < 0.4f)
                            {
                                toDelete.Add(child.gameObject);
                            }
                        }
                    }

                    if (toDelete.Count > 0)
                    {
                        foreach (var go in toDelete)
                        {
                            Undo.DestroyObjectImmediate(go);
                        }
                        EditorUtility.SetDirty(parent.gameObject);

                        if (currentCategory == PaletteCategory.Wall && autoConnectWalls)
                        {
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(1, 0, 0));  // NE
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(-1, 0, 0)); // SW
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, 1, 0));  // NW
                            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, -1, 0)); // SE
                        }
                    }
                }
            }
        }

        private GameObject FindPlacedParent(GameObject picked, Transform root)
        {
            if (picked == null || root == null) return null;
            Transform current = picked.transform;
            Transform child = current;
            while (current != null && current != root)
            {
                if (current.parent == root)
                {
                    string n = current.name;
                    if (n == "Walls" || n == "Statues" || n == "WallMountings" || n == "Patches" || n == "Mobs" || n == "FloorProps")
                    {
                        return child.gameObject;
                    }
                    return current.gameObject;
                }
                child = current;
                current = current.parent;
            }
            return null;
        }

        private void DrawPrefabOutline(GameObject prefab, Vector3 snapPos, int rotation, Color color)
        {
            if (prefab == null) return;

            var sr = prefab.GetComponentInChildren<SpriteRenderer>();
            float spriteHeight = 1.0f;
            if (sr != null && sr.sprite != null)
            {
                spriteHeight = sr.sprite.bounds.size.y;
            }

            float effectiveScale = GetEffectiveScale(currentCategory);
            float scale = effectiveScale != 0f ? effectiveScale : 1.0f;
            float h = spriteHeight * scale;
            CollisionResolver.ResolvedCollider resolved = ResolveCollisionPreview(prefab, currentCategory, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, rotation);
            Vector2 localSize = resolved.worldSize / scale;
            Vector2 localOffset = resolved.worldOffset / scale;

            // Compute local corners relative to the pivot (0, 0)
            float halfX = localSize.x / 2f;
            float halfY = localSize.y / 2f;
            Vector3 localC0 = localOffset + new Vector2(-halfX, -halfY);
            Vector3 localC1 = localOffset + new Vector2(halfX, -halfY);
            Vector3 localC2 = localOffset + new Vector2(halfX, halfY);
            Vector3 localC3 = localOffset + new Vector2(-halfX, halfY);

            // Rotate and scale the local corners, then translate to snapPos
            Quaternion rot = Quaternion.Euler(0f, 0f, rotation * 90f);
            Vector3 b0 = snapPos + rot * Vector3.Scale(localC0, new Vector3(scale, scale, 1f));
            Vector3 b1 = snapPos + rot * Vector3.Scale(localC1, new Vector3(scale, scale, 1f));
            Vector3 b2 = snapPos + rot * Vector3.Scale(localC2, new Vector3(scale, scale, 1f));
            Vector3 b3 = snapPos + rot * Vector3.Scale(localC3, new Vector3(scale, scale, 1f));

            // Project upwards by sprite height
            Vector3 offset = new Vector3(0f, h, 0f);
            Vector3 t0 = b0 + offset;
            Vector3 t1 = b1 + offset;
            Vector3 t2 = b2 + offset;
            Vector3 t3 = b3 + offset;

            Handles.color = color;

            // Draw base rectangle (dotted)
            Handles.DrawDottedLine(b0, b1, 2f);
            Handles.DrawDottedLine(b1, b2, 2f);
            Handles.DrawDottedLine(b2, b3, 2f);
            Handles.DrawDottedLine(b3, b0, 2f);

            // Draw top rectangle (solid)
            Handles.DrawAAPolyLine(3f, t0, t1, t2, t3, t0);

            // Draw vertical pillars
            Handles.DrawLine(b0, t0);
            Handles.DrawLine(b1, t1);
            Handles.DrawLine(b2, t2);
            Handles.DrawLine(b3, t3);
        }

        private void PerformEyedropper(Vector3Int cellPos, Vector3 snapPos)
        {
            // Try Eyedropper on prefab first
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(parent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child == null || child.GetComponent<SpriteRenderer>() == null) continue;
                    if (Vector3.Distance(child.position, snapPos) < 0.25f)
                    {
                        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                        if (sourcePrefab == null)
                        {
                            GameObject outermostRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(child.gameObject);
                            if (outermostRoot != null)
                            {
                                sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(outermostRoot);
                            }
                        }

                        if (sourcePrefab != null)
                        {
                            // Select category
                            string filename = sourcePrefab.name.ToLower();
                            if (filename.Contains("wall")) currentCategory = PaletteCategory.Wall;
                            else if (filename.StartsWith("enemy_")) currentCategory = PaletteCategory.Mob;
                            else currentCategory = PaletteCategory.Prop;

                            selectedPrefab = sourcePrefab;
                            selectedTile = null;
                            selectedAssetName = CleanName(sourcePrefab.name);
                            customCollisionMode = CollisionMode.Auto;

                            float origScaleX = sourcePrefab.transform.localScale.x;
                            if (origScaleX != 0f)
                            {
                                prefabScaleMultiplier = child.transform.lossyScale.x / origScaleX;
                            }
                            else
                            {
                                prefabScaleMultiplier = 1.0f;
                            }
                            
                            var sr = sourcePrefab.GetComponentInChildren<SpriteRenderer>();
                            selectedAssetIcon = sr != null ? sr.sprite : null;

                            currentTool = ToolMode.Paint;
                            bool useIsometricWallLogic = (currentCategory == PaletteCategory.Wall || IsWallObject(child.gameObject)) && (currentPaintMode == PaintMode.Isometric);
                            if (useIsometricWallLogic)
                            {
                                var childSr = child.GetComponentInChildren<SpriteRenderer>();
                                prefabRotation = (childSr != null && childSr.flipX) ? 1 : 0;
                            }
                            else
                            {
                                prefabRotation = Mathf.RoundToInt(child.transform.rotation.eulerAngles.z / 90f) % 4;
                            }

                            Repaint();
                            return;
                        }
                    }
                }
            }

            // Try Eyedropper on Floor Tile
            if (targetTilemap != null)
            {
                TileBase tile = targetTilemap.GetTile(cellPos);
                if (tile != null)
                {
                    currentCategory = PaletteCategory.Floor;
                    selectedTile = tile;
                    selectedPrefab = null;
                    selectedAssetName = tile.name;

                    Sprite s = (tile as Tile)?.sprite;
                    selectedAssetIcon = s;

                    currentTool = ToolMode.Paint;
                    Repaint();
                }
            }
        }



        private void ConfigureAssetPackColliders()
        {
            string folderPath = "Assets/Prefabs/Props/ShatteredKeep_PixelLab";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError($"[World Painter] Folder not found: {folderPath}");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });
            int modifiedCount = 0;

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;

                string name = prefab.name.ToLower();

                // Load prefab contents as editing root
                GameObject editRoot = PrefabUtility.LoadPrefabContents(path);
                
                PaletteCategory category = name.Contains("wall") ? PaletteCategory.Wall : PaletteCategory.Prop;
                CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(prefab, category, CollisionMode.Auto, customColliderSize, customColliderOffset, 0.5f, 0, collisionRules);
                ConfigureCollider(editRoot, resolved.effectiveMode, 0.5f);
                ApplySorting(editRoot, resolved);
                
                PrefabUtility.SaveAsPrefabAsset(editRoot, path);
                modifiedCount++;

                PrefabUtility.UnloadPrefabContents(editRoot);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[World Painter] Configured {modifiedCount} prefab assets in {folderPath}.");
            EditorUtility.DisplayDialog("Success", $"Successfully updated {modifiedCount} prefabs in ShatteredKeep_PixelLab pack!", "OK");
        }

        private Vector3 ComputeCompensatedLocalScale(Transform parent, Vector3 targetWorldScale, float rotationAngleDegrees)
        {
            if (currentPaintMode == PaintMode.TopDown)
            {
                return targetWorldScale;
            }

            if (parent == null)
            {
                return targetWorldScale;
            }
            
            Vector3 parentScale = parent.lossyScale;
            if (Mathf.Approximately(parentScale.x, parentScale.y) && Mathf.Approximately(parentScale.y, parentScale.z))
            {
                // Uniform parent scale: localScale = targetWorldScale / parentScale.x
                float pScale = parentScale.x != 0f ? parentScale.x : 1f;
                return targetWorldScale / pScale;
            }
            
            // Non-uniform parent scale: compute scale along local axes to prevent skewing/shearing
            float rad = rotationAngleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);
            
            float px = parentScale.x != 0f ? parentScale.x : 1f;
            float py = parentScale.y != 0f ? parentScale.y : 1f;
            float pz = parentScale.z != 0f ? parentScale.z : 1f;
            
            float lx = Mathf.Sqrt((cos * px) * (cos * px) + (sin * py) * (sin * py));
            float ly = Mathf.Sqrt((-sin * px) * (-sin * px) + (cos * py) * (cos * py));
            
            float sx = lx > 0f ? targetWorldScale.x / lx : targetWorldScale.x;
            float sy = ly > 0f ? targetWorldScale.y / ly : targetWorldScale.y;
            float sz = pz > 0f ? targetWorldScale.z / pz : targetWorldScale.z;
            
            return new Vector3(sx, sy, sz);
        }

        private CollisionMode GetDefaultCollisionMode(GameObject prefab, PaletteCategory category)
        {
            float effectiveScale = GetEffectiveScale(category);
            return CollisionResolver.Resolve(prefab, category, CollisionMode.Auto, Vector2.one, Vector2.zero, effectiveScale, prefabRotation, collisionRules).effectiveMode;
        }

        private void ConfigureCollider(GameObject placed, CollisionMode mode, float scaleMultiplier)
        {
            if (placed == null) return;
            PaletteCategory category = GuessCategory(placed);
            CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(placed, category, mode, customColliderSize, customColliderOffset, scaleMultiplier, Mathf.RoundToInt(placed.transform.eulerAngles.z / 90f), collisionRules);

            // First, remove any existing colliders if they exist
            var existingColliders = placed.GetComponentsInChildren<Collider2D>(true);
            foreach (var col in existingColliders)
            {
                DestroyImmediate(col);
            }

            if (resolved.effectiveMode == CollisionMode.Passable)
            {
                return; // Passable, no collider needed
            }

            float scale = scaleMultiplier != 0f ? scaleMultiplier : 1.0f;

            var newBox = placed.AddComponent<BoxCollider2D>();
            newBox.size = resolved.worldSize / scale;
            newBox.offset = resolved.worldOffset / scale;

            if (!string.IsNullOrEmpty(resolved.layerName))
            {
                int layer = LayerMask.NameToLayer(resolved.layerName);
                if (layer != -1)
                {
                    placed.layer = layer;
                }
            }
        }

        private struct SpriteVisibleBounds
        {
            public float minX;
            public float maxX;
            public float minY;
            public float maxY;
        }

        private Dictionary<Sprite, SpriteVisibleBounds> visibleBoundsCache = new Dictionary<Sprite, SpriteVisibleBounds>();

        private Vector3 GetPlacementOffset(GameObject prefab)
        {
            Vector3 finalOffset = positionOffset;
            if (currentPaintMode == PaintMode.TopDown)
            {
                return finalOffset;
            }

            if (autoAlignBaseToGrid && currentCategory != PaletteCategory.Floor && prefab != null)
            {
                var sr = prefab.GetComponentInChildren<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    int rotationForOffset = (currentCategory == PaletteCategory.Wall || IsWallObject(prefab)) ? 0 : prefabRotation;
                    float scaleY = prefab.transform.localScale.y * GetEffectiveScale(currentCategory);
                    float autoY = CalculateAutoYOffset(sr.sprite, scaleY, rotationForOffset);
                    finalOffset.y += autoY;
                }
            }
            return finalOffset;
        }

        private float GetRotatedLocalVisibleBottom(Sprite sprite, int rotationSteps)
        {
            if (sprite == null) return 0f;
            SpriteVisibleBounds bounds = GetSpriteVisibleBounds(sprite);
            int r = (rotationSteps % 4 + 4) % 4;
            if (r == 0) return bounds.minY;
            if (r == 1) return bounds.minX;
            if (r == 2) return -bounds.maxY;
            return -bounds.maxX;
        }

        private float CalculateAutoYOffset(Sprite sprite, float worldScaleY, int rotationSteps)
        {
            if (sprite == null) return 0f;

            float cellHeight = 0.47f; // Default fallback for typical isometric ratio
            Grid grid = null;
            if (targetTilemap != null)
            {
                cellHeight = targetTilemap.cellSize.y;
                grid = targetTilemap.layoutGrid;
            }
            if (grid == null)
            {
                grid = GameObject.FindObjectOfType<Grid>();
            }
            if (grid != null && targetTilemap == null)
            {
                cellHeight = grid.cellSize.y;
            }

            if (grid != null && currentPaintMode == PaintMode.Isometric)
            {
                cellHeight *= Mathf.Abs(grid.transform.lossyScale.y);
            }

            float localVisibleBottom = GetRotatedLocalVisibleBottom(sprite, rotationSteps);

            // Align visible bottom to the bottom vertex of the cell (-cellHeight / 2)
            float targetLocalY = -cellHeight / 2f;
            float currentLocalY = localVisibleBottom * worldScaleY;
            return targetLocalY - currentLocalY;
        }

        private SpriteVisibleBounds GetSpriteVisibleBounds(Sprite sprite)
        {
            if (sprite == null) return new SpriteVisibleBounds { minX = 0f, maxX = 0f, minY = 0f, maxY = 0f };

            if (visibleBoundsCache.TryGetValue(sprite, out var bounds))
            {
                return bounds;
            }

            // Default fallback is sprite.bounds
            SpriteVisibleBounds result = new SpriteVisibleBounds
            {
                minX = sprite.bounds.min.x,
                maxX = sprite.bounds.max.x,
                minY = sprite.bounds.min.y,
                maxY = sprite.bounds.max.y
            };

            Texture2D texture = sprite.texture;
            if (texture == null) return result;

            string path = AssetDatabase.GetAssetPath(texture);
            if (string.IsNullOrEmpty(path)) return result;

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) return result;

            bool wasReadable = importer.isReadable;
            if (!wasReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }

            try
            {
                Rect rect = sprite.rect;
                int minX = (int)rect.width;
                int maxX = 0;
                int minY = (int)rect.height;
                int maxY = 0;
                bool found = false;

                for (int y = 0; y < (int)rect.height; y++)
                {
                    for (int x = 0; x < (int)rect.width; x++)
                    {
                        Color pixel = texture.GetPixel((int)rect.x + x, (int)rect.y + y);
                        if (pixel.a > 0.05f)
                        {
                            if (x < minX) minX = x;
                            if (x > maxX) maxX = x;
                            if (y < minY) minY = y;
                            if (y > maxY) maxY = y;
                            found = true;
                        }
                    }
                }

                if (found)
                {
                    float unitPerPixelX = sprite.bounds.size.x / rect.width;
                    float unitPerPixelY = sprite.bounds.size.y / rect.height;

                    result.minX = sprite.bounds.min.x + minX * unitPerPixelX;
                    result.maxX = sprite.bounds.min.x + maxX * unitPerPixelX;
                    result.minY = sprite.bounds.min.y + minY * unitPerPixelY;
                    result.maxY = sprite.bounds.min.y + maxY * unitPerPixelY;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[World Painter] Could not read sprite pixels: {ex.Message}");
            }
            finally
            {
                if (!wasReadable)
                {
                    importer.isReadable = false;
                    importer.SaveAndReimport();
                }
            }

            visibleBoundsCache[sprite] = result;
            return result;
        }

        private GameObject GetEraseTargetObject(Vector3Int cellPos, Vector3 snapPos)
        {
            if (currentCategory == PaletteCategory.Floor) return null;

            Transform parent = GetTargetParent();
            if (parent == null) return null;

            // 1. Try PickGameObject first
            if (Event.current != null)
            {
                try
                {
                    GameObject picked = HandleUtility.PickGameObject(Event.current.mousePosition, false);
                    GameObject placedParent = FindPlacedParent(picked, parent);
                    if (placedParent != null)
                    {
                        return placedParent;
                    }
                }
                catch
                {
                    // Ignore event exceptions and proceed to fallback
                }
            }

            // 2. Fallback: check close distance or cell matching
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null || child.gameObject == null) continue;
                if (child.GetComponent<Tilemap>() != null) continue;
                if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

                Vector3Int childCell;
                Vector3 childSnap;
                GetCellAndSnapPos(child.position, out childCell, out childSnap);

                if (childCell == cellPos || Vector3.Distance(child.position, snapPos) < 0.4f)
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private void DrawTargetObjectOutline(GameObject go, Color color)
        {
            if (go == null) return;

            var box = go.GetComponentInChildren<BoxCollider2D>(true);
            var sr = go.GetComponentInChildren<SpriteRenderer>(true);

            Vector2 localSize = Vector2.one;
            Vector2 localOffset = Vector2.zero;
            float scaleY = go.transform.lossyScale.y;
            float spriteHeight = 1.0f;

            if (sr != null && sr.sprite != null)
            {
                spriteHeight = sr.sprite.bounds.size.y;
            }

            if (box != null)
            {
                localSize = box.size;
                localOffset = box.offset;
            }
            else if (sr != null && sr.sprite != null)
            {
                // Passable or no collider, draw thin base outline
                float localVisibleBottom = GetSpriteVisibleBounds(sr.sprite).minY;
                localSize = new Vector2(sr.sprite.bounds.size.x * 0.8f, 0.1f);
                localOffset = new Vector2(0f, localVisibleBottom + 0.05f);
            }

            // Local corners of collider
            float halfX = localSize.x / 2f;
            float halfY = localSize.y / 2f;
            Vector3 localC0 = localOffset + new Vector2(-halfX, -halfY);
            Vector3 localC1 = localOffset + new Vector2(halfX, -halfY);
            Vector3 localC2 = localOffset + new Vector2(halfX, halfY);
            Vector3 localC3 = localOffset + new Vector2(-halfX, halfY);

            // Transform to world space
            Transform trans = box != null ? box.transform : (sr != null ? sr.transform : go.transform);
            Vector3 b0 = trans.TransformPoint(localC0);
            Vector3 b1 = trans.TransformPoint(localC1);
            Vector3 b2 = trans.TransformPoint(localC2);
            Vector3 b3 = trans.TransformPoint(localC3);

            // Project upwards by sprite height
            float h = spriteHeight * scaleY;
            Vector3 offset = trans.up * h;
            Vector3 t0 = b0 + offset;
            Vector3 t1 = b1 + offset;
            Vector3 t2 = b2 + offset;
            Vector3 t3 = b3 + offset;

            Handles.color = color;

            // Draw base rectangle (dotted)
            Handles.DrawDottedLine(b0, b1, 2f);
            Handles.DrawDottedLine(b1, b2, 2f);
            Handles.DrawDottedLine(b2, b3, 2f);
            Handles.DrawDottedLine(b3, b0, 2f);

            // Draw top rectangle (solid)
            Handles.DrawAAPolyLine(3f, t0, t1, t2, t3, t0);

            // Draw vertical pillars
            Handles.DrawLine(b0, t0);
            Handles.DrawLine(b1, t1);
            Handles.DrawLine(b2, t2);
            Handles.DrawLine(b3, t3);

            // Side fills
            Color sideFill = color;
            sideFill.a = 0.05f;
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b1, t1, t0 }, sideFill, Color.clear);
            Handles.DrawSolidRectangleWithOutline(new[] { b0, b3, t3, t0 }, sideFill, Color.clear);
        }

        private void DrawActiveCollisionGizmo(Vector3 snapPos)
        {
            if (currentTool == ToolMode.Paint && selectedPrefab != null && currentCategory != PaletteCategory.Floor)
            {
                Vector3 finalOffset = GetPlacementOffset(selectedPrefab);
                float effectiveScale = GetEffectiveScale(currentCategory);
                CollisionResolver.ResolvedCollider resolved = ResolveCollisionPreview(selectedPrefab, currentCategory, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, prefabRotation);
                DrawResolvedColliderRect(snapPos + finalOffset, prefabRotation, effectiveScale, resolved);
            }

            GameObject selectedInstance = GetSelectedEditableInstance();
            if (selectedInstance != null)
            {
                var box = selectedInstance.GetComponentInChildren<BoxCollider2D>(true);
                if (box != null)
                {
                    CollisionMode mode = GuessCategory(selectedInstance) == PaletteCategory.Wall ? CollisionMode.WallBlock : CollisionMode.Custom;
                    DrawBoxColliderRect(box, mode);
                }
            }

            if (cachedHoveredObject != null)
            {
                var box = cachedHoveredObject.GetComponentInChildren<BoxCollider2D>(true);
                if (box != null)
                {
                    DrawBoxColliderRect(box, GuessCategory(cachedHoveredObject) == PaletteCategory.Wall ? CollisionMode.WallBlock : CollisionMode.Custom);
                }
            }
        }

        private void DrawResolvedColliderRect(Vector3 center, int rotationSteps, float scale, CollisionResolver.ResolvedCollider resolved)
        {
            if (resolved.effectiveMode == CollisionMode.Passable && resolved.worldSize == Vector2.zero) return;
            float safeScale = scale != 0f ? scale : 1f;
            Vector2 localSize = resolved.worldSize / safeScale;
            Vector2 localOffset = resolved.worldOffset / safeScale;
            Quaternion rot = Quaternion.Euler(0f, 0f, rotationSteps * 90f);
            DrawColliderQuad(
                center + rot * Vector3.Scale(localOffset + new Vector2(-localSize.x / 2f, -localSize.y / 2f), new Vector3(safeScale, safeScale, 1f)),
                center + rot * Vector3.Scale(localOffset + new Vector2(localSize.x / 2f, -localSize.y / 2f), new Vector3(safeScale, safeScale, 1f)),
                center + rot * Vector3.Scale(localOffset + new Vector2(localSize.x / 2f, localSize.y / 2f), new Vector3(safeScale, safeScale, 1f)),
                center + rot * Vector3.Scale(localOffset + new Vector2(-localSize.x / 2f, localSize.y / 2f), new Vector3(safeScale, safeScale, 1f)),
                resolved.effectiveMode);
        }

        private void DrawBoxColliderRect(BoxCollider2D box, CollisionMode mode)
        {
            Vector2 size = box.size;
            Vector2 offset = box.offset;
            DrawColliderQuad(
                box.transform.TransformPoint(offset + new Vector2(-size.x / 2f, -size.y / 2f)),
                box.transform.TransformPoint(offset + new Vector2(size.x / 2f, -size.y / 2f)),
                box.transform.TransformPoint(offset + new Vector2(size.x / 2f, size.y / 2f)),
                box.transform.TransformPoint(offset + new Vector2(-size.x / 2f, size.y / 2f)),
                mode);
        }

        private void DrawColliderQuad(Vector3 c0, Vector3 c1, Vector3 c2, Vector3 c3, CollisionMode mode)
        {
            Color color = GetCollisionColor(mode);
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(4f, c0, c1, c2, c3, c0);
            Handles.color = color;
            Handles.DrawAAPolyLine(2f, c0, c1, c2, c3, c0);
            if (mode == CollisionMode.WallBlock || mode == CollisionMode.Custom)
            {
                Handles.DrawDottedLine(c0, c2, 3f);
                Handles.DrawDottedLine(c1, c3, 3f);
            }
        }

        private Color GetCollisionColor(CollisionMode mode)
        {
            switch (mode)
            {
                case CollisionMode.WallBlock: return new Color(0.84f, 0.29f, 0.29f, 0.9f);
                case CollisionMode.SmallFootprint: return new Color(0.9f, 0.77f, 0.29f, 0.7f);
                case CollisionMode.FullFootprint: return new Color(0.9f, 0.54f, 0.23f, 0.8f);
                case CollisionMode.Custom: return new Color(0.75f, 0.31f, 0.82f, 0.8f);
                default: return new Color(0.23f, 0.82f, 0.48f, 0.6f);
            }
        }

        private Transform GetTargetParent()
        {
            Transform peeked = PeekTargetParent();
            if (peeked != null)
            {
                return peeked;
            }

            if (targetTilemap == null)
            {
                return null;
            }

            GameObject propsRoot = GameObject.Find("Props_Root");
            if (propsRoot == null)
            {
                propsRoot = new GameObject("Props_Root");
                propsRoot.transform.position = Vector3.zero;
                propsRoot.transform.rotation = Quaternion.identity;
                propsRoot.transform.localScale = Vector3.one;
                Undo.RegisterCreatedObjectUndo(propsRoot, "Create Props_Root");
            }
            return propsRoot.transform;
        }

        private Transform PeekTargetParent()
        {
            if (targetParent != null && !IsGridOrTilemapTransform(targetParent))
            {
                return targetParent;
            }
            if (targetTilemap == null) return null;

            Transform tilemapParent = targetTilemap.transform.parent;
            if (tilemapParent == null) return targetTilemap.transform;

            if (IsGridOrTilemapTransform(tilemapParent))
            {
                GameObject existing = GameObject.Find("Props_Root");
                return existing != null ? existing.transform : null;
            }

            return tilemapParent;
        }

        private static bool IsGridOrTilemapTransform(Transform candidate)
        {
            if (candidate == null) return false;
            string lowerName = candidate.name.ToLowerInvariant();
            return candidate.GetComponent<Grid>() != null ||
                   candidate.GetComponent<Tilemap>() != null ||
                   lowerName.Contains("grid") ||
                   lowerName.Contains("tilemap");
        }

        private void GetRecursiveChildren(Transform current, List<Transform> result)
        {
            foreach (Transform child in current)
            {
                if (child == null) continue;
                result.Add(child);
                GetRecursiveChildren(child, result);
            }
        }

        private Transform GetOrCreateGroupParent(Transform root, string prefabName, PaletteCategory category)
        {
            if (root == null) return null;
            string groupName = GroupClassifier.Classify(prefabName, category);
            
            Transform groupTransform = root.Find(groupName);
            if (groupTransform == null)
            {
                GameObject groupGo = new GameObject(groupName);
                groupGo.transform.SetParent(root, false);
                groupGo.transform.localPosition = Vector3.zero;
                groupGo.transform.localRotation = Quaternion.identity;
                groupGo.transform.localScale = Vector3.one;
                Undo.RegisterCreatedObjectUndo(groupGo, "Create Group " + groupName);
                groupTransform = groupGo.transform;
            }
            return groupTransform;
        }

        private void DrawSeparator()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color(0.3f, 0.3f, 0.3f, 1f));
        }

        private void RefreshSavedMapsList()
        {
            savedMapPaths.Clear();
            string folder = "Assets/RIMA_MapData/UnifiedMaps";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string[] files = Directory.GetFiles(folder, "*.json");
            foreach (var f in files)
            {
                savedMapPaths.Add(f.Replace('\\', '/'));
            }
        }

        private void CreateNewMap()
        {
            if (EditorUtility.DisplayDialog("New Map", "Are you sure you want to clear the canvas and start a new map?", "Yes", "No"))
            {
                ClearSceneCanvas();
                activeMapName = "NewMap";
                activeMapPath = string.Empty;
            }
        }

        private void ClearSceneCanvas()
        {
            if (targetTilemap != null)
            {
                Undo.RegisterCompleteObjectUndo(targetTilemap, "Clear Tilemap");
                targetTilemap.ClearAllTiles();
            }
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                Undo.RegisterCompleteObjectUndo(parent, "Clear Spawned Objects");
                List<GameObject> childrenToDestroy = new List<GameObject>();
                foreach (Transform child in parent)
                {
                    if (child != null && child.gameObject != null && child.GetComponent<Tilemap>() == null)
                    {
                        childrenToDestroy.Add(child.gameObject);
                    }
                }
                foreach (var child in childrenToDestroy)
                {
                    Undo.DestroyObjectImmediate(child);
                }
            }
        }

        private void SaveActiveMapAs()
        {
            string folder = "Assets/RIMA_MapData/UnifiedMaps";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string path = EditorUtility.SaveFilePanelInProject("Save Unified Map", "Map_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"), "json", "Choose where to save map data", folder);
            if (!string.IsNullOrEmpty(path))
            {
                SaveMapData(path);
            }
        }

        private void SaveMapData(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            UnifiedMapSaveData data = new UnifiedMapSaveData();
            data.mapName = Path.GetFileNameWithoutExtension(path);

            // 1. Serialize Tiles
            if (targetTilemap != null)
            {
                BoundsInt bounds = targetTilemap.cellBounds;
                foreach (var pos in bounds.allPositionsWithin)
                {
                    TileBase tile = targetTilemap.GetTile(pos);
                    if (tile != null)
                    {
                        string tilePath = AssetDatabase.GetAssetPath(tile);
                        data.tiles.Add(new UnifiedTileData
                        {
                            position = pos,
                            tileAssetPath = tilePath
                        });
                    }
                }
            }

            // 2. Serialize Objects
            Transform parent = GetTargetParent();
            if (parent != null)
            {
                List<Transform> allChildren = new List<Transform>();
                GetRecursiveChildren(parent, allChildren);
                foreach (Transform child in allChildren)
                {
                    if (child == null || child.gameObject == null) continue;
                    if (child.GetComponent<Tilemap>() != null) continue;
                    if (child.GetComponent<SpriteRenderer>() == null) continue; // Skip group containers

                    // Try to get original prefab asset path
                    GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                    string prefabPath = prefabAsset != null ? AssetDatabase.GetAssetPath(prefabAsset) : string.Empty;
                    
                    if (string.IsNullOrEmpty(prefabPath))
                    {
                        // Fallback: search by name in scanned prefabs
                        string cName = child.name.Replace("(Clone)", "").Trim();
                        var scan = wallPrefabs.Concat(propPrefabs).Concat(mobPrefabs).FirstOrDefault(x => x.prefab != null && x.prefab.name == cName);
                        if (scan != null)
                        {
                            prefabPath = scan.assetPath;
                        }
                    }

                    if (!string.IsNullOrEmpty(prefabPath))
                    {
                        var col = child.GetComponent<BoxCollider2D>();
                        CollisionMode mode = CollisionMode.Auto;
                        Vector2 colSize = Vector2.one;
                        Vector2 colOffset = Vector2.zero;

                        if (col != null)
                        {
                            colSize = col.size;
                            colOffset = col.offset;
                            mode = CollisionMode.Custom;
                        }

                        data.objects.Add(new UnifiedObjectData
                        {
                            prefabAssetPath = prefabPath,
                            position = child.localPosition,
                            rotation = child.localEulerAngles,
                            scale = child.localScale,
                            collisionMode = mode,
                            colliderSize = colSize,
                            colliderOffset = colOffset,
                            name = child.name
                        });
                    }
                }
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
            
            activeMapPath = path;
            activeMapName = data.mapName;
            
            RefreshSavedMapsList();
            Debug.Log($"[World Painter] Saved map to: {path}");
        }

        private void LoadMapData(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Debug.LogError($"[World Painter] Map file not found: {path}");
                return;
            }

            string json = File.ReadAllText(path);
            UnifiedMapSaveData data = JsonUtility.FromJson<UnifiedMapSaveData>(json);
            if (data == null)
            {
                Debug.LogError($"[World Painter] Failed to parse map data from {path}");
                return;
            }

            ClearSceneCanvas();

            // 1. Deserialize Tiles
            if (targetTilemap != null && data.tiles != null)
            {
                foreach (var tileData in data.tiles)
                {
                    TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(tileData.tileAssetPath);
                    if (tile != null)
                    {
                        targetTilemap.SetTile(tileData.position, tile);
                    }
                }
            }

            // 2. Deserialize Objects
            Transform parent = GetTargetParent();
            if (parent != null && data.objects != null)
            {
                foreach (var objData in data.objects)
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objData.prefabAssetPath);
                    if (prefab != null)
                    {
                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                        go.transform.localPosition = objData.position;
                        go.transform.localEulerAngles = objData.rotation;
                        go.transform.localScale = objData.scale;
                        go.name = objData.name;

                        PaletteCategory loadedCategory = IsWallObject(prefab) ? PaletteCategory.Wall :
                            (prefab.name.ToLowerInvariant().StartsWith("enemy_") || prefab.name.ToLowerInvariant().StartsWith("mob_") ? PaletteCategory.Mob : PaletteCategory.Prop);
                        int loadedRotation = Mathf.RoundToInt(objData.rotation.z / 90f);
                        CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(prefab, loadedCategory, objData.collisionMode, objData.colliderSize, objData.colliderOffset, objData.scale.y, loadedRotation, collisionRules);
                        ConfigureCollider(go, resolved.effectiveMode, objData.scale.y);
                        ApplySorting(go, resolved);
                        
                        if (objData.collisionMode == CollisionMode.Custom)
                        {
                            var box = go.GetComponent<BoxCollider2D>();
                            if (box == null) box = go.AddComponent<BoxCollider2D>();
                            box.size = objData.colliderSize;
                            box.offset = objData.colliderOffset;
                        }

                        Undo.RegisterCreatedObjectUndo(go, "Load Map Object");
                    }
                }
            }

            activeMapPath = path;
            activeMapName = data.mapName;
            Debug.Log($"[World Painter] Loaded map: {path}");
        }

        private GameObject FindWallAtCell(Vector3Int cellPos)
        {
            Transform parent = GetTargetParent();
            if (parent == null || targetTilemap == null) return null;
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null) continue;
                if (IsWallObject(child.gameObject))
                {
                    // Ignore container folders which don't have visual renderers directly on them
                    if (child.GetComponent<SpriteRenderer>() == null && child.GetComponent<Renderer>() == null)
                    {
                        continue;
                    }
                    Vector3Int childCell = targetTilemap.WorldToCell(child.position);
                    if (childCell == cellPos)
                    {
                        return child.gameObject;
                    }
                }
            }
            return null;
        }

        private bool TryResolveTopDownWallPrefab(ref GameObject prefab, int rotationSteps, bool showWarning)
        {
            if (currentPaintMode != PaintMode.TopDown || prefab == null || !IsTopDownNsDirection(rotationSteps))
            {
                return true;
            }

            GameObject faceNS = FindTopDownFaceNSPrefab(prefab);
            if (faceNS != null)
            {
                prefab = faceNS;
                return true;
            }

            const string message = "TopDown modunda face_NS gerekli";
            Debug.LogWarning("[World Painter] " + message);
            if (showWarning)
            {
                EditorUtility.DisplayDialog("World Painter", message, "OK");
            }
            return false;
        }

        private bool IsTopDownNsDirection(int rotationSteps)
        {
            int r = (rotationSteps % 4 + 4) % 4;
            return r == 1 || r == 3;
        }

        private GameObject FindTopDownFaceNSPrefab(GameObject reference)
        {
            if (wallPrefabs == null || wallPrefabs.Count == 0) return null;

            string familyKey = GetWallFamilyKey(reference != null ? reference.name : string.Empty);
            IEnumerable<GameObject> candidates = wallPrefabs
                .Where(x => x != null && x.prefab != null)
                .Select(x => x.prefab);
            if (!string.IsNullOrEmpty(familyKey))
            {
                candidates = candidates.Where(x => GetWallFamilyKey(x.name) == familyKey);
            }

            return candidates.FirstOrDefault(IsFaceNSPrefab);
        }

        private static bool IsFaceNSPrefab(GameObject prefab)
        {
            if (prefab == null) return false;
            string lower = prefab.name.ToLowerInvariant();
            return lower.Contains("face_ns") || lower.Contains("face-ns") || lower.Contains("facens") || lower.Contains("face ns");
        }

        private void PaintWallWithConnections(Vector3Int cellPos, Vector3 snapPos, GameObject selectedPrefab)
        {
            Transform baseParent = GetTargetParent();
            if (baseParent == null || targetTilemap == null) return;
            if (!TryResolveTopDownWallPrefab(ref selectedPrefab, prefabRotation, true))
            {
                return;
            }
            selectedPrefab = GetRandomWallVariantFromGroup(selectedPrefab);
            Transform parent = GetOrCreateGroupParent(baseParent, selectedPrefab.name, PaletteCategory.Wall);
            if (parent == null) return;

            GameObject existingWall = FindWallAtCell(cellPos);
            if (existingWall == null)
            {
                existingWall = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
                if (parent != null)
                {
                    existingWall.transform.SetParent(parent, false);
                }
                existingWall.transform.position = snapPos + GetPlacementOffset(selectedPrefab);
                
                bool useIsometricWallLogic = (currentPaintMode == PaintMode.Isometric);
                int placementRotationSteps = currentPaintMode == PaintMode.TopDown && IsTopDownNsDirection(prefabRotation) ? 0 : prefabRotation;
                float effectiveRotationDegrees = useIsometricWallLogic ? 0f : placementRotationSteps * 90f;
                float effectiveScale = GetEffectiveScale(PaletteCategory.Wall);
                existingWall.transform.rotation = Quaternion.Euler(0f, 0f, effectiveRotationDegrees);
                
                var srs = existingWall.GetComponentsInChildren<SpriteRenderer>(true);
                bool shouldFlipX = useIsometricWallLogic && (prefabRotation == 1 || prefabRotation == 3);
                foreach (var sr in srs)
                {
                    sr.flipX = shouldFlipX;
                }

                if (parent != null)
                {
                    existingWall.transform.localScale = ComputeCompensatedLocalScale(
                        parent,
                        selectedPrefab.transform.localScale * effectiveScale,
                        effectiveRotationDegrees
                    );
                }
                else
                {
                    existingWall.transform.localScale = selectedPrefab.transform.localScale * effectiveScale;
                }

                CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(selectedPrefab, PaletteCategory.Wall, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, useIsometricWallLogic ? 0 : placementRotationSteps, collisionRules);
                ConfigureCollider(existingWall, resolved.effectiveMode, effectiveScale);
                ApplySorting(existingWall, resolved);

                Undo.RegisterCreatedObjectUndo(existingWall, "Place Wall");
            }

            UpdateWallConnectionsAt(cellPos);
            UpdateWallConnectionsAt(cellPos + new Vector3Int(1, 0, 0));  // NE
            UpdateWallConnectionsAt(cellPos + new Vector3Int(-1, 0, 0)); // SW
            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, 1, 0));  // NW
            UpdateWallConnectionsAt(cellPos + new Vector3Int(0, -1, 0)); // SE

            if (parent != null)
            {
                EditorUtility.SetDirty(parent.gameObject);
            }
        }

        private void UpdateWallConnectionsAt(Vector3Int pos)
        {
            if (targetTilemap == null) return;
            GameObject wallObj = FindWallAtCell(pos);
            if (wallObj == null) return;

            if (wallPrefabs == null) return;
            List<ScanResult> connectionWallPrefabs = wallPrefabs
                .Where(x => x != null && x.prefab != null && !x.prefab.name.ToLowerInvariant().Contains("arch"))
                .ToList();
            if (connectionWallPrefabs.Count == 0) return;

            bool hasNE = FindWallAtCell(pos + new Vector3Int(1, 0, 0)) != null;
            bool hasSW = FindWallAtCell(pos + new Vector3Int(-1, 0, 0)) != null;
            bool hasNW = FindWallAtCell(pos + new Vector3Int(0, 1, 0)) != null;
            bool hasSE = FindWallAtCell(pos + new Vector3Int(0, -1, 0)) != null;

            GameObject newPrefab = null;
            int rotationSteps = 0;
            bool shouldFlipX = false;

            GameObject pNW_SE = connectionWallPrefabs[0].prefab;
            GameObject pNE_SW = connectionWallPrefabs.Count > 1 ? connectionWallPrefabs[1].prefab : pNW_SE;
            GameObject pCorner = connectionWallPrefabs.Count > 2 ? connectionWallPrefabs[2].prefab : pNW_SE;
            GameObject pCrack = connectionWallPrefabs.Count > 3 ? connectionWallPrefabs[3].prefab : pNW_SE;
            GameObject prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(wallObj);
            ApplyWallConnectionFamily(prefabSource != null ? prefabSource : wallObj, ref pNW_SE, ref pNE_SW, ref pCorner, ref pCrack);
            bool isSingleFaceFallback = (pNW_SE == pNE_SW);
            GameObject pFaceNS = FindTopDownFaceNSPrefab(prefabSource != null ? prefabSource : wallObj);

            if (currentPaintMode == PaintMode.Isometric)
            {
                if ((hasNE || hasSW) && !hasNW && !hasSE)
                {
                    newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f && pCrack != pNE_SW) ? pCrack : pNE_SW;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if ((hasNW || hasSE) && !hasNE && !hasSW)
                {
                    newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f && pCrack != pNW_SE) ? pCrack : pNW_SE;
                    rotationSteps = 0;
                    shouldFlipX = isSingleFaceFallback;
                }
                else if (hasNW && hasNE && !hasSE && !hasSW)
                {
                    newPrefab = pCorner;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if (hasNE && hasSE && !hasNW && !hasSW)
                {
                    newPrefab = pCorner;
                    rotationSteps = 0;
                    shouldFlipX = true;
                }
                else if (hasSE && hasSW && !hasNW && !hasNE)
                {
                    newPrefab = pCorner;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if (hasSW && hasNW && !hasNE && !hasSE)
                {
                    newPrefab = pCorner;
                    rotationSteps = 0;
                    shouldFlipX = true;
                }
                else if (hasNE || hasSW || hasNW || hasSE)
                {
                    newPrefab = pCrack;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else
                {
                    newPrefab = pNW_SE;
                    rotationSteps = 0;
                    shouldFlipX = (prefabRotation == 1 || prefabRotation == 3);
                }
            }
            else // TopDown (Orthogonal) Mode
            {
                if ((hasNE || hasSW) && !hasNW && !hasSE)
                {
                    newPrefab = (randomizeWallCracks && UnityEngine.Random.value < 0.15f && pCrack != pNW_SE) ? pCrack : pNW_SE;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if ((hasNW || hasSE) && !hasNE && !hasSW)
                {
                    if (pFaceNS == null)
                    {
                        Debug.LogWarning("[World Painter] TopDown modunda face_NS gerekli");
                        return;
                    }
                    newPrefab = pFaceNS;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if (hasNW && hasNE && !hasSE && !hasSW)
                {
                    newPrefab = pCorner;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else if (hasNE && hasSE && !hasNW && !hasSW)
                {
                    newPrefab = pCorner;
                    rotationSteps = 1; // 90° Z rotation
                    shouldFlipX = false;
                }
                else if (hasSE && hasSW && !hasNW && !hasNE)
                {
                    newPrefab = pCorner;
                    rotationSteps = 2; // 180° Z rotation
                    shouldFlipX = false;
                }
                else if (hasSW && hasNW && !hasNE && !hasSE)
                {
                    newPrefab = pCorner;
                    rotationSteps = 3; // 270° Z rotation
                    shouldFlipX = false;
                }
                else if (hasNE || hasSW || hasNW || hasSE)
                {
                    newPrefab = pCrack;
                    rotationSteps = 0;
                    shouldFlipX = false;
                }
                else
                {
                    if (IsTopDownNsDirection(prefabRotation))
                    {
                        if (pFaceNS == null)
                        {
                            Debug.LogWarning("[World Painter] TopDown modunda face_NS gerekli");
                            return;
                        }
                        newPrefab = pFaceNS;
                        rotationSteps = 0;
                    }
                    else
                    {
                        newPrefab = pNW_SE;
                        rotationSteps = prefabRotation;
                    }
                    shouldFlipX = false;
                }
            }
            newPrefab = GetRandomWallVariantFromGroup(newPrefab);

            var currentSRs = wallObj.GetComponentsInChildren<SpriteRenderer>(true);
            bool currentFlipX = currentSRs.Length > 0 && currentSRs[0].flipX;
            bool isSamePrefab = (prefabSource != null && prefabSource == newPrefab);
            int currentRotationSteps = Mathf.RoundToInt(wallObj.transform.localEulerAngles.z / 90f);

            if (!isSamePrefab || currentRotationSteps != rotationSteps || currentFlipX != shouldFlipX)
            {
                Transform parent = wallObj.transform.parent;

                GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(newPrefab);
                if (parent != null)
                {
                    newWall.transform.SetParent(parent, false);
                }
                newWall.transform.position = targetTilemap.GetCellCenterWorld(pos) + GetPlacementOffset(newPrefab);
                
                bool useIsometricWallLogic = (currentPaintMode == PaintMode.Isometric);
                float effectiveRotation = useIsometricWallLogic ? 0f : rotationSteps * 90f;
                float effectiveScale = GetEffectiveScale(PaletteCategory.Wall);
                newWall.transform.rotation = Quaternion.Euler(0f, 0f, effectiveRotation);
                
                var srs = newWall.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var sr in srs)
                {
                    sr.flipX = shouldFlipX;
                }

                if (parent != null)
                {
                    newWall.transform.localScale = ComputeCompensatedLocalScale(
                        parent,
                        newPrefab.transform.localScale * effectiveScale,
                        effectiveRotation
                    );
                }
                else
                {
                    newWall.transform.localScale = newPrefab.transform.localScale * effectiveScale;
                }

                CollisionResolver.ResolvedCollider resolved = CollisionResolver.Resolve(newPrefab, PaletteCategory.Wall, customCollisionMode, customColliderSize, customColliderOffset, effectiveScale, useIsometricWallLogic ? 0 : rotationSteps, collisionRules);
                ConfigureCollider(newWall, resolved.effectiveMode, effectiveScale);
                ApplySorting(newWall, resolved);

                Undo.RegisterCreatedObjectUndo(newWall, "Auto-Connect Wall");
                Undo.DestroyObjectImmediate(wallObj);
            }
        }

        private void ApplyWallConnectionFamily(GameObject source, ref GameObject pNW_SE, ref GameObject pNE_SW, ref GameObject pCorner, ref GameObject pCrack)
        {
            string familyKey = GetWallFamilyKey(source != null ? source.name : string.Empty);
            if (string.IsNullOrEmpty(familyKey) || wallPrefabs == null) return;

            List<GameObject> family = wallPrefabs
                .Where(x => x != null && x.prefab != null && GetWallFamilyKey(x.prefab.name) == familyKey && !x.prefab.name.ToLowerInvariant().Contains("arch"))
                .Select(x => x.prefab)
                .Distinct()
                .ToList();
            if (family.Count == 0) return;

            GameObject face = FindWallPrefabByKeyword(family, "face") ?? FindWallPrefabByKeyword(family, "wall") ?? family[0];
            GameObject corner = FindWallPrefabByKeyword(family, "corner") ?? face;
            GameObject damaged = FindWallPrefabByKeyword(family, "crack") ?? face;

            pNW_SE = face;
            pNE_SW = face;
            pCorner = corner;
            pCrack = damaged;
        }

        private static string GetWallFamilyKey(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            string lower = name.ToLowerInvariant();
            int wallIndex = lower.IndexOf("wall", StringComparison.Ordinal);
            if (wallIndex < 0) return string.Empty;
            return lower.Substring(0, wallIndex + "wall".Length);
        }

        private static GameObject FindWallPrefabByKeyword(List<GameObject> prefabs, string keyword)
        {
            if (prefabs == null || string.IsNullOrEmpty(keyword)) return null;
            return prefabs.FirstOrDefault(x => x != null && x.name.ToLowerInvariant().Contains(keyword));
        }

        private void RebuildAllWallConnections()
        {
            Transform parent = GetTargetParent();
            if (parent == null || targetTilemap == null) return;
            
            List<Vector3Int> wallCells = new List<Vector3Int>();
            List<Transform> allChildren = new List<Transform>();
            GetRecursiveChildren(parent, allChildren);
            foreach (Transform child in allChildren)
            {
                if (child == null) continue;
                if (IsWallObject(child.gameObject))
                {
                    Vector3Int cell = targetTilemap.WorldToCell(child.position);
                    if (!wallCells.Contains(cell))
                    {
                        wallCells.Add(cell);
                    }
                }
            }

            if (wallCells.Count == 0) return;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("Rebuild All Wall Connections");

            foreach (var cell in wallCells)
            {
                UpdateWallConnectionsAt(cell);
            }

            EditorUtility.SetDirty(parent.gameObject);
            Debug.Log($"[World Painter] Rebuilt connections for {wallCells.Count} walls.");
        }

        private void AttachIsoSorterToAllPlacedObjects()
        {
            Transform parent = GetTargetParent();
            if (parent == null)
            {
                Debug.LogWarning("[World Painter] Target parent is not assigned.");
                return;
            }

            int count = 0;
            int removedWallCount = 0;
            var renderers = parent.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var sr in renderers)
            {
                if (sr == null) continue;
                if (sr.GetComponentInParent<Tilemap>() != null) continue;

                GameObject go = sr.gameObject;
                if (IsWallObject(go) || (go.transform.parent != null && IsWallObject(go.transform.parent.gameObject)))
                {
                    if (RemoveIsoSorter(go))
                    {
                        removedWallCount++;
                    }
                    continue;
                }
                var sorter = go.GetComponent<RIMA.IsoSorter>();
                if (sorter == null)
                {
                    sorter = go.AddComponent<RIMA.IsoSorter>();
                    EditorUtility.SetDirty(go);
                    count++;
                }
            }

            Debug.Log($"[World Painter] Attached IsoSorter component to {count} GameObjects with SpriteRenderer. Removed {removedWallCount} wall IsoSorter components.");
        }
    }
}
#endif
