# CODEX -- Codex Agent Guide (RIMA)
Role: mechanical bulk work executor. NOT the orchestrator.
Universal project rules: RULES.md (read first). Claude orchestrator rules: CLAUDE.md.
Agent stack: ANTIGRAVITY.md. Routing: AGENTS.md.

## Codex Capabilities in RIMA

Codex handles bounded, well-scoped mechanical tasks. The orchestrator (Claude) gives you:
- Exact file paths allowed to read/write
- Inline excerpts where needed
- Expected output format

Do NOT auto-discover context. If a required file is not in your task scope, STOP and report.

## Codex Skills (What to Do)

### Skill Implementation
- Add new skills under Assets/Scripts/Skills/<ClassName>/<SkillName>.cs
- Pattern: extend SkillBase, set [Header] fields, implement Execute()
- Cost fields: rageCost (Warblade) or resourceCost (others)
- Call GetComponentInParent<> in Awake (see SkillBase.cs lines 28-33)

### Resource System Additions
- New resource: extend PlayerResourceBase, implement Current/Max/TrySpend/Add
- Fires OnResourceChanged (UnityEvent<int,int>) on every change
- Register in Warblade_SkillController / target SkillController

### State Machine Extensions (BasicAttack)
- New state: implement IBasicAttackState (CurrentStep, StepWindow, WindowOpen, Advance, Reset, Tick)
- Wire into BasicAttackProfile (ScriptableObject)
- MeleeChainState and CastRhythmState are reference implementations

### Enemy / Affix Additions
- New mob affix: implement IMobAffix interface, add under Enemies/Affixes/
- New mob attack: extend or follow pattern of MobAttack_* classes under Enemies/Attacks/
- BaseMobBehavior drives lifecycle; EnemyAI drives targeting/movement

### Refactoring (mechanical only)
- Extract shared logic into base class / interface (no design changes)
- Rename symbols across a bounded file set when orchestrator lists them
- Remove dead code from files orchestrator explicitly flags

### Test Writing (EditMode)
- Target: DungeonGraph, PlayerResourceBase subclasses, SkillBase.TryActivate flow, StatusEffectSystem
- EditMode rules: no Awake() -- call explicit Init(). Seed: Random.InitState(42).
- Use Is.InRange for DungeonGraph assertions
- Coroutines and Singletons -> PlayMode only

### Commit Format
Every commit: Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
One commit per bounded task. Commit message: imperative tense, what changed.

## CODEX_TASK.md Otomasyonu

Kullanici "codex_task.md oku" ya da "codex.md oku" dediginde su sirayı takip et:

1. **CODEX_DONE.md'yi bosalt** (tamamen sil veya bos birak) -- onceki sonucu temizle
2. **CODEX_TASK.md'yi oku** -- gorev buradadir
3. **Gorevi yap** -- sadece dosya okuma/yazma; MCP arac yok
4. **CODEX_DONE.md'ye yaz** -- Claude'un anlayacagi formatta sonucu rapor et
5. **Gorev DONE ve ERRORS: NONE ise** -- CODEX_TASK.md icerigini tamamen sil (bos birak)
6. **Gorev PARTIAL veya BLOCKED ise** -- CODEX_TASK.md'ye dokunma, CODEX_DONE.md'ye neden bildir

CODEX_DONE.md rapor formati:
```
STATUS: DONE / PARTIAL / BLOCKED
COMPLETED:
- <yapilan is>
ERRORS: NONE / <liste>
FILES_TOUCHED: <yollar>
COMMIT: <hash veya NONE>
NEXT_SIGNAL: <Claude'a ne yapacagini soyle>
```

## Standart Workflow

1. Read CLAUDE.md (project rules, encoding, folder map).
2. Read only the files the orchestrator listed. Nothing else.
3. Implement exactly the bounded scope given. No scope expansion.
4. Commit. Report in the format below.
5. If scope is ambiguous or a file is missing from your list: STOP, report BLOCKED.

Report format every task:
```
STATUS: DONE / PARTIAL / BLOCKED
COMPLETED: <bullets>
ERRORS: NONE / <list>
FILES_TOUCHED: <paths>
COMMIT: <hash>
```

## What NOT to Do

- No architecture decisions (class hierarchy, system design) -- defer to Claude
- No balance decisions (damage numbers, cooldowns, costs) -- defer to Claude
- No cross-system changes unless every affected file is in the allowed list
- No edits to GDD.md, MASTER_KARAR_BELGESI.md, or archived files
- No edits to Library/ or PackageCache/
- No non-ASCII characters in .md files (encoding mismatch between agents)
- No spawning sub-agents or calling external tools not in your task scope
- No silent retries beyond the scope given -- report errors, let orchestrator decide
