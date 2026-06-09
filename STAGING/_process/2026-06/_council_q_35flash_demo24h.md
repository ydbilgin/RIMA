# RIMA jüri demosu — 24 saatte EN İYİ oynanabilir hale getirme (LEAN / SHIP-FAST / OVER-ENGINEERING-CRITIQUE lensi)

Sen Gemini 3.5 Flash (High) advisor'ısın. Lensin: EN YALIN yol + over-engineering eleştirisi + ne KESİLMELİ. 24 saatte ship etmeye odaklan. Kod yazma, ANALİZ.

## CONTEXT
RIMA = Unity 2D top-down ARPG roguelite. Hedef: jüriye gösterilecek, baştan sona OYNANABİLİR, hatasız dikey-slice demo. 24 SAAT var.

### Demo akışı
MainMenu → Chamber (Warblade/Elementalist seç) → Combat1 → Combat2 → Shop → Combat3 → Boss → Victory/Death.

### MEVCUT DURUM (kod-tamam + push'lu, insan-playtest YOK)
Softlock fix (76/76 test) · forced lineer sıra + sabit kamera (ortho 5.0) · 2-sınıf kilit · PauseMenu (10/10) · boss spawn+telegraph+Victory · Shop 3 stand (8/8) · kılıç sorting fix · 594 test, 0 yeni fail.

### GAP'LER (bilinçli yapılmadı)
- **ANIMASYONLAR:** Üretim ~0, çoğu hareket kod-driven. Sprite-anim minimal/YOK. PixelLab kurulu ama gated. Kullanıcı artık animasyonları birlikte üretmek istiyor.
- Sanat placeholder: Elementalist rune disc + shop stand = renkli kareler.
- Boss build-gap: prefab Resources'ta değil (editör çalışır, build çalışmaz). 1-satır fix.
- Legacy _IsoGame (post-demo).

### YENİ DİREKTİF
24 saat, en iyi oynanabilir demo. Animasyonları birlikte üreteceğiz. Animasyon scope içinde.

## SORULAR (yalın/ship-fast lensinden)
1. 24 saatte EN YALIN "best demo" yolu ne? Hangi işler ZAMAN YAKAR ve KESİLMELİ?
2. Animasyon TUZAĞI: 24h'de PixelLab ile çok asset üretmeye kalkmak over-engineering mi? Minimum kaç animasyon yeterli "cilalı" hissi verir? (örn: sadece Warblade idle+walk+attack + boss idle mı?)
3. Boss build-gap: 1-satır fix YAP mı yoksa editör-demo'da bırak (zaman yakma) mı?
4. "Playtest first" — kod-tamam ama test edilmemiş bir demoda EN YALIN doğrulama planı ne? Hangi 5 senaryo zorunlu?
5. Sanat placeholder ile ship etmek kabul edilebilir mi? Hangi placeholder MUTLAKA değişmeli, hangisi jüri fark etmez?
6. Over-engineering uyarısı: bu planda nereye gereksiz efor harcama riski var? 24h saat-programını YALIN tut.

Acımasız önceliklendir. "Kesilecekler" listesi ZORUNLU.
