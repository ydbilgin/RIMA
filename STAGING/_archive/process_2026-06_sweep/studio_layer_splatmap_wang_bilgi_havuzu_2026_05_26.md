# Studio → RIMA Bilgi Havuzu: Layer/Splatmap + Custom Wang Build + Townscaper

**Tarih:** 2026-05-26 (Studio S15 close)
**Kaynak orchestrator:** LaurethStudio Opus orchestrator
**Hedef:** RIMA orchestrator — bilgi olarak kullan, **kendi proje kontekstinde** karar ver. Bu dosya dayatma değil, transfer.

---

## I. Nedir Bu Dosya

LaurethStudio S15 oturumunda 2D pixel-art pipeline'ı için bir dizi karar verildi (Wang tile build, splatmap shader, layer architecture, 4 tweet pattern). Bu kararlar **Studio universal** olarak kayıt edildi ama **RIMA için adapte edilmesi RIMA orchestrator'ın işi.**

Studio buradan ne dayatır:
- Hiçbir şey. Bu sadece bilgi transfer.

Studio buradan ne önerir:
- RIMA action roguelite için yararlı olabilecek pattern + pipeline kararları.
- RIMA mevcut KARAR'larıyla (#131 16-key Wang, #98 cyan palette, #143 6-layer environment) çakışma/sinerji noktaları.
- RIMA orchestrator istediği zaman kendi MEMORY'sine entegre eder.

---

## II. Studio Kayıt Dosyaları (Referans)

| Konu | Studio path | Status |
|---|---|---|
| Studio Custom Wang Build Workflow | `F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow.md` | LOCK |
| Wang tile pipeline research | `F:/LaurethStudio/05_RESEARCH/2026_05_26_pixellab_wang_tile_workflow.md` | LOCK |
| Studio Orijinali Hibrit (oyun kararı) | `F:/LaurethStudio/MEMORY/studio_orijinali_hibrit_lock.md` | LOCK aday |
| Üçlü AI sentez (Sonnet+Codex+agy) | `F:/LaurethStudio/05_RESEARCH/2026_05_26_studio_orijinali_hibrit_uclu_sentez.md` | LOCK |
| Game Artist Prompt Template v1 | `F:/LaurethStudio/04_TEMPLATES/studio_game_artist_prompt_template_v1.md` | LIVE |
| AI Asset QA Gate v2 | `F:/LaurethStudio/01_PIPELINE/2D/ai_asset_qa_gate.md` | LIVE (10 test çift katman) |
| 2D Illusion Library (32 teknik) | `F:/LaurethStudio/MEMORY/2d_illusion_library.md` | Reference |
| Splatmap Shader Python proof | `F:/LaurethStudio/02_GAMES/StudioHibritFarmingSim/Tools/PainterSuiteV2/splatmap_shader_proof.py` | Proof PASS |
| Wang demo + NeighborAnalyzer | `F:/LaurethStudio/02_GAMES/StudioHibritFarmingSim/Tools/PainterSuiteV2/` | Çalışan |

---

## III. Ana Studio Kararları (RIMA için relevant)

### 1. PixelLab Tool Kullanımı
- **KULLAN:** Create Image S-XL Pro (text-to-image), Edit Image / Inpaint, Map Object, Character States
- **KULLANMA:** Create Tiles Pro (auto Tileset aracı — geometrik Wang yapamıyor, S15 verdict)

### 2. Wang Tile Üretim İki Yöntem
- **Yöntem A — Tek-tek tile çizdir:** 4 base tile + matematiksel assembly (Tilesetter veya Python) + Inpaint seam fix + Aseprite palette remap. Küçük scope için.
- **Yöntem B — Büyük composition çizdir + grid'e böl + komşuluk mantığı:** 128×128 = 4×4 grid, AI bir kerede 16 tile aynı stil. Sonra grid'e böl + komşuluk analiz + Wang slot tayin + inpaint kenar fix. Büyük scope için (biome geçişleri, dekor zincir).
- **Yöntem B+ hibrit (Codex önerisi):** Y → analyzer → Z fill → validator. Önce büyük composition (style coherence), sonra eksik slot için targeted inpaint.

### 3. Splatmap Shader (Zemin Geçişleri)
**Kritik insight:** Wang tile **zemin için yanlış araç**. Wang grid-bound = tile sınırı görünür = cozy organic kıvrım imkansız.

**Doğru pipeline (Codex Q1.2 HLSL kodu hazır):**
- `WorldSpaceSplatmap.shader` + `DitherBlend.hlsl` Bayer 4×4 dither
- Multi-channel splatmap (RGBA = 4 farklı biome layer)
- Priority blend: water > stone > sand > dirt > grass base
- Photoshop/Aseprite freehand mask edit
- Per-pixel render, GPU shader, 60fps zero CPU
- Python simülasyon proof: `splatmap_shader_proof.py` PASS

**Studio karar:**
- **Zemin geçişleri** (grass/dirt/sand/water/stone) = Splatmap Shader
- **Building/prop** (duvar, çit, kapı, yol kenarı) = Wang tile

### 4. Aseprite 16-Color Otomatik Palette Remap
```bash
aseprite.exe -b input.png --color-mode indexed --palette studio_palette.gpl --save-as output.png
```
PainterSuite v2 Palette-Lock Daemon Python watch folder → %100 unattended. Studio palette force kilit.

### 5. AI Asset QA Gate v2 (10 Test Çift Katman)
- **Katman A (Technical):** Seam / Alpha / Scale / Silhouette / Palette
- **Katman B (Game-Artist):** Silhouette readability / Color depth / Mixel / Anti-aliasing / Pose readability

Her sprite her iki katmanı geçer veya REDDEDİLİR.

### 6. Studio Game Artist Prompt Template
Kanonik frontmatter: ASSET_TYPE / RESOLUTION / CAMERA_POV / LIGHTING / PALETTE / BRUSH_THICKNESS / ANATOMY_PROPORTION / STYLE_REF / NEGATIVE_ABSOLUTE.

Generic prompt yerine artist-tier disipline yazım.

### 7. NeighborAnalyzer.py (Wang 16 Detection)
- 32×32 cell input → 4 kenar piksel band analiz → binary mask N/E/S/W → Wang ID
- Big-Endian encoding: `ID = N×8 + E×4 + S×2 + W×1`
- Threshold 0.5, edge band 4-px, corner exclude 4-px (cross-edge contamination önleme)
- agy + Codex iki ayrı versiyon yazdı, çalışıyor.

---

## IV. 4 Tweet + Realtime Parallax Plugin + Townscaper (Bilgi Eklemesi)

Studio kullanıcısı şunları araştırma listesine ekledi (Studio S15 close, agy + Codex tekrar incelemeye dispatch edildi):

### Tweet 1: https://x.com/_amohs/status/2040860074743156903
İçerik **araştırılıyor** (Studio dispatch background'da). RIMA kendi sürümünde web grounding yapabilir.

### Tweet 2: https://x.com/aminerehioui/status/2055785406315090062
**GameObject-less Tilemap mimari** (önceki Studio sentez). 10k tile için 10k GameObject yasak. Unity Tilemap + GPU Instancing (`DrawMeshInstancedIndirect`) + central GridManager. RIMA için zaten geçerli — dungeon/arena tile rendering aynı disiplin.

### Tweet 3: https://x.com/RomesteadGame/status/2058951277917192294
İçerik **araştırılıyor** (Studio dispatch background'da).

### Tweet 4: https://x.com/eringijirou/status/2059224550718779767
**Syrup Map Auto-Decoration Plugin (RPG Maker)** — kabaca zemin çiz, sistem dekoratif öğeleri otomatik yerleştirir (%5 çiçek + %12 taş + %3 yosun). %80 tasarımcı zaman tasarrufu. RIMA dungeon/arena ortamı dekoratif öğe placement için relevant olabilir.

### Realtime Parallax Map Builder (RPG Maker MZ Plugin)
**URL:** https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
Layer-bazlı runtime map building. Studio için Unity URP'ye transplant edilmesi araştırılıyor. RIMA için relevant olabilir (dungeon room editor, modlama tool).

### Townscaper ⭐ Altın Referans
Oscar Stålberg, tek dev, Steam hit. **WFC (Wave Function Collapse)** + click-to-build + komşuluk-bazlı procedural. Studio'nun Wang/Blob slot picking ve PainterSuite v2 level editor için **birincil ilham**.

**Townscaper'dan 7 potansiyel transplant:**
1. WFC algoritma → Wang/Blob slot picking güçlendirme
2. Click-and-build runtime level editor
3. Brush tool minimalizm (1 tool, 1 click, no menu)
4. Color/material layer switch
5. Hex+irregular vs square grid
6. Procedural komşuluk = "build" hissi (RIMA arena composer için relevant olabilir mi?)
7. Tek dev shipping scale referansı

RIMA için soru: WFC RIMA dungeon/arena procgen'ında zaten kullanılıyor mu (KARAR ekosistemi)? Eğer hayırsa, Townscaper-vari WFC RIMA için yeni eksen olabilir.

---

## V. RIMA İçin Olası Adaptasyon Noktaları

**Bunlar Studio'nun önerileridir. RIMA orchestrator değerlendirip kendi MEMORY'sine entegre eder.**

### A. RIMA palette ile Studio Custom Wang Build entegrasyonu
- RIMA palette cyan #00FFCC accent (Karar #98)
- Studio palette baseline farklı, RIMA için ayrı `rima_palette.gpl` yarat
- Aseprite Palette-Lock Daemon RIMA için ayrı watch folder

### B. RIMA Wang ihtiyacı (KARAR #131) ile Studio Wang Build
- KARAR #131 (16-key Wang lookup) zaten RIMA'da var
- Studio Custom Wang Build = KARAR #131'in **pratik PixelLab uygulaması**
- Çakışma değil, entegrasyon
- Iki yöntem (A tek-tek + B büyük composition) RIMA için de geçerli

### C. RIMA zemin için Splatmap Shader
- RIMA action roguelite dungeon/arena ortamlarında splatmap shader doğal kullanım
- Lava + stone + magic moss + crystal layer'ları çoklu-channel splatmap
- Codex `WorldSpaceSplatmap.shader` HLSL kodu RIMA için direkt port edilebilir
- RIMA'nın KARAR #143 (6-layer environment) ile sinerji

### D. RIMA Decor Brush (Syrup-tarzı, Tweet 4)
- Dungeon arena floor dekorasyonu (çatlak, kemik, magic mark) otomatik rule-based placement
- Studio Auto-Decor Brush konsepti RIMA'ya direkt transfer

### E. RIMA için Townscaper-vari Arena Composer?
- Action roguelite arena procgen'ı için WFC-tabanlı composer
- KARAR #143 environment + WFC = procedural arena variety
- M0 spike olarak RIMA'da test edilebilir

### F. RIMA için Realtime Parallax Plugin?
- Action 2D'de parallax background derinlik
- RIMA cyan + violet ışıltılı atmosphere için layer-based depth
- Unity URP custom pass + parallax shader

### G. AI Asset QA Gate v2 RIMA için zorunlu
- 10 test çift katman RIMA tüm AI asset'leri için
- KARAR_005 RIMA'da zaten var, v2 update Studio'dan al

---

## VI. RIMA Karar Anları (RIMA orchestrator değerlendirir)

RIMA orchestrator bu bilgi havuzunu okuyup şu kararları kendisi verecek:

1. Hangi Studio pattern RIMA için relevant? (yukarıdaki A-G'den hangisi RIMA pipeline'a girer)
2. Hangi pattern adapt edilmeli vs olduğu gibi kullanılmalı?
3. RIMA mevcut KARAR'larıyla (#131, #98, #143) çakışan / sinerji oluşturan kısımlar?
4. RIMA için yeni KARAR aday'ı oluşturulmalı mı (örn "RIMA Custom Wang Build" - KARAR #131 + Studio entegrasyon)?
5. RIMA Tools/ klasörüne hangi Python script'ler kopyalanmalı?
6. RIMA MEMORY/INDEX.md güncelleme planı?

Studio Opus orchestrator buraya kadar bilgi transferi yaptı. **RIMA kendi orchestrator session'ında karar verir.**

---

## VII. Cross-Link (Studio'ya geri-bağlantı)

Eğer RIMA bu pattern'leri MEMORY'sine entegre ederse, RIMA memory dosyalarında **`[[F:/LaurethStudio/MEMORY/studio_custom_wang_build_workflow]]`** gibi cross-link verebilir. Studio bu pattern'in **kaynak otoritesidir**. RIMA versiyonu uygulama dokümantasyonudur.

Studio'da bu pattern'lerde değişiklik olursa RIMA orchestrator'a bildirilir (memory sync workflow).

---

## VIII. Studio'nun Şu Anki Açık İşleri (RIMA için bilgi)

Studio S15'te şu paralel dispatch'ler hâlâ çalışıyor olabilir:
- agy: 4 tweet + Realtime Parallax + Townscaper detaylı re-check
- Codex: Layer/splatmap + 4 tweet + WFC + RIMA transplant teknik plan

Bu dispatch'ler sonuçlanınca Studio sentez güncellenecek. RIMA bu güncellemeleri ister Studio MEMORY'sinden okur, ister Studio orchestrator transfer eder.

---

## IX. RIMA İçin Sade Action Listesi (Öneri)

**Eğer RIMA orchestrator bu bilgileri uygulamayı seçerse:**

1. `RIMA/MEMORY/wang_tile_build_workflow_rima.md` yeni dosya yarat (Studio universal pattern'in RIMA-spesifik versiyonu)
2. `RIMA/MEMORY/MEMORY.md` INDEX güncelleme
3. `RIMA/Tools/palette_lock_daemon.py` Python script (Studio'dan kopyala + RIMA path'leri)
4. `RIMA/Art/Palettes/rima_palette.gpl` (16-color cyan brand baseline)
5. `RIMA/Tools/neighbor_analyzer.py` + `wang_validator.py` (Studio Codex çıktıları kopyala)
6. `RIMA/Shaders/WorldSpaceSplatmap.shader` (Studio Codex HLSL kopyala, RIMA palette adapt)
7. `RIMA/MEMORY/active_ai_asset_qa_gate_v2.md` (Studio KARAR_005 RIMA için update)

Studio'nun M0 spike Gün 1-2-6 dispatch çıktıları RIMA için doğrudan kullanılabilir:
- `02_GAMES/StudioHibritFarmingSim/STAGING/spike_M0_codex_raw_output_2026_05_26.md` (Unity scaffold, splatmap shader, Wang pilot, co-op networking)

---

## X. Studio Notlar (RIMA için referans)

**Studio orchestrator'ın özel notu:**

Studio S15 oturumu büyük bir paradigma değişikliği yaşadı:
- Wang tile = zemin için yanlış araç → Splatmap Shader doğru araç
- Wang = building/prop için
- Layer architecture = freehand mask edit + GPU shader blend
- Townscaper WFC = Studio (ve potansiyel RIMA) için altın referans

Bu paradigmanın **RIMA action context'inde** ne kadar geçerli olduğu RIMA orchestrator'ın değerlendirmesine kalıyor. Action roguelite dungeon/arena ortamları cozy farm'dan farklı ihtiyaçlara sahip. Studio kendi disiplinini RIMA'ya dayatmaz.

---

**Hazırlayan:** Studio Opus orchestrator (LaurethStudio session 2026-05-26)
**Dosya hedefi:** RIMA orchestrator next session start dosyası
**Action item RIMA için:** Studio bu bilgileri verdi, sen kendi proje kontekstinde karar ver.
