#!/usr/bin/env python3
"""
PixelLab Discord export analyzer.

Phase 1 (default): JSON parse -> message chunks -> Ollama text model -> per-channel digest -> MASTER.md
Phase 2 (--with-images): vision model on image attachments + nearby text context
Phase 3 (--with-videos): ffmpeg keyframe extract -> vision model -> per-video summary

Authority weighting: messages from users with roles matching authority_roles.txt
are tagged [AUTHORITY: <role>] and given priority by the model + collected separately.

Usage:
    python analyze.py                              # text-only (fast, ~minutes)
    python analyze.py --with-images                # + vision pass on images
    python analyze.py --with-images --with-videos  # full
    python analyze.py --image-limit 50             # cap per channel
"""

import argparse
import base64
import json
import shutil
import subprocess
import sys
import urllib.request
from pathlib import Path

OLLAMA_URL = "http://localhost:11434/api/generate"
DEFAULT_TEXT_MODEL = "qwen2.5:14b"
DEFAULT_VISION_MODEL = "gemma4:e4b"
FALLBACK_TEXT_MODEL = "qwen2.5vl:7b"
FALLBACK_VISION_MODEL = "qwen2.5vl:7b"
CHUNK_SIZE = 60
KEYFRAME_INTERVAL = 2  # seconds between extracted frames
MAX_FRAMES_PER_VIDEO = 8

IMAGE_EXTS = {".png", ".jpg", ".jpeg", ".webp", ".gif", ".bmp"}
VIDEO_EXTS = {".mp4", ".mov", ".webm", ".mkv", ".avi"}

SCRIPT_DIR = Path(__file__).resolve().parent


# ----- Ollama -----

def ollama_call(model, prompt, images=None, timeout=900):
    payload = {
        "model": model,
        "prompt": prompt,
        "stream": False,
        "options": {
            "num_ctx": 4096,   # Discord chunk'ları kısa, 4K yeterli — VRAM tasarrufu
            "num_gpu": 99,     # Tüm layer'ları GPU'ya zorla
        },
    }
    if images:
        payload["images"] = images
    req = urllib.request.Request(
        OLLAMA_URL,
        data=json.dumps(payload).encode("utf-8"),
        headers={"Content-Type": "application/json"},
    )
    with urllib.request.urlopen(req, timeout=timeout) as resp:
        body = json.loads(resp.read())
    return body.get("response", "")


def ollama_check(model):
    try:
        resp = urllib.request.urlopen("http://localhost:11434/api/tags", timeout=5)
        tags = json.loads(resp.read())
        installed = {m["name"].split(":")[0] for m in tags.get("models", [])}
        model_base = model.split(":")[0]
        if model_base not in installed:
            print(f"  Model '{model}' bulunamadi, ollama pull baslatiliyor...")
            subprocess.run(["ollama", "pull", model], check=True)
    except urllib.error.URLError as e:
        print(f"ERROR: Ollama erisilemedi (localhost:11434). 'ollama serve' calisiyor mu? {e}")
        sys.exit(1)


# ----- Authority -----

def load_authority_patterns():
    f = SCRIPT_DIR / "authority_roles.txt"
    if not f.exists():
        return []
    pats = []
    for line in f.read_text(encoding="utf-8").splitlines():
        s = line.strip()
        if s and not s.startswith("#"):
            pats.append(s.lower())
    return pats


def match_authority(roles, patterns):
    if not roles or not patterns:
        return None
    for r in roles:
        rname = (r.get("name") or "").lower()
        for p in patterns:
            if p in rname:
                return r.get("name")
    return None


# ----- Parse -----

def parse_export(json_path, authority_patterns):
    with open(json_path, encoding="utf-8") as f:
        data = json.load(f)
    msgs = []
    for m in data.get("messages", []):
        text = (m.get("content") or "").strip()
        atts = m.get("attachments", []) or []
        if not text and not atts:
            continue
        author = m.get("author") or {}
        roles = author.get("roles") or []
        auth_role = match_authority(roles, authority_patterns)
        msgs.append({
            "id": m.get("id"),
            "ts": m.get("timestamp"),
            "author": author.get("nickname") or author.get("name") or "?",
            "is_bot": bool(author.get("isBot")),
            "authority": auth_role,  # role name or None
            "text": text,
            "attachments": atts,
        })
    return msgs


def chunk_messages(msgs, size=CHUNK_SIZE):
    for i in range(0, len(msgs), size):
        yield msgs[i:i + size]


def format_chunk(chunk):
    lines = []
    for m in chunk:
        if m["is_bot"]:
            continue
        att_note = ""
        if m["attachments"]:
            kinds = ", ".join((a.get("fileName") or "?")[:40] for a in m["attachments"][:3])
            att_note = f" [+{len(m['attachments'])} att: {kinds}]"
        auth = f" [AUTHORITY: {m['authority']}]" if m["authority"] else ""
        text = m["text"][:600] if m["text"] else "(no text, attachment only)"
        lines.append(f"[{m['author']}]{auth}{att_note}: {text}")
    return "\n".join(lines)


# ----- Prompts -----

TEXT_PROMPT = """You analyze a Discord conversation chunk from the PixelLab AI pixel-art community.

Extract ONLY high-signal info useful for building a real PixelLab workflow.

## Strict definitions (be ruthless)

- **PROMPT** = an actual generation prompt sent to PixelLab. Multi-word descriptive
  text that names subject, style, palette, pose, etc. Examples that ARE prompts:
  "A pixel knight in plate armor, top-down 3/4, 64x64". Examples that are NOT
  prompts: "nice!", "ty", "much to do", "looks great", emoji-only, vague phrases,
  status updates. WHEN UNSURE, DROP IT.
- **UI SETTING** = a concrete UI value with name + value. Capture VERBATIM:
  "Camera View: Side", "Generation Mode: Pro Standard", "Output Size: 128x128",
  "Padding H: 60%". Vague references like "use pro mode" without naming the
  exact setting do not count.
- **PROBLEM + SOLUTION** = a specific failure mode + a specific fix. Both halves
  required. "Animation broke" alone = drop. "Quadruped validator rejected my
  request, switched to template mode" = keep.
- **TIP / TRICK** = reproducible technique. Must include enough detail to repeat.
- **COOL TECHNIQUE** = unexpected combo or workflow with concrete steps.
- **WARNING** = a documented limitation or footgun.

## Authority handling

Messages tagged [AUTHORITY: <role>] are PixelLab staff/devs. ALWAYS preserve a
**verbatim quote** of authority statements (truncate to <=200 chars if long),
not just a paraphrase. Use this format:

> **[Authority: <role>] <username>:** "<verbatim quote>"

If the authority statement is on a topic above (Problem, Tip, etc.), put both
the verbatim quote AND a one-line takeaway under that section.

## Output

Structured markdown. Section headers ONLY when that section has real content.
If the chunk has nothing of value, output exactly: `(nothing notable)`

CHUNK:
{chunk}
"""

SYNTHESIS_PROMPT = """Merge multiple chunk-level digests into a single channel digest.

## Rules

1. **Deduplicate aggressively.** Same prompt/setting/tip across chunks = list ONCE.
   If something repeats, append `(seen Nx)` after it.
2. **Preserve verbatim authority quotes.** Every `**[Authority: ...]**` quote
   from chunks must survive into the final digest, with original quote text.
   Do not paraphrase authority quotes.
3. **Group by theme**: `## Prompts` / `## UI Settings` / `## Problems & Solutions`
   / `## Tips` / `## Cool Techniques` / `## Warnings` / `## From Authority`.
4. **From Authority section** = collect every authority quote from chunks here,
   even if also mentioned in another section. This section is the single source
   of truth for what PixelLab staff said.
5. **Cut noise.** Drop pure social chatter, status updates, "thanks", and any
   "Prompt" entries that fail the strict definition (must be multi-word
   descriptive generation text).
6. **Section omission**: if a section has zero items after dedup, omit it entirely.

CHUNK DIGESTS:
{digests}

Output clean markdown only. No preamble, no explanation, no closing remarks.
"""

EXECUTIVE_SUMMARY_PROMPT = """You are summarizing a multi-channel Discord digest
for a game developer building a PixelLab-based pixel-art pipeline. The reader
will spend SECONDS on this. Be brutally short and high-signal.

## Output exactly these sections

# Executive Summary

## Top Authority Takeaways
3-7 bullets. Each = one verbatim authority quote (truncated to <=180 chars) +
one short takeaway in parentheses. Author + role in bold prefix.
Example: **Kaninen (developer):** "API has skeletons, MCP doesn't" (use REST API
when MCP feature-poor).

## Recurring Workflow Patterns
3-7 bullets. Patterns mentioned in MULTIPLE channels. One-liner each.

## Concrete UI Settings Mentioned
List every verbatim UI setting value found, deduplicated.
Format: `Setting Name: value` — one per line.

## Confirmed Bugs / Limitations
3-6 bullets. Specific failure modes and what triggers them.

## Useful Prompts (verbatim, max 5)
Only the BEST verbatim prompts that are actually descriptive enough to be
reusable. Skip vague ones.

## Skip / Avoid
Things the community confirmed don't work.

## Source channels with most signal
List 3-5 channel names ranked by usefulness density.

INPUT (full per-channel digest):
{digests}

Output ONLY the summary in the format above. No preamble.
"""

VISION_PROMPT = """You analyze an image shared in the PixelLab AI Discord.

Surrounding chat context:
{context}

Output ONLY what helps a PixelLab user. No fluff, 2-5 lines max.

## Image type → what to extract

- **Sprite output:** subject + camera angle (top-down/side/3-quarter) + canvas
  size if guessable + visible style traits (palette family, outline weight,
  shading style) + obvious quality issues if any (muddy / asymmetric / broken
  outline / good).
- **UI screenshot:** list every visible setting verbatim with its value, e.g.
  `Camera View: High Top-Down`, `Generation Mode: Pro Standard`, `Output: 128x128`,
  `Padding H: 50% V: 20%`. If a dropdown is open, list the active value.
- **Failure / before-after / comparison:** describe specifically what differs.
- **Sprite-sheet / grid:** number of frames, layout (rows x cols), animation
  type if guessable.

If the image is irrelevant (meme, off-topic, social): output exactly `(irrelevant)`.
"""


# ----- Phases -----

def phase_text(label, msgs, digest_dir, text_model):
    non_bot = [m for m in msgs if not m["is_bot"]]
    auth_msgs = [m for m in non_bot if m["authority"]]
    out_path = digest_dir / f"{label}_text.md"
    if out_path.exists():
        print(f"  [text] skip - already done ({out_path.name})")
        return out_path, auth_msgs
    if not non_bot:
        print(f"  [text] no human messages")
        return None, []
    print(f"  [text] {len(non_bot)} human msgs ({len(auth_msgs)} authority) -> chunks of {CHUNK_SIZE}")

    chunk_digests = []
    total = (len(non_bot) + CHUNK_SIZE - 1) // CHUNK_SIZE
    for i, chunk in enumerate(chunk_messages(non_bot)):
        body = format_chunk(chunk)
        if not body.strip():
            continue
        try:
            digest = ollama_call(text_model, TEXT_PROMPT.format(chunk=body))
        except Exception as e:
            print(f"    chunk {i + 1}/{total}: ERR {e}")
            continue
        digest = digest.strip()
        if digest and "(nothing notable)" not in digest.lower():
            chunk_digests.append(digest)
        print(f"    chunk {i + 1}/{total} OK")

    if not chunk_digests:
        print(f"  [text] {label}: no notable content extracted")
        return None, auth_msgs

    print(f"  [text] synthesizing {len(chunk_digests)} chunk digests")
    try:
        final = ollama_call(text_model, SYNTHESIS_PROMPT.format(digests="\n\n---\n\n".join(chunk_digests)))
    except Exception as e:
        print(f"  [text] synthesis ERR {e}; concatenating raw")
        final = "\n\n---\n\n".join(chunk_digests)

    out_path = digest_dir / f"{label}_text.md"
    out_path.write_text(f"# {label} - Text Digest\n\n{final}\n", encoding="utf-8")
    print(f"  [text] -> {out_path.name}")
    return out_path, auth_msgs


def encode_image(path):
    with open(path, "rb") as f:
        return base64.b64encode(f.read()).decode("ascii")


def find_context(msgs, msg_id, window=6):
    for i, m in enumerate(msgs):
        if m["id"] == msg_id:
            lo = max(0, i - window)
            hi = min(len(msgs), i + window + 1)
            lines = []
            for x in msgs[lo:hi]:
                if x["text"]:
                    body = x["text"][:200]
                elif x["attachments"]:
                    names = ", ".join((a.get("fileName") or "?")[:30] for a in x["attachments"][:2])
                    body = f"(attachment only: {names})"
                else:
                    continue
                auth = " AUTHORITY" if x["authority"] else ""
                lines.append(f"[{x['author']}{auth}]: {body}")
            return "\n".join(lines)
    return ""


def find_media(media_dir, fname):
    if not media_dir.exists() or not fname:
        return None
    # Try exact match first
    matches = list(media_dir.rglob(fname))
    if matches:
        return matches[0]
    # DCE renames downloaded files as "<stem>-<16hex>.<ext>" — try prefix match
    stem = Path(fname).stem
    ext = Path(fname).suffix
    if stem and ext:
        matches = list(media_dir.rglob(f"{stem}-*{ext}"))
        if matches:
            return matches[0]
    # Fallback: any file containing the stem
    if stem:
        matches = [p for p in media_dir.rglob(f"*{stem}*") if p.is_file()]
        return matches[0] if matches else None
    return None


def phase_images(label, msgs, media_dir, digest_dir, vision_model, limit=None):
    out_path = digest_dir / f"{label}_images.md"
    if out_path.exists():
        print(f"  [image] skip - already done ({out_path.name})")
        return out_path
    items = []
    for m in msgs:
        if m["is_bot"]:
            continue
        for a in m["attachments"]:
            ext = Path(a.get("fileName") or "").suffix.lower()
            if ext in IMAGE_EXTS:
                items.append((m, a))

    if not items:
        return None
    # Authority-first ordering
    items.sort(key=lambda t: (0 if t[0]["authority"] else 1))
    if limit:
        items = items[:limit]
    print(f"  [image] {len(items)} candidates (authority-first)")

    blocks = []
    for i, (m, att) in enumerate(items):
        fname = att.get("fileName") or "?"
        path = find_media(media_dir, fname)
        if not path:
            continue
        try:
            b64 = encode_image(path)
            ctx = find_context(msgs, m["id"])
            desc = ollama_call(vision_model, VISION_PROMPT.format(context=ctx or "(no nearby text)"), images=[b64])
        except Exception as e:
            print(f"    img {i + 1}/{len(items)}: ERR {e}")
            continue
        desc = desc.strip()
        if "(irrelevant)" in desc.lower() or not desc:
            print(f"    img {i + 1}/{len(items)} skip (irrelevant)")
            continue
        auth_tag = f" **[Authority: {m['authority']}]**" if m["authority"] else ""
        blocks.append(f"### {fname}{auth_tag}\n\n_Context:_\n```\n{ctx[:400]}\n```\n\n{desc}\n")
        print(f"    img {i + 1}/{len(items)} OK")

    if not blocks:
        return None
    out_path = digest_dir / f"{label}_images.md"
    out_path.write_text(f"# {label} - Image Digest\n\n" + "\n---\n\n".join(blocks), encoding="utf-8")
    print(f"  [image] -> {out_path.name}")
    return out_path


def extract_keyframes(video_path, interval=KEYFRAME_INTERVAL, max_frames=MAX_FRAMES_PER_VIDEO):
    out_dir = video_path.parent / f".{video_path.stem}_frames"
    out_dir.mkdir(exist_ok=True)
    pattern = str(out_dir / "frame_%03d.jpg")
    cmd = ["ffmpeg", "-y", "-loglevel", "error", "-i", str(video_path),
           "-vf", f"fps=1/{interval}", "-frames:v", str(max_frames),
           "-q:v", "3", pattern]
    subprocess.run(cmd, check=True, capture_output=True)
    return sorted(out_dir.glob("frame_*.jpg"))


def phase_videos(label, msgs, media_dir, digest_dir, vision_model, limit=None):
    out_path = digest_dir / f"{label}_videos.md"
    if out_path.exists():
        print(f"  [video] skip - already done ({out_path.name})")
        return out_path
    if not shutil.which("ffmpeg"):
        print(f"  [video] ffmpeg yok -> video phase atlandi (kur: https://ffmpeg.org)")
        return None

    items = []
    for m in msgs:
        if m["is_bot"]:
            continue
        for a in m["attachments"]:
            ext = Path(a.get("fileName") or "").suffix.lower()
            if ext in VIDEO_EXTS:
                items.append((m, a))

    if not items:
        return None
    items.sort(key=lambda t: (0 if t[0]["authority"] else 1))
    if limit:
        items = items[:limit]
    print(f"  [video] {len(items)} candidates (authority-first)")

    blocks = []
    for i, (m, att) in enumerate(items):
        fname = att.get("fileName") or "?"
        path = find_media(media_dir, fname)
        if not path:
            continue
        try:
            frames = extract_keyframes(path)
        except subprocess.CalledProcessError as e:
            print(f"    vid {i + 1}/{len(items)}: ffmpeg ERR")
            continue

        ctx = find_context(msgs, m["id"])
        frame_descs = []
        for j, fr in enumerate(frames):
            try:
                b64 = encode_image(fr)
                desc = ollama_call(vision_model, VISION_PROMPT.format(context=ctx or "(no nearby text)"), images=[b64])
                desc = desc.strip()
                if desc and "(irrelevant)" not in desc.lower():
                    frame_descs.append((j, desc))
            except Exception as e:
                print(f"      frame {j}: ERR {e}")
        if not frame_descs:
            print(f"    vid {i + 1}/{len(items)} skip (no useful frames)")
            continue

        auth_tag = f" **[Authority: {m['authority']}]**" if m["authority"] else ""
        body = "\n\n".join(f"_Frame {j} (~{j * KEYFRAME_INTERVAL}s):_ {d}" for j, d in frame_descs)
        blocks.append(f"### {fname}{auth_tag}\n\n_Context:_\n```\n{ctx[:400]}\n```\n\n{body}\n")
        print(f"    vid {i + 1}/{len(items)} OK ({len(frame_descs)} frames)")

    if not blocks:
        return None
    out_path = digest_dir / f"{label}_videos.md"
    out_path.write_text(f"# {label} - Video Digest\n\n" + "\n---\n\n".join(blocks), encoding="utf-8")
    print(f"  [video] -> {out_path.name}")
    return out_path


# ----- Master -----

def write_master(digest_dir, channel_outputs, all_authority):
    master = digest_dir / "MASTER.md"
    with master.open("w", encoding="utf-8") as f:
        f.write("# PixelLab Discord - Master Digest\n\n")
        f.write("Phases run, per channel, alphabetical.\n\n")

        if all_authority:
            f.write("## Cross-Channel Authority Index\n\n")
            f.write("Authority-tagged users seen across all channels (deduplicated):\n\n")
            seen = {}
            for label, msgs in all_authority:
                for m in msgs:
                    key = (m["author"], m["authority"])
                    seen.setdefault(key, []).append(label)
            for (author, role), chans in sorted(seen.items(), key=lambda x: -len(x[1])):
                f.write(f"- **{author}** (`{role}`) - {len(chans)} msgs across {len(set(chans))} channels\n")
            f.write("\n---\n\n")

        for label, paths in channel_outputs:
            f.write(f"## {label}\n\n")
            for kind, p in paths:
                f.write(f"### [{kind}] {p.name}\n\n")
                f.write(p.read_text(encoding="utf-8"))
                f.write("\n\n")
            f.write("---\n\n")
    print(f"\nMASTER -> {master}")
    return master


def write_executive_summary(digest_dir, channel_outputs, text_model):
    """Final stage: collapse all per-channel digests into a Claude-friendly
    short executive summary. Reader spends seconds, not minutes."""
    chunks = []
    for label, paths in channel_outputs:
        for kind, p in paths:
            try:
                content = p.read_text(encoding="utf-8")
            except Exception:
                continue
            chunks.append(f"\n\n# Channel: {label} ({kind})\n\n{content}")

    if not chunks:
        return None

    combined = "".join(chunks)
    # Cap input size so we don't blow context. qwen2.5:14b has ~32k tokens; we
    # leave room for output. Roughly 90k chars upper bound for input.
    cap = 90000
    if len(combined) > cap:
        print(f"  [exec] input {len(combined)} chars > cap {cap}, truncating tail")
        combined = combined[:cap] + "\n\n[... truncated for length ...]"

    print(f"  [exec] synthesizing executive summary ({len(combined)} chars input)")
    try:
        summary = ollama_call(text_model, EXECUTIVE_SUMMARY_PROMPT.format(digests=combined))
    except Exception as e:
        print(f"  [exec] ERR {e}")
        return None

    out_path = digest_dir / "for_claude.md"
    out_path.write_text(summary.strip() + "\n", encoding="utf-8")
    print(f"  [exec] -> {out_path.name}  (Claude reads this; MASTER is fallback)")
    return out_path


# ----- Main -----

def main():
    ap = argparse.ArgumentParser(description="PixelLab Discord export analyzer")
    ap.add_argument("--export-dir", default=str(SCRIPT_DIR / ".." / ".." / "STAGING" / "discord_pixellab"))
    ap.add_argument("--with-images", action="store_true", help="vision phase on images")
    ap.add_argument("--with-videos", action="store_true", help="vision phase on video keyframes (needs ffmpeg)")
    ap.add_argument("--image-limit", type=int, default=None, help="cap images per channel")
    ap.add_argument("--video-limit", type=int, default=None, help="cap videos per channel")
    ap.add_argument("--text-model", default=DEFAULT_TEXT_MODEL)
    ap.add_argument("--vision-model", default=DEFAULT_VISION_MODEL)
    args = ap.parse_args()

    ollama_check(args.text_model)

    export_dir = Path(args.export_dir).resolve()
    digest_dir = export_dir / "digest"
    digest_dir.mkdir(parents=True, exist_ok=True)

    json_files = sorted(export_dir.glob("*.json"))
    if not json_files:
        print(f"ERROR: {export_dir} altinda *.json yok. Once export.ps1 calistir.")
        sys.exit(1)

    auth_patterns = load_authority_patterns()
    print(f"Channels: {len(json_files)}")
    print(f"Text model: {args.text_model}")
    if args.with_images or args.with_videos:
        print(f"Vision model: {args.vision_model}")
    print(f"Authority patterns: {len(auth_patterns)} ({', '.join(auth_patterns[:5])}...)")
    print(f"Output: {digest_dir}")

    channel_outputs = []
    all_authority = []

    for jp in json_files:
        label = jp.stem
        media_dir = export_dir / f"{label}_media"
        print(f"\n[{label}]")
        msgs = parse_export(jp, auth_patterns)
        print(f"  {len(msgs)} msgs (incl bots)")

        outs = []
        text_path, auth_msgs = phase_text(label, msgs, digest_dir, args.text_model)
        if text_path:
            outs.append(("text", text_path))
        if auth_msgs:
            all_authority.append((label, auth_msgs))

        if args.with_images:
            p = phase_images(label, msgs, media_dir, digest_dir, args.vision_model, limit=args.image_limit)
            if p:
                outs.append(("images", p))
        if args.with_videos:
            p = phase_videos(label, msgs, media_dir, digest_dir, args.vision_model, limit=args.video_limit)
            if p:
                outs.append(("videos", p))

        if outs:
            channel_outputs.append((label, outs))

    if channel_outputs:
        write_master(digest_dir, channel_outputs, all_authority)
        print()
        write_executive_summary(digest_dir, channel_outputs, args.text_model)
    else:
        print("\nHic digest uretilemedi.")


if __name__ == "__main__":
    main()
