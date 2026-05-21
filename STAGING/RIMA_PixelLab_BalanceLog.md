# RIMA PixelLab Balance Log

> **Owner:** Orchestrator (Sonnet). Per-batch append after each Codex/USER dispatch.
> **Memory ref:** `reference_pixellab_v3_budget_formula`, `5000_pixellab_allocation_lock`

## Mevcut Balance

**Last check (2026-05-20 S95 LATE NIGHT):** 2,433 / 5,000 gen

`mcp__pixellab__get_balance` — orchestrator her büyük batch öncesi ve sonrası check.

## Per-Batch Tracking

| Batch | Date | Tool | Reserve | Real | Δ | Notes |
|---|---|---|---|---|---|---|
| _Initial state_ | 2026-05-20 | — | — | — | — | 2433/5000 baseline (S95 LATE NIGHT) |
| Pilot A 1.1 Wall Face | 2026-05-20 | create_object 128 n=4 view=side | 25-40 | **20** | **-5 to -20** | object_id `54c88cfe-bf99-4d22-9964-65eb236380e6`. Pilot A test: numbered description fallback ÇALIŞTI (3/4 piece doğru). Finalized: aa49fd5c (face_EW), 0a36c905 (corner), 7daff11c (arch). Frame 0 (face_NS flat drift) dismissed. tag: act1_wall_pilot_a_s95. |

## Faz-Bazlı Subtotal

| Faz | Reserve avg | Real (running) | Status |
|---|---|---|---|
| Faz 1 Demo MVP | 280 | 20 | Pilot A firing (Batch 1.1, ~30-90s processing) |
| Faz 2.1 Warblade | 544 | 0 | Awaiting Pilot B |
| Faz 2.2 Mob | 200 | 0 | Defer until Pilot B PASS |
| Faz 3 VFX | 150 | 0 | Awaiting Faz 1.5 anchor |
| Faz 4 UI | 90 | 0 | Last priority |
| **Total** | **1264** | **0** | **0% spent** |

## Pilot Gate Status

- **Pilot A** (Batch 1.1 Wall Face Pack, 40 gen reserve): ✅ **PASS** — real cost 20 gen, 3/4 piece finalized (aa49fd5c face_EW, 0a36c905 corner, 7daff11c arch). Frame 0 face_NS drift → Batch 1.1b consolidate (face_NS + 3 damaged variant).
- **Pilot B** (Warblade 3-state V3, ~96 gen): NOT FIRED — Batch 1.1b ve UIUX smoke test sonrası prompt formülü hazırlanacak.

## Real Cost Empirical Refinement

Per-batch real cost → formula refinement:
- Modeled state base avg: 15-20 gen
- Modeled animasyon: 10-15 gen
- Pilot B sonrası refined: TBD

## Cross-Reference

- Plan: `STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md`
- Master spec: `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md` (Karar #9 bütçe)
- Memory: `reference_pixellab_v3_budget_formula`

## Antigravity Review Verdict (2026-05-20)

- 8 artifact reviewed, 2 minor issues, 0 blocker
- Final: PROCEED TO PILOT A & B
- Düzeltmeler: Panel 5 EditMode.ChangeEditMode lock + v1 Floor→Ground rename
- Açık user kararı: UIUX Q2 (group lock UI), Q10 (Selected Instance scope)
- Pilot ready: Batch 1.1 (40 gen MCP) + Warblade 3-state (~96 gen V3 USER)

