# RIMA Tools

## audit_cleanup.py
A pure-Python file hygiene scanner for the RIMA project. It detects memory orphans, stale files, oversized memory files, staging orphans, archive intruders, and root clutter.

### Usage
```bash
python Tools/audit_cleanup.py
```

### Parameters
- `--memory-dir`: Path to Claude's memory folder (default: Windows absolute path).
- `--repo-dir`: Path to the project root (default: current directory).
- `--output`: Path to the generated report (default: `STAGING/_AUDIT_REPORT.md`).

### Recommended Cadence
Run at every phase transition or every 14 days to keep the project clean and token-efficient. The report should be triaged by Claude to decide which files to delete or archive.
