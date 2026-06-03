# Weapon-to-Hand Mount — Triple-AI Synthesis (S6, 2026-05-30)

Sources: **agy** (Gemini, video/footage + transcript) · **codex** (cx, code-grounded) · **Opus** (synthesis + design). User question: the reference clip attaches a weapon to the hand "directly"; but RIMA characters will have **visible drawn hands**, so a directly-pinned separate weapon sprite may misalign. What's the right architecture?

---

## ⚔️ SWING-VISIBILITY DECISION — FINAL (3-AI, 2026-05-30): **Option R, weapon NOT hidden**
The user pushed back on "hide the weapon during the swing." All three confirmed the user is right:
- **agy (sourced):** Hades does NOT hide the weapon — it stays fully visible + trail/smear (3D-to-2D pipeline, Josh Barnett VFX overlay). Industry norm = (V) visible+trail or (S) single smear pose. Full-hide (H) is RARE: it kills weapon identity, breaks the eye's motion tracking, and reduces weight/impact.
- **cx (code):** Option **R** is the smallest/lowest-risk change in our code — fade `weaponRenderer.color.a` to ~0.3-0.5 while `OrientationSync.IsSwinging`; hooks in `HandAnchorAttach` (field :33, swing-start `HandleComboStep` :66/:75, `LateUpdate` restore :96/:106) + `OrientationSync.IsSwinging` (:43/:44). Reduced alpha + LineRenderer slash masks the float-rotation pixel jaggies.
- **Opus:** adopt R. Supersedes the old "hide → painterly flipbook REPLACES weapon" canon. The painterly slash VFX stays — it just reads ON TOP of a faded (not removed) weapon.
- **IMPLEMENTED (2026-05-30, dotnet-green):** `HandAnchorAttach.LateUpdate` now eases `weaponRenderer` alpha to `swingWeaponAlpha=0.4` during the swing and back to 1.0 after (~0.06s). Visual F5 verify + slash-VFX trigger wiring (PlayerAttack→SlashArcVFX) stay gated. `RIMA_DIRECTION_LOCK_S6.md` §5 corrected.

> NOTE: every mention of "hide the weapon during the swing" BELOW is superseded by this block — read it as "fade to ~0.4 + trail."

---

## 0. Honest caveat about the reference video
`https://www.youtube.com/shorts/DXpEIk1EsEU` is **NOT a weapon-attachment tutorial.** It is Challacade's **"Fixing the most common framerate bug"** — a delta-time / frame-independence explainer (transcript confirmed). The weapon-carry you noticed is in the **gameplay footage** (his game *Moonshire*, LÖVE/Love2D, top-down pixel), not the lesson. Two consequences:
- The "direct hand attach" you saw works in his game **because his chibi knight has NO detailed drawn hands** (limbs are blobs / hidden under armor; the hilt merges into the body). He sidesteps exactly the problem you're worried about.
- **Bonus free takeaway from the actual topic:** make sure every RIMA per-frame motion script multiplies by `Time.deltaTime` (weapon swing already does — `OrientationSync.Update():114`). Worth a pass over movement/projectiles for 60 vs 144 Hz parity.

---

## 1. The three AIs converge
| | agy (footage + practice) | codex (our code) | Opus (design) |
|---|---|---|---|
| Naive static HandAnchor for visible hands? | **NOT viable** — hand bobs per-frame, foreshortens per-direction; weapon floats/clips | **NOT viable** — Level1 only adjusts per **direction** (8 values), nothing tracks per **frame** | agree — Level1 is the gap the user intuited |
| Fix path | per-direction anchor LUT + dynamic sort + **hide weapon during swing → slash VFX** | **Rank 1: hybrid** Level2 per-frame `SpriteHandData` for held pose + swing-hide → VFX | agree — this is the codebase's own `Level2SpriteHandData` design |
| Strong alternative | **bake the HAND onto the weapon sprite + handless-when-armed body** → alignment bug eliminated | Rank 3 (bake weapon into body) breaks weapon-swap / `WeaponDatabaseSO` | agy's variant is *better* than cx rank-3: weapon stays swappable |

Key code facts (cx, cited): live `Player.prefab:253-262` uses **Level1**; sort is per-direction only (`HandAnchorAttach:181-187`) — no per-frame sort/mask/hand-overlay exists; baking weapon into body fights `WeaponDatabaseSO:8-20`.

---

## 2. The pivotal decision (this determines everything): **where does the gripping hand live?**

**Option A — hand on the BODY (visible hands always drawn on the character):**
- Requires the separate weapon to align to that drawn hand every frame → **Level2 `SpriteHandData`** (already coded) + a **hand-anchor authoring tool** to mark the hand pixel on each frame.
- Swing: hide the weapon renderer (around `OrientationSync.IsSwinging`) → painterly slash VFX (locked design). So only **idle + walk held poses** need alignment, not the swing.
- Cost: tool + per-frame data (8 dir × N frames × forms × classes). Demo (Warblade only) = manageable; full 10-class = blocked without the tool.

**Option B — hand baked onto the WEAPON, body is handless-when-armed (agy's pick):**
- The weapon sprite includes its own gripping hand; the armed body draws no holding hand. Pin + rotate the hand+weapon as ONE unit per direction — **reuses existing OrientationSync, no per-frame body-hand data.**
- Weapon stays swappable (each weapon = its own 8-dir hand-included sprite). Eliminates alignment bugs entirely.
- Cost: each weapon needs hand-inclusive 8-dir art; the off-hand can still be on the body. Best fit for **weapon-draft** scaling.

> RIMA's body is **already weaponless** (locked). Option B is the smaller step from where we are — but it conflicts with "characters HAVE visible hands" IF you meant the holding hand. Reconcile: draw the **off-hand on the body**, bake the **weapon-hand into the weapon**. Best of both.

---

## 3. Opus recommendation (ranked)
1. **Demo (1 class Warblade): Option B-lite.** Weaponless body (as today) + a Warblade weapon sprite with the grip-hand baked in, pinned/rotated per-direction via the existing `OrientationSync` (8 offsets/rotations already there), + hide-during-swing → slash VFX. **Near-zero new code**; no authoring tool needed for the demo. Add per-direction sort (already present) + verify N/NE/NW behind-body.
2. **Full vision (10 classes + draft): keep Option B** as the spine (hand-on-weapon scales with weapon-swap), and only adopt **Level2 (Option A)** selectively for classes that demand expressive body-hands.
3. **Avoid:** baking the weapon into the *body* sprite (cx rank-3) — content explosion + breaks `WeaponDatabaseSO` swapping.

**Hard constraints both options must honor (agy):**
- No single uniform anchor for all 8 dirs — per-direction LUT (we have it).
- No arbitrary float rotations on pixel art mid-move (double-pixel jaggies) — snap to the 8 dirs or pixel-friendly steps; the swing arc is OK because the weapon is hidden→VFX during it.
- Dynamic per-direction sort (weapon behind body for N/NE/NW) — present; extend if a frame needs hand-over-weapon.

---

## 4. Concrete next steps (when art direction is chosen)
- **If B-lite (recommended for demo):** produce Warblade weapon sprite WITH grip-hand (8-dir, or 5+3 mirror), wire to `HandAnchorAttach` Level1, add weapon-renderer hide/show around `OrientationSync.IsSwinging` + slash-VFX trigger. No new system.
- **If A (Level2):** build/restore the **SpriteHandData annotator** (the `OnDrawGizmosSelected` gizmo hints a partial one existed), author Warblade frames, flip `HandAnchorAttach.attachMode` → `Level2SpriteHandData` on `Player.prefab`.
- Either way: **swing-time weapon visibility control + slash-arc flipbook** is shared work and should be built first (it's the piece that makes the held-pose-only alignment sufficient).

**USER DECISION NEEDED:** does the held/gripping hand live on the **body** (→ A, needs tool) or on the **weapon** (→ B, recommended)? Everything downstream forks on this one art-direction call.
