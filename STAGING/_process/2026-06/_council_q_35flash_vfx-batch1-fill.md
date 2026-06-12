# 64px VFX Batch 1 — 13 boş slot: LEAN / over-production avcısı (Gemini 3.5 Flash High)

Sen laurethstudio'nun "kullanılmayacağı üretme" bekçisisin. RIMA = 2D pixel-art roguelite, DEMO yetiştiriliyor. Görev: 13 boş slotu doldurma hevesine sağlıklı şüphe.

Gerekirse: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
READ: STAGING/PIXELLAB_VFX_BATCH_LIMITS_2026-06-12.md · STAGING/SKILL_VFX_PRODUCTION_SPEC_2026-06-12.md · STAGING/MODULAR_ABILITY_DECISION_2026-06-12.md

## Durum
64px batch'te 16 slot, 3 dolu (slash arc, ice spike, Elementalist bolt), 13 boş. Maliyet çağrı-başına sabit → "slot bedava" diye doldurma cazibesi var. Kullanıcı diğer skill'ler / diğer sınıflar / global reusable düşünüyor.

## Sorular (lean, somut)
1. **GERÇEK demo ihtiyacı:** 10 dk jüri demosunda Warblade+Elementalist 2 sınıf oynanıyor. Slash arc + ice spike + bolt DIŞINDA gerçekten EKRANDA görünecek kaç 64px VFX var? Geri kalan 13 slot demo'da görünür mü, yoksa "gelecekte lazım olur" spekülasyonu mu?
2. **Bedava-slot yanılgısı:** "çağrı maliyeti sabit, doldur gitsin" mantığının tuzağı ne? (Üretilen ama kullanılmayan asset = import/atlas/QC yükü, style-drift, kütüphane çöplüğü.) Modüler ders (bu session 3 hedef ampirik çöktü) burada nasıl geçerli?
3. **Minimum öneri:** Eğer 13'ü doldurmayacaksak, gerçekten değecek EN FAZLA kaç ekstra item? Hangileri (sadece demo'da görünecek + yüksek-tekrar olanlar)? Geri kalanı boş bırak / sonra üret mi?
4. **Global reusable tuzağı:** "her oyunda kullanılır" generic VFX üretmek şimdi mantıklı mı, yoksa o particle/code işi mi (PixelLab sprite israfı)? Şimdi mi sonra mı?

Kısa, net. "Boş bırak / sonra üret" demekten çekinme. Türkçe serbest.
