using UnityEngine;
using RIMA.Balance;

// Add these fields to the existing BasicAttackProfile instead of replacing the whole file blindly.
public partial class BasicAttackProfile : ScriptableObject
{
    [Header("Damage Scaling")]
    public DamageType lmbDamageType = DamageType.Physical;
    public DamageType rmbDamageType = DamageType.Physical;
    public DamageSourceType lmbSourceType = DamageSourceType.LMB;
    public DamageSourceType rmbSourceType = DamageSourceType.RMB;
}

/*
Recommended asset defaults:

Warblade:      LMB Physical, RMB Physical
Elementalist:  LMB Ability,  RMB Ability
Shadowblade:   LMB Physical, RMB Physical/Ability depending on current Veil Flicker implementation
Ranger:        LMB Physical, RMB Physical
Ronin:         LMB Physical, RMB Physical
*/
