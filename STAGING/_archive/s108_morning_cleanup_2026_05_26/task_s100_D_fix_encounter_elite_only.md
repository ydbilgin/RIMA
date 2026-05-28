ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Not needed for this fix.

# Amaç: Task D Fix — eliteOnly field ThreatBudget.IsEligible'da kontrol edilmiyor

rima-qc bulgusu: `EncounterWaveSO.EncounterEnemyEntry.eliteOnly` field declare edilmiş ama `ThreatBudget.IsEligible` metodunda hiç okunmuyor. Normal room'da eliteOnly=true entry'ler spawn ediliyor.

## Düzeltilecek dosya

`Assets/Scripts/Encounter/ThreatBudget.cs`

## Fix

`IsEligible` metodunu bul (satır ~121-139). `eliteRoom` parametresi var mı kontrol et.

**Eğer `isEliteRoom` parametresi yoksa** — metod imzasına ekle ve EncounterController'dan geçir:

```csharp
// ThreatBudget.IsEligible imzası
private bool IsEligible(EncounterEnemyEntry entry, bool isEliteRoom)
{
    // ... mevcut kontroller ...
    if (entry.eliteOnly && !isEliteRoom) return false;
    // ...
}
```

**EncounterController.cs** içinde ThreatBudget'a `isEliteRoom` bilgisini geçen çağrıyı güncelle. `[SerializeField] private bool isEliteRoom;` field ekle (Inspector'dan set edilir).

## Bonus (opsiyonel, eğer hızlıysa)

Namespace: tüm 4 dosyada `namespace RIMA` → `namespace RIMA.Encounter` olarak güncelle. Compile sırasında ambiguity yoksa yap, varsa atla.

## Başarı Kriterleri
- [ ] `IsEligible` eliteOnly kontrolü yapıyor
- [ ] Normal room'da eliteOnly=true entry spawn edilmiyor
- [ ] Compile errors yok
- [ ] Console 0 error
