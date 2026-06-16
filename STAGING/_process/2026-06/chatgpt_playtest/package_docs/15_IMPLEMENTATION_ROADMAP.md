# Claude Uygulama Yol Haritası

## Sprint 0 — Reproduction ve envanter
- Projeyi yedekle/branch aç.
- Reward, room flow, interaction, input, facing, skill cast, UI prefab ve singleton sınıflarını haritala.
- Altı screenshot senaryosunu reproduce et.
- Her P0 için failing test/log kaydı oluştur.

## Sprint 1 — Reward ownership
- `RewardSession` owner belirle.
- Eski objelerin room transition'da dispose edilmesini sağla.
- Door gate'i reward completion'a bağla.
- G interaction current room/session doğrulaması ekle.
- Pause/resume ve scene reload testleri.

## Sprint 2 — Aim contract
- Tek AimService.
- Body/weapon/skills ortak snapshot.
- Cardinal direction + hysteresis.
- Mouse ve controller parity.
- Bütün Faz 1 skill tiplerini test et.

## Sprint 3 — Lifecycle
- BuildPlacementController lazy creation kaldır.
- Teardown `.Instance` çağrılarını temizle.
- Domain Reload on/off ve 20 loop test.

## Sprint 4 — Reward card layout
- Runtime RectTransform ölçü logging.
- LayoutGroup/Fitter çatışmasını düzelt.
- Long Turkish/English localization test.
- Combo metadata'yı ScriptableObject veya canonical skill data'dan doldur.

## Sprint 5 — HUD polish
- Canvas scaler + safe area + UI scale.
- Vitality/skill/minimap prefabları.
- Önce greybox, sonra 9-slice art.

## Sprint 6 — Modal polish
- Reward -> Pause -> Settings -> Codex sırasıyla.
- Ortak panel/button/tab/input control kit.

## Sprint 7 — World rewards ve Egg
- WorldRewardChoiceSet.
- Egg presentation, focus, hatch, reject animations.
- Existing reward types üzerine bağla; yeni economy ekleme.

## Sprint 8 — Final QA
- Resolution/localization/controller/accessibility.
- Scene transitions, save/load, restart run.
- Profiling: canvas rebuild, particles, pooled reward objects.

## Her sprint sonunda
- Değişen dosya listesi
- Before/after screenshot/video
- Test sonuçları
- Kalan risk
- Canonical karar belgesine eklenmesi gereken yeni lock
