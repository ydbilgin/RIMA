ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Kullanıcı direktifi: "HER ODA her zaman bitirilebilir (clear) olmalı; her odada kontrol et." HİÇBİR oda soft-lock olamaz. Mevcut clear pipeline VAR (önceki cx audit doğruladı: EncounterController.cs:142-175 → RoomRunDirector.cs:520-529,727-817). Görev = bu garantiyi denetle + boşlukları kapat + her odayı test eden EditMode testi ekle.

# Canlı sistem (legacy'ye DOKUNMA: RoomLoader/RuntimeRoomManager/Gate.cs/GateBehavior/DoorTrigger)
Canlı yol: `_Arena` → RoomRunDirector → IsoRoomBuilder → EncounterController → RoomRunExitDoorTrigger → DungeonGraph.

# PART A — AUDIT (önce bunu yap, sonuçları raporla)
Her oda TÜRÜ ve DemoRoomBank.asset'teki her template için clear koşulunu çıkar (file:line):
1. Combat/Elite/Boss odaları: clear = tüm düşman ölümü mü? Wave sayısı nereden geliyor (EncounterController/RoomTemplateSO/encounter config)? "2 mob öldürünce kilit" = 2. wave mı bekleniyor, yoksa düşman walkable-dışı spawn olup "alive" mı kalıyor?
2. **DÜŞMANSIZ odalar** (Treasure/Intro/Boss_Intro/Event veya enemy count=0 olan herhangi template): bunlar nasıl clear oluyor? EncounterController 0 düşmanla OnRoomCleared fire ediyor mu, yoksa clear'ı asla tetiklemeyip KİLİTLENİYOR mu? (En olası soft-lock kaynağı bu.)
3. Clear sonrası: reward/draft modalı açık kalırsa Time.timeScale=0 kalıp input kilitler mi? Exit kapısı her clear'da en az 1 tane açılıyor mu?
4. DemoRoomBank'teki TÜM template'leri tek tek listele: tür + enemy/wave + exit slot sayısı + "completable mı?" değerlendirmesi.

# PART B — IMPLEMENT (sadece NET-GÜVENLİ olanlar, cerrahi, mevcut çalışan combat akışını BOZMADAN)
1. **Düşmansız oda garantisi:** Bir oda 0 düşmanla başlıyorsa (Treasure/Intro vb.) clear ANINDA tetiklensin (exit/reward açılsın), asla beklemede kalmasın. (EncounterController başlatmada activeEnemies==0 ise hemen OnRoomCleared.)
2. **Clear sequence sağlamlığı:** RoomClearSequence her durumda Time.timeScale'i 1'e restore etsin + en az 1 exit açsın (zaten kısmen var; garantiyi sağlamlaştır, regression yaratma).
3. Değişiklik MİNİMAL olsun; mevcut yeşil testleri kırma.

# PART C — TEST (kullanıcının "her odada kontrol et" direktifi = bu test)
EditMode testi ekle: DemoRoomBank'teki (ve mümkünse tüm RoomTemplateSO'ların) HER template için "completable" invariant'ı assert et:
- ya en az 1 öldürülebilir düşman + en az 1 exit slot,
- ya da düşmansızsa auto-clear yolu mevcut + en az 1 exit slot.
Hiçbir template "düşman bekliyor ama hiç düşman yok / exit yok" durumunda OLMAMALI. Test her oda için tek tek rapor versin (hangi oda PASS/FAIL).

# PART D — PROPOSE ONLY (implement ETME, file:line ile öner)
Runtime "düşman walkable-dışı sıkıştı / wave spawn olmadı" gibi durumlar için bir watchdog/fail-safe (örn. X saniye alive-enemy-hareketsiz veya unreachable → force-clear) MANTIKLI mı? Tasarımını öner, ben karar vereceğim. (Şimdi yazma — maskeleme riski var.)

# Çıktı
CODEX_DONE.md'ye: PART A audit tablosu (her oda) + PART B değişen dosyalar (file:line + root-cause) + PART C test sonucu (her oda PASS/FAIL) + PART D öneri. Testleri çalıştır, sonucu yaz. BLOCKED yaz belirsizse. Untracked dosyalara dokunma.
