# TASK_LABELS.md
> **Ne zaman yükle:** Yeni bir görev etiketlenirken veya örnek etiket listesine bakılırken.

---

## ÖRNEK ETİKETLEMELER

### SIMPLE — Antigravity

| Görev | Neden SIMPLE |
|---|---|
| Global Light 2D intensity değiştir | Tek inspector değeri |
| Prefaba Animator component ekle | Mekanik, bağlam yok |
| Animator Controller'ı prefaba ata | Tek adım |
| Tilemap'e tile boya (renk/boyut belli) | Tekrar eden, öngörülebilir |
| Sahneye GameObject ekle, transform ayarla | Mekanik |
| Component değerini inspector'dan güncelle | Tek inspector değeri |
| Build al (platform belli) | Mekanik |
| Player tag ata | Tek adım |

### SIMPLE — Kiro

| Görev | Neden SIMPLE |
|---|---|
| PixelLab'dan karakter güney sprite indir | Net API çağrısı |
| `ART/_REVIEW/` altına sprite kaydet | Dosya işlemi |
| Tek bir ScriptableObject field'ı doldur | Mekanik |
| Mevcut script'e küçük null check ekle | Lokal, düşük risk |
| PixelLab animasyon job başlat (template belli) | Net parametre |
| Script skeleton yaz (metodlar belli, logic yok) | Mekanik |

### SIMPLE — Ollama

| Görev | Neden SIMPLE |
|---|---|
| 5 NPC diyaloğu yaz (ton ve karakter belli) | Metin üretimi, kalite-kritik değil |
| Item flavor text yaz (format belli) | Metin üretimi |
| TR → EN çeviri (oyun metni) | Çeviri |

---

### ADVANCED — Claude Code Sonnet

| Görev | Neden ADVANCED |
|---|---|
| Yeni sistem script'i yaz (manager, controller) | Teknik karar var, mimari değil |
| Hata ayıklama (runtime exception) | Bağlam gerekiyor |
| Mevcut sisteme yeni özellik ekle | Entegrasyon var |
| PixelLab prompt optimize et | Teknik bilgi gerekiyor |
| Animasyon builder script yaz | Birden fazla dosya |
| Antigravity/Kiro için görev paketi hazırla | Bağlam yorumu |
| Dokümantasyon yaz/güncelle | Yazım kararı |
| Unity MCP ile sahne kurulumu (sistem entegrasyonu var) | Bağlam gerekiyor |

---

### CRITICAL — Claude Code Opus

| Görev | Neden CRITICAL |
|---|---|
| Yeni class tasarımı (kaynak, skill set, feel) | Yaratıcı yön kararı |
| Boss mekanik tasarımı (fazlar, faz tetikleyicileri) | Combat feel + mimari |
| Cross-class sinerji kararı | Build balance |
| Skill progression / ekonomi kararı | Balance, sistem geneli |
| Combat feel ayarı (hız, hasar, geri tepme) | Kalite kararı |
| Yeni sistem mimarisi (yeni pattern, büyük manager) | Mimari |
| Lore / hikaye entegrasyonu | Yaratıcı yön |
| 3+ sistemi etkileyen refactor | Risk + bağlam |
| "Bu mekanik eğlenceli mi?" sorusu | Kalite kararı |
| Oda tipi veya map generation tasarımı | Sistem tasarımı |

---

## GRİ ALANLAR

Emin değilsen şu soruları sor:

1. **"Yanlış yapılırsa ne olur?"**
   - Geri alınabilir, düşük etki → SIMPLE/ADVANCED
   - Projeyi bozar veya yeniden tasarım gerektirir → CRITICAL

2. **"Bu görev bir karar içeriyor mu?"**
   - Hayır, sadece uygulama → SIMPLE
   - Teknik karar var → ADVANCED
   - Tasarım/yaratıcı karar var → CRITICAL

3. **"Kiro bunu bağlam okumadan yapabilir mi?"**
   - Evet → SIMPLE
   - Hayır → ADVANCED veya CRITICAL
