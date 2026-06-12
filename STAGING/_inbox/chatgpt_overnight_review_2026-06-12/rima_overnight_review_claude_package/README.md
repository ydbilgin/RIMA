# RIMA Overnight Review Fix Package — 2026-06-12

Bu paket, RIMA Unity 2D ARPG roguelite projesindeki overnight autonomous run sonrası yapılan comparative correctness + design review bulgularını Claude/Codex uygulaması için düzenler.

## İçerik

1. `00_PROMPT_FOR_CLAUDE.md`
   - Claude'a doğrudan yapıştırılacak ana görev prompt'u.
   - Fix sırası, guardrails, test beklentileri ve raporlama formatı içerir.

2. `01_FINDINGS_DETAILED.md`
   - Kritik / orta / canon sapması bulgularının detaylı açıklaması.
   - Neden riskli oldukları ve hangi dosyalara bakılması gerektiği yazılı.

3. `02_PATCH_PLAN_BY_FILE.md`
   - Dosya dosya uygulanacak değişiklik planı.
   - Kod mimarisi önerileri ve minimal/refactor seçenekleri.

4. `03_TEST_AND_VALIDATION_CHECKLIST.md`
   - Combat correctness, Director Mode, telemetry, regression ve manual playtest checklist'i.

5. `04_ACCEPTANCE_CRITERIA.md`
   - Claude işi bitirdiğinde neyi “done” kabul edeceğiz.
   - Fail durumunda ne raporlanmalı.

6. `05_SHORT_ISSUE_BODY.md`
   - İstersen GitHub issue/task olarak yapıştırılacak kısa özet.

## Özet karar

Stat math core genel olarak iyi; ama production wiring tarafında üç ciddi risk var:

- Melee finisher yanlışlıkla crit gibi hesaplanıyor olabilir.
- DamageCalculator defender stats destekliyor ama SkillRuntime production path defender stat geçmiyor.
- Ranger basic projectile yeni DamagePacket/DamageCalculator hattını bypass ediyor.

Director Mode tarafında kümülatif sahne davranışı canon gereği bug değil; fakat Test modunda overlay/raycast sızıntısı mutlaka doğrulanmalı.

## Kullanım

Claude'a önce `00_PROMPT_FOR_CLAUDE.md` dosyasını ver.
Ardından bu zip'in tamamını bağlam olarak yükle.
Claude'dan önce plan çıkarmasını, sonra patch uygulamasını, sonra test raporu vermesini iste.

Önerilen çalışma sırası:

1. Critical combat fixes
2. Director Test-mode raycast/visibility validation
3. HP authority ve stat provider tasarım kararı
4. Tests
5. Manual play notes
