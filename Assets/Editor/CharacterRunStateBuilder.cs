using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Linq;

public class CharacterRunStateBuilder
{
    static readonly string[] Characters = { "Shadowblade", "Elementalist", "Ranger" };

    // 8-dir order matching existing BlendTrees: N, NE, E, SE, S, SW, W, NW
    static readonly (string dir, float x, float y)[] DirData = new[]
    {
        ("north",      0f,      1f),
        ("north-east", 0.707f,  0.707f),
        ("east",       1f,      0f),
        ("south-east", 0.707f, -0.707f),
        ("south",      0f,     -1f),
        ("south-west", -0.707f,-0.707f),
        ("west",      -1f,      0f),
        ("north-west", -0.707f, 0.707f),
    };

    [MenuItem("RIMA/Add Run State to Controllers")]
    public static void AddRunStateAll()
    {
        int done = 0;
        foreach (var ch in Characters)
        {
            string controllerPath = $"Assets/Animations/Characters/{ch}/{ch}.controller";
            var ctrl = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (ctrl == null)
            {
                Debug.LogWarning($"[RunStateBuilder] Controller not found: {controllerPath}");
                continue;
            }

            AddRunState(ctrl, ch);
            EditorUtility.SetDirty(ctrl);
            done++;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        string msg = $"Run state added to {done}/{Characters.Length} controllers.";
        Debug.Log("[RunStateBuilder] " + msg);
    }

    static void AddRunState(AnimatorController ctrl, string charName)
    {
        var sm = ctrl.layers[0].stateMachine;

        // Check if Run state already exists
        var existingRun = sm.states.FirstOrDefault(s => s.state.name == "Run");
        if (existingRun.state != null)
        {
            Debug.Log($"[RunStateBuilder] {charName}: Run state already exists, rebuilding BlendTree.");
            // Remove old state and re-add
            sm.RemoveState(existingRun.state);
        }

        // Find Walk state to copy position
        var walkState = sm.states.FirstOrDefault(s => s.state.name == "Walk").state;
        var idleState = sm.states.FirstOrDefault(s => s.state.name == "Idle").state;

        // Build BlendTree
        AnimatorState runState;
        BlendTree bt;
        runState = ctrl.CreateBlendTreeInController("Run", out bt, 0);
        bt.blendType = BlendTreeType.SimpleDirectional2D;
        bt.blendParameter = "DirX";
        bt.blendParameterY = "DirY";
        bt.name = "Run";

        string chLower = charName.ToLower();
        foreach (var (dir, x, y) in DirData)
        {
            string clipPath = $"Assets/Animations/Characters/{charName}/{chLower}_run_{dir}.anim";
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            if (clip == null)
            {
                Debug.LogWarning($"[RunStateBuilder] Missing clip: {clipPath}");
                continue;
            }
            bt.AddChild(clip, new Vector2(x, y));
        }

        runState.name = "Run";

        // --- Transitions from Walk to Run ---
        if (walkState != null)
        {
            var w2r = walkState.AddTransition(runState);
            w2r.hasExitTime = false;
            w2r.duration = 0f;
            w2r.AddCondition(AnimatorConditionMode.Greater, 0.7f, "Speed");

            // Run back to Walk
            var r2w = runState.AddTransition(walkState);
            r2w.hasExitTime = false;
            r2w.duration = 0f;
            r2w.AddCondition(AnimatorConditionMode.Less, 0.7f, "Speed");
            r2w.AddCondition(AnimatorConditionMode.Greater, 0.05f, "Speed");
        }

        // Run to Idle
        if (idleState != null)
        {
            var r2i = runState.AddTransition(idleState);
            r2i.hasExitTime = false;
            r2i.duration = 0f;
            r2i.AddCondition(AnimatorConditionMode.Less, 0.05f, "Speed");
        }

        // Idle to Run (direct jump for high speed)
        if (idleState != null)
        {
            var i2r = idleState.AddTransition(runState);
            i2r.hasExitTime = false;
            i2r.duration = 0f;
            i2r.AddCondition(AnimatorConditionMode.Greater, 0.7f, "Speed");
        }

        Debug.Log($"[RunStateBuilder] {charName}: Run state added with {bt.children.Length} directions.");
    }
}
