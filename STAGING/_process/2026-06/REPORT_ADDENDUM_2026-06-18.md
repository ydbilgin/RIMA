# RAPOR ADDENDUM — 2026-06-18 demo-hardening oturumu

> Bu, 2026-06-06 raporundan SONRA eklenen demo-proven işlerin rapora entegre edilecek taslağıdır. Tam Türkçe karakterlerle yazılmıştır. Kullanıcı uygun bölümlere (özellikle §3 Sistem/Mimari ve §7 Karşılaşılan Zorluklar) yerleştirecek. Hepsi kod-confirmed; commit referansları verilmiştir.

## §3'e (Sistem/Mimari notları)

**Hareket güvenliği — yetenek tabanlı ışınlanma/atılma.** Blink (ışınlanma), Iron Charge ve Blade Rush atılma yetenekleri başlangıçta yürünebilirlik haritasını (WalkabilityMap) atlıyor, oyuncuyu harita dışına (void) gönderebiliyordu. Bu yetenekler, oyuncu hareketinde zaten kullanılan `WalkabilityMap.IsDashableWorld` / `ClampVelocityToWalkable` katmanından geçirilerek tek bir yetkili yürünebilirlik kapısına bağlandı (commit `6ba61ff5`).

**Görsel atmosfer — URP son-işleme.** Sahneye URP Volume tabanlı son-işleme eklendi: Bloom (rift ve ayna parıltısı), Color Adjustments (pozlama/kontrast), Neutral Tonemapping ve Vignette; ek olarak global 2D ışık seviyesi ve brazier'a sıcak nokta-ışık. Kamera son-işleme aktif edildi (commit `9359b7a5`). Bu, "yüzen ada + void" kanonunu koruyarak okunabilirliği artırır.

**Editör deneme alanı (Chamber).** Antrenman odasında oyuncu artık `[K]` ile aktif sınıfın tüm uygulanmış yetenek listesinden (SkillDatabase) istediğini Q/E/R/F yuvalarına atayıp kukla üzerinde deneyebilir. Seçim yalnızca oda-içi geçerlidir (bellekte tutulur, oyuna girişte sıfırlanır); koşu yükü/draft akışına dokunmaz (commit `981ac783`).

## §7'ye (Karşılaşılan Zorluklar ve Çözümler) — güçlü vaka örnekleri

**Yetenek aktivasyonunda "harca-sonra-veto" hatası.** Bazı yetenekler hedef menzilde olmasa bile (no-op) mana/bekleme süresini tüketiyor, "ölü buton" hissi yaratıyordu. Temel sınıfa (`SkillBase`) maliyet/bekleme harcanmadan ÖNCE çalışan bir `CanExecute()` veto kancası eklendi; menzil-kapılı yetenekler bunu uygulayarak boşa harcamayı engelliyor (commit `6ba61ff5`). Ayrıca yetersiz-kaynak ve veto durumlarında kısa ses + atıcı parlaması + bildirim eklendi; bekleme süresindeki tuş tekrarları bilinçli olarak sessiz bırakıldı (commit `d0e6466e`).

**Meta-ilerleme senkron kaybı.** `_Arena` koşu yönlendiricisi oda-temizleme olayını ilerleme istatistiklerine bildirmiyordu; bu yüzden temizlenen oda sayısı 0 kalıyor, ölüm/zafer ekranı ve Echo ödülü hep "Oda 1" gösteriyordu. `RoomRunDirector` ile `RunStats` arasına oda-temizleme köprüsü kuruldu (commit `6ba61ff5`).

**Boss faz bütünlüğü.** Penitent Sovereign'da yüksek anlık hasar (burst) Faz-2'yi atlayıp doğrudan Faz-3'e geçirebiliyordu. Faz-2'ye giriş zamanı kaydedilip Faz-3 tetikleyicisi en az 8 saniyelik bir faz-kilidiyle kapatıldı; böylece Faz-2 mekaniği her zaman görünür (commit `6ba61ff5`). (Not: boss saldırı telegraph sistemi P1/P2/P3'te zaten tam kapsanmış durumda.)

**Editör aracı sağlamlığı (Director Mode).** Zaten bir yuvada bulunan yetenek ikinci bir yuvaya atanınca aynı bileşen paylaşılıp bekleme süreleri bozuluyordu. Atama yolunda, bileşen eklenmeden ÖNCE çift-yuva reddi eklendi (mevcut hata-bildirim arayüzü kullanılarak), öksüz bileşen sızıntısı önlendi (commit `d0e6466e`).

**Editör-içi demo stabilitesi.** Önceki oturumdan: çift-sahip timeScale donması (HitStop × HitPauseDriver) tek sürücüye yönlendirildi; sınırsız FPS kaynaklı sürücü thrash'i 60-FPS kapağı (FrameRateGuard) ile çözüldü. Demo editörde sunulacağı için bu stabilite kritik.

## Cooldown göstergesi
Yetenek çubuğunda bekleme süresi artık her aktif cooldown'da sayısal olarak gösteriliyor (≥1 sn tam sayı, <1 sn ondalık); önceden yalnızca ≥5 sn eşiğindeydi (commit `981ac783`).

---
**Metodoloji notu (§ uygunsa):** Bu oturumdaki kararlar çok-danışmanlı sentez (Codex + iki Gemini modeli) ve bağımsız bir adversarial Opus eleştirmeni ile alındı; eleştirmen, riskli bir öneriyi (run-içi para birimi göçü) kanıtla reddederek demo-arifesi kapsamını daralttı. Bu, projedeki AI-destekli geliştirme disiplinine somut bir örnektir.
