#!/usr/bin/env python3
"""
PixelLab Araştırması — Karakter Animasyon Sistemleri
====================================================
96px karakterler için en kaliteli animasyon yöntemini bul.

Kullanım:
  python pixellab_arastirma.py

Çıktı:
  F:\Antigravity Projeler\2d roguelite\kiro_arastirma\
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

# ─── AYARLAR ──────────────────────────────────────────────────────────────────

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"

# Çıktılar
CIKTI_DOSYASI    = r"F:\Antigravity Projeler\2d roguelite\kiro_arastirma\pixellab_arastirma_cikti.md"
HAM_VERI_DOSYASI = r"F:\Antigravity Projeler\2d roguelite\kiro_arastirma\pixellab_arastirma_ham.md"

# ─── GPU AYARLARI ─────────────────────────────────────────────────────────────

GPU_OPTIONS = {
    "temperature":    0.7,
    "top_p":          0.9,
    "num_predict":    8000,
    "num_ctx":        4096,
    "num_gpu":        99,
    "repeat_penalty": 1.05,
}

# ─── FORMAT ───────────────────────────────────────────────────────────────────

FORMAT_ONEKI = """Structure your response in EXACTLY TWO PARTS:

### PART 1 — RAW DATA
List ALL facts, names, numbers in TABLE or BULLET format. No analysis.

### PART 2 — ANALYSIS
Design lessons, adaptations, notes.

Now answer:
---
"""

# ─── BÖLÜMLER ─────────────────────────────────────────────────────────────────

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — PixelLab Animasyon Yöntemleri Karşılaştırması",
        "prompt": """
PixelLab'da karakter animasyonu üretmenin TÜM yöntemlerini karşılaştır:

1. **Template Animations** (örn: walking-8-frames, running-slide)
2. **Animate with Text** (text prompt ile animasyon)
3. **Animate with Skeleton** (skeleton pose ile animasyon)
4. **Interpolation** (first/last frame arası doldurma)
5. **Pro Mode Character + Template** (şu an kullandığımız)

Her yöntem için TABLO formatında:
- Boyut limiti (min-max px)
- Frame sayısı (min-max)
- Maliyet (generation sayısı)
- Kalite (1-10 skala)
- Tutarlılık (1-10 skala)
- Smoothness (1-10 skala)
- Tier gereksinimi (1 veya 2)
- Ne zaman kullanılmalı

Sonra: 96px karakter için EN KALİTELİ yöntem hangisi?
"""
    },
    {
        "baslik": "BOLUM 02 — Interpolation Detayları",
        "prompt": """
PixelLab Interpolation özelliğini detaylı incele:

1. Boyut limiti gerçekten sadece 64x64 mi? Tier 2'de değişiyor mu?
2. Nasıl çalışır? (first frame + last frame → intermediate frames)
3. Kaç frame üretir? (2-20 arası mı?)
4. Template animasyonlarla birlikte kullanılabilir mi?
5. MCP API'den erişilebilir mi yoksa sadece Web UI'dan mı?
6. Kalite nasıl? (template animasyonlara göre)
7. 96px karakter için kullanılabilir mi? (workaround var mı?)

TABLO formatında özetle, sonra: "96px karakterler için interpolation kullanmalı mıyız?" sorusunu cevapla.
"""
    },
    {
        "baslik": "BOLUM 03 — Tier 1 vs Tier 2 Farkları",
        "prompt": """
PixelLab Tier 1 ve Tier 2 abonelik farkları:

1. Animasyon özellikleri (hangi tool'lar Tier 2'ye özel?)
2. Boyut limitleri (Tier 2'de daha büyük karakterler üretilebilir mi?)
3. Frame sayısı limitleri
4. Eş zamanlı job limiti (Tier 1: 8, Tier 2: ?)
5. Kalite farkı var mı? (aynı tool'u kullanınca)
6. Pro mode özellikleri (Tier 2'ye özel mi?)

TABLO formatında karşılaştır.

Sonra: "Tier 2 aboneliğimiz var, 96px karakterler için hangi yeni imkanlar açıldı?" sorusunu cevapla.
"""
    },
    {
        "baslik": "BOLUM 04 — 96px Karakter İçin Optimal Workflow",
        "prompt": """
Şu bilgileri göz önünde bulundur:

- Karakter boyutu: 96px
- Yön sayısı: 8 (south, south-west, west, north-west, north, north-east, east, south-east)
- Animasyonlar: idle, walk, run, dash, attack, death (6 animasyon)
- Tier 2 abonelik var
- Hedef: EN KALİTELİ, EN SMOOTH animasyonlar
- Maliyet: önemli değil (generation sayısı sınırsız değil ama kalite öncelikli)

ÜÇ FARKLI WORKFLOW ÖNER:

**Workflow A: MCP API (vibe coding)**
- Adımlar
- Maliyet (toplam generation)
- Beklenen kalite
- Artılar/eksiler

**Workflow B: Web UI (manuel)**
- Adımlar
- Maliyet
- Beklenen kalite
- Artılar/eksiler

**Workflow C: Hybrid (MCP + Web UI)**
- Adımlar
- Maliyet
- Beklenen kalite
- Artılar/eksiler

Sonra: "Hangi workflow'u önerirsin ve neden?" sorusunu cevapla.
"""
    },
    {
        "baslik": "BOLUM 05 — Template Animasyon Kalitesi",
        "prompt": """
PixelLab template animasyonları (walking-8-frames, running-8-frames, fight-stance-idle-8-frames, running-slide, lead-jab, falling-back-death) hakkında:

1. Frame sayısı gerçekten 8 mi? (isimde 8-frames yazıyor ama)
2. Smoothness nasıl? (1-10 skala)
3. Tutarlılık nasıl? (8 yön arasında stil tutarlı mı?)
4. Custom animasyonlara göre kalite farkı?
5. 96px karakterde iyi görünür mü?
6. Hangi template'ler en smooth?
7. Hangi template'ler en az smooth?

TABLO formatında özetle.

Sonra: "Template animasyonlar yeterli mi yoksa custom animasyon (interpolation/skeleton) gerekli mi?" sorusunu cevapla.
"""
    },
    {
        "baslik": "BOLUM 06 — MCP API Limitleri",
        "prompt": """
PixelLab MCP API üzerinden erişilebilen özellikler:

1. create_character (Pro mode) — boyut limiti?
2. animate_character (template) — hangi template'ler var?
3. Interpolation MCP'den erişilebilir mi?
4. Skeleton-based animation MCP'den erişilebilir mi?
5. Custom frame count ayarlanabilir mi?
6. MCP'den yapılamayan ama Web UI'dan yapılabilen özellikler neler?

TABLO formatında listele.

Sonra: "MCP API yeterli mi yoksa Web UI'a geçmeli miyiz?" sorusunu cevapla.
"""
    },
    {
        "baslik": "BOLUM 07 — Final Öneri",
        "prompt": """
Tüm bilgileri sentezle ve KESIN bir öneri ver:

**Durum:**
- 96px karakter (Warblade)
- 8 yön
- 6 animasyon (idle, walk, run, dash, attack, death)
- Tier 2 abonelik
- Hedef: EN KALİTELİ sonuç

**Soru:**
1. Hangi yöntemi kullanmalıyız? (MCP template / Web UI interpolation / Web UI skeleton / başka?)
2. Adım adım workflow nedir?
3. Tahmini maliyet (generation sayısı)?
4. Beklenen kalite (1-10)?
5. Alternatif yöntemler var mı?

KARAR TABLOSU formatında sun:
| Yöntem | Kalite | Maliyet | Süre | Zorluk | Öneri |
|--------|--------|---------|------|--------|-------|

Sonra: "Kesin önerim: [YÖNTEM] çünkü [NEDEN]" formatında cevapla.
"""
    },
]

# ─── ÇALIŞMA ──────────────────────────────────────────────────────────────────

def ollama_sor(prompt: str) -> str:
    payload = {
        "model": MODEL,
        "prompt": FORMAT_ONEKI + prompt,
        "stream": False,
        "options": GPU_OPTIONS,
    }
    try:
        r = requests.post(OLLAMA_URL, json=payload, timeout=1200)
        r.raise_for_status()
        return r.json().get("response", "").strip()
    except Exception as e:
        return f"HATA: {e}"


def ham_veri_cikar(metin: str) -> str:
    """PART 1 bölümünü ayır."""
    if "### PART 2" in metin:
        return metin.split("### PART 2")[0].strip()
    return metin


def main():
    toplam = len(BOLUMLER)
    print("\n" + "="*65)
    print(f"  PixelLab Araştırması")
    print(f"  Model: {MODEL} | Bölüm: {toplam}")
    print(f"  Tahmini süre: {toplam*5}–{toplam*12} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# PixelLab Araştırması — Karakter Animasyon Sistemleri\n\n")
        f.write(f"**Hedef:** 96px karakterler için en kaliteli animasyon yöntemi\n\n")
        f.write(f"**Model:** {MODEL}\n")
        f.write(f"**Tarih:** {datetime.now().strftime('%Y-%m-%d %H:%M')}\n")
        f.write(f"**Bölüm:** {toplam}\n\n")
        f.write("---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# PixelLab Araştırması — Ham Veri\n\n")
        f.write(f"*Sadece PART 1 (tablolar, listeler)*\n\n")
        f.write(f"**Tarih:** {datetime.now().strftime('%Y-%m-%d %H:%M')}\n\n")
        f.write("---\n\n")

    basari = 0
    for i, bolum in enumerate(BOLUMLER, 1):
        print(f"\n[{i:02d}/{toplam}] {datetime.now().strftime('%H:%M')} — {bolum['baslik'][:60]}")
        print(f"         Düşünüyor...", end="", flush=True)

        cikti = ollama_sor(bolum["prompt"])

        if cikti.startswith("HATA"):
            print(f" ✗\n         {cikti}")
        else:
            print(f" ✓  ({len(cikti):,} karakter)")
            basari += 1

        with open(CIKTI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{cikti}\n\n---\n\n")

        with open(HAM_VERI_DOSYASI, "a", encoding="utf-8") as f:
            f.write(f"## {bolum['baslik']}\n\n{ham_veri_cikar(cikti)}\n\n---\n\n")

    print(f"\n{'='*65}")
    print(f"  TAMAMLANDI: {basari}/{toplam} | {datetime.now().strftime('%H:%M')}")
    print(f"  Çıktı: {CIKTI_DOSYASI}")
    print(f"  Ham Veri: {HAM_VERI_DOSYASI}")
    print("="*65)


if __name__ == "__main__":
    main()
