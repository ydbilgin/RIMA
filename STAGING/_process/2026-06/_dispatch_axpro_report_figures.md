READ-ONLY HARD KISIT: Bu bir GÖRSEL DENETİM görevi. HİÇBİR dosya/kod düzenleme, HİÇBİR git komutu ÇALIŞTIRMA. İZİN VERİLEN tek yazma: kendi bulgu dosyan (aşağıda).

# Amaç
RIMA akademik bitirme raporundaki 12 figürün demo-öncesi SON görsel denetimi. YARIN jüriye sunulacak. Bu görevin ÇEKİRDEĞİ: **her PNG'yi AÇ VE GERÇEKTEN İNCELE** (vision). Geçmişte bir figür yanlışlıkla masaüstü/Steam screenshot'ıydı (taskbar + pencere kenarı görünüyordu) ve gözden kaçmıştı — bu hatayı bir daha YAKALAMAK senin asıl işin.

## Önce caption'ları oku
Rapor: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\report\RIMA_Senior_Design_Report.md` — Şekil 1-12 caption satırlarını çıkar (format: `[Şekil N: açıklama | dosya.png]`).

## AÇ VE İNCELE (mutlak yollar — hepsini tek tek aç)
1. `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\report\figures_2026-06-18\fig_gameplay_hud.png` (Şekil 1: oyun ekranı + HUD)
2. `...\STAGING\report\figures_2026-06-18\fig_draft_reward.png` (Şekil 2: 3-kart ödül draft)
3. `...\STAGING\report\figures_v2\fig06_warblade.png` (Şekil 3: Warblade silahsız sprite)
4. `...\STAGING\report\figures_2026-06-18\fig_buildmode_centerpiece.png` (Şekil 4: Build Mode F2 editör)
5. `...\STAGING\report\figures_2026-06-18\fig_director_mode.png` (Şekil 5: Director Mode dock)
6. `...\STAGING\report\figures_2026-06-18\fig_rooms_island_grid.png` (Şekil 6: 6 oda cliff-island oyun-içi)
7. `...\STAGING\report\figures_2026-06-18\fig_rooms_grid.png` (Şekil 7: JSON→tilemap şematik)
8. `...\STAGING\report\figures_v2\class_lineup_sheet.png` (Şekil 8: 10 sınıf idle dizilim)
9. `...\STAGING\report\figures_2026-06-18\fig_weapon_mount.png` (Şekil 9: silah mount)
10. `...\STAGING\report\figures_v2\mob_roster_sheet.png` (Şekil 10: 12 düşman kadro sheet)
11. `...\STAGING\report\figures_2026-06-18\fig_graphify_godnodes.png` (Şekil 11: en bağlı 10 node, 6 turuncu editör + 4 mavi runtime)
12. `...\STAGING\report\figures_2026-06-18\fig_graphify_full.png` (Şekil 12: tam graf 6925 node)
(`...` = `F:\Antigravity Projeler\2d roguelite\RIMA`)

## Her figür için kontrol et
- **Caption EŞLEŞMESİ:** Görsel, caption'ın söylediği şeyi gösteriyor mu? (yanlış görsel = P0)
- **MASAÜSTÜ SIZINTISI (P0):** Windows taskbar, Steam overlay, tarayıcı/pencere chrome, masaüstü ikonu, fare imleci, editör pencere kenarı — Unity oyun/sahne görüntüsü DIŞINDA herhangi bir şey var mı? Varsa P0.
- **OKUNABİLİRLİK:** Rapor ölçeğinde metin/etiket okunur mu? Çok karanlık/bulanık/düşük kontrast mı?
- **TUTARLILIK:** Şekil 11'de gerçekten 6 turuncu + 4 mavi node sayılıyor mu? Etiketler doğru mu?
- **KALİTE:** Bozuk sprite, eksik HUD, placeholder, yarıda kalmış render?

## Çıktı
Bulguları şu dosyaya YAZ: `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\_process\2026-06\report_council\axpro_findings.md`
Format: en üstte **VERDICT: PASS / PASS-WITH-FIXES / FAIL**, sonra per-figür tablo: `| Şekil | Caption eşleşme(✓/✗) | Masaüstü sızıntı(yok/VAR) | Okunabilirlik | Not |`.
Dönüşte (stdout) ≤15 satır: VERDICT + sorunlu figürlerin no'su + en kritik bulgu.
