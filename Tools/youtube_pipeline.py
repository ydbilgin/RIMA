#!/usr/bin/env python3
"""PixelLab YouTube analysis pipeline.

Downloads videos, extracts frames, analyzes with local vision/text models.
Output: STAGING/youtube_analysis/<video_id>/analysis.md
        STAGING/youtube_analysis/SYNTHESIS.md (final)

Usage:
  python youtube_pipeline.py               # full pipeline, vision model
  python youtube_pipeline.py --list        # list videos + priority scores, no download
  python youtube_pipeline.py --id ID       # single video
  python youtube_pipeline.py --model gemma4:26b  # override model (overnight)
  python youtube_pipeline.py --priority-only     # only high-priority videos
"""

import argparse
import base64
import json
import subprocess
import sys
import urllib.request
import urllib.error
from pathlib import Path
from datetime import datetime

# --- Config ---
CHANNEL_URLS = [
    "https://www.youtube.com/@PixelLab_AI/videos",
    "https://www.youtube.com/@PixelLab_AI/shorts",
]
REPO_ROOT = Path(__file__).parent.parent
OUT_DIR = REPO_ROOT / "STAGING" / "youtube_analysis"
OLLAMA_URL = "http://localhost:11434"
DEFAULT_MODEL = "qwen2.5vl:7b"

# Frame sampling: seconds between frames per video length bucket
FRAME_INTERVAL = {
    "short":  2,   # < 90s  (Shorts)
    "medium": 5,   # 90s - 600s
    "long":   10,  # > 600s
}
MAX_FRAMES_PER_VIDEO = 60

# Pipeline relevance keywords -> score
KEYWORDS = {
    "animation": 3, "animate": 3, "walk": 3, "idle": 3, "skeleton": 3,
    "interpolat": 3, "rotation": 3, "rotate": 3, "character": 2,
    "isometric": 3, "inpaint": 2, "consistent": 2, "sprite": 2,
    "boss": 2, "mob": 2, "enemy": 2, "style": 1,
    "tile": 1, "tileset": 1, "edit": 1, "workflow": 1,
}

FRAME_PROMPT = (
    "Analyze this PixelLab pixel art tool tutorial frame. "
    "Extract ONLY actionable information:\n"
    "- Tool or feature shown (exact name if visible)\n"
    "- Settings visible (canvas size, frame count, model/style options, parameters)\n"
    "- Workflow step being demonstrated\n"
    "- Any UI text, labels, or values visible\n"
    "- Relevance to: character animation, sprite rotation, style consistency, "
    "isometric tiles, batch generation\n\n"
    "Bullet points only. Skip generic observations. "
    "If frame shows no relevant content, write 'SKIP'."
)

TRANSCRIPT_PROMPT = (
    "PixelLab tutorial transcript. Context: 2D roguelite game, "
    "isometric view, 128px characters, 4-directional sprites.\n\n"
    "Extract:\n"
    "1. Workflow steps with specific parameter values\n"
    "2. Settings mentioned (canvas size, frame counts, model names, API options)\n"
    "3. Tips, tricks, non-obvious techniques\n"
    "4. Limitations or known issues\n"
    "5. Anything for: character animation, rotation, style consistency, batch gen\n\n"
    "Format: structured markdown with ## headers. "
    "Skip intro/outro/marketing content."
)

SYNTHESIS_PROMPT = (
    "You are synthesizing PixelLab tutorial analysis for a 2D roguelite game pipeline.\n"
    "Game specs: isometric, 128px characters, 4-directional sprites, Unity URP.\n\n"
    "Below are per-video analyses. Produce a synthesis document with:\n"
    "## Key Workflow Discoveries\n"
    "## Settings & Parameters (canonical values found across videos)\n"
    "## Animation Techniques\n"
    "## Style Consistency Methods\n"
    "## Rotation / Multi-Direction Workflow\n"
    "## Isometric Tiles & Objects\n"
    "## Surprising Findings (non-obvious, not in docs)\n"
    "## What NOT to do (confirmed limitations)\n\n"
    "Be specific. Prefer concrete values over vague descriptions.\n\n"
    "--- ANALYSES ---\n"
)


def score_video(title: str) -> int:
    t = title.lower()
    return sum(v for k, v in KEYWORDS.items() if k in t)


def ollama_call(model: str, prompt: str, image_path: Path | None = None) -> str:
    payload: dict = {"model": model, "prompt": prompt, "stream": False}
    if image_path:
        payload["images"] = [base64.b64encode(image_path.read_bytes()).decode()]
    data = json.dumps(payload).encode()
    req = urllib.request.Request(
        f"{OLLAMA_URL}/api/generate",
        data=data,
        headers={"Content-Type": "application/json"},
    )
    try:
        with urllib.request.urlopen(req, timeout=120) as resp:
            result = json.loads(resp.read())
            return result.get("response", "").strip()
    except urllib.error.URLError as e:
        return f"[ollama error: {e}]"


def list_channel_videos(url: str) -> list[dict]:
    cmd = [
        "yt-dlp", "--flat-playlist",
        "--print", "%(id)s|%(title)s|%(duration)s",
        "--no-warnings", url,
    ]
    result = subprocess.run(cmd, capture_output=True, text=True, timeout=60)
    videos = []
    for line in result.stdout.strip().splitlines():
        parts = line.split("|", 2)
        if len(parts) == 3:
            vid_id, title, dur = parts
            try:
                duration = float(dur)
            except ValueError:
                duration = 45.0  # Shorts without duration
            videos.append({"id": vid_id, "title": title, "duration": duration,
                           "score": score_video(title)})
    return videos


def download_video(vid_id: str, out_path: Path) -> Path | None:
    out_path.mkdir(parents=True, exist_ok=True)
    mp4 = out_path / f"{vid_id}.mp4"
    if mp4.exists():
        return mp4
    cmd = [
        "yt-dlp", "-f", "bestvideo[height<=720][ext=mp4]+bestaudio/best[height<=720]",
        "--merge-output-format", "mp4",
        "-o", str(mp4),
        f"https://www.youtube.com/watch?v={vid_id}",
        "--no-warnings", "--quiet",
    ]
    r = subprocess.run(cmd, timeout=300)
    return mp4 if (r.returncode == 0 and mp4.exists()) else None


def get_transcript(vid_id: str, out_path: Path) -> str | None:
    txt = out_path / f"{vid_id}.txt"
    if txt.exists():
        return txt.read_text(encoding="utf-8")
    # Try auto-captions via yt-dlp
    cmd = [
        "yt-dlp", "--write-auto-subs", "--skip-download",
        "--sub-format", "txt", "--sub-langs", "en",
        "-o", str(out_path / vid_id),
        f"https://www.youtube.com/watch?v={vid_id}",
        "--no-warnings", "--quiet",
    ]
    subprocess.run(cmd, timeout=60)
    # yt-dlp saves as <vid_id>.en.txt
    for candidate in out_path.glob(f"{vid_id}*.txt"):
        return candidate.read_text(encoding="utf-8", errors="replace")
    return None


def extract_frames(video_path: Path, out_dir: Path, duration: float) -> list[Path]:
    out_dir.mkdir(parents=True, exist_ok=True)
    existing = sorted(out_dir.glob("frame_*.jpg"))
    if existing:
        return existing

    if duration < 90:
        interval = FRAME_INTERVAL["short"]
    elif duration < 600:
        interval = FRAME_INTERVAL["medium"]
    else:
        interval = FRAME_INTERVAL["long"]

    # Limit total frames
    n_frames = min(int(duration / interval), MAX_FRAMES_PER_VIDEO)
    interval = max(interval, int(duration / MAX_FRAMES_PER_VIDEO))

    cmd = [
        "ffmpeg", "-i", str(video_path),
        "-vf", f"fps=1/{interval},scale=640:-1",
        "-q:v", "3",
        str(out_dir / "frame_%04d.jpg"),
        "-loglevel", "error", "-y",
    ]
    subprocess.run(cmd, timeout=300)
    return sorted(out_dir.glob("frame_*.jpg"))


def analyze_video(video: dict, model: str, skip_download: bool = False) -> str:
    vid_id = video["id"]
    title = video["title"]
    duration = video["duration"]
    vid_dir = OUT_DIR / vid_id
    analysis_file = vid_dir / "analysis.md"

    if analysis_file.exists():
        print(f"  [skip] {vid_id} already analyzed")
        return analysis_file.read_text(encoding="utf-8")

    print(f"\n[{vid_id}] {title[:60]} ({int(duration)}s, score={video['score']})")
    vid_dir.mkdir(parents=True, exist_ok=True)

    lines = [f"# {title}", f"id: {vid_id}  duration: {int(duration)}s", ""]

    # --- Transcript ---
    transcript = get_transcript(vid_id, vid_dir)
    if transcript:
        print("  transcript: found, analyzing...")
        t_analysis = ollama_call(model, TRANSCRIPT_PROMPT + "\n\n" + transcript[:8000])
        lines += ["## Transcript Analysis", t_analysis, ""]
    else:
        print("  transcript: not available")

    # --- Frames ---
    if not skip_download:
        mp4 = download_video(vid_id, vid_dir)
        if mp4:
            frames = extract_frames(mp4, vid_dir / "frames", duration)
            print(f"  frames: {len(frames)} extracted, analyzing...")
            frame_notes = []
            for i, frame in enumerate(frames):
                print(f"    frame {i+1}/{len(frames)}", end="\r")
                note = ollama_call(model, FRAME_PROMPT, frame)
                if note and note.upper() != "SKIP":
                    ts = int(i * (duration / len(frames)))
                    frame_notes.append(f"**{ts}s** — {note}")
            if frame_notes:
                lines += ["## Frame Analysis", *frame_notes, ""]
            # Optionally delete video after analysis to save space
            mp4.unlink(missing_ok=True)
        else:
            print("  download failed")

    content = "\n".join(lines)
    analysis_file.write_text(content, encoding="utf-8")
    return content


def synthesize(model: str):
    analyses = []
    for f in sorted(OUT_DIR.glob("*/analysis.md")):
        analyses.append(f"### {f.parent.name}\n" + f.read_text(encoding="utf-8")[:3000])

    if not analyses:
        print("No analyses found to synthesize.")
        return

    print(f"\nSynthesizing {len(analyses)} video analyses...")
    combined = SYNTHESIS_PROMPT + "\n\n".join(analyses)
    synthesis = ollama_call(model, combined[:32000])

    out = OUT_DIR / "SYNTHESIS.md"
    out.write_text(
        f"# PixelLab YouTube Analysis Synthesis\n"
        f"Generated: {datetime.now().strftime('%Y-%m-%d %H:%M')}\n"
        f"Videos: {len(analyses)}\n\n"
        + synthesis,
        encoding="utf-8",
    )
    print(f"Synthesis saved: {out}")


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--model", default=DEFAULT_MODEL, help="Ollama model to use")
    parser.add_argument("--list", action="store_true", help="List videos + scores only")
    parser.add_argument("--id", help="Analyze single video ID")
    parser.add_argument("--priority-only", action="store_true", help="Only videos with score >= 4")
    parser.add_argument("--synthesize-only", action="store_true", help="Only run synthesis step")
    parser.add_argument("--min-score", type=int, default=0)
    args = parser.parse_args()

    OUT_DIR.mkdir(parents=True, exist_ok=True)

    # Collect all videos
    all_videos = []
    for url in CHANNEL_URLS:
        print(f"Fetching: {url}")
        all_videos.extend(list_channel_videos(url))

    # Deduplicate by id
    seen = set()
    videos = []
    for v in all_videos:
        if v["id"] not in seen:
            seen.add(v["id"])
            videos.append(v)

    # Sort by score desc
    videos.sort(key=lambda v: v["score"], reverse=True)

    if args.list:
        print(f"\n{'Score':>5}  {'Duration':>8}  {'ID':>12}  Title")
        print("-" * 80)
        for v in videos:
            dur = f"{int(v['duration'])}s" if v['duration'] else "?"
            print(f"{v['score']:>5}  {dur:>8}  {v['id']:>12}  {v['title'][:50]}")
        print(f"\nTotal: {len(videos)} videos")
        return

    if args.synthesize_only:
        synthesize(args.model)
        return

    if args.id:
        # Find or construct video entry
        match = next((v for v in videos if v["id"] == args.id), None)
        if not match:
            match = {"id": args.id, "title": args.id, "duration": 0, "score": 0}
        analyze_video(match, args.model)
        synthesize(args.model)
        return

    # Filter
    min_score = 4 if args.priority_only else args.min_score
    filtered = [v for v in videos if v["score"] >= min_score]
    print(f"\n{len(filtered)} videos to analyze (score >= {min_score}), model: {args.model}")

    for video in filtered:
        analyze_video(video, args.model)

    synthesize(args.model)
    print("\nDone.")


if __name__ == "__main__":
    main()
