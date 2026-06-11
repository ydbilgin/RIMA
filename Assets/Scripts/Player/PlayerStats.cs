using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Player HP ve temel stat yönetimi.
    /// CharacterHPBar ve diğer sistemler buradan okur.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("HP")]
        [SerializeField] private float maxHP = 100f;
        private float currentHP;

        public float MaxHP     => maxHP;
        public float CurrentHP => currentHP;

        private void Awake()
        {
            currentHP = maxHP;
        }

        public void TakeDamage(float amount)
        {
            currentHP = Mathf.Max(0f, currentHP - amount);
        }

        public void Heal(float amount)
        {
            currentHP = Mathf.Min(maxHP, currentHP + amount);
        }

        public void SetMaxHP(float value, bool refill = true)
        {
            maxHP = Mathf.Max(1f, value);
            currentHP = refill ? maxHP : Mathf.Min(currentHP, maxHP);
        }

        public bool IsDead => currentHP <= 0f;
    }
}
