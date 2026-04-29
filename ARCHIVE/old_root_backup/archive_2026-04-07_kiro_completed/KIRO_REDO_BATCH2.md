# KIRO TASK — Redo Batch 2 (Enemies + Icons)
*Updated: 2026-04-07 | Read this file, apply in order. Do not read other files.*
*Warblade and Elementalist REDO removed — handled in Aseprite (base sprite = style lock, user does it).*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## TASK 1 — ShardWalker (REDO)

**Why:** Previous version was solid blue humanoid — the "fractured shards with visible gaps and rift light" feel was lost.

```
mcp__pixellab__create_character(
  name="ShardWalker",
  description="fractured humanoid creature made of broken stone shards held together by vivid blue-purple rift energy, visible glowing gaps between shards where rift light pours through, arm raised in throwing stance ready to launch shards, jagged silhouette with stone debris floating around body, cold stone grey with bright blue-purple rift glow at every fracture gap, dark fantasy roguelite enemy, RIMA universe",
  mode="pro",
  size=64,
  view="low top-down"
)
```

Save: `STAGING/Enemies/Act1/ShardWalker/sprites.zip` (overwrite existing)

---

## TASK 2 — Icon: IronCharge (REDO)

**Why:** Previous version showed a gun barrel. Should be armored shoulder charge / fist impact.

```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, armored steel pauldron shoulder charging forward with explosive blue-purple rift energy burst and impact shockwave lines, showing momentum and force of a body charge, NOT a gun NOT a cannon, dark iron armor piece with rift energy, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```

Save: `STAGING/Icons/Skills/Warblade/IronCharge.png`

---

## TASK 3 — Icon: Shadowstep (REDO)

**Why:** Previous version showed a magic staff. Should be shadow dash with dual afterimage.

```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, two overlapping dark silhouettes showing dash movement, primary figure solid indigo-black and ghost afterimage fading in deep violet behind it, motion blur trail connecting them, instant teleport dash visual with rift energy wisps at start and end points, no staff no weapon, pure movement shadow icon, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```

Save: `STAGING/Icons/Skills/Shadowblade/Shadowstep.png`

---

## TASK 4 — Icon: EvasiveRoll (REDO)

**Why:** Previous version showed a bomb. Should be a rolling dodge with motion trail.

```
mcp__pixellab__create_map_object(
  description="dark fantasy game skill icon, a leather-armored figure in mid-roll dodge position, body curled into rolling shape with motion speed lines trailing behind, rift energy blue-purple streak showing direction of evasive movement, dynamic tumbling dodge pose, dark leather brown and blue-purple rift energy colors, single centered object on transparent background",
  width=64,
  height=64,
  view="side",
  shading="detailed shading",
  outline="single color outline",
  detail="high detail"
)
```

Save: `STAGING/Icons/Skills/Ranger/EvasiveRoll.png`

---

## COMPLETION LOG

When done, append to `STAGING/DONE.txt`:
```
[DONE-REDO2] ShardWalker | character_id | YYYY-MM-DD
[DONE-REDO2] Icon/IronCharge | object_id | YYYY-MM-DD
[DONE-REDO2] Icon/Shadowstep | object_id | YYYY-MM-DD
[DONE-REDO2] Icon/EvasiveRoll | object_id | YYYY-MM-DD
```
