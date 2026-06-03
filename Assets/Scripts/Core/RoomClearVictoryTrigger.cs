using System.Collections.Generic;
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RIMA
{
    public class RoomClearVictoryTrigger : MonoBehaviour
    {
        private int remainingMobs;
        private bool victoryRaised;
        [SerializeField] private Transform rewardSpawnPoint;
        [SerializeField] private Sprite rewardSprite;
        [SerializeField] private float rewardColliderRadius = 0.45f;

        private bool rewardSpawned;
        private readonly List<(Health health, UnityAction listener)> listeners = new();
        private const string RewardSpritePath = "Assets/Sprites/Environment/Reward/reward_relic.png";
        private const string RuntimeRewardSpritePath = "UI/RIMA/RIMA_UI_Node_Chest";

        private void Start()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                if (enemy == null) continue;

                Health health = enemy.GetComponent<Health>() ?? enemy.GetComponentInChildren<Health>();
                if (health == null || health.IsDead) continue;

                remainingMobs++;
                bool counted = false;
                if (health.OnDeath == null) health.OnDeath = new UnityEvent();

                UnityAction listener = () =>
                {
                    if (counted) return;
                    counted = true;
                    remainingMobs = Mathf.Max(0, remainingMobs - 1);
                    if (remainingMobs == 0 && !victoryRaised)
                    {
                        victoryRaised = true;
                        HandleRoomCleared();
                    }
                };

                health.OnDeath.AddListener(listener);
                listeners.Add((health, listener));
            }

            if (remainingMobs == 0 && !victoryRaised)
            {
                victoryRaised = true;
                HandleRoomCleared();
            }
        }

        private void OnDestroy()
        {
            foreach (var pair in listeners)
            {
                if (pair.health != null && pair.health.OnDeath != null)
                    pair.health.OnDeath.RemoveListener(pair.listener);
            }

            listeners.Clear();
        }

        private void HandleRoomCleared()
        {
            MapFlowManager flow = MapFlowManager.ActiveInstance;
            if (flow != null && flow.HasMapList)
            {
                UnlockSceneExit();
                SpawnRewardPickup();
                return;
            }

            RoomLoader.RaiseDemoComplete();
        }

        private static void UnlockSceneExit()
        {
            DoorTrigger[] doors = FindObjectsByType<DoorTrigger>(FindObjectsSortMode.None);
            bool unlockedDoor = false;

            foreach (DoorTrigger door in doors)
            {
                if (door == null || door.Direction != DoorDirection.North) continue;

                GateBehavior legacyGate = door.GetComponent<GateBehavior>();
                if (legacyGate != null) legacyGate.Unlock(RoomType.Combat);

                RIMA.Environment.Gate gate = door.GetComponent<RIMA.Environment.Gate>();
                if (gate != null) gate.Unlock();

                door.SetActive(true);
                unlockedDoor = true;
            }

            if (unlockedDoor) return;

            foreach (RIMA.Environment.Gate gate in FindObjectsByType<RIMA.Environment.Gate>(FindObjectsSortMode.None))
                if (gate != null) gate.Unlock();
        }

        private void SpawnRewardPickup()
        {
            if (rewardSpawned) return;

            Sprite sprite = ResolveRewardSprite();
            if (sprite == null)
            {
                Debug.LogWarning("[Reward] reward_relic sprite not found; pickup not spawned.");
                return;
            }

            GameObject go = new GameObject("RewardPickup");
            go.transform.position = ResolveRewardSpawnPosition();

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = Color.white;
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 0;

            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = Mathf.Max(0.05f, rewardColliderRadius);

            go.AddComponent<RewardPickup>();
            rewardSpawned = true;
        }

        private Sprite ResolveRewardSprite()
        {
            if (rewardSprite != null) return rewardSprite;

#if UNITY_EDITOR
            rewardSprite = AssetDatabase.LoadAssetAtPath<Sprite>(RewardSpritePath);
            if (rewardSprite != null) return rewardSprite;

            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(RewardSpritePath);
            foreach (Object asset in assets)
                if (asset is Sprite sprite)
                    return rewardSprite = sprite;
#endif

            return rewardSprite = Resources.Load<Sprite>(RuntimeRewardSpritePath);
        }

        private Vector3 ResolveRewardSpawnPosition()
        {
            if (rewardSpawnPoint != null) return rewardSpawnPoint.position;

            Tilemap bestTilemap = null;
            foreach (Tilemap tilemap in FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
            {
                if (tilemap == null) continue;
                string name = tilemap.name.ToLowerInvariant();
                if (name.Contains("floor") || name.Contains("iso"))
                {
                    bestTilemap = tilemap;
                    break;
                }
            }

            if (bestTilemap != null)
            {
                BoundsInt bounds = bestTilemap.cellBounds;
                Vector3Int cell = new Vector3Int(
                    Mathf.RoundToInt((bounds.xMin + bounds.xMax) * 0.5f),
                    Mathf.RoundToInt((bounds.yMin + bounds.yMax) * 0.5f),
                    0);
                return bestTilemap.GetCellCenterWorld(cell);
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return player != null ? player.transform.position : transform.position;
        }
    }
}
