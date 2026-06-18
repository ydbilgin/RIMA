# DEMO HIGH-4 FIX BATCH SPEC (2026-06-18)

Demo ~yarın, editörde. Bu batch = 4 confirmed demo-killer bug. ROOT-FIX öncelikli, cerrahi.

## ACTIVE RULES
(1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear — sessizce partial implement etme.

## NLM ACCESS
RIMA design context gerekirse: `NB=$(cat .claude/nlm.local); uvx --from notebooklm-mcp-cli nlm notebook query $NB "<soru>"`. Direct-read sadece: CURRENT_STATUS.md / kod / STAGING / memory.

## UNITY ERROR CHECK
İş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR (silme), raporda console durumunu yaz. Unity instance = `RIMA@ed023e0b` (tek instance). **Sen TEK Unity-ajansın** — başka ajan Unity'ye dokunmuyor.

## GRAPHIFY
Cross-file/mimari soruda önce graphify query (graph.json: `STAGING/_process/2026-06/graphify_fullmap/graphify-out/`), bulk-read'den ~71x ucuz. Ama bu görevde dosyalar zaten pinpoint — direkt oku.

---

## FIX 1 — MOVEMENT OFF-MAP (ROOT, 🔴 demo-killer)
**Sorun:** Dash/teleport skilleri WalkabilityMap kontrolünü bypass ediyor → void'e ışınlanma/strand, irrecoverable.
- `Blink.cs:32-66` — ölü "Wall" raycast + hard teleport, WalkabilityMap check YOK.
- `IronCharge.cs:53` — raw velocity, clamp YOK.
- `BladeRush.cs:40` — raw velocity, clamp YOK.

**Fix (ROOT, mevcut API'yi AYNALA — yeni API uydurma):**
1. ÖNCE `WalkabilityMap.cs`'i oku → `ClampVelocityToWalkable` ve `IsDashableWorld` (veya gerçek isimleri) imzalarını doğrula.
2. ÖNCE `PlayerController.TryDash` + `KnockbackReceiver`'ın bu API'yi NASIL kullandığını oku — aynı deseni uygula.
3. IronCharge + BladeRush: raw velocity → `ClampVelocityToWalkable` üzerinden geçir.
4. Blink (teleport): hedef noktayı `IsDashableWorld` ile doğrula; walkable değilse en yakın walkable noktaya snap'le ya da iptal et (TryDash'in teleport/destination doğrulamasını aynala). Ölü "Wall" raycast'i SİLME, sadece walkability guard ekle (cerrahi).

## FIX 2 — SkillBase SPEND-BEFORE-VETO (ROOT, 🔴, EN RİSKLİ — base class)
**Sorun:** `SkillBase.cs:72-87` — Execute no-op olsa bile (ör. Chain Lightning menzilde düşman yokken) mana+cooldown harcanıyor = ölü buton.
**Fix:**
1. ÖNCE cast pipeline'ı oku (SkillBase.cs tamamı + skillin nasıl invoke edildiği). Mevcut contract'ı anla.
2. Minimal contract değişikliği: `Execute` başarı/başarısızlık döndürsün (bool) VEYA virtual `CanExecute()` (default `true`) ekle. Cost/cooldown SADECE gerçek başarıda düşülsün.
3. **KONSERVATİF:** default davranış = mevcut tüm skiller etkilenmesin (CanExecute default true / Execute default success). Sadece no-op edebilen skiller (range-gated) veto edebilsin.
4. ⚠️ Bu base-class değişikliği TÜM skill subclass'larını kapsar. Tüm subclass'ların compile olduğunu ve davranışının bozulmadığını doğrula. Eğer contract değişikliği skilleri kıracaksa → **BLOCKED yaz**, sessizce yarım bırakma.
5. NOT: "yetersiz-kaynak sessiz no-op feedback" (toast/SFX) AYRI bir MED bug — bu batch'te DEĞİL, ekleme.

## FIX 3 — RunStats PROGRESSION-DESYNC (🔴, meta-tez ekranda çöküyor)
**Sorun:** `_Arena` RoomRunDirector room-clear'ı `RunStats`'a bildirmiyor (köprü yok) → roomsCleared=0, Echo award floored, death/victory ekranı hep "ODA 1".
**Fix:** `RoomRunDirector`'ın room-clear/oda-ilerleme noktasını (`HandleEncounterCleared` / `BuildCurrentRoom` — gerçek metodu oku ve bul) `RunStats`'a notify edecek köprü ekle (roomsCleared artışı). ÖNCE RunStats API'sini oku, mevcut increment metodunu kullan (yeni state uydurma).

## FIX 4 — Boss PHASE-2 BURST-SKIP (🔴, canon ihlali)
**Sorun:** `PenitentSovereign.cs:225-240` — 8s phase-lock yok → burst hasarla Faz-2 atlanıp Faz-3'e geçiliyor; Faz-2 mekaniği hiç görünmüyor.
**Fix:** Faz-3 trigger'ını `Time-since-Faz2 >= 8s` ile gate'le. Faz-2'ye giriş zamanını kaydet; HP eşiği aşılsa bile 8s geçmeden Faz-3'e geçme.

---

## DOĞRULAMA (zorunlu)
1. Dosyaları kaydet → Unity recompile tetikle (`refresh_unity` veya editör focus) → `editor_state.isCompiling` false olana kadar bekle.
2. `read_console` (Error+Warning) → **0 error** hedef. Kendi hatanı çöz.
3. **PLAY ETME / runtime test YAPMA** (stall riski + tek-Unity-ajan). Sadece compile-clean doğrula. Runtime doğrulama orchestrator + auditor'a kalacak.

## ÇIKTI (E1 — token economy)
Detay raporu `STAGING/_process/2026-06/DEMO_HIGH4_FIX_DONE_2026-06-18.md`'ye yaz:
- Her fix için: dokunulan dosya+satır, NE değişti, neden, varsa risk/varsayım.
- SkillBase contract değişikliğinin TAM açıklaması (subclass etkisi).
- Console durumu (error/warning sayısı + içerik).
- BLOCKED varsa açıkça yaz.

Dönüşte (bana) SADECE ≤10 satır: hangi fix DONE / BLOCKED, console durumu, dosya yolu. Rapor içeriğini dönüşe gömme.
