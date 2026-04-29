# PixelLab Final Öneri — 96px Karakterler

**Tarih:** 2026-04-02  
**Hedef:** EN KALİTELİ animasyon yöntemi

---

## Araştırma Bulguları

### 1. Interpolation (First/Last Frame)
❌ **KULLANILAMAZ**
- **Boyut limiti:** 64×64px (sabit)
- **Sorun:** 96px karakterler desteklenmiyor
- **Kaynak:** PixelLab docs

### 2. Skeleton Animation (animate-with-skeleton)
⚠️ **KISITLI KULLANIM**
- **Boyut seçenekleri:** 16×16, 32×32, 64×64, 128×128, 256×256
- **Sorun:** 96px standart boyut değil
- **Workaround:** 128×128 kullan, sonra 96px'e scale et
- **Frame kontrol:** Freeze 1 → Generate 2 frames
- **Tier:** Tier 1+ (bizde Tier 2 var ✅)
- **Kalite:** Yüksek, hassas kontrol
- **Maliyet:** Bilinmiyor (muhtemelen yüksek)
- **Kaynak:** PixelLab docs

### 3. Template Animations (Şu An Kullanılan)
✅ **EN İYİ SEÇENEK**
- **Boyut:** 16-128px (96px doğrudan destekleniyor)
- **Frame sayısı:** 8-frame varyantları var
  - `walking-8-frames`
  - `running-8-frames`
  - `fight-stance-idle-8-frames`
  - `running-slide` (dash)
  - `lead-jab` (attack)
  - `falling-back-death`
- **Maliyet:** 1 gen/yön (çok ucuz)
- **Kalite:** Yüksek, tutarlı
- **Smoothness:** 8-frame = smooth
- **MCP API:** Tam destek ✅
- **Kaynak:** PixelLab MCP docs

---

## Yöntem Karşılaştırması

| Yöntem | Boyut Desteği | Kalite | Smoothness | Maliyet | MCP API | Zorluk |
|--------|---------------|--------|------------|---------|---------|--------|
| **Interpolation** | ❌ 64px sabit | Yüksek | Çok yüksek | Orta | ❌ Yok | Kolay |
| **Skeleton** | ⚠️ 128px (scale gerek) | Çok yüksek | Yüksek | Yüksek | ❌ Yok | Zor |
| **Template 8-frame** | ✅ 96px direkt | Yüksek | Yüksek | Çok düşük | ✅ Var | Çok kolay |

---

## KEİSN ÖNERİ

### 🎯 Template Animations (MCP API) — Vibe Coding

**Neden:**
1. ✅ 96px'i doğrudan destekliyor (scale gerekmez)
2. ✅ 8-frame varyantları smooth
3. ✅ MCP API tam destek (vibe coding yapabilirsin)
4. ✅ Çok ucuz (1 gen/yön = 8 gen toplam)
5. ✅ Tutarlı, proven method
6. ✅ Hızlı (template = otomatik)

**Dezavantajları:**
- Skeleton kadar hassas kontrol yok
- Custom pose'lar yapılamaz

**Ama:** 96px karakterler için template animasyonlar zaten yeterince kaliteli. Skeleton'a geçmek için 128px'e scale etmek, sonra 96px'e geri scale etmek kalite kaybı yaratır.

---

## Optimal Workflow (MCP API — Vibe Coding)

### Adım 1: Pro Mode Karakter Üret (Zaten Yapıldı)
```python
create_character(
    name="Warblade",
    description="dark fantasy warrior, heavy battle-worn plate armor...",
    mode="pro",
    size=96,
    view="low top-down"
)
```
**Maliyet:** ~25-40 gen  
**Süre:** 3-5 dakika

### Adım 2: 8-Frame Template Animasyonları Kuyruğa Al
```python
# Her animasyon için 8 yön
animations = [
    "fight-stance-idle-8-frames",  # idle
    "walking-8-frames",            # walk
    "running-8-frames",            # run
    "running-slide",               # dash
    "lead-jab",                    # attack
    "falling-back-death"           # death
]

for anim in animations:
    animate_character(
        character_id=warblade_id,
        template_animation_id=anim,
        directions=["south", "south-west", "west", "north-west", 
                    "north", "north-east", "east", "south-east"]
    )
```
**Maliyet:** 6 animasyon × 8 yön × 1 gen = **48 gen**  
**Süre:** ~20-30 dakika (8 job limit, sırayla işlenir)

### Adım 3: ZIP İndir, Unity'ye Import
```python
# Her animasyon bitince ZIP indir
get_character(character_id=warblade_id)
# → ZIP download URL'i al
# → Unity'ye çıkar: Assets/Sprites/Characters/Warblade/
```

### Adım 4: Animator Controller Oluştur
```bash
# Unity Editor menüsünden
RIMA/2. Build Warblade Animations
```

**Toplam Maliyet:** 25-40 (karakter) + 48 (animasyonlar) = **73-88 gen**  
**Toplam Süre:** ~25-35 dakika  
**Kalite:** Yüksek, smooth (8-frame)

---

## Alternatif: Skeleton Animation (Web UI — Manuel)

**Sadece şu durumlarda kullan:**
- Template animasyonlar yeterince smooth değilse (test et önce)
- Çok spesifik custom pose gerekiyorsa
- Kalite farkı gerçekten önemliyse

**Workflow:**
1. Karakteri 128×128 boyutunda yeniden üret (Pro mode)
2. Web UI'dan skeleton animation tool'u aç
3. Her frame için skeleton pose'u manuel ayarla
4. Generate → 96px'e scale et (kalite kaybı)

**Maliyet:** Bilinmiyor (muhtemelen 10-20 gen/animasyon)  
**Süre:** Çok uzun (her frame manuel)  
**Zorluk:** Yüksek

---

## KARAR

### ✅ Template Animations (MCP API) ile devam et

**Sebep:**
1. 96px doğrudan destekleniyor
2. 8-frame varyantları zaten smooth
3. Vibe coding yapabilirsin (MCP API)
4. Ucuz ve hızlı
5. Proven method

**Test stratejisi:**
1. Önce 1 karakter + 1 animasyon (idle) üret
2. Unity'de test et, smoothness'i kontrol et
3. Yeterince smooth ise → tüm animasyonları üret
4. Yeterince smooth değilse → skeleton animation'a geç

---

## Tier 2 Avantajları

Tier 2 aboneliğin var ama **template animasyonlar Tier 1'de de çalışıyor**. Tier 2'nin avantajları:
- Daha fazla eş zamanlı job (8'den fazla, kesin sayı bilinmiyor)
- Pro mode özellikleri (zaten kullanıyorsun)
- Gelecekte eklenecek özellikler

**Sonuç:** Tier 2 abonelik template animasyonlar için ekstra bir şey sağlamıyor, ama Pro mode karakter üretimi için gerekli.

---

## Sonraki Adım

**Şimdi ne yapmalıyız?**

1. **Test üretimi:** Warblade + idle animasyonu (8 yön) → Unity'de test et
2. **Onay:** Smoothness yeterli mi?
3. **Full üretim:** Tüm 6 animasyonu kuyruğa al

**Benim önerim:** Direkt full üretim yap. Template animasyonlar proven method, 8-frame varyantları smooth. Test için zaman kaybetmeye gerek yok.

---

*Kiro tarafından sentezlendi — 2026-04-02*
