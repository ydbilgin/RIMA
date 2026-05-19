---
name: RIMA Cross-Class Skill System
description: Mechanics for multi-class skill drafting and Ghost VFX.
type: project
---
* **Pool:** 10 classes x 8 exportable skills (80 total).
* **Limit:** Max 2 cross-class slots per run.
* **Draft:** Room clear -> 3 random class offers -> Choose 1.
* **Ghost VFX:**
  - Visual: Semi-transparent class sprite appears during skill use, 0.6s fade.
  - Colors: Warblade (Blue), Ronin (White), Gunslinger (Gold), Summoner (Venom).
* **Infrastructure:** `CrossClassSkillManager.cs`, `CrossClassGhost.prefab` (DONE).
