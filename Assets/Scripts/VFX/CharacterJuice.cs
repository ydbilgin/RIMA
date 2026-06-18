using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Kod-only "animasyon hissi" — statik sprite + engine juice (bkz. SkillVfx.cs aynı ruh).
    /// Frame animasyonu YOK; sadece görsel child transform'una (Body + HandAnchor) tween.
    /// Root pozisyonuna ASLA dokunmaz (fizik/collider/hitbox etkilenmez).
    ///
    /// - Idle: nefes-bob (sin), Pixel Perfect Camera ile uyum için TAM-PIXEL adımlarla (1/PPU katları).
    /// - Yürüme: hareket yönüne hafif Z-tilt + daha hızlı bob.
    /// - Saldırı: kısa pozisyon-lunge punch (scale yerine; pixel shimmer'ı önler).
    ///
    /// Death (Health.IsDead) ve pause (Time.timeScale == 0) durumlarında durur.
    /// Inspector'dan enable toggle ile tamamen kapatılabilir (demo riski sıfırlama).
    /// </summary>
    public class CharacterJuice : MonoBehaviour
    {
        [Header("Toggle")]
        [Tooltip("Kapatınca tüm juice durur ve transform'lar base konuma döner.")]
        [SerializeField] private bool enableJuice = true;

        [Header("Bob")]
        [Tooltip("Nefes-bob genliği (piksel). Pixel-snap için tam-piksel adımlara yuvarlanır.")]
        [SerializeField] private float bobAmplitudePx = 1f;
        [Tooltip("Pixel Perfect Camera PPU (1px = 1/PPU dünya birimi).")]
        [SerializeField] private float pixelsPerUnit = 64f;
        [Tooltip("Idle nefes periyodu (saniye).")]
        [SerializeField] private float idlePeriod = 2.2f;
        [Tooltip("Yürürken bob periyodu (saniye) — daha hızlı.")]
        [SerializeField] private float walkPeriod = 0.35f;

        [Header("Walk Tilt")]
        [Tooltip("Yürürken hareket yönüne doğru Z eğimi (derece).")]
        [SerializeField] private float tiltDegrees = 3.5f;
        [Tooltip("Eğim/bob mod geçişlerinin yumuşama hızı.")]
        [SerializeField] private float blendSpeed = 10f;

        [Header("Attack Punch")]
        [Tooltip("Saldırıda hareket yönüne ileri-geri lunge miktarı (piksel).")]
        [SerializeField] private float punchPixels = 2f;
        [Tooltip("Punch süresi (saniye).")]
        [SerializeField] private float punchDuration = 0.1f;

        private Transform body;
        private Transform handAnchor;
        private SpriteRenderer bodyRenderer;

        private Vector3 bodyBasePos;
        private Vector3 handBasePos;
        private Quaternion bodyBaseRot;

        private PlayerController controller;
        private PlayerAttack attack;
        private Health health;

        private float bobPhase;
        private float walkBlend;   // 0 = idle, 1 = walk
        private float punchTimer;  // saniye geriye sayar
        private Vector2 punchDir;
        private bool wired;

        private float PixelStep => pixelsPerUnit > 0.01f ? 1f / pixelsPerUnit : 1f / 64f;

        public void SetHandBasePosition(Vector3 localPosition)
        {
            handBasePos = localPosition;
        }

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            attack = GetComponent<PlayerAttack>();
            health = GetComponent<Health>();

            body = transform.Find("Body");
            handAnchor = transform.Find("HandAnchor");

            if (body != null)
            {
                bodyRenderer = body.GetComponent<SpriteRenderer>();
                bodyBasePos = body.localPosition;
                bodyBaseRot = body.localRotation;
            }
            if (handAnchor != null)
                handBasePos = handAnchor.localPosition;
        }

        private void OnEnable()
        {
            if (attack != null && !wired)
            {
                attack.OnComboStep += HandleComboStep;
                wired = true;
            }
        }

        private void OnDisable()
        {
            if (attack != null && wired)
            {
                attack.OnComboStep -= HandleComboStep;
                wired = false;
            }
            // Toggle/disable: base konuma geri dön (T-pose dondurma değil, temiz reset).
            ResetToBase();
        }

        private void HandleComboStep(int step)
        {
            punchDir = ResolveFacing();
            punchTimer = Mathf.Max(0.01f, punchDuration);
        }

        private void LateUpdate()
        {
            if (body == null) return;

            // Pause: unscaled time KULLANMA — juice donsun (donmuş kare hareket etmesin).
            if (Time.timeScale == 0f) return;

            // Death: juice DUR ve base konuma dön (bob'lu ceset olmasın).
            bool dead = health != null && health.IsDead;

            if (!enableJuice || dead)
            {
                ResetToBase();
                return;
            }

            float dt = Time.deltaTime;
            bool moving = controller != null && controller.IsMoving && !controller.IsDashing;

            // Idle <-> walk yumuşak geçiş.
            walkBlend = Mathf.MoveTowards(walkBlend, moving ? 1f : 0f, blendSpeed * dt);

            float period = Mathf.Lerp(idlePeriod, walkPeriod, walkBlend);
            if (period < 0.01f) period = 0.01f;
            bobPhase += (Mathf.PI * 2f) * (dt / period);
            if (bobPhase > Mathf.PI * 2f) bobPhase -= Mathf.PI * 2f;

            // Bob: yürürken bir miktar daha belirgin; tam-piksel adımlara yuvarla.
            float ampPx = bobAmplitudePx * Mathf.Lerp(1f, 1.5f, walkBlend);
            float bobWorld = Mathf.Sin(bobPhase) * ampPx * PixelStep;
            bobWorld = SnapToPixel(bobWorld);

            // Attack punch: hareket yönüne ileri-geri lunge (scale değil — shimmer önler).
            Vector2 punchOffset = Vector2.zero;
            if (punchTimer > 0f)
            {
                punchTimer -= dt;
                float t = Mathf.Clamp01(punchTimer / Mathf.Max(0.01f, punchDuration)); // 1 -> 0
                // Yarıda ileri, sonra geri (sin tepe): kısa ileri-geri lunge.
                float amount = Mathf.Sin(t * Mathf.PI) * punchPixels * PixelStep;
                punchOffset = punchDir * amount;
            }

            Vector3 offset = new Vector3(
                SnapToPixel(punchOffset.x),
                bobWorld + SnapToPixel(punchOffset.y),
                0f);

            // Bob/punch'ı Body'ye uygula; silah el hizasında kalsın diye HandAnchor'a AYNI offset.
            body.localPosition = bodyBasePos + offset;
            if (handAnchor != null)
                handAnchor.localPosition = handBasePos + offset;

            // Walk tilt: hareket yönüne doğru Z eğimi (yalnız Body'ye; HandAnchor eğilmesin ki silah dik kalsın).
            float targetTilt = 0f;
            if (walkBlend > 0.001f)
            {
                Vector2 dir = ResolveFacing();
                targetTilt = -Mathf.Sign(dir.x) * tiltDegrees * walkBlend;
            }
            Quaternion targetRot = bodyBaseRot * Quaternion.Euler(0f, 0f, targetTilt);
            body.localRotation = Quaternion.RotateTowards(body.localRotation, targetRot, blendSpeed * 60f * dt);
        }

        private Vector2 ResolveFacing()
        {
            if (controller != null)
            {
                Vector2 f = controller.FacingDirection;
                if (f.sqrMagnitude > 0.0001f) return f.normalized;
            }
            return Vector2.right;
        }

        private float SnapToPixel(float worldValue)
        {
            float step = PixelStep;
            return Mathf.Round(worldValue / step) * step;
        }

        private void ResetToBase()
        {
            if (body != null)
            {
                body.localPosition = bodyBasePos;
                body.localRotation = bodyBaseRot;
            }
            if (handAnchor != null)
                handAnchor.localPosition = handBasePos;
            walkBlend = 0f;
            punchTimer = 0f;
        }
    }
}
