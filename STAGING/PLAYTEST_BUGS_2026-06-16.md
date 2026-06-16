# Playtest Bug Batch — 2026-06-16 (kullanıcı canlı test, screenshot)

Kaynak: kullanıcı _Arena playtest + screenshot (reward draft "ODA 1"). Demo 19 Haz. Hepsi council-gate + 0-Unity-error + (gerekirse) screenshot ile çözülecek.

## B1 — Tooltip dikey alt-alta (U1) · GÖRÜNÜR · MED-HIGH
- **Belirti:** reward kart hover'da metin tek karakter/satır dikey şerit (screenshot: orta kart üstünde "Opportunistic Strike / CC altındaki hedefe" dikey). Kullanıcı: "her yerde çıkmalı mı emin değilim."
- **Yer:** `Assets/Scripts/UI/TooltipSystem.cs` (+ SkillOfferUI hover tetiği).
- **Fix:** TMP `enableWordWrapping=true` + `preferredWidth`/`ContentSizeFitter` ya da sabit genişlik; width 0'a çökmesin → dikey sarma biter. **KARAR:** reward kart zaten tam açıklama gösteriyor → hover-tooltip kartlarda GEREKSİZ olabilir; kart üstünde tooltip'i KAPAT (sadece kompakt yerlerde göster). Kullanıcıya doğrula.
- ⚠️ ax_pro notu: SkillOfferUI chip `wordWrapping=false`+Ellipsis (T7 bağlamı) — aynı yer.

## B2 — Attack cursor'a gitmiyor (AIM-02) · GAMEPLAY-KRİTİK · HIGH
- **Belirti:** saldırı mouse/cursor yönüne değil karakter facing/last-move yönüne gidiyor.
- **Paket prensibi (06_AIM doc):** tek aim kaynağı = AimService; CastContext attack başında `AimDirection`/`CursorWorldPoint` snapshot alır; hit geometry bunu kullanır (facing DEĞİL). Projectile/Cone/Line = AimDirection; Ground-target = CursorWorldPoint.
- ⛔ **CANON REJECT:** paketin "4-cardinal S/E/N/W + flip-yok + ayrı clip" kısmı = **8-yön LOCKED ile çakışır, KULLANMA.** Sadece "attack cursor'a gitsin" prensibini al; sprite mevcut 8-yön facing'i kullanmaya devam.
- **Yer:** aim/attack-targeting kodu (grep: PlayerController/AimService/WeaponSlot/skill cast). İncele → cast hedefini cursor world point yap.

## B3 — Karakter çok uzağa vuruyor · HIGH (B2 ile bağlı olabilir)
- **Belirti:** attack menzili/erişimi fazla (ss yok).
- **Hipotez:** ya attack range fazla, ya aim cursor'u yanlış projekte ediyor (B2 ile aynı kök olabilir — yanlış yön → hedef nokta uzakta). B2 ile birlikte incele; gerekirse menzil clamp.

## B4 — Kapı collider'ı çok büyük → takılıyor · MED (gameplay-blocking)
- **Belirti:** kapının kapladığı (collision) alan görselden büyük → oyuncu mapte takılıyor. "Resim kadar kaplasın."
- **Fix:** kapı prefab/objesi collider'ını sprite footprint'ine küçült (grep: DoorTrigger / door prefab collider).

## B5 — Walkable alan belirsiz → PLAYTEST yürüme gerekli · MED
- **Belirti:** obje konumları + zemin biten yerler (ORTA boşluklar dahil) net değil. Kullanıcı: "yürüyerek test et, walkable alanı net belirle."
- **Plan:** play-mode'da oyuncuyu obje/kenar/orta-boşluklara yürüt (input enjekte) + collision/OOB gözlemle → walkable bounds netleştir (cliff-kenar collider'ları + obje collider'ları). No-leak disiplini.

## B6 — F2 ve " çalışmıyor · ROOT-CAUSE BULUNDU · HIGH (centerpiece-blocker)
- **KÖK NEDEN:** `BuildModeController.EnterBuildMode` (l.218-221) draft/overlay AÇIKKEN erken-return ("centerpiece protection"). Kullanıcı reward draft açıkken denedi → bloklandı = TASARIM.
- **AKSİYON:** önce ödül seç (draft kapanır) → F2/" çalışmalı. Hâlâ çalışmazsa = `DirectorMode.Instance` null (RIMA-002 bootstrap precondition; full-flow'da Director/BuildMode kurulmaz). Play'de doğrula: dev-direct _Arena'da DirectorMode+BuildModeController bootstrap oluyor mu.

## Öncelik sırası (demo)
1. **B2+B3 (aim/cursor+range)** — gameplay-kritik, oyun hissini bozuyor.
2. **B6 (F2/")** — centerpiece-blocker (draft-sonrası doğrula + precondition).
3. **B1 (tooltip)** — görünür, hızlı fix.
4. **B4 (kapı collider)** — takılma.
5. **B5 (walkable)** — playtest yürüme (B4 ile birlikte).
- Hepsi: cx-impl (B1/B2/B4 well-specced) + orchestrator playtest (B5/B6/aim-verify) + council gate. 4-cardinal her yerde REJECT.

## EXEC GÜNCELLEME (2026-06-16)
- **B2 DONE** (builder-opus + ax_flash council). Kök = stale PlayerPref `AttackAimMode=0`; fix = migration-key bump (`_20260503`→`_20260616`) self-heal + live pref=TowardsMouse. PlayerController.cs +4/-1, 0-error, persistence ✓. Council 1-4 PASS. **B3 (uzağa) = aynı kök, çözüldü** (menzil değil).
- **B2-FOLLOWUP (post-demo, demo-kritik DEĞİL):** ax_flash buldu — `AimedShot.cs` (Ranger, 1.5s şarj) + `Blizzard.cs` (Elementalist, 3s channel) `player.FacingDirection`'ı bekledikten SONRA okuyor → 0.18s combat-lock bitmiş → movement-facing. Fix = rutin başında aim yön/hedef cache'le (snapshot). Demo=Warblade olduğu için golden-path'te yok; post-demo onar.
- **B1 DONE** (cx + self-verify): SkillOfferUI kart-hover'da tooltip kaldırıldı (hover SFX + chain-pulse KORUNDU), TooltipSystem MinWidth=220/Preferred=280/wrapping-on/clamp → dikey-çöküş imkansız. SkillOfferUI.cs + TooltipSystem.cs, 0-error, persistence ✓.
- **B4 REASSESS (canlı collider verisi):** kapılar (ExitDoor_Elite/Chest) = **0.8x0.7 BoxCollider2D TRIGGER** → küçük + BLOKLAMAZ. "Kapı çok yer kaplıyor/takılıyorum" = yanlış-teşhis; gerçek takılma = oda **`Collision` CompositeCollider2D (14.4x9.2)** = cliff-kenar/walkable sınırı (odada toplam 5 collider: 1 composite + 2 küçük solid + 2 door-trigger). → B4 ve B5 AYNI iş.
- **B5 (walkable):** walkable = CompositeCollider2D. Composite görsel floor'a (orta-boşluklar dahil) uyuyor mu = **temiz play-walk + screenshot ile teyit** (collider boundary vs sprite floor). Gerekirse composite/cliff collider düzelt.
- **B6 (F2) — DAHA DERİN + temiz-verify gerek:** canlı testte `DirectorMode.Instance=NULL` çıktı (BuildModeController var) → `EnterBuildMode` DirectorMode null'sa erken-return = F2 ölü. AMA o play session ANORMAL'di (UIManager.Instance da null; "zaten play'deydi" = stale/leak session) → bulgu güvenilmez. **Temiz dev-direct _Arena play ile DirectorMode bootstrap'i doğrula** (RIMA-002: full-flow'da kurulmaz). Draft-guard (l.218-221) ayrıca geçerli.
- ⚠️ **Editör "already in play mode" bulundu** (bir dispatch play'i açık bırakmış) → stop edildi, pmss=MainMenu restore. Sonraki play-verify temiz başlasın.
- **Bu tur DONE:** B1, B2, B3. **Kalan:** B4+B5 (walkable play-walk+screenshot, birlikte) · B6 (temiz F2 bootstrap verify). Hepsi temiz play-test gerektiriyor.
