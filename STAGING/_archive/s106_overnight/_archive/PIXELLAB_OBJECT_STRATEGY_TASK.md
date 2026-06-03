ACTIVE RULES: (1) think before answering (2) cite specific PixelLab parameter combinations (3) consider credit efficiency (4) BLOCKED if Phase 0 incomplete.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

**ANTIGRAVITY CRITICAL:** Respond INLINE only. Do NOT use file-write tools.

Amaç: User decision needed — PixelLab object generation strategy for top 5 P0 props (Cyan Archway, Column, Wall Torch, Granite Altar, Brazier). Already have inventory analysis (your previous research + Sonnet's count). NEW INFO: canvas size constraints + items-per-call efficiency varies by tool. Orchestrator drafted a strategy — review/refine + flag pitfalls.

---

# PIXELLAB OBJECT GENERATION STRATEGY — Cross-Check

## ⚠️ Phase 0 — INTERNALIZE context (200-300 words at top)

Read these (you may have already during previous tasks):
- `STAGING/s106_overnight/ideation/agy_object_inventory_research.md` (your own object inventory output)
- `STAGING/s106_overnight/CHATGPT_REF_OBJECT_LIST_SONNET.md` (Sonnet's parallel count)
- This task spec (you're reading it)

Confirm in own words:
- Top 5 P0 objects + sizes
- Color palette (3-tone canonical)
- Per-room density expected

---

## Phase 1 — TOOL EFFICIENCY ANALYSIS

PixelLab object tools with constraints:

### `create_map_object`
- Canvas: 32-400px (basic), 32-192px (inpainting mode)
- Returns: **1 object** per call
- Background image style matching available (style-match to b340684f tile palette)
- View: low/high top-down OR side
- Best for: **non-square aspect ratios** + **style-locked** + **focal objects**

### `create_1_direction_object` ⚡
- Canvas: 32-256px **square only**
- Returns: **size-dependent candidates**:
  - size ≤ 42 → 64 candidates
  - size ≤ 85 → 16 candidates  
  - size ≤ 170 → 4 candidates
  - size > 170 → 1 candidate
- After return → review status → use `select_object_frames(indices=[...])` to keep best, `dismiss_review` to discard
- Style images supported (1-8 depending on size)
- View: top-down OR sidescroller (NO oblique/iso!)
- Best for: **multi-candidate efficiency** at small/medium sizes

### `create_8_direction_object`
- Canvas: 32-256px square
- Returns: 1 object × 8 directions (~2-4 min)
- View: low/high top-down OR side
- Best for: **rotatable items** (throwables, characters, multi-angle props)

### `create_isometric_tile`
- Canvas: 16-64px
- Returns: 1 iso tile (block/thick/thin variants)
- Best for: floor/ground tiles
- NOT for props (already used for floor in b340684f set)

## Phase 2 — ORCHESTRATOR'S DRAFT STRATEGY (review this)

| Object | Size | Tool | Items/call | Cost |
|---|---|---|---|---|
| Cyan Archway | 96×128 (non-square) | `create_map_object` w/ b340684f bg | 1 | 1 |
| Monolithic Column | 64×128 (non-square — square workaround: 128×128) | `create_1_direction_object` size=128 | 4 | 1 |
| Wall Torch sconce | 32×64 (non-square — workaround size=64) | `create_1_direction_object` size=64 | 16 | 1 |
| Granite Altar | 96×64 (non-square — workaround size=96 or use map_object) | `create_1_direction_object` size=96 | 4 | 1 |
| Freestanding Brazier | 32×64 (size=64) | `create_1_direction_object` size=64 | 16 | 1 |

**Total: 5 credits → 41 candidates (1 + 4 + 16 + 4 + 16)**

### Questions to validate or contest:
1. **Aspect ratio problem:** `create_1_direction_object` is SQUARE-only. Column needs 64×128 vertical. Padding to 128×128 wastes pixels. Better to use `create_map_object` for non-square objects? Or accept 128×128 square and crop manually post-gen?
2. **Style matching:** `create_1_direction_object` uses style_images (max 8 at size≤85). Should we pass 4 b340684f floor tiles as style refs to lock palette?
3. **Aspect handling for 96×64 altar:** create_map_object with inpainting bg or create_1_dir at size=96 with later crop?
4. **Multi-candidate selection:** Once 16 brazier candidates returned, who picks the best 4-6? Antigravity vision review? Opus self-check? Both?

## Phase 3 — YOUR RECOMMENDED ALTERNATIVE (or confirm orchestrator's)

Either:
- (A) Confirm orchestrator's draft + flag pitfalls only
- (B) Propose different tool assignment per object + justify

Be SPECIFIC — exact tool, exact size, exact style_images approach, exact view.

## Phase 4 — Multi-candidate SELECTION criteria

After generation returns 16+4+4+16+1=41 candidates, what's the selection rubric?

For wall torch (16 candidates):
- Keep 4-6 best per criteria: ...
- Reject criteria: ...

For column (4 candidates): keep 2-3 best per: ...

Aim for ~12-16 final objects across 5 P0 (subset of 41 candidates).

## Phase 5 — SOCKET-PAINT vs PRE-GEN architecture decision

**Current state:** RoomPainterWindow.cs Stream D added PropSocket brush. User paints socket cell, Generate spawns prefab at socket world position.

**Question:** After we generate 12-16 final P0 objects, how does the painter use them?
- (A) **Manual prefab assignment per socket type** in painter UI (TorchSocket → wall_torch.prefab dropdown)
- (B) **Auto-selection from registry** based on socket subtype (TorchSocket → registry.GetProp(SocketType.Torch).random())
- (C) **Replace socket system with direct object brush** (paint a torch cell at exact position, ignore socket abstraction)

Recommend ONE + justify (consider workflow ergonomics + flexibility).

---

## Output format (INLINE ~800-1200 words)

```markdown
# PixelLab Object Strategy Cross-Check — <Codex profile / Antigravity account> — 2026-05-25

## Phase 0 — Context internalize
<200-300 words>

## Phase 1 — Tool efficiency confirmation
<your read of the constraints, any error in my draft?>

## Phase 2 — Confirm/Contest orchestrator's draft
<table with corrections + reasoning>

## Phase 3 — Your recommended strategy
<final tool assignment per object>

## Phase 4 — Multi-candidate selection criteria
<rubric>

## Phase 5 — Socket-paint vs Pre-gen architecture
<A/B/C verdict + reason>

## Pitfalls flagged
1. ...
2. ...
```

## Constraints
- 800-1200 words total
- Specific parameter values (size, view, outline, shading)
- Credit-conscious (1389 generations remaining for the month)
- Aspect ratio + square-only tools is a key tension

## Estimated time: 15-25 min
