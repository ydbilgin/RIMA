ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA Map Designer'ı (RIMA/Map Designer menüsü, `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs` + ilgili) "işlevsel ve güzel" hale getirme kararı için KOD/FEASIBILITY envanteri. ANALYSIS ONLY — kod değişikliği YOK.

# Bağlam
Map Designer 7-sekmeli unified araç olarak yazıldı (eski RoomData dönemi). O zamandan beri oyun RoomTemplateSO + IsoRoomBuilder + 26 template (Assets/Data/Rooms, Generated dahil) + BridsonPoissonAutoPlacer prop sistemi + checker zemin + overlay tilemap'e geçti. Yarın kullanıcı sunumda Map Designer'la CANLI oda düzenleme göstermek istiyor.

# Sorular (file:line kanıtıyla)
1. Map Designer'ın ŞU ANKİ gerçek durumu: 7 sekme neler, hangileri çalışıyor, hangileri eski sisteme (RoomData/eski registry) bağlı, hangileri RoomTemplateSO'dan habersiz?
2. RoomTemplateSO entegrasyon boşluğu: Designer mevcut 26 template'i AÇABİLİYOR mu, düzenleyip KAYDEDEBİLİYOR mu? Yoksa ayrı bir veri yolunda mı? Gap listesi.
3. En yüksek ROI iyileştirmeler (S/M boyutlu, bir gecede cx'in yapabileceği): örn. template load/save sekmesi, floor/hole boyama → walkable mask güncelleme, prop ekleme/silme + auto-placer butonu, IsoRoomBuilder canlı önizleme (Room Browser entegrasyonu), kapı düzenleme. Her birinin gerçek boyutu + dokunulacak dosyalar.
4. Riskler: hangi sekmeler kırılgan/test kapsamında (UnifiedDesignerTests), neye dokunmak regresyon riski?
5. Önerilen kesim: "yarın sunumda etkileyici + güvenli" minimum set.

# Çıktı
CODEX_DONE'a: sekme envanteri tablosu + gap listesi + ROI-sıralı iyileştirme önerileri (boyut+dosyalar) + risk notları. Prior audit reproduce etme.
