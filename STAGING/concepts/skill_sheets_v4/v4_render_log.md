# v4 Skill Sheet Render Log

Date: 2026-05-21
Generator: Codex built-in image generation tool
Requested model/path: gpt-image-1 style generation per task prompt
Output format: PNG
Requested size: 1280x1280
Actual generator size: 1254x1254
Post-process: copied each generated PNG from Codex image cache to project staging, then resized staged copies to 1280x1280 with Pillow LANCZOS. Originals were left in the Codex image cache.
Destination directory: STAGING/concepts/skill_sheets_v4
Retries: none

Shared negative prompt: flat icon grid, symbolic icon, static portrait, left portrait panel, tiny isolated skill/weapon icons, no mobs, modern UI overlay, card UI, photographic, anime cel shading, pure side view, pure top down, text-heavy infographic, watermark.
Shared style prompt: crisp RIMA canonical pixel art mood, 2x2 grid of in-action combat snapshots, chibi ARPG combat sprites at 64-128px scale, 30-35 degree angled isometric camera, Act 1 dark granite floor, subtle cyan rift accent, stone wall hints, Hades + Diablo synthesis mood without cloning either.

## Outputs

1. 01_warblade_v4_in_action.png
   Skills: Cleave / Iron Crush / Earthsplitter / Blade Rush
   Prompt summary: Warblade with visible oversized greatsword in four active combat scenes versus Bone Walker, Bone Archer, Skull, and Imp Demon; wide cleave arc, downward crush, cyan earth fracture, forward dash trail.

2. 02_ronin_v4_in_action.png
   Skills: Quickdraw / Iaido Stance / Sakura Veil / Final Draw
   Prompt summary: Ronin katana fighter in four active combat scenes versus Bone Walker, Specter Ghost, Wraith Specter, and Goblin; quickdraw slash, pre-draw counter stance, petal veil cut, decisive final draw.

3. 03_gunslinger_v4_in_action.png
   Skills: Twin Fire / Ricochet Shot / Fan The Hammer / Dead Eye
   Prompt summary: Leather Gunslinger with visible twin flintlock pistols in four active combat scenes versus Cyan Slime, Bone Archer, Bat swarm, and Imp Demon; twin shots, ricochet trail, rapid hammer fan, focused piercing shot.

4. 04_ranger_v4_in_action.png
   Skills: Aimed Shot / Black Arrow / Bone Trap / Barbed Net Shot
   Prompt summary: Ranger with visible longbow and quiver in four active combat scenes versus Skull, Specter Ghost, Dungeon Rat, and Goblin; aimed line shot, black-cyan arrow, floor bone trap, weighted barbed net arrow.

5. 05_elementalist_v4_in_action.png
   Skills: Glacial Spike / Prism Beam / Meteor / Frozen Orb
   Prompt summary: Staffless Elementalist with floating golden rune disc in four active combat scenes versus Imp Demon, Bone Archer, Skull, and Cyan Slime; ice line, radiant beam, meteor impact, slow ice orb burst.

6. 06_shadowblade_v4_in_action.png
   Skills: Phase Step / Severance / Smoke Veil / Night Aperture
   Prompt summary: Shadowblade with twin reverse-grip short blades in four active combat scenes versus Bone Walker, Wraith Specter, Goblin, and Specter Ghost; phase-through scar, scar collapse line, smoke ambush, mirrored aperture dash.

7. 07_ravager_v4_in_action.png
   Skills: Berserk / Axe Throw / Earthcrack / Whirlwind
   Prompt summary: Ravager with dual compact hatchets in four active combat scenes versus Bone Walker, Bone Archer, Skull, and Imp Demon; blood cyclone frenzy, spinning thrown hatchet, cyan ground crack, two-hatchet whirlwind.

8. 08_hexer_v4_in_action.png
   Skills: Curse Mark / Decay Aura / Shackle Curse / Death Wail
   Prompt summary: Hexer in purple-black hooded robe with green-flame curse staff and chained grimoire in four active combat scenes versus Bone Walker, Cyan Slime, Goblin, and Specter Ghost; curse stamp, decay aura, spectral chains, ghostly wail cone.

9. 09_brawler_v4_in_action.png
   Skills: Uppercut / Earth Slam / Body Lock / Knockout
   Prompt summary: Unarmed Brawler with leather hand wraps and orange charge accents in four active combat scenes versus Skull, Dungeon Rat, Goblin, and Imp Demon; launching uppercut, floor shockwave, grappling throw, charged hook knockout.

10. 10_summoner_v4_in_action.png
    Skills: Summon Wisp / Spirit Bind / Beacon / Sacrifice
    Prompt summary: Summoner with soul lantern and conductor gesture in four active combat scenes versus Bat, Wraith Specter, Bone Archer, and Skull; wisp summon, spectral tether, command beacon with minions, sacrificial soul-flame explosion.

## Source Notes

Skill choices were constrained to STAGING/concepts/skill_sheets_v3/skill_enumeration_v3.json. Class visual identity details for the six unspecified classes were checked through NotebookLM query 30ddffa5-292f-4248-8e77-68074af901be.
