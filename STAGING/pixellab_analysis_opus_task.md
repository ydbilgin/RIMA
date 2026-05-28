# PixelLab Envanter — Opus Visual/Brand Coherence Analiz

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`

## ROL
Sen Opus reviewer (HARD `feedback_sonnet_default_opus_exception` — Opus reviewer + complex judgment). 243 PixelLab obje bağımsız analiz et — sadece visual/brand coherence. Codex/agy çıktısına BAKMA, ortak karar SONRA loop sentez aşamasında.

## Amaç
Her 243 obje için **bağımsız Opus verdict**: KEEP / DELETE / REVIEW. RIMA brand fit (Hades Elysium V1 wall-less floating, cyan glow), HIGH TOP-DOWN 3/4 angle, visual kalite judgment.

## Bağlam (envanter)
- Master doc: `STAGING/PIXELLAB_INVENTORY_MASTER.md` (243 obje + kategori + local cross-check)
- Bilinen LIVE brand: Hades Elysium V1 (memory `project_walless_v1_hades_elysium_lock`)
- HIGH TOP-DOWN 3/4 lock (memory `project_high_top_down_3_4_lock_2026_05_24`)
- Cyan #00FFCC focal motif (memory `project_yarik_3scale_language`)

## İş kalemleri

### 1. Her obje için verdict
Master doc'taki 243 obje listesini gez (kategori bazlı). Her biri için:
- `mcp__pixellab__get_asset(asset_id)` veya `mcp__pixellab__get_asset_preview(asset_id)` — görsel inceleme (preview image)
- Verdict: **KEEP** (LIVE asset, kullanılabilir) / **DELETE** (visual quality kötü veya off-brand) / **REVIEW** (belirsiz, kullanıcı kararı)

### 2. Kategori bazlı toplu değerlendirme
Bireysel verdict zor olabilir 243 obje için — pratik yol:
- Her kategori sample'ı incele (rastgele 3-5 obje)
- Kategori sample uyumlu mu Hades Elysium V1 brand'e
- Sample uyumsuzsa: tüm kategoriyi DELETE candidate
- Sample uyumluysa: tüm kategoriyi KEEP candidate (per-obje detay 2. tur)

### 3. Kritik kategoriler (öncelik)
- **weapons** (18 obje) — silah üretimi henüz başlamamış sanmıştık, halbuki 18 var. Hangi class için? Kullanılabilir mi?
- **skill_icons** (22 obje) — tamamen cloud-only, hiç import edilmemiş. UI için gerekli. Tutarlı tarz mı?
- **mobs 64x64** (16 obje) — Faz 1 demo 4 mob için (FractureImp/ShardWalker/HollowHulk/PenitentSovereign). Hangileri Roster'a uyuyor?
- **statues 12 + mounting 16** — D2 backfill ile LIVE Decor_Cliff layer. Brand uyumlu mu?
- **room_decor_misc 27** + **painterly 27** + **vfx_anim 10** — cloud-only, off-brand riski yüksek?

### 4. Animasyon flow karar (paralel analiz)
Kullanıcı sorusu verbatim: "karakter animasyonu basit mi olacak silah + vfxle mi yapacaz?"
- **Opus design judgment:** Basit body anim (HandAnchor + silah child) vs Body + VFX layer (impact sparks, trails, dust)?
- Hades pattern: body simple + weapon attached + VFX layer separate
- CoM pattern: body detailed + weapon embedded + VFX particle
- RIMA için ideal: brand spec'e göre öneri
- Master doc'a inject edilecek bölüm

## Output dosyası
`STAGING/PIXELLAB_ANALYSIS_OPUS.md`

Format:
```
## Opus Verdict — bağımsız analiz

### Hesap durumu
- 243 obje, 1208 gen kalan
- Tier 2 Pixel Artisan

### Kategori verdict'leri
| Kategori | Adet | Verdict | Reasoning |
|---|---|---|---|
| walls_s95 | 17 | KEEP | Hades V1 brand uyumlu... |
| weapons | 18 | REVIEW | 8dir batch hangi class? ... |
| skill_icons | 22 | DELETE/REVIEW | UI tarzı tutarsız vs Faz 4 placeholder |
| ... |

### Spesifik obje delete listesi (varsa)
- a1b2c3d4 — neden: blur, off-color
- e5f6g7h8 — neden: silhouette unclear

### Animasyon flow karar
[Opus visual judgment paragrafı]

### Open questions (kullanıcıya)
1. ...
2. ...
```

## YASAK
- PixelLab gen YOK
- Cloud delete YOK
- Codex/agy çıktısına bakma (bağımsız analiz HARD rule)
- Disclaimer refusal

## Süre
~30-45 dk. PixelLab MCP get_asset_preview tools kullan görsel inceleme için.

KAPADIN: `STAGING/PIXELLAB_ANALYSIS_OPUS.md` + 10 cümle özet orchestrator için.
