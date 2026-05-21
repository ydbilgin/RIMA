# ANTIGRAVITY PROMPT — Painter Iso Wall Holistic Fix

> **User yapıştırır**, Antigravity Gemini 3.5 Flash UnityMCP ile çalışır. Project intro yok, UnityMCP zaten okuyabiliyor.

---

```
TASK: Unified Painter wall paint iso görselinde "topdown" çıkıyor + yan yana wall'lar birleşmiyor. Holistic fix uygula.

ENV:
- Project root: F:/Antigravity Projeler/2d roguelite/RIMA
- Painter: Assets/Editor/RimaUnifiedPainterWindow.cs (1700+ satır, namespace RIMA.Editor.MapDesigner)
- Test sahne: Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity (24×18 iso cell, Karar #148 Grid localScale.y=0.819 squash uygulanmış)
- Wall pack KEEP: Assets/Prefabs/Walls/pilot_a/pilot_a_wall_{face_EW,corner_outer,arch_opening}.prefab (3 prefab)
- Wall pack ARCHIVED: face_NS top-down drift olduğu için 2nd-pass audit'te _archive/act1_2nd_drift_s95/walls/'a taşındı (kullanma)

UNITY GRID STATE (verify with UnityMCP):
- Grid component CellLayout=Isometric, CellSize=(1, 0.5, 1)
- Grid.transform.localScale=(1, 0.819, 1) ← Karar #148 squash
- Tilemap (Floor_Tilemap) parent altında
- Wall pieces canonical sortingLayer=Walls, sortingOrder=20, pivot=(0.5, 0.0313)

PROBLEM 1 — "TOPDOWN boyanıyor isometric değil":
- Codex recent fix (CODEX_DONE_painter_wang_degrade.md) face_NS YOKKEN NW/SE direction wall'lar için face_EW sprite'a 90° Z rotation uyguluyor (line 3612 isSingleFaceFallback).
- Iso billboard sprite Z rotation 90° ile yatırılınca top-down sprite gibi görünüyor (user feedback).
- Bu yanlış fallback. Z rotation iso wall için anlamlı değil.

PROBLEM 2 — Yan yana wall'lar birleşmiyor:
- Wang formülü (UpdateWallConnectionsAt line 3576+, ApplyWallConnectionFamily line 3678+) 4 piece bekliyor: face_NS / face_EW / corner / crack.
- face_NS archived, elimizde 3 piece. Codex graceful degrade fix uyguladı ama hâlâ NW/SE direction yanlış.
- Sonuç: yan yana wall'lar aynı sprite tekrarı, gerçek wall seam alignment yok.

PROBLEM 3 — Mevcut state (UnityMCP ile verify et):
- IsoShowcaseRoom_S95 sahnesinde Codex big v2 build wallar var (17 piece, N1-N9 + E1-E6 + S1-S2)
- Walls cy=17 row için cx=0,3,6,9,11,14,17,20,23 — 3 cell aralık (test scene build spec'ten)
- 0 orphan test paint kalmış olmalı (orchestrator UnityMCP ile temizledi)
- prefabScaleMultiplier default 1.0f (line 40, orchestrator fix uyguladı)

SCOPE:
1. RimaUnifiedPainterWindow.cs (1700+ satır) wall paint logic'ini derin incele:
   - PaintWallWithConnections (line 3525+)
   - UpdateWallConnectionsAt (line 3576+)
   - ApplyWallConnectionFamily (line 3678+)
   - GetPlacementOffset (line 2841+)
   - ComputeCompensatedLocalScale (line 2758+)
   - CalculateAutoYOffset
2. Wall cell→world coord conversion iso cellLayout aware mi? UnityMCP ile test et:
   - 3 farklı cell pattern paint et: (10,10), (11,10), (10,11), (11,11), (12,11)
   - World positions iso pattern verifying mi? (cell+1X → world(+0.5, +0.205), cell+1Y → world(-0.5, +0.205))
3. NW/SE direction wall placement 90° Z rotation YANLIŞ — iso billboard sprite yatırılıyor. ALTERNATIF FIX:
   - Option A: face_NS yokken NW/SE direction wall paint REDDET, user'a uyarı göster ("face_NS asset eksik, NW/SE wall placement disabled")
   - Option B: face_EW sprite'ı SpriteRenderer.flipX = true ile mirror et (Z rotation kullanma)
   - Option C: face_EW sprite'ı NW/SE pivot offset değişimi ile yerleştir (rotation YOK)
   - **EN İYİ:** kararını UnityMCP ile pre-test ederek belirle. Hangi opsiyon iso görsel tutarlılık verir, sahnede test paint görsel görerek seç.
4. Wall birleşim (adjacency snap):
   - Yan yana 2 wall arası world distance iso diamond pattern mi?
   - Wall sprite size 128px, iso billboard, pivot (0.5, 0.0313) — neighbor cell'le edge alignment mı?
   - Eğer sprite size 128px ve iso cell world unit ~1.41 ise wall'lar gap'lı veya overlap'lı kalır
   - Çözüm: sprite scale veya cell width adjustment? VEYA `GetPlacementOffset` cell-aware iso pivot offset hesabı.
5. autoConnectWalls behavior iso layout'a uyumlu mu? Wang16-style adjacency formülü iso cell coord'ları için doğru direction'ları algılıyor mu?
   - Iso cell (+1, 0) = SE direction, (-1, 0) = NW direction
   - Iso cell (0, +1) = NE direction, (0, -1) = SW direction
   - Current code (line 3584-3587):
     hasNE = (0, +1, 0)
     hasSW = (0, -1, 0)
     hasNW = (-1, 0, 0)
     hasSE = (+1, 0, 0)
   - Iso cell math ile uyumlu mu verify et
6. UnityMCP ile manual test:
   - IsoShowcaseRoom_S95 aç
   - Painter aç, Wall sekmesi, face_EW seç, auto-connect ON
   - 3 farklı pattern paint et: (10,10),(10,11),(10,12) — north column / (10,10),(11,10),(12,10) — east row / (10,10),(11,11),(12,12) — diagonal
   - World position rapor et, screenshot al, görsel inceleyip "topdown gibi mi iso gibi mi" karar ver
   - Eğer hâlâ topdown gibi → fix uygula, test tekrarla, iterate
7. Final fix uygulandıktan sonra:
   - Sahneyi temizle (test paint'leri sil)
   - read_console 0 error verify
   - Sahne save etme (orchestrator karar verecek)
   - Rapor yaz: STAGING/ANTIGRAVITY_DONE_painter_iso_holistic.md

CRITICAL CONSTRAINTS:
- face_NS asset üretilmesi başka bir batch (Batch A, kuyrukta) — şu an üretmiyoruz, painter logic'i 3-piece pack ile çalışmalı.
- Z rotation iso billboard'u yatırır — fix bu mantıkla devam etmemeli.
- prefabScaleMultiplier default 1.0f (orchestrator fix uygulandı, doğrulayabilirsin).
- Karar #148 squash Grid.localScale.y=0.819 — bu UYGULANIYOR, dokunma, painter compensate ETMELİ.
- Sahne 17 mevcut wall'ı var (Codex big v2 build), bunları KORUMA — sadece test paint yap, sonra cleanup.
- PathC_BaseTest.unity'ye dokunma.

EXPECTED OUTCOME:
- User painter ile yan yana 3 wall paint ettiğinde:
  - 3 wall iso pattern'da (diamond cell adjacency) yerleşir
  - Hiç top-down sprite görünüş yok (Z rotation 90° hile YOK)
  - Wall'lar arası seamless edge (gap/overlap yok)
  - Wang variation: tek wall (no neighbor), 2-neighbor straight, 3-neighbor T, 4-neighbor cross — visual coherent
  - Auto-connect ON çalışıyor
- Manual test 0 error console

Devam et iteratif:
1. Read painter code
2. UnityMCP ile state inspect
3. Hipotez oluştur
4. Test paint yap
5. Görsel sonuç değerlendir
6. Fix uygula
7. Tekrar test
8. PASS sonrası rapor

GO.
```

---

## Notlar (User için)

- Bu prompt **paste-ready** — Antigravity Gemini 3.5 Flash'a olduğu gibi yapıştır
- Antigravity UnityMCP üzerinden Unity'yi okuyabilir, sahneyi test edebilir
- Beklenen output: STAGING/ANTIGRAVITY_DONE_painter_iso_holistic.md raporu + code değişimi
- Eğer Antigravity fix uygularsa, ben sonra sonucu doğrularım (read_console + UnityMCP test paint)

Antigravity başarılı olursa:
1. Painter iso wall paint çalışır
2. Yan yana birleşim çalışır
3. Production Batch A (face_NS gen) hâlâ gerekli ama painter tek başına da çalışıyor

Antigravity başarısız olursa:
1. Production Batch A öncelik artırılır (face_NS gen → Wang formülü tam çalışır)
2. Painter wall paint geçici olarak manual mode (auto-connect OFF) kalır
