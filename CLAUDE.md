# RIMA — Claude Code Talimatları
Working dir: `F:/Antigravity Projeler/2d roguelite/RIMA/`. `../` yasak.

## Session Başı
`CURRENT_STATUS.md` oku, kaldığı yerden devam. Kullanıcı hatırlatmaz.

## Memory
Private: `C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/MEMORY.md` → ilgili `*.md` aç.
Başka AI isen: repo `.md` + bu klasörü tara.

## Klasörler
`Assets/` · `TASARIM/` · `GUIDES/` · `CONCEPT_ART/` · `_ARCHIVE/` · `_STAGING/`
Root: CLAUDE.md, CURRENT_STATUS.md, AGENTS.md, README.md, Unity files. Yeni doc → GUIDES/ veya TASARIM/.

## Temizlik
Bitmiş `CODEX_*.md` → `_ARCHIVE/CODEX_TAMAMLANDI/` · Geçici → `_ARCHIVE/` · Silinecek → başına `# [ARCHIVE]`

## Rol
**Claude:** tasarım, mimari, debug, karar, QC yargısı.
**Codex:** mekanik+analitik (sprite import, anim, prefab, SO, izole C#, dosya, doc, review).
**Delegation Gate (tümü EVET → Codex):** Deterministik · Mekanik · İzole · Net sınırlı

### Agent Routing (detay: `AGENTS.md`)
| İş | Agent |
|---|---|
| doc/memory/arşiv | rima-doc |
| QC/lint/görsel review | rima-qc |
| Prompt üretimi | rima-asset |
| Tasarım kararı | rima-design (Opus 4.7) |
| Prompt'a sığan üretim | general-purpose |
| 1-2 satır edit | Claude |

Claude üretim/review/prompt yazmaz — orkestra eder.
Token-first · State (spawn öncesi kaydet) · Scope kayması → Claude'a eskalat

## /clear
Review PASS · yeni faz · 20+ mesaj · ağır batch → "/clear at"

## Test
NUnit + Unity Test Runner + MCP `run_tests`. Sonnet yaz+koş+düzelt.
- EditMode: `Awake()` yok → explicit init (`SetMaxHP(100)`)
- Seeded: `Random.InitState(42)` SetUp'ta
- DungeonGraph: `Is.InRange(12,14)` · Coroutine/Singleton → PlayMode

## Sprite/Asset (S43)
128px canvas · Create from Style Reference PRO · MCP `create_character` YASAK (kredi)
User PixelLab UI üretir → Claude prompt+style ref verir → Codex import → rima-qc → Claude karar
Detay: `_STAGING/PROMPTS_S43/PRODUCTION_GUIDE_S43.md` · `memory/feedback_pixellab_create_character_workflow.md`

## Token Tasarrufu
1. Session başı: `CURRENT_STATUS.md` + `SYSTEM_MAP.md`. Kaynak dosya açma.
2. Edit: sadece ilgili satır aralığı.
3. Yapısal değişiklik → SYSTEM_MAP güncelle.
4. Analiz/cleanup → Gemma (`gemma4:e4b`) veya Codex; silme → PowerShell direkt.
**Compact default:** tüm internal .md compact. Guide hariç.
**/lint:** faz geçişi · 5+ karar · asset öncesi · tutarsızlık şüphesi

## Model
Sonnet 4.6 default. Opus 4.7: multi-system trade-off. Claude yönetir.

## Dil
Kullanıcı: Türkçe · CODEX_*.md: İngilizce · PixelLab prompt: İngilizce · Kod: İngilizce

## Dosya Haritası
**Her session:** `CURRENT_STATUS.md` · `SYSTEM_MAP.md` · `AGENTS.md`
**Gerektikçe:** `TASARIM/STYLE_BIBLE.md` · `TASARIM/GDD.md` · `MASTER_KARAR_BELGESI.md` · `ROOM_MECHANICS.md` · `SINIF_VE_SKILL_KARAR_BELGESI.md` · `COMBAT_ROSTER.md` · `BOSS_DESIGN.md`
**Faz scope:** `TASARIM/FAZLAR/FAZ_MASTER.md` → aktif faz dosyası
**PixelLab ref** (`F:/Antigravity Projeler/Pixellab/`): `PIXELLAB_PIPELINE.md` · `PIXELLAB_API_V2.md` · `AGENTS.md`

## Proje
Unity 2D URP · Namespace RIMA · Sahne `Assets/Scenes/_IsoGame.unity`
**S43:** Karakter canvas 128² · Zemin 64×32 · Duvar 64×96 · PPU=64
Ton: Fractured Epic (Hero Siege/Diablo kompakt, ne chibi ne portrait)

## Interaksiyon (G tuşu)
- RewardPickup: proximity→`[G]`→G→DraftManager.ShowDraft→Destroy
- MapFragment: proximity→`[G]`→G→DungeonGraph.RevealAhead→Destroy
- Kapı: hemen açılır; ödül alınmamışsa HUD reminder
- World prompt: WorldSpace Canvas · sortingOrder=20 · scale=0.012 · interactRadius 1.5-1.8

## Inspector (SYSTEM_MAP.md → detay)
RuntimeRoomManager: bossPrefab · hud(HUDController) · playerTransform · rewardPickupPrefab · mapFragmentPrefab
