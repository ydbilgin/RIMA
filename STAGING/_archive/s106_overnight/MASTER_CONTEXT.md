# RIMA S106 OVERNIGHT — MASTER CONTEXT

> **For: any Codex / Antigravity / sub-agent dispatched tonight.**
> This file is your SINGLE SOURCE OF TRUTH. Read it fully before starting. Do NOT autonomously read other project files unless the task explicitly requires it.

**Session:** S106 overnight (2026-05-25 ~02:55 → ~08:30)
**Orchestrator:** Opus (Claude Code), autonomous mode — user is sleeping, no questions allowed back
**Goal:** Logic-first room builder pipeline producing chatgpt_ref-quality ARPG dungeon rooms, with world's-easiest blueprint painter tool

---

## 🎯 Mission (verbatim from user, 2026-05-25 02:50)

> RIMA için Unity'de logic-first room builder + visual dressing sistemini kuruyoruz.
>
> **Amacım:** Elimdeki mevcut modular wall asset packleriyle, referanslardaki gibi büyük top-down / fake-isometric ARPG dungeon odaları oluşturmak istiyorum. Bu sistem sprite placement olmayacak. Önce oda logic'i kurulacak, sonra mevcut asset pack görselleri bu logic'in üzerine visual dressing olarak yerleşecek.
>
> **Referans oda hedefleri:** geniş combat room, diamond/stepped-diamond arena, ritual/boss room, flooded crypt room, library/prison room, açık ön cepheli oda, arka kapılı/portal setpiece oda.
>
> **Önemli:** Yeni asset üretimi istemiyorum. Önce elimizdeki assetlerle sistemin çalışıp çalışmadığını değerlendir. Eksik asset varsa listele ama sistemi placeholder ile yürüt.
>
> **Sabaha kadar amaç:**
> 1. Mevcut assetleri WallPieceData mantığıyla gruplandırmak
> 2. Prefab standardının doğru olup olmadığını kontrol etmek
> 3. Placeholder veya mevcut assetlerle 3-5 test oda üretmek
> 4. Debug gizmos ile logic'i kanıtlamak
> 5. Eksikleri net raporlamak
> 6. Bu sistemle referanslardaki büyük ARPG odalarının yapılabilirliğini değerlendirmek
>
> Bana genel "yaptım" deme. Kanıtla: screenshot, gizmo, metadata, kullanılan assetler, eksikler, hatalar, sonraki düzeltmeler şeklinde rapor ver.
>
> **Tool kullanıcı dostluğu:** bir insan için dünyanın en kolay kullanımı olan toolunu yap, ben oraya blueprint çizeyim otomatik duvarları entegre etsin, saçma sapan etmesin birbirine bağlayacak şekilde, 2D box'ları ayarlı şekilde olacak.

---

## 🏗️ Architecture — Logic-First 4-Layer System

### Layer 1: Logic
- room footprint
- walkable area
- blocked wall footprint
- collider bounds
- door / entrance / exit sockets
- enemy spawn points, prop sockets, objective sockets

### Layer 2: Wall Chain
- rear wall chain (dominant visual hat)
- side wall chains (stepped, not true diagonal)
- front low/open edge
- corner / connector nodes
- door / arch nodes
- alcove / protrusion nodes
- seam / cleanup nodes

### Layer 3: Visual Dressing
- mevcut wall asset sprite'ları (just visuals, plugged into Layer 2 sockets)
- rear wall setpiece
- connector columns, corner pieces, door/arch pieces, low front, seam overlays

### Layer 4: Decoration / VFX
- torch, banner, brazier, cage, bookshelf, altar, rift crystal
- cyan crack decals, fog/glow/water VFX

**Kritik prensip:** Collider ve walkable logic, görsel sprite'lardan BAĞIMSIZ olmalı. Görsel duvarın tamamı collider olmayacak. Collider sadece blocked footprint / duvar taban izi üzerinden kurulacak.

---

## 📦 Asset Pack — Current State (Inventory)

### Wall Sheets
| Sheet | Pieces | Status | Path |
|---|---|---|---|
| sheet_1 | 8 | ✅ **Typed** (straight / outer_corner / inner_corner / end / door_l / door_r / alcove / protrusion) | `Assets/Sprites/AssetPackV3/walls/sheet_1/` |
| sheet_2 | 9 | ⏳ Untyped, named piece_01..09 | `Assets/Sprites/AssetPackV3/walls/sheet_2/` |
| sheet_3 | 11 | ⏳ Cell-named (cell_01_R0C0 ... cell_08_R3C1_R) — looks like a 4×2 grid extraction | `Assets/Sprites/AssetPackV3/walls/sheet_3/` |
| sheet_4 | 16 | ⏳ Untyped, named piece_01..16 (likely 4×4 master sheet from S102/S104) | `Assets/Sprites/AssetPackV3/walls/sheet_4/` |

### Floor
- 16 tile_*.png pieces in `Assets/Sprites/AssetPackV3/floor/`

### Placeholders (already prefabbed, 14 total)
- `wp_rear_wall_{1,2,3}x.prefab`, `wp_side_wall_{1,2,3}x.prefab`
- `wp_connector.prefab`, `wp_outer_corner.prefab`, `wp_inner_corner.prefab`
- `wp_door_arch.prefab`
- `wp_low_front_{1,2}x.prefab`, `wp_open_gap.prefab`, `wp_seam.prefab`
- Path: `Assets/Prefabs/Environment/Walls/Placeholders/`

### Asset Groups (per blueprint_room ADIM 1 — to be filled tonight)
- **A. Connector / Column Pieces** — straight connector, tall column, end column, support, decorative, pillar-like
- **B. Rear Wall Pieces** — uzun düz, kısa düz, yüksek wall span, door-area pieces
- **C. Side Wall / Step Pieces** — yan duvar, kısa span, L dönüş, kademeli zincir, placeholder yedek
- **D. Corner / Turn Pieces** — outer/inner corner, L-shaped, alcove dönüş
- **E. Door / Arch / Portal Pieces** — door arch, gate, open passage, portal frame, boss gate
- **F. Low Front / Open Edge Pieces** — low wall, broken low wall, parapet, open gap, front filler
- **G. Seam / Cleanup / Filler** — rubble, small block, shadow strip, seam patch, gap filler
- **H. Props / Decoration** — torch, banner, chain, bookcase, brazier, crate, sarcophagus, altar, crystal, cage

### Asset Metadata Per Piece (target schema)
- id, source sheet, sprite rect, asset group, type
- direction (rear / side_left / side_right / front / any)
- lengthInCells (1, 2, 3, 4, setpiece)
- footprintSize, visualOffset, colliderFootprint
- connectLeft, connectRight
- heightType (low, normal, tall, setpiece)
- prefabRef, notes/known-issues

---

## 🧱 Prefab Standard (target hierarchy)

Each wall piece prefab MUST be:

```
WallPiece_Root (Transform at footprint anchor, NOT sprite center)
├── FootprintAnchor
├── Visual (SpriteRenderer, offset from root)
├── Collider2D (gameplay footprint, NOT sprite full extent)
├── Socket_Left, Socket_Right, Socket_Back, Socket_Front
├── SeamSocket_Left, SeamSocket_Right
└── PropSocket_1, PropSocket_2
```

**Rules:**
- Root pozisyonu sprite merkezinde DEĞİL, footprint anchor'da olmalı
- Visual child olarak offsetlenmeli
- Collider sprite'ın tamamına değil, gameplay footprint'e göre ayarlanmalı
- Sortyıng pivot bottom / footprint anchor mantığında olmalı
- Sprite bounding box'a göre hizalama yapılmamalı

---

## 🔄 Room Builder Flow (12 steps, blueprint_room ADIM 4-5 pattern)

1. **Room preset seç** (BasicCombat / SteppedDiamond / Ritual / FloodedCrypt / LibraryAlcove / BossArena)
2. **Room footprint üret** (rectangle / diamond-ish / stepped diamond / irregular / flooded / alcove)
3. **Walkable area üret** (oyuncu+düşman gezi alanı, geniş+temiz combat alanı korunmalı)
4. **Blocked footprint üret** (rear/side/front/prop)
5. **Rear wall chain kur** — örnek: `Connector + RearWall_3x + DoorArch + RearWall_3x + Connector`
6. **Side wall chains kur** — stepped, 1-2 cell aralıklarla içe/dışa, diamond hissi
7. **Front edge kur** — ÖN YÜKSEK DUVAR YOK, low front / open gap / rubble / shadow
8. **Corner / connector yerleştir** — tüm dönüşlerde
9. **Door / arch / portal yerleştir** — rear center veya rear side; boss/ritual büyük setpiece
10. **Seam / cleanup overlay** — birleşim yerlerine seam, rubble/filler, base shadow
11. **Prop sockets üret** — torch, banner, enemy spawn, chest/objective, altar, rift VFX
12. **Debug gizmos üret** — green walkable, red blocked, yellow wall chain, purple door, blue sockets, cyan low front, orange connector/corner

---

## 💎 Stepped Diamond Rule

Referanslardaki diamond görünüm için gerçek diagonal wall sprite ŞART DEĞİL. Kare grid üzerinde stepped wall chain:
- rear wall daha dar veya baskın merkezli
- side chains aşağı doğru 1-2 cell kademelerle genişler
- front edge açık veya low wall
- görsel olarak diamond / rhombus hissi verir

Örnek ölçü: rear 8-10, middle 12-14, front opening 5-8, side step 2-3 cell'de 1 cell.

---

## 🧪 Required Mini Test Scenarios (6 tests)

| # | Test | Components | Success criteria |
|---|---|---|---|
| 1 | Basic wall chain | Connector + RearWall_2x + Connector | Hizalı, sockets uçlarda, collider sadece tabanda |
| 2 | Corner turn | RearWall + OuterCorner + SideWall | Dönüş doğru, corner seam gizliyor, chain kırılmıyor |
| 3 | Low front edge | LowFront_2x + OpenGap + LowFront_2x | Ön kenar yüksek değil, giriş açık, collider boşluğu kapatmıyor |
| 4 | Door setup | DoorColumnL + DoorArch + DoorColumnR | Kapı rear chain'e oturuyor, door socket doğru, kapı açıklığı open |
| 5 | Stepped diamond room | rear+stepped sides+low front+walkable center | Diamond hissi, walkable temiz, blocked footprint doğru |
| 6 | Visual replacement | Placeholder Visual child → real asset sprite | Algoritma değişmiyor, sadece sprite değişiyor |

---

## 🏛️ Required 5 Test Rooms

| # | Room | Spec |
|---|---|---|
| 1 | **Basic Combat** | Geniş boş merkez, rear yüksek, side kısa/kademeli, front low/open, 4 enemy spawn, 2 prop socket |
| 2 | **Ritual Diamond** | Stepped diamond footprint, rear+portal/arch, merkez ritual socket, simetrik enemy spawn, cyan rift VFX sockets |
| 3 | **Flooded Crypt** | Walkable platformlar, su alanları reserved/non-walkable, side stepped, sarcophagus/altar sockets |
| 4 | **Library / Alcove** | Rear bookshelf prop sockets, 1-2 yan alcove, merkez walkable, desk/table sockets |
| 5 | **Large Boss Arena** | Geniş walkable, büyük rear setpiece, stepped sides, front mostly open, boss center, player front entrance, wave side sockets |

### Per-Room Deliverable (ZORUNLU)
1. Scene screenshot
2. Debug gizmo screenshot
3. Used asset list
4. Missing asset list
5. Metadata issues
6. Collider issues
7. Sorting issues
8. Pivot/anchor issues
9. "Can this create chatgpt_ref style rooms?" değerlendirmesi
10. Next actions

---

## 💻 Current V2 Code Map (already on disk, NOT to be wholesale rewritten)

### Runtime — `Assets/Scripts/Runtime/Walls/V2/`
- `WallPieceEnums.cs` — enums (RoomShapeType, FrontEdgeMode, WallDirection, etc.)
- `WallPieceData.cs` — ScriptableObject schema for each wall piece
- `WallPieceRegistry.cs` — central registry of all pieces (asset at `Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset`)
- `WallPiece.cs` — runtime MonoBehaviour on prefabs
- `RoomSpec.cs` — serializable room config (size, shape, alcoves, niches, presets, walkable cells)
- `WallChainRoomBuilder.cs` — **CORE algorithm**, S105 P0 fixed (7 bugs + 1 P1), no compile errors

### Editor — `Assets/Scripts/Editor/Walls/V2/`
- `RoomPainterWindow.cs` — **USER-FACING paint UI** (target of Stream D polish). Grid-based, BrushMode {Walkable, Erase, Door, Alcove, Protrusion}. Has 22-cell default, save/load, preset support, water pools, interior islands. NEEDS UX OVERHAUL.
- `RoomBuilderTestRunner.cs` — test scenario runner

### Already Fixed in S105 (Codex P0 wave, 0 console error)
1. Corner 3-prefab overlap → rear/side endpoint exclusion
2. Diamond yamuk → expand-then-shrink two-stage
3. 1-cell corridor fake corner → opposite-open exclusion
4. Length 1/2 wall connector overlap → bypass
5. Niche/protrusion depth floating → corner offset by depth
6. Corner direction (Any) → derive from open sides
7. FillFrontWithLow cursor sabit → cursor += d.lengthInCells
8. (P1) Painter alcove brush → nicheSpecs mapping

---

## 🖼️ Visual References (chatgpt_ref folder) — ⭐ CRITICAL TARGET

> **User direct instruction (2026-05-25 03:00):** "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\chatgpt_ref\blueprint_room, F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\concepts\chatgpt_ref bu iki dosyayı unutma sakın bunlara benzer odalar çıkacak (objeler hariç)"
>
> **Translation:** These two folders are THE visual target. Rooms similar to these — **excluding objects** (props can differ, but room shape/wall composition/diamond layout/footprint hierarchy MUST match).

Path: `STAGING/concepts/chatgpt_ref/`

### blueprint_room/ — METHODOLOGY MANUAL (5 PNG, the bible for tonight)
1. **ADIM 1 — Asset Pack'i Gruplandır** — labels 6 asset groups (1 Connector/Column, 2 Rear Wall, 3 Corner/Turn, 4 Door/Arch, 5 Low Front Edge, 6 Seam/Cleanup/Filler) with example sprites + usage tips
2. **ADIM 4 — Library/Alcove Room** — full 7-step construction of an 11×11 room: footprint → rear chain → side chains → alcove → open front → interior props → final shell
3. **ADIM 5 — Open Front / Flooded Room** — 6-step construction with water reserved zones
4. (Other ADIM images cover Combat/Ritual/Boss patterns — view all 5)

### chatgpt_ref/ root + new_chatgpt/ — VISUAL TARGETS
- ChatGPT Image 25 May 2026 00_18_45 (1-4).png — **Hades/CoM-style ARPG arena** ground truth. Pixel-art 3/4 top-down, diamond layouts, cyan rift VFX, torch glow, ornate setpieces.

---

## 🔒 HARD RULES (NO EXCEPTIONS, every agent must obey)

1. ❌ **NO Unity crash** — All AssetDatabase ops MUST be wrapped in `AssetDatabase.StartAssetEditing()` / `StopAssetEditing()`. Save scene before destructive ops. No domain reload mid-batch. No infinite editor loops.
2. ❌ **NO PixelLab gen** overnight (rule 1, gece autonomous yasak)
3. ❌ **NO Karar #152/#153/#154 progress** without user approval — flag and continue
4. ❌ **NO new asset generation suggestions** without first testing with existing + placeholder for at least 3 rooms (user explicit)
5. ❌ **NO direct `& $agy`** in PowerShell — use `agy_dispatch.py` only
6. ✅ **Logic-first** — algorithm changes before sprite swaps (rule 10)
7. ✅ **Every dispatch prompt has:** ACTIVE RULES line + Amaç line + NLM ACCESS hint + path to THIS file
8. ✅ **"Verifiable proof" only** — screenshots, gizmos, file paths, line numbers. No "yaptım" claims.
9. ✅ **Multi-AI cross-validation** before any P0 — Codex code + Antigravity deep + Opus design
10. ✅ **After every completed task** — update `CURRENT_STATUS.md` + add memory entry + append `SESSION_LOG.md`

---

## 🔄 Multi-Agent Review Loop (THE pattern)

Per task (whether implementer = Codex / Antigravity / Opus):

```
ROUND 1 — IMPLEMENT
  Implementer does work, writes verifiable artifact (file, screenshot, report)
  Claims "DONE" with proof

ROUND 2 — PARALLEL MULTI-AI REVIEW
  Reviewer A (different AI) reads artifact + spec → PASS / FAIL / REWORK + notes
  Reviewer B (third AI) does same independently → PASS / FAIL / REWORK + notes
  (Reviewer C only on critical tasks: design / painter / 5-test-rooms)

ROUND 3 — RECONCILE (Opus = orchestrator)
  All PASS → DONE, advance to next task
  Disagreement → orchestrator judges, logs reasoning in SESSION_LOG
  ≥1 FAIL/REWORK → back to implementer with combined feedback
  Max 3 iteration rounds → flag BLOCKED, surface to user_morning_review
```

### Agent specialization
| Agent | Strength | Best for |
|---|---|---|
| **Codex (cx_dispatch, GPT-5.5)** | Surgical C# refactor, deterministic edits, Unity integration | Implementation, prefab generation, scriptable object creation |
| **Antigravity (agy_dispatch, Gemini 3.5 Flash High)** | Vision (can SEE PNGs), independent code review, **industry research (web search)** | Asset classification, design review, "how do AAA games solve X", second opinion |
| **Opus (orchestrator, this conversation)** | Cross-system synthesis, design judgment, conflict resolution | Plan reconciliation, multi-AI verdict aggregation, decision-making |
| **rima-sonnet (sub-agent)** | Analysis, planning, cross-ref | Lightweight reviews, prompt drafting, final-report polish |
| **rima-qc (sub-agent)** | PASS/FAIL strict review | Final gates, screenshot comparison |

### Antigravity research speciality (NEW per user 2026-05-25)
> "internetten araştırman gereken endüstride bunu nasıl çözmüşler gibi bir şeyi düşünürsen de antigravitye sor en güncel ve en iyi araştırmayı o yapıyor"

Industry/web research questions → Antigravity (it has fresh search). Examples:
- How do Hades/Diablo 4/Path of Exile/Children of Morta handle modular tile composition?
- What's the standard Unity tilemap-vs-prefab tradeoff for ARPG dungeons?
- Best practices for grid-based level editor windows
- Open-source examples of blueprint-to-asset pipelines

---

## 📂 Session Artifacts (where to read/write)

```
STAGING/s106_overnight/
├── MASTER_CONTEXT.md          ← this file (read by every agent)
├── IDEATION_TASK.md            ← prompt for ideation dispatch
├── RESEARCH_TASK.md            ← prompt for industry research (Antigravity)
├── MASTER_PLAN.md              ← created after ideation+research synthesis
├── SESSION_LOG.md              ← chronological record of every step
├── ideation/
│   ├── codex_response.md       ← Codex ideation output
│   └── agy_response.md         ← Antigravity ideation output
│   └── research_response.md    ← Antigravity research output
├── stream_b_assets/            ← taxonomy classifications, WallPieceData JSON
├── stream_c_validation/        ← code review reports
├── stream_d_painter/           ← painter tool changelog, diffs
└── stream_e_rooms/             ← 5 test room screenshots + reports

STAGING/s106_morning/
└── OVERNIGHT_DELIVERABLE.md   ← final TL;DR for user breakfast review
```

---

## 🎁 Authorized Extensions (if all 5 streams finish early)

User (2026-05-25 03:00): "geliştirmen gereken bi konu bulursanız beraber agentlara da sorarak onları da yapabilirsiniz ekstra olarak"

If Streams A-E complete with time remaining, orchestrator may dispatch additional improvements via the same multi-agent loop. Candidates to consider (NOT mandatory):
- Floor tile auto-fill from sheet_3/4 patterns
- Per-room ambient lighting presets
- Procedural prop placement using prop sockets (within hard rules — no new PixelLab gen)
- Real wall asset visual swap on a single test room as a proof-of-concept
- Doc/memory consistency sweep (run `/lint` if available)
- Codebase warning cleanup (Codex --effort medium)

**Constraint:** Extensions must still obey HARD RULES. NO new PixelLab gen. NO Karar #152/#153/#154 progress.

## 🔓 NLM Access

User (2026-05-25 03:00): "nlm login oldum kullanman gerekirse kullanırsın"

NLM CLI is authenticated. If during ideation/review an agent needs RIMA design context not in MASTER_CONTEXT.md or the file list, query NLM:
```bash
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
```

LIVE notebook ID: `30ddffa5-292f-4248-8e77-68074af901be`. Other IDs are deprecated.

## 🎓 Quick Self-Test for Sub-Agents

Before starting your task, you should be able to answer YES to all:
- [ ] Did I read this MASTER_CONTEXT.md fully?
- [ ] Do I know the 4 layers and that collider ≠ sprite extent?
- [ ] Do I know that NEW asset gen is BANNED tonight (placeholder OK)?
- [ ] Do I know to write CURRENT_STATUS + memory after my task?
- [ ] Do I know I MUST provide verifiable proof, not "yaptım"?
- [ ] Do I know Unity must NOT crash (AssetDatabase batching, scene save discipline)?

If any NO → re-read the relevant section. Then proceed.
