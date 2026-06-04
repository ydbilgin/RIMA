# UI/UX REDESIGN — COUNCIL KARARI (2026-06-04)

**Yöntem:** /council = cx `laurethayday` (codebase feasibility) + ax 3.1 Pro (deep UX/art) + ax 3.5 Flash (lean) → Opus sentez.
Advisor çıktıları: `CODEX_DONE_laurethayday.md`, `_council_a_31pro_ux_camera_cards.md`, `_council_a_35flash_ux_camera_cards.md`.
⚠️ NLM auth expired (cx canon çekemedi) — palet bilinen (#00FFCC cyan / #3A1A4A void / #E89020 orange), üretim devam edebilir; sonra `nlm login`.

## A) KAMERA SCROLL-ZOOM — "integer crisp endpoint easing" (pop yok)
- **Kök kusur:** mevcut kod fractional `_zoom`'a ease edip PPC'ye bırakınca PPC tamsayı-orana snap'liyor → settle-pop.
- **Karar:** scroll hedefi crisp PPC seviyesine kilitlensin. orthoSize'ı **tam crisp endpoint** boyutuna ease et (PPC kapalı), endpoint'e varınca refResolution'ı ata + PPC'yi aç → seamless, pop YOK.
- **Değerler (cx kod-semantiği, `_zoom`=refRes çarpanı; büyük=uzak):** default **~1.0**, min(yakın) **0.7**, max(uzak) **1.6** (serialized field, constant değil). Mevcut 2.0 default çok uzak; 3.0 max çok geniş. R = default'a dön. LerpSpeed ~12-15.

## B) DRAFT KART — jitter + "Seç" FIX (tek yapısal değişiklik ikisini de çözer)
- **Kök neden:** HoverScale tüm kart ROOT'una uygulanıyor → raycast-target kursör altından kayıyor → enter/exit flicker (jitter) VE tıklama iptal ("Seç" çalışmıyor). Ayrıca bg+Button'da ÇİFT handler.
- **Karar (yapı):** `cardGo` = sabit full-size raycast HITBOX (CardW×CardH, transparent Image raycastTarget=true, TEK `CardJuiceHandler`, Button burada). Tüm görseller child `VisualRoot`'a taşınır. **Hover SADECE `VisualRoot`'u ölçekler.** bg/diğer görsel Image'lar raycastTarget=false. Per-card Canvas+overrideSorting churn KALDIR; bring-to-front = `SetAsLastSibling()` (hover end'de restore).
- **Premium hover:** VisualRoot scale 1.05 + Y +20px yukarı; hover edilmeyen kartlar CanvasGroup alpha 0.4 + scale 0.95. Confirm: seçilen beyaz flash + merkeze, diğerleri fade (basit 2D tween — 3D tilt/particle DEFER, over-engineering).
- **Seç robustluk:** `DraftManager.ShowDraftDelayed` `WaitForSeconds`→`WaitForSecondsRealtime` (DraftManager.cs:99-101).

## C) BOYUTLAR (1920×1080 ref, okunaklı)
- **Draft kart:** 280×400, gap 36, icon 100, name 22, desc 14 (line-height bol), tier chip 80×22 @10, button 160×40 @15. Title 34, subtitle 15. Container **tam ortada** (anchor 0.5,0.5, pos 0,0).
- **Skill bar:** primary (LMB/RMB) 56, secondary (Q/E/R/F) 44, gap 8, icon ~%72, key label 11/10. Alt-orta.

## D) REWARD TRIGGER — press-G
- `RewardPickup` `OnTriggerEnter2D` otomatik-collect → "playerInRange=true + ShowPrompt"e çevir; collect body'sini `Collect()`'e taşı, `Update`'te `Keyboard.current[Key.G].wasPressedThisFrame` ile çağır; `OnTriggerExit2D` prompt temizle. `collected` guard + `DraftThenOpenExit()` korunur. Pattern = `DoorTrigger.cs:38-39,70,149,158,184`.

## E) ASSET ÜRETİM (cx `laurethayday` $imagegen, micro-pack)
- **ÖNCE REUSE:** `Assets/Resources/UI/RIMA/...`'da `card_frame_stone`, `icon_frame_hex`, `hex_slot_mask`, Pack 9-slice VAR — yeterliyse generate ETME.
- **ÜRET (en yüksek değer):** **skill ikon pack** 8-12 adet, 96×96 source (draft'ta 88-100px, HUD'da 28-34px), tek coherent sheet/grid (palet uyumu), gameplay-okunaklı (strike/dash/slam/cleave/guard/echo/whirl/leap...). Cyan rift + void-mor + warm-orange.
- **GEREKİRSE:** card frame 280×400 (obsidian slab + cyan seal çatlağı + warm-orange aşınmış kenar) — mevcut `card_frame_stone` yetersizse. Opsiyonel HUD bar backing 360×72.
- **ÜRETME:** chip/buton/düz glow/cooldown-pie/key-label = uGUI (renk/9-slice/additive tint).
- **Import:** `Assets/Resources/UI/RIMA/SkillIcons/`, `/Draft/`, `/HUD/`. Wire: `SkillOfferUI.cs` bg:415 glow:405 icon:444 button:493; `SkillBarUI.cs` bg:141 icon:147 glow:163. Path'leri `RimaUITheme`'e ekle (RimaUITheme.cs:12 pattern).

## UYGULAMA SIRASI (cx order)
1. SkillOfferUI hitbox/visuals fix (jitter+Seç) + sizes + hover. 2. ShowDraftDelayed realtime. 3. RewardPickup press-G. 4. SkillBarUI sizes. 5. CameraZoom integer-endpoint. → her biri play-verify. 6. Asset micro-pack üret + import + wire + play-verify. Her aşama checkpoint commit.
