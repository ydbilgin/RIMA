# RIMA UI Redesign — Council Brief & Decision Draft (2026-06-04)

**Amaç:** 4 UI ekranını (MainMenu, CharacterSelect, Death, in-game SkillBar/HUD) NLM-canon "Vivid Vulnerability" tonuna ve "UI yoktur, sadece bilgi vardır" felsefesine göre yeniden tasarlamak. Kullanıcı council ile ortak karar istedi. Bu dosya = Opus taslağı; cx + Gemini critique → finalize.

## Cross-cutting canon (NLM, LOCKED — değiştirilemez kısıtlar)
- **Felsefe:** Opak dikdörtgen panel / dev çerçeve / siyah blok YASAK. Yarı-saydam, ince, keskin kenarlı, "kirli kâğıt üstüne mürekkep." Combat alanı okunur kalmalı.
- **Palet:** slate `#3A3D42` base · cyan `#00FFCC` accent (enerji/sınır/aktif) · warm `#E89020` secondary (ödül/brazier) · void-mor `#3A1A4A`/`#7B3FA0` (derinlik/Rage) · rift-red sadece tehlike/boss. **Renk = anlam.**
- **Font:** tiny pixel + pixel serif; sayılar minimal (HP max gösterilmez).
- **Rarity:** common gray / rare teal-blue / epic purple / legendary gold.
- **Asset:** 9-slice frame + runtime text/icon; baked gameplay text YOK. Reuse `Resources/UI/RIMA/Pack/` (bg_seal_keep, pedestal_seal, card_frame_9slice, button_9slice, bar_frame_9slice, bar_fill, panel_frame_9slice) + RimaUITheme helpers. Eksikse imagegen (ax generate_image / cx $imagegen) ile ON-BRAND backdrop üret.
- **KULLANICI KISITI:** Build seed YOK. Death ekranındaki "COPY BUILD SEED" butonu kaldırılacak; hiçbir ekrana seed input eklenmeyecek.

---

## 1) DEATH SCREEN (kullanıcı: "gerçekten berbat")
**Mevcut (DeathScreenManager.cs):** runtime panel, slow-mo→freeze→fade. Sorunlar: button rect'leri çok sıkışık (18-40%/42-62%/64-86%), stil tutarsız (Wishlist cyan border, diğerleri muted), stats 16pt küçük, "COPY BUILD SEED" + "TRY AGAIN" + "MAIN MENU" + "WISHLIST" 4 buton dar bantta üst üste, NextClassTeaser statik ek.

**Proposed redesign:**
- Tam-ekran void karartma (85% void-mor→siyah radial), backdrop sprite opsiyonel (yoksa düz).
- ORTADA dikey akış, geniş nefes payı: (a) kısa canon ölüm satırı (rastgele, 26-30pt pixel-serif, TextPrimary) — mevcut canon satırları kullan; (b) ince ayraç (cyan hairline); (c) run stats DİKEY, ince, küçük pixel font, sadece `ODA · KILLS · SÜRE` (Build satırı KALDIR); (d) 2 birincil aksiyon yatay: **TEKRAR DENE [R]** · **ANA MENÜ** (eşit stil, button_9slice translucent, cyan hover); (e) WISHLIST = en altta küçük, ikincil (marketing, dramayı bozmasın).
- Tutarlı stil: tüm butonlar aynı frame + cyan hover; panel border = ince cyan hairline (opak kutu değil).
- COPY BUILD SEED **KALDIR**. NextClassTeaser KALDIR (clutter) ya da tek satır sessiz hint'e indir.
- Opsiyonel polish: ölüm anında ekran çok hafif desature + vignette (kod, ucuz).

**Açık soru (council):** Wishlist butonu kalsın mı yoksa sadece menüye mi taşınsın? Ölüm satırı sınıf/oda-bağlamlı mı olsun (canon "denilebilir ama asla kişisel değil")?

---

## 2) MAIN MENU (kullanıcı: "nasıl güzel olabilir")
**Mevcut (MainMenuScreen.cs):** runtime, dark fill + main_menu_bg backdrop + "RIMA" logo + "THE SEAL BENEATH THE KEEP" subtitle + 3 buton (NEW RUN/SETTINGS/QUIT) + "S43 Dev Build" label. SETTINGS stub.

**Proposed redesign:**
- **Tagline DEĞİŞ:** canon "epik slogan yok, sessiz tespit" → "THE SEAL BENEATH THE KEEP" yerine sessiz canon satır: **"Yine geldin."** (ya da "Burası hatırlar." / "Rift açık. Her zaman açık oldu.") — küçük, cyan, fısıltı.
- **Atmosfer:** ON-BRAND backdrop (kırık keep silüeti + cyan rift parıltısı + void derinlik). main_menu_bg yoksa/zayıfsa imagegen ile üret. Hafif paralaks/yavaş bloom-pulse (ucuz, ama D3D11 sonrası).
- **Logo:** "RIMA" pixel-serif, cyan rift kenar-glow accent (baked değil, runtime renk).
- **Butonlar:** ink-on-paper, translucent button_9slice, dikey ortada-sol ya da ortada; cyan hover. NEW RUN birincil (hafif vurgulu). SETTINGS stub ise ya gizle ya "yakında" disabled. QUIT.
- Version label = sağ-alt, çok küçük, %35 opacity.

**Açık soru (council):** SETTINGS butonu kalsın mı (stub) yoksa gizlensin mi? Backdrop reuse mu imagegen mi (mevcut main_menu_bg kalitesi)?

---

## 3) CHARACTER SELECT — yan açıklama paneli (kullanıcı: "karakter seçince yanda açıklaması yazsın")
**Mevcut (CharacterSelectScreen.cs):** 3-panel (sol 10-kart grid · orta portrait+tagline · sağ "IDENTITY"). **Sorun: sağ panel SADECE statik label (LMB/SKILL1-4/PASSIVE) — sınıf seçilince güncellenmiyor, gerçek açıklama yok.**

**Proposed redesign (kullanıcının ASIL isteği):**
- **Sağ panel = dinamik sınıf kimliği.** Sınıf seçilince doldur:
  - **Tematik kimlik** (1 satır slogan, accent renk) — örn Warblade: "Yaklaş. Sabitle. Zırh kır. İnfaz et."
  - **Oynanış tarzı** (1-2 cümle, wrap, TextMuted).
  - **Ana kaynak/mekanik** (örn "Rage 0-100 · sadece vurarak dolar").
  - Accent bar = ClassAccent(cls).
- **Veri kaynağı:** NLM 10-sınıf kimlikleri (bu session çekildi, aşağıda Ek-A). `RimaUITheme.ClassTagline` zaten line1/line2 var → genişlet ya da yeni `ClassIdentity(cls)` ekle (identity + playstyle + resource).
- Orta panel portrait+tagline kalır; sağ panel artık zengin + dinamik. Kilit durumunda: açıklama görünür ama "X Echoes gerekli" eklenir.

**Açık soru (council):** Skill listesi (LMB/4 skill/passive) hâlâ gösterilsin mi (gerçek skill adlarıyla) yoksa sadece kimlik+oynanış+kaynak mı? Skill adları controller'lardan çekilebilir mi (kapsam)?

---

## 4) IN-GAME SKILL BAR / HUD (kullanıcı: "skill bar nasıl olmalı")
**Mevcut (SkillBarUI.cs):** 6 hex slot (2 primary 56px + 4 secondary 44px), bar_frame_9slice backing, radial cooldown, class controller resolution. Sorunlar: key label çok küçük (10/8pt), glow statik cyan (class accent değil), backing görünürlük belirsiz.

**Proposed redesign:**
- **Key label** okunur boyut + kontrast (alt-sağ köşe, ince outline/shadow ki floor üstünde okunsun).
- **Glow/accent = ClassAccent(cls)** (cyan sabit yerine sınıf rengi; ready state'te accent glow).
- **Cooldown** radial fill netliği: koyu overlay + ince accent kenar; bitince kısa flash (ucuz juice).
- Backing: translucent (canon "altında/üstünde yatay ayırıcı çizgi YOK").
- Cooldown sayısı opsiyonel (canon "sayı minimal" → sadece radial, sayı yok ya da çok küçük).
- HP/Resource bar mevcut iyi; dokunma (canon uyumlu). Skill bar accent + label = ana iş.

**Açık soru (council):** Cooldown'da sayısal saniye gösterilsin mi (canon "sayı minimal")? Slot boyutları/spacing değişsin mi?

---

## Cross-cutting plan
- Tüm değişiklik runtime UI kodu (sahne churn'ü minimal) → compile-verify GPU'suz yapılabilir; play-verify Unity D3D11 restart sonrası.
- Sıra önerisi: (1) Death (en kötü, en görünür) → (2) CharSelect yan panel (net istek) → (3) MainMenu → (4) SkillBar. Her biri ayrı commit.
- imagegen ihtiyacı: MainMenu backdrop + (ops) Death backdrop. Önce mevcut asset değeri kontrol, sonra üret.
- **Build seed: hiçbir yerde YOK.**

## Ek-A: 10 sınıf NLM kimlikleri (char select veri kaynağı)
- **Warblade** — "Yaklaş. Sabitle. Zırh kır. İnfaz et." / Ağır zırh-kırıcı 2-el kılıç + parry, ritmi dikte. / Rage 0-100, sadece vurarak/CC ile dolar; Sundered→infaz. / `#C09455`
- **Ranger** — "Sana ulaşamazlar. Her saniye kayıp veriyorsun." / Mesafe disiplini + tuzak + Mark. / Focus, 4m+ mesafede dolar yakında erir. / `#7BA7BC`
- **Shadowblade** — "Görmeden önce hissedilir." / Hızlı pozisyonel, Phase + toplu infaz. / Energy+Combo, Rift Scar/Sever. / `#5A2A8A`
- **Elementalist** — "Her şeyi yakıyorum. Ama önce ritmi buluyorum." / Fire/Frost ritmi → Lightbreak. / Mana+Elemental State. / `#FFF000`
- **Ravager** — "Az canken daha tehlikeliyim." / Düşük-HP momentum, çift balta kan-furyası. / Fury, sadece hasar ALARAK dolar (düşük HP=hızlı). / `#D43F3F`
- **Ronin** — "Çek. Kes. Kın. Bir nefeste." / İaido bekle-cezalandır, hareketsizlik. / Tension, kın+hareketsizken birikir. / `#C8A87A`
- **Gunslinger** — "Mermin yok. Senin zamanın da yok." / Kinetik run-and-gun, ısı yönetimi. / Heat, mükemmel "Sıfır" soğutma. / `#FF4400`
- **Brawler** — "Düşersen kalk. Ama önce yumruğum kalkar." / Silahsız ritmik kombo, whiff-punish. / Charge 0-5, Shattered. / `#FF8C00`
- **Summoner** — "Ben savaşmıyorum. Feda ediyorum." / Minyon kondüktör, feda-burst. / Charges 0-4, çağır+feda. / `#00FF88`
- **Hexer** — "Sabır. 10'a gelince sen bitiyorsun." / Lanet biriktir+yay, karar-anı patlat. / Hex Stacks 0-10, Hexblast. / `#8B0000`
