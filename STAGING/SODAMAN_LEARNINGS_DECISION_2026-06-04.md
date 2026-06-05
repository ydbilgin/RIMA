# SODAMAN — Learnings & Skill-Selection UX Decision (2026-06-04)

**Method:** /council — cx (yekta, feasibility/reuse, `CODEX_DONE_yekta.md`) ‖ ax Gemini 3.1 Pro High (deep UX) ‖ ax Gemini 3.5 Flash High (lean) → Opus synthesis.
**Source advisor files:** `STAGING/_council_cx_sodaman.md`, `_council_q_31pro_sodaman.md`, `_council_q_35flash_sodaman.md`. cx answer = `CODEX_DONE_yekta.md`.
**Trigger:** User asked to (a) examine Sodaman, (b) think about "hover-like things when selecting skills," (c) council what RIMA can borrow.
**Scope:** ANALYSIS / DECISION ONLY this session. No code. Implementation = next session (handoff).

---

## 0. What Sodaman is (ground-truth, Steam + reviews)
Cyberpunk **bullet-heaven roguelite**, top-down pixel 2D. Dev Tape Corps (4), ~78% positive / 630 reviews, ~4-8h. Signature ideas:
- **Draft from 3 SODA CANS** (a thematic *vessel*) instead of the genre-standard "3 cards."
- **7 soda colors × 10+ skills each**; mixing colors = "cocktail" **synergy** effects (build-crafting via color readability).
- Meta: 40+ enhancement cards (deck), 6 body-parts × 30+ cybernetic augments, 8 weapons w/ synergy discovery, between-run spaceship **hub**.

---

## 1. Sodaman → RIMA: TAKE / SKIP (3-advisor consensus)

| Sodaman idea | Verdict | Rationale (synthesis) |
|---|---|---|
| **Color-coded skill families = instant readability** | ✅ **TAKE** | Map Sodaman's 7-color legibility onto RIMA's **10 class accent colors** (already in `RimaUITheme.cs:78-91`). Player should read *family/synergy* from color before reading text. Lock a hex palette + glow for all 10 classes; all UI "speaks" those colors. |
| **Synergy signalling ("cocktail")** | ✅ **TAKE (lean form)** | NOT Sodaman's color-mixing formulas (3.5 Flash: "drowns 10-class identity"). RIMA already has tag/chain synergy (`ChainWindowTracker`, `DraftManager.OwnedActiveSkillNames`, static ChainChip). Surface it on **hover** instead of inventing a new system. |
| **3-vessel draft theming** | ✅ **TAKE (re-skin only)** | Keep RIMA's 3-card draft; re-theme cards as diegetic **"Rift Seals / Echo crystals"** (3.1 Pro) — broken organic geometry, not boxes. Canon-fit ("opak kutu yasak"). Pure art/skin, mechanic unchanged. |
| **Deck-building meta (40+ map cards)** | ⏸️ **DEFER** | Effort-monster; core loop must settle first (3.5 Flash). Revisit post-demo. |
| **Cybernetic augments / 6 body-parts** | ❌ **SKIP** | Cyberpunk framing clashes with RIMA Rift/Echo lore (3.1 Pro). If a slot-system is ever wanted → "Echo Tethers/Soul Nodes," not body-parts. |
| **8 weapons w/ synergy** | ⏸️ **DEFER** | RIMA's class-skill pool already provides build breadth; weapon layer is separate scope. |
| **Between-run hub** | 🔵 **ALREADY HAVE** | CharacterSelect "roster room" is RIMA's hub equivalent; extend it later, don't build new. |

---

## 2. SKILL-SELECTION HOVER & FEEDBACK (the user's core ask)

**Key reuse finding (cx):** RIMA's hover machinery is ~80% built. `SkillOfferUI` already has fixed raycast hitbox + child `VisualRoot` hover scale/lift (`:391-402`, `:752-825`), rarity glow (`:403-418`), select flash (`:445-456`, `:877-890`), select SFX. **The gap is that `TooltipSystem` (`TooltipSystem.cs:84-218`, fully written: name/tier/description/CD) is NOT wired to the cards** — `rg` found zero callers. That's the single highest value/effort win.

**Decision — 3 tiers, cheapest first:**

- **Tier 1 (MVP, ~1 cx task) — WIRE WHAT EXISTS:**
  1. **Hover tooltip:** add a `SkillData`/`RewardOffer` ref to `CardJuiceState`; on pointer-enter call `TooltipSystem.Instance.Show(...)`, on exit `Hide()`. (cx: `SkillOfferUI.CardJuiceHandler` is the insertion point.) Draw it canon-style (3.1 Pro): **no opaque box** — radial ink-wash gradient + cyan hairlines top/bottom, not a panel.
  2. **Equipped-synergy highlight:** on hover, reuse `ChainWindowTracker.ChainsWith` + `DraftManager.OwnedActiveSkillNames` to **pulse/glow** the matching equipped slot(s) on the SkillBar. (cx-backed; cheaper than a drawn line.)
  3. **Hover SFX:** low-volume crystal/glass tick on enter (reuse `Sfx.Cast` or add `DraftHover` enum). Needs a real clip to be audible.
- **Tier 2 (juice polish):** **"Rift-Arc tether"** (3.1 Pro) — draw a glowing cyan energy line from the hovered card to the synergistic equipped skill. Higher effort; do after the left panel exists so there's an anchor to draw to.
- **Tier 3 (skin):** re-theme draft cards as Rift Seals (organic geometry), `OutBack`/`OutElastic` ease on hover-lift, desaturate non-hovered to ~30%.

**Over-engineering guard (3.5 Flash):** do NOT write a screen-space Tooltip Manager with edge-clamping. A fixed-position tooltip + `CanvasGroup.alpha` fade is enough for MVP.

---

## 3. RUN-IN LEFT SKILL PANEL (toggle) — user request #1

**What it is:** a key-toggled left slide-out showing this run's equipped skills (in-game HUD, real-time, glance-able).

**Design (synthesis):** Partial **peek** covering left ~20-25% (3.1 Pro). Diegetic: a vertical "Rift tear"/ink line with skill icons hung on it like nodes-on-a-string; **icons + accent color + radial cooldown only, no text** (text lives in the ESC codex). Actives (Q/E/R/F) top, passives below. Ready skills breathe cyan.

**Implementation (cx):** new small `SkillPanelToggleUI` MonoBehaviour under HUD canvas (do NOT bloat `HUDController`). Reuse: `SkillBarUI`'s controller-resolution pattern (`SkillBarUI.cs:312-383`), `CharacterSelectScreen.BuildSkillRow` visual, `SkillDatabase.FindByName()` for any text, `RimaUITheme` frames/icons/accents. Equipped data lives on the **class controllers** (`*_SkillController.GetAllSlots/SlotCount`), not just SkillBarUI.

**⚠️ OPEN DECISION — toggle key (BLOCKED, cx):** `KeyBindManager` only reserves Tab/Esc and has no panel action. **TAB is already taken** by `CharacterSheetUI` (the existing build-overview at timeScale 0.1 via `UIManager`). So either:
- **(A)** repurpose/upgrade the existing TAB `CharacterSheetUI` INTO this diegetic left panel (one overview, no new key), **or**
- **(B)** add a new `GameAction` + key (suggest **hold `Alt`** for peek, or toggle **`C`**).
→ Needs user pick. *Recommendation: (A)* — there's already a TAB build overview; turning it into the nicer left panel avoids key sprawl and duplicate "what's my build" surfaces.

---

## 4. ESC CODEX SKILL SCREEN — user request #2

**What it is:** pause + browse ALL class-specific skills for theorycraft/build planning.

**Layout decision (user asked full-screen vs side-panel):** **Full-screen overlay** (3.1 Pro) — a codex is where the player exhales and plans; a side panel cramps it. Canon-fit: pause, **heavy-blur + desaturate the frozen game behind** (not a flat menu) → "looking into the Echoes," constellation/node layout radiating from the character, locked skills = hollow ink outlines, unlocked = filled + class-color glow. **Constellation is the Phase-2 visual; Phase-1 ships a grid/list.**

**Implementation (cx):** new `SkillCodexUI` overlay; **do NOT hijack `SettingsMenuUI`** (cx overrides 3.5 Flash here). Route via `UIManager` as another pause layer (it already owns `Time.timeScale=0`, `UIManager.cs:225-233`) — codex must NOT write timeScale itself. Reuse `CharacterSelectScreen` skill-row rendering (`:830-880`) + `SkillDatabase.GetAll()` + the Tier-1 tooltip on hover.

**⚠️ BLOCKERS (cx):**
1. **Data coverage:** `SkillDatabase` only registers **Warblade, Elementalist, Shadowblade, Ranger** (+ neutral passives). Ravager/Gunslinger/Brawler/Summoner/Hexer/Ronin are NOT enumerable from one source (Ronin skills exist under `Combat/Classes/Ronin/Skills/*` but aren't registered). **Full 10-class codex is blocked until all classes register through one data source.** → MVP codex = implemented classes only, expand as classes get registered.
2. **Lock granularity:** only **class-level** unlock exists (CharacterSelect PlayerPrefs). No per-skill unlock state in code. → Codex shows class-locked vs unlocked; per-skill locking is a future design choice, not free.

---

## 5. CURRENCY NAME "Echo"

**Synthesis:** Aesthetically "Echo" fits RIMA's Rift/Echo lore well (3.1 Pro, 3.5 Flash both: keep). **The real issue is a naming COLLISION, not taste (cx):** "Echo" is used for BOTH (a) the **meta unlock currency** (`CharacterSelectScreen.cs:37-39` `DemoStartingEcho`/`EchoBalancePrefsKey`, lock/cost strings) AND (b) a **gameplay mechanic** (cross-class "shadow Echo" in `SkillOfferUI`/`RewardOffer`/cross-class files). Run-money is actually **Gold/Altın** via `PlayerEconomy` — separate from Echo.

**Decision:** Per HARD RULE, currency name stays an **open decision for the user + NLM canon** — do NOT bulk-rename. If a rename happens it should target **only the meta unlock currency** (cheap: centralize in `CharacterSelectScreen` or a tiny `MetaCurrency` helper) to remove the collision — e.g. meta = **"Vestige"** (3.1 Pro alt, fits antique-seal aesthetic) while keeping the "Echo" *mechanic*. Global `Echo` rename is UNSAFE.

---

## 6. PRIORITIZED HANDOFF BACKLOG (next session)

| # | Task | Effort | Reuse anchor | Gate |
|---|---|---|---|---|
| 1 | **Tier-1 hover** = wire `TooltipSystem` into draft cards + equipped-synergy pulse + hover SFX | S (1 cx) | `SkillOfferUI.CardJuiceHandler`, `TooltipSystem`, `ChainWindowTracker`, `DraftManager.OwnedActiveSkillNames` | hover SFX needs a clip |
| 2 | **Left skill panel MVP** (`SkillPanelToggleUI`, icons+CD only) | M | `SkillBarUI` resolution, `*_SkillController.GetAllSlots`, `RimaUITheme` | **toggle-key decision (§3 A vs B)** |
| 3 | **ESC `SkillCodexUI` MVP** (full-screen, blur-bg, grid of implemented classes, hover tooltip) | M | `UIManager` pause layer, `CharacterSelectScreen` rows, `SkillDatabase.GetAll()` | implemented classes only (§4 blocker 1) |
| 4 | Tier-2 Rift-Arc tether + Tier-3 card re-skin (Rift Seals) | M | after #2 anchor exists | art pass |
| 5 | Register remaining 6 classes in `SkillDatabase` (unblocks full codex) | M | `SkillDatabase.Add` | enables #3 full |

**Open decisions for user:** (1) left-panel key — upgrade existing TAB sheet (A) or new key (B)?; (2) currency name / Echo-collision (NLM canon).

**Routing:** implementation → cx (yekta reliable this session); council = cx ‖ ax-3.1 ‖ ax-3.5 → Opus. PixelLab/animation = GATED.
