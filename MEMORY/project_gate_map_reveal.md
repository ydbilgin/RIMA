---
topic: project_gate_map_reveal
updated: 2026-05-04
---

# Project Gate And Map Reveal

Use when: gates, door sockets, map fragment, partial map, route reveal, camera framing.

Primary doc:
- `TASARIM/GATE_SOCKET_AND_MAP_REVEAL_BLUEPRINT_2026-05-04.md`

Reference:
- `TASARIM/UI_CONCEPTS/rima_gate_socket_concept_2026-05-04.png`

Rules:
- Gate concept sheet is visual vocabulary, not final gameplay camera.
- Combat camera should not be constant full-room overview.
- Gates are reusable templates: neutral base, inpainted style fill, animated open/reveal via
  first/end frame interpolation.
- Gate meaning should come from silhouette + glow + overlay; fine detail is secondary.
- Main gate thresholds should read around 1.5x-2x character height at gameplay zoom.
- `next 1-2 nodes` means graph edges from current node.
- Hidden far nodes should stay fogged; map fragments reveal near future, not full route.
