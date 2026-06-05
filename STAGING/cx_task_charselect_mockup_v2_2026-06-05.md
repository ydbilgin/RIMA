ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v3 HTML mockup üret (HTML-first akış: kullanıcı tarayıcıda onaylayacak → sonra Unity'ye çevrilecek). Council kararlarına %100 uyum.

# READ FIRST (sırayla)
1. `STAGING/CHARSELECT_V3_DECISION_2026-06-05.md` — TÜM kararlar + kullanıcı onayları (✅ bölümü dahil). Bu doküman = spec.
2. `STAGING/_council_brief_charselect_v3.md` — kullanıcı SABİT direktifleri + NLM canon.
3. `STAGING/mockups/charselect_concept_ref_2026-06-05.png` — görsel yön referansı (birebir kopya DEĞİL; bakabiliyorsan bak).
4. `STAGING/mockups/charselect_mockup.html` — v1 (DEĞİŞTİRME; sadece teknik altyapısına bak, sprite path'leri vb. reuse edilebilir).

# Görev
**YENİ DOSYA:** `STAGING/mockups/charselect_mockup_v2.html` — tek dosya, self-contained (CSS+JS inline), file:// ile tarayıcıda direkt açılır.

## Zorunlu içerik (decision doc'tan; kısaca)
- **Sahne:** void-mor arka plan + iso yüzen taş ada hissi. Mevcut backdrop varsa reuse: `Assets/Resources/UI/RIMA/CharacterSelect/room_bg.png` (relative path ile). Uymazsa CSS gradient void + CSS iso karo (diamond) grid çiz. Ada altı SAF karanlık void — altta dekor/resim YOK.
- **Karakterler:** 10 sınıf (Warblade, Elementalist, Ranger, Shadowblade, Ronin, Ravager, Gunslinger, Brawler, Summoner, Hexer). GERÇEK idle_south sprite'larını bul ve relative path ile kullan (v1 mockup hangi path'leri kullanıyorsa onlardan başla; alternatif: `Assets/Resources/UI/RIMA/CharacterSelect/` ve `Characters/` altına bak). `image-rendering: pixelated` şart.
- **Dizilim:** 5+5 iki sıra, TEK iso düzlem hissi, her karakter bir karo merkezine snap, arka sıra yarım-karo stagger, sıralar arası boşluk. TÜM sprite'lar AYNI boyut (seçilide büyüme YOK).
- **Kilitli (6 sınıf: Ronin/Ravager/Gunslinger/Brawler/Summoner/Hexer):** sprite OPAK SİYAH silüet (CSS `filter: brightness(0)`) + çok hafif cyan rim (`drop-shadow(0 0 1px rgba(0,255,204,.4))`) + ayak dibinde kilit glifi + etiket: canon fiyat + OR-yolu (örn. "150 ◈ · veya Act 1 ×3"). Canon fiyatlar: Ravager/Ronin 150, Gunslinger/Brawler/Summoner 200, Hexer 250. OR-yolları decision doc'ta.
- **Açık (4):** Warblade(default seçili)/Elementalist/Ranger/Shadowblade normal renkli, altında isim etiketi.
- **Seçili:** cyan ayak-halkası + soft aura SADECE. Hover: daha soluk halka. HİÇBİR öğe dikey hareket etmez (bob/parallax YASAK; glow opacity nefesi OK).
- **Sol panel (ince taş çerçeve, ink-on-paper, opak slab YOK):** ad + 3 tag + motto(1 satır) + açıklama(2 satır) + kaynak bloğu (RAGE vb.) + 5 stat barı (Hasar/Dayanıklılık/Hız/Kontrol/Zorluk, cyan segment).
- **Sağ panel (AYNI çerçeve):** "YETENEKLER" — sınıfın TAM skill listesi, scroll YOK → kompakt: 3 başlangıç aktifi ikon+isim+1-satır açıklama; kalan skill'ler ikon+isim satırı; **açılma-şartlı skill'ler karanlık + SADECE isim + şart metni** ("Açılış: ..."), açıklama gizli. Skill verisi: Warblade+Elementalist gerçek isimler kullan (NLM'den sorgula veya `Assets/Scripts/Skills/SkillDatabase.cs`'den oku), diğer sınıflar temsili placeholder isim OK.
- **Üst bar:** sol "RIMA — KARAKTER SEÇ", sağ "◈ 80" (hover title="Shattered Echo"). Başka öğe YOK.
- **Alt:** TEK merkezi state-driven buton: SEÇ / "KİLİDİ AÇ — 150 ◈" (para yeterse) / "YETERSİZ ◈" (pasif). Taş-plaka stili, web-button değil. Sol-alt küçük GERİ. Mağaza hissi YASAK.
- **JS:** data-driven — her karakter tek JS objesi {id, name, tags, motto, desc, resource, stats[5], skills[], locked, cost, orPath}. Tıkla-seç → paneller + buton state güncellenir. Kilitliye tıklanabilir (bilgisi panelde açılır; isim görünür, detaylar "?" olabilir). Demo Echo = 80 (görseldeki gibi; YETERSİZ state'i test edilebilsin).
- **Işık:** panellerin arkasına radial-gradient cyan (merkez) + turuncu (üst köşeler, brazier hissi). Brazier görseli varsa statik kullan.
- **Tipografi:** premium his — Google Fonts YOK (offline çalışmalı); system-stack + letter-spacing ile çöz.

## YASAKLAR
- Dikey scroll (panel overflow hidden, içerik sığacak şekilde kompakt).
- Herhangi bir öğenin dikey pozisyon animasyonu.
- Seçilide scale/merkeze taşıma.
- "Echo satın al" / mağaza öğesi.
- 400-Echo veya HERHANGİ skill fiyatı (skill'ler PARA ile açılmaz — sadece achievement şartı metni).
- v1 dosyasını değiştirmek.

## Çıktı
- `STAGING/mockups/charselect_mockup_v2.html` (yeni dosya).
- CODEX_DONE.md: ne yaptın, sprite path'leri nereden buldun, bilinen kısıtlar (3-5 madde, kısa).
