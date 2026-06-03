using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA
{
    /// <summary>
    /// RIMA → 4. Dungeon Wiring — tek menü komutuyla Hades-style dungeon sistemi kurulumu:
    ///   1. Oda yoksa RoomBuilder.Build(42) ile inşa eder
    ///   2. Systems GO oluşturur + RuntimeRoomManager ekler
    ///   3. 4 DoorTrigger GameObject oluşturur (NSEW)
    ///   4. RuntimeRoomManager'a tüm referansları atar (tilemap, door triggers, enemy prefabs)
    ///
    /// Idempotent — defalarca çalıştırılabilir, mevcut nesneleri korur.
    /// </summary>
    public static class DungeonSetup
    {
        // RoomBuilder ile eşleşmeli
        private const int RoomW  = 32;
        private const int RoomH  = 24;
        private const int WallT  = 2;
        private const int DoorDW = 2;

        private static int DoorXStart => (RoomW - DoorDW) / 2; // 11
        private static int DoorYStart => (RoomH - DoorDW) / 2; // 8

        [MenuItem("RIMA/Utilities/Dungeon Wiring")]
        public static void Run()
        {
            int changes = 0;

            // ── 1. Tilemap referanslarını bul ─────────────────────────────────
            var wallTilemap  = FindTilemap("IsoGrid/Walls",  "Room/Wall");
            var floorTilemap = FindTilemap("IsoGrid/Ground", "Room/Ground");

            // Oda yoksa inşa et
            if (floorTilemap == null || floorTilemap.GetUsedTilesCount() < 10)
            {
                Debug.Log("[DungeonSetup] Oda bulunamadı — RoomBuilder çağrılıyor (seed=42)...");
                RoomBuilder.Build(42);
                changes++;
                wallTilemap  = FindTilemap("IsoGrid/Walls",  "Room/Wall");
                floorTilemap = FindTilemap("IsoGrid/Ground", "Room/Ground");
            }

            if (floorTilemap == null || wallTilemap == null)
            {
                Debug.LogError("[DungeonSetup] Tilemap bulunamadı. Önce RIMA → 3. Build Room çalıştır.");
                return;
            }

            // ── 2. Systems GO + bileşenler ────────────────────────────────────
            var systemsGO = GameObject.Find("Systems") ?? new GameObject("Systems");
            changes += EnsureComponent<HitStop>(systemsGO);
            changes += EnsureComponent<CameraShake>(systemsGO);

            var rrm = systemsGO.GetComponent<RuntimeRoomManager>();
            if (rrm == null)
            {
                rrm = systemsGO.AddComponent<RuntimeRoomManager>();
                Debug.Log("[DungeonSetup] RuntimeRoomManager eklendi.");
                changes++;
            }

            // ── 3. DoorTrigger GameObjects ────────────────────────────────────
            // Kapı açıklığının tilemap world-center pozisyonuna yerleştirilir.
            // Her kapı iç kısma (floor tarafına 1 tile) konumlandırılır.
            var doorN = GetOrCreateDoorTrigger("DoorNorth", DoorDirection.North,
                floorTilemap, new Vector3Int(DoorXStart, RoomH - WallT - 1, 0));
            var doorS = GetOrCreateDoorTrigger("DoorSouth", DoorDirection.South,
                floorTilemap, new Vector3Int(DoorXStart, WallT, 0));
            var doorE = GetOrCreateDoorTrigger("DoorEast", DoorDirection.East,
                floorTilemap, new Vector3Int(RoomW - WallT - 1, DoorYStart, 0));
            var doorW = GetOrCreateDoorTrigger("DoorWest", DoorDirection.West,
                floorTilemap, new Vector3Int(WallT, DoorYStart, 0));
            changes += 4;

            // ── 4. RuntimeRoomManager referanslarını ata ──────────────────────
            var so = new SerializedObject(rrm);
            SetRef(so, "wallTilemap",  wallTilemap);
            SetRef(so, "floorTilemap", floorTilemap);
            SetRef(so, "doorNorth", doorN.GetComponent<DoorTrigger>());
            SetRef(so, "doorSouth", doorS.GetComponent<DoorTrigger>());
            SetRef(so, "doorEast",  doorE.GetComponent<DoorTrigger>());
            SetRef(so, "doorWest",  doorW.GetComponent<DoorTrigger>());

#pragma warning disable CS0618
            var hud = Object.FindObjectOfType<HUDController>();
#pragma warning restore CS0618
            if (hud != null) SetRef(so, "hud", hud);

            // ── 5. Enemy prefabları ata ───────────────────────────────────────
            var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies" });
            if (guids.Length > 0)
            {
                var prop = so.FindProperty("enemyPrefabs");
                if (prop != null)
                {
                    prop.arraySize = guids.Length;
                    for (int i = 0; i < guids.Length; i++)
                    {
                        var path   = AssetDatabase.GUIDToAssetPath(guids[i]);
                        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                        prop.GetArrayElementAtIndex(i).objectReferenceValue = prefab;
                    }
                    Debug.Log($"[DungeonSetup] {guids.Length} düşman prefabı RuntimeRoomManager'a atandı.");
                    changes++;
                }
            }
            else
            {
                Debug.LogWarning("[DungeonSetup] Assets/Prefabs/Enemies/ içinde prefab bulunamadı.");
            }

            so.ApplyModifiedProperties();

            // ── 6. Dirty + bilgi ──────────────────────────────────────────────
            EditorUtility.SetDirty(systemsGO);
            EditorSceneManager.MarkAllScenesDirty();
            Debug.Log($"[DungeonSetup] TAMAMLANDI — {changes} değişiklik. " +
                      "Ctrl+S ile kaydet, ardından RIMA → Combat Test Setup → Play.");
        }

        [MenuItem("RIMA/Utilities/Dungeon Wiring", true)]
        private static bool Validate() => !Application.isPlaying;

        // ─── Helpers ──────────────────────────────────────────────────────────

        private static Tilemap FindTilemap(string path1, string path2)
        {
            var go = GameObject.Find(path1) ?? GameObject.Find(path2);
            return go != null ? go.GetComponent<Tilemap>() : null;
        }

        private static int EnsureComponent<T>(GameObject go) where T : Component
        {
            if (go.GetComponent<T>() == null) { go.AddComponent<T>(); return 1; }
            return 0;
        }

        private static GameObject GetOrCreateDoorTrigger(
            string goName,
            DoorDirection dir,
            Tilemap tilemap,
            Vector3Int referenceTile)
        {
            var existing = GameObject.Find(goName);
            if (existing != null) return existing;

            Vector3 worldPos = tilemap.GetCellCenterWorld(referenceTile);

            var go  = new GameObject(goName);
            go.transform.position = worldPos;

            var col     = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size      = new Vector2(2.5f, 2.5f); // door gap = 2 tiles; collider biraz büyük

            var trigger = go.AddComponent<DoorTrigger>();
            var tso     = new SerializedObject(trigger);
            tso.FindProperty("direction").enumValueIndex = (int)dir;
            tso.ApplyModifiedProperties();

            Debug.Log($"[DungeonSetup] {goName} oluşturuldu @ {worldPos}");
            return go;
        }

        private static void SetRef(SerializedObject so, string propName, Object obj)
        {
            var prop = so.FindProperty(propName);
            if (prop != null)
                prop.objectReferenceValue = obj;
            else
                Debug.LogWarning($"[DungeonSetup] SerializedProperty '{propName}' bulunamadı.");
        }
    }
}

