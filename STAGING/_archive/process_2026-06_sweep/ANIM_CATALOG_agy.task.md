# RIMA — Animation Catalog Research (agy / Antigravity)

## Amaç
RIMA Warblade demo Faz 1 + 9 class Faz 4 master için PixelLab Web UI Add Animation flow'una uygun, indie 2D action benchmark + V3 best practice rapor. Sentez orchestrator'a inline döner (DOSYA YAZMA).

## Bağlam (kullanıcı verbatim)
"Karakterlere animasyonlar yapılacak ya hangi animasyon için hangi stateler kullanılabilir end ve first frame gerekir mi prompt ne olur gibi. Bu karaktere senin prompt vereceğin benim animasyonu yapacağım ekran. Start ve end frame i karakterimize state üretimiyle ayarlayabiliyoruz ve smoothluk için kaç frame olmalı onu da düşün."

PixelLab "Add Animation" ekranı 2026-05-27:
- 9 hazır anim (Idle 2 opt, Jumping 16 opt, Running 3 opt, Walking 19 opt, Backflip 1, Crouching 1, Front Flip 1, Getting Up 1, Slide 1)
- Combat (Kicking 1, Punching 6, Reactions 4, Fireball 1)
- Interactions (Drinking, Picking Up, Pull/Push/Throw)
- Custom: Animation V3 (Action Description + Frame Count 4-16 slider + Keep first frame check)
- Advanced: Custom Frames (Start Frame + End Frame picker)
- Karakter: warblade 120x120, 8 dir, mannequin template
- Cost: 2 generations per direction

## Sorular (lütfen yanıtla, kısa, kanıt linkleri ile)

### S1. Indie 2D action frame budget benchmark
Şu oyunlarda Idle / Walk / Run / Basic Attack / Hit / Death / Skill animasyonları kaç frame:
- Hades (Supergiant)
- Children of Morta (DeadMage)
- Hyper Light Drifter (Heart Machine)
- Diablo III sprite/2D modu
- Eastward (Pixpil)
- Sea of Stars (Sabotage)
- Eldest Souls / Tunic
- (varsa) modern PixelLab demo

Tablo: oyun, anim type, frame count, FPS, loop/one-shot, notes.

### S2. PixelLab Animation V3 best practice
- "Keep first frame" checkbox NE ZAMAN işaretlenir?
  - Loop animasyonlar (idle/walk) → genelde işaretli (cycle başlangıç = bitiş)
  - One-shot (attack/death) → işaretsiz? (sonra başka clip'e geçilecek)
- "Action Description" prompt grammar (kısa imperative? compound?)
- "Frame Count 4-16" slider için sweet spot her anim type için
- "Custom Frames" Start + End frame nasıl character_state ile entegre olur
- 8-direction generation cost (2 gen × 8 dir = 16 gen per anim) — doğru mu?
- Mannequin template animation transfer (universal pose set) avantajı

### S3. Start + End frame "character_state" pattern
- PixelLab character_state generation:
  - Tek state üretimi = N gen credit (number)
  - First + End frame state'inden çıkarılır mı (single state output)?
  - State pose + transparent BG, 8-dir aynı anda mı yoksa per-view?
- n_frames numbered list alternatifi character_state'e göre 4-8x ucuz (RIMA memory `feedback_state_vs_n_frames_cost_lock`) — onay?
- reference_image_base64 + n_frames birlikte kullanılabilir mi animation v3 için

### S4. Frame count → smoothness perception
- 10-12 FPS RIMA base (PROJECT_RULES.md)
- Idle 4 vs 6 frame fark hissedilir mi @ 10fps?
- Walk 6 vs 8 vs 10 frame Cinderella/Pixar/Disney "12 principles" sweet spot?
- Attack one-shot 8 vs 10 vs 12 vs 14 frame → impact framing literature?
- Death 6 vs 10 frame → drama vs cost trade-off?
- Frame skip pattern (12fps anim @ 8 frame = 0.66s) snappier perception?

### S5. Loop vs one-shot semantics
- Idle/Walk/Run = loop (Keep first frame = start matches end)
- Attack/Skill/Death/Hit = one-shot (transition state machine'de)
- Hangi anim'ler "loop iki yöne" (idle bob fwd-back) vs "loop tek yön"?
- Animator state machine için one-shot trigger + exit time best practice?

### S6. 8-direction sprite mirror pattern
- 5+3 mirror (S/SE/E/NE/N + flipX → W/SW/NW) RIMA'da LIVE
- PixelLab "2 generations per direction" — sadece 5 dir mi gen, 3 mirror Unity'de mi?
- Top-down 3/4 angle (RIMA HIGH TOP-DOWN ~70-80°) için flipX bozulma riski (silah el seçimi vb)?

### S7. Combat anim impact framing (en kritik)
- "Anticipation → Strike → Hold → Follow-through → Recovery" 5-phase rule
- Hold frame (impact pause 2-3 frame) hissi nasıl artırır?
- "Frame 0 = windup pose, Frame N-1 = follow-through" yeterli mi yoksa hold frame eklemeli (start + hold + end pattern)?
- Hades smash combo Demo'da bilinen frame breakdown var mı?

## Çıktı formatı
8-12 paragraf, bullet'lı. Tablo veya bullet, kısa. Frame count önerileri RIMA bağlamında:
- Idle: __ frame, Keep first __
- Walk: __ frame, Keep first __
- Run: __ frame
- Basic Attack: __ frame (windup __, hold __, follow __)
- Hit: __ frame
- Death: __ frame
- Skill (Iron Charge tipi dash): __ frame
- Skill (Earthsplitter tipi slam): __ frame
- Skill (Death Blow tipi finisher): __ frame

## YASAK
- Asset gen, kod yazımı, dosya yazımı YOK
- Sadece inline rapor, orchestrator'a return
