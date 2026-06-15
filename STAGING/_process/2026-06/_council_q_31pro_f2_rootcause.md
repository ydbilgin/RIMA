# ax Gemini 3.1 Pro (High) — DEEP / architecture / root-cause lens

READ THESE FILES (do not assume; open them):
1. STAGING/_process/2026-06/_council_f2_rootcause_2026-06-15.md  ← brief + orchestrator canlı repro bulguları (ŞART)
2. Assets/Scripts/Core/RewardPickup.cs
3. Assets/Scripts/Skills/DraftManager.cs
4. Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs
5. Assets/Scripts/UI/SkillOfferUI.cs
6. Assets/Scripts/Core/RuntimeRoomManager.cs

## DURUM
RIMA demo bug F2: reward topla → 3-kart skill draft AÇILMIYOR. Orchestrator (Opus) canlı repro ile KANITLADI: `DraftManager.ShowDraft()` izolasyonda (room=1, auto-managers) SkillOfferPanel canvas + 6 buton kuruyor, IsDraftActive=True → **kart-render path ÇALIŞIYOR**. ELENDİ: dep-null (#3), render hatası, Instance-null (#1, EnsureDraftManager savunuyor — RoomRunDirector.cs:1374,1506). KALAN: (A) Forge room-depth misresolve, (C) pickup-collect, (D) flow-spesifik.

## SENİN LENS'İN: derin mimari + kök-neden muhakemesi
**Q1:** ShowDraft çalıştığına göre, kalan (A)/(C)/(D) içinde en olası kök neden hangisi ve NEDEN? Kod akışını file:line ile izle. Özellikle: `DraftThenOpenExit` coroutine guard'ı (RewardPickup.cs:182-186) erken çıkar mı? `IsDraftActive` ne zaman false olur (kim resetler)? Forge return (DraftManager.cs:211-224) golden-path'te tetiklenebilir mi?

**Q2 (KRİTİK):** F2 golden-path'i (normal ilk combat oda) mı bozuyor, yoksa sadece Forge(4/8)/Echo odalarını mı? Mimari kanıtla.

**Q4:** Düzeltme gerekiyorsa en küçük cerrahi fix (mimari olarak doğru, side-effect'siz). 5 adayı körlemesine fixleme YOK.

Çıktı: ≤ kısa, her iddia file:line ile. Spekülasyon değil, koda dayalı.
