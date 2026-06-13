# RIMA Build Mode — Konsey Kararı (Yalın/Acımasız Lens)

**Net Duruş:** Sunuma 1 hafta kala **REF1** ve **REF2**'deki dünya-uzayında dokulu, organik fırçalı zemin harmanlama (terrain blending) tekniklerine girişmek **net bir intihardır ve aşırı-mühendislik (over-engineering) tuzağıdır.** RIMA oda bazlı (room-based) bir ARPG'dir; devasa arazileri olan bir RTS değildir. Jürinin görmek istediği şey kusursuz zemin harmanlama shader'ları değil, Build Mode'un mekanik olarak **çalışması**, odanın tasarlanabilmesi ve kaydedilip oynanabilmesidir.

Aşağıdaki tablo, 1 haftalık kısıtlı sürede riskleri minimize edip demo değerini maksimize etmek için hazırlanan acımasız öncelik listesidir:

---

## 🛠️ AL / POST-DEMO / ATLA Karar Tablosu

| Özellik / Teknik | Durum | Yalın Gerekçe (Aşırı-Mühendislik Eleştirisi) |
| :--- | :---: | :--- |
| **Grid-Based Validity Ghost & Rotate** (P2) | **AL** | Oyuncunun neyi nereye koyabileceğini (kırmızı/yeşil ghost) anında görmesi temel oyun hissini (game feel) oluşturur. Kaçınılmazdır. |
| **Prop Palette & Placement** (P2) | **AL** | Zindana engel, varil, sandık yerleştirmek Build Mode'un varoluş amacıdır. En yüksek demo değerine sahiptir. |
| **URP 2D Light Placement** (P4) | **AL** | Odaya meşale/ışık koyulduğunda karanlık odanın anında aydınlanması en ucuz ama en yüksek **"WOW"** efektini yaratır. |
| **Runtime Save/Load** (P4) | **AL** | Tasarlanan odayı kaydedip hemen Play Mode'da test edebilmek projenin teknik başarısının kanıtıdır. |
| **Basit Single-Layer Tile Paint** (Grid API) | **AL (Nice-to-Have)** | Sadece Unity'nin `Grid API`'sini kullanarak (`SetTile`) tıklanan hücreye zemin döşeme. Yazması 20 satır sürer, sıfır risklidir. |
| **Organic Circular Terrain Brush** (REF1/REF2) | **ATLA** | Zemin sınırlarını organik fırçayla yumuşatma ARPG için gereksiz bir efordur. Kare grid yapısı (retro pixel-art) RIMA için zaten doğal olanıdır. |
| **Custom Low-GameObject Terrain System** | **ATLA** | Unity'nin yerleşik `Tilemap` sistemi zaten mesh'leri chunk'lar halinde birleştirir. Olmayan bir performans problemini çözmeye çalışmaktır. |
| **World-Space Blend Shader & Splatmap** | **ATLA** | URP 2D ile custom pixel-art harmanlama shader'ları yazmak, import ayarlarıyla boğuşmak ve 1 haftada görsel tutarlılığı bozmak demektir. |
| **Build Mode Undo/Redo** (P2) | **POST-DEMO** | Sunum anında tek yönlü düzgün çalışan bir akış yeterlidir. Hata yapınca silme (LMB/RMB) mekaniği varsa Undo'ya gerek kalmaz. |
| **Selection / Move Mode** (P5) | **POST-DEMO** | Yanlış koyulan prop'u seçip taşımak yerine silip yeniden koymak (LMB/RMB) daha kolaydır. Efor tasarrufu sağlar. |

---

## 🔍 Yalın Gerekçeler ve Sorularınızın Yanıtları

1. **Terrain Teknikleri Gerekli mi?** 
   * **Hayır, tamamen gereksiz.** RIMA 64px pixel-art kare tile'lar kullanan iso-staggered bir ızgara yapısına sahiptir. Grid bazlı retro ARPG'lerde sert kenarlı zemin geçişleri zaten beklenen ve estetik duran bir yapıdır. REF1/REF2 teknikleri bu projenin görsel kimliğini bulandırır ve odak kaymasına yol açar.
2. **Az-GameObject Terrain RIMA Ölçeğinde Çözülmüş bir Problem mi?**
   * **Evet, çözülmüştür.** Unity'nin yerleşik `TilemapRenderer`'ı zaten chunking kullanarak mesh'leri batch'ler halinde çizer. Oda bazlı bir ARPG'de elle chunking sistemi yazmaya çalışmak zaman kaybıdır.
3. **Wow Faktörü Nerede?**
   * Jürinin gözünü boyayacak şey **prop yerleştirme**, yerleştirirken çıkan **validity ghost görsel geribildirimi**, odaya konulan **2D dinamik ışıklar** ve en önemlisi **Save/Load çalıştırıp** odada düşmanlarla savaşabilmektir. Fırça ile zemin boyamak "görünmez altyapı" sınıfına girer ve efor/demo-değeri oranı çok düşüktür.
4. **En Ucuz "Nice-to-Have" Alternatif Nedir?**
   * Mevcut [BuildModeController.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Build/BuildModeController.cs) (ya da ilgili build controller) yapısını bozmadan, sadece `Grid` üzerinden fare koordinatını okuyup `tilemap.SetTile(cellPos, selectedTile)` çağıran **basit bir zemin boyama fırçası**. Shader ve matematik içermez, tamamen güvenlidir. Ek olarak, validity ghost için yumuşak bir outline veya pulse animasyonu içeren basit bir Shader Graph jüriye çok daha premium hissettirecektir.

Bu doğrultuda, [CURRENT_STATUS.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/CURRENT_STATUS.md) dosyasındaki mevcut planı bozmadan **P2 (Ghost, Rotate, Palette) -> P4 (Light, Save/Load)** akışına odaklanılmalı; P3'teki "fırça" aşaması en basit düz zemin boyama seviyesine indirgenerek geçilmelidir.

