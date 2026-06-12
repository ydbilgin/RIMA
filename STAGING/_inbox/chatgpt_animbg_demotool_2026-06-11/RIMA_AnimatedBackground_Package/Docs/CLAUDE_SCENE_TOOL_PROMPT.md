# Claude Code Prompt — Runtime Demo Navigator

RIMA için runtime room jumper ekle. F1 Spawn, F2 Combat, F3 Elite, F4 Shop, F5 Chest, F6 BossIntro, F7 BossArena, N/B next/back, Backquote panel. Geçiş: 0.25s fade out -> room prefab/build -> player spawn -> fade in. Önce mevcut runtime room registry/loader kullan; yoksa demo prefab listesi. EditorWindow bağımlılığı, Addressables, multi-scene streaming, save/load yok.
