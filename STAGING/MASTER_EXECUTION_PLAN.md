# RIMA Master Execution Plan (S114, 2026-05-28)

**Status:** LIVE — tek kanonik master plan. Opus rima-design sentezi (agy sanity + Opus direct source-read verify). Codex dispatch profile-race ile takıldı (laurethgame cred-blob contention, `feedback_codex_agy_profile_race`); 4 teknik soru Opus tarafından kaynak kodu DOĞRUDAN okunarak doğrulandı — Codex'ten daha güçlü kanıt. NO CODE WRITTEN — plan only.

**Amaç:** Projenin BÜTÜN açık işlerini tek sıralı plana koymak. Faz 1 demo kritik path + paralel track'ler + gate'ler + efor/sahip. Demo, Track A + playtest dışında HİÇBİR şeye bağımlı değil — gerisi gate-fill.

---

## 0. Yönetici Özeti (30 saniye)

- **Demo'ya giden TEK seri zincir = Track A combat.** A2 mount → A3 timing → A4 juice → ⛔A5 freeze → art fan-out → ⛔D3 playtest → demo loop. Başka her şey bunu beklerken koşan FILL.
- **Faz 0 = baseline temizlik:** cliff black-layer fix (doğrulandı, kök neden kesin) + A1 WeaponDatabase orphan temizlik (dikkatli, A2 referansını bozmadan).
- **⛔ İki gate kullanıcı-manuel:** A5 (combat feel kilidi) + D3 (integrated playtest). Bir de izole **git-push GATE** (kullanıcı kararı, plan onu beklemez).
- **Art (anim+weapon sprite+VFX) A5'ten ÖNCE BAŞLAMAZ** — LOCK, asart churn riski %100 gerçek (3 bağımsız kaynak + agy hemfikir).
- **T3 tool, decor/parallax, asset hygiene = paralel FILL**, write-disjoint, gate beklerken koşar, demo'yu BLOKLAMAZ.

---

## 1. Faz Yapısı + Çıkış Kriterleri

### FAZ 0 — Baseline Temizlik (yarım gün, A'ya paralel başlanabilir)
Demo'nun görsel ve veri temelini düzeltir. A zinciriyle dosya-disjoint, paralel koşar.

| İş | Açıklama | Efor | Kim |
|---|---|---|---|
| **0.1 Cliff black fix P0** | Light2D `m_ApplyToSortingLayers`'a `Decor_Cliff`(12)+`Decor_Floor`(13) ekle (Global + 4 autolight). Layer'a DOKUNMA — sorun ışık gitmemesi. | S | Sonnet write + Codex review |
| **0.2 Cliff floating-look P1** | Inactive `RimLight_*_Cyan` + `Brazier_*_WarmLight` aktive et, target'larına `Decor_Cliff` ekle. Cyan-rim brand. | S | Sonnet |
| **0.3 Cliff P2 runtime bug** | `DirectionalCliffTile.GetTileData` yön çözümü `#if UNITY_EDITOR`(37-78) dışına taşı. ⚠️ Bkz Bağımlılık-4. | S | Sonnet write + Codex review |
| **0.4 A1 orphan sil** | `WeaponDatabaseSO.asset` orphan → sil. ⚠️ Bkz Bağımlılık-5 (A2 referansını DOĞRULA önce). | S | Sonnet |

**Çıkış kriteri:** Play mode'da cliff'ler ışıklı (cyan-rim + amber fill, siyah değil), doğru yön yüzü gösteriyor; `execute_code` "active lights targeting Decor_Cliff > 0"; orphan asset silinmiş, hiçbir referans kırılmamış (Player.prefab + scene compile temiz).

---

### FAZ 1a — Combat Çekirdek (KRİTİK SERİ ZİNCİR, 2-3 gün)
Demo'nun tek gerçek bağımlılık zinciri. Her adım bir öncekinin üstüne kuruluyor.

| Sıra | İş | Açıklama | Efor | Kim |
|---|---|---|---|---|
| **A2** | Mount bridge | `HandAnchorAttach` + `OrientationSync.Sync(FacingDir8)` wire. `WeaponSorter`→sil. Player.prefab `bodyRenderer` + canonical `handOffsets[]` doldur. | M | Sonnet write + Codex review |
| **A3** | Graybox timing | `MeleeChainBehavior` + `BasicAttackProfile` startup/active/recovery/cancel pencereleri. `CombatEventBus.PublishHit/Kill` çağrılarını DOĞRU frame'lere yerleştir. | M | Codex write + Sonnet review |
| **A4** | Juice wire | `CombatEventBus` subscriber'lar + scene/prefab juice + dash. Hit pause + shake + freeze. **+ SFX placeholder + VFX event-hook placeholder** (agy catch — bkz 1b notu). | M | Sonnet write + Codex review |

**A2/A3 paralel notu:** Dosya-disjoint (Bağımlılık-1). Teorik paralel yazılabilir AMA seri tut — A3 timing'i A4 juice'un üstüne kuruluyor, A2 mount görsel doğrulama için A3'ten önce gerekli. Seri zincir = düşük senkron maliyeti. agy de seri öneriyor.

**A3↔A4 seam:** `CombatEventBus` static bus ZATEN LIVE (`PublishHit/Kill/Dash` + 6 event mevcut, `Assets/Scripts/Combat/CombatEventBus.cs`). A3 publish çağrılarını yazar, A4 subscribe eder. Event kontratı dondurulduktan sonra A4 büyük ölçüde A3'e paralel koşabilir — ama final tuning A5'i bekler.

**⛔ A5 — TIMING-FREEZE GATE (kullanıcı-manuel):** Kullanıcı combat feel'i onaylar. Bu nokta combat değerlerini (frame timing, juice şiddeti, hitbox) KİLİTLER. A5 onayı GELMEDEN art üretimi başlamaz.

**Çıkış kriteri (Faz 1a):** Warblade graybox combo oynanabilir, mount silah elde, juice hissediliyor; kullanıcı A5'te "feel kilitlendi" onayı verdi.

---

### FAZ 1b — Art Fan-Out (A5 SONRASI, paralel, ~3-5 gün)
A5 freeze'den sonra başlar. Locked timing'e göre net frame sayıları üretilir → churn yok.

| İş | Açıklama | Efor | Kim |
|---|---|---|---|
| **B anim** | Warblade 11 anim + Apex state. Faz 1 UCUZ başla: Idle 4f (1 gen/dir). Weaponless body. | L | **User-manual (PixelLab Web UI)** — MCP otonom YASAK |
| **C weapon sprite** | Warblade silahları, PPU=64, 8-dir (5 üret + 3 mirror flipX), HIGH TOP-DOWN 3/4. | M | User-manual (PixelLab Web UI) |
| **D VFX** | Cyan hitspark + dash trail (painterly layer). A4'teki event-hook'lara bağlanır. | M | User-manual + Sonnet wire |

**LOCK:** B + C, A5 freeze'den ÖNCE BAŞLAMAZ (NLM WEAPON_ANIM_VFX_PRODUCTION_LOCK). A4'te VFX/SFX event-hook placeholder hazır olmalı (agy catch) ki D ekleneceğinde koda geri dönüş olmasın.

**Çıkış kriteri:** Warblade combat görsel olarak tamamlanmış (anim + silah + VFX bağlı), graybox kutular kalkmış.

---

### FAZ 1c — Demo Polish + Playtest (~1-2 gün)

| İş | Açıklama | Efor | Kim |
|---|---|---|---|
| **5 oda + 4 mob + Fragment + Gate** | Demo scope; room-transition loop ZATEN LIVE. İçerik doğrulama + tuning. | M | Sonnet |
| **⛔ D3** | INTEGRATED PLAYTEST GATE (kullanıcı-manuel). 10 dk loop end-to-end. | — | User-manual gate |
| **demo combat loop** | D3 PASS → Faz 1 milestone demo kapanış. | S | — |

**Çıkış kriteri:** Warblade tek class + 5 oda + 4 mob + Map Fragment + Gate, ~10 dk loop, kullanıcı D3'te onayladı. **= FAZ 1 DEMO TAMAM.**

---

## 2. Sıralı Kritik Path (tek zincir, gate'li)

```
FAZ 0 (paralel, demo'yu bloklamaz)
  0.1 cliff P0 → 0.2 P1 → 0.3 P2 ; 0.4 orphan sil
          │
          ▼
A2 mount bridge ──► A3 graybox timing ──► A4 juice wire (+SFX/VFX hook placeholder)
                                                  │
                                                  ▼
                                         ⛔ A5 TIMING-FREEZE  (kullanıcı-manuel)
                                                  │
                          ┌───────────────────────┼───────────────────────┐
                          ▼                        ▼                       ▼
                       B anim                 C weapon sprite           D VFX
                     (PixelLab manuel)        (PixelLab manuel)      (manuel+wire)
                          └───────────────────────┼───────────────────────┘
                                                  ▼
                                    5 oda + 4 mob + Fragment + Gate polish
                                                  │
                                                  ▼
                                         ⛔ D3 INTEGRATED PLAYTEST  (kullanıcı-manuel)
                                                  │
                                                  ▼
                                          DEMO COMBAT LOOP (Faz 1 tamam)
```

**İzole düğüm (zincir dışı):** ⛔ **git-push GATE** — kullanıcı kararı. Plan bunu BEKLEMEZ (bkz §7).

---

## 3. Paralel Track Tablosu (gate beklerken FILL)

| Track | İş | Ne zaman koşar | Efor | Writer | Write-disjoint? |
|---|---|---|---|---|---|
| **B (engine tool)** | T3 F1-F7, ~1280 LOC | A5/D3 gate beklerken; combat'tan tamamen bağımsız | L (5-7 gün) | Codex (C1/C2/C7/C10) + Sonnet (gerisi), rotation | ✅ Combat dosyalarına dokunmaz. ⚠️ C10 RoomLoader.OnRoomLoaded'a self-bootstrap (scene edit YOK) — bkz Bağımlılık-3 |
| **C (decor/parallax)** | P1-P3: SceneView kamera hook / ParallaxRig paneli / factor-link | Gate beklerken; combat'tan bağımsız | M | Sonnet | ✅ `Background/ParallaxLayer.cs`, `Inspector/Sections/ParallaxSection.cs` — combat & T3 ile disjoint |
| **D (asset hygiene)** | #41 PixelLab sentez (243 obje, 3-AI analiz DONE) | Anytime | M | Sonnet (sentez) | ✅ Sadece STAGING + asset meta |
| **D (asset hygiene)** | statue 12 prefab AssetCategory backfill | Anytime | S | Sonnet | ✅ Prefab meta, izole |
| **D (asset hygiene)** | #42 delete (51 sprite DELETE adayı) | #41 sentez SONRASI | S | Sonnet (user onayı ile) | ✅ İzole, ama delete = geri dönülmez → user onayı |

**T3 dilimleme (agy önerisi — ADOPTE):** T3'ü tek 5-7 günlük blok yapma. **T3-MVP (F1+F2, ~1 gün, JSON+registry foundation)** gate-fill'de yap; **T3-Polish (F3-F7)** combat demo kapandıktan SONRA. Gerekçe: D3 playtest'ten combat feedback gelirse T3'ün ortasında context-switch maliyeti. F1+F2 foundation kısmi başarıda bile değerli (LiveRoomReloader Editor-side serializer'dan okuyabilir = T2 fallback).

---

## 4. Bağımlılık Uyarıları (doğrulandı — Opus direct source-read)

**Bağımlılık-1 — Track A iç ownership (Soru 1, DOĞRULANDI):**
A2 dosyaları [`OrientationSync.cs` (ns RIMA.Combat), `HandAnchorAttach.cs` (ns RIMA)] ile A3 dosyaları [`MeleeChainBehavior.cs`, `BasicAttackProfile.cs`] **disjoint** — birbirine refere ETMİYOR. A4 `CombatEventBus.cs` ayrı static bus, ZATEN LIVE. Çakışma yok. A2↔A3 teknik paralel olabilir ama design gereği seri tutuldu.

**Bağımlılık-2 — T3 C4 compile-blocking (Soru 2):**
C4 = `RuntimeAssetRegistry.cs` (henüz YOK). İddia: C6/C9/C10 C4'e hard-blocked; C5/C7/C8/C11 C4-stub'a paralel. C4 küçük tip (class + `Get(guid)`); stub imza ile paralel yazım MÜMKÜN. T3 doc faz planı (F2 önce C4) tutarlı. (Codex bu soruyu doğrulayamadan takıldı; spec yapısı + LOC tablosu desteğiyle kabul.)

**Bağımlılık-3 — Scene-save çakışması (Soru 3, DOĞRULANDI):**
`RoomLoader.cs:16` → `public static event Action<RoomConfig, GameObject> OnRoomLoaded` GERÇEKTEN var. C10 (LiveRoomReloader) bu static event'e **runtime self-bootstrap** (AddComponent at startup) ile abone olur — `PlayableArena_Test01.unity` scene dosyasını MODIFY ETMEZ. Dolayısıyla **A4 (juice scene/prefab save) ile C10 arasında scene-save merge conflict riski YOK** (C10 self-bootstrap kuralı uygulanırsa). LOCK: C10 self-bootstrap olsun, scene'e elle eklenmesin.

**Bağımlılık-4 — Cliff P2 #if guard (Soru 4, DOĞRULANDI):**
`DirectionalCliffTile.cs` satır 37-78 `#if UNITY_EDITOR` içinde; Play'de satır 35 fallback (hep güney) çalışıyor. İç gövde sadece runtime-safe API kullanıyor (`Tilemap.HasTile`, `CliffAutoPlacer.floorTilemap`) — Editor-only çağrı YOK → guard kaldırılabilir, derlenir. ⚠️ **CAVEAT:** Blok `Object.FindObjectOfType<CliffAutoPlacer>()` çağırıyor — runtime'da sahnede `CliffAutoPlacer` + atanmış `floorTilemap` YOKSA satır 42 erken return → yine güney fallback. Fix sadece `#if` kaldırmak DEĞİL; runtime'da placer (veya baked komşu verisi) erişilebilir olmalı. 0.3 task'ı bunu içermeli, yoksa bug yarım çözülür.

**Bağımlılık-5 — A1 orphan sil sırası (yeni Opus bulgu):**
`HandAnchorAttach.weaponDatabase` alanı `WeaponDatabaseSO` tipinde (`HandAnchorAttach.cs:18,68`). A1 kararı: canonical *asset* = `Resources/WeaponDatabase.asset`, orphan = `WeaponDatabaseSO.asset`. **0.4 delete ÖNCE doğrula:** Player.prefab'taki `HandAnchorAttach` hangi asset instance'ına işaret ediyor? Orphan'a işaret ediyorsa silmek referansı kırar → önce canonical'a yeniden bağla, SONRA sil. A2 mount bridge bu referansa bağımlı.

**Bağımlılık-6 — profile race (operasyonel):**
agy + Codex AYNI `laurethgame` profile'ı kapıştı (S114 bu dispatch'te Codex takıldı, `feedback_codex_agy_profile_race` doğrulandı). Paralel dispatch'lerde AYRI profile zorunlu VEYA sıralı. Combat (Codex review) + T3 (Codex write) aynı anda dispatch edilecekse farklı account ver.

---

## 5. Efor + Sahip Özeti

| İş | Efor | Sahip | Gate? |
|---|---|---|---|
| 0.1-0.2 cliff lighting | S | Sonnet+Codex | — |
| 0.3 cliff P2 + placer caveat | S | Sonnet+Codex | — |
| 0.4 orphan sil | S | Sonnet | — |
| A2 mount bridge | M | Sonnet+Codex | — |
| A3 graybox timing | M | Codex+Sonnet | — |
| A4 juice + SFX/VFX hooks | M | Sonnet+Codex | — |
| **A5 timing-freeze** | — | **USER** | ⛔ |
| B anim | L | User-manual | A5 sonrası |
| C weapon sprite | M | User-manual | A5 sonrası |
| D VFX | M | User-manual+Sonnet | A5 sonrası |
| 5 oda/4 mob polish | M | Sonnet | — |
| **D3 playtest** | — | **USER** | ⛔ |
| T3-MVP (F1+F2) | M | Codex+Sonnet | FILL |
| T3-Polish (F3-F7) | L | rotation | demo sonrası |
| decor/parallax P1-P3 | M | Sonnet | FILL |
| #41 PixelLab sentez | M | Sonnet | FILL |
| statue backfill | S | Sonnet | FILL |
| #42 delete | S | Sonnet+user | FILL |
| **git-push** | — | **USER** | ⛔ izole |

---

## 6. Şimdi Başlanabilir İlk 5 Aksiyon (somut)

1. **0.1 cliff P0 fix dispatch** — Sonnet'e: Light2D `m_ApplyToSortingLayers`'a `Decor_Cliff`(12)+`Decor_Floor`(13) ekle (Global + 4 autolight), inactive RimLight+Brazier aktive et. Codex review. Ref: `STAGING/CLIFF_BLACK_LAYER_DIAGNOSIS.md`. (Demo'yu bloklamaz, anında görünür kazanım.)
2. **0.4 A1 orphan doğrula+sil** — Sonnet'e: Player.prefab `HandAnchorAttach.weaponDatabase` hangi asset? Canonical (`Resources/WeaponDatabase.asset`) değilse rebind, sonra `WeaponDatabaseSO.asset` orphan sil. (A2 prereq.)
3. **A2 mount bridge dispatch** — Sonnet write: `HandAnchorAttach` ↔ `OrientationSync.Sync(FacingDir8)` wire, `WeaponSorter` sil, Player.prefab `bodyRenderer`+`handOffsets[]` doldur. Codex review. (Kritik path başlangıcı.)
4. **FILL paralel başlat — #41 PixelLab sentez** — Sonnet'e: `STAGING/PIXELLAB_INVENTORY_MASTER.md` + 3 analiz dosyasını (Opus/Codex/agy) sentezle, tek karar listesi (KEEP/DELETE/REVIEW). Combat'a paralel, gate beklemez. (Profile race: combat Codex review'dan FARKLI account ver.)
5. **0.3 cliff P2 fix dispatch (placer caveat ile)** — Sonnet write: `DirectionalCliffTile.GetTileData` `#if UNITY_EDITOR` kaldır, AMA runtime'da `CliffAutoPlacer.floorTilemap` erişilebilirliğini garanti et (Bağımlılık-4). Codex review.

---

## 7. Git-Push GATE (izole kullanıcı kararı — plan beklemez)

**Durum:** origin/master diverge. Kullanıcının 23 May line-ending normalization "Initial check-in" commit'i nedeniyle local 19 commit ileri, fast-forward DEĞİL. rebase/merge/force-push hepsi riskli. **Claude force-push YAPMAZ — kullanıcı kararı.**

**Yorum (paralel iş güvenli mi?):** EVET, güvenli. Tüm Faz 0/1a/1b/1c işi LOCAL commit'lerde birikir; push diverge'i çözmek için remote'a yazmak GEREKMEZ. Local geçmiş bozulmaz. Kullanıcı push kararını (rebase onto origin / merge / force-with-lease) istediği zaman, paralel work tamamen bağımsız verir. **Tek uyarı:** push kararına kadar yeni hard reset / force ops YAPILMASIN ki local 19-commit güvende kalsın. Plan bu düğümü beklemeden ilerler.

---

## 8. LOCKED Kurallarla Uyum

| Kural | Durum |
|---|---|
| VFX-first weapon (anim+sprite A5 freeze öncesi başlamaz) | ✅ Faz 1b A5-gated |
| Demo T3/parallax/hygiene'e bağımlı değil | ✅ Hepsi FILL |
| Scene-save çakışma (A4 + C10 aynı .unity) | ✅ C10 self-bootstrap, scene edit yok (Bağımlılık-3) |
| Kamera PixelPerfect 640×360 LOCK | ✅ Dokunulmaz |
| Weapon PPU=64, 8-dir (5+3 mirror), HIGH TOP-DOWN 3/4 | ✅ Faz 1b C |
| PixelLab MCP otonom YASAK | ✅ B/C/D user-manual Web UI |
| Karakter 64px içerik/120px canvas | ✅ |
| Sonnet=impl default, Opus=2+ system, writer≠reviewer | ✅ Tablo §1/§5 |

**CONFLICTS WITH LOCKED RULES?: NONE.**

---

## 9. Kritik Çelişki / Açık Not

- **agy catch (ADOPTE):** SFX ve VFX-event-hook placeholder, A4 juice aşamasında kodda hazır olmalı. Plan başlangıcı bunu atlamıştı; A4 scope'una eklendi. Aksi halde D (VFX/SFX) eklenirken koda geri dönüş = ekstra churn.
- **agy catch (ADOPTE):** T3 5-7 günlük tek blok değil, MVP (F1+F2) + Polish (F3-F7) dilimlenir; polish demo sonrasına.
- **Codex verify takıldı:** `laurethgame` profile race. 4 sorunun 3'ü (Soru 1/3/4) Opus tarafından kaynak kodu DOĞRUDAN okunarak doğrulandı (Codex özetinden güçlü). Soru 2 (C4 stub-paralel) spec yapısıyla kabul; ilk T3-MVP dispatch'inde C4 imzasını önce dondur, riski sıfırla.
- **Opus yeni bulgu (Bağımlılık-5):** A1 orphan sil, A2 mount referansını kırabilir — delete öncesi Player.prefab referansını doğrula. Faz 0'a eklendi.
- **Opus yeni bulgu (Bağımlılık-4 caveat):** Cliff P2 fix sadece `#if` kaldırmak değil — runtime'da `CliffAutoPlacer`/floorTilemap erişimi gerekli, yoksa bug yarım çözülür.
