using UnityEngine;
using RIMA;

/// <summary>
/// Editor utility: validates Player prefab required components.
/// Karar #123 Yol A Level 1 - HandAnchorAttach must be on the Player root.
/// </summary>
public class PlayerPrefabSetup : MonoBehaviour
{
    [ContextMenu("Validate Player Setup")]
    private void Validate()
    {
        var ha = GetComponent<HandAnchorAttach>();
        if (ha == null) Debug.LogError("PlayerPrefabSetup: HandAnchorAttach missing on Player root.");
        else Debug.Log("PlayerPrefabSetup: HandAnchorAttach OK.");

        var ch = GetComponent<CombatHandler>();
        if (ch == null) Debug.LogWarning("PlayerPrefabSetup: CombatHandler missing.");
        else Debug.Log("PlayerPrefabSetup: CombatHandler OK.");

        var sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) Debug.LogWarning("PlayerPrefabSetup: No SpriteRenderer found in children.");
        else Debug.Log($"PlayerPrefabSetup: SpriteRenderer OK ({sr.gameObject.name}).");
    }
}
