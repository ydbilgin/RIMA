import json
from youtube_transcript_api import YouTubeTranscriptApi

try:
    transcript = YouTubeTranscriptApi.get_transcript('moCpjMOOBGk')
    with open('vid_transcript.txt', 'w', encoding='utf-8') as f:
        for entry in transcript:
            f.write(f"[{entry['start']:.2f}] {entry['text']}\n")
    print("Transcript saved.")
except Exception as e:
    print(f"Error: {e}")
