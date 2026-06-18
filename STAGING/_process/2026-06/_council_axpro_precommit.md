READ-ONLY HARD: HİÇBİR dosya/kod düzenleme, HİÇBİR git komutu (add/commit/checkout/restore). Sadece OKU + incele. İzin verilen tek yazma: kendi bulgu dosyan.

## ⚠️ ADVERSARIAL MANDATE — KRİTİK
RUBBER-STAMP DEĞİLSİN. Varsayılan tavrın ŞÜPHE. Bu batch'i AI ajanları üretti — TASARIM/MİMARİ HATASI + KÖTÜ FİGÜR YAPTIKLARINI VARSAY, aktif olarak ZAYIF NOKTA ara. "İyi görünüyor → APPROVE" = BAŞARISIZLIK. APPROVE vermeden önce gerçekten kusur aradığını KANITLA. En az 1 somut şüphe/risk noktası getir; hiç bulamıyorsan sağlamlığı KANITLA (his değil). Şüphede → APPROVE-WITH-FIXES veya REJECT, asla "kör APPROVE". Figürleri GERÇEKTEN AÇ (vision) — caption'ı yalanlayan/jüri sorusu doğuran her şeyi yakala.

# Amaç
RIMA demo YARIN. Commit ÖNCESİ, bu session'ın TÜM uncommitted batch'ini MİMARİ/TASARIM BÜTÜNLÜĞÜ + FİGÜR KALİTESİ lensinden bağımsız denetle. Onay verecek misin?

## İncele
- `git diff` + `git status` (uncommitted). 6 grup (özet):
  (1) Elementalist 8-yön idle .anim re-point · (2) 13 Elementalist skill SkillVfx + ArcaneBlast fix · (3) demo-safety class-lock (ClassUnlockPolicy.IsDemoPlayable tek-gerçek + ilgili UI/PlayerClassManager) · (4) DebugLogOverlay(F3)+enstrümantasyon+DraftManager pasif HUD toast · (5) rapor+figür+DOCX · (6) process artifacts
- **Figürleri AÇ VE İNCELE** (re-capture edildi): `STAGING/report/figures_2026-06-18/fig_rooms_island_grid.png` (Şekil6 — void-leak düzeltildi mi, mor dikdörtgen kalmış mı?) + `STAGING/report/figures_2026-06-18/fig_weapon_mount.png` (Şekil9 — silah elde mi, caption'la uyumlu mu?)
- Referans: `STAGING/_process/2026-06/report_council/auditor_*.md` + `_done_*.md`

## Sorular
1. **Mimari bütünlük:** demo-safety katmanı (IsDemoPlayable tek-kaynak + Director bypass) tutarlı/temiz bir tasarım mı, yoksa dağınık/kırılgan mı? DebugLogOverlay + enstrümantasyon tasarımı sağlam mı?
2. **Tasarım smell:** Bu batch'te post-demo borç yaratan ya da mimariyi zehirleyen bir karar var mı?
3. **Figür kalitesi:** Şekil 6 ve Şekil 9 artık caption'larıyla tutarlı + jüriye sunulabilir mi? Kalan görsel kusur?
4. **Genel:** Bu batch commit'e + demoya hazır mı?

## Çıktı
YAZ: `STAGING/_process/2026-06/_council_axpro_precommit_findings.md`. Dönüşte ≤10 satır: **VERDICT (APPROVE / APPROVE-WITH-FIXES / REJECT)** + gerekçe + (varsa) commit-öncesi MUTLAKA-düzelt.
