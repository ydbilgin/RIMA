using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace RIMA.CameraSystem
{
    // Mouse-wheel zoom. While the wheel is moving we drive orthographicSize directly so the
    // zoom glides continuously; once it settles we hand control back to the Pixel Perfect
    // Camera so the pixel art snaps crisp again. R resets to the authored framing.
    [RequireComponent(typeof(PixelPerfectCamera))]
    [RequireComponent(typeof(Camera))]
    public sealed class CameraZoom : MonoBehaviour
    {
        private const int BaseRefResolutionX = 320;
        private const int BaseRefResolutionY = 180;
        private const float MinZoom = 0.5f;
        private const float MaxZoom = 3f;
        private const float ScrollNotch = 120f;
        private const float ZoomLerpSpeed = 12f;  // how fast current chases target (higher = snappier)
        private const float SettleDelay = 0.12f;   // quiet time after last scroll before re-snapping to pixel-perfect

        private PixelPerfectCamera _ppc;
        private Camera _camera;
        private float _targetZoom = 1f;
        private float _currentZoom = 1f;
        private float _seedZoom = 1f;
        private float _lastScrollTime = -10f;
        private bool _seeded;

        private void Awake()
        {
            _ppc = GetComponent<PixelPerfectCamera>();
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_ppc == null || _camera == null) return;

            if (!_seeded)
            {
                _seedZoom = Mathf.Clamp((float)_ppc.refResolutionX / BaseRefResolutionX, MinZoom, MaxZoom);
                _targetZoom = _currentZoom = _seedZoom;
                _seeded = true;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
            {
                _targetZoom = _seedZoom;
                _lastScrollTime = Time.unscaledTime;
            }

            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                float scrollY = mouse.scroll.ReadValue().y;
                if (!Mathf.Approximately(scrollY, 0f))
                {
                    float notches = Mathf.Abs(scrollY / ScrollNotch);
                    _targetZoom *= scrollY > 0f ? Mathf.Pow(0.9f, notches) : Mathf.Pow(1.1f, notches);
                    _targetZoom = Mathf.Clamp(_targetZoom, MinZoom, MaxZoom);
                    _lastScrollTime = Time.unscaledTime;
                }
            }

            // Frame-rate independent smoothing toward the target zoom.
            _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, 1f - Mathf.Exp(-ZoomLerpSpeed * Time.unscaledDeltaTime));

            bool settled = (Time.unscaledTime - _lastScrollTime) >= SettleDelay
                           && Mathf.Abs(_currentZoom - _targetZoom) < 0.002f;

            if (settled)
            {
                // At rest: let the Pixel Perfect Camera own orthographicSize for crisp pixels.
                _currentZoom = _targetZoom;
                ApplyPixelPerfect(_currentZoom);
            }
            else
            {
                // Mid-zoom: take over from the Pixel Perfect Camera and slide orthographicSize.
                if (_ppc.enabled) _ppc.enabled = false;
                _camera.orthographicSize = OrthoForZoom(_currentZoom);
            }
        }

        private void ApplyPixelPerfect(float zoom)
        {
            int refX = RoundToEven(BaseRefResolutionX * zoom);
            int refY = RoundToEven(BaseRefResolutionY * zoom);
            if (_ppc.refResolutionX != refX || _ppc.refResolutionY != refY)
            {
                _ppc.refResolutionX = refX;
                _ppc.refResolutionY = refY;
            }

            if (!_ppc.enabled) _ppc.enabled = true;
        }

        private float OrthoForZoom(float zoom)
        {
            float ppu = Mathf.Max(1f, _ppc.assetsPPU);
            return (BaseRefResolutionY * zoom) / (2f * ppu);
        }

        private static int RoundToEven(float value)
        {
            int rounded = Mathf.RoundToInt(value);
            return (rounded & 1) == 0 ? rounded : rounded + 1;
        }
    }
}
