ACTIVE RULES: (1) think before answering (2) concrete file:line (3) flag real issues/regressions (4) UNSURE if can't verify.

# GÖREV — Integration backlog + W2 audio review (code/feasibility)
Sen YAZMIYORSUN — review + düzeltme. İki şey:

## A) Integration backlog: `STAGING/INTEGRATION_BACKLOG_S6.md`
Workflow audit (7 Explore + Opus sentez) üretti. Doğrula + eleştir:
1. **#1 DÜZELTME:** Opus runtime-verify yaptı (play mode): GenerateOffers GERÇEK skill döndürüyor (Gravity Cleave/Earthsplitter/Berserker's Blood) — backlog'un "gold/heal'e düşüyor" iddiası YANLIŞ mekanizma. AMA `PlayerClassManager` + `Warblade_SkillController` player'da YOK → `DraftManager.AssignActive` skill component'i bulamaz → seçilen skill TAKILMIYOR. Bu doğru mu? Warblade.prefab / Player'da Warblade_SkillController gerçekten yok mu? Eklenince EnsureDefaultLoadout skill component'lerini AddComponent ediyor mu (AssignActive bunlara ihtiyaç duyar)? Doğru fix yolu ne?
2. **Ranks 1-5 file:line kanıtlarını DOĞRULA** (VFXRouter.entries boş mu, SlashArcVFX field null mı, HitFlashDriver çağrılmıyor mu, LightPulse subscriber yok mu, boss-death 1-frame race gerçek mi).
3. **Feasibility + sıra:** 1-15 non-gated sırası doğru mu? Gizli-pahalı veya gizli-bağımlı olan? (örn. #8 SkillBar #1+#7'ye bağlı)

## B) W2 audio kodu
- `Assets/Scripts/Audio/AudioManager.cs` (YENİ) — prosedürel synth (Tone/Noise/Sweep + AudioClip.Create/SetData), RuntimeInitializeOnLoadMethod bootstrap, static Play(Sfx), AudioSource. Doğru/leak/null-safe mi?
- Hook'lar: `Health.TakeDamage` (Hit + Death), `DraftManager.FinishPick` (DraftSelect), `Gate.Unlock` (GateOpen). Doğru yerde mi, side-effect/spam riski (Hit her damage'da, player+enemy)?

# ÇIKTI → CODEX_DONE_yekta.md
STATUS: COMPLETED
- #1 doğrulama + doğru fix yolu (file:line)
- Ranks 1-5 kanıt AGREE/DISAGREE
- W2 audio: AGREE/ISSUE
- Kaçırılan bağımlılık/risk
