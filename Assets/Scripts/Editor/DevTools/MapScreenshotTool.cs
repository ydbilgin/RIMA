#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using RIMA.MapDesigner.Room.Data;
using RIMA.Editor.Map;

namespace RIMA.Editor.DevTools
{
    public static class MapScreenshotTool
    {
        private static List<RoomTemplateSO> templatesToProcess;
        private static int currentIndex;
        private static string outputDir;
        private static string schematicsDir;
        private static string scenesDir;
        private static bool isRunning;
        private static int step; // 0: build and frame, 1: take screenshot, 2: wait frame
        private static double nextStepTime;

        [MenuItem("RIMA/Utilities/Capture All Maps and Scenes")]
        public static void StartCapture()
        {
            if (isRunning)
            {
                Debug.LogWarning("[MapScreenshotTool] Already running!");
                return;
            }

            if (Application.isPlaying)
            {
                Debug.LogError("[MapScreenshotTool] Must be in Edit Mode!");
                return;
            }

            // Find all RoomTemplates
            templatesToProcess = AssetDatabase.FindAssets("t:RoomTemplateSO", new[] { "Assets/Data/Rooms" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<RoomTemplateSO>)
                .Where(t => t != null)
                .OrderBy(t => t.name)
                .ToList();

            if (templatesToProcess.Count == 0)
            {
                Debug.LogError("[MapScreenshotTool] No RoomTemplateSO found in Assets/Data/Rooms!");
                return;
            }

            outputDir = Path.Combine(Directory.GetCurrentDirectory(), "STAGING", "captured_maps");
            schematicsDir = Path.Combine(outputDir, "schematics");
            scenesDir = Path.Combine(outputDir, "scenes");

            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }
            Directory.CreateDirectory(outputDir);
            Directory.CreateDirectory(schematicsDir);
            Directory.CreateDirectory(scenesDir);

            currentIndex = 0;
            step = 0;
            isRunning = true;
            nextStepTime = EditorApplication.timeSinceStartup;

            // Open Arena Scene if not open
            if (EditorSceneManager.GetActiveScene().path != RoomTemplateBuildUtility.ArenaScenePath)
            {
                EditorSceneManager.OpenScene(RoomTemplateBuildUtility.ArenaScenePath, OpenSceneMode.Single);
            }

            EditorApplication.update += UpdateLoop;
            Debug.Log($"[MapScreenshotTool] Started capturing {templatesToProcess.Count} maps...");
        }

        private static void UpdateLoop()
        {
            if (!isRunning) return;

            if (EditorApplication.timeSinceStartup < nextStepTime) return;

            if (currentIndex >= templatesToProcess.Count)
            {
                FinishCapture();
                return;
            }

            RoomTemplateSO template = templatesToProcess[currentIndex];

            if (step == 0)
            {
                // 1. Generate 2D schematic preview PNG
                string schematicPath = Path.Combine(schematicsDir, $"{template.name}.png");
                GenerateSchematicPNG(template, schematicPath);

                // 2. Build in Arena scene
                RoomTemplateBuildUtility.BuildInArena(template, "MapScreenshotTool");

                // Focus scene view and force frame
                if (SceneView.lastActiveSceneView != null)
                {
                    SceneView.lastActiveSceneView.Focus();
                }

                step = 1;
                nextStepTime = EditorApplication.timeSinceStartup + 0.3; // wait 300ms for rendering/framing to stabilize
            }
            else if (step == 1)
            {
                // 3. Take scene screenshot
                string scenePath = Path.Combine(scenesDir, $"{template.name}.png");
                CaptureSceneView(scenePath);

                Debug.Log($"[MapScreenshotTool] Captured scene screenshot for {template.name} ({currentIndex + 1}/{templatesToProcess.Count})");

                step = 2;
                nextStepTime = EditorApplication.timeSinceStartup + 0.1;
            }
            else if (step == 2)
            {
                // Move to next template
                currentIndex++;
                step = 0;
                nextStepTime = EditorApplication.timeSinceStartup + 0.1;
            }
        }

        private static void FinishCapture()
        {
            EditorApplication.update -= UpdateLoop;
            isRunning = false;
            Debug.Log($"[MapScreenshotTool] Successfully captured all {templatesToProcess.Count} maps. Files saved in STAGING/captured_maps");
        }

        private static void CaptureSceneView(string outputPath)
        {
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null)
            {
                Debug.LogError("[MapScreenshotTool] No active SceneView found!");
                return;
            }

            Camera camera = sceneView.camera;
            if (camera == null)
            {
                Debug.LogError("[MapScreenshotTool] No camera on SceneView!");
                return;
            }

            sceneView.Repaint();

            int width = 1280;
            int height = 720;
            RenderTexture rt = new RenderTexture(width, height, 24);
            RenderTexture oldRt = camera.targetTexture;
            
            camera.targetTexture = rt;
            camera.Render();
            camera.targetTexture = oldRt;

            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenShot.Apply();

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(outputPath, bytes);

            RenderTexture.active = null;
            rt.Release();
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(screenShot);
        }

        private static void GenerateSchematicPNG(RoomTemplateSO template, string outputPath)
        {
            int cellSize = 32;
            int borderSize = 1;
            int width = template.bounds.width * cellSize;
            int height = template.bounds.height * cellSize;
            
            if (width <= 0 || height <= 0) return;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            Color voidColor = Color.black;
            Color walkableColor = new Color(0.16f, 0.18f, 0.18f);
            Color borderColor = new Color(0.25f, 0.25f, 0.25f);

            // Clear
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tex.SetPixel(x, y, voidColor);

            // Draw cells
            for (int y = template.bounds.yMin; y < template.bounds.yMax; y++)
            {
                for (int x = template.bounds.xMin; x < template.bounds.xMax; x++)
                {
                    Vector2Int tile = new Vector2Int(x, y);
                    int localX = x - template.bounds.xMin;
                    int localY = y - template.bounds.yMin;

                    Color fill = template.IsWalkable(tile) ? walkableColor : voidColor;

                    for (int cy = 0; cy < cellSize; cy++)
                    {
                        for (int cx = 0; cx < cellSize; cx++)
                        {
                            int px = localX * cellSize + cx;
                            int py = localY * cellSize + cy;

                            if (cx < borderSize || cy < borderSize)
                            {
                                tex.SetPixel(px, py, borderColor);
                            }
                            else
                            {
                                tex.SetPixel(px, py, fill);
                            }
                        }
                    }
                }
            }

            // Draw Player Spawn
            if (template.playerSpawn != null)
            {
                int localX = template.playerSpawn.position.x - template.bounds.xMin;
                int localY = template.playerSpawn.position.y - template.bounds.yMin;
                int startX = localX * cellSize + cellSize / 4;
                int startY = localY * cellSize + cellSize / 4;
                int size = cellSize / 2;
                for (int cy = 0; cy < size; cy++)
                    for (int cx = 0; cx < size; cx++)
                        tex.SetPixel(startX + cx, startY + cy, Color.green);
            }

            // Draw Door Sockets
            if (template.doorSockets != null)
            {
                for (int i = 0; i < template.doorSockets.Count; i++)
                {
                    var door = template.doorSockets[i];
                    if (door == null) continue;
                    int localX = door.position.x - template.bounds.xMin;
                    int localY = door.position.y - template.bounds.yMin;

                    int slotIndex = RoomTemplateSO.ExitSlotIndex(door);
                    Color doorColor = Color.gray;
                    if (slotIndex == 0) doorColor = new Color(0.25f, 0.65f, 1f); // NW
                    else if (slotIndex == 1) doorColor = new Color(0.1f, 1f, 0.95f); // N
                    else if (slotIndex == 2) doorColor = new Color(0.85f, 0.55f, 1f); // NE

                    int startX = localX * cellSize + cellSize / 4;
                    int startY = localY * cellSize + cellSize / 4;
                    int size = cellSize / 2;
                    for (int cy = 0; cy < size; cy++)
                        for (int cx = 0; cx < size; cx++)
                            tex.SetPixel(startX + cx, startY + cy, doorColor);
                }
            }

            tex.Apply();
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(outputPath, bytes);
            Object.DestroyImmediate(tex);
        }
    }
}
#endif
