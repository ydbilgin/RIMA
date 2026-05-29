ACTIVE RULES: (1) think before answering (2) example-backed (3) actionable (4) UNSURE if unknown.
RESPOND INLINE (dosyaya yazma).

# GÖREV — W1b map sistemi UX/feel review (design)
Opus, RIMA'nın "harita parçası / map / gate" sistemini bağladı (`MapProgressController.cs` + mevcut `MapPanelUI` Ashen Glyph parşömen). Mekanik:
- 5-oda linear path grafiği (3 Combat → Rest/reward → Boss), cyan node + bağlantı, current-room highlight, visited dimming.
- **Fragment reveal:** sadece "current + 1 sonraki oda" tipi görünür (isRevealed), ötesi gizli — oyuncu ilerledikçe açılır.
- Oda geçişinde harita **2.2s flash** (reveal beat) + **M** ile tam görünüm.
- Gate = (henüz) prosedürel placeholder; rift-seam görseli W1b polish.

Değerlendir (örnekli — StS/Hades/Dead Cells map UX):
1. "current + next reveal" + transition-flash + M-toggle — bu demo için DOĞRU map UX mi? StS gibi tüm path mi göstermeli, yoksa bu progressive-reveal daha mı iyi (RIMA "parçalanmış dünya" temasına uygun)?
2. Flash 2.2s oyunu kesiyor mu, yoksa juice mi? Süre/tetik doğru mu?
3. Map'i ANLAMLI yapmak için: next-room "danger hint" (örn "Boss: Rift damage") eklemeli mi? Başka ucuz anlamlandırma?
4. Gate=rift-seam görseli ne kadar kritik (W1b'de mi yapılmalı, sonraya mı)?

# ÇIKTI (inline)
(A) SENTEZ: map UX onay/düzeltme + en kritik 2-3 öneri.
(B) HAM: örnek oyun map-UX verileri, sayılar, referans.
