ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

# T-LOC Call-site Replacements

`Loc.cs` is already written at `Assets/Scripts/Core/Loc.cs` (namespace RIMA).
All 9 UI/Combat files below must be updated to use `Loc.T(key)` instead of hard-coded strings.

**NO NEW FILES.** Only edit the listed files.
**NO REFACTOR** of unrelated code.
After all edits, use `read_console` to confirm 0 compile errors.

---

## 1. MainMenuController.cs — `Assets/Scripts/UI/MainMenuController.cs`

Subscribes to `Loc.OnLanguageChanged` to rebuild (the whole menu is rebuilt on `Start` → on language change call `RebuildMenu()`).

### Changes:

a) In `Start()` add subscribe: `Loc.OnLanguageChanged += RebuildMenu;`

b) Add `OnDestroy()`:
```csharp
private void OnDestroy() { Loc.OnLanguageChanged -= RebuildMenu; }
```

c) Add helper:
```csharp
private void RebuildMenu()
{
    if (runtimeRoot != null) Destroy(runtimeRoot.gameObject);
    BuildRuntimeMenu();
}
```

d) In `AddTitleColumn`:
- `"RIMA"` → `Loc.T("menu.title")`   (keep as-is per instructions — title stays RIMA)
- `"THE RIFT HUNTERS"` → `Loc.T("menu.subtitle")`
- `"Yine geldin."` → `Loc.T("menu.whisper")`
- `"BAŞLA"` → `Loc.T("menu.btn.start")`
- `"AYARLAR"` → `Loc.T("menu.btn.settings")`
- `"ÇIKIŞ"` → `Loc.T("menu.btn.quit")`

---

## 2. SettingsMenuUI.cs — `Assets/Scripts/UI/SettingsMenuUI.cs`

### Changes:

a) Add `using RIMA;` if not already present. (Namespace is `RIMA`, class is in `RIMA` namespace — `using RIMA;` may not be needed, but ensure `Loc` is reachable; since `Loc` is in namespace `RIMA` and `SettingsMenuUI` is also in `RIMA`, no extra using needed.)

b) Add `const string PrefLang = "rima.lang";`

c) In `BuildUI()`:
- `title.text = "SETTINGS"` → `title.text = Loc.T("settings.title")`
- `AddSectionHeader(panel, "GAMEPLAY", y)` → `AddSectionHeader(panel, Loc.T("settings.gameplay"), y)`
- `AddBoolToggleRow(panel, "Aim Mode", "MOUSE", "FACING", ...)` → label=`Loc.T("settings.aim_mode")`, onText=`Loc.T("settings.mouse")`, offText=`Loc.T("settings.facing")`
- `AddBoolToggleRow(panel, "Dash Mode", "MOUSE", "FACING", ...)` → label=`Loc.T("settings.dash_mode")`, onText=`Loc.T("settings.mouse")`, offText=`Loc.T("settings.facing")`
- `AddSectionHeader(panel, "ACCESSIBILITY", y)` → `Loc.T("settings.accessibility")`
- `AddToggleRow(panel, "Screen Shake", ...)` → label=`Loc.T("settings.screen_shake")`
- `AddToggleRow(panel, "Hit Stop", ...)` → label=`Loc.T("settings.hit_stop")`
- `AddToggleRow(panel, "Low HP Vignette", ...)` → label=`Loc.T("settings.low_hp_vignette")`
- `AddToggleRow(panel, "Damage Numbers", ...)` → label=`Loc.T("settings.damage_numbers")`
- `AddToggleRow(panel, "Chromatic Aberration", ...)` → label=`Loc.T("settings.chromatic_aberration")`
- `AddSectionHeader(panel, "AUDIO", y)` → `Loc.T("settings.audio")`
- `AddSliderRow(panel, "Master", ...)` → label=`Loc.T("settings.master")`
- `AddSliderRow(panel, "Music", ...)` → label=`Loc.T("settings.music")`
- `AddSliderRow(panel, "SFX", ...)` → label=`Loc.T("settings.sfx")`
- `AddSectionHeader(panel, "CONTROLS", y)` → `Loc.T("settings.controls")`
- `AddReadOnlyRow(panel, "Move", "WASD", y)` → label=`Loc.T("settings.move")`
- `AddBindRow(panel, "Dash", ...)` → `Loc.T("settings.dash")`
- `AddBindRow(panel, "Attack", ...)` → `Loc.T("settings.attack")`
- `AddBindRow(panel, "Alt Attack", ...)` → `Loc.T("settings.alt_attack")`
- `AddBindRow(panel, "Skill 1", ...)` → `Loc.T("settings.skill_1")`
- `AddBindRow(panel, "Skill 2", ...)` → `Loc.T("settings.skill_2")`
- `AddBindRow(panel, "Skill 3", ...)` → `Loc.T("settings.skill_3")`
- `AddBindRow(panel, "Skill 4", ...)` → `Loc.T("settings.skill_4")`
- `AddBindRow(panel, "Rift Break", ...)` → `Loc.T("settings.rift_break")`
- `AddButton(panel, "RESET CONTROLS", ...)` → `Loc.T("settings.btn.reset")`
- `AddButton(panel, "RESUME", ...)` → `Loc.T("settings.btn.resume")`
- `AddButton(panel, "QUIT TO MENU", ...)` → `Loc.T("settings.btn.quit_to_menu")`

d) In `AddToggleRow` the ON/OFF fallback (when `customText == null`):
- `"ON"` → `Loc.T("settings.on")`
- `"OFF"` → `Loc.T("settings.off")`

e) Add Language toggle row **at the top of the GAMEPLAY section** (before aim/dash rows, after `AddSectionHeader(panel, Loc.T("settings.gameplay"), y)`):

```csharp
// ── Language toggle ──────────────────────────────────────────
y = AddLanguageRow(panel, y);
```

And the new helper method:
```csharp
private float AddLanguageRow(RectTransform parent, float y)
{
    var row = MakeRect("Toggle_Language", parent);
    row.anchorMin = new Vector2(0f, 1f);
    row.anchorMax = new Vector2(1f, 1f);
    row.pivot = new Vector2(0f, 1f);
    row.anchoredPosition = new Vector2(30f, y);
    row.sizeDelta = new Vector2(-60f, 22f);

    var lbl = MakeTMP("Label", row);
    var lr = lbl.GetComponent<RectTransform>();
    lr.anchorMin = Vector2.zero;
    lr.anchorMax = new Vector2(0.6f, 1f);
    lr.offsetMin = lr.offsetMax = Vector2.zero;
    lbl.fontSize = 10f;
    lbl.color = new Color(0.8f, 0.85f, 0.9f, 0.9f);
    lbl.alignment = TextAlignmentOptions.Left;

    var btnGo = new GameObject("LangBtn", typeof(RectTransform));
    btnGo.transform.SetParent(row, false);
    var btnRt = btnGo.GetComponent<RectTransform>();
    btnRt.anchorMin = new Vector2(0.65f, 0f);
    btnRt.anchorMax = new Vector2(1f, 1f);
    btnRt.offsetMin = btnRt.offsetMax = Vector2.zero;

    var btnImg = btnGo.AddComponent<Image>();
    var btnTxt = MakeTMP("BtnTxt", btnGo.GetComponent<RectTransform>());
    var btRt = btnTxt.GetComponent<RectTransform>();
    btRt.anchorMin = Vector2.zero;
    btRt.anchorMax = Vector2.one;
    btRt.offsetMin = btRt.offsetMax = Vector2.zero;
    btnTxt.fontSize = 9f;
    btnTxt.fontStyle = FontStyles.Bold;
    btnTxt.color = Color.white;
    btnTxt.alignment = TextAlignmentOptions.Center;

    Color onColor  = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.3f);
    Color offColor = new Color(0.2f, 0.2f, 0.25f, 0.5f);

    void Paint()
    {
        bool isTR = Loc.CurrentLanguage == "tr";
        lbl.text = Loc.T("settings.language");
        btnTxt.text = isTR ? Loc.T("settings.lang.tr") : Loc.T("settings.lang.en");
        btnImg.color = isTR ? onColor : offColor;
    }
    Paint();
    refreshers.Add(Paint);

    var btn = btnGo.AddComponent<Button>();
    btn.onClick.AddListener(() =>
    {
        Loc.SetLanguage(Loc.CurrentLanguage == "tr" ? "en" : "tr");
        // OnLanguageChanged will fire; if this panel is still open, refreshers will repaint it.
    });

    return y - 26f;
}
```

f) Subscribe to rebuild in `BuildUI()` (end of method):
```csharp
Loc.OnLanguageChanged += RebuildUI;
```

g) Add `RebuildUI` method and unsubscribe in `OnDestroy`:
```csharp
private void RebuildUI()
{
    // Destroy all children (the built UI) and rebuild.
    var root = GetComponent<RectTransform>();
    if (root != null)
    {
        for (int i = root.childCount - 1; i >= 0; i--)
            Destroy(root.GetChild(i).gameObject);
    }
    refreshers.Clear();
    _bindLabels.Clear();
    masterSlider = null;
    musicSlider  = null;
    sfxSlider    = null;
    BuildUI();
    // Re-apply current open state
    if (isOpen) { canvasGroup.alpha = 1f; canvasGroup.blocksRaycasts = true; canvasGroup.interactable = true; }
}
```

And in `OnDestroy()` append:
```csharp
Loc.OnLanguageChanged -= RebuildUI;
```

---

## 3. CharacterSelectScreen.cs — `Assets/Scripts/UI/CharacterSelectScreen.cs`

Only replace the hard-coded strings at the exact lines noted. Do not restructure.

Find and replace:
- Line ~368: `"RIMA <color=#B0B3BC>- KARAKTER SEÇ</color>"` → `Loc.T("char_select.title")`
- Line ~541: `"KİLİTLİ"` (in `MakeText("KİLİTLİ", ...)`) → `Loc.T("char_select.locked")`
- Line ~751: `"YETENEKLER"` (section header text) → `Loc.T("char_select.skills")`
- Line ~759: `"Yetenekler yakinda"` → `Loc.T("char_select.skills_soon")`
- Line ~808: `"SEÇ"` on the select button label → `Loc.T("char_select.btn.select")`
- Line ~856: `"GERİ"` → `Loc.T("char_select.btn.back")`
- Unlock button text `"KİLİDİ AÇ — {0}"` → use `Loc.T("char_select.btn.unlock", echoAmount)` (where the string is constructed with the echo cost variable)
- `"YETERSİZ SHATTERED ECHO"` → `Loc.T("char_select.not_enough_echo")`
- Stat labels `"HASAR"/"DAYANIK"/"HIZ"/"KONTROL"/"ZORLUK"` → `Loc.T("char_select.stats.damage")` etc.
  Note: "DAYANIK" in code maps to key `char_select.stats.durability` (TR value = "DAYANIKLILIK")
- `"TAM LİSTE"` → `Loc.T("char_select.full_list")`
- Unlock condition string `"{0} SHATTERED ECHO veya {1}"` → `Loc.T("char_select.unlock_condition", arg0, arg1)`

---

## 4. CharacterSheetUI.cs — `Assets/Scripts/UI/CharacterSheetUI.cs`

- `"ACTIVE KIT"` → `Loc.T("char_sheet.active_kit")`
- `"SYNERGIES"` → `Loc.T("char_sheet.synergies")`
- `"DUNGEON ROUTE"` → `Loc.T("char_sheet.dungeon_route")`
- `"ACTIVE ECHOES"` → `Loc.T("char_sheet.active_echoes")`
- `"NO CLASS"` → `Loc.T("char_sheet.no_class")`

---

## 5. ChamberSelectBootstrap.cs — `Assets/Scripts/UI/ChamberSelectBootstrap.cs`

- `$"[G] Bürün — {nearest.classType.ToString().ToUpperInvariant()}"` → `Loc.T("chamber_select.prompt.attune", nearest.classType.ToString().ToUpperInvariant())`
- `$"[G] Kilidi Aç — {UnlockCost(nearest.classType)} SHATTERED ECHO"` → `Loc.T("chamber_select.prompt.unlock", UnlockCost(nearest.classType))`
- `"[G] Rift'e Gir"` → `Loc.T("chamber_select.prompt.enter_rift")`
- `"DUMMY HP {0}/{1}"` pattern (wherever dummy HP is displayed) → `Loc.T("chamber_select.dummy_hp", currentHp, maxHp)` (use the actual variable names in that context)

---

## 6. ExecutePromptDriver.cs — `Assets/Scripts/Combat/ExecutePromptDriver.cs`

- `promptLabel.text = "[RMB] İnfaz"` → `promptLabel.text = Loc.T("combat.prompt.execute")`
  (This is in `CreatePromptLabel()` around line 68.)

Also add re-set in Update when showing (so language switch takes effect live):
After the line that sets `promptLabel.gameObject.SetActive(true)` (when showing the prompt), set:
`promptLabel.text = Loc.T("combat.prompt.execute");`

---

## 7. RewardPickup.cs — `Assets/Scripts/Core/RewardPickup.cs`

- `promptText.text = "Topla: G"` (two occurrences: in `ShowPrompt()` and in `EnsurePromptVisuals()`) → `Loc.T("reward.prompt.take")`
- `HUDController.Instance?.SetInteractionPrompt("Topla: G")` → `HUDController.Instance?.SetInteractionPrompt(Loc.T("reward.prompt.take"))`

---

## 8. SkillOfferUI.cs — `Assets/Scripts/UI/SkillOfferUI.cs`

In `Show()`:
- `$"ODA {roomNumber}  —  ODUL SEC"` → `Loc.T("draft.title_room", roomNumber)`
- `"ODUL SEC"` → `Loc.T("draft.title_generic")`
- `"Birini sec — digerleri kaybolur"` → `Loc.T("draft.subtitle")`

In `ShowReplaceMode()`:
- `titleLabel.text = "SLOT DOLU"` → `Loc.T("draft.slot_full")`
- `subtitleLabel.text = $"{incoming.skillName.ToUpperInvariant()} almak icin hangisini birakmak istiyorsun?"` → `Loc.T("draft.replace_prompt", incoming.skillName.ToUpperInvariant())`
- `skipTxt.text = "ATLA — alma"` → `Loc.T("draft.btn.skip")`

In `BuildRewardCard()`:
- `name = $"+{offer.goldAmount} ALTIN"` → `Loc.T("draft.gold_title", offer.goldAmount)`
- `desc = "Hazinene ekle"` → `Loc.T("draft.gold_desc")`
- `name = $"+%{offer.healPercent} CAN"` → `Loc.T("draft.heal_title", offer.healPercent)`
- `desc = "Aninda iyiles"` → `Loc.T("draft.heal_desc")`
- Echo card desc `"Cagir bir yankisi (C)."` → `Loc.T("draft.echo_desc")`

In `BuildChainChip()`:
- `chipTxt.text = $"⟂ pairs with {partner.ToUpperInvariant()}"` → `Loc.T("draft.pairs_with", partner.ToUpperInvariant())`

Find the "SEC" select button label in `BuildSkillCard()` or wherever the select button text is set → `Loc.T("draft.btn.select")`

---

## 9. DeathScreenManager.cs — `Assets/Scripts/Core/DeathScreenManager.cs`

- `DeathLines` static array → replace with a property/method that reads from Loc:

Replace:
```csharp
private static readonly string[] DeathLines =
{
    "The rift remembers. You won't.",
    "Not an ending. Just a place where you stopped."
};
```
With:
```csharp
private static string[] GetDeathLines() => new[]
{
    Loc.T("death.quote_1"),
    Loc.T("death.quote_2"),
};
```

And update usage: `DeathLines[Random.Range(0, DeathLines.Length)]` → `GetDeathLines()[Random.Range(0, 2)]`

- In `EnsurePanelControls()`:
  - `StyleButton(restartButton, "TEKRAR DENE [R]", 15f)` → `StyleButton(restartButton, Loc.T("death.btn.retry"), 15f)`
  - `StyleButton(mainMenuButton, "ANA MENÜ", 15f)` → `StyleButton(mainMenuButton, Loc.T("death.btn.main_menu"), 15f)`
  - Also the `CreateButton(...)` calls with those same strings.

- In `BuildRunStats()` the `TOPLAM:` line → use `Loc.T("death.stats.total_echo", echoAward)` for just that final line.

---

## 10. DemoCompleteOverlay.cs — `Assets/Scripts/Core/DemoCompleteOverlay.cs`

- `"DEMO COMPLETE"` → `Loc.T("victory.title")`
- `"The full descent awaits."` → `Loc.T("victory.subtitle")`
- `"Next echo awaits — a new class joins the descent."` and `"Next echo: a new class awaits the descent."` → `Loc.T("victory.teaser")`
- `"WISHLIST ON STEAM"` → `Loc.T("victory.btn.wishlist")` (value is same in both langs)
- `"MAIN MENU"` → `Loc.T("victory.btn.main_menu")`
- `"PLAY AGAIN"` → `Loc.T("victory.btn.play_again")`
- In `BuildRunSummary()` the `ODA: {0}    KILLS: {1}    SÜRE: {2}` line → use `Loc.T("victory.stats.summary", RunStats.RoomReached, kills, FormatSeconds(RunStats.RunTimeSeconds))` for that part of the string. Keep the Echo breakdown lines in TR (they use RunStats directly and are secondary info).

---

## 11. SkillCodexUI.cs — `Assets/Scripts/UI/SkillCodexUI.cs`

- `"SKILL CODEX"` (title label) → `Loc.T("codex.title")`
- `"BU SINIF ICIN KAYITLI SKILL YOK"` → `Loc.T("codex.empty")`  (around line 234)
- `$"{0} (YAKINDA)"` pattern → `Loc.T("codex.coming_soon", skillName)` (around line 380)

---

## 12. RunMapOverlay.cs — `Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs`

This uses IMGUI (`GUI.Label`), no TMP rebuild needed.

- `"RUN PATH"` → `Loc.T("map.title")`
- `"M ile kapat   -   cyan = bulundugun oda"` → `Loc.T("map.subtitle")`

---

## Verification

After all edits, call `read_console`. Expected: 0 errors.
If any compile error, fix it before writing CODEX_DONE.md.

Write result to `CODEX_DONE_tloc_callsites.md` with:
- Files edited (list)
- Compile status
- Any BLOCKED items
