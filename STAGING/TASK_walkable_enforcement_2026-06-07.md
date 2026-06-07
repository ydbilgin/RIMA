# TASK: Walkable enforcement — player AND mobs physically bound to walkable cells

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

## Amaç (USER REQUIREMENT, hard)
Walkable alanlar DOĞRU ve FİZİKSEL olarak uygulanmalı: ne karakter ne moblar walkable-olmayan hücrelerden geçebilmeli — dış void kenarı, İÇ DELİKLER (donut ortası), bloke hücreler dahil; normal hareket, KNOCKBACK savrulması, dash ve elite-teleport dahil. Tek doğruluk kaynağı = template walkable verisi.

## Audit evidence (Explore, file:line — read before coding)
- IsoRoomBuilder.cs:479-553 BuildBoundary: SADECE dış çevre ringi; iç delikler collider ALMIYOR (488-493 skip).
- BaseMobBehavior.cs:166-200: kinematik Rigidbody2D + linearVelocity — kinematik gövde static collider'dan FİZİKSEL BLOKLANMAZ (contact callback ≠ collision response); walkability hiç sorgulanmıyor.
- KnockbackReceiver.cs:79-94: rb.linearVelocity doğrudan — sınır kontrolü YOK.
- PlayerController.cs:305-339: WalkabilityMap pre-check VAR (iyi) ama 330-334 axis-split diyagonal köşe-kesmeye izin veriyor; dash sürüşünde devam-clamp'i yok (186-269).
- EliteAffix.cs:183-220: teleport tilemap-tile varlığına bakıyor, WalkabilityMap/IsWalkable değil.
- WalkabilityMap.cs:186-214: floor tilemap'ten türetiliyor, RoomTemplateSO.walkableGrid'den DEĞİL; runtime kullanıcısı sadece player.
- PropColliderAutoBuilder.cs:24-55: blocking prop BoxCollider2D var ama explicit layer atanmıyor.

## Work items
1. **Boundary ring TAM:** IsoRoomBuilder.BuildBoundary → walkable-olmayan ve 8-yönde EN AZ BİR walkable komşusu olan HER hücreye collision tile (iç delik rimleri dahil). CompositeCollider2D zaten var; geometri yeniden üretilsin. (Skip kuralındaki "no walkable neighbor" filtresi kalır — uzak void hücreleri gereksiz collider almasın.)
2. **WalkabilityMap kaynağı = template:** WalkabilityMap'i RoomTemplateSO.walkableGrid'den doldur (floor-tile varlığı yerine; chamber/_Arena her oda kurulumunda RoomRunDirector init etsin). Mevcut API (IsWalkable/IsWalkableWorld/IsReachable) korunur.
3. **Mob hareket clamp'i:** BaseMobBehavior.FixedUpdate → velocity uygulanmadan önce PlayerController'daki pre-check pattern'inin aynısı (next-pos walkable değilse: önce axis-slide dene, ikisi de değilse zero). WalkabilityMap yoksa mevcut davranış (permissive) kalsın.
4. **Knockback clamp (player+mob):** KnockbackReceiver.DoKnockback süresince her FixedUpdate adımında next-pos kontrolü — walkable dışına çıkacaksa velocity'yi o eksende kes (duvara çarpmış gibi dur, geri sektirme YOK). Knockdown driver'ı aynı receiver'dan geçiyorsa otomatik kapsanır — doğrula.
5. **Diyagonal köşe-kesme fix'i (player+mob ortak helper):** axis-split slide'da, diyagonal hareket için HEM (x+dx,y) HEM (x,y+dy) walkable olmalı; tek helper'a çıkar, PlayerController + BaseMobBehavior kullansın. Dash: dash süresince de aynı pre-check (TryDash hedef kontrolü kalır).
6. **Elite teleport:** IsTeleportDestinationValid → WalkabilityMap.IsWalkable + (varsa) IsReachableFromPlayer; 3 deneme başarısızsa mevcut sessiz-kal davranışı OK ama Debug.LogWarning ekle (bir kez).
7. **Prop layer:** PropColliderAutoBuilder → collider GO'suna explicit "Default" layer (boundary ile aynı blok grubu); player+enemy ikisini de engellediğini doğrula.
8. **Testler:** EditMode — donut template'inde iç rim hücrelerinin collision tile aldığını assert et. PlayMode probe — (a) mob donut deliği üstünden hedefe DÜZ gidemiyor (pozisyonu hole hücresine hiç girmiyor), (b) player'a deliğe doğru knockback uygula → hole hücresine girmiyor, (c) chamber + _Arena akışı bozulmadı (smoke 26/26 + 1/2/3-kapı slot testi yeşil kalır).
9. **Commit** (ydbilgin, English, no Co-Authored-By): `fix(physics): enforce walkable grid for players and mobs incl. interior holes and knockback`. CODEX_DONE/done-dosyasına file:line özet + probe kanıtı.

## Constraints
- Player↔Enemy collision DISABLED kalır (bilinçli — BaseMobBehavior.cs:86); bu task onu değiştirmez.
- Chasm/NarrowPassage trigger sistemine dokunma (ayrı mekanik).
- Performans: per-FixedUpdate kontrol O(1) grid lookup olmalı (BFS/flood-fill YOK hot path'te).
- UI-JSON editör task'inin dosyalarına (UnifiedMapDesigner/RoomJsonImporter/exporter) DOKUNMA — paralel/ardışık ayrı iş.
