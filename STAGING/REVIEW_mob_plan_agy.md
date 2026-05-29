ACTIVE RULES: (1) think before answering (2) example-backed (3) actionable (4) UNSURE if unknown.

NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>" (NLM auth düşmüş olabilir — düşerse local dosyalarla devam, BLOCK olma).

RESPOND INLINE (dosyaya yazma).

# GÖREV — Mob üretim planı VERİMLİLİK review (design/market)
Opus `STAGING/MOB_PRODUCTION_PLAN_S6.md` yazdı (proje root). ELEŞTİREL review — sen YAZMIYORSUN, fikir+review.

Değerlendir (örnekli):
1. **Demo enemy yön sayısı:** top-down pixel ARPG demo'sunda düşmanlar 8-dir mi GEREK, yoksa 4-dir (veya 2-dir + flip) YETER mi? Örnek oyunlar (Children of Morta, Enter the Gungeon, Hades). Player 8-dir, mob 4-dir mantıklı mı?
2. **Anim state minimumu:** bir demo düşmanı için kaç anim state "yeterli" (idle/walk/attack/death)? Hangileri kesilebilir (hit-react = flash/shake yeter mi)? death tek-yön olabilir mi?
3. **Recolor/aura elite varyant** best-practice: palette-swap + shader aura ile "yeni düşman hissi" ne kadar ikna edici? Örnekler.
4. **Boss üretim önceliği:** boss'u graybox ile demo-loop tamamlamak (mekanik önce) vs tam sprite önce — hangisi demo→wishlist için daha akıllı?
5. Gen-bütçe (4-dir ~60-70 gen) demo için makul mu, kesilebilecek yer var mı?

# ÇIKTI (inline)
(A) KISA SENTEZ: her karar-noktası için öneri + en kritik 2-3 düzeltme.
(B) HAM BULGULAR/KAYNAKLAR: web örnekleri, sayılar, oyun-spesifik veriler (sıkıştırma — [[feedback_agy_research_synthesis_plus_raw]]).
