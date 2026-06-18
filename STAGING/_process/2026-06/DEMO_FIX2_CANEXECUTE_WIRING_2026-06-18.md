# FIX-UP: SkillBase CanExecute() override wiring (2026-06-18)

Council (auditor-opus + cx, ikisi de PASS-WITH-FIXES) kararı: FIX 2 infra-only kaldı, hiçbir skill `CanExecute()` override etmiyor → range-gated no-op skiller hâlâ mana+cd israf ediyor (asıl "ölü buton" semptomu). Bu fix-up onu tamamlar.

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## UNITY ERROR CHECK
İş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR. Instance `RIMA@ed023e0b`. **Sen TEK Unity-ajansın.** PLAY ETME — sadece compile-clean.
⚠️ Kullanıcı şu an `_Arena` sahnesini editörde açık tutuyor; recompile/domain-reload normal, sahneyi DIRTY bırakma/kaydetme — sadece kod dosyalarına dokun.

## ALTYAPI (zaten var, commit edilmemiş)
`SkillBase.cs`: `protected virtual bool CanExecute() => true;` eklendi, `TryActivate()` cost/cooldown'dan ÖNCE `if (!CanExecute()) return false;` çağırıyor. Sen sadece subclass override'larını ekleyeceksin.

## GÖREV: 5 skill'e `protected override bool CanExecute()` ekle
Her skill için: `Execute()`'i OKU → Execute'in HİÇBİR ŞEY yapmadan döndüğü kesin no-op koşulunu bul (genelde "menzilde/range'de hedef yok" → CircleCast/OverlapCircle boş) → AYNI tespiti `CanExecute()` içinde READ-ONLY tekrarla, no-op durumunda `false` döndür.

Hedef skiller (auditor tespiti):
- `Assets/Scripts/Skills/Elementalist/ChainLightning.cs` (~satır 33) — ASIL demo bug (menzilde düşman yok).
- `Assets/Scripts/Skills/Warblade/CripplingBlow.cs` (~32)
- `Assets/Scripts/Skills/Warblade/SunderMark.cs` (~31)
- `Assets/Scripts/Skills/Warblade/DeepWound.cs` (~35)
- `Assets/Scripts/Skills/Warblade/IronCounter.cs` (~82)

## KRİTİK KURALLAR
1. **CanExecute() YAN-ETKİSİZ olmalı** — state değiştirme, kaynak harcama, VFX/ses tetikleme YOK. Sadece sorgu (ör. `Physics2D.OverlapCircle(..., enemyMask) != null`). Execute'tan SONRA da çalışacak, çift-iş olmasın.
2. **False-positive YASAK:** Geçerli bir cast'i yanlışlıkla bloklamaktan KAÇIN. Demo'da geçerli skill'in bloklanması, mana israfından DAHA KÖTÜ. Bir skill'in no-op koşulundan EMİN DEĞİLSEN → o skill'i ATLA (override ekleme, raporda "uncertain, skipped" yaz). ChainLightning kesin → mutlaka wire et.
3. **IronCounter doğrula:** Eğer self-buff/parry gibi HER ZAMAN bir şey yapan skill ise (target gerektirmiyorsa) override EKLEME — sadece gerçekten target-gated no-op edebilen skillere ekle.
4. Cerrahi: sadece bu 5 dosya. Execute'in kullandığı menzil/mask/yarıçap değerlerini birebir aynala (yeni sabit uydurma).

## DOĞRULAMA
Recompile → `editor_state.isCompiling` false → `read_console` (Error+Warning) → 0 error. PLAY ETME.

## ÇIKTI (E1)
Detay → `STAGING/_process/2026-06/DEMO_FIX2_CANEXECUTE_DONE_2026-06-18.md` (her skill: eklenen koşul + Execute'teki kaynağı + atlanan varsa neden + console durumu).
Bana dönüş ≤8 satır: hangi skiller wire edildi / atlandı, console durumu, rapor yolu.
