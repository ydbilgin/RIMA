# TASK_ROUTING_RULES.md
> **Ne zaman yükle:** Bir görevi kime vereceğine karar verirken.
> **Ne zaman yükleme:** İş yapılırken.

---

## KARAR AĞACI

```
Görev açık tanımlı mı? (tek doğru çözüm var, bağlam gerekmez)
├── EVET → SIMPLE
│   ├── Unity Editor işi mi?          → Antigravity
│   └── Script/PixelLab çağrısı mı?  → Kiro
│
├── KISMEN (teknik karar var ama mimari değil)
│   └── → ADVANCED → Claude Code (Sonnet)
│
└── HAYIR (yaratıcı yön, mimari, kalite kararı)
    └── → CRITICAL → Claude Code (Opus)
```

---

## WORKER KAPASİTELERİ

### Antigravity
**Yapabilir:** Unity Editor içi işler — component ekle/çıkar, inspector değeri ata, sahne düzenle, prefab kur, Animator Controller bağla, tilemap boya, build al.
**Yapamaz:** Tasarım kararı, yeni sistem yaz, hata analizi, bağlam yorumlama.
**Görev formatı:** `AGENTS.md → Antigravity Görevleri`

### Kiro
**Yapabilir:** Bağımsız, net tanımlı script yazma; PixelLab MCP çağrısı; Unity MCP ile sahne işlemleri; tek dosya düzenleme.
**Yapamaz:** Çok dosyayı kapsayan mimari değişiklik, tasarım kararı, sistem entegrasyonu.
**Görev formatı:** `AGENTS.md → Kiro Görevleri`

### Claude Code Sonnet
**Yapabilir:** Sistem yazma, hata ayıklama, refactor, dokümantasyon, PixelLab prompt optimizasyonu, Antigravity/Kiro için görev paketi hazırlama.
**Yapamaz:** "Bu mekanik eğlenceli mi?" gibi yaratıcı kalite kararları → Opus gerekir.

### Claude Code Opus
**Yapabilir:** Her şey + yaratıcı yön kararları, sistem mimarisi, combat feel, class/boss tasarımı, build balance, lore entegrasyonu.
**Ne zaman:** Sadece CRITICAL etiketli işlerde. Rutin iş için israf.

### Ollama (yerel LLM)
**Yapabilir:** Lore metni, item açıklamaları, NPC diyaloğu, TR/EN çeviri, flavor text.
**Ne zaman:** Sadece metin üretimi gereken ve kalite-kritik olmayan durumlarda. İnternetsiz veya token tasarrufu gerektiğinde.
**Yapamaz:** Kod yazma, sistem kararı, Unity işleri.

---

## ETİKET KURALLARI

### SIMPLE
- Tek doğru çözüm var
- Bağlam gerektirmez (sadece görevi oku, yap)
- Geri alınabilir veya düşük riskli
- Test: "Kiro bunu 5 dakikada yapabilir mi?" → Evet ise SIMPLE

### ADVANCED
- Teknik karar var ama mimari değil
- Proje bağlamı gerekiyor
- Birden fazla dosyayı etkiliyor
- Test: "Yanlış yapılırsa refactor gerekir ama projeyi bozmaz"

### CRITICAL
Aşağıdakilerden biri geçerliyse → CRITICAL:
- Combat feel / game feel kararı
- Class tasarımı veya skill balance
- Boss mekanik tasarımı
- Build progression / ekonomi kararı
- Sistem mimarisi (yeni manager, yeni pattern)
- Lore veya hikaye entegrasyonu
- Büyük refactor (3+ sistem etkiliyor)
- "Bu nasıl hissettirmeli?" sorusu içeriyorsa

---

## DEVİR KURALLARI

1. SIMPLE görevi devretmeden önce: tam bağımlılıkları yaz (hangi script, hangi prefab, hangi path)
2. ADVANCED görevi devretme — Claude Code'da tut
3. CRITICAL görevi asla devretme
4. Devredilen görev beklenmedik karar noktasına ulaşırsa: Claude Code'a döndür
5. Devredilen görevin çıktısını Claude Code review eder
