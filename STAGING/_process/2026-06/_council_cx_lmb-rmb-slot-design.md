ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
  (NLM_NOTEBOOK_ID: 30ddffa5-292f-4248-8e77-68074af901be)
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
LMB/RMB slot tasarım ve üretim kararı için feasibility/reuse analizi

ANALYSIS ONLY — no code changes. Answer from feasibility / what-already-exists / reuse-vs-build lens.

## Sorular

### Q1: ElectricLancer
NLM'e sor: "ElectricLancer class — is this a canonical class name or alias for Elementalist?"
Ayrıca codda `ElectricLancer` string'ini ara (ClassType enum, tüm .cs dosyaları). Kanonik mi, alias mi, typo mu?

### Q2: LMB/RMB slot teknik farkı
- `Assets/Scripts/UI/SkillBarUI.cs` oku — satır 368-374 civarındaki LMB/RMB boş-çizim bloğunu bul
- Diğer skill slotları nasıl SkillBase ile besleniyor? (PlayerSkillKit, ClassSkillKit veya benzeri — Grep et)
- LMB/RMB'yi aktif etmek için sadece o if bloğunu açmak yeterli mi, yoksa class kit → slot binding için ayrı bir sistem (ClassKitSO, PlayerController binding) gerekiyor mu?
- Tahmini iş büyüklüğü: kaç satır / hangi dosyalar?

### Q3: UI layout — sol vs mevcut
- Şu an SkillBarUI nerede konumlandırılıyor? (anchors, Canvas position)
- HP bar / resource bar nerede? (Grep: HPBar, HealthBar, ResourceBar)
- Sol tarafa taşımak için ne gerekir — sadece anchor değişikliği mi, yoksa ayrı prefab/layout mı?

### Q4: İkon üretim teknik
- `Assets/Sprites/UI/Icons/Skills/` klasörüne bak — mevcut 32x32 ikonlar nasıl oluşturulmuş (PixelLab meta var mı, imagegen var mı?)
- `cx imagegen` skill'i projede kullanılmış mı? (STAGING/_process/ veya MEMORY'de iz var mı?)
- PixelLab `create_1_direction_object` vs imagegen: hangisi 32x32 pixel-art için daha önce kullanıldı?

Write result to CODEX_DONE.md. Do NOT reproduce any prior audit. Short, actionable answers.
