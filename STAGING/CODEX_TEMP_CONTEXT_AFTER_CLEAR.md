# Codex Temp Context After Clear

Date: 2026-05-03
Purpose: Short handoff context before user runs /clear.

## User Instruction

- User wants Codex to act only on explicit tasks.
- Do not change many files unless directly asked.
- Do not improvise broad cleanup.
- When asked for guides, write Turkish guidance, but project internal md files stay ASCII.
- Keep Codex-marked notes when updating CURRENT_STATUS.md so Claude can review them later.

## Latest Work Context

- Claude limit ended; user pasted prior Claude/Codex conversation here.
- CODEX.md was read in this session.
- Important rule from CODEX.md: Codex is mechanical executor, does not override Claude, stays in task scope.
- Do not edit CLAUDE.md unless explicitly asked. It was previously only read.

## Current Direction/Animation Decision

- Earlier 4-diagonal-only idea was challenged by user and then superseded.
- Current intended behavior is Hades-like visually, not a claim about Hades internal code.
- Player facing should be 8-way last-facing:
  - move left -> future run_W -> stop on idle_W
  - move right -> future run_E -> stop on idle_E
  - move up-left -> future run_NW -> stop on idle_NW
  - move down-right -> future run_SE -> stop on idle_SE
  - input noise/deadzone should not change facing
- Run animations are not created yet. Pipeline should be run-first, no walk.

## Critical Open Risk

CODEX.md has a permanent S43 Direction Convention table with an offset mapping:

```text
S  -> source south-east.png
SE -> source east.png
E  -> source north-east.png
NE -> source north.png
N  -> source north-west.png
NW -> source west.png
W  -> source south-west.png
SW -> source south.png
```

This may conflict with the latest pasted conversation, where controller rebuilds may have used direct source names
like idle_W -> west.png and idle_S -> south.png.

Before any more Unity animation/import work, verify actual sprite/controller binding against CODEX.md and
MEMORY/feedback_pixellab_direction.md. Do not assume direct compass mapping is correct.

## Files Mentioned As Recently Touched In Pasted Conversation

- Assets/Scripts/Player/PlayerController.cs
- Assets/Scripts/Player/PlayerAnimator.cs
- Assets/Tests/EditMode/PlayerAnimatorDirectionTests.cs
- Assets/Scripts/UI/CharacterSelectScreen.cs
- Assets/Animations/Characters/*/*.controller
- Assets/Resources/Characters/*/*.controller
- Assets/Animations/Characters/*/*idle*.anim
- STAGING/WARBLADE_ANIMATION_PIPELINE.md
- STAGING/PRODUCTION_GUIDE_S43.md
- STAGING/SKILL_SHEETS_CODEX_EVALUATION.md
- CURRENT_STATUS.md

Do not revert unrelated dirty worktree changes.

## Verification Reported In Pasted Conversation

- EditMode direction + character select tests reportedly passed 16/16 after fixes.
- Runtime frame sampling reportedly showed all 8 stop directions stable for 4 frames each.
- Console warnings were MCP client handler logs, not game compile errors.

These should be re-verified if the next task depends on them.

## Next Sensible Task

If user asks to continue animation work:

1. Read CURRENT_STATUS.md, CODEX.md, MEMORY/feedback_pixellab_direction.md, and relevant Warblade guide.
2. Verify actual Unity mapping frame-by-frame before editing.
3. Resolve S43 offset vs direct compass mapping explicitly.
4. Only then produce/run/import Warblade run animations or update docs.

End.
