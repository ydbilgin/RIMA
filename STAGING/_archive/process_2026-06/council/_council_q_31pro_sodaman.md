# RIMA Council — Sodaman analizi (DEEP UX / ARCHITECTURE lens — Gemini 3.1 Pro High)

Sen RIMA için bir DERİN UX / oyun-his danışmanısın. Çıktın RIMA orkestratörüne (Opus) gider; o nihai kararı verir. Tasarım yargısı ve mimari öner; kod yazma.

Eğer web erişimin varsa Steam sayfasını da incele: https://store.steampowered.com/app/2178990/Sodaman/ (ek detay için). Erişimin yoksa aşağıdaki fact-sheet'i kullan.

## SODAMAN — FACT SHEET (ground-truth)
- Tür: cyberpunk **bullet-heaven roguelite**, top-down, pixel 2D. Dev Tape Corps (4 kişi). ~%78 olumlu / 630 inceleme. ~4-8 saat içerik.
- Core: süper-asker vs alien sürüleri, prosedürel gezegenler, "soda-powered" build'ler.
- **İMZA SKILL-DRAFT:** level-up'ta power-up'ı **3 SODA KUTUSUNDAN** seçersin (tematik kap) — tür-standardı "3 kart" yerine. **7 soda RENGİ**, her renk **10+ skill**. Renk karıştırma = "soda kokteyli" = sinerji efektleri ("mix, shake, add a dash of fury").
- Meta/deck: haritaya saçılı **40+ enhancement kartı** → deck kur. **6 vücut parçası + 30+ sibernetik augment** (boss/sandık blueprint). **8 silah** farklı playstyle, sinerji keşfi.
- Run-arası **uzay-gemisi HUB** (customize/meta). 91 achievement, score attack.

## RIMA — BAĞLAM
2D iso roguelite ARPG, **10 sınıf**, Hades/Children-of-Morta 3/4 top-down look, URP 2D + Pixel Perfect 640×360, PPU64. **Tasarım canonu:** "Vivid Vulnerability / UI yoktur sadece bilgi vardır" — **opak panel kutuları YASAK**, ink-on-paper estetik, **renk = anlam**, diegetic. Lore: Rift/Echo (cyan #00FFCC = Rift enerjisi + antik mühürler; void-mor). Currency = "Echo" (isim emin değil).
**Mevcut skill-seçim sistemleri:** (a) DraftManager + SkillOfferUI = 3-kart reward draft (hover-scale + select-flash + rarity_glow + obsidian/cyan 9-slice frame), (b) SkillBar = in-run Q/E/R/F/Z/X class-accent ready-glow, (c) CharacterSelect "roster room" diegetic, (d) RimaUITheme merkezi tema + class accent renkleri + PassiveIcon.

## KULLANICININ ASIL DERDİ
Kullanıcı "skill seçerken **hover gibi şeyler** var, bunları nasıl ekleyebiliriz" diyor + Sodaman'dan "neler alınabilir." Ayrıca 2 BEKLEYEN ÖZELLİK tasarlamak istiyor:
1. **Run-içi sol açılır-kapanır skill paneli** — bir TUŞLA aç/kapa; o run'da equipped/selected skilleri gösterir (in-game HUD).
2. **ESC → codex-tarzı detaylı skill ekranı** — oyunu durdurup TÜM sınıfa-özel skilleri görme (build/theorycraft).

## SENİN ANALİZİN — şu 5 başlığı derinlemesine işle
1. **NE ALINIR / NE ALINMAZ (Sodaman → RIMA):** 3-kap draft theming, renk-kodlu skill aileleri (okunabilirlik + sinerji sinyali), deck/meta katmanı, run-arası hub, augment-slot sistemi — her birini RIMA canonu (renk=anlam, diegetic, Rift/Echo) ile uyumu açısından AL/ALMA + NEDEN. RIMA'nın 10 sınıf-rengi Sodaman'ın 7-renk okunabilirlik mantığına nasıl eşlenir?
2. **SKILL-SEÇİM HOVER & FEEDBACK (en kritik):** En iyi-sınıf hover/seçim feedback'i tasarla — hover-lift/scale, glow, **tooltip reveal** (skill tam metni hover'da açılır), **synergy highlight** (hover edilen skill, halihazırda equipped olan combo'ları "aydınlatır"), audio/juice katmanları, **controller/gamepad focus** uyumu, readability. Referans: Hades boon-hover (kısa+net+rarity renk), Dead Cells, Brotato/Vampire Survivors tooltip. RIMA'nın "opak kutu yasak / ink-on-paper" canonuyla tooltip nasıl çizilir (kutu değil, glow+hairline)?
3. **RUN-İÇİ SOL SKILL PANELİ (toggle):** Etkileşim modeli + görsel hiyerarşi. NE gösterir (equipped aktifler Q/E/R/F + pasifler + cooldown durumu + aktif sinerjiler), nasıl slide eder (sol-kenar, kısmi peek mi tam mı), hangi tuş, diegetic nasıl olur (Echo/Rift motifi). Combat'ı bloklamadan glance-able olmalı.
4. **ESC CODEX SKILL EKRANI:** Tam-ekran mı yoksa sağ-panel mi (kullanıcı bunu soruyor — öner + neden)? TÜM sınıf skilleri grid/tree, hover→detay, locked/unlocked görsel ayrımı, sinerji grafiği/highlight, build-planlama hissi. Sodaman'ın augment/hub ekranı + Hades "Boons" codex + Path-of-Exile passive-tree okunabilirlik dersleri.
5. **CURRENCY adı:** "Echo" RIMA Rift/Echo lore'una oturuyor mu, yoksa daha güçlü diegetic bir isim mi (öner 2-3 alternatif + gerekçe)? (Hafif dokun — asıl NLM canon kararı.)

ÇIKTI: Her başlık için net tasarım + somut etkileşim/görsel spesifikasyon + öncelik (hangisi en çok his katar). Genel laf değil, RIMA'ya uygulanabilir konkre öneri.
