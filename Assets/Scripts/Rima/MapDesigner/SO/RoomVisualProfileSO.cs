using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/RoomVisualProfile")]
    public class RoomVisualProfileSO : ScriptableObject
    {
        public string profileId;
        public RoomVisualMode visualMode;
        public bool usesWallKit = true;
        public bool usesTierBBackground = false;
        public Color floorTone = new Color(0.16f, 0.14f, 0.18f);
        public List<PatchAtlasSO> allowedPatchAtlases;
        public List<PropDefinitionSO> allowedProps;
        public string lightingProfileId;
    }

    public enum RoomVisualMode
    {
        DungeonEnclosed,
        HadesArena,
        RuinedCourtyard,
        Shrine,
        RiftChamber,
        BossArena
    }
}
