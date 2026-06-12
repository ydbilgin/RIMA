# 03 — Council Sentezi (Opus tasarım + Gemini UX)

## Karar: Sol-rail, 6-sekmeli, chrome-skinli uGUI Director Mode

### Mod akışı + kamera (Opus)
- DIRECTOR giriş: `timeScale=0`, kamera serbest-cam'e lerp (orthoSize ×2.2, oda merkezi), `unscaledDeltaTime` ile (timeScale=0'da donmaz). HUD fade-in, karakter input off, dünya etkileşimi (tıkla-koy) on.
- Serbest-cam: WASD pan, scroll zoom, orta-tuş drag.
- "BAŞLAT": kamera karaktere dalış, timeScale=1, input on, HUD minimize (telemetri şeridi + geri rozeti kalır).
- Geri: ` veya rozet → DIRECTOR. Sahne KORUNUR (kümülatif, reset yok).
- **KRİTİK:** tüm HUD tween + kamera `unscaledDeltaTime`.

### Sekme yapısı (sol dikey rail ~64px, üst bar DEĞİL — top-down görüşü kesmesin)
| # | Sekme | İçerik | Hook |
|---|---|---|---|
| 1 | Spawn | enemy palette (slot grid), tıkla-koy sınırsız, sağ-tık sil, wave preset | `EncounterController.SpawnEnemy` |
| 2 | Map | node grafiği, seç→atla, reroll seed | `DungeonGraph.Generate`, `JumpToNode` |
| 3 | Build | 3 alt-sekme: Tile / Cliff / Prop | PaintCell / Regenerate / Generate |
| 4 | Class & Skill | 10 sınıf swap, skill draft override, LMB/RMB ata | `SetPrimaryClass`, `DraftManager` |
| 5 | Stats | ClassStatRuntime slider'ları, canlı | runtime copy |
| 6 | Telemetry | DPS/TTK/hit canlı, CSV export | hasar event abonelik |

Aktif sekme `slot_active`, pasif `slot_normal`. İçerik paneli `minimap_frame` (Window 9-slice).

### Chrome skin eşlemesi (Opus)
| Eleman | Chrome sprite |
|---|---|
| Ana panel | `minimap_frame` (Window) |
| Sol rail sekmeler | `slot_normal`/`slot_active` |
| Aksiyon butonu (Başlat/Generate) | `ribbon_base` |
| Alt-sekme/küçük buton | `menu_button` |
| Palette hücresi | `slot_normal` |
| LMB/RMB slot | `slot_lmb_rmb` |
| Skill/preset kart | `reward_card` |
| Hover ipucu | `tooltip_box` |

### "Güzel" reçetesi
CanvasScaler Scale-With-Screen 1920×1080 match 0.5 · tüm chrome Image=Sliced · Point filter, no-compression · `UI_Chrome.spriteatlas` (1 draw call) · pixel font no-AA · ember accent aktif sekme+Başlat · slate gövde.

### Gemini UX desenleri
- **Sekme geçişi:** `CanvasGroup` (alpha+interactable+blocksRaycasts) toggle — Destroy ETME.
- **Tıkla-yerleştir:** sol-tık drag-paint, sağ-tık hızlı sil, imleç ucunda yarı-saydam ghost preview, grid-snap (`ScreenToWorldPoint`+round), yerleştirmede "pop" scale animasyonu, fare tekeri brush boyutu.
- **Director akışı (Halo Forge / Far Cry Arcade):** pause → tanrı kamerası → editör UI → Play'de snapshot kaydet (quick-reset için) → zaman akar.
- **uGUI güzelleştirme:** 9-slice zorunlu, çentikli pixel slider (Fill+Handle ayrı sprite), TMP+Point, hover'da pixel renk/outline + UI ses.
- **Training mode (SF/GG):** anlık DPS + combo hasar + floating text, dummy AI davranış dropdown (Pasif/Saldır/Hareketli), hitbox overlay tek tuş, **Quick Reset** (HP/CD/düşman son snapshot'a).

### Build sırası (cx parçaları)
1. İskelet: DIRECTOR/TEST mod + kamera lerp + ` toggle + boş uGUI Window + sol rail + chrome skin (en yüksek "güzel" getirisi)
2. Spawn sekmesi (SpawnEnemy hook doğrula)
3. Build/Tile (F2 absorbe: PaintCell public, IMGUI sök, uGUI palette)
4. Cliff + Prop alt-sekmeleri
5. Class&Skill + Stats
6. Telemetry (event + CSV)

### Riskler
- `SpawnEnemy` public imza doğrulanamadı → cx ilk iş teyit/expose
- timeScale=0 → unscaledDeltaTime ZORUNLU (yoksa tween/kamera donar)
- PaintCell private+IMGUI → refactor, tilemap null-guard
- Cliff/Prop Regenerate mevcut sahneyi ezebilir → additive/clear-scoped tut
- Chrome 9-slice border meta set değilse köşe bozulur → import QC

### Snapshot (Gemini, eklenecek)
"Başlat" anında dünya durumu (spawn/paint/prop/HP) snapshot → Quick Reset tek tuşla son snapshot'a döner.
