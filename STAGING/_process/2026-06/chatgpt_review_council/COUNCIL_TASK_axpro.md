# COUNCIL — ChatGPT REV2 Review Analizi (ax Pro lens: DESIGN/UX YARGISI + VISION)

ACTIVE RULES: (1) think before answering (2) min — no speculation (3) read-only ANALYSIS (4) BLOCKED if unclear.

## ⛔ MUTLAK KISIT — READ-ONLY
**Hiçbir dosyayı DEĞİŞTİRME. Kod yazma. `git add/commit/push` KESİNLİKLE YOK. Unity'de mutasyon YOK.** Sadece OKU + ANALİZ ET + tek bir RESP dosyası yaz. (Geçen oturum bir advisor rogue gidip `git add .` ile her şeyi commit'ledi — bu YASAK, tekrarlama.)

## Sen kimsin
Sen council'in **design/UX yargısı + görsel (vision)** lens'isin. Sende vision var → ChatGPT'nin önerdiği layout görsellerini GERÇEKTEN İNCELE. Diğer advisor'lar (kod-fizibilite + lean-skeptic) ayrı bakıyor.

## Bağlam
- RIMA = Unity 2D top-down ARPG roguelite, bitirme **demosu 19 Haziran = 2 GÜN**. Tez: "oyun değil environment + tooling showcase" — hocaya CANLI sunum. Ton = **"Fractured Epic"**, dramatik/canlı, grimdark DEĞİL.
- Görsel canon: slate #3A3D42 · void mor #3A1A4A · cyan ≤%15 · ember #E89020 · ambient 0.22.
- ChatGPT REV2 review paketi geldi; council ile bağımsız analiz edip ORTAK karar çıkaracağız.

## ChatGPT paketi
Klasör: `STAGING/_process/2026-06/chatgpt_review_rev2/RIMA_ChatGPT_Review_2026-06-17_REV2/`
- Metin: `01_EXECUTIVE_DECISION.md` · `03_SCREEN_BY_SCREEN_REVIEW.md` · `04_DIRECTOR_MODE_REDESIGN.md` · `06_GAME_UI_REDESIGN.md` · `07_TWO_DAY_DEMO_PLAN.md`
- **GÖRSELLER (vision ile aç — ZORUNLU):** `visuals/director_mode_proposed_layout.png` · `visuals/combat_hud_proposed_markup.png` · `visuals/capture_qa_failures.png`
- (İstersen oyunun mevcut hâli: `STAGING/_process/2026-06/chatgpt_review_rev2/RIMA_ChatGPT_Review_2026-06-17_REV2/source_reference/RIMA_screens_v2_for_chatgpt.zip` içinde 25 state PNG — açıp önce/sonra kıyaslayabilirsin.)

## SENİN SORULARIN (design/UX/vision)
1. **Director proposed layout (görseli incele):** 56px top-bar / 64px left-rail / 280-320px library / center viewport / 320-360px inspector / status-bar — bu düzen RIMA tezi ("profesyonel runtime editor showcase") için DOĞRU mu? Unity Editor / Hades dev-tool diline benziyor mu? Renk hiyerarşisi (base #11131A, ember=karar-rengi, cyan ≤%15) sağlam mı? **2 günde "debug→ürün" sıçraması bu görselle gerçekten olur mu, yoksa yarım kalıp daha kötü mü görünür?**
2. **HUD markup (görseli incele):** önerilen ölçüler (HP 200-220×14-16, slot 44-56px) 1080p'de doğru okunabilirlik dengesini veriyor mu? Eksik/yanlış bir şey var mı? RIMA'nın minimalist kimliğini öldürür mü?
3. **Boss sunum kararı:** ChatGPT "boss güçlü değil, P0 prototype-problem" diyor (HUD overlap, neon-yeşil bar, shop-residue, subtitle-overlap, zemine oturmuyor). Bu yargı doğru mu? Demo'da hocayı en çok hangi görsel kusur utandırır — öncelik sıran?
4. **En yüksek görsel-kaldıraç:** 2 günde hocanın "bu profesyonel" demesini sağlayacak TEK en yüksek-etkili görsel iş ne? ChatGPT'nin P0 sırası (capture→boss→HUD→blob→low-HP→Director) görsel-etki açısından doğru mu, yoksa Director-shell mi en öne alınmalı?
5. **Capture-QA görseli:** ChatGPT'nin "aynı SHA → FAIL" + "yanlış isimlendirilmiş/duplicate ekran" eleştirisi haklı mı? Demo sunumunda sahte-kanıt riski?
6. **ChatGPT'nin görsel açıdan AŞIRIYA kaçtığı / GÖZDEN KAÇIRDIĞI:** 2-gün kısıtında over-design olan 1 öneri + ChatGPT'nin atladığı 1 görsel sorun.

## ÇIKTI (E1: dönüş ≤10 satır)
Tüm analizi şu dosyaya YAZ: `STAGING/_process/2026-06/chatgpt_review_council/RESP_axpro.md`
Format: her soru için net yargı + gerekçe; görselleri GERÇEKTEN açtığını kanıtla (ne gördüğünü betimle). Sonunda **"ax Pro'nun tek-cümle kararı"** + **"2 günde en yüksek görsel-kaldıraç TEK iş"** + **"ChatGPT'nin en aşırı 1 önerisi"**.
Dönüşte SADECE: RESP yolu + 8 satır en kritik bulgu. TAM TÜRKÇE karakter zorunlu.
