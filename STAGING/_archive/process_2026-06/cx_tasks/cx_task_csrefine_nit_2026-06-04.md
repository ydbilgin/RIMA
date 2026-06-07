ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
ax verification'ın bulduğu TEK nit'i düzelt: IdentityZone'da alt kimlik etiketleri (motto/playstyle/resource/lock) `anchorMin.x=0.08` ile YENİ eklenen sol portre frame'iyle ÇAKIŞIYOR. Etiketleri portrenin SAĞINA hizala. KOD-ONLY, sadece CharacterSelectScreen.cs.

# Dosya
Assets/Scripts/UI/CharacterSelectScreen.cs

# Fix (surgical)
IdentityZone içinde motto / playstyle / resource / lock label RectTransform'larının `anchorMin.x` değerini **0.08 → 0.32** yap (classNameLabel ile aynı sol-hizaya gelsinler, portrenin sağında). ax referans satırları ~494/501/508/515 (mottoRt/playstyleRt/resourceRt/lockRt). Sadece anchorMin.x; başka şeye dokunma. classNameLabel zaten doğruysa onu referans al.

# Doğrulama (ZORUNLU)
- refresh_unity compile=request → read_console types=error filter=CS → 0 CS hatası.
- Play-mode probe: IdentityZone'daki motto/playstyle/resource/lock label'larının anchorMin.x=0.32 olduğunu + portre frame'inin sol ~0–0.30'da kaldığını (çakışma yok) raporla.
- Değişen satırları + probe sonucunu profil-DONE'a yaz. Sahne DEĞİŞMEMELİ.
