# Görsel Bazlı Bug Raporu

## Screenshot 1 — Play'e basınca yanlış ekran / yanlış sahne state'i

Kullanıcı gözlemi:
- Play'e basınca karakter seçme ekranı beklenirken doğrudan oyun/arena benzeri sahne görünüyor.
- Skill/codex ekranı veya gameplay paneli akışa erken giriyor.
- Karakter seçimi yapılmadan player sahneye spawn olmuş gibi duruyor.
- Kılıç/silah cliff arasından görünüyor, karakterle doğru render edilmiyor.

Teşhis:
- Boot flow veya scene routing hâlâ kirli olabilir.
- Beklenen akış: MainMenu -> CharacterSelect / Chamber -> _Arena.
- Görünen davranış, CharacterSelect/Chamber'ın bypass edildiğini veya stale UI/chamber root temizliğinin tam yapılmadığını düşündürüyor.
- Eğer güncel akışta ChamberSelectBootstrap varsa, MainMenu button ve authored scene entry noktası tekrar kontrol edilmeli.

Öncelik: P0

---

## Screenshot 2 — Mor debug/aim line ve sağ tarafa yürüyememe

Kullanıcı gözlemi:
- Mor çizginin sağ tarafına player yürüyemiyor.
- Görselde floor var ama hareket fiziksel olarak engelleniyor.
- Map zaten küçük; combat için bu kadar küçük olmamalı.

Teşhis:
- Walkable mask, room bounds, generated collider veya player clamp görsel floor ile eşleşmiyor.
- Muhtemel root cause:
  - Room template walkable polygon yanlış.
  - IsoRoomBuilder cliff/void collider'ı floor içine taşıyor.
  - Player movement clamp, camera bounds veya RoomBounds sağ çıkıntıyı yok sayıyor.
  - CompositeCollider2D veya TilemapCollider2D sağ tarafta görünmez duvar üretiyor.
  - Debug aim line visual kalmış, ama asıl blok hareket/collider tarafında.

Öncelik: P0/P1

---

## Screenshot 3 — ESC basınca Skill Codex gibi büyük debug ekranı açılıyor

Kullanıcı gözlemi:
- ESC basınca büyük “YETENEK KODEKSİ” ekranı geliyor.
- Bu ekran doğrudan scene'de olmamalı.
- Pause/options/new run/exit gibi menü seçenekleri yok.
- UI görseli rough/debug görünüyor, oyunun stiline uymuyor.

Teşhis:
- ESC binding yanlışlıkla Skill Codex / debug panel açıyor olabilir.
- PauseMenu ile Codex ayrılmalı.
- Codex, Pause Menu içinden seçilebilir ayrı panel olmalı; ESC'nin default davranışı Pause olmalı.
- Skill Codex gameplay başlangıcında veya ESC ile doğrudan full-screen debug olarak açılmamalı.

Öncelik: P0/P1

---

## Screenshot 4 — Hover sırasında mavi debug text / tooltip bozulması

Kullanıcı gözlemi:
- Mouse ile skill üstüne gelince mavi renkli bozuk yazılar/katmanlar görünüyor.
- Bazı skill iconları her zaman yüklenmiyor; boş/kahverengi kutular görünüyor.

Teşhis:
- Tooltip prefab veya runtime tooltip layout bozuk.
- TMP text overflow/word-wrap/pivot/anchor yanlış.
- Tooltip, Codex paneli kapanınca temizlenmiyor olabilir.
- Skill icon loading sistemi deterministic değil.
- Null icon fallback var ama asıl missing icon loglanmıyor olabilir.

Öncelik: P1
