# Microgame / Atolye Research Note
Date: 2026-05-01
Owner mark: CODEX_RESEARCH
Status: research note, not locked design

## Purpose
User asked for a practical read on four X posts/videos:
- Could a workshop-style or simple short-session game be made with our current tools?
- Can PixelLab help enough to make this practical?
- Are there ideas worth preserving for RIMA?
- Can maps/levels for this kind of game be generated automatically?

This note is intentionally written as input for Claude/orchestrator. It should not be treated as a final design decision.

## Evidence Limits
- X post bodies, media URLs, view/like/reply counts were accessible through public tweet mirrors and browser view.
- Video media was downloaded and reviewed through sampled frames.
- Full reply text was not accessible without X login. Browser showed the "join X to read replies" gate. Treat reply analysis as unavailable, not as negative signal.

## Video 1 - AI Sprite Tool / Walkcycle Pipeline
Source:
https://x.com/chongdashu/status/2049430279517810969

Post summary:
The author built a small AI sprite tool because the same walkcycle workflow kept repeating:
reference sprite -> video model walkcycle -> split frames -> choose start/end frames -> select tweens -> generate spritesheet.

Observed video content:
- A UI previews a pixel character on a green background.
- Candidate animation frames are shown in a strip/grid.
- User appears to select key frames and intermediate frames.
- Final output is effectively a controlled spritesheet workflow.

Practical lesson:
This is highly relevant to RIMA. It is not about "AI makes the game"; it is about converting AI output into a disciplined production pipe. The important design pattern is:
1. Generate or select a strong reference pose.
2. Generate motion candidates.
3. Split into frames.
4. Human-pick readable key poses.
5. Use only approved in-betweens.
6. Export into Unity-ready sheets.

RIMA relevance:
- Directly supports the current PixelLab -> Aseprite QC -> Unity import bottleneck.
- Reinforces existing CODEX.md rules: 128x128, PPU 64, Multiple mode, center pivot, no scale drift.
- Strongly suggests adding frame-pick discipline to the Visual Contract work: every skill/anim should define the readable peak frame, recovery frame, and silhouette requirement.

## Video 2 - Unity Value Experiments / Feel Through Parameters
Source:
https://x.com/moi_rai_/status/2049433406535086531

Post summary:
The developer starts a series about changing a Unity value in their game, jokingly doing things programmers told them not to do.

Observed video content:
- A top-down/isometric roguelite combat scene.
- Unity Inspector values are visible.
- A character/enemy/attack suddenly becomes huge or exaggerated.
- The same game becomes comedic and visually shareable through one parameter change.

Practical lesson:
Game feel and social-share value can come from simple parameter experiments, not just new content. One exaggerated value can create a memorable clip.

RIMA relevance:
- Useful after the current Visual Contract gate.
- Each skill should have both a "production range" and an "experiment range" for:
  - knockback
  - launch height
  - hitstop
  - dash distance
  - target scale
  - VFX radius
  - screen shake
- This can create a cheap internal playtest mode: "break one value and see if the skill becomes more readable or more fun."

Risk:
Do not let funny parameter clips become design decisions. They should feed feel tuning, not override class identity locks.

## Video 3 - Short Physics Challenge / Daily Puzzle Shape
Source:
https://x.com/eztehvibes/status/2049732145858683130

Post summary:
"Is anyone faster in completing this challenge?"

Observed video content:
- Vertical short-form video.
- A small obstacle map with a ball/metal object.
- The player tries to route the object through a compact board.
- Fast restart / time-challenge energy.
- The map is simple, readable, and suitable for repeated attempts.

Practical lesson:
This is the strongest "toilet game" reference from the set. The loop is simple:
look at board -> try route -> fail or finish -> restart -> compare time.

Automation fit:
Very high. This kind of game should not rely on AI drawing whole maps. Better approach:
1. Generate level layout in code from grammar.
2. Validate with a solver or physics simulation.
3. Score difficulty.
4. Skin the board with PixelLab tiles/objects.
5. Ship daily seeds or short challenge packs.

Possible level grammar:
- start
- goal
- fixed blockers
- moving blockers
- bounce pads
- gates
- one-way strips
- narrow channels
- hazard zones

RIMA relevance:
- Can become a side-mode or post-run relic test chamber.
- Can also serve as a production tool: tiny rooms that test one skill affordance at a time.
- Ranger trap-line rooms, Brawler launch rooms, Gunslinger heat rhythm rooms, and Shadowblade phase rooms could all use the same "one mechanic, one short challenge" structure.

## Video 4 - Craft & Deliver / Workshop Loop
Source:
https://x.com/anilmadak/status/2049661498859852204
Steam:
https://store.steampowered.com/app/4085740/Craft__Deliver/

Post summary:
"Build your own workshop! Craft, pack, deliver."

Observed media:
- Cozy garage/workshop art.
- Workbench, tools, boxes, truck/driveway, suburban delivery setting.
- Clear promise: build item, pack item, deliver item.

Steam/store loop read:
- Take order.
- Craft item.
- Pack.
- Deliver.
- Use earnings/progression to improve the workshop.

Practical lesson:
This is feasible with our tools because the asset set is bounded:
- one workshop room
- props and stations
- boxes/packages
- simple customer/order UI
- small delivery map or abstract delivery step

PixelLab fit:
High. PixelLab can create:
- workbench
- tools
- boxes
- shelves
- garage props
- delivery van
- house/front-yard props
- simple NPC/customer sprites

The hard part is not asset creation. The hard part is making each order feel satisfying in 30-90 seconds.

## Feasibility With Our Current Tooling
Yes, these styles are feasible if scope is held tight.

Best scope:
- One main mechanic.
- One screen or one small room.
- 30-90 second session.
- Instant restart or immediate next order.
- Light meta progression.
- Strong visual feedback.

PixelLab should be used for:
- static props
- object variants
- tiles / board skins
- simple character references
- controlled animation candidates

PixelLab should not be trusted for:
- complete procedural level logic
- final frame selection without QC
- scale/pivot consistency unless checked
- game balance
- retention economy

## Recommended RIMA Takeaways
1. Add "shareable peak frame" to Skill Visual Contract.
Every skill should identify the frame a player would screenshot or understand in a 1-second clip.

2. Add "parameter experiment pass" after Visual Contract.
For each class, test exaggerated safe values before final tuning.

3. Consider an endgame/post-run "Workshop / Test Chamber" layer.
Not for Phase 1. Possible later layer:
- player repairs relics between runs
- repaired relics unlock challenge rooms
- challenge rooms test class skills in 30-60 second tasks
- rewards feed cosmetics, relic variants, or optional modifiers

4. Use procedural grammar for micro maps.
Do not AI-generate full maps. Generate logic in Unity, then skin with PixelLab.

5. Treat short-session design as a separate product lens.
RIMA's main roguelite loop is heavier. A short-session layer could still help:
- daily challenge
- skill lab
- endgame forge tests
- relic repair minigames

## Candidate Microgame Concepts

### Mini Workshop Rush
Loop:
take order -> craft correct part -> pack -> deliver -> upgrade station.

Why it works:
Simple, readable, cozy, asset-bounded.

Risk:
Can become shallow unless orders introduce meaningful variation.

### Marble Room Daily
Loop:
one generated board -> route ball/object to goal -> time score -> daily seed.

Why it works:
Best automation fit. Low content cost, high replay value.

Risk:
Needs excellent physics feel and fast restart.

### RIMA Skill Lab
Loop:
select class -> one short skill challenge -> score based on time/style/accuracy.

Why it works:
Directly helps RIMA production and could become later content.

Risk:
Requires combat/skill systems to be stable first.

### Pack It Cozy
Loop:
fit generated objects into a box/truck with constraints.

Why it works:
Good mobile/toilet-game format. Easy to theme as workshop or relic packing.

Risk:
Needs strong tactile input handling.

### One-Room Roguelite Snack
Loop:
one room -> 60 seconds -> 3 skill picks -> score.

Why it works:
Reuses RIMA room/combat systems.

Risk:
Could distract from main Phase 1 if started too early.

## Recommendation
Do not pivot RIMA into a workshop game now.

Best path:
1. Finish current Visual Contract and sprite pipeline gate.
2. Use the sprite-tool lesson immediately for animation discipline.
3. Keep "RIMA Skill Lab / Repair Workshop Endgame" as a later idea.
4. If making a separate small game, choose Marble Room Daily or Mini Workshop Rush first because both are asset-bounded and map/order generation can be automated.

