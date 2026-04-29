# KIRO TASK — Screenshot Analysis + Cleanup
*Date: 2026-04-08 | Read this file, apply in order. Do not read other files.*

---

## RISK LEVEL: LOW
> - Deterministic: her screenshot için sabit çıktı formatı
> - Mechanical: görünen UI elemanlarını listele, yorum yok
> - Isolated: sadece aşağıda listelenen dosyalara dokunuyor
> - Bounded: adımlar tam belirli
> - Mechanically verifiable: çıktı dosyaları yazıldı mı kontrol edilebilir

---

## CONTEXT

`F:/Antigravity Projeler/2d roguelite/claude ss/` klasöründe 68 adet Screenshot_N.png dosyası var.
Bu ekran görüntüleri PixelLab'in Aseprite Extension arayüzünü gösteriyor.
Görev:
1. Her screenshot'ı oku, Aseprite PixelLab plugin panelini gösterenleri belgele
2. Belgelenen bilgiyi `ASEPRITE_EXTENSION.md` dosyasına yaz (tıklama bazlı rehber)
3. Biten klasörleri `_BACKUP/` altına taşı

---

## FILES TOUCHED

**OKUMA:**
- `F:/Antigravity Projeler/2d roguelite/claude ss/Screenshot_1.png` ... `Screenshot_68.png`

**YAZMA:**
- `F:/Antigravity Projeler/Pixellab/ASEPRITE_EXTENSION.md`

**TAŞIMA (kaynak → hedef):**
- `F:/Antigravity Projeler/2d roguelite/KIRO_CHARACTER_ANIMATIONS.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/KIRO_CHARACTER_BASE_SPRITES.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/KIRO_DEATH_ANIM_FIX.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/KIRO_MOB_ATTACKS.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/KIRO_VOID_HALFTHRALL.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RelicCaster_temp` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RelicCaster_temp.zip` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/ANIMATION_PROGRESS.txt` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/ART` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/ART_REDIRECT` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/ASEPRITE_GOREVLER.md` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/pixellab_downloads` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/claude ss` → `F:/Antigravity Projeler/2d roguelite/_BACKUP/claude_ss/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_ANIMATION_BATCH3.md` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_PIXELLAB_ACT1_TILES.md` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/KIRO_UNITY_MOB_PREFABS.md` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/pixellab_ai_session` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/temp_halfthrall_qc.png` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`
- `F:/Antigravity Projeler/2d roguelite/RIMA/temp_qc_frame.png` → `F:/Antigravity Projeler/2d roguelite/RIMA/_BACKUP/`

---

## STOP AND ESCALATE — Report to Claude if:

- Bir screenshot açılamıyor
- Hedef `_BACKUP/` klasörü yok (oluşturma yetkisi var ama bildirmelisin)
- ASEPRITE_EXTENSION.md yazılamıyor
- Bir screenshot Aseprite plugin dışında bir şey gösteriyor (Unity, tarayıcı, vb.) — belirtmeli misin? EVET, REPORT'a yaz

---

## TASK 1 — Screenshot Analizi

### Adım 1: Her screenshot'ı oku

68 dosyanın hepsini sırayla oku: `Screenshot_1.png` ... `Screenshot_68.png`

Her screenshot için şunu not et (internal not, sonraki adımda kullanacaksın):
```
Screenshot_N:
  KONU: [Aseprite Plugin Panel / Aseprite Ana Ekran / Başka Uygulama / Belirsiz]
  GÖRÜNEN PANELLER/BÖLÜMLER: [listele]
  UI ELEMANLARI: [her görünür label, input, dropdown, button, slider, checkbox]
    - Label: "..." | Tip: [input/dropdown/button/slider/checkbox] | Değer/Seçenekler: [...]
  NOT: [dikkat çeken özel bir şey varsa]
```

### Adım 2: ASEPRITE_EXTENSION.md dosyasını yaz

`F:/Antigravity Projeler/Pixellab/ASEPRITE_EXTENSION.md` dosyasını **tamamen yeniden yaz** (eski içerik silinir) aşağıdaki yapıda:

```markdown
# PixelLab Aseprite Extension — Tam UI Rehberi
> Kaynak: Screenshot analizi (68 görüntü, 2026-04-08)
> Ne zaman yükle: Aseprite ile animasyon üretirken

---

## Extension Nasıl Açılır
[Screenshot'lardan gördüklerini yaz — menü yolu, kısayol]

---

## Plugin Paneli — Bölümler ve Alanlar

### [Bölüm 1 adı — screenshot'tan al]
[Her alan için:]
- **[Alan adı]** — Tip: [dropdown/input/button/...] — Seçenekler/Değerler: [...] — Ne işe yarar: [kısa açıklama]

### [Bölüm 2 adı]
...

---

## Animasyon Üretimi — Tıklama Bazlı Adımlar

### Yeni Animasyon Oluştur (Attack / Dash için)
```
STEP 1: [tam tıklama talimatı]
STEP 2: ...
```

### Reference Image Yükleme
```
STEP 1: ...
```

### Frame Sayısı ve Ayarlar
...

### Generate ve Export
...

---

## Dropdown Seçenekleri — Tam Liste
[Her dropdown için tüm seçenekleri listele]

---

## Bilinen Sorunlar ve Çözümleri
[Screenshot'lardan veya genel bilgiden gözlemlenen sorunlar]
```

**Kural:** Sadece screenshot'lardan gördüğün şeyleri yaz. Tahmin etme. Emin olmadığın yere `[screenshot'ta net görünmüyor]` yaz.

---

## TASK 2 — Dosya Taşıma

Yukarıdaki FILES TOUCHED bölümündeki tüm taşıma işlemlerini yap.

Her taşıma için:
1. Hedef klasör yoksa oluştur
2. Dosyayı/klasörü taşı
3. Başarılı mı kontrol et

**Taşıma sırası önemli değil, hepsini yap.**

---

## QC

**Task 1 QC:**
- `F:/Antigravity Projeler/Pixellab/ASEPRITE_EXTENSION.md` dosyası oluşturuldu mu? → PASS
- Dosya 50 satırdan uzun mu? → PASS (az içerik = screenshot okunamadı demek)
- "screenshot'ta net görünmüyor" ibaresi var mı? → Normal, sorun değil

**Task 2 QC:**
- Tüm kaynak dosyalar/klasörler artık orijinal konumunda yok → PASS
- Tüm dosyalar _BACKUP/ altında mevcut → PASS

---

## REPORT — Fill this before saying anything to user

```
STATUS: DONE / FAILED / PARTIAL

COMPLETED:
  - Task 1 Screenshot analizi — [kaç screenshot Aseprite plugin gösterdi / kaç tanesi başka şey]
  - Task 1 ASEPRITE_EXTENSION.md — [kaç satır yazıldı]
  - Task 2 Cleanup — [kaç dosya/klasör taşındı]

ERRORS:
  - [hata + hangi task] veya NONE

QC_RESULT:
  - ASEPRITE_EXTENSION.md — PASS/FAIL — [sebep]
  - Cleanup — PASS/FAIL — [sebep]

NEXT_SIGNAL: "aseprite analizi hazır"
```
