# DECISION — Code-Only Animation Stratejisi + Knockdown (2026-06-05)

**Council:** ax-3.1-Pro (derin teknik) ‖ ax-3.5-Flash (lean) ‖ Opus-advisor (kod-audit, `_council_opus_codeanim.md`) → Opus sentez. (cx chamber'da meşguldü; kod envanterini Opus-advisor file:line doğruladı.)

## 🔑 KOD GERÇEĞİ (Opus-advisor audit — brief'i düzeltti)
Altyapı ~%70 KURULU: `GroundBlobShadow.cs` ayrı gölge HER mob+player'da (knockdown'ın 1 numaralı şartı bedava) ·
`BasicAttackProfile.cs:40` per-combo-step `knockbackForce[]/knockbackDuration[]` (tunable SO ZATEN VAR) ·
`SkillStateTracker` Broken/Sundered `OnStateEntered` event'leri (poise-trigger hazır) · `HitStop` singleton +
8 juice driver + `FeelToggleSettings`. ⚠️ İKİ paralel knockback impl: `KnockbackReceiver` (ana) +
`KnockbackComponent` (boss, hard-coded 6-12f) → BİRLEŞTİRİLECEK.

## KARARLAR

### K-ANA: Karakter/mob için YENİ ANİMASYON ÜRETİMİ = DEMO'DA SIFIR ✅ (3/3)
Üretilmiş idle+walk (var olan) yeter. Gerisi kod+VFX. Eski canon anim-planının Tier-1'i (Hit/Death/Basic-Attack
üretilmiş frame) DEMO İÇİN AMEND: kod karşılıyor. (Basic attack zaten kilitli mimaride kod: weapon-hand+slash-arc.)

### KNOCKDOWN: KOD-ONLY YAPILIR — görsel reçete (3 görüşün sentezi):
- ❌ 90° tam yatırma (3.1) → "ayakta uzanmış adam" okunur (3/4 perspektif). ❌ Sadece yassı-squash (Flash) →
  karikatür pankek, dark-fantasy tona aykırı.
- ✅ **Opus-advisor reçetesi:** parabol arc (gövde child'ı havalanır, GÖLGE YERDE kalır — GroundBlobShadow hazır)
  + **~35° eğme + Y-squash 0.6** + 1-2 sönen sekme + toz puff + yerde `stunTime` + get-up (geri eğim + mini hop).
  Sersemlik göstergesi: çizgi-film yıldızı YOK — hafif cyan baş-üstü mote (ton uyumu) ya da hiç (yerde olmak yeter).
- **Get-up I-FRAME ZORUNLU** (juggle-lock = #1 risk; player'a da mob'a da).
- Rotasyon pixel-grid notu: eğme visual-child'da serbest (PPC altında kabul edilebilir); rahatsız ederse
  15° step fallback.

### TIER TABLOSU
- **T1 KOD-ONLY (üretme):** knockback ✓(var) · knockdown (yeni KnockdownDriver) · stagger (X-sine+tint) ·
  mob-ölüm (squash→koyu decal→fade; 0 asset) · spawn (drop+easeOutBack+toz) · dash (ghost-trail) ·
  melee (weapon-hand+arc — kilitli) · lunge (arc+apex-scale+sabit gölge).
- **T2 BİR-STILL DEĞER (post-demo, GATED):** cast-pose 1-frame · elite-intro 1-frame.
- **T3 ÜRET (boss'lar, kullanıcıyla):** boss telegraph/stagger/death — büyük odak sprite'ında kod-shake glitch okunur.

### MEKANİK: Poise-metre YOK (demo) — RIMA-özgü tetik:
Ağır skill'ler + **hedef Broken/Sundered state'indeyken** gelen ağır vuruş = knockdown (SkillStateTracker
event'ine abone — Sundered Beat kimliğiyle birebir sinerji). Hafif vuruş = normal knockback. Bayrak:
mob tanımında `isKnockdownable`.

### MİMARİ (ayarlanabilirlik — kullanıcı şartı):
Mega-SO YOK. (1) İki knockback impl BİRLEŞİR (boss → KnockbackReceiver). (2) `HitImpulse` struct
(force/duration/knockdown?) BasicAttackProfile pattern'ine eklenir. (3) **2-3 arketip `KnockdownProfile` SO**
(Light/Heavy/Boss: arcHeight, tiltAngle, bounceCount, stunTime, getupIFrame) — skill/mob bunlara referans verir.
(4) FeelToggleSettings'e knockdown toggle.

## Uygulama (kuyruğa — sıralamayı ROADMAP council'i belirleyecek)
[M] cx: knockback birleştirme + KnockdownDriver + KnockdownProfile×3 + Broken-tetik + get-up i-frame ·
[S] mob-ölüm squash-to-decal · [S] spawn/dash ghost (ayrı küçük işler, Flash-able olanlar işaretlenecek).

## Disagreement kaydı
3.1 "1-frame corpse üret" → Flash+Opus-adv "demo'da 0 asset" kazandı (T2/T3'e ertelendi). Flash "SO kurma" →
Opus-adv kanıtıyla reddedildi (SO pattern'i ZATEN var; 2-3 arketip SO = tweak-hell'i önler, Flash'ın kendi
"prefab tweak hell" riskine cevap).
