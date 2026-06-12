# ChatGPT'ye — RIMA Sandbox/Director Tool TASARIMI (paralel düşün)

> Bağlamı biliyorsun (class stats handoff). Şimdi oyun-içi **Sandbox/Director debug+balance tool**'unu tasarla. Bizim council (Opus+Gemini) bir taslak çıkardı (`03_COUNCIL_SYNTHESIS.md`). Sen PARALEL düşün: onaylа/geliştir + kendi katkını ekle.

## İstediğimiz çıktı
### A) Tasarım review + geliştirme
Council'ın 6-sekmeli sol-rail chrome-skinli uGUI Director Mode tasarımını incele. Eksik/yanlış/geliştirilebilir ne var? Sekme sırası, mod akışı, UX detayları.

### B) GÖRSEL MOCKUP (önemli)
Tool'un **bitmiş HUD görünümünü çiz** — ekteki chrome skin'le (`chrome_kit/`), pixel-art, RIMA paleti (void mor #3A1A4A, ember #E89020, slate #3A3D42). "Python wireframe gibi DEĞİL, güzel HUD" istiyoruz. En az: Director Mode ana ekranı (sol rail + Spawn sekmesi açık + serbest-cam görünümü). Mümkünse 2-3 sekme mockup'ı.

### C) uGUI prefab hiyerarşisi
Unity'de nasıl kurulacağı: Canvas → DirectorPanel → TabRail → ContentArea → her sekme paneli. Hangi chrome sprite nerede, hangi layout group, CanvasGroup toggle. Net hiyerarşi ağacı.

### D) Ek fikir
Council'ın kaçırdığı bir UX/tool var mı? (snapshot quick-reset, dummy AI davranış modu, hitbox overlay, vb.)

## Kısıtlar
- Oyun-içi runtime (build'de çalışır), Unity URP 2D, uGUI
- Chrome skin zaten ürettik (ekte) — onu kullan, yeni chrome üretme
- Sekmeler mevcut runtime sistemlere hook'lanıyor (`02_EXISTING_SYSTEMS.md`)
- timeScale=0'da unscaledDeltaTime, sol-rail layout

## Ekler
- `01_TOOL_VISION.md` — kullanıcı vizyonu + mod akışı
- `02_EXISTING_SYSTEMS.md` — hook'lanacak runtime API'ler
- `03_COUNCIL_SYNTHESIS.md` — Opus tasarım + Gemini UX
- `04_STAT_DECISION.md` — kilitli stat modeli (Stats sekmesi için)
- `chrome_kit/` — gerçek chrome skin (preview + slice'lar)

Türkçe, net. B (görsel mockup) en kritik — onu mutlaka üret.
