# Codex Task — Object Üretim Master Spec Review v1

**Reviewer:** Codex (high effort)
**Spec under review:** `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`
**Source task:** `STAGING/OPUS_TASK_object_production_master_spec_s95.md`
**Output:** `STAGING/CODEX_DONE_object_production_master_review_v1.md`

## Görev

Opus draft v1'i PixelLab API constraint'lerine + bilinen Discord/docs best practice'ine + RIMA production realities'e karşı doğrula. Her 9 karar başlığı için verdict: **LIVE / NEEDS_REVISION / BLOCKED**.

## Bağlam

User S95 LATE NIGHT direktifi:
> "create tiles pro ile tile üretilmiyor mu? objede pixeli verip descriptiona exact verebilirsin. 65c99904-12b8-4b98-9e5f-fe2f280f6a2f bu 128piksel ama düzgün şekilde duruyor. 128pikselde 4 çeşit yapabiliyoruz, 64te 16, 32 piksel 64 tane. obje üretirken her zaman grupla mantıksal olarak gruplamalısın. opusa codexe review ettir son kararı getir."

Hedef: tile üretim disiplinli, batched, stil tutarlı, prompt drift'siz.

## Verified API Constraints (Opus zaten doğruladı, Codex re-confirm etsin)

- `create_object`: size 32-256 SQUARE, directions 1/8, n_frames [1, 4, 16, 64] (sadece dir=1), view low/high top-down/side, state_of, object_view (top-down/sidescroller/null), reference_image
- `create_tiles_pro`: tile_type (hex/hex_pointy/isometric/octagon/square_topdown), tile_size 16-128, tile_height 16-256 (non-square mümkün), tile_view_angle 0-90 (0=side, 90=top-down), outline_mode (outline/segmentation)
- `create_isometric_tile`: size 16-64 MAX, tile_shape (thin/thick/block), detail/outline/shading
- `create_object_state`: edit_description 1-1000 char, group_id paylaşımı

## Critical Review Points

### 1. L2a tool seçimi
Opus: `create_object` size=128 view="low top-down" n_frames=4 batch.
Soru: L2a "collider source + flat footprint" rolü için `create_isometric_tile` size=64 thin tile daha mı doğru, yoksa bundle stil tutarlılığı gerçekten kritik mi?

### 2. L2b tool seçimi
Opus: `create_object` size=128 SQUARE view="side" n_frames=4 batch.
Soru:
- 64w × 128h dikey wall sprite gerçekten 128 square canvas'ta üretiliyor mu (sprite canvas'ı dolduruyor mu)?
- `create_tiles_pro` tile_size=64 tile_height=128 tile_view_angle=0 yolu test edildi mi PixelLab Discord/docs'da? Stil "wall billboard" kategorisinde mi çıkıyor?

### 3. Size × n_frames matrix
Opus: 32→64, 64→16, 128→4, 256→1.
Soru: n_frames=64 batch'inde AI gerçekten 64 stable frame veriyor mu, yoksa 32-48 frame'de degrade ediyor mu? PixelLab consistent-style pack'in pratik upper limit'i?

### 4. Grouping kuralı (max 16 unique per dispatch)
Opus: max 16 unique item, kalan slot variant'lar.
Soru: "17-64). variants of items above" notasyonu PixelLab AI tarafından doğru parse ediliyor mu? Explicit numbered list (64 numara açıkça yazılı) daha mı güvenli?

### 5. state_of vs yeni object
Opus: ≤%30 piksel değişim = state_of, > %30 = yeni object.
Soru: Wall damaged (üst kısmı kırık, taş dağılmış) %30 sınırını aşıyor olabilir. State_of API'sinde piksel değişimi sınırı resmi olarak ne?

### 6. View parametresi mapping
Opus: L2a=low top-down, L2b=side, mob=low top-down, tall prop=high top-down, wall-mounted=side.
Soru:
- `view="side"` ile 128 square canvas'ta dikey billboard yeterli yüksek çıkıyor mu?
- `object_view="top-down"` explicit vs null default farkı nedir?

### 7. Description prompt formülü
Opus formülü: `[GEOMETRY], [MATERIAL+HEX], [DETAIL+HEX], [PERSPECTIVE], [STYLE ANCHOR], [BACKGROUND]`
Soru:
- "Hades-style" / "dark fantasy" / genre label YASAK confirm.
- "single dark facet from side perspective" yeterli mi, yoksa "tall vertical sprite filling canvas height" eklenmeli mi?
- Color hex zorunlu mu yoksa color name (granite gray) da çalışıyor mu?

### 8. 4-piece wall batch
Opus: tek dispatch n_frames=4 ile face_NS + face_EW + corner + arch.
Soru: PixelLab consistent-style pack pipeline "different items" mı, "same item variants" mı yapıyor? Stil tutarlı 4 farklı geometry realistik mi?

### 9. Bütçe plan
Opus: Sıra 1-3 ~240 gen, total plan ~1,130 gen / 2,500 bütçe.
Soru:
- Per-dispatch gen estimate'leri doğru mu (n_frames=4 batch ≈ 25 gen)?
- S94 batch'inde 64×64 base object ≈ 20 gen, state ≈ 10 gen ispatlandı. 128×128 base + n_frames=4 gerçek cost ne?

## Output Format

```markdown
# Codex Review — Object Production Master Spec v1

## Overall Verdict
{PASS / PASS_WITH_REVISIONS / NEEDS_MAJOR_REWORK / BLOCKED}

## Per-Karar Verdict

### Karar 1: L2a tool
- Verdict: LIVE / NEEDS_REVISION
- Comment: ...
- Suggested change (if any): ...

### Karar 2: L2b tool
- Verdict: ...
- Comment: ...
- Suggested change: ...

### ... (3-9)

## Open Questions for Iter 2
- ...

## Critical Blockers (if any)
- ...
```

## Hard Constraints

- **Sadece review — kod, asset, prefab YOK.**
- **PixelLab MCP dispatch ETME.**
- Mevcut PixelLab Discord/docs bilginle constraint'leri çapraz check et.
- Iter 1 review, max ~30 dk.
