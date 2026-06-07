# Claude'a Verilecek Cerrahi Düzeltme Prompt'u

Aşağıdaki görev, RIMA final raporunun ikinci review turundan çıkan cerrahi düzeltmeleri uygulamak içindir.

## Ana hedef

Raporu baştan yazma. Toptan yeniden yapılandırma yapma. Sadece final teslim öncesi güven kıracak yerleri, görsel/metin çelişkilerini, abartılı iddiaları, sayı tutarsızlıklarını ve terminoloji problemlerini düzelt.

## Girdi dosyaları

- `RAPOR_RIMA_2026-06-06.docx`
- `GAME_DESIGN_INTENT_2026-06-06.md`
- `VISION_VS_CURRENT_2026-06-06.md`
- Bu paketteki review dosyaları

## Kritik doktrin

- Floor yeniden üretme.
- Full wall sistemi önermeye veya yazmaya çalışma.
- Normal odalar floating-island + cliff + portal yaklaşımıdır.
- Physical wall door değil, Rift portalı kullanılır.
- 8 yön portal yok.
- Portal facing direction = 1.
- Exit socket = EXIT_NW, EXIT_N, EXIT_NE.
- Entry = ENTRY_S arrival anchor; kalıcı portal objesi değil.
- Graph’ta olmayan Heal/Lore portalı raporda vaat edilmemeli.
- Var olan sistemleri yok sayma; sıfırdan sistem yazma önerme.

## Uygulama görevleri

### 1. Görsel/şekil temizliği
Rapor içindeki Şekil 1–5'i kontrol et. Debug kare, yanlış UI state, placeholder veya caption ile uyuşmayan içerik varsa işaretle ve değişim listesi oluştur.

Gerekli yeni görseller:
- Şekil 1: temiz Attunement Chamber genel görünümü.
- Şekil 2: net `[G] Bürün — SINIF` prompt'u.
- Şekil 3: aktif combat + `[RMB] İnfaz` prompt'u mümkünse görünür.
- Şekil 4: 3-kart draft + tooltip + sinerji.
- Şekil 5: back-edge Rift portalları.
- Şekil 6: Warblade render.
- Şekil 8: Room JSON / Editor Paint → RoomJsonImporter → RoomTemplateSO → Validator → IsoRoomBuilder → _Arena Runtime Room → QC Screenshot / Smoke Test.
- Şekil 13: Test Runner.
- Şekil 14: QC before/after.

### 2. Terminoloji düzeltmesi
Rapor genelinde şunları düzelt:
- “arka duvar kapısı” → “arka kenardaki Rift portalı”
- “kapı” → bağlama göre “Rift portalı / çıkış portalı”
- “door slot” → “portal socket”
- “duvar” çağrışımını normal runtime odalarında azalt.

### 3. Gate-slot açıklaması
§3.5.5 içine şu fikri net ekle:

RIMA'da portal yön sayısı ve slot sayısı ayrıdır. Demo kapsamında tek bir temel portal facing direction kullanılır; aynı portal gövdesi back-edge üzerindeki üç çıkış soketine yerleştirilir: EXIT_NW, EXIT_N, EXIT_NE. Graph 1 çıkış üretirse EXIT_N, 2 çıkış üretirse EXIT_NW + EXIT_NE, 3 çıkış üretirse üçü birlikte render edilir. ENTRY_S kalıcı portal objesi değil, spawn/arrival anchor’dır.

### 4. Sayı tutarlılığı
26 oda şablonu ile 25 gate-slot migrasyonu çelişiyorsa açıkla:
“26 şablonun 25’i run odasıdır; Attunement Chamber özel akışa sahip olduğu için migrasyon dışında tutulmuştur.”

Test tarafında:
- ~529 test tanımı
- 410 PASS / 0 FAIL / 1 inconclusive son kayıtlı koşu

Bunları aynı şeymiş gibi yazma. Test envanteri ve koşu sonucu olarak ayır.

### 5. Metodoloji güçlendirme
Reviewer-FAIL vakasını tabloya dönüştür. 9 bug iddiası varsa gerçek bug gruplarıyla eşleştir. Anekdot gibi bırakma.

Önerilen tablo:
| Review vakası | Bulgu sayısı | Kritik bulgu | Sonuç |
|---|---:|---|---|

### 6. Oyun hissi / ses çelişkisi
Ses durumunu ikiye ayır:
- demo temel SFX katmanı
- final/orijinal müzik ve sınıf özel sesler

“Ses var mı yok mu?” sorusu doğmamalı.

### 7. Skill placeholder güvence cümlesi
Eğer doğruysa ekle:
“Placeholder kayıtlar SkillDatabase’de tasarım envanteri olarak tutulmakta, demo draft havuzuna dahil edilmemektedir; draft sistemi yalnızca implementasyonu tamamlanmış ve sınıf filtresinden geçen skill’leri sunar.”

### 8. Encoding temizliği
Türkçe karakterleri kontrol et:
- Cift → Çift
- Yonlu → Yönlü
- Katmani → Katmanı
- dogru → doğru
- calisması → çalışması
- sarsintisi → sarsıntısı
- tanımlanmıstır → tanımlanmıştır

## Çıktı formatı

Lütfen iki çıktı üret:

1. `REPORT_PATCH_PLAN.md`
   - Hangi bölümde ne değişecek?
   - Hangi cümle değişecek?
   - Hangi şekil değişecek?
   - Risk seviyesi nedir?

2. Uygulanmışsa:
   - düzeltilmiş docx
   - değişiklik özeti

## Acceptance criteria

- Şekiller caption’ların vaat ettiği şeyi gösteriyor.
- Debug kare yok.
- Placeholder görsel notu yok.
- Portal-only doktrin ile “kapı/duvar” dili çelişmiyor.
- Test sayıları karışmıyor.
- Reviewer-FAIL metodoloji kanıtı olarak okunuyor.
- 10 sınıf / 4 playable dürüst kapsam beyanı korunuyor.
- Türkçe karakter bozukluğu kalmıyor.
