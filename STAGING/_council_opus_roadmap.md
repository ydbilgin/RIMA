# OPUS ADVISOR — Oynanabilir Roadmap (BAĞIMSIZ) 2026-06-05

> Bağımsız Opus görüşü. Sentez orchestrator'da. Bu rapor brief'in 4 ⚠️ belirsizliğini KOD-DOĞRULAMA ile çözer ve kendi sıralı planımı verir.

---

## 0. KOD GERÇEĞİ — brief'i DÜZELTİYOR (file:line kanıt)

Brief diyor ki: "⚠️ HandAnchor/OrientationSync/WeaponSorter kodda VAR MI yoksa sadece tasarım kilidi mi belirsiz."
**Cevap: SADECE kilit DEĞİL — TAM KODLU + Warblade için UÇTAN UCA WIRE'LI + test-sahneli.** Bu, tüm sıralamayı değiştirir.

| İddia (brief) | Gerçek (kanıt) |
|---|---|
| HandAnchor/OrientationSync var mı? | VAR. `Assets/Scripts/Combat/OrientationSync.cs` (178 satır): 8-dir `handOffsets[8]`, `weaponRotations[8]`, per-dir `flipY` (line 85-89), zaman-bazlı prosedürel swing `BeginSwing/Update` (line 98-151) — strike frame `attackStartup`'a hizalı. |
| Bridge var mı? | VAR. `Assets/Scripts/Systems/Combat/HandAnchorAttach.cs` (250 satır): `PlayerAttack.OnComboStep`'e abone (line 58-59), per-yön sort-order (N/NE/NW → body arkası, line 199-205), swing sırasında silahı GİZLEMEZ → alpha 0.4'e FADE eder (line 35-37, 100-114). Level1Static + Level2SpriteHandData iki mod. |
| WeaponSorter? | Ayrı class YOK; sort mantığı `HandAnchorAttach.UpdateWeaponSortOrder` içinde (line 199-205). NLM'in "WeaponSorter" adı = bu fonksiyon. |
| DB + asset? | VAR. `Assets/Resources/WeaponDatabase.asset` — **1 entry: Warblade** (anchorOffset 0.2,0.1; twoHanded). `Assets/Prefabs/Weapons/Warblade_Greatsword.prefab` GERÇEK sprite'lı (guid 8b41ec...). |
| Player'a wire'lı mı? | VAR. İKİ Player prefab (`Assets/Prefabs/Player.prefab` + `Assets/Resources/Prefabs/Player.prefab`) ikisinde HandAnchor child + HandAnchorAttach (`classId: Warblade, attachMode: 0`). |
| Test edilmiş mi? | EVET. `Assets/Scenes/Phase2_WeaponAttach_Test.unity` + 3 screenshot (idle/walk/attack) `Assets/Screenshots/phase2_weapon_attach_*.png`. |

**eski_anchors silinmesi (⚠️ brief sorusu):** Silinen `Characters/eski_anchors/*/metadata.json` = **Level-2 per-sprite el anchor verisi**. Live sistem **Level1Static** (`attachMode: 0`) çalışıyor → SO'daki `handOffsets[8]` dizisini kullanır, silinen metadata'yı DEĞİL. **Demo yolunu BLOKLAMAZ.** Level-2'ye demo'da gerek yok (post-demo cilası). Yeniden çıkarmaya GEREK YOK.

**Swing canon çelişkisi (NLM'de açık duruyordu):** Kod zaten kararı vermiş — "gizle" DEĞİL "alpha 0.4 fade" (HandAnchorAttach line 35-37 yorumu: "keep weapon VISIBLE... Hades keeps it visible"). NLM'in "attack'ta silahı gizle + slash-VFX" reçetesi SUPERSEDED. **Üretim kararı: silah görünür kalır, VFX üstüne biner. Bu kilidi yeniden açma.**

### Bunun roadmap'e etkisi (KRİTİK)
Silah işi "sıfırdan sistem" DEĞİL — **"1 sınıf çalışıyor → 9 sınıfa veri-replikasyonu + sprite üretimi"** işi. Risk dramatik düştü. Asıl kalan iş kod değil, **(a) 9 silah sprite üretimi (kullanıcı-gated PixelLab), (b) per-class anchor/rotation tuning verisi, (c) B-11 combat lifecycle.** Sistem mimarisi RİSKİ BİTTİ.

---

## 1. SIRALI ROADMAP (faz → işler → boyut → kim → bağımlılık)

Kullanıcı önceliği (silah→oturtma→anim→diğer) ile teknik bağımlılık şu noktada ÇAKIŞIYOR: **silahın "ele oturması" zaten Warblade'de ÇÖZÜLMÜŞ.** O yüzden kullanıcının #1/#2'si büyük ölçüde "yeni sprite + tuning verisi", #3'ü (anim) bugün "kod-only" kararıyla küçüldü. Bu, B-11'i öne çekmemize izin verir.

### FAZ 0 — DOĞRULAMA + PİLOT GENİŞLETME (otonom, ~yarım oturum)
| # | İş | Boyut | Kim | Bağımlılık |
|---|---|---|---|---|
| 0.1 | Phase2_WeaponAttach_Test sahnesini AÇ, play-verify (Warblade idle/walk/attack 8-yön, swing-fade, sort) — hâlâ çalışıyor mu? | S | cx (Unity-verify) | — |
| 0.2 | WeaponDatabaseSO'ya `weaponRotations[8]` + `flipY` per-class alanları taşı (şu an OrientationSync'te SerializeField — veri DB'de olmalı ki kullanıcı sınıf-başı ayarlasın) | S | cx | 0.1 |
| 0.3 | Anchor-tuning EDITOR aracı: Scene'de Warblade'i 8 yöne döndür, handOffset/rotation'ı canlı kaydır→SO'ya yaz (kullanıcı şartı: "veri-bazlı, kullanıcı ayarlayabilsin") | M | cx | 0.2 |

### FAZ 1 — B-11 COMBAT LIFECYCLE (otonom, döngünün kalbi) — **SİLAHTAN ÖNCE**
| # | İş | Boyut | Kim | Bağımlılık |
|---|---|---|---|---|
| 1.1 | clear-tespit → slow-mo → reward draft (parçalar VAR: EncounterController + RewardPickup + DraftManager) — uçtan uca bağla | M | cx | — (silahtan bağımsız) |
| 1.2 | reward al → kapı "yak" (dark→light) → walk-into-door → sonraki oda yükle (MapFlowManager VAR) | M | cx | 1.1 |
| 1.3 | Echo award play-verify (kod hazır) + run sonu (+n SHATTERED ECHO) → Chamber unlock | S | cx/Sonnet | 1.2, Chamber-done |
| 1.4 | Soft-lock guard + lifecycle EditMode/PlayMode testi | S | cx | 1.1-1.3 |

### FAZ 2 — KNOCKDOWN + JUICE (otonom; CODEANIM kararı) — B-11 İLE PARALEL/HEMEN SONRA
| # | İş | Boyut | Kim | Bağımlılık |
|---|---|---|---|---|
| 2.1 | İki knockback impl BİRLEŞTİR (KnockbackComponent→KnockbackReceiver) + `HitImpulse` struct | M | cx | — |
| 2.2 | KnockdownDriver + 3 KnockdownProfile SO (Light/Heavy/Boss) + Broken/Sundered tetik (SkillStateTracker event) + get-up i-frame | M | cx | 2.1 |
| 2.3 | mob-ölüm squash→decal→fade (0 asset) | S | Flash/Sonnet | — |
| 2.4 | spawn (drop+easeOutBack+toz) + dash ghost-trail | S | Flash/Sonnet | — |

### FAZ 3 — SİLAH ÜRETİM (KULLANICI-GATED PixelLab oturumu) — Faz 0.3 hazır olunca
| # | İş | Boyut | Kim | Bağımlılık |
|---|---|---|---|---|
| 3.1 | **PİLOT-2 oturumu:** Ronin katana + Ranger bow üret (asimetrik test) → DB entry + prefab + anchor tuning + play-verify | M | KULLANICI (PixelLab) + cx (wire) | 0.3 |
| 3.2 | PİLOT onayı sonrası **KALAN-7 batch oturumu** (Shadowblade, Ravager, Gunslinger, Elementalist, Summoner, Hexer, Brawler) | L | KULLANICI (PixelLab) + cx (wire) | 3.1 onay |
| 3.3 | Her sınıf: DB entry + prefab + per-yön anchor/rotation tuning (Faz 0.3 aracıyla) | M | cx + KULLANICI (tuning beğeni) | 3.1/3.2 |

### FAZ 4 — ODA/CONTENT + RUN-İÇİ UX (otonom, paralel-able)
| # | İş | Boyut | Kim | Bağımlılık |
|---|---|---|---|---|
| 4.1 | B-12 production RoomBank (15 oda hazır) + pacing + depthCount 10-15 | M | cx/Sonnet | B-11 |
| 4.2 | Modüler-props: checker + auto-placer koşturma | M | cx | 4.1 |
| 4.3 | Run-içi UX: draft-kart hover (TooltipSystem ✓kısmen bağla) · sol skill paneli toggle · ESC codex | M | cx | — |
| 4.4 | Elementalist büyü-VFX Phase-1 (kod/particle) | M | cx | — |

### FAZ 5 — GATED ASSET BATCH'LERİ (kullanıcı, ayrı oturum)
| 5.1 | Skill ikonları (PixelLab 64px) | L | KULLANICI | — |
| 5.2 | Mob görselleri (PixelLab) | L | KULLANICI | — |

---

## 2. KRİTİK YOL

```
Chamber (cx in-flight) ──┐
                         ├──► B-11 lifecycle (1.1→1.2→1.3) ──► OYNANABİLİR DÖNGÜ (silahsız bile)
Faz 0 weapon-verify ─────┘                                         │
                                                                   ▼
Faz 0.3 anchor-tool ──► PİLOT-2 (3.1) ──► KALAN-7 (3.2) ──► TAM SİLAHLI DEMO
```

**Kritik yol = Chamber → B-11 lifecycle → ilk-oynanabilir.** Silah sprite üretimi kritik yolda DEĞİL (Warblade ile döngü zaten silahlı oynanabilir; diğer 9 sınıf "doldurma"). Bu yüzden **B-11'i silahtan ÖNCE bitir** — minimum-oynanabilir en hızlı oraya gelir.

---

## 3. PARALELLİK ŞEMASI (kullanıcı-gated vs otonom)

```
OTONOM KOD BLOĞU (cx, kullanıcı yokken):        KULLANICI-GATED (PixelLab oturumu):
  Faz 0 (verify + anchor-tool)                    (Faz 0 biter bitmez)
  Faz 1 (B-11 lifecycle)         ────paralel────► PİLOT-2 silah üretimi (3.1)
  Faz 2 (knockdown/juice)                                │
  Faz 4.1-4.4 (oda/UX/VFX)                               ▼
                               ◄──cx wire eder── KALAN-7 üretim (3.2)
```

- **En verimli düzen:** Kullanıcı PixelLab'da silah üretirken cx B-11 + knockdown yazar. Kullanıcı bir batch bitirince cx onları DB'ye wire'lar (Warblade pattern'i kopyala = mekanik iş). İkisi birbirini beklemez.
- **Gate noktaları (yalnız 3):** (a) silah sprite üretimi, (b) per-class anchor tuning beğenisi, (c) skill/mob asset batch'leri. Geri kalan TAMAMEN otonom.

---

## 4. BRİEF'İN 4 SORUSUNA NET CEVAP

### S1 — Sıralama + kritik yol
**B-11 ÖNCE, silah SONRA.** Gerekçe: Warblade zaten silahlı çalışıyor → döngü silahla oynanabilir; B-11 olmadan oyun OYNANMAZ. Sıra: Faz0(verify+tool) → Faz1(B-11) ‖ Faz2(knockdown) → Faz3(silah, gated) → Faz4(content). Kritik yol = Chamber→B-11.

### S2 — Silah üretim oturumu tasarımı
- **2'li pilot, 10'lu DEĞİL.** Ama pilot çifti = **Ronin (katana, asimetrik+kın) + Ranger (bow, en zor perspektif/asimetri).** Brief'in önerdiği Warblade+Ronin DEĞİL — **Warblade ZATEN LIVE** (tekrar üretme). En zor iki asimetrik silahı pilotla ki anchor/flipY/sort reçetesi uçta kanıtlansın; kolay olanlar (orb, lantern, grimoire) batch'te risksiz akar.
- **İkiz silahlar (hançer/balta/tabanca) = TEK sprite.** NLM canon tablosu zaten "L+R mirror, 1-dir runtime rot" diyor (Gunslinger/Shadowblade satırları). Tek el-sprite üret → ikinci eli runtime `flipX`+ikinci HandAnchor ile yansıt. Çift sprite üretmek 2× maliyet + senkron derdi; mirror bedava. (Mimari zaten flipY destekli; ikiz için ikinci anchor + flip = küçük cx işi.)
- **Üretim→anchor zincirleme:** Her silah için sırayla: PixelLab Create Image S-XL (Direction:None, init=class anchor crop) → import PPU64 → DB entry (Warblade'i kopyala) → Faz 0.3 editör aracıyla 8-yön anchor/rotation tuning → play-verify. Pilot'ta bu zinciri 1 kez kullanıcıyla yürü, batch'te tekrarla.

### S3 — Sıralama riski (silah B-11'den önce mi sonra mı?)
**Silah B-11'den SONRA = daha az rework.** Silahlar combat-feel'i değiştirir AMA silah-mount sistemi B-11'e DOKUNMAZ (ayrı katman: lifecycle = oda/reward/kapı akışı; silah = render/anchor). Tersine, **silahı önce yaparsak 9 sprite üretiminde takılıp döngüsüz kalırız.** B-11 silahsız-Warblade ile test edilebilir, silah eklenince feel zenginleşir ama yapı kırılmaz. Rework riski düşük çünkü iki sistem dik (orthogonal).

### S3b — Knockdown ne zaman?
**Knockdown = B-11 ile PARALEL ya da hemen sonra (Faz 2), silahtan bağımsız.** Gerekçe: knockdown altyapısı %70 hazır (CODEANIM audit: GroundBlobShadow + BasicAttackProfile knockback[] + SkillStateTracker event), kod-only, sprite gerektirmez → kullanıcı silah üretirken cx bunu yazabilir. **#1 risk = juggle-lock → get-up i-frame ZORUNLU (CODEANIM kararında kilit).** Knockdown'ı silaha bağlama; combat-feel'in çekirdeği, erken gelmeli.

### S4 — 2-3 oturuma bölüm (net teslimatlı)

**OTURUM A (OTONOM, kullanıcısız) — "Oynanabilir iskelet":**
Teslimat: Faz 0 (silah-verify + DB veri-taşıma + anchor editör aracı) + Faz 1 (B-11 lifecycle uçtan uca) + Faz 2.1-2.2 (knockback birleşme + KnockdownDriver). **Sonuç: Warblade ile tam run-döngüsü + knockdown, play-verified.**

**OTURUM B (KULLANICI-GATED + cx paralel) — "Silah doldurma":**
Teslimat: PİLOT-2 (Ronin+Ranger) kullanıcıyla → onay → KALAN-7 batch. Paralelde cx: Faz 2.3-2.4 juice + Faz 4.3 run-içi UX. **Sonuç: 10 sınıf silahlı + cilalı combat.**

**OTURUM C (OTONOM + gated batch) — "İçerik + cila":**
Teslimat: Faz 4.1-4.2 (RoomBank + props) + 4.4 (Elementalist VFX) + (gated, ayrı) Faz 5 skill/mob asset batch. **Sonuç: tam-içerikli demo.**

---

## 5. RİSKLER (3)

1. **Çift Player prefab drift.** `Assets/Prefabs/Player.prefab` VE `Assets/Resources/Prefabs/Player.prefab` ikisi de HandAnchorAttach'lı. Hangisi canlı sahnelerde? Silah/tuning değişikliği İKİSİNE de gitmeli yoksa "düzelttim ama oyunda eski" hatası. **Aksiyon: Faz 0.1'de hangisinin runtime-yüklendiğini doğrula, tek-kaynak yap.**
2. **Anchor tuning = gizli zaman-yiyici.** 10 sınıf × 8 yön (mirror'la 5) elle hizalama, "veri-bazlı kullanıcı-ayarlanabilir" şartıyla. Editör aracı (Faz 0.3) OLMADAN her silah saatlerce sürer. **Araç kritik-yolun ön-koşulu; pilot'tan ÖNCE bitmeli.**
3. **Juggle-lock (knockdown).** CODEANIM'de işaretli #1 risk. Get-up i-frame olmadan knockdown player'ı/mob'u sonsuz kilitler. **Faz 2.2 i-frame'siz MERGE edilemez** — test şartı.

---

## ÖZET KARARLAR (orchestrator için)
- KOD: silah sistemi YAŞIYOR ve Warblade UÇTAN-UCA wire'lı/test'li — "sistem inşa" işi DEĞİL, "veri+sprite replikasyon" işi.
- eski_anchors silinmesi demo'yu BLOKLAMAZ (Level1Static SO-array kullanır, silinen metadata Level-2).
- Swing="gizle" SUPERSEDED → "alpha 0.4 fade" (kodda zaten karar verilmiş).
- SIRA: B-11 ÖNCE → silah SONRA (daha az rework, en hızlı oynanabilir).
- Pilot = Ronin+Ranger (asimetrik, Warblade DEĞİL — zaten live). İkiz silah = tek sprite + runtime mirror.
- Knockdown = Faz 2, silahtan bağımsız, kullanıcı-üretirken cx-paralel; get-up i-frame ZORUNLU.
