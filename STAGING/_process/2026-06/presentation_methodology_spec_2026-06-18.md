# GÖREV: Sunum "çalışma prensipleri" segmenti + Rapor §8 alt-başlığı

ACTIVE RULES: (1) think before writing (2) min, no speculation (3) surgical (4) BLOCKED if unclear.
**TÜM TÜRKÇE KARAKTER ZORUNLU** (ç ğ ı İ ö ş ü). Akademik + sunum dili. Unity'ye DOKUNMA.

## Amaç
Kullanıcı bitirme SUNUMUNDA hocaya "AI ile nasıl çalıştığımızı / yapımız için ne yaptığımızı" gösterecek. İki çıktı üret:
**(A)** Raporun Bölüm 8'ine eklenecek DENGELİ bir alt-başlık metni (council uyarısı: AI'yı güzelleme YAPMA; geliştirici-yönetir / AI-uygular / her şey denetlenir disiplinini öne çıkar).
**(B)** Hocaya 2-3 dk CANLI CLI demo script'i + Türkçe konuşma notları (provalı, GÜVENLİ — yıkıcı/uzun-süren komut YOK).

## GERÇEK workflow (bunları kullan, UYDURMA — kendi başına araştırma, brief bağlayıcı)
**Orkestra:** Geliştirici (yön veren) → **Claude Code (Opus 4.8) = orchestrator** (kullanıcının içinde olduğu CLI). Orchestrator bulk iş YAPMAZ; işi böler, dağıtır, denetler, senteze yapar.
**Uygulayıcılar:** `builder-opus` (Claude alt-ajan, kod yazar) · `crafter-sonnet` (hafif/mekanik) · **cx = Codex (gpt-5.5)** background dispatch · **ax = Gemini** (3.1 Pro = derin/vision, 3.5 Flash = yalın/hızlı).
**Denetim:** `auditor-opus` (salt-okunur, bağımsız QC) · **council** (cx + ax Pro + ax Flash çoklu-danışman sentezi) · KURAL: **writer ≠ reviewer** (yazan denetlemez), her değişiklik bağımsız geçer.
**Araçlar:** **UnityMCP** (Claude Unity Editor'ü CANLI sürer — sahne yükler, screenshot çeker, console okur) · **PixelLab MCP** (piksel-art sprite üretimi) · **graphify** (kod → bilgi grafiği, 6925 node, sorgulanabilir AI hafızası) · **NotebookLM** (tasarım-kararı bilgi tabanı) · kalıcı **memory dosyaları** (oturumlar arası hafıza).
**Prensipler:** insan NE/hangi-sıra/kabul-kriteri belirler; AI denetlenen bir hatta uygular; çok-sistem kesen kararlar council'e; iddialar **data-proof** ile doğrulanır ("yeşil test = çalışıyor değildir"); bilgi memory/NLM/graphify'da kalıcılaşır.

## (A) Rapor §8 alt-başlığı — taslak içerik
Başlık önerisi: **"8.x İnsan-AI İş Bölümü ve Çalışma Prensipleri"**. ~2-3 paragraf:
- Tek geliştirici, "stüdyo gibi" çalıştı: orchestrator + uzman ajan rolleri (uygulayıcı/denetçi/danışman) + araçlar.
- Disiplin: writer≠reviewer denetim kapısı, council ile kritik kararlar, data-proof doğrulama. AI = kaldıraç, yön = geliştiricide.
- Mühendislik çıktısına bağla (prompt-engineering güzellemesi DEĞİL): bu yapı sayesinde 1 kişi çok-sistemli bir dikey-dilim + editör araç zinciri üretebildi.
- Ton: ölçülü, akademik; "otonom AI" abartısı yok.

## (B) Canlı CLI demo script — provalı + güvenli
2-3 dk akış öner (her adım: NE yazılır/gösterilir + hoca'ya NE söylenir). GÜVENLİ örnekler (yıkıcı değil):
- Ajan kadrosunu/rolleri tek ekranda göster (ör. agent listesi / bir dispatch task .md / `MEMORY/MEMORY.md`).
- CANLI tek aksiyon: Claude'a "oyunun ekran görüntüsünü al" de → UnityMCP ile screenshot çıksın (etkileyici + güvenli; ~10sn).
- Denetim kanıtı: bir `AUDIT_*.md` / council çıktısı göster — "her değişiklik bağımsız denetlenir".
- Hafıza: `graphify query` ya da `MEMORY.md` göster — "proje hafızası sorgulanabilir".
- Kapanış cümlesi: insan yönetir, AI denetimli uygular.
- ⚠️ Demo riskleri + B-planı (internet/Unity kapalıysa ne gösterilir — hazır screenshot/artifact ekran görüntüleri) ekle.

## Çıktılar (2 dosya)
- (A) → `STAGING/_process/2026-06/REPORT_S8_WORKINGPRINCIPLES.md` (writer rapora gömecek)
- (B) → `STAGING/report/SUNUM_CANLI_CLI_DEMO.md` (bağımsız sunum el-kitabı)
Bana dönüş ≤10 satır: iki dosyanın yolu + (B) demo akışının adım başlıkları + riskli/varsayım nokta.
