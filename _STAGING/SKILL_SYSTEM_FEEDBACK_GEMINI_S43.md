# RIMA Skill System Feedback — Gemini S43

**Date:** 2026-04-28
**Author:** Gemini
**Scope:** Character skills, mob skills, boss skills, cross-class consistency.

## Executive Verdict
RIMA'nın mevcut yetenek sistemi ve combat tasarımı S41/S42 revizyonlarıyla birlikte MMORPG hantallığından kurtulmuş, hızlı ve "Hades-vari" roguelite aksiyon temeline tam oturmuştur. "Bekle-kazan" ve "stat-buff" yeteneklerinin temizlenmesi oyunun kalitesini büyük ölçüde artırmıştır. Ancak, Unity projesindeki asset'ler ve ana tasarım dokümanları (TASARIM klasörü) bu son kararların çok gerisinde kalmıştır. Acil olarak doküman senkronizasyonu ve Unity asset temizliği gerekmektedir.

## Source Drift / Document Conflicts
- **`TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` (STALE):** Shadowblade ve Ranger'ın tam redesign'larını, Warblade ve diğer sınıfların isim değişikliklerini (S41 `SKILL_REVIZYON_PLANI.md`) içermemektedir.
- **`TASARIM/CROSS_CLASS_SKILL_MATRIX.md` (STALE):** Tamamen eski yetenek isimlerine (Kidney Shot, Flare, Aimed Shot vb.) göre yazılıdır. S41 revizyonu sonrası köprü yetenekleri ve sinerjilerin çoğu geçersiz hale gelmiştir.
- **`TASARIM/BOSS_DESIGN.md` (STALE):** Penitent Sovereign boss'u 2 fazlı görünmektedir. S42 `MOB_BOSS_REDESIGN_S42.md` dokümanındaki 3 fazlı tasarım çok daha iyidir ve aktif kabul edilmelidir.
- **`TASARIM/COMBAT_ROSTER.md` (STALE):** Düşman tasarımları Act 1 için S42'nin gerisindedir (Ruin Hulk sahte tehdidi, eski auto-attack ağırlıklı moblar burada yer almaktadır).

## Character Skill Review

### Warblade
- **Değerlendirme:** Sınıf fantezisi (Rage, zırh kırma, infaz) çok güçlü. S41 revizyonundaki `War Stomp` yerine `Earthsplitter` kararı silüet çakışmasını önlemek için harika bir detay.
- **Tavsiye:** `Iron Counter` yeteneği piksel art ortamında zor okunabilir. Counter penceresi (0.8s) çok net bir parlama (tell glow) ile desteklenmeli. Tüm S41 değişikliklerini koruyun.

### Elementalist
- **Değerlendirme:** Elementler arası ritim ve Lightbreak mekaniği başarılı. `Prism Lance` -> `Prism Beam` (Cursor channel) ve `Combustion` -> `Element Charge` gibi değişiklikler yeteneklerin MMO filler hissiyatını tamamen yok etmiş.
- **Tavsiye:** Elementalist S41 tasarımı mükemmel. Değiştirmeyin. Fire/Frost/Radiance poz ayrımını animasyonlarda kesinlikle uygulayın.

### Shadowblade
- **Değerlendirme:** S41 tam redesign'ı sınıfı kurtarmış. Eski WoW Rogue klonu yetenekler (Vanish, Fan of Knives) yerine `Veil Burst`, `Phase Step`, `Death Mark` gelmesi, Void/Rift temalı "Yankı/Scar" mekaniğini çok iyi yansıtıyor.
- **Tavsiye:** Yeni Sever kaynağı ve Rift Scar mekaniği harika. Tamamen bu S41 versiyonunda kalın, eski tasarımı çöpe atın.

### Ranger
- **Değerlendirme:** Shadowblade gibi Ranger da MMO Hunter yükünden (Flare, Tethering Arrow) kurtulmuş. "Focus" kaynağının mesafeye bağlanması mükemmel bir risk/ödül mekaniği.
- **Tavsiye:** `Rift Arrow` (şarjlı) ve `Predator's Mark` gibi yeni yeteneklerle kite döngüsü çok daha aktif. S41 tasarımını koruyun. `Skirmish Shot` önerisi hareketliliği artırdığı için kesinlikle tutulmalı.

### Ravager
- **Değerlendirme:** Sadece hasar alarak dolan Fury kaynağı Warblade ile harika bir zıtlık oluşturuyor. `Intimidating Shout` -> `Bloodied Roar` ve `Battle Cry` -> `Blood Pact` (HP harcama) değişiklikleri fanteziyi pekiştirmiş.
- **Tavsiye:** `Blood Pact` riskli ama ödüllendirici. Onaylayın ve S41 isimlerini ana dokümana geçirin.

### Ronin
- **Değerlendirme:** Tension kaynağı ve sürekli hareket zorunluluğu çok dinamik. Iaido stili hızlı combat için ideal.
- **Tavsiye:** S41 isim değişiklikleri (`Sōken-giri`, `Sakura Veil`) temayı tamamlıyor. Geçerli kılın.

### Gunslinger
- **Değerlendirme:** Heat ve Overheat mekaniği oyuncuyu agresif ama dikkatli olmaya zorluyor. `Critical Shot` gibi pasif/basit yeteneklerin `Deadshot`'a, `Dead Eye`'ın `Rift Grenade`'e çevrilmesi aksiyonu artırmış.
- **Tavsiye:** S41 redesign'ı çok iyi. Batı kamerasından çıkıp Rift/Dark Fantasy "Rift-marked bandana" tarzına geçişi tasarıma tam oturtun.

### Brawler
- **Değerlendirme:** Charge mekaniği ve combo zincirleri (Weave penceresi) dövüş oyunu hissini Top-Down ARPG'ye güzel aktarmış.
- **Tavsiye:** `Rush Combo` -> `Combo Chain` animasyon netliği açısından doğru. Değişiklikleri onaylayın.

### Summoner
- **Değerlendirme:** Pasif "pet" sınıfı yerine minyonları aktif feda etme mekaniği oyunu dinamik tutuyor. `Command Beacon` eklemesi kontrolü artırmış.
- **Tavsiye:** Minyon silüetlerinin oyuncuyu ve mobları gölgelememesine dikkat edin (görsel karmaşa riski). S41 tasarımını koruyun.

### Hexer
- **Değerlendirme:** Hex Stack biriktirme ve Hexblast patlaması sınıfın omurgası. 
- **Tavsiye:** S41 notlarında belirtilen `Soul Bargain` yerine `Blight Sigil` (cursor curse zone) yeteneğinin eklenmesini kesinlikle uygulayın. Soul Bargain'in HP trade mekaniği Ravager/Summoner ile çakışıyor, `Blight Sigil` ise Hexer'a daha çok alan kontrolü sağlıyor.

## Mob Skill Review
- **Değerlendirme:** Faz 1 S42 redesign'ı (`MOB_BOSS_REDESIGN_S42.md`) harika bir iş çıkarmış. Her düşman (Imp sürüsü, Walker uzak baskısı, Crawler yeraltı tehdidi) oyuncudan farklı bir refleks istiyor. Temas hasarı ve auto-attack'in azaltılıp yetenek pencerelerine (tell -> window) geçilmesi Hades tarzı combat'ı mükemmel sağlamış.
- **Tavsiye:** `COMBAT_ROSTER.md` dosyasındaki eski Act 1 moblarını görmezden gelin. Seam Crawler + Chain Warden Echo kombosu çok zorlu olabilir, ilk odalarda beraber spawn olmalarını engelleyen bir kural koyun. (PLAYTEST NEEDED)

## Boss Skill Review
- **Değerlendirme:** Penitent Sovereign boss'unun S42'deki 3 fazlı (Zincirin Altında -> Kırılan Zincir -> Sovereign Awakened) tasarımı, eski 2 fazlı tasarıma kıyasla çok daha destansı ve eğiticidir. Ortaya çıkan Rift Tear hazard'ı arena kontrolünü öğretiyor.
- **Tavsiye:** `BOSS_DESIGN.md` içerisindeki Penitent Sovereign tasarımını çöpe atın. S42'deki 3 fazlı yapıyı baz alın.

## Cross-Class Review
- **Değerlendirme:** `CROSS_CLASS_SKILL_MATRIX.md` tamamen eskimiştir. Yeni yetenek isimleri (Shadowblade ve Ranger özelinde) bu matristeki tüm "Sinerji" ve "Auto Pasif" mekaniklerini geçersiz kılmıştır.
- **Tavsiye:** Matrix'i şu an manuel olarak yamamaya çalışmayın. `SINIF_VE_SKILL_KARAR_BELGESI.md` S41 notlarına göre tamamen güncellendikten sonra, Cross-Class Matrix'i sıfırdan yeniden oluşturun. Köprü mantığı (ör. "root → execute") hala çok iyi, ancak kullanılan skill isimleri ve koşulları yeni yeteneklere göre yeniden yazılmalıdır.

## Implementation Risk
- **Tehlike:** Unity projesi içerisindeki `Assets/Data/Skills/` ve `Assets/Data/CrossClass/` klasörleri test amaçlı oluşturulmuş eski, çöp asset'lerle dolu (`Skill_WhirlwindSlash`, `CCS_Elem_EmberTouch` vb.). Bu durum geliştirme sırasında yanlış yeteneğin koda dökülmesine yol açabilir.
- **Tavsiye:** Mevcut `Data/Skills` ve `Data/CrossClass` klasörlerinin içini tamamen temizleyin (veya bir `_Legacy` klasörüne taşıyın). Implementasyona sadece Warblade'in Faz 1 S41 onaylı güncel 6 yeteneğinin ScriptableObject'lerini oluşturarak sıfırdan başlayın.

## Highest Priority Changes
1. **Doküman Senkronizasyonu:** `SINIF_VE_SKILL_KARAR_BELGESI.md` ve `CROSS_CLASS_SKILL_MATRIX.md` dosyalarını S41 `SKILL_REVIZYON_PLANI.md` standartlarına çekin.
2. **Asset Temizliği:** Unity projesindeki eski Skill ve CrossClass ScriptableObject'lerini temizleyin.
3. **Boss Tasarımı:** Penitent Sovereign'i sadece 3 fazlı S42 versiyonu olarak referans alın.

## Final Recommendation
Tasarımsal olarak RIMA mükemmel bir noktaya (S42) ulaşmıştır. Artık "tasarım düşünmeyi" bırakıp, mevcut `_STAGING` onaylı kararları ana belgelere (TASARIM klasörü) mühürlemeli ve Unity'de Warblade'in yeteneklerini kodlamaya başlamalısınız. Yeni fikir aramayın, mevcut temizlenmiş S42 planını hayata geçirin.