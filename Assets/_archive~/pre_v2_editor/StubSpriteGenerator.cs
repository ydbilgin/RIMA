using System.Collections.Generic;
using System.IO;
using RIMA.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public static class StubSpriteGenerator
    {
        private const string ClassFolder = "Assets/Data/Classes";
        private const string MobFolder = "Assets/Data/Mobs/F1";
        private const string CharPlaceholderFolder = "Assets/Art/Placeholders/Classes";
        private const string MobPlaceholderFolder = "Assets/Art/Placeholders/Mobs/F1";
        private const string WeaponPlaceholderFolder = "Assets/Art/Placeholders/Weapons";

        private static readonly Dictionary<char, byte[]> Font5x7 = BuildFont5x7();

        [MenuItem("RIMA/Tools/Generate Placeholder Sprites for Classes + Mobs")]
        public static void Generate()
        {
            EnsureFolder(CharPlaceholderFolder);
            EnsureFolder(MobPlaceholderFolder);
            EnsureFolder(WeaponPlaceholderFolder);

            int characters = 0;
            int mobs = 0;
            int weapons = 0;

            foreach (string guid in AssetDatabase.FindAssets("t:CharacterClassDefinition", new[] { ClassFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CharacterClassDefinition def = AssetDatabase.LoadAssetAtPath<CharacterClassDefinition>(path);
                if (def == null) continue;

                Color bg = HashColor(def.className, 0.65f, 0.55f);
                string initials = Initials(def.className);
                string charPath = $"{CharPlaceholderFolder}/{def.className}_placeholder.png";
                SavePlaceholder(charPath, 64, 64, bg, initials);
                def.idleSprite = LoadSprite(charPath);
                characters++;

                if (def.weaponDecoupled && def.weaponCanvas.x > 0 && def.weaponCanvas.y > 0)
                {
                    Color weaponBg = HashColor(def.className + "_weapon", 0.55f, 0.45f);
                    string weaponPath = $"{WeaponPlaceholderFolder}/{def.className}_weapon_placeholder.png";
                    SavePlaceholder(weaponPath, def.weaponCanvas.x, def.weaponCanvas.y, weaponBg, "W");
                    def.weaponSprite = LoadSprite(weaponPath);
                    weapons++;
                }

                EditorUtility.SetDirty(def);
            }

            foreach (string guid in AssetDatabase.FindAssets("t:MobDefinition", new[] { MobFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                MobDefinition def = AssetDatabase.LoadAssetAtPath<MobDefinition>(path);
                if (def == null) continue;

                Color bg = RoleColor(def.role);
                string initials = Initials(def.mobName);
                string mobPath = $"{MobPlaceholderFolder}/{def.mobName}_placeholder.png";
                int w = Mathf.Max(16, def.canvasSize.x);
                int h = Mathf.Max(16, def.canvasSize.y);
                SavePlaceholder(mobPath, w, h, bg, initials);
                def.idleSprite = LoadSprite(mobPath);
                mobs++;
                EditorUtility.SetDirty(def);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[RIMA] StubSpriteGenerator: characters={characters}, mobs={mobs}, weapons={weapons}");
        }

        private static Color RoleColor(MobRole role)
        {
            switch (role)
            {
                case MobRole.Swarm: return new Color(0.30f, 0.55f, 0.30f);
                case MobRole.Melee: return new Color(0.55f, 0.30f, 0.30f);
                case MobRole.Ranged: return new Color(0.30f, 0.40f, 0.60f);
                case MobRole.Caster: return new Color(0.45f, 0.30f, 0.55f);
                case MobRole.Elite: return new Color(0.65f, 0.25f, 0.25f);
                case MobRole.MiniBoss: return new Color(0.45f, 0.10f, 0.30f);
                case MobRole.Support: return new Color(0.30f, 0.55f, 0.55f);
                case MobRole.Pack: return new Color(0.55f, 0.45f, 0.25f);
                default: return Color.gray;
            }
        }

        private static Color HashColor(string seed, float s, float v)
        {
            int hash = 17;
            foreach (char c in seed) hash = hash * 31 + c;
            float h = (Mathf.Abs(hash) % 360) / 360f;
            return Color.HSVToRGB(h, s, v);
        }

        private static string Initials(string name)
        {
            if (string.IsNullOrEmpty(name)) return "??";
            string[] parts = name.Split(new[] { ' ', '_', '-' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2) return $"{parts[0][0]}{parts[1][0]}".ToUpperInvariant();
            return name.Length >= 2 ? name.Substring(0, 2).ToUpperInvariant() : name.ToUpperInvariant().PadRight(2, '?');
        }

        private static void SavePlaceholder(string assetPath, int width, int height, Color bg, string text)
        {
            string fullPath = Path.Combine(Application.dataPath, assetPath.Substring("Assets/".Length));
            string dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            Color border = bg * 0.6f; border.a = 1f;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isBorder = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    pixels[y * width + x] = isBorder ? border : bg;
                }
            }
            tex.SetPixels(pixels);

            int textWidth = text.Length * 6 - 1;
            int textX = Mathf.Max(2, (width - textWidth) / 2);
            int textY = Mathf.Max(2, (height - 7) / 2);
            foreach (char ch in text.ToUpperInvariant())
            {
                DrawChar5x7(tex, ch, textX, textY, Color.white);
                textX += 6;
            }
            tex.Apply();

            byte[] png = tex.EncodeToPNG();
            File.WriteAllBytes(fullPath, png);
            Object.DestroyImmediate(tex);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = false;
                importer.spritePixelsPerUnit = 64;
                importer.SaveAndReimport();
            }
        }

        private static Sprite LoadSprite(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        }

        private static void DrawChar5x7(Texture2D tex, char ch, int originX, int originY, Color color)
        {
            if (!Font5x7.TryGetValue(ch, out byte[] glyph)) return;
            for (int row = 0; row < 7; row++)
            {
                byte bits = glyph[row];
                for (int col = 0; col < 5; col++)
                {
                    if (((bits >> (4 - col)) & 1) == 1)
                    {
                        int x = originX + col;
                        int y = originY + (6 - row);
                        if (x >= 0 && x < tex.width && y >= 0 && y < tex.height)
                            tex.SetPixel(x, y, color);
                    }
                }
            }
        }

        private static Dictionary<char, byte[]> BuildFont5x7()
        {
            var f = new Dictionary<char, byte[]>();
            f['A'] = new byte[] { 0b01110, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001 };
            f['B'] = new byte[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10001, 0b10001, 0b11110 };
            f['C'] = new byte[] { 0b01110, 0b10001, 0b10000, 0b10000, 0b10000, 0b10001, 0b01110 };
            f['D'] = new byte[] { 0b11110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b11110 };
            f['E'] = new byte[] { 0b11111, 0b10000, 0b10000, 0b11110, 0b10000, 0b10000, 0b11111 };
            f['F'] = new byte[] { 0b11111, 0b10000, 0b10000, 0b11110, 0b10000, 0b10000, 0b10000 };
            f['G'] = new byte[] { 0b01110, 0b10001, 0b10000, 0b10111, 0b10001, 0b10001, 0b01110 };
            f['H'] = new byte[] { 0b10001, 0b10001, 0b10001, 0b11111, 0b10001, 0b10001, 0b10001 };
            f['I'] = new byte[] { 0b01110, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b01110 };
            f['J'] = new byte[] { 0b00111, 0b00010, 0b00010, 0b00010, 0b00010, 0b10010, 0b01100 };
            f['K'] = new byte[] { 0b10001, 0b10010, 0b10100, 0b11000, 0b10100, 0b10010, 0b10001 };
            f['L'] = new byte[] { 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b10000, 0b11111 };
            f['M'] = new byte[] { 0b10001, 0b11011, 0b10101, 0b10101, 0b10001, 0b10001, 0b10001 };
            f['N'] = new byte[] { 0b10001, 0b11001, 0b10101, 0b10011, 0b10001, 0b10001, 0b10001 };
            f['O'] = new byte[] { 0b01110, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 };
            f['P'] = new byte[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10000, 0b10000, 0b10000 };
            f['Q'] = new byte[] { 0b01110, 0b10001, 0b10001, 0b10001, 0b10101, 0b10010, 0b01101 };
            f['R'] = new byte[] { 0b11110, 0b10001, 0b10001, 0b11110, 0b10100, 0b10010, 0b10001 };
            f['S'] = new byte[] { 0b01111, 0b10000, 0b10000, 0b01110, 0b00001, 0b00001, 0b11110 };
            f['T'] = new byte[] { 0b11111, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100, 0b00100 };
            f['U'] = new byte[] { 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01110 };
            f['V'] = new byte[] { 0b10001, 0b10001, 0b10001, 0b10001, 0b10001, 0b01010, 0b00100 };
            f['W'] = new byte[] { 0b10001, 0b10001, 0b10001, 0b10101, 0b10101, 0b10101, 0b01010 };
            f['X'] = new byte[] { 0b10001, 0b10001, 0b01010, 0b00100, 0b01010, 0b10001, 0b10001 };
            f['Y'] = new byte[] { 0b10001, 0b10001, 0b01010, 0b00100, 0b00100, 0b00100, 0b00100 };
            f['Z'] = new byte[] { 0b11111, 0b00001, 0b00010, 0b00100, 0b01000, 0b10000, 0b11111 };
            f['?'] = new byte[] { 0b01110, 0b10001, 0b00001, 0b00010, 0b00100, 0b00000, 0b00100 };
            return f;
        }

        private static void EnsureFolder(string folder)
        {
            if (AssetDatabase.IsValidFolder(folder)) return;
            string parent = Path.GetDirectoryName(folder)?.Replace('\\', '/');
            string leaf = Path.GetFileName(folder);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
            {
                EnsureFolder(parent);
            }
            AssetDatabase.CreateFolder(parent ?? "Assets", leaf);
        }
    }
}
