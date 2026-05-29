using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Audio
{
    public enum Sfx { Hit, Shatter, Dash, Cast, DraftSelect, GateOpen, Death, BossIntro }

    /// <summary>
    /// Minimal self-contained SFX layer (W2 skeleton). Generates placeholder clips procedurally
    /// at runtime (zero asset files / zero gen) so combat/UI get audible game-feel immediately;
    /// real clips can replace the generated ones later by assigning to <see cref="clips"/>.
    /// Self-bootstraps (RuntimeInitializeOnLoadMethod) so no scene wiring is needed.
    /// Call <see cref="Play"/> from anywhere: AudioManager.Play(Sfx.Hit).
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private AudioSource src;
        private readonly Dictionary<Sfx, AudioClip> clips = new Dictionary<Sfx, AudioClip>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null) return;
            var go = new GameObject("AudioManager_Auto");
            DontDestroyOnLoad(go);
            go.AddComponent<AudioManager>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            GenerateClips();
        }

        /// <summary>Play a SFX one-shot. No-op if manager/clip missing (safe from anywhere).</summary>
        public static void Play(Sfx sfx, float volume = 1f)
        {
            if (Instance == null || Instance.src == null) return;
            if (Instance.clips.TryGetValue(sfx, out var clip) && clip != null)
                Instance.src.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        private void GenerateClips()
        {
            clips[Sfx.Hit]         = Noise(0.08f, 0.55f, 1400f, true);  // meaty thwack
            clips[Sfx.Shatter]     = Noise(0.13f, 0.45f, 5000f, true);  // glass/armor clink
            clips[Sfx.Dash]        = Sweep(0.16f, 220f, 640f, 0.45f);
            clips[Sfx.Cast]        = Tone(0.20f, 540f, 0.45f, true);
            clips[Sfx.DraftSelect] = Tone(0.12f, 900f, 0.40f, false);
            clips[Sfx.GateOpen]    = Sweep(0.45f, 130f, 360f, 0.45f);
            clips[Sfx.Death]       = Sweep(0.38f, 420f, 80f, 0.50f);
            clips[Sfx.BossIntro]   = Tone(0.70f, 110f, 0.55f, true);
        }

        // ── procedural synth helpers (mono, 44.1k) ──────────────────────
        private const int Rate = 44100;

        private static AudioClip Tone(float dur, float freq, float amp, bool decay)
        {
            int n = Mathf.Max(1, (int)(Rate * dur));
            var data = new float[n];
            for (int i = 0; i < n; i++)
            {
                float t = (float)i / Rate;
                float env = decay ? Mathf.Exp(-t * 8f) : 1f - (float)i / n;
                data[i] = Mathf.Sin(2f * Mathf.PI * freq * t) * amp * env;
            }
            var c = AudioClip.Create("sfx_tone", n, 1, Rate, false);
            c.SetData(data, 0);
            return c;
        }

        private static AudioClip Noise(float dur, float amp, float lowpassHz, bool decay)
        {
            int n = Mathf.Max(1, (int)(Rate * dur));
            var data = new float[n];
            float prev = 0f;
            float a = Mathf.Clamp01(lowpassHz / Rate);
            for (int i = 0; i < n; i++)
            {
                float white = Random.value * 2f - 1f;
                prev = Mathf.Lerp(prev, white, a);
                float env = decay ? Mathf.Exp(-(float)i / n * 6f) : 1f;
                data[i] = prev * amp * env;
            }
            var c = AudioClip.Create("sfx_noise", n, 1, Rate, false);
            c.SetData(data, 0);
            return c;
        }

        private static AudioClip Sweep(float dur, float f0, float f1, float amp)
        {
            int n = Mathf.Max(1, (int)(Rate * dur));
            var data = new float[n];
            float phase = 0f;
            for (int i = 0; i < n; i++)
            {
                float t = (float)i / n;
                float f = Mathf.Lerp(f0, f1, t);
                phase += 2f * Mathf.PI * f / Rate;
                float env = Mathf.Sin(Mathf.PI * t);   // smooth in/out
                data[i] = Mathf.Sin(phase) * amp * env;
            }
            var c = AudioClip.Create("sfx_sweep", n, 1, Rate, false);
            c.SetData(data, 0);
            return c;
        }
    }
}
