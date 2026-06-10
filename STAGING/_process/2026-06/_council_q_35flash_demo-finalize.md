# RIMA demo vertical slice — LEAN/SHIP-FAST lens (Gemini 3.5 Flash High)

Proje koku: F:\Antigravity Projeler\2d roguelite\RIMA. Read-only analiz, kod degistirme. Gerekli dosyalari OKU.

LENS: en yalin yol + over-engineering elestirisi. Sunum birkac saat icinde; MINIMUM is ile basit oynanabilir demo. Hangi testler/temizlikler GEREKSIZ, hangileri sart.

## Analiz konulari

1) MOB GORUNMEME/SIYAH: 12 mob sprite wire edildi, bazilari gorunmuyor. En hizli kok-neden + en yalin fix (Assets/Prefabs/Enemies/, EncounterController, RoomRunDirector fallback prefab). Dosya:satir.

2) SKILL-ICON BOS: DraftManager.ClassKits sadece Warblade+Elementalist; kit'siz sinif bos bar. EN YALIN cozum: demo'yu 2 sinifa kisitla mi, yoksa kit eklemek mi? Hangisi daha az risk.

3) 8-P0 CLEANUP: hangileri sunum icin SART, hangileri over-kill/erteleme. Yalin sira.

4) TEST OTOMASYONU: Birkaç saatlik sunum icin test otomasyonu yazmak MANTIKLI MI yoksa over-engineering mi? Eger yazilacaksa SADECE en yuksek-deger 2-3 test hangisi (mob-sprite-null, ClassKit-coverage, clear->door-softlock gibi)? Yalin liste.

CIKTI: (a) kisa teshis, (b) sart-olan-cleanup vs erteleme, (c) yazmaya deger MINIMUM test listesi (yoksa "gerekmez" de).
