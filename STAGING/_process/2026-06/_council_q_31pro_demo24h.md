# RIMA jüri demosu — 24 saatte EN İYİ oynanabilir hale getirme (DEEP / ARCHITECTURE / RISK lensi)

Sen Gemini 3.1 Pro (High) advisor'ısın. Lensin: DERİN mimari + RİSK + sıralama. Aşağıdaki bağlamı oku, 6 soruya derinlemesine cevap ver. Kod yazma, ANALİZ.

## CONTEXT
RIMA = Unity 2D top-down ARPG roguelite. Hedef: jüriye gösterilecek, baştan sona OYNANABİLİR, hatasız/stuck'sız dikey-slice + mümkün olan EN İYİ görsel/animasyon cilası. 24 SAAT var.

### Demo akışı
MainMenu → Chamber (Warblade VEYA Elementalist seç; 8 sınıf kilitli) → Combat1 → Combat2 → Shop → Combat3 → Boss → Victory/Death → MainMenu. Zorunlu lineer.

### MEVCUT DURUM (kod-tamam + push'lu, AMA insan-playtest YOK)
- Softlock kök-fix (reward/draft timeout + garantili çıkış) — 76/76 test
- Forced lineer demo sırası + sabit kamera (ortho 5.0)
- 2-sınıf kilit (Warblade+Elementalist açık)
- PauseMenu (ESC) — 10/10 test
- Boss demo'da spawn + ölüm→Victory; telegraph wire'lı
- Shop: 3 stand (Heal/Damage/+MaxHP), 8/8 test
- Warblade kılıç sorting fix + Elementalist yanlış-kılıç suppress
- 594 EditMode test, 17 fail pre-existing, 0 yeni fail, +18 yeni yeşil

### GAP'LER (bilinçli yapılmadı)
- **ANIMASYONLAR (yeni en-büyük scope):** Üretim ~0; çoğu hareket kod-driven. Karakter/mob/boss gerçek sprite-anim (idle/walk/attack/cast/hurt/death) minimal/YOK. PixelLab pipeline kurulu (method B-hibrit kilitli) ama üretim user-gated. Kullanıcı ARTIK animasyonları birlikte üretmek istiyor.
- **Sanat placeholder:** Elementalist rune disc, shop stand'lar = renkli kareler.
- **Boss prefab build-gap:** prefab Resources'ta değil → standalone build'de editör-dışı spawn olmaz (editör çalışır). 1-satır fix var.
- Legacy _IsoGame decommission (post-demo).

### YENİ DİREKTİF (kullanıcı)
24 saat, en iyi oynanabilir demo. Animasyonları birlikte "tak tak" üreteceğiz. Animasyon artık scope İÇİNDE.

## SORULAR (derin/risk lensinden)
1. 24 saati nasıl bölmeli? Risk-sıralı öncelik: (a) playtest+hardening (b) animasyon üretimi (c) sanat polish (d) build-gap. Hangi sıra demoyu EN AZ riskle en iyi yapar?
2. Animasyon stratejisi: 24h'de gerçekçi olarak ne animasyonlanmalı? "2 sınıf + boss tam" mı yoksa "Warblade tam + gerisi minimal/kod-driven" mı? Hangi 5-8 animasyon en çok demo-değeri verir?
3. Boss prefab build-gap: editör-demo mu standalone-build mi gösterilmeli? Mimari risk hangisinde?
4. EN BÜYÜK "jüri önünde kırılır" riski ne? Nasıl de-risk edilir?
5. Sanat placeholder jüri için kabul edilebilir mi, yoksa animasyon+placeholder-art bir arada tutarsız mı görünür? Minimum görsel tutarlılık barı ne?
6. Saat-saat 24h programı öner (uyku/mola dahil ~12-16 aktif saat varsay; animasyon seansı = user-present blok).

Net, risk-öncelikli, somut cevap ver.
