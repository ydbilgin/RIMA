# KARAR — Sandbox/Director Mode Tool (2026-06-12)

> Sentez: Council (Opus tasarım + Gemini UX) + ChatGPT (review + 3 mockup + uGUI hiyerarşi + ek fikir). Mockup'lar chrome skin'in profesyonel durduğunu kanıtladı. KİLİTLİ.

## 1. Onaylanan ana yapı (tüm danışmanlar hemfikir)
- **Oyun-içi runtime tool**, `#if DEMO_BUILD || DEVELOPMENT_BUILD` guard (save yazmaz, analytics kapalı, asset kirletmez)
- **Master tuş `` ` ``** → DIRECTOR (timeScale=0, free-cam, world-edit) ↔ TEST (timeScale=1, oyna)
- Tüm Director loop/tween/kamera **unscaledDeltaTime**
- Sekme geçişi **CanvasGroup** (alpha+interactable+blocksRaycasts) — Destroy YOK
- **Sol dikey rail** (~96px), üst bar değil (top-down görüşü kesmesin)
- Sahne **kümülatif** (Director↔Test arası korunur), "Başlat" anında **snapshot** → Quick Reset

## 2. Sekme sırası — ChatGPT düzeltmesi KABUL (demo-akışı)
Council sırası teknik; ChatGPT demo-etkisi için yeniden sıraladı:
**1. Spawn → 2. Class&Skill → 3. Stats → 4. Build(Tile/Cliff/Prop) → 5. Map → 6. Telemetry**
Gerekçe: izleyici önce düşman+karakter+sayı+sonuç görmeli; Map/Build teknik, sonra.

## 3. Chrome skin eşlemesi (mockup'larla doğrulandı)
| Eleman | Sprite |
|---|---|
| Ana panel (Window) | `minimap_frame` Sliced |
| Sol rail sekme | `slot_normal`/`slot_active` |
| Büyük aksiyon (Başlat/Generate/Export/Jump) | `ribbon_base` |
| Küçük/alt buton | `menu_button` |
| Palette hücresi | `slot_normal` |
| LMB/RMB slot | `slot_lmb_rmb` |
| Skill/preset kart | `reward_card` |
| Tooltip/inspector | `tooltip_box` |
| Alt telemetri şeridi | `menu_button` stretched |

CanvasScaler 1920×1080 match 0.5 · Point filter · `UI_Chrome.spriteatlas` (1 draw call) · pixel font no-AA · ember accent aktif+Başlat · slate gövde.

## 4. Sekme içeriği + hook
| Sekme | İçerik | Hook |
|---|---|---|
| **Spawn** | enemy palette grid, tıkla-koy SINIRSIZ, sağ-tık sil, ghost preview, wave preset (Triple/Lockdown/Clear) | `EncounterController.SpawnEnemy(id,pos)` ⚠️doğrula |
| **Class&Skill** | 10 sınıf swap, skill draft override, LMB/RMB ata | `PlayerClassManager.SetPrimaryClass`, `DraftManager` |
| **Stats** | maxHP/physPower/abilityPower/atkSpeed×/moveSpeed/debugMult slider canlı + Reset/Save/Export | `ClassStatRuntime` (kopya) |
| **Build/Tile** | palette+brush | `InPlayMapPaintOverlay.PaintCell` (public yap, IMGUI sök) |
| **Build/Cliff** | height/edge/scope/override + Regenerate/Undo/Clear | `CliffAutoPlacer.Regenerate()` |
| **Build/Prop** | density slider + Generate/Clear | `BridsonPoissonAutoPlacer`+`RoomDecorationPass` |
| **Map** | node grafiği, seç→atla, reroll seed | `DungeonGraph.Generate`, `JumpToNode` |
| **Telemetry** | DPS/TTK/damage-source/resource grafiği, CSV export | hasar event abonelik |

## 5. ChatGPT UX eklemeleri (KABUL)
- **Başlat butonu çift-durum:** DIRECTOR'da "BAŞLAT", TEST'te "DIRECTOR'A DÖN" — aynı yer/chrome
- **Alt şerit mod yazısı:** PLACE/ERASE/PAINT/INSPECT
- **Sağ-tık tutarlı:** her sekmede sil/erase/remove
- **Ghost preview ZORUNLU** + grid-snap + brush boyutu (fare tekeri) + "pop" animasyon
- **Selection inspector** (`tooltip_box`): seçili mob/prop/node → ID/HP/AI-mode/prefab
- **Dummy AI mode** (spawn'a): Passive/Attack/Move/Cast/NoCooldown/BossLoop
- **Hitbox overlay** (H): player cyan / enemy amber / damage-zone kırmızı / projectile çizgi
- **Snapshot stack** (5 slot): F5 al, F9 dön
- **Encounter recipe export** JSON (spawn+room+stats+class+seed) — Claude/cx'e test verisi

## 6. Build sırası (cx parçaları)
**ÖN-KOŞUL (bağımsız, foundational):**
- **Stat sistemi çekirdeği** — `ClassStatProfile`+`ClassStatRuntime`+`DamagePacket`+`DamageCalculator`+10 asset+wiring (önceki kilit, Faz 1-3). Stats sekmesinin önkoşulu, her halükarda gerekli.

**SONRA (Director Mode):**
1. **İskelet:** mod+kamera lerp+` toggle+chrome Window+sol rail (en yüksek "güzel" getirisi)
2. Spawn sekmesi (SpawnEnemy hook doğrula)
3. Class&Skill + Stats
4. Build/Tile (F2 absorbe: PaintCell public, IMGUI sök)
5. Cliff+Prop
6. Map + Telemetry

## 7. Riskler
- `SpawnEnemy` public imza doğrulanamadı → cx ilk iş teyit/expose
- timeScale=0 → unscaledDeltaTime ZORUNLU
- `PaintCell` private+IMGUI → refactor, F2 absorbe (çift paint-UI olmasın)
- Cliff/Prop Regenerate manuel işleri ezmesin → scope+undo
- Chrome 9-slice border meta QC (zaten cx mount'ta set edildi)

## 8. uGUI hiyerarşi
ChatGPT'nin tam ağacı: `STAGING/_process/2026-06/chatgpt_sandbox_output/docs/C_uGUI_hierarchy.md` — cx'e doğrudan referans.

## ORCHESTRATOR NEXT
1. Stat sistemi çekirdeği (cx, Faz 1-3) — bağımsız, foundational
2. Director iskelet (cx) — kamera+toggle+chrome Window+rail
3. Sekmeler demo-sırasında
