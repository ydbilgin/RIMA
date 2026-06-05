# SENIOR DESIGN — DETAYLI RAPOR PLANI (living doc)

**Amaç:** Bitirme/senior design **detaylı raporu** (~30-40 sayfa), Türkçe, öğrenci dili. Kaynak = ara rapor; bazı şeyleri detaylandırarak genişlet. Map screenshot'ları eklenecek.
**Durum:** PLAN aşaması. Draft başlamadı. (Claude tasarrufu: bulk yazım cx/agy'ye, Opus outline+review.)
**Sıralama kararı:** Önce oynanabilir iso demo + gerçek screenshot'lar → state kilitlenince draft başlat (erken draft = redraft israfı).
**Oyun durumu (2026-06-01):** _IsoGame edit-mode iso floor SAĞLAM; F5'te RuntimeRoomManager flat oda kuruyordu → cx'e fix dispatch edildi (`STAGING/cx_task_playable_iso_room.md`).

---

## Kaynak materyaller
- **`ARA_RAPOR_RIMA.docx` (proje KÖKÜ)** — teslim edilen GERÇEK ara rapor (user 2026-06-01 teyit etti). ANA İSKELET.
- `ARCHIVE/AKADEMIK/ARA_RAPOR_RIMA.md` — aynı raporun markdown kaynağı
- `ARCHIVE/AKADEMIK/ARA_RAPOR_RIMA.docx` / `YasinDeryaBilginAraRapor.pdf` — teslim edilen sürüm
- `ARCHIVE/AKADEMIK/ara_rapor_ornek.docx` — **FORMAT ŞABLONU** (hoca formatı)
- `ARCHIVE/AKADEMIK/create_ara_rapor.py` — md→docx üretim scripti (uyarlanacak)
- `ARCHIVE/old_root_backup/HOCA_MAIL.md` — proje önerisi/kapsam (hoca beklentisi)
- `CURRENT_STATUS.md`, `.claude/PROJECT_RULES.md`, `MEMORY/` — güncel gerçek durum
- NLM notebook `30ddffa5-...` — canonical tasarım
- `STAGING/imagegen/concept01-10_*.png` — agy/Imagen konsept görselleri
- Unity sahneleri (`_IsoGame`, `DemoMap_S6`, `PlayableArena_Test01`) — screenshot

## ⚠️ Ara rapordaki GÜNCELLİĞİNİ YİTİREN bilgiler (düzeltilecek)
- **Yön:** "4 çapraz yön (SE/NE/NW/SW)" → **8 yön LOCKED** (5 sprite üret + 3 mirror flipX). 4-dir REVOKED (Karar #114).
- **Sprite canvas:** "128x128" (ara rapor) → **GÜNCEL (2026-06-06, dosyalardan ölçüldü): 120-128px kare, sınıfa göre değişir** (Warblade/Brawler/Elementalist 120 · Gunslinger/Hexer/Ravager/Shadowblade/Summoner 124 · Ranger/Ronin 128), görünür gövde ~64px, PPU 64. (Eski "64px'e döndü" notu 2026-06-01'de geçerliydi, SUPERSEDED — kullanıcı 2026-06-06: "karakterlerimizin boyutu değişti".)
- **Perspektif:** "~35° top-down" → evrildi; high top-down 3/4 + **iso floating-island floor** yönü (10 konsept iso kazandı).
- **Animasyon:** run-first hâlâ geçerli ama 8-yön'e göre revize.
- Boss adı / lore canon NLM ile doğrulanmalı.

## ➕ EKLENECEK YENİ BÖLÜMLER (ara rapordan SONRAKİ iş)
- **Çevre/harita üretim pipeline'ı:** modüler PixelLab pipeline, iso floating-island, floor451 16-varyant, cliff sistemi (CliffAutoPlacer+DirectionalCliffTile), cyan decal + Light2D bütçesi.
- **Map Designer aracı** (in-editor tool) — pipeline'ın somut deliverable'ı, özgün mühendislik katkısı.
- **Portal / preview-ada / cyan-orb ışınlanma** sistemi.
- **Agentic AI orkestrasyon** bölümü GENİŞLET: cx (Codex/kod), ax→agy (Gemini/research+image), Opus (synth/review), NLM knowledge base, Unity MCP, multi-account routing → solo geliştirici = sanal ekip. (En özgün akademik katkı.)
- **Güncel test sonuçları** (EditMode 474/486 vb. — ara rapordaki 128/128 eski).
- **Görsel ekler:** map screenshots + concept images + sprite sheet + tool UI.

### 🛠️ "Ne tool yaptık" — Geliştirme Araçları bölümü (user vurguladı — RAPORA EKLE)
Bu bölüm raporun en özgün mühendislik kanıtı. Yapılan araçlar:
- **RIMA Map Designer** — tek pencerede 7-tab in-editor harita tasarım aracı + F2 in-game overlay, paylaşımlı RoomData (oyunun deliverable'ı).
- **Modüler PixelLab üretim pipeline'ı** — create_tiles_pro (iso floor) + create_1_direction_object (prop/edge) + asset import/registry baker (`RuntimeAssetRegistryBaker`, AssetPack v3).
- **Cliff generate sistemi** — `CliffAutoPlacer` (mantıksal yerleşim) + `DirectionalCliffTile` (komşuya göre yön sprite).
- **Agentic AI orkestrasyon altyapısı** — cx (Codex/kod dispatch, profil auto-rotate), ax→agy (Gemini, ConPTY dispatcher; /p /ask_gemini /generate_image komutları), NLM knowledge base (NotebookLM canonical tasarım kaynağı), Unity MCP otomasyonu, multi-account routing.
- **Editör araçları** — tile importer'lar (PixelLab PNG sheet / Wang), pivot batch fix, depth-band SO generator, skill icon registry builder vb.

### 🔮 "Neler eklenebilir" — Yol Haritası bölümü (user vurguladı — RAPORA EKLE)
Portal/preview-ada/cyan-orb ışınlanma, organik prosedürel iso odalar, çift-sınıf ultimate sistemi tam entegrasyon, boss çeşitliliği (Act 2-4), ses/müzik, Steam yayın hazırlığı. (Ayrıntı CURRENT_STATUS + NLM.)

### Referans video analizi (ax/agy, 2026-06-01)
- `STAGING/agy_video_analysis_d2_sprites.md` — "Building Diablo 2 with AI / 16-dir iso sprites" videosu analizi → RIMA'nın bunu nasıl aşacağına dair çıkarımlar. Sanat-yönü + sprite pipeline bölümüne kanıt/karşılaştırma olarak eklenebilir.
- **VIDEO-2 "Pixel Art Iso Tilesets" (Apox Fox) — `STAGING/agy_video_analysis_iso_tilesets.md` (transcript-temelli, GERÇEK):** İlkeler: Rule-of-Two (2:1), 3-yüz küp ışık (üst parlak/sol orta/sağ koyu), floor net-wall koyu kontrast, Z-as-Y katman. RIMA AKSİYONLARI: (1) PixelLab prompt şablonu floor/wall (iso projection + top-left light + slate base + cyan #00FFCC crack + "seamless interior --no vertical walls"); (2) Unity import **Mesh Type: Full Rect** (Tight DEĞİL), PPU 64, cell (0.96,0.585,1); (3) **"Diamond Masking" / AI-to-Iso crop editor tool** (AI tile'ı 0.96x0.585 elmasa maskele, taşan pixel alpha=0); (4) **CLIFF ÇÖZÜMÜ ⭐ (kuyruktaki taşma sorununa):** AI'dan SADECE düz üst-elmas üret → cliff'i Unity'de script/shader ile aşağı EXTRUDE + dark iron'a (#1A1E24) karart; AI'a dikey duvar çizdirme; (5) Wang seamless: AI "borderless seamless" + Unity Rule Tile + perlin dağıtım + cyan crack varyantı seam kamuflajı. ⚠️ Gemini sort-axis (0,1,-0.26) önerdi AMA bizim LOCK (0,1,0) [[feedback-depth-sort-custom-axis-not-manual-ysort-s6]] — test et, körü körüne uygulama.
- **ax çıkarımları (rapora + üretime gir):** video=3D→2D pre-render 16-yön (plastik/çamurlu, baked-ışık URP2D ile çakışır). RIMA üstünlüğü: (1) **8 yönde kal** — snappier, az bellek; (2) **flat-lit sprite + Unity Light2D** (PixelLab prompt'a `flat lighting/unlit/no cast shadows`); (3) **smear-frame, 10-12 fps** (smooth değil snappy); (4) **palet disiplini** — hex prompt + Color-Swap/Palette shader; (5) **iso pivot ayakta/tile-merkez** (karakter süzülmesin); (6) **auto-pivot + 8-yön klip auto-slice editor tool** yaz (video'nun Sprite Analyzer muadili) — bu da "ne tool yaptık" bölümüne aday. NOT: Gemini videoyu frame-frame izlemedi, konu-bilgisiyle analiz etti.

## Önerilen yapı (~30-35 sayfa) — REVİZE 2026-06-06 (council kararı: `STAGING/REPORT_CONTENT_DECISION_2026-06-06.md`)
**İlke: anlatı akışı "ne yaptık → nasıl inşa ettik → nasıl çalıştık → nasıl doğruladık". Unity/C# tarihçesi-tanımı YOK (jüri biliyor; teknoloji seçimi = 1 paragraf gerekçe). Aşırı teknik detay YOK — file:line/algoritma matematiği rapora girmez, Ek'teki kanıt matrisine gider.**
1. Kapak / İçindekiler / Özet (TR) + Abstract (EN)
2. **Giriş** (3 s) — problem: tek geliştiriciyle kapsamlı aksiyon-roguelite; yaklaşım: veri-güdümlü mimari + AI-destekli süreç. [kullanıcı son rötuş]
3. **Oyun: RIMA ve Oynanabilir Döngü** (6 s) — tür + 1 sayfalık referans-oyun karşılaştırma matrisi (Hades/Dead Cells/StS × oda-akışı/harita/seçim) BURAYA gömülü (ayrı literatür bölümü YOK); tam döngü anlatısı (menü→Attunement Chamber yürünebilir seçim→run→draft→dallanan kapılar→boss→victory/ölüm→Shattered Echo); 1 örnek sınıf üzerinden skill/sinerji; state-machine 1 basit şema.
4. **İnşa: Veri-Güdümlü Oda Sistemi ve Araçlar** (7 s) — "oda = veri" fikri (RoomTemplateSO), JSON import ile ChatGPT'ye oda tasarlatma hikâyesi, 26 odalık havuz, otomatik cliff + Poisson-disk prop (1'er paragraf, matematiksiz), Map Designer + Room Browser ("ne tool yaptık" = en özgün mühendislik kanıtı); teknoloji gerekçesi (Unity 6 URP 2D, pixel-perfect, depth-sort) 1 sayfa alt-bölüm.
5. **Görsel Pipeline** (2-3 s) — PixelLab AI pixel-art: 10 sınıf × 8 yön (5 üretim + 3 ayna flipX); **GÜNCEL canvas 120-128px kare (sınıfa göre; ölçülü: Warblade/Brawler/Elementalist 120, Ranger/Ronin 128, diğerleri 124), görünür gövde ~64px, PPU 64, Point filter**; iso floating-island sanat yönü kısaca.
6. **Süreç: AI-Destekli Çok-Ajanlı Geliştirme Metodolojisi** (5 s) — orchestrator+coder+council, yazar≠reviewer cross-QC, karar dökümanları; öğrencinin rolü = sistem mimarı/pipeline tasarımcısı (algı-riski çerçevesi); vaka: 10-task otonom kuyruk.
7. **Doğrulama: Test ve Kalite** (4 s) — ~85 EditMode + 11 PlayMode dosya / ~490 test / son koşu 410 PASS; görsel oda-QC SÜRECİ anlatısı (26 oda tarandı → 2 FAIL + sistemik bug → kök neden → fix → re-QC) — "süreç hata buldu ve düzeltti" hikâyesi.
8. **Karşılaşılan Zorluklar ve Çözümler** (3 s) — 4-5 seçme vaka, problem→teşhis→çözüm: D3D12 crash→D3D11, cliff taşması, singleton'ın Systems'i öldürmesi, iso derinlik.
9. **Yol Haritası + Sonuç** (2 s) — [kullanıcı son rötuş]
10. **Kaynakça + Ekler** — kanıt matrisi (iddia→dosya/test/screenshot), 12-kare screenshot seti, metrik tabloları.

## 🎨 ART DIRECTION LOCK (concept odalardan, 2026-06-01 — rapor "sanat yönü" bölümü + build hedefi)
agy concept odaları (`STAGING/imagegen/concept0{1,3,5,7}_*_ISO.png`) = north star. Görsel dil:
- Yüzen izometrik taş ada, **mor void** atmosferi (purple swirl), high top-down 3/4.
- **KOYU slate/granit** zemin (soluk/beyaz DEĞİL); zemin derzlerinde **cyan #00FFCC enerji çatlağı/rune** ölçülü parıltı (~%5-8, bloom glow).
- Ada kenarlarında **KALIN KOYU GRİ taş cliff** aşağı düşüyor (kahverengi YASAK).
- Oda-tipi öğeleri: kırık ruined-keep duvarı (hero), rune-kazılı tile + cyan swirl portal + sandık (reward), concentric rune-ring + zincir + cyan boss-summon (boss arena).
- Painterly ama net, güçlü siluet. (Bunlar agy/Imagen illüstrasyonu = yön; in-engine = modüler tile + Light2D + bloom ile gerçekleştirilir — ax kararı (A), `STAGING/agy_build_decision_concept_realization.md`.)

## 📸 Screenshot ihtiyaç listesi (rapor için)
- [ ] iso floating-island floor (game-view + scene-view) — `_IsoGame`
- [ ] büyük iso oda demo (rebuild gerekebilir — scratch sahne restart'ta gitti)
- [ ] karakter sprite (8-dir sheet, örn. Elementalist/Warblade)
- [ ] Map Designer aracı UI
- [ ] concept images (`STAGING/imagegen/`)
- [ ] combat / room-flow / HUD / minimap (varsa)
- [ ] boss arena
- [ ] mimari diyagram (script ilişkileri — çizilecek)

## İş akışı (Claude tasarrufu — kullanıcı direktifi)
- **Opus:** outline + section brief + final review/stitch (düşük token, yüksek değer).
- **cx (Codex):** bölüm draft yazımı + docx üretimi (create_ara_rapor.py uyarla).
- **ax/agy (Gemini):** uzun-form draft, referans oyun analizi genişletme, dil cilası.
- Çıktı: `ara_rapor_ornek.docx` formatına uygun docx.

## 🔁 SÜREKLİ DÜŞÜN — "ne eklenebilir" running list
- Screenshot'lar CANLI prototipten çekilsin (inandırıcılık).
- Sınıf renk paleti tablosu (project_class_colors).
- Geliştirme zaman çizelgesi / sprint geçmişi (proje yönetimi kanıtı).
- Risk & kapsam yönetimi bölümü.
- "Agentic AI ile solo geliştirme" = en özgün katkı, ayrı vurgu + belki bir akış diyagramı.
- Performans/optimizasyon notları (pixel-perfect, tilemap, sprite atlas) eklenebilir.
- Kullanıcı testleri / playtest bulguları (varsa STAGING/playtest_report).
