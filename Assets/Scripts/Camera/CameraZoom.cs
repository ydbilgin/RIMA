using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace RIMA.CameraSystem
{
    [RequireComponent(typeof(PixelPerfectCamera))]
    public sealed class CameraZoom : MonoBehaviour
    {
        private const int BaseRefResolutionX = 320;
        private const int BaseRefResolutionY = 180;
        private const float MinZoom = 0.5f;
        private const float MaxZoom = 3f;
        private const float ScrollNotch = 120f;

        private PixelPerfectCamera _ppc;
        private float _zoom = 1f;
        private bool _seeded;

        private void Awake()
        {
            _ppc = GetComponent<PixelPerfectCamera>();
        }

        private void Update()
        {
            if (_ppc == null || !_ppc.enabled) return;

            if (!_seeded)
            {
                _zoom = Mathf.Clamp((float)_ppc.refResolutionX / BaseRefResolutionX, MinZoom, MaxZoom);
                _seeded = true;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
            {
                _zoom = 1f;
                ApplyZoom();
                return;
            }

            Mouse mouse = Mouse.current;
            if (mouse == null) return;

            float scrollY = mouse.scroll.ReadValue().y;
            if (Mathf.Approximately(scrollY, 0f)) return;

            float notches = Mathf.Abs(scrollY / ScrollNotch);
            _zoom *= scrollY > 0f ? Mathf.Pow(0.9f, notches) : Mathf.Pow(1.1f, notches);
            _zoom = Mathf.Clamp(_zoom, MinZoom, MaxZoom);
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            _ppc.refResolutionX = RoundToEven(BaseRefResolutionX * _zoom);
            _ppc.refResolutionY = RoundToEven(BaseRefResolutionY * _zoom);
        }

        private static int RoundToEven(float value)
        {
            int rounded = Mathf.RoundToInt(value);
            return (rounded & 1) == 0 ? rounded : rounded + 1;
        }
    }
}
