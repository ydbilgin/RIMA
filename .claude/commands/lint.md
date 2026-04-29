---
description: RIMA bilgi tabanı sağlık taraması — çelişki, stale entry, orphan memory tespit et ve temizlik listesi çıkar.
---

# /lint — RIMA Knowledge Base Sağlık Taraması

Bu komutu çalıştırırken şu adımları izle. Kullanıcıya sormadan, doğrudan tara ve raporla.

## Adım 1 — Index Yükle

Oku:
- `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md`
- `F:/Antigravity Projeler/2d roguelite/RIMA/CURRENT_STATUS.md`

## Adım 2 — Kritik Belgeleri Tara

Şu dosyaları oku (hepsini aynı anda parallel):
- `TASARIM/MASTER_KARAR_BELGESI.md`
- `TASARIM/FAZLAR/FAZ_MASTER.md`
- `TASARIM/ANIMATION_REDESIGN.md`
- `TASARIM/GDD.md` (sadece ilk 100 satır)

## Adım 3 — Çelişki Taraması

Şunları karşılaştır:

| Kontrol | Kaynak A | Kaynak B |
|---|---|---|
| Class roster (10 class?) | MEMORY: project_rima.md | MASTER_KARAR_BELGESI |
| Aktif faz | CURRENT_STATUS | FAZ_MASTER |
| Sprite sistemi (body+weapon ayrı?) | MEMORY: project_character_system.md | ANIMATION_REDESIGN |
| PixelLab pipeline durumu | MEMORY: project_pixellab_pipeline.md | MASTER_KARAR_BELGESI |
| Cross-class skill slotları (2 slot?) | MEMORY: project_cross_class_skills.md | GDD |

## Adım 4 — Stale Memory Tespiti

MEMORY.md'deki her entry için tarih ve içerik kontrolü:
- "TAMAMLANDI" denen ama CURRENT_STATUS'ta hâlâ aktif sayılanlar
- 2026-04 öncesi tarihli proje memoryleri — hâlâ geçerli mi?
- Birbirini tekrar eden/çakışan memory dosyaları

## Adım 5 — Orphan Kontrolü

MEMORY.md'de listelenen ama dosyası eksik olan entryleri tespit et (Read ile kontrol et).

## Adım 6 — Rapor Çıkar

Şu formatta özetle:

```
## LINT RAPORU — [tarih]

### 🔴 Çelişkiler (acil düzeltme)
- [A vs B: kısa açıklama]

### 🟡 Stale Entries (güncellenmeli)
- [memory dosyası: neden stale]

### 🟢 Orphan / Eksik Dosyalar
- [MEMORY.md'de var ama fiziksel dosya yok]

### ✅ Temiz
- [kontrol edildi, sorun yok]

### Önerilen Aksiyon
1. [yapılacak iş]
```

Raporu kullanıcıya göster, sonra "Hangisini düzeltelim?" diye sor.
