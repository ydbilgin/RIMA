# CODEX TASK: Review PIXELLAB_CLASS_DESCRIPTIONS.md

SADECE BUNU YAP. BAŞKA HİÇBİR DOSYAYA, SCRIPTE, PREFAB'A VEYA AYARA KARIŞMA.

## Target File

`TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/PIXELLAB_CLASS_DESCRIPTIONS.md`

## Task

Read the file and run the following checks for all 10 class prompts. Report every finding.

---

## Check 1 — Absolute Canvas Direction Language

Look for any remaining absolute direction words in the prompt backtick strings:
- `left`, `right`, `downward-left`, `downward-right`, `upper-left`, `upper-right`

Exception: words like "left shoulder" or "left hand" that describe CHARACTER ANATOMY (not canvas direction) are acceptable and should NOT be flagged.

Flag only cases where the direction describes WHERE ON THE CANVAS the weapon/item appears (e.g. "blade tip past left hip", "barrel pointing downward-right").

---

## Check 2 — Weapon Visibility Invariant

Every class that has a weapon or held item must contain the phrase:
`remains clearly ... outside ... body silhouette ... in every directional rotation frame`
OR equivalent language covering all-direction visibility.

Check each class:
- Warblade (greatsword)
- Brawler (fists)
- Ravager (two axes)
- Ronin (drawn katana + sheathed katana)
- Shadowblade (two void blades)
- Elementalist (lightning orb)
- Gunslinger (two pistols)
- Hexer (staff + lantern)
- Ranger (bow + nocked arrow)
- Summoner (scepter)

Flag any class missing the invariant.

---

## Check 3 — Canvas Fill Language

Every class prompt must end with:
`character fills canvas height from head to feet full figure tightly framed no dead space above or below`

Verify all 10 classes have this exact phrase at the end of their backtick string.

---

## Check 4 — Duplicate / Contradictory Weapon Descriptions

For each class, check if the weapon is described more than once in conflicting ways (e.g., described as "at left hip" in one place and "outside body boundary" in another with a contradiction). Flag contradictions only, not repetition for emphasis.

---

## Check 5 — Gender Consistency

| Class | Expected Gender |
|-------|----------------|
| Warblade | male |
| Brawler | male |
| Ravager | male |
| Ronin | male |
| Shadowblade | male |
| Elementalist | female |
| Gunslinger | female |
| Hexer | female |
| Ranger | female |
| Summoner | female |

Verify the first word of each prompt is `male` or `female` as listed above.

---

## Output Format

List findings per check. If a check passes for all classes, write "PASS — all classes clear".
Do not rewrite the prompts. Report only.

---

## REPORT

STATUS: 
COMPLETED:
ERRORS:
NEXT_SIGNAL: "Codex review tamamlandı, bulgular hazır"
