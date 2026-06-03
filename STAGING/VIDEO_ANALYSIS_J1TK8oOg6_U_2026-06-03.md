# Video Analysis — https://youtu.be/J1TK8oOg6_U → RIMA synthesis
Date: 2026-06-03 · Analyst: research sub-agent (Opus reads & decides)
Tool that accessed the video: **ax → Gemini 3.1 Pro (High)** (native YouTube analysis, succeeded — no fallback needed). RIMA canon cross-checked via NLM notebook `30ddffa5`.

---

## 1. Video summary (what it actually is)
- **Channel:** Game Design Library
- **Format:** Game-design video essay / analysis (~6 min 24 s).
- **Subject:** Breaks down **5 core mechanics/design choices in *Mina the Hollower*** (Yacht Club Games — the Shovel Knight studio's Game-Boy-styled top-down action-adventure).
- **Thesis:** Even when a game is built on challenging, high-friction retro inspiration (and Souls-like punishment), good designers **streamline mechanics and soften the punishing edges** — granular accessibility, low-commitment items, and forgiving currency recovery let players take risks and explore without grueling, repetitive frustration.

Note: *Mina the Hollower* is a **linear top-down action-adventure (Zelda-like), NOT a roguelite.** So its systems are inspiration, not 1:1 templates — the value is in the *design principles*, which port cleanly even when the genre doesn't.

---

## 2. Raw ideas extracted (from ax-Gemini, verbatim structure)

1. **Burrow → Movement** [0:00–0:48] — Burrow underground for speed + projectile-invulnerability (still melee-vulnerable); emerging grants a longer/higher jump used for platforming gaps and passing under walls.
2. **Burrow → Combat** [0:48–1:10] — Outrun long attacks, dive *under* shield/tracking enemies to flank them, or burrow under rocks to pick up & throw for big damage.
3. **Burrow → Puzzles** [1:10–1:36] — Implicit + explicit puzzles built on the burrow verb (e.g. a town guard blocks the path; the player intuits they can just burrow under him).
4. **Extensive gameplay modifiers** [1:36–2:40] — Deep list of difficulty/accessibility sliders (damage dealt/taken, AI tracking, healing strength). Warning: too much granularity can let players overwhelm themselves or trivialize the game into boredom.
5. **Assist Mode (curated modifier preset)** [2:40–3:04] — One "Assist Mode" toggle auto-selects a sane batch of modifiers = easy-mode preset, taming the overwhelming slider list. Critique: it's buried at the bottom of the menu where players miss it.
6. **Side Arms (disposable utility weapons)** [3:04–4:07] — 15 secondary weapons scattered on the map (ranged dmg, status effects, niche movement). Carry only **1** at a time; swapping, leaving the area, or dying loses the old one. Lesson: disposability **forces players to actually use items instead of hoarding** them.
7. **"Bone Up" (streamlined level-up)** [4:07–4:58] — Collect enough bones → game pauses → choose **1 of 4** upgrades (dmg, defense, etc.). A closed progression loop that keeps struggling players engaged: keep fighting, keep getting stronger.
8. **Forgiving Souls-like currency checkpoint** [4:58–6:22] — Bones are a Souls-like currency at risk on death, BUT you only *permanently* lose them if you **die twice in a row**. Die once, reach the next checkpoint on the next life → bones saved. Removes the misery of corpse-runs into optional dangerous side paths.

**Quick numbers:** 3 burrow uses · 15 side arms · carry 1 · level-up = 1-of-4 · lose currency only after 2 deaths in a row.

---

## 3. RIMA-fit synthesis (idea-by-idea)

| # | Video idea | RIMA canon today | Fit verdict |
|---|------------|------------------|-------------|
| 1-3 | Burrow as one verb, 3 uses (move/combat/puzzle) | RIMA has no burrow; movement = dodge/dash. Signature verb = **Sundered Beat (BREAK→EXECUTE)**. | **Principle fits, mechanic doesn't.** Don't add burrowing. DO steal the *philosophy*: make Sundered Beat (and dash) pay off in multiple contexts, not one. See rec #4. |
| 4 | Granular difficulty sliders | RIMA = **4 fixed modes** (Echo/Rift/Fracture/Void) + mid-run Curse Gates + Grudge/Nemesis. No granular sliders. | **Partial fit / deliberately diverge.** The video itself *warns* sliders cause self-sabotage. RIMA's fixed-mode + Curse-Gate model is arguably better-curated. Don't build a slider screen. But the *accessibility* sliders (screen-shake amplitude, outline thickness) RIMA already plans — keep those. |
| 5 | Assist Mode = one curated preset, surfaced well | RIMA's **Echo mode** (-25% enemy HP, -15% CD) IS exactly this. | **Already done — validate the lesson.** The actionable bit: the video's *only* critique is Assist Mode being buried. **Surface Echo mode prominently** at class-select / first run, don't hide it. Cheap UX win. |
| 6 | Side Arms — disposable, 1-at-a-time floor pickups | RIMA **hard-bans** item grids, backpacks, floor-looted weapons, stored consumables (potions are instant-use shop buys). | **Conflict — do NOT adopt as-is.** It violates a canonical lock. The *transferable kernel* = "disposable forces use, not hoarding," which RIMA already honors (instant-use consumables). Optional reframe: see rec #5 (a single temporary "Rift Charge" active), but flagged low-priority/risky. |
| 7 | "Bone Up" = pause → 1-of-4 upgrade draft | RIMA is **actively building a 3-card Hades reward draft** (RewardPickup → DraftManager.ShowDraft). | **Strong fit — already on the right path.** Validates the current build. Actionable nuance: video does *pause-on-pickup* mid-combat-ish + closed loop. Confirms 3-card draft is correct; consider the "keep-them-engaged-while-struggling" framing. Mostly a confidence signal, low new work. |
| 8 | Forgiving currency: lose only on **2nd consecutive death** | RIMA's **Bones/Death-Marker** system is currently *lore/corpse-marker only* (Dark Souls bloodstain), with vague planned interactions. Run currencies (Gold, Shards) are 100% lost on death; Echoes are permanent. | **BEST FIT — fills a real gap.** RIMA has the bone *marker* but no currency-recovery *rule*. This idea hands RIMA a concrete, lore-perfect, low-risk mechanic. See TOP PICK. |

---

## 4. Prioritized recommendations (RIMA-specific, value/effort ordered)

### R1 — Death-Marker currency recovery ("avenge the fallen" → bone retrieval) — **HIGH value / LOW-MED effort** ⭐
RIMA already plans a bone/skeleton death-marker at the death spot. Give it a *function* borrowed from the video: when you die, your lost run-currency (a portion of Gold/Shards) is "imprinted" on the marker. On the **next** run, reach/clear that room → reclaim it. If you die again *before* reclaiming, it's gone for good (the video's "2-deaths-in-a-row" rule, adapted to roguelite per-run structure).
- **Why it fits RIMA specifically:** Lore is already written — markers = "the failed containers." Reclaiming = "absolution." This is the video's #8 + RIMA's existing #6 plan, fused. Zero canon conflict; it *completes* a planned-but-undesigned system.
- **Roguelite adaptation note:** Mina is linear so "next life same level" works directly. RIMA rooms are random per run, so anchor recovery to **room TYPE / depth or the Hub-side marker tally**, not exact geometry. Simplest MVP: a single "Echo Debt" stored at death; first elite/combat clear next run refunds it; dying again first forfeits it.
- **Effort:** Data + one manager hook + a marker-interaction. No new art beyond the already-planned bone sprite.

### R2 — Surface Echo (assist) mode prominently — **MED value / TRIVIAL effort**
Video's lone critique of Mina = Assist Mode buried at menu bottom. RIMA's Echo mode is the equivalent. Put difficulty mode (esp. Echo) **on the class-select / new-run screen**, not deep in options. One line of UX placement. Directly actions the video's most concrete lesson.

### R3 — Reframe the 3-card draft as the "struggle → power" engagement loop — **MED value / TRIVIAL effort (confirmation)**
The current 3-card Hades draft already matches "Bone Up." No rebuild needed. The takeaway is *tuning intent*: ensure draft cadence is frequent/visible enough that a struggling player feels steady power growth (the video's whole point of "Bone Up"). Validate drop rate / draft frequency against this goal during playtest. Mostly confirms current direction.

### R4 — Make the signature verb multi-purpose (the burrow principle, abstracted) — **MED value / DESIGN effort**
Don't add burrowing. Do apply the burrow lesson to **Sundered Beat / dash**: ensure the core verb has movement *and* combat *and* (light) puzzle/traversal payoff, so it feels like Mina's burrow does — one button, many situations. E.g. dash through Void Rift hazards safely, BREAK destructible cover, reposition behind tracking enemies. Design-doc task, not a feature yet.

### R5 — (OPTIONAL, flagged risky) Single temporary "Rift Charge" active — **LOW value / MED effort / canon-tension**
If a one-shot disposable active is ever wanted, frame it as a lore-consistent **Rift Charge** (a single seal-energy burst, use-it-or-lose-it on death), NOT a side-arm weapon or inventory item. This skirts the no-item-grid lock by being a single charged ability, not stored gear. **Recommend deferring** — it adds inventory-adjacent complexity the canon deliberately rejected, and Curse Gates already cover "risk for power."

**Do NOT do:** granular difficulty sliders (canon + video both warn against), burrow mechanic, floor-looted secondary weapons / item grid (hard canon ban).

---

## 5. TOP PICK
**R1 — Wire the bone Death-Marker into a forgiving currency-recovery rule ("Echo Debt" reclaim, forfeited on a second death).**

It's the single idea where the video and RIMA's *own already-planned-but-unfinished* system click together perfectly: RIMA has the marker and the lore ("failed containers," "absolution") but no mechanic attached; the video supplies a proven, low-friction recovery rule (lose only on the second death). It costs little (data + one manager hook + the bone sprite already on the roadmap), introduces zero canon conflict, deepens the death/economy loop that NLM confirms is otherwise pure-loss, and turns a flavor-only marker into a real risk/reward decision. Everything else in the video is either already in RIMA (Echo mode, 3-card draft) or a deliberate non-fit (sliders, side-arms, burrow).
