# QUEUE-10 ROUTING DECISION — 2026-06-05 (gece, otonom)

Council: cx yasinderyabilgin (feasibility/reuse, file:line) ‖ ax-Gemini-3.1-Pro (architecture/sequencing) ‖ ax-Gemini-3.5-Flash (lean/ship-fast) → Opus sentez.
Ham çıktılar: `CODEX_DONE_yasinderyabilgin.md` (ÇOK değerli file:line envanteri) · ax çıktıları transcript'te.

## Konsensus (3/3)
- **T1 Knockdown = en riskli iş.** Solo cx, kimseyle paralel kombat dosyası paylaşmaz. Get-up i-frame OLMADAN ship edilmez (juggle-lock). Smoke-test gate: Chamber combat-dummy ile ağır vuruş → uç→squash→yat→i-frame→kalk→tekrar hasar alır + AI normal.
- **T1, T2, T9 aynı enemy/death/damage hattına dokunur → SERİLEŞTİR** (1 → 2 → 9-kalanı).
- **T6 → T5 sırası** (SkillDatabase kaydı codex'i besler); T5 MVP = kayıtlı sınıflarla başlar.
- **T7 import fix = mekanik, izole**; başka Unity import/scene op ile paralel YAPMA.
- **T10 = SADECE build-settings'ten `_FazMVP_Demo_s99` çıkarma**; dead-code = ayrı read-only audit. `PlayableArena_Test01` AKTİF referanslı — DOKUNMA.

## Çözülen anlaşmazlıklar (Opus kararı)
1. **T1 kapsamı:** Flash "merge'ü ertele, SO'ları kes" dedi; CODEANIM kararı (kullanıcı onaylı) merge + 3 SO der → **CODEANIM uygulanır** AMA boss `KnockbackComponent` invaziv rewrite DEĞİL **compat-forwarding adapter** (cx önerisi) + zorunlu dummy smoke-test. Direct `AddForce` skill'leri (Cleave/IronCharge/WarStomp/BladeRush) v1'de bypass kalabilir — not düşülür, kırılmaz.
2. **ax-Opus-4.6 pilotu:** 3.1 "T5 pilot" dedi; cx "T5 paylaşımlı pause/input = cx'e" dedi → **pilot = T2 ölüm-decal** (S, izole, görsel-doğrulanabilir; cx review). Pilot PASS → T3/T5 review'larında ax-Opus-4.6 kullanılır; FAIL → kanal raporlanır, iş Flash'a düşer. Model string = `Claude Opus 4.6 (Thinking)` (settings.json swap, ax tek-seferde).
3. **T9 üçlüsü:** 3.1 "hepsini ertele", Flash "sadece Mote-Heal", cx "#14 zaten ~var, #17 TANIMSIZ=BLOCKED" → **#14 = data-QA only (XS)** · **#26 DEFER** (T4 ile draft-çakışması + düşük playtest ROI) · **#17 BLOCKED → kullanıcıya soru** (Echo-mote = pickup-heal mi, kart-VFX mi, Echo-currency-heal mi?).
4. **T3 anchor tool:** Flash "tool'u kes, Inspector Vector2 yeter" dedi; ROADMAP kararı tool'u Oturum B önkoşulu yapar → **LEAN tool yapılır** (SceneView handle, `OrientationSync.handOffsets` = source-of-truth, Undo/SetDirty; WeaponDatabaseSO offset'i İKİNCİL — tool yazmaz, sadece okur-gösterir).
5. **T8 kapsamı:** overlay-path ZATEN İMPLEMENTE (cx kanıtı: `IsoRoomBuilder.cs:292-352`) → T8 = checker floor `((x+y)&1)` + 15 Generated SO'ya CompositionRoleMap+BridsonPoisson koşturan editor utility. Yoğunluk: merkez temiz kalır (CompositionRoleMap clean-center pass'i var).

## Final plan — lane'ler ve sıra

**Lane cx-A (solo, combat):** T1 knockdown [M+] → bitince ax-Opus-4.6 mimari review + Opus dummy smoke-test → commit.
**Lane cx-B (paralel profil):** T4 hover (SODAMAN spec: TooltipSystem→CardJuiceHandler + synergy pulse + ink-wash stil) → sonra T5 SkillCodexUI (T6 bitmiş olur).
**Lane ax (tek-seferde sıra):** T6 SkillDatabase 6-sınıf placeholder-kaydı (Flash; draft'a sızmasın = unlock/implemented guard) → T3 lean anchor-tool (Flash) → [T1 bitince] **T2 pilot (Opus 4.6 Thinking)** → review görevleri.
**Lane Sonnet-MCP/Opus (Unity ops, importlar serili):** T7 ikon import fix → T10 build-settings → T8 checker+placer → #14 data-QA.

**Cross-review matrisi (yazar ≠ reviewer):**
| İş | Yazar | Review |
|---|---|---|
| T1 | cx-A | ax-Opus-4.6 + Opus play smoke-test |
| T4 | cx-B | ax-Flash + Opus play-verify |
| T6 | ax-Flash | Opus diff + draft-guard kontrolü |
| T7 | Sonnet-MCP | Opus diff + 1 ikon görsel |
| T10 | Sonnet-MCP | Opus diff |
| T3 | ax-Flash | Sonnet-MCP in-editor |
| T2 | **ax-Opus-4.6 (PİLOT)** | cx |
| T5 | cx-B | Sonnet-MCP in-editor + ax-Flash |
| T8 | Sonnet-MCP | cx diff + Opus play-verify |

**DEFER/BLOCKED:** #26 Card-Weight (defer) · #17 Echo-Mote-Heal (BLOCKED — kullanıcı tanımı gerek) · dead-code sweep (ayrı audit) · T5 blur/desat (v1'de YOK — Flash uyarısı, URP risk; flat panel + activeSelf).

**Stabilite ilkesi:** her lane işi bitince compile-clean + (runtime işlerde) play smoke-test → commit → sonraki. Session yarıda ölürse en değerli işler önce: T1/T4/T7 wave-1'de.
