# 01 — Build Order

Bu sıralama önemlidir. Sırayı bozarsan sistemler birbirini ezer, Unity de senin ruhunu prefab override olarak kaydeder.

## Phase 0 — Proje sağlık kontrolü

Önce:
- Console compile error = 0
- MainMenu → CharacterSelect/Chamber → _Arena akışı doğrulanmalı
- ESC bug’ı düzeltilmeli
- Skill Codex oyun başında açılmamalı
- Tek cursor kalmalı
- Time.timeScale kilitlenmemeli

Çünkü daha oda, boss, shop yazmadan önce oyun zaten menüde bayılıyorsa gerisi tiyatro.

## Phase 1 — Demo boot flow

Hedef:
```text
MainMenu → Chamber/CharacterSelect → _Arena
```

Yapılacak:
1. Play/New Run yalnızca CharacterSelect/Chamber açmalı.
2. Character seçilmeden run başlamamalı.
3. Demo için sadece Warblade ve Elementalist seçilebilir olmalı.
4. Diğer sınıflar görsel olarak görünse bile disabled/locked/display-only olabilir.
5. Chamber’dan çıkış G ile yapılmalı.
6. Run’a geçişte UIManager.ResumeGame() veya eşdeğer state reset çalışmalı.

## Phase 2 — Player combat minimum

Warblade:
- LMB: slash
- RMB veya Q: cleave/charge
- Space: dash
- Hitbox net, cooldown basit

Elementalist:
- LMB: projectile
- RMB veya Q: small AoE
- Space: dash
- Projectile spawn noktası doğru

Bu aşamada animasyon kötü olabilir ama vuruş net olmalı.

## Phase 3 — Weapon attachment

Warblade:
- Greatsword player child object olarak bağlanır.
- SortingGroup altında kalır.
- Floor/cliff arkasına düşmez.
- Direction’a göre localPosition/localRotation ayarlanır.

Elementalist:
- Elde silah yok.
- Floating rune disc sağ avuç üstünde/önünde hover eder.
- Disc ayrı SpriteRenderer olabilir.
- Cast sırasında kısa glow/pulse oynatılır.

## Phase 4 — Room chain

Hedef:
```text
Combat_01 → Combat_02 → Shop_01 → Combat_03 → Boss_01
```

Yapılacak:
- RoomRunDirector içinde demo override/forced sequence eklenebilir.
- Random graph devre dışı bırakılabilir.
- Kapı/portal seçenekleri minimum tutulur.
- Her oda clear condition ile bir sonrakine geçer.

## Phase 5 — Combat room content

Room 1:
- 3-4 basit mob
- oyuncu öğrenir

Room 2:
- 5-6 mob
- 1 ranged veya elite-lite

Room 3/pre-boss:
- 6-8 mob
- daha sıkışık ama adil arena

Her odada:
- En az 2 temiz dash lane
- Görsel floor = yürünebilir alan
- Görünmez duvar yok
- Mob spawn oyuncunun dibinde değil

## Phase 6 — Shop room

Minimum shop:
- 3 seçenek
- heal
- damage upgrade
- cooldown veya max HP upgrade
- Shattered Echo harcama
- Alınan item hemen uygulanır
- Shop’tan çıkış net

Ekonomiyi şimdilik mükemmelleştirme. Demo shop “çalışıyor mu?” testidir.

## Phase 7 — Boss room

Boss:
- Tek boss
- 3 hareket yeterli
  1. Yakın saldırı / slam
  2. Telegraph AoE
  3. Projectile veya charge
- Faz sistemi opsiyonel, en fazla HP %50’de hızlanma
- Boss ölünce demo clear screen

## Phase 8 — Polish pass, ama minimum

Sadece:
- Hit flash
- Damage number veya HP feedback
- Boss telegraph
- Shop readable UI
- Pause menu
- Demo clear screen

Make-up/art pass sonra.
