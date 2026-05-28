# CODEX TASK — Warblade weapon + anim + VFX teknik feasibility

Amac: Warblade Faz 1 demo icin weapon-mount + animasyon + VFX uretim sirasinin TEKNIK olarak dogru olup olmadigini dogrula. Opus design agent senin ciktini sentezleyip kullaniciya final lock sunacak. INLINE cevap ver, dosyaya yazma.

EFFORT: xhigh

## Onceden DOGRULANMIS codebase gercekleri (Opus verify etti — tekrar bakma, USE et)
1. `Assets/Resources/Weapons/Warblade_Greatsword.png` VAR ama 280-byte PLACEHOLDER. .meta: Multiple sprite mode, 2 sub-sprite (rect ~20x6 + ~38x13 px), PPU=64, pivot 0.18,0.5. Yani plan'daki "PNG YOK" YANLIS — placeholder var, kullanilamaz durumda.
2. `OrientationSync.cs` (RIMA.Combat) public `Sync(FacingDir8 dir)` — handOffsets[8] + weaponRotations[8] LIVE degerlerle dolu. AMA hicbir yerden CAGRILMIYOR.
3. `WeaponSorter.cs` (RIMA.Combat) public `UpdateSort(FacingDir8 dir)` — N/NE/NW behind, digerleri front. AMA hicbir yerden CAGRILMIYOR.
4. `HandAnchorAttach.cs` (RIMA namespace, Systems/Combat) `Start()` -> `AttachWeapon("Base")`. Level1Static: weapon parent + localPosition=anchorOffset + identity rotation. Level2: per-sprite SpriteHandData. orientBetweenHands SADECE Level2'de kullaniliyor.
5. `PlayerAnimator.cs` 4-WAY DIAGONAL sistem (NE/NW/SW/SE) kullaniyor, Animator float DirX/DirY ile. `FacingDir8` enum'u KULLANMIYOR. `sr.flipX = false` EXPLICIT set ediyor (satir 103). Yani plan'in "W/SW/NW flipX mirror" iddiasi live PlayerAnimator ile CELISIYOR.
6. `PlayerController.FacingDirection` => Vector2 dondurur, FacingDir8 DEGIL. Vector2 -> FacingDir8 donusumu LIVE kodda YOK.
7. `WeaponDatabaseSO` tek entry: Warblade/Base, anchorOffset {x:0.2,y:0.1}, twoHanded=true, orientBetweenHands=true. (orientBetweenHands=true ama Level1'de etkisiz.)

## SANA SORULAR (teknik feasibility — net cevap ver)
S1. Weapon-mount orientation gap: OrientationSync + WeaponSorter dead code (cagrilmiyor) + PlayerAnimator 4-dir diagonal kullaniyor (8-dir degil). Faz 1 demo icin weapon'in 4-yon (NE/NW/SW/SE) dogru aci/sort almasi icin EN AZ kod yolu nedir? Secenekler:
   (a) PlayerAnimator'a Vector2->FacingDir8 bridge ekle + Sync/UpdateSort cagir (8-dir),
   (b) OrientationSync/WeaponSorter'i 4-dir diagonal'a indirgeyen yeni minimal bridge,
   (c) HandAnchorAttach icine LateUpdate'te facing oku + dogrudan rotate/sort.
   Hangisi Faz 1 demo icin en az LOC + en az regresyon riski? Tahmini LOC ver.
S2. Greatsword asset: mevcut 64x16 PPU64 placeholder'i SILIP yeni 128x256 PPU? olarak mi import etmeli? Codex senin onceki PIXELLAB_ANALYSIS'inde "PPU 64 kalsin, HandAnchor offsetleri PPU64'e gore" dedin. Plan ise PPU 100 + 128x256 oneriyor. Karakter body PPU kac? (verify et: warblade body sprite .meta PPU). Weapon PPU body PPU ile AYNI olmali mi yoksa farkli olabilir mi (ayni HandAnchor world-unit offset korunsun diye)? Net karar: PPU=64 mi 100 mu, ve hangi offset retune gerekir?
S3. WeaponDatabaseSO entry: Level1 static modda twoHanded + orientBetweenHands etkisiz. Faz 1 demo greatsword'u 2-el gorunumu icin Level1'de NE yapmali (anchorOffset elle iki-el-ortasi, rotation OrientationSync'ten)? Yoksa Faz1 icin Level2 SpriteHandData'ya gecmeli mi (ama o her frame icin SO ister, pahali)? Net oneri.
S4. VFX hook noktalari: PlayerAttack / PlayerAnimator hangi event/frame'de impact tetikleniyor? (grep et: PlayerAttack.cs OnComboStep, attack timing, AnimationEvent var mi?). Hitspark / dash trail / crit overlay hangi mekanizma ile spawn edilmeli — Animation Event mi, attack hitbox active-frame callback mi, timer mi? RIMA'da mevcut VFX spawn altyapisi (VFXSpawner/PoolManager) VAR MI? grep et, varsa path ver.
S5. VFX kaynak ayrimi: Bu efektlerin hangileri Unity-side (procedural particle / shader / trail renderer / sprite-flash) ucuz ve runtime-tunable, hangileri PixelLab flipbook olarak DAHA IYI (stilize buyuk slash arc, ultimate)? Her efekt icin (hitspark, dash trail, hit-flash, screen shake, slash arc, crit overlay, death VFX) Unity-vs-PixelLab oner + KRITER (ne zaman Unity ne zaman PixelLab).
S6. Uretim sirasi dogru mu: Opus sentez su sirayi LOCK etmeyi dusunuyor: BLOK A (engine: weapon-mount bridge fix + asset import slot + WeaponDatabaseSO + prefab wire) -> BLOK B (greatsword PNG uret) -> BLOK C (anim uret) -> BLOK D (VFX wire) -> BLOK E (mount + 4-dir + VFX integrated test). Bu sirada teknik bagimlilik HATASI var mi? Ozellikle: weapon-mount bridge'i asset VARKEN mi yoksa placeholder ile mi test etmeli (BLOK A vs B sirasi)?

## KISIT
- Faz 1 = Warblade TEK (diger 9 class Faz 4, detaya girme).
- Min kod, no speculation. LOC tahminleri ver.
- INLINE cevap. Net madde madde S1-S6.
