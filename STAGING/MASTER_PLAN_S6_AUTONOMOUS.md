# MASTER PLAN — S6 Overnight Autonomous (2026-05-30, Opus 4.8 lead, user AWAY ~10h)

> **PICKUP (yeni session ÖNCE BUNU OKU):** Bu dosya = gece otonom çalışmanın TEK kanonik roadmap'i.
> Sıra: `.claude/PROJECT_RULES.md` → `CURRENT_STATUS.md` (en üst overnight blok) → BU DOSYA → `STAGING/DESIGN_LOCK_S6.md` (sentez bitince).

## Direktif + kurallar (kullanıcı, 2026-05-30)
- **Opus TEK karar verici. SORMA, devam et.** ax+cx = danışman (review/fikir), writer DEĞİL ([[feedback-opus-decides-codex-agy-review-s6]]).
- **Kod yazan ≠ review eden** ([[feedback-code-writer-rotation]]). Quota'ya göre writer rotasyonu (Codex/Sonnet/Opus).
- **Placeholder politikası:** her şeyi placeholder'la kur, AMA aşağıdaki PLACEHOLDER REGISTRY'de "yerine ne gelecek + kaynak" not et.
- **Audio = ERTELE** (listede). Üretim sonra: **Sora (ChatGPT Plus) + Gemini Pro**.
- **Çelişki YOK.** Havada-yüzen-ada setting'ine uygun tutarlı hikâye + mantıklı ışık (gaz lambası SAÇMA).
- **Workflow serbest. NLM context için. Status+memory güncel tut** (continuity).
- Dil: kullanıcı Türkçe / agent İngilizce ([[feedback-language-turkish-user-english-agents]]). Commit: ydbilgin, Claude-trailer yok; **push BLOCKED**.

## Quota snapshot (2026-05-30 ~02:1x) + routing
| Kaynak | Durum | Kullanım |
|---|---|---|
| **cx Codex = yekta** | week %14 (sağlıklı; diğerleri %90-97 DOLU) | kod-write (mekanik/bulk), `cx_dispatch.py --profile yekta` |
| **ax Antigravity** | 5 hesap ~%100 boş | review + research + design lens (`agy_dispatch.py`) |
| **Opus (ben)** | self-monitor | karar + zor multi-system kod + sentez + kritik review |
| **Sonnet** | bol (Max) | mekanik write alternatifi (writer≠reviewer için) |
**Writer→Reviewer matrisi:** mekanik = cx-yekta write → agy review · zor algo = Opus write → cx/agy review · multi-system = Opus write → cx+agy review. Reviewer DAİMA writer'dan farklı.

## In-flight (bu blok yazılırken background)
- `wf_b87f702d` (Workflow rima-design-lock) — 5 boyut araştırma + sentez → DESIGN_LOCK draft döner.
- `bqnebiyoq` (cx-yekta) — DESIGN_CONSULT_CX (kod-fizibilite) → `CODEX_DONE_yekta.md`.
- `bkoqakmuu` (agy) — DESIGN_CONSULT_AX (oyuncu/estetik) → `AGY_DONE_<acct>.md`.
- **Bitince:** Opus sentez → `STAGING/DESIGN_LOCK_S6.md` yaz → PHASE 2 başlasın.

---

## SIRALI PLAN

### PHASE 0 — DESIGN LOCK (in flight) → `DESIGN_LOCK_S6.md`
4-kaynak converge (workflow + cx + agy + NLM) → Opus sentez. Çıktı: kilitli hikâye / ışık / harita / oyun-ekranları / game-feel + PLACEHOLDER REGISTRY + çelişki-çözüm. **PHASE 2 buna bağımlı; PHASE 1 değil.**

### PHASE 1 — OTONOM-GÜVENLİ KOD (design-independent, HEMEN, Unity AÇIK)
| # | İş | Writer | Reviewer | Not |
|---|---|---|---|---|
| 1.1 | Demo E2E hardening: boss-death→class-select **bypass** (PenitentSovereign.cs:562-572) + **death-screen scale-0 fix** (scene DeathScreenCanvas localScale 0) | cx-yekta | agy+Opus | softlock + ölüm ekranı blocker |
| 1.2 | **VFXRouter.entries doldur** (scene) — wired slash/flash/spark gerçekten ateşlensin | Opus | cx | cx-flag entries:[] |
| 1.3 | **Weapon Player-wiring**: Warblade.prefab + HandAnchorAttach/OrientationSync + WeaponDatabase + greatsword 31ee0f73 import | cx-yekta | Opus | cx'in bulduğu gizli blocker |
| 1.4 | Game-feel: input buffer + coyote time (PlayerController) + directional camera impulse (CameraPunch) + hitstop tune (normal0.04/crit0.06/finisher0.10) | cx/Sonnet | Opus | agy game-feel punch-list |
| 1.5 | **Victory Wishlist CTA** scaffold: run-stat + `steam://openurl` placeholder + share-seed + next-class teaser-silhouette placeholder | Opus | cx | dönüşüm noktası |
| 1.6 | Audio: Resources/Audio loader + eksik event hook (prosedürel fallback KALIR) | cx-yekta | agy | CLIP'ler ertelendi |

### PHASE 2 — DESIGN-LOCKED BUILD (DESIGN_LOCK_S6.md sonrası)
| # | İş | Not |
|---|---|---|
| 2.1 | **Lighting rework** — kilitli ışık konsepti (cyan-rift/rune/glow, gaz-lamba DEĞİL) → URP 2D Light rig | floating-island mantığı |
| 2.2 | **Story integration** — oda-başı cyan monolog + boss intro line + lore threading | tutarlı hikâye |
| 2.3 | **Connected-rooms** — kilitli bağlantı yaklaşımı (landmark/bridge/transition tutarlılığı) | "güzel+tutarlı map" |
| 2.4 | **Cheap map UI** — "Oda X/5" breadcrumb + kapı ödül-önizleme (tam minimap DEĞİL) | |

### PHASE 3 — ART PLACEHOLDER + CODEX IMAGE-GEN
| # | İş | Kaynak |
|---|---|---|
| 3.1 | Game-screen görselleri (menu/death/victory bg + teaser silhouette) — kilitli boyutlarda | **Codex image-gen** (sonra PixelLab) |
| 3.2 | **Mob archive-restore** — no-sprite placeholder'ları 0-gen arşiv sprite'la değiştir | `ARCHIVE/Sprites_Enemies_old/` (otonom) |
| 3.3 | Boss graybox (mekanik) | gerçek art = gated (PixelLab/RTX-local) |

### PHASE 4 — ERTELENEN (listede, otonom DEĞİL)
- **Audio** müzik+SFX → Sora (ChatGPT Plus) + Gemini Pro.
- Tam minimap + fog-of-war · T3 live-editor entegrasyonu · gerçek boss/weapon art (PixelLab/RTX-local) · **git-push** (remote divergence, kullanıcı kararı).

---

## PLACEHOLDER REGISTRY
| Placeholder (şimdi) | Yerine gelecek | Kaynak | Otonom? |
|---|---|---|---|
| Mob renkli-kare/no-sprite | arşiv mob sprite | `ARCHIVE/Sprites_Enemies_old/` | ✅ |
| Boss no-sprite (graybox) | gerçek boss pixel-art | PixelLab / RTX-local | ❌ gated |
| Game-screen bg/silhouette | Codex image-gen → sonra PixelLab | Codex `$imagegen` | ✅ (boyut DESIGN_LOCK'tan) |
| Audio (prosedürel SFX) | gerçek müzik+SFX | Sora + Gemini Pro | ❌ ertelendi |
| Steam wishlist URL | gerçek appid `steam://openurl/<appid>` | kullanıcı | ❌ |
| Weapon (Warblade) | cyan greatsword 31ee0f73 (var) / diğer sınıflar | PixelLab | kısmen ✅ |

## ⚠️ UNITY VERIFY NOTU (gece, ÖNEMLİ)
UnityMCP `read_console`/play-mode çağrıları bu gece **kararsız** (timeout; `refresh_unity` çalışıyor ama console hang). **MCP-bağımsız compile-verify kanalı = Unity Editor.log:** `C:/Users/ydbil/AppData/Local/Unity/Editor/Editor.log` → `tail -8000 | grep "error CS"` (boş = temiz). Mevcut compile **TEMİZ** doğrulandı (son 8000 satır error-free; Health.cs RIMA.Audio CS0234 transient'ti, aynı assembly RIMA.Runtime). **Play-verify (F5/A5) = kullanıcı gate'i**, otonom değil. cx scene `.unity` YAML'ı doğrudan düzenleyebiliyor (Batch A temiz çıktı, `.gitattributes *.unity binary` → `git diff --text` ile bak).

## İLERLEME LOG (her item bitince güncelle)
- ✅ **PHASE 0 design-lock** → `DESIGN_LOCK_DEMO_S6.md` RATIFIED (§9 Opus kararları).
- ✅ **PHASE 1 Batch A** (cx-yekta `bw95dfno4`): A1 boss class-select bypass (PenitentSovereign.cs:75,565 `suppressClassSelectOnDeath=true` guard) · A2 death-screen scale {0}→{1} + DeathScreenManager refs wired + EnsureVisibleScale/FindChildRecursive helpers · A3 VFXRouter.entries 4 tag (HitSpark/DeathBurst/ShadowSilhouette/HandGlowVFX, lifetime=2 → §5'e göre tune-pending 0.5/0.8/0.4). **File-review PASS + compile-clean (Editor.log).** Play-verify=F5 pending.
- ✅ **PHASE 1 Batch B** (cx-yekta `bh3ov28w3`): juice tuning — hitstop tiers 0.04/0.07/0.12(/0.20 boss=TODO no-event) ICD0.05 + finisher→crit tier · camera kick `-hitDirection` (0.08/0.16/0.22 decay6) · ScreenShakeDriver→singleton+CurrentOffset+60/40 directional, transform-write KALDIRILDI · CameraFollow 3 offset null-guard ADD · HitStop.cs [Obsolete]. **File-review PASS + Editor.log compile-CLEAN (03:27).** Not: CameraShake + ScreenShakeDriver ikisi sahnedeyse çift-shake → F5 tune.
- ✅ **PHASE 1 Batch C** (cx-yekta `b4fjyqttb`): C1 attack input-buffer (PlayerAttack.cs:194/228 + InputBufferService RequestAttack) · C2 dash cliff-grace 0.10s (PlayerController RefreshDashableOrigin/HasDashableOriginGrace, void-dash engellenmiş) · C3 skill-hit parity — **tüm sınıf skill'leri** (Elementalist/Ranger/Shadowblade/Warblade) `SkillRuntime.DealDamage(...,this)` paylaşılan wrapper → OnHit publish (PlayerProjectile/DamageZone dahil), basic-attack double-publish YOK. **`dotnet build RIMA.Runtime.csproj` PASS 0-err + Editor.log clean (03:39).** Not: Assembly-CSharp.csproj stale (CliffPlacementRules taşınmış, Unity etkilenmiyor).
- ✅ **PHASE 1 Batch D** (cx-yekta `bmofznysj`): Victory (DemoCompleteOverlay self-build: run-stat + gold panel + cyan Wishlist CTA `steam://openurl/` placeholder url + teaser + MENU/PLAY AGAIN) + Death (DeathScreenManager: rotating non-blaming canon copy + Wishlist CTA + Copy-Build-Seed + TRY AGAIN + teaser, self-build, Batch-A scale guard korundu). **`dotnet build RIMA.Runtime` 0-err.** ⚠️ steamWishlistUrl = placeholder `app/0/` (kullanıcı appid girecek). **= PHASE 1 KOD TAMAM (A+B+C+D).**
- ✅ **PHASE 2 Batch E** (cx-yekta `b6k7gc2uf`): RoomMonologController (new, RuntimeInitializeOnLoad self-build typewriter 30cps/3s/fade, #48E0FF) hooks RoomLoader.OnRoomChanged → R2/R3/R4/R5 §1.5 replikleri + R5 boss title-card; `Say(string)` static; PenitentSovereign.cs phase-2=33% HP + "Discipline breaks before the chain does." **`dotnet build RIMA.Runtime` 0-err.**
- 🔨 **PHASE 3 Batch F** (cx-yekta `btl12mbws`, in flight): AudioManager Resources/Audio override loader (gerçek klip drop-in zero-code, prosedürel fallback) + opsiyonel BGM + Dash/Shatter/finisher-accent hook'ları. Pure .cs. (CLIP içeriği = Sora+Gemini, ertelendi.)
- ✅ **PHASE 3 Batch F** (cx-yekta `btl12mbws`): AudioManager Resources/Audio override loader (gerçek klip drop-in zero-code) + opsiyonel music_demo BGM + Sfx.Finisher + Dash/Finisher/Shatter hook. **`dotnet build RIMA.Runtime` 0-err.** CLIP içeriği = Sora+Gemini ertelendi.
- ✅ **Docs:** `IMAGEGEN_PACK_S6.md` (ekran asset prompt+px+path) · `SCENE_WIRING_RUNBOOK_S6.md` (gated iş adım-adım) · `imagegen_probe/test.png` (cx image_gen ✅ DOĞRULANDI — gerçek 64×64 on-brand cyan PNG → ekran görselleri OTONOM üretilebilir, PixelLab upgrade sonra).
- ✅ **Batch G** (cx-yekta `bq82fgqku`): RunStats singleton (kills via CombatEventBus.OnKill deduped / time / rooms / build via PlayerClassManager+skill-slots) + Build-Seed (RIMA-WB-IC-GC-SM-ESx4) + Victory/Death CTA gerçek veri + Copy-Build-Seed clipboard. **`dotnet build` 0-err/0-warn.**
- ✅ **CHECKPOINT COMMIT `698bcec0`** (Yasin Derya Bilgin, Claude-trailer YOK, 55 dosya/1845+): PHASE1 A-G + story + audio + 4 design doc. Gece işi git-güvende. **Push BLOCKED** (remote divergence). `git reset --soft HEAD~1` geri alır.
- ⚠️ **Unity import notu:** UnityMCP `refresh_unity` "recovered" diyor ama AslAslında import ETMİYOR (bridge degraded) → yeni .cs'leri Unity henüz reimport etmedi (RoomMonologController.cs.meta elle yazıldı). **Kod doğru** (cx `dotnet build RIMA.Runtime` 0-err). Kullanıcı dönünce Unity restart → otomatik reimport+compile.
- ❌ **Batch H** (cx-yekta `b7skdpe9i`) FAILED: (a) **cx_dispatch.py encoding bug** — `print(result)` cp1254'te U+2192 (→) crash → exit 1 + boş DONE. **FIXED** (cx_dispatch.py'ye stdout utf-8 reconfigure eklendi). (b) cx batch'te 4 image'i kaydetmedi (probe'da tek image çalışmıştı; batch'te değil). NOT: `Resources/UI/RIMA/` zaten 4-Mayıs mevcut UI asset'leri içeriyor (MenuDungeonBackground/frames/node-ikonları) — demo artless değil.
- ❌ **Batch H-retry** (`b2tymsw2d`) + **Batch I impact-frame** (`bcm68ngfm`): cx ÇIKTI ÜRETMEDİ (image kaydetmedi / ImpactFrameDriver.cs yaratmadı). Kök: **yekta 5h-window BLOCKED** (%100, reset 07:05; ~10 batch yaktı). cx image-gen ayrıca headless'ta güvenilmez.
- 🌅 **WIND-DOWN (04:2x):** yüksek-değerli otonom iş BİTTİ. Kalan = scene-rig/art/play (hepsi gated) + minor .cs polish (impact-frame — yekta reset'inde veya kullanıcı). **MORNING REPORT = `STAGING/MORNING_REPORT_S6.md`** (kullanıcı dönüşünde ÖNCE bunu oku). Image-gen → PixelLab (`IMAGEGEN_PACK_S6.md`). Scene/weapon/F5 → `SCENE_WIRING_RUNBOOK_S6.md`.
- ✅ **ImpactFrameDriver (Opus-WRITTEN, agy-REVIEWED, commit `a8b47e68`):** cx blocked → Opus yazdı (kullanıcı düzeltmesi: "durma, opusa yazdır"). Self-build cyan→purple flash on crit/finisher+kill, unscaled-time, debounce 0.12, agy nit'leri folded (OnDestroy/SubsystemReset/OnDisable-clear). `dotnet build` 0-err.
- 🔁 **ROUTING DERSİ:** cx rate-limit → **Opus-write + agy-review** (DURMA). cx-yekta 5h-BLOCKED reset **07:05**.
- ❓ **UI/IMAGE durumu (kullanıcı sordu):** UI = KOD seviyesinde self-building (Victory/Death/monolog) ✅; sahne görsel-düzenleme + gerçek frame görselleri = GATED (Unity/F5). Codex image-gen = **BAŞARISIZ** (probe 1 görsel yaptı, batch+retry kaydetmedi) → ekran görselleri **ÜRETİLMEDİ**, `IMAGEGEN_PACK_S6.md` PixelLab için hazır.
- ⏭️ **POST-/CLEAR NEXT (Opus-write, cx blocked):** RoomLightingController (per-room mood §2.3, URP Light2D OK) · screen-frame wiring (mevcut Resources/UI/RIMA stone-frames). yekta 07:05 → cx'e dönülebilir. **Büyük iş gated** (Unity restart + PixelLab + F5 + push).
