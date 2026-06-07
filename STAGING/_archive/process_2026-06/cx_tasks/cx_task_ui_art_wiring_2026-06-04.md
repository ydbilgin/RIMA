ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Draft kartlarını ve skill bar'ı MEVCUT premium Pack asset'leriyle güzelleştir (üretim YOK — assetler zaten var, sadece wire). Karar = `STAGING/UI_UX_REDESIGN_DECISION_2026-06-04.md`.

# Dosyalar (SADECE bunlar)
- Assets/Scripts/UI/SkillOfferUI.cs   (Task-1 sonrası YENİDEN OKU — kart kökü sabit hitbox, görseller `VisualRoot` altında, kart 280x400)
- Assets/Scripts/UI/SkillBarUI.cs
- Assets/Scripts/UI/RimaUITheme.cs     (sprite path'leri buraya ekle, RimaUITheme.cs:12 pattern)

# MEVCUT ASSET'LER (Resources.Load yolu = "UI/RIMA/..." uzantısız):
- Kart frame: `UI/RIMA/Pack/card_frame_9slice` (obsidian + cyan rün, 9-slice — PREMIUM, kullan)
- Rarity glow: `UI/RIMA/rarity_glow_common` / `_rare` / `_epic` / `_legendary`
- Select flash: `UI/RIMA/card_select_flash`
- Skill slot frame: `UI/RIMA/RIMA_UI_SkillSlotFrame` (yoksa `UI/RIMA/icon_frame_hex`)
- HUD backing: `UI/RIMA/Pack/bar_frame_9slice` (9-slice) veya `UI/RIMA/Pack/pedestal_seal`
- Aktif skill ikonları: `Assets/Sprites/UI/Icons/Icon_*.png` (offer'ın icon alanı zaten varsa onu kullan; DEĞİŞTİRME)

# Değişiklikler

## 1) SkillOfferUI — kart görselleri (VisualRoot içine)
- Kart bg/frame: VisualRoot'un en altına bir Image = `card_frame_9slice` yükle, `type=Sliced` (border meta yoksa orchestrator import'ta border verecek — yine de Sliced ata), full-stretch. Mevcut düz placeholder bg'yi bununla DEĞİŞTİR (raycastTarget=false). Kartın okunabilirliği için frame içine hafif koyu yarı-saydam zemin koyabilirsin.
- Rarity glow: frame'in ARKASINA (VisualRoot'tan önce ya da en alt) bir Image = offer rarity'sine göre `rarity_glow_{common|rare|epic|legendary}` (Simple, additive görünüm için renk beyaz; raycastTarget=false). Rarity bilgisi offer'da yoksa common kullan.
- Icon: mevcut icon-atama yolunu KORU (offer.icon). Sadece icon Image'ı frame'in ikon alanına otursun (boyut decision'daki 100px).
- Path'leri hard-code Resources.Load yerine RimaUITheme'e ekle (DraftCardFramePath, RarityGlowPath(rarity) gibi).

## 2) SkillBarUI — boyut + frame + HUD backing
- Slot boyut: primary (LMB/RMB) 56, secondary (Q/E/R/F) 44, gap 8 (SkillBarUI.cs:22-24). Alt-orta yerleşim korunur.
- Her slot'a frame Image = `RIMA_UI_SkillSlotFrame` (icon'un üstünde/altında uygun katman). icon ~%72 slot.
- Bar'ın ARKASINA tek HUD backing Image = `Pack/bar_frame_9slice` (Sliced) veya `pedestal_seal` (Simple) — slot kümesini saran, hafif. Bottom-center.
- Path'leri RimaUITheme'e ekle.

# Notlar
- 9-slice Image'larda `type=Sliced` ata; sprite'ın border'ı import'ta set edilmemişse orchestrator düzeltecek (sen kodu Sliced yaz).
- Mevcut wiring/callback'leri (onPick, OnOfferSelected, Warblade_SkillController.GetSlot) BOZMA.
- Compile-mantıklı; Unity doğrulamayı orchestrator yapar.
- Sonuç → CODEX_DONE.md: değişen dosyalar + hangi asset nereye + riskli noktalar.
