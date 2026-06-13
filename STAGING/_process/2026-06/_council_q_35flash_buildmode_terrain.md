# Council — ax Gemini 3.5 Flash (High) — LEAN / SHIP-FAST / OVER-ENGINEERING-CRITIQUE lens

LENS (sen = ax 3.5 Flash): en yalin yol + asiri-muhendislik elestirisi. "Bunu yapmasak ne kaybederiz?" Demo-degeri vs efor. Acimasizca kes.

## RIMA BAGLAM
Unity 2D top-down 3/4 ARPG. Iso cellLayout (fake-iso), 64px kare tile, URP 2D + Pixel Perfect (PPU 64). Oda-bazli ARPG (RTS DEGIL). Build Mode: P1 BITTI (quote -> kamera zoom-out + Build sekmesi). P2 devam (prop palette + iso validity ghost + LMB/RMB + rotate + undo, Grid API'den). Planli: P3 tile/walkability brush, P4 light+auto-scatter+save/load, P5 selection. Sunum ~1 HAFTA, in-editor. Calisan demo riske atilmamali.

## REFERANS TEKNIKLERI (videolardan)
- REF1 (Amine Rehioui): Runtime RTS map editor, organik firca terrain (tile-grid degil), world-space dokulu zemin, az-GameObject perf.
- REF2 (Orb): Tileset yerine world-space texture terrain, dairesel firca, organik grass/dirt/water harman, pixel-art shader.

## YANITLA (4) — YALIN/ACIMASIZ
1) Bu terrain teknikleri RIMA icin GERCEKTEN gerekli mi yoksa "havali ama gereksiz" mi? Mevcut tile sistem sunum icin yeterli mi?
2) Az-GameObject terrain = RIMA olceginde cozulmus-problem mi (Tilemap zaten chunk)? Bunu yapmak zaman israfi mi?
3) ~1 haftada EN YUKSEK demo-degeri/efor ne? Build Mode'un hangi parcasi "wow" yaratir, hangisi gorunmez altyapi? Firca-terrain'i demo icin ATLAYIP P2-P5 prop/light/save'e odaklanmak daha mi akilli?
4) Eger illa bir sey alinacaksa: EN UCUZ, en az-riskli, P1-P2'yi BOZMAYAN, AYRI opsiyonel "nice-to-have" hangisi? (or. validity ghost'a yumusak highlight, ya da basit tek-katman tile-paint brush — full world-space shader DEGIL)

Cikti: kisa net durus + AL/POST-DEMO/ATLA tablosu + neden (yalin gerekce).
