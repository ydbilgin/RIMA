# FIX-6 follow-up — coroutine completion nulling (cx, cerrahi)

ACTIVE RULES: (1) think (2) min code — sadece bu (3) surgical — SADECE RoomRunDirector.cs (4) BLOCKED if unclear.
UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var (MCP bridge disposed-object) BİLDİR; raporda console durumu.

## SORUN (cx verify bulgusu, VERIFY_cx.md FIX-6)
`Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` — `openingDraftSequence` field + BeginRun assign + StopClearSequences stop+null ZATEN VAR (FIX-6 kısmen uygulandı). EKSİK: `OpeningKitDraftSequence` coroutine'inin **tamamlanma/erken-çıkış (`yield break`) yollarında** `openingDraftSequence` null'lanmıyor → bitince stale ref kalıyor. Spec: "Sequence tamamlanınca null'a çek."

## YAP (SADECE bu)
`OpeningKitDraftSequence` içinde **her çıkış noktasında** (normal son + her erken `yield break`) `openingDraftSequence = null;` ata. (En temiz: coroutine gövdesinin sonunda + erken `yield break`'lerden hemen önce; ya da tek bir nullify-on-exit deseni.) Başka HİÇBİR şeye dokunma. FIX-4'e DOKUNMA (orchestrator waive etti).

## DOĞRULA
Recompile bekle → read_console 0 project-error. Rapor ≤10 satır: hangi satırlara null eklendi + console durumu.
