---
name: codex-vs-opus-split
description: "Don't auto-route everything to Codex. Use Opus / Sonnet (self or sub-agent) for design judgment, UI/UX refactor, shader design, content tuning. Reserve Codex for mechanical implementation batches."
metadata: 
  node_type: memory
  type: feedback
  originSessionId: 6463930c-ee28-4abe-b2e6-2c17db7c8cd5
---

# Codex vs Opus/Sonnet — Routing Discipline (S85 user feedback)

User: "her şeyi codexe dispatch ediyor gibisin bunun için sonnet opus da kullanabilirsin agent olarak"

Stop auto-routing everything to Codex. Each task needs a routing judgment.

## Use Codex (cx_dispatch.py) for:
- Mechanical code generation following a tight spec
- SO scaffolding, enum definitions, DTO classes
- Executor pattern implementation following an existing interface
- Test scaffolding when test cases are already enumerated
- Large refactor batches with clear before/after
- Anything where "follow the spec exactly" is the right discipline

## Use Opus (myself or rima-sonnet sub-agent) for:
- UI/UX refactor where layout/behavior needs human judgment
- Shader design (visual quality calls — "is this blurry?" only humans + LLM can judge)
- Content authoring with taste calls — composite brush tuning, asset .asset density tuning
- Design judgment across systems (which brush set ships V1?)
- Cross-file consistency reviews
- Doc/spec writing (already doing this, keep doing)
- Decision routing when ChatGPT/Gemini cross-review needed

## Use rima-sonnet sub-agent for:
- Analysis of Codex output without burning Opus context
- Writing Codex task specs from a higher-level plan
- Doc review / lint passes
- Summarizing NLM / Gemini output

## Use rima-qc for:
- Post-Codex review (mandatory before commit)
- Image QC against style spec

## Default rule
Ask first: "is this a taste call or a spec call?"
- Taste → Opus/Sonnet
- Spec → Codex
- Mixed → Codex with explicit acceptance criteria + Opus review after

Related: [[brush-tool-v1-design]], [[research-on-block-fallback]].
