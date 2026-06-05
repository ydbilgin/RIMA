ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
CharacterSelect v4 "ATTUNEMENT CHAMBER" — **"map yapar gibi"**: seçim ekranı = GERÇEK ODA. Backdrop-resim
DEĞİL; mevcut modüler sistemle inşa (RoomTemplateSO + IsoRoomBuilder + props). Karar =
`STAGING/TBH_WALK_SELECT_DECISION_2026-06-05.md` (✅ + SUPERSEDED bölümleri). Layout referansı =
`STAGING/imagegen/attunement_chamber_bg_crescent.png` (SADECE kompozisyon ilhamı: sol-alt spawn → orta
hilal pedestaller → sağ-üst kapı; dummy cebi sol-altta).

# KURALLAR
KOD+ASSET işi; .unity dosyası DİSKTE DÜZENLEME YASAK (her şey runtime bootstrap veya yeni .asset).
"◈/elmas glif" YASAK. v3.2 Canvas ekranı SİLME — TAB-fallback olarak kalır.

# FAZLAR

## P0 — Pedestal asset ($imagegen — kullanıcı onaylı üretim)
`$imagegen` ile TEK sprite: iso düşük yuvarlak taş pedestal/platform diski, üstü düz (bir karakter durabilir),
slate-gri taş + kenarında ÇOK hafif cyan rün şeridi, 64px-char ölçeğiyle uyumlu (~128×96 görünür), transparan
arka plan (mevcut imagegen asset-pack pipeline'ınla: cell-split/chroma temizliği). Import: PPU64/Point/no-compress
→ `Assets/Sprites/Environment/Props/echo_pedestal.png` → `PropDefinitionSO` `EchoPedestal`
(`Assets/Data/Props/`, footprint 2×2, blocksMovement=true) + PropRegistry kaydı.

## P1 — Oda verisi: `Assets/Data/Rooms/Special/Chamber_CharSelect.asset` (RoomTemplateSO — GERÇEK asset)
Editor-script/CreateInstance ile üret (RoomJsonImporter pattern'i serbest). İçerik (crescent referansına göre):
- Çapraz ada walkable mask (~22×16 hücre; SOL-ALT geniş giriş platformu → ORTA gövde → SAĞ-ÜST daralan kapı
  terası). Kenarlarda cliff otomatik (IsoRoomBuilder zaten yapıyor).
- 10× EchoPedestal prop'u ORTA şeritte yumuşak hilal kavisi (sol-alttan sağ-üste; aralarında 1-2 hücre).
- Sol-alt-orta DUMMY CEBİ boş bırak (props yok, ~4×4 açık alan).
- playerSpawn socket = sol-alt platform; KAPI = sağ-üst (RuinedKeepKit `arch_gate` prop olarak + kapı hücresi
  işaretle); 1-2 brazier + 1-2 pillar + FloorRiftCrack decal serpiştir (az — sahne ferah).
- overlayMask: spawn→hilal→kapı arası ince patika (mevcut overlay altyapısı, floor451_15 placeholder tile).

## P2 — Runtime bootstrap: `ChamberSelectBootstrap.cs` (YENİ, `Assets/Scripts/UI/` veya `Core/`)
CharacterSelect akışında devreye girer (MainMenu→CharSelect): Grid(0.96×0.585)+Ground/Collision/Overlay
tilemap'leri KODLA kur (IsoRoomBuilderTests/_Arena pattern'i) → IsoRoomBuilder.Build(Chamber_CharSelect) →
kamera (mevcut CameraFollow+PixelPerfect 640×360 lock) + global light. Canvas UI: v3.2 panelleri DEFAULT GİZLİ.

## P3 — Oyuncu + heykeller + bürünme
- GERÇEK player prefab'ı son-oynanan sınıf olarak spawn'da (PlayerClassManager.SelectedClass; _Arena'daki
  gerçek-player kurulum pattern'i). Gerçek hareket/anim.
- 10 echo HEYKELİ: world-space SpriteRenderer, idle_south frame-0 DONUK + taş-grisi tint (~0.75,0.78,0.82);
  KİLİTLİ = opak siyah; her biri kendi pedestalının ÜSTÜNDE (FIT ayak-pivot değerleri CharacterSelectScreen'de
  mevcut — reuse); altta world-space isim etiketi (kilitlide + "{cost} SHATTERED ECHO · veya {orPath}").
- Pedestal yakınına gelince (mesafe eşiği): yan paneller (v3.2 build reuse) kayarak görünür + "[E] Echo'ya Bürün"
  prompt. **E (açık):** ≤0.4s yumuşak cyan emilme (FLASH YASAK) → player prefab yeni sınıfla respawn (aynı
  pozisyon) → eski sınıf heykeli kendi pedestalına döner → SelectClass() (class-carry korunur). **E (kilitli +
  bakiye yeter):** EchoWallet.TrySpend → unlock → heykel renklenir.
- **Kapı:** sağ-üst kapı hücresine girince "Rift'e Gir" prompt + onayla mevcut run-start akışı.
- **TAB:** klasik v3.2 tıkla-seç Canvas overlay toggle (orada seçim chamber'a senkron).

## P4 — Dummy (GERÇEK combat)
Dummy cebinde gerçek damageable hedef: mevcut enemy altyapısından AI'sız training-target (HP bar üstünde,
gerçek player saldırısı vurur — HitFlash/damage-number/knockback zaten çalışır; 2sn hasarsız → HP full yenilenir;
ÖLMEZ). Görsel placeholder: mevcut bones_marker veya wall_stub (yeni asset üretme — gerçek dummy sprite sonra).

# Doğrulama (faz faz raporla)
dotnet build PASS + console 0 error + play-observe kanıtları: oda kuruldu (floor/cliff/pedestal sayıları),
player spawn+yürüme, E-bürünme sınıf değişimi + heykel senkronu, kilitli unlock, kapı run-start, TAB overlay,
dummy HP düşüş+regen. CODEX_DONE.md'ye faz başına kanıt. Bir faz BLOCKED ise nedenini yaz, sonraki faza geçme.
