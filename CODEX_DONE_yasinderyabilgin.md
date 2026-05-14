Codex result summary - UI scenes task

Commit:
- 01985d7 [ui] MainMenu + CharacterSelect scenes - dark rift palette, 10-class selector, navigation flow

Implemented:
- Added scene-backed MainMenu and CharacterSelect UI flow.
- Added MainMenuController and CharacterSelectController.
- Added UI helper behaviours for title alpha pulse, rift crack motion, and button hover/press feedback.
- Added CreateUIScenes editor tool at RIMA/Tools/Create UI Scenes.
- Generated Assets/Scenes/UI/MainMenu.unity and Assets/Scenes/UI/CharacterSelect.unity.
- Generated rift crack sprite asset at Assets/Sprites/UI/rift_crack_64.png.
- Updated Build Settings order: MainMenu, CharacterSelect, RoomPipelineTest, _FazMVP_Demo.
- Guarded legacy MainMenuScreen auto-init so it does not overlay the new UI scenes.

Validation:
- Unity compile check: no project compile errors.
- Ran RIMA/Tools/Create UI Scenes through the live Unity editor.
- Scene validation found controllers, buttons, persistent button listeners, 10 class data entries, pulse/rift behaviours, and Build Settings order.
- Play-mode navigation checked: MainMenu -> CharacterSelect, 10 active class buttons, Hexer selection updates info panel, confirm loads RoomPipelineTest.

Notes:
- Batchmode Unity could not run because the project was already open in another Unity instance, so the live connected editor was used for the menu execution and play-mode test.
- Console errors observed after play-mode were MCP transport disconnect messages only, not project errors.
