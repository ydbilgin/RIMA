ROL: RIMA (2D top-down ARPG roguelite, Unity) için ENDÜSTRİ ARAŞTIRMASI. Orchestrator (Claude Opus 4.8) senin fikrini ALACAK, kararı KENDİSİ verecek. Sen araştır + sentezle, KARAR VERME. INLINE cevap ver — dosyaya yazma.

KONU: Top-down / high-top-down-3/4 aksiyon oyunları silahı 8 yöne nasıl render ediyor ve karakterin eline nasıl bağlıyor — silahı HER animasyon karesine bake ETMEDEN.

Spesifik oyunları araştır: Alabaster Dawn, Colossus: Eternal Blight, Hades, Hades II, Children of Morta, Death's Door, Moonlighter, Tunche, Wizard of Legend, Hyper Light Drifter.

Cevapla:
1. Silahı AYRI bir sprite olarak hand-anchor/bone'a mı bağlıyorlar, yoksa karakter sheet'ine yön başına BAKE mi ediyorlar? Hangisi baskın, neden?
2. 8 yön için kaç silah sprite'ı CIZILIYOR? (1 sprite + rotation/flip, birkaç, yoksa 8 elle çizim?) Arkaya bakan (kuzey) silah nasıl ele alınıyor?
3. Silah sprite'ının karaktere göre piksel/çözünürlük oranı?
4. El-bağlama tekniği: bone/socket mi anchor transform mı? Frame başına anchor pozisyon datası var mı? Silahın gövdenin ÖNÜNDE/ARKASINDA sıralanması yön başına nasıl flip ediliyor?
5. "VFX-first weapon" (Colossus): silah okunurluğunun ne kadarı swing VFX'i ne kadarı silah sprite'ı tarafından taşınıyor?

RIMA'nın MEVCUT kilitli yaklaşımı (DOĞRULA veya ELEŞTİR): Silah başına TEK sprite, 8 yön = HandAnchor child SpriteRenderer + OrientationSync kod (yön başına offset + sorting flip), silahı 8 yöne BAKE ETMEDEN. Karakter SILAHSIZ üretiliyor. PPU 64. Body sprite 120px canvas / 64px içerik. High-top-down 3/4 (~70-80°), Hades/CoM tarzı.

ÇIKTI: Sentezlenmiş İLKELER + RIMA için net öneri (kilidi onayla veya değiştir, oyunlardan gerekçeyle). Kısa, madde madde, dolgu yok. INLINE.
