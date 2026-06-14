# Parallax Builder — Preview Scrub DONE

**Date:** 2026-05-28 | **Agent:** Sonnet (orchestrator direct)

---

## Özet

Room Painter'da **Edit-mode real-time parallax preview** uygulandı. Play mode'a girmeden Room Painter inspector'ındaki "Parallax" foldout açıkken "Preview Pan" slider'larını sürünce sahnedeki tüm `ParallaxLayer` GO'ları derinlik etkisini canlı gösteriyor.

---

## Değiştirilen Dosyalar (sadece 2 dosya)

### 1. `Assets/Scripts/Background/ParallaxLayer.cs`
- `public static Vector2 EditorPreviewOffset` static field eklendi — editor slider'ın itmesi için.
- `LateUpdate()` `#if UNITY_EDITOR` bloğu ile iki ayrı metodağa ayrıldı:
  - `ApplyEditorPreview()` — Edit mode'da `EditorPreviewOffset`'i her layer'ın `factor`'ı ile çarparak `transform.position` uygular.
  - `ApplyRuntimeParallax()` — Play mode'da eski camera-delta davranışı (DEĞİŞMEDİ).
- `RecaptureOrigin()` güncellendi: Edit mode'da restore `transform.position = _layerStart` önce, sonra Capture → origin drift yok.
- `Capture()` güncellendi: `_captured = (target != null) || !Application.isPlaying` — Edit mode'da Camera.main null olsa bile capture ediliyor.

### 2. `Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs`
- `using RIMA.Background;` eklendi.
- `_previewX`, `_previewY` static state eklendi.
- `DrawPreviewScrub()` public static metod eklendi:
  - "Preview Pan (Edit Mode)" header (koyu mor arka plan, Hades Elysium stili).
  - Pan X + Pan Y slider (aralık: -12..+12 world units).
  - Slider değişince → `ParallaxLayer.EditorPreviewOffset` set → `RecaptureAllLayerOrigins()` → `SceneView.RepaintAll()`.
  - X ekseni için görsel cursor bar.
  - Offset koordinat etiketi + "PREVIEW ACTIVE" / "centered" durum etiketi (PREVIEW ACTIVE = cyan #00FFCC tonu).
  - "Reset" butonu (offset sıfırla + recapture).
- `RecaptureAllLayerOrigins()` private static: `FindObjectsByType<ParallaxLayer>` + her birine `RecaptureOrigin()`.
- `Draw(asset)` metodunun sonuna `DrawPreviewScrub()` çağrısı eklendi (asset.defaultLayer bağımsız, her zaman görünür).

---

## Compile Durumu

`mcp__UnityMCP__refresh_unity scope=all mode=force` → `0 error, 0 warning` ✅

---

## Manuel Verify Adımları

1. Unity'de `PlayableArena_Test01` sahnesini aç (ParallaxRig 6 child layer var).
2. `RIMA > Room Painter` menüsünden pencereyi aç.
3. Palette'ten herhangi bir asset seç → Inspector açılır.
4. Sağ panelde "Parallax" foldout'u aç.
5. **Pan X slider'ını sürün** → sahnedeki BG_Void/BG_Far/BG_Mid/BG_Near farklı hızda kayar (near daha hızlı, void çok yavaş).
6. Pan Y slider → dikey parallax.
7. "Reset" → tüm katmanlar orijinal pozisyona döner.
8. Play mode'a gir → eski runtime davranışı bozulmamış olmalı (Camera.main delta bazlı).

---

## Teknik Notlar

- Runtime kodu `#if UNITY_EDITOR` guard'ı ile tamamen izole edildi — build'e 0 byte overhead.
- Origin drift problemi çözüldü: `RecaptureOrigin()` her slider değişiminde transform'u `_layerStart`'a geri yükler, sonra yeni origin yakalar. Sürekli slider sürüşünde pozisyon birikimi oluşmuyor.
- `EditorPreviewOffset` static → tüm ParallaxLayer instance'ları aynı "sanal kamera" offset'ini görür. Bu kasıtlı — farklı depth factor'lar zaten per-layer ayrım sağlıyor.

---

## Kalan En Kritik 3 Görev (bkz. PARALLAX_BUILDER_STATE_AUDIT.md)

1. **SceneView camera delta hook** — Scene View'da alt+orta fare ile kamerayı kaydırınca da preview çalışsın.
2. **ParallaxRig authoring paneli** — Sahnedeki ParallaxRig child layer'larını listele, tier dropdown, visibility toggle.
3. **Factor-to-scene live link** — Inspector'da factor slider değişince sahnedeki ParallaxLayer component'in factor field'ı da güncellenir.
