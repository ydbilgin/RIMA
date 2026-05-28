# Antigravity Research: Wall-less Top-Down Game Design

ACTIVE RULES: (1) think before answering (2) min words, no fluff (3) cite industry examples (4) BLOCKED if claim unsubstantiated.

Amaç: User RIMA 2D top-down ARPG/roguelite için "duvarsız oyun" (wall-less) yapma fikrini değerlendiriyor. Sadece tile zemin + harita objeleri (kolon, prop, ışık, uçurum, su, mobilya) boundary tanımlayacak. Industry'de bu hangi tekniklerle yapılıyor, hangi oyunlar referans, RIMA'ya hangisi uyar — research-heavy verdict ver.

RESPOND INLINE — do NOT write to file. ConPTY captures stdout.

## RIMA bağlamı (tek paragraf)
- 2D top-down ARPG/roguelite, 35° iso tile floor (PixelLab pixel art, PPU 64)
- Karakter: 120×120 chibi pixel art, 8-yön
- Mevcut wall sistemi: 14 placeholder prefab, 4 sheet PixelLab walls, V2 namespace (S104 P0 fix DONE)
- Camera: Yüksek top-down 3/4 (Hades / Children of Morta / Diablo III ref)
- Sahne: `Assets/Scenes/Test/PlayableArena.unity` — 352-cell iso floor LIVE, NO walls intentionally

## SORU (4 başlık altında inline yanıtla)

### 1. Wall-less endüstri yöntemleri (3-5 örnek oyun)
- Hangi 2D/3D top-down/iso oyunlar duvar yerine başka boundary mekanizmaları kullanmış?
- Her örnek için: oyun adı + boundary tipi (uçurum / su / engel objesi / ışık duvarı / invisible collider + visual cue) + neden işe yarıyor

### 2. Kategorize edilebilir teknikler (taxonomy)
- **Negative space** (zemin yokluğu — void/uçurum)
- **Object barriers** (kolon, ağaç, mobilya, taş bloğu kümeleri)
- **Elevation** (yüksek/alçak tile + 1-tile height difference)
- **Liquid boundaries** (su, lav, asit)
- **Light/dark** (karanlık alanlar erişilemez)
- **Soft barriers** (mob spawn, oyuncu yön hissi ile gerileme)
- Hibrit kombinasyonlar — hangi oyun hangi karışımı kullanıyor

### 3. Avantaj / Dezavantaj çözümlemesi (RIMA için)
- Asset cost: Wall production vs object production
- Readability: Player wall sınırını okuyor mu yoksa "buradan gidemem" sezgisi var mı?
- Combat: Walls knockback/cover sağlar — duvarsız bunun yerine ne?
- Procgen: Object-based boundary procgen daha kolay/zor mu?
- Visual fidelity: Top-down 3/4 perspektifte duvar zaten yarı görünür — wall-less + iyi object density "premium" mu görünür?

### 4. RIMA özel öneri (verdict)
- Saf wall-less mı, hybrid mı (örn. dış sınır duvar + iç boundary object), yoksa walls'a geri dön mü?
- Eğer wall-less öneriyorsan: ilk 3 prop tipi (öncelik sırası) + density target (cells/object) + boundary readability cue (ışık, gölge, perspective)
- Hangi industry oyun en yakın referans (1 tane seç)
- Risk: Wall-less'ın RIMA'da fail edebileceği 2 senaryo

## Format kısıtlaması
- Toplam 600-800 kelime (Türkçe veya İngilizce, fark etmez)
- Inline structured output — başlık + bullet
- "[KAYNAK]" işareti ile her major claim'i etiketle (ör. "[KAYNAK: Hades Steam dev blog]" veya "[KAYNAK: gözlem, oyun screenshot'tan]")
- Spekülasyon varsa "[SPECULATIVE]" ile belirt
- Verdict net olsun — "şu yöne git" deyim, kararsız bırakma
