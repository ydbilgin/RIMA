using UnityEngine;

namespace RIMA
{
    public interface IBasicAttackState
    {
        int CurrentStep { get; }
        float StepWindow { get; }
        bool WindowOpen { get; }
        void Advance();
        void Reset();
        void Tick(float deltaTime);
    }
}
