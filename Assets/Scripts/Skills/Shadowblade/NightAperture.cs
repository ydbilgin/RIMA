using UnityEngine;

namespace RIMA
{
    public class NightAperture : ShadowbladeSkillBase
    {
        [SerializeField] private float duration = 6f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Night Aperture";
            cooldown = 18f;
            resourceCost = 40;
        }

        protected override void Execute()
        {
            SkillRuntime.State(gameObject)?.Apply("NightAperture", duration, 1, 1);
            shadow?.AddSever(20);
        }
    }
}
