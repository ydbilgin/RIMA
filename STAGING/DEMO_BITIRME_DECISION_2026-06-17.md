# DEMO BİTİRME DECISION — 2026-06-17

> **Kaynak:** ultracode council workflow `wf_78b723a8` (23 agent, graphify+22-ekran+kod toprakli, adversarial-verify) + telegraph design agent (`ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`) + A1 prop import (F2 19 prop). Demo ~19 Haziran. Kısıt: balance=0 (yeni gen YOK), tek cx-bugra Unity-agent (seri), "yeşil-assert ≠ çalışıyor".

## TEK CÜMLE VERDICT
Son ~1 günü **CANLI Edit-to-Play hikâyesine** harca (CombatJuice + arena döşeme + enemy kontrast) + sıfır-kod sunum kazanımları (audio prova, run-map/draft canlı yakalama). 6 biten sistemi KORU. Demo screenshot zip'iyle değil, hocanın canlı provada **gördüğü+duyduğu** ile kazanılır. Combat eye-candy ve 5-enemy animator wire = TUZAK (kırılgan yol, sıfır görünür fark).

## MUST-HAVE SENARYOLAR (demo'nun omurgası)
1. **Edit-to-Play tek kesintisiz beat** — F2'de prop yerleştir → çık → AYNI odayı oyna. Tez (%60 mimari: "reusable in-game tooling") buna bağlı. Şu an F2 ve combat ayrı oda gibi yakalanmış. Teknik gerçek (room_current.json persist). Combat-dışı oda (Merchant/dressing) kullan → spawnProps=false combat gate'inden kaç. **Effort: S (pre-bake JSON + prova).**
2. **Full-flow tek yol:** GATE → Director (stat-bump + canlı enemy spawn) → combat → Penitent boss → reward draft → run-map. 6 alt-sistem DONE. SEAM'leri (F2 in/out, Director, portal) tek seferde doğrula — "green-assert burned us" tam burada. **Effort: S (sıralama + 1 zorunlu kesintisiz dry-run).**
3. **Skill-draft pick (synergy chip sesli oku) + run-map (M) 8 node-art tipi** — progression loop'un pakette DOĞRU screenshot'u YOK (20/21 overlay'i yakalayamadı, ScreenSpaceOverlay). Canlı render oluyor. Run-map'te 8 bitmiş node sprite branching'i KANITLIYOR — en güçlü progression kanıtı, şu an hocaya "hiç" gösteriliyor. **Effort: XS (sunum cue + gerekirse OBS grab).**

## BEAUTIFICATION — sıralı (etki/efor)
1. **CombatJuice.prefab → `_Arena.unity`** [S/HIGH] — combat-feel stack (number/hit-stop/shake/punch/VFX) kodlu+bağlı ama `_Arena`'da YOK (GUID `2b4f2f85d031f4e429fe752646926eb7`). Tek instantiate. **#1 etki/efor.** Not: 5 component (HitPause/Shake/CamPunch/VFXRouter/DamageNumber), HitFlash/ImpactFrame EKLEME. Risk: düşük (unscaled-time, timeScale-immune). TEST: melee→number+jolt, kill→0.12s freeze, sonra F2 in/out + Director aç/kapa + tekrar melee (overlay'e timeScale sızmıyor mu).
2. **`Enemy_SeloutOutline.mat` → FractureImp/HalfThrall/Penitent** [S-M/HIGH] — #1 cross-screen kusur: koyu mob'lar slate #3A3D42 zeminde görünmez. `SeloutOutline.shader` mevcut (_OutlineStrength ~0.55, ember-nötr, cyan DEĞİL). Risk: düşük (mat-ref değişimi, trivial rollback). DoorTrigger atla (runtime sprite).
3. **Arena'yı 5-6 prop ile döşe (Unity Editor'den, F2-save DEĞİL)** [S/HIGH] — boş oda "hiçbir şey olmuyor" okunuyor; döşeme hem doldurur hem "Build Mode ile authored" tezini görsel kanıtlar. **KULLANICI elle yerleştirsin** (verified sahneyi remote-agent bozmasın). 19 prop repo'da. Risk: combat spawnProps=false → template prop görünmez; scene-child olarak koy VEYA Merchant/dressing oda; spawn koridorlarından/boss footprint'ten uzak, blocksWalkable=false; sonra 1 dalga playtest (nav).
4. **1 döşeli odayı `room_current.json` pre-bake + commit** [S/MED-HIGH] — en riskli canlı seam'i deterministik reload'a çevir. RoomLayoutSerializer.CurrentJsonPath + RuntimeRoomManager mevcut. TEST: read-back; sapma varsa canlı-place provasına düş.
5. **🎯 TELEGRAPH — boss P2/P3 + 2 küçük extension** [M/HIGH] (telegraph spec'ten, kullanıcı isteği) — sistem ZATEN var (`Enemies/EnemyTelegraph.cs` SpawnCircle/Line/Cone + decal + windup + teardown). Eksik: boss Phase-2/3 saldırıları + HolyLash/ShackleThrow yer-telegraph'sız; ChainExplosion 3s gecikmeli AoE görsel çizilmiyor. İş: `EnemyTelegraph.cs` +`SpawnDelayedRing`/+`FlashImpact` (SkillVfx.ImpactBurst sarmalar); `PenitentSovereign.cs` 6 saldırıya telegraph+snap. ⚠️ telegraph süresi gerçek windup ile BİREBİR eşleşmeli (ChainExplosion 3s ↔ ring delay, P3 0.85x) yoksa "yalan telegraph". Habersiz mermi = mevcut Throw zaten telegraph'sız. Orphan `Enemy/Telegraph/` → `[Obsolete]` (silme). Spec: `STAGING/ENEMY_TELEGRAPH_VFX_SPEC_2026-06-17.md`.
6. **Merchant shop-stand teardown'u boss öncesi doğrula** [S/MED] — boss climax'ına sarı-kare placeholder kart sızıyor. RoomRunDirector.DestroyActiveShop(). Capture-staging mi canlı-bug mu önce ayırt et (room-by-room), ShopRoomController refactor YOK.
7. **Audio prova + `music_demo.wav` bed** [XS/MED] — 18/19 cue ZATEN canlı çalışıyor (AudioManager, 42 call-site). Sadece canlı duyulabilirlik provası + 1 CC0/royalty-free ambient loop (`Resources/Audio/music_demo.wav`, import-only). Sessiz demo = "bitmemiş". CC0 zorunlu (akademik teslim).

## KİLİTLİ EXECUTION SIRASI (her adımda elle-test kapısı)
1. **6 biten sistemi TEK kesintisiz full-flow dry-run'da RE-VERIFY** [manual] — SEAM'leri izle (F2 in/out, Director, portal). >1s freeze / leftover scrim / console-error YOK. Seam takılırsa DUR-düzelt, yeni iş ekleme. **Non-negotiable gate.**
2. **CombatJuice → _Arena** [cx] — TEST: play'de melee/kill juice + F2/Director sonrası tekrar; console 0. Yeşil-assert YETMEZ, play'de izle.
3. **Enemy_SeloutOutline.mat + 3 enemy** [cx, step-2 ile aynı seri pass] — TEST: before/after combat shot, silüet okunur, cyan-budget patlamaz, telegraph okunur, console 0.
4. **Arena döşeme (5-6 prop) + room_current.json pre-bake/commit** [KULLANICI prop + cx JSON] — TEST: oyuncu tüm koridorlarda serbest (soft-lock yok), 1 dalga temiz, reload layout'u üretir, boss footprint açık.
5. **🎯 Telegraph: boss P2/P3 + extension'lar** [cx] — TEST: her boss saldırısı yer-telegraph çıkarır, windup süresi gerçek hasarla birebir, ChainExplosion ring 3s senkron, console 0; canlı boss dövüşü izle.
6. **Merchant teardown doğrula** [cx, sadece canlı-bug ise] — TEST: boss odasında 0 placard, distinct-SHA boss shot.
7. **Capture-truth: draft + run-map overlay** [manual, EN SON] — OBS/game-view grab (canvas-mode mutation DEĞİL); distinct-SHA; geçici render değişikliği geri al (no-debug-state-leak).
8. **OPSİYONEL (vakit kalırsa):** Build-UI coord-chip + tool-highlight (BuildModeUiStyle), pause/draft scrim, music bed.
9. **Sunum run-sheet** (EDIT_TO_PLAY_STORYBOARD'a ekle): keypress sırası, draft-synergy oku, run-map call-out, **graphify 6/10 god-node = AÇILIŞ kancası**, run-map = kapanış.

## DOKUNMA (korunan, DONE+tested)
GATE bootstrap · Penitent boss · HUD · reward-bleed · Build Mode placement/snap/phantom (sadece BuildModeUiStyle skin OK) · Director IDE-skin (code-skin, prefab-refactor YOK) · 3 canlı-wave enemy controller (sadece MAT swap) · spawnProps=false gate (F1 fix) · weaponless player anim.

## İPTAL (council kesti)
5-enemy black-blob wire (demo'da görünmez+kırılma) · EnemyOutlinePulse (dead path) · Build Phase-4 save write-back · PropLight2D (yeni kod) · SlashArc/HandGlow/RiftGlow (canlı attack-path) · tileset/decal import (M-L, düşük etki) · raw scene-view shot 25 (prototip okunuyor) · SHA-dedup harness · spawnProps=true zorlama (F1 soft-lock geri gelir).

## AÇIK RİSKLER
- Seri-Unity darboğazı: her cx tek bugra'dan; step 1+6 atlanırsa tam "green-assert" failure → 1 ve 6 non-negotiable.
- CombatJuice × overlay timeScale: unscaled asserted ama step-2 manuel in/out kanıtlar.
- Arena döşeme soft-lock (F1 tekrarı): collider'lı prop spawn'a yakın olmasın.
- Delivery: draft+run-map screenshot'ta görünmez → canlı/kayıt şart, yoksa 2 payoff beat kanıtsız.
- Menü seam: [Obsolete] MainMenuScreen yanlış giriş/çift-EventSystem → ilk 10sn kırılır, dry-run'da entry doğrula.
- SeloutOutline boss'ta over-brighten edebilir → gözle doğrula.
- music_demo CC0 ZORUNLU.
