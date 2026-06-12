# 01 — Mevcut Kod Sistemi (Explore analizi)

## ClassType enum (10) — `Assets/Scripts/Skills/SkillData.cs`
Warblade✓, Elementalist✓, Shadowblade✓, Ranger✓, Ronin✓(pilot) = implemented
Ravager, Gunslinger, Brawler, Summoner, Hexer = NOT implemented

## KRİTİK: Birleşik class stat sistemi YOK
- **HP** (`Core/Health.cs`): `maxHP=100` — TÜM class'larda sabit. `incomingDamageMultiplier`, `healMultiplier` var.
- **moveSpeed** (`Player/PlayerController.cs`): `4.5` — TÜM class'larda sabit. dash: 18 spd / 0.15s / 0.8s cd.
- **Per-class stat scaling YOK.** `CharacterClassDefinition` = placeholder (100HP/5spd/10dmg, KULLANILMIYOR).

## Tek farklılaştırıcı = BasicAttackProfile
`Combat/BasicAttack/BasicAttackProfile.cs` + 5 .asset:
| Class | BehaviorType | Combo dmg | Toplam | RMB |
|---|---|---|---|---|
| Warblade | Melee | [25,30,40] | 95 | 34 dmg / 1.5s / 30 rage |
| Elementalist | CastRhythm | [18,18,28] | 64 | — |
| Shadowblade | VeilStrike | [20] | 20 | — |
| Ranger | ShotCadence | [18] | 18 | — |
| Ronin | IaidoStance | varies | — | — |

## Damage formülü (flat) — `Skills/SkillRuntime.cs`
`Final = base × statusMult × incomingDamageMult` (min 1). **Attack/MagicAttack stat'ı YOK, crit YOK, armor YOK.** Skill damage'ları hardcoded int (örn. Fireball 30, DeathBlow 40×4, Ambush 80×3, AimedShot 80).

## Resource sistemleri (class'a göre tamamen farklı) — `Systems/Resources/`
Rage (hit-gain) · Mana (+8/s) · Energy (+15/s) · Focus (mesafe-bazlı) · ComboPoint. `PlayerResourceBase.cs` taban.

## Sonuç
Sistem ilkel/flat. Demo "dengeleme altyapısı" için birleşik stat modelinin **kurulması** gerekiyor — mevcut tek sinyal BasicAttackProfile combo damage.
