# KARAR — ChatGPT Deep Static Audit Değerlendirmesi (2026-06-07)

**Council:** cx (LIVE_FLOW_PROOF + olgusal denetim) + ax-3.1-Pro (mimari) + ax-3.5-Flash (lean) → Opus sentez.
**Girdi:** `STAGING/_incoming/deep_static_audit_2026-06-07/` (15 bulgu, 7-commit patch planı).
**Kanıt dokümanı:** `STAGING/audit/LIVE_FLOW_PROOF_2026-06-07.md` (cx, dosya:satır kanıtlı).

## ANA SONUÇ: Canlı yol KESİNLEŞTİ — `TEMPLATE_BUILDER_LIVE`
MainMenu → CharacterSelect (chamber bootstrap) → **_Arena** → `RoomRunDirector.BeginRun` → `IsoRoomBuilder.Build + BuildExitDoors` (NW/N/NE gate-slot) → `RoomRunExitDoorTrigger` → DungeonGraph child-choice. **RoomLoader / RoomSequenceData / Gate.cs / GateBehavior / DoorTrigger hiçbir canlı sahnede yok** — tamamı legacy. Audit'in F-001 "blocker" korkusu çürüdü; ama "kanıtla, sonra kod yaz" disiplini doğruydu ve kanıt artık kalıcı dokümanda.

## Audit'in GÖREMEDİĞİ, cx'in BULDUĞU 2 gerçek sorun
1. **🐛 Ranger skill retire bug'ı:** `IsRetiredOfferSkill` (SkillDatabase.cs:595-607) Aimed Shot / Disengage / Multi Shot'ı offer havuzundan DIŞLIYOR — ama NLM'e göre bunlar Ranger'ın GÜNCEL canonical skill'leri (revoked olanlar Shadowblade'in eski isimleri: Backstab/Shadow Step/Fan of Knives). Demo'da Ranger oynanabilir sınıf → draft havuzu sakatlanmış olabilir. + CharacterSelect fallback UI revoked Shadowblade isimlerini hâlâ gösteriyor (CharacterSelectScreen.cs:1559-1582).
2. **⚠️ Event node sızıntısı:** `DungeonGraph` runtime'da `Event` tipi node üretebiliyor (DungeonGraph.cs:121-149) ama demo portal tipleri Combat/Elite/Reward/Boss — Event portalının rünü/art'ı yok.
3. (Bonus) `RuntimeRoomManager`'ın mevcut `[Obsolete]` mesajı yanlış halefi işaret ediyor (RoomLoader'ı — o da legacy!).

## Bulgu-bazlı nihai karar (3 danışman uzlaşısı + Opus)

| F | Karar | Gerekçe (kanıt LIVE_FLOW_PROOF'ta) |
|---|---|---|
| F-001 | **FALSE-POSITIVE** | _Arena'da RoomRunDirector+IsoRoomBuilder; RoomLoader hiçbir canlı sahnede yok |
| F-002 | **PARTIAL→T3 içinde** | Gate.cs root-scale gerçek ama LEGACY; canlı ExitDoor'a T3'te animasyon eklerken kural: SADECE child visual scale |
| F-003 | **ACCEPT** | SYSTEM_MAP (son güncelleme 2026-04-29!) RuntimeRoomManager+N/S/E/W kapı anlatıyor — banner şart |
| F-004 | **PARTIAL** | isImplemented leak = FALSE (iki path filtreli); AMA retire-list canon çelişkisi GERÇEK (yukarıda #1) |
| F-005 | **ACCEPT = T3'ün kendisi** | Canlı görsel: her sokete aynı `gateNorthSprite` (IsoRoomBuilder.cs:805-818) — açı/tip yok |
| F-006 | **FALSE-POSITIVE** | Branching choice-index canlı (RoomRunDirector.cs:969-985, 1032-1040) |
| F-007 | **MODIFY** | Derin Gate/RiftPortalView refactor POST-DEMO; T3 = mevcut ExitDoor'a minimal skin-binding (3/3 uzlaşı) |
| F-008 | **ACCEPT+genişlet** | Doc notu + **Event node guard** (demo'da Event üretme veya Combat'a map'le) |
| F-009 | **PARALEL** | Silah ChatGPT turu zaten çözüyor; sonuç=WEAPON_PRODUCTION_OVERRIDE dokümanı |
| F-010/011/012 | **ACCEPT-hafif** | CURRENT_STATUS kazanır notları + HUD spec "pending" etiketi + asset statü manifest'i (portalpack task'ında zaten var) |
| F-013/015 | **BÜYÜK ÖLÇÜDE BAYAT** | fig01-05 bugün yeniden çekildi; portal 1/2-exit in-game kanıtı var; kalan tek şey kullanıcı beğeni onayı |
| F-014 | **ACCEPT+düzelt** | [Obsolete] guard VAR ama mesajı stale — RoomRunDirector/_Arena'yı işaret edecek |

## UYGULAMA PLANI (7 commit → 4 iş, ~1.5-2 saat)

**A1 — Stale-guard + legacy mühür paketi [S, ~20dk]:** SYSTEM_MAP başına STALE banner · GDD/ROOM_MECHANICS/CLASS_SILHOUETTE_BIBLE'a "AI_READER_GUIDE kazanır" notu · RoomLoader+Gate.cs+GateBehavior+DoorTrigger'a LEGACY banner · RuntimeRoomManager [Obsolete] mesajını düzelt · AI_READER_GUIDE'a "Legacy dosyalar" bölümü. **T3'ün ÖN KOŞULU (tek zorunlu gate — 3/3 uzlaşı).**
**A2 — T3 portal wiring [M, cx]:** Hedef KESİN: `IsoRoomBuilder.BuildExitDoors/CreateExitDoorObject` + _Arena serialized sprite ref'leri. N=frontal · NW=açılı · NE=açılı+flipX (rün/badge ters dönmez) · tip-bazlı skin (Combat/Elite/Chest/Boss) · açılış animasyonu SADECE child visual'da · choice-index zinciri korunur · `TASK_portalpack_production` manifest/guide işleri dahil. Test: 1/2/3-exit + boss-center + flipX.
**A3 — Skill canon fix [S-M]:** Retire listesi düzeltmesi (Ranger canonical'ları offer'a döner — NLM teyitli) + revoked Shadowblade görünümleri temizlenir + canon snapshot testi.
**A4 — Event node guard [S]:** Demo graph'ında Event üretimi kapat/map'le + AI_READER F-008 notu.

## KULLANICI ONAYI (2026-06-07): SIRA = A1 → A3 → A4 → A2
Gerekçe (kullanıcı): A1 yanlış yönlendirmeyi kapatır (T3 şartı) · A3 = Ranger draft havuzunu etkileyen GERÇEK demo bug'ı, oynanış doğruluğu polish'ten önce · A4 Event sızıntısı T3 skin tablosuyla çakışmadan temizlenmeli · A2 en son, IsoRoomBuilder live path'e bağlanır.

### A2 KESİN KURALLARI (kullanıcı dikte etti — dispatch brief'ine AYNEN girer)
- RoomLoader/Gate.cs path'ine DOKUNMA — legacy.
- Hedef: `IsoRoomBuilder.CreateExitDoorObject`.
- N=frontal, NW=angled, NE=angled flipX.
- Rune/badge/label FLIP EDİLMEZ.
- Portal tipleri: Combat, Elite, Chest/Reward, Boss. Heal/Lore YOK.
- Event YOK veya demo-supported tipe map edilir (A4 hallediyor).
- Animasyon SADECE child visual'da; root/collider scale DEĞİŞMEZ.

**RED (audit'ten):** audit-utility tool yazımı (tek seferlik kanıt yeterli oldu) · LiveFlowProof'u T3 blocker yapmak (doküman zaten üretildi) · Gate/RiftPortalView derin refactor'ı (post-demo) · 6 dosyadan fazlasına banner yayma.
**Audit'in RED LIST'i AYNEN BENİMSENDİ** (Heal/Lore portal yok, full-wall yok, 8-yön portal yok, boss redesign yok vb.).
