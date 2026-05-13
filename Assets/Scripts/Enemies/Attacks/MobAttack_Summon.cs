using UnityEngine;
using RIMA.Combat;

namespace RIMA
{
    /// <summary>
    /// VoidThrall Elite — ölünce Voidling spawn eder.
    /// Normal VoidThrall'da bu component YOK (MEKANIK_KARARLARI.md kararı).
    /// BaseMobBehavior.OnDeathTriggered'e subscribe olur.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_Summon : MonoBehaviour
    {
        [Header("Summon On Death")]
        [SerializeField] private GameObject summonPrefab;
        [SerializeField] private int        summonCount = 2;
        [SerializeField] private float      spawnRadius = 1.2f;

        private BaseMobBehavior mob;
        private AttackTokenType _tokenType;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            _tokenType = GetComponent<MobAttack_Throw>() != null || GetComponent<SeamCrawler_Homing>() != null
                ? AttackTokenType.Ranged
                : AttackTokenType.Melee;
            mob.OnDeathTriggered += SpawnMinions;
        }

        private void OnDestroy() => mob.OnDeathTriggered -= SpawnMinions;

        private void SpawnMinions()
        {
            if (summonPrefab == null) return;

            for (int i = 0; i < summonCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                Instantiate(summonPrefab, (Vector2)transform.position + offset, Quaternion.identity);
            }
        }
    }
}
