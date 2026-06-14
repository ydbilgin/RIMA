# Edit-to-Play Demo Video — STORYBOARD DECISION (LOCKED 2026-06-14)

> Council: cx (feasibility/reuse) + ax 3.1 Pro (deep/narrative) + ax 3.5 Flash (lean/ship-fast). Synthesis: Opus 4.8.
> Tez: RIMA = "oyun degil, **environment + ilk vertical slice**" (domain-specific reusable tooling). Centerpiece video. Sunum ~20 Haziran.
> Kaynak ham cikti: `STAGING/_process/2026-06/_council_*_edit_to_play_storyboard.md` + CODEX_DONE.

---

## KARAR OZETI (locked)
- **Sure:** 2:00-2:20. (Flash 90sn / Pro 3dk arasi; cx shot-list ~2:05.)
- **"Edit-to-Play" wow momenti = F2 Build Mode toggle** (BuildModeController). Play icinde F2 → Build Mode (pause + Build tab + WorkingTemplate) → 1 prop yerlestir → F2 tekrar → AYNI ODADA oyun devam, prop yerinde. Bug-free, en guclu tez kaniti.
- **Canli tile/oda cizimi YOK** (demo-patlatma + yavas). IsoRoomBuilder'in CIKTISI gosterilir (`_Arena` data→oda), tek prop placement ile "icerik enjeksiyonu" kanitlanir.
- **Golden-path (reward→kart) F2-fix'e BAGLI** (asagida dependency). Fix varsa cek; yoksa pickup oncesi kes + slayt.
- **Cerceve:** Sen oyuncu degil **sistem muhendisisin**. Teknik overlay (telemetry/log) HER AN ekranda. Klinik/teknolojik ses, epik DEGIL. Oyunu satma.

---

## VITRINE 4 GOD-NODE (cx stabilite sirasi — graphify 6/10 editor verisiyle rezonans)
1. **BuildModeController** — F2 runtime Build Mode, edit-to-play kahramani. Tek-sahip F2 registry, working-copy guvenli.
2. **RoomRunDirector + IsoRoomBuilder** — Uzam/mimari: data (RoomTemplateSO) → oda; `_Arena` omurga, kapilar/reward yasam-dongusu.
3. **DirectorMode (Spawn/Stats/Telemetry)** — backquote overlay; dusman spawn, stat slider, telemetry CSV. Combat tuning + audit.
4. **BuildPlacementController** — prop placement (palette→ghost→click). ⚠️ Eski `InPlayMapPaintOverlay` SATMA (retired, RIMA_LEGACY_MAPPAINT off).

---

## SANIYE-SANIYE SHOT LIST (locked)

| Zaman | Gorsel | Arac | Voiceover (taslak) |
|---|---|---|---|
| **0:00-0:10** HOOK | Graphify full-map; zoom top-10 node, 6'si KIRMIZI=editor | (data-viz) | "RIMA bir oyun degil. 6925 dugumluk bu mimaride en cok bagli 10 dugumun 6'si oyun mekanigi degil — editor araci. Bu bir tesadüf degil." |
| **0:10-0:25** ARENA | `_Arena` canonical yuzen oda acilir (IsoRoomBuilder/RoomRunDirector data'dan kurdu) | RoomRunDirector + IsoRoomBuilder | "Ilk katman: mekansal veriyi aninda sahneye doken izometrik oda mimarisi. Cizmiyoruz — veri sahneye seri-hale geliyor." |
| **0:25-0:50** EDIT | Play icinde **F2** → Build Mode (pause, Build tab, WorkingTemplate). 1 PropRegistry prop (rift_crystal) yerlestir; cyan Light2D yanar | BuildModeController + BuildPlacementController | "Calisan oyunun icinde, derleme beklemeden icerik enjekte ediyoruz." |
| **0:50-0:55** ⭐ EDIT-TO-PLAY | **F2 tekrar** → Build Mode cikar, UI/kamera restore, AYNI odada oyun devam, prop yerinde | BuildModeController | "Derleme yok, rebuild yok. Duzenlenen veri ayni saniyede oynanabilir ve denetlenebilir bir vertical slice." |
| **0:55-1:25** PLAY/TUNE | backquote → DirectorMode Spawn: 1-2 dusman + `physPower` slider 50→250. **Director'i GORUNUR sekilde kapat** (time pause!). LMB ile vur → 250+ hasar sayilari ucar | DirectorMode | "Catisma dengesini canli enjekte ediyoruz; slider'la artirilan hasar ayni vurusta dogrulaniyor." |
| **1:25-1:50** GOLDEN-PATH †(F2-fix'e bagli) | RoomCleared → RewardPickup → G topla → DraftManager kart secimi → exit kapi acilir → G ile gec | RoomRunDirector | "Sistem tam akisi yurutuyor: temizle → odul → kart → sonraki oda. Bu oyun degil, pipeline'in stres testi." (overlay HER AN acik) |
| **1:50-2:10** AUDIT | DirectorMode Telemetry: DPS/event satirlari; 2sn CSV insert (Excel/VSCode) | DirectorMode Telemetry | "Ve surekli denetliyoruz — her olay loglanir, CSV'ye doner." |
| **2:10-2:20** CLOSE | Sonraki oda yuklenir → freeze; RoomRunDirector·IsoRoomBuilder·DirectorMode etiketleri | — | "Bir kez duzenle, aninda oyna, surekli denetle. RIMA, kendi grafigiyle kanitlanan bir uretim ortamidir." |

---

## GOLDEN-PATH BYPASS (cx kod-gercekligi)
- Akis: temiz MainMenu → Chamber → `_Arena`, ILK Combat odasinda basla. **Legacy RoomLoader reward-room/chest'lere ATLAMA.**
- **F1 leak:** reward-room gecislerinden kacin; `RoomRunDirector.BuildCurrentRoom` (DestroyActiveReward cagirir) kullan.
- **F2 reward→kart:** tek prova edilmis Combat-clear; RewardPickup.Collect → DraftManager.ShowDraft. Bir kez patlarsa golden-path'i pickup ONCESI kes, F2'yi "bilinen limitasyon" slaytı yap (canli demo segmenti DEGIL).
- Forge depth 4/8 ve Echo/chest reward yollarindan KACIN.

## † DEPENDENCY (kritik)
**1:25-1:50 golden-path segmenti F2 bug fix'ine bagli** (EXECUTION #2: minimal repro → en kucuk fix → playtest). 
- Fix cekim-gunune kadar hazirsa → segmenti tam cek.
- Hazir degilse → **fallback:** pickup oncesi kes, golden-path'i "bilinen limitasyon" slaytıyla anlat. Video'nun ANA tezi (F2 edit-to-play + stat-to-damage) F2 bug'dan BAGIMSIZ, her halukarda saglam.

---

## RISKLER & KARSI-ONLEM (3 advisor merged)
| Risk | Karsi-onlem |
|---|---|
| "Oyunu satma" tuzagi (jüri: mimari nerede?) | Teknik layer (telemetry/log/FPS) HER AN ekranda. UI'yi tamamen temizleme. Klinik ses. |
| Bug gosterme / canli patlama | Dogaclama YOK. Tiklanacak buton+koordinat onceden plan. OBS 10× prova, tek kusursuz take. |
| Asiri-teknik → jüri kaybi | Sadece gorsel UI (buton/slider/node). Graphify = 10sn data-viz punch, uzun anlatma. Kod satiri gosterme. |
| Odak dagitma (her sey gosterme) | 4 god-node kurali. "3 akisi (Oda/Dusman/Odul) kusursuz baglayan pipeline" > "her sey yapan arac". |
| DirectorMode time-pause | Combat'tan ONCE Director'i GORUNUR kapat. |
| Canli tile cizimi | KESILDI. Sadece prop placement (terrain-edit'ten guvenli). |

## ACILIS & KAPANIS (locked)
- **Hook (0:00-0:10):** Graphify map + "6925 node, 6/10 god-node = editor" → hemen canli odada F2 Build Mode. (Opsiyonel punch: "RIMA is NOT a game" flash.)
- **Kapanis (2:10-2:20):** Sonraki oda yuklenir, freeze on 3 god-node etiketi. "Edit once, play immediately, audit continuously. RIMA = kendi grafigiyle kanitlanan uretim ortami."

---

## REDDEDILEN ONERILER (iz birakmak icin)
- ❌ **Canli oda/tile cizme** (3.1 Pro) — demo-patlatma + yavas; IsoRoomBuilder ciktisi yeter.
- ❌ **Card Pool Manager %100 drop wow** (3.1 Pro) — boyle bir tool YOK + F2 bug ustunde; wow=F2 toggle.
- ❌ **Legacy InPlayMapPaintOverlay vitrine** — retired, hikayeyi karistirir.
- ❌ **3dk+ / boss / dual-class / cok-odali akis** (Flash) — sure+bug riski; slayt kaniti olarak gec.

## SONRAKI ADIM
1. (opsiyonel) Bu storyboard'u kullaniciyla teyit.
2. **F2 bug minimal repro → fix** (EXECUTION #2) — golden-path segmentinin on-kosulu.
3. OBS prova (10×) + tek take kayit.
