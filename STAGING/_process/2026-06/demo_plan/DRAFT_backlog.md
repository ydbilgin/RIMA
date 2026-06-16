# DRAFT — Demo-First Ordered Backlog (MERGE)
> Demo = 19 June 2026 (3 gün). Merge: ChatGPT paket P0/P1 (verified-real only) + 4 yeni feature design (G1 maps / G2 doors / G3 run-map / G4 director) + Katman-1/2 backlog + REWARD-02/run-map ALREADY-DONE.
> Method: paket root-cause'lari GERCEK koda dogrulandi (D1-D5 + spot-verify). Paket ornek-sinif adlari KULLANILMADI.
> Sira: (1) golden-path-first → (2) dependency → (3) risk. Effort S/M/L. Unity? = Unity Editor/MCP dokunusu gerekir mi.

## MVP CUT-LINE (PLAYABLE demo icin MUST-HAVE)
Golden-path video akisi: MainMenu → Chamber/CharSelect → _Arena → combat → ODA TEMIZLENDI → reward (G) → kart draft → portal/run-map → sonraki oda → boss. Bunu BOZAN her sey MVP. Asagida T1-T9 = MVP; T10+ = STRETCH.

---
## ORDERED BACKLOG

| id | title | why (golden-path) | effort | risk | Unity? | deps | gp-impact | verification + SCREENSHOT capture point |
|----|-------|------|--------|------|--------|------|-----------|------------------------------------------|
| **DONE-1** | REWARD-02 G-collect | reward al = akisin kalbi | — | — | — | — | CRITICAL | ALREADY-DONE (RewardPickup OnTriggerStay2D + Awake CheckInitialPlayerOverlap; dual-verified). Regresyon: canli G repro OBS provasinda. |
| **DONE-2** | Run-map branching (DungeonGraph mid-mix + per-run seed) | M = portal sayisi/cesitlilik | — | — | — | — | HIGH | ALREADY-DONE (RoomRunDirector forceDemoSequence=false @_Arena, 30/30 test). KALAN→T6 (M-overlay gorsel + dal tik). |
| **DONE-3** | LIFE-01 BuildPlacementController quit-guard | console temiz (kismi) | — | — | — | — | LOW | ALREADY-DONE (kismi); ATM vb. → T8. |
| **T1** | Director Mode bleed gate (reward draft acikken overlay tam gizle) | **EN YUKSEK demo deger**: overlay draft uzerine biniyor = video kirici | M | MED | y | UIManager.IsAnyOverlayOpen (VAR l.41) | CRITICAL | DirectorMode.Update poll → IsAnyOverlayOpen ise SetOverlayVisible(false)+freecam/spawn skip; kapaninca geri ac (overlaySuppressedByModal flag, dual-class l.2066-73 DOKUNMA). Belt: sortingOrder 950→~120 (cx ONCE modal canvas'lari OLCSUN). **SS: draft acikken before/after — Director GORUNMEZ.** |
| **T2** | Golden-path full LIVE verify (input+screenshot artik calisiyor) | tum MVP zinciri tek otururumda kanit | M | LOW | y | T1 | CRITICAL | dev-direct _Arena: F2/`/G/ESC/combat gercek input enjekte. **SS: her beat (combat→cleared→G→draft→portal→next room).** Metodoloji: manage_camera screenshot overlay yakaliyor (DUZELTILDI). |
| **T3** | Camera zoom-out (oda "kucuk" hissinin #1 koku) | haritalar buyuk hissetsin = scope (a) | S | MED | y | — | HIGH | RoomRunDirector useFixedDemoCamera=true + fixedOrthographicSize=5.0 (@l.110-111) → 36x28 boss bile ~10x7 tile. Fix: size buyut VEYA FitCameraToRoom toggle. **SS: ayni oda once/sonra — tile sayisi artisi.** ApplyFixedDemoCamera arrive'da self-heal. |
| **T4** | Chest bank wire (DemoRoomBank chest listesi BOS) | run-map Chest node'u bos oda spawnluyor | S | LOW | y | DONE-2 | MED | Assetler VAR, sadece referanslanmamis. DemoRoomBank.asset chest slot'una RoomTemplateSO ata. **SS: Chest node'a gir → dolu oda.** Smoke Test All Templates. |
| **T5** | Portal teleport coroutine (walk-in → blue beam → cam zoom → load) | scope (b); su an AdvanceTo INSTANT, gecis hissi yok | M | MED | y | T3 (cam restore) | HIGH | TryEnterDoor (@l.1820, su an instant AdvanceTo @l.1827) icine 1 coroutine: collapse 0.15s+input lock+dissolve puff → blue beam 0.30s → cam zoom 0.25s → RoomTransitionFX.DoTransition(onBlack=AdvanceTo) → fade-in+materialize. ~1.0s. VFX reuse: EchoPuffBurst + telegraph_line_beam.png + SkillVfx. Fallback null FX→direct AdvanceTo (asla softlock). try/finally input release. **SS: portal gecis mid-beam (beam+zoom).** |
| **T6** | Run-map portal bar (ekran-alti, N node=N portal) + dal-nav canli tik | scope (c); DONE-2 verisini gorsele baglar | M | MED→LOW | y | DONE-2, T5 | HIGH | YENI hafif ekran-alti bar, CurrentChoices (=DungeonGraph.ChildrenOf) ile beslenir. RunMapOverlay (M, IMGUI) KORUNUR; UI/Map/MapPanelUI tablet-preview DIRILTME (enum-kopru blokeri). SADECE DoorOpen state'inde goster. Walk-into (A) birincil + bar salt-onizleme; click-to-enter (B) bonus → ikisi de TryEnterDoor→T5 beam. **SS: 3-portal oda → bar 3 node; M-overlay; dal tik.** |
| **T7** | DATA-01 reward chip metni (trigger+outcome) | draft kart netligi; "Eslesir" belirsiz | S | LOW | n* | — | MED | SkillOfferUI.BuildChainChip @374 `Loc.T("draft.pairs_with",...)`. Loc string'i trigger+outcome'a genislet (ornek: Iron Charge→Gravity Cleave: stun+Rage). Kod degil, Loc edit. *Unity yalniz string-reload. **SS: kart chip yeni metin.** |
| **T8** | LIFE-01 batch quit-guard (AttackTokenManager + diger lazy-singleton) | console scene-close warning (gameplay-kirmaz ama temiz demo) | M | LOW | y | — | LOW | graphify ile tara (graph.json: lazy-singleton anti-deseni TUM manager'larda). Her birine _isQuitting guard + InstanceIfExists (BuildPlacementController presedani). **SS/proof: read_console scene-close → 0 resurrect warning.** |
| **T9** | SkillBar slot+font buyut (cok kucuk) | HUD okunabilirligi (e); 1080p sinirli/1440p mikro | S | LOW | y | — | MED | SkillBarUI PrimarySize=56/SecondarySize=44 (l.24-25), font 13/10/8 → ~82/64, font 16-22. Sabit deger, tek dosya. **SS: HUD skill bar 1080p okunabilir.** |
| **T10** | HUD canvas scaler garanti + HP/resource bar buyut | HUD 72x4 raw px (Ashen Glyph micro-spec) 1440p'de mikroskobik | M | MED | y | — | MED | HUDController scene HUD_Canvas: runtime SADECE renderMode set (l.84-92), CanvasScaler EKLEMIYOR. 1920x1080 scaler garanti + bar ~3-4x. NOT: micro-spec'i terk = tasarim karari (kullanici onayi). **SS: HUD 1080p+1440p.** |
| **T11** | Reward kart buyut (280x400→~360x540) | draft okunabilirligi | M | LOW | y | T7 | LOW | SkillOfferUI CardWidth=280/CardHeight=400 (l.41-42); ChainChip sabit sizeDelta (LayoutGroup YOK — UI-01 paket root-cause YANLIS, layout-conflict degil, overflow). Surgical chip genislik/font. **SS: kart + chip overflow yok.** |
| **T12** | Director frame recolor (P1 code-only) | (d) "cok sacma" = TopBadge opak ember slab (l.775) | M | LOW | y | T1 | LOW | TopBadge 0.48,0.18,0.06,0.92 → ribbon/micro-banner. Canon palette slate#3A3D42/void#3A1A4A derinlik/ember#E89020 radial-only/cyan≤15%. **SS: Director once/sonra.** |
| **T13** | Egg = RewardPickup salt-gorsel re-skin (sadece bos vakit) | scope (f) sunum; ekonomi DEGIL | S | LOW | y | — | LOW | RewardDefinition/WorldRewardChoiceSet YOK (paket ornek). Inspector sprite ata (basalt-egg) + idle pulse. Mekanik AYNI: G→ShowDraft. Hatch=RewardSpawnPop variant. **SS: egg sprite + G→draft.** |

---
## STRETCH (MVP-disi; sadece zaman kalirsa, dusuk-risk)
T10 HUD scaler, T11 reward kart, T12 Director recolor, T13 egg + Katman-2: U4 'ODA TEMIZLENDI' ortala, J1 reward slow-mo juice, U1 tooltip width, U2 Codex scroll, U3 resource bars, F1 reward room-leak. HEAVY (POST-DEMO): Director 9-slice swap (D3 asset baglı), Pause/Settings/Codex layout overhaul, HP micro-spec terk tam-tasarim, single AimSnapshot/CastContext refactor (REJECT — scope disi modularizasyon).

## ASSET-GAPS (gercekten eksik → kim uretir)
1. **Blue-beam / portal teleport VFX** (T5) — Portal.cs placeholder; beam/zoom/dissolve YOK. Reuse: EchoPuffBurst + telegraph_line_beam.png + SkillVfx + Cinemachine zoom; dedicated tall cyan beam sprite = OPSIYONEL post-demo PixelLab. **P1.**
2. **Egg presentation** (T13) — kodda Egg yok, sprite yok. PixelLab 96x96 shell+idle(6f) transparent. Sheet/hatch/reject ATLA. **P3.**
3. **Run-map node ikonlari** (T6) — Chest node VAR; Combat/Elite/Boss/Merchant eksik. Renk+TR-etiket placeholder demo-yeter (NON-blocker).
4. **Cliff-rim landmark prop** (T3/maps) — yoksa CombatBiome reuse.
5. **Settings input chrome / Director 9-slice** — POST-DEMO (mevcut Resources/UI/RIMA/Pack/*_9slice'tan turetilebilir). **P2.**
> RIMA'DA ZATEN VAR (redo etme): 9-slice pack, ResourceFrame/SkillSlotFrame/MiniMapFrame, minimap node set, reward card_frame+rarity_glow, Chrome set, ~85 skill icon, RimaUITheme renkleri. Buyuk oda sablonlari (24x18→36x28) ZATEN VAR.

## CANON-REJECTIONS (regress ETME)
- **4-cardinal S/E/N/W sprite** (paket STYLE_BIBLE/MASTER #53) → **REJECT** (Karar #114: 8-yon LOCKED, 5 sprite+3 mirror flipX; #114 > #53 tarihce; kullanici 2026-06-16 acikca reddetti).
- **"no flip / flip edilmemeli"** → **REJECT** (flipX W←E/SW←SE/NW←NE = canon pipeline).
- **PPU=128** ("PPU=64 yasak") → **REJECT** (RIMA canli PPU=64, S59).
- **Kamera 35° iso / iso-grid cellSize 1x0.5** → **ADAPT degil REJECT-math** (high top-down 3/4 + cliff-tile 32x32; kompozisyon prensipleri ADAPT, iso matematigi REJECT).
- **Minimap sag-ust** (paket HUD_SPEC) → **REJECT yerlesim** (kullanici karari = ekran-alti, T6); minimap-stil ipuclari ADAPT.
- **Egg 3-choice WorldRewardChoiceSet / RewardDefinition SO / 7-state machine** → **DEFER sistem** (paralel reward ekonomisi = kullanici kisitini ihlal); sadece T13 re-skin ALLOW.
- **Tek AimSnapshot/CastContext refactor** → **REJECT** (demo scope disi modularizasyon; surgical kalsin).
