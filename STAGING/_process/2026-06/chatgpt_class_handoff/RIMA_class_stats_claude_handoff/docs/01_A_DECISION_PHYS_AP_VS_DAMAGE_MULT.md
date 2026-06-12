# A — Class Stat Modeli Kararı

## Kısa karar

**Phys/AP split korunmalı.**

Gemini'nin önerdiği tek `damageMult` yaklaşımı sadece debug/presenter override olarak kullanılabilir. Production canon stat modeli tek hasar çarpanına indirgenmemeli.

## Çatışma

### NLM canon

- İki ayrı hasar statı var:
  - Physical Damage / `physPower`
  - Ability Power / `abilityPower`
- Hasar formülü üç ana katmanlı:
  - Identity × Build multiplier, cap ×3.0
  - Taşan hasar Posture/Break hasarına döner
  - Situational multiplier ayrı cap ×2.0
- Class hissi yalnızca HP/moveSpeed farkıyla verilmez.
- Ağır/hızlı farkı finalde animasyon frame, recovery, dash-cancel ve input response ile verilir.
- UI anlatımı 5-bar: Hasar / Dayanıklılık / Hız / Kontrol / Zorluk.

### Gemini önerisi

- `attackPower/magicPower` ayrımını terk et.
- Tek `damageMult` kullan.
- Ability tagleriyle ayrımı çöz.
- Modern roguelite için daha sade olduğunu savunuyor.

## Benim çıkarımım

Gemini'nin sadeleştirme içgüdüsü demo için anlaşılır ama RIMA'nın item/build omurgasıyla çatışıyor.

RIMA'da item sistemi zaten iki damage statına dayalı:

| Item/Component | Stat dili |
|---|---|
| Iron Shard | Phys Damage |
| Void Fragment | Ability Power |
| Vampiric Blade | Phys + sustain |
| Fracture Amp | AP + Crit + RiftMark bonus |
| Surge Catalyst | AP + Attack Speed |
| Arcane Bastion | AP + Armor |
| Rift Piercer | Phys + Crit |

Bu ayrımı tek `damageMult` altında eritmek item kararlarını yüzeyselleştirir. Mage, curse, minion ve physical melee class'ları aynı matematik omurgasına bağlanır. Bu, RIMA'nın class/build identity hedefini zayıflatır.

## Nihai model

Demo için merkezi bir `ClassStatProfile` ScriptableObject oluştur:

```csharp
public enum DamageType
{
    Physical,
    Ability,
    True
}

[CreateAssetMenu(menuName = "RIMA/Balance/Class Stat Profile")]
public class ClassStatProfile : ScriptableObject
{
    public ClassType classType;

    [Header("Runtime Stats")]
    public int maxHP = 100;
    public float physPower = 100f;
    public float abilityPower = 100f;
    public float attackSpeedMult = 1f;
    public float moveSpeed = 4.5f;

    [Header("UI 5-Bar")]
    [Range(1,5)] public int damage;
    [Range(1,5)] public int durability;
    [Range(1,5)] public int speed;
    [Range(1,5)] public int control;
    [Range(1,5)] public int difficulty;

    [Header("Debug Only")]
    public float debugGlobalDamageMult = 1f;
}
```

## Hasar formülü

```txt
FinalDamage =
    baseDamage
    × classStatMultiplier
    × identityBuildMultiplierCapped
    × situationalMultiplier
    × debugGlobalDamageMult
    × incomingDamageMultiplier
```

### Katmanlar

| Katman | Açıklama |
|---|---|
| `baseDamage` | SkillData / BasicAttackProfile / projectile asset'ten gelir |
| `classStatMultiplier` | Physical ise `physPower / 100`, Ability ise `abilityPower / 100` |
| `identityBuildMultiplierCapped` | Build sinerjisi, cap ×3.0 |
| `overflowToPosture` | ×3.0 üstü hasar Posture/Break hasarına çevrilir |
| `situationalMultiplier` | Weak point, trap-trigger, Scar collapse, Opened target gibi koşullar, ayrı cap ×2.0 |
| `debugGlobalDamageMult` | Sadece demo/debug override |
| `incomingDamageMultiplier` | Health tarafında var olan savunma/gelen hasar çarpanı |

## Neden `damageMult` production stat olmamalı?

Tek `damageMult` bu problemleri doğurur:

1. Phys item ve AP item arasındaki karar anlamını azaltır.
2. Elementalist, Summoner, Hexer aynı matematikte sadece farklı efektli DPS class'ına dönüşür.
3. Warblade/Ravager/Brawler gibi physical bruiser kimlikleri item tarafında mage class'larla fazla örtüşür.
4. Legendary build matrix'te component path ayrımı zayıflar.
5. Cross-class sistemde primary/secondary davranış ayrımı yerine tek sayı büyütme oyunu oluşur.

## Nerede kullanılabilir?

`damageMult` şu amaçla kalabilir:

```csharp
public float debugGlobalDamageMult = 1f;
```

Kullanım:
- Presenter mode'da "tüm hasarı %20 artır" slider'ı.
- QA testinde encounter hızlandırma.
- Build kırılmasını hızlı göstermek.
- Ancak item, class veya skill statı olarak kullanılmaz.

## Demo için uygulanacak minimum set

| Stat | Demo görevi | Final yorumu |
|---|---|---|
| `maxHP` | Class dayanıklılığı farkını gösterir | Farklar dar tutulmalı |
| `physPower` | Physical skill/basic scaling | Canon stat |
| `abilityPower` | Spell/minion/curse scaling | Canon stat |
| `attackSpeedMult` | Basic cadence ve demo tempo farkı | Finalde animasyonla desteklenecek |
| `moveSpeed` | Demo placeholder | Finalde sınıf hissini tek başına taşımaz |

## Kesin uyarı

Class identity sadece `maxHP` ve `moveSpeed` farkıyla çözülmemeli. Demo için bu farklar kullanılabilir, ama final his animasyon timing ve skill state interaction üzerinden kurulmalı.
