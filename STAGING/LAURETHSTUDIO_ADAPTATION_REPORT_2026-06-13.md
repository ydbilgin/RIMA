# LaurethStudio Adaptasyon Raporu — RIMA Düzeni → Stüdyo-Evrensel (2026-06-13)

> **Amaç:** (1) RIMA repo'sunda somut, önceliklendirilmiş bir temizlik planı çıkarmak; (2) RIMA sürecinde evrimleşen taşınabilir düzeni `F:/LaurethStudio/`'ya aktarmak için KOPYALA-GEÇ / UYARLA / YENİDEN-KUR ayrımını netleştirmek; (3) yeni session'ın sıfır bağlamla uygulayabileceği numaralı talimat vermek.
>
> **Kaynaklar (üçü de tam okundu):** `STAGING/_process/2026-06/_index_rima_structure_RESULT.md` (1075 satır, somut envanter), `STAGING/LAURETHSTUDIO_PLAYBOOK_EXTRACTION_2026-06-13.md` (kavramsal playbook), `project_laureth_studio_master_plan.md` (mevcut stüdyo yapısı), inline NLM çapraz-kontrol bulguları.
>
> **Envanter istatistiği (kanıt tabanı):** .claude/ = 48 dosya · global skill = 25 · user-memory = 315 dosya · repo MEMORY/ = 82 dosya · STAGING üst-seviye = 536 dosya · toplam 1006 öğe.
>
> **KRİTİK ÖN-BULGU — `F:/LaurethStudio/` SIFIRDAN KURULMAYACAK, ZATEN VAR.** `project_laureth_studio_master_plan.md` net: stüdyo klasörü mevcut, kendi `00_RULES/STUDIO_CONSTITUTION.md` (55+ STUDIO_KARAR), `02_GAMES/` (CircuitBreaker, Caterpillar, FracturedFarm, HeirloomTractor, Paravan), `03_IDEAS/`, `05_RESEARCH/`, `MEMORY/`, `CURRENT_STATUS.md` yapısına sahip. Bu rapor **mevcut stüdyoya orkestra-disiplini katmanı eklemeyi** hedefler — yeni iskelet kurmayı DEĞİL. Bölüm 3 bu çelişmemeyi açıkça işler.

---

## BÖLÜM 1 — ÖNCE RIMA DÜZENİ (bu repo'da temizlik; orchestrator UYGULAYACAK, kullanıcı onayı sonrası)

Tüm aksiyonlar **taşı/arşivle**, **silme minimumda** (git history zaten koruyor; envanterdeki "asla silinmez, taşınır" konvansiyonu geçerli). Sıra = etki/risk oranına göre.

### 1.A — `.claude/` bayat codex_*.txt / betik arşivi (DÜŞÜK RİSK, YÜKSEK KAZANÇ)

48 dosyalık `.claude/` kökünde ~30 tek-seferlik prompt/betik birikmiş. Bunlar artık `cx dispatch` (CodexAuthManager) ile değiştirilmiş eski file-based Codex workflow'unun kalıntıları. **Öneri konum: `.claude/_archive/`** (yeni klasör; git-tracked kalır).

| Dosya | Aksiyon | Gerekçe |
|---|---|---|
| codex_anchor_prompt.txt, codex_anim_guides_prompt.txt, codex_anim_prompt_update.txt | ARŞİV | Eski Codex anchor/anim promptları; cx dispatch header'ı bunların yerini aldı |
| codex_arch_review_prompt.txt, codex_brush_prompt.txt, codex_brush_task.md | ARŞİV | Tamamlanmış tek-seferlik görev/review promptları |
| codex_chibi_lore_review_*.txt (prompt+output), codex_concept_art_f1_2_prompt.txt | ARŞİV | Stress-test çıktıları; tarihsel, aktif değil |
| codex_f2_t1t2_prompt.txt, codex_ghost_attack_review_prompt.txt, codex_open_vista_review_prompt.txt | ARŞİV | Eski faz/review promptları |
| codex_pixellab_video_synthesis_prompt.txt, codex_roomdesigner_f1_*.txt, codex_s60_design_bundle_review_prompt.txt | ARŞİV | Tek-seferlik üretim/review |
| codex_scene_gen_prompt.txt, codex_sprite_qc_prompt.txt, codex_t3a_prompt.txt, codex_t3b_prompt.txt, codex_tiled_task.txt, e1e2_codex_prompt.txt, terrain_blend_prompt.md | ARŞİV | Tamamlanmış görev promptları |
| run_codex_brush.py, run_codex_f1.sh, run_codex_f1_compact.sh, run_codex_t3a.py | ARŞİV | Eski file-based Codex başlatıcı betikleri; cx dispatch çağrısıyla supersede |
| scheduled_tasks.lock | **KULLANICIYA SOR** | Aktif zamanlanmış görev kilidi olabilir; silmeden önce doğrula |
| nlm_last_sync.txt, nlm_sync_state.txt (289KB) | **KAL** | `/nlm-sync` aktif state dosyaları — dokunma |
| PROJECT_RULES.md, settings.json, agents/, commands/ | **KAL** | Canlı orkestra altyapısı |

> **Net aksiyon:** ~25 `.txt` + 4 betik → `.claude/_archive/`. `agents/`, `commands/`, `settings.json`, `PROJECT_RULES.md`, nlm_*.txt yerinde kalır.

### 1.B — User-memory triyajı (315 dosya — en büyük drift kaynağı)

Envanterde bayrak dağılımı: çoğunluk **orphan + stale_pit** (MEMORY.md index'inde pointer yok + point-in-time bayat), bir kısmı **revoked** (iptal edilmiş karar), küçük bir kısım **bayraksız** (zaten index'te, geçerli). Triyaj üç kategoriye ayrılır.

**B1 — SİL adayları (revoked VE içeriği başka dosyayla supersede edilmiş):**

| Dosya | Aksiyon | Gerekçe |
|---|---|---|
| project_fakeiso_term_revoked_2026_05_22.md | SİL | Dosya adı "revoked"; iptal edilmiş terim, hiçbir referans yok (TOP-10 #1) |
| project_3kit_bg_architecture_lock.md | SİL | revoked+stale_pit; parallax-modular mimari terk edildi |
| project_canonical_character_roster_lock.md | SİL | revoked; roster_v2 supersede etti (project_canonical_character_roster_v2.md geçerli) |
| project_high_top_down_3_4_lock_2026_05_24.md, project_diamond_iso_tilemap_lock_2026_05_24.md | SİL | revoked; pure-2d-topdown pivot (2026-05-12) supersede etti |
| project_cliff_pivot_manual_brush_2026_05_26.md, project_multilayer_painter_v1_lock.md, project_modular_pipeline_lock.md, project_core_wall_system_v2_lock_2026_05_24.md, project_brush_tool_v1.md, project_brush_v1_manual_composition_system.md | SİL | revoked+stale_pit; eski painter/wall iterasyonları, sonraki LOCK'lar geçerli |
| feedback_layered_terrain_mandatory.md, feedback_nlm_first_context_strict.md, feedback_cx_review_only_routing.md | **KULLANICIYA SOR** | revoked ama MEMORY.md routing bölümünde hâlâ pointer'ı var (cx_review_only) → önce index'ten çıkar, sonra sil |

**B2 — ARŞİV adayları (stale ama tarihsel/kanıt değeri var):** Çoğu `feedback_agy_*`, `feedback_codex_*`, `feedback_pixellab_*` ve `project_cliff_*` / `project_iso_*` / `project_demo_*` dosyaları **orphan+stale_pit**. Bunlar yanlış değil, sadece güncel akışta tetiklenmiyor (örn. agy→ax geçişi, iso→topdown pivot). **Öneri: user-memory'de `_archive/` alt-dizini** veya MEMORY_ARCHIVE'a referansla indeks-dışı bırakma. **Toplu silme YAPMA** — tarihsel "neden böyle karar verdik" izi.

> Bu kategori ~150+ dosya. Tek tek elle taranamaz; **önerilen yöntem:** `/lint` skill'ini çalıştır (zaten orphan/stale tarıyor), çıktısını kullanıcıyla gözden geçir, batch-arşivle. Bu rapor tek tek dosya kararı vermez — TOP örnekler yukarıda, geri kalan `/lint` + onay akışına bırakılır.

**B3 — MEMORY.md INDEX'e EKLENECEKLER (orphan ama HÂLÂ GEÇERLİ kurallar):** Bunlar index'te pointer'ı olmadığı için "orphan" bayraklı ama içerik canlı ve doğru. Index'e 1 satır pointer eklenmeli (silinmemeli, arşivlenmemeli):

| Dosya | Gerekçe (neden hâlâ geçerli) |
|---|---|
| feedback_warn_then_apply_if_insistent.md | Canon-çatışmasında "önce uyar, ısrar ederse uygula" — aktif davranış kuralı, HARD |
| feedback_brief_short_lowrisk_constraints_destructive.md | "Agent'lara kısa brief ver, full anlatma" — aktif dispatch disiplini |
| feedback_mcp_tool_schema.md | "Her MCP tool için task yazmadan ToolSearch çağır" — hâlâ doğru |
| feedback_model_selection.md | "Claude modeli seçer, user execute eder" — hâlâ aktif kural |
| feedback_research_on_block.md | "Otonom'da görüş gereken yerde tahmin etme, araştır" — aktif |
| feedback_orchestrator_delegate_route_by_difficulty.md | Routing disiplini, güncel orkestra kuralıyla uyumlu |
| feedback_compile_in_unity_autonomous.md | "Kod değişikliğinden sonra Unity'de recompile" — aktif |
| feedback_negation_to_positive_prompts.md, feedback_pixellab_style_ref_instruction.md, feedback_pixellab_sxl_low_detail.md | PixelLab prompt kuralları — pipeline hâlâ canlı |

> **Aksiyon:** Bu ~9 dosya için MEMORY.md'nin ilgili bölümüne (HARD RULES / Routing / Teknik) birer pointer satırı ekle. "orphan" bayrağı böylece kalkar.

### 1.C — Repo MEMORY/ ↔ user-memory çakışmaları (tek-kaynak kararı)

Envanter D bölümü 5 birebir çakışan dosya tespit etti (aynı isim, iki konum, drift riski):

| Dosya | Tek-kaynak önerisi |
|---|---|
| project_karar_149_subroom_encounter_lock.md | **Repo MEMORY/ canonical** (proje-spesifik tasarım kilidi) → user-memory kopyasını SİL |
| project_karar_150_act1_envanter_live.md | **Repo MEMORY/ canonical** → user-memory kopyası SİL (üstelik revoked bayraklı) |
| project_subroom_canonical_tags_lock.md | **Repo MEMORY/ canonical** → user-memory kopyası SİL |
| project_wall_production_pipeline_s99_late.md | **İKİSİ DE SİL/ARŞİV** — revoked (s108 sonrası duvar kuralı değişti) |
| project_resume_2026-06-11.md | **İKİSİ DE BAYAT** — eski session resume; CURRENT_STATUS.md güncel → arşivle |

> **Genel kural (tek-kaynak):** Proje-spesifik tasarım kilitleri → **repo `MEMORY/`** (oyunla beraber yaşar, versiyonlanır). Çapraz-proje/operasyonel davranış kuralları → **user-memory** (`~/.claude/.../memory/`). Çakışmada repo MEMORY/ proje-içeriği için kazanır; user-memory operasyonel-davranış için kazanır.

### 1.D — NLM-sync düzeltmeleri (bayat OPERASYONEL bilgi)

NLM canonical KB'de TASARIM kararları taze ama **operasyonel routing bayat**. Çapraz-kontrol bulguları:

| Bayat NLM içeriği | Düzeltme |
|---|---|
| Eski local-model routing (qwen2.5 / deepseek-r1 hâlâ kayıtlı) | NLM'den çıkar/güncelle — cx/ax'e geçileli çok oldu; local model artık "sormadan asla" (feedback_never_run_local_model). Local routing notları yanıltıcı. |
| laurethgame profili rotasyonda görünüyor | NLM'de DISABLED/yasak olarak işaretle (feedback_disabled_profile_hard_ban: yekta/laurethgame DISABLED = kesin yasak) |
| project_nlm_notebook_id.md "revoked" bayraklı (user-memory) | **KULLANICIYA SOR** — notebook ID hâlâ `30ddffa5...` kullanımda; "revoked" bayrağı yanlış olabilir, doğrula |

> **Drift hiyerarşisi nüansı (rapora kazınan ayrım):** TASARIM kararlarında **NLM canonical kazanır** (notebook üst-otorite). OPERASYONEL routing'de **local memory daha taze** (NLM'deki routing 14+ gün bayat). Yani: bir tasarım sorusunda NLM'e güven; bir "hangi modele dispatch" sorusunda user-memory'nin Routing bölümüne güven. NLM-sync sırasında operasyonel routing dosyalarını NLM'e push ETME (orada bayatlar) — sadece TASARIM/MEMORY tasarım dosyalarını sync et.

### 1.E — STAGING üst-seviye temizliği (536 dosya → ~LIVE'lar kalır)

Envanter her dosyayı LIVE / BAYAT olarak etiketlemiş. Pattern net:
- **LIVE = kalır** (üst-seviye): `*_DECISION_<tarih>.md`, `*_PLAN*.md`, aktif `*_SPEC*.md`, README.md. Örn. HUD_LAYOUT_DECISION_2026-06-12, MODULAR_ABILITY_DECISION_2026-06-12, DEMO_TOOLS_SCOPE_DECISION (yeni), INVERSION_MECHANIC_DECISION.
- **BAYAT = `STAGING/_process/2026-06/`'ya taşı:** Tüm `*_CX_TASK.md`, `*_AGY_TASK.md`, `*_DONE.md`, `REVIEW_*.md`, `ANALYZE_*.md`, `*_qc.md`, `research_*.md`, `*_S6.md` ve isimlendirme-dışı tamamlanmış dosyalar. Bu, envanterdeki "BAYAT" işaretli ~350+ dosyanın tamamı.

> **Yöntem:** Tek tek taşıma elle yapılmaz. **`archive_staging_process.ps1`** (whitelist-tabanlı, idempotent — LIVE pattern'leri korur, gerisini tarihli `_process/`'e taşır) yazılıp çalıştırılır. Bu script ZATEN playbook'ta "bootstrap'a eklenecek" olarak işaretli; RIMA'da ilk kez burada çalıştırılır. **Önemli istisna:** `LAURETHSTUDIO_PLAYBOOK_EXTRACTION_2026-06-13.md` ve bu rapor (`LAURETHSTUDIO_ADAPTATION_REPORT_2026-06-13.md`) — envanter bunları "BAYAT şüpheli" işaretledi ama AKTİF LIVE belgeler; whitelist'e ekle, taşıma.

> **TOP öncelik (ilk taşınacak, envanter TOP-10'dan):** ANALYZE_AX_CLEANUP_MAPDESIGNER_S6.md, D5_5_cliff_2stage_separation_task.md ve tüm `BUILD_*_CX.md`, `REVIEW_*_cx/agy.md` serileri.

---

## BÖLÜM 2 — LAURETHSTUDIO'YA TAŞINACAKLAR

### 2.1 — KOPYALA-GEÇ (değişiklik gerekmez; sadece doğrula)

Bunlar zaten **global** (`~/.claude/skills/` + `~/.claude/commands/`), dinamik kök ile açık projeye anchor olur. LaurethStudio'da (veya `02_GAMES/` alt-projelerinde) otomatik kullanılabilir. **Aksiyon = sadece smoke-doğrulama listesi:**

| Skill/Komut | Doğrulama |
|---|---|
| `/cx /ax_flash /ax_pro /ax_opus` | LaurethStudio kökünde dummy dispatch → sonuç doğru köke düşüyor mu? (ax CWD-fix 2026-06-13 smoke-verified, ama yeni kökte tekrar doğrula) |
| `/council` | writer≠reviewer roster (cx + ax-pro + ax-opus) çalışıyor mu |
| `/nlm /nlm-sync` | Parametrik (`NLM_NOTEBOOK_ID`, `NLM_REPO`) — LaurethStudio için AYRI notebook gerekir mi (Bölüm 4 açık soru) |
| `/bootstrap-project` | Zaten stüdyo-evrensel (NLM'de ilan edilmiş); mevcut projede orkestra kurar |
| `verification-before-completion`, `systematic-debugging`, `subagent-driven-development` | Proje-bağımsız global skill'ler; doğrudan çalışır |
| `/commit /save-session /phase-close /plan /lint` | Global komutlar — `<PROJECT_NAME>` parametreli olanlar var (aşağı bak) |
| Görsel: `/agy_image /codex_image /gpt-image-2 /pixelify /generate2dsprite /generate2dmap` | Proje-bağımsız (pixelify/pixel-cleanup hariç — UYARLA'ya bak) |

**Stüdyo-evrensel ilan edilmiş (NLM):** `/bootstrap-project` · CLAUDE.md stub pattern · Studio AI Asset QA Gate v2 (STUDIO_KARAR_005, 10-test görsel kapısı) · Wang tile build workflow (studio pattern universal) · LAURETH_2D_ILLUSION_LIBRARY (32 teknik proje-bağımsız havuz). **Bunlar zaten `F:/LaurethStudio/`'da canonical** (master plan: `00_RULES/STUDIO_CONSTITUTION.md` + `05_RESEARCH/` + `MEMORY/`) — RIMA'dan tekrar taşıma GEREKMEZ, sadece RIMA-içi kopyaların stüdyoya pointer verdiğini doğrula (örn. RIMA `MEMORY/wang_tile_build_workflow_rima.md` zaten `F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow.md`'yi üst-otorite gösteriyor — doğru pattern).

### 2.2 — UYARLA (RIMA-hardcode temizliği gerek)

| Öğe | RIMA-hardcode | Uyarlama |
|---|---|---|
| `pixel-cleanup` skill | SKILL.md içinde RIMA yolu | Yolu parametrik/CWD-relative yap |
| `pixelify` skill | `ppu_grid_reference.md` RIMA referansı | Referansı proje-parametrik yap (PPU her oyunda farklı) |
| `rima-conventions` skill | Tamamen RIMA kurallarına özel | **Şablonlaştır → `laureth-conventions` veya proje-lokal `<proje>-conventions`.** RIMA'ya özgü kontroller (PPU değeri, Y-sort axis, S101 PILLAR-LESS, torch baked) çıkar; generic kontroller (ACTIVE RULES header eksik, dispatch effort flag eksik, Türkçe-ASCII ihlali) korunur. Her proje kendi varyantını tohumlar. |
| `.claude/agents/rima-*.md` (6 agent) | rima-asset/design/doc/qc/research/sonnet | **Genel agent şablonları:** `studio-asset.md`, `studio-design.md`, `studio-doc.md`, `studio-qc.md`, `studio-research.md`, `studio-sonnet.md`. İçerikten RIMA-spesifik referansları (NLM notebook ID, Act1 canon, PixelLab pipeline) parametre/placeholder yap. |
| `ax_flash/ax_opus/ax_pro` skill'leri | Ortak auth manager dosyasına referans | Auth manager zaten global (CodexAuthManager); referans yolu doğru. Yeni kök doğrulaması yeter — kod değişmez. |
| `commands/` (lint, phase-close, plan) | `<PROJECT_NAME>` placeholder VAR (lint/phase-close/plan zaten parametrik) | Stüdyo geneli için `<PROJECT_NAME>` env/config'ten beslensin. nlm-sync zaten `NLM_NOTEBOOK_ID`/`NLM_REPO` parametrik (`nlm-sync-template` var). |

### 2.3 — YENİDEN-KUR (boş-şablon içerikleri — LaurethStudio session kopyalasın)

> **NOT:** `F:/LaurethStudio/` zaten kendi `STUDIO_CONSTITUTION.md` + `CURRENT_STATUS.md`'ye sahip. Aşağıdaki şablonlar **`02_GAMES/<yeni-oyun>/` alt-projeleri** için ve stüdyo katmanının orkestra-disiplini eksikse onu tamamlamak için. Mevcut stüdyo dosyalarının ÜZERİNE YAZMA — eksikse ekle.

**(a) `CLAUDE.md` stub (3 satır, ~30 token):**
```
# <PROJE_ADI>
Session start: `.claude/PROJECT_RULES.md` + `CURRENT_STATUS.md` oku. Başka okuma.
Sub-agents: bu dosyayı yoksay, orchestrator bağlamı doğrudan verir.
```

**(b) `.claude/PROJECT_RULES.md` iskeleti (bölüm başlıkları):**
```
# <PROJE_ADI> — Orkestra Kuralları

## Orchestrator Context Koruma
ORCHESTRATOR BULK İŞ YAPMAZ. < 3 dosya oku / < 5 satır edit = direkt; aksi = dispatch.

## Agent Routing
kod/Unity/mekanik=cx · hızlı bilgi/lean=ax Flash · derin/mimari/vision=ax Pro · kritik review=ax Opus · analiz=Sonnet subagent.

## Universal Coding Principles (Karpathy 4)
(1) düşün+varsayım listele (2) min kod, spekülasyon yok (3) cerrahi — sadece görev dosyaları (4) hedef-odaklı, BLOCKED if unclear.

## Dispatch Header Zorunluluğu
Her dispatch ilk satırları: `ACTIVE RULES: ...` + (varsa) `NLM ACCESS:` + `# Amaç`.

## Kanıt Disiplini
Kanıtsız "bitti" DENMEZ. Çalıştırılmış doğrulama çıktısı zorunlu.

## Drift Hierarchy
(1) Canonical KB > (2) Local memory (point-in-time, stale risk) > (3) STAGING draft.

## writer ≠ reviewer
Otonom run'da üreten kendi işini onaylamaz. Council = bağımsız tanık + sentez.

## Build-Safety (engine-spesifik)
Dev-ortamı API'si ≠ ship-ortamı API'si. Runtime yol her zaman dağıtımda mevcut olandan beslenir.
```

**(c) `MEMORY.md` index formatı:**
```
# <PROJE> Memory Index
> Canonical source = <KB pointer>. Detay → topic .md. Bu index = kısa pointer'lar.

## HARD RULES (her session aktif)
- [kural-adı](dosya.md) — tek satır özet

## Routing (güncel)
- [pointer](dosya.md) — tek satır

## Proje durumu
- [pointer](dosya.md) — tek satır
```

**(d) Memory dosya frontmatter şablonu:**
```
---
name: <kebab-case-ad>
description: "<tek cümle ne/ne zaman>"
metadata:
  node_type: memory
  type: feedback | project | reference
---
# <Başlık>
<gövde — kural + kanıt + tarih>
```

**(e) STAGING klasör disiplini:**
```
STAGING/                      ← SADECE LIVE (DECISION/PLAN/SPEC/aktif TASK)
STAGING/_process/<YYYY-MM>/   ← geçici (council/review/done/research/analiz)
STAGING/_archive/             ← eski LIVE versiyonlar (One-LIVE kuralı)
```
`_` prefix = nlm-sync/tarama atlar. Asla silinmez, taşınır. `archive_staging_process.ps1` (whitelist, idempotent) periyodik çalışır.

**(f) CURRENT_STATUS lean RESUME şablonu:**
```
# <PROJE> — CURRENT STATUS

## RESUME (tek aktif blok, max ~50 satır)
**Tarih:** YYYY-MM-DD
**Durum:** <1-2 cümle nerede kaldık>
**Sıradaki:** <numaralı, kısa>
**Bloker/Flag:** <varsa>

> Önceki RESUME blokları git history'de. Bu dosya REPLACE edilir, prepend EDİLMEZ.
```

---

## BÖLÜM 3 — LAURETHSTUDIO SESSION UYGULAMA TALİMATI (self-contained, sıralı)

> Yeni session sıfır bağlamla başlar. Önce **`F:/LaurethStudio/CURRENT_STATUS.md` + `F:/LaurethStudio/00_RULES/STUDIO_CONSTITUTION.md` oku** — mevcut yapıyla çelişme. Bu talimat o yapının ÜZERİNE orkestra-disiplini katmanı ekler.

**ADIM 0 — Mevcut durumu sapta (çelişme önleme):**
- `F:/LaurethStudio/` kök yapısını listele. `00_RULES/`, `02_GAMES/`, `03_IDEAS/`, `05_RESEARCH/`, `MEMORY/`, `CURRENT_STATUS.md` var mı doğrula (master plan'a göre VAR).
- `00_RULES/STUDIO_CONSTITUTION.md`'de orkestra/routing/Karpathy kuralları var mı kontrol et. **Varsa → ekleme yapma.** Yoksa → ADIM 2 şablonlarını oraya enjekte et.
- Yeni iskelet KURMA. Bu bir mevcut-stüdyo güncellemesi.

**ADIM 1 — `.claude/` katmanını stüdyo köküne kur (yoksa):**
- `F:/LaurethStudio/.claude/PROJECT_RULES.md` yoksa → Bölüm 2.3(b) iskeletini yaz (stüdyo-geneli kurallar).
- `F:/LaurethStudio/CLAUDE.md` stub'ı yoksa → 2.3(a).
- `.claude/agents/studio-*.md` (6 genel agent şablonu) → RIMA'dan uyarlanmış (Bölüm 2.2).
- `.claude/_archive/` boş klasörü oluştur (gelecek hijyen için).

**ADIM 2 — Kanıt disiplini + dispatch header'ı kilitle:**
- STUDIO_CONSTITUTION veya PROJECT_RULES'a: Karpathy-4 + `ACTIVE RULES/NLM ACCESS/# Amaç` header + "kanıtsız bitti yok" bloğunu ekle (2.3(b)'den). Zaten varsa atla.

**ADIM 3 — Routing + dispatch skill'lerini smoke-doğrula:**
- `F:/LaurethStudio/` kökünden bir dummy `/ax_flash "test"` dispatch et → sonucun `F:/LaurethStudio/.claude/`'a (veya açık alt-projeye) düştüğünü DOĞRULA. ax CWD-fix 2026-06-13 verified ama yeni kökte tekrar.
- `/council` rosterini (writer≠reviewer) onayla.

**ADIM 4 — STAGING hijyeni + One-LIVE + lean RESUME aktive et:**
- `F:/LaurethStudio/STAGING/` (veya stüdyo eşdeğeri) altına `_process/<YYYY-MM>/` + `_archive/` kur.
- `archive_staging_process.ps1` (whitelist: DECISION/PLAN/SPEC/README/aktif-TASK korunur) yaz — **önce RIMA'da test et** (Bölüm 1.E), sonra stüdyoya kopyala.
- `CURRENT_STATUS.md` lean-RESUME kuralını (replace, prepend değil) `/save-session` skill'ine + dosya başına not olarak ekle.

**ADIM 5 — Bilgi katmanı (NLM/notebook kararı):**
- **AÇIK SORU (Bölüm 4):** LaurethStudio kendi NLM notebook'una mı sahip olacak, yoksa RIMA notebook'unu mu paylaşacak? Master plan stüdyo `MEMORY/`'sinin ayrı olduğunu söylüyor → muhtemelen **ayrı notebook**. Karar verilene kadar `/nlm-sync`'i stüdyoda çalıştırma; "sonra" notu bırak.
- İki-katmanlı memory (index + topic, 2.3(c)/(d)) iskeleti stüdyo `MEMORY/`'sinde zaten var mı kontrol et; yoksa tohumla.
- `studio-conventions` (rima-conventions'tan generic'leştirilmiş, 2.2) proje-lokal skill'ini tohumla.

**ADIM 6 — Doğrula + commit:**
- `verification-before-completion` ile her adımın kanıtını topla (klasör var mı, dummy dispatch düştü mü, script idempotent mi).
- Stüdyo repo'sunda mantıksal commit (`/commit`).

---

## BÖLÜM 4 — AÇIK SORULAR (kullanıcı kararı gerek; maks 5)

1. **NLM notebook:** LaurethStudio AYRI bir NotebookLM notebook'u mu alacak, yoksa RIMA'nın `30ddffa5...` notebook'unu mu paylaşacak? (Master plan stüdyo MEMORY'sinin ayrı olduğunu ima ediyor — ama tasarım KB'si paylaşılabilir.) ADIM 5 buna bağlı.

2. **`project_nlm_notebook_id.md` "revoked" bayrağı doğru mu?** Notebook ID hâlâ aktif kullanımda görünüyor ama user-memory'de revoked işaretli. Silinmeli mi, bayrak mı yanlış?

3. **User-memory B2 arşivi (~150+ stale_pit dosya):** Toplu `/lint`+arşiv mi, yoksa dosya-dosya onay mı istiyorsunuz? (Rapor toplu+onay öneriyor ama bu çok dosya etkiliyor.)

4. **`rima-conventions` → `studio-conventions` generic'leştirmesi:** RIMA kendi `rima-conventions`'ını KORUYACAK (RIMA-spesifik PPU/Y-sort kontrolleri lazım) — stüdyo için AYRI generic varyant mı, yoksa tek parametrik skill mi? İki-skill önerisi mi, tek-parametrik mi?

5. **`.claude/` codex_*.txt arşivi (Bölüm 1.A):** `.claude/_archive/`'a taşıma onaylanıyor mu, yoksa tamamen sil mi (git history zaten tutuyor)? `scheduled_tasks.lock` aktif mi?

---

> **Uygulama notu:** Bu rapor SADECE plan. Hiçbir dosya bu session'da taşınmadı/silinmedi (surgical kural). Bölüm 1 RIMA-içi, orchestrator kullanıcı onayından sonra uygular. Bölüm 2-3 ayrı LaurethStudio session'ında uygulanır.
