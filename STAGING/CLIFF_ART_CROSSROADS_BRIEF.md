ACTIVE RULES: (1) think before answering (2) concrete/min-fluff (3) scope: this art/placement crossroads only (4) flag uncertain.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read: STAGING/CLIFF_DEPTH_SYNTHESIS_S114S5.md, Assets/Scripts/Environment/CliffAutoPlacer.cs + DirectionalCliffTile.cs, Assets/Sprites/Environment/KitB_Cliff/cliff_S.png. RESPOND INLINE.

# Amaç
Kullanıcı (keskin sanat gözü, cliff'i defalarca reddetti) net bir kavşakta: "mevcut cliff'lerle düzgün kullanarak çözebilir miyiz, yoksa tile-boyutuna uygun farklı çizdirmeli miyiz?" Karar + somut yol istiyorum.

# Setup (DEĞİŞMEZ)
Unity URP 2D, yüksek top-down 3/4 (~70-80°, Hades/Children of Morta). Isometric diamond tilemap, cellSize (1, 0.61, 1), PPU 64. Floating-island arena, abyss backdrop (tek nebula görseli). Marka cyan #00FFCC.

# Mevcut cliff
- Sprite `cliff_S`: **128×192 px (2 hücre geniş, 3 hücre yüksek), tall kaya sütunu/sarkan-chunk**, top-center pivot.
- Render: floor'un ALTINDA (Ground sorting) → floor üstüne taşan kısım OCCLUDED, sadece void'e sarkan kısım görünür.
- Yerleşim: floor cell'in S/SE/SW komşusu boşsa (exterior-void flood + monotonic-south open-drop) + iç-delik kenarları.

# İKİ SOMUT PROBLEM (kullanıcı kırmızı/sarı işaretledi)
1. **RED — çıkıntı/uzak-kenar "kule" fazlalığı:** tall-column sprite, adanın çıkıntılarında ve kameraya uzak (yukarı/yan) kenarlarında void üzerine sarkınca dik KULE gibi duruyor (bazıları tam, bazıları parça). Adanın ön/alt kenarındaki cliff'ler İYİ; uzak/çıkıntı olanlar fazlalık.
2. **YELLOW — iç-delik derinliği bu açıda okumuyor:** 1-2 hücrelik iç-deliğe cliff yüzü sarkıttık AMA 70-80° top-down'da iç-duvar yakın-lip tarafından örtülüyor → "derinlik" hissi yok, sadece koyu delik.

# SORULAR (her birine concrete cevap)
A. **Mevcut tall-column sprite ile** temiz, profesyonel bir ada-kenarı OKUNUR mu (yerleştirme kuralı + occlusion ile RED çözülerek)? Yoksa bu sprite şekli (tall standalone column) sürekli-kenar için yapısal olarak yanlış mı?
B. **Yeni tile-boyutlu edge art** (iso kenara hizalı KISA yüz + dış/iç köşe parçaları, Wang/dual-grid edge seti) gerekli mi? Eğer evet: kaç parça, hangi boyut (PixelLab opsiyonları: 64×64 / 128×128 / 256×256 / 344×192 / 512×288), iso-kenar açısına nasıl hizalanır?
C. **RED fix:** çıkıntı/uzak-kenar cliff'lerini kesmek için "ön-cephe" (kameraya bakan alt silüet) tespiti — exterior-void-flood + monotonic-south'a EK olarak hangi kural? (örn. cliff'in sarktığı void ekranın ALT-dış sınırına bağlı mı, yoksa çıkıntı-arası notch mu?)
D. **YELLOW fix:** steep top-down'da iç-delik derinliği nasıl OKUTULUR? (delikleri büyüt / küçük delik açma / "dark pit" + kısa iç-yüz / backdrop-through / vazgeç?) Endüstride yüzen-ada iç-deliği nasıl gösterilir?
E. **DEMO için karar:** mevcut+placement-fix mi (hızlı), yoksa yeni-edge-art mı (kaliteli ama PixelLab üretim)? En yüksek kalite/efor oranı hangisi?

# İSTENEN: madde-madde, RIMA bu hafta uygulanabilir, net tavsiye + en yüksek-impact ilk adım.
