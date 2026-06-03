# RIMA — Engel/Prop seti + 3-yön kapı SANAT YÖNÜ (deep design lens)

Sen kıdemli bir 2D izometrik oyun sanat-yönetmenisin. RIMA = "Shattered Keep" temalı izometrik ARPG roguelite. Derin/mimari sanat-yönü lens'iyle cevap ver (lean değil — en iyi tasarım).

ŞU GÖRSEL DOSYALARI OKU (sen oku, ben yapıştırmıyorum — büyük dosya inline YOK):
- STAGING/imagegen/concept01_hero_room_ISO.png
- STAGING/imagegen/concept03_sundered_beat_ISO.png
- STAGING/imagegen/concept05_portal_chest_ISO.png
- STAGING/imagegen/concept07_boss_arena_ISO.png
- Assets/Sprites/Environment/CliffKit_RefB_pixelified/cliff_S.png  (taş stili/palet eşleştirme)

Tema/palet: koyu slate granit + cyan mühür enerjisi #00FFCC + void-mor. Pixel-art, 8-10 renk, flat-lit, izometrik.
Iso cell = 0.96×0.585 world @ PPU64 → ~61×37px elmas taban. Proplar bottom-center pivot, hücreye oturmalı.

## Cevapla
1) **4-6 ENGEL/PROP** öner (Shattered Keep, gameplay-blocker + flavor karışımı, görsel çeşitlilik: dikey/yatay/parlak/karanlık). Her biri için: isim · 1-cümle görsel tarif · cell-footprint (1 veya 2 cell) · önerilen px canvas (genişlik ~64 katları, yükseklik bottom-pivot'a göre) · palet vurgusu (granit/cyan/void oranı). Konseptlerdeki öğelerle (kırık keep duvarı, rune, zincir, mühür-çatlağı) tutarlı olsun.
2) **3-YÖN KAPI** (Ön=S kameraya bakar / Sol=SW / Sağ=SE; Sağ = Sol'un flipX aynası). Kapı = oda çıkışı, dikey-rift portal kararıyla uyumlu (cyan rift kemer). On-brand kemer tasarımı nasıl: silüet, cyan rift yoğunluğu, taş çerçeve. Her yön için açının nasıl farklılaştığını (perspektif) anlat. px canvas (~128 geniş = 2-tile, yükseklik?). Ön ve Sol üretilip Sağ mirror'lanacak — bu mirror SW→SE için görsel olarak sorun yaratır mı (asimetri var mı)?
3) Kısa: bu set "hero room" konseptine ne kadar yaklaştırır, eksik kalan 1 on-brand öğe ne?

Kısa ve madde-madde yaz. Üretime-hazır spec hedefi.
