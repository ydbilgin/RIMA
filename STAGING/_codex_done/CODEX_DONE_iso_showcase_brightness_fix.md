IsoShowcaseRoom S95 brightness/framing fix complete.

Before/after parameters:
- GlobalLight2D_Ambient intensity: 0.25 -> 0.55
- GlobalLight2D_Ambient color: #1A1A2A -> #2A2238
- L1_Rift_Cyan_PointLight intensity: 1.0 -> 1.7
- L2_Torch_West_Orange_PointLight intensity: 0.8 -> 1.4
- L3_Torch_East_Orange_PointLight intensity: 0.8 -> 1.4
- L4_Brazier_Orange_PointLight intensity: 0.6 -> 1.1
- L5_Brazier_Cyan_PointLight intensity: 0.5 -> 0.9
- Main Camera orthographic size: 5 -> 3.5
- Main Camera position: (1.5, 1.838, -10) -> (2.2, 2.3, -10)

New screenshot:
- STAGING/screenshots/IsoShowcaseRoom_S95_brightness_fix.png

Console status:
- 0 errors after clearing stale MCP bridge errors and re-reading console.

Visibility note:
- The room should now read clearly as a showcase shot: ambient visibility is higher, cyan/warm focal lights have stronger separation, and the camera crops out the previous empty right side.
