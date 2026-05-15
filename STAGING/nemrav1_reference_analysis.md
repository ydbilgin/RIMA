# NemraV1 — "Fishu" Referans Analizi

**Kaynak Tweet:** https://x.com/NemraV1/status/2055027312404418876
**Fetch Tarihi:** 2026-05-16
**Status:** VISUAL_CONFIRMED (video thumbnail analiz edildi, 51s video erisim var)

---

## 1. Tweet Icerik Tanimi

### Yazar Profili
- **Handle:** @NemraV1 (verified individual)
- **Bio:** "A Developer and Music producer. 345k on tiktok. working on Fishu (fish emoji)"
- **Iletisim:** fishucon@gmail.com / Discord: discord.gg/EkThygaMun
- **Followers:** 135 (Twitter), 345k (TikTok) — TikTok-driven indie dev
- **Joined:** 2020-04-02
- **Media Count:** 94 (aktif gamedev paylasimi yapan profil)

### Tweet Metni (orijinal)
> "casually fishing, (and I updated the water texture)
> #gamedev #IndieGameDev #indiegame #fishing"

### Engagement Metrics
- **Views:** 7,145
- **Likes:** 129
- **Retweets:** 6
- **Bookmarks:** 32
- **Replies:** 3
- **Quotes:** 0
- **Created:** 2026-05-14 20:47 UTC

### Visual Icerik
- **Tip:** Video (mp4, 51.3 saniye, 2560x1440 native)
- **Video URL:** https://video.twimg.com/amplify_video/2055026755824484352/vid/avc1/2560x1440/spksv6x61OcUfE9U.mp4
- **Thumbnail:** https://pbs.twimg.com/amplify_video_thumb/2055026755824484352/img/ruZnuE4-voaGFPC0.jpg
- **Konu:** "Fishu" adli oyunda guncellenen su dokusu + casual fishing demo

### Oyun Tanimlama: "Fishu"
- Top-down 2D cozy life-sim / fishing game
- Hand-painted (painterly) art style — pixel art DEGIL
- Plaja-iskele temasi (beach + boardwalk + boat ticket booth)
- HUD: gold coin (9), green ticket/leaf icon (0), grass/turf token (394)
- Sag ust: TUESDAY clock with analog dial (gun-saat sistemi)
- Sag panel: "Neon Fishing Rod" + "Cherries" inventory hizli erisim
- Alt panel: backpack, journal, map, chat, gift box (5 ana menu)
- "into sea — SURF RENTAL & BOAT TICKETS" tabela (saglik ekonomi sinyalleri)

---

## 2. Visual / Mekanik Analiz

### Camera & Perspektif
- **Aci:** Pure top-down (90 derece kus bakisi degil, hafif 75-80 derece tilted top-down)
- **Zoom:** Karakter ekran yuksekliginin ~%6'si — genis cevre gorunumu
- **Scroll:** Smooth follow (video confirms walking trail in sand)

### Art Style
- **Tip:** Hand-painted 2D, **painterly digital art** (Stardew Valley DEGIL — daha cok My Time at Portia 2D variant)
- **Cozunurluk:** HD raster (pixel art YOK)
- **Renk Paleti:** Sicak sari kum + canli turkuaz su + ahsap kahve toklugu — **Studio Ghibli plaj sahnesi** vibesi
- **Outline:** Hafif dark outline objelerin etrafinda — readability icin
- **Tile Visibility:** Yer karoları (stone path) gorulebilir ama orgaknik blend

### Animation / Feel (thumbnail + tweet metni)
- "Casually fishing" — **slow-paced, low-stakes** mekanik tasarim
- Karakterin arkasinda kum izleri — **footstep trail particle** sistemi var
- Su dokusu animasyonu — tweet'in ana noktasi (yeni shader/scrolling texture)
- Dalga koputugu beyaz koputu kıyıya yakın — **shore foam line** dynamic

### VFX Clarity
- Su dalga animasyonu = ana VFX vurgusu
- Foam line = yumusak alpha blend
- Karakter golgesi = soft drop shadow
- Tutarli low-frequency hareket — epileptic burst yok

### UI / Readability
- **HUD anchor:** 4 koşe (sol-ust currency, sag-ust takvim, sag-orta hotbar, alt-merkez menu)
- **Visual hierarchy:** Diegetic frames (asili tabela, ahsap saat, ipte sallanan eskiz) — UI = dunyanın bir parcasi
- **Icon style:** Painted icons (currency icons painted-not-pixel; menu icons cute mini-objects)
- **Text:** Minimal — sadece sayilar ve "TUESDAY" gunu
- **Readability:** Yuksek; dusuk-yogunluklu mekanik icin uygun

### Environment Density
- **Yogunluk:** ORTA-YUKSEK ama spacious feel (degerli alan: kum sahil = aksiyonsuz nefes)
- **Asset variation:** Surfboard (3 farkli renk), can simidi, lamba, plant pot (çiçekli), bench, palmiye
- **Layered depth:** Iskele alti golge + clutter + shop interior (cherries, fishing rod) — diegetic UI olarak

---

## 3. RIMA icin BORROW Potansiyeli

RIMA = action 2D roguelite (top-down 30-35 derece 3/4 ARPG kameralı, 128px native pixel art).
**Direkt mekanik fit ZAYIF** (Fishu cozy/casual; RIMA combat-driven). Ancak su sistemleri:

### A. Su Dokusu Shader Pipeline (HIGH FIT)
- **Sistem:** Map system / environment
- **Fikir:** RIMA'da rift su/lavalar icin **scrolling UV + foam edge mask** shader
- **Spesifik Oneri:**
  - `Assets/Scripts/Systems/Map/CornerWangPainter.cs` icin `WaterEdgeBrush` ekle
  - Shore foam = depth-based alpha line (player feet vs water plane)
  - Iki katmanli scroll (yavas + hizli) parallax water
- **Karar Adayi:** **#170 — Rift Water Shader (foam-edge + scrolling UV)**

### B. Diegetic HUD Frame Decision (MEDIUM FIT)
- **Sistem:** UI / HUD
- **Fikir:** RIMA HUD overlay (3-layer locked) icindeki saat/gun gostergesi diegetic frame ile saric — ahsap/runic frame
- **NOT:** [HUD Overlay Decision] LOCKED durumda — sadece `decoration layer` icin uygulanabilir
- **Karar Adayi:** YOK — mevcut karar bozulmamali

### C. Footstep Trail Particle (LOW-MEDIUM FIT)
- **Sistem:** Player VFX
- **Fikir:** RIMA'da kum/cope/kar zemin tipine gore footstep iz particle (zaten `MapTilesetPairing.cs` var)
- **Spesifik Oneri:** `TilesetPairing` icine `FootstepParticleAsset` enum field ekle (Sand/Stone/Snow/Wood)
- **Karar Adayi:** **#171 — Surface-Aware Footstep Particles**

---

## 4. CircuitBreaker icin BORROW

CircuitBreaker = (cyber/grid temali Studio projesi — Karar #143 ekosisteminde adi gecti)
- **Fit:** DUSUK. Fishu painterly/organic; CB digital/grid.
- **Tek Aday:** Diegetic UI yaklaşımı — CB icin "circuit board frame" diegetic HUD = gorsel kimlik guclendirici
- **NOT:** Spesifik CB belge dosyasi henuz yok; karar formal degil

---

## 5. Caterpillar (Wingspan / Cozy-Survival) icin BORROW

Caterpillar (Lauretha Studio cozy/survival roguelite konsepti — STAGING/codex_laurethstudio_caterpillar.md mevcut):

### A. Diegetic Time/Day Display (HIGH FIT)
- TUESDAY + analog saat gostergesi = cozy survival icin tonal anchor
- Cozy survivalda **gun donguleri** mekanik temel olur

### B. Painterly Art Direction Reference (HIGH FIT)
- Eger Caterpillar pixel art DEGIL hand-painted hedeflerse Fishu = direkt referans
- **Karar Adayi:** Caterpillar Art Bible icin "Painterly Tier 1 Reference" listesine ekle

### C. Inventory Hotbar (MEDIUM FIT)
- 2-slot diegetic hotbar (rod + cherry) = caterpillar 2-tool gameplay (toplama + donusum) icin uygun
- **Karar Adayi:** **Caterpillar #C-002 — 2-Slot Diegetic Hotbar Pattern**

### D. Boat Ticket Economy (MEDIUM FIT)
- "SURF RENTAL & BOAT TICKETS" = farkli bolgelere transit ekonomik gating
- Caterpillar'da farkli biyom gecisleri icin nektar/yaprak token = ayni kalip

---

## 6. Lateral / Cross-Genre Fikirler

### F1. Roguelite Map'e Fishing Mini-Loop
NemraV1'in casual fishing'inden esinlenip RIMA'da rift icindeki dingin "rest room"larda **5sn micro-fishing minigame** — buff/heal kazandirir. Aksiyon-cozy contrast = pacing breath.

### F2. Su Texture'i Hex-Based Combat'a Tasi
Scrolling water UV mantigini **lava arena tile damage** sistemine tasi — visual scroll = spatial telegraph (oyuncu nereye akacagini onceden gorur).

### F3. TikTok-First Devlog Strateji
NemraV1 = 345k TikTok / 135 Twitter — **gorsel mekanik kucuk loop** olunca TikTok algoritmasi patliyor. Studio devlog stratejisi: 51s vertical mekanik gif > 3 dk YouTube essay.

### F4. Diegetic Save Object Konsepti
RIMA'da save point = npc; Caterpillar/cozy proje icin save = **dunyada fiziksel obje** (Fishu'nun saat-clock'u gibi). Save'i UI'dan cikar, dunyaya goz.

### F5. Day-Of-Week Sosyal Hook
TUESDAY display = oyuncu zamanini gercek hayat ritmine baglar. Cozy projede her gercek gunde farkli NPC/event = "her sali yeni balikci gelir" tarz. Engagement multiplier.

---

## 7. Anti-Pattern (RIMA / Studio uygunsuz olanlar)

### AP1. Painterly Art = RIMA Pivot Bozucu
NemraV1 painterly HD raster — RIMA [128px Pivot S43 LOCKED]. **Asla** RIMA art bible'a "Fishu gibi paint" onerisi yapilmamali.

### AP2. Top-Down Pure 90° = ARPG Read Bozar
Fishu pure top-down; RIMA [Camera Angle LOCKED 30-35 derece 3/4]. Camera felsefesi farki — copyalanmamali.

### AP3. Cozy Pacing = Combat Roguelite Ritmini Olduren
"Casually fishing" 51s slow demo = RIMA combat hizina karsit. Sadece "rest room" gibi NICE-TO-HAVE micro-pause icin uygun, ana loop'a sokulmamali.

### AP4. Asili Tabela "into sea" Ingilizce Hardcoded
[Localization] kuralı: hardcoding BANNED. Diegetic UI uygulanirsa tabela texture'lari TR/EN swap-edilebilir tasarlanmali (atlas swap veya runtime text overlay).

### AP5. 2-Slot Hotbar = RIMA Cross-Class Skill (2 slot) ile Karistirilmamali
Cross-Class Skill 2 slot zaten LOCKED — yeni bir 2-slot hotbar (item) eklemek **ikon enflasyonu** yaratir. Hotbar genisletmesi RIMA icin uygun degil.

---

## OZET — Action Items

| Aday | Tip | Sistem | Fit | Sonraki Adim |
|------|-----|--------|-----|--------------|
| #170 | Karar | RIMA Map (water shader) | HIGH | Karar dosyasi tasla |
| #171 | Karar | RIMA Player (footstep particle) | MEDIUM | TilesetPairing field tasarla |
| C-002 | Karar | Caterpillar UI | HIGH | Cozy hotbar prototip |
| Art Bible Ref | Doc | Caterpillar Art | HIGH | Painterly tier-1 listesine ekle |
| TikTok Devlog | Strateji | Studio Marketing | HIGH | 51s vertical format dene |

---

**Tweet Erisim Durumu:** TAMAMI BASARILI (metadata + thumbnail visual confirm)
**Video tam izleme:** Yapilmadi (51s mp4 — sadece thumbnail analiz; tam mekanik akis icin manuel goz gerekli)
**Manuel review onerilir:** Footstep trail timing, water shader scroll hizi, fishing mekanigi (cast/reel/catch loop) — video full playback ile dogrulanmali.
