# Antigravity Cross-Validation: RIMA Wall-less BG Layer Architecture

ACTIVE RULES: (1) think before answering (2) NET, no waffle (3) industry + practical (4) BLOCKED if can't be concrete.

Amaç: ChatGPT'ye user şu briefi gönderdi (aşağıda). ChatGPT detaylı yanıt verdi. User şimdi cross-validation istiyor (3-agent rule). Sen Antigravity (Gemini 3 Pro) olarak SAME prompt'a cevap ver. Codex paralel olarak aynı soruyu yanıtlıyor. Sonra Opus üçünü sentezleyecek.

RESPOND INLINE — do NOT write to file.

# CONTEXT (kısa)
RIMA = 2D top-down pixel art ARPG, V1 wall-less Hades Elysium style LOCKED. Karakter ~64px. Floating arena above void, cyan rift + warm brazier contrast. Unity + PixelLab asset gen. Reference: 10 ChatGPT mockup images at `STAGING/Chatgpt_walless/` (premium dark fantasy floating arena look — that's the bar).

# THE BRIEF (user's exact question — answer all 10 numbered points)

> RIMA için top-down 2D pixel-art, wall-less floating arena sisteminde arka plan/void derinliğini doğru kurmak istiyorum. Bana genel geçer değil, uygulanabilir ve net bir teknik öneri ver.
>
> Bağlam:
> - Oyun: RIMA
> - Görsel yön: dark fantasy, floating stone arena, abyss/void depth, cyan rift glow + warm brazier contrast
> - Kamera: high top-down / ARPG style
> - Karakter ölçeği: yaklaşık 64 px
> - Arena: büyük odalar / geniş boss arenaları / wall-less platform hissi
> - Ön plan: tile tabanlı taş zemin ve cliff edge/cliff face mantığı
> - Arka plan için PixelLab kullanıyorum, bu yüzden output size sınırlı; tek dev resim yerine modüler yaklaşım daha mantıklı olabilir
> - Unity'de kullanacağım
> - Arka plan animasyonlu olacak ama full frame-by-frame büyük background animasyonu istemiyorum
> - Amaç: "arkaya resim koyulmuş" gibi değil, gerçekten platform derin void üstünde asılıymış hissi
>
> Şu soruları doğrudan cevapla:
> 1. En doğru çözüm tek büyük background image mı, iki image'i birleştirip pano yapmak mı, yoksa repeating/tileable + parallax katman sistemi mi?
> 2. Hangi katmanlar repeating olmalı, hangi katmanlar wide strip olmalı, hangi katmanlar unique/special-room olmalı?
> 3. PixelLab boyut limitini düşünerek en mantıklı asset breakdown'ı çıkar:
>    - kaç layer
>    - her layer ne işe yarıyor
>    - her layer için önerilen çözünürlük
>    - şeffaf mı opak mı
>    - tekrarlı mı tekil mi
> 4. Aşağıdaki gibi net ölçü öner:
>    - Void base
>    - Far ruins strip
>    - Floating islands layer
>    - Fog layer
>    - Light beam / glow / particles
>    Ölçüleri örnek vererek yaz (örn 512x512, 1024x576, 1024x1024 vs.) ve nedenini açıkla.
> 5. Eğer iki görseli birleştirmek gerekiyorsa, bunu hangi durumda ve nasıl yapmalıyım? Rastgele büyük pano gibi birleştirmek neden kötü olur, doğru yöntem ne?
> 6. Animasyon tarafında en doğru yaklaşımı ver:
>    - hangi katman kaymalı
>    - hangisi pulse yapmalı
>    - hangisi particle olarak çözülmeli
>    - hangisini statik bırakmak mantıklı
> 7. Unity'de parallax ve sorting layer mantığını öner:
>    - örnek sorting order
>    - örnek parallax değerleri
> 8. Farklı oda tipleri için varyasyon sistemi ver:
>    - normal combat room
>    - boss room
>    - treasure/safe room
>    Aynı temel sistem nasıl varyasyonlu kullanılabilir?
> 9. Bana sonunda uygulanabilir bir production plan çıkar:
>    - önce hangi assetleri üretmeliyim
>    - sonra Unity'de nasıl kurmalıyım
>    - minimum gerekli set nedir
>    - "nice to have" set nedir
> 10. En sonda, PixelLab'da üretebilmem için her layer için kısa ama güçlü generation prompt'ları ver.
>
> Çok önemli:
> - Cevap teorik değil, uygulamaya dönük olsun
> - "bence olabilir" tarzı muğlak konuşma istemiyorum; en mantıklı pipeline'ı seç ve savun
> - Büyük tek arka plan resmi yerine modüler yaklaşım daha mantıklıysa bunu net söyle
> - Ama unique boss room background gerekiyorsa onu da ayrıca belirt
> - Hedef: performanslı, tekrar kullanılabilir, animasyon dostu, premium görünen bir arka plan pipeline'ı

# Çıktı format
1. Net karar (single big / panorama / parallax — sen seç ve savun)
2. Katman sistemi tablosu (layer adı | tekrar? | size | opaque/transparent | rol)
3. Asset size önerileri tablosu (her layer için netcraft sayı, PixelLab'a uygun)
4. Unity kurulum (sorting order tablosu, parallax math)
5. Oda tipi varyasyonları (combat/boss/treasure — aynı sistem, ne değişiyor)
6. Production checklist (ne önce, ne sonra, MVP set vs nice-to-have)
7. PixelLab gen prompts (her layer için 1-2 cümle)

# Kısıtlar
- 1000-1400 kelime (TR veya EN)
- Tüm 10 soruyu cevapla
- Inline structured output
- Spekülasyon [SPECULATIVE], industry [KAYNAK]
- Net verdict, savun
