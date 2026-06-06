using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public class InputBufferService : MonoBehaviour
    {
        // T2 M1: dash buffer 80ms (spec). Attack buffer keeps a slightly larger window for leniency.
        [SerializeField] private float bufferWindow = 0.08f;

        private enum Pending { None, Dash, Attack }

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

        public void RequestAttack()
        {
            pending = Pending.Attack;
            bufferExpiry = Time.time + bufferWindow;
        }

        private void Update()
        {
            if (pending == Pending.None) return;
            if (Time.time > bufferExpiry) { pending = Pending.None; return; }
            if (attack != null && attack.IsCommitted) return;

            if (pending == Pending.Dash)
                controller?.TryDash();
            else if (pending == Pending.Attack)
                attack?.ExecuteBufferedPrimaryAttack();

            pending = Pending.None;
        }
    }
}
