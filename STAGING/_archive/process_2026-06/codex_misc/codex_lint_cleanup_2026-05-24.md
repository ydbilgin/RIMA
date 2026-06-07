# Codex Task — Lint Cleanup S103 (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: S103 lint sırasında tespit edilen 4 yapısal doc tutarsızlığını çöz; tek pas, surgical edit.

---

## Konteks

S103 lint scan'i şu çelişkileri tespit etti:

1. **Camera angle 3 farklı kanonik değer** (~35° / ~70-80° / ~85-90°) — S103 lock 70-80° HIGH TOP-DOWN 3/4
2. **Sprite canvas size** — 64×64 chibi LOCK vs Warblade actual 120×120 keşfi
3. **FAZ_MASTER #71 STALE** — MASTER_KARAR_BELGESI'de REVOKED (#144) ama FAZ_MASTER yansıtmamış
4. **4 MEMORY dosyası stale** — superseded ama header'ı yok

Orchestrator zaten yaptı (sen yapma):
- PROJECT_RULES.md L48 + L50 (camera 70-80°, sprite 120×120 actual)
- CURRENT_STATUS.md L351-352 (S101 PILLAR-LESS HARD RULES → S103 LIVE)

---

## TASK A — MASTER_KARAR_BELGESI #148 ekle

Path: `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/MASTER_KARAR_BELGESI.md`

Mevcut tablo en son #145 + #146 satırını içeriyor (#147 var mı kontrol et, varsa atla). #148 yeni satır olarak ekle:

| # | Karar | Sonuç | Tarih |
|---|-------|-------|-------|
| 148 | **2026-05-24 Camera + Sprite Reconcile LOCK** | Camera ~35° (#45/#100/#113) ve ~85-90° (project_topdown_pivot_lock) lock'ları **REVOKED**. Yeni canonical: **HIGH TOP-DOWN 3/4 (~70-80° from horizon, Hades/CoM/D3 ref)**. Karar #114 reaffirm (8-dir LOCK). "True iso diamond" 2026-05-24 pivot terim hatası, REVOKED. **Sprite canvas:** Warblade actual 120×120 PNG verified — Karar #72/#74/#100 "64×64 chibi" canvas-tarafı REVISE: "chibi" tasarım intent terimi (proportion/styling), canvas boyutu farklı (Warblade 120, kalan class'lar pilot test sonrası karar). Wall ratio re-anchor: char 120 : wall 384 = 1:3.2 (Hades range). PixelLab S-XL non-square 256×768 / 340×768 native destek verified. PPU=64 LOCK korunur. Detay: `MEMORY/project_high_top_down_3_4_lock_2026_05_24.md` + `STAGING/codex_lint_cleanup_2026-05-24.md`. | 2026-05-24 |

Aynı zamanda eski karar satırlarına cross-ref ekle (sadece pasif annotation, satırı silme):
- Karar #36 satırına ekle sonuna: `(SUPERSEDED by #148 2026-05-24 — HIGH TOP-DOWN 3/4 70-80°)`
- Karar #45 satırına ekle sonuna: `(SUPERSEDED by #148 2026-05-24 — 70-80° HIGH TOP-DOWN 3/4)`
- Karar #100 satırına ekle sonuna: `(canvas tarafı SUPERSEDED by #148 — 120×120 actual, chibi = design term)`
- Karar #113 satırına ekle sonuna: `(SUPERSEDED by #148 2026-05-24 — 70-80° HIGH TOP-DOWN 3/4)`

---

## TASK B — FAZ_MASTER.md update

Path: `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/FAZLAR/FAZ_MASTER.md`

1. **Satır L32 update** (KARARLAR list):
   - Eski: `- **Silah Gorünürlük Single-State (Karar #71 LOCKED):** Silah hep elde, Ronin istisna (sheath/draw kimligi). Pixel art constraint + combat-readability. Detay: ../MAKEUP_BACKLOG.md + ../CINEMATIC_LAYER_v1.md`
   - Yeni: `- ~~Silah Gorünürlük Single-State (Karar #71)~~ **REVOKED by Karar #144 (2026-05-16) + #146 (2026-05-18):** Body weaponless + Weapon Child SR + Puff system. Detay: MASTER_KARAR_BELGESI #144/#146.`

2. **MASTER_KARAR_BELGESI SENKRONU tablosunda L190 satırı**:
   - Eski: `| #71 | Silah Single-State: hep elde, Ronin sheath/draw istisna | Faz 1+ |`
   - Yeni: `| ~~#71~~ | ~~Silah Single-State~~ REVOKED by #144/#146 | Faz 1+ |`

3. **Aynı tabloya 3 yeni satır ekle (#135 sonrası)**:
```
| #144 | Karakter weaponless + Weapon Child SR (Karar #71/#73 OVERRIDE) | Faz 1+ |
| #146 | Weapon Visibility Input-Driven Puff System (#71+#144 unify) | Faz 1+ |
| #147 | Multi-Layer Painter System (RoomTemplate List<BackgroundLayerData>) | Faz 1+ |
| #148 | 2026-05-24 Camera + Sprite Reconcile (HIGH TOP-DOWN 3/4, 70-80°, sprite 120×120 actual) | Tüm fazlar |
```

4. **Satırın altındaki "Durum:" satırını güncelle**:
   - Eski: `**Durum: FAZ_MASTER tablosu sadece Faz-impact özet. #72-#135 canonical kararlar icin ../MASTER_KARAR_BELGESI.md referans. Son guncelleme 2026-05-15 S77.**`
   - Yeni: `**Durum: FAZ_MASTER tablosu sadece Faz-impact özet. #72-#148 canonical kararlar icin ../MASTER_KARAR_BELGESI.md referans. Son guncelleme 2026-05-24 S103.**`

---

## TASK C — MEMORY SUPERSEDED headers (4 dosya)

Her dosyanın en üstüne (frontmatter YAML'den sonra, içeriğin başına) SUPERSEDED uyarı bloku ekle. Frontmatter ve mevcut içeriği KORU.

### C1 — `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_topdown_pivot_lock.md`

Frontmatter'dan sonra (ilk `#` heading öncesi) ekle:

```
> **⚠️ SUPERSEDED 2026-05-24 by [[project-high-top-down-3-4-lock-2026-05-24]]**
> Camera angle ~85-90° değer GÜNCEL DEĞİL. Yeni canonical: ~70-80° HIGH TOP-DOWN 3/4 (Karar #114 reaffirm + Karar #148). Bu dosya history için tutulur.
```

### C2 — `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_modular_pipeline_lock.md`

Frontmatter'dan sonra ekle:

```
> **⚠️ PARTIAL STALE 2026-05-24**
> "Wang autotile deprecated for RIMA" iddiası GÜNCEL DEĞİL — Karar #118 Corner Wang Pipeline + #131 + #135 ile Wang reaktive edildi. Modular asset pipeline ana iskelet (Mod B + manual transition + n_frames) hâlâ LIVE.
```

### C3 — `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_painter_consolidation_lock.md`

Frontmatter'dan sonra ekle:

```
> **⚠️ SUPERSEDED 2026-05-24**
> RimaWorldPainterWindow primary iddiası GÜNCEL DEĞİL — Karar #132 (RimaMapDesignerWindow) + #134 (Procedural Pivot) + #147 (Multi-Layer Painter) ile painter ecosystem değişti. JSON map loader Phase H statüsü de revize gerekli.
```

### C4 — `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_wall_production_pipeline_s99_late.md`

Frontmatter'dan sonra ekle:

```
> **⚠️ SUPERSEDED 2026-05-24 by [[project-pillar-seam-cover-lock-2026-05-24]]**
> S99 modular kit overlap (1.59 unit spacing seamless) bilgisi history. S100 → S101 PILLAR-LESS → S102 → S103 PILLAR SEAM-COVER zincirinde 2 katman geriden. LIVE: pillar seam-cover production.
```

---

## TASK D — S103 close memory yaz

Path: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_s103_session_close_2026_05_24.md`

İçerik:

```markdown
---
name: s103-session-close-2026-05-24
description: "S103 close — 16 skill ekosistem, HIGH TOP-DOWN 3/4 reaffirm, sprite 120×120 keşfi, wall production iter, pillar seam-cover LIVE, lint cleanup."
metadata: 
  node_type: memory
  type: project
  originSessionId: e90c4fd4-80b4-4597-843b-c5a7999e710f
---

# S103 Close — Skill Ecosystem + Wall Iter + Lint Cleanup (2026-05-24)

## Major decisions

| # | Karar | Memory |
|---|---|---|
| Karar #148 | Camera + Sprite Reconcile (70-80° HIGH TOP-DOWN 3/4 + Warblade 120×120 actual) | [[project-high-top-down-3-4-lock-2026-05-24]] |
| LIVE | Pillar seam-cover production (S101 PILLAR-LESS REVOKED) | [[project-pillar-seam-cover-lock-2026-05-24]] |
| HARD | $imagegen Codex built-in tool mode (CLI YASAK) | [[feedback-codex-imagegen-skill-not-env-var]] |
| HARD | cx_dispatch --effort xhigh default | [[feedback-codex-effort-xhigh-2026-05-24]] |
| LIVE | Walls — NO banner baked + doorway empty void + torch separate prop | (CURRENT_STATUS HARD RULES) |

## Skill ecosystem expansion

16 skill aktif: brainstorming, find-skills, flux-2-klein, gpt-image-2, graphify, humanizer, image-inpainting, image-outpainting, impeccable, rima-conventions (custom), skill-creator, subagent-driven-development, systematic-debugging, unity-mcp-skill, verification-before-completion, write-a-skill.

Custom skill yazıldı: `$rima-conventions` (path: `~/.claude/skills/rima-conventions/`). 7/7 test PASS, production-ready proactive trigger.

Agent Sprite Forge sandbox keşfi (background dispatch byqk9xro3 tamamlandı close anında — değerlendirme S104).

## Wall production iter (ratio epey ileri-geri)

128×96 → 128×192 → 128×256 → 128×768 (PixelLab limit) → **FINAL 128×384** (HIGH TOP-DOWN 3/4 lock + 1:3.2 char ratio Hades range)

- N-Landmark setpiece: 256×384 (2 tile wide dramatic)
- Pillar universal: 64×384 (wall ile aynı yükseklik)
- 14 mevcut raw 1024×1024 yeniden downscale
- 3 yeni raw gen: NW-broken + NE-broken + pillar
- Background dispatch bdkrtgasb tamamlandı close anında — 17 final asset QC bekliyor (S104).

## Lint cleanup yapıldı (S103 close)

- `PROJECT_RULES.md` — camera 85-90° → 70-80°, sprite 64×64 → 120×120 actual canvas notu
- `CURRENT_STATUS.md` — S101 PILLAR-LESS HARD RULES → S103 LIVE
- `MASTER_KARAR_BELGESI.md` — Karar #148 eklendi, #36/#45/#100/#113 SUPERSEDED annotation
- `FAZ_MASTER.md` — #71 REVOKED işareti, #144/#146/#147/#148 satırları eklendi
- 4 stale memory file SUPERSEDED header eklendi

## Next session (S104) — NEXT BIG STEPS

1. Wall dispatch bdkrtgasb QC: 17 asset HIGH TOP-DOWN 3/4 uyumu doğrula
2. Sprite Forge sandbox sonucu değerlendir: production'a entegre vs sandbox stay
3. Unity import + iso grid setup + 1 test oda (manual placement)
4. Custom Wall Brush Tool yazımı (EditorWindow + snap + auto-pillar)
5. 5 benzersiz oda compose + Warblade char
6. Opus QC final (chatgpt_ref benzerlik verdict)

## How to use

Bu memory S103 → S104 hand-off için kanonik. S104 başlangıcında bu dosya + CURRENT_STATUS + PROJECT_RULES yeterli context.
```

---

## Doğrulama (PASS criteria)

Codex execution sonunda doğrula:
1. MASTER_KARAR_BELGESI.md son satırı `| 148 |` ile başlıyor ve 4 eski karar (#36/#45/#100/#113) SUPERSEDED annotation içeriyor
2. FAZ_MASTER.md `| #144 | ... | Faz 1+ |` satırı var
3. 4 memory dosyasının en üstünde ⚠️ SUPERSEDED veya ⚠️ PARTIAL STALE bloku var
4. project_s103_session_close_2026_05_24.md mevcut

git commit otomatik yapma — orchestrator review sonrası karar verir.
