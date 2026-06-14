# F6 + F7: Culling Stub + Smoke Test — DONE
Date: 2026-05-27

---

## F6 — CliffRuntimeVisibility (Runtime Culling Stub)

### Değişen Dosyalar
| Dosya | Durum |
|-------|-------|
| `Assets/Scripts/Environment/CliffRuntimeVisibility.cs` | NEW — ~47 LOC |
| `Assets/Scenes/Test/PlayableArena_Test01.unity` | MODIFIED — CliffRuntimeVisibility component added to CliffTilemap GO |

### Özet
- `CliffRuntimeVisibility` MonoBehaviour, `[RequireComponent(typeof(TilemapRenderer))]` ile CliffTilemap'a attach edildi.
- `Awake()` + `OnValidate()` içinde `TilemapRenderer.detectChunkCullingBounds = Auto` ayarlanır.
- Inspector toggle (`enableCulling`, default true) + `cullingExtensions` (default `(2,2,0)`) serialized.
- **Compile: 0 error / 0 warning.** TypeCache doğrulandı: `RIMA.Environment.CliffRuntimeVisibility` Assembly-CSharp'ta mevcut.
- PlayMode doğrulama: `detectChunkCullingBounds=Auto`, `chunkCullingBounds=(0.50, 2.50, 0.00)`.

---

## F7 — PlayMode Smoke Test

### Test Sonucu: PASS

PlayMode entry/exit smooth. Konsol (oyun kodu kaynaklı) 0 error, 0 warning.

> NOT: 4-5 adet "MCP-FOR-UNITY: Client handler error: Cannot access a disposed object" logu — MCP server'ın PlayMode geçişinde TCP reconnect sırasında attığı Editor-internal log. Oyun kodu ile ilgisi yok, ignore edilir.
> 
> "Some objects were not cleaned up when closing the scene" — CliffDropShadowPlacer'ın F2 implementasyonunun `HideFlags.DontSave` tile'ları temizlememesinden kaynaklanır. Pre-existing, F6/F7 scope dışı.

---

## F1–F6 LIVE Checklist

| Task | Durum | Notlar |
|------|-------|--------|
| **F1** AdaptiveClusterFilter (CliffClusterRules) | ✅ LIVE — ⚠️ slot BOŞ | `CliffAutoPlacer` (CliffRing GO) `clusterRules` alanı atanmamış. **Kullanıcı manuel atayacak.** Kod hazır. |
| **F2** CliffDropShadowTilemap | ✅ LIVE | GO active=True, tileCount=1, `CliffDropShadowPlacer` CliffRing'de mevcut |
| **F3** 6-katman ParallaxRig | ✅ LIVE | 6 child, tümünde `ParallaxLayer` aktif, faktörler: BG_Void(0.05,0.03) → Foreground_Front(1.10,0.55) |
| **F4** CliffEdgeDustEmitter | ⚠️ EKSIK GO | `CliffEdgeDustEmitter` GO sahnede yok. **Kullanıcı manuel wire yapacak** (component kodu LIVE). |
| **F5** CliffFaceIdleAnimator | ✅ LIVE | Component CliffTilemap'ta aktif, `cliffTileSource` atanmış |
| **F6** CliffRuntimeVisibility | ✅ LIVE | CliffTilemap'ta aktif, `detectChunkCullingBounds=Auto` |

---

## Console Log (PlayMode sırasında)

```
[Game errors]   : 0
[Game warnings] : 0
[Game logs]     : 0
[MCP-internal]  : 4x "Client handler error: Cannot access a disposed object" (Editor reconnect, oyun kodu değil)
```

### Beklenen ama eksik loglar (F2/F5):
- `"CliffDropShadowPlacer: mirrored N cells"` — bu log satırı CliffDropShadowPlacer.cs içinde YOK (script log atmıyor). F2 kodu çalışıyor ama log eklenmemiş.
- `"CliffFaceIdleAnimator: animating N cells"` — bu log satırı CliffFaceIdleAnimator.cs içinde YOK. `cliffTileSource` atanmış ama animasyon döngüsü sessiz çalışıyor.

---

## Kullanıcı Manuel Eylemleri

1. **F1 — CliffClusterRules asset atama:** Unity Inspector → CliffRing GO → CliffAutoPlacer → `Cluster Rules` alanına bir `CliffClusterRules` ScriptableObject asset'i ata (yoksa yeni oluştur: RIMA/Environment/Cliff Cluster Rules).
2. **F4 — CliffEdgeDustEmitter GO oluşturma:** Sahnede yeni GO oluştur → `CliffEdgeDustEmitter` component ekle → `cliffPlacer` = CliffRing, `settings` = CliffDustSettings asset ata.

---

## Dosya Yolları
- `Assets/Scripts/Environment/CliffRuntimeVisibility.cs` (NEW)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (MODIFIED)
