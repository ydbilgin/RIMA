# Öncelikli Fix Planı

## P0 — Önce akış ve kilitlenme

1. **Aktif sahne/boot flow kanıtı**
   - Kullanıcı MainMenu'den mi Play'e basıyor, yoksa Unity Editor'da `_Arena` açıkken mi Play'e basıyor?
   - Bu ayrım yapılmadan CharacterSelect bug'ı kesinleşmez.

2. **Room clear kilitlenmesi**
   - İki mob öldükten sonra oyun neden devam etmiyor?
   - Enemy death event, room clear state, reward/exit spawn ve timeScale kontrol edilmeli.

3. **ESC davranışı**
   - Kod şu an SkillCodex açıyor. Önce PauseMenu davranışı düzeltilmeli.

## P1 — Oyuncu hissini bozan büyük görsel/teknik hatalar

4. **Walkability mismatch**
   - Floor görünen yere yürüyememe çok ağır hissiyat bozar.
   - Debug overlay şart.

5. **Weapon sorting/mount**
   - Kılıç cliff altından görünüyorsa karakter okunurluğu çöküyor.
   - Minimal mount profile patch gerekli.

6. **Skill Codex hover + icon loading**
   - Codex şimdilik debug panel gibi hissediyor.
   - Tooltip ve icon fallback düzenlenmeli.

## P2 — Görsel sunum / polish

7. **PauseMenu art direction**
   - RIMA stilinde küçük, temiz, yarı transparan pause panel.
   - Codex ayrı bir alt panel olmalı.

8. **Room size / template tuning**
   - Combat odaları daha geniş, dash lane'leri daha okunur olmalı.
   - Bu bug fix'ten sonra design pass olarak ele alınabilir.

## Tek cümlelik karar

Önce oyunun doğru yerden başlayıp doğru şekilde duraklayıp doğru şekilde devam ettiğini kanıtla. Sonra weapon/room/UI polish'e geç. Aksi halde oyuncu daha menüyü açamadan evren kırılıyor, ki The Fracturing için bile biraz fazla.
