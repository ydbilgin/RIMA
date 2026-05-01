# RIMA — Animasyon Yeniden Tasarımı
*Karar tarihi: 2026-04-06 | Tüm kararlar Claude'a ait*

---

## Durum Özeti (Play Test sonrası tespitler)

| Varlık | Sorun | Karar |
|--------|-------|-------|
| HalfThrall | Controller yoktu, VoidThrall controller'ı takılıydı | ✅ Fix: HalfThrall.controller oluşturuldu |
| VoidThrall | Death'te Idle clip var, Attack state yok | Sıfırdan üretilecek |
| Penitent | Attack "cross-punch" → temaya uymuyor | Sıfırdan üretilecek |
| ChainWarden | Bazı state'ler siyah | Death BlendTree düzeltildi, attack yeniden üretilecek |
| RelicCaster | Projectile player'a gelmiyor | Attack yeniden üretilecek, kod düzeltilecek |
| FractureImp | Siyah aralar, attack tekrarlamıyor (visual) | Attack yeniden üretilecek |
| Elementalist | Attack yumruk atıyor | Tamamen yeniden üretilecek |
| Ranger | Attack yumruk atıyor, ok yok | Tamamen yeniden üretilecek |
| Shadowblade | Attack animasyonları berbat | Attack'lar yeniden üretilecek |
| Tüm karakterler | Ölüm trigger'ı bağlı değildi | ✅ Fix: Health.OnDeath → TriggerDeath bağlandı |
| Tüm moblar | Death tek yön clip (sadece south) | ✅ Fix: BlendTree'ye dönüştürüldü |

---

## MOB ATTACK YENİDEN TASARIMI

### HalfThrall — "Void Grab"
**Kimlik:** VoidThrall'ın küçük, zayıf versiyonu. Sürü halinde gelirler.
**Attack:** Küçük pençeleriyle bir yakalama/tırmalama hareketi. Hızlı, 4 frame.
- Frame 1-2: Pençeyi geriye çekiyor
- Frame 3: İleri atılıyor (reach)
- Frame 4: Geri çekiliyor
**Mekanik:** Melee hit. Hafif knockback yok — sadece hasar.
**Animasyon sayısı:** 4 yön (N/S/E/W) × 4 frame = temel set

---

### VoidThrall — "Void Pulse"
**Kimlik:** Void enerjisiyle şişirilmiş ağır düşman. Patlama riski barındırıyor.
**Attack:** Kollarını açıp göğsünden void enerji dalgası yayıyor. AoE yakın mesafe.
- Frame 1-2: Kollar geriye açılıyor
- Frame 3-4: Göğüs şişiyor, void enerji birikimi
- Frame 5-6: Enerji patlar, kollar öne geliyor
**Mekanik:** AoE melee. VoidThrall_DeathSplit ile sinerji (öldürünce HalfThrall spawn).
**Animasyon sayısı:** 4 yön × 6 frame + death 4 yön × 6 frame

---

### Penitent — "Holy Condemnation"
**Kimlik:** Dini fanatik. Ceza verir, af vermez.
**Attack (ESKİ cross-punch → SIFIRLA):**
**Attack (YENİ):** Elleri gökyüzüne açıyor, başının üstünde kutsal enerji birikiyor → yere indiriyor.
- Frame 1-2: Elleri kaldırıyor
- Frame 3-4: Enerji yukarıda parlıyor (static glow)
- Frame 5-6: Elleri yere çarpıyor, enerji yayılıyor (küçük AoE flash)
- Frame 7: Dönüş pozu
**Mekanik:** MobAttack_PenitentCombo → bu animasyonla sync olacak.
**Not:** Ölüm animasyonu da yeniden üretilecek — "çökmek" değil "diz çöküp öne kapanmak."
**Animasyon sayısı:** 4 yön × 7 frame (attack) + 4 yön × 8 frame (death)

---

### ChainWarden — "Chain Lash"
**Kimlik:** Zincirli gardiyan. Uzak mesafeden kontrol eder.
**Attack (ESKİ cross-punch → SIFIRLA):**
**Attack (YENİ):** Elindeki zinciri öne doğru fırlatıyor, geri çekiyor.
- Frame 1-2: Zinciri geriye alıyor (charge)
- Frame 3-4: Fırlatıyor — zincir uzanıyor (havada ayrı sprite olarak gösterilebilir)
- Frame 5-6: Zincir geri çekiliyor
**Mekanik:** MobAttack_ChainPull — bu animasyonla sync. Chain pull mekaniği aktive.
**Animasyon sayısı:** 4 yön × 6 frame

---

### RelicCaster — "Relic Bolt"
**Kimlik:** Büyücü. Uzaktan saldırır, projectile fırlatır.
**Attack (ESKİ cross-punch → SIFIRLA):**
**Attack (YENİ):** Elindeki reliki öne doğru kaldırıyor, oradan enerji topu fırlatıyor.
- Frame 1-2: Reliki kaldırıyor
- Frame 3-4: Şarj oluyor (enerji birikiyor)
- Frame 5: Fırlatma pozu (projectile bu anda spawn olur)
- Frame 6: Dinlenme pozuna dönüş
**Mekanik:** MobAttack_Barrier → aslında bu bir projectile saldırı olmalı.
**Kod değişikliği gerekli:** MobAttack_Barrier yerine projectile fırlatma sistemi.
**Animasyon sayısı:** 4 yön × 6 frame

---

### FractureImp — "Shard Slash"
**Kimlik:** Hızlı, küçük şeytan. Çok vuran, az hasarlı.
**Attack (ESKİ cross-punch → SIFIRLA):**
**Attack (YENİ):** İki pençesiyle hızlı X şeklinde çapraz slash.
- Frame 1: Sol pençe geçiyor
- Frame 2: Sağ pençe geçiyor
- Frame 3: Geri çekiliyor
**Hız:** Diğer moblardan 2x hızlı. Görsel dinamizm önemli.
**Mekanik:** Hızlı ama zayıf hasar. FractureImp_ShardScatter ölümde aktive.
**Animasyon sayısı:** 4 yön × 3 frame (hızlı attack)

---

## KARAKTER ATTACK YENİDEN TASARIMI

### Elementalist — "Elemental Cast"
**Problem:** Şu an yumruk atıyor — tamamen yanlış.
**Çözüm:** Mouse yönüne bakan cast animasyonu. Elinden elemental projectile çıkıyor.
- Frame 1-2: El öne uzanıyor
- Frame 3: Elemental enerji toplanıyor (parlama)
- Frame 4: Projectile fırlatılıyor (bu frame'de projectile spawn olur)
- Frame 5-6: El geri çekiliyor
**Görsel:** Şimdilik ateş (Fireball) — farklı elementler için farklı renk tonu
**Yön sistemi:** 4 yön animasyon yeterli (N/S/W/E) — East → West + flipX
**Animasyon sayısı:** 4 yön × 6 frame

---

### Ranger — "Arrow Shot"
**Problem:** Yumruk atıyor — tamamen yanlış.
**Çözüm:** Ok atışı animasyonu. Mouse cursor yönüne.
- Frame 1-2: Yay geriliyor
- Frame 3: Tam gerilmiş poz (nişan)
- Frame 4: Bırakma (ok bu frame'de spawn olur)
- Frame 5: Yay gevşiyor
**Yön sistemi:** Mouse cursor yönüne bakıyor (Hades gibi)
**GDD kararı:** Karakter sprite'ı mouse yönüne döner (yönden bağımsız ok atışı)
**Ok mekaniği:**
- Ok havada görünen Projectile olarak spawn olur
- Düşmana çarptığında hit efekti (renk flash)
- Belirli range sonra kaybolur
**Animasyon sayısı:** 4 yön × 5 frame (+ ok projectile sprite'ı)

---

### Shadowblade — "Shadow Combo"
**Problem:** Attack animasyonları berbat.
**Çözüm:** 3 farklı combo step animasyonu.
- **Step 0:** Sol bıçak yatay slash
- **Step 1:** Sağ bıçak dikey slash
- **Step 2:** Çift bıçak ileri thrust
**Hız:** Elementalist'ten hızlı. Animasyon 4-5 frame max.
**Yön sistemi:** Hareket yönüne bakıyor (mevcut sistem korunur)
**Animasyon sayısı:** 4 yön × 3 step × 5 frame = 60 frame total

---

## DASH YENİDEN TASARIMI (her class farklı)

### Warblade — "Iron Surge"
**Görsel:** Ağır, yer titreten forward charge. Dash sırasında kısa metal trail.
**Animasyon:** Düşük öne eğilme → hızlı forward lean → ayakta bitiş (3-4 frame)
**Unity efekti:** Dash sırasında kısa metalik iz (Particle System, sprite trail)
**Ses:** Ağır metal ayak sesi + whoosh

### Elementalist — "Blink"
**Görsel:** Küçük teleport. Kaybolup 1.5 birim ötede belirir.
**Animasyon:** Fade out (2 frame) → boş → fade in (2 frame)
**Unity efekti:** Çıkış noktasında küçük elemental puf (ateş veya buz parçacıkları)
**Ses:** Magical teleport pop

### Ranger — "Vault"
**Görsel:** Hafif, graceful geriye atlama. Tehlikeden uzaklaşma.
**Animasyon:** Sıçrama (2 frame) → havada (1 frame) → iniş (2 frame)
**Yön:** Hareket yönünün tersine (geri atlama hissi)
**Unity efekti:** İnişte küçük toz bulutu
**Ses:** Hafif atlama sesi

### Shadowblade — "Shadow Step"
**Görsel:** Anlık kaybolma ve başka yerde belirme. Gölge siluet bırakır.
**Animasyon:** Anlık dissolve → başlangıç noktasında gölge silüet (1-2 frame)
**Unity efekti:** Başlangıç noktasında 0.3s bekleyen gölge sprite
**Ses:** Dark whoosh, sessiz iniş

---

## HIT FEEDBACK (PixelLab gerektirmiyor — Unity'de yapılır)

Her düşmanın ve karakterin hit aldığında görsel feedback'i:
- **White flash:** 0.1s süre, SpriteRenderer.color = beyaz → normal
- **Knockback micro:** 0.05s hafif geri itilme (Rigidbody2D velocity burst)

Bu rima-codex'e verilebilir — `HitFlash.cs` komponenti, Health.OnDamageTaken'e baglanir.

---

## PixelLab Üretim Önceliği

| Batch | İçerik | Öncelik |
|-------|---------|---------|
| BATCH_MOB_ATTACKS | HalfThrall/VoidThrall/Penitent/ChainWarden/RelicCaster/FractureImp attack | 🔴 Kritik |
| BATCH_CHAR_ATTACKS | Elementalist/Ranger/Shadowblade attack | 🔴 Kritik |
| BATCH_VOID_DEATH | VoidThrall + HalfThrall death animasyonları | 🟠 Yüksek |
| BATCH_DASH | 4 class dash animasyonu | 🟡 Orta |

---

## Kod Değişiklikleri (animasyonlarla birlikte)

| Değişiklik | Dosya | Not |
|-----------|-------|-----|
| Ranger projectile sistemi | Ranger_SkillController.cs | Ok havada görünmeli |
| RelicCaster projectile | MobAttack_Barrier.cs → MobAttack_Projectile.cs | Barrier değil fırlatmalı |
| HitFlash component | HitFlash.cs (yeni) | Tüm varlıklara eklenecek |
| EnemyAnimator attack trigger | EnemyAnimator.cs | Bool → Trigger daha temiz |
