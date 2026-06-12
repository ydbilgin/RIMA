# Council Q — LMB/RMB Slot Tasarım ve Üretim Kararı
## Lens: Deep Architecture + Game Design

BAĞLAM:
- RIMA 2D top-down roguelite, 8 class (Warblade + Elementalist açık, 6 kilitli)
- Skill bar mevcut durumu: sağda/altta, LMB/RMB slotları (0,1) kasıtlı boş — diğer skill slotları SkillBase ile besleniyor
- Act1 visual canon: slate #3A3D42 · void mor #3A1A4A · ember #E89020
- Referans oyunlar: Hades, Diablo 2/3, Children of Morta, Dead Cells

## Sorular

### Q1: ElectricLancer
"ElectricLancer" ismi koda girmiş ama kanonik 8-class listesinde yok (Warblade, Elementalist, Ranger, Shadowblade, Ronin, Gunslinger, Ravager, Brawler). Bu bir:
- Elementalist'in eski kod adı mı?
- Planlanan 9. class mı?
- Typo/drift mi?
Cevap NLM'de olabilir. Önemli çünkü LMB/RMB contract buna göre şekillenecek.

### Q2: UI Yerleşim Mimarisi
LMB/RMB ikonları için 3 opsiyon:
A) Mevcut skill bar'da kalır (slot 0,1), sadece görselleştirilir
B) Sol tarafa HP bar yanına taşınır (Diablo tarzı — "action buttons" ayrı)
C) Skill bar'ın başına gelir ama görsel olarak ayrışır (boyut farkı, farklı frame)

RIMA'nın top-down chibi kimliği + roguelite loop'u (class seçimi, hızlı okuma) göz önünde bulundurarak:
- Hangi yaklaşım en iyi UX/okunabilirlik sağlar?
- Sol HP bar + LMB/RMB grubu bir "class identity panel" oluşturabilir mi?
- Roguelite genre conventions'a göre doğru yaklaşım hangisi?

### Q3: İkon Üretim Kalitesi
PixelLab `create_1_direction_object` (32x32, dedicated pixel art tool) vs imagegen (genel image generation):
- 32x32 pixel art icon için hangisi daha tutarlı, temiz silhouette üretir?
- Mevcut RIMA ikon kütüphanesiyle (65+ ikon, 32x32 canon) stil tutarlılığı açısından hangisi?
- Her class için ayrı üretim mi, yoksa unified ikon seti mi (LMB=sword silhouette, RMB=radial burst)?

Kısa, actionable yanıt ver. Çakışma varsa flag et.
