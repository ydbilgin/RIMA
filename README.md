# RIMA
2D roguelite — Hades tarzi combat, cross-class skill draft, rift dunyasi.
**S43 · Faz 1** — Visual Contract asamasi. Devam icin: `CURRENT_STATUS.md`

---

## Takim

| Rol | Kim | Gorev |
|---|---|---|
| **Orkestra Sefi** | Claude Code (Sonnet 4.6) | Mimari, routing, QC, entegrasyon — tek karar mercii |
| **Tasarim Danismani** | rima-design (Opus 4.7) | Cross-system denge, kimlik kararlari |
| **Mekanik Calistirici** | rima-codex (Sonnet + cx CLI) | Codex CLI uzerinden bounded implementasyon |
| **Dokuman / QC / Asset** | rima-doc · rima-qc · rima-asset · rima-research | Yazim, dogrulama, prompt, web arastirma |
| **Urun Sahibi** | Antigravity | Play test, gorsel degerlendirme, kullanici karari |

**Yonlendirme kurali:**
- Mimari / sistem / QC karari → Claude only
- Cross-system denge / kimlik → rima-design (Opus)
- Sinirli mekanik is → rima-codex
- Dok yaz / arsivle → rima-doc
- Dogrula / lint → rima-qc
- PixelLab / Gemini prompt → rima-asset
- Dis arastirma → rima-research

Detay: `AGENTS.md`

---

## Mevcut Durum (S43)

- **10 sinif, 192 skill** — v2 audit kilitledi (2026-04-30)
- **Production gate HOLD** — Visual Contract template bekleniyor
- **PixelLab:** 2586/5000 gen kullanildi, son tarih 2026-05-18
- **Testler:** 24/24 PASS

---

## Tamamlanan Sistemler

**Core:**
- Health, RageSystem, StatusEffectSystem
- PlayerController (WASD + 8 yon + dash), PlayerAttack (3-hit combo)
- EnemyTier, EnemyAnimator, ShardWalker AI, HUDManager, SkillDraft

**Kaynak Sistemi:**
- PlayerResourceBase, RageSystem, ManaSystem, EnergySystem, ComboPointSystem, FocusSystem
- PlayerProjectile, DamageZone

**Skills (10 sinif × ~12 skill = 192 skill, v2 audit):**
- Warblade · Elementalist · Shadowblade · Ranger · Ravager
- Ronin · Gunslinger · Brawler · Summoner · Hexer

**Mob AI:**
- Penitent_AntiHealAura, VoidThrall_DeathSplit
- ChainWarden, RelicCaster, FractureImp
- Boss: PenitentSovereign, HollowMite, TheWound

---

## Sirada

1. Visual Contract template (`TASARIM/SKILL_VISUAL_CONTRACT.md`)
2. Top-4 sinif kontratlari (Brawler, Shadowblade, Ravager, Ranger)
3. Unity state overlay spec
4. Brawler char anchor → V3 keyframe workflow

---

## Okuma Haritasi

| Dosya | Ne icin |
|---|---|
| `CURRENT_STATUS.md` | Her session basi — devam noktasi |
| `AGENTS.md` | Routing, model secimi, context kurallari |
| `CODEX.md` | Codex gorev kurallari |
| `TASARIM/GDD.md` | Oyun tasarimi hakikati |
| `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` | Sinif + skill kararlari (v2 canonical) |
| `TASARIM/MASTER_KARAR_BELGESI.md` | Kilitli sistem kararlari (#54-#58) |
| `SYSTEM_MAP.md` | Mimari harita |
| `MEMORY/INDEX.md` | Agent bellek indeksi |
