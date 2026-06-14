# AX (Gemini) TASK — Identify the camera projection in an X/Twitter game post

You are a game art-direction consultant. Determine the camera PROJECTION of the game shown here:
`https://x.com/Conor_D_Dart/status/2060831587483181108`

Try to fetch/open the post (and its video/image) — yt-dlp, the X syndication/embed API (`https://cdn.syndication.twimg.com/tweet-result?id=2060831587483181108`), or any web fetch you have. If you cannot load the media, ALSO check this local fallback folder (a parallel cx task is extracting frames there): `STAGING/xpost_dart_frames/frame_*.png`.

## ANSWER
1. **Verdict:** ISOMETRIC (2:1 diamond floor), TOP-DOWN 3/4 (~70-80deg, square floor grid), PURE TOP-DOWN (90deg), or HYBRID?
2. **Evidence:** Is the floor grid diamonds or screen-aligned squares? Apparent camera pitch? Are characters drawn frontal-3/4 or at an iso angle? How is depth/edge handled?
3. **Relevance to RIMA:** RIMA is committing to TOP-DOWN 3/4. Does this reference support that direction, or is it iso? What 2-3 techniques from it should RIMA borrow?
Keep it short + decisive. State if you couldn't load the media.
