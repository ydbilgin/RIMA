---
status: REFERENCE
faz: 1
tarih: 2026-05-04
ozet: "Gate/socket ve harita reveal blueprint"
---
# RIMA Gate Socket and Map Reveal Blueprint

Status: design blueprint, not implementation lock
Date: 2026-05-04
Audience: AI agents first; keep compact.

## Gate Concept Image Read

Reference: `TASARIM/UI_CONCEPTS/rima_gate_socket_concept_2026-05-04.png`

Top gate cards = visual vocabulary, not final individual assets.
Bottom `MULTI-GATE ROOM EXAMPLE` = layout schematic, not normal gameplay camera.

Use image for:
- room-type gate language
- frame/light/icon differences
- 1/2/3+ gate possibility
- fogged unrevealed path idea

Do not use image as proof that RIMA camera should show the whole room from far away.

## Best Gameplay Camera

Default combat view should stay close enough for player/enemy readability.

Recommended:
- normal combat camera shows player, nearby threats, and enough floor for dashing
- shell edges/gates can be visible when near screen edge
- after room clear, camera may slightly reframe or zoom out to show valid gates
- zoom-out should be small; do not make character tiny
- if a valid gate is off-screen, use subtle edge marker or objective arrow

Avoid:
- constant full-room overview
- camera so distant that combat feel becomes board-game-like
- gate selection UI that replaces in-world thresholds with floating arrows

## Gate Visual Contract

Gates are in-world thresholds placed on authored shell edges or meaningful interior thresholds.

Allowed forms:
- stone arch
- wall breach
- stair
- chained doorway
- rift threshold
- lift
- bridge mouth
- shrine passage

Each gate needs:
- neutral readable silhouette
- target room promise through icon/light/frame
- lock/reveal state
- optional route label only in UI overlay, not baked into art

Scale:
- gate should read at gameplay zoom
- rough target: 1.5x-2x character height for main thresholds
- silhouette and glow must carry meaning; fine stone detail is secondary

## Asset Workflow

Generate gates as reusable templates.

Base:
- blank/neutral gate socket with consistent silhouette
- empty interior/mask area
- no baked target icon text

Style fill:
- use inpaint/style-match to change interior glow, seals, props, frame accents
- keep silhouette and collision read consistent
- variants: combat, elite, chest, forge, merchant, event, curse, boss, unrevealed

Animation:
- first frame = closed/dim/fogged/rest
- end frame = open/active/revealed
- use interpolate for 6-8 frame open/reveal loops when possible
- keep glow/fog/rift effects separable from gate base if practical

Unity composition:
- base gate sprite
- overlay icon/light
- trigger/collider
- lock/fog overlay
- optional animated effect layer

Production notes: `TASARIM/UI_PRODUCTION_RULES_FROM_OPUS_REVIEW_2026-05-04.md`

## Map Reveal Rule

`next 1-2 nodes` means graph edges from current node, not screen distance.

Step 1:
- direct exits the player can choose now
- should become clear after room clear/reward flow

Step 2:
- exits after one chosen room
- shown only if revealed by fragment or special rule

Hidden:
- do not reveal far route
- do not reveal exact far-node count
- do not show hidden node icons clearly
- fog silhouettes are okay only as mood, not reliable information

Map fragments:
- reveal one or two graph steps ahead from current route context
- can reveal room type for step-2 nodes
- should not reveal outgoing branches from unreached unrevealed nodes unless special upgrade/event

Decision effect:
- player chooses among visible direct gates
- fragments improve planning by revealing near future
- if the player reaches a previously unseen node, its next exits become known by local room/gate
  logic, and further map knowledge still requires fragments

