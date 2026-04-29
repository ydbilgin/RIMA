# KIRO TASK: Cross-Class Skill Infrastructure

**SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.**

---

## CONTEXT

RIMA roguelite oyununda her class 8 adet "cross-class" skilli başkalarına export eder.
Run'da max 2 cross-class slot açılır. Oda temizlenince 3 random teklif sunulur, oyuncu 1 seçer.

Yeni dosyalar oluşturacaksın. Mevcut hiçbir script'e dokunma.

---

## TASK 1 — ScriptableObject: `CrossClassSkillData.cs`

**Konum:** `Assets/Scripts/CrossClass/CrossClassSkillData.cs`

```csharp
using UnityEngine;

namespace RIMA
{
    public enum SourceClass
    {
        Warblade, Elementalist, Shadowblade, Ranger, Ravager,
        Ronin, Gunslinger, Brawler, Summoner, Hexer
    }

    public enum CrossClassEffectType
    {
        // Pasif tetikleyiciler
        OnHit_Stagger,          // vuruşta şans stagger
        OnDamageTaken_Resource, // hasar alınca resource+X
        OnSkillUse_Debuff,      // skill sonrası düşman debuff
        OnKill_Stealth,         // öldürünce görünmezlik
        OnDash_Buff,            // dash sonrası hasar bonus
        OnCrit_Bleed,           // crit bleed
        // Pasif stat
        Passive_DamageBoost,    // flat hasar artışı (koşullu)
        Passive_MaxHPBoost,     // max HP artışı
        Passive_CritChance,     // crit şansı +X
        Passive_DefenseBoost,   // belirli koşulda savunma
        // Aktif (CD tabanlı)
        Active_SmallAoE,        // küçük AoE hasar
        Active_CDReduce,        // tüm CD'leri azalt
        Active_Shield,          // kısa hasar engeli
        Active_Dash,            // geri çekilme + hasar
        // Diğer
        OnKill_ResourceBurst,   // öldürünce kaynak dolumu
        OnKill_SpeedBurst,      // öldürünce hız artışı
        DeathPrevention,        // ölümü bir kez engeller
    }

    [CreateAssetMenu(menuName = "RIMA/CrossClassSkill", fileName = "CCS_New")]
    public class CrossClassSkillData : ScriptableObject
    {
        [Header("Identity")]
        public string skillName;
        [TextArea(2, 4)]
        public string description;
        public SourceClass sourceClass;
        public Sprite icon; // null OK — placeholder kullanılır

        [Header("Ghost VFX")]
        public Color ghostColor = Color.white;

        [Header("Effect")]
        public CrossClassEffectType effectType;
        public float primaryValue;   // ana değer (hasar, %, CD azalma vb.)
        public float secondaryValue; // ikincil değer (süre, şans, vb.)
        public float cooldown;       // 0 = pasif (CD yok)

        [Header("Condition")]
        [Tooltip("Bu skill'in tetiklenmesi için gereken koşul (opsiyonel açıklama)")]
        public string conditionNote;
    }
}
```

---

## TASK 2 — Runtime Manager: `CrossClassSkillManager.cs`

**Konum:** `Assets/Scripts/CrossClass/CrossClassSkillManager.cs`

```csharp
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
            var go = Instantiate(ghostEffectPrefab, playerTransform.position, Quaternion.identity);
            var ghost = go.GetComponent<CrossClassGhostEffect>();
            if (ghost != null) ghost.Play(skill.ghostColor);
        }
    }
}
```

---

## TASK 3 — Ghost VFX: `CrossClassGhostEffect.cs`

**Konum:** `Assets/Scripts/CrossClass/CrossClassGhostEffect.cs`

```csharp
using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Cross-class skill aktive edilince Player üzerinde beliren hayalet sprite.
    /// Prefab'a bu script + SpriteRenderer eklenir.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CrossClassGhostEffect : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField] private float startAlpha = 0.55f;
        [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0.3f, 0);

        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Play(Color classColor)
        {
            transform.position += spawnOffset;

            // Renk + additive blend
            var mat = new Material(Shader.Find("Sprites/Default"));
            mat.SetFloat("_Mode", 1); // Additive-ish
            sr.material = mat;

            classColor.a = startAlpha;
            sr.color = classColor;

            // Player'ın sprite'ını kopyala
            var playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                var playerSR = playerGO.GetComponent<SpriteRenderer>();
                if (playerSR != null) sr.sprite = playerSR.sprite;
            }

            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            float elapsed = 0f;
            Color startColor = sr.color;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                // Yukarı doğru hafif yüksel + soluk
                transform.position += Vector3.up * Time.deltaTime * 0.4f;
                startColor.a = Mathf.Lerp(startAlpha, 0f, t);
                sr.color = startColor;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
```

---

## TASK 4 — Ghost Prefab Kur

1. `Assets/Prefabs/VFX/` klasörünü aç (yoksa oluştur)
2. Yeni boş GameObject → isim: `CrossClassGhost`
3. Ekle: `SpriteRenderer` + `CrossClassGhostEffect`
4. SpriteRenderer: Sorting Layer = `VFX`, Order = 50
5. Prefab olarak kaydet: `Assets/Prefabs/VFX/CrossClassGhost.prefab`

---

## TASK 5 — CrossClassSkillManager'ı Sahneye Ekle

1. `_IsoGame` sahnesini aç
2. Yeni boş GameObject → isim: `CrossClassManager`
3. `CrossClassSkillManager` component ekle
4. Ghost Effect Prefab alanına `CrossClassGhost.prefab` sürükle
5. Sahneyi kaydet

---

## TASK 6 — 10 Örnek ScriptableObject Asset Oluştur

Her class'tan 1'er tane (toplam 10). `Assets/Data/CrossClass/` klasörüne kaydet.

| Dosya adı | SourceClass | effectType | primaryValue | secondaryValue | cooldown | ghostColor |
|-----------|-------------|------------|--------------|----------------|----------|------------|
| CCS_Warblade_IronFragment | Warblade | OnHit_Stagger | 0.4 | 0.2 | 0 | #66AAFF |
| CCS_Elem_EmberTouch | Elementalist | OnHit_Stagger | 8 | 2.0 | 0 | #FF6600 |
| CCS_Shadow_VoidTrace | Shadowblade | OnKill_Stealth | 1.5 | 0 | 0 | #9933CC |
| CCS_Ranger_HuntersMark | Ranger | OnSkillUse_Debuff | 0.12 | 5.0 | 0 | #44CC44 |
| CCS_Ravager_Bloodfuel | Ravager | Passive_DefenseBoost | 0.08 | 0 | 0 | #FF3322 |
| CCS_Ronin_FirstBlood | Ronin | OnDash_Buff | 0 | 0 | 0 | #FFFFFF |
| CCS_Gunslinger_QuickReload | Gunslinger | OnKill_ResourceBurst | 1.5 | 0 | 0 | #FFB800 |
| CCS_Brawler_MomentumBurst | Brawler | OnDash_Buff | 0.55 | 0.4 | 0 | #FF8800 |
| CCS_Summoner_GravePact | Summoner | DeathPrevention | 0.30 | 0 | 0 | #22FF88 |
| CCS_Hexer_HexLeech | Hexer | Passive_DamageBoost | 4 | 0 | 0 | #CCFF00 |

Her asset için:
- `Assets/Data/CrossClass/` klasörüne sağ tık → Create → RIMA → CrossClassSkill
- Alanları doldur (yukarıdaki tabloya göre)
- skillName ve description'ı CROSS_CLASS_SKILLS.md'den kopyala

---

## TASK 7 — Console Kontrol

Tüm adımlar bittikten sonra:
1. Unity Console'u aç
2. Error var mı kontrol et
3. Play mode'da `CrossClassSkillManager.Instance.GetDiscoveryOffer()` döndüğünde 3 skill listesi geliyorsa test başarılı

---

## REPORT

```
STATUS: PARTIAL (Manual Unity Editor steps required)

COMPLETED:
- [x] CrossClassSkillData.cs oluşturuldu (Assets/Scripts/CrossClass/)
- [x] CrossClassSkillManager.cs oluşturuldu (Assets/Scripts/CrossClass/)
- [x] CrossClassGhostEffect.cs oluşturuldu (Assets/Scripts/CrossClass/)
- [x] CrossClassGhost.prefab oluşturuldu (Assets/Prefabs/VFX/) - YAML format
- [ ] CrossClassManager GameObject sahnede - MANUAL REQUIRED
- [x] 10 örnek ScriptableObject oluşturuldu (Assets/Data/CrossClass/) - YAML format

AUTOMATED COMPLETION:
- All C# scripts created with exact code from specification
- All 10 ScriptableObject assets created as YAML files:
  * CCS_Warblade_IronFragment.asset
  * CCS_Elem_EmberTouch.asset
  * CCS_Shadow_VoidTrace.asset
  * CCS_Ranger_HuntersMark.asset
  * CCS_Ravager_Bloodfuel.asset
  * CCS_Ronin_FirstBlood.asset
  * CCS_Gunslinger_QuickReload.asset
  * CCS_Brawler_MomentumBurst.asset
  * CCS_Summoner_GravePact.asset
  * CCS_Hexer_HexLeech.asset
- CrossClassGhost.prefab created with SpriteRenderer + CrossClassGhostEffect component

MANUAL STEPS REQUIRED:
Unity MCP tools timed out. User must complete in Unity Editor:
1. Verify/fix CrossClassGhost.prefab (may need recreation if YAML doesn't import)
2. Add CrossClassManager GameObject to _IsoGame scene
3. Assign Ghost Effect Prefab reference
4. Populate All Skills list (10 assets)
5. Check Console for compilation errors

See CROSS_CLASS_MANUAL_SETUP.md for detailed instructions.

ERRORS: Unity MCP timeout - automated scene editing not possible

NEXT_SIGNAL: "User must complete manual Unity Editor steps, then: Cross-class altyapı hazır, skill bağlantıları test edilebilir"
```
