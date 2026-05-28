ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — review only, no edits (4) BLOCKED if unclear.

**Output dosyası:** `CODEX_DONE_cliff_phase3_review.md` — max 500 kelime.

---

# Amaç

S110 Phase 3 cliff refactor REVIEW (kod yazma, sadece inceleme + edge case audit).

User talimatı (verbatim):
> "Cliflerin mantıklı yerleştirilmesi: boşluk olduğunta tersine doğru koyarsak dışa taşma olmaz. iç boşluklar için de mantıksal şekilde düşün"

Orchestrator yorumu (uyguladı):
- **Eski algoritma:** Floor cell'in S/SE/SW komşusu boşsa → o **boş komşuya** cliff koy (void cell yerleştirme). Cliff sprite top-pivot + `transformOffset.y` ile aşağı sarkıyordu → görsel "dışa taşma".
- **Yeni algoritma (S110 Phase 3):** Aynı koşulda cliff'i **floor cell'in kendisine** koy. Sprite sarkacı zaten aşağı doğru — void üstüne biner ama floor sınırını aşmaz. Spike filter kaldırıldı (void yerleşimi olmadığı için spike concept artık geçersiz).
- **İç pocket mantığı:** Doğal davranış — pocket'in etrafındaki tüm floor cell'ler aynı kuralla cliff alır (komşu boş → cliff). Ekstra filtre yok.

## İncelenecek dosya

`F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Scripts\Environment\CliffAutoPlacer.cs`

Mevcut LIVE state (compile 0 error). 111 satır. Refactor `CollectCliffCells()` (lines ~77-99) + `IsSpike` method silindi.

## Görev — şu 6 noktayı raporla

1. **Algoritma doğruluğu:** "Ters yerleştirme" yorumum doğru mu? User'ın "dışa taşma olmaz" ifadesini doğru yorumladım mı, yoksa kullanıcı başka bir şey istiyor olabilir mi? Eğer alternatif yorum varsa açıkla.

2. **Edge case audit:** Şu durumlarda algoritma ne yapıyor, doğru mu?
   - **İzole floor cell** (tek-cell, 8 komşu boş)
   - **İnce 1-cell koridor** (sadece S yönde uzayan)
   - **Floor cluster içinde 1-cell void pocket**
   - **L-corner** (floor'un S+SE+SW hepsi farklı zamanlarda boş)
   - **Diagonal floor** (sadece SW veya SE komşu var)

3. **Sprite & offset uyumluluğu:** `cliffTile` `DeterministicVariantTile` veya `TileBase`. Eski yerleşimde `transformOffset.y = 1.5` (S106) veya `1.21875` (S108) ayarlıydı — bunlar **void cell**'e yerleştirilmiş cliff'in görsel hizalanması içindi. Yeni yerleştirmede (floor cell'in kendisi), aynı offset hâlâ doğru çalışır mı? Görsel hizalama bozulur mu — düzeltme gerek mi (offset'i 0 veya farklı yapmak)?

4. **Sorting & rendering:** CliffTilemap layer Floor, order -1. Cliff şu an floor altına render ediliyor. Yeni yerleşimde (cliff cell = floor cell aynı), iki tile aynı cell pozisyonunda — render sırası ne olur? Floor sprite cliff'i örter mi, ya da tersi? Sorting order güncellemesi gerek mi?

5. **Performans:** `floorBounds.allPositionsWithin` her cell için 3x HasTile çağrısı. 100×100 floor için ~30K HasTile. Halt risk? Cache pattern öneri?

6. **VERDICT:** ADOPT (mevcut hali bırak) / PATCH (X düzeltme gerek) / REVERT (algoritma yanlış, eski hale dön) + tek cümle sebep.

## Yöntem

- Dosyayı `cat -n Assets/Scripts/Environment/CliffAutoPlacer.cs` ile oku
- Görsel hizalama için `CliffPlacementRules_Hades.asset` (varsa) ve `KitB_Cliff` sprite import settings (PPU 64, top-pivot) referans al
- Spekülasyon yok, dosyaya bakıp empiri çıkar
