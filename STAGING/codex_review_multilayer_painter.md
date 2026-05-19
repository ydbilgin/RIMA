# Codex Review: Multi-Layer Painter Plan v1

## Context

S91 LOCK = Map Plan v1 (Hibrit painted bg + gameplay overlay, Hades model). Bu sabah `RoomTemplateSO.backgroundSprite` (single Sprite) field eklendi + runtime spawn LIVE (419/419 EditMode PASS). 

**User feedback bu turn:** "Multi-layer painter istiyorum başından beri, LOCK'la, RIMA + diğer oyunlarda kullanacağım reusable pattern." 1 sprite yetmez → birden çok painted layer üst üste, sortingOrder seçilebilir.

Orchestrator (Claude) plan v1 yazdı: `STAGING/MULTILAYER_PAINTER_PLAN_v1.md`.

**Senin görevin:** Bu plan'ı review et + verdict ver. PASS olursa Claude Sonnet'e Phase 1 implementation dispatch edecek (LOCK gate).

## Plan dosyası

**Read:** `STAGING/MULTILAYER_PAINTER_PLAN_v1.md` (full file). 10 section, ~250 satır.

## Review yap

Plan §10'da listelenen 6 madde için PASS/FAIL ver:

(a) **Data model soundness** — `BackgroundLayerData` field set (layerName, sprite, sortingOrder, offset, scale, tint, visible) painter UX için yeterli mi? Eksik mi?

(b) **Migration cleanliness** — `backgroundSprite` removal + List<BackgroundLayerData> replacement asset breakage yapar mı? (Plan diyor ki: bu sabah eklenen field hiçbir non-test asset'e set edilmedi, Spawn_01 cleared.)

(c) **Runtime spawn correctness** — Plan §3 foreach-layer block: null safety, sortingOrder per layer, transform parent chain doğru mu? 

(d) **Phase 1 vs Phase 2 split** — Custom Inspector ReorderableList (Phase 1) ilk olarak yeterli mi? Dedicated EditorWindow (Phase 2) defer mantıklı mı? Yoksa Phase 1'de bile EditorWindow gerek mi?

(e) **Asset size table** — PixelLab Pro 512/632/688 sizes (plan §5) gerçek MCP/Web UI ile aligned mı? Eksik bir size var mı?

(f) **Open questions §8** — 5 maddeye verdict ver:
1. sortingLayerName eklemek gerek mi Phase 1'de?
2. 8 layer × 14 oda = 112 SR mass spawn cost kabul edilir mi?
3. Characters backgroundLayers'da OLAMAZ mı? (Plan diyor evet, reaffirm.)
4. Phase 1.5 RoomData spec uyumu — backgroundLayers list aynı pattern mi?
5. Inspector thumbnail async load — `AssetPreview.IsLoadingAssetPreview` pattern OK mi?

## Output formatı

`CODEX_DONE_multilayer_painter_review.md` dosyasına şu format:

```markdown
# Codex Review: Multi-Layer Painter Plan v1 — VERDICT

## Verdict: PASS | FAIL | PASS_WITH_REVISIONS

## Section-by-section
### (a) Data model: PASS/FAIL
[evidence + rationale]

### (b) Migration: PASS/FAIL
...

### (c) Runtime spawn: PASS/FAIL
...

### (d) Phase 1/2 split: PASS/FAIL
...

### (e) Asset sizes: PASS/FAIL
...

### (f) Open questions verdicts
1. sortingLayerName: YES/NO + reason
2. Mass spawn cost: ACCEPTABLE/CONCERN + reason
3. Characters scope: AGREE/DISAGREE + reason
4. Phase 1.5 alignment: ALIGNED/MISMATCH + reason
5. Thumbnail async: OK/NEEDS_REVISION + reason

## Critical findings (FIX FIRST if any)
[list]

## Recommendations (nice-to-have, not blocking)
[list]

## Final go/no-go
[1-line clear directive: "PROCEED with Phase 1 impl" OR "REVISE plan section X before impl"]
```

## Hard limits

- Sadece plan'ı review et. KOD YAZMA.
- 5 dakika max effort. Plan ~250 satır, hızlı oku.
- Asset sizes (§5) tablosunu PixelLab Pro UI bilgisiyle çapraz kontrol et — uydurma. Memory referansı: `reference_pixellab_create_image_pro.md`.
- Eğer plan'da kritik bir gözden kaçan şey görürsen FIX-FIRST'e ekle, sessiz geçme.
- Memory/CURRENT_STATUS dokunma — Task #4 sonrası yapılacak.
