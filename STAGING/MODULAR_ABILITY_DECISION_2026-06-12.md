# MODULAR ABILITY DESIGN — DECISION (2026-06-12)

**Kaynak:** YouTube — Feed the Overlord, "modular design saved my entire project" (video id `9CQgPaHAV1E`).
**Council:** cx (feasibility/reuse) + ax Gemini 3.1 Pro (mimari) + ax Gemini 3.5 Flash (lean). Sentez: Opus.
**Ham çıktılar:** `STAGING/_process/2026-06/_council_*_modular-ability.md` + `CODEX_DONE_yekta.md`.

---

## Video ne diyor (özet)
Bir spell tek bir bespoke kod bloğu değil, küçük yeniden-kullanılabilir modüllerin **recipe**'sidir:
- **AIMING** (closest / most-health / caster / crew) + **DELIVERY** (projectile / instant / drop / lingering-field / chain) + **SHAPE** (single / circle / line / cone) + **EFFECT** (damage / chill / poison / knockback / buff…).
- Frost Orb = closest + projectile + single + [damage, chill, bonus-vs-chill]. Yeni spell = sıfırdan değil, **parça swap**.
- Passive = TRIGGER + REACTION + TARGETING + opsiyonel CONDITION.
- **Kendi uyarısı:** her şeyi modülerleştirme; "does this system need enough variation to make modularity worth it?"; **prototipte yapma**, önce rough sistemle oynanabilir hale getir.

## RIMA'nın gerçek durumu (cx kod taraması)
- **75 bespoke active skill subclass** (Warblade 14, Elementalist 15, Ranger 20, Shadowblade 22, Ronin 4) + **14 passive class**. Hepsi `SkillBase.Execute()` override'lı → tam video'nun uyardığı "bespoke blob per skill".
- **Reusable runtime'ın yarısı ZATEN var:** `SkillRuntime` (FindNearestEnemy, EnemiesInCircle/InLine/InCone, DealDamage→DamagePacket), `PlayerProjectile`, `DamageZone` (ticking field), `StatusEffectSystem.ApplyEffect`, `SkillStateTracker`, ayrıca yeni `ProjectileFanSpawner` + `PlacedEffectSpawner`.
- **Eksik olan:** recipe asset şeması + generic executor. Yeni combat runtime DEĞİL.
- **DRY ihlalleri somut:** projectile spawn dizisi ~6 skill'de kopyalanmış; circle-AOE pattern ~5 skill'de; **45 adet `ApplyEffect(` çağrısı**; targeting helper'ları VAR ama Backstab/Hemorrhage/ChainLightning/LivingBomb/ShadowStep hâlâ kendi lokal kopyalarını kullanıyor.

## ⚠️ cx düzeltmesi (önemli)
`CombatContract` bir **runtime damage gate değil** — `Assets/Tests/Contracts/CombatContract.cs`'teki basic-attack davranış test sözleşmesi. Modüler EFFECT'lerin gerçek risk yüzeyi: `DamagePacket` / `DamageCalculator` / `SkillRuntime` / `PlayerProjectile` / `Health`. (CURRENT_STATUS'taki "CombatContract gate" ifadesi runtime gate sanısı yaratıyor — değil.)

---

## KARAR

### ❌ ŞİMDİ YAPMA: Tam SO-composition refactor
Üç advisor da hemfikir. 75 skill'i Aim/Delivery/Shape/Effect SO'larına bölmek → 240+ ScriptableObject asset + Inspector-debug cehennemi + çalışan demo skill'lerinin regression riski. Flash net: demo aşamasında **stratejik hata**, demo değeri sıfır. Prototip/demo aşamasında modülerleştirme video'nun kendi uyarısına aykırı.

### ✅ ŞİMDİ YAP (demo içi, ucuz, yüksek-getirili — C# helper konsolidasyonu)
Mevcut `SkillBase` mimarisini KIRMADAN, en çok tekrar eden 3 pattern'i tek noktaya çek:
1. **Status apply standardizasyonu** — 45 dağınık `ApplyEffect(` çağrısını tek imzaya topla: `SkillRuntime.ApplyStatus(target, type, duration, magnitude, packetCtx)`. DOT'ların packet bypass'ı (StatusEffectSystem:199 `Health.TakeDamage` direkt) policy olarak netleşsin.
2. **Lokal targeting kopyalarını sil** — Backstab/Hemorrhage/ChainLightning/LivingBomb/ShadowStep mevcut `SkillRuntime` helper'larına yönlendir (saf DRY, davranış değişmez).
3. **`Passive_StatThreshold` generic passive** — "HP < %X → stat +Y" kalıbındaki basit pasifleri tek inspector-configurable script'e indir (Flash: ~%60 pasif script tasarrufu). `WarbladePassives` Wrath Protocol pilot.

Bu adım additive, regression-güvenli, yeni asset şeması gerektirmez. **Tek cx dispatch + edit-mode test gate** ile yapılabilir.

### 🔜 POST-DEMO (somut duplicate skill ailesi "hak edince"): opt-in SkillRecipe katmanı
cx'in spec'i + 3.1 Pro'nun köprü mimarisi:
- `SkillRecipe : ScriptableObject` → AIMING / DELIVERY / SHAPE / EFFECT[] referansları.
- `RecipeSkill : SkillBase` → mevcut helper'ları (SkillRuntime, PlayerProjectile, DamageZone, StatusEffectSystem, SkillStateTracker) **çağırır, replace etmez**. Echo origin/aim override'ı korunur (3.1 Pro: `SkillExecutionContext` Origin/Aim'i `SkillBase.SkillOrigin/SkillAim`'den besler).
- **EFFECT pipeline kuralları (cx — ZORUNLU):** (a) zero-damage effect (heal/mark/teleport/buff) damage path'e GİRMEZ, `baseDamage=0` packet emit ETME (Health/DamageCalculator min-1'e clamp'liyor); (b) `bypassStatScaling` default'u mevcut `SkillRuntime.DealDamage(int)` davranışıyla (=true) eşleşmeli yoksa sessiz rebalance; (c) projectile taxonomy/crit/element istiyorsa packet SET et; (d) **crit ≠ finisher** ayrımı (juice sistemleri `HitEvent.isCrit`'i finisher sanıyor); (e) effect ordering explicit (`[Weakened, damage]` ≠ `[damage, Weakened]`); (f) DOT packet policy.
- **Pilot:** sadece projectile-shot + basit AOE-circle ailesi. Migrate = yeni skill mevcut recipe'ye temiz oturduğunda; eski bespoke'ları toplu migrate ETME.

### 🚫 ASLA modülerleştirme (üç advisor hemfikir — bespoke kalır)
- **Class-signature ultimate'lar** (frame-frame kamera, özel anim event'leri).
- **Boss mekanikleri** (state machine / timeline kalsın).
- **Cross-class Echo** — bu bir EFFECT değil, `SkillBase` üstü context-override sistemi; modüller onun verdiği Origin'i tüketir, kendisi modül olmaz.
- Class kaynak kimliği (Warblade rage, Ronin tension) — generic DamageEffect'e eritme; gerekirse class-spesifik EFFECT modülü (`WarbladeRageScalingEffect`) yaz, jenerikleştirme.

---

## Advisor anlaşmazlığı + Opus kararı
**Tek gerçek gerilim:** SO-composition mı (3.1 Pro en istekli) yoksa salt C# helper mı (Flash en şüpheci)? cx ortada (opt-in SO ama full migration pahalı).
**Kararım:** zamanlama Flash'tan (post-demo), mimari cx'ten (opt-in SkillRecipe), teknik köprü detayları 3.1 Pro'dan (context bridge + payload pipeline). Demo için SO YOK, sadece helper konsolidasyonu (Şimdi-Yap adımı). SO recipe katmanı post-demo'da, somut duplicate aile hak edince, cx'in EFFECT kurallarıyla.

## Sonraki aksiyon
"Şimdi Yap" 3 maddesi → tek `cx dispatch` task'ı (status-apply konsolidasyon + lokal-targeting temizlik + `Passive_StatThreshold`), edit-mode test gate. İstersen bu task'ı hazırlayayım. Post-demo SkillRecipe spec'i bu dosyada hazır bekliyor.

---

## ⚠️ AMPİRİK GÜNCELLEME (2026-06-12 — Step 1 cx ile test edildi → premise ÇÖKTÜ)
"Şimdi Yap" madde 1+2 (`TASK_modular_dry_cleanup_step1.md`) cx'e "birebir-eşdeğer değilse dokunma" altın kuralıyla dispatch edildi. **Sonuç: cx 10 call-site'ın 10'unu da SKIP etti** — gerçek eşdeğer duplikasyon YOK:
- **5 AOE skill** (WarStomp/GravityCleave/FanOfKnives/Blizzard/Meteor): her biri skill-özel mantık taşıyor — radial knockback, pull force, koşullu Stunned-vs-Chill, reference-target debuff okuma, dead-Health'siz collider'a chill. Ortak helper'a sıkıştırmak ya davranış kaybı ya flag-şişmesi olurdu.
- **5 targeting kopyası** (Backstab/Hemorrhage/ChainLightning/LivingBomb/ShadowStep): `SkillRuntime` helper'larından farklı — dead-target filtresi, per-jump exclude-set, max-range farkı, collider-vs-EnemyAI arama. Eşdeğer değil.
- cx sadece kullanılmayan 11-satır `DamageCircle` helper'ı ekledi (0 redirect) → ölü kod, **revert edildi**. 0 commit.

**Çıkarım (önemli):** RIMA'nın skill "duplikasyonu" yüzeyseldi; call-site seviyesinde her skill GERÇEK varyasyon taşıyor. Bu, council'ın "over-modülerleştirme" uyarısını bu küçük ölçekte bile doğruladı. **Bespoke-per-skill pattern aslında gerçek per-skill farkları taşıyor — "temizlenecek kopya kod" sanılan şey yoktu.** Madde 1+2 TERK EDİLDİ.

**Madde 3 (Passive_StatThreshold) — SURVEY YAPILDI (`TASK_passive_threshold_survey.md`, analiz-only):** Tüm 15 kayıtlı pasif tarandı (14 `PassiveBase` + 1 yanlış-kayıt `IroncladMomentum`). **Sadece 1 temiz FIT** (WrathProtocol), tek sınıfta. Geri kalan 14 = event-driven / floor-clamp / oneshot-buff / proc — eşik→stat-histeresis DEĞİL. Eşik 3-4 sınıftı → **1 FIT = OVERKILL**. cx kararı: **YAZMA**. WrathProtocol bespoke kalır. Madde 3 TERK EDİLDİ.

**Ortam notu:** cx batchmode EditMode testleri **Unity editör açıkken çalışmıyor** ("another Unity instance"). Test-gate gereken işlerde ya Unity kapat ya da Unity MCP `run_tests` (editör-içi) kullan.

---

## 🏁 NİHAİ HÜKÜM (2026-06-12)
**"Şimdi Yap"ın 3 maddesi de ampirik olarak ÇÖKTÜ — gerçek hedef yoktu:**
- Madde 1 (AOE helper): 5/5 skill skill-özel mantık → eşdeğer duplikasyon yok.
- Madde 2 (targeting redirect): 5/5 kopya helper'dan farklı → eşdeğer değil.
- Madde 3 (Passive_StatThreshold): 1/15 fit → overkill.

**Bu bir başarısızlık değil, bir DOĞRULAMA.** RIMA'nın skill/passive kodu yüzeyde "kopya" gibi görünüyordu ama call-site seviyesinde her biri gerçek varyasyon taşıyor. Video + council'ın "her şeyi modülerleştirme, varyasyonu hak ediyor mu" uyarısı bu küçük ölçekte birebir kanıtlandı. **Demo öncesi modüler temizlik İŞİ YOK — mevcut bespoke yapı haklı.**

**GEÇERLİ KALAN:** post-demo opt-in `SkillRecipe` SO spec'i (yukarıda) — somut bir duplicate skill ailesi "hak edince" devreye girer. O güne kadar dosya arşiv-referans. Net kazanç: stüdyo-disiplini (`project_modular_design_philosophy` memory) + bu ampirik veri.
