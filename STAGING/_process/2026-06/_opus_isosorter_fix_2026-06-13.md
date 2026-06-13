# IsoSorter × CharacterJuice sort-titremesi fix (2026-06-13)

- IsoSorter.cs: `[SerializeField] Transform sortReference` eklendi; null ise eski davranış (kendi transform'u), doluysa Y'yi ondan okur. sortingOrder yine kendi SpriteRenderer'ına yazılır. CharacterJuice.cs'e dokunulmadı.
- Her iki Warblade variant'ında (Resources/Prefabs + Prefabs/Characters) Body IsoSorter.sortReference = prefab ROOT transform'a atandı (PrefabInstance modification, fileID 7741234598012345678 → root 6087234464528791844).
- Base Player.prefab'a dokunulmadı (null default = geriye-uyumlu).
- Recompile: 0 error (read_console temiz).
- EditMode (RIMA.Tests.EditMode): 541 test, 11 fail — hepsi baseline (PixelLab/Wang asset path, MCP scene method, perf anti-pattern, prefab health, DontDestroyOnLoad). Sort/IsoSorter ilişkili YENİ fail YOK.
- Data-proof (execute_code): Warblade variant'ta Body.localPosition.y +0.03 bob → sortingOrder STABLE (0→0). sortReference=null legacy obje → +0.03 hareket order'ı DEĞİŞTİRDİ (0→-3, eski davranış korundu).
- Git commit YAPILMADI.
