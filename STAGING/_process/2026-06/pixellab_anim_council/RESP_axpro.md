# RESP_axpro — PixelLab Weaponless/Armed State Prompt-Craft + Eksik Asset Envanteri (2026-06-16)

## Q1 — KÖK NEDEN ANALİZİ (Adversarial)

Orchestrator'ın Kök Neden Analizi **tasarım ve estetik açıdan kesinlikle doğrudur.**

1. **"Şekil Değiştiren Kılıç" (Shape-Shifting Blade) Sendromu:** Piksel sanatı, doğası gereği her pikselin belirli bir formu temsil ettiği bir disiplindir. State prompt'larında silahın soyut olarak ("sword") bağımsız çağrılması, difüzyon modelinin kılıcı her frame veya yönde farklı bir piksel cluster'ı olarak çizmesine sebep olur. Bu durum, animasyon oynatıldığında kılıcın "titremesine", "şekil değiştirmesine" ve siluet kalitesinin çökmesine yol açar.
2. **Parametre Analizi:**
   - **`palette-snap`:** Renk uyumunu garantiler ama geometrik tutarlılığı kurtarmaz.
   - **`AI-freedom` / `strength`:** Modelin tasarımsal serbestliğini yönetir. Düşük değerler (`0.3 - 0.4`) silahın ana hatlarını korurken, yüksek değerler silahın kabza, balçak ve namlu oranlarını tamamen bozar.
3. **Çözüm Yolu:**
   - **Baked (Gömülü) Çözüm (Demo Kısa Vade):** Kılıcın formunu bir **Armed Anchor** pozunda (örneğin kılıcın en belirgin olduğu `breathing-idle` pozu) dondurup piksel yapısını sabitlemek; ardından tüm aksiyonları bu anchor'dan `palette-snap=true` ile türeterek geometrik formu alt state'lere taşımak en doğru baked yaklaşımdır.
   - **Decoupled (Bağımsız) Çözüm (Post-Demo Uzun Vade):** Silahın karakterden tamamen ayrılması, el koordinatlarının (`HandAnchorAttach`) tanımlanması ve silahın statik/yönlü tek bir sprite olarak runtime'da yerleştirilmesi, bu geometrik tutarsızlığı kökten çözen profesyonel tasarım standardıdır.

---

## Q2 — KALICI PROMPT-CRAFT REÇETELERİ

### (A) SİLAHSIZ (Weaponless) State Reçetesi
Weapon-mount altyapısı için silahsız, anatomik ve kinetik olarak doğru pozlar üreten prompt yapısı.

#### Prompt Şablonu (Kopyala-Yapıştır)
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, [ACTION_STATE_POSE], EMPTY HANDS, hands open and curved in a weaponless grip posture, fingers curled around an invisible haft, weight balanced, preserve armor silhouette, no redesign, no weapons, no held items, nothing in hands
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: dark slate gray armor, brown leather straps, brass highlights, messy black hair
```

*Not: `[ACTION_STATE_POSE]` kısmına silah kelimeleri geçmeden hareketin mekaniğini yazın (örn: "strike-windup pose, right arm drawn back across the torso, chest twisted, fist clenched as if gripping a two-handed handle").*

#### Negatif Prompt
```markdown
sword, blade, weapon, dagger, staff, whip, gun, shield, holding item, carrying object, blurry edges, anti-aliasing, soft gradients, 3d render, isometric, watermark
```

#### Kelime Listesi (DO / DON'T)
* **DO (Yazılması Gerekenler):** `empty hands`, `weapon-ready grip posture`, `fists clenched as if gripping invisible haft`, `open palm slightly curved`, `hands empty`, `nothing in hands`.
* **DON'T (Yasaklı Kelimeler):** `wielding sword`, `blade ready`, `sword at side`, `sword strike`, `slash`, `weapon identity`.

---

### (B) TUTARLI-SİLAHLI (Armed Baked) State Reçetesi
Demo sunumu için kılıcı sprite'a gömülü (baked) ama tutarlı üreten armed-anchor reçetesi.

#### Adım 1: Armed Anchor State Üretimi (Kılıç Tasarım Kilidi)
Karakterin kılıçlı temel halini belirleyen anchor state'tir. `palette-snap=false` ile kılıcın görsel dilini kurarız.
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, breathing idle stance, holding a massive two-handed greatsword, metal blade clearly visible pointing down-right, brass crossguard, dark leather grip, preserve armor silhouette, no redesign, no VFX
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: dark slate gray armor, brass highlights, messy black hair, metallic gray steel blade
```

#### Adım 2: Aksiyon State'lerinin Anchor'dan Türetilmesi
**Önemli:** Adım 1'deki Armed Anchor ID'si referans verilerek çağrılır. **`palette-snap=true`** ve **`AI-freedom = 0.35`** olarak ayarlanır.
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, [ACTION_STATE_POSE] with the same greatsword, preserving weapon identity, keeping the exact blade design and crossguard form, hands gripping weapon, preserve armor silhouette, no redesign, no VFX
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: snaps to parent state palette
```

#### Genellenebilir Prompt-Craft Kuralları (10 Madde)
1. **Biomechanical Focus (Silahsız):** Silahsız promptlarda eylem tanımlarını "kılıç savurma" gibi nesne tabanlı değil, "omuz rotasyonu", "kol açısı" gibi eklem ve kinetik tabanlı yapın.
2. **Establish the Armed Anchor (Silahlı):** Baked silahlarda mutlaka önce durağan bir `Armed Anchor` state'i üretip kılıç geometrisini onaylayın, ardından aksiyonları bu anchor referansı üzerinden türetin.
3. **Strict Camera Lock:** Her promptta `top-down 35 degree ARPG angle` ibaresini kullanarak 3/4 açılı karakter oranlarını koruyun.
4. **Palette Lock (Snap):** Renk tutarlılığı için türetilmiş state'lerde `palette-snap=true` kullanarak renklerin pikseller arası kaymasını önleyin.
5. **Low Freedom for Consistency:** `AI-freedom` değerini `0.3 - 0.4` aralığında sabitleyin; daha yüksek değerler kılıcın ve zırhın geometrik kimliğini bozar.
6. **No Redesign Guard:** Her state promptunun sonuna `preserve armor and silhouette, no redesign, no VFX` koruma etiketlerini ekleyin.
7. **Negative Weapon Filter (Silahsız):** Silahsız üretimlerde negatif prompt kısmını `sword, blade, weapon, dagger, shield, held objects` kelimeleriyle doldurun.
8. **Horizontal-Right Weapon Bias:** Bağımsız silah üretimlerinde (`create_object`), silahın her zaman yatay-sağ (kabza solda, uç sağda) çizilmesini sağlayın. Bu, Unity'deki pivot ayarlamalarını standartlaştırır.
9. **Transparent Headroom:** Animasyonlarda lunge veya vuruş sırasında karakterin kırpılmasını önlemek için canvas'ta en az %40 boşluk (`leave wide transparent headroom`) bırakın.
10. **Matte Pixel Clusters:** Bulanık veya gradyanlı modern motor çıktılarından kaçınmak için `matte hand-pixeled clusters, no anti-aliasing` etiketini prompta gömün.

---

## Q3 — EKSİK / YAPILMASI GEREKEN ASSET ENVANTERİ

Demo 19 Haziran sunumu için estetik ve oyun tasarımı açısından eksik assetlerin değerlendirilmesi:

1. **Warblade Armed-Anchor REDO + Idle/Run/LMB Animasyonları (P1 - Sunum Kritik):**
   * *Estetik Yorum:* Karakterin ana animasyonlarında kılıcın şekil değiştirmesi sunum kalitesini doğrudan düşürür. Golden-path boyunca oyuncunun gözü bu karakterde olacaktır. Redo **kritik derecede gereklidir**.
   * *Fizibilite:* 3 günde PixelLab Web UI üzerinden 1 anchor ve 3 animasyon türetmek son derece gerçekçidir. Aseprite'ta temizlik süresi dahil max 4-5 saattir.
2. **Elementalist Energy Bolt Core Sprite (P1 - Demo Kritik):**
   * *Estetik Yorum:* Mevcut sarı top mermi oyunun "Alabaster Dawn" stilini baltalamaktadır. Nötr cyan-white core sprite üretilerek `SkillVfx.cs` tint özelliği doğrulanmalıdır.
   * *Fizibilite:* Çok pratiktir. MCP `create_1_direction_object` veya `create_object` ile tek seferde 64px boyutunda üretilebilir.
3. **VFX Core: Fireball & Fire Impact & Glacial Spike (P1 - Demo Kritik):**
   * *Estetik Yorum:* Büyülerin düşmana çarpma hissi ve görsel geri bildirimi (juice) roguelite combat'ın temelidir. Fireball ve Glacial Spike golden-path combatında kullanılmaktadır.
   * *Fizibilite:* MCP üzerinden hızlıca üretilebilir, Unity'de scale/tint ayarlarıyla 3 günde rahatlıkla yetişir.
4. **VFX Core: Frozen Orb & Light Beam (P2 - Post-Demo):**
   * *Estetik Yorum:* Üst segment büyülerdir. Golden-path akışında bu yetenekler kilitli veya kullanılmıyorsa demo için kritik değildir. Sunum süresini doldurmayacaklarsa üretimi post-demoya ertelenmelidir.
5. **Run-Map Sembolleri / Chrome (P3 - Düşük):**
   * *Estetik Yorum:* Harita sistemi bir UI elementidir. 3 günlük demo hedeflerinde (sadece arena savaşı ve mekanik testleri) şık ama statik bir placeholder UI yeterlidir. PixelLab ile harita node'u üretmek zaman kaybıdır.
6. **Alt Portal Bar + Mavi Işın Beam (T5) (P2 - Orta):**
   * *Estetik Yorum:* Beat 5 geçiş sahnesinde portalın aktifleşmesi ve oyuncunun odayı temizledikten sonra zafer odasına geçişi için atmosferik bir unsurdur. Vakit kalırsa yapılmalıdır.
7. **Cliff Mapler için Tile/Backdrop (P1 - Demo Kritik):**
   * *Estetik Yorum:* Zemin dışındaki boşluğun gri veya siyah bir boşluk (void) olarak görünmesi derinlik hissini yok eder. Haritanın kenarlarındaki uçurum (cliff) tile'ları ve backdrop tasarımı tamamlanmalıdır.
   * *Fizibilite:* Mevcut tile'lar ve accent'ler kullanılarak Unity'de sahne tasarımı (procedural patch veya manual brush) ile çözülmelidir. 1 günlük çalışma ile bitirilebilir.
