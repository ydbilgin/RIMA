# RIMA — Council advisor: DEEP ARCHITECTURE / DESIGN lens

Read this brief and reason FROM it (do NOT re-derive code state — file:line ground truth is already in it):
`F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_process\2026-06\_council_interaction_prompt_tests_2026-06-08.md`

You are the DEEP ARCHITECTURE / LOCALIZATION-MAINTAINABILITY advisor. Be decisive and lean — no essay. Answer the 4 questions in the brief:

1. **Convention A (key token baked into the loc string, current) vs B (single InteractionPromptFormatter adds the key; loc strings = action text only).** Decisive pick + why. Weigh: input keys ([G]/[RMB]/[E]) are NON-translatable physical bindings, yet under A they are duplicated across the TR and EN dicts; key-rebinding cost; whether B structurally ELIMINATES the whole `[G] [G]` bug class; standard i18n practice (separation of binding from translatable text); VERSUS the churn of editing the just-shipped Loc tables + all call sites right before a demo, and the project rule of minimal/surgical change.

2. **Exact test set** under your pick: which EditMode + PlayMode test files, and 4-8 highest-value assertions (no redundant/low-ROI tests). Propose the concrete mechanism to ENUMERATE loc strings for the lint (the dicts are private static in Loc.cs).

3. **Regression-guard tests** for pedestal-identity (#1) and HUD-off-in-chamber (#2): the code already satisfies both — worth a guard test, or wasted effort?

4. **Distinct-sprite test** for the 10 classes (assert each resolves a distinct sprite, not all the same generic silhouette): worth it or out of scope?
