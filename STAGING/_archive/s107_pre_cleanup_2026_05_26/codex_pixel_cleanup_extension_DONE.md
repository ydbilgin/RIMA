# pixel_cleanup Extension DONE

Date: 2026-05-24

## Created files
- `Tools/pixel_cleanup/smart_merge.py`
- `Tools/pixel_cleanup/region_fix.py`
- `Tools/pixel_cleanup/tests/test_smart_merge.py`
- `Tools/pixel_cleanup/tests/test_region_fix.py`
- `Tools/pixel_cleanup/tests/test_gui_smoke.md`

## Integration status
- `pixel_cleanup.py` current hooks call `smart_merge.py` for `--smart_merge`.
- `pixel_cleanup.py` current hooks call `region_fix.py` for `--region x,y,size`.
- `pixel_cleanup.py --gui` routes to `pixel_cleanup_gui.py`.
- `~/.claude/skills/pixel-cleanup/SKILL.md` contains GUI, smart merge, region fix, auto-decision sections, and the new triggers.

## Verification results
- `python -m py_compile .\Tools\pixel_cleanup\smart_merge.py .\Tools\pixel_cleanup\region_fix.py .\Tools\pixel_cleanup\pixel_cleanup_gui.py .\Tools\pixel_cleanup\pixel_cleanup.py` PASS.
- `python -m pytest .\Tools\pixel_cleanup\tests -q` PASS: 15 passed.
- `python .\Tools\pixel_cleanup\pixel_cleanup.py --smart_merge --input .\STAGING\pixel_cleanup_verify\smart_cli_input.png` PASS.
- `python .\Tools\pixel_cleanup\pixel_cleanup.py --input .\STAGING\pixel_cleanup_verify\smart_cli_input.png --region 15,15,16` PASS.
- `python .\Tools\pixel_cleanup\pixel_cleanup.py --smart_merge --input .\STAGING\pixel_cleanup_verify\smart_cli_input.png --output .\STAGING\pixel_cleanup_verify\smart_cli_output.png --report .\STAGING\pixel_cleanup_verify\smart_cli_report.json --apply_cleanup --force` PASS.
- `python .\Tools\pixel_cleanup\pixel_cleanup.py --input .\STAGING\pixel_cleanup_verify\smart_cli_input.png --region 15,15,16 --output .\STAGING\pixel_cleanup_verify\region_cli_output.png --report .\STAGING\pixel_cleanup_verify\region_cli_report.json --apply_cleanup --force` PASS.
- Tkinter GUI smoke PASS via self-closing launch with sample PNG.

## GUI smoke screenshot
- `STAGING/pixel_cleanup_verify/gui_smoke_screenshot.png`

## Issues / blockers
- `ANTIGRAVITY.md` was referenced by project routing rules but is not present in the repository root.
- GUI screenshot was obtainable as a Windows desktop capture, not a cropped app-only capture.

## Recommended next steps
- User/manual pass on real PixelLab outputs through the GUI.
- Tune confidence thresholds with production sprites after first real batch.
- Review the generated smart merge cluster reports for intentional accent colors before aggressive auto mode.
