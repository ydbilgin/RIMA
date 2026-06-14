# TASK: "Little Master" oyunu + üreticisi araştırması (Codex)

ACTIVE RULES: (1) think before answering (2) min output, no speculation — kanıtlanmamış iddiayı "DOĞRULANMADI" diye işaretle (3) sadece istenen araştırma (4) bulamazsan BLOCKED yaz.

**Amaç:** Steam'deki "Little Master" oyununu (app id 4692780) VE bu oyunun üreticisini/stüdyosunu araştır. Üreticinin BAŞKA yaptığı işler — ÖZELLIKLE bir RPG Maker plugin'i — kritik öneme sahip. Kullanıcı RIMA projesinde "live tool" (çalışan oyuna bağlanıp oda/harita canlı düzenleme — RPG Maker MV/MZ tarzı in-game live editor) yapmaya çalışıyor ve bu RPG Maker plugin'i referans/ilham kaynağı.

Web erişimin var — kullan. İncelenecek URL: https://store.steampowered.com/app/4692780/Little_Master/

## Cevaplaman gerekenler

### A. Little Master (oyun)
1. Ne tür bir oyun? (genre, perspektif, art style, çekirdek loop)
2. Geliştirici + yayıncı adı (developer/publisher)
3. Çıkış tarihi / erken erişim durumu, fiyat
4. Çekirdek mekanikler — özellikle harita/oda/level tasarımı, editör, modlama veya "creator" özelliği var mı?
5. Hangi engine ile yapılmış (RPG Maker mi, Unity mi, custom mu)?
6. Karşılama / inceleme / dikkat çeken nokta

### B. Üretici (developer/studio)
1. Geliştiricinin adı, varsa web sitesi / itch.io / GitHub / X(Twitter)
2. Başka hangi oyunları/araçları yapmışlar?
3. **RPG Maker plugin'i:** Adı ne? Ne işe yarıyor? Hangi RPG Maker versiyonu (MV / MZ)? Nereden bulunuyor (itch.io, GitHub, forum)? Açık kaynak mı? Live/in-game editing veya tooling ile ilgili mi?

### C. RIMA "live tool" ilgisi (en önemli)
- RPG Maker plugin'i bir **live/in-game map editor** veya **dev tooling** ise: nasıl çalışıyor? Mimari yaklaşımı ne (çalışan oyuna nasıl bağlanıyor, hot-reload, JSON, vs.)?
- RIMA T3 live tool için ödünç alınabilecek somut fikirler (varsa).

## Çıktı formatı
Inline döndür (dosyaya YAZMA — dispatcher inline yakalar). Başlıklar A/B/C. Her kritik iddianın yanına kaynak URL. Doğrulayamadığını "DOĞRULANMADI" yaz, uydurma.
