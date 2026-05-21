# Codex Task — Antigravity Verify + 2 Follow-up (S95 Phase A+B+C)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_antigravity_verify_plus_followup_s95.md`
> **Geri dönülebilir:** Auto-commit YOK. Sahne kapanır save edilmez, sadece script edit.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Bağlam

Opus review verdict: Antigravity'nin wall layering fix'i PASS, ama 3 hardcoded sortingLayer literal + 2 follow-up gap (transparency research flagged). Bu task: verify + 2 small fix.

**Source:** `STAGING/ANTIGRAVITY_DONE.md` + Opus brief (`STAGING/OPUS_TASK_antigravity_done_review_s95.md`).

## Phase A — Verify Antigravity Claims

### A1. dotnet build
- `dotnet build` targeted (RIMA.Runtime, Assembly-CSharp, Assembly-CSharp-Editor) → 0 error
- Rapor: build status

### A2. Hardcoded sortingLayer literal grep
`Assets/Editor/RimaUnifiedPainterWindow.cs`'de şu line'ları gözle:
- Line 1481-1485 (PaintPrefab)
- Line 2628-2629 (PaintWallWithConnections)
- Line 2746-2747 (UpdateWallConnectionsAt)

Her birinde:
- `"Walls"` / `"Entities"` string literal var mı?
- `sortingOrder = 20` magic number var mı?

3 yerin de duplikasyonunu raporla (Karpathy single-source ihlali — UIUX v3.1 Q8 implementation subsume edecek, **şu an fix etme**, sadece doğrula).

### A3. EditorGUILayout.EndScrollView fix cerrahi mi?
`DrawPalettePanel` (line 766 civarı) review et:
- Line 793 early-return path (`if (activeList == null || activeList.Count == 0)`) EndScrollView var mı?
- Line 822 normal exit EndScrollView var mı?
- Method scope dışına başka kod taşımamış mı?
- Verdict: cerrahi PASS / scope creep

### A4. Unity Editor smoke test
- Editor cold-open + Painter window aç-kapat
- Console'da `GUIClip`, `BeginScrollView called without EndScrollView`, layout mismatch log'u var mı?
- Beklenen: 0 log

### A5. Scene state verify
- `PathC_BaseTest.unity` (Antigravity'nin temizlediği sahne) reload
- `Default` layer'da kaç renderer? Beklenen: 0
- `Walls` layer'da kaç renderer? Beklenen: 52
- `Entities` layer'da kaç renderer? Beklenen: 8

### A6. ConfigureCollider DEĞİŞTİRİLMEDİ mi?
Line 1910-1986 (ConfigureCollider) Antigravity tarafından edit edilmediğini doğrula. WallBlock formülü `(spriteWidth*scale, 0.8f)` intact mı?
- Antigravity'nin "size=(4.00, 1.60)" iddiası SAHNE INSTANCE WRITE — painter formülü kilitli olmalı
- Diff veya hash check ile teyit

---

## Phase B — Follow-up Touches (Small Scope)

### B1. IsometricSortSetup.cs `"Wall"` → `"Walls"` fix

**Dosya:** `Assets/Editor/DevTools/IsometricSortSetup.cs`

**Bağlam:** Transparency research §Drift Notes flag'lemişti — bu script hâlâ `"Wall"` (singular) string'i kullanıyor, Walls/Wall duality kaynağı. Tek satır edit.

**Adım:**
- Grep `"Wall"` (whole-word, plural değil) — bul
- `"Walls"` (canonical) ile değiştir
- Sadece sortingLayer name string'leri (başka identifier'a dokunma)

### B2. RimaSortingLayerValidator.cs orphan layer create cleanup

**Dosya:** `Assets/Editor/RimaSortingLayerValidator.cs`

**Bağlam:** Transparency research audit table'ı `Detail/Accent/Props` orphan layer DELETE candidate dedi. Atomic cleanup zaten TagManager'dan sildi. Bu validator hâlâ create ediyorsa, atomic cleanup öncesi mantığı geri getirir.

**Adım:**
- `Detail` / `Accent` / `Props` layer create satırlarını comment-out et (HARD DELETE değil, scene serialized GUID geri-uyum riski için)
- Validator canonical 5 layer'da (Default, Ground, Walls, Entities, VFX) kontrol etsin
- Açıklama yorumu ekle: `// 2026-05-20 S95: Detail/Accent/Props orphan, atomic cleanup TagManager'dan sildi`

---

## Phase C — Verdict

```markdown
# Antigravity Verify + Follow-up — Codex Report

## Phase A Verify
### A1 dotnet build
- Result: 0 error / N error
### A2 Hardcoded literal grep
- Line 1481: "Walls" CONFIRMED (Karpathy violation, UIUX v3.1 subsume planlı)
- Line 1485: "Entities" CONFIRMED
- Line 2628/2629: "Walls" + order=20 CONFIRMED
- Line 2746/2747: aynı
- Verdict: 3-way duplication, subsume by UIUX implementation
### A3 EndScrollView
- Line 793 early-return: EndScrollView present YES/NO
- Line 822 normal exit: EndScrollView present YES/NO
- Scope: cerrahi PASS / creep
### A4 Editor smoke test
- GUIClip log: 0 / N
- Layout mismatch: 0 / N
### A5 Scene renderer counts
- Default: X (expected 0)
- Walls: X (expected 52)
- Entities: X (expected 8)
### A6 ConfigureCollider intact
- Diff: clean YES/NO
- WallBlock formula `(spriteWidth*scale, 0.8f)`: intact YES/NO

## Phase B Follow-up
### B1 IsometricSortSetup.cs
- `"Wall"` occurrences found: N
- Replaced with `"Walls"`: N
- Files touched: IsometricSortSetup.cs
### B2 RimaSortingLayerValidator.cs
- Detail/Accent/Props create lines commented-out: YES/NO
- Canonical 5-layer check: YES/NO

## Phase C Verdict
- Antigravity claims: PASS / FAIL_DETAIL
- Sortinglayer hardcoded drift: CONFIRMED (subsume by UIUX impl)
- Follow-up B1+B2: APPLIED / BLOCKED
- dotnet build post-fix: 0 error / N
- ÖNERİ: UIUX DRAFT v3.1 implementation Codex task'ı dispatch et

## Git Diff Summary
- Assets/Editor/DevTools/IsometricSortSetup.cs (N lines)
- Assets/Editor/RimaSortingLayerValidator.cs (N lines)

## Açık Sorular
- ...
```

## Hard Constraints

- **Sadece B1 (IsometricSortSetup.cs) + B2 (RimaSortingLayerValidator.cs)** edit. RimaUnifiedPainterWindow.cs'e DOKUNMA (UIUX implementation ayrı task'ta).
- **Auto-commit YOK** — user manual commit.
- **Scene save YASAK** (smoke test cold-open + close, kalıcı değişiklik yok).
- **BLOCKED if unclear:**
  - IsometricSortSetup.cs'de "Wall" referansının silinmesi başka serialized GUID'i kırarsa STOP
  - RimaSortingLayerValidator.cs orphan layer create'i sahnede aktif kullanım varsa STOP
