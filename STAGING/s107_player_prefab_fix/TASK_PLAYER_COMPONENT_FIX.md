# TASK: Warblade Player Prefab — Component Fix + Scene Override Cleanup

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: PlayableArena.unity'deki 11 "Broken text PPtr 1441862778" hatasını kalıcı olarak çöz. Warblade prefab'ında eksik olan 5 gameplay component'ini prefab'a kalıcı ekle ve sahnedeki bozuk added-override'ları temizle. Player WASD/Health/Combat tam çalışır halde kalmalı.

## Problem
`Assets/Scenes/Test/PlayableArena.unity` içinde PrefabInstance `&1441862776` (Warblade Player), prefab guid `51d1b129b77e6f04fbeca10699526625`:

- `m_AddedComponents` listesinde 5 entry (lines 8648-8663):
  - 1441862779 (RIMA.RageSystem)
  - 1441862780 (RIMA.KnockbackReceiver)
  - 1441862781 (RIMA.PlayerAttack)
  - 1441862782 (RIMA.PlayerController)
  - 1441862783 (RIMA.Health)
- Bu 5 MonoBehaviour blok lines 8665-8767 arasında tanımlı, hepsinin `m_GameObject: {fileID: 1441862778}` — bu GameObject scene'de TANIMLI DEĞİL (sadece component'ler stranded).
- Sonuç: 11x "Broken text PPtr" console error.

Warblade prefab (`Assets/Prefabs/Characters/Warblade.prefab`) içinde bu 5 component'in hiçbiri YOK — sadece base graphics + transform var.

## Çözüm Planı

### Adım 1: Warblade prefab'a 5 component'i kalıcı ekle
Warblade prefab variant — base prefab guid `64f340ef3dbef474da6aa6b17fe976f4`. Variant'ın root GameObject'ine 5 MonoBehaviour ekle:

| Component | Script GUID | Default değerler (sahnedeki strand'tan kopyala) |
|---|---|---|
| RIMA.Health | 74b1c4004c6ac85449206c7a9a52561c | maxHP=100, incomingDamageMultiplier=1, healMultiplier=1 |
| RIMA.RageSystem | 351890a7bb286de489ba092f80303d4b | maxRage=100, ragePerHitDealt=1, gainMultiplier=1, ragePerHitTaken=5, ragePerKill=3, decayDelay=1.5, decayPerSecond=10, furyThreshold=50, bloodrageThreshold=80 |
| RIMA.KnockbackReceiver | 2860fe9f3bedbf44d8ec7399934fe436 | knockbackResistance=0 |
| RIMA.PlayerAttack | b9abc5473e54225448f6d57e5e5170b8 | basicAttackProfile=null, slashArcVFX=null, outgoingDamageMultiplier=1, baseDamage=0 |
| RIMA.PlayerController | abe7c6a690aaf7448a29af32917e59db | moveSpeed=4.5, dashSpeed=18, dashDuration=0.15, dashCooldown=0.8, commitmentMoveMult=0.25, combatFacingLockDuration=0.18 |

Approach: UnityMCP `manage_components` (action=add) ile Warblade prefab'ı open et (manage_prefabs open), 5 component'i sırayla ekle, prefab save+close. UnityMCP ile yapmazsan YAML direkt edit yapma — Unity prefab variant property merge'ünü bozar.

### Adım 2: Sahnedeki bozuk override'ları sil
`Assets/Scenes/Test/PlayableArena.unity` YAML'inde:
- Lines 8648-8663: `m_AddedComponents` listesindeki 5 entry'i sil (boş liste bırak: `m_AddedComponents: []`)
- Lines 8665-8767: 5 MonoBehaviour bloğunu (`&1441862779`, `&1441862780`, `&1441862781`, `&1441862782`, `&1441862783`) tamamen sil

Bu işi UnityMCP ile sahneyi açıp prefab instance'ın override'larını "Apply to Prefab" veya "Revert" ile yapma — base prefab'ta zaten yok bunlar, override revert direkt siler. Adım 1'den sonra prefab'tan miras alacaklar.

### Adım 3: Verify
1. `read_console` — 0 Broken PPtr error
2. PlayableArena sceneyi yeniden aç, hierarchy'de Player altında 5 component görün
3. EditorPlay testi YAPMA (player movable mı diye), sadece compile + scene load OK
4. Inline rapor — NOT a file

## Hard Constraints
- AssetDatabase batch wrapper kullan (Unity safety protocol)
- Scene save discipline: değişiklik sonra explicit save
- Console check zorunlu
- BLOCKED durumlar: prefab open edilemiyorsa, component guid'ler mismatched ise → durup raporla
- Surgical: sadece Warblade.prefab + PlayableArena.unity dokun, başka dosya yok

## Beklenen Çıktı (inline)
- Adım 1 sonucu: 5 component eklendi mi, prefab save OK mı
- Adım 2 sonucu: scene YAML satır sayısı eski/yeni, override list temizliği OK mı
- Adım 3: console error count (eski 11 → yeni 0 olmalı)
- Commit yapma — orchestrator user onayı sonra commit edecek
