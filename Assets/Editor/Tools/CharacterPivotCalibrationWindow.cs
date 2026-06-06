using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// RIMA/Characters/Calibration — per-class pivot audit + Apply Auto + manual nudge.
/// Scans Assets/Resources/Characters for idle sprites. Reuses SpritePivotBatchFix
/// alpha-scan logic. Nudge values persisted via EditorPrefs.
/// </summary>
public sealed class CharacterPivotCalibrationWindow : EditorWindow
{
    private const string CharactersRoot = "Assets/Resources/Characters";
    private const string NudgeKeyPrefix = "RIMA.PivotNudge.";
    private const float AlphaThreshold = 10f / 255f;

    // ── per-class data ──────────────────────────────────────────────────────
    private struct ClassData
    {
        public string ClassName;
        public List<string> SpritePaths;
        public float AutoPivotY;      // normalised [0,1], averaged across sprites
        public float CurrentPivotY;   // first sprite's current setting
        public int   CanvasHeight;
        public int   AutoFeetRow;     // pixel row from bottom
        public float NudgePx;         // user-controlled nudge in pixels
    }

    private List<ClassData> _classes = new();
    private Vector2 _scroll;
    private bool _scanned;

    // ── lifecycle ───────────────────────────────────────────────────────────
    [MenuItem("RIMA/Characters/Calibration")]
    public static void Open() => GetWindow<CharacterPivotCalibrationWindow>("Char Pivot Cal");

    private void OnGUI()
    {
        EditorGUILayout.Space(4);

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Scan All Classes", GUILayout.Height(26)))
                Scan();

            GUI.enabled = _scanned && _classes.Count > 0;
            if (GUILayout.Button("Apply Auto – All", GUILayout.Height(26)))
                ApplyAll();
            GUI.enabled = true;
        }

        if (!_scanned)
        {
            EditorGUILayout.HelpBox("Press Scan to audit character sprite pivots.", MessageType.Info);
            return;
        }

        if (_classes.Count == 0)
        {
            EditorGUILayout.HelpBox($"No idle sprites found under {CharactersRoot}.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space(4);

        // header
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            GUILayout.Label("Class",         GUILayout.Width(110));
            GUILayout.Label("Canvas H",      GUILayout.Width(64));
            GUILayout.Label("Auto Feet px",  GUILayout.Width(80));
            GUILayout.Label("Current pivot", GUILayout.Width(84));
            GUILayout.Label("Nudge (px)",    GUILayout.Width(72));
            GUILayout.Label("Status",        GUILayout.Width(56));
            GUILayout.FlexibleSpace();
        }

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        for (int i = 0; i < _classes.Count; i++)
        {
            ClassData cd = _classes[i];
            bool off = IsPivotOff(cd);

            using (new EditorGUILayout.HorizontalScope(off ? HighlightStyle() : GUIStyle.none))
            {
                GUILayout.Label(cd.ClassName,                              GUILayout.Width(110));
                GUILayout.Label(cd.CanvasHeight.ToString(),                GUILayout.Width(64));
                GUILayout.Label($"{cd.AutoFeetRow} ({cd.AutoPivotY:F3})",  GUILayout.Width(80));
                GUILayout.Label($"{cd.CurrentPivotY:F3}",                  GUILayout.Width(84));

                // nudge field
                float nudge = EditorGUILayout.FloatField(cd.NudgePx, GUILayout.Width(60));
                if (!Mathf.Approximately(nudge, cd.NudgePx))
                {
                    cd.NudgePx = nudge;
                    _classes[i] = cd;
                    SaveNudge(cd.ClassName, nudge);
                }

                // status badge
                GUILayout.Label(off ? "OFF" : "OK",
                    off ? OffStyle() : OkStyle(), GUILayout.Width(40));

                // per-class apply
                if (GUILayout.Button("Apply", GUILayout.Width(54)))
                {
                    ApplyClass(cd);
                    Scan(); // re-read after import
                    break;
                }
            }
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4);
        EditorGUILayout.HelpBox(
            "Auto Feet px = lowest opaque row from bottom of canvas (alpha > 10).\n" +
            "Nudge shifts the auto pivot up/down in pixels (positive = up).\n" +
            "Status is OFF when current pivot differs from auto by > 2 px.",
            MessageType.None);
    }

    // ── scan ────────────────────────────────────────────────────────────────
    private void Scan()
    {
        _classes.Clear();
        _scanned = true;

        if (!AssetDatabase.IsValidFolder(CharactersRoot))
        {
            Debug.LogWarning($"CharacterPivotCalibration: folder missing — {CharactersRoot}");
            return;
        }

        // group sprites by class folder name
        var byClass = new Dictionary<string, List<string>>();
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { CharactersRoot });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!IsIdleSprite(path)) continue;
            string cls = GetClassName(path);
            if (!byClass.TryGetValue(cls, out var list))
            {
                list = new List<string>();
                byClass[cls] = list;
            }
            list.Add(path);
        }

        foreach (var kv in byClass.OrderBy(p => p.Key))
        {
            var cd = BuildClassData(kv.Key, kv.Value);
            _classes.Add(cd);
        }

        Repaint();
    }

    private ClassData BuildClassData(string className, List<string> paths)
    {
        float nudge = LoadNudge(className);
        var cd = new ClassData
        {
            ClassName  = className,
            SpritePaths = paths,
            NudgePx    = nudge,
        };

        // Make first sprite readable temporarily to read pivot + measure
        var firstPath = paths[0];
        var importer = AssetImporter.GetAtPath(firstPath) as TextureImporter;
        if (importer == null) return cd;

        // Read current pivot (normalised)
        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        cd.CurrentPivotY = settings.spritePivot.y;

        // Make readable, load, measure
        bool wasReadable = importer.isReadable;
        importer.isReadable = true;
        importer.SaveAndReimport();

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(firstPath);
        if (tex != null)
        {
            cd.CanvasHeight = tex.height;
            int lowestRow = FindLowestOpaqueRow(tex);
            cd.AutoFeetRow = lowestRow;
            cd.AutoPivotY  = (float)lowestRow / tex.height;
        }

        // Restore readable state
        if (importer.isReadable != wasReadable)
        {
            importer.isReadable = wasReadable;
            importer.SaveAndReimport();
        }

        return cd;
    }

    // ── apply ────────────────────────────────────────────────────────────────
    private void ApplyAll()
    {
        foreach (var cd in _classes) ApplyClass(cd);
        Scan();
    }

    private void ApplyClass(ClassData cd)
    {
        int applied = 0;
        foreach (string path in cd.SpritePaths)
        {
            if (ApplyMeasuredPivot(path, cd.NudgePx))
                applied++;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"CharacterPivotCalibration: {cd.ClassName} — applied measured-feet pivot to {applied} sprite(s). Nudge={cd.NudgePx:F1}px");
    }

    private static bool ApplyMeasuredPivot(string path, float nudgePx)
    {
        if (AssetImporter.GetAtPath(path) is not TextureImporter importer) return false;

        bool wasReadable = importer.isReadable;
        importer.isReadable = true;
        importer.textureType = TextureImporterType.Sprite;
        if (importer.spriteImportMode != SpriteImportMode.Single)
            importer.spriteImportMode = SpriteImportMode.Single;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled       = false;
        importer.filterMode          = FilterMode.Point;
        importer.spritePixelsPerUnit = 64f;
        importer.SaveAndReimport();

        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (tex == null) return false;

        int lowestRow = FindLowestOpaqueRow(tex);
        // Apply nudge (positive = move pivot up, i.e. higher row index)
        int nudgedRow = Mathf.Clamp(lowestRow + Mathf.RoundToInt(nudgePx), 0, tex.height - 1);
        float pivotY  = Mathf.Clamp01((float)nudgedRow / tex.height);
        var pivot = new Vector2(0.5f, pivotY);

        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteAlignment = (int)SpriteAlignment.Custom;
        settings.spritePivot     = pivot;
        importer.SetTextureSettings(settings);
        importer.spritePivot = pivot;

        importer.isReadable = wasReadable;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled       = false;
        importer.filterMode          = FilterMode.Point;
        importer.spritePixelsPerUnit = 64f;
        importer.SaveAndReimport();

        return true;
    }

    // ── helpers ──────────────────────────────────────────────────────────────
    private static int FindLowestOpaqueRow(Texture2D tex)
    {
        Color32[] pixels = tex.GetPixels32();
        for (int y = 0; y < tex.height; y++)
        {
            int rowOffset = y * tex.width;
            for (int x = 0; x < tex.width; x++)
            {
                if (pixels[rowOffset + x].a > 10)
                    return y;
            }
        }
        return 0;
    }

    private static bool IsIdleSprite(string path)
    {
        string file = Path.GetFileName(path);
        return path.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase)
            && file.Contains("_idle_");
    }

    private static string GetClassName(string path)
    {
        // Assets/Resources/Characters/<ClassName>/...
        string rel = path.Replace('\\', '/');
        string[] parts = rel.Split('/');
        // find "Characters" and take the next segment
        for (int i = 0; i < parts.Length - 1; i++)
        {
            if (parts[i] == "Characters") return parts[i + 1];
        }
        return Path.GetFileName(Path.GetDirectoryName(rel));
    }

    private static bool IsPivotOff(ClassData cd)
    {
        if (cd.CanvasHeight <= 0) return false;
        float currentRow  = cd.CurrentPivotY * cd.CanvasHeight;
        float targetRow   = cd.AutoFeetRow + cd.NudgePx;
        return Mathf.Abs(currentRow - targetRow) > 2f;
    }

    // ── EditorPrefs persistence ──────────────────────────────────────────────
    private static void SaveNudge(string cls, float v) =>
        EditorPrefs.SetFloat(NudgeKeyPrefix + cls, v);
    private static float LoadNudge(string cls) =>
        EditorPrefs.GetFloat(NudgeKeyPrefix + cls, 0f);

    // ── styles ───────────────────────────────────────────────────────────────
    private static GUIStyle _highlightStyle;
    private static GUIStyle _offStyle;
    private static GUIStyle _okStyle;

    private static GUIStyle HighlightStyle()
    {
        if (_highlightStyle == null)
        {
            _highlightStyle = new GUIStyle();
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, new Color(1f, 0.85f, 0.3f, 0.18f));
            tex.Apply();
            _highlightStyle.normal.background = tex;
        }
        return _highlightStyle;
    }

    private static GUIStyle OffStyle()
    {
        if (_offStyle == null)
        {
            _offStyle = new GUIStyle(EditorStyles.boldLabel);
            _offStyle.normal.textColor = new Color(0.9f, 0.3f, 0.2f);
        }
        return _offStyle;
    }

    private static GUIStyle OkStyle()
    {
        if (_okStyle == null)
        {
            _okStyle = new GUIStyle(EditorStyles.label);
            _okStyle.normal.textColor = new Color(0.2f, 0.7f, 0.3f);
        }
        return _okStyle;
    }
}
