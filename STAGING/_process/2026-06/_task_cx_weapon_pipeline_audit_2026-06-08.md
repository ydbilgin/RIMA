ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.
DO NOT DELEGATE — do this yourself.
DO NOT use Unity MCP / do NOT enter Play mode — another agent owns Unity right now. This is STATIC file/code analysis only.

# Amaç
Execute the weapon/animation pipeline AUDIT below. ANALYSIS ONLY — produce a report, write NO code. Read `AI_READER_GUIDE.md` (repo root) first + obey its REVOKED/CURRENT rules.

## ⚠️ CRITICAL — the brief below was AUTHORED BY ChatGPT from reading the repo. It contains ChatGPT's OWN GUESSES ("benim önerim", "bence", "benim tahminim", proposed schemas, proposed mob set). ChatGPT has been STALE/WRONG before on this project. So:
- VERIFY every ChatGPT assumption against the ACTUAL code/docs. For each guess, mark **CONFIRMED** or **REFUTED** with file:line evidence.
- Do NOT accept ChatGPT's proposed answers as fact. Ground-truth first, then decide.
- Explicitly call out anything ChatGPT claims that is already implemented/decided in the live code (the recurring "proposes things already done" pattern).

## Output
Write the full structured report (sections 1-9 of the brief) to: `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md`
File:line evidence on every finding. Severity tags. End with the "Bugün yapılmalı / post-demo / asla" decision. NO code changes. Also write a short pointer + the executive verdict to CODEX_DONE.md.

---

# ===== BRIEF FROM CHATGPT (execute this, but verify its assumptions) =====

RIMA repo: ydbilgin/RIMA, branch: master.

Görev: Silahsız-body animasyonu + silahı ele/avuç üstüne mount etme pipeline'ını HOLİSTİK ve ACIMASIZ şekilde review et. Kod yazmaya başlamadan önce önce audit raporu üret. Önce repo kökündeki AI_READER_GUIDE.md dosyasını oku ve REVOKED / CURRENT STATUS kurallarına uy.

İncelenecek dosyalar:
KOD: Assets/Scripts/Systems/Combat/HandAnchorAttach.cs · Assets/Scripts/Combat/OrientationSync.cs · Assets/Scripts/Systems/Combat/WeaponDatabaseSO.cs · Assets/Scripts/Data/SpriteHandData.cs · Assets/Scripts/Editor/Combat/OrientationSyncAnchorEditor.cs
PLAN/KARAR: STAGING/WEAPONLESS_ANIM_WEAPON_MOUNT_PLAN.md · STAGING/WEAPON_ANIM_VFX_PRODUCTION_LOCK.md · STAGING/CODEANIM_DECISION_2026-06-05.md
PIXELLAB/ASSET: STAGING/PIXELLAB_SESSION_PLAN_2026-06-07.md · STAGING/WEAPON_BATCH_PLAN.md · STAGING/ANIMATION_PROMPT_CATALOG.md
WEAPON PACK: STAGING/chatgpt_weapon_pack/README.md · 01_CANON_WEAPONS.md · 02_PRODUCTION_CONSTRAINTS.md

AMAÇ — kesinleştirilecek: (1) Mevcut HandAnchorAttach+OrientationSync+WeaponDatabaseSO+SpriteHandData mimarisi demo için yeterli mi? (2) 10 class özel mount davranışlarını kaldırır mı? (3) Hangi kararlar güncel, hangi doküman stale? (4) PixelLab: hedef boyut mu, büyük canvas küçültme mi? (5) Weaponless animasyon hâlâ gerekli mi yoksa code-only ile post-demo'ya mı itildi? (6) Minimal patch set nedir?

## 1) Kod mimarisi review (dosya/satır kanıtıyla)
1.1 HandAnchorAttach: Start'ta AttachWeapon("Base") doğru mu? weaponDatabase null → sessiz fail mi? classId hardcoded "Warblade" mı kalmış / runtime class'tan mı geliyor? weapon prefab spawn sonrası OrientationSync'e bağlanıyor mu? Level1Static + Level2SpriteHandData gerçekten çalışıyor mu? Level2 demo için gerekli mi yoksa post-demo mu? swing weapon alpha-fade güncel kararlarla çelişiyor mu? ÖZEL: WEAPON_ANIM_VFX_PRODUCTION_LOCK L3 "attack'ta weapon hide + weapon-inclusive slash arc" diyorsa, mevcut "fade alpha 0.4" ile çelişiyor mu? Karar: demo için fade mi kalsın, hide sadece slash-arc flipbook gelince mi? SwingVisibilityPolicy enum gerekir mi?
1.2 OrientationSync: 8 yön handOffsets+weaponRotations yatay-sağ konvansiyona uygun mu? flipY sadece lineer blade için mi doğru? Bow/pistol/grimoire/lantern/disc'te flipY bozar mı? Tek weaponRenderer varsayımı dual weapon'ları kaldırır mı? SetWeaponTransform sadece ilk SpriteRenderer'ı mı alıyor? Dual için WeaponMountView gerekir mi? Canlı kod 8 yön mü 4-diagonal mı? PlayerAnimator/FacingDir8/FacingDirection ilişkisini doğrula. OrientationSync korunmalı mı yoksa HandAnchorAttach içinde 4-diagonal minimal bridge mi?
1.3 WeaponDatabaseSO: WeaponEntry.handOffsets runtime'da kullanılıyor mu? twoHanded/orientBetweenHands Level1'de etkili mi yoksa metadata mı? dual/hover/static-torso/NoWeapon(Brawler) için yeterli veri var mı? Minimal schema çıkar. ChatGPT'nin tahmini alanlar (DOĞRULA/DÜZELT): mountMode(SingleHand/TwoHanded/DualMirrored/HoverPalm/StaticTorsoAttachment/NoWeapon) · flipPolicy(None/FlipYOnLeftDirs/FlipXOffhandOnly/Custom) · swingVisibility(KeepVisible/Fade/Hide) · leftHandPrimary · usesSwingArc · handOffsets[8] · rotations[8] · hoverOffset · offhandPrefab/child renderer · staticTorsoAttachmentPrefab. Mega-SO yaratma; minimal ama 10 class'ı taşısın.
1.4 SpriteHandData: sprite-bazlı left/right pixel anchor doğru mu? per-frame SO maliyeti demo için fazla mı? Level2 ne zaman gerekli? Karar: Demo=Level1 static mount, Post-demo=SpriteHandData per-frame. Doğru mu?
1.5 OrientationSyncAnchorEditor: 8 yön offset tuning yeterli mi? dual/hover/static-scabbard editor desteği eksik mi? sadece tek handAnchor offset mi düzenliyor, WeaponDatabaseSO entry offsetlerini düzenlemiyor mu? WeaponMountProfile editor gerekir mi yoksa şimdilik yeterli mi?

## 2) 10 class özel durum review (01_CANON_WEAPONS canonical; kırmızı çizgiler: 1 class=1 weapon=1 silhouette, varyant yok, 8 yön weapon sprite YOK, PPU64+Point, Elementalist asa yasak, Gunslinger western yasak, Shadowblade glow yasak, Brawler silahsız, Summoner staff swing yok)
Her class için: Canon weapon / Mount mode / Sprite count / Drawing direction / Runtime flip-rotation / Grip-pivot / Sorting rule / Attack davranışı / Gerekli kod desteği / Risk.
Özel: Warblade(greatsword, yatay-sağ, low guard, attack fade mi hide mı?) · Ranger(bow sol el; preferRightHand Level1'de çözmüyor olabilir → leftHandPrimary gerekir mi? bow flipY bozar mı?) · Shadowblade(twin dagger reverse-grip; tek sprite+offhand flipX; tek-renderer varsayımı yetersiz mi? DualMirrored gerekir mi?) · Ravager(dual axe; tek sprite+mirror yeter mi? iki elde sorting?) · Gunslinger(dual rift-pistol, western yasak; pistol rotation blade gibi mi custom mı?) · Elementalist(floating golden rune disc, avuç üstünde hover, HandAnchorAttach ile OLMAMALI gibi → HoverWeaponMount script gerekir mi? bob+spin+sort?) · Summoner(soul lantern sol el/hover; staff swing yok; Elementalist HoverPalm taşır mı?) · Hexer(grimoire/totem/scepter, focus object; StaticFrontFocus/HoverPalm/SingleHand?) · Ronin(katana sağ çekili, sol belde kın zorunlu; kın baked mi static-torso-attachment mı? demo için hangisi güvenli?) · Brawler(weapon yok; sargı baked mi? WeaponDatabase entry mi NoWeapon mı?).

## 3) Üretim: boyut + açı + PixelLab
3.1 Boyut: A) büyük canvas→Unity'de küçült vs B) hedef boyutta üret. Ground truth: Warblade_Greatsword 64×16 çalışıyor, PPU64, pivot sap. Dosya:satır kanıtıyla karar ver (ChatGPT tahmini: target-size kazanmalı — DOĞRULA). Çıktı: net karar+gerekçe+iptal edilen eski kararlar+hangi doc güncellenmeli. Cevapla: 128→64 downscale pixel grid bozar mı? PixelLab native üretimde hedef boyut daha mı güvenli? style_images en büyük ref boyutu batch'i belirliyorsa plan nasıl?
3.2 Çizim açısı: horizontal-right mı / vertical mı / 45° mi? Canlı OrientationSync yatay-sağ'a göre ayarlıysa değiştirme maliyetini açıkla. (ChatGPT öneri: tüm silahlar horizontal-right sap-sol uç-sağ; disk/lantern/grimoire yönsüz=rotasyonsuz özel mode — DOĞRULA.)
3.3 WEAPON_BATCH_PLAN canon drift'leri bul+düzelt: Elementalist staff/orb→rune disc · Gunslinger flintlock/western→rift-tech pistol · Hexer staff/whip→grimoire/totem/scepter · Brawler gauntlet-weapon→body cosmetic/no-weapon. Yeni batch plan öner (boyut karıştırma, style refs hedef boyuta downscale, ≤8 item/batch, sadece gerekli asset, live asset'i tekrar üretme).

## 4) Animasyon pipeline: ANIMATION_PROMPT_CATALOG vs CODEANIM_DECISION çelişkisi var mı? (Catalog SPLIT attack anim öneriyor; CODEANIM demo'da yeni char/mob anim=0, melee=weapon-hand+slash-arc+code/VFX, knockdown/stagger/death/spawn/dash code-only.) Karar: Catalog demo'da aktif mi post-demo referans mı? Weaponless attack body anim hâlâ üretilmeli mi yoksa idle/walk mevcut + code-only attack/VFX yeterli mi? SPLIT sadece post-demo/hero/boss için mi? Öneri: CATALOG başına status ekle (DEMO ACTIVE: yok/sadece referans · POST-DEMO: SPLIT body anim · DO NOT GENERATE BEFORE TIMING FREEZE).

## 5) PIXELLAB_SESSION_PLAN mob çelişkisi: T2_MOB_PROTOTYPE_SPEC 64px vs COMBAT_ROSTER (Walker 112/Void Thrall 128/Chain Warden 128/Relic Caster 80). "4 arketip" belirsiz. Karar: demo 4 mob arketipi canon hangisi? boyutlar? 64px eski fallback mi? yön 4+flipX mi? (ChatGPT öneri: Shard Walker 112 ranged · Void Thrall 128 splitter · Chain Warden 128 controller · Relic Caster 80 support; Bruiser/Fracture Imp post-demo — DOĞRULA/DÜZELT.)

## 6) Test/doğrulama planı (kod yazılacaksa önce): weapon mount tests (Warblade spawn / Brawler no-weapon / missing-entry warning-not-crash / PPU-import validation / OrientationSync 8-yön rotation derece / per-weapon flip / dual main+offhand renderer / Elementalist hover not blade-rotate / Ranger left-hand / Ronin scabbard static). Visual QC checklist (8 yön weapon ele yakın · N/NE/NW arkada · S/SE/SW/E/W önde · attack fade/hide doğru · slash arc weapon identity · bow/pistol/grimoire flipY ters mi · hover disc avuçta). Asset validation (PPU64/Point/grip-pivot/transparent/target-size/no-AA/no-wrong-class/no-Elementalist-staff/no-western/no-Shadowblade-glow).

## 7) Çıktı formatı (rapora bu başlıklar): 1.Executive verdict (pipeline sağlam mı / demo yeterli mi / 10 class eksik) · 2.Dosya dosya bulgular (dosya:satır/sorun/etki/öneri/önem KRİTİK-ORTA-KOZMETİK) · 3.Karar tablosu (PPU/çizim açısı/boyut stratejisi/animation strategy/mount mode strategy/PixelLab batch) · 4.10 class mount matrix · 5.Minimal kod değişiklikleri (hangi dosya/yeni enum-SO-script neden/~LOC/regression riski) · 6.Doküman cleanup (stale dosya/superseded satır/canonical olacak doc) · 7.Patch planı (commit sırası: audit doc→schema/profile minimal patch→dual/hover/static/no-weapon→tests→docs cleanup→asset plan cleanup) · 8.RED LIST (büyük rewrite yok, PlayerAnimator bozma yok, PPU100 yok, büyük-canvas-küçültme yok, Elementalist staff yok, western yok, Shadowblade glow yok, 8-yön weapon sprite yok, Brawler'a silah prefab yok, tüm anim'leri PixelLab'da üretmeye başlama yok) · 9.Son öneri (bugün/post-demo/asla).

Önce SADECE rapor üret. Kod yazma. Onay bekle.
