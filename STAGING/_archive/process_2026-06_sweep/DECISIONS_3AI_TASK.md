ACTIVE RULES: (1) think before deciding (2) min/no speculation (3) surgical (4) flag if unclear.

NLM ACCESS: query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read: kod / STAGING / CURRENT_STATUS / PROJECT_RULES / memory.

# RIMA — 3 KARAR (her biri için: seçenekler + ÖNERİN + gerekçe)

Bağlam: RIMA Faz 1 demo, top-down 3/4 ARPG (Hades/Children of Morta tarzı). Combat çekirdeği hazır (silah mount + windup timing + juice). Şimdi hatasız oynanabilir standalone build hedefleniyor. Aşağıdaki 3 karar LOCK'lanacak. Codex: teknik/Unity feasibility odaklı cevapla. agy: referans oyunlar nasıl yapıyor + research odaklı cevapla.

## KARAR 1 — Gate (kapı) davranışı
Oda geçiş kapısı nasıl olmalı?
- (a) Oda temizlenince kapı ORTAYA ÇIKAR (appear-on-clear).
- (b) Kapının yeri BAŞTAN BELLİ + KİLİTLİ, oda temizlenince AÇILIR (location-visible, clear-to-unlock).
- (c) Fragment toplanması ZORUNLU → her kapı fragment ister.
Mevcut `Gate.cs` state machine: Locked / AwaitingFragment / Unlocked. Demo readability + Hades/ARPG pattern ışığında hangisi? Map Fragment nereye oturur (her gate mi, sadece reward room mu)?

## KARAR 2 — Cliff derinlik/yerleşim
Kullanıcı kuralı: cliff'ler ZEMİNLE ASLA AYNI SEVİYEDE OLMAZ — ya ÇOK ALTINDA ya ÇOK ÜSTÜNDE (floating-island derinliği). Şu an: messy 283 auto-cliff (iç hücreler + lob-arası bant + zemin-üstü X'ler = saçma). L3 parallax (factor 0.08, sorting -400) ambiyans için cliff'in baya altında olacak (PARALLAX_L3_DESIGN.md).
Sorular: (1) temiz dış-perimetre cliff (ada kenarı, aşağı inen) tek başına yeterli mi, yoksa ek "çok üstte" dekoratif cliff de mi? (2) "çok altta/çok üstte" derinlik teknik olarak nasıl (sorting layer / Z offset / parallax factor farkı / scale)? (3) iç/lob-arası/X cliff'leri silme kriteri ne olmalı (perimetre tanımı)? Referans oyunlarda floating-arena kenarı nasıl çözülüyor?

## KARAR 3 — Weapon canvas boyutu (PixelLab üretim hazırlığı)
Kullanıcı dönünce PixelLab'dan silah üretecek — canvas boyutu LOCK'lanmalı. Locked: PPU 64, 8-dir (5 üret + 3 mirror flipX), HandAnchor mount, top-down 3/4. Body: 64px içerik / 120px canvas.
Mevcut referans: `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` (sprite boyutunu Unity'den oku). Sorular: (1) silah sprite canvas boyutu kaç px olmalı (büyük silah greatsword vs küçük dagger ayrı mı, tek standart mı)? (2) pivot/mount noktası nerede (HandAnchor ile hizalı)? (3) body 64/120 + PPU 64 ile oransal doğru görünüm için silah px aralığı? Net sayı(lar) ver — PixelLab'a girilecek.

### KARAR 3b — Weapon YÖN üretimi (KRİTİK — kullanıcı sordu)
Silah 8 YÖNDE Mİ ÜRETİLECEK? RIMA mevcut lock'u (`project_weapon_system_8dir_lock`, A2'de implement edildi): **TEK sprite/silah**, body 8-dir weaponless çizilir, silah HandAnchor child SR olarak takılıp OrientationSync ile per-yön döndürülür/offsetlenir — silah 8 kez ÇİZİLMEZ.
Sorular: (1) ENDÜSTRİDE top-down 8-dir oyunlar silahı nasıl üretiyor — 8 yön baked frame mi, yoksa tek-sprite-mount+rotate mi? (agy research: Hades, CoM, Colossus Eternal Blight, top-down ARPG'ler). (2) RIMA'nın tek-sprite-mount yaklaşımı doğru/yeterli mi, yoksa bazı yönlerde (örn. N/arka) silahın ayrı çizilmesi gerekir mi? (3) Tek-sprite yaklaşımı görsel olarak hangi durumda kırılır (asimetrik silah, perspektif) ve çözümü? Net tavsiye: kaç sprite üretilecek (1 mi, birkaç yön mü, 8 mi) ve neden.

# Çıktı (her AI)
Her karar için: seçim + 1-2 cümle gerekçe. Net, kısa, uygulanabilir.
