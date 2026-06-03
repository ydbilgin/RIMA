ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: query RIMA canon first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read allowed: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# TASK: Build-architecture feasibility review for a big design+build push (CODE LENS)

RIMA = 2D top-down pixel roguelite, FLOATING ISLAND in a void (cyan rift brand #00FFCC),
5-room demo (3 combat -> reward -> boss 'Penitent Sovereign'), Unity URP 2D + Pixel Perfect
Camera (ref 640x360) + URP 2D Lights, PPU 64. You are the CODE/ARCHITECTURE reviewer. Do NOT
write code. Tell me the cleanest way to BUILD the following in the EXISTING codebase, cite files.

## Scope to assess (the user wants all buildable code infra built now, rest as placeholders)
For EACH, give: cleanest architecture in current code (file-cited), autonomous-safe to build NOW
with placeholders Y/N, what NEEDS a design lock first, and concrete risks/contradictions.

1. **Story-driven / room-driven LIGHTING** — URP 2D Lights already in scene. How to make lighting
   data-driven per room/mood (e.g. a RoomLightingProfile SO + RoomLoader hook) instead of hardcoded?
   Where do the existing Light2D objects live / how are they wired?
2. **Connected-room MAP system** — RoomLoader + DungeonGraph + Gate exist. How to make 5 rooms feel
   CONNECTED (shared landmark / bridge / consistent transition) in code? What wiring is missing?
   (cite RoomLoader.cs, DungeonGraph, MapProgressController.cs)
3. **Game-screen UI framework** — is there a shared UI base (RimaUITheme?) or is each screen ad-hoc?
   Recommend a consistent approach for menu / HUD / draft / death / victory + how to slot in
   Codex-generated background images (correct size, point-filter, PPU 64).
4. **Game-feel code hooks** — VFXRouter (entries empty), input buffering + coyote time, directional
   camera impulse. Where in code (PlayerController, CameraFollow/CameraPunchController, VFXRouter)
   do these hook in cleanly?

## DELIVER (write result to the DONE file, concise, file-cited)
- Per-item architecture recommendation + autonomous-safe Y/N + needs-design-lock note.
- A single ORDERED build sequence (what to code first to avoid rework), grouping autonomous-safe
  code vs design-locked work.
- Top risks / contradictions with current systems (file-cited). No fluff.
