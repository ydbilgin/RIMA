# 05 — Claude Deep Audit Prompt

Aşağıdaki prompt'u Claude'a ver. Kod yazmaya başlamadan önce rapor üretmesini zorunlu kıl.

```text
RIMA reposunda derin audit yap.

Önce repo kökündeki AI_READER_GUIDE.md dosyasını oku. Oradaki okuma sırasına ve REVOKED listesine uy.

Bu audit'in amacı yeni özellik yazmak değil:
1. live room/portal flow'u kanıtlamak,
2. eski-yeni sistem çakışmalarını bulmak,
3. stale docs'u işaretlemek,
4. portal wiring'in doğru path'e bağlanmasını garanti etmek,
5. skill/weapon canon drift'ini temizlemek.

Kırmızı çizgiler:
- Physical door sistemine dönme.
- Full wall dungeon kurma.
- Heal/Lore portal üretme.
- 8 yön portal üretme.
- Entry portal objesi ekleme.
- Done taskları tekrar yapma.
- CURRENT_STATUS task state için kazanır.
- AI_READER canonical drift için kazanır.

AŞAMA 1 — LIVE FLOW PROOF
Şu dosyaları/scene wiring'i kanıtla:
- Main scene aktif room manager kim?
- RoomLoader live mı?
- RuntimeRoomManager live mı yoksa obsolete/legacy mi?
- RoomTemplateSO/IsoRoomBuilder path live mı?
- Gate/portal spawn eden gerçek method hangisi?
- Choice-index/branch geçişi hangi event ile çalışıyor?

Çıktı:
STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md

AŞAMA 2 — GATE ROOT SCALE BUG
Kontrol et:
- RoomLoader root collider sabit varsayıyor mu?
- Gate.OpenAnimCoroutine root scale değiştiriyor mu?
- Bu collider bounds'u bozuyor mu?

Eğer bozuyorsa:
- animasyonu visual child'a taşı,
- root scale sabit kalacak test ekle.

AŞAMA 3 — PORTAL LIVE BINDING
R4 kararına göre:
- N = frontal
- NW = angled
- NE = angled flipX
- Portal types = Combat / Elite / Chest / Boss

Kontrol:
- PortalSkin/PortalSkinSO var mı?
- DoorPortal prefab var mı?
- BuildExitDoors/RoomLoader live path bunu kullanıyor mu?
- Generic gate_arch hâlâ live visual mı?

AŞAMA 4 — DOC STALE GUARDS
Aşağıdaki dosyalarda stale/future/pending guard ekle:
- SYSTEM_MAP.md
- TASARIM/GDD.md
- TASARIM/CLASS_SILHOUETTE_BIBLE.md
- TASARIM/STYLE_BIBLE.md
- TASARIM/ROOM_MECHANICS.md
- STAGING/MASTER_PLAN_FINAL_2026-06-06.md

AŞAMA 5 — SKILL CANON AUDIT
SkillDatabase içindeki tüm skillleri tabloya dök:
- skillName
- classType
- isImplemented
- draft-visible?
- canonical status
- action

Özellikle eski isimler:
Backstab, Shadow Step, Fan of Knives, Aimed Shot, Disengage, Multi Shot.

AŞAMA 6 — WEAPON PRODUCTION OVERRIDE
01_CANON_WEAPONS.md içindeki büyük-canvas/center-pivot kararı ile live üretim pratiğini karşılaştır.
Gerekirse ayrı override dosyası yaz:
- target-size
- horizontal-right
- grip pivot
- PPU64 Point
- no variants

ÇIKTI FORMAT:
A) Kesin doğrular
B) Blocker hatalar
C) Doğrulanması gereken şüpheler
D) Patch planı, commit sırasıyla
E) Test planı
F) Yapılmayacaklar
G) Gereksiz/fazlalık/legacy matrix

İlk commit kod değil audit raporu olsun. Kod yazmadan önce live flow proof istiyorum.
```
