# BUILDER-OPUS — RIMA Akademik Bitirme Raporu (KTO Karatay formatı, 30-35 sayfa) — 2026-06-18

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear. cx KULLANMA. git commit YOK.

## AMAÇ
KTO Karatay Senior Design formatında, projenin GERÇEKTE NE OLDUĞUNU anlatan resmi teknik rapor. **Sunum notu/konuşma metni DEĞİL** — referans rapor gibi sistem/mimari/özellik/kod anlatan akademik doküman. Hedef **30-35 sayfa**.

Referans format (BİREBİR taklit et): `F:\BarberApp\Yasin_Derya_Bilgin_Senior_Design_1.pdf` — kapak + numaralı teal başlıklar + İçindekiler + monospace kod blokları + figürler + sayfa no.

## ÇIKTILAR
1. `STAGING/report/RIMA_Senior_Design_Report.md` — Türkçe, tam TR karakter, aşağıdaki yapı.
2. `STAGING/report/make_akademik_docx.py` — `STAGING/report/create_rapor_docx.py`'yi şablon al; BarberApp stiline GÖRE üret (aşağıda).
3. `STAGING/report/RIMA_Senior_Design_Report.docx` — ÜRET + doğrula (var, >100KB, ~30+ sayfa, figürler gömülü).

## İÇERİK KAYNAĞI (otorite sırasıyla)
- **`STAGING/report/RAPOR_DRAFT_2026-06-06.md` (951 satır) = ANA içerik** — RIMA-doğru, zaten yazılmış (Özet/Giriş/Oyun/Oda-sistemi/Görsel/AI-metodoloji/Doğrulama/Zorluklar/Sonuç). Bunu KULLAN, yeniden yazma; sadece aşağıdaki yapıya yeniden-organize et + güncelle.
- Güncel sistemleri ENTEGRE et (RAPOR_DRAFT'ta eksik/eski olanlar): Build Mode (F2) + Director Mode detayı, combat-bug vaka analizi, graphify audit (god-node 6/10), bu oturum polish'i (prop Y-sort, silah mount, HUD sol-alt). Kaynak: `STAGING/SUNUM_RAPOR_ICERIK_2026-06-17.md` + `STAGING/PLAYTEST_POLISH_DECISION_2026-06-17.md`.
- Kod detayı gerekiyorsa `CODE_MAP.md` + ilgili `Assets/Scripts/**` dosyalarını Grep/Read (kısmi).

## KAPAK (referans birebir)
- Üstte ortada logo: `figures_2026-06-18/kto_logo.png` (~6cm genişlik, ortalı).
- Altında ortalı bold: `KTO KARATAY UNIVERSITY` / `FACULTY OF ENGINEERING` / `COMPUTER ENGİNEERİNG`.
- Boşluk, ortalı bold: `SENIOR DESIGN PROJECT - 2`  ⚠️(varsayım: RIMA ikinci proje → "- 2"; kullanıcı düzeltirse tek satır).
- Büyük boşluk, sol-alt blok (bold etiket + değer):
  - `İsim ve Soyisim    :  Yasin Derya Bilgin`
  - `Öğrenci Numarası   :  231450075`
  - `Proje İsmi         :  RIMA - Rift Avcıları`
- Kapaktan sonra sayfa sonu → İçindekiler sayfası.

## STİL (referans birebir — make_akademik_docx.py)
- Sayfa: A4, kenar boşlukları ~2.4-2.8cm. Gövde font: Calibri/Aptos 11pt. Satır arası rahat.
- Başlıklar TEAL/koyu-mavi (referans tonu ~ #1F4E5F / #21576B): `Heading 1` = "1 BÖLÜM ADI" (büyük), `Heading 2` = "1.1 Alt", `Heading 3` = "1.1.1". Numaralandırma metne göm (otomatik numaralandırma şart değil).
- **İçindekiler:** python-docx TOC field ekle (`fldSimple TOC \o "1-3"`) VEYA başlıklardan elle TOC tablosu üret (sayfa no ile). Referansta nokta-lider + sayfa no var.
- Kod/alan listeleri: monospace (Consolas 9-10pt), açık-gri arka plan paragraf.
- Figürler: `doc.add_picture` ~14-15cm + altında italik gri "Şekil N: ...".
- Sayfa numarası: alt-sağ (footer field PAGE).

## RAPOR YAPISI (numaralı bölümler — RAPOR_DRAFT içeriğini buraya MAP et)
1. **PROJE GENEL BAKIŞ** — 1.1 Proje Tanımı (RIMA nedir: top-down ARPG roguelite + "environment+tooling" tezi), 1.2 Amaç ve Önem (tek-geliştirici kapsamlı oyun + mühendislik tezi), 1.3 Kullanılan Teknolojiler (Unity 6, URP 2D, C#, ScriptableObject, Input System, PixelLab/AI pipeline), 1.4 Proje Yapısı (Assets/Scripts klasör ağacı — CODE_MAP'ten: MapDesigner/Skills/Systems/UI/Combat/Core/Enemies...).
2. **SİSTEM MİMARİSİ** — Unity bileşen-tabanlı + veri-güdümlü mimari; sahne akışı (MainMenu→CharSelect→Run); manager/singleton katmanı; veri→sahne pipeline (RoomTemplateSO→IsoRoomBuilder→RoomRunDirector). Mimari diyagram (ASCII/figure).
3. **VERİ MODELLERİ (ScriptableObject Tasarımı)** — referansın "Veritabanı" bölümünün RIMA karşılığı: RoomTemplateSO, PropDefinitionSO, SkillData/DraftManager, WeaponDatabaseSO, BasicAttackProfile, vb. — alanlarıyla (monospace listeler).
4. **ANA SİSTEMLER VE ÖZELLİKLER** — Combat loop (LMB combo + Q/E/R/F + stat→hasar), Düşman AI + wave, Boss (3 faz + telegraph), Skill Draft + tier/sinerji, Branching run-map, Dual-class/cross-class, Death-loop/meta. (RAPOR_DRAFT böl.2'den.)
5. **OYUN-İÇİ GELİŞTİRME ARAÇLARI (CENTERPIECE)** — Build Mode (F2, Edit-to-Play), Director Mode (runtime UI factory: Spawn/Stats/Telemetry/Map), F1 debug. (RAPOR_DRAFT böl.3.5 + güncel.) Figürler: fig_buildmode_centerpiece, fig_director_mode.
6. **VERİ-GÜDÜMLÜ ODA SİSTEMİ + MAJOR CODE ANALİZİ** — IsoRoomBuilder/RoomRunDirector pipeline + bir derin kod örneği (kod listingi + katmanlar arası akış). (RAPOR_DRAFT böl.3.)
7. **GÖRSEL ÜRETİM HATTI** — sanat yönü, sprite üretimi (PixelLab, 8-yön), pixel-art import disiplini. (RAPOR_DRAFT böl.4.) Figür: fig_weapon_mount + figures_v2.
8. **YAPAY ZEKÂ DESTEKLİ ÇOK-AJANLI GELİŞTİRME METODOLOJİSİ** — council/cx/ax dispatch + graphify + süreç kuralları. (RAPOR_DRAFT böl.5.)
9. **DOĞRULAMA: TEST VE KALİTE GÜVENCESİ** — test altyapısı + combat-bug vaka analizi + çok-katmanlı QA. (RAPOR_DRAFT böl.6 + güncel combat-fix.)
10. **GRAPHIFY KOD-GRAFI AUDİT** — 6925 node/118 community, god-node 6/10 editor = tooling tezi sayısal kanıt. Figürler: fig_graphify_godnodes, fig_graphify_full.
11. **KARŞILAŞILAN ZORLUKLAR VE ÇÖZÜMLER** — (RAPOR_DRAFT böl.7.)
12. **SONUÇ VE GELECEK GELİŞTİRMELER** — proje özeti, teknik başarılar, öne çıkan özellikler, gelecek planlar (dürüst: Elementalist 8-yön, daha çok sınıf/oda).

## FİGÜRLER (mevcut: `STAGING/report/figures_2026-06-18/` + `STAGING/report/figures_v2/` + `STAGING/report/graphify/`)
Gameplay+HUD=fig_gameplay_hud · Draft=fig_draft_reward · BuildMode=fig_buildmode_centerpiece · Director=fig_director_mode · Weapon=fig_weapon_mount · Graphify=fig_graphify_godnodes/full · Eski: figures_v2/fig03_combat, fig06_warblade, class_lineup_sheet, mob_roster_sheet vb. Uygun bölümlere yerleştir (her ana bölümde en az 1 görsel hedefle).

## ÇIKTI RAPORU
`STAGING/report/AKADEMIK_RAPOR_DONE.md`'ye ≤15 satır: docx yolu+boyut, tahmini sayfa, bölüm sayısı, gömülen figür sayısı, atlanan/riskli, kapak SD-no varsayımı. Bana ≤10 satır özet.
