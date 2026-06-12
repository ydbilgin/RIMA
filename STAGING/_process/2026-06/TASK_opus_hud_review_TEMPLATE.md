# TASK: Opus HUD Design Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Görev
ChatGPT'nin önerdiği HUD tasarımını RIMA projesine uygunluk açısından değerlendir.

## Proje Kısıtları (bunlara göre değerlendir)
- Pixel art top-down 3/4 roguelite
- Renk: void mor `#3A1A4A` panel, ember `#E89020` vurgu, slate `#3A3D42` zemin
- Skill bar: LMB/RMB sol başta (daha büyük, ember kenarlı), 6 skill slot yanında
- Referans: Hades / Dead Cells HUD sadeliği
- URP 2D Renderer — ScreenSpaceOverlay Canvas kullanıyoruz

## ChatGPT Çıktısı
[BURAYA CHATGPT ÇIKTISI GELECEK]

## İstenen Değerlendirme
1. Renk/stil uyumu — RIMA paletine uyuyor mu?
2. Skill bar layout — mevcut implementasyonla çakışma var mı?
3. Uygulanabilirlik — Unity Canvas ile yapılabilir mi, ne kadar iş?
4. En iyi 2-3 öneri — hangilerini önceliklendirelim?
5. Red flag var mı — RIMA'nın kısıtlarıyla çelişen bir şey?

## Çıktı Formatı
PASS / PARTIAL / REJECT + madde madde notlar. Kısa, karar odaklı.
