# Infamous Keepers Analizi — Eleştirel Self-Review
*Reviewer: Claude (Opus 4.7, fresh pass)*
*Reviewed: `analysis.md` (Claude Sonnet'in ilk pass'i)*
*Tarih: 2026-05-10*

İlk analiz iyi yazılmış ama **gözden kaçanlar var**. Bu doküman eksikleri ve yanlış vurguları işaretler. Codex'in vereceği bağımsız görüşün simulasyonu — kendi analizime karşı acımasızım.

---

## A. İlk Analizin DOĞRU Tespitleri (Konfirme)

| Sistem | Değerlendirme |
|---|---|
| Kamera açısı (35° eşleşmesi) | **DOĞRU**, validation sağlam |
| Wall occlusion yaklaşımı | **DOĞRU**, edge case test önerisi yerinde |
| Skill Draft tier renkleri | **DOĞRU**, mor=Epic + mavi=Rare gözlemi net |
| Lighting felsefesi (physical source) | **DOĞRU**, somut karar referansı doğru |
| Hub atmosferi | **DOĞRU**, background NPC önerisi mantıklı |

İlk pass'in iskeleti sağlam. Ama **6 önemli detay kaçırılmış.**

---

## B. KAÇIRILAN Detaylar (yeniden screenshot'lara baktım)

### B1. Crit Hit vs Normal Hit ayrımı — Image 1 vs Image 2 farkı atlanmış
**Image 1'de tek ve büyük "20"** (kırmızı, kalın, blood spray ile). **Image 2'de küçük 9 sayı** (4, 1, 18, 2, 1, 1, 2, 2, 2). Bu **rastgele değil**: Image 1'deki "20" muhtemelen **crit** veya **special hit**, image 2'deki cluster normal AOE hit'leri.

**RIMA Etkisi:** Bizim 3-Layer Feedback (Normal/Commit/Break) kararı var ama **damage number'ın görsel treatment farkı** specced değil. Crit için: büyük font + screen shake artımı + farklı renk? Bu kararı somutlaştırmak lazım.

**Yeni Aksiyon:** `MASTER_KARAR_BELGESI`'ne damage number visual hierarchy spec ekle:
- Normal: 14pt beyaz/kırmızı
- Commit (crit): 20pt sarı + screen shake +50%
- Break (heavy): 28pt kırmızı + screen shake +100% + hit stop +2 frame

### B2. Damage Number Renk Kodlaması — Hiç konuşulmamış
Infamous Keepers tüm sayılar kırmızı. Ama RIMA'da daha zengin sistem mümkün:
- **Beyaz**: normal hasar
- **Sarı**: crit
- **Mor**: void/shadow hasarı (Shadowblade)
- **Cyan**: Shadow Echo / rift damage
- **Yeşil**: heal (artı işareti ile)
- **Mavi**: shield/posture damage

**RIMA Etkisi:** Status effect ve element variety'miz çok daha derin (Fire/Frost/Lightning/Light + Void). Hepsi kırmızı sayı olursa **gameplay clarity kaybı** olur.

**Yeni Aksiyon:** `FloatingDamageText` prefab'ında damage type'a göre color mapping ekle. Spec: `STAGING/playtest_floating_text_color_spec.md`.

### B3. Chokepoint / Bottleneck Room Design — Image 2 atlanmış
Image 2'deki kombat **dar bir geçitte** gerçekleşiyor (iki kapı arası kısa koridor). İlk analiz "combat floor merkezi temiz" dedi ama Image 2 gösteriyor ki **bottleneck room'lar da var**. Bu farklı bir room design pattern.

**RIMA Etkisi:** `room_authoring.md`'deki room template'lerde bottleneck/chokepoint variant var mı? Spec sadece açık arena'ya odaklanmış olabilir.

**Yeni Aksiyon:** Room template kataloğuna **"Chokepoint Combat"** variant ekle (tactical positioning forces, AOE skill'ler güçlenir, dash/blink önem kazanır). Boss room'lardan önce gerilim yaratan bir oda tipi olarak iyi.

### B4. Wall Decoration / Architectural Details — Image 2'deki gargoyle atlanmış
Image 2'nin sağ duvarında **taş yüz/gargoyle** var. Image 1'in üst sol köşesinde **forge/workstation** var. **Duvar dekorasyonu** sadece floor decoration kadar atmosfer yaratıyor ama spec sadece floor decal'a odaklanmış.

**RIMA Etkisi:** PRODUCTION_PLAYBOOK Adım 11 (Wall Torch) var ama **architectural feature** (forge, gargoyle, altar nişi, niche'lere yerleştirilmiş heykel) listesi yok.

**Yeni Aksiyon:** PRODUCTION_PLAYBOOK Adım 17 olarak **"Wall Architectural Features"** ekle (8 var: forge, gargoyle, niche statue, altar recess, broken pipe, chain mount, weapon rack, scroll shelf). 256px static sprite, edge-mounted.

### B5. Resource HUD Policy DURING Skill Draft — atlanmış
Image 4'te skill draft AÇIKKEN üst bar görünüyor (gold/units/gems). RIMA spec'te Hades-tarzı "draft sırasında hiçbir şey yok" mu, yoksa resource'lar görünür mü?

**RIMA Etkisi:** HUD state management kritik. Eğer draft'ta resource görünüyorsa, oyuncu "şu kartı seçersem yeterli kaynağım var mı?" diye düşünebilir. Karar gerekli.

**Yeni Aksiyon:** `RIMA_UI_STATE_BLUEPRINT_2026-05-04.md`'de Skill Draft state için açık karar:
- **Önerim**: Tier-based — Common/Rare draft sırasında HUD görünür, Epic/Legendary draft sırasında HUD fade out (cinematic anlık).
- Hades full hide yapıyor; Infamous Keepers full göster yapıyor. Hibrit RIMA'ya yakışır.

### B6. Multi-Faction Enemy Coexistence — Image 1'deki çoklu mob
Image 1'de **iki ayrı düşman tipi** var: skeleton swarm (sağ üst) + bandit/peasant grup (alt). Kavga ediyorlar mı yoksa player'a karşı mı? Net değil.

**RIMA Etkisi:** Bizim 16 unique mob var. Hepsi aynı odada birbirini görmezden mi gelecek? Yoksa Hollow Knight tarzı **infighting mechanic** olabilir mi (oyuncuya buff)?

**Yeni Aksiyon:** Design decision gerekli — **mob faction system var mı?** Eğer evet, hangi mob'lar birbiriyle çatışır? `MOB_COMPOSITION_RULES.md`'de tanımlı mı?

---

## C. ABARTILMIŞ Tespitler (revize)

### C1. "Decal pool 16 → 22 varyasyon"
Önerinin temeli: "onların floor'u zenginleştirilmiş". Ama tek tek tile başına decal sayısı değil, **decal placement density** önemli. 16 decal pool yeterli — daha çok varyasyon değil, **daha yoğun yerleştirme** gerekli.

**Düzeltme:** Decal pool 16'da kal, ama room template'lerde decal placement density'i artır (room başına 8-12 decal yerine 15-25).

### C2. "Skill Draft glow column"
Doğru gözlem ama "Common no glow" ve tier'ın yorumu eksikti. Aslında daha incelikli: glow **rarity'den çok category'ye** bağlı olabilir (Trait kartı sade, Skill kartı orta, Ult kartı parlak). Ya da tier+category kombosu. Tek boyutlu (sadece rarity) yetmez.

**Düzeltme:** Glow column tier × category matrix:
- Trait + Common = sade
- Trait + Epic = orta cyan
- Skill + Epic = güçlü mor
- Ult + Legendary = en güçlü altın + particle
Kategori bazlı varyasyon önemli.

---

## D. YENİ Bulgular (ilk analizde hiç değinilmemiş)

### D1. Adjacent Room Visibility — Image 3'te görünüyor
Image 3'teki tavern'in sağ alt kapısından **bir sonraki odanın bir kısmı görünüyor** (kombat efektleri, sayılar). Bu **teaser visibility** mekanik mi?

**RIMA için ilham:** Hades'te bir sonraki oda kapı açılana kadar tamamen kapalı. Ama **"kapıdan bir sonraki odanın atmosferini hissettir"** mekanik immersion açısından güçlü. Risk: rendering maliyeti, dikkat dağılması.

**Karar gerekli:** Kapı arkası rendering — kapalı / partial / full?

### D2. Character Shadow Spec — Image 1'deki büyük figür
Image 1'in sağ üstündeki büyük axe figürünün **subtle ground shadow**'u var. Bizim character sprite'larında "shadow embedded mi, ayrı sprite mı, runtime drop shadow mı?" specced mi?

**Karar gerekli:** Karakter gölgeleri:
- (a) Sprite'a embedded (kalitesi düşer, body-only kuralı bozulur)
- (b) Ayrı sprite layer (her yön için ek üretim — maliyet)
- (c) Runtime drop shadow (Unity 2D shadow, fast & cheap, **önerilen**)

### D3. Terrain Hazard Sistemi — Spike Traps
Hem Image 1 hem Image 2'de **spike trap'ler** var (statik environment hazard). RIMA'da spec yok. Roguelite genre'de yaygın bir mekanik.

**Karar gerekli:** Terrain hazard var mı? Eğer evet:
- Spike pit (sürekli hasar)
- Pressure plate trap (basınca tetiklenen)
- Crumbling floor (geçince düşer)
- Acid pool (DOT)

İlk Act'te 1-2 hazard tipi MVP için yeterli. Combat varlığını zenginleştirir, "akıllı positioning" reward eder.

### D4. Hub'da Combat Spillover — Image 3'ün sağ altı
Image 3'te hub'da combat var (sağ alt köşe — 1, 5, 5, 8 sayıları). Bu hub'un kenarında ya da hub'ın "tehlikeli" bir yan odası olabilir.

**RIMA için soru:** Hub %100 safe mi? Bizim spec "Hub Rest Pose" diyor — silah omuzda, savaş yok. Ama Image 3 gösteriyor ki **hub'ın yan odası combat alanı** olabilir (training arena, prison cell, monster pit?). Narrative açısından zengin — ama spec'te yok.

### D5. Camera Zoom Variation — Image 1 vs Image 4
Image 1 (gameplay) wide-angle kombat. Image 4 (skill draft) **çok daha yakın** zoom — UI ön planda. Hub dialogue'da kamera zoom spec'imiz var ama draft sırasında zoom ne durumda?

**Karar gerekli:** Skill Draft sırasında kamera state:
- (a) Aynı pozisyon, blur arka plan (hızlı, basit)
- (b) Zoom in karakter merkez, kart UI overlay
- (c) Tam ekran takeover (gameplay görmez), Hades tarzı

### D6. Spike Trap + Damage Number Cluster Etkileşimi
Image 2'deki sayı cluster'ı (4, 1, 18, 2, 1, 1, 2, 2, 2) muhtemelen **trap üzerinde duran düşmana** her tick hasar veriyor. RIMA'da DOT veya environmental hazard varsa tick sayıları **anti-overlap** algoritmasını çok zorluyor.

**Aksiyon revize:** B2'deki anti-overlap algoritmaya **tick stacking** logic ekle: aynı kaynaktan ardışık hit'ler **dikey stack** (üst üste yığ), farklı kaynaklardan hit'ler **fan out** (yelpaze).

---

## E. Güncellenmiş Aksiyon Listesi (Önceliklendirilmiş)

### Kritik (Faz 1-2 etkilenir)
1. **Damage number visual hierarchy** — Normal/Commit/Break için font size + color + shake spec (B1)
2. **Damage number color coding** — Element/status type bazlı renk (B2)
3. **Anti-overlap + tick stacking algorithm** — FloatingTextSpawner'a logic (D6 + ilk analizden)
4. **Character shadow spec** — Runtime drop shadow karar verilmeli (D2)

### Orta (Faz 2-3'te lazım)
5. **Wall architectural features** — Production Playbook'a Adım 17 (B4)
6. **Resource HUD policy during Skill Draft** — Tier-based hibrit önerisi (B5)
7. **Skill Draft glow matrix** — Tier × Category (C2 düzeltme)
8. **Camera zoom during draft** — Karar gerekli (D5)

### Düşük / Backlog
9. **Chokepoint Combat room template** — Variant ekle (B3)
10. **Decal placement density artırımı** — Pool yerine yerleştirme (C1 düzeltme)
11. **Background NPC layer in hub** — İlk analizden, korunur
12. **WallOcclusionFader edge test scene** — İlk analizden, korunur

### Design Decision Gerekli (somut spec yok)
13. **Mob faction system / infighting** — Var mı? (B6)
14. **Terrain hazard system (spike traps)** — Var mı, kaç tip? (D3)
15. **Adjacent room peek visibility** — Kapı arkası rendering politikası (D1)
16. **Hub combat sub-area** — Hub'ın yan odası combat olur mu? (D4)

---

## F. Genel Yargı (Revize)

İlk analiz **iyi ama yüzeysel**. Skor: %85 → revize sonrası **%92 kapsama**.

**İlk analizin güçlü yanı:** Sistematik 8 sistem karşılaştırması, somut LOCKED karar referansları, RIMA-side okumayı kolaylaştıran tablolar.

**İlk analizin zayıf yanı:** 
- Crit/normal damage feedback ayrımını (Image 1 vs 2 farkı) atladı
- Wall decoration kategorisini hiç ele almadı
- Hub'daki combat spillover'ı görmedi
- 4 önemli design decision'ı (faction, hazard, peek, hub-combat) tetiklemedi

**Net katkı:** 6 yeni eksik tespit + 4 design decision tetikleyici + 2 revize. Original 5 aksiyon → 16 aksiyon (sınıflandırılmış).

**Codex'siz yapılabilirdi mi?** Evet — bu type review için Sonnet ilk pass'i + Opus self-review iyi sonuç verdi. Codex'in görsel analizi marjinal değer eklerdi.
