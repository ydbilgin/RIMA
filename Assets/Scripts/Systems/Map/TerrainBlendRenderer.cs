using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TerrainBlendRenderer : MonoBehaviour
    {
        private Texture2D _splatMap;
        private Mesh _mesh;
        private Material _materialInstance;

        public void Render(RIMA.MapDesigner.RoomData room, RimaBiomePreset biome, TerrainBlendConfig config)
        {
            if (room.vertexGrid == null || config == null || config.blendMaterial == null)
            {
                return;
            }

            Clear();

            int width = room.size.x;
            int height = room.size.y;
            int vw = width + 1;
            int vh = height + 1;

            // Build terrain ID to splat channel mapping (max 4 channels)
            Dictionary<int, int> idToChannel = new Dictionary<int, int>();
            int channelIndex = 0;
            if (biome != null && biome.terrains != null)
            {
                for (int i = 0; i < biome.terrains.Count && channelIndex < 4; i++)
                {
                    MapTerrain terrain = biome.terrains[i];
                    if (terrain != null && !idToChannel.ContainsKey(terrain.id))
                    {
                        idToChannel[terrain.id] = channelIndex;
                        channelIndex++;
                    }
                }
            }

            // Generate RGBA splat map from vertex grid
            _splatMap = new Texture2D(vw, vh, TextureFormat.RGBA32, false);
            _splatMap.filterMode = FilterMode.Bilinear;
            _splatMap.wrapMode = TextureWrapMode.Clamp;

            Color[] pixels = new Color[vw * vh];
            for (int y = 0; y < vh; y++)
            {
                for (int x = 0; x < vw; x++)
                {
                    int terrainId = room.vertexGrid[x, y];
                    int channel;
                    if (!idToChannel.TryGetValue(terrainId, out channel))
                    {
                        channel = 0;
                    }

                    Color pixel = new Color(0, 0, 0, 0);
                    switch (channel)
                    {
                        case 0: pixel.r = 1f; break;
                        case 1: pixel.g = 1f; break;
                        case 2: pixel.b = 1f; break;
                        case 3: pixel.a = 1f; break;
                    }
                    pixels[y * vw + x] = pixel;
                }
            }
            _splatMap.SetPixels(pixels);
            _splatMap.Apply();

            // Create quad mesh
            _mesh = new Mesh();
            _mesh.name = "TerrainBlendQuad";
            _mesh.vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(width, 0, 0),
                new Vector3(width, height, 0),
                new Vector3(0, height, 0)
            };
            _mesh.uv = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            _mesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            // Apply to components
            MeshFilter mf = GetComponent<MeshFilter>();
            MeshRenderer mr = GetComponent<MeshRenderer>();
            mf.sharedMesh = _mesh;

            _materialInstance = new Material(config.blendMaterial);
            mr.sharedMaterial = _materialInstance;
            mr.sortingLayerName = "Default";
            mr.sortingOrder = -1;

            // Set material properties
            _materialInstance.SetTexture("_SplatMap", _splatMap);
            _materialInstance.SetFloat("_TerrainTiling", config.terrainTiling);
            _materialInstance.SetFloat("_NoiseScale", config.noiseScale);
            _materialInstance.SetFloat("_NoiseStrength", config.noiseStrength);
            _materialInstance.SetFloat("_BlendSharpness", config.blendSharpness);

            if (config.noiseTexture != null)
            {
                _materialInstance.SetTexture("_NoiseTex", config.noiseTexture);
            }

            string[] texProps = { "_TerrainTex0", "_TerrainTex1", "_TerrainTex2", "_TerrainTex3" };
            if (config.terrainTextures != null)
            {
                for (int i = 0; i < 4 && i < config.terrainTextures.Length; i++)
                {
                    if (config.terrainTextures[i] != null)
                    {
                        _materialInstance.SetTexture(texProps[i], config.terrainTextures[i]);
                    }
                }
            }
        }

        public void Clear()
        {
            if (_splatMap != null)
            {
                if (Application.isPlaying) Destroy(_splatMap);
                else DestroyImmediate(_splatMap);
                _splatMap = null;
            }

            if (_mesh != null)
            {
                if (Application.isPlaying) Destroy(_mesh);
                else DestroyImmediate(_mesh);
                _mesh = null;
            }

            if (_materialInstance != null)
            {
                if (Application.isPlaying) Destroy(_materialInstance);
                else DestroyImmediate(_materialInstance);
                _materialInstance = null;
            }
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}
