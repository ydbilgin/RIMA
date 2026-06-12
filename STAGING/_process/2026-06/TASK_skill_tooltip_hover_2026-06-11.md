ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

# TASK: Skill Bar — Hover Tooltip (Skill Açıklaması)

## HEDEF
Skill bar'daki skill slotlarının üzerine mouse gidince bir tooltip popup açılsın ve skill adı + açıklaması gösterilsin.

## ARAŞTIR
1. `Assets/Scripts/UI/` altında SkillBar ile ilgili scriptleri bul (Grep: SkillBar, SkillSlot, SkillButton)
2. Mevcut skill data yapısını bul — SkillSO veya benzeri ScriptableObject (Grep: SkillSO, ISkill, SkillData)
3. Tooltip sistemi varsa bul (Grep: Tooltip, TooltipUI)

## YAPILACAK
- Her skill slot'una Unity EventSystem `IPointerEnterHandler` / `IPointerExitHandler` ekle
- Tooltip: skill adı (bold) + açıklama metni. Basit TextMeshPro panel, Canvas içinde.
- Tooltip pozisyonu: mouse yakını, ekran sınırını aşmasın (clamp)
- Eğer global TooltipManager yoksa minimal bir tane yaz (singleton, Show/Hide metotları)
- Mevcut SkillSO'da açıklama alanı yoksa `[TextArea] string description` ekle

## BAŞARI KRİTERİ
- Skill üzerine gidince tooltip görünür, çıkınca kaybolur
- Compilation error yok
- Unity test runner pass (varsa ilgili test)
