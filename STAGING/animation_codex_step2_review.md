# Animation Spec Codex Review — Step 2/3 — execute every step

## Context

rima-design Opus, RIMA animation system spec DRAFT yazdı: `STAGING/animation_spec_draft.md` (~2500 word, 9 bölüm).

User 25-gün school deadline. Faz 1 scope = Warblade primary + seam_crawler. **Sen Codex production feasibility review yapacaksın.** rima-design Step 3'te senin review'ını synthesize edip LOCK yapacak.

## Task

`STAGING/animation_spec_draft.md` Bölüm 9'da 7 spesifik soru var. Her birine **production engineer perspektifinden** cevap ver. Verdict + alternatif + risk + recommendation şeklinde.

## Sorular (Bölüm 9'dan)

### 1. CRITICAL — Faz 1 ~200 gen + ~40h Aseprite cleanup 25-gün'e fit mi?
- 200 sprite × 12 min Aseprite cleanup avg = ~40h
- PixelLab gen wait ~5h
- Unity import + AnimatorController ~10-15h
- TOTAL: ~60-75h focused work
- 25 gün × 3-4h/gün = ~75-100h available
- **Verdict:** REALISTIC / MARGINAL / CUT-SCOPE? Hangi state'ler tek başına Faz 1.5'e atılabilir?

### 2. CRITICAL — Karar #122 T1 AnimationClip event 8-dir blend tree firing pattern
- Beat 3 impact frame Unity AnimationClip event triggers `OnCommitBeat()`
- Risk: 8-dir blend tree mid-blend interpolation'da event ONCE mi 8x mi fire?
- Unity AnimationClip event vs custom timeline marker hangisi lighter?
- **Verdict:** Implementation pattern recommendation (kod örneği iyi olur)

### 3. Karar #120 split-animation Death 12f hash verification Aseprite workflow Faz 1 fit mi?
- Pre-fall 6f + ground 6f, apex byte-identical
- Aseprite Lua export script gerekli mi?
- **Verdict:** Faz 1'de tut yoksa Faz 1.5 defer?

### 4. Smear frame PixelLab Custom V3 native produce ediyor mu?
- Prompt clause: "one distorted stretch frame at peak weapon velocity"
- Native unusable ise Aseprite manual ~5-10 min/attack anim × 3 attack × 8 dir = 2-4h/class
- **Verdict:** Pilot test öner + native/manual recommendation

### 5. Selout outline shader vs sprite-baked
- Shader URP 2D pass: ~4-6h Codex setup + pipeline dependency
- Manual Aseprite: ~3-5 min/sprite × 200 sprite = ~12h Faz 1
- **Verdict:** Production economy hangisi? Hangi bottleneck'i çözer?

### 6. Karar #109 ambient idle Custom V3 vs Web UI
- Per-class 5-10f ambient
- `breathing-idle` template per-class personality yetersiz
- Custom V3 Web UI ~16 gen × 10 class = 160 gen Faz 2
- **Verdict:** Token economy + Faz 2 polish layer fit

### 7. Karar #124 form variation Faz 1 MVP fit mi?
- Warblade T2 Rift greatsword 8-dir: 30 min PixelLab + 1h Unity
- WeaponDatabase form lookup + SpriteRenderer swap on T3 cast
- T3 trigger Faz 2 — Faz 1 = debug toggle showcase
- **Verdict:** Hafta 2 polish slot'a sığar mı yoksa Faz 2 defer?

## Output

**Yeni dosya:** `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/animation_codex_step2_review.md`

Format:
```markdown
# Animation Codex Review — Step 2/3 — 2026-05-14

## Q1 Production budget verdict
[VERDICT: REALISTIC / MARGINAL / CUT-SCOPE]
[Specific cut suggestion if needed]
[Alternative path]
[Risk: 1-3 sentence]

## Q2 Karar #122 T1 event firing pattern
[Unity AnimationClip event behavior on 8-dir blend tree — fires once or per-dir?]
[Implementation pattern code skeleton]
[Verdict: AnimationClip event / Custom timeline marker / Other]

## Q3 ... (her soru için)

...

## Genel Verdict (rima-design Step 3 için)
- Faz 1 scope final recommendation
- En kritik 2 cut suggestion
- En kritik 2 LOCK confirmation
```

## Resources (Codex okumalı)

1. `STAGING/animation_spec_draft.md` — rima-design draft (öncelik)
2. `STAGING/animation_video_analysis.md` — video referans
3. `TASARIM/MASTER_KARAR_BELGESI.md` — Karar #71/#100/#108/#109/#114/#120/#122/#123/#124 references
4. `Assets/Scripts/Combat/**` — Combat FAZ 1.0 mimari mevcut kod (Karar #110)
5. `Packages/manifest.json` — Unity 6.3 LTS URP 2D version check

## Constraints

- DO NOT touch source files (read-only inceleme)
- DO NOT change rima-design draft (sen review yazıyorsun)
- Output: tek dosya STAGING/animation_codex_step2_review.md
- ~1500-2000 word focused
- Engineer perspective: kod feasibility + production economy + Faz 1 deadline
- Commit YOK (rima-design Step 3 LOCK sonrası orchestrator commit eder)

## Deliverable

CODEX_DONE.md append:
```
## [2026-05-14 S70] Animation Spec Step 2 Review — Codex production feasibility

- 7 soru cevaplandı, STAGING/animation_codex_step2_review.md
- Faz 1 scope verdict: REALISTIC / MARGINAL / CUT-SCOPE
- En kritik 2 cut: ...
- rima-design Step 3 LOCK için hazır
```
