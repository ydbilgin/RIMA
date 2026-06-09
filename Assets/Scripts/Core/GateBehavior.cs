// ⚠️ LEGACY (2026-06-07): Bu sınıf CANLI demo yolunda DEĞİL (kanıt: STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md).
// Canlı yol: _Arena → RoomRunDirector → IsoRoomBuilder.BuildExitDoors. Yeni iş BURAYA BAĞLANMAZ.
using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Gate görsel state machine: Locked → Unlocking (animasyon) → Open (oda tipi sprite).
    ///
    /// Setup (Gate_Prefab):
    ///   SpriteRenderer         ← bu script yönetir
    ///   Animator               ← opsiyonel; gate_unlock_sheet.png animasyonu için
    ///   BoxCollider2D          ← opsiyonel; DoorTrigger ile birlikte kullanılabilir
    ///   GateBehavior           ← bu script
    ///   DoorTrigger (ayrı GO) ← navigation trigger, ayrı child GO'a ekle
    ///
    /// Spritelar hazır olunca Inspector'dan ata:
    ///   spriteLocked          ← gate_locked.png
    ///   spriteUnlockedBase    ← gate_unlocked_base.png
    ///   unlockAnimClip        ← gate_unlock_sheet animasyonu (Animator Controller'a eklenmiş)
    ///   spriteRoomCombat      ← gate_combat.png
    ///   spriteRoomBoss        ← gate_boss.png
    ///   (Faz B) diğerleri     ← gate_chest, gate_elite, gate_merchant, gate_forge, gate_event, gate_curse
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [System.Obsolete("LEGACY _IsoGame gate visuals — live exit is RoomRunDirector.RoomRunExitDoorTrigger. Post-demo removal.", false)]
    public class GateBehavior : MonoBehaviour
    {
        [Header("Direction")]
        [SerializeField] private DoorDirection direction;

        [Header("Sprites — Static")]
        [SerializeField] private Sprite spriteLocked;
        [SerializeField] private Sprite spriteUnlockedBase;

        [Header("Sprites — Room Types (Faz A)")]
        [SerializeField] private Sprite spriteRoomCombat;
        [SerializeField] private Sprite spriteRoomBoss;

        [Header("Sprites — Room Types (Faz B, sonradan atanacak)")]
        [SerializeField] private Sprite spriteRoomElite;
        [SerializeField] private Sprite spriteRoomChest;
        [SerializeField] private Sprite spriteRoomMerchant;
        [SerializeField] private Sprite spriteRoomForge;
        [SerializeField] private Sprite spriteRoomEvent;
        [SerializeField] private Sprite spriteRoomCurse;

        [Header("Animation")]
        [Tooltip("gate_unlock_sheet animasyon clip'i. Animator Controller'da olmalı.")]
        [SerializeField] private string unlockAnimTrigger = "Unlock";
        [SerializeField] private float  unlockAnimDuration = 0.48f; // 8 frame × 60ms

        // ─── Internal state ──────────────────────────────────────────────────────
        private enum GateState { Hidden, Locked, Unlocking, Open }

        private GateState state = GateState.Locked;
        private RoomType pendingRoomType = RoomType.Combat;
        private SpriteRenderer sr;
        private Animator anim;

        // ─── Lifecycle ───────────────────────────────────────────────────────────

        private void Awake()
        {
            sr   = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>(); // null if no Animator — handled gracefully
        }

        private void Start()
        {
            ApplyState(GateState.Hidden);
        }

        // ─── Public API ──────────────────────────────────────────────────────────

        public DoorDirection Direction => direction;
        public bool IsOpen => state == GateState.Open;

        /// <summary>
        /// RuntimeRoomManager çağırır — bu kapı aktif bir çıkış.
        /// Unlock animasyonu oynar, açılınca oda tipi sprite'ı gösterir.
        /// </summary>
        public void Unlock(RoomType targetRoomType)
        {
            if (state == GateState.Open || state == GateState.Unlocking) return;
            pendingRoomType = targetRoomType;
            StartCoroutine(UnlockSequence());
        }

        /// <summary>Bu yönde grafta çıkış yok — tamamen gizle.</summary>
        public void Hide()
        {
            StopAllCoroutines();
            ApplyState(GateState.Hidden);
        }

        /// <summary>Yeni oda başladığında kilitle (RuntimeRoomManager.StartRoom çağırır).</summary>
        public void Lock()
        {
            StopAllCoroutines();
            ApplyState(GateState.Locked);
        }

        /// <summary>Oda temizlenmeden zorla aç (test / debug için).</summary>
        public void ForceOpen(RoomType targetRoomType = RoomType.Combat)
        {
            StopAllCoroutines();
            pendingRoomType = targetRoomType;
            ApplyState(GateState.Open);
        }

        private IEnumerator UnlockSequence()
        {
            ApplyState(GateState.Unlocking);

            if (anim != null)
            {
                anim.SetTrigger(unlockAnimTrigger);
                yield return new WaitForSeconds(unlockAnimDuration);
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }

            ApplyState(GateState.Open);
        }

        // ─── State application ───────────────────────────────────────────────────

        private void ApplyState(GateState newState)
        {
            state = newState;

            switch (newState)
            {
                case GateState.Hidden:
                    if (anim != null) anim.enabled = false;
                    sr.enabled = false;
                    break;

                case GateState.Locked:
                    if (anim != null) anim.enabled = false;
                    sr.enabled = true;
                    sr.sprite = spriteLocked;
                    break;

                case GateState.Unlocking:
                    sr.enabled = true;
                    if (anim != null) anim.enabled = true;
                    break;

                case GateState.Open:
                    sr.enabled = true;
                    if (anim != null) anim.enabled = false;
                    sr.sprite = GetRoomTypeSprite(pendingRoomType);
                    break;
            }
        }

        private Sprite GetRoomTypeSprite(RoomType type)
        {
            Sprite s = type switch
            {
                RoomType.Combat   => spriteRoomCombat,
                RoomType.Elite    => spriteRoomElite,
                RoomType.Boss     => spriteRoomBoss,
                RoomType.Chest    => spriteRoomChest,
                RoomType.Merchant => spriteRoomMerchant,
                RoomType.Forge    => spriteRoomForge,
                RoomType.Event    => spriteRoomEvent,
                RoomType.Curse    => spriteRoomCurse,
                _                 => null
            };

            return s != null ? s : spriteUnlockedBase;
        }

        // ─── Gizmo ───────────────────────────────────────────────────────────────

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = state == GateState.Locked ? Color.red
                         : state == GateState.Open   ? Color.green
                                                     : Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 1.2f);

#if UNITY_EDITOR
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * 0.8f,
                $"Gate [{direction}→{pendingRoomType}] — {state}"
            );
#endif
        }
    }
}
