# Opus Task — Object Üretim Master Spec (Opus + Codex döngü)

> **Owner:** rima-design (Opus)
> **Partner:** Codex via `cx_dispatch.py`
> **Output:** `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`
> **Mode:** Spec only. PixelLab dispatch ETMEZ. Kod yazmaz. Sadece kararlar + prompt formülleri.
> **Final review:** User Gemini 3.5 Flash'a ayrı session'da son review yaptıracak.

## Bağlam

User'ın direktifi (S95 LATE NIGHT 2026-05-20):
> "create tiles pro ile tile üretilmiyor mu? objede pixeli verip descriptiona exact verebilirsin. 65c99904-12b8-4b98-9e5f-fe2f280f6a2f bu 128piksel ama düzgün şekilde duruyor. 128pikselde 4 çeşit yapabiliyoruz, 64te 16, 32 piksel 64 tane. obje üretirken her zaman grupla mantıksal olarak gruplamalısın. opusa codexe review ettir son kararı getir."

## Referans Object İncelemesi

**ID:** `65c99904-12b8-4b98-9e5f-fe2f280f6a2f`
- size: 128×128, directions=1, view: n/a (default)
- Görsel: 4×4 grid taş döşeme, **flat top-down** floor block (Hades-style dikey wall DEĞİL)
- Tags: `act1_wall_pieces_s95`
- Çıkarım: View parametresi verilmezse default top-down. Hades-style side-face wall için `view: "side"` zorunlu.

## Tartışılacak Kararlar (Opus → Codex review döngü)

### 1. L2a Wall Base — Hangi Tool?

Production Plan: `create_isometric_tile` + thickness 0.15

**Alternatifler:**
- (a) `create_isometric_tile` size=64, tile_shape="thin tile" (~10%)
- (b) `create_tiles_pro` tile_type=isometric, tile_size=64, tile_view_angle=90 (flat), 4 numbered tile
- (c) `create_object` size=64, view=default → 65c99904 gibi flat block

**Opus karar ver:** Hangi tool L2a için, neden? L2a "footprint + isometric perspektif altlık" — collider source.

### 2. L2b Wall Face — Hangi Tool?

Production Plan: `create_object` tall sprite 64×128 (mümkün değil, square constraint)

**Alternatifler:**
- (A) `create_object` size=128 square, view="side", directions=1, n_frames=4 → 4 candidate review mode
- (C) `create_tiles_pro` tile_type=square_topdown, tile_size=64, tile_height=128, tile_view_angle=0 → non-square 64×128
- (Hibrit) Face → tiles_pro (style consistency), corner+arch → object (detay)

**Opus karar ver:** Hangi yol, sebep ne? n_frames=4 candidate vs ayrı dispatch trade-off?

### 3. Size + n_frames Stratejisi (Genel Kural)

User'ın mantığı doğrulansın:

| Object size | Recommended n_frames | Total grid | Kategori örnekleri |
|---|---|---|---|
| 32 | 64 (8×8) | Yere konan ufak şeyler | potion, key, gem, coin, rune, small_skull, candle_stub |
| 64 | 16 (4×4) | Orta items + mob silhouette | chest, barrel, small_statue, void_flame_torch, decor_silhouette_X |
| 128 | 4 (2×2) | Büyük items | wall_face, throne, altar, big_statue, archway, sarcophagus |

**Opus karar ver:** Bu tablo doğru mu, eksik kategori var mı? Sınırlar kesin mi (örn 64'te 8 candidate da olur, n_frames=8 var mı)?

### 4. Grouping Kuralı (Mantıksal Bundle)

Tek dispatch'te benzer item'ları grupla. Örnekler:

**32px batch (n_frames=64):**
- "1). gold coin pile 2). silver key 3). red potion vial 4). blue mana orb 5). ancient rune stone... (64 numbered)"
- Tüm dungeon clutter tek dispatch'te, stil tutarlı

**64px batch (n_frames=16):**
- Wall mounting collection: "1). iron sconce holder 2). chain hook 3). wall pennant 4). torch bracket... (16 numbered)"
- Veya decor furniture: "1). wooden chest 2). oak barrel 3). stone urn..."

**128px batch (n_frames=4):**
- Wall pieces: "1). wall face north view 2). wall face east view 3). corner piece 4). arch opening"
- Stil consistency tek dispatch'te kritik

**Opus karar ver:** Grouping cluster sayısı limit ne? (Aynı dispatch'te 64 farklı şey beklemek prompt drift yapar mı?) Numbered prompt'ta zarif bölünme stratejisi.

### 5. state_of vs Yeni Object

**state_of:** Mevcut object'in variant'ı (intact + damaged + mossy aynı `group_id`, edit_description ile küçük değişiklik).

**Yeni object:** Bağımsız üretim, group yok.

Önerim:
- **state_of kullan:** wall_face_NS_intact → state(damaged) → state(mossy). Stil identical, sadece overlay edit.
- **Yeni object:** wall_face_NS vs wall_face_EW (farklı perspektif, ayrı üretim mantıklı)

**Opus karar ver:** Bu doğru mu? state_of'un sınırı ne?

### 6. View Parametresi

`create_object` view seçenekleri: "low top-down" / "high top-down" / "side"

| Use case | View | Sebep |
|---|---|---|
| L2a flat floor tile | (n/a, default) veya "low top-down" | 65c99904 örneği, üstten flat |
| L2b dikey wall face | **"side"** | Hades-style, duvar dik yüzü kameraya bakar |
| Floor clutter (potion, rune) | "low top-down" | Yatay perspektif |
| Mob silhouette | "low top-down" | Karakter perspektifi |
| Tall prop (statue, altar) | "side" veya "high top-down" | Yükseklik vurgu |

**Opus karar ver:** Bu mapping doğru mu? `object_view` ek parametresi nasıl kullanılır?

### 7. Description Prompt Format

PixelLab prompt'unda **exact spec** vermek mümkün (size+description). Memory `feedback_negation_to_positive_prompts`:
- Positive RIMA specs zorunlu (1:3 negation ratio)
- Genre label YASAK ("dark fantasy" yok)
- Pozisyon anlatımı OK ("right hand" YASAK, "shield arm" OK)
- Pure RIMA visual descriptor

**Örnek wall face prompt:**
```
ancient stone keep wall facing south, granite #3A3D42 base, cyan #00FFCC mineral veins in cracks, weathered mortar lines, single dark facet from side perspective, isolated on transparent background, painterly pixel art, no outline
```

**Opus karar ver:** Bu prompt formülü iyi mi? "Side perspective" yeterli mi, yoksa "Hades-style tall wall billboard" eklenmeli mi?

### 8. 4-Piece Wall Batch (n_frames=4) Stratejisi

Tek dispatch'te 4 piece (face_NS + face_EW + corner_outer + arch) stil tutarlı üretimi.

**Numbered prompt:**
```
1). wall face south, granite weathered, cyan vein highlight, single facet from side view
2). wall face east, granite weathered, cyan vein highlight, perpendicular facet
3). outer corner piece, two facets meeting at 90 degrees, both visible
4). arched doorway opening, single rough archway through wall
```

Sonra `select_object_frames` ile seçim, dismiss et stil farklılarını.

**Opus karar ver:** Bu strateji çalışır mı? n_frames=4'te 4 farklı piece ister gerçekten farklı çıkar mı, yoksa aynı'nın varyantları mı?

### 9. Bütçe & Trade-off

Mevcut bütçe: ~2,500 PixelLab gen kaldı (5,000 total).

**Hesap:**
- Wall full set (face_NS + face_EW + 2 corner + end_cap + arch + damaged variants): ~16-20 piece
- 128px n_frames=4 batch = 1 dispatch ≈ 25 gen
- Tüm wall set = 4-5 batch dispatch = 100-125 gen
- Void Flame (3 state × Act 1) ≈ 20 gen
- Floor props (32/64 mixed batch) ≈ 80 gen
- **Total wall+void+props ≈ 220-250 gen** → bütçeden rahatlık var

**Opus karar ver:** Bütçe makul mü? Hangi batch'ler önce, hangi sıra?

## Süreç (Opus ⇆ Codex Döngü)

1. **Iter 1 — Opus tasarım:** 9 karar başlığını cevapla (tablo + 1-2 paragraf rasyonel her biri için)
2. **Iter 1 — Codex review dispatch:** `cx_dispatch.py` ile
   - Codex task: "Bu spec'i PixelLab API constraint'lerine karşı doğrula. n_frames sınırı doğru mu, view parametresi mapping mantıklı mı, prompt formülü PixelLab'in bilinen Discord/docs best practice'iyle uyumlu mu, bütçe hesabı gerçekçi mi?"
   - Done: `STAGING/CODEX_DONE_object_production_master_review_v1.md`
3. **Iter 2 — Opus revize:** Codex feedback üzerinden düzelt
4. **Iter 2 — Codex re-validate:** "Revize spec implementable mı, açık tartışma noktası kaldı mı?"
5. **Iter 3 (opsiyonel) — STOP** Codex onay verdiğinde
6. **Aşırı uzarsa (>3 iter veya >90 dk):** BLOCKED yaz, kalan açık sorular enumerate, durdur

## Çıktı Format — `STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`

```markdown
# Object Üretim Master Spec — DRAFT v{N}

## Verdict
{LIVE / NEEDS_USER_INPUT / BLOCKED}

## Iter Log
- v1: Opus draft → Codex review (4 LIVE, 5 NEEDS_REVISION)
- v2: Opus revize → Codex re-review (PASS)

## Master Kararlar
### 1. L2a tool: create_isometric_tile
### 2. L2b tool: create_object 128×128 + n_frames=4
### 3. Size × n_frames Matrix
### 4. Grouping Kuralı
### 5. state_of vs Yeni Object
### 6. View Parametresi Mapping
### 7. Description Prompt Formülü
### 8. 4-Piece Wall Batch
### 9. Bütçe Plan

## Prompt Templates (Ready-to-Use)
### L2b wall face batch (128px n_frames=4)
{exact prompt text}

### Floor clutter batch (32px n_frames=64)
{exact prompt text}

### Wall mounting batch (64px n_frames=16)
{exact prompt text}

## Codex Review Excerpts
{key feedback quotes}

## Açık Sorular (Gemini 3.5 Flash review için)
- {Soru 1}
- {Soru 2}
```

## Hard Constraints

- **PixelLab MCP dispatch YASAK.** Sadece spec — gerçek üretim user tarafından (V3 web UI veya MCP) onay sonrası yapılır.
- **Karpathy 4 inline.**
- **NLM ACCESS:** `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
- **Codex dispatch:** Background DEĞİL — bekle, oku, devam et.
- **Geri dönülebilir:** Hiç kod, hiç asset, hiç prefab değişikliği. Sadece spec.

## Orchestrator'a Final Rapor

- Kaç iter, verdict
- Hangi 9 karar Codex review'dan geçti
- User'a (Gemini 3.5 Flash review için) açık sorular
- Önerilen ilk dispatch (eğer onaylanırsa) — örn "wall face batch test, 25 gen"
