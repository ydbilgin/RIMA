ACTIVE RULES: concise, ground claims, say UNSURE if unclear. RESPOND INLINE (not to a file). You are the REVIEWER.

# REVIEW — Opus-written ImpactFrameDriver.cs (writer != reviewer)

cx-yekta is rate-limited, so Opus wrote this. Review it for correctness. Read these files:
- `Assets/Scripts/Combat/Juice/ImpactFrameDriver.cs` (the new code)
- `Assets/Scripts/Combat/CombatEventBus.cs` (the event API it uses)
- For reference, the sibling driver pattern: `Assets/Scripts/Combat/Juice/CameraPunchController.cs` / `ScreenShakeDriver.cs`

It already compiles (dotnet build RIMA.Runtime, 0 errors). Focus on LOGIC/correctness, not syntax:
1. Does it correctly trigger ONLY on heavy hits (crit/finisher) + kills, not every hit? (HitEvent has `isCrit`, no `isFinisher`.)
2. Self-build + RuntimeInitializeOnLoad + DontDestroyOnLoad + dup-guard — any leak/double-instance risk across scene loads?
3. Unscaled-time flash during hitstop (timeScale 0) — will WaitForSecondsRealtime actually advance? Any risk it stays stuck visible?
4. Event subscribe/unsubscribe correctness (OnEnable/OnDisable) given CombatEventBus resets statics on domain reload.
5. Is the flash SUBTLE enough (alpha 0.18/0.12, ~0.06s total) — or epilepsy/over-flash risk on rapid crits? (debounce 0.05s.)
Give a short verdict: PASS / PASS-WITH-NITS / FAIL + the specific fixes if any. Keep under 250 words.
