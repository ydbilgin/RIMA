# 09 — Pack'teki 6 Sorunun Sıralı Yanıtı

## 1. Combat fix yeterli mi?

**Hayır, kısmi ve doğru yönde bir fix.**

`detectionRange 8 → 12` ve Player re-acquire:

- eski spawn mesafesi sorununu çözer
- spawn-order yarışına karşı yardımcı olur
- sentetik testte chase/attack'i açabilir

Ancak sistem hâlâ `Player` tag'ine bağlıdır. Üstelik bazı saldırılar hasar verirken collider üzerinde doğrudan `CompareTag("Player")` kullanır. Bu nedenle yalnızca transform bulmak yetmez.

`DemoPlayer` untagged riski gerçek bir demo-killer'dır, fakat sahne objesinin untagged olması gerçek runtime karakterin de untagged olduğunu tek başına kanıtlamaz. CharacterSelect → `_Arena` gerçek yolunda instantiate edilen canonical player root incelenmelidir.

Ayrıca boss Player'ı yalnızca `Start()` içinde arar ve re-acquire yapmaz. Bu da ayrı bir blocker'dır.

**Kesin doğrulama:** cold start, real flow, chase, player damage, enemy death, wave clear, draft, next room ve boss zincirini üç kez çalıştır.

## 2. Penitent lethality risk mi?

**Evet. Token gating yeterli değil.**

Combo toplamı 85/100 HP. Token sistemi aynı anda kaç saldırganın saldırı başlatacağını sınırlar; Penitent'in tek combo burst'ünü azaltmaz ve başka melee saldırıyla çakışmayı engellemez.

Encounter asset'inde Penitent `maxSimultaneous: 1`, dolayısıyla iki Penitent varsayımı doğrulanmalıdır. Asıl güvenilir risk: Penitent + başka melee ve 85 HP burst.

**Demo kararı:**

- Opening wave'den çıkar.
- Wave 2/3'e koy.
- Combo toplamını yaklaşık 40–50'ye indir.
- Başlangıç telegraph'ını uzat.
- Üçüncü vuruşa ayrı tell ekle.

## 3. İki günlük bitirme planı doğru mu?

İçerik doğru, fakat sıra güncellenmelidir.

### Doğru sıra

1. Player target/tag
2. Boss re-acquire
3. AttackTokenManager lifecycle
4. Penitent demo tuning
5. Full-flow manual gate
6. CombatJuice
7. Enemy outline
8. Kritik boss telegraph'ları
9. Edit-to-Play canlı kanıtı
10. Screenshot recapture
11. Yedek video

Combat kapanmadan prop/capture polish'e geçmek yanlış önceliktir.

## 4. 19 Haziran öncesi demo-killer riskler

- Runtime Player tag yanlışlığı
- Boss'un Player'ı hiç bulamaması
- AttackTokenManager'ın scene destroy sonrası `_shuttingDown` kalması
- Penitent opening burst
- CombatJuice sonrası `Time.timeScale` sızıntısı
- F2/Director sonrası input veya combat'ın dönmemesi
- reward/death overlay'in odalar arasında kalması
- portal/room transition soft-lock
- Build prop collider soft-lock
- boss ölümü sonrası continuation tetiklenmemesi
- çift EventSystem/input
- screenshot'ların yanlış state'i kanıtlaması
- canlı demo için yedek videonun olmaması

## 5. Environment + tooling tezi destekleniyor mu?

**Evet, fakat en güçlü kanıt henüz düzgün gösterilmiyor.**

Repo yapısı tezi destekliyor:

- F2 Build Mode
- room persistence
- Director runtime stat/spawn/telemetry
- data-driven encounter asset
- draft/run-map
- reusable combat/telegraph systems

Zayıf nokta, reusability'nin tek bir kesintisiz canlı beat ile gösterilmemesi.

En güçlü demo:

`F2 place → exit F2 → same prop in play → Director stat change → Director spawn → combat → reward → run-map`

Director'ın güzel görünmesi tek başına tooling kanıtı değildir. Yapılan işlemin dünyadaki sonucu görünmelidir.

## 6. Telegraph yaklaşımı sağlam mı?

**Mevcut `RIMA.EnemyTelegraph` sistemini reuse etmek doğru karardır.**

Yeni framework yazılmamalı.

Öncelik:

1. ChainExplosion delayed rings
2. Wrath danger/safe-zone
3. Charge line
4. HolyLash arc
5. FractureStrike
6. Shackle flash opsiyonel

Kritik teknik şart:

- Telegraph başında origin/direction/marker pozisyonlarını snapshot al.
- Damage aynı snapshot'ı kullansın.
- Safe zone farklı renkte/biçimde okunmalı.
- FlashImpact, CombatJuice ile çift feedback oluşturmamalı.
- Telegraph süresi gerçek damage timing'iyle birebir eşleşmeli.
