# RIMA Playtest Bug Pack FULL — 2026-06-08

Bu paket, kullanıcının ChatGPT'ye attığı 4 playtest görseli + sözlü bug notları + ChatGPT'nin GitHub/RIMA repo kontrolünden çıkan ek bulguları tek yerde toplar.

## En önemli sonuç

Bu artık sadece "görsel feedback" paketi değil. Repo kontrolüne göre iki şey netleşti:

1. **Canlı oda akışı eski RuntimeRoomManager değil:** aktif yol `_Arena -> RoomRunDirector -> IsoRoomBuilder`. Legacy `RoomLoader`, `DoorTrigger`, `GateBehavior`, `RuntimeRoomManager` tarafına patch yazılmamalı.
2. **ESC davranışı gerçekten kodda SkillCodex'e bağlı:** `UIManager.OnEsc()` şu anda PauseMenu açmak yerine `OpenSkillCodex()` çağırıyor. Kullanıcının "ESC basınca Yetenek Kodeksi geliyor" gözlemi tasarım-hissi bug'ı değil, doğrudan mevcut kod davranışı.

## Dosyalar

- `01_CLAUDE_PROMPT_PASTE.md` — ilk sade prompt.
- `02_VISUAL_BUG_REPORT.md` — görsellerden çıkan bug raporu.
- `03_REPRO_AND_EXPECTED_FLOW.md` — repro + beklenen akış.
- `04_ACCEPTANCE_CHECKLIST.md` — fix sonrası kontrol listesi.
- `05_TECHNICAL_SUSPECTS.md` — ilk teknik şüpheli listesi.
- `06_CHATGPT_REPO_CONTROL_REPORT.md` — ChatGPT'nin repo kontrolü ve kesinleşen ek bulgular.
- `07_CLAUDE_MASTER_PROMPT_WITH_REPO_EVIDENCE.md` — Claude'a verilecek güncel ana prompt.
- `08_PRIORITY_FIX_PLAN.md` — önce ne düzeltilecek, hangi sistemden başlanacak.
- `09_TESTS_AND_ACCEPTANCE.md` — önerilen PlayMode/EditMode testleri.
- `10_SCREENSHOT_MANIFEST.md` — 4 görselin tek tek açıklaması ve dosya durumu.
- `screenshots/` — ortamda fiziksel olarak erişilebilen görsel dosyası.

## Not

İlk üç ekran görüntüsü ChatGPT konuşmasında görüldü ama bu çalışma ortamında ayrı dosya olarak mount edilmedi. Bu yüzden manifestte ayrıntılı tarif edildi. Claude'a atarken kullanıcının orijinal 4 görseli de aynı issue/zip içine koyması ideal.
