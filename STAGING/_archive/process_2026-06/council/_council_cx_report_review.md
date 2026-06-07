ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Bitirme raporu taslağının OLGUSAL DOĞRULUK review'ı. ANALYSIS ONLY — rapora veya koda yazma.

# Görev
`STAGING/report/RAPOR_DRAFT_2026-06-06.md` dosyasını OKU ve içindeki HER SOMUT İDDİAYI kod tabanına karşı doğrula:
- Sayılar: 26 oda, 10 sınıf, 111 skill/67 implement, test sayıları (~490 EditMode/410 PASS), 80 karakter PNG, canvas 120-128, PPU 64, tier ağırlıkları, prop sayıları, state machine durum adları/sayısı.
- Sistem iddiaları: "IsoRoomBuilder şunu yapar", "auto-placer şöyle çalışır", "draft 3 kart sunar", "[G] etkileşimi", "TAB fallback", "Shattered Echo formülü/ödülü" — kod gerçeğiyle uyuşuyor mu?
- Süreç iddiaları: 10-task kuyruk anlatısı, QC vakası sayıları (15 OK/9 şüpheli/2 FAIL), singleton fix hikâyesi — CURRENT_STATUS/STAGING kayıtlarıyla tutarlı mı?
- Abartı/uydurma tespiti: kodda KARŞILIĞI OLMAYAN her iddiayı işaretle (LLM-yazımı raporlarda en büyük risk).

# Çıktı (CODEX_DONE'a)
Tablo: iddia (bölüm/satır yaklaşık) | doğru mu (TRUE/FALSE/ABARTILI/DOĞRULANAMADI) | kanıt veya düzeltme önerisi. Sonda: düzeltilmesi ŞART olan ilk 10 madde listesi. Rapor metnini yeniden YAZMA — sadece bulgu listesi.
