---
status: REFERENCE
faz: 1
tarih: 2026-04-30
ozet: "Hasar hesaplama formülleri"
---
# RIMA — Hasar Hesaplama Politikasi
**Status: v1 LOCKED — 2026-05-09**

## Multiplier Kategorileri

Tum hasar multiplier'lari 3 kategoriye ayrilir:

| Kategori | Ornekler | Kural |
|---|---|---|
| **Identity** | Identity Passive bonusu, KEYSTONE aktif etkisi | En yuksek tam gecerli; ayni kategoride ikincisi %50 katki |
| **Build** | MODIFIER always-on, RESONANCE proc, OnDash bonusu | En yuksek tam gecerli; ayni kategoride ikincisi %50 katki |
| **Situational** | Break Window, Cross-Class Proc text, Aim-Shot vurusu | Kendi icinde toplamsal; kategori basina x2.0 cap |

## Tek Vurus Tavani

- **Identity + Build kategorileri toplami:** x3.0 baseline cap.
- Cap ustu hasar → Posture damage'e yonlendirilir (HP degil).
- **Outcome multiplier'lar** (Break Window +%50, Weak Spot x1.75) ayri kategori, x2.0 ayri cap.

## Ornekler

### Shadowblade Phase Step + STRIKE:
- Identity Passive (Scar Memory): +%25 → 1.25 (Identity)
- OnDash Proc: +%25 → 1.25 (Build) — farkli kategori, tam gecerli
- MODIFIER always-on: +%15 → 1.15 (Build) — ayni kategoride ikinci, %50 katki = +%7.5
- Toplam: 1.25 x 1.25 x 1.075 = ~1.68 (cap altinda)
- Break Window (+%50): ayri Situational, 1.68 x 1.50 = ~2.52 (cap altinda)

### NOT — Stacking Cap Uygulamasi:
Carpimsal hesap: (Identity x Build) — her kategori kendi icinde en yuksek + %50 digerler.
Toplam x3.0 asilirsa: asan fark Posture damage olur.

## v2 Notlari
- Weak Spot (v2 adayi): Point weak-spot x1.75 (aim siniflari), Zone weak-spot x1.40 (AoE siniflari) — ikisi ayri outcome.
- Sayisal poise meter (v2): posture damage formulu bu dokumani genisletir.

## Acik Sorular (yanit bekliyor)
1. Minyon vuruslarinin posture damage carpani → oneri: %50 (Summoner icin critical)
2. ZONE skill'lerin posture katkisi → oneri: %75 (alan hasari, tam deger degil)
3. Summoner minyon-HP posture hesabi → kararlastitirilmamis

