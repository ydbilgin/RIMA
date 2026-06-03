# Cross-Class System — Design Spec (user-specified, LIVE build)

Status: GREENLIT by user 2026-05-31. Build live ("en mantıklı halini"). Vertical-slice feature.

## Core concept
The player picks a **guest cross-class**. That guest contributes **one signature skill** that becomes a **cross-class add-on bound to one of the player's skill slots**. Pressing that slot summons a **black silhouette** of the guest class that performs the guest skill, then vanishes.

## Behavior (the key spec)
- Pressing the cross-class slot spawns a **black silhouette actor** (solid black tint of the guest class's idle sprite — reuse existing `Resources/Characters/<Guest>/` sprite, material = unlit black/silhouette).
- **Puff-IN** VFX when it appears, **Puff-OUT** VFX when it leaves (small black smoke "puf").
- **If the guest skill is RANGED:** silhouette appears (at the player or slightly offset toward the cursor/target), performs the ranged attack toward the targeted mob from a distance, then puffs out.
- **If the guest skill is MELEE:** silhouette travels next to the target mob, strikes toward it, then puffs out.
- The silhouette is transient (lifetime = skill duration + small tail), does NOT persist or take damage. Pure "summon one move, vanish."

## Lore fusion (free win)
The silhouette = a **Sundered Echo** (NLM canon: the run recovers "Shattered Echoes / forgotten faces"). Cross-class = calling the echo of another hero to strike once. This makes a mechanic also a story beat — keep this framing in VFX/audio (cold cyan rim on the black silhouette as it forms/dissolves).

## Sensible defaults (Opus-decided; user said "en mantıklı") — confirm only if a reviewer objects
- **Acquisition:** for the demo, cross-class is chosen once (class-select secondary pick, or a draft "Echo" card). Start with: a draft-granted "Echo of <Class>" card that binds the guest signature skill to a free slot (or the ult slot). Keep selection data-driven so meta-unlock can layer on later.
- **Slot binding:** binds to a dedicated cross-class action (reuse an open skill-bar slot; the registry already supports Skill1-4 + ult). Default: ultimate-adjacent slot, single guest skill.
- **Cost/cooldown:** longer cooldown than native skills (it's a "guest favor"), no native resource cost (or small). Tunable.
- **Ranged vs melee detection:** read from the guest skill's own metadata (range/archetype) — the same data the skill already uses; do NOT hardcode per class.
- **Targeting:** melee → nearest enemy to cursor/aim; ranged → toward cursor/aim like native cursor-aim.

## Implementation surface (for the impl plan)
- A `CrossClassEcho` MonoBehaviour/actor: spawns silhouette GO, plays puff-in, moves (ranged: stay/offset; melee: dash to target), invokes the guest skill's effect (reuse the guest skill controller/behavior so damage/break logic is identical to the real class — must respect the P0 "Enemy" layer + Sundered-Beat break), plays puff-out, despawns.
- Silhouette visual: instantiate guest idle sprite, swap material to an unlit black (+ optional cyan rim), set sorting on "Entities"/Pivot (Custom-Axis Y-sort per project rule — NOT manual sortingOrder).
- Puff VFX: small black smoke burst (reuse/extend existing VFX system; painterly, cyan-tinted edge; pixelated-particle rules).
- Binding: extend the skill/draft data so a slot can hold a "guest skill ref + isCrossClass" flag.
- Reuse the guest class's existing skill behavior so we don't reimplement 10 skills — the echo just *performs* an existing skill from a temporary actor.

## Constraints (project)
- Cursor-aim KEEP. Enemy detection uses layer **"Enemy"** (post-P0). Y-sort via Custom-Axis (Entities/Pivot), never manual sortingOrder. Painterly VFX. PixelLab sprites (silhouette = recolor of existing, no new gen). Compile in Unity (not just dotnet) after each change.
