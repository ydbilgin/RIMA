# RIMA — Bitirme Sunumu HAZIRLIK (2026-06-11)

> Council (cx laurethayday ground-truth + ax 3.1 Pro strateji + ax 3.5 Flash risk) → Opus sentez. Sorular: `STAGING/_process/2026-06/_council_*_presentation-prep.md`.

## 🎯 ANA MESAJ (one-liner — son slaytta bu kalsın)
> *"RIMA sadece oynanabilir bir oyun değil; rol-tabanlı otonom AI ajanlarından oluşan sanal bir stüdyonun, insan mimarisi (system architect) altında ne kadar karmaşık ve doğrulanmış bir yazılım sistemi inşa edebileceğinin çalışan kanıtıdır."*

## 🧭 TEZ ÇERÇEVESİ
- **%70 pipeline/mimari (katkı/novelty) · %30 oyun (kanıt/validasyon).** Oyun = pipeline'ın çalıştığını ispatlayan **case study**.
- Akademik isim: *"Karmaşık yazılım inşası için Çok-Ajanlı (Multi-Agent) AI Orkestrasyon Çerçevesi + veri-güdümlü roguelite mimarisi: RIMA vaka çalışması."*
- Novelty cümlesi: **"prompt-at-geç değil"** — rol-tabanlı (orchestrate/execute/council), **author≠reviewer ayrımı**, kalıcı bilgi-tabanı (NotebookLM), doğrulama-kanıtı (task files + decision docs).

## ⚠️ KRİTİK DÜZELTME (yanlış iddia ETME)
**RIMA'nın AI-pipeline'ı GELİŞTİRME-ZAMANI (dev-time), runtime DEĞİL.** Oyun çalışırken LLM çağırmıyor; odalar önceden-üretilmiş ScriptableObject. → Demo'da **"AI canlı oda üretiyor" GÖSTERME/SÖYLEME.** Pipeline'ı **metodoloji** olarak göster (council kayıtları, commit geçmişi, decision docs, tooling) + oyunu **sonuç** olarak.

## 🎬 3-PERDE AKIŞ (~8-10 dk)
1. **Kanca + Tez (1-2 dk):** *"Bugün size bir oyun değil, onu üreten sanal stüdyomuzu sunuyoruz."* Problem (oyun-geliştirme karmaşıklığı) → çok-ajanlı çözüm. Kısa tut.
2. **CANLI DEMO (2-3 dk) — şüpheyi sil:** çalışan oyunu göster (aşağıda runbook). Önce kalite/çalışırlık kanıtı.
3. **MİMARİ + PIPELINE (2 dk):** Claude orchestrator + Codex executor + Gemini council + NotebookLM KB + Unity/PixelLab MCP şeması. "Nasıl yaptık" = asıl tez.
4. **TOOLING (1.5 dk) — asıl "vay canına":** Room Browser + Map Designer (aşağıda). "26 odayı tek tek elle değil, AI-destekli sistematik tooling ile yönettik."
5. **SONUÇ + DEĞERLENDİRME (1 dk):** metrikler + öğrenilen dersler + limitler (context-loss/halüsinasyon → NotebookLM KB + reviewer ile çözdük) → one-liner.

## 🕹️ CANLI DEMO RUNBOOK (editör-first — cx onaylı)
**Giriş:** Unity menü → `RIMA/Play From Main Menu` (hangi sahne açık olursa olsun MainMenu'den boot eder).
1. **MainMenu → CharacterSelect** ("dinamik sınıf kimliği").
2. **Attunement Chamber:** Warblade veya Elementalist seç (demo bu 2'ye kilitli) → rift'e gir → `_Arena`.
3. **Run (forced sequence):** Combat → Combat → **Shop** (Merchant) → Combat → **Boss** → post-boss. Söyle: "akış deterministik, jüri için sabitlenmiş."
   - **EN İYİ ODALAR (cx):** ilk combat = `combat_large_cross_01` (okunur) · büyük gösteriş = `combatlarge_twin_basins_01` (38×24) · klimaks = `boss_shattered_oval_01` (36×28) · post-boss = `combat_large_hourglass_01`.
4. **Boss → dual-class:** boss öldür → sınıf-seçim overlay → ikinci sınıf seç → unlock draft (3 kart) → kapı → post-boss birleşik kit → **Victory (DemoCompleteOverlay)**. **⚠️ Bu anı ÖNCEDEN PROVA ET** (en kritik moment, softlock guard'lı + 90s timeout ama yine de).

## 🛠️ TOOLING GÖSTERİMİ (gameplay'den SONRA)
- **`RIMA/Room Browser`:** oda tıkla → `_Arena`'da canlı kurulur + SceneView çerçeveler. 26 oda genişliğini göster. **⚠️ Play Mode'da DEĞİL** (play mode'da çalışmaz) + `_Arena`'da IsoRoomBuilder olmalı.
- **`RIMA/Map Designer` (Rooms sekmesi):** authoring kanıtı — library count, Build in Arena, Auto Props, JSON export, validation rozetleri, kapı soketleri. **Asset'i canlı düzenleme** (riskli); sadece butonları/validation'ı göster.

## 📊 METRİKLER (slayt için — cx ground-truth, KÜRATE edilmiş)
- **590 git commit** (sistematik, izlenebilir süreç).
- **~600 otomatik test** (637 test marker: 592 EditMode + 41 PlayMode). **Dürüst ifade:** "son tam suite **584/599**; 15 bilinen fail PRE-EXISTING (bu koşunun regresyonu değil)." → **"tüm testler geçiyor" DEME.**
- **Oda:** ~26 template tasarlandı, repo'da 21 aktif .asset (15 Generated); **demo'da 13 oda** (9 combat + 2 elite + 1 boss + 1 shop).
- **AI kadrosu:** 6 rol (Claude Sonnet orchestrator · Codex/cx executor · Gemini 3.1 Pro · Gemini 3.5 Flash · Opus · reviewer) + 3 MCP (NotebookLM design-KB · Unity · PixelLab).
- **Asset (kürate):** 80 karakter PNG · 12 Act-1 mob · 83 UI ikon · 16 VFX · 335 environment PNG · 21 ses. (**Ham 3935/3601 totalini KULLANMA** — şişkin görünür.)

## 🎓 HOCA SORULARI + SAVUNMA (3.1 Pro — ezberle)
- **"Madem AI yaptı, senin katkın ne?"** → *"Ben sistem mimarıyım. Orkestrasyonu, context-aktarım altyapısını, hata-denetim (reviewer) mekanizmalarını ve prompt-mühendisliğini ben tasarladım. Tuğlayı AI dizdi; binanın statik hesabını ve mimarisini ben yaptım."*
- **"Copilot/ChatGPT'den farkı?"** → *"Copilot reaktif, satır tamamlar. Bizimki proaktif + rol-tabanlı: Claude orkestre eder, Codex uygular, Gemini konseyi denetler. Tek-AI limitini diğer modellerle kapatan sistemik pipeline."*
- **"Sınırlamalar?"** → *"En büyük sorun context-loss + halüsinasyon. Çözüm: NotebookLM tabanlı kalıcı design-KB + author≠reviewer denetim + standardize task/decision docs."*
- **"Ölçeklenebilir mi?"** → *"Süreç tekrarlanabilir/ölçülebilir (task file → execute → council-review → verify). Yeni özellik aynı boru hattından geçer."*

## 🛡️ RİSK + YEDEK (3.5 Flash + cx)
- **YEDEK VİDEO ŞART:** kesintisiz tek-parça demo kaydı (gameplay + tooling) masaüstünde hazır. Canlı patlarsa sunumu kurtarır.
- **Editör-first demo** (cx: son build oda-havuzundan ESKİ → stale; build kullanacaksan ÖNCE taze release build + MainMenu→Boss→Victory smoke).
- **Boss→dual-class→draft→post-boss rotasını prova et** (en kritik an).
- **Legacy YOLLARI gösterme:** `_IsoGame*`, MainMenuScreen, RuntimeRoomManager, GateBehavior (demo-dışı).
- Test ifadesini dürüst kur (584/599, pre-existing). Shop "Echo→Gold" çözülmedi — sorulursa "bilinçli, meta-currency tutarlılığı post-demo."

## ✅ MİN HAZIRLIK ÇEKLİSTİ (sunumdan önce)
1. **Yedek video çek** (gameplay full loop + Room Browser + Map Designer).
2. **Kronometreyle 2× kuru prova** (süre aşımı en sık hata).
3. **Masaüstü temizliği:** build/video/slayt tek klasörde, tek tık.
4. Unity açık + `_Arena` IsoRoomBuilder bağlı + Room Browser/Map Designer test-aç.
5. Boss→dual-class anını canlı dene (softlock yok mu).

## 🚫 SAKIN YAPMA
- "AI canlı oda üretiyor" deme (dev-time pipeline). · "Tüm testler geçiyor" deme. · Canlı internet/LLM API'sine bel bağlama. · Slaytlara metin yığma. · Legacy sahne gösterme. · Ham asset-totali (3935) söyleme.
