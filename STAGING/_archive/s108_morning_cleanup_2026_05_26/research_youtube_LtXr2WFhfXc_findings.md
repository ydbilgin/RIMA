# Research: YouTube Video LtXr2WFhfXc — RIMA Findings

**Date:** 2026-05-22
**URL:** https://youtu.be/LtXr2WFhfXc
**Gemini Model:** Default (gemini-2.5-pro-preview via settings.json)
**RIMA Relevance:** MEDIUM — Doğrudan oyun tasarımı değil, AI-assisted development workflow.

---

## Video Özeti

Video, **Selman Kahya** tarafından hazırlanmış bir Türkçe teknik içerik. Başlık: "Claude Code Token'larımı 4'e Böldüm — Yorumları Cevaplıyorum." Konu: her sabah Discord'a AI haber özeti atan bir "Sabah Asistanı" otomasyon projesi — ve bu projenin Claude Code kullanırken nasıl 120.000 token/çalıştırma'dan 30.000'e indirildiği.

Sorun şuydu: Claude Code'a web sitelerini ham HTML olarak okutmak devasa token yanıyordu. Çözüm: **"Karar gerekiyorsa LLM, gerekmiyorsa kod kullan"** prensibi. Go ile yazılan "Briefer" adlı küçük bir CLI aracı ham veriyi temizleyip sadece karar verisini LLM'e iletir hale getirildi. Token maliyeti %75 düştü.

Videodaki ikinci katman: Go dilinin bu iş için neden seçildiği (hız, cross-platform binary, basitlik), Discord webhook entegrasyonu, ofis-ev senkronizasyon sistemi. Oyun geliştirme özelinde değil ama **AI-assisted tooling mimarisi** açısından doğrudan transfer edilebilir dersler içeriyor.

---

## RIMA için Actionable Çıkarımlar

### 1. Codex Dispatch Token Maliyeti — Validator-First Pattern
Codex'e JSON harita dosyası veya scene state'i doğrudan okutmak yerine, önceden çalışan bir validator script (C# EditMode test veya Python lint) çıktısını Codex'e ver. Codex sadece "hata var mı / nerede?" sorusunu yanıtlar, tüm JSON'u parse etmez. Bu mevcut `codex_task_*.md` dispatch mimarisine uyar — task dosyasına "önce bu validator'ı çalıştır, çıktıyı oku, ondan sonra düzelt" adımı ekle.

### 2. PixelLab API Call Zincirinde LLM'i Daralt
PixelLab batch üretiminde (tiles_pro, modular pack) stil kararı (renk paleti, tile tipi seçimi) LLM'e, teknik parametreler (width/height/n_frames/seed) ise sabit bir template JSON'a taşı. Şu anda `pixellab_tile_production_batches_v1.md` içinde parametreler hem tasarım hem teknik karışık gidiyor — ayır.

### 3. JSON Map System — "Briefer" Eşdeğeri
Planlanan JSON-driven map sistemi için harita verisini Claude/Codex'e iletmeden önce bir "map briefer" script'i yaz (Python veya C# EditorScript): oda sayısı, bağlantı tutarlılığı, eksik tile referansları gibi summary metrics üretsin. Codex bu summary'yi okur, ham JSON'u değil. Act 1 vertical slice (6 oda) şu an yönetilebilir boyutta ama ilerleyen fazlarda kritik.

### 4. Autonomous Codex Dispatch — Deterministik vs. Karar Görevleri Ayrımı
Mevcut Codex dispatch task'larına şu ayrımı standart olarak ekle:
- **Deterministik (koda devret):** dosya kopyalama, sprite slice parametreleri, Unity AssetDatabase refresh, Unity compile error check.
- **Karar (LLM'e ver):** hangi tile varyantı seçilecek, hangi oda layout'u tutarlı, hangi karakter animasyon frame'i eksik.

Bu `CODEX_TASK_*.md` template'ine "DETERMINISTIC STEPS" + "LLM DECISION STEPS" section'ları olarak formalize edilebilir.

### 5. Codex Oturumu Başında Bağlam Minimizasyonu
Video'nun "temizlenmiş veri" prensibi, RIMA'daki `feedback_nlm_first_context_strict.md` ile örtüşüyor ama daha spesifik bir şey söylüyor: Codex'e oturum başında tüm proje dosyalarını taratma, sadece o task için gereken "pre-digested" özeti ver. Örneğin weapon decouple task'ı için Codex'e `CODEX_TASK_phase2_weapon_decouple_level2.md` + sadece ilgili 2-3 script path'i ver, `Assets/` klasörünü taratma.

### 6. Modular Pipeline'da Go/Script Binary Eşdeğeri
Modular asset pack workflow'unda tekrarlanan işler (atlas slice, contact sheet üretimi, tileset inventory JSON güncelleme) şu an Python scriptlerle yapılıyor. Bu scriptleri tek bir CLI entry point'ten çağırılabilir hale getir — Selman'ın "Briefer" CLI'ına benzer şekilde. Bu, Codex task'larında "önce bu script'i çalıştır" adımını standartlaştırır.

### 7. Discord/Notification Entegrasyonu (Düşük Öncelik)
Video'da Discord webhook kullanımı anlatılıyor. RIMA'da autonomous Codex dispatch tamamlandığında ya da PixelLab batch bittiğinde Discord notification (veya en basiti: STAGING'e sonuç dosyası + console push notification) eklenebilir. S98 autonomous roadmap'te bu bir hook olarak değerlendirilebilir.

---

## Sonuç

Video oyun tasarımı değil, AI workflow optimizasyonu hakkında. RIMA açısından doğrudan oynanış veya combat design çıkarımı yok. Ancak **AI-assisted tooling mimarisi** (Codex dispatch, PixelLab batch, JSON map system) için somut ve transfer edilebilir bir prensip sunuyor: deterministik işleri kod yapsın, LLM sadece karar versin. Bu prensibin RIMA Codex dispatch template'ine ve modular pipeline script'lerine uygulanması token maliyetini ve hata oranını düşürür.

**Öneri:** Bu çıkarımları `CODEX_TASK_*.md` template standartlarına ekle; deterministik/karar ayrımını task formatının bir parçası haline getir.
