# ChatGPT'ye Atılacak Prompt — 26 Oda Şablonu Görsel Review'ı (2026-06-07)

> Kullanım: Aşağıdaki prompt'u kopyala + `RIMA_ODA_SCREENSHOTLARI_2026-06-07.zip`'i ekle.
> (Zip: 26 odanın deterministik-seed'li _Arena kareleri + CONTACT_SHEET.html + oda metadata tablosu)

---

## PROMPT (buradan aşağısını kopyala)

Ekteki zip'te RIMA'nın (2D top-down chibi pixel-art roguelite; yüzen ada odaları + arka kenarda Rift portalları) **26 oda şablonunun tamamının** oyun motoru içinde kurulmuş halinin ekran görüntüleri var. Her kare deterministik prop seed'iyle alındı (tekrarlanabilir). `00_ROOM_METADATA.md` dosyasında her odanın adı, tipi (Combat/Elite/Boss/Chest/Corridor/Shrine/Treasure/Spawn), boyutu, walkable hücre sayısı, prop sayısı ve çıkış soketi bilgisi var — kareleri bu tabloyla birlikte oku.

**Tasarım bağlamı (değerlendirme kriterlerin):**
1. **Yüzen ada doktrini:** Her oda boşlukta asılı bir ada gibi okunmalı — cliff kenarları doğal, "kesilmiş dikdörtgen" hissi anti-pattern
2. **Savaş okunabilirliği:** Combat odalarında merkez bölge ferah olmalı (oyuncu+3-5 düşman rahat dövüşebilmeli); prop'lar kenara/köşeye kümelenmeli, choke-point yaratabilir ama merkezi tıkamamalı
3. **Çıkış soketleri:** Arka kenarda (kuzey) NW/N/NE portal noktaları mantıklı konumda mı, kapı önü yürünebilir mi
4. **Silüet çeşitliliği:** 26 oda birbirinden ayırt edilebilir mi — benzer silüetli "kopya" odalar israf
5. **Tip uygunluğu:** Boss odası epik/geniş mi, Elite tehditkâr mı, Chest/Treasure ödül hissi veriyor mu, Corridor geçiş hissi veriyor mu

**Senden istenen çıktı:**
1. **Oda başına tek satır verdict tablosu:** `oda_adı · verdict (KEEP / REWORK / CUT) · tek cümle gerekçe`
   - REWORK ise NE yapılacağı somut olsun ("merkez 3×3 temizlensin", "güney cliff'i içe alınsın", "prop yoğunluğu yarıya" gibi)
   - CUT önerisi = silüeti başka bir odayla çakışan veya kompozisyonu kurtarılamaz olanlar
2. **Production RoomBank önerisi:** Demo run'ları için en iyi 12-15 odalık seti seç (tip dağılımı dengeli: ~7-8 Combat, 2 Elite, 1-2 Chest/Treasure, 1-2 Corridor, 1 Boss) — hangi odalar girsin, neden
3. **Sistemik desenler:** Birden çok odada tekrarlayan sorun görürsen (örn. "tüm donut'larda iç delik kenarı çıplak", "büyük odalar hep boş hissettiriyor") ayrı başlıkta listele — bunlar tek tek fix yerine araç/kural fix'i olur
4. **En iyi 3 oda:** Rapor/tanıtım görseli olmaya en uygun 3 kare

**Kurallar:** Oda İÇERİĞİ hakkında karar ver (kompozisyon/okunabilirlik/çeşitlilik); sanat stilini (tile sprite'ları, renk paleti) değerlendirme — onlar ayrı pipeline. Duvar/fiziksel kapı ÖNERME (duvarsız ada + portal doktrini kilitli). Yeni oda tipi (Heal/Shop/Event) ÖNERME — demo kapsamı Combat/Elite/Chest/Boss+mevcut yardımcılar. Türkçe yanıtla.
