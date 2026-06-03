ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
IsoRoomBuilder izometrik oda render sistemini feasibility/reuse/bug lensinden review et — ANALYSIS ONLY, kod değiştirme.

## Oku (PATH ile, kendin oku — inline yok)
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` (ANA inceleme hedefi)
- `Assets/Scripts/MapDesigner/Props/PropDefinitionSO.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropColliderAutoBuilder.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropSorterRuntime.cs`
- `Assets/Scripts/MapDesigner/Props/Runtime/PropRuntimeSpawner.cs`
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs`

## Bağlam
IsoRoomBuilder MonoBehaviour bir RoomTemplateSO'yu izometrik yüzen-ada olarak çiziyor: floor (walkable ∪ prop-footprint mask), yönlü cliff (her floor hücresi için SW-komşu void VEYA SE-komşu void → cliff; sprite cellCenter=GetCellCenterWorld + tuck S=(0,0.29)/SE=(-0.48,0.29)/SW=(0.48,0.29)), void-ring CompositeCollider2D boundary, spawn/door marker, props (mevcut PropDefinitionSO/PropRegistrySO reuse — kendi iso BuildProps'u GetCellCenterWorld ile), door gate görselleri (gate_north/gate_west sprite + rune overlay child).

## Review soruları (her birine PASS/FAIL + file:line + somut fix)
1. **floor-mask** = walkable ∪ prop-footprint birleşimi (BuildFloor + AddPropFloorCells): doğru/robust mu? Edge-case: prop-footprint bounds dışına taşarsa, decor prop void hücreye yerleşirse, propRegistry null ise ne olur?
2. **cliff** per-hücre SW/SE-void kuralı (BuildCliffs + GetCliffSprite): mantık tutarlı mı? cellCenter+tuck konumlandırma robust mu? Bir hücre hem SW hem SE void ise (South) doğru mu?
3. **prop iso yerleşim + collider:** BuildProps GetCellCenterWorld kullanıyor (iyi). AMA PropColliderAutoBuilder tile-unit boyutlu BoxCollider2D yaratıyor (size=footprintSize, offset=size*0.5) — iso world'de (cell 0.96×0.585) bu collider YANLIŞ boyut/offset. Ne kadar kritik (gameplay-blocker mı)? En temiz fix ne — (a) PropColliderAutoBuilder'a iso-mod, (b) IsoRoomBuilder kendi collider'ı koysun, (c) ertele?
4. **BuildGates görsel-only:** GateBehavior lock/unlock gameplay wiring YOK (P5'e bırakıldı), sadece gate sprite + rune child. Kabul edilebilir bir ara-durum mu, yoksa şimdi GateBehavior bağlanmalı mı?
5. **genel bug/kod-kalite:** edit-mode Destroy (ClearPrevious runtime Destroy çağırıyor — editor'da sorun?), null-guard'lar, sorting: cliff/gate "Floor" layer'da, prop "Props" layer'da — Props layer Floor'un önünde ama Entities'e göre nerede (oyuncu prop'un arkasında/önünde doğru render olur mu)?

Sonucu CODEX_DONE.md'ye yaz. Prior audit'i tekrarlama. Min-code + surgical öner.
