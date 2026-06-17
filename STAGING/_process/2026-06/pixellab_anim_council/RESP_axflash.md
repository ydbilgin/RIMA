# RESP_axflash — PixelLab Weaponless/Armed State Prompt-Craft + Eksik Asset Envanteri (2026-06-16)

## Q1 — KÖK NEDEN ANALİZİ (Adversarial)

Hipotez **doğrudur**. Silahı her promptta sıfırdan çağırmak geometrik tutarsızlık yaratır.

*   **PixelLab Durumu:** Model her state'i ayrı çizdiği için kılıç boyutu ve şekli kareler arasında titrer.
*   **Hızlı Çözüm (Demo Bake):** Önce tek bir kılıçlı temel duruş (**Armed Anchor**) üret. Diğer state'leri (mid-run, strike-windup vb.) bu Anchor ID'sine bağlayıp `palette-snap=true` ve `AI-freedom = 0.35` ile üret. Kılıç tutarlı kalır.
*   **Gelecek Planı (Post-Demo Mount):** Silahı karakterden tamamen ayır. Karakteri silahsız üret, kılıcı Unity'de runtime'da eline tak (`HandAnchorAttach`). Demo sonrası kesinlikle bu yönteme geçilecek.

---

## Q2 — KALICI PROMPT-CRAFT REÇETELERİ

### (A) SİLAHSIZ (Weaponless) State Reçetesi
Karakterin eli boş, silah takmaya hazır duruşu.

#### Prompt Şablonu (Kopyala-Yapıştır)
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, [ACTION_STATE_POSE], EMPTY HANDS, hands empty, fists loosely clenched, weaponless stance, preserve armor and silhouette, no redesign, no weapons, no held items, nothing in hands
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: dark slate gray armor, brown leather straps, brass highlights, messy black hair
```
*Not: `[ACTION_STATE_POSE]` kısmına silah demeden pozu yaz: örn. `strike-windup pose, right arm drawn back, torso twisted`.*

#### Negatif Prompt
```markdown
sword, blade, weapon, dagger, shield, held objects, hands holding items, blurry edges, anti-aliasing, soft gradients, 3d render, isometric, watermark
```

#### DO / DON'T Kelimeleri
*   **DO:** `empty hands`, `fists loosely clenched`, `no held items`, `weaponless stance`, `nothing in hands`.
*   **DON'T:** `sword`, `blade`, `weapon`, `shield`, `slash VFX`.

---

### (B) TUTARLI-SİLAHLI (Armed Baked) State Reçetesi
Demo için silahı gömülü ama kaymayan animasyon üretme.

#### Adım 1: Armed Anchor (Kılıç Tasarımı Kilitleme)
`palette-snap=false` ile kılıcı karaktere giydir.
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, breathing idle stance, holding a massive two-handed greatsword, metal blade visible pointing down-right, brass crossguard, preserve armor and silhouette, no redesign, no VFX
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
[PALETTE]: dark slate gray armor, brass highlights, messy black hair, metallic gray sword
```

#### Adım 2: Aksiyona Dönüştürme (Anchor ID'si üzerinden)
`palette-snap=true` ve `AI-freedom = 0.35` ile anchor'dan türet.
```markdown
[CHARACTER]: same warblade, high top-down 2D game sprite, [ACTION_STATE_POSE] with the same greatsword, preserving weapon shape and crossguard design, preserve armor and silhouette, no redesign, no VFX
[STYLE]: 2D pixel art, crisp hard edges, matte hand-pixeled clusters, no anti-aliasing, top-down 35 degree ARPG angle, transparent background
```

#### Genel Prompt-Craft Kuralları
1.  **State-First:** Önce pozu (`create_character_state`) sabitle, sonra animasyonu (`animate_character`) türet.
2.  **Strict Angle:** Her promptta `top-down 35 degree ARPG angle` yaz.
3.  **Low Freedom:** Türetmelerde `AI-freedom` değerini `0.3 - 0.4` aralığında tut.
4.  **Palette Snap:** Renklerin bozulmaması için `palette-snap=true` kullan.
5.  **No Buzzwords:** "highly detailed" gibi boş kelimeleri sil, `crisp hard edges, no anti-aliasing` kullan.
6.  **Transparent Padding:** Karakterin kenarlara çarpmaması için canvas'ta boş alan bırak (`leave wide transparent headroom`).
7.  **Arm Mechanices (Silahsız):** Hareketi kılıçla değil kol hareketleriyle tarif et.
8.  **Standard Weapon Angles:** Silahı yatay-sağ çizdir (`horizontal-right`).
9.  **Negative Filtering:** Silahsızlarda negatif prompta tüm silah isimlerini yaz.
10. **5+3 Direction Rule:** Sadece S, SE, E, NE, N yönlerini üret; diğer 3 yönü Unity'de mirror et.

---

## Q3 — EKSİK / YAPILMASI GEREKEN ASSET ENVANTERİ

Demo'ya 3 gün kala neyi yapmalı, neyi ertelemeli listesi:

1.  **Warblade Armed-Anchor REDO + Idle/Run/LMB (P1 - Demo Kritik):**
    *   **Yap:** Sunumda kılıcın sürekli değişmesi kötü görünür. Golden-path'in ana karakteridir. **Kritik.** (Kullanıcı PixelLab Web UI ile yapacak, 3-4 saat).
2.  **Elementalist Energy Bolt Sprite (Mermi) (P1 - Demo Kritik):**
    *   **Yap:** Sarı top mermi çok sırıtır. **Kritik.** (Orchestrator MCP `create_1_direction_object` ile üretecek, 5 dk).
3.  **VFX Core: Fireball & Fire Impact & Glacial Spike (P1 - Demo Kritik):**
    *   **Yap:** Savaş hissiyatı için şart. Golden-path combat büyülerini besler. **Kritik.** (Orchestrator MCP ile üretecek, Unity'de bağlanacak).
4.  **VFX Core: Frozen Orb & Light Beam (P2 - Pas Geç):**
    *   **Ertele:** Golden-path'te kullanılmayan veya sunumda gösterilmeyecek skillerdir. 3 gün kala vakit harcama. **Post-Demo.**
5.  **Run-Map Sembolleri / Chrome (P3 - Pas Geç):**
    *   **Ertele:** UI harita detayıdır. Sadece savaş arenası gösterileceğinden mevcut basit butonlar yeterlidir. Çizimle uğraşma. **Post-Demo.**
6.  **Alt Portal Bar + Mavi Işın Beam (T5) (P2 - Vakit Kalırsa):**
    *   **Ertele/Bypass:** Zafer sahnesi geçişinde portal efekti olarak kullanılır. Vakit kalırsa hızlıca 1 sprite üretilebilir. Değilse basit bir particle/VFX ile geçiştir.
7.  **Cliff Mapler için Tile/Backdrop (P1 - Demo Kritik):**
    *   **Yap:** Haritanın uçurum kenarlarında zemin boş görünmemeli. (Unity terrain düzenlemesiyle mevcut tile'lar dizilmeli. Yeni çizim gerekmez). **Kritik.**

### axflash Eylem Planı (Hızlı Teslimat)
*   **Kullanıcıya Kalan:** Warblade karakterini Web UI üzerinden Armed-Anchor REDO yöntemiyle yeniden üretmek ve Aseprite'ta temizlemek.
*   **Bana (Ajan) Kalan:** `Energy Bolt`, `Fireball` ve `Glacial Spike` mermi/efektlerini MCP tool'uyla otonom üretip Unity klasörüne koymak.
