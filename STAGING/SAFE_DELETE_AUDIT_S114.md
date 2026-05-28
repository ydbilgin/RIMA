# Güvenli-Silme Audit (S114)

Tarih: 2026-05-29 | Auditor: Claude Sonnet 4.6

---

## SAFE-DELETE (0 referans — kullanıcı onayıyla silinebilir)

| Dosya | GUID | Kanıt |
|-------|------|-------|
| `Assets/Art/_TempReferencePacks/Jaqmarti_IsoWalls/` (klasör + 1 PNG) | `5bbd691d810099d4abac7135434757bb` (klasör), `de4ec889433900c44903ac23c3a16053` (library_walls_00.png) | Grep: 0 ref — `.meta` dosyası dışında hiçbir .unity/.prefab/.asset referanslamamıyor |
| `Assets/Art/_TempReferencePacks/Nilsen303_IsoDungeon/` (klasör + 1 PNG) | `c0650f0a06d114d43b20707b14878ba4` (klasör), `3d17636b4f8a1c04591ccf08bc3a316f` (DungeonIsometricTileset.png) | Grep: 0 ref — `.meta` dosyası dışında hiçbir .unity/.prefab/.asset referanslamamıyor |
| `Assets/Art/Characters/Warblade/south.png` | `863bde67b712e2f4490403f3d82bafeb` | Grep: 0 ref — tüm .unity/.prefab/.asset/.anim/.controller/.cs tarandı. RIMAWallChainBuilderMenu.cs `Rotations/warblade_south.png`'i kullanıyor (farklı dosya). Resources'taki `warblade_idle_south.png` ile boyut farklı (2385 vs 5898 byte) — ayrı içerik |

---

## UNSAFE (referanslı — SİLME)

| Dosya | GUID | Nerede referanslı |
|-------|------|-------------------|
| `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/` (16 PNG) | tile_0..15 GUIDleri (bkz. altı) | `Assets/ScriptableObjects/Floor/IsoTiles35/tile_0..15.asset` (16 tile asset) + `Assets/Scenes/Test/PlayableArena_Test01.unity` (build'e dahil aktif sahne) + `Assets/Scenes/Test/PlayableArena.unity` |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` (3 PNG) | `377514c8578bb654eb311edad2112772`, `44ebb8416dc9bba4cb2922c286ac6583`, `63cbe503f81eb0e41951822d6d636051` | `Assets/Data/Tiles/Act1_ShatteredKeep/_archive_old_iso/` altındaki 6 tile.asset + `Assets/Scenes/_Archive/IsoShowcaseRoom_S95_s99.unity` — tile asset'ler archive klasöründe ama GUID bağı devam ediyor |
| `Assets/Prefabs/Obstacles/StoneColumn.prefab` (void-dışı StoneColumn) | `995cb83b025364a4481d9b13ac077bed` | `Assets/Scenes/Test/PlayableArena_Test01.unity` (aktif sahne, 3+ referans) + `Assets/Scripts/Map/RoomBuilder.cs` (path hardcoded `PfbStoneColumn`) + `Assets/Scripts/Map/ObstaclePrefabBuilder.cs` + `Assets/Scripts/Obstacles/StoneColumn.cs` + `Assets/Scripts/Obstacles/ObstacleBase.cs` |

### floor_iso_pixellab_35deg GUID listesi (referans için)
```
tile_0.png:  51edfb6813a5d8b46b5aee88de6fda83
tile_1.png:  ab59b077c2fc1ce47a26c93b6f4b266b
tile_2.png:  cca02172f61522247bb7d1f0b503d813
tile_3.png:  aac9a04f4e1bad9498ff24ac85549de1
tile_4.png:  47160392801df7d4aa2b9cb1efdc0a1b
tile_5.png:  a4248ebae138eb545b126402d139c379
tile_6.png:  68b89379a47047d4a861ec5bdd2d811a
tile_7.png:  3f5b236ae704f62439c1d36bc5c300ba
tile_8.png:  eebf388715a41154eb07fdb2969c8cc7
tile_9.png:  66212f93892533248ad356ea78504c48
tile_10.png: 67ed0760799bf9c42a4fee7940bf11cc
tile_11.png: 7aaf31c80b5d0c34cb2ecf28a8bb0fe9
tile_12.png: d1d6aefafd1526e41a3e4d556061eeb1
tile_13.png: 2487782f66b885740b968ff73824920e
tile_14.png: 384c698dc82b12f44990f5e44377b87e
tile_15.png: ebbbb9b71191091438c0a194427bfe27
```

---

## REVIEW (belirsiz)

| Dosya | Neden belirsiz |
|-------|----------------|
| `Assets/Sprites/AssetPackV3/floor_iso_pixellab_35deg/` (tüm 16 dosya) | Karar #150 gereği iso DEPRECATED ama hem `PlayableArena_Test01.unity` (build sahne) hem `PlayableArena.unity` (S95 tarihli, eski) aktif referans veriyor. Silmeden önce: (1) PlayableArena.unity'nin gerçekten retired olduğunu teyit et, (2) PlayableArena_Test01.unity'nin bu tile'lara ihtiyacı var mı yoksa temizlenecek mi karar ver. Sprite'ları silmek `tile_0..15.asset` chain'i kırar. |
| `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` (3 PNG) | Yalnızca `_archive_old_iso/` tile asset'leri ve `_Archive/` sahnesi referanslıyor. Archive klasöründeki bağ bilinçli mi yoksa cleanup gerekiyor mu belirsiz. Archive temizlenirse PNG'ler serbest kalır. |
| `Assets/Tiles/Column_Tex.png` + `Assets/Tiles/Column.asset` | Bu iki dosya yalnızca `_Recovery/0.unity`'de referanslı — gerçek sahne değil, recovery dosyası. Void-dışı StoneColumn adayı kapsamında incelendi; teknik SAFE-DELETE ama `_Recovery` klasörünün statüsü netleştirilmeli. |

---

## Ek Notlar

- **south.png dupe iddiası kısmen yanlış:** `Art/Characters/Warblade/south.png` (2385 byte) ile `Resources/Characters/Warblade/warblade_idle_south.png` (5898 byte) farklı dosyalar, tam duplicate değil. İlki 0 referanslı → SAFE-DELETE.
- **StoneColumn placer bağı:** `RoomBuilder.cs` ve `ObstaclePrefabBuilder.cs` hardcoded path ile StoneColumn prefab'ı kullanıyor. Prefab silinirse runtime spawn kodu çöker. "Void-dışı StoneColumn" sorusu: PlayableArena_Test01 sahnesi dışında da script sistemi üzerinden spawn ediliyor. UNSAFE.
- **Bu audit silmez.** Tüm işlemler git-recoverable ama kullanıcı onayı şart.
