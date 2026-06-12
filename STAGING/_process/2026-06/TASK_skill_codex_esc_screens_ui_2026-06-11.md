ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# TASK: Skill Codex Ekranı + ESC Pause Ekranı — Görsel İyileştirme

## ARAŞTIR (önce yap, sonra uygula)
1. `Assets/Scripts/UI/` altında SkillCodex, PauseMenu, ESCMenu ile ilgili tüm scriptleri bul
2. Bu ekranların mevcut Unity prefab/scene'lerini bul
3. Şu an "full Python" / plain UI mı? Hangi görseller var, hangisi eksik?
4. Act1 visual canon'ı NLM'den sorgula: slate #3A3D42, void mor #3A1A4A, ember #E89020

## HEDEF
Skill Codex ekranı ve ESC/Pause ekranı görsel olarak oyun tarzına uygun hale getirilsin.

### Skill Codex Ekranı
- Arka plan: Act1 renk paleti (koyu slate/void mor gradient veya panel)
- Başlık: dekoratif font veya border sprite
- Skill kartları: ikon yuvaları, açıklama alanı düzenli
- Eğer ikon yuvası için basit 9-slice panel sprite üretilmesi gerekiyorsa PixelLab ile üret

### ESC / Pause Ekranı  
- Overlay: yarı saydam koyu panel (void mor tonu)
- Butonlar: pixel-art styled, Act1 renk uyumlu
- Eğer buton sprite üretimi gerekiyorsa: 64×20 veya 96×24 pixel buton sprite (idle/hover/press states)

## YAPILACAK
1. Mevcut UI scriptlerini/prefabları incele
2. Eksik görselleri listele
3. Basit arka plan paneller ve buton spriteler için PixelLab generate_asset_preview çağrıları yap (UnityMCP üzerinden veya direkt)
4. Unity'de prefab'lara uygula (Image componentleri güncelle, renkler ayarla)
5. Compilation check

## BAŞARI KRİTERİ
- Her iki ekran Act1 görsel tonuna uygun (koyu, ember vurguları)
- Plain white/grey Unity default UI elemanı kalmamış
- Compilation error yok
