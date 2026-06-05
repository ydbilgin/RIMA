# Council Question — Modular Map Learnings for RIMA: Prop-Groups + Path-Mask + Terraces (DEEP ARCHITECTURE lens)

You are ONE advisor in a 4-advisor RIMA council (others: Opus design+visual, Gemini 3.5 Flash lean, Codex feasibility joining later). YOUR LENS = DEEP SYSTEMS ARCHITECTURE (data model, pipeline, long-term cost).

READ FIRST (you have file tools):
1. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_council_brief_modular_props.md` — context, current RIMA room system, questions 1-5.
2. View if you can: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\mockups\ref_modular_garden_game_2026-06-05.png` (text description in brief).
3. Optionally inspect: `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs`, `Assets/Data/Props/` (PropDefinitionSO/PropRegistry), `Assets/Scripts/MapDesigner/Room/` RoomTemplateSO.

ANSWER the brief's 5 questions with NET positions. Architecture focus:
- Q1 prop-groups: best data model (PropGroupSO referencing PropDefinitionSO members with offsets + merged footprint? prefab variants? baked into RoomTemplateSO?) — weigh authoring ergonomics vs runtime simplicity vs the project's "no clutter / adopt existing SO" rule. Mirror/variant mechanism. Integration cost into IsoRoomBuilder placement + collider building (PropColliderAutoBuilder is Box-only today).
- Q2 path/decal mask: second tile-type mask layer in RoomTemplateSO vs decal sprites; interaction with iso-cell sizing (0.96×0.585) and the open 32px-tile-drift question; render order/depth-sort (project lock: Camera Custom-Axis (0,1,0), single Entities layer).
- Q3 multi-terrace heightGrid: now / v2 / never — consider depth-sort lock, collision layers (Player/Enemy separation lock), cliff solver rework, camera framing in big rooms.
- Q4 sequencing vs B-12 production RoomBank: embed or separate task; S/M/L sizing; recommended order.
- Q5 any additional steal-worthy technique.

OUTPUT to STDOUT, bullet-style, short, explicit disagreements welcome. English OK.