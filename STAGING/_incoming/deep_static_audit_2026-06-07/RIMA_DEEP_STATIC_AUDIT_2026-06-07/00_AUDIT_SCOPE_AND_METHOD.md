# 00 — Audit Scope and Method

## Okunan / kullanılan ana kaynaklar

### Canon / durum
- `AI_READER_GUIDE.md`
- `CURRENT_STATUS.md`
- `STAGING/MASTER_PLAN_FINAL_2026-06-06.md`
- `STAGING/GATESLOT_DECISION_2026-06-07.md`
- `STAGING/R4_DECISION_2026-06-07.md`
- `STAGING/PORTAL_PACK_DECISION_2026-06-06.md`

### Koddan incelenen ana dosyalar
- `Assets/Scripts/Systems/Map/RoomSequenceData.cs`
- `Assets/Scripts/Systems/Map/RoomLoader.cs`
- `Assets/Scripts/Environment/Gate.cs`
- `Assets/Scripts/Core/RuntimeRoomManager.cs`
- `Assets/Scripts/Skills/SkillOfferGenerator.cs`
- `Assets/Scripts/Skills/SkillDatabase.cs` kısmi
- `SYSTEM_MAP.md`

### Tasarım/canon drift için kullanılan dosyalar
- `STAGING/chatgpt_weapon_pack/01_CANON_WEAPONS.md`
- uploaded/cached TASARIM doküman özetleri:
  - `GDD.md`
  - `HUD_DESIGN_SPEC.md`
  - eski style/room mechanics belgeleri

## Sınırlar

GitHub connector şu anda bazı uzun dosyalarda satır aralığı/dizin listesi vermiyor. Bu yüzden:
- tüm repo dosya ağacı çıkarılamadı,
- her dosya satır satır okunamadı,
- scene/prefab references canlı doğrulanamadı.

Bu yüzden bu paket kendisini "runtime-certified full audit" diye satmaz. O cümleyi kurarsak biz de yazılım tarihinin küçük yalancılarına katılırız. Gerek yok.

## Bu audit'in gerçek değeri

Claude'a şunu yaptırır:
- live flow'u kanıtla,
- stale docs'u kilitle,
- gate/root collider bug riskini doğrula,
- portal binding'i doğru path'e bağla,
- skill canon drift'i çıkar,
- "sistem var ama kullanılan sistem değil" tuzağını çöz.
