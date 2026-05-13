# Codex Review: AI-Assisted Map Builder (Karar #115 candidate) S66

**Tarih:** 2026-05-13
**Tip:** INDEPENDENT REVIEW + KOD İNCELEME
**Çıktı:** CODEX_DONE.md'ye append

## KRİTİK UYARI

Antigravity'nin önerisinde **"tıpkı oyun oynar gibi" / "in-game level editor" / "fullscreen game-view editor"** framing'i var. **OPUS BU FRAMING'İ REDDETTİ.** Scope yutucu. Sen de aynısını yap — Unity Editor window kalsın, polish ile evrilsin. Fullscreen game-view editor refactor önermek = 80-120 saat scope creep = Faz 1.0 patlat.

## LOCKED Kısıtlar (Sorgulama, Üzerinden Tasarla)

- **Karar #62:** 15 node runtime procedural. Map Builder authoring-time, runtime placement bozmaz.
- **Karar #85:** Hades arena + Open Vista parallax. Map Builder vista template authoring'e izin verir ama runtime selection bozmaz.
- **Karar #100:** 32x32 tile, ~35° top-down, chibi 64x64.
- **Karar #106:** PixelLab MCP üretim YASAK, web UI kullanılır. **Unity Editor içinden PixelLab Inpaint API çağrısı = bu yasağa girer = REDDET.**
- **Karar #113:** Camera ~35° tek konverjans + Orthographic Size kalibrasyonu.
- **Karar #114:** 8 yön animasyon (karakter). WallAutoConnect 4-bit NSEW kalır (ayrı domain).

## Opus Initial Judgment Özet

**Verdict:** ACCEPT WITH MODIFICATIONS

**Kabul edilenler:**
- AI baseline generator (C# procedural, LLM çağrısı YOK)
- Inpaint region re-seed (Unity-internal, PixelLab API DEĞİL)
- Anchor zone painter (placement değil zone)
- FloorVariantPainter/WallAutoConnect %80 reuse
- BrushController stack'e 2 yeni mode (Inpaint Region + Anchor Zone)
- F2 window'a Generate/Reseed butonları ekle (8-12 saat)
- Save → RoomConfig prefab + variant metadata (PNG export YOK)

**Reddedilenler:**
- Fullscreen game-view editor refactor (40-60h yutucu)
- PixelLab Inpaint API çağrısı (Karar #106 ihlali)
- Runtime placement override (Karar #62 ihlali)
- PNG export (RIMA mimarisi prefab-based)

**Scope:**
- Faz 1.0: MVP generator + Generate butonu = 12-16h
- Faz 1.5: Inpaint region + Anchor zone + UX polish = 30-40h
- Exit criteria Faz 1.0: 5 oda generate, user <%20 düzeltme

## Codex Görevin

### Adım 1 — Kod İnceleme
Şu dosyaları oku ve mevcut mimariyi anla:
- `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs`
- `Assets/Editor/RoomDesigner/FloorVariantPainter.cs`
- `Assets/Editor/RoomDesigner/WallAutoConnect.cs`
- `Assets/Editor/RoomDesigner/Brushes/BrushController.cs` (varsa)
- `Assets/Editor/RoomDesigner/SaveLoad/RoomSaver.cs` (varsa)
- `Assets/Scripts/Runtime/Room/RoomConfig.cs`
- `Assets/Scripts/Runtime/Room/RoomLoader.cs`
- `Assets/Scripts/Runtime/Room/RoomRegistry.cs`

### Adım 2 — Bağımsız Review

Opus'un kararını sorgula. Şu format:

```
# Codex Independent Review: Map Builder S66

## Verdict
[AGREE / MODIFY / DISAGREE Opus]

## Opus 5 Açık Sorusuna Cevap (kod incelemeden sonra)
1. RoomBaselineGenerator: ScriptableObject template vs kod tablosu — Codex önerisi + neden
2. Inpaint region re-seed semantiği: user-painted variant lock layer mı, üzerine yazılır mı?
3. Anchor zone schema: rect/polygon/tile-mask — runtime spawn sistemi hangisini ister?
4. Variant metadata serialization: int[] grid + LUT vs per-tile struct array — vista odalar için bellek?
5. F2 "Generate" deterministic: seed + biome + archetype reproducibility garanti?
6. Preview panel: render texture vs ayrı camera — Karar #113 ~35° tutarlılığı?

## Mevcut RoomDesigner Kod Durumu
- Hangi dosyalar var, hangileri eksik
- BrushController stack genişleme noktası
- FloorVariantPainter generator hook olarak kullanılabilir mi
- WallAutoConnect 4-bit/8-bit netleştirme

## Faz 1.0 MVP Generator Tasarım Önerisi
Concrete plan:
- Dosya yapısı: yeni dosya isim, namespace, sorumluluk
- Algoritma: archetype template seçim → floor variant seed → wall layout → prop anchor sample
- Determinism: seed pipeline
- Generate butonu UI integration

## Faz 1.5 Inpaint + Anchor Zone Tasarım Önerisi
- Inpaint region rect selection: nasıl, hangi tool kategorisinde
- Anchor zone forbidden/preferred: serialize semantiği
- Runtime spawn sistem köprü

## Risk Tespitleri (Opus kaçırdı)
- Editor performans (büyük oda + variant blend gerçek-zamanlı render)
- RoomConfig migration (mevcut prefab'lar yeni variant metadata için)
- WallAutoConnect 4-bit vs 8 yön karakter — gerçek çelişki var mı (Opus "ayrı domain" dedi, kod kontrol et)

## Karar #115 Final Metin Önerisi (Codex revize)
[Opus draft'ı al, kod gerçeğiyle netleştir]

## ORCHESTRATOR NEXT STEP
- Eğer Codex AGREE: rima-doc spawn → MASTER_KARAR_BELGESI #115 + room_authoring.md revize + GDD'ye AI-Assisted Map Builder pipeline notu
- Eğer MODIFY: Opus'a final synthesis dispatch
- Eğer DISAGREE: alternatif öneri
```

## Kısıtlar

- Kod yazma YOK — sadece review + tasarım önerisi
- Türkçe, 1000-1500 kelime
- LOCKED kararları sorgulayabilirsin ama override etme
- Antigravity'nin "oyun gibi" framing'ini REDDETTİĞİNİ teyit et — scope disiplinli olsun
- CODEX_DONE.md'ye append
- Effort: high (kod inceleme + tasarım)
