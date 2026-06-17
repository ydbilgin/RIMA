# GÖREV: RIMA Akademik Rapor — Kritik Review (council, READ-ONLY)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
**READ-ONLY: koda/git'e/dosyaya DOKUNMA. Sadece oku + figürlere BAK + bulgu raporu yaz (aşağıdaki çıktı dosyasına).**

NLM ACCESS: RIMA tasarım bağlamı gerekirse:
  NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

## Amaç
RIMA Senior Design (bitirme) akademik raporunu eleştirel incele. Bu rapor YARIN teslim/sunum için. Kullanıcı 4 somut şikayet verdi; her birini KANITLA + düzeltme önerisi yaz.

## Oku (ana metin — TAM oku)
- `STAGING/report/RIMA_Senior_Design_Report.md` (62 KB, 11 figür referansı, 12 bölüm)

## Figürlere BAK (vision — her birini aç ve değerlendir: gerçekten RIMA/Unity'den mi? yerinde mi?)
- Şekil 1 → `STAGING/report/figures_2026-06-18/fig_gameplay_hud.png`
- Şekil 2 → `STAGING/report/figures_2026-06-18/fig_draft_reward.png`
- Şekil 3 → `STAGING/report/figures_v2/fig06_warblade.png`
- Şekil 4 → `STAGING/report/figures_2026-06-18/fig_buildmode_centerpiece.png`
- Şekil 5 → `STAGING/report/figures_2026-06-18/fig_director_mode.png`
- Şekil 6 → `report_screenshots/11_map_designer.png` ← **DOSYA YOK (klasör boş). Bu figür KIRIK.** Doğrula + raporda nasıl görünüyor?
- Şekil 7 → `STAGING/report/figures_v2/class_lineup_sheet.png`
- Şekil 8 → `STAGING/report/figures_2026-06-18/fig_weapon_mount.png`
- Şekil 9 → `STAGING/report/figures_v2/mob_roster_sheet.png`
- Şekil 10 → `STAGING/report/figures_2026-06-18/fig_graphify_godnodes.png`
- Şekil 11 → `STAGING/report/figures_2026-06-18/fig_graphify_full.png`

## Değerlendir (kullanıcının 4 SOMUT maddesi — her biri için kanıtlı bulgu + öneri)
1. **FIGÜR DENETİMİ:** Her figür gerçekten RIMA/Unity'den + yerinde mi? Hangileri ChatGPT-vari/jenerik/yersiz/kırık? (Şekil 6 zaten kırık — başka var mı?) Hangi figür hangi bölüme ait olmalı?
2. **AI-ODAĞI FAZLA:** Rapor şu an çok "full AI süreç raporu" gibi mi duruyor? AI bahsedilsin ama DENGELİ — oyun + yazılım mimarisi öne çıkmalı. Hangi pasajlar AI'yı aşırı öne çıkarıyor? (satır no + alıntı ver)
3. **EKSİK: "ne ne işe yarıyor" + dosya/klasör yapısı.** Sistem/sınıf sorumlulukları (hangi script ne yapar) + `Assets/Scripts/` klasör yapısı açıklaması YOK/zayıf. Nereye, ne içerikle eklenmeli? (referans: tipik bitirme raporu "Proje Yapısı" + "katmanlı kod analizi" bölümleri)
4. **ChatGPT-vari gereksiz pasajlar:** Şişirme/jenerik/"as an AI"-kokan/boş retorik pasajları tespit et (satır no + alıntı). Çıkarılmalı/sadeleştirilmeli olanları listele.

## Ek kontrol
- Bölüm yapısı akademik bitirme formatına uygun mu? Eksik standart bölüm var mı (örn. Sistem Mimarisi / Proje Klasör Yapısı / Test & Doğrulama)?
- Türkçe akademik dil tutarlı mı?

## ÇIKTI (E1 — DOSYAYA yaz, dönüşte ≤10 satır özet + dosya yolu)
Bulguları şu dosyaya yaz: `STAGING/_process/2026-06/AX_REPORT_REVIEW_VERDICT.md`
Format:
- **A. Figür tablosu:** Şekil | dosya | gerçek-Unity mi? | yerinde mi? | aksiyon (TUT/DEĞİŞTİR/ÇIKAR/YENİDEN-ÇEK)
- **B. AI-odağı:** aşırı pasajlar (satır no + alıntı + öneri)
- **C. Eksik yapı bölümleri:** ne eklenmeli, nereye, taslak başlıklar
- **D. ChatGPT-vari pasajlar:** çıkarılacak/sadeleştirilecek (satır no + alıntı)
- **E. Öncelikli aksiyon listesi** (rapor revizyonu için sıralı, somut)
