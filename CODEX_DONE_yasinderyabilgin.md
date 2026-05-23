Task H Fix complete.

Changed:
- Updated Assets/Scripts/Core/RuntimeRoomManager.cs to use RIMA.Save.CheckpointManager and RIMA.Save.CheckpointData.
- Deleted Assets/Scripts/Core/CheckpointSystem.cs.
- Deleted Assets/Scripts/Core/CheckpointSystem.cs.meta.

Validation:
- rg "CheckpointSystem" Assets/Scripts/ returned no matches.
- rg "RIMA\.CheckpointData" Assets/Scripts/ returned no matches.
- CheckpointSystem.cs and .meta are absent.
- Unity AssetDatabase refresh + compile requested and completed; editor state idle / not compiling.
- Unity console error query after compile returned 0 errors.

BLOCKED: no.
