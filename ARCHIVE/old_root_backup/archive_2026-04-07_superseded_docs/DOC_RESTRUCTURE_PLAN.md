# DOC_RESTRUCTURE_PLAN.md
> **Ne zaman yükle:** Dokümantasyon değişikliği yaparken. Normalde yükleme.

---

## HEDEFLEDİĞİMİZ YAPI

```
2d roguelite/
├── README.md              ← Giriş noktası. max 50 satır. Okuma haritası.
├── CURRENT_STATUS.md      ← Devam noktası. max 60 satır. Her session buradan başla.
├── AGENTS.md              ← Ajan kuralları. Kim ne yapar, hangi model, SIMPLE/ADVANCED/CRITICAL.
├── GELISTIRME_PLANI.md    ← Faz roadmap. Adım adım talimatlar silindi. ~150 satır.
├── DOC_AUDIT.md           ← Bu dosya sistemini yönetmek için. (normalde yükleme)
├── DOC_RESTRUCTURE_PLAN.md← Bu plan. (normalde yükleme)
├── CONTEXT_LOADING_RULES.md← Hangi dosyayı ne zaman oku. (normalde yükleme)
├── MODEL_ROUTING.md       ← Sonnet/Opus karar kuralları. (normalde yükleme)
├── MEMORY_HYGIENE.md      ← Memory sistemi kuralları. (normalde yükleme)
│
├── TASARIM/               ← Tasarım modülleri — gerektiğinde aç
│   ├── GDD.md             ← Ana tasarım belgesi (koru)
│   ├── MASTER_SINIF_VE_CROSSCLASS.md (koru)
│   ├── MOB_TASARIMI.md    (koru)
│   ├── SANAT_PROMPTLARI.md(koru)
│   ├── SINIF_VE_SKILL_KARAR_BELGESI.md (koru)
│   ├── GORSEL_YONERGE.md  (koru — README boyut tablosu buraya taşınır)
│   ├── YENI_FIKIRLER_ARASTIRMA.md (koru)
│   ├── KAYNAKLAR.md       (koru)
│   ├── _ARSIV/            ← Buraya taşınacaklar (aşağıda liste)
│   ├── arsiv/             (mevcut, koru)
│   └── arastirma/         (mevcut, koru)
│
├── ART/
│   ├── _REVIEW/           (koru)
│   ├── karakterler/       (koru)
│   ├── dusmanlar/         (koru)
│   ├── environment/       (koru)
│   ├── skill_iconlari/    (koru)
│   └── _ARSIV/            ← Buraya taşınacaklar (aşağıda liste)
│
└── RIMA/                  ← Unity projesi (dokunma)
```

---

## ADIM ADIM EYLEMLER

### 1. SİL (geri dönüşüm sepeti yeterli)

```
ART/00_TAMAMLANDI_UNITY.md
ART/00_TEMIZLIK_RAPORU.md
ART/00_TEMIZLIK_TAMAMLANDI.md
ART/01_ollama_kullanimi.md
ART/02_session_ozeti.md
ART/03_FINAL_ONERI.md
ART/04_SESSION_OZETI_FINAL.md
ART/05_WARBLADE_DURUM.md
ART/06_FINAL_RAPOR.md
ART/07_ILERLEME.md
ART/08_TAMAMLANDI.md
ART/09_SESSION_FINAL.md
ART/pixellab_arastirma_cikti.md
ART/pixellab_arastirma_ham.md
KIRO_GOREVLER.md
```

### 2. ARŞİVLE (taşı, silme)

```
ANTIGRAVITY_LOGO_FIKIRLERI.md         → ART/_ARSIV/
ART_REDIRECT/README.md                → ART/_ARSIV/ART_REDIRECT_PIPELINE.md
ART/_ARSIV_2026-04-02/                → ART/_ARSIV/ (zaten arşiv, bırak)
TASARIM/HIKAYE_VE_GAMEPLAY.md         → TASARIM/_ARSIV/
TASARIM/BOSS_SISTEMI.md               → TASARIM/_ARSIV/ (GDD'de var)
TASARIM/LOKALIZASYON_VE_HİKAYE.md    → TASARIM/_ARSIV/ (faz 3+)
TASARIM/oyun_tasarim_sentez_master_plan.md → TASARIM/_ARSIV/
TASARIM/OYUN_YAPI_VE_ROADMAP.md      → TASARIM/_ARSIV/ (GELISTIRME_PLANI yeterli)
GOREVLER.md                           → TASARIM/_ARSIV/ (CURRENT_STATUS alacak)
```

### 3. KIRP (mevcut dosyaları küçült)

| Dosya | Mevcut | Hedef | Ne Çıkacak |
|---|---|---|---|
| `README.md` | ~200 satır | 50 satır | SON DURUM log → CURRENT_STATUS'a, boyut tablosu → GORSEL_YONERGE.md |
| `GELISTIRME_PLANI.md` | 898 satır | ~150 satır | Faz 0 adım detayları silinir, tamamlanan adımlar "✅ Tamamlandı" tek satıra indirilir |

### 4. OLUŞTUR (yeni dosyalar)

- `CURRENT_STATUS.md` — Mevcut odak, biten son iş, sıradaki 3 görev
- `AGENTS.md` — Claude Code / Antigravity / Kiro / PixelLab görev kuralları
- `CONTEXT_LOADING_RULES.md` — Hangi session'da ne okunur
- `MODEL_ROUTING.md` — Sonnet/Opus routing
- `MEMORY_HYGIENE.md` — Memory sistemi

---

## ÖNCE / SONRA TOKEN KARŞILAŞTIRMASI

| Senaryo | Önce | Sonra |
|---|---|---|
| Session başlangıcı | ~6.000 token | ~1.500 token |
| Unity iş session'ı | ~6.000 token | ~2.000 token (+ AGENTS) |
| Art üretim session'ı | ~6.000 token | ~2.500 token (+ SANAT_PROMPTLARI) |
| Tasarım kararı | ~8.000 token | ~3.000 token (+ GDD modülü) |

---

## UYGULAMA SIRASI

1. [x] DOC_AUDIT.md yaz
2. [x] DOC_RESTRUCTURE_PLAN.md yaz
3. [ ] CONTEXT_LOADING_RULES.md yaz
4. [ ] MODEL_ROUTING.md yaz
5. [ ] MEMORY_HYGIENE.md yaz
6. [ ] AGENTS.md yaz
7. [ ] **Onay al** → silme/taşıma işlemleri başlat
8. [ ] README.md küçült
9. [ ] CURRENT_STATUS.md oluştur
10. [ ] GELISTIRME_PLANI.md kırp
