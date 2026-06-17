TASK 9 reward monolog bleed
Changed: Assets/Scripts/UI/SkillOfferUI.cs, Assets/Scripts/UI/RoomMonologController.cs
Approach: suppress RoomMonologController groups while SkillOfferUI is open; no OverlayDark/global alpha change.
Verify flow: Play Mode, active RoomMonologController.Say("Bu...degil. Sadece dur..."), then SkillOfferUI.Show reward cards.
Runtime assert: reward panel active, RoomMonolog_Line inactive, RoomMonolog_Title inactive.
Screenshot: STAGING/_process/2026-06/demo_fix_tasks/TASK_9_reward_monolog_suppressed.png
Close verify: SkillOfferUI.Hide(), then new monolog line active again.
Console: 0 errors, 0 warnings after compile/play verification.
