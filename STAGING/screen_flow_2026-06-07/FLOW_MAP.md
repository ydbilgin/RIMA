# RIMA Ekran Akış Kataloğu v2
**Tarih:** 2026-06-07  
**Sahne yakaları:** `ScreenCapture.CaptureScreenshot` ile (yalnızca Game View)  
**Kısıtlar:** Düşman görselleri üretilmedi; akış karelerinde echo dummy kullanıldı (idle_south sprite + cyan tint 0.45/0.96/1/0.72, Enemy katmanı, AI yok). Boss sprite placeholder — 16_boss_room karesinde kare dışı.

---

| # | Dosya | Ekran | Nereden | Tetik | Nereye |
|---|-------|-------|---------|-------|--------|
| 01 | 01_main_menu.png | Ana Menü | Uygulama başlangıcı | Uygulama başladı | BASLA → CharacterSelect |
| 02 | 02_settings.png | Ayarlar | Ana Menü | AYARLAR tıklandı | Kapat → Ana Menü |
| 03 | 03_chamber_wide.png | Attunement Chamber (geniş) | Ana Menü → BASLA | CharacterSelect yüklendi | Karakter seç → 04 |
| 04 | 04_attune_prompt.png | Attunement Prompt | Chamber | Karaktere yaklaşıldı | [E] → 05 |
| 05 | 05_attuned.png | Attuned Echo | Chamber | [E] bürünme | Rift kapısına yönel → 06 |
| 06 | 06_rift_gate_prompt.png | Rift Kapısı Prompt | Chamber | Rift kapısına yaklaşıldı | [G] → Koşu başladı |
| 07 | 07_run_room_spawn.png | Koşu — Oda Spawn | _Arena yüklendi | Sahne geçişi | Düşmanlarla karşılaş → 08 |
| 08 | 08_combat.png | Savaş | Oda | Düşmanlar aktif | Düşmanı kır → 09 |
| 09 | 09_execute_prompt.png | Execute Prompt | Savaş | Broken/Sundered durumu + yakın mesafe | [RMB] infaz → savaş devam |
| 10 | 10_room_clear_reward.png | Oda Temizlendi + Ödül | Savaş | Tüm düşmanlar yenildi | [G] Ödülü Al → 11 |
| 11 | 11_draft.png | Beceri Draft | Oda temizlendi | RewardPickup tetiklendi | Kart seçildi → 12 |
| 12 | 12_portals.png | Portal Kapıları | Draft | Draft tamamlandı | Portal seçildi → sonraki oda |
| 13 | 13_map_overlay.png | Koşu Haritası | Oyun içi | [M] tuşu | [M] tekrar → oyun |
| 14 | 14_character_sheet.png | Karakter Sayfası | Oyun içi | [TAB] tuşu | [TAB] tekrar → oyun |
| 15 | 15_skill_codex.png | Beceri Kodeksi | Oyun içi | [ESC] tuşu | [ESC] tekrar → oyun |
| 16 | 16_boss_room.png | Boss Odası | Koşu ilerlemesi | Boss arena yüklendi | Boss yenildi → 17 |
| 17 | 17_victory.png | Zafer (Demo Tamamlandı) | Boss yenildi | DemoCompleteOverlay | MAIN MENU → 19 / PLAY AGAIN → 03 |
| 18 | 18_death.png | Ölüm Ekranı | Savaş | Oyuncu canı bitti | ANA MENÜ → 19 / TEKRAR → 03 |
| 19 | 19_return_menu.png | Geri Dönen Ana Menü | Ölüm/Zafer | MainMenu yüklendi ("Yine geldin.") | BASLA → 03 |

---

## Notlar

- **Echo Dummy:** Kareler 08, 09, 10, 12'de savaş kopyaları (echo dummy) kullanıldı. Gerçek düşman sprite'ları (FractureImp, SeamCrawler vb.) henüz üretilmedi.
- **Boss Sprite:** 16_boss_room karesinde boss sprite placeholder sarı renkte; kare dışında tutuldu, yalnızca HP barı görünür.
- **Frame 17:** `DemoCompleteOverlay.Show()` ile tetiklendi — tam zafer/CTA ekranı (run özeti + Steam Wishlist butonu).
- **Frame 19:** MainMenu sahnesinin "Yine geldin." durumu — önceki koşudan dönen oyuncuya gösterilen selamlama metni aktif.
- **Yakalama yöntemi:** Tüm kareler `UnityEngine.ScreenCapture.CaptureScreenshot(path)` ile alındı. Editor penceresi/masaüstü/OS imleci hiçbir karede görünmez.
