# RIMA — Kullanıcı Adım Adım Guide

**Tarih:** 2026-05-11
**Amaç:** Senin ne yapacağın + Claude'un ne yapacağı, her oturum için. Hangi tuşa basacağını söyleyecek seviyede detaylı.

**Okuma sırası:** Faz 0 → Faz 1 → Faz 2. Her faz önceki PASS olmadan başlamaz.

---

## İşaretler

🧑 **Sen yapacaksın** (UI tıklama, görsel onay, dosya kaydetme)
🤖 **Ben yapacağım** (kod, prompt, dispatch, doc, QC)
✅ **Validation** (bu adım bitti mi anlamak için)

---

## FAZ 0: POC GATE

**Amaç:** Yeni Unity URP 3D projesinde 1 oda + 1 karakter + lighting test. PASS olmadan üretim başlamaz.

**Tahmini süre:** 2-3 saat.

### Adım 0.1 — Yeni Unity Projesi

🧑 **Sen:**
1. Unity Hub aç → New Project
2. Template: **Universal 3D** (URP)
3. Editor Version: Unity 6.3 LTS
4. Project name: `RIMA_2.5D`
5. Location: tercih ettiğin dizin
6. Create

🤖 **Ben:**
- UnityMCP üzerinden yeni instance'a bağlanmayı bekliyorum
- Sen "proje açıldı" der demez bağlanırım

✅ **Validation:** Unity Editor açık, boş URP scene görünüyor

---

### Adım 0.2 — Salvage Manifest

🤖 **Ben:**
- Hangi scriptler/asset'ler yeni projeye taşınacak liste hazırlarım
- Codex'e dispatch etmeden önce sana göstereceğim

🧑 **Sen:**
- Listeyi onayla
- (Veya "şunu da ekle" de)

✅ **Validation:** Manifest dokümanı onaylı

---

### Adım 0.3 — Scripts + Folder Structure

🤖 **Ben (UnityMCP + rima-codex):**
- Folder tree oluştur: Assets/Scripts/{Runtime, Editor}, Assets/Sprites/{Weapons, Bodies, Anchors}, Assets/Prefabs, Assets/Materials/PixelArt
- Billboard.cs (SpriteRenderer her frame kameraya döner)
- PixelArtMaterial.shader (URP Unlit + Point filter)
- Pixel-Perfect Camera setup script
- WeaponController.cs stub (AnchorFollow + AttackDriven)
- WeaponAnchorMap.cs (ScriptableObject)

🧑 **Sen:**
- Hiçbir şey yapmıyorsun
- Editor'ün compile bitirmesini bekle (~30 sn)

✅ **Validation:** Console'da hata yok. `Assets/Scripts/Runtime/Billboard.cs` görünüyor

---

### Adım 0.4 — Test Sahnesi

🤖 **Ben (UnityMCP):**
- Yeni Scene: `POC_Test.unity`
- 3D Plane (floor, 5×5 unit, gri material)
- 3D Cube'lardan duvarlar (ProBuilder çıkıntı stili — locked decision)
- Directional Light (45° yukarıdan)
- Camera: Orthographic, position (0, 8, -5), rotation (35, 0, 0), size 5
- Empty GameObject "Player" + Billboard component

🧑 **Sen:**
- Editor'da sahneye bak. Tepeden açılı bir oda görüyor olmalısın
- "OK" de

✅ **Validation:** Sahne yukarı açılı bakış, duvarların üstü görünüyor

---

### Adım 0.5 — Billboard Test

🤖 **Ben:**
- Player GameObject'e SpriteRenderer ekle
- Test sprite olarak Warblade south anchor'ı import et (PIXELLAB_OUTPUTS/'tan kopya)
- Billboard.cs component'i ekle

🧑 **Sen:**
1. Play tuşuna bas
2. Scene view'da kamera Y rotasyonunu yavaşça çevir (Scene gizmo'sundan)
3. Sprite hep kameraya bakıyor mu kontrol et

✅ **Validation:** Sprite kamera nereye baksa hep cepheden görünüyor, billboard çalışıyor

---

### Adım 0.6 — Pixel-Perfect Kontrol

🧑 **Sen:**
1. Scene view'da Player'a yaklaş (zoom in)
2. Sprite'a bak — pixel'ler net mi? Bulanık mı?
3. Karakteri yavaşça hareket ettir (Transform.position W/S)
4. Hareket ederken pixel'ler shimmer yapıyor mu (titreşim)?

🤖 **Ben:**
- Shimmer varsa → Pixel-Perfect Camera ayarları + position snap fix dispatch ederim
- Bulanıksa → Filter Mode = Point kontrolü

✅ **Validation:** Pixel'ler net, shimmer yok, karakter okunaklı

---

### Adım 0.7 — Lighting Test

🧑 **Sen:**
1. Directional Light'ın rotation'ını değiştir (Scene view'dan)
2. Karakter sprite'ında ışık etkisi görünüyor mu?
3. Pixel art bozulmuyor mu (gradient/blur)?

🤖 **Ben:**
- Pixel art bozulduysa → URP Unlit shader'a geç (önerilen)
- Lit shader istiyorsan → Toon shader ile binary shadow setup

✅ **Validation:** Lighting görünüyor ama pixel art bozulmuyor. Lit/Unlit kararı netleşti.

---

### POC GATE KARARI

🤖 **Ben:**
- 7 alt-adımın hepsi PASS oldu mu kontrol ederim
- PASS → Faz 1'e geç
- FAIL → root cause fix dispatch et, tekrar test

🧑 **Sen:**
- POC PASS dediğimde "OK, Faz 1'e geç" de

✅ **Validation:** Sahnede 1 oda + 1 billboard karakter + ışık çalışıyor, pixel-perfect

---

## FAZ 1: Shadowblade Pilot

**Amaç:** 1 sınıf üzerinde tüm 5 aşamayı uçtan uca yap. Pipeline validate.

**Tahmini süre:** 3-4 saat aktif sen + 4-5 saat arka plan ben.

### Adım 1.1 — Face Cleanup

🧑 **Sen:**
1. Aseprite aç
2. `PIXELLAB_OUTPUTS/shadowblade/south_anchor.png` aç
3. Yeni layer üstüne "FaceMask" — yüz bölgesini siyahla doldur (saç/kapüşon dışında)
4. PixelLab Extension > Inpaint butonuna bas
5. Prompt:
   ```
   Improve facial features. Sharper eyes, defined nose, clean mouth.
   Maintain character identity (same gender, age, ethnic features).
   Pixel art, 64 PPU equivalent, NO anti-alias, hard pixel edges,
   match surrounding palette exactly. Small facial detail level (~16-20 pixels for face region).
   ```
6. 2-3 iterasyon, beğen
7. FaceMask layer'ı sil (sadece result kalır)
8. Save as: `PIXELLAB_OUTPUTS/shadowblade/south_clean.png`

🤖 **Ben:**
- Aseprite çalışırken ben Faz 1.2 prompt'larını hazırlarım (paralel)

✅ **Validation:** Yüz net, vücudun geri kalanı aynı, palette korunmuş

**Süre:** ~10-15 dk

---

### Adım 1.2 — Weapon Removal (Body-Only Anchor)

**NEW > PRO kuralı:** Önce `edit_image` (NEW, ücretsiz) dene. Drift varsa `edit_image_pro` (PRO, 20 gen).

🤖 **Ben:**
- Edit prompt + suffix hazırlarım (3 yön için)
- "PRESERVE EXACTLY: 30-35 degree 3/4 top-down view, 1px solid black outline, identical palette, identical proportions, identical pixel density (64 PPU), no resampling, no smoothing, hard pixel edges only."

🧑 **Sen:**
1. PixelLab Web App aç (3 sekme aç)
2. Her sekmede **önce Edit Image (NEW, ücretsiz)**:
   - URL: `pixellab.ai/create?tool=edit_image`
   - Sekme 1: south_clean.png → "remove dual blades, show empty hands relaxed" + suffix
   - Sekme 2: east_clean.png → aynı
   - Sekme 3: north_clean.png → aynı
3. Generate (paralel, sıfır credit harcaması)
4. Sonuçları incele:
   - **PASS** (outline temiz, palette korunmuş, drift yok) → kaydet, sonraki adıma geç
   - **FAIL** (drift, palette bozulma, outline yarım) → Edit Image Pro (PRO 20 gen) ile yeniden dene
5. Final kayıt:
   - `shadowblade_south_body.png`
   - `shadowblade_east_body.png`
   - `shadowblade_north_body.png`

🤖 **Ben:**
- rima-qc çağırırım: outline + palette + drift kontrolü
- FAIL ise Pro'ya yönlendiririm

✅ **Validation:** 3 yön body-only anchor, eller "boş kavrama" pozunda

**Süre:** ~30-45 dk (NEW PASS olursa daha hızlı)
**Credit:** 0-60 (NEW ücretsiz; PRO fallback olursa 3 yön × 20 = 60)

---

### Adım 1.3 — Body Animation (silahsız)

🤖 **Ben:**
- Her animasyon için Custom Animation V3 promptu hazırlarım
- 7 anim × 3 yön = 21 anim
- "EMPTY HANDS, gripping as if holding dagger" suffix'i her attack'ta

🧑 **Sen:**
1. PixelLab Web App > Shadowblade karakter sayfası > Add Animation > Custom Animation V3
2. Her anim için:
   - Start Frame: body-only anchor (Adım 1.2'den)
   - Frame Count: 4-8 (anime göre)
   - Prompt: benim verdiğim
3. Sonuçları kaydet: `PIXELLAB_OUTPUTS/shadowblade/anims/<anim_name>_<dir>.png`

**Anim listesi:**
- idle (6 frame)
- walk (6 frame)
- hurt (4 frame)
- death (8 frame)
- dash (4 frame)
- attack_LMB (8 frame, 2 segment)
- attack_RMB (8 frame, 2 segment)

🤖 **Ben:**
- rima-qc her batch sonrası inceler

✅ **Validation:** 21 anim sheet hazır, ellerde silah görünmüyor, "kavrama pozu" tutarlı

**Süre:** ~1.5-2 saat (paralel sekmelerle hızlanır)

---

### Adım 1.4 — Hand Anchor Atlas

🧑 **Sen:**
1. Aseprite'ta her anim sheet'i aç (sırayla)
2. Yeni layer ekle: "HandAnchor" (üstte)
3. Magenta renk seç (#FF00FF)
4. **Her frame'de elin dominant pixel'ine 1 pixel mor nokta koy** (Pencil tool, 1px)
   - Shadowblade dual blades → tek el için yeterli (sağ el)
5. Save (PNG export — HandAnchor layer dahil)

🤖 **Ben (rima-codex):**
- Python script yazdırırım: PNG'yi okur, her frame'in magenta pixel'ini bulur, JSON yazar
- Unity Editor script yazdırırım: JSON'u WeaponAnchorMap.asset ScriptableObject'e çevirir
- Magenta layer otomatik strip edilir (clean export)

✅ **Validation:** Her anim sheet için magenta dot atlas çıkıyor, WeaponAnchorMap.asset dosyaları üretildi

**Süre:** ~30-45 dk (her anim ~3-5 dk)

---

### Adım 1.5 — Weapon Sprite (Dagger)

🧑 **Sen:**
1. Aseprite'ta `shadowblade_south_clean.png` aç
2. Sadece dagger bölgesini seç (rectangle marquee)
3. Crop + 64×64 canvas'a padding ekle (dagger merkezde)
4. Save: `shadowblade_dagger_init.png`

🧑 **Sen (PixelLab Web App):**
1. Create Image S-XL (new) aç
2. Ayarlar:
   - Direction: **None**
   - View: **High top-down**
   - Detail: **Highly detailed**
   - Outline: **Single color outline**
   - Width × Height: **64 × 64**
   - Init image: `shadowblade_dagger_init.png` upload
   - Transparent background: ON
3. Description: (Ben vereceğim)
4. Generate → beğen → save: `shadowblade_dagger_v1.png`

🤖 **Ben:**
- Description prompt'u hazırlarım
- UnityMCP üzerinden Sprite Editor'da custom pivot'u kabza pixel'inde işaretlerim

✅ **Validation:** 64×64 dagger sprite, kabza pixel pivot olarak Sprite Editor'da işaretli

**Süre:** ~15-20 dk

---

### Adım 1.6 — Unity Entegrasyon

🤖 **Ben (UnityMCP + rima-codex):**
- Prefab oluştur: Shadowblade.prefab
  - Body SpriteRenderer (Animator clipleri)
  - WeaponPivot (Transform)
    - WeaponSpriteRenderer (dagger)
    - WeaponHitbox (BoxCollider3D, disabled)
- 7 Animation Clip oluştur (PNG sheet'lerden import)
- AttackProfile_ShadowbladeLMB.asset
  - RotationCurve: 0° → -90° (horizontal slash)
  - HitboxWindow: 0.2-0.4 normalizedTime
- AttackProfile_ShadowbladeRMB.asset (farklı saldırı)
- WeaponController.cs bağla
- 2 dagger instance (dual blades)

🧑 **Sen:**
1. Play tuşuna bas
2. WASD ile hareket et — body anim doğru mu?
3. LMB bas — attack anim + dagger savruluyor mu?
4. RMB bas — alternatif attack çalışıyor mu?
5. Mob (placeholder cube) yanına git, attack yap → hitbox damage tetikleniyor mu?

🤖 **Ben:**
- Hata varsa diagnose + Codex fix dispatch

✅ **Validation:** Karakter hareket eder, attack'lar çalışır, hitbox damage verir

**Süre:** ~45-60 dk

---

### FAZ 1 SONU

🤖 **Ben:**
- Pilot raporu yazarım: ne çalıştı, ne çalışmadı, time/credit gerçek vs tahmin
- Iyileştirme listesi (varsa)

🧑 **Sen:**
- Görsel inceleme: Shadowblade in-game iyi görünüyor mu?
- "OK Faz 2'ye geç" de

✅ **Validation:** Pilot PASS. Pipeline validated.

---

## FAZ 2: Diğer Sınıflar (Paralel)

**Pilot PASS olduktan sonra** Ranger / Elementalist / Warblade paralel başlar.

### Strateji

🧑 **Sen:**
- 3 sınıfı aynı gün veya farklı günlerde işle
- Her sınıfta Faz 1.1-1.6 sırasını takip et
- PixelLab credit'ini paralel sekme açarak hızlandır

🤖 **Ben:**
- Her sınıfın aşama promptlarını paralel hazırlarım
- QC her aşama sonrası
- Unity integration her sınıfın 1.6'sında

### Sınıf-Özel Notlar

**Ranger:**
- Bow ayrı sprite (128×128)
- Arrow projectile ayrı prefab (16×16, Unity'de spawn)
- Quiver dekoratif (sırtta, body sprite'a dahil)

**Elementalist:**
- Silah YOK — orb = Unity Particle System
- Aseprite'ta manuel orb silme (birkaç pixel)
- Adım 1.5 yerine: Particle System spec (color, behavior)

**Warblade:**
- Weapon Removal: Create Character Pro New + style image (Edit Image Pro yetersiz dev kılıç için)
- Greatsword 256×256 canvas (192 yok, 256 padding transparent)
- AttackProfile: longer windup, larger hitbox

### Tahmini Süre (3 sınıf paralel)

- Tüm 3 sınıf: **~6-8 saat aktif sen, ~10-12 saat arka plan ben**

---

## Referans Kartları

### A) PixelLab Web App Ayarları (Hızlı Lookup)

**Custom Animation V3 (karakter anim):**
- Start Frame: clean anchor
- Frame Count: 4-8 (çift sayı)
- Keep First Frame: ON (idle/walk/hurt), OFF (death)
- 8 yön ZORUNLU (3 üretilir, 5 v2/v3'e — flip için)

**Edit Image Pro (weapon removal):**
- Mevcut anchor yükle
- Mask: silah bölgesi
- Prompt: "remove [weapon], show empty hands"
- Suffix: PRESERVE EXACTLY (yukarıda)

**Create Image S-XL (new) (silah sprite):**
- Direction: None
- View: High top-down
- Detail: Highly detailed
- Outline: Single color outline
- Init image: anchor'dan crop
- Transparent background: ON

### B) Aseprite Hızlı İşler

**Face Cleanup:**
- Layer: FaceMask (siyah, yüz bölgesi)
- PixelLab Extension > Inpaint
- Prompt: Adım 1.1'deki şablon

**Magenta Dot (Hand Anchor):**
- Layer: HandAnchor (üstte)
- Color: #FF00FF
- Pencil 1px, her frame'de dominant el pixel'i

**Weapon Init Crop:**
- Selection: weapon bölgesi
- Crop → New canvas (target size)
- Padding ile silah merkezde

### C) Unity Sprite Editor Custom Pivot

1. Sprite seç → Inspector → Sprite Editor
2. Pivot dropdown: **Custom**
3. Mode: **Pixels**
4. X / Y: silahın grip pixel koordinatı
5. Apply

### D) Hatalı Olabilecek Şeyler

| Sorun | Sebep | Çözüm |
|---|---|---|
| Pixel shimmer hareket sırasında | Position snap yok | Codex fix: LateUpdate'te `position = round(pos * 64) / 64` |
| Silah body önünde değil arkasında | sortingOrder yanlış | WeaponController'da direction-based sortingOrder check |
| Attack hitbox vurmuyor | Layer mask yanlış / window timing | AttackProfile.hitboxStart/End check |
| Anim frame index out of bounds | normalizedTime > 1 (loop) | `phase = normalizedTime % 1f` modulo |
| Particle orb sönük | URP additive shader yok | Custom Particle/Additive material |

---

## Hata Yaparsam Ne Olur

Korkma. Hepsi reversible:
- Aseprite save: ayrı dosya, eski versiyon kayıp değil
- PixelLab generate: credit harcanır ama eski sonuç hala var
- Unity prefab: Ctrl+Z var, ya da git revert
- Pipeline tıkanırsa: Claude root cause analiz eder, fix dispatch eder

**Bana her zaman sorabilirsin:** "Bu adımda takıldım", "Bu ne demek?", "Sırayı değiştirelim mi?"

---

## Sonraki Adım

POC GATE başla → Unity'de yeni URP 3D proje aç → bana haber ver.
