# Acceptance Checklist

## P0 — Flow / Scene / Input

- [ ] Main Menu Start butonu CharacterSelect / Chamber akışını açıyor.
- [ ] CharacterSelect/Chamber tamamlanmadan _Arena'ya geçilmiyor.
- [ ] Play başlangıcında Skill Codex açılmıyor.
- [ ] ESC doğrudan Skill Codex açmıyor.
- [ ] ESC gameplay'de PauseMenu açıyor.
- [ ] PauseMenu'de Resume, New Run, Settings, Skill Codex, Exit seçenekleri var.
- [ ] Codex kapanınca oyun kilitlenmiyor.
- [ ] Time.timeScale, input action map, cursor state ve UI modal stack restore ediliyor.

## P0/P1 — Movement / Room / Collider

- [ ] Görsel floor olan her tile gerçekten walkable.
- [ ] Sağ floor çıkıntısı invisible wall ile bloklanmıyor.
- [ ] Room template walkable area ile generated collider aynı kaynaktan geliyor.
- [ ] Player block olduğunda hangi collider/layer çarptığı debug loglanabiliyor.
- [ ] Scene gizmo/debug overlay ile walkable polygon, void collider, player collider görülebiliyor.
- [ ] Combat odaları minimum 24x18, tercihen 28x20 hissi veriyor.
- [ ] En az 2 yönde 8+ unit dash lane var.

## P1 — Weapon / Sorting

- [ ] Weapon PlayerRoot altında ve aynı SortingGroup içinde.
- [ ] WeaponRenderer Ground/Cliff/Wall altına düşmüyor.
- [ ] Weapon floor üstünde, karakterle birlikte render oluyor.
- [ ] WeaponSlot localPosition yönlere göre doğru.
- [ ] Geçici beyaz sword sprite yerine Warblade'e uygun, top-down ARPG açılı, okunaklı sword kullanılıyor.

## P1 — UI / Codex / Tooltip / Icons

- [ ] Skill Codex debug panel gibi görünmüyor; PauseMenu içinden açılabilir ayrı ekran.
- [ ] Tooltip hover'da taşmıyor, mavi debug text çıkmıyor.
- [ ] TooltipManager panel kapanınca force-hide yapıyor.
- [ ] Skill icon missing ise warning log var.
- [ ] Missing icon fallback gösteriyor ama gerçek hata gizlenmiyor.
- [ ] Icon loading deterministic: ScriptableObject Sprite ref veya doğrulanmış Resources/Addressables key.

## P2 — Polish

- [ ] Harita/oda görsel olarak floating island + rift portal doktrinine uyuyor.
- [ ] UI görsel dili RIMA'nın Ashen Glyph / diegetic HUD yaklaşımına yaklaşıyor.
- [ ] Debug görselleri/aim çizgileri production playtest'te görünmüyor veya toggle altında.
