# DECISION — TBH Analizi + Yürüyerek-Seçim (Walk-Based CharSelect) Konseptleri (2026-06-05)

**Council:** ax-3.1-Pro (video analizi) ‖ Opus-advisor (web/Steam analizi, `_council_opus_tbh.md`) ‖
ax-3.5-Flash (yürüme-konsept turu + kod-feasibility) → Opus sentez. Girdi: kullanıcı "TBH: Task Bar Hero
giriş mantığı = yürüme; RIMA'ya özgü farklı bir şey yapabiliriz."

## TBH analiz özeti (3 advisor)
- TBH = Windows taskbar'ında yaşayan idle-RPG. Karakter "seçimi" aslında parti/Formation UI'ı — düz grid,
  yoğun hover-tooltip. **Model olarak RIMA'ya UYMAZ** (Opus-advisor: casual idle parti-yönetimi vs RIMA
  tek-karakter dark-fantasy run-commit). Asıl alınacak şey kullanıcının işaret ettiği **giriş fikri: YÜRÜME**.
- IA mikro-alıntılar (3.1 Pro): hover-tooltip yoğun bilgi sunumu · kilitlide şartı hover'da net göster ·
  panelleri küçült, sahne nefes alsın.

## 🎯 SENTEZ — ÖNERİLEN KONSEPT: **"Walkable Roster Room"** (A+B hibrit, RIMA'ya özgü)
Mevcut v3.2 roster-room adası AYNEN kalır (10 karakter karolarda, siyah silüetler, halka) — ama artık
EKRAN değil MEKÂN:
1. Oyuncu adaya **son-oynanan sınıf** olarak spawn olur (kapıya yakın). WASD ile yürür (mevcut
   PlayerController reuse — Flash kod-doğruladı).
2. Açık bir echo'ya YAKLAŞINCA: yan paneller o sınıfın bilgisiyle **kayarak belirir** (uzaktayken paneller
   GİZLİ → sahne nefes alır; TBH IA dersi + Opus-advisor "paneller sanatı boğuyor" kritiği çözülür).
   Üstünde `[E] Echo'ya Bürün` prompt'u.
3. **E = bürünme (possession):** kısa cyan rift-absorb (FLASH DEĞİL — yumuşak emilme; flash yasağı korunur);
   oyuncunun bedeni o sınıfa dönüşür; eski sınıf echo'su senin eski karona geçer (takas). LORE BİREBİR:
   Shattered Echoes = saçılan class yüzleri; seçim = o yüzü geri giymek.
4. **Kilitli silüete yaklaşınca:** itme yok (rahatsız etmez), sadece karanlık ripple + hover/yaklaşma
   tooltip'i: "150 ◈ Shattered Echo · veya Act 2 boss'unu Warblade ile yen" + yeterliyse `[E] Kilidi Aç`.
5. **Onay = kuzey Rift kapısından YÜRÜYEREK çıkmak** (canon K/B/D kapı kuralıyla uyumlu; SEÇ butonu kalkar
   ya da fallback'te kalır).
6. **Sürtünme sigortaları (Flash):** son-sınıf spawn + kapıya yakın → aynı sınıfla tekrar run <1.5sn ·
   **TAB = klasik tıkla-seç overlay** (v3.2 UI fallback olarak YAŞAR — hızlı restart isteyen için) ·
   yerleşim kompakt.

## Alternatifler (kullanıcı seçimine sunuldu)
- **B-saf "Echo-Mirror Pedestals" [S]:** pedestal yarım-çemberi, E-attune, merkez rift havuzu onay.
  Daha ucuz ama mevcut ada/yerleşim atılır.
- **C "Fractured Gallery Bridge" [L]:** yan-kaydırmalı ayna köprüsü — en sinematik, en pahalı. DEMO İÇİN RED.
- **v3.2 kalsın + IA polish [S]:** yürüme yok; sadece panel-küçültme + hover-şart tooltip'leri.

## Maliyet (Flash kod-incelemeli): Walkable Roster Room = **M**
Reuse: PlayerController + idle/walk anim + CharacterSelectScreen panel/veri akışı + trigger collider
pattern'i + EchoWallet. Yeni: vessel-swap mantığı, yaklaşma-trigger'lı panel show/hide, kapı-çıkış trigger,
TAB fallback toggle.

## ✅ KULLANICI KARARI (2026-06-05): **KONSEPT 2 + YÜRÜME + DUMMY = "ATTUNEMENT CHAMBER" (v4)**
Kullanıcı 2'yi seçti ve genişletti: sahne TEK RESİM backdrop (room_bg pipeline'ı) + üstüne gerçek
sprite'lar; donuk state = YENİ SPRITE GEREKMEZ (idle frame-0 durdur + taş-grisi tint; kilitli=siyah silüet);
bürününce NORMAL yürüme/idle ile odada serbest dolaşım; **vurulabilir DUMMY** (HP'li, yenilenir; görsel
ilk fazda placeholder, gerçek sprite sonra PixelLab GATED).
- 3 konsept görseli üretildi (`STAGING/imagegen/concept{1,2,3}_*.png`); kullanıcı 2'ye yöneldi.
- **Boş sahne backdrop ÜRETİLDİ + QC PASS:** `STAGING/imagegen/attunement_chamber_bg_empty.png`
  (boş pedestaller dairesel + merkez rift havuzu + kuzey uyuyan kapı + brazier'lar, on-brand).
- **Mimari karar (Opus):** Canvas-space mover (menü Canvas kalır; WASD RectTransform hareketi + mesafe
  trigger'ları; fizik/Rigidbody YOK — overkill). Dummy combat = faux (HitFlash+damage-number+HP bar UI;
  gerçek combat stack'i menüye sokulmaz).
- "5-5 yan sayfa" sorusu: GEREKMEZ — hepsi tek sahnede; roster büyürse mekân büyür (yürüme bunun çözümü).
- Implementasyon = cx task `STAGING/cx_task_charselect_v4_chamber_2026-06-05.md` (P1 yürüme+bürünme+kapı,
  P2 dummy).
