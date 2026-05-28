ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

**RESPOND INLINE ONLY. DO NOT write to any file. DO NOT save to sandbox.**

---

# Amaç

LaurethStudio'nun yeni PainterSuite v1.1 ana feature'ı **"RIMA Realtime Room Painter"** için authoring UX benchmark. ChatGPT spec elimizde (10-layer EditorWindow + asset palette + scene placement + collision painting + occlusion + parallax depth + save/export). Yol B 2-3 hafta.

Sen (agy) competitive landscape + UX best practice araştır.

## Görev

### Bölüm 1 — Benchmark matrisi (5-7 tool)

Şu authoring tool'larını incele (vision varsa video/screenshot kontrol et):

1. **Hendrix Realtime Parallax Map Builder** (RPG Maker MZ, kaynak ilham) — workflow özet, eksiklik, UX strong points
2. **Tilemapper / Pyxel Edit** (standalone tile painter) — sprite atlas + layer workflow
3. **Unity 2D Tilemap Extras** (Animated Tile, Rule Tile, Group Tile) — Unity built-in nasıl approach yapıyor
4. **Synty Studios POLYGON Map Painter / ProBuilder Map Tools** (Asset Store, paid) — pro Unity tool UX
5. **Tiled Map Editor** (open source standalone, .tmx format) — multi-layer + collision painting
6. **Ogmo Editor 3** (indie standalone, JSON) — 2D level editor mektup-mertebe açıklığı
7. **Trixel Creative / RPG in a Box** (3D ama UX paralel) — UI ergonomics

Her tool için satır:

```
| Tool | Platform/Engine | Layer support | Collision paint | Occlusion zones | Parallax | Save format | Standout UX feature | LaurethStudio için çekilecek |
```

### Bölüm 2 — TOP 5 UX patterns

PainterSuite Room Painter'da uygulanması gereken **5 kritik UX pattern**:
- Drag-and-drop asset palette layout
- Layer toggle/visibility/lock pattern (PS / Procreate / Aseprite)
- Snap toggle keyboard shortcut (Ctrl + tıkla / hold Shift)
- Inspector context-sensitive (selection bazında değişir)
- Undo history depth (ne kadar geri sayım?)

Her pattern için: hangi tool yapıyor + niye iyi + Unity GUILayout ile maliyeti (XS/S/M).

### Bölüm 3 — Common pitfalls

5-7 yaygın hata. Örnek: "Layer 50+ olunca dropdown şişer" / "Asset palette scroll lag" / "Save format binary olursa diff olmaz" / "Undo stack memory leak" / "SceneView gizmo karakter overlap".

Her hata için: yaşandığı tool örneği + çözüm.

### Bölüm 4 — 3 actionable insight for RIMA Room Painter

ChatGPT spec'i biliyor olarak (10 layer, asset palette, collision paint, occlusion, parallax, save) — bu 3 actionable insight (her biri implement edilirse Painter'ı diğerlerinden ayıracak):

1. ?
2. ?
3. ?

Her biri için: ne, niye değerli, effort (XS/S/M/L), market gap (hangi rakip yapmıyor).

## Çıktı

Markdown, max 1200 kelime. Web search izinli — Asset Store comparisons, tool websites, YouTube demo videolar, indie forum threads (gamedev.net, polycount).

Hedef: PainterSuite v1.1 Room Painter MVP scope'unu (Phase A 5-9 gün) bilgilendirsin, kritik UX kararları evidence-based hale getirsin.
