---
name: Temporary Files Cleanup
type: feedback
trigger: temp file, one-time report, eval output, STAGING cleanup
description: Rule for one-time files: state deletion target at creation, delete immediately after use
---

## Rule
When creating a one-time file (QC report, review prompt, eval):
- State its deletion target in the same message.
- Delete immediately after its purpose is served.
- Do not accumulate temporary outputs in STAGING/.
