using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Future extension hook for obstacle behavior queries on walkability/dashability.
    /// MVP: no implementers yet. WalkabilityMap iterates registered providers later.
    /// </summary>
    public interface IObstacle
    {
        Vector3Int Cell { get; }
        bool IsWalkable { get; }   // can the player walk through?
        bool IsDashable { get; }   // can the player dash through?
        bool IsSkillable { get; }  // can a bypass-skill go through?
        bool TakesDamage { get; }  // destructible?
    }
}
