import subprocess, sys, os

prompt_file = os.path.join(os.path.dirname(__file__), "codex_brush_prompt.txt")
with open(prompt_file, "r", encoding="utf-8") as f:
    prompt = f.read()

cx = r"C:\Users\ydbil\AppData\Roaming\npm\cx.cmd"
cmd = [
    cx, "run", "yasinderyabilgin", "exec",
    "--skip-git-repo-check",
    "--color", "never",
    "--dangerously-bypass-approvals-and-sandbox",
    "--config", "model_reasoning_effort=high",
    prompt
]

result = subprocess.run(cmd, stdin=subprocess.DEVNULL, capture_output=False, text=True)
sys.exit(result.returncode)
