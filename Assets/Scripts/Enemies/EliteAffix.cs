using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    /// <summary>
    /// Elite enemy affix system — modifies enemy behavior at spawn.
    /// RuntimeRoomManager randomly picks affixes for enemies in later rooms.
    ///
    /// 4 Affixes:
    ///   Shielded  — takes 50% less damage, shield VFX
    ///   Berserker — 40% more damage, 20% more speed, red tint
    ///   Teleporter — blinks to random position every N seconds
    ///   Vampiric  — heals on dealing damage, green tint
    ///
    /// Usage: EliteAffix.Apply(enemyGameObject, AffixType.Berserker);
    /// </summary>
    public enum AffixType
    {
        None,
        Shielded,
        Berserker,
        Teleporter,
        Vampiric
    }

    public class EliteAffix : MonoBehaviour
    {
        [SerializeField] private AffixType affixType = AffixType.None;

        [Header("Shielded")]
        [SerializeField] private float shieldDamageReduction = 0.5f;

        [Header("Berserker")]
        [SerializeField] private float berserkDamageMult = 1.4f;
        [SerializeField] private float berserkSpeedMult = 1.2f;

        [Header("Teleporter")]
        [SerializeField] private float teleportInterval = 4f;
        [SerializeField] private float teleportRadius = 3f;
        [SerializeField] private float teleportCollisionRadius = 0.3f;
        private float teleportTimer;

        [Header("Vampiric")]
        [SerializeField] private float vampiricHealPercent = 0.15f; // 15% of damage dealt

        // Cached
        private Health health;
        private BaseMobBehavior mob;
        private SpriteRenderer spriteRenderer;
        private Tilemap floorTilemap;
        private Tilemap wallTilemap;
        private Color originalColor;
        private bool initialized;

        public AffixType CurrentAffix => affixType;

        /// <summary>
        /// Static helper: add and configure an elite affix on a mob.
        /// Call right after Instantiate in RuntimeRoomManager.
        /// </summary>
        public static EliteAffix Apply(GameObject enemy, AffixType type)
        {
            var affix = enemy.AddComponent<EliteAffix>();
            affix.affixType = type;
            affix.InitAffix();
            return affix;
        }

        /// <summary>
        /// Pick a random affix (for rooms past threshold).
        /// </summary>
        public static AffixType RandomAffix()
        {
            var values = new[] { AffixType.Shielded, AffixType.Berserker, AffixType.Teleporter, AffixType.Vampiric };
            return values[Random.Range(0, values.Length)];
        }

        private void Awake()
        {
            health = GetComponent<Health>();
            mob = GetComponent<BaseMobBehavior>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            if (spriteRenderer != null)
                originalColor = spriteRenderer.color;
        }

        private void Start()
        {
            // If set from Inspector (manual placement)
            if (affixType != AffixType.None)
                InitAffix();
        }

        private void InitAffix()
        {
            if (initialized) return;   // cx: Apply() and Start() both call InitAffix → guard against double HP-scale
            initialized = true;
            // Re-cache if Awake hasn't run yet
            if (health == null) health = GetComponent<Health>();
            if (mob == null) mob = GetComponent<BaseMobBehavior>();
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null) originalColor = spriteRenderer.color;

            switch (affixType)
            {
                case AffixType.Shielded:
                    if (health != null)
                        health.incomingDamageMultiplier = shieldDamageReduction;
                    // Shield visual: cyan tint
                    SetTint(new Color(0.6f, 0.9f, 1f, 1f));
                    // Scale HP up 30%. ScaleMaxHP takes an INT multiplier → RoundToInt(MaxHP*1.3)/MaxHP
                    // integer-divides to 1 (no-op bug). Use SetMaxHP with the computed value instead.
                    if (health != null)
                        health.SetMaxHP(Mathf.RoundToInt(health.MaxHP * 1.3f));
                    break;

                case AffixType.Berserker:
                    // Red tint + more damage/speed handled via mob
                    SetTint(new Color(1f, 0.4f, 0.3f, 1f));
                    break;

                case AffixType.Teleporter:
                    teleportTimer = teleportInterval;
                    // Purple tint
                    SetTint(new Color(0.8f, 0.5f, 1f, 1f));
                    break;

                case AffixType.Vampiric:
                    // Green tint
                    SetTint(new Color(0.4f, 1f, 0.5f, 1f));
                    break;
            }

            Debug.Log($"[EliteAffix] {gameObject.name} → {affixType}");
        }

        private void Update()
        {
            if (health != null && health.IsDead) return;

            switch (affixType)
            {
                case AffixType.Teleporter:
                    teleportTimer -= Time.deltaTime;
                    if (teleportTimer <= 0f)
                    {
                        Teleport();
                        teleportTimer = teleportInterval;
                    }
                    break;
            }
        }

        // ─── Berserker queries (BaseMobBehavior reads these) ──────────

        /// <summary>Damage multiplier for this mob's attacks.</summary>
        public float GetDamageMultiplier()
        {
            return affixType == AffixType.Berserker ? berserkDamageMult : 1f;
        }

        /// <summary>Speed multiplier for this mob's movement.</summary>
        public float GetSpeedMultiplier()
        {
            return affixType == AffixType.Berserker ? berserkSpeedMult : 1f;
        }

        // ─── Vampiric: call from mob attack script ────────────────────

        /// <summary>Call when this mob deals damage. Returns heal amount.</summary>
        public int OnDealtDamage(int damageDealt)
        {
            if (affixType != AffixType.Vampiric || health == null) return 0;
            int healAmount = Mathf.Max(1, Mathf.RoundToInt(damageDealt * vampiricHealPercent));
            health.Heal(healAmount);
            return healAmount;
        }

        // ─── Teleporter ──────────────────────────────────────────────

        private bool _warnedTeleportFail;

        private void Teleport()
        {
            const int maxAttempts = 3;
            LayerMask wallMask = LayerMask.GetMask("Default");

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * teleportRadius;
                Vector3 newPos = transform.position + (Vector3)randomOffset;

                if (!IsTeleportDestinationValid(newPos, wallMask))
                    continue;

                transform.position = newPos;

                // Brief flash
                if (spriteRenderer != null)
                    StartCoroutine(TeleportFlash());
                return;
            }

            // All attempts failed — log once per mob so logs don't spam.
            if (!_warnedTeleportFail)
            {
                Debug.LogWarning($"[EliteAffix] Teleporter on '{gameObject.name}' found no valid walkable destination after {maxAttempts} attempts; staying put.");
                _warnedTeleportFail = true;
            }
        }

        private bool IsTeleportDestinationValid(Vector3 worldPos, LayerMask wallMask)
        {
            // Primary check: authoritative walkability (RoomTemplateSO grid or floor tilemap).
            RIMA.Environment.WalkabilityMap walkMap = RIMA.Environment.WalkabilityMap.Instance;
            if (walkMap != null)
            {
                if (!walkMap.IsWalkableWorld(worldPos))
                    return false;

                // Reachability gate: don't teleport to a disconnected island.
                if (walkMap.floorTilemap != null)
                {
                    Vector3Int cell = walkMap.floorTilemap.WorldToCell(worldPos);
                    if (!walkMap.IsReachableFromPlayer(cell))
                        return false;
                }
            }
            else
            {
                // Fallback: legacy tilemap lookup (no WalkabilityMap in scene).
                ResolveTeleportTilemaps();
                if (floorTilemap != null)
                {
                    Vector3Int cell = floorTilemap.WorldToCell(worldPos);
                    if (floorTilemap.GetTile(cell) == null)
                        return false;

                    if (wallTilemap != null && wallTilemap.GetTile(cell) != null)
                        return false;
                }
            }

            return Physics2D.OverlapCircle(worldPos, teleportCollisionRadius, wallMask) == null;
        }

        private void ResolveTeleportTilemaps()
        {
            if (floorTilemap == null)
                floorTilemap = FindTilemap("IsoGrid/Ground") ?? FindTilemap("Room/Ground") ?? FindTilemap("Grid/GroundTilemap") ?? FindTilemap("GroundTilemap");

            if (wallTilemap == null)
                wallTilemap = FindTilemap("IsoGrid/Walls") ?? FindTilemap("Room/Wall") ?? FindTilemap("Grid/WallsTilemap") ?? FindTilemap("WallsTilemap");
        }

        private static Tilemap FindTilemap(string path)
        {
            GameObject go = GameObject.Find(path);
            return go != null ? go.GetComponent<Tilemap>() : null;
        }

        private System.Collections.IEnumerator TeleportFlash()
        {
            if (spriteRenderer == null) yield break;
            var tint = spriteRenderer.color;
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = tint;
        }

        // ─── Visual ──────────────────────────────────────────────────

        private void SetTint(Color tint)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = tint;
        }

        /// <summary>Get the affix name for UI display.</summary>
        public string GetDisplayName()
        {
            return affixType switch
            {
                AffixType.Shielded   => "Shielded",
                AffixType.Berserker  => "Berserker",
                AffixType.Teleporter => "Teleporter",
                AffixType.Vampiric   => "Vampiric",
                _ => ""
            };
        }
    }
}
