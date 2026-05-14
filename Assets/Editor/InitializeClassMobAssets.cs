using System.Collections.Generic;
using System.IO;
using RIMA.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public static class InitializeClassMobAssets
    {
        private const string ClassFolder = "Assets/Data/Classes";
        private const string MobFolder = "Assets/Data/Mobs/F1";

        private class ClassSpec
        {
            public string name;
            public string anchor;
            public Vector2Int weaponCanvas;
            public bool weaponDecoupled;
            public string[] passiveAccessories;
            public string role;
        }

        private class MobSpec
        {
            public string name;
            public MobRole role;
            public Vector2Int canvasSize;
            public string silhouette;
            public bool isFlying;
            public bool isElite;
            public bool hasIntegratedWeapon;
        }

        private static readonly ClassSpec[] Classes = new[]
        {
            new ClassSpec { name = "Warblade",     anchor = "warblade",     weaponCanvas = new Vector2Int(56, 20), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Two-handed greatsword fighter, mid-range melee" },
            new ClassSpec { name = "Ranger",       anchor = "ranger",       weaponCanvas = new Vector2Int(48, 56), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Compound bow scout, tactical ranged" },
            new ClassSpec { name = "Shadowblade", anchor = "shadowblade",  weaponCanvas = new Vector2Int(24, 24), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Twin reverse daggers, ambush burst" },
            new ClassSpec { name = "Elementalist", anchor = "elementalist", weaponCanvas = new Vector2Int(0, 0),   weaponDecoupled = false, passiveAccessories = new string[0],         role = "Disc caster (Unity VFX, no weapon sprite, Karar #59)" },
            new ClassSpec { name = "Ravager",      anchor = "ravager",      weaponCanvas = new Vector2Int(28, 28), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Dual hatchets berserker, sustained melee" },
            new ClassSpec { name = "Ronin",        anchor = "ronin",        weaponCanvas = new Vector2Int(56, 20), weaponDecoupled = true,  passiveAccessories = new[] { "scabbard" }, role = "Katana iaido duelist, sheath/draw burst" },
            new ClassSpec { name = "Gunslinger",   anchor = "gunslinger",   weaponCanvas = new Vector2Int(24, 20), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Dual pistols, ranged kiter" },
            new ClassSpec { name = "Brawler",      anchor = "brawler",      weaponCanvas = new Vector2Int(0, 0),   weaponDecoupled = false, passiveAccessories = new string[0],         role = "Bare-fisted boxer (no weapon sprite)" },
            new ClassSpec { name = "Summoner",     anchor = "summoner",     weaponCanvas = new Vector2Int(28, 32), weaponDecoupled = true,  passiveAccessories = new string[0],         role = "Soul lantern, spirit-call support" },
            new ClassSpec { name = "Hexer",        anchor = "hexer",        weaponCanvas = new Vector2Int(48, 56), weaponDecoupled = true,  passiveAccessories = new[] { "grimoire" }, role = "Curse staff (grimoire passive body accessory, Karar #18 + #123)" }
        };

        private static readonly MobSpec[] Mobs = new[]
        {
            new MobSpec { name = "SeamCrawler",   role = MobRole.Swarm,    canvasSize = new Vector2Int(64, 64), silhouette = "Hunched quadruped husk dragging fractured ribcage shell",                                isFlying = false, isElite = false, hasIntegratedWeapon = false },
            new MobSpec { name = "PlateWidow",    role = MobRole.Elite,    canvasSize = new Vector2Int(80, 80), silhouette = "Broad armored matriarch, asymmetric pauldron weight, halberd-stub geometry",            isFlying = false, isElite = true,  hasIntegratedWeapon = false },
            new MobSpec { name = "RelicCaster",   role = MobRole.Caster,   canvasSize = new Vector2Int(64, 64), silhouette = "Thin hooded cultist cradling floating shard at chest, tall narrow",                     isFlying = false, isElite = false, hasIntegratedWeapon = false },
            new MobSpec { name = "RiftHound",     role = MobRole.Pack,     canvasSize = new Vector2Int(96, 96), silhouette = "Low predator, head split vertically into two jaw-halves, slides on rift",               isFlying = false, isElite = false, hasIntegratedWeapon = false },
            new MobSpec { name = "HollowArbiter", role = MobRole.MiniBoss, canvasSize = new Vector2Int(96, 96), silhouette = "Tall crowned humanoid, broken sword fused to forearm, heavy cape silhouette",          isFlying = false, isElite = true,  hasIntegratedWeapon = true },
            new MobSpec { name = "SpireChoirling",role = MobRole.Support,  canvasSize = new Vector2Int(64, 64), silhouette = "Suspended levitating torso, arms outstretched horizontal, torn cloth trails downward", isFlying = true,  isElite = false, hasIntegratedWeapon = false }
        };

        [MenuItem("RIMA/Tools/Initialize Class + Mob Definition Assets")]
        public static void Initialize()
        {
            EnsureFolder("Assets/Data");
            EnsureFolder(ClassFolder);
            EnsureFolder("Assets/Data/Mobs");
            EnsureFolder(MobFolder);

            int created = 0;
            int updated = 0;

            foreach (ClassSpec spec in Classes)
            {
                string path = $"{ClassFolder}/{spec.name}.asset";
                CharacterClassDefinition def = AssetDatabase.LoadAssetAtPath<CharacterClassDefinition>(path);
                if (def == null)
                {
                    def = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                    AssetDatabase.CreateAsset(def, path);
                    created++;
                }
                else
                {
                    updated++;
                }

                def.className = spec.name;
                def.anchorName = spec.anchor;
                def.roleDescription = spec.role;
                def.weaponCanvas = spec.weaponCanvas;
                def.weaponDecoupled = spec.weaponDecoupled;
                def.passiveAccessories = new List<string>(spec.passiveAccessories);
                EditorUtility.SetDirty(def);
            }

            foreach (MobSpec spec in Mobs)
            {
                string path = $"{MobFolder}/{spec.name}.asset";
                MobDefinition def = AssetDatabase.LoadAssetAtPath<MobDefinition>(path);
                if (def == null)
                {
                    def = ScriptableObject.CreateInstance<MobDefinition>();
                    AssetDatabase.CreateAsset(def, path);
                    created++;
                }
                else
                {
                    updated++;
                }

                def.mobName = spec.name;
                def.role = spec.role;
                def.canvasSize = spec.canvasSize;
                def.silhouette = spec.silhouette;
                def.isFlying = spec.isFlying;
                def.isElite = spec.isElite;
                def.hasIntegratedWeapon = spec.hasIntegratedWeapon;
                def.biomeKey = "F1";
                def.riftPaletteAccent = "cyan #00FFCC + violet #5A2A8A";
                EditorUtility.SetDirty(def);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[RIMA] InitializeClassMobAssets done: created={created}, updated={updated}, classes={Classes.Length}, mobs={Mobs.Length}");
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
