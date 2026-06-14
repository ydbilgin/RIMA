# CX TASK — Download X video + extract frames + identify projection

ACTIVE RULES: (1) think (2) minimal (3) surgical — only download + frames + analysis (4) note BLOCKED if download fails.

## GOAL
Determine the camera PROJECTION of the game in this X/Twitter post:
`https://x.com/Conor_D_Dart/status/2060831587483181108`

The RIMA team is deciding between TOP-DOWN 3/4 (~70-80deg, square floor grid, Hades/Children of Morta) vs ISOMETRIC (2:1 diamond floor). The user asks: "is this reference also top-down 3/4?"

## DO
1. Download the post's video/media with yt-dlp (try `yt-dlp "<url>"`; if blocked, try `yt-dlp --cookies-from-browser chrome "<url>"` or fetch via gallery-dl / the X syndication API). If video download is impossible, grab the still image(s).
2. Extract ~8 representative frames with ffmpeg as PNGs into `STAGING/xpost_dart_frames/` named `frame_01.png` ... `frame_08.png` (e.g. `ffmpeg -i video.mp4 -vf fps=1 STAGING/xpost_dart_frames/frame_%02d.png`).
3. Inspect the frames and classify the projection. Answer specifically:
   - Is the FLOOR grid drawn as DIAMONDS (isometric 2:1) or as SQUARES aligned to screen X/Y (top-down 3/4)?
   - What is the apparent camera pitch (shallow ~30-35deg iso, vs steep ~70-80deg top-down 3/4)?
   - Are characters/props drawn top-down 3/4 frontal or at an iso angle?
   - Verdict: ISOMETRIC, TOP-DOWN 3/4, PURE TOP-DOWN (90deg), or HYBRID — with the visual evidence that decided it.
4. Note any art techniques worth stealing for RIMA (edge treatment, lighting, depth cues).

## OUTPUT
A short markdown report: the verdict + evidence + the list of extracted frame paths. If download blocked, say exactly what you tried and BLOCKED.
