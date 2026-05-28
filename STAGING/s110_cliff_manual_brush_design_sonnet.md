# S110 — Cliff manuel brush + Cliff/Parallax depth chooser design

**Agent:** general-purpose Sonnet sub-agent (design + planning, no code)
**Effort:** ~30 dakika (design spec doc)

---

## User talimatı (verbatim)

> "Daha önce tile çizip sildiğim yerlerde cliff var şu an aktif tile olmayan yerlerdeki cliffi sil"
> "Ben istediğim takdirde oralara serpiştiririm ve istediğim seviyede (layer cliff ve layer parallax arasına seçme imkanı koyarak derinlik için)"

## Karar yorumu

1. **Auto cliff system DEPRECATED.** Sahnedeki cliff temizlendi, `CliffAutoPlacer.enabled = false` (orchestrator yaptı). Auto-placement algoritması artık kullanılmıyor.
2. **Manuel cliff brush LIVE olacak.** Yeni Room Painter (Phase A Day 2+) içine entegre edilecek.
3. **Depth chooser:** Designer cliff sprite'ı paint ederken **layer seçebilmeli** — `Cliff` veya `Parallax` arasında. Yani aynı cliff sprite, farklı depth tier'larında kullanılabilir.

## Görev

`STAGING/CLIFF_MANUAL_BRUSH_DESIGN.md` yaz. 6 bölüm:

### Bölüm 1: Önceki sistem ile karşılaştırma
- Eski: `CliffAutoPlacer.cs` floor pattern'ından otomatik cliff üretir, kullanıcı override blacklist ile manuel silme yapar
- Yeni: Designer Room Painter'da manuel cliff brush ile **istediği yere** cliff koyar, Cliff veya Parallax layer arasında **seçim yapar**
- Avantajlar/dezavantajlar tablosu (auto vs manual)
- CliffAutoPlacer ne olur: legacy bırak (disabled), code silme — gelecekte geri eklenebilir

### Bölüm 2: Depth chooser UX
- Designer bir cliff sprite seçtiğinde palette'ten, üst toolbar veya context dropdown'da:
  - Layer A: `Cliff` (foreground depth, gameplay layer)
  - Layer B: `Parallax` (background depth, parallax velocity factor uygulanır)
- Aynı sprite iki farklı sahne sonucu üretir (Cliff yakın, Parallax uzak/yavaş)
- Klavye shortcut önerisi (örn `1`=Cliff, `2`=Parallax)
- Visual differentiation: brush ghost preview rengi layer'a göre değişir (Cliff cyan, Parallax purple)

### Bölüm 3: Cliff brush spec
- Tek tıkla bir cell'e cliff sprite koy (selected sprite from palette)
- Drag-paint: mouse'u sürükle, geçilen cell'lere paint
- Erase mode: sağ tık veya alt + tık → cliff sil
- Variant cycling: scroll wheel ile aynı sprite'ın farklı varyantları arasında geçiş (KitB_Cliff cliff_S, cliff_SE, cliff_SW vb 9 sprite var)
- Mevcut PainterSuite ColliderPainter pattern'i reuse (drag-mouse handler + ghost preview + undo group)

### Bölüm 4: Layer integration (RoomData)
- `RoomData.placements` listesi içinde `PlacementRecord.layer = RoomLayer.Cliff` veya `RoomLayer.Parallax`
- `RoomLayerData[Cliff].depthValue` ve `[Parallax].depthValue` — sort layer + Y-sort + parallax factor
- Parallax katmanına konan cliff sprite, runtime'da `ParallaxLayer` component'i alır (Package'tan), factor=`RoomLayerData[Parallax].depthValue * 0.1` (örnek formül)

### Bölüm 5: Migration path
- Mevcut sahne `PlayableArena_Test01.unity` — cliff şu an boş (temizlendi). Designer Room Painter ile yeniden cliff serpiştirecek.
- KitB_Cliff sprite'ları (9 adet) Room Painter Asset Palette'inde `Cliff` kategorisi altında otomatik görünecek (folder scan)
- CliffAutoPlacer GO sahne'de kalır (disabled) — istenirse Inspector'da Regenerate tıklatılabilir (acil bir test için)

### Bölüm 6: Phase A Day 2-3 etkisi
- **Day 2 (Asset Palette):** Cliff sprite'ları doğal şekilde palette'te görünür, ekstra iş yok.
- **Day 3 (SceneView placement):** Manuel cliff brush bu day'in dönüşümü — placement logic + layer selector burada implement edilir.
- **Day 4 (Layer system):** Cliff/Parallax depth chooser bu day'e bağlı. Cliff için `CliffYSortManager` integration var ise dokunma, manuel-paint placement Y-sort'tan etkilenmeli (drop edge sarkma).

## Çıktı

Inline rapor (Agent yanıtı, ≤300 kelime):
- 6 bölüm tamam mı
- Open question'lar (örn "Cliff brush sprite varyantı için manuel mi seçilir yoksa rotasyon mu (R tuşu) ile? — Codex/agy yorum istesem")
- En kritik 3 risk (örn "Cliff sprite layer Parallax'a konunca ParallaxLayer factor kim ayarlar, runtime instantiate eden script kim")

DESIGN ONLY — kod yazma. Sadece `STAGING/CLIFF_MANUAL_BRUSH_DESIGN.md` dosyası üret.
