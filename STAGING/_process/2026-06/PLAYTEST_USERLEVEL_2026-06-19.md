# Playtest Evidence Log — 2026-06-19 (User-Level Drive)
Driver: Sonnet sub-agent (read-only Unity driver). No code modified.

---

## SCENARIO 0 — FREEZE REPRODUCTION & CHARACTERIZATION

### Boot flow (Warblade + Card_0)
| Step | timeScale | DraftActive | skillOfferOpen | Notes |
|------|-----------|-------------|----------------|-------|
| Button_Basla click | 1 | — | — | main menu |
| Warblade Hit + StartButton | 1 | — | — | char select |
| Pre-pick (draft open) | 0 | True | True | expected pause |
| Card_0 Btn.onClick.Invoke() | 0 | — | — | same frame as invoke |
| Round-trip 1 (next execute_code) | **1** | False | False | draft closed |
| Round-trip 2 | **0** | False | False | ← FREEZE re-appears |
| Round-trip 3 | **0** | False | False | all UIManager flags = False |

**Finding**: timeScale momentarily returns to 1 (draft closed correctly), then something re-sets it to 0 within ~1 frame. All UIManager tracked flags are False at freeze time — the owner of the 0 is NOT any of the 5 known UIManager flags.

### 0b — Self-heal test
| Action | timeScale |
|--------|-----------|
| ClosePause() alone | 0 (no change) |
| OpenPause() | 0 |
| ClosePause() after OpenPause | **1** |

**Result**: YES, self-heals. Opening then closing the pause menu fully restores timeScale to 1. Freeze is **recoverable**, not a hard-lock. UIManager.ApplyTimeScale is called on ClosePause and re-evaluates all flags → clears the stuck 0.

### 0c — Card_1 comparison (Warblade)
| Round-trip | timeScale |
|------------|-----------|
| Card_1 click (same frame) | 0 |
| +1 | **1** |
| +2 | **1** |
| +3 | **1** |

**Result**: Card_1 does NOT freeze. timeScale stays at 1 across all 3 subsequent round-trips. Freeze is **Card_0-specific**.

### 0d — Elementalist + Card_0
| Round-trip | timeScale |
|------------|-----------|
| Card_0 click | 0 |
| +1 | **1** |
| +2 | **0** ← freeze |

**Result**: Freeze reproduces with Elementalist + Card_0 too. **NOT class-specific** — Card_0 index is the common factor.

### Console errors during Scenario 0
**0 errors, 0 warnings.**

---

## SCENARIO 1 — T9 RESTART FIX

Self-healed Elementalist freeze first, then invoked `DeathScreenManager.RestartRun()`.

| Step | timeScale | DraftActive | skillOfferOpen |
|------|-----------|-------------|----------------|
| RestartRun() called | 1 | — | — |
| Next round-trip | **0** | **True** | **True** |

Opening draft re-appeared after restart. Picked Card_1 (not Card_0 to isolate T9 from freeze):

| Step | timeScale | DraftActive |
|------|-----------|-------------|
| Card_1 click | 0 | — |
| +1 | **1** | False |
| +2 (stable) | **1** | False |

**Result: T9 PASS** — restart triggers a fresh opening draft, draft closes cleanly, timeScale resumes to 1 and stays there (when using non-Card_0 cards).

Screenshot: `Assets/Screenshots/Playtest_2026-06-19/scenario1_t9_restart_active_run.png`

### Console errors during Scenario 1
**0 errors, 0 warnings.**

---

## SCENARIO 2 — T7 REWARD DRAFT + PAUSE

After T9 run was active (timeScale=1, Elementalist with Warblade fallback skillController):

1. Found 2 enemies: HalfThrall(Clone) maxHP=30, FractureImp(Clone) maxHP=60
2. Note: a pre-existing timeScale=0 freeze was present from Scenario 0d (Card_0 Elementalist); had to self-heal first.
3. Killed enemies via `Health.TakeDamage(9999)` — real damage path (not ForceCollect).
4. A 3rd enemy (HalfThrall) spawned mid-process (wave continues while healed/unhealed).
5. RRD.LifecycleState = **Cleared**, EncounterController.encounterActive = **False** after all kills.
6. RewardPickup spawned at pos (1.92, 6.82) — player must walk to it (no auto-draft).
7. Called `RewardPickup.Collect()` to simulate player collecting chest.

| Step | timeScale | DraftActive | skillOfferOpen |
|------|-----------|-------------|----------------|
| Pre-Collect | 1 | False | False |
| Collect() | 0 | True | True |
| Pick Card_1 | 0 | — | — |
| +1 (immediate) | **1** | **False** | — |

RRD.LifecycleState after pick = **DoorOpen**, activeDoors = **2**.

**Result: T7 PASS** — reward draft appears on chest collect (not auto on room-clear), timeScale=0 correctly during draft, timeScale=1 + doors open after card pick.

Screenshot: `Assets/Screenshots/Playtest_2026-06-19/scenario2_t7_doors_open_post_reward.png`

### Console errors during Scenario 2
**0 errors, 0 warnings.**

---

## SCREENSHOTS
- `Assets/Screenshots/Playtest_2026-06-19/scenario0_run_active_card0.png` — active run after self-heal (world view)
- `Assets/Screenshots/Playtest_2026-06-19/scenario1_t9_restart_active_run.png` — active run after T9 restart
- `Assets/Screenshots/Playtest_2026-06-19/scenario2_t7_doors_open_post_reward.png` — doors open post-reward pick

---

## FREEZE ROOT CAUSE HYPOTHESIS (evidence only, no fix)
- Card_0 is the opening kit's first card. Something in the Card_0 skill's initialization path sets timeScale to 0 (or re-triggers a draft-open flag) on a **deferred frame** after the draft closes.
- All 5 UIManager flags remain False — the stale-0 owner is NOT UIManager's normal pause/offer/codex/tab/settings flags. Either (a) a coroutine sets timeScale directly, or (b) a parallel system (HitStop/HitPauseDriver candidate from prior session) reacts to Card_0's skill activation.
- The freeze is card-index-specific (Card_0 = position 0 = first skill in offer list), not card-type-specific (reproduced across Warblade + Elementalist).
- Self-heal via OpenPause→ClosePause works because UIManager.ApplyTimeScale re-evaluates all flags and clears the stale 0.

---

## FINAL EDITOR STATE
- Play mode: **STOPPED**
- Time.timeScale: **1** (edit-mode default)
- Console: **0 errors, 0 warnings**
- No game code or assets modified.

---

## POST-FIX VERIFICATION — HitPauseDriver.cs:124 guard (2026-06-19 session 2)

Fix applied: `previousTimeScale = Time.timeScale > 0.0001f ? Time.timeScale : 1f;`

TEST 1 Warblade Card_0: PASS — RT1=1/False, RT2=1/False. RT3=0/False = DeathScreenManager (player died ~21s), NOT a Card_0 hang. Final timeScale settled at 1.
TEST 1 Elementalist Card_0: PASS — RT1=1/False, RT2=1/False. RT3=0/False = same death screen pattern. Final timeScale settled at 1.
TEST 2 Warblade Card_1 (control): PASS — RT1=1/False, RT2=1/False, stable 1. No regression.
TEST 3 Unpause-during-overlay: HitPauseDriver NOT present in _Arena scene (0 instances). TriggerPause could not be invoked live. SIMULATION: with timeScale=0 during draft, fix gives previousTimeScale=1f — a HPD restore WOULD set TS=1 while DraftActive=True (theoretical regression IF HPD added to scene). Not reproducible today. Verdict: no confirmed regression, but latent risk if HPD enters _Arena.
TEST 4 T9 restart: RestartRun() via reflection -> DraftActive=True timeScale=0 -> PickCard(1) -> timeScale=1. PASS. T4b reward draft: SKIPPED (enemies alive, clean kill path not safe to execute headlessly).
CONSOLE: 0 errors, 0 warnings across all sessions.
SCREENSHOT: Assets/Screenshots/Playtest_2026-06-19/test1_warblade_card0_post_pick.png
