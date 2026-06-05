# Task — CharacterSelect.unity: authored eski UI'yı sahnede kalıcı INACTIVE yap (Unity MCP işi)

Karar (Opus): `CharacterSelect.unity` sahnesindeki authored eski ekran (`CharacterSelectCanvas/Root` —
çocukları Header/InfoPanel/ClassGridPanel/BottomBar) edit-mode'da hâlâ görünüyor; runtime'da kod kapatıyor
ama sahnede aktif kayıtlı. Kalıcı inactive yapılacak. SİLME YOK.

## Adımlar (Unity MCP üzerinden — Unity şu an boş, kullanabilirsin)
1. Aktif sahne `CharacterSelect` değilse `Assets/Scenes/CharacterSelect.unity`'yi aç (manage_scene load).
2. `CharacterSelectCanvas/Root` GameObject'ini bul → `SetActive(false)` (manage_gameobject veya execute_code).
   SADECE bu objeyi kapat — component silme/değiştirme YOK, başka objeye dokunma YOK.
3. Sahneyi KAYDET (manage_scene save).
4. PLAY-VERIFY: play mode'a gir → şunları programatik doğrula ve raporla:
   - `RuntimeRoot_CharSelect` kuruldu (var + activeInHierarchy=true)
   - `CharacterSelectCanvas/Root` hâlâ inactive
   - RoomCharacter_* sayısı = 10
   - Console error sayısı = 0 (read_console types=error)
5. Play'den çık. Sahnenin kayıtlı halinde Root.activeSelf=false olduğunu bir kez daha doğrula.

## Çıktı
`STAGING/_ax_done_scene_authored.md` dosyasına: yapılan adımlar + 4. adımın 4 kanıt değeri + sorun varsa BLOCKED yaz.
