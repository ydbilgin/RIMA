# 02 — Findings by Severity

## P0 / Blocker

### F-001 — Live path mismatch: RoomLoader tek gate vs gate-slot portal sistemi

**Kanıt:**
- `RoomSequenceData.cs` sadece `gatePosition` ve `gateSize` alanı taşıyor.
- `RoomLoader.cs` `RoomSequenceData[] _sequence` kullanıyor.
- `RoomLoader.BuildRoomContent` tek `Gate_Room{index}_Exit` oluşturuyor.
- `Gate.OnPlayerEntered` doğrudan `LoadNext()` bağlı.

**Canon:**
- `AI_READER`: fiziksel kapı yok, floating island + Rift portal var.
- `GATESLOT_DECISION`: 1→N, 2→NW+NE, 3→NW+N+NE.
- `R4_DECISION`: N=frontal, NW=angled, NE=runtime flipX.

**Risk:**
T3 portal asset'i yanlışlıkla eski tek-gate path'e makyaj olarak bağlanır.

**Kesin yapılacak:**
Claude önce `LiveFlowProof` üretmeli. Kod yazmak sonra.

---

### F-002 — Gate root scale animasyonu collider stabilite garantisini bozabilir

**Kanıt:**
RoomLoader yorumları root/collider unscaled kalmalı diyor.
Gate.cs open anim root `transform.localScale` değiştiriyor.

**Risk:**
- Trigger collider dünyada küçülür/büyür.
- Player transition penceresi kayabilir.
- Portal visual ve trigger aynı anda squash olur.
- "root stays scale 1" kabulü bozulur.

**Fix:**
- Root scale asla değişmez.
- Open anim sadece `GateVisual` / `RiftPortalView` child transform'da oynar.
- Regression test şart.

---

### F-003 — SYSTEM_MAP stale ve yanlış path'e yönlendiriyor

**Kanıt:**
SYSTEM_MAP:
- RuntimeRoomManager ana lifecycle
- DoorTrigger
- GateBehavior
- N/S/E/W physical door
- Wall tile placed/removed

AI_READER:
- Physical door revoked
- Wall-heavy revoked
- Rift portal canon

**Fix:**
SYSTEM_MAP başına büyük uyarı:
```md
> STALE / SUPERSEDED. Live flow için CURRENT_STATUS + AI_READER_GUIDE kazanır.
```

---

### F-004 — SkillDatabase canonical drift + placeholder leak riski

**Kanıt:**
`SkillOfferGenerator.GetSource()` fallback path'inde `isImplemented` filtreliyor.
Ama `SkillDatabase.Instance != null` ise doğrudan `SkillDatabase.Instance.GetPool(...)` kullanıyor.
`SkillDatabase.GetPool()` full okunamadı; isImplemented filtresi doğrulanmalı.

**Risk:**
- Placeholder skill draft'a sızabilir.
- Revoked/old skill isimleri draft'a çıkabilir.

**Fix:**
- `SkillDatabase.GetPool` inspect.
- Test: `isImplemented=false` hiçbir scenario'da offer'a düşmez.
- Skill canon snapshot test.

---

## P1 / High

### F-005 — PortalSkin / DoorPortal live binding kanıtı yok

**Kanıt:**
R4 `PortalSkin.frameFrontal/frameAngled` diyor.
RoomLoader snippet generic `Environment/Gate/gate_arch` kullanıyor.

**Risk:**
Asset pack onaylansa bile oyunda generic arch kalır.

**Fix:**
- `PortalSkinSO` veya table.
- Socket visual mapping:
  - N → frontal
  - NW → angled
  - NE → angled flipX
- Portal type mapping:
  - Combat / Elite / Chest / Boss

---

### F-006 — RoomLoader lineer LoadNext, branch choice ile çelişebilir

**Kanıt:**
RoomLoader `LoadNextInstance()` sequence index +1 yapıyor.

**Risk:**
Rapor/oyun "branching route choice" der ama live path lineer olabilir.

**Fix:**
- Eğer branch live değilse rapor dilini yumuşat.
- Eğer branch live ise choice-index router kanıtla.
- T3 gate/portal click -> child node mapping test.

---

### F-007 — Gate.cs semantik olarak eski gate, yeni Rift portal değil

**Kanıt:**
Gate.cs:
- placeholder sprite
- room category tint
- Bridge/Ritual/Treasure mapping
- old "gate canonical spec 8 variant"

**Risk:**
Yeni portal sadece eski gate üzerine kozmetik olur.

**Fix:**
- Gate = trigger/state logic
- RiftPortalView = visual/skin/pulse/rune
- Root logic ile visual ayrımı.

---

### F-008 — GDD/Room mechanics future oda tipleri portal üretimini şişirebilir

**Risk:**
Spirit/Shop/Curse/Event gibi full-game oda tipleri tekrar portal type olarak döner.

**Fix:**
Demo note:
```md
Demo portal types: Combat / Elite / Chest(Reward) / Boss.
Other room types are future/full-game reference.
```

---

### F-009 — Weapon canon boyut/pivot güncellemesi şart

**Kanıt:**
`01_CANON_WEAPONS`:
- Warblade 256→192 trim, Unity final 96×96.
- Pivot merkez pixel olmalı diyor.
Son üretim mantığı:
- target-size safer
- pivot = grip point

**Risk:**
Silah üretimi downscale/pivot hatasıyla başlar.

**Fix:**
`WEAPON_PRODUCTION_OVERRIDE_2026-06-07.md`:
- target-size output
- horizontal-right convention
- pivot = real grip
- PPU64 Point
- no variants

---

## P2 / Medium

### F-010 — MASTER_PLAN task state stale olabilir
MASTER_PLAN sıra için iyi, ama CURRENT_STATUS task state kazanmalı.

### F-011 — HUD_DESIGN_SPEC pending, canonical değil
HUD spec "onay bekleniyor"; UI tasklarında reference ama canon sayılmasın.

### F-012 — Deprecated/optional portal assets statü standardı eksik
Elite v1 deprecated, boss angled optional, Reward/Boss rune needs review.

### F-013 — Rapor figürleri hala gating
fig01-05 kesin temiz ScreenshotMode ile alınmadan final docx kapanmasın.

### F-014 — RuntimeRoomManager silinmesin ama legacy guard olmalı
Büyük obsolete sistem. Silme riskli; guard şart.

### F-015 — Portal contact sheet PASS değil
In-game 1/2/3 exit screenshot olmadan asset sheet "production-ready" sayılmaz.
