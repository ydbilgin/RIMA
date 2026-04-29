# RIMA — Harici Araçlar & Kaynaklar
*Kaynak: https://github.com/Kavex/GameDev-Resources + kişisel seçkiler*
*Son güncelleme: 2026-03-31 —claude*

> Bu dosya proje boyunca kullanılabilecek ücretsiz/düşük maliyetli araçları listeler.
> Her araç "ne zaman lazım olur" notu ile birlikte verilmiştir.

---

## 🔊 SES EFEKTİ (SFX) ÜRETİMİ

### Bfxr
**Link:** bfxr.net
**Ücret:** Ücretsiz
**Ne yapar:** Retro/chiptune oyun sesi üreteci. Sliders ile anında hit, explosion, powerup, coin, jump, laser sesi üretirsin. Export: WAV.
**RIMA'da ne zaman:** Faz 1 — Warblade hit sesi, cleave impact, dash whoosh, ölüm burst. Iron Warden slam.
**Nasıl kullanılır:**
1. bfxr.net → browser'da açılır
2. Sol menüden ses tipi seç (Hit/Hurt, Explosion, Powerup...)
3. "Mutate" veya "Randomize" butonlarıyla varyasyon üret
4. Beğendiğinde "Export .WAV" → Unity'e import

---

### ChipTone
**Link:** sfbgames.itch.io/chiptone
**Ücret:** Ücretsiz
**Ne yapar:** Bfxr'dan daha güçlü SFX üreteci. Waveform, envelope, pitch, filter — tam kontrol. Export: WAV.
**RIMA'da ne zaman:** Bfxr yetmezse — boss giriş efekti, skill cast sesi, UI click.
**Not:** Bfxr'la başla, ChipTone'u detay için sakla.

---

### Freesound
**Link:** freesound.org
**Ücret:** Ücretsiz (CC lisanslı — kullanmadan önce lisansı kontrol et)
**Ne yapar:** Milyonlarca gerçek ses kaydı — ambient, atmospheric, impact, voice.
**RIMA'da ne zaman:** Dungeon ambient loop (Act 1: taş zemin titreşimi, Act 2: bataklık, Act 3: void hum). Boss müzik intro.
**Nasıl kullanılır:**
1. Arama kutusuna gir: "dungeon ambience loop", "void hum", "stone rumble"
2. Filtrele: Duration (loop için 5s+), License: CC0 (en güvenli)
3. İndir → Unity → AudioSource Loop: ✓

---

### BeepBox
**Link:** beepbox.co
**Ücret:** Ücretsiz
**Ne yapar:** Tamamen browser tabanlı chiptune besteci. Act temalarını nota nota taslak olarak çıkarabilirsin. Export: WAV, OGG, MIDI.
**RIMA'da ne zaman:** Act 1 / Act 3 / Hub müzik taslağı. Finale sunum için placeholder BGM.
**Not:** Gerçek composer yoksa bu araçla kaba taslak çıkar → soundcloud'a at → topluluk feedback al.

---

### FreeSFX / FreePD / Musopen
| Araç | Link | Ne için | Not |
|------|------|---------|-----|
| FreeSFX | freesfx.co.uk | Hazır SFX paketi indir | Lisans kontrol et |
| FreePD | freepd.com | Public domain müzik | Trailer/teaser için |
| Musopen | musopen.org | Klasik müzik arşivi | Sadece lokal test için |

---

## 🎨 ASSET / GÖRSEL KAYNAKLAR

### Kenney Assets
**Link:** kenney.nl/assets
**Ücret:** Ücretsiz, ticari kullanım OK (CC0)
**Ne yapar:** Binlerce ücretsiz oyun asset'i — UI paketi, pixel karakter, tile set, ses efekti.
**RIMA'da ne zaman:**
- **Şu an:** UI elementleri — health bar frame, button, panel → HUDManager'a bağla
- **Placeholder:** Gerçek sprite'lar hazır olmadan Kenney sprite'larını kullan
- **Ses:** Kenney'nin ses paketleri de var (kenney.nl/assets/category/audio)
**Öneri:** "UI Pack" ve "Pixel Platformer" paketlerine bak.

---

### OpenGameArt
**Link:** opengameart.org
**Ücret:** Ücretsiz (lisans değişir — her asset için kontrol et)
**Ne yapar:** CC lisanslı sprite, tile, müzik arşivi.
**RIMA'da ne zaman:**
- Gemini'ye "make something like this but top-down" diyeceğin **referans görseller** bulmak için
- Act tile set'leri için görsel referans
- Placeholder müzik BGM

---

### 420 Pixel Art RPG Icons
**Link:** opengameart.org üzerinde arama: "420 rpg icons"
**Ücret:** Ücretsiz, ticari kullanım OK
**Ne yapar:** 420 adet pixel art RPG skill/item ikonu seti.
**RIMA'da ne zaman:** PixelLab ile kendi skill ikonlarını üretene kadar **placeholder skill ikonları** olarak kullan.

---

## 🖊️ PIXEL ART ARAÇLARI

### PiskelApp
**Link:** piskelapp.com
**Ücret:** Ücretsiz
**Ne yapar:** Browser tabanlı pixel art editörü. Aseprite'ın ücretsiz online alternatifi. Animasyon desteği var. Export: PNG sprite sheet, GIF, JSON.
**RIMA'da ne zaman:**
- Ana bilgisayarda değilsen ve hızlı bir değişiklik gerekiyorsa
- Başka birinin bilgisayarında bir şey göstermek gerekirse
- Aseprite kurulu değil, ama küçük bir placeholder lazımsa
**Dezavantaj:** PixelLab extension'ı yok. Ana workflow Aseprite + PixelLab.

---

### Pixelicious
**Link:** pixelicious.xyz
**Ücret:** Ücretsiz
**Ne yapar:** Normal görseli → pixel art'a çevirir (image-to-pixel art converter).
**RIMA'da ne zaman:** Gemini'den gelen referans görseli pixel art stiline "dönüştürmek" için ön adım. PixelLab'ten önce kabaca dönüştür.

---

## 📊 PROJE YÖNETİMİ

### Trello
**Link:** trello.com
**Ücret:** Ücretsiz plan yeterli
**Ne yapar:** Kanban board — "Yapılacak / Devam / Tamamlandı" kartları.
**RIMA'da ne zaman:** Faz 1 task'larını buraya al. README'deki "Şu An Yapılacak" listesini Trello'da tut.
**Board önerisi:**
```
Sütunlar: Backlog | Faz 1 | Faz 2 | In Progress | Done
```

### Taiga
**Link:** taiga.io
**Ücret:** Ücretsiz (open source)
**Ne yapar:** Trello'dan güçlü — sprint, backlog, bug tracker.
**Not:** Solo dev için Trello genellikle daha az overhead.

---

## 📢 PAZARLAMA (Steam aşaması — henüz erken)

### Glitch — Video Game Marketing
**Link:** glitch.com (marketing tool, not the code platform)
**Ücret:** Freemium
**Ne yapar:** Video oyunlara özel marketing otomasyon — press kit, email list, social.
**RIMA'da ne zaman:** Demo hazır olduğunda, Steam sayfası açmadan önce.

---

## 📈 ANALİTİK (Beta/Playtest aşaması)

| Araç | Ne için | Ne zaman |
|------|---------|----------|
| Unity Analytics (built-in) | Run başına istatistik, ölüm noktaları, oturum uzunluğu | Beta/playtest |
| itch.io Analytics | İlk playtest'ler için | Demo çıkışı |
| Steam Analytics | Wishlist, conversion, retention | Steam çıkışı |

---

## 🔗 HIZLI ERİŞİM LİSTESİ

```
SFX ÜRETİMİ (şu an):
  Bfxr        → bfxr.net
  ChipTone    → sfbgames.itch.io/chiptone
  Freesound   → freesound.org

MÜZİK TASLAK (şu an):
  BeepBox     → beepbox.co

ASSET (şu an):
  Kenney      → kenney.nl/assets
  OpenGameArt → opengameart.org

PIXEL ART (yedek):
  PiskelApp   → piskelapp.com
  Pixelicious → pixelicious.xyz

PROJE YÖNETİMİ:
  Trello      → trello.com

PAZARLAMA (ileride):
  Glitch      → (video game marketing tool)
```
