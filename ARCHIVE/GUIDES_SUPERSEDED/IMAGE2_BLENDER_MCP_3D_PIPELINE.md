# IMAGE2 + BLENDER MCP 3D PRODUCTION PIPELINE
> Gelecekteki projeler için tekrar kullanılabilir üretim rehberi  
> Versiyon: 1.0  
> Dil: Türkçe

---

## 1) Kısa Cevap

### 3D daha smooth olur mu?
Evet, **doğru kurulumla** genelde daha smooth olur.

Neden:
- Skeletal animation gerçek zamanlı interpolasyon alır.
- Blend tree geçişleri 2D frame swap’a göre daha yumuşaktır.
- Aynı rig üstünde idle/run/attack geçişi daha stabil olur.

Ama otomatik değil:
- Kötü rig, kötü weight paint, düşük FPS, kötü transition ayarı varsa 3D de sert görünür.

Kural:
- `3D = daha yüksek smooth potansiyeli`
- `2D = daha düşük teknik karmaşa`
- `Hybrid (3D üret -> sprite bake) = 2D görünüm + 3D tutarlılık`

---

## 2) Bu Pipeline Ne İşe Yarar?

Bu pipeline ile:
- `GPT Image 2` ile konsept, materyal, decal, texture varyasyonları üretirsin.
- `Blender + MCP` ile modelleme/temizlik/UV/bake/export sürecini hızlandırırsın.
- Hazır assetlerden başlayıp özgün görsel kimlik çıkarırsın.

Hedef:
- Hızlı prototip + üretilebilir kalite + stil tutarlılığı.

---

## 3) Araç Rolleri (Net Ayrım)

### GPT Image 2
- Konsept sheet (front/side/back/3-4)
- Stil varyasyonları
- Materyal referansları
- Decal/trim pattern fikirleri
- Icon, UI, splash destekleri

### Blender MCP
- Tekrarlı teknik adımları otomasyon:
  - import
  - transform/scale normalization
  - batch rename
  - mesh cleanup
  - UV pack
  - bake set çalıştırma
  - export

### Hazır Asset Kaynakları
- Base mesh, ekipman, prop, environment kit.
- Süreyi çok kısaltır.
- Özgünlük için mutasyon şart (siluet + materyal + kitbash).

---

## 4) Üretim Stratejisi (Önerilen)

### En güvenli yol: "Asset-first + AI-style + Blender mutate"
1. Hazır asset ile başla (lisans temiz).
2. Image 2 ile hedef stil ve karakter kimliği kilitle.
3. Blender’da siluet ve prop yerleşimi değiştir.
4. Texture/decal pass ile özgünleştir.
5. Rig/anim ile oyun içi test et.

Bu yol, tamamen sıfırdan modellemeye göre daha hızlı ve daha güvenli.

---

## 5) Pre-Production (Üretime Girmeden Önce)

## 5.1 Stil Kiti (zorunlu)
- `Style Anchor Board`: 6-12 referans görsel
- `Material Board`: metal/deri/taş/kumaş örnekleri
- `Shape Language`: keskin mi yuvarlak mı?
- `Renk kuralı`: ana + vurgu + efekt paleti

## 5.2 Teknik Bütçe
- Hedef platform: PC / Mobil / Console
- Karakter tri budget
- Texture budget (1K/2K/4K)
- Draw call hedefi
- Shader sınırı

## 5.3 Rig Standardı
- Humanoid mi custom mı?
- Root bone standardı
- Silah socket naming standardı

---

## 6) Image 2 Aşaması (Konseptten Üretime)

## 6.1 Üretilecek minimum set
- Character sheet:
  - front
  - side
  - back
  - 3/4
- Weapon close-up
- Material breakdown (metal/leather/cloth)
- FX mood frame (glow/smoke/runes)

## 6.2 Prompt yapısı (template)
`[character archetype], [silhouette keywords], [material keywords], [mood], orthographic character sheet, game-ready concept, clean readable shapes`

## 6.3 Tutarlılık kuralı
- Her yeni üretimde:
  - aynı karakter adı
  - aynı silhouette anahtar kelimeleri
  - aynı material tanımları
- Rastgele sapmayı azalt.

---

## 7) Hazır Asset Seçimi (Kalite + Lisans)

## 7.1 Teknik filtre
- Temiz topology
- Mantıklı UV
- Riglenebilir mesh
- Non-overlapping UV (gerekiyorsa)

## 7.2 Lisans filtre
- Commercial use açık mı?
- Türev eser izni var mı?
- Redistribute kısıtları var mı?

## 7.3 Red flag
- Aşırı triangulated low-quality mesh
- UV karması
- Belirsiz lisans

---

## 8) Blender MCP Üretim Akışı (Adım Adım)

## 8.1 Import + normalize
- Birimleri standardize et.
- Pivot/origin düzelt.
- Forward/up axis tutarlı olsun.
- Ölçekleri freeze/apply et.

## 8.2 Kitbash + siluet mutasyonu
- Hazır parçaları birleştir.
- En az 3 ana siluet değişikliği yap:
  - omuz formu
  - baş/kapüşon formu
  - silah oranı

## 8.3 Mesh cleanup
- Merge by distance
- Normal fix
- Non-manifold kontrol
- N-gon temizliği (gerekliyse)

## 8.4 UV ve bake hazırlık
- UV island düzeni
- Texel density eşitle
- Bake cage kontrolü

## 8.5 Bake
- Normal
- AO
- Curvature (opsiyonel)
- ID map (material mask için)

## 8.6 Export
- Oyun motoruna göre:
  - FBX veya GLB
- Naming standardı ile export.

---

## 9) Özgünlük Katmanı (Asıl Fark Burada)

Hazır assetin “hazır asset gibi görünmesini” engellemek için:

1. Siluet değişikliği (zorunlu)
2. Özel decal seti (Image 2 ile üret)
3. Özel trim sheet (Image 2 + manual cleanup)
4. Renk/roughness standardı (proje style bible’a kilitli)
5. FX kimliği (ör. sadece bu class’a özel glow pattern)

Kural:
- Sadece renk değiştirip geçme.
- Form + materyal + motif üçlüsünü birlikte değiştir.

---

## 10) Rig ve Animasyon (Smoothluk Çekirdeği)

## 10.1 Rig kalite checklist
- Root bone temiz
- IK/FK setup (gerekiyorsa)
- El/ayak pole vector stabil
- Weapon grip pose standard

## 10.2 Animasyon geçiş kuralı
- Idle -> Run transition kısa ama sert değil
- Run start / run stop ayrı klip varsa çok daha yumuşak olur
- Attack cancel pencereleri gameplay’e göre ayarlanmalı

## 10.3 Smoothluk için kritik parametreler
- Anim sample rate
- Transition duration
- Foot sliding kontrolü
- Root motion kararının net olması

---

## 11) Engine Entegrasyonu (Unity/Unreal Fark Etmez)

## 11.1 Import standardı
- Uniform scale
- Rig avatar mapping
- Material reassignment

## 11.2 Animator/State Machine
- Parametreler:
  - Speed
  - DirectionX / DirectionY
  - Attack state
  - IsDash
- Transition koşulları net ve test edilebilir olmalı.

## 11.3 LOD ve performans
- LOD0/1/2 stratejisi
- Uzakta shader sadeleşmesi
- Overdraw ve draw call kontrolü

---

## 12) QA Gate (Her Asset Geçmeden Önce)

## GATE A — Görsel
- Siluet net mi?
- Class kimliği okunuyor mu?
- Stil bible ile uyumlu mu?

## GATE B — Teknik
- Naming doğru mu?
- Transform/scale temiz mi?
- UV/bake artefact var mı?

## GATE C — Oyun içi
- Idle/run/attack geçişleri smooth mu?
- Kamera açısından model okunuyor mu?
- FPS düşüşü var mı?

---

## 13) Haftalık Üretim Planı (Örnek)

### Gün 1
- Image 2 konsept sheet + style lock

### Gün 2
- Hazır asset seçimi + kitbash base

### Gün 3
- Blender cleanup + UV + bake

### Gün 4
- Rig + temel animasyon (idle/run/attack)

### Gün 5
- Engine entegrasyon + QA + düzeltme

---

## 14) Dosya ve İsimlendirme Standardı

Örnek:
- `Characters/KnightA/model/KnightA_LOD0.fbx`
- `Characters/KnightA/textures/KnightA_BC.png`
- `Characters/KnightA/textures/KnightA_N.png`
- `Characters/KnightA/anim/KnightA_Run.fbx`
- `Characters/KnightA/meta/KnightA_license.txt`

Kural:
- Assete bakınca kim, ne, hangi sürüm anlaşılsın.

---

## 15) En Sık Yapılan Hatalar

1. Sadece texture değiştirip “özgün oldu” sanmak.
2. Rig’i geç kurup sonradan mesh’i bozmak.
3. LOD/performansı en sona bırakmak.
4. Kamera açısından okumayı test etmemek.
5. Lisans metadatasını saklamamak.

---

## 16) Karar Çerçevesi: 2D mi 3D mi?

3D seç:
- Uzun vadede çok class + çok animasyon olacaksa
- Blend smoothluğu kritikse
- VFX/skin varyasyonları artacaksa

2D seç:
- Kısa scope, düşük teknik risk isteniyorsa
- Ekipte 3D rig/anim tecrübesi zayıfsa

Hybrid seç:
- 2D görünüm istiyorsun ama anim tutarlılığı 3D’den gelsin istiyorsan

---

## 17) Final Öneri

Senin tipinde aksiyon oyunlarında en dengeli model:
- `3D üretim pipeline` (Image 2 + Blender MCP + asset mutate)
- Gerekirse görsel sunumda stylized/painted look

Bu sayede:
- Smoothluk artar,
- Üretim tekrar edilebilir hale gelir,
- Aynı kaliteyi yeni classlara daha hızlı taşırsın.

