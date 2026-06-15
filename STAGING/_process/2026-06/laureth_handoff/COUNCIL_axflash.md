# COUNCIL — RIMA Mekanik & Polish Değerlendirmesi (ax_flash)

## S1 — Grup A (A1-A6) Combat-Feel Önerileri
- **A1 (Flowstep):** POST — Combo/akıcılık ödülü oynanış dengesini değiştirebilir; golden-path GREEN doğrulandığından risk almaya değmez.
- **A2 (Posture-Crack & Shattered Glyphs):** POST — Mevcut Posture altyapısına yaslanan, sunum etkisini en yüksek artıran ve risk/efor oranı en optimize combat-feel adayıdır.
- **A3 (Aggression-Gated Heal):** POST — Gri-can/anti-turtle mekaniğinin 5 gün kala solo-dev tarafından dengelenmesi aşırı yüksek risklidir.
- **A4 (Sweet-Spot Damage):** POST — Pozisyonlama hasarı nice-to-have bir eklentidir, öncelikli değildir.
- **A5 (Rolling/Deferred Health):** POST — A3 ile benzer görsel-mekanik cila olup karmaşıklığı artıracaktır.
- **A6 (Accessibility Contract):** POST — Video odaklı tech-demo sunumunda jürinin doğrudan oynamayacağı varsayımıyla gereksizdir.

## S2 — Grup B (B1-B10) Derinlik & Meta Önerileri
*Not: Karar#25 (Hub-upgrade/kalıcı meta yasağı) ile çakışan maddeler: B2 (Kalıcı run-mutator), B5 (Run-arası kalıcı miras), B10 (Hub-bank kalıcı para aktarımı).*
- **B1 (Fragment Contract Door):** POST — Run-içi kontratlar Karar#25'i çiğnemez, run-graph mimarisini anlamlandırmak için post-demo dönemde güçlü bir adaydır.
- **B2 (Hub Rift room-mutator):** DROP — Karar#25 kalıcı hub-meta kuralı ile doğrudan çakışır.
- **B3 (Curse Gate):** POST — Görüş daraltma/light limitleri gibi görsel riskler taşır, post-demo için uygundur.
- **B4 (Resonance Forecast):** POST — Draft kartlarının okunabilirliğini artıran, eforu orta düzeyde bir tasarım iyileştirmesidir.
- **B5 (Echo Memory Bias):** POST — Sadece run-içi geçici bias ile sınırlandırılırsa (Karar#25 çiğnemeden) post-demo uygulanabilir.
- **B6 (Luck Stat):** POST — Görünmez stat derinliği demo sonrasında da geri plana atılabilir.
- **B7 (Placement-Driven Resource):** POST — Oda geometrisi ile combat'ı bağlayan yenilikçi bir fikir, post-demo için idealdir.
- **B8 (Status Layering):** POST — Çok sınıflı aşamaya (Faz 3+) geçildiğinde anlam kazanacak bir derinlik katmanıdır.
- **B9 (Fixed-Verb + Rotating Environmental Rule):** POST (ÖNCELİKLİ) — Environment ve authoring/tooling tezimizi en güçlü besleyen, tek sınıfla oda bazlı kuralları (rüzgar, kırılgan zemin vb.) Director ile yönetmeyi sağlayan en değerli meta fikridir.
- **B10 (Drop & Bank):** POST — Sadece run-içi drop/retrieval kısmı (hub-bank'e dokunmadan) gerilim yaratmak için post-demo denenebilir.

## S3 — Polish (Tier-1/2/3) Değerlendirmesi
- **Zaten Var Olanlar:** Tier-1 #3 (Screen Shake - ScreenShakeDriver/CameraShake), Tier-1 #4 (Hit-Stop - HitStop/HitPauseDriver), Tier-1 #5 (Damage# Pop - DamageNumberDriver/DamagePopup), Düşman Telegraph (EnemyTelegraph), Posture Sistemi (x3.0 cap).
- **Değerlendirme:** Geri kalan tüm polish maddeleri **POST (Tamamen Demo Sonrasına)** ertelenmelidir. Golden-path videosu ve Edit-to-Play akışı az önce GREEN olarak doğrulanmıştır. Herhangi bir görsel/animasyon cilası eklemek (örn. vignette, shadow, ghost-trail), golden-path'i destabilize etme riski taşıdığından demo penceresinde kabul edilemez.

## S4 — META Bağımsız Yargı
- **Orchestrator Eğilimi Değerlendirmesi:** Kısmen EKSİK.
- **Gerekçe:** Orchestrator'ın demo scope-lock'u koruma ve her şeyi erteleme kararı kesinlikle doğrudur (golden-path'i bozmamak 1 numaralı önceliktir). Ancak, post-demo vizyonunda sadece A2'yi (Posture-Crack) en güçlü aday olarak öne çıkarması eksiktir. RIMA'nın asıl tezi "oyun değil, environment/tooling" olduğundan, bu tezi en çok besleyen ve oda tasarım araçlarımızı oynanışa bağlayan **B9 (Rotating Environmental Rule)** post-demo için A2 ile birlikte en güçlü öncelik olmalıdır.

**TEK CÜMLE TAVSİYE:** Golden-path doğrulaması sıfır hata ile tamamlanmışken demo kapsamı kesinlikle kilitli tutulmalı (hepsi POST/DROP), demo sonrası süreçte ise combat-feel için A2 ile mimari/tooling tezi için B9 paralel olarak önceliklendirilmelidir.
