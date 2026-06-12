# Council Q — LMB/RMB Slot Tasarım ve Üretim Kararı
## Lens: Leanest Path + Over-engineering Critique

BAĞLAM:
- RIMA 2D top-down roguelite, skill bar'da LMB/RMB slot (0,1) kasıtlı boş
- Act1 visual canon: slate #3A3D42 · void mor #3A1A4A · ember #E89020
- Şu an sadece 2 class açık (Warblade + Elementalist), 6 kilitli

## Sorular — LEAN LENS

### Q1: Minimum viable LMB/RMB
Şu an LMB/RMB slotları boş çiziliyor. Kullanıcıya "ne basıyorum" bilgisini en az kodla göstermek için ne lazım?
- Sadece ikon eklemek yeterli mi (swap placeholder → real icon)?
- Mevcut slot sistem reuse edilebilir mi, ayrı UI elemanı mı gerekiyor?

### Q2: Sol tarafa taşıma — over-engineering mi?
HP bar + LMB/RMB sol panel önerisi: şimdi yapılması gereken mi, yoksa 2 class açıkken ertelenmeli mi?
- Şu anki mevcut skill bar pozisyonunda kalırsa ne kaybedilir?
- Taşıma işi büyük mü küçük mü — sprint bütçesine değer mi?

### Q3: İkon üretimi — en hızlı yol
4 ikon (Warblade LMB/RMB + Elementalist LMB/RMB) için:
- PixelLab `create_1_direction_object` mi, cx imagegen mi, yoksa mevcut 65 ikondan reuse mi?
- "Şimdi ship et" perspektifinden en hızlı kabul edilebilir çözüm ne?

Kısa, direkt, over-engineering'i eleştir. 3.1 Pro'nun derin analizini tamamla ama farklı ol.
