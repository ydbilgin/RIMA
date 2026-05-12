using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public class InputBufferService : MonoBehaviour
    {
        [SerializeField] private float bufferWindow = 0.18f;

        private enum Pending { None, Dash }

        private Pending pending;
        private float bufferExpiry;
        private PlayerAttack attack;
        private PlayerController controller;

        private void Awake()
        {
            attack = GetComponent<PlayerAttack>();
            controller = GetComponent<PlayerController>();
        }

        public void RequestDash()
        {
            pending = Pending.Dash;
            bufferExpiry = Time.time + bufferWindow;
        }

        private void Update()
        {
            if (pending == Pending.None) return;
            if (Time.time > bufferExpiry) { pending = Pending.None; return; }
            if (attack != null && attack.IsCommitted) return;

            if (pending == Pending.Dash)
                controller.TryDash();

            pending = Pending.None;
        }
    }
}
