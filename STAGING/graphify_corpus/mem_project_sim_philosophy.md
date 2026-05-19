---
name: RIMA Simulation Philosophy
description: Data-driven combat balance rules.
type: project
---

# CORE RULE
* Every battle (Room/Elite/Boss) must be simulated.
* Decisions = Data > Intuition.
* Sync: rima_sim.py output -> SINIF doc values.

# LAYERS
* v3 (Current): Std (5x200HP), Elite (2x500HP). 4-Act Boss Matrix.
* v4 (Planned): Architecture scaling (Corridor=0.3, Corner=1.3).

# CALIBRATION TARGETS (SURVIVAL)
* Act 1-4 Boss: 80% > 60% > 40% > 25%
* Rooms (Std/Elite): 95% / 85%
