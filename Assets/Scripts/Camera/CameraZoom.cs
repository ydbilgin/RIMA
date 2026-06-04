using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace RIMA.CameraSystem
{
    // Mouse-wheel zoom for a pixel-perfect camera. While the wheel is moving the
    // PixelPerfectCamera is disabled and orthographicSize glides continuously. ~0.12s after
    // the last scroll the target is snapped to the nearest CRISP integer pixel-ratio and the
    // camera eases exactly onto it, so re-enabling the PixelPerfectCamera produces ZERO pop
    // (the old version eased to a fractional zoom and let PPC snap, which jumped). R resets.
    [RequireComponent(typeof(PixelPerfectCamera))]
    [RequireComponent(typeof(Camera))]
    public sealed class CameraZoom : MonoBehaviour
    {
        private const int BaseRefResolutionX = 320;
        private const int BaseRefResolutionY = 180;
        private const float ScrollNotch = 120f;
        private const float ZoomLerpSpeed = 14f;
        private const float SettleDelay = 0.12f;

        [Header("Zoom range (zoom = refRes multiplier; larger = more world / further out)")]
        [SerializeField] private float defaultZoom = 1.0f;
        [SerializeField] private float minZoom = 0.7f;   // closest
        [SerializeField] private float maxZoom = 1.6f;   // furthest

        private PixelPerfectCamera _ppc;
        private Camera _camera;
        private float _targetZoom = 1f;
        private float _currentZoom = 1f;
        private float _lastScrollTime = -10f;
        private bool _snappedToCrisp;
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
                _targetZoom = _currentZoom = Mathf.Clamp(defaultZoom, minZoom, maxZoom);
                _snappedToCrisp = false;
                _seeded = true;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
            {
                _targetZoom = Mathf.Clamp(defaultZoom, minZoom, maxZoom);
                _lastScrollTime = Time.unscaledTime;
                _snappedToCrisp = false;
            }

            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                float scrollY = mouse.scroll.ReadValue().y;
                if (!Mathf.Approximately(scrollY, 0f))
                {
                    float notches = Mathf.Abs(scrollY / ScrollNotch);
                    // scroll up = zoom in = smaller zoom value
                    _targetZoom *= scrollY > 0f ? Mathf.Pow(0.9f, notches) : Mathf.Pow(1.1f, notches);
                    _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
                    _lastScrollTime = Time.unscaledTime;
                    _snappedToCrisp = false;
                }
            }

            // After the wheel goes quiet, snap the TARGET to the nearest crisp pixel ratio once,
            // so the camera eases exactly onto it (no PPC re-quantize pop on re-enable).
            bool quiet = (Time.unscaledTime - _lastScrollTime) >= SettleDelay;
            if (quiet && !_snappedToCrisp)
            {
                _targetZoom = NearestCrispZoom(_targetZoom);
                _snappedToCrisp = true;
            }

            _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, 1f - Mathf.Exp(-ZoomLerpSpeed * Time.unscaledDeltaTime));

            bool atTarget = Mathf.Abs(_currentZoom - _targetZoom) < 0.0005f;
            if (_snappedToCrisp && atTarget)
            {
                _currentZoom = _targetZoom;
                ApplyPixelPerfect(_currentZoom);
            }
            else
            {
                if (_ppc.enabled) _ppc.enabled = false;
                _camera.orthographicSize = OrthoForZoom(_currentZoom);
            }
        }

        // Nearest zoom whose pixel ratio (screenHeight / refResY) is an integer, clamped to range.
        private float NearestCrispZoom(float zoom)
        {
            float h = Screen.height;
            if (h <= 0f) return zoom;

            int n = Mathf.RoundToInt(h / (BaseRefResolutionY * zoom));
            int nMin = Mathf.Max(1, Mathf.FloorToInt(h / (BaseRefResolutionY * maxZoom)));
            int nMax = Mathf.Max(nMin, Mathf.CeilToInt(h / (BaseRefResolutionY * minZoom)));
            n = Mathf.Clamp(n, nMin, nMax);
            return h / (BaseRefResolutionY * n);
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
