using System.IO;
using RIMA.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public class SpriteHandAnnotatorWindow : EditorWindow
    {
        private const string DefaultOutputFolder = "Assets/Data";

        private Sprite sprite;
        private SpriteHandData handData;
        private bool editingLeft = true;
        private Vector2 previewScroll;
        private float zoom = 4f;

        [MenuItem("RIMA/Tools/Sprite Hand Annotator")]
        public static void Open()
        {
            GetWindow<SpriteHandAnnotatorWindow>("Hand Annotator");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Sprite Hand Data", EditorStyles.boldLabel);
            using (new EditorGUI.ChangeCheckScope())
            {
                sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", sprite, typeof(Sprite), false);
                handData = (SpriteHandData)EditorGUILayout.ObjectField("Hand Data", handData, typeof(SpriteHandData), false);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Toggle(editingLeft, "Left", EditorStyles.toolbarButton)) editingLeft = true;
                if (GUILayout.Toggle(!editingLeft, "Right", EditorStyles.toolbarButton)) editingLeft = false;
            }

            zoom = EditorGUILayout.Slider("Zoom", zoom, 1f, 16f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Create/Load Data", GUILayout.Height(24)))
                {
                    CreateOrLoadData();
                }

                using (new EditorGUI.DisabledScope(handData == null))
                {
                    if (GUILayout.Button("Save", GUILayout.Height(24)))
                    {
                        EditorUtility.SetDirty(handData);
                        AssetDatabase.SaveAssets();
                    }
                }
            }

            DrawFields();
            DrawPreview();
        }

        private void DrawFields()
        {
            if (handData == null) return;

            EditorGUILayout.Space(6);
            handData.sprite = (Sprite)EditorGUILayout.ObjectField("Data Sprite", handData.sprite, typeof(Sprite), false);
            handData.hasLeftHand = EditorGUILayout.Toggle("Has Left Hand", handData.hasLeftHand);
            handData.handLeftPx = EditorGUILayout.Vector2Field("Hand Left Anchor", handData.handLeftPx);
            handData.hasRightHand = EditorGUILayout.Toggle("Has Right Hand", handData.hasRightHand);
            handData.handRightPx = EditorGUILayout.Vector2Field("Hand Right Anchor", handData.handRightPx);
        }

        private void DrawPreview()
        {
            if (sprite == null)
            {
                EditorGUILayout.HelpBox("Assign a sprite, then click Create/Load Data.", MessageType.Info);
                return;
            }

            Texture2D texture = sprite.texture;
            Rect rect = sprite.rect;
            Rect uv = new Rect(
                rect.x / texture.width,
                rect.y / texture.height,
                rect.width / texture.width,
                rect.height / texture.height);

            Vector2 drawSize = rect.size * zoom;
            previewScroll = EditorGUILayout.BeginScrollView(previewScroll, GUILayout.ExpandHeight(true));
            Rect drawRect = GUILayoutUtility.GetRect(drawSize.x, drawSize.y, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            GUI.DrawTextureWithTexCoords(drawRect, texture, uv, true);

            if (handData != null)
            {
                DrawAnchor(drawRect, rect.size, handData.handLeftPx, Color.cyan, "L");
                DrawAnchor(drawRect, rect.size, handData.handRightPx, Color.magenta, "R");
            }

            HandleClick(drawRect, rect.size);
            EditorGUILayout.EndScrollView();
        }

        private void HandleClick(Rect drawRect, Vector2 spriteSize)
        {
            Event current = Event.current;
            if (handData == null || current.type != EventType.MouseDown || current.button != 0)
            {
                return;
            }

            if (!drawRect.Contains(current.mousePosition))
            {
                return;
            }

            Vector2 local = current.mousePosition - drawRect.position;
            Vector2 pixel = new Vector2(local.x / drawRect.width * spriteSize.x, spriteSize.y - local.y / drawRect.height * spriteSize.y);
            pixel.x = Mathf.Clamp(pixel.x, 0f, spriteSize.x);
            pixel.y = Mathf.Clamp(pixel.y, 0f, spriteSize.y);

            Undo.RecordObject(handData, "Set Hand Anchor");
            if (editingLeft)
            {
                handData.handLeftPx = pixel;
                handData.hasLeftHand = true;
            }
            else
            {
                handData.handRightPx = pixel;
                handData.hasRightHand = true;
            }

            EditorUtility.SetDirty(handData);
            current.Use();
            Repaint();
        }

        private static void DrawAnchor(Rect drawRect, Vector2 spriteSize, Vector2 pixel, Color color, string label)
        {
            Vector2 position = new Vector2(
                drawRect.x + pixel.x / spriteSize.x * drawRect.width,
                drawRect.y + (1f - pixel.y / spriteSize.y) * drawRect.height);

            Handles.BeginGUI();
            Color old = Handles.color;
            Handles.color = color;
            Handles.DrawLine(position + Vector2.left * 8f, position + Vector2.right * 8f);
            Handles.DrawLine(position + Vector2.down * 8f, position + Vector2.up * 8f);
            GUI.Label(new Rect(position.x + 6f, position.y - 16f, 24f, 18f), label);
            Handles.color = old;
            Handles.EndGUI();
        }

        private void CreateOrLoadData()
        {
            if (sprite == null) return;

            string spritePath = AssetDatabase.GetAssetPath(sprite);
            string dataPath = FindExistingData(sprite);
            if (string.IsNullOrEmpty(dataPath))
            {
                EnsureFolder(DefaultOutputFolder);
                string fileName = $"SpriteHandData_{Path.GetFileNameWithoutExtension(spritePath)}.asset";
                dataPath = AssetDatabase.GenerateUniqueAssetPath($"{DefaultOutputFolder}/{fileName}");

                handData = CreateInstance<SpriteHandData>();
                handData.sprite = sprite;
                handData.handLeftPx = new Vector2(sprite.rect.width * 0.42f, sprite.rect.height * 0.55f);
                handData.handRightPx = new Vector2(sprite.rect.width * 0.58f, sprite.rect.height * 0.55f);
                AssetDatabase.CreateAsset(handData, dataPath);
            }
            else
            {
                handData = AssetDatabase.LoadAssetAtPath<SpriteHandData>(dataPath);
                handData.sprite = sprite;
            }

            EditorUtility.SetDirty(handData);
            AssetDatabase.SaveAssets();
            Selection.activeObject = handData;
        }

        private static string FindExistingData(Sprite targetSprite)
        {
            string[] guids = AssetDatabase.FindAssets("t:SpriteHandData", new[] { DefaultOutputFolder });
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SpriteHandData data = AssetDatabase.LoadAssetAtPath<SpriteHandData>(path);
                if (data != null && data.Matches(targetSprite))
                {
                    return path;
                }
            }

            return null;
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
