# TASK: ChatGPT HUD Pack — Opus Review (2026-06-11)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Görev
ChatGPT'nin ürettiği RIMA HUD Implementation Pack'i RIMA projesine uygunluk açısından değerlendir. Karar odaklı, madde madde rapor yaz.

## RIMA Proje Kısıtları
- Pixel art top-down 3/4 roguelite (Hades/Dead Cells referans)
- Renk: void mor `#3A1A4A` panel, ember `#E89020` vurgu, slate `#3A3D42` zemin
- Mevcut UI: SkillBarUI, PauseMenuUI, SkillCodexUI, RimaUITheme — Unity Canvas (ScreenSpaceOverlay)
- PPU: 64 (proje standardı)
- URP 2D Renderer, TextMeshPro
- Demo build kısıtı: en az iş, en fazla görsel etki
- Sadece Warblade + Elementalist demo'da açık (8 class kilitli)

## ChatGPT Çıktısı — İncelenecek İçerik

### style_tokens.json (renk paleti)
```json
{
  "colors": {
    "background_ink": "#05070B",
    "panel_black": "#090B0F",
    "cyan_rift": "#00FFCC",
    "amber_primary": "#FF9A1F",
    "hp_red": "#C82026",
    "resource_blue": "#1B7BA8"
  },
  "pixel_settings": {
    "filter_mode": "Point",
    "canvas_scaler": "Scale With Screen Size, Reference 1920x1080, Match 0.5"
  }
}
```

### Asset Checklist (özet)
Gerekenler:
- UI Core Atlas: panel_9slice_small (96x96), panel_9slice_large (160x160), corner seti (32x32), divider, ornament
- Gameplay HUD: portrait_frame (96x96), hp_bar_frame (280x24), resource_bar_frame (280x20), hotbar_slot seti (72x72), keycap (48x24)
- Minimap: frame (280x220), player/room/enemy marker
- Reward card: card_frame_9slice (180x180), rarity ribbon (112x28), select_button (180x52)
- Pause: panel (340x58 butonlar), arrows, gem
- Skill Codex: window (220x220), class_tab (220x52), skill_row (1200x62), icon_frame (52x52), scrollbar

### Unity Implementation Notları
- Canvas hierarchy: Canvas_Root > SafeArea > HUD_Persistent + ModalLayer + TooltipLayer
- 9-slice zorunlu — panel/buton scale edilmemeli
- HotbarSlotData model (InputSlot enum: LMB/RMB/Q/E/R/F)
- RewardOption model (skill + rarity + title + desc + icon)
- ScrollRect + pooled row prefab (SkillCodex için)
- Debug kırmızı kareler runtime'da kapatılmalı

### Asset Manifest (P0 öncelikli)
panel_9slice, corner, portrait_frame, hp_bar_frame, hotbar_slot_normal/active, keycap, reward_card_frame, reward_rarity_ribbon, pause_panel/button, codex_window/tab/row

## Değerlendir

### 1. Renk/stil uyumu
ChatGPT'nin paleti (`#00FFCC` cyan, `#FF9A1F` amber) RIMA'nın `#3A1A4A`/`#E89020` paletine uyuyor mu? Çelişme var mı?

### 2. Asset kapsamı — gerçekçi mi?
Asset listesi demo için aşırı mı, yoksa yerinde mi? Hangi assetler demo için zorunlu (P0), hangileri ertelenebilir?

### 3. Teknik uyum
Canvas hierarchy önerisi mevcut SkillBarUI/PauseMenuUI/SkillCodexUI ile çelişiyor mu? HotbarSlotData modeli mevcut SkillBarUI implementasyonuyla uyumlu mu? 9-slice yaklaşımı mevcut RimaUITheme ile uyumlu mu?

### 4. En değerli 3 öneri — hangilerini önceliklendirelim?
Demo için en fazla görsel etki yaratacak 3 öneriyi seç. Neden?

### 5. Red flag / dikkat
Proje kısıtlarıyla çelişen, fazla iş gerektiren veya yanlış yönde giden öneri var mı?

## Çıktı Formatı
Türkçe. PASS/PARTIAL/REJECT genel değerlendirme + 5 madde üzerinden bulgular. Toplam ~300-400 kelime. Karar odaklı.
