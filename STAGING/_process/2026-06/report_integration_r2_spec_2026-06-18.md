# GÖREV: Rapor entegrasyon r2 — oda figürleri + JSON + §8.6 → DOCX yeniden üret

ACTIVE RULES: think→min→surgical→BLOCKED if unclear. **TÜM TÜRKÇE KARAKTER ZORUNLU** (ç ğ ı İ ö ş ü). Unity'ye DOKUNMA.
GRAPHIFY: mimari soru olursa graph.json `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`.

## Tek kaynak + pipeline
- Düzenle: `STAGING/report/RIMA_Senior_Design_Report.md` (figür satırı `[Şekil N: caption | path_hint]`).
- DOCX: `cd STAGING/report && python make_akademik_docx.py`. Kod-block: .py `_add_code_block` (``` fence) destekliyor — doğrula.

## Mevcut figür durumu (r1 sonrası 1-10)
1 gameplay·2 draft·3 warblade·4 buildmode·5 director·6 class_lineup(§7.2)·7 weapon_mount(§7.4)·8 mob_roster(§7.6)·9 graphify_godnodes(§10.2)·10 graphify_full(§10.3).
⚠️ r1'de warblade caption'ına "silah mount Şekil 7'de" forward-ref eklenmişti — renumber'da bunu güncelle.

## EKLENECEKLER

### A. Yeni figür — §6.2 (İzometrik Yüzen Ada: Otomatik Uçurum Yerleşimi)
`[Şekil X: Act-1'in 6 odası IsoRoomBuilder ile üretilen cliff-island oyun-içi görünümü — procedural uçurum kenarı + void arka plan + 2D ışık | figures_2026-06-18/fig_rooms_island_grid.png]`
6.2 metnine 1 cümle bağ: BuildCliffs'in taban hücrelerinden procedural uçurum ürettiği + ada-görünümünün veri+rig'den geldiği.

### B. Yeni figür + JSON — §6.4 (Dış Kaynaklı İçerik ve JSON İçe Aktarma)
- `[Şekil Y: Aynı 6 odanın JSON→tilemap şematik veri katmanı (oda kimliği, boyut, zemin bölgeleri) | figures_2026-06-18/fig_rooms_grid.png]`
- **Shrunk JSON kod-bloğu** (oda formatı kanıtı) — `Assets/Data/Map/Act1_ShatteredKeep/json/act1_entry_hall.json`'dan ~15-18 satırlık kısaltılmış excerpt (schema_version, room_id, width/height, floor.zones bir örnek, walls bir örnek). UYDURMA — gerçek dosyadan kısalt.
- **Door-graph kod-bloğu** (oda bağlantı verisi) — `_manifest.json`'dan `door_graph` excerpt (entry_hall'un N/E/W dalları). 6-oda bağlılık grafiğini gösterir.
- 6.4 metnine: bu odaların harici JSON şablonlardan içe aktarıldığı + door_graph'ın run-map dallanmasını beslediği (1-2 cümle).

### C. §8.6 alt-başlığı — Bölüm 8 sonuna
`STAGING/_process/2026-06/REPORT_S8_WORKINGPRINCIPLES.md` içeriğini (## 8.6 İnsan-YZ İş Bölümü ve Çalışma Prensipleri) RIMA_Senior_Design_Report.md'de **Bölüm 8'in son alt-başlığı 8.5'ten SONRA** ekle. (Dosyadaki "BOLUM_8_ZORLUKLAR.md" notu eski yapı — gerçek hedef RIMA_Senior_Design_Report.md Bölüm 8.) Markdown başlık/madde formatına uydur.

## RENUMBER (dikkatli)
Yeni 2 figür §6'da (eski Şekil 5 ile 6 arasına) giriyor → **eski 6→8, 7→9, 8→10, 9→11, 10→12**; yeni island=6, yeni schematic=7. Grep `Şekil ` ile TÜM atıfları bul (tanım + gövde + caption forward-ref), sıralı 1-12 yap. Tutarlılık kritik.

## BİTİRİNCE
`cd STAGING/report && python make_akademik_docx.py` → DOCX üret; **"görsel bulunamadı" placeholder 0** olmalı (programatik doğrula); 12 figür + logo gömülü; kod-blokları render oldu mu kontrol et.
Çıktı: `STAGING/_process/2026-06/REPORT_INTEGRATION_R2_DONE.md` — değişen maddeler + figür yeni-no haritası (1-12) + DOCX placeholder durumu + JSON/door-graph blok eklendi mi.
Dönüş ≤10 satır.
