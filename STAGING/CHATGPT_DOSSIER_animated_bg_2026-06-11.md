# RIMA — Act 1 "Shattered Keep" Animated Background — ÜRETİM BRIEF (ChatGPT'ye)

> Senin önerin (1 statik ana bg + 4-8 frame loop + parallax katmanları + fog/rift/particle overlay) bizim council kararımızla BİREBİR örtüşüyor — onu uygulayalım. İşte RIMA kanonuna göre net brief.

## TEMA
Yüzen-ada arena, derin bir VOID içinde asılı. Uzakta parçalanmış kale/keep silüetleri yüzüyor. Cyan rift enerjisi void'i kesiyor. (Odalar = yüzen diorama platformları; arka plan = altlarındaki/etraflarındaki void.)

## RENK
Koyu desatüre taban (deep blue-black / charcoal void) + **cyan rift aksanı #2BD9D9** + **warm ember aksanı #E89020**. DÜŞÜK kontrast, MAT — arka planda kalmalı, ön-plan (pixel karakter/düşman) okunurluğunu BOZMAMALI.

## KATMANLAR (3 ayrı — parallax için)
1. **FAR (statik / çok-yavaş drift):** deep void + uzak parçalanmış kale silüetleri + soluk yıldız/toz. OPAK. ~2048×1152 (16:9 + parallax drift payı).
2. **MID (animated loop):** sürüklenen sis bankları + nabız atan cyan rift glow. **6 frame, seamless continuous loop**, ŞEFFAF PNG. Aynı boyut.
3. **FRONT OVERLAY (animated loop):** süzülen ember/toz parçacıkları + hafif enerji parıltısı. **6 frame, seamless loop**, ŞEFFAF PNG, ÇOK HAFİF (düşük alpha). Aynı boyut.

## FRAME / LOOP
Her animated katman **6 frame**, kesintisiz **seamless loop** (ping-pong değil). Teslim = layer başına ayrı PNG frame'ler (Unity sprite-sequence / Animated Tile) + opsiyonel tek sprite-strip.

## BOYUT / ORAN
16:9. Katman canvas ~**2048×1152** (parallax drift payı; ekran 1920×1080).

## STİL UYARISI (önemli)
RIMA pixel-art (PixelLab) sprite kullanıyor. Arka plan FAR/parallax olduğu için painterly-moody OLABİLİR AMA: low-detail, hafif blur/karanlık, "arkada" okunmalı — keskin detay / yüksek kontrast YOK (ön-planla yarışmasın). Mümkünse hafif pixel/dithered doku ki pixel sprite'larla uyumlu dursun.

## İLK ÜRETİM (öncelik)
**MID katmanı ile başla** (sürüklenen sis + cyan rift glow, 6 frame seamless loop, şeffaf PNG, 2048×1152) — en yüksek "canlılık" etkisi orada. Sonra FAR (statik silüet) + FRONT (ember overlay).

> Not (bizim tarafımız): bg = environment-katmanı → PixelLab-only kilidinin istisnası (yakın prop/karakter PixelLab kalır). Çıktıyı Unity'de parallax + Pixel Perfect'te titreme/okunurluk açısından doğrulayıp council'den geçireceğiz.
