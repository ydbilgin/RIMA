using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;

namespace RIMA
{
    [RequireComponent(typeof(SpriteSkin))]
    public class SpriteSkinFix : MonoBehaviour
    {
        IEnumerator Start()
        {
            var skin = GetComponent<SpriteSkin>();
            skin.enabled = false;
            yield return null;
            skin.enabled = true;
        }
    }
}
