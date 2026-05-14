import sys
import re

try:
    from youtube_transcript_api import YouTubeTranscriptApi
except ImportError:
    import subprocess
    subprocess.check_call([sys.executable, "-m", "pip", "install", "youtube-transcript-api"])
    from youtube_transcript_api import YouTubeTranscriptApi

if len(sys.argv) < 2:
    print("Usage: python fetch_transcript.py <url>")
    sys.exit(1)

url = sys.argv[1]
match = re.search(r'(?:v=|youtu\.be/|shorts/)([\w-]+)', url)
if match:
    vid = match.group(1)
    try:
        # Check if we have multiple languages
        try:
            transcript = YouTubeTranscriptApi.get_transcript(vid, languages=['en', 'tr', 'en-US', 'en-GB'])
        except Exception:
            transcript = YouTubeTranscriptApi.get_transcript(vid)
            
        print(f"--- Transcript for {vid} ---")
        for entry in transcript:
            print(f"[{entry['start']:.2f}] {entry['text']}")
    except Exception as e:
        print(f"Transcript Error: {e}")
else:
    print("Invalid URL")
