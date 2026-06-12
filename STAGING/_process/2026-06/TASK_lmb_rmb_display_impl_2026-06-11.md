# TASK: LMB/RMB Slot Display Implementation (2026-06-11)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Görev
LMB/RMB slot'larını `SkillBarUI`'da display-only göster. `BasicAttackProfile` ScriptableObject'e ikon alanları ekle, Warblade + Elementalist asset'lerini güncelle, `SkillBarUI` LMB/RMB slotlarını bu verilerle doldur.

## Karar Referansı
`STAGING/LMB_RMB_SLOT_DECISION_2026-06-11.md` — kısaca:
- UI-only display, ~40-80 LOC
- Cooldown yok (LMB/RMB CD'siz basic attack)
- `PlayerAttack` binding değişikliği YOK
- LMB/RMB slotları sol başta (slot 0,1), frame %20-30 daha büyük, ember (#E89020) kenarlı, void mor (#3A1A4A) arka plan

## Etkilenen Dosyalar (SADECE BUNLAR)
1. `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs` — `lmbIcon`, `rmbIcon` (Sprite), `lmbName`, `rmbName` (string) alanları ekle
2. `Assets/Scripts/UI/SkillBarUI.cs` — satır 353-374 civarı: slot 0,1 için `BasicAttackProfile` verisini çek, ikon + isim göster, `UpdateSlotEmpty` yerine; LMB/RMB slot frame'lerine görsel ayrışım (boyut/renk)
3. `Assets/ScriptableObjects/BasicAttack/Warblade_BasicAttack.asset` (veya benzer path) — `lmbIcon`, `rmbIcon` ata (mevcut ikonlardan reuse; aşağıya bak)
4. `Assets/ScriptableObjects/BasicAttack/Elementalist_BasicAttack.asset` (veya benzer path) — aynı

## İkon Reuse — Önce Kontrol Et
Mevcut ikon klasörü: `Assets/Art/Icons/Skills/` (veya benzer).

| Slot | Karakter | Tercih |
|---|---|---|
| LMB | Warblade | `skill_cleave.png` veya `skill_bladerush.png` |
| RMB | Warblade | `skill_warstomp.png` |
| LMB | Elementalist | `skill_arcaneblast.png` veya benzeri |
| RMB | Elementalist | `skill_chainlightning.png` veya benzeri |

Uygun ikon bulamazsan: BasicAttackProfile alanlarını ekle ve asset'te boş bırak (null). Ikon üretimi ayrı task.

## BasicAttackProfile.cs Mevcut Yapısı
Dosyayı oku, mevcut alanlara bak, sadece gerekli alanları ekle. Spekülatif alan ekleme.

## SkillBarUI.cs — LMB/RMB Slot Mantığı
- `PlayerAttack` veya `CharacterData` üzerinden `BasicAttackProfile` referansı al (mevcut nasıl bağlıysa öyle)
- Slot 0 = LMB, Slot 1 = RMB olarak kabul et (mevcut kod nasıl işliyorsa doğrula)
- `UpdateSlotEmpty` çağrısını bu slotlar için override et veya koşul ekle
- LMB/RMB frame'leri: `RectTransform.sizeDelta` ile %25 büyüt, `Image.color` = ember (#E89020) border, bg = void mor (#3A1A4A)
- Mevcut slot sistemi nasıl çalışıyorsa ona uygun implement et — mevcut kodu oku, uydur

## Başarı Kriteri
1. Unity'de play mode: skill bar sol başında LMB/RMB slotları ikon + isim gösteriyor
2. Codex console'da compile error yok
3. `git status` — bu 3 dosya modified, başka değişiklik yok

## Commit
Başarılıysa commit:
```
feat(ui): LMB/RMB slot display-only — BasicAttackProfile icons + SkillBarUI binding
```
