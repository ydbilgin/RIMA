using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RIMA
{
    /// <summary>
    /// Play-test yardımcı overlay — yalnızca geliştirme için.
    /// Klavye: 1-4 class | K öldür | F HP doldur | R sahne reset
    /// Mob paneli: mouse tıklamalı butonlar
    /// </summary>
    public class TestSwitcher : MonoBehaviour
    {
        [System.Serializable]
        public struct ClassEntry
        {
            public string className;
            public RuntimeAnimatorController controller;
        }

        [Header("Classes")]
        [SerializeField] private ClassEntry[] classes;

        // ─── Runtime refs ───────────────────────────────────────────────────
        private GameObject  _playerRoot;
        private Transform   _spriteChild;
        private Health      _health;
        private Animator    _animator;
        private int         _currentClass = 0;

        // Mob yönetimi
        private struct MobEntry
        {
            public string      name;
            public GameObject  go;
            public Health      health;
            public Vector3     originalPos;
        }
        private MobEntry[] _mobs;

        // ─── GUI state ──────────────────────────────────────────────────────
        private GUIStyle _boxStyle;
        private GUIStyle _labelStyle;
        private GUIStyle _btnStyle;
        private GUIStyle _btnActiveStyle;
        private bool     _stylesInit;
        private int      _isolatedMob = -1; // -1 = hepsi aktif

        // ─── Init ───────────────────────────────────────────────────────────
        private void Awake()
        {
            _playerRoot  = GameObject.FindGameObjectWithTag("Player");
            if (_playerRoot != null)
            {
                _spriteChild = _playerRoot.transform.Find("Sprite");
                _health      = _playerRoot.GetComponent<Health>();
            }
            if (_spriteChild != null)
                _animator = _spriteChild.GetComponent<Animator>();

            CollectMobs();
        }

        private void CollectMobs()
        {
            string[] names = { "HalfThrall", "VoidThrall", "Penitent",
                               "ChainWarden", "RelicCaster", "FractureImp" };
            var list = new System.Collections.Generic.List<MobEntry>();
            foreach (var n in names)
            {
                var go = GameObject.Find(n);
                if (go == null) continue;
                list.Add(new MobEntry
                {
                    name        = n,
                    go          = go,
                    health      = go.GetComponent<Health>(),
                    originalPos = go.transform.position
                });
            }
            _mobs = list.ToArray();
        }

        // ─── Update (klavye) ────────────────────────────────────────────────
        private void Update()
        {
            var kb = Keyboard.current;
            if (kb == null) return;

            if (kb.digit1Key.wasPressedThisFrame) SwitchClass(0);
            if (kb.digit2Key.wasPressedThisFrame) SwitchClass(1);
            if (kb.digit3Key.wasPressedThisFrame) SwitchClass(2);
            if (kb.digit4Key.wasPressedThisFrame) SwitchClass(3);

            if (kb.kKey.wasPressedThisFrame && _health != null)
                _health.TakeDamage(99999);

            if (kb.fKey.wasPressedThisFrame && _health != null)
                _health.RestoreToFull();

            if (kb.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void SwitchClass(int idx)
        {
            if (idx < 0 || idx >= classes.Length || _animator == null) return;
            _currentClass = idx;
            _animator.runtimeAnimatorController = classes[idx].controller;
            if (_health != null && _health.IsDead) _health.RestoreToFull();
            Debug.Log($"[TestSwitcher] Class: {classes[idx].className}");
        }

        // ─── Mob yönetimi ───────────────────────────────────────────────────
        private void IsolateMob(int idx)
        {
            _isolatedMob = idx;

            for (int i = 0; i < _mobs.Length; i++)
            {
                bool show = (i == idx);
                if (_mobs[i].go == null) continue;
                _mobs[i].go.SetActive(show);

                if (show)
                {
                    // Player'ın 2 unit sağına konumlandır ve HP'yi doldur
                    if (_playerRoot != null)
                        _mobs[i].go.transform.position = _playerRoot.transform.position + Vector3.right * 2f;

                    var h = _mobs[i].health;
                    if (h != null) h.RestoreToFull();

                    // Collider'ı aç (death'te kapanmış olabilir)
                    var col = _mobs[i].go.GetComponent<Collider2D>();
                    if (col != null) col.enabled = true;

                    // BaseMobBehavior'ı resetle
                    var mob = _mobs[i].go.GetComponent<BaseMobBehavior>();
                    if (mob != null) mob.enabled = true;
                }
            }
        }

        private void ShowAll()
        {
            _isolatedMob = -1;
            for (int i = 0; i < _mobs.Length; i++)
            {
                if (_mobs[i].go == null) continue;
                _mobs[i].go.SetActive(true);
                _mobs[i].go.transform.position = _mobs[i].originalPos;

                var h = _mobs[i].health;
                if (h != null) h.RestoreToFull();

                var col = _mobs[i].go.GetComponent<Collider2D>();
                if (col != null) col.enabled = true;

                var mob = _mobs[i].go.GetComponent<BaseMobBehavior>();
                if (mob != null) mob.enabled = true;
            }
        }

        // ─── GUI ────────────────────────────────────────────────────────────
        private void OnGUI()
        {
            if (!_stylesInit) { InitStyles(); _stylesInit = true; }

            float x = 10f;
            float y = 10f;
            float w = 200f;
            float lh = 22f;
            float bh = 26f;

            // ── CLASS PANELİ ────────────────────────────────────────────────
            int classLines = (classes != null ? classes.Length : 0) + 3;
            GUI.Box(new Rect(x - 4, y - 4, w + 8, classLines * lh + 8), GUIContent.none, _boxStyle);

            GUI.Label(new Rect(x, y, w, lh), "── SINIF ──", _labelStyle); y += lh;
            if (classes != null)
            {
                for (int i = 0; i < classes.Length; i++)
                {
                    bool active = (i == _currentClass);
                    string lbl = $"{(active ? "► " : "  ")}[{i + 1}] {classes[i].className}";
                    if (GUI.Button(new Rect(x, y, w, lh), lbl, active ? _btnActiveStyle : _btnStyle))
                        SwitchClass(i);
                    y += lh;
                }
            }
            y += 4;
            GUI.Label(new Rect(x, y, w, lh), "[K] Öldür  [F] HP  [R] Reset", _labelStyle);
            y += lh + 12;

            // ── MOB PANELİ ─────────────────────────────────────────────────
            if (_mobs == null || _mobs.Length == 0) return;

            int mobLines = _mobs.Length + 2;
            GUI.Box(new Rect(x - 4, y - 4, w + 8, mobLines * bh + 8), GUIContent.none, _boxStyle);

            GUI.Label(new Rect(x, y, w, lh), "── MOB ──", _labelStyle); y += lh;

            for (int i = 0; i < _mobs.Length; i++)
            {
                bool iso = (i == _isolatedMob);
                string lbl = iso ? $"● {_mobs[i].name}" : $"○ {_mobs[i].name}";
                if (GUI.Button(new Rect(x, y, w, bh), lbl, iso ? _btnActiveStyle : _btnStyle))
                    IsolateMob(i);
                y += bh + 2;
            }

            if (GUI.Button(new Rect(x, y, w, bh), "Hepsini Göster", _btnStyle))
                ShowAll();
        }

        // ─── Stil ──────────────────────────────────────────────────────────
        private void InitStyles()
        {
            var bgTex = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.65f));

            _boxStyle = new GUIStyle(GUI.skin.box);
            _boxStyle.normal.background = bgTex;

            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize  = 13,
                fontStyle = FontStyle.Bold,
                normal    = { textColor = Color.white }
            };

            _btnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize  = 12,
                alignment = TextAnchor.MiddleLeft,
                normal    = { textColor = new Color(0.85f, 0.85f, 0.85f) }
            };

            _btnActiveStyle = new GUIStyle(_btnStyle)
            {
                fontStyle = FontStyle.Bold,
                normal    = { textColor = new Color(1f, 0.85f, 0.3f) }
            };
        }

        private static Texture2D MakeTex(int w, int h, Color col)
        {
            var pix = new Color[w * h];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            var t = new Texture2D(w, h);
            t.SetPixels(pix); t.Apply();
            return t;
        }
    }
}
