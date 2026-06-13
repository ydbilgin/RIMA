ACTIVE RULES: (1) think before judging (2) evidence-based — dosyaları kendin oku (3) surgical — SADECE OKU, hiçbir dosya değiştirme (4) emin değilsen UNCERTAIN de.

NLM ACCESS: Gerekmez (lokal triyaj incelemesi).

# Amaç — COUNCIL REVIEW: RIMA memory/yapı temizlik planının risk denetimi
Orchestrator (Fable) RIMA'da büyük temizlik uygulayacak (kullanıcı tam yetki verdi). Senin görevin: planın **SİL ve ARŞİV kararlarındaki riskleri** bağımsız denetlemek. Son karar orchestrator'da — sen kör nokta ara.

## Oku:
1. `STAGING/LAURETHSTUDIO_ADAPTATION_REPORT_2026-06-13.md` → **SADECE Bölüm 1** (RIMA temizlik planı: A codex artifact arşivi · B user-memory SİL/ARŞİV/INDEX-EKLE triyaj tablosu · C repo↔user çakışmaları · D NLM düzeltmeleri · E STAGING temizliği)
2. Çapraz-kontrol için: `C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\MEMORY.md` (mevcut index — neyin aktif sayıldığı)
3. Şüphelendiğin SİL adayı memory dosyalarının kendisini aç-oku (yol: aynı klasör).

## Denetim soruları:
1. **SİL listesinde hâlâ-değerli bir şey var mı?** (ör. süpersede edilmiş görünen ama içinde başka yerde olmayan teknik detay/kanıt barındıran dosya — bunlar SİL değil ARŞİV olmalı)
2. **ARŞİV yerine SİL olması gerekenler?** (tamamen değersiz, yanlış bilgi içeren — tutmak drift riski)
3. **INDEX-EKLE listesi eksik mi?** Orphan ama hâlâ-geçerli kural olup listede olmayan örnekler ara (özellikle feedback_warn_then_apply_if_insistent, feedback_brief_short_lowrisk, feedback_queue_decide_order, feedback_research_on_block, feedback_fable_diagnose_agents_execute gibi davranış kuralları).
4. **Geri-dönülmezlik:** plan "asla silinmez, arşive taşınır" konservatif ilkesine (PROJECT_RULES Iteration Cleanup) uyuyor mu? SİL kategorisi bu ilkeyle çelişiyorsa söyle.
5. Repo↔user çakışma çözümlerinde (C) yanlış yön seçilmiş olan var mı?

## ÇIKTI — şu dosyaya yaz: `STAGING/_process/2026-06/_council_cleanup_verdict.md`
- Genel: ONAY / KOŞULLU ONAY / RED + 1 paragraf gerekçe
- Madde madde: "şu dosya SİL→ARŞİV olmalı çünkü..." formatında somut düzeltmeler (maks 15)
- INDEX-EKLE'ye eklenecek ek adaylar (varsa)
- Tek cümle: uygulamaya geçilebilir mi?
