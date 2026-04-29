#!/usr/bin/env python3
"""
Ollama Araştırma — Standart Şablon
===================================
Yeni bir araştırma için bu dosyayı kopyala, BOLUMLER listesini doldur, çalıştır.

Kullanım:
  python ollama_arastirma.py

Çıktı:
  CIKTI_DOSYASI ve HAM_VERI_DOSYASI değişkenlerine istediğin yolu ver.
  Çıktılar her zaman proje klasörüne kaydedilir, bu şablon değişmez.
"""

import sys, requests, os
from datetime import datetime

sys.stdout.reconfigure(encoding='utf-8')

# ─── AYARLAR ──────────────────────────────────────────────────────────────────

OLLAMA_URL = "http://localhost:11434/api/generate"
MODEL      = "deepseek-r1:14b"          # Kullandığın modeli buraya yaz

# Çıktılar — her proje kendi klasörüne
CIKTI_DOSYASI    = r"C:\cikti_yolunu_degistir.md"
HAM_VERI_DOSYASI = r"C:\cikti_ham_yolunu_degistir.md"

# ─── GPU AYARLARI (RTX 5080 için optimize) ────────────────────────────────────
#
# SORUN: RTX 5080 (Blackwell SM 12.0) + AMD iGPU birlikte varsa Ollama CPU'ya düşer.
# ÇÖZÜM: Aygıt Yöneticisi → AMD Radeon(TM) Graphics → Devre Dışı Bırak
# Sonra Ollama'yı yeniden başlat.
#
# num_ctx:  4096  → KV cache ~1.6 GB → 16 GB VRAM'a rahatça sığar
#                   (16384 kullanma: model+KVcache toplam 25 GB olur, VRAM taşar → CPU)
# num_gpu:  99    → Tüm katmanları GPU'ya zorla (Ollama bazen kendisi offload eder)
# num_predict: 8000 → Bölüm başına maksimum token (kesilmemesi için)
# timeout:  1200s → Bölüm başına 20 dakika (reasoning modeller uzun düşünür)

GPU_OPTIONS = {
    "temperature":    0.7,
    "top_p":          0.9,
    "num_predict":    8000,
    "num_ctx":        4096,
    "num_gpu":        99,
    "repeat_penalty": 1.05,
}

# ─── FORMAT (isteğe bağlı) ────────────────────────────────────────────────────

FORMAT_ONEKI = """Structure your response in EXACTLY TWO PARTS:

### PART 1 — RAW DATA
List ALL facts, names, numbers in TABLE or BULLET format. No analysis.

### PART 2 — ANALYSIS
Design lessons, adaptations, notes.

Now answer:
---
"""

# ─── BÖLÜMLER — BURAYA YAZ ────────────────────────────────────────────────────

BOLUMLER = [
    {
        "baslik": "BOLUM 01 — Örnek Başlık",
        "prompt": """
Burada araştırma sorusunu yaz.
Detaylı ol — modele ne istediğini net söyle.
"""
    },
    # {
    #     "baslik": "BOLUM 02 — ...",
    #     "prompt": """..."""
    # },
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
    print(f"  Model: {MODEL} | Bölüm: {toplam}")
    print(f"  Tahmini süre: {toplam*5}–{toplam*12} dakika")
    print(f"  Başlangıç: {datetime.now().strftime('%H:%M')}")
    print("="*65)

    with open(CIKTI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Araştırma Çıktısı\n")
        f.write(f"*Model: {MODEL} | {datetime.now().strftime('%Y-%m-%d %H:%M')} | {toplam} bölüm*\n\n---\n\n")

    with open(HAM_VERI_DOSYASI, "w", encoding="utf-8") as f:
        f.write(f"# Ham Veri\n*Sadece PART 1 | {datetime.now().strftime('%Y-%m-%d %H:%M')}*\n\n---\n\n")

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
    print("="*65)


if __name__ == "__main__":
    main()
