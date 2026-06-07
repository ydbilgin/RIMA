# RIMA — Deep Static Repo Audit Pack

Bu paket önceki hızlı audit'in üstüne yazılmış daha derin **statik repo audit** paketidir.

## Çok önemli dürüstlük notu

Bu audit:
- GitHub repo dosyaları,
- repo rehberleri,
- karar belgeleri,
- ulaşılan C# kaynak dosyaları,
- önceki tasarım dokümanları

üzerinden yapılmış **statik audit**tir.

Bu audit şunları YAPMAZ:
- Unity Editor açmaz.
- Scene/Prefab Inspector referanslarını canlı doğrulamaz.
- PlayMode/EditMode testlerini koşturmaz.
- Asset import ayarlarını Unity meta seviyesinde doğrulamaz.
- Main scene runtime object graph'ını canlı görmez.

Yani "tam runtime doğrulama" değil; **Claude/Codex'in repo içinde yapacağı gerçek full audit için hedefli master plan**dır.

Ama bu paket, yanlış yere kod yazmayı engelleyecek seviyede nettir. Özellikle portal/gate/live-flow tarafında.

## Ana sonuç

Projede ana risk kod eksikliğinden çok **çoklu eski-yeni sistem katmanı**:

1. Güncel canon: floating island + Rift portal + 3 slot.
2. Bazı canlı-looking kod: RoomLoader + RoomSequenceData + tek gate.
3. Eski büyük manager: RuntimeRoomManager obsolete ama hâlâ büyük ve kandırıcı.
4. Eski docs: SYSTEM_MAP, STYLE/GDD/skill docs agent'ı yanlış yola çekebilir.
5. Portal görsel kararı: R4 ile güncellendi ama live binding kanıtı henüz şart.
