---
status: REFERENCE
faz: 1
tarih: 2026-04-17
ozet: "Mob kompozisyon ve yerleştirme kuralları"
---
# RIMA — Mob Kompozisyon Kurallari
**Status: v1 LOCKED — 2026-05-09**

## Kural 1 — M06 + M04 Birliktelik Yasagi

**Relic Caster (M06) + Penitent Bruiser (M04) ayni odada YASAK — F1 katinda.**

Sebep: M06'nin Aegis'i + M04'un Anti-Heal aurasi birlesince priority conflict yaratir.
Oyuncu "hangi threat once?" sorusunu F1'de henuz bilmiyor.

**F2/F3 istisnasi:** Bu kombinasyon "advanced room" olarak F2/F3'te izinli.
Kural: Composition tag → `advanced_priority_conflict`, sadece depth 3+ odalarda secilir.

## Kural 2 — M08 + M04 Birliktelik Yasagi

**Hollow Hulk (M08) + Penitent Bruiser (M04) ayni odada YASAK — tum katlarda.**

Sebep: M08 charge/deprem AoE'si oyuncuyu merkeze sikistirir. M04 aurasi iyilesme engeller.
Arena dar + sustain yok = oyuncu kacis yolu bulamiyor. Damage + sustain denial birlesimi her katman icin too hard.

Istisna: Boss odasi degerlendirmesi ayri — arena buyukse (30x30+) degerlendirilebilir.

## Kural 3 — Elite Oda M08 Zorunlulugu

**Elite odalar M08 icermek ZORUNDA DEGIL.** Alternatif elite kombo ornegi:
- 2xM06(4pt) + 2xM04(4pt) + 2xM01(1pt) = 18pt — YASAK (Kural 1 ihlali, F1)
- 2xM07(3pt) + 2xM05(4pt) + 2xM02(3pt) = 20pt — gecerli elite (M08 yok)

Kural: Elite oda tanimi "zorlu kompozisyon", sadece M08 degil.

## Kural 4 — M07 Telegraph Trainer

**M07 Riftbound Augur:** Augur saldirilari 1.2s belirgin alan telegraph birakir.
Oyuncu bu pencerede dash etmezse +%50 hasar alir.

Tasarim amaci: Act 1'de "telegraph okuma + reaktif dodge" kalibini ogretmek.
Bu, Act 1 Boss (Penitent Sovereign) Faz 2 mekanikleri icin hazirlik.

Mevcut tasarim (sadece debuff) bu rolu karsilamiyor — guncelleme bekliyor.
Status: **TODO v1 — Augur tanimi guncellenmeli.**

## Versiyonlama

| Versiyon | Tarih | Degisim |
|---|---|---|
| v1.0 | 2026-05-09 | Ilk kompozisyon kurallari (Opus denge review) |

