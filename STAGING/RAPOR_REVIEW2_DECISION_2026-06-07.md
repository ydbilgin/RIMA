# KARAR — ChatGPT Rapor Review-2 Paketi Değerlendirmesi (2026-06-07)

**Council:** cx (olgusal denetim, kanıt:dosya:satır) + ax-3.1-Pro (derin/jüri lensi) + ax-3.5-Flash (lean/maliyet lensi) → Opus sentez.
**Girdi:** `STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/` (30 bulgu).
**Hedef:** `STAGING/report/RAPOR_DRAFT_2026-06-06.md` → `RAPOR_RIMA_2026-06-06.docx`.

## Council'in düzelttiği ChatGPT hataları (cx kanıtlı)
1. **Test sayıları bayat:** Gerçek envanter **508 EditMode + 41 PlayMode = 549** (ChatGPT: ~529). Son kayıtlı koşu 410 PASS / 0 FAIL / 1 inconclusive GERÇEK (TestResults_EditMode.summary.txt). Envanter snapshot'tan SONRA büyüdü — rapor bunu böyle anlatacak.
2. **EXIT_NW/EXIT_N/EXIT_NE + ENTRY_S kod adı DEĞİL:** Gerçek: `door_NW_01/door_N_01/door_NE_01` (RoomTemplateSO.cs:40-42), spawn=`player_spawn_01`. Raporda kavramsal etiket olarak kullanılabilir ama kod kimliği gibi SUNULMAZ — köprü cümlesi şart.
3. **#16 before/after "zor" varsayımı YANLIŞ:** Before/after görseller ZATEN DİSKTE (`Assets/Screenshots/RoomQC/` vs `RoomQC_v2/`, ör. combat_large_diamond_01.png) + doğrulama metni. Flash'ın MINIMIZE'ı bu yüzden geçersiz → ACCEPT (maliyet=tek kompozisyon).
4. **25/26 açıklaması doğru ama kapsamlanmalı:** Curated set=26 (Generated 15 + Library 10 + Special 1); 25 run şablonu migre edildi; `Chamber_CharSelect` özel akış (`riftExit` soketi, NW/N/NE konvansiyonu dışı). Ham klasör sayısı 31 (Demo/Sample dahil) — raporda "26" curated set olarak kalır.
5. **Encoding kaynağı:** docx script'i DEĞİL (UTF-8 temiz) — kaynak markdown'da, son 11-edit turunun eklediği §2.6/§3.5.6 + §8'in bazı satırları (791, 805, 807). TÜM rapor taranacak.

## Bulgu-bazlı nihai karar (30 bulgu)

| # | Konu | Karar | Not |
|---|---|---|---|
| 1 | Şekil 1-5 yeniden çekim | **ACCEPT** | ScreenshotMode çıktıları ZATEN VAR (2026-06-07, hud/nohud) — QC edip map'le |
| 2 | kapı→Rift portalı terminolojisi | **ACCEPT+köprü** | Toptan değişim + tek dipnot: "kod mimarisinde tarihsel olarak `door_*`" (3.1 Pro formülü) |
| 3 | Portal yön/slot açıklaması | **MODIFY** | Fikir doğru; EXIT_*/ENTRY_S'yi kod adı gibi yazma — gerçek adlarla köprüle |
| 4 | 25/26 çelişki açıklaması | **ACCEPT (kapsamlı)** | "26 curated; 25 run; Chamber_CharSelect özel akış" |
| 5 | Fallback dili | **ACCEPT** | "Geliştirici güvenlik ağı (fail-safe)" çerçevesi |
| 6 | UI↔JSON mimari değer önce | **ACCEPT** | SO-canonical/JSON-exchange ayrımı bölüm başına |
| 7 | Test adları ekle | **ACCEPT (gerçek adlarla)** | Round-trip/debounce gerçek test sınıf adları kodda doğrulanarak yazılacak |
| 8 | ScreenshotMode = sunum/debug-temizleme aracı | **ACCEPT** | |
| 9 | Ses iki seviye | **ACCEPT** | Demo CC0 SFX ↔ final müzik/prodüksiyon ayrımı |
| 10 | Hit-pause gerekçe | **MODIFY (Flash)** | "Playtest" YAZMA (savunma yükü) → "demo tuning çalışmalarıyla belirlendi" |
| 11 | İnfaz prompt görseli | **MODIFY-OPSİYONEL** | Ayrı şekil YOK; Şekil 3 setinde denenir, olmazsa skip (Flash+Pro uzlaşı) |
| 12 | Reviewer-FAIL tablosu | **ACCEPT** | Gerçek vakalar: knockdown 2-MAJOR · oda-QC 2FAIL+9şüpheli · JSON-editör 3 · T2 9-bug |
| 13 | Reviewer-FAIL tanımı | **ACCEPT** | İlk cümlede akademik tanım |
| 14 | Envanter/koşu ayrımı | **ACCEPT (gerçek sayılarla)** | 549 tanım (508E+41P) · son kayıtlı EditMode koşusu 410/0/1 |
| 15 | Şekil 13 Test Runner | **MODIFY** | Dürüst varyant: güncel filtreli yeşil koşu screenshot'ı + caption "kayıtlı tam koşu=410/0/1" — tam koşu şu an 19 pre-existing fail içerdiğinden tam-koşu görseli ÇEKİLMEZ |
| 16 | QC before/after | **ACCEPT** | Materyal diskte hazır — yan yana kompozisyon |
| 17 | Walkable üçe böl | **MODIFY (Pro)** | Alt başlık değil; tek paragrafta bold mini-yapı |
| 18 | Skill guard cümlesi | **ACCEPT (kanıtlı)** | GetPool `!isImplemented` filtre (SkillDatabase.cs:580-587) + OfferGenerator + fallback; Codex UI envanter gösterir ayrımı yazılacak |
| 19 | "Çıtanın altında..." | **MODIFY (Pro)** | Ruh korunur: "ticari ölçeği birebir değil, mekanik prototipini hedefledi" |
| 20 | Müzik tonu iddiası | **ACCEPT-SFX-varyantı** | Adaptif müzik YOK → sadece "clear SFX + görsel geri bildirim" cümlesi (3/3 uzlaşı) |
| 21 | Gate-slot şeması | **REJECT** | Pro: metin yeterli; ücretsiz alternatif: Şekil 11 (Map Designer Rooms önizleme NW/N/NE etiketli) zaten bunu gösteriyor |
| 22 | Warblade görseli | **ACCEPT-ucuz** | Mevcut sprite'tan nearest-neighbor büyütme + temiz arka plan (render üretimi YOK) |
| 23 | Pipeline diyagramı (Şekil 8) | **ACCEPT** | Pro: "1 numaralı yatırım". Basit temiz diyagram üretilecek |
| 24 | "AI araçtır, süreci ben tasarladım" | **ACCEPT** | Tezin zırhı |
| 25 | Üçlü kalite-güvence tablosu | **ACCEPT** | Test/Görsel-QC/Bağımsız-review katman tablosu |
| 26 | Caption dürüstlüğü | **ACCEPT** | Değişmeyen her şekilde caption görsele uydurulur |
| 27 | §8 test ayrımı | **ACCEPT (549/410)** | |
| 28 | Boss törpüleme | **ACCEPT** | |
| 29 | AI kayıt/lisans | **MODIFY (Flash)** | "Prompt kayıtları dokümante edilmiştir" YAZMA → yumuşak: "jeneratif araçlar akademik prototip kapsamında; CC0 ses lisans dosyaları projede" (lisans dosyası GERÇEK, commit'li) |
| 30 | Encoding temizliği | **ACCEPT (tüm rapor)** | §2.6/§3.5.6/§8 + tam tarama |

**Skor: 19 ACCEPT · 8 MODIFY · 1 REJECT · 2 ACCEPT-varyant.**

## Uygulama planı (3 lane, ~2-2.5 saat)

**Lane 1 — Metin pass'i (Sonnet agent, tek geçiş, ~60-75 dk):** #2,3,4,5,6,7,9,10,13,14,17,18,19,20,24,27,28,29,30 + #12/#25 tabloları. ⚠️ Agent brief'ine AÇIK kural: **tüm metin tam Türkçe karakterlerle** (önceki turun ASCII hatası tekrarlanmayacak).
**Lane 2 — Şekil seansı (~45-60 dk):** (a) FIGURE_FILES mapping'e mevcut 11/12 dosyalarını bağla (bedava) · (b) Şekil 1-5'i screenshots_auto/ çıktılarından seç+QC · (c) Şekil 14 before/after kompozisyonu (RoomQC vs RoomQC_v2) · (d) Şekil 6 Warblade büyütme · (e) Şekil 8 pipeline diyagramı üret · (f) Şekil 13 filtreli yeşil Test Runner screenshot (Unity açık).
**Lane 3 — docx re-gen + verify + son okuma (~10 dk).**

**Kullanıcı gate'i:** Yeni şekil seti embed edilmeden önce kullanıcıya kontak-sheet gösterilir (beğeni süzgeci protokolü).

## Ders (memory'ye)
Doc-yazan agent'lar memory-ASCII konvansiyonunu akademik metne taşıyabiliyor → rapor/sunum edit brief'lerine "tam Türkçe karakter ZORUNLU" satırı eklenecek. Orchestrator QC'si edit listesine değil ÇIKTI metnine de bakmalı.
