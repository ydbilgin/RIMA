# Codex Task — Clean Test Scene + UIUX Painter Bug Test (S95)

> **Profile:** any active cx profile (Unity açık, MCP bağlı)
> **Effort:** high
> **Output:** `STAGING/CODEX_DONE_clean_test_scene_plus_uiux_bug_test.md`

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

## User Direktifi (S95 LATE NIGHT 2026-05-20)

> "bu kullanımı cidden sen test eder misin bi sorun var seçiyorum wall'u büyük geliyor üste geliyor. wall'lar goru8ndun altına gidiyor. bazen consoleda hata çıkıyor. bunu unitymcpyle test et ayrıca pixellabda ürettiğin gerçek duvarları şu an koy bu eskilkeri kaldır ki doğru test yapalım."

## 3 Bölüm

### Bölüm 1 — Eski Wall'ları Temizle

`PathC_BaseTest.unity` sahnesinde **tüm mevcut wall_* instance'ları sil:**
- Grid altındaki tüm wall_00 / wall_01 / wall_02 / wall_03 / wall_* GameObjects (43 instance)
- Props_Root altındaki tüm wall_* (12 instance)
- Toplam 55 wall instance (Codex Phase A raporundan)
- Statue/mounting/decor instance'larına DOKUNMA — sadece wall_*
- **Sahneyi SAVE ET** (kalıcı temizlik)

### Bölüm 2 — Pilot A 3 Pilot Wall'unu Painter ile Yerleştir

**Mevcut asset'ler (Pilot A finalize sonrası, asset folder'da hazır):**
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_1_face_EW.png` (side billboard)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_2_corner_outer.png`
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_3_arch_opening.png`

**Adım:**

1. **Prefab oluştur** (her PNG için):
   - `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_face_EW.prefab`
   - `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_corner_outer.prefab`
   - `Assets/Prefabs/Walls/pilot_a/pilot_a_wall_arch_opening.prefab`
   - Her prefab: SpriteRenderer + sprite reference (pivot zaten 0.5, 0.0313 Custom)
   - BoxCollider2D auto: WallBlock mode (CollisionResolver'dan)

2. **Painter ile place et** (UIUX implementation v3.1 LIVE — bu test):
   - Painter window menu'den aç (`RIMA/Tools/Unified Painter` veya benzeri)
   - Category: "Duvar" seç
   - Palette'den `pilot_a_wall_face_EW` seç
   - Cell (3, 3) ila (6, 3) arasında 4 hücre boyu paint et (south wall)
   - Cell (3, 3) ila (3, 6) arasında 4 hücre boyu paint et (east wall)
   - Cell (3, 6) köşeye `pilot_a_wall_corner_outer` (1 instance)
   - Cell (4, 5) ortaya `pilot_a_wall_arch_opening` (1 instance, geçit)
   - **Painter Auto-Connect aktif** — wall_* prefix prefab'lar otomatik bağlansın

3. **Sonuç:** Küçük test odası (4×4 cell) ile L-shape duvar + arch
4. **Sahneyi SAVE ET** (kalıcı test zemini)

### Bölüm 3 — UIUX Painter Bug Test

User şikayetleri:
1. Wall seçince **büyük geliyor üste geliyor** (Selected Instance Editor scope/position bug?)
2. Wall'lar **göründüğün altına gidiyor** (sortingLayer/Y-sort bug?)
3. Console'da **bazen hata çıkıyor** (GUILayout? Selection callback? Null reference?)

**Test akışı:**

#### 3.1 Console Log Capture
- Unity Console clear
- Painter window aç-kapat 3 kez
- Wall seç, Panel 5 doldur, "Edit Collider in Scene" tıkla, drag handle test, deselect
- Sahnede başka GameObject seç (statue, ground tile) → Panel 5 davranış
- Console'da TÜM ERROR + WARNING log'larını yakala (stack trace dahil)

#### 3.2 "Wall Büyük Geliyor Üste" Bug Replication
- Sahnede mevcut bir wall'u seç (Panel 5'in çağıracağı durum)
- Panel 5'te wall'un transform.localScale, transform.position, BoxCollider2D.size, BoxCollider2D.offset değerlerini RAPORLA
- "Büyük geliyor" = sprite world scale yanlış mı, pivot ofset mi, collider size mı?
- "Üste geliyor" = transform.position.y yanlış mı, sortingOrder mu yanlış?
- Bug root cause + tahmini fix önerisi

#### 3.3 "Wall Görünmez Altına" Bug Replication
- Pilot A 3 wall'ı sahneye paint'ledikten sonra (Bölüm 2 sonrası)
- Camera Game view'da test: wall'lar görünüyor mu?
- Eğer altında kalıyorsa: sortingLayer ne, sortingOrder ne, Ground tile sortingLayer ne?
- Bug root cause: Painter `CollisionResolver.Resolve` döndüğü `ResolvedCollider.layerName` ne, `sortingOrder` ne?

#### 3.4 Genel Bug Listesi
- Test sırasında tespit edilen tüm bug'ları topla
- Her bug için: replication adımı + console log + root cause + fix önerisi

### Bölüm 4 — Screenshot
- Test odası tam görünür: `STAGING/clean_test_scene_room.png`
- Camera framing: 4×4 test room + 4 wall + arch + corner + ground tile

## Output Format

```markdown
# Clean Test Scene + UIUX Bug Test — Codex Report

## Bölüm 1: Eski Wall Temizliği
- Deleted: 55 wall instance (43 Grid + 12 Props_Root)
- Statue/mounting preserved
- Scene saved: YES

## Bölüm 2: Pilot A Place
- 3 prefab created: pilot_a_wall_face_EW / corner_outer / arch_opening
- Painter place: 4 south wall + 4 east wall + 1 corner + 1 arch
- Painter auto-connect: PASS / FAIL
- Scene saved: YES

## Bölüm 3.1: Console Logs
- Painter open-close 3x: X errors, Y warnings
- Selection switch test: Z errors
- Captured logs:
  - [Error] ... (stack)
  - [Warning] ...

## Bölüm 3.2: "Wall Büyük Üste" Bug
- Replication: ...
- transform values: scale=, position=, collider size=, offset=
- Root cause: ...
- Fix suggestion: ...

## Bölüm 3.3: "Wall Görünmez Altı" Bug
- Replication: ...
- Wall sortingLayer / order: ...
- Ground sortingLayer / order: ...
- Root cause: ...
- Fix suggestion: ...

## Bölüm 3.4: Genel Bug Listesi
- Bug 1: ...
- Bug 2: ...

## Bölüm 4: Screenshot
- STAGING/clean_test_scene_room.png yazıldı

## Açık Sorular
- ...
```

## Hard Constraints

- **Sahneyi SAVE ET** (Bölüm 1 + 2 — kalıcı test zemini, user istedi).
- **Auto-commit YOK** — user manual commit.
- **Sadece wall_* instance'ları sil** — statue/mounting/decor preserve.
- **Pilot A 3 wall yeterli** — eski 5 wall PNG (act1_wall_*) kullanma (drift kaynak).
- **Bug fix UYGULAMA** — sadece liste + root cause + öneri. User karar verecek hangileri fix dispatch.
- **BLOCKED if unclear:** Painter window erişim sorunu, sahne kapalı, prefab create fail vb. STOP.
