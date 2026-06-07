# KARAR — ChatGPT UX Akış Feedback'i Değerlendirmesi (2026-06-07 akşam)

**Girdi:** `STAGING/_incoming/ux_flow_feedback_2026-06-07/` (6 doc: kare-kare review, oda boyut/kompozisyon, asset listesi, dil standardı, sunum akışı).
**Değerlendirme:** Opus (tam council gereksiz — maddeler mevcut MASTER_PLAN görevlerine map'leniyor; olgusal iddialar kendi karelerimizle doğrulanabilir durumda).

## Düzeltilen ChatGPT iddiaları
1. **"Portal rune seti üretilmeli" → KISMEN YANLIŞ:** 4 tip rünü ZATEN ÜRETİLMİŞ ve oyunda (12_portals'ta görünen kılıç ikonları). Gerçek sorun YERLEŞİM: rünler kemerin üstünde havada/uzakta yüzüyor, bağlamsız. İş = yeniden üretim değil, kemerin alın hizasına ATTACH + boyut/label.
2. **"Portallar hepsi aynı, ayrıştır" ↔ kullanıcı "hepsi aynı olsun":** ÇELİŞKİ DEĞİL — çözüm zaten U1'de: TEK gövde (kullanıcı) + tip rünü/label/tint (ChatGPT). İkisi de karşılanıyor.
3. **Telegraph decal'leri (03/#15):** zaten üretildi+import edildi (Art/Telegraphs) — T6'da wire'lanacak.
4. **Boss ritual circle + seal fragments:** zaten üretildi (Art/Boss) — T6'da wire'lanacak.
5. **"[E] Bürün":** etkileşim tuşu G'ye taşınmıştı (482864af) — kare eski overlay sahnelemesinden [E] gösteriyor olabilir; canlıda [G]. Dil pass'inde tek formata bağlanacak.

## Kabul edilen ana bulgular → görev eşlemesi
| Bulgu | Görev |
|---|---|
| Boss odası YEŞİL DEBUG KARE + düz sarı HP bar + Victory dev sarı panel = "sunum kırıkları" | **T6 kapsamı GENİŞLEDİ:** global SeamCrawler/yeşil-kutu temizliği (run odaları dahil) + boss HP 9-slice frame + Victory paneli RIMA koyu 9-slice + boss intro 1.5s (zaten spec'te) + telegraph/ritual-circle wiring |
| TR/EN karışımı + durum banner'ları yok ("şimdi ne yapacağım?") | **YENİ T-LANG görevi:** 04 dosyasındaki terim tablosu birebir uygulanır + state banner sistemi (Echo Seç / Rift'e Gir / Düşmanları Temizle / Ödülünü Seç / Bir Portal Seç / Boss Yaklaşıyor) |
| Portal rune/label attach + plaque | **U1-devam (U2):** rün kemere monte + `COMBAT/ELITE/ÖDÜL/BOSS` plaque (label frame imagegen'le üretilir) |
| Chamber: pedestal %25-35 küçük + 5+5 çift yay + merkez altar + class adı/glow + weapon silüet ikonu | **T5 kapsamı netleşti** (U1'in spacing fix'i üstüne) — weapon silüet ikonları (10×32px) PixelLab seansına eklendi |
| Draft: başlık küçült + tag chip + sinerji + SEÇ hizası | **T4** (zaten kapsamdaydı, teyit) |
| Hole rim decal + edge filler + ground decal + arrival ring + parapet | **T8 + imagegen batch-3 GENİŞLEDİ** (03 dosyasının Batch A-E listesi temel alınır; transparan decal'ler magenta-chroma pipeline) |
| Kamera %10-15 yakın + oyuncu kompozisyonu | **Playtest-gated tuning** — kullanıcı feel-test'iyle birlikte |
| Oda boyut reçeteleri (combat 18×12-22×14 vb.) | **BEKLETİLDİ** — 26-oda ChatGPT review'ının sonucuyla birlikte tek oda-rework turu (çifte rework israfı önlenir) |
| Sunum akışı 7-8dk + "Codex/CharSheet/Settings gösterme" | **Sunum Çekim Rehberi'ne girdi** (kuyruğun sonunda) |

## RED
- "Pedestal yeniden üretimi (smaller pedestal set)" → önce T5'te SCALE ile dene (mevcut sprite %70'e küçült); yetmezse PixelLab seansına eklenir.
- Oda boyutlarını HEMEN değiştirmek → oda-review birleşik turu bekler.

## Güncel otonom kuyruk
U1 (çalışıyor) → **T6-genişletilmiş** → **T-LANG** → T4 → T5 → T8+batch-3 → Sunum Rehberi → Tool Audit. PixelLab seansı: moblar + 3 silah + 10 weapon-silüet-ikonu (+gerekirse pedestal).
