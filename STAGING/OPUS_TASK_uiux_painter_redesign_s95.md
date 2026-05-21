# Opus Task — RimaUnifiedPainter UI/UX Redesign (S95)

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Output:** `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` (proposal + Codex review iterations)
> **Mode:** Tasarım kararı + spec only. Kod yazma YOK (uygulama ayrı task).

## Hedef

`Assets/Editor/RimaUnifiedPainterWindow.cs` (2812 satır) için **UI/UX redesign spec'i** çıkar. Tek dosya tek output. Geri dönülebilir — sadece spec, Unity dosyasına dokunma.

## Mevcut Durum (özet)

- 4 kategori: Floor/Wall/Prop/Mob (UI: Zemin/Duvar/Obje/Canavar)
- CollisionMode enum: `Auto, Passable, SmallFootprint, FullFootprint, WallBlock, Custom`
- Auto-collision prefab adına göre (`wall_*` → WallBlock, `mounting_*` → Passable, vb)
- 6 alt-grup (Walls/Statues/WallMountings/Patches/Mobs/FloorProps) GetOrCreateGroupParent ile
- Save/Load: UnifiedMapSaveData → per-instance collision + position

## Bilinen UX Sorunları (Codex tespiti)

1. **Grup paneli yok** — Walls/Statues/Mobs/Props sadece Unity Hierarchy'de görünür, in-window yok
2. **Target parent warning yanıltıcı** — auto-create Props_Root'u göstermiyor, designer'ı yanıltıyor
3. **Palette ad clip** — 92×110 tile uzun adları keser (wall_face_NS_damaged → "wall_face..."
4. **Hitbox ayarı görünmez** — sadece Custom mode seçilince Vector2Field açılır, default modlarda gizmo yok
5. **Per-prefab hitbox tweak yok** — sadece per-instance, "tüm bu duvar tipinin hitbox'ını şöyle yap" diyemiyor

## Hedef UX

User şunları rahatça yapsın:
- (a) Yerleştirilmiş objelerin grup hiyerarşisini in-window görsün, count/visibility/lock kontrolü olsun
- (b) Hitbox'ı her zaman expand'li görsün (Custom yok bile olsa Auto'nun ne ürettiği gözüksün) + scene'de gizmo
- (c) Per-prefab override ("bu prefab adına yapışan default collider'ı şöyle tanımla" — JSON/SO config'i)
- (d) Duvar/obje "block mu pass mı" ayrımı palette tile'da rozet olarak görünsün

## Süreç (Opus ⇆ Codex Döngü)

1. **Iter 1 — Opus tasarım taslağı yaz** (ASCII mockup + IMGUI structure, layout, panel hiyerarşisi). Mevcut dosyadaki bölümlere `// PANEL_X` yer işareti ekle (spec'te referans için).
2. **Iter 1 — Codex review dispatch:** `cx_dispatch.py` ile Codex'e gönder:
   - Prompt: "Bu UI/UX spec'i Unity IMGUI'da implement edilebilir mi? Layout race, GUILayout indent, EditorWindow Repaint loop, prefab field reference loss riski var mı? Naming/scope çakışması var mı?"
   - Done dosyası: `STAGING/CODEX_DONE_uiux_painter_review_v1.md`
3. **Iter 2 — Opus revize** Codex feedback üzerinden. Eksikleri kapat, gerçekçi olmayan UI önerilerini düşür.
4. **Iter 2 — Codex re-review:** "Bu revize spec implementable mı, açık tartışma noktası kaldı mı?"
5. **Iter 3 (opsiyonel) — Opus final** Codex onay verdiğinde STOP.
6. **Aşırı uzarsa (>3 iter veya >90 dk):** BLOCKED yaz, kalan açık sorunları enumerate et, durdur.

## Çıktı Format — `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md`

```markdown
# RimaUnifiedPainter UI/UX Redesign — DRAFT v{N}

## Verdict
{LIVE / NEEDS_USER_INPUT / BLOCKED}

## Iter Log
- v1: Opus draft → Codex review (1 LIVE, 3 NEEDS_REVIEW)
- v2: Opus revize → Codex re-review (PASS)

## Final Spec
### Panel 1: Scene Organization
{ASCII mockup}
{Implementation notes}

### Panel 2: Collision Inspector
{ASCII mockup}
{Default collision resolver logic}
{Per-prefab override config (CollisionRulesSO?)}

### Panel 3: Palette Tile Redesign
{Width, padding, name wrap, badge spec}

### Panel 4: Target Status Banner
{Where, what messages, color states}

## Codex Review Excerpts
{Quote key feedback lines}

## Açık Sorular (kullanıcıya)
- {Soru 1}
- {Soru 2}
```

## Hard Constraints

- **Geri dönülebilir:** Hiçbir aşamada `RimaUnifiedPainterWindow.cs` dosyasına dokunma. Sadece `STAGING/UIUX_PAINTER_DESIGN_DRAFT.md` yaz.
- **Karpathy 4 inline:** (1) düşün önce (2) min spec (3) cerrahi — sadece UI/UX, mimari refactor önerme (4) belirsizse BLOCKED.
- **NLM ACCESS:** Tasarım context'i için NLM:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Codex dispatch komutu:**
  ```bash
  python '/f/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py' \
    --task-file STAGING/CODEX_TASK_uiux_painter_review_v{N}.md --effort high
  ```
  Background değil — subagent burada bekleyebilir, Codex sonucu gelince devam et.
- **No code generation** — sadece spec + ASCII + pseudo-code (gerekirse).
