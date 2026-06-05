# COUNCIL — Opus (independent advisor): TBH "Task Bar Hero" karakter seçimi → RIMA uyarlanabilirliği

Date: 2026-06-05
Lens: game-design judgment + web-görsel analiz (WebSearch + WebFetch; video İZLENMEDİ — metin/screenshot kaynakları + diğer advisorlar video tarafını kapatıyor).

---

## 0) NET POZİSYON (önce sonuç)

**SKIP-as-template / PARTIAL-borrow (2 mikro-öğe).**

TBH'nin karakter seçim *modeli* RIMA'ya uyarlanamaz çünkü oyun-şekli temelden farklı (idle auto-battler, 3-kişilik kalıcı parti, taskbar mini-pencere — RIMA tek-karakter roguelite run). Roster-room'u TBH'ye benzetmek RIMA kimliğini zayıflatır. Ama TBH 2 küçük UX detayını çok temiz yapıyor; bunlar RIMA'nın MEVCUT v3.2 roster-room'una taşınabilir (yeni layout DEĞİL, mevcut layout'a ek):
1. **Portre-üstü "action dot" rozeti** (TBH: harcanmamış skill-point kırmızı nokta) → RIMA'da "yeni açıldı / okunmamış" veya "bu sınıfta seçilebilir build var" sinyali.
2. **Erişim-tier'ı net etiketle** (Free / Free-DLC / Paid) → RIMA'da locked silüetin nasıl açılacağını TEK kelimede söyleyen mikro-etiket (zaten "KİLİDİ AÇ {Echo}" var → TBH bunu doğruluyor, layout değişmez).

Geri kalan her şey: **SKIP.**

---

## 1) TBH ne ve karakter seçimi nasıl çalışıyor (kaynaklardan çıkardığım kadarıyla)

**Oyun-şekli:** Free-to-play idle hack-and-slash RPG. Windows taskbar'ında yaşayan mini-pencere; sen çalışırken parti otomatik dövüşüyor (Shift+F12 pozisyon reset, Shift+F11 ölçek). "Adorable pixel heroes" — ton CASUAL/sevimli, premium/dark değil. 500+ item, Common→Cosmic rarity, Steam Market trading. Yani meta-progression ve loot-grind merkezli, run-loop değil.

**Roster yapısı:** 6 sınıf. 3 ücretsiz başlangıç (Knight/Ranger/Sorcerer) + Priest (ücretsiz DLC) + Hunter/Slayer (ücretli DLC). Sınıf "açma" iki kanaldan: (a) oyun-içi para/rune ile slot/sınıf açma, (b) Steam store DLC claim.

**Seçim mekaniği = "Formation screen" (karakter-seç DEĞİL, parti-yönetimi):**
- Bu bir tek-seçim ekranı değil; **kalıcı 3-kişilik parti dizme** ekranı. Hero'ları slot'lara koyuyorsun, pozisyon değiştiriyorsun (reorder cooldown YOK; yeni hero deploy 60sn cooldown), pet takıyorsun, düşük-level hero'yu "carry" edip power-level'lıyorsun.
- **Görsel sunum:** hero PORTRELERİ (sprite-portre). Portre üzerinde **kırmızı nokta = harcanmamış skill-point** uyarısı. Savaşta 3 hero **yan yana pozisyonel** (ön=tank Knight, arka=DPS/Priest) — yani "formation" hem UI hem combat-layout.
- **Slot ekonomisi:** 1. ek slot 5.000 gold, 3.'sü 150.000 gold → açma bir grind-jest, bir "seç" jesti değil.
- **Bilgi mimarisi:** kaynaklar stat/skill panelini görsel olarak DETAYLANDIRMIYOR; vurgu fonksiyonel (rol etiketi: "Knight — Frontline tank — Free"). Yani bilgi sunumu RIMA'nın 5-stat-bar + tam skill-listesi kadar zengin DEĞİL görünüyor; daha çok rol + tier tablosu.

**Çok iyi yaptığı şey:** (1) kalabalık-yönetimi tek ekranda sıkıştırma (taskbar mini-pencere kısıtı zorluyor — minimalizm zorunluluktan erdem); (2) portre-üstü action-dot ile "neyi unuttun" sinyali; (3) erişim-tier'ı dürüst/net etiketleme.

---

## 2) RIMA v3.2 ile kıyas — TBH neyi DAHA İYİ, neyi DAHA KÖTÜ yapıyor?

**TBH daha iyi:**
- **Action-dot sinyali** — "şu karakterde yapılacak iş var" mikro-uyarısı. RIMA roster-room'unda bu YOK; eklemek ucuz kazanç.
- **Erişim-tier okunaklılığı** — neyin nasıl açıldığı tek bakışta belli. (RIMA zaten "KİLİDİ AÇ {Echo}" ile bunu yapıyor → confirm.)

**RIMA daha iyi (TBH'yi taklit ETMEMELİ):**
- **Diegetik sahne > portre-grid.** RIMA'nın iso taş-ada üstünde duran 10 karakteri, ayakta prosedürel cyan halka, sol kimlik paneli + sağ tam skill listesi = TBH'nin fonksiyonel-tablo sunumundan KİMLİK olarak çok daha güçlü. TBH minimalizmi taskbar-kısıtının ürünü; RIMA tam-ekran roguelite, o kısıt yok.
- **Bilgi derinliği.** RIMA 5 stat-bar + şartlı skill isimleri (karanlık + şart) sunuyor; TBH rol+tier tablosu. RIMA'nın "build-okuma" değeri daha yüksek.
- **Ton uyumu.** TBH "adorable/casual"; RIMA dark-fantasy "shattered echo". TBH'nin görsel tonunu almak RIMA kimliğini bozar.

**Temel uyumsuzluk (neden ADOPT yanlış):**
- TBH **çok-karakter kalıcı parti** seçer; RIMA **tek-karakter run-başı** seçer. TBH'nin formation/reorder/pet/carry mekaniğinin RIMA'da karşılığı YOK — taşımak yanlış soruna çözüm üretir.
- TBH'de "seçim" aslında **grind-gated slot açma**; RIMA'da seçim **run kimliği commit'i** (PlayerClassManager.SelectedClass). Farklı duygusal jest.

---

## 3) AL / ATLA

**AL (PARTIAL — mevcut v3.2 layout'a EK, yeni layout DEĞİL):**
- **AL-1: Portre/pedestal-üstü "action dot" rozeti.** RIMA semantiği: "yeni açıldı" veya "bu sınıf seçilmedi-hiç" veya "okunmamış skill detayı". Diegetik karşılığı: pedestal_seal halkasının üstünde küçük cyan mote/parıltı (kırmızı-nokta DEĞİL — RIMA paletinde cyan=sinyal). Renk disiplini 60-30-10 korunur.
- **AL-2: Erişim-jesti netliği (zaten var → confirm + keskinleştir).** "KİLİDİ AÇ {Echo}" plakası doğru yönde. TBH bunu doğruluyor: locked-item'ın nasıl açılacağı GİZLENMEMELİ. Tek ekleme: currency adı net olunca ({Echo} GATED) tek-kelime tutarlılığı.

**ATLA (SKIP):**
- TBH portre-grid layout'u (RIMA diegetik sahne daha güçlü).
- Formation / 3-parti / reorder / pet / carry (oyun-şekli yok).
- Taskbar-minimalizmi (kısıt yok, taklit RIMA'yı fakirleştirir).
- Casual/adorable ton (kimlik çatışması).
- DLC-tier-gated sınıf açma ekonomisi (RIMA içsel Echo-unlock'u kullanıyor — meta vs mekanik isim çakışması zaten GATED, bunu daha da karıştırma).

---

## 4) Kullanıcının "görsel hissi tam beğenmedi" sorununa dürüst not

TBH bu sorunu ÇÖZMEZ. Kullanıcının roster-room hoşnutsuzluğu büyük ihtimalle layout-paradigması değil **render-feel** (kontrast, okunabilirlik, karakter-okunması, halka VFX hissi) — ki bu Sodaman-council'in zaten işaret ettiği eksen (renk-okunabilirlik + synergy-sinyal). TBH'den layout kopyalamak yanlış kapı. Doğru kapı: mevcut diegetik sahneyi feel-polish (kontrast/silüet-okuma/hover-juice) + AL-1 action-dot sinyali. HTML-first mockup akışı (kullanıcı zaten istedi) bunu test etmenin doğru yolu — TBH'yi referans-board'a "ne YAPMAYALIM (casual portre-grid)" örneği olarak koy, "kopyala" olarak değil.

---

## 5) DECISION (RIMA formatı)

```
DECISION: TBH karakter-seçim modelini ŞABLON olarak SKIP; sadece 2 mikro-UX öğesi PARTIAL-borrow (portre-üstü action-dot sinyali + erişim-tier netliği), mevcut v3.2 roster-room layout'u KORUNUR.
RATIONALE: TBH idle/taskbar/3-parti auto-battler; RIMA tek-karakter dark-fantasy roguelite. Oyun-şekli uyumsuz → formation/portre-grid/casual-ton taşımak RIMA kimliğini zayıflatır. RIMA'nın diegetik sahnesi + stat/skill derinliği TBH'den ZATEN üstün. Sadece action-dot ve tier-netliği ucuz, kimlik-nötr kazanç.
TRADE-OFF: TBH'nin tek-ekran sıkışıklık/minimalizm verimi alınmaz — ama RIMA'da o kısıt (taskbar) yok, dolayısıyla kayıp değil. Action-dot eklemek bir parça VFX/UI işi.
SYSTEMS AFFECTED: CharacterSelect (UI/VFX katmanı, layout DEĞİL), unlock-sinyal sunumu. Combat/economy/class-identity ETKİLENMEZ.
CONFLICTS WITH LOCKED RULES?: NONE. (60-30-10 renk disiplini ve cyan=sinyal lock'una uyar — action-dot cyan-mote olarak; kırmızı-nokta TBH'den DEĞİL.) Currency "Echo" isim-çakışması zaten GATED; AL-2 bu kararı beklemeli.
ORCHESTRATOR NEXT STEP: (1) Kullanıcının asıl hoşnutsuzluğu = feel-polish, layout değil — HTML-first mockup akışında TBH'yi "kopyalanacak" değil "kaçınılacak casual-grid" referansı olarak işaretle. (2) AL-1 action-dot'u CharSelect feel-polish iş paketine EK madde olarak ver (rima-doc → spec; sonra cx). (3) AL-2 currency-isim kararı çözülene kadar BEKLE.
```

---

## Kaynaklar
- Steam: https://store.steampowered.com/app/3678970/TBH_Task_Bar_Hero/
- Beginner guide (mobalytics): https://mobalytics.gg/news/guides/taskbar-hero-beginner-guide
- GAMES.GG beginner: https://games.gg/tbh-task-bar-hero/guides/tbh-task-bar-hero-beginners-guide/
- Wiki: https://tbhtaskbarhero.wiki/
- thegamer tier/guide: https://www.thegamer.com/tbh-task-bar-hero-character-class-tier-list-best-guide/
- taskbarhero.xyz beginner: https://taskbarhero.xyz/guides/beginner-guide

> Not: Video İZLENMEDİ (kapsam dışı — diğer advisorlar). Formation-screen'in detaylı görsel layout'u (drag-drop mı, panel mi) metin kaynaklarında belgelenmemiş; bu boşluğu video-advisor doldurmalı. Yukarıdaki pozisyon oyun-ŞEKLİ analizine dayanır ve o detaydan bağımsız olarak sağlamdır.
