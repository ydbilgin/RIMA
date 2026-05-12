# Alabaster Dawn — Polish Reference for RIMA
**2026-05-12 | Araştırma notu**

## Kaynak Oyun
- **Alabaster Dawn** — Radical Fish Games (CrossCode geliştiricileri)
- Early Access: 7 Mayıs 2026 | Steam: ~%96 Overwhelmingly Positive
- Engine: Custom TypeScript/WebGL — gerçek 3D geometry + üstüne pixel art sprite projection
- Yani: efektleri fake değil, gerçek 3D'den geliyor. RIMA'ya adapte = efekti taklit et, mekanizmayı değil.

---

## 1. Yükseklik / Fake-3D Derinlik (EN ÖNEMLİ)

### Alabaster Dawn'da ne yapıyor:
Karakter gerçekten 3D ortamda hareket ediyor, yükseklik bilgisi var. Yükseldikçe/alçaldıkça shadow sprite büyüklüğü ve offset değişiyor. Yüksekten düşme animasyonu (karakter yukarıda küçük, düşerken büyüyen shadow) + iniş tozu efekti var.

### RIMA adaptasyonu (Unity URP 2D):

**A) Blob Shadow (Drop Shadow)**
- Karakterin altına ayrı bir `shadow_sprite` (yumuşak daire, %30-40 opacity, additive değil multiply blend)
- Normal zemin: shadow tam altında, scale ~0.6-0.7
- Yüksekten inerken: shadow yere projeksiyon gibi davranır — karakter yukarıdayken shadow daha büyük + daha az opak
- Implementation: `ShadowController.cs` — karakter Y offset'ine göre shadow alpha ve scale lerp

**B) Yüksekten İniş Efekti (Priority: HIGH)**
- Karakter bir ledge/platform'dan aşağı inerken:
  1. Küçük "whoosh" trail (Trail Renderer, 0.05s, additive, şeffaf)
  2. İniş anında: landing dust burst (Particle System, 6-8 pixel, fan out, 0.15s)
  3. Kamera hafif shake (0.05s, 1-2px magnitude) — küçük düşmede çok hafif
  4. Karakterde 1-2 frame squash (scale Y = 0.85, hızla 1.0'a dön)
- **RIMA context:** Rift crack alanlarından aşağı inerken, zımba efektli saldırılarda bu kullanılabilir

**C) Yukarı Çıkma Efekti**
- Ledge'e tırmanma veya zıplama:
  1. Hafif stretch (scale Y = 1.1, hızla 1.0'a dön)
  2. Ayakların altında küçük toz puff (ayrılış tozu)
  3. Eğer yükseğe çıkıyorsa: shadow küçülür + soluklaşır

**D) Y-Sort + Z-Layering**
- Tüm sprite'lar pivot noktası altına göre Y-sort (Unity Tilemap Renderer: Individual tiles, sorting by Y)
- Yüksek platform objeleri ayrı sorting layer: "Elevated"
- Karakterin platform arkasına geçmesi: sprite masking veya sorting layer geçişi

---

## 2. Combat Juice / Savaş Hissi

### Hit-Stop
- Ağır vuruşta `Time.timeScale = 0` için **2-4 frame** (0.033-0.066s at 60fps)
- Hafif vuruşta: 1 frame yeterli, bazı durumlarda hiç yok
- Hit-stop sırasında particle'lar ve trail'lar devam etsin (unscaled time kullan)

### Hasar Sayıları (Damage Numbers)
- TextMeshPro world-space, spawn on hit
- Initial velocity: vuruş yönüne göre yukarı + hafif random X
- Gravity scale: hafif aşağı çekiş
- Renk kodu: normal=beyaz, crit=sarı/turuncu, heal=yeşil, elemental=element rengine göre
- Scale: hasar miktarına göre — 1-50 arası normal, 50+ büyük, crit her zaman büyük
- 0.6-0.8s sonra fade out + hafif yukarı hareket

### Enerji İmzaları / Hit Sparks
- Her sınıfın farklı renk hit spark paketi (Warblade=kırmızı, Hexer=mor, vb.)
- Spark yönü: gelen saldırı yönünün tersi (knockback yönü ile aynı)
- 4-6 pixel spark, 0.1-0.15s ömür

---

## 3. Hareket Smoothness

### Dash Speed Lines
- Trail Renderer: karakter üstünde, time=0.08-0.1s, additive blend, beyaz/şeffaf
- Dash bitince trail hemen kill (veya fade out)
- Afterimage alternatifi: dasht sırasında 2-3 ghost sprite spawn, %30-50 opacity, hızla fade

### Acceleration / Deceleration
- Anlık hız yerine lerp ile hız geçişi (`Vector2.Lerp` veya `MoveTowards`)
- Duruş: 3-5 frame içinde yavaşla (kaymayı simüle et — çok az, Hades gibi)
- Yön değişimi: küçük "pivot" animasyon frame'i

### Coyote Time
- Platform kenarından düştükten sonra ~0.1-0.15s daha zıplama/dash izni
- RIMA'da platform kenarı jump varsa mutlak ekle

---

## 4. Ortam Derinliği (Parallax)

### Katman Yapısı (3-4 katman yeterli)
| Katman | Hız Çarpanı | İçerik |
|--------|-------------|--------|
| Background far | 0.1-0.15x | Sis, uzak mimari siluet |
| Background mid | 0.3x | Sütunlar, arka duvarlar |
| Background near | 0.6x | Yakın duvar detayları, torçlar |
| Foreground | 1.0x (normal) | Zemin, karakter, objeler |

- Parallax `MonoBehaviour`: `transform.position = startPos + (cameraPos - cameraStart) * speedMultiplier`
- Dikey parallax da ekle (sadece yatay değil) — daha zengin his

### Işık ve Gölge
- Point light 2D: ateş, meşale, enerji kristalleri (dinamik flickering amplitude)
- Karakter üstünde hafif ambient light boya (gerçekçilik için ışıktan etkilensin)
- Gölgeli köşeler: corner vignette sprite veya global volume post-process vignette

---

## 5. UI / HUD Polish

### Can Barı
- Hasar alırken: kırmızı bar hemen düşür, altındaki "white health" bar gecikmeli (~0.5s) azal (klasik Hollow Knight tarzı)
- Düşme sesi + hafif kamera shake (büyük hasar için)
- Crit aldığında: ekran köşeleri kırmızı flash (0.1s)

### Boss Can Barı
- Ekran altında segment bar — her segment bir faz
- Faz değişiminde: bar titreme + kısa hit-stop + segment yanma efekti

### Etkiyi Güçlendiren Küçük Detaylar
- Skill kullanımında: hafif kamera zoom out (scale 1.02x, 0.1s) sonra geri
- Kill anında: mini "radial burst" particle (8 yön, kısa, renk=düşman rengi)
- Level up: ekrana kısa glow + ses

---

## 6. RIMA Öncelik Listesi

| Özellik | Etki | Maliyet | Öncelik |
|---------|------|---------|---------|
| Blob shadow + yükseklik offset | Yüksek | Düşük | **P0** |
| Landing dust + squash/stretch | Yüksek | Düşük | **P0** |
| Damage numbers (TMPro physics) | Yüksek | Orta | **P0** |
| Hit-stop (2-4 frame) | Yüksek | Çok düşük | **P0** |
| Dash trail / afterimage | Orta | Düşük | P1 |
| Parallax (3-4 katman) | Orta | Orta | P1 |
| White health bar lag | Orta | Düşük | P1 |
| Renk kodlu hit sparks | Orta | Orta | P1 |
| Crit ekran kızarma | Düşük | Çok düşük | P2 |
| Skeletal rigging (chibi rig) | Yüksek | Çok Yüksek | P3 (Faz 2) |

---

## Notlar
- Blob shadow özelliği RIMA'nın top-down ~30-35° viewing angle'ına çok uygun — karakter yere "yapışık" hissi verir
- Hit-stop + landing dust kombini: en ucuz, en yüksek etki "game feel" geliştirmesi
- Alabaster Dawn'ın gerçek 3D engine'i kopyalanamaz ama efektlerin tamamı Unity'de sprite/particle ile simüle edilebilir
- Yükseklik sistemi: RIMA'da platform/rift kenarları varsa bu sistemi erkenden koy, sonradan eklemek zorlaşır
