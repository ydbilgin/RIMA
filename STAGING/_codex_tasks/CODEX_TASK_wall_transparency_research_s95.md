# Codex Task — Wall Transparency / Occlusion + Sorting Layer Audit (S95)

> **Profile:** any active cx profile (Unity açık)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_wall_transparency_research_s95.md`
> **Mode:** Research + audit, **NO code/asset change**. Sadece rapor.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — sadece rapor (4) BLOCKED if unclear.

## Bağlam

User direktif (S95 LATE NIGHT 2026-05-20):
> "duvar gibi şeyler saydamlaşacak şekilde olacak oyunların uyguladığı tarzda. oyunlar bunu nasıl uyguluyor araştır codexe de sor sonra tekrar bana sun. isimleri düzenleyip öyle bana net anlat. yapıyı doğru kuralım"

Sahne: `Assets/Scenes/Demo/PathC_BaseTest.unity` — isometric Grid, scale (1, 0.5, 1), karakter+duvar overlap edince **duvar fade out** olsun.

## Üç Bölüm

### Bölüm 1 — Industry-Standard Wall Occlusion Teknikleri

Aşağıdaki oyunlar nasıl yapıyor, kısa teknik özet (her biri 2-4 satır):

1. **Hades** (2D angled top-down)
2. **Stardew Valley** (2D top-down)
3. **Diablo IV / II Resurrected** (3D isometric)
4. **Hyper Light Drifter** (2D top-down)
5. **Disco Elysium** (3D isometric, painted)
6. **CrossCode** (2.5D pixel-isometric, en yakın RIMA reference)

Her teknik için:
- **Yöntem adı** (sprite swap / alpha shader / stencil mask / raycast trigger / shader graph dissolve)
- **Trigger** (player Y > wall Y? raycast? collider overlap? distance check?)
- **Visual effect** (full transparent / dither / outline-only / cutout circle / soft fade)
- **Performance** (per-frame raycast vs static mesh shader)

### Bölüm 2 — Unity 2D Implementation Yolları

Unity'de bu efekti uygulamak için **3 farklı path** öner, her biri için:

| Path | Yöntem | Kod scope | Trade-off |
|---|---|---|---|
| A | Sprite swap | C# script, transparency state | Basit, shader yok ama discrete states |
| B | Alpha shader (sprite-lit / sprite-unlit + alpha multiply) | URP Shader Graph veya custom shader | Smooth fade, GPU work, URP needed |
| C | Stencil mask cutout | Stencil buffer + sprite material | Karakter etrafında "hole" — Diablo style |

Her path için **Unity 2D URP** spesifik:
- Shader değişikliği gerek mi
- 2D Renderer Asset config değişikliği gerek mi
- Sorting layer interaction (bu sistem nasıl çalışır, mevcut layer'ları nasıl kullanır)
- Compute cost tahmini (frame başına ms)

### Bölüm 3 — Mevcut Sorting Layer Audit + Cleanup Önerisi

Mevcut 11 sorting layer (Diagnose raporundan):
```
0  Default
1  Patch
2  Scatter
3  Detail
4  Accent
5  Props
6  Ground
7  Walls
8  Entities
9  VFX
10 Wall  ← duplicate gibi
```

Her layer için **rapor:**
- **Kullanım kontrolü:** Hangi prefab/scene asset'lerinde sortingLayerName olarak kullanılıyor (grep `Assets/`)
- **Drift mi intentional mı:** Patch/Scatter/Detail/Accent/Props muhtemelen Brush V1 eski iterasyon — kullanılıyor mu hâlâ?
- **Walls vs Wall:** Hangisi kullanımda, hangisi orphan?

Sonuçta **temiz layer setini öner:**
- Hangileri silinmeli (kullanım yok)
- Hangileri rename edilmeli (Walls/Wall duplicate çöz)
- Final hierarchy önerisi (wall transparency sistemi ile uyumlu) — örn:
  ```
  Background (en altta)
  Floor
  FloorOverlay
  Entities (karakter + L2b wall, Y-sort)
  Overhead (wall fade override için override layer?)
  VFX (en üstte)
  UI
  ```

### Önerilen Final Mimari

Bölüm 1 + 2 + 3'ü harmanla, RIMA için **single recommended path** öner:
- Hangi industry teknik en uygun
- Hangi Unity implementation path
- Hangi sorting layer set
- Implementation effort tahmini (1 günlük iş mi, 1 hafta mı)

## Output Format

```markdown
# Wall Transparency Research — Codex Report

## Bölüm 1: Industry Techniques
### Hades
- Yöntem: ...
- Trigger: ...
- Visual: ...
- Performance: ...
### Stardew Valley
...

## Bölüm 2: Unity 2D Implementation Paths
### Path A — Sprite Swap
- Kod scope: ...
- Pros/Cons: ...

### Path B — Alpha Shader (URP)
...

### Path C — Stencil Mask
...

## Bölüm 3: Sorting Layer Audit
### Mevcut Layer Kullanım Matrisi
| Layer | Kullanım sayısı | Asset örneği | Verdict |
|---|---|---|---|
| Default | X | ... | KEEP |
| Patch | 0/N | ... | DELETE? |
| Walls | X | ... | KEEP |
| Wall | 0 | — | DELETE (orphan) |

### Önerilen Temiz Layer Set
```
Background
Floor
...
```

## Final Mimari Önerisi
- Technique: Path B (alpha shader) + Hades-style trigger
- Sorting layers: [list]
- Effort: ~X hours/days
- Implementation order: 1) layer cleanup, 2) shader, 3) script, 4) tune

## Açık Sorular
- {soru}
```

## Hard Constraints

- **Kod/asset/scene/.meta değişikliği YASAK** — sadece rapor.
- **Grep/AssetDatabase scan OK** — sortingLayerName usage audit için.
- **Auto-commit YOK** (zaten kod yok).
- **BLOCKED if unclear:** Hangi oyunlar incelensin veya Unity URP versiyon belirsizse durdur.
