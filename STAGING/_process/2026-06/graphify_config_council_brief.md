# COUNCIL SORUSU — graphify hangi config? (2x2 deney sonucu)

## Bağlam
RIMA = Unity 2D top-down ARPG (C#). Bitirme demosu ~20 Haziran (≈6 gün). graphify = bir klasoru bilgi grafigine ceviren arac (AST yapisal kenarlar BEDAVA+deterministik; uzerine LLM subagent'lari "semantic" kenar ekler: INFERRED/AMBIGUOUS + semantically_similar_to/conceptually_related_to/rationale_for = "degisken" katman). graphify'in satilik degeri = bariz-olmayan capraz baglantilar.

2x2 deney: ayni 3 dosya (reward->draft alt-sistemi: DraftManager.cs + ChestBehavior.cs + RewardPickup.cs, ~1083 satir) uzerinde sadece SEMANTIC extraction calistirildi (AST haric = 4 config'de sabit oldugu icin). Amac: model (Sonnet/Opus) x mode (normal/deep) hangisi "daha uygun" (gecerli-kenar basina maliyet).

## Ham metrikler
| Config | node | edge | EXTR | INFER | AMBIG | deger-kenar | reward->draft |
|---|---|---|---|---|---|---|---|
| normal+sonnet | 41 | 46 | 44 | 2 | 0 | 2 | YES |
| normal+opus | 31 | 39 | 33 | 6 | 0 | 5 | YES |
| deep+sonnet | 57 | 82 | 62 | 16 | 4 | 20 | YES |
| deep+opus | 38 | 53 | 39 | 13 | 1 | 7 | YES |

Token: hepsi ~50-65k (yakin). Fiyat: Opus ~5x Sonnet/token. 4 config DE reward->draft kenarini yakaladi.

## Kalite analizi (yargi-kenarlarini elle okudum)
- **deep+sonnet'in "20 deger-kenari" SISIRILMIS:** 5 kenar = ayni dis sembolu (SkillDatabase, PlayerClassManager, PlayerEconomy, Health, RuntimeRoomManager) HER DOSYADA ayri node yapip 0.95 "similar" diye baglamis -> dogrusu TEK paylasilan node; mimari gurultu. + 1 cesilkili AMBIGUOUS dup. Yani en yuksek sayi ama en kotu kalite/kenar.
- **deep+opus:** dis sembolleri DOGRU konsolide etti (shared_* tek node). Tek gercek "surpriz" kenari bu buldu: BuildChestOffers ~ MaybeInjectEchoOffer (sandik-odulu <-> Echo offer enjeksiyonu) = DOGRUDAN F2 bug'i (Echo reward->draft karti cikmiyor).
- **normal+opus:** 4 kenar, hepsi mantikli, muhafazakar, yuksek sinyal.
- **normal+sonnet:** 2 kenar, minimal ama dogru, en ucuz.

## Orchestrator on-karari (council critique edecek)
- **Mimari harita** (F1/F2 yapisal, tum codebase): **normal+sonnet** -> AST yapiyi bedava verir, Sonnet ucuz semantic ekler, kilit kenarlar cikar, Opus primi degmez.
- **"Surpriz"/tasarim-bilgi grafigi** (kucuk secili korpus): **deep+opus** -> tek dogru-konsolide + gercek-icgorulu config.
- **Ele:** deep+sonnet (gurultu-siskin).

## Council'e sorular
1. Mimari-harita icin normal+sonnet GERCEKTEN yeterli mi, yoksa Opus/deep sart mi?
2. deep+opus'un 5x maliyeti tasarim-bilgi grafigi icin haklı mi, yoksa normal+opus yeter mi?
3. deep+sonnet'i elemek dogru mu?
4. TEK config mi (basitlik) yoksa amaca-gore IKILI mi (normal+sonnet arch / deep+opus design) onerirsiniz? Demo'ya 6 gun kala simdi tam-run mantikli mi, yoksa post-demo mu?

Kisa, net, gerekce-li cevap verin.
