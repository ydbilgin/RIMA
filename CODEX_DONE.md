STATUS: DONE

COMPLETED:
- Applied CT-SKILL-01 deterministic text fixes to `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`.
- Warblade skill #6 renamed from `War Stomp` to `Earthsplitter`.
- Warblade skill #7 chain condition updated to `Earthsplitter sonrasi`.
- Inserted Rage additive clarification note after the Warblade Rage resource line.
- Elementalist skill #11 renamed from `Combustion` to `Element Charge`.
- Elementalist Fire Burst build axis updated to `Element Charge+...`.
- Shadowblade numbered skill #7 renamed from `Sever` to `Severance`; resource `Sever (0-100)` was not touched.
- Ravager `Undying Tenacity` row replaced with the pre-decided death-cheat behavior and updated Death Wish chain condition.
- Ronin Iaido Burst build axis updated from `Quickdraw+...` to `Quickdraw Slash+...`.

VERIFICATION:
- `rg` confirmed the expected replacement strings are present.
- `git diff --numstat -- TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` reports `8` added and `7` removed lines.
- Remaining `War Stomp` references are outside the requested exact changes: Warblade `Control Breaker` build axis and Brawler `Aerial Rave` synergy.
- Remaining `Combustion` references are only in the S41 revision summary old-name row.
- `CT-SKILL-01 done — 8 lines changed.`

ERRORS: NONE
NEXT_SIGNAL: "CT-SKILL-01 done — Claude review deterministic doc edits"
