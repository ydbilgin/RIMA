# pixel_cleanup GUI Manual Smoke Test

1. Run from `Tools/pixel_cleanup`:
   `python pixel_cleanup.py --gui`
2. Confirm the window title is `pixel_cleanup GUI`.
3. Open a PNG with `Open`.
4. Confirm the original panel renders the source image.
5. Click `Analyze` and confirm the cleaned preview updates.
6. Enable `Region Fix Mode`, click a suspect pixel, and confirm the modal lists suggestions or the manual-mode message.
7. Click `Apply Auto`, then `Save`, and confirm a PNG plus JSON report are written.
