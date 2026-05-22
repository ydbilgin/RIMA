# Asset Production Master Plan v1 -- Codex Review

## 1. Lock integrity check

| Lock | Verdict | Notes |
|---|---|---|
| L1 Pure top-down pivot | TWEAK | Pure 2D top-down is correctly locked, but canon still distinguishes floor tiles from Hades-like wall/prop perspective. Do not imply every object is pure 90 degrees until Item 2 proves it. |
| L2 Weapon decouple arch | PASS | `weapon_pipeline_v1.md` and `weapon_web_prompts_v1.md` match: body weaponless, Child SpriteRenderer/HandAnchor, 11 weapon sprites plus Elementalist orb amendment. Master says 12 weapon sprites; current prompt file has 11 plus Brawler exception, so wording should be "11 generated weapon/focus sprites, Brawler excluded." |
| L3 No PixelLab night gen | PASS | Correctly captured: generation blocked, read-only get/list and docs/balance checks allowed. |
| L4 Hibrit variant strategy C | TWEAK | The lock is directionally correct, but the source canon is broader: Option C is hybrid authored/painted background plus gameplay overlays. The plan narrows it to Act 2-4 hero manual Edit Image Pro plus autonomous state variants. Mark that as production interpretation, not the entire lock. |
| L5 Modular tile pipeline Mod B | PASS_WITH_CONFLICT | Wang deprecation and create_tiles_pro-first direction are consistent with S98/master direction. However NLM canon still says 32x32 floors in older pivot memory, while `weapon_pipeline_v1.md` and this plan use 64x64. This must be explicitly reconciled before production. |
| L6 Wall + decoration pipeline | PASS | `create_object` for walls/props, not floor tile generation, is consistent. |
| L7 Tile size | TWEAK | False-positive if treated as globally settled. Master locks 64x64 production; NLM returned 32x32 as older canonical floor size, and S98 report does not itself prove the 64x64 lock. Needs source note: "S98 supersedes S59 32px floor lock." |
| L8 Painter default | FAIL | Code has field defaults `PaintMode.TopDown`, but `LoadPainterPrefs()` defaults missing prefs to `PaintMode.Isometric`. Master's "Painter default TopDown primary" is not fully true in live code unless prefs already exist. |

Missing lock: asset import folder/catalog structure and WeaponDatabase population scheduling.

## 2. PixelLab parameter table cross-check

`create_tiles_pro` values are mostly correct for the pure-top-down floor intent: `tile_type=square_topdown`, `tile_view=top-down`, `tile_view_angle=90`, `tile_depth_ratio=0`, and `outline_mode=segmentation` match the MCP doc and prior review. Missing params: explicit `tile_size`, `seed` policy, and `style_images=PENDING_DECISION/PENDING_TEST` because style references change cost and consistency.

`create_object view=PENDING_TEST` is correct. MCP docs expose `low top-down`/`high top-down`, not a true 90-degree object camera, so the workaround prompt test is necessary. `object_view=top-down` appears in older v2 docs, but current MCP tools expose `create_1_direction_object`, `create_8_direction_object`, and `create_map_object` with limited view choices. Keep this test-gated.

## 3. Pricing table accuracy

| Item | Verdict |
|---|---|
| create_object 20/30/40 by size | TWEAK: current MCP descriptions say 20-40 generations depending on resulting image size, not a documented 20/30/40 step table. The table is plausible but should be estimate, not canon. |
| create_object_state 20-40 | PASS. Tool docs match. |
| create_tiles_pro 25 for 64px | PASS for 64px square without style references. Docs say 20 or 25 by size/type; style refs can be 20-40. |
| Edit Image Pro 20/25/40 | PASS. Docs confirm 1-256 = 20, 257-314 = 25, 315-512 = 40. |
| Inpaint v3 20 flat | PASS. Docs confirm 20 per use. |

Balance check succeeded: 2265 / 5000 generations, subscription active.

## 4. Asset taxonomy completeness vs Act 1 layout

Unique layout IDs:

| Source | IDs |
|---|---|
| walls[] | `archway_n`, `column_broken`, `column_hero`, `shrine_pedestal`, `throne_dais` |
| props[] | `banner_tattered`, `rubble_pile_large`, `rubble_pile_small`, `skull_pile`, `torch_wall` |

Section 4's prefab summary is inaccurate: it lists `shrine_pedestal` and `throne_dais` under props, but the JSON uses them in `walls[]`. Items 4 and 5 do cover all required IDs except the plan summary omits `shrine_pedestal`/`throne_dais` from walls[]. Batch composition includes all current layout IDs: `torch_wall`, `banner_tattered`, small/large rubble, skull pile, columns, archway, shrine, throne. No typos found in actual batch entries.

Gaps remain for `rewards.chests[]`, key-required door/gate socket, pickups/key shard/relic, and hazard/dash telegraph. These are acknowledged but not first-class decision items.

## 5. Sequential decision list integrity

Dependencies are mostly correct: Item 2 blocks all object batches; Item 7 depends on Item 1; Item 8 depends on Item 5; Item 9 depends on Items 3 and 8. Item 10 should not be `PENDING_LOCK` while Items 1-9 are undecided because its order may change after the Item 2 test and batch risk verdicts.

Missing items: floor tile size reconciliation, style reference policy, asset folder/catalog lock, chest/reward/gate/hazard scheduling, and a mixed-batch palette test. Items 3/4/5 can merge after Item 2, but separate locks are better because each size bucket has different risk and cost.

## 6. Batch fill rule risk

The cross-act fill risk is real. PixelLab often normalizes a batch toward one shared palette/style when multiple biomes appear in one prompt. Filling unused slots saves at most one future call per size class, but can damage Act-specific readability. Mitigation: same-act-only for vertical slice, or one low-stakes mixed 32px test before mixed 48-80 or 88-168 batches. Empty slots are cheaper than hero-prop rework.

## 7. Budget reality check

Current balance: 2265. Realistic Act 1-only range:

| Phase | Calls | Estimate |
|---|---:|---:|
| Floor create_tiles_pro | 1-4 | 25-100 |
| Object view test | 1-2 | 20-80 |
| Wall prototype | 1 | 20-40 |
| Full walls | 5 | 100-200 |
| Medium props | 1-2 | 30-80 |
| Small props/decals | 1-2 | 20-80 |
| Hero props | 3 | 60-120 |
| Act 1 realistic total | 13-19 | 275-700 |

The plan's 620-640 is plausible but should not call 32px 64 items "1 create_object call" as proven canon; current MCP review candidates/capacity behavior should be test-confirmed. Hibrit Act 2-4 manual additions depend on selected anchors: 5-10 hero edits per act at 20-40 each is 300-1200 generations if repeated across three acts, plus autonomous state variants.

## 8. Recommendation -- first unlock priority

First lock should be Item 2: `create_object` view parameter test. Item 1 floor choices matter, but every screenshot improvement beyond floor depends on object camera correctness: walls, columns, torches, banners, rubble, shrine, throne, skull pile, and hero props. It is also the highest unknown risk because PixelLab object tools do not expose pure 90-degree overhead. A 1-2 prop test with `rubble_pile_small` and `torch_wall` gives immediate evidence and unblocks Items 3/4/5/6.

## 9. Final verdict -- 3-sentence executive

Verdict: TWEAK, not ready as-is, because L7 tile size and L8 painter default have source/code conflicts and mixed-batch policy needs a test gate. Push Item 2 first: lock create_object view through a small prop test, then decide object batch compositions. If locks proceed sequentially and PixelLab tests pass, vertical-slice asset uplift can ship in roughly 1-2 focused production sessions; full Act 1 polish plus Act 2-4 variant planning needs additional review.
