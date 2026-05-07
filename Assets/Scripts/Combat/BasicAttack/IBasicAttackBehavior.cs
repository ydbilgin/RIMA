namespace RIMA
{
    /// <summary>
    /// Strategy interface for class-specific basic attack behavior.
    /// Each class (or behavior group) provides its own implementation.
    /// BasicAttackProfile holds data; this interface holds logic.
    /// </summary>
    public interface IBasicAttackBehavior
    {
        void OnUpdate(PlayerAttack owner, BasicAttackProfile profile, float dt);
        void OnLMBInput(PlayerAttack owner, BasicAttackProfile profile, bool pressed, bool released);
        void OnRMBInput(PlayerAttack owner, BasicAttackProfile profile, bool pressed, bool released);
    }
}
