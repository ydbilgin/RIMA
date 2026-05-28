# Sınıf Balance + Skill Öneri Analizi — S95 (2026-05-20)

> Opus tasarım analizi. 10 sınıf × NLM canon × kod gerçeği cross-ref. Faz 1-3 öncelik filtresi. Onay/red bekliyor.

---

## Kritik Durum Tespiti (Kod tarafı)

**Sadece Ronin için sistem skeleton kurulmuş.** `TensionSystem` + 4 skill çalışıyor. Warblade (Faz 1 zorunlu sınıfı) **henüz kodlanmamış** — sadece design doc'ta var.

`CrossClassSkillManager` çalışıyor ama eski "passive trigger" modelini (`CrossClassEffectType` enum) implementliyor. **Karar #122 (Echo Resonance T1-T4 tier) ile kod uyumsuz.** Tek hardcoded combo: Warblade-Beat3 → Ronin Quickdraw.

T1 (her cross-class kombosu için Commit-Beat) gerektiren genel mimari yok. **Faz 2 başlamadan refactor şart.**

---

## WARBLADE — "Yaklaş. Sabitle. Zırh kır. İnfaz et."

**Kaynak: DENGELI** | **Cross-class: YÜKSEK**

Rage ekonomisi sağlam (+10/vuruş, CC'liye +20, idle -5/sn). Her ~5-6 vuruşta 1 RMB/F kullanımı — Faz 1 baseline doğru pacing.

**Balance riski:** F (Bladestorm) "5sn CC immune + her 0.5s AoE %120" Rage gate olmadan çok güçlü. Cross-class Brawler Charge stacking ile fırtınada Rage koruma loop'u çıkabilir.

- **Balance önerisi:** Bladestorm aktivasyonda %50 Rage harca, sonra her 1sn'de %10 drain. Rage bitince erken iptal.
- **Eksik skill:** Sundered state'ten yararlanan **proactive** bir skill yok. Şu an Sundered = sadece pasif modifier. Öneri: **"Verdict Cleave"** — Sundered düşmana yapılan LMB Beat 3 yarıçap 2u ek AoE patlatır (Family Tag Fracture spread için).
- **Yeni pasif:** **"Iron Reservoir"** — Rage 100'e ulaştığında 3sn boyunca skill CD'leri %30 hızlı düşer. Cap'e ulaşma incentive (şu an oyuncu 80'de F'ye basıyor, 100 hedeflemiyor).
- **Risk:** Bladestorm Rage gate Faz 1 prototip için fazla erken karmaşıklık. Faz 2 tier upgrade'ine sakla.

---

## ELEMENTALIST — "Yakıyorum, ama önce ritmi buluyorum."

**Kaynak: ZAYIF** | **Cross-class: ÇOK YÜKSEK** (Echo family pivot)

Mana + Elemental State dual sistem **çok karmaşık**. "Convergence" (ateş+buz geçiş) belirsiz — tetikleme net değil, draft'ta mı base'de mi belirsiz. **Faz 2'de implement edilemez, tasarım netleştirilmeli.**

- **Balance önerisi:** **Convergence'i OFFER POOL'a taşı**, base'de tutma. Common: tek element. Rare: element swap unlock. Epic: Convergence reaction. Onboarding'i kurtarır.
- **Eksik skill:** Defansif outlet yok. **"Frost Bulwark"** — 30 mana, 1.5s frost zırh (1 hit absorb + AoE freeze). Faz 2'de Q slotu olabilir.
- **Yeni pasif:** **"Brand Echo"** — Echo Family tag'i taşıyan düşmana 3. aynı element vuruşunda otomatik 30 mana iadesi. Trinity Storm (F) için ramp sürdürür.
- **Risk:** Convergence'i tier upgrade'e taşımak Karar #5 ile uyumlu. Açık.

---

## SHADOWBLADE — "Görmüyorsun. Zaten geç."

**Kaynak: GÜÇLÜ** | **Cross-class: YÜKSEK** (Veil + Echo + Bleed üçlüsünde köşe taşı)

Sever (0-100) kaynağı pozisyonel oynanışla doluyor — **mekanik olarak en zarif sistem.** Rift Scar Family Tag kimlik tag'i ile uyumlu.

- **Balance önerisi:** Severance execute "Scar Collapse afterward (3+ Scars)" gate doğru ama bossta execute yok kuralı (Universal Boss Rule) ile **bossta Severance ne yapar?** Belirsiz. **Çözüm:** Bossta Severance = "Scar 3+'ta %200 hasar + Scar reset" (kill yerine damage burst).
- **Eksik skill:** Wraith Form (F) **karakter kimliğinden uzak** — Shadowblade fantezisi "yara bırakan", "fazlanan" değil. Daha kimlik dolu: **"Scar Detonation"** — F: tüm Scar'lı düşmanları aynı anda 0.3s phase teleport sonrası tek koordineli vuruş. "10 hayalet aynı anda hareket" görselliği.
- **Yeni pasif:** **"Veil Memory"** — Düşmandan geçtikten sonra pozisyonda 1sn Veil tag'i kalır, oradan saldıran herhangi bir aksiyon crit. Pozisyonel oynanışı reward eder.
- **Risk:** Scar Detonation Karar #5 movement cap'ine girmez (area apply, jab değil). Açık.

---

## RANGER — "Sana ulaşamazsın."

**Kaynak: GÜÇLÜ** | **Cross-class: ORTA** (Pierce paylaşımı Gunslinger'la fazla benzer)

Focus mekaniği mesafe disiplini için en temiz örnek. 4m+ ile 2m yakında erime — eğitici.

- **Balance önerisi:** Faz 1'de 2m yakında Focus erime **çok cezalandırıcı**. Faz 2'de erime hızını yarıya indir, Focus yeniden dolması için 6m+ gerekli.
- **Eksik skill:** Tuzak (trap) family Ranger kimliği ama RMB/F'de tuzak yok. **"Tripwire Snare"** — yeni Q slotu: zemine 2u radius tuzak, 5sn aktif, ilk düşman Marked + Trapped (Final Strike execute gate'i için ön koşul). Bu olmadan Final Strike execute imkansız.
- **Yeni pasif:** **"Hunter's Patience"** — Focus 100'de iken 4 saniye boyunca her ok %30 ek hasar + Pierce 2 düşman. Cap'e ulaşma ödülü.
- **Risk:** Tripwire Snare placement skill, movement değil. Karar #5 cap'ine girmez. Açık.

---

## RAVAGER — "Az canken daha tehlikeliyim."

**Kaynak: ÇOK ZAYIF (risk)** | **Cross-class: ORTA** (Bleed paylaşımı dağınık)

Fury sadece **hasar ALARAK** doluyor — solo boss savaşlarında Penitent Sovereign vurmuyorsa Fury sıfır kalıyor. **Kaynak ekonomisi kırılgan.**

- **Balance önerisi:** **Hook Pull başarılı bağlantısında +15 Fury** ekle. Hook miss = 0. Risk/reward dengeli. Kendi can kaybetmeden kaynak üretme yolu.
- **Eksik skill:** Berserk Mode (F) "kan halkası" görsel olarak güçlü ama mekanik olarak Bladestorm Bleed varyantı. Daha kimlik dolu: **"Death Embrace"** — F: HP %30 altındayken aktive edilebilir, 4sn tüm hasarı %50 azalt + LMB'ler hedeften HP çal (%25 leech). Risk öncesi ödül.
- **Yeni pasif:** **"Bloodbound Edge"** — Bleed'li düşmana crit %150 hasar (normal crit 200 değil). Pierce/Veil crit'lerinden ayrı kimlik.
- **Risk:** Hook +15 Fury → 4sn CD koy yoksa spam. Death Embrace executable HP gate'i kendi HP'si, Universal Boss Rule "No HP<30% execute" ile çakışmaz.

---

## RONIN — "Tek kesik, sonsuz sessizlik."

**Kaynak: KIRILGAN** | **Cross-class: YÜKSEK** (Cut + Pressure + Bleed üçlüsü)

**Kod tarafı incelendi:** `TensionSystem` idle +1/sn, moving -2/sn, iaido +5/sn. NLM tasarımıyla uyumlu. **Ama Faz 1-2 Warblade odaklı combat'ta yumurtaya benziyor** — secondary olarak gelirse Warblade kombolarken Tension drain edebilir.

- **Balance önerisi:** Iaido Stance'tayken `iaidoGainPerSecond` 5 değil **3**, ama Stance içinde sabit dururken **deflect penceresi** açılsın (class-specific parry). Tension gain parry başarısında +25. Karakter kimliği güçlenir.
- **Eksik skill:** Mugen No Kiri (F) 5sn cooldownsız iaido = **yumuşak break edici**, Tension drain edilmiyor. **"Single Cut"** Flash Draw bossta execute olarak ne yapar belirsiz. Bossta Flash Draw = Tension MAX'tan tek vuruşta %300 hasar + Tension reset. Lock'la.
- **Yeni pasif:** **"Sheath Discipline"** — Sheath Walk (LMB) ile yapılan 3 ardışık vuruşta hareket yokken **+50% Tension gain** (`iaidoGainPerSecond` 7.5'e çıkar dinamik). Kod tarafı kolay.
- **Risk:** Tension cross-class oyuncusu Warblade primary olarak hareket ederse Tension drain → Ronin secondary value veremez. **Çözüm:** Karar #122 `TriggerWarbladeBeat3RoninQuickdraw` zaten 8u radius'ta free trigger çağırıyor — bu **doğru tasarım**, sınıf primary değil iken Quickdraw'a Tension gate koyma.

---

## GUNSLINGER — "Koş, ateş et, bitir."

**Kaynak: DENGELI** | **Cross-class: ORTA** (Pierce Ranger'la, Heat tek başına)

Heat economy klasik FF/MMO pattern — overheat öncesi vent. Sağlam.

- **Balance önerisi:** Overheat sonrası "zorunlu cooldown" cezası spec yok. **Önerin:** Overheat 2sn tüm skill disabled, ama bu sürede LMB hasarı %150. Risk → ödül.
- **Eksik skill:** Reload Roll movement skill Karar #5'te planlı (Exposed Line state Phase 2). Ama **Vent skill** (active Heat dump) eksik. **"Pressure Vent"** — Heat -%50 + 1.5sn atış hızı %50 hızlı.
- **Yeni pasif:** **"Last Bullet's Promise"** — Heat 90+'da iken her atış crit chance +%20. Overheat'e kadar push reward.
- **Risk:** Overheat post-cooldown +%150 dmg → Warblade Iron Reservoir ile kombine **broken** olabilir (aşağıda).

---

## BRAWLER — "Düşersen kalk."

**Kaynak: GÜÇLÜ** | **Cross-class: YÜKSEK** (Strike + Pressure paylaşımı)

Charge (0-5) bankalanabilir kaynak — **en derin** mekanik. RMB ile Overdrive Fuel transfer eleğant.

- **Balance önerisi:** Weave (RMB) "temas kaçınma Charge verir" — timing skill, çok cömert olursa pasif farming. **Önerin:** Weave perfect (0.2s window) = +1 Charge. Normal Weave = sadece i-frame, charge yok.
- **Eksik skill:** Charge 5 maksimumda iken burst skill yok — F (Overdrive) mod değişiyor ama "5 Charge tek vuruşta harca" yok. **"Knockout Hook"** — yeni slot: 5 Charge harca, tek mega vuruş + Cracked/Shattered state. Combat'a burst climax.
- **Yeni pasif:** **"Brawler's Rhythm"** — 4-hit Jab kombosu kesintisiz tamamlandığında 1.5sn tüm vuruşlar Pressure tag uygular. Combo discipline ödülü.
- **Risk:** Weave perfect window = parry mekaniği. Karar #6 "class'a özel parry skill'leri" ile uyumlu. Açık.

---

## SUMMONER — "Ben savaşmıyorum. Feda ediyorum."

**Kaynak: BELİRSİZ (spec yetersiz)** | **Cross-class: DÜŞÜK** (Soul-bond izolasyon)

Sacrifice Charges (0-4) zamanla + minyon ölümleriyle doluyor — pacing belirsiz. Faz 4'te geliyor, **tasarım eksikliği erkenden çözülmeli.**

- **Balance önerisi:** "Zamanla" Sacrifice doluyor — spec yok. **Önerin:** 1 Charge / 8sn idle, minyon ölümü = +1 charge ani. Max 4. Minyonsuz Summoner 32sn'de 4 charge yapar, savaşılır.
- **Eksik skill:** Bone Spike (LMB) **çok zayıf**, "minyonları hedefe yönlendirir" yardımcı eylem — direct damage outlet yok. **"Soul Lance"** RMB: 1 Sacrifice Charge harca, en yakın minyonu fırlat (minyon ölür + %200 hasar). Active sacrifice kimliğe uygun.
- **Yeni pasif:** **"Necromancer's Tax"** — Her aktif minyon Sacrifice gain rate'i +%25 hızlandırır (3 minyon = +%75 hızlanma). Minyon ekonomisini reward.
- **Risk:** Soul Lance + minyon = potansiyel CC infinite mi? 5sn ICD koy.

---

## HEXER — "10'a gelince sen bitiyorsun."

**Kaynak: GÜÇLÜ** | **Cross-class: ORTA** (Hex stack izolasyon, ama Echo ile sinerji potansiyeli)

Hex Stacks (0-10) düşman üzerinde birikir → 10'da Hexblast. **Net, eğitici, derin.**

- **Balance önerisi:** 10 stack hızı belirsiz. **Önerin:** LMB +1 stack / 0.4s. RMB cone +1 stack max 3 düşmana. 4-5sn'de 10 stack. Penitent Sovereign 60+sn savaşta Hexblast 2-3 kez tetiklenir.
- **Eksik skill:** **Stack koruma yok** — decay etmiyor mu? Spec yok. Karar lazım: **decay etmesin, hedef ölünce sıfırlanır.** Yayılım Hex Cascade (F) zaten var. **"Hex Bind"** — yeni slot: 1 düşmanı 2sn root + tüm stack'leri ona transfer. Tank manipülasyonu.
- **Yeni pasif:** **"Lingering Curse"** — Hexblast tetiklendiğinde patlayan düşmandan 3u radius içindeki tüm düşmanlara 3 Hex stack uygula. Mob temizleme.
- **Risk:** Karar #6 "Hexer cross-spread kuralı" korunuyor. Açık.

---

# 🎯 OPUS KARARI — En Kritik 5 Değişiklik (Öncelik Sırasıyla)

## 1. Cross-class kod mimarisi Karar #122'ye align — ŞİMDİ
`CrossClassSkillManager` T1/T2/T3/T4 tier sistemini bilmiyor. Faz 3'te 28-56 kombo gelecek, mevcut mimari ölçeklenmez.
- **Aksiyon:** rima-doc → `TASARIM/CROSS_CLASS_TECH_SPEC.md` güncelle
- **Neden şimdi:** Faz 2 başlamadan skeleton refactor; sonra teknik borç katlanır

## 2. Warblade'a Sundered state proaktif skill
Şu an Sundered = pasif modifier. Faz 1'in playtest sınıfı, Family Tag Fracture'ı proaktif kullanan skill yok.
- **Aksiyon:** "Verdict Cleave" skill ekle (LMB Beat 3 üzerine)
- **Neden şimdi:** Faz 1 offer pool ile test ediliyor; eksik skill = boş slot

## 3. Elementalist Mana + Convergence sadeleştir
Mevcut spec Faz 2'de implementlenemez. Convergence tier upgrade'e taşımak Karar #5 ile uyumlu.
- **Aksiyon:** Common = tek element. Rare = swap. Epic = Convergence
- **Neden şimdi:** Faz 2 Elementalist eklemesi yakın, konsept lock'lanmalı

## 4. Ravager Fury ekonomisi
Sadece hasar alarak Fury bağımlılığı boss savaşlarında ölü kalır.
- **Aksiyon:** Hook hit-on-success +15 Fury, miss = 0
- **Neden şimdi:** Faz 3 dual-class içinde Ravager secondary olarak gelirse, primary class hasar almama incentive verir; Ravager value veremez

## 5. Boss'ta execute skiller alternatif behavior
Universal "No HP<30% execute on boss" kuralı ama Severance/Death Blow/Final Strike/Flash Draw bossta **ne yapar** belirsiz.
- **Aksiyon:** Per-class burst convert: "execute = %200-300 hasar + state reset"
- **Neden şimdi:** Penitent Sovereign Faz 1'de zaten oyunda; kural net olmazsa playtest confusion + nerf gerekir

---

# ⚠️ Broken Kombinasyon Uyarıları

## 1. Warblade Iron Reservoir + Gunslinger Overheat
Cap-ulaşma indirimi + Overheat burst = 2 farklı sınıf cap-reward sistemi üst üste. **Skill CD %30 hızlı + atış hızı %50 hızlı + atış hasarı %150 = teorik 3x DPS.**
- **Çözüm:** İki "cap reward" pasifi aynı build'de olamasın (mutual exclusion tag)

## 2. Hexer + Elementalist Echo + Shadowblade Veil
3 farklı Family Tag = T4 Rift Proc Bond (%100 dmg + %50 armor pen). Hexer 10-stack + Echo detonate + Veil crit aynı düşmanda triplet — bossta 3-4sn'de execute hazır.
- **Çözüm:** T4 Rift Proc'a "bossta dmg %50 reduce" Universal Boss Rule eklemesi

## 3. Brawler Weave perfect + Ronin Iaido Stance parry
İki sınıf da timing-parry. Cross-class olduğunda iki parry pencere aynı anda → %100 dodge build.
- **Çözüm:** Bir build içinde max 1 parry mekaniği (Karar #5 movement cap'inin parry versiyonu)

---

# 💀 Dead Combo Listesi (Tasarım Boşlukları)

| Kombo | Sorun |
|---|---|
| **Summoner + Ranger** | Pozisyon paradoks (Summoner yakın, Ranger 4m+). Tag paylaşımı yok (Soul vs Pierce). |
| **Hexer + Ravager** | Stacker vs risk-taker zıt fanteziler. Bleed/Hemorrhage var ama Hex Stack transfer edilemez. İki kaynak rekabet ediyor. |
| **Elementalist + Warblade** | OWNS çakışmıyor ama sinerji yolu da yok (menzilli vs yakın). Faz 5'e "Burning Sundered" hybrid tag gerek. |
| **Ronin + Gunslinger** | Stillness vs continuous mobility zıt. Tag paylaşımı yok. "Rift Shot" gibi neutral cross-skill Faz 4 öncesi spec'lensin. |
| **Shadowblade + Brawler** | Veil/Scar (mesafe) vs Strike (yakın). Tag paylaşımı yok, fantasy çakışıyor (suikastçi vs frontal yumruk). Faz 5 expansion. |

---

# Karar Uyumluluk Kontrolü

- **Karar #60 (skill tier):** Hiçbir öneri ihlal etmiyor ✓
- **Karar #122 (Echo Resonance T1-T4):** Önerilerim Family Tag paylaşımını koruyor ✓
- **Karar #6 (Class-specific parry):** Brawler Weave + Ronin Iaido zaten class-specific ✓
- **Karar #80 (Class silhouette / weapon lock):** Hiçbir öneri dual-wield/weapon swap içermiyor ✓

---

# Orchestrator Next Steps (Sonraki Session)

1. **Önce karar:** Bu önerilerden hangileri LIVE'a alınacak? User onayı bekliyor.
2. **Eğer onaylanırsa:**
   - Kritik #1 (cross-class mimari) için ayrı teknik spec görevi: rima-design tekrar → T1-T4 mimari mapping (CrossClassEffectType → triggerTier + familyTag)
   - Kritik #2-5 için `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` güncellemesi (rima-doc)
   - Broken kombo mutual-exclusion için MASTER_KARAR_BELGESI'ne Karar #151 önerisi (build içinde max 1 "cap reward" pasifi, max 1 parry mekaniği)
   - Dead combo'lar Faz 5 backlog
3. **Warblade kod skeleton henüz yok** — Faz 1 implementation Warblade önce gerektirir

## İlgili Dosya Yolları

- `Assets/Scripts/CrossClass/CrossClassSkillManager.cs` — T1-T4 mimari refactor needed
- `Assets/Scripts/CrossClass/CrossClassSkillData.cs` — enum redesign (effectType → triggerTier + familyTag)
- `Assets/Scripts/Combat/Classes/Ronin/TensionSystem.cs` — Sheath Discipline pasif kolay eklenir
- Warblade kod skeleton **henüz yok**

---

**Generated:** 2026-05-20 S95 LATE
**Status:** Önerilebilir kararlar — user onayı bekleniyor, hiçbir kod/spec değişikliği yapılmadı
