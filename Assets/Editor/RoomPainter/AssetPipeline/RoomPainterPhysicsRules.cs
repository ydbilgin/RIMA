using System;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public struct PhysicsConfig
    {
        public bool isBlock;
        public RigidbodyType2D bodyType;
        public ColliderShape colliderShape;
        public bool isTrigger;
        public string physicsLayerName;
        // D2: 6-layer architecture field (2026-05-27)
        public RoomLayer layer;

        public PhysicsConfig(bool isBlock, RigidbodyType2D bodyType, ColliderShape colliderShape, bool isTrigger, string physicsLayerName, RoomLayer layer = RoomLayer.Floor)
        {
            this.isBlock = isBlock;
            this.bodyType = bodyType;
            this.colliderShape = colliderShape;
            this.isTrigger = isTrigger;
            this.physicsLayerName = physicsLayerName;
            this.layer = layer;
        }
    }

    public static class RoomPainterPhysicsRules
    {
        private static readonly (string Keyword, PhysicsConfig Config)[] Rules =
        {
            // D2: cliff face decor (L3, no collider) — MUST come before "cliff" to avoid premature match
            ("mounting", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default", RoomLayer.Cliff)),
            ("vine",     new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default", RoomLayer.Cliff)),
            ("chain",    new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default", RoomLayer.Cliff)),
            // D2: walkable decor (L4, no collider) — before generic "stone", "dirt", etc.
            ("rune_circle",  new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default", RoomLayer.Decals)),
            ("bone_cluster", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default", RoomLayer.Decals)),
            // D2: wall blocker (L5, BoxCollider2D solid)
            ("statue",   new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle", RoomLayer.Wall)),
            ("pedestal", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle", RoomLayer.Wall)),
            ("plinth",   new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle", RoomLayer.Wall)),
            ("wall", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle")),
            ("cliff", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle")),
            ("pillar", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Capsule, false, "Obstacle")),
            ("column", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Capsule, false, "Obstacle")),
            ("door", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Obstacle")),
            ("altar", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Prop")),
            ("brazier", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Circle, false, "Prop")),
            ("banner", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("prop", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Prop")),
            ("ritual", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Prop")),
            ("enemy", new PhysicsConfig(true, RigidbodyType2D.Dynamic, ColliderShape.Capsule, false, "Enemy")),
            ("npc", new PhysicsConfig(true, RigidbodyType2D.Dynamic, ColliderShape.Capsule, false, "NPC")),
            ("pickup", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, true, "Pickup")),
            ("item", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, true, "Pickup")),
            ("coin", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Circle, true, "Pickup")),
            ("chest", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Interactable")),
            ("floor", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("decal", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("moss", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("crack", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("parallax", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("bg", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("rift", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("sky", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("torch", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, false, "Prop")),
            ("lamp", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("light", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("glow", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("flame", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("ember", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("trigger", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, true, "Trigger")),
            ("zone", new PhysicsConfig(true, RigidbodyType2D.Static, ColliderShape.Box, true, "Trigger")),
            ("tile", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("wang16", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("dirt", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("sand", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("stone", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
            ("cobble", new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default")),
        };

        public static PhysicsConfig Resolve(string assetPath)
        {
            string filename = string.IsNullOrEmpty(assetPath)
                ? string.Empty
                : System.IO.Path.GetFileNameWithoutExtension(assetPath).ToLowerInvariant();
            for (int i = 0; i < Rules.Length; i++)
            {
                if (filename.IndexOf(Rules[i].Keyword, StringComparison.Ordinal) >= 0)
                {
                    return Rules[i].Config;
                }
            }

            return new PhysicsConfig(false, RigidbodyType2D.Static, ColliderShape.Box, false, "Default");
        }
    }
}
