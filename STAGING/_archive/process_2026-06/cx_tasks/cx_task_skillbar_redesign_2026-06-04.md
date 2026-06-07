ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
In-game skill bar'ı canon "color=meaning + no boxes" yönünde cilala. Tümü SkillBarUI.cs içinde cerrahi.

READ FIRST: STAGING/UI_REDESIGN_SCREENS_DECISION_2026-06-04.md (§3 SkillBar).

## Dosya (sadece bu)
- Assets/Scripts/UI/SkillBarUI.cs

## Yapılacaklar
1. **Class-accent glow:** ready-state glow sabit cyan yerine `RimaUITheme.ClassAccent(PlayerClassManager.Instance.PrimaryClass)`. OnPrimaryClassSet'e cache et (her frame Instance sorgusu yok). Boş/empty slot glow = clear.
2. **Key label okunurluk:** font büyüt (primary ~12-13pt, secondary ~10pt), alpha artır, TMP outline/shadow ekle (floor üstünde okunsun), key rect biraz büyüt. Layout rewrite YOK.
3. **Cooldown radial-only** (sayısal saniye EKLEME — canon "sayı minimal"): overlay'i koyulaştır/netleştir (SlotCDOverlay daha opak), kenar/glow'u class-accent ile tint edebilirsin. **Ready-flash:** slot başına `wasOnCooldown` bool tut; cooldown 0'a inince kısa (0.15-0.2s) accent flash.
4. **9-slice backing'i hafiflet/kaldır:** mevcut HUDBacking opak kutu gibi okunuyorsa kaldır ya da alpha'yı çok düşür → hex slotlar floor üstünde **drop-shadow ile yüzsün** (canon "altında/üstünde yatay ayırıcı/kutu YOK"). Her slot'a hafif drop-shadow (offset shadow Image ya da TMP/Image shadow).

## Doğrulama
- Derleme temiz (0 hata). Play-mode'a GİRME (kullanıcı D3D11 restart edecek).
- CODEX_DONE.md'ye: değişen satırlar + 4 madde durumu + compile.
