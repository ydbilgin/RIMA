# ChatGPT'ye Atılacak Prompt — Rapor Review Turu 2 (2026-06-07)

> Kullanım: Aşağıdaki prompt'u kopyala, ChatGPT'ye yapıştır, yanına şu dosyaları ekle:
> 1. **`RAPOR_RIMA_2026-06-06.docx`** (zorunlu — güncel rapor, şekiller gömülü)
> 2. `GAME_DESIGN_INTENT_2026-06-06.md` (önerilir — oyunun tasarım niyeti, raporu bağlamla okumasını sağlar)
> 3. `VISION_VS_CURRENT_2026-06-06.md` (opsiyonel — vizyon vs mevcut durum ayrımı, abartı tespitinde referans)

---

## PROMPT (buradan aşağısını kopyala)

Sana bitirme projemin (senior design) final rapor taslağını gönderiyorum. Bu **ikinci review turu** — ilk turdaki geri bildirimlerin (kaynakça, şekil atıfları, dil temizliği, abartılı iddiaların törpülenmesi) büyük kısmı uygulandı.

**Proje özeti:** RIMA — Unity 6 ile tek geliştirici tarafından yapılan 2D izometrik roguelite. Raporun iki ana katkısı var: (1) oyunun kendisi ve veri-güdümlü oda/araç sistemleri, (2) tek geliştiricinin kapsamı yetiştirebilmek için kurduğu çok-ajanlı yapay zekâ geliştirme metodolojisi (yazar≠reviewer kuralı, otonom gece kuyrukları, üçlü kalite güvencesi).

**Bu turda rapora YENİ eklenen bölümler** (özellikle bunlara odaklan):
1. **Gate-slot kapı sistemi** (§3.5.5 genişletildi) — authored NW/N/NE kapı slot'ları, kapı sayısına göre seçim kuralı, 25 şablonun migrasyonu
2. **UI↔JSON çift yönlü oda editörü** (§3.5.6 yeni) — editör içi boyama toolbar'ı + ScriptableObject-canonical / JSON-export mimarisi + round-trip testleri
3. **Walkable fizik zorlaması** (§3.2'ye eklendi) — donut odaların iç deliği, knockback/mob clamp'leri, tünelleme analizi
4. **ScreenshotMode aracı** (§3.5'e eklendi) — deterministik seed ile tekrarlanabilir rapor görselleri
5. **Oyun hissi katmanı** (§2'ye eklendi) — hit-pause/shake tier'ları, infaz prompt'u, ses entegrasyonu, dash input buffer
6. **Reviewer-FAIL vakası** (§5/§6'ya eklendi) — bağımsız review'ın ilk teslimde 9 gerçek bug yakalaması; metodolojinin çalıştığının kanıtı olarak sunuldu
7. Test tablosu (Tablo 6.1) yeni test gruplarıyla güncellendi

**Senden istediklerim (jüri gözüyle oku):**
1. **Yeni bölümler raporun geri kalanıyla üslup ve derinlik olarak tutarlı mı?** Yamanmış gibi duran yer var mı?
2. **Savunulabilirlik:** Jürinin "bunu kanıtlayabilir misin?" diyeceği iddia kaldı mı? Özellikle yeni bölümlerde abartı/şişirme kokusu var mı? (Raporun ilkesi: dürüst kapsam beyanı — örn. "10 sınıflık veri modeli, 4'ü uçtan uca oynanabilir")
3. **Akış:** Yeni eklemelerle bölüm geçişleri bozuldu mu? Tekrara düşen, kısaltılabilecek pasajlar?
4. **Metodoloji bölümünün ikna ediciliği:** "9 bug yakalayan reviewer" vakası iyi konumlanmış mı, yoksa anekdot gibi mi duruyor? Jüriye nasıl daha güçlü sunulur?
5. **Eksik:** Bir bitirme jürisinin arayıp da bulamayacağı bir şey var mı (ölçüm, karşılaştırma, literatür bağı, sınırlılıklar)?
6. **Şekiller:** Gömülü şekiller metinle eşleşiyor mu, atıfsız/alakasız şekil var mı?

**Çıktı formatı:** Numaralı, eyleme dönüştürülebilir madde listesi. Her madde: [bölüm referansı] + sorun + somut öneri + önem derecesi (KRİTİK/ORTA/KOZMETİK). Raporu baştan yazma, toptan yeniden yapılandırma önerme — cerrahi düzeltme listesi istiyorum. Türkçe yanıt ver.
