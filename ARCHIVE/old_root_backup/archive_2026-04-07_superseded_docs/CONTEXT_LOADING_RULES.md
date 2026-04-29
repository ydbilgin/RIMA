# CONTEXT_LOADING_RULES.md
> **Ne zaman yükle:** Session başında ne okuyacağını belirlerken, veya AI'a "ne oku" talimatı verirken.
> **Ne zaman yükleme:** Kod yazarken, asset üretirken, rutin işlerde.

---

## TEMEL KURAL

Her session'da **sadece 2 dosya** zorunlu okunur:
1. `README.md` — projeyi tanı
2. `CURRENT_STATUS.md` — nereden devam ediyoruz

Geri kalan her şey **ihtiyaca göre açılır**.

---

## SESSION TİPİ → YÜKLENECEK DOSYALAR

### Genel / "Nereden kaldık?" session'ı
```
README.md + CURRENT_STATUS.md
```

### Unity / Kod işi
```
README.md + CURRENT_STATUS.md + AGENTS.md
```
İhtiyaç olursa: `RIMA/Assets/Scripts/[ilgili script]`

### PixelLab art üretimi
```
README.md + CURRENT_STATUS.md + TASARIM/STYLE_BIBLE.md + ART/PIXELLAB_PIPELINE_DECISION.md
```
İhtiyaç olursa: `ART/PIXELLAB_TIER2_CAPABILITIES.md` (limit/maliyet) · `ART/PRODUCTION_BACKLOG.md` (ne üretilecek) · `ART/ASSET_LOG_TEMPLATE.md` (kayıt formatı)

### Tasarım kararı / yeni sistem
```
README.md + CURRENT_STATUS.md + TASARIM/GDD.md
```
İhtiyaç olursa: ilgili TASARIM/ modülü

### Cross-class / skill sistemi
```
README.md + CURRENT_STATUS.md + TASARIM/MASTER_SINIF_VE_CROSSCLASS.md
```

### Mob/düşman tasarımı
```
README.md + CURRENT_STATUS.md + TASARIM/MOB_TASARIMI.md
```

### Görev yönlendirme / devir kararı
```
TASK_ROUTING_RULES.md + TASK_LABELS.md
```
İhtiyaç olursa: `DELEGATION_TEMPLATE.md`

### Dokümantasyon yönetimi
```
DOC_AUDIT.md + DOC_RESTRUCTURE_PLAN.md
```

---

## ASLA VARSAYILAN OLARAK YÜKLEME

Bu dosyalar sadece açıkça istenirse açılır:

- `GELISTIRME_PLANI.md` — faz roadmap, genel bağlam için gerek yok
- `TASARIM/GDD.md` — tasarım kararı gerekmiyorsa yükleme
- `TASARIM/LOKALIZASYON_VE_HİKAYE.md` — Faz 3+ içeriği
- `TASARIM/BOSS_SISTEMI.md` — Faz 2+ içeriği
- `TASARIM/MASTER_SINIF_VE_CROSSCLASS.md` — sadece cross-class işlerde
- `ART/_ARSIV/` altındaki her şey — arşiv, dokunma
- `TASARIM/arsiv/` ve `TASARIM/arastirma/` — arşiv/araştırma, dokunma
- `DOC_AUDIT.md`, `DOC_RESTRUCTURE_PLAN.md` — doc yönetimi dışında yükleme

---

## MODÜL AÇMA KURALI

```
Soru: Bu bilgiyi bulmak için hangi dosyayı açmam lazım?
→ CONTEXT_LOADING_RULES.md'ye bak
→ O dosyayı aç, sadece onu

Değil: README + GELISTIRME_PLANI + GDD + MOB_TASARIMI hepsini birden yükle
```

---

## SESSION SONU KURALI

Her session sonunda `CURRENT_STATUS.md` güncellenir:
- Biten iş → "✅" ile işaretle veya sil
- Yeni bilgi → ekle
- Asla 60 satırı geçme
