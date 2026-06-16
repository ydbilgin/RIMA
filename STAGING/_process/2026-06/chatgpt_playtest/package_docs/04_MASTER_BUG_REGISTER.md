# Master Bug Register

## REWARD-01 — Eski reward objeleri yeni odalarda kalıyor
- **Öncelik:** P0
- **Gözlem:** Önceki odada alınmayan üçlü reward, iki oda sonra hâlâ yerde.
- **Risk:** Yanlış interactable seçimi, duplicated state, progression corruption.
- **Muhtemel neden:** Reward objeleri room-owned parent altında değil; session dispose/reset yok; event/input kayıtları stale.
- **Başarı kriteri:** 10 oda boyunca önceki odadan tek bir reward GameObject/interactable kalmamalı.

## REWARD-02 — G basınca UI açılmıyor
- **Öncelik:** P0
- **Gözlem:** Dördüncü odadaki reward üzerinde `G` basılıyor, skill/pasif/item ekranı yok.
- **Muhtemel neden:** Eski interactable nearest target; stale `selectionActive`; null offer; input map disable; session ID mismatch.
- **Başarı kriteri:** Aktif reward target varken `G` tek seferde doğru inspect/selection UI'yi açmalı veya açık hata feedback'i vermeli.

## REWARD-03 — Ödül çözülmeden oda terk edilebiliyor
- **Öncelik:** P0
- **Gözlem:** Üçlü reward alınmadan ilerlenmiş.
- **Karar:** Seçim zorunluysa kapılar `Completed` state öncesi kapalı. Seçim opsiyonelse ayrı `Reddet/Atla` işlemi olmalı ve session temiz kapanmalı.

## UI-01 — Reward footer/metin genişliği çöküyor
- **Öncelik:** P1
- **Gözlem:** Gravity Cleave kartında cyan alan dar bir sütun, metin harf harf aşağı.
- **Muhtemel neden:** RectTransform width yaklaşık sıfır; LayoutGroup + ContentSizeFitter çatışması; prefab state/pooling.
- **Başarı kriteri:** Türkçe/İngilizce bütün kartlarda footer min genişlik korunur; dikey harf sarılması olmaz.

## DATA-01 — “Eşleşir” etiketi belirsiz ve mapping güvenilmez
- **Öncelik:** P1
- **Doğru mantık:** Bir skill diğerinden sonra kullanıldığında bonus tetikleyen zincir/kombo.
- **Canonical örnek:** Iron Charge → Gravity Cleave: çekilen düşmanlar 1,5 sn stun ve +15 Rage.
- **Şüpheli örnek:** Earthsplitter → Gravity Cleave canonical tabloda yok; Earthsplitter'ın zinciri Bladestorm ve Ironclad Momentum ile ilişkili.
- **Başarı kriteri:** UI `trigger + outcome` gösterir. “Eşleşir” tek başına kullanılmaz.

## AIM-01 — Body ve WeaponSlot aynı yönü tüketmiyor
- **Öncelik:** P0 combat feel
- **Gözlem:** Silah mouse'a dönerken gövde eski movement-facing'de.
- **Muhtemel neden:** Weapon `aimDirection`; Animator `lastMoveDirection` kullanıyor. Idle'da body update edilmiyor.
- **Başarı kriteri:** Combat aim sırasında body ve weapon aynı frame cardinal facing'e geçer.

## AIM-02 — Skill mouse yerine facing yönüne vuruyor
- **Öncelik:** P0
- **Muhtemel yanlış kaynaklar:** `transform.right`, `lastMoveDirection`, Animator floats, WeaponSlot rotation.
- **Doğru kaynak:** Projectile/cone/line = AimDirection; ground target = CursorWorldPoint; self AoE = caster origin.
- **Başarı kriteri:** Bütün skill geometry ve VFX tek `AimSnapshot/CastContext` kullanır.

## LIFE-01 — BuildPlacementController scene kapanışında kalıyor
- **Öncelik:** P1
- **Unity uyarısı:**
```text
Some objects were not cleaned up when closing the scene.
(Did you spawn new GameObjects from OnDestroy?)
The following scene GameObjects were found:
BuildPlacementController
```
- **En güçlü neden:** Scene teardown sırasında başka bir `OnDestroy/OnDisable`, lazy singleton `.Instance` çağırıyor; getter yeni GameObject üretiyor.
- **Başarı kriteri:** Play Mode/scene change 20 tekrar, warning yok; controller sayısı daima 0 veya 1.

## HUD-01 — HUD okunamayacak kadar küçük
- **Öncelik:** P1
- **Gözlem:** Can/resource sol üstte birkaç piksel; skill bar çok küçük.
- **Karar:** Vitality sol alt; skill bar alt merkez; LMB/RMB daha büyük; minimap sağ üst; UI scale ayarı.

## UI-03 — Pause menüsü final oyun kalitesinde değil
- **Öncelik:** P2
- **Gözlem:** Düz siyah küçük panel, focus/selection zayıf, kompozisyon ölçeği düşük.

## UI-04 — Settings sıkışık ve erişilebilirlik yetersiz
- **Öncelik:** P2
- **Karar:** Sol kategori rail; sağ içerik; sabit footer; UI Scale; camera shake/hit-stop/VFX/damage number seçenekleri.

## UI-05 — Skill Codex bilgi mimarisi zayıf
- **Öncelik:** P2
- **Karar:** Class list + skill list + detail panel. Combo/state/boss rule ayrı bloklar.
