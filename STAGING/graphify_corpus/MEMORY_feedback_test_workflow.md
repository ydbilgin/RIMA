---
name: Test Running Rule
description: Run PlayMode tests after major changes or at session end.
type: feedback
---
* **Rule:** Run tests at session end or post-major refactor. Not for every file edit.
* **Why:** Saves tokens. 0 failure = 1-line result (cheap).
* **Action:** `run_tests(mode=PlayMode, include_failed_tests=true, include_details=false)`.
* **Standard:** If passed, stop. If failed, read only error `message`.
* **Growth:** Add 1-2 tests to `RoomFlowTests.cs` for every new system.
