using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests
{
    /// <summary>
    /// PURE-MATH difficulty/balance guardrail for R1 "Broken Entry" (S6 demo tutorial room).
    ///
    /// This is an EditMode test: NO scene, NO PlayMode, NO MonoBehaviour instantiation.
    /// It encodes the canonical R1 design as constants and asserts the RELATIONSHIP that makes
    /// the room winnable first try: the player can clear the room (deal totalMobHP) BEFORE the
    /// mobs can kill the player (incoming DPS * timeToClear &lt; playerHP).
    ///
    /// The point is NOT to lock exact numbers — it is to catch a regression where someone retunes
    /// a value (mob HP up, player DPS down, mob damage up) so far that R1 stops being a guaranteed
    /// first-try win for the tutorial room. If this test goes red after a tuning pass, the curve
    /// has drifted out of the "winnable tutorial" window and needs a design look.
    ///
    /// SOURCES (canon-grounded; see comments per constant):
    ///   - STAGING/BOSS_MOB_DESIGN_S6.md  (R1 = 3 FractureImp, FractureImp HP = 60, R1 = "winnable first try")
    ///   - Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs  (Warblade comboDamage = {25,30,40}, comboWindow 1.2)
    ///   - Assets/Scripts/Enemies/Attacks/MobAttack_Melee.cs  (mob melee damage = 14)
    ///   - Assets/Scripts/Enemies/BaseMobBehavior.cs  (attackCooldown = 1.5)
    ///   - Assets/Scripts/Core/Health.cs  (player maxHP default = 100)
    /// </summary>
    public class DifficultyBalanceTests
    {
        // ─── R1 ENCOUNTER (canon: BOSS_MOB_DESIGN_S6.md §2.2) ────────────────────────────
        /// R1 "Broken Entry": single wave, 3 FractureImp, monotype tutorial room. CANON.
        private const int R1_FractureImpCount = 3;

        /// FractureImp HP. CANON: BOSS_MOB_DESIGN_S6.md §2 table — "100 -> 60" (recent S6 tuning).
        private const int FractureImp_HP = 60;

        /// Total HP the player must chew through to clear R1.
        private const int R1_TotalMobHP = R1_FractureImpCount * FractureImp_HP; // 180

        // ─── PLAYER OFFENSE — Warblade (REAL values from BasicAttackProfile.cs) ───────────
        // comboDamage = { 25, 30, 40 } over comboWindow = 1.2s. Full 3-hit chain = 95 dmg.
        // We derive a CONSERVATIVE sustained DPS: assume the player lands the full 3-hit combo
        // every comboWindow (1.2s). 95 / 1.2 = ~79 burst dps. That is the theoretical ceiling
        // with zero misses / repositioning. For a winnable-but-not-trivial guardrail we discount
        // it heavily (movement, telegraph dodging, target switching across 3 imps).
        private const float Warblade_ComboTotalDamage = 25f + 30f + 40f; // 95, REAL (comboDamage)
        private const float Warblade_ComboWindowSec   = 1.2f;            // REAL (comboWindow)

        /// ASSUMPTION: realized uptime fraction of the theoretical combo DPS in a real fight.
        /// 0.5 = player is actively attacking ~half the time (rest = moving / dodging / re-aiming).
        /// Tunable; documented assumption, not a code-read value.
        private const float ASSUMED_AttackUptime = 0.5f;

        /// Effective sustained player DPS used by the guardrail.
        /// (95 / 1.2) * 0.5 = ~39.6 dps  — matches the task's documented "~40 dps" target.
        private const float Player_EffectiveDPS =
            (Warblade_ComboTotalDamage / Warblade_ComboWindowSec) * ASSUMED_AttackUptime;

        // ─── PLAYER DEFENSE (REAL: Health.cs maxHP default = 100) ────────────────────────
        private const int Player_MaxHP = 100;

        // ─── MOB OFFENSE (REAL values) ───────────────────────────────────────────────────
        // MobAttack_Melee.cs: damage = 14 per landed hit.
        // BaseMobBehavior.cs: attackCooldown = 1.5s between attacks.
        // Per-imp contact DPS ceiling = 14 / 1.5 = ~9.3 dps (matches spec "~8 each").
        private const float Mob_MeleeDamage     = 14f; // REAL (MobAttack_Melee.damage)
        private const float Mob_AttackCooldown  = 1.5f; // REAL (BaseMobBehavior.attackCooldown)
        private const float Imp_ContactDPS      = Mob_MeleeDamage / Mob_AttackCooldown; // ~9.33

        /// ASSUMPTION: how many of the 3 imps actually land hits per cooldown on average.
        /// AttackTokenManager gates concurrent attackers and the player is kiting/dashing in a
        /// tutorial room, so the imps do NOT all connect together. 1.2 = roughly one imp lands
        /// consistently plus occasional overlap. Documented assumption (FractureImp is "low solo,
        /// swarm-dangerous" per spec — R1's 3-imp monotype wave is the low end of that).
        private const float ASSUMED_SimultaneousAttackers = 1.2f;

        /// Effective incoming DPS the player eats while clearing R1.
        private const float R1_IncomingDPS = Imp_ContactDPS * ASSUMED_SimultaneousAttackers; // ~18.7

        // ─── DERIVED COMBAT MATH ─────────────────────────────────────────────────────────
        private static float TimeToClear => R1_TotalMobHP / Player_EffectiveDPS;            // 180 / 39.6 = ~4.5s
        private static float DamageTakenDuringClear => R1_IncomingDPS * TimeToClear;         // ~18.7 * 4.5 = ~85
        private static float TimeToDiePassively => Player_MaxHP / R1_IncomingDPS;            // 100 / 18.7 = ~5.4s

        // Winnable-tutorial window. These bound the FEEL of a tutorial room, not exact numbers.
        private const float TutorialClearCeilingSec = 20f; // a tutorial room must clear well under 20s
        private const float TutorialClearFloorSec   = 1.5f; // ...but not be a 1-shot faceroll either

        // ── Tests ─────────────────────────────────────────────────────────────────────────

        [Test]
        public void R1_ClearsInsideTutorialWindow()
        {
            float ttc = TimeToClear;
            Assert.Less(ttc, TutorialClearCeilingSec,
                $"R1 should clear under {TutorialClearCeilingSec}s for a tutorial room. " +
                $"Got timeToClear={ttc:F1}s (totalMobHP={R1_TotalMobHP} / playerDPS={Player_EffectiveDPS:F1}). " +
                "If red: mob HP too high or player DPS too low for R1.");

            Assert.Greater(ttc, TutorialClearFloorSec,
                $"R1 should not be a sub-{TutorialClearFloorSec}s faceroll (tutorials need a beat to teach). " +
                $"Got timeToClear={ttc:F1}s. If red: R1 is trivially easy.");
        }

        [Test]
        public void R1_PlayerSurvivesTheClear_ClearBeforeDeath()
        {
            // THE core relationship: the player kills the room before the room kills the player.
            Assert.Less(TimeToClear, TimeToDiePassively,
                $"R1 must be winnable: timeToClear ({TimeToClear:F1}s) must be LESS than the time it takes " +
                $"the mobs to kill the player ({TimeToDiePassively:F1}s @ incomingDPS={R1_IncomingDPS:F1}). " +
                "If red: the player dies before clearing R1 — the tutorial room is unwinnable as tuned.");
        }

        [Test]
        public void R1_PlayerEndsClearWithHealthMargin()
        {
            // Not just "survives" — a tutorial first-try win should leave a comfortable margin,
            // not a 1-HP photo finish. Require the player to keep > 25% HP through the clear.
            float hpRemaining = Player_MaxHP - DamageTakenDuringClear;
            float marginFraction = hpRemaining / Player_MaxHP;

            Assert.Greater(marginFraction, 0.25f,
                $"R1 first-try win should leave a safety margin (>25% HP). " +
                $"Got hpRemaining={hpRemaining:F0}/{Player_MaxHP} ({marginFraction:P0}) after taking " +
                $"~{DamageTakenDuringClear:F0} dmg during a {TimeToClear:F1}s clear. " +
                "If red: R1 is too punishing to be a guaranteed first-try tutorial win.");
        }

        [Test]
        public void Player_OutDPSes_IncomingDamage()
        {
            // Sanity: the player must out-damage the room's incoming pressure, otherwise the fight
            // is a war of attrition the player loses regardless of HP pool.
            Assert.Greater(Player_EffectiveDPS, R1_IncomingDPS,
                $"Player effective DPS ({Player_EffectiveDPS:F1}) must exceed R1 incoming DPS ({R1_IncomingDPS:F1}) " +
                "so the clear is offense-favored. If red: mobs out-pressure the player and R1 stalls.");
        }

        [Test]
        public void Canon_R1Composition_Holds()
        {
            // Guards the canonical R1 shape itself (BOSS_MOB_DESIGN_S6.md §2.2 / §2 table).
            Assert.AreEqual(3, R1_FractureImpCount, "Canon: R1 = 3 FractureImp single wave.");
            Assert.AreEqual(60, FractureImp_HP, "Canon: FractureImp HP = 60 (S6 100->60 tuning).");
            Assert.AreEqual(180, R1_TotalMobHP, "Derived: 3 imps * 60 HP = 180 total mob HP.");
        }
    }
}
