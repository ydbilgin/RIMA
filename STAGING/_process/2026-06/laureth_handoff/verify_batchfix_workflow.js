export const meta = {
  name: 'verify-demo-batchfix',
  description: 'Adversarially verify the 6 demo batch-fixes against spec via independent parallel Opus lenses',
  phases: [
    { title: 'Verify', detail: 'one auditor-opus lens per fix, independent' },
    { title: 'Synthesize', detail: 'collect verdicts, flag FAIL/risk' },
  ],
}

// 6 surgical fixes from STAGING/DEMO_BATCH_FIX_SPEC_2026-06-15.md
const FIXES = [
  { id: 'FIX-1', file: 'Assets/Scripts/Skills/DraftManager.cs', expect: 'Start ~L113 lambda PlayerClassManager.OnSecondaryClassSelected += _ => {...} converted to a NAMED private method; unsubscribed in BOTH OnDisable AND OnDestroy with null-guard.' },
  { id: 'FIX-2', file: 'Assets/Scripts/UI/BuildMode/BuildPlacementController.cs', expect: 'HandleKeyboard() ~L295 FIRST line guards: if (searchField != null && searchField.isFocused) return; so typing in search does not trigger hotkeys.' },
  { id: 'FIX-3', file: 'Assets/Scripts/Skills/DraftManager.cs', expect: 'ShowDraft() ~L195 FIRST line (before IsDraftPending=false): if (IsDraftActive) return; (re-entry guard).' },
  { id: 'FIX-4', file: 'Assets/Scripts/UI/BuildModeController.cs', expect: 'EnterBuildMode() ~L211 FIRST line guards against entering while UIManager.IsAnyOverlayOpen OR DraftManager IsDraftActive/IsDraftPending.' },
  { id: 'FIX-5', file: 'Assets/Scripts/UI/DirectorMode.cs', expect: 'SetState() Test-branch else block (the one calling SetOverlayVisible(false)) ALSO sets hasCameraTarget=false. NOTE: this depends on the overlay-fix else block existing; if that else block is absent, FIX-5 should be reported BLOCKED, not invented.' },
  { id: 'FIX-6', file: 'Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs', expect: 'BeginRun() ~L181 stores StartCoroutine(OpeningKitDraftSequence()) in a private Coroutine openingDraftSequence field; StopClearSequences() ~L1737 stops+nulls it; sequence nulls itself on completion.' },
]

const SCHEMA = {
  type: 'object',
  required: ['fixId', 'applied', 'matchesSpec', 'surgical', 'regressionRisk', 'verdict', 'evidence'],
  properties: {
    fixId: { type: 'string' },
    applied: { type: 'boolean', description: 'is the intended change present in current code' },
    matchesSpec: { type: 'boolean', description: 'does the present change match the spec exactly (placement, guard logic, unsubscribe symmetry)' },
    surgical: { type: 'boolean', description: 'diff for this file contains ONLY this fix-related change, no unrelated refactor' },
    regressionRisk: { type: 'string', enum: ['none', 'low', 'medium', 'high'] },
    verdict: { type: 'string', enum: ['PASS', 'FAIL', 'BLOCKED'] },
    evidence: { type: 'string', description: 'brief file:line + exactly what was found; cite the diff' },
  },
}

phase('Verify')
const verdicts = await parallel(FIXES.map(fx => () =>
  agent(
    `You are an independent adversarial verifier for ONE demo batch-fix. Default to skepticism: only PASS if you can SEE the correct change in current code.\n` +
    `Fix: ${fx.id}\nFile: ${fx.file}\nExpected change: ${fx.expect}\n\n` +
    `Method: (1) Read the relevant region of ${fx.file}. (2) Run: git -C "F:/Antigravity Projeler/2d roguelite/RIMA" diff -- ${fx.file}  to see the actual working-tree change. (3) Also read STAGING/_process/2026-06/laureth_handoff/BATCHFIX_RESULT.md for the executor's claim about ${fx.id}.\n` +
    `Judge: is the change PRESENT, does it MATCH SPEC exactly (guard placement at method top, unsubscribe symmetry, null-guards), is it SURGICAL (no unrelated edits), and what is the golden-path REGRESSION risk? ` +
    `If the change is absent because of a documented dependency (e.g. FIX-5 overlay else-block missing), verdict=BLOCKED not FAIL. If present but wrong/risky, verdict=FAIL. Return the structured verdict only.`,
    { label: `verify:${fx.id}`, phase: 'Verify', agentType: 'auditor-opus', schema: SCHEMA }
  )
))

phase('Synthesize')
const clean = verdicts.filter(Boolean)
const fails = clean.filter(v => v.verdict === 'FAIL')
const blocked = clean.filter(v => v.verdict === 'BLOCKED')
const risky = clean.filter(v => v.regressionRisk === 'high' || v.regressionRisk === 'medium')
log(`Verify done: ${clean.length}/${FIXES.length} judged | PASS=${clean.filter(v=>v.verdict==='PASS').length} FAIL=${fails.length} BLOCKED=${blocked.length} | risk(med+high)=${risky.length}`)

return {
  total: FIXES.length,
  judged: clean.length,
  pass: clean.filter(v => v.verdict === 'PASS').map(v => v.fixId),
  fail: fails.map(v => ({ fixId: v.fixId, why: v.evidence })),
  blocked: blocked.map(v => ({ fixId: v.fixId, why: v.evidence })),
  risky: risky.map(v => ({ fixId: v.fixId, risk: v.regressionRisk, why: v.evidence })),
  verdicts: clean,
}
