# 02 — Test Plan Stage by Stage

## Test mantığı

Her şeyi aynı anda test etme. Bu, Unity’ye “lütfen rastgele kır” demenin daha pahalı yoludur.

Test sırası:

```text
1. Boot flow
2. Character selection
3. Player movement/combat
4. Weapon render/sorting
5. Single room clear
6. Room transition
7. Full room chain
8. Shop
9. Boss
10. Full demo run
```

## Test 1 — Boot Flow Test

### Repro
1. Unity Play’e bas.
2. MainMenu açılıyor mu?
3. New Run’a bas.
4. CharacterSelect/Chamber açılıyor mu?
5. Skill Codex açılmamalı.
6. ESC PauseMenu açmalı, Codex açmamalı.

### Pass kriteri
- Play başlangıcında Skill Codex yok.
- Tek cursor var.
- Time.timeScale = 1.
- ESC doğru menüyü açıyor.
- Character seçimi olmadan _Arena başlamıyor.

## Test 2 — Character Selection Test

Warblade:
- Seçiliyor mu?
- PlayerClassManager doğru class mı?
- Görsel Warblade mi?
- SkillBar Warblade skilllerini gösteriyor mu?

Elementalist:
- Seçiliyor mu?
- PlayerClassManager doğru class mı?
- SkillBar Elementalist skilllerini gösteriyor mu?
- Staff yok, rune disc var mı?

## Test 3 — Movement + Collider Test

Her oda için:
- Oyuncu görsel floor olan her yere yürüyebiliyor mu?
- Cliff/void alanına geçemiyor mu?
- Sağ/sol kenarda görünmez duvar var mı?
- Dash sırasında collider takılıyor mu?

Debug önerisi:
```csharp
OnCollisionStay2D:
Debug.Log($"BLOCKED BY {col.collider.name} layer={LayerMask.LayerToName(col.collider.gameObject.layer)}");
```

## Test 4 — Weapon Sorting Test

Warblade:
- Sword floor üstünde mi?
- Cliff arkasına düşüyor mu?
- Sola/sağa dönünce ters/baş aşağı oluyor mu?
- Dash/attack sırasında kayıyor mu?

Elementalist:
- Rune disc elde değil, hover mı?
- Karakterin arkasında kayboluyor mu?
- Cast sırasında spawn doğru mu?

Pass:
- Weapon/Rune her zaman Characters/Entities layer’da.
- Ground/Cliff/Walls altında kalmıyor.
- SortingGroup altında player ile birlikte davranıyor.

## Test 5 — Single Combat Room Test

Her oda ayrı test edilir:
- Oda build oluyor mu?
- Enemy spawn oluyor mu?
- Enemy ölünce counter azalıyor mu?
- Oda clear oluyor mu?
- Portal/exit aktifleşiyor mu?

Pass:
- Console error yok.
- Oda bitince transition hazır.

## Test 6 — Room Chain Test

Zorunlu demo sequence:
```text
Combat_01
Combat_02
Shop_01
Combat_03
Boss_01
DemoClear
```

Pass:
- Sequence sapmıyor.
- Shop oda yanlışlıkla combat spawn etmiyor.
- Boss oda boss dışında gereksiz oda logic’i tetiklemiyor.
- Her geçişte stale object temizleniyor.

## Test 7 — Shop Test

Repro:
1. Shop’a gir.
2. 3 seçenek gör.
3. Birini al.
4. Echo azalıyor.
5. Upgrade uygulanıyor.
6. Shop’tan çık.

Pass:
- Yetersiz Echo varsa item alınmıyor.
- Alınan item tekrar alınamıyor veya UI güncelleniyor.
- Shop ESC/Pause ile kilitlenmiyor.

## Test 8 — Boss Test

Repro:
1. Boss odasına gir.
2. Boss spawn oluyor.
3. Boss attack telegraph gösteriyor.
4. Boss oyuncuyu öldürebiliyor.
5. Oyuncu boss’u öldürebiliyor.
6. Boss ölünce demo clear.

Pass:
- Boss collider doğru.
- Attack telegraph okunuyor.
- Damage windows adil.
- Death sequence bir kez çalışıyor.

## Test 9 — Full Run Smoke Test

En az 5 kez:
- 3 kez Warblade
- 2 kez Elementalist

Kaydedilecek:
- Run süresi
- Nerede takıldı?
- Oyuncu öldü mü?
- Hangi oda sıkıcı geldi?
- Hangi class daha zayıf?
- Console error/warning

## Test rapor formatı

```text
BUILD:
DATE:
CLASS:
RUN RESULT: Clear / Dead / Softlock / Crash

BUGS:
1.
2.

FEEL NOTES:
- Combat:
- Movement:
- Weapon:
- Shop:
- Boss:

PRIORITY:
P0:
P1:
P2:
```
