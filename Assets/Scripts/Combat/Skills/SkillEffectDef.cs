using UnityEngine;

namespace RIMA.Combat.Skills
{
    public enum SkillEffectAngleMode
    {
        ProjectileRotated,
        ProjectileDirectional8,
        BeamRotated,
        Radial,
        Cone
    }

    public enum SkillEffectConeRenderMode
    {
        Rotated,
        Directional8
    }

    [CreateAssetMenu(fileName = "SkillEffectDef", menuName = "RIMA/Combat/Skill Effect Def")]
    public class SkillEffectDef : ScriptableObject
    {
        [Header("Identity")]
        public string effectId;
        public string displayName;
        [TextArea(2, 4)]
        public string description;
        public ClassType classType = ClassType.None;
        public Sprite icon;

        [Header("Angle")]
        public SkillEffectAngleMode angleMode = SkillEffectAngleMode.ProjectileRotated;
        public SkillEffectConeRenderMode coneRenderMode = SkillEffectConeRenderMode.Rotated;
        public float rotationOffsetDegrees;
        public bool snapRotatedProjectileToEightDirections;

        [Header("Sprites")]
        public Sprite rotatedSprite;
        public Sprite[] directionalSprites = new Sprite[8];
        public Sprite impactSprite;

        [Header("Prefab")]
        public GameObject effectPrefab;
        public GameObject impactPrefab;

        [Header("Playback")]
        public float lifetime = 0.5f;
        public float scale = 1f;
        public int sortingOrderOffset;
        public bool destroyOnImpact = true;

        public bool RequiresDirectionalSprites()
        {
            return angleMode == SkillEffectAngleMode.ProjectileDirectional8 ||
                   (angleMode == SkillEffectAngleMode.Cone && coneRenderMode == SkillEffectConeRenderMode.Directional8);
        }

        public bool UsesRuntimeRotation()
        {
            return angleMode == SkillEffectAngleMode.ProjectileRotated ||
                   angleMode == SkillEffectAngleMode.BeamRotated ||
                   (angleMode == SkillEffectAngleMode.Cone && coneRenderMode == SkillEffectConeRenderMode.Rotated);
        }

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(effectId))
            {
                error = "effectId is required";
                return false;
            }

            if (lifetime <= 0f)
            {
                error = "lifetime must be greater than 0";
                return false;
            }

            if (scale <= 0f)
            {
                error = "scale must be greater than 0";
                return false;
            }

            if (RequiresDirectionalSprites() && (directionalSprites == null || directionalSprites.Length != 8))
            {
                error = "directionalSprites must contain exactly 8 entries";
                return false;
            }

            error = null;
            return true;
        }
    }
}
