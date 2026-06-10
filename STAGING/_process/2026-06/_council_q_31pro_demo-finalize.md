# RIMA demo vertical slice — DERIN/MIMARI lens (Gemini 3.1 Pro High)

Proje koku: F:\Antigravity Projeler\2d roguelite\RIMA. Read-only analiz, kod degistirme. Gerekli dosyalari OKU (file tool).

LENS: derin mimari + tasarim. Tek-state-source, spawn pipeline butunlugu, test mimarisi gibi yapisal aciklardan bak.

## Analiz konulari

1) MOB GORUNMEME/SIYAH RENDER: Moblar (Assets/Prefabs/Enemies/) 12 Act-1 sprite ile wire edildi ama bazilari oyunda gorunmuyor/siyah. EncounterController/EncounterBank + RoomRunDirector.CreateDefaultCombatFallbackWave/ResolveDefaultEnemyPrefab hangi prefablari spawn ediyor? Wire seti ile spawn-edilebilen set arasinda mimari bosluk var mi? Siyah = eksik sprite mi sorting/material mi? Kok-neden + dosya:satir.

2) SKILL-ICON BOS: DraftManager.ClassKits sadece Warblade+Elementalist (DraftManager.cs:70); GetOpeningKitDraft kit yoksa return eder -> bos bar. Hangi siniflar secilebilir, kit'siz sinif secilince ne olur, baska kok-neden var mi. Mimari oneri: kit kapsami vs sinif-secim kisitlamasi.

3) 8-P0 CLEANUP (DraftManager depth, Build Settings, MapFragment prefab, kapi konsolidasyonu, MainMenuScreen disable, boss->victory, opening-draft timeout, HUD/SkillBar retry) — eksik/risk var mi, mimari acidan.

4) TEST OTOMASYONU (en onemli): Bu regresyonlari (mob-gorunmuyor, skill-bar-bos, depth-stuck, kapi-softlock, cift-sistem) yakalayacak Unity EditMode/PlayMode testleri YAZILABILIR MI? Hangi testler, ne assert eder, EditMode/PlayMode, yazilabilirlik. Mevcut test altyapisi (Assets/Tests/) uzerine nasil kurulur.

CIKTI: (a) kesin teshis tablosu (bug->kok-neden->dosya:satir->fix), (b) cleanup eksikleri, (c) yazilabilir test listesi (test adi->assert->mode->zorluk).
