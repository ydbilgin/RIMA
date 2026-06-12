ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Modular-design council kararının (STAGING/MODULAR_ABILITY_DECISION_2026-06-12.md) "Şimdi Yap" adımının 3'ü: "HP/resource eşiği → stat modifier" kalıbındaki basit pasifleri tek inspector-configurable generic component'e indirgeyen `Passive_StatThreshold` ekle. PİLOT olarak SADECE Wrath Protocol'ü ona bağla. Diğer pasifleri KÖRÜ KÖRÜNE migrate ETME — kaç tanesinin kalıba uyduğunu raporla, karar orchestrator'da.

## Bağlam: mevcut pilot pasif
`Assets/Scripts/Skills/Passives/WarbladePassives.cs:66` `Passive_WrathProtocol`:
HP < %50 iken `rage.gainMultiplier += bonus` (level 1/2/3 → +0.20/+0.35/+0.50), HP %50 üstüne çıkınca geri alır. `PassiveBase`'den türer, `OnLevelUp(level)` + `Update()` kullanır.

## İŞ
1. `Assets/Scripts/Skills/Passives/` altına `Passive_StatThreshold : PassiveBase` ekle. Inspector-configurable:
   - eşik kaynağı (HP ratio — şimdilik sadece HP yeter, resource'u spekülatif ekleme)
   - eşik değeri (örn 0.5 = %50)
   - karşılaştırma yönü (altında / üstünde)
   - hedef stat + level başına magnitude dizisi (1/2/3)
   - aktif/deaktif histeresis (WrathProtocol'deki `wasLowHP` enter/exit mantığını birebir koru — sürekli += yapmasın)
   Mevcut WrathProtocol davranışını BİREBİR üretebilmeli (enter eşik → bir kez ekle, exit → bir kez çıkar).
2. Wrath Protocol'ü bu generic'e PİLOT olarak bağla. Eski `Passive_WrathProtocol` class'ını SİLME — ya generic'e delege et ya da yan yana bırak; demo'da çalışan davranış BOZULMAMALI. En güvenli yol: generic'i ekle, WrathProtocol'ü generic'i kullanacak şekilde sadeleştir AMA aynı public yüzeyi/prefab referanslarını koru. Prefab/SkillData referansı kırılacaksa → o referansı koru, BLOCKED yaz.
3. Diğer pasifleri tara (`Assets/Scripts/Skills/Passives/**` + tüm sınıflar). "HP/resource eşiği → stat modifier (enter/exit histeresisli)" kalıbına TEMİZ uyan kaç pasif var? Listele. UYANLARI bu task'ta migrate ETME — sadece raporla (sonraki karar orchestrator'da). Uymayanları (event-driven heal, floor-clamp, knockback-trigger gibi) bespoke bırak.

## YAPMA
- WrathProtocol dışındaki pasifleri migrate etme (sadece raporla).
- Resource-threshold / çoklu-stat / spekülatif config alanı ekleme — minimum.
- SkillRecipe SO / Step 1 helper'larına dokunma (o ayrı task, tamamlandı).
- Prefab/SkillData/SkillDatabase referanslarını kırma — kırılırsa BLOCKED.

## Doğrulama gate'i (commit ÖNCESİ zorunlu)
1. Derlenmeli (compile error YOK).
2. Edit-mode testleri yeşil. Pasif testi varsa özellikle koştur.
3. Mümkünse: Wrath Protocol'ün enter/exit davranışını doğrulayan minimal bir edit-mode test ekle (HP %50 altına in → multiplier artar, üstüne çık → geri döner). Test zaten varsa yenisini yazma.
4. Yeşil DEĞİLSE commit ETME → BLOCKED + hata çıktısı.
5. Yeşilse commit: `refactor(passives): add Passive_StatThreshold generic + pilot Wrath Protocol [behavior preserved]`.

## CODEX_DONE'a yaz
- `Passive_StatThreshold` config alanları + imza.
- Wrath Protocol nasıl bağlandı (delege mi, sadeleştirme mi) + davranış korunma kanıtı.
- Kalıba uyan DİĞER pasiflerin listesi (migrate edilmedi — gelecek karar için).
- Test sonucu.
