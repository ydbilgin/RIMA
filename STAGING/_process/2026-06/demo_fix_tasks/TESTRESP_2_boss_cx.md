A) 2A PASS: boss node=4 type=Boss lifecycle=Combat; activeEnemies=1; activeShopController=null; active ShopRoomController/merchant stand not present in boss-room scan.
A) 2B PASS: PenitentSovereign_Boss pos=(5.641,5.732), sprite bounds center=(5.641,5.932) ext=(1,1) inside Ground tilemap bounds center=(1.68,9.21) ext=(15.12,9.21); screenshot shows no HUD overlap.
A) 2C PASS: BossHealthPanel active; HPFill=AD0F14FF (crimson, no green); Phase66Notch anchor=0.66 and Phase33Notch anchor=0.33; labels THE PENITENT SOVEREIGN + PHASE I present.
A) 2D PASS: RoomMonolog line panel/text at screen y~381, above SkillBar top y~94 and below boss HUD; no boss-body overlap in game_view/scene_view screenshots.
B) Reward bleed YOK: active 3-card SkillOfferUI panel cards=3, offerOrder=1050 scrim=0D0D12D9; RoomMonolog order=1030 and line/title groups inactive alpha=0 during offer.
B) Cause/effect: current RoomMonologController diff only moves line/title anchoring upward (34->104 / title bottom anchor); no sorting change, so effect on reward bleed is neutral; it improves bottom safe-area placement.
Screenshots: Assets/Screenshots/TEST_2_boss_game_view_20260617-143509.png, TEST_2_boss_scene_view_20260617-143509.png, TEST_2_reward_bleed_active_game_view_20260617-143509.png.
Console: 0 errors; 6 warnings from forced inactive SkillOfferUI_Auto coroutine path during reward activation.
