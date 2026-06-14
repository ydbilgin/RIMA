# graphify config secimi — DERIN / MIMARI lens (Gemini 3.1 Pro High)

## Oku (inline degil, dosyayi ac)
STAGING/_process/2026-06/graphify_config_council_brief.md  — tum metrikler, kalite analizi, 4 soru orada.
Ham JSON: STAGING/_process/2026-06/graphify_exp/{normal_sonnet,normal_opus,deep_sonnet,deep_opus}.json

## Senin lensin: derin mimari + bilgi-grafigi degeri
graphify'in asil satilik degeri "bilmedigin capraz baglantiyi bulmak". Bu lensle degerlendir:

1. **Konsolidasyon farki onemli mi?** deep+opus dis sembolleri TEK paylasilan node yapti (shared_*); deep+sonnet her dosyada AYRI node + 0.95 "similar" kenar. Graf-kalitesi/community-detection acisindan bu fark ne kadar kritik? Sonnet'in deseni grafigi bozar mi (sahte yuksek-derece node'lar, yanlis community)?
2. **deep+opus'un buldugu BuildChestOffers ~ MaybeInjectEchoOffer kenari** gibi gercek-icgoru kenarlari, graphify'i kullanmanin TEK gercek gerekcesi mi? Eger oyleyse, sadece deep+opus bu degeri uretiyorsa, ucuz config'ler (normal+sonnet) "graphify yapmis olmak" disinda deger katiyor mu?
3. **Chunking kor-noktasi:** subagent her chunk'ta 20-25 dosya goruyor; farkli chunk'lardaki node'lar arasi semantic benzerligi HICBIR subagent goremiyor. Bu yapisal limit, model secimini anlamsiz kilacak kadar baskin mi? (yani "Opus sec" demek, chunk-arasi kayip yaninda onemsiz mi kaliyor?)
4. **Amaca-gore ikili strateji** (normal+sonnet=mimari harita / deep+opus=tasarim-bilgi grafigi kucuk korpus) dogru bir ayrim mi, yoksa over-engineering mi? Tek bir "dogru" config var mi?

Gerekce-li, mimari-derinlikli cevap ver. Kisa tut ama yargilarini savun.
