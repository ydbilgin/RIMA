# CODEX TASK — Painter Drag-Paint + Smart Auto-Connect

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Sorun

User'ın eski painter workflow'u: **mouse basılı tut + sürükle → devamlı paint** + **wall paint auto-connect Wang variant otomatik değişir**. S95 LATE NIGHT 2 refactor sonrası bu davranış BOZULDU. User: "basılı tutup çekince olmalıydı şimdi yapamıyoruz bunu."

## Mevcut Painter State

`Assets/Editor/RimaUnifiedPainterWindow.cs` — Antigravity painter fix sonrası iso paint çalışıyor ama:
- Wall paint mouse click TEK cell paint, drag (mouse down + move) çalışmıyor
- Auto-connect Wang variant logic var ama tek face piece olduğu için graceful degrade (flipX) ile face_EW çoğaltılıyor — gerçek "Wang variant pick" yapmıyor (face_NS archived, corner_inner/T/end_cap yok)

## Görev

### Fix 1 — Drag-Paint (Mouse Hold + Drag)
Şu an `PerformAction` (line ~2313) mouse down'da PaintWall, drag yok. Wall paint da mouse down'a sınırlı (line ~2324: `if (Event.current.type == EventType.MouseDown)`). Bunu **MouseDown + MouseDrag** olarak genişlet:

```csharp
if (currentCategory == PaletteCategory.Wall && autoConnectWalls)
{
    // Allow drag paint for walls too
    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
    {
        // Check if cell empty (avoid spam in same cell)
        if (FindWallAtCell(cellPos) == null)
        {
            PaintWallWithConnections(cellPos, snapPos, selectedPrefab);
        }
    }
}
```

Aynı drag-paint Prop kategorisine de uygula (image #9 dense prop layout için).

### Fix 2 — Smart Auto-Connect Visual Feedback
`UpdateWallConnectionsAt` neighbor check yapıyor ama user'a görsel feedback YOK. Drag sırasında:
- Hover edilen cell highlight (cyan glow)
- Adjacent wall'larla connection preview (line gizmo arası)
- Wang variant prediction visible (eğer face_NS yoksa "flipX" annotation)

Bu büyük UI overhead, opsiyonel. **Minimal:** drag-paint working + auto-connect mevcut logic preserve.

### Fix 3 — Erase Drag (zaten var, doğrula)
Floor erase drag ediliyor (line ~2342). Wall + Prop erase de drag yapıyor mu doğrula.

## Test (UnityMCP)

1. Sahneyi aç `IsoShowcaseRoom_S95.unity`
2. Painter Wall sekmesi, face_EW seç, auto-connect ON
3. Scene View'de **mouse basılı tut + sürükle** 3-4 cell üzerinde
4. Beklenti: her cell'e wall paint, auto-connect Wang variant
5. Drag erase test: Erase tool, basılı tut + sürükle → wall'lar silinir

## Allowed File Writes

- **MODIFY:** `Assets/Editor/RimaUnifiedPainterWindow.cs` (PerformAction + minimal helper)

## Forbidden

- Diğer painter logic'e dokunma (Antigravity flipX, Y-squash compensate, mode toggle, asset add/remove preserve)
- Sahne dosyalarına dokunma

## Rapor

`STAGING/CODEX_DONE_drag_paint_auto_connect.md`:
- Fix line ranges
- UnityMCP drag-paint test verify (X cell paint in 1 drag)
- Console 0 error

## Effort

low-medium — ~20dk, single function edit.
