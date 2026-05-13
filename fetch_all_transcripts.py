import json
from youtube_transcript_api import YouTubeTranscriptApi

videos = {
    "moCpjMOOBGk": "GBA-Style Sprites",
    "Hhx9QZwYoZY": "Object Creator",
    "sMV94of38ck": "Game Jam Workflow",
    "o8AZRTx36DE": "Side-Scroller Asset Pack",
    "1FWEXTiJnlc": "Animated Pixel Art Scenes",
    "1CjxHZoZE_I": "Animate Between 2 Frames",
    "zghUW8fGqsM": "Pixel Animations Tool",
    "8TRHAC3fUpo": "Walking Animations",
    "qVDkp1baJkU": "Interior Maps for Top-Down Games",
    "84yChPoOaew": "Side Scroller Level",
    "up2hU_lzQsA": "Pixel Art PNGTuber",
    "0SQRclReGo4": "Destructible Environments",
    "T4by1uEXuE4": "Isometric Animals",
    "7HTLYLo3tTQ": "Cohesive Game Art Style"
}

all_transcripts = {}

for vid, title in videos.items():
    try:
        transcript = YouTubeTranscriptApi.get_transcript(vid)
        text = " ".join([entry['text'] for entry in transcript])
        all_transcripts[title] = text
        print(f"Fetched: {title}")
    except Exception as e:
        print(f"Failed to fetch {title}: {e}")

with open("all_transcripts.json", "w", encoding="utf-8") as f:
    json.dump(all_transcripts, f, ensure_ascii=False, indent=2)
