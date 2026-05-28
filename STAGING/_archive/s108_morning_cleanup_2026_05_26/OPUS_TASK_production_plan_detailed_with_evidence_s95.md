# Opus Task — Production Plan Detailed + Evidence-Backed (S95)

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Output:** `STAGING/PRODUCTION_PLAN_DETAILED_v1.md`
> **Mode:** Comprehensive üretim planı dosyası. Her batch için: tool + params + EXACT prompt + geçmiş referans örnek + beklenen output + bütçe. Kanıt katmanı zorunlu.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "Önceki örneklerde şu promptu vermişim şu çıkmış gibi kanıtlı şekilde detaylı dosya yazıp obje üretim planı dosyası yaz. Codex'e review ettir o dosyayı ben bi de antigravity'e review ettireyim. mantıklı şekilde planı oluşturalım. tam kanıtlı detaylı yapsın."

## Hedef

`STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`'deki **9 karar + 3 template** ve `STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md`'deki **4-faz konuşması** birleşik bir EVIDENCE-BACKED üretim planına dönüştür.

**Kanıt katmanı:** Her batch için **geçmiş PixelLab obje örneği** referansla (object_id, prompt formülü ipucu, çıktı görsel, kalite gözlemi). Yeni batch'in benzer kalitede çıkacağını argümante et.

## Kapsam

### 4 Faz × her batch için zorunlu alanlar

Her batch entry'sinde:

1. **Tool + Parameters** (exact)
   - MCP tool adı (`mcp__pixellab__create_object` / `create_isometric_tile` / `create_object_state` / `create_tiles_pro`)
   - size, view, directions, n_frames, object_view, item_descriptions vb.

2. **Exact Prompt Text**
   - Master spec Karar #7 formülüne uygun: `[GEOMETRY], [MATERIAL+name+HEX], [DETAIL+HEX], [PERSPECTIVE+CANVAS OCCUPANCY], [STYLE ANCHOR], [BACKGROUND]`
   - Genre label YASAK, "Hades-style" YASAK (3rd-party name), color HEX zorunlu
   - n_frames=4+ ise item_descriptions array tam liste

3. **Reference / Evidence (Kanıt)**
   - `mcp__pixellab__list_objects` ile geçmiş envanter çek (limit 50, multiple calls if needed, tag filter dene)
   - Aynı kategoride üretim varsa: object_id + adı + size + view + çıktı görsel açıklama
   - **Format:** "Object 65c99904 — 'Act 1 Shattered Keep wall' — size=128, view=n/a default. Çıktı: 4×4 stone tile (flat top-down, NOT side wall). **Bizim batch'te fark:** view='side' explicit → Hades dikey wall face beklenir."
   - Referans yoksa: "İlk üretim, kanıt yok. Pilot risk, küçük batch öneri."

4. **Beklenen Output**
   - Frame count, dimensions, transparent BG, palette uyum
   - Review mode (n_frames=4+) için: 4 candidate, seçilen 1-2 finalize

5. **Bütçe + Sıra**
   - Reserve gen (master spec Karar #9'dan)
   - Faz içinde sıra (1.1, 1.2, vb.)

6. **Risk + Pilot Strategy**
   - "Drift varsa Plan B" — örn n_frames=4 başarısızsa 4 ayrı n_frames=1 dispatch
   - `item_descriptions` field MCP wrapper'ında çalışıyor mu (master spec implementation caveat)

### 4 Faz Outline

**Faz 1 — Demo MVP Asset (görsel komple, mevcut chars yeterli)**
- 1.1 Wall Face Pack (Template A — 128 n_frames=4)
- 1.2 Wall Damaged Variants (state_of veya yeni batch — master spec Karar #5)
- 1.3 Wall Mountings (Template C — 64 n_frames=16)
- 1.4 Floor Clutter (Template B — 32 n_frames=64, item_descriptions 16 base × 4 variant)
- 1.5 Void Flame (64 n_frames=4 view=side, 3 state Act 1 cyan)
- 1.6 Interior Ruined Pieces (128 n_frames=4 — geometry değişim, yeni batch)

**Faz 2 — Karakter & Mob (V3 web UI, USER manual)**
- 2.1-2.5: PixelLab MCP dispatch YOK. Sadece V3 web UI prompt formülleri + state listesi (memory `feedback_character_state_planning_before_production` HARD LOCK)
- Her karakter/mob için: state listesi ÖNCE (idle/walk/attack/skill/variant)
- User onay gate
- Bütçe: V3 ayrı pricing, RIMA gen budget dışı

**Faz 3 — VFX & Animasyon**
- 3.1 Void Flame flicker (`animate_object`, Faz 1.5 state'lerinden)
- 3.2 Slash VFX (64 n_frames=4 anim grid)
- 3.3 Dust/Spark VFX (32 n_frames=16)
- 3.4 Decor Silhouette idle (opsiyonel, `animate_object`)

**Faz 4 — UI Polish & Detay**
- 4.1 Item Icons (32 n_frames=64)
- 4.2 HUD Elements (gpt-image-1 / Codex imagegen, PixelLab DIŞI)
- 4.3 Boss Prep (128 n_frames=4)

## Süreç (Opus ⇆ Codex Döngü)

### Adım 1 — PixelLab Envanter Çek

`mcp__pixellab__list_objects` çağrı(lar)ı ile geçmiş üretim envanterini al:
- Limit 50 (max), gerekirse offset'le multiple calls
- Tag filter dene: `act1_wall_pieces_s95`, `rift_obj_s94`, `decor_silhouette`, vb.
- Her object için: id, name, status, size, view, tags
- Görsel inceleme: en yakın referans için `get_object(object_id, include_preview=true)` ile preview al, kalite gözlemini not et

**Önemli referans objeler (orchestrator zaten gördü):**
- `65c99904-12b8-4b98-9e5f-fe2f280f6a2f` — Act 1 wall stone block (128px, view=n/a default, flat top-down çıktı)
- `b340684f-552b-49e6-a281-ab360d376564` — Act 1 isometric floor tile (16 tile batch, S95 LATE çıktı, PathC_BaseTest'te paint edildi)

### Adım 2 — Plan Dosyası Yaz

`STAGING/PRODUCTION_PLAN_DETAILED_v1.md`:
- Master spec linkleri başta (cross-ref temiz)
- 4 faz, her batch için yukarıdaki 6 zorunlu alan
- Mantıksal akış: önce envanter özeti, sonra faz-faz batch'ler, son toplam bütçe + risk listesi

### Adım 3 — Codex Review Dispatch

`cx_dispatch.py` ile Codex'e gönder:
- Codex task file: `STAGING/CODEX_TASK_production_plan_review_v1.md`
- Codex'in soruları:
  - PixelLab API constraint check (size, view, n_frames sınırları doğru kullanılmış mı)
  - `item_descriptions` field MCP wrapper'ında forward ediliyor mu (master spec implementation caveat tekrar)
  - Geçmiş referanslar yeterli mi (cherry-pick mi, comprehensive mi)
  - Prompt formülü Karar #7'ye uyumlu mu (genre label yasak, HEX zorunlu)
  - Bütçe hesabı master spec Karar #9 ile uyumlu mu
- Output: `STAGING/CODEX_DONE_production_plan_review_v1.md`

### Adım 4 — Opus Revize

Codex feedback üzerinden plan dosyasını güncelle (inline edit).

### Adım 5 — Final Pass

Tek iter Codex review yeterli (master spec zaten LIVE). Aşırı iter YASAK — bu plan dosyasının ana iş değil envanter aggregation + spec application. Max 2 iter.

### Adım 6 — User'a Antigravity Hazırlık

Final plan dosyası yazıldıktan sonra orchestrator (Sonnet) Antigravity (Gemini 3.5 Flash) review prompt'unu hazırlayacak. Sub-agent bu adımı YAPMAZ — sadece plan dosyasını hazır eder.

## Çıktı Format (`STAGING/PRODUCTION_PLAN_DETAILED_v1.md`)

```markdown
# RIMA Production Plan Detailed — v{N}

## Metadata
- Date: 2026-05-20
- Master spec: STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md
- Wall+Object plan: STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md
- Budget remaining: ~2,500 PixelLab gen

## PixelLab Envanter Özet
- Total objects: N
- By category:
  - Wall pieces (tag act1_wall_pieces_s95): X objects
  - Floor tiles (tag floor_iso_s95): X objects
  - Rift objects (tag rift_obj_s94): X objects
  - Mob silhouettes (decor): X objects
- Key reference objects (visual inspection):
  - 65c99904 — wall block 128px (flat top-down, view default issue)
  - b340684f — floor tile batch (isometric, LIVE paint)
  - ... (other key refs)

## Faz 1 — Demo MVP Asset (~190-225 gen reserve)

### Batch 1.1 — Wall Face Pack (Template A)
#### Tool + Parameters
- Tool: mcp__pixellab__create_object
- Params: size=128, view="side", directions=1, n_frames=4, object_view=None
- item_descriptions: [
    "wall face north view, granite #3A3D42 base, cyan #00FFCC mineral veins in cracks, weathered mortar lines, single facet from side perspective, tall vertical wall billboard fills canvas height, painterly pixel art, isolated on transparent background, no outline",
    "wall face east view, ... (perpendicular facet, 90° from north)",
    "outer corner wall piece, two facets meeting at 90 degrees, both visible, granite weathered cyan veins",
    "arched doorway opening, rough archway through wall, granite arch with cyan vein highlight, vertical wall billboard"
  ]

#### Exact Prompt (main description)
"Act 1 Shattered Keep stone wall pieces, granite #3A3D42, cyan #00FFCC mineral accents, painterly pixel art, isolated transparent background"

#### Reference / Evidence
- **Object 65c99904** — "Act 1 Shattered Keep ancient ruined stone keep wall" — size=128, view=n/a (default).
  - Çıktı görsel: 4×4 grid stone tile, **flat top-down** (zemin gibi), NOT Hades-style dikey wall face.
  - **Sebep:** view parameter eksik veya default'tu, low top-down çıktı.
  - **Bizim batch'te fark:** view="side" explicit + item_descriptions'da "tall vertical wall billboard fills canvas height" cue → Hades dikey wall face beklenir.
- Pilot risk: ilk side-view batch, kanıt yok. n_frames=4 review mode safety net.

#### Beklenen Output
- 4 candidate, status="review"
- Her frame: 128×128, transparent BG, granite + cyan, side perspective
- select_object_frames ile 2-3 finalize, gerisi dismiss

#### Bütçe + Sıra
- Reserve: 40 gen (master spec Karar #9, range 25-40)
- Sıra: 1.1 (Faz 1'in ilk batch'i)

#### Risk + Pilot Strategy
- `item_descriptions` MCP wrapper'da forward ediliyor mu — pilot doğrulayacak
- Drift varsa Plan B: 4 ayrı n_frames=1 dispatch (face_NS, face_EW, corner, arch ayrı)
- Stil tutarlılığı: tek batch'te 4 frame style coherence beklenir (master spec Karar #4)

---

### Batch 1.2 — Wall Damaged Variants
{aynı format}

### Batch 1.3 — Wall Mountings (Template C)
{aynı format}

...

## Faz 2 — Karakter & Mob (V3 web UI USER)

### 2.1 — Warblade State List + V3 Prompts
#### State Listesi (üretim ÖNCESİ zorunlu, memory HARD LOCK)
- idle_S, idle_SE, idle_E, idle_NE, idle_N (5 produce + 3 mirror)
- walk_S, walk_SE, walk_E, walk_NE, walk_N
- attack_strike_S, attack_strike_E (sword vertical, sword horizontal)
- hit_react, death
- rage_burst (class signature, Karar locked)
- 6 cross-class skill anchor (Karar #79)

#### V3 Web UI Prompt Template
{prompt text — user web UI'da kullanır}

#### Reference
- Anchor PixelLab character_id (canonical roster v2): Warblade `2656075d`
- State anchor: `idle_S` Karar #100 base

...

## Faz 3 — VFX & Animasyon (~85-125 gen)
...

## Faz 4 — UI Polish & Detay (~70-90 gen)
...

## Toplam Bütçe Plan
| Faz | Reserve | Cumulative | % of 2,500 |
|---|---|---|---|
| Faz 1 | 190-225 | 225 | 9% |
| Faz 2 | 0 (V3) | 225 | 9% |
| Faz 3 | 85-125 | 350 | 14% |
| Faz 4 | 70-90 | 440 | 18% |
| **Total** | **~345-440** | **440** | **~18%** |

Marj: kalan ~%82 (≈2,060 gen) Act 2 / Act 3 / iterasyon için.

## Pilot Strategy (Master Spec Implementation Caveat)
- İlk batch (1.1 Wall Face Pack) **pilot** — `item_descriptions` field forward + side-view quality doğrula
- Pilot başarısızsa Plan B/C dispatch (master spec Karar #8)
- Pilot başarılıysa Faz 1.2-1.6 production scale up

## Açık Sorular (User Antigravity review için)
- {Soru 1: pilot batch sonra mı tüm Faz 1, yoksa pilot + 2-3 ayrı küçük batch?}
- {Soru 2: Faz 2 V3 web UI Warblade state listesi 23 madde — bu ölçek ilk karakter için aşırı mı, MVP'de daha küçük set mi?}
- {Soru 3: Faz 3 VFX animate_object kalite kanıtı yok, pilot küçük batch öneri var mı?}

## Codex Review Excerpts
{Codex feedback özetleri}
```

## Hard Constraints

- **PixelLab dispatch YASAK.** Sadece list_objects + get_object (preview) — kanıt için. Yeni üretim YOK.
- **Karpathy 4 inline.**
- **NLM ACCESS:** `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Codex dispatch:** Background DEĞİL — bekle, oku, devam et.
- **Max 2 iter.** Aşırı uzarsa BLOCKED yaz.
- **Geri dönülebilir:** Sadece dosya yazma, kod/asset/scene değişimi yok.

## Orchestrator'a Final Rapor

- list_objects toplam kaç obje çekildi
- Her faz için kaç batch, toplam gen reserve
- Codex review verdict + revize edilen kararlar
- User Antigravity review için açık sorular
- Pilot dispatch önerisi (ilk gerçek üretim)
