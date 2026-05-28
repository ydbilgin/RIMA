# TASK: Cliff North/NE Diagnosis — Why are some cliffs in saçma yerler?

ACTIVE RULES: (1) think before reviewing (2) min response, cite specific image areas (3) inline only (4) BLOCKED if image unreadable.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

Amaç: User sabah dedi: "Cliffler doğru tarafta güzel duruyor ama north'takiler saçma sapan yerlere geliyor". Ama mevcut konfig 3-dir (S + SE + SW) — north yönünde cliff KOYULMUYOR (kod kontrol edildi). Yine de user'a göre "north saçma" durumlar var. Sebep tespit + somut fix öner.

## Mevcut Config (S107 overnight final state)
- `CliffAutoPlacer.CollectCliffCells()` sadece 3 yön: S, SE, SW
- `DeterministicVariantTile.transformOffset = (0, 1.5, 0)` (sprite 1.5 unit yukarı çekili)
- `spriteScale = (1, 1)` (PPU 64 native)
- `TilemapRenderer` layer=Floor, order=-1 (Floor sprite arkasında render)
- Floor cellSize = (1, 0.609) dimetric
- 262 cliff tile

## Screenshot
`Assets/Screenshots/cliff_morning_state.png` — current state (3-dir + offset 1.5)

## Bilgi: RIMA Coordinate System
- Tilemap Vector3Int — Y-axis pozitif = "north", negatif = "south"
- Ancak DIMETRIC floor sprite: 64×32 px, dimetric perspective — visually "north" görünen yer Tilemap coordinate olarak değil "diagonal" olabilir
- Yani user'ın "north" dediği visual top-of-screen, ama Tilemap koordinatında bu NE veya NW corner olabilir
- Floor cells diagonal patterns oluşturuyor — visual "north" edge aslında SE veya NE direction cell olabilir

## Sorular (her biri için somut cevap)

### 1. Saçma North bölgeleri tespit
- Screenshot'tan hangi bölgeler "saçma" görünüyor? Spesifik konum (sağ üst köşe, sol üst kıvrım vs)
- Bu bölgeler Tilemap koordinatında hangi yönde? (S/SE/SW yönlerden hangi cell?)

### 2. Sebep analizi (3 hipotez test et)
- **H1 Visual north ≠ Tilemap south**: Dimetric perspective sayesinde visually "north" görünen kenar aslında SE veya SW cell? Yani 3-dir kuralı doğru ama visual yorum farklı?
- **H2 Sorting layer issue**: Cliff sprite Floor arkasında render OK ama bazı bölgelerde Floor sprite eksik/farklı sorting?
- **H3 Sprite art problemi**: Cliff sprite'ın kendisi "north" görünüm vermek üzere tasarlanmamış (drop face aşağı bakar), bazı positions'da ters duruyor?

### 3. Fix önerisi (eğer gerçekten bug ise)
- Hangi cell'ler problemli, hangileri silinmeli/taşınmalı?
- 2-dir (sadece SE+SW, S skip)?
- 4-dir (S+SE+SW+E veya +W)?
- Per-cell custom dir mask (designer manuel disable)?
- Yeni cliff sprite (N-facing variant)?

### 4. Verdict — autonomous uygulanacak
- Net tek satır karar
- Effort tahmini

## Hard Constraints
- Inline only
- Spesifik image bölgeleri cite et
- Net karar zorunlu — autonomous uygulanacak
- BLOCKED: bilgi yetersiz veya image okunamaz

## Beklenen uzunluk
400-600 kelime, max signal.
