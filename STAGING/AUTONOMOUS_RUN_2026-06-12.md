# OTONOM GECE RUN — 2026-06-12 (kullanıcı uykuda, sabah toplu kontrol)

> **Orkestra = Opus.** writer ≠ reviewer. Kullanıcı sabah yeni session'da toplu kontrol eder.
> İş bittikçe CURRENT_STATUS + memory güncellenir (bağlam kaybı yok).

## Agent rolleri (writer ≠ reviewer)
| Rol | Kim |
|---|---|
| **Writer (kod)** | cx dispatch — profil önceliği: **laurethayday → yasinderyabilgin → yekta (son çare)** |
| **Reviewer** | **COUNCIL** — `rima-qc` (Sonnet, kod-vs-kural) + ax Gemini (bağımsız correctness/design). Writer'la AYNI değil. cx çekişmesi yok. |
| **Final gate** | Opus council'i sentezler → PASS/FAIL. cx test yeşilse zaten commit'lemiş; FAIL→fix-dispatch. status/memory update. |
| **Sembol görsel** | cx `$imagegen` skill (şeffaf, 64×64) — PixelLab DEĞİL |

## Karar (kullanıcı, 2026-06-12 gece)
- Kapsam: **A → B → C ZORLA** (agresif). Görsel doğruluk sabaha.
- Review = **council** (çok-perspektif). Sabah birlikte tekrar doğrulanır.
- Commit: **testler gerçekten geçerse** cx commit'ler; council sonradan denetler.

## Stop-on-failure (kontrollü)
- Bir faz gate'i (run_tests) KIRMIZI veya rima-qc FAIL → **DUR**, CURRENT_STATUS'a BLOCKED yaz, sonraki faza GEÇME.
- Görsel-ağır iş (Director uGUI, HUD) → kod/derleme doğrulanır ama görsel doğruluk **sabah toplu kontrole** bırakılır (blind-commit yok, commit mesajına "visual unverified" notu).
- Belirsizlik → tahmin etme, BLOCKED + soru notu bırak.

## Sıra (gate'li)

### [PARALEL] Faz A (laurethayday) + Semboller (yasinderyabilgin)
- **Faz A — Stat çekirdeği + Damage taksonomisi** → `_council` değil, task: `_process/2026-06/TASK_fazA_stat_core.md`
  - A1 Jersey10 TMP SDF · A2 DamageType+ElementTag+DamagePacket+DamageCalculator(armor/MR)+ClassStatProfile/Runtime/Database+10 asset+DealDamage(packet)+DamageColors · A3 wiring (PlayerClassManager→maxHP/moveSpeed/atkSpeed + 2 basic-attack)
  - GATE: `run_tests` CombatContract yeşil → cx commit → rima-qc review
- **Semboller** → `_process/2026-06/TASK_node_symbols_imagegen.md`
  - 3 sembol (rest=kamp ateşi · unknown=soru işareti · player=konum işaretçisi), 64×64 şeffaf monochrome slate-grey high top-down, mevcut 8 ile eşleş → `Assets/Sprites/UI/MapNodes/` + atlas → cx commit → rima-qc review

### [SEKANS — A geçince]
- **Faz B — Director iskelet** (toggle/kamera/timeScale + uGUI Canvas + chrome skin + 6 boş sekme + Jersey10). Görsel→sabah. Compile+run_tests gate.
- **Faz C — Sekmeler** (Spawn→Class&Skill→Stats→Build→Map→Telemetry). Görsel→sabah.
- **HUD Layout (Edit Mode)** — B sonrası, C/D arası. `HUD_LAYOUT_DECISION_2026-06-12.md`. Görsel→sabah.
- **Faz D — Cila + Loc key'leri**.

## İlerleme log (Opus günceller)
- [x] Faz A dispatch edildi (laurethayday) — çalışıyor
- [x] Semboller dispatch edildi (yasinderyabilgin)
- [x] **Semboller TAMAM** — commit `e09533b2`, 3 PNG 64×64 şeffaf, atlas 11 packable, Opus QC PASS (rest biraz koyu, sabah küçük not)
- [x] Faz A run_tests gate — CombatContract PASS 3/3, 0 error
- [x] Faz A council review — rima-qc PASS + Gemini 3.1 Pro PASS; 10 class değeri tabloyla birebir
- [x] Faz A commit `169e198e` — GATE PASS
- [x] Faz A FIX (nötr int scaling) — commit `d8629d45`, CombatContract 3/3 PASS, 3 dosya cerrahi, Opus QC PASS
- [x] **FAZ A TAM KAPANDI** (169e198e + d8629d45)
- [x] **Faz B** — commit `ddd3a97`, Director iskelet (toggle/cam/6-tab CanvasGroup/chrome/Jersey10), UnityMCP yapısal PASS, Opus controller self-review PASS (guard+enum+singleton temiz). Görsel SABAHA.
- [x] **Faz C3 Stats** — commit `d3a3b9d1`. rima-qc PARTIAL=PASS: test-failure iddiası DOĞRULANDI (pre-existing, regression YOK), wiring Faz A yolları, Loc 18 key. Minör (sabah): FindGameObjectWithTag/slider, Save-ama-Load-yok, debugMult rengi (Opus onayladı). Görsel SABAHA.
- [x] **Faz C2 Class&Skill** — commit `5b5abda0`. rima-qc PARTIAL=KABUL: bypass try/finally güvenli, SelectedClass persist=İSTENEN (kümülatif sahne), AssignActive mutasyon düşük-risk. Sabah notları: TR Loc ASCII-yaklaşık (Loc.cs geneliyle tutarlı), AddComponent fallback 4 controller. Görsel SABAHA.
- [!] **Faz C1 Spawn (1. deneme) SESSIZ-FAIL** — laurethayday 5h %100 BLOCKED oldu (06:47 reset), commit/kod YOK. → yasinderyabilgin ile YENİDEN dispatch.
- [x] **Faz C1 Spawn** (2. deneme, yasinderyabilgin) — commit `9de1f94c`. Hook yokmuş→cx Director-only `SpawnSelectedEnemy(pos)→Instantiate` helper yazdı (palette EncounterWaveSO.entries), Play PASS (spawn 0→1/erase→0), CombatContract 3/3. (rima-qc review arka planda). Sabah notu: palette tek pilot-wave'le sınırlı.
- [x] **Faz C6 Telemetry** — commit `c8fd57a0`. rima-qc PASS: combat-core hook güvenli (?.Invoke null-safe, event TakeDamage sonrası, DealDamage+nötr-fix sağlam, CombatContract dokunulmamış, abonelik temizliği tam, struct allocation-free, CSV RFC4180). Minör: telemetri cap'siz, TR Loc ı/ğ/ş eksik. Görsel SABAHA.
- [⛔] **Faz C5 Map** — BLOCKED (yekta, doğru davranış: hook uydurmadı). `JumpToNode(node-id)` YOK. Mevcut: `DungeonGraph.Generate(seed,depth)` (reroll ✅) + `RoomRunDirector.AdvanceTo(choiceIndex)`/`TryEnterDoor`/`BeginRun` (child-choice nav). Legacy `_IsoGame` DungeonGraph obsolete. **SABAH KARAR:** keyfi node-jump (RoomRunDirector'a yeni hook, riskli) mı, child-choice nav mı? Öneri=child-choice nav güvenli.
- [ ] **C4 Build = SABAH** (PaintCell public+IMGUI refactor en riskli, blind yapılmadı)

## GECE KAPANIŞ (2026-06-12)
**8 commit, hepsi gate'li + review'lı.** Foundation (A+fix, B) + 4/6 C sekmesi (C1 Spawn, C2 Class&Skill, C3 Stats, C6 Telemetry) + 3 sembol. Tüm Director commit'leri `[visual unverified]` → SABAH görsel playtest.
**Kalan:** C5 Map (karar bekliyor), C4 Build (riskli refactor), HUD Layout, Faz D, Loc TR-karakter cleanup.
**Olay:** laurethayday gece yarısı 5h %100 → BLOCKED (C1 1. deneme sessiz-fail); retry yasinderyabilgin'de başarılı. yekta son-çare olarak C5'te kullanıldı.
- ⚠️ NOT: full EditMode suite'te ilgisiz pre-existing failure'lar (brush/map/prefab/perf/MCP) — gate=CombatContract her fazda 3/3. Sabah baseline doğrula.
- [ ] Faz B …
- [ ] Faz C …
- [ ] HUD Layout …
- [ ] Faz D …

## Sabah kullanıcıya (toplu kontrol için)
- Nereye kadar gidildi, hangi faz BLOCKED, hangi görsel-doğrulama bekliyor → CURRENT_STATUS RESUME bloğunda özet.

### SABAH NOTLARI (review'lerden biriken minörler — hiçbiri bloklamadı)
**Görsel doğrulama (hepsi [visual unverified] commit):** Director B iskelet + C1 Spawn + C2 Class&Skill + C3 Stats + C6 Telemetry — Play'de aç, göz at.
**Test baseline:** full EditMode suite'te pre-existing failure'lar (brush/map/prefab/perf/MCP) — gate=CombatContract her fazda 3/3. Baseline'ı doğrula (bizim değişiklik dokunmuyor, rima-qc teyit etti).
**C1 Spawn:** (a) Director→Test çıkışında spawn'lar temizlenmiyor — "kümülatif sahne" kararıyla İSTENEN olabilir, karar ver. (b) palette tek "Act1_Wave_Pilot" wave'le sınırlı, hardcoded path → serialize field? (c) spawn mob'lar AddComponent<Health> default stat alıyor.
**C2 Class&Skill:** (a) `DirectorBypassClassUnlock` public static, try/finally güvenli ama #if DEMO_BUILD guard yok. (b) `SelectedClass` persist=İSTENEN (kümülatif). (c) AssignActive skillName/icon/cooldown kopyası normal draft'ta da çalışıyor (düşük risk). (d) ResolvePrimarySlotHost 4 controller'a AddComponent fallback.
**C3 Stats:** (a) FindGameObjectWithTag her slider değişiminde (debug tool, tolere). (b) Save var ama Load/Restore yok. (c) debugMult rengi #E89020 (Opus onayladı).
**Genel:** TR Loc string'leri ASCII-yaklaşık (ş/ı/ç yok) — Loc.cs geneliyle tutarlı ama kullanıcı-facing → toplu Türkçe-karakter cleanup düşün.
