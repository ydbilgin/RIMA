# COUNCIL DECISION — Demo-arifesi sıradaki iş önceliklendirme (2026-06-18)

**READ-ONLY. Kod/dosya/git'e DOKUNMA. Unity ÇALIŞTIRMA.** Sadece karar + gerekçe.

## Durum
RIMA Unity ARPG, **demo ~yarın, editörde canlı sunulacak**. Bugün tamamlanan + commit'lenen (`6ba61ff5`, test-clean, council PASS):
- HIGH-4 demo-killer fix: movement off-map (WalkabilityMap guard) · SkillBase spend-before-veto + 4 skill CanExecute override · RunStats progression-desync · boss Phase-2 8s lock.
- Demo-polish uygulandı (uncommitted): URP Bloom/ColorGrade/Vignette + GlobalLight 0.5 + brazier warm 2D-light + camera-bg.

## KARAR: Demo'ya kalan sürede ne yapılmalı, hangi sırayla? Neyi ertele?
Her advisor şunları değerlendirsin (demo-görünürlüğü × risk ekseninde; demo-arifesi = core'a riskli dokunma):

### A) MED/LOW bug backlog (hangileri demo-relevant = canlı sunumda görünür, hangileri post-demo?)
1. **Failed-cast SESSİZ no-op** — yetersiz kaynak/menzilde hedef yokken ses/flash/toast YOK (sunucu skill basıp hiçbir şey olmaması = bozuk görünür). Fix: failed-cast feedback (SFX/flash/toast).
2. **healMultiplier kalıcı bozulma** — Penitent boss AntiHealAura × Warblade Crippling Blow concurrent save/restore race.
3. **Director dup-slot** — zaten-equipped skill 2. slota → shared cd (Director Mode demo-centerpiece).
4. **Merchant PERSISTENT Echo drain** — shop meta-currency harcıyor (run-vs-meta sınır ihlali).
5. **Dead-but-acting** — ~2.3s ölüm penceresinde kapı/ödül etkileşimi.
6. **Glacial+Burn detonate yok · Ice-Shatter 3x DEAD CODE · Severance 1-Scar** (combo correctness, subtle).

### B) Tier-2 polish (mevcut sistemi tune; demo-değeri var mı, risk?)
- Hit-flash beyaz (combat okunabilirliği — hasar path'ine dokunur) · HUD HP-bar lerp + toast ease (UI, düşük risk) · low-HP/Rage kırmızı-ekran de-stack (overlay glitch, UI).

### C) Polish commit + exposure
- Uygulanan polish'i şimdi commit'lemeli mi? Sahne hâlâ "karanlık" — postExposure 0.35→0.6 açılmalı mı, yoksa moody-atmosfer kalsın mı?

### D) Post-demo (perf) — sadece teyit: 9 Find-in-hot-path (CameraFollow/BaseMobBehavior/PlaytestRoomClearedHelper en kötü). Demo-blocker değil, guarded-cache post-demo. Katılıyor musun?

## ÇIKTI
**Sıralı aksiyon planı** (yarına kadar yapılacaklar, öncelik sırasıyla) + her madde için demo-değeri/risk gerekçesi + ertelenecekler listesi. Çıktıyı `STAGING/_process/2026-06/_council_next_priority_<advisor>_2026-06-18.md`'ye yaz, dönüşte ≤8 satır.
