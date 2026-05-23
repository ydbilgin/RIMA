# CLAUDE CODE PLAN: Multi-Layer Flat Top-Down Cliff & Tileset System

This document provides a step-by-step implementation plan for **Claude Code** to set up the multi-layered flat top-down orthogonal/oblique cliff tileset system in Unity for the RIMA project.

---

## 1. Ground & Cliff Multi-Layer Concept
To avoid 3D/isometric "cube thickness" biases from AI generation, we separate terrain into modular flat pieces and stack them inside Unity using **multiple Tilemap layers**:
*   **Layer 1: Ground (Bottom)** - Flat seamless grass/ground.
*   **Layer 2: Mid-Cliff (Level 1)** - Flat flagstone paths + low cliff wall facades hanging down.
*   **Layer 3: High-Cliff (Level 2)** - Flat bright grass + high cliff wall facades hanging down.

Uçurum taşlarının (cliff walls) dikey yüzeyleri, **Sprite Pivot Point = Top (Custom X=0.5, Y=1.0)** ayarlanarak üst katmandan aşağıya doğru sarkıtılır ve alttaki zemini doğal bir şekilde kapatarak 2.5D derinlik hissi yaratır.

---

## 2. Technical Setup Tasks for Claude Code

### Task 1: Create the Multi-Layer Tilemap Grid Structure
Create a script or manually set up the Grid GameObject hierarchy in the active test scene (`Assets/Scenes/Demo/TopDownTest_Map1.unity`):
```text
Grid (Grid Component, Cell Size = 0.5 x 0.5 x 0)
 ├── Tilemap_Ground (Sorting Layer: Default, Order: 0) - Base terrain
 ├── Tilemap_MidCliff (Sorting Layer: Default, Order: 10) - Mid-level path & low cliff faces
 └── Tilemap_HighCliff (Sorting Layer: Default, Order: 20) - High-level platforms & high cliff faces
```
*   Ensure each Tilemap has a `TilemapRenderer` with **detect chunk culling** enabled.
*   Configure a `TilemapCollider2D` on `Tilemap_MidCliff` and `Tilemap_HighCliff` for wall collision boundaries.

### Task 2: Configure 2D URP Renderer & Custom Y-Axis Sorting
Configure the rendering sorting axes to handle orthogonal depth layering:
1.  Navigate to **Project Settings -> Graphics**.
2.  Set **Transparency Sort Mode** to `Custom Axis`.
3.  Set **Transparency Sort Axis** to `(0, 1, 0)` (Y-Axis sorting, so sprites lower on the screen render in front of sprites higher on the screen).
4.  In the `Player` and `Enemy` Prefabs, ensure:
    *   `Sprite Sort Point` is set to `Pivot`.
    *   The **Sprite Pivot** is set to the very bottom center (where the feet touch the ground, `X = 0.5, Y = 0`).

### Task 3: Enhance custom RoomDesigner for Multi-Layer Painting
Modify `Assets/Editor/RoomDesigner/RimaRoomDesignerWindow.cs` to add support for the three vertical layers.

1.  **Add Layer Target Selection:**
    Add an enum and selection buttons in the RoomDesigner UI:
    ```csharp
    public enum TargetDesignLayer
    {
        Ground,
        MidCliff,
        HighCliff
    }
    ```
2.  **Redirect Paint Actions:**
    Ensure that painting with stamp/bucket brushes writes to the correct selected target tilemap according to `TargetDesignLayer`.

---

## 3. PixelLab Generation Guidelines for the Designer

### A. Flat Ground Floor
*   **Prompt:** `seamless flat stone floor texture, 100% flat 2D top-down, Zelda GBC style, crisp pixel art, no shadows, no height, no depth, solid ground, hand-drawn pixel art`
*   **Settings:** 32x32px, Point filter.

### B. Vertical Cliff Wall
*   **Prompt:** `vertical rocky cliff wall facade, stone texture, straight-on flat front-facing view, 2D pixel art, no top face, no bevel, tileable horizontally, RPG Maker style`
*   **Settings:** 32x32px or 32x64px, Sprite Pivot = Top (X=0.5, Y=1.0).

### C. Curved Edge Border
*   **Prompt:** `curved rocky edge border, transition outline, flat 2D top-down view, separating grass and rock, crisp black outlines, transparent background, no thickness`
