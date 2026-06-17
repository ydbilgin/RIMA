using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Shop
{
    /// <summary>
    /// Spawns 3 ShopStands in the Merchant room and immediately opens exit doors
    /// by fast-forwarding the RoomRunLifecycle so the player can always leave.
    ///
    /// Lifecycle contract:
    ///   RoomRunDirector.BuildCurrentRoom() calls lifecycle.BeginCombat() before
    ///   StartRoomEncounter(). In StartRoomEncounter the Merchant branch calls
    ///   ShopRoomController.Setup(director) which:
    ///     1. Spawns 3 stands at world positions relative to the room center.
    ///     2. Calls director.OpenMerchantExitDoors() (internal bridge) to
    ///        advance lifecycle Combat → Cleared → RewardTaken → DoorOpen and
    ///        opens the exit doors immediately.
    /// </summary>
    public sealed class ShopRoomController : MonoBehaviour
    {
        // Relative stand positions (world-space offsets from room center).
        // Merchant room is 16x12; stands are placed in a gentle arc.
        private static readonly Vector3[] StandOffsets = new Vector3[]
        {
            new Vector3(-2.0f,  0.5f, 0f),  // left stand
            new Vector3( 0.0f,  1.0f, 0f),  // center stand
            new Vector3( 2.0f,  0.5f, 0f),  // right stand
        };

        private readonly List<ShopStand> stands = new List<ShopStand>();
        private Vector3 roomCenter;

        /// <summary>
        /// Called once by RoomRunDirector when the current node is Merchant.
        /// </summary>
        public void Setup(Vector3 center)
        {
            roomCenter = center;
            SpawnStands();
        }

        private void SpawnStands()
        {
            // Build the 3 offer instances.
            ShopOfferData[] offers = new ShopOfferData[]
            {
                ShopOfferData.CreateHeal(),
                ShopOfferData.CreateDamageBoost(),
                ShopOfferData.CreateMaxHPBoost(),
            };

            stands.Clear();

            for (int i = 0; i < offers.Length; i++)
            {
                Vector3 worldPos = roomCenter + StandOffsets[i];

                GameObject standGO = new GameObject($"ShopStand_{offers[i].id}");
                standGO.transform.position = worldPos;

                // Visual: a coloured square sprite so the stand is visible in-game.
                SpriteRenderer sr = standGO.AddComponent<SpriteRenderer>();
                sr.sprite             = CreateStandSprite(offers[i].id);
                sr.sortingLayerName   = "Entities";
                sr.sortingOrder       = 2;
                standGO.transform.localScale = new Vector3(0.6f, 0.6f, 1f);

                // The stand logic (requires CircleCollider2D via [RequireComponent]).
                ShopStand stand = standGO.AddComponent<ShopStand>();
                stand.Initialize(offers[i]);
                stands.Add(stand);

                // Placeholder polish: gentle bob + slow rotation so stands read as intentional.
                // Remove once real art/animation is in place.
                var floater = standGO.AddComponent<RIMA.PlaceholderFloat>();

                Debug.Log($"[ShopRoomController] Spawned stand [{i}] '{offers[i].displayName}' at {worldPos}");
            }
        }

        private static Sprite CreateStandSprite(ShopOfferId id)
        {
            // Create a simple 8x8 coloured pixel as a stand indicator.
            // Replace with real sprites when assets are ready.
            // Colors drawn from RIMA UI palette so they read as "intentional system" not broken.
            Color color = id switch
            {
                ShopOfferId.Heal         => new Color(0.0f, 1.0f, 0.8f, 1f),   // #00FFCC teal — RIMA CharSelectCyan
                ShopOfferId.DamageBoost  => new Color(0.95f, 0.74f, 0.24f, 1f), // Gold — RIMA RimaUITheme.Gold
                ShopOfferId.MaxHPBoost   => new Color(0.28f, 0.88f, 1.0f, 1f),  // #47E0FF cyan — RIMA RimaUITheme.Cyan
                _                        => new Color(0.28f, 0.88f, 1.0f, 1f),
            };

            Texture2D tex = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            tex.hideFlags = HideFlags.HideAndDontSave;

            Color[] pixels = new Color[64];
            for (int i = 0; i < 64; i++) pixels[i] = color;
            tex.SetPixels(pixels);
            tex.Apply();

            Sprite spr = Sprite.Create(tex, new Rect(0f, 0f, 8f, 8f), new Vector2(0.5f, 0.5f), 16f);
            spr.hideFlags = HideFlags.HideAndDontSave;
            return spr;
        }

        public void Cleanup()
        {
            // Clean up any surviving stand GameObjects when this controller is destroyed
            // (e.g. on room advance / scene transition).
            for (int i = stands.Count - 1; i >= 0; i--)
            {
                if (stands[i] != null)
                {
                    stands[i].gameObject.SetActive(false);
                    DestroyRuntimeObject(stands[i].gameObject);
                }
            }

            stands.Clear();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private static void DestroyRuntimeObject(GameObject target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }
    }
}
