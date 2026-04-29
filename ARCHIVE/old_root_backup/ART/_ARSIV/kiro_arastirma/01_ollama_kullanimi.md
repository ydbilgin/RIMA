# Ollama Kullanımı — Araştırma Altyapısı

**Tarih:** 2026-04-02  
**Kaynak:** `F:\Antigravity Projeler\Ollama\`

---

## Ollama Nedir?

Ollama, yerel makinede LLM (Large Language Model) çalıştırmak için bir araçtır. API üzerinden modellere sorgu gönderebilirsin.

- **API Endpoint:** `http://localhost:11434/api/generate`
- **Varsayılan Model:** `deepseek-r1:14b` (reasoning model, derin analiz için)
- **Alternatif:** `qwen2.5:14b` (Türkçe çıktı daha iyi)

---

## Nasıl Kullanılır?

### 1. Şablon Script Kopyala

```bash
cp "F:\Antigravity Projeler\Ollama\ollama_arastirma.py" ./arastirma_adin.py
```

### 2. Script'i Düzenle

```python
# Çıktı dosyalarını proje klasörüne ayarla
CIKTI_DOSYASI    = r"F:\Antigravity Projeler\2d roguelite\kiro_arastirma\pixellab_arastirma.md"
HAM_VERI_DOSYASI = r"F:\Antigravity Projeler\2d roguelite\kiro_arastirma\pixellab_ham_veri.md"

# Model seç
MODEL = "deepseek-r1:14b"  # veya "qwen2.5:14b"

# Araştırma bölümlerini doldur
BOLUMLER = [
    {
        "baslik": "BOLUM 01 — PixelLab Animasyon Sistemleri",
        "prompt": """
PixelLab'da karakter animasyonu üretmenin tüm yöntemlerini araştır:
- animate-with-text vs animate-with-skeleton farkları
- Boyut limitleri (64px vs 256px)
- Frame sayısı limitleri
- Template animasyonlar vs custom animasyonlar
- MCP API vs Web UI farkları
- Kalite karşılaştırması

Tablo formatında özetle.
"""
    },
    # Daha fazla bölüm ekle...
]
```

### 3. Çalıştır

```bash
python arastirma_adin.py
```

**Süre:** Bölüm başına ~35 saniye (deepseek-r1:14b, RTX 5080)

---

## GPU Optimizasyonu (RTX 5080)

### Sorun: CPU'ya Düşme

**Belirti:** `ollama ps` çıktısında `89% CPU / 11% GPU`

**Neden:** AMD iGPU (Ryzen entegre grafik) Ollama'nın GPU keşfini bozuyor

**Çözüm:**
1. Aygıt Yöneticisi → Ekran bağdaştırıcıları
2. AMD Radeon(TM) Graphics → **Devre Dışı Bırak**
3. Ollama'yı kapat ve yeniden başlat

**Doğrulama:**
```bash
ollama ps
# Çıktı: 100% GPU olmalı
```

### Doğru GPU Ayarları

```python
GPU_OPTIONS = {
    "temperature":    0.7,
    "top_p":          0.9,
    "num_predict":    8000,   # Bölüm başına max token
    "num_ctx":        4096,   # ⚠️ 16384 YAPMA (VRAM taşar)
    "num_gpu":        99,     # Tüm katmanları GPU'ya zorla
    "repeat_penalty": 1.05,
}
```

| Ayar | Yanlış | Doğru | Neden |
|------|--------|-------|-------|
| `num_ctx` | 16384 | 4096 | 16384 → KV cache ~12 GB + model 9.7 GB = 25 GB → VRAM taşar |
| `num_gpu` | (yok) | 99 | Ollama bazen layer'ları CPU'ya atar, 99 bunu engeller |

---

## Çıktı Formatı

Script iki dosya üretir:

1. **Ana Çıktı** (`CIKTI_DOSYASI`): Tam analiz + ham veri
2. **Ham Veri** (`HAM_VERI_DOSYASI`): Sadece PART 1 (tablolar, listeler)

### Format Yapısı

```markdown
### PART 1 — RAW DATA
Tablo veya bullet formatında tüm gerçekler, isimler, sayılar.

### PART 2 — ANALYSIS
Tasarım dersleri, uyarlamalar, notlar.
```

---

## Ne Zaman Kullan?

| Durum | Ollama | Web Araştırması |
|-------|--------|-----------------|
| Derin analiz, karşılaştırma | ✅ | ❌ |
| Güncel API dokümantasyonu | ❌ | ✅ |
| Çok bölümlü araştırma (10+ soru) | ✅ | ❌ |
| Hızlı tek soru | ❌ | ✅ |

---

## Sistem Bilgisi

- **GPU:** RTX 5080 (16 GB VRAM, Blackwell SM 12.0)
- **Ollama:** 0.18.2
- **Model:** deepseek-r1:14b (~9.7 GB VRAM)
- **Hız:** ~35 saniye/bölüm (100% GPU)

---

## Örnek Kullanım

```bash
# 1. Script kopyala
cp "F:\Antigravity Projeler\Ollama\ollama_arastirma.py" ./pixellab_arastirma.py

# 2. Düzenle (BOLUMLER listesini doldur)
# 3. Çalıştır
python pixellab_arastirma.py

# Çıktı:
# - pixellab_arastirma.md (tam analiz)
# - pixellab_ham_veri.md (sadece tablolar)
```

---

*Bu belge Kiro tarafından 2026-04-02 tarihinde oluşturuldu.*
