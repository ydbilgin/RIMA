"""Extract Opus rima-design output from transcript JSONL line 1344."""
import json
from pathlib import Path

TRANSCRIPT = Path(r"C:/Users/ydbil/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/a12da79a-6b77-423a-8b7c-59af8ccea2f8.jsonl")
OUT = Path(r"F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/_opus_design_raw.md")

with TRANSCRIPT.open("r", encoding="utf-8") as f:
    for i, line in enumerate(f, start=1):
        if i == 1344:
            data = json.loads(line)
            content = data.get("content", "")
            print(f"Top-level content length: {len(content)}")
            inner = json.loads(content) if content.startswith("{") else None
            if inner:
                print(f"Inner keys: {list(inner.keys())}")
            else:
                start = content.find("<result>")
                end = content.find("</result>")
                if start >= 0 and end >= 0:
                    result_text = content[start + len("<result>"):end]
                    OUT.write_text(result_text, encoding="utf-8")
                    print(f"Wrote {OUT} ({len(result_text)} chars)")
                else:
                    print("No <result> tags found")
                    OUT.write_text(content, encoding="utf-8")
                    print(f"Wrote raw content ({len(content)} chars)")
            break
