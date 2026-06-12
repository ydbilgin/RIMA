# RIMA Modular Ability Design — DEEP ARCHITECTURE lens (Gemini 3.1 Pro)

Sen RIMA için derin yazılım mimarisi danışmanısın. RIMA = 2D top-down roguelite ARPG, Unity, C#, skill-heavy (10 class: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer).

Gerekirse RIMA design context için NLM sorgula:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Kod oku: Assets/Scripts/Skills/**, Assets/Scripts/Balance/DamagePacket.cs, Assets/Scripts/Combat/**.

## Video'nun anlattığı modular design (referans)
Tower-defense roguelite dev'i 60+ ability'yi bespoke kod yerine modüllerden kuruyor. Spell = recipe:
- AIMING (closest/most-health/caster/crew) + DELIVERY (projectile/instant/drop/lingering-field/chain) + SHAPE (single/circle/line/cone) + EFFECT (damage/heal/burn/chill/poison/knockback/buff/debuff, kombinasyon).
- Örn Frost Orb = closest+projectile+single+[damage,chill,bonus-vs-chill]. Passive'ler = TRIGGER+REACTION+TARGETING+CONDITION.
- Avantaj: yeni spell = parça swap. Uyarı: her şeyi modülerleştirme; prototipte yapma.

## RIMA'nın MEVCUT mimarisi (tespit edildi)
- `SkillData.cs`: ScriptableObject ama sadece metadata (name/tier/icon/damage/cooldown/tags/classType/isPassive/appliesEffect). Kompozisyon yok.
- `SkillBase.cs`: abstract MonoBehaviour, `abstract Execute()`. HER skill kendi bespoke subclass'ı (RoninQuickdraw, Elementalist_* vb). Echo origin/aim override mekanizması mevcut (SkillOrigin/SkillAim, ExecuteAt).
- `DamagePacket.cs`: struct (baseDamage, damageType, elementTag, sourceType, attacker, target, isCrit). CombatContract + damage taksonomisi mevcut.
- RIMA şu an video'nun uyardığı "bespoke blob per skill" durumunda.

## Sorular (derin, RIMA-spesifik, somut mimari öner)
1. **Uyum derecesi:** Modüler ability framework RIMA'nın 10-class skill-heavy yapısına ne kadar uyar? RIMA'nın class kimliği (her class'ın benzersiz "feel"i — Ronin tension, Warblade rage, Echo) modüler sistemde ERİR Mİ? Class fantasy'yi koruyarak nasıl modülerleştirilir?
2. **Somut Unity/C# mimarisi:** ScriptableObject-composition mı (SkillRecipeSO -> AimModuleSO + DeliveryModuleSO + ShapeModuleSO + EffectModuleSO[] referansları) yoksa runtime component-composition mı, yoksa hibrit (SO data + runtime executor)? Her yaklaşımın trade-off'u. Mevcut SkillBase.Execute() + Echo override mekanizmasıyla nasıl köprülenir? Migration path (bespoke -> recipe) nasıl kademeli olur?
3. **CombatContract entegrasyonu:** EFFECT modülleri DamagePacket üretip CombatContract gate'inden geçecek. Modüler EFFECT zinciri (örn [damage, chill, bonus-vs-chill]) ile mevcut packet sistemi (finisher, zero-damage, bypass kuralları) arasında mimari çakışma/risk nerede? Effect ordering, stat-scaling, crit nasıl ele alınmalı?
4. **Sınır:** RIMA'da hangi alanlar modülerleştirilMEli (overkill), hangileri kesinlikle modülerleştirilmeli? Boss mekanikleri, class-signature ultimate'lar, cross-class Echo gibi unique şeyler modüler mi kalmalı bespoke mı?

Her cevap somut C# tip/SO isimleri içersin (örnek API imzaları). Mevcut RIMA isimlerine (SkillBase, DamagePacket, SkillData) saygı göster. Türkçe karakter kullanabilirsin.
