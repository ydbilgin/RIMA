# DOC_AUDIT.md
> **Ne zaman yükle:** Dokümantasyon reorganizasyonu yaparken. Normalde yükleme.
> **Ne zaman yükleme:** Asset üretimi, kod yazma, Unity işleri sırasında.

Tarih: 2026-04-02

---

## MEVCUT DOSYA ENVANTERİ

### Kök (2d roguelite/)

| Dosya | Satır | Durum | Eylem |
|---|---|---|---|
| `README.md` | ~200 | Log yığını haline gelmiş. SON DURUM bölümü çok uzun. | KÜÇÜLT → max 50 satır |
| `GELISTIRME_PLANI.md` | 898 | Faz 0 detayları, eski adım adım talimatlar içinde, yük. | KIRP → sadece faz özeti + kim ne yapar |
| `GOREVLER.md` | 144 | Kısmen eski. Birçok görev tamamlandı ama işaretlenmemiş. | CURRENT_STATUS ile birleştir |
| `KIRO_GOREVLER.md` | 116 | Tamamı stale — "henüz üretilmedi" dolu. | SİL → AGENTS.md'ye devret |
| `ANTIGRAVITY_LOGO_FIKIRLERI.md` | 63 | Antigravity çıktısı. Logo stratejisi. Geçici değer var. | ARŞİVLE |

### ART/

| Dosya/Klasör | Satır | Durum | Eylem |
|---|---|---|---|
| `ART/00-09_*.md` (10 dosya) | 1843 | Eski session raporları. Bilgiler README'ye taşınmış. | SİL |
| `ART/pixellab_arastirma_*.md` | 102 | Ham araştırma notları. GDD + pipeline dokümanlarında var. | SİL |
| `ART/_ARSIV_2026-04-02/` | 9 dosya | Arşivlenmiş ama hâlâ erişilebilir. | _ARSIV/ klasörüne taşı, erişme |
| `ART/_REVIEW/` | — | Aktif — review işleri burada. | KORU |
| `ART/karakterler/`, `dusmanlar/`, `environment/` | — | Aktif sprite klasörleri. | KORU |
| `ART_REDIRECT/README.md` | 64 | Genel AI art pipeline rehberi. RIMA'ya özgü değil. | ARŞİVLE — çok spesifik ihtiyaçta aç |

### TASARIM/

| Dosya | Satır | Durum | Eylem |
|---|---|---|---|
| `GDD.md` | 591 | Ana tasarım belgesi. İçeriğin çoğu doğru. | KORU — ama yükleme |
| `OYUN_YAPI_VE_ROADMAP.md` | 309 | GELISTIRME_PLANI + GDD ile %60 örtüşüyor. | KIRP veya GELISTIRME_PLANI'na yönlendir |
| `HIKAYE_VE_GAMEPLAY.md` | 245 | GDD Bölüm 16 ile örtüşüyor. | ARŞİVLE — GDD zaten var |
| `MASTER_SINIF_VE_CROSSCLASS.md` | 583 | Detaylı ve özgün. 28 kombo tablosu burada. | KORU — gerektiğinde aç |
| `MOB_TASARIMI.md` | 648 | Detaylı mob stats. Aktif referans. | KORU — gerektiğinde aç |
| `BOSS_SISTEMI.md` | 315 | GDD Bölüm 15'le örtüşüyor. | GDD'ye yönlendir, arsive al |
| `GORSEL_YONERGE.md` | 409 | PixelLab boyut tablosu + stil kuralları. Kısmen README'de var. | KIRP → README boyut tablosu kalsın, detaylar buraya |
| `SINIF_VE_SKILL_KARAR_BELGESI.md` | 324 | Karar geçmişi. Referans değeri var. | KORU — gerektiğinde aç |
| `SANAT_PROMPTLARI.md` | 285 | Aktif PixelLab/Gemini promptları. | KORU |
| `LOKALIZASYON_VE_HİKAYE.md` | 788 | Faz 3+ içeriği. Şu an erken. | ARŞİVLE |
| `YENI_FIKIRLER_ARASTIRMA.md` | 108 | Backlog fikirleri. | KORU — gerektiğinde aç |
| `KAYNAKLAR.md` | 183 | Link listesi. | KORU — gerektiğinde aç |
| `arsiv/` | 8 dosya | Eski versiyon dosyalar. | KORU (arsiv zaten) |
| `arastirma/` | 7 dosya + PDF | Araştırma materyalleri. | KORU (araştırma zaten) |
| `oyun_tasarim_sentez_master_plan.md` | 426 | GDD + OYUN_YAPI ile örtüşüyor. | ARŞİVLE |

---

## TEMEL SORUNLAR

1. **Tek kaynak yok** — Aynı bilgi README + GELISTIRME_PLANI + GDD + OYUN_YAPI'da tekrarlıyor
2. **Log kirliliği** — README SON DURUM bölümü 150+ satır, her session büyüyor
3. **Stale görevler** — GOREVLER.md ve KIRO_GOREVLER.md güncel değil
4. **Yükleme kuralı yok** — AI her session'da ne okuyacağını bilmiyor
5. **Ajan ayrımı yok** — Kiro/Antigravity/Claude görev sınırları belirsiz

## TOKEN İSRAFI HESABI

Mevcut "başlangıç okuması" (README + GOREVLER + KIRO):
- ~460 satır = tahminen ~6.000 token her session başında

Hedef:
- README (50 satır) + CURRENT_STATUS (50 satır) = ~1.500 token
- Gerektiğinde modüller açılır
