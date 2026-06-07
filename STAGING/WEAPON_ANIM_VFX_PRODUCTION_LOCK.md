# WARBLADE WEAPON + ANIM + VFX PRODUCTION LOCK

**Tarih:** 2026-05-28
**Author:** rima-design (Opus 4.7) — TRIPLE-AI synthesis (Opus judgment + Codex xhigh teknik feasibility + agy referans/risk second-eye + codebase ground-truth verify)
**Status:** LOCKED — Faz 1 Warblade demo. Asset gen yok; bu doc uretim sirasini ve VFX-kaynak ayrimini kilitler.
**Scope:** Faz 1 = Warblade TEK (project_demo_phase1_milestone_lock). 9 class + cross-class = Faz 4, sadece BLOK F'de outline.
**Supersedes (kismi):** WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md Bolum 4+6 (sira), ve o doc'taki yanlis "PNG YOK" + "PPU 100" + "W/SW/NW flipX mirror" iddialari (asagida duzeltildi).

---

## 0. CODEBASE GROUND-TRUTH (Opus verify — plan'daki yanlislar duzeltildi)

Bu kararlar codebase'den dogrulandi, ezbere/plan iddiasina guvenilmedi:

| Iddia (eski plan) | GERCEK (verify edildi) | Sonuc |
|---|---|---|
| "Greatsword PNG YOK, uretilmedi" | `Assets/Resources/Weapons/Warblade_Greatsword.png` VAR — 280 byte PLACEHOLDER (Multiple mode, 2 sub-sprite ~20x6+~38x13 px, PPU 64, pivot 0.18,0.5). | PNG var ama kullanilamaz. SIL + yeni uret. |
| "Weapon PPU 100 + body PPU mixed" | Tum 8 Warblade body rotation sprite UNIFORM **PPU 64** (Rotations/ klasoru .meta verify). | Weapon **PPU 64** kalir. Body re-import GEREKMEZ. PPU 100'e gecis = gereksiz churn (Codex'in "mixed PPU" okumasi stale kopyaya aitti). |
| "W/SW/NW flipX mirror weapon" | `PlayerAnimator.cs` satir 103 `sr.flipX = false` EXPLICIT. 4-way DIAGONAL (NE/NW/SW/SE) kullaniyor, FacingDir8 enum'u KULLANMIYOR. | flipX-mirror weapon plani live kodla CELISIR. Weapon yon = 4-diagonal facing'den turetilir, flipX'ten DEGIL. |
| "OrientationSync/WeaponSorter LIVE, sadece PlayerAnimator cagrisi verify" | OrientationSync.Sync() + WeaponSorter.UpdateSort() HICBIR yerden cagrilmiyor — DEAD CODE. PlayerController.FacingDirection => Vector2 (FacingDir8 degil). Vector2->FacingDir8 bridge YOK. | Weapon-mount orientation tamamen UNWIRED. BLOK A'nin asil isi bu bridge. |
| "WeaponDatabaseSO tek dogru entry" | 2 asset var: `Assets/Resources/WeaponDatabase.asset` (HandAnchorAttach bunu okur, prefab = sade Transform+SR) + `Assets/ScriptableObjects/Weapons/WeaponDatabaseSO.asset`. OrientationSync/WeaponSorter farkli prefab'ta (`Prefabs/Combat/Weapons/Warblade.prefab`), aktif degil. | Hangi prefab'in aktif oldugunu netlestir; component'leri aktif prefab'a tasi. |
| "VFX altyapisi yok, sifirdan" | VAR: `CombatEventBus.cs` (OnHit/OnKill), `VFXRouter.cs`, juice driver'lar (ScreenShakeDriver/HitPauseDriver/DamageNumberDriver), `SlashArcVFX.cs`, `HitImpact.cs`, `DeathVFX.cs`. Prefab: HitSpark/DeathBurst/SlashArcVFX. AMA hicbiri sahneye wire EDILMEMIS. | VFX = wire-up isi, sifirdan kod degil. Buyuk avantaj. |

> ⚠️ **STALE STATUS NOTE (2026-06-08 audit):** The "OrientationSync dead/unwired" claim above is stale. `OrientationSync` is live-wired in canonical `Assets/Prefabs/Player.prefab`; `HandAnchorAttach` wires the spawned weapon through `SetWeaponTransform()` and calls `Sync()` per `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md`. PPU 64 and production-order locks in this document remain canonical.

---

## 1. LOCKED KARARLAR (5 madde)

### L1 — VFX-FIRST / GRAYBOX-COMBAT-FIRST sira (sira ters cevrildi)
Eski plan: asset -> mount -> anim -> VFX. **YANLIS.** Codex + agy + Colossus raporu UCU de ayni: combat timing (wind-up/active/recovery + hitbox + hitstop) bir GRAYBOX uzerinde once kilitlenir, art SONRA uretilir. Aksi halde "silah cok yavas" geri bildirimi gelince tum body anim + weapon asset cope. **LOCK: BLOK A (graybox combat + mount bridge + VFX juice prototype) -> timing freeze -> BLOK B (anim) -> BLOK C (weapon asset) -> BLOK D (entegrasyon polish).**

### L2 — VFX kaynak ayrimi: PROSEDUREL=Unity, FORM/HACIM=PixelLab (Codex kriteri agy ile rafine edildi)
Codex "kucuk=Unity, buyuk=PixelLab" dedi; agy duzeltti ve bu daha dogru: **"Dinamik/prosedurel veri gerektiren (rastgele yayilim, trail, flash, shake, sayilar) = Unity. Piksel-grid butunlugu + hacim + el-cizimi form (slash arc, ultimate, boss death) = PixelLab flipbook."** Tablo BLOK D'de.

### L3 — Detached weapon SADECE idle/walk/run icin; ATTACK swing'de weapon-inclusive slash arc
agy Q2 kritik bulgu: 70-80 derece top-down'da TEK weapon sprite'i 360 dondurmek "karton/yapistirma" gorunur (foreshortening). Hades/CoM attack aninda ya embedded ya weapon-inclusive flipbook kullaniyor. **LOCK: Faz 1'de idle/walk/run = detached HandAnchor weapon (sprite rotate OK, statik pozda kabul). Attack swing aninda = weapon sprite'i KAPAT, swing'i weapon-iceren slash-arc VFX (PixelLab flipbook) tasi.** Bu, `project_weapon_pipeline_lock` (decoupled weapon) ile CELISMEZ — decouple korunur, sadece attack frame'inde weapon SR.enabled=false + slash arc devreye girer. Bridge kodu (L4) sortlamayi zaten dinamik yonetir.

### L4 — Weapon-mount bridge = HandAnchorAttach icinde 4-diagonal (Codex (c) secenegi)
OrientationSync/WeaponSorter dead code + PlayerAnimator 4-dir diagonal kullandigi icin, en az-LOC + en az-regresyon yolu: HandAnchorAttach Level1'e LateUpdate ekle, PlayerController.FacingDirection oku, NE/NW/SW/SE'ye snap et, weapon localRotation (SE -45/NE 45/NW 135/SW -135) + sortingOrder (NE/NW behind, SE/SW front) dogrudan set. PlayerAnimator'a DOKUNMA. OrientationSync/WeaponSorter'i yeniden wire ETME (deger tablolari referans olarak kullanilir). **Tahmini 35-55 LOC.** Weapon PPU = **64** (body ile ayni, scale telafisi yok).

> ⚠️ **STALE (2026-06-08 audit):** The 4-diagonal bridge prescription above is historical. `OrientationSync` is now wired on `Player.prefab` and called by `HandAnchorAttach`; do not use this paragraph as current implementation status. Keep the PPU 64/order lock.

### L5 — Faz 1 = Level1 Static mount; Level2 SpriteHandData ERTELENDI
twoHanded + orientBetweenHands Level1'de etkisiz (sadece metadata). Faz 1 iki-el gorunumu: weapon pivot = grip, anchorOffset = iki el gorsel ortasi, rotation L4 bridge'den. Level2 per-frame hand data = Faz 2+ (her frame icin SO pahali, demo'ya degmez).

---

## 2. NUMARALI SIRALI GOREV LISTESI

Format: **# — ne / KIM yapar / kaynak / bagimlilik / basari kriteri / efor**

### BLOK A — GRAYBOX COMBAT + MOUNT BRIDGE + JUICE PROTOTYPE (timing kilitleme — ART YOK)

**A1 — Aktif WeaponDatabase + prefab netlestir.**
KIM: Sonnet (codebase) + kullanici (Unity Inspector verify).
Kaynak: Unity (kod/asset).
Bagimlilik: yok.
Kriter: HandAnchorAttach hangi WeaponDatabase.asset'i okuyor + o entry hangi prefab'i isaret ediyor net; OrientationSync/WeaponSorter component'leri (varsa kullanilacaksa) aktif prefab'ta. 2 dead asset/prefab'tan hangisi gomulecek karar.
Efor: 0.5 sa (verify + temizlik).

**A2 — Weapon-mount bridge yaz (HandAnchorAttach Level1, 4-diagonal).**
KIM: Sonnet write + Codex review (mekanik kod, code_writer_rotation).
Kaynak: Unity (kod, ~35-55 LOC).
Bagimlilik: A1.
Kriter: LateUpdate'te FacingDirection -> NE/NW/SW/SE snap -> weapon localRotation + sortingOrder set. PlayerAnimator'a dokunulmadi. Compile 0 err. Placeholder bir kutu sprite ile 4 yonde dogru aci + sort gorulur.
Efor: 1-1.5 sa.

**A3 — Graybox hitbox + combat timing (wind-up / active / recovery).**
KIM: Sonnet write + Codex review.
Kaynak: Unity (kod, MeleeChainBehavior mevcut — ExecuteCombo same-frame damage; timing parametrize et).
Bagimlilik: A2.
Kriter: Attack 3 fazli (wind-up Xms / active Yms / recovery Zms), hitbox sadece active frame'de. Parametreler Inspector'dan tunable. PlayMode'da bir mob'a vurus calisir.
Efor: 1.5-2 sa.

**A4 — Juice prototype wire (hitstop + screen shake + hit-flash + slash arc).**
KIM: Sonnet write + Codex review.
Kaynak: Unity (mevcut CombatEventBus + juice driver'lari sahneye wire et; sifirdan kod degil).
Bagimlilik: A3. Ayrica: PlayerController.TryDash'e `CombatEventBus.PublishDash` ekle (dash trail icin; su an sadece demo yayinliyor — Codex bulgusu).
Kriter: CombatEventBus.OnHit -> hitspark + hitstop + shake + hit-flash + damage number tetiklenir. agy reçetesi (Bolum 3) baz: normal 50-80ms hitstop, shake 0.05-0.1u; crit 120-180ms, shake 0.2-0.3u; hit-flash 1 frame solid beyaz + 3 frame fade.
Efor: 2-3 sa.

**A5 — COMBAT TIMING FREEZE (gate).**
KIM: kullanici (playtest onay).
Kaynak: n/a (karar).
Bagimlilik: A2-A4.
Kriter: Graybox combat 10-15 sn oynanip "vurus hissi dogru, tempo dogru" onayi. **Bu onay olmadan BLOK B/C baslamaz.** Wind-up/active/recovery frame sayilari + slash arc timing burada KILITLENIR -> anim/asset bu sayilara gore uretilir.
Efor: 0.5 sa.

### BLOK B — ANIMASYON (silahsiz body, kilitli timing'e gore)

**B1 — Weaponless CHARACTER block uygula (PixelLab character desc).**
KIM: kullanici (PixelLab Web UI).
Kaynak: PixelLab (gen yok, sadece description edit).
Bagimlilik: A5 (timing freeze — frame sayisi netlesti).
Kriter: ANIMATION_PROMPT_CATALOG Bolum 4.1 weaponless CHARACTER block; "two-handed greatsword" -> "EMPTY HANDS weapon-ready grip".
Efor: 0.25 sa.

**B2 — Tier 1 south-only anim uret: Idle 4f, Walk 8f, Hurt 4f, Death 6f.**
KIM: kullanici (PixelLab Web UI, manuel — gece otonom YASAK).
Kaynak: PixelLab.
Bagimlilik: B1.
Kriter: 4 anim south-only, weaponless, ANIMATION_PROMPT_CATALOG prompt'lari. ~6-12 gen. Unity import + clip.
Efor: 0.5-1 gun (gen + import + clip).

**B3 — Basic Attack south SPLIT uret (Apex State + Part1 + Part2).**
KIM: kullanici (PixelLab Web UI; create_character_state MCP user-approved exception OK).
Kaynak: PixelLab.
Bagimlilik: B1, A5 (active-frame = apex frame timing'e gore).
Kriter: 8f total, apex frame A5'te kilitlenen active-frame'e denk gelir. Weaponless (silah swing'i slash arc VFX tasiyacak, L3). Apex 20-40 gen + Part 2x2 gen.
Efor: 0.5-1 gun. **R1: credit check ZORUNLU once.**

**B4 — Animator state machine wire (idle/walk/hurt/death/attack).**
KIM: kullanici (Unity Animator, kod yok).
Kaynak: Unity.
Bagimlilik: B2, B3.
Kriter: m_LoopTime=1 (idle/walk), =0 (attack/hurt/death). Attack clip'inde active-frame'e AnimationEvent VEYA (Codex bulgusu: clip'lerde event yok, MeleeChainBehavior same-frame damage) — A3 timing ile senkron. ComboStep + Attack trigger mevcut PlayerAnimator ile uyumlu.
Efor: 1-2 sa.

### BLOK C — WEAPON ASSET (en son — kilitli ele gore)

**C1 — Mevcut placeholder greatsword PNG sil.**
KIM: kullanici/Sonnet.
Kaynak: Unity.
Bagimlilik: yok (paralel A/B ile).
Kriter: `Assets/Resources/Weapons/Warblade_Greatsword.png` (280b placeholder) ve .meta temizlenir veya overwrite icin isaretlenir.
Efor: 0.1 sa.

**C2 — Greatsword PNG uret (128x256, PPU 64, handle pivot).**
KIM: kullanici (PixelLab Web UI manuel) VEYA $imagegen -> pixel cleanup -> Remove BG.
Kaynak: PixelLab (idle/walk/run mount sprite'i).
Bagimlilik: B4 (body anim el pozisyonu netlesti — weapon ona gore pivot).
Kriter: 128x256, PPU **64** (body ile ayni), pivot = grip handle ortasi (~y 0.22-0.28). Transparent BG. WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN Bolum 2.3 prompt (PPU 100 -> 64 duzelt).
Efor: 0.5 gun.

**C3 — Weapon PNG import + WeaponDatabase entry + prefab wire.**
KIM: kullanici/Sonnet.
Kaynak: Unity.
Bagimlilik: C2, A1.
Kriter: PPU 64, pivot grip, prefab SpriteRenderer beslenir. WeaponDatabase Warblade/Base entry: anchorOffset = iki-el-ortasi (PlayMode tune), twoHanded=true (metadata), gripOffset PlayMode ince ayar.
Efor: 0.5-1 sa.

### BLOK D — VFX ASSET + ENTEGRASYON POLISH

**D1 — Slash arc flipbook (weapon-inclusive) uret/wire.**
KIM: kullanici (PixelLab — buyuk hacimli form, L2) + Sonnet (SlashArcVFX wire).
Kaynak: **PixelLab** (slash arc = form/hacim, agy: el-cizimi flipbook).
Bagimlilik: A4 (SlashArcVFX mevcut), A5 timing.
Kriter: Attack active-frame'inde weapon SR.enabled=false (L3) + slash arc flipbook play. Cyan #00FFCC brand-uyumlu.
Efor: 0.5 gun.

**D2 — Unity-side VFX final tune (hitspark, dash trail, hit-flash, shake, crit, death).**
KIM: Sonnet (mevcut prefab/driver tune).
Kaynak: **Unity** (prosedurel, L2).
Bagimlilik: A4.
Kriter: hitspark particle (HitSpark.prefab), dash trail (ghost sprite/TrailRenderer), hit-flash shader, screen shake (linear impulse normal / decaying sine heavy), crit overlay (vignette + buyuk damage number; opsiyonel PixelLab cut-in nadir), death = DeathBurst particle (boss = PixelLab flipbook, Faz 4).
Efor: 1-2 gun.

**D3 — ENTEGRE TEST: body + weapon mount + 4-dir + attack-swing-arc + VFX + juice.**
KIM: kullanici (playtest).
Kaynak: n/a.
Bagimlilik: B4, C3, D1, D2.
Kriter: 4 yonde weapon dogru aci+sort (idle/walk); attack'ta weapon kapanip slash arc; hitspark+hitstop+shake+flash impact frame'de; dash trail. "Karton" hissi yok. Faz 1 demo combat loop tam.
Efor: 0.5-1 gun.

### BLOK F — FAZ 4 OUTLINE (demo onayi SONRASI, detay yok)
- 9 class weapon (canon-corrected PPU64 outline: katana 96x192, dagger 64x96, Ranger bow 128x192, Ravager greataxe 128x192, Gunslinger rift-tech pistol, Hexer grimoire/totem/scepter, Summoner tome/orb, Elementalist floating rune disc, Brawler NO weapon). Old staff/orb-for-Elementalist, Hexer whip, western/flintlock Gunslinger, and Brawler gauntlet-as-weapon terms are FORBIDDEN. Her biri WeaponDatabase entry + prefab where applicable.
- Cross-class anim rollout (Elementalist -> Ranger -> Shadowblade -> Ronin -> Gunslinger -> Ravager -> Hexer -> Brawler -> Summoner).
- Boss/elite death = PixelLab flipbook (L2 form kriteri).
- 5-dir expansion (S->E->N->SE->NE; W/SW/NW = 4-dir diagonal facing turetir, native gen DEGIL).
- Level2 SpriteHandData per-frame (yumusak mount gerekirse).

---

## 3. JUICE REÇETESI (agy, Faz 1 baz degerler — A4'te uygula)
- **Hitstop:** normal 50-80ms (3-5 frame @60fps), crit/heavy 120-180ms + dusman 1-2px geri otele.
- **Screen shake:** normal sure 0.1s amplitude 0.05-0.1u linear-impulse (darbe yonunde); heavy 0.25s amplitude 0.2-0.3u decaying sine.
- **Hit-flash:** ilk frame solid beyaz (RGB 1,1,1 / HDR emissive), sonraki 3 frame lineer fade.
- **Spawn timing:** slash arc = active ilk frame; hitspark = darbe degme noktasinda hitstop basladigi an.

---

## 4. RISK + ACIK SORU
- **R1 (KRITIK):** PixelLab credit. Tier 1 south-only ~28-52 gen, apex state pahali (20-40). BLOK B baslamadan balance check.
- **R2 (Codex/agy conflict, COZULDU):** Codex "PPU 100'e normalize", agy/Opus verify "body uniform PPU 64". KARAR: PPU 64 kalir, body re-import yok (Codex stale kopya okumus).
- **R3 (Codex/agy conflict, COZULDU):** sira (asset-first vs VFX-first). KARAR: VFX/graybox-first (L1) — 3 kaynak uzlasti.
- **R4 (agy uyari):** detached weapon attack'ta karton gorunur. KARAR: L3 (attack'ta weapon kapan + slash arc). Idle/walk/run'da detached kabul.
- **R5 (acik):** A1'de iki WeaponDatabase asset + iki prefab var. Hangisi canonical? Kullanici/Sonnet Unity'de netlestirmeli (BLOK A1 gate).
- **R6 (Codex):** CombatEventBus/VFXRouter/juice driver'lar kodda var ama sahnede wire yok. A4 = wire isi; gizli "yok sayilan obje" riskine dikkat.

---

## Cross-links
- WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md (Bolum 1 prompt'lar + 2.3 weapon spec — PPU 100->64 duzelt, sira SUPERSEDED)
- ANIMATION_PROMPT_CATALOG.md (11 anim + 6 apex state prompt'lari)
- Assets/Scripts/Systems/Combat/HandAnchorAttach.cs (L4 bridge buraya)
- Assets/Scripts/Combat/OrientationSync.cs + WeaponSorter.cs (deger tablolari referans, dead code)
- Assets/Scripts/Player/PlayerAnimator.cs (4-dir diagonal, flipX=false — DOKUNMA)
- Assets/Scripts/Combat/CombatEventBus.cs + VFXRouter.cs + Juice/* (A4 wire)
- Memory: project_weapon_pipeline_lock, project_weapon_system_8dir_lock, project_juice_features_v1, project_demo_phase1_milestone_lock
