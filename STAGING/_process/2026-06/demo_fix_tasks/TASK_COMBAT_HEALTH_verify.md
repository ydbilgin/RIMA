# TASK — Combat health verify (DATA-driven, screenshot'a güvenme)

ACTIVE RULES: (1) think before acting (2) min steps (3) surgical — TANI + tek gerçek-combat capture; gerekmedikçe kod değiştirme (4) BLOCKED if combat unreachable.

NLM ACCESS: gerekirse NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR, raporda console durumunu yaz.

E1 OUTPUT: Sonucu DOSYAYA yaz (DONE); dönüşün ≤10 satır + yol.

## AMAÇ (kritik gate)
capture_v3'te "combat" diye etiketli kareler aslında DEATH (KILLS 0 / SÜRE 00:00) veya DRAFT ("ODA 1 — ÖDÜL SEÇ") ekranı çıktı → **gerçek mid-combat HİÇ doğrulanmadı.** Şüphe: oyuncu oda-1'de anında mı ölüyor? Combat'ın GERÇEKTEN oynanabilir olduğunu **DATA ile** (execute_code, screenshot DEĞİL) doğrula. Bu, CombatJuice beautification öncesi ZORUNLU gate — combat bozuksa #1 öncelik o.

## ADIMLAR
1. `_Arena` yükle (gerekirse full-flow MainMenu→run), enter play.
2. Opening draft varsa geç (1 kart seç — programatik veya input).
3. **execute_code ile zaman içinde ÖLÇ** (her ~3-4s'de bir örnek, ~15-20s combat boyunca): player current/max HP, alive enemy count, kill/clear count, elapsed time. Sayıları topla.
4. **DOĞRULA:**
   - Oyuncu ≥15s hayatta mı (anında ölmüyor)?
   - En az 1 düşman öldürülebiliyor mu (KILLS artıyor)?
   - Düşmanlar hasar veriyor ama instant-kill ETMİYOR mu?
   - Düşmanlar görünür + aktif (idle değil) mi?
5. **ARAŞTIR instant-death:** prior death (KILLS 0/SÜRE 00:00) tekrar üretiliyor mu? Bir instant-death / soft-lock path var mı? Aday kök nedenler: spawn-overlap collision damage · player spawn'ı düşman içinde · draft-skip→death · environment/fall hasarı · wave anında flood. Hangisi olduğunu KANITLA (data).
6. **1 GERÇEK mid-combat screenshot:** düşmanlar canlı + player dövüşüyor + HP bar görünür. Semantic doğrula (death/draft DEĞİL). `capture_v3/` veya `capture_v3_combat/` altına açıklayıcı isimle.
7. read_console.

## VERDICT (raporun başına)
**COMBAT: HEALTHY / BROKEN** + neden. Ölçüm tablosu (t | playerHP | aliveEnemies | kills). Instant-death bulgusu (var/yok + kök neden + hangi dosya/satır). Eğer BASİT+kesin bir fix varsa (örn. spawn-overlap), öner ama UYGULAMA — orchestrator karar verecek.

RAPOR: `STAGING/_process/2026-06/demo_fix_tasks/DONE_combat_health.md`
