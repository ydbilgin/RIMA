ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Council (FEASIBILITY/REUSE lens): how to make the RIMA chamber interaction prompt fully Hades-style — ALL interaction text at ONE fixed bottom-of-screen spot, nothing floating. ANALYSIS ONLY, no code changes. Write to CODEX_DONE.md.

# Lens
You are the CODE/FEASIBILITY/REUSE advisor. Answer from "what already exists + cheapest surgical path". Ground in actual code (file:line). Do NOT change code.

# Current code state (Assets/Scripts/UI/ChamberSelectBootstrap.cs)
- There IS already a fixed screen-space prompt: CreatePromptLabel (~1145) builds "ChamberPromptPanel" on a ScreenSpaceOverlay canvas (EnsureChamberOverlayCanvas), anchor (0.5,0.13), size 440x60, dark-slate bg + cyan border child, [G] keycap, TMP fontSize 24, fades in 0.08s. ShowPrompt(pos,text) ignores pos and sets text+active+fade. Proximity loop (~240-335) calls ShowPrompt within interaction radius 1.9 for figures/portal/dummy.
- Prompt text today: unlocked figure → "WARBLADE — Bürün"; locked figure → "[G] RANGER — {cost} Echo ile Aç" (affordable) or "RANGER — [KİLİTLİ] {cost} Echo" (not); portal → "Rift'e Gir"; dummy → "[G] KUKLA — Sınıf Seç".
- There are ALSO floating WORLD-space name labels per figure: CreateWorldText("EchoLabel_{cls}", ...) (~1011), positioned above each pedestal, now shown only for the nearest/highlighted figure (RefreshEchoVisuals SetActive(highlighted)). These float above the figure in world space → their screen position varies as the player moves. The class NAME is therefore shown TWICE when near a figure: once in the fixed bottom prompt and once floating above the figure.
- User complaint: "interact hep ekranın belli yerinde çıkmıyor gibi" — wants EVERYTHING at one fixed bottom spot (Hades). The floating EchoLabel is the varying-position text.

# Questions (feasibility/reuse, numeric where possible)
Q1 Cheapest correct path to make ALL interaction/identity text appear ONLY at the fixed bottom prompt and remove the floating, position-varying text. Specifically: should we DELETE the per-figure CreateWorldText name labels (EchoLabel) entirely and rely on the bottom prompt (which already shows the name+action)? What code is removed vs kept? Any dependency on EchoLabel elsewhere (RefreshEchoVisuals, highlight logic)?
Q2 If floating names are removed, the player can't see which figure is which until walking up to each. Is there a cheap REUSE-friendly way to keep at-a-glance identity WITHOUT floating text (e.g. the bottom prompt already names the focused figure on approach — is that enough)? Or is a single always-visible name under the focused pedestal acceptable if it's the SAME fixed-position element? Give the lowest-risk option.
Q3 Validate the existing bottom prompt content format for each case (unlocked / locked+cost / portal / dummy). Recommend ONE consistent line format that includes identity + action + lock/cost. Is the current ShowPrompt branching reusable as-is with just string tweaks?
Q4 Anything in the current prompt setup (anchor 0.5,0.13, size 440x60, radius 1.9, fade 0.08s, keycap) that is feasibility-wise wrong or should change for a clean Hades feel? Reuse-first.

Write your answer to CODEX_DONE.md.
