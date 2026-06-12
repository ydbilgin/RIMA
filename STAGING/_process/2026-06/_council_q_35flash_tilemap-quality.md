# Council Question — RIMA Tilemap Visual Quality (PRAGMATIC / lean / ship-fast lens)

Sen ax Gemini 3.5 Flash High'sın. Mercek = EN YALIN YOL + OVER-ENGINEERING ELEŞTİRİSİ. Deadline'daki bir demo için "en az işle en çok görsel kazanç" neyse onu söyle. Süslü mimari değil, ship-fast. Kısa, madde madde, acımasızca pratik.

## KONU
Kullanıcı bir referans Unity 6 projesinde gördüğü çok güzel ZENGİN İZOMETRİK fantasy tilemap'leri RIMA'ya uyarlamak istiyor (Asset Store tileset + Animated Tile su + Animator meşale + autotile + büyük el-authored harita). RIMA demo-finalizasyon fazında, deadline var.

## RIMA KİLİTLERİ (saygı duy; ihlal öneriyorsan "[LOCK-RİSK]" işaretle)
- room-based roguelite (açık-dünya DEĞİL). Deadline.
- S59: HIGH TOP-DOWN 3/4. "NO iso, NO 45° diamond." Referans TRUE ISO → çakışıyor.
- PixelLab-only (asset-store yasak gibi; ama PixelLab tileset/animate araçları var).
- 32×32 tile, PPU=64, URP 2D + Pixel Perfect + 2D Lights.

## KOD GROUND-TRUTH
- Live floor = düz tek tile, autotile yok.
- Repo'da dormant Wang/layered painter VAR (FloorWangResolver, LayeredRoomPainter, CornerWangPainter, CliffYSortManager...) ama demo'ya bağlı değil; ne kadar bitmiş bilinmiyor.
- Props/dekorasyon sistemi ~%80 var ama live'a bağlı değil; plan yazıldı.
- ParallaxLayer.cs var ama kullanılmıyor.

## SORULAR (acımasız pragmatik)
1. Bu görsel kalitenin **en ucuz %80'i** neyle gelir? (Hangi 1-2 şey en çok fark yaratır, gerisi over-engineering mi?)
2. Dormant Wang/layered painter'ı bağlamak = **tatlı kaçış mı, yoksa bitmemiş-kod-bataklığı riski mi?** Deadline'da buna girmeli mi, yoksa düz-tile + dekorasyon + ışık yeter mi?
3. Animated su/meşale: deadline'da değer mi, yoksa "nice-to-have, demo sonrası" mı? En yalın ekleme yolu?
4. ISO'ya geçmek: tek kelime — deadline'da yapılır mı? (Hayırsa neden, evetse şartı.)
5. Eğer 1 günün/3 günün olsa SADECE neyi yaparsın (görsel kazanç sırası)? Neyi KESİNLİKLE yapmazsın (deadline tuzağı)?

Kapanışta: "deadline'daki demo için yapılacak TEK şey" + "sakın yapma" listesi (1-2 madde).
