import json
import glob
import os

all_text = []

for file_path in glob.glob("subs_*.json3"):
    try:
        with open(file_path, "r", encoding="utf-8") as f:
            data = json.load(f)
            
        video_id = file_path.split("_")[1].split(".")[0]
        text_parts = []
        
        # In json3, events contain the text segments
        if "events" in data:
            for event in data["events"]:
                if "segs" in event:
                    for seg in event["segs"]:
                        if "utf8" in seg:
                            text_parts.append(seg["utf8"].replace("\n", " "))
        
        full_text = "".join(text_parts).strip()
        if full_text:
            all_text.append(f"--- VIDEO ID: {video_id} ---\n{full_text}\n")
    except Exception as e:
        print(f"Failed to parse {file_path}: {e}")

with open("parsed_transcripts.txt", "w", encoding="utf-8") as f:
    f.write("\n".join(all_text))

print(f"Successfully parsed {len(all_text)} transcripts.")
