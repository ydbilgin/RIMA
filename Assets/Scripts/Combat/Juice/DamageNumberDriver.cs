using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RIMA.Combat
{
    public class DamageNumberDriver : MonoBehaviour
    {
        public static DamageNumberDriver Instance { get; private set; }

        [SerializeField] private int poolSize = 24;
        [SerializeField] private float lifetime = 1.2f;
        [SerializeField] private float floatDistance = 0.5f;
        [SerializeField] private float baseFontSize = 3f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color critColor = Color.yellow;

        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            BuildPool();
        }

        private void OnEnable()
        {
            CombatEventBus.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void BuildPool()
        {
            for (int i = 0; i < Mathf.Max(0, poolSize); i++)
            {
                GameObject instance = CreateNumberObject();
                instance.SetActive(false);
                pool.Enqueue(instance);
            }
        }

        private void HandleHit(HitEvent e)
        {
            Spawn(e.worldPos, e.damage, e.isCrit);
        }

        public void Spawn(Vector3 worldPosition, float damage, bool isCrit)
        {
            GameObject numberObject = GetNumberObject();
            if (numberObject == null)
            {
                return;
            }

            numberObject.transform.SetParent(transform, false);
            numberObject.transform.position = worldPosition;
            numberObject.transform.rotation = Quaternion.identity;
            numberObject.SetActive(true);

            string text = Mathf.CeilToInt(damage).ToString();
            Color color = isCrit ? critColor : normalColor;
            float fontSize = isCrit ? baseFontSize * 1.5f : baseFontSize;
            ApplyText(numberObject, text, color, fontSize);

            StartCoroutine(AnimateNumber(numberObject, worldPosition, color));
        }

        private GameObject GetNumberObject()
        {
            while (pool.Count > 0)
            {
                GameObject pooled = pool.Dequeue();
                if (pooled != null)
                {
                    return pooled;
                }
            }

            return CreateNumberObject();
        }

        private GameObject CreateNumberObject()
        {
            GameObject go = new GameObject("DamageNumber");
            go.transform.SetParent(transform, false);

            TextMeshPro tmp = go.AddComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = baseFontSize;
            tmp.color = normalColor;
            tmp.text = string.Empty;

            TextMesh textMesh = go.AddComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = 32;
            textMesh.characterSize = 0.1f;
            textMesh.color = normalColor;
            textMesh.text = string.Empty;
            textMesh.gameObject.SetActive(false);

            return go;
        }

        private void ApplyText(GameObject go, string text, Color color, float fontSize)
        {
            TextMeshPro tmp = go.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.enabled = true;
                tmp.text = text;
                tmp.color = color;
                tmp.fontSize = fontSize;
                return;
            }

            TextMesh textMesh = go.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.gameObject.SetActive(true);
                textMesh.text = text;
                textMesh.color = color;
                textMesh.characterSize = fontSize * 0.033f;
            }
        }

        private IEnumerator AnimateNumber(GameObject go, Vector3 startPosition, Color startColor)
        {
            float elapsed = 0f;
            while (elapsed < lifetime && go != null)
            {
                float t = lifetime <= 0f ? 1f : elapsed / lifetime;
                go.transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * floatDistance, t);

                Color color = startColor;
                color.a = 1f - t;
                SetAlpha(go, color);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            if (go != null)
            {
                go.SetActive(false);
                pool.Enqueue(go);
            }
        }

        private static void SetAlpha(GameObject go, Color color)
        {
            TextMeshPro tmp = go.GetComponent<TextMeshPro>();
            if (tmp != null && tmp.enabled)
            {
                tmp.color = color;
                return;
            }

            TextMesh textMesh = go.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.color = color;
            }
        }
    }
}
