using System.Collections.Generic;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.SO;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Runtime
{
    [ExecuteAlways]
    public sealed class RoomDecalChunkRenderer : MonoBehaviour
    {
        [SerializeField] private RoomDecalDataSO decalData;
        [SerializeField] private PatchAtlasSO patchAtlas;
        [SerializeField] private Material materialOverride;
        [SerializeField] private RectInt chunkCells;
        [SerializeField] private bool constrainToChunkCells;

        private readonly List<Material> generatedMaterials = new List<Material>();

        public RoomDecalDataSO DecalData
        {
            get { return decalData; }
            set { decalData = value; }
        }

        public PatchAtlasSO PatchAtlas
        {
            get { return patchAtlas; }
            set { patchAtlas = value; }
        }

        public void Build()
        {
            DisposeGeneratedResources();
            BuildLayer(4, constrainToChunkCells, chunkCells);
            BuildLayer(5, constrainToChunkCells, chunkCells);
            BuildLayer(6, constrainToChunkCells, chunkCells);
        }

        public void RebuildDirty(RectInt cells)
        {
            if (constrainToChunkCells && !Overlaps(chunkCells, cells))
            {
                return;
            }

            Build();
        }

        private void OnEnable()
        {
            Build();
        }

        private void OnDisable()
        {
            DisposeGeneratedResources();
        }

        private void OnDestroy()
        {
            DisposeGeneratedResources();
        }

        private void BuildLayer(byte layer, bool filterByCells, RectInt cells)
        {
            GameObject child = EnsureLayerChild(layer);
            MeshFilter filter = child.GetComponent<MeshFilter>();
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();

            Mesh oldMesh = filter.sharedMesh;
            if (oldMesh != null)
            {
                DestroyResource(oldMesh);
            }

            var vertices = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();

            Texture2D texture = null;
            if (decalData != null && decalData.placements != null)
            {
                for (int i = 0; i < decalData.placements.Count; i++)
                {
                    DecalPlacement placement = decalData.placements[i];
                    if (placement.layer != layer)
                    {
                        continue;
                    }

                    if (filterByCells && !cells.Contains(WorldToCell(placement.worldPos)))
                    {
                        continue;
                    }

                    Sprite sprite = ResolveSprite(placement.spriteId);
                    if (sprite == null)
                    {
                        continue;
                    }

                    if (texture == null)
                    {
                        texture = sprite.texture;
                    }

                    AddQuad(vertices, uvs, triangles, placement, sprite);
                }
            }

            Mesh mesh = new Mesh();
            mesh.name = "RoomDecalMesh_L" + layer;
            mesh.SetVertices(vertices);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            filter.sharedMesh = mesh;

            renderer.sharedMaterial = ResolveMaterial(texture);
            renderer.sortingLayerName = SortingLayerFor(layer);
            renderer.sortingOrder = SortingOrderFor(layer);
        }

        private GameObject EnsureLayerChild(byte layer)
        {
            string childName = "RoomDecalLayer_L" + layer;
            Transform existing = transform.Find(childName);
            if (existing != null)
            {
                EnsureComponents(existing.gameObject);
                return existing.gameObject;
            }

            GameObject child = new GameObject(childName);
            child.transform.SetParent(transform, false);
            EnsureComponents(child);
            return child;
        }

        private static void EnsureComponents(GameObject child)
        {
            if (child.GetComponent<MeshFilter>() == null)
            {
                child.AddComponent<MeshFilter>();
            }

            if (child.GetComponent<MeshRenderer>() == null)
            {
                child.AddComponent<MeshRenderer>();
            }
        }

        private Sprite ResolveSprite(int spriteId)
        {
            if (patchAtlas == null || patchAtlas.variants == null || spriteId < 0 || spriteId >= patchAtlas.variants.Length)
            {
                return null;
            }

            return patchAtlas.variants[spriteId];
        }

        private Material ResolveMaterial(Texture2D texture)
        {
            if (materialOverride != null)
            {
                return materialOverride;
            }

            Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default");
            }

            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            Material material = new Material(shader);
            if (texture != null)
            {
                material.mainTexture = texture;
            }

            ConfigureTransparentMaterial(material, texture);
            generatedMaterials.Add(material);
            return material;
        }

        private static void ConfigureTransparentMaterial(Material material, Texture2D texture)
        {
            if (material == null)
            {
                return;
            }

            if (texture != null && material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", Color.white);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_Blend"))
            {
                material.SetFloat("_Blend", 0f);
            }

            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 0f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHATEST_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        private static void AddQuad(List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, DecalPlacement placement, Sprite sprite)
        {
            int start = vertices.Count;
            Rect bounds = SpriteLocalRect(sprite);
            bool flipX = (placement.flags & 1) != 0;
            bool flipY = (placement.flags & 2) != 0;
            float angle = placement.rotationStep * 90f * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            AddVertex(vertices, placement.worldPos, new Vector2(bounds.xMin, bounds.yMin), flipX, flipY, cos, sin);
            AddVertex(vertices, placement.worldPos, new Vector2(bounds.xMax, bounds.yMin), flipX, flipY, cos, sin);
            AddVertex(vertices, placement.worldPos, new Vector2(bounds.xMax, bounds.yMax), flipX, flipY, cos, sin);
            AddVertex(vertices, placement.worldPos, new Vector2(bounds.xMin, bounds.yMax), flipX, flipY, cos, sin);

            Rect uv = SpriteUvRect(sprite);
            uvs.Add(new Vector2(uv.xMin, uv.yMin));
            uvs.Add(new Vector2(uv.xMax, uv.yMin));
            uvs.Add(new Vector2(uv.xMax, uv.yMax));
            uvs.Add(new Vector2(uv.xMin, uv.yMax));

            triangles.Add(start);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start);
            triangles.Add(start + 2);
            triangles.Add(start + 3);
        }

        private static void AddVertex(List<Vector3> vertices, Vector2 worldPos, Vector2 local, bool flipX, bool flipY, float cos, float sin)
        {
            if (flipX)
            {
                local.x = -local.x;
            }

            if (flipY)
            {
                local.y = -local.y;
            }

            Vector2 rotated = new Vector2(local.x * cos - local.y * sin, local.x * sin + local.y * cos);
            vertices.Add(new Vector3(worldPos.x + rotated.x, worldPos.y + rotated.y, 0f));
        }

        private static Rect SpriteLocalRect(Sprite sprite)
        {
            Bounds bounds = sprite.bounds;
            return new Rect(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);
        }

        private static Rect SpriteUvRect(Sprite sprite)
        {
            Rect textureRect = sprite.textureRect;
            Texture2D texture = sprite.texture;
            return new Rect(
                textureRect.xMin / texture.width,
                textureRect.yMin / texture.height,
                textureRect.width / texture.width,
                textureRect.height / texture.height);
        }

        private static Vector2Int WorldToCell(Vector2 worldPos)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
        }

        private static bool Overlaps(RectInt a, RectInt b)
        {
            return a.xMin < b.xMax && a.xMax > b.xMin && a.yMin < b.yMax && a.yMax > b.yMin;
        }

        private static string SortingLayerFor(byte layer)
        {
            switch (layer)
            {
                case 5:
                    return "Detail";
                case 6:
                    return "Accent";
                default:
                    return "Patch";
            }
        }

        private static int SortingOrderFor(byte layer)
        {
            switch (layer)
            {
                case 5:
                    return 3;
                case 6:
                    return 4;
                default:
                    return 1;
            }
        }

        private void DisposeGeneratedResources()
        {
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(true);
            for (int i = 0; i < filters.Length; i++)
            {
                if (filters[i] != null && filters[i].sharedMesh != null)
                {
                    Mesh mesh = filters[i].sharedMesh;
                    filters[i].sharedMesh = null;
                    DestroyResource(mesh);
                }
            }

            for (int i = 0; i < generatedMaterials.Count; i++)
            {
                if (generatedMaterials[i] != null)
                {
                    DestroyResource(generatedMaterials[i]);
                }
            }

            generatedMaterials.Clear();
        }

        private static void DestroyResource(Object resource)
        {
            if (Application.isPlaying)
            {
                Destroy(resource);
            }
            else
            {
                DestroyImmediate(resource);
            }
        }
    }
}
