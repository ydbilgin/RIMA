# COUNCIL-TEST 2 — Boss presentation BAĞIMSIZ RUNTIME REPRO (cx lens)

ACTIVE RULES: (1) think before acting (2) READ-ONLY test (3) Unity KOD/GIT mutasyonu YOK — sadece play+assert+read_console (4) BLOCKED if unclear.

## ⛔ READ-ONLY
Kod/asset/scene DEĞİŞTİRME-KAYDETME. `git add/commit` YOK. Sadece play → assert → screenshot → read_console → STOP. Bitince STOP + playModeStartScene=MainMenu.

## Amaç
Başka cx executor Boss-presentation fix yaptı (4 sub-fix PASS dedi). Sen BAĞIMSIZ doğrula + bir kullanıcı-bildirimi bug'ı kontrol et.

## Değişen dosyalar (referans — DOKUNMA)
`RoomRunDirector.cs, ShopRoomController.cs, PenitentSovereign.cs, BossHealthBar.cs, RoomMonologController.cs`.

## A) Boss sub-fix doğrulama (runtime, boss odasına git)
Deterministik route ile boss odasına gir (executor: Combat→Combat→Merchant→Combat→Boss force-route). `execute_code` ile assert (compact string):
1. **2A residue:** boss odasında active ShopRoomController sayısı = 0, active merchant stand = 0?
2. **2B scale/anchor:** boss bounds arena içinde, feet floor'da, HUD ile overlap YOK?
3. **2C health-bar:** crimson/stone fill (yeşil YOK), %66/%33 phase-notch var, name+phase label var?
4. **2D subtitle:** monolog/subtitle skill-bar'ın ÜStÜnde güvenli-alanda, boss gövdesine binmiyor?
Screenshot al (game_view + scene_view).

## B) ⚠️ KULLANICI-BİLDİRİMİ: Ödül-ekranı monolog-bleed (Task #9) — ZORUNLU
Kullanıcı gördü: ÖDÜL SEÇ ekranında (SkillOfferUI, 3 kart) arka planda `RoomMonologController` flavor/monolog cümlesi ("Bu...değil. Sadece dur...") + ekstra text kartların ARKASINDAN sızıyor (scrim onları kapatmıyor).
- Bir oda temizle → ödül ekranına gel (force-route veya normal). 
- Assert/screenshot ile KONTROL ET: ödül ekranında monolog/flavor text kartların/scrim'in arkasından GÖRÜNÜYOR mu?
- Net söyle: **BLEED VAR mı YOK mu?** Varsa: hangi canvas/sorting (RoomMonolog'un sortingOrder vs SkillOfferUI scrim). cx'in RoomMonologController değişikliği bunu ETKİLEDİ mi (iyileştirdi/bozdu/nötr)?

## ÇIKTI (dönüş ≤8 satır)
`STAGING/_process/2026-06/demo_fix_tasks/TESTRESP_2_boss_cx.md`'ye yaz. Dönüşte: A) 4 sub-fix PASS/FAIL + assert kanıtı · B) ödül-bleed VAR/YOK + sebep · console durumu.
