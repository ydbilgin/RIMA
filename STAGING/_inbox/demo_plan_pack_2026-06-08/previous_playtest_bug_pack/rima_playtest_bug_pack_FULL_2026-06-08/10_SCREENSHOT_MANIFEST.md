# Screenshot Manifest

## Dosya durumu

Bu ChatGPT çalışma ortamında fiziksel olarak yalnızca son/hover bug görseli pakete alınabildi:

```txt
screenshots/04_skill_codex_hover_bug_available.png
```

Kullanıcının konuşmada attığı diğer 3 görsel ChatGPT tarafından görsel olarak incelendi ama ayrı dosya olarak mount edilmedi. Claude tarafında mümkünse orijinal 4 görsel tekrar issue/zip içine eklenmeli.

## Screenshot 1 — Play sonrası yanlış arena/combat görüntüsü

Gözlem:

- Karakter seçme ekranı beklenirken yüzen ada/arena görünüyor.
- Kullanıcı “2 tane fare geliyor, onları kesince kilitlenip kalıyor” dedi.
- Player sahneye class seçimi yapılmadan düşmüş gibi.
- Kılıç/cliff sorting problemi de bu sahnede görünüyor.

Kodla ilişkili kontrol:

- Aktif scene `_Arena` mı?
- MainMenu'den mi gelindi, editor doğrudan _Arena'dan mı Play'e basıldı?
- `RoomRunDirector` room clear sequence kilitleniyor mu?

## Screenshot 2 — Mor çizgi ve sağ tarafa yürüyememe

Gözlem:

- Mor aim/debug line var.
- Mor çizginin sağ tarafına player yürüyemiyor.
- Görselde zemin var ama hareket bloklu.
- Oda combat için küçük hissediyor.

Kodla ilişkili kontrol:

- `IsoRoomBuilder.LastFloorCells`
- `WalkabilityMap`
- collision/camera/player clamp
- aktif `RoomTemplateSO`

## Screenshot 3 — ESC ile Yetenek Kodeksi

Gözlem:

- ESC basınca full-screen “YETENEK KODEKSİ” açılıyor.
- Pause/options/new run/exit yok.
- Görsel debug panel gibi ve direkt scene üstüne geliyor.

Kodla ilişkili kontrol:

- `UIManager.OnEsc()` şu an gerçekten `OpenSkillCodex()` çağırıyor.
- Burada kullanıcı haklı: bu davranış PauseMenu olarak yeniden tasarlanmalı.

## Screenshot 4 — Hover'da mavi bozuk text / icon eksikleri

Gözlem:

- Mouse skill row üstüne gelince sol tarafta mavi dikey bozuk text çıkıyor.
- Bazı iconlar boş/fallback gibi.

Kodla ilişkili kontrol:

- `SkillCodexUI`
- `TooltipSystem`
- `RimaUITheme.PassiveIcon`
- `SkillData.icon`
- multiple canvas/sorting order
