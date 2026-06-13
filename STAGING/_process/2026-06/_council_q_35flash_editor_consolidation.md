# Council — ax Gemini 3.5 Flash (High) — LEAN / SHIP-FAST / ASIRI-MUHENDISLIK lensi

LENS (sen=ax 3.5 Flash): en yalin guvenli yol + asiri-muhendislik elestirisi. Demo ~1 hafta, kullanici sinirli ("cok kotu" dedi). Calisani BOZMA.

## Durum
RIMA'da 3 ORTUSEN map editoru cakisiyor: (1) ONCEDEN VAR `DevTools/InPlayMapPaintOverlay.cs` (F2, IMGUI cirkin, ama KAYDEDER + Editor ile entegre), (2) BENIM `UI/BuildMode/*` (uGUI guzel, prop/tile/walkability, oyun-UI gizler, undo; kayit yok; az once F2'ye baglandi -> CAKISMA), (3) Editor `UnifiedMapDesigner` (edit-mode). Kullanici: tek guzel + sonsuz-map + TEST-korumali editor; "tekrar bu sorunla karsilasmayalim".

## YANITLA (4) — YALIN/ACIMASIZ
1) KONSOLIDASYON: 3 araci tek'e indirmenin EN YALIN, en az-riskli yolu ne? "uGUI front + UnifiedDesignerCore back + eski IMGUI emekli" hakli mi, yoksa daha ucuz bir yol mu (or. sadece eski IMGUI'yi emekli et + benim Build Mode'a basit save ekle; ya da benim Build Mode'u emekli et + eski IMGUI'yi reskin)? Erken-soyutlama tuzagina dikkat.
2) TEST: kullanici "tekrar olmasin" diyor ama ASIRI test altyapisi da tuzak. EN YUKSEK deger/efor MINIMUM test seti ne? (hangi 4-6 test bu hata sinifini -F2 cakisma, keybind, overlap, placement, save/load- gercekten yakalar; gerisi asiri mi?) "Tek keybind sahiplik guard'i" basit mi tutulmali?
3) SONSUZ MAP: gercekten gerekli mi yoksa "buyuk ama sinirli" yeterli mi? Sonsuz-resize asiri-muhendislik mi?
4) TMP yazi bozulmasi: en ucuz mitigasyon ne (static atlas tek tikla mi, yoksa daha basit bir sey mi)?
+ Bu listede "calisani bozma" acisindan en riskli adim hangisi, nasil sirala?

Cikti: kisa yalin konsolidasyon + minimum test seti + AL/SONRA/ATLA (yalin gerekce).
