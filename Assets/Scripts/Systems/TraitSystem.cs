using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Trait sistemi — saf sayısal bonuslar, mekanik yok.
    /// Kaynak: Sandık + Forge (Faz 2).
    /// Her trait max 3 kez alınabilir (stacks).
    /// </summary>
    public class TraitSystem : MonoBehaviour
    {
        public static TraitSystem Instance { get; private set; }

        // Trait stack sayıları
        private Dictionary<string, int> traitStacks = new Dictionary<string, int>();

        // Component refs
        private Health playerHealth;
        private RageSystem playerRage;
        private PlayerAttack playerAttack;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                playerRage = player.GetComponent<RageSystem>();
                playerAttack = player.GetComponent<PlayerAttack>();
            }
        }

        /// <summary>
        /// Trait ekle (sandık veya forge'dan).
        /// </summary>
        public bool AddTrait(string traitName)
        {
            if (!IsValidTrait(traitName))
            {
                Debug.LogWarning($"[TraitSystem] Geçersiz trait: {traitName}");
                return false;
            }

            int currentStacks = GetTraitStacks(traitName);
            int maxStacks = GetMaxStacks(traitName);

            if (currentStacks >= maxStacks)
            {
                Debug.LogWarning($"[TraitSystem] {traitName} zaten max stack ({maxStacks})");
                return false;
            }

            // Stack ekle
            if (!traitStacks.ContainsKey(traitName))
                traitStacks[traitName] = 0;

            traitStacks[traitName]++;
            ApplyTraitBonus(traitName);

            Debug.Log($"[TraitSystem] {traitName} eklendi (stack: {traitStacks[traitName]}/{maxStacks})");
            return true;
        }

        /// <summary>
        /// Trait stack sayısını döndür.
        /// </summary>
        public int GetTraitStacks(string traitName)
        {
            return traitStacks.ContainsKey(traitName) ? traitStacks[traitName] : 0;
        }

        /// <summary>
        /// Tüm aktif traitleri döndür.
        /// </summary>
        public Dictionary<string, int> GetAllTraits()
        {
            return new Dictionary<string, int>(traitStacks);
        }

        /// <summary>
        /// Trait bonusunu uygula.
        /// </summary>
        private void ApplyTraitBonus(string traitName)
        {
            switch (traitName)
            {
                case "Toughened Hide":
                    if (playerHealth != null)
                        playerHealth.AddMaxHP(20);
                    break;

                case "Honed Reflexes":
                    // CD azalma — skill controller'lara uygulanacak
                    // TODO: Skill controller'lara global CD multiplier eklenince buraya bağlanacak
                    Debug.Log("[TraitSystem] Honed Reflexes: CD -5% (henüz uygulanmadı)");
                    break;

                case "Iron Will":
                    // CC direnci — StatusEffectSystem'e uygulanacak
                    // TODO: StatusEffectSystem'e CC duration multiplier eklenince buraya bağlanacak
                    Debug.Log("[TraitSystem] Iron Will: CC süresi -20% (henüz uygulanmadı)");
                    break;

                case "Deep Reserves":
                    if (playerRage != null)
                        playerRage.AddMaxRage(15);
                    break;

                case "Stoic Endurance":
                    // Savaş dışı HP regen — Health.cs'e uygulanacak
                    // TODO: Health.cs'e out-of-combat regen eklenince buraya bağlanacak
                    Debug.Log("[TraitSystem] Stoic Endurance: +1 HP/s regen (henüz uygulanmadı)");
                    break;

                case "Killing Momentum":
                    // Kill sonrası hız bonusu — event-based
                    // TODO: Kill event'i geldiğinde tetiklenecek
                    Debug.Log("[TraitSystem] Killing Momentum: Kill → 3s +15% hız (henüz uygulanmadı)");
                    break;
            }
        }

        /// <summary>
        /// Trait geçerli mi?
        /// </summary>
        private bool IsValidTrait(string traitName)
        {
            return traitName switch
            {
                "Toughened Hide" => true,
                "Honed Reflexes" => true,
                "Iron Will" => true,
                "Deep Reserves" => true,
                "Stoic Endurance" => true,
                "Killing Momentum" => true,
                _ => false
            };
        }

        /// <summary>
        /// Trait'in max stack sayısı.
        /// </summary>
        private int GetMaxStacks(string traitName)
        {
            return traitName switch
            {
                "Toughened Hide" => 3,
                "Honed Reflexes" => 2,
                "Iron Will" => 2,
                "Deep Reserves" => 3,
                "Stoic Endurance" => 3,
                "Killing Momentum" => 3,
                _ => 1
            };
        }

        /// <summary>
        /// Trait açıklaması.
        /// </summary>
        public static string GetTraitDescription(string traitName)
        {
            return traitName switch
            {
                "Toughened Hide" => "Max HP +20 (her stack)",
                "Honed Reflexes" => "Tüm skill CD -%5 (her stack)",
                "Iron Will" => "CC süresi -%20 (her stack)",
                "Deep Reserves" => "Max Rage +15 (her stack)",
                "Stoic Endurance" => "Savaş dışı HP regen +1/s (her stack)",
                "Killing Momentum" => "Kill sonrası 3s +%15 hız (her stack)",
                _ => "Bilinmeyen trait"
            };
        }

        /// <summary>
        /// Rastgele trait seç (sandık için).
        /// </summary>
        public static string GetRandomTrait()
        {
            string[] allTraits = {
                "Toughened Hide",
                "Honed Reflexes",
                "Iron Will",
                "Deep Reserves",
                "Stoic Endurance",
                "Killing Momentum"
            };

            return allTraits[Random.Range(0, allTraits.Length)];
        }

        /// <summary>
        /// Trait ağırlıklı seçim (max stack'e ulaşmamış olanlar).
        /// </summary>
        public string GetWeightedRandomTrait()
        {
            string[] allTraits = {
                "Toughened Hide",
                "Honed Reflexes",
                "Iron Will",
                "Deep Reserves",
                "Stoic Endurance",
                "Killing Momentum"
            };

            // Max stack'e ulaşmamış traitleri filtrele
            var available = new List<string>();
            foreach (var trait in allTraits)
            {
                if (GetTraitStacks(trait) < GetMaxStacks(trait))
                    available.Add(trait);
            }

            if (available.Count == 0)
            {
                Debug.LogWarning("[TraitSystem] Tüm traitler max stack'te!");
                return allTraits[Random.Range(0, allTraits.Length)];
            }

            return available[Random.Range(0, available.Count)];
        }

        // ── Kill Event Handler (Killing Momentum için) ──────────

        public void OnKill()
        {
            int stacks = GetTraitStacks("Killing Momentum");
            if (stacks > 0)
            {
                float speedBonus = stacks * 0.15f;
                // TODO: PlayerController'a speed buff uygula
                Debug.Log($"[TraitSystem] Killing Momentum: +{speedBonus * 100}% hız (3s)");
            }
        }
    }
}
