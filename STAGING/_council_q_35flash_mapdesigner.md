# Soru: Map Designer iyileştirme — LEAN lens (bir gecede ne yapılır, ne yapılMAZ)

RIMA Unity projesi. Mevcut: 7-sekmeli eski "Map Designer" penceresi (eski RoomData sistemine bağlı) + YENİ veri yolu: RoomTemplateSO (26 oda) → IsoRoomBuilder (runtime iso ada inşası) + BridsonPoissonAutoPlacer (Poisson prop yerleşimi) + yeni Room Browser penceresi (tıkla→_Arena'da kur, dün gece yazıldı). Kullanıcı: "Map Designer'ı istediğimiz gibi işlevsel güzel hale getir" + yarın sunumda canlı oda düzenleme gösterecek.

Diğer danışman (3.1 Pro) önerisi: hibrit mimari (pencere=kontrol paneli + SceneView=tuval); Faz-1 (bu gece): canlı SO→rebuild bağı + "Generate Props + Randomize Seed" butonu + kapı toggler; Faz-2 (sonra): SceneView fırça, undo, flood-fill validatör, eski 7-sekme temizliği.

## Senin lensin: EN YALIN — kes/sırala/riski söyle
1. 3.1'in Faz-1'i bir cx gecesi için gerçekçi mi? Hangi parçası ilk atılır, hangi parçası olmazsa olmaz? (Sunduğu 4 maddeyi ROI sırasına koy, gerekirse kes.)
2. Mevcut Room Browser'ı BÜYÜTMEK (template seç + auto-placer butonu + seed + kaydet eklemek) vs yeni pencere yazmak vs eski 7-sekmeyi yamalamak — hangisi en az kod/en az risk? Tek öneri ver.
3. Kırılma riskleri: SO'ya yazma (AssetDatabase.SaveAssets, Undo), edit-mode rebuild performansı (26 oda büyükleri 599 hücre), mevcut UnifiedDesignerTests'i kırmadan çalışma.
4. "Güzel" için bu gece 3 küçük dokunuş (örn. thumbnail, dirty-yıldızı, buton yerleşimi) — fazlasını ertele.
5. Sunum provası riski: gece implement edilen araç sabah ilk kullanımda patlamasın diye cx hangi otomatik kanıtları üretmeli?

Türkçe, keskin, madde madde. "Hepsi olur" deme.
