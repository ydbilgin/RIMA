# DELEGATION_TEMPLATE.md
> **Ne zaman yükle:** Bir görev için karar kartı üretirken.
> **Ne zaman yükleme:** Normal geliştirme session'larında.

---

## KARAR KARTI FORMATI

Her görev için Claude Code şu formatı üretir:

```
---
Label: SIMPLE | ADVANCED | CRITICAL
Recommended Worker: Antigravity | Kiro | Claude Code Sonnet | Claude Code Opus | Ollama
Why: [tek cümle gerekçe]
Input Files Needed: [dosya listesi veya "yok"]
Output Format: [ne üretilecek: script, prefab, sprite, inspector değeri, vb.]
Safe to Delegate? Yes | No
---
```

---

## ÖRNEKLER

### Örnek 1 — Antigravity görevi

```
---
Label: SIMPLE
Recommended Worker: Antigravity
Why: Tek inspector değeri — bağlam gerektirmez.
Input Files Needed: yok
Output Format: Sahne değişikliği (Global Light 2D intensity = 2.5)
Safe to Delegate? Yes
---
Görev: _Sandbox sahnesindeki Global Light 2D nesnesinin intensity değerini 1.5'ten 2.5'e çıkar.
```

### Örnek 2 — Kiro görevi

```
---
Label: SIMPLE
Recommended Worker: Kiro
Why: Net API çağrısı, parametreler belli, karar yok.
Input Files Needed: TASARIM/SANAT_PROMPTLARI.md (SeamCrawler bölümü)
Output Format: ART/_REVIEW/seam_crawler_south.png
Safe to Delegate? Yes
---
Görev: PixelLab MCP ile SeamCrawler karakterinin south.png'sini indir, ART/_REVIEW/mobs/ altına kaydet.
mcp__pixellab__get_character(id="[id]") ile URL al, curl ile indir.
```

### Örnek 3 — Claude Code Sonnet

```
---
Label: ADVANCED
Recommended Worker: Claude Code Sonnet
Why: Birden fazla sistemi entegre ediyor, teknik karar var.
Input Files Needed: Assets/Scripts/Core/RoomManager.cs, Assets/Scripts/Core/StatusEffectSystem.cs
Output Format: Güncellenmiş RoomManager.cs + yeni RoomRewardManager.cs
Safe to Delegate? No
---
Görev: Oda temizlenince StatusEffect stack'lerini sıfırla, reward hesapla ve HUDManager'a bildir.
```

### Örnek 4 — Claude Code Opus

```
---
Label: CRITICAL
Recommended Worker: Claude Code Opus
Why: Class kaynak sistemi tasarımı — combat feel + progression mimarisi kararı.
Input Files Needed: TASARIM/GDD.md (Elementalist bölümü), TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md
Output Format: Tasarım kararı belgesi + ManaSystem.cs iskelet
Safe to Delegate? No
---
Görev: Elementalist'in mana regeni nasıl çalışmalı? Pasif regen mi, kill-based mi, combo-based mi?
Combat feel açısından hangisi daha sağlıklı?
```

### Örnek 5 — Ollama

```
---
Label: SIMPLE
Recommended Worker: Ollama
Why: Metin üretimi, format ve ton belli, kalite-kritik değil.
Input Files Needed: yok (ton: gizemli, kısa, melankolik)
Output Format: 3 adet dialog satırı (Forge Wraith NPC, TR)
Safe to Delegate? Yes
---
Görev: Forge Wraith NPC için 3 kısa diyalog satırı yaz. Ton: gizemli, yıpranmış, savaşçı.
Her satır max 15 kelime. Örnek: "Çatlak seni tanıyor. Ama sen onu tanıyor musun?"
```

---

## TOPLU DEVİR PAKETİ FORMATI

Birden fazla SIMPLE görev aynı anda devredilecekse:

```markdown
## [Antigravity | Kiro] — Görev Paketi [tarih]

> Bu dosyayı oku, başka dosya okuma. Sırayla yap. Her görev bağımsız.

### Görev 1 — [isim]
[görev açıklaması]
Başarı kriteri: [ne kontrol edilecek]

### Görev 2 — [isim]
[görev açıklaması]
Başarı kriteri: [ne kontrol edilecek]
```

Bu paket `AGENTS.md` → ilgili ajan bölümüne eklenir.
