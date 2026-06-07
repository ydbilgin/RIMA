ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ChatGPT'nin bitirme-raporu review paketindeki (30 bulgu) İDDİALARIN OLGUSAL DOĞRULUĞUNU kod/dosya kanıtıyla denetle — feasibility/what-exists lensi. ANALYSIS ONLY, kod değişikliği YOK.

# READ these source files
- STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/01_ACIMASIZ_REVIEW.md (30 bulgu)
- STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/02_ONCELIKLI_DUZELTME_LISTESI.md
- STAGING/report/RAPOR_DRAFT_2026-06-06.md (hedef rapor; büyük — sadece ilgili bölümleri tara)

# Sub-questions (her biri için KANIT: dosya:satır veya komut çıktısı)
S1. TEST SAYILARI (bulgu #14/#15/#27): Raporun Tablo 6.1 ve §6.2'sinde şu an hangi sayılar yazıyor? ChatGPT'nin "~529 tanım / 410 PASS / 0 FAIL / 1 inconclusive" ayrımı gerçek verilere uyuyor mu? Test envanterini say: `Assets/Tests/` altında [Test]/[UnityTest] attribute grep-count (EditMode vs PlayMode ayrı). Gerçek sayıları raporla.
S2. ENCODING (bulgu #30): RAPOR_DRAFT_2026-06-06.md içinde ASCII'ye düşmüş Türkçe kelime var mı? Grep: "Cift|Yonlu|Katmani|dogru |calisma|sarsinti|tanimlan|yurur|buyur". Varsa hangi bölümlerde (yeni eklenen §2.6/§3.5.6/§3.5.7'de mi = son edit turu kaynaklı mı)? docx üretim script'i (STAGING/report/create_rapor_docx.py) Türkçe karakterleri bozuyor mu — script'in başlık/normalize mantığına bak.
S3. SKILL GUARD (bulgu #18): "Placeholder skill'ler draft havuzuna sızmaz" iddiası kodda gerçek mi? SkillDatabase'de isImplemented alanı + DraftManager/draft havuzu filtresinde bu guard'ın kullanımı — dosya:satır kanıtı. Sızma yolu var mı (ör. codex UI placeholder gösterir ama draft göstermez — ayrımı net yaz).
S4. TERMİNOLOJİ (bulgu #2/#3): Kodda socketId'ler door_NW/N/NE; raporda "Rift portalı" dersek kod-rapor tutarsızlığı oluşur mu? Raporda şu an "kapı/duvar/kemer" kaç yerde geçiyor (kabaca)? Runtime'da oyuncuya görünen şey ne (GateBehavior/portal sprite — hangisi)? ChatGPT'nin önerdiği "EXIT_NW/EXIT_N/EXIT_NE + ENTRY_S" adlandırması koddaki gerçek adlarla (door_NW_01, spawn socket) eşleşiyor mu — raporda kod adı anılıyorsa nasıl köprülenmeli?
S5. ŞEKİLLER (bulgu #1/#15/#16/#22/#23): Raporda şu an kaç şekil gömülü, hangileri placeholder? (RAPOR_DRAFT'taki şekil notlarına + create_rapor_docx.py'nin gömdüğü dosya listesine bak; STAGING/report_screenshots/ içeriğiyle karşılaştır.) ChatGPT'nin istediği yeniden-çekimler (Şekil 1-5) + yeni şekiller (6 Warblade render, 8 pipeline diyagramı, 13 Test Runner, 14 QC before/after) için kaynak materyal MEVCUT mu (ör. STAGING/ROOM_QC_REPORT before-görselleri, screenshots_auto/)? Her şekil için: MEVCUT-KULLAN / YENİDEN-ÇEK-FEASIBLE / ÜRETIM-GEREKLİ-ZOR etiketi.
S6. SAYI TUTARLILIĞI (bulgu #4): 26 şablon vs 25 migrasyon — gerçek neden ne? (Assets/Data/Rooms altında template say; Chamber_CharSelect migrasyona dahil mi — git log f63ac34c veya validator/migrasyon koduna bak.) ChatGPT'nin önerdiği açıklama cümlesi OLGUSAL olarak doğru mu?

# Çıktı
CODEX_DONE.md'ye: her S için VERIFIED-TRUE / VERIFIED-FALSE / PARTIAL + kanıt satırı. Sonda: ChatGPT bulgularından olgusal hata içerenlerin listesi (bulgu# + neden). Önceki audit'leri kopyalama, taze kanıt topla.
