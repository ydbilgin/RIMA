ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.
RESPOND INLINE.

# Amaç (KÖK-NEDEN DEBUG)
PlayableArena_Test01'de oyuncu, void'e doğru SÜREKLİ -3 Y hızıyla kayıyor (demo oynanamaz: kamera void'e takip → "siyah ekran"). Opus probe etti ama kod-çelişkisi var. Sen tüm movement stack'i oku + kök nedeni + min-fix öner.

# Opus'un KANITLADIĞI gözlemler (play mode, MCP)
- Player rb: bodyType=Dynamic, **gravityScale=0, linearDamping=0** (drag yok → bir kez verilen hız KALICI), constraints=FreezeRotation.
- Player rb.linearVelocity = **(0.02, -3.00) SABİT** — pozisyon -3.5'ten -200'e kayıyor; mob'lar chase ediyor (cluster -200'de = "siyah ekran" kökü).
- Input YOK: moveAction.ReadValue=(0,0), Keyboard WASD hepsi False.
- isDashing=FALSE, dashDir=(0,0) → stuck-dash değil.
- Chasm/NarrowPassage YOK, player noktasında 0 collider.
- KnockbackReceiver.activeKnockback boş (decay/zero yapıyor, sabit -3 değil).
- **ÇELİŞKİ:** PlayerController.FixedUpdate (satır 263-290) input=0 iken `rb.linearVelocity = desiredVel`(=0) yapmalı (isDashing false, moveInput 0 → satır 289 vel=0). AMA runtime'da PC ENABLED iken bile vel=-3. PC DISABLED + vel-reset YOK → vel hâlâ -3 (kayma devam). → PC kapansa da açılsa da -3.
- İlk açılışta `PlayerController.enabled` scene'de FALSE serialize idi (Opus True yaptı). PlayerMovementController (legacy, memory'de buggy flag'li) enabled idi (Opus disable etti). İkisi de movement controller.
- Hiçbir script PlayerController.enabled set etmiyor (grep). Player'da KnockbackReceiver tek `rb.linearVelocity=` yazan (decay/zero).

# OKU (tam movement stack)
- Assets/Scripts/Player/PlayerController.cs (TAM — Awake/Start init, dash, FixedUpdate, rb assignment)
- Assets/Scripts/Player/PlayerMovementController.cs (legacy — FixedUpdate'i constant vel mi yazıyor? memory: Awake'te bodyType set ediyor)
- Assets/Scripts/Core/KnockbackReceiver.cs (TAM)
- Assets/Scripts/Player/PlayerPrefabSetup.cs (player init?)
- Assets/Prefabs/Player.prefab + Assets/Scenes/Test/PlayableArena_Test01.unity (Player'ın hangi component'leri enabled, rb ayarları, 2 controller durumu)

# Sorular
1. -3 Y SABİT hızı NE veriyor (tek-seferlik impulse mı, her-frame write mı)? PlayerMovementController.FixedUpdate input=0'da ne yapıyor — constant downward mı?
2. PC ENABLED iken FixedUpdate neden vel=0 yapmıyor? (erken-return path? rb referansı yanlış mı? 2 rb mı? execution-order'da PMC sonradan mı yazıyor?)
3. İki controller (PlayerController vs PlayerMovementController) — hangisi CANONICAL, diğeri silinmeli/disable mi? Combat A2 PlayerController.FacingDirection kullanıyor.
4. MIN FIX: oyuncu idle'da sabit dursun, WASD ile hareket etsin, void'e kaymasın. Hangi dosya/satır?

# Çıktı: kök-neden (kesin) + min-fix (dosya:satır) + hangi controller canonical.
