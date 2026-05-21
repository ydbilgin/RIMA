# Opus Task — Antigravity DONE Review + Unified Painter Functionality Audit

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Output:** Orchestrator'a judgment + Codex review brief. Dosya yazma yok.
> **Mode:** Judgment + cross-check. File edit yok, agent spawn yok.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "antigravity_done'a baksın opus codexe de review etsin unified paintera nasıl işlevlik koyabiliriz baksın mantıksız bi iş varsa uyarsın opus da kabul ederse düzeltsin conflict olursa bana sorsun"

## Görev

`STAGING/ANTIGRAVITY_DONE.md` (Antigravity'nin wall layering fix sonucu) review et. 4 katmanlı analiz:

### Katman 1 — Spec Cross-Check (Antigravity yapılanlar ↔ LIVE spec'ler)

Antigravity tüm 52 duvarı **Walls** sorting layer'a, 8 prop'u **Entities**'e koymuş.

**Spec referansı doğrula:**
- `STAGING/PRODUCTION_PLAN_DETAILED_v1_1.md` (LIVE)
- `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md` (Iter 2 LIVE)
- `STAGING/CODEX_DONE_wall_transparency_research_s95.md` (Path B)
- `STAGING/RIMA_S95_LATE_NIGHT_REVIEW.md` (Antigravity'nin kendi review verdict'i)

**Soru:** L2b wall sprite (tall billboard, Hades-style, 128px) hangi sortingLayer'da olmalı?
- Wall transparency research **Entities** demişti (karakter ile aynı Y-sort katmanı, occlusion için)
- Antigravity **Walls**'a koymuş — bu Y-sort'u bozar mı?
- L2a base tile (collider source) ile L2b sprite ayrımı korunmuş mu?

**Verdict:** PASS / DRIFT / CONFLICT

### Katman 2 — Unified Painter Kod Değişiklikleri (Karpathy 4 audit)

Antigravity 3 method edit etti:
- `PaintPrefab`
- `PaintWallWithConnections`
- `UpdateWallConnectionsAt`

Bu method'larda **hardcoded "Walls" / "Entities"** sortingLayer string'i mi var? Eğer öyleyse drift kaynağı — UIUX spec C grubu (DRAFT v3.1) `GroupClassifier` ve `CollisionResolver` helper'larıyla single source of truth diyor.

Ayrıca:
- `EditorGUILayout.EndScrollView()` eksik fix — Karpathy #3 cerrahi mi (sadece eksik kapanış), yoksa daha geniş refactor mu yapmış?
- Collider hard-coded `size=(4.00, 1.60), offset=(0.00, 0.80)` — bu master spec'in `CollisionResolver` mantığını bypass mi?

**Verdict:** PASS / DRIFT / KARPATHY_VIOLATION

### Katman 3 — Unified Painter İşlevsellik Önerisi (user istedi)

User: "unified paintera nasıl işlevlik koyabiliriz baksın". Mevcut state + Antigravity edits + UIUX spec C grubu DRAFT v3.1'i göz önünde tutarak, **ek işlevsellik önerileri** çıkar:

Örnek soru başlıkları (Opus karar versin nereye gidiyoruz):
- Sortinglayer otomasyonu CollisionRulesSO'ya bağlansın mı, hardcoded mu kalsın?
- Wall L2a vs L2b ayrımı painter'da bir toggle olmalı mı (Karar #1 master spec)?
- Selected Instance Editor (Panel 5 UIUX spec) bu fix sonrası nasıl entegre olur — mevcut walls'a layer/order tweak edilebilir mi?
- Multi-select batch sortingLayer migration (örn yanlışlıkla Walls'a düşen 52 wall'u Entities'e tek tıkla taşıma) yararlı feature mi?
- Asset-import-time pivot/alignment otomatik düzeltme (RimaSortingLayerValidator gibi RimaSpriteImportValidator) eklensin mi?

### Katman 4 — Codex Review Brief (orchestrator dispatch edecek)

Codex'in kod-tarafı doğrulaması için brief hazırla:

**Codex check listesi:**
1. `RimaUnifiedPainterWindow.cs` Antigravity edit'lerinde:
   - PaintPrefab, PaintWallWithConnections, UpdateWallConnectionsAt yeni sortingLayer kod parçalarını grep et
   - Hardcoded string mı, helper class'a delege mi?
2. `dotnet build` 0 error doğrula
3. Console runtime — sahne reload + Painter pencere aç-kapat, layout error var mı?
4. Sahnede gerçekten 0 Default-layer renderer var mı (Antigravity claim doğrulaması)
5. Collider size (4, 1.6) ve offset (0, 0.8) — bu prefab'a özel mi, instance'a uygulanmış mı? CollisionRulesSO'nun gelecek tasarımıyla uyum?

## Çıktı (Orchestrator'a Direkt)

```markdown
## Antigravity DONE Review — Opus Judgment

### Katman 1 — Spec Cross-Check
- VERDICT: ...
- Walls vs Entities sorting layer drift: var/yok
- L2a vs L2b ayrımı: korunmuş/yok

### Katman 2 — Karpathy Audit
- PaintPrefab/PaintWallWithConnections sortingLayer hardcoded mı: ...
- EndScrollView fix cerrahi mi: ...
- Collider size hardcoded mu: ...
- VERDICT: ...

### Katman 3 — Unified Painter Functionality Önerisi
- Öneri 1: ...
- Öneri 2: ...
- Öneri 3: ...

### Katman 4 — Codex Review Brief
[Codex'e direkt verilecek prompt skeleton]

### Final Verdict
- PASS / NEEDS_REVISION / CONFLICT
- Conflict varsa: spesifik nokta, user'a sorulacak soru

### Conflict (varsa, user'a yöneltilecek)
- Soru 1: ...
- Soru 2: ...
```

## Hard Constraints

- **No file writes** — sadece judgment + brief.
- **No agent spawns** — orchestrator Codex'e dispatch edecek.
- **NLM ACCESS:** `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Karpathy 4 inline.**
- Max 1 iter — bu review, döngü değil.
