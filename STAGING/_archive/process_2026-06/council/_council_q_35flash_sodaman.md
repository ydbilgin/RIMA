# RIMA Council — Sodaman analizi (LEAN / SHIP-FAST + OVER-ENGINEERING KRİTİĞİ lens — Gemini 3.5 Flash High)

Sen RIMA için "en yalın yol + aşırı-mühendislik kritiği" danışmanısın. Çıktın RIMA orkestratörüne (Opus) gider; o karar verir. Görevin: en AZ işle en çok his/değer; gereksiz olanı KES.

## SODAMAN — FACT SHEET (ground-truth)
- cyberpunk **bullet-heaven roguelite**, top-down pixel 2D, ~%78 olumlu/630, ~4-8 saat.
- **İMZA DRAFT:** level-up'ta power-up'ı **3 SODA KUTUSUNDAN** seç (tematik kap, "3 kart" yerine). **7 renk × 10+ skill**, renk-karıştır = sinerji "kokteyl".
- Meta: 40+ enhancement kartı (deck), 6 vücut-parçası × 30+ augment, 8 silah-playstyle.
- Run-arası uzay-gemisi hub. 91 achievement.

## RIMA — BAĞLAM (yalın)
2D iso roguelite ARPG, 10 sınıf, Pixel Perfect 640×360 PPU64. Canon: opak kutu YASAK, ink-on-paper, renk=anlam, diegetic. Currency "Echo" (isim emin değil).
**Zaten var:** DraftManager + SkillOfferUI (3-kart hover-scale + select-flash + rarity_glow + 9-slice frame), SkillBar (Q/E/R/F/Z/X), RimaUITheme (tema+icon+accent), CharacterSelect roster-room.

## KULLANICI İSTEĞİ
"Skill seçerken hover gibi şeyler nasıl eklenir" + Sodaman'dan "ne alınır." + 2 yeni özellik:
1. **Run-içi sol toggle skill paneli** (tuşla aç/kapa, equipped skiller).
2. **ESC codex skill ekranı** (tüm sınıf skilleri, theorycraft).

## SENİN ANALİZİN — LEAN açıdan
1. **Sodaman'dan ALINMAYACAKLAR (kritik):** 7-renk/40-kart/30-augment/8-silah/hub gibi şeylerin HANGİSİ RIMA için scope-creep/over-engineering? RIMA'nın halihazırdaki draft+10-sınıf sistemine HANGİ TEK fikir en yüksek değer/iş oranı verir? (1-2 şey seç, gerisini "DEFER/SKIP" işaretle + neden.)
2. **HOVER — en yalın yüksek-his:** Mevcut SkillOfferUI hover-scale'in üstüne, **minimum** kodla en çok his katacak 3 şey ne (örn: hover'da tooltip-metin reveal, ufak audio tick, equipped-synergy highlight)? Hangileri "güzel ama gereksiz" → KES. Sıfırdan tooltip-sistemi yazmaya GEREK var mı yoksa mevcut text + bir CanvasGroup fade yeter mi?
3. **Sol skill paneli — MVP:** En yalın çalışan versiyon ne? Yeni karmaşık sistem mi yoksa "mevcut SkillBar verisini bir slide-out CanvasGroup'ta listele" mi? Hangi tuş (kullanıcıya bir öneri). Aşırı-tasarımdan (animasyonlu sinerji-grafiği vs.) kaçın — glance-able liste yeter mi?
4. **ESC codex ekranı — MVP:** Tam-ekran mı / yan-panel mi (en az iş hangisi)? Skill verisi zaten bir yerde enumere ediliyorsa onu bir scroll-grid'de göster + hover-detay; YENİ veri modeli yazmaya GEREK YOK. SettingsMenuUI pause-pattern reuse edilebilir mi? Sinerji-tree gibi şeyleri DEFER et.
5. **Currency adı:** "Echo" yeterince iyi mi, yoksa değiştirmek gereksiz iş mi? (Pragmatik: değiştirmenin maliyeti vs. faydası.)

ÇIKTI: Kısa, net, "YAP / KES / DEFER" tablosu + her özellik için MVP-tanımı (1-2 cümle). En çok over-engineering riskini nerede görüyorsun açıkça söyle.
