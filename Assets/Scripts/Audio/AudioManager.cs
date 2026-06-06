using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Audio
{
    public enum Sfx
    {
        // ── original cues ─────────────────────────────────────
        Hit, Shatter, Dash, Cast, DraftSelect, GateOpen, Death, BossIntro, Finisher,
        // ── T2 additions ──────────────────────────────────────
        SwingLight,       // light M1 swing
        SwingHeavy,       // heavy / finisher swing
        HitImpact,        // meaty hit confirm (light+heavy)
        EnemyDeath,       // enemy kill
        ExecutePayoff,    // DeathBlow execute land
        RoomClear,        // room cleared / portal opens
        DraftHover,       // skill card hover
        ChamberAmbient,   // looping chamber background
        KnockdownThud,    // knockdown land impact
    }

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

        // Demo: procedural placeholder SFX read as noise ("sesler manasız"), so they stay muted.
        // Real clips dropped into Resources/Audio/<Sfx>.wav override + auto-unmute per cue.
        // Real audio is produced after animation (DECISIONS_S6); flip false only to A/B the synth.
        [SerializeField] private bool muteProceduralFallback = true;

        private AudioSource src;
        private AudioSource musicSrc;
        private AudioSource ambientSrc;
        private readonly Dictionary<Sfx, AudioClip> clips = new Dictionary<Sfx, AudioClip>();
        private readonly HashSet<Sfx> realClips = new HashSet<Sfx>();

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
            LoadResourceOverrides();
            TryPlayMusic();
            TryPlayAmbient();
        }

        /// <summary>Play a SFX one-shot. No-op if manager/clip missing (safe from anywhere).</summary>
        public static void Play(Sfx sfx, float volume = 1f)
        {
            if (Instance == null || Instance.src == null) return;
            // Hold the cue but stay silent while only the procedural placeholder exists.
            if (Instance.muteProceduralFallback && !Instance.realClips.Contains(sfx)) return;
            if (Instance.clips.TryGetValue(sfx, out var clip) && clip != null)
                Instance.src.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        private void GenerateClips()
        {
            clips[Sfx.Hit]           = Noise(0.08f, 0.55f, 1400f, true);
            clips[Sfx.Shatter]       = Noise(0.13f, 0.45f, 5000f, true);
            clips[Sfx.Dash]          = Sweep(0.16f, 220f, 640f, 0.45f);
            clips[Sfx.Cast]          = Tone(0.20f, 540f, 0.45f, true);
            clips[Sfx.DraftSelect]   = Tone(0.12f, 900f, 0.40f, false);
            clips[Sfx.GateOpen]      = Sweep(0.45f, 130f, 360f, 0.45f);
            clips[Sfx.Death]         = Sweep(0.38f, 420f, 80f, 0.50f);
            clips[Sfx.BossIntro]     = Tone(0.70f, 110f, 0.55f, true);
            clips[Sfx.Finisher]      = Sweep(0.28f, 180f, 900f, 0.50f);
            // T2 additions — procedural fallbacks (replaced by real clips if present in Resources/Audio)
            clips[Sfx.SwingLight]    = Sweep(0.10f, 320f, 800f, 0.38f);
            clips[Sfx.SwingHeavy]    = Sweep(0.14f, 200f, 600f, 0.55f);
            clips[Sfx.HitImpact]     = Noise(0.10f, 0.60f, 1200f, true);
            clips[Sfx.EnemyDeath]    = Sweep(0.30f, 380f, 70f, 0.48f);
            clips[Sfx.ExecutePayoff] = Sweep(0.35f, 160f, 1100f, 0.60f);
            clips[Sfx.RoomClear]     = Sweep(0.50f, 120f, 420f, 0.50f);
            clips[Sfx.DraftHover]    = Tone(0.08f, 1100f, 0.28f, true);
            clips[Sfx.ChamberAmbient]= Tone(3.00f, 80f, 0.18f, false);
            clips[Sfx.KnockdownThud] = Noise(0.14f, 0.65f, 900f, true);
        }

        private void LoadResourceOverrides()
        {
            foreach (Sfx sfx in System.Enum.GetValues(typeof(Sfx)))
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/" + sfx);
                if (clip != null)
                {
                    clips[sfx] = clip;
                    realClips.Add(sfx);  // real clip present → this cue unmutes
                }
            }
        }

        private void TryPlayMusic()
        {
            AudioClip music = Resources.Load<AudioClip>("Audio/music_demo");
            if (music == null) return;

            musicSrc = gameObject.AddComponent<AudioSource>();
            musicSrc.clip = music;
            musicSrc.loop = true;
            musicSrc.playOnAwake = false;
            musicSrc.spatialBlend = 0f;
            musicSrc.volume = 0.25f;
            musicSrc.Play();
        }

        private void TryPlayAmbient()
        {
            if (!realClips.Contains(Sfx.ChamberAmbient)) return;
            if (!clips.TryGetValue(Sfx.ChamberAmbient, out var clip) || clip == null) return;

            ambientSrc = gameObject.AddComponent<AudioSource>();
            ambientSrc.clip = clip;
            ambientSrc.loop = true;
            ambientSrc.playOnAwake = false;
            ambientSrc.spatialBlend = 0f;
            ambientSrc.volume = 0.12f;
            ambientSrc.Play();
        }

        /// <summary>Play a SFX as a looping ambient source. Returns the AudioSource so caller can stop it.
        /// Obeys the same mute-fallback rule as <see cref="Play"/>.</summary>
        public static AudioSource PlayLooped(Sfx sfx, float volume = 0.5f)
        {
            if (Instance == null || Instance.src == null) return null;
            if (Instance.muteProceduralFallback && !Instance.realClips.Contains(sfx)) return null;
            if (!Instance.clips.TryGetValue(sfx, out var clip) || clip == null) return null;

            var loopSrc = Instance.gameObject.AddComponent<AudioSource>();
            loopSrc.clip = clip;
            loopSrc.loop = true;
            loopSrc.playOnAwake = false;
            loopSrc.spatialBlend = 0f;
            loopSrc.volume = Mathf.Clamp01(volume);
            loopSrc.Play();
            return loopSrc;
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
