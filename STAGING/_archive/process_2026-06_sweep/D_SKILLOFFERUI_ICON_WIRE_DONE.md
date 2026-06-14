# Track D — SkillOfferUI Icon Wire DONE
Date: 2026-05-28 | Author: Sonnet (direct write, no dispatch)

## Ne Değişti

### `Assets/Scripts/UI/SkillOfferUI.cs` — 3 surgical edit

| Satır (orijinal) | Değişiklik |
|---|---|
| L260 — `BuildSkillCard` imzası | `Sprite icon = null` opsiyonel parametre eklendi |
| L289-291 — icon Image bloğu | `if (icon != null) { sprite = icon; color = white }` else placeholder korundu |
| L219 — `BuildRewardCard` çağrısı | `offer.skill?.icon` geçiliyor |
| L78 — `ShowReplaceMode` çağrısı | `sd.icon` geçiliyor |

## Icon Kaynağı Nasıl Bağlandı

`SkillData` (RIMA namespace, `Assets/Scripts/Skills/SkillData.cs`) zaten `public Sprite icon` alanına sahip.  
7 serialized `.asset` dosyası (`Assets/Data/Skills/*.asset`) icon GUID'lerini zaten taşıyor — Unity runtime'da bu asset'lerden türetilen `SkillData` örnekleri `icon != null` döner.

`SkillDatabase.Build()` runtime'da `ScriptableObject.CreateInstance<SkillData>()` ile oluşturduğu örneklere icon ataması yapmıyor → bu yolla gelen skill kartları **fallback gray placeholder** gösterir (mevcut davranış korundu, regression yok).

19 `Assets/Sprites/UI/Icons/*.png` dosyası ile eşleştirme bu PR'ın dışında — SkillDatabase'e icon yükleme eklemek Track D scope'u aşar.

## Compile Durumu

Unity MCP `read_console` → **0 error** (compile sonrası doğrulandı).

## Manuel Verify Adımı

1. Play Mode'a gir veya Draft/Reward trigger'ı tetikle.
2. SkillOfferUI kartlarını aç.
3. `Assets/Data/Skills/*.asset` tabanlı skill'lerin (örn. Gravity Cleave, Whirlwind Slash) icon slotunda karşılıklı PNG görünmeli.
4. SkillDatabase runtime skill'leri (örn. Iron Charge, Cleave) hâlâ gri placeholder gösterir — beklenen davranış.
5. `ShowReplaceMode` (slot dolu swap ekranı) için de aynı test — `currentActives[i].icon` artık karta geçiliyor.
