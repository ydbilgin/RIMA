ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç: EncounterBank + ThreatBudget Sistemi — Dinamik Wave Tetikleyici

Mevcut 7 temel düşman (ShardWalker, VoidThrall, Penitent ve diğerleri) üzerinde çalışacak ThreatBudget tabanlı wave spawn sistemi. Karar #82 ve #84 referans.

## NLM Context Çek

Başlamadan önce şunu sorgula:
```
uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "EncounterBank ThreatBudget wave spawn sistemi detayları, Karar 82 ve 84 içeriği, mevcut enemy script dosya yolları"
```

## Mimari (NLM'den doğrula ve genişlet)

### ScriptableObjects
- `EncounterWaveSO` — wave tanımı: ThreatBudget float, enemy entry listesi (enemy type + count + weight)
- `EncounterBankSO` — room'a assign edilen wave koleksiyonu: EncounterWaveSO[] waves, difficulty curve

### Runtime Scripts
- `ThreatBudget.cs` — MonoBehaviour veya pure class
  - float budget → enemy'lerin "maliyet" toplamına göre spawn kararı
  - Spawn(EncounterWaveSO wave, Transform[] spawnPoints)
- `EncounterController.cs` — MonoBehaviour, room'a attach
  - [SerializeField] EncounterBankSO bank
  - OnRoomEnter() → wave seç, ThreatBudget'a ver
  - OnAllEnemiesDead() → next wave veya room clear event

### File Paths (CREATE)
- `Assets/Scripts/Encounter/ThreatBudget.cs`
- `Assets/Scripts/Encounter/EncounterController.cs`
- `Assets/Scripts/Encounter/EncounterWaveSO.cs` (ScriptableObject)
- `Assets/Scripts/Encounter/EncounterBankSO.cs` (ScriptableObject)
- `Assets/ScriptableObjects/Encounters/Act1_Wave_Pilot.asset` (örnek wave SO)
- `Assets/ScriptableObjects/Encounters/Act1_EncounterBank_Pilot.asset` (örnek bank SO)

## Başarı Kriterleri

- [ ] Compile errors yok
- [ ] EncounterController Inspector'da görünür, EncounterBankSO assign edilebilir
- [ ] Pilot .asset dosyaları oluşturulmuş
- [ ] Console'da hata yok
- [ ] ~~(Bonus) OverlapWallRoomTest sahnesine EncounterController ekle~~ — KALDIRILDI: A task ile sahne conflict riski. EncounterController sahneye Task A commit olduktan sonra ayrı task ile eklenecek.
