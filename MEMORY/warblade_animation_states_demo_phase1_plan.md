---
name: warblade-animation-states-demo-phase1-plan
description: Warblade demo Faz 1 animation state production planı. PixelLab character_state ile first/end keyframe çıkar, kullanıcı in-between'i Aseprite/Unity Animator'da kendi yapar. State spec + prompt + tier prioritization. 2026-05-27 pending user approval.
metadata:
  type: project
  source: local memory + S110 LATE planning (NLM YOK kullanıcı direktifi)
---

## Workflow disiplini (kullanıcı 2026-05-27 lock)
- **State'leri Claude (orchestrator) belirler + prompt yazar** → kullanıcı onaylar → PixelLab Web UI gen
- **First + end frame her state'ten export edilir** (PixelLab character_state özelliği)
- **Animasyon (in-between)** kullanıcı kendi yapar (Aseprite tween veya Unity Animator interpolate)
- MCP autonomous state gen YASAK ([[feedback-pixellab-mcp-halt-strict]])

## Anchor referansı
- **Character ID:** `2656075d-d113-4f18-a6c1-94b5a6b8bf65` (Warblade young v2, S100 verified)
- **Canvas:** 120×120
- **Pivot:** bottom-center foot (top-down 3/4 standard)
- **Style:** weaponless body, weapon = separate sprite child via HandAnchor (Karar #123/#144/#146)
- **Camera POV:** HIGH TOP-DOWN 3/4 ~70-80°
- **Palette:** Aseprite post-gen 16-color remap (rima_palette.gpl)

## State tier prioritization (cost-aware)

### Tier 1 — Demo BLOCKER (5 state, mandatory)
Demo loop oynanmaz bunlar olmadan:

| # | State | View count | First frame | End frame | Notes |
|---|---|---|---|---|---|
| 1 | **Idle** | 5 native (S/SE/E/NE/N) + 3 mirror | rest pose, weight on one leg | shifted weight, opposite leg | 2-frame breathing bob |
| 2 | **Walk** | 5 native + 3 mirror | left foot forward extended | right foot forward extended | 2 keyframe + interpolate cycle |
| 3 | **Basic Attack** (LMB single swing) | 5 native + 3 mirror | windup pose (sword right side raised) | follow-through (sword left side low) | First combo hit only Faz 1; 3-hit combo Track B |
| 4 | **Hit/Stagger** | 4 (S/E/N/W) | impact pose (body recoil back) | recovery (returning to idle) | 4-dir yeter readability için |
| 5 | **Death** | 4 (S/E/N/W) | knees buckling forward | flat on ground | dramatic moment, 4-dir |

**Toplam Tier 1:** 5 state × ~5-8 view = 25-40 keyframe pair (50-80 sprite). Cost: orta.

### Tier 2 — Skill animation (4 state, Faz 1 demo havuz)
Skill demo'da görünür ama Tier 1 sonrası:

| # | State | View | First frame | End frame | Notes |
|---|---|---|---|---|---|
| 6 | **Iron Charge / Dash** | 5 native + 3 mirror | crouch + lean forward (charge prep) | full extension forward (dash impact moment) | Dash + Iron Charge aynı clip |
| 7 | **Earthsplitter (slam)** | 5 native + 3 mirror | weapon overhead raised | weapon embedded in ground (impact crater) | knockup AoE moment |
| 8 | **Iron Counter (parry stance)** | 5 native + 3 mirror | guard up, weapon defensive cross | parry trigger reflect pose | 0.8s parry window |
| 9 | **Death Blow (execute swing)** | 5 native + 3 mirror | massive windup (weapon way back, rage glow body) | full extension cleave through | rage spend visual |

**Toplam Tier 2:** 4 state × ~5-8 view = 20-32 keyframe pair (40-64 sprite). Cost: orta.

### Tier 3 — Polish (Track B otonom, demo blocker DEĞİL)
Demo loop çalışırken kullanıcı boşta iken otonom:

| State | Notes |
|---|---|
| Basic Attack hit 2 + hit 3 | 3-hit combo full chain |
| Gravity Cleave (downward swing + pull VFX) | basic attack + VFX overlay yeter Faz 1'de |
| Sunder Mark cast | basic attack + cyan VFX yeter Faz 1'de |
| Crippling Blow | basic attack variant |
| Run (faster cycle) | walk hızlanmış variant |
| Jump/dodge roll | gameplay'de yok şu an |
| Victory pose | demo sonu opsiyonel |

## PixelLab prompt template (her state için)

```
Tool: PixelLab Create Character State (Web UI) veya Pose tool
Character: 2656075d-d113-4f18-a6c1-94b5a6b8bf65 (Warblade young v2)

State Name: <state_name>
View: south (önce, sonra E/SE/NE replications)
Canvas: 120×120 (anchor matches)
Pivot: bottom-center foot

First Frame Description: <pose tanım, 1 cümle>
End Frame Description: <pose tanım, 1 cümle>

Outfit: identical to anchor reference (weaponless body — leather pauldrons, brown breastplate, no weapon sprite)
Camera POV: high top-down 3/4 ~70-80°
Negative: do NOT include weapon (sword/blade), keep hands empty
Background: transparent
Style: pixel art, 16-color limit, crisp edges no AA fringe
```

## Production sıra önerisi (kullanıcı onayı sonrası)

**Phase 1 — Tier 1 South-only (1-2 gün, 5 state × 1 view = 5 keyframe pair)**
1. Idle south
2. Walk south
3. Basic Attack south
4. Hit south
5. Death south

→ Unity import + Warblade prefab south view test → demo loop'ta görüntü kontrol → onay.

**Phase 2 — Tier 1 multi-view (2-3 gün, kalan 4 view × 5 state)**
Her state için E/SE/NE replikasyonu. W/SW/NW mirror.

**Phase 3 — Tier 2 South-only (1-2 gün, 4 skill state × 1 view)**

**Phase 4 — Tier 2 multi-view (2-3 gün)**

**Phase 5 — Tier 3 polish** (Track B, kullanıcı boşta iken)

## Animasyon clip plan (Unity Animator, kullanıcı yapar)
| State | First/End frame'ler | Interpolation method |
|---|---|---|
| Idle | 2 keyframe | Looping bob, ease in-out 2s |
| Walk | 2 keyframe | Looping cycle, linear 0.8s |
| Basic Attack | 2 keyframe | Trigger, fast 0.3s, hit frame ~0.15s |
| Hit | 2 keyframe | Trigger, snap recoil 0.2s |
| Death | 2 keyframe | Trigger, drop 0.5s, no loop |
| Iron Charge | 2 keyframe | Trigger, fast 0.15s prep + 0.25s dash |
| Earthsplitter | 2 keyframe | Trigger, slow 0.6s windup + 0.2s impact |
| Iron Counter | 2 keyframe | Hold trigger, instant on→off |
| Death Blow | 2 keyframe | Trigger, dramatic 0.8s windup + 0.2s cleave |

## Cost referansı
- PixelLab Create Character State: ~? credit per gen (Web UI manual, kullanıcı kontrolünde)
- Edit Image Pro pose extract: 40 gen per run
- Alternative: animate-with-text v3 → 6-8 frame cycle direct (no first/end split) — Tier 1 idle/walk için bu da seçenek

## Cross-link
[[canonical-character-roster-v2]] [[project-weapon-pipeline-lock]] [[pixellab-animation-techniques]] [[feedback-pixellab-mcp-halt-strict]] [[project-demo-phase1-milestone-lock]] [[warblade-12-common-skills-spec]] [[reference-pixellab-prompt-grammar]]
