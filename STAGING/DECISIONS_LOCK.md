# RIMA — 4 KARAR LOCK (3-AI sentez)

Tarih: 2026-05-28 · Karar veren: rima-design (Opus) · Surec: GERCEK live dispatch (file-fallback DEGIL).

## Dispatch kaniti (GERCEK 3-AI — ucu de kostu)
- **Codex — KOSTU (2 kez):** `cx_dispatch.py --task-file STAGING/DECISIONS_3AI_TASK.md --effort xhigh`. PROFILE_SELECTED=yasinderyabilgin, `CODEX_DONE_yasinderyabilgin.md` yazildi. Tam yapili 4-karar cevabi dondu (Gate.cs / weapon prefab+meta / cliff+parallax kod+doc okudu; NLM auth expired -> direct-read kanit kullandi).
- **agy — KOSTU (1 retry sonrasi):** 1. deneme `cmd /c agy_dispatch.cmd ...` -> sadece Windows banner (arg passthrough olmadi, FAILED). 2. deneme `& "...agy_dispatch.cmd" --task-file ... --print-timeout 600` (PowerShell call-operator) -> ConPTY session acildi, ACCOUNT_SELECTED=yasinderyabilgin, NLM+direct-read yapti (Gate.cs / PARALLAX_L3_DESIGN.md / weapon prefab+meta / WEAPON_ANIM_VFX_PRODUCTION_LOCK.md okudu), TAM 4-karar research-cevabi dondu (Hades/CoM atifli).
- **Opus (ben) — DOGRUDAN OKUDUM:** `Gate.cs` (default state = AwaitingFragment; `OnRoomCleared`->Unlock; alpha Locked/AwaitingFragment 0.4, Unlocked 1.0; collider sadece Unlocked'ta acik), `Warblade_Greatsword.prefab` (SR m_Size 1x0.25 = placeholder), `Warblade_Greatsword.png.meta` (canvas ~64px, sub-rect'ler 20x6 / 38x13, PPU 64, pivot 0.18/0.50, alignment 9 custom). `PARALLAX_L3_DESIGN.md` (factor 0.08/0.04, sorting -400).

> SINGLE LIVE. Locked rule ihlali YOK. Tum dispatch GERCEKTEN kostu (doc-fallback DEGIL).

---

## KARAR 1 — Gate davranisi
**LOCKED: (b) location-visible, clear-to-unlock.** Kapinin yeri BASTAN BELLI + KILITLI (alpha 0.4, collider kapali); oda temizlenince `Unlock()` -> `State.Unlocked` (alpha 1.0, collider acik) + open-anim. Map Fragment HER gate'te DEGIL — sadece reward/Treasure/Shop + Boss Approach climax gate'inde `State.AwaitingFragment` yolu (gate bekler, fragment gelince Unlock). Normal combat room: gate `AwaitingFragment` veya `Locked` -> oda temizlenince `Unlocked`.
- NOT (kod gercegi): `Gate.cs`'te default `CurrentState = AwaitingFragment`, state'ler = Locked/AwaitingFragment/Unlocked/Unrevealed. `_useFragmentGateFlow` diye bir field YOK — fragment-gate ayrimi cagiranin hangi state'i set ettigiyle yapilir (room-cleared -> `Unlock()` cagir; fragment-gerektiren oda -> `AwaitingFragment`'ta birak, fragment gelince `Unlock()`).
- Gerekce: Oyuncu cikis yerini bastan gormeli (demo readability + combat sirasinda taktiksel konumlanma); mevcut 3-state machine bunu SIFIR-RISK tasir (kod zaten boyle: default AwaitingFragment, OnRoomCleared->Unlock). Hades/ARPG locked-exit normu. Map Fragment'i her kapiya bagli zorunlu toll yapmak demo akisini kirar.
- Atif — UCU DE HEMFIKIR. Codex: (b), fragment per-gate degil reward pickup. agy: (b), fragment sadece Treasure/Shop + Boss Approach "climax gate"te anahtar olsun (readability + tempo). Opus: ayni — `Gate.cs` zaten (b) yi opt-in `AwaitingFragment` ile destekliyor, bedava.

## KARAR 2 — Cliff derinlik/yerlesim
**LOCKED: Faz 1 = TEK temiz dis-perimetre "asagi drop" cliff. Otomatik "cok ustte" dekoratif cliff EKLEME** (gerekirse sonra ELLE, non-collider, `DecorCliffTilemap`).
- **Silme kriteri (perimetre tanimi):** En buyuk oynanabilir floor component'ini bul -> padded bounds'tan disari void'i flood-fill et -> SADECE S/SE/SW komsusu disari-bagli void olan floor kenar hucrelerini TUT. Internal hole / lob-arasi band / X strip / 1-3 hucre izole cluster = SIL. Cliff run/cluster `>=4` esigi.
- **Derinlik teknigi (cok altta / cok ustte):** Z ile DEGIL — sorting layer/order + dusuk parallax + kontrast/scale dili ile. Cliff face = `Decor_Cliff` (order 12, floor ustunde "cok ustte" okur). L3 dip ambiyans = factor (0.08,0.04), sortingOrder -400 ("cok altta" okur, cliff'in mutlak altinda). Floor 0, opsiyonel drop-shadow -20. PPU 64, top-center pivot, yarim-cell asagi offset korunur.
- **"Cok ustte" notu (agy uyarisi, ONEMLI):** Top-down 3/4'te zemin-ustu duran opak dekoratif cliff oyuncu/dusman/projectile gorusunu KAPATIR (kor nokta). Bu yuzden "cok ustte" auto cliff YASAK. Gercekten ust-katman derinligi istenirse = Foreground sorting layer (+600), hizli parallax (1.10-1.20), blur ile sahne KENARINDA cerceveleme (oynanis alanini ortmeden) — F3 Foreground_Front katmani zaten bunu yapiyor. Ic dikey engel gerekirse cliff degil DUVAR/HEYKEL (`Assets/Prefabs/Decor/Statues`).
- Gerekce: 283 auto-cliff'in 166 cluster'a bolunmesi gurultu; ekstra auto dekor cliff ayni karisikligi buyutur + gorus kapatir. Floating-arena okumasi = authored dis rim + altta void/atmosfer (Hades/CoM), ic kontur cliff'i degil. Tek floating-island siluet + dipte ambiyans = kullanicinin "ya cok altta ya cok ustte" kuralini saglar.
- Atif — UCU DE HEMFIKIR (perimetre-only). Codex: perimetre-only + flood-fill S/SE/SW + cluster>=4 + L3 -400 + Z degil sorting. agy: dis-perimetre yeterli, ic/lob cliff SIL, "cok ustte" gorus kapatir -> cliff degil Foreground/duvar/heykel, dip = L3 -400/(0.08,0.04). Opus: ayni, `PARALLAX_L3_DESIGN.md` ile birebir; agy'nin gorus-kapatma uyarisini foreground cozumune yonlendirdim.

## KARAR 3 — Weapon canvas boyutu (CELISKI -> Opus sentez)
**LOCKED: PixelLab uretim canvas'i `128x128` (transparent, PPU 64) BASELINE; gorunur icerik tier'a gore degisir. Pivot = grip `(0.1875, 0.50)` (= 12px @ PPU64, mevcut .meta ile birebir). Import sonrasi tight-crop edilebilir (Unity oransal sprite-rect korur).**
- **Gorunur icerik hedefleri (px, PPU 64):** dagger 24-40 · tek-el (kilic/balta/orb) 40-56 · greatsword/2H **96-112 uzun** (yan-yana 63px body ile oransal), 12-20 blade kalinligi · 14-22 hilt/grip.
- **Pivot/mount:** Base silah EAST/RIGHT baksin. Pivot = grip/HandAnchor temas noktasi, `(0.1875, 0.50)`. Weapon prefab HandAnchor child, pivotundan doner; localPosition 0,0,0; mevcut anchorOffset yeni import sonrasi +-4px (`0.0625u`) toleransla retune.
- **CELISKI cozumu (neden 128, neden 64x32 DEGIL):** agy `64x32` tek-standart onerdi — bu dagger/tek-el icin dogru AMA greatsword icin YANLIS: 64px canvas gorunur uzunlugu 64px'e kilitler, 63px-boy body yaninda kisa kalir; oransal greatsword ~96-112px uzunluk ister (Opus, body 120/63 + greatsword placeholder 64x16'yi DOGRUDAN olcerek dogruladi). 128 baseline TUM tier'lari (dagger->greatsword) tek import ayariyla tutar; agy'nin 64x32 visible-icerik aralici (dagger 24-32, greatsword 52-64) cok kucuk. Codex'in 128 baseline + size-tier yaklasimini ALDIM, agy'nin pivot degeri (0.1875/0.50) + tek-import-standardizasyon hedefini KORUDUM. Codex'in 192/256 XL ust-tier'i opsiyonel kaldi (128'de 112px sigar, sart degil).
- Atif — CELISKI. Codex: 128 baseline + size-tier (greatsword 192/256 ust-tier). agy: tek-standart 64x32 (greatsword visible 52-64, dagger 24-32), pivot 0.1875/0.50. Opus: 128 baseline (agy'nin 64 cap'i greatsword'u oransal kiriyor — body 120/63 olcumuyle gerekce), pivot 0.1875/0.50 (agy+meta ile ayni).

## KARAR 3b — Weapon YON uretimi
**LOCKED: Faz 1 = `1` sprite/silah (east/right). 8 yon weapon sprite URETME.** Body 8-dir weaponless cizilir; silah HandAnchor child SR olarak takilir, OrientationSync ile per-yon rotate/offset/sort. N/NE/NW'de silah body ARKASINA sort'lanir (behind-body).
- **ATTACK ANI cozumu (agy insight, ALINDI):** Idle/Walk/Run = mounted-sprite rotate. ATTACK aninda mounted silah sprite'ini GIZLE + silahi-iceren slash-arc VFX flipbook oynat. Sebep: Hades/CoM tam olarak bunu yapar — donen kart-sprite saldiri smear pozunda "karton" gibi kirilir; VFX arc hem bu kirilmayi gizler hem juice katar (RIMA combat juice lock ile uyumlu). Bu, "tek-sprite ne zaman kirilir" sorusunun en temiz cevabi: en cok attack-smear'da kirilir, cozum attack'i VFX'e devretmek.
- Gerekce: Endustri ikiye boluyor — yuksek butce ARPG saldiri weapon'i anim/VFX'e bake eder; moduler gear sistemi tek sprite mount+rotate eder. RIMA mevcut lock = moduler yol (`project_weapon_system_8dir_lock`: body weaponless + HandAnchor child + OrientationSync), bu yuzden MVP'de 1 sprite EN DUSUK-RISK + en hizli. Weaponless body + mounted weapon (idle/move) + slash-VFX (attack) yeterli readability + juice verir.
- **Tek-sprite NE ZAMAN KIRILIR + cozum:** (1) Attack smear -> silahi gizle + slash-arc VFX (agy, ana cozum). (2) Asimetrik/guclu-perspektif silah (yay, tabanca, egri katana) ve N/back occlusion -> once sort/mask, sonra GEREKIRSE asimetrik silah icin SW/NW mirror veya 3 override sprite (front/back/side). Tam 8-dir bake YOK.
- Atif — UCU DE HEMFIKIR (1 sprite, 8-dir bake etme). Codex: 1 sprite, kirilma=asimetrik/perspektif/back-occlusion -> 3 override fallback. agy: idle/walk/run 1 sprite + ATTACK'ta silah gizle + weapon-inclusive slash-arc VFX (Hades/CoM kart-kirilma onleme), asimetrikte mirror sprite. Opus: ayni — `project_weapon_system_8dir_lock` ile birebir reaffirm; agy'nin attack-VFX insight'ini ana kirilma-cozumu olarak LOCK'a aldim (en guclu tasarim noktasi).

---

## Acik celiski ozeti (sentez kararlari)
- **Karar 3 (canvas):** Codex 128-baseline+tier vs agy 64x32-tek-standart -> **Opus: 128 baseline** (64 cap greatsword'u oransal kiriyor; body 120/63 + placeholder 64x16 olcumuyle). agy pivot (0.1875/0.50) korundu.
- **Karar 3b (attack):** agy'nin "attack'ta silah gizle + slash-VFX" insight'i Codex'in genel "VFX fallback"ine gore daha keskin -> **LOCK'a ana cozum olarak alindi.**
- **Karar 1 & 2:** UCU DE HEMFIKIR, celiski yok.

## CONFLICTS WITH LOCKED RULES?: NONE
- Weapon 8-dir mount lock (`project_weapon_system_8dir_lock`) -> KORUNUR (Karar 3b birebir uyumlu, ezmiyor reaffirm ediyor; attack-VFX combat juice lock ile uyumlu).
- PPU 64 -> KORUNUR (tum weapon/cliff/parallax degerleri 64 PPU).
- Kamera 640x360 -> KORUNUR (hicbir karar dokunmuyor).
- Decor_Cliff sorting (12) -> KORUNUR (cliff face order 12 "cok ustte"; L3 -400 mutlak altinda).
- L3 `PARALLAX_L3_DESIGN.md` (factor 0.08/0.04, -400) -> KORUNUR (Karar 2 onu referans alir, ezmez).
