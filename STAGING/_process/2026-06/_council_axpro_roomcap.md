# Council AX PRO Decision: Room Capture Pipeline

1. **Pipeline & Sorun:** Canonical akış `RuntimeRoomManager` → `RoomRunDirector` → `IsoRoomBuilder`'dır. İlk deneme legacy `RoomLoaderMenu` ile bare sahnede yapıldığı için `IsoRoomBuilder` atlandı; void BG, ışıklar ve kamera `_Arena` sahne setup'ına ait olduğu için render edilmedi.
2. **Auto-cliff-gen:** `IsoRoomBuilder.BuildCliffs(floorCells)` fonksiyonu procedüral olarak taban hücrelerinden cliff hesaplar ve çizer.
3. **Capture Stratejisi (En Sağlam = Play-Mode + LiveRoomReloader):** `_Arena` sahnesini kirletmemek (no-leak) için kesinlikle **Play-Mode + LiveRoomReloader** kullanılmalıdır. Play mode'da `_Arena` çalışırken JSON'u `room_current.json`'a basıp anında capture almak, sahne dosyasına dokunmadığı için Git-leak riskini sıfırlar.
4. **Figür Anlatısı:** **İkisi birden (Schematic + Island)**. JSON verisinin grid'e dizilişini kanıtlamak için schematic; tasarım-oynanış hedefini göstermek için island görünümü yan yana sunulmalıdır (veri güdümlü mimarinin en güçlü akademik kanıtı).
