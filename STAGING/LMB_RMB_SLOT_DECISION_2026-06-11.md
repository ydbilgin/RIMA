# LMB/RMB Slot — Council Karar (2026-06-11)

Advisors: cx (yasinderyabilgin) · ax 3.1 Pro · ax 3.5 Flash · Opus (sentez)

---

## 1. ElectricLancer
**KARAR: Typo/drift. Koda hiç girmemiş.**
- `ClassType` enum'da `ElectricLancer` = 0 hit. Kanonik: `Elementalist`.
- Yeni class planlanıyorsa önce NLM contract'a girecek. Şimdilik ignore.

## 2. LMB/RMB Slot Fix — Teknik Yaklaşım
**KARAR: UI-only display, ~40-80 LOC. Full SkillBase integration = erteleme.**

### Yapılacak
- `SkillBarUI.cs` satır 353-374: LMB/RMB slotları `UpdateSlotEmpty` yerine `BasicAttackProfile` verisini gösterecek
- `BasicAttackProfile` ScriptableObject'ten `lmbIcon` / `rmbIcon` (32x32 PNG) + isim çek
- Display-only: ikon + isim render, cooldown yok (LMB/RMB CD'siz basic attack)
- `PlayerAttack` binding değişikliği YOK — sadece görsel

### Etkilenen dosyalar
- `Assets/Scripts/UI/SkillBarUI.cs` (satır 353-374 civarı)
- `Assets/Scripts/Combat/BasicAttack/BasicAttackProfile.cs` (alan ekle: `lmbIcon`, `rmbIcon`)
- Warblade + Elementalist `BasicAttackProfile.asset` (ikon ata)

### Kapsam dışı (şimdi değil)
- `PlayerAttack` → SkillBase bridge (~150-300 LOC)
- Sol panel / class identity panel

## 3. Yerleşim — LMB/RMB Konumu
**KARAR: Skill bar başında, görsel ayrışım (Opsiyon C). Sol panel = erteleme.**

- LMB/RMB slotları `SkillBarUI`'da sol başta kalır (zaten slot 0,1)
- Görsel ayrışım: slot frame %20-30 daha büyük, ember (#E89020) kenarlı, void mor (#3A1A4A) arka plan
- Anchor değişikliği YOK, layout iş YOK
- Sol panel (HP yanı, Diablo tarzı) = class identity panel olarak ileriki sprint'e ertelendi

## 4. İkon Üretim
**KARAR: Mevcut 65 ikondan reuse → yoksa cx imagegen 32x32. PixelLab değil.**

### Warblade
- LMB (Iron Combo / melee slash): `skill_cleave.png` veya `skill_bladerush.png` reuse
- RMB (Rage Outlet / AoE burst): `skill_warstomp.png` reuse
- Yoksa imagegen prompt: `"32x32 pixel art skill icon, [aksiyon], transparent background, dark fantasy, no frame, no border"`

### Elementalist
- LMB (Rift Bolt / projectile): `skill_arcaneblast.png` veya benzeri reuse
- RMB (Element Switch / Lightbreak): `skill_chainlightning.png` veya benzeri reuse
- Yoksa imagegen

### Import ayarları (mevcut canon)
- 32x32 RGBA, point filter, no mipmaps, PPU 64, transparent BG
- `SkillIconRegistryBuilder` ile registry rebuild

---

## Uygulama Sırası
1. cx dispatch: `BasicAttackProfile` ikon alanı + Warblade/Elementalist asset güncelleme + `SkillBarUI` LMB/RMB display (effort: medium)
2. Uygun ikon yoksa: imagegen skill ile üret
3. SkillIconRegistryBuilder rebuild
4. Sol panel / class identity panel: sonraki sprint

---

*Çakışma notu: 3.1 Pro PixelLab önerdi, cx mevcut pipeline'ın imagegen-ok olduğunu kanıtladı. cx galip (proje geçmişi > dış öneri).*
