# Soru: Senior design (bitirme) raporu — içerik mimarisi ve akademik çerçeve

RIMA = Unity 6 URP 2D izometrik aksiyon-roguelite (Türkçe bitirme projesi, ~30-40 sayfa akademik rapor hedefi). Ara rapor teslim edildi; şimdi detaylı final rapor planlanıyor.

READ these files first:
- STAGING/SENIOR_DESIGN_REPORT_PLAN.md (mevcut plan)
- CURRENT_STATUS.md (sadece en üstteki 2-3 RESUME bloğu — projenin bugünkü durumu)

## Projenin bugünkü durumu (özet)
- OYNANABILIR tam döngü: MainMenu → Attunement Chamber (yürünebilir diegetic karakter seçimi: gerçek odada WASD ile yürü, heykellere E ile bürün, kapıdan çıkınca run başlar) → _Arena run (state machine: combat→clear→slow-mo→reward draft→branching doors→boss→victory/death→meta-currency award)
- Data-driven oda sistemi: RoomTemplateSO (ScriptableObject) + IsoRoomBuilder (runtime izometrik ada inşası + otomatik cliff yerleşimi) + 26 oda template + JSON importer (ChatGPT'ye oda tasarlatıp içe aktarma) + 7-sekmeli Map Designer editör aracı + Poisson-disk prop auto-placement (Bridson algoritması + kompozisyon rol haritası)
- Combat feel: kod-only knockdown sistemi (animasyon üretmeden parabol+eğme+squash), skill draft (3-kart Hades-tarzı), tooltip/codex UI
- Çok-ajanlı geliştirme metodolojisi: Claude orchestrator + Codex (kod) + Gemini council (karar) + yazar≠reviewer cross-QC + görsel oda QC (screenshot tabanlı) + EditMode/PlayMode test suite
- Asset pipeline: PixelLab AI pixel-art üretimi (10 sınıf × 8 yön karakter), kurallı pipeline (PPU 64, Point filter, integer scaling)

## Senin lensin: DERİN MİMARİ / AKADEMİK ÇERÇEVE
1. Bu içerikten akademik olarak EN değerli "katkı" (contribution) anlatısı hangisi? (a) data-driven prosedürel oda/level tooling, (b) AI-destekli çok-ajanlı oyun geliştirme metodolojisi, (c) kod-only game-feel sistemleri, (d) bütünleşik vaka çalışması? Bitirme projesi jürisi gözüyle değerlendir.
2. Rapor bölüm mimarisi öner: başlıktan sonuca tam iskelet (Türkçe akademik konvansiyon: Özet, Giriş, İlgili Çalışmalar, Yöntem, Sistem Tasarımı, Uygulama, Test/Doğrulama, Sonuç). Hangi sistem hangi bölüme?
3. "İlgili Çalışmalar" için hangi karşılaştırmalar güçlü olur? (Hades/Dead Cells/StS level-flow; Wave Function Collapse/Poisson sampling literatürü; AI-assisted development literatürü)
4. Çok-ajanlı geliştirme pipeline'ı rapora "Yöntem" bölümü olarak girmeli mi yoksa ayrı bir "Geliştirme Metodolojisi" bölümü mü? Akademik riskleri var mı (örn. "kodu AI yazdı" algısı)? Nasıl çerçevelenmeli?
5. Hangi figürler/diyagramlar şart? (mimari diyagram, state machine, veri akışı, ekran görüntüleri — somut liste)
6. Mevcut plandaki en büyük boşluk/zayıflık ne?

Cevabı Türkçe ver, madde madde, net önerilerle.
