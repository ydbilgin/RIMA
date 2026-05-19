import requests
import re

urls = [
    'https://kenney.nl/assets/tiny-dungeon',
    'https://kenney.nl/assets/tiny-town',
    'https://kenney.nl/assets/roguelike-dungeon' # guess
]

for u in urls:
    try:
        text = requests.get(u, headers={'User-Agent': 'Mozilla/5.0'}).text
        matches = re.findall(r'href=[\'"]?(https://(?:[a-zA-Z0-9-]+\.)*kenney\.nl/.*?\.zip)[\'"]?', text)
        print(f"{u}: {matches}")
    except Exception as e:
        print(e)
