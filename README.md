# RIMA
2D roguelite — Hades tarzı combat, cross-class skill draft, rift dünyası.
**Faz 1** — play test aşaması. Devam için: `CURRENT_STATUS.md`

---

## Takım

| Rol | Kim | Otorite |
|---|---|---|
| **Tek Karar Mercii** | Claude Code | Mimari, routing, QC, script, entegrasyon |
| **Deterministik Çalıştırıcı** | Kiro (Sonnet) | MECHANICAL_EXECUTION — LOW-risk, adımlar net |
| **İzole Kod Uzmanı** | Codex | ISOLATED_CODE — utility, bounded implementasyon |
| **Hazırlık Desteği** | Ollama Local (RTX 5080) | LOCAL_RESEARCH — log analizi, RAG, ilk geçiş |
| **Ürün Sahibi** | Antigravity | Play test, görsel değerlendirme, USER_DECISION |

**Temel kural:**
- CRITICAL_DECISION / ARCHITECTURE / QC → Claude only
- MECHANICAL_EXECUTION → Kiro (LOW-risk, adımlar explicit)
- ISOLATED_CODE → Codex (Claude scope belirler + doğrular)
- LOCAL_RESEARCH → Ollama (Claude karar verir)
- USER_DECISION → Antigravity'ye sor

---

## Görev Yönlendirme

```
Mimari / sistem tasarımı?                   → ARCHITECTURE  → Claude only
Root cause analizi, QC yargısı?             → CRITICAL_DECISION / QC → Claude only
Bounded utility script / converter?          → ISOLATED_CODE → Codex (Claude doğrular)
PixelLab API çağrısı + dosya kaydetme?       → MECHANICAL_EXECUTION → Kiro
Unity MCP amele (texture/prefab/animation)?  → MECHANICAL_EXECUTION → Kiro
Log analizi / döküman clustering / RAG?      → LOCAL_RESEARCH → Ollama
Play test, görsel kalite, estetik karar?     → USER_DECISION → Antigravity
```

**Delegation Gate:** deterministik + mekanik + izole + net sınırlı + mekanik doğrulanabilir?
→ Tümü EVET → devret | Herhangi biri HAYIR → Claude yapar

---

## Tamamlanan Sistemler

**Core:**
- Health (OnDamageTaken + SetMaxHP), RageSystem, StatusEffectSystem
- PlayerController (WASD + 8 yön + dash), PlayerAttack (3-hit combo)
- EnemyTier, EnemyAnimator, ShardWalker AI, HUDManager, SkillDraft sistemi

**Kaynak Sistemi:**
- PlayerResourceBase, RageSystem, ManaSystem, EnergySystem, ComboPointSystem, FocusSystem
- PlayerProjectile, DamageZone (shared skill helpers)

**Skills (4/4 class × 12 skill = 48 skill):**
- Warblade: 12/12 ✅ | Elementalist: 12/12 ✅ | Shadowblade: 12/12 ✅ | Ranger: 12/12 ✅

**Mob AI:**
- Penitent_AntiHealAura, VoidThrall_DeathSplit
- ChainWarden, RelicCaster, FractureImp → mevcut MobAttack_* ile hazır

**PixelLab (tamamlandı):**
- 9 karakter + 6 enemy prefab, 9 AnimatorController
- Act1 Test Room (20×15 tile), Floor + Wall tilemap

---

## Sırada

- **Play test** — 4 class + mob + tilemap + animasyonlar (`_Sandbox.unity`)
- Faz 2 planlama (backlog: `../memory/project_rima_backlog.md`)

---

## Okuma Haritası

| Dosya | Ne için |
|---|---|
| `CURRENT_STATUS.md` (RIMA/) | Her session başı — devam noktası |
| `AGENTS.md` | Routing, escalation, ajan detayları |
| `PIXELLAB.md` | PixelLab production reference |
| `ASSET_MAP.md` | Sprite/prefab/animator yolları + araç kararları (aktif) |
| `KIRO_TEMPLATE.md` | Kiro görevi şablonu |
| `TASARIM/GDD.md` | Oyun tasarımı hakikati |
| `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md` | Sınıf + skill kararları |
| `GELISTIRME_PLANI.md` | Faz roadmap |

---

## Proje Dizini

```
2d roguelite/
├── CLAUDE.md (RIMA/ içinde)    ← Session başı otomatik yüklenir
├── README.md                   ← Bu dosya
├── CURRENT_STATUS.md (RIMA/)   ← Aktif durum
├── AGENTS.md                   ← Routing + escalation
├── PIXELLAB.md                 ← PixelLab production reference
├── KIRO_TEMPLATE.md            ← Kiro dosyası şablonu (LOW-risk only)
├── KIRO_*.md                   ← Aktif Kiro görevleri (İngilizce)
├── STAGING/                   ← Kiro ham çıktıları
├── TASARIM/                    ← GDD · STYLE_BIBLE · SINIF_VE_SKILL_KARAR_BELGESI
└── RIMA/                       ← Unity projesi
       └── Assets/Scripts/
             ├── Skills/Warblade/      ← 12 skill ✅
             ├── Skills/Elementalist/  ← 12 skill ✅
             ├── Skills/Shadowblade/   ← 12 skill ✅
             ├── Skills/Ranger/        ← 12 skill ✅
             └── Systems/Resources/   ← PlayerResourceBase + 5 kaynak sistemi
```
