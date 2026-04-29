# CODEX BRAIN (Persistent)

## Purpose
This file is Codex's persistent operational memory for this project.
Read this file first on every new user request.

## Protection Rule
- CODEX.md is protected and must NOT be deleted.
- If missing, recreate immediately and restore these rules.

## Priority Order
1. User's latest explicit command
2. Rules in CODEX.md
3. Active task content in CODEX_TASKS.md

## Core Workflow Rules
1. If a task is executed from CODEX_TASKS.md:
   - Write final task output to CODEX_DONE.md.
   - Clear CODEX_TASKS.md content after completion (keep file).
   - Tell user exactly: CODEX_DONE'a yazdim.
2. CODEX_DONE write mode:
   - Default: overwrite with only the current completed task report.
   - If user explicitly asks append for that turn: append as requested.
3. Stay strictly within task scope.
4. Do not edit unrelated files unless user explicitly requests.

## MCP Connection Memory (Permanent)
- Do NOT call any MCP tool unless the current task truly requires it (Unity MCP, Canva, etc.).
- Avoid "just in case" MCP calls; they add latency and can increase token burn.
- Default: solve via local files + shell. Escalate to MCP only when needed (e.g., Unity scene inspection, automated import/wiring).

## Codex Capability Memory for RIMA (Permanent)
- RIMA'da asıl ihtiyaç yeni Unity paketi/araç değil; asset QC, import disiplini, local script doğrulaması, test okuma/yazma, denge analizi ve playtest loop doğrulaması.
- `$imagegen` skill:
  - Use for raster outputs: concept art, sprite/spritesheet exploration, VFX sheet ideation, UI mockup, transparent-background cutout, visual variants.
  - Default path is built-in image generation. CLI fallback only if user explicitly asks or confirms true native transparency fallback.
  - Project-bound output must be copied/moved into the workspace; never leave a referenced asset only under `$CODEX_HOME`.
  - Do not use it to replace the locked PixelLab pipeline without explicit user/Claude direction. Treat it as concept/support/QC aid unless task says production asset.
- screenshot / image viewing:
  - Use for Unity screenshot review, anchor visual QC, sprite scale/pose/handedness checks, and before/after visual evidence.
- doc / pdf style work:
  - Use for GDD, guide, production contract, asset pipeline docs, and long report summarization when asked.
- jupyter-notebook style analysis:
  - Useful for skill damage curves, drop rates, economy, room scaling, resource tuning, RNG tables, and balance simulations.
- sora/video:
  - Possible for trailer/concept video direction, but not critical for Faz 1.

## RIMA Project Infrastructure Snapshot (Memory)
- Unity stack already strong: Unity 6000.3.6f1, URP 17.3.0, 2D Animation, Aseprite Importer, PSD Importer, Tilemap, Input System, Test Framework.
- Main scene: `Assets/Scenes/_IsoGame.unity`.
- Runtime systems appear present: room loop, dungeon graph, HUD, draft, enemy, resource systems.
- Test structure exists: about 10 EditMode and 1 PlayMode test file. Verify exact count before reporting.
- Useful local tools:
  - `Tools/prefab_health_check.py` — prefab health/QC check.
  - `Tools/audit_cleanup.py` — staging/archive/root clutter audit.
  - `Tools/remove_bg.py` — asset background cleanup.
  - `Tools/comfy_*` — pixel art candidate generation/selection support.
  - `GUIDES/PRODUCTION_CONTRACT.md` — asset production contract reference.
- Active bottleneck is pipeline discipline:
  - 128x128 canvas
  - PPU 64
  - Multiple sprite mode
  - center pivot
  - direction suffix standard
  - idle/run scale drift QC
  - PixelLab -> Aseprite QC -> Unity import -> prefab/animator wiring
- Current attention flags:
  - `CURRENT_STATUS.md` is the active next-work source: PixelLab production, Warblade character_id, Elementalist/Brawler/Hexer/Summoner regeneration, Unity Batch QC, Death screen, Skill draft UI, Faz 1 loop test.
  - Possible `RageSystem` default mismatch: status says `ragePerHitDealt=2`, `ragePerKill=5`; script may show `1` and `3`. Verify before changing.
  - `AGENTS.md` may display Turkish mojibake/encoding corruption in terminal. Content may still be readable, but fix only when explicitly tasked.

## Claude Handoff Prompt - 2026-04-29 (ASCII-safe)

Copy/paste this to Claude when the Codex capability + MCP discussion needs to be restored:

```text
Claude, carry this forward into RIMA project memory/status.

Codex read-only pass summary:
1. Most useful non-MCP Codex skills for RIMA:
   - imagegen: useful for concept art, sprite sheet exploration, VFX sheet ideation, UI mockups, transparent cutouts, and visual variants. Treat as support/concept/QC unless a production-asset task explicitly says otherwise. Default is built-in imagegen; CLI fallback only on explicit request or confirmed true native transparency need.
   - screenshot / image viewing: useful for Unity screenshots, gameplay visual QC, anchor/sprite scale checks, pose/handedness checks, and before/after evidence.
   - doc / pdf: useful for summarizing GDD, guides, production contracts, and asset pipeline docs.
   - jupyter-notebook: useful for numerical balance work such as skill damage, drop rates, economy, room scaling, resource tuning, RNG tables, and simulations.
   - sora/video: possible for trailer or concept video direction, but not critical for Phase 1.

2. The project already has strong Unity infrastructure:
   - Unity 6000.3.6f1
   - URP 17.3.0
   - 2D Animation, Aseprite Importer, PSD Importer, Tilemap, Input System, Test Framework installed.
   - Main scene: Assets/Scenes/_IsoGame.unity
   - Runtime systems appear present: room loop, dungeon graph, HUD, draft, enemy, resource systems.
   - Test structure exists: roughly 10 EditMode and 1 PlayMode test file, but verify exact count before reporting.

3. RIMA's main bottleneck is not MCP; it is pipeline discipline:
   - 128x128 canvas
   - PPU 64
   - Multiple sprite mode
   - center pivot
   - 8-direction suffix standard
   - idle/run scale drift QC
   - PixelLab -> Aseprite QC -> Unity import -> prefab/animator wiring chain

4. Existing useful local tools:
   - Tools/prefab_health_check.py: prefab health/QC check.
   - Tools/audit_cleanup.py: staging/archive/root clutter audit.
   - Tools/remove_bg.py: asset background cleanup.
   - Tools/comfy_*: pixel art candidate generation/selection support.
   - GUIDES/PRODUCTION_CONTRACT.md: main asset production contract reference.

5. Current attention flags:
   - CURRENT_STATUS.md is the active next-work source.
   - Next items include PixelLab production, Warblade character_id, Elementalist/Brawler/Hexer/Summoner regeneration, Unity Batch QC, Death screen, Skill draft UI, and Phase 1 loop testing.
   - Possible RageSystem default mismatch: CURRENT_STATUS says ragePerHitDealt=2 and ragePerKill=5 reset target; script may show 1 and 3. Verify before changing.
   - AGENTS.md has Turkish mojibake/encoding corruption when read in terminal. Do not casually "fix" it during unrelated work.

Encoding note:
AGENTS.md appears double-encoded/mojibake in Codex terminal output. For shared Claude/Codex handoffs, prefer ASCII-safe English or Turkish without diacritics unless we deliberately do an encoding cleanup task. If encoding cleanup is requested, first back up the file, detect actual bytes, convert once to UTF-8, then verify both Claude and Codex can read it. Do not repeatedly re-save the same file with different encodings.

Recommended next Claude action:
Review whether this should be synced into CURRENT_STATUS.md / CLAUDE.md / AGENTS.md via rima-doc. Do not treat it as a Codex production task by itself.
```

## Project File Map (Read this — do NOT scan files individually)

### Root (F:/Antigravity Projeler/2d roguelite/RIMA/)
| File | Purpose |
|---|---|
| CURRENT_STATUS.md | Active next-work source — read every session |
| CLAUDE.md | Claude Code instructions (routing, rules, workflow) |
| AGENTS.md | Agent routing matrix (encoding warning: may mojibake in terminal) |
| SYSTEM_MAP.md | Unity system wiring — Inspector refs, prefab slots |
| CODEX.md | This file — Codex persistent brain |
| CODEX_TASKS.md | Active task queue — read before starting any task |
| CODEX_DONE.md | Task output destination — overwrite per task |

### TASARIM/
| File | Purpose |
|---|---|
| GDD.md | Game design document — core pillars |
| MASTER_KARAR_BELGESI.md | All major design decisions, numbered |
| SINIF_VE_SKILL_KARAR_BELGESI.md | Per-class skill/lore decisions |
| STYLE_BIBLE.md | Visual identity — palette, tone, proportions |
| COMBAT_ROSTER.md | Enemy list + combat stats |
| BOSS_DESIGN.md | Boss mechanics |
| FAZLAR/FAZ_MASTER.md | Phase roadmap — which features belong to which phase |

### _STAGING/
| Path | Purpose |
|---|---|
| PROMPTS_S43/PRODUCTION_GUIDE_S43.md | Active PixelLab prompt guide — all 10 class descriptions |
| PROMPTS_S43/styleref_cheatsheet_v1.md | Anchor visual identity per class |
| anchors/<class>/<class>_anchor.png | Approved anchor sprite for that class |
| RIFT_REMNANT_PLAN_S43.md | Rift vs non-rift taxonomy decision (Codex analysis) |

### Assets/
| Path | Purpose |
|---|---|
| Scenes/_IsoGame.unity | Main active scene |
| Scripts/ | All C# runtime code |
| Sprites/Characters/ | Imported character spritesheet PNGs |
| Prefabs/ | Runtime prefabs (player, enemy, room, HUD) |

### Other folders
| Folder | Purpose |
|---|---|
| Characters/ | Raw PixelLab export PNGs before Unity import |
| GUIDES/ | Production contracts, pipeline guides, how-tos |
| CONCEPT_ART/ | Gemini/ComfyUI concept renders (non-production) |
| _ARCHIVE/ | Completed/deprecated files — do not edit |
| Tools/ | Python QC/audit scripts |

## Reporting Memory (Permanent)
- For every non-trivial task report, produce Claude-review-ready detail by default.
- Minimum report structure:
  1) direct answers/results
  2) evidence table (source -> claim -> confidence)
  3) explicit assumptions and gaps
  4) reviewer checklist (PASS/FAIL points)
- Do not send short summary-only reports unless user explicitly asks for brief output.

## Default Editable Files (without extra permission)
- CODEX.md
- CODEX_TASKS.md
- CODEX_DONE.md

## Failure Recovery
If any step is missed:
- Fix immediately.
- Log correction in CODEX_DONE.md.
- Continue from corrected state.

## PixelLab Production Baseline (RIMA)
- Final in-game character/animation standard: `128x128`.
- User-confirmed live behavior:
  - `Animate with text NEW` at `128px` can reach `16 frames`.
  - `Animate with text NEW` at `220px` observed max around `10`.
- Practical default:
  - Use `Animate with text NEW` for key motion chunks.
  - Use `Interpolate NEW` for in-betweens and transitions.

## Prompt Policy Memory
- Keep prompts short and structural.
- One generation = one motion intention.
- No camera angle text in prompt (camera handled in UI).
- Use hard constraints for weapon-hand continuity.
- Warblade mandatory constraints:
  - `both hands on same long hilt`
  - `right hand near crossguard, left hand near pommel`

## Animation Construction Memory
- Prefer segmented chains over one long request:
  - `A -> B`
  - `B -> C`
  - optional `C -> D`
- For run to idle, generate a dedicated short bridge (`run_end -> idle_start`) instead of full regen.

## Known Asset History Memory
- Warblade `220x220` PNG sets were converted to `128x128` with backups preserved.
- Backup folders (timestamped):
  - `F:\Antigravity Projeler\2d roguelite\RIMA\Characters\Warblade\_backup_220_20260422_023956`
  - `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Sprites\Characters\Warblade\_backup_220_20260422_023956`

## Direction Convention (RIMA — Kalıcı Kural)

Tüm 8-yön animasyon wiring işlerinde bu tabloyu kullan. Blend position ASLA tersine çevrilmez.

| Direction | DirX  | DirY  | Sprite suffix | BlendPos      |
|-----------|-------|-------|---------------|---------------|
| S         | 0     | -1    | _S            | (0, -1)       |
| SE        | +0.71 | -0.71 | _SE           | (0.71, -0.71) |
| E         | +1    | 0     | _E            | (1, 0)        |
| NE        | +0.71 | +0.71 | _NE           | (0.71, 0.71)  |
| N         | 0     | +1    | _N            | (0, 1)        |
| NW        | -0.71 | +0.71 | _NW           | (-0.71, 0.71) |
| W         | -1    | 0     | _W            | (-1, 0)       |
| SW        | -0.71 | -0.71 | _SW           | (-0.71, -0.71)|

Import standard (tüm animasyon tipleri):
- PPU = 64
- Sprite Mode = Multiple
- Frame size = 128x128 per cell
- Pivot = center (0.5, 0.5)
- Filter = Point (no filter)
- Compression = Uncompressed

Dosya adlandırma: büyük harf suffix. warblade_run_SE.png DOGRU, warblade_run_se.png YANLIS.

## Sprite Import Pitfalls (Geçmişte Yapılan Hatalar — Tekrarlama)

### Hata 1 — PPU Tutarsızlığı (session 34)
Idle sprite PPU=44, Run sprite PPU=64 olarak import edildi.
Sonuç: Idle karakteri Run'dan farklı boyutta render edildi (1.34x-1.66x fark).
KURAL: Tüm animasyon tipleri (idle, run, attack, skill, dash, death) için PPU=64 kullan. Geçici workaround olarak farklı PPU ASLA ayarlama.

### Hata 2 — Single Mode Auto-Trim + Yanlış Pivot (session 34)
Idle spritelar Single mode import edildi. Unity transparent pikselleri kırptı: 128x128 canvas → 94x101 karakter alanı.
Pivot otomatik (0,0) sol-alt köşe olarak atandı.
Run spritelar Multiple mode, full 128x128, pivot center (0.5,0.5) idi.
Sonuç: Idle↔Run geçişinde karakter pozisyonu zıpladı (farklı pivot noktası).
KURAL: Her zaman Multiple mode kullan, explicit 128x128 rect tanımla, pivot=(0.5,0.5) center. Single mode auto-trim'e güvenme.

### Hata 3 — Blend Position Ters Atama (session 34)
run_SE clip'i blend tree'de (-0.71,-0.71) = SW pozisyonuna atandı.
Sonuç: SE yönünde hareket edince SW yönünün sprite'ı oynuyor.
KURAL: Yukarıdaki Direction Convention tablosunu kullan. SE = (0.71,-0.71) = sağ+aşağı. Asla tahmin etme, tablodan kopyala.

## Sprite Scale Consistency Rules (session 34 — Kalıcı Kural)

Idle ve Run spritelar arasında karakter canvas doluluk oranı tutarsızlığı tespit edildi.
Idle bbox height ~45% canvas, Run bbox height ~61% canvas. Kök neden: farklı PixelLab üretim akışı.

### Kural 1 — Aynı Pipeline
Blend-critical state'ler (idle, run, attack) için **aynı PixelLab akışını** kullan.
`Custom Animation V3` → idle için de, run için de.
`Create Character` → **base sprite üretir** (scale anchor). Bu base sprite V3 için first frame olur. Create Character'dan direkt animasyon klip oluşturma.

### Kural 2 — Baseline Scale Lock
Her class için bir "baseline direction frame" (genellikle _S) üretip kilitle.
Tüm diğer yönler bu frame'e ±%5 bbox-height toleransında olmalı. Dışına çıkan → yeniden üret.

### Kural 3 — Import Sabittir, Art Sorununu Kod ile Çözme
PPU=64, 128×128, Multiple, center pivot sabittir. Scale farkı görünüyorsa sorun Unity değil sprite art occupancy'dir.
Unity import ayarlarını değiştirerek geçici fix yapma — sprite'ı yeniden üret.

### Kural 4 — Prompt Framing Constraint (zorunlu)
Her PixelLab prompt'una ekle: `full body, centered, same scale as reference, no zoom-in`
Animasyon akışında karakter referansı her seferinde aynı olmalı (gallery'den ya da upload).

### Kural 5 — Aseprite QC Adımı
Unity'e import etmeden önce: Aseprite'da aynı yön idle + run karelerini üst üste koy.
Silhouette ve ayak noktası hizası tutuyorsa → geç. Tutmuyorsa → yeniden üret.
