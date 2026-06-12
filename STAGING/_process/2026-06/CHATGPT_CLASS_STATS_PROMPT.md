# ChatGPT'ye — RIMA Class Stat Modeli + Tool Brainstorm

> Sen RIMA reposuna GitHub'dan erişebiliyorsun. Aşağıdaki dosyaları oku, mevcut stat sistemini analiz et, sonra (A) birleşik class stat modeli öner, (B) 10 class için sayısal değer dağıt, (C) ek demo/dengeleme tool fikirleri üret.

## Repo: ydbilgin/RIMA (branch: master)

### Oku (stat-ilgili dosyalar):
```
Assets/Scripts/Skills/SkillData.cs                         (ClassType enum — 10 class)
Assets/Scripts/Core/Health.cs                              (maxHP, incomingDamageMultiplier)
Assets/Scripts/Player/PlayerController.cs                  (moveSpeed=4.5, dash)
Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs    (combo damage, attack speed/commitment)
Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Warblade.asset
Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Elementalist.asset
Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Ranger.asset
Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Shadowblade.asset
Assets/Resources/Combat/BasicAttack/BasicAttackProfile_Ronin.asset
Assets/Scripts/Skills/SkillRuntime.cs                      (damage formülü)
Assets/Scripts/Skills/SkillBase.cs                         (cooldown, resource cost)
Assets/Scripts/Systems/Resources/                          (Rage/Mana/Energy/Focus/Tension)
Assets/Scripts/Systems/PlayerClassManager.cs               (class init)
Assets/Scripts/Player/                                     (PlayerHealth/PlayerAttack)
```

## Mevcut durum (bizim analiz — doğrula/düzelt)
- **10 class**, 5 implemented (Warblade, Elementalist, Shadowblade, Ranger, Ronin), 5 değil (Ravager, Gunslinger, Brawler, Summoner, Hexer)
- **Per-class stat scaling YOK:** HP=100, moveSpeed=4.5 TÜM class'larda sabit
- **Tek farklılaştırıcı = BasicAttackProfile** (combo damage + attack speed/commitment frame). Skill damage'ları hardcoded int.
- **Damage formülü flat:** `base × statusMult × incomingDamageMult` (min 1). Attack/MagicAttack stat'ı YOK, crit YOK, armor YOK.
- Combo dmg: Warblade [25,30,40]=95 · Elementalist [18,18,28]=64 · Shadowblade [20] · Ranger [18]
- Resource sistemleri tamamen farklı: Rage (hit-gain), Mana (+8/s), Energy (+15/s), Focus (mesafe-bazlı), Tension (stance)

## İstediğimiz çıktı

### A) Birleşik Class Stat Modeli
Hangi stat'lar olmalı? Öner: `attackPower`, `magicPower`, `attackSpeed`, `castSpeed`, `moveSpeed`, `maxHP`, ve gerekiyorsa `critChance`, `armor`. Roguelite normlarına göre hangi minimal set yeterli? Damage scaling formülü öner (örn. `skillDamage = base × (attackPower/100)`).

### B) 10 Class için Sayısal Dağılım
Her class'ı bir arketipe oturt (tank / bruiser / glass-cannon / assassin / ranged / mage / summoner / hybrid) ve TABLO ver:
| Class | Arketip | maxHP | attackPower | magicPower | attackSpeed | moveSpeed | resource |
Değerleri **mevcut combo damage'larla tutarlı** ve birbirine göre **dengeli** ver (örn. Warblade tanky-yavaş-güçlü, Shadowblade glass-cannon-hızlı). Baz=100 referansıyla oransal. Gerekçeyle.

### C) Ek Demo/Dengeleme Tool Fikirleri
Bizde zaten planlı: god-mode, kill-all, reset, spawn, stat slider'ları (speed/damage/HP), debug HUD, presenter mode, slow-mo, free-cam. Bunlardan BAŞKA, dengeleme demosu için ne eklenebilir? (örn. DPS meter, damage-number toggle, hitbox görselleştirme, class A/B karşılaştırma, encounter difficulty slider, stat preset kaydet/yükle). Her biri: ne + Unity'de nasıl + neden dengelemeye yarar.

## Demo'nun amacı
"Dengeleme altyapı sistemi kuruldu, animasyonlar sonradan gelecek" — yani stat/balance sistemini canlı ayarlanabilir göstermek. Tool'lar buna hizmet etmeli.

Türkçe, net, tablo formatında.
