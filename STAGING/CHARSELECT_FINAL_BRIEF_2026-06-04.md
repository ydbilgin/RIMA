# CHARACTER SELECT — FINAL POLISH + DEMO UNLOCK ECONOMY — BRIEF
Date: 2026-06-04 · Council (cx + ax-3.1 + ax-3.5) → Opus → implement → verify. User feel-tested, "çok yakın" (very close).

## USER DIRECTIVES (6)
1. **BUG — class-carry:** "oyuna girince ayrıca karakteri seçince her türlü warblade geliyor" → no matter which class you SELECT, the game always starts as Warblade. The selected class is NOT carried to gameplay. FIND root cause + FIX. (Likely the roster-room rebuild broke the SelectClass → PlayerClassManager.SelectedClass wiring that SEÇ/scene-load reads.)
2. **Bigger side info:** left identity popup + right skills panel content should be a bit BIGGER / more readable (currently too small after the "sensible sizes" pass).
3. **Reposition chars if needed:** if bigger panels encroach, adjust character positions so NO character is hidden behind anything (panels, columns). Keep all 10 fully visible.
4. **Locked = full BLACK SILHOUETTE:** locked classes shown as fully-black silhouettes (closed-like, only silhouette), NOT just dimmed. On unlock → revert to normal sprite. (ax-3.1's original "Obsidian Silhouette" — user now wants it.)
5. **Demo Echo balance:** give the player a starting Echo amount so that, when showing the professor, they CAN unlock a locked class (demonstrate the unlock flow). Enough to unlock at least one (cheapest locked = 120 Echo).
6. **Locked not selectable-to-play + unlock works:** clicking a locked char must NOT select it as the playable character (SEÇ stays blocked). Instead the unlock CTA ("KİLİDİ AÇ — {cost} Echo") must FUNCTION: if Echo ≥ cost → spend Echo → unlock that class → it becomes normal + selectable.

## CURRENT STATE (committed ed236d2b)
Roster room: 10 idle_south chars (front 4 unlocked y.34 / back 6 locked y.62), tight hitbox, left identity popup (x.012-.175), right vertical skills (x.86-.988), bottom SEÇ/GERİ, UI-VFX selection ring, backdrop room_bg. Locked = dimmed (~0.40) + lock + Echo cost; SEÇ→"KİLİDİ AÇ — {cost} Echo" disabled. IsUnlocked/UnlockCost/LockedButtonText exist. SelectClass updates UI.

## COUNCIL QUESTIONS
1. **class-carry bug (cx feasibility — ROOT CAUSE):** how does the selected class reach gameplay? Where is PlayerClassManager.SelectedClass set, and does the roster-room SelectClass/SEÇ-OnStartRun actually set it? Why does it always end up Warblade? Exact file/line + fix.
2. **Echo system (cx):** where is Echo currency stored (RunStats? PlayerPrefs? a meta-progress save)? Is there unlock PERSISTENCE (does unlocking a class save)? How do IsUnlocked/UnlockCost read it? What's the least-code path to (a) seed a demo Echo balance, (b) make KİLİDİ AÇ spend Echo + persist the unlock + flip the char to normal/selectable?
3. **Demo Echo amount + unlock flow (ax-3.1 design + ax-3.5 lean):** how much starting Echo for the demo (enough to unlock ≥1, not trivialize)? Unlock UX: click locked → CTA → spend → reveal animation? Keep it demo-appropriate, not a full meta-economy.
4. **Locked silhouette (design):** full-black silhouette recipe (tint to near-black, keep alpha shape) + reveal-to-normal on unlock. Confirm idle_south alpha gives a clean silhouette via color tint (black, alpha intact).
5. **Bigger panels + no-occlusion (design + feasibility):** how much bigger for left/right info (readable) while keeping chars un-occluded — new panel widths + char center band + sizes. Numbers.
6. **Locked not selectable:** clicking locked → show unlock CTA only (don't set it as the chosen playable; SEÇ stays for unlocked only). Confirm the cleanest gate.

## CONSTRAINTS
Reuse-first, procedural CharacterSelectScreen.cs, KEEP all working wiring. Output: bug fix + concrete numbers (Echo amount, panel sizes, char coords) + silhouette recipe + unlock-flow spec.
