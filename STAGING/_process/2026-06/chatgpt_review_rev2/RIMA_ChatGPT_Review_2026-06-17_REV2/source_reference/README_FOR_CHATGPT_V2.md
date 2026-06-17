# RIMA — KAPSAMLI Ekran Görüntüleri v2 + Fonksiyonel Kanıt + Güzelleştirme İncelemesi (ChatGPT)
> RIMA = solo-dev 2D top-down ARPG roguelite (Unity URP 2D, pixel-art, high top-down 3/4). Demo 19 Haz. **v1 = 13 ekran; bu v2 = 25 state + fonksiyonel runtime-assert** (council-enumerated, dev-direct _Arena).
>
> **ChatGPT'den istenen:** her ekran için (a) görsel/UX değerlendirmesi, (b) **somut "ne nasıl değişebilir" güzelleştirme** önerisi, (c) solo-dev 2-gün quick-win vs post-demo. Stil canon: koyu-ama-okunur, void MOR + slate gri zemin + cyan ≤%15 vurgu + ember turuncu; "grimdark" DEĞİL.

## 🔴 REVIEWER'A TALİMAT (ChatGPT — lütfen oku)
Yüzeysel "şunu parlat" yorumu İSTEMİYORUZ. **Gerçek, opinyonlu, kullanılabilirlik-odaklı** eleştiri istiyoruz:
1. Her ekran/sistem için **NE KULLANIŞSIZ/KÖTÜ ve NEDEN** (sadece estetik değil — UX akışı, okunabilirlik, kullanıcının ne yapacağını anlaması).
2. **Somut redesign** önerisi (jenerik "kontrast artır" değil — "X paneli Y'ye böl, Z ikonu ekle, şu hiyerarşi" gibi).
3. Her öneriye **etki/efor** (solo-dev 2-gün quick-win vs post-demo) + **öncelik sırası.**
4. **Aşağıdaki Claude izlenimine KATIL ya da İTİRAZ ET** (özellikle Director Mode "debug-overlay gibi" tezine).

### 👁️ CLAUDE'UN AÇIK İZLENİMİ (bunlara tepki ver)
- **Director Mode = en zayıf halka:** debug-overlay gibi duruyor, ÜRÜN gibi değil — turuncu wireframe çerçeve enflasyonu, yoğun metin, hiyerarşi yok, dağınık paneller. İroni: demo tezi "RIMA = environment + tooling showcase" yani tooling YILDIZ olmalı ama bir programcı debug-paneli gibi. **Bu en kritik düzeltme.**
- **Build Mode:** fonksiyonel sağlam (8/8 assert) ama mor grid sahneyi eziyor, palet minik/etiketsiz → "araç" hissi zayıf.
- **Combat:** büyük siyah-blob düşman "eksik sprite" gibi; void arka plan "harita-dışı boşluk" gibi → ucuz gösteriyor.
- **HUD:** fazla minimal, can/kaynak zor okunuyor.
- **Güçlü:** Codex (profesyonel), Boss (atmosferik+okunabilir), Merchant, draft (düzeldi), renk-canon.
> ChatGPT: bu tezlere katılıyor musun? Director'ı "debug-overlay"den "profesyonel tool"a çevirmek için somut layout/komponent planı ver.

## 🟢 FONKSİYONEL KANIT (sadece görsel değil — sistemler GERÇEKTEN çalışıyor)
- **BuildMode 8/8 PASS:** asset-seçim doğru · **tile yerleştirme `(2,3)`'e tam oturuyor** (`tilePosition=(2,3)`, `placedByUser=BuildMode`) · erase · undo/redo · working-copy izolasyonu (kaynak .asset kirlenmiyor) · exit temizliği. → **Build mode fonksiyonel olarak SAĞLAM** (kullanıcının "tile doğru oturuyor mu" sorusu = EVET).
- **DirectorMode 6/6 PASS:** spawn (count+1) · stat (`physPower=177` uygulanıyor) · telemetry CSV export.
- **0 console error.** Assert detayları: `_buildmode_asserts.json` / `_director_asserts.json`.

---
## EKRAN EKRAN (NE · ŞU AN · 🎨 NE NASIL DEĞİŞEBİLİR)

### Menü & Ayar
- **01 MainMenu** — RIMA logo + menü. Backdrop bazen düz çıkıyor (v1 tutarsızlık). 🎨 backdrop her zaman + menü kontrast şeridi.
- **17 Settings** — bölümler (dil/erişilebilirlik/ses/kontrol). İşlevsel, sade. 🎨 arka dim/blur (yüzme hissi), satır nefesi, toggle renk-kod.
- **18 Codex (YETENEK KODEKSİ)** — 10 sınıf sekmesi + skill listesi. ✅ en güçlü ekranlardan. 🎨 satır yüksekliği+ikon büyüt, rarity sol-şerit.

### Combat & HUD
- **03/05 HUD normal** — minimal HUD (üst-sol minik, alt slot). 🎨 net can+kaynak barı, slot ikon+tuş-harf+cooldown.
- **13 Combat mid** — ⚠️ **büyük SİYAH BLOB düşman** (sol-üst) okunmuyor; küçük mor-yıldız mob'lar daha iyi; void arka plan. 🎨 düşmana outline/rim-light + focus-pixel + x1.5; void→atmosferik renk (#1a0f2e).
- **14 Combat low-HP** — kırmızı tint. 🎨 alpha %30 + sadece kenarlar + nabız (full-screen değil).
- **02/20 Reward/Opening draft** — 3 kart (scrim'li, düzeldi). 🎨 rarity renk+glow, açıklama büyüt, kart-hover tooltip yeri.

### Modlar (dev/demo)
- **07 Director base / 08 spawn / 09 stats** — turuncu sci-fi panel. İşlevsel (assert PASS) ama kalabalık. 🎨 panelleri grupla+collapse, telemetry hasar-tipi renk-kod, ikon-grid (IMGUI hissi azalt).
- **10 Build entry / 11 prop-placed / 12 tile-tool** — ✅ fonksiyonel (8/8). ⚠️ mor grid baskın, palet ikonları küçük/etiketsiz. 🎨 grid soluk/ince, asset kart thumbnail+etiket, seçili kalın amber çerçeve, geçerli/geçersiz yeşil/kırmızı ghost + sebep, durum-şeridi (asset/cell/undo-count).

### Overlay & Oda-tipleri
- **16 Pause** — Resume/Settings/Codex/MainMenu/Quit (scrim'li). 🎨 panel büyüt+cyan accent.
- **19 TAB CharacterSheet** — stat paneli. 🎨 hizalı satır, hiyerarşi.
- **21 RunMap (KOŞU YOLU)** — branching DAG (M-bleed düzeldi). 🎨 düğüm tip-ikonu, bağlantı belirgin, mevcut-düğüm parlak halka.
- **22 Merchant** — ✅ 3 shop-stand (cyan halka+kart+fiyat). İyi. 🎨 fiyat-etiket okunurluk, "Satın Al E" prompt netliği.
- **23 Boss / 24 BossHealthBar** — ✅ "THE PENITENT SOVEREIGN" + yeşil bar + okunabilir golem + monolog. Güçlü. 🎨 boss-bar'a faz-işaretleri, intro kamera.

### Son-durum
- **04/15 DeathScreen** — ölüm ekranı. 🎨 net başlık, run-stat hizası.
- **25 Scene-view** — editör görünümü (referans).

---
## 🎨 GENEL "NE NASIL DEĞİŞEBİLİR" (council sentezi — Dead Cells/Hades/StS referans)
**Quick-win (2 gün):**
1. **Paylaşılan modal scrim** zaten eklendi → tüm overlay'lerde tutarlı dim.
2. **Asset/kart UI:** ikon+etiket+seçili-vurgu (kalın çerçeve/glow), eşit kart boyu, 8/12px tutarlı spacing.
3. **Düşman okunabilirliği:** outline/rim-light + focus-pixel (siyah blob → silüet).
4. **Void→atmosfer:** arka plan siyah değil koyu-mor.
5. **HUD juice:** can barı lerp, low-HP nabız (kenar), hasar-sayı.
6. **Build/Director:** grid soluklaştır, palet thumbnail+etiket, durum-şeridi, geçerli/geçersiz renk-feedback.
**Post-demo:** map-panel birleştirme (RunMap/Dungeon/MapPanel tek sistem), undo-history paneli, asset-inspector, gamepad nav, colorblind-safe states.

## ULAŞILAMAYAN (bu pakette yok)
- DemoComplete/Victory (prefab yok + terminal-clear akışı) · Forge/Chest/Event odaları (procedural, dev-direct sabit dizide yok) · CharacterSelect/Chamber (full-flow gerekir) · RoomTransitionFX (frame-perfect timing). İstenirse ayrı tur.

## TİPOGRAFİ/TASARIM SORUSU (ChatGPT'ye)
Genel: font hiyerarşisi, ikonografi (run-map/build/slot ikonsuz), spacing tutarlılığı, renk-canon (mor/slate/cyan/ember) sadık mı — ve her ekran için en yüksek-etkili 1-2 değişiklik ne olurdu?
