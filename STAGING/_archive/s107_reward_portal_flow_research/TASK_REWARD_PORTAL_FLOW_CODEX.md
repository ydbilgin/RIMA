# TASK: RIMA Reward + Portal Flow — Deep Code/Architecture Reference

ACTIVE RULES: (1) think before reviewing (2) min response, no speculation (3) cite specific games/codepaths (4) BLOCKED if uncertain.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: RIMA için Reward + Portal Flow kararı verilirken Unity ARPG/roguelite genre'ında **kod mimarisi açısından** somut karşılaştırma. Hangi pattern hangi kod kompleksitesi gerektiriyor + hangi Unity pattern'leri ölçeklenir.

Çıktı **inline** (dosya YOK), 600-1000 kelime.

## RIMA Context
- Engine: Unity 6 + URP 17.3
- Game type: Top-down 3/4 roguelite ARPG (Hades/CoM/D3 ref)
- 8 sınıf roster v2, Warblade + Elementalist starter
- Wall-less V1 Hades Elysium LOCK
- Mevcut mimari: ScriptableObject-heavy (RoomBackgroundRig, CliffPlacementRules, DeterministicVariantTile)
- Karpathy 4 — min code, surgical, no speculation

## Görev — 4 bölüm

### Bölüm 1: 5 Pattern Architecture Comparison
| # | Pattern | RIMA için Unity API/pattern | LoC tahmin | Risk |
|---|---|---|---|---|
| A | Lineer reward + 1 portal | RoomController + RewardSpawner + simple state | ~150 | yok |
| B | 3 portal door choice | RoomGraph + PortalChoice UI + RoomTypePool | ~400 | path planning UI |
| C | 3 boon choice + 1 portal | BoonPool + BoonChoiceUI + ClassBoonRegistry | ~500 | per-class boon variety |
| D | 3 portal+preview combo (Slay-Spire path) | RoomGraph node preview + multi-attr UI | ~700 | UI complex |
| E | 2 reward + 2 portal (tied) | RewardPortalPair scriptable + selection logic | ~350 | doc/UX |

Her satır için:
- Unity ScriptableObject vs MonoBehaviour mimari önerisi
- State machine veya event-driven? (UniRx/Cysharp/native UnityEvent)
- Pool/registry yapısı (sınıfa göre boon, oda türüne göre reward)
- Data-driven olabilir mi (designer ScriptableObject editing vs hardcoded)

### Bölüm 2: Hades / Dead Cells / Slay the Spire — Kod Yaklaşımları
Public/leak/dev talk'lardan elde edilebilen mimari ipuçları:
- Hades — Sjson scripting + boon registry + RoomManager (referans bilinen)
- Dead Cells — Haxe + scroll/cell/item modular system
- Slay the Spire — Java + MapNode/PathGenerator (oldukça documented)
- RIMA'ya **hangi mimari adapte edilebilir**? Açıkça söyle.

### Bölüm 3: Karpathy 4 ile Genişleme Stratejisi
Eğer önce A (basit) yapılırsa, sonra C → B/D'ye nasıl **breaking change OLMADAN** geçilir?
- Hangi ScriptableObject interface'leri başlangıçta tanımlamalı
- RewardProvider, PortalDestinationProvider, ChoiceUI gibi abstraction layer'lar gerekli mi yoksa premature?
- "MVP A + extension hook" vs "C'yi direkt MVP yap" — hangisi daha az toplam kod?

### Bölüm 4: Verdict — Kod Açısından Öneri
- En önerilen pattern (1 adet)
- MVP scope (hangi MVP → hangi v2 yapılır)
- Implementation order (önce hangi ScriptableObject, sonra hangi system)
- Risk (hangi kararı bozmamak lazım downstream)

## Hard constraints
- Inline only — dosya YAZMA
- Speculative chaining yasak — net karar, BLOCKED'sa söyle
- Unity API isimlerini doğru ver (ScriptableObject, UnityEvent, SerializeReference, etc.)
- Karpathy 4 — min code önerisi her noktada

## Codex effort
**high** (xhigh DEĞIL — quota tasarrufu, user 3→1 Codex hesabı düşüyor)
