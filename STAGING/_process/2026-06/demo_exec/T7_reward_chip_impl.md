# T7 — Reward card trigger/outcome chip from canonical SO (DATA-01)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only the reward-card UI file(s) (4) BLOCKED if unclear.
NLM ACCESS: design canon via `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query "$NB" "<q>"`. Direct-read only: CURRENT_STATUS.md, .claude/PROJECT_RULES.md, code, STAGING, memory.
GRAPHIFY: cross-file/architecture sorularında önce graphify query (graph.json: STAGING/_process/2026-06/graphify_fullmap/graphify-out/), bulk-read'den ~71x ucuz.
UNITY ERROR CHECK: iş bitince `read_console` (Error+Warning); KENDİ hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme); raporda console durumunu (0-error şart) yaz.
E1: sonucu `.cx_dispatch` done dosyasına yaz, dönüş ≤10 satır özet + değişen dosyalar.

## Amaç
RIMA demo (19 Haziran). Reward draft (skill seçim kartı) üzerinde bir skill'in **trigger** (ne zaman tetiklenir) ve **outcome/combo** (ne yapar) bilgisini gösteren "chip"/satır, **canonical ScriptableObject verisinden** okumalı — sabit/yanlış bir eşleştirme tablosundan DEĞİL (DATA-01: "X ile eşleşir" bazı yerlerde canonical skill verisiyle uyuşmuyor).

## Kısıtlar (ZORUNLU)
- **Surgical, NO refactor.** Sadece reward-card render eden UI dosyası (muhtemelen `Assets/Scripts/UI/SkillOfferUI.cs`) + gerekiyorsa ilgili skill SO okuma. RewardPickup.cs'e DOKUNMA (REWARD-02 zaten çözüldü). DungeonGraph/RoomRunDirector'a DOKUNMA (run-map branching done).
- **8-yön canon LOCKED** — yön/sprite işine girme.
- Bir ChatGPT paketi combo tablosunu HARD-CODE ETME. Paketin "Earthsplitter->Gravity Cleave canonical değil" iddiası DOĞRULANMADI — gerçek skill SO'su ne diyorsa onu göster.

## Yapılacak (investigate-first)
1. `SkillOfferUI.cs`'te reward kartının trigger/outcome/combo metnini nasıl ürettiğini bul (grep: "trigger", "combo", "outcome", "eşleş", Loc.T çağrıları, kart alanları).
2. Skill verisinin canonical kaynağını bul (skill ScriptableObject — ör. SkillDefinitionSO / SkillSO / ability SO; grep skill SO tipini + trigger/outcome alanlarını). Karttaki metin BU SO alanlarından gelmeli.
3. Eğer kart zaten SO'dan okuyorsa: DATA-01'in gerçek kök-nedenini doğrula (yanlış alan mı, eksik Loc key mi) ve cerrahi düzelt. Eğer hard-coded/yanlış eşleştirme varsa: SO okumaya çevir.
4. Loc key'leri çözülüyor mu doğrula (eksikse ekle, TR+EN — Loc.cs çift dilli mevcut).

## Doğrulama (GATE)
- Compile + `read_console` = **0 error** (zorunlu, raporla).
- Mümkünse EditMode: kartın gösterdiği trigger/outcome bir örnek skill SO'sunun alanlarıyla eşleşiyor (assert) — ya da execute_code ile bir skill SO yükleyip kartın ürettiği metni karşılaştır (data-proof).
- Oyun-içi screenshot (chip görünür) = T2 live-verify'da orchestrator alacak; sen kod+compile+data-proof ver.

## Rapor (done dosyası, ≤10 satır)
VERDICT + root-cause (gerçek koda dayalı) + değişen dosyalar + uygulanan çözüm + console durumu (0-error) + kalan risk.
