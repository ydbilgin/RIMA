# [ARCHIVE] Warblade Idle Animasyon — PixelLab + Aseprite Guide

**Temel kural:** Tüm keyframe'ler PixelLab'dan gelir. Aseprite sadece interpolate eder, manuel çizim yok.

Kaynak: `Characters/Warblade/base/warblade_base_[yön].png`
Çıktı: `Characters/Warblade/idle/warblade_idle_[yön]_anim.png`
Sıra: `S → SE → E → NE → N → NW → W → SW`

---

## Adım 1 — PixelLab: İkinci keyframe'i üret

Her yön için:
1. PixelLab → **Edit Image** (veya Create from Reference)
2. Input: `warblade_base_[yön].png`
3. Prompt: aynı karakter, hafif ağırlık kayması — omuzlar 1-2px aşağı, kılıç hafifçe öne eğik, başka hiçbir şey değişmez
4. Size: 128px | AI Freedom: 0 | Preset: male human
5. Kaydet: `warblade_idle_[yön]_B.png`

→ Artık elimizde **2 PixelLab frame** var: `base` (A) + `B`

---

## Adım 2 — Aseprite: Interpolate

1. Aseprite'ta yeni 128×128 dosya aç
2. Frame 1'e `warblade_base_[yön].png` (A) → kopyala/yapıştır
3. Frame ekle → Frame 2'ye `warblade_idle_[yön]_B.png` → kopyala/yapıştır
4. Frame 1 → sağ tık → **Frame Properties** → **Loop Start**
5. Frame 2 → sağ tık → **Frame Properties** → **Loop End**
6. Üst menü: **Cel → Interpolate Cels** → Between frames: **2** → OK
7. Sonuç: **4 frame** — `A → I1 → I2 → B`
8. Tüm frameleri seç → **Frame Duration: 120ms**

---

## Adım 3 — Export

- **File → Export Sprite Sheet** → Layout: Horizontal Strip
- Kaydet: `Characters/Warblade/idle/warblade_idle_[yön]_anim.png`

---

## QC Kontrol

- [ ] A ve B arasında fark var (B'de omuz/kılıç hafif aşağı)
- [ ] Loop seamless (B → A geçişi boşluksuz)
- [ ] Kılıç omuzda kaldı, siluet bozulmadı
