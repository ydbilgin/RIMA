# CODEX_DISPATCH — Codex Görev Kuralları
*Claude (orkestra şefi) bu dosyayı okur ve her Codex görev prompt'una ilgili kuralları gömer.
Codex dosyayı doğrudan okumaz — Claude bağlamı besler.*

## Proje
- **RIMA**: 2D isometric roguelite, Unity (C#), TextMeshPro
- **Phase**: 1 (Active) — combat fluidity + UI + assets
- **Branch**: master

## Kritik Kurallar (Her Codex Prompt'una Ekle)
- Model: **gpt-5.5** her zaman (o4-mini yasak)
- Her görev sonunda **commit** et
- Yorum satırı yazma (WHY açık değilse)
- `Time.timeScale` sadece `UIManager` üzerinden yaz
- Hata yönetimi: sadece sistem sınırlarında (user input, external API)

## Temel Dosya Yolları
```
Assets/Scripts/           — tüm C# kaynak kodları
Assets/Scripts/Core/      — MapFragment, RewardPickup, WallOcclusionFader
Assets/Scripts/Combat/    — savaş sistemi, BasicAttack, Skills
Assets/Scripts/UI/        — UIManager, HUDController, SkillBarUI, vb.
Assets/Scripts/Systems/   — FocusSystem, StatusEffectSystem
Assets/Tests/EditMode/    — unit testler (144/144 PASS tutulacak)
Assets/Tests/PlayMode/    — entegrasyon testleri
Assets/Resources/Combat/  — BasicAttackProfile .asset dosyaları
TASARIM/                  — tasarım dokümanları (LOCKED olanları değiştirme)
MEMORY/                   — agent memory dosyaları
```

## Mimari Kısıtlamalar
- `UIManager`: singleton, mutual exclusion (TAB/ESC/SkillOffer), tek timeScale kaynağı
- `BasicAttackProfile`: saf data ScriptableObject — davranış kodu IBasicAttackBehavior'da
- `SkillDraftSystem`: Hades-tarzı 3-seçenek draft, `TriggerDraft(roomNumber)` API
- `RimaUITheme`: tüm renk/boyut sabitleri buradan, baked PNG panel yok

## Güven Döngüsü — ZORUNLU (Her Görevde)
**Implementasyon bitti → commit öncesi bu döngüyü çalıştır. Atlarsan commit etme.**

```
ADIM 1: "Bu implementasyona %100 güvenin var mı?"
ADIM 2: Güven yoksa → açıkları listele → düzelt → ADIM 1'e dön
ADIM 3: %100 güven → commit
```

- 2-3 iterasyon yeterli; fazlası getiri azalır
- "Derleniyor" yeterli değil — her edge case doğrulanmış olmalı
- Test yoksa önce test yaz, sonra döngüyü çalıştır
