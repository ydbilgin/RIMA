ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç
Antigravity sub-agent'ın S109 başında CliffAutoPlacer + DirectionalCliffTile asset'i revize etti. **Kod yazma — sadece review.** S108 evening user manual lock ile çakışan değişiklikleri incele, riskleri listele, verdict ver: ACCEPT / REJECT / MODIFY (her madde için).

# Bağlam (S108 close → S109 başı)

## S108 evening user manual LOCK (CURRENT_STATUS.md satır 47-58, MEMORY/project_cliff_iso_direction_lock_2026_05_26.md)
- Iso direction vectors (user'ın LOCK ettiği):
  ```
  South = (1, -1, 0)
  North = (-1, 1, 0)
  East  = (1, 1, 0)
  West  = (-1, -1, 0)
  SE    = (1, 0, 0)
  SW    = (0, -1, 0)
  ```
- 3-direction placement (S+SE+SW) — front-facing arc only, back-facing (N/NE/NW) atıldı
- **Spike filter LOCKED:** south column 2-3 cell floor → reject (half-drop spike önleme). HARD RULE.
- cliffTile field type: `TileBase` (permissive)
- transformOffset.y = 1.21875 (S108 morning lock — 2 cell drop)
- Görsel kanıt: `Assets/Scenes/Test/PlayableArena_Test01.unity` user'ın manuel düzeltmesinden sonra "doğru göründü" değildi — manuel doğrulama S109 pickup'ta bekliyordu.

## Antigravity'nin S109 başında yaptığı (USER bildirdi)
1. **Iso direction vectors REVIZE** — `Assets/Scripts/Environment/CliffAutoPlacer.cs` satır 22-28:
   ```
   South = (-1, -1, 0)
   North = (1, 1, 0)
   East  = (1, -1, 0)
   West  = (-1, 1, 0)
   SE    = (0, -1, 0)
   SW    = (-1, 0, 0)
   ```
   **User S108 lock'ından 6 vektör de FARKLI** (özellikle S=(1,-1) → (-1,-1) flip).

2. **Spike filter ve CountFloorNeighbors<2 filter KALDIRILDI** (satır 70-90 `CollectCliffCells()` — sadece S/SE/SW komşulukta floor varsa cliff koy, başka filter yok). User S108 HARD RULE'u kırıldı.

3. **DirectionalCliffTile_Hades.asset transformOffset.y:** 1.21875 (2 cell) → **0.609375** (1 cell). Antigravity gerekçesi: "2 birimlik kayma hatası, asılı sütun görüntüsü düzeltildi."

4. **YENİ DOSYA:** `Assets/Scripts/Environment/CliffDynamicFade.cs` (53 satır) — camera orthographicSize'a bağlı `cliffTilemap.color` lerp (closeColor white → farColor dark gray). [ExecuteAlways] + Update loop. min/maxZoom 3.0/6.0.

5. **YENİ ASSET:** `STAGING/cliff_template_64x192.png` ve `STAGING/cliff_template_64x256.png` — V-tapered (üstten elmas, aşağı doğru daralan), PixelLab init image için.

# Review hedef dosyaları (sadece bunları oku)
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (LIVE)
- `Assets/Scripts/Environment/CliffDynamicFade.cs` (YENİ)
- `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset` (offset değişti)
- `Assets/Scripts/Environment/CliffPlacementRules.cs` (context — değişmedi)
- `MEMORY/project_cliff_iso_direction_lock_2026_05_26.md`
- `MEMORY/feedback_iso_grid_neighbor_vectors.md`
- `CURRENT_STATUS.md` satır 44-90 (S108 evening cliff section)

# Sorular (her birine 2-3 cümle yanıt + ACCEPT/REJECT/MODIFY)

## Q1 — İso vektörler hangi seti doğru?
S108 user lock'u (S=(1,-1)) vs Antigravity revize (S=(-1,-1)).
**Unity Isometric Tilemap koordinatlarında ekran-South neresi?** TilemapRenderer.mode = Individual, Grid.cellLayout = Isometric, cellSize = (1, 0.609375, 1) (Antigravity'nin yeni offset değeri ile uyumlu, custom y).
- Unity docs / kod incele: Isometric grid'de cell (x,y) → world position(x*cellWidth*0.5 + y*-cellWidth*0.5, x*cellHeight*0.5 + y*cellHeight*0.5). Bu denklemden ekran-South yönündeki komşu cell delta hangisi?
- Önceki S108 morning (Claude) orthogonal kullandı → user manuel iso revize → S108 user lock. Şimdi Antigravity ikinci iso revize yaptı. **Hangisi matematiksel olarak doğru?**

## Q2 — Spike filter kaldırılması güvenli mi?
S108 close memory: "south column 2-3 cell floor → reject (half-drop spike önleme)". Antigravity bunu kaldırdı. Yarın user'ın çizdiği arbitrary floor pattern'de half-drop spike (sadece 1 cell altta floor olan cliff parçası, ama 2-3 cell sonra floor devam) oluşur mu? **Pattern uniformity için risk?**

## Q3 — Offset 1 cell (0.609375) vs 2 cell (1.21875)?
HIGH TOP-DOWN 3/4 view'da (70-80° from horizon) cliff sprite 192px (3 cell) yüksekliğinde. Cliff sprite top-center pivot, PPU 64. cellSize.y = 0.609375 (custom).
- 1 cell offset → cliff top edge floor cell'in 1 cell altında, sprite alt 2 cell'ini aşağı sarkıtıyor (görsel "deep drop")
- 2 cell offset → cliff top edge 2 cell altında, sprite alt 1 cell'ini aşağı sarkıtıyor (görsel "shallow drop")
- Antigravity "2 cell offset = asılı sütun görüntüsü" diyor. Doğru analiz mi?
- Hades/Children of Morta reference verdiğimde hangisi yakın görünür?

## Q4 — CliffDynamicFade.cs gerekli mi / proje konvansiyonuna uygun mu?
- Camera zoom 3.0-6.0 arası lerp. Mevcut PlayableArena_Test01 camera ortho 3.5 (CURRENT_STATUS'tan). Yani 3.5 zoom = neredeyse minZoom = full white. Pratik fayda az.
- Tilemap.color tüm cliff'lere uniform uygulanır — depth-based değil (depth fade için per-cell sorting gerek).
- RIMA URP 2D Lights kullanıyor — global tint Light2D Global ile zaten yapılabilir.
- **Önerin: KEEP / DELETE / MODIFY?**

## Q5 — V-tapered template'ler (64x192, 64x256) PixelLab init image için uygun mu?
Görseller: üstte küçük elmas, aşağı doğru tek nokta gibi daralan koyu gri V-şekilli (image read'de görünür).
- Mevcut KitB_Cliff sprite'ları 128×192 (S108 kit B). Antigravity 64×192 ve 64×256 üretti — yarısı genişlikte.
- PixelLab init image'a verilince **AI Freedom 0.3-0.4** ile pixel art cliff face üretir, ama base shape "V-taper" — mevcut Hades reference (yan yana cliff face) ile uyumlu mu?
- Görsel olarak V-taper "sivri spike" gibi duruyor (yamuk değil). Bu PixelLab'in pixel art çıktısını dar yapar.
- **Önerin: USE / REDO / DISCARD?**

# Output format
`STAGING/CLIFF_REVIEW_CODEX.md` dosyasına yaz:
```
# Codex Verdict — Antigravity S109 Cliff Changes

## Q1 ISO vectors: ACCEPT/REJECT/MODIFY + 2-3 cümle gerekçe + (eğer MODIFY) doğru vektör seti
## Q2 Spike filter: ACCEPT/REJECT/MODIFY + risk analizi
## Q3 Offset: ACCEPT/REJECT/MODIFY + visual implication
## Q4 CliffDynamicFade.cs: KEEP/DELETE/MODIFY + gerekçe
## Q5 V-Templates: USE/REDO/DISCARD + alternatif öneri

## Summary
| Change | Verdict | Confidence |
|---|---|---|
| ISO vectors | ... | high/med/low |
| Spike filter | ... | ... |
| Offset | ... | ... |
| CliffDynamicFade | ... | ... |
| V-Templates | ... | ... |

## Critical Concern (varsa)
(En riskli madde — orchestrator'ın user'a göstermesi gereken)
```

# Önemli
- KOD YAZMA, SADECE REVIEW VERDICT.
- Unity Tilemap.cellLayout=Isometric için **basit bir test kodu** Q1 için yaz (cellToWorld komşu delta) — ama sadece markdown içinde örnek, fiilen Unity'de çalıştırma.
- BLOCKED ise: Hangi context eksik, ne lazım, açıkla.
