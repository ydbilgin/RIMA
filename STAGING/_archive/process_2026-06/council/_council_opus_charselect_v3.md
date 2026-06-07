# COUNCIL — Opus Advisor Position — CharacterSelect v3 + Skill-Echo-Unlock (2026-06-05)

> Lens: game-design judgment + visual evaluation (I can see the concept image; the other advisors cannot).
> This is my INDEPENDENT position. Synthesis is someone else's job. I did not adopt ChatGPT's brief.

---

## 1. K1 DECISION — Should skills also be Echo-unlocked? → **NO. Keep canon. The "400 Echo Mastery" panel is a design trap. Replace it.**

### Precedent table (game → mechanic → lesson for RIMA)

| Game | What unlocks & with what | Interaction with in-run draft/pool | Player reception | Lesson for RIMA |
|---|---|---|---|---|
| **Dead Cells** | Blueprints (weapons/skills) unlocked with Cells → enter the run-time drop **pool** | Each unlock **dilutes** the pool: good items appear less often as you unlock more junk | **Documented frustration.** Players ask to *lock items OUT* of the pool; devs had to add Custom Mode item-locking + "Merchandise" RNG-control upgrade as a band-aid | This is EXACTLY the "unlock = add to draft pool" model the brief asks about. It is a **known anti-pattern.** Adding skills to RIMA's 3-card Rift-Seal pool via Echo would dilute the draft and punish progression. **Avoid.** |
| **Hades** | Mirror of Night (permanent stat/ability talents, Darkness) + Aspects (weapon-reshaping, Titan Blood) — two *separate* currencies | Mirror = always-on passive power, **never touches** the boon draft. Aspects change a weapon, also outside the boon RNG | **Gold standard.** Praised precisely because permanent power is *separate* from the run's randomized choices; free respec keeps it low-commitment | If RIMA ever does permanent power, it must be a **separate axis that does NOT enter the draft** — never "buy a card into your deck." |
| **Death Must Die** | Class "signs" (abilities) unlocked by **achievements**, not currency. No gold-for-stats | Unlocks are gated by *doing things*, currency is gear-only | Well received; the achievement-gate makes unlocks feel earned, not bought | **Direct support for RIMA canon's OR-path:** Shadowblade = 150 Echo OR Act1×3. Achievement-unlock reads as mastery; currency-unlock reads as a paywall. |
| **Halls of Torment** | Stat shrine + gear unlocks via currency | Meta-stats are flat power, gear is a separate slot system; **not injected into ability draft** | Some early-game "can't progress without grinding meta" complaints, but the *separation* is not the issue | Meta power that sits *outside* the build-draft is fine. Meta power that *is* the build-draft creates grind-walls. |
| **Rogue Legacy 2** | Manor: classes + flat stats via Gold; every buy raises all costs | Manor is permanent passive power; the *run's* spells/relics are still found in-run | Loved by completionists; cost-escalation paces it. But it's a **flat-power** model, not a pool-injection model | The escalating-cost class unlock maps cleanly to RIMA's **character** unlock (80→250). Do NOT extend that same store to per-skill mastery. |
| **Vampire Survivors** | PowerUps (flat meta-stats, Gold) + character unlocks (achievement/find) | Weapons evolve in-run; meta is pure flat stat floor | Praised for low-stakes; meta never gates the *fun choices* of a run | Meta = stat floor, not choice-content. Reinforces: keep Echo for **access** (which class), not for **build content** (which skill). |

### The core distinction the brief asked for
- **"Unlock = add skill to draft pool" (Dead Cells model)** → **REJECT for RIMA.** RIMA's identity is a tight, readable 3-card Rift-Seal draft (StS-style, build resets on death). Every Echo-bought skill you add makes that draft noisier and the next good draft rarer. You would be *selling the player pool dilution.* This directly damages RIMA's strongest, most canon-protected loop.
- **"Unlock = permanent passive power" (Hades Mirror model)** → only acceptable if it lives on a **separate axis** that never enters the draft. RIMA canon explicitly says **no permanent skill tree**, build is 100% run-draft. So even this is a canon AMEND, and I do not recommend it now.

### My NET recommendation
**Keep NLM canon. Skills are NOT bought with Echo. The 12 common skills stay open from the start; build is entirely the in-run 3-card draft.**

Re-skin the "USTALIK YETENEKLERİ — 400 ECHO" panel on the concept into one of these (preference order):

1. **(PREFERRED) "Signature / Mastery skills = achievement-unlocked, currency-free."** Each class has 1–3 mastery skills shown locked, with the unlock condition being a *deed* ("Clear Act 1 with Warblade", "Land 50 Sundered Beats"), NOT an Echo price. This (a) honors canon's existing OR-paths, (b) matches Death Must Die's well-received model, (c) keeps the screen free of any shop-feel — which the user has BANNED everywhere else. When unlocked, the mastery skill simply **joins that class's draftable common pool** (still drafted in-run, still resets on death) — so it deepens identity without buying permanent power.
2. **(ALT) Drop the mastery panel entirely for now.** Right panel shows only the 3 starting actives + a short "More skills appear as Rift-Seal drafts during runs" line. Lowest risk; ships the visual without committing to an unbuilt system (SkillDatabase only has 4 of 10 classes; per-skill lock code does NOT exist — building Echo-per-skill is real engineering cost for negative design value).

**Do NOT** make mastery skills Echo-priced. It contradicts canon, reproduces a known anti-pattern (Dead Cells), and re-introduces the exact "store feel" directive #6 forbids — just on the skill panel instead of the character grid.

### Economy sketch (consistency with the 80–250 character canon)
- Keep **character** unlocks as the only Echo sink: canon 80 / 150 / 200 / 250, each with an OR achievement path (canon already specifies these — the concept's 120/160/200 should be corrected to canon, and the OR-path should be surfaced in UI: "150 Echo **veya** Act1'i 3 kez bitir").
- **Echo per run:** target so the *first* alt class (80) lands at roughly run 3–4, and the full roster is a long-tail goal (~25–40 runs), matching Hades/RL2 pacing where the roster is a multi-session arc, not a single-evening dump.
  - Suggested: **~20–30 Echo per cleared run**, scaled by depth/Act reached (e.g. Act1 clear ≈ 15, Act2 ≈ 25, Act3 ≈ 40), with a small first-time-per-Act bonus. No purchase, no doubling, no "watch ad / buy" — directive #6.
- **No second currency for skills.** A single sink (character access) keeps the mental model clean and avoids the "which currency does what" confusion that the Echo-name collision already threatens (see K2).

---

## 2. K2 — Currency name → **Keep the canon lore word "Echoes", but in UI always render the META-currency as its full proper-noun form "Shattered Echo / Shattered Echoes", and reserve bare "Echo" for the gameplay mechanics.**

Reasoning:
- Canon name is **"Shattered Echoes"** and the lore (class "faces" scattered in the Fracturing, recollected to become whole) is *thematically perfect* for a character-unlock currency — you are literally re-collecting shattered class identities to play them. Throwing that away for a generic word like "Vestige" loses a strong, earned lore hook. **Do not full-rename.**
- The real problem is the **bare token "Echo"** colliding with Echo Mode, Echo Imprints, Shadow Echo, Echo Twin, the mob names, etc. The fix is *disambiguation by full form*, not a new word:
  - **Meta-currency, ALWAYS full form in UI:** top bar = **"80 Ⓢ Shattered Echo"** (or the ◆ shard glyph), unlock copy = "Kilidi aç — 150 Shattered Echo". Never abbreviate it to "Echo" on this screen.
  - Bare **"Echo"** stays free for the gameplay-flavor systems (Echo Imprints draft, Shadow Echo, etc.), which live in other screens — so the two never appear side-by-side in the same context.
- If the user still finds "Shattered Echo" too long for the top-bar number, the acceptable compromise is a **shard glyph + number** ("◈ 80") with the word "Shattered Echo" appearing only on hover/unlock copy. This keeps lore, kills the collision, and keeps the HUD terse.
- **Verdict:** No rename. Disambiguate via mandatory full form + glyph. "Vestige" only as a last resort if playtesters still confuse the two after the full-form fix — and even then I'd prefer renaming the *easy-mode* "Echo Mode", not the currency.

---

## 3. K3 — Visual improvements (directive-compliant, concrete)

Honoring the SABİT directives (single screen, no scroll, no bob/parallax, clean dark void under island, characters tile-seated, equal size, **opaque black** locked silhouettes, no shop feel, click-on-character to select, no portrait strip/pedestal).

**Layout — keep 2 rows of 5+5, but fix the row geometry.**
- The concept's two rows read as front-big / back-small-and-slightly-floating. **Both rows must sit on the SAME iso tile plane** with correct depth-sort (Camera custom-axis, the project's locked sort), so back-row feet are *on tiles further back*, not smaller sprites hovering. Equal sprite size (directive #4) + true iso depth = natural "further away" without scaling tricks.
- Snap every character pivot to a **tile center** (one character per diamond tile), with 1–2 empty tiles of breathing room between columns. This directly answers directive #3 (feet on tiles, not floating) and ChatGPT's "grid hissi ama tablo gibi olmasın" — the iso diamond grid gives structure without looking like a spreadsheet.
- A subtle **alternating-tile-offset / brick stagger** between the two rows (back row shifted half a tile) breaks the rigid 5×2 table and reads more like a room than a roster grid. Keep it gentle.

**Locked characters — the concept VIOLATES the directive; fix it.**
- In the concept, locked characters (Elementalist, Ranger, etc.) are rendered as **full colored sprites with a small lock icon.** Directive #5 (user OVERRIDE) demands **opaque pure-black silhouettes.** This is the single biggest correction: locked = #000 fill of the sprite shape, only a small cyan lock glyph + cost/condition floating near the feet. The existing Unity build already does black silhouette (`567b8c75`) — the *mockup* must match the game, not the other way around.
- Put the **cost as the canon OR-form** under locked sprites: "150 ◈ / Act1 ×3" — small, low-contrast, no button-y chrome. Avoids store feel.

**Top bar.**
- Keep it thin and dark (ink-on-paper, no opaque slab — canon "UI yoktur, sadece bilgi vardır"). Left: "RIMA — KARAKTER SEÇ". Right: **◈ 80 Shattered Echo** (K2). That's all. No tabs, no shop button.

**Left panel (identity).**
- The 5 cyan stat bars are good and readable. Tighten: cap motto to one line, description to two. Keep the RAGE/resource block — it's the highest-identity element and should be the visual anchor of the panel. Frame = thin stone/metal 9-slice, NOT a flat opaque box (matches the right panel's framed look, stays ink-on-paper).

**Right panel (skills).**
- Replace "USTALIK YETENEKLERİ — ??? 400 ECHO" (K1) with EITHER the achievement-condition mastery rows ("Açılış: Act1'i Warblade ile bitir") OR remove the block and add one line: "Daha fazla yetenek run sırasında Rift-Seal draft'ında belirir." Either kills the shop-feel on this panel.
- Keep the 3 active-skill icon+blurb rows; they read well in the concept.

**Island / brazier / composition.**
- Two braziers framing the top columns = good, atmospheric, keep. They must be **static** (directive #1: no bob — flame *flicker* via shader/particle is fine, no vertical position movement of any element).
- Underside of the island: the concept's tapered dark base is acceptable but ensure **pure dark void, no decor/preview islands below** (directive #2). Fade the island base into void; no second floating object beneath.
- Cyan tile cracks: keep sparse (the concept is close). Don't over-glow or it competes with the selection ring.

**Selection feedback.**
- Selected = cyan **foot-ring + soft aura only** (directives #4/#7). The concept's Warblade ring is right. No scale-up, no pull-to-center, no extra particles. Hover = a fainter ring + cursor change so hover and selected are distinguishable (ChatGPT's "hover ve selected net hissedilmeli" — valid, keep).

**Bottom.**
- One centered primary button, label state-driven: **SEÇ / KİLİDİ AÇ (150 ◈) / YETERSİZ ◈**. Optional small "GERİ" bottom-left. Keep the bottom sparse (directive #8, ChatGPT's "alt fazla dolu olmasın" — valid).

---

## 4. Visual critique of the concept image (what works / what doesn't)

**Works:**
1. **Overall mood is on-brand** — void-purple sky, cyan-cracked iso island, twin warm braziers, framed stone panels. This already reads as "inside the game," not a web mockup. The strongest part.
2. **Left identity panel + 5 cyan stat bars** are clear, premium, and readable; the RAGE block sells class identity well.
3. **Selected-Warblade foot-ring + name plate** is the correct selection language (glow, not scale) — matches directives exactly.
4. **Top bar restraint** (title left / currency right, thin and dark) fits the ink-on-paper canon.

**Doesn't work / must fix:**
1. **Locked characters are full-color sprites, not opaque black silhouettes** — directly violates SABİT directive #5. Biggest correction.
2. **"USTALIK YETENEKLERİ — 400 ECHO" panel** reintroduces a shop-feel (directive #6 bans store vibes; canon bans Echo-bought skills). Must be re-skinned to achievement-gate or removed (K1).
3. **Two-row depth reads as front-big / back-floating** rather than one iso plane at two depths — risks the "characters floating" failure (directive #3) and the equal-size rule (#4). Needs true iso seating + depth-sort.
4. **Concept costs (120/160/200) diverge from canon (80/150/200/250) and show no OR achievement path.** Correct numbers + surface the dual-path.
5. **Tile↔feet alignment is approximate** — characters need to snap to tile centers so each clearly "owns" a diamond, reinforcing tile-seating and making click-targets unambiguous.

---

## 5. ChatGPT brief — ADOPT / SKIP

**ADOPT (with reason):**
- 10 chars one scene, click-on-character select, no portrait strip / pedestal / hero-showcase — matches user directives; correct.
- Selected = glow/ring only, no scale/centering — correct, canon-aligned.
- Echo not purchasable, only earned, no store button, show-current-amount only — matches directive #6 and my economy stance.
- Left identity panel + right skills panel + thin top bar + single bottom action button — clean IA, keep.
- "Hover ve selected net hissedilmeli" (distinct hover vs selected states) — valid, I added it to K3.
- "Grid hissi ama tablo gibi olmasın" — valid; my iso-diamond + half-tile stagger answers it.
- Data-driven (each char a data object, selected/locked/unlocked state) — correct engineering shape.

**SKIP / OVERRIDE (with reason):**
- ChatGPT "locked = visible-but-dim silhouette, not a black blob" — **OVERRIDDEN by user directive #5: opaque pure black.** Hard skip.
- ChatGPT's premium-feel framing risks tipping into opaque slabs — **constrain to ink-on-paper / thin 9-slice frames** per canon ("UI yoktur, sadece bilgi vardır"). Don't add solid panels.
- ChatGPT is silent on the mastery/Echo-skill question — **I fill the gap with K1: no Echo-bought skills; achievement-gate or remove.** Do not let the concept's 400-Echo panel pass by default.
- ChatGPT's loose "5+5 veya benzeri" — **commit to 5+5 on one iso plane with proper depth-sort + tile-center snapping**, not an underspecified balance.
- ChatGPT's "braziers/effects" — **must be static position** (no bob, directive #1); flicker only.

---

## TL;DR (my position)
- **K1: NO** Echo-bought skills. The "unlock→draft pool" model is Dead Cells' documented anti-pattern; the "unlock→permanent power" model contradicts canon's no-skill-tree rule. Keep 12 skills open + run-draft. Re-skin the "400 Echo Mastery" panel to **achievement-gated mastery** (Death Must Die model) or drop it. Echo stays a single sink: **character access only**, ~20–40/run, canon 80–250 prices with OR-paths.
- **K2:** Keep canon lore — **no rename.** UI always renders the meta-currency in full form / shard glyph ("◈ Shattered Echo"); bare "Echo" reserved for gameplay mechanics. "Vestige" only as last-resort.
- **K3:** Fix the concept to directive: **opaque-black locked silhouettes**, both rows on **one iso plane with depth-sort + tile-center snapping**, replace the 400-Echo skill panel, canon prices + OR-paths, static braziers, thin ink-on-paper frames, single state-driven bottom button.
- **Concept verdict:** great mood + correct selection language; three real violations (color-not-black locks, shop-feel skill panel, floating back row) and wrong unlock numbers.
- **ChatGPT:** good IA scaffolding to adopt, but override its "dim-not-black" silhouettes and its silence on the skill-unlock question.
