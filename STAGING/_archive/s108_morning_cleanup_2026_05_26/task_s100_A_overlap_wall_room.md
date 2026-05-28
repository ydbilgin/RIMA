ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: OverlapWallRoom — Modular Kit ile Seamless Perimeter Test Room

Modular kit (M01-M16) sprite'larını 1.59 unit X spacing ile outer-only perimeter duvar odası oluştur.

## Teknik Detaylar

- M01/M02 sprite = 102×114px, PPU=64 → 1.59×1.78 world unit, pivot center
- X spacing: **1.59 unit** (content edge-to-edge, seamless — Lego overlap yöntemi)
- Topology: outer perimeter only, 10×6 grid
  - Straight walls: M01 (horizontal), M02 (vertical)
  - Outer corners: M03 (TL), M04 (TR), M05 (BL), M06 (BR)
  - Doorway bir kenarda: M09/M10
- BoxCollider2D per piece: size=(1.59, 1.0), offset=(0, -0.39)
- SortingLayer="Walls" — tüm wall piece'lerde teyit et

## File Paths

- Mevcut prefab'lar: `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/modular_kit_v1/`
  - M01.prefab … M16.prefab
- Output scene: `Assets/Scenes/Demo/OverlapWallRoomTest.unity`
- Screenshot: `Assets/Screenshots/OverlapWallRoom_v1.png` (UnityMCP screenshot veya Editor screenshot)

## Adımlar

1. Mevcut M01-M16 prefab'ların SortingLayer="Walls" olduğunu kontrol et, değilse fix et
2. OverlapWallRoomTest.unity sahnesi oluştur (veya varsa aç)
3. Perimeter loop'u kodla: 10×6 outer grid, her pozisyona doğru prefab'ı yerleştir
   - Düz kenar → M01 veya M02
   - Köşe → M03-M06
   - Doorway pozisyonu → M09/M10
4. Her piece'e BoxCollider2D ekle (size + offset yukarıdaki gibi)
5. Sahneyi kaydet, console'u kontrol et
6. Screenshot al

## Başarı Kriterleri

- [ ] OverlapWallRoomTest.unity sahnesi oynatılabilir, duvarlar seamless görünür
- [ ] Tüm wall piece'lerde SortingLayer="Walls"
- [ ] BoxCollider2D her piece'de doğru ayarlanmış
- [ ] Console'da hata/warning yok
- [ ] Screenshot kaydedilmiş
