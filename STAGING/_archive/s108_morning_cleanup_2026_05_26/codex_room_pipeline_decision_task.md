ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — RIMA Room/Dungeon Visual Pipeline DECISION

**Amaç:** ChatGPT'nin önerdiği "Fractured Chamber / semi-wall illusion" yaklaşımı + 4 örnek oda render'ı + 2 spec sheet'i Codex perspektifinden değerlendir. Production-safe yön için bağımsız bir 9-başlıklı yazılı görüş üret. Claude (orchestrator) bu çıktıyı kendi görüşüyle sentezleyip son kararı verecek — son sözü Codex'in değil, Claude'un olacak.

## Senin işin
Aşağıdaki ChatGPT mesajını + 6 görseli incele, Çıktı Formatı bölümündeki 9 başlığa madde madde cevap yaz. Kod YAZMA. Sadece teknik karar destek + risk analizi + üretim sırası önerisi. Cevabını `STAGING/codex_room_pipeline_decision_response.md` olarak kaydet.

## İnceleyeceğin görseller (sırayla aç ve hepsini gör)

1. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_11_22.png`
   — Spec sheet TR: "Duvar yerine kullanılabilecek farklı yöntemler" (6 alternatif: Fractured Room Island, Backwall Landmark, Prop Wall, Fog/Darkness Boundary, Rift Portal Geçişleri, Arena/Stage Diorama) + modüler asset listesi + oda tipleri.

2. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_29_22 (1).png`
   — Combat room örneği: ~14 karakter, fractured granite floor island, north backwall arch + rift center, amber torches, cyan rift cracks zeminde + duvarlarda, void boundary.

3. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_29_23 (2).png`
   — Ritual chamber: merkezde devasa cyan rift portal, semi-circle backwall, oturmuş ritual aktörü, mage cast halkası, daha az karakter, daha "boss room" tonu.

4. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_29_23 (3).png`
   — Mixed combat room: ~20 karakter, mage telegraph, sarcophagus prop'u, vampire boss üst sağda, geçiş arch'ları her yönde — fractured island variant.

5. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_29_23 (4).png`
   — Boss/elite combat: kırmızı banner backwall, scattered combatants, cyan crack web zeminde, daha "ruined fort" hissi, granite blok prop'lar merkezde.

6. `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_29_29.png`
   — RIMA-özel oda tipi spec sheet TR: 9 oda tipi (Battered Hall, Rift Gate Chamber, Prison Hold, Ritual Chamber, Library Archive, Flooded Crypt, Elite Arena, Transition Corridor) + PixelLab "neden full duvar zor" reality check + modüler backwall sistemi (7 parça: Left End, Mid Broken, Stone Cover, Boss Gate, Stone Cover, Mid Torch, Right End) + birleştirme örneği + asset set + oda kurulum mantığı (5 layer: BG/Prop/Edge/Floor/Deco+Light).

## Ek konteks — ChatGPT'ye user'ın orijinal mesajı (TAMAMI, kısaltmadan)

[ChatGPT'ye gönderilen orijinal mesaj — Codex bu konteksti tam bilsin]

---
RIMA ROOM / DUNGEON VISUAL PIPELINE DEĞERLENDİRME

Bağlam:
RIMA, dark fantasy 2D pixel-art roguelite ARPG.
Dünya: "Shattered Keep" / "Fracturing" sonrası parçalanmış taş yapılar, cyan rift çatlakları, amber torch/candle ışıkları, karanlık dungeon atmosferi.
Hedef: room-by-room ilerleyen, combat okunurluğu güçlü, büyük ama modüler kurulabilen odalar.
Karakterler: ekte verdiğim 10 sınıf sprite'ı. Bunlar stil, ölçek, okunurluk ve combat yerleşimi için referans. Karakterlerin stilini bozmak istemiyorum.

EKLER / NEYE BAKMAN GEREKİYOR
1. Character sprites:
- Bunlar oyuncu sınıfları / combat aktörleri.
- Oda ölçeği, yürünebilir alan, savaş yoğunluğu ve okunurluk için referans.
- Bunları "oda içinde ne kadar küçük / büyük görünmeli" sorusu için değerlendir.

2. Oda konsept görselleri:
- Bunlar hedef ambiyansa yaklaşan örnekler.
- Bazıları boss room, bazıları büyük combat room, bazıları flooded/prison/library gibi varyasyonlar.
- Bunları analiz edip hangi yönün daha production-safe olduğunu değerlendir.

3. GDD / oyun bağlamı:
- RIMA'nın atmosferi, mücadele yapısı, class fantasy, rift teması ve run yapısı için kullan.

ANA PROBLEM
Ben referanslardaki gibi büyük, karanlık, isometric/fake-isometric hissi veren dungeon odaları istiyorum.
Ama özellikle şu noktada takılıyorum:
- tam modüler yüksek duvar sistemi kurmak zor
- PixelLab'da duvarı üretip sonra birleştirmek zor
- north/west wall, köşe birleşimleri, kapı açıklıkları ve seam'ler sorun oluyor
- 512x512 üretim sınırı yüzünden büyük odaları tek parça üretmek mantıklı değil
- full 3D shell + 2D sprite hibriti mümkün ama üretim yükü artıyor
- daha kolay ama güçlü görünen bir sistem arıyorum

ÖNEMLİ GÖZLEM
Benim asıl hedefim "mükemmel teknik duvar sistemi" değil.
Asıl hedefim:
- referanslardaki gibi güçlü dungeon ambience
- büyük combat alanları
- modüler room-by-room ilerleme
- farklı tema odaları
- PixelLab + Unity ile gerçekten üretilebilir bir pipeline

BUGÜNE KADAR GELEN FİKİRLERİN ÖZETİ

1. Full modular high wall system
Klasik yaklaşım:
- north/back wall
- west/left wall
- corners
- doors
- wall caps
- full room shell

Sorunları:
- PixelLab'da kusursuz modüler bağlamak zor
- perspective / angle / seam sorunları
- her modülün aynı stil ve hizaya oturması zor
- production cost yüksek

2. 2.5D hybrid system
- Unity'de 3D room shell
- 2D sprites for characters/enemies
- pixel-art textures on 3D meshes
- orthographic camera

Artıları:
- geometry/collision daha kontrollü olabilir
- duvar formu daha kolay çözülebilir

Eksileri:
- sorting/occlusion/lighting mismatch/pixel-perfect riski
- pipeline daha kompleks
- benim şu anki üretim kolaylığı hedefime ters olabilir

3. Full 2D fractured chamber / semi-wall illusion system
Bu şu an en mantıklı aday gibi duruyor:
- büyük taş zemin adası / floor island
- dışarıda black void / dark void / rift darkness
- düşük kırık taş kenarlar / low broken edges
- sadece üstte veya üst+solda decorative backwall / landmark cards
- props ile sınır tanımlama
- cyan rift cracks + amber lights + rubble + pillars ile ambiyans
- merkezde temiz combat alanı

Bu sistemin mantığı:
"Tam duvarlı oda" yerine
"Fracturing sonrası kırılmış combat chamber" hissi vermek.

Bence bu fikir RIMA lore'una çok uyuyor:
- kusursuz birleşmeyen duvarlar sorun değil, tasarım dili olur
- eksik/kırık/geçişli yapı worldbuilding'e dönüşür
- duvar problemi tasarımsal avantaja çevrilebilir

NEDEN PIXELLAB'DA TAM DUVARI YAPMAK ZOR?
Bunu özellikle değerlendirmeni istiyorum.

Benim gördüğüm sorunlar:
1. Create Image Pro çoğu zaman "full room / poster" üretmeye kayıyor
2. Modüler bağlanacak duvar şeridi üretmek zor
3. Her modül aynı alt hizaya / perspektife gelmeyebiliyor
4. left / right edge uyumsuz olabiliyor
5. tek parça duvar büyütünce pixel art bozuluyor
6. 512x512 limiti yüzünden büyük boss room wall'ı tek asset olarak üretmek verimsiz

Dolayısıyla önerilen çözüm:
PixelLab'a büyük oda değil, küçük modüler parçalar üretmek:
- floor pieces
- low edge pieces
- backwall cards
- landmark pieces
- seam covers
- prop boundary pieces
- decals / rift cracks / torch glows

ÖNERİLEN RIMA GÖRSEL DİLİ
Bunu değerlendir ve gerekiyorsa geliştir:

RIMA room formula:
- fractured dark granite floor island
- broken low stone edges
- black/rift void around room
- cyan rift cracks
- amber torch/candle light
- optional top/north landmark backwall
- optional left-side structural card
- props concentrated near edges
- clean center for combat
- boss room / prison / library / ritual / flooded crypt tema varyasyonları

RIMA duvar kimliği:
- clean perfect wall değil
- fractured granite masonry
- cyan cracks between stones
- missing chunks / void leaks
- broken arches / damaged gates
- torch highlights
- seamleri pillar / rubble / banner / torch / crack ile gizleme
- "perfect tiling" yerine "intentional broken composition"

BASTION REFERANSI
Bastion'u da araştırmanı istiyorum.
Sebep:
- tam duvarlı klasik iç mekan zorunlu değil
- fractured / floating / staged combat areas mantığı var
- alan hissi duvar olmadan da verilebiliyor
- sahne kompozisyonu + edge language + void hissi önemli

Lütfen Bastion'dan, gerekiyorsa Hades / Curse of the Dead Gods / Children of Morta gibi örneklerden çıkarım yap:
- hangisi RIMA'ya daha yakın?
- hangi görsel prensipleri ödünç almalıyız?
- hangilerini almamalıyız?

BENDEN BEKLENEN SONUÇ
Ben senden sadece yorum değil, karar destek istiyorum.

Lütfen şu sorulara net cevap ver:

1. RIMA için ilk production-safe yön hangisi olmalı?
A) Full modular high wall 2D
B) 2.5D hybrid room shell
C) Full 2D fractured chamber / semi-wall illusion system

2. Ekteki oda görsellerine ve karakter ölçeğine bakınca en mantıklı oda boyutu / combat density nasıl olmalı?

3. PixelLab ile gerçekçi üretim sırası nasıl olmalı?

4. PixelLab'da hangi asset kategorilerini üretmeliyim?

5. Büyük odaları nasıl kurmalıyım?

6. RIMA'ya özgü "duvar dili" tam olarak nasıl tanımlanmalı?

7. Hangi oda tipleri ilk batch'te yapılmalı?

8. Hangi şeyleri procedural yapmalı, hangi şeyleri elde curate etmeliyim?

9. Codex / Claude Code / PixelLab / Unity iş bölümü nasıl olmalı?

10. MVP planı nasıl olmalı?

Ana hedef:
Ben duvar sisteminde boğulmadan, RIMA'ya özgü, büyük ve atmosferik combat odaları üretebileyim.
Yani amaç "perfect wall tech" değil; "strong room ambience + production-safe modular pipeline".
---

## RIMA proje konteksti (kısa)
- 2D pixel-art top-down ARPG roguelite (Karar #114 LOCKED: 8-dir, 5 sprite üret 3 mirror)
- Karakterler 64×64 chibi, PPU 64, 10-12 fps anim
- URP 2D Renderer + Pixel Perfect Camera + 2D Lights
- Tile 32×32 top-down, near-pure top-down (~85-90° from horizon, Diablo / Children of Morta angle)
- S102 Architecture LOCKED: Hybrid Template + Decor Overlay (Opus+Codex consensus, 2026-05-23)
- S101 Wall production: PixelLab 2x2 grid sheets, PILLAR-LESS + overlay decor, NO baked torches/banners
- S102 yan track: Modular Wall Shell MVP (ChatGPT'nin N/W wall family separation, 4×4 sheet = 16 piece) — bekleyen, henüz üretilmedi
- Local Flux LoRA training: ai-toolkit + PixelWave base + 335 dataset, PAUSED at step ~30
- Studio-wide procgen lock: MASTER Poisson + Dual Grid, AVOID WFC default

## Çıktı Formatı (9 başlık zorunlu — kendi yazını `STAGING/codex_room_pipeline_decision_response.md`'ye kaydet)

```
# Codex Görüşü — RIMA Room Pipeline Karar Desteği

## 1. Executive decision
- Yön: A / B / C / hibrit — net seçim + 2-3 cümle gerekçe
- Confidence: low / medium / high

## 2. PixelLab reality check
- Tam duvar neden zor (somut, deneyime dayalı)
- Hangi asset yaklaşımı daha doğru (modüler parça vs full sheet)
- Önerilen sheet boyutu / cell sayısı / yön ayrımı

## 3. Best visual system for RIMA
- Net tanım: "RIMA odası = X + Y + Z" formülü
- Hangi katmanlar zorunlu, hangileri opsiyonel
- ChatGPT'nin "fractured chamber" tanımına ekleyeceğin / çıkaracağın madde var mı

## 4. First asset batch
- Öncelik sırasıyla 10-15 asset listesi (kategori + isim)
- Hangileri PixelLab, hangileri reuse, hangileri overlay
- Üretim sırası mantığı (önce ne, neden o)

## 5. Unity assembly approach
- Room nasıl kurulmalı: prefab shell / Tilemap / Scene / hibrit
- Mevcut Hybrid Template+Decor scaffolding ile uyumlu mu (Assets/Scripts/Rooms/RoomTemplate.cs)
- 5-layer rendering order (BG/Prop/Edge/Floor/Deco) — sorting layer önerisi
- Camera + lighting setup notları

## 6. Recommended first room archetypes
- İlk batch'te yapılacak 3-5 oda tipi (sırayla)
- Her birinin neden seçildiği (mekanik test değeri + asset reuse oranı)
- Estimated asset count per room

## 7. Bastion / other reference takeaways
- Bastion ne öğretiyor (somut prensip)
- Hades / Children of Morta / Curse of the Dead Gods karşılaştırması
- RIMA için hangisi en yakın referans, hangilerinden uzak dur

## 8. Risks and mitigation
- Top 5 risk (production, görsel okunurluk, scope, pipeline, asset drift)
- Her birine 1 cümlelik mitigation

## 9. MVP plan
- Step-by-step, 5-10 adım, her adımın çıktısı + sahibi (PixelLab/Codex/Claude/Unity/user)
- Hangi adım blocker, hangi adım paralel olabilir
- "MVP done" tanımı (kabul kriteri)
```

## Notlar
- Cevabını TÜRKÇE yaz (user TR konuşuyor)
- Görselleri açıklarken hangi görseli kastettiğini belirt (örn: "21_29_22 (1) combat room'da...")
- Karakter ölçeği için 64×64 chibi sprite olduğunu unut ETME — örnek render'lardaki karakterler RIMA stiline yakın ama ölçek hesabı RIMA'nın 64×64 PPU 64 setup'ı üzerinden olmalı
- "Şu an yapılan modüler wall shell MVP (4×4 sheet, 16 piece)" yan track'i ile fractured chamber yaklaşımı çakışıyor mu — bunu özellikle değerlendir, ikisi co-exist edebilir mi?
- Konuşma değil, karar destek. Madde madde, net önerme, BLOCKED yazacaksan sebebini söyle.
