# BİTİRME RAPORU NOTLARI — birikimli (her otonom güncellemede beslenir)

> Amaç: rapor yazımında detaylı düşünülecek ham madde listesi. Her madde = rapora girebilecek bir iş/karar/kanıt.

## Sistem anlatımı (rapor §sistemler)
- **Dual-class akışı TAMAMLANDI (2026-06-10):** Boss artık terminal node değil; boss ölümü → ikincil sınıf seçim overlay'i (timeScale=0) → unlock draft → "İKİNCİL SINIF AÇILDI" banner → post-boss arenada birleşik kit OYNANIYOR → victory. Rapora: "ayırt edici mekanik yalnızca veri/UI olarak değil, oynanış döngüsünün içinde kanıtlanır" anlatısı + akış diyagramı (6-node lineer graph).
- **Run-graph mimarisi:** `DungeonGraph.BuildDemoSequence()` deterministik 6-node lineer graph; üretim tarafında StS-lite branching `Generate(seed, depth)` da mevcut — raporda "demo=deterministik, sistem=prosedürel" ayrımı anlatılabilir.
- **Guard-tabanlı sağlamlık:** reward 12s + draft 90s timeout, `ForceOpenExitDoorsFromAnyClearedState` — "softlock imkansızlaştırma" tasarım kararı; test-driven (594 EditMode testi, 18 bilinen pre-existing fail, regresyon 0).
- **Primary-filtre edge-case:** Elementalist primary iken Elementalist'i ikincil seçebilme bug'ı iki katmanda kapatıldı (UI kart filtresi + manager guard) — raporda "savunmalı tasarım" örneği.

## Game-feel / juice (rapor §oyun hissi)
- **Boss screen-shake restorasyonu:** [Obsolete] API'ye giden 6 ölü çağrı statik analizle bulundu (sub-agent audit), canlı `ScreenShakeDriver`'a tier eşlemesiyle (hafif 0.25/0.30 → ağır 0.55/0.45) taşındı + runtime bootstrap. Raporda "ölü-kod tespiti + yaşayan sistem entegrasyonu" örneği.
- NLM kanon: hit-confirm üçlüsü (slash-arc + hitspark + 0.08s hit-flash + directional shake), hit-stop 0.05-0.15s, damage numbers world-space TMPro — hepsi kodda mevcut; raporda juice mimarisi şeması.

## UX / onboarding (rapor §kullanılabilirlik)
- İlk odada solan kontrol ipucu satırı (tek sefer, 8s, panel'siz) — kanon "UI yoktur, bilgi vardır" ilkesine uyum; jüri/soğuk-oyuncu onboarding'i.
- Wishlist butonu demo build'de gizlendi (placeholder URL).

## AI-destekli üretim süreci (rapor §yöntem — hoca özetindeki "AI destekli araçlar" kısmının kanıtı)
- Çok-ajanlı orkestrasyon: Opus orchestrator + Sonnet sub-agent'lar (kod taslağı) + insan-onaylı diff uygulama + Unity MCP ile compile/test doğrulama döngüsü.
- Üç bağımsız kaynak çapraz-doğrulaması (ChatGPT repo-review + statik audit + NLM kanon) → tek plan sentezi. Raporda "AI çıktısı doğrulanmadan uygulanmaz" metodolojisi.
- PixelLab pipeline: karakter 120px 8-yön (5 üret + 3 mirror), ikon 32px obje-batch (64'lük tek çağrı), fireball 8-yön obje. Test: "594 test, hedefli suite 20/20".

## Lisans / atıf listesi (rapor ekleri)
- SFX: 18 klip Kenney (CC0).
- Müzik: music_demo = OpenGameArt "Loopable Dungeon Ambience", JaggedStone, CC0. (eklendiğinde doğrula)
- PixelLab üretimleri: kendi aboneliğimiz, ticari kullanım OK.

## Ekran görüntüsü ihtiyaçları (rapor şekilleri)
- Sahne-sahne "şu an oynanabilir durum" screenshot seti (otonom tur planlandı — alınınca buraya yol yazılacak).
- Dual-class akışı 4-kare şerit: boss ölümü → seçim → banner → post-boss combat.
