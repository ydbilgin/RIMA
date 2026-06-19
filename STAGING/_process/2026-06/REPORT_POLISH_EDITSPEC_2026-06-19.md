# RAPOR CİLA EDIT-SPEC (Opus sentez/karar — council ax_pro + ax_flash)

Kaynak council: `_council_axpro_report_polish_2026-06-19.md` + `_council_axflash_report_polish_2026-06-19.md`.
Hedef: `STAGING/report/RIMA_Senior_Design_Report.md` → rebuild `make_akademik_docx.py`.
**UYGULAMA ZAMANI:** Track A (playtest) bitince, §11 entegrasyonu + Unity screenshot ile TEK birleşik pass. (§11 hem council hem Track A dokunuyor → çift-edit yok.)

## 🎯 GLOBAL İLKE (Opus kararı — advisor'ları DENGELE)
Her iki advisor "AI-odağını azalt" diyor AMA AI-destekli çok-ajanlı geliştirme = bu tezin ÇEKİRDEK KATKISI. Karar: **dili akademikleştir, katkının özünü KORU.** §8'i silme/gutlamama; model kod-adlarını prosadan çıkarıp rol-tabanlı terimlere çevir + tek dipnot/tablo. "Fiction/devlog" tonunu temizle, metodoloji katkısını nesnel anlat.

## UYGULA (yüksek güven — iki advisor hemfikir veya net doğru)

1. **Şekil atıfları (ax_pro P1) — UYGULA.** Gövdede 12 figürün hiçbirine "(bkz. Şekil N)" / "Şekil N'de görüldüğü üzere" atfı yok. Her figürün yakın paragrafına net atıf ekle. "Aşağıdaki şekil…" → "Şekil N'de…". (En yüksek akademik kazanç, düşük risk.)

2. **Oyun/İngilizce jargon + "Centerpiece" (ax_pro P1 + ax_flash P2.4) — UYGULA.** §5 başlığından "(Centerpiece)" çıkar. İlk-kullanımda Türkçe + parantez orijinal, sonra Türkçe devam. Eşleme (ax_flash): yalan telegraph→telegraph-hasar uyumsuzluğu · void→harita dışı geçersiz alan · bespoke kalır→özelleştirilmiş yapısını korur · Rage bar→Öfke göstergesi · clobber→üzerine yazma · "az iş çok his"→minimum kaynakla yüksek oyun hissi · Run→Oturum (Run) · Tooling→Geliştirme Araçları · Spawn→üretim/doğurma · Idle→boşta. **NOT:** "Build Mode"/"Director Mode" tool öz-adları kalsın (ilk kullanımda Türkçe gloss).

3. **AI insansallaştırma + model adları (ax_pro P2 + ax_flash P1.2 — İKİSİ) — UYGULA (özü koru).** Prosadan çıkar: "builder-opus", "crafter-sonnet", "cx/gpt-5.5", "ax/Gemini 3.1 Pro", BLOCKED/DONE operasyonel mesajları. Yerine rol-soyutlama: "Kod Üretim Modülü", "Kalite Kontrol Ajanı", "Mimari Çapraz-Doğrulama Modelleri". "Danışman Konsey"→"Mimari değerlendirme / çapraz-doğrulama"; "anlaşmazlıktan değer üretme"→"farklı modellerin çapraz denetimiyle risk tespiti". Model adları gerekiyorsa TEK dipnot/tablo. **Metodoloji katkısı KALIR, sadece dil nesnelleşir.**

5. **Ham JSON / QC-log blokları (ax_flash P1.3) — UYGULA (özetle).** §6.4 ham JSON (act1_entry_hall.json/door_graph) + §9.3 ham log ([RoomQCFix] PASS…) → şematik özet/tablo. (Appendix yerine in-place kondense = daha az risk.)

6. **"Ders:" devlog formatı (ax_pro P2) — UYGULA.** §11'deki "Ders:" → "Çıkarım:" + nesnel mühendislik dili. (Birleşik §11 pass'inde.)

9. **Bold yığılması (ax_pro P3) — UYGULA (minör).** §4.1/§8.6 paragraf-başı **bold** konuları uygun yerde `####` alt-başlığa çevir.

10. **§12.1 mükerrer istatistik (ax_flash P3.6) — UYGULA (minör).** "26 oda / 549 test" gibi önceki bölümlerde geçen veriler → sadeleştir, sentez vurgula.

11. **Şekil 12 gerekçe (ax_flash P3.7) — UYGULA (kaldırma, gerekçelendir).** 6925-node grafiğin altına: hangi mimari karara etki etti / karmaşıklığı nasıl görünür kıldı — somut cümle. (graphify tezi kanıtı, silme.)

## §11 — BİRLEŞİK PASS'TE (Track A sonrası)
4. **Donanım-spesifik debug (ax_flash P1.1) — KISMİ.** §11.1 (RTX5080/D3D12 çökme) saf donanım/sürücü ise → çıkar veya tek cümleye indir. §11.9 → timeScale tek-sahip-yazma dersini KORU, RTX5080/D3D12/FPS donanım-spesifiğini buda.
7. **§11 alt-başlık sayısı (ax_flash P1.1) — KISMİ/KONSERVATİF.** "Logbook" havasını azalt ama güçlü mimari vakaları KORU (tez kanıtı). Zayıfları birleştir, güçlüleri tut. Track A'nın T7/T9 vakalarını da buraya nesnel ekle.
8. **§11.7 meta-ilerleme (ax_flash P2.5) — REFRAME (silme).** "Listener bağlanmadı = dikkatsizlik" algısı haklı → sil yerine §9 test/doğrulama bağlamında "entegrasyon testiyle yakalanan mantıksal-bağlantı hatası" olarak nesnelleştir.

## ŞÜPHELİ/DİKKAT
- §11 konsolidasyonu agresif olmasın — bu vakalar tezin "karşılaşılan zorluk → mühendislik çözümü" kanıtı; nesnelleştir, gutlamama.
- Track A'nın ürettiği Unity screenshot **Şekil 1'i (fig_gameplay_hud.png) tazeleyebilir** — birleşik pass'te kontrol et (eski/yanlış görsel mi).
- Türkçe karakter ZORUNLU; figür numaralandırma 1-12 tutarlı kalmalı.
