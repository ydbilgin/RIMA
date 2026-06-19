# Report Finalize — 2026-06-19 Changelog

P1 §11.9: timeScale dual-owner çözümü DÜZELTİLDİ — HitPauseDriver=yazma sahibi, HitStop=[Obsolete] kaldırıldı (önceki metin tersiydi, kod-doğrulandı).
P1 §8 model-name leaks: §1.3 Codex/cx/Gemini/ax → "Kod Üretim Modülü / Mimari Çapraz-Doğrulama kanalları¹"; §8.3 satır 497 → soyut roller; §8.5 satır 515 "kota dolan Codex" → "kota dolan uygulayıcı ajan"; §8.6 satır 529 "Claude Code" → "orkestrasyon katmanı¹".
P1 Dipnot: blockquote (> ¹) → düz paragraf (¹ ...) — DOCX generator blockquote işlemiyor, doğrudan paragraf daha temiz.
P1 Özet/Abstract: OZET_ABSTRACT.md içeriği rapor başına (## 1 öncesi) ## ÖZET + ## ABSTRACT başlıkları olarak enjekte edildi; DOCX'te H1 olarak görünüyor.
P2 §4.4 çapraz-ref: "ayrıntı Bölüm 9" → "(ayrıntı §11.4)" düzeltildi.
P2 Boss adı: §4.1 boss paragrafına "Demo karşılaşmasının kod-tarafı adı Penitent Sovereign'dır (bkz. §11.8)." eklendi.
P2 §11.11: Düz paragraf → Sorun/Teşhis/Çözüm/Çıkarım şablonuna oturtuldu.
EK A: 7 playtest ekran görüntüsü Şekil 13–19 olarak eklendi (warblade-card0, lmb-uçuş, lmb-patlama, burn-kırmızı, chill-mavi, buildmode-prop, cliff-generate); make_akademik_docx.py'ye PLAYTEST_2026 dizini FIGURE_DIRS'e eklendi.
DOCX rebuild: BAŞARILI — 0 hata, 20 gömülü görsel (19 figür + kapak logo), 16 H1, 12838 KB. Tüm 7 yeni figür çözümlendi (OK).
UNCOMMITTED — git commit yapılmadı.
