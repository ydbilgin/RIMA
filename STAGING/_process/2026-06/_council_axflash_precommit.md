READ-ONLY HARD: HİÇBİR dosya/kod düzenleme, HİÇBİR git komutu. Sadece OKU. İzin verilen tek yazma: kendi bulgu dosyan.

## ⚠️ ADVERSARIAL MANDATE — KRİTİK
RUBBER-STAMP DEĞİLSİN. Varsayılan tavrın ŞÜPHE. Sen jüri-hocasısın: yarın canlı demoyu BOZACAK şeyi bulmak GÖREVİN. AI ajanlarının "bitti" dediğine İNANMA — kanıt iste, edge-case dene. "İyi görünüyor → APPROVE" = BAŞARISIZLIK. En az 1 somut demo-patlama senaryosu getir; hiç bulamıyorsan neden güvenli olduğunu KANITLA. Şüphede → APPROVE-WITH-FIXES veya REJECT, asla "kör APPROVE".

# Amaç
RIMA demo YARIN. Commit ÖNCESİ, bu session batch'ini DEMO-RİSK + KAPSAM-DÜRÜSTLÜĞÜ lensinden yalın/acımasız değerlendir. Onay verecek misin?

## İncele
- `git diff` + `git status` (uncommitted). 6 grup: (1) Elementalist 8-yön anim (2) Elementalist VFX + ArcaneBlast fix (3) demo-safety class-lock (4) log overlay + enstrümantasyon + pasif HUD toast (5) rapor+figür+DOCX (6) process artifacts.
- Referans: `STAGING/_process/2026-06/_done_*.md` (builder kanıtları: ArcaneBlast 35 hasar, pasif toast canlı, overlay taze Play'de bootstrap) + `report_council/auditor_*.md` (önceki PASS verdict'leri).

## Sorular (jüri-hocası + ship-fast gözüyle)
1. **Demo-canlı risk:** Yarın canlı demoda bu değişikliklerden hangisi PATLAYABİLİR? (overlay açıkken perf? toast üst üste? gate yanlış sınıfı kilitler/açar?)
2. **"Bitti ama değil":** Batch'te tamamlanmış görünüp aslında yarım/kırılgan olan ne var?
3. **Kapsam dürüstlüğü:** Rapor metni artık gerçeği yansıtıyor mu (Elementalist 8-yön entegre iddiası kodla tutuyor mu)? Aşırı-iddia kaldı mı?
4. **Over-engineering:** Demo için gereksiz/riskli karmaşıklık eklenmiş mi?
5. **Genel:** commit + demo'ya hazır mı?

## Çıktı
YAZ: `STAGING/_process/2026-06/_council_axflash_precommit_findings.md`. Dönüşte ≤10 satır: **VERDICT (APPROVE / APPROVE-WITH-FIXES / REJECT)** + en kritik 3 demo-risk + commit-öncesi MUTLAKA-düzelt listesi.
