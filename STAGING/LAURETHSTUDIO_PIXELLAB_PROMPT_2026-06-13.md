# LaurethStudio Agent Prompt — PixelLab Pipeline Upgrade (kaynak: resmi PixelLab YouTube, son 3 ay)

> Bu dosya KULLANICI tarafından LaurethStudio'daki agent'a verilecek bir prompt'tur. (RIMA tarafı bunu uygulamaz; yalnız aktarım için yazıldı. LaurethStudio klasörüne dokunulmadı.)

---

## PROMPT (LaurethStudio agent'ına yapıştır)

Sen LaurethStudio'nun asset-pipeline agent'ısın. Aşağıdaki PixelLab MCP iş akışı güncellemelerini stüdyonun sprite/animasyon üretim standardına işle. Kaynak = PixelLab'in resmi YouTube kanalında son 3 ayda (2026-03 → 06) duyurulan yeni özellikler. Hedef: daha tutarlı kadro + daha ucuz/temiz animasyon.

**Benimsenecek 5 standart:**

1. **States-first üretim.** Bir karakteri doğrudan idle'dan animate etme. ÖNCE hedef pozu üret (`create_character_state`: mid-walk / fighting-stance / casting / hit-react / boss-phase), SONRA o state'ten animate et. Faydası: idle-start jank'i biter; aynı base'ten bedava düşman varyantı ve boss-faz görselleri çıkar.

2. **Animate with Text V3 = varsayılan animasyon modeli.** `animate_character` çağrılarında V3 kullan (pro'ya göre ~3-5× daha ucuz kredi + daha çok kare). `pro` modelini yalnız ağır/öne-çıkan impact animasyonlarına sakla.

3. **Interpolate ile loop + geçiş.** Kusursuz döngü için ilk ve son kareyi aynı yap; iki poz arası yumuşak geçiş için interpolate kullan. Manuel "tek kare loop-hitch" temizliğini bırak.

4. **create-from-style-reference = tutarlılık belkemiği.** Tek bir kanon "çapa" sprite belirle; tüm kadroyu (mob, NPC, loot) o sprite'ı style-reference vererek üret. Style-ref feedback döngüsünü çalıştır. Aynı teknikle loot/relic nadirlik (rarity) tier görselleri tutarlı kalır.

5. **Tileset PRO akışı.** Terrain için PRO tileset (alt+üst terrain + duvar/geçiş), sonra "transform-to-layout → edit-pro (make prettier)" cila adımı. İzleme listesi: Object Creator paket/state'leri = yıkılabilir/destructible objeler için (ileride).

**Kısıt:** Her üretim stüdyonun kanon paleti + style-ref çapasına uymalı; tek seferlik tutarsız sprite üretme. Üretim sonrası alfa/temizlik QC uygula.

---

### Notlar (kullanıcı için, prompt'a dahil değil)
- Tam transkript + ayrıntılı sentez RIMA tarafında: `STAGING/_process/2026-06/_research_pixellab_yt_2026-06-13.md`.
- RIMA'da da memory'ye kaydedildi: `reference_pixellab_yt_pipeline_2026-06`.
