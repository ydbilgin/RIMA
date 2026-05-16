# Codex Task — Generate Skill .asset SOs for 4 Incomplete Classes

**Type:** Implementation (mechanical .asset SO generation from existing .cs files)
**Effort:** medium
**Estimated:** 2-4 hours Codex (4 sub-dispatches @ ~30-60 min each)
**Dispatch:** AWAIT USER APPROVAL (Q3 in class_skill_gap_analysis_s85.md)
**Output:** ~50 new .asset files + CODEX_DONE.md report

---

## 0. MUST READ FIRST

1. `STAGING/class_skill_gap_analysis_s85.md` — full gap analysis + approval status
2. `Assets/Scripts/Skills/SkillData.cs` — SO field signature
3. `Assets/Data/Skills/Skill_WhirlwindSlash.asset` — reference YAML format
4. For each target class, the corresponding skill .cs files

---

## 1. Context

RIMA has 10 player classes. Skill IMPLEMENTATION status:

| Class | .cs Skill files | .asset Skills | Gap |
|---|---|---|---|
| Warblade | 14 | 7 | 7 missing |
| Shadowblade | ~14-22 | 0 | ~14-22 missing |
| Elementalist | ~13+ | 0 | ~13+ missing |
| Ranger | ~14-18 | 0 | ~14-18 missing |
| **Total .asset gap** | ~55-60 | 7 | **~48-53** |

**This task generates the missing .asset files** so the 4 implemented classes have full SkillData SOs. The 6 fully missing classes (Brawler, Ronin, Ravager, Gunslinger, Hexer, Summoner) are a separate task (skill design + .cs + .asset — see class_skill_gap_analysis_s85.md §3).

---

## 2. SkillData Format (from `Assets/Scripts/Skills/SkillData.cs`)

```csharp
public class SkillData : ScriptableObject {
    public string skillName;
    [TextArea] public string description;
    public SkillTier tier;                 // Common=0, Rare=1, Epic=2, Mythic=3, Legendary=4
    public Sprite icon;                    // nullable
    public int damage;
    public float cooldown;
    public SkillTag[] tags;                // Melee, Ranged, Dash, AOE, Fire, Ice, Lightning, Void, Poison, Physical, Summon, Trap, Passive

    public ClassType classType;            // None, Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer

    public bool isPassive;
    [TextArea] public string passiveDescription;
    public StatusEffectType appliesEffect;
}
```

Reference .asset YAML (`Assets/Data/Skills/Skill_WhirlwindSlash.asset`):
```yaml
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_Script: {fileID: 11500000, guid: 9c6b129952618bf46bb6a44104f7dd61, type: 3}
  m_Name: Skill_WhirlwindSlash
  skillName: Whirlwind Slash
  description: "..."
  tier: 1
  damage: 35
  cooldown: 5
  tags: [...]
  classType: 1
  isPassive: 0
  ...
```

---

## 3. Scope — Sub-Dispatch Strategy

**4 sub-dispatches, one per class.** Each sub-dispatch:
1. Reads all .cs files in `Assets/Scripts/Skills/<Class>/` (excluding `<Class>SkillBase.cs`, `<Class>_SkillController.cs`, base/utility files)
2. For each skill .cs, extracts:
   - Class name (file name without .cs)
   - Best-guess `description` from class XML comments or first lines
   - Best-guess `tags` from .cs (e.g., uses `DamageZone` → AOE; mentions `Projectile` → Ranged)
   - Best-guess `damage` and `cooldown` from public fields if present (else defaults: damage=20, cooldown=5)
   - `classType` per dispatch target (Warblade/Shadowblade/Elementalist/Ranger)
3. Creates `Assets/Data/Skills/<Class>/Skill_<Name>.asset` (NEW subfolder per class to keep organized)
4. Verifies SkillData YAML loads in Unity Inspector

### Sub-dispatch order (priority by user feedback):
- **Sub 1:** Warblade — fill 7 missing (BladeRush, Cleave, CripplingBlow, IronCounter, IronCrush, IroncladMomentum, BattleSurge, SunderMark, DeepWound, Earthsplitter — list will be confirmed by .cs glob)
- **Sub 2:** Shadowblade — ~14-22 .asset
- **Sub 3:** Elementalist — ~13+ .asset
- **Sub 4:** Ranger — ~14-18 .asset

---

## 4. Acceptance Criteria

A. `dotnet build RIMA.Runtime.csproj` returns 0 errors (no new code, only .asset, but verify no breakage).
B. Each generated `.asset` opens in Unity Inspector without missing script reference.
C. Each `.asset` has correct `classType` set to its parent class enum value.
D. Skill name matches .cs class name (with sensible spacing — "BladeRush" → "Blade Rush").
E. Default tags are guesses; user will manually tune in Inspector after.
F. New subfolders `Assets/Data/Skills/<Class>/` created with .meta.
G. Existing 7 Warblade .asset files NOT modified (skip if present, log "exists" in report).
H. All `.asset.meta` GUIDs are unique (duplicate scan PASS).
I. No `.cs` file modified.
J. No commits — orchestrator commits.

---

## 5. Safety Rules

All rules from `STAGING/codex_safety_review_output.md` apply. Key:

1. **No code modifications.** This task only creates `.asset` SO files.
2. **Read before write** — for each .cs, read header + public fields to extract defaults.
3. **AssetDatabase batch:** `StartAssetEditing()` + try + finally `StopAssetEditing()` + `SaveAssets()`.
4. **No `AssetDatabase.Refresh()`** (no external files written).
5. **Folder creation safety:** `Directory.CreateDirectory()` per class folder; commit `.meta` for each new folder.
6. **Max 5 files per dispatch** is impossible at this scale (1 class = 14-22 .asset). Each class dispatch is ~14-22 `.asset` writes which is acceptable IF wrapped in single `StartAssetEditing()` block. Codex must NOT issue mid-batch `SaveAssets`.

---

## 6. Codex Self-Review Checklist

1. Did I read SkillData.cs format?
2. Did I read at least one existing .asset YAML as reference?
3. Did I read all target .cs files BEFORE writing .asset?
4. Did I avoid modifying any .cs file?
5. Are all generated .asset's `classType` correct?
6. Are all generated .asset filenames `Skill_<PascalName>.asset`?
7. Did I avoid overwriting existing Warblade .asset files (those 7)?
8. Did I create `<Class>/` subfolders and their `.meta` files?
9. Did `dotnet build` still pass?
10. Did I list all new .asset GUIDs in CODEX_DONE.md (for duplicate scan)?
11. Did I leave damage/cooldown/tags as best-effort defaults, ready for user tuning?
12. Did I NOT commit?

---

## 7. Output Format (CODEX_DONE.md)

```markdown
# Class Skill .asset Generation — Codex Report

## Sub-dispatch 1: Warblade
Files created: [count]
[list: Skill_<Name>.asset paths + GUIDs]
Skipped (already exists): [list]

## Sub-dispatch 2: Shadowblade
...

## Sub-dispatch 3: Elementalist
...

## Sub-dispatch 4: Ranger
...

## Total
Created: [N] new .asset files
Skipped: [M] existing
GUID scan: [PASS/FAIL]

## Default Field Heuristics Used
- damage default: 20 (unless class field found)
- cooldown default: 5 (unless class field found)
- tags default per class:
  - Warblade: [Melee, Physical]
  - Shadowblade: [Melee, Physical, Poison/Void hints]
  - Elementalist: [Ranged, Fire/Ice/Lightning per skill name hints]
  - Ranger: [Ranged, Physical, Trap hints]

## Manual Tuning Required (user)
[for each class, list which fields need user review: damage tuning, tag accuracy, description polish]

## Build Result
dotnet build: PASS / FAIL

## Files Modified Outside Scope
None (must be empty).
```

---

## 8. User Approval Gate

Before dispatch, user must approve:
- [ ] **Q3** in `class_skill_gap_analysis_s85.md` — "Generate .asset SOs for 4 incomplete classes" — YES/NO
- [ ] Sub-folder structure `Assets/Data/Skills/<Class>/` (vs flat Assets/Data/Skills/) — confirm preference
- [ ] Default heuristics (damage=20, cooldown=5) acceptable as placeholder, or user has different defaults?

---

## 9. Dispatch Command (after user approval)

```bash
python cx_dispatch.py --task-file STAGING/codex_class_skill_asset_gen.md --effort high
```

Run as BG. ~2-4 hours total. Orchestrator monitors per-sub-dispatch CODEX_DONE.md output and runs rima-qc after each class is done.
