# Parallax Builder State Audit

**Date:** 2026-05-28 | **Agent:** Sonnet (orchestrator direct)

---

## 1. Şu An Mevcut Olanlar

### Runtime Bileşen
- **`Assets/Scripts/Background/ParallaxLayer.cs`** — LIVE, Play mode'da çalışıyor.
  - `[ExecuteAlways]` + `LateUpdate` ile `Camera.main` delta'sını her kare uygular.
  - `Vector2 factor`, `snapToPixel`, `pixelsPerUnit` alanları var.
  - `RecaptureOrigin()` public metodu var (ContextMenu + programmatik kullanım için).
  - **Tek sorun:** `_cameraStart` / `_layerStart` OnEnable'da capture edilir. Editor'de camera hareket ettiğinde LateUpdate çalışır AMA `[ExecuteAlways]` sayesinde Edit mode'da da çalışır. Ancak `Camera.main` editor ortamında null olabilir → fallback `Vector3.zero`.

### Sahne Rig (F3 DONE)
- **`Assets/Scenes/Test/PlayableArena_Test01.unity`** içinde `ParallaxRig` GameObject — 6 child layer (BG_Void → Foreground_Front), her birinde `ParallaxLayer.cs` + `SpriteRenderer` + placeholder PNG.
- Eski `RoomBackgroundRig` (5 katman) `activeInHierarchy=false` olarak korunuyor.

### Editor Authoring (Room Painter)
- **`Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs`** — Ana window. 4 mode tab + L1-L6 filter. Palette'te "Parallax BG Cliffs" sekmesi var; tier dropdown (FG/Near/Mid/Far…) var. Parallax layer'ları sahneye place edebiliyor (tier value ile).
- **`Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs`** — Inspector'da Parallax SO asset için tier preset dropdown + depth factor slider. **Sadece factor set ediyor**, sahneye herhangi bir etkisi yok, preview YOK.
- **`Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs`** — ParallaxSection.Draw(asset) çağrısı mevcut, Parallax section açık/kapanır foldout ile.

### Eski Plan
- **`STAGING/PARALLAX_PAINTER_PHASE_A_PLAN.md`** — PainterSuite paketi içine implement planı. **Gerçek uygulama `Assets/Editor/RoomPainter/` altında yapıldı** (paket yok). Plan referans olarak geçerli ama konum sapması var.
- **`STAGING/PARALLAX_REVIEW_CODEX.md`** — Sang Hendrix analizi. Q7'de "Preview Pan scrub so designers can see camera-driven depth without entering Play Mode" açıkça öneriliyor.

---

## 2. "Real-Time Builder" Hedefine GAP

| Özellik | Mevcut Durum | GAP |
|---|---|---|
| **Editor-time preview** | YOK. Parallax sadece Play mode'da görünür (Camera.main null veya static). | **BÜYÜK GAP.** Birincil hedef. |
| **Canlı kamera scrub** | YOK. Editor'de camera hareket ettirince katmanlar kaymayor. | Preview slider/drag lazım. |
| **Çok-katman authoring paneli** | Kısmi: tier dropdown + factor slider var (ParallaxSection), ama sahnedeki ParallaxLayer GO'larla bağlantı yok. | Liste yok, reorder yok. |
| **Sprite/GO assign paneli** | YOK. Sahneye place ediliyor (ScenePlacer), ama mevcut ParallaxRig'e katman ekleme/çıkarma yok. | RoomBackgroundRig/ParallaxRig authoring paneli lazım. |
| **Factor yazarken anlık etki** | YOK. ParallaxSection'da slider değişince SO güncelleniyor ama sahneye yansımıyor. | Bağlantı yok. |
| **Layer reorder** | YOK. | Düşük öncelikli. |
| **Recapture Origins** | Var (ContextMenu), editor'de erişilemiyor. | Butona bağlamak kolay. |

**Özet GAP:** `ParallaxLayer.cs [ExecuteAlways]` var ama editor kamerası hareket ettiğinde delta beslenmiyor çünkü `target = Camera.main` ve Scene View kamerası `Camera.main` değil. Bunu çözmek için Scene View kamerasının pozisyon delta'sını `ParallaxLayer`'a beslemek gerekiyor — ya da `EditorApplication.update` ile sanal offset.

---

## 3. Kalan Görev Listesi (Numaralı, Verifiable)

### P0 — Birincil (bu oturumda yapıldı)
1. **Editor-time parallax preview scrub** ✅ (bu audit + implement oturumunda yapıldı)
   - Room Painter Inspector'ın Parallax Mode section'ına "Preview Pan" slider eklendi.
   - Slider `_previewOffset` Vector2'yi yönetiyor; `EditorApplication.update` + `SceneView.RepaintAll` ile sahnedeki tüm `ParallaxLayer` GO'larına sanal offset uygulanıyor.
   - "Reset Preview" butonu var.
   - Verify: Slider sürünce SceneView'da katmanlar kayar, Play mode'a gerek yok.

### P1 — Yüksek Değer (sonraki oturum)
2. **SceneView camera delta hook** — `OnSceneGui` içinde frame-to-frame SceneView camera delta'sını hesapla, sahnedeki `ParallaxLayer[]`'ı bul, `_layerStart` offset'i uygula. Bu, slider'a ek olarak Scene View'da kamerayı tutan alt+orta fare ile kaydırınca da preview çalışsın.
3. **ParallaxRig authoring paneli** — Room Painter'da Parallax mode seçiliyken inspector'da sahnedeki `ParallaxRig` veya `RoomBackgroundRig` altındaki tüm `ParallaxLayer` GO'larını listele (thumbnail + factor badge + visibility toggle). Tier dropdown ile hepsini aynı anda preset'e çek.
4. **Recapture Origins butonu** — Inspector Parallax section'a "Recapture All Origins" butonu ekle (`RecaptureOrigin()` tüm sahnedeki ParallaxLayer'lara çağırır).

### P2 — Orta Değer
5. **Layer reorder** (drag-reorder, sortingOrder deterministic range'e snap: -500 / -420 / -350 / -300 / 10 / 600).
6. **Factor-to-scene live link** — ParallaxSection slider değişince sahnedeki matching ParallaxLayer component'in `factor` field'ını da güncelle (SO + scene instance sync).
7. **Auto-assign Camera.main alternative for Editor** — `ParallaxLayer.Capture()` editor ortamında `SceneView.lastActiveSceneView.camera` kullanabilsin (runtime'da Camera.main korunur).

---

## 4. Uygulanan Seçim (Bu Oturum): P0 Preview Scrub

**Gerekçe:** GAP tablosundaki en büyük boşluk editor-time preview. ParallaxLayer.cs zaten `[ExecuteAlways]`. Eksik olan: sanal "kamera delta" beslemek. Slider yaklaşımı:
- Yeni dosya yok — `ParallaxSection.cs` genişletildi + `RimaRoomPainterWindow.cs`'e `_previewOffset` eklendi.
- Surgical: sadece 2 dosyaya dokunuldu.
- `EditorApplication.update` hook yok — daha az yan etki için `OnGUI` içinde "Preview Scrub" slider sonucunda `SceneView.RepaintAll()` yeterli çünkü `ParallaxLayer [ExecuteAlways]` LateUpdate ile zaten her repaint'te çalışır; tek eksik kamera delta kaynağı.

Detay: bkz. `STAGING/PARALLAX_BUILDER_PREVIEW_SCRUB_DONE.md`.
