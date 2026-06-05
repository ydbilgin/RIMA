# Code Review: T4 Hover Tooltip & Synergy Pulse (Commit: `69ebdd2a`)

**Verdict:** `PASS-WITH-NOTES`

---

## 1. Hover → TooltipSystem Show/Hide Connection
*Evaluating potential memory leaks, orphan GameObjects, and timeScale=0 compatibility.*

### Delay & timeScale Compatibility
* **unscaled time kullanım durumu:** **PASS**. 
  * `TooltipSystem.cs:126` has been updated to use `yield return new WaitForSecondsRealtime(showDelay);`. This correctly guarantees that the tooltip delay works and completes even when `Time.timeScale` is `0` (e.g., when the draft or pause screens are active).
  * Hover tweens (`SkillOfferUI.cs:819`) and idle glows (`SkillOfferUI.cs:724`) in `SkillOfferUI` also correctly use `Time.unscaledDeltaTime` and `Time.unscaledTime` respectively, ensuring no visual freezes when time is paused.

### GameObject Leak & Orphan Risks
* **Card Destroy / Reset Clean-up:** **PASS-WITH-NOTES**.
  * When a selection is reset or confirmed, `SkillOfferUI.cs` calls `TooltipSystem.Instance?.Hide()` in both `ClearCards()` (`:849`) and `BeginConfirmPick()` (`:599`), preventing the tooltip from staying open after cards are destroyed in the normal flow.
  * **Note 1 (Orphaned Tooltip Panel on Destroy):** `TooltipSystem.cs` creates `tooltipPanel` as a direct child of the scene's `Canvas` (`TooltipSystem.cs:48`), rather than as a child of its own GameObject. However, `TooltipSystem` does **not** implement an `OnDestroy` method. If the GameObject holding the `TooltipSystem` is destroyed (for example, on scene transitions, or if `SkillOfferUI_Auto` is cleared), the `tooltipPanel` GameObject under the Canvas will be **leaked/orphaned** in the scene.
    * *Recommended Fix:* Add the following to `TooltipSystem.cs`:
      ```csharp
      private void OnDestroy()
      {
          if (tooltipPanel != null)
              Destroy(tooltipPanel);
      }
      ```
  * **Note 2 (Stuck Tooltip on Disable):** If the GameObject holding `TooltipSystem` is set inactive (`gameObject.SetActive(false)`) while a tooltip is actively showing, its `Update()` will stop running, and `tooltipPanel` (which is parented to the Canvas) will remain active and **stuck on screen** forever.
    * *Recommended Fix:* Add the following to `TooltipSystem.cs`:
      ```csharp
      private void OnDisable()
      {
          Hide();
      }
      ```
  * **Note 3 (Canvas Caching Risk):** `TooltipSystem` resolves and caches its Canvas once in `Start()` via `BuildTooltip()`. If `TooltipSystem.Instance` is created by `CharacterSelectScreen` and then reused by `SkillOfferUI` in the same scene but they are placed on different Canvas components, the tooltip will be drawn on the deactivated or wrong Canvas.

---

## 2. Synergy Pulse (SkillBarUI)
*Evaluating potential side effects, border glow overrides, and scale collisions.*

### Cooldown & Ready Flash Collisions
* **Visual Conflicts:** **PASS-WITH-NOTES**.
  * **Scale Collision:** **PASS**. The synergy pulse scales the slot's root transform (`ui.root`) up to `1.12x` (`1f + SynergyPulseScale * wave`) and correctly resets it to `Vector3.one` when finished (`SkillBarUI.cs:332`). Since no other code animates the local scale of the slot roots, there is zero conflict here.
  * **Border Color Override:** **PASS-WITH-NOTES**. During the `0.45s` pulse, the border color is overridden from the class accent (e.g. Warblade Orange) to `RimaUITheme.Cyan` (`SkillBarUI.cs:343`). 
    * If a skill is on cooldown, its dim border glow is overridden by the bright Cyan pulse. However, since this happens while the draft UI is open and the game is paused, this is a harmless visual feedback cue.
    * Once the pulse timer (`synergyPulseTimers[i]`) decays to `0f`, the color override ceases. On the very next frame, `UpdateSlot()` will naturally set the color back to the correct class accent border. Thus, there is no permanent color state corruption.
    * **Note 4 (Unscaled Delta Time for Pulse):** Decrementing `synergyPulseTimers[i]` using `Time.unscaledDeltaTime` (`SkillBarUI.cs:241`) is correct because the pulse needs to animate while drafting is open (`timeScale = 0`).

---

## 3. TooltipSystem Style Modification Caller Impact
*Evaluating if the styling changes break other callers like `CharacterSelectScreen` or `SkillCodexUI`.*

### Caller Compatibility
* **Canvas Resolution Fallback:** **PASS**.
  * The canvas lookup changed to `GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>()` (`TooltipSystem.cs:43`).
  * In `CharacterSelectScreen.cs:1617` and `SkillCodexUI.cs:447`, the `TooltipSystem` is added to a newly instantiated GameObject named `"TooltipSystem_Runtime"` (which has no parent Canvas). Thus, `GetComponentInParent` returns null and it falls back to `FindObjectOfType<Canvas>()` exactly as before.
* **Layout & Font Size Scaling:** **PASS**.
  * Font size was increased from `10` to `11`, and the color was shifted slightly to a cyan hue (`TooltipSystem.cs:87-88`).
  * Since `TooltipSystem.cs` dynamically calculates size delta using TextMeshPro's rendered values (`tooltipText.GetRenderedValues(false)`), the tooltip box automatically adjusts its height and width and does not clip or overflow.
* **Realtime Delay Benefit:** **PASS**.
  * Switching from `WaitForSeconds` to `WaitForSecondsRealtime` is a direct fix for other callers like `SkillCodexUI` and `CharacterSelectScreen` where timeScale can be `0`. Tooltip delay now functions reliably in all screen states.
