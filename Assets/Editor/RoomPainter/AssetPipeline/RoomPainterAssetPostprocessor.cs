using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public sealed class RoomPainterAssetPostprocessor : AssetPostprocessor
    {
        public const string MetadataRoot = "Assets/RoomPainter/AssetMetadata";
        private const string LayerDataRoot = "Assets/RoomPainter/LayerData";

        private static readonly string[] ImportRoots =
        {
            "Assets/Sprites",
            "Assets/Prefabs"
        };

        private static readonly HashSet<string> PendingImports = new HashSet<string>();
        private static readonly HashSet<string> PendingSpriteImporterPaths = new HashSet<string>();
        private static readonly HashSet<string> KnownAssetPaths = new HashSet<string>();
        private static bool isFlushing;
        private static bool delayQueued;
        private static bool spriteImporterDelayQueued;
        private static bool baselineCaptured;

        public static void InitializeWatcher()
        {
            EditorApplication.projectChanged -= QueueProjectChangedScan;
            EditorApplication.projectChanged += QueueProjectChangedScan;
        }

        public override uint GetVersion()
        {
            return 2;
        }

        private void OnPostprocessTexture(Texture2D texture)
        {
            QueueSpriteImporter(assetPath);
            QueueImport(assetPath, false);
            QueueFlush();
        }

        private void OnPostprocessPrefab(GameObject root)
        {
            QueueImport(assetPath, false);
            QueueFlush();
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            HandlePostprocess(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandlePostprocess(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
        }

        private static void HandlePostprocess(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            QueueImports(importedAssets);
            QueueImports(movedAssets);
            PublishDeletes(deletedAssets);
            PublishDeletes(movedFromAssetPaths);
            QueueFlush();
        }

        public static RoomPainterAsset LoadMetadataForAssetPath(string assetPath)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<RoomPainterAsset>(GetMetadataPath(guid));
        }

        public static RoomPainterAsset EnsureMetadataForAssetPath(string assetPath)
        {
            string normalized = Normalize(assetPath);
            if (!ShouldProcess(normalized))
            {
                return LoadMetadataForAssetPath(normalized);
            }

            RoomPainterAsset existing = LoadMetadataForAssetPath(normalized);
            if (existing != null)
            {
                return existing;
            }

            bool wasFlushing = isFlushing;
            isFlushing = true;
            try
            {
                EnsureFolder("Assets", "RoomPainter");
                EnsureFolder("Assets/RoomPainter", "AssetMetadata");
                EnsureFolder("Assets/RoomPainter", "LayerData");
                CreateOrUpdateMetadata(normalized);
                AssetDatabase.SaveAssets();
            }
            finally
            {
                isFlushing = wasFlushing;
            }

            return LoadMetadataForAssetPath(normalized);
        }

        public static int BackfillMetadataUnderFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
            {
                return 0;
            }

            string[] folders = { folderPath };
            string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", folders);
            string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D", folders);
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", folders);

            int created = 0;
            try
            {
                AssetDatabase.StartAssetEditing();
                created += EnsureForGuids(spriteGuids);
                created += EnsureForGuids(textureGuids);
                created += EnsureForGuids(prefabGuids);
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }

            return created;
        }

        private static int EnsureForGuids(string[] guids)
        {
            if (guids == null)
            {
                return 0;
            }

            int created = 0;
            for (int i = 0; i < guids.Length; i++)
            {
                string path = Normalize(AssetDatabase.GUIDToAssetPath(guids[i]));
                if (!ShouldProcess(path))
                {
                    continue;
                }

                if (LoadMetadataForAssetPath(path) != null)
                {
                    continue;
                }

                if (EnsureMetadataForAssetPath(path) != null)
                {
                    created++;
                }
            }

            return created;
        }

        private static void QueueImports(string[] paths)
        {
            if (paths == null)
            {
                return;
            }

            for (int i = 0; i < paths.Length; i++)
            {
                QueueImport(paths[i], false);
            }
        }

        private static void QueueImport(string path, bool skipExistingMetadata)
        {
            string normalized = Normalize(path);
            if (!ShouldProcess(normalized))
            {
                return;
            }

            if (skipExistingMetadata && LoadMetadataForAssetPath(normalized) != null)
            {
                return;
            }

            PendingImports.Add(normalized);
        }

        private static void QueueFlush()
        {
            if (delayQueued)
            {
                return;
            }

            delayQueued = true;
            EditorApplication.delayCall += FlushPendingImports;
        }

        private static void QueueSpriteImporter(string path)
        {
            string normalized = Normalize(path);
            if (!ShouldProcess(normalized) || !IsSpritePath(normalized))
            {
                return;
            }

            PendingSpriteImporterPaths.Add(normalized);
            if (spriteImporterDelayQueued)
            {
                return;
            }

            spriteImporterDelayQueued = true;
            EditorApplication.delayCall += FlushSpriteImporterUpdates;
        }

        private static void QueueProjectChangedScan()
        {
            if (isFlushing)
            {
                return;
            }

            if (!baselineCaptured)
            {
                CaptureKnownAssets();
                baselineCaptured = true;
                return;
            }

            if (PendingImports.Count > 0)
            {
                QueueFlush();
                return;
            }

            int before = PendingImports.Count;
            QueueNewAssetsByType("t:Texture2D");
            QueueNewAssetsByType("t:Sprite");
            QueueNewAssetsByType("t:Prefab");
            if (PendingImports.Count > before)
            {
                QueueFlush();
            }
        }

        private static void CaptureKnownAssets()
        {
            AddKnownAssetsByType("t:Texture2D");
            AddKnownAssetsByType("t:Sprite");
            AddKnownAssetsByType("t:Prefab");
        }

        private static void AddKnownAssetsByType(string filter)
        {
            string[] roots = ValidImportRoots();
            if (roots.Length == 0)
            {
                return;
            }

            string[] guids = AssetDatabase.FindAssets(filter, roots);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = Normalize(AssetDatabase.GUIDToAssetPath(guids[i]));
                if (ShouldProcess(path))
                {
                    KnownAssetPaths.Add(path);
                }
            }
        }

        private static void QueueNewAssetsByType(string filter)
        {
            string[] roots = ValidImportRoots();
            if (roots.Length == 0)
            {
                return;
            }

            string[] guids = AssetDatabase.FindAssets(filter, roots);
            for (int i = 0; i < guids.Length; i++)
            {
                string path = Normalize(AssetDatabase.GUIDToAssetPath(guids[i]));
                if (!ShouldProcess(path) || KnownAssetPaths.Contains(path))
                {
                    continue;
                }

                QueueImport(path, true);
            }
        }

        private static string[] ValidImportRoots()
        {
            List<string> roots = new List<string>();
            for (int i = 0; i < ImportRoots.Length; i++)
            {
                if (AssetDatabase.IsValidFolder(ImportRoots[i]))
                {
                    roots.Add(ImportRoots[i]);
                }
            }

            return roots.ToArray();
        }

        private static void FlushSpriteImporterUpdates()
        {
            spriteImporterDelayQueued = false;

            if (isFlushing || EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                spriteImporterDelayQueued = true;
                EditorApplication.delayCall += FlushSpriteImporterUpdates;
                return;
            }

            if (PendingSpriteImporterPaths.Count == 0)
            {
                return;
            }

            string[] paths = new string[PendingSpriteImporterPaths.Count];
            PendingSpriteImporterPaths.CopyTo(paths);
            PendingSpriteImporterPaths.Clear();

            for (int i = 0; i < paths.Length; i++)
            {
                EnsureSpriteImporter(paths[i]);
                QueueImport(paths[i], false);
            }

            if (PendingImports.Count > 0)
            {
                QueueFlush();
            }
        }

        private static void FlushPendingImports()
        {
            delayQueued = false;

            if (isFlushing || EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                QueueFlush();
                return;
            }

            if (PendingImports.Count == 0)
            {
                return;
            }

            isFlushing = true;
            try
            {
                EnsureFolder("Assets", "RoomPainter");
                EnsureFolder("Assets/RoomPainter", "AssetMetadata");
                EnsureFolder("Assets/RoomPainter", "LayerData");

                string[] paths = new string[PendingImports.Count];
                PendingImports.CopyTo(paths);
                PendingImports.Clear();

                for (int i = 0; i < paths.Length; i++)
                {
                    CreateOrUpdateMetadata(paths[i]);
                }

                AssetDatabase.SaveAssets();
            }
            finally
            {
                isFlushing = false;
            }
        }

        private static void CreateOrUpdateMetadata(string assetPath)
        {
            if (NeedsSpriteImporterUpdate(assetPath))
            {
                QueueSpriteImporter(assetPath);
                PendingImports.Add(assetPath);
                return;
            }

            Object source = LoadSource(assetPath);
            if (source == null)
            {
                PendingImports.Add(assetPath);
                return;
            }

            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid))
            {
                return;
            }

            string metadataPath = GetMetadataPath(guid);
            RoomPainterAsset metadata = AssetDatabase.LoadAssetAtPath<RoomPainterAsset>(metadataPath);
            bool created = metadata == null;
            if (created)
            {
                metadata = ScriptableObject.CreateInstance<RoomPainterAsset>();
                metadata.id = guid;
                metadata.displayName = source.name;
                metadata.defaultLayer = RoomPainterAssetScanner.InferLayer(assetPath);
                metadata.defaultScale = Vector2.one;
                metadata.defaultVisualOffset = Vector2.zero;
                metadata.ySortEnabled = metadata.defaultLayer == RoomLayer.Cliff || metadata.defaultLayer == RoomLayer.Wall || metadata.defaultLayer == RoomLayer.Props;
                metadata.colliderSize = InferColliderSize(source);
                ApplyPhysicsDefaults(metadata, assetPath);
                ApplyLayerDefaults(metadata);
                AssetDatabase.CreateAsset(metadata, metadataPath);
            }

            metadata.id = guid;
            metadata.category = ParentFolderName(assetPath);
            metadata.sprite = source as Sprite;
            metadata.prefab = source as GameObject;

            if (metadata.colliderSize == Vector2.zero)
            {
                metadata.colliderSize = InferColliderSize(source);
            }

            EditorUtility.SetDirty(metadata);
            KnownAssetPaths.Add(Normalize(assetPath));
            RoomPainterAssetEvents.PublishCreatedOrUpdated(metadata);
        }

        private static void ApplyPhysicsDefaults(RoomPainterAsset metadata, string assetPath)
        {
            PhysicsConfig config = RoomPainterPhysicsRules.Resolve(assetPath);
            metadata.isBlock = config.isBlock;
            metadata.bodyType = config.bodyType;
            metadata.colliderShape = config.colliderShape;
            metadata.isTrigger = config.isTrigger;
            metadata.physicsLayerName = config.physicsLayerName;
        }

        private static void ApplyLayerDefaults(RoomPainterAsset metadata)
        {
            RoomLayerData layerData = LoadOrCreateLayerData(metadata.defaultLayer);
            metadata.defaultSortingLayer = layerData != null && !string.IsNullOrEmpty(layerData.sortingLayerName)
                ? layerData.sortingLayerName
                : "Default";
            metadata.defaultOrder = layerData != null ? layerData.defaultOrder : 0;
        }

        private static RoomLayerData LoadOrCreateLayerData(RoomLayer layer)
        {
            string path = LayerDataRoot + "/Layer_" + layer + ".asset";
            RoomLayerData data = AssetDatabase.LoadAssetAtPath<RoomLayerData>(path);
            if (data != null)
            {
                return data;
            }

            data = ScriptableObject.CreateInstance<RoomLayerData>();
            data.layer = layer;
            data.sortingLayerName = "Default";
            data.defaultOrder = DefaultOrder(layer);
            data.ySortEnabled = layer == RoomLayer.Cliff || layer == RoomLayer.Wall || layer == RoomLayer.Props;
            data.isCameraRelative = layer == RoomLayer.Parallax;
            AssetDatabase.CreateAsset(data, path);
            EditorUtility.SetDirty(data);
            return data;
        }

        private static int DefaultOrder(RoomLayer layer)
        {
            switch (layer)
            {
                case RoomLayer.Cliff:
                    return 5;
                case RoomLayer.Wall:
                    return 8;
                case RoomLayer.Props:
                    return 10;
                case RoomLayer.Decals:
                    return 2;
                case RoomLayer.Lighting:
                    return 20;
                case RoomLayer.Parallax:
                    return -100;
                default:
                    return 0;
            }
        }

        private static Vector2 InferColliderSize(Object source)
        {
            if (source is Sprite sprite)
            {
                return Vector2.Max(sprite.bounds.size * 0.85f, new Vector2(0.01f, 0.01f));
            }

            if (source is GameObject prefab)
            {
                SpriteRenderer renderer = prefab.GetComponentInChildren<SpriteRenderer>();
                if (renderer != null && renderer.sprite != null)
                {
                    return Vector2.Max(renderer.sprite.bounds.size * 0.85f, new Vector2(0.01f, 0.01f));
                }
            }

            return Vector2.one;
        }

        private static Object LoadSource(string assetPath)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                return sprite;
            }

            Object[] representations = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            for (int i = 0; i < representations.Length; i++)
            {
                if (representations[i] is Sprite spriteRepresentation)
                {
                    return spriteRepresentation;
                }
            }

            return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        }

        private static bool NeedsSpriteImporterUpdate(string assetPath)
        {
            if (!IsSpritePath(assetPath))
            {
                return false;
            }

            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                return false;
            }

            return importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Single;
        }

        private static void EnsureSpriteImporter(string assetPath)
        {
            if (!IsSpritePath(assetPath))
            {
                return;
            }

            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                return;
            }

            if (importer.textureType == TextureImporterType.Sprite && importer.spriteImportMode == SpriteImportMode.Single)
            {
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.SaveAndReimport();
        }

        private static bool ShouldProcess(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath) || assetPath.EndsWith(".meta"))
            {
                return false;
            }

            if (!IsUnderImportRoot(assetPath))
            {
                return false;
            }

            string extension = System.IO.Path.GetExtension(assetPath).ToLowerInvariant();
            return extension == ".png" || extension == ".psd" || extension == ".jpg" || extension == ".jpeg" || extension == ".prefab";
        }

        private static bool IsSpritePath(string assetPath)
        {
            string extension = System.IO.Path.GetExtension(assetPath).ToLowerInvariant();
            return extension == ".png" || extension == ".psd" || extension == ".jpg" || extension == ".jpeg";
        }

        private static bool IsUnderImportRoot(string assetPath)
        {
            string normalized = Normalize(assetPath);
            for (int i = 0; i < ImportRoots.Length; i++)
            {
                if (normalized.StartsWith(ImportRoots[i] + "/", System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void PublishDeletes(string[] paths)
        {
            if (paths == null)
            {
                return;
            }

            for (int i = 0; i < paths.Length; i++)
            {
                string guid = AssetDatabase.AssetPathToGUID(paths[i]);
                if (!string.IsNullOrEmpty(guid))
                {
                    RoomPainterAssetEvents.PublishDeleted(guid);
                }
            }
        }

        private static string GetMetadataPath(string guid)
        {
            return MetadataRoot + "/" + guid + ".asset";
        }

        private static string ParentFolderName(string assetPath)
        {
            string normalizedPath = Normalize(assetPath);
            int fileSlashIndex = normalizedPath.LastIndexOf('/');
            if (fileSlashIndex < 0)
            {
                return string.Empty;
            }

            string folderPath = normalizedPath.Substring(0, fileSlashIndex);
            int parentSlashIndex = folderPath.LastIndexOf('/');
            return parentSlashIndex >= 0 ? folderPath.Substring(parentSlashIndex + 1) : folderPath;
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static string Normalize(string path)
        {
            return string.IsNullOrEmpty(path) ? string.Empty : path.Replace('\\', '/');
        }
    }

    [InitializeOnLoad]
    internal static class RoomPainterAssetPostprocessorBootstrap
    {
        static RoomPainterAssetPostprocessorBootstrap()
        {
            RoomPainterAssetPostprocessor.InitializeWatcher();
        }
    }
}
