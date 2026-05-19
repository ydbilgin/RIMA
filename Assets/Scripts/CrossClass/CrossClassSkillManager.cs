using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RIMA
{
    /// <summary>
    /// Run boyunca cross-class skill slot'larını yönetir.
    /// Singleton. PlayerController bulduktan sonra Init() çağrılır.
    /// </summary>
    public class CrossClassSkillManager : MonoBehaviour
    {
        public static CrossClassSkillManager Instance { get; private set; }

        [Header("Pool — Inspector'dan doldur")]
        [SerializeField] private List<CrossClassSkillData> allSkills = new();

        [Header("Slots (max 2)")]
        public CrossClassSkillData slot1;
        public CrossClassSkillData slot2;

        // CD takibi
        private float slot1CD;
        private float slot2CD;

        // Ghost effect prefab
        [SerializeField] private GameObject ghostEffectPrefab;

        private Transform playerTransform;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void Init(Transform player)
        {
            playerTransform = player;
        }

        private void Update()
        {
            if (slot1CD > 0) slot1CD -= Time.deltaTime;
            if (slot2CD > 0) slot2CD -= Time.deltaTime;
        }

        // ─────────────────────────────────────────────
        // Discovery — 3 random teklif üret (farklı class'lardan)
        // ─────────────────────────────────────────────
        public List<CrossClassSkillData> GetDiscoveryOffer()
        {
            // Zaten alınmış class'ları hariç tut
            var excluded = new HashSet<SourceClass>();
            if (slot1 != null) excluded.Add(slot1.sourceClass);
            if (slot2 != null) excluded.Add(slot2.sourceClass);

            var pool = allSkills
                .Where(s => !excluded.Contains(s.sourceClass))
                .ToList();

            // Shuffle
            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (pool[i], pool[j]) = (pool[j], pool[i]);
            }

            // Her class'tan max 1 olacak şekilde 3 al
            var result = new List<CrossClassSkillData>();
            var seen = new HashSet<SourceClass>();
            foreach (var s in pool)
            {
                if (seen.Contains(s.sourceClass)) continue;
                seen.Add(s.sourceClass);
                result.Add(s);
                if (result.Count == 3) break;
            }
            return result;
        }

        // ─────────────────────────────────────────────
        // Equip
        // ─────────────────────────────────────────────
        public bool EquipSkill(CrossClassSkillData skill)
        {
            if (slot1 == null) { slot1 = skill; return true; }
            if (slot2 == null) { slot2 = skill; return true; }
            return false; // dolu
        }

        // ─────────────────────────────────────────────
        // Activate (Active tip skilleri için)
        // ─────────────────────────────────────────────
        public void ActivateSlot(int slotIndex)
        {
            var skill = slotIndex == 1 ? slot1 : slot2;
            var cdRef = slotIndex == 1 ? slot1CD : slot2CD;
            if (skill == null || cdRef > 0 || skill.cooldown <= 0) return;

            ApplyEffect(skill);

            if (slotIndex == 1) slot1CD = skill.cooldown;
            else slot2CD = skill.cooldown;
        }

        // ─────────────────────────────────────────────
        // Passive hooks — PlayerAttack/PlayerController bunları çağırır
        // ─────────────────────────────────────────────
        public void OnHit(GameObject target) => CheckPassive(CrossClassEffectType.OnHit_Stagger, target);
        public void OnDamageTaken(int amount) => CheckPassiveGeneric(CrossClassEffectType.OnDamageTaken_Resource);
        public void OnKill(GameObject target)
        {
            CheckPassive(CrossClassEffectType.OnKill_Stealth, target);
            CheckPassive(CrossClassEffectType.OnKill_SpeedBurst, target);
            CheckPassive(CrossClassEffectType.OnKill_ResourceBurst, target);
            CheckPassiveGeneric(CrossClassEffectType.DeathPrevention);
        }
        public void OnDash() => CheckPassiveGeneric(CrossClassEffectType.OnDash_Buff);
        public void OnCrit(GameObject target) => CheckPassive(CrossClassEffectType.OnCrit_Bleed, target);
        public void OnSkillUse(GameObject nearestEnemy) => CheckPassive(CrossClassEffectType.OnSkillUse_Debuff, nearestEnemy);

        public void TriggerWarbladeBeat3RoninQuickdraw(Vector2 origin)
        {
            foreach (var ronin in FindObjectsByType<RoninController>(FindObjectsSortMode.None))
            {
                if (ronin == null || !ronin.enabled) continue;
                if (Vector2.Distance(origin, ronin.transform.position) > 8f) continue;

                ronin.TriggerQuickdrawGhost(origin);
                SpawnGhost(new Color(0.42f, 0.95f, 1f, 0.55f), ronin.transform.position);
            }
        }

        // ─────────────────────────────────────────────
        // Internal
        // ─────────────────────────────────────────────
        private void CheckPassive(CrossClassEffectType type, GameObject target)
        {
            if (slot1 != null && slot1.cooldown <= 0 && slot1.effectType == type) ApplyEffect(slot1, target);
            if (slot2 != null && slot2.cooldown <= 0 && slot2.effectType == type) ApplyEffect(slot2, target);
        }

        private void CheckPassiveGeneric(CrossClassEffectType type)
        {
            if (slot1 != null && slot1.cooldown <= 0 && slot1.effectType == type) ApplyEffect(slot1);
            if (slot2 != null && slot2.cooldown <= 0 && slot2.effectType == type) ApplyEffect(slot2);
        }

        private void ApplyEffect(CrossClassSkillData skill, GameObject target = null)
        {
            // Ghost VFX
            SpawnGhost(skill);

            // Efekt uygulaması — şimdilik stub, ilerisi genişler
            Debug.Log($"[CrossClass] Applied: {skill.skillName} ({skill.effectType})");

            // TODO: Efekt tipleri genişledikçe buraya case'ler eklenir
            // Örnek: OnKill_Stealth → playerRenderer.color.a = 0.2f için 1.5s coroutine
        }

        private void SpawnGhost(CrossClassSkillData skill)
        {
            if (ghostEffectPrefab == null || playerTransform == null) return;
            SpawnGhost(skill.ghostColor, playerTransform.position);
        }

        private void SpawnGhost(Color color, Vector3 position)
        {
            if (ghostEffectPrefab == null) return;
            var go = Instantiate(ghostEffectPrefab, position, Quaternion.identity);
            var ghost = go.GetComponent<CrossClassGhostEffect>();
            if (ghost != null) ghost.Play(color);
        }
    }
}
