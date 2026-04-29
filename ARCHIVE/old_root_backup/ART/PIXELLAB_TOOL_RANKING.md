# PIXELLAB_TOOL_RANKING.md
> **Ne zaman yükle:** Hangi aracı kullanacağına karar verirken.
> Kaynak: PixelLab resmi docs + MCP araç tanımları (2026-04-02)

---

## RIMA İÇİN ARAÇ ÖNCELİK SIRASI

### 1. `create_character` (MCP) — ★★★★★ ANA ARAÇ
**Kullanım:** Tüm karakter ve düşman BASE üretimi
**Risk:** Düşük (prototype), orta (pro — seed yok)
**Prototype:** ✅ standard mode, hızlı
**Final:** ✅ pro mode, 8 yön tutarlı
**Cleanup ihtimali:** Orta — palette dağılabilir, outline artifact olabilir
**Not:** character_id'yi kaybet = karakteri kaybet. ID her zaman ASSET_LOG'a yazılır.

---

### 2. `animate_character` template (MCP) — ★★★★★ ANİMASYON STANDARDI
**Kullanım:** walk, idle, run, death, attack — tüm standart animasyonlar
**Maliyet:** 1 gen/yön → çok ucuz
**Risk:** Düşük
**Prototype:** ✅
**Final:** ✅ (titreme kontrolü yap)
**Cleanup ihtimali:** Düşük-orta — bazen frame timing Aseprite'ta ayarlanır
**47 template var** — STYLE_BIBLE.md'de seçilen 6 template yeterli

---

### 3. `create_tiles_pro` (MCP) — ★★★★☆ TİLE STANDARDI
**Kullanım:** Floor, wall, prop, skill icon
**Risk:** Düşük (seed ile reproducible)
**Prototype:** ✅
**Final:** ✅
**Cleanup ihtimali:** Orta — seamless test Aseprite'ta zorunlu
**Parametre:** `outline_mode: segmentation` daha temiz sonuç verir

---

### 4. `create_topdown_tileset` (MCP) — ★★★★☆ TERRAIN TRANSİSYON
**Kullanım:** Zemin-duvar geçişi, Wang tileset (16-23 tile)
**Risk:** Düşük
**Prototype:** ✅
**Final:** ✅
**Cleanup ihtimali:** Orta — edge kontrolü gerekir
**Güç:** base_tile_id zinciri ile act'ler arası tutarlılık

---

### 5. Rotate (Aseprite Extension) — ★★★☆☆ YARDIMCI ARAÇ
**Kullanım:** Mevcut sprite'ı farklı yöne çevir (örn: south → east)
**Canvas kısıtı:** 128x128, 64x64, 32x32, 16x16 — başka boyut kabul etmez
**Risk:** Orta — kalite tutarsız, her rotasyon farklı çıkabilir
**Prototype:** ✅ hızlı yön testi için
**Final:** ⚠️ create_character pro daha iyi — rotate yerine doğrudan 8 yön üret
**Cleanup ihtimali:** Yüksek — manuel silhouette düzeltme çoğu zaman gerekir
**Önerilen workflow:** Rotate → rough fix → sonraki yöne geç

---

### 6. Animate with Skeleton (Aseprite Extension) — ★★★☆☆ KONTROLLÜ ANİMASYON
**Kullanım:** Template animasyon yetmediğinde, spesifik hareket kontrolü
**Canvas kısıtı:** 256x256, 128x128, 64x64, 32x32, 16x16
**Risk:** Orta — iteratif, zaman alır
**Prototype:** ⚠️ yavaş, sadece kalite kritik aşamada
**Final:** ✅ en yüksek kontrol buradan
**Cleanup ihtimali:** Düşük (elle kontrol ediliyor)
**Tier:** Tier 1+ gerekli — Tier 2'de sorun yok
**Workflow:** template skeleton → freeze 1 frame → generate 2 → refine → güçlendir

---

### 7. Animation-to-Animation (Aseprite Extension) — ★★★☆☆ VARYASYON
**Kullanım:** Mevcut animasyondan yeni varyasyon üret
**Canvas kısıtı:** 128x128, 64x64, 32x32, 16x16
**Risk:** Orta-yüksek — kalite kayıt kalitesine bağlı
**Prototype:** ⚠️
**Final:** ⚠️ — dikkatli test et
**Tier:** **Tier 2 zorunlu**
**Cleanup ihtimali:** Yüksek — manuel cleanup bekleniyor (docs bunu açıkça söylüyor)
**Best use:** Elite tier renk/hareket varyantı için

---

### 8. `animate_character` custom (MCP) — ★★☆☆☆ PAHALIL ARAÇ
**Kullanım:** Template yoksa özel hareket
**Maliyet:** 20-40 gen/yön — çok pahalı
**Risk:** Yüksek (maliyet + kalite değişken)
**Prototype:** ❌ israf
**Final:** ⚠️ sadece template gerçekten yetmezse
**KURAL:** confirm_cost=true asla ilk çağrıda. Maliyet göster → onay al → çalıştır.

---

### 9. Edit / Inpaint (Aseprite Extension) — ★★☆☆☆ NOKTA DÜZELTİCİ
**Kullanım:** Belirli alan cleanup, artifact fix
**Risk:** Orta — yanlış alanı bozabilir
**Prototype:** ✅ hızlı patch
**Final:** ⚠️ dikkatli, BACKUP al
**Cleanup ihtimali:** Bu zaten cleanup aracı

---

### 10. Isometric / Map Tools — ★☆☆☆☆ RIMA İÇİN YOK
**RIMA low top-down** — isometric araçlar yanlış perspektif. Kullanma.

---

## ÖZET KARAR TABLOSU

| İş | Araç | Önce |
|---|---|---|
| Karakter BASE | `create_character` MCP | her zaman |
| Template anim | `animate_character` template MCP | her zaman |
| Tile / floor | `create_tiles_pro` MCP (seed ile) | her zaman |
| Terrain geçişi | `create_topdown_tileset` MCP | her zaman |
| Yön testi | Rotate (extension) | sadece test |
| Spesifik anim kontrolü | Skeleton (extension) | final kalite |
| Anim varyantı | Anim-to-anim (Tier 2) | deneme ile |
| VFX, UI, logo | Aseprite manuel | her zaman |
| Custom anim | `animate_character` custom | son çare |
