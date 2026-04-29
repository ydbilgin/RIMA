# MEMORY_HYGIENE.md
> **Ne zaman yükle:** Dokümantasyon yönetimi session'ında, veya "memory nasıl çalışıyor" sorusu geldiğinde.
> **Ne zaman yükleme:** Normal geliştirme session'larında.

---

## DOKÜMANLARın SORUMLULUKLARI

| Dosya | Sorumluluk | Max Boyut |
|---|---|---|
| `README.md` | Proje tanımı + okuma haritası | 50 satır |
| `CURRENT_STATUS.md` | Şu anki odak + sıradaki görevler | 60 satır |
| `AGENTS.md` | Ajan kuralları + görev etiketleri | 80 satır |
| `GELISTIRME_PLANI.md` | Faz roadmap (özet) | 150 satır |
| `TASARIM/GDD.md` | Oyun tasarımı hakikati | sınırsız |
| `TASARIM/*.md` modüller | Spesifik konu detayı | sınırsız |

---

## TEKİL KAYNAK KURALI

**Aynı bilgi yalnızca bir yerde olur.**

- Boyut tablosu → `TASARIM/GORSEL_YONERGE.md` (README'den çıkar)
- Faz roadmap → `GELISTIRME_PLANI.md` (GDD'den çıkar)
- Mevcut odak → `CURRENT_STATUS.md` (README'den çıkar)
- Mob stats → `TASARIM/MOB_TASARIMI.md` (başka yerde yok)
- PixelLab promptları → `TASARIM/SANAT_PROMPTLARI.md` (başka yerde yok)

---

## README KİRLENME İŞARETLERİ

README güncellenmesi gerekiyor eğer:
- SON DURUM bölümü 5'ten fazla madde içeriyorsa → CURRENT_STATUS'a taşı
- Herhangi bir teknik detay içeriyorsa → ilgili modüle taşı
- Tablo veya kod bloğu içeriyorsa → ilgili modüle taşı
- 50 satırı geçiyorsa → temizle

---

## CURRENT_STATUS KİRLENME İŞARETLERİ

CURRENT_STATUS temizlenmesi gerekiyor eğer:
- Biten işler "✅" olarak 1 haftadan fazla duruyorsa → sil
- Sıradaki görevler 5'ten fazla madde içeriyorsa → öncelik seç, geri kalan GELISTIRME_PLANI'na
- 60 satırı geçiyorsa → temizle

---

## ARŞİV KURALI

Arşivlenen dosyalar `_ARSIV/` klasörüne taşınır.
- Silinmez — ileride referans olabilir
- Ama `CONTEXT_LOADING_RULES.md`'de "asla yükleme" listesinde
- Arşivlenen dosyanın içeriğini aktif dosyalarda tutma

---

## CLAUDE MEMORY (C:\Users\ydbil\.claude\...)

Claude'un kendi memory sistemi ayrıdır:
- Proje değişkenleri (hangi faz, hangi sprint) → `CURRENT_STATUS.md`'de tut, Claude memory'ye yazma
- Kalıcı tercihler (çalışma kuralları, stil) → Claude memory'ye yaz
- Kod yapısı → Claude memory'ye yazma, kodu oku

---

## HER SESSION SONU KONTROL LİSTESİ

```
[ ] CURRENT_STATUS.md güncellendi mi?
[ ] README 50 satırın altında mı?
[ ] Yeni bir modül oluşturulduysa CONTEXT_LOADING_RULES.md'ye eklendi mi?
[ ] Yeni ajan görevi varsa AGENTS.md'ye eklendi mi?
```
