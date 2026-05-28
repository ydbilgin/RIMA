using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class CharacterAnimationBuilder
{
    static readonly string[] Characters = { "Shadowblade", "Elementalist", "Ranger" };

    struct AnimDef
    {
        public string folderName;  // in Sprites/
        public string shortName;   // for .anim filename
        public bool loop;
        public int frameCount;
        public float fps;
    }

    static readonly AnimDef[] Animations = new AnimDef[]
    {
        new AnimDef { folderName = "fight-stance-idle",  shortName = "idle",  loop = true,  frameCount = 8, fps = 8f },
        new AnimDef { folderName = "walking-8-frames",   shortName = "walk",  loop = true,  frameCount = 8, fps = 8f },
        new AnimDef { folderName = "running-8-frames",   shortName = "run",   loop = true,  frameCount = 8, fps = 8f },
        new AnimDef { folderName = "falling-back-death", shortName = "death", loop = false, frameCount = 7, fps = 8f },
    };

    static readonly string[] Directions = { "north", "north-east", "east", "south-east", "south", "south-west", "west", "north-west" };

    [MenuItem("RIMA/Build Character Animation Clips")]
    public static void BuildAll()
    {
        int built = 0;
        int skipped = 0;
        var errors = new List<string>();

        AssetDatabase.StartAssetEditing();
        try
        {
            foreach (var ch in Characters)
            {
                string chLower = ch.ToLower();
                string outDir = $"Assets/Animations/Characters/{ch}";
                Directory.CreateDirectory(Application.dataPath.Replace("Assets", "") + outDir);

                foreach (var anim in Animations)
                {
                    foreach (var dir in Directions)
                    {
                        string spriteFolder = $"Assets/Sprites/Characters/{ch}/animations/{anim.folderName}/{dir}";
                        string fullFolder = Application.dataPath.Replace("Assets", "") + spriteFolder;

                        if (!Directory.Exists(fullFolder))
                        {
                            skipped++;
                            continue;
                        }

                        var pngFiles = Directory.GetFiles(fullFolder, "*.png")
                            .OrderBy(f => f)
                            .ToArray();

                        if (pngFiles.Length == 0)
                        {
                            skipped++;
                            continue;
                        }

                        // Load sprites
                        var sprites = new List<Sprite>();
                        foreach (var f in pngFiles)
                        {
                            string relPath = $"Assets/Sprites/Characters/{ch}/animations/{anim.folderName}/{dir}/{Path.GetFileName(f)}";
                            // Try single sprite first, then sub-assets
                            Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(relPath);
                            if (sp == null)
                            {
                                var all = AssetDatabase.LoadAllAssetsAtPath(relPath);
                                sp = all.OfType<Sprite>().FirstOrDefault();
                            }
                            if (sp != null)
                                sprites.Add(sp);
                        }

                        if (sprites.Count == 0)
                        {
                            errors.Add($"No sprites loaded: {ch}/{anim.folderName}/{dir}");
                            continue;
                        }

                        // Build clip
                        string clipName = $"{chLower}_{anim.shortName}_{dir}";
                        string clipPath = $"{outDir}/{clipName}.anim";

                        AnimationClip clip = BuildClip(clipName, sprites, anim.fps, anim.loop);

                        // Save or overwrite
                        AnimationClip existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
                        if (existing != null)
                        {
                            EditorUtility.CopySerialized(clip, existing);
                            EditorUtility.SetDirty(existing);
                        }
                        else
                        {
                            AssetDatabase.CreateAsset(clip, clipPath);
                        }
                        built++;
                    }
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string msg = $"Build complete: {built} clips built, {skipped} skipped.";
        if (errors.Count > 0)
            msg += $"\nErrors ({errors.Count}):\n" + string.Join("\n", errors);

        Debug.Log("[CharacterAnimationBuilder] " + msg);
    }

    static AnimationClip BuildClip(string name, List<Sprite> sprites, float fps, bool loop)
    {
        var clip = new AnimationClip();
        clip.name = name;
        clip.frameRate = fps;

        if (loop)
        {
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
        }

        float frameDuration = 1f / fps;
        int frameCount = sprites.Count;

        var keyframes = new ObjectReferenceKeyframe[frameCount + 1];
        for (int i = 0; i < frameCount; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * frameDuration,
                value = sprites[i]
            };
        }
        // Loop-back frame
        keyframes[frameCount] = new ObjectReferenceKeyframe
        {
            time = frameCount * frameDuration,
            value = sprites[0]
        };

        var binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
        return clip;
    }
}
