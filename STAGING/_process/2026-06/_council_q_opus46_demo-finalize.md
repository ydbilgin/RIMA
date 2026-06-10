# RIMA demo vertical slice — DERIN-AKIL lens (Claude Opus 4.6 Thinking)

Proje koku: F:\Antigravity Projeler\2d roguelite\RIMA. Read-only analiz, kod degistirme. Gerekli dosyalari OKU.

LENS: derin akil yurutme — tekrarlayan "duzeltildi ama yine bozuk" dongusunun KOK nedenini ve test-otomasyonunun bunu nasil kiracagini coz.

## Analiz konulari

1) MOB GORUNMEME/SIYAH RENDER: Moblar 12 sprite ile wire edildi (Assets/Prefabs/Enemies/) ama bazilari oyunda gorunmuyor/siyah. EncounterController/EncounterBank + RoomRunDirector.CreateDefaultCombatFallbackWave/ResolveDefaultEnemyPrefab spawn seti vs wire seti boslugu. Kok-neden + dosya:satir.

2) SKILL-ICON BOS: DraftManager.ClassKits sadece Warblade+Elementalist (DraftManager.cs:70); kit'siz sinif (Shadowblade vb.) acilis draft'i almiyor -> bos bar. Hangi siniflar etkilenir, baska kok-neden var mi.

3) 8-P0 CLEANUP eksik/risk: DraftManager depth, Build Settings, MapFragment prefab, kapi konsolidasyonu, MainMenuScreen disable, boss->victory, opening-draft timeout, HUD/SkillBar retry.

4) TEST OTOMASYONU (en onemli): Bu regresyonlari otomatik yakalayacak Unity EditMode/PlayMode testleri YAZILABILIR MI? Tekrarlayan regresyon dongusunu kiracak EN DEGERLI test seti hangisi? Test adi -> assert -> EditMode/PlayMode -> yazilabilirlik.

CIKTI: (a) kesin teshis tablosu, (b) cleanup eksikleri, (c) yazilabilir test listesi + hangi testin "duzeltildi-ama-bozuk" doingusunu kiracagi.
