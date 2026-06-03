I will start by checking the current permissions to see which paths are accessible for file operations and commands.
I will list the contents of the `C:\Users\ydbil\.gemini\antigravity-cli\scratch` directory to understand the folder structure and verify if the `STAGING` directory exists there.
I will list the contents of the `STAGING` directory inside scratch to see what files exist there.
I will read the `RIMA_CANON_BRIEF_FROM_NLM.md` file to see what information about RIMA and its classes is already available in the scratch folder.
I will inspect the file `AGY_DONE_ydbilgin.md` to see if there is any relevant background or existing notes from earlier rounds.
I will read `RESEARCH_HADESLIKE_AGY.md` to see if it contains information related to weapon variants or if it's the precursor to the research I'm conducting now.
I will run an NLM query using the provided CLI tool to retrieve information about the Summoner and Hexer class kits and resources in the RIMA canon.
I will check the status of the NLM query task to see if it has finished executing and returned the class details.
I will initiate another NLM query in the background to get a complete breakdown of all 10 classes' resources, base weapons, and signature mechanics according to the RIMA design canon.
I will schedule a brief one-shot timer to allow the background NLM queries to complete and return their results.
I will now pause to let the background queries execute. I will resume as soon as the results are returned.
I will check the list of running tasks to see if our NLM queries have completed.
I will schedule another timer for 30 seconds to allow the background NLM queries to complete their searches.
I will pause and wait for the tasks to finish.
I will check the status of our background tasks to see if the second NLM query (task-20) has completed.
I will schedule another 15-second timer to give the second NLM query (task-20) time to complete.
I will pause to wait for the task.
I will create the research file `RESEARCH_WEAPONS_AGY.md` inside the `STAGING` directory. This document will analyze how weapon variants, swaps, and skill evolutions can be implemented in RIMA. It draws concrete mechanics from games like Hades, Dead Cells, and Risk of Rain 2, maps them to RIMA's 10 classes, and flags conflicts with locked design directions.
I will create `AGY_DONE_Antigravity.md` in the scratch directory to serve as the signature completion file for this task. It will mirror the structured research contents to record the completion under my agent name.
I have completed the breadth-lens research on per-class weapon variants, weapon-swap mechanics, and skill evolution for **RIMA: Sundered Brink**.

The findings have been successfully written to the following files:
*   [RESEARCH_WEAPONS_AGY.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/scratch/STAGING/RESEARCH_WEAPONS_AGY.md) (Main research document in staging)
*   [AGY_DONE_Antigravity.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/scratch/AGY_DONE_Antigravity.md) (Task completion signature file)

### Summary of the Research

1. **Per-Class Weapon-Variant Table:**
   Grounded all 10 classes in 2-3 specific weapon variants. These utilize RIMA's `HandAnchor` system to swap sprites cheaply without requiring custom character animation rigs. They modify stats like attack speed, range, and resources (e.g., Ronin's Tension, Brawler's Charge, Hexer's Hex Stacks).

2. **Weapon-Swap Model Options:**
   Analyzed three models based on source games:
   *   *Pre-Run Hub selection (Hades Aspects/Wizard of Legend)*: Locked for the run, purchased with **Shattered Echoes**.
   *   *Mid-Run Draft modifiers (Hades Hammers/Brotato/Risk of Rain 2)*: Upgrades properties of the equipped weapon between stages.
   *   *On-the-fly random drops (Dead Cells/Skul)*: Instantly swaps character builds mid-stage.
   *   **Recommendation:** A hybrid of **Hub Aspects** (establishing base sprite, basic attack, resource scaling) and **Mid-Run Draft Cards** (offering weapon-specific skill evolutions).

3. **Weapon-Driven Skill-Evolution Model:**
   To satisfy the locked **12 skills/class** directive, weapon variants do *not* create new skills. Instead, they act as runtime decorators that modify the **Shape** and **State** tags (e.g., changing *Line* to *Cone*, or *Fracture* to *Bleed*) of existing skills. Detailed Q/E/R/F evolution examples are provided for **Warblade**, **Ranger**, and **Gunslinger**.

4. **Power-Creep & Scope Control:**
   *   **The 12-Skill Constraint**: Standardizes skill databases by applying weapon properties as decorators rather than individual skill assets.
   *   **Rig Rigidity**: Restricts combat actions to 3 base rigs (**Swing**, **Cast**, **Shoot**), ensuring new variants map directly onto existing sprite animations.
   *   **Tag-Based Synergy**: Decouples specific logic from weapons by utilizing the global synergy matrix (Shape/State tags).

5. **Locked Direction Conflicts Flagged:**
   *   *12 skills/class (Depth-not-Breadth)*: Writing unique skills per variant violates this. Mitigated by runtime decorator tag-shifting.
   *   *PixelLab sprite budget*: Custom animation sets per weapon violate this. Mitigated by using static rigs and attaching sprites to `HandAnchor`.
   *   *Cyan-Sparing*: Weapon glows must remain subtle to not wash out the desaturated slate-and-iron color palette.

6. **Top Vertical Slice Picks for "Sundered Beat" (BREAK -> EXECUTE):**
   *   **Warblade [Greatsword vs. Iron Greatmace]**: Greatmace swaps sweep attacks for high-commitment armor-breaking slams.
   *   **Ranger [Precision Bow vs. Arbalest Crossbow]**: Crossbow trades kiting mobility for high-piercing guard destruction.
   *   **Gunslinger [Dual Revolvers vs. Sawed-Off Shotgun]**: Shotgun pulls the ranged character into close-quarters melee risk.
