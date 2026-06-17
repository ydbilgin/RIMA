# RIMA — Rift Avcıları

## Bitirme Projesi Sunum Raporu · 18 Haziran 2026

Bir oyun değil; bir environment + vertical slice + oyun-içi geliştirici araç seti.

---

## 1. RIMA Nedir? (Tez)

RIMA, ilk bakışta 2D izometrik bir aksiyon-roguelite oyunu gibi görünür. Ancak bu projenin asıl iddiası, sadece bir oyun olması **değildir**. RIMA, üç katmanın birleşimidir: oynanabilir bir **environment** (yaşayan, gezilebilir bir dünya), uçtan uca çalışan bir **vertical slice** (MainMenu'den boss karşılaşmasına kadar kesintisiz bir döngü) ve en önemlisi, **yeniden-kullanılabilir oyun-içi geliştirici araç seti** (oyun çalışırken seviye tasarlamayı, zorluk ayarlamayı ve telemetri toplamayı mümkün kılan editör katmanı).

Bu ayrım, projenin nasıl değerlendirilmesi gerektiğini de belirler. Değerlendirme ekseni içerik hacmine değil, sistem mimarisine ve mühendislik disiplinine dayanır:

| Eksen | Ağırlık | Ne anlatıyor |
|---|---|---|
| Oyun (içerik/oynanış) | ~%20 | Vertical slice çalışıyor, ama asıl mesele bu değil |
| Mimari (sistem tasarımı + tooling) | ~%60 | Veri-güdümlü oda sistemi, oyun-içi editör, ölçeklenebilirlik |
| Graphify-audit (veri-destekli kanıt) | ~%20 | Mimari iddianın kod-grafı metrikleriyle ispatı |

Bu tez bir slogan değildir; veriyle desteklenir. Aşağıdaki god-node (en bağlı kod düğümleri) figürü bir teaser'dır: en bağlı node'ların **yaklaşık 6/10'u bir editör/tool sınıfıdır**. Yani projenin ağırlık merkezi, oynanış kodu değil, geliştirme araçlarıdır. Bunun sayısal kanıtı Bölüm 6'dadır.

![Graphify god-node analizi — en bağlı node'ların 6/10'u editör/tool sınıfı](figures_2026-06-18/fig_graphify_godnodes.png)

*Şekil 1: Graphify god-node analizi. En bağlı kod düğümlerinin çoğunluğu tooling katmanıdır — "bu bir environment + tooling" tezinin sayısal kanıtı (detay Bölüm 6).*

---

## 2. Sunumda Ne Göstermeli — Demo Akışı (Run-of-show)

Aşağıdaki adımlar sunum sırasında ekranda gösterilecek akıştır. Her adımda "ne yapılacak + hangi tuş + ne anlatılacak" net biçimde verilmiştir.

1. **AÇILIŞ:** Graphify god-node görseli → "bu bir environment+tooling, veriyle".
2. MainMenu → BAŞLA → karakter seç (**Warblade**).
3. **Combat:** bir oda — hareket, LMB combo, Q/E/R/F skiller, wave temizle. Juice'u vurgula (hasar sayısı, hit-stop, ekran sarsıntısı).
4. **Boss:** telegraph'ları göster — kırmızı tehlike dairesi + **yeşil güvenli-halka**; telegraph bitince hasar; can barı düşüşü.
5. **Reward → Draft:** kart seç (build çeşitliliği anlat).
6. **Run-map:** branching oda ilerleme (per-run seed, Merchant/Elite).
7. ⭐ **CENTERPIECE — Edit-to-Play:** `F2` Build Mode → prop koy/oda düzenle → çık → AYNI odayı oyna. "Unity editörü açmadan, oyun çalışırken seviye tasarımı."
8. **Director Mode (`):** stat slider ile canlı zorluk ayarı / spawn / telemetry.
9. **KAPANIŞ:** graphify + AI-destekli süreç (council/cx/ax dispatch) → "geliştirme sürecini de mühendislik problemi olarak ele aldım."

---

## 3. Çalışan Çekirdek Sistemler (Vertical Slice Kanıtı)

RIMA'nın vertical slice'ı laf değildir; MainMenu'den boss karşılaşmasına uzanan tam döngü çalışır durumdadır. Aşağıdaki sistemlerin her biri kod-teyitli ve runtime-doğrulanmıştır. Önemli bir not: bu oturumda "combat çalışıyor" iddiası mühendislik disipliniyle test edilmiş, bir bug keşfedilip kök-nedenine indirilerek düzeltilmiştir (detay Bölüm 5) — bu rapor onu gizlemez, tam tersine güçlü mühendislik anlatısının parçası yapar.

| Sistem | Ne yapar |
|---|---|
| Combat loop (Warblade) | Hareket + LMB combo + Q/E/R/F skiller; stat → hasar matematiği (LMB lineer ölçekler) |
| Düşman AI | Algıla → kovala → saldır döngüsü; oda başına spawn dalgaları |
| Boss | 3 fazlı karşılaşma + telegraph'lı saldırılar + çalışan can barı |
| Reward → Draft | Dalga sonu 3-kart skill seçimi → build çeşitliliği |
| Branching run-map | Per-run seed ile dallanan oda ilerlemesi (Merchant / Elite düğümleri) |

Combat artık net bir "vuruyor" hissi verir: hasar sayıları, hit-stop ve ekran sarsıntısı bir araya gelerek geri bildirimi güçlendirir. HUD bu oturumda modern bir sol-alt yerleşime taşınmıştır.

![Oynanış + yeni HUD yerleşimi](figures_2026-06-18/fig_gameplay_hud.png)

*Şekil 2: Combat oynanışı ve yeniden tasarlanan sol-alt HUD. Hareket, combo ve skill geri bildirimi (juice) bir arada.*

![Reward → Draft kart seçimi](figures_2026-06-18/fig_draft_reward.png)

*Şekil 3: Reward → Draft ekranı. Oyuncu dalga sonunda üç karttan birini seçerek build'ini şekillendirir.*

---

## 4. Oyun-İçi Tooling — CENTERPIECE

Bu projenin en ayırt edici katmanı, oyunun içine gömülü geliştirici araçlarıdır. Unity editörünü açıp kapatmadan, oyun çalışırken seviye tasarlamayı ve sistemleri ayarlamayı mümkün kılarlar.

### Build Mode (F2) — Edit-to-Play

Build Mode, çalışan oyunun içinde bir seviye editörüdür. `F2` ile açılır; prop ve tile paletinden seçim yapılır, izometrik ızgaraya ghost-snap ile yerleştirilir, undo/redo ile düzenlenir. Palet **veri-güdümlüdür**: `BuildModeAssetCatalog`, `PropRegistrySO` veri kaynağından beslenir, yani yeni bir prop eklemek elle kodlama değil veri ekleme işidir. En kritik özellik **Edit-to-Play** döngüsüdür: oda kur → Build Mode'dan çık → **aynı odayı oyna**. Tasarım ile test arasındaki tur süresi neredeyse sıfıra iner.

![Build Mode — Edit-to-Play seviye editörü (CENTERPIECE)](figures_2026-06-18/fig_buildmode_centerpiece.png)

*Şekil 4: Build Mode (F2) — oyun çalışırken çalışan, veri-güdümlü seviye editörü. Prop/tile paleti, izometrik ızgara, ghost-snap ve undo/redo. Edit-to-Play: kur, çık, aynı odayı oyna.*

### Director Mode (`) — Runtime Tuning Aracı

Director Mode, çalışma zamanında üretilen (sahnesiz boot eden) bir UI factory'dir: stat slider'larıyla canlı zorluk ayarı, düşman spawn'ı, CSV telemetri kaydı, Build/Map erişimi ve serbest kamera. Bu oturumda eski "debug-overlay" görünümünden profesyonel bir IDE-dock layout'a yükseltilmiş, skill listesi için ScrollRect eklenmiştir (önceden taşan girişler artık kaydırılabilir).

![Director Mode — runtime tuning ve telemetri aracı](figures_2026-06-18/fig_director_mode.png)

*Şekil 5: Director Mode (`) — runtime stat tuning, spawn, telemetri (CSV) ve serbest kamera. Bu oturumda IDE-dock skin ve kaydırılabilir skill listesi.*

> **Rapor cümlesi:** "Tasarım iterasyonlarını Unity editörü açıp kapatmadan, oyun çalışırken yapan bir oyun-içi geliştirici aracı yazdım."

---

## 5. Mühendislik Süreci (Meta-Mühendislik)

RIMA'nın geliştirme süreci de bir mühendislik problemi olarak ele alınmıştır. Tek geliştirici, çok-ajanlı bir AI orkestrasyonu kurmuştur: bağımsız bakış açılarını ve adversarial doğrulamayı bir araya getiren **council** workflow'u, kod yazımı/değişikliği için **cx (Codex)** ve derin analiz/vision için **ax (Gemini/Opus)** dispatch zinciri, ve sorgulanabilir bir kod-grafı olan **graphify**. Yazar-reviewer ayrımı, karar dökümanı zorunluluğu ve her tamamlanan iş için doğrulama kanıtı gerekliliği temel ilkelerdir.

### ⭐ Combat-Bug Vaka Analizi — "Yeşil-Assert ≠ Çalışıyor"

Bu oturumun en güçlü mühendislik anlatısı bir combat bug'ının keşfi ve çözümüdür:

- **Keşif:** 35-state capture'da "combat" etiketli karelerin aslında draft/death ekranı olduğu, gerçek öldürme sayısının 0 olduğu fark edildi. Sentetik testler yeşildi ama full-flow'u atlıyordu — yani "yeşil-assert ≠ çalışıyor".
- **Kök neden (veri-güdümlü):** `BaseMobBehavior.detectionRange = 8`, spawn mesafesinden (9.12) küçüktü → düşmanlar kalıcı Idle durumuna takılıyordu (HalfThrall prefab'ında değer 6). Token-gating ise sağlamdı.
- **Fix (cerrahi):** detectionRange 8→12, prefab güncellemesi ve savunmacı bir Player re-acquire eklendi.
- **Doğrulama:** Gerçek full-flow (MainMenu → CharSelect → _Arena) ile mob'ların gerçek (tagged) oyuncuya kovalama→saldırı yaptığı, açılış dalgasının kazanılabilir olduğu (kills 0→3) ve instant-death olmadığı runtime'da kanıtlandı.

Bunun üzerine ek bir doğrulama katmanı uygulandı: bağımsız bir dış AI-review (ChatGPT, kod + screenshot) tek-arena verify'ımızın kaçırabileceği derin riskleri işaret etti. **Verify-first** ilkesiyle işaret edilen 5 riskten **2'si reproduce etmedi** (capture artefaktı + zaten iyi olan player-tag → boş işten kaçınıldı), **3'ü gerçekti ve düzeltildi**. Sonuç: otomatik test → dış AI-review → runtime-reproduce şeklinde **çok-katmanlı doğrulama** ve **yanlış-pozitif eleme** içeren olgun bir mühendislik süreci.

> **Rapor cümlesi:** "Otomatik testlerin yeşil olması yetmez; veri-güdümlü runtime doğrulama ile gerçek oynanışı kanıtladım."

---

## 6. Graphify Audit (Veri-Destekli Tez)

Mimari iddia bir his değil, ölçülebilir bir gerçektir. Graphify, projenin tüm kod tabanını sorgulanabilir bir bilgi grafına çevirir: **6925 node / 118 community**. Bu graf üzerinde yapılan **god-node analizi** (en çok bağlantıya sahip düğümler), tezin sayısal kanıtını verir: **en bağlı node'ların yaklaşık 6/10'u editör/tool sınıfıdır.** Yani projenin yapısal ağırlık merkezi, oynanış mantığı değil, tooling katmanıdır — bu da "RIMA bir tooling environment" iddiasını veriyle doğrular.

![Graphify god-node yakın plan](figures_2026-06-18/fig_graphify_godnodes.png)

*Şekil 6: God-node detayı — en yüksek bağlantı sayısına sahip düğümler. Çoğunluk tooling/editör katmanında.*

![Graphify tam kod-grafı — 6925 node / 118 community](figures_2026-06-18/fig_graphify_full.png)

*Şekil 7: Projenin tam kod-grafı (6925 node / 118 community). Topluluklar sistem sınırlarına karşılık gelir; tooling kümeleri merkezde yoğunlaşır.*

> **Not (sunum):** İnteraktif graf canlı açılabilir — `STAGING/report/graphify/graph_interactive.html`. Sunum sırasında bir node'a tıklayıp komşuluklarını göstermek tezi güçlendirir.

> **Rapor cümlesi:** "Mimari iddiamı graf-metrikleriyle destekledim — god-node'ların çoğunluğu tooling katmanı."

---

## 7. Görsel / Game-feel Polish

Az asset ile maksimum his ilkesi, projenin görsel-his katmanına yön verir. Engine-katmanı juice ve ARPG telegraph konvansiyonları, ek sanat varlığı gerektirmeden oynanışı okunur ve tatmin edici kılar.

- **CombatJuice:** hasar sayısı, hit-stop, ekran sarsıntısı ve kamera-punch — `_Arena`'ya bağlandı ve canlı doğrulandı (hit→sayı, kill→freeze, timeScale-immune, sızıntı yok). Kodlu ama bağlanmamış bir stack keşfedilip aktive edildi: "az iş, çok his".
- **Enemy readability:** gerçek silhouette outline (`EnemyReadable.shader`, herhangi bir renkte) + ambient 0.22→0.35. Düşmanlar net biçimde öne çıkar, atmosfer korunur. Gizli bir bug (her frame material'i clobber eden `EnsureVisibleSprite`) kök-nedene inilerek düzeltildi.
- **Boss telegraph:** Wrath saldırısında danger'dan ayrı bir **yeşil safe-ring** ("nereye dur" okunur), origin-snapshot (telegraph == hasar pozisyonu) ve finisher-only FlashImpact. Boss'un 6 saldırısı telegraph'a bağlandı, windup'lar bit-bit senkron — "yalan telegraph" yok.
- **Music bed:** JaggedStone CC0 dungeon-ambience (akademik açıdan lisans-net), AudioManager loop, SFX-altı seviyede.
- **Deterministik ilk-oda:** demo açılışı her seferinde aynı, tahmin edilebilir oda ile başlar.

Bu oturumun polish geçişi: prop Y-sıralaması düzeltildi (dik proplar Entities katmanına, yer-decal'ları Decals'a), silah ele mount'u iyileştirildi (anchorOffset tuning), HUD sol-alta taşındı.

![Silah ele mount — facing'e göre ön/arka katman](figures_2026-06-18/fig_weapon_mount.png)

*Şekil 8: Silah 8-yön ele mount. Karakterin baktığı yöne göre silah ön/arka katmana geçer.*

> **Rapor cümlesi:** "Az asset'le maksimum his — engine-katmanı juice + telegraph konvansiyonları."

---

## 8. Bilinen Eksikler + Yol Haritası (Dürüst)

Her eksik dürüstçe listelenir ve mümkün olduğunca bir altyapı-gerekçesine bağlanır. Önemli olan: eksiklerin çoğu içerik üretimi (kredi/zaman) kaynaklıdır, mimari değil — ve her birini hızlandıran tool/pipeline zaten yazılmıştır.

| Eksik | Durum | Neden / Plan |
|---|---|---|
| Elementalist 8-yön sprite | BLOCKED | PixelLab kredi limiti; pipeline hazır, kredi gelince üretilecek (o zamana kadar mevcut yön + flipX reuse). |
| Elementalist diğer skill VFX | yapım sırasında | Sadece Fireball SkillVfx bağlı; diğerleri SkillVfx wiring ile tamamlanacak. |
| Prop collider (sandık/fıçı walk-through) | sırada | PropColliderAutoBuilder offset taban-merkeze çekilecek (cerrahi data, refactor değil). |
| HUD HP barı rengi | iyileştirilecek | class-tint cyan → crimson'a (`#C01020`) çekilecek; düşük-can efekti zenginleşecek. |
| Director kart tasarımı | kısmi | scroll çözüldü; Hades-stili ikon+badge sonra gelecek (şu an işlevsel, metin-ağırlıklı). |
| 5/10 sınıf | tasarım kararı | derinlik > genişlik; demo Warblade + Elementalist üzerine odaklanır. |

---

## 9. Hocaya Konuşma Notları — "Bu var, şöyle güzelleşecek"

Aşağıdakiler sunum sırasında doğrudan söylenecek cümlelerdir. Her madde "ne çalışıyor" ile başlar, "nasıl iyileşecek" ile biter — dürüst, kendinden emin, savunmacı değil.

- "Çekirdek combat + boss + draft + run-map çalışıyor — vertical slice tamam. **İyileşecek:** düşman çeşitliliği + skill VFX zenginliği."
- "Build Mode oyun-içi seviye editörü — centerpiece. **İyileşecek:** Lights/Decals kategorileri dolacak + oda kaydet/yükle."
- "Director Mode canlı tuning aracı. **İyileşecek:** kart tasarımı Hades-stili ikon+badge'e geçecek (şu an işlevsel, metin-ağırlıklı)."
- "HUD modern sol-alt yerleşim. **İyileşecek:** HP barı rengi crimson'a, can-düşük efekti zenginleşecek."
- "Silah 8-yön ele mount + facing'e göre ön/arka katman. **İyileşecek:** her yön ince-ayar (bir yön doğrulandı, kalanı playtest sonrası)."
- "İkinci sınıf Elementalist var ama **eksik:** 8-yön sprite + skill VFX'leri (kredi/üretim sırada) — sahnede uzun tutma."
- "Asıl güç: mimari + oyun-içi tooling + AI-destekli geliştirme süreci + graphify ile veriyle-kanıtlanmış tooling tezi."

---

*RIMA — Rift Avcıları · Bitirme Projesi Sunum Raporu · 18 Haziran 2026 · Yasin Derya Bilgin*
