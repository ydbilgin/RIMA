ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

# Code Review — Antigravity S95 Cleanup

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Görev

Antigravity (Gemini) aşağıdaki değişiklikleri yaptı. Her dosyayı oku, review et, PASS/FAIL ver.

## İncelenecek Dosyalar

### 1. Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs
**Yapılan:** Legacy juice çağrıları silindi (HitStop, LightPulse, DamagePopup, CameraShake).
**Kontrol et:**
- CombatEventBus.PublishHit + PublishKill doğru çağrılıyor mu?
- Null reference riski var mı?
- Knockback null check yeterli mi?
- Herhangi bir compile hatası riski var mı?

### 2. Assets/Scripts/Combat/BasicAttack/MarkPulseBehavior.cs
**Yapılan:** Antigravity dokunmadı (CombatEventBus yoktu, sadece legacy vardı).
**Kontrol et:**
- Bu karar doğru muydu? Legacy çağrılar burada sorun yaratır mı?
- Bus subscriber'larla çift efekt riski devam ediyor mu?
- Öneride bulun: Bus eklenip legacy silinmeli mi?

### 3. Assets/Editor/RimaUnifiedPainterWindow.cs
**Yapılan:** Props_Root parent + sub-gruplar eklendi (Walls/Statues/WallMountings/Patches/Mobs/FloorProps).
**Kontrol et:**
- Props_Root bulma/oluşturma mantığı sağlam mı? (null check, scene context)
- GetRecursiveChildren doğru implement edilmiş mi?
- Sub-grup kategorizasyonu prefab naming convention ile uyumlu mu? (`wall_*`, `statue_*`, `mounting_*` vb.)
- Erase/Save/Load/WallConnect fonksiyonları rekürsif güncellemeyi kapsamış mı?
- Herhangi bir memory leak veya Editor-only API hatası riski var mı?
- **UI/UX açısından:** Sub-grup hiyerarşisi kullanıcıya sahnede görünüyor mu? Daha kullanıcı dostu yapılabilir mi? Örn. grup başlıkları, toggle, renkli etiket?

### 4. Assets/Scenes/Demo/PathC_BaseTest.unity
**Yapılan:** Props_Root GameObject sahnede scene root'a eklendi.
**Kontrol et:**
- Props_Root transform identity mi? (0,0,0 pos, 0,0,0 rot, 1,1,1 scale)
- Grid/Tilemap'in child'ı olmadığını doğrula

## UI/UX Genel Değerlendirme

RimaUnifiedPainterWindow.cs tüm dosyayı okuyarak şunu değerlendir:
- Mevcut arayüz bir map designer için kullanıcı dostu mu?
- En kritik 3 UX sorunu nedir?
- Öneride bulun (kod yazmana gerek yok, sadece öneri)

## Output Formatı

Sonucu STAGING/CODEX_DONE_review_antigravity_s95.md olarak yaz:

```
# Codex Review — Antigravity S95 Cleanup

## BasicAttackBehaviorBase.cs
Verdict: PASS / FAIL / PASS_WITH_NOTES
Bulgular: ...

## MarkPulseBehavior.cs
Verdict: PASS / FAIL / PASS_WITH_NOTES
Karar doğru muydu: EVET/HAYIR
Öneri: ...

## RimaUnifiedPainterWindow.cs
Verdict: PASS / FAIL / PASS_WITH_NOTES
Bulgular: ...
UI/UX Notlar: ...

## PathC_BaseTest.unity
Verdict: PASS / FAIL
Bulgular: ...

## UI/UX Genel Öneriler (Top 3)
1. ...
2. ...
3. ...

## Genel Verdict
PASS / PASS_WITH_NOTES / FAIL
```


---
ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE_yasinderyabilgin.md AS THE VERY LAST STEP.