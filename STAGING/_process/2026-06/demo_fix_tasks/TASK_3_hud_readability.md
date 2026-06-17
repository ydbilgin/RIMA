# TASK 3 — HUD Readability + Low-HP Vignette (1-2h)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: gerekirse `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read: CURRENT_STATUS / PROJECT_RULES / kod / STAGING / memory.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz.

## Bağlam
Karar: `STAGING/CHATGPT_REV2_COUNCIL_DECISION_2026-06-17.md` (HUD = P0, ucuz). Spec: `STAGING/_process/2026-06/chatgpt_review_rev2/.../06_GAME_UI_REDESIGN.md` (HUD bölümü) + görsel `visuals/combat_hud_proposed_markup.png`. Council kök-neden: RESP_cx.md Q3 — HUD procedural, ölçüler `HUDController.cs:54-56` sabitleri.

**Sorun:** 1080p'de HP/resource barları + skill slotları çok küçük, koltuktan okunmuyor. Low-HP full-screen kırmızı wash oynanışı/rengi öldürüyor.

## Fix
### 3A — Bar/slot resize (sabitler)
- HP bar: **200-220 × 14-16px** (mevcut 72×4 override; `HUDController.cs:54-56,586-649`).
- Resource bar: **150-170 × 8-10px**.
- LMB/RMB slot: **52-56px**; Q/E/R/F slot: **44-48px** (slot tarafı `SkillBarUI`'ye dokunabilir).
- Key label 12-14px, cooldown number 16-18px okunur.

### 3B — Low-HP overlay
- Full-screen kırmızı wash KALDIR → sadece **kenar vignette %12-18 opacity**.
- HP <%20'de 0.8-1.0sn pulse. **Merkez + düşman-telegraph TEMİZ kalmalı.**

## ⚠️ KRİTİK — SDF/pixel-perfect netlik (ax Pro uyarısı)
Font/bar büyürken **TMP SDF veya pixel-perfect netliği bozulup BULANIKLAŞMASIN.** Bulanık font tüm profesyonel algıyı öldürür. Resize sonrası net kaldığını runtime'da göz/screenshot ile doğrula. Gerekirse SDF material/PPU ayarını koru.

## Kısıt
- Cerrahi: `HUDController.cs` + (slot için) `SkillBarUI`. İlgisiz HUD mantığına DOKUNMA.
- Mevcut HUD state mantığı (cooldown sweep, perfect-condition pulse, ulti-lock) DEĞİŞMEsin — sadece ölçü + vignette.
- git'e DOKUNMA.

## VERIFY (runtime)
- Play'e gir, combat sahnesi. Screenshot/göz: HP/resource/slot okunur + SDF net (bulanık değil) + low-HP kenar-vignette (merkez temiz).
- `read_console` 0-error.

## ÇIKTI (E1: ≤10 satır)
Evidence → `STAGING/_process/2026-06/demo_fix_tasks/DONE_3_hud.md` (+screenshot). Dönüşte: değişen dosyalar + 3A/3B durumu + SDF-netlik doğrulaması + console + risk.
