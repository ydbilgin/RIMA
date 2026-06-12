# RIMA — Oda Güzelleştirme + Animated Background — ChatGPT'ye Soru Dosyası (2026-06-11)

> (Sen `CapturedMaps_RIMA.zip` içindeki oda render'larını + şematiklerini gördün. İşte verdiğimiz kararlar + 2 sorum. Lütfen aşağıdaki KİLİTLERE uy — ihlal eden öneri verme.)

## RIMA bağlamı + KİLİTLER (değiştirilemez)
- room-based 2D ARPG roguelite. Demo-finalizasyon, deadline var.
- Sanat: **HIGH TOP-DOWN 3/4** (Hades / Children of Morta ref). **ISO YOK, 45° diamond YOK.**
- Asset: **PixelLab-only** (asset-store tileset paketi YOK). 32px tile, PPU 64, URP 2D Renderer + Pixel Perfect Camera + 2D Lights.
- Oyun açık-dünya OLMAYACAK; oda-bazlı kalacak.

## MEVCUT DURUM (render'larda görünen sorun)
Büyük gri DÜZ zemin (tek-değer, monoton) + props tek köşede + IŞIK YOK (her yer aynı gri-değer) + arka plan çok soluk. İyi taban: yüzen-ada silüeti + alttaki uçurum + cyan-rift kenarları.

## REFERANS (kullanıcının gösterdiği)
Başka birinin AÇIK-DÜNYA mobil projesi: zengin izometrik krallık haritası (asset-store tileset + autotile + çok-katmanlı tilemap + animated su/meşale). Çok güzel görünüyor — AMA o TRUE-ISO + asset-store + open-world. RIMA = top-down-3/4 + PixelLab-only + room-based → o look'u birebir KOPYALAYAMAYIZ; sadece kilitlere uyan teknikleri ödünç alabiliriz.

## VERDİĞİMİZ KARARLAR (3-tur council: Codex + Gemini 3.1 Pro + Gemini 3.5 Flash → sentez)
1. **Sorun boyut DEĞİL** (odalar zaten 24×18–36×28); sorun = düz + boş + ışıksız → **FILL + IŞIK**.
2. **ISO'ya geçme, Wang/autotile painter'a girme** (deadline tuzağı; post-demo).
3. **Sıra: IŞIK ÖNCE** (en yüksek görsel kazanç — ışıksız boş alanda dekor ayarlamak körleştirir) → **DEKORASYON** → QC.
4. **Işık:** per-oda global ambient (soğuk-mavi, kısık) + 2-3 point light (key = focal üstü, accent = brazier/kapı) + **renk-kimliği** (rift/ember sıcak-turuncu vs soğuk oda). [Kod scaffold HAZIR, default-preserving.]
5. **Dekorasyon = "çerçeveli diorama" (random saçma DEĞİL):** kenar-yoğun, merkez-temiz (dövüş alanı), **oda başına 1 focal/landmark**, prop/duvar diplerine **grounding shadow** (sahte yarı-saydam gölge — yoksa proplar havada süzülür). density ~0.3, deterministik. [auto-placer + validator sistemi HAZIR.]
6. **Proplar: 4 tier, hepsi ŞEFFAF, PixelLab** (decal 32px/64-batch · küçük prop 64px/16 · focal 128px/4 · landmark >170/1). İlk 40 şeffaf decal üretildi.

---

## ❓ SORU 1 — Oda güzelleştirme yöntemi
Yukarıdaki kararlar + gördüğün render'lar ışığında: **RIMA odalarını güzelleştirmek için SENİN yöntemin ne?**
- Referans projeyi güzel yapan şey SADECE ışık mı, yoksa (cohesive tileset / autotile / renk-çeşitliliği / yoğunluk / yükseklik / animated detay) mı? RIMA'nın kilitleri içinde bunların hangilerini ALABİLİRİZ, hangileri post-demo?
- Atladığımız bir görsel kaldıraç var mı? (gölge/kontrast/kompozisyon/renk-paleti/depth?)
- Sıralama (ışık→dekor) doğru mu? "Çerçeveli diorama + 1 focal + per-oda ışık" yeterli mi?
- KİLİTLERE uy (top-down 3/4, PixelLab-only, room-based, no iso/wang).

## ❓ SORU 2 — Animated background
Arka planı (yüzen-ada void'i) **canlandırmak** istiyoruz.
- En iyi yöntem ne? (sürüklenen sis / parıldayan void-yıldızları / nabız atan cyan-rift / yüzen moloz-ember?)
- **PixelLab animated sprite (create_animated_pro, 16-frame) + shader/parallax drift** kombinasyonu mu, yoksa başka bir yaklaşım mı?
- Tam-ekran animasyonlu backdrop ağır mı; yoksa statik parallax katmanı + birkaç animasyonlu aksan-eleman mı daha akıllı?
- Top-down 3/4 + **Pixel Perfect Camera**'da animated bg **titremeden** nasıl olur?
- PixelLab dışında (imagegen sadece statik 1024² verir) animated bg için pratik bir yol var mı?

## ❓ SORU 3 — Sunum / sahne-geçiş tool'u
RIMA tek-scene değil; çok odalı (Combat→Merchant→Boss vb. + ayrı oda template'leri). Hocaya sunumda **basit sahne/oda GEÇİŞİ** yapabilmem lazım.
- En yalın yöntem ne? Oyun-içi **fade geçiş** (oda→oda siyaha fade) mı, sunum için **room-jumper navigatör** (istediğin odaya tek-tık atla) mı, ikisi mi?
- RIMA'da zaten `RIMA/Room Browser` editör penceresi var (oda tıkla → _Arena'da kur). Sunum için bunu mu cilalayalım, yoksa runtime bir geçiş-sistemi mi?
- Deadline + demo için over-engineering olmadan en pratik tool?

> Not: Cevabını council'den geçireceğiz (stale/over-engineering elemesi); KİLİT ihlali öneriyorsan açıkça işaretle.
