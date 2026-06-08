# Council — FIX MİMARİSİ / ROOT-CAUSE lensi (Gemini 3.1 Pro High)

Sen kıdemli Unity oyun mimarisisin. Aşağıdaki 7 playtest bug'ı için (kullanıcı screenshot + ChatGPT repo-kontrolü) **çözüm mimarisi + root-cause hipotezi** ver. Kod doğrulamasını başka advisor (cx) yapıyor — sen TASARIM/yaklaşım katmanına odaklan. Çıktını SADECE bu cevap metni olarak ver, DOSYA YAZMA. Uydurma; emin değilsen "cx doğrulasın" de.

## Bağlam
RIMA = Unity 2D top-down roguelite, bitirme demosu. Canlı yol: MainMenu → CharacterSelect/Chamber → `_Arena` → RoomRunDirector → IsoRoomBuilder. ⚠️ Bazı bug'lar son commit'lerle ZATEN düzelmiş olabilir (dual-system fix `d96e86f9`, ESC-codex MVP `54558059`, walkable enforcement `3b800815`) → stale olabileceklerini varsay, cx doğrulayacak.

## 7 BUG
1. Play→char-select bypass, arena direkt (veya editor'da _Arena'dan Play). 2. 2 mob öldürünce kilitlenme (room clear takılıyor). 3. ESC→Yetenek Kodeksi (PauseMenu YOK). 4. SkillCodex hover'da mavi bozuk tooltip (yanlış canvas parent şüphesi). 5. Skill iconları bazen yüklenmiyor (boş/kahverengi). 6. Kılıç cliff'ten görünüyor (weapon sorting) + asset kötü. 7. Mor debug aim-line floor'da + sağına yürüyememe + oda küçük.

## İSTENEN (her bug için kısa)
- En olası root-cause (mimari açıdan).
- DOĞRU çözüm mimarisi (örn: ESC için modal-stack/PauseMenu deseni; tooltip için dedicated TooltipCanvas + sortingOrder; weapon için SortingGroup hiyerarşisi; walkable için görsel-floor=walkable invariant + debug overlay).
- Demo-blocker mı, polish mi?

Ek: **ESC/PauseMenu UX'i** için RIMA stiline uygun minimal pause menü tasarımı öner (Resume/New Run/Settings/Skill Codex/Exit). Net, uygulanabilir.
