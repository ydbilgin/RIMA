ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only SkillOfferUI.cs (+ a small helper if needed) (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory.

# Amaç (Purpose)
Add Hades/Balatro-style "juice" animations to the 3-card skill-draft selection screen (`Assets/Scripts/UI/SkillOfferUI.cs`). Cards are built procedurally in code (no prefabs): each card = a RectTransform with bg Image (`RimaUITheme.SmallPanelFrame`), Icon, name/desc TMP, a "SEC" Button. A slide-in stagger on show ALREADY exists (`SlideIn` coroutine). Theme: cyan (#00FFCC / `RimaUITheme.Cyan`) = seal energy. PURE coroutine/Lerp (NO DOTween). Must compile clean (check read_console). Do NOT touch DraftManager logic or the pick callback contract.

# Scope — implement these 4 touches (priority order), all in SkillOfferUI.cs

## 1. HOVER (pointer enter/exit + keyboard select/deselect) — highest priority
On each built card, add pointer + selection handling so hover works with BOTH mouse and keyboard/gamepad:
- Attach an `EventTrigger` (or a small `MonoBehaviour` implementing `IPointerEnterHandler,IPointerExitHandler,ISelectHandler,IDeselectHandler`) to the card. A nested private helper class inside SkillOfferUI is fine.
- On hover-ENTER: scale card `1.0 -> 1.08` over `0.12s` ease-out-cubic; set a per-card child `Canvas` (`overrideSorting=true, sortingOrder=10`) so the hovered card draws OVER siblings (add the Canvas+GraphicRaycaster once at build time, toggle overrideSorting / sortingOrder on hover); raise a cyan glow (see #2) to alpha 0.75 + glow scale 1.12.
- On hover-EXIT: reverse to scale 1.0, sortingOrder 0, glow back to idle pulse.
- Run each tween as a coroutine; cancel/replace the card's previous tween when a new hover event arrives (store a Coroutine handle per card). Use `Time.unscaledDeltaTime` (draft pauses timeScale).

## 2. CYAN GLOW behind each card (idle pulse + hover flare)
- At build time add a child Image BEHIND the card bg: a soft cyan rect (use `RimaUITheme.Cyan`, raycastTarget=false, slightly larger than the card e.g. sizeDelta = card + 24px, sorting behind bg). If no soft sprite is available, a plain tinted Image is acceptable.
- IDLE: gently pulse its alpha `0.15 <-> 0.35` via `Mathf.Sin(Time.unscaledTime * 1.2f + phase)`, phase = cardIndex.
- HOVER: lerp alpha up to `0.75` and glow localScale `0.95 -> 1.12`.
- Tint the glow by the card's tier color (reuse the existing `TierColor(offer)` for reward cards) so rarity reads through the glow.

## 3. SELECT confirm (when SEC clicked / card chosen)
Currently the SEC button onClick calls `onPick?.Invoke(captured, idx)` immediately. Wrap so a short confirm animation plays FIRST, then invokes the callback:
- Chosen card: anticipation squash to `0.94` over `0.06s` (ease-in-quad), then fly toward screen-center (anchoredPosition -> Vector2.zero of the cards container) while scaling to `1.25` over `0.30s` ease-out-back.
- Other cards: drop down (anchoredPosition.y -= 500) + fade alpha -> 0 over `0.22s` ease-in-cubic; set their CanvasGroup interactable/blocksRaycasts=false.
- Screen flash: a full-screen cyan Image alpha `0 -> 0.5 (0.04s) -> 0 (0.16s)`.
- After the confirm anim (~0.34s, unscaled), THEN call the original `onPick?.Invoke(captured, idx)`. Guard against double-clicks (disable all card buttons once one is chosen).

## 4. Keep it robust
- All time math on `Time.unscaledDeltaTime` / `Time.unscaledTime` (timeScale is 0 during the draft).
- `ClearCards()` must stop any running per-card tween coroutines (it already StopAllCoroutines — keep that working; the new coroutines are on `this`).
- Do not break `ShowReplaceMode` (apply the same hover treatment to its cards if cheap, else leave it; replace cards reuse `BuildSkillCard`).
- Easing helpers: add small static funcs `EaseOutCubic(t)=1-pow(1-t,3)`, `EaseOutBack`, `EaseInQuad`, `EaseInCubic`.

# Reference (research, exact values above came from it)
Gemini research summary is in `STAGING/ax_card_anim_research.md` (read it for the full value table + a C# template). Use those numbers.

# Verify
After editing, ensure project compiles (read_console, 0 errors). Do NOT enter play mode (orchestrator will play-test). Do NOT commit. Report: what changed in SkillOfferUI.cs (methods added/modified) + confirm compile-clean.

# Routing
profile laurethayday (fall back to yekta if quota-limited).
