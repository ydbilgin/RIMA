# RIMA Tam Oyun Audit — Lens 1: COMBAT / HASAR / STAT MANTIĞI
Tarih: 2026-06-13 · READ-ONLY inceleme · Kapsam: `Assets/Scripts/**` hasar yolları, stat/scaling, ara katmanlar, sayısal tuzaklar.
Not: PlayerProjectile packet'siz dalı + DirectorMode DPS penceresi bugün fix'lendi (uncommitted) — mevcut haliyle değerlendirildi, yeniden raporlanmadı.

---

## Mimari özet (hasar yolu haritası)

Merkezi yol `SkillRuntime.DealDamage(...)` → `DamageCalculator.Calculate(...)` → `Health.TakeDamage(int)`.
Dört giriş overload'u var ve **stat-scaling + status çarpanı + event davranışları birbirinden farklı**:

| Tip | İmza (SkillRuntime.cs) | bypassStatScaling | applyStatusMultiplier | OnDamageApplied + PublishSkillHit |
|---|---|---|---|---|
| 1 | `DealDamage(hp, int, Component source, popup=false)` :97 | **TRUE** (zorla) | **FALSE** (zorla) | VAR (DealDamage içinden) |
| 2 | `DealDamage(hp, int, popup, GameObject, ...)` :107 | **TRUE** (zorla) | default TRUE | VAR |
| 3 | `DealDamage(hp, DamagePacket, ...)` :129 | packet'ten (Create default=**FALSE** → SCALED) | default TRUE | VAR |
| 4 | `DealDamage(hp, int, popup=true)` :92 → Tip 2'ye (statusMult=false) | **TRUE** | **FALSE** | VAR |
| — | çıplak `hp.TakeDamage(int)` | yok | yok | **YOK** (telemetri+VFXRouter sessiz) |

Sonuç: **stat slider'ı (physPower/abilityPower) yalnızca Tip-3 (DamagePacket, bypass=false) yoluna işler.** Oyuncu yeteneklerinin ~%96'sı Tip-1/2/4 → stat'a sağır.

---

## BULGULAR

### 🟠 HIGH — Stat-scaling 52/54 yetenekte BYPASS, sadece LMB/projectile scaled
`SkillRuntime.cs:104,107-120` — Tip-1 ve Tip-2 overload'ları packet'i `bypassStatScaling:true` ile kuruyor.
Tüm Q/E/R/F skill'leri (Elementalist, Warblade, Shadowblade, Ranger, Ronin) Tip-1/4 çağırıyor → physPower/abilityPower slider'ı bu hasarlara HİÇ işlemiyor. Sadece basic-attack LMB (`BasicAttackBehaviorBase.cs:70-79` packet, bypass=false → SCALED) ve Ranger ok'u (`ShotCadenceBehavior.cs:57` SCALED) stat'a tepki veriyor.
Demo gözüyle: stat slider'ı oynatınca yalnız LMB sayısı değişir, yetenek hasarları sabit kalır — "stat sistemi var" iddiası yetenek tarafında YANILTICI. Tasarımsal karar olabilir ama tutarsız.
Minimal fix: demo-only ise dokunma; tutarlılık isteniyorsa skill çağrılarını DamagePacket (bypass=false) tipine taşı veya en azından demo betiğinde "yetenekler bypass, LMB scaled" notu netleşsin.

### 🟠 HIGH — MarkPulseBehavior (Ravager LMB) merkezi yolu TAMAMEN atlıyor
`MarkPulseBehavior.cs:127` `hp.TakeDamage(finalDmg)` çıplak.
Sonuç: (a) DamageCalculator hiç çalışmaz → armor/magicResist/identityBuild/situational/crit/debugGlobalDamageMult HİÇBİRİ uygulanmaz; (b) `OnDamageApplied` telemetri publish edilmez → **DirectorMode DPS penceresinde Ravager LMB görünmez**; (c) `PublishSkillHit`/CombatEventBus yok → VFXRouter hit/kill burst'leri ÇIKMAZ. Status çarpanını `:124` elle uyguluyor ama gerisi eksik.
Karşılaştırma: kardeş davranış `BasicAttackBehaviorBase.ApplyMeleeHit` (Warblade) düzgün packet yolu kullanıyor — bu ikisi tutarsız.
Minimal fix: `:127` bloğunu `ApplyMeleeHit` gibi `SkillRuntime.DealDamage(hp, packet, popup:false, ...)` çağrısına çevir; elle status çarpanı + elle popup `:130` satırlarını sil (DealDamage zaten yapar).

### 🟡 MEDIUM — debugGlobalDamageMult bypass yolunda da uygulanıyor, ama TakeDamage yolunda uygulanmıyor (tutarsız)
`DamageCalculator.cs:45,52` — debugMult bypassStatScaling'ten BAĞIMSIZ, her DamageCalculator yoluna işler (Tip-1/2/3/4 hepsi). Bu İYİ.
Ama çıplak `hp.TakeDamage` yolları (MarkPulse, tüm enemy attack'ları, StatusEffect DoT'ları) DamageCalculator'a hiç girmediği için debug çarpanına SAĞIR. "DEBUG HASAR ÇARPANI" slider'ı (DirectorMode.cs:1588) Ravager LMB'ye ve düşman hasarına etki etmez.
Minimal fix: demo'da slider sadece player skill/LMB ölçeklemesi için sunuluyorsa kabul; aksi halde MarkPulse fix'i (yukarıda) bunu da çözer.

### 🟡 MEDIUM — Status çarpanı (Weakened/Scorch) yeteneklerin %96'sında uygulanmıyor
Tip-1 ve Tip-4 `applyStatusMultiplier=false`. Yani düşmana Weakened (+%25 alınan hasar) / Scorch (+%25) bindirildiğinde, oyuncunun Q/E/R/F yetenekleri bu zaafiyetten YARARLANMAZ — sadece LMB ve projectile (Tip-3) yararlanır.
`StatusEffectSystem.cs:194-196` çarpanı doğru hesaplıyor; sorun çağrı tarafında atlanması.
Minimal fix: skill çağrılarını Tip-3'e taşımak veya Tip-1 çağrılarını `applyStatusMultiplier:true` ile yapan bir overload kullanmak. Demo-kritik değil (debuff combo demo'da nadiren kurulur).

### 🟡 MEDIUM — DamageZone "first-frame ücretsiz tick" + OnTriggerStay zamanlaması
`DamageZone.cs:49-53` — `tickTimer` 0'dan başlar; ilk `OnTriggerStay2D` çağrısında `tickTimer += dt` küçük kalır, ilk tick `tickInterval` dolana kadar gecikir (sorun değil). Ancak `tickTimer` zone içindeki TÜM hedefler için TEK sayaç — birden çok düşman varsa OnTriggerStay her hedef için ayrı çağrılır, `tickTimer` her çağrıda artar → çok-hedefli zone'da tick aralığı hedef sayısına bölünür (hedef başına değil, toplam). Yani 3 düşman varsa hasar ~3x sık atılır ama rastgele hedefe.
Minimal fix: per-collider tick takibi (Dictionary<Collider2D,float>) veya merkezi `OverlapCircleAll` + tek sayaçlı Update döngüsü. Demo'da tek hedef varsa görünmez; demo-kritik değil.

### ⚪ LOW — "%100 hasar azaltma" hâlâ 1 hasara floor'lar (Evasion/SetImmune dışında)
`Health.cs:54-56` — `incomingDamageMultiplier=0` olsa bile `Mathf.Max(1, ...)` → 1 hasar geçer. Kod yorumu (TODO E1) bunu zaten biliyor. Evasion `incomingDamageMultiplier=0` (Evasion.cs:41) ile "tam dodge" sağlamaya çalışıyor ama bu floor yüzünden her vuruşta 1 hasar sızar.
Not: Tam bağışıklık ayrı `immune` guard'ı ile (`:51`) doğru çalışıyor; Evasion onu kullanmıyor.
Minimal fix: `incomingDamageMultiplier <= 0f` ise erken `return` (floor'dan önce). Tek satır, düşük risk. Demo'da Evasion gösterilecekse değerli.

### ⚪ LOW — OnDamageTaken çarpan-ÖNCESİ ham değer publish ediyor
`Health.cs:21,53` — `OnDamageTaken?.Invoke(amount)` `incomingDamageMultiplier` uygulanMADAN önceki ham `amount`'u yayınlıyor (yorum bunu kabul ediyor). Bu event'i dinleyen biri (örn. rage-on-hurt) gerçek alınan hasardan farklı sayı görür. Şu an dinleyici az; düşük etki.
Minimal fix: gerekiyorsa `effective`'i de taşıyan ikinci parametre; aksi halde davranışı dokümante et.

### ⚪ LOW — DeepWound ilk vuruş popup, bleed tick'leri popup'sız (kozmetik tutarsızlık)
`DeepWound.cs:37` Tip-4 (popup=true) → ilk vuruş sayı gösterir; `:51` Tip-1 (popup=false) → bleed tick'leri sessiz. Diğer DoT'lar (StatusEffectSystem) da sessiz, yani bleed'in sessiz olması tutarlı; asıl tek-seferlik `:37`'nin popup'lı olması istisna. Görsel tutarlılık için kontrol edilmeli.
Minimal fix: `:37`'yi `DealDamage(target, hitDamage, this)` (Tip-1, popup'suz) yap veya bilerek popup isteniyorsa bırak.

---

## Sayısal tuzak taraması (özet)
- **0'a bölme:** DamageCalculator defense `resistance/(resistance+100)` — payda hep ≥100, güvenli. ✅
- **Negatif hasar:** `baseDamage<=0 → 0` (DamageCalculator.cs:58); Health `amount<=0 → return` (:52). ✅
- **Negatif heal:** `effective<=0 → return` (Health.cs:76). ✅
- **IsDead guard:** Tip-3 girişinde var (SkillRuntime.cs:132); her skill kendi döngüsünde `hp.IsDead` kontrol ediyor; **enemy attack çıplak yolları guard'sız** (`EnemyAI.cs:93` player Health'i IsDead kontrol etmeden vuruyor — ama Health.TakeDamage kendi içinde IsDead guard'lı, çift ölüm yok). ✅ kabul edilebilir.
- **int/float floor:** DoT remainder sistemi (`StatusEffectSystem.cs:199-209`) küsuratı biriktiriyor — düzgün. ✅
- **Ölü hedefe hasar:** merkezi yol guard'lı; risk yok.

---

## ÖZET TABLO

| # | Önem | Yer | Konu |
|---|---|---|---|
| 1 | 🟠 HIGH | SkillRuntime.cs:104,107-120 | 52/54 yetenek stat-scaling BYPASS; yalnız LMB/projectile scaled |
| 2 | 🟠 HIGH | MarkPulseBehavior.cs:127 | Ravager LMB merkezi yolu atlıyor → DPS telemetri + VFXRouter + calc yok |
| 3 | 🟡 MED | DamageCalculator.cs:45 / TakeDamage yolları | debugGlobalDamageMult bypass yolunda var, çıplak TakeDamage'da yok |
| 4 | 🟡 MED | SkillRuntime.cs Tip-1/4 | Weakened/Scorch çarpanı yeteneklerde uygulanmıyor |
| 5 | 🟡 MED | DamageZone.cs:49-53 | Tek sayaç çok-hedefte tick aralığını bozuyor |
| 6 | ⚪ LOW | Health.cs:54-56 | %100 DR → hâlâ 1 hasar floor (Evasion sızıntısı) |
| 7 | ⚪ LOW | Health.cs:53 | OnDamageTaken çarpan-öncesi ham değer yayınlıyor |
| 8 | ⚪ LOW | DeepWound.cs:37 | İlk vuruş popup'lı, bleed sessiz (kozmetik) |

🔴 DEMO-KRİTİK bulgu YOK — hiçbir yol crash/0-bölme/negatif-loop üretmiyor. En yüksek demo-riski #2 (Ravager LMB DirectorMode DPS penceresinde görünmez) ve #1 (stat slider yeteneklere etkisiz görünür).
