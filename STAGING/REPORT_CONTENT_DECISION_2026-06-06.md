# KARAR: Senior Design Final Rapor — İçerik Planı (2026-06-06)

Council: cx-laurethayday (kod envanteri) + ax-3.1-Pro (akademik mimari) + ax-3.5-Flash (lean) → Opus sentez.
Kullanıcı direktifleri: (1) raporu EN SONDA Claude yazar, council sadece içerik planlar; (2) AŞIRI teknik detay YOK — anlatı odaklı "neler yaptık ve neden"; (3) karakter boyutu GÜNCELLENDİ — rapor yeni spec'i kullanır.

## Oybirliği (3/3)
- Ana katkı çerçevesi: "oyun yaptım" DEĞİL → **"solo geliştirici, AI-destekli çok-ajanlı süreçle, veri-güdümlü bir oyun + kendi araç zincirini inşa etti"** bütünleşik vaka anlatısı.
- KES: 10 sınıfın wiki dökümü (1 örnek sınıf yeter) · "Unity/C# nedir" tanımları · roguelite tarihçesi · uzun kod blokları · AI felsefesi.
- Yeni iş üretme: rapor malzemesi MEVCUT artifact'lerden (test sonuçları, git log, STAGING karar dökümanları, QC raporları, screenshot'lar).
- Yazım: bölüm bölüm (tek seferde 30 sayfa = tekrar/uydurma riski).

## Anlaşmazlık + kararım
3.1-Pro: ağır akademik aygıt (BPMN, UML, yeni metrik altyapısı). Flash: yalın + hazır malzeme. Kullanıcı: az teknik.
**KARAR: Flash iskeleti + 3.1'in metodoloji çerçevesi, yeni ölçüm altyapısı YOK** — sadece zaten elde olan sayılar kullanılır (test sayıları, oda sayısı, sınıf/skill sayıları, QC bulgu istatistiği). Diyagramlar basit akış şemaları (3-4 adet); UML'i kullanıcı kendisi çizer (savunma hazırlığı).

## RAPOR İSKELETİ (~30-35 sayfa, TR)
1. **Giriş** (3 s) — problem: tek geliştiriciyle kapsamlı aksiyon-roguelite; çözüm yaklaşımı: veri-güdümlü mimari + AI-destekli süreç. [kullanıcı son halini kendi diliyle düzenler]
2. **İlgili Çalışmalar** (2 s) — 1 sayfalık karşılaştırma matrisi (Hades/Dead Cells/StS × kamera/oda-akışı/harita-üretimi × RIMA) + kısa PCG ve multi-agent-SE değinmesi (MetaGPT/ChatDev 1 paragraf).
3. **Geliştirme Metodolojisi: AI-Destekli Çok-Ajanlı Süreç** (5 s) — ANLATI: orchestrator+coder+council, yazar≠reviewer cross-QC, karar dökümanları. Öğrencinin rolü = sistem mimarı / pipeline tasarımcısı (algı riskine karşı net çerçeve). Örnek vaka: 10-task otonom kuyruk gecesi.
4. **Oyun Tasarımı ve Oynanabilir Döngü** (6 s) — ANLATI: tam döngü (menü→Attunement Chamber yürünebilir diegetic karakter seçimi→run→combat→3-kart draft→dallanan kapılar→boss→victory/ölüm→Shattered Echo meta-parası). 1 örnek sınıf üzerinden skill/sinerji anlatımı. State-machine 1 basit şema.
5. **Veri-Güdümlü Oda Sistemi ve Araçlar** (6 s) — ANLATI: oda = veri (RoomTemplateSO), JSON import ile ChatGPT'ye oda tasarlatma hikâyesi, 26 odalık havuz, Map Designer + Room Browser editör araçları, otomatik cliff yerleşimi + Poisson-disk prop dağıtımı (1'er paragraf, matematiksiz). Veri-akışı 1 şema.
6. **Görsel Pipeline** (2-3 s) — PixelLab AI pixel-art: 10 sınıf × 8 yön (5 üretim + 3 ayna); **GÜNCEL boyut: 120-128px kare canvas (sınıfa göre; Warblade 120, Ranger/Ronin 128), görünür gövde ~64px, PPU 64, Point filter** — eski 64×64/4-yön anlatımı tamamen revize. Kısa.
7. **Test ve Kalite Güvencesi** (4 s) — sayılar: ~85 EditMode + 11 PlayMode test dosyası, ~490 test, son koşu 410 PASS; görsel oda-QC SÜRECİ anlatısı (26 oda tarandı → 2 FAIL + sistemik bug bulundu → kök neden → düzeltme → re-QC). Jüri için altın: "süreç hata buldu ve düzeltti" hikâyesi.
8. **Karşılaşılan Zorluklar ve Çözümler** (3 s) — seçme 4-5 vaka: RTX5080 D3D12 crash→D3D11, cliff taşması, singleton'ın Systems'i öldürmesi, iso derinlik sıralaması. Her biri problem→teşhis→çözüm formatında kısa.
9. **Sonuç ve Gelecek Çalışmalar** (2 s) — [kullanıcı son halini kendi diliyle düzenler]
10. **Ek: Kanıt Matrisi** (1-2 s) — iddia → kanıt (dosya/ekran görüntüsü/test) tablosu (cx envanterinden süzme; rapora güvenilirlik katar, gövdeyi şişirmez).

## EKRAN GÖRÜNTÜSÜ KAMPANYASI (tek oturum, ~12 kare)
Oyun içi (tek play-through): (1) MainMenu, (2) Attunement Chamber genel + [E] prompt, (3) run odası genel (checker+prop), (4) combat anı, (5) knockdown anı, (6) 3-kart draft + hover tooltip, (7) dallanan kapılar (2-3 kapı), (8) M run-haritası overlay, (9) boss arena, (10) victory/death + "+n SHATTERED ECHO".
Editör: (11) Map Designer penceresi, (12) Room Browser + _Arena'da kurulu oda (mühendislik kanıtı).

## YAZIM SÜRECİ
- Sıra: Böl.5→4→7 (teknik çekirdek önce, terminoloji kilitlenir) → 3 → 8 → 6 → 2 → 1+9 (en son).
- Her bölüm ayrı oturum/dispatch; Claude yazar, kullanıcı bölüm başına onay.
- Kullanıcı kendisi: Özet/Giriş/Sonuç son rötuşu + basit UML/şema çizimi + kilit sayıları ezber (savunma: test sayıları, oda sayısı, sınıf sayısı, pipeline akışı).
- Mevcut ARA_RAPOR_RIMA.docx'ten taşınanlar boyut/yön bilgisi açısından TARANIR (64×64→120-128, 4-yön→8-yön düzeltmeleri).

## Advisor ham çıktıları
- cx: `CODEX_DONE_laurethayday.md` (kanıt envanteri + metrik tabloları — Ek-10 ve sayılar buradan)
- 3.1-Pro / 3.5-Flash: transcript (b7apjd5c0 / bf1tv9csr output dosyaları)
