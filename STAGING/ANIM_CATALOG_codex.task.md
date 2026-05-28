# RIMA — Animation Catalog Asset Inventory + Cost (Codex)

## Amaç
RIMA Warblade asset envanteri + animation clip mevcut durum + PixelLab cost analizi + Unity Animator setup tahmini. Sentez orchestrator'a inline (DOSYA YAZMA).

## Bağlam
Kullanıcı PixelLab Web UI "Add Animation" ekranına anim ekleyecek. Claude (rima-design Opus) ANIMATION_PROMPT_CATALOG.md hazırlıyor. Codex'in görevi: envanter + cost realist confirm.

## Sorular

### S1. Warblade asset envanteri
Şu dosyalar mevcut mu, boyut + state nedir:
- `Assets/Art/Characters/Warblade/Rotations/warblade_south.png` (LIVE biliniyor)
- `warblade_north.png`, `warblade_east.png`, `warblade_west.png` mevcut mu (canvas 120x120 confirm)
- `warblade_south-east.png`, `warblade_north-east.png`, `warblade_south-west.png`, `warblade_north-west.png`
- `Assets/Art/Characters/Warblade/south.png` (root) ile rotations farkı

### S2. Mevcut Animator
- `Assets/Animations/Characters/Warblade/Warblade.controller` state listesi (idle south/east/north/west + diagonals var)
- Hangi state'ler boş (placeholder), hangi state'ler bağlı clip
- Walk/Attack/Hit/Death state mevcut mu
- Mob Animator (`ChainWarden.controller`, `Penitent.controller` vb) state pattern → Warblade için referans

### S3. Asset import settings verify
- warblade_south.png import:
  - Pixel Per Unit = 64
  - Filter = Point (no AA)
  - Compression = None
  - Sprite Mode = Single (Multiple ise sprite sheet)
  - Pivot = Bottom (RIMA standard)

### S4. PixelLab Animation V3 cost (web UI deklared)
- Generation cost per direction = 2
- 8-direction full = 16 gen per anim
- 5-direction (RIMA pattern, 3 mirror Unity) = 10 gen per anim
- South-only first pass (Tier 1 Demo Faz 1) = 2 gen per anim
- Mevcut PixelLab account credit balance check (eğer accessible — `mcp__pixellab__get_balance`)

### S5. character_state vs n_frames maliyet karşılaştırma
- `create_character_state` per state = ? credit
- `animate_with_text` v3 + n_frames + reference_image = ? credit
- RIMA `feedback_state_vs_n_frames_cost_lock` "state 4-8x pahalı" iddiası confirm/deny

### S6. Unity Animator state machine setup (kod ÇIKARMADAN tahmin)
Warblade'e şu state'ler eklenirse:
- Idle (loop, default)
- Walk (loop, Speed > 0.1 transition)
- Run (loop, Speed > 5 transition)
- BasicAttack1 (trigger, exit time = 1)
- BasicAttack2 (trigger, BasicAttack1 transition)
- Hit (trigger, any state)
- Death (trigger, any state, no exit)
- IronCharge (trigger, skill input)
- Earthsplitter, GravityCleave, SunderMark, DeathBlow, IronCounter (trigger, skill input)

Toplam state count, transition count, parameter count tahmini.

### S7. Memory cross-check
Şu memory files'in mevcut Warblade animation context'i:
- `MEMORY/warblade_animation_states_demo_phase1_plan.md`
- `MEMORY/nine_class_animation_states_demo_phase1_plan.md`

Bunlardaki Tier 1 state list + Faz 4 plan ile bu catalog uyumlu mu?

### S8. Iso anim risk
- HIGH TOP-DOWN 3/4 ~70-80° angle (PROJECT_RULES.md)
- 8-dir flipX bozulma riski (özellikle silah sağ el → sol el dönüşü)
- Dominant hand consistency (Warblade greatsword sağ el)

## Çıktı formatı
- Asset envanter tablosu (dosya | mevcut | size | notes)
- PixelLab cost tablosu (anim type | south only | 5-dir | 8-dir)
- character_state vs n_frames cost karşılaştırma
- Animator setup state/transition tahmini
- 3-5 risk/blocker bullet

## YASAK
- Kod yazma YOK
- Dosya değişikliği YOK
- Scene mutation YOK
- PixelLab MCP create_* / animate_* call YASAK (gece halt rule)
- Sadece inline analiz return
