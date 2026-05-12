import os
import requests
from bs4 import BeautifulSoup
import markdownify
import re

output_dir = "PixelLabDocs"
os.makedirs(output_dir, exist_ok=True)

base_url = "https://www.pixellab.ai"
docs_url = "https://www.pixellab.ai/docs"

response = requests.get(docs_url)
soup = BeautifulSoup(response.text, "html.parser")

links = []
for a in soup.find_all("a", href=True):
    href = a["href"]
    if href.startswith("/docs") or href.startswith("https://www.pixellab.ai/docs"):
        if href.startswith("/"):
            href = base_url + href
        # Ensure it doesn't have fragments
        href = href.split("#")[0]
        if href not in links:
            links.append(href)

links.append("https://api.pixellab.ai/mcp/docs")

# Remove duplicates
links = list(set(links))

print(f"Found {len(links)} links. Downloading...")

for i, link in enumerate(links):
    try:
        res = requests.get(link)
        if res.status_code == 200:
            # We want to extract just the main content if possible, but markdownify the whole body is okay.
            page_soup = BeautifulSoup(res.text, "html.parser")
            # Usually the main content is in a main or article tag, or div with id/class main
            main_content = page_soup.find("main")
            if not main_content:
                 main_content = page_soup.body if page_soup.body else page_soup
                 
            md = markdownify.markdownify(str(main_content), heading_style="ATX")
            
            # create filename from url
            parts = link.rstrip("/").split("/")
            filename = parts[-1]
            if filename == "docs":
                if "api.pixellab.ai/mcp" in link:
                    filename = "mcp_docs"
                else:
                    filename = "index"
            
            # clean filename
            filename = re.sub(r'[^a-zA-Z0-9_\-]', '', filename)
            if not filename:
                filename = f"page_{i}"
                
            filepath = os.path.join(output_dir, filename + ".md")
            
            # if exists, append random
            counter = 1
            original_filename = filename
            while os.path.exists(filepath):
                 filepath = os.path.join(output_dir, f"{original_filename}_{counter}.md")
                 counter += 1
                 
            with open(filepath, "w", encoding="utf-8") as f:
                f.write(f"Source: {link}\n\n")
                f.write(md)
            print(f"[{i+1}/{len(links)}] Saved {filepath}")
        else:
            print(f"[{i+1}/{len(links)}] Failed to fetch {link} - Status: {res.status_code}")
    except Exception as e:
        print(f"[{i+1}/{len(links)}] Error fetching {link}: {e}")

print("Done!")
